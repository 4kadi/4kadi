using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KontrolaKadi
{
    class SetTempQuick : SPanel
    {
        PlcVars.Word _TemperaturaNastavljena;

        DateTime mouseClickedAt;
        TimeSpan longClickDuration = TimeSpan.FromMilliseconds(500);
        System.Timers.Timer mouseDownTimer;

        Color defaultColor;

        public PlcVars.Word TemperaturaNastavljena
        {
            get { return _TemperaturaNastavljena; }
            set
            {
                _TemperaturaNastavljena = value;
                InitializeIfPossible();
            }
        }

        public override string Text { get { return lblDescript.Text; } set { lblDescript.Text = value; } }

        Button btnPlus, btnMinus;
        TextBox tb;
        CenteredLabel lblDescript;

        const int size = 30;

        public string Unit = "°C";

        public short Value {
            get { return GetValue(); }
            set { setValue(value); } 
        }

        public SetTempQuick()
        {
            btnMinus = new Button()
            {
                Text = "-",
                Top = 20,
                Left = 10,
                Height = size,
                Width = (int) (size*1.5)
            };
            Controls.Add(btnMinus);

            tb = new TextBox()
            {
                Enabled = false,
                Top = btnMinus.Top,
                Left = btnMinus.Right + 5,
                Width = 70,
                Height = size
            };
            Controls.Add(tb);

            defaultColor = tb.BackColor;

            btnPlus = new Button()
            {
                Text = "+",
                Top = btnMinus.Top,
                Left = tb.Right + 5,
                Height = size,
                Width = (int) (size * 1.5)
            };
            Controls.Add(btnPlus);

            lblDescript = new CenteredLabel()
            {
                Dock = DockStyle.None,
                Width = btnPlus.Right - btnMinus.Left,
                Left = btnMinus.Left,
                Top = tb.Top - 25,      
                Text = "Temperatura",
                Font = new System.Drawing.Font("arial", 10)
            };
            Controls.Add(lblDescript);

            Width = btnPlus.Right + 10;
            Height = btnMinus.Bottom + 10;

            btnMinus.MouseDown += BtnMinus_MouseDown;
            btnMinus.MouseUp += BtnMinus_MouseUp;

            btnPlus.MouseDown += BtnPlus_MouseDown;
            btnPlus.MouseUp += BtnPlus_MouseUp;
        }

        private void BtnPlus_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDownTimer.Stop();

            var now = DateTime.Now;

            if ((now - mouseClickedAt) < longClickDuration)
            {
                IncreaseTemperature();
            }
        }

        private void BtnPlus_MouseDown(object sender, MouseEventArgs e)
        {
            mouseClickedAt = DateTime.Now;
            mouseDownTimer = new System.Timers.Timer(longClickDuration.TotalMilliseconds);
            mouseDownTimer.Elapsed += (snd, ea) => MouseDownTimer_Elapsed("increment", null);
            mouseDownTimer.Start();
        }
                
        private void BtnMinus_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDownTimer.Stop();

            var now = DateTime.Now;

            if ((now - mouseClickedAt) < longClickDuration)
            {
                DecreaseTemperature();
            }
        }

        private void BtnMinus_MouseDown(object sender, MouseEventArgs e)
        {
            mouseClickedAt = DateTime.Now;
            mouseDownTimer = new System.Timers.Timer(longClickDuration.TotalMilliseconds);
            mouseDownTimer.Elapsed += (snd, ea)=> MouseDownTimer_Elapsed("decrement", null);
            mouseDownTimer.Start();
        }

        private void MouseDownTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            var mode = (string)sender;
            if (mode == "decrement")
            {
                DecreaseTemperature();
            }
            else if (mode == "increment")
            {
                IncreaseTemperature();
            }
            mouseDownTimer.Interval = 100;            
        }

        void DecreaseTemperature()
        {            
            TemperaturaNastavljena.Value_short -= 1;
        }
        void IncreaseTemperature()
        {
            TemperaturaNastavljena.Value_short += 1;
        }

        void setValue(short value)
        {
            if (tb.InvokeRequired)
            {
                var m = new MethodInvoker(delegate 
                {
                    tb.Text = value + Unit;
                });

                Invoke(m);
                return;
            }

            tb.Text = value + Unit;

        }

        short GetValue()
        {
            try
            {
                return Convert.ToInt16(tb.Text.Replace(Unit, ""));
            }
            catch (Exception)
            {
                return 0;
            }
        }

        void InitializeIfPossible()
        {
            if (_TemperaturaNastavljena != null)
            {
                _TemperaturaNastavljena.ValueChanged += _TemperaturaNastavljena_ValueChanged;
            }
        }

        private void _TemperaturaNastavljena_ValueChanged(object sender, EventArgs e)
        {
            Console.WriteLine();

            if (tb.InvokeRequired)
            {
                var m = new MethodInvoker(delegate 
                {
                    tb.Text = TemperaturaNastavljena.Value_short.ToString("0");
                });
                Invoke(m);
                Invalidate();
                return;
            }

            tb.Text = TemperaturaNastavljena.Value_short.ToString("0");
            Invalidate();
        }

        public void Highlight()
        {
            var m = new MethodInvoker(delegate 
            {
                tb.BackColor = Color.LightGreen;
            });
            Invoke(m);
        }

        public void RemoveHighlight()
        {
            var m = new MethodInvoker(delegate
            {
                tb.BackColor = defaultColor;
            });
            Invoke(m);
            
        }
    }
}
