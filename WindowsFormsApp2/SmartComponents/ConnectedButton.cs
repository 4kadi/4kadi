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
    public class ConnectedButton : Button
    {
        public int ID { get; set; } = 0;

        System.Timers.Timer updater = new System.Timers.Timer();

        void IDerrHandler()
        {
            if (ID == 0)
            {
                var msg = "ID was not set for control ConnectedButton. You can set property ID to any number greater from 1. You must set it in Designer, or programatilcally AFTER FORM LOADS.";
                MessageBox.Show(msg);
                throw new Exception(msg);
            }
        }
        
        public int ConnectionStatus { get; set; }        
        private Bitmap disconnectedIcon = Properties.Resources.disconnected;
        private Bitmap connectedIcon = Properties.Resources.connected;
        private Bitmap connectedWarningIcon = Properties.Resources.connect_warning1;
        private Bitmap connectingIcon = Properties.Resources.connecing;
        private int connstatcnt = 0;
        
       
        float PictureSize = 1.25F;
        
        public int RefreshOriginalVal { get; set; }

        public ConnectedButton()
        {
            bool designMode = (LicenseManager.UsageMode == LicenseUsageMode.Designtime);
        
            Text = "";

            RefreshOriginalVal = 500;
            ConnectionStatus = (int)Connection.Status.NotInitialised;

            BackgroundImage = disconnectedIcon;
            BackColor = DefaultBackColor;
            disconnectedIcon = Misc.Scale(disconnectedIcon, Height * PictureSize);
            connectedIcon = Misc.Scale(connectedIcon, Height * PictureSize);
            connectedWarningIcon = Misc.Scale(connectedWarningIcon, Height * PictureSize);
            connectingIcon = Misc.Scale(connectingIcon, Height * PictureSize);
            BackgroundImageLayout = ImageLayout.Center;
            BackColor = Color.Transparent;
            FormatBtn();
            TextChanged += ConnectedButton_TextChanged;
            UpdateConnectionStatus();

            if (designMode)
            {
                return;
            }

            // object own status retriever

            this.ParentChanged += ConnectedButton_ParentChanged;

        }

        private void ConnectedButton_ParentChanged(object sender, EventArgs e)
        {
            var form = this.FindForm();
            if (form != null)
            {
                form.Load += ConnectedButton_Load;
            }
            
        }

        private void ConnectedButton_Load(object sender, EventArgs e)
        {
            IDerrHandler();
            this.Click += Clicked;

            updater.Interval = Settings.UpdateValuesPCms;
            updater.Elapsed += Updater;
            updater.Start();
        }

        private void ConnectedButton_TextChanged(object sender, EventArgs e)
        {
            if (Text != "")
            {
                Text = "";
            }
            
        }

        void FormatBtn()
        {
            Width = 60;
            Height = 40;            
        }

        
        private void Clicked(object sender, EventArgs e)
        {
            
            if (ConnectionStatus == (int)Connection.Status.Error)
            {
                Connect();
            }
            else if (ConnectionStatus == (int)Connection.Status.Connecting)
            {
                Disconnect();
            }
            else if (ConnectionStatus == (int)Connection.Status.Connected)
            {
                Disconnect();
            }
            else if (ConnectionStatus == (int)Connection.Status.Warning)
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
                BackgroundImage = connectingIcon;
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
                BackgroundImage = connectingIcon;
                FormControl.Form_settings.DisconnectAsync(this.ID);
            }
            else
            {
                FormControl.identify.ShowPermissionError();
            }
        }

        public void UpdateConnectionStatus()
        {
            if (ConnectionStatus == (int)Connection.Status.NotInitialised){
                BackgroundImage = disconnectedIcon;
                connstatcnt = 0;
                return;}

            if (ConnectionStatus == (int)Connection.Status.Error){
                BackgroundImage = disconnectedIcon;
                connstatcnt = 0;
                return;}

            if (ConnectionStatus == (int)Connection.Status.Warning){
                BackgroundImage = connectedWarningIcon;
                connstatcnt = 0;
                return;}

            if (ConnectionStatus == (int)Connection.Status.Connecting){
                BackgroundImage = connectingIcon;
                connstatcnt = 0;
                return;}

            if (ConnectionStatus == (int)Connection.Status.Connected){       
                if (connstatcnt >= 2){
                    BackgroundImage = connectedIcon;}
                connstatcnt++;
                return;}            
        }
                

        public void RetrieveConnectionStatus()
        {
            try
            {                
                ConnectionStatus = (int)LogoControler.LOGOConnection[ID].connectionStatusLOGO;
            }
            catch (Exception e)
            {
                ConnectionStatus = (int)Connection.Status.Error;
                var a = e.Message;
            }

        }

        public void Updater(object sender, System.Timers.ElapsedEventArgs e)
        {            
            while (true)
            {
                try
                {
                    UpdateConnectionStatus();
                    Thread.Sleep(RefreshOriginalVal);
                    RetrieveConnectionStatus();       
                }
                catch 
                {
                    Thread.Sleep(Settings.UpdateValuesPCms);
                }                
            }
            
        }
        
    }
}
