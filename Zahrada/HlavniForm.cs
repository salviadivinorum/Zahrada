﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zahrada
{
    public partial class HlavniForm : Form
    {

        #region Clenske promenne Hlavniho formulare

        // promenna udalost na klikani tlacitek
        public event EventHandler OnButtonZoomPusClick;
        public event EventHandler OnButtonZoomMinusClick;
        private bool mrizZapnuta = true;
        private int ulozGrid = 0;
        private const int RGBMAX = 255;
        private string CustomPlanSizeString { get; set; } // automaticka vlastnost pro vlastni velikost planu ...
        private float CustomX { get; set; } // custom sirka planu
        private float CustomY { get; set; }// custom vyska planu

        //public event ObjectSelectedEventHandler ObjectSelected;

        #endregion

        #region Konstruktor Hlavniho okna
        public HlavniForm()
        {
            InitializeComponent();
            myInit();
        }

        /// <summary>
        /// Na vlozeny toolBox Nastroje nalinkuje platno vlozenePlatno
        /// </summary>
        private void myInit()  // vlozenyToolBox ovlada vlozenePlatno
        {
            vlozenyToolBox.SetPlatno(vlozenePlatno); // nalinkuju vlozenyToolBox z navrhare na vlozenePlatno z navrhare
                                                     //vlozenePlatno.Platno_DoubleClick(null, null); // timto si pomaham - inicializuju vlastne mujFiltredPropertyGrid a Platno - ramecek A4

            // uprava platna do Zoom = 0.25, posun o 100 pixelu dolu a doprava
            vlozenePlatno.dx = 100;
            vlozenePlatno.dy = 100;
            vlozenePlatno.Zoom = 0.25f;
            //vlozenePlatno.NajdiZoomComboBoxvMainForm();
            //vlozenePlatno.UpravZoomVComboBoxu(1);

            vlozenyToolBox.NajdiUndoReodBtnsVmainForm();
            vlozenePlatno.NajdiStatusStripVmainForm(); // potrebuju pro text ve statusstripu
            toolStrip1.BackColor = Color.FromArgb(17, Color.CadetBlue);
            statusStrip.BackColor = Color.FromArgb(17, Color.CadetBlue);


            //this.vlozenePlatno.ParentForm = this;
            //OnButtonZoomPusClick += new EventHandler(KlikNaPlus); // priradim udalost na tlacitko + Zoom
            //OnButtonZoomMinusClick += new EventHandler(KlikNaMinus); // priradim udal na tlac - Zoom

            // ObjectSelected += new ObjectSelectedEventHandler(OnBasicObjectSelected);
            //vlozenePlatno.SetMainForm(this);
        }
        #endregion

        #region Obsluha podruznych udalosti - Load, apod
        private void hlavniFormularoveOkno_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized; // maximalizace formul. okna
            penWidthtoolStripComboBox.SelectedIndex = 1; // comboboxy nahe v toolstripu
            colorFillingOnOffToolStripComboBox.SelectedIndex = 0;
            textureFillingOnOffToolStripComboBox.SelectedIndex = 0;

            //gridToolStripComboBox.SelectedIndex = 0;
            //zoomToolStripComboBox.SelectedIndex = 1;
           // closedToolStripComboBox.SelectedIndex = 0;
            vlozenePlatno.Zoom = 0.25f;
            
            
            // vlozenePlatno.Scale(new Size(2f, 2f))
        }

        #endregion

       
        

        // pomocnametoda - zneguje mi barvu ....
        private Color InvertMeAColour(Color ColourToInvert)
        {
            return Color.FromArgb(RGBMAX - ColourToInvert.R,
              RGBMAX - ColourToInvert.G, RGBMAX - ColourToInvert.B);
        }


        // Label namisto tlacitka PenColor        
        #region PenColorLabel
        private void PokusToolStripLabel_Click(object sender, EventArgs e)
        {
            mujColorDialog.ShowDialog(this);
            vlozenePlatno.SetPenColor(mujColorDialog.Color);
            vlozenePlatno.ChangeOption("select");

            //vlozenePlatno.PushSelectionToShowInCustomGrid
            vlozenePlatno.PushPlease();
            vlozenePlatno.Focus();
            vlozenePlatno.Redraw(true);
        }

        private void PenColorToolStripLabel_MouseEnter(object sender, EventArgs e)
        {
            Color barva = vlozenePlatno.creationPenColor;
            Color barvaPera = InvertMeAColour(vlozenePlatno.creationPenColor);
            // Cast to allow reuse of method.
            ToolStripItem tsi = (ToolStripItem)sender;

            // Create semi-transparent picture.
            Bitmap bm = new Bitmap(tsi.Width, tsi.Height);
            for (int y = 0; y < tsi.Height; y++)
            {
                for (int x = 0; x < tsi.Width; x++)
                    //bm.SetPixel(x, y, Color.FromArgb(150, barva));
                    bm.SetPixel(x, y, barva);
            }

            // Set background.
            tsi.BackgroundImage = bm;
            tsi.ForeColor = barvaPera;

        }


        private void PenColorToolStripLabel_MouseLeave(object sender, EventArgs e)
        {
            (sender as ToolStripItem).BackgroundImage = null;
            (sender as ToolStripItem).ForeColor = Color.Black;
        }

        #endregion

        
        // Label namisto tlacitka FillColor
        #region FillColor Label
        private void fillColorToolStripLabel_Click(object sender, EventArgs e)
        {
            //mujColorDialog.Color = fillColorToolStripButton.BackColor;
            mujColorDialog.ShowDialog(this);
            //fillColorToolStripButton.BackColor = mujColorDialog.Color;
            vlozenePlatno.SetFillColor(mujColorDialog.Color);
            vlozenePlatno.ChangeOption("select");

            vlozenePlatno.PushPlease();
            vlozenePlatno.Focus();
            vlozenePlatno.Redraw(true);
        }


        private void fillColorToolStripLabel_MouseEnter(object sender, EventArgs e)
        {
            Color barva = vlozenePlatno.creationFillColor;
            Color barvaPera = InvertMeAColour(vlozenePlatno.creationFillColor);
            // Cast to allow reuse of method.
            ToolStripItem tsi = (ToolStripItem)sender;

            // Create semi-transparent picture.
            Bitmap bm = new Bitmap(tsi.Width, tsi.Height);
            for (int y = 0; y < tsi.Height; y++)
            {
                for (int x = 0; x < tsi.Width; x++)
                    //bm.SetPixel(x, y, Color.FromArgb(150, barva));
                bm.SetPixel(x, y, barva);
            }

            // Set background.
            tsi.BackgroundImage = bm;
            tsi.ForeColor = barvaPera;
        }


        private void fillColorToolStripLabel_MouseLeave(object sender, EventArgs e)
        {
            (sender as ToolStripItem).BackgroundImage = null;
            (sender as ToolStripItem).ForeColor = Color.Black;
        }
        #endregion


        // Label namisto tlacitka VzorTextury
        #region Vzor Textury Label
        private void texturaToolStripLabel_Click(object sender, EventArgs e)
        {
            
            vlozenePlatno.TextureLoader();
            vlozenePlatno.ChangeOption("select");

            vlozenePlatno.PushPlease();
            vlozenePlatno.Focus();
            vlozenePlatno.Redraw(true);
        }

        

        private void texturaToolStripLabel_MouseEnter(object sender, EventArgs e)
        {
            
            Image bitm = vlozenePlatno.creationTexturePattern.Image;
            

            // Cast to allow reuse of method.
            ToolStripItem tsi = (ToolStripItem)sender;

            
            // Set background.
            tsi.BackgroundImage = bitm;
            tsi.ForeColor = Color.White;
            //tsi.ImageTransparentColor = Color.LimeGreen;


        }

        private void texturaToolStripLabel_MouseLeave(object sender, EventArgs e)
        {
            (sender as ToolStripItem).BackgroundImage = null;
            (sender as ToolStripItem).ForeColor = Color.Black;
        }




        #endregion
        

       
        // zakladni Zoom In + 
        public void zoomINtoolStripButton_Click(object sender, EventArgs e)
        {

            // int sirkaPlatna = vlozenePlatno.Size.Width;
            // int vyskaPlatna = vlozenePlatno.Size.Height;


            //vlozenePlatno.ZoomIn();


            int ix = (int)vlozenePlatno.Size.Width / 2;
            int iy = (int)vlozenePlatno.Size.Height / 2;


            if (vlozenePlatno.Zoom <= 15f)                
            {
                if (vlozenePlatno.Fit2grid & vlozenePlatno.Mřížka > 0)
                {
                    //int grid = gridSize;
                    vlozenePlatno.dx = (int)(vlozenePlatno.dx - (ix / (vlozenePlatno.Zoom * 2)));
                    vlozenePlatno.dy = (int)(vlozenePlatno.dy - (iy / (vlozenePlatno.Zoom * 2)));

                    vlozenePlatno.dx = vlozenePlatno.Mřížka * ((vlozenePlatno.dx) / vlozenePlatno.Mřížka);
                    vlozenePlatno.dy = vlozenePlatno.Mřížka * ((vlozenePlatno.dy) / vlozenePlatno.Mřížka);

                    vlozenePlatno.ZoomIn();
                    //gridSize = grid;
                }
                else
                {
                    vlozenePlatno.dx = (int)(vlozenePlatno.dx - (ix / (vlozenePlatno.Zoom * 2)));
                    vlozenePlatno.dy = (int)(vlozenePlatno.dy - (iy / (vlozenePlatno.Zoom * 2)));
                    vlozenePlatno.ZoomIn();
                }
            }   

        }

        // zkaldni Zoom Out -
        public void zoomOUTtoolStripButton_Click(object sender, EventArgs e)
        {
            
            //vlozenePlatno.ZoomOut();

            
            int ix = (int)vlozenePlatno.Size.Width / 2;
            int iy = (int)vlozenePlatno.Size.Height / 2;


            if (vlozenePlatno.Zoom > 0.01f & vlozenePlatno.Zoom <= 21f)
            {
                if (vlozenePlatno.Fit2grid & vlozenePlatno.Mřížka > 0)
                {
                    //int gr = gridSize;
                    vlozenePlatno.dx = (int)(vlozenePlatno.dx + (ix / (vlozenePlatno.Zoom)));
                    vlozenePlatno.dy = (int)(vlozenePlatno.dy + (iy / (vlozenePlatno.Zoom)));

                    vlozenePlatno.dx = vlozenePlatno.Mřížka * ((vlozenePlatno.dx) / vlozenePlatno.Mřížka);
                    vlozenePlatno.dy = vlozenePlatno.Mřížka * ((vlozenePlatno.dy) / vlozenePlatno.Mřížka);

                    vlozenePlatno.ZoomOut();
                    //gridSize = gr;
                }
                else
                {
                    vlozenePlatno.dx = (int)(vlozenePlatno.dx + (ix / (vlozenePlatno.Zoom)));
                    vlozenePlatno.dy = (int)(vlozenePlatno.dy + (iy / (vlozenePlatno.Zoom)));
                    vlozenePlatno.ZoomOut();
                }

            }


           
            
        }


        

        // FILLING barvou On/Off
        private void fillingOnOffToolStripComboBox_DropDownClosed(object sender, EventArgs e)
        {
            vlozenePlatno.Focus();
            if (colorFillingOnOffToolStripComboBox.SelectedIndex == 0)
            {
                // fillColorToolStripButton.BackColor = Color.Transparent;
                vlozenePlatno.SetColorFilled(false);                
                //vlozenePlatno.Redraw(true);             
            }
            else if (colorFillingOnOffToolStripComboBox.SelectedIndex == 1)
            {
                textureFillingOnOffToolStripComboBox.SelectedIndex = 0;
                vlozenePlatno.SetTextureFilled(false);               

                    
                vlozenePlatno.SetColorFilled(true);
                
               // vlozenePlatno.Redraw(true);
                vlozenePlatno.ChangeOption("select");
            }

            vlozenePlatno.PushPlease();
            vlozenePlatno.Redraw(true);
            vlozenePlatno.Focus();

        }

        // FILLING texturou Ano/Ne
        private void textureFillingOnOffToolStripComboBox_DropDownClosed(object sender, EventArgs e)
        {  

            if (textureFillingOnOffToolStripComboBox.SelectedIndex == 0)
            {
                vlozenePlatno.SetTextureFilled(false);
              
            }
            else
            {
                vlozenePlatno.SetTextureFilled(true);
                colorFillingOnOffToolStripComboBox.SelectedIndex = 0;
                vlozenePlatno.SetColorFilled(false);
               
            }

            vlozenePlatno.PushPlease();
            vlozenePlatno.Redraw(true);
            vlozenePlatno.Focus();
        }


        // Exit a Closed()
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
          
        // osetreni undo/redo sipek
        #region Undo/Redo sipky
        private void undoToolStripButton_Click(object sender, EventArgs e)
        {
            vlozenePlatno.Undo();
            undoToolStripButton.Enabled = vlozenePlatno.UndoEnabled();
            redoToolStripButton.Enabled = vlozenePlatno.RedoEnabled();
        }

        private void redoToolStripButton_Click(object sender, EventArgs e)
        {
            vlozenePlatno.Redo();
            redoToolStripButton.Enabled = vlozenePlatno.RedoEnabled();
            undoToolStripButton.Enabled = vlozenePlatno.UndoEnabled();
        } 
        #endregion

        // Dodelat Pen Width - sirku pera ....
        private void penWidthtoolStripComboBox_DropDownClosed(object sender, EventArgs e)
        {
            int index = penWidthtoolStripComboBox.SelectedIndex;
            switch (index)
            {
                case 0:
                    vlozenePlatno.SetPenWidth(0.5f);
                    break;
                case 1:
                    vlozenePlatno.SetPenWidth(1f);
                    break;
                case 2:
                    vlozenePlatno.SetPenWidth(2f);
                    break;
                case 3:
                    vlozenePlatno.SetPenWidth(5f);
                    break;
                case 4:
                    vlozenePlatno.SetPenWidth(10f);
                    break;
                case 5:
                    vlozenePlatno.SetPenWidth(20f);
                    break;
                case 6:
                    vlozenePlatno.SetPenWidth(50f);
                    break;
            }



            vlozenePlatno.PushPlease();
            vlozenePlatno.Focus();
            vlozenePlatno.Redraw(true);
        } 
        

       

        // Save As tlacitko ...
        private void saveAsToolStripButton_Click(object sender, EventArgs e)
        {
            vlozenyToolBox.CheckedRBSave();
            vlozenePlatno.Saver();
            
            
            //vlozenePlatno.shapes.indeOfSavedPlan = FrameToolStripDropDownButton.Se
        }

        // Open tlacitko ...
        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            vlozenePlatno.Loader();
            vlozenyToolBox.CheckRBLoad2(); // CheckedRBLoad2(); // radiobuttony chcecke obnovi
            vlozenyToolBox.Refresh();
            MarkFrameToolStripMenuItem();
        }

        // pomocna metoda po upsesnem Loadu ...
        private void MarkFrameToolStripMenuItem()
        {
            UnmarkToolStripDropDownItems(FrameToolStripDropDownButton);
            int index = vlozenePlatno.shapes.indeOfSavedPlan;
            switch (index)
            {
                case 0:
                    //A4PortraitToolStripMenuItem.Checked = true;
                    A4PortraitToolStripMenuItem_Click(null, null);
                    break;
                case 1:
                    //A4LandcapeToolStripMenuItem.Checked = true;
                    A4LandcapeToolStripMenuItem_Click(null, null);
                    break;
                case 2:
                    //A3PortraitToolStripMenuItem.Checked = true;
                    A3PortraitToolStripMenuItem_Click(null, null);
                    break;
                case 3:
                    //A3LandcapeToolStripMenuItem.Checked = true;
                    A3LandcapeToolStripMenuItem_Click(null, null);
                    break;
                case 4:
                    CustumSizeToolStripMenuItem.Checked = true;
                    vlozenePlatno.Rámeček = true;
                    float sirka = vlozenePlatno.Šířka / 100;
                    float vyska = vlozenePlatno.Výška / 100;
                    CustomPlanSizeString = "Vlastní plán " + sirka.ToString() + "m x " + vyska.ToString() + "m";
                    FrameToolStripDropDownButton.Text = CustomPlanSizeString;
                    CustumSizeToolStripMenuItem.Text = CustomPlanSizeString;
                    vlozenePlatno.Focus();
                    vlozenePlatno.Redraw(true);
                    break;
                    /*
                default:
                    //A4PortraitToolStripMenuItem.Checked = true;
                    A4PortraitToolStripMenuItem_Click(null, null);
                    break;
                    */

            }
        }

        // Print Preview tlacitko ...
        private void printPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
           // float z = vlozenePlatno.Zoom;
            vlozenePlatno.PreviewBeforePrinting(0.25f);
        }

       
        // Print tlacitko ...
        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            vlozenePlatno.PrintMe();
            /*
            vlozenePlatno.printDialog1.AllowSomePages = true;
            vlozenePlatno.printDialog1.ShowHelp = true;
            vlozenePlatno.printDialog1.Document = vlozenePlatno.docToPrint;
            DialogResult result = vlozenePlatno.printDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                vlozenePlatno.docToPrint.Print();
            }
            */

        }

        // Export To tlacitko ...
        private void exportToJpgpngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            vlozenePlatno.ExportTo();
        }


        #region Zadavani rozmeru planu
        // Rozmery platna na DropDownButton ... pro jednotlive polozky tohoto DropDown tlacitka
       
        private void A3LandcapeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            vlozenePlatno.Rámeček = true;
            vlozenePlatno.Šířka = 4200;
            vlozenePlatno.Výška = 2970;

            vlozenePlatno.shapes.indeOfSavedPlan = 3; // pro budouci load planu
            //Unmark();
            UnmarkToolStripDropDownItems(FrameToolStripDropDownButton);
            A3LandcapeToolStripMenuItem.Checked = true;
            FrameToolStripDropDownButton.Text = A3LandcapeToolStripMenuItem.Text; //"Plán 42m x 29,7m";

            vlozenePlatno.PushPlease();
            vlozenePlatno.Focus();
            vlozenePlatno.Redraw(true);
        }

        private void A3PortraitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            vlozenePlatno.Rámeček = true;
            vlozenePlatno.Šířka = 2970;
            vlozenePlatno.Výška = 4200;

            vlozenePlatno.shapes.indeOfSavedPlan = 2; // pro budouci load planu
            //Unmark();
            UnmarkToolStripDropDownItems(FrameToolStripDropDownButton);
            A3PortraitToolStripMenuItem.Checked = true;
            FrameToolStripDropDownButton.Text = A3PortraitToolStripMenuItem.Text; //"Plán 29,7m x 42m";

            vlozenePlatno.PushPlease();
            vlozenePlatno.Focus();
            vlozenePlatno.Redraw(true);
        }

        private void A4LandcapeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            vlozenePlatno.Rámeček = true;
            vlozenePlatno.Šířka = 2970;
            vlozenePlatno.Výška = 2100;

            vlozenePlatno.shapes.indeOfSavedPlan = 1; // pro budouci load planu
            // Unmark();
            UnmarkToolStripDropDownItems(FrameToolStripDropDownButton);
            A4LandcapeToolStripMenuItem.Checked = true;
            FrameToolStripDropDownButton.Text = A4LandcapeToolStripMenuItem.Text; //"Plán 29,7m x 21m";

            vlozenePlatno.PushPlease();
            vlozenePlatno.Focus();
            vlozenePlatno.Redraw(true);

        }

        private void A4PortraitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            vlozenePlatno.Rámeček = true;
            vlozenePlatno.Šířka = 2100;
            vlozenePlatno.Výška = 2970;
            
            vlozenePlatno.shapes.indeOfSavedPlan = 0; // pro budouci load planu
            // Unmark();
            UnmarkToolStripDropDownItems(FrameToolStripDropDownButton);
            A4PortraitToolStripMenuItem.Checked = true;
            FrameToolStripDropDownButton.Text = A4PortraitToolStripMenuItem.Text;   //"Plán 21m x 29,7m";

            vlozenePlatno.PushPlease();
            vlozenePlatno.Focus();
            vlozenePlatno.Redraw(true);
        }


       
        private void CustumSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (RozmerPlanuForm customSizeWindow = new RozmerPlanuForm())
            {
                if (CustomPlanSizeString != "")
                {
                    customSizeWindow.XdimensionTextBox.Text = CustomX.ToString();
                    customSizeWindow.YdimensionTextBox.Text = CustomY.ToString();

                }
                DialogResult dr = customSizeWindow.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    vlozenePlatno.Rámeček = true;
                    vlozenePlatno.Šířka = (int)(customSizeWindow.x * 100);
                    vlozenePlatno.Výška = (int)(customSizeWindow.y * 100);
                    CustomX = customSizeWindow.x;
                    CustomY = customSizeWindow.y;
                    CustomPlanSizeString = "Vlastní plán " + customSizeWindow.x.ToString() + "m x " + customSizeWindow.y.ToString() + "m";
                    FrameToolStripDropDownButton.Text = CustomPlanSizeString;
                    CustumSizeToolStripMenuItem.Text = CustomPlanSizeString;
                    //Unmark();
                    UnmarkToolStripDropDownItems(FrameToolStripDropDownButton);
                    CustumSizeToolStripMenuItem.Checked = true;

                    vlozenePlatno.shapes.indeOfSavedPlan = 4; // pro budouci load planu

                }
                else
                {
                    Unmark2Special();

                }

            }

            vlozenePlatno.PushPlease();
            vlozenePlatno.Focus();
            vlozenePlatno.Redraw(true);

            //vlozenePlatno.shapes.indeOfSavedPlan = 0; // pro budouci load planu

        }

        // pomocna metoda k un-check v tool strip DropDownButton - vsem polozkam Item
        /*
        private void Unmark()
        {
            object ob1;
            object ob2;
            ToolStripMenuItem b;
            foreach (object a in FrameToolStripDropDownButton.DropDownItems)
            {
                ob1 = a.GetType();
                ob2 = typeof(ToolStripSeparator);

                if (ob1 != ob2)
                {
                    b = (ToolStripMenuItem)a;
                    b.Checked = false;
                }
            }
        }

        */
        // odznaci ve Vlastni rozmeru to co je potreba ... a oznaci co je treba
        private void Unmark2Special()
        {
            object ob1;
            object ob2;
            ToolStripMenuItem b;
            foreach (object a in FrameToolStripDropDownButton.DropDownItems)
            {
                ob1 = a.GetType();
                ob2 = typeof(ToolStripSeparator);

                if (ob1 != ob2)
                {
                    b = (ToolStripMenuItem)a;
                    //b.Checked = false;
                    if (b.Checked)
                    {
                        FrameToolStripDropDownButton.Text = b.Text;
                        if (b.Name == "CustumSizeToolStripMenuItem")
                        {
                            FrameToolStripDropDownButton.Text = CustomPlanSizeString;
                            b.Text = CustomPlanSizeString;
                        }
                    }
                }
            }
        }

        private void OffFrameToolStripMenuItem_Click(object sender, EventArgs e)
        {
           // Unmark();
            UnmarkToolStripDropDownItems(FrameToolStripDropDownButton);
            OffFrameToolStripMenuItem.Checked = true;
            vlozenePlatno.Rámeček = false;
            FrameToolStripDropDownButton.Text = "Rozměry plánu vypnuty";

            vlozenePlatno.PushPlease();
            vlozenePlatno.Redraw(true);

        }

       

        private void YsnapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            vlozenePlatno.Fit2grid = true;
            SnapToolStripDropDownButton.Text = "Přichytávat";
            YsnapToolStripMenuItem.Checked = true;
            NsnapToolStripMenuItem.Checked = false;

           
        }

        private void NsnapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            vlozenePlatno.Fit2grid = false;
            SnapToolStripDropDownButton.Text = "Nepřichytávat";
            NsnapToolStripMenuItem.Checked = true;
            YsnapToolStripMenuItem.Checked = false;
        }

        private void GridToolStripDropDownButton_DropDownClosed(object sender, EventArgs e)
        {
            
        }

        private void GridToolStripDropDownButton_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string text = e.ClickedItem.Text;
            ToolStripMenuItem item = (ToolStripMenuItem)e.ClickedItem;

            int grs;
            //MessageBox.Show(text);
            switch (text)
            {
                case "Mříž Vypnuta":
                    grs = 0;
                    //UnmarkToolStripDropDownItems(GridToolStripDropDownButton);
                    //item.Checked = true;
                    //item.
                    break;
                case "Mříž 1cm":
                    grs = 1;
                    break;
                case "Mříž 5cm":
                    grs = 5;
                    break;
                case "Mříž 10cm":
                    grs = 10;
                    break;
                case "Mříž 25cm":
                    grs = 25;
                    break;
                case "Mříž 50cm":
                    grs = 50;
                    break;
                case "Mříž 100cm":
                    grs = 100;
                    break;
                case "Mříž 250cm":
                    grs = 250;
                    break;
                case "Mříž 500cm":
                    grs = 500;
                    break;
                default:
                    grs = 0;
                    break;
            }
            UnmarkToolStripDropDownItems(GridToolStripDropDownButton);
            item.Checked = true;
            GridToolStripDropDownButton.Text = item.Text;
            vlozenePlatno.Mřížka = grs;

            vlozenePlatno.PushPlease();
            vlozenePlatno.Focus();

        }

        #endregion

        // pomocna metoda k un-check v tool strip DropDownButton - vsem polozkam Item
        private void UnmarkToolStripDropDownItems(ToolStripDropDownButton button)
        {
            object ob1;
            object ob2;
            ToolStripMenuItem b;
            foreach (object a in button.DropDownItems)
            {
                ob1 = a.GetType();
                ob2 = typeof(ToolStripSeparator);

                if (ob1 != ob2)
                {
                    b = (ToolStripMenuItem)a;
                    b.Checked = false;
                }
            }

        }

        
    }

}
