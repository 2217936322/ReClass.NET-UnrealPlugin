using System;
using System.Drawing;
using ReClassNET;
using ReClassNET.Memory;
using ReClassNET.Nodes;
using ReClassNET.Plugins;
using ReClassNET.Util;
using System.Runtime.InteropServices;

namespace UnrealPlugin
{
    public class UnrealPluginExt : Plugin
    {
        private IPluginHost host;

        internal static Settings Settings;

        private INodeInfoReader reader;

        public override bool Initialize(IPluginHost host)
        {
            System.Diagnostics.Debugger.Launch();

            if (this.host != null)
            {
                Terminate();
            }

            this.host = host ?? throw new ArgumentNullException(nameof(host));

            Settings = host.Settings;

            // Register the InfoReader
            reader = new FrostBiteNodeInfoReader();
            host.RegisterNodeInfoReader(reader);

            // Register ProcessAttached handler for caching all gnames
            host.Process.ProcessAttached += OnProcessAttached;

            return true;
        }

        private void OnProcessAttached(RemoteProcess sender)
        {
            //sender.UpdateProcessInformations();


            //todo: cache names array
        }

        public override void Terminate()
        {
            //host.DeregisterNodeInfoReader(reader);
            
            //TODO: Detect process name and initialize gNames offsets
        }
    }


    /// <summary>A custom node info reader which outputs Frostbite type infos.</summary>
    public class FrostBiteNodeInfoReader : INodeInfoReader
    {
        public string ReadNodeInfo(BaseNode node, IntPtr value, MemoryBuffer memory)
        {
#if RECLASSNET64
            int index = memory.Process.ReadRemoteObject<int>(value.Add(new IntPtr(0x18)));

            return ReadNameIndex(index, memory);
#else
            // TODO: add x86 support
            return null;
#endif
        }



        private string ReadNameIndex(int nameIndex, MemoryBuffer memory)
        {
            if (nameIndex < 1)
            {
                return null;
            }

            // TODO: Remove hardcoded offset & and use gNames offset initialized in OnProcessAttached
            var processModule = memory.Process.GetModuleByName(memory.Process.UnderlayingProcess.Name);
            var gNames = memory.Process.ReadRemoteObject<IntPtr>(processModule.Start.Add(new IntPtr(0x36E8790)));

            if (gNames.MayBeValid())
            {
                int numElements = memory.Process.ReadRemoteObject<int>(gNames.Add(new IntPtr(0x400)));
                int numChunks = memory.Process.ReadRemoteObject<int>(gNames.Add(new IntPtr(0x404)));

                int indexChunk = nameIndex / 16384;
                int indexName = nameIndex % 16384;

                if (nameIndex < numElements &&  indexChunk < numChunks)
                {
                    var chunkPtr = memory.Process.ReadRemoteObject<IntPtr>(gNames.Add(new IntPtr(indexChunk * 0x8)));

                    if (chunkPtr.MayBeValid())
                    {
                        var namePtr = memory.Process.ReadRemoteObject<IntPtr>(chunkPtr.Add(new IntPtr(indexName * 0x8)));

                        int nameEntryIndex = memory.Process.ReadRemoteObject<int>(namePtr);

                        if ((nameEntryIndex >> 1) == nameIndex)
                        {
                            bool wideChar = (nameEntryIndex & 1) != 0;

                            if (wideChar)
                            {
                                var name = memory.Process.ReadRemoteString(System.Text.Encoding.Unicode, namePtr.Add(new IntPtr(0x10)), 1024);

                                return name;
                            }
                            else
                            {
                                var name = memory.Process.ReadRemoteString(System.Text.Encoding.ASCII, namePtr.Add(new IntPtr(0x10)), 1024);

                                return name;
                            }
                        }
                    }
                }
            }

            return null;
        }
    }
}
