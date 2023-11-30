using System;
using System.Collections.Generic;
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
            TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            Dock = DockStyle.Fill;
        }

    }

    public class KadNaslov : CenteredLabel
    {
        public KadNaslov()
        {
            Font = new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif, 16, System.Drawing.FontStyle.Bold);
            Enabled = false;            
            Text = "ABCD";
        }
    }
}