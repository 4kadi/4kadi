using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

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

            #region LOGO1
            
            var kad1 = (EnojnaKad)findControl("enojnaKad1");
         

            var cb1 = (ConnectedButton)findControl("connectedButton1");
            cb1.ID = 1;
            cb1.Text = "LOGO1";

            var stopWatch1 = (StopWatch)findControl("stopWatch1");
            var p = Val.logocontroler.Prop1;

            stopWatch1.CurrentTime = p.stopwatchCurrentTime;
            stopWatch1.ReminderTime = p.stopwatchReminder;
            stopWatch1.PauseTime = p.stopwatchPauseTime;
            stopWatch1.TimeLeft = p.stopwatchTimeLeft;
            stopWatch1.PauseLeft = p.stopwatchPauseLeft;
            stopWatch1.Finished = p.stopwatchFinished;
            stopWatch1.Paused = p.stopwatchPaused;
            stopWatch1.InProgress = p.stopwatchInProgress;
            stopWatch1.Prisotnost = p.Prisotnost_CurrentState1;
            stopWatch1.UpostevajPrisotnost = p.AutostartStopwatch;
            stopWatch1.BtnReset = p.stopwatchResetPulse;
            stopWatch1.BtnStart = p.stopwatchStartPulse;
            stopWatch1.BtnStop = p.stopwatchStopPulse;

            var enojnaKad1 = (Kad)findControl("enojnaKad1");
            enojnaKad1.NameOfKad = p.ImeKadi1;

            enojnaKad1.Urnik.UrnikAktiven = p.UrnikAktiven;
            enojnaKad1.Urnik.currentTime = p.LogoDatetime;
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
            enojnaKad1.Urnik.NapakaNastTemperatur = p.NapakaNastavitevTemperatur;
            enojnaKad1.Urnik.TemperaturaAktivnegaUrnika = p.temperaturaAktivnegaUrnika;
            enojnaKad1.Urnik.TemperaturaNektivnegaUrnika = p.temperaturaNeaktivnegaUrnika;

            enojnaKad1.SwitchesGroupbox.Prisotnost.Value_SwitchSetting = p.Prisotnost_AutoManSwitch1;
            enojnaKad1.SwitchesGroupbox.Prisotnost.Value_CurrentState_PLC = p.Prisotnost_CurrentState1;

            enojnaKad1.SwitchesGroupbox.Grelec1.Value_SwitchSetting = p.Grelec1_AutoManSwitch;
            enojnaKad1.SwitchesGroupbox.Grelec1.Value_CurrentState_PLC = p.Grelec1_CurrentState;

            enojnaKad1.SwitchesGroupbox.Grelec2.Value_SwitchSetting = p.Grelec2_AutoManSwitch;
            enojnaKad1.SwitchesGroupbox.Grelec2.Value_CurrentState_PLC = p.Grelec2_CurrentState;

            enojnaKad1.SwitchesGroupbox.ČrpalkaNivo.Value_SwitchSetting = p.ČrpalkaNivo_AutoManSwitch;
            enojnaKad1.SwitchesGroupbox.ČrpalkaNivo.Value_CurrentState_PLC = p.ČrpalkaNivo_CurrentState;

            enojnaKad1.SwitchesGroupbox.ČrpalkaFilter.Value_SwitchSetting = p.ČrpalkaFilter_AutoManSwitch;
            enojnaKad1.SwitchesGroupbox.ČrpalkaFilter.Value_CurrentState_PLC = p.ČrpalkaFilter_CurrentState;

            enojnaKad1.SwitchesGroupbox.ČrpalkaČasovnoDolivanje.Value_SwitchSetting = p.ČrpalkaČasovnoDolivanje_AutoManSwitch;
            enojnaKad1.SwitchesGroupbox.ČrpalkaČasovnoDolivanje.Value_CurrentState_PLC = p.ČrpalkaČasovnoDolivanje_CurrentState;

            enojnaKad1.SwitchesGroupbox.ČrpalkaHlajenje.Value_SwitchSetting = p.ČrpalkaHlajenje_AutoManSwitch;
            enojnaKad1.SwitchesGroupbox.ČrpalkaHlajenje.Value_CurrentState_PLC = p.ČrpalkaHlajenje_CurrentState; // TODO this is null

            enojnaKad1.SwitchesGroupbox.TlačniSezorFiltra.Value_SwitchSetting = p.TlačniSezorFiltra_AutoManSwitch;
            enojnaKad1.SwitchesGroupbox.TlačniSezorFiltra.Value_CurrentState_PLC = p.TlačniSezorFiltra_CurrentState;

            enojnaKad1.SwitchesGroupbox.MixVentilPlus.Value_SwitchSetting = p.MixVentilPlus_AutoManSwitch;
            enojnaKad1.SwitchesGroupbox.MixVentilPlus.Value_CurrentState_PLC = p.MixVentilPlus_CurrentState;

            enojnaKad1.SwitchesGroupbox.MixVentilMinus.Value_SwitchSetting = p.MixVentilMinus_AutoManSwitch;
            enojnaKad1.SwitchesGroupbox.MixVentilMinus.Value_CurrentState_PLC = p.MixVentilMinus_CurrentState;

            enojnaKad1.SwitchesGroupbox.NivoVisok.Value_SwitchSetting = p.NivoVisok_AutoManSwitch;
            enojnaKad1.SwitchesGroupbox.NivoVisok.Value_CurrentState_PLC = p.NivoVisok_CurrentState;

            enojnaKad1.SwitchesGroupbox.NivoNizek.Value_SwitchSetting = p.NivoNizek_AutoManSwitch;
            enojnaKad1.SwitchesGroupbox.NivoNizek.Value_CurrentState_PLC = p.AlarmNivoNizek_CurrentState;

            enojnaKad1.SwitchesGroupbox.VentilZrak.Value_SwitchSetting = p.VentilZrak_AutoManSwitch;
            enojnaKad1.SwitchesGroupbox.VentilZrak.Value_CurrentState_PLC = p.VentilZrak_CurrentState;

            enojnaKad1.SwitchesGroupbox.VentilPokrov.Value_SwitchSetting = p.VentilPokrov_AutoManSwitch;
            enojnaKad1.SwitchesGroupbox.VentilPokrov.Value_CurrentState_PLC = p.VentilPokrov_CurrentState;

            enojnaKad1.SwitchesGroupbox.VentilVoda.Value_SwitchSetting = p.VentilVoda_AutoManSwitch;
            enojnaKad1.SwitchesGroupbox.VentilVoda.Value_CurrentState_PLC = p.VentilVoda_CurrentState;

            enojnaKad1.SwitchesGroupbox.FlowSwitch.Value_SwitchSetting = p.FlowSwitch_AutoManSwitch;
            enojnaKad1.SwitchesGroupbox.FlowSwitch.Value_CurrentState_PLC = p.FlowSwitch_CurrentState;

            enojnaKad1.PowerSetGroupbox.PowerSetGrelnik1.ValuePowerset = p.Grelec1_Moc;
            enojnaKad1.PowerSetGroupbox.PowerSetGrelnik2.ValuePowerset = p.Grelec2_Moc;
            enojnaKad1.PowerSetGroupbox.PowerSetCrpalkaNivo.ValuePowerset = p.ČrpalkaNivo_Moc;
            enojnaKad1.PowerSetGroupbox.PowerSetCrpalkaFilter.ValuePowerset = p.ČrpalkaFilter_Moc;
            enojnaKad1.PowerSetGroupbox.PowerSetCrpalkaCasovna.ValuePowerset = p.ČrpalkaČasovnoDolivanje_Moc;
            enojnaKad1.PowerSetGroupbox.PowerSetCrpalkaHlajenje.ValuePowerset = p.ČrpalkaHlajenje_Moc;
            enojnaKad1.PowerSetGroupbox.PowerSetRezerva.ValuePowerset = p.Rezerva_Moc;
            enojnaKad1.PowerMonitorGroupbox.PorabaSkupna.ValuePowerset = p.PorabaCelotnegaSistema;
            enojnaKad1.PowerMonitorGroupbox.PorabaTeKadi.ValuePowerset = p.TrenutnaKad_Poraba;
            enojnaKad1.PowerMonitorGroupbox.OmejitevPorabaSkupna.ValuePowerset = p.OmejitevPorabeCelotnegaSistema;

            enojnaKad1.PvSelector.T1 = p.Temperatura1;
            enojnaKad1.PvSelector.T2 = p.Temperatura2;
            enojnaKad1.PvSelector.PV = p.ProcesnaTemperatura;
            enojnaKad1.PvSelector.Function = p.IzbiraProcesneTemperature;
            enojnaKad1.PvSelector.SenFail_T1 = p.SenFailTemperatura1;
            enojnaKad1.PvSelector.SenFail_T2 = p.SenFailTemperatura2;

            enojnaKad1.Ph.PhReading = p.PhReading;
            enojnaKad1.Ph.PhSetPoint = p.PhSetpoint;
            enojnaKad1.Ph.SenErr = p.SenFailPh;

            #endregion
            
            var cb2 = (ConnectedButton)findControl("connectedButton2");
            cb2.ID = 2;
            cb2.Text = "LOGO2";

            var cb3 = (ConnectedButton)findControl("connectedButton3");
            cb3.ID = 3;
            cb3.Text = "LOGO3";

            var cb4 = (ConnectedButton)findControl("connectedButton4");
            cb4.ID = 4;
            cb4.Text = "LOGO4";

            var cb5 = (ConnectedButton)findControl("connectedButton5");
            cb5.ID = 5;
            cb5.Text = "LOGO5";

            var cb6 = (ConnectedButton)findControl("connectedButton6");
            cb6.ID = 6;
            cb6.Text = "LOGO6";

            var cb7 = (ConnectedButton)findControl("connectedButton7");
            cb7.ID = 7;
            cb7.Text = "LOGO7";

            var cb8 = (ConnectedButton)findControl("connectedButton8");
            cb8.ID = 8;
            cb8.Text = "LOGO8";

            
        }

        Control findControl(string name)
        {
            return Helper.findControl(name, form);
        }
    }
}
