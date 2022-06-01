namespace VisionSystemUserInterface
{
    partial class BackupBrands
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BackupBrands));
            this.backupBrandsControl = new VisionSystemControlLibrary.BackupBrandsControl();
            this.SuspendLayout();
            // 
            // backupBrandsControl
            // 
            this.backupBrandsControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.backupBrandsControl.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.backupBrandsControl.LocalDiskDefaultPath = "D:\\Backup\\Brand\\";
            this.backupBrandsControl.Location = new System.Drawing.Point(0, 106);
            this.backupBrandsControl.Name = "backupBrandsControl";
            this.backupBrandsControl.Size = new System.Drawing.Size(1024, 662);
            this.backupBrandsControl.SystemBrandPath = "D:\\Brand\\";
            this.backupBrandsControl.SystemDeviceState = VisionSystemClassLibrary.Enum.DeviceState.Stop;
            this.backupBrandsControl.TabIndex = 1;
            this.backupBrandsControl.USBDeviceDefaultPath = "";
            this.backupBrandsControl.Close_Click += new System.EventHandler(this.backupBrandsControl_Close_Click);
            // 
            // BackupBrands
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.backupBrandsControl);
            this.Name = "BackupBrands";
            this.Text = "BackupBrands";
            this.Load += new System.EventHandler(this.BackupBrands_Load);
            this.Controls.SetChildIndex(this.backupBrandsControl, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private VisionSystemControlLibrary.BackupBrandsControl backupBrandsControl;

    }
}