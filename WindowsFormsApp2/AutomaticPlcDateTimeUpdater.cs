using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Timers;
using System.Windows.Forms;

namespace KontrolaKadi
{
    public class AutomaticPlcDateTimeUpdater
    {
        System.Timers.Timer DatetimeChecker_Timer;
        Prop1 p1;
        Prop2 p2;
        Prop3 p3;
        Prop4 p4;
        Prop5 p5;
        Prop6 p6;
        Prop7 p7;
        Prop8 p8;

        public AutomaticPlcDateTimeUpdater()
        {
            DatetimeChecker_Timer = new System.Timers.Timer(5000) { 
                AutoReset = false
            };
            DatetimeChecker_Timer.Start();
            DatetimeChecker_Timer.Elapsed += DatetimeChecker;

        }

        void DatetimeChecker(object sender, ElapsedEventArgs e)
        {
            Initialize();

            if (!areAllPlcClocksCorrect())
            {
                MessageBox.Show("Čas na krmilnikih odstopa od časa na PC. Nastavite pravilen čas na PC, nato kliknite OK. Sistem bo nato posodobil čas na krmilnikih.");
                UpdateAll();
            }
            SubscribeNext();
        }

        void SubscribeNext()
        {
            DatetimeChecker_Timer.Interval = 60000; // Check logo clock once per hour
            DatetimeChecker_Timer.Start();
        }

        void Initialize()
        {
            if (p8 == null)
            {
                p1 = Val.logocontroler.Prop1;
                p2 = Val.logocontroler.Prop2;
                p3 = Val.logocontroler.Prop3;
                p4 = Val.logocontroler.Prop4;
                p5 = Val.logocontroler.Prop5;
                p6 = Val.logocontroler.Prop6;
                p7 = Val.logocontroler.Prop7;
                p8 = Val.logocontroler.Prop8;
            }            
        }

        bool areAllPlcClocksCorrect()
        {
            if (!isClockCorrect(p1))
            {
                return false;
            }

            return true;
            // TODO the rest of PLCs
        }

        bool isClockCorrect(PropComm prop)
        {
            if (prop.GetType() == typeof(Prop1))
            {
                var plcTime = p1.LogoDatetime.Value_Datetime;
                var pcTime = DateTime.Now;

                if (Math.Abs((plcTime - pcTime).TotalMinutes) > 1)
                {
                    return false;
                }
                return true;
            }
            // TODO the rest of PLCs
            return true;
        }

        public static void UpdateAll()
        {
            Val.logocontroler.Prop1.LogoDatetime.Value_Datetime = DateTime.Now;
            // TODO the rest of PLCs
        }
    }
}
