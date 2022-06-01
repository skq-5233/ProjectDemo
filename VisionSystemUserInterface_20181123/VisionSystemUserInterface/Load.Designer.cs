namespace VisionSystemUserInterface
{
    partial class Load
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Load));
            this.loadControl = new VisionSystemControlLibrary.LoadControl();
            this.customProgressBar = new VisionSystemControlLibrary.CustomProgressBar();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // loadControl
            // 
            this.loadControl.AppVersion = "label";
            this.loadControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.loadControl.BitmapTrademark = ((System.Drawing.Bitmap)(resources.GetObject("loadControl.BitmapTrademark")));
            this.loadControl.DeviceName = "EXAMPLE";
            this.loadControl.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.loadControl.Location = new System.Drawing.Point(244, 208);
            this.loadControl.MessageTextIndex = 0;
            this.loadControl.Name = "loadControl";
            this.loadControl.ProgressBarMaximum = 100D;
            this.loadControl.ProgressBarMinimum = 0D;
            this.loadControl.ProgressBarStepNumber = 10D;
            this.loadControl.ProgressBarValue = 0D;
            this.loadControl.Size = new System.Drawing.Size(536, 352);
            this.loadControl.TabIndex = 0;
            // 
            // customProgressBar
            // 
            this.customProgressBar.BackColor = System.Drawing.Color.White;
            this.customProgressBar.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.customProgressBar.Location = new System.Drawing.Point(181, 455);
            this.customProgressBar.Maximum = 100D;
            this.customProgressBar.Minimum = 0D;
            this.customProgressBar.Name = "customProgressBar";
            this.customProgressBar.Size = new System.Drawing.Size(222, 19);
            this.customProgressBar.StepColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(189)))), ((int)(((byte)(51)))));
            this.customProgressBar.StepNumber = 10D;
            this.customProgressBar.TabIndex = 1;
            this.customProgressBar.Value = 0D;
            // 
            // timer1
            // 
            this.timer1.Interval = 250;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 26.25F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(83)))), ((int)(((byte)(158)))));
            this.label1.Location = new System.Drawing.Point(172, 306);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(715, 45);
            this.label1.TabIndex = 2;
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 26.25F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(83)))), ((int)(((byte)(158)))));
            this.label2.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.label2.Location = new System.Drawing.Point(172, 367);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(388, 44);
            this.label2.TabIndex = 3;
            this.label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // Load
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.BackgroundImage = global::VisionSystemUserInterface.Properties.Resources.Load_Background;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.customProgressBar);
            this.Controls.Add(this.loadControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Load";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Load";
            this.Load += new System.EventHandler(this.Load_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private VisionSystemControlLibrary.LoadControl loadControl;
        private VisionSystemControlLibrary.CustomProgressBar customProgressBar;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}