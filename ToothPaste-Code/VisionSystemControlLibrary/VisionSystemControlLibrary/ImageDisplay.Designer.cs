namespace VisionSystemControlLibrary
{
    partial class ImageDisplay
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageDisplay));
            this.pictureBoxBackground = new System.Windows.Forms.PictureBox();
            this.statusBar = new VisionSystemControlLibrary.StatusBar();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBackground)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxBackground
            // 
            this.pictureBoxBackground.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pictureBoxBackground.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxBackground.Name = "pictureBoxBackground";
            this.pictureBoxBackground.Size = new System.Drawing.Size(640, 480);
            this.pictureBoxBackground.TabIndex = 8;
            this.pictureBoxBackground.TabStop = false;
            this.pictureBoxBackground.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBoxBackground_MouseClick);
            this.pictureBoxBackground.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pictureBoxBackground_MouseDoubleClick);
            this.pictureBoxBackground.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBoxBackground_MouseDown);
            this.pictureBoxBackground.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBoxBackground_MouseMove);
            this.pictureBoxBackground.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBoxBackground_MouseUp);
            // 
            // statusBar
            // 
            this.statusBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(126)))), ((int)(((byte)(126)))), ((int)(((byte)(126)))));
            this.statusBar.ColorControlBackground = System.Drawing.Color.FromArgb(((int)(((byte)(126)))), ((int)(((byte)(126)))), ((int)(((byte)(126)))));
            this.statusBar.ControlScale = 1D;
            this.statusBar.ControlScale_X = 0.99375D;
            this.statusBar.ControlScale_Y = 1D;
            this.statusBar.ControlSize = new System.Drawing.Size(636, 50);
            this.statusBar.CurrentValueFont = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.statusBar.CurrentValueLocation = new System.Drawing.Point(353, 28);
            this.statusBar.CurrentValueSize = new System.Drawing.Size(109, 15);
            this.statusBar.Information = ((VisionSystemClassLibrary.Struct.ImageInformation)(resources.GetObject("statusBar.Information")));
            this.statusBar.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.statusBar.Location = new System.Drawing.Point(2, 2);
            this.statusBar.MaxValueFont = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.statusBar.MaxValueLocation = new System.Drawing.Point(473, 28);
            this.statusBar.MaxValueSize = new System.Drawing.Size(109, 15);
            this.statusBar.MessageFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.statusBar.MessageLampLocation = new System.Drawing.Point(590, 10);
            this.statusBar.MessageLampSize = new System.Drawing.Size(31, 32);
            this.statusBar.MessageLocation = new System.Drawing.Point(8, 8);
            this.statusBar.MessageSize = new System.Drawing.Size(540, 18);
            this.statusBar.MinValueFont = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.statusBar.MinValueLocation = new System.Drawing.Point(234, 28);
            this.statusBar.MinValueSize = new System.Drawing.Size(109, 15);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(636, 50);
            this.statusBar.SlotLocation = new System.Drawing.Point(13, 31);
            this.statusBar.SlotSize = new System.Drawing.Size(216, 12);
            this.statusBar.TabIndex = 9;
            // 
            // ImageDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.pictureBoxBackground);
            this.Name = "ImageDisplay";
            this.Size = new System.Drawing.Size(640, 480);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBackground)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxBackground;
        private StatusBar statusBar;
    }
}
