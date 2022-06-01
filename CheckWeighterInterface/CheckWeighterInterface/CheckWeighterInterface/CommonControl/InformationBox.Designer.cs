
namespace CheckWeighterInterface.CommonControl
{
    partial class InformationBox
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InformationBox));
            this.panelControl_infoBox = new DevExpress.XtraEditors.PanelControl();
            this.pictureEdit1 = new DevExpress.XtraEditors.PictureEdit();
            this.labelControl_infoTitle = new DevExpress.XtraEditors.LabelControl();
            this.simpleButton_infoOK = new DevExpress.XtraEditors.SimpleButton();
            this.timer_disappear = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl_infoBox)).BeginInit();
            this.panelControl_infoBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl_infoBox
            // 
            this.panelControl_infoBox.Appearance.BackColor = System.Drawing.Color.Silver;
            this.panelControl_infoBox.Appearance.Options.UseBackColor = true;
            this.panelControl_infoBox.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.panelControl_infoBox.Controls.Add(this.pictureEdit1);
            this.panelControl_infoBox.Controls.Add(this.labelControl_infoTitle);
            this.panelControl_infoBox.Controls.Add(this.simpleButton_infoOK);
            this.panelControl_infoBox.Location = new System.Drawing.Point(0, 0);
            this.panelControl_infoBox.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
            this.panelControl_infoBox.LookAndFeel.UseDefaultLookAndFeel = false;
            this.panelControl_infoBox.Name = "panelControl_infoBox";
            this.panelControl_infoBox.Size = new System.Drawing.Size(350, 150);
            this.panelControl_infoBox.TabIndex = 1;
            // 
            // pictureEdit1
            // 
            this.pictureEdit1.EditValue = ((object)(resources.GetObject("pictureEdit1.EditValue")));
            this.pictureEdit1.Location = new System.Drawing.Point(0, 0);
            this.pictureEdit1.Name = "pictureEdit1";
            this.pictureEdit1.Properties.Appearance.BackColor = System.Drawing.Color.DimGray;
            this.pictureEdit1.Properties.Appearance.Options.UseBackColor = true;
            this.pictureEdit1.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.pictureEdit1.Size = new System.Drawing.Size(50, 39);
            this.pictureEdit1.TabIndex = 34;
            // 
            // labelControl_infoTitle
            // 
            this.labelControl_infoTitle.Appearance.BackColor = System.Drawing.Color.DimGray;
            this.labelControl_infoTitle.Appearance.Font = new System.Drawing.Font("微软雅黑", 15.75F);
            this.labelControl_infoTitle.Appearance.ForeColor = System.Drawing.Color.White;
            this.labelControl_infoTitle.Appearance.Options.UseBackColor = true;
            this.labelControl_infoTitle.Appearance.Options.UseFont = true;
            this.labelControl_infoTitle.Appearance.Options.UseForeColor = true;
            this.labelControl_infoTitle.Appearance.Options.UseTextOptions = true;
            this.labelControl_infoTitle.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.labelControl_infoTitle.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.labelControl_infoTitle.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl_infoTitle.Location = new System.Drawing.Point(0, 0);
            this.labelControl_infoTitle.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
            this.labelControl_infoTitle.LookAndFeel.UseDefaultLookAndFeel = false;
            this.labelControl_infoTitle.Name = "labelControl_infoTitle";
            this.labelControl_infoTitle.Size = new System.Drawing.Size(350, 39);
            this.labelControl_infoTitle.TabIndex = 33;
            this.labelControl_infoTitle.Text = "   删除成功！";
            // 
            // simpleButton_infoOK
            // 
            this.simpleButton_infoOK.Appearance.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.simpleButton_infoOK.Appearance.Options.UseFont = true;
            this.simpleButton_infoOK.Appearance.Options.UseTextOptions = true;
            this.simpleButton_infoOK.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.simpleButton_infoOK.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.simpleButton_infoOK.AppearancePressed.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.simpleButton_infoOK.AppearancePressed.Options.UseFont = true;
            this.simpleButton_infoOK.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.simpleButton_infoOK.ImageOptions.ImageToTextIndent = 10;
            this.simpleButton_infoOK.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.simpleButton_infoOK.Location = new System.Drawing.Point(98, 73);
            this.simpleButton_infoOK.LookAndFeel.SkinName = "DevExpress Dark Style";
            this.simpleButton_infoOK.LookAndFeel.UseDefaultLookAndFeel = false;
            this.simpleButton_infoOK.Name = "simpleButton_infoOK";
            this.simpleButton_infoOK.ShowFocusRectangle = DevExpress.Utils.DefaultBoolean.False;
            this.simpleButton_infoOK.Size = new System.Drawing.Size(146, 46);
            this.simpleButton_infoOK.TabIndex = 6;
            this.simpleButton_infoOK.Text = "确 定";
            this.simpleButton_infoOK.Click += new System.EventHandler(this.simpleButton_infoOK_Click);
            // 
            // timer_disappear
            // 
            this.timer_disappear.Interval = 1000;
            this.timer_disappear.Tick += new System.EventHandler(this.timer_disappear_Tick);
            // 
            // InformationBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelControl_infoBox);
            this.Name = "InformationBox";
            this.Size = new System.Drawing.Size(350, 150);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl_infoBox)).EndInit();
            this.panelControl_infoBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl_infoBox;
        private DevExpress.XtraEditors.LabelControl labelControl_infoTitle;
        private DevExpress.XtraEditors.SimpleButton simpleButton_infoOK;
        private System.Windows.Forms.Timer timer_disappear;
        private DevExpress.XtraEditors.PictureEdit pictureEdit1;
    }
}
