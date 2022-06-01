
namespace CheckWeighterInterface.SystemTest
{
    partial class SystemTest
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
            this.navigationFrame_sysTest = new DevExpress.XtraBars.Navigation.NavigationFrame();
            this.navigationPage_signalTest = new DevExpress.XtraBars.Navigation.NavigationPage();
            this.navigationPage_realTimeCurve = new DevExpress.XtraBars.Navigation.NavigationPage();
            ((System.ComponentModel.ISupportInitialize)(this.navigationFrame_sysTest)).BeginInit();
            this.navigationFrame_sysTest.SuspendLayout();
            this.SuspendLayout();
            // 
            // navigationFrame_sysTest
            // 
            this.navigationFrame_sysTest.Controls.Add(this.navigationPage_signalTest);
            this.navigationFrame_sysTest.Controls.Add(this.navigationPage_realTimeCurve);
            this.navigationFrame_sysTest.Location = new System.Drawing.Point(0, 0);
            this.navigationFrame_sysTest.Name = "navigationFrame_sysTest";
            this.navigationFrame_sysTest.Pages.AddRange(new DevExpress.XtraBars.Navigation.NavigationPageBase[] {
            this.navigationPage_signalTest,
            this.navigationPage_realTimeCurve});
            this.navigationFrame_sysTest.SelectedPage = this.navigationPage_signalTest;
            this.navigationFrame_sysTest.Size = new System.Drawing.Size(1024, 617);
            this.navigationFrame_sysTest.TabIndex = 0;
            this.navigationFrame_sysTest.Text = "navigationFrame1";
            this.navigationFrame_sysTest.TransitionAnimationProperties.FrameInterval = 3000;
            // 
            // navigationPage_signalTest
            // 
            this.navigationPage_signalTest.Name = "navigationPage_signalTest";
            this.navigationPage_signalTest.Size = new System.Drawing.Size(1024, 617);
            // 
            // navigationPage_realTimeCurve
            // 
            this.navigationPage_realTimeCurve.Caption = "navigationPage_realTimeCurve";
            this.navigationPage_realTimeCurve.Name = "navigationPage_realTimeCurve";
            this.navigationPage_realTimeCurve.Size = new System.Drawing.Size(1024, 617);
            // 
            // SystemTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.navigationFrame_sysTest);
            this.Name = "SystemTest";
            this.Size = new System.Drawing.Size(1024, 617);
            ((System.ComponentModel.ISupportInitialize)(this.navigationFrame_sysTest)).EndInit();
            this.navigationFrame_sysTest.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.Navigation.NavigationFrame navigationFrame_sysTest;
        private DevExpress.XtraBars.Navigation.NavigationPage navigationPage_signalTest;
        private DevExpress.XtraBars.Navigation.NavigationPage navigationPage_realTimeCurve;
    }
}
