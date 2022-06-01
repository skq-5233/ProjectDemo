namespace VisionSystemUserInterface
{
    partial class SystemConfiguration
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
            this.systemControl = new VisionSystemControlLibrary.SystemControl();
            this.SuspendLayout();
            // 
            // systemControl
            // 
            this.systemControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.systemControl.CommonParameterNumber = 0;
            this.systemControl.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.systemControl.Location = new System.Drawing.Point(0, 106);
            this.systemControl.Name = "systemControl";
            this.systemControl.Size = new System.Drawing.Size(1024, 662);
            this.systemControl.SystemDeviceState = VisionSystemClassLibrary.Enum.DeviceState.Stop;
            this.systemControl.TabIndex = 1;
            this.systemControl.Close_Click += new System.EventHandler(this.systemControl_Close_Click);
            this.systemControl.Ok_Click += new System.EventHandler(this.systemControl_Ok_Click);
            this.systemControl.Ok_CommonParameterChanged_Click += new System.EventHandler(this.systemControl_Ok_CommonParameterChanged_Click);
            this.systemControl.LanguageChanged += new System.EventHandler(this.systemControl_LanguageChanged);
            this.systemControl.PCTimerTick += new System.EventHandler(this.systemControl_PCTimerTick);
            // 
            // SystemConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.systemControl);
            this.Name = "SystemConfiguration";
            this.Text = "SystemConfiguration";
            this.Load += new System.EventHandler(this.SystemConfiguration_Load);
            this.Controls.SetChildIndex(this.systemControl, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private VisionSystemControlLibrary.SystemControl systemControl;



    }
}