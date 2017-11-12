using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ReClassNET.Forms;

namespace UnrealPlugin.Forms
{
	public partial class ApplicationForm : IconForm
	{
		public string ApplicationName => nameTextBox.Text;

		public ApplicationForm()
		{
			InitializeComponent();
		}
	}
}
