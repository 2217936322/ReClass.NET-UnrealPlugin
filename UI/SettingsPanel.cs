using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ReClassNET.UI;
using ReClassNET.Util;

namespace UnrealPlugin.UI
{
	public partial class SettingsPanel : UserControl
	{
		internal class UnrealEngineVersionComboBox : EnumComboBox<UnrealEngineVersion> { }
		internal class PlatformComboBox : EnumComboBox<Platform> { }
		internal class PatternMethodComboBox : EnumComboBox<PatternMethod> { }

		public SettingsPanel(UnrealPluginExt plugin)
		{
			InitializeComponent();

			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			BackColor = Color.Transparent;

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
			UpdateSettingsFromGui();
		}

		private void UpdateGuiFromSettings(UnrealApplicationSettings settings)
		{
			if (settings == null)
			{
				applicationSettingsGroupBox.Enabled = false;

				return;
			}

			applicationSettingsGroupBox.Enabled = true;

			engineVersionComboBox.SelectedValue = settings.Version;
			platformComboBox.SelectedValue = settings.Platform;

			patternMethodComboBox.SelectedValue = settings.PatternMethod;

			patternTextBox.Text = settings.Pattern;
			patternOffsetNumericUpDown.Value = settings.PatternOffset;

			objectNameIndexOffsetNumericUpDown.Value = settings.UObjectNameOffset;
			nameEntryIndexOffsetNumericUpDown.Value = settings.FNameEntryIndexOffset;
			nameEntryDataOffsetNumericUpDown.Value = settings.FNameEntryNameDataOffset;
			nameEntryIsWideCharCheckBox.Checked = settings.FNameEntryIsWide;
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
			//settings.PatternModule = ;
			settings.Pattern = patternTextBox.Text;
			settings.PatternOffset = (int)patternOffsetNumericUpDown.Value;

			settings.UObjectNameOffset = (int)objectNameIndexOffsetNumericUpDown.Value;
			settings.FNameEntryIndexOffset = (int)nameEntryIndexOffsetNumericUpDown.Value;
			settings.FNameEntryNameDataOffset = (int)nameEntryDataOffsetNumericUpDown.Value;
			settings.FNameEntryIsWide = nameEntryIsWideCharCheckBox.Checked;
		}
	}
}
