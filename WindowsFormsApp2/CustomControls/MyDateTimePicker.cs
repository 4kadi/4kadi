using System;
using System.Windows.Forms;

namespace KontrolaKadi
{
    public class MyDateTimePicker : SPanel
    {
        private TextBox textBox;
        private Button btnIncrement;
        private Button btnDecrement;

        public DateTime Value;

        MyTimer TimerCnt = new MyTimer();
        bool directionIncrement = true;
        int pressedDuration = 0;

        public MyDateTimePicker()
        {
            InitializeComponents();
            UpdateTextBox();
        }

        private void InitializeComponents()
        {
            Height = 48;
            Width = 237;
            

            btnDecrement = new Button();
            btnDecrement.Text = "▼";
            btnDecrement.Left = 0;
            btnDecrement.Top = 2;
            btnDecrement.Width = 50;
            btnDecrement.Height = 45;
            btnDecrement.MouseDown += BtnDecrement_MouseDown;
            btnDecrement.MouseUp += BtnDecrement_MouseUp;
            btnDecrement.MouseLeave += BtnDecrement_MouseLeave;

            textBox = new TextBox();
            textBox.Top = 2;
            textBox.Left = btnDecrement.Right + 5;
            textBox.Width = 125;
            textBox.Height = btnDecrement.Height;
            textBox.Font = new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif, 25, System.Drawing.FontStyle.Bold);
            textBox.KeyPress += TextBox_KeyPress;
            textBox.MouseDown += TextBox_MouseDown;

            btnIncrement = new Button();
            btnIncrement.Height = btnDecrement.Height;
            btnIncrement.Width = btnDecrement.Width;
            btnIncrement.Top = btnDecrement.Top;
            btnIncrement.Text = "▲";
            btnIncrement.Left = textBox.Right + 5;
            btnIncrement.MouseDown += BtnIncrement_MouseDown;
            btnIncrement.MouseUp += BtnIncrement_MouseUp;
            btnIncrement.MouseLeave += BtnIncrement_MouseLeave;

            Controls.Add(textBox);
            Controls.Add(btnIncrement);
            Controls.Add(btnDecrement);

            textBox.VisibleChanged += TextBox_VisibleChanged;            

        }

        private void TextBox_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                textBox.Select(2, 2);                
                UpdateTextBox();
            }            
        }

        void startTimerCnt()
        {
            TimerCnt = new MyTimer()
            {
                AutoReset = true,
                Interval = 150                
            };

            TimerCnt.Elapsed += TimerCnt_Elapsed;
            TimerCnt.Start();
        }

        private void TimerCnt_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (directionIncrement)
            {
                var m = new MethodInvoker(Increment);
                Parent.Invoke(m);
            }
            else
            {
                var m = new MethodInvoker(Decrement);
                Parent.Invoke(m);
            }
            pressedDuration++;
        }

        void stopTimer()
        {
            pressedDuration = 0;
            TimerCnt.Stop();
        }

        private void BtnIncrement_MouseDown(object sender, MouseEventArgs e)
        {
            Increment();
            startTimerCnt();
            directionIncrement = true;
        }
        private void BtnIncrement_MouseUp(object sender, MouseEventArgs e)
        {
            stopTimer();
        }
        private void BtnIncrement_MouseLeave(object sender, EventArgs e)
        {
            stopTimer();
        }

        private void BtnDecrement_MouseDown(object sender, MouseEventArgs e)
        {
            Decrement();
            startTimerCnt();
            directionIncrement = false;
        }
        private void BtnDecrement_MouseUp(object sender, MouseEventArgs e)
        {
            stopTimer();
        }
        private void BtnDecrement_MouseLeave(object sender, EventArgs e)
        {
            stopTimer();
        }

        private void Increment() // protect maximums
        {
            try
            {
                if (Value == null)
                {
                    return;
                }
                if (Value >= DateTime.MinValue + TimeSpan.FromHours(9))
                {
                    return;
                }

                if (textBox.SelectionStart == 0)
                {
                    // hours selected
                    Value = Value.AddHours(1);
                }
                else if (textBox.SelectionStart == 2)
                {
                    // minutes selected
                    if (pressedDuration > 5)
                    {
                        Value = Value.AddMinutes(5);
                    }
                    else
                    {
                        Value = Value.AddMinutes(1);
                    }
                    
                }
                else if (textBox.SelectionStart == 5)
                {
                    // seconds selected
                    Value = Value.AddSeconds(10);
                }
                else
                {
                    return;
                }

                UpdateTextBox();
            }
            catch (Exception)
            {
                throw;
            }
           
        }

        private void Decrement()
        {
            if (Value == null)
            {
                return;
            }
            if (Value <= DateTime.MinValue)
            {
                return;
            }
            if (textBox.SelectionStart == 0)
            {
                // hours selected
                Value = Value.AddHours(-1);
            }
            else if (textBox.SelectionStart == 2)
            {
                try
                {
                    // minutes selected
                    if (pressedDuration > 5)
                    {
                        Value = Value.AddMinutes(-5);
                    }
                    else
                    {
                        Value = Value.AddMinutes(-1);
                    }
                }
                catch 
                {
                    Value = DateTime.MinValue;
                }
               
            }
            else if (textBox.SelectionStart == 5)
            {
                // seconds selected
                Value = Value.AddSeconds(-10);
            }
            else
            {
                return;
            }
            UpdateTextBox();
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
            return;
        }

        private void TextBox_MouseDown(object sender, MouseEventArgs e)
        {
            int charIndex = textBox.GetCharIndexFromPosition(e.Location);

            if (charIndex <= 2)
            {
                textBox.Select(0, 1);
            }
            else if (charIndex <= 4)
            {
                textBox.Select(2, 2);
            }
            else if (charIndex <= 7)
            {
                textBox.Select(5, 2);
            }
        }

        public void UpdateTextBox()
        {
            var ss = textBox.SelectionStart;
            var sl = textBox.SelectionLength;
            textBox.Text = Value.ToString("H:mm:ss");
            textBox.Select(ss, sl);
        }
    }
}