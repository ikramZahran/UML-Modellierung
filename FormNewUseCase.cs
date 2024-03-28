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
    public partial class FormNewUseCase : Form
    {
        private string useCaseText = "";
        public FormNewUseCase(string text)
        {
            InitializeComponent();
            label1.Text = text;
        }

        public string UseCaseText { get => useCaseText; set => useCaseText = value; }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox_Name.Text))
            {
                UseCaseText = textBox_Name.Text;
                this.Close();
            }
        }

        private void FormNewUseCase_Load(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
