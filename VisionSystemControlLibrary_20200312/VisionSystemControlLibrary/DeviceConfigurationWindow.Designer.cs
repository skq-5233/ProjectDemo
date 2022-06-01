namespace VisionSystemControlLibrary
{
    partial class DeviceConfigurationWindow
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
            this.deviceConfiguration = new VisionSystemControlLibrary.DeviceConfiguration();
            this.SuspendLayout();
            // 
            // deviceConfiguration
            // 
            this.deviceConfiguration.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.deviceConfiguration.DeviceDataIndex = -1;
            this.deviceConfiguration.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.deviceConfiguration.Location = new System.Drawing.Point(172, 54);
            this.deviceConfiguration.Name = "deviceConfiguration";
            this.deviceConfiguration.Size = new System.Drawing.Size(680, 659);
            this.deviceConfiguration.TabIndex = 0;
            this.deviceConfiguration.Close_Click += new System.EventHandler(this.deviceConfiguration_Close_Click);
            // 
            // DeviceConfigurationWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.deviceConfiguration);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "DeviceConfigurationWindow";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DeviceConfigurationWindow";
            this.ResumeLayout(false);

        }

        #endregion

        private DeviceConfiguration deviceConfiguration;
    }
}