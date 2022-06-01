namespace VisionSystemControlLibrary
{
    partial class NetDiagnoseWindows
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
            this.netDiagnose1 = new VisionSystemControlLibrary.NetDiagnose();
            this.SuspendLayout();
            // 
            // netDiagnose1
            // 
            this.netDiagnose1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.netDiagnose1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.netDiagnose1.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.netDiagnose1.Location = new System.Drawing.Point(0, 0);
            this.netDiagnose1.Margin = new System.Windows.Forms.Padding(4);
            this.netDiagnose1.Name = "netDiagnose1";
            this.netDiagnose1.Size = new System.Drawing.Size(1024, 768);
            this.netDiagnose1.TabIndex = 0;
            this.netDiagnose1.ControllerConnect_Click += new System.EventHandler(this.netDiagnose1_ControllerConnect_Click);
            this.netDiagnose1.CameraConnect_Click += new System.EventHandler(this.netDiagnose1_CameraConnect_Click);
            this.netDiagnose1.Close_Click += new System.EventHandler(this.netDiagnose1_Close_Click);
            // 
            // NetDiagnoseWindows
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.netDiagnose1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "NetDiagnoseWindows";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NetDiagnoseWindows";
            this.ResumeLayout(false);

        }

        #endregion

        private NetDiagnose netDiagnose1;
    }
}