namespace VisionSystemUserInterface
{
    partial class Work
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Work));
            this.timerLoad = new System.Windows.Forms.Timer(this.components);
            this.workControl = new VisionSystemControlLibrary.WorkControl();
            this.SuspendLayout();
            // 
            // timerLoad
            // 
            this.timerLoad.Interval = 250;
            this.timerLoad.Tick += new System.EventHandler(this.timerLoad_Tick);
            // 
            // workControl
            // 
            this.workControl.AppProductKey = "";
            this.workControl.AppVersion = "";
            this.workControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.workControl.BitmapBackground = new System.Drawing.Bitmap[] {
        ((System.Drawing.Bitmap)(resources.GetObject("workControl.BitmapBackground")))};
            this.workControl.BitmapTrademarkLeft = ((System.Drawing.Bitmap)(resources.GetObject("workControl.BitmapTrademarkLeft")));
            this.workControl.BitmapTrademarkRight = ((System.Drawing.Bitmap)(resources.GetObject("workControl.BitmapTrademarkRight")));
            this.workControl.ControllerApplicationVersion = "";
            this.workControl.Data_Value = 0;
            this.workControl.HMIApplicationVersion = "";
            this.workControl.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.workControl.Location = new System.Drawing.Point(0, 106);
            this.workControl.Name = "workControl";
            this.workControl.ReadyToUpdate = false;
            this.workControl.Size = new System.Drawing.Size(1024, 662);
            this.workControl.SystemDeviceState = VisionSystemClassLibrary.Enum.DeviceState.Stop;
            this.workControl.TabIndex = 1;
            this.workControl.UpdateControllerApplicationVersion = "";
            this.workControl.UpdateHMIApplicationVersion = "";
            this.workControl.UpdateNumber = 0;
            this.workControl.BrandManagement_Click += new System.EventHandler(this.workControl_BrandManagement_Click);
            this.workControl.Live_Click += new System.EventHandler(this.workControl_Live_Click);
            this.workControl.System_Click += new System.EventHandler(this.workControl_System_Click);
            this.workControl.DevicesSetup_Click += new System.EventHandler(this.workControl_DevicesSetup_Click);
            this.workControl.QualityCheck_Click += new System.EventHandler(this.workControl_QualityCheck_Click);
            this.workControl.Tolerances_Click += new System.EventHandler(this.workControl_Tolerances_Click);
            this.workControl.Statistics_Click += new System.EventHandler(this.workControl_Statistics_Click);
            this.workControl.Update_Click += new System.EventHandler(this.workControl_Update_Click);
            this.workControl.UpdateHMI += new System.EventHandler(this.workControl_UpdateHMI);
            this.workControl.PasswordEnter += new System.EventHandler(this.workControl_PasswordEnter);
            this.workControl.CameraDisplay_DoubleClick += new System.EventHandler(this.workControl_CameraDisplay_DoubleClick);
            this.workControl.ClearAllFaultMessages += new System.EventHandler(this.workControl_ClearAllFaultMessages);
            this.workControl.SetFaultMessageState += new System.EventHandler(this.workControl_SetFaultMessageState);
            this.workControl.Ping_Click += new System.EventHandler(this.workControl_Ping_Click);
            this.workControl.Connect_Click += new System.EventHandler(this.workControl_Connect_Click);
            // 
            // Work
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.workControl);
            this.Name = "Work";
            this.Text = "Work";
            this.Load += new System.EventHandler(this.Work_Load);
            this.Controls.SetChildIndex(this.workControl, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timerLoad;
        private VisionSystemControlLibrary.WorkControl workControl;
    }
}