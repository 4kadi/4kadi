
namespace KontrolaKadi
{
    partial class Gui_MalaPec
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Gui_MalaPec));
            this.panelTop = new System.Windows.Forms.Panel();
            this.connectedButton1 = new KontrolaKadi.ConnectedButton();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSettings = new System.Windows.Forms.Button();
            this.stopWatch1 = new KontrolaKadi.StopWatch();
            this.enojnaKad1 = new KontrolaKadi.EnojnaKad();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panelTop.Controls.Add(this.connectedButton1);
            this.panelTop.Controls.Add(this.label2);
            this.panelTop.Controls.Add(this.btnSettings);
            this.panelTop.Location = new System.Drawing.Point(1, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1469, 78);
            this.panelTop.TabIndex = 4;
            // 
            // connectedButton1
            // 
            this.connectedButton1.BackColor = System.Drawing.Color.Transparent;
            this.connectedButton1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("connectedButton1.BackgroundImage")));
            this.connectedButton1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.connectedButton1.ConnectionStatus = -4;
            this.connectedButton1.ID = 1;
            this.connectedButton1.Location = new System.Drawing.Point(11, 26);
            this.connectedButton1.Name = "connectedButton1";
            this.connectedButton1.RefreshOriginalVal = 500;
            this.connectedButton1.Size = new System.Drawing.Size(60, 40);
            this.connectedButton1.TabIndex = 7;
            this.connectedButton1.UseVisualStyleBackColor = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(8, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 16);
            this.label2.TabIndex = 5;
            this.label2.Text = "Peč PC1";
            // 
            // btnSettings
            // 
            this.btnSettings.Location = new System.Drawing.Point(881, 12);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(194, 45);
            this.btnSettings.TabIndex = 0;
            this.btnSettings.Text = "Napredne Nastavitve";
            this.btnSettings.UseVisualStyleBackColor = true;
            // 
            // stopWatch1
            // 
            this.stopWatch1.Location = new System.Drawing.Point(333, 423);
            this.stopWatch1.Name = "stopWatch1";
            this.stopWatch1.Size = new System.Drawing.Size(145, 255);
            this.stopWatch1.TabIndex = 6;
            // 
            // enojnaKad1
            // 
            this.enojnaKad1.ID = 1;
            this.enojnaKad1.Location = new System.Drawing.Point(530, 103);
            this.enojnaKad1.Name = "enojnaKad1";
            this.enojnaKad1.Size = new System.Drawing.Size(250, 600);
            this.enojnaKad1.TabIndex = 7;
            // 
            // Gui_MalaPec
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(1355, 841);
            this.Controls.Add(this.enojnaKad1);
            this.Controls.Add(this.stopWatch1);
            this.Controls.Add(this.panelTop);
            this.DoubleBuffered = true;
            this.Name = "Gui_MalaPec";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "3";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Gui_MalaPec_Load_1);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.Label label2;
        private ConnectedButton connectedButton1;
        private StopWatch stopWatch1;
        private EnojnaKad enojnaKad1;
    }
}