using System;
using System.Drawing;
using ReClassNET;
using ReClassNET.Memory;
using ReClassNET.Nodes;
using ReClassNET.Plugins;
using ReClassNET.Util;
using ReClassNET.MemoryScanner;

namespace UnrealPlugin
{
    public class UnrealPluginExt : Plugin, INodeInfoReader
    {
        private IPluginHost host;
        internal static Settings Settings;

        private IntPtr gNames;

        public override bool Initialize(IPluginHost host)
        {
            //System.Diagnostics.Debugger.Launch();

            if (this.host != null)
            {
                Terminate();
            }

            this.host = host ?? throw new ArgumentNullException(nameof(host));

            Settings = host.Settings;

            // Register the InfoReader
            host.RegisterNodeInfoReader(this);

            // Register ProcessAttached handler
            host.Process.ProcessAttached += OnProcessAttached;

            return true;
        }

        private IntPtr FindPattern(RemoteProcess process, Module module, string pattern)
        {
            // Read Module Bytes
            var moduleBytes = process.ReadRemoteMemory(module.Start, module.Size.ToInt32());

            // Parse Bytepattern
            var bytePattern = BytePattern.Parse(pattern);

            // Find Bytepattern in our copy
            for (int i = 0; i < moduleBytes.Length; i++)
            {
                if (bytePattern.Equals(moduleBytes, i))
                {
                    return module.Start.Add(new IntPtr(i));
                }
            }

            return new IntPtr(0);
        }

        private void OnProcessAttached(RemoteProcess process)
        {
            process.UpdateProcessInformations();

            var processName = System.IO.Path.GetFileName(process.UnderlayingProcess.Path).ToLower();

            switch (processName)
            {
                // TODO: Add more games

                case "tslgame.exe": // Playerunknown's Battlegrounds
                    {
                        var pattern = "48 89 1D ?? ?? ?? ?? 48 8B 5C 24 ?? 48 83 C4 28 C3 48 8B 5C 24 ?? 48 89 05 ?? ?? ?? ?? 48 83 C4 28 C3";
                        var address = FindPattern(process, process.GetModuleByName(processName), pattern);

                        if (!address.IsNull())
                        {
                            var offset = process.ReadRemoteObject<int>(address.Add(new IntPtr(0x3)));
                            gNames = process.ReadRemoteObject<IntPtr>(address.Add(new IntPtr(offset + 7)));
                        }
                        else
                        {
                            gNames = new IntPtr(0);
                        }

                        break;
                    }

                default:
                    {
                        gNames = new IntPtr(0);
                        return;
                    }
            }
        }

        public override void Terminate()
        {
            host.DeregisterNodeInfoReader(this);
        }

        public string ReadNodeInfo(BaseNode node, IntPtr value, MemoryBuffer memory)
        {
            if (!gNames.IsNull())
            {
#if RECLASSNET64
                int nameIndex = memory.Process.ReadRemoteObject<int>(value.Add(new IntPtr(0x18)));
#else
                int nameIndex = memory.Process.ReadRemoteObject<int>(value.Add(new IntPtr(0x10)));
#endif
                return ReadNameIndex(nameIndex, memory);
            }

            return null;
        }

        private string ReadNameIndex(int nameIndex, MemoryBuffer memory)
        {
            if (nameIndex < 1)
            {
                return null;
            }

            if (!gNames.IsNull())
            {
                int ptrsize = System.Runtime.InteropServices.Marshal.SizeOf<IntPtr>();

                int numElements = memory.Process.ReadRemoteObject<int>(gNames.Add(new IntPtr(0x80 * ptrsize)));
                int numChunks = memory.Process.ReadRemoteObject<int>(gNames.Add(new IntPtr(0x80 * ptrsize + 0x4)));

                int indexChunk = nameIndex / 16384;
                int indexName = nameIndex % 16384;

                if (nameIndex < numElements && indexChunk < numChunks)
                {
                    var chunkPtr = memory.Process.ReadRemoteObject<IntPtr>(gNames.Add(new IntPtr(indexChunk * ptrsize)));

                    if (chunkPtr.MayBeValid())
                    {
                        var namePtr = memory.Process.ReadRemoteObject<IntPtr>(chunkPtr.Add(new IntPtr(indexName * ptrsize)));

                        int nameEntryIndex = memory.Process.ReadRemoteObject<int>(namePtr);

                        if ((nameEntryIndex >> 1) == nameIndex)
                        {
                            bool wideChar = (nameEntryIndex & 1) != 0;

                            if (wideChar)
                            {
                                var name = memory.Process.ReadRemoteString(System.Text.Encoding.Unicode, namePtr.Add(new IntPtr(0x8 + ptrsize)), 1024);

                                return name;
                            }
                            else
                            {
                                var name = memory.Process.ReadRemoteString(System.Text.Encoding.ASCII, namePtr.Add(new IntPtr(0x8 + ptrsize)), 1024);

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
