using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;

namespace KontrolaKadi
{
    public partial class Gui_MalaPec : Form
    {
        Prop1 p1 = Val.logocontroler.Prop1; 
        Prop2 p2 = Val.logocontroler.Prop2;

        Prop1 prop = Val.logocontroler.Prop1;

        GuiLink guiLink;
       

        public Gui_MalaPec()
        {
            InitializeComponent();           
            FormatTopPanel();
            FormClosed += Gui_MalaPec_FormClosed;
            Resize += Gui_MalaPec_Resize;
            SetupForm();
            Load += Gui_MalaPec_Load;            
        }
 
        private void Gui_MalaPec_Load(object sender, EventArgs e)
        {           
            registerEvents();
            guiLink = new GuiLink(this);            
        }



        private void Gui_MalaPec_FormClosed(object sender, FormClosedEventArgs e)
        {
            Helper.ExitApp();
        }
              
        private void FormatTopPanel()
        {            
            panelTop.Width = Width;   
            positionBtnSettings();

        }

        void positionBtnSettings()
        {
            btnSettings.Left = panelTop.Right - btnSettings.Width - 50;
        }

        private void ReFormatTopPannel()
        {
            panelTop.Width = Width;
            positionBtnSettings();
        }

        private void SetupForm()
        {
            DoubleBuffered = true;
            Shown += Gui_MalaPec_Shown;
        }

        private void Gui_MalaPec_Shown(object sender, EventArgs e)
        {
            FormControl.Form_settings.Hide();
        }

        private void Gui_MalaPec_Resize(object sender, EventArgs e)
        {
            ReFormatTopPannel();           
        }


        void registerEvents()
        {
            btnSettings.Click += BtnSettings_Click;
        }

        private void BtnSettings_Click(object sender, EventArgs e)
        {
            FormControl.ShowForm_Settings();
            this.Hide();

        }

        string TimeToString(TimeSpan val)
        {
            return val.ToString("c");
        }

        private void warningManager1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Gui_MalaPec_Load_1(object sender, EventArgs e)
        {

        }

        private void enojnaKad1_Load(object sender, EventArgs e)
        {

        }
    }

    class SysTimer : System.Timers.Timer
    {
        public SysTimer() : base()
        {

        }
        public SysTimer(double interval) : base(interval)
        {

        }
    }
}
