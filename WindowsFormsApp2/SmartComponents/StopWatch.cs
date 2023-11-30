using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using System.ComponentModel;

namespace KontrolaKadi
{
    public class StopWatch : Panel
    {
        bool designMode;

        Color AlertBackColor = Color.Red;
        Color ActiveBackColor = Color.Green;
        Color PausedBackColor = Color.Yellow;
        Color NormalColor;
        public bool AlertState = false;

        private BtnTimeset btnOpomnik;

        private BtnTimeset btnPause;

        PlcVars.AlarmBit buzzPulseUrgent;  // TODO SET FROM OUTSIDE
        PlcVars.AlarmBit buzzPulseNormal;  // TODO SET FROM OUTSIDE
        PlcVars.AlarmBit buzzPulseStop;  // TODO SET FROM OUTSIDE

        PlcVars.Bit setReminderPlus;    // TODO SET FROM OUTSIDE
        PlcVars.Bit setReminderMinus;   // TODO SET FROM OUTSIDE

        PlcVars.Bit setPausePlus;   // TODO SET FROM OUTSIDE
        PlcVars.Bit setPauseMinus;  // TODO SET FROM OUTSIDE

        PlcVars.Word CurrentTime;  // TODO SET FROM OUTSIDE
        PlcVars.Word ReminderTime;  // TODO SET FROM OUTSIDE
        PlcVars.Word PauseTime;   // TODO SET FROM OUTSIDE

        private GroupBox groupBox2;
        private Label lblReminder;
        private Label label11;
        private Label label12;
        private Label label4;
        private Label label3;
        private Label label2;

        const string dflttime = "0:00:00";
        const string dflttimeFormat = @"h\:mm\:ss";
        const string dflttimeFormat2 = @"HH\:mm\:ss";

        const string OKstring = "OK";
        const string EDITstring = "Edit";      
       
        private int panelMaxWidth = 135;
                
        MyTimer updater = new MyTimer();

        public StopWatch()
        {
            designMode = (LicenseManager.UsageMode == LicenseUsageMode.Designtime);
            NormalColor = BackColor;

            this.groupBox2 = new GroupBox();
            this.label4 = new Label();
            this.label3 = new Label();
            this.label2 = new Label();
            this.lblReminder = new Label();           
            this.label11 = new Label();
            this.label12 = new Label();
            this.btnOpomnik = new BtnTimeset();
            this.btnPause = new BtnTimeset();          

            // 
            // panel1
            // 
            Controls.Add(this.groupBox2);
            groupBox2.Controls.Add(this.btnOpomnik);
            groupBox2.Controls.Add(this.btnPause);
            groupBox2.Controls.Add(this.btnPause);
            Size = new Size(panelMaxWidth, 205);
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(this.label4);
            groupBox2.Controls.Add(this.label3);
            groupBox2.Controls.Add(this.label2);
            groupBox2.Controls.Add(this.lblReminder);
            groupBox2.Controls.Add(this.label11);
            groupBox2.Controls.Add(this.label12);
            groupBox2.Location = new Point(3, 3);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(panelMaxWidth - 6, 200);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new Point(80, 10);
            this.label4.Name = "label4";
            this.label4.Size = new Size(24, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "sec";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new Point(40, 10);
            this.label3.Name = "label3";
            this.label3.Size = new Size(23, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "min";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new Point(12, 10);
            this.label2.Name = "label2";
            this.label2.Size = new Size(13, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "h";
            //  
            // label1 - stopwatch time
            // 
            this.lblReminder.AutoSize = true;
            this.lblReminder.Font = new Font("Impact", 24F);
            this.lblReminder.Location = new Point(5, 20);
            this.lblReminder.Name = "label1";
            this.lblReminder.Size = new Size(panelMaxWidth - 20, 48);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new Point(25, 65);
            this.label11.Name = "label11";
            this.label11.Size = new Size(24, 13);
            this.label11.TabIndex = 5;
            this.label11.Text = "Nastavljen čas:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new Point(20, 125);
            this.label12.Name = "label12";
            this.label12.Size = new Size(20, 33);
            this.label12.TabIndex = 5;
            this.label12.Text = "Dovoljena pavza:";
            // 
            // btnOpomnik
            // 
            this.btnOpomnik.Location = new Point(groupBox2.Left +5, 83);
            this.btnOpomnik.Name = "btnOpomnik";
            this.btnOpomnik.Size = new Size(groupBox2.Width -16, 30);
            this.btnOpomnik.TabIndex = 1;
            this.btnOpomnik.UseVisualStyleBackColor = true;
            btnOpomnik.Enabled = true;
            btnOpomnik.Click += BtnOpomnik_Click;
            btnOpomnik.StopWatchReference = this;
            // 
            // btnPause
            // 
            this.btnPause.Location = new Point(btnOpomnik.Location.X, btnOpomnik.Bottom + 30);
            this.btnPause.Name = "btnOpomnik";
            this.btnPause.Size = btnOpomnik.Size;
            this.btnPause.TabIndex = 1;
            this.btnPause.UseVisualStyleBackColor = true;
            btnPause.Enabled = true;
            btnPause.Click += btnPause_Click;
            btnPause.StopWatchReference = this;

            if (designMode)
            {
                return;
            }

            updater.Interval = 100;
            updater.AutoReset = true;
            updater.Elapsed += Updater_Elapsed;
            updater.Start();

        }

        private void Updater_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (CurrentTime != null)
            {
                btnOpomnik.dateTimePicker.Text = ConvertPlcVarToDisplayTime(CurrentTime);
            }
            if (ReminderTime != null)
            {
                lblReminder.Text = ConvertPlcVarToDisplayTime(CurrentTime);
            }
            if (PauseTime != null)
            {
                btnPause.Text = ConvertPlcVarToDisplayTime(PauseTime);
            }
        }

        public static string ConvertPlcVarToDisplayTime(PlcVars.Word plcTime)
        {
            return "0:00:00"; // TODO convert logic
        }
           
       
        public void SetWidth(int width)
        {
            if (width <= panelMaxWidth)
            {
                this.Width = width;
            }
            else
            {
                this.Width = panelMaxWidth;
            }
        }
        
        private void BtnOpomnik_Click(object sender, EventArgs e)
        {
            btnOpomnik.ShowForm();
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            btnPause.ShowForm();
        }
               
        private void ResumeTiming(string oldSarzaID, string oldTime)
        {            
            BackColor = ActiveBackColor;
        }


        private void StartNewTiming()
        { 
            // Todo send to plc reset command
        }

        private void StopTiming()
        {            
            btnOpomnik.Enabled = true;            
            BackColor = NormalColor;        
        }

        private void PauseTiming()
        {        
            BackColor = PausedBackColor;            
        }

        public string UnknownID()
        {
            return "UNKNOWN ID!"; // TODO - avtomatsko generiranje imen za saržo
        }

        public class BtnTimeset : Button
        {
            public Form OpomnikSetFormSetForm = new Form();
            public MyDateTimePicker dateTimePicker = new MyDateTimePicker();
            Label label = new Label();
            Button OKbtn = new Button();
            public StopWatch StopWatchReference;
            public string Text1
            {
                get { return label.Text; }
                set { label.Text = value; }
            }

            private DateTime val;

            public DateTime Value
            {
                get 
                { 
                    return val;                
                }
                set 
                { 
                    val = value;                
                }
            }

            public BtnTimeset()
            {
                OpomnikSetFormSetForm.TopMost = true;
                OpomnikSetFormSetForm.ControlBox = false;
                dateTimePicker.Top = 40;
                dateTimePicker.Left = 5;
               
                label.Top = 10;
                label.Left = 10;
                label.Height = 20;
                label.Width = 220;
                label.Font = new Font(new FontFamily(System.Drawing.Text.GenericFontFamilies.SansSerif), 10);

                OKbtn.Top = 100;
                OKbtn.Width = 100;
                OKbtn.Left = ((OpomnikSetFormSetForm.Width - OKbtn.Width) / 2) - 20;
                OKbtn.Height = 50;
                OKbtn.Text = "OK";
                OKbtn.Font = label.Font;

                OpomnikSetFormSetForm.Width = 280;
                OpomnikSetFormSetForm.Height = OKbtn.Bottom + 40;

                OpomnikSetFormSetForm.Controls.Add(OKbtn);
                OpomnikSetFormSetForm.Controls.Add(label);
                OpomnikSetFormSetForm.Controls.Add(dateTimePicker);

                OpomnikSetFormSetForm.LostFocus += SetTemps_form_LostFocus;
                OKbtn.Click += OKbtn_Click;

            }

            private void OKbtn_Click(object sender, EventArgs e)
            {
                // TODO send to PLC
                OpomnikSetFormSetForm.Hide();
            }

            private void SetTemps_form_LostFocus(object sender, EventArgs e)
            {
                OpomnikSetFormSetForm.Focus();
            }

            public void ShowForm()
            {
                OpomnikSetFormSetForm.Show();
            }
        }
      
    }
}
