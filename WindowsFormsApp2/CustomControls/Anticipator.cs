using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace KontrolaKadi
{
    public class Anticipator : SGroupBox
    {
        RichTextBox tb;
        List<AnticipatedEvent> anticipatedEventList = new List<AnticipatedEvent>();
        List<AnticipatedEvent> anticipatedEventListBuffer = new List<AnticipatedEvent>();

        List<StopWatch> StopWatchList = new List<StopWatch>();

        MyTimer t = new MyTimer();

        Form ParentForm;

        public Anticipator()
        {
            tb = new RichTextBox()
            {
                Font = new Font("arial", 10),
                Multiline = true,
                Height = 150,
                Width = 260,
                Top = 25,
                Left = 5,
                ReadOnly = true
            };

            Width = tb.Width + tb.Left*2;
            Height = tb.Height + tb.Top + 5;

            HandleCreated += Anticipator_HandleCreated;

            t.Interval = Settings.UpdateValuesPCms;
            t.Elapsed += T_Elapsed;
            t.AutoReset = false;
            t.Start();
        }

        private void T_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Task.Run(update);
        }

        void update()
        {
            anticipatedEventListBuffer.Clear();
            geteventsFromStopWatch();
            sortEvents();
            anticipatedEventList = anticipatedEventListBuffer;
            showToUser();
            t.Start();
        }

        void showToUser()
        {
            if (InvokeRequired)
            {
                var m = new MethodInvoker(showToUser);
                Invoke(m);
                return;
            }

            try
            {
                string timeToGo = "";                
                tb.Clear();

                foreach (var item in anticipatedEventList)
                {
                    if (item.TimeToGo < TimeSpan.FromMinutes(60)) // limit to 1 hour
                    {                       
                        timeToGo = "[" + item.TimeToGo.ToString("mm") + "min] - ";
                        
                        tb.Text += timeToGo + item.Name + Environment.NewLine;
                    }                                     
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        void sortEvents()
        {
            anticipatedEventListBuffer.Sort((event1, event2 )=> event1.TimeToGo.CompareTo(event2));           
        }

        private void Anticipator_HandleCreated(object sender, EventArgs e)
        {
            Text = "Sledeči dogodki";
            Font = new Font("arial", 12, FontStyle.Bold);

            tb.Enter += Anticipator_Enter;
            Controls.Add(tb);
            ParentForm = FindForm();

            ParentForm.Load += ParentForm_Load;
        }

        private void ParentForm_Load(object sender, EventArgs e)
        {
            populateStopWatchList();
        }

        private void Anticipator_Enter(object sender, EventArgs e)
        {
            Helper.Unfocus(sender);
        }

        void populateStopWatchList()
        {
            var form = FindForm();
            var controls = Helper.GetAllControls(form);

            foreach (var item in controls)
            {
                if (item.GetType() == typeof(StopWatch))
                {
                    StopWatchList.Add((StopWatch)item);
                }
            }
        }
                
        void geteventsFromStopWatch()
        {            
            foreach (var item in StopWatchList)
            {
                if (item.InProgress != null && item.InProgress.Value_bool)
                {
                    var t = item.TimeLeft.Value_short;
                    var name = getNameFromStopwatch(item);
                    var evnt = new AnticipatedEvent(t, name);
                    anticipatedEventListBuffer.Add(evnt);
                }                
            }        
        }

        string getNameFromStopwatch(StopWatch stpw)
        {
            try
            {
                Type t = stpw.Parent.GetType();

                if (t.BaseType == typeof(Kad))
                {
                    var kad = (Kad)stpw.Parent;
                    return kad.DisplayName;
                }
                else
                {
                    throw new Exception("Stopwatch must be child control od Kad type control, and not child control of any other control or a form directly");
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            
        }
    }

    public class AnticipatedEvent
    {
        public TimeSpan TimeToGo { get; set; }
        public string Name { get; set; }
        public AnticipatedEvent(TimeSpan TimeToGo, string Name)
        {
            ctor(TimeToGo, Name);
        }

        public AnticipatedEvent(int TimeToGo_s, string Name)
        {
            var t = new TimeSpan(0, 0, TimeToGo_s);
            ctor(t, Name);
        }

        void ctor(TimeSpan TimeToGo, string Name)
        {
            this.TimeToGo = TimeToGo;
            this.Name = Name;
        }
    }
}
