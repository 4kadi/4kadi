using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KontrolaKadi
{
    public partial class KadSubmenu : Form
    {
        public int ID { get; set; } = 0;
        public Urnik Urnik;
        public KadSubmenu()
        {
            bool designMode = (LicenseManager.UsageMode == LicenseUsageMode.Designtime);
            InitializeComponent();

            if (designMode)
            {
                return;
            }
            
            manageUrnik();
            Click += KadSubmenu_Click;
            LostFocus += KadSubmenu_LostFocus;
            FormClosing += KadSubmenu_FormClosing;
            Load += KadSubmenu_Load;
        }

        private void KadSubmenu_Load(object sender, EventArgs e)
        {
            manageID();
        }

        void manageID()
        {
            if (ID == 0)
            {
                throw new Exception("You should set ID property to the control KadSubmenu.");
            }
        }

        private void KadSubmenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            close();
        }

        private void KadSubmenu_LostFocus(object sender, EventArgs e)
        {
            close();
        }

        private void KadSubmenu_Click(object sender, EventArgs e)
        {
            var ee = (MouseEventArgs)e;
            var btn = ee.Button;

            if (ee.Button == MouseButtons.Right)
            {
                close();
            }           
        }

        void manageUrnik()
        {
            Urnik = new Urnik()
            { 
                Text = "Urniki",
                Top = 10,
                Left = 10
            };
            Controls.Add(Urnik);
        }

        void close()
        {
            Hide();
        }
    }
}
