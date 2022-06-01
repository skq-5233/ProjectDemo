namespace VisionSystemControlLibrary
{
    partial class Tolerances
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
            this.customButtonLearning = new VisionSystemControlLibrary.CustomButton();
            this.customButtonRunStop = new VisionSystemControlLibrary.CustomButton();
            this.SuspendLayout();
            // 
            // customButtonLearning
            // 
            this.customButtonLearning.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonLearning.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonLearning.BitmapIconWhole = global::VisionSystemControlLibrary.Properties.Resources.Learn;
            this.customButtonLearning.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.Button;
            this.customButtonLearning.Chinese_TextDisplay = new string[] {
        "学习"};
            this.customButtonLearning.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonLearning.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonLearning.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(47, 45)};
            this.customButtonLearning.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonLearning.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonLearning.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonLearning.CurrentTextGroupIndex = 0;
            this.customButtonLearning.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonLearning.CustomButtonData = null;
            this.customButtonLearning.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonLearning.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonLearning.DrawIcon = true;
            this.customButtonLearning.DrawText = false;
            this.customButtonLearning.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonLearning.English_TextDisplay = new string[] {
        "LEARN"};
            this.customButtonLearning.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonLearning.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonLearning.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(52, 45)};
            this.customButtonLearning.FocusBackgroundDisplay = false;
            this.customButtonLearning.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonLearning.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonLearning.ForeColor = System.Drawing.Color.White;
            this.customButtonLearning.HoverBackgroundDisplay = false;
            this.customButtonLearning.IconIndex = new int[] {
        1,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonLearning.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(2, 5),
        new System.Drawing.Point(2, 5)};
            this.customButtonLearning.IconNumber = 2;
            this.customButtonLearning.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(53, 37),
        new System.Drawing.Size(53, 37)};
            this.customButtonLearning.LabelControlMode = false;
            this.customButtonLearning.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonLearning.Location = new System.Drawing.Point(557, 72);
            this.customButtonLearning.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonLearning.Name = "customButtonLearning";
            this.customButtonLearning.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonLearning.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonLearning.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonLearning.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonLearning.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonLearning.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonLearning.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonLearning.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonLearning.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonLearning.Size = new System.Drawing.Size(53, 45);
            this.customButtonLearning.SizeButton = new System.Drawing.Size(53, 45);
            this.customButtonLearning.TabIndex = 42;
            this.customButtonLearning.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonLearning.TextGroupNumber = 1;
            this.customButtonLearning.UpdateControl = true;
            this.customButtonLearning.CustomButton_Click += new System.EventHandler(this.customButtonLearning_CustomButton_Click);
            // 
            // customButtonRunStop
            // 
            this.customButtonRunStop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonRunStop.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonRunStop.BitmapIconWhole = global::VisionSystemControlLibrary.Properties.Resources.RunStop;
            this.customButtonRunStop.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.Button;
            this.customButtonRunStop.Chinese_TextDisplay = new string[] {
        "运行/停止"};
            this.customButtonRunStop.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonRunStop.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonRunStop.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(47, 45)};
            this.customButtonRunStop.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonRunStop.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonRunStop.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonRunStop.CurrentTextGroupIndex = 0;
            this.customButtonRunStop.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Selected;
            this.customButtonRunStop.CustomButtonData = null;
            this.customButtonRunStop.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonRunStop.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Switch;
            this.customButtonRunStop.DrawIcon = true;
            this.customButtonRunStop.DrawText = false;
            this.customButtonRunStop.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonRunStop.English_TextDisplay = new string[] {
        "RUN/STOP"};
            this.customButtonRunStop.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonRunStop.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonRunStop.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(52, 45)};
            this.customButtonRunStop.FocusBackgroundDisplay = false;
            this.customButtonRunStop.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonRunStop.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonRunStop.ForeColor = System.Drawing.Color.White;
            this.customButtonRunStop.HoverBackgroundDisplay = false;
            this.customButtonRunStop.IconIndex = new int[] {
        3,
        2,
        1,
        0,
        0,
        1,
        0,
        1,
        0};
            this.customButtonRunStop.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(3, 4),
        new System.Drawing.Point(3, 4),
        new System.Drawing.Point(3, 4),
        new System.Drawing.Point(3, 4)};
            this.customButtonRunStop.IconNumber = 4;
            this.customButtonRunStop.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(53, 37),
        new System.Drawing.Size(53, 37),
        new System.Drawing.Size(53, 37),
        new System.Drawing.Size(53, 37)};
            this.customButtonRunStop.LabelControlMode = false;
            this.customButtonRunStop.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonRunStop.Location = new System.Drawing.Point(557, 20);
            this.customButtonRunStop.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonRunStop.Name = "customButtonRunStop";
            this.customButtonRunStop.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonRunStop.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonRunStop.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonRunStop.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonRunStop.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonRunStop.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonRunStop.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonRunStop.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonRunStop.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonRunStop.Size = new System.Drawing.Size(53, 45);
            this.customButtonRunStop.SizeButton = new System.Drawing.Size(53, 45);
            this.customButtonRunStop.TabIndex = 41;
            this.customButtonRunStop.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonRunStop.TextGroupNumber = 1;
            this.customButtonRunStop.UpdateControl = true;
            this.customButtonRunStop.CustomButton_Click += new System.EventHandler(this.customButtonRunStop_CustomButton_Click);
            // 
            // Tolerances
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.Controls.Add(this.customButtonLearning);
            this.Controls.Add(this.customButtonRunStop);
            this.Name = "Tolerances";
            this.Size = new System.Drawing.Size(616, 148);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Tolerances_Paint);
            this.DoubleClick += new System.EventHandler(this.Tolerances_DoubleClick);
            this.ResumeLayout(false);

        }

        #endregion

        private CustomButton customButtonRunStop;
        private CustomButton customButtonLearning;
    }
}
