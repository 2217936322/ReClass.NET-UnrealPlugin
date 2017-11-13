namespace UnrealPlugin.UI
{
	partial class SettingsPanel
	{
		/// <summary> 
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Verwendete Ressourcen bereinigen.
		/// </summary>
		/// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Vom Komponenten-Designer generierter Code

		/// <summary> 
		/// Erforderliche Methode für die Designerunterstützung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
			this.applicationSettingsGroupBox = new System.Windows.Forms.GroupBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.nameEntryIsWideCharCheckBox = new System.Windows.Forms.CheckBox();
			this.nameEntryDataOffsetNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.nameEntryIndexOffsetNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.objectNameIndexOffsetNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.nameEntryLabel = new System.Windows.Forms.Label();
			this.objectLabel = new System.Windows.Forms.Label();
			this.patternGroupBox = new System.Windows.Forms.GroupBox();
			this.patternModuleTextBox = new System.Windows.Forms.TextBox();
			this.moduleLabel = new System.Windows.Forms.Label();
			this.patternOffsetNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.methodLabel = new System.Windows.Forms.Label();
			this.patternTextBox = new System.Windows.Forms.TextBox();
			this.patternLabel = new System.Windows.Forms.Label();
			this.platformLabel = new System.Windows.Forms.Label();
			this.engineVersionLabel = new System.Windows.Forms.Label();
			this.applicationComboBox = new System.Windows.Forms.ComboBox();
			this.applicationLabel = new System.Windows.Forms.Label();
			this.objectOuterOffsetNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.displayFullNameCheckBox = new System.Windows.Forms.CheckBox();
			this.deleteButton = new ReClassNET.UI.IconButton();
			this.addButton = new ReClassNET.UI.IconButton();
			this.patternMethodComboBox = new UnrealPlugin.UI.SettingsPanel.PatternMethodComboBox();
			this.platformComboBox = new UnrealPlugin.UI.SettingsPanel.PlatformComboBox();
			this.engineVersionComboBox = new UnrealPlugin.UI.SettingsPanel.UnrealEngineVersionComboBox();
			this.applicationSettingsGroupBox.SuspendLayout();
			this.groupBox3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nameEntryDataOffsetNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nameEntryIndexOffsetNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.objectNameIndexOffsetNumericUpDown)).BeginInit();
			this.patternGroupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.patternOffsetNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.objectOuterOffsetNumericUpDown)).BeginInit();
			this.SuspendLayout();
			// 
			// applicationSettingsGroupBox
			// 
			this.applicationSettingsGroupBox.Controls.Add(this.groupBox3);
			this.applicationSettingsGroupBox.Controls.Add(this.patternGroupBox);
			this.applicationSettingsGroupBox.Controls.Add(this.platformLabel);
			this.applicationSettingsGroupBox.Controls.Add(this.platformComboBox);
			this.applicationSettingsGroupBox.Controls.Add(this.engineVersionLabel);
			this.applicationSettingsGroupBox.Controls.Add(this.engineVersionComboBox);
			this.applicationSettingsGroupBox.Location = new System.Drawing.Point(9, 33);
			this.applicationSettingsGroupBox.Name = "applicationSettingsGroupBox";
			this.applicationSettingsGroupBox.Size = new System.Drawing.Size(539, 281);
			this.applicationSettingsGroupBox.TabIndex = 6;
			this.applicationSettingsGroupBox.TabStop = false;
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.displayFullNameCheckBox);
			this.groupBox3.Controls.Add(this.objectOuterOffsetNumericUpDown);
			this.groupBox3.Controls.Add(this.nameEntryIsWideCharCheckBox);
			this.groupBox3.Controls.Add(this.nameEntryDataOffsetNumericUpDown);
			this.groupBox3.Controls.Add(this.nameEntryIndexOffsetNumericUpDown);
			this.groupBox3.Controls.Add(this.objectNameIndexOffsetNumericUpDown);
			this.groupBox3.Controls.Add(this.nameEntryLabel);
			this.groupBox3.Controls.Add(this.objectLabel);
			this.groupBox3.Location = new System.Drawing.Point(9, 154);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(524, 121);
			this.groupBox3.TabIndex = 5;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Offsets";
			// 
			// nameEntryIsWideCharCheckBox
			// 
			this.nameEntryIsWideCharCheckBox.AutoSize = true;
			this.nameEntryIsWideCharCheckBox.Location = new System.Drawing.Point(390, 80);
			this.nameEntryIsWideCharCheckBox.Name = "nameEntryIsWideCharCheckBox";
			this.nameEntryIsWideCharCheckBox.Size = new System.Drawing.Size(87, 17);
			this.nameEntryIsWideCharCheckBox.TabIndex = 5;
			this.nameEntryIsWideCharCheckBox.Text = "Is Wide Char";
			this.nameEntryIsWideCharCheckBox.UseVisualStyleBackColor = true;
			this.nameEntryIsWideCharCheckBox.CheckedChanged += new System.EventHandler(this.OnInputChanged);
			// 
			// nameEntryDataOffsetNumericUpDown
			// 
			this.nameEntryDataOffsetNumericUpDown.Hexadecimal = true;
			this.nameEntryDataOffsetNumericUpDown.Location = new System.Drawing.Point(319, 79);
			this.nameEntryDataOffsetNumericUpDown.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
			this.nameEntryDataOffsetNumericUpDown.Name = "nameEntryDataOffsetNumericUpDown";
			this.nameEntryDataOffsetNumericUpDown.Size = new System.Drawing.Size(50, 20);
			this.nameEntryDataOffsetNumericUpDown.TabIndex = 4;
			this.nameEntryDataOffsetNumericUpDown.ValueChanged += new System.EventHandler(this.OnInputChanged);
			// 
			// nameEntryIndexOffsetNumericUpDown
			// 
			this.nameEntryIndexOffsetNumericUpDown.Hexadecimal = true;
			this.nameEntryIndexOffsetNumericUpDown.Location = new System.Drawing.Point(319, 53);
			this.nameEntryIndexOffsetNumericUpDown.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
			this.nameEntryIndexOffsetNumericUpDown.Name = "nameEntryIndexOffsetNumericUpDown";
			this.nameEntryIndexOffsetNumericUpDown.Size = new System.Drawing.Size(50, 20);
			this.nameEntryIndexOffsetNumericUpDown.TabIndex = 3;
			this.nameEntryIndexOffsetNumericUpDown.ValueChanged += new System.EventHandler(this.OnInputChanged);
			// 
			// objectNameIndexOffsetNumericUpDown
			// 
			this.objectNameIndexOffsetNumericUpDown.Hexadecimal = true;
			this.objectNameIndexOffsetNumericUpDown.Location = new System.Drawing.Point(93, 53);
			this.objectNameIndexOffsetNumericUpDown.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
			this.objectNameIndexOffsetNumericUpDown.Name = "objectNameIndexOffsetNumericUpDown";
			this.objectNameIndexOffsetNumericUpDown.Size = new System.Drawing.Size(50, 20);
			this.objectNameIndexOffsetNumericUpDown.TabIndex = 2;
			this.objectNameIndexOffsetNumericUpDown.ValueChanged += new System.EventHandler(this.OnInputChanged);
			// 
			// nameEntryLabel
			// 
			this.nameEntryLabel.Location = new System.Drawing.Point(260, 16);
			this.nameEntryLabel.Name = "nameEntryLabel";
			this.nameEntryLabel.Size = new System.Drawing.Size(91, 100);
			this.nameEntryLabel.TabIndex = 1;
			this.nameEntryLabel.Text = "class NameEntry\r\n{\r\n    ···\r\n    Index\r\n    ···\r\n    Data\r\n}";
			// 
			// objectLabel
			// 
			this.objectLabel.Location = new System.Drawing.Point(6, 16);
			this.objectLabel.Name = "objectLabel";
			this.objectLabel.Size = new System.Drawing.Size(81, 100);
			this.objectLabel.TabIndex = 0;
			this.objectLabel.Text = "class UObject\r\n{\r\n    ···\r\n    NameIndex\r\n    ···\r\n    Outer\r\n}";
			// 
			// patternGroupBox
			// 
			this.patternGroupBox.Controls.Add(this.patternModuleTextBox);
			this.patternGroupBox.Controls.Add(this.moduleLabel);
			this.patternGroupBox.Controls.Add(this.patternOffsetNumericUpDown);
			this.patternGroupBox.Controls.Add(this.label1);
			this.patternGroupBox.Controls.Add(this.methodLabel);
			this.patternGroupBox.Controls.Add(this.patternMethodComboBox);
			this.patternGroupBox.Controls.Add(this.patternTextBox);
			this.patternGroupBox.Controls.Add(this.patternLabel);
			this.patternGroupBox.Location = new System.Drawing.Point(9, 48);
			this.patternGroupBox.Name = "patternGroupBox";
			this.patternGroupBox.Size = new System.Drawing.Size(524, 100);
			this.patternGroupBox.TabIndex = 4;
			this.patternGroupBox.TabStop = false;
			this.patternGroupBox.Text = "Global Names Pattern";
			// 
			// patternModuleTextBox
			// 
			this.patternModuleTextBox.Location = new System.Drawing.Point(56, 72);
			this.patternModuleTextBox.Name = "patternModuleTextBox";
			this.patternModuleTextBox.Size = new System.Drawing.Size(462, 20);
			this.patternModuleTextBox.TabIndex = 7;
			this.patternModuleTextBox.TextChanged += new System.EventHandler(this.OnInputChanged);
			// 
			// moduleLabel
			// 
			this.moduleLabel.AutoSize = true;
			this.moduleLabel.Location = new System.Drawing.Point(6, 75);
			this.moduleLabel.Name = "moduleLabel";
			this.moduleLabel.Size = new System.Drawing.Size(45, 13);
			this.moduleLabel.TabIndex = 6;
			this.moduleLabel.Text = "Module:";
			// 
			// patternOffsetNumericUpDown
			// 
			this.patternOffsetNumericUpDown.Location = new System.Drawing.Point(465, 45);
			this.patternOffsetNumericUpDown.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
			this.patternOffsetNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
			this.patternOffsetNumericUpDown.Name = "patternOffsetNumericUpDown";
			this.patternOffsetNumericUpDown.Size = new System.Drawing.Size(53, 20);
			this.patternOffsetNumericUpDown.TabIndex = 5;
			this.patternOffsetNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
			this.patternOffsetNumericUpDown.ValueChanged += new System.EventHandler(this.OnInputChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(425, 47);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(38, 13);
			this.label1.TabIndex = 4;
			this.label1.Text = "Offset:";
			// 
			// methodLabel
			// 
			this.methodLabel.AutoSize = true;
			this.methodLabel.Location = new System.Drawing.Point(6, 48);
			this.methodLabel.Name = "methodLabel";
			this.methodLabel.Size = new System.Drawing.Size(46, 13);
			this.methodLabel.TabIndex = 3;
			this.methodLabel.Text = "Method:";
			// 
			// patternTextBox
			// 
			this.patternTextBox.Location = new System.Drawing.Point(56, 19);
			this.patternTextBox.Name = "patternTextBox";
			this.patternTextBox.Size = new System.Drawing.Size(462, 20);
			this.patternTextBox.TabIndex = 1;
			this.patternTextBox.TextChanged += new System.EventHandler(this.OnInputChanged);
			// 
			// patternLabel
			// 
			this.patternLabel.AutoSize = true;
			this.patternLabel.Location = new System.Drawing.Point(6, 22);
			this.patternLabel.Name = "patternLabel";
			this.patternLabel.Size = new System.Drawing.Size(44, 13);
			this.patternLabel.TabIndex = 0;
			this.patternLabel.Text = "Pattern:";
			// 
			// platformLabel
			// 
			this.platformLabel.AutoSize = true;
			this.platformLabel.Location = new System.Drawing.Point(241, 22);
			this.platformLabel.Name = "platformLabel";
			this.platformLabel.Size = new System.Drawing.Size(48, 13);
			this.platformLabel.TabIndex = 3;
			this.platformLabel.Text = "Platform:";
			// 
			// engineVersionLabel
			// 
			this.engineVersionLabel.AutoSize = true;
			this.engineVersionLabel.Location = new System.Drawing.Point(6, 22);
			this.engineVersionLabel.Name = "engineVersionLabel";
			this.engineVersionLabel.Size = new System.Drawing.Size(43, 13);
			this.engineVersionLabel.TabIndex = 1;
			this.engineVersionLabel.Text = "Engine:";
			// 
			// applicationComboBox
			// 
			this.applicationComboBox.DisplayMember = "Name";
			this.applicationComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.applicationComboBox.FormattingEnabled = true;
			this.applicationComboBox.Location = new System.Drawing.Point(63, 6);
			this.applicationComboBox.Name = "applicationComboBox";
			this.applicationComboBox.Size = new System.Drawing.Size(427, 21);
			this.applicationComboBox.TabIndex = 5;
			this.applicationComboBox.SelectedIndexChanged += new System.EventHandler(this.applicationComboBox_SelectedIndexChanged);
			// 
			// applicationLabel
			// 
			this.applicationLabel.AutoSize = true;
			this.applicationLabel.Location = new System.Drawing.Point(15, 9);
			this.applicationLabel.Name = "applicationLabel";
			this.applicationLabel.Size = new System.Drawing.Size(38, 13);
			this.applicationLabel.TabIndex = 4;
			this.applicationLabel.Text = "Game:";
			// 
			// objectOuterOffsetNumericUpDown
			// 
			this.objectOuterOffsetNumericUpDown.Hexadecimal = true;
			this.objectOuterOffsetNumericUpDown.Location = new System.Drawing.Point(93, 79);
			this.objectOuterOffsetNumericUpDown.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
			this.objectOuterOffsetNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
			this.objectOuterOffsetNumericUpDown.Name = "objectOuterOffsetNumericUpDown";
			this.objectOuterOffsetNumericUpDown.Size = new System.Drawing.Size(50, 20);
			this.objectOuterOffsetNumericUpDown.TabIndex = 6;
			this.objectOuterOffsetNumericUpDown.ValueChanged += new System.EventHandler(this.OnInputChanged);
			// 
			// displayFullNameCheckBox
			// 
			this.displayFullNameCheckBox.AutoSize = true;
			this.displayFullNameCheckBox.Location = new System.Drawing.Point(390, 54);
			this.displayFullNameCheckBox.Name = "displayFullNameCheckBox";
			this.displayFullNameCheckBox.Size = new System.Drawing.Size(110, 17);
			this.displayFullNameCheckBox.TabIndex = 7;
			this.displayFullNameCheckBox.Text = "Display Full Name";
			this.displayFullNameCheckBox.UseVisualStyleBackColor = true;
			this.displayFullNameCheckBox.CheckedChanged += new System.EventHandler(this.OnInputChanged);
			// 
			// deleteButton
			// 
			this.deleteButton.Image = global::UnrealPlugin.Properties.Resources.B16x16_Button_Delete;
			this.deleteButton.Location = new System.Drawing.Point(525, 6);
			this.deleteButton.Name = "deleteButton";
			this.deleteButton.Pressed = false;
			this.deleteButton.Selected = false;
			this.deleteButton.Size = new System.Drawing.Size(23, 22);
			this.deleteButton.TabIndex = 8;
			this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
			// 
			// addButton
			// 
			this.addButton.Image = global::UnrealPlugin.Properties.Resources.B16x16_Button_Add;
			this.addButton.Location = new System.Drawing.Point(496, 6);
			this.addButton.Name = "addButton";
			this.addButton.Pressed = false;
			this.addButton.Selected = false;
			this.addButton.Size = new System.Drawing.Size(23, 22);
			this.addButton.TabIndex = 7;
			this.addButton.Click += new System.EventHandler(this.addButton_Click);
			// 
			// patternMethodComboBox
			// 
			this.patternMethodComboBox.Location = new System.Drawing.Point(56, 45);
			this.patternMethodComboBox.Name = "patternMethodComboBox";
			this.patternMethodComboBox.Size = new System.Drawing.Size(359, 21);
			this.patternMethodComboBox.TabIndex = 2;
			this.patternMethodComboBox.SelectionChangeCommitted += new System.EventHandler(this.OnInputChanged);
			// 
			// platformComboBox
			// 
			this.platformComboBox.Location = new System.Drawing.Point(295, 19);
			this.platformComboBox.Name = "platformComboBox";
			this.platformComboBox.Size = new System.Drawing.Size(65, 21);
			this.platformComboBox.TabIndex = 2;
			this.platformComboBox.SelectionChangeCommitted += new System.EventHandler(this.OnInputChanged);
			// 
			// engineVersionComboBox
			// 
			this.engineVersionComboBox.Location = new System.Drawing.Point(54, 19);
			this.engineVersionComboBox.Name = "engineVersionComboBox";
			this.engineVersionComboBox.Size = new System.Drawing.Size(156, 21);
			this.engineVersionComboBox.TabIndex = 0;
			this.engineVersionComboBox.SelectedIndexChanged += new System.EventHandler(this.engineVersionComboBox_SelectedIndexChanged);
			this.engineVersionComboBox.SelectionChangeCommitted += new System.EventHandler(this.OnInputChanged);
			// 
			// SettingsPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.deleteButton);
			this.Controls.Add(this.addButton);
			this.Controls.Add(this.applicationSettingsGroupBox);
			this.Controls.Add(this.applicationComboBox);
			this.Controls.Add(this.applicationLabel);
			this.Name = "SettingsPanel";
			this.Size = new System.Drawing.Size(554, 329);
			this.applicationSettingsGroupBox.ResumeLayout(false);
			this.applicationSettingsGroupBox.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nameEntryDataOffsetNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nameEntryIndexOffsetNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.objectNameIndexOffsetNumericUpDown)).EndInit();
			this.patternGroupBox.ResumeLayout(false);
			this.patternGroupBox.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.patternOffsetNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.objectOuterOffsetNumericUpDown)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox applicationSettingsGroupBox;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.CheckBox nameEntryIsWideCharCheckBox;
		private System.Windows.Forms.NumericUpDown nameEntryDataOffsetNumericUpDown;
		private System.Windows.Forms.NumericUpDown nameEntryIndexOffsetNumericUpDown;
		private System.Windows.Forms.NumericUpDown objectNameIndexOffsetNumericUpDown;
		private System.Windows.Forms.Label nameEntryLabel;
		private System.Windows.Forms.Label objectLabel;
		private System.Windows.Forms.GroupBox patternGroupBox;
		private System.Windows.Forms.Label methodLabel;
		private PatternMethodComboBox patternMethodComboBox;
		private System.Windows.Forms.TextBox patternTextBox;
		private System.Windows.Forms.Label patternLabel;
		private System.Windows.Forms.Label platformLabel;
		private PlatformComboBox platformComboBox;
		private System.Windows.Forms.Label engineVersionLabel;
		private UnrealEngineVersionComboBox engineVersionComboBox;
		private System.Windows.Forms.ComboBox applicationComboBox;
		private System.Windows.Forms.Label applicationLabel;
		private ReClassNET.UI.IconButton addButton;
		private ReClassNET.UI.IconButton deleteButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown patternOffsetNumericUpDown;
		private System.Windows.Forms.TextBox patternModuleTextBox;
		private System.Windows.Forms.Label moduleLabel;
		private System.Windows.Forms.CheckBox displayFullNameCheckBox;
		private System.Windows.Forms.NumericUpDown objectOuterOffsetNumericUpDown;
	}
}
