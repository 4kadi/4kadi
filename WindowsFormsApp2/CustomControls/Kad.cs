using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KontrolaKadi
{
    class Kad : SPanel
    {        
        private string _displayName;

        public string DisplayName
        {
            get { return _displayName; }
            set 
            {
                if (value == null)
                {
                    return;
                }
                _displayName = value;
                displayNameChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public CenteredLabel lblNextMove = new CenteredLabel();

        public EventHandler displayNameChanged;      

        public SPanel backImage = new SPanel();  
        public readonly int backImageHeight = 137;

        Rectangle outline;
        Pen outlinePen = new Pen(Color.Black, 1);
     
        bool mouseOver = false;
        bool designMode;

        InfoPanel infoIconPanel = new InfoPanel();
               
        public KadSubmenu submenu;

        public Urnik Urnik;     // just reference from submenu
        public SwitchesGroupbox SwitchesGroupbox; // just reference from submenu
        public PowerSetGroupbox PowerSetGroupbox; // just reference from submenu
        public PowerMonitorGroupBox PowerMonitorGroupbox; // just reference from submenu
        public PVSelector PvSelector;    // just reference from submenu
        public PhControler Ph;    // just reference from submenu

        private PlcVars.DWord nameOfKad;

        public PlcVars.DWord NameOfKad
        {
            get { return nameOfKad; }
            set 
            {
                if (value == null)
                { return; }
                nameOfKad = value;
                nameOfKad.ValueChanged += NameOfKad_ValueChanged;
            }
        }

        private void NameOfKad_ValueChanged(object sender, EventArgs e)
        {
            var dn = StringDoubleWordConverter.DecodeDoubleWordToString(nameOfKad.Value_int);
            DisplayName = dn;
        }

        public Kad():base()
        {
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true); // Prevents flicker

            designMode = (LicenseManager.UsageMode == LicenseUsageMode.Designtime);

            Height = 700;
            Width = 250;

            ManageBackImage();            

            if (designMode == true)
            {
                return;
            }
                        
            manageInfoIconsPanel();
            manageNextMoveLable();

            Paint += Kad_Paint;
            
            XmlController.XmlChanged += RefreshFromXml;
          
            HandleCreated += Kad_HandleCreated;
        }
        

        private void Kad_HandleCreated(object sender, EventArgs e)
        {
            RefreshFromXml(this, EventArgs.Empty);

            submenu = new KadSubmenu()
            {            
                Width = 1500,
                Height = 1100,
            };
            submenu.Show(); submenu.Hide(); //triggers the load of subsequent components

            Urnik = submenu.Urnik;
            SwitchesGroupbox = submenu.SwitchesGroupbox;
            PowerSetGroupbox = submenu.PowerSetGroupbox;
            PowerMonitorGroupbox = submenu.PowerMonitorGroupBox;
            PvSelector = submenu.PVSelector;
            Ph = submenu.PhControler;

            backImage.MouseUp += BackImage_MouseUp;
            Urnik.NextEventDescription_Changed += Urnik_NextEventDescription_Changed;
        }

        private void BackImage_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                showSubmenu();
            }

            if (e.Button == MouseButtons.Right)
            {
                changeDisplayName();
            }
        }

        void changeDisplayName()
        {
            using (var form = new InputForm(this.DisplayName))
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string val = form.InputValue;
                    // You might want to validate the input before assigning
                    if (val.Length <= 4)
                    {
                        if (NameOfKad == null)
                        {
                            throw new Exception("Internal error: You forgot to set NameOfKad property and link it to PLC value");
                        }
                        var buff = StringDoubleWordConverter.EncodeStringToDoubleWord(val);
                        NameOfKad.Value_int = buff;
                    }
                    else
                    {                    
                        MessageBox.Show("Dolžina imena mora biti največ 4 znake");                                              
                    }
                }
            }
        }

        void showSubmenu()
        {
            submenu.Show();
            submenu.BringToFront();
            submenu.WindowState = FormWindowState.Normal;
        }

        private void manageNextMoveLable()
        {
            lblNextMove.Dock = DockStyle.None;
            lblNextMove.Top = 270;
            lblNextMove.Left = 25;
            lblNextMove.Width = 200;
            lblNextMove.Height = 25;
            lblNextMove.Text = "";
            Controls.Add(lblNextMove);
        }

              
        private void Urnik_NextEventDescription_Changed(object sender, EventArgs e)
        {
            Urnik_NextEventDescription_Changed();
        }

        void Urnik_NextEventDescription_Changed()
        {
            if (InvokeRequired)
            {
                var m = new MethodInvoker(Urnik_NextEventDescription_Changed);
                Invoke(m);
                return;
            }

            lblNextMove.Text = Urnik.NextEventDescription;
        }

        void RefreshFromXml(object sender, EventArgs e)
        {
            try
            {
                var m = new MethodInvoker(delegate 
                { 
                    //
                });
                FormControl.Gui.Invoke(m);
                
            }
            catch (Exception ex)
            {
                throw new Exception("Internal error inside event RefreshFromXml. Error details: " + ex.Message);
            }                         
        }

        void manageInfoIconsPanel()
        {
            infoIconPanel.Top = backImage.Bottom + 50;
            infoIconPanel.BackColor = Color.LightGray;
            infoIconPanel.Left = ((Width - infoIconPanel.Width) / 2);
            Controls.Add(infoIconPanel);
        }
                           
        private void Kad_Paint(object sender, PaintEventArgs e)
        {
            if (mouseOver)
            {
                DrawOutlineOnMouseOver(e);
            }
        }

        void ManageBackImage()
        {
            backImage.Top = 80;
            backImage.Height = 180;
            backImage.BackgroundImageLayout = ImageLayout.Stretch;             

            if (designMode == true)
            {
                return;
            }

            backImage.MouseEnter += BackImage_MouseEnter;
            backImage.MouseLeave += BackImage_MouseLeave;
        }

        void DrawOutlineOnMouseOver(PaintEventArgs e)
        {
            outline = new Rectangle(new Point(backImage.Left, backImage.Top), new Size(backImage.Width, backImage.Height));            
            outline.Inflate(2,2);
            e.Graphics.DrawRectangle(outlinePen, outline);
        }

        private void BackImage_MouseEnter(object sender, EventArgs e)
        {
            mouseOver = true;
            Invalidate();
        }
        private void BackImage_MouseLeave(object sender, EventArgs e)
        {
            mouseOver = false;
            Invalidate();
        }

    }


    class EnojnaKad : Kad
    {
        Color BarvaKadi = Color.Red;
        SPanel rectBarvaKadi;

        KadNaslov lblKadNaslov = new KadNaslov();

        public EnojnaKad() : base()
        {
            backImage.BackgroundImage = Properties.Resources.Kad___konstrukcija;
            backImage.Width = 100;
            backImage.Left = ((base.Width - backImage.Width) / 2) -5;
            base.Controls.Add(backImage);
            barvajKad();
            backImage.Controls.Add(rectBarvaKadi);            
            rectBarvaKadi.Controls.Add(lblKadNaslov);        
            this.Paint += EnojnaKad_Paint;
            base.displayNameChanged += displayName;               

        }

        void displayName(object sender, EventArgs e)
        {
            if (!IsHandleCreated || DisplayName == null)
            {
                return;
            }

            MethodInvoker m = new MethodInvoker(delegate 
            {
                lblKadNaslov.Text = DisplayName; 
            });
            Invoke(m);
        }
        
        private void EnojnaKad_Paint(object sender, PaintEventArgs e)
        {
           
        }

        void barvajKad()
        {
            rectBarvaKadi = new SPanel()
            {
                Top = 35,
                Left = 10,
                Width = 80,
                Height = 143,
                BackColor = BarvaKadi,
                Enabled = false
                
            };
            
        }
    }
    public partial class InputForm : Form
    {
        public string InputValue { get; private set; }
        Button confirmation;

        public InputForm(string currentName)
        {
            // Set up the form, add a Label, a TextBox, and a Button
            this.Text = "Spremeni ime kadi";
            this.Size = new Size(300, 150);

            Label label = new Label() { Left = 50, Top = 20, Text = "Ime:" };
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 200 };
            textBox.Text = currentName;
            confirmation = new Button() { Text = "Ok", Left = 100, Width = 100, Top = 80 };
            confirmation.Click += (sender, e) => { InputValue = textBox.Text; this.DialogResult = DialogResult.OK; };

            this.Controls.Add(label);
            this.Controls.Add(textBox);
            this.Controls.Add(confirmation);
            textBox.KeyDown += TextBox_KeyDown;
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                confirmation.PerformClick();
            }
        }
    }

    public class StringDoubleWordConverter
    {
        // Encodes up to the first 4 characters of a string to a double word (32-bit integer)
        public static int EncodeStringToDoubleWord(string input)
        {
            // Ensure the string is exactly 4 characters long by padding with '\0' (NULL) if it's shorter
            input = input.PadRight(4, '\0');

            int encodedValue = 0;
            for (int i = 0; i < 4; i++)
            {
                // '\0' will simply add no value in the corresponding byte
                encodedValue |= input[i] << (24 - i * 8);
            }
            return encodedValue;
        }

        // Decodes a double word (32-bit integer) to a 4-character string
        public static string DecodeDoubleWordToString(int doubleWord)
        {
            char[] chars = new char[4];
            for (int i = 0; i < 4; i++)
            {
                chars[i] = (char)((doubleWord >> (24 - i * 8)) & 0xFF);
            }
            // Create a string from the char array and then trim the NULL characters at the end
            return new string(chars).TrimEnd('\0');
        }
    }

}
