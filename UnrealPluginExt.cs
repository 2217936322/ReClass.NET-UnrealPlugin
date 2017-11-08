using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using ReClassNET;
using ReClassNET.Memory;
using ReClassNET.MemoryScanner;
using ReClassNET.Nodes;
using ReClassNET.Plugins;
using ReClassNET.Util;
using UnrealPlugin.Config;
using UnrealPlugin.NameResolver;
using System.IO;
using System.Windows.Forms;
using ReClassNET.Forms;
using ReClassNET.UI;
using UnrealPlugin.UI;

namespace UnrealPlugin
{
	public class UnrealPluginExt : Plugin, INodeInfoReader
	{
		private readonly Dictionary<string, UnrealApplicationSettings> applicationSettings = new Dictionary<string, UnrealApplicationSettings>();

		private IPluginHost host;
		
		private INameResolver resolver;

		public override Image Icon => Properties.Resources.icon;

		public override bool Initialize(IPluginHost host)
		{
			//System.Diagnostics.Debugger.Launch();

			if (this.host != null)
			{
				Terminate();
			}

			this.host = host ?? throw new ArgumentNullException(nameof(host));

			// Register the InfoReader
			host.RegisterNodeInfoReader(this);

			// Register ProcessAttached handler
			host.Process.ProcessAttached += OnProcessAttached;

			GlobalWindowManager.WindowAdded += WindowAddedHandler;

			return true;
		}

		public override void Terminate()
		{
			GlobalWindowManager.WindowAdded -= WindowAddedHandler;

			host.DeregisterNodeInfoReader(this);
		}

		private void WindowAddedHandler(object sender, GlobalWindowManagerEventArgs e)
		{
			if (e.Form is SettingsForm settingsForm)
			{
				settingsForm.Shown += delegate
				{
					try
					{
						var imageIndex = settingsForm.SettingsTabControl.ImageList.Images.Add(Properties.Resources.icon, Color.Transparent);

						var newTab = new TabPage("Unreal Engine");
						newTab.UseVisualStyleBackColor = true;
						newTab.ImageIndex = imageIndex;

						var optionsPanel = new SettingsPanel(this);
						newTab.Controls.Add(optionsPanel);
						optionsPanel.Dock = DockStyle.Fill;

						settingsForm.SettingsTabControl.TabPages.Add(newTab);
					}
					catch (Exception ex)
					{
						Debug.Fail(ex.ToString());
					}
				};
			}
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

			var processName = Path.GetFileName(process.UnderlayingProcess.Path).ToLower();

			if (applicationSettings.TryGetValue(processName, out var settings))
			{
				var namesArrayPtr = FindPattern(process, process.GetModuleByName(settings.PatternModule), settings.GlobalNamesArrayPattern);

				if (!namesArrayPtr.IsNull())
				{
					switch (settings.Version)
					{
						case UnrealEngineVersion.UE1:
							resolver = new UnrealEngine1NameResolver(process, new UnrealEngine1Config
							{

							});
							break;
						case UnrealEngineVersion.UE2:
							resolver = new UnrealEngine2NameResolver(process, new UnrealEngine2Config
							{

							});
							break;
						case UnrealEngineVersion.UE3:
							resolver = new UnrealEngine3NameResolver(process, new UnrealEngine3Config
							{

							});
							break;
						case UnrealEngineVersion.UE4:
							resolver = new UnrealEngine4NameResolver(process, new UnrealEngine4Config
							{

							});
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
				}
			}

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
				/*case "udk.exe":
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
					break;*/
				/*case "xiii.exe":
					var config = new UnrealEngine2Config
					{
						FNameEntryIndexOffset = 0x00,
						FNameEntryNameDataOffset = 0x0D,
						UObjectNameOffset = 0x20
					};

					var pattern = "A1 ?? ?? ?? ?? 8B 88";
					var address = FindPattern(process, process.GetModuleByName("core.dll"), pattern);

					if (!address.IsNull())
					{
						config.GlobalArrayPtr = process.ReadRemoteIntPtr(address + 1);

						resolver = new UnrealEngine2NameResolver(process, config);
					}
					break;*/
				/*case "ut2004.exe":
					var config = new UnrealEngine2Config
					{
						FNameEntryIndexOffset = 0x00,
						FNameEntryNameDataOffset = 0x0C,
						FNameEntryIsWide = true,
						UObjectNameOffset = 0x24
					};

					var pattern = "A1 ?? ?? ?? ?? 8B 88";
					var address = FindPattern(process, process.GetModuleByName("core.dll"), pattern);

					if (!address.IsNull())
					{
						config.GlobalArrayPtr = process.ReadRemoteIntPtr(address + 1);

						resolver = new UnrealEngine2NameResolver(process, config);
					}
					break;*/
				case "deusex.exe":
					var config = new UnrealEngine1Config
					{
						FNameEntryIndexOffset = 0x00,
						FNameEntryNameDataOffset = 0x0C,
						FNameEntryIsWide = true,
						UObjectNameOffset = 0x20
					};

					var pattern = "A1 ?? ?? ?? ?? 8B 88";
					var address = FindPattern(process, process.GetModuleByName("core.dll"), pattern);

					if (!address.IsNull())
					{
						config.GlobalArrayPtr = process.ReadRemoteIntPtr(address + 1);

						resolver = new UnrealEngine1NameResolver(process, config);
					}
					break;
			}
		}

		public string ReadNodeInfo(BaseNode node, IntPtr value, MemoryBuffer memory)
		{
			return resolver?.ReadNameOfObject(value);
		}
	}
}
