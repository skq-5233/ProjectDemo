namespace VisionSystemUserInterface
{
    partial class DevicesSetup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DevicesSetup));
            this.deviceControl = new VisionSystemControlLibrary.DeviceControl();
            this.SuspendLayout();
            // 
            // deviceControl
            // 
            this.deviceControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.deviceControl.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.deviceControl.Location = new System.Drawing.Point(0, 106);
            this.deviceControl.Name = "deviceControl";
            this.deviceControl.Size = new System.Drawing.Size(1024, 662);
            this.deviceControl.SystemDeviceState = VisionSystemClassLibrary.Enum.DeviceState.Stop;
            this.deviceControl.TabIndex = 1;
            this.deviceControl.Close_Click += new System.EventHandler(this.deviceControl_Close_Click);
            this.deviceControl.RefreshList_Click += new System.EventHandler(this.deviceControl_RefreshList_Click);
            this.deviceControl.ResetDevice_Click += new System.EventHandler(this.deviceControl_ResetDevice_Click);
            this.deviceControl.ConfigDevice_Click += new System.EventHandler(this.deviceControl_ConfigDevice_Click);
            this.deviceControl.ParameterSettings_Save += new System.EventHandler(this.deviceControl_ParameterSettings_Save);
            this.deviceControl.TestIO_Click += new System.EventHandler(this.deviceControl_TestIO_Click);
            this.deviceControl.TestIO_Close_Click += new System.EventHandler(this.deviceControl_TestIO_Close_Click);
            this.deviceControl.ConfigImage_Click += new System.EventHandler(this.deviceControl_ConfigImage_Click);
            this.deviceControl.AlignDateTime_Click += new System.EventHandler(this.deviceControl_AlignDateTime_Click);
            // 
            // DevicesSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.deviceControl);
            this.Name = "DevicesSetup";
            this.Text = "DevicesSetup";
            this.Load += new System.EventHandler(this.DevicesSetup_Load);
            this.Controls.SetChildIndex(this.deviceControl, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private VisionSystemControlLibrary.DeviceControl deviceControl;
    }
}