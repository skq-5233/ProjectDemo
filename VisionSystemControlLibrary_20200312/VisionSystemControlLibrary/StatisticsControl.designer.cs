namespace VisionSystemControlLibrary
{
    partial class StatisticsControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StatisticsControl));
            this.timerStatistics = new System.Windows.Forms.Timer(this.components);
            this.labelIcon = new System.Windows.Forms.Label();
            this.customButtonShiftValue_2 = new VisionSystemControlLibrary.CustomButton();
            this.customButtonDurationTimeText = new VisionSystemControlLibrary.CustomButton();
            this.customButtonBrandText = new VisionSystemControlLibrary.CustomButton();
            this.customButtonRejectedValue = new VisionSystemControlLibrary.CustomButton();
            this.customButtonRejectedText = new VisionSystemControlLibrary.CustomButton();
            this.customButtonInspectedValue = new VisionSystemControlLibrary.CustomButton();
            this.customButtonInspectedText = new VisionSystemControlLibrary.CustomButton();
            this.customButtonBrandValue = new VisionSystemControlLibrary.CustomButton();
            this.customButtonDurationTimeValue = new VisionSystemControlLibrary.CustomButton();
            this.customButtonShiftTimeValue = new VisionSystemControlLibrary.CustomButton();
            this.customButtonShiftText = new VisionSystemControlLibrary.CustomButton();
            this.customButtonShiftValue_1 = new VisionSystemControlLibrary.CustomButton();
            this.customButtonSelectAll = new VisionSystemControlLibrary.CustomButton();
            this.parameterSettingsPanel = new VisionSystemControlLibrary.ParameterSettingsPanel();
            this.customButtonPlus = new VisionSystemControlLibrary.CustomButton();
            this.customButtonSubtract = new VisionSystemControlLibrary.CustomButton();
            this.customButtonStatusBar = new VisionSystemControlLibrary.CustomButton();
            this.imageDisplayView = new VisionSystemControlLibrary.ImageDisplay();
            this.customButtonViewBackground = new VisionSystemControlLibrary.CustomButton();
            this.customButtonNextPage_List_1 = new VisionSystemControlLibrary.CustomButton();
            this.customButtonPreviousPage_List_1 = new VisionSystemControlLibrary.CustomButton();
            this.customButtonParameter = new VisionSystemControlLibrary.CustomButton();
            this.customButtonViewReject = new VisionSystemControlLibrary.CustomButton();
            this.customButtonSelection = new VisionSystemControlLibrary.CustomButton();
            this.customListRejects = new VisionSystemControlLibrary.CustomList();
            this.customButtonRecords = new VisionSystemControlLibrary.CustomButton();
            this.customButtonClose = new VisionSystemControlLibrary.CustomButton();
            this.customButtonCaption = new VisionSystemControlLibrary.CustomButton();
            this.SuspendLayout();
            // 
            // timerStatistics
            // 
            this.timerStatistics.Interval = 1000;
            this.timerStatistics.Tick += new System.EventHandler(this.timerStatistics_Tick);
            // 
            // labelIcon
            // 
            this.labelIcon.BackColor = System.Drawing.Color.Transparent;
            this.labelIcon.Image = global::VisionSystemControlLibrary.Properties.Resources.RenameBrand;
            this.labelIcon.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelIcon.Location = new System.Drawing.Point(37, 18);
            this.labelIcon.Name = "labelIcon";
            this.labelIcon.Size = new System.Drawing.Size(53, 37);
            this.labelIcon.TabIndex = 177;
            // 
            // customButtonShiftValue_2
            // 
            this.customButtonShiftValue_2.BackColor = System.Drawing.Color.Black;
            this.customButtonShiftValue_2.BackgroundColor = System.Drawing.Color.Black;
            this.customButtonShiftValue_2.BitmapIconWhole = null;
            this.customButtonShiftValue_2.BitmapWhole = null;
            this.customButtonShiftValue_2.Chinese_TextDisplay = new string[] {
        "（当前）：&（历史）："};
            this.customButtonShiftValue_2.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 3),
        new System.Drawing.Point(0, 3)};
            this.customButtonShiftValue_2.Chinese_TextNumberInTextGroup = new int[] {
        1,
        1};
            this.customButtonShiftValue_2.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(88, 23),
        new System.Drawing.Size(88, 23)};
            this.customButtonShiftValue_2.ColorTextDisable = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
            this.customButtonShiftValue_2.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonShiftValue_2.ColorTextSelected = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonShiftValue_2.CurrentTextGroupIndex = 0;
            this.customButtonShiftValue_2.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonShiftValue_2.CustomButtonData = null;
            this.customButtonShiftValue_2.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonShiftValue_2.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonShiftValue_2.DrawIcon = false;
            this.customButtonShiftValue_2.DrawText = true;
            this.customButtonShiftValue_2.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonShiftValue_2.English_TextDisplay = new string[] {
        "（Cur.）：&（Hist）："};
            this.customButtonShiftValue_2.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 3),
        new System.Drawing.Point(0, 3)};
            this.customButtonShiftValue_2.English_TextNumberInTextGroup = new int[] {
        1,
        1};
            this.customButtonShiftValue_2.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(89, 23),
        new System.Drawing.Size(89, 23)};
            this.customButtonShiftValue_2.FocusBackgroundDisplay = false;
            this.customButtonShiftValue_2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonShiftValue_2.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonShiftValue_2.ForeColor = System.Drawing.Color.White;
            this.customButtonShiftValue_2.HoverBackgroundDisplay = false;
            this.customButtonShiftValue_2.IconIndex = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonShiftValue_2.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonShiftValue_2.IconNumber = 1;
            this.customButtonShiftValue_2.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(0, 0)};
            this.customButtonShiftValue_2.LabelControlMode = true;
            this.customButtonShiftValue_2.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonShiftValue_2.Location = new System.Drawing.Point(86, 78);
            this.customButtonShiftValue_2.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonShiftValue_2.Name = "customButtonShiftValue_2";
            this.customButtonShiftValue_2.RectBottom = new System.Drawing.Rectangle(3, 23, 237, 3);
            this.customButtonShiftValue_2.RectFill = new System.Drawing.Rectangle(3, 3, 237, 20);
            this.customButtonShiftValue_2.RectLeft = new System.Drawing.Rectangle(0, 3, 3, 20);
            this.customButtonShiftValue_2.RectLeftBottom = new System.Drawing.Rectangle(0, 23, 3, 3);
            this.customButtonShiftValue_2.RectLeftTop = new System.Drawing.Rectangle(0, 0, 3, 3);
            this.customButtonShiftValue_2.RectRight = new System.Drawing.Rectangle(240, 3, 3, 20);
            this.customButtonShiftValue_2.RectRightBottom = new System.Drawing.Rectangle(240, 23, 3, 3);
            this.customButtonShiftValue_2.RectRightTop = new System.Drawing.Rectangle(240, 0, 3, 3);
            this.customButtonShiftValue_2.RectTop = new System.Drawing.Rectangle(3, 0, 237, 3);
            this.customButtonShiftValue_2.Size = new System.Drawing.Size(90, 30);
            this.customButtonShiftValue_2.SizeButton = new System.Drawing.Size(90, 30);
            this.customButtonShiftValue_2.TabIndex = 176;
            this.customButtonShiftValue_2.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonShiftValue_2.TextGroupNumber = 2;
            this.customButtonShiftValue_2.UpdateControl = true;
            // 
            // customButtonDurationTimeText
            // 
            this.customButtonDurationTimeText.BackColor = System.Drawing.Color.Black;
            this.customButtonDurationTimeText.BackgroundColor = System.Drawing.Color.Black;
            this.customButtonDurationTimeText.BitmapIconWhole = null;
            this.customButtonDurationTimeText.BitmapWhole = null;
            this.customButtonDurationTimeText.Chinese_TextDisplay = new string[] {
        "剔除率："};
            this.customButtonDurationTimeText.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 3)};
            this.customButtonDurationTimeText.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonDurationTimeText.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(72, 23)};
            this.customButtonDurationTimeText.ColorTextDisable = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
            this.customButtonDurationTimeText.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonDurationTimeText.ColorTextSelected = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonDurationTimeText.CurrentTextGroupIndex = 0;
            this.customButtonDurationTimeText.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonDurationTimeText.CustomButtonData = null;
            this.customButtonDurationTimeText.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonDurationTimeText.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonDurationTimeText.DrawIcon = false;
            this.customButtonDurationTimeText.DrawText = true;
            this.customButtonDurationTimeText.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonDurationTimeText.English_TextDisplay = new string[] {
        "Rejected Rate："};
            this.customButtonDurationTimeText.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 3)};
            this.customButtonDurationTimeText.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonDurationTimeText.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(136, 23)};
            this.customButtonDurationTimeText.FocusBackgroundDisplay = false;
            this.customButtonDurationTimeText.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonDurationTimeText.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonDurationTimeText.ForeColor = System.Drawing.Color.White;
            this.customButtonDurationTimeText.HoverBackgroundDisplay = false;
            this.customButtonDurationTimeText.IconIndex = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonDurationTimeText.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonDurationTimeText.IconNumber = 1;
            this.customButtonDurationTimeText.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(0, 0)};
            this.customButtonDurationTimeText.LabelControlMode = true;
            this.customButtonDurationTimeText.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonDurationTimeText.Location = new System.Drawing.Point(530, 108);
            this.customButtonDurationTimeText.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonDurationTimeText.Name = "customButtonDurationTimeText";
            this.customButtonDurationTimeText.RectBottom = new System.Drawing.Rectangle(3, 23, 237, 3);
            this.customButtonDurationTimeText.RectFill = new System.Drawing.Rectangle(3, 3, 237, 20);
            this.customButtonDurationTimeText.RectLeft = new System.Drawing.Rectangle(0, 3, 3, 20);
            this.customButtonDurationTimeText.RectLeftBottom = new System.Drawing.Rectangle(0, 23, 3, 3);
            this.customButtonDurationTimeText.RectLeftTop = new System.Drawing.Rectangle(0, 0, 3, 3);
            this.customButtonDurationTimeText.RectRight = new System.Drawing.Rectangle(240, 3, 3, 20);
            this.customButtonDurationTimeText.RectRightBottom = new System.Drawing.Rectangle(240, 23, 3, 3);
            this.customButtonDurationTimeText.RectRightTop = new System.Drawing.Rectangle(240, 0, 3, 3);
            this.customButtonDurationTimeText.RectTop = new System.Drawing.Rectangle(3, 0, 237, 3);
            this.customButtonDurationTimeText.Size = new System.Drawing.Size(136, 30);
            this.customButtonDurationTimeText.SizeButton = new System.Drawing.Size(136, 30);
            this.customButtonDurationTimeText.TabIndex = 175;
            this.customButtonDurationTimeText.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonDurationTimeText.TextGroupNumber = 1;
            this.customButtonDurationTimeText.UpdateControl = true;
            // 
            // customButtonBrandText
            // 
            this.customButtonBrandText.BackColor = System.Drawing.Color.Black;
            this.customButtonBrandText.BackgroundColor = System.Drawing.Color.Black;
            this.customButtonBrandText.BitmapIconWhole = null;
            this.customButtonBrandText.BitmapWhole = null;
            this.customButtonBrandText.Chinese_TextDisplay = new string[] {
        "品牌："};
            this.customButtonBrandText.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 3)};
            this.customButtonBrandText.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonBrandText.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(55, 23)};
            this.customButtonBrandText.ColorTextDisable = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
            this.customButtonBrandText.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonBrandText.ColorTextSelected = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonBrandText.CurrentTextGroupIndex = 0;
            this.customButtonBrandText.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonBrandText.CustomButtonData = null;
            this.customButtonBrandText.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonBrandText.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonBrandText.DrawIcon = false;
            this.customButtonBrandText.DrawText = true;
            this.customButtonBrandText.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonBrandText.English_TextDisplay = new string[] {
        "Brand："};
            this.customButtonBrandText.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 3)};
            this.customButtonBrandText.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonBrandText.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(72, 23)};
            this.customButtonBrandText.FocusBackgroundDisplay = false;
            this.customButtonBrandText.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonBrandText.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonBrandText.ForeColor = System.Drawing.Color.White;
            this.customButtonBrandText.HoverBackgroundDisplay = false;
            this.customButtonBrandText.IconIndex = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonBrandText.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonBrandText.IconNumber = 1;
            this.customButtonBrandText.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(0, 0)};
            this.customButtonBrandText.LabelControlMode = true;
            this.customButtonBrandText.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonBrandText.Location = new System.Drawing.Point(556, 78);
            this.customButtonBrandText.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonBrandText.Name = "customButtonBrandText";
            this.customButtonBrandText.RectBottom = new System.Drawing.Rectangle(3, 23, 237, 3);
            this.customButtonBrandText.RectFill = new System.Drawing.Rectangle(3, 3, 237, 20);
            this.customButtonBrandText.RectLeft = new System.Drawing.Rectangle(0, 3, 3, 20);
            this.customButtonBrandText.RectLeftBottom = new System.Drawing.Rectangle(0, 23, 3, 3);
            this.customButtonBrandText.RectLeftTop = new System.Drawing.Rectangle(0, 0, 3, 3);
            this.customButtonBrandText.RectRight = new System.Drawing.Rectangle(240, 3, 3, 20);
            this.customButtonBrandText.RectRightBottom = new System.Drawing.Rectangle(240, 23, 3, 3);
            this.customButtonBrandText.RectRightTop = new System.Drawing.Rectangle(240, 0, 3, 3);
            this.customButtonBrandText.RectTop = new System.Drawing.Rectangle(3, 0, 237, 3);
            this.customButtonBrandText.Size = new System.Drawing.Size(94, 30);
            this.customButtonBrandText.SizeButton = new System.Drawing.Size(94, 30);
            this.customButtonBrandText.TabIndex = 174;
            this.customButtonBrandText.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonBrandText.TextGroupNumber = 1;
            this.customButtonBrandText.UpdateControl = true;
            // 
            // customButtonRejectedValue
            // 
            this.customButtonRejectedValue.BackColor = System.Drawing.Color.Black;
            this.customButtonRejectedValue.BackgroundColor = System.Drawing.Color.Black;
            this.customButtonRejectedValue.BitmapIconWhole = null;
            this.customButtonRejectedValue.BitmapWhole = null;
            this.customButtonRejectedValue.Chinese_TextDisplay = new string[] {
        "数值"};
            this.customButtonRejectedValue.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 3)};
            this.customButtonRejectedValue.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonRejectedValue.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(39, 24)};
            this.customButtonRejectedValue.ColorTextDisable = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
            this.customButtonRejectedValue.ColorTextEnable = System.Drawing.Color.Yellow;
            this.customButtonRejectedValue.ColorTextSelected = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonRejectedValue.CurrentTextGroupIndex = 0;
            this.customButtonRejectedValue.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonRejectedValue.CustomButtonData = null;
            this.customButtonRejectedValue.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Automatic;
            this.customButtonRejectedValue.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonRejectedValue.DrawIcon = false;
            this.customButtonRejectedValue.DrawText = true;
            this.customButtonRejectedValue.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonRejectedValue.English_TextDisplay = new string[] {
        "Value"};
            this.customButtonRejectedValue.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 3)};
            this.customButtonRejectedValue.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonRejectedValue.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(52, 24)};
            this.customButtonRejectedValue.FocusBackgroundDisplay = false;
            this.customButtonRejectedValue.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonRejectedValue.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonRejectedValue.ForeColor = System.Drawing.Color.White;
            this.customButtonRejectedValue.HoverBackgroundDisplay = false;
            this.customButtonRejectedValue.IconIndex = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonRejectedValue.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonRejectedValue.IconNumber = 1;
            this.customButtonRejectedValue.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(0, 0)};
            this.customButtonRejectedValue.LabelControlMode = true;
            this.customButtonRejectedValue.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonRejectedValue.Location = new System.Drawing.Point(324, 108);
            this.customButtonRejectedValue.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonRejectedValue.Name = "customButtonRejectedValue";
            this.customButtonRejectedValue.RectBottom = new System.Drawing.Rectangle(3, 23, 237, 3);
            this.customButtonRejectedValue.RectFill = new System.Drawing.Rectangle(3, 3, 237, 20);
            this.customButtonRejectedValue.RectLeft = new System.Drawing.Rectangle(0, 3, 3, 20);
            this.customButtonRejectedValue.RectLeftBottom = new System.Drawing.Rectangle(0, 23, 3, 3);
            this.customButtonRejectedValue.RectLeftTop = new System.Drawing.Rectangle(0, 0, 3, 3);
            this.customButtonRejectedValue.RectRight = new System.Drawing.Rectangle(240, 3, 3, 20);
            this.customButtonRejectedValue.RectRightBottom = new System.Drawing.Rectangle(240, 23, 3, 3);
            this.customButtonRejectedValue.RectRightTop = new System.Drawing.Rectangle(240, 0, 3, 3);
            this.customButtonRejectedValue.RectTop = new System.Drawing.Rectangle(3, 0, 237, 3);
            this.customButtonRejectedValue.Size = new System.Drawing.Size(206, 30);
            this.customButtonRejectedValue.SizeButton = new System.Drawing.Size(206, 30);
            this.customButtonRejectedValue.TabIndex = 173;
            this.customButtonRejectedValue.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonRejectedValue.TextGroupNumber = 1;
            this.customButtonRejectedValue.UpdateControl = true;
            // 
            // customButtonRejectedText
            // 
            this.customButtonRejectedText.BackColor = System.Drawing.Color.Black;
            this.customButtonRejectedText.BackgroundColor = System.Drawing.Color.Black;
            this.customButtonRejectedText.BitmapIconWhole = null;
            this.customButtonRejectedText.BitmapWhole = null;
            this.customButtonRejectedText.Chinese_TextDisplay = new string[] {
        "剔除数量："};
            this.customButtonRejectedText.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 3)};
            this.customButtonRejectedText.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonRejectedText.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(88, 23)};
            this.customButtonRejectedText.ColorTextDisable = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
            this.customButtonRejectedText.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonRejectedText.ColorTextSelected = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonRejectedText.CurrentTextGroupIndex = 0;
            this.customButtonRejectedText.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonRejectedText.CustomButtonData = null;
            this.customButtonRejectedText.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonRejectedText.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonRejectedText.DrawIcon = false;
            this.customButtonRejectedText.DrawText = true;
            this.customButtonRejectedText.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonRejectedText.English_TextDisplay = new string[] {
        "Rejected："};
            this.customButtonRejectedText.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 3)};
            this.customButtonRejectedText.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonRejectedText.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(94, 23)};
            this.customButtonRejectedText.FocusBackgroundDisplay = false;
            this.customButtonRejectedText.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonRejectedText.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonRejectedText.ForeColor = System.Drawing.Color.White;
            this.customButtonRejectedText.HoverBackgroundDisplay = false;
            this.customButtonRejectedText.IconIndex = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonRejectedText.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonRejectedText.IconNumber = 1;
            this.customButtonRejectedText.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(0, 0)};
            this.customButtonRejectedText.LabelControlMode = true;
            this.customButtonRejectedText.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonRejectedText.Location = new System.Drawing.Point(230, 108);
            this.customButtonRejectedText.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonRejectedText.Name = "customButtonRejectedText";
            this.customButtonRejectedText.RectBottom = new System.Drawing.Rectangle(3, 23, 237, 3);
            this.customButtonRejectedText.RectFill = new System.Drawing.Rectangle(3, 3, 237, 20);
            this.customButtonRejectedText.RectLeft = new System.Drawing.Rectangle(0, 3, 3, 20);
            this.customButtonRejectedText.RectLeftBottom = new System.Drawing.Rectangle(0, 23, 3, 3);
            this.customButtonRejectedText.RectLeftTop = new System.Drawing.Rectangle(0, 0, 3, 3);
            this.customButtonRejectedText.RectRight = new System.Drawing.Rectangle(240, 3, 3, 20);
            this.customButtonRejectedText.RectRightBottom = new System.Drawing.Rectangle(240, 23, 3, 3);
            this.customButtonRejectedText.RectRightTop = new System.Drawing.Rectangle(240, 0, 3, 3);
            this.customButtonRejectedText.RectTop = new System.Drawing.Rectangle(3, 0, 237, 3);
            this.customButtonRejectedText.Size = new System.Drawing.Size(94, 30);
            this.customButtonRejectedText.SizeButton = new System.Drawing.Size(94, 30);
            this.customButtonRejectedText.TabIndex = 172;
            this.customButtonRejectedText.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonRejectedText.TextGroupNumber = 1;
            this.customButtonRejectedText.UpdateControl = true;
            // 
            // customButtonInspectedValue
            // 
            this.customButtonInspectedValue.BackColor = System.Drawing.Color.Black;
            this.customButtonInspectedValue.BackgroundColor = System.Drawing.Color.Black;
            this.customButtonInspectedValue.BitmapIconWhole = null;
            this.customButtonInspectedValue.BitmapWhole = null;
            this.customButtonInspectedValue.Chinese_TextDisplay = new string[] {
        "数值"};
            this.customButtonInspectedValue.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 3)};
            this.customButtonInspectedValue.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonInspectedValue.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(39, 24)};
            this.customButtonInspectedValue.ColorTextDisable = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
            this.customButtonInspectedValue.ColorTextEnable = System.Drawing.Color.Yellow;
            this.customButtonInspectedValue.ColorTextSelected = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonInspectedValue.CurrentTextGroupIndex = 0;
            this.customButtonInspectedValue.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonInspectedValue.CustomButtonData = null;
            this.customButtonInspectedValue.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Automatic;
            this.customButtonInspectedValue.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonInspectedValue.DrawIcon = false;
            this.customButtonInspectedValue.DrawText = true;
            this.customButtonInspectedValue.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonInspectedValue.English_TextDisplay = new string[] {
        "Value"};
            this.customButtonInspectedValue.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 3)};
            this.customButtonInspectedValue.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonInspectedValue.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(52, 24)};
            this.customButtonInspectedValue.FocusBackgroundDisplay = false;
            this.customButtonInspectedValue.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonInspectedValue.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonInspectedValue.ForeColor = System.Drawing.Color.White;
            this.customButtonInspectedValue.HoverBackgroundDisplay = false;
            this.customButtonInspectedValue.IconIndex = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonInspectedValue.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonInspectedValue.IconNumber = 1;
            this.customButtonInspectedValue.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(0, 0)};
            this.customButtonInspectedValue.LabelControlMode = true;
            this.customButtonInspectedValue.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonInspectedValue.Location = new System.Drawing.Point(109, 108);
            this.customButtonInspectedValue.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonInspectedValue.Name = "customButtonInspectedValue";
            this.customButtonInspectedValue.RectBottom = new System.Drawing.Rectangle(3, 23, 237, 3);
            this.customButtonInspectedValue.RectFill = new System.Drawing.Rectangle(3, 3, 237, 20);
            this.customButtonInspectedValue.RectLeft = new System.Drawing.Rectangle(0, 3, 3, 20);
            this.customButtonInspectedValue.RectLeftBottom = new System.Drawing.Rectangle(0, 23, 3, 3);
            this.customButtonInspectedValue.RectLeftTop = new System.Drawing.Rectangle(0, 0, 3, 3);
            this.customButtonInspectedValue.RectRight = new System.Drawing.Rectangle(240, 3, 3, 20);
            this.customButtonInspectedValue.RectRightBottom = new System.Drawing.Rectangle(240, 23, 3, 3);
            this.customButtonInspectedValue.RectRightTop = new System.Drawing.Rectangle(240, 0, 3, 3);
            this.customButtonInspectedValue.RectTop = new System.Drawing.Rectangle(3, 0, 237, 3);
            this.customButtonInspectedValue.Size = new System.Drawing.Size(121, 30);
            this.customButtonInspectedValue.SizeButton = new System.Drawing.Size(121, 30);
            this.customButtonInspectedValue.TabIndex = 171;
            this.customButtonInspectedValue.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonInspectedValue.TextGroupNumber = 1;
            this.customButtonInspectedValue.UpdateControl = true;
            // 
            // customButtonInspectedText
            // 
            this.customButtonInspectedText.BackColor = System.Drawing.Color.Black;
            this.customButtonInspectedText.BackgroundColor = System.Drawing.Color.Black;
            this.customButtonInspectedText.BitmapIconWhole = null;
            this.customButtonInspectedText.BitmapWhole = null;
            this.customButtonInspectedText.Chinese_TextDisplay = new string[] {
        "检测数量："};
            this.customButtonInspectedText.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 3)};
            this.customButtonInspectedText.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonInspectedText.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(88, 23)};
            this.customButtonInspectedText.ColorTextDisable = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
            this.customButtonInspectedText.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonInspectedText.ColorTextSelected = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonInspectedText.CurrentTextGroupIndex = 0;
            this.customButtonInspectedText.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonInspectedText.CustomButtonData = null;
            this.customButtonInspectedText.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonInspectedText.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonInspectedText.DrawIcon = false;
            this.customButtonInspectedText.DrawText = true;
            this.customButtonInspectedText.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonInspectedText.English_TextDisplay = new string[] {
        "Inspected："};
            this.customButtonInspectedText.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 3)};
            this.customButtonInspectedText.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonInspectedText.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(103, 23)};
            this.customButtonInspectedText.FocusBackgroundDisplay = false;
            this.customButtonInspectedText.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonInspectedText.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonInspectedText.ForeColor = System.Drawing.Color.White;
            this.customButtonInspectedText.HoverBackgroundDisplay = false;
            this.customButtonInspectedText.IconIndex = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonInspectedText.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonInspectedText.IconNumber = 1;
            this.customButtonInspectedText.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(0, 0)};
            this.customButtonInspectedText.LabelControlMode = true;
            this.customButtonInspectedText.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonInspectedText.Location = new System.Drawing.Point(6, 108);
            this.customButtonInspectedText.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonInspectedText.Name = "customButtonInspectedText";
            this.customButtonInspectedText.RectBottom = new System.Drawing.Rectangle(3, 23, 237, 3);
            this.customButtonInspectedText.RectFill = new System.Drawing.Rectangle(3, 3, 237, 20);
            this.customButtonInspectedText.RectLeft = new System.Drawing.Rectangle(0, 3, 3, 20);
            this.customButtonInspectedText.RectLeftBottom = new System.Drawing.Rectangle(0, 23, 3, 3);
            this.customButtonInspectedText.RectLeftTop = new System.Drawing.Rectangle(0, 0, 3, 3);
            this.customButtonInspectedText.RectRight = new System.Drawing.Rectangle(240, 3, 3, 20);
            this.customButtonInspectedText.RectRightBottom = new System.Drawing.Rectangle(240, 23, 3, 3);
            this.customButtonInspectedText.RectRightTop = new System.Drawing.Rectangle(240, 0, 3, 3);
            this.customButtonInspectedText.RectTop = new System.Drawing.Rectangle(3, 0, 237, 3);
            this.customButtonInspectedText.Size = new System.Drawing.Size(103, 30);
            this.customButtonInspectedText.SizeButton = new System.Drawing.Size(103, 30);
            this.customButtonInspectedText.TabIndex = 170;
            this.customButtonInspectedText.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonInspectedText.TextGroupNumber = 1;
            this.customButtonInspectedText.UpdateControl = true;
            // 
            // customButtonBrandValue
            // 
            this.customButtonBrandValue.BackColor = System.Drawing.Color.Black;
            this.customButtonBrandValue.BackgroundColor = System.Drawing.Color.Black;
            this.customButtonBrandValue.BitmapIconWhole = null;
            this.customButtonBrandValue.BitmapWhole = null;
            this.customButtonBrandValue.Chinese_TextDisplay = new string[] {
        "数值"};
            this.customButtonBrandValue.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 3)};
            this.customButtonBrandValue.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonBrandValue.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(39, 24)};
            this.customButtonBrandValue.ColorTextDisable = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
            this.customButtonBrandValue.ColorTextEnable = System.Drawing.Color.Orange;
            this.customButtonBrandValue.ColorTextSelected = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonBrandValue.CurrentTextGroupIndex = 0;
            this.customButtonBrandValue.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonBrandValue.CustomButtonData = null;
            this.customButtonBrandValue.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Automatic;
            this.customButtonBrandValue.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonBrandValue.DrawIcon = false;
            this.customButtonBrandValue.DrawText = true;
            this.customButtonBrandValue.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonBrandValue.English_TextDisplay = new string[] {
        "Value"};
            this.customButtonBrandValue.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 3)};
            this.customButtonBrandValue.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonBrandValue.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(52, 24)};
            this.customButtonBrandValue.FocusBackgroundDisplay = false;
            this.customButtonBrandValue.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonBrandValue.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonBrandValue.ForeColor = System.Drawing.Color.White;
            this.customButtonBrandValue.HoverBackgroundDisplay = false;
            this.customButtonBrandValue.IconIndex = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonBrandValue.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonBrandValue.IconNumber = 1;
            this.customButtonBrandValue.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(0, 0)};
            this.customButtonBrandValue.LabelControlMode = true;
            this.customButtonBrandValue.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonBrandValue.Location = new System.Drawing.Point(650, 78);
            this.customButtonBrandValue.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonBrandValue.Name = "customButtonBrandValue";
            this.customButtonBrandValue.RectBottom = new System.Drawing.Rectangle(3, 23, 237, 3);
            this.customButtonBrandValue.RectFill = new System.Drawing.Rectangle(3, 3, 237, 20);
            this.customButtonBrandValue.RectLeft = new System.Drawing.Rectangle(0, 3, 3, 20);
            this.customButtonBrandValue.RectLeftBottom = new System.Drawing.Rectangle(0, 23, 3, 3);
            this.customButtonBrandValue.RectLeftTop = new System.Drawing.Rectangle(0, 0, 3, 3);
            this.customButtonBrandValue.RectRight = new System.Drawing.Rectangle(240, 3, 3, 20);
            this.customButtonBrandValue.RectRightBottom = new System.Drawing.Rectangle(240, 23, 3, 3);
            this.customButtonBrandValue.RectRightTop = new System.Drawing.Rectangle(240, 0, 3, 3);
            this.customButtonBrandValue.RectTop = new System.Drawing.Rectangle(3, 0, 237, 3);
            this.customButtonBrandValue.Size = new System.Drawing.Size(206, 30);
            this.customButtonBrandValue.SizeButton = new System.Drawing.Size(206, 30);
            this.customButtonBrandValue.TabIndex = 169;
            this.customButtonBrandValue.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonBrandValue.TextGroupNumber = 1;
            this.customButtonBrandValue.UpdateControl = true;
            // 
            // customButtonDurationTimeValue
            // 
            this.customButtonDurationTimeValue.BackColor = System.Drawing.Color.Black;
            this.customButtonDurationTimeValue.BackgroundColor = System.Drawing.Color.Black;
            this.customButtonDurationTimeValue.BitmapIconWhole = null;
            this.customButtonDurationTimeValue.BitmapWhole = null;
            this.customButtonDurationTimeValue.Chinese_TextDisplay = new string[] {
        "剔除率"};
            this.customButtonDurationTimeValue.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 3)};
            this.customButtonDurationTimeValue.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonDurationTimeValue.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(55, 24)};
            this.customButtonDurationTimeValue.ColorTextDisable = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
            this.customButtonDurationTimeValue.ColorTextEnable = System.Drawing.Color.SpringGreen;
            this.customButtonDurationTimeValue.ColorTextSelected = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonDurationTimeValue.CurrentTextGroupIndex = 0;
            this.customButtonDurationTimeValue.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonDurationTimeValue.CustomButtonData = null;
            this.customButtonDurationTimeValue.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Automatic;
            this.customButtonDurationTimeValue.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonDurationTimeValue.DrawIcon = false;
            this.customButtonDurationTimeValue.DrawText = true;
            this.customButtonDurationTimeValue.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonDurationTimeValue.English_TextDisplay = new string[] {
        "Rate"};
            this.customButtonDurationTimeValue.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 3)};
            this.customButtonDurationTimeValue.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonDurationTimeValue.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(43, 24)};
            this.customButtonDurationTimeValue.FocusBackgroundDisplay = false;
            this.customButtonDurationTimeValue.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonDurationTimeValue.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonDurationTimeValue.ForeColor = System.Drawing.Color.White;
            this.customButtonDurationTimeValue.HoverBackgroundDisplay = false;
            this.customButtonDurationTimeValue.IconIndex = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonDurationTimeValue.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonDurationTimeValue.IconNumber = 1;
            this.customButtonDurationTimeValue.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(0, 0)};
            this.customButtonDurationTimeValue.LabelControlMode = true;
            this.customButtonDurationTimeValue.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonDurationTimeValue.Location = new System.Drawing.Point(666, 108);
            this.customButtonDurationTimeValue.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonDurationTimeValue.Name = "customButtonDurationTimeValue";
            this.customButtonDurationTimeValue.RectBottom = new System.Drawing.Rectangle(3, 23, 237, 3);
            this.customButtonDurationTimeValue.RectFill = new System.Drawing.Rectangle(3, 3, 237, 20);
            this.customButtonDurationTimeValue.RectLeft = new System.Drawing.Rectangle(0, 3, 3, 20);
            this.customButtonDurationTimeValue.RectLeftBottom = new System.Drawing.Rectangle(0, 23, 3, 3);
            this.customButtonDurationTimeValue.RectLeftTop = new System.Drawing.Rectangle(0, 0, 3, 3);
            this.customButtonDurationTimeValue.RectRight = new System.Drawing.Rectangle(240, 3, 3, 20);
            this.customButtonDurationTimeValue.RectRightBottom = new System.Drawing.Rectangle(240, 23, 3, 3);
            this.customButtonDurationTimeValue.RectRightTop = new System.Drawing.Rectangle(240, 0, 3, 3);
            this.customButtonDurationTimeValue.RectTop = new System.Drawing.Rectangle(3, 0, 237, 3);
            this.customButtonDurationTimeValue.Size = new System.Drawing.Size(190, 30);
            this.customButtonDurationTimeValue.SizeButton = new System.Drawing.Size(190, 30);
            this.customButtonDurationTimeValue.TabIndex = 168;
            this.customButtonDurationTimeValue.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonDurationTimeValue.TextGroupNumber = 1;
            this.customButtonDurationTimeValue.UpdateControl = true;
            // 
            // customButtonShiftTimeValue
            // 
            this.customButtonShiftTimeValue.BackColor = System.Drawing.Color.Black;
            this.customButtonShiftTimeValue.BackgroundColor = System.Drawing.Color.Black;
            this.customButtonShiftTimeValue.BitmapIconWhole = null;
            this.customButtonShiftTimeValue.BitmapWhole = null;
            this.customButtonShiftTimeValue.Chinese_TextDisplay = new string[] {
        "开始时间 ~ 结束时间"};
            this.customButtonShiftTimeValue.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 3)};
            this.customButtonShiftTimeValue.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonShiftTimeValue.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(160, 24)};
            this.customButtonShiftTimeValue.ColorTextDisable = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
            this.customButtonShiftTimeValue.ColorTextEnable = System.Drawing.Color.Yellow;
            this.customButtonShiftTimeValue.ColorTextSelected = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonShiftTimeValue.CurrentTextGroupIndex = 0;
            this.customButtonShiftTimeValue.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonShiftTimeValue.CustomButtonData = null;
            this.customButtonShiftTimeValue.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Automatic;
            this.customButtonShiftTimeValue.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonShiftTimeValue.DrawIcon = false;
            this.customButtonShiftTimeValue.DrawText = true;
            this.customButtonShiftTimeValue.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonShiftTimeValue.English_TextDisplay = new string[] {
        "BEGIN TIME ~ END TIME"};
            this.customButtonShiftTimeValue.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 3)};
            this.customButtonShiftTimeValue.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonShiftTimeValue.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(212, 24)};
            this.customButtonShiftTimeValue.FocusBackgroundDisplay = false;
            this.customButtonShiftTimeValue.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonShiftTimeValue.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonShiftTimeValue.ForeColor = System.Drawing.Color.White;
            this.customButtonShiftTimeValue.HoverBackgroundDisplay = false;
            this.customButtonShiftTimeValue.IconIndex = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonShiftTimeValue.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonShiftTimeValue.IconNumber = 1;
            this.customButtonShiftTimeValue.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(0, 0)};
            this.customButtonShiftTimeValue.LabelControlMode = true;
            this.customButtonShiftTimeValue.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonShiftTimeValue.Location = new System.Drawing.Point(176, 78);
            this.customButtonShiftTimeValue.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonShiftTimeValue.Name = "customButtonShiftTimeValue";
            this.customButtonShiftTimeValue.RectBottom = new System.Drawing.Rectangle(3, 23, 237, 3);
            this.customButtonShiftTimeValue.RectFill = new System.Drawing.Rectangle(3, 3, 237, 20);
            this.customButtonShiftTimeValue.RectLeft = new System.Drawing.Rectangle(0, 3, 3, 20);
            this.customButtonShiftTimeValue.RectLeftBottom = new System.Drawing.Rectangle(0, 23, 3, 3);
            this.customButtonShiftTimeValue.RectLeftTop = new System.Drawing.Rectangle(0, 0, 3, 3);
            this.customButtonShiftTimeValue.RectRight = new System.Drawing.Rectangle(240, 3, 3, 20);
            this.customButtonShiftTimeValue.RectRightBottom = new System.Drawing.Rectangle(240, 23, 3, 3);
            this.customButtonShiftTimeValue.RectRightTop = new System.Drawing.Rectangle(240, 0, 3, 3);
            this.customButtonShiftTimeValue.RectTop = new System.Drawing.Rectangle(3, 0, 237, 3);
            this.customButtonShiftTimeValue.Size = new System.Drawing.Size(380, 30);
            this.customButtonShiftTimeValue.SizeButton = new System.Drawing.Size(380, 30);
            this.customButtonShiftTimeValue.TabIndex = 166;
            this.customButtonShiftTimeValue.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonShiftTimeValue.TextGroupNumber = 1;
            this.customButtonShiftTimeValue.UpdateControl = true;
            // 
            // customButtonShiftText
            // 
            this.customButtonShiftText.BackColor = System.Drawing.Color.Black;
            this.customButtonShiftText.BackgroundColor = System.Drawing.Color.Black;
            this.customButtonShiftText.BitmapIconWhole = null;
            this.customButtonShiftText.BitmapWhole = null;
            this.customButtonShiftText.Chinese_TextDisplay = new string[] {
        "班次"};
            this.customButtonShiftText.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 3)};
            this.customButtonShiftText.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonShiftText.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(39, 23)};
            this.customButtonShiftText.ColorTextDisable = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
            this.customButtonShiftText.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonShiftText.ColorTextSelected = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonShiftText.CurrentTextGroupIndex = 0;
            this.customButtonShiftText.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonShiftText.CustomButtonData = null;
            this.customButtonShiftText.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonShiftText.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonShiftText.DrawIcon = false;
            this.customButtonShiftText.DrawText = true;
            this.customButtonShiftText.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonShiftText.English_TextDisplay = new string[] {
        "Shift"};
            this.customButtonShiftText.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 3)};
            this.customButtonShiftText.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonShiftText.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(45, 23)};
            this.customButtonShiftText.FocusBackgroundDisplay = false;
            this.customButtonShiftText.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonShiftText.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonShiftText.ForeColor = System.Drawing.Color.White;
            this.customButtonShiftText.HoverBackgroundDisplay = false;
            this.customButtonShiftText.IconIndex = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonShiftText.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonShiftText.IconNumber = 1;
            this.customButtonShiftText.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(0, 0)};
            this.customButtonShiftText.LabelControlMode = true;
            this.customButtonShiftText.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonShiftText.Location = new System.Drawing.Point(6, 78);
            this.customButtonShiftText.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonShiftText.Name = "customButtonShiftText";
            this.customButtonShiftText.RectBottom = new System.Drawing.Rectangle(3, 23, 237, 3);
            this.customButtonShiftText.RectFill = new System.Drawing.Rectangle(3, 3, 237, 20);
            this.customButtonShiftText.RectLeft = new System.Drawing.Rectangle(0, 3, 3, 20);
            this.customButtonShiftText.RectLeftBottom = new System.Drawing.Rectangle(0, 23, 3, 3);
            this.customButtonShiftText.RectLeftTop = new System.Drawing.Rectangle(0, 0, 3, 3);
            this.customButtonShiftText.RectRight = new System.Drawing.Rectangle(240, 3, 3, 20);
            this.customButtonShiftText.RectRightBottom = new System.Drawing.Rectangle(240, 23, 3, 3);
            this.customButtonShiftText.RectRightTop = new System.Drawing.Rectangle(240, 0, 3, 3);
            this.customButtonShiftText.RectTop = new System.Drawing.Rectangle(3, 0, 237, 3);
            this.customButtonShiftText.Size = new System.Drawing.Size(50, 30);
            this.customButtonShiftText.SizeButton = new System.Drawing.Size(50, 30);
            this.customButtonShiftText.TabIndex = 165;
            this.customButtonShiftText.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonShiftText.TextGroupNumber = 1;
            this.customButtonShiftText.UpdateControl = true;
            // 
            // customButtonShiftValue_1
            // 
            this.customButtonShiftValue_1.BackColor = System.Drawing.Color.Black;
            this.customButtonShiftValue_1.BackgroundColor = System.Drawing.Color.Black;
            this.customButtonShiftValue_1.BitmapIconWhole = null;
            this.customButtonShiftValue_1.BitmapWhole = null;
            this.customButtonShiftValue_1.Chinese_TextDisplay = new string[] {
        "班次号"};
            this.customButtonShiftValue_1.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonShiftValue_1.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonShiftValue_1.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(39, 31)};
            this.customButtonShiftValue_1.ColorTextDisable = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
            this.customButtonShiftValue_1.ColorTextEnable = System.Drawing.Color.Yellow;
            this.customButtonShiftValue_1.ColorTextSelected = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonShiftValue_1.CurrentTextGroupIndex = 0;
            this.customButtonShiftValue_1.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonShiftValue_1.CustomButtonData = null;
            this.customButtonShiftValue_1.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Automatic;
            this.customButtonShiftValue_1.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonShiftValue_1.DrawIcon = false;
            this.customButtonShiftValue_1.DrawText = true;
            this.customButtonShiftValue_1.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonShiftValue_1.English_TextDisplay = new string[] {
        "No."};
            this.customButtonShiftValue_1.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 3)};
            this.customButtonShiftValue_1.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonShiftValue_1.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(35, 24)};
            this.customButtonShiftValue_1.FocusBackgroundDisplay = false;
            this.customButtonShiftValue_1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonShiftValue_1.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonShiftValue_1.ForeColor = System.Drawing.Color.White;
            this.customButtonShiftValue_1.HoverBackgroundDisplay = false;
            this.customButtonShiftValue_1.IconIndex = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonShiftValue_1.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonShiftValue_1.IconNumber = 1;
            this.customButtonShiftValue_1.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(0, 0)};
            this.customButtonShiftValue_1.LabelControlMode = true;
            this.customButtonShiftValue_1.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonShiftValue_1.Location = new System.Drawing.Point(56, 78);
            this.customButtonShiftValue_1.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonShiftValue_1.Name = "customButtonShiftValue_1";
            this.customButtonShiftValue_1.RectBottom = new System.Drawing.Rectangle(3, 23, 237, 3);
            this.customButtonShiftValue_1.RectFill = new System.Drawing.Rectangle(3, 3, 237, 20);
            this.customButtonShiftValue_1.RectLeft = new System.Drawing.Rectangle(0, 3, 3, 20);
            this.customButtonShiftValue_1.RectLeftBottom = new System.Drawing.Rectangle(0, 23, 3, 3);
            this.customButtonShiftValue_1.RectLeftTop = new System.Drawing.Rectangle(0, 0, 3, 3);
            this.customButtonShiftValue_1.RectRight = new System.Drawing.Rectangle(240, 3, 3, 20);
            this.customButtonShiftValue_1.RectRightBottom = new System.Drawing.Rectangle(240, 23, 3, 3);
            this.customButtonShiftValue_1.RectRightTop = new System.Drawing.Rectangle(240, 0, 3, 3);
            this.customButtonShiftValue_1.RectTop = new System.Drawing.Rectangle(3, 0, 237, 3);
            this.customButtonShiftValue_1.Size = new System.Drawing.Size(50, 30);
            this.customButtonShiftValue_1.SizeButton = new System.Drawing.Size(50, 30);
            this.customButtonShiftValue_1.TabIndex = 167;
            this.customButtonShiftValue_1.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonShiftValue_1.TextGroupNumber = 1;
            this.customButtonShiftValue_1.UpdateControl = true;
            // 
            // customButtonSelectAll
            // 
            this.customButtonSelectAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonSelectAll.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonSelectAll.BitmapIconWhole = global::VisionSystemControlLibrary.Properties.Resources.SelectAll;
            this.customButtonSelectAll.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.Button;
            this.customButtonSelectAll.Chinese_TextDisplay = new string[] {
        "选择所有"};
            this.customButtonSelectAll.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 13)};
            this.customButtonSelectAll.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonSelectAll.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(72, 23)};
            this.customButtonSelectAll.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonSelectAll.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonSelectAll.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonSelectAll.CurrentTextGroupIndex = 0;
            this.customButtonSelectAll.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Selected;
            this.customButtonSelectAll.CustomButtonData = null;
            this.customButtonSelectAll.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonSelectAll.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Switch;
            this.customButtonSelectAll.DrawIcon = true;
            this.customButtonSelectAll.DrawText = true;
            this.customButtonSelectAll.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonSelectAll.English_TextDisplay = new string[] {
        "SELECT&ALL"};
            this.customButtonSelectAll.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 4),
        new System.Drawing.Point(5, 22)};
            this.customButtonSelectAll.English_TextNumberInTextGroup = new int[] {
        2};
            this.customButtonSelectAll.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(65, 23),
        new System.Drawing.Size(36, 23)};
            this.customButtonSelectAll.FocusBackgroundDisplay = false;
            this.customButtonSelectAll.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonSelectAll.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonSelectAll.ForeColor = System.Drawing.Color.White;
            this.customButtonSelectAll.HoverBackgroundDisplay = false;
            this.customButtonSelectAll.IconIndex = new int[] {
        1,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonSelectAll.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(75, 2),
        new System.Drawing.Point(75, 2)};
            this.customButtonSelectAll.IconNumber = 2;
            this.customButtonSelectAll.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(48, 48),
        new System.Drawing.Size(48, 48)};
            this.customButtonSelectAll.LabelControlMode = false;
            this.customButtonSelectAll.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonSelectAll.Location = new System.Drawing.Point(162, 594);
            this.customButtonSelectAll.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonSelectAll.Name = "customButtonSelectAll";
            this.customButtonSelectAll.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonSelectAll.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonSelectAll.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonSelectAll.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonSelectAll.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonSelectAll.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonSelectAll.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonSelectAll.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonSelectAll.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonSelectAll.Size = new System.Drawing.Size(130, 50);
            this.customButtonSelectAll.SizeButton = new System.Drawing.Size(130, 50);
            this.customButtonSelectAll.TabIndex = 164;
            this.customButtonSelectAll.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonSelectAll.TextGroupNumber = 1;
            this.customButtonSelectAll.UpdateControl = true;
            this.customButtonSelectAll.CustomButton_Click += new System.EventHandler(this.customButtonSelectAll_CustomButton_Click);
            // 
            // parameterSettingsPanel
            // 
            this.parameterSettingsPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.parameterSettingsPanel.Chinese_ParameterName = new string[] {
        "",
        "",
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null};
            this.parameterSettingsPanel.Chinese_ParameterValueNameDisplay = new string[] {
        "",
        "",
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null};
            this.parameterSettingsPanel.ControlEnabled = true;
            this.parameterSettingsPanel.English_ParameterName = new string[] {
        "",
        "",
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null};
            this.parameterSettingsPanel.English_ParameterValueNameDisplay = new string[] {
        "",
        "",
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null};
            this.parameterSettingsPanel.EnterValueEnabled = false;
            this.parameterSettingsPanel.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.parameterSettingsPanel.Location = new System.Drawing.Point(437, 147);
            this.parameterSettingsPanel.Name = "parameterSettingsPanel";
            this.parameterSettingsPanel.ParameterCurrentValue = new float[] {
        50F,
        50F,
        50F,
        50F,
        50F,
        50F,
        50F,
        50F,
        50F,
        50F,
        50F,
        50F,
        50F,
        50F,
        50F};
            this.parameterSettingsPanel.ParameterEnabled = new bool[] {
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true};
            this.parameterSettingsPanel.ParameterMaxValue = new float[] {
        100F,
        100F,
        100F,
        100F,
        100F,
        100F,
        100F,
        100F,
        100F,
        100F,
        100F,
        100F,
        100F,
        100F,
        100F};
            this.parameterSettingsPanel.ParameterMinValue = new float[] {
        0F,
        0F,
        0F,
        0F,
        0F,
        0F,
        0F,
        0F,
        0F,
        0F,
        0F,
        0F,
        0F,
        0F,
        0F};
            this.parameterSettingsPanel.ParameterPrecision = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.parameterSettingsPanel.ParameterType = new int[] {
        2,
        2,
        2,
        2,
        2,
        2,
        2,
        2,
        2,
        2,
        2,
        2,
        2,
        2,
        2};
            this.parameterSettingsPanel.ShowSettingsButton = false;
            this.parameterSettingsPanel.Size = new System.Drawing.Size(220, 450);
            this.parameterSettingsPanel.TabIndex = 85;
            this.parameterSettingsPanel.WindowParameter = 3;
            // 
            // customButtonPlus
            // 
            this.customButtonPlus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonPlus.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonPlus.BitmapIconWhole = null;
            this.customButtonPlus.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.Button;
            this.customButtonPlus.Chinese_TextDisplay = new string[] {
        "+"};
            this.customButtonPlus.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(11, 8)};
            this.customButtonPlus.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonPlus.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(18, 23)};
            this.customButtonPlus.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonPlus.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonPlus.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonPlus.CurrentTextGroupIndex = 0;
            this.customButtonPlus.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonPlus.CustomButtonData = null;
            this.customButtonPlus.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonPlus.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonPlus.DrawIcon = false;
            this.customButtonPlus.DrawText = true;
            this.customButtonPlus.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonPlus.English_TextDisplay = new string[] {
        "+"};
            this.customButtonPlus.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(11, 8)};
            this.customButtonPlus.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonPlus.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(18, 23)};
            this.customButtonPlus.FocusBackgroundDisplay = false;
            this.customButtonPlus.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonPlus.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonPlus.ForeColor = System.Drawing.Color.White;
            this.customButtonPlus.HoverBackgroundDisplay = false;
            this.customButtonPlus.IconIndex = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonPlus.IconLocation = new System.Drawing.Point[0];
            this.customButtonPlus.IconNumber = 0;
            this.customButtonPlus.IconSize = new System.Drawing.Size[0];
            this.customButtonPlus.LabelControlMode = false;
            this.customButtonPlus.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonPlus.Location = new System.Drawing.Point(798, 603);
            this.customButtonPlus.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonPlus.Name = "customButtonPlus";
            this.customButtonPlus.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonPlus.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonPlus.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonPlus.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonPlus.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonPlus.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonPlus.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonPlus.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonPlus.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonPlus.Size = new System.Drawing.Size(41, 41);
            this.customButtonPlus.SizeButton = new System.Drawing.Size(41, 41);
            this.customButtonPlus.TabIndex = 95;
            this.customButtonPlus.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.customButtonPlus.TextGroupNumber = 1;
            this.customButtonPlus.UpdateControl = true;
            this.customButtonPlus.CustomButton_Click += new System.EventHandler(this.customButtonPlus_CustomButton_Click);
            // 
            // customButtonSubtract
            // 
            this.customButtonSubtract.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonSubtract.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonSubtract.BitmapIconWhole = null;
            this.customButtonSubtract.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.Button;
            this.customButtonSubtract.Chinese_TextDisplay = new string[] {
        "-"};
            this.customButtonSubtract.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(14, 8)};
            this.customButtonSubtract.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonSubtract.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(13, 23)};
            this.customButtonSubtract.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonSubtract.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonSubtract.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonSubtract.CurrentTextGroupIndex = 0;
            this.customButtonSubtract.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonSubtract.CustomButtonData = null;
            this.customButtonSubtract.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonSubtract.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonSubtract.DrawIcon = false;
            this.customButtonSubtract.DrawText = true;
            this.customButtonSubtract.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonSubtract.English_TextDisplay = new string[] {
        "-"};
            this.customButtonSubtract.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(14, 8)};
            this.customButtonSubtract.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonSubtract.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(13, 23)};
            this.customButtonSubtract.FocusBackgroundDisplay = false;
            this.customButtonSubtract.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonSubtract.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonSubtract.ForeColor = System.Drawing.Color.White;
            this.customButtonSubtract.HoverBackgroundDisplay = false;
            this.customButtonSubtract.IconIndex = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonSubtract.IconLocation = new System.Drawing.Point[0];
            this.customButtonSubtract.IconNumber = 0;
            this.customButtonSubtract.IconSize = new System.Drawing.Size[0];
            this.customButtonSubtract.LabelControlMode = false;
            this.customButtonSubtract.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonSubtract.Location = new System.Drawing.Point(656, 603);
            this.customButtonSubtract.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonSubtract.Name = "customButtonSubtract";
            this.customButtonSubtract.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonSubtract.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonSubtract.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonSubtract.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonSubtract.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonSubtract.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonSubtract.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonSubtract.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonSubtract.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonSubtract.Size = new System.Drawing.Size(41, 41);
            this.customButtonSubtract.SizeButton = new System.Drawing.Size(41, 41);
            this.customButtonSubtract.TabIndex = 94;
            this.customButtonSubtract.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.customButtonSubtract.TextGroupNumber = 1;
            this.customButtonSubtract.UpdateControl = true;
            this.customButtonSubtract.CustomButton_Click += new System.EventHandler(this.customButtonSubtract_CustomButton_Click);
            // 
            // customButtonStatusBar
            // 
            this.customButtonStatusBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonStatusBar.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonStatusBar.BitmapIconWhole = global::VisionSystemControlLibrary.Properties.Resources.TitleBar;
            this.customButtonStatusBar.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.Button;
            this.customButtonStatusBar.Chinese_TextDisplay = new string[] {
        "图像&标题栏"};
            this.customButtonStatusBar.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 4),
        new System.Drawing.Point(5, 22)};
            this.customButtonStatusBar.Chinese_TextNumberInTextGroup = new int[] {
        2};
            this.customButtonStatusBar.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(39, 23),
        new System.Drawing.Size(55, 23)};
            this.customButtonStatusBar.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonStatusBar.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonStatusBar.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonStatusBar.CurrentTextGroupIndex = 0;
            this.customButtonStatusBar.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonStatusBar.CustomButtonData = null;
            this.customButtonStatusBar.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonStatusBar.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Switch;
            this.customButtonStatusBar.DrawIcon = true;
            this.customButtonStatusBar.DrawText = true;
            this.customButtonStatusBar.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonStatusBar.English_TextDisplay = new string[] {
        "STATUS&BAR"};
            this.customButtonStatusBar.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 4),
        new System.Drawing.Point(5, 22)};
            this.customButtonStatusBar.English_TextNumberInTextGroup = new int[] {
        2};
            this.customButtonStatusBar.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(71, 23),
        new System.Drawing.Size(41, 23)};
            this.customButtonStatusBar.FocusBackgroundDisplay = false;
            this.customButtonStatusBar.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonStatusBar.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonStatusBar.ForeColor = System.Drawing.Color.White;
            this.customButtonStatusBar.HoverBackgroundDisplay = false;
            this.customButtonStatusBar.IconIndex = new int[] {
        1,
        3,
        0,
        2,
        2,
        0,
        2,
        0,
        2};
            this.customButtonStatusBar.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(80, 9),
        new System.Drawing.Point(80, 9),
        new System.Drawing.Point(80, 9),
        new System.Drawing.Point(80, 9)};
            this.customButtonStatusBar.IconNumber = 4;
            this.customButtonStatusBar.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(53, 37),
        new System.Drawing.Size(53, 37),
        new System.Drawing.Size(53, 37),
        new System.Drawing.Size(53, 37)};
            this.customButtonStatusBar.LabelControlMode = false;
            this.customButtonStatusBar.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonStatusBar.Location = new System.Drawing.Point(889, 599);
            this.customButtonStatusBar.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonStatusBar.Name = "customButtonStatusBar";
            this.customButtonStatusBar.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonStatusBar.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonStatusBar.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonStatusBar.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonStatusBar.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonStatusBar.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonStatusBar.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonStatusBar.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonStatusBar.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonStatusBar.Size = new System.Drawing.Size(130, 50);
            this.customButtonStatusBar.SizeButton = new System.Drawing.Size(130, 50);
            this.customButtonStatusBar.TabIndex = 93;
            this.customButtonStatusBar.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonStatusBar.TextGroupNumber = 1;
            this.customButtonStatusBar.UpdateControl = true;
            this.customButtonStatusBar.Visible = false;
            this.customButtonStatusBar.CustomButton_Click += new System.EventHandler(this.customButtonStatusBar_CustomButton_Click);
            // 
            // imageDisplayView
            // 
            this.imageDisplayView.AutoShowTitle = false;
            this.imageDisplayView.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.imageDisplayView.BitmapDisplay = null;
            this.imageDisplayView.ColorStatusBarControlBackground = System.Drawing.Color.FromArgb(((int)(((byte)(126)))), ((int)(((byte)(126)))), ((int)(((byte)(126)))));
            this.imageDisplayView.ControlScale = 1D;
            this.imageDisplayView.ControlScale_X = 0.9D;
            this.imageDisplayView.ControlScale_Y = 0.9D;
            this.imageDisplayView.ControlSize = new System.Drawing.Size(576, 432);
            this.imageDisplayView.CurrentValueFont = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.imageDisplayView.CurrentValueLocation = new System.Drawing.Point(318, 25);
            this.imageDisplayView.CurrentValueSize = new System.Drawing.Size(98, 13);
            this.imageDisplayView.ImageFileName = "1.jpg";
            this.imageDisplayView.ImageFilePath = ".\\ConfigData\\RejectsImage\\";
            this.imageDisplayView.Information = ((VisionSystemClassLibrary.Struct.ImageInformation)(resources.GetObject("imageDisplayView.Information")));
            this.imageDisplayView.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.imageDisplayView.Location = new System.Drawing.Point(440, 150);
            this.imageDisplayView.MaxValueFont = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.imageDisplayView.MaxValueLocation = new System.Drawing.Point(425, 25);
            this.imageDisplayView.MaxValueSize = new System.Drawing.Size(98, 13);
            this.imageDisplayView.MessageFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.imageDisplayView.MessageLampLocation = new System.Drawing.Point(531, 9);
            this.imageDisplayView.MessageLampSize = new System.Drawing.Size(28, 28);
            this.imageDisplayView.MessageLocation = new System.Drawing.Point(8, 5);
            this.imageDisplayView.MessageSize = new System.Drawing.Size(486, 18);
            this.imageDisplayView.MinValueFont = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.imageDisplayView.MinValueLocation = new System.Drawing.Point(211, 25);
            this.imageDisplayView.MinValueSize = new System.Drawing.Size(98, 13);
            this.imageDisplayView.Name = "imageDisplayView";
            this.imageDisplayView.ShowTitle = true;
            this.imageDisplayView.Size = new System.Drawing.Size(576, 432);
            this.imageDisplayView.SlotLocation = new System.Drawing.Point(12, 27);
            this.imageDisplayView.SlotSize = new System.Drawing.Size(192, 10);
            this.imageDisplayView.StatusBarControlScale = 1D;
            this.imageDisplayView.StatusBarControlScale_X = 0.894375D;
            this.imageDisplayView.StatusBarControlScale_Y = 0.9D;
            this.imageDisplayView.StatusBarControlSize = new System.Drawing.Size(572, 45);
            this.imageDisplayView.TabIndex = 92;
            this.imageDisplayView.YOffset = 2;
            // 
            // customButtonViewBackground
            // 
            this.customButtonViewBackground.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.customButtonViewBackground.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.customButtonViewBackground.BitmapIconWhole = null;
            this.customButtonViewBackground.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.CameraBackground;
            this.customButtonViewBackground.Chinese_TextDisplay = new string[] {
        "放大背景"};
            this.customButtonViewBackground.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 14)};
            this.customButtonViewBackground.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonViewBackground.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(39, 23)};
            this.customButtonViewBackground.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonViewBackground.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonViewBackground.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonViewBackground.CurrentTextGroupIndex = 0;
            this.customButtonViewBackground.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonViewBackground.CustomButtonData = null;
            this.customButtonViewBackground.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonViewBackground.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonViewBackground.DrawIcon = true;
            this.customButtonViewBackground.DrawText = false;
            this.customButtonViewBackground.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonViewBackground.English_TextDisplay = new string[] {
        "ZOOMIN BACKGROUND"};
            this.customButtonViewBackground.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 14)};
            this.customButtonViewBackground.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonViewBackground.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(52, 23)};
            this.customButtonViewBackground.FocusBackgroundDisplay = false;
            this.customButtonViewBackground.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonViewBackground.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonViewBackground.ForeColor = System.Drawing.Color.White;
            this.customButtonViewBackground.HoverBackgroundDisplay = false;
            this.customButtonViewBackground.IconIndex = new int[] {
        1,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonViewBackground.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonViewBackground.IconNumber = 1;
            this.customButtonViewBackground.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(0, 0)};
            this.customButtonViewBackground.LabelControlMode = true;
            this.customButtonViewBackground.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonViewBackground.Location = new System.Drawing.Point(437, 147);
            this.customButtonViewBackground.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonViewBackground.Name = "customButtonViewBackground";
            this.customButtonViewBackground.RectBottom = new System.Drawing.Rectangle(3, 103, 134, 3);
            this.customButtonViewBackground.RectFill = new System.Drawing.Rectangle(3, 3, 134, 100);
            this.customButtonViewBackground.RectLeft = new System.Drawing.Rectangle(0, 3, 3, 100);
            this.customButtonViewBackground.RectLeftBottom = new System.Drawing.Rectangle(0, 103, 3, 3);
            this.customButtonViewBackground.RectLeftTop = new System.Drawing.Rectangle(0, 0, 3, 3);
            this.customButtonViewBackground.RectRight = new System.Drawing.Rectangle(137, 3, 3, 100);
            this.customButtonViewBackground.RectRightBottom = new System.Drawing.Rectangle(137, 103, 3, 3);
            this.customButtonViewBackground.RectRightTop = new System.Drawing.Rectangle(137, 0, 3, 3);
            this.customButtonViewBackground.RectTop = new System.Drawing.Rectangle(3, 0, 134, 3);
            this.customButtonViewBackground.Size = new System.Drawing.Size(582, 438);
            this.customButtonViewBackground.SizeButton = new System.Drawing.Size(582, 438);
            this.customButtonViewBackground.TabIndex = 72;
            this.customButtonViewBackground.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonViewBackground.TextGroupNumber = 1;
            this.customButtonViewBackground.UpdateControl = true;
            // 
            // customButtonNextPage_List_1
            // 
            this.customButtonNextPage_List_1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonNextPage_List_1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonNextPage_List_1.BitmapIconWhole = global::VisionSystemControlLibrary.Properties.Resources.Down;
            this.customButtonNextPage_List_1.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.Button;
            this.customButtonNextPage_List_1.Chinese_TextDisplay = new string[] {
        "下一页"};
            this.customButtonNextPage_List_1.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 5)};
            this.customButtonNextPage_List_1.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonNextPage_List_1.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(55, 23)};
            this.customButtonNextPage_List_1.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonNextPage_List_1.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonNextPage_List_1.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonNextPage_List_1.CurrentTextGroupIndex = 0;
            this.customButtonNextPage_List_1.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonNextPage_List_1.CustomButtonData = null;
            this.customButtonNextPage_List_1.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonNextPage_List_1.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonNextPage_List_1.DrawIcon = true;
            this.customButtonNextPage_List_1.DrawText = false;
            this.customButtonNextPage_List_1.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonNextPage_List_1.English_TextDisplay = new string[] {
        "NEXT&PAGE"};
            this.customButtonNextPage_List_1.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 5),
        new System.Drawing.Point(5, 25)};
            this.customButtonNextPage_List_1.English_TextNumberInTextGroup = new int[] {
        2};
            this.customButtonNextPage_List_1.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(51, 23),
        new System.Drawing.Size(51, 23)};
            this.customButtonNextPage_List_1.FocusBackgroundDisplay = false;
            this.customButtonNextPage_List_1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonNextPage_List_1.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonNextPage_List_1.ForeColor = System.Drawing.Color.White;
            this.customButtonNextPage_List_1.HoverBackgroundDisplay = false;
            this.customButtonNextPage_List_1.IconIndex = new int[] {
        1,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonNextPage_List_1.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(8, 2),
        new System.Drawing.Point(8, 2)};
            this.customButtonNextPage_List_1.IconNumber = 2;
            this.customButtonNextPage_List_1.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(48, 70),
        new System.Drawing.Size(48, 70)};
            this.customButtonNextPage_List_1.LabelControlMode = false;
            this.customButtonNextPage_List_1.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonNextPage_List_1.Location = new System.Drawing.Point(363, 581);
            this.customButtonNextPage_List_1.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonNextPage_List_1.Name = "customButtonNextPage_List_1";
            this.customButtonNextPage_List_1.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonNextPage_List_1.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonNextPage_List_1.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonNextPage_List_1.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonNextPage_List_1.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonNextPage_List_1.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonNextPage_List_1.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonNextPage_List_1.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonNextPage_List_1.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonNextPage_List_1.Size = new System.Drawing.Size(63, 73);
            this.customButtonNextPage_List_1.SizeButton = new System.Drawing.Size(63, 73);
            this.customButtonNextPage_List_1.TabIndex = 88;
            this.customButtonNextPage_List_1.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonNextPage_List_1.TextGroupNumber = 1;
            this.customButtonNextPage_List_1.UpdateControl = true;
            this.customButtonNextPage_List_1.CustomButton_Click += new System.EventHandler(this.customButtonNextPage_List_1_CustomButton_Click);
            // 
            // customButtonPreviousPage_List_1
            // 
            this.customButtonPreviousPage_List_1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonPreviousPage_List_1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonPreviousPage_List_1.BitmapIconWhole = global::VisionSystemControlLibrary.Properties.Resources.Up;
            this.customButtonPreviousPage_List_1.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.Button;
            this.customButtonPreviousPage_List_1.Chinese_TextDisplay = new string[] {
        "上一页"};
            this.customButtonPreviousPage_List_1.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 5)};
            this.customButtonPreviousPage_List_1.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonPreviousPage_List_1.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(55, 23)};
            this.customButtonPreviousPage_List_1.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonPreviousPage_List_1.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonPreviousPage_List_1.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonPreviousPage_List_1.CurrentTextGroupIndex = 0;
            this.customButtonPreviousPage_List_1.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonPreviousPage_List_1.CustomButtonData = null;
            this.customButtonPreviousPage_List_1.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonPreviousPage_List_1.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonPreviousPage_List_1.DrawIcon = true;
            this.customButtonPreviousPage_List_1.DrawText = false;
            this.customButtonPreviousPage_List_1.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonPreviousPage_List_1.English_TextDisplay = new string[] {
        "PREV.&PAGE"};
            this.customButtonPreviousPage_List_1.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 5),
        new System.Drawing.Point(5, 25)};
            this.customButtonPreviousPage_List_1.English_TextNumberInTextGroup = new int[] {
        2};
            this.customButtonPreviousPage_List_1.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(54, 23),
        new System.Drawing.Size(51, 23)};
            this.customButtonPreviousPage_List_1.FocusBackgroundDisplay = false;
            this.customButtonPreviousPage_List_1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonPreviousPage_List_1.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonPreviousPage_List_1.ForeColor = System.Drawing.Color.White;
            this.customButtonPreviousPage_List_1.HoverBackgroundDisplay = false;
            this.customButtonPreviousPage_List_1.IconIndex = new int[] {
        1,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonPreviousPage_List_1.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(8, 2),
        new System.Drawing.Point(8, 2)};
            this.customButtonPreviousPage_List_1.IconNumber = 2;
            this.customButtonPreviousPage_List_1.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(48, 70),
        new System.Drawing.Size(48, 70)};
            this.customButtonPreviousPage_List_1.LabelControlMode = false;
            this.customButtonPreviousPage_List_1.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonPreviousPage_List_1.Location = new System.Drawing.Point(297, 581);
            this.customButtonPreviousPage_List_1.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonPreviousPage_List_1.Name = "customButtonPreviousPage_List_1";
            this.customButtonPreviousPage_List_1.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonPreviousPage_List_1.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonPreviousPage_List_1.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonPreviousPage_List_1.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonPreviousPage_List_1.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonPreviousPage_List_1.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonPreviousPage_List_1.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonPreviousPage_List_1.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonPreviousPage_List_1.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonPreviousPage_List_1.Size = new System.Drawing.Size(63, 73);
            this.customButtonPreviousPage_List_1.SizeButton = new System.Drawing.Size(63, 73);
            this.customButtonPreviousPage_List_1.TabIndex = 87;
            this.customButtonPreviousPage_List_1.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonPreviousPage_List_1.TextGroupNumber = 1;
            this.customButtonPreviousPage_List_1.UpdateControl = true;
            this.customButtonPreviousPage_List_1.CustomButton_Click += new System.EventHandler(this.customButtonPreviousPage_List_1_CustomButton_Click);
            // 
            // customButtonParameter
            // 
            this.customButtonParameter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonParameter.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonParameter.BitmapIconWhole = global::VisionSystemControlLibrary.Properties.Resources.Parameter;
            this.customButtonParameter.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.Button;
            this.customButtonParameter.Chinese_TextDisplay = new string[] {
        "参数信息"};
            this.customButtonParameter.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 13)};
            this.customButtonParameter.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonParameter.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(72, 23)};
            this.customButtonParameter.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonParameter.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonParameter.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonParameter.CurrentTextGroupIndex = 0;
            this.customButtonParameter.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonParameter.CustomButtonData = null;
            this.customButtonParameter.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonParameter.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Switch;
            this.customButtonParameter.DrawIcon = true;
            this.customButtonParameter.DrawText = true;
            this.customButtonParameter.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonParameter.English_TextDisplay = new string[] {
        "PARAMETER"};
            this.customButtonParameter.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 13)};
            this.customButtonParameter.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonParameter.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(111, 23)};
            this.customButtonParameter.FocusBackgroundDisplay = false;
            this.customButtonParameter.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonParameter.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonParameter.ForeColor = System.Drawing.Color.White;
            this.customButtonParameter.HoverBackgroundDisplay = false;
            this.customButtonParameter.IconIndex = new int[] {
        1,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonParameter.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(110, 5),
        new System.Drawing.Point(110, 5)};
            this.customButtonParameter.IconNumber = 2;
            this.customButtonParameter.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(40, 40),
        new System.Drawing.Size(40, 40)};
            this.customButtonParameter.LabelControlMode = false;
            this.customButtonParameter.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonParameter.Location = new System.Drawing.Point(6, 594);
            this.customButtonParameter.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonParameter.Name = "customButtonParameter";
            this.customButtonParameter.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonParameter.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonParameter.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonParameter.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonParameter.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonParameter.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonParameter.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonParameter.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonParameter.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonParameter.Size = new System.Drawing.Size(150, 50);
            this.customButtonParameter.SizeButton = new System.Drawing.Size(150, 50);
            this.customButtonParameter.TabIndex = 84;
            this.customButtonParameter.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonParameter.TextGroupNumber = 1;
            this.customButtonParameter.UpdateControl = true;
            this.customButtonParameter.CustomButton_Click += new System.EventHandler(this.customButtonParameter_CustomButton_Click);
            // 
            // customButtonViewReject
            // 
            this.customButtonViewReject.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonViewReject.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonViewReject.BitmapIconWhole = global::VisionSystemControlLibrary.Properties.Resources.LoadReject;
            this.customButtonViewReject.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.Button;
            this.customButtonViewReject.Chinese_TextDisplay = new string[] {
        "查看&剔除图像"};
            this.customButtonViewReject.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 3),
        new System.Drawing.Point(5, 23)};
            this.customButtonViewReject.Chinese_TextNumberInTextGroup = new int[] {
        2};
            this.customButtonViewReject.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(39, 23),
        new System.Drawing.Size(72, 23)};
            this.customButtonViewReject.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonViewReject.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonViewReject.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonViewReject.CurrentTextGroupIndex = 0;
            this.customButtonViewReject.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonViewReject.CustomButtonData = null;
            this.customButtonViewReject.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonViewReject.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Switch;
            this.customButtonViewReject.DrawIcon = true;
            this.customButtonViewReject.DrawText = true;
            this.customButtonViewReject.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonViewReject.English_TextDisplay = new string[] {
        "VIEW&REJECT"};
            this.customButtonViewReject.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 3),
        new System.Drawing.Point(5, 23)};
            this.customButtonViewReject.English_TextNumberInTextGroup = new int[] {
        2};
            this.customButtonViewReject.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(50, 23),
        new System.Drawing.Size(65, 23)};
            this.customButtonViewReject.FocusBackgroundDisplay = false;
            this.customButtonViewReject.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonViewReject.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonViewReject.ForeColor = System.Drawing.Color.White;
            this.customButtonViewReject.HoverBackgroundDisplay = false;
            this.customButtonViewReject.IconIndex = new int[] {
        1,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonViewReject.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(70, 8),
        new System.Drawing.Point(70, 8)};
            this.customButtonViewReject.IconNumber = 2;
            this.customButtonViewReject.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(53, 37),
        new System.Drawing.Size(53, 37)};
            this.customButtonViewReject.LabelControlMode = false;
            this.customButtonViewReject.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonViewReject.Location = new System.Drawing.Point(437, 599);
            this.customButtonViewReject.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonViewReject.Name = "customButtonViewReject";
            this.customButtonViewReject.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonViewReject.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonViewReject.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonViewReject.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonViewReject.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonViewReject.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonViewReject.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonViewReject.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonViewReject.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonViewReject.Size = new System.Drawing.Size(130, 50);
            this.customButtonViewReject.SizeButton = new System.Drawing.Size(130, 50);
            this.customButtonViewReject.TabIndex = 83;
            this.customButtonViewReject.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonViewReject.TextGroupNumber = 1;
            this.customButtonViewReject.UpdateControl = true;
            this.customButtonViewReject.CustomButton_Click += new System.EventHandler(this.customButtonViewReject_CustomButton_Click);
            // 
            // customButtonSelection
            // 
            this.customButtonSelection.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonSelection.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonSelection.BitmapIconWhole = null;
            this.customButtonSelection.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.Button;
            this.customButtonSelection.Chinese_TextDisplay = new string[] {
        "1"};
            this.customButtonSelection.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(32, 8)};
            this.customButtonSelection.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonSelection.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(16, 24)};
            this.customButtonSelection.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonSelection.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonSelection.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonSelection.CurrentTextGroupIndex = 0;
            this.customButtonSelection.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonSelection.CustomButtonData = null;
            this.customButtonSelection.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Automatic;
            this.customButtonSelection.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonSelection.DrawIcon = false;
            this.customButtonSelection.DrawText = true;
            this.customButtonSelection.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonSelection.English_TextDisplay = new string[] {
        "1"};
            this.customButtonSelection.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(32, 8)};
            this.customButtonSelection.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonSelection.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(16, 24)};
            this.customButtonSelection.FocusBackgroundDisplay = false;
            this.customButtonSelection.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonSelection.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonSelection.ForeColor = System.Drawing.Color.White;
            this.customButtonSelection.HoverBackgroundDisplay = false;
            this.customButtonSelection.IconIndex = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonSelection.IconLocation = new System.Drawing.Point[0];
            this.customButtonSelection.IconNumber = 0;
            this.customButtonSelection.IconSize = new System.Drawing.Size[0];
            this.customButtonSelection.LabelControlMode = false;
            this.customButtonSelection.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonSelection.Location = new System.Drawing.Point(707, 603);
            this.customButtonSelection.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonSelection.Name = "customButtonSelection";
            this.customButtonSelection.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonSelection.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonSelection.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonSelection.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonSelection.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonSelection.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonSelection.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonSelection.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonSelection.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonSelection.Size = new System.Drawing.Size(81, 41);
            this.customButtonSelection.SizeButton = new System.Drawing.Size(81, 41);
            this.customButtonSelection.TabIndex = 82;
            this.customButtonSelection.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.customButtonSelection.TextGroupNumber = 1;
            this.customButtonSelection.UpdateControl = true;
            this.customButtonSelection.CustomButton_Click += new System.EventHandler(this.customButtonSelection_CustomButton_Click);
            // 
            // customListRejects
            // 
            this.customListRejects.BackColor = System.Drawing.Color.Black;
            this.customListRejects.BitmapBackgroundIndex = new int[] {
        0,
        0,
        0,
        0};
            this.customListRejects.BitmapBackgroundNumber = 1;
            this.customListRejects.BitmapBackgroundWhole = global::VisionSystemControlLibrary.Properties.Resources.ListHeader;
            this.customListRejects.BitmapIcon = new System.Drawing.Bitmap[] {
        global::VisionSystemControlLibrary.Properties.Resources.CustomListItem_BackupBrandFolder,
        global::VisionSystemControlLibrary.Properties.Resources.CustomListItem_Folder,
        global::VisionSystemControlLibrary.Properties.Resources.CustomListItem_SystemFile,
        global::VisionSystemControlLibrary.Properties.Resources.CustomListItem_Marked};
            this.customListRejects.Chinese_ColumnNameDisplay = new string[] {
        "工具",
        "状态",
        "数值",
        "查看"};
            this.customListRejects.ColorControlBackground = System.Drawing.Color.Black;
            this.customListRejects.ColorItemBackgroundSelected = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.customListRejects.ColorItemBackgroundUnselected = System.Drawing.Color.Black;
            this.customListRejects.ColorItemTextSelected = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customListRejects.ColorItemTextUnselected = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customListRejects.ColorListHeaderBackground = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.customListRejects.ColorListHeaderColumnText_Disable = new System.Drawing.Color[] {
        System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))))};
            this.customListRejects.ColorListHeaderColumnText_Enable = new System.Drawing.Color[] {
        System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))))};
            this.customListRejects.ColorListItemBackground = System.Drawing.Color.Black;
            this.customListRejects.ColorPageBackground = System.Drawing.Color.Black;
            this.customListRejects.ColorPageText = System.Drawing.Color.Yellow;
            this.customListRejects.ColumnNameXOffSetValue = 2;
            this.customListRejects.ColumnNumber = 4;
            this.customListRejects.ColumnWidth = new int[] {
        200,
        55,
        85,
        69};
            this.customListRejects.CurrentColumnNameGroupIndex = new int[] {
        0,
        0,
        0,
        0};
            this.customListRejects.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customListRejects.English_ColumnNameDisplay = new string[] {
        "Tool",
        "State",
        "Value",
        "Marked"};
            this.customListRejects.FontListHeader = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customListRejects.FontListItem = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customListRejects.FontPage = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customListRejects.ForeColor = System.Drawing.Color.White;
            this.customListRejects.ItemDataDisplay = new bool[] {
        true,
        true,
        true,
        true};
            this.customListRejects.ItemDataNumber = 0;
            this.customListRejects.ItemIconIndex = new int[] {
        -1,
        -1,
        -1,
        -1};
            this.customListRejects.ItemIconNumber = 4;
            this.customListRejects.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customListRejects.ListEnabled = true;
            this.customListRejects.ListHeaderHeight = 36;
            this.customListRejects.ListHeaderTextAlignment = System.Drawing.StringAlignment.Near;
            this.customListRejects.ListItemHeight = 35;
            this.customListRejects.ListItemTextAlignment = System.Drawing.StringAlignment.Near;
            this.customListRejects.ListItemXOffSetValue = 7;
            this.customListRejects.Location = new System.Drawing.Point(6, 147);
            this.customListRejects.Name = "customListRejects";
            this.customListRejects.PageHeight = 25;
            this.customListRejects.RectBottom = new System.Drawing.Rectangle(3, 23, 237, 3);
            this.customListRejects.RectFill = new System.Drawing.Rectangle(3, 3, 237, 20);
            this.customListRejects.RectLeft = new System.Drawing.Rectangle(0, 3, 3, 20);
            this.customListRejects.RectLeftBottom = new System.Drawing.Rectangle(0, 23, 3, 3);
            this.customListRejects.RectLeftTop = new System.Drawing.Rectangle(0, 0, 3, 3);
            this.customListRejects.RectRight = new System.Drawing.Rectangle(240, 3, 3, 20);
            this.customListRejects.RectRightBottom = new System.Drawing.Rectangle(240, 23, 3, 3);
            this.customListRejects.RectRightTop = new System.Drawing.Rectangle(240, 0, 3, 3);
            this.customListRejects.RectTop = new System.Drawing.Rectangle(3, 0, 237, 3);
            this.customListRejects.SelectedItemNumber = 0;
            this.customListRejects.SelectedItemType = false;
            this.customListRejects.SelectionColumnIndex = 3;
            this.customListRejects.Size = new System.Drawing.Size(420, 420);
            this.customListRejects.SizeControl = new System.Drawing.Size(420, 420);
            this.customListRejects.SizeItemIcon = new System.Drawing.Size[] {
        new System.Drawing.Size(20, 20),
        new System.Drawing.Size(20, 20),
        new System.Drawing.Size(20, 20),
        new System.Drawing.Size(20, 20)};
            this.customListRejects.TabIndex = 77;
            this.customListRejects.CustomListItem_Click += new System.EventHandler(this.customListRejects_CustomListItem_Click);
            // 
            // customButtonRecords
            // 
            this.customButtonRecords.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonRecords.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonRecords.BitmapIconWhole = global::VisionSystemControlLibrary.Properties.Resources.RenameBrand;
            this.customButtonRecords.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.Button;
            this.customButtonRecords.Chinese_TextDisplay = new string[] {
        "历史数据"};
            this.customButtonRecords.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 13)};
            this.customButtonRecords.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonRecords.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(72, 23)};
            this.customButtonRecords.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonRecords.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonRecords.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonRecords.CurrentTextGroupIndex = 0;
            this.customButtonRecords.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonRecords.CustomButtonData = null;
            this.customButtonRecords.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonRecords.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonRecords.DrawIcon = true;
            this.customButtonRecords.DrawText = true;
            this.customButtonRecords.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonRecords.English_TextDisplay = new string[] {
        "RECORDS"};
            this.customButtonRecords.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 13)};
            this.customButtonRecords.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonRecords.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(86, 23)};
            this.customButtonRecords.FocusBackgroundDisplay = false;
            this.customButtonRecords.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonRecords.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonRecords.ForeColor = System.Drawing.Color.White;
            this.customButtonRecords.HoverBackgroundDisplay = false;
            this.customButtonRecords.IconIndex = new int[] {
        1,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonRecords.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(90, 8),
        new System.Drawing.Point(90, 8)};
            this.customButtonRecords.IconNumber = 2;
            this.customButtonRecords.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(53, 37),
        new System.Drawing.Size(53, 37)};
            this.customButtonRecords.LabelControlMode = false;
            this.customButtonRecords.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonRecords.Location = new System.Drawing.Point(873, 83);
            this.customButtonRecords.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonRecords.Name = "customButtonRecords";
            this.customButtonRecords.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonRecords.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonRecords.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonRecords.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonRecords.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonRecords.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonRecords.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonRecords.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonRecords.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonRecords.Size = new System.Drawing.Size(130, 50);
            this.customButtonRecords.SizeButton = new System.Drawing.Size(130, 50);
            this.customButtonRecords.TabIndex = 76;
            this.customButtonRecords.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonRecords.TextGroupNumber = 1;
            this.customButtonRecords.UpdateControl = true;
            this.customButtonRecords.CustomButton_Click += new System.EventHandler(this.customButtonRecords_CustomButton_Click);
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
            this.customButtonClose.TabIndex = 53;
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
        " - 统计"};
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
        " - STATISTICS"};
            this.customButtonCaption.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(314, 7)};
            this.customButtonCaption.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonCaption.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(147, 29)};
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
            this.customButtonCaption.TabIndex = 52;
            this.customButtonCaption.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.customButtonCaption.TextGroupNumber = 1;
            this.customButtonCaption.UpdateControl = true;
            // 
            // StatisticsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.Controls.Add(this.labelIcon);
            this.Controls.Add(this.customButtonShiftValue_2);
            this.Controls.Add(this.customButtonDurationTimeText);
            this.Controls.Add(this.customButtonBrandText);
            this.Controls.Add(this.customButtonRejectedValue);
            this.Controls.Add(this.customButtonRejectedText);
            this.Controls.Add(this.customButtonInspectedValue);
            this.Controls.Add(this.customButtonInspectedText);
            this.Controls.Add(this.customButtonBrandValue);
            this.Controls.Add(this.customButtonDurationTimeValue);
            this.Controls.Add(this.customButtonShiftTimeValue);
            this.Controls.Add(this.customButtonShiftText);
            this.Controls.Add(this.customButtonShiftValue_1);
            this.Controls.Add(this.customButtonSelectAll);
            this.Controls.Add(this.parameterSettingsPanel);
            this.Controls.Add(this.customButtonPlus);
            this.Controls.Add(this.customButtonSubtract);
            this.Controls.Add(this.customButtonStatusBar);
            this.Controls.Add(this.imageDisplayView);
            this.Controls.Add(this.customButtonViewBackground);
            this.Controls.Add(this.customButtonNextPage_List_1);
            this.Controls.Add(this.customButtonPreviousPage_List_1);
            this.Controls.Add(this.customButtonParameter);
            this.Controls.Add(this.customButtonViewReject);
            this.Controls.Add(this.customButtonSelection);
            this.Controls.Add(this.customListRejects);
            this.Controls.Add(this.customButtonRecords);
            this.Controls.Add(this.customButtonClose);
            this.Controls.Add(this.customButtonCaption);
            this.Name = "StatisticsControl";
            this.Size = new System.Drawing.Size(1024, 662);
            this.ResumeLayout(false);

        }

        #endregion

        private CustomButton customButtonClose;
        private CustomButton customButtonCaption;
        private CustomButton customButtonViewBackground;
        private CustomButton customButtonRecords;
        private CustomList customListRejects;
        private CustomButton customButtonSelection;
        private CustomButton customButtonViewReject;
        private CustomButton customButtonParameter;
        private ParameterSettingsPanel parameterSettingsPanel;
        private CustomButton customButtonNextPage_List_1;
        private CustomButton customButtonPreviousPage_List_1;
        private System.Windows.Forms.Timer timerStatistics;
        private ImageDisplay imageDisplayView;
        private CustomButton customButtonStatusBar;
        private CustomButton customButtonSubtract;
        private CustomButton customButtonPlus;
        private CustomButton customButtonSelectAll;
        private CustomButton customButtonShiftValue_2;
        private CustomButton customButtonDurationTimeText;
        private CustomButton customButtonBrandText;
        private CustomButton customButtonRejectedValue;
        private CustomButton customButtonRejectedText;
        private CustomButton customButtonInspectedValue;
        private CustomButton customButtonInspectedText;
        private CustomButton customButtonBrandValue;
        private CustomButton customButtonDurationTimeValue;
        private CustomButton customButtonShiftTimeValue;
        private CustomButton customButtonShiftText;
        private CustomButton customButtonShiftValue_1;
        private System.Windows.Forms.Label labelIcon;
    }
}
