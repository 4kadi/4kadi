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

        public PlcVars.DWord stopwatchCurrentTime;
        public PlcVars.Word stopwatchTimeLeft;
        public PlcVars.DWord stopwatchReminder, stopwatchPauseTime;

        public PlcVars.Bit stopwatchStartPulse, stopwatchStopPulse, stopwatchResetPulse, stopwatchFinished, stopwatchPaused;

        public PlcVars.Bit prisotnost;
        public PlcVars.Word AutostartStopwatch;

        public PlcVars.Byte weekday1, weekday2, weekday3, weekday4, weekday5, weekday6;        
        public PlcVars.Word ontime1, ontime2, ontime3, ontime4, ontime5, ontime6;
        public PlcVars.Word offtime1, offtime2, offtime3, offtime4, offtime5, offtime6;
        public PlcVars.Bit UrnikAktiven;

        public PlcVars.Word test;


        public Prop1(Sharp7.S7Client client) : base(client)
        {
            
            PCWD = new PlcVars.Word(this, new PlcVars.WordAddress(XmlController.GetPCWD_Address()), false); //PC Watchdog

            stopwatchCurrentTime = new PlcVars.DWord(this, new PlcVars.DoubleWordAddress(10), false);
            stopwatchTimeLeft = new PlcVars.Word(this, new PlcVars.WordAddress(14), false);
            stopwatchReminder = new PlcVars.DWord(this, new PlcVars.DoubleWordAddress(16), true);
            stopwatchPauseTime = new PlcVars.DWord(this, new PlcVars.DoubleWordAddress(20), true);

            stopwatchStartPulse = new PlcVars.Bit(this, new PlcVars.BitAddress(21, 0), true);
            stopwatchStopPulse = new PlcVars.Bit(this, new PlcVars.BitAddress(22, 0), true);
            stopwatchResetPulse = new PlcVars.Bit(this, new PlcVars.BitAddress(23,0), true);
            stopwatchFinished = new PlcVars.Bit(this, new PlcVars.BitAddress(24, 0), false);
            stopwatchPaused = new PlcVars.Bit(this, new PlcVars.BitAddress(25, 0), false);            

            prisotnost = new PlcVars.Bit(this, new PlcVars.BitAddress(30, 0), false);
            AutostartStopwatch = new PlcVars.Word(this, new PlcVars.WordAddress(32), true);

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

            test = new PlcVars.Word(this, new PlcVars.WordAddress(38), false);

        }

    }
}
