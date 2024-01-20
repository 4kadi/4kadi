using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KontrolaKadi
{
    class OgrevenjeSet :SGroupBox
    {
        Label lbl1;
        CB_OgrevenjeSetselector cbSelector;


        public OgrevenjeSet()
        {
            Text = "Način";

            lbl1 = new Label()
            { 
                Text = "Izbira režima:",
                Left = 10,
                Top = 15
            };
            Controls.Add(lbl1);

            cbSelector = new CB_OgrevenjeSetselector()
            {
                Left = 65,
                Top = lbl1.Top - 5
            };
            Controls.Add(cbSelector);

            Width = 200;
            Height = 150;
        }

        public class CB_OgrevenjeSetselector : ComboBox
        {
            public CB_OgrevenjeSetselector()
            {
                values();
                this.DropDownStyle = ComboBoxStyle.DropDownList;
            }

            void values()
            {
                var list = new List<ComboboxItem>();
                list.Add(new ComboboxItem("Izključeno", 0));
                list.Add(new ComboboxItem("Urnik", 1));

                this.DataSource = list;
                DisplayMember = "Text";
                ValueMember = "Value";
            }
        }
    }
}
