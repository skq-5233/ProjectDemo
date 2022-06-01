namespace VisionSystemControlLibrary
{
    partial class StatusBar
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
            this.labelMessage = new System.Windows.Forms.Label();
            this.customButtonMessageLamp = new VisionSystemControlLibrary.CustomButton();
            this.customButtonMaxValue = new VisionSystemControlLibrary.CustomButton();
            this.customButtonCurrentValue = new VisionSystemControlLibrary.CustomButton();
            this.customButtonMinValue = new VisionSystemControlLibrary.CustomButton();
            this.customListHeaderSlot = new VisionSystemControlLibrary.CustomListHeader();
            this.customButtonBackground = new VisionSystemControlLibrary.CustomButton();
            this.SuspendLayout();
            // 
            // labelMessage
            // 
            this.labelMessage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(92)))), ((int)(((byte)(88)))));
            this.labelMessage.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelMessage.ForeColor = System.Drawing.Color.SpringGreen;
            this.labelMessage.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelMessage.Location = new System.Drawing.Point(9, 8);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(544, 18);
            this.labelMessage.TabIndex = 53;
            this.labelMessage.Text = "OK";
            // 
            // customButtonMessageLamp
            // 
            this.customButtonMessageLamp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(92)))), ((int)(((byte)(88)))));
            this.customButtonMessageLamp.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(92)))), ((int)(((byte)(88)))));
            this.customButtonMessageLamp.BitmapIconWhole = null;
            this.customButtonMessageLamp.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.MessageLamp;
            this.customButtonMessageLamp.Chinese_TextDisplay = new string[] {
        ""};
            this.customButtonMessageLamp.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonMessageLamp.Chinese_TextNumberInTextGroup = new int[] {
        0};
            this.customButtonMessageLamp.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(0, 0)};
            this.customButtonMessageLamp.ColorTextDisable = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
            this.customButtonMessageLamp.ColorTextEnable = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonMessageLamp.ColorTextSelected = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonMessageLamp.CurrentTextGroupIndex = 0;
            this.customButtonMessageLamp.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonMessageLamp.CustomButtonData = null;
            this.customButtonMessageLamp.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonMessageLamp.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonMessageLamp.DrawIcon = false;
            this.customButtonMessageLamp.DrawText = false;
            this.customButtonMessageLamp.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonMessageLamp.English_TextDisplay = new string[] {
        ""};
            this.customButtonMessageLamp.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonMessageLamp.English_TextNumberInTextGroup = new int[] {
        0};
            this.customButtonMessageLamp.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(0, 0)};
            this.customButtonMessageLamp.FocusBackgroundDisplay = false;
            this.customButtonMessageLamp.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonMessageLamp.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonMessageLamp.ForeColor = System.Drawing.Color.White;
            this.customButtonMessageLamp.HoverBackgroundDisplay = false;
            this.customButtonMessageLamp.IconIndex = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonMessageLamp.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonMessageLamp.IconNumber = 1;
            this.customButtonMessageLamp.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(0, 0)};
            this.customButtonMessageLamp.LabelControlMode = true;
            this.customButtonMessageLamp.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonMessageLamp.Location = new System.Drawing.Point(594, 10);
            this.customButtonMessageLamp.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonMessageLamp.Name = "customButtonMessageLamp";
            this.customButtonMessageLamp.RectBottom = new System.Drawing.Rectangle(3, 175, 172, 4);
            this.customButtonMessageLamp.RectFill = new System.Drawing.Rectangle(3, 3, 172, 172);
            this.customButtonMessageLamp.RectLeft = new System.Drawing.Rectangle(0, 3, 3, 172);
            this.customButtonMessageLamp.RectLeftBottom = new System.Drawing.Rectangle(0, 175, 3, 4);
            this.customButtonMessageLamp.RectLeftTop = new System.Drawing.Rectangle(0, 0, 3, 3);
            this.customButtonMessageLamp.RectRight = new System.Drawing.Rectangle(175, 3, 4, 172);
            this.customButtonMessageLamp.RectRightBottom = new System.Drawing.Rectangle(175, 175, 4, 4);
            this.customButtonMessageLamp.RectRightTop = new System.Drawing.Rectangle(175, 0, 4, 3);
            this.customButtonMessageLamp.RectTop = new System.Drawing.Rectangle(3, 0, 172, 3);
            this.customButtonMessageLamp.Size = new System.Drawing.Size(32, 32);
            this.customButtonMessageLamp.SizeButton = new System.Drawing.Size(32, 32);
            this.customButtonMessageLamp.TabIndex = 58;
            this.customButtonMessageLamp.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.customButtonMessageLamp.TextGroupNumber = 1;
            this.customButtonMessageLamp.UpdateControl = true;
            // 
            // customButtonMaxValue
            // 
            this.customButtonMaxValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(92)))), ((int)(((byte)(88)))));
            this.customButtonMaxValue.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(92)))), ((int)(((byte)(88)))));
            this.customButtonMaxValue.BitmapIconWhole = null;
            this.customButtonMaxValue.BitmapWhole = null;
            this.customButtonMaxValue.Chinese_TextDisplay = new string[] {
        "最大值 = "};
            this.customButtonMaxValue.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonMaxValue.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonMaxValue.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(61, 15)};
            this.customButtonMaxValue.ColorTextDisable = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
            this.customButtonMaxValue.ColorTextEnable = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonMaxValue.ColorTextSelected = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonMaxValue.CurrentTextGroupIndex = 0;
            this.customButtonMaxValue.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonMaxValue.CustomButtonData = null;
            this.customButtonMaxValue.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonMaxValue.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonMaxValue.DrawIcon = false;
            this.customButtonMaxValue.DrawText = true;
            this.customButtonMaxValue.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonMaxValue.English_TextDisplay = new string[] {
        "Max = "};
            this.customButtonMaxValue.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonMaxValue.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonMaxValue.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(50, 15)};
            this.customButtonMaxValue.FocusBackgroundDisplay = false;
            this.customButtonMaxValue.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonMaxValue.FontText = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonMaxValue.ForeColor = System.Drawing.Color.White;
            this.customButtonMaxValue.HoverBackgroundDisplay = false;
            this.customButtonMaxValue.IconIndex = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonMaxValue.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonMaxValue.IconNumber = 1;
            this.customButtonMaxValue.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(0, 0)};
            this.customButtonMaxValue.LabelControlMode = true;
            this.customButtonMaxValue.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonMaxValue.Location = new System.Drawing.Point(476, 28);
            this.customButtonMaxValue.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonMaxValue.Name = "customButtonMaxValue";
            this.customButtonMaxValue.RectBottom = new System.Drawing.Rectangle(3, 23, 237, 3);
            this.customButtonMaxValue.RectFill = new System.Drawing.Rectangle(3, 3, 237, 20);
            this.customButtonMaxValue.RectLeft = new System.Drawing.Rectangle(0, 3, 3, 20);
            this.customButtonMaxValue.RectLeftBottom = new System.Drawing.Rectangle(0, 23, 3, 3);
            this.customButtonMaxValue.RectLeftTop = new System.Drawing.Rectangle(0, 0, 3, 3);
            this.customButtonMaxValue.RectRight = new System.Drawing.Rectangle(240, 3, 3, 20);
            this.customButtonMaxValue.RectRightBottom = new System.Drawing.Rectangle(240, 23, 3, 3);
            this.customButtonMaxValue.RectRightTop = new System.Drawing.Rectangle(240, 0, 3, 3);
            this.customButtonMaxValue.RectTop = new System.Drawing.Rectangle(3, 0, 237, 3);
            this.customButtonMaxValue.Size = new System.Drawing.Size(110, 15);
            this.customButtonMaxValue.SizeButton = new System.Drawing.Size(110, 15);
            this.customButtonMaxValue.TabIndex = 57;
            this.customButtonMaxValue.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonMaxValue.TextGroupNumber = 1;
            this.customButtonMaxValue.UpdateControl = true;
            // 
            // customButtonCurrentValue
            // 
            this.customButtonCurrentValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(92)))), ((int)(((byte)(88)))));
            this.customButtonCurrentValue.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(92)))), ((int)(((byte)(88)))));
            this.customButtonCurrentValue.BitmapIconWhole = null;
            this.customButtonCurrentValue.BitmapWhole = null;
            this.customButtonCurrentValue.Chinese_TextDisplay = new string[] {
        "当前值 = "};
            this.customButtonCurrentValue.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonCurrentValue.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonCurrentValue.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(61, 15)};
            this.customButtonCurrentValue.ColorTextDisable = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
            this.customButtonCurrentValue.ColorTextEnable = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonCurrentValue.ColorTextSelected = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonCurrentValue.CurrentTextGroupIndex = 0;
            this.customButtonCurrentValue.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonCurrentValue.CustomButtonData = null;
            this.customButtonCurrentValue.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonCurrentValue.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonCurrentValue.DrawIcon = false;
            this.customButtonCurrentValue.DrawText = true;
            this.customButtonCurrentValue.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonCurrentValue.English_TextDisplay = new string[] {
        "Val = "};
            this.customButtonCurrentValue.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonCurrentValue.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonCurrentValue.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(41, 15)};
            this.customButtonCurrentValue.FocusBackgroundDisplay = false;
            this.customButtonCurrentValue.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonCurrentValue.FontText = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonCurrentValue.ForeColor = System.Drawing.Color.White;
            this.customButtonCurrentValue.HoverBackgroundDisplay = false;
            this.customButtonCurrentValue.IconIndex = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonCurrentValue.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonCurrentValue.IconNumber = 1;
            this.customButtonCurrentValue.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(0, 0)};
            this.customButtonCurrentValue.LabelControlMode = true;
            this.customButtonCurrentValue.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonCurrentValue.Location = new System.Drawing.Point(356, 28);
            this.customButtonCurrentValue.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonCurrentValue.Name = "customButtonCurrentValue";
            this.customButtonCurrentValue.RectBottom = new System.Drawing.Rectangle(3, 23, 237, 3);
            this.customButtonCurrentValue.RectFill = new System.Drawing.Rectangle(3, 3, 237, 20);
            this.customButtonCurrentValue.RectLeft = new System.Drawing.Rectangle(0, 3, 3, 20);
            this.customButtonCurrentValue.RectLeftBottom = new System.Drawing.Rectangle(0, 23, 3, 3);
            this.customButtonCurrentValue.RectLeftTop = new System.Drawing.Rectangle(0, 0, 3, 3);
            this.customButtonCurrentValue.RectRight = new System.Drawing.Rectangle(240, 3, 3, 20);
            this.customButtonCurrentValue.RectRightBottom = new System.Drawing.Rectangle(240, 23, 3, 3);
            this.customButtonCurrentValue.RectRightTop = new System.Drawing.Rectangle(240, 0, 3, 3);
            this.customButtonCurrentValue.RectTop = new System.Drawing.Rectangle(3, 0, 237, 3);
            this.customButtonCurrentValue.Size = new System.Drawing.Size(110, 15);
            this.customButtonCurrentValue.SizeButton = new System.Drawing.Size(110, 15);
            this.customButtonCurrentValue.TabIndex = 56;
            this.customButtonCurrentValue.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonCurrentValue.TextGroupNumber = 1;
            this.customButtonCurrentValue.UpdateControl = true;
            // 
            // customButtonMinValue
            // 
            this.customButtonMinValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(92)))), ((int)(((byte)(88)))));
            this.customButtonMinValue.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(92)))), ((int)(((byte)(88)))));
            this.customButtonMinValue.BitmapIconWhole = null;
            this.customButtonMinValue.BitmapWhole = null;
            this.customButtonMinValue.Chinese_TextDisplay = new string[] {
        "最小值 = "};
            this.customButtonMinValue.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonMinValue.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonMinValue.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(61, 15)};
            this.customButtonMinValue.ColorTextDisable = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
            this.customButtonMinValue.ColorTextEnable = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonMinValue.ColorTextSelected = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonMinValue.CurrentTextGroupIndex = 0;
            this.customButtonMinValue.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonMinValue.CustomButtonData = null;
            this.customButtonMinValue.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonMinValue.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonMinValue.DrawIcon = false;
            this.customButtonMinValue.DrawText = true;
            this.customButtonMinValue.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonMinValue.English_TextDisplay = new string[] {
        "Min = "};
            this.customButtonMinValue.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonMinValue.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonMinValue.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(47, 15)};
            this.customButtonMinValue.FocusBackgroundDisplay = false;
            this.customButtonMinValue.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonMinValue.FontText = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonMinValue.ForeColor = System.Drawing.Color.White;
            this.customButtonMinValue.HoverBackgroundDisplay = false;
            this.customButtonMinValue.IconIndex = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonMinValue.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonMinValue.IconNumber = 1;
            this.customButtonMinValue.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(0, 0)};
            this.customButtonMinValue.LabelControlMode = true;
            this.customButtonMinValue.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonMinValue.Location = new System.Drawing.Point(236, 28);
            this.customButtonMinValue.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonMinValue.Name = "customButtonMinValue";
            this.customButtonMinValue.RectBottom = new System.Drawing.Rectangle(3, 23, 237, 3);
            this.customButtonMinValue.RectFill = new System.Drawing.Rectangle(3, 3, 237, 20);
            this.customButtonMinValue.RectLeft = new System.Drawing.Rectangle(0, 3, 3, 20);
            this.customButtonMinValue.RectLeftBottom = new System.Drawing.Rectangle(0, 23, 3, 3);
            this.customButtonMinValue.RectLeftTop = new System.Drawing.Rectangle(0, 0, 3, 3);
            this.customButtonMinValue.RectRight = new System.Drawing.Rectangle(240, 3, 3, 20);
            this.customButtonMinValue.RectRightBottom = new System.Drawing.Rectangle(240, 23, 3, 3);
            this.customButtonMinValue.RectRightTop = new System.Drawing.Rectangle(240, 0, 3, 3);
            this.customButtonMinValue.RectTop = new System.Drawing.Rectangle(3, 0, 237, 3);
            this.customButtonMinValue.Size = new System.Drawing.Size(110, 15);
            this.customButtonMinValue.SizeButton = new System.Drawing.Size(110, 15);
            this.customButtonMinValue.TabIndex = 55;
            this.customButtonMinValue.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonMinValue.TextGroupNumber = 1;
            this.customButtonMinValue.UpdateControl = true;
            // 
            // customListHeaderSlot
            // 
            this.customListHeaderSlot.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(92)))), ((int)(((byte)(88)))));
            this.customListHeaderSlot.BitmapBackgroundIndex = new int[] {
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
            this.customListHeaderSlot.BitmapBackgroundNumber = 3;
            this.customListHeaderSlot.BitmapBackgroundWhole = global::VisionSystemControlLibrary.Properties.Resources.StatusBar;
            this.customListHeaderSlot.BitmapIcon = new System.Drawing.Bitmap[] {
        null};
            this.customListHeaderSlot.BitmapIconWhole = null;
            this.customListHeaderSlot.Chinese_ColumnNameDisplay = new string[] {
        " ",
        " ",
        " ",
        " ",
        " ",
        " ",
        " ",
        " ",
        " ",
        " ",
        " ",
        " "};
            this.customListHeaderSlot.ColorControlBackground = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(92)))), ((int)(((byte)(88)))));
            this.customListHeaderSlot.ColumnNumber = 12;
            this.customListHeaderSlot.ColumnWidth = new int[] {
        18,
        18,
        18,
        18,
        18,
        18,
        18,
        18,
        18,
        18,
        18,
        18};
            this.customListHeaderSlot.CurrentColumnNameGroupIndex = new int[] {
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
            this.customListHeaderSlot.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customListHeaderSlot.English_ColumnNameDisplay = new string[] {
        " ",
        " ",
        " ",
        " ",
        " ",
        " ",
        " ",
        " ",
        " ",
        " ",
        " ",
        " "};
            this.customListHeaderSlot.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customListHeaderSlot.IconIndex = new int[] {
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
            this.customListHeaderSlot.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0),
        new System.Drawing.Point(18, 0),
        new System.Drawing.Point(36, 0),
        new System.Drawing.Point(54, 0),
        new System.Drawing.Point(72, 0),
        new System.Drawing.Point(90, 0),
        new System.Drawing.Point(108, 0),
        new System.Drawing.Point(126, 0),
        new System.Drawing.Point(144, 0),
        new System.Drawing.Point(162, 0),
        new System.Drawing.Point(180, 0),
        new System.Drawing.Point(198, 0)};
            this.customListHeaderSlot.IconNumber = 1;
            this.customListHeaderSlot.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(0, 0),
        new System.Drawing.Size(0, 0),
        new System.Drawing.Size(0, 0),
        new System.Drawing.Size(0, 0),
        new System.Drawing.Size(0, 0),
        new System.Drawing.Size(0, 0),
        new System.Drawing.Size(0, 0),
        new System.Drawing.Size(0, 0),
        new System.Drawing.Size(0, 0),
        new System.Drawing.Size(0, 0),
        new System.Drawing.Size(0, 0),
        new System.Drawing.Size(0, 0)};
            this.customListHeaderSlot.LabelControlMode = true;
            this.customListHeaderSlot.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customListHeaderSlot.ListHeaderEnabled = true;
            this.customListHeaderSlot.Location = new System.Drawing.Point(14, 31);
            this.customListHeaderSlot.Name = "customListHeaderSlot";
            this.customListHeaderSlot.RectBottom = new System.Drawing.Rectangle(3, 175, 172, 3);
            this.customListHeaderSlot.RectFill = new System.Drawing.Rectangle(3, 3, 172, 172);
            this.customListHeaderSlot.RectLeft = new System.Drawing.Rectangle(0, 3, 3, 172);
            this.customListHeaderSlot.RectLeftBottom = new System.Drawing.Rectangle(0, 175, 3, 3);
            this.customListHeaderSlot.RectLeftTop = new System.Drawing.Rectangle(0, 0, 3, 3);
            this.customListHeaderSlot.RectRight = new System.Drawing.Rectangle(175, 3, 3, 172);
            this.customListHeaderSlot.RectRightBottom = new System.Drawing.Rectangle(175, 175, 3, 3);
            this.customListHeaderSlot.RectRightTop = new System.Drawing.Rectangle(175, 0, 3, 3);
            this.customListHeaderSlot.RectTop = new System.Drawing.Rectangle(3, 0, 172, 3);
            this.customListHeaderSlot.Size = new System.Drawing.Size(216, 12);
            this.customListHeaderSlot.SizeListHeader = new System.Drawing.Size(216, 12);
            this.customListHeaderSlot.TabIndex = 54;
            this.customListHeaderSlot.TextAlignment = System.Drawing.StringAlignment.Near;
            this.customListHeaderSlot.XOffSetValue = 7;
            // 
            // customButtonBackground
            // 
            this.customButtonBackground.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(92)))), ((int)(((byte)(88)))));
            this.customButtonBackground.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(92)))), ((int)(((byte)(88)))));
            this.customButtonBackground.BitmapIconWhole = null;
            this.customButtonBackground.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.StatusBarBackground;
            this.customButtonBackground.Chinese_TextDisplay = new string[] {
        "返回"};
            this.customButtonBackground.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 14)};
            this.customButtonBackground.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonBackground.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(39, 23)};
            this.customButtonBackground.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonBackground.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonBackground.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonBackground.CurrentTextGroupIndex = 0;
            this.customButtonBackground.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonBackground.CustomButtonData = null;
            this.customButtonBackground.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonBackground.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonBackground.DrawIcon = true;
            this.customButtonBackground.DrawText = false;
            this.customButtonBackground.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonBackground.English_TextDisplay = new string[] {
        "BACK"};
            this.customButtonBackground.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 14)};
            this.customButtonBackground.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonBackground.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(52, 23)};
            this.customButtonBackground.FocusBackgroundDisplay = false;
            this.customButtonBackground.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonBackground.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonBackground.ForeColor = System.Drawing.Color.White;
            this.customButtonBackground.HoverBackgroundDisplay = false;
            this.customButtonBackground.IconIndex = new int[] {
        1,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonBackground.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonBackground.IconNumber = 1;
            this.customButtonBackground.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(0, 0)};
            this.customButtonBackground.LabelControlMode = true;
            this.customButtonBackground.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonBackground.Location = new System.Drawing.Point(0, 0);
            this.customButtonBackground.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonBackground.Name = "customButtonBackground";
            this.customButtonBackground.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonBackground.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonBackground.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonBackground.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonBackground.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonBackground.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonBackground.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonBackground.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonBackground.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonBackground.Size = new System.Drawing.Size(640, 50);
            this.customButtonBackground.SizeButton = new System.Drawing.Size(640, 50);
            this.customButtonBackground.TabIndex = 36;
            this.customButtonBackground.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonBackground.TextGroupNumber = 1;
            this.customButtonBackground.UpdateControl = true;
            // 
            // StatusBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(92)))), ((int)(((byte)(88)))));
            this.Controls.Add(this.customButtonMessageLamp);
            this.Controls.Add(this.customButtonMaxValue);
            this.Controls.Add(this.customButtonCurrentValue);
            this.Controls.Add(this.customButtonMinValue);
            this.Controls.Add(this.customListHeaderSlot);
            this.Controls.Add(this.labelMessage);
            this.Controls.Add(this.customButtonBackground);
            this.Name = "StatusBar";
            this.Size = new System.Drawing.Size(640, 50);
            this.ResumeLayout(false);

        }

        #endregion

        private CustomButton customButtonBackground;
        private System.Windows.Forms.Label labelMessage;
        private CustomListHeader customListHeaderSlot;
        private CustomButton customButtonMinValue;
        private CustomButton customButtonCurrentValue;
        private CustomButton customButtonMaxValue;
        private CustomButton customButtonMessageLamp;


    }
}
