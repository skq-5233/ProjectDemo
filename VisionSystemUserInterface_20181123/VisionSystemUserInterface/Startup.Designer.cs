namespace VisionSystemUserInterface
{
    partial class Startup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Startup));
            this.labelText = new System.Windows.Forms.Label();
            this.loadControl = new VisionSystemControlLibrary.LoadControl();
            this.customProgressBar = new VisionSystemControlLibrary.CustomProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelText
            // 
            this.labelText.BackColor = System.Drawing.Color.Transparent;
            this.labelText.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelText.ForeColor = System.Drawing.Color.White;
            this.labelText.Location = new System.Drawing.Point(288, 337);
            this.labelText.Name = "labelText";
            this.labelText.Size = new System.Drawing.Size(460, 86);
            this.labelText.TabIndex = 0;
            this.labelText.Text = "NAME";
            this.labelText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelText.Visible = false;
            // 
            // loadControl
            // 
            this.loadControl.AppVersion = "";
            this.loadControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.loadControl.BitmapTrademark = ((System.Drawing.Bitmap)(resources.GetObject("loadControl.BitmapTrademark")));
            this.loadControl.DeviceName = "";
            this.loadControl.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.loadControl.Location = new System.Drawing.Point(244, 208);
            this.loadControl.MessageTextIndex = 0;
            this.loadControl.Name = "loadControl";
            this.loadControl.ProgressBarMaximum = 100D;
            this.loadControl.ProgressBarMinimum = 0D;
            this.loadControl.ProgressBarStepNumber = 10D;
            this.loadControl.ProgressBarValue = 0D;
            this.loadControl.Size = new System.Drawing.Size(536, 352);
            this.loadControl.TabIndex = 1;
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
            this.customProgressBar.TabIndex = 2;
            this.customProgressBar.Value = 0D;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 26.25F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(83)))), ((int)(((byte)(158)))));
            this.label1.Location = new System.Drawing.Point(172, 306);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(715, 45);
            this.label1.TabIndex = 3;
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
            this.label2.TabIndex = 4;
            this.label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // Startup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.BackgroundImage = global::VisionSystemUserInterface.Properties.Resources.Load_Background;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.customProgressBar);
            this.Controls.Add(this.loadControl);
            this.Controls.Add(this.labelText);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Startup";
            this.ShowInTaskbar = false;
            this.Text = "Startup";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelText;
        private VisionSystemControlLibrary.LoadControl loadControl;
        private VisionSystemControlLibrary.CustomProgressBar customProgressBar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}