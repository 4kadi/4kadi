using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KontrolaKadi
{
    public class PlcVars
    {
        public static List<AlarmBit> AllAlarmMessageVars { get; private set; } = new List<AlarmBit>();

        public static void ReportComunicationMessage(string message)
        {
            SysLog.SetMessage(message);
        }

        public class PlcAddress
        {
            public readonly int Address;

            public PlcAddress(int address)
            {
                Address = address;
            }

            public int GetAddress()
            {
                return Address;
            }
        }

        public class BitAddress : PlcAddress
        {
            public readonly ushort SubAddress;

            public BitAddress(int address, ushort subAddress) : base(address)
            {
                SubAddress = subAddress;
            }
            public int GetSubAddress()
            {
                return SubAddress;
            }

            public string GetStringRepresentation()
            {
                return "bit at " + GetAddress() + GetSubAddress();
            }

        }

        public class WordAddress : PlcAddress
        {
            public WordAddress(int address) : base(address)
            {

            }

            public string GetStringRepresentation()
            {
                return "VW" + GetAddress();
            }
        }

        public class ByteAddress : PlcAddress
        {
            public ByteAddress(int address) : base(address)
            {

            }

            public string GetStringRepresentation()
            {
                return "VW" + GetAddress();
            }
        }

        public class DoubleWordAddress : PlcAddress
        {
            public DoubleWordAddress(int address) : base(address)
            {

            }

            public string GetStringRepresentation()
            {
                return "DW" + GetAddress();
            }

        }

        // ////////////////////////////////////////////////////////

        public abstract class PlcType
        {
            public Sharp7.S7Client Client;

            private ushort _syncEvery_X_Time = 1;
            public ushort SyncEvery_X_Time // set to 1 for syncing to occur every loop -- 0 = turn sync off -- 2 = every 2nd loop...
            {
                get { return _syncEvery_X_Time; }
                set { Chk_SyncEvery_X_Time_Val(value); }
            }

            public PlcType(PropComm this_prop)
            {
                // adding base types of all variables to list "sorted" by prop to efficiently call sync procedure
                this_prop.AutoSync.Add(this);
                Client = this_prop.Client;
            }

            public abstract void SyncWithPLC();

            void Chk_SyncEvery_X_Time_Val(ushort val)
            {
                // limit
                if (val >= 0)
                {
                    if (val <= 5)
                    {
                        _syncEvery_X_Time = val;
                    }
                    else
                    {
                        _syncEvery_X_Time = 5;
                    }

                }
                else
                {
                    _syncEvery_X_Time = 1;
                }

            }



            public void AddWarningMonitor(object valueToTrigerWarning, WarningManager.WarningTriggerCondition Condition, string WarningMessage)
            {
                WarningManager.AddWarningTrackerFromPLCVar(this, valueToTrigerWarning, Condition, WarningMessage);
            }
        }


        public class Word : PlcType
        {
            public short? Value
            {
                get
                {
                    return PLCval;
                }
                set
                {
                    if (value != null)
                    {
                        ReadFromPCtoBuffer(value);
                    }
                }
            }

            public short Value_short
            {
                get
                {
                    if (PLCval != null)
                    {
                        return (short)PLCval;
                    }
                    else
                    {
                        return 0;
                    }
                }
                set
                {

                    ReadFromPCtoBuffer(value);

                }
            }

            public string Value_string
            {
                get
                {
                    var buff = Value;
                    if (buff != null)
                    {
                        return _prefixToShow + Value.ToString() + _postFixToShow;
                    }
                    return PropComm.NA;
                }
            }

            private short? PLCval;
            private short? previousPLCval;
            private short? PCval;
            private bool directionToPLC = false;
            private WordAddress _TypeAndAdress;
            int ErrRead;
            int ErrWrite;
            short? buffRead;
            short? buffWrite;
            string _prefixToShow;
            string _postFixToShow;
            bool _IsWritable = false;

            public event EventHandler ValueChanged;
            
            public Word(PropComm prop, WordAddress TypeAndAdress, string prefixToShow, string postFixToShow, bool IsWritable) : base(prop)
            {
                Ctor(TypeAndAdress, prefixToShow, postFixToShow, IsWritable);
            }

            public Word(PropComm prop, WordAddress TypeAndAdress, bool IsWritable) : base(prop)
            {
                Ctor(TypeAndAdress, "", "", IsWritable);
            }

            void Ctor(WordAddress TypeAndAdress, string prefixToShow, string postFixToShow, bool IsWritable)
            {
                PLCval = null;
                PCval = null;


                _TypeAndAdress = TypeAndAdress;
                _prefixToShow = prefixToShow;
                _postFixToShow = postFixToShow;
                _IsWritable = IsWritable;             

            }

            public override void SyncWithPLC()
            {
                try
                {
                    ReadFromPLCtoBuffer(false);
                    WriteToPLCFromBuffer();
                }
                catch (Exception ex)
                {
                    ReportComunicationMessage(ex.Message);
                }

            }

            private void ReadFromPLCtoBuffer(bool forceRead)
            {                
                if (directionToPLC == false || forceRead)
                {
                    if (Client != null)
                    {
                        buffRead = Connection.BufferRead(Client, _TypeAndAdress, out ErrRead);
                        if (ErrRead == 0 && buffRead != null)
                        {
                            if (buffRead != PLCval)
                            {
                                PLCval = buffRead; buffRead = null;                               
                            }
                            if (previousPLCval != PLCval)
                            {                                
                                previousPLCval = PLCval;
                                Task.Run(InvokeOnValueChangeEvent);                                
                            }
                        }
                        else
                        {
                            ReportError("Read from PLC failed.", null, forceRead);
                        }
                    }
                }
            }

            void InvokeOnValueChangeEvent()
            {               
                ValueChanged?.Invoke(this, EventArgs.Empty);                
            }

            private void WriteToPLCFromBuffer()
            {
                if (PLCval == null)
                {
                    directionToPLC = false;
                }
                else if (directionToPLC == true)
                {
                    if (PCval != null && PLCval != null)
                    {
                        if (PCval != PLCval)
                        {
                            buffWrite = PCval;

                            if (Client != null)
                            {
                                if (_IsWritable)
                                {
                                    Connection.PLCwrite(Client, _TypeAndAdress, (short)buffWrite, out ErrWrite);
                                    if (ErrWrite == 0)
                                    {
                                        PLCval = buffWrite;
                                    }
                                    else
                                    {
                                        ReportError_throwException("Write to PLC failed.");
                                    }
                                }
                                else
                                {
                                    ReportError_throwException("Write to PLC failed IsWritable flag must be true for writing to PLC.");
                                }
                            }
                        }
                        directionToPLC = false;
                    }
                }
            }

            private void ReadFromPCtoBuffer(short? value)
            {
                if (value != null)
                {
                    directionToPLC = true;
                    PCval = value;
                }
            }

            public void ReportError_throwException(string Message)
            {
                ReportError(Message, null, null);
            }
            public void ReportError(string Message, bool? forceSet_FlagToReport, bool? forceRead_FlagToReport)
            {
                string Address = _TypeAndAdress.GetStringRepresentation();
                string ErrTyp_Read = Client.ErrorText(ErrRead);
                string ErrTyp_Write = Client.ErrorText(ErrWrite);
                string ClientName = "Logo" + Client.deviceID;
                string Flags = "directionToPLC: " + directionToPLC;

                if (forceSet_FlagToReport != null)
                {
                    Flags += " forceSet: " + forceSet_FlagToReport.ToString() + ";";
                }

                if (forceRead_FlagToReport != null)
                {
                    Flags += " forceRead: " + forceSet_FlagToReport.ToString() + ";";
                }

                Flags += " isWritable: " + _IsWritable.ToString() + ";";


                SysLog.Message.SetMessage(Message + " " +
                    "Address: " + Address + ", " +
                    "Read Error type: " + ErrTyp_Read + ", " +
                    "Write Error type: " + ErrTyp_Write + ", " +
                    "Client: " + ClientName + ". " +
                    "Flags: " + Flags);
            }

        }

        public class Byte : PlcType
        {
            public byte? Value
            {
                get
                {
                    return PLCval;
                }
                set
                {
                    if (value != null)
                    {
                        ReadFromPCtoBuffer(value);
                    }
                }
            }

            public byte Value_byte
            {
                get
                {
                    if (PLCval != null)
                    {
                        return (byte)PLCval;
                    }
                    else
                    {
                        return 0;
                    }
                }
                set
                {                
                    ReadFromPCtoBuffer(value);                    
                }
            }

            public string Value_string
            {
                get
                {
                    var buff = Value;
                    if (buff != null)
                    {
                        return _prefixToShow + Value.ToString() + _postFixToShow;
                    }
                    return PropComm.NA;
                }
            }

            private byte? PLCval;
            private byte? previousPLCval;
            private byte? PCval;
            private bool directionToPLC = false;
            private ByteAddress _TypeAndAdress;
            int ErrRead;
            int ErrWrite;
            byte? buffRead;
            byte? buffWrite;
            string _prefixToShow;
            string _postFixToShow;
            bool _IsWritable = false;

            public event EventHandler ValueChanged;           

            public Byte(PropComm prop, ByteAddress TypeAndAdress, string prefixToShow, string postFixToShow, bool IsWritable) : base(prop)
            {
                Ctor(TypeAndAdress, prefixToShow, postFixToShow, IsWritable);
            }

            public Byte(PropComm prop, ByteAddress TypeAndAdress, bool IsWritable) : base(prop)
            {
                Ctor(TypeAndAdress, "", "", IsWritable);
            }

            void Ctor(ByteAddress TypeAndAdress, string prefixToShow, string postFixToShow, bool IsWritable)
            {
                PLCval = null;
                PCval = null;


                _TypeAndAdress = TypeAndAdress;
                _prefixToShow = prefixToShow;
                _postFixToShow = postFixToShow;
                _IsWritable = IsWritable;                
            }

            public override void SyncWithPLC()
            {
                try
                {
                    ReadFromPLCtoBuffer(false);
                    WriteToPLCFromBuffer();
                }
                catch (Exception ex)
                {
                    ReportComunicationMessage(ex.Message);
                }

            }

            private void ReadFromPLCtoBuffer(bool forceRead)
            {
                if (directionToPLC == false || forceRead)
                {
                    if (Client != null)
                    {
                        buffRead = (byte)Connection.BufferRead(Client, _TypeAndAdress, out ErrRead);

                        if (ErrRead == 0 && buffRead != null)
                        {
                            if (buffRead != PLCval)
                            {        
                                PLCval = buffRead; buffRead = null;
                            }
                            if (previousPLCval != PLCval)
                            {
                                previousPLCval = PLCval;
                                Task.Run(InvokeOnValueChangeEvent);
                            }                            
                        }
                        else
                        {
                            ReportError_throwException("Read from PLC failed.", null, forceRead);
                        }
                    }
                }
            }

            void InvokeOnValueChangeEvent()
            {
                ValueChanged?.Invoke(this, EventArgs.Empty);
            }

            private void WriteToPLCFromBuffer()
            {
                if (PLCval == null)
                {
                    directionToPLC = false;
                }
                else if (directionToPLC == true)
                {
                    if (PCval != null && PLCval != null)
                    {
                        if (PCval != PLCval)
                        {
                            buffWrite = PCval;

                            if (Client != null)
                            {
                                if (_IsWritable)
                                {
                                    Connection.PLCwrite(Client, _TypeAndAdress, (byte)buffWrite, out ErrWrite);
                                    if (ErrWrite == 0)
                                    {
                                        PLCval = buffWrite;
                                    }
                                    else
                                    {
                                        ReportError_throwException("Write to PLC failed.");
                                    }
                                }
                                else
                                {
                                    ReportError_throwException("Write to PLC failed IsWritable flag must be true for writing to PLC.");
                                }
                            }
                        }
                        directionToPLC = false;
                    }
                }
            }

            private void ReadFromPCtoBuffer(byte? value)
            {
                if (value != null)
                {
                    directionToPLC = true;
                    PCval = value;
                }
            }

            public void ReportError_throwException(string Message)
            {
                ReportError_throwException(Message, null, null);
            }
            public void ReportError_throwException(string Message, bool? forceSet_FlagToReport, bool? forceRead_FlagToReport)
            {
                string Address = _TypeAndAdress.GetStringRepresentation();
                string ErrTyp_Read = Client.ErrorText(ErrRead);
                string ErrTyp_Write = Client.ErrorText(ErrWrite);
                string ClientName = "Logo" + Client.deviceID;
                string Flags;

                Flags = "directionToPLC: " + directionToPLC;

                if (forceSet_FlagToReport != null)
                {
                    Flags += " forceSet: " + forceSet_FlagToReport.ToString() + ";";
                }

                if (forceRead_FlagToReport != null)
                {
                    Flags += " forceRead: " + forceSet_FlagToReport.ToString() + ";";
                }

                Flags += " isWritable: " + _IsWritable.ToString() + ";";


                throw new Exception(
                    Message + " " +
                    "Address: " + Address + ", " +
                    "Read Error type: " + ErrTyp_Read + ", " +
                    "Write Error type: " + ErrTyp_Write + ", " +
                    "Client: " + Client + ". " +
                    "Flags: " + Flags);
            }

        }

        public class DWord : PlcType
        {
            public short? Value
            {
                get
                {
                    return PLCval;
                }
                set
                {
                    if (value != null)
                    {
                        ReadFromPCtoBuffer(value);
                    }
                }
            }

            public short? Value_short
            {
                get
                {
                    if (PLCval != null)
                    {
                        return (short)PLCval;
                    }
                    else
                    {
                        return 0;
                    }
                }
                set
                {
                    if (value != null)
                    {
                        ReadFromPCtoBuffer(value);
                    }
                }
            }

            public string Value_string
            {
                get
                {
                    var buff = Value;
                    if (buff != null)
                    {
                        return _prefixToShow + Value.ToString() + _postFixToShow;
                    }
                    return PropComm.NA;
                }
            }

            private short? PLCval;
            private short? previousPLCval;
            private short? PCval;
            private bool directionToPLC = false;
            private DoubleWordAddress _TypeAndAdress;            
            int ErrRead;
            int ErrWrite;
            short? buffRead;
            short? buffWrite;
            string _prefixToShow;
            string _postFixToShow;
            bool _IsWritable = false;

            public event EventHandler ValueChanged;

            private void Ctor(PropComm prop, DoubleWordAddress TypeAndAdress, string prefixToShow, string postFixToShow, bool IsWritable)
            {
                PLCval = null;
                PCval = null;
                
                _TypeAndAdress = TypeAndAdress;
                _prefixToShow = prefixToShow;
                _postFixToShow = postFixToShow;
                _IsWritable = IsWritable;

            }

            public DWord(PropComm prop, DoubleWordAddress TypeAndAdress, string prefixToShow, string postFixToShow, bool IsWritable) : base(prop)
            {
                Ctor(prop, TypeAndAdress, prefixToShow, postFixToShow, IsWritable);
            }

            public DWord(PropComm prop, DoubleWordAddress TypeAndAdress, bool IsWritable) : base(prop)
            {
                Ctor(prop, TypeAndAdress, "", "", IsWritable);
            }

            public override void SyncWithPLC()
            {
                try
                {
                    ReadFromPLCtoBuffer(false);
                    WriteToPLCFromBuffer();
                }
                catch (Exception ex)
                {
                    ReportComunicationMessage(ex.Message);
                }

            }

            private void ReadFromPLCtoBuffer(bool forceRead)
            {
                if (directionToPLC == false || forceRead)
                {
                    if (Client != null)
                    {
                        buffRead = Connection.BufferRead(Client, _TypeAndAdress, out ErrRead);
                        if (ErrRead == 0 && buffRead != null)
                        {
                            if (buffRead != PLCval)
                            {
                                PLCval = buffRead; buffRead = null;                                
                            }
                            if (previousPLCval != PLCval)
                            {
                                previousPLCval = PLCval;
                                Task.Run(InvokeOnValueChangeEvent);
                            }
                            
                        }
                        else
                        {
                            ReportError_throwException("Read from PLC failed.", null, forceRead);
                        }
                    }
                }
            }

            void InvokeOnValueChangeEvent()
            {
                ValueChanged?.Invoke(this, EventArgs.Empty);
            }

            private void WriteToPLCFromBuffer()
            {
                if (PLCval == null)
                {
                    directionToPLC = false;
                }
                else if (directionToPLC == true)
                {
                    if (PCval != null && PLCval != null)
                    {
                        if (PCval != PLCval)
                        {
                            buffWrite = PCval;

                            if (Client != null)
                            {
                                if (_IsWritable)
                                {
                                    Connection.PLCwrite(Client, _TypeAndAdress, (short)buffWrite, out ErrWrite);
                                    if (ErrWrite == 0)
                                    {
                                        PLCval = buffWrite;
                                    }
                                    else
                                    {
                                        ReportError_throwException("Write to PLC failed.");
                                    }
                                }
                                else
                                {
                                    ReportError_throwException("Write to PLC failed IsWritable flag must be true for writing to PLC.");
                                }
                            }
                        }
                        directionToPLC = false;
                    }
                }
            }

            private void ReadFromPCtoBuffer(short? value)
            {
                if (value != null)
                {
                    directionToPLC = true;
                    PCval = value;
                }
            }

            public void ReportError_throwException(string Message)
            {
                ReportError_throwException(Message, null, null);
            }
            public void ReportError_throwException(string Message, bool? forceSet_FlagToReport, bool? forceRead_FlagToReport)
            {
                throw new Exception(Message);
            }
        }

        public class TimeSet : PlcType
        {
            public string Value
            {
                get
                {
                    if (PLCval != null)
                    {
                        if (PLCval != null)
                        {
                            return DecodeWordToTime((short)PLCval, 1).ToString();
                        }
                    }
                    return PropComm.NA;

                }
                set
                {
                    if (value != null)
                    {
                        if (value != PropComm.NA)
                        {
                            try
                            {
                                var buff = EncodeWordToTime(value);
                                ReadFromPCtoBuffer(buff);
                            }
                            catch (Exception ex)
                            {
                                ReportError_throwException("Error setting value for weektimer." + ex.Message);
                            }
                        }

                    }
                }
            }

            private short? PLCval;
            private short? previousPLCval;
            private short? PCval;
            private bool directionToPLC = false;
            private readonly WordAddress _TypeAndAdress;
            private readonly Sharp7.S7Client _Client;
            int ErrRead;
            int ErrWrite;
            short? buffRead;
            short? buffWrite;
            readonly bool _IsWritable = false;

            public event EventHandler ValueChanged;

            public TimeSet(PropComm prop, WordAddress TypeAndAdress, bool IsWritable) : base(prop)
            {
                PLCval = null;
                PCval = null;

                _Client = Client;
                _TypeAndAdress = TypeAndAdress;
                _IsWritable = IsWritable;
            }

            public override void SyncWithPLC()
            {
                try
                {
                    ReadFromPLCtoBuffer(false);
                    WriteToPLCFromBuffer();
                }
                catch (Exception ex)
                {
                    ReportComunicationMessage(ex.Message);
                }
            }

            private void ReadFromPLCtoBuffer(bool forceRead)
            {
                if (directionToPLC == false || forceRead)
                {
                    if (_Client != null)
                    {
                        buffRead = Connection.BufferRead(_Client, _TypeAndAdress, out ErrRead);
                        if (ErrRead == 0 && buffRead != null) 
                        {                          
                           PLCval = buffRead; buffRead = null;                           
                        }
                        if (previousPLCval != PLCval)
                        {
                            previousPLCval = PLCval;
                            Task.Run(InvokeOnValueChangeEvent);
                        }
                    }
                    else
                    {
                        ReportError_throwException("Read from PLC failed.", null, forceRead);
                    }
                }
            }

            void InvokeOnValueChangeEvent()
            {
                ValueChanged?.Invoke(this, EventArgs.Empty);
            }

            private void WriteToPLCFromBuffer()
            {
                if (PLCval == null)
                {
                    directionToPLC = false;
                }
                else if (directionToPLC == true)
                {
                    if (PCval != null && PLCval != null)
                    {
                        if (PCval != PLCval)
                        {
                            buffWrite = PCval;

                            if (_Client != null)
                            {
                                if (_IsWritable)
                                {
                                    Connection.PLCwrite(_Client, _TypeAndAdress, (short)buffWrite, out ErrWrite);
                                    if (ErrWrite == 0)
                                    {
                                        PLCval = buffWrite;
                                    }
                                    else
                                    {
                                        ReportError_throwException("Write to PLC failed.");
                                    }
                                }
                                else
                                {
                                    ReportError_throwException("Write to PLC failed IsWritable flag must be true for writing to PLC.");
                                }
                            }
                        }
                        directionToPLC = false;
                    }
                }
            }

            private void ReadFromPCtoBuffer(short? value)
            {
                if (value != null)
                {
                    directionToPLC = true;
                    PCval = value;
                }
            }

            public void ReportError_throwException(string Message)
            {
                ReportError_throwException(Message, null, null);
            }
            public void ReportError_throwException(string Message, bool? forceSet_FlagToReport, bool? forceRead_FlagToReport)
            {
                string Address = _TypeAndAdress.GetStringRepresentation();
                string ErrTyp_Read = _Client.ErrorText(ErrRead);
                string ErrTyp_Write = _Client.ErrorText(ErrWrite);
                string Client = "Logo" + _Client.deviceID;
                string Flags;

                Flags = "directionToPLC: " + directionToPLC;

                if (forceSet_FlagToReport != null)
                {
                    Flags += " forceSet: " + forceSet_FlagToReport.ToString() + ";";
                }

                if (forceRead_FlagToReport != null)
                {
                    Flags += " forceRead: " + forceSet_FlagToReport.ToString() + ";";
                }

                Flags += " isWritable: " + _IsWritable.ToString() + ";";


                throw new Exception(
                    Message + " " +
                    "Address: " + Address + ", " +
                    "Read Error type: " + ErrTyp_Read + ", " +
                    "Write Error type: " + ErrTyp_Write + ", " +
                    "Client: " + Client + ". " +
                    "Flags: " + Flags);
            }
        }

        public class TemperatureShow : PlcType
        {
            public float? Value
            {
                get
                {
                    if (PLCval != null)
                    {
                        return Scalate(PLCval);
                    }
                    return 0;
                }
                set
                {
                    if (value != null)
                    {
                        ReadFromPCtoBuffer(DeScalate(value));
                    }
                }
            }

            public string Value_string
            {
                get
                {
                    var buff = Value;
                    if (buff != null)
                    {
                        float val = (float)Value;
                        return _prefixToShow + val + _postFixToShow;
                    }
                    return PropComm.NA;
                }
            }

            public string Value_string_formatted
            {
                get
                {
                    var buff = Value;
                    if (buff != null)
                    {
                        float val = (float)Value;
                        return _prefixToShow + val.ToString("0.0") + _postFixToShow;
                    }
                    return PropComm.NA;
                }
            }

            private short? PLCval;
            private short? previousPLCval;
            private short? PCval;
            private bool directionToPLC = false;
            private readonly WordAddress _TypeAndAdress;
            private readonly Sharp7.S7Client _Client;
            int ErrRead;
            int ErrWrite;
            short? buffRead;
            short? buffWrite;
            readonly string _prefixToShow;
            readonly string _postFixToShow;
            public float _kx;
            public float _n;
            readonly bool _isWritable;
            private int decimalPlaces;

            public event EventHandler ValueChanged;

            public int DecimalPlaces
            {
                get { return decimalPlaces; }
                set
                {
                    if (value < 0)
                    {
                        decimalPlaces = 0;
                    }
                    if (value > 5)
                    {
                        decimalPlaces = 5;
                    }
                    decimalPlaces = value;
                }
            }

            public TemperatureShow(PropComm prop, WordAddress TypeAndAdress, string prefixToShow, string postFixToShow, float calibOffset, float calibMultiply, int decimals, bool IsWritable) : base(prop)
            {
                PLCval = null;
                PCval = null;

                decimalPlaces = decimals;

                _Client = Client;
                _TypeAndAdress = TypeAndAdress;
                _prefixToShow = prefixToShow;
                _postFixToShow = postFixToShow;
                _n = calibOffset;
                _kx = calibMultiply;
                _isWritable = IsWritable;

            }

            public override void SyncWithPLC()
            {
                try
                {
                    ReadFromPLCtoBuffer(false);
                    WriteToPLCFromBuffer();
                }
                catch (Exception ex)
                {
                    ReportComunicationMessage(ex.Message);
                }
            }

            private void ReadFromPCtoBuffer(short? value)
            {
                if (value != null)
                {
                    if (value != PCval)
                    {
                        directionToPLC = true;
                        PCval = value;
                    }
                }
            }

            private void ReadFromPLCtoBuffer(bool forceRead)
            {
                if (directionToPLC == false || forceRead)
                {
                    if (_Client != null)
                    {
                        buffRead = Connection.BufferRead(_Client, _TypeAndAdress, out ErrRead);
                        if (ErrRead == 0 && buffRead != null) 
                        { 
                            PLCval = buffRead; buffRead = null; 
                        }
                        if (previousPLCval != PLCval)
                        {
                            previousPLCval = PLCval;
                            Task.Run(InvokeOnValueChangeEvent);
                        }
                        else
                        {
                            ReportError_throwException("Read from PLC failed.", null, forceRead);
                        }
                    }
                }
            }

            void InvokeOnValueChangeEvent()
            {
                ValueChanged?.Invoke(this, EventArgs.Empty);
            }

            private void WriteToPLCFromBuffer()
            {
                if (PLCval == null)
                {
                    directionToPLC = false;
                }
                else if (directionToPLC == true)
                {
                    if (PCval != null && PLCval != null)
                    {
                        if (PCval != PLCval)
                        {
                            buffWrite = PCval;

                            if (_Client != null)
                            {
                                if (_isWritable)
                                {
                                    Connection.PLCwrite(_Client, _TypeAndAdress, (short)buffWrite, out ErrWrite);
                                    if (ErrWrite == 0)
                                    {
                                        PLCval = buffWrite;
                                    }
                                    else
                                    {
                                        ReportError_throwException("Write to PLC failed.");
                                    }
                                }
                                else
                                {
                                    ReportError_throwException("Write to PLC failed IsWritable flag must be true for writing to PLC.");
                                }
                            }
                        }
                        directionToPLC = false;
                    }
                }
            }

            public virtual float? Scalate(short? val)
            {
                if (val != null)
                {
                    var value = (float)val;
                    return (_kx * value + _n);
                }
                return 0;
            }

            private short? DeScalate(float? val)
            {
                if (val != null)
                {
                    return Misc.ToShort(((float)val - _n) / _kx);
                }
                return null;
            }

            public void ReportError_throwException(string Message)
            {
                ReportError_throwException(Message, null, null);
            }
            public void ReportError_throwException(string Message, bool? forceSet_FlagToReport, bool? forceRead_FlagToReport)
            {
                string Address = _TypeAndAdress.GetStringRepresentation();
                string ErrTyp_Read = _Client.ErrorText(ErrRead);
                string ErrTyp_Write = _Client.ErrorText(ErrWrite);
                string Client = "Logo" + _Client.deviceID;
                string Flags;

                Flags = "directionToPLC: " + directionToPLC;

                if (forceSet_FlagToReport != null)
                {
                    Flags += " forceSet: " + forceSet_FlagToReport.ToString() + ";";
                }

                if (forceRead_FlagToReport != null)
                {
                    Flags += " forceRead: " + forceSet_FlagToReport.ToString() + ";";
                }

                throw new Exception(
                    Message + " " +
                    "Address: " + Address + ", " +
                    "Read Error type: " + ErrTyp_Read + ", " +
                    "Write Error type: " + ErrTyp_Write + ", " +
                    "Client: " + Client + ". " +
                    "Flags: " + Flags);
            }

        }

        public class Bit : PlcType
        {
            public bool? Value
            {
                get
                {
                    return PLCval;
                }
                set
                {
                    ReadFromPCtoBuffer(value);
                }
            }

            public bool Value_bool
            {
                get
                {
                    if (PLCval != null)
                    {
                        return (bool)PLCval;
                    }
                    else
                    {
                        return false;
                    }
                }
                set
                {
                    ReadFromPCtoBuffer(value);
                }
            }

            public void ToggleValue()
            {
                Value_bool = !Value_bool;
            }

            private bool? PLCval;
            private bool? previousPLCval;
            private bool? PCval;
            private bool directionToPLC = false;
            private readonly BitAddress _TypeAndAdress;
            private readonly Sharp7.S7Client _Client;
            int ErrRead;
            int ErrWrite;
            short? buffRead;
            short? buffWrite;
            readonly bool _IsWritable = false;
            byte sendpulseState = 0;

            public delegate void ValueChangedDelegate(object sender, EventArgs e);
            public event ValueChangedDelegate ValueChanged;

            public Bit(PropComm prop, BitAddress TypeAndAdress, bool IsWritable) : base(prop)
            {
                PLCval = null;
                PCval = null;

                _Client = Client;
                _TypeAndAdress = TypeAndAdress;
                _IsWritable = IsWritable;
               
            }


            public void SendPulse()
            {
                sendpulseState = 1;
                Value = true;
            }

            private void StopSendPulse()
            {
                if (sendpulseState == 2)
                {
                    directionToPLC = false;
                    ReadFromPLCtoBuffer(true);

                    if (!Value_bool)
                    {
                        sendpulseState = 0;
                    }
                }
            }

            public override void SyncWithPLC()
            {
                try
                {
                    if (sendpulseState == 1)
                    {
                        WriteToPLCFromBuffer(true);
                        Value = false;
                        sendpulseState = 2;
                    }
                    else if (sendpulseState == 2)
                    {
                        WriteToPLCFromBuffer(true);
                        StopSendPulse();
                    }
                    else
                    {
                        ReadFromPLCtoBuffer(false);
                        WriteToPLCFromBuffer(false);
                    }
                }
                catch (Exception ex)
                {
                    ReportComunicationMessage(ex.Message);
                }
            }

            private void ReadFromPLCtoBuffer(bool forceRead)
            {
                if (directionToPLC == false || forceRead)
                {
                    if (_Client != null)
                    {
                        buffRead = Connection.BufferRead(_Client, _TypeAndAdress, out ErrRead);
                        if (ErrRead == 0 && buffRead != null)
                        {
                            if (buffRead > 0) 
                            {                                                        
                                PLCval = true;                                                            
                            } 
                            else 
                            {                                                               
                                PLCval = false;
                            }
                            if (previousPLCval != PLCval)
                            {
                                previousPLCval = PLCval;
                                Task.Run(InvokeOnValueChangeEvent);
                            }

                            buffRead = null;
                        }
                        else
                        {
                            ReportError_throwException("Read from PLC failed.", null, forceRead);
                        }
                    }
                }
            }

            void InvokeOnValueChangeEvent()
            {
                ValueChanged?.Invoke(this, EventArgs.Empty);
            }

            private void WriteToPLCFromBuffer(bool forceWrite)
            {
                if (PLCval == null)
                {
                    directionToPLC = false;
                }
                else if (directionToPLC == true)
                {
                    if (PCval != null && PLCval != null)
                    {
                        if (PCval != PLCval || forceWrite)
                        {
                            if ((bool)PCval)
                            { buffWrite = 1; }
                            else { buffWrite = 0; }

                            if (_Client != null)
                            {
                                if (_IsWritable)
                                {
                                    Connection.PLCwrite(_Client, _TypeAndAdress, (short)buffWrite, out ErrWrite);
                                    if (ErrWrite == 0)
                                    {
                                        PLCval = Convert.ToBoolean(buffWrite);
                                    }
                                    else
                                    {
                                        ReportError_throwException("Write to PLC failed.");
                                    }
                                }
                                else
                                {
                                    ReportError_throwException("Write to PLC failed IsWritable flag must be true for writing to PLC.");
                                }
                            }
                        }
                        directionToPLC = false;
                    }
                }
            }

            private void ReadFromPCtoBuffer(bool? value)
            {
                if (value != null)
                {
                    directionToPLC = true;
                    PCval = value;
                }
            }

            void ReportError_throwException(string Message)
            {
                ReportError_throwException(Message, null, null);
            }
            public void ReportError_throwException(string Message, bool? forceSet_FlagToReport, bool? forceRead_FlagToReport)
            {
                string Address = _TypeAndAdress.GetStringRepresentation();
                string ErrTyp_Read = _Client.ErrorText(ErrRead);
                string ErrTyp_Write = _Client.ErrorText(ErrWrite);
                string Client = "Logo" + _Client.deviceID;
                string Flags;

                Flags = "directionToPLC: " + directionToPLC;

                if (forceSet_FlagToReport != null)
                {
                    Flags += " forceSet: " + forceSet_FlagToReport.ToString() + ";";
                }

                if (forceRead_FlagToReport != null)
                {
                    Flags += " forceRead: " + forceSet_FlagToReport.ToString() + ";";
                }

                Flags += " isWritable: " + _IsWritable.ToString() + ";";


                throw new Exception(
                    Message + " " +
                    "Address: " + Address + ", " +
                    "Read Error type: " + ErrTyp_Read + ", " +
                    "Write Error type: " + ErrTyp_Write + ", " +
                    "Client: " + Client + ". " +
                    "Flags: " + Flags);
            }

        }

        protected class LogoDate_Second : Byte
        {
            public LogoDate_Second(PropComm prop) : base(prop, new ByteAddress(990), "", "", true)
            {

            }
        }

        protected class LogoDate_Minute : Byte
        {
            public LogoDate_Minute(PropComm prop) : base(prop, new ByteAddress(989), "", "", true)
            {

            }
        }

        protected class LogoDate_Hour : Byte
        {
            public LogoDate_Hour(PropComm prop) : base(prop, new ByteAddress(988), "", "", true)
            {

            }
        }

        protected class LogoDate_Day : Byte
        {
            public LogoDate_Day(PropComm prop) : base(prop, new ByteAddress(987), "", "", true)
            {

            }
        }

        protected class LogoDate_Month : Byte
        {
            public LogoDate_Month(PropComm prop) : base(prop, new ByteAddress(986), "", "", true)
            {

            }
        }

        protected class LogoDate_Year : Byte
        {
            public LogoDate_Year(PropComm prop) : base(prop, new ByteAddress(985), "", "", true)
            {

            }
        }

        public class LogoDateTime
        {
            LogoDate_Year Y;
            LogoDate_Month M;
            LogoDate_Day D;
            LogoDate_Hour h;
            LogoDate_Minute m;
            LogoDate_Second s;

            DateTime _Datetime;
            public DateTime Value_Datetime
            {
                get { return _Datetime; }
                set
                {
                    SetDateTimeOnPLC(value);
                }
            }
            public TimeSpan Value_TodaysTime { get; private set; }

            public event EventHandler ValueChanged;

            public LogoDateTime(PropComm prop)
            {
                Y = new LogoDate_Year(prop);
                M = new LogoDate_Month(prop);
                D = new LogoDate_Day(prop);
                h = new LogoDate_Hour(prop);
                m = new LogoDate_Minute(prop);
                s = new LogoDate_Second(prop);

                s.ValueChanged += Clock_ValueChanged;
            }

            private void Clock_ValueChanged(object sender, EventArgs e)
            {
                _Datetime = new DateTime(Y.Value_byte + 2000, M.Value_byte, D.Value_byte).AddHours(h.Value_byte).AddMinutes(m.Value_byte).AddSeconds(s.Value_byte);
                if (ValueChanged != null)
                {
                    Value_TodaysTime = TimeSpan.FromHours(_Datetime.Hour) + TimeSpan.FromMinutes(_Datetime.Minute) + TimeSpan.FromSeconds(_Datetime.Second);
                    ValueChanged.Invoke(this, EventArgs.Empty);
                }
            }

            void SetDateTimeOnPLC(DateTime value)
            {
                Y.Value = (byte)(value.Year -2000); // convert to two digit format
                M.Value = (byte)value.Month;
                D.Value = (byte)value.Day;
                h.Value = (byte)value.Hour;
                m.Value = (byte)value.Minute;
                s.Value = (byte)value.Second;
            }
        }
     
        public class AlarmBit : Bit
        {
            public string Message { get; private set; }
            public bool InvertState = false;

            public AlarmBit(PropComm prop, BitAddress TypeAndAdress, string Message, bool invertState, bool IsWritable) : base(prop, TypeAndAdress, IsWritable)
            {
                InvertState = invertState;               
                this.Message = Message;
                AllAlarmMessageVars.Add(this);
                AddMonitor();
            }

            public AlarmBit(PropComm prop, BitAddress TypeAndAdress, string Message, bool invertState) : base(prop, TypeAndAdress, false)
            {
                InvertState = invertState;              
                this.Message = Message;
                AllAlarmMessageVars.Add(this);
                AddMonitor();
            }
            public AlarmBit(PropComm prop, BitAddress TypeAndAdress, string Message) : base(prop, TypeAndAdress, false)
            {
                InvertState = false;
                this.Message = Message;
                AllAlarmMessageVars.Add(this);
                AddMonitor();
            }

            void AddMonitor()
            {
                if (InvertState)
                {
                    AddWarningMonitor(false, WarningManager.WarningTriggerCondition.EqualTo, Message);
                }
                else
                {
                    AddWarningMonitor(true, WarningManager.WarningTriggerCondition.EqualTo, Message);
                }

            }
        }

        // ////////////////////////////////////////////////////////

        public static string RemoveFromEnd(string s, string suffix)
        {
            if (s.EndsWith(suffix))
            {
                return s.Substring(0, s.Length - suffix.Length);
            }
            else
            {
                return s;
            }
        }

        public static string RemoveFromBegining(string s, string prefix)
        {
            if (s.StartsWith(prefix))
            {
                return s.Substring(prefix.Length, s.Length - prefix.Length);
            }
            else
            {
                return s;
            }
        }

        public static string DecodeWordToTime(short word, int method) //
        {
            if (method == 1) // Siemens logo week timer
            {
                //  1234 -> ##:##           
                var d1 = word / 4069;
                var leftover = word % 4069;
                if (d1 > 2)
                { return PropComm.NA; }
                var d2 = leftover / 256;
                leftover = word % 256;
                if (d2 > 9)
                { return PropComm.NA; }
                var d3 = leftover / 16;
                leftover = word % 16;
                if (d3 > 5)
                { return PropComm.NA; }
                var d4 = leftover;
                if (d4 > 9)
                { return PropComm.NA; }
                return ((d1.ToString() + d2.ToString()).ToString() + ":" + (d3.ToString() + d4.ToString()).ToString());
            }
            if (method == 2) // used to get clock from siemens logo
            {
                //  1234 -> ##:##           
                var d1 = word / 256;
                var leftover = word % 256;
                return d1.ToString("00") + ":" + leftover.ToString("00");
            }

            return "ERR";

        }

        public static short? EncodeWordToTime(string val)
        {
            var s = val.ToCharArray();
            if (s.Length != 5)
            {
                return null;
            }

            short con;
            try
            {
                con = Convert.ToInt16((short.Parse(s[0].ToString()) * 4096) + (short.Parse(s[1].ToString()) * 256) + (short.Parse(s[3].ToString()) * 16) + (short.Parse(s[4].ToString())));
            }
            catch
            {
                return null;
            }

            return con;

        }

    }
}
