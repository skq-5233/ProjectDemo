namespace VisionSystemControlLibrary
{
    partial class StatisticsRecordWindow
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
            this.statisticsRecord = new VisionSystemControlLibrary.StatisticsRecord();
            this.SuspendLayout();
            // 
            // statisticsRecord
            // 
            this.statisticsRecord.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.statisticsRecord.Chinese_CameraName = "";
            this.statisticsRecord.English_CameraName = "";
            this.statisticsRecord.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.statisticsRecord.Location = new System.Drawing.Point(74, 69);
            this.statisticsRecord.Name = "statisticsRecord";
            this.statisticsRecord.SelectedCameraType = VisionSystemClassLibrary.Enum.CameraType.Camera_1;
            this.statisticsRecord.Size = new System.Drawing.Size(876, 630);
            this.statisticsRecord.TabIndex = 0;
            this.statisticsRecord.Delete_Click += new System.EventHandler(this.statisticsRecord_Delete_Click);
            this.statisticsRecord.Close_Click += new System.EventHandler(this.statisticsRecord_Close_Click);
            // 
            // StatisticsRecordWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.statisticsRecord);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "StatisticsRecordWindow";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "StatisticsRecordWindow";
            this.ResumeLayout(false);

        }

        #endregion

        private StatisticsRecord statisticsRecord;

    }
}