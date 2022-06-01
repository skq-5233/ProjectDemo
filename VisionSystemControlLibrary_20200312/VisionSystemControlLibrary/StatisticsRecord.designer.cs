namespace VisionSystemControlLibrary
{
    partial class StatisticsRecord
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
            this.timerRecord = new System.Windows.Forms.Timer(this.components);
            this.customButtonDateTime = new VisionSystemControlLibrary.CustomButton();
            this.customButtonDeleteAll = new VisionSystemControlLibrary.CustomButton();
            this.customButtonDelete = new VisionSystemControlLibrary.CustomButton();
            this.customButtonSearch = new VisionSystemControlLibrary.CustomButton();
            this.customButtonNextPage_List_2 = new VisionSystemControlLibrary.CustomButton();
            this.customButtonPreviousPage_List_2 = new VisionSystemControlLibrary.CustomButton();
            this.customButtonNextPage_List_1 = new VisionSystemControlLibrary.CustomButton();
            this.customButtonPreviousPage_List_1 = new VisionSystemControlLibrary.CustomButton();
            this.customButtonCancel = new VisionSystemControlLibrary.CustomButton();
            this.customButtonOk = new VisionSystemControlLibrary.CustomButton();
            this.customList_2 = new VisionSystemControlLibrary.CustomList();
            this.customList_1 = new VisionSystemControlLibrary.CustomList();
            this.customButtonCaption = new VisionSystemControlLibrary.CustomButton();
            this.customButtonBackground = new VisionSystemControlLibrary.CustomButton();
            this.SuspendLayout();
            // 
            // timerRecord
            // 
            this.timerRecord.Interval = 1000;
            this.timerRecord.Tick += new System.EventHandler(this.timerRecord_Tick);
            // 
            // customButtonDateTime
            // 
            this.customButtonDateTime.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonDateTime.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonDateTime.BitmapIconWhole = null;
            this.customButtonDateTime.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.Button;
            this.customButtonDateTime.Chinese_TextDisplay = new string[] {
        "时间"};
            this.customButtonDateTime.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(45, 13)};
            this.customButtonDateTime.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonDateTime.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(39, 24)};
            this.customButtonDateTime.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonDateTime.ColorTextEnable = System.Drawing.Color.Yellow;
            this.customButtonDateTime.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonDateTime.CurrentTextGroupIndex = 0;
            this.customButtonDateTime.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonDateTime.CustomButtonData = null;
            this.customButtonDateTime.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Automatic;
            this.customButtonDateTime.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonDateTime.DrawIcon = false;
            this.customButtonDateTime.DrawText = true;
            this.customButtonDateTime.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonDateTime.English_TextDisplay = new string[] {
        "TIME"};
            this.customButtonDateTime.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(41, 13)};
            this.customButtonDateTime.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonDateTime.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(48, 24)};
            this.customButtonDateTime.FocusBackgroundDisplay = false;
            this.customButtonDateTime.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonDateTime.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonDateTime.ForeColor = System.Drawing.Color.White;
            this.customButtonDateTime.HoverBackgroundDisplay = false;
            this.customButtonDateTime.IconIndex = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonDateTime.IconLocation = new System.Drawing.Point[0];
            this.customButtonDateTime.IconNumber = 0;
            this.customButtonDateTime.IconSize = new System.Drawing.Size[0];
            this.customButtonDateTime.LabelControlMode = false;
            this.customButtonDateTime.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonDateTime.Location = new System.Drawing.Point(9, 517);
            this.customButtonDateTime.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonDateTime.Name = "customButtonDateTime";
            this.customButtonDateTime.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonDateTime.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonDateTime.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonDateTime.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonDateTime.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonDateTime.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonDateTime.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonDateTime.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonDateTime.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonDateTime.Size = new System.Drawing.Size(130, 50);
            this.customButtonDateTime.SizeButton = new System.Drawing.Size(130, 50);
            this.customButtonDateTime.TabIndex = 167;
            this.customButtonDateTime.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.customButtonDateTime.TextGroupNumber = 1;
            this.customButtonDateTime.UpdateControl = true;
            this.customButtonDateTime.Visible = false;
            this.customButtonDateTime.CustomButton_Click += new System.EventHandler(this.customButtonDateTime_CustomButton_Click);
            // 
            // customButtonDeleteAll
            // 
            this.customButtonDeleteAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonDeleteAll.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonDeleteAll.BitmapIconWhole = global::VisionSystemControlLibrary.Properties.Resources.DeleteBrand;
            this.customButtonDeleteAll.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.Button;
            this.customButtonDeleteAll.Chinese_TextDisplay = new string[] {
        "删除所有"};
            this.customButtonDeleteAll.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 13)};
            this.customButtonDeleteAll.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonDeleteAll.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(72, 23)};
            this.customButtonDeleteAll.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonDeleteAll.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonDeleteAll.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonDeleteAll.CurrentTextGroupIndex = 0;
            this.customButtonDeleteAll.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonDeleteAll.CustomButtonData = null;
            this.customButtonDeleteAll.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonDeleteAll.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonDeleteAll.DrawIcon = true;
            this.customButtonDeleteAll.DrawText = true;
            this.customButtonDeleteAll.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonDeleteAll.English_TextDisplay = new string[] {
        "DELETE&ALL"};
            this.customButtonDeleteAll.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 4),
        new System.Drawing.Point(5, 22)};
            this.customButtonDeleteAll.English_TextNumberInTextGroup = new int[] {
        2};
            this.customButtonDeleteAll.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(66, 23),
        new System.Drawing.Size(36, 23)};
            this.customButtonDeleteAll.FocusBackgroundDisplay = false;
            this.customButtonDeleteAll.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonDeleteAll.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonDeleteAll.ForeColor = System.Drawing.Color.White;
            this.customButtonDeleteAll.HoverBackgroundDisplay = false;
            this.customButtonDeleteAll.IconIndex = new int[] {
        1,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonDeleteAll.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(73, 7),
        new System.Drawing.Point(73, 7)};
            this.customButtonDeleteAll.IconNumber = 2;
            this.customButtonDeleteAll.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(53, 37),
        new System.Drawing.Size(53, 37)};
            this.customButtonDeleteAll.LabelControlMode = false;
            this.customButtonDeleteAll.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonDeleteAll.Location = new System.Drawing.Point(430, 458);
            this.customButtonDeleteAll.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonDeleteAll.Name = "customButtonDeleteAll";
            this.customButtonDeleteAll.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonDeleteAll.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonDeleteAll.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonDeleteAll.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonDeleteAll.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonDeleteAll.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonDeleteAll.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonDeleteAll.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonDeleteAll.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonDeleteAll.Size = new System.Drawing.Size(130, 50);
            this.customButtonDeleteAll.SizeButton = new System.Drawing.Size(130, 50);
            this.customButtonDeleteAll.TabIndex = 164;
            this.customButtonDeleteAll.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonDeleteAll.TextGroupNumber = 1;
            this.customButtonDeleteAll.UpdateControl = true;
            this.customButtonDeleteAll.CustomButton_Click += new System.EventHandler(this.customButtonDeleteAll_CustomButton_Click);
            // 
            // customButtonDelete
            // 
            this.customButtonDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonDelete.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonDelete.BitmapIconWhole = global::VisionSystemControlLibrary.Properties.Resources.Reject;
            this.customButtonDelete.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.Button;
            this.customButtonDelete.Chinese_TextDisplay = new string[] {
        "删除"};
            this.customButtonDelete.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 13)};
            this.customButtonDelete.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonDelete.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(39, 23)};
            this.customButtonDelete.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonDelete.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonDelete.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonDelete.CurrentTextGroupIndex = 0;
            this.customButtonDelete.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonDelete.CustomButtonData = null;
            this.customButtonDelete.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonDelete.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonDelete.DrawIcon = true;
            this.customButtonDelete.DrawText = true;
            this.customButtonDelete.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonDelete.English_TextDisplay = new string[] {
        "DELETE"};
            this.customButtonDelete.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 13)};
            this.customButtonDelete.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonDelete.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(66, 23)};
            this.customButtonDelete.FocusBackgroundDisplay = false;
            this.customButtonDelete.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonDelete.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonDelete.ForeColor = System.Drawing.Color.White;
            this.customButtonDelete.HoverBackgroundDisplay = false;
            this.customButtonDelete.IconIndex = new int[] {
        1,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonDelete.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(80, 7),
        new System.Drawing.Point(80, 7)};
            this.customButtonDelete.IconNumber = 2;
            this.customButtonDelete.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(53, 37),
        new System.Drawing.Size(53, 37)};
            this.customButtonDelete.LabelControlMode = false;
            this.customButtonDelete.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonDelete.Location = new System.Drawing.Point(293, 458);
            this.customButtonDelete.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonDelete.Name = "customButtonDelete";
            this.customButtonDelete.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonDelete.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonDelete.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonDelete.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonDelete.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonDelete.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonDelete.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonDelete.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonDelete.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonDelete.Size = new System.Drawing.Size(130, 50);
            this.customButtonDelete.SizeButton = new System.Drawing.Size(130, 50);
            this.customButtonDelete.TabIndex = 163;
            this.customButtonDelete.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonDelete.TextGroupNumber = 1;
            this.customButtonDelete.UpdateControl = true;
            this.customButtonDelete.CustomButton_Click += new System.EventHandler(this.customButtonDelete_CustomButton_Click);
            // 
            // customButtonSearch
            // 
            this.customButtonSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonSearch.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonSearch.BitmapIconWhole = global::VisionSystemControlLibrary.Properties.Resources.Learn;
            this.customButtonSearch.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.Button;
            this.customButtonSearch.Chinese_TextDisplay = new string[] {
        "查找"};
            this.customButtonSearch.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 13)};
            this.customButtonSearch.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonSearch.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(39, 23)};
            this.customButtonSearch.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonSearch.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonSearch.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonSearch.CurrentTextGroupIndex = 0;
            this.customButtonSearch.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonSearch.CustomButtonData = null;
            this.customButtonSearch.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonSearch.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Switch;
            this.customButtonSearch.DrawIcon = true;
            this.customButtonSearch.DrawText = true;
            this.customButtonSearch.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonSearch.English_TextDisplay = new string[] {
        "SEARCH"};
            this.customButtonSearch.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 13)};
            this.customButtonSearch.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonSearch.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(74, 23)};
            this.customButtonSearch.FocusBackgroundDisplay = false;
            this.customButtonSearch.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonSearch.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonSearch.ForeColor = System.Drawing.Color.White;
            this.customButtonSearch.HoverBackgroundDisplay = false;
            this.customButtonSearch.IconIndex = new int[] {
        1,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonSearch.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(75, 7),
        new System.Drawing.Point(75, 7)};
            this.customButtonSearch.IconNumber = 2;
            this.customButtonSearch.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(53, 37),
        new System.Drawing.Size(53, 37)};
            this.customButtonSearch.LabelControlMode = false;
            this.customButtonSearch.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonSearch.Location = new System.Drawing.Point(9, 458);
            this.customButtonSearch.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonSearch.Name = "customButtonSearch";
            this.customButtonSearch.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonSearch.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonSearch.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonSearch.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonSearch.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonSearch.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonSearch.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonSearch.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonSearch.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonSearch.Size = new System.Drawing.Size(130, 50);
            this.customButtonSearch.SizeButton = new System.Drawing.Size(130, 50);
            this.customButtonSearch.TabIndex = 161;
            this.customButtonSearch.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonSearch.TextGroupNumber = 1;
            this.customButtonSearch.UpdateControl = true;
            this.customButtonSearch.CustomButton_Click += new System.EventHandler(this.customButtonSearch_CustomButton_Click);
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
            this.customButtonNextPage_List_2.TabIndex = 95;
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
            this.customButtonPreviousPage_List_2.TabIndex = 94;
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
            this.customButtonNextPage_List_1.TabIndex = 93;
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
            this.customButtonPreviousPage_List_1.TabIndex = 92;
            this.customButtonPreviousPage_List_1.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonPreviousPage_List_1.TextGroupNumber = 1;
            this.customButtonPreviousPage_List_1.UpdateControl = true;
            this.customButtonPreviousPage_List_1.CustomButton_Click += new System.EventHandler(this.customButtonPreviousPage_List_1_CustomButton_Click);
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
        new System.Drawing.Point(47, 8),
        new System.Drawing.Point(47, 8)};
            this.customButtonCancel.IconNumber = 2;
            this.customButtonCancel.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(53, 37),
        new System.Drawing.Size(53, 37)};
            this.customButtonCancel.LabelControlMode = false;
            this.customButtonCancel.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonCancel.Location = new System.Drawing.Point(446, 566);
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
            this.customButtonCancel.Size = new System.Drawing.Size(130, 50);
            this.customButtonCancel.SizeButton = new System.Drawing.Size(130, 50);
            this.customButtonCancel.TabIndex = 81;
            this.customButtonCancel.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonCancel.TextGroupNumber = 1;
            this.customButtonCancel.UpdateControl = true;
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
        new System.Drawing.Point(43, 7),
        new System.Drawing.Point(43, 7)};
            this.customButtonOk.IconNumber = 2;
            this.customButtonOk.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(53, 37),
        new System.Drawing.Size(53, 37)};
            this.customButtonOk.LabelControlMode = false;
            this.customButtonOk.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonOk.Location = new System.Drawing.Point(301, 566);
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
            this.customButtonOk.TabIndex = 80;
            this.customButtonOk.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonOk.TextGroupNumber = 1;
            this.customButtonOk.UpdateControl = true;
            this.customButtonOk.CustomButton_Click += new System.EventHandler(this.customButtonOk_CustomButton_Click);
            // 
            // customList_2
            // 
            this.customList_2.BackColor = System.Drawing.Color.Black;
            this.customList_2.BitmapBackgroundIndex = new int[] {
        0,
        0};
            this.customList_2.BitmapBackgroundNumber = 1;
            this.customList_2.BitmapBackgroundWhole = global::VisionSystemControlLibrary.Properties.Resources.ListHeader;
            this.customList_2.BitmapIcon = new System.Drawing.Bitmap[] {
        global::VisionSystemControlLibrary.Properties.Resources.CustomListItem_BackupBrandFolder,
        global::VisionSystemControlLibrary.Properties.Resources.CustomListItem_Folder,
        global::VisionSystemControlLibrary.Properties.Resources.CustomListItem_SystemFile,
        global::VisionSystemControlLibrary.Properties.Resources.CustomListItem_Marked};
            this.customList_2.Chinese_ColumnNameDisplay = new string[] {
        "时间",
        "品牌"};
            this.customList_2.ColorControlBackground = System.Drawing.Color.Black;
            this.customList_2.ColorItemBackgroundSelected = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.customList_2.ColorItemBackgroundUnselected = System.Drawing.Color.Black;
            this.customList_2.ColorItemTextSelected = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customList_2.ColorItemTextUnselected = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customList_2.ColorListHeaderBackground = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.customList_2.ColorListHeaderColumnText_Disable = new System.Drawing.Color[] {
        System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))))};
            this.customList_2.ColorListHeaderColumnText_Enable = new System.Drawing.Color[] {
        System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))))};
            this.customList_2.ColorListItemBackground = System.Drawing.Color.Black;
            this.customList_2.ColorPageBackground = System.Drawing.Color.Black;
            this.customList_2.ColorPageText = System.Drawing.Color.Yellow;
            this.customList_2.ColumnNameXOffSetValue = 2;
            this.customList_2.ColumnNumber = 2;
            this.customList_2.ColumnWidth = new int[] {
        360,
        200};
            this.customList_2.CurrentColumnNameGroupIndex = new int[] {
        0,
        0};
            this.customList_2.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customList_2.English_ColumnNameDisplay = new string[] {
        "Time",
        "Brand"};
            this.customList_2.FontListHeader = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customList_2.FontListItem = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customList_2.FontPage = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customList_2.ForeColor = System.Drawing.Color.White;
            this.customList_2.ItemDataDisplay = new bool[] {
        true,
        true};
            this.customList_2.ItemDataNumber = 0;
            this.customList_2.ItemIconIndex = new int[] {
        -1,
        -1};
            this.customList_2.ItemIconNumber = 4;
            this.customList_2.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customList_2.ListEnabled = true;
            this.customList_2.ListHeaderHeight = 26;
            this.customList_2.ListHeaderTextAlignment = System.Drawing.StringAlignment.Near;
            this.customList_2.ListItemHeight = 25;
            this.customList_2.ListItemTextAlignment = System.Drawing.StringAlignment.Near;
            this.customList_2.ListItemXOffSetValue = 7;
            this.customList_2.Location = new System.Drawing.Point(293, 76);
            this.customList_2.Name = "customList_2";
            this.customList_2.PageHeight = 25;
            this.customList_2.RectBottom = new System.Drawing.Rectangle(3, 23, 237, 3);
            this.customList_2.RectFill = new System.Drawing.Rectangle(3, 3, 237, 20);
            this.customList_2.RectLeft = new System.Drawing.Rectangle(0, 3, 3, 20);
            this.customList_2.RectLeftBottom = new System.Drawing.Rectangle(0, 23, 3, 3);
            this.customList_2.RectLeftTop = new System.Drawing.Rectangle(0, 0, 3, 3);
            this.customList_2.RectRight = new System.Drawing.Rectangle(240, 3, 3, 20);
            this.customList_2.RectRightBottom = new System.Drawing.Rectangle(240, 23, 3, 3);
            this.customList_2.RectRightTop = new System.Drawing.Rectangle(240, 0, 3, 3);
            this.customList_2.RectTop = new System.Drawing.Rectangle(3, 0, 237, 3);
            this.customList_2.SelectedItemNumber = 0;
            this.customList_2.SelectedItemType = false;
            this.customList_2.SelectionColumnIndex = -1;
            this.customList_2.Size = new System.Drawing.Size(571, 360);
            this.customList_2.SizeControl = new System.Drawing.Size(571, 360);
            this.customList_2.SizeItemIcon = new System.Drawing.Size[] {
        new System.Drawing.Size(20, 20),
        new System.Drawing.Size(20, 20)};
            this.customList_2.TabIndex = 79;
            this.customList_2.CustomListItem_Click += new System.EventHandler(this.customList_2_CustomListItem_Click);
            // 
            // customList_1
            // 
            this.customList_1.BackColor = System.Drawing.Color.Black;
            this.customList_1.BitmapBackgroundIndex = new int[] {
        0,
        0};
            this.customList_1.BitmapBackgroundNumber = 1;
            this.customList_1.BitmapBackgroundWhole = global::VisionSystemControlLibrary.Properties.Resources.ListHeader;
            this.customList_1.BitmapIcon = new System.Drawing.Bitmap[] {
        global::VisionSystemControlLibrary.Properties.Resources.CustomListItem_BackupBrandFolder,
        global::VisionSystemControlLibrary.Properties.Resources.CustomListItem_Folder,
        global::VisionSystemControlLibrary.Properties.Resources.CustomListItem_SystemFile,
        global::VisionSystemControlLibrary.Properties.Resources.CustomListItem_Marked};
            this.customList_1.Chinese_ColumnNameDisplay = new string[] {
        "班次",
        "时间"};
            this.customList_1.ColorControlBackground = System.Drawing.Color.Black;
            this.customList_1.ColorItemBackgroundSelected = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.customList_1.ColorItemBackgroundUnselected = System.Drawing.Color.Black;
            this.customList_1.ColorItemTextSelected = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customList_1.ColorItemTextUnselected = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customList_1.ColorListHeaderBackground = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.customList_1.ColorListHeaderColumnText_Disable = new System.Drawing.Color[] {
        System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))))};
            this.customList_1.ColorListHeaderColumnText_Enable = new System.Drawing.Color[] {
        System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))))};
            this.customList_1.ColorListItemBackground = System.Drawing.Color.Black;
            this.customList_1.ColorPageBackground = System.Drawing.Color.Black;
            this.customList_1.ColorPageText = System.Drawing.Color.Yellow;
            this.customList_1.ColumnNameXOffSetValue = 2;
            this.customList_1.ColumnNumber = 2;
            this.customList_1.ColumnWidth = new int[] {
        60,
        200};
            this.customList_1.CurrentColumnNameGroupIndex = new int[] {
        0,
        0};
            this.customList_1.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customList_1.English_ColumnNameDisplay = new string[] {
        "Shift",
        "Time"};
            this.customList_1.FontListHeader = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customList_1.FontListItem = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customList_1.FontPage = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customList_1.ForeColor = System.Drawing.Color.White;
            this.customList_1.ItemDataDisplay = new bool[] {
        true,
        true};
            this.customList_1.ItemDataNumber = 0;
            this.customList_1.ItemIconIndex = new int[] {
        -1,
        -1};
            this.customList_1.ItemIconNumber = 4;
            this.customList_1.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customList_1.ListEnabled = true;
            this.customList_1.ListHeaderHeight = 26;
            this.customList_1.ListHeaderTextAlignment = System.Drawing.StringAlignment.Near;
            this.customList_1.ListItemHeight = 25;
            this.customList_1.ListItemTextAlignment = System.Drawing.StringAlignment.Near;
            this.customList_1.ListItemXOffSetValue = 7;
            this.customList_1.Location = new System.Drawing.Point(9, 76);
            this.customList_1.Name = "customList_1";
            this.customList_1.PageHeight = 25;
            this.customList_1.RectBottom = new System.Drawing.Rectangle(3, 23, 237, 3);
            this.customList_1.RectFill = new System.Drawing.Rectangle(3, 3, 237, 20);
            this.customList_1.RectLeft = new System.Drawing.Rectangle(0, 3, 3, 20);
            this.customList_1.RectLeftBottom = new System.Drawing.Rectangle(0, 23, 3, 3);
            this.customList_1.RectLeftTop = new System.Drawing.Rectangle(0, 0, 3, 3);
            this.customList_1.RectRight = new System.Drawing.Rectangle(240, 3, 3, 20);
            this.customList_1.RectRightBottom = new System.Drawing.Rectangle(240, 23, 3, 3);
            this.customList_1.RectRightTop = new System.Drawing.Rectangle(240, 0, 3, 3);
            this.customList_1.RectTop = new System.Drawing.Rectangle(3, 0, 237, 3);
            this.customList_1.SelectedItemNumber = 0;
            this.customList_1.SelectedItemType = false;
            this.customList_1.SelectionColumnIndex = -1;
            this.customList_1.Size = new System.Drawing.Size(271, 360);
            this.customList_1.SizeControl = new System.Drawing.Size(271, 360);
            this.customList_1.SizeItemIcon = new System.Drawing.Size[] {
        new System.Drawing.Size(20, 20),
        new System.Drawing.Size(20, 20)};
            this.customList_1.TabIndex = 78;
            this.customList_1.CustomListItem_Click += new System.EventHandler(this.customList_1_CustomListItem_Click);
            // 
            // customButtonCaption
            // 
            this.customButtonCaption.BackColor = System.Drawing.Color.Black;
            this.customButtonCaption.BackgroundColor = System.Drawing.Color.Black;
            this.customButtonCaption.BitmapIconWhole = null;
            this.customButtonCaption.BitmapWhole = null;
            this.customButtonCaption.Chinese_TextDisplay = new string[] {
        " - 历史数据"};
            this.customButtonCaption.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(332, 7)};
            this.customButtonCaption.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonCaption.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(111, 29)};
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
        " - STATISTICS RECORD"};
            this.customButtonCaption.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(267, 7)};
            this.customButtonCaption.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonCaption.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(241, 29)};
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
            // customButtonBackground
            // 
            this.customButtonBackground.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.customButtonBackground.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
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
            this.customButtonBackground.TabIndex = 160;
            this.customButtonBackground.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonBackground.TextGroupNumber = 1;
            this.customButtonBackground.UpdateControl = true;
            // 
            // StatisticsRecord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.Controls.Add(this.customButtonDateTime);
            this.Controls.Add(this.customButtonDeleteAll);
            this.Controls.Add(this.customButtonDelete);
            this.Controls.Add(this.customButtonSearch);
            this.Controls.Add(this.customButtonNextPage_List_2);
            this.Controls.Add(this.customButtonPreviousPage_List_2);
            this.Controls.Add(this.customButtonNextPage_List_1);
            this.Controls.Add(this.customButtonPreviousPage_List_1);
            this.Controls.Add(this.customButtonCancel);
            this.Controls.Add(this.customButtonOk);
            this.Controls.Add(this.customList_2);
            this.Controls.Add(this.customList_1);
            this.Controls.Add(this.customButtonCaption);
            this.Controls.Add(this.customButtonBackground);
            this.Name = "StatisticsRecord";
            this.Size = new System.Drawing.Size(876, 630);
            this.ResumeLayout(false);

        }

        #endregion

        private CustomButton customButtonCaption;
        private CustomList customList_1;
        private CustomList customList_2;
        private CustomButton customButtonCancel;
        private CustomButton customButtonOk;
        private CustomButton customButtonNextPage_List_2;
        private CustomButton customButtonPreviousPage_List_2;
        private CustomButton customButtonNextPage_List_1;
        private CustomButton customButtonPreviousPage_List_1;
        private CustomButton customButtonBackground;
        private CustomButton customButtonSearch;
        private CustomButton customButtonDelete;
        private CustomButton customButtonDeleteAll;
        private System.Windows.Forms.Timer timerRecord;
        private CustomButton customButtonDateTime;
    }
}
