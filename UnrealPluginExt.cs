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

			var processName = GetFileName(process.UnderlayingProcess.Path).ToLower();

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
				var nameIndex = memory.Process.ReadRemoteObject<int>(value + 0x18);
#else
				var nameIndex = memory.Process.ReadRemoteObject<int>(value + 0x10);
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
				var numElements = memory.Process.ReadRemoteObject<int>(gNames + 0x80 * IntPtr.Size);
				var numChunks = memory.Process.ReadRemoteObject<int>(gNames + 0x80 * IntPtr.Size + 0x4);

				var indexChunk = nameIndex / 16384;
				var indexName = nameIndex % 16384;

				if (nameIndex < numElements && indexChunk < numChunks)
				{
					var chunkPtr = memory.Process.ReadRemoteObject<IntPtr>(gNames + indexChunk * IntPtr.Size);

					if (chunkPtr.MayBeValid())
					{
						var namePtr = memory.Process.ReadRemoteObject<IntPtr>(chunkPtr + indexName * IntPtr.Size);

						var nameEntryIndex = memory.Process.ReadRemoteObject<int>(namePtr);

						if (nameEntryIndex >> 1 == nameIndex)
						{
							var wideChar = (nameEntryIndex & 1) != 0;

							var name = memory.Process.ReadRemoteString(wideChar ? Encoding.Unicode : Encoding.ASCII, namePtr + 0x8 + IntPtr.Size, 1024);

							return name;
						}
					}
				}
			}

			return null;
		}
	}
}
