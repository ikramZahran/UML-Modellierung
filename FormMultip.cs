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
    public partial class FormMultip : Form
    {
        public FormMultip()
        {
            InitializeComponent();
        }
        private string textValue;

        public string TextValue { get => textValue; set => textValue = value; }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            textValue = comboBox_M.Text;
            this.Close();
        }

        private void FormMultip_Load(object sender, EventArgs e)
        {
            comboBox_M.Items.Add("0..1");
            comboBox_M.Items.Add("0..*");
            comboBox_M.Items.Add("1..*");
            comboBox_M.Items.Add("1");
            comboBox_M.Items.Add("*");
            comboBox_M.SelectedIndex = 0;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

      /*  private Boolean autoriser_multiplicity(String valeur)
        {
            if (ASC(valeur)<48 and asc(valeur)<>8)
            {

            }
        }*/
    }
}
