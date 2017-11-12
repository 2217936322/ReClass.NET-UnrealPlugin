using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using ReClassNET.Forms;
using ReClassNET.Memory;
using ReClassNET.MemoryScanner;
using ReClassNET.Nodes;
using ReClassNET.Plugins;
using ReClassNET.UI;
using ReClassNET.Util;
using UnrealPlugin.Config;
using UnrealPlugin.NameResolver;
using UnrealPlugin.UI;

namespace UnrealPlugin
{
	public class UnrealPluginExt : Plugin, INodeInfoReader
	{
		private const string ConfigApplicationsKey = "UnrealPlugin.Applications";

		private IPluginHost host;
		
		private INameResolver resolver;

		public override Image Icon => Properties.Resources.B16x16_Icon;

		public List<UnrealApplicationSettings> Applications { get; } = new List<UnrealApplicationSettings>()
		{
			new UnrealApplicationSettings
			{
				Name = "Playerunknown's Battlegrounds",
				Version = UnrealEngineVersion.UE4,
				Platform = Platform.x64,
				ProcessName = "tslgame.exe",
				PatternMethod = PatternMethod.InstructionAddressPlusOffset,
				Pattern = "48 89 1D ?? ?? ?? ?? 48 8B 5C 24 ?? 48 83 C4 28 C3 48 8B 5C 24 ?? 48 89 05 ?? ?? ?? ?? 48 83 C4 28 C3",
				PatternOffset = 3,
				UObjectNameOffset = 4,
				FNameEntryIndexOffset = 0,
				FNameEntryNameDataOffset = 0xC
			},
			new UnrealApplicationSettings
			{
				Name = "Caffeine",
				Version = UnrealEngineVersion.UE3,
				Platform = Platform.x86,
				ProcessName = "udk.exe",
				PatternMethod = PatternMethod.Direct,
				Pattern = "8B 0D ?? ?? ?? ?? 83 3C 81 00 74",
				PatternOffset = 2,
				UObjectNameOffset = 0x2C,
				FNameEntryIndexOffset = 8,
				FNameEntryNameDataOffset = 0x10
			},
			new UnrealApplicationSettings
			{
				Name = "XIII",
				Version = UnrealEngineVersion.UE2,
				Platform = Platform.x86,
				ProcessName = "xiii.exe",
				PatternMethod = PatternMethod.Direct,
				Pattern = "A1 ?? ?? ?? ?? 8B 88",
				PatternOffset = 1,
				UObjectNameOffset = 0x20,
				FNameEntryIndexOffset = 0,
				FNameEntryNameDataOffset = 0xD
			},
			new UnrealApplicationSettings
			{
				Name = "Unreal Tournament 2004",
				Version = UnrealEngineVersion.UE2,
				Platform = Platform.x86,
				ProcessName = "xiii.exe",
				PatternMethod = PatternMethod.Direct,
				PatternModule = "core.dll",
				Pattern = "A1 ?? ?? ?? ?? 8B 88",
				PatternOffset = 1,
				UObjectNameOffset = 0x24,
				FNameEntryIndexOffset = 0,
				FNameEntryNameDataOffset = 0xC,
				FNameEntryIsWide = true
			},
			new UnrealApplicationSettings
			{
				Name = "Deus Ex",
				Version = UnrealEngineVersion.UE1,
				Platform = Platform.x86,
				ProcessName = "deusex.exe",
				PatternMethod = PatternMethod.Direct,
				PatternModule = "core.dll",
				Pattern = "A1 ?? ?? ?? ?? 8B 88",
				PatternOffset = 1,
				UObjectNameOffset = 0x20,
				FNameEntryIndexOffset = 0,
				FNameEntryNameDataOffset = 0xC,
				FNameEntryIsWide = true
			}
		};

		public override bool Initialize(IPluginHost host)
		{
			System.Diagnostics.Debugger.Launch();

			if (this.host != null)
			{
				Terminate();
			}

			this.host = host ?? throw new ArgumentNullException(nameof(host));

			var applications = host.Settings.CustomData.GetXElement(ConfigApplicationsKey, null);
			if (applications != null)
			{
				Applications.AddRange(applications.Elements().Select(UnrealApplicationSettings.FromXml));
			}

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

			host.Settings.CustomData.SetXElement(
				ConfigApplicationsKey,
				new XElement(
					"Applications",
					Applications.Select(UnrealApplicationSettings.ToXml)
				)
			);
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

						var newTab = new TabPage("Unreal Engine")
						{
							UseVisualStyleBackColor = true,
							ImageIndex = imageIndex
						};

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

		private void OnProcessAttached(RemoteProcess process)
		{
			process.UpdateProcessInformations();

			resolver = null;

			var processName = Path.GetFileName(process.UnderlayingProcess.Path).ToLower();

			var settings = Applications.FirstOrDefault(s => s.ProcessName == processName);
			if (settings != null)
			{
				var namesArrayPtr = FindPattern(process, process.GetModuleByName(settings.PatternModule), settings.Pattern);

				if (!namesArrayPtr.IsNull())
				{
					switch (settings.Version)
					{
						case UnrealEngineVersion.UE1:
							resolver = new UnrealEngine1NameResolver(process, new UnrealEngine1Config
							{
								GlobalArrayPtr = namesArrayPtr,
								UObjectNameOffset = settings.UObjectNameOffset,
								FNameEntryIndexOffset = settings.FNameEntryIndexOffset,
								FNameEntryNameDataOffset = settings.FNameEntryNameDataOffset,
								FNameEntryIsWide = settings.FNameEntryIsWide
							});
							break;
						case UnrealEngineVersion.UE2:
							resolver = new UnrealEngine2NameResolver(process, new UnrealEngine2Config
							{
								GlobalArrayPtr = namesArrayPtr,
								UObjectNameOffset = settings.UObjectNameOffset,
								FNameEntryIndexOffset = settings.FNameEntryIndexOffset,
								FNameEntryNameDataOffset = settings.FNameEntryNameDataOffset,
								FNameEntryIsWide = settings.FNameEntryIsWide
							});
							break;
						case UnrealEngineVersion.UE3:
							resolver = new UnrealEngine3NameResolver(process, new UnrealEngine3Config
							{
								GlobalArrayPtr = namesArrayPtr,
								UObjectNameOffset = settings.UObjectNameOffset,
								FNameEntryIndexOffset = settings.FNameEntryIndexOffset,
								FNameEntryNameDataOffset = settings.FNameEntryNameDataOffset
							});
							break;
						case UnrealEngineVersion.UE4:
							resolver = new UnrealEngine4NameResolver(process, new UnrealEngine4Config
							{
								GlobalArrayPtr = namesArrayPtr,
								UObjectNameOffset = settings.UObjectNameOffset,
								FNameEntryIndexOffset = settings.FNameEntryIndexOffset,
								FNameEntryNameDataOffset = settings.FNameEntryNameDataOffset
							});
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
				}
			}
		}

		public string ReadNodeInfo(BaseNode node, IntPtr value, MemoryBuffer memory)
		{
			return resolver?.ReadNameOfObject(value);
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
	}
}
