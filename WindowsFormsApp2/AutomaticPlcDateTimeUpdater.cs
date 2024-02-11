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
            if (DoesPlcClockNeedsCorrection(p1))
            {
                return false;
            }

            return true;
            // TODO the rest of PLCs
        }

        bool DoesPlcClockNeedsCorrection(PropComm prop)
        {
            DateTime plcTime;
            DateTime pcTime = DateTime.Now;

            if (prop.GetType() == typeof(Prop1))
            {
                var isConnected = LogoControler.LOGOConnection[1].connectionStatusLOGO;
                if (isConnected != Connection.Status.Connected)
                {
                    return false;
                }

                plcTime = p1.LogoDatetime.Value_Datetime; 

                if (Math.Abs((plcTime - pcTime).TotalMinutes) > 1)
                {
                    return true;
                }             
            }

            else if (prop.GetType() == typeof(Prop2))
            {
                var isConnected = LogoControler.LOGOConnection[2].connectionStatusLOGO;
                if (isConnected != Connection.Status.Connected)
                {
                    return false;
                }

                plcTime = p2.LogoDatetime.Value_Datetime;              

                if (Math.Abs((plcTime - pcTime).TotalMinutes) > 1)
                {
                    return true;
                }                
            }

            else if (prop.GetType() == typeof(Prop3))
            {
                var isConnected = LogoControler.LOGOConnection[3].connectionStatusLOGO;
                if (isConnected != Connection.Status.Connected)
                {
                    return false;
                }

                 plcTime = p3.LogoDatetime.Value_Datetime;

                if (Math.Abs((plcTime - pcTime).TotalMinutes) > 1)
                {
                    return true;
                }                
            }

            else if (prop.GetType() == typeof(Prop4))
            {
                var isConnected = LogoControler.LOGOConnection[4].connectionStatusLOGO;
                if (isConnected != Connection.Status.Connected)
                {
                    return false;
                }

                 plcTime = p4.LogoDatetime.Value_Datetime;

                if (Math.Abs((plcTime - pcTime).TotalMinutes) > 1)
                {
                    return true;
                }               
            }

            else if (prop.GetType() == typeof(Prop5))
            {
                var isConnected = LogoControler.LOGOConnection[5].connectionStatusLOGO;
                if (isConnected != Connection.Status.Connected)
                {
                    return false;
                }

                 plcTime = p5.LogoDatetime.Value_Datetime;

                if (Math.Abs((plcTime - pcTime).TotalMinutes) > 1)
                {
                    return true;
                }                
            }

            else if (prop.GetType() == typeof(Prop6))
            {
                var isConnected = LogoControler.LOGOConnection[6].connectionStatusLOGO;
                if (isConnected != Connection.Status.Connected)
                {
                    return false;
                }

                plcTime = p6.LogoDatetime.Value_Datetime;

                if (Math.Abs((plcTime - pcTime).TotalMinutes) > 1)
                {
                    return true;
                }               
            }

            else if (prop.GetType() == typeof(Prop7))
            {
                var isConnected = LogoControler.LOGOConnection[7].connectionStatusLOGO;
                if (isConnected != Connection.Status.Connected)
                {
                    return false;
                }

                plcTime = p7.LogoDatetime.Value_Datetime;

                if (Math.Abs((plcTime - pcTime).TotalMinutes) > 1)
                {
                    return true;
                }               
            }

            else if (prop.GetType() == typeof(Prop8))
            {
                var isConnected = LogoControler.LOGOConnection[8].connectionStatusLOGO;
                if (isConnected != Connection.Status.Connected)
                {
                    return false;
                }

                plcTime = p8.LogoDatetime.Value_Datetime;

                if (Math.Abs((plcTime - pcTime).TotalMinutes) > 1)
                {
                    return true;
                }               
            }            
            return false;
        }

        public static void UpdateAll()
        {
            var now = DateTime.Now;
            Val.logocontroler.Prop1.LogoDatetime.Value_Datetime = now;
            Val.logocontroler.Prop2.LogoDatetime.Value_Datetime = now;
            Val.logocontroler.Prop3.LogoDatetime.Value_Datetime = now;
            Val.logocontroler.Prop4.LogoDatetime.Value_Datetime = now;
            Val.logocontroler.Prop5.LogoDatetime.Value_Datetime = now;
            Val.logocontroler.Prop6.LogoDatetime.Value_Datetime = now;
            Val.logocontroler.Prop7.LogoDatetime.Value_Datetime = now;
            Val.logocontroler.Prop8.LogoDatetime.Value_Datetime = now;
          
        }

    }
}
