namespace VisionSystemUserInterface
{
    partial class ImageConfiguration
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
            this.imageConfigurationControl = new VisionSystemControlLibrary.ImageConfigurationControl();
            this.SuspendLayout();
            // 
            // imageConfigurationControl
            // 
            this.imageConfigurationControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.imageConfigurationControl.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.imageConfigurationControl.Location = new System.Drawing.Point(0, 106);
            this.imageConfigurationControl.Name = "imageConfigurationControl";
            this.imageConfigurationControl.Size = new System.Drawing.Size(1024, 662);
            this.imageConfigurationControl.SystemDeviceState = VisionSystemClassLibrary.Enum.DeviceState.Stop;
            this.imageConfigurationControl.TabIndex = 1;
            this.imageConfigurationControl.Close_Click += new System.EventHandler(this.imageConfigurationControl_Close_Click);
            this.imageConfigurationControl.SaveProduct_Click += new System.EventHandler(this.imageConfigurationControl_SaveProduct_Click);
            this.imageConfigurationControl.FocusCalibration_Click += new System.EventHandler(this.imageConfigurationControl_FocusCalibration_Click);
            this.imageConfigurationControl.WhiteBalance_Click += new System.EventHandler(this.imageConfigurationControl_WhiteBalance_Click);
            this.imageConfigurationControl.ParameterValueChanged += new System.EventHandler(this.imageConfigurationControl_ParameterValueChanged);
            // 
            // ImageConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.imageConfigurationControl);
            this.Name = "ImageConfiguration";
            this.Text = "ImageConfiguration";
            this.Load += new System.EventHandler(this.ImageConfiguration_Load);
            this.Controls.SetChildIndex(this.imageConfigurationControl, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private VisionSystemControlLibrary.ImageConfigurationControl imageConfigurationControl;
    }
}