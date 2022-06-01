namespace VisionSystemControlLibrary
{
    partial class IOSignalDiagnosisWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IOSignalDiagnosisWindow));
            this.ioSignalDiagnosis = new VisionSystemControlLibrary.IOSignalDiagnosis();
            this.SuspendLayout();
            // 
            // ioSignalDiagnosis
            // 
            this.ioSignalDiagnosis.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.ioSignalDiagnosis.Chinese_SelectedDeviceName = null;
            this.ioSignalDiagnosis.English_SelectedDeviceName = null;
            this.ioSignalDiagnosis.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.ioSignalDiagnosis.Location = new System.Drawing.Point(115, 200);
            this.ioSignalDiagnosis.Name = "ioSignalDiagnosis";
            this.ioSignalDiagnosis.Size = new System.Drawing.Size(793, 368);
            this.ioSignalDiagnosis.TabIndex = 0;
            this.ioSignalDiagnosis.Close_Click += new System.EventHandler(this.ioSignalDiagnosis_Close_Click);
            // 
            // IOSignalDiagnosisWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.ioSignalDiagnosis);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "IOSignalDiagnosisWindow";
            this.Text = "IOSignalDiagnosisWindow";
            this.ResumeLayout(false);

        }

        #endregion

        private IOSignalDiagnosis ioSignalDiagnosis;
    }
}