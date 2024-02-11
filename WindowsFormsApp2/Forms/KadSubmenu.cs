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
    public partial class KadSubmenu : Form
    {
        public int ID { get; set; } = 0;        
        public Urnik Urnik { get; private set; }
        public SwitchesGroupbox SwitchesGroupbox { get; private set; }
        public PowerSetGroupbox PowerSetGroupbox { get; private set; }
        public PowerMonitorGroupBox PowerMonitorGroupBox { get; private set; }
        public PVSelector PVSelector { get; private set; }

        public PhControler PhControler { get; private set; }

        public KadSubmenu()
        {
            bool designMode = (LicenseManager.UsageMode == LicenseUsageMode.Designtime);
            InitializeComponent();

            if (designMode)
            {
                return;
            }

            manageUrnik();
            manageSwitchesGroupbox();
            managePowerSetGroupbox();
            managePowerMonitorGroupbox();
            managePVSelectorGroupBox();
            managePhGroupBox();

            Click += KadSubmenu_Click;
            LostFocus += KadSubmenu_LostFocus;
            FormClosing += KadSubmenu_FormClosing;          
        }

        private void KadSubmenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            close();
        }

        private void KadSubmenu_LostFocus(object sender, EventArgs e)
        {
            close();
        }

        private void KadSubmenu_Click(object sender, EventArgs e)
        {
            var ee = (MouseEventArgs)e;
            var btn = ee.Button;

            if (ee.Button == MouseButtons.Right)
            {
                close();
            }
        }

        void manageUrnik()
        {
            Urnik = new Urnik()
            {
                Text = "Urniki",
                Top = 10,
                Left = 10
            };
            Controls.Add(Urnik);
        }
        void manageSwitchesGroupbox()
        {
            SwitchesGroupbox = new SwitchesGroupbox()
            {
                Top = 10,
                Left = 500
            };
            Controls.Add(SwitchesGroupbox);
        }

        void managePowerSetGroupbox()
        {
            PowerSetGroupbox = new PowerSetGroupbox()
            {
                Text = "Nastavitve moči",
                Top = 260,
                Left = 1100                
            };
            Controls.Add(PowerSetGroupbox);
        }

        void managePowerMonitorGroupbox()
        {
            PowerMonitorGroupBox = new PowerMonitorGroupBox()
            {
                Text = "Poraba",
                Top = 10,
                Left = 1100
            };
            Controls.Add(PowerMonitorGroupBox);
        }

        void managePVSelectorGroupBox()
        {
            PVSelector = new PVSelector()
            {
                Top = Urnik.Bottom + 15,
                Left = Urnik.Left, 
                Text = "Procesna temperatura"
            };
            Controls.Add(PVSelector);
        }

        void managePhGroupBox()
        {
            PhControler = new PhControler()
            {
                Top = PVSelector.Bottom + 15,
                Left = Urnik.Left,
                Text = "Ph"
            };
            Controls.Add(PhControler);
                                  
        }

        void close()
        {
            Hide();
        }
    }

    public class SwitchesGroupbox : SGroupBox
    {
        private int posX = 15; // Set the starting X position for all switches
        private int posX2 = 270; // Set the starting X position for all switches
        private int posY = 30; // Set the starting Y position for the first switch
        private int posY_dynamic = 0;
        private int spacing = 85; // Set the vertical spacing between switches

        public AutoManSwitch Prisotnost { get; private set; }
        public AutoManSwitch Grelec1 { get; private set; }
        public AutoManSwitch Grelec2 { get; private set; }
        public AutoManSwitch ČrpalkaNivo { get; private set; }
        public AutoManSwitch ČrpalkaFilter { get; private set; }
        public AutoManSwitch ČrpalkaČasovnoDolivanje { get; private set; }
        public AutoManSwitch ČrpalkaHlajenje { get; private set; }
        public AutoManSwitch TlačniSezorFiltra { get; private set; }
        public AutoManSwitch MixVentilPlus { get; private set; }
        public AutoManSwitch MixVentilMinus { get; private set; }
        public AutoManSwitch NivoVisok { get; private set; }
        public AutoManSwitch NivoNizek { get; private set; }
        public AutoManSwitch VentilZrak { get; private set; }
        public AutoManSwitch VentilPokrov { get; private set; }
        public AutoManSwitch VentilVoda { get; private set; }
        public AutoManSwitch FlowSwitch { get; private set; }

        public List<Control> ControlList { get; private set; }

        public SwitchesGroupbox()
        {
            ControlList = new List<Control>();
            Text = "Stanje Procesa";
            Font = new Font("arial", 15, FontStyle.Bold);
            posY_dynamic = posY;
            CreateSwitches();
        }

        private void CreateSwitches()
        {
            Prisotnost = InitializeSwitch("Prisotnost", posX, CurrentPosY()); AddControlIfItIsUsed(Prisotnost);
            VentilVoda = InitializeSwitch("Ventil Voda", posX2, CurrentPosY()); AddControlIfItIsUsed(VentilVoda);

            Grelec1 = InitializeSwitch("Grelec 1", posX, NextPosY()); AddControlIfItIsUsed(Grelec1);
            Grelec2 = InitializeSwitch("Grelec 2", posX2, CurrentPosY()); AddControlIfItIsUsed(Grelec2);

            ČrpalkaNivo = InitializeSwitch("Črpalka Nivo", posX, NextPosY()); AddControlIfItIsUsed(ČrpalkaNivo);
            ČrpalkaFilter = InitializeSwitch("Črpalka Filter", posX2, CurrentPosY()); AddControlIfItIsUsed(ČrpalkaFilter);
            ČrpalkaČasovnoDolivanje = InitializeSwitch("Črpalka Časovno Dolivanje", posX, NextPosY()); AddControlIfItIsUsed(ČrpalkaČasovnoDolivanje);
            ČrpalkaHlajenje = InitializeSwitch("Črpalka Hlajenje", posX2, CurrentPosY()); AddControlIfItIsUsed(ČrpalkaHlajenje);

            TlačniSezorFiltra = InitializeSwitch("Tlačni Sezor Filtra", posX, NextPosY()); AddControlIfItIsUsed(TlačniSezorFiltra);
            FlowSwitch = InitializeSwitch("Flow Switch", posX2, CurrentPosY()); AddControlIfItIsUsed(FlowSwitch);

            MixVentilPlus = InitializeSwitch("Mix Ventil +", posX, NextPosY()); AddControlIfItIsUsed(MixVentilPlus);
            MixVentilMinus = InitializeSwitch("Mix Ventil -", posX2, CurrentPosY()); AddControlIfItIsUsed(MixVentilMinus);

            NivoVisok = InitializeSwitch("Nivo Visok", posX, NextPosY()); AddControlIfItIsUsed(NivoVisok);
            NivoNizek = InitializeSwitch("Nivo Nizek", posX2, CurrentPosY()); AddControlIfItIsUsed(NivoNizek);

            VentilZrak = InitializeSwitch("Ventil Zrak", posX, NextPosY()); AddControlIfItIsUsed(VentilZrak);
            VentilPokrov = InitializeSwitch("Ventil Pokrov", posX2, CurrentPosY()); AddControlIfItIsUsed(VentilPokrov);


            Height = NextPosY() + 20;
            Width = Prisotnost.Width * 2 + 50;

            //
            Controls.AddRange(ControlList.ToArray());
        }

        private int NextPosY()
        {
            posY_dynamic += spacing;
            return posY_dynamic;
        }
        private int CurrentPosY()
        {
            return posY_dynamic;
        }


        void AddControlIfItIsUsed(Control c)
        {
            if (true)
            {
                ControlList.Add(c);
            }

        }

        private AutoManSwitch InitializeSwitch(string text, int left, int top)
        {
            return new AutoManSwitch()
            {
                Text = text,
                Left = left,
                Top = top
            };
        }
    }

    public class PowerSetGroupbox : SGroupBox
    {
        int YstartPos = 30;
        int Yspacing = 70;
        int YcurrentPos = 0;
        int Xpos = 15;

        public GrelnikPowerSet PowerSetGrelnik1 { get; private set; }
        public GrelnikPowerSet PowerSetGrelnik2 { get; private set; }
        public CrpalkaPowerSet PowerSetCrpalkaNivo { get; private set; }
        public CrpalkaPowerSet PowerSetCrpalkaFilter { get; private set; }
        public CrpalkaPowerSet PowerSetCrpalkaCasovna { get; private set; }
        public CrpalkaPowerSet PowerSetCrpalkaHlajenje { get; private set; }
        public GrelnikPowerSet PowerSetRezerva { get; private set; }

        public PowerSetGroupbox()
        {
            Font = new Font("arial", 15, FontStyle.Bold);
            YcurrentPos = YstartPos;


            PowerSetGrelnik1 = new GrelnikPowerSet()
            {
                Text = "Grelnik 1",
                Top = CurrentYPosition(),
                Left = Xpos
            };
            Controls.Add(PowerSetGrelnik1);

            PowerSetGrelnik2 = new GrelnikPowerSet()
            {
                Text = "Grelnik 2",
                Top = NextYPosition(),
                Left = Xpos
            };
            Controls.Add(PowerSetGrelnik2);

            PowerSetCrpalkaNivo = new CrpalkaPowerSet()
            {
                Text = "Nivojska črpalka",
                Top = NextYPosition(),
                Left = Xpos
            };
            Controls.Add(PowerSetCrpalkaNivo);

            PowerSetCrpalkaFilter = new CrpalkaPowerSet()
            {
                Text = "Filtracijska črpalka",
                Top = NextYPosition(),
                Left = Xpos
            };
            Controls.Add(PowerSetCrpalkaFilter);

            PowerSetCrpalkaCasovna = new CrpalkaPowerSet()
            {
                Text = "Časovna črpalka",
                Top = NextYPosition(),
                Left = Xpos
            };
            Controls.Add(PowerSetCrpalkaCasovna);

            PowerSetCrpalkaHlajenje = new CrpalkaPowerSet()
            {
                Text = "Črpalka hlajenja",
                Top = NextYPosition(),
                Left = Xpos
            };
            Controls.Add(PowerSetCrpalkaHlajenje);

            PowerSetRezerva = new GrelnikPowerSet()
            {
                Text = "Rezerva",
                Top = NextYPosition(),
                Left = Xpos
            };
            Controls.Add(PowerSetRezerva);

            Width = PowerSetGrelnik1.Width + 30;
            Height = PowerSetRezerva.Bottom + 15;
        }

        int NextYPosition()
        {
            YcurrentPos += Yspacing;
            return YcurrentPos;
        }

        int CurrentYPosition()
        {
            return YcurrentPos;
        }
    }

    public class PowerMonitorGroupBox : SGroupBox
    {
        int YstartPos = 30;
        int Yspacing = 70;
        int YcurrentPos = 0;
        int Xpos = 15;

        public PowerSet PorabaSkupna { get; private set; }
        public PowerSet OmejitevPorabaSkupna { get; private set; }
        public PowerSet PorabaTeKadi { get; private set; }


        public PowerMonitorGroupBox()
        {
            Font = new Font("arial", 15, FontStyle.Bold);
            YcurrentPos = YstartPos;

            PorabaSkupna = new PowerSet()
            {
                ReadOnly = true,
                Left = Xpos,
                Text = "Skupna poraba",
                Top = CurrentYPosition()
            };
            Controls.Add(PorabaSkupna);

            OmejitevPorabaSkupna = new PowerSet()
            {
                ReadOnly = true,
                Left = Xpos,
                Text = "Omejitev skupne porabe",
                Top = NextYPosition()
            };
            Controls.Add(OmejitevPorabaSkupna);

            PorabaTeKadi = new PowerSet() 
            { 
                ReadOnly = true,
                Left = Xpos,
                Text = "Poraba te kadi",
                Top = NextYPosition()
            };
            Controls.Add(PorabaTeKadi);

            Width = PorabaSkupna.Width + 30;
            Height = PorabaTeKadi.Bottom + 15;

        }

        int NextYPosition()
        {
            YcurrentPos += Yspacing;
            return YcurrentPos;
        }

        int CurrentYPosition()
        {
            return YcurrentPos;
        }
    }
}
