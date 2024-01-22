using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KontrolaKadi
{
    public class CenteredLabel : Label
    {
        public CenteredLabel() : base()
        {
            AutoSize = false;
            TextAlign = ContentAlignment.MiddleCenter;
            Dock = DockStyle.None;
            Padding = new Padding(0);
        }

    }

    public class KadNaslov : CenteredLabel
    {
        public KadNaslov()
        {
            Font = new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif, 16, System.Drawing.FontStyle.Bold);
            BackColor = Color.Red;
            Enabled = false;            
            Text = "ABCD";
            Dock = DockStyle.Fill;
        }
    }
}