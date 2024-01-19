using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KontrolaKadi
{
    public class PVSelector : SGroupBox
    {
        bool initialized = false;

        private PlcVars.Word _T1;

        public PlcVars.Word T1
        {
            get { return _T1; }
            set
            {
                _T1 = value;
                Any_ValueChanged(null, EventArgs.Empty);
            }
        }

        private PlcVars.Word _T2;

        public PlcVars.Word T2
        {
            get { return _T2; }
            set 
            {
                _T2 = value;
                Any_ValueChanged(null, EventArgs.Empty);
            }
        }

        private PlcVars.Word _PV;

        public PlcVars.Word PV
        {
            get { return _PV; }
            set 
            { 
                _PV = value;
                Any_ValueChanged(null, EventArgs.Empty);
            }
        }

        private PlcVars.Word _Function;

        public PlcVars.Word Function
        {
            get { return _Function; }
            set 
            { 
                _Function = value;
                Any_ValueChanged(null, EventArgs.Empty);
            }
        }

        TbTemperatureShow tbT1, tbT2, tbPV;
        Label lblT1, lblT2, lblPV, lblCb;
        CB_PVselector cbSelector;

        const int lbloffsetY = 6;

        public PVSelector()
        {
            Font = new Font("arial", 15, FontStyle.Bold);

            tbT1 = new TbTemperatureShow()
            {
                Top = 35,
                Left = 50
            };
            Controls.Add(tbT1);

            tbT2 = new TbTemperatureShow()
            {
                Top = tbT1.Bottom + 15,
                Left = tbT1.Left
            };
            Controls.Add(tbT2);

            cbSelector = new CB_PVselector()
            {
                Top = tbT1.Top,
                Left = tbT1.Left + tbT1.Width + 120
            };
            Controls.Add(cbSelector);

            tbPV = new TbTemperatureShow()
            {
                Top = tbT2.Top,
                Left = cbSelector.Left
            };
            Controls.Add(tbPV);

            //
            lblT1 = new Label()
            {
                Font = new Font("arial", 10),
                Top = tbT1.Top + lbloffsetY,
                Left = tbT1.Left - 30,
                Text = "T1:"
            };
            Controls.Add(lblT1);

            lblT2 = new Label()
            {
                Font = new Font("arial", 10),
                Top = tbT2.Top + lbloffsetY,
                Left = lblT1.Left,
                Text = "T2:"
            };
            Controls.Add(lblT2);

            lblPV = new Label()
            {
                Font = new Font("arial", 10),
                Top = tbPV.Top + lbloffsetY,
                Left = tbPV.Left - 90,
                Text = "Upoštevano:"
            };
            Controls.Add(lblPV);

            lblCb = new Label()
            {
                Font = new Font("arial", 10),
                Top = lblT1.Top + lbloffsetY -2,
                Left = lblPV.Left +25,
                Text = "Izbira:"
            };
            Controls.Add(lblCb);

            Width = 450;
            Height = 150;
        }

        private void CbSelector_DropDownClosed(object sender, EventArgs e)
        {
            if (!initialized)
            {
                return;
            }
            Function.Value_short = (short)cbSelector.SelectedValue;
        }

        private void Any_ValueChanged(object sender, EventArgs e)
        {
            Any_ValueChanged();
        }

        void Any_ValueChanged()
        {
            InitializeIfPossible();

            if (!initialized)
            {
                return;
            }

            if (InvokeRequired)
            {
                var m = new MethodInvoker(Any_ValueChanged);
                Invoke(m);
                return;
            }

            tbT1.Text = T1.Value_short.ToString();
            tbT2.Text = T2.Value_short.ToString();
            tbPV.Text = PV.Value_short.ToString();

            cbSelector.SelectedValue = Function.Value_short;
        }


        void InitializeIfPossible()
        {
            if (initialized)
            {
                return;
            }

            if (T1 != null && T2 != null && PV != null && Function != null)
            {
                T1.ValueChanged += Any_ValueChanged;
                T2.ValueChanged += Any_ValueChanged;
                PV.ValueChanged += Any_ValueChanged;
                Function.ValueChanged += Any_ValueChanged;

                cbSelector.DropDownClosed += CbSelector_DropDownClosed;                

                initialized = true;
            }
        }

        class TbTemperatureShow : TextBox
        {
            string Unit = "°C";
            override public string Text
            {
                get { return base.Text.Replace(Unit, ""); }
                set { UpdateText(value); }
            }
            public TbTemperatureShow()
            {
                ReadOnly = true;
                Width = 80;
                Enter += TemperatureShow_Enter;
            }

            void UpdateText(string text)
            {
                var m = new MethodInvoker(delegate
                {
                    var n = Convert.ToDouble(text);
                    n = n / 10;
                    base.Text = n.ToString("0.0") + Unit;
                });

                Invoke(m);
            }

            private void TemperatureShow_Enter(object sender, EventArgs e)
            {
                try
                {
                    var form = FindForm();
                    var arrayOfControls = form.Controls.Find("unfocus", true);
                    var tb = (TextBox)arrayOfControls[0];
                    tb.Focus();
                }
                catch
                { }
            }
        }

        public class CB_PVselector : ComboBox
        {
            public CB_PVselector()
            {
                values();
                this.DropDownStyle = ComboBoxStyle.DropDownList;

            }



            void values()
            {
                var list = new List<CB_PVselector_Item>();
                list.Add(new CB_PVselector_Item("Povprečje", 0));
                list.Add(new CB_PVselector_Item("T1", 1));
                list.Add(new CB_PVselector_Item("T2", 2));
                list.Add(new CB_PVselector_Item("Manjša", 3));
                list.Add(new CB_PVselector_Item("Večja", 4));
                this.DataSource = list;
                DisplayMember = "Text";
                ValueMember = "Value";
            }
        }

        class CB_PVselector_Item
        {
            public string Text { get; set; }
            public short Value { get; set; }

            public CB_PVselector_Item(string text, short value)
            {
                Text = text;
                Value = value;                
            }

            public override string ToString()
            {
                return Text;
            }
        }
    }
}
