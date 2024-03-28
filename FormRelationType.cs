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
    public partial class FormRelationType : Form
    {
        string message = "Message";
        string relationType = "MS";
        public FormRelationType()
        {
            InitializeComponent();
        }

        public string Message { get => message; set => message = value; }
        public string RelationType { get => relationType; set => relationType = value; }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox_Message.Text))
            {
                if (RBtnMS.Checked)
                {
                    relationType = "MS";
                }
                else if (RBtnMR.Checked)
                {
                    relationType = "MA";
                }
                else
                {
                    relationType = "RM";
                }
                message = textBox_Message.Text;
                this.Close();
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
