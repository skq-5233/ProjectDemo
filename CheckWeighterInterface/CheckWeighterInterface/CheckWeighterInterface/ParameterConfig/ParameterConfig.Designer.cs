
namespace CheckWeighterInterface.ParameterConfig
{
    partial class ParameterConfig
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
            this.navigationFrame_paraConfig = new DevExpress.XtraBars.Navigation.NavigationFrame();
            this.navigationPage_benchmarkConfig = new DevExpress.XtraBars.Navigation.NavigationPage();
            this.navigationPage_algorithmConfig = new DevExpress.XtraBars.Navigation.NavigationPage();
            ((System.ComponentModel.ISupportInitialize)(this.navigationFrame_paraConfig)).BeginInit();
            this.navigationFrame_paraConfig.SuspendLayout();
            this.SuspendLayout();
            // 
            // navigationFrame_paraConfig
            // 
            this.navigationFrame_paraConfig.Controls.Add(this.navigationPage_benchmarkConfig);
            this.navigationFrame_paraConfig.Controls.Add(this.navigationPage_algorithmConfig);
            this.navigationFrame_paraConfig.Location = new System.Drawing.Point(0, 0);
            this.navigationFrame_paraConfig.Name = "navigationFrame_paraConfig";
            this.navigationFrame_paraConfig.Pages.AddRange(new DevExpress.XtraBars.Navigation.NavigationPageBase[] {
            this.navigationPage_benchmarkConfig,
            this.navigationPage_algorithmConfig});
            this.navigationFrame_paraConfig.SelectedPage = this.navigationPage_benchmarkConfig;
            this.navigationFrame_paraConfig.Size = new System.Drawing.Size(1024, 617);
            this.navigationFrame_paraConfig.TabIndex = 0;
            this.navigationFrame_paraConfig.Text = "navigationFrame1";
            this.navigationFrame_paraConfig.TransitionAnimationProperties.FrameInterval = 3000;
            // 
            // navigationPage_benchmarkConfig
            // 
            this.navigationPage_benchmarkConfig.Name = "navigationPage_benchmarkConfig";
            this.navigationPage_benchmarkConfig.Size = new System.Drawing.Size(1024, 617);
            // 
            // navigationPage_algorithmConfig
            // 
            this.navigationPage_algorithmConfig.Caption = "navigationPage_algorithmConfig";
            this.navigationPage_algorithmConfig.Name = "navigationPage_algorithmConfig";
            this.navigationPage_algorithmConfig.Size = new System.Drawing.Size(1024, 617);
            // 
            // ParameterConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.navigationFrame_paraConfig);
            this.Name = "ParameterConfig";
            this.Size = new System.Drawing.Size(1024, 617);
            ((System.ComponentModel.ISupportInitialize)(this.navigationFrame_paraConfig)).EndInit();
            this.navigationFrame_paraConfig.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.Navigation.NavigationFrame navigationFrame_paraConfig;
        private DevExpress.XtraBars.Navigation.NavigationPage navigationPage_benchmarkConfig;
        private DevExpress.XtraBars.Navigation.NavigationPage navigationPage_algorithmConfig;
    }
}
