namespace VisionSystemControlLibrary
{
    partial class SystemControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SystemControl));
            this.timerPCTime = new System.Windows.Forms.Timer(this.components);
            this.customButtonNextPage_ValueList = new VisionSystemControlLibrary.CustomButton();
            this.customButtonPreviousPage_ValueList = new VisionSystemControlLibrary.CustomButton();
            this.customButtonNextPage_ParameterList = new VisionSystemControlLibrary.CustomButton();
            this.customButtonPreviousPage_ParameterList = new VisionSystemControlLibrary.CustomButton();
            this.customButtonCancel = new VisionSystemControlLibrary.CustomButton();
            this.customButtonOk = new VisionSystemControlLibrary.CustomButton();
            this.customButtonValueList = new VisionSystemControlLibrary.CustomButton();
            this.customButtonParameterList = new VisionSystemControlLibrary.CustomButton();
            this.customButtonClose = new VisionSystemControlLibrary.CustomButton();
            this.customButtonCaption = new VisionSystemControlLibrary.CustomButton();
            this.customListValueList = new VisionSystemControlLibrary.CustomList();
            this.customListParameterList = new VisionSystemControlLibrary.CustomList();
            this.timerCommonParameter = new System.Windows.Forms.Timer(this.components);
            this.labelIcon = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // timerPCTime
            // 
            this.timerPCTime.Interval = 1000;
            this.timerPCTime.Tick += new System.EventHandler(this.timerPCTime_Tick);
            // 
            // customButtonNextPage_ValueList
            // 
            this.customButtonNextPage_ValueList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonNextPage_ValueList.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonNextPage_ValueList.BitmapIconWhole = global::VisionSystemControlLibrary.Properties.Resources.Down;
            this.customButtonNextPage_ValueList.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.Button;
            this.customButtonNextPage_ValueList.Chinese_TextDisplay = new string[] {
        "下一页"};
            this.customButtonNextPage_ValueList.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 5)};
            this.customButtonNextPage_ValueList.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonNextPage_ValueList.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(55, 23)};
            this.customButtonNextPage_ValueList.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonNextPage_ValueList.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonNextPage_ValueList.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonNextPage_ValueList.CurrentTextGroupIndex = 0;
            this.customButtonNextPage_ValueList.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonNextPage_ValueList.CustomButtonData = null;
            this.customButtonNextPage_ValueList.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonNextPage_ValueList.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonNextPage_ValueList.DrawIcon = true;
            this.customButtonNextPage_ValueList.DrawText = false;
            this.customButtonNextPage_ValueList.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonNextPage_ValueList.English_TextDisplay = new string[] {
        "NEXT&PAGE"};
            this.customButtonNextPage_ValueList.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 5),
        new System.Drawing.Point(5, 25)};
            this.customButtonNextPage_ValueList.English_TextNumberInTextGroup = new int[] {
        2};
            this.customButtonNextPage_ValueList.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(51, 23),
        new System.Drawing.Size(51, 23)};
            this.customButtonNextPage_ValueList.FocusBackgroundDisplay = false;
            this.customButtonNextPage_ValueList.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonNextPage_ValueList.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonNextPage_ValueList.ForeColor = System.Drawing.Color.White;
            this.customButtonNextPage_ValueList.HoverBackgroundDisplay = false;
            this.customButtonNextPage_ValueList.IconIndex = new int[] {
        1,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonNextPage_ValueList.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(8, 12),
        new System.Drawing.Point(8, 12)};
            this.customButtonNextPage_ValueList.IconNumber = 2;
            this.customButtonNextPage_ValueList.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(48, 70),
        new System.Drawing.Size(48, 70)};
            this.customButtonNextPage_ValueList.LabelControlMode = false;
            this.customButtonNextPage_ValueList.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonNextPage_ValueList.Location = new System.Drawing.Point(928, 557);
            this.customButtonNextPage_ValueList.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonNextPage_ValueList.Name = "customButtonNextPage_ValueList";
            this.customButtonNextPage_ValueList.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonNextPage_ValueList.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonNextPage_ValueList.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonNextPage_ValueList.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonNextPage_ValueList.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonNextPage_ValueList.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonNextPage_ValueList.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonNextPage_ValueList.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonNextPage_ValueList.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonNextPage_ValueList.Size = new System.Drawing.Size(63, 97);
            this.customButtonNextPage_ValueList.SizeButton = new System.Drawing.Size(63, 97);
            this.customButtonNextPage_ValueList.TabIndex = 48;
            this.customButtonNextPage_ValueList.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonNextPage_ValueList.TextGroupNumber = 1;
            this.customButtonNextPage_ValueList.UpdateControl = true;
            this.customButtonNextPage_ValueList.Visible = false;
            this.customButtonNextPage_ValueList.CustomButton_Click += new System.EventHandler(this.customButtonNextPage_ValueList_CustomButton_Click);
            // 
            // customButtonPreviousPage_ValueList
            // 
            this.customButtonPreviousPage_ValueList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonPreviousPage_ValueList.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonPreviousPage_ValueList.BitmapIconWhole = global::VisionSystemControlLibrary.Properties.Resources.Up;
            this.customButtonPreviousPage_ValueList.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.Button;
            this.customButtonPreviousPage_ValueList.Chinese_TextDisplay = new string[] {
        "上一页"};
            this.customButtonPreviousPage_ValueList.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 5)};
            this.customButtonPreviousPage_ValueList.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonPreviousPage_ValueList.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(55, 23)};
            this.customButtonPreviousPage_ValueList.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonPreviousPage_ValueList.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonPreviousPage_ValueList.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonPreviousPage_ValueList.CurrentTextGroupIndex = 0;
            this.customButtonPreviousPage_ValueList.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonPreviousPage_ValueList.CustomButtonData = null;
            this.customButtonPreviousPage_ValueList.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonPreviousPage_ValueList.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonPreviousPage_ValueList.DrawIcon = true;
            this.customButtonPreviousPage_ValueList.DrawText = false;
            this.customButtonPreviousPage_ValueList.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonPreviousPage_ValueList.English_TextDisplay = new string[] {
        "PREV.&PAGE"};
            this.customButtonPreviousPage_ValueList.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 5),
        new System.Drawing.Point(5, 25)};
            this.customButtonPreviousPage_ValueList.English_TextNumberInTextGroup = new int[] {
        2};
            this.customButtonPreviousPage_ValueList.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(54, 23),
        new System.Drawing.Size(51, 23)};
            this.customButtonPreviousPage_ValueList.FocusBackgroundDisplay = false;
            this.customButtonPreviousPage_ValueList.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonPreviousPage_ValueList.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonPreviousPage_ValueList.ForeColor = System.Drawing.Color.White;
            this.customButtonPreviousPage_ValueList.HoverBackgroundDisplay = false;
            this.customButtonPreviousPage_ValueList.IconIndex = new int[] {
        1,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonPreviousPage_ValueList.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(8, 12),
        new System.Drawing.Point(8, 12)};
            this.customButtonPreviousPage_ValueList.IconNumber = 2;
            this.customButtonPreviousPage_ValueList.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(48, 70),
        new System.Drawing.Size(48, 70)};
            this.customButtonPreviousPage_ValueList.LabelControlMode = false;
            this.customButtonPreviousPage_ValueList.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonPreviousPage_ValueList.Location = new System.Drawing.Point(862, 557);
            this.customButtonPreviousPage_ValueList.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonPreviousPage_ValueList.Name = "customButtonPreviousPage_ValueList";
            this.customButtonPreviousPage_ValueList.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonPreviousPage_ValueList.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonPreviousPage_ValueList.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonPreviousPage_ValueList.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonPreviousPage_ValueList.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonPreviousPage_ValueList.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonPreviousPage_ValueList.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonPreviousPage_ValueList.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonPreviousPage_ValueList.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonPreviousPage_ValueList.Size = new System.Drawing.Size(63, 97);
            this.customButtonPreviousPage_ValueList.SizeButton = new System.Drawing.Size(63, 97);
            this.customButtonPreviousPage_ValueList.TabIndex = 47;
            this.customButtonPreviousPage_ValueList.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonPreviousPage_ValueList.TextGroupNumber = 1;
            this.customButtonPreviousPage_ValueList.UpdateControl = true;
            this.customButtonPreviousPage_ValueList.Visible = false;
            this.customButtonPreviousPage_ValueList.CustomButton_Click += new System.EventHandler(this.customButtonPreviousPage_ValueList_CustomButton_Click);
            // 
            // customButtonNextPage_ParameterList
            // 
            this.customButtonNextPage_ParameterList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonNextPage_ParameterList.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonNextPage_ParameterList.BitmapIconWhole = global::VisionSystemControlLibrary.Properties.Resources.Down;
            this.customButtonNextPage_ParameterList.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.Button;
            this.customButtonNextPage_ParameterList.Chinese_TextDisplay = new string[] {
        "下一页"};
            this.customButtonNextPage_ParameterList.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 5)};
            this.customButtonNextPage_ParameterList.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonNextPage_ParameterList.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(55, 23)};
            this.customButtonNextPage_ParameterList.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonNextPage_ParameterList.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonNextPage_ParameterList.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonNextPage_ParameterList.CurrentTextGroupIndex = 0;
            this.customButtonNextPage_ParameterList.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonNextPage_ParameterList.CustomButtonData = null;
            this.customButtonNextPage_ParameterList.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonNextPage_ParameterList.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonNextPage_ParameterList.DrawIcon = true;
            this.customButtonNextPage_ParameterList.DrawText = false;
            this.customButtonNextPage_ParameterList.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonNextPage_ParameterList.English_TextDisplay = new string[] {
        "NEXT&PAGE"};
            this.customButtonNextPage_ParameterList.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 5),
        new System.Drawing.Point(5, 25)};
            this.customButtonNextPage_ParameterList.English_TextNumberInTextGroup = new int[] {
        2};
            this.customButtonNextPage_ParameterList.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(51, 23),
        new System.Drawing.Size(51, 23)};
            this.customButtonNextPage_ParameterList.FocusBackgroundDisplay = false;
            this.customButtonNextPage_ParameterList.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonNextPage_ParameterList.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonNextPage_ParameterList.ForeColor = System.Drawing.Color.White;
            this.customButtonNextPage_ParameterList.HoverBackgroundDisplay = false;
            this.customButtonNextPage_ParameterList.IconIndex = new int[] {
        1,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonNextPage_ParameterList.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(8, 12),
        new System.Drawing.Point(8, 12)};
            this.customButtonNextPage_ParameterList.IconNumber = 2;
            this.customButtonNextPage_ParameterList.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(48, 70),
        new System.Drawing.Size(48, 70)};
            this.customButtonNextPage_ParameterList.LabelControlMode = false;
            this.customButtonNextPage_ParameterList.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonNextPage_ParameterList.Location = new System.Drawing.Point(422, 557);
            this.customButtonNextPage_ParameterList.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonNextPage_ParameterList.Name = "customButtonNextPage_ParameterList";
            this.customButtonNextPage_ParameterList.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonNextPage_ParameterList.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonNextPage_ParameterList.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonNextPage_ParameterList.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonNextPage_ParameterList.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonNextPage_ParameterList.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonNextPage_ParameterList.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonNextPage_ParameterList.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonNextPage_ParameterList.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonNextPage_ParameterList.Size = new System.Drawing.Size(63, 97);
            this.customButtonNextPage_ParameterList.SizeButton = new System.Drawing.Size(63, 97);
            this.customButtonNextPage_ParameterList.TabIndex = 46;
            this.customButtonNextPage_ParameterList.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonNextPage_ParameterList.TextGroupNumber = 1;
            this.customButtonNextPage_ParameterList.UpdateControl = true;
            this.customButtonNextPage_ParameterList.Visible = false;
            this.customButtonNextPage_ParameterList.CustomButton_Click += new System.EventHandler(this.customButtonNextPage_ParameterList_CustomButton_Click);
            // 
            // customButtonPreviousPage_ParameterList
            // 
            this.customButtonPreviousPage_ParameterList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonPreviousPage_ParameterList.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonPreviousPage_ParameterList.BitmapIconWhole = global::VisionSystemControlLibrary.Properties.Resources.Up;
            this.customButtonPreviousPage_ParameterList.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.Button;
            this.customButtonPreviousPage_ParameterList.Chinese_TextDisplay = new string[] {
        "上一页"};
            this.customButtonPreviousPage_ParameterList.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 5)};
            this.customButtonPreviousPage_ParameterList.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonPreviousPage_ParameterList.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(55, 23)};
            this.customButtonPreviousPage_ParameterList.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonPreviousPage_ParameterList.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonPreviousPage_ParameterList.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonPreviousPage_ParameterList.CurrentTextGroupIndex = 0;
            this.customButtonPreviousPage_ParameterList.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonPreviousPage_ParameterList.CustomButtonData = null;
            this.customButtonPreviousPage_ParameterList.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonPreviousPage_ParameterList.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonPreviousPage_ParameterList.DrawIcon = true;
            this.customButtonPreviousPage_ParameterList.DrawText = false;
            this.customButtonPreviousPage_ParameterList.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonPreviousPage_ParameterList.English_TextDisplay = new string[] {
        "PREV.&PAGE"};
            this.customButtonPreviousPage_ParameterList.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 5),
        new System.Drawing.Point(5, 25)};
            this.customButtonPreviousPage_ParameterList.English_TextNumberInTextGroup = new int[] {
        2};
            this.customButtonPreviousPage_ParameterList.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(54, 23),
        new System.Drawing.Size(51, 23)};
            this.customButtonPreviousPage_ParameterList.FocusBackgroundDisplay = false;
            this.customButtonPreviousPage_ParameterList.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonPreviousPage_ParameterList.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonPreviousPage_ParameterList.ForeColor = System.Drawing.Color.White;
            this.customButtonPreviousPage_ParameterList.HoverBackgroundDisplay = false;
            this.customButtonPreviousPage_ParameterList.IconIndex = new int[] {
        1,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonPreviousPage_ParameterList.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(8, 12),
        new System.Drawing.Point(8, 12)};
            this.customButtonPreviousPage_ParameterList.IconNumber = 2;
            this.customButtonPreviousPage_ParameterList.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(48, 70),
        new System.Drawing.Size(48, 70)};
            this.customButtonPreviousPage_ParameterList.LabelControlMode = false;
            this.customButtonPreviousPage_ParameterList.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonPreviousPage_ParameterList.Location = new System.Drawing.Point(356, 557);
            this.customButtonPreviousPage_ParameterList.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonPreviousPage_ParameterList.Name = "customButtonPreviousPage_ParameterList";
            this.customButtonPreviousPage_ParameterList.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonPreviousPage_ParameterList.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonPreviousPage_ParameterList.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonPreviousPage_ParameterList.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonPreviousPage_ParameterList.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonPreviousPage_ParameterList.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonPreviousPage_ParameterList.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonPreviousPage_ParameterList.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonPreviousPage_ParameterList.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonPreviousPage_ParameterList.Size = new System.Drawing.Size(63, 97);
            this.customButtonPreviousPage_ParameterList.SizeButton = new System.Drawing.Size(63, 97);
            this.customButtonPreviousPage_ParameterList.TabIndex = 45;
            this.customButtonPreviousPage_ParameterList.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonPreviousPage_ParameterList.TextGroupNumber = 1;
            this.customButtonPreviousPage_ParameterList.UpdateControl = true;
            this.customButtonPreviousPage_ParameterList.Visible = false;
            this.customButtonPreviousPage_ParameterList.CustomButton_Click += new System.EventHandler(this.customButtonPreviousPage_ParameterList_CustomButton_Click);
            // 
            // customButtonCancel
            // 
            this.customButtonCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonCancel.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonCancel.BitmapIconWhole = global::VisionSystemControlLibrary.Properties.Resources.Cancel;
            this.customButtonCancel.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.Button;
            this.customButtonCancel.Chinese_TextDisplay = new string[] {
        "确定"};
            this.customButtonCancel.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 10)};
            this.customButtonCancel.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonCancel.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(39, 23)};
            this.customButtonCancel.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonCancel.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonCancel.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonCancel.CurrentTextGroupIndex = 0;
            this.customButtonCancel.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonCancel.CustomButtonData = null;
            this.customButtonCancel.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonCancel.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonCancel.DrawIcon = true;
            this.customButtonCancel.DrawText = false;
            this.customButtonCancel.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonCancel.English_TextDisplay = new string[] {
        "OK"};
            this.customButtonCancel.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 10)};
            this.customButtonCancel.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonCancel.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(52, 23)};
            this.customButtonCancel.FocusBackgroundDisplay = false;
            this.customButtonCancel.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonCancel.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonCancel.ForeColor = System.Drawing.Color.White;
            this.customButtonCancel.HoverBackgroundDisplay = false;
            this.customButtonCancel.IconIndex = new int[] {
        1,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonCancel.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(47, 10),
        new System.Drawing.Point(47, 10)};
            this.customButtonCancel.IconNumber = 2;
            this.customButtonCancel.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(53, 37),
        new System.Drawing.Size(53, 37)};
            this.customButtonCancel.LabelControlMode = false;
            this.customButtonCancel.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonCancel.Location = new System.Drawing.Point(171, 581);
            this.customButtonCancel.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonCancel.Name = "customButtonCancel";
            this.customButtonCancel.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonCancel.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonCancel.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonCancel.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonCancel.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonCancel.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonCancel.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonCancel.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonCancel.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonCancel.Size = new System.Drawing.Size(131, 57);
            this.customButtonCancel.SizeButton = new System.Drawing.Size(131, 57);
            this.customButtonCancel.TabIndex = 44;
            this.customButtonCancel.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonCancel.TextGroupNumber = 1;
            this.customButtonCancel.UpdateControl = true;
            this.customButtonCancel.Visible = false;
            this.customButtonCancel.CustomButton_Click += new System.EventHandler(this.customButtonCancel_CustomButton_Click);
            // 
            // customButtonOk
            // 
            this.customButtonOk.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonOk.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonOk.BitmapIconWhole = global::VisionSystemControlLibrary.Properties.Resources.OK;
            this.customButtonOk.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.Button;
            this.customButtonOk.Chinese_TextDisplay = new string[] {
        "确定"};
            this.customButtonOk.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 10)};
            this.customButtonOk.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonOk.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(39, 23)};
            this.customButtonOk.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonOk.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonOk.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonOk.CurrentTextGroupIndex = 0;
            this.customButtonOk.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonOk.CustomButtonData = null;
            this.customButtonOk.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonOk.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonOk.DrawIcon = true;
            this.customButtonOk.DrawText = false;
            this.customButtonOk.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonOk.English_TextDisplay = new string[] {
        "OK"};
            this.customButtonOk.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 10)};
            this.customButtonOk.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonOk.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(52, 23)};
            this.customButtonOk.FocusBackgroundDisplay = false;
            this.customButtonOk.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonOk.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonOk.ForeColor = System.Drawing.Color.White;
            this.customButtonOk.HoverBackgroundDisplay = false;
            this.customButtonOk.IconIndex = new int[] {
        1,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonOk.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(40, 10),
        new System.Drawing.Point(40, 10)};
            this.customButtonOk.IconNumber = 2;
            this.customButtonOk.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(53, 37),
        new System.Drawing.Size(53, 37)};
            this.customButtonOk.LabelControlMode = false;
            this.customButtonOk.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonOk.Location = new System.Drawing.Point(30, 581);
            this.customButtonOk.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonOk.Name = "customButtonOk";
            this.customButtonOk.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonOk.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonOk.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonOk.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonOk.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonOk.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonOk.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonOk.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonOk.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonOk.Size = new System.Drawing.Size(131, 57);
            this.customButtonOk.SizeButton = new System.Drawing.Size(131, 57);
            this.customButtonOk.TabIndex = 43;
            this.customButtonOk.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonOk.TextGroupNumber = 1;
            this.customButtonOk.UpdateControl = true;
            this.customButtonOk.Visible = false;
            this.customButtonOk.CustomButton_Click += new System.EventHandler(this.customButtonOk_CustomButton_Click);
            // 
            // customButtonValueList
            // 
            this.customButtonValueList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.customButtonValueList.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.customButtonValueList.BitmapIconWhole = null;
            this.customButtonValueList.BitmapWhole = null;
            this.customButtonValueList.Chinese_TextDisplay = new string[] {
        "数值列表"};
            this.customButtonValueList.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 3)};
            this.customButtonValueList.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonValueList.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(72, 23)};
            this.customButtonValueList.ColorTextDisable = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
            this.customButtonValueList.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonValueList.ColorTextSelected = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonValueList.CurrentTextGroupIndex = 0;
            this.customButtonValueList.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonValueList.CustomButtonData = null;
            this.customButtonValueList.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonValueList.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonValueList.DrawIcon = false;
            this.customButtonValueList.DrawText = true;
            this.customButtonValueList.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonValueList.English_TextDisplay = new string[] {
        "Value List"};
            this.customButtonValueList.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 3)};
            this.customButtonValueList.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonValueList.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(86, 23)};
            this.customButtonValueList.FocusBackgroundDisplay = false;
            this.customButtonValueList.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonValueList.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonValueList.ForeColor = System.Drawing.Color.White;
            this.customButtonValueList.HoverBackgroundDisplay = false;
            this.customButtonValueList.IconIndex = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonValueList.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonValueList.IconNumber = 1;
            this.customButtonValueList.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(0, 0)};
            this.customButtonValueList.LabelControlMode = true;
            this.customButtonValueList.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonValueList.Location = new System.Drawing.Point(523, 81);
            this.customButtonValueList.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonValueList.Name = "customButtonValueList";
            this.customButtonValueList.RectBottom = new System.Drawing.Rectangle(3, 23, 237, 3);
            this.customButtonValueList.RectFill = new System.Drawing.Rectangle(3, 3, 237, 20);
            this.customButtonValueList.RectLeft = new System.Drawing.Rectangle(0, 3, 3, 20);
            this.customButtonValueList.RectLeftBottom = new System.Drawing.Rectangle(0, 23, 3, 3);
            this.customButtonValueList.RectLeftTop = new System.Drawing.Rectangle(0, 0, 3, 3);
            this.customButtonValueList.RectRight = new System.Drawing.Rectangle(240, 3, 3, 20);
            this.customButtonValueList.RectRightBottom = new System.Drawing.Rectangle(240, 23, 3, 3);
            this.customButtonValueList.RectRightTop = new System.Drawing.Rectangle(240, 0, 3, 3);
            this.customButtonValueList.RectTop = new System.Drawing.Rectangle(3, 0, 237, 3);
            this.customButtonValueList.Size = new System.Drawing.Size(182, 30);
            this.customButtonValueList.SizeButton = new System.Drawing.Size(182, 30);
            this.customButtonValueList.TabIndex = 42;
            this.customButtonValueList.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonValueList.TextGroupNumber = 1;
            this.customButtonValueList.UpdateControl = true;
            // 
            // customButtonParameterList
            // 
            this.customButtonParameterList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.customButtonParameterList.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.customButtonParameterList.BitmapIconWhole = null;
            this.customButtonParameterList.BitmapWhole = null;
            this.customButtonParameterList.Chinese_TextDisplay = new string[] {
        "参数列表"};
            this.customButtonParameterList.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 3)};
            this.customButtonParameterList.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonParameterList.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(72, 23)};
            this.customButtonParameterList.ColorTextDisable = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
            this.customButtonParameterList.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonParameterList.ColorTextSelected = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonParameterList.CurrentTextGroupIndex = 0;
            this.customButtonParameterList.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonParameterList.CustomButtonData = null;
            this.customButtonParameterList.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonParameterList.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonParameterList.DrawIcon = false;
            this.customButtonParameterList.DrawText = true;
            this.customButtonParameterList.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonParameterList.English_TextDisplay = new string[] {
        "Parameter List"};
            this.customButtonParameterList.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 3)};
            this.customButtonParameterList.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonParameterList.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(126, 23)};
            this.customButtonParameterList.FocusBackgroundDisplay = false;
            this.customButtonParameterList.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonParameterList.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonParameterList.ForeColor = System.Drawing.Color.White;
            this.customButtonParameterList.HoverBackgroundDisplay = false;
            this.customButtonParameterList.IconIndex = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonParameterList.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonParameterList.IconNumber = 1;
            this.customButtonParameterList.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(0, 0)};
            this.customButtonParameterList.LabelControlMode = true;
            this.customButtonParameterList.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonParameterList.Location = new System.Drawing.Point(26, 81);
            this.customButtonParameterList.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonParameterList.Name = "customButtonParameterList";
            this.customButtonParameterList.RectBottom = new System.Drawing.Rectangle(3, 23, 237, 3);
            this.customButtonParameterList.RectFill = new System.Drawing.Rectangle(3, 3, 237, 20);
            this.customButtonParameterList.RectLeft = new System.Drawing.Rectangle(0, 3, 3, 20);
            this.customButtonParameterList.RectLeftBottom = new System.Drawing.Rectangle(0, 23, 3, 3);
            this.customButtonParameterList.RectLeftTop = new System.Drawing.Rectangle(0, 0, 3, 3);
            this.customButtonParameterList.RectRight = new System.Drawing.Rectangle(240, 3, 3, 20);
            this.customButtonParameterList.RectRightBottom = new System.Drawing.Rectangle(240, 23, 3, 3);
            this.customButtonParameterList.RectRightTop = new System.Drawing.Rectangle(240, 0, 3, 3);
            this.customButtonParameterList.RectTop = new System.Drawing.Rectangle(3, 0, 237, 3);
            this.customButtonParameterList.Size = new System.Drawing.Size(182, 30);
            this.customButtonParameterList.SizeButton = new System.Drawing.Size(182, 30);
            this.customButtonParameterList.TabIndex = 41;
            this.customButtonParameterList.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonParameterList.TextGroupNumber = 1;
            this.customButtonParameterList.UpdateControl = true;
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
            this.customButtonClose.TabIndex = 40;
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
        "视觉系统配置"};
            this.customButtonCaption.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(322, 7)};
            this.customButtonCaption.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonCaption.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(131, 28)};
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
        "VISION SYSTEM CONFIGURATION"};
            this.customButtonCaption.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(207, 7)};
            this.customButtonCaption.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonCaption.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(361, 28)};
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
            this.customButtonCaption.TabIndex = 39;
            this.customButtonCaption.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.customButtonCaption.TextGroupNumber = 1;
            this.customButtonCaption.UpdateControl = true;
            // 
            // customListValueList
            // 
            this.customListValueList.BackColor = System.Drawing.Color.Black;
            this.customListValueList.BitmapBackgroundIndex = new int[] {
        0,
        0};
            this.customListValueList.BitmapBackgroundNumber = 1;
            this.customListValueList.BitmapBackgroundWhole = ((System.Drawing.Bitmap)(resources.GetObject("customListValueList.BitmapBackgroundWhole")));
            this.customListValueList.BitmapIcon = new System.Drawing.Bitmap[] {
        null,
        null,
        null,
        null,
        null};
            this.customListValueList.Chinese_ColumnNameDisplay = new string[] {
        "名称",
        "数值"};
            this.customListValueList.ColorControlBackground = System.Drawing.Color.Black;
            this.customListValueList.ColorItemBackgroundSelected = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.customListValueList.ColorItemBackgroundUnselected = System.Drawing.Color.Black;
            this.customListValueList.ColorItemTextSelected = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customListValueList.ColorItemTextUnselected = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customListValueList.ColorListHeaderBackground = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.customListValueList.ColorListHeaderColumnText_Disable = new System.Drawing.Color[] {
        System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))))};
            this.customListValueList.ColorListHeaderColumnText_Enable = new System.Drawing.Color[] {
        System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))))};
            this.customListValueList.ColorListItemBackground = System.Drawing.Color.Black;
            this.customListValueList.ColorPageBackground = System.Drawing.Color.Black;
            this.customListValueList.ColorPageText = System.Drawing.Color.Yellow;
            this.customListValueList.ColumnNameXOffSetValue = 7;
            this.customListValueList.ColumnNumber = 2;
            this.customListValueList.ColumnWidth = new int[] {
        376,
        77};
            this.customListValueList.CurrentColumnNameGroupIndex = new int[] {
        0,
        0};
            this.customListValueList.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customListValueList.English_ColumnNameDisplay = new string[] {
        "Name",
        "Value"};
            this.customListValueList.FontListHeader = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customListValueList.FontListItem = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customListValueList.FontPage = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customListValueList.ForeColor = System.Drawing.Color.White;
            this.customListValueList.ItemDataDisplay = new bool[] {
        true,
        true};
            this.customListValueList.ItemDataNumber = 0;
            this.customListValueList.ItemIconIndex = new int[] {
        -1,
        -1};
            this.customListValueList.ItemIconNumber = 5;
            this.customListValueList.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customListValueList.ListEnabled = true;
            this.customListValueList.ListHeaderHeight = 36;
            this.customListValueList.ListHeaderTextAlignment = System.Drawing.StringAlignment.Near;
            this.customListValueList.ListItemHeight = 35;
            this.customListValueList.ListItemTextAlignment = System.Drawing.StringAlignment.Near;
            this.customListValueList.ListItemXOffSetValue = 7;
            this.customListValueList.Location = new System.Drawing.Point(527, 114);
            this.customListValueList.Name = "customListValueList";
            this.customListValueList.PageHeight = 25;
            this.customListValueList.RectBottom = new System.Drawing.Rectangle(3, 23, 237, 3);
            this.customListValueList.RectFill = new System.Drawing.Rectangle(3, 3, 237, 20);
            this.customListValueList.RectLeft = new System.Drawing.Rectangle(0, 3, 3, 20);
            this.customListValueList.RectLeftBottom = new System.Drawing.Rectangle(0, 23, 3, 3);
            this.customListValueList.RectLeftTop = new System.Drawing.Rectangle(0, 0, 3, 3);
            this.customListValueList.RectRight = new System.Drawing.Rectangle(240, 3, 3, 20);
            this.customListValueList.RectRightBottom = new System.Drawing.Rectangle(240, 23, 3, 3);
            this.customListValueList.RectRightTop = new System.Drawing.Rectangle(240, 0, 3, 3);
            this.customListValueList.RectTop = new System.Drawing.Rectangle(3, 0, 237, 3);
            this.customListValueList.SelectedItemNumber = 0;
            this.customListValueList.SelectedItemType = false;
            this.customListValueList.SelectionColumnIndex = -1;
            this.customListValueList.Size = new System.Drawing.Size(464, 434);
            this.customListValueList.SizeControl = new System.Drawing.Size(464, 434);
            this.customListValueList.SizeItemIcon = new System.Drawing.Size[] {
        new System.Drawing.Size(20, 20),
        new System.Drawing.Size(20, 20)};
            this.customListValueList.TabIndex = 3;
            this.customListValueList.CustomListItem_Click += new System.EventHandler(this.customListValueList_CustomListItem_Click);
            // 
            // customListParameterList
            // 
            this.customListParameterList.BackColor = System.Drawing.Color.Black;
            this.customListParameterList.BitmapBackgroundIndex = new int[] {
        0,
        0};
            this.customListParameterList.BitmapBackgroundNumber = 1;
            this.customListParameterList.BitmapBackgroundWhole = ((System.Drawing.Bitmap)(resources.GetObject("customListParameterList.BitmapBackgroundWhole")));
            this.customListParameterList.BitmapIcon = new System.Drawing.Bitmap[] {
        null,
        null,
        null,
        null,
        null};
            this.customListParameterList.Chinese_ColumnNameDisplay = new string[] {
        "名称",
        "数值"};
            this.customListParameterList.ColorControlBackground = System.Drawing.Color.Black;
            this.customListParameterList.ColorItemBackgroundSelected = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.customListParameterList.ColorItemBackgroundUnselected = System.Drawing.Color.Black;
            this.customListParameterList.ColorItemTextSelected = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customListParameterList.ColorItemTextUnselected = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customListParameterList.ColorListHeaderBackground = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.customListParameterList.ColorListHeaderColumnText_Disable = new System.Drawing.Color[] {
        System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))))};
            this.customListParameterList.ColorListHeaderColumnText_Enable = new System.Drawing.Color[] {
        System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))))};
            this.customListParameterList.ColorListItemBackground = System.Drawing.Color.Black;
            this.customListParameterList.ColorPageBackground = System.Drawing.Color.Black;
            this.customListParameterList.ColorPageText = System.Drawing.Color.Yellow;
            this.customListParameterList.ColumnNameXOffSetValue = 7;
            this.customListParameterList.ColumnNumber = 2;
            this.customListParameterList.ColumnWidth = new int[] {
        155,
        289};
            this.customListParameterList.CurrentColumnNameGroupIndex = new int[] {
        0,
        0};
            this.customListParameterList.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customListParameterList.English_ColumnNameDisplay = new string[] {
        "Name",
        "Value"};
            this.customListParameterList.FontListHeader = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customListParameterList.FontListItem = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customListParameterList.FontPage = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customListParameterList.ForeColor = System.Drawing.Color.White;
            this.customListParameterList.ItemDataDisplay = new bool[] {
        true,
        true};
            this.customListParameterList.ItemDataNumber = 0;
            this.customListParameterList.ItemIconIndex = new int[] {
        -1,
        -1};
            this.customListParameterList.ItemIconNumber = 5;
            this.customListParameterList.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customListParameterList.ListEnabled = true;
            this.customListParameterList.ListHeaderHeight = 36;
            this.customListParameterList.ListHeaderTextAlignment = System.Drawing.StringAlignment.Near;
            this.customListParameterList.ListItemHeight = 35;
            this.customListParameterList.ListItemTextAlignment = System.Drawing.StringAlignment.Near;
            this.customListParameterList.ListItemXOffSetValue = 7;
            this.customListParameterList.Location = new System.Drawing.Point(30, 114);
            this.customListParameterList.Name = "customListParameterList";
            this.customListParameterList.PageHeight = 25;
            this.customListParameterList.RectBottom = new System.Drawing.Rectangle(3, 23, 237, 3);
            this.customListParameterList.RectFill = new System.Drawing.Rectangle(3, 3, 237, 20);
            this.customListParameterList.RectLeft = new System.Drawing.Rectangle(0, 3, 3, 20);
            this.customListParameterList.RectLeftBottom = new System.Drawing.Rectangle(0, 23, 3, 3);
            this.customListParameterList.RectLeftTop = new System.Drawing.Rectangle(0, 0, 3, 3);
            this.customListParameterList.RectRight = new System.Drawing.Rectangle(240, 3, 3, 20);
            this.customListParameterList.RectRightBottom = new System.Drawing.Rectangle(240, 23, 3, 3);
            this.customListParameterList.RectRightTop = new System.Drawing.Rectangle(240, 0, 3, 3);
            this.customListParameterList.RectTop = new System.Drawing.Rectangle(3, 0, 237, 3);
            this.customListParameterList.SelectedItemNumber = 0;
            this.customListParameterList.SelectedItemType = false;
            this.customListParameterList.SelectionColumnIndex = -1;
            this.customListParameterList.Size = new System.Drawing.Size(455, 434);
            this.customListParameterList.SizeControl = new System.Drawing.Size(455, 434);
            this.customListParameterList.SizeItemIcon = new System.Drawing.Size[] {
        new System.Drawing.Size(20, 20),
        new System.Drawing.Size(20, 20)};
            this.customListParameterList.TabIndex = 1;
            this.customListParameterList.CustomListItem_Click += new System.EventHandler(this.customListParameterList_CustomListItem_Click);
            // 
            // timerCommonParameter
            // 
            this.timerCommonParameter.Interval = 1000;
            this.timerCommonParameter.Tick += new System.EventHandler(this.timerCommonParameter_Tick);
            // 
            // labelIcon
            // 
            this.labelIcon.BackColor = System.Drawing.Color.Transparent;
            this.labelIcon.Image = global::VisionSystemControlLibrary.Properties.Resources.System;
            this.labelIcon.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelIcon.Location = new System.Drawing.Point(28, 12);
            this.labelIcon.Name = "labelIcon";
            this.labelIcon.Size = new System.Drawing.Size(48, 48);
            this.labelIcon.TabIndex = 50;
            // 
            // SystemControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.Controls.Add(this.labelIcon);
            this.Controls.Add(this.customButtonNextPage_ValueList);
            this.Controls.Add(this.customButtonPreviousPage_ValueList);
            this.Controls.Add(this.customButtonNextPage_ParameterList);
            this.Controls.Add(this.customButtonPreviousPage_ParameterList);
            this.Controls.Add(this.customButtonCancel);
            this.Controls.Add(this.customButtonOk);
            this.Controls.Add(this.customButtonValueList);
            this.Controls.Add(this.customButtonParameterList);
            this.Controls.Add(this.customButtonClose);
            this.Controls.Add(this.customButtonCaption);
            this.Controls.Add(this.customListValueList);
            this.Controls.Add(this.customListParameterList);
            this.Name = "SystemControl";
            this.Size = new System.Drawing.Size(1024, 662);
            this.Load += new System.EventHandler(this.SystemControl_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private CustomList customListParameterList;
        private CustomList customListValueList;
        private System.Windows.Forms.Timer timerPCTime;
        private CustomButton customButtonCaption;
        private CustomButton customButtonClose;
        private CustomButton customButtonParameterList;
        private CustomButton customButtonValueList;
        private CustomButton customButtonOk;
        private CustomButton customButtonCancel;
        private CustomButton customButtonNextPage_ParameterList;
        private CustomButton customButtonPreviousPage_ParameterList;
        private CustomButton customButtonNextPage_ValueList;
        private CustomButton customButtonPreviousPage_ValueList;
        private System.Windows.Forms.Timer timerCommonParameter;
        private System.Windows.Forms.Label labelIcon;
    }
}
