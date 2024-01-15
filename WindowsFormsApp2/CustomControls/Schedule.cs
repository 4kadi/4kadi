using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KontrolaKadi.SmartComponents
{
    class Schedule : SPanel
    {
        ScheduleRow vrsta1 = new ScheduleRow();
        ScheduleRow vrsta2 = new ScheduleRow();
        ScheduleRow vrsta3 = new ScheduleRow();
        ScheduleRow vrsta4 = new ScheduleRow();
        ScheduleRow vrsta5 = new ScheduleRow();
        ScheduleRow vrsta6 = new ScheduleRow();
        ScheduleRow vrsta7 = new ScheduleRow();

        public Schedule()
        {
            Controls.Add(vrsta1); vrsta1.Top = 0;
            Controls.Add(vrsta2); vrsta2.Top = vrsta1.Bottom;
            Controls.Add(vrsta3); vrsta3.Top = vrsta2.Bottom;
            Controls.Add(vrsta4); vrsta4.Top = vrsta3.Bottom;
            Controls.Add(vrsta5); vrsta5.Top = vrsta4.Bottom;
            Controls.Add(vrsta6); vrsta6.Top = vrsta5.Bottom;
            Controls.Add(vrsta7); vrsta7.Top = vrsta6.Bottom;
            Width = vrsta1.Width;
            Height = vrsta7.Bottom;
        }
    }

    class ScheduleRow : SPanel
    {
        public Label Dan = new Label();
        Label od_ = new Label();
        Label do_ = new Label();
        ScheduleEnable enabled = new ScheduleEnable();
        ScheduleTimeSelect TimeFrom = new ScheduleTimeSelect();
        ScheduleTimeSelect TimeTo = new ScheduleTimeSelect();
        public ScheduleRow()
        {            
            Height = 33;

            Dan.Top = 10;
            Dan.Left = 10;
            Dan.Width = 50;
            Dan.Text = "weekday";

            od_.Top = Dan.Top;
            od_.Left = Dan.Right + 10;
            od_.Width = 25;
            od_.Text = "od:";

            TimeFrom.Top = Dan.Top -3;
            TimeFrom.Left = od_.Right + 5;
            TimeFrom.Width = 100;

            do_.Top = Dan.Top;
            do_.Left = TimeFrom.Right + 10;
            do_.Width = 25;
            do_.Text = "do:";

            TimeTo.Top = Dan.Top -3;
            TimeTo.Left = do_.Right + 5;
            TimeTo.Width = 100;

            enabled.Top = Dan.Top -3;
            enabled.Left = TimeTo.Right + 10;
            enabled.Width = 100;
            enabled.Text = "Omogočeno";

            Width = enabled.Right + 70;

            Controls.Add(Dan);
            Controls.Add(od_);            
            Controls.Add(TimeFrom);
            Controls.Add(do_);
            Controls.Add(TimeTo);
            Controls.Add(enabled);
        }
    }

    class ScheduleEnable : CheckBox
    {
        public ScheduleEnable()
        {

        }
    }

    class ScheduleTimeSelect : ComboBox
    {
        public ScheduleTimeSelect()
        {

        }
    }

}
