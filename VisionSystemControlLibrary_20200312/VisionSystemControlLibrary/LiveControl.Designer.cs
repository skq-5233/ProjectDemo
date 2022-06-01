namespace VisionSystemControlLibrary
{
    partial class LiveControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LiveControl));
            this.pictureBoxBackground = new System.Windows.Forms.PictureBox();
            this.labelIcon = new System.Windows.Forms.Label();
            this.imageDisplayLiveViewZoomIn = new VisionSystemControlLibrary.ImageDisplay();
            this.imageDisplayLiveViewZoomOut = new VisionSystemControlLibrary.ImageDisplay();
            this.customButtonZoomInBackground = new VisionSystemControlLibrary.CustomButton();
            this.customButtonZoomImage = new VisionSystemControlLibrary.CustomButton();
            this.customButtonSelfTrigger = new VisionSystemControlLibrary.CustomButton();
            this.customButtonLastLearntImage = new VisionSystemControlLibrary.CustomButton();
            this.customButtonLiveViewZoomOut = new VisionSystemControlLibrary.CustomButton();
            this.customButtonLiveViewZoomIn = new VisionSystemControlLibrary.CustomButton();
            this.customButtonClose = new VisionSystemControlLibrary.CustomButton();
            this.customButtonCaption = new VisionSystemControlLibrary.CustomButton();
            this.imageDisplayLastLearntImage = new VisionSystemControlLibrary.ImageDisplay();
            this.customButtonZoomOutBackground = new VisionSystemControlLibrary.CustomButton();
            this.customButtonLastLearntImageBackground = new VisionSystemControlLibrary.CustomButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBackground)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxBackground
            // 
            this.pictureBoxBackground.Location = new System.Drawing.Point(0, 60);
            this.pictureBoxBackground.Name = "pictureBoxBackground";
            this.pictureBoxBackground.Size = new System.Drawing.Size(686, 518);
            this.pictureBoxBackground.TabIndex = 16;
            this.pictureBoxBackground.TabStop = false;
            // 
            // labelIcon
            // 
            this.labelIcon.BackColor = System.Drawing.Color.Transparent;
            this.labelIcon.Image = global::VisionSystemControlLibrary.Properties.Resources.ConfigImage;
            this.labelIcon.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelIcon.Location = new System.Drawing.Point(28, 17);
            this.labelIcon.Name = "labelIcon";
            this.labelIcon.Size = new System.Drawing.Size(53, 37);
            this.labelIcon.TabIndex = 59;
            // 
            // imageDisplayLiveViewZoomIn
            // 
            this.imageDisplayLiveViewZoomIn.AutoShowTitle = true;
            this.imageDisplayLiveViewZoomIn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.imageDisplayLiveViewZoomIn.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.imageDisplayLiveViewZoomIn.BitmapDisplay = null;
            this.imageDisplayLiveViewZoomIn.ColorStatusBarControlBackground = System.Drawing.Color.FromArgb(((int)(((byte)(126)))), ((int)(((byte)(126)))), ((int)(((byte)(126)))));
            this.imageDisplayLiveViewZoomIn.ControlScale = 1D;
            this.imageDisplayLiveViewZoomIn.ControlScale_X = 1D;
            this.imageDisplayLiveViewZoomIn.ControlScale_Y = 1D;
            this.imageDisplayLiveViewZoomIn.ControlSize = new System.Drawing.Size(640, 480);
            this.imageDisplayLiveViewZoomIn.CurrentValueFont = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.imageDisplayLiveViewZoomIn.CurrentValueLocation = new System.Drawing.Point(353, 28);
            this.imageDisplayLiveViewZoomIn.CurrentValueSize = new System.Drawing.Size(109, 15);
            this.imageDisplayLiveViewZoomIn.ImageFileName = "1.jpg";
            this.imageDisplayLiveViewZoomIn.ImageFilePath = ".\\ConfigData\\RejectsImage\\";
            this.imageDisplayLiveViewZoomIn.Information = ((VisionSystemClassLibrary.Struct.ImageInformation)(resources.GetObject("imageDisplayLiveViewZoomIn.Information")));
            this.imageDisplayLiveViewZoomIn.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.imageDisplayLiveViewZoomIn.Location = new System.Drawing.Point(19, 85);
            this.imageDisplayLiveViewZoomIn.MaxValueFont = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.imageDisplayLiveViewZoomIn.MaxValueLocation = new System.Drawing.Point(473, 28);
            this.imageDisplayLiveViewZoomIn.MaxValueSize = new System.Drawing.Size(109, 15);
            this.imageDisplayLiveViewZoomIn.MessageFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.imageDisplayLiveViewZoomIn.MessageLampLocation = new System.Drawing.Point(590, 9);
            this.imageDisplayLiveViewZoomIn.MessageLampSize = new System.Drawing.Size(31, 32);
            this.imageDisplayLiveViewZoomIn.MessageLocation = new System.Drawing.Point(8, 8);
            this.imageDisplayLiveViewZoomIn.MessageSize = new System.Drawing.Size(540, 18);
            this.imageDisplayLiveViewZoomIn.MinValueFont = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.imageDisplayLiveViewZoomIn.MinValueLocation = new System.Drawing.Point(234, 28);
            this.imageDisplayLiveViewZoomIn.MinValueSize = new System.Drawing.Size(109, 15);
            this.imageDisplayLiveViewZoomIn.Name = "imageDisplayLiveViewZoomIn";
            this.imageDisplayLiveViewZoomIn.ShowTitle = true;
            this.imageDisplayLiveViewZoomIn.Size = new System.Drawing.Size(640, 480);
            this.imageDisplayLiveViewZoomIn.SlotLocation = new System.Drawing.Point(13, 31);
            this.imageDisplayLiveViewZoomIn.SlotSize = new System.Drawing.Size(216, 12);
            this.imageDisplayLiveViewZoomIn.StatusBarControlScale = 1D;
            this.imageDisplayLiveViewZoomIn.StatusBarControlScale_X = 0.99375D;
            this.imageDisplayLiveViewZoomIn.StatusBarControlScale_Y = 1D;
            this.imageDisplayLiveViewZoomIn.StatusBarControlSize = new System.Drawing.Size(636, 50);
            this.imageDisplayLiveViewZoomIn.TabIndex = 11;
            this.imageDisplayLiveViewZoomIn.YOffset = 2;
            // 
            // imageDisplayLiveViewZoomOut
            // 
            this.imageDisplayLiveViewZoomOut.AutoShowTitle = true;
            this.imageDisplayLiveViewZoomOut.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.imageDisplayLiveViewZoomOut.BitmapDisplay = null;
            this.imageDisplayLiveViewZoomOut.ColorStatusBarControlBackground = System.Drawing.Color.FromArgb(((int)(((byte)(126)))), ((int)(((byte)(126)))), ((int)(((byte)(126)))));
            this.imageDisplayLiveViewZoomOut.ControlScale = 1D;
            this.imageDisplayLiveViewZoomOut.ControlScale_X = 0.5D;
            this.imageDisplayLiveViewZoomOut.ControlScale_Y = 0.5D;
            this.imageDisplayLiveViewZoomOut.ControlSize = new System.Drawing.Size(320, 240);
            this.imageDisplayLiveViewZoomOut.CurrentValueFont = new System.Drawing.Font("微软雅黑", 5F, System.Drawing.FontStyle.Bold);
            this.imageDisplayLiveViewZoomOut.CurrentValueLocation = new System.Drawing.Point(176, 14);
            this.imageDisplayLiveViewZoomOut.CurrentValueSize = new System.Drawing.Size(54, 7);
            this.imageDisplayLiveViewZoomOut.ImageFileName = "1.jpg";
            this.imageDisplayLiveViewZoomOut.ImageFilePath = ".\\ConfigData\\RejectsImage\\";
            this.imageDisplayLiveViewZoomOut.Information = ((VisionSystemClassLibrary.Struct.ImageInformation)(resources.GetObject("imageDisplayLiveViewZoomOut.Information")));
            this.imageDisplayLiveViewZoomOut.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.imageDisplayLiveViewZoomOut.Location = new System.Drawing.Point(692, 90);
            this.imageDisplayLiveViewZoomOut.MaxValueFont = new System.Drawing.Font("微软雅黑", 5F, System.Drawing.FontStyle.Bold);
            this.imageDisplayLiveViewZoomOut.MaxValueLocation = new System.Drawing.Point(236, 14);
            this.imageDisplayLiveViewZoomOut.MaxValueSize = new System.Drawing.Size(54, 7);
            this.imageDisplayLiveViewZoomOut.MessageFont = new System.Drawing.Font("微软雅黑", 6F, System.Drawing.FontStyle.Bold);
            this.imageDisplayLiveViewZoomOut.MessageLampLocation = new System.Drawing.Point(295, 5);
            this.imageDisplayLiveViewZoomOut.MessageLampSize = new System.Drawing.Size(15, 16);
            this.imageDisplayLiveViewZoomOut.MessageLocation = new System.Drawing.Point(4, 4);
            this.imageDisplayLiveViewZoomOut.MessageSize = new System.Drawing.Size(270, 9);
            this.imageDisplayLiveViewZoomOut.MinValueFont = new System.Drawing.Font("微软雅黑", 5F, System.Drawing.FontStyle.Bold);
            this.imageDisplayLiveViewZoomOut.MinValueLocation = new System.Drawing.Point(117, 14);
            this.imageDisplayLiveViewZoomOut.MinValueSize = new System.Drawing.Size(54, 7);
            this.imageDisplayLiveViewZoomOut.Name = "imageDisplayLiveViewZoomOut";
            this.imageDisplayLiveViewZoomOut.ShowTitle = true;
            this.imageDisplayLiveViewZoomOut.Size = new System.Drawing.Size(320, 240);
            this.imageDisplayLiveViewZoomOut.SlotLocation = new System.Drawing.Point(6, 15);
            this.imageDisplayLiveViewZoomOut.SlotSize = new System.Drawing.Size(108, 6);
            this.imageDisplayLiveViewZoomOut.StatusBarControlScale = 1D;
            this.imageDisplayLiveViewZoomOut.StatusBarControlScale_X = 0.496875D;
            this.imageDisplayLiveViewZoomOut.StatusBarControlScale_Y = 0.5D;
            this.imageDisplayLiveViewZoomOut.StatusBarControlSize = new System.Drawing.Size(318, 25);
            this.imageDisplayLiveViewZoomOut.TabIndex = 12;
            this.imageDisplayLiveViewZoomOut.Visible = false;
            this.imageDisplayLiveViewZoomOut.YOffset = 2;
            // 
            // customButtonZoomInBackground
            // 
            this.customButtonZoomInBackground.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.customButtonZoomInBackground.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.customButtonZoomInBackground.BitmapIconWhole = null;
            this.customButtonZoomInBackground.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.CameraBackground;
            this.customButtonZoomInBackground.Chinese_TextDisplay = new string[] {
        "放大背景"};
            this.customButtonZoomInBackground.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 14)};
            this.customButtonZoomInBackground.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonZoomInBackground.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(39, 23)};
            this.customButtonZoomInBackground.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonZoomInBackground.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonZoomInBackground.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonZoomInBackground.CurrentTextGroupIndex = 0;
            this.customButtonZoomInBackground.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonZoomInBackground.CustomButtonData = null;
            this.customButtonZoomInBackground.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonZoomInBackground.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonZoomInBackground.DrawIcon = true;
            this.customButtonZoomInBackground.DrawText = false;
            this.customButtonZoomInBackground.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonZoomInBackground.English_TextDisplay = new string[] {
        "ZOOMIN BACKGROUND"};
            this.customButtonZoomInBackground.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 14)};
            this.customButtonZoomInBackground.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonZoomInBackground.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(52, 23)};
            this.customButtonZoomInBackground.FocusBackgroundDisplay = false;
            this.customButtonZoomInBackground.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonZoomInBackground.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonZoomInBackground.ForeColor = System.Drawing.Color.White;
            this.customButtonZoomInBackground.HoverBackgroundDisplay = false;
            this.customButtonZoomInBackground.IconIndex = new int[] {
        1,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonZoomInBackground.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonZoomInBackground.IconNumber = 1;
            this.customButtonZoomInBackground.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(0, 0)};
            this.customButtonZoomInBackground.LabelControlMode = true;
            this.customButtonZoomInBackground.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonZoomInBackground.Location = new System.Drawing.Point(16, 82);
            this.customButtonZoomInBackground.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonZoomInBackground.Name = "customButtonZoomInBackground";
            this.customButtonZoomInBackground.RectBottom = new System.Drawing.Rectangle(3, 103, 134, 3);
            this.customButtonZoomInBackground.RectFill = new System.Drawing.Rectangle(3, 3, 134, 100);
            this.customButtonZoomInBackground.RectLeft = new System.Drawing.Rectangle(0, 3, 3, 100);
            this.customButtonZoomInBackground.RectLeftBottom = new System.Drawing.Rectangle(0, 103, 3, 3);
            this.customButtonZoomInBackground.RectLeftTop = new System.Drawing.Rectangle(0, 0, 3, 3);
            this.customButtonZoomInBackground.RectRight = new System.Drawing.Rectangle(137, 3, 3, 100);
            this.customButtonZoomInBackground.RectRightBottom = new System.Drawing.Rectangle(137, 103, 3, 3);
            this.customButtonZoomInBackground.RectRightTop = new System.Drawing.Rectangle(137, 0, 3, 3);
            this.customButtonZoomInBackground.RectTop = new System.Drawing.Rectangle(3, 0, 134, 3);
            this.customButtonZoomInBackground.Size = new System.Drawing.Size(646, 486);
            this.customButtonZoomInBackground.SizeButton = new System.Drawing.Size(646, 486);
            this.customButtonZoomInBackground.TabIndex = 42;
            this.customButtonZoomInBackground.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonZoomInBackground.TextGroupNumber = 1;
            this.customButtonZoomInBackground.UpdateControl = true;
            // 
            // customButtonZoomImage
            // 
            this.customButtonZoomImage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonZoomImage.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonZoomImage.BitmapIconWhole = global::VisionSystemControlLibrary.Properties.Resources.Zoom;
            this.customButtonZoomImage.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.Button;
            this.customButtonZoomImage.Chinese_TextDisplay = new string[] {
        "放大图像&缩小图像"};
            this.customButtonZoomImage.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 17),
        new System.Drawing.Point(5, 17)};
            this.customButtonZoomImage.Chinese_TextNumberInTextGroup = new int[] {
        1,
        1};
            this.customButtonZoomImage.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(72, 23),
        new System.Drawing.Size(72, 23)};
            this.customButtonZoomImage.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonZoomImage.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonZoomImage.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonZoomImage.CurrentTextGroupIndex = 0;
            this.customButtonZoomImage.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonZoomImage.CustomButtonData = null;
            this.customButtonZoomImage.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonZoomImage.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Switch;
            this.customButtonZoomImage.DrawIcon = true;
            this.customButtonZoomImage.DrawText = true;
            this.customButtonZoomImage.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonZoomImage.English_TextDisplay = new string[] {
        "ZOOM&IMAGE IN&ZOOM&IMAGE OUT"};
            this.customButtonZoomImage.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 7),
        new System.Drawing.Point(5, 28),
        new System.Drawing.Point(5, 7),
        new System.Drawing.Point(5, 28)};
            this.customButtonZoomImage.English_TextNumberInTextGroup = new int[] {
        2,
        2};
            this.customButtonZoomImage.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(60, 23),
        new System.Drawing.Size(87, 23),
        new System.Drawing.Size(60, 23),
        new System.Drawing.Size(104, 23)};
            this.customButtonZoomImage.FocusBackgroundDisplay = false;
            this.customButtonZoomImage.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonZoomImage.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonZoomImage.ForeColor = System.Drawing.Color.White;
            this.customButtonZoomImage.HoverBackgroundDisplay = false;
            this.customButtonZoomImage.IconIndex = new int[] {
        2,
        3,
        0,
        1,
        1,
        0,
        1,
        0,
        1};
            this.customButtonZoomImage.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(105, 10),
        new System.Drawing.Point(105, 10),
        new System.Drawing.Point(105, 10),
        new System.Drawing.Point(105, 10)};
            this.customButtonZoomImage.IconNumber = 4;
            this.customButtonZoomImage.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(53, 37),
        new System.Drawing.Size(53, 37),
        new System.Drawing.Size(53, 37),
        new System.Drawing.Size(53, 37)};
            this.customButtonZoomImage.LabelControlMode = false;
            this.customButtonZoomImage.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonZoomImage.Location = new System.Drawing.Point(176, 581);
            this.customButtonZoomImage.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonZoomImage.Name = "customButtonZoomImage";
            this.customButtonZoomImage.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonZoomImage.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonZoomImage.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonZoomImage.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonZoomImage.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonZoomImage.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonZoomImage.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonZoomImage.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonZoomImage.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonZoomImage.Size = new System.Drawing.Size(155, 59);
            this.customButtonZoomImage.SizeButton = new System.Drawing.Size(155, 59);
            this.customButtonZoomImage.TabIndex = 41;
            this.customButtonZoomImage.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonZoomImage.TextGroupNumber = 2;
            this.customButtonZoomImage.UpdateControl = true;
            this.customButtonZoomImage.Visible = false;
            this.customButtonZoomImage.CustomButton_Click += new System.EventHandler(this.customButtonZoomImage_CustomButton_Click);
            // 
            // customButtonSelfTrigger
            // 
            this.customButtonSelfTrigger.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonSelfTrigger.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonSelfTrigger.BitmapIconWhole = global::VisionSystemControlLibrary.Properties.Resources.ResetDevice;
            this.customButtonSelfTrigger.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.Button;
            this.customButtonSelfTrigger.Chinese_TextDisplay = new string[] {
        "自触发"};
            this.customButtonSelfTrigger.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 17)};
            this.customButtonSelfTrigger.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonSelfTrigger.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(55, 23)};
            this.customButtonSelfTrigger.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonSelfTrigger.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonSelfTrigger.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonSelfTrigger.CurrentTextGroupIndex = 0;
            this.customButtonSelfTrigger.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonSelfTrigger.CustomButtonData = null;
            this.customButtonSelfTrigger.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonSelfTrigger.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Switch;
            this.customButtonSelfTrigger.DrawIcon = true;
            this.customButtonSelfTrigger.DrawText = true;
            this.customButtonSelfTrigger.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonSelfTrigger.English_TextDisplay = new string[] {
        "SELF&TRIGGER"};
            this.customButtonSelfTrigger.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 7),
        new System.Drawing.Point(5, 28)};
            this.customButtonSelfTrigger.English_TextNumberInTextGroup = new int[] {
        2};
            this.customButtonSelfTrigger.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(43, 23),
        new System.Drawing.Size(79, 23)};
            this.customButtonSelfTrigger.FocusBackgroundDisplay = false;
            this.customButtonSelfTrigger.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonSelfTrigger.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonSelfTrigger.ForeColor = System.Drawing.Color.White;
            this.customButtonSelfTrigger.HoverBackgroundDisplay = false;
            this.customButtonSelfTrigger.IconIndex = new int[] {
        1,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonSelfTrigger.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(85, 9),
        new System.Drawing.Point(85, 9)};
            this.customButtonSelfTrigger.IconNumber = 2;
            this.customButtonSelfTrigger.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(87, 47),
        new System.Drawing.Size(87, 47)};
            this.customButtonSelfTrigger.LabelControlMode = false;
            this.customButtonSelfTrigger.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonSelfTrigger.Location = new System.Drawing.Point(19, 581);
            this.customButtonSelfTrigger.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonSelfTrigger.Name = "customButtonSelfTrigger";
            this.customButtonSelfTrigger.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonSelfTrigger.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonSelfTrigger.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonSelfTrigger.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonSelfTrigger.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonSelfTrigger.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonSelfTrigger.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonSelfTrigger.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonSelfTrigger.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonSelfTrigger.Size = new System.Drawing.Size(155, 59);
            this.customButtonSelfTrigger.SizeButton = new System.Drawing.Size(155, 59);
            this.customButtonSelfTrigger.TabIndex = 40;
            this.customButtonSelfTrigger.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonSelfTrigger.TextGroupNumber = 1;
            this.customButtonSelfTrigger.UpdateControl = true;
            this.customButtonSelfTrigger.CustomButton_Click += new System.EventHandler(this.customButtonSelfTrigger_CustomButton_Click);
            // 
            // customButtonLastLearntImage
            // 
            this.customButtonLastLearntImage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.customButtonLastLearntImage.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.customButtonLastLearntImage.BitmapIconWhole = null;
            this.customButtonLastLearntImage.BitmapWhole = null;
            this.customButtonLastLearntImage.Chinese_TextDisplay = new string[] {
        "最新学习图像"};
            this.customButtonLastLearntImage.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(120, 0)};
            this.customButtonLastLearntImage.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonLastLearntImage.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(92, 19)};
            this.customButtonLastLearntImage.ColorTextDisable = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
            this.customButtonLastLearntImage.ColorTextEnable = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonLastLearntImage.ColorTextSelected = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonLastLearntImage.CurrentTextGroupIndex = 0;
            this.customButtonLastLearntImage.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonLastLearntImage.CustomButtonData = null;
            this.customButtonLastLearntImage.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Automatic;
            this.customButtonLastLearntImage.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonLastLearntImage.DrawIcon = false;
            this.customButtonLastLearntImage.DrawText = true;
            this.customButtonLastLearntImage.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonLastLearntImage.English_TextDisplay = new string[] {
        "LAST LEARNT IMAGE"};
            this.customButtonLastLearntImage.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(87, 0)};
            this.customButtonLastLearntImage.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonLastLearntImage.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(158, 19)};
            this.customButtonLastLearntImage.FocusBackgroundDisplay = false;
            this.customButtonLastLearntImage.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonLastLearntImage.FontText = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonLastLearntImage.ForeColor = System.Drawing.Color.White;
            this.customButtonLastLearntImage.HoverBackgroundDisplay = false;
            this.customButtonLastLearntImage.IconIndex = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonLastLearntImage.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonLastLearntImage.IconNumber = 1;
            this.customButtonLastLearntImage.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(0, 0)};
            this.customButtonLastLearntImage.LabelControlMode = true;
            this.customButtonLastLearntImage.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonLastLearntImage.Location = new System.Drawing.Point(686, 370);
            this.customButtonLastLearntImage.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonLastLearntImage.Name = "customButtonLastLearntImage";
            this.customButtonLastLearntImage.RectBottom = new System.Drawing.Rectangle(3, 23, 237, 3);
            this.customButtonLastLearntImage.RectFill = new System.Drawing.Rectangle(3, 3, 237, 20);
            this.customButtonLastLearntImage.RectLeft = new System.Drawing.Rectangle(0, 3, 3, 20);
            this.customButtonLastLearntImage.RectLeftBottom = new System.Drawing.Rectangle(0, 23, 3, 3);
            this.customButtonLastLearntImage.RectLeftTop = new System.Drawing.Rectangle(0, 0, 3, 3);
            this.customButtonLastLearntImage.RectRight = new System.Drawing.Rectangle(240, 3, 3, 20);
            this.customButtonLastLearntImage.RectRightBottom = new System.Drawing.Rectangle(240, 23, 3, 3);
            this.customButtonLastLearntImage.RectRightTop = new System.Drawing.Rectangle(240, 0, 3, 3);
            this.customButtonLastLearntImage.RectTop = new System.Drawing.Rectangle(3, 0, 237, 3);
            this.customButtonLastLearntImage.Size = new System.Drawing.Size(333, 18);
            this.customButtonLastLearntImage.SizeButton = new System.Drawing.Size(333, 18);
            this.customButtonLastLearntImage.TabIndex = 39;
            this.customButtonLastLearntImage.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.customButtonLastLearntImage.TextGroupNumber = 1;
            this.customButtonLastLearntImage.UpdateControl = true;
            // 
            // customButtonLiveViewZoomOut
            // 
            this.customButtonLiveViewZoomOut.BackColor = System.Drawing.Color.Black;
            this.customButtonLiveViewZoomOut.BackgroundColor = System.Drawing.Color.Black;
            this.customButtonLiveViewZoomOut.BitmapIconWhole = null;
            this.customButtonLiveViewZoomOut.BitmapWhole = null;
            this.customButtonLiveViewZoomOut.Chinese_TextDisplay = new string[] {
        "在线"};
            this.customButtonLiveViewZoomOut.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(149, 0)};
            this.customButtonLiveViewZoomOut.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonLiveViewZoomOut.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(34, 19)};
            this.customButtonLiveViewZoomOut.ColorTextDisable = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
            this.customButtonLiveViewZoomOut.ColorTextEnable = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonLiveViewZoomOut.ColorTextSelected = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonLiveViewZoomOut.CurrentTextGroupIndex = 0;
            this.customButtonLiveViewZoomOut.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonLiveViewZoomOut.CustomButtonData = null;
            this.customButtonLiveViewZoomOut.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Automatic;
            this.customButtonLiveViewZoomOut.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonLiveViewZoomOut.DrawIcon = false;
            this.customButtonLiveViewZoomOut.DrawText = true;
            this.customButtonLiveViewZoomOut.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonLiveViewZoomOut.English_TextDisplay = new string[] {
        "LIVE VIEW"};
            this.customButtonLiveViewZoomOut.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(126, 0)};
            this.customButtonLiveViewZoomOut.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonLiveViewZoomOut.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(80, 19)};
            this.customButtonLiveViewZoomOut.FocusBackgroundDisplay = false;
            this.customButtonLiveViewZoomOut.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonLiveViewZoomOut.FontText = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonLiveViewZoomOut.ForeColor = System.Drawing.Color.White;
            this.customButtonLiveViewZoomOut.HoverBackgroundDisplay = false;
            this.customButtonLiveViewZoomOut.IconIndex = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonLiveViewZoomOut.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonLiveViewZoomOut.IconNumber = 1;
            this.customButtonLiveViewZoomOut.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(0, 0)};
            this.customButtonLiveViewZoomOut.LabelControlMode = true;
            this.customButtonLiveViewZoomOut.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonLiveViewZoomOut.Location = new System.Drawing.Point(686, 66);
            this.customButtonLiveViewZoomOut.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonLiveViewZoomOut.Name = "customButtonLiveViewZoomOut";
            this.customButtonLiveViewZoomOut.RectBottom = new System.Drawing.Rectangle(3, 23, 237, 3);
            this.customButtonLiveViewZoomOut.RectFill = new System.Drawing.Rectangle(3, 3, 237, 20);
            this.customButtonLiveViewZoomOut.RectLeft = new System.Drawing.Rectangle(0, 3, 3, 20);
            this.customButtonLiveViewZoomOut.RectLeftBottom = new System.Drawing.Rectangle(0, 23, 3, 3);
            this.customButtonLiveViewZoomOut.RectLeftTop = new System.Drawing.Rectangle(0, 0, 3, 3);
            this.customButtonLiveViewZoomOut.RectRight = new System.Drawing.Rectangle(240, 3, 3, 20);
            this.customButtonLiveViewZoomOut.RectRightBottom = new System.Drawing.Rectangle(240, 23, 3, 3);
            this.customButtonLiveViewZoomOut.RectRightTop = new System.Drawing.Rectangle(240, 0, 3, 3);
            this.customButtonLiveViewZoomOut.RectTop = new System.Drawing.Rectangle(3, 0, 237, 3);
            this.customButtonLiveViewZoomOut.Size = new System.Drawing.Size(333, 18);
            this.customButtonLiveViewZoomOut.SizeButton = new System.Drawing.Size(333, 18);
            this.customButtonLiveViewZoomOut.TabIndex = 38;
            this.customButtonLiveViewZoomOut.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.customButtonLiveViewZoomOut.TextGroupNumber = 1;
            this.customButtonLiveViewZoomOut.UpdateControl = false;
            this.customButtonLiveViewZoomOut.Visible = false;
            // 
            // customButtonLiveViewZoomIn
            // 
            this.customButtonLiveViewZoomIn.BackColor = System.Drawing.Color.Black;
            this.customButtonLiveViewZoomIn.BackgroundColor = System.Drawing.Color.Black;
            this.customButtonLiveViewZoomIn.BitmapIconWhole = null;
            this.customButtonLiveViewZoomIn.BitmapWhole = null;
            this.customButtonLiveViewZoomIn.Chinese_TextDisplay = new string[] {
        "放大"};
            this.customButtonLiveViewZoomIn.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(308, 0)};
            this.customButtonLiveViewZoomIn.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonLiveViewZoomIn.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(34, 18)};
            this.customButtonLiveViewZoomIn.ColorTextDisable = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
            this.customButtonLiveViewZoomIn.ColorTextEnable = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonLiveViewZoomIn.ColorTextSelected = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonLiveViewZoomIn.CurrentTextGroupIndex = 0;
            this.customButtonLiveViewZoomIn.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonLiveViewZoomIn.CustomButtonData = null;
            this.customButtonLiveViewZoomIn.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonLiveViewZoomIn.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonLiveViewZoomIn.DrawIcon = false;
            this.customButtonLiveViewZoomIn.DrawText = true;
            this.customButtonLiveViewZoomIn.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonLiveViewZoomIn.English_TextDisplay = new string[] {
        "ZOOMED LIVE VIEW"};
            this.customButtonLiveViewZoomIn.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(250, 0)};
            this.customButtonLiveViewZoomIn.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonLiveViewZoomIn.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(151, 18)};
            this.customButtonLiveViewZoomIn.FocusBackgroundDisplay = false;
            this.customButtonLiveViewZoomIn.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonLiveViewZoomIn.FontText = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonLiveViewZoomIn.ForeColor = System.Drawing.Color.White;
            this.customButtonLiveViewZoomIn.HoverBackgroundDisplay = false;
            this.customButtonLiveViewZoomIn.IconIndex = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonLiveViewZoomIn.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonLiveViewZoomIn.IconNumber = 1;
            this.customButtonLiveViewZoomIn.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(0, 0)};
            this.customButtonLiveViewZoomIn.LabelControlMode = true;
            this.customButtonLiveViewZoomIn.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonLiveViewZoomIn.Location = new System.Drawing.Point(14, 63);
            this.customButtonLiveViewZoomIn.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonLiveViewZoomIn.Name = "customButtonLiveViewZoomIn";
            this.customButtonLiveViewZoomIn.RectBottom = new System.Drawing.Rectangle(3, 23, 237, 3);
            this.customButtonLiveViewZoomIn.RectFill = new System.Drawing.Rectangle(3, 3, 237, 20);
            this.customButtonLiveViewZoomIn.RectLeft = new System.Drawing.Rectangle(0, 3, 3, 20);
            this.customButtonLiveViewZoomIn.RectLeftBottom = new System.Drawing.Rectangle(0, 23, 3, 3);
            this.customButtonLiveViewZoomIn.RectLeftTop = new System.Drawing.Rectangle(0, 0, 3, 3);
            this.customButtonLiveViewZoomIn.RectRight = new System.Drawing.Rectangle(240, 3, 3, 20);
            this.customButtonLiveViewZoomIn.RectRightBottom = new System.Drawing.Rectangle(240, 23, 3, 3);
            this.customButtonLiveViewZoomIn.RectRightTop = new System.Drawing.Rectangle(240, 0, 3, 3);
            this.customButtonLiveViewZoomIn.RectTop = new System.Drawing.Rectangle(3, 0, 237, 3);
            this.customButtonLiveViewZoomIn.Size = new System.Drawing.Size(651, 18);
            this.customButtonLiveViewZoomIn.SizeButton = new System.Drawing.Size(651, 18);
            this.customButtonLiveViewZoomIn.TabIndex = 37;
            this.customButtonLiveViewZoomIn.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.customButtonLiveViewZoomIn.TextGroupNumber = 1;
            this.customButtonLiveViewZoomIn.UpdateControl = true;
            // 
            // customButtonClose
            // 
            this.customButtonClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonClose.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonClose.BitmapIconWhole = global::VisionSystemControlLibrary.Properties.Resources.Close;
            this.customButtonClose.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.Button;
            this.customButtonClose.Chinese_TextDisplay = new string[] {
        "返回"};
            this.customButtonClose.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 14)};
            this.customButtonClose.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonClose.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(39, 23)};
            this.customButtonClose.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonClose.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonClose.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonClose.CurrentTextGroupIndex = 0;
            this.customButtonClose.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonClose.CustomButtonData = null;
            this.customButtonClose.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonClose.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonClose.DrawIcon = true;
            this.customButtonClose.DrawText = false;
            this.customButtonClose.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonClose.English_TextDisplay = new string[] {
        "BACK"};
            this.customButtonClose.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 14)};
            this.customButtonClose.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonClose.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(52, 23)};
            this.customButtonClose.FocusBackgroundDisplay = false;
            this.customButtonClose.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonClose.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonClose.ForeColor = System.Drawing.Color.White;
            this.customButtonClose.HoverBackgroundDisplay = false;
            this.customButtonClose.IconIndex = new int[] {
        1,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonClose.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(28, 1),
        new System.Drawing.Point(28, 1)};
            this.customButtonClose.IconNumber = 2;
            this.customButtonClose.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(70, 48),
        new System.Drawing.Size(70, 48)};
            this.customButtonClose.LabelControlMode = false;
            this.customButtonClose.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonClose.Location = new System.Drawing.Point(891, 10);
            this.customButtonClose.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonClose.Name = "customButtonClose";
            this.customButtonClose.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonClose.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonClose.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonClose.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonClose.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonClose.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonClose.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonClose.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonClose.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonClose.Size = new System.Drawing.Size(129, 51);
            this.customButtonClose.SizeButton = new System.Drawing.Size(129, 51);
            this.customButtonClose.TabIndex = 36;
            this.customButtonClose.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonClose.TextGroupNumber = 1;
            this.customButtonClose.UpdateControl = true;
            this.customButtonClose.CustomButton_Click += new System.EventHandler(this.customButtonClose_CustomButton_Click);
            // 
            // customButtonCaption
            // 
            this.customButtonCaption.BackColor = System.Drawing.Color.Black;
            this.customButtonCaption.BackgroundColor = System.Drawing.Color.Black;
            this.customButtonCaption.BitmapIconWhole = null;
            this.customButtonCaption.BitmapWhole = null;
            this.customButtonCaption.Chinese_TextDisplay = new string[] {
        " - 在线"};
            this.customButtonCaption.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(353, 7)};
            this.customButtonCaption.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonCaption.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(70, 29)};
            this.customButtonCaption.ColorTextDisable = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
            this.customButtonCaption.ColorTextEnable = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonCaption.ColorTextSelected = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonCaption.CurrentTextGroupIndex = 0;
            this.customButtonCaption.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonCaption.CustomButtonData = null;
            this.customButtonCaption.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Automatic;
            this.customButtonCaption.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonCaption.DrawIcon = false;
            this.customButtonCaption.DrawText = true;
            this.customButtonCaption.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonCaption.English_TextDisplay = new string[] {
        " - LIVE VIEW"};
            this.customButtonCaption.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(320, 7)};
            this.customButtonCaption.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonCaption.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(135, 29)};
            this.customButtonCaption.FocusBackgroundDisplay = false;
            this.customButtonCaption.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonCaption.FontText = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonCaption.ForeColor = System.Drawing.Color.White;
            this.customButtonCaption.HoverBackgroundDisplay = false;
            this.customButtonCaption.IconIndex = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonCaption.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonCaption.IconNumber = 1;
            this.customButtonCaption.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(0, 0)};
            this.customButtonCaption.LabelControlMode = true;
            this.customButtonCaption.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonCaption.Location = new System.Drawing.Point(107, 14);
            this.customButtonCaption.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonCaption.Name = "customButtonCaption";
            this.customButtonCaption.RectBottom = new System.Drawing.Rectangle(3, 23, 237, 3);
            this.customButtonCaption.RectFill = new System.Drawing.Rectangle(3, 3, 237, 20);
            this.customButtonCaption.RectLeft = new System.Drawing.Rectangle(0, 3, 3, 20);
            this.customButtonCaption.RectLeftBottom = new System.Drawing.Rectangle(0, 23, 3, 3);
            this.customButtonCaption.RectLeftTop = new System.Drawing.Rectangle(0, 0, 3, 3);
            this.customButtonCaption.RectRight = new System.Drawing.Rectangle(240, 3, 3, 20);
            this.customButtonCaption.RectRightBottom = new System.Drawing.Rectangle(240, 23, 3, 3);
            this.customButtonCaption.RectRightTop = new System.Drawing.Rectangle(240, 0, 3, 3);
            this.customButtonCaption.RectTop = new System.Drawing.Rectangle(3, 0, 237, 3);
            this.customButtonCaption.Size = new System.Drawing.Size(776, 44);
            this.customButtonCaption.SizeButton = new System.Drawing.Size(776, 44);
            this.customButtonCaption.TabIndex = 33;
            this.customButtonCaption.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.customButtonCaption.TextGroupNumber = 1;
            this.customButtonCaption.UpdateControl = true;
            // 
            // imageDisplayLastLearntImage
            // 
            this.imageDisplayLastLearntImage.AutoShowTitle = false;
            this.imageDisplayLastLearntImage.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.imageDisplayLastLearntImage.BitmapDisplay = null;
            this.imageDisplayLastLearntImage.ColorStatusBarControlBackground = System.Drawing.Color.FromArgb(((int)(((byte)(126)))), ((int)(((byte)(126)))), ((int)(((byte)(126)))));
            this.imageDisplayLastLearntImage.ControlScale = 1D;
            this.imageDisplayLastLearntImage.ControlScale_X = 0.5D;
            this.imageDisplayLastLearntImage.ControlScale_Y = 0.5D;
            this.imageDisplayLastLearntImage.ControlSize = new System.Drawing.Size(320, 240);
            this.imageDisplayLastLearntImage.CurrentValueFont = new System.Drawing.Font("微软雅黑", 5F, System.Drawing.FontStyle.Bold);
            this.imageDisplayLastLearntImage.CurrentValueLocation = new System.Drawing.Point(176, 14);
            this.imageDisplayLastLearntImage.CurrentValueSize = new System.Drawing.Size(54, 7);
            this.imageDisplayLastLearntImage.ImageFileName = "1.jpg";
            this.imageDisplayLastLearntImage.ImageFilePath = ".\\ConfigData\\RejectsImage\\";
            this.imageDisplayLastLearntImage.Information = ((VisionSystemClassLibrary.Struct.ImageInformation)(resources.GetObject("imageDisplayLastLearntImage.Information")));
            this.imageDisplayLastLearntImage.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.imageDisplayLastLearntImage.Location = new System.Drawing.Point(692, 394);
            this.imageDisplayLastLearntImage.MaxValueFont = new System.Drawing.Font("微软雅黑", 5F, System.Drawing.FontStyle.Bold);
            this.imageDisplayLastLearntImage.MaxValueLocation = new System.Drawing.Point(236, 14);
            this.imageDisplayLastLearntImage.MaxValueSize = new System.Drawing.Size(54, 7);
            this.imageDisplayLastLearntImage.MessageFont = new System.Drawing.Font("微软雅黑", 6F, System.Drawing.FontStyle.Bold);
            this.imageDisplayLastLearntImage.MessageLampLocation = new System.Drawing.Point(295, 5);
            this.imageDisplayLastLearntImage.MessageLampSize = new System.Drawing.Size(15, 16);
            this.imageDisplayLastLearntImage.MessageLocation = new System.Drawing.Point(4, 4);
            this.imageDisplayLastLearntImage.MessageSize = new System.Drawing.Size(270, 9);
            this.imageDisplayLastLearntImage.MinValueFont = new System.Drawing.Font("微软雅黑", 5F, System.Drawing.FontStyle.Bold);
            this.imageDisplayLastLearntImage.MinValueLocation = new System.Drawing.Point(117, 14);
            this.imageDisplayLastLearntImage.MinValueSize = new System.Drawing.Size(54, 7);
            this.imageDisplayLastLearntImage.Name = "imageDisplayLastLearntImage";
            this.imageDisplayLastLearntImage.ShowTitle = false;
            this.imageDisplayLastLearntImage.Size = new System.Drawing.Size(320, 240);
            this.imageDisplayLastLearntImage.SlotLocation = new System.Drawing.Point(6, 15);
            this.imageDisplayLastLearntImage.SlotSize = new System.Drawing.Size(108, 6);
            this.imageDisplayLastLearntImage.StatusBarControlScale = 1D;
            this.imageDisplayLastLearntImage.StatusBarControlScale_X = 0.496875D;
            this.imageDisplayLastLearntImage.StatusBarControlScale_Y = 0.5D;
            this.imageDisplayLastLearntImage.StatusBarControlSize = new System.Drawing.Size(318, 25);
            this.imageDisplayLastLearntImage.TabIndex = 15;
            this.imageDisplayLastLearntImage.YOffset = 2;
            // 
            // customButtonZoomOutBackground
            // 
            this.customButtonZoomOutBackground.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.customButtonZoomOutBackground.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.customButtonZoomOutBackground.BitmapIconWhole = null;
            this.customButtonZoomOutBackground.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.CameraBackground;
            this.customButtonZoomOutBackground.Chinese_TextDisplay = new string[] {
        "缩小背景"};
            this.customButtonZoomOutBackground.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 14)};
            this.customButtonZoomOutBackground.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonZoomOutBackground.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(39, 23)};
            this.customButtonZoomOutBackground.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonZoomOutBackground.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonZoomOutBackground.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonZoomOutBackground.CurrentTextGroupIndex = 0;
            this.customButtonZoomOutBackground.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonZoomOutBackground.CustomButtonData = null;
            this.customButtonZoomOutBackground.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonZoomOutBackground.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonZoomOutBackground.DrawIcon = true;
            this.customButtonZoomOutBackground.DrawText = false;
            this.customButtonZoomOutBackground.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonZoomOutBackground.English_TextDisplay = new string[] {
        "ZOOMOUT BACKGROUND"};
            this.customButtonZoomOutBackground.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 14)};
            this.customButtonZoomOutBackground.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonZoomOutBackground.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(52, 23)};
            this.customButtonZoomOutBackground.FocusBackgroundDisplay = false;
            this.customButtonZoomOutBackground.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonZoomOutBackground.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonZoomOutBackground.ForeColor = System.Drawing.Color.White;
            this.customButtonZoomOutBackground.HoverBackgroundDisplay = false;
            this.customButtonZoomOutBackground.IconIndex = new int[] {
        1,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonZoomOutBackground.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonZoomOutBackground.IconNumber = 1;
            this.customButtonZoomOutBackground.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(0, 0)};
            this.customButtonZoomOutBackground.LabelControlMode = true;
            this.customButtonZoomOutBackground.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonZoomOutBackground.Location = new System.Drawing.Point(689, 87);
            this.customButtonZoomOutBackground.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonZoomOutBackground.Name = "customButtonZoomOutBackground";
            this.customButtonZoomOutBackground.RectBottom = new System.Drawing.Rectangle(3, 103, 134, 3);
            this.customButtonZoomOutBackground.RectFill = new System.Drawing.Rectangle(3, 3, 134, 100);
            this.customButtonZoomOutBackground.RectLeft = new System.Drawing.Rectangle(0, 3, 3, 100);
            this.customButtonZoomOutBackground.RectLeftBottom = new System.Drawing.Rectangle(0, 103, 3, 3);
            this.customButtonZoomOutBackground.RectLeftTop = new System.Drawing.Rectangle(0, 0, 3, 3);
            this.customButtonZoomOutBackground.RectRight = new System.Drawing.Rectangle(137, 3, 3, 100);
            this.customButtonZoomOutBackground.RectRightBottom = new System.Drawing.Rectangle(137, 103, 3, 3);
            this.customButtonZoomOutBackground.RectRightTop = new System.Drawing.Rectangle(137, 0, 3, 3);
            this.customButtonZoomOutBackground.RectTop = new System.Drawing.Rectangle(3, 0, 134, 3);
            this.customButtonZoomOutBackground.Size = new System.Drawing.Size(326, 246);
            this.customButtonZoomOutBackground.SizeButton = new System.Drawing.Size(326, 246);
            this.customButtonZoomOutBackground.TabIndex = 43;
            this.customButtonZoomOutBackground.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonZoomOutBackground.TextGroupNumber = 1;
            this.customButtonZoomOutBackground.UpdateControl = true;
            this.customButtonZoomOutBackground.Visible = false;
            // 
            // customButtonLastLearntImageBackground
            // 
            this.customButtonLastLearntImageBackground.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.customButtonLastLearntImageBackground.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.customButtonLastLearntImageBackground.BitmapIconWhole = null;
            this.customButtonLastLearntImageBackground.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.CameraBackground;
            this.customButtonLastLearntImageBackground.Chinese_TextDisplay = new string[] {
        "最新学习图像背景"};
            this.customButtonLastLearntImageBackground.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 14)};
            this.customButtonLastLearntImageBackground.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonLastLearntImageBackground.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(39, 23)};
            this.customButtonLastLearntImageBackground.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonLastLearntImageBackground.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonLastLearntImageBackground.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonLastLearntImageBackground.CurrentTextGroupIndex = 0;
            this.customButtonLastLearntImageBackground.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonLastLearntImageBackground.CustomButtonData = null;
            this.customButtonLastLearntImageBackground.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonLastLearntImageBackground.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonLastLearntImageBackground.DrawIcon = true;
            this.customButtonLastLearntImageBackground.DrawText = false;
            this.customButtonLastLearntImageBackground.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonLastLearntImageBackground.English_TextDisplay = new string[] {
        "LAST LEARNT IMAGE BACKGROUND"};
            this.customButtonLastLearntImageBackground.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 14)};
            this.customButtonLastLearntImageBackground.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonLastLearntImageBackground.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(52, 23)};
            this.customButtonLastLearntImageBackground.FocusBackgroundDisplay = false;
            this.customButtonLastLearntImageBackground.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonLastLearntImageBackground.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonLastLearntImageBackground.ForeColor = System.Drawing.Color.White;
            this.customButtonLastLearntImageBackground.HoverBackgroundDisplay = false;
            this.customButtonLastLearntImageBackground.IconIndex = new int[] {
        1,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonLastLearntImageBackground.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonLastLearntImageBackground.IconNumber = 1;
            this.customButtonLastLearntImageBackground.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(0, 0)};
            this.customButtonLastLearntImageBackground.LabelControlMode = true;
            this.customButtonLastLearntImageBackground.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonLastLearntImageBackground.Location = new System.Drawing.Point(689, 391);
            this.customButtonLastLearntImageBackground.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonLastLearntImageBackground.Name = "customButtonLastLearntImageBackground";
            this.customButtonLastLearntImageBackground.RectBottom = new System.Drawing.Rectangle(3, 103, 134, 3);
            this.customButtonLastLearntImageBackground.RectFill = new System.Drawing.Rectangle(3, 3, 134, 100);
            this.customButtonLastLearntImageBackground.RectLeft = new System.Drawing.Rectangle(0, 3, 3, 100);
            this.customButtonLastLearntImageBackground.RectLeftBottom = new System.Drawing.Rectangle(0, 103, 3, 3);
            this.customButtonLastLearntImageBackground.RectLeftTop = new System.Drawing.Rectangle(0, 0, 3, 3);
            this.customButtonLastLearntImageBackground.RectRight = new System.Drawing.Rectangle(137, 3, 3, 100);
            this.customButtonLastLearntImageBackground.RectRightBottom = new System.Drawing.Rectangle(137, 103, 3, 3);
            this.customButtonLastLearntImageBackground.RectRightTop = new System.Drawing.Rectangle(137, 0, 3, 3);
            this.customButtonLastLearntImageBackground.RectTop = new System.Drawing.Rectangle(3, 0, 134, 3);
            this.customButtonLastLearntImageBackground.Size = new System.Drawing.Size(326, 246);
            this.customButtonLastLearntImageBackground.SizeButton = new System.Drawing.Size(326, 246);
            this.customButtonLastLearntImageBackground.TabIndex = 44;
            this.customButtonLastLearntImageBackground.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonLastLearntImageBackground.TextGroupNumber = 1;
            this.customButtonLastLearntImageBackground.UpdateControl = true;
            // 
            // LiveControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.Controls.Add(this.labelIcon);
            this.Controls.Add(this.imageDisplayLiveViewZoomIn);
            this.Controls.Add(this.imageDisplayLiveViewZoomOut);
            this.Controls.Add(this.customButtonZoomInBackground);
            this.Controls.Add(this.customButtonZoomImage);
            this.Controls.Add(this.customButtonSelfTrigger);
            this.Controls.Add(this.customButtonLastLearntImage);
            this.Controls.Add(this.customButtonLiveViewZoomOut);
            this.Controls.Add(this.customButtonLiveViewZoomIn);
            this.Controls.Add(this.customButtonClose);
            this.Controls.Add(this.customButtonCaption);
            this.Controls.Add(this.imageDisplayLastLearntImage);
            this.Controls.Add(this.pictureBoxBackground);
            this.Controls.Add(this.customButtonZoomOutBackground);
            this.Controls.Add(this.customButtonLastLearntImageBackground);
            this.Name = "LiveControl";
            this.Size = new System.Drawing.Size(1024, 662);
            this.Load += new System.EventHandler(this.LiveControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBackground)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ImageDisplay imageDisplayLiveViewZoomIn;
        private ImageDisplay imageDisplayLiveViewZoomOut;
        private ImageDisplay imageDisplayLastLearntImage;
        private System.Windows.Forms.PictureBox pictureBoxBackground;
        private CustomButton customButtonCaption;
        private CustomButton customButtonClose;
        private CustomButton customButtonLiveViewZoomIn;
        private CustomButton customButtonLiveViewZoomOut;
        private CustomButton customButtonLastLearntImage;
        private CustomButton customButtonSelfTrigger;
        private CustomButton customButtonZoomImage;
        private CustomButton customButtonZoomInBackground;
        private CustomButton customButtonZoomOutBackground;
        private CustomButton customButtonLastLearntImageBackground;
        private System.Windows.Forms.Label labelIcon;
    }
}
