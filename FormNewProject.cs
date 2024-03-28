using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace SHI_UML
{
    public partial class FormNewProject : Form
    {
        UMLProject umlProj;
        public FormNewProject()
        {
            InitializeComponent();
        }

        internal UMLProject UmlProj { get => umlProj; set => umlProj = value; }

        private void FormNewProject_Load(object sender, EventArgs e)
        {

        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog Fbd = new FolderBrowserDialog();
            if (Fbd.ShowDialog() == DialogResult.OK)
            {
                textBox_Location.Text = Fbd.SelectedPath + @"\";
            }
        }

        private void BtnCreat_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox_Name.Text) && !string.IsNullOrWhiteSpace(textBox_Location.Text))
            {
                string pathpr = textBox_Location.Text + textBox_Name.Text;
                umlProj = new UMLProject
                {
                    Name = textBox_Name.Text,
                    PathP = pathpr,
                };
                if (!Directory.Exists(pathpr))
                {
                    Directory.CreateDirectory(pathpr);
                }
                this.Close();
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
