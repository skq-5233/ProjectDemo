namespace VisionSystemControlLibrary
{
    partial class ParameterSettingsWindow
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
            this.parameterSettings = new VisionSystemControlLibrary.ParameterSettings();
            this.SuspendLayout();
            // 
            // parameterSettings
            // 
            this.parameterSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.parameterSettings.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.parameterSettings.Location = new System.Drawing.Point(287, 118);
            this.parameterSettings.Name = "parameterSettings";
            this.parameterSettings.Size = new System.Drawing.Size(450, 532);
            this.parameterSettings.TabIndex = 0;
            this.parameterSettings.Close_Click += new System.EventHandler(this.parameterSettings_Close_Click);
            this.parameterSettings.Save_Click += new System.EventHandler(this.parameterSettings_Save_Click);
            // 
            // ParameterSettingsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.parameterSettings);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ParameterSettingsWindow";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ParameterSettingsWindow";
            this.ResumeLayout(false);

        }

        #endregion

        private ParameterSettings parameterSettings;
    }
}