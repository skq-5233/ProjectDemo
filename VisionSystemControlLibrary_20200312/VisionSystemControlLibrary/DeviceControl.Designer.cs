namespace VisionSystemControlLibrary
{
    partial class DeviceControl
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeviceControl));
            this.labelMessage1 = new System.Windows.Forms.Label();
            this.customButtonAlignDateTime = new VisionSystemControlLibrary.CustomButton();
            this.customButtonConfigImage = new VisionSystemControlLibrary.CustomButton();
            this.customButtonTestIO = new VisionSystemControlLibrary.CustomButton();
            this.customButtonConfigDevice = new VisionSystemControlLibrary.CustomButton();
            this.customButtonResetDevice = new VisionSystemControlLibrary.CustomButton();
            this.customButtonRefreshList = new VisionSystemControlLibrary.CustomButton();
            this.customButtonMessage3 = new VisionSystemControlLibrary.CustomButton();
            this.customButtonMessage2 = new VisionSystemControlLibrary.CustomButton();
            this.customButtonMessage1 = new VisionSystemControlLibrary.CustomButton();
            this.customButtonClose = new VisionSystemControlLibrary.CustomButton();
            this.customButtonCaption = new VisionSystemControlLibrary.CustomButton();
            this.customList = new VisionSystemControlLibrary.CustomList();
            this.customButtonNextPage = new VisionSystemControlLibrary.CustomButton();
            this.customButtonPreviousPage = new VisionSystemControlLibrary.CustomButton();
            this.timerConfigDevice = new System.Windows.Forms.Timer(this.components);
            this.labelIcon = new System.Windows.Forms.Label();
            this.customButtonParameterSettings = new VisionSystemControlLibrary.CustomButton();
            this.SuspendLayout();
            // 
            // labelMessage1
            // 
            this.labelMessage1.BackColor = System.Drawing.Color.Black;
            this.labelMessage1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelMessage1.ForeColor = System.Drawing.Color.White;
            this.labelMessage1.Location = new System.Drawing.Point(14, 113);
            this.labelMessage1.Name = "labelMessage1";
            this.labelMessage1.Size = new System.Drawing.Size(549, 30);
            this.labelMessage1.TabIndex = 3;
            this.labelMessage1.Text = "LABEL（Dev.Name）";
            this.labelMessage1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // customButtonAlignDateTime
            // 
            this.customButtonAlignDateTime.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonAlignDateTime.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonAlignDateTime.BitmapIconWhole = global::VisionSystemControlLibrary.Properties.Resources.AlignDateTime;
            this.customButtonAlignDateTime.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.Button;
            this.customButtonAlignDateTime.Chinese_TextDisplay = new string[] {
        "同步&日期/时间"};
            this.customButtonAlignDateTime.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 7),
        new System.Drawing.Point(5, 27)};
            this.customButtonAlignDateTime.Chinese_TextNumberInTextGroup = new int[] {
        2};
            this.customButtonAlignDateTime.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(39, 23),
        new System.Drawing.Size(80, 23)};
            this.customButtonAlignDateTime.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonAlignDateTime.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonAlignDateTime.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonAlignDateTime.CurrentTextGroupIndex = 0;
            this.customButtonAlignDateTime.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonAlignDateTime.CustomButtonData = null;
            this.customButtonAlignDateTime.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonAlignDateTime.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonAlignDateTime.DrawIcon = true;
            this.customButtonAlignDateTime.DrawText = true;
            this.customButtonAlignDateTime.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonAlignDateTime.English_TextDisplay = new string[] {
        "ALIGN&DATE/TIME"};
            this.customButtonAlignDateTime.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 7),
        new System.Drawing.Point(5, 27)};
            this.customButtonAlignDateTime.English_TextNumberInTextGroup = new int[] {
        2};
            this.customButtonAlignDateTime.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(59, 23),
        new System.Drawing.Size(101, 23)};
            this.customButtonAlignDateTime.FocusBackgroundDisplay = false;
            this.customButtonAlignDateTime.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonAlignDateTime.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonAlignDateTime.ForeColor = System.Drawing.Color.White;
            this.customButtonAlignDateTime.HoverBackgroundDisplay = false;
            this.customButtonAlignDateTime.IconIndex = new int[] {
        1,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonAlignDateTime.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(90, 10),
        new System.Drawing.Point(90, 10)};
            this.customButtonAlignDateTime.IconNumber = 2;
            this.customButtonAlignDateTime.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(53, 37),
        new System.Drawing.Size(53, 37)};
            this.customButtonAlignDateTime.LabelControlMode = false;
            this.customButtonAlignDateTime.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonAlignDateTime.Location = new System.Drawing.Point(868, 455);
            this.customButtonAlignDateTime.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonAlignDateTime.Name = "customButtonAlignDateTime";
            this.customButtonAlignDateTime.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonAlignDateTime.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonAlignDateTime.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonAlignDateTime.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonAlignDateTime.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonAlignDateTime.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonAlignDateTime.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonAlignDateTime.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonAlignDateTime.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonAlignDateTime.Size = new System.Drawing.Size(144, 56);
            this.customButtonAlignDateTime.SizeButton = new System.Drawing.Size(144, 56);
            this.customButtonAlignDateTime.TabIndex = 54;
            this.customButtonAlignDateTime.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonAlignDateTime.TextGroupNumber = 1;
            this.customButtonAlignDateTime.UpdateControl = true;
            this.customButtonAlignDateTime.CustomButton_Click += new System.EventHandler(this.customButtonAlignDateTime_CustomButton_Click);
            // 
            // customButtonConfigImage
            // 
            this.customButtonConfigImage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonConfigImage.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonConfigImage.BitmapIconWhole = global::VisionSystemControlLibrary.Properties.Resources.ConfigImage;
            this.customButtonConfigImage.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.Button;
            this.customButtonConfigImage.Chinese_TextDisplay = new string[] {
        "配置图像"};
            this.customButtonConfigImage.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 16)};
            this.customButtonConfigImage.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonConfigImage.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(72, 23)};
            this.customButtonConfigImage.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonConfigImage.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonConfigImage.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonConfigImage.CurrentTextGroupIndex = 0;
            this.customButtonConfigImage.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonConfigImage.CustomButtonData = null;
            this.customButtonConfigImage.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonConfigImage.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonConfigImage.DrawIcon = true;
            this.customButtonConfigImage.DrawText = true;
            this.customButtonConfigImage.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonConfigImage.English_TextDisplay = new string[] {
        "CONFIG&IMAGE"};
            this.customButtonConfigImage.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 7),
        new System.Drawing.Point(5, 27)};
            this.customButtonConfigImage.English_TextNumberInTextGroup = new int[] {
        2};
            this.customButtonConfigImage.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(72, 23),
        new System.Drawing.Size(63, 23)};
            this.customButtonConfigImage.FocusBackgroundDisplay = false;
            this.customButtonConfigImage.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonConfigImage.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonConfigImage.ForeColor = System.Drawing.Color.White;
            this.customButtonConfigImage.HoverBackgroundDisplay = false;
            this.customButtonConfigImage.IconIndex = new int[] {
        1,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonConfigImage.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(88, 10),
        new System.Drawing.Point(88, 10)};
            this.customButtonConfigImage.IconNumber = 2;
            this.customButtonConfigImage.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(53, 37),
        new System.Drawing.Size(53, 37)};
            this.customButtonConfigImage.LabelControlMode = false;
            this.customButtonConfigImage.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonConfigImage.Location = new System.Drawing.Point(868, 395);
            this.customButtonConfigImage.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonConfigImage.Name = "customButtonConfigImage";
            this.customButtonConfigImage.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonConfigImage.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonConfigImage.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonConfigImage.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonConfigImage.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonConfigImage.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonConfigImage.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonConfigImage.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonConfigImage.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonConfigImage.Size = new System.Drawing.Size(144, 56);
            this.customButtonConfigImage.SizeButton = new System.Drawing.Size(144, 56);
            this.customButtonConfigImage.TabIndex = 53;
            this.customButtonConfigImage.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonConfigImage.TextGroupNumber = 1;
            this.customButtonConfigImage.UpdateControl = true;
            this.customButtonConfigImage.CustomButton_Click += new System.EventHandler(this.customButtonConfigImage_CustomButton_Click);
            // 
            // customButtonTestIO
            // 
            this.customButtonTestIO.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonTestIO.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonTestIO.BitmapIconWhole = global::VisionSystemControlLibrary.Properties.Resources.RunStop;
            this.customButtonTestIO.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.Button;
            this.customButtonTestIO.Chinese_TextDisplay = new string[] {
        "测试 I/O"};
            this.customButtonTestIO.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 16)};
            this.customButtonTestIO.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonTestIO.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(70, 23)};
            this.customButtonTestIO.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonTestIO.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonTestIO.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonTestIO.CurrentTextGroupIndex = 0;
            this.customButtonTestIO.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonTestIO.CustomButtonData = null;
            this.customButtonTestIO.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonTestIO.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Switch;
            this.customButtonTestIO.DrawIcon = true;
            this.customButtonTestIO.DrawText = true;
            this.customButtonTestIO.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonTestIO.English_TextDisplay = new string[] {
        "TEST I/O"};
            this.customButtonTestIO.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 16)};
            this.customButtonTestIO.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonTestIO.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(77, 23)};
            this.customButtonTestIO.FocusBackgroundDisplay = false;
            this.customButtonTestIO.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonTestIO.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonTestIO.ForeColor = System.Drawing.Color.White;
            this.customButtonTestIO.HoverBackgroundDisplay = false;
            this.customButtonTestIO.IconIndex = new int[] {
        3,
        2,
        1,
        0,
        0,
        1,
        0,
        1,
        0};
            this.customButtonTestIO.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(88, 10),
        new System.Drawing.Point(88, 10),
        new System.Drawing.Point(88, 10),
        new System.Drawing.Point(88, 10)};
            this.customButtonTestIO.IconNumber = 4;
            this.customButtonTestIO.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(53, 37),
        new System.Drawing.Size(53, 37),
        new System.Drawing.Size(53, 37),
        new System.Drawing.Size(53, 37)};
            this.customButtonTestIO.LabelControlMode = false;
            this.customButtonTestIO.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonTestIO.Location = new System.Drawing.Point(868, 336);
            this.customButtonTestIO.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonTestIO.Name = "customButtonTestIO";
            this.customButtonTestIO.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonTestIO.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonTestIO.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonTestIO.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonTestIO.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonTestIO.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonTestIO.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonTestIO.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonTestIO.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonTestIO.Size = new System.Drawing.Size(144, 56);
            this.customButtonTestIO.SizeButton = new System.Drawing.Size(144, 56);
            this.customButtonTestIO.TabIndex = 52;
            this.customButtonTestIO.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonTestIO.TextGroupNumber = 1;
            this.customButtonTestIO.UpdateControl = true;
            this.customButtonTestIO.CustomButton_Click += new System.EventHandler(this.customButtonTestIO_CustomButton_Click);
            // 
            // customButtonConfigDevice
            // 
            this.customButtonConfigDevice.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonConfigDevice.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonConfigDevice.BitmapIconWhole = global::VisionSystemControlLibrary.Properties.Resources.ConfigDevice;
            this.customButtonConfigDevice.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.Button;
            this.customButtonConfigDevice.Chinese_TextDisplay = new string[] {
        "配置设备"};
            this.customButtonConfigDevice.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 16)};
            this.customButtonConfigDevice.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonConfigDevice.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(72, 23)};
            this.customButtonConfigDevice.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonConfigDevice.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonConfigDevice.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonConfigDevice.CurrentTextGroupIndex = 0;
            this.customButtonConfigDevice.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonConfigDevice.CustomButtonData = null;
            this.customButtonConfigDevice.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonConfigDevice.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonConfigDevice.DrawIcon = true;
            this.customButtonConfigDevice.DrawText = true;
            this.customButtonConfigDevice.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonConfigDevice.English_TextDisplay = new string[] {
        "CONFIG&DEVICE"};
            this.customButtonConfigDevice.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 7),
        new System.Drawing.Point(5, 27)};
            this.customButtonConfigDevice.English_TextNumberInTextGroup = new int[] {
        2};
            this.customButtonConfigDevice.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(72, 23),
        new System.Drawing.Size(66, 23)};
            this.customButtonConfigDevice.FocusBackgroundDisplay = false;
            this.customButtonConfigDevice.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonConfigDevice.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonConfigDevice.ForeColor = System.Drawing.Color.White;
            this.customButtonConfigDevice.HoverBackgroundDisplay = false;
            this.customButtonConfigDevice.IconIndex = new int[] {
        1,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonConfigDevice.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(84, 10),
        new System.Drawing.Point(84, 10)};
            this.customButtonConfigDevice.IconNumber = 2;
            this.customButtonConfigDevice.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(53, 37),
        new System.Drawing.Size(53, 37)};
            this.customButtonConfigDevice.LabelControlMode = false;
            this.customButtonConfigDevice.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonConfigDevice.Location = new System.Drawing.Point(868, 218);
            this.customButtonConfigDevice.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonConfigDevice.Name = "customButtonConfigDevice";
            this.customButtonConfigDevice.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonConfigDevice.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonConfigDevice.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonConfigDevice.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonConfigDevice.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonConfigDevice.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonConfigDevice.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonConfigDevice.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonConfigDevice.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonConfigDevice.Size = new System.Drawing.Size(144, 56);
            this.customButtonConfigDevice.SizeButton = new System.Drawing.Size(144, 56);
            this.customButtonConfigDevice.TabIndex = 51;
            this.customButtonConfigDevice.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonConfigDevice.TextGroupNumber = 1;
            this.customButtonConfigDevice.UpdateControl = true;
            this.customButtonConfigDevice.CustomButton_Click += new System.EventHandler(this.customButtonConfigDevice_CustomButton_Click);
            // 
            // customButtonResetDevice
            // 
            this.customButtonResetDevice.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonResetDevice.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonResetDevice.BitmapIconWhole = global::VisionSystemControlLibrary.Properties.Resources.ResetDevice;
            this.customButtonResetDevice.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.Button;
            this.customButtonResetDevice.Chinese_TextDisplay = new string[] {
        "复位设备"};
            this.customButtonResetDevice.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 16)};
            this.customButtonResetDevice.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonResetDevice.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(72, 23)};
            this.customButtonResetDevice.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonResetDevice.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonResetDevice.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonResetDevice.CurrentTextGroupIndex = 0;
            this.customButtonResetDevice.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonResetDevice.CustomButtonData = null;
            this.customButtonResetDevice.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonResetDevice.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonResetDevice.DrawIcon = true;
            this.customButtonResetDevice.DrawText = true;
            this.customButtonResetDevice.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonResetDevice.English_TextDisplay = new string[] {
        "RESET&DEVICE"};
            this.customButtonResetDevice.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 7),
        new System.Drawing.Point(5, 27)};
            this.customButtonResetDevice.English_TextNumberInTextGroup = new int[] {
        2};
            this.customButtonResetDevice.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(56, 23),
        new System.Drawing.Size(66, 23)};
            this.customButtonResetDevice.FocusBackgroundDisplay = false;
            this.customButtonResetDevice.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonResetDevice.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonResetDevice.ForeColor = System.Drawing.Color.White;
            this.customButtonResetDevice.HoverBackgroundDisplay = false;
            this.customButtonResetDevice.IconIndex = new int[] {
        1,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonResetDevice.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(73, 8),
        new System.Drawing.Point(73, 8)};
            this.customButtonResetDevice.IconNumber = 2;
            this.customButtonResetDevice.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(87, 47),
        new System.Drawing.Size(87, 47)};
            this.customButtonResetDevice.LabelControlMode = false;
            this.customButtonResetDevice.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonResetDevice.Location = new System.Drawing.Point(868, 159);
            this.customButtonResetDevice.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonResetDevice.Name = "customButtonResetDevice";
            this.customButtonResetDevice.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonResetDevice.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonResetDevice.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonResetDevice.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonResetDevice.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonResetDevice.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonResetDevice.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonResetDevice.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonResetDevice.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonResetDevice.Size = new System.Drawing.Size(144, 56);
            this.customButtonResetDevice.SizeButton = new System.Drawing.Size(144, 56);
            this.customButtonResetDevice.TabIndex = 50;
            this.customButtonResetDevice.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonResetDevice.TextGroupNumber = 1;
            this.customButtonResetDevice.UpdateControl = true;
            this.customButtonResetDevice.CustomButton_Click += new System.EventHandler(this.customButtonResetDevice_CustomButton_Click);
            // 
            // customButtonRefreshList
            // 
            this.customButtonRefreshList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonRefreshList.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonRefreshList.BitmapIconWhole = global::VisionSystemControlLibrary.Properties.Resources.RefreshList;
            this.customButtonRefreshList.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.Button;
            this.customButtonRefreshList.Chinese_TextDisplay = new string[] {
        "更新列表"};
            this.customButtonRefreshList.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 16)};
            this.customButtonRefreshList.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonRefreshList.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(72, 23)};
            this.customButtonRefreshList.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonRefreshList.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonRefreshList.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonRefreshList.CurrentTextGroupIndex = 0;
            this.customButtonRefreshList.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonRefreshList.CustomButtonData = null;
            this.customButtonRefreshList.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonRefreshList.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonRefreshList.DrawIcon = true;
            this.customButtonRefreshList.DrawText = true;
            this.customButtonRefreshList.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonRefreshList.English_TextDisplay = new string[] {
        "REFRESH&LIST"};
            this.customButtonRefreshList.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 7),
        new System.Drawing.Point(5, 27)};
            this.customButtonRefreshList.English_TextNumberInTextGroup = new int[] {
        2};
            this.customButtonRefreshList.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(80, 23),
        new System.Drawing.Size(41, 23)};
            this.customButtonRefreshList.FocusBackgroundDisplay = false;
            this.customButtonRefreshList.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonRefreshList.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonRefreshList.ForeColor = System.Drawing.Color.White;
            this.customButtonRefreshList.HoverBackgroundDisplay = false;
            this.customButtonRefreshList.IconIndex = new int[] {
        1,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonRefreshList.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(88, 11),
        new System.Drawing.Point(88, 11)};
            this.customButtonRefreshList.IconNumber = 2;
            this.customButtonRefreshList.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(53, 37),
        new System.Drawing.Size(53, 37)};
            this.customButtonRefreshList.LabelControlMode = false;
            this.customButtonRefreshList.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonRefreshList.Location = new System.Drawing.Point(868, 83);
            this.customButtonRefreshList.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonRefreshList.Name = "customButtonRefreshList";
            this.customButtonRefreshList.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonRefreshList.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonRefreshList.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonRefreshList.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonRefreshList.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonRefreshList.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonRefreshList.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonRefreshList.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonRefreshList.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonRefreshList.Size = new System.Drawing.Size(144, 56);
            this.customButtonRefreshList.SizeButton = new System.Drawing.Size(144, 56);
            this.customButtonRefreshList.TabIndex = 49;
            this.customButtonRefreshList.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonRefreshList.TextGroupNumber = 1;
            this.customButtonRefreshList.UpdateControl = true;
            this.customButtonRefreshList.Visible = false;
            this.customButtonRefreshList.CustomButton_Click += new System.EventHandler(this.customButtonRefreshList_CustomButton_Click);
            // 
            // customButtonMessage3
            // 
            this.customButtonMessage3.BackColor = System.Drawing.Color.Black;
            this.customButtonMessage3.BackgroundColor = System.Drawing.Color.Black;
            this.customButtonMessage3.BitmapIconWhole = null;
            this.customButtonMessage3.BitmapWhole = null;
            this.customButtonMessage3.Chinese_TextDisplay = new string[] {
        "正在配置设备...&正在复位设备...& "};
            this.customButtonMessage3.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 3),
        new System.Drawing.Point(0, 3),
        new System.Drawing.Point(0, 3)};
            this.customButtonMessage3.Chinese_TextNumberInTextGroup = new int[] {
        1,
        1,
        1};
            this.customButtonMessage3.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(119, 23),
        new System.Drawing.Size(119, 23),
        new System.Drawing.Size(6, 23)};
            this.customButtonMessage3.ColorTextDisable = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
            this.customButtonMessage3.ColorTextEnable = System.Drawing.Color.Yellow;
            this.customButtonMessage3.ColorTextSelected = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonMessage3.CurrentTextGroupIndex = 0;
            this.customButtonMessage3.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonMessage3.CustomButtonData = null;
            this.customButtonMessage3.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonMessage3.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonMessage3.DrawIcon = false;
            this.customButtonMessage3.DrawText = true;
            this.customButtonMessage3.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonMessage3.English_TextDisplay = new string[] {
        "Configuring Device...&Resetting Device...& "};
            this.customButtonMessage3.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 3),
        new System.Drawing.Point(0, 3),
        new System.Drawing.Point(0, 3)};
            this.customButtonMessage3.English_TextNumberInTextGroup = new int[] {
        1,
        1,
        1};
            this.customButtonMessage3.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(179, 23),
        new System.Drawing.Size(159, 23),
        new System.Drawing.Size(6, 23)};
            this.customButtonMessage3.FocusBackgroundDisplay = false;
            this.customButtonMessage3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonMessage3.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonMessage3.ForeColor = System.Drawing.Color.White;
            this.customButtonMessage3.HoverBackgroundDisplay = false;
            this.customButtonMessage3.IconIndex = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonMessage3.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonMessage3.IconNumber = 1;
            this.customButtonMessage3.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(0, 0)};
            this.customButtonMessage3.LabelControlMode = true;
            this.customButtonMessage3.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonMessage3.Location = new System.Drawing.Point(563, 113);
            this.customButtonMessage3.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonMessage3.Name = "customButtonMessage3";
            this.customButtonMessage3.RectBottom = new System.Drawing.Rectangle(3, 23, 237, 3);
            this.customButtonMessage3.RectFill = new System.Drawing.Rectangle(3, 3, 237, 20);
            this.customButtonMessage3.RectLeft = new System.Drawing.Rectangle(0, 3, 3, 20);
            this.customButtonMessage3.RectLeftBottom = new System.Drawing.Rectangle(0, 23, 3, 3);
            this.customButtonMessage3.RectLeftTop = new System.Drawing.Rectangle(0, 0, 3, 3);
            this.customButtonMessage3.RectRight = new System.Drawing.Rectangle(240, 3, 3, 20);
            this.customButtonMessage3.RectRightBottom = new System.Drawing.Rectangle(240, 23, 3, 3);
            this.customButtonMessage3.RectRightTop = new System.Drawing.Rectangle(240, 0, 3, 3);
            this.customButtonMessage3.RectTop = new System.Drawing.Rectangle(3, 0, 237, 3);
            this.customButtonMessage3.Size = new System.Drawing.Size(297, 30);
            this.customButtonMessage3.SizeButton = new System.Drawing.Size(297, 30);
            this.customButtonMessage3.TabIndex = 48;
            this.customButtonMessage3.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonMessage3.TextGroupNumber = 3;
            this.customButtonMessage3.UpdateControl = true;
            // 
            // customButtonMessage2
            // 
            this.customButtonMessage2.BackColor = System.Drawing.Color.Black;
            this.customButtonMessage2.BackgroundColor = System.Drawing.Color.Black;
            this.customButtonMessage2.BitmapIconWhole = null;
            this.customButtonMessage2.BitmapWhole = null;
            this.customButtonMessage2.Chinese_TextDisplay = new string[] {
        "固件版本："};
            this.customButtonMessage2.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 3)};
            this.customButtonMessage2.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonMessage2.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(88, 24)};
            this.customButtonMessage2.ColorTextDisable = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
            this.customButtonMessage2.ColorTextEnable = System.Drawing.Color.Yellow;
            this.customButtonMessage2.ColorTextSelected = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonMessage2.CurrentTextGroupIndex = 0;
            this.customButtonMessage2.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonMessage2.CustomButtonData = null;
            this.customButtonMessage2.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Automatic;
            this.customButtonMessage2.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonMessage2.DrawIcon = false;
            this.customButtonMessage2.DrawText = false;
            this.customButtonMessage2.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonMessage2.English_TextDisplay = new string[] {
        "Required firmware："};
            this.customButtonMessage2.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 3)};
            this.customButtonMessage2.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonMessage2.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(177, 24)};
            this.customButtonMessage2.FocusBackgroundDisplay = false;
            this.customButtonMessage2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonMessage2.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonMessage2.ForeColor = System.Drawing.Color.White;
            this.customButtonMessage2.HoverBackgroundDisplay = false;
            this.customButtonMessage2.IconIndex = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonMessage2.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonMessage2.IconNumber = 1;
            this.customButtonMessage2.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(0, 0)};
            this.customButtonMessage2.LabelControlMode = true;
            this.customButtonMessage2.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonMessage2.Location = new System.Drawing.Point(563, 83);
            this.customButtonMessage2.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonMessage2.Name = "customButtonMessage2";
            this.customButtonMessage2.RectBottom = new System.Drawing.Rectangle(3, 23, 237, 3);
            this.customButtonMessage2.RectFill = new System.Drawing.Rectangle(3, 3, 237, 20);
            this.customButtonMessage2.RectLeft = new System.Drawing.Rectangle(0, 3, 3, 20);
            this.customButtonMessage2.RectLeftBottom = new System.Drawing.Rectangle(0, 23, 3, 3);
            this.customButtonMessage2.RectLeftTop = new System.Drawing.Rectangle(0, 0, 3, 3);
            this.customButtonMessage2.RectRight = new System.Drawing.Rectangle(240, 3, 3, 20);
            this.customButtonMessage2.RectRightBottom = new System.Drawing.Rectangle(240, 23, 3, 3);
            this.customButtonMessage2.RectRightTop = new System.Drawing.Rectangle(240, 0, 3, 3);
            this.customButtonMessage2.RectTop = new System.Drawing.Rectangle(3, 0, 237, 3);
            this.customButtonMessage2.Size = new System.Drawing.Size(297, 30);
            this.customButtonMessage2.SizeButton = new System.Drawing.Size(297, 30);
            this.customButtonMessage2.TabIndex = 47;
            this.customButtonMessage2.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonMessage2.TextGroupNumber = 1;
            this.customButtonMessage2.UpdateControl = true;
            // 
            // customButtonMessage1
            // 
            this.customButtonMessage1.BackColor = System.Drawing.Color.Black;
            this.customButtonMessage1.BackgroundColor = System.Drawing.Color.Black;
            this.customButtonMessage1.BitmapIconWhole = null;
            this.customButtonMessage1.BitmapWhole = null;
            this.customButtonMessage1.Chinese_TextDisplay = new string[] {
        "选择一个设备&选择的设备& "};
            this.customButtonMessage1.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 3),
        new System.Drawing.Point(0, 3),
        new System.Drawing.Point(0, 3)};
            this.customButtonMessage1.Chinese_TextNumberInTextGroup = new int[] {
        1,
        1,
        1};
            this.customButtonMessage1.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(105, 23),
        new System.Drawing.Size(88, 23),
        new System.Drawing.Size(6, 23)};
            this.customButtonMessage1.ColorTextDisable = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
            this.customButtonMessage1.ColorTextEnable = System.Drawing.Color.Yellow;
            this.customButtonMessage1.ColorTextSelected = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonMessage1.CurrentTextGroupIndex = 0;
            this.customButtonMessage1.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonMessage1.CustomButtonData = null;
            this.customButtonMessage1.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonMessage1.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonMessage1.DrawIcon = false;
            this.customButtonMessage1.DrawText = true;
            this.customButtonMessage1.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonMessage1.English_TextDisplay = new string[] {
        "Select a device&Selected device& "};
            this.customButtonMessage1.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 3),
        new System.Drawing.Point(0, 3),
        new System.Drawing.Point(0, 3)};
            this.customButtonMessage1.English_TextNumberInTextGroup = new int[] {
        1,
        1,
        1};
            this.customButtonMessage1.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(128, 23),
        new System.Drawing.Size(134, 23),
        new System.Drawing.Size(6, 23)};
            this.customButtonMessage1.FocusBackgroundDisplay = false;
            this.customButtonMessage1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonMessage1.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonMessage1.ForeColor = System.Drawing.Color.White;
            this.customButtonMessage1.HoverBackgroundDisplay = false;
            this.customButtonMessage1.IconIndex = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonMessage1.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonMessage1.IconNumber = 1;
            this.customButtonMessage1.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(0, 0)};
            this.customButtonMessage1.LabelControlMode = true;
            this.customButtonMessage1.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonMessage1.Location = new System.Drawing.Point(14, 83);
            this.customButtonMessage1.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonMessage1.Name = "customButtonMessage1";
            this.customButtonMessage1.RectBottom = new System.Drawing.Rectangle(3, 23, 237, 3);
            this.customButtonMessage1.RectFill = new System.Drawing.Rectangle(3, 3, 237, 20);
            this.customButtonMessage1.RectLeft = new System.Drawing.Rectangle(0, 3, 3, 20);
            this.customButtonMessage1.RectLeftBottom = new System.Drawing.Rectangle(0, 23, 3, 3);
            this.customButtonMessage1.RectLeftTop = new System.Drawing.Rectangle(0, 0, 3, 3);
            this.customButtonMessage1.RectRight = new System.Drawing.Rectangle(240, 3, 3, 20);
            this.customButtonMessage1.RectRightBottom = new System.Drawing.Rectangle(240, 23, 3, 3);
            this.customButtonMessage1.RectRightTop = new System.Drawing.Rectangle(240, 0, 3, 3);
            this.customButtonMessage1.RectTop = new System.Drawing.Rectangle(3, 0, 237, 3);
            this.customButtonMessage1.Size = new System.Drawing.Size(549, 30);
            this.customButtonMessage1.SizeButton = new System.Drawing.Size(549, 30);
            this.customButtonMessage1.TabIndex = 46;
            this.customButtonMessage1.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonMessage1.TextGroupNumber = 3;
            this.customButtonMessage1.UpdateControl = true;
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
            this.customButtonClose.TabIndex = 45;
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
        "设备管理"};
            this.customButtonCaption.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(343, 7)};
            this.customButtonCaption.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonCaption.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(90, 28)};
            this.customButtonCaption.ColorTextDisable = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
            this.customButtonCaption.ColorTextEnable = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonCaption.ColorTextSelected = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonCaption.CurrentTextGroupIndex = 0;
            this.customButtonCaption.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonCaption.CustomButtonData = null;
            this.customButtonCaption.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonCaption.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonCaption.DrawIcon = false;
            this.customButtonCaption.DrawText = true;
            this.customButtonCaption.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonCaption.English_TextDisplay = new string[] {
        "VISION SYSTEM DEVICE CONFIGURATION"};
            this.customButtonCaption.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(166, 7)};
            this.customButtonCaption.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonCaption.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(443, 28)};
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
            this.customButtonCaption.TabIndex = 44;
            this.customButtonCaption.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.customButtonCaption.TextGroupNumber = 1;
            this.customButtonCaption.UpdateControl = true;
            // 
            // customList
            // 
            this.customList.BackColor = System.Drawing.Color.Black;
            this.customList.BitmapBackgroundIndex = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customList.BitmapBackgroundNumber = 1;
            this.customList.BitmapBackgroundWhole = ((System.Drawing.Bitmap)(resources.GetObject("customList.BitmapBackgroundWhole")));
            this.customList.BitmapIcon = new System.Drawing.Bitmap[] {
        null,
        null,
        null,
        null,
        null,
        null};
            this.customList.Chinese_ColumnNameDisplay = new string[] {
        "序列号",
        "MAC地址",
        "IP地址",
        "端口",
        "版本",
        "相机",
        "控制器"};
            this.customList.ColorControlBackground = System.Drawing.Color.Black;
            this.customList.ColorItemBackgroundSelected = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.customList.ColorItemBackgroundUnselected = System.Drawing.Color.Black;
            this.customList.ColorItemTextSelected = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customList.ColorItemTextUnselected = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customList.ColorListHeaderBackground = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.customList.ColorListHeaderColumnText_Disable = new System.Drawing.Color[] {
        System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))))};
            this.customList.ColorListHeaderColumnText_Enable = new System.Drawing.Color[] {
        System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))))};
            this.customList.ColorListItemBackground = System.Drawing.Color.Black;
            this.customList.ColorPageBackground = System.Drawing.Color.Black;
            this.customList.ColorPageText = System.Drawing.Color.White;
            this.customList.ColumnNameXOffSetValue = 7;
            this.customList.ColumnNumber = 7;
            this.customList.ColumnWidth = new int[] {
        140,
        150,
        130,
        70,
        90,
        130,
        125};
            this.customList.CurrentColumnNameGroupIndex = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customList.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customList.English_ColumnNameDisplay = new string[] {
        "Serial Port",
        "MAC Address",
        "IP Address",
        "Port",
        "Version",
        "Camera",
        "Controller"};
            this.customList.FontListHeader = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customList.FontListItem = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customList.FontPage = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customList.ForeColor = System.Drawing.Color.White;
            this.customList.ItemDataDisplay = new bool[] {
        true,
        true,
        true,
        true,
        true,
        true,
        true};
            this.customList.ItemDataNumber = 0;
            this.customList.ItemIconIndex = new int[] {
        -1,
        -1,
        -1,
        -1,
        -1,
        -1,
        -1};
            this.customList.ItemIconNumber = 6;
            this.customList.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customList.ListEnabled = true;
            this.customList.ListHeaderHeight = 36;
            this.customList.ListHeaderTextAlignment = System.Drawing.StringAlignment.Near;
            this.customList.ListItemHeight = 35;
            this.customList.ListItemTextAlignment = System.Drawing.StringAlignment.Near;
            this.customList.ListItemXOffSetValue = 7;
            this.customList.Location = new System.Drawing.Point(14, 159);
            this.customList.Name = "customList";
            this.customList.PageHeight = 25;
            this.customList.RectBottom = new System.Drawing.Rectangle(3, 23, 237, 3);
            this.customList.RectFill = new System.Drawing.Rectangle(3, 3, 237, 20);
            this.customList.RectLeft = new System.Drawing.Rectangle(0, 3, 3, 20);
            this.customList.RectLeftBottom = new System.Drawing.Rectangle(0, 23, 3, 3);
            this.customList.RectLeftTop = new System.Drawing.Rectangle(0, 0, 3, 3);
            this.customList.RectRight = new System.Drawing.Rectangle(240, 3, 3, 20);
            this.customList.RectRightBottom = new System.Drawing.Rectangle(240, 23, 3, 3);
            this.customList.RectRightTop = new System.Drawing.Rectangle(240, 0, 3, 3);
            this.customList.RectTop = new System.Drawing.Rectangle(3, 0, 237, 3);
            this.customList.SelectedItemNumber = 0;
            this.customList.SelectedItemType = false;
            this.customList.SelectionColumnIndex = -1;
            this.customList.Size = new System.Drawing.Size(846, 484);
            this.customList.SizeControl = new System.Drawing.Size(846, 484);
            this.customList.SizeItemIcon = new System.Drawing.Size[] {
        new System.Drawing.Size(20, 20),
        new System.Drawing.Size(20, 20),
        new System.Drawing.Size(20, 20),
        new System.Drawing.Size(20, 20),
        new System.Drawing.Size(20, 20),
        new System.Drawing.Size(20, 20),
        new System.Drawing.Size(20, 20)};
            this.customList.TabIndex = 43;
            this.customList.CustomListItem_Click += new System.EventHandler(this.customListItem_Click);
            // 
            // customButtonNextPage
            // 
            this.customButtonNextPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonNextPage.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonNextPage.BitmapIconWhole = global::VisionSystemControlLibrary.Properties.Resources.Down;
            this.customButtonNextPage.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.Button;
            this.customButtonNextPage.Chinese_TextDisplay = new string[] {
        "下一页"};
            this.customButtonNextPage.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 5)};
            this.customButtonNextPage.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonNextPage.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(55, 23)};
            this.customButtonNextPage.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonNextPage.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonNextPage.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonNextPage.CurrentTextGroupIndex = 0;
            this.customButtonNextPage.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonNextPage.CustomButtonData = null;
            this.customButtonNextPage.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonNextPage.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonNextPage.DrawIcon = true;
            this.customButtonNextPage.DrawText = false;
            this.customButtonNextPage.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonNextPage.English_TextDisplay = new string[] {
        "NEXT&PAGE"};
            this.customButtonNextPage.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 5),
        new System.Drawing.Point(5, 25)};
            this.customButtonNextPage.English_TextNumberInTextGroup = new int[] {
        2};
            this.customButtonNextPage.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(51, 23),
        new System.Drawing.Size(51, 23)};
            this.customButtonNextPage.FocusBackgroundDisplay = false;
            this.customButtonNextPage.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonNextPage.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonNextPage.ForeColor = System.Drawing.Color.White;
            this.customButtonNextPage.HoverBackgroundDisplay = false;
            this.customButtonNextPage.IconIndex = new int[] {
        1,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonNextPage.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(8, 12),
        new System.Drawing.Point(8, 12)};
            this.customButtonNextPage.IconNumber = 2;
            this.customButtonNextPage.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(48, 70),
        new System.Drawing.Size(48, 70)};
            this.customButtonNextPage.LabelControlMode = false;
            this.customButtonNextPage.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonNextPage.Location = new System.Drawing.Point(942, 548);
            this.customButtonNextPage.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonNextPage.Name = "customButtonNextPage";
            this.customButtonNextPage.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonNextPage.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonNextPage.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonNextPage.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonNextPage.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonNextPage.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonNextPage.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonNextPage.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonNextPage.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonNextPage.Size = new System.Drawing.Size(63, 97);
            this.customButtonNextPage.SizeButton = new System.Drawing.Size(63, 97);
            this.customButtonNextPage.TabIndex = 56;
            this.customButtonNextPage.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonNextPage.TextGroupNumber = 1;
            this.customButtonNextPage.UpdateControl = true;
            this.customButtonNextPage.Visible = false;
            this.customButtonNextPage.CustomButton_Click += new System.EventHandler(this.customButtonNextPage_CustomButton_Click);
            // 
            // customButtonPreviousPage
            // 
            this.customButtonPreviousPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonPreviousPage.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonPreviousPage.BitmapIconWhole = global::VisionSystemControlLibrary.Properties.Resources.Up;
            this.customButtonPreviousPage.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.Button;
            this.customButtonPreviousPage.Chinese_TextDisplay = new string[] {
        "上一页"};
            this.customButtonPreviousPage.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 5)};
            this.customButtonPreviousPage.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonPreviousPage.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(55, 23)};
            this.customButtonPreviousPage.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonPreviousPage.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonPreviousPage.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonPreviousPage.CurrentTextGroupIndex = 0;
            this.customButtonPreviousPage.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonPreviousPage.CustomButtonData = null;
            this.customButtonPreviousPage.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonPreviousPage.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonPreviousPage.DrawIcon = true;
            this.customButtonPreviousPage.DrawText = false;
            this.customButtonPreviousPage.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonPreviousPage.English_TextDisplay = new string[] {
        "PREV.&PAGE"};
            this.customButtonPreviousPage.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 5),
        new System.Drawing.Point(5, 25)};
            this.customButtonPreviousPage.English_TextNumberInTextGroup = new int[] {
        2};
            this.customButtonPreviousPage.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(54, 23),
        new System.Drawing.Size(51, 23)};
            this.customButtonPreviousPage.FocusBackgroundDisplay = false;
            this.customButtonPreviousPage.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonPreviousPage.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonPreviousPage.ForeColor = System.Drawing.Color.White;
            this.customButtonPreviousPage.HoverBackgroundDisplay = false;
            this.customButtonPreviousPage.IconIndex = new int[] {
        1,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonPreviousPage.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(8, 12),
        new System.Drawing.Point(8, 12)};
            this.customButtonPreviousPage.IconNumber = 2;
            this.customButtonPreviousPage.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(48, 70),
        new System.Drawing.Size(48, 70)};
            this.customButtonPreviousPage.LabelControlMode = false;
            this.customButtonPreviousPage.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonPreviousPage.Location = new System.Drawing.Point(875, 548);
            this.customButtonPreviousPage.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonPreviousPage.Name = "customButtonPreviousPage";
            this.customButtonPreviousPage.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonPreviousPage.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonPreviousPage.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonPreviousPage.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonPreviousPage.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonPreviousPage.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonPreviousPage.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonPreviousPage.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonPreviousPage.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonPreviousPage.Size = new System.Drawing.Size(63, 97);
            this.customButtonPreviousPage.SizeButton = new System.Drawing.Size(63, 97);
            this.customButtonPreviousPage.TabIndex = 55;
            this.customButtonPreviousPage.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonPreviousPage.TextGroupNumber = 1;
            this.customButtonPreviousPage.UpdateControl = true;
            this.customButtonPreviousPage.Visible = false;
            this.customButtonPreviousPage.CustomButton_Click += new System.EventHandler(this.customButtonPreviousPage_CustomButton_Click);
            // 
            // timerConfigDevice
            // 
            this.timerConfigDevice.Interval = 1000;
            this.timerConfigDevice.Tick += new System.EventHandler(this.timerConfigDevice_Tick);
            // 
            // labelIcon
            // 
            this.labelIcon.BackColor = System.Drawing.Color.Transparent;
            this.labelIcon.Image = global::VisionSystemControlLibrary.Properties.Resources.Device;
            this.labelIcon.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelIcon.Location = new System.Drawing.Point(28, 17);
            this.labelIcon.Name = "labelIcon";
            this.labelIcon.Size = new System.Drawing.Size(53, 37);
            this.labelIcon.TabIndex = 58;
            // 
            // customButtonParameterSettings
            // 
            this.customButtonParameterSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.customButtonParameterSettings.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.customButtonParameterSettings.BitmapIconWhole = global::VisionSystemControlLibrary.Properties.Resources.Port;
            this.customButtonParameterSettings.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.Button;
            this.customButtonParameterSettings.Chinese_TextDisplay = new string[] {
        "参数设置"};
            this.customButtonParameterSettings.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 16)};
            this.customButtonParameterSettings.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonParameterSettings.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(72, 23)};
            this.customButtonParameterSettings.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonParameterSettings.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonParameterSettings.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonParameterSettings.CurrentTextGroupIndex = 0;
            this.customButtonParameterSettings.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonParameterSettings.CustomButtonData = null;
            this.customButtonParameterSettings.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonParameterSettings.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonParameterSettings.DrawIcon = true;
            this.customButtonParameterSettings.DrawText = true;
            this.customButtonParameterSettings.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonParameterSettings.English_TextDisplay = new string[] {
        "PARAM&SETTINGS"};
            this.customButtonParameterSettings.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 7),
        new System.Drawing.Point(5, 27)};
            this.customButtonParameterSettings.English_TextNumberInTextGroup = new int[] {
        2};
            this.customButtonParameterSettings.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(70, 23),
        new System.Drawing.Size(88, 23)};
            this.customButtonParameterSettings.FocusBackgroundDisplay = false;
            this.customButtonParameterSettings.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonParameterSettings.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonParameterSettings.ForeColor = System.Drawing.Color.White;
            this.customButtonParameterSettings.HoverBackgroundDisplay = false;
            this.customButtonParameterSettings.IconIndex = new int[] {
        1,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonParameterSettings.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(84, 10),
        new System.Drawing.Point(84, 10)};
            this.customButtonParameterSettings.IconNumber = 2;
            this.customButtonParameterSettings.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(53, 37),
        new System.Drawing.Size(53, 37)};
            this.customButtonParameterSettings.LabelControlMode = false;
            this.customButtonParameterSettings.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonParameterSettings.Location = new System.Drawing.Point(868, 277);
            this.customButtonParameterSettings.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonParameterSettings.Name = "customButtonParameterSettings";
            this.customButtonParameterSettings.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonParameterSettings.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonParameterSettings.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonParameterSettings.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonParameterSettings.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonParameterSettings.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonParameterSettings.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonParameterSettings.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonParameterSettings.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonParameterSettings.Size = new System.Drawing.Size(144, 56);
            this.customButtonParameterSettings.SizeButton = new System.Drawing.Size(144, 56);
            this.customButtonParameterSettings.TabIndex = 105;
            this.customButtonParameterSettings.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonParameterSettings.TextGroupNumber = 1;
            this.customButtonParameterSettings.UpdateControl = true;
            this.customButtonParameterSettings.CustomButton_Click += new System.EventHandler(this.customButtonParameterSettings_CustomButton_Click);
            // 
            // DeviceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.Controls.Add(this.customButtonParameterSettings);
            this.Controls.Add(this.labelIcon);
            this.Controls.Add(this.customButtonNextPage);
            this.Controls.Add(this.customButtonPreviousPage);
            this.Controls.Add(this.customButtonAlignDateTime);
            this.Controls.Add(this.customButtonConfigImage);
            this.Controls.Add(this.customButtonTestIO);
            this.Controls.Add(this.customButtonConfigDevice);
            this.Controls.Add(this.customButtonResetDevice);
            this.Controls.Add(this.customButtonRefreshList);
            this.Controls.Add(this.customButtonMessage3);
            this.Controls.Add(this.customButtonMessage2);
            this.Controls.Add(this.customButtonMessage1);
            this.Controls.Add(this.customButtonClose);
            this.Controls.Add(this.customButtonCaption);
            this.Controls.Add(this.customList);
            this.Controls.Add(this.labelMessage1);
            this.Name = "DeviceControl";
            this.Size = new System.Drawing.Size(1024, 662);
            this.Load += new System.EventHandler(this.DeviceControl_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelMessage1;
        private CustomList customList;
        private CustomButton customButtonCaption;
        private CustomButton customButtonClose;
        private CustomButton customButtonMessage1;
        private CustomButton customButtonMessage2;
        private CustomButton customButtonMessage3;
        private CustomButton customButtonRefreshList;
        private CustomButton customButtonResetDevice;
        private CustomButton customButtonConfigDevice;
        private CustomButton customButtonTestIO;
        private CustomButton customButtonConfigImage;
        private CustomButton customButtonAlignDateTime;
        private CustomButton customButtonNextPage;
        private CustomButton customButtonPreviousPage;
        private System.Windows.Forms.Timer timerConfigDevice;
        private System.Windows.Forms.Label labelIcon;
        private CustomButton customButtonParameterSettings;
    }
}