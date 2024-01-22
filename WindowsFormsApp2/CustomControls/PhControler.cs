using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KontrolaKadi
{
    public class PhControler : SGroupBox
    {
        bool initialized = false;

        

        TbPhShow tbPh1;
        CenteredLabel trenurno;

        SetPhQuick PhSp_Control;

        private PlcVars.Word _PhSp;

        public PlcVars.Word PhSetPoint
        {
            get { return _PhSp; }
            set 
            { 
                _PhSp = value;
                Any_ValueChanged(null, EventArgs.Empty);
            }
        }


        private PlcVars.Word _PhReading;
        public PlcVars.Word PhReading
        {
            get { return _PhReading; }
            set
            {
                _PhReading = value;
                Any_ValueChanged(null, EventArgs.Empty);
            }
        }

        private PlcVars.Bit _SenErr;

        public PlcVars.Bit SenErr
        {
            get { return _SenErr; }
            set
            {
                _SenErr = value;
                Any_ValueChanged(null, EventArgs.Empty);
            }
        }
               
        Color originalColor;

        public PhControler()
        {
            Font = new Font("arial", 15, FontStyle.Bold);

            tbPh1 = new TbPhShow()
            {
                Top = 45,
                Left = 15
            };
            Controls.Add(tbPh1);
            originalColor = tbPh1.BackColor;

            trenurno = new CenteredLabel()
            {
                Font = new Font("arial", 10),
                Top = tbPh1.Top - 25,
                Left = tbPh1.Left,
                Width = tbPh1.Width,
                Text = "Trenutno:"
            };
            Controls.Add(trenurno);

            PhSp_Control = new SetPhQuick()
            {
                Top = tbPh1.Top-20,
                Left = trenurno.Right + 10,
                Text = "Željena vrednost:",
                
            };
            Controls.Add(PhSp_Control);
        
            
            Height = PhSp_Control.Bottom + 15;
            Width = PhSp_Control.Right + 15;
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

            try
            {
                // Ph reading
                tbPh1.Text = PhReading.Value_short.ToString("0.0");

                // Senzor error
                if (SenErr.Value_bool)
                {
                    tbPh1.BackColor = Color.Red;
                }
                else
                {
                    tbPh1.BackColor = originalColor;
                }
                
            }
            catch (Exception ex)
            {
                // TODO log
            }

        }

        void InitializeIfPossible()
        {
            if (initialized)
            {
                return;
            }

            if (PhReading != null && SenErr != null && PhSetPoint != null)
            {
                PhSp_Control.Value = PhSetPoint; // feed reference to subclass

                PhReading.ValueChanged += Any_ValueChanged;
                PhSetPoint.ValueChanged += Any_ValueChanged;
                SenErr.ValueChanged += SenErr_ValueChanged;
                initialized = true;
            }
        }

        private void PhSetpoint_ValueChanged(object sender, EventArgs e)
        {
            Any_ValueChanged();
        }

        private void SenErr_ValueChanged(object sender, EventArgs e)
        {
            Any_ValueChanged();
        }
    }

    class TbPhShow : TextBox
    {
        string Unit = "";
        override public string Text
        {
            get 
            {
                if (Unit == "")
                {
                    return base.Text;
                }
                return base.Text.Replace(Unit, "");
            }
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
                double n = 0;
                string txt = "empty";

                try
                {
                    n = Convert.ToDouble(text);
                    n = n / 10;
                    txt = n.ToString("0.0");
                    txt += Unit;                    
                    base.Text = txt;
                }
                catch (Exception ex)
                {
                    string log = ex.Message + ". txt was: " + txt + ". n was: " + n.ToString();
                }
               
            });

            Invoke(m);
        }

        private void PhShow_Enter(object sender, EventArgs e)
        {
            Helper.Unfocus(sender);
        }
    }
}
