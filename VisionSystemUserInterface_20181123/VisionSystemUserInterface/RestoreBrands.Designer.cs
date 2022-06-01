namespace VisionSystemUserInterface
{
    partial class RestoreBrands
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RestoreBrands));
            this.restoreBrandsControl = new VisionSystemControlLibrary.RestoreBrandsControl();
            this.SuspendLayout();
            // 
            // restoreBrandsControl
            // 
            this.restoreBrandsControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.restoreBrandsControl.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.restoreBrandsControl.LocalDiskDefaultPath = "D:\\Backup\\Brand\\";
            this.restoreBrandsControl.Location = new System.Drawing.Point(0, 106);
            this.restoreBrandsControl.Name = "restoreBrandsControl";
            this.restoreBrandsControl.Size = new System.Drawing.Size(1024, 662);
            this.restoreBrandsControl.SystemBrandPath = "D:\\Brand\\";
            this.restoreBrandsControl.SystemDeviceState = VisionSystemClassLibrary.Enum.DeviceState.Stop;
            this.restoreBrandsControl.TabIndex = 1;
            this.restoreBrandsControl.USBDeviceDefaultPath = "";
            this.restoreBrandsControl.Close_Click += new System.EventHandler(this.restoreBrandsControl_Close_Click);
            this.restoreBrandsControl.Restore_Click += new System.EventHandler(this.restoreBrandsControl_Restore_Click);
            this.restoreBrandsControl.RestoreBrandsSuccess += new System.EventHandler(this.restoreBrandsControl_RestoreBrandsSuccess);
            // 
            // RestoreBrands
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.restoreBrandsControl);
            this.Name = "RestoreBrands";
            this.Text = "RestoreBrands";
            this.Load += new System.EventHandler(this.RestoreBrands_Load);
            this.Controls.SetChildIndex(this.restoreBrandsControl, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private VisionSystemControlLibrary.RestoreBrandsControl restoreBrandsControl;

    }
}