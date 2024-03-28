using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SHI_UML
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        UMLProject umpr;

        List<DiagClass> diagClasses = new List<DiagClass>();//Collection dyal diagram Class f projet
        List<DiagUseCase> diagUseCases = new List<DiagUseCase>();//Collection dyal diagram use case f projet
        List<DiagSequence> diagSequences = new List<DiagSequence>();//Collection dyal diagram Sequence f projet

        List<Relation> relations = new List<Relation>(); //les relations li mrsomin f workspace
        List<Relation> rects = new List<Relation>();//les fragments li mrsomin f workspace

        private string currentDiag = "";//type dyal diagram li khdamine 3lih (Cla -> Class, Cas -> Use case, Seq -> Sequence)
        private string currentDiagName = "";//smia dyal diagram li khdamin 3lih
        private string currentDiagPath = "";//chemin dyalo f dossier (fin ayt7et lfichier)

        //UR_Diagram class fiha 3 collection (collection dyal les diagrams de class, use case, sequence)
        //kola collection katmtel les etats li daz fihom dak dyagram li khdamine 3lih, bach ila dar undo kanrj3o l'etat lawla , ou ila dar redo kan affichiw l'etat lakhera
        List<UR_Diagram> UR_Diagrams = new List<UR_Diagram>();//collection f l7ala ila kano 3ndna bzzaf dyal les diagram kanb9aw 3a9line 3la les etats dyawlhom ila navigua mne diagram l diagram akhor
        int val = -1;//indice dyal etat li fih diagram (undo -> val--, redo -> val++) zad chi 7aja f workspace -> val++

        private void Form1_Load(object sender, EventArgs e)
        {
            //m3a demarge dyal had form , kan affichiw bach icree un nouveau projet
            FormNewProject F = new FormNewProject();
            F.ShowDialog();//hadi hia li kat afficher 
            if (F.UmlProj != null)
            {
                //ila kan baghi ikhdem 3la projet, kan activiw wla ndesactiviw (buttons,...)
                //diagramToolStripMenuItem.Enabled = true;
                //diagramToolStripMenuItem1.Enabled = true;
                ribBtnNew.Enabled = false;
                ribBtnClose.Enabled = true;
                Save.Enabled = true;
                SaveAll.Enabled = true;
                ribBtnOpen.Enabled = false;
                PanToolBoxs1.Visible = true;
                umpr = F.UmlProj;
                SaveProject();
            }
            else
            {
                ribBtnClose.Enabled = false;
                ribDiagram.Enabled = false;
                ribPanSave.Enabled = false;
                ribPanEdit.Enabled = false;
            }
        }

        private void PanWorkSpace_Paint(object sender, PaintEventArgs e)
        {
            //hna kitrssmo les relations ...
            //kan3aytolha b "PanWorkSpace.Invalidate();"
            try
            {
                //hadi bach maytrach decalge fl7ala ila scrolla
                e.Graphics.TranslateTransform(PanWorkSpace.AutoScrollPosition.X, PanWorkSpace.AutoScrollPosition.Y);
                if (currentDiag == "Cla")
                {
                    e.Graphics.Clear(Color.FromArgb(175, 213, 237));//kanclearew Workspace mne les relations ...
                    for (int i = 0; i < relations.Count; i++)//kandiro bouce 3la les relations dyal diagram li khdamine 3lih
                    {
                        DrawRelationLine(i, new Pen(relations[i].ColorC, 5));//kan3ayto 3la la methode li ghadi trssem lina les relations, ou kan3tiwha l'indice (position) dyal relation f parametre
                    }
                    FillListBox();
                }
                else if (currentDiag == "Cas")
                {
                    e.Graphics.Clear(Color.FromArgb(175, 213, 237));//b7al lfo9
                    e.Graphics.DrawRectangle(new Pen(Color.Black, 5), 250, 20, 800, PanWorkSpace.Height);//kanrssmo dak lmorba3 li iji f lwasset (use case)
                    for (int i = 0; i < relations.Count; i++)//b7al lfo9
                    {
                        DrawRelationActorUseCase(i, new Pen(relations[i].ColorC, 5));//b7al lfo9
                    }
                    FillListBoxUseCase();
                }
                else
                {
                    e.Graphics.Clear(Color.FromArgb(175, 213, 237));//b7al fo9
                    YDrawseq = 300;
                    for (int i = 0; i < relations.Count; i++)//b7al lfo9
                    {
                        DrawRelationSequence(i, new Pen(relations[i].ColorC, 5));//b7al lfo9
                    }
                    for (int i = 0; i < relations.Count; i++)
                    {
                        int cont1 = FindControl(relations[i].NameUC1);//kan9lbo 3la lcontrol1 d relation f position i
                        int cont2 = FindControl(relations[i].NameUC2);//kan9lbo 3la lcontrol2 d relation f position i
                        if (cont1 != -1)//-1 mal9heche , (!= -1) mne ghir (-1) rah l9ah
                        {
                            DrawLifeLine(PanWorkSpace.Controls[cont1]);//ila kan kanrssmo (ligne de vie)
                        }
                        if (cont2 != -1)//b7al lfo9
                        {
                            DrawLifeLine(PanWorkSpace.Controls[cont2]);//b7al lfo9
                        }
                    }
                    for (int i = 0; i < rects.Count; i++)
                    {
                        //kanboucliw f les fragments li 3ndna
                        e.Graphics.DrawRectangle(new Pen(Color.Black, 6), GetRectangle(rects[i].Point1, rects[i].Point2));//kanrassmo lfragment
                    }
                    FillListBoxUseCase();//kan3amro listbox dyal les relations
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region DiagClass
        int nbrpanel = 0;//hada ki mtel l3adad dyal panels + id dyalhoum
        int pa = 0;//hada ki mtel l3adad dyal ga3 les objets (messages,actor, ...) + id dyalhoum
        int clX = 50;//coordonne X dyal Class
        int clY = 50;//coordonne Y dyal Class
        bool dashed = false;//hadi fach katkoun true lkhet ki trssem m9ate3
        //DiagClass dg = new DiagClass();

        private void BtnClass_Click(object sender, EventArgs e)
        {
            AddThings();//hadi katzid f collection dyal UndoRedo (URDiagrams)
            val++;//kanpointiw 3la position dyalo ( fach ndiro undo val--, redo val++)

            FormNewClass at = new FormNewClass();//kandiro instance l Form li ghadi nzido biha lclass
            at.ShowDialog();//kan affichiw lForm
            UmlClass ut = at.ClassUML;//lForm katred lina UmlClass 

            if (ut != null)//ila kan null makanzido walo (null quitta lForm), diffirent de null ya3ni dakhel des details f lForm ou radi trad lina UmlClass 3amra b les donnee
            {
                //kantestew 3la la cordonnee X dyal class li 9bel
                if (clX > 1200)//ila kant fo9 1200
                {
                    clY += 310;//kanrj3o lta7et
                    clX = 50;//ou kanrdo l X dyal class flawel
                }
                nbrpanel++;
                string p = "classUML" + nbrpanel;//kan9ado id dyal class, bach kat3awna n9albo f les relations
                AddUmlClassPanelToWorkspace(p, ut, clX, clY, false, false);//hna kanzido lclass
                clX += 200;//class jaya atcreeya fla coordonnee X + 200, hadechi ila mafateche 1200
                //dg.UmlClasses.Add(new UmlClass {
                //    Name = p,
                //    Text = ut.Text,
                //    Attributs = ut.Attributs,
                //    Methodes = ut.Methodes,
                //});

                AddThings();
            }
        }
        //ta7et, f had code , kankhalte bin class(li huwa obj UML) ou panel (kangoul panel ou ana kan3ni biha class)
        //had la methode katkhlina ncreew class dyalna ou nzidoh f workspace, 
        //had class 3ibara 3la panel wasto: 
        //(Label li kan7to fih smia d class) 
        //+ 2 (DatagridView li kan affichiw fihoum les attributs et les methodes) 
        //+ (button d supprimer bach kanmss7o la class et les relations dyalha m3a d'autre class)
        private string AddUmlClassPanelToWorkspace(string panelName, UmlClass ut, int px, int py, bool hideatt, bool hidemeth)//kan ajoutiw class dyalna
        {
            Panel p = new Panel();//kancriyiw panel jdida...
            p.BackColor = Color.Gray;//kan3tiw Color
            p.Name = panelName;////Name
            p.Size = new Size(160, 115);
            p.Location = new Point(px, py);
            p.BackColor = Color.FromArgb(38, 134, 170);
            p.AllowDrop = true;//hadi daroriya tkoun true, bach panel li drna mno drag, i9der idir drop f l'autre panel
            //had events li lta7et daroriyin, huma li kikhliwna ndiro les relation entre les class
            p.DragDrop += new DragEventHandler(Panel_DragDrop);//event 
            p.DragEnter += new DragEventHandler(Panel_DragEnter);//event
            p.MouseDown += new MouseEventHandler(Panel_MouseDown);//event
            Button b = new Button();//Button bache issuprimi panel
            b.Text = "X";
            b.Size = new Size(25, 25);
            b.Location = new Point(132, 32);
            b.ForeColor = Color.White;
            b.BackColor = Color.Red;
            b.Click += new EventHandler(buttonDeleteClass_Click);//event
            Program.TooltipObj(b, "Delete " + ut.Name);//ToolTip
            Label l = new Label();//label fin itafficha smia dyal Table
            Program.TooltipObj(l, "Class " + ut.Name + ", Double Click to change name");//ToolTip
            l.BackColor = Color.WhiteSmoke;
            l.Text = ut.Text;//smiya dyal Table
            l.Size = new Size(160, 30);
            l.Cursor = System.Windows.Forms.Cursors.SizeAll;
            l.TextAlign = ContentAlignment.MiddleCenter;
            l.Font = new Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            l.Dock = DockStyle.Top;
            l.DoubleClick += new EventHandler(Label_DoubleClick);//event
            l.MouseMove += new MouseEventHandler(Label_MouseMove);//event
            l.MouseDown += new MouseEventHandler(Label_MouseDown);//event
            l.MouseUp += new MouseEventHandler(Label_MouseUp);//event
            DataGridView D_att = new DataGridView();
            DataGridView D_meth = new DataGridView();
            D_att.BackgroundColor = Color.FromArgb(74, 174, 213);
            D_meth.BackgroundColor = Color.FromArgb(74, 174, 213);
            if (hideatt)
            {
                D_att.Columns.Add("att" + nbrpanel, @"Attributes\/");
                D_att.Size = new Size(148, 20);
                D_meth.Location = new Point(6, 80);
            }
            else
            {
                D_att.Columns.Add("att" + nbrpanel, @"Attributes/\");
                D_att.Size = new Size(148, 140);
                D_meth.Location = new Point(6, 200);
                p.Height += 115;
            }
            if (hidemeth)
            {
                D_meth.Columns.Add("meth" + nbrpanel, @"Methodes\/");
                D_meth.Size = new Size(148, 20);
            }
            else
            {
                D_meth.Columns.Add("meth" + nbrpanel, @"Methodes/\");
                D_meth.Size = new Size(148, 98);
                p.Height += 70;
            }
            D_att.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(HideShowatt);
            D_meth.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(HideShowMeth);
            D_att.Location = new Point(6, 60);
            D_att.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            D_meth.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            D_att.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            D_meth.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            D_att.RowHeadersVisible = false;
            D_meth.RowHeadersVisible = false;
            for (int i = 0; i < ut.Attributs.Count; i++)
            {
                D_att.Rows.Add(ut.Attributs[i]);//kan3mro dataGridview1 b les attributs
            }
            for (int i = 0; i < ut.Methodes.Count; i++)
            {
                D_meth.Rows.Add(ut.Methodes[i]);//kan3mro dataGridview2 b les methodes
            }
            p.Controls.Add(b);//kanzido l button f panel dyalna , button 3ando position 0 f lclass
            p.Controls.Add(l);//kanzido l label f panel dyalna, ......... 1
            p.Controls.Add(D_att);//kanzido l dataGridview1 f panel dyalna , ......... 2
            p.Controls.Add(D_meth);//kanzido l dataGridview2 f panel dyalna , ......... 3
            PanWorkSpace.Controls.Add(p);//kan ajoutiw had panel f workspace
            return p.Name;
        }

        #region events dyal class li kanzido

        private void HideShowatt(object sender, DataGridViewCellMouseEventArgs e)//Afficher ou masquer les Attributs
        {
            DataGridView dgce = (DataGridView)sender;//DataGridView lideclonchat levent
            if (dgce.Columns[0].HeaderText == @"Attributes/\")//kantestew wache nmasquiw les attribut
            {
                dgce.Columns[0].HeaderText = @"Attributes\/";//ila clicka mra jaya i taffichaw
                dgce.Size = new Size(148, 20);//kanbdlo size bache tban bli tmaskaw
                dgce.Parent.Height += -115;//kan9sso height dyal Table li huwa Parent dyal had DataGridView
                dgce.Parent.Controls[3].Location = new Point(6, 80);//dataGridView dyal les methode kanbdloliha location
            }
            else//wla n affichiw les attributs
            {
                dgce.Columns[0].HeaderText = @"Attributes/\";//ila clicka mra jaya i tmaskaw
                dgce.Size = new Size(148, 140);//kanrdo size bache tban bli taffichat
                dgce.Parent.Height += 115;//kanzido height dyal Table li huwa Parent dyal had DataGridView
                dgce.Parent.Controls[3].Location = new Point(6, 200);//kanrdo location dyal dataGridView Kima kant
            }
        }

        private void HideShowMeth(object sender, DataGridViewCellMouseEventArgs e)//Afficher ou masquer les Methodes
        {
            DataGridView dgce = (DataGridView)sender;//DataGridView lideclonchat levent
            if (dgce.Columns[0].HeaderText == @"Methodes/\")//kantestew wache nmasquiw les Methodes
            {
                dgce.Columns[0].HeaderText = @"Methodes\/";//ila clicka mra jaya i taffichaw
                dgce.Size = new Size(148, 20);//kanbdlo size bache tban bli tmaskaw
                dgce.Parent.Height += -70;//kan9sso height dyal Table li huwa Parent dyal had DataGridView
            }
            else//wla n affichiw les Methodes
            {
                dgce.Columns[0].HeaderText = @"Methodes/\";//ila clicka mra jaya i tmaskaw
                dgce.Size = new Size(148, 98);//kanrdo size bache tban bli taffichat
                dgce.Parent.Height += 70;//kanzido height dyal Table li huwa Parent dyal had DataGridView
            }
        }

        private void buttonDeleteClass_Click(object sender, EventArgs e)
        {
            AddThings();
            val++;

            Button b = (Button)sender;//Button li decloncha levent
            List<int> li = FindDrawLinesByName(b.Parent.Name);//kanjibo les realtions dyal Panel li 3ndo
            for (int i = 0; i < li.Count; i++)
            {
                //hna kanboucliw 3la ga3 les relations
                int a = FindDrawLinesByName1(b.Parent.Name);//hadi daymen katjib lawla fihom (katjib wa7da)
                relations.RemoveAt(a);//kanmss7o dik lwa7da li jabt
                //ou kat3awed la boucle , mdertche RemoveAt(i); 7it at3ti bug d out of range
            }
            PanWorkSpace.Controls.Remove(b.Parent);//kanmss7o had control (class) mne Workspace
            PanWorkSpace.Invalidate();//kan7awlo n3awdo nrssmo les relations, (ila makant ta relations , maghadi itrssem walo!)

            AddThings();
        }

        #region kanrssmo lrelation bin class1 ou class2

        private void Panel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                Panel p = (Panel)sender;//panel li bda event
                p.DoDragDrop(p, DragDropEffects.Move);//enfois ki bghi iglissi bache idir DraAndDrop, kanpassiw had panel kamel, lfin ghadi iglissiha
            }
        }

        private void Panel_DragEnter(object sender, DragEventArgs e)
        {
            //darori AllowDrop = true
            e.Effect = DragDropEffects.Move;//kan3tiw wa7ed sora bli i9der idropi had lpanel f panel akhor
        }

        private void Panel_DragDrop(object sender, DragEventArgs e)
        {
            //darori AllowDrop = true
            Panel p = (Panel)e.Data.GetData(typeof(Panel));//fache ki droppi panel, kanjibo Data li seftnaha, t9der tkoun ay objet, f had l7ala hia panel
            Panel b = (Panel)sender;//panel li fin dropina
            int a = FindDrawLinesByNames(p.Name, b.Name);
            if (a == -1)
            {
                Color Clr = new Color();//Objet Color
                ColorDialog cd = new ColorDialog();
                if (cd.ShowDialog() == DialogResult.OK)
                {
                    AddThings();
                    val++;

                    Clr = cd.Color;//kan7ato Color li jbna mne dialog f Objet dyalna
                    Relation R = new Relation  //kanajoutiw drawline mne panel lawel, li panel li tdropa fih, ou kankhdo location dyalhoum, bache nrssmo relation binathoum
                    {
                        NameUC1 = p.Name,
                        NameUC2 = b.Name,
                        Point1 = p.Location,
                        Point2 = b.Location,
                        Dashed = dashed,
                        ColorC = Clr,
                    };
                    if (p.Location.X < b.Location.X)
                    {
                        R.NameUO1 = AddLabelMultip("LabMultip" + (pa++), "0..1", p.Location.X + p.Width + 10, p.Location.Y + Convert.ToInt32(p.Height / 2) - 25);
                        R.NameUO2 = AddLabelMultip("LabMultip" + (pa++), "0..1", b.Location.X - 50, b.Location.Y + Convert.ToInt32(b.Height / 2) - 25);
                    }
                    else
                    {
                        R.NameUO1 = AddLabelMultip("LabMultip" + (pa++), "0..1", p.Location.X - 50, p.Location.Y + Convert.ToInt32(p.Height / 2) - 25);
                        R.NameUO2 = AddLabelMultip("LabMultip" + (pa++), "0..1", b.Location.X + b.Width + 10, b.Location.Y + Convert.ToInt32(b.Height / 2) - 25);
                    }
                    relations.Add(R);

                    AddThings();
                }

            }
            PanWorkSpace.Invalidate();//kan3ayto event Paint bache irssemles les relations

        }

        #endregion

        #region kan7rko Table dyalna

        private Point MouseDownLocation;

        private void Label_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MouseDownLocation = e.Location;
            }
        }

        private void Label_MouseMove(object sender, MouseEventArgs e)
        {
            Label L = (Label)sender;
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {//hadechi mne stack over flow, bache n7arko panel(class)
                List<int> p1 = FindDrawLinesByNameUC1(L.Parent.Name);
                List<int> p2 = FindDrawLinesByNameUC2(L.Parent.Name);
                for (int i = 0; i < p1.Count; i++)
                {
                    if (p1[i] != -1)
                    {
                        Relation r1 = relations[p1[i]];
                        int c1 = FindControl(r1.NameUO1);
                        if (c1 != -1)
                        {
                            Control C = PanWorkSpace.Controls[c1];
                            C.Left = e.X + C.Left - MouseDownLocation.X;
                            C.Top = e.Y + C.Top - MouseDownLocation.Y;
                        }
                    }
                }
                for (int i = 0; i < p2.Count; i++)
                {
                    if (p2[i] != -1)
                    {
                        Relation r2 = relations[p2[i]];
                        int c2 = FindControl(r2.NameUO2);
                        if (c2 != -1)
                        {
                            Control C = PanWorkSpace.Controls[c2];
                            C.Left = e.X + C.Left - MouseDownLocation.X;
                            C.Top = e.Y + C.Top - MouseDownLocation.Y;
                        }
                    }
                }
                L.Parent.Left = e.X + L.Parent.Left - MouseDownLocation.X;
                L.Parent.Top = e.Y + L.Parent.Top - MouseDownLocation.Y;
                MoveLines(L.Parent.Name);//mne ba3ed ma kan7rko Table , kan7ko ta les relation li 3andha m3atables lokherine
            }
        }

        private void Label_MouseUp(object sender, MouseEventArgs e)
        {
            AddThings();
            val++;

            Label L = (Label)sender;
            //int c = FindUCT(L.Parent.Name);
            //if (c != -1)
            //{
            //    collUCT.ListCT[c].X = L.Parent.Location.X;
            //    collUCT.ListCT[c].Y = L.Parent.Location.Y;
            //}
            MoveLines(L.Parent.Name);//refresh les relations

            AddThings();
        }

        #endregion

        #endregion

        #region find relations and controls
        private List<int> FindDrawLinesByName(string X)//kan3tiw smiya dyal panel, ou kanjobo ga3 les relations dyalhad panel
        {
            List<int> li = new List<int>();
            for (int i = 0; i < relations.Count; i++)
            {
                if ((relations[i].NameUC1 == X) || (relations[i].NameUC2 == X))//ila kant had smiya f Name1, Name2, kanzidoha f List
                {
                    li.Add(i);//kanzido la position dyalpanel li l9inah f had list (i position dyalo f la class)
                }
            }
            return li;//kan returniw List
        }

        private int FindDrawLinesByName1(string name)//kan3tiw smiya dyal panel, ou kanjobo ga3 les relations dyalhad panel
        {
            for (int i = 0; i < relations.Count; i++)
            {
                if ((relations[i].NameUC1 == name) || (relations[i].NameUC2 == name))
                {
                    return i;//kan returniw position fach kanl9aweh
                }
            }
            return -1;
        }
        private List<int> FindDrawLinesByNameUC1(string name)//kan3tiw smiya dyal panel, ou kanjobo ga3 les relations dyalhad panel
        {
            List<int> li = new List<int>();
            for (int i = 0; i < relations.Count; i++)
            {
                if (relations[i].NameUC1 == name)
                {
                    li.Add(i);
                }
            }
            return li;
        }
        private List<int> FindDrawLinesByNameUC2(string name)//kan3tiw smiya dyal panel, ou kanjobo ga3 les relations dyalhad panel
        {
            List<int> li = new List<int>();
            for (int i = 0; i < relations.Count; i++)
            {
                if (relations[i].NameUC2 == name)
                {
                    li.Add(i);
                }
            }
            return li;
        }

        private int FindDrawLinesByNames(string Name1, string Name2)
        {
            for (int i = 0; i < relations.Count; i++)
            {
                if (((relations[i].NameUC1 == Name1) && (relations[i].NameUC2 == Name2)) || ((relations[i].NameUC1 == Name2) && (relations[i].NameUC2 == Name1)))
                {
                    return i;
                }
            }
            return -1;
        }

        private int FindControl(string X)//kan3tiw smiya dyal panel
        {
            for (int i = 0; i < PanWorkSpace.Controls.Count; i++)
            {
                if (PanWorkSpace.Controls[i].Name == X)
                {
                    return i;//ila l9ah, kanreturniw position dyalo
                }
            }
            return -1;//ila mal9ache kanreturniw -1
        }
        #endregion

        private void Label_DoubleClick(object sender, EventArgs e)//event duble click bache nmodifiw smia dyal table
        {
            AddThings();
            val++;

            Label L = (Label)sender;//kanjibo label li declocha l event
            FormChangeText G = new FormChangeText(L.Text);//kan3ayto l Form , ou kan3tiwha smiya li f had label daba 
            G.ShowDialog();//kan affichiw lform
            L.Text = G.TextValue;//kanjibo smia li dakhelna fdik lform, ou kan7toha f Text dyal Label

            Program.TooltipObj(L, " " + L.Text + ", Double Click to change text");

            AddThings();
        }
        private void Label_DoubleClickMultip(object sender, EventArgs e)//event duble click bache nmodifiw smia dyal table
        {
            AddThings();
            val++;

            Label L = (Label)sender;//kanjibo label li declocha l event
            FormMultip G = new FormMultip();//kan3ayto l Form , ou kan3tiwha smiya li f had label daba 
            G.ShowDialog();//kan affichiw lform
            L.Text = G.TextValue;//kanjibo smia li dakhelna fdik lform, ou kan7toha f Text dyal Label

            AddThings();
        }

        private void MoveLines(string Name)//methode bache n7rko les relation bin les table , 3ndha comme paramametre Name, had name dyalpanel li kan7rko
        {
            //panel -> Table
            //Name panel li 7rknah, kan7to Name dyalo
            try
            {
                List<int> li = FindDrawLinesByName(Name);//kan9lbo 3la les relations dyal had panel m3a panels lokherine
                for (int i = 0; i < li.Count; i++)
                {
                    int p1 = FindControl(relations[li[i]].NameUC1);//kan9lbo 3la panel lawel
                    int p2 = FindControl(relations[li[i]].NameUC2);//kan9lbo 3la panel li mlier m3a lawel
                    int c1 = FindControl(relations[li[i]].NameUO1);//kan9lbo 3la label d panel lawel
                    int c2 = FindControl(relations[li[i]].NameUO2);//kan9lbo 3la label d panel li mlier m3a lawel
                    if (p1 != -1 && p2 != -1)
                    {
                        string nuo1 = "";
                        string nuo2 = "";
                        Control P1 = PanWorkSpace.Controls[p1];//kan pointiw 3la panel Lawel
                        Control P2 = PanWorkSpace.Controls[p2];//kan pointiw 3la panel li mliyi m3a lawel
                        if (c1 != -1)
                        {
                            nuo1 = PanWorkSpace.Controls[c1].Name;//kan pointiw 3la label d panel Lawel
                        }
                        if (c2 != -1)
                        {
                            nuo2 = PanWorkSpace.Controls[c2].Name;//kan pointiw 3la label d panel li mliyi m3a lawel
                        }
                        relations.Add(new Relation
                        {
                            NameUC1 = P1.Name,
                            NameUC2 = P2.Name,
                            NameUO1 = nuo1,
                            NameUO2 = nuo2,
                            Point1 = P1.Location,
                            Point2 = P2.Location,
                            ColorC = relations[li[i]].ColorC,
                            Dashed = relations[li[i]].Dashed
                        });//kanzido f drawlines bache n9dro nrssmoha f event Paint dyal panel2(panel li kanrssmo fih les relation)
                    }
                    relations.Remove(relations[li[i]]);//ila kan ki 7rek panel(Table) kanmss7o had relation, bache n3awdo nrsmoha f la nouvelle position 
                    PanWorkSpace.Invalidate();//kan 3yto l event Paint dyal panel2 (bache irsmlina ga3 les relations li kaynine f collection drawlines)
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DrawRelationLine(int i, Pen pen)//Methode kanrssmo biha Relation bin P1 ou P2 li f fdrawline li 3ndo position i
        {
            try
            {
                int c1 = FindControl(relations[i].NameUC1);//kan9lbo 3la table dyalna (Panel)
                int c2 = FindControl(relations[i].NameUC2);//kan9lbo 3la table dyalna (Panel)
                if (c1 == -1 || c2 == -1)//ila mal9inache chi wa7edfihoum kanmss7o drawline
                {
                    relations.Remove(relations[i]);
                }
                else
                {
                    if (relations[i].Dashed)
                    {
                        float[] dashValues = { 5, 5, 5, 5 };//hado les valeurs bach nrssmo khet mt9ate3
                        pen.DashPattern = dashValues;//kandawzo douk les valeur l pen li ghadi nrassmo bih f Workspace
                    }
                    //imageA chare7 fiha hadechi li hna
                    int wi1 = PanWorkSpace.Controls[c1].Size.Width; int hei1 = PanWorkSpace.Controls[c1].Size.Height;//kanjibo lwidth ou lheught dyal les panelli binathoum lrelation
                    int wi2 = PanWorkSpace.Controls[c2].Size.Width; int hei2 = PanWorkSpace.Controls[c2].Size.Height;//        //        //        //
                    int valWi1 = Convert.ToInt32(wi1 / 2); int valhei1 = Convert.ToInt32(hei1 / 2);//kan7asbo ness dyalhom bach nrssmo l relation mne wasset table
                    int valWi2 = Convert.ToInt32(wi2 / 2); int valhei2 = Convert.ToInt32(hei2 / 2);//        //        //        //
                    if (relations[i].Point1.Y != relations[i].Point2.Y)//ila kan mkhtalfin f Y, kandiro des calculs bache nrssmo wa7ed line maykouneche mayel
                    {
                        if (((relations[i].Point1.Y + 200) > (relations[i].Point2.Y)) && ((relations[i].Point1.Y) < (relations[i].Point2.Y + 150)))//ila kano les table bjouj 9rab mne Y
                        {
                            int x1 = Convert.ToInt32((relations[i].Point2.X + valWi2 - relations[i].Point1.X + valWi1) / 2) + relations[i].Point1.X;//x hia ness dyal lmassfa bih had jouj les tables
                            Point p1 = new Point(relations[i].Point1.X + valWi1, relations[i].Point1.Y + valhei1);//+110,+190 kan7awlo nbdaw relation mne wasset table, machi mne rass dyalo
                            Point p2 = new Point(x1, relations[i].Point1.Y + valhei1);
                            PanWorkSpace.CreateGraphics().DrawLine(pen, p1, p2);//kanrssmo line lawel bin table1 ta lness d lmasafa li binha ou bin table jouj (le cas dyal ma3ndhoumeche nfess Y)
                            Point p3 = new Point(x1, relations[i].Point2.Y + valhei2);
                            Point p4 = new Point(relations[i].Point2.X + valWi2, relations[i].Point2.Y + valhei2);
                            PanWorkSpace.CreateGraphics().DrawLine(pen, p3, p4);//kanrssmo line lawel bin ness d lmasafa li bin table1 ou table2 tal ltable2
                            PanWorkSpace.CreateGraphics().DrawLine(pen, p2, p3);//kanrssmo line akhore bache irbet dik ness lmassfa (ki rssem Y)
                        }
                        else // ila kano b3ad (wa7ed habet bzaf 3la lkhor)
                        {
                            //kandiro na9ta li ghadi tliyer binathoum mne ou 3andha X dyal P1 ou Y dyal P2
                            Point p1 = new Point(relations[i].Point1.X + valWi1, relations[i].Point1.Y + valhei1);
                            Point p2 = new Point(relations[i].Point1.X + valWi1, relations[i].Point2.Y + valhei2);//hia hadi
                            Point p3 = new Point(relations[i].Point2.X + valWi2, relations[i].Point2.Y + valhei2);
                            PanWorkSpace.CreateGraphics().DrawLine(pen, p1, p2);
                            PanWorkSpace.CreateGraphics().DrawLine(pen, p2, p3);
                        }

                    }
                    else if ((relations[i].Point1.X == relations[i].Point2.X) && (relations[i].Point1.Y == relations[i].Point2.Y))//kanrssmo Relation Avec lui meme
                    {
                        int xP1 = relations[i].Point1.X + valWi1 - 20;
                        int yP1 = relations[i].Point1.Y + valhei1 + 50;
                        Point p1 = new Point(xP1, yP1);
                        Point p2 = new Point(xP1, yP1 - 210);
                        Point p3 = new Point(xP1 - 150, yP1 - 210);
                        Point p4 = new Point(xP1 - 150, yP1 - 50);
                        PanWorkSpace.CreateGraphics().DrawLine(pen, p1, p2);
                        PanWorkSpace.CreateGraphics().DrawLine(pen, p2, p3);
                        PanWorkSpace.CreateGraphics().DrawLine(pen, p3, p4);
                        PanWorkSpace.CreateGraphics().DrawLine(pen, p4, new Point(p1.X, p1.Y - 50));

                    }
                    else
                    {
                        //ila kan 3ndhoum neffes Y, kanrssmo relation direct bin les tables bjouj
                        Point p1 = new Point(relations[i].Point1.X + valWi1, relations[i].Point1.Y + valhei1);
                        Point p2 = new Point(relations[i].Point2.X + valWi2, relations[i].Point2.Y + valhei2);
                        PanWorkSpace.CreateGraphics().DrawLine(pen, p1, p2);//kanrssmo relation
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FillListBox()
        {
            listBox_Relations.Items.Clear();//kan cleariw ListBox bache mayt3awdouche
            for (int i = 0; i < relations.Count; i++)
            {
                Relation d = relations[i];
                int c1 = FindControl(d.NameUC1);//kan9lbo3la lpanel li f drawline 
                int c2 = FindControl(d.NameUC2);//   //   //   //
                if (c1 == -1 || c2 == -1)//ila mal9aheche
                {
                    relations.Remove(d);//kanmss7o drawline
                }
                else
                {
                    //l9ah kanzidoh flistbox,
                    //kanzido smiya dyal label d panel1(Table1) ou smiya dlabel d panel2(Table2)
                    //label kayen f position 1 dyal controls d panel (Table)
                    listBox_Relations.Items.Add(PanWorkSpace.Controls[c1].Controls[1].Text + " ---> " + PanWorkSpace.Controls[c2].Controls[1].Text);
                }
            }
        }

        private void AddPanelObject(string name, int X, int Y)//kan ajoutiw panel dyala agregat f panel2 (fin kanrsmo diagram), location (X, Y), bache fach ndiro Open njibo lcation dyalo li kan f lawel
        {
            Panel p1 = new Panel();
            p1.Size = new Size(60, 36);
            p1.Location = new Point(X, Y);
            p1.Cursor = System.Windows.Forms.Cursors.SizeAll;
            p1.ContextMenuStrip = contextMenuStrip2;
            p1.Name = name;
            //p1.MouseDown += new MouseEventHandler(labelpanel_MouseDown);//click droit ila bgha imsse7 l'objet
            p1.Paint += new PaintEventHandler(panel_Paint);
            ControlExtension.Draggable(p1, true);
            p1.Invalidate();
            PanWorkSpace.Controls.Add(p1);
        }

        private void BtnAgrega_Click(object sender, EventArgs e)
        {
            AddThings();
            val++;

            pa++;
            AddPanelObject("PanAgrega" + pa, 20, 20);

            AddThings();
        }

        private void BtnCompo_Click(object sender, EventArgs e)
        {
            AddThings();
            val++;

            pa++;
            AddPanelObject("PanCompo" + pa, 20, 20);

            AddThings();
        }

        private void BtnDepend_Click(object sender, EventArgs e)
        {
            AddThings();
            val++;

            pa++;
            AddPanelObject("PanDepend" + pa, 20, 20);

            AddThings();
        }

        private void BtnInher_Click(object sender, EventArgs e)
        {
            AddThings();
            val++;

            pa++;
            AddPanelObject("PanInher" + pa, 20, 20);

            AddThings();
        }

        private void BtnMultip_Click(object sender, EventArgs e)
        {
            AddThings();
            val++;

            pa++;
            AddLabelMultip("LabMultip" + pa, "0..1", 30, 10);//kan ajoutiw label dyal Enumeration f panel2 location 30,10 / 3la 7assab label li khtar

            AddThings();
        }

        private string AddLabelMultip(string name, string Text, int X, int Y)//kan ajoutiw label dyal enumeration fin kanrssmo diagram nefss lblan m3a location (X, Y)...
        {
            Label l = new Label
            {
                Text = Text,
                Location = new Point(X, Y),
                Cursor = System.Windows.Forms.Cursors.SizeAll,
                AutoSize = true,
                Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                Name = name,
                ContextMenuStrip = contextMenuStrip2,
            };
            l.DoubleClick += new EventHandler(Label_DoubleClickMultip);//double click bache i9der ibdel smiya 
            //l.MouseDown += new MouseEventHandler(labelpanel_MouseDown);//click droit ila bgha imsse7 l'objet 
            l.MouseMove += new MouseEventHandler(Label_MouseMoveM);//event
            l.MouseDown += new MouseEventHandler(Label_MouseDownM);//event
            PanWorkSpace.Controls.Add(l);
            return l.Name;
        }
        private string AddLabelTextTochange(string name, string Text, int X, int Y)//kan ajoutiw label dyal enumeration fin kanrssmo diagram nefss lblan m3a location (X, Y)...
        {
            Label l = new Label
            {
                Text = Text,
                Location = new Point(X, Y),
                Cursor = System.Windows.Forms.Cursors.SizeAll,
                AutoSize = true,
                Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                Name = name,
                ContextMenuStrip = contextMenuStrip2,
            };
            l.DoubleClick += new EventHandler(Label_DoubleClick);//double click bache i9der ibdel smiya 
            l.MouseMove += new MouseEventHandler(Label_MouseMoveM);//event
            l.MouseDown += new MouseEventHandler(Label_MouseDownM);//event
            PanWorkSpace.Controls.Add(l);
            return l.Name;
        }

        private void panel_Paint(object sender, PaintEventArgs e)
        {
            Panel p = (Panel)sender;//panel li decloche event
            int w = Convert.ToInt32(p.Size.Width / 2);
            int h = Convert.ToInt32(p.Size.Height / 2);
            Point[] ps = {//Collection dyal les points bache nrssmo agrgation (wla composition)
                new Point(1, h),
                new Point(w, 0),
                new Point(p.Size.Width -1, h),
                new Point(w, p.Size.Height -1),
            };
            Point[] ps1 = {//Collection dyal les points bache nrssmo heiritage (wla Dependence)
                new Point(w, 0),
                new Point(p.Size.Width -1, p.Size.Height -1),
                new Point(0, p.Size.Height -1),
            };
            if (p.Name.Substring(0, 4) == "PanA")//hadi Composition
            {
                e.Graphics.DrawPolygon(new Pen(Color.Black, 6), ps);
            }
            else if (p.Name.Substring(0, 4) == "PanC")
            {
                e.Graphics.FillPolygon(Brushes.Black, ps);
                e.Graphics.DrawPolygon(new Pen(Color.Black, 6), ps);
            }
            else if (p.Name.Substring(0, 4) == "PanD")
            {
                e.Graphics.DrawPolygon(new Pen(Color.Black, 6), ps1);
            }
            else if (p.Name.Substring(0, 4) == "PanI")
            {
                e.Graphics.FillPolygon(Brushes.Black, ps1);
                e.Graphics.DrawPolygon(new Pen(Color.Black, 6), ps1);
            }
        }
        
        #endregion

        #region Diagram Use Case

        private void ActorUseCase_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                Panel p = (Panel)sender;//panel li bda event
                p.DoDragDrop(p.Name, DragDropEffects.Move);//enfois ki bghi iglissi bache idir DraAndDrop, kanpassiw had panel kamel, lfin ghadi iglissiha
            }
        }

        private void ActorUseCase_DragEnter(object sender, DragEventArgs e)
        {
            //darori AllowDrop = true
            e.Effect = DragDropEffects.Move;//kan3tiw wa7ed sora bli i9der idropi had lpanel f panel akhor
        }

        private void ActorUseCase_DragDrop(object sender, DragEventArgs e)
        {
            //darori AllowDrop = true
            string pa = (string)e.Data.GetData(typeof(string));
            int cont = FindControl(pa);
            Control p = PanWorkSpace.Controls[cont];
            bool dash;
            if (PanWorkSpace.Controls[cont].GetType().Name == "Panel")
            {
                dash = dashed;
            }
            else
            {
                dash = true;
            }
            //fache ki droppi panel, kanjibo Data li seftnaha, t9der tkoun ay objet, f had l7ala hia panel

            Control b = (Control)sender;//panel li fin dropina
            int a = FindDrawLinesByNames(p.Name, b.Name);
            if (a == -1)
            {
                Color Clr = new Color();//Objet Color
                ColorDialog cd = new ColorDialog();
                if (cd.ShowDialog() == DialogResult.OK)
                {
                    AddThings();
                    val++;

                    Clr = cd.Color;//kan7ato Color li jbna mne dialog f Objet dyalna
                    Relation R = new Relation  //kanajoutiw drawline mne panel lawel, li panel li tdropa fih, ou kankhdo location dyalhoum, bache nrssmo relation binathoum
                    {
                        NameUC1 = p.Name,
                        NameUC2 = b.Name,
                        Point1 = p.Location,
                        Point2 = b.Location,
                        Dashed = dash,
                        ColorC = Clr,
                    };
                    relations.Add(R);

                    AddThings();
                }

            }
            makerelation = false;
            PanWorkSpace.Invalidate();//kan3ayto event Paint bache irssemles les relations
        }

        private void DrawRelationActorUseCase(int i, Pen pen)
        {
            try
            {
                //DrawArrow(pen);
                int c1 = FindControl(relations[i].NameUC1);//kan9lbo 3la table dyalna (Panel)
                int c2 = FindControl(relations[i].NameUC2);//kan9lbo 3la table dyalna (Panel)
                if (c1 == -1 || c2 == -1)//ila mal9inache chi wa7edfihoum kanmss7o drawline
                {
                    relations.Remove(relations[i]);
                }
                else
                {
                    if (relations[i].Dashed)
                    {
                        float[] dashValues = { 5, 5, 5, 5 };
                        pen.DashPattern = dashValues;
                    }
                    int wi1 = PanWorkSpace.Controls[c1].Size.Width; int hei1 = PanWorkSpace.Controls[c1].Size.Height;//kanjibo lwidth ou lheught dyal les panelli binathoum lrelation
                    int wi2 = PanWorkSpace.Controls[c2].Size.Width; int hei2 = PanWorkSpace.Controls[c2].Size.Height;//        //        //        //
                    int valWi1 = Convert.ToInt32(wi1 / 2); int valhei1 = Convert.ToInt32(hei1 / 2);//kan7asbo ness dyalhom bach nrssmo l relation mne wasset table
                    int valWi2 = Convert.ToInt32(wi2 / 2); int valhei2 = Convert.ToInt32(hei2 / 2);//        //        //        //

                    //kanrssmo relation direct bin les tables bjouj
                    Point p1 = new Point(relations[i].Point1.X + valWi1, relations[i].Point1.Y + valhei1);
                    Point p2 = new Point(relations[i].Point2.X + valWi2, relations[i].Point2.Y + valhei2);
                    PanWorkSpace.CreateGraphics().DrawLine(pen, p1, p2);//kanrssmo relation
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FillListBoxUseCase()
        {
            listBox_Relations.Items.Clear();//kan cleariw ListBox bache mayt3awdouche
            for (int i = 0; i < relations.Count; i++)
            {
                Relation d = relations[i];
                int c1 = FindControl(d.NameUC1);//kan9lbo3la lpanel li f drawline 
                int c2 = FindControl(d.NameUC2);//   //   //   //
                if (c1 == -1 || c2 == -1)//ila mal9aheche
                {
                    relations.Remove(d);//kanmss7o drawline
                }
                else
                {
                    string text1 = "";
                    string text2 = "";
                    if (PanWorkSpace.Controls[c1].GetType().Name == "Panel")
                    {
                        text1 = PanWorkSpace.Controls[c1].Controls[0].Text;
                    }
                    else
                    {
                        text1 = PanWorkSpace.Controls[c1].Text;
                    }
                    if (PanWorkSpace.Controls[c2].GetType().Name == "Panel")
                    {
                        text2 = PanWorkSpace.Controls[c2].Controls[0].Text;
                    }
                    else
                    {
                        text2 = PanWorkSpace.Controls[c2].Text;
                    }
                    //l9ah kanzidoh flistbox,
                    //kanzido smiya dyal label d panel1(Table1) ou smiya dlabel d panel2(Table2)
                    //label kayen f position 1 dyal controls d panel (Table)
                    listBox_Relations.Items.Add(text1 + " ---> " + text2);
                }
            }
        }


        private Point MouseDownLocation1;

        private void Actor_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MouseDownLocation1 = e.Location;
            }
        }

        private void Actor_MouseMove(object sender, MouseEventArgs e)
        {
            Label L = (Label)sender;
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {

                //hadechi mne stack over flow, bache n7arko panel(class)
                L.Parent.Left = e.X + L.Parent.Left - MouseDownLocation1.X;
                L.Parent.Top = e.Y + L.Parent.Top - MouseDownLocation1.Y;
                MoveLinesActorUseCase(L.Parent.Name);
                //mne ba3ed ma kan7rko Table , kan7ko ta les relation li 3andha m3atables lokherine

            }
        }

        private void Actor_MouseUp(object sender, MouseEventArgs e)
        {
            AddThings();
            val++;
            Label L = (Label)sender;
            MoveLinesActorUseCase(L.Parent.Name);
            //MoveLines(L.Parent.Name);//refresh les relations
            AddThings();
        }


        private Point MouseDownLocation2;


        private bool makerelation = false;

        private void UseCase_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (makerelation)
                {
                    Label p = (Label)sender;
                    p.DoDragDrop(p.Name, DragDropEffects.Move);
                }
                else
                {
                    MouseDownLocation2 = e.Location;
                }
            }
        }

        private void UseCase_MouseMove(object sender, MouseEventArgs e)
        {
            Label L = (Label)sender;
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                //hadechi mne stack over flow, bache n7arko panel(class)
                L.Left = e.X + L.Left - MouseDownLocation2.X;
                L.Top = e.Y + L.Top - MouseDownLocation2.Y;
                MoveLinesActorUseCase(L.Name);//mne ba3ed ma kan7rko Table , kan7ko ta les relation li 3andha m3atables lokherine
            }
        }
        
        private void LabelUseCase_Paint(object sender, PaintEventArgs e)
        {
            Control c = sender as Control;
            Rectangle r = new Rectangle(0, 0, c.Width - 2, c.Height - 2);
            e.Graphics.DrawEllipse(new Pen(Color.Black, 5), r);
        }

        private void MoveLinesActorUseCase(string Name)//methode bache n7rko les relation bin les table , 3ndha comme paramametre Name, had name dyalpanel li kan7rko
        {
            //panel -> Table
            //Name panel li 7rknah, kan7to Name dyalo
            try
            {
                List<int> li = FindDrawLinesByName(Name);//kan9lbo 3la les relations dyal had panel m3a panels lokherine
                for (int i = 0; i < li.Count; i++)
                {
                    int p1 = FindControl(relations[li[i]].NameUC1);//kan9lbo 3la panel lawel
                    int p2 = FindControl(relations[li[i]].NameUC2);//kan9lbo 3la panel li mlier m3a lawel
                    if (p1 != -1 && p2 != -1)
                    {
                        Control P1 = PanWorkSpace.Controls[p1];//kan pointiw 3la panel Lawel
                        Control P2 = PanWorkSpace.Controls[p2];//kan pointiw 3la panel li mliyi m3a lawel
                        relations.Add(new Relation
                        {
                            NameUC1 = P1.Name,
                            NameUC2 = P2.Name,
                            Point1 = P1.Location,
                            Point2 = P2.Location,
                            ColorC = relations[li[i]].ColorC,
                            Dashed = relations[li[i]].Dashed
                        });//kanzido f drawlines bache n9dro nrssmoha f event Paint dyal panel2(panel li kanrssmo fih les relation)
                    }
                    relations.Remove(relations[li[i]]);//ila kan ki 7rek panel(Table) kanmss7o had relation, bache n3awdo nrsmoha f la nouvelle position 
                    PanWorkSpace.Invalidate();//kan 3yto l event Paint dyal panel2 (bache irsmlina ga3 les relations li kaynine f collection drawlines)
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripItem menuItem = sender as ToolStripItem;
            if (menuItem != null)
            {
                // Retrieve the ContextMenuStrip that owns this ToolStripItem
                ContextMenuStrip owner = menuItem.Owner as ContextMenuStrip;
                if (owner != null)
                {
                    AddThings();
                    val++;

                    // Get the control that is displaying this context menu
                    Control sourceControl = owner.SourceControl;
                    PanWorkSpace.Controls.Remove(sourceControl);

                    AddThings();
                }
            }
        }

        private void relationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            makerelation = true;
        }

        private void AddPanActor(string name, string text, int x, int y, bool forUsecase)
        {
            Panel p = new Panel
            {
                Name = name,
                Size = new Size(120, 185),
                AllowDrop = true,
                BackgroundImage = global::SHI_UML.Properties.Resources.actor,
                BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom,
                Location = new Point(x, y),
                ContextMenuStrip = contextMenuStrip2,
            };
            Label l1 = new Label
            {
                AutoSize = false,
                Text = text,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Bottom,
                Size = new Size(120, 36),
            };
            l1.DoubleClick += new EventHandler(Label_DoubleClick);//event
            Program.TooltipObj(l1, " " + l1.Text + ", Double Click to change text");
            p.Controls.Add(l1);
            Label l2 = new Label
            {
                AutoSize = false,
                Text = "",
                Dock = DockStyle.Top,
                Size = new Size(120, 28),
                BackColor = Color.FromArgb(38, 134, 170),
                Cursor = System.Windows.Forms.Cursors.SizeAll,
            };
            if (forUsecase)
            {
                p.DragDrop += new DragEventHandler(ActorUseCase_DragDrop);//event
                p.DragEnter += new DragEventHandler(ActorUseCase_DragEnter);//event
                p.MouseDown += new MouseEventHandler(ActorUseCase_MouseDown);//event
                l2.MouseMove += new MouseEventHandler(Actor_MouseMove);//event
                l2.MouseDown += new MouseEventHandler(Actor_MouseDown);//event
                l2.MouseUp += new MouseEventHandler(Actor_MouseUp);//event
                p.Controls.Add(l2);
            }
            else
            {
                p.DragDrop += new DragEventHandler(ActorSequence_DragDrop);//event
                p.DragEnter += new DragEventHandler(ActorSequence_DragEnter);//event
                p.MouseDown += new MouseEventHandler(ActorSequence_MouseDown);//event
                DrawLifeLine(p);
            }
            PanWorkSpace.Controls.Add(p);
        }

        private void AddUseCaseLab(string name, string text, int x, int y)
        {
            Label l = new Label
            {
                Name = name,
                AutoSize = false,
                Text = text,
                AllowDrop = true,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(200, 100),
                Cursor = System.Windows.Forms.Cursors.SizeAll,
                ContextMenuStrip = contextMenuStrip1,
                Location = new Point(x, y),
            };
            l.DoubleClick += new EventHandler(Label_DoubleClick);//event
            l.MouseMove += new MouseEventHandler(UseCase_MouseMove);//event
            l.MouseDown += new MouseEventHandler(UseCase_MouseDown);//event
            l.DragDrop += new DragEventHandler(ActorUseCase_DragDrop);//event
            l.DragEnter += new DragEventHandler(ActorUseCase_DragEnter);//event
            l.Paint += new PaintEventHandler(LabelUseCase_Paint);//event
            l.Invalidate();
            PanWorkSpace.Controls.Add(l);
        }

        private void BtnUseCase_Click(object sender, EventArgs e)
        {
            AddThings();
            val++;

            FormNewUseCase f = new FormNewUseCase("Use case text : ");
            f.ShowDialog();
            pa++;
            if (!String.IsNullOrWhiteSpace(f.UseCaseText))//kantestew wach string li rdat lina lform(FormNewUseCase) null (vide) wla fiha ghir les espaces
            {
                AddUseCaseLab("LabUseCase" + pa, f.UseCaseText, 400, 80);
            }

            AddThings();
        }

        private void BtnActor_Click(object sender, EventArgs e)
        {
            AddThings();
            val++;

            FormNewActor f = new FormNewActor(true);
            f.ShowDialog();
            pa++;
            if (f.ActorN.Substring(0, 3) == "act")
            {
                AddPanActor("PanActor" + pa, f.ActorN.Substring(3), 100, 50, true);
            }
            else
            {
                AddSActorSys("LabActSys" + pa, f.ActorN.Substring(3), 1100, 50);
            }

            AddThings();
        }

        private void AddSActorSys(string name, string text, int x, int y)
        {
            Label l = new Label
            {
                Name = name,
                AutoSize = false,
                Text = text,
                AllowDrop = true,
                TextAlign = ContentAlignment.MiddleCenter,
                Cursor = System.Windows.Forms.Cursors.SizeAll,
                Size = new Size(200, 90),
                ContextMenuStrip = contextMenuStrip1,
                Location = new Point(x, y),
            };
            l.DoubleClick += new EventHandler(Label_DoubleClick);//event
            l.MouseMove += new MouseEventHandler(UseCase_MouseMove);//event
            l.MouseDown += new MouseEventHandler(UseCase_MouseDown);//event
            l.DragDrop += new DragEventHandler(ActorUseCase_DragDrop);//event
            l.DragEnter += new DragEventHandler(ActorUseCase_DragEnter);//event
            l.Paint += new PaintEventHandler(LabelSequence_Paint);//event
            l.Invalidate();
            DrawLifeLine(l);
            PanWorkSpace.Controls.Add(l);
        }

        private void BtnSterInclExte_Click(object sender, EventArgs e)
        {
            AddThings();
            val++;

            pa++;
            AddLabelTextTochange("LabMultip" + pa, "<<" + ((Button)sender).Text + ">>", 30, 10);

            AddThings();
        }

        private void BtnInheritance_Click(object sender, EventArgs e)
        {
            AddThings();
            val++;

            pa++;
            AddPanelObject("PanDepend" + pa, 20, 20);

            AddThings();
        }
        #endregion

        #region Diagram Sequence

        private void DrawArrow(Pen pen)
        {
            using (GraphicsPath capPath = new GraphicsPath())
            {
                // A triangle
                capPath.AddLine(-2, 0, 2, 0);
                capPath.AddLine(-2, 0, 0, 2);
                capPath.AddLine(0, 2, 2, 0);

                pen.CustomEndCap = new System.Drawing.Drawing2D.CustomLineCap(null, capPath);
            }
        }
        private void DrawFilledArrow(Pen pen)
        {
            pen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
        }

        private void AddSequenceLab(string name, string text, int x, int y)
        {
            Label l = new Label
            {
                Name = name,
                AutoSize = false,
                Text = text,
                AllowDrop = true,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(150, 70),
                ContextMenuStrip = contextMenuStrip1,
                Location = new Point(x, y),
            };
            l.DoubleClick += new EventHandler(Label_DoubleClick);//event
            l.MouseDown += new MouseEventHandler(Sequence_MouseDown);//event
            l.DragDrop += new DragEventHandler(ActorSequence_DragDrop);//event
            l.DragEnter += new DragEventHandler(ActorSequence_DragEnter);//event
            l.Paint += new PaintEventHandler(LabelSequence_Paint);//event
            l.Invalidate();
            DrawLifeLine(l);
            PanWorkSpace.Controls.Add(l);
        }

        private void Sequence_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Label p = (Label)sender;
                p.DoDragDrop(p.Name, DragDropEffects.Move);
            }
        }

        private void LabelSequence_Paint(object sender, PaintEventArgs e)
        {
            Control c = sender as Control;
            Rectangle r = new Rectangle(0, 0, c.Width - 2, c.Height - 2);
            e.Graphics.DrawRectangle(new Pen(Color.Black, 5), r);
        }

        private void ActorSequence_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                Panel p = (Panel)sender;//panel li bda event
                p.DoDragDrop(p.Name, DragDropEffects.Move);//enfois ki bghi iglissi bache idir DraAndDrop, kanpassiw had panel kamel, lfin ghadi iglissiha
            }
        }

        private void ActorSequence_DragEnter(object sender, DragEventArgs e)
        {
            //darori AllowDrop = true
            e.Effect = DragDropEffects.Move;//kan3tiw wa7ed sora bli i9der idropi had lpanel f panel akhor
        }

        private void ActorSequence_DragDrop(object sender, DragEventArgs e)
        {
            //darori AllowDrop = true
            string pa1 = (string)e.Data.GetData(typeof(string));
            int cont = FindControl(pa1);
            Control p = PanWorkSpace.Controls[cont];
            //fache ki droppi panel, kanjibo Data li seftnaha, t9der tkoun ay objet, f had l7ala hia panel

            Control b = (Control)sender;//panel li fin dropina
            int a = FindDrawLinesByNames(p.Name, b.Name);
            Color Clr = new Color();//Objet Color
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
            {
                AddThings();
                val++;

                string rela = "MS";
                string message = "";
                Clr = cd.Color;//kan7ato Color li jbna mne dialog f Objet dyalna
                if (p.Location != b.Location)
                {
                    int wi1 = p.Size.Width; int hei1 = p.Size.Height;//kanjibo lwidth ou lheught dyal les panelli binathoum lrelation
                    int wi2 = b.Size.Width; int hei2 = b.Size.Height;//        //        //        //
                    int valWi1 = Convert.ToInt32(wi1 / 2); int valhei1 = Convert.ToInt32(hei1 / 2);//kan7asbo ness dyalhom bach nrssmo l relation mne wasset table
                    int valWi2 = Convert.ToInt32(wi2 / 2); int valhei2 = Convert.ToInt32(hei2 / 2);//        //        //        //
                    FormRelationType F = new FormRelationType();
                    F.ShowDialog();
                    rela = F.RelationType;
                    message = F.Message;
                    int x1 = Convert.ToInt32((b.Location.X + valWi2 - p.Location.X + valWi1) / 2) + p.Location.X;//x hia ness dyal lmassfa bih had jouj les tables
                    pa++;
                    AddLabelTextTochange("LabMessage" + pa, message, x1, YDrawseq);
                }
                Relation R = new Relation  //kanajoutiw drawline mne panel lawel, li panel li tdropa fih, ou kankhdo location dyalhoum, bache nrssmo relation binathoum
                {
                    NameUC1 = p.Name,
                    NameUC2 = b.Name,
                    Point1 = p.Location,
                    Point2 = b.Location,
                    Dashed = false,
                    ColorC = Clr,
                    NameUO1 = rela,
                    NameUO2 = message,
                };
                relations.Add(R);

                AddThings();
            }
            PanWorkSpace.Invalidate();//kan3ayto event Paint bache irssemles les relations
        }

        int Xseq = 50, Yseq = 30;

        private void BtnSequence_Click(object sender, EventArgs e)
        {
            AddThings();
            val++;

            FormNewUseCase f = new FormNewUseCase("Sequence text : ");
            f.ShowDialog();
            pa++;
            if (!String.IsNullOrWhiteSpace(f.UseCaseText))
            {
                AddSequenceLab("LabSequence" + pa, f.UseCaseText, Xseq, Yseq);
                Xseq += 230;
            }

            AddThings();
        }

        private void BtnActorSequence_Click(object sender, EventArgs e)
        {
            AddThings();
            val++;

            FormNewActor f = new FormNewActor(false);
            f.ShowDialog();
            pa++;
            AddPanActor("PanActor" + pa, f.ActorN.Substring(3), Xseq, 5, false);
            Xseq += 200;

            AddThings();
        }

        private void BtnMessage_Click(object sender, EventArgs e)
        {
            AddThings();
            val++;

            pa++;
            AddLabelTextTochange("LabMessage" + pa, "Message / note", 30, 10);

            AddThings();
        }

        private void DrawLifeLine(Control C)
        {
            int x = C.Location.X + Convert.ToInt32(C.Width / 2);
            int y = C.Location.Y + Convert.ToInt32(C.Height / 2);
            Point p = new Point(x, y);
            Pen pen = new Pen(Color.Black, 8);
            float[] dashValues = { 2, 2, 2, 2 };
            pen.DashPattern = dashValues;
            PanWorkSpace.CreateGraphics().DrawLine(pen, p, new Point(x, PanWorkSpace.Height));
        }
        int YDrawseq = 300;

        private void DrawRelationSequence(int i, Pen pen)
        {
            try
            {
                //DrawArrow(pen);
                int c1 = FindControl(relations[i].NameUC1);//
                int c2 = FindControl(relations[i].NameUC2);//
                if (c1 == -1 || c2 == -1)//ila mal9inache chi wa7edfihoum kanmss7o drawline
                {
                    relations.Remove(relations[i]);
                }
                else
                {
                    if (relations[i].NameUO1 == "MS")
                    {
                        DrawFilledArrow(pen);
                    }
                    else if (relations[i].NameUO1 == "MA")
                    {
                        DrawArrow(pen);
                    }
                    else
                    {
                        DrawFilledArrow(pen);
                        float[] dashValues = { 5, 5, 5, 5 };
                        pen.DashPattern = dashValues;
                    }
                    int wi1 = PanWorkSpace.Controls[c1].Size.Width; int hei1 = PanWorkSpace.Controls[c1].Size.Height;//kanjibo lwidth ou lheught dyal les panelli binathoum lrelation
                    int wi2 = PanWorkSpace.Controls[c2].Size.Width; int hei2 = PanWorkSpace.Controls[c2].Size.Height;//        //        //        //
                    int valWi1 = Convert.ToInt32(wi1 / 2); int valhei1 = Convert.ToInt32(hei1 / 2);//kan7asbo ness dyalhom bach nrssmo l relation mne wasset table
                    int valWi2 = Convert.ToInt32(wi2 / 2); int valhei2 = Convert.ToInt32(hei2 / 2);//        //        //        //

                    if (relations[i].Point1 == relations[i].Point2)
                    {
                        DrawArrow(pen);
                        Point p1 = new Point(relations[i].Point1.X + valWi1, YDrawseq);
                        Point p2 = new Point(relations[i].Point2.X + 40 + valWi2, YDrawseq);
                        Point p3 = new Point(relations[i].Point2.X + 40 + valWi2, YDrawseq + 30);
                        Point p4 = new Point(relations[i].Point2.X + valWi2, YDrawseq + 30);
                        PanWorkSpace.CreateGraphics().DrawLine(new Pen(pen.Color, 5), p1, p2);//kanrssmo relation
                        PanWorkSpace.CreateGraphics().DrawLine(new Pen(pen.Color, 5), p2, p3);//kanrssmo relation
                        PanWorkSpace.CreateGraphics().DrawLine(pen, p3, p4);//kanrssmo relation
                        YDrawseq += 10;
                    }
                    else
                    {
                        //kanrssmo relation direct bin les tables bjouj
                        Point p1 = new Point(relations[i].Point1.X + valWi1, YDrawseq);
                        Point p2 = new Point(relations[i].Point2.X + valWi2, YDrawseq);
                        PanWorkSpace.CreateGraphics().DrawLine(pen, p1, p2);//kanrssmo relation
                    }
                    YDrawseq += 40;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion


        private void classToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormNewDiagram F = new FormNewDiagram();
            F.ShowDialog();
            if (F.DiagName != "")
            {
                umpr.DiagramsNames.Add("Cla" + F.DiagName);
                DiagClass d = new DiagClass { Name = F.DiagName };
                diagClasses.Add(d);
                if (currentDiag != "")
                {
                    ChangeEtatWorkSpace();
                }
                //toolStrip1.Enabled = true;
                currentDiagPath = umpr.PathP + @"\" + F.DiagName + ".cla";
                SaveDiagClass(d, currentDiagPath);
                PanRelations.Visible = true;
                FillTreeView();
                FillPanLabels();
                SaveProject();
            }
        }

        private void useCaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormNewDiagram F = new FormNewDiagram();
            F.ShowDialog();
            if (F.DiagName != "")
            {
                umpr.DiagramsNames.Add("Cas" + F.DiagName);
                DiagUseCase d = new DiagUseCase { Name = F.DiagName };
                diagUseCases.Add(d);
                if (currentDiag != "")
                {
                    ChangeEtatWorkSpace();
                }
                //toolStrip1.Enabled = true;
                currentDiagPath = umpr.PathP + @"\" + F.DiagName + ".cas";
                SaveDiagUseCase(d, currentDiagPath);
                PanRelations.Visible = true;
                FillTreeView();
                FillPanLabels();
                SaveProject();
            }
        }

        private void sequenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormNewDiagram F = new FormNewDiagram();
            F.ShowDialog();
            if (F.DiagName != "")
            {
                umpr.DiagramsNames.Add("Seq" + F.DiagName);
                DiagSequence d = new DiagSequence { Name = F.DiagName };
                diagSequences.Add(d);
                if (currentDiag != "")
                {
                    ChangeEtatWorkSpace();
                }
                //toolStrip1.Enabled = true;
                currentDiagPath = umpr.PathP + @"\" + F.DiagName + ".seq";
                SaveDiagSequence(d, currentDiagPath);
                PanRelations.Visible = true;
                FillTreeView();
                FillPanLabels();
                SaveProject();
            }
        }

        private void projectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormNewProject F = new FormNewProject();
            F.ShowDialog();
            if (F.UmlProj != null)
            {
                //diagramToolStripMenuItem.Enabled = true;
                //diagramToolStripMenuItem1.Enabled = true;
                ribBtnNew.Enabled = false;
                ribBtnOpen.Enabled = false;
                ribBtnClose.Enabled = true;
                ribDiagram.Enabled = true;
                ribPanSave.Enabled = true;
                ribPanEdit.Enabled = true;
                PanToolBoxs1.Visible = true;
                umpr = F.UmlProj;
                SaveProject();
            }
        }

        private void FillTreeView()
        {
            TreeViewProject.Nodes.Clear();
            TreeViewProject.Nodes.Add(umpr.Name);
            for (int i = 0; i < umpr.DiagramsNames.Count; i++)
            {
                TreeViewProject.Nodes[0].Nodes.Add(umpr.DiagramsNames[i].Substring(3));
            }
        }

        private void FillPanLabels()
        {
            PanLabels.Controls.Clear();
            for (int i = 0; i < umpr.DiagramsNames.Count; i++)
            {
                if (FindURDiagram(umpr.DiagramsNames[i]) == -1)
                {
                    UR_Diagrams.Add(new UR_Diagram(umpr.DiagramsNames[i]));
                }
                Label l = new Label
                {
                    Text = umpr.DiagramsNames[i].Substring(3),
                    AutoSize = true,
                    Dock = DockStyle.Left,
                    Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                };
                l.Click += new EventHandler(LabelDiagram_Click);
                PanLabels.Controls.Add(l);
                if (i == umpr.DiagramsNames.Count - 1)
                {
                    LabelDiagram_Click(l.Parent.Controls[i], null);
                }
            }
        }

        #region find diagrams

        private int FindURDiagram(string name)
        {
            for (int i = 0; i < UR_Diagrams.Count; i++)
            {
                if (UR_Diagrams[i].Name == name)
                {
                    return i;
                }
            }
            return -1;
        }

        private int FindDiagram(string name)
        {
            for (int i = 0; i < umpr.DiagramsNames.Count; i++)
            {
                if (umpr.DiagramsNames[i].Substring(3) == name)
                {
                    return i;
                }
            }
            return -1;
        }
        private int FindDiagClasses(string name)
        {
            for (int i = 0; i < diagClasses.Count; i++)
            {
                if (diagClasses[i].Name == name)
                {
                    return i;
                }
            }
            return -1;
        }

        private int FindDiagUseCases(string name)
        {
            for (int i = 0; i < diagUseCases.Count; i++)
            {
                if (diagUseCases[i].Name == name)
                {
                    return i;
                }
            }
            return -1;
        }

        private int FindDiagSequences(string name)
        {
            for (int i = 0; i < diagSequences.Count; i++)
            {
                if (diagSequences[i].Name == name)
                {
                    return i;
                }
            }
            return -1;
        }
        #endregion

        private void LabelDiagram_Click(object sender, EventArgs e)
        {
            if (currentDiag != "")
            {
                ChangeEtatWorkSpace();
                relations = new List<Relation>();
                int indice1 = FindDiagramByName(currentDiagName);
                if (indice1 != -1)
                {
                    if (currentDiag == "Cla")
                    {
                        UR_Diagrams[indice1].valDC = val;
                    }
                    else if (currentDiag == "Cas")
                    {
                        UR_Diagrams[indice1].valDU = val;
                    }
                    else
                    {
                        UR_Diagrams[indice1].valDS = val;
                    }
                }
            }
            Label l = sender as Label;
            int d = FindDiagram(l.Text);
            if (d != -1)
            {
                currentDiag = umpr.DiagramsNames[d].Substring(0, 3);
                currentDiagName = umpr.DiagramsNames[d].Substring(3);
                currentDiagPath = umpr.PathP + @"\" + umpr.DiagramsNames[d].Substring(3) + "." + currentDiag.ToLower();
            }
            for (int i = 0; i < PanLabels.Controls.Count; i++)
            {
                PanLabels.Controls[i].BackColor = Color.SteelBlue;
            }

            int indice = FindDiagramByName(currentDiagName);
            if (indice != -1)
            {
                //MessageBox.Show("Indice = " + indice + " ,,, count " + UR_Diagrams.Count);
                if (currentDiag == "Cla")
                {
                    val = UR_Diagrams[indice].valDC;
                }
                else if (currentDiag == "Cas")
                {
                    val = UR_Diagrams[indice].valDU;
                }
                else
                {
                    val = UR_Diagrams[indice].valDS;
                }
            }
            l.BackColor = Color.White;

            FillWorkSpace();
        }

        private void FillWorkSpace()
        {
            if (currentDiag == "Cla")
            {
                PanTb1.Visible = true;
                PanTb2.Visible = false;
                PanTb3.Visible = false;
                int i = FindDiagClasses(currentDiagName);
                if (i != -1)
                {
                    SetDiagClass(diagClasses[i], true);
                }
            }
            else if (currentDiag == "Cas")
            {
                PanTb1.Visible = false;
                PanTb2.Visible = true;
                PanTb3.Visible = false;
                int i = FindDiagUseCases(currentDiagName);
                if (i != -1)
                {
                    SetDiagUseCase(diagUseCases[i], true);
                }
            }
            else
            {
                PanTb1.Visible = false;
                PanTb2.Visible = false;
                PanTb3.Visible = true;
                int i = FindDiagSequences(currentDiagName);
                if (i != -1)
                {
                    SetDiagSequence(diagSequences[i], true);
                }
            }
        }

        private void ChangeEtatWorkSpace()
        {
            if (currentDiag == "Cla")
            {
                int i = FindDiagClasses(currentDiagName);
                if (i != -1)
                {
                    GetDiagClass(i);
                }
            }
            else if (currentDiag == "Cas")
            {
                int i = FindDiagUseCases(currentDiagName);
                if (i != -1)
                {
                    GetDiagUseCase(i);
                }
            }
            else
            {
                int i = FindDiagSequences(currentDiagName);
                if (i != -1)
                {
                    GetDiagSequence(i);
                }
            }
        }
        
        private void GetDiagClass(int indice)
        {
            DiagClass dc = diagClasses[indice];

            dc.UmlClasses.Clear();
            dc.UmlObjects.Clear();

            for (int i = 0; i < PanWorkSpace.Controls.Count; i++)
            {
                if (PanWorkSpace.Controls[i].Name.Substring(0, 5) == "class")
                {
                    UmlClass uC = new UmlClass
                    {
                        Name = PanWorkSpace.Controls[i].Name,
                        Text = PanWorkSpace.Controls[i].Controls[1].Text,
                        X = PanWorkSpace.Controls[i].Location.X,
                        Y = PanWorkSpace.Controls[i].Location.Y,
                        
                    };
                    DataGridView D_att = PanWorkSpace.Controls[i].Controls[2] as DataGridView;
                    if (D_att.Columns[0].HeaderText == @"Attributes/\")
                    {
                        uC.HideAtt = false;
                    }
                    else
                    {
                        uC.HideAtt = true;
                    }
                    for (int j = 0; j < D_att.Rows.Count - 1; j++)
                    {
                        object o = D_att.Rows[j].Cells[0].Value;
                        if (o != null)
                        {
                            uC.Attributs.Add(o.ToString());
                        }
                    }
                    DataGridView D_Meth = PanWorkSpace.Controls[i].Controls[3] as DataGridView;
                    if (D_Meth.Columns[0].HeaderText == @"Methodes/\")
                    {
                        uC.HideMeth = false;
                    }
                    else
                    {
                        uC.HideMeth = true;
                    }
                    for (int j = 0; j < D_Meth.Rows.Count -1; j++)
                    {
                        object o = D_Meth.Rows[j].Cells[0].Value;
                        if (o != null)
                        {
                            uC.Methodes.Add(o.ToString());
                        }
                    }
                    dc.UmlClasses.Add(uC);
                }
                else
                {
                    dc.UmlObjects.Add(new UmlClassObjects
                    {
                        Name = PanWorkSpace.Controls[i].Name,
                        Text = PanWorkSpace.Controls[i].Text,
                        X = PanWorkSpace.Controls[i].Location.X,
                        Y = PanWorkSpace.Controls[i].Location.Y,
                    });
                }
            }

            dc.Relations.Clear();
            for (int i = 0; i < relations.Count; i++)
            {
                dc.Relations.Add(relations[i]);
            }
        }

        private void GetDiagUseCase(int indice)
        {
            DiagUseCase du = diagUseCases[indice];

            du.Actors.Clear();
            du.Objects.Clear();
            du.UseCases.Clear();

            for (int i = 0; i < PanWorkSpace.Controls.Count; i++)
            {
                if (PanWorkSpace.Controls[i].Name.Substring(0,5) == "LabUs")
                {
                    du.UseCases.Add(new UmlClassObjects
                    {
                        Name = PanWorkSpace.Controls[i].Name,
                        Text = PanWorkSpace.Controls[i].Text,
                        X = PanWorkSpace.Controls[i].Location.X,
                        Y = PanWorkSpace.Controls[i].Location.Y,
                    });
                }
                else if(PanWorkSpace.Controls[i].Name.Substring(0, 5) == "PanAc")
                {
                    du.Actors.Add(new UmlClassObjects
                    {
                        Name = PanWorkSpace.Controls[i].Name,
                        Text = PanWorkSpace.Controls[i].Controls[0].Text,
                        X = PanWorkSpace.Controls[i].Location.X,
                        Y = PanWorkSpace.Controls[i].Location.Y,
                    });
                }
                else if (PanWorkSpace.Controls[i].Name.Substring(0, 5) == "LabAc")
                {
                    du.Actors.Add(new UmlClassObjects
                    {
                        Name = PanWorkSpace.Controls[i].Name,
                        Text = PanWorkSpace.Controls[i].Text,
                        X = PanWorkSpace.Controls[i].Location.X,
                        Y = PanWorkSpace.Controls[i].Location.Y,
                    });
                }
                else
                {
                    du.Objects.Add(new UmlClassObjects
                    {
                        Name = PanWorkSpace.Controls[i].Name,
                        Text = PanWorkSpace.Controls[i].Text,
                        X = PanWorkSpace.Controls[i].Location.X,
                        Y = PanWorkSpace.Controls[i].Location.Y,
                    });
                }

            }

            du.Relations.Clear();
            for (int i = 0; i < relations.Count; i++)
            {
                du.Relations.Add(relations[i]);
            }
        }

        private void GetDiagSequence(int indice)
        {
            DiagSequence ds = diagSequences[indice];
            
            ds.Messages.Clear();
            ds.Relations.Clear();
            ds.Sequences.Clear();
            ds.Actors.Clear();

            for (int i = 0; i < PanWorkSpace.Controls.Count; i++)
            {
                if (PanWorkSpace.Controls[i].Name.Substring(0, 5) == "LabSe")
                {
                    ds.Sequences.Add(new UmlClassObjects
                    {
                        Name = PanWorkSpace.Controls[i].Name,
                        Text = PanWorkSpace.Controls[i].Text,
                        X = PanWorkSpace.Controls[i].Location.X,
                        Y = PanWorkSpace.Controls[i].Location.Y,
                    });
                }
                else if (PanWorkSpace.Controls[i].Name.Substring(0, 5) == "PanAc")
                {
                    ds.Actors.Add(new UmlClassObjects
                    {
                        Name = PanWorkSpace.Controls[i].Name,
                        Text = PanWorkSpace.Controls[i].Controls[0].Text,
                        X = PanWorkSpace.Controls[i].Location.X,
                        Y = PanWorkSpace.Controls[i].Location.Y,
                    });
                }
                else
                {
                    ds.Messages.Add(new UmlClassObjects
                    {
                        Name = PanWorkSpace.Controls[i].Name,
                        Text = PanWorkSpace.Controls[i].Text,
                        X = PanWorkSpace.Controls[i].Location.X,
                        Y = PanWorkSpace.Controls[i].Location.Y,
                    });
                }
            }

            ds.Relations.Clear();
            for (int i = 0; i < relations.Count; i++)
            {
                ds.Relations.Add(relations[i]);
            }
        }

        private void SetDiagClass(DiagClass dc,bool clearWorkSpace)
        {
            if (clearWorkSpace)
            {
                PanWorkSpace.Controls.Clear();
                relations.Clear();
            }
            for (int i = 0; i < dc.UmlClasses.Count; i++)
            {
                AddUmlClassPanelToWorkspace(dc.UmlClasses[i].Name, dc.UmlClasses[i], dc.UmlClasses[i].X, dc.UmlClasses[i].Y, dc.UmlClasses[i].HideAtt, dc.UmlClasses[i].HideMeth);
            }
            for (int i = 0; i < dc.UmlObjects.Count; i++)
            {
                if (dc.UmlObjects[i].Name.Substring(0,3) == "Pan")
                {
                    AddPanelObject(dc.UmlObjects[i].Name, dc.UmlObjects[i].X, dc.UmlObjects[i].Y);
                }
                else
                {
                    AddLabelTextTochange(dc.UmlObjects[i].Name, dc.UmlObjects[i].Text, dc.UmlObjects[i].X, dc.UmlObjects[i].Y);
                }
            }
            for (int i = 0; i < dc.Relations.Count; i++)
            {
                relations.Add(dc.Relations[i]);
            }
            PanWorkSpace.Invalidate();
        }

        private void SetDiagUseCase(DiagUseCase du, bool clearWorkSpace)
        {
            if (clearWorkSpace)
            {
                PanWorkSpace.Controls.Clear();
                relations.Clear();
            }
            for (int i = 0; i < du.Actors.Count; i++)
            {
                if (du.Actors[i].Name.Substring(0, 5) == "PanAc")
                {
                    AddPanActor(du.Actors[i].Name, du.Actors[i].Text, du.Actors[i].X, du.Actors[i].Y, true);
                }
                else
                {
                    AddSActorSys(du.Actors[i].Name, du.Actors[i].Text, du.Actors[i].X, du.Actors[i].Y);
                }
            }
            for (int i = 0; i < du.UseCases.Count; i++)
            {
                AddUseCaseLab(du.UseCases[i].Name, du.UseCases[i].Text, du.UseCases[i].X, du.UseCases[i].Y);
            }
            for (int i = 0; i < du.Objects.Count; i++)
            {
                if (du.Objects[i].Name.Substring(0, 3) == "Pan")
                {
                    AddPanelObject(du.Objects[i].Name, du.Objects[i].X, du.Objects[i].Y);
                }
                else
                {
                    AddLabelTextTochange(du.Objects[i].Name, du.Objects[i].Text, du.Objects[i].X, du.Objects[i].Y);
                }
            }
            for (int i = 0; i < du.Relations.Count; i++)
            {
                relations.Add(du.Relations[i]);
            }
            PanWorkSpace.Invalidate();
        }

        private void SetDiagSequence(DiagSequence ds, bool clearWorkSpace)
        {
            if (clearWorkSpace)
            {
                PanWorkSpace.Controls.Clear();
                relations.Clear();
                rects.Clear();
            }
            Xseq = 50;
            for (int i = 0; i < ds.Sequences.Count; i++)
            {
                AddSequenceLab(ds.Sequences[i].Name, ds.Sequences[i].Text, ds.Sequences[i].X, ds.Sequences[i].Y);
                Xseq += 230;
            }
            for (int i = 0; i < ds.Actors.Count; i++)
            {
                AddPanActor(ds.Actors[i].Name, ds.Actors[i].Text, ds.Actors[i].X, ds.Actors[i].Y, false);
                Xseq += 230;
            }
            for (int i = 0; i < ds.Messages.Count; i++)
            {
                AddLabelTextTochange(ds.Messages[i].Name, ds.Messages[i].Text, ds.Messages[i].X, ds.Messages[i].Y);
            }

            for (int i = 0; i < ds.Relations.Count; i++)
            {
                relations.Add(ds.Relations[i]);
            }
            for (int i = 0; i < ds.Rects.Count; i++)
            {
                rects.Add(ds.Rects[i]);
            }
            PanWorkSpace.Invalidate();
            PanWorkSpace.Invalidate();
        }

        private void SaveProject()
        {
            FileStream fs = new FileStream(umpr.PathP + @"\" + umpr.Name + ".shi", FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, umpr);
            fs.Close();
        }

        private void SaveDiagClass(DiagClass dc, string path)
        {
            FileStream fs = new FileStream(path, FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, dc);
            fs.Close();
        }

        private void SaveDiagUseCase(DiagUseCase dc, string path)
        {
            FileStream fs = new FileStream(path, FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, dc);
            fs.Close();
        }

        private void SaveDiagSequence(DiagSequence dc, string path)
        {
            FileStream fs = new FileStream(path, FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, dc);
            fs.Close();
        }

        private void TSBtnSave_Click(object sender, EventArgs e)
        {
            if (currentDiag == "Cla")
            {
                int i = FindDiagClasses(currentDiagName);
                if (i != -1)
                {
                    ChangeEtatWorkSpace();
                    SaveDiagClass(diagClasses[i], currentDiagPath);
                }
            }
            else if (currentDiag == "Cas")
            {
                int i = FindDiagUseCases(currentDiagName);
                if (i != -1)
                {
                    ChangeEtatWorkSpace();
                    SaveDiagUseCase(diagUseCases[i], currentDiagPath);
                }
            }
            else
            {
                int i = FindDiagSequences(currentDiagName);
                if (i != -1)
                {
                    ChangeEtatWorkSpace();
                    SaveDiagSequence(diagSequences[i], currentDiagPath);
                }
            }
            SaveProject();
        }

        private void TSBtnSaveAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < diagClasses.Count; i++)
            {
                SaveDiagClass(diagClasses[i], umpr.PathP + @"\" + diagClasses[i].Name + ".cla");
            }
            for (int i = 0; i < diagUseCases.Count; i++)
            {
                SaveDiagUseCase(diagUseCases[i], umpr.PathP + @"\" + diagUseCases[i].Name + ".cas");
            }
            for (int i = 0; i < diagSequences.Count; i++)
            {
                SaveDiagSequence(diagSequences[i], umpr.PathP + @"\" + diagSequences[i].Name + ".seq");
            }
            SaveProject();
        }

        private void TSBtnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Clear Work space?", "Question?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                relations.Clear();
                PanWorkSpace.Invalidate();
                PanWorkSpace.Controls.Clear();
            }
        }

        private int GetDiagramFromTreeView()
        {
            if (TreeViewProject.Nodes.Count > 0)
            {
                if (TreeViewProject.SelectedNode.Parent != null)
                {
                    return TreeViewProject.SelectedNode.Index;
                }
            }
            return -1;
        }

        private void deleteToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            int i = GetDiagramFromTreeView();
            if (i != -1)
            {
                umpr.DiagramsNames.RemoveAt(i);
                FillTreeView();
                FillPanLabels();
                if (umpr.DiagramsNames.Count == 0)
                {
                    PanToolBoxs1.Visible = false;
                }
            }
        }

        private void projectToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text File(*.shi)|*.shi";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {

                    FileStream fs = new FileStream(ofd.FileName, FileMode.Open);
                    BinaryFormatter bf = new BinaryFormatter();
                    umpr = (UMLProject)bf.Deserialize(fs);//ppppppppp/ppppppppp.shi
                    fs.Close();
                    umpr.PathP = ofd.FileName.Substring(0, ofd.FileName.Length - (umpr.Name.Length + 5));
                    for (int i = 0; i < umpr.DiagramsNames.Count; i++)
                    {
                        if (umpr.DiagramsNames[i].Substring(0, 3) == "Cla")
                        {
                            OpenDiagClass(umpr.PathP + @"\" + umpr.DiagramsNames[i].Substring(3) + ".cla");
                        }
                        else if (umpr.DiagramsNames[i].Substring(0, 3) == "Cas")
                        {
                            OpenDiagUseCase(umpr.PathP + @"\" + umpr.DiagramsNames[i].Substring(3) + ".cas");
                        }
                        else
                        {
                            OpenDiagSequence(umpr.PathP + @"\" + umpr.DiagramsNames[i].Substring(3) + ".seq");
                        }
                    }


                    //**************************
                    //toolStrip1.Enabled = true;
                    ribBtnNew.Enabled = false;
                    ribBtnOpen.Enabled = false;
                    ribBtnClose.Enabled = true;
                    ribDiagram.Enabled = true;
                    PanRelations.Visible = true;
                    FillTreeView();
                    FillPanLabels();
                    SaveProject();
                    ribBtnClose.Enabled = true;
                    ribPanSave.Enabled = true;
                    ribPanEdit.Enabled = true;
                    PanToolBoxs1.Visible = true;
                    //projectToolStripMenuItem1.Enabled = false;
                    //projectToolStripMenuItem.Enabled = false;
                    //diagramToolStripMenuItem1.Enabled = true;
                    //diagramToolStripMenuItem.Enabled = true;
                }
                catch (Exception exp)
                {
                    MessageBox.Show(exp.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void diagramToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Diagram class(*.cla)|*.cla|Diagram use case(*.cas)|*.cas|Diagram sequence(*.seq)|*.seq";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (ofd.DefaultExt.ToLower() == "cla")
                {
                    OpenDiagClass(ofd.FileName);
                }
                else if (ofd.DefaultExt.ToLower() == "cas")
                {
                    OpenDiagUseCase(ofd.FileName);
                }
                else if (ofd.DefaultExt.ToLower() == "seq")
                {
                    OpenDiagSequence(ofd.FileName);
                }
                FillTreeView();
                FillPanLabels();
                SaveProject();
            }
        }

        private void OpenDiagClass(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            diagClasses.Add((DiagClass)bf.Deserialize(fs));
            fs.Close();
        }

        private void OpenDiagUseCase(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            diagUseCases.Add((DiagUseCase)bf.Deserialize(fs));
            fs.Close();
        }

        private void OpenDiagSequence(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            diagSequences.Add((DiagSequence)bf.Deserialize(fs));
            fs.Close();
        }

        private void BtnShowRelation_Click(object sender, EventArgs e)
        {
            if (listBox_Relations.Items.Count > 0)
            {
                int i = listBox_Relations.SelectedIndex;
                if (currentDiag == "Cla")
                {
                    DrawRelationLine(i, new Pen(Color.Red, 5));
                }
                else if (currentDiag == "Cas")
                {
                    DrawRelationActorUseCase(i, new Pen(Color.Red, 5));
                }
                else
                {
                    DrawRelationSequence(i, new Pen(Color.Red, 5));
                }
            }
        }

        private void BtnDeleteRelation_Click(object sender, EventArgs e)
        {
            if (listBox_Relations.Items.Count > 0)
            {
                int i = listBox_Relations.SelectedIndex;
                relations.RemoveAt(i);
                PanWorkSpace.Invalidate();
            }
        }

        private void PanWorkSpace_Scroll(object sender, ScrollEventArgs e)
        {
            PanWorkSpace.Invalidate();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (umpr != null)
            {
                TSBtnSaveAll_Click(null, null);
            }
        }

        private void TSBtnOpen_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(umpr.PathP);
            System.Diagnostics.Process.Start(umpr.PathP);
        }

        Point stP;
        Point endP;
        bool isMd = false;
        bool drawRect = false;
        private void PanWorkSpace_MouseDown(object sender, MouseEventArgs e)
        {
            if (drawRect)
            {
                isMd = true;
                stP = e.Location;
            }
        }

        private void PanWorkSpace_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMd)
            {
                endP = e.Location;
                PanWorkSpace.CreateGraphics().DrawRectangle(new Pen(Color.Black, 6), GetRectangle(stP, endP));
            }
        }

        private void PanWorkSpace_MouseUp(object sender, MouseEventArgs e)
        {
            if (isMd)
            {
                endP = e.Location;
                isMd = false;
                drawRect = false;
                BtnRec.BackColor = Color.FromArgb(74, 174, 213);
                int i = FindDiagSequences(currentDiagName);
                if (i != -1)
                {
                    diagSequences[i].Rects.Add(new Relation
                    {
                        Point1 = stP,
                        Point2 = endP
                    });
                    rects.Add(new Relation
                    {
                        Point1 = stP,
                        Point2 = endP
                    });
                }
                PanWorkSpace.Invalidate();
            }
        }

        private Rectangle GetRectangle(Point p1, Point p2)
        {
            Rectangle r = new Rectangle();

            r.X = Math.Min(p1.X, p2.X);
            r.Y = Math.Min(p1.Y, p2.Y);

            r.Width = Math.Abs(p1.X - p2.X);
            r.Height = Math.Abs(p1.Y - p2.Y);

            return r;
        }

        private void BtnRec_Click(object sender, EventArgs e)
        {
            AddThings();
            val++;

            drawRect = true;
            BtnRec.BackColor = Color.DarkBlue;
            AddThings();
        }

        private void closeProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("are you sure to close this project ?", "confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                //diagramToolStripMenuItem.Enabled = false;
                //diagramToolStripMenuItem1.Enabled = false;
                //projectToolStripMenuItem1.Enabled = true;
                //projectToolStripMenuItem.Enabled = true;
                ribBtnNew.Enabled = true;
                ribBtnOpen.Enabled = true;
                ribBtnClose.Enabled = false;
                Save.Enabled = false;
                SaveAll.Enabled = false;
                PanToolBoxs1.Visible = false;
                PanTb1.Visible = false;
                PanTb2.Visible = false;
                PanTb3.Visible = false;
                ribDiagram.Enabled = false;
                ribPanSave.Enabled = false;
                ribPanEdit.Enabled = false;
                PanLabels.Controls.Clear();
                PanWorkSpace.Controls.Clear();
                PanWorkSpace.CreateGraphics().Clear(Color.FromArgb(175, 213, 237));
                umpr = null;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("exist ?", "confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void BtnClearRelations_Click(object sender, EventArgs e)
        {
            relations.Clear();
            PanWorkSpace.Invalidate();
        }

        private Point MouseDownLocationM;

        private void Label_MouseDownM(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MouseDownLocationM = e.Location;
            }
        }

        private void Label_MouseMoveM(object sender, MouseEventArgs e)
        {
            Label L = (Label)sender;
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                L.Left = e.X + L.Left - MouseDownLocationM.X;
                L.Top = e.Y + L.Top - MouseDownLocationM.Y;
            }
        }

        private DiagClass URGetDiagClass()
        {
            DiagClass dc = new DiagClass();
            

            for (int i = 0; i < PanWorkSpace.Controls.Count; i++)
            {
                if (PanWorkSpace.Controls[i].Name.Substring(0, 5) == "class")
                {
                    UmlClass uC = new UmlClass
                    {
                        Name = PanWorkSpace.Controls[i].Name,
                        Text = PanWorkSpace.Controls[i].Controls[1].Text,
                        X = PanWorkSpace.Controls[i].Location.X,
                        Y = PanWorkSpace.Controls[i].Location.Y,

                    };
                    DataGridView D_att = PanWorkSpace.Controls[i].Controls[2] as DataGridView;
                    if (D_att.Columns[0].HeaderText == @"Attributes/\")
                    {
                        uC.HideAtt = false;
                    }
                    else
                    {
                        uC.HideAtt = true;
                    }
                    for (int j = 0; j < D_att.Rows.Count - 1; j++)
                    {
                        object o = D_att.Rows[j].Cells[0].Value;
                        if (o != null)
                        {
                            uC.Attributs.Add(o.ToString());
                        }
                    }
                    DataGridView D_Meth = PanWorkSpace.Controls[i].Controls[3] as DataGridView;
                    if (D_Meth.Columns[0].HeaderText == @"Methodes/\")
                    {
                        uC.HideMeth = false;
                    }
                    else
                    {
                        uC.HideMeth = true;
                    }
                    for (int j = 0; j < D_Meth.Rows.Count - 1; j++)
                    {
                        object o = D_Meth.Rows[j].Cells[0].Value;
                        if (o != null)
                        {
                            uC.Methodes.Add(o.ToString());
                        }
                    }
                    dc.UmlClasses.Add(uC);
                }
                else
                {
                    dc.UmlObjects.Add(new UmlClassObjects
                    {
                        Name = PanWorkSpace.Controls[i].Name,
                        Text = PanWorkSpace.Controls[i].Text,
                        X = PanWorkSpace.Controls[i].Location.X,
                        Y = PanWorkSpace.Controls[i].Location.Y,
                    });
                }
            }
            
            for (int i = 0; i < relations.Count; i++)
            {
                dc.Relations.Add(relations[i]);
            }
            return dc;
        }

        private DiagUseCase URGetDiagUseCase()
        {
            DiagUseCase du = new DiagUseCase();
            

            for (int i = 0; i < PanWorkSpace.Controls.Count; i++)
            {
                if (PanWorkSpace.Controls[i].Name.Substring(0, 5) == "LabUs")
                {
                    du.UseCases.Add(new UmlClassObjects
                    {
                        Name = PanWorkSpace.Controls[i].Name,
                        Text = PanWorkSpace.Controls[i].Text,
                        X = PanWorkSpace.Controls[i].Location.X,
                        Y = PanWorkSpace.Controls[i].Location.Y,
                    });
                }
                else if (PanWorkSpace.Controls[i].Name.Substring(0, 5) == "PanAc")
                {
                    du.Actors.Add(new UmlClassObjects
                    {
                        Name = PanWorkSpace.Controls[i].Name,
                        Text = PanWorkSpace.Controls[i].Controls[0].Text,
                        X = PanWorkSpace.Controls[i].Location.X,
                        Y = PanWorkSpace.Controls[i].Location.Y,
                    });
                }
                else if (PanWorkSpace.Controls[i].Name.Substring(0, 5) == "LabAc")
                {
                    du.Actors.Add(new UmlClassObjects
                    {
                        Name = PanWorkSpace.Controls[i].Name,
                        Text = PanWorkSpace.Controls[i].Text,
                        X = PanWorkSpace.Controls[i].Location.X,
                        Y = PanWorkSpace.Controls[i].Location.Y,
                    });
                }
                else
                {
                    du.Objects.Add(new UmlClassObjects
                    {
                        Name = PanWorkSpace.Controls[i].Name,
                        Text = PanWorkSpace.Controls[i].Text,
                        X = PanWorkSpace.Controls[i].Location.X,
                        Y = PanWorkSpace.Controls[i].Location.Y,
                    });
                }

            }
            
            for (int i = 0; i < relations.Count; i++)
            {
                du.Relations.Add(relations[i]);
            }
            return du;
        }

        private DiagSequence URGetDiagSequence()
        {
            DiagSequence ds = new DiagSequence();
            

            for (int i = 0; i < PanWorkSpace.Controls.Count; i++)
            {
                if (PanWorkSpace.Controls[i].Name.Substring(0, 5) == "LabSe")
                {
                    ds.Sequences.Add(new UmlClassObjects
                    {
                        Name = PanWorkSpace.Controls[i].Name,
                        Text = PanWorkSpace.Controls[i].Text,
                        X = PanWorkSpace.Controls[i].Location.X,
                        Y = PanWorkSpace.Controls[i].Location.Y,
                    });
                }
                else if (PanWorkSpace.Controls[i].Name.Substring(0, 5) == "PanAc")
                {
                    ds.Actors.Add(new UmlClassObjects
                    {
                        Name = PanWorkSpace.Controls[i].Name,
                        Text = PanWorkSpace.Controls[i].Controls[0].Text,
                        X = PanWorkSpace.Controls[i].Location.X,
                        Y = PanWorkSpace.Controls[i].Location.Y,
                    });
                }
                else
                {
                    ds.Messages.Add(new UmlClassObjects
                    {
                        Name = PanWorkSpace.Controls[i].Name,
                        Text = PanWorkSpace.Controls[i].Text,
                        X = PanWorkSpace.Controls[i].Location.X,
                        Y = PanWorkSpace.Controls[i].Location.Y,
                    });
                }
            }
            
            for (int i = 0; i < relations.Count; i++)
            {
                ds.Relations.Add(relations[i]);
            }
            return ds;
        }

        private int FindDiagramByName(string name)
        {
            for (int i = 0; i < umpr.DiagramsNames.Count; i++)
            {
                if (umpr.DiagramsNames[i].Substring(3) == name)
                {
                    return i;
                }
            }
            return -1;
        }

        private void AddThings()//val ==> addfromindice
        {
            int maxSaves = 50;
            if (currentDiag == "Cla")
            {
                int i = FindDiagramByName(currentDiagName);
                if (i != -1)
                {
                    if (UR_Diagrams[i].diagClasses.Count < maxSaves)
                    {
                        for (int j = val; j < UR_Diagrams[i].diagClasses.Count; j++)
                        {
                            UR_Diagrams[i].diagClasses.RemoveAt(j);
                        }
                        UR_Diagrams[i].diagClasses.Add(URGetDiagClass());
                    }
                    else
                    {
                        UR_Diagrams[i].diagClasses.RemoveAt(0);
                        UR_Diagrams[i].diagClasses.Add(URGetDiagClass());
                    }
                }
            }
            else if (currentDiag == "Cas")
            {
                int i = FindDiagramByName(currentDiagName);
                if (i != -1)
                {
                    if (UR_Diagrams[i].diagUseCases.Count < maxSaves)
                    {
                        for (int j = val; j < UR_Diagrams[i].diagSequences.Count; j++)
                        {
                            UR_Diagrams[i].diagSequences.RemoveAt(j);
                        }
                        UR_Diagrams[i].diagUseCases.Add(URGetDiagUseCase());
                    }
                    else
                    {
                        UR_Diagrams[i].diagUseCases.RemoveAt(0);
                        UR_Diagrams[i].diagUseCases.Add(URGetDiagUseCase());
                    }
                }
            }
            else
            {
                int i = FindDiagramByName(currentDiagName);
                if (i != -1)
                {
                    if (UR_Diagrams[i].diagSequences.Count < maxSaves)
                    {
                        for (int j = val; j < UR_Diagrams[i].diagSequences.Count; j++)
                        {
                            UR_Diagrams[i].diagSequences.RemoveAt(j);
                        }
                        UR_Diagrams[i].diagSequences.Add(URGetDiagSequence());
                    }
                    else
                    {
                        UR_Diagrams[i].diagSequences.RemoveAt(0);
                        UR_Diagrams[i].diagSequences.Add(URGetDiagSequence());
                    }
                }
            }
            //UR_Diagrams[i]
        }

        private void TSBtn_Undo_Click(object sender, EventArgs e)
        {
            if (val > 0)
            {
                val--;
            }
            UndoAndRedo();
        }

        private void TSBtn_Redo_Click(object sender, EventArgs e)
        {
            val++;
            UndoAndRedo();
        }

        private void UndoAndRedo()
        {
            int i = FindDiagramByName(currentDiagName);
            if (i != -1)
            {
                if (val >= 0)
                {
                    if (currentDiag == "Cla")
                    {
                        if (val < UR_Diagrams[i].diagClasses.Count)
                        {
                            SetDiagClass(UR_Diagrams[i].diagClasses[val], true);
                        }
                        else
                        {
                            val = UR_Diagrams[i].diagClasses.Count - 1;
                        }
                    }
                    else if (currentDiag == "Cas")
                    {
                        if (val < UR_Diagrams[i].diagUseCases.Count)
                        {
                            SetDiagUseCase(UR_Diagrams[i].diagUseCases[val], true);
                        }
                        else
                        {
                            val = UR_Diagrams[i].diagUseCases.Count - 1;
                        }
                    }
                    else
                    {
                        if (val < UR_Diagrams[i].diagSequences.Count)
                        {
                            SetDiagSequence(UR_Diagrams[i].diagSequences[val], true);
                        }
                        else
                        {
                            val = UR_Diagrams[i].diagSequences.Count - 1;
                        }
                    }
                }
            }
        }

        private void ribbon1_DoubleClick(object sender, EventArgs e) //pour agrandir la fenetre
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
            }
        }

        bool drag = false;
        Point start_point = new Point(0, 0);
        private void Rib_MouseDown(object sender, MouseEventArgs e)
        {
            drag = true;
            start_point = new Point(e.X, e.Y);
        }

        private void Rib_MouseMove(object sender, MouseEventArgs e)
        {
            if (drag)
            {
                Point p = PointToScreen(e.Location);
                this.Location = new Point(p.X - start_point.X, p.Y - start_point.Y);
            }
        }

        private void Rib_MouseUp(object sender, MouseEventArgs e)
        {
            drag = false;
        }

        private void Copy_Click(object sender, EventArgs e)
        {
            switch (currentDiag)
            {
                case "Cla":
                    Clipboard.SetData("Cla",URGetDiagClass());
                    break;
                case "Cas":
                    Clipboard.SetData("Cas", URGetDiagUseCase());
                    break;
                case "Seq":
                    Clipboard.SetData("Seq", URGetDiagSequence());
                    break;
            }
        }

        private void Cut_Click(object sender, EventArgs e)
        {
            switch (currentDiag)
            {
                case "Cla":
                    Clipboard.SetData("Cla", URGetDiagClass());
                    break;
                case "Cas":
                    Clipboard.SetData("Cas", URGetDiagUseCase());
                    break;
                case "Seq":
                    Clipboard.SetData("Seq", URGetDiagSequence());
                    break;
            }
            PanWorkSpace.Controls.Clear();
            relations.Clear();
            PanWorkSpace.Invalidate();
        }

        private void NameIDVerifyDiagClass(DiagClass dc)
        {
            for (int i = 0; i < PanWorkSpace.Controls.Count; i++)
            {
                for (int j = 0; j < dc.UmlClasses.Count; j++)
                {
                    if (PanWorkSpace.Controls[i].Name == dc.UmlClasses[j].Name)
                    {
                        dc.ChangeNameID();
                        break;
                    }
                }
            }
        }

        private void NameIDVerifyUseCase(DiagUseCase du)
        {
            for (int i = 0; i < PanWorkSpace.Controls.Count; i++)
            {
                for (int j = 0; j < du.UseCases.Count; j++)
                {
                    if (PanWorkSpace.Controls[i].Name == du.UseCases[j].Name)
                    {
                        du.ChangeNameID();
                        break;
                    }
                }
                for (int j = 0; j < du.Actors.Count; j++)
                {
                    if (PanWorkSpace.Controls[i].Name == du.UseCases[j].Name)
                    {
                        du.ChangeNameID();
                        break;
                    }
                }
            }
        }

        private void Paste_Click(object sender, EventArgs e)
        {
            switch (currentDiag)
            {
                case "Cla":
                    AddThings();
                    val++;
                    if (Clipboard.ContainsData("Cla"))
                    {
                        DiagClass dc = (DiagClass)Clipboard.GetData("Cla");
                        NameIDVerifyDiagClass(dc);
                        SetDiagClass(dc, false);
                    }
                    AddThings();
                    break;
                case "Cas":
                    AddThings();
                    val++;
                    if (Clipboard.ContainsData("Cas"))
                    {
                        DiagUseCase du = (DiagUseCase)Clipboard.GetData("Cas");
                        NameIDVerifyUseCase(du);
                        SetDiagUseCase(du, false);
                    }
                    AddThings();
                    break;
                case "Seq":
                    AddThings();
                    val++;
                    if (Clipboard.ContainsData("Seq"))
                    {
                        SetDiagSequence((DiagSequence)Clipboard.GetData("Seq"), true);
                    }
                    AddThings();
                    break;
            }
        }

        private void SaveAs_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Text File(*."+currentDiag+")|*." + currentDiag;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                if (currentDiag == "Cla")
                {
                    int i = FindDiagClasses(currentDiagName);
                    if (i != -1)
                    {
                        ChangeEtatWorkSpace();
                        SaveDiagClass(diagClasses[i], sfd.FileName);
                    }
                }
                else if (currentDiag == "Cas")
                {
                    int i = FindDiagUseCases(currentDiagName);
                    if (i != -1)
                    {
                        ChangeEtatWorkSpace();
                        SaveDiagUseCase(diagUseCases[i], sfd.FileName);
                    }
                }
                else
                {
                    int i = FindDiagSequences(currentDiagName);
                    if (i != -1)
                    {
                        ChangeEtatWorkSpace();
                        SaveDiagSequence(diagSequences[i], sfd.FileName);
                    }
                }
            }
        }

        private void btnFermer_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAgrandir_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
            }
        }

        private void btnReduire_Click(object sender, EventArgs e)
        {
                this.WindowState = FormWindowState.Minimized;
        }

        private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://localhost/shi_site/elementor-22/");
        }

        private void btn_clearFrag_Click(object sender, EventArgs e)
        {
            rects.Clear();
            int i = FindDiagSequences(currentDiagName);
            if (i != -1)
            {
                diagSequences[i].Rects.Clear();

            }
            PanWorkSpace.Invalidate(); //appele pour redéssiner les composant(fragement)
        }
    }
}