namespace VisionSystemControlLibrary
{
    partial class DateTimePanelWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DateTimePanelWindow));
            this.dateTimePanel = new VisionSystemControlLibrary.DateTimePanel();
            this.SuspendLayout();
            // 
            // dateTimePanel
            // 
            this.dateTimePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.dateTimePanel.Chinese_Caption = "标题";
            this.dateTimePanel.DisplayTime_1 = ((VisionSystemClassLibrary.Struct.SYSTEMTIME)(resources.GetObject("dateTimePanel.DisplayTime_1")));
            this.dateTimePanel.DisplayTime_2 = ((VisionSystemClassLibrary.Struct.SYSTEMTIME)(resources.GetObject("dateTimePanel.DisplayTime_2")));
            this.dateTimePanel.English_Caption = "CAPTION";
            this.dateTimePanel.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.dateTimePanel.Location = new System.Drawing.Point(205, 234);
            this.dateTimePanel.Name = "dateTimePanel";
            this.dateTimePanel.PanelType = VisionSystemControlLibrary.DateTimePanelType.StatisticsTimeSearch_1;
            this.dateTimePanel.ShiftTimeCheck = true;
            this.dateTimePanel.Size = new System.Drawing.Size(614, 300);
            this.dateTimePanel.TabIndex = 0;
            this.dateTimePanel.Close_Click += new System.EventHandler(this.dateTimePanel_Close_Click);
            // 
            // DateTimePanelWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.dateTimePanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "DateTimePanelWindow";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DateTimePanelWindow";
            this.ResumeLayout(false);

        }

        #endregion

        private DateTimePanel dateTimePanel;
    }
}