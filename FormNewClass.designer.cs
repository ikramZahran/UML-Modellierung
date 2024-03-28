namespace SHI_UML
{
    partial class FormNewClass
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormNewClass));
            this.listBox_Meth = new System.Windows.Forms.ListBox();
            this.listBox_att = new System.Windows.Forms.ListBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_Name = new System.Windows.Forms.TextBox();
            this.button_Add = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBox_att = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_Attribute = new System.Windows.Forms.TextBox();
            this.button_AddAtt = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.comboBox_meth = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_Methode = new System.Windows.Forms.TextBox();
            this.button_AddMeth = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBox_Meth
            // 
            this.listBox_Meth.FormattingEnabled = true;
            this.listBox_Meth.Location = new System.Drawing.Point(275, 288);
            this.listBox_Meth.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.listBox_Meth.Name = "listBox_Meth";
            this.listBox_Meth.Size = new System.Drawing.Size(216, 225);
            this.listBox_Meth.TabIndex = 61;
            // 
            // listBox_att
            // 
            this.listBox_att.FormattingEnabled = true;
            this.listBox_att.Location = new System.Drawing.Point(9, 288);
            this.listBox_att.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.listBox_att.Name = "listBox_att";
            this.listBox_att.Size = new System.Drawing.Size(216, 225);
            this.listBox_att.TabIndex = 60;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(213)))), ((int)(((byte)(237)))));
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.Black;
            this.label9.Location = new System.Drawing.Point(9, 262);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(215, 20);
            this.label9.TabIndex = 59;
            this.label9.Text = "Attributs";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(213)))), ((int)(((byte)(237)))));
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(274, 262);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(215, 20);
            this.label8.TabIndex = 58;
            this.label8.Text = "Methods";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(9, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 20);
            this.label1.TabIndex = 51;
            this.label1.Text = "Class Name :\r\n";
            // 
            // textBox_Name
            // 
            this.textBox_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_Name.Location = new System.Drawing.Point(121, 7);
            this.textBox_Name.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBox_Name.Name = "textBox_Name";
            this.textBox_Name.Size = new System.Drawing.Size(370, 26);
            this.textBox_Name.TabIndex = 47;
            // 
            // button_Add
            // 
            this.button_Add.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(213)))), ((int)(((byte)(237)))));
            this.button_Add.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_Add.FlatAppearance.BorderSize = 0;
            this.button_Add.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Add.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Add.ForeColor = System.Drawing.Color.Black;
            this.button_Add.Location = new System.Drawing.Point(289, 523);
            this.button_Add.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_Add.Name = "button_Add";
            this.button_Add.Size = new System.Drawing.Size(99, 31);
            this.button_Add.TabIndex = 43;
            this.button_Add.Text = "&Add";
            this.button_Add.UseVisualStyleBackColor = false;
            this.button_Add.Click += new System.EventHandler(this.button_Add_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(213)))), ((int)(((byte)(237)))));
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.Black;
            this.btnCancel.Location = new System.Drawing.Point(392, 523);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(99, 31);
            this.btnCancel.TabIndex = 63;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboBox_att);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox_Attribute);
            this.groupBox1.Controls.Add(this.button_AddAtt);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(8, 44);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(483, 97);
            this.groupBox1.TabIndex = 64;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Attributs :";
            // 
            // comboBox_att
            // 
            this.comboBox_att.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox_att.FormattingEnabled = true;
            this.comboBox_att.Location = new System.Drawing.Point(125, 24);
            this.comboBox_att.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.comboBox_att.Name = "comboBox_att";
            this.comboBox_att.Size = new System.Drawing.Size(98, 28);
            this.comboBox_att.TabIndex = 67;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(47, 28);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 20);
            this.label4.TabIndex = 66;
            this.label4.Text = "Visibility :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(6, 61);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 20);
            this.label2.TabIndex = 65;
            this.label2.Text = "Attribut name :";
            // 
            // textBox_Attribute
            // 
            this.textBox_Attribute.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_Attribute.Location = new System.Drawing.Point(125, 58);
            this.textBox_Attribute.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBox_Attribute.Name = "textBox_Attribute";
            this.textBox_Attribute.Size = new System.Drawing.Size(237, 26);
            this.textBox_Attribute.TabIndex = 64;
            // 
            // button_AddAtt
            // 
            this.button_AddAtt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(213)))), ((int)(((byte)(237)))));
            this.button_AddAtt.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_AddAtt.FlatAppearance.BorderSize = 0;
            this.button_AddAtt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_AddAtt.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_AddAtt.ForeColor = System.Drawing.Color.Black;
            this.button_AddAtt.Location = new System.Drawing.Point(369, 56);
            this.button_AddAtt.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_AddAtt.Name = "button_AddAtt";
            this.button_AddAtt.Size = new System.Drawing.Size(99, 31);
            this.button_AddAtt.TabIndex = 63;
            this.button_AddAtt.Text = "&Add Attribut";
            this.button_AddAtt.UseVisualStyleBackColor = false;
            this.button_AddAtt.Click += new System.EventHandler(this.button_AddAtt_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.comboBox_meth);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.textBox_Methode);
            this.groupBox2.Controls.Add(this.button_AddMeth);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(8, 147);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(483, 101);
            this.groupBox2.TabIndex = 68;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Methods :";
            // 
            // comboBox_meth
            // 
            this.comboBox_meth.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox_meth.FormattingEnabled = true;
            this.comboBox_meth.Location = new System.Drawing.Point(126, 22);
            this.comboBox_meth.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.comboBox_meth.Name = "comboBox_meth";
            this.comboBox_meth.Size = new System.Drawing.Size(98, 28);
            this.comboBox_meth.TabIndex = 67;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(50, 26);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 20);
            this.label5.TabIndex = 66;
            this.label5.Text = "Visibility :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(7, 58);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 20);
            this.label3.TabIndex = 65;
            this.label3.Text = "Method name :";
            // 
            // textBox_Methode
            // 
            this.textBox_Methode.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_Methode.Location = new System.Drawing.Point(126, 55);
            this.textBox_Methode.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBox_Methode.Name = "textBox_Methode";
            this.textBox_Methode.Size = new System.Drawing.Size(236, 26);
            this.textBox_Methode.TabIndex = 64;
            // 
            // button_AddMeth
            // 
            this.button_AddMeth.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(213)))), ((int)(((byte)(237)))));
            this.button_AddMeth.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_AddMeth.FlatAppearance.BorderSize = 0;
            this.button_AddMeth.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_AddMeth.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_AddMeth.ForeColor = System.Drawing.Color.Black;
            this.button_AddMeth.Location = new System.Drawing.Point(369, 53);
            this.button_AddMeth.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_AddMeth.Name = "button_AddMeth";
            this.button_AddMeth.Size = new System.Drawing.Size(99, 31);
            this.button_AddMeth.TabIndex = 63;
            this.button_AddMeth.Text = "&Add method";
            this.button_AddMeth.UseVisualStyleBackColor = false;
            this.button_AddMeth.Click += new System.EventHandler(this.button_AddMeth_Click);
            // 
            // FormNewClass
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(134)))), ((int)(((byte)(170)))));
            this.ClientSize = new System.Drawing.Size(496, 560);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.listBox_Meth);
            this.Controls.Add(this.listBox_att);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_Name);
            this.Controls.Add(this.button_Add);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "FormNewClass";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "New Class";
            this.Load += new System.EventHandler(this.FormNewClass_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox_Meth;
        private System.Windows.Forms.ListBox listBox_att;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_Name;
        private System.Windows.Forms.Button button_Add;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox comboBox_att;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_Attribute;
        private System.Windows.Forms.Button button_AddAtt;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox comboBox_meth;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_Methode;
        private System.Windows.Forms.Button button_AddMeth;
    }
}