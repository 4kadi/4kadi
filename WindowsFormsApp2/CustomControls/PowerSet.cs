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
    public partial class PowerSet : SGroupBox
    {
        private PlcVars.Word valuePowerset;

        bool _ReadOnly = false;
        public bool ReadOnly { 
            get { return _ReadOnly; } 
            set { _ReadOnly = value; readonlyValueUpdated(); } }

        public PlcVars.Word ValuePowerset
        {
            get { return valuePowerset; }
            set 
            { 
                valuePowerset = value;
                if (ValuePowerset != null)
                {
                    valuePowerset.ValueChanged += Value_Powerset_ValueChanged;
                }
            }
        }

        public string Unit { get; set; } = "W";

        Label title;
        protected Button minus, plus;
        protected TextBox tb;
        public PowerSet()
        {
            Font = new Font("arial", 10);
            Width = 180;
            Height = 60;

            title = new Label
            {
                Top = 25,
                Left = 5,
                Width = 40,
                Text = "Moč:"
            };
            Controls.Add(title);

            minus = new Button()
            {
                Top = title.Top - 2,
                Left = title.Right + 5,
                Width = 25,
                Text = "-",
                Font = new Font("arial", 10)
            };
            Controls.Add(minus);

            tb = new TextBox()
            {
                Left = minus.Right + 5,
                Width = 60,
                Top = title.Top - 2,
                ReadOnly = true
            };
            Controls.Add(tb);

            plus = new Button()
            {
                Top = title.Top - 2,
                Left = tb.Right + 5,
                Width = minus.Width,
                Text = "+",
                Font = new Font("arial", 10)
            };
            Controls.Add(plus);

            Enter += PowerSet_Enter;
        }

        private void PowerSet_Enter(object sender, EventArgs e)
        {
            try
            {
                var form = FindForm();
                var arrayOfControls = form.Controls.Find("unfocus", true);
                var tb = (TextBox)arrayOfControls[0];
                tb.Focus();
            }
            catch 
            {}
        
        }

        void readonlyValueUpdated()
        {
            if (_ReadOnly)
            {
                minus.Visible = false;
                plus.Visible = false;
            }
            else
            {
                minus.Visible = true;
                plus.Visible = true;
            }
        }

        private void Value_Powerset_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                var m = new MethodInvoker(delegate 
                {
                    tb.Text = valuePowerset.Value_short.ToString() + Unit;
                });

                Invoke(m);
                
            }
            catch (Exception ex)
            {
                
            }
           
        }
    }
}
