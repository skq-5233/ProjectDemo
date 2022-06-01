namespace VisionSystemControlLibrary
{
    partial class ShiftConfigurationWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShiftConfigurationWindow));
            this.shiftConfiguration = new VisionSystemControlLibrary.ShiftConfiguration();
            this.SuspendLayout();
            // 
            // shiftConfiguration
            // 
            this.shiftConfiguration.ApplySettings = true;
            this.shiftConfiguration.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.shiftConfiguration.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.shiftConfiguration.Location = new System.Drawing.Point(306, 118);
            this.shiftConfiguration.MaxShiftNumber = 24;
            this.shiftConfiguration.MinShiftNumber = 1;
            this.shiftConfiguration.Name = "shiftConfiguration";
            this.shiftConfiguration.ShiftTimeConfiguration = new VisionSystemClassLibrary.Struct.ShiftTime[] {
        ((VisionSystemClassLibrary.Struct.ShiftTime)(resources.GetObject("shiftConfiguration.ShiftTimeConfiguration"))),
        ((VisionSystemClassLibrary.Struct.ShiftTime)(resources.GetObject("shiftConfiguration.ShiftTimeConfiguration1"))),
        ((VisionSystemClassLibrary.Struct.ShiftTime)(resources.GetObject("shiftConfiguration.ShiftTimeConfiguration2")))};
            this.shiftConfiguration.ShiftTimeConfiguration_Original = new VisionSystemClassLibrary.Struct.ShiftTime[] {
        ((VisionSystemClassLibrary.Struct.ShiftTime)(resources.GetObject("shiftConfiguration.ShiftTimeConfiguration_Original"))),
        ((VisionSystemClassLibrary.Struct.ShiftTime)(resources.GetObject("shiftConfiguration.ShiftTimeConfiguration_Original1"))),
        ((VisionSystemClassLibrary.Struct.ShiftTime)(resources.GetObject("shiftConfiguration.ShiftTimeConfiguration_Original2")))};
            this.shiftConfiguration.Size = new System.Drawing.Size(413, 532);
            this.shiftConfiguration.TabIndex = 0;
            this.shiftConfiguration.Close_Click += new System.EventHandler(this.shiftConfiguration_Close_Click);
            // 
            // ShiftConfigurationWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.shiftConfiguration);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ShiftConfigurationWindow";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ShiftConfigurationWindow";
            this.ResumeLayout(false);

        }

        #endregion

        private ShiftConfiguration shiftConfiguration;
    }
}