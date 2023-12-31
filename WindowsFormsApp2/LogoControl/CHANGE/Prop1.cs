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

        public Prop1(Sharp7.S7Client client) : base(client)
        {
            
            PCWD = new PlcVars.Word(this, new PlcVars.WordAddress(GetPCWD_Address()), false); //PC Watchdog

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

        }

    }
}
