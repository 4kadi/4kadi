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
        public static void Unfocus(object sender)
        {
            try
            {                
                object buff = sender; // store to buffer
                Form form;
          
                while (true)
                {
                    if (IsDerivedFromForm(buff))
                    {
                        form = (Form)buff;
                        break; // parent form has been found - break while loop and continue
                    }
                    else if (IsDerivedFromControl(buff))
                    {
                        var control = (Control)buff; // Control was found
                        buff = control.Parent; // Store its parent to buffer - next loop will check if its a parent form
                    }
                    else
                    {
                        return; // terminate this operation - parent form wasnt found
                    }
                }


                var arrayOfControls = form.Controls.Find("unfocus", true); // finds a dummy control that is used to revert focus to, so the cursor doesnt blink in desired textboxes
                TextBox tb;
                if (arrayOfControls.Length == 0)
                {
                    tb = new TextBox() // if it doesnt exsist creates one
                    {
                        Height = 0,
                        Width = 0,
                        Name = "unfocus",
                        Top = 0,
                        Left = 0
                    };
                    form.Controls.Add(tb);
                }
                else
                {
                    tb = (TextBox)arrayOfControls[0];
                }
                
                tb.Focus();
            }
            catch (Exception ex)
            { }
        }

        public static bool IsDerivedFromForm(object obj)
        {
            Type type = obj.GetType();
            while (type != null)
            {
                if (type == typeof(Form))
                {
                    return true;
                }
                type = type.BaseType;
            }
            return false;
        }

        public static bool IsDerivedFromControl(object obj)
        {
            Type type = obj.GetType();

            while (type != null)
            {
                if (type == typeof(Control))
                {
                    return true;
                }
                type = type.BaseType;
            }
            return false;
        }

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