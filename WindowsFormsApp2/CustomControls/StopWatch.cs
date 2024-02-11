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
    public class StopWatch : SGroupBox
    {
        bool designMode;
        bool initialized = false;

        Color AlertBackColor = Color.Red;
        Color ActiveBackColor = Color.Green;
        Color PausedBackColor = Color.Yellow;
        Color NormalColor;
        public bool AlertState = false;

        private BtnTimeset btnOpomnik;
        private BtnTimeset btnPause;

        PlcVars.DWord _CurrentTime;
        PlcVars.Word _TimeLeft;
        PlcVars.DWord _ReminderTime;
        PlcVars.DWord _PauseTime;
        PlcVars.Word _PauseLeft;
        PlcVars.Bit _Finished;
        PlcVars.Bit _Paused;
        PlcVars.Bit _InProgress;
        PlcVars.Bit _Prisotnost;
        PlcVars.Word _Autostart;
        PlcVars.Bit _BtnReset;
        PlcVars.Bit _BtnStart;
        PlcVars.Bit _BtnStop;

        public PlcVars.DWord CurrentTime 
        { 
            get { return _CurrentTime; } 
            set { _CurrentTime = value; InitializeIfPossible(); } 
        }

        public PlcVars.Word TimeLeft
        {
            get { return _TimeLeft; }
            set { _TimeLeft = value; InitializeIfPossible(); }
        }

        public PlcVars.DWord ReminderTime
        {
            get { return _ReminderTime; }
            set { _ReminderTime = value; InitializeIfPossible(); }
        }

        public PlcVars.DWord PauseTime
        {
            get { return _PauseTime; }
            set { _PauseTime = value; InitializeIfPossible(); }
        }

        public PlcVars.Word PauseLeft
        {
            get { return _PauseLeft; }
            set { _PauseLeft = value; InitializeIfPossible(); }
        }

        public PlcVars.Bit Finished
        {
            get { return _Finished; }
            set { _Finished = value; InitializeIfPossible(); }
        }

        public PlcVars.Bit Paused
        {
            get { return _Paused; }
            set { _Paused = value; InitializeIfPossible(); }
        }

        public PlcVars.Bit InProgress
        {
            get { return _InProgress; }
            set { _InProgress = value; InitializeIfPossible(); }
        }

        public PlcVars.Bit Prisotnost
        {
            get { return _Prisotnost; }
            set { _Prisotnost = value; InitializeIfPossible(); }
        }

        public PlcVars.Word UpostevajPrisotnost
        {
            get { return _Autostart; }
            set { _Autostart = value; InitializeIfPossible(); }
        }

        public PlcVars.Bit BtnReset
        {
            get { return _BtnReset; }
            set { _BtnReset = value; }
        }

        public PlcVars.Bit BtnStart
        {
            get { return _BtnStart; }
            set { _BtnStart = value; }
        }

        public PlcVars.Bit BtnStop
        {
            get { return _BtnStop; }
            set { _BtnStop = value; }
        }

        private GroupBox groupBox2;
        private Label stopwatchTime;
        private Label label11;
        private Label label12;
        private Label label4;
        private Label label3;
        private Label label2;

        private ManButton ManStart, ManReset;

        private CheckBox chkUpostevajPrisotnost;

        const string dflttimeFormat = @"HH\:mm\:ss";
        const string dflttimeFormat2 = @"h\:mm\:ss";        

        const string OKstring = "OK";
        const string EDITstring = "Edit";      
       
        private int panelMaxWidth = 145;

        public StopWatch()
        {
            designMode = (LicenseManager.UsageMode == LicenseUsageMode.Designtime);
            NormalColor = BackColor;

            this.groupBox2 = new GroupBox();
            this.label4 = new Label();
            this.label3 = new Label();
            this.label2 = new Label();
            this.stopwatchTime = new Label();
            this.label11 = new Label();
            this.label12 = new Label();
            this.btnOpomnik = new BtnTimeset();
            this.btnPause = new BtnTimeset();
            chkUpostevajPrisotnost = new CheckBox();
            ManStart = new ManButton();
            ManReset = new ManButton();

            // 
            // panel1
            // 
            Controls.Add(this.groupBox2);
            groupBox2.Controls.Add(this.btnOpomnik);
            groupBox2.Controls.Add(this.btnPause);
            groupBox2.Controls.Add(this.btnPause);
            Size = new Size(panelMaxWidth, 255);
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(this.label4);
            groupBox2.Controls.Add(this.label3);
            groupBox2.Controls.Add(this.label2);
            groupBox2.Controls.Add(this.stopwatchTime);
            groupBox2.Controls.Add(this.label11);
            groupBox2.Controls.Add(this.label12);
            groupBox2.Controls.Add(chkUpostevajPrisotnost);
            groupBox2.Location = new Point(3, 3);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(panelMaxWidth - 6, Size.Height - 35);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Top = 7;
            groupBox2.Left = 5;


            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new Point(90, 10);
            this.label4.Name = "label4";
            this.label4.Size = new Size(24, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "sec";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new Point(50, 10);
            this.label3.Name = "label3";
            this.label3.Size = new Size(23, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "min";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new Point(22, 10);
            this.label2.Name = "label2";
            this.label2.Size = new Size(13, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "h";
            //  
            // label1 - stopwatch time
            // 
            this.stopwatchTime.AutoSize = true;
            this.stopwatchTime.Font = new Font("Impact", 24F);
            this.stopwatchTime.Location = new Point(15, 20);
            this.stopwatchTime.Name = "label1";
            this.stopwatchTime.Size = new Size(panelMaxWidth - 20, 48);
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
            this.btnOpomnik.Location = new Point(groupBox2.Left + 5, 83);
            this.btnOpomnik.Name = "btnOpomnik";
            this.btnOpomnik.Size = new Size(groupBox2.Width - 16, 30);
            this.btnOpomnik.TabIndex = 1;
            this.btnOpomnik.UseVisualStyleBackColor = true;
            btnOpomnik.Enabled = true;
            btnOpomnik.Click += BtnOpomnik_Click;
            btnOpomnik.OKbtn.Click += btnOpomnikOK_Click;
            btnOpomnik.StopWatchReference = this;
            btnOpomnik.Font = new Font("arial", 13, FontStyle.Bold);
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
            btnPause.OKbtn.Click += btnPauseOK_Click;
            btnPause.StopWatchReference = this;
            btnPause.Font = new Font("arial", 13, FontStyle.Bold);
            //
            // chkAutostart
            //
            chkUpostevajPrisotnost.Text = "Upoštevaj prisotnost";
            chkUpostevajPrisotnost.Click += ChkUpostevajPrisotnost_Click;
            chkUpostevajPrisotnost.Top = btnPause.Bottom + 10;
            chkUpostevajPrisotnost.Left = 10;
            chkUpostevajPrisotnost.Width = 125;
            //
            // ManStart
            //
            ManStart.Text = "...";
            ManStart.Top = groupBox2.Bottom + 5;
            ManStart.Left = groupBox2.Left + 2;
            ManStart.Width = groupBox2.Width / 2 - 4;
            Controls.Add(ManStart);
            //
            // ManReset
            //
            ManReset.Text = "...";
            ManReset.Top = ManStart.Top;
            ManReset.Left = ManStart.Right + 5;
            ManReset.Width = ManStart.Width;
            Controls.Add(ManReset);

            Height = ManStart.Bottom + 12;
            Width = groupBox2.Width + groupBox2.Left * 2 +2;

            TextChanged += StopWatch_TextChanged;
            HandleCreated += StopWatch_HandleCreated;            
        }

        private void StopWatch_TextChanged(object sender, EventArgs e)
        {
            Text = "";
        }

        private void StopWatch_HandleCreated(object sender, EventArgs e)
        {      
            Text = "";
           
            if (designMode)
            {
                return;
            }

            ManStart.Reset_Clicked += ManStart_Reset_Clicked;
            ManStart.Start_Clicked += ManStart_Start_Clicked;
            ManStart.Stop_Clicked += ManStart_Stop_Clicked;

            ManReset.Reset_Clicked += ManReset_Reset_Clicked;          
            
        }

        private void ManReset_Reset_Clicked(object sender, EventArgs e)
        {
            if (initialized)
            {
                BtnReset.SendPulse();
            }
        }

        private void ManStart_Stop_Clicked(object sender, EventArgs e)
        {
            if (BtnStop != null)
            {
                BtnStop.SendPulse();                
            }
            else
            {
                throw new Exception("You should set all of the PlcVars in the class StopWatch to be able to use this functionality. BtnStop property was null.");
            }
            
        }

        private void ManStart_Start_Clicked(object sender, EventArgs e)
        {
            if (BtnStart != null)
            {
                BtnStart.SendPulse();
            }
            else
            {
                throw new Exception("You should set all of the PlcVars in the class StopWatch to be able to use this functionality. BtnStart property was null.");
            }
        }

        private void ManStart_Reset_Clicked(object sender, EventArgs e)
        {
            if (BtnReset != null)
            {
                BtnReset.SendPulse();
            }
            else
            {
                throw new Exception("You should set all of the PlcVars in the class StopWatch to be able to use this functionality. BtnReset property was null.");
            }
        }

        void InitializeIfPossible()
        {
            if (CurrentTime != null && TimeLeft != null && PauseLeft != null &&
                ReminderTime != null && PauseTime != null && 
                Finished != null && Paused != null && InProgress != null &&
                Prisotnost != null && UpostevajPrisotnost != null)
            {
                CurrentTime.ValueChanged += Updater_Elapsed;
                TimeLeft.ValueChanged += Updater_Elapsed;
                PauseLeft.ValueChanged += Updater_Elapsed;
                ReminderTime.ValueChanged += Updater_Elapsed;
                PauseTime.ValueChanged += Updater_Elapsed;
                Finished.ValueChanged += Updater_Elapsed;
                Paused.ValueChanged += Updater_Elapsed;
                InProgress.ValueChanged += Updater_Elapsed;
                Prisotnost.ValueChanged += Updater_Elapsed;
                Prisotnost.ValueChanged += Prisotnost_ValueChanged;
                UpostevajPrisotnost.ValueChanged += Updater_Elapsed;
                initialized = true;
                
                Updater_Elapsed(null, null);
                
            }            
        }

        private void Prisotnost_ValueChanged(object sender, EventArgs e)
        {
            if (Prisotnost.Value_bool)
            {
                // value changed to true
                if (UpostevajPrisotnost.Value_short > 0)
                {

                }
            }
            else
            {
                // value changed to false
                if (UpostevajPrisotnost.Value_short == 0)
                {                    
                    // če je obkljukano, da se upošteva prisotnost sarže
                    if (isFinished())
                    {
                        // ko je sarža izvzeta resetiraj štoparico (če je le ta potekla)
                       
                    }
                }
            }
        }

        void ManButtonFunctionDecide()
        {
            if (isFinished())
            {
                ManStart.CurrentFunction = ManButton.Functions.Invisible;
                ManReset.CurrentFunction = ManButton.Functions.Reset;
            }
            else if (isPaused())
            {
                ManStart.CurrentFunction = ManButton.Functions.Start;
                ManReset.CurrentFunction = ManButton.Functions.Reset;
            }
            else if (isReadyToStart())
            {
                ManStart.CurrentFunction = ManButton.Functions.Start;
                ManReset.CurrentFunction = ManButton.Functions.Reset;
            }
            else if (isRunning())
            {
                ManStart.CurrentFunction = ManButton.Functions.Stop;
                ManReset.CurrentFunction = ManButton.Functions.Reset;
            }
        }

        private void ChkUpostevajPrisotnost_Click(object sender, EventArgs e)
        {
            if (chkUpostevajPrisotnost.Checked)
            {
                UpostevajPrisotnost.Value_short = 1;
            }
            else
            {
                UpostevajPrisotnost.Value_short = 0;
            }
        }

        private void btnOpomnikOK_Click(object sender, EventArgs e)
        {
            var timeSpan = (btnOpomnik.dateTimePicker.Value - DateTime.MinValue).TotalSeconds;
            ReminderTime.Value_int = (int)timeSpan;
            btnOpomnik.OpomnikSetFormSetForm.Hide();
        }

        private void btnPauseOK_Click(object sender, EventArgs e) 
        {
            var timeSpan = (btnPause.dateTimePicker.Value - DateTime.MinValue).TotalSeconds;
            PauseTime.Value_int = (int)timeSpan;
            btnPause.OpomnikSetFormSetForm.Hide();
        }

        private void Updater_Elapsed(object sender, EventArgs e)
        {
            Task.Run(update);            
        }

        void update()
        {
            if (initialized)
            {
                Invoke(new MethodInvoker(delegate
                {
                    var test = ConvertPlcVarToDisplayTime(CurrentTime);

                    decideMainStopwatchText();
                    btnOpomnik.SetValue(ReminderTime);
                    btnPause.Text = ConvertPlcVarToDisplayTime(PauseTime);
                    backcolor();
                    autostart();
                    ManButtonFunctionDecide();
                }));

            }
        }

        void decideMainStopwatchText()
        {
            if (isPaused())
            {
                stopwatchTime.Text = ConvertPlcVarToDisplayTime(PauseLeft);
            }
            else
            {
                stopwatchTime.Text = ConvertPlcVarToDisplayTime(TimeLeft);
            }            
        }

        void autostart()
        {
            if (UpostevajPrisotnost.Value_short >= 1)
            {
                chkUpostevajPrisotnost.Checked = true;
            }
            else
            {
                chkUpostevajPrisotnost.Checked = false;
            }
        }

        void backcolor()
        {
          
            if (isRunning())
            {
                BackColor = ActiveBackColor;
            }            
            else if (isFinished())
            {
                BackColor = AlertBackColor;
            }
            else if (isReadyToStart())
            {
                BackColor = NormalColor;
            }
            else if (isPaused())
            {
                BackColor = PausedBackColor;
            }
            else
            {
                BackColor = NormalColor;
            }
        }

        bool isRunning()
        {
            return InProgress.Value_bool && !Paused.Value_bool && !Finished.Value_bool;
        }

        bool isPaused()
        {
            return !Finished.Value_bool && Paused.Value_bool && InProgress.Value_bool;
        }

        bool isFinished()
        {
            return Finished.Value_bool && Paused.Value_bool && InProgress.Value_bool;
        }

        bool isReadyToStart()
        {
            return !Finished.Value_bool && Paused.Value_bool && !InProgress.Value_bool;
        }

        public static string ConvertPlcVarToDisplayTime(PlcVars.DWord plcTime)
        {
            var buff = new TimeSpan(0, 0, plcTime.Value_int);
            var str = buff.ToString(dflttimeFormat2);
            return str;  
        }

        public static string ConvertPlcVarToDisplayTime(PlcVars.Word plcTime)
        {
            var buff = new TimeSpan(0, 0, (int)plcTime.Value_short);
            return buff.ToString(dflttimeFormat2);
        }

        public static DateTime ConvertPlcVarToDateTime(PlcVars.DWord plcTime)
        {
            var d = DateTime.MinValue;
            var dt = d.AddSeconds(plcTime.Value_int);
            return dt;
        }
        public static DateTime ConvertPlcVarToDateTime(PlcVars.Word plcTime)
        {
            var d = DateTime.MinValue;
            var dt = d.AddSeconds((int)plcTime.Value_short);
            return dt;
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

        public string UnknownID()
        {
            return "UNKNOWN ID!"; // TODO - avtomatsko generiranje imen za saržo
        }
               
        public class BtnTimeset : Button
        {
            public Form OpomnikSetFormSetForm = new Form();
            public MyDateTimePicker dateTimePicker = new MyDateTimePicker();
            Label label = new Label();
            public Button OKbtn = new Button();
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
                    Text = value.ToString(dflttimeFormat);                    
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
                
            }

            private void SetTemps_form_LostFocus(object sender, EventArgs e)
            {
                OpomnikSetFormSetForm.Focus();
            }

            public void ShowForm()
            {
                OpomnikSetFormSetForm.Show();
                dateTimePicker.Value = Value;
                dateTimePicker.UpdateTextBox();
            }

            public void SetValue(DateTime value)
            {
                Value = value;
            }

            public void SetValue(PlcVars.DWord value)
            {                
                Value = ConvertPlcVarToDateTime(value);
            }
            public void SetValue(PlcVars.Word value)
            {
                Value = ConvertPlcVarToDateTime(value);
            }

        }

        public class ManButton : Button
        {
            Functions _CurrentFunction = Functions.Reset;
            public Functions CurrentFunction
            {
                get { return _CurrentFunction; }
                set
                {
                    _CurrentFunction = value;
                    UpdateFunction();
                }
            }

            public event EventHandler Reset_Clicked, Start_Clicked, Stop_Clicked;

            public ManButton()
            {
                Click += ManButton_Click;
                UpdateFunction();
            }

            private void ManButton_Click(object sender, EventArgs e)
            {
                if (CurrentFunction == Functions.Reset)
                {
                    if (Reset_Clicked != null)
                    {
                        Reset_Clicked.Invoke(this, EventArgs.Empty);
                    }

                }
                else if (CurrentFunction == Functions.Start)
                {
                    if (Start_Clicked != null)
                    {
                        Start_Clicked.Invoke(this, EventArgs.Empty);
                    }

                }
                else if (CurrentFunction == Functions.Stop)
                {
                    if (Stop_Clicked != null)
                    {
                        Stop_Clicked.Invoke(this, EventArgs.Empty);
                    }
                }
            }

            void UpdateFunction()
            {
                if (CurrentFunction == Functions.Reset)
                {
                    Text = "Reset";
                    Visible = true;
                }
                else if (CurrentFunction == Functions.Start)
                {
                    Text = "Start";
                    Visible = true;
                }
                else if (CurrentFunction == Functions.Stop)
                {
                    Text = "Stop";
                    Visible = true;
                }
                else if (CurrentFunction == Functions.Invisible)
                {
                    Text = "";
                    Visible = false;
                }
            }

            public enum Functions
            {
                Reset = 0,
                Stop = 1,
                Start = 2,
                Invisible = 3
            }
        }

    }

    
}
