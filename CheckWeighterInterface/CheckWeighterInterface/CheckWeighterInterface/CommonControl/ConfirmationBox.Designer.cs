
namespace CheckWeighterInterface.CommonControl
{
    partial class ConfirmationBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfirmationBox));
            this.panelControl_confirmationBox = new DevExpress.XtraEditors.PanelControl();
            this.labelControl_confirmationBoxTitle = new DevExpress.XtraEditors.LabelControl();
            this.simpleButton_confirmationCancel = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton_confirmationOK = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl_confirmationBox)).BeginInit();
            this.panelControl_confirmationBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControl_confirmationBox
            // 
            this.panelControl_confirmationBox.Appearance.BackColor = System.Drawing.Color.Silver;
            this.panelControl_confirmationBox.Appearance.Options.UseBackColor = true;
            this.panelControl_confirmationBox.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.panelControl_confirmationBox.Controls.Add(this.labelControl_confirmationBoxTitle);
            this.panelControl_confirmationBox.Controls.Add(this.simpleButton_confirmationCancel);
            this.panelControl_confirmationBox.Controls.Add(this.simpleButton_confirmationOK);
            this.panelControl_confirmationBox.Location = new System.Drawing.Point(0, 0);
            this.panelControl_confirmationBox.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
            this.panelControl_confirmationBox.LookAndFeel.UseDefaultLookAndFeel = false;
            this.panelControl_confirmationBox.Name = "panelControl_confirmationBox";
            this.panelControl_confirmationBox.Size = new System.Drawing.Size(350, 150);
            this.panelControl_confirmationBox.TabIndex = 0;
            // 
            // labelControl_confirmationBoxTitle
            // 
            this.labelControl_confirmationBoxTitle.Appearance.BackColor = System.Drawing.Color.DimGray;
            this.labelControl_confirmationBoxTitle.Appearance.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelControl_confirmationBoxTitle.Appearance.ForeColor = System.Drawing.Color.White;
            this.labelControl_confirmationBoxTitle.Appearance.Options.UseBackColor = true;
            this.labelControl_confirmationBoxTitle.Appearance.Options.UseFont = true;
            this.labelControl_confirmationBoxTitle.Appearance.Options.UseForeColor = true;
            this.labelControl_confirmationBoxTitle.Appearance.Options.UseTextOptions = true;
            this.labelControl_confirmationBoxTitle.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.labelControl_confirmationBoxTitle.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.labelControl_confirmationBoxTitle.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl_confirmationBoxTitle.Location = new System.Drawing.Point(0, 0);
            this.labelControl_confirmationBoxTitle.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
            this.labelControl_confirmationBoxTitle.LookAndFeel.UseDefaultLookAndFeel = false;
            this.labelControl_confirmationBoxTitle.Name = "labelControl_confirmationBoxTitle";
            this.labelControl_confirmationBoxTitle.Size = new System.Drawing.Size(350, 40);
            this.labelControl_confirmationBoxTitle.TabIndex = 33;
            this.labelControl_confirmationBoxTitle.Text = "确认关闭软件？";
            this.labelControl_confirmationBoxTitle.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Warning;
            // 
            // simpleButton_confirmationCancel
            // 
            this.simpleButton_confirmationCancel.Appearance.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.simpleButton_confirmationCancel.Appearance.Options.UseFont = true;
            this.simpleButton_confirmationCancel.AppearancePressed.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.simpleButton_confirmationCancel.AppearancePressed.Options.UseFont = true;
            this.simpleButton_confirmationCancel.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton_confirmationCancel.ImageOptions.Image")));
            this.simpleButton_confirmationCancel.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.simpleButton_confirmationCancel.ImageOptions.ImageToTextIndent = 10;
            this.simpleButton_confirmationCancel.Location = new System.Drawing.Point(187, 66);
            this.simpleButton_confirmationCancel.LookAndFeel.SkinName = "DevExpress Dark Style";
            this.simpleButton_confirmationCancel.LookAndFeel.UseDefaultLookAndFeel = false;
            this.simpleButton_confirmationCancel.Name = "simpleButton_confirmationCancel";
            this.simpleButton_confirmationCancel.ShowFocusRectangle = DevExpress.Utils.DefaultBoolean.False;
            this.simpleButton_confirmationCancel.Size = new System.Drawing.Size(146, 46);
            this.simpleButton_confirmationCancel.TabIndex = 8;
            this.simpleButton_confirmationCancel.Text = "取 消";
            this.simpleButton_confirmationCancel.Click += new System.EventHandler(this.simpleButton_confirmationCancel_Click);
            // 
            // simpleButton_confirmationOK
            // 
            this.simpleButton_confirmationOK.Appearance.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.simpleButton_confirmationOK.Appearance.Options.UseFont = true;
            this.simpleButton_confirmationOK.Appearance.Options.UseTextOptions = true;
            this.simpleButton_confirmationOK.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.simpleButton_confirmationOK.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.simpleButton_confirmationOK.AppearancePressed.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.simpleButton_confirmationOK.AppearancePressed.Options.UseFont = true;
            this.simpleButton_confirmationOK.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton_confirmationOK.ImageOptions.Image")));
            this.simpleButton_confirmationOK.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.simpleButton_confirmationOK.ImageOptions.ImageToTextIndent = 10;
            this.simpleButton_confirmationOK.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.simpleButton_confirmationOK.Location = new System.Drawing.Point(16, 66);
            this.simpleButton_confirmationOK.LookAndFeel.SkinName = "DevExpress Dark Style";
            this.simpleButton_confirmationOK.LookAndFeel.UseDefaultLookAndFeel = false;
            this.simpleButton_confirmationOK.Name = "simpleButton_confirmationOK";
            this.simpleButton_confirmationOK.ShowFocusRectangle = DevExpress.Utils.DefaultBoolean.False;
            this.simpleButton_confirmationOK.Size = new System.Drawing.Size(146, 46);
            this.simpleButton_confirmationOK.TabIndex = 6;
            this.simpleButton_confirmationOK.Text = "确 定";
            this.simpleButton_confirmationOK.Click += new System.EventHandler(this.simpleButton_confirmationOK_Click);
            // 
            // ConfirmationBox
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelControl_confirmationBox);
            this.Location = new System.Drawing.Point(337, 250);
            this.Name = "ConfirmationBox";
            this.Size = new System.Drawing.Size(350, 150);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl_confirmationBox)).EndInit();
            this.panelControl_confirmationBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl_confirmationBox;
        private DevExpress.XtraEditors.SimpleButton simpleButton_confirmationCancel;
        private DevExpress.XtraEditors.SimpleButton simpleButton_confirmationOK;
        private DevExpress.XtraEditors.LabelControl labelControl_confirmationBoxTitle;
    }
}
