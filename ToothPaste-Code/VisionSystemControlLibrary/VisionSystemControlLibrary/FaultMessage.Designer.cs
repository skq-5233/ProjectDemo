namespace VisionSystemControlLibrary
{
    partial class FaultMessage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FaultMessage));
            this.timerData = new System.Windows.Forms.Timer(this.components);
            this.customButtonOption = new VisionSystemControlLibrary.CustomButton();
            this.customButtonClearAll = new VisionSystemControlLibrary.CustomButton();
            this.customButtonNextPage_List_2 = new VisionSystemControlLibrary.CustomButton();
            this.customButtonPreviousPage_List_2 = new VisionSystemControlLibrary.CustomButton();
            this.customButtonNextPage_List_1 = new VisionSystemControlLibrary.CustomButton();
            this.customButtonPreviousPage_List_1 = new VisionSystemControlLibrary.CustomButton();
            this.customButtonOk = new VisionSystemControlLibrary.CustomButton();
            this.customListFault_2 = new VisionSystemControlLibrary.CustomList();
            this.customButtonCaption = new VisionSystemControlLibrary.CustomButton();
            this.customListFault_1 = new VisionSystemControlLibrary.CustomList();
            this.customButtonBackground = new VisionSystemControlLibrary.CustomButton();
            this.SuspendLayout();
            // 
            // timerData
            // 
            this.timerData.Interval = 1000;
            this.timerData.Tick += new System.EventHandler(this.timerData_Tick);
            // 
            // customButtonOption
            // 
            this.customButtonOption.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonOption.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonOption.BitmapIconWhole = global::VisionSystemControlLibrary.Properties.Resources.Port;
            this.customButtonOption.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.Button;
            this.customButtonOption.Chinese_TextDisplay = new string[] {
        "使能/禁止"};
            this.customButtonOption.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 13)};
            this.customButtonOption.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonOption.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(80, 23)};
            this.customButtonOption.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonOption.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonOption.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonOption.CurrentTextGroupIndex = 0;
            this.customButtonOption.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonOption.CustomButtonData = null;
            this.customButtonOption.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonOption.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonOption.DrawIcon = true;
            this.customButtonOption.DrawText = true;
            this.customButtonOption.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonOption.English_TextDisplay = new string[] {
        "OPTION"};
            this.customButtonOption.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 13)};
            this.customButtonOption.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonOption.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(73, 23)};
            this.customButtonOption.FocusBackgroundDisplay = false;
            this.customButtonOption.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonOption.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonOption.ForeColor = System.Drawing.Color.White;
            this.customButtonOption.HoverBackgroundDisplay = false;
            this.customButtonOption.IconIndex = new int[] {
        1,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonOption.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(85, 7),
        new System.Drawing.Point(85, 7)};
            this.customButtonOption.IconNumber = 2;
            this.customButtonOption.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(53, 37),
        new System.Drawing.Size(53, 37)};
            this.customButtonOption.LabelControlMode = false;
            this.customButtonOption.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonOption.Location = new System.Drawing.Point(450, 455);
            this.customButtonOption.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonOption.Name = "customButtonOption";
            this.customButtonOption.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonOption.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonOption.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonOption.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonOption.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonOption.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonOption.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonOption.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonOption.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonOption.Size = new System.Drawing.Size(150, 50);
            this.customButtonOption.SizeButton = new System.Drawing.Size(150, 50);
            this.customButtonOption.TabIndex = 173;
            this.customButtonOption.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonOption.TextGroupNumber = 1;
            this.customButtonOption.UpdateControl = true;
            this.customButtonOption.CustomButton_Click += new System.EventHandler(this.customButtonOption_CustomButton_Click);
            // 
            // customButtonClearAll
            // 
            this.customButtonClearAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonClearAll.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonClearAll.BitmapIconWhole = global::VisionSystemControlLibrary.Properties.Resources.DeleteBrand;
            this.customButtonClearAll.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.Button;
            this.customButtonClearAll.Chinese_TextDisplay = new string[] {
        "清除所有"};
            this.customButtonClearAll.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 13)};
            this.customButtonClearAll.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonClearAll.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(72, 23)};
            this.customButtonClearAll.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonClearAll.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonClearAll.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonClearAll.CurrentTextGroupIndex = 0;
            this.customButtonClearAll.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonClearAll.CustomButtonData = null;
            this.customButtonClearAll.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonClearAll.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonClearAll.DrawIcon = true;
            this.customButtonClearAll.DrawText = true;
            this.customButtonClearAll.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonClearAll.English_TextDisplay = new string[] {
        "CLEAR&ALL"};
            this.customButtonClearAll.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 4),
        new System.Drawing.Point(5, 22)};
            this.customButtonClearAll.English_TextNumberInTextGroup = new int[] {
        2};
            this.customButtonClearAll.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(59, 23),
        new System.Drawing.Size(36, 23)};
            this.customButtonClearAll.FocusBackgroundDisplay = false;
            this.customButtonClearAll.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonClearAll.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonClearAll.ForeColor = System.Drawing.Color.White;
            this.customButtonClearAll.HoverBackgroundDisplay = false;
            this.customButtonClearAll.IconIndex = new int[] {
        1,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonClearAll.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(73, 7),
        new System.Drawing.Point(73, 7)};
            this.customButtonClearAll.IconNumber = 2;
            this.customButtonClearAll.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(53, 37),
        new System.Drawing.Size(53, 37)};
            this.customButtonClearAll.LabelControlMode = false;
            this.customButtonClearAll.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonClearAll.Location = new System.Drawing.Point(293, 455);
            this.customButtonClearAll.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonClearAll.Name = "customButtonClearAll";
            this.customButtonClearAll.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonClearAll.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonClearAll.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonClearAll.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonClearAll.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonClearAll.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonClearAll.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonClearAll.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonClearAll.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonClearAll.Size = new System.Drawing.Size(130, 50);
            this.customButtonClearAll.SizeButton = new System.Drawing.Size(130, 50);
            this.customButtonClearAll.TabIndex = 170;
            this.customButtonClearAll.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonClearAll.TextGroupNumber = 1;
            this.customButtonClearAll.UpdateControl = true;
            this.customButtonClearAll.CustomButton_Click += new System.EventHandler(this.customButtonClearAll_CustomButton_Click);
            // 
            // customButtonNextPage_List_2
            // 
            this.customButtonNextPage_List_2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonNextPage_List_2.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonNextPage_List_2.BitmapIconWhole = global::VisionSystemControlLibrary.Properties.Resources.Down;
            this.customButtonNextPage_List_2.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.Button;
            this.customButtonNextPage_List_2.Chinese_TextDisplay = new string[] {
        "下一页"};
            this.customButtonNextPage_List_2.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 5)};
            this.customButtonNextPage_List_2.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonNextPage_List_2.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(55, 23)};
            this.customButtonNextPage_List_2.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonNextPage_List_2.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonNextPage_List_2.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonNextPage_List_2.CurrentTextGroupIndex = 0;
            this.customButtonNextPage_List_2.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonNextPage_List_2.CustomButtonData = null;
            this.customButtonNextPage_List_2.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonNextPage_List_2.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonNextPage_List_2.DrawIcon = true;
            this.customButtonNextPage_List_2.DrawText = false;
            this.customButtonNextPage_List_2.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonNextPage_List_2.English_TextDisplay = new string[] {
        "NEXT&PAGE"};
            this.customButtonNextPage_List_2.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 5),
        new System.Drawing.Point(5, 25)};
            this.customButtonNextPage_List_2.English_TextNumberInTextGroup = new int[] {
        2};
            this.customButtonNextPage_List_2.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(51, 23),
        new System.Drawing.Size(51, 23)};
            this.customButtonNextPage_List_2.FocusBackgroundDisplay = false;
            this.customButtonNextPage_List_2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonNextPage_List_2.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonNextPage_List_2.ForeColor = System.Drawing.Color.White;
            this.customButtonNextPage_List_2.HoverBackgroundDisplay = false;
            this.customButtonNextPage_List_2.IconIndex = new int[] {
        1,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonNextPage_List_2.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(8, 2),
        new System.Drawing.Point(8, 2)};
            this.customButtonNextPage_List_2.IconNumber = 2;
            this.customButtonNextPage_List_2.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(48, 70),
        new System.Drawing.Size(48, 70)};
            this.customButtonNextPage_List_2.LabelControlMode = false;
            this.customButtonNextPage_List_2.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonNextPage_List_2.Location = new System.Drawing.Point(801, 445);
            this.customButtonNextPage_List_2.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonNextPage_List_2.Name = "customButtonNextPage_List_2";
            this.customButtonNextPage_List_2.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonNextPage_List_2.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonNextPage_List_2.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonNextPage_List_2.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonNextPage_List_2.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonNextPage_List_2.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonNextPage_List_2.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonNextPage_List_2.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonNextPage_List_2.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonNextPage_List_2.Size = new System.Drawing.Size(63, 73);
            this.customButtonNextPage_List_2.SizeButton = new System.Drawing.Size(63, 73);
            this.customButtonNextPage_List_2.TabIndex = 169;
            this.customButtonNextPage_List_2.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonNextPage_List_2.TextGroupNumber = 1;
            this.customButtonNextPage_List_2.UpdateControl = true;
            this.customButtonNextPage_List_2.CustomButton_Click += new System.EventHandler(this.customButtonNextPage_List_2_CustomButton_Click);
            // 
            // customButtonPreviousPage_List_2
            // 
            this.customButtonPreviousPage_List_2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonPreviousPage_List_2.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonPreviousPage_List_2.BitmapIconWhole = global::VisionSystemControlLibrary.Properties.Resources.Up;
            this.customButtonPreviousPage_List_2.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.Button;
            this.customButtonPreviousPage_List_2.Chinese_TextDisplay = new string[] {
        "上一页"};
            this.customButtonPreviousPage_List_2.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 5)};
            this.customButtonPreviousPage_List_2.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonPreviousPage_List_2.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(55, 23)};
            this.customButtonPreviousPage_List_2.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonPreviousPage_List_2.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonPreviousPage_List_2.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonPreviousPage_List_2.CurrentTextGroupIndex = 0;
            this.customButtonPreviousPage_List_2.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonPreviousPage_List_2.CustomButtonData = null;
            this.customButtonPreviousPage_List_2.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonPreviousPage_List_2.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonPreviousPage_List_2.DrawIcon = true;
            this.customButtonPreviousPage_List_2.DrawText = false;
            this.customButtonPreviousPage_List_2.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonPreviousPage_List_2.English_TextDisplay = new string[] {
        "PREV.&PAGE"};
            this.customButtonPreviousPage_List_2.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 5),
        new System.Drawing.Point(5, 25)};
            this.customButtonPreviousPage_List_2.English_TextNumberInTextGroup = new int[] {
        2};
            this.customButtonPreviousPage_List_2.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(54, 23),
        new System.Drawing.Size(51, 23)};
            this.customButtonPreviousPage_List_2.FocusBackgroundDisplay = false;
            this.customButtonPreviousPage_List_2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonPreviousPage_List_2.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonPreviousPage_List_2.ForeColor = System.Drawing.Color.White;
            this.customButtonPreviousPage_List_2.HoverBackgroundDisplay = false;
            this.customButtonPreviousPage_List_2.IconIndex = new int[] {
        1,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonPreviousPage_List_2.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(8, 2),
        new System.Drawing.Point(8, 2)};
            this.customButtonPreviousPage_List_2.IconNumber = 2;
            this.customButtonPreviousPage_List_2.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(48, 70),
        new System.Drawing.Size(48, 70)};
            this.customButtonPreviousPage_List_2.LabelControlMode = false;
            this.customButtonPreviousPage_List_2.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonPreviousPage_List_2.Location = new System.Drawing.Point(735, 445);
            this.customButtonPreviousPage_List_2.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonPreviousPage_List_2.Name = "customButtonPreviousPage_List_2";
            this.customButtonPreviousPage_List_2.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonPreviousPage_List_2.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonPreviousPage_List_2.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonPreviousPage_List_2.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonPreviousPage_List_2.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonPreviousPage_List_2.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonPreviousPage_List_2.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonPreviousPage_List_2.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonPreviousPage_List_2.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonPreviousPage_List_2.Size = new System.Drawing.Size(63, 73);
            this.customButtonPreviousPage_List_2.SizeButton = new System.Drawing.Size(63, 73);
            this.customButtonPreviousPage_List_2.TabIndex = 168;
            this.customButtonPreviousPage_List_2.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonPreviousPage_List_2.TextGroupNumber = 1;
            this.customButtonPreviousPage_List_2.UpdateControl = true;
            this.customButtonPreviousPage_List_2.CustomButton_Click += new System.EventHandler(this.customButtonPreviousPage_List_2_CustomButton_Click);
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
            this.customButtonNextPage_List_1.Location = new System.Drawing.Point(217, 445);
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
            this.customButtonNextPage_List_1.TabIndex = 167;
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
            this.customButtonPreviousPage_List_1.Location = new System.Drawing.Point(151, 445);
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
            this.customButtonPreviousPage_List_1.TabIndex = 166;
            this.customButtonPreviousPage_List_1.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonPreviousPage_List_1.TextGroupNumber = 1;
            this.customButtonPreviousPage_List_1.UpdateControl = true;
            this.customButtonPreviousPage_List_1.CustomButton_Click += new System.EventHandler(this.customButtonPreviousPage_List_1_CustomButton_Click);
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
        new System.Drawing.Point(43, 7),
        new System.Drawing.Point(43, 7)};
            this.customButtonOk.IconNumber = 2;
            this.customButtonOk.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(53, 37),
        new System.Drawing.Size(53, 37)};
            this.customButtonOk.LabelControlMode = false;
            this.customButtonOk.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonOk.Location = new System.Drawing.Point(373, 558);
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
            this.customButtonOk.Size = new System.Drawing.Size(130, 50);
            this.customButtonOk.SizeButton = new System.Drawing.Size(130, 50);
            this.customButtonOk.TabIndex = 165;
            this.customButtonOk.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonOk.TextGroupNumber = 1;
            this.customButtonOk.UpdateControl = true;
            this.customButtonOk.CustomButton_Click += new System.EventHandler(this.customButtonOk_CustomButton_Click);
            // 
            // customListFault_2
            // 
            this.customListFault_2.BackColor = System.Drawing.Color.Black;
            this.customListFault_2.BitmapBackgroundIndex = new int[] {
        0,
        0};
            this.customListFault_2.BitmapBackgroundNumber = 1;
            this.customListFault_2.BitmapBackgroundWhole = global::VisionSystemControlLibrary.Properties.Resources.ListHeader;
            this.customListFault_2.BitmapIcon = new System.Drawing.Bitmap[] {
        global::VisionSystemControlLibrary.Properties.Resources.CustomListItem_BackupBrandFolder,
        global::VisionSystemControlLibrary.Properties.Resources.CustomListItem_Folder,
        global::VisionSystemControlLibrary.Properties.Resources.CustomListItem_SystemFile,
        global::VisionSystemControlLibrary.Properties.Resources.CustomListItem_Marked};
            this.customListFault_2.Chinese_ColumnNameDisplay = new string[] {
        "名称",
        "时间"};
            this.customListFault_2.ColorControlBackground = System.Drawing.Color.Black;
            this.customListFault_2.ColorItemBackgroundSelected = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.customListFault_2.ColorItemBackgroundUnselected = System.Drawing.Color.Black;
            this.customListFault_2.ColorItemTextSelected = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customListFault_2.ColorItemTextUnselected = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customListFault_2.ColorListHeaderBackground = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.customListFault_2.ColorListHeaderColumnText_Disable = new System.Drawing.Color[] {
        System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))))};
            this.customListFault_2.ColorListHeaderColumnText_Enable = new System.Drawing.Color[] {
        System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))))};
            this.customListFault_2.ColorListItemBackground = System.Drawing.Color.Black;
            this.customListFault_2.ColorPageBackground = System.Drawing.Color.Black;
            this.customListFault_2.ColorPageText = System.Drawing.Color.Yellow;
            this.customListFault_2.ColumnNameXOffSetValue = 2;
            this.customListFault_2.ColumnNumber = 2;
            this.customListFault_2.ColumnWidth = new int[] {
        320,
        240};
            this.customListFault_2.CurrentColumnNameGroupIndex = new int[] {
        0,
        0};
            this.customListFault_2.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customListFault_2.English_ColumnNameDisplay = new string[] {
        "Name",
        "Time"};
            this.customListFault_2.FontListHeader = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customListFault_2.FontListItem = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customListFault_2.FontPage = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customListFault_2.ForeColor = System.Drawing.Color.White;
            this.customListFault_2.ItemDataDisplay = new bool[] {
        true,
        true};
            this.customListFault_2.ItemDataNumber = 0;
            this.customListFault_2.ItemIconIndex = new int[] {
        -1,
        -1};
            this.customListFault_2.ItemIconNumber = 4;
            this.customListFault_2.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customListFault_2.ListEnabled = true;
            this.customListFault_2.ListHeaderHeight = 36;
            this.customListFault_2.ListHeaderTextAlignment = System.Drawing.StringAlignment.Near;
            this.customListFault_2.ListItemHeight = 35;
            this.customListFault_2.ListItemTextAlignment = System.Drawing.StringAlignment.Near;
            this.customListFault_2.ListItemXOffSetValue = 7;
            this.customListFault_2.Location = new System.Drawing.Point(293, 76);
            this.customListFault_2.Name = "customListFault_2";
            this.customListFault_2.PageHeight = 25;
            this.customListFault_2.RectBottom = new System.Drawing.Rectangle(3, 23, 237, 3);
            this.customListFault_2.RectFill = new System.Drawing.Rectangle(3, 3, 237, 20);
            this.customListFault_2.RectLeft = new System.Drawing.Rectangle(0, 3, 3, 20);
            this.customListFault_2.RectLeftBottom = new System.Drawing.Rectangle(0, 23, 3, 3);
            this.customListFault_2.RectLeftTop = new System.Drawing.Rectangle(0, 0, 3, 3);
            this.customListFault_2.RectRight = new System.Drawing.Rectangle(240, 3, 3, 20);
            this.customListFault_2.RectRightBottom = new System.Drawing.Rectangle(240, 23, 3, 3);
            this.customListFault_2.RectRightTop = new System.Drawing.Rectangle(240, 0, 3, 3);
            this.customListFault_2.RectTop = new System.Drawing.Rectangle(3, 0, 237, 3);
            this.customListFault_2.SelectedItemNumber = 0;
            this.customListFault_2.SelectedItemType = false;
            this.customListFault_2.SelectionColumnIndex = -1;
            this.customListFault_2.Size = new System.Drawing.Size(571, 360);
            this.customListFault_2.SizeControl = new System.Drawing.Size(571, 360);
            this.customListFault_2.SizeItemIcon = new System.Drawing.Size[] {
        new System.Drawing.Size(20, 20),
        new System.Drawing.Size(20, 20)};
            this.customListFault_2.TabIndex = 80;
            this.customListFault_2.CustomListItem_Click += new System.EventHandler(this.customListFault_2_CustomListItem_Click);
            // 
            // customButtonCaption
            // 
            this.customButtonCaption.BackColor = System.Drawing.Color.Black;
            this.customButtonCaption.BackgroundColor = System.Drawing.Color.Black;
            this.customButtonCaption.BitmapIconWhole = null;
            this.customButtonCaption.BitmapWhole = null;
            this.customButtonCaption.Chinese_TextDisplay = new string[] {
        "故障信息"};
            this.customButtonCaption.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(343, 7)};
            this.customButtonCaption.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonCaption.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(90, 29)};
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
        "FAULT MESSAGE"};
            this.customButtonCaption.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(297, 7)};
            this.customButtonCaption.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonCaption.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(181, 29)};
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
            this.customButtonCaption.Location = new System.Drawing.Point(49, 11);
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
            this.customButtonCaption.TabIndex = 53;
            this.customButtonCaption.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.customButtonCaption.TextGroupNumber = 1;
            this.customButtonCaption.UpdateControl = true;
            // 
            // customListFault_1
            // 
            this.customListFault_1.BackColor = System.Drawing.Color.Black;
            this.customListFault_1.BitmapBackgroundIndex = new int[] {
        0,
        0};
            this.customListFault_1.BitmapBackgroundNumber = 1;
            this.customListFault_1.BitmapBackgroundWhole = ((System.Drawing.Bitmap)(resources.GetObject("customListFault_1.BitmapBackgroundWhole")));
            this.customListFault_1.BitmapIcon = new System.Drawing.Bitmap[] {
        null,
        null,
        null,
        null,
        null,
        null};
            this.customListFault_1.Chinese_ColumnNameDisplay = new string[] {
        "相机",
        "数值"};
            this.customListFault_1.ColorControlBackground = System.Drawing.Color.Black;
            this.customListFault_1.ColorItemBackgroundSelected = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.customListFault_1.ColorItemBackgroundUnselected = System.Drawing.Color.Black;
            this.customListFault_1.ColorItemTextSelected = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customListFault_1.ColorItemTextUnselected = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customListFault_1.ColorListHeaderBackground = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.customListFault_1.ColorListHeaderColumnText_Disable = new System.Drawing.Color[] {
        System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))))};
            this.customListFault_1.ColorListHeaderColumnText_Enable = new System.Drawing.Color[] {
        System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))))};
            this.customListFault_1.ColorListItemBackground = System.Drawing.Color.Black;
            this.customListFault_1.ColorPageBackground = System.Drawing.Color.Black;
            this.customListFault_1.ColorPageText = System.Drawing.Color.White;
            this.customListFault_1.ColumnNameXOffSetValue = 7;
            this.customListFault_1.ColumnNumber = 2;
            this.customListFault_1.ColumnWidth = new int[] {
        160,
        100};
            this.customListFault_1.CurrentColumnNameGroupIndex = new int[] {
        0,
        0};
            this.customListFault_1.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customListFault_1.English_ColumnNameDisplay = new string[] {
        "Camera",
        "Value"};
            this.customListFault_1.FontListHeader = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customListFault_1.FontListItem = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customListFault_1.FontPage = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customListFault_1.ForeColor = System.Drawing.Color.White;
            this.customListFault_1.ItemDataDisplay = new bool[] {
        true,
        true};
            this.customListFault_1.ItemDataNumber = 0;
            this.customListFault_1.ItemIconIndex = new int[] {
        -1,
        -1};
            this.customListFault_1.ItemIconNumber = 6;
            this.customListFault_1.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customListFault_1.ListEnabled = true;
            this.customListFault_1.ListHeaderHeight = 36;
            this.customListFault_1.ListHeaderTextAlignment = System.Drawing.StringAlignment.Near;
            this.customListFault_1.ListItemHeight = 35;
            this.customListFault_1.ListItemTextAlignment = System.Drawing.StringAlignment.Near;
            this.customListFault_1.ListItemXOffSetValue = 7;
            this.customListFault_1.Location = new System.Drawing.Point(9, 76);
            this.customListFault_1.Name = "customListFault_1";
            this.customListFault_1.PageHeight = 25;
            this.customListFault_1.RectBottom = new System.Drawing.Rectangle(3, 23, 237, 3);
            this.customListFault_1.RectFill = new System.Drawing.Rectangle(3, 3, 237, 20);
            this.customListFault_1.RectLeft = new System.Drawing.Rectangle(0, 3, 3, 20);
            this.customListFault_1.RectLeftBottom = new System.Drawing.Rectangle(0, 23, 3, 3);
            this.customListFault_1.RectLeftTop = new System.Drawing.Rectangle(0, 0, 3, 3);
            this.customListFault_1.RectRight = new System.Drawing.Rectangle(240, 3, 3, 20);
            this.customListFault_1.RectRightBottom = new System.Drawing.Rectangle(240, 23, 3, 3);
            this.customListFault_1.RectRightTop = new System.Drawing.Rectangle(240, 0, 3, 3);
            this.customListFault_1.RectTop = new System.Drawing.Rectangle(3, 0, 237, 3);
            this.customListFault_1.SelectedItemNumber = 0;
            this.customListFault_1.SelectedItemType = false;
            this.customListFault_1.SelectionColumnIndex = -1;
            this.customListFault_1.Size = new System.Drawing.Size(271, 360);
            this.customListFault_1.SizeControl = new System.Drawing.Size(271, 360);
            this.customListFault_1.SizeItemIcon = new System.Drawing.Size[] {
        new System.Drawing.Size(20, 20),
        new System.Drawing.Size(20, 20)};
            this.customListFault_1.TabIndex = 0;
            this.customListFault_1.CustomListItem_Click += new System.EventHandler(this.customListFault_1_CustomListItem_Click);
            // 
            // customButtonBackground
            // 
            this.customButtonBackground.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonBackground.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonBackground.BitmapIconWhole = null;
            this.customButtonBackground.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.KeyboardBackground;
            this.customButtonBackground.Chinese_TextDisplay = new string[] {
        "缩小背景"};
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
        "ZOOMOUT BACKGROUND"};
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
            this.customButtonBackground.RectBottom = new System.Drawing.Rectangle(3, 103, 134, 3);
            this.customButtonBackground.RectFill = new System.Drawing.Rectangle(3, 3, 134, 100);
            this.customButtonBackground.RectLeft = new System.Drawing.Rectangle(0, 3, 3, 100);
            this.customButtonBackground.RectLeftBottom = new System.Drawing.Rectangle(0, 103, 3, 3);
            this.customButtonBackground.RectLeftTop = new System.Drawing.Rectangle(0, 0, 3, 3);
            this.customButtonBackground.RectRight = new System.Drawing.Rectangle(137, 3, 3, 100);
            this.customButtonBackground.RectRightBottom = new System.Drawing.Rectangle(137, 103, 3, 3);
            this.customButtonBackground.RectRightTop = new System.Drawing.Rectangle(137, 0, 3, 3);
            this.customButtonBackground.RectTop = new System.Drawing.Rectangle(3, 0, 134, 3);
            this.customButtonBackground.Size = new System.Drawing.Size(876, 630);
            this.customButtonBackground.SizeButton = new System.Drawing.Size(876, 630);
            this.customButtonBackground.TabIndex = 171;
            this.customButtonBackground.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonBackground.TextGroupNumber = 1;
            this.customButtonBackground.UpdateControl = true;
            // 
            // FaultMessage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.Controls.Add(this.customButtonOption);
            this.Controls.Add(this.customButtonClearAll);
            this.Controls.Add(this.customButtonNextPage_List_2);
            this.Controls.Add(this.customButtonPreviousPage_List_2);
            this.Controls.Add(this.customButtonNextPage_List_1);
            this.Controls.Add(this.customButtonPreviousPage_List_1);
            this.Controls.Add(this.customButtonOk);
            this.Controls.Add(this.customListFault_2);
            this.Controls.Add(this.customButtonCaption);
            this.Controls.Add(this.customListFault_1);
            this.Controls.Add(this.customButtonBackground);
            this.Name = "FaultMessage";
            this.Size = new System.Drawing.Size(876, 630);
            this.ResumeLayout(false);

        }

        #endregion

        private CustomList customListFault_1;
        private CustomButton customButtonCaption;
        private CustomList customListFault_2;
        private CustomButton customButtonClearAll;
        private CustomButton customButtonNextPage_List_2;
        private CustomButton customButtonPreviousPage_List_2;
        private CustomButton customButtonNextPage_List_1;
        private CustomButton customButtonPreviousPage_List_1;
        private CustomButton customButtonOk;
        private System.Windows.Forms.Timer timerData;
        private CustomButton customButtonBackground;
        private CustomButton customButtonOption;
    }
}
