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
            //
            var cb1 = (ConnectedButton)findControl("connectedButton1");
            cb1.ID = 1;

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
