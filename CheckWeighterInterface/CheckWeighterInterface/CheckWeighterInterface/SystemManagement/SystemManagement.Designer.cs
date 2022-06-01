
namespace CheckWeighterInterface.SystemManagement
{
    partial class SystemManagement
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.navigationFrame_systemManagement = new DevExpress.XtraBars.Navigation.NavigationFrame();
            this.navigationPage_brandManagement = new DevExpress.XtraBars.Navigation.NavigationPage();
            this.navigationPage_calibrationCorrection = new DevExpress.XtraBars.Navigation.NavigationPage();
            this.navigationPage_systemConfig = new DevExpress.XtraBars.Navigation.NavigationPage();
            ((System.ComponentModel.ISupportInitialize)(this.navigationFrame_systemManagement)).BeginInit();
            this.navigationFrame_systemManagement.SuspendLayout();
            this.SuspendLayout();
            // 
            // navigationFrame_systemManagement
            // 
            this.navigationFrame_systemManagement.Controls.Add(this.navigationPage_brandManagement);
            this.navigationFrame_systemManagement.Controls.Add(this.navigationPage_calibrationCorrection);
            this.navigationFrame_systemManagement.Controls.Add(this.navigationPage_systemConfig);
            this.navigationFrame_systemManagement.Location = new System.Drawing.Point(0, 0);
            this.navigationFrame_systemManagement.Name = "navigationFrame_systemManagement";
            this.navigationFrame_systemManagement.Pages.AddRange(new DevExpress.XtraBars.Navigation.NavigationPageBase[] {
            this.navigationPage_brandManagement,
            this.navigationPage_calibrationCorrection,
            this.navigationPage_systemConfig});
            this.navigationFrame_systemManagement.SelectedPage = this.navigationPage_brandManagement;
            this.navigationFrame_systemManagement.Size = new System.Drawing.Size(1024, 617);
            this.navigationFrame_systemManagement.TabIndex = 0;
            this.navigationFrame_systemManagement.Text = "navigationFrame1";
            this.navigationFrame_systemManagement.TransitionAnimationProperties.FrameInterval = 3000;
            // 
            // navigationPage_brandManagement
            // 
            this.navigationPage_brandManagement.Name = "navigationPage_brandManagement";
            this.navigationPage_brandManagement.Size = new System.Drawing.Size(1024, 617);
            // 
            // navigationPage_calibrationCorrection
            // 
            this.navigationPage_calibrationCorrection.Name = "navigationPage_calibrationCorrection";
            this.navigationPage_calibrationCorrection.Size = new System.Drawing.Size(1024, 617);
            // 
            // navigationPage_systemConfig
            // 
            this.navigationPage_systemConfig.Name = "navigationPage_systemConfig";
            this.navigationPage_systemConfig.Size = new System.Drawing.Size(1024, 617);
            // 
            // SystemManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.navigationFrame_systemManagement);
            this.Name = "SystemManagement";
            this.Size = new System.Drawing.Size(1024, 617);
            ((System.ComponentModel.ISupportInitialize)(this.navigationFrame_systemManagement)).EndInit();
            this.navigationFrame_systemManagement.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.Navigation.NavigationFrame navigationFrame_systemManagement;
        private DevExpress.XtraBars.Navigation.NavigationPage navigationPage_brandManagement;
        private DevExpress.XtraBars.Navigation.NavigationPage navigationPage_calibrationCorrection;
        private DevExpress.XtraBars.Navigation.NavigationPage navigationPage_systemConfig;
    }
}
