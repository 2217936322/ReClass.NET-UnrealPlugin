using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ReClassNET;
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
		private IPluginHost host;
		
		private INameResolver resolver;

		public override Image Icon => Properties.Resources.B16x16_Icon;

		public List<UnrealApplicationSettings> Applications { get; } = new List<UnrealApplicationSettings>()
		/*{
			new UnrealApplicationSettings
			{
				Name = "Playerunknown's Battlegrounds",
				Version = UnrealEngineVersion.UE4,
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
		}*/;

		public override bool Initialize(IPluginHost host)
		{
			System.Diagnostics.Debugger.Launch();

			if (this.host != null)
			{
				Terminate();
			}

			this.host = host ?? throw new ArgumentNullException(nameof(host));

			LoadApplications();

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

			SaveApplications();
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
				var moduleName = string.IsNullOrEmpty(settings.PatternModule) ? settings.ProcessName : settings.PatternModule;

				var namesArrayPtr = PatternScanner.FindPattern(
					BytePattern.Parse(settings.Pattern),
					process,
					process.GetModuleByName(moduleName)
				);
				if (!namesArrayPtr.IsNull())
				{
					switch (settings.PatternMethod)
					{
						case PatternMethod.Direct:
							namesArrayPtr = process.ReadRemoteIntPtr(namesArrayPtr + settings.PatternOffset);
							break;
						case PatternMethod.Derefence:
							var temp = process.ReadRemoteIntPtr(namesArrayPtr + settings.PatternOffset);
							namesArrayPtr = process.ReadRemoteIntPtr(temp);
							break;
					}

					if (namesArrayPtr.MayBeValid())
					{
						switch (settings.Version)
						{
							case UnrealEngineVersion.UE1:
								resolver = new UnrealEngine1NameResolver(process, new UnrealEngine1Config(settings)
								{
									GlobalArrayPtr = namesArrayPtr
								});
								break;
							case UnrealEngineVersion.UE2:
								resolver = new UnrealEngine2NameResolver(process, new UnrealEngine2Config(settings)
								{
									GlobalArrayPtr = namesArrayPtr
								});
								break;
							case UnrealEngineVersion.UE3:
								resolver = new UnrealEngine3NameResolver(process, new UnrealEngine3Config(settings)
								{
									GlobalArrayPtr = namesArrayPtr
								});
								break;
							case UnrealEngineVersion.UE4:
								resolver = new UnrealEngine4NameResolver(process, new UnrealEngine4Config(settings)
								{
									GlobalArrayPtr = namesArrayPtr
								});
								break;
							default:
								throw new ArgumentOutOfRangeException();
						}
					}
				}
			}
		}

		public string ReadNodeInfo(BaseNode node, IntPtr nodeAddress, IntPtr nodeValue, MemoryBuffer memory)
		{
			return resolver?.ReadNameOfObject(nodeValue);
		}

		private static readonly Lazy<string> applicationsPath = new Lazy<string>(() =>
		{
			var type = typeof(UnrealPluginExt);

			string path = null;
			try
			{
				path = type.Assembly.Location;
			}
			catch (Exception)
			{

			}

			if (string.IsNullOrEmpty(path))
			{
				path = type.Assembly.GetName().CodeBase;
				path = PathUtil.FileUrlToPath(path);
			}

			path = Path.GetDirectoryName(path);

			path = Path.Combine(path, "UnrealEngineApplications");

			return path;
		});

		private void LoadApplications()
		{
			Applications.Clear();

			try
			{
				var dir = new DirectoryInfo(applicationsPath.Value);
				if (dir.Exists)
				{
					foreach (var file in dir.EnumerateFiles())
					{
						try
						{
							Applications.Add(UnrealApplicationSettings.ReadFromFile(file.FullName));
						}
						catch (Exception ex)
						{
							Program.ShowException(ex);
						}
					}
				}
			}
			catch (DirectoryNotFoundException)
			{
				// ignore
			}
		}

		private void SaveApplications()
		{
			Directory.CreateDirectory(applicationsPath.Value);

			foreach (var app in Applications)
			{
				var path = Path.Combine(applicationsPath.Value, GetFilenameFromApplication(app));

				try
				{
					UnrealApplicationSettings.WriteToFile(app, path);
				}
				catch (Exception ex)
				{
					Program.ShowException(ex);
				}
			}
		}

		internal static void DeleteApplication(UnrealApplicationSettings settings)
		{
			Contract.Requires(settings != null);

			var path = Path.Combine(applicationsPath.Value, GetFilenameFromApplication(settings));

			File.Delete(path);
		}

		private static string GetFilenameFromApplication(UnrealApplicationSettings settings)
		{
			Contract.Requires(settings != null);

			var name = settings.Name;
			foreach (var c in Path.GetInvalidFileNameChars())
			{
				name = name.Replace(c, '_');
			}
			return $"{name}.xml";
		}
	}
}
