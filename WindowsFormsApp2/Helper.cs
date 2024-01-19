using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
using System.Threading;
using System.Windows.Forms;

namespace KontrolaKadi
{

    public class Helper
    {
        public static string FloatToStringWeb(float f, string postFix)
        {
            return f.ToString("0.##").Replace(",", ".") + postFix;
        }        

        public static string FloatToStringWeb(double f, string postFix)
        {
            return f.ToString("0.##").Replace(",", ".") + postFix;
        }

        public class Initialiser
        {            
            public Initialiser()
            {                
                Val.InitialiseClass();
            }
        }
        
               
        public static string GetNumbersOnlyFromString(string input)
        {
            return new string(input.Where(c => char.IsDigit(c)).ToArray());
        }

        private static bool alreadyClosing = false;
        public static void ExitApp()
        {
            if (alreadyClosing)
            {
                return;
            }
            alreadyClosing = true;

            if (Program.GetContext().Restart)
            {
                return;
            }
            
            try
            {
                if (Form.ActiveForm != null)
                {
                    Form.ActiveForm.Hide();
                }                
            }
            catch  
            { }

            try
            {
                FormControl.CloseApp_Preparation();
            }
            catch 
            { }

            try
            {
                FormControl.Form_settings.ButtonDisconnectALL_Click(null, null);
                Application.DoEvents();
                Application.DoEvents();
                Application.DoEvents();
            }
            catch  
            { }

            Thread.Sleep(500);
            Application.Exit();
            Process.GetCurrentProcess().Kill();

        }

    }
}