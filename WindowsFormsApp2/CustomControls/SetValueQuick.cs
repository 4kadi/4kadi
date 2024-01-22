using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KontrolaKadi
{

    public class SetValueQuick : SPanel
    {

        bool initialized = false;
        DateTime mouseClickedAt;
        TimeSpan longClickDuration = TimeSpan.FromMilliseconds(500);
        System.Timers.Timer mouseDownTimer;

        Color defaultColor;

        PlcVars.Word _Value;
        public PlcVars.Word Value
        {
            get { return _Value; }
            set
            {
                _Value = value;
                InitializeIfPossible();
            }
        }

        public override string Text { get { return lblDescript.Text; } set { lblDescript.Text = value; } }

        Button btnPlus, btnMinus;
        TextBox tb;
        CenteredLabel lblDescript;

        const int size = 30;

        private string unit = "";

        public string Unit
        {
            get { return unit; }
            protected set 
            { 
                unit = value;
                _TemperaturaNastavljena_ValueChanged(this, EventArgs.Empty);
            }
        }


        private int _LowLimit = short.MinValue;

        public int LowLimit
        {
            get { return _LowLimit; }
            set { _LowLimit = value; }
        }

        private int _HiLimit = short.MaxValue;

        public int HiLimit
        {
            get { return _HiLimit; }
            set { _HiLimit = value; }
        }


        public SetValueQuick()
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
                ReadOnly = true,
                Top = btnMinus.Top,
                Left = btnMinus.Right + 5,
                Width = 70,
                Height = size,
                TextAlign = HorizontalAlignment.Center
            };
            Controls.Add(tb);
            tb.Enter += Tb_Enter;

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
                Width = this.Width,
                Left = 0,
                Top = tb.Top - 30,
                Text = "Temperatura",
                Font = new Font("arial", 10),
                Height = 30,
                Padding = new Padding(0,10,10,0)                       
            };   
            
            Controls.Add(lblDescript);

            Width = btnPlus.Right + 10;
            Height = btnMinus.Bottom + 10;

            btnMinus.MouseDown += BtnMinus_MouseDown;
            btnMinus.MouseUp += BtnMinus_MouseUp;

            btnPlus.MouseDown += BtnPlus_MouseDown;
            btnPlus.MouseUp += BtnPlus_MouseUp;
        }

        private void Tb_Enter(object sender, EventArgs e)
        {
            Helper.Unfocus(sender);
        }

        private void BtnPlus_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDownTimer.Stop();

            var now = DateTime.Now;

            if ((now - mouseClickedAt) < longClickDuration)
            {
                IncreaseValue();
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
                DecreaseValue();
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
                DecreaseValue();
            }
            else if (mode == "increment")
            {
                IncreaseValue();
            }
            mouseDownTimer.Interval = 100;            
        }

        void DecreaseValue()
        {
            if (Value.Value_short > LowLimit)
            {
                Value.Value_short -= 10;            
            }
            
        }
        void IncreaseValue()
        {
            if (Value.Value_short < HiLimit)
            {
                Value.Value_short += 10;
            }             
        }

        void InitializeIfPossible()
        {
            if (_Value != null)
            {
                _Value.ValueChanged += _TemperaturaNastavljena_ValueChanged;
                initialized = true;
            }
        }

        private void _TemperaturaNastavljena_ValueChanged(object sender, EventArgs e)
        {
            if (!initialized)
            {
                return;
            }

            var m = new MethodInvoker(delegate
            {
                double num = (double)Value.Value_short / 10;
                var txt = num.ToString("0.#") + Unit;
                tb.Text = txt;
            });
            Invoke(m);

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

    public class SetTempQuick_5_95 : SetValueQuick
    {
        public SetTempQuick_5_95()
        {
            HiLimit = 950;
            LowLimit = 50;
            Unit = "°C";
        }
    }

    public class SetPhQuick : SetValueQuick
    {
        public SetPhQuick()
        {
            HiLimit = 70;
            LowLimit = 0;
            Unit = "";
        }
    }
}
