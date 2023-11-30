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
        public int ID {get;set;} = 0; // set in the designer or dinamically

        public SPanel backImage = new SPanel();  
        public readonly int backImageHeight = 137;

        Rectangle outline;
        Pen outlinePen = new Pen(Color.Black, 1);
     
        bool mouseOver = false;
        bool designMode;

        InfoPanel infoIconPanel = new InfoPanel();

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

            Paint += Kad_Paint;
            
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
