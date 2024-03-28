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
    public partial class FormNewClass : Form
    {
        public FormNewClass()
        {
            InitializeComponent();
        }

        private void FormNewClass_Load(object sender, EventArgs e)
        {
            comboBox_att.Items.Add("+");
            comboBox_att.Items.Add("-");
            comboBox_att.Items.Add("#");
            comboBox_att.SelectedIndex = 1;

            comboBox_meth.Items.Add("+");
            comboBox_meth.Items.Add("-");
            comboBox_meth.Items.Add("#");
            comboBox_meth.SelectedIndex = 0;
        }
        UmlClass classUML;
        List<string> Attributes = new List<string>();
        List<string> Methdodes = new List<string>();

        internal UmlClass ClassUML { get => classUML; set => classUML = value; }

        private void button_Add_Click(object sender, EventArgs e)
        {
            classUML = new UmlClass
            {
                Text = textBox_Name.Text,
                Attributs = Attributes,
                Methodes = Methdodes,
            };
            this.Close();
            //if (!string.IsNullOrWhiteSpace(textBox_Name.Text))
            //{
            //}
            //else
            //{
            //    MessageBox.Show("Warning : Name is Empty", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //}
        }

        private void button_AddAtt_Click(object sender, EventArgs e)
        {
            Attributes.Add(comboBox_att.Text + " " + textBox_Attribute.Text);
            listBox_att.Items.Add(comboBox_att.Text + " " + textBox_Attribute.Text);
        }

        private void button_AddMeth_Click(object sender, EventArgs e)
        {
            Methdodes.Add(comboBox_meth.Text + " " + textBox_Methode.Text);
            listBox_Meth.Items.Add(comboBox_meth.Text + " " + textBox_Methode.Text);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

