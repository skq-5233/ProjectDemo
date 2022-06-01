namespace VisionSystemUserInterface
{
    partial class StatisticsView
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
            this.statisticsControl = new VisionSystemControlLibrary.StatisticsControl();
            this.SuspendLayout();
            // 
            // statisticsControl
            // 
            this.statisticsControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.statisticsControl.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.statisticsControl.Location = new System.Drawing.Point(0, 106);
            this.statisticsControl.Name = "statisticsControl";
            this.statisticsControl.Relevancy = false;
            this.statisticsControl.Size = new System.Drawing.Size(1024, 662);
            this.statisticsControl.SystemDeviceState = VisionSystemClassLibrary.Enum.DeviceState.Stop;
            this.statisticsControl.TabIndex = 1;
            this.statisticsControl.Close_Click += new System.EventHandler(this.statisticsControl_Close_Click);
            this.statisticsControl.GetRecordData += new System.EventHandler(this.statisticsControl_GetRecordData);
            this.statisticsControl.GetRecords += new System.EventHandler(this.statisticsControl_GetRecords);
            this.statisticsControl.DeleteRecords += new System.EventHandler(this.statisticsControl_DeleteRecords);
            this.statisticsControl.ViewRejectImage += new System.EventHandler(this.statisticsControl_ViewRejectImage);
            this.statisticsControl.Relevancy_Click += new System.EventHandler(this.statisticsControl_Relevancy_Click);
            this.statisticsControl.ViewRejectImage_Relevancy += new System.EventHandler(this.statisticsControl_ViewRejectImage_Relevancy);
            // 
            // StatisticsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.statisticsControl);
            this.Name = "StatisticsView";
            this.Text = "StatisticsView";
            this.Load += new System.EventHandler(this.StatisticsView_Load);
            this.Controls.SetChildIndex(this.statisticsControl, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private VisionSystemControlLibrary.StatisticsControl statisticsControl;



    }
}