namespace VisionSystemUserInterface
{
    partial class QualityCheck
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
            this.qualityCheckControl = new VisionSystemControlLibrary.QualityCheckControl();
            this.SuspendLayout();
            // 
            // qualityCheckControl
            // 
            this.qualityCheckControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.qualityCheckControl.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.qualityCheckControl.Location = new System.Drawing.Point(0, 106);
            this.qualityCheckControl.MachineSpeed = 800F;
            this.qualityCheckControl.Name = "qualityCheckControl";
            this.qualityCheckControl.Size = new System.Drawing.Size(1024, 662);
            this.qualityCheckControl.SystemDeviceState = VisionSystemClassLibrary.Enum.DeviceState.Stop;
            this.qualityCheckControl.TabIndex = 1;
            this.qualityCheckControl.ToolChanged += new System.EventHandler(this.qualityCheckControl_ToolChanged);
            this.qualityCheckControl.ToolParameterValueChanged += new System.EventHandler(this.qualityCheckControl_ToolParameterValueChanged);
            this.qualityCheckControl.ToolRegionChanged += new System.EventHandler(this.qualityCheckControl_ToolRegionChanged);
            this.qualityCheckControl.Close_Click += new System.EventHandler(this.qualityCheckControl_Close_Click);
            this.qualityCheckControl.SaveProduct_Click += new System.EventHandler(this.qualityCheckControl_SaveProduct_Click);
            this.qualityCheckControl.LearnSample_Click += new System.EventHandler(this.qualityCheckControl_LearnSample_Click);
            this.qualityCheckControl.LiveView_Click += new System.EventHandler(this.qualityCheckControl_LiveView_Click);
            this.qualityCheckControl.LoadSample_Click += new System.EventHandler(this.qualityCheckControl_LoadSample_Click);
            this.qualityCheckControl.ManageTools_Click += new System.EventHandler(this.qualityCheckControl_ManageTools_Click);
            this.qualityCheckControl.LoadReject_Click += new System.EventHandler(this.qualityCheckControl_LoadReject_Click);
            this.qualityCheckControl.LoadReject_ImageSelect_Click += new System.EventHandler(this.qualityCheckControl_LoadReject_ImageSelect_Click);
            // 
            // QualityCheck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.qualityCheckControl);
            this.Name = "QualityCheck";
            this.Text = "QualityCheck";
            this.Load += new System.EventHandler(this.QualityCheck_Load);
            this.Controls.SetChildIndex(this.qualityCheckControl, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private VisionSystemControlLibrary.QualityCheckControl qualityCheckControl;

    }
}