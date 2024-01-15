using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KontrolaKadi
{
    public partial class AutoManSwitch : Switch3point
    {
        private PlcVars.Word _Value_SwitchSetting;
        public PlcVars.Word Value_SwitchSetting 
        { 
            get { return _Value_SwitchSetting; } 

            set 
            {
                _Value_SwitchSetting = value;
                Value_SwitchSetting.ValueChanged += VaueChangedOnPLC;
            } 
        }

        private PlcVars.Bit _CurrentState_PLC;
        public PlcVars.Bit Value_CurrentState_PLC 
        { 
            get { return _CurrentState_PLC; } 
            set 
            {
                if (value != null)
                {
                    _CurrentState_PLC = value;
                    Value_CurrentState_PLC.ValueChanged += VaueChangedOnPLC;

                }                
            } 
        }

        public AutoManSwitch()
        {           
            base.StateChanged += AutoManSwitch_StateChanged;
            Click += AutoManSwitch_Click;
        }


        private void AutoManSwitch_Click(object sender, EventArgs e)
        {
           
        }

        private void AutoManSwitch_StateChanged(object sender, EventArgs e)
        {
            sendSwitchPositionDataToPLC();
        }

        void VaueChangedOnPLC(object sender, EventArgs e)
        {
            base.StateForReporting = _CurrentState_PLC.Value_bool;
            base.SetStateFromPlcVar(_Value_SwitchSetting);
        }

        void sendSwitchPositionDataToPLC()
        {
            if (Value_SwitchSetting == null)
            {
                return;
            }
            if (base.State == SwitchState.Left)
            {
                Value_SwitchSetting.Value_short = 1; // MAN 0
            }
            else if (base.State == SwitchState.Center)
            {
                Value_SwitchSetting.Value_short = 0; // AUTO
            }
            else if (base.State == SwitchState.Right)
            {
                Value_SwitchSetting.Value_short = 2;// MAN 1
            }
        }
    }
}
