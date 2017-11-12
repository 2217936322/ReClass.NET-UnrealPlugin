using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Windows.Forms;
using ReClassNET.UI;
using UnrealPlugin.Forms;

namespace UnrealPlugin.UI
{
	public partial class SettingsPanel : UserControl
	{
		internal class UnrealEngineVersionComboBox : EnumComboBox<UnrealEngineVersion> { }
		internal class PlatformComboBox : EnumComboBox<Platform> { }
		internal class PatternMethodComboBox : EnumComboBox<PatternMethod> { }

		private readonly UnrealPluginExt plugin;

		private bool disableEvents = false;

		public SettingsPanel(UnrealPluginExt plugin)
		{
			Contract.Requires(plugin != null);

			this.plugin = plugin;

			InitializeComponent();

			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			BackColor = Color.Transparent;

			BindSettings();
		}

		private void BindSettings()
		{
			applicationComboBox.DataSource = plugin.Applications;
		}

		private UnrealApplicationSettings GetSelectedApplicationSettings() => applicationComboBox.SelectedItem as UnrealApplicationSettings;

		private void applicationComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateGuiFromSettings(GetSelectedApplicationSettings());
		}

		private void engineVersionComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			var version = engineVersionComboBox.SelectedValue;
			nameEntryIsWideCharCheckBox.Enabled = version == UnrealEngineVersion.UE1 || version == UnrealEngineVersion.UE2;
		}

		private void OnInputChanged(object sender, EventArgs e)
		{
			if (!disableEvents)
			{
				UpdateSettingsFromGui();
			}
		}

		private void UpdateGuiFromSettings(UnrealApplicationSettings settings)
		{
			if (settings == null)
			{
				applicationSettingsGroupBox.Enabled = false;

				return;
			}

			try
			{
				disableEvents = true;

				applicationSettingsGroupBox.Enabled = true;

				engineVersionComboBox.SelectedValue = settings.Version;
				platformComboBox.SelectedValue = settings.Platform;

				patternMethodComboBox.SelectedValue = settings.PatternMethod;
				patternModuleTextBox.Text = settings.PatternModule;
				patternTextBox.Text = settings.Pattern;
				patternOffsetNumericUpDown.Value = settings.PatternOffset;

				objectNameIndexOffsetNumericUpDown.Value = settings.UObjectNameOffset;
				nameEntryIndexOffsetNumericUpDown.Value = settings.FNameEntryIndexOffset;
				nameEntryDataOffsetNumericUpDown.Value = settings.FNameEntryNameDataOffset;
				nameEntryIsWideCharCheckBox.Checked = settings.FNameEntryIsWide;
			}
			finally
			{
				disableEvents = false;
			}
		}

		private void UpdateSettingsFromGui()
		{
			var settings = GetSelectedApplicationSettings();
			if (settings == null)
			{
				return;
			}

			settings.Version = engineVersionComboBox.SelectedValue;
			settings.Platform = platformComboBox.SelectedValue;

			settings.PatternMethod = patternMethodComboBox.SelectedValue;
			settings.PatternModule = patternModuleTextBox.Text.Trim();
			settings.Pattern = patternTextBox.Text.Trim();
			settings.PatternOffset = (int)patternOffsetNumericUpDown.Value;

			settings.UObjectNameOffset = (int)objectNameIndexOffsetNumericUpDown.Value;
			settings.FNameEntryIndexOffset = (int)nameEntryIndexOffsetNumericUpDown.Value;
			settings.FNameEntryNameDataOffset = (int)nameEntryDataOffsetNumericUpDown.Value;
			settings.FNameEntryIsWide = nameEntryIsWideCharCheckBox.Checked;
		}

		private void addButton_Click(object sender, EventArgs e)
		{
			using (var caf = new ApplicationForm())
			{
				if (caf.ShowDialog() == DialogResult.OK)
				{
					var settings = new UnrealApplicationSettings
					{
						Name = caf.ApplicationName.Trim()
					};
					plugin.Applications.Add(settings);

					BindSettings();

					applicationComboBox.SelectedItem = settings;
				}
			}
		}

		private void deleteButton_Click(object sender, EventArgs e)
		{
			var settings = GetSelectedApplicationSettings();
			if (settings == null)
			{
				return;
			}

			plugin.Applications.Remove(settings);

			BindSettings();
		}
	}
}
