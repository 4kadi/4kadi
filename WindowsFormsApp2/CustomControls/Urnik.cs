using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;

namespace KontrolaKadi
{
    public class Urnik : GroupBox
    {
        bool classInitializedProperly;
        public bool ClassInitializedProperly
        {
            get { return classInitializedProperly; }
            private set
            {
                classInitializedProperly = value;
                if (classInitializedProperly)
                { ValueChanged(this, EventArgs.Empty); }
            }
        }

        Label nextEventDescription = new Label();

        SetTempQuick panelTempAktivnegaUrnika, panelTempNeaktivnegaUrnika;

        Form ParentForm;

        public event EventHandler NextEventDescription_Changed;

        public string NextEventDescription
        {
            get { return nextEventDescription.Text; }
            private set
            {
                nextEventDescription.Text = value;
                if (NextEventDescription != null)
                {
                    NextEventDescription_Changed.Invoke(this, EventArgs.Empty);
                }
            }
        }

        Color ColorDefault;

        UrnikSection section1, section2, section3, section4, section5, section6;
        SGroupBox sectionTemperature;

        PlcVars.Bit _UrnikAktiven;
        PlcVars.AlarmBit _NapakaNastTemperatur;
        PlcVars.LogoDateTime _currentTime;

        PlcVars.Byte _DayOfTheWeek1;
        PlcVars.Word _StartTime1;
        PlcVars.Word _EndTime1;

        PlcVars.Byte _DayOfTheWeek2;
        PlcVars.Word _StartTime2;
        PlcVars.Word _EndTime2;

        PlcVars.Byte _DayOfTheWeek3;
        PlcVars.Word _StartTime3;
        PlcVars.Word _EndTime3;

        PlcVars.Byte _DayOfTheWeek4;
        PlcVars.Word _StartTime4;
        PlcVars.Word _EndTime4;

        PlcVars.Byte _DayOfTheWeek5;
        PlcVars.Word _StartTime5;
        PlcVars.Word _EndTime5;

        PlcVars.Byte _DayOfTheWeek6;
        PlcVars.Word _StartTime6;
        PlcVars.Word _EndTime6;

        PlcVars.Word _TemperaturaAktivnegaUrnika;
        PlcVars.Word _TemperaturaNektivnegaUrnika;


        public PlcVars.LogoDateTime currentTime
        {
            get
            {
                return _currentTime;
            }
            set
            {
                _currentTime = value; registerEvent(value);
                section1.CurrentTime = value; section2.CurrentTime = value; section3.CurrentTime = value; section4.CurrentTime = value; section5.CurrentTime = value; section6.CurrentTime = value;
            }
        }

        public PlcVars.Bit UrnikAktiven { get { return _UrnikAktiven; } set { _UrnikAktiven = value; registerEvent(value); } }

        public PlcVars.AlarmBit NapakaNastTemperatur { get { return _NapakaNastTemperatur; } set { _NapakaNastTemperatur = value; registerEvent(value); } }

        public PlcVars.Byte DayOfTheWeek1 { get { return _DayOfTheWeek1; } set { _DayOfTheWeek1 = value; section1.DayOfTheWeek = value; registerEvent(value); } }
        public PlcVars.Word StartTime1 { get { return _StartTime1; } set { _StartTime1 = value; section1.StartTime = value; registerEvent(value); } }
        public PlcVars.Word EndTime1 { get { return _EndTime1; } set { _EndTime1 = value; section1.EndTime = value; registerEvent(value); } }

        public PlcVars.Byte DayOfTheWeek2 { get { return _DayOfTheWeek2; } set { _DayOfTheWeek2 = value; section2.DayOfTheWeek = value; registerEvent(value); } }
        public PlcVars.Word StartTime2 { get { return _StartTime2; } set { _StartTime2 = value; section2.StartTime = value; registerEvent(value); } }
        public PlcVars.Word EndTime2 { get { return _EndTime2; } set { _EndTime2 = value; section2.EndTime = value; registerEvent(value); } }

        public PlcVars.Byte DayOfTheWeek3 { get { return _DayOfTheWeek3; } set { _DayOfTheWeek3 = value; section3.DayOfTheWeek = value; registerEvent(value); } }
        public PlcVars.Word StartTime3 { get { return _StartTime3; } set { _StartTime3 = value; section3.StartTime = value; registerEvent(value); } }
        public PlcVars.Word EndTime3 { get { return _EndTime3; } set { _EndTime3 = value; section3.EndTime = value; registerEvent(value); } }

        public PlcVars.Byte DayOfTheWeek4 { get { return _DayOfTheWeek4; } set { _DayOfTheWeek4 = value; section4.DayOfTheWeek = value; registerEvent(value); } }
        public PlcVars.Word StartTime4 { get { return _StartTime4; } set { _StartTime4 = value; section4.StartTime = value; registerEvent(value); } }
        public PlcVars.Word EndTime4 { get { return _EndTime4; } set { _EndTime4 = value; section4.EndTime = value; registerEvent(value); } }

        public PlcVars.Byte DayOfTheWeek5 { get { return _DayOfTheWeek5; } set { _DayOfTheWeek5 = value; section5.DayOfTheWeek = value; registerEvent(value); } }
        public PlcVars.Word StartTime5 { get { return _StartTime5; } set { _StartTime5 = value; section5.StartTime = value; registerEvent(value); } }
        public PlcVars.Word EndTime5 { get { return _EndTime5; } set { _EndTime5 = value; section5.EndTime = value; registerEvent(value); } }

        public PlcVars.Byte DayOfTheWeek6 { get { return _DayOfTheWeek6; } set { _DayOfTheWeek6 = value; section6.DayOfTheWeek = value; registerEvent(value); } }
        public PlcVars.Word StartTime6 { get { return _StartTime6; } set { _StartTime6 = value; section6.StartTime = value; registerEvent(value); } }
        public PlcVars.Word EndTime6 { get { return _EndTime6; } set { _EndTime6 = value; section6.EndTime = value; registerEvent(value); } }

        public PlcVars.Word TemperaturaAktivnegaUrnika { get { return _TemperaturaAktivnegaUrnika; } set { _TemperaturaAktivnegaUrnika = value; panelTempAktivnegaUrnika.TemperaturaNastavljena = value; registerEvent(value); } }
        public PlcVars.Word TemperaturaNektivnegaUrnika { get { return _TemperaturaNektivnegaUrnika; } set { _TemperaturaNektivnegaUrnika = value; panelTempNeaktivnegaUrnika.TemperaturaNastavljena = value; registerEvent(value); } }



        public Urnik()
        {
            Font = new Font("arial", 15, FontStyle.Bold);

            nextEventDescription.Top = 25;
            nextEventDescription.Left = 60;
            nextEventDescription.Width = 380;
            nextEventDescription.BackColor = Color.LightBlue;
            nextEventDescription.Text = PropComm.NA;
            nextEventDescription.Font = new Font("arial", 10, FontStyle.Bold);
            Controls.Add(nextEventDescription);

            section1 = new UrnikSection();
            section1.Top = 50;
            section1.Left = 20;
            section1.Name = "urnikSection1";
            section1.Text = "Urnik 1";
            Controls.Add(section1);

            section2 = new UrnikSection();
            section2.Top = section1.Bottom + 5;
            section2.Left = 20;
            section2.Name = "urnikSection2";
            section2.Text = "Urnik 2";
            Controls.Add(section2);

            section3 = new UrnikSection();
            section3.Top = section2.Bottom + 5;
            section3.Left = 20;
            section3.Name = "urnikSection3";
            section3.Text = "Urnik 3";
            Controls.Add(section3);

            section4 = new UrnikSection();
            section4.Top = section3.Bottom + 5;
            section4.Left = 20;
            section4.Name = "urniksection4";
            section4.Text = "Urnik 4";
            Controls.Add(section4);

            section5 = new UrnikSection();
            section5.Top = section4.Bottom + 5;
            section5.Left = 20;
            section5.Name = "urniksection5";
            section5.Text = "Urnik 5";
            Controls.Add(section5);

            section6 = new UrnikSection();
            section6.Top = section5.Bottom + 5;
            section6.Left = 20;
            section6.Name = "urniksection6" + 5;
            section6.Text = "Urnik 6";
            Controls.Add(section6);

            manageTemperatureSection();

            Height = sectionTemperature.Bottom - section1.Top + 60;
            Width = section1.Right - section1.Left + 40;

            HandleCreated += Urnik_HandleCreated;
        }

        private void Urnik_HandleCreated(object sender, EventArgs e)
        {
            ParentForm = FindForm();
            ParentForm.VisibleChanged += ParentForm_VisibleChanged;
        }

        private void ParentForm_VisibleChanged(object sender, EventArgs e)
        {
            updateControls();
        }

        void registerEvent(PlcVars.Bit val)
        {
            InitializeIfPossible();
            val.ValueChanged += ValueChanged;
        }

        void registerEvent(PlcVars.Byte val)
        {
            InitializeIfPossible();
            val.ValueChanged += ValueChanged;
        }

        void registerEvent(PlcVars.Word val)
        {
            InitializeIfPossible();
            val.ValueChanged += ValueChanged;
        }

        void registerEvent(PlcVars.DWord val)
        {
            InitializeIfPossible();

            val.ValueChanged += ValueChanged;
        }

        void registerEvent(PlcVars.LogoDateTime val)
        {
            InitializeIfPossible();
            val.ValueChanged += ValueChanged;
        }

        void registerOtherEvents()
        {
            UrnikAktiven.ValueChanged += UrnikAktiven_ValueChanged;
            NapakaNastTemperatur.ValueChanged += NapakaNastTemperatur_ValueChanged;
            TemperaturaAktivnegaUrnika.ValueChanged += TemperaturaAktivnegaUrnika_ValueChanged;
            TemperaturaNektivnegaUrnika.ValueChanged += TemperaturaNektivnegaUrnika_ValueChanged;
        }

        private void UrnikAktiven_ValueChanged(object sender, EventArgs e)
        {
            if (UrnikAktiven.Value_bool)
            {
                panelTempAktivnegaUrnika.Highlight();
                panelTempNeaktivnegaUrnika.RemoveHighlight();
            }
            else
            {
                panelTempAktivnegaUrnika.RemoveHighlight();
                panelTempNeaktivnegaUrnika.Highlight();
            }
        }

        private void TemperaturaNektivnegaUrnika_ValueChanged(object sender, EventArgs e)
        {
            panelTempNeaktivnegaUrnika.Value = TemperaturaNektivnegaUrnika.Value_short;
        }

        private void TemperaturaAktivnegaUrnika_ValueChanged(object sender, EventArgs e)
        {
            panelTempAktivnegaUrnika.Value = TemperaturaAktivnegaUrnika.Value_short;
        }

        private void NapakaNastTemperatur_ValueChanged(object sender, EventArgs e)
        {
            var m = new MethodInvoker(delegate
            {
                if (NapakaNastTemperatur.Value_bool)
                {
                    sectionTemperature.BackColor = Color.Red;
                }
                else
                {
                    sectionTemperature.BackColor = ColorDefault;
                }
            });
            Invoke(m);

        }

        private bool InitializeIfPossible()
        {
            var init = this.ClassInitializedProperly = AreAllVariablesInitialized();

            if (init)
            {
                registerOtherEvents();
            }

            return init;
        }
        public bool AreAllVariablesInitialized()
        {
            // Check if any of the variables are null
            return currentTime != null &&
                   UrnikAktiven != null &&
                   DayOfTheWeek1 != null &&
                   StartTime1 != null &&
                   EndTime1 != null &&
                   DayOfTheWeek2 != null &&
                   StartTime2 != null &&
                   EndTime2 != null &&
                   DayOfTheWeek3 != null &&
                   StartTime3 != null &&
                   EndTime3 != null &&
                   DayOfTheWeek4 != null &&
                   StartTime4 != null &&
                   EndTime4 != null &&
                   DayOfTheWeek5 != null &&
                   StartTime5 != null &&
                   EndTime5 != null &&
                   DayOfTheWeek6 != null &&
                   StartTime6 != null &&
                   EndTime6 != null &&
                   NapakaNastTemperatur != null &&
                   TemperaturaAktivnegaUrnika != null &&
                   TemperaturaNektivnegaUrnika != null;
        }

        private void ValueChanged(object sender, EventArgs e)
        {
            ValueChanged();
        }

        private void ValueChanged()
        {
            if (!ClassInitializedProperly)
            {
                return;
            }

            var ne = getNextEvent();

            if (InvokeRequired)
            {
                MethodInvoker m = new MethodInvoker(ValueChanged);
                Invoke(m);
                return;
            }

            try
            {
                if (ne != null)
                {
                    NextEventDescription = GetNextEventDescription(ne);
                }
                else
                {
                    NextEventDescription = "Urnik je izključen.";
                }
                updateControls();
            }
            catch
            {

            }
        }

        public string GetNextEventDescription(OneDaysEvent eventPart)
        {
            if (eventPart == null)
            {
                return "Urnik je izključen";
            }

            string msg = eventPart.CurrentlyActive ? "VKLOP" : "IZKLOP";

            int hours = eventPart.EventTime.Hours;
            int minutes = eventPart.EventTime.Minutes;

            msg += $" - {eventPart.DayOfWeek.ToString()} ob {hours:00}:{minutes:00}";

            return msg;
        }

        void updateControls()
        {
            section1.UpdateControls();
            section2.UpdateControls();
            section3.UpdateControls();
            section4.UpdateControls();
            section5.UpdateControls();
            section6.UpdateControls();
            UpdateSectionTemperatureControls();

        }

        void UpdateSectionTemperatureControls()
        {

        }

        private List<OneDaysEvent> SortEventsOneWeekFromNow(List<OneDaysEvent> events)
        {
            events.Sort((x, y) => x.TimeUntilEvent.CompareTo(y.TimeUntilEvent));

            return events;
        }



        private OneDaysEvent getNextEvent()
        {
            var allEvents = getAllEvents();
            var FolowingEventsSorted = SortEventsOneWeekFromNow(allEvents);
            if (FolowingEventsSorted.Count > 0)
            {
                return FolowingEventsSorted[0];
            }

            return null;
        }
        private List<WeektimerEvent> getAllWeektimerEvents()
        {
            List<WeektimerEvent> buff = new List<WeektimerEvent>();

            var wte1 = new WeektimerEvent(StartTime1, EndTime1, DayOfTheWeek1); buff.Add(wte1);
            var wte2 = new WeektimerEvent(StartTime2, EndTime2, DayOfTheWeek2); buff.Add(wte2);
            var wte3 = new WeektimerEvent(StartTime3, EndTime3, DayOfTheWeek3); buff.Add(wte3);
            var wte4 = new WeektimerEvent(StartTime4, EndTime4, DayOfTheWeek4); buff.Add(wte4);
            var wte5 = new WeektimerEvent(StartTime5, EndTime5, DayOfTheWeek5); buff.Add(wte5);
            var wte6 = new WeektimerEvent(StartTime6, EndTime6, DayOfTheWeek6); buff.Add(wte6);

            return buff;
        }

        private List<OneDaysEvent> getAllEvents()
        {
            if (currentTime == null)
            {
                return new List<OneDaysEvent>();
            }

            List<WeektimerEvent> weektimerEvents = getAllWeektimerEvents();
            var AllWeekEvents = new List<OneDaysEvent>();

            foreach (var weektimerEvent in weektimerEvents)
            {
                if (weektimerEvent.Monday)
                {
                    AllWeekEvents.Add(new OneDaysEvent(currentTime, CustomDayOfWeek.Ponedeljek, weektimerEvent.StartTime, true));
                    AllWeekEvents.Add(new OneDaysEvent(currentTime, CustomDayOfWeek.Ponedeljek, weektimerEvent.EndTime, false));
                }
                if (weektimerEvent.Tuesday)
                {
                    AllWeekEvents.Add(new OneDaysEvent(currentTime, CustomDayOfWeek.Torek, weektimerEvent.StartTime, true));
                    AllWeekEvents.Add(new OneDaysEvent(currentTime, CustomDayOfWeek.Torek, weektimerEvent.EndTime, false));
                }
                if (weektimerEvent.Wednesday)
                {
                    AllWeekEvents.Add(new OneDaysEvent(currentTime, CustomDayOfWeek.Sreda, weektimerEvent.StartTime, true));
                    AllWeekEvents.Add(new OneDaysEvent(currentTime, CustomDayOfWeek.Sreda, weektimerEvent.EndTime, false));
                }
                if (weektimerEvent.Thursday)
                {
                    AllWeekEvents.Add(new OneDaysEvent(currentTime, CustomDayOfWeek.Četrtek, weektimerEvent.StartTime, true));
                    AllWeekEvents.Add(new OneDaysEvent(currentTime, CustomDayOfWeek.Četrtek, weektimerEvent.EndTime, false));
                }
                if (weektimerEvent.Friday)
                {
                    AllWeekEvents.Add(new OneDaysEvent(currentTime, CustomDayOfWeek.Petek, weektimerEvent.StartTime, true));
                    AllWeekEvents.Add(new OneDaysEvent(currentTime, CustomDayOfWeek.Petek, weektimerEvent.EndTime, false));
                }
                if (weektimerEvent.Saturday)
                {
                    AllWeekEvents.Add(new OneDaysEvent(currentTime, CustomDayOfWeek.Sobota, weektimerEvent.StartTime, true));
                    AllWeekEvents.Add(new OneDaysEvent(currentTime, CustomDayOfWeek.Sobota, weektimerEvent.EndTime, false));
                }
                if (weektimerEvent.Sunday)
                {
                    AllWeekEvents.Add(new OneDaysEvent(currentTime, CustomDayOfWeek.Nedelja, weektimerEvent.StartTime, true));
                    AllWeekEvents.Add(new OneDaysEvent(currentTime, CustomDayOfWeek.Nedelja, weektimerEvent.EndTime, false));
                }
            }
            return AllWeekEvents;
        }

        void manageTemperatureSection()
        {
            sectionTemperature = new SGroupBox()
            {
                Top = section6.Bottom + 5,
                Left = section6.Left
            };

            panelTempAktivnegaUrnika = new SetTempQuick()
            {
                Top = 20,
                Left = 10,
                Text = "Temp. Aktivnega urnika",
                TemperaturaNastavljena = TemperaturaAktivnegaUrnika
            };

            sectionTemperature.Controls.Add(panelTempAktivnegaUrnika);

            panelTempNeaktivnegaUrnika = new SetTempQuick()
            {
                Top = panelTempAktivnegaUrnika.Top,
                Left = panelTempAktivnegaUrnika.Right + 10,
                Text = "Temp. Neaktivnega urnika"
            };

            sectionTemperature.Controls.Add(panelTempNeaktivnegaUrnika);
            Controls.Add(sectionTemperature);

            sectionTemperature.Width = section6.Width;
            sectionTemperature.Height = panelTempAktivnegaUrnika.Height + 30;

            ColorDefault = sectionTemperature.BackColor;

        }

    }

    public class WeektimerEvent
    {
        public TimeSpan StartTime { get; private set; }
        public TimeSpan EndTime { get; private set; }

        public bool Monday { get; private set; }
        public bool Tuesday { get; private set; }
        public bool Wednesday { get; private set; }
        public bool Thursday { get; private set; }
        public bool Friday { get; private set; }
        public bool Saturday { get; private set; }
        public bool Sunday { get; private set; }

        public WeektimerEvent(PlcVars.Word StartTime, PlcVars.Word EndTime, PlcVars.Byte WeekDay)
        {
            this.StartTime = LogoTimeEncoder.Time.GetTimespan(StartTime);
            this.EndTime = LogoTimeEncoder.Time.GetTimespan(EndTime);
            Monday = LogoTimeEncoder.WeekDay.IsItMonday(WeekDay);
            Tuesday = LogoTimeEncoder.WeekDay.IsItTuesday(WeekDay);
            Wednesday = LogoTimeEncoder.WeekDay.IsItWednesday(WeekDay);
            Thursday = LogoTimeEncoder.WeekDay.IsItThursday(WeekDay);
            Friday = LogoTimeEncoder.WeekDay.IsItFriday(WeekDay);
            Saturday = LogoTimeEncoder.WeekDay.IsItSaturday(WeekDay);
            Sunday = LogoTimeEncoder.WeekDay.IsItSunday(WeekDay);
        }
                
        public CustomDayOfWeek? GetNextActiveDay(TimeSpan currentTime)
        {

            var today = DateTime.Now.DayOfWeek;
            int todayIndex = (int)today; // DayOfWeek enum starts from 0 (Sunday) to 6 (Saturday)
            int nextDayIndex;

            // First, check if there's an active event today that hasn't ended yet
            CustomDayOfWeek todayCustomDay = (CustomDayOfWeek)(todayIndex + 1); // +1 to align with your enum where 1 is Sunday
            if (IsItActiveOnDay(todayCustomDay) && this.EndTime > currentTime)
            {
                return todayCustomDay;
            }

            // If today's event is over, or there's no event today, find the next active day
            for (int i = 1; i <= 7; i++)
            {
                nextDayIndex = (todayIndex + i) % 7;
                CustomDayOfWeek nextDay = (CustomDayOfWeek)(nextDayIndex + 1); // +1 for enum alignment

                if (IsItActiveOnDay(nextDay))
                {
                    return nextDay;
                }
            }

            return CustomDayOfWeek.Izključen; // No active day found
        }
        
        public bool IsItActiveOnAnyDay()
        {
            if (Monday || Tuesday || Wednesday || Thursday || Friday || Saturday || Sunday)
            {
                return true;
            }
            return false;
        }

        public bool IsItActiveOnDay(CustomDayOfWeek weekDay)
        {
            switch (weekDay)
            {
                case CustomDayOfWeek.Ponedeljek:
                    return Monday;
                case CustomDayOfWeek.Torek:
                    return Tuesday;
                case CustomDayOfWeek.Sreda:
                    return Wednesday;
                case CustomDayOfWeek.Četrtek:
                    return Thursday;
                case CustomDayOfWeek.Petek:
                    return Friday;
                case CustomDayOfWeek.Sobota:
                    return Saturday;
                case CustomDayOfWeek.Nedelja:
                    return Sunday;
                default:
                    return false;
            }
        }

        public bool IsItActiveToday()
        {
            var weekDay = (int)DateTime.Now.DayOfWeek + 1; // offset because type mismatch
            var todaysCustomDayOfWeek = (CustomDayOfWeek)weekDay;

            return IsItActiveOnDay(todaysCustomDayOfWeek);
        }
    }

    public class WeektimerEventPart
    {
        public bool turningOnOrOff;
        public CustomDayOfWeek dayOfWeek;
        public TimeSpan Time;

        public WeektimerEventPart(bool turningOnOrOff, CustomDayOfWeek dayOfWeek, TimeSpan Time)
        {
            this.turningOnOrOff = turningOnOrOff;
            this.dayOfWeek = dayOfWeek;
            this.Time = Time;
        }
    }
    
    class UrnikSection : GroupBox
    {        
        ChkBox mon1, tue1, wed1, thu1, fri1, sat1, sun1;
        ComboBoxHour cbHourOn, cbHourOff;
        ComboBoxMinute cbMinOn, cbMinOff;
        Label vklop;
        Label izklop;
        Label I1, I2;

        PlcVars.LogoDateTime _currentTime;
        public PlcVars.LogoDateTime CurrentTime
        {
            get { return _currentTime; }
            set
            {
                _currentTime = value;               
            }
        }        

        public PlcVars.Byte _DayOfTheWeek;
        public PlcVars.Word _StartTime;
        public PlcVars.Word _EndTime;

        public PlcVars.Byte DayOfTheWeek
        {
            get { return _DayOfTheWeek; }
            set 
            { 
                _DayOfTheWeek = value;
                initializeIfPossible();
            }
        }

        public PlcVars.Word StartTime
        {
            get { return _StartTime; }
            set 
            {
                _StartTime = value;
                initializeIfPossible();
            }
        }

        public PlcVars.Word EndTime
        {
            get { return _EndTime; }
            set 
            {
                _EndTime = value;
                initializeIfPossible();
            }
        }


        Color todaysColor = Color.Yellow;
        Color checkedColor = Color.LightGreen;
        Color uncheckedColor;

        public UrnikSection()
        {
            const int chkBoxTop = 20;
            const int spacing = 60;
            const int leftOffset = 10;
            const int width = 10;
            const int lbltop = 55;
            const int cbTop = lbltop - 3;

            Font = new Font("arial", 10, FontStyle.Bold);

            mon1 = new ChkBox()
            {
                AutoSize = true,
                Top = chkBoxTop,
                Left = leftOffset,
                Text = "pon",
                Width = width               
            };
            mon1.Click += Mon1_Click;

            uncheckedColor = mon1.BackColor;

            tue1 = new ChkBox()
            {
                AutoSize = true,
                Top = chkBoxTop,
                Left = leftOffset + spacing*1,
                Text = "tor",
                Width = width
            };
            tue1.Click += Tue1_Click;

            wed1 = new ChkBox()
            {                
                Top = chkBoxTop,
                Left = leftOffset + spacing*2,
                Text = "sre",
                Width = width
            }; 
            wed1.Click += Wed1_Click;

            thu1 = new ChkBox()
            {
                AutoSize = true,
                Top = chkBoxTop,
                Left = leftOffset + spacing*3,
                Text = "čet",
                Width = width
            };
            thu1.Click += Thu1_Click;

            fri1 = new ChkBox()
            {
                Top = chkBoxTop,
                Left = leftOffset + spacing*4,
                Text = "pet",
                Width = width
            };
            fri1.Click += Fri1_Click;

            sat1 = new ChkBox()
            {
                Top = chkBoxTop,
                Left = leftOffset + spacing*5,
                Text = "sob",
                Width = width
            };
            sat1.Click += Sat1_Click;

            sun1 = new ChkBox()
            {
                Top = chkBoxTop,
                Left = leftOffset + spacing*6,
                Text = "ned"
            };
            sun1.Click += Sun1_Click;

            //
            vklop = new Label()
            {
                AutoSize = true,
                Top = lbltop,
                Text = "Vklop ob:",
                Left = leftOffset,                
            };

            cbHourOn = new ComboBoxHour()
            {
                Top = cbTop,
                Left = vklop.Left + 73
            };
            cbHourOn.DropDownClosed += CbHourOn_DropDownClosed;            

            I1 = new Label()
            {
                AutoSize = true,
                Top = lbltop,
                Text = ":",
                Left = cbHourOn.Left + 50
            };

            cbMinOn = new ComboBoxMinute()
            {
                Top = cbTop,
                Left = I1.Left + 13
            };
            cbMinOn.DropDownClosed += CbMinOn_DropDownClosed;

            //
            izklop = new Label()
            {
                AutoSize = true,
                Top = lbltop,
                Text = "Izklop ob:",
                Left = leftOffset + 205,
            };

            cbHourOff = new ComboBoxHour()
            {
                Top = cbTop,
                Left = izklop.Left + 77
            };
            cbHourOff.DropDownClosed += CbHourOff_DropDownClosed;

            I2 = new Label()
            {
                AutoSize = true,
                Top = lbltop,
                Text = ":",
                Left = cbHourOff.Left + 63
            };            

            cbMinOff = new ComboBoxMinute()
            {
                Top = cbTop,
                Left = I2.Left + 13
            };
            cbMinOff.DropDownClosed += CbMinOff_DropDownClosed;

            Controls.Add(mon1); Controls.Add(tue1); 
            Controls.Add(wed1); Controls.Add(thu1);
            Controls.Add(fri1); Controls.Add(sat1); 
            Controls.Add(sun1);

            Controls.Add(vklop); Controls.Add(izklop);
            Controls.Add(I1); Controls.Add(I2);

            Controls.Add(cbHourOn); Controls.Add(cbHourOff);
            Controls.Add(cbMinOn); Controls.Add(cbMinOff);


            Width = 430;
            Height = 90;          
            //

        }

        bool IsItInitialized()
        {
            if (StartTime != null && EndTime != null && CurrentTime != null)
            {
                return true;
            }
            return false;
        }

        void initializeIfPossible()
        {
            if (IsItInitialized())
            {
                StartTime.ValueChanged += StartTime_ValueChanged;
                EndTime.ValueChanged += EndTime_ValueChanged;
                CurrentTime.ValueChanged += CurrentTime_ValueChanged;
            }            
        }

        private void CurrentTime_ValueChanged(object sender, EventArgs e)
        {
            UpdateControls();
        }

        private void EndTime_ValueChanged(object sender, EventArgs e)
        {
            UpdateControls();
        }

        private void StartTime_ValueChanged(object sender, EventArgs e)
        {
            UpdateControls();
        }

        private void CbMinOff_DropDownClosed(object sender, EventArgs e)
        {
            updateEndTime();
        }

        private void CbHourOff_DropDownClosed(object sender, EventArgs e)
        {
            updateEndTime();
        }

        private void CbMinOn_DropDownClosed(object sender, EventArgs e)
        {
            updateStartTime();            
        }

        private void CbHourOn_DropDownClosed(object sender, EventArgs e)
        {
            updateStartTime();
        }

     
        void updateEndTime()
        {
            var m = cbMinOff.SelectedItem.ToString();
            var mm = Convert.ToInt16(m);
            var h = cbHourOff.SelectedItem.ToString();
            var hh = Convert.ToInt16(h);

            var a = LogoTimeEncoder.Time.ConvertToShort(hh, mm);
            EndTime.Value_short = a;
  
        }
        void updateStartTime()
        {
            var m = cbMinOn.SelectedItem.ToString();
            var mm = Convert.ToInt16(m);
            var h = cbHourOn.SelectedItem.ToString();
            var hh = Convert.ToInt16(h);

            var a = LogoTimeEncoder.Time.ConvertToShort(hh, mm);
            StartTime.Value_short = a;
      
        }

        public void UpdateControls()
        {
            if (!IsItInitialized())
            {
                return;
            }

            if (InvokeRequired)
            {
                var m = new MethodInvoker(UpdateControls);
                Invoke(m);
                return;
            }
                UpdateValues();
                UpdateColors();             
        }

        void UpdateColors()
        {
            ChkBox a = mon1;
            a.BackColor = a.Checked ? checkedColor : uncheckedColor;

            a = tue1;
            a.BackColor = a.Checked ? checkedColor : uncheckedColor;

            a = wed1;
            a.BackColor = a.Checked ? checkedColor : uncheckedColor;

            a = thu1;
            a.BackColor = a.Checked ? checkedColor : uncheckedColor;

            a = fri1;
            a.BackColor = a.Checked ? checkedColor : uncheckedColor;

            a = sat1;
            a.BackColor = a.Checked ? checkedColor : uncheckedColor;

            a = sun1;
            a.BackColor = a.Checked ? checkedColor : uncheckedColor;

            var todaysDayOfTheWeek = CurrentTime.Datetime.DayOfWeek;

            var startTime = LogoTimeEncoder.Time.GetTimespan(StartTime);
            var endTime = LogoTimeEncoder.Time.GetTimespan(EndTime);
            var currentTime = CurrentTime.TodaysTime;
            bool active = currentTime >= startTime && currentTime < endTime;

            switch (todaysDayOfTheWeek)
            {
                case DayOfWeek.Sunday:
                    if (sun1.Checked && active)
                    {
                        sun1.BackColor = todaysColor;
                    }
                    break;
                case DayOfWeek.Monday:
                    if (mon1.Checked && active)
                    {
                        mon1.BackColor = todaysColor;
                    }
                    break;
                case DayOfWeek.Tuesday:
                    if (tue1.Checked && active)
                    {
                        tue1.BackColor = todaysColor;
                    }
                    break;
                case DayOfWeek.Wednesday:
                    if (wed1.Checked && active)
                    {
                        wed1.BackColor = todaysColor;
                    }
                    break;
                case DayOfWeek.Thursday:
                    if (thu1.Checked && active)
                    {
                        thu1.BackColor = todaysColor;
                    }
                    break;
                case DayOfWeek.Friday:
                    if (fri1.Checked && active)
                    {
                        fri1.BackColor = todaysColor;
                    }
                    break;
                case DayOfWeek.Saturday:
                    if (sat1.Checked && active)
                    {
                        sat1.BackColor = todaysColor;
                    }
                    break;
                default:
                    break;
            }
        }

        private void Sun1_Click(object sender, EventArgs e)
        {
            var a = (ChkBox)sender;
            var chkd = a.Checked;
            a.Checked = !chkd; // prevents visual update of the control (control is updated only when PLC value changes)
            if (chkd)
            {
                DayOfTheWeek.Value_byte += 1;                
            }
            else
            {
                DayOfTheWeek.Value_byte -= 1;               
            }
            UpdateColors();            
        }

        private void Sat1_Click(object sender, EventArgs e)
        {
            var a = (ChkBox)sender;
            var chkd = a.Checked;
            a.Checked = !chkd;
            if (chkd)
            {
                DayOfTheWeek.Value_byte += 64;
            }
            else
            {
                DayOfTheWeek.Value_byte -= 64;
            }
            UpdateColors();
            
        }

        private void Fri1_Click(object sender, EventArgs e)
        {
            var a = (ChkBox)sender;
            var chkd = a.Checked;
            a.Checked = !chkd;
            if (chkd)
            {
                DayOfTheWeek.Value_byte += 32;
            }
            else
            {
                DayOfTheWeek.Value_byte -= 32;
            }
            UpdateColors();
            
        }

        private void Thu1_Click(object sender, EventArgs e)
        {
            var a = (ChkBox)sender;
            var chkd = a.Checked;
            a.Checked = !chkd;
            if (chkd)
            {
                DayOfTheWeek.Value_byte += 16;
            }
            else
            {
                DayOfTheWeek.Value_byte -= 16;
            }
            UpdateColors();
           
        }

        private void Wed1_Click(object sender, EventArgs e)
        {
            var a = (ChkBox)sender;
            var chkd = a.Checked;
            a.Checked = !chkd;
            if (chkd)
            {
                DayOfTheWeek.Value_byte += 8;
            }
            else
            {
                DayOfTheWeek.Value_byte -= 8;
            }
            UpdateColors();
            
        }

        private void Tue1_Click(object sender, EventArgs e)
        {
            var a = (ChkBox)sender;
            var chkd = a.Checked;
            a.Checked = !chkd;
            if (chkd)
            {
                DayOfTheWeek.Value_byte += 4;
            }
            else
            {
                DayOfTheWeek.Value_byte -= 4;
            }
            UpdateColors();
           
        }

        private void Mon1_Click(object sender, EventArgs e)
        {
            var a = (ChkBox)sender;
            var chkd = a.Checked;
            a.Checked = !chkd;
            if (chkd)
            {
                DayOfTheWeek.Value_byte += 2;
            }
            else
            {
                DayOfTheWeek.Value_byte -= 2;                
            }
            UpdateColors();
            
        }

        private void UpdateValues()
        {      
            if (DayOfTheWeek != null)
            {                
                mon1.Checked = LogoTimeEncoder.WeekDay.IsItMonday(DayOfTheWeek);
                tue1.Checked = LogoTimeEncoder.WeekDay.IsItTuesday(DayOfTheWeek);
                wed1.Checked = LogoTimeEncoder.WeekDay.IsItWednesday(DayOfTheWeek);
                thu1.Checked = LogoTimeEncoder.WeekDay.IsItThursday(DayOfTheWeek);
                fri1.Checked = LogoTimeEncoder.WeekDay.IsItFriday(DayOfTheWeek);
                sat1.Checked = LogoTimeEncoder.WeekDay.IsItSaturday(DayOfTheWeek);
                sun1.Checked = LogoTimeEncoder.WeekDay.IsItSunday(DayOfTheWeek);                
            }
            else
            {
                throw new Exception("You must set DayOfTheWeek variable from outside the class, to be able to display and set values. Name of this control is: " + Name);
            }

            if (StartTime != null)
            {
                if (!cbHourOn.DroppedDown)
                {
                    cbHourOn.SelectedItem = LogoTimeEncoder.Time.GetHours(StartTime).ToString("00");
                }
                if (!cbMinOn.DroppedDown)
                {
                    cbMinOn.SelectedItem = LogoTimeEncoder.Time.GetMinutes(StartTime).ToString("00");
                }
            }
            else
            {
                throw new Exception("You must set StartTime variable from outside the class, to be able to display and set values.");
            }

            if (EndTime != null)
            {
                if (!cbHourOff.DroppedDown)
                {
                    cbHourOff.SelectedItem = LogoTimeEncoder.Time.GetHours(EndTime).ToString("00");
                }
                if (!cbMinOff.DroppedDown)
                {
                    cbMinOff.SelectedItem = LogoTimeEncoder.Time.GetMinutes(EndTime).ToString("00");
                }
            }
            else
            {
                throw new Exception("You must set EndTime variable from outside the class, to be able to display and set values.");
            }
        }

        class ChkBox : CheckBox
        {
            public ChkBox()
            {
                AutoSize = true;
            }
        }

        class ComboBoxHour : ComboBox
        {
            public ComboBoxHour()
            {
                Width = 45;
                populate();
                DropDownStyle = ComboBoxStyle.DropDownList;
            }

            void populate()
            {
                int hour = 0;

                for (int i = 0; i <= 23; i++)
                {                    
                    Items.Add(hour.ToString("00"));
                    hour = hour + 1;
                }
            }
        }

        class ComboBoxMinute : ComboBox
        {
            public ComboBoxMinute()
            {
                Width = 45;
                populate();
                DropDownStyle = ComboBoxStyle.DropDownList;
            }
            void populate()
            {
                int minute = 0;

                for (int i = 0; i < 6; i++)
                {                    
                    Items.Add(minute.ToString("00"));
                    minute = minute + 1 * 10;
                }
                Items.Add("59");
            }
        }        
    }

    public class LogoTimeEncoder
    {
        public class WeekDay
        {
            public static bool IsItMonday(PlcVars.Byte weekday)
            {
                var a = weekday.Value_byte;


                if (a >= 64) // saturday
                {
                    a -= 64;
                }
                if (a >= 32) // friday
                {
                    a -= 32;
                }
                if (a >= 16) // thursday
                {
                    a -= 16;
                }
                if (a >= 8) // wednesday
                {
                    a -= 8;
                }
                if (a >= 4) // tuesday
                {
                    a -= 4;
                }
                if (a >= 2) // monday
                {
                    a -= 2;
                    return true;
                }

                return false;

            }

            public static bool IsItTuesday(PlcVars.Byte weekday)
            {
                var a = weekday.Value_byte;


                if (a >= 64) // saturday
                {
                    a -= 64;
                }
                if (a >= 32) // friday
                {
                    a -= 32;
                }
                if (a >= 16) // thursday
                {
                    a -= 16;
                }
                if (a >= 8) // wednesday
                {
                    a -= 8;
                }
                if (a >= 4) // tuesday
                {
                    a -= 4;
                    return true;
                }

                return false;

            }

            public static bool IsItWednesday(PlcVars.Byte weekday)
            {
                var a = weekday.Value_byte;


                if (a >= 64) // saturday
                {
                    a -= 64;
                }
                if (a >= 32) // friday
                {
                    a -= 32;
                }
                if (a >= 16) // thursday
                {
                    a -= 16;
                }
                if (a >= 8) // wednesday
                {
                    a -= 8;
                    return true;
                }

                return false;

            }

            public static bool IsItThursday(PlcVars.Byte weekday)
            {
                var a = weekday.Value_byte;


                if (a >= 64) // saturday
                {
                    a -= 64;
                }
                if (a >= 32) // friday
                {
                    a -= 32;
                }
                if (a >= 16) // thursday
                {
                    a -= 16;
                    return true;
                }

                return false;

            }

            public static bool IsItFriday(PlcVars.Byte weekday)
            {
                var a = weekday.Value_byte;


                if (a >= 64) // saturday
                {
                    a -= 64;
                }
                if (a >= 32) // friday
                {
                    a -= 32;
                    return true;
                }

                return false;

            }

            public static bool IsItSaturday(PlcVars.Byte weekday)
            {
                var a = weekday.Value_byte;


                if (a >= 64) // saturday
                {
                    a -= 64;
                    return true;
                }

                return false;

            }

            public static bool IsItSunday(PlcVars.Byte weekday)
            {
                var a = weekday.Value_byte;


                if (a >= 64) // saturday
                {
                    a -= 64;
                }
                if (a >= 32) // friday
                {
                    a -= 32;
                }
                if (a >= 16) // thursday
                {
                    a -= 16;
                }
                if (a >= 8) // wednesday
                {
                    a -= 8;
                }
                if (a >= 4) // tuesday
                {
                    a -= 4;
                }
                if (a >= 2) // monday
                {
                    a -= 2;
                }
                if (a >= 1) // sunday
                {
                    a -= 1;
                    return true;
                }

                return false;

            }
        }

        public class Time
        {
            public static int GetMinutes(PlcVars.Word time)
            {
                if (time != null)
                {
                    int t = time.Value_short;
                    if (t < 0)
                    {
                        return 0;
                    }
                    int tenhours = t / 4096;
                    t = t - tenhours * 4096;
                    int hours = t / 256;
                    t = t - hours * 256;
                    int tenminutes = t / 16;
                    t = t - tenminutes * 16;
                    int minutes = t;

                    int trueHours = tenhours * 10 + hours;
                    int trueMinutes = tenminutes * 10 + minutes;

                    return trueMinutes;
                }

                return 0;
                
            }

            public static int GetHours(PlcVars.Word time)
            {
                if (time != null)
                {
                    int t = time.Value_short;
                    if (t<0)
                    {
                        return 0;
                    }
                    int tenhours = t / 4096;
                    t = t - tenhours * 4096;
                    int hours = t / 256;

                    int trueHours = tenhours * 10 + hours;

                    return trueHours;
                }

                return 0;
            }

            public static TimeSpan GetTimespan(PlcVars.Word time)
            {
                var h = GetHours(time);
                var m = GetMinutes(time);
                TimeSpan t = new TimeSpan(h, m, 0);
                
                return t;
            }

            public static short ConvertToShort(int hours, int minutes)
            {
                if (hours <= 0)
                {
                    hours = 0;
                }
                if (hours > 23)
                {
                    hours = 23;
                }
                if (minutes < 0)
                {
                    minutes = 0;
                }
                if (minutes > 59)
                {
                    minutes = 59;
                }

                var ctenhours = hours / 10;
                var chours = hours - ctenhours * 10;
                var ctenminutes = minutes / 10;
                var cminutes = minutes - ctenminutes * 10;

                var buff = cminutes + ctenminutes * 16 + chours * 256 + ctenhours * 4096;

                return (short)buff;

            }
        }
    }

    public class OneDaysEvent
    {
        public CustomDayOfWeek DayOfWeek { get; private set; }
        public TimeSpan EventTime { get; private set; }
        public bool CurrentlyActive { get; private set; } // true = Event is starting, false = Event is ending
        public TimeSpan TimeUntilEvent { get; private set; }

        // Constructor
        public OneDaysEvent(PlcVars.LogoDateTime CurrentTimeOnPlc, CustomDayOfWeek dayOfWeek, TimeSpan eventTime, bool currentlyActive)
        {
            DayOfWeek = dayOfWeek;
            EventTime = eventTime;
            CurrentlyActive = currentlyActive;            

            var currentDayOfWeek = (CustomDayOfWeek)((int)CurrentTimeOnPlc.Datetime.DayOfWeek + 1); // +1 to align with your enum
            int daysUntilEvent = (int)DayOfWeek - (int)currentDayOfWeek;
            bool isPastEvent = false;

            if (daysUntilEvent == 0)
            {
                if (CurrentTimeOnPlc.TodaysTime > EventTime)
                {
                    isPastEvent = true;
                }
            }

            // Adjust for rollover
            if (daysUntilEvent < 0 || isPastEvent)
            {
                daysUntilEvent += 7; // Add whole week
            }

            TimeUntilEvent = TimeSpan.FromDays(daysUntilEvent) + EventTime;
        }

        public bool IsEventTodayAndActive(TimeSpan currentTime)
        {
            var today = DateTime.Now.DayOfWeek;
            CustomDayOfWeek todayCustomDay = (CustomDayOfWeek)((int)today + 1); // +1 to align with enum where 1 is Sunday

            if (todayCustomDay == DayOfWeek)
            {
                if (CurrentlyActive && currentTime >= EventTime) // Event is starting and current time is past the start time
                {
                    return true;
                }
                else if (!CurrentlyActive && currentTime <= EventTime) // Event is ending and current time is before the end time
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsEventTodayAndOver(TimeSpan currentTime)
        {
            var today = DateTime.Now.DayOfWeek;
            CustomDayOfWeek todayCustomDay = (CustomDayOfWeek)((int)today + 1); // +1 to align with enum where 1 is Sunday

            if (todayCustomDay == DayOfWeek && !CurrentlyActive && currentTime > EventTime) // Event is over today
            {
                return true;
            }

            return false;
        }

        public bool IsEventUpcoming(TimeSpan currentTime)
        {
            var today = DateTime.Now.DayOfWeek;
            CustomDayOfWeek todayCustomDay = (CustomDayOfWeek)((int)today + 1); // +1 to align with enum where 1 is Sunday

            if (todayCustomDay == DayOfWeek && CurrentlyActive && currentTime < EventTime) // Event is starting soon
            {
                return true;
            }

            return false;
        }

        
    }

    public enum CustomDayOfWeek
    {
        Izključen = 0,
        Nedelja = 1,
        Ponedeljek = 2,
        Torek = 3,
        Sreda = 4,
        Četrtek = 5,
        Petek = 6,
        Sobota = 7
    }

}
