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

		private INameResolver resolver;

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

			resolver = null;

			var processName = GetFileName(process.UnderlayingProcess.Path).ToLower();
			switch (processName)
			{
				// TODO: Add more games

				/*case "tslgame.exe": // Playerunknown's Battlegrounds
					{
						var pattern = "48 89 1D ?? ?? ?? ?? 48 8B 5C 24 ?? 48 83 C4 28 C3 48 8B 5C 24 ?? 48 89 05 ?? ?? ?? ?? 48 83 C4 28 C3";
						var address = FindPattern(process, process.GetModuleByName(processName), pattern);

						if (!address.IsNull())
						{
							var offset = process.ReadRemoteInt32(address + 0x3);
							gNames = process.ReadRemoteIntPtr(address + offset + 7);
						}

						break;
					}*/
				case "udk.exe":
					var config = new UnrealEngine3Config
					{
						FNameEntryIndexOffset = 0x8,
						FNameEntryNameDataOffset = 0x10,
						UObjectNameOffset = 0x2C
					};

					var pattern = "8B 0D ?? ?? ?? ?? 83 3C 81 00 74";
					var address = FindPattern(process, process.GetModuleByName(processName), pattern);

					if (!address.IsNull())
					{
						config.GlobalArrayPtr = process.ReadRemoteIntPtr(address + 2);

						resolver = new UnrealEngine3NameResolver(process, config);
					}
					break;
			}
		}

		public override void Terminate()
		{
			host.DeregisterNodeInfoReader(this);
		}

		public string ReadNodeInfo(BaseNode node, IntPtr value, MemoryBuffer memory)
		{
			return resolver?.ReadNameOfObject(value);
		}
	}
}
