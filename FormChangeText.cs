using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SHI_UML
{
    public partial class FormChangeText : Form
    {
        private string textValue;
        string text = "";
        public FormChangeText(string txt)
        {
            InitializeComponent();
            text = txt;
        }

        public string TextValue { get => textValue; set => textValue = value; }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            textValue = textBox_Text.Text;
            this.Close();
        }

        private void FormChangeText_Load(object sender, EventArgs e)
        {
            textBox_Text.Text = text;
            textBox_Text.SelectAll();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
