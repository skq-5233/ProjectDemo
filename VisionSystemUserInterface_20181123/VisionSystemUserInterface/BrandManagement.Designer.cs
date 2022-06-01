namespace VisionSystemUserInterface
{
    partial class BrandManagement
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BrandManagement));
            this.brandControl = new VisionSystemControlLibrary.BrandControl();
            this.SuspendLayout();
            // 
            // brandControl
            // 
            this.brandControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.brandControl.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.brandControl.Location = new System.Drawing.Point(0, 106);
            this.brandControl.Name = "brandControl";
            this.brandControl.Size = new System.Drawing.Size(1024, 662);
            this.brandControl.SystemDeviceState = VisionSystemClassLibrary.Enum.DeviceState.Stop;
            this.brandControl.TabIndex = 1;
            this.brandControl.Close_Click += new System.EventHandler(this.brandControl_Close_Click);
            this.brandControl.LoadReloadBrand_Click += new System.EventHandler(this.brandControl_LoadReloadBrand_Click);
            this.brandControl.LoadReloadBrandSuccess += new System.EventHandler(this.brandControl_LoadReloadBrandSuccess);
            this.brandControl.BackupBrands_Click += new System.EventHandler(this.brandControl_BackupBrands_Click);
            this.brandControl.RestoreBrands_Click += new System.EventHandler(this.brandControl_RestoreBrands_Click);
            // 
            // BrandManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.brandControl);
            this.Name = "BrandManagement";
            this.Text = "BrandManagement";
            this.Load += new System.EventHandler(this.BrandManagement_Load);
            this.Controls.SetChildIndex(this.brandControl, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private VisionSystemControlLibrary.BrandControl brandControl;
    }
}