using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;

namespace KontrolaKadi
{
    public class Prop1 : PropComm
    {
        //PC Watchdog
        public PlcVars.Word PCWD;

        public PlcVars.Bit AnyAlarm;

        public PlcVars.DWord stopwatchCurrentTime;
        public PlcVars.Word stopwatchTimeLeft, stopwatchPauseLeft;
        public PlcVars.DWord stopwatchReminder, stopwatchPauseTime;

        public PlcVars.Bit stopwatchStartPulse, stopwatchStopPulse, stopwatchResetPulse, stopwatchFinished, stopwatchPaused, stopwatchInProgress;
     
        public PlcVars.Word AutostartStopwatch;

        public PlcVars.Byte weekday1, weekday2, weekday3, weekday4, weekday5, weekday6;        
        public PlcVars.Word ontime1, ontime2, ontime3, ontime4, ontime5, ontime6;
        public PlcVars.Word offtime1, offtime2, offtime3, offtime4, offtime5, offtime6;
        public PlcVars.Bit UrnikAktiven;

        public PlcVars.AlarmBit NapakaNastavitevTemperatur;
        public PlcVars.Word temperaturaAktivnegaUrnika, temperaturaNeaktivnegaUrnika;

        public PlcVars.Word Prisotnost_AutoManSwitch;
        public PlcVars.Bit Prisotnost_CurrentState;

        public PlcVars.Word Grelec1_AutoManSwitch, Grelec2_AutoManSwitch;
        public PlcVars.Bit Grelec1_CurrentState, Grelec2_CurrentState;


        public PlcVars.Word ČrpalkaNivo_AutoManSwitch;
        public PlcVars.Bit ČrpalkaNivo_CurrentState;

        public PlcVars.Word ČrpalkaFilter_AutoManSwitch;
        public PlcVars.Bit ČrpalkaFilter_CurrentState;

        public PlcVars.Word ČrpalkaČasovnoDolivanje_AutoManSwitch;
        public PlcVars.Bit ČrpalkaČasovnoDolivanje_CurrentState;

        public PlcVars.Word ČrpalkaHlajenje_AutoManSwitch;
        public PlcVars.Bit ČrpalkaHlajenje_CurrentState;

        public PlcVars.Word TlačniSezorFiltra_AutoManSwitch;
        public PlcVars.AlarmBit TlačniSezorFiltra_CurrentState;

        public PlcVars.Word MixVentilPlus_AutoManSwitch;
        public PlcVars.Bit MixVentilPlus_CurrentState;

        public PlcVars.Word MixVentilMinus_AutoManSwitch;
        public PlcVars.Bit MixVentilMinus_CurrentState;

        public PlcVars.Word NivoVisok_AutoManSwitch;
        public PlcVars.AlarmBit NivoVisok_CurrentState;

        public PlcVars.Word NivoNizek_AutoManSwitch;
        public PlcVars.Bit AlarmNivoNizek_CurrentState;

        public PlcVars.Word VentilZrak_AutoManSwitch;
        public PlcVars.Bit VentilZrak_CurrentState;

        public PlcVars.Word VentilPokrov_AutoManSwitch;
        public PlcVars.Bit VentilPokrov_CurrentState;

        public PlcVars.Word VentilVoda_AutoManSwitch;
        public PlcVars.Bit VentilVoda_CurrentState;

        public PlcVars.Word FlowSwitch_AutoManSwitch;
        public PlcVars.Bit FlowSwitch_CurrentState;

        public PlcVars.Word PrejšnjaKad_Poraba, Grelec1_Moc, Grelec2_Moc, ČrpalkaNivo_Moc, ČrpalkaFilter_Moc, ČrpalkaČasovnoDolivanje_Moc, ČrpalkaHlajenje_Moc, Rezerva_Moc, TrenutnaKad_Poraba, SestevekPorabe;
        public PlcVars.Word OmejitevPorabeCelotnegaSistema, PorabaCelotnegaSistema;

        public PlcVars.Word IzbiraProcesneTemperature, Temperatura1, Temperatura2, ProcesnaTemperatura, TemperaturaHr, PhReading, Prevodnost, Rezerva1, Rezerva2, Rezerva3;

        public PlcVars.AlarmBit SenFailTemperatura1, SenFailTemperatura2, SenFailTemperaturaHr, SenFailPh, SenFailPrevodnost, SenFailRezerva1, SenFailRezerva2, SenFailRezerva3;

        public PlcVars.Word PhSetpoint;

        public Prop1(Sharp7.S7Client client) : base(client)
        {
            
            PCWD = new PlcVars.Word(this, new PlcVars.WordAddress(XmlController.GetPCWD_Address()), false); //PC Watchdog
            AnyAlarm = new PlcVars.Bit(this, new PlcVars.BitAddress(9, 0), false);

            stopwatchCurrentTime = new PlcVars.DWord(this, new PlcVars.DoubleWordAddress(10), false);
            stopwatchReminder = new PlcVars.DWord(this, new PlcVars.DoubleWordAddress(14), true);
            stopwatchPauseTime = new PlcVars.DWord(this, new PlcVars.DoubleWordAddress(18), true);
            AutostartStopwatch = new PlcVars.Word(this, new PlcVars.WordAddress(22), true);
            stopwatchTimeLeft = new PlcVars.Word(this, new PlcVars.WordAddress(24), false);
            stopwatchPauseLeft = new PlcVars.Word(this, new PlcVars.WordAddress(26), false);

            stopwatchStartPulse = new PlcVars.Bit(this, new PlcVars.BitAddress(28, 0), true);
            stopwatchStopPulse = new PlcVars.Bit(this, new PlcVars.BitAddress(29, 0), true);
            stopwatchResetPulse = new PlcVars.Bit(this, new PlcVars.BitAddress(30,0), true);
            stopwatchFinished = new PlcVars.Bit(this, new PlcVars.BitAddress(31, 0), false);
            stopwatchPaused = new PlcVars.Bit(this, new PlcVars.BitAddress(31, 1), false);
            stopwatchInProgress = new PlcVars.Bit(this, new PlcVars.BitAddress(31, 2), false);

            weekday1 = new PlcVars.Byte(this, new PlcVars.ByteAddress(40), true);
            ontime1 = new PlcVars.Word(this, new PlcVars.WordAddress(41), true);
            offtime1 = new PlcVars.Word(this, new PlcVars.WordAddress(43), true);
            weekday2 = new PlcVars.Byte(this, new PlcVars.ByteAddress(45), true);
            ontime2 = new PlcVars.Word(this, new PlcVars.WordAddress(46), true);
            offtime2 = new PlcVars.Word(this, new PlcVars.WordAddress(48), true);
            weekday3 = new PlcVars.Byte(this, new PlcVars.ByteAddress(50), true);
            ontime3 = new PlcVars.Word(this, new PlcVars.WordAddress(51), true);
            offtime3 = new PlcVars.Word(this, new PlcVars.WordAddress(53), true);
            weekday4 = new PlcVars.Byte(this, new PlcVars.ByteAddress(55), true);
            ontime4 = new PlcVars.Word(this, new PlcVars.WordAddress(56), true);
            offtime4 = new PlcVars.Word(this, new PlcVars.WordAddress(58), true);
            weekday5 = new PlcVars.Byte(this, new PlcVars.ByteAddress(60), true);
            ontime5 = new PlcVars.Word(this, new PlcVars.WordAddress(61), true);
            offtime5 = new PlcVars.Word(this, new PlcVars.WordAddress(63), true);
            weekday6 = new PlcVars.Byte(this, new PlcVars.ByteAddress(65), true);
            ontime6 = new PlcVars.Word(this, new PlcVars.WordAddress(66), true);
            offtime6 = new PlcVars.Word(this, new PlcVars.WordAddress(68), true);
            UrnikAktiven = new PlcVars.Bit(this, new PlcVars.BitAddress(70, 0), true);

            NapakaNastavitevTemperatur = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(71, 0), "Napaka nastavitev željenih temperatur");
            temperaturaAktivnegaUrnika = new PlcVars.Word(this, new PlcVars.WordAddress(72), true);
            temperaturaNeaktivnegaUrnika = new PlcVars.Word(this, new PlcVars.WordAddress(74), true);

            Prisotnost_AutoManSwitch = new PlcVars.Word(this, new PlcVars.WordAddress(76), true);
            Prisotnost_CurrentState = new PlcVars.Bit(this, new PlcVars.BitAddress(78, 0), false);

            Grelec1_AutoManSwitch = new PlcVars.Word(this, new PlcVars.WordAddress(80), true);
            Grelec1_CurrentState = new PlcVars.Bit(this, new PlcVars.BitAddress(82, 0), false);
            Grelec2_AutoManSwitch = new PlcVars.Word(this, new PlcVars.WordAddress(86), true);
            Grelec2_CurrentState = new PlcVars.Bit(this, new PlcVars.BitAddress(88, 0), false);

            ČrpalkaNivo_AutoManSwitch = new PlcVars.Word(this, new PlcVars.WordAddress(90), true);
            ČrpalkaNivo_CurrentState = new PlcVars.Bit(this, new PlcVars.BitAddress(92, 0), false);

            ČrpalkaFilter_AutoManSwitch = new PlcVars.Word(this, new PlcVars.WordAddress(93), true);
            ČrpalkaFilter_CurrentState = new PlcVars.Bit(this, new PlcVars.BitAddress(95, 0), false);

            ČrpalkaČasovnoDolivanje_AutoManSwitch = new PlcVars.Word(this, new PlcVars.WordAddress(96), true);
            ČrpalkaČasovnoDolivanje_CurrentState = new PlcVars.Bit(this, new PlcVars.BitAddress(98, 0), false);

            ČrpalkaHlajenje_AutoManSwitch = new PlcVars.Word(this, new PlcVars.WordAddress(99), true);
            ČrpalkaHlajenje_CurrentState = new PlcVars.Bit(this, new PlcVars.BitAddress(101, 0), false);

            TlačniSezorFiltra_AutoManSwitch = new PlcVars.Word(this, new PlcVars.WordAddress(102), true);
            TlačniSezorFiltra_CurrentState = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(104, 0), "Potrebno čiščenje filtra");

            MixVentilPlus_AutoManSwitch = new PlcVars.Word(this, new PlcVars.WordAddress(105), true);
            MixVentilPlus_CurrentState = new PlcVars.Bit(this, new PlcVars.BitAddress(107, 0), false);

            MixVentilMinus_AutoManSwitch = new PlcVars.Word(this, new PlcVars.WordAddress(108), true);
            MixVentilMinus_CurrentState = new PlcVars.Bit(this, new PlcVars.BitAddress(110, 0), false);

            NivoVisok_AutoManSwitch = new PlcVars.Word(this, new PlcVars.WordAddress(111), true);
            NivoVisok_CurrentState = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(113, 0), "Nivo je previsok", true);

            NivoNizek_AutoManSwitch = new PlcVars.Word(this, new PlcVars.WordAddress(114), true);
            AlarmNivoNizek_CurrentState = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(116, 0),  "Nivo je prenizek", true);

            VentilZrak_AutoManSwitch = new PlcVars.Word(this, new PlcVars.WordAddress(117), true);
            VentilZrak_CurrentState = new PlcVars.Bit(this, new PlcVars.BitAddress(119, 0), false);

            VentilPokrov_AutoManSwitch = new PlcVars.Word(this, new PlcVars.WordAddress(120), true);
            VentilPokrov_CurrentState = new PlcVars.Bit(this, new PlcVars.BitAddress(122, 0), false);

            VentilVoda_AutoManSwitch = new PlcVars.Word(this, new PlcVars.WordAddress(123), true);
            VentilVoda_CurrentState = new PlcVars.Bit(this, new PlcVars.BitAddress(125, 0), false);

            FlowSwitch_AutoManSwitch = new PlcVars.Word(this, new PlcVars.WordAddress(126), true);
            FlowSwitch_CurrentState = new PlcVars.Bit(this, new PlcVars.BitAddress(128, 0), false);

            PrejšnjaKad_Poraba = new PlcVars.Word(this, new PlcVars.WordAddress(130), false);
            Grelec1_Moc = new PlcVars.Word(this, new PlcVars.WordAddress(132), true);
            Grelec2_Moc = new PlcVars.Word(this, new PlcVars.WordAddress(134), true);
            ČrpalkaNivo_Moc = new PlcVars.Word(this, new PlcVars.WordAddress(136), true);
            ČrpalkaFilter_Moc = new PlcVars.Word(this, new PlcVars.WordAddress(138), true);
            ČrpalkaČasovnoDolivanje_Moc = new PlcVars.Word(this, new PlcVars.WordAddress(140), true);
            ČrpalkaHlajenje_Moc = new PlcVars.Word(this, new PlcVars.WordAddress(142), true);
            Rezerva_Moc = new PlcVars.Word(this, new PlcVars.WordAddress(144), true);
            TrenutnaKad_Poraba = new PlcVars.Word(this, new PlcVars.WordAddress(146), false);
            SestevekPorabe = new PlcVars.Word(this, new PlcVars.WordAddress(148), false);
            OmejitevPorabeCelotnegaSistema = new PlcVars.Word(this, new PlcVars.WordAddress(150), false);
            PorabaCelotnegaSistema = new PlcVars.Word(this, new PlcVars.WordAddress(152), false);

            IzbiraProcesneTemperature = new PlcVars.Word(this, new PlcVars.WordAddress(168), true);
            Temperatura1 = new PlcVars.Word(this, new PlcVars.WordAddress(170), false);
            Temperatura2 = new PlcVars.Word(this, new PlcVars.WordAddress(172), false);
            ProcesnaTemperatura = new PlcVars.Word(this, new PlcVars.WordAddress(174), false);
            TemperaturaHr = new PlcVars.Word(this, new PlcVars.WordAddress(176), false);
            PhReading = new PlcVars.Word(this, new PlcVars.WordAddress(178), false);
            Prevodnost = new PlcVars.Word(this, new PlcVars.WordAddress(180), false);
            Rezerva1 = new PlcVars.Word(this, new PlcVars.WordAddress(182), false); 
            Rezerva2 = new PlcVars.Word(this, new PlcVars.WordAddress(184), false);
            Rezerva3 = new PlcVars.Word(this, new PlcVars.WordAddress(186), false);

            SenFailTemperatura1 = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(190,0), "Napaka temperaturnega tipala 1");
            SenFailTemperatura2 = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(190, 1), "Napaka temperaturnega tipala 2");
            SenFailTemperaturaHr = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(190, 2), "Napaka temperaturnega tipala hranilnika");
            SenFailPh = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(190, 3), "Napaka meritve Ph");
            SenFailPrevodnost = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(190, 4), "Napaka meritve prevodnosti");
            SenFailRezerva1 = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(190, 5), "Napaka meritve rezerva1", true);
            SenFailRezerva2 = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(190, 6), "Napaka meritve rezerva2", true);
            SenFailRezerva3 = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(190, 7), "Napaka meritve rezerva3", true);

            PhSetpoint = new PlcVars.Word(this, new PlcVars.WordAddress(200), true);
        }
    }
}
