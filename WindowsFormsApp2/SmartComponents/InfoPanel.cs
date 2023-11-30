using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KontrolaKadi
{
    public class InfoPanel : SPanel
    {
        InfopanelComponent_Prisotnost prisotnost;
        InfopanelComponent_Nivo nivo;
        InfopanelComponent_Rezim rocniNacin;
        InfopanelComponent_Napaka napaka;
        InfopanelComponent_Pregretje pregretje;
        InfopanelComponent_Grelnik grelnik;

        public readonly int infoIconSize = InfoPanelBase.infoIconSize;
        public readonly int spacing = 5;
        int numberOfIcons = 6;
        public static bool debugIcons = true;

        public InfoPanel()
        {
            prisotnost = new InfopanelComponent_Prisotnost() {Left = spacing };
            nivo = new InfopanelComponent_Nivo() { Left = prisotnost.Right + spacing};
            rocniNacin = new InfopanelComponent_Rezim() { Left = nivo.Right + spacing };
            napaka = new InfopanelComponent_Napaka() { Left = rocniNacin.Right + spacing };
            pregretje = new InfopanelComponent_Pregretje() { Left = napaka.Right + spacing };
            grelnik = new InfopanelComponent_Grelnik() { Left = pregretje.Right + spacing };

            Controls.Add(prisotnost);
            Controls.Add(nivo);
            Controls.Add(rocniNacin);
            Controls.Add(napaka);
            Controls.Add(pregretje);
            Controls.Add(grelnik);

            Width = infoIconSize * numberOfIcons + spacing * (numberOfIcons +2);
            Height = infoIconSize + 10;

            this.Paint += InfoPanel_Paint;

        }

        private void InfoPanel_Paint(object sender, PaintEventArgs e)
        {
            paintOutline(e);
        }

        void paintOutline(PaintEventArgs e)
        {
            
        }
    }

    public class InfoPanelBase : SPanel
    {       
        public static readonly int infoIconSize = 30;
        public MyTimer AutoUpdater;


        public InfoPanelBase() : base()
        {
            Top = 5;
            AutoUpdater = new MyTimer();
            AutoUpdater.AutoReset = true;        
            AutoUpdater.Start();
        }
    }

    public class InfopanelComponent_Prisotnost : InfoPanelBase
    {
        Bitmap prisotno;    
        public PlcVars.Bit valueHolder;
        public InfopanelComponent_Prisotnost()
        {
            prisotno = Properties.Resources.prisotnost; 
            BackgroundImageLayout = ImageLayout.Stretch;            
            Width = infoIconSize;
            Height = infoIconSize;
            AutoUpdater.Elapsed += AutoUpdater_Elapsed;
        }

        private void AutoUpdater_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (InfoPanel.debugIcons)
            {
                BackgroundImage = prisotno;
            }

            if (valueHolder == null)
            {
                return;
            }

            if (valueHolder.Value_bool == true)
            {
                BackgroundImage = prisotno;
            }
            else
            {
                BackgroundImage = null;
            }
        }
    }

    public class InfopanelComponent_Nivo : InfoPanelBase
    {
        Bitmap nivoPresezen;     
        public PlcVars.Bit valueHolder;
        public InfopanelComponent_Nivo()
        {
            nivoPresezen = Properties.Resources.SYTK_R_nivo;
            BackgroundImageLayout = ImageLayout.Stretch;
            Width = infoIconSize;
            Height = infoIconSize;
            AutoUpdater.Elapsed += AutoUpdater_Elapsed;
        }

        private void AutoUpdater_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (InfoPanel.debugIcons)
            {
                BackgroundImage = nivoPresezen;
            }

            if (valueHolder == null)
            {
                return;
            }

            if (valueHolder.Value_bool == true)
            {
                BackgroundImage = nivoPresezen;
            }
            else
            {
                BackgroundImage = null;
            }
        }
    }

    public class InfopanelComponent_Rezim : InfoPanelBase
    {
        Bitmap rocno;
        Bitmap urnikAktiv, urnikPasiv;
        Bitmap izklop;
        public PlcVars.Bit valueHolder_UrnikAktiven;
        public PlcVars.Bit valueHolder_Rocno;
        public PlcVars.Bit valueHolder_Off;

        public InfopanelComponent_Rezim() : base()
        {
            rocno = Properties.Resources.manual_ctrl;
            urnikAktiv = Properties.Resources.ura_zelena;
            urnikPasiv = Properties.Resources.ura_rumena;
            izklop = Properties.Resources.off;

            BackgroundImageLayout = ImageLayout.Stretch;
            Width = infoIconSize;
            Height = infoIconSize;
            AutoUpdater.Elapsed += AutoUpdater_Elapsed;
        }

        private void AutoUpdater_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (InfoPanel.debugIcons)
            {
                BackgroundImage = rocno;
            }

            if (izklop == null)
            {
                return;
            }

            if (valueHolder_Off != null && valueHolder_Off.Value_bool)
            {
                BackgroundImage = izklop;
            }
            else if (valueHolder_Rocno != null && valueHolder_Rocno.Value_bool)
            {
                BackgroundImage = rocno;
            }
            else if (valueHolder_UrnikAktiven != null &&valueHolder_UrnikAktiven.Value_bool)
            {
                BackgroundImage = urnikAktiv;
            }
            else
            {
                BackgroundImage = urnikPasiv;
            }
        }
    }

    public class InfopanelComponent_Napaka : InfoPanelBase
    {
        Bitmap error;
        Bitmap warning;
        public PlcVars.Bit errorvalueHolder;
        public PlcVars.Bit warningvalueHolder;
        public InfopanelComponent_Napaka() : base()
        {
            error = Properties.Resources.warning_icon_png_1;
            warning = Properties.Resources.warning_icon_24;
            BackgroundImageLayout = ImageLayout.Stretch;
            Width = infoIconSize;
            Height = infoIconSize;
            AutoUpdater.Elapsed += AutoUpdater_Elapsed;
        }

        private void AutoUpdater_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (InfoPanel.debugIcons)
            {
                BackgroundImage = error;
            }

            if (errorvalueHolder != null)
            {
                if (errorvalueHolder.Value_bool == true)
                {
                    BackgroundImage = error;                    
                }
            }
            else if (warningvalueHolder != null)
            {
                if (warningvalueHolder.Value_bool == true)
                {
                    BackgroundImage = warning;
                }
            }
        }
    }

    public class InfopanelComponent_Pregretje : InfoPanelBase
    {
        Bitmap pregreto;
        public PlcVars.Bit valueHolder;
        public InfopanelComponent_Pregretje() : base()
        {
            pregreto = Properties.Resources.overheat;           
            BackgroundImageLayout = ImageLayout.Stretch;
            Width = infoIconSize;
            Height = infoIconSize;
            AutoUpdater.Elapsed += AutoUpdater_Elapsed;
        }

        private void AutoUpdater_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (InfoPanel.debugIcons)
            {
                BackgroundImage = pregreto;
            }

            if (valueHolder == null)
            {
                return;
            }

            if (valueHolder.Value_bool == true)
            {
                BackgroundImage = pregreto;
            }
            else
            {
                BackgroundImage = null;
            }
        }
    }

    public class InfopanelComponent_Grelnik : InfoPanelBase
    {
        Bitmap grelecAktiven;
        public PlcVars.Bit valueHolder;
        public InfopanelComponent_Grelnik() : base()
        {
            grelecAktiven = Properties.Resources.SYTK_grelec;
            BackgroundImageLayout = ImageLayout.Stretch;
            Width = infoIconSize;
            Height = infoIconSize;
            AutoUpdater.Elapsed += AutoUpdater_Elapsed;
        }

        private void AutoUpdater_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (InfoPanel.debugIcons)
            {
                BackgroundImage = grelecAktiven;
            }

            if (valueHolder == null)
            {
                return;
            }

            if (valueHolder.Value_bool == true)
            {
                BackgroundImage = grelecAktiven;
            }
            else
            {
                BackgroundImage = null;
            }
        }
    }
}
