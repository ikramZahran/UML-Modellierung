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
    public partial class FormNewActor : Form
    {
        string actorN = "";
        bool showGB = false;
        public FormNewActor(bool showGB)
        {
            InitializeComponent();
            this.showGB = showGB;
        }

        public string ActorN { get => actorN; set => actorN = value; }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox_Name.Text))
            {
                if (RBtnAct.Checked)
                {
                    actorN = "act" + textBox_Name.Text;
                }
                else
                {
                    actorN = "sys" + textBox_Name.Text;
                }
                this.Close();
            }
        }

        private void FormNewActor_Load(object sender, EventArgs e)
        {
            groupBox1.Visible = showGB;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
