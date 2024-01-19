using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KontrolaKadi
{
    class Kad : UserControl
    {
        private int id;
        public int ID 
        {
            get 
            {
                return id; 
            }
            set 
            { 
                id = value; 
            }

        }

        private string _displayName;

        public string DisplayName
        {
            get { return _displayName; }
            set 
            { 
                _displayName = value;
                displayNameChanged.Invoke(null, null);
            }
        }

        public CenteredLabel lblNextMove = new CenteredLabel();

        public EventHandler displayNameChanged;      

        public SPanel backImage = new SPanel();  
        public readonly int backImageHeight = 137;

        Rectangle outline;
        Pen outlinePen = new Pen(Color.Black, 1);
     
        bool mouseOver = false;
        bool designMode;

        InfoPanel infoIconPanel = new InfoPanel();
               
        public KadSubmenu submenu;

        public Urnik Urnik;     // just reference from submenu
        public SwitchesGroupbox SwitchesGroupbox; // just reference from submenu
        public PowerSetGroupbox PowerSetGroupbox; // just reference from submenu
        public PowerMonitorGroupBox PowerMonitorGroupbox; // just reference from submenu
        public PVSelector PvSelector;    // just reference from submenu

        public Kad():base()
        {
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true); // Prevents flicker

            designMode = (LicenseManager.UsageMode == LicenseUsageMode.Designtime);

            Height = 600;
            Width = 250;

            ManageBackImage();            

            if (designMode == true)
            {
                return;
            }
                        
            manageInfoIconsPanel();
            manageNextMoveLable();

            Paint += Kad_Paint;
            
            XmlController.XmlChanged += RefreshFromXml;

            this.Load += Kad_Load;           

        }

        private void manageNextMoveLable()
        {
            lblNextMove.Dock = DockStyle.None;
            lblNextMove.Top = 270;
            lblNextMove.Left = 25;
            lblNextMove.Width = 200;
            lblNextMove.Height = 25;
            lblNextMove.Text = "";
            Controls.Add(lblNextMove);
        }
        private void BackImage_Click(object sender, EventArgs e)
        {            
            submenu.Show();
            submenu.BringToFront();
            submenu.WindowState = FormWindowState.Normal;

        }

        private void Kad_Load(object sender, EventArgs e)
        {
            RefreshFromXml(null, null);

            submenu = new KadSubmenu()
            {
                ID = id,
                Width = 1500,
                Height = 900,
            };
            submenu.Show(); submenu.Hide(); //triggers the load of subsequent components

            Urnik = submenu.Urnik;
            SwitchesGroupbox = submenu.SwitchesGroupbox;
            PowerSetGroupbox = submenu.PowerSetGroupbox;
            PowerMonitorGroupbox = submenu.PowerMonitorGroupBox;
            PvSelector = submenu.PVSelector;

            backImage.Click += BackImage_Click;
            Urnik.NextEventDescription_Changed += Urnik_NextEventDescription_Changed;

        }

        private void Urnik_NextEventDescription_Changed(object sender, EventArgs e)
        {
            Urnik_NextEventDescription_Changed();
        }

        void Urnik_NextEventDescription_Changed()
        {
            if (InvokeRequired)
            {
                var m = new MethodInvoker(Urnik_NextEventDescription_Changed);
                Invoke(m);
                return;
            }

            lblNextMove.Text = Urnik.NextEventDescription;
        }

        void RefreshFromXml(object sender, EventArgs e)
        {
            try
            {
                var m = new MethodInvoker(delegate 
                { 
                    DisplayName = XmlController.GetImeKadi(ID); 
                });
                FormControl.Gui.Invoke(m);
                
            }
            catch (Exception ex)
            {
                throw new Exception("Internal error inside event RefreshFromXml. Error details: " + ex.Message);
            }                         
        }

        void manageInfoIconsPanel()
        {
            infoIconPanel.Top = backImage.Bottom + 50;
            infoIconPanel.BackColor = Color.LightGray;
            infoIconPanel.Left = ((Width - infoIconPanel.Width) / 2);
            Controls.Add(infoIconPanel);
        }
                           
        private void Kad_Paint(object sender, PaintEventArgs e)
        {
            if (mouseOver)
            {
                DrawOutlineOnMouseOver(e);
            }
        }

        void ManageBackImage()
        {
            backImage.Top = 80;
            backImage.Height = 180;
            backImage.BackgroundImageLayout = ImageLayout.Stretch;             

            if (designMode == true)
            {
                return;
            }

            backImage.MouseEnter += BackImage_MouseEnter;
            backImage.MouseLeave += BackImage_MouseLeave;
        }

        void DrawOutlineOnMouseOver(PaintEventArgs e)
        {
            outline = new Rectangle(new Point(backImage.Left, backImage.Top), new Size(backImage.Width, backImage.Height));            
            outline.Inflate(2,2);
            e.Graphics.DrawRectangle(outlinePen, outline);
        }

        private void BackImage_MouseEnter(object sender, EventArgs e)
        {
            mouseOver = true;
            Invalidate();
        }
        private void BackImage_MouseLeave(object sender, EventArgs e)
        {
            mouseOver = false;
            Invalidate();
        }

    }

    public partial class ErrDialogForm : Form
    {
        public ErrDialogForm()
        {
            
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // Close the form when the OK button is clicked
            this.Close();
        }
    }


    class EnojnaKad : Kad
    {
        Color BarvaKadi = Color.Red;
        SPanel rectBarvaKadi;

        KadNaslov lblKadNaslov = new KadNaslov();

        public EnojnaKad() : base()
        {
            backImage.BackgroundImage = Properties.Resources.Kad___konstrukcija;
            backImage.Width = 100;
            backImage.Left = ((base.Width - backImage.Width) / 2) -5;
            base.Controls.Add(backImage);
            barvajKad();
            backImage.Controls.Add(rectBarvaKadi);            
            rectBarvaKadi.Controls.Add(lblKadNaslov);        
            this.Paint += EnojnaKad_Paint;
            base.displayNameChanged += displayName;               

        }

        void displayName(object sender, EventArgs e)
        {
            MethodInvoker m = new MethodInvoker(delegate 
            {
                lblKadNaslov.Text = base.DisplayName;
            });
            m.Invoke();
        }
        
        private void EnojnaKad_Paint(object sender, PaintEventArgs e)
        {
           
        }

        void barvajKad()
        {
            rectBarvaKadi = new SPanel()
            {
                Top = 35,
                Left = 10,
                Width = 80,
                Height = 143,
                BackColor = BarvaKadi,
                Enabled = false
                
            };
            
        }
    }
}
