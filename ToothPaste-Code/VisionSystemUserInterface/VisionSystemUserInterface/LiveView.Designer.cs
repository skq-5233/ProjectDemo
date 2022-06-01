namespace VisionSystemUserInterface
{
    partial class LiveView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LiveView));
            this.liveControl = new VisionSystemControlLibrary.LiveControl();
            this.SuspendLayout();
            // 
            // liveControl
            // 
            this.liveControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.liveControl.BitmapBackground = new System.Drawing.Bitmap[] {
        ((System.Drawing.Bitmap)(resources.GetObject("liveControl.BitmapBackground")))};
            this.liveControl.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.liveControl.Location = new System.Drawing.Point(0, 106);
            this.liveControl.Name = "liveControl";
            this.liveControl.SelfTrigger = false;
            this.liveControl.Size = new System.Drawing.Size(1024, 662);
            this.liveControl.SystemDeviceState = VisionSystemClassLibrary.Enum.DeviceState.Stop;
            this.liveControl.TabIndex = 1;
            this.liveControl.ZoomImage = true;
            this.liveControl.Close_Click += new System.EventHandler(this.liveControl_Close_Click);
            this.liveControl.SelfTrigger_Click += new System.EventHandler(this.liveControl_SelfTrigger_Click);
            // 
            // LiveView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.liveControl);
            this.Name = "LiveView";
            this.Text = "LiveView";
            this.Load += new System.EventHandler(this.LiveView_Load);
            this.Controls.SetChildIndex(this.liveControl, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private VisionSystemControlLibrary.LiveControl liveControl;
    }
}