using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KontrolaKadi
{
    class GuiLink
    {
        Thread t;
        Gui_MalaPec form;

        public GuiLink(Gui_MalaPec form)
        {
            this.form = form;
            t = new Thread(thread);
            t.Start();
        }



        void thread()
        {
            var warnmng = (WarningManager)findControl("warningManager1");    

            //
            var kad1 = (EnojnaKad)findControl("enojnaKad1");
         

            var cb1 = (ConnectedButton)findControl("connectedButton1");

            var stopWatch1 = (StopWatch)findControl("stopWatch1");
            var p = Val.logocontroler.Prop1;

            stopWatch1.CurrentTime = p.stopwatchCurrentTime;
            stopWatch1.ReminderTime = p.stopwatchReminder;
            stopWatch1.PauseTime = p.stopwatchPauseTime;
            stopWatch1.TimeLeft = p.stopwatchTimeLeft;
            stopWatch1.Finished = p.stopwatchFinished;
            stopWatch1.Paused = p.stopwatchPaused;
            stopWatch1.Prisotnost = p.prisotnost;
            stopWatch1.Autostart = p.AutostartStopwatch;

            var enojnaKad1 = (Kad)findControl("enojnaKad1");
            enojnaKad1.Urnik.UrnikAktiven = p.UrnikAktiven;
            enojnaKad1.Urnik.currentTime = p.LogoClock;
            enojnaKad1.Urnik.DayOfTheWeek1 = p.weekday1;
            enojnaKad1.Urnik.DayOfTheWeek2 = p.weekday2;
            enojnaKad1.Urnik.DayOfTheWeek3 = p.weekday3;
            enojnaKad1.Urnik.StartTime1 = p.ontime1;
            enojnaKad1.Urnik.StartTime2 = p.ontime2;
            enojnaKad1.Urnik.StartTime3 = p.ontime3;
            enojnaKad1.Urnik.EndTime1 = p.offtime1;
            enojnaKad1.Urnik.EndTime2 = p.offtime2;
            enojnaKad1.Urnik.EndTime3 = p.offtime3;
            enojnaKad1.Urnik.DayOfTheWeek4 = p.weekday4;
            enojnaKad1.Urnik.DayOfTheWeek5 = p.weekday5;
            enojnaKad1.Urnik.DayOfTheWeek6 = p.weekday6;
            enojnaKad1.Urnik.StartTime4 = p.ontime4;
            enojnaKad1.Urnik.StartTime5 = p.ontime5;
            enojnaKad1.Urnik.StartTime6 = p.ontime6;
            enojnaKad1.Urnik.EndTime4 = p.offtime4;
            enojnaKad1.Urnik.EndTime5 = p.offtime5;
            enojnaKad1.Urnik.EndTime6 = p.offtime6;

        }

        Control findControl(string name)
        {
            try
            {
                var arr = form.Controls.Find(name, true);
                var rtrn = arr[0];
                return rtrn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
