namespace VisionSystemControlLibrary
{
    partial class SensorSelectWindow
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
            this.sensorSelect1 = new VisionSystemControlLibrary.SensorSelect();
            this.SuspendLayout();
            // 
            // sensorSelect1
            // 
            this.sensorSelect1.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.sensorSelect1.Location = new System.Drawing.Point(282, 84);
            this.sensorSelect1.Name = "sensorSelect1";
            this.sensorSelect1.Size = new System.Drawing.Size(460, 600);
            this.sensorSelect1.TabIndex = 0;
            this.sensorSelect1.Close_Click += new System.EventHandler(this.sensorSelect1_Close_Click);
            // 
            // SensorSelectWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.sensorSelect1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SensorSelectWindow";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SensorSelectWindow";
            this.ResumeLayout(false);

        }

        #endregion

        private SensorSelect sensorSelect1;
    }
}