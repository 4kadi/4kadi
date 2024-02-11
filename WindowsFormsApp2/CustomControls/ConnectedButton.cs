using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Threading;

namespace KontrolaKadi
{
    public class ConnectedButton : SPanel
    {
        // --- THIS PROPERTIES MUST BE SET ---
        public int ID { get; set; } = 0;

        private string text = "TEXT";
        public override string Text
        {
            get { return text; }
            set
            {
                if (IsHandleCreated)
                {
                    SetText(value);
                }
                text = value;
            }
        }

        // ------------------------------

        public Color BackColorLbl
        {
            get { return lbl.BackColor; }
            set { lbl.BackColor = value; }
        }

        Button btn = new Button();
        CenteredLabel lbl = new CenteredLabel();

        System.Timers.Timer updater = new System.Timers.Timer();

        public Connection.Status ConnectionStatus { get; set; }
        private Bitmap disconnectedIcon = Properties.Resources.disconnected;
        private Bitmap connectedIcon = Properties.Resources.connected;
        private Bitmap connectedWarningIcon = Properties.Resources.connect_warning1;
        private Bitmap connectingIcon = Properties.Resources.connecing;
        private int connstatcnt = 0;


        float PictureSize = 1.25F;


        public ConnectedButton()
        {
            lbl.ForeColor = Color.White;
            bool designMode = (LicenseManager.UsageMode == LicenseUsageMode.Designtime);

            ConnectionStatus = Connection.Status.NotInitialised;

            btn.BackgroundImage = disconnectedIcon;
            btn.BackColor = DefaultBackColor;
            disconnectedIcon = Misc.Scale(disconnectedIcon, Height * PictureSize);
            connectedIcon = Misc.Scale(connectedIcon, Height * PictureSize);
            connectedWarningIcon = Misc.Scale(connectedWarningIcon, Height * PictureSize);
            connectingIcon = Misc.Scale(connectingIcon, Height * PictureSize);
            btn.BackgroundImageLayout = ImageLayout.Stretch;
            btn.BackColor = Color.Transparent;

            Controls.Add(btn);
         
            Controls.Add(lbl);
            FormatBtn();


            if (designMode)
            {
                return;
            }

            HandleCreated += ConnectedButton_HandleCreated;
            UpdateConnectionStatus();

        }

        void SetText(string text)
        {
            try
            {
                var m = new MethodInvoker(delegate { lbl.Text = text; });
                Invoke(m);
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        private void ConnectedButton_HandleCreated(object sender, EventArgs e)
        {
            btn.Click += Clicked;

            updater.Interval = Settings.UpdateValuesPCms;
            updater.Elapsed += Updater;
            updater.Start();

            SetText(text);
        }

        private void ConnectedButton_Load(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void FormatBtn()
        {
            btn.Top = 20;
            btn.Left = 5;
            btn.Width = 40;
            btn.Height = 40;

            Width = btn.Left * 2 + btn.Width;
            Height = btn.Height + 5 + btn.Top;

            lbl.Width = Width;
            lbl.Top = Top;
            lbl.Font = new Font("arial", 9, FontStyle.Bold);
        }


        private void Clicked(object sender, EventArgs e)
        {

            if (ConnectionStatus == Connection.Status.Error)
            {
                Connect();
            }
            else if (ConnectionStatus == Connection.Status.Connecting)
            {
                Disconnect();
            }
            else if (ConnectionStatus == Connection.Status.Connected)
            {
                Disconnect();
            }
            else if (ConnectionStatus == Connection.Status.Warning)
            {
                Disconnect();
            }
            else
            {
                Connect();
            }

        }

        public void Connect()
        {
            if (FormControl.identify.GetPermision(4))
            {
                btn.BackgroundImage = connectingIcon;
                FormControl.Form_settings.ConnectAsync(this.ID);
            }
            else
            {
                FormControl.identify.ShowPermissionError();
            }
        }

        public void Disconnect()
        {
            if (FormControl.identify.GetPermision(4))
            {
                btn.BackgroundImage = connectingIcon;
                FormControl.Form_settings.DisconnectAsync(this.ID);
            }
            else
            {
                FormControl.identify.ShowPermissionError();
            }
        }

        public void UpdateConnectionStatus()
        {
            if (ConnectionStatus == Connection.Status.NotInitialised)
            {
                btn.BackgroundImage = disconnectedIcon;
                connstatcnt = 0;
                return;
            }

            if (ConnectionStatus == Connection.Status.Error)
            {
                btn.BackgroundImage = disconnectedIcon;
                connstatcnt = 0;
                return;
            }

            if (ConnectionStatus == Connection.Status.Warning)
            {
                btn.BackgroundImage = connectedWarningIcon;
                connstatcnt = 0;
                return;
            }

            if (ConnectionStatus == Connection.Status.Connecting)
            {
                btn.BackgroundImage = connectingIcon;
                connstatcnt = 0;
                return;
            }

            if (ConnectionStatus == Connection.Status.Connected)
            {
                if (connstatcnt >= 2)
                {
                    btn.BackgroundImage = connectedIcon;
                }
                connstatcnt++;
                return;
            }
        }


        public void RetrieveConnectionStatus()
        {
            if (ID == 0)
            {
                throw new Exception("ConnectedButton ["+ Name +"] must have unique ID. Please set its ID property.");
            }

            try
            {
                var con = LogoControler.LOGOConnection[ID];
                if (con != null)
                {
                    ConnectionStatus = con.connectionStatusLOGO;
                }
            }
            catch (Exception e)
            {
                ConnectionStatus = Connection.Status.Error;
                var a = e.Message;
            }

        }

        public void Updater(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UpdateConnectionStatus();
                Thread.Sleep(Settings.UpdateValuesPCms);
                RetrieveConnectionStatus();
            }
            catch
            {
                // TODO Log exceptions
            }
        }
    }
}
