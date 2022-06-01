namespace VisionSystemUserInterface
{
    partial class Template
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Template));
            this.titleBarControl = new VisionSystemControlLibrary.TitleBarControl();
            this.SuspendLayout();
            // 
            // titleBarControl
            // 
            this.titleBarControl.AdministratorPassword = "";
            this.titleBarControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.titleBarControl.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("titleBarControl.BackgroundImage")));
            this.titleBarControl.CameraName_Chinese = new string[] {
        "",
        "",
        ""};
            this.titleBarControl.CameraName_English = new string[] {
        "",
        "",
        ""};
            this.titleBarControl.Caption = "EXAMPLE";
            this.titleBarControl.ControlEnabled = true;
            this.titleBarControl.CurrentBrand = "label";
            this.titleBarControl.CurrentMachineType = "";
            this.titleBarControl.CurrentShift = "";
            this.titleBarControl.FaultExist = new bool[] {
        false,
        false,
        false};
            this.titleBarControl.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.titleBarControl.Location = new System.Drawing.Point(12, 12);
            this.titleBarControl.Name = "titleBarControl";
            this.titleBarControl.NetCheckShow = true;
            this.titleBarControl.PCTime = "";
            this.titleBarControl.Size = new System.Drawing.Size(1014, 100);
            this.titleBarControl.StateShow = true;
            this.titleBarControl.SystemDeviceState = VisionSystemClassLibrary.Enum.DeviceState.Stop;
            this.titleBarControl.TabIndex = 0;
            this.titleBarControl.TitleBarStyle = false;
            this.titleBarControl.UserPassword = "";
            this.titleBarControl.WindowParameter = 0;
            this.titleBarControl.NetCheck_Click += new System.EventHandler(this.titleBarControl_NetCheck_Click);
            this.titleBarControl.State_Click += new System.EventHandler(this.titleBarControl_State_Click);
            this.titleBarControl.GetFaultMessages += new System.EventHandler(this.titleBarControl_GetFaultMessages);
            this.titleBarControl.ChangeInterface += new System.EventHandler(this.titleBarControl_ChangeInterface);
            // 
            // Template
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.titleBarControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Template";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Template";
            this.Load += new System.EventHandler(this.Template_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private VisionSystemControlLibrary.TitleBarControl titleBarControl;

    }
}