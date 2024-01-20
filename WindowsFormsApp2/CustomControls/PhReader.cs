using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KontrolaKadi
{
    public class PhReader : SGroupBox
    {
        bool initialized = false;

        private PlcVars.Word _Ph;

        TbPhShow tbPh1;
        Label trenurno;

        public PlcVars.Word Ph
        {
            get { return _Ph; }
            set
            {
                _Ph = value;
                Any_ValueChanged(null, EventArgs.Empty);
            }
        }

        public PhReader()
        {
            Font = new Font("arial", 15, FontStyle.Bold);

            tbPh1 = new TbPhShow()
            {
                Top = 35,
                Left = 75
            };
            Controls.Add(tbPh1);

            trenurno = new Label()
            {
                Font = new Font("arial", 10),
                Top = 35 + 5,
                Left = 5,
                Text = "Trenutno:"
            };
            Controls.Add(trenurno);
        }

        private void Any_ValueChanged(object sender, EventArgs e)
        {
            Any_ValueChanged();
        }

        void Any_ValueChanged()
        {
            InitializeIfPossible();

            if (!initialized)
            {
                return;
            }

            if (InvokeRequired)
            {
                var m = new MethodInvoker(Any_ValueChanged);
                Invoke(m);
                return;
            }

            tbPh1.Text = Ph.Value_short.ToString();
        }

        void InitializeIfPossible()
        {
            if (initialized)
            {
                return;
            }

            if (Ph != null)
            {
                Ph.ValueChanged += Any_ValueChanged;
                initialized = true;
            }
        }
    }

    class TbPhShow : TextBox
    {
        string Unit = "";
        override public string Text
        {
            get { return base.Text.Replace(Unit, ""); }
            set { UpdateText(value); }
        }
        public TbPhShow()
        {
            ReadOnly = true;
            Width = 80;
            Enter += PhShow_Enter;
        }

        void UpdateText(string text)
        {
            var m = new MethodInvoker(delegate
            {
                var n = Convert.ToDouble(text);
                n = n / 10;
                base.Text = n.ToString("0.0") + Unit;
            });

            Invoke(m);
        }

        private void PhShow_Enter(object sender, EventArgs e)
        {
            try
            {
                var form = FindForm();
                var arrayOfControls = form.Controls.Find("unfocus", true);
                var tb = (TextBox)arrayOfControls[0];
                tb.Focus();
            }
            catch
            { }
        }
    }
}
