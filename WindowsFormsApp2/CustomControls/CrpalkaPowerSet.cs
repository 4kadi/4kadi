using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KontrolaKadi
{
    public class CrpalkaPowerSet : PowerSet
    {
        const short increment = 50;
        public CrpalkaPowerSet()
        {
            plus.Click += Plus_Click;
            minus.Click += Minus_Click;
        }

        private void Minus_Click(object sender, EventArgs e)
        {
            if (ValuePowerset == null)
            {
                return;
            }

            short num;
            var success = short.TryParse(tb.Text.Replace(Unit, ""), out num);

            num -= increment;

            short remainder = (short)(num % increment);
            if (remainder != 0)
            {
                num -= remainder; // Adjust to the nearest lower multiple of increment
            }

            if (num < 0)
            {
                num = 0;
            }

            if (success)
            {
                ValuePowerset.Value_short = num;
            }
        }

        private void Plus_Click(object sender, EventArgs e)
        {
            if(ValuePowerset == null)
            {
                return;
            }

            short num;
            var success = short.TryParse(tb.Text.Replace(Unit, ""), out num);


            if (num < 0)
            {
                num = 0;
            }

            // Adjust to the nearest higher multiple of increment if necessary
            short remainder = (short)(num % increment);
            if (remainder != 0)
            {
                num += (short)(increment - remainder);
            }

            else
            {
                num += increment;
            }     

            if (success)
            {
                ValuePowerset.Value_short = num;
            }
        }
    }
}
