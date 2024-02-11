using System.Collections.Generic;
using System.Linq;
using System.Web;
using System;
using System.Threading;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;

namespace KontrolaKadi
{

    public class WarningManager : SGroupBox
    {
        RichTextBox tb;
        // Local warnings        
        public static List<Warning> Warnings = new List<Warning>();

        MyTimer DisplayOnScreenTimer;
        Thread ConnecttionStatusErrorReportThread;

        public WarningManager()
        {
            tb = new RichTextBox()
            {
                Font = new Font("arial", 10),
                Height = 150,
                Width = 260,
                Top = 25,
                Left = 5
            };

            Controls.Add(tb);
            StartWarningTrackerThread();


            tb.Multiline = true;
            Setup();

            Width = tb.Width + tb.Left*2;
            Height = tb.Height + tb.Top+5;

            Font = new Font("arial", 12, FontStyle.Bold);
            tb.ScrollBars = RichTextBoxScrollBars.None;
        }

        void AddTitle()
        {
            Text = "ALARMI";
        }

        static Misc.SmartThread WarningTrackerThread;        
        static readonly List<Tracker> MessageTrackerList = new List<Tracker>();

        void Setup()
        {
            bool designMode = (LicenseManager.UsageMode == LicenseUsageMode.Designtime);
            if (designMode)
            {
                return;                
            }

            tb.ReadOnly = true;
            tb.HandleCreated += WarningManager_HandleCreated;            
        }

        private void WarningManager_HandleCreated(object sender, EventArgs e)
        {
            AddTitle();
            tb.Enter += WarningManager_Enter;

            ShowOnDisplayMethodInvoker = new MethodInvoker(delegate { ShowOnDisplay(); });
            DisplayOnScreenTimer = new MyTimer()
            {
                Interval = Settings.UpdateValuesPCms,
                AutoReset = false
            };
            DisplayOnScreenTimer.Elapsed += DisplayOnScreenTimer_Elapsed;
            DisplayOnScreenTimer.Start();

            ConnecttionStatusErrorReportThread = new Thread(connectionStatusErrorReport);
            ConnecttionStatusErrorReportThread.Start();

            
        }


        MethodInvoker ShowOnDisplayMethodInvoker;
        private void DisplayOnScreenTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Invoke(ShowOnDisplayMethodInvoker);
        }

        void connectionStatusErrorReport()
        {
            var form = FindForm();
            var controls = Helper.GetAllControls(form);

            foreach (var item in controls)
            {
                if (item.GetType() == typeof(ConnectedButton))
                {
                    // TODO connection error report
                }
            }
        }

        private void WarningManager_Enter(object sender, EventArgs e)
        {
            Helper.Unfocus(sender);
        }


        string sporocilaZaprikazBuff = "";
        void ShowOnDisplay()
        {
            try
            {
                sporocilaZaprikazBuff = "";

                for (int i = 0; i < Warnings.Count; i++)
                {
                    sporocilaZaprikazBuff += Warnings[i].GetMessage() + Environment.NewLine;
                }
                tb.Text = sporocilaZaprikazBuff;
            }
            catch (Exception ex)
            {
                throw;
            }

            DisplayOnScreenTimer.Start();


        }
        public static void AddMessageForUser_Warning(string source, string message)
        {
            if (!PreventThisMessage_IsDuplicacate(source, message))
            {
                Warnings.Add(new Warning(source, message));
                SysLog.Message.SetMessage("Message shown to user: " + message);
            }
        }

        void WarningTrackerThreadMethod()
        {
            bool Alarm = false;                
            
            try
            {
                while (true)
                {
                    Thread.Sleep(Settings.UpdateValuesPCms);

                    foreach (var item in MessageTrackerList)
                    {
                        Alarm = item.UpdateValue_TriggerAlarm();

                        if (Alarm)
                        {
                            AddMessageForUser_Warning(item.WarningSource, item.WarningMessage);
                        }
                        else
                        {                            
                            RemoveMessageForUser_Warning(item.WarningSource, item.WarningMessage);
                        }
                    }
                }                
                                               
            }
            catch (Exception ex)
            {
                throw new Exception("WarningTrackerThread encountered an error and was terminated: " + ex.Message);
            }
        }

        public static void AddWarningTrackerFromPLCVar(PlcVars.PlcType PlcVar, object valueToTrigerWarning, WarningTriggerCondition Condition, string WarningSource, string WarningMessage)
        {
            Tracker t = new Tracker(PlcVar, valueToTrigerWarning, Condition, WarningSource, WarningMessage);
            MessageTrackerList.Add(t);
                        
        }

        public static void AddMessageForUser_Warning(Warning warning)
        {
            if (!PreventThisMessage_IsDuplicacate(warning.GetSource(), warning.GetMessage()))
            { 
                Warnings.Add(warning);
                SysLog.Message.SetMessage("Message shown to user: " + warning);
            }
            
        }

        public static void RemoveMessageForUser_Warning(Warning warning)
        {
            if (Warnings.Contains(warning))
            {
                Warnings.Remove(warning);
            }            
        }
        public static void RemoveMessageForUser_Warning(string source, string warning)
        {
            if (Warnings != null)
            {         
                Warning buff;                
                for (int i = 0; i < Warnings.Count; i++)
                {
                    buff = Warnings[i];
                    if (buff.GetMessage() == warning)
                    {
                        if (buff.GetSource() == source)
                        {
                            Warnings.RemoveAt(i);
                            return;
                        }
                        
                    }
                }
            }
            
        }

        public enum WarningTriggerCondition
        {
            EqualTo,
            NotEqualTo,
            GreaterThan,
            LessThan,
            GreaterThanOrEqualTo,
            LessThanOrEqualTo
        }

        
        static bool PreventThisMessage_IsDuplicacate(string source, string message)
        {
            if (Warnings != null)
            {
                foreach (var item in Warnings)
                {
                    if (message == item.GetMessage())
                    {
                        if (source == item.GetSource())
                        {
                            return true;
                        }                        
                    }
                }
            }
            return false;
        }

        void StartWarningTrackerThread()
        {
            WarningTrackerThread = new Misc.SmartThread(() => WarningTrackerThreadMethod());
            WarningTrackerThread.Start("WarningTrackerThread", ApartmentState.MTA, true);
        }

    

        class Tracker
        {
            public PlcVars.PlcType PlcVar;
            public object valueToTrigerWarning;
            public WarningTriggerCondition Condition;
            public string WarningMessage;
            public string WarningSource;

            public Tracker(PlcVars.PlcType PlcVar, object valueToTrigerWarning, WarningTriggerCondition Condition, string WarningSource, string WarningMessage)
            {
                this.WarningMessage = WarningMessage;
                this.WarningSource = WarningSource;

                // Type checks only
                var typ = PlcVar.GetType();
                PlcVars.PlcType buff;

                var typ1 = valueToTrigerWarning.GetType();
                object buff1;

                if (typ == typeof(PlcVars.AlarmBit))
                {
                    buff = (PlcVars.AlarmBit)PlcVar;
                }

                else if (typ == typeof(PlcVars.Bit))
                {
                    buff = (PlcVars.Bit)PlcVar;
                }

                else if (typ == typeof(PlcVars.Byte))
                {
                    buff = (PlcVars.Byte)PlcVar;
                }

                else if (typ == typeof(PlcVars.Word))
                {
                    buff = (PlcVars.Word)PlcVar;
                }

                else if (typ == typeof(PlcVars.DWord))
                {
                    buff = (PlcVars.DWord)PlcVar;
                }

                else
                {
                    throw new Exception("Error: Unsupported type was passed (PlcVars.PlcType PlcVar). Supported types are: PlcVars.Bit, PlcVars.Byte, PlcVars.Word, PlcVars.DWord. Parent Class of this exception is WarningManager");
                }

                this.PlcVar = buff;

                //


                string messageErrorTypeConflict = "Error: Type conflict. If PlcVars.Bit was passed, valueToTrigerWarning should be of type bool, and vice versa.";

               
                if (typ1 == typeof(bool))
                {
                    if (Condition != WarningTriggerCondition.EqualTo && Condition != WarningTriggerCondition.NotEqualTo)
                    { }
                    else if (typ == typeof(PlcVars.AlarmBit))
                    { }
                    else if (typ != typeof(PlcVars.Bit))
                    { }
                    else
                    {
                        throw new Exception(messageErrorTypeConflict);
                    }

                    buff1 = (bool)valueToTrigerWarning;
                }

                else if (typ1 == typeof(short))
                {
                    if (typ == typeof(PlcVars.Bit))
                    {
                        throw new Exception(messageErrorTypeConflict);
                    }
                    buff1 = (short)valueToTrigerWarning;
                }

                else if (typ1 == typeof(int))
                {
                    if (typ == typeof(PlcVars.Bit))
                    {
                        throw new Exception(messageErrorTypeConflict);
                    }
                    buff1 = (int)valueToTrigerWarning;
                }

                else if (typ1 == typeof(bool?))
                {
                    if (typ != typeof(PlcVars.Bit))
                    {
                        throw new Exception(messageErrorTypeConflict);
                    }
                    buff1 = (bool?)valueToTrigerWarning;
                }

                else if (typ1 == typeof(short?))
                {
                    if (typ == typeof(PlcVars.Bit))
                    {
                        throw new Exception(messageErrorTypeConflict);
                    }
                    buff1 = (short?)valueToTrigerWarning;
                }

                else if (typ1 == typeof(int?))
                {
                    if (typ == typeof(PlcVars.Bit))
                    {
                        throw new Exception(messageErrorTypeConflict);
                    }
                    buff1 = (int?)valueToTrigerWarning;
                }

                else
                {
                    throw new Exception("Error: Unsupported type was passed (object valueToTrigerWarning). Supported types are: bool, short, int, bool?, short?, int? . Parent Class of this exception is WarningManager");
                }

                this.valueToTrigerWarning = buff1;


                this.Condition = Condition;
            }

            public bool UpdateValue_TriggerAlarm()
            {
                try
                {
                    // Type checks
                    var typ = PlcVar.GetType();

                    if (typ == typeof(PlcVars.AlarmBit))
                    {
                        var buff = (PlcVars.AlarmBit)PlcVar;
                        var val = buff.Value;

                        if (val != null)
                        {
                            return CompareBool_Alarm((bool)val);
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else if (typ == typeof(PlcVars.Bit))
                    {
                        var buff = (PlcVars.Bit)PlcVar;
                        var val = buff.Value;

                        if (val != null)
                        {
                            return CompareBool_Alarm((bool)val);
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else if (typ == typeof(PlcVars.Byte))
                    {
                        var buff = (PlcVars.Byte)PlcVar;
                        var val = buff.Value;

                        if (val != null)
                        {
                            return CompareOthers_Alarm((short)val);
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else if (typ == typeof(PlcVars.Word))
                    {
                        var buff = (PlcVars.Word)PlcVar;
                        var val = buff.Value;

                        if (val != null)
                        {
                            return CompareOthers_Alarm((short)val);
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else if (typ == typeof(PlcVars.DWord))
                    {
                        var buff = (PlcVars.DWord)PlcVar;
                        var val = buff.Value;

                        if (val != null)
                        {
                            return CompareOthers_Alarm((short)val);
                        }
                        else
                        {
                            return false;
                        }
                    }

                    throw new Exception("Wrong type was stored in collection.");
                }
                catch (Exception ex)
                {

                    throw new Exception("UpdateValue() reportred an Error (parent class: WarningManager): " + ex.Message);
                }

            }

            bool CompareBool_Alarm(bool val)
            {

                if (Condition == WarningTriggerCondition.EqualTo)
                {
                    if (val == (bool)valueToTrigerWarning)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                if (Condition == WarningTriggerCondition.NotEqualTo)
                {
                    if (val == (bool)valueToTrigerWarning)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }

                throw new Exception("Condition parameter was invalid.");
            }

            bool CompareOthers_Alarm(short val)
            {
                if (Condition == WarningTriggerCondition.EqualTo)
                {
                    if (val == (int)valueToTrigerWarning)
                    {
                        return true;
                    }
                    else { return false; }
                }

                if (Condition == WarningTriggerCondition.GreaterThan)
                {
                    if (val > (int)valueToTrigerWarning)
                    {
                        return true;
                    }
                    else { return false; }
                }

                if (Condition == WarningTriggerCondition.GreaterThanOrEqualTo)
                {
                    if (val >= (int)valueToTrigerWarning)
                    {
                        return true;
                    }
                    else { return false; }
                }

                if (Condition == WarningTriggerCondition.LessThan)
                {
                    if (val < (int)valueToTrigerWarning)
                    {
                        return true;
                    }
                    else { return false; }
                }

                if (Condition == WarningTriggerCondition.LessThanOrEqualTo)
                {
                    if (val <= (int)valueToTrigerWarning)
                    {
                        return true;
                    }
                    else { return false; }
                }

                if (Condition == WarningTriggerCondition.NotEqualTo)
                {
                    if (val != (int)valueToTrigerWarning)
                    {
                        return true;
                    }
                    else { return false; }
                }

                throw new Exception("Condition parameter was invalid.");
            }
        }

        public class Warning
        {       
            readonly string message = "";
            readonly string source = "";

            public Warning(string Source, string Message)
            {               
                message = Message;
                source = Source;
            }

            public string GetCompleteMessage()
            {
                return "[" + source + "]" + message;
            }

            public string GetSource()
            {
                return  source;
            }

            public string GetMessage()
            {
                return message;
            }
        }
       
    }


}