using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace KontrolaKadi
{
    public class Switch3point : SGroupBox
    {
        private string leftLabel = "0";

        public string LeftLabel
        {
            get { return leftLabel; }
            set { leftLabel = value; }
        }

        private string centerLabel = "AUTO";

        public string CenterLabel
        {
            get { return centerLabel; }
            set { centerLabel = value; }
        }

        private string rightLabel = "1";

        public string RightLabel
        {
            get { return rightLabel; }
            set { rightLabel = value; }
        }

        private bool currentState;

        public bool StateForReporting // does not represent state of controls position but state of the process that this control is part of - this value should be set from outside
        {
            get { return currentState; }
            set { currentState = value; updateCurrentState(value); }
        }

        public string CurrentStateFalseText { get; set; } = "Ni aktivno";
        public Color CurrentStateFalseColor { get; set; } = Color.Pink;
        public string CurrentStateTrueText { get; set; } = "Aktivno";
        public Color CurrentStateTrueColor { get; set; } = Color.LightGreen;

        CenteredLabel lblCurrentState;

        private SwitchState _state = SwitchState.Center;
        public SwitchState State
        { 
            get { return _state; } 
            set { _state = value; Invalidate(); } 
        }
        private bool isDragging = false;
        public event EventHandler StateChanged;
        Rectangle toggleRectangle = new Rectangle();
        int toggleWidth = 45;
        bool designMode = false;
        int switchHeight = 20;
        int switchWidth = 100;
        int switchStartX = 15;
        int switchYPosition = 27;

        Rectangle switchRectangle;
        Brush switchBrush = Brushes.LightGray;

        public Switch3point()
        {
            designMode = (LicenseManager.UsageMode == LicenseUsageMode.Designtime);

            // Set a default size for the control
            this.Size = new Size(240, 75);
            switchRectangle = new Rectangle(switchStartX, switchYPosition, switchWidth, switchHeight);

            lblCurrentState = new CenteredLabel()
            {
                Top = switchYPosition,
                Left = switchStartX + switchWidth + 5,
                Width = 100,
                Text = CurrentStateFalseText,
                Font = new Font("Arial", 11, FontStyle.Bold)

        };
            Controls.Add(lblCurrentState);

            this.Font = new Font("Arial", 11, FontStyle.Bold);
            HandleCreated += Switch3point_HandleCreated;
            this.Click += Switch3point_Click;
            
        }

        private void Switch3point_Click(object sender, EventArgs e)
        {
            
        }

        public void SetStateFromPlcVar(PlcVars.Word state)
        {
            if (state.Value_short == 1)
            {
                State = SwitchState.Left;
            }
            else if (state.Value_short == 2)
            {
                State = SwitchState.Right;
            }
            else
            {
                State = SwitchState.Center;
            }
            
        }

        private void Switch3point_HandleCreated(object sender, EventArgs e)
        {
            updateCurrentState(false);
        }

        void updateCurrentState(bool value)
        {
            var m = new MethodInvoker(delegate 
            {
                if (value)
                {
                    lblCurrentState.Text = CurrentStateTrueText;
                    if (CurrentStateTrueColor != null)
                    {
                        lblCurrentState.BackColor = CurrentStateTrueColor;
                    }
                }
                else
                {
                    lblCurrentState.Text = CurrentStateFalseText;
                    if (CurrentStateFalseColor != null)
                    {
                        lblCurrentState.BackColor = CurrentStateFalseColor;
                    }
                }
            });

            if (IsHandleCreated)
            {
                Invoke(m);
            }
           
        }

        protected override void OnPaint(PaintEventArgs e)
        {           
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;                      

            // Draw the base of the switch
            g.FillRectangle(switchBrush, switchRectangle);

            // Always keep the Y-coordinate and height of the toggle the same
            int toggleY = switchRectangle.Top;
            int toggleHeight = switchHeight;

            if (!isDragging)
            {
                // Update the position of the toggle based on the state when not dragging
                switch (_state)
                {
                    case SwitchState.Left:
                        toggleRectangle = new Rectangle(switchRectangle.Left, toggleY, toggleWidth, toggleHeight);
                        break;
                    case SwitchState.Center:
                        toggleRectangle = new Rectangle(switchRectangle.Left + (switchWidth - toggleWidth) / 2, toggleY, toggleWidth, toggleHeight);
                        break;
                    case SwitchState.Right:
                        toggleRectangle = new Rectangle(switchRectangle.Right - toggleWidth, toggleY, toggleWidth, toggleHeight);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                // When dragging, only update the X-coordinate
                toggleRectangle = new Rectangle(toggleRectangle.X, toggleY, toggleWidth, toggleHeight);
            }

            toggleRectangle.Inflate(-2, -2); // Adjust the size of the toggle
            g.FillRectangle(Brushes.DarkGray, toggleRectangle); // Draw the toggle

            // Draw three vertical lines on the toggle
            Color lineColor = Color.FromArgb(80, 80, 80);
            Pen linePen = new Pen(lineColor, 2);
            int lineSpacing = toggleWidth / 6;
            int startLineX = (toggleRectangle.Left + (toggleWidth - (3 * lineSpacing)) / 2)+3;
            for (int i = 0; i < 3; i++)
            {
                int xPosition = startLineX + i * lineSpacing;
                g.DrawLine(linePen, xPosition, toggleRectangle.Top + 3, xPosition, toggleRectangle.Bottom - 3);
            }

            // Define label positions and draw them
            StringFormat stringFormat = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Near
            };

            Font regularLabelFont = new Font("Arial", 9);
            Font boldLabelFont = new Font("Arial", 11, FontStyle.Bold);
            Brush labelBrush = Brushes.Black;

            int labelY = toggleRectangle.Bottom + 5; // 5 pixels below the toggle

            // Calculate label width and center positions
            int labelQuarterWidth = switchWidth / 4; // quarter width
            int labelHalfWidth = switchWidth / 2; // half width
            int leftLabelX = switchStartX;
            int centerLabelX = switchStartX + labelQuarterWidth;
            int rightLabelX = switchStartX + labelQuarterWidth + labelHalfWidth;

            // Draw labels for each state
            Font leftLabelFont = _state == SwitchState.Left ? boldLabelFont : regularLabelFont;
            Font centerLabelFont = _state == SwitchState.Center ? boldLabelFont : regularLabelFont;
            Font rightLabelFont = _state == SwitchState.Right ? boldLabelFont : regularLabelFont;

            g.DrawString(leftLabel, leftLabelFont, labelBrush, new Rectangle(leftLabelX, labelY, labelQuarterWidth, 20), stringFormat);
            g.DrawString(centerLabel, centerLabelFont, labelBrush, new Rectangle(centerLabelX, labelY, labelHalfWidth, 20), stringFormat);
            g.DrawString(rightLabel, rightLabelFont, labelBrush, new Rectangle(rightLabelX, labelY, labelQuarterWidth, 20), stringFormat);

        }
        public enum SwitchState
        {
            Left, Center, Right
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (designMode) { return; }

            base.OnMouseDown(e);

            // Check if the mouse is within the toggle rectangle or if dragging is not already happening
            if (toggleRectangle.Contains(e.Location) || !isDragging)
            {
                isDragging = true;

                // Calculate the new X position for the toggle, centered around the mouse
                int newToggleX = e.X - (toggleWidth / 2);

                // Constrain the toggle within the switch bounds
                int minToggleX = switchStartX; // Leftmost position the toggle can be
                int maxToggleX = switchStartX + switchWidth - toggleWidth; // Rightmost position the toggle can be

                newToggleX = Math.Max(newToggleX, minToggleX);
                newToggleX = Math.Min(newToggleX, maxToggleX);

                // Update the toggle position
                toggleRectangle.X = newToggleX;

                Invalidate(); // Redraw the control
            }
        }


        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (designMode) { return; }

            base.OnMouseMove(e);
            if (isDragging)
            {
                // Calculate new X position for the toggle, centered around the mouse
                int newToggleX = e.X - (toggleWidth / 2);

                // Constrain the toggle within the switch bounds
                int minToggleX = switchStartX; // Leftmost position the toggle can be
                int maxToggleX = switchStartX + switchWidth - toggleWidth; // Rightmost position the toggle can be

                newToggleX = Math.Max(newToggleX, minToggleX);
                newToggleX = Math.Min(newToggleX, maxToggleX);

                // Directly set the position of the toggle
                toggleRectangle.X = newToggleX;

                Invalidate(); // Redraw the control
                this.Update();  // Force immediate repaint
            }
        }



        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (designMode) { return; }

            base.OnMouseUp(e);
            isDragging = false;

            // Determine the closest state
            int leftBoundary = switchStartX + switchWidth / 3; // One-third into the switch width
            int rightBoundary = switchStartX + 2 * switchWidth / 3; // Two-thirds into the switch width
            int centerOfToggle = toggleRectangle.X + (toggleWidth / 2);

            if (centerOfToggle < leftBoundary)
            {
                _state = SwitchState.Left;
            }
            else if (centerOfToggle >= rightBoundary)
            {
                _state = SwitchState.Right;
            }
            else
            {
                _state = SwitchState.Center;
            }

            // Update the toggle position based on the new state
            switch (_state)
            {
                case SwitchState.Left:
                    toggleRectangle.X = switchStartX;
                    break;
                case SwitchState.Center:
                    toggleRectangle.X = switchStartX + (switchWidth - toggleWidth) / 2;
                    break;
                case SwitchState.Right:
                    toggleRectangle.X = switchStartX + switchWidth - toggleWidth;
                    break;
            }

            Invalidate(); // Redraw the control

            OnStateChanged(EventArgs.Empty);
        }

        protected virtual void OnStateChanged(EventArgs e)
        {
            StateChanged?.Invoke(this, e);
        }

    }
}
