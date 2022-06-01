namespace VisionSystemUserInterface
{
    partial class RejectsView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RejectsView));
            this.rejectsControl = new VisionSystemControlLibrary.RejectsControl();
            this.SuspendLayout();
            // 
            // rejectsControl
            // 
            this.rejectsControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.rejectsControl.BackupImageIndex = 0;
            this.rejectsControl.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.rejectsControl.Location = new System.Drawing.Point(0, 106);
            this.rejectsControl.Name = "rejectsControl";
            this.rejectsControl.Size = new System.Drawing.Size(1024, 662);
            this.rejectsControl.SystemDeviceState = VisionSystemClassLibrary.Enum.DeviceState.Stop;
            this.rejectsControl.TabIndex = 1;
            this.rejectsControl.ViewToolGraphics = false;
            this.rejectsControl.Close_Click += new System.EventHandler(this.rejectsControl_Close_Click);
            this.rejectsControl.Item_Click += new System.EventHandler(this.rejectsControl_Item_Click);
            this.rejectsControl.ViewToolGraphics_Click += new System.EventHandler(this.rejectsControl_ViewToolGraphics_Click);
            this.rejectsControl.ClearAll_Click += new System.EventHandler(this.rejectsControl_ClearAll_Click);
            this.rejectsControl.BackupAllImages_Event += new System.EventHandler(this.rejectsControl_BackupAllImages_Event);
            // 
            // RejectsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.rejectsControl);
            this.Name = "RejectsView";
            this.Text = "RejectsView";
            this.Load += new System.EventHandler(this.RejectsView_Load);
            this.Controls.SetChildIndex(this.rejectsControl, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private VisionSystemControlLibrary.RejectsControl rejectsControl;

    }
}