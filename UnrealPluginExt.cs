using System;
using System.Text;
using ReClassNET;
using ReClassNET.Memory;
using ReClassNET.MemoryScanner;
using ReClassNET.Nodes;
using ReClassNET.Plugins;
using ReClassNET.Util;
using static System.IO.Path;

namespace UnrealPlugin
{
	public class UnrealPluginExt : Plugin, INodeInfoReader
	{
		private IPluginHost host;
		internal static Settings Settings;

		private IntPtr gNames;
        private IntPtr gObjects;

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

		private static IntPtr FindPattern(RemoteProcess process, Module module, string pattern)
		{
			// Read Module Bytes
			var moduleBytes = process.ReadRemoteMemory(module.Start, module.Size.ToInt32());

			// Parse Bytepattern
			var bytePattern = BytePattern.Parse(pattern);

			// Find Bytepattern in our copy
			var limit = moduleBytes.Length - bytePattern.Length;
			for (var i = 0; i < limit; ++i)
			{
				if (bytePattern.Equals(moduleBytes, i))
				{
					return module.Start + i;
				}
			}

			return IntPtr.Zero;
		}

		private void OnProcessAttached(RemoteProcess process)
		{
			process.UpdateProcessInformations();

			gNames = IntPtr.Zero;
            gObjects = IntPtr.Zero;

            var processName = GetFileName(process.UnderlayingProcess.Path).ToLower();
			switch (processName)
			{
                // TODO: Add more games
                
                case "tslgame.exe": // Playerunknown's Battlegrounds
                case "fortniteclient-win64-shipping.exe": // Fortnite
                    {
						var pattern = "48 89 1D ?? ?? ?? ?? 48 8B 5C 24 ?? 48 83 C4 28 C3 48 8B 5C 24 ?? 48 89 05 ?? ?? ?? ?? 48 83 C4 28 C3";
						var address = FindPattern(process, process.GetModuleByName(processName), pattern);

						if (!address.IsNull())
						{
							var offset = process.ReadRemoteInt32(address + 0x3);
							gNames = process.ReadRemoteIntPtr(address + offset + 7);
						}

                        pattern = "48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8B D6 48 89 B5";
                        address = FindPattern(process, process.GetModuleByName(processName), pattern);

                        if (!address.IsNull())
                        {
                            var offset = process.ReadRemoteInt32(address + 0x3);
                            gObjects = address + offset + 7;
                        }

                        break;
					}
			}
		}

		public override void Terminate()
		{
			host.DeregisterNodeInfoReader(this);
		}

		public string ReadNodeInfo(BaseNode node, IntPtr value, MemoryBuffer memory)
		{
            if (IsUObject(value, memory))
            {
                return GetUObjectName(value, memory);
            }

            return null;
		}

        private bool IsUObject(IntPtr objectPtr, MemoryBuffer memory)
        {
            if (gObjects.IsNull())
            {
                return false;
            }

            var internalIndex = memory.Process.ReadRemoteInt32(objectPtr + IntPtr.Size + 0x4);

            var numElements = memory.Process.ReadRemoteInt32(gObjects + 0x10 + IntPtr.Size + 0x4);

            if (internalIndex < 0 || internalIndex >= numElements) return false;

            var objects = memory.Process.ReadRemoteIntPtr(gObjects + 0x10);

#if RECLASSNET64
            var objectPtrCheck = memory.Process.ReadRemoteIntPtr(objects + internalIndex * 0x18);
#else
			var objectPtrCheck = memory.Process.ReadRemoteIntPtr(objects + internalIndex * 0x10);
#endif

            bool result = objectPtr == objectPtrCheck;

            return result;
        }

		private string GetUObjectName(IntPtr objectPtr, MemoryBuffer memory)
		{
			if (gNames.IsNull())
			{
				return null;
			}

#if RECLASSNET64
            var nameIndex = memory.Process.ReadRemoteInt32(objectPtr + 0x18);
#else
			var nameIndex = memory.Process.ReadRemoteInt32(objectPtr + 0x10);
#endif

            if (nameIndex < 1)
            {
                return null;
            }

            var numElements = memory.Process.ReadRemoteInt32(gNames + 0x80 * IntPtr.Size);
			var numChunks = memory.Process.ReadRemoteInt32(gNames + 0x80 * IntPtr.Size + 0x4);

			var indexChunk = nameIndex / 16384;
			var indexName = nameIndex % 16384;

			if (nameIndex < numElements && indexChunk < numChunks)
			{
				var chunkPtr = memory.Process.ReadRemoteIntPtr(gNames + indexChunk * IntPtr.Size);

				if (chunkPtr.MayBeValid())
				{
					var namePtr = memory.Process.ReadRemoteIntPtr(chunkPtr + indexName * IntPtr.Size);

					var nameEntryIndex = memory.Process.ReadRemoteInt32(namePtr);

					if (nameEntryIndex >> 1 == nameIndex)
					{
						var wideChar = (nameEntryIndex & 1) != 0;

						var name = memory.Process.ReadRemoteString(wideChar ? Encoding.Unicode : Encoding.ASCII, namePtr + 0x8 + IntPtr.Size, 1024);

						return name;
					}
				}
			}

			return null;
		}
	}
}
