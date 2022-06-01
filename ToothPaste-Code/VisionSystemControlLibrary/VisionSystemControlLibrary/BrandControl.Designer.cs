namespace VisionSystemControlLibrary
{
    partial class BrandControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BrandControl));
            this.labelMessage = new System.Windows.Forms.Label();
            this.customButtonNextPage = new VisionSystemControlLibrary.CustomButton();
            this.customButtonPreviousPage = new VisionSystemControlLibrary.CustomButton();
            this.customButtonRestoreBrands = new VisionSystemControlLibrary.CustomButton();
            this.customButtonBackupBrands = new VisionSystemControlLibrary.CustomButton();
            this.customButtonDeleteBrand = new VisionSystemControlLibrary.CustomButton();
            this.customButtonRenameBrand = new VisionSystemControlLibrary.CustomButton();
            this.customButtonCopyBrand = new VisionSystemControlLibrary.CustomButton();
            this.customButtonLoadBrand = new VisionSystemControlLibrary.CustomButton();
            this.customButtonSaveCurrent = new VisionSystemControlLibrary.CustomButton();
            this.customButtonMessage2 = new VisionSystemControlLibrary.CustomButton();
            this.customButtonMessage1 = new VisionSystemControlLibrary.CustomButton();
            this.customButtonClose = new VisionSystemControlLibrary.CustomButton();
            this.customButtonCaption = new VisionSystemControlLibrary.CustomButton();
            this.customList = new VisionSystemControlLibrary.CustomList();
            this.timerLoadReloadBrand = new System.Windows.Forms.Timer(this.components);
            this.labelIcon = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelMessage
            // 
            this.labelMessage.BackColor = System.Drawing.Color.Black;
            this.labelMessage.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelMessage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(225)))), ((int)(((byte)(0)))));
            this.labelMessage.Location = new System.Drawing.Point(21, 85);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(847, 40);
            this.labelMessage.TabIndex = 2;
            this.labelMessage.Text = "LABEL(Cur. Brand)";
            this.labelMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.customButtonNextPage.Location = new System.Drawing.Point(948, 557);
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
            this.customButtonNextPage.TabIndex = 53;
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
            this.customButtonPreviousPage.Location = new System.Drawing.Point(882, 557);
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
            this.customButtonPreviousPage.TabIndex = 52;
            this.customButtonPreviousPage.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonPreviousPage.TextGroupNumber = 1;
            this.customButtonPreviousPage.UpdateControl = true;
            this.customButtonPreviousPage.Visible = false;
            this.customButtonPreviousPage.CustomButton_Click += new System.EventHandler(this.customButtonPreviousPage_CustomButton_Click);
            // 
            // customButtonRestoreBrands
            // 
            this.customButtonRestoreBrands.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonRestoreBrands.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonRestoreBrands.BitmapIconWhole = global::VisionSystemControlLibrary.Properties.Resources.RestoreBrands;
            this.customButtonRestoreBrands.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.Button;
            this.customButtonRestoreBrands.Chinese_TextDisplay = new string[] {
        "恢复品牌"};
            this.customButtonRestoreBrands.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 13)};
            this.customButtonRestoreBrands.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonRestoreBrands.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(72, 23)};
            this.customButtonRestoreBrands.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonRestoreBrands.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonRestoreBrands.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonRestoreBrands.CurrentTextGroupIndex = 0;
            this.customButtonRestoreBrands.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonRestoreBrands.CustomButtonData = null;
            this.customButtonRestoreBrands.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonRestoreBrands.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonRestoreBrands.DrawIcon = true;
            this.customButtonRestoreBrands.DrawText = true;
            this.customButtonRestoreBrands.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonRestoreBrands.English_TextDisplay = new string[] {
        "RESTORE&BRANDS"};
            this.customButtonRestoreBrands.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 4),
        new System.Drawing.Point(5, 24)};
            this.customButtonRestoreBrands.English_TextNumberInTextGroup = new int[] {
        2};
            this.customButtonRestoreBrands.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(81, 23),
        new System.Drawing.Size(78, 23)};
            this.customButtonRestoreBrands.FocusBackgroundDisplay = false;
            this.customButtonRestoreBrands.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonRestoreBrands.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonRestoreBrands.ForeColor = System.Drawing.Color.White;
            this.customButtonRestoreBrands.HoverBackgroundDisplay = false;
            this.customButtonRestoreBrands.IconIndex = new int[] {
        1,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonRestoreBrands.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(73, 6),
        new System.Drawing.Point(73, 6)};
            this.customButtonRestoreBrands.IconNumber = 2;
            this.customButtonRestoreBrands.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(53, 37),
        new System.Drawing.Size(53, 37)};
            this.customButtonRestoreBrands.LabelControlMode = false;
            this.customButtonRestoreBrands.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonRestoreBrands.Location = new System.Drawing.Point(882, 495);
            this.customButtonRestoreBrands.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonRestoreBrands.Name = "customButtonRestoreBrands";
            this.customButtonRestoreBrands.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonRestoreBrands.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonRestoreBrands.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonRestoreBrands.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonRestoreBrands.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonRestoreBrands.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonRestoreBrands.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonRestoreBrands.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonRestoreBrands.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonRestoreBrands.Size = new System.Drawing.Size(129, 51);
            this.customButtonRestoreBrands.SizeButton = new System.Drawing.Size(129, 51);
            this.customButtonRestoreBrands.TabIndex = 51;
            this.customButtonRestoreBrands.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonRestoreBrands.TextGroupNumber = 1;
            this.customButtonRestoreBrands.UpdateControl = true;
            this.customButtonRestoreBrands.CustomButton_Click += new System.EventHandler(this.customButtonRestoreBrands_CustomButton_Click);
            // 
            // customButtonBackupBrands
            // 
            this.customButtonBackupBrands.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonBackupBrands.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonBackupBrands.BitmapIconWhole = global::VisionSystemControlLibrary.Properties.Resources.BackupBrands;
            this.customButtonBackupBrands.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.Button;
            this.customButtonBackupBrands.Chinese_TextDisplay = new string[] {
        "备份品牌"};
            this.customButtonBackupBrands.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 13)};
            this.customButtonBackupBrands.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonBackupBrands.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(72, 23)};
            this.customButtonBackupBrands.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonBackupBrands.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonBackupBrands.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonBackupBrands.CurrentTextGroupIndex = 0;
            this.customButtonBackupBrands.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonBackupBrands.CustomButtonData = null;
            this.customButtonBackupBrands.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonBackupBrands.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonBackupBrands.DrawIcon = true;
            this.customButtonBackupBrands.DrawText = true;
            this.customButtonBackupBrands.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonBackupBrands.English_TextDisplay = new string[] {
        "BACKUP&BRANDS"};
            this.customButtonBackupBrands.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 4),
        new System.Drawing.Point(5, 24)};
            this.customButtonBackupBrands.English_TextNumberInTextGroup = new int[] {
        2};
            this.customButtonBackupBrands.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(76, 23),
        new System.Drawing.Size(78, 23)};
            this.customButtonBackupBrands.FocusBackgroundDisplay = false;
            this.customButtonBackupBrands.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonBackupBrands.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonBackupBrands.ForeColor = System.Drawing.Color.White;
            this.customButtonBackupBrands.HoverBackgroundDisplay = false;
            this.customButtonBackupBrands.IconIndex = new int[] {
        1,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonBackupBrands.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(73, 6),
        new System.Drawing.Point(73, 6)};
            this.customButtonBackupBrands.IconNumber = 2;
            this.customButtonBackupBrands.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(53, 37),
        new System.Drawing.Size(53, 37)};
            this.customButtonBackupBrands.LabelControlMode = false;
            this.customButtonBackupBrands.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonBackupBrands.Location = new System.Drawing.Point(882, 441);
            this.customButtonBackupBrands.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonBackupBrands.Name = "customButtonBackupBrands";
            this.customButtonBackupBrands.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonBackupBrands.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonBackupBrands.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonBackupBrands.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonBackupBrands.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonBackupBrands.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonBackupBrands.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonBackupBrands.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonBackupBrands.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonBackupBrands.Size = new System.Drawing.Size(129, 51);
            this.customButtonBackupBrands.SizeButton = new System.Drawing.Size(129, 51);
            this.customButtonBackupBrands.TabIndex = 50;
            this.customButtonBackupBrands.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonBackupBrands.TextGroupNumber = 1;
            this.customButtonBackupBrands.UpdateControl = true;
            this.customButtonBackupBrands.CustomButton_Click += new System.EventHandler(this.customButtonBackupBrands_CustomButton_Click);
            // 
            // customButtonDeleteBrand
            // 
            this.customButtonDeleteBrand.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonDeleteBrand.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonDeleteBrand.BitmapIconWhole = global::VisionSystemControlLibrary.Properties.Resources.DeleteBrand;
            this.customButtonDeleteBrand.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.Button;
            this.customButtonDeleteBrand.Chinese_TextDisplay = new string[] {
        "删除品牌"};
            this.customButtonDeleteBrand.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 13)};
            this.customButtonDeleteBrand.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonDeleteBrand.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(72, 23)};
            this.customButtonDeleteBrand.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonDeleteBrand.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonDeleteBrand.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonDeleteBrand.CurrentTextGroupIndex = 0;
            this.customButtonDeleteBrand.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonDeleteBrand.CustomButtonData = null;
            this.customButtonDeleteBrand.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonDeleteBrand.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonDeleteBrand.DrawIcon = true;
            this.customButtonDeleteBrand.DrawText = true;
            this.customButtonDeleteBrand.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonDeleteBrand.English_TextDisplay = new string[] {
        "DELETE&BRAND"};
            this.customButtonDeleteBrand.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 4),
        new System.Drawing.Point(5, 24)};
            this.customButtonDeleteBrand.English_TextNumberInTextGroup = new int[] {
        2};
            this.customButtonDeleteBrand.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(66, 23),
        new System.Drawing.Size(68, 23)};
            this.customButtonDeleteBrand.FocusBackgroundDisplay = false;
            this.customButtonDeleteBrand.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonDeleteBrand.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonDeleteBrand.ForeColor = System.Drawing.Color.White;
            this.customButtonDeleteBrand.HoverBackgroundDisplay = false;
            this.customButtonDeleteBrand.IconIndex = new int[] {
        1,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonDeleteBrand.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(73, 5),
        new System.Drawing.Point(73, 5)};
            this.customButtonDeleteBrand.IconNumber = 2;
            this.customButtonDeleteBrand.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(53, 37),
        new System.Drawing.Size(53, 37)};
            this.customButtonDeleteBrand.LabelControlMode = false;
            this.customButtonDeleteBrand.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonDeleteBrand.Location = new System.Drawing.Point(882, 387);
            this.customButtonDeleteBrand.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonDeleteBrand.Name = "customButtonDeleteBrand";
            this.customButtonDeleteBrand.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonDeleteBrand.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonDeleteBrand.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonDeleteBrand.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonDeleteBrand.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonDeleteBrand.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonDeleteBrand.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonDeleteBrand.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonDeleteBrand.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonDeleteBrand.Size = new System.Drawing.Size(129, 51);
            this.customButtonDeleteBrand.SizeButton = new System.Drawing.Size(129, 51);
            this.customButtonDeleteBrand.TabIndex = 49;
            this.customButtonDeleteBrand.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonDeleteBrand.TextGroupNumber = 1;
            this.customButtonDeleteBrand.UpdateControl = true;
            this.customButtonDeleteBrand.CustomButton_Click += new System.EventHandler(this.customButtonDeleteBrand_CustomButton_Click);
            // 
            // customButtonRenameBrand
            // 
            this.customButtonRenameBrand.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonRenameBrand.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonRenameBrand.BitmapIconWhole = global::VisionSystemControlLibrary.Properties.Resources.RenameBrand;
            this.customButtonRenameBrand.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.Button;
            this.customButtonRenameBrand.Chinese_TextDisplay = new string[] {
        "重命名&品牌"};
            this.customButtonRenameBrand.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 4),
        new System.Drawing.Point(5, 24)};
            this.customButtonRenameBrand.Chinese_TextNumberInTextGroup = new int[] {
        2};
            this.customButtonRenameBrand.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(55, 23),
        new System.Drawing.Size(39, 23)};
            this.customButtonRenameBrand.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonRenameBrand.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonRenameBrand.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonRenameBrand.CurrentTextGroupIndex = 0;
            this.customButtonRenameBrand.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonRenameBrand.CustomButtonData = null;
            this.customButtonRenameBrand.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonRenameBrand.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonRenameBrand.DrawIcon = true;
            this.customButtonRenameBrand.DrawText = true;
            this.customButtonRenameBrand.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonRenameBrand.English_TextDisplay = new string[] {
        "RENAME&BRAND"};
            this.customButtonRenameBrand.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 4),
        new System.Drawing.Point(5, 24)};
            this.customButtonRenameBrand.English_TextNumberInTextGroup = new int[] {
        2};
            this.customButtonRenameBrand.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(79, 23),
        new System.Drawing.Size(68, 23)};
            this.customButtonRenameBrand.FocusBackgroundDisplay = false;
            this.customButtonRenameBrand.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonRenameBrand.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonRenameBrand.ForeColor = System.Drawing.Color.White;
            this.customButtonRenameBrand.HoverBackgroundDisplay = false;
            this.customButtonRenameBrand.IconIndex = new int[] {
        1,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonRenameBrand.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(86, 7),
        new System.Drawing.Point(86, 7)};
            this.customButtonRenameBrand.IconNumber = 2;
            this.customButtonRenameBrand.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(53, 37),
        new System.Drawing.Size(53, 37)};
            this.customButtonRenameBrand.LabelControlMode = false;
            this.customButtonRenameBrand.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonRenameBrand.Location = new System.Drawing.Point(882, 333);
            this.customButtonRenameBrand.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonRenameBrand.Name = "customButtonRenameBrand";
            this.customButtonRenameBrand.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonRenameBrand.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonRenameBrand.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonRenameBrand.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonRenameBrand.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonRenameBrand.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonRenameBrand.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonRenameBrand.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonRenameBrand.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonRenameBrand.Size = new System.Drawing.Size(129, 51);
            this.customButtonRenameBrand.SizeButton = new System.Drawing.Size(129, 51);
            this.customButtonRenameBrand.TabIndex = 48;
            this.customButtonRenameBrand.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonRenameBrand.TextGroupNumber = 1;
            this.customButtonRenameBrand.UpdateControl = true;
            this.customButtonRenameBrand.CustomButton_Click += new System.EventHandler(this.customButtonRenameBrand_CustomButton_Click);
            // 
            // customButtonCopyBrand
            // 
            this.customButtonCopyBrand.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonCopyBrand.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonCopyBrand.BitmapIconWhole = global::VisionSystemControlLibrary.Properties.Resources.CopyBrand;
            this.customButtonCopyBrand.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.Button;
            this.customButtonCopyBrand.Chinese_TextDisplay = new string[] {
        "复制品牌"};
            this.customButtonCopyBrand.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 13)};
            this.customButtonCopyBrand.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonCopyBrand.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(72, 23)};
            this.customButtonCopyBrand.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonCopyBrand.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonCopyBrand.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonCopyBrand.CurrentTextGroupIndex = 0;
            this.customButtonCopyBrand.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonCopyBrand.CustomButtonData = null;
            this.customButtonCopyBrand.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonCopyBrand.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonCopyBrand.DrawIcon = true;
            this.customButtonCopyBrand.DrawText = true;
            this.customButtonCopyBrand.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonCopyBrand.English_TextDisplay = new string[] {
        "COPY&BRAND"};
            this.customButtonCopyBrand.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 4),
        new System.Drawing.Point(5, 24)};
            this.customButtonCopyBrand.English_TextNumberInTextGroup = new int[] {
        2};
            this.customButtonCopyBrand.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(52, 23),
        new System.Drawing.Size(68, 23)};
            this.customButtonCopyBrand.FocusBackgroundDisplay = false;
            this.customButtonCopyBrand.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonCopyBrand.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonCopyBrand.ForeColor = System.Drawing.Color.White;
            this.customButtonCopyBrand.HoverBackgroundDisplay = false;
            this.customButtonCopyBrand.IconIndex = new int[] {
        1,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonCopyBrand.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(73, 5),
        new System.Drawing.Point(73, 5)};
            this.customButtonCopyBrand.IconNumber = 2;
            this.customButtonCopyBrand.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(53, 37),
        new System.Drawing.Size(53, 37)};
            this.customButtonCopyBrand.LabelControlMode = false;
            this.customButtonCopyBrand.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonCopyBrand.Location = new System.Drawing.Point(882, 279);
            this.customButtonCopyBrand.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonCopyBrand.Name = "customButtonCopyBrand";
            this.customButtonCopyBrand.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonCopyBrand.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonCopyBrand.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonCopyBrand.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonCopyBrand.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonCopyBrand.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonCopyBrand.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonCopyBrand.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonCopyBrand.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonCopyBrand.Size = new System.Drawing.Size(129, 51);
            this.customButtonCopyBrand.SizeButton = new System.Drawing.Size(129, 51);
            this.customButtonCopyBrand.TabIndex = 47;
            this.customButtonCopyBrand.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonCopyBrand.TextGroupNumber = 1;
            this.customButtonCopyBrand.UpdateControl = true;
            this.customButtonCopyBrand.CustomButton_Click += new System.EventHandler(this.customButtonCopyBrand_CustomButton_Click);
            // 
            // customButtonLoadBrand
            // 
            this.customButtonLoadBrand.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonLoadBrand.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonLoadBrand.BitmapIconWhole = global::VisionSystemControlLibrary.Properties.Resources.LoadBrand;
            this.customButtonLoadBrand.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.Button;
            this.customButtonLoadBrand.Chinese_TextDisplay = new string[] {
        "加载品牌&重载品牌"};
            this.customButtonLoadBrand.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 13),
        new System.Drawing.Point(5, 13)};
            this.customButtonLoadBrand.Chinese_TextNumberInTextGroup = new int[] {
        1,
        1};
            this.customButtonLoadBrand.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(72, 23),
        new System.Drawing.Size(72, 23)};
            this.customButtonLoadBrand.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonLoadBrand.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonLoadBrand.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonLoadBrand.CurrentTextGroupIndex = 0;
            this.customButtonLoadBrand.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonLoadBrand.CustomButtonData = null;
            this.customButtonLoadBrand.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonLoadBrand.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonLoadBrand.DrawIcon = true;
            this.customButtonLoadBrand.DrawText = true;
            this.customButtonLoadBrand.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonLoadBrand.English_TextDisplay = new string[] {
        "LOAD&BRAND&RELOAD&BRAND"};
            this.customButtonLoadBrand.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 4),
        new System.Drawing.Point(5, 24),
        new System.Drawing.Point(5, 4),
        new System.Drawing.Point(5, 24)};
            this.customButtonLoadBrand.English_TextNumberInTextGroup = new int[] {
        2,
        2};
            this.customButtonLoadBrand.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(54, 23),
        new System.Drawing.Size(68, 23),
        new System.Drawing.Size(75, 23),
        new System.Drawing.Size(68, 23)};
            this.customButtonLoadBrand.FocusBackgroundDisplay = false;
            this.customButtonLoadBrand.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonLoadBrand.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonLoadBrand.ForeColor = System.Drawing.Color.White;
            this.customButtonLoadBrand.HoverBackgroundDisplay = false;
            this.customButtonLoadBrand.IconIndex = new int[] {
        1,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonLoadBrand.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(75, 7),
        new System.Drawing.Point(75, 7)};
            this.customButtonLoadBrand.IconNumber = 2;
            this.customButtonLoadBrand.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(53, 37),
        new System.Drawing.Size(53, 37)};
            this.customButtonLoadBrand.LabelControlMode = false;
            this.customButtonLoadBrand.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonLoadBrand.Location = new System.Drawing.Point(882, 225);
            this.customButtonLoadBrand.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonLoadBrand.Name = "customButtonLoadBrand";
            this.customButtonLoadBrand.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonLoadBrand.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonLoadBrand.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonLoadBrand.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonLoadBrand.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonLoadBrand.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonLoadBrand.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonLoadBrand.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonLoadBrand.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonLoadBrand.Size = new System.Drawing.Size(129, 51);
            this.customButtonLoadBrand.SizeButton = new System.Drawing.Size(129, 51);
            this.customButtonLoadBrand.TabIndex = 46;
            this.customButtonLoadBrand.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonLoadBrand.TextGroupNumber = 2;
            this.customButtonLoadBrand.UpdateControl = true;
            this.customButtonLoadBrand.CustomButton_Click += new System.EventHandler(this.customButtonLoadBrand_CustomButton_Click);
            // 
            // customButtonSaveCurrent
            // 
            this.customButtonSaveCurrent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonSaveCurrent.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonSaveCurrent.BitmapIconWhole = global::VisionSystemControlLibrary.Properties.Resources.SaveCurrentBrand;
            this.customButtonSaveCurrent.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.Button;
            this.customButtonSaveCurrent.Chinese_TextDisplay = new string[] {
        "保存&当前品牌"};
            this.customButtonSaveCurrent.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 4),
        new System.Drawing.Point(5, 24)};
            this.customButtonSaveCurrent.Chinese_TextNumberInTextGroup = new int[] {
        2};
            this.customButtonSaveCurrent.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(39, 23),
        new System.Drawing.Size(72, 23)};
            this.customButtonSaveCurrent.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonSaveCurrent.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonSaveCurrent.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonSaveCurrent.CurrentTextGroupIndex = 0;
            this.customButtonSaveCurrent.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonSaveCurrent.CustomButtonData = null;
            this.customButtonSaveCurrent.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonSaveCurrent.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonSaveCurrent.DrawIcon = true;
            this.customButtonSaveCurrent.DrawText = true;
            this.customButtonSaveCurrent.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonSaveCurrent.English_TextDisplay = new string[] {
        "SAVE&CURRENT"};
            this.customButtonSaveCurrent.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 4),
        new System.Drawing.Point(5, 24)};
            this.customButtonSaveCurrent.English_TextNumberInTextGroup = new int[] {
        2};
            this.customButtonSaveCurrent.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(49, 23),
        new System.Drawing.Size(86, 23)};
            this.customButtonSaveCurrent.FocusBackgroundDisplay = false;
            this.customButtonSaveCurrent.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonSaveCurrent.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonSaveCurrent.ForeColor = System.Drawing.Color.White;
            this.customButtonSaveCurrent.HoverBackgroundDisplay = false;
            this.customButtonSaveCurrent.IconIndex = new int[] {
        1,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonSaveCurrent.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(70, 5),
        new System.Drawing.Point(70, 5)};
            this.customButtonSaveCurrent.IconNumber = 2;
            this.customButtonSaveCurrent.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(53, 37),
        new System.Drawing.Size(53, 37)};
            this.customButtonSaveCurrent.LabelControlMode = false;
            this.customButtonSaveCurrent.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonSaveCurrent.Location = new System.Drawing.Point(882, 139);
            this.customButtonSaveCurrent.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonSaveCurrent.Name = "customButtonSaveCurrent";
            this.customButtonSaveCurrent.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonSaveCurrent.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonSaveCurrent.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonSaveCurrent.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonSaveCurrent.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonSaveCurrent.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonSaveCurrent.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonSaveCurrent.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonSaveCurrent.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonSaveCurrent.Size = new System.Drawing.Size(129, 51);
            this.customButtonSaveCurrent.SizeButton = new System.Drawing.Size(129, 51);
            this.customButtonSaveCurrent.TabIndex = 45;
            this.customButtonSaveCurrent.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonSaveCurrent.TextGroupNumber = 1;
            this.customButtonSaveCurrent.UpdateControl = true;
            this.customButtonSaveCurrent.CustomButton_Click += new System.EventHandler(this.customButtonSaveCurrent_CustomButton_Click);
            // 
            // customButtonMessage2
            // 
            this.customButtonMessage2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.customButtonMessage2.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.customButtonMessage2.BitmapIconWhole = null;
            this.customButtonMessage2.BitmapWhole = null;
            this.customButtonMessage2.Chinese_TextDisplay = new string[] {
        "（C）= 当前品牌"};
            this.customButtonMessage2.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonMessage2.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonMessage2.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(117, 20)};
            this.customButtonMessage2.ColorTextDisable = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
            this.customButtonMessage2.ColorTextEnable = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonMessage2.ColorTextSelected = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonMessage2.CurrentTextGroupIndex = 0;
            this.customButtonMessage2.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonMessage2.CustomButtonData = null;
            this.customButtonMessage2.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonMessage2.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonMessage2.DrawIcon = false;
            this.customButtonMessage2.DrawText = true;
            this.customButtonMessage2.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonMessage2.English_TextDisplay = new string[] {
        "（C）= CURRENT"};
            this.customButtonMessage2.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonMessage2.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonMessage2.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(130, 20)};
            this.customButtonMessage2.FocusBackgroundDisplay = false;
            this.customButtonMessage2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonMessage2.FontText = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
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
            this.customButtonMessage2.Location = new System.Drawing.Point(870, 105);
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
            this.customButtonMessage2.Size = new System.Drawing.Size(141, 20);
            this.customButtonMessage2.SizeButton = new System.Drawing.Size(141, 20);
            this.customButtonMessage2.TabIndex = 41;
            this.customButtonMessage2.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonMessage2.TextGroupNumber = 1;
            this.customButtonMessage2.UpdateControl = true;
            // 
            // customButtonMessage1
            // 
            this.customButtonMessage1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.customButtonMessage1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.customButtonMessage1.BitmapIconWhole = null;
            this.customButtonMessage1.BitmapWhole = null;
            this.customButtonMessage1.Chinese_TextDisplay = new string[] {
        "（M）= 预设品牌"};
            this.customButtonMessage1.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonMessage1.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonMessage1.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(122, 20)};
            this.customButtonMessage1.ColorTextDisable = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
            this.customButtonMessage1.ColorTextEnable = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
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
        "（M）= MASTER"};
            this.customButtonMessage1.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonMessage1.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonMessage1.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(126, 20)};
            this.customButtonMessage1.FocusBackgroundDisplay = false;
            this.customButtonMessage1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonMessage1.FontText = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
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
            this.customButtonMessage1.Location = new System.Drawing.Point(870, 85);
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
            this.customButtonMessage1.Size = new System.Drawing.Size(141, 20);
            this.customButtonMessage1.SizeButton = new System.Drawing.Size(141, 20);
            this.customButtonMessage1.TabIndex = 40;
            this.customButtonMessage1.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonMessage1.TextGroupNumber = 1;
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
            this.customButtonClose.TabIndex = 39;
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
        "品牌管理"};
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
        "VISION SYSTEM BRAND MANAGEMENT"};
            this.customButtonCaption.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(176, 7)};
            this.customButtonCaption.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonCaption.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(423, 28)};
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
            this.customButtonCaption.TabIndex = 38;
            this.customButtonCaption.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.customButtonCaption.TextGroupNumber = 1;
            this.customButtonCaption.UpdateControl = true;
            // 
            // customList
            // 
            this.customList.BackColor = System.Drawing.Color.Black;
            this.customList.BitmapBackgroundIndex = new int[] {
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
        "品牌名称"};
            this.customList.ColorControlBackground = System.Drawing.Color.Black;
            this.customList.ColorItemBackgroundSelected = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.customList.ColorItemBackgroundUnselected = System.Drawing.Color.Black;
            this.customList.ColorItemTextSelected = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customList.ColorItemTextUnselected = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.customList.ColorListHeaderBackground = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.customList.ColorListHeaderColumnText_Disable = new System.Drawing.Color[] {
        System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))))};
            this.customList.ColorListHeaderColumnText_Enable = new System.Drawing.Color[] {
        System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))))};
            this.customList.ColorListItemBackground = System.Drawing.Color.Black;
            this.customList.ColorPageBackground = System.Drawing.Color.Black;
            this.customList.ColorPageText = System.Drawing.Color.Yellow;
            this.customList.ColumnNameXOffSetValue = 0;
            this.customList.ColumnNumber = 1;
            this.customList.ColumnWidth = new int[] {
        835};
            this.customList.CurrentColumnNameGroupIndex = new int[] {
        0};
            this.customList.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customList.English_ColumnNameDisplay = new string[] {
        "Brand Name"};
            this.customList.FontListHeader = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customList.FontListItem = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customList.FontPage = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customList.ForeColor = System.Drawing.Color.White;
            this.customList.ItemDataDisplay = new bool[] {
        true};
            this.customList.ItemDataNumber = 0;
            this.customList.ItemIconIndex = new int[] {
        -1};
            this.customList.ItemIconNumber = 6;
            this.customList.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customList.ListEnabled = true;
            this.customList.ListHeaderHeight = 36;
            this.customList.ListHeaderTextAlignment = System.Drawing.StringAlignment.Center;
            this.customList.ListItemHeight = 35;
            this.customList.ListItemTextAlignment = System.Drawing.StringAlignment.Center;
            this.customList.ListItemXOffSetValue = 0;
            this.customList.Location = new System.Drawing.Point(21, 138);
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
            this.customList.SelectedItemType = true;
            this.customList.SelectionColumnIndex = -1;
            this.customList.Size = new System.Drawing.Size(846, 484);
            this.customList.SizeControl = new System.Drawing.Size(846, 484);
            this.customList.SizeItemIcon = new System.Drawing.Size[] {
        new System.Drawing.Size(20, 20)};
            this.customList.TabIndex = 54;
            this.customList.CustomListItem_Click += new System.EventHandler(this.customList_CustomListItem_Click);
            // 
            // timerLoadReloadBrand
            // 
            this.timerLoadReloadBrand.Interval = 1000;
            this.timerLoadReloadBrand.Tick += new System.EventHandler(this.timerLoadReloadBrand_Tick);
            // 
            // labelIcon
            // 
            this.labelIcon.BackColor = System.Drawing.Color.Transparent;
            this.labelIcon.Image = global::VisionSystemControlLibrary.Properties.Resources.LoadBrand;
            this.labelIcon.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelIcon.Location = new System.Drawing.Point(28, 17);
            this.labelIcon.Name = "labelIcon";
            this.labelIcon.Size = new System.Drawing.Size(53, 37);
            this.labelIcon.TabIndex = 56;
            // 
            // BrandControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.Controls.Add(this.labelIcon);
            this.Controls.Add(this.customList);
            this.Controls.Add(this.customButtonNextPage);
            this.Controls.Add(this.customButtonPreviousPage);
            this.Controls.Add(this.customButtonRestoreBrands);
            this.Controls.Add(this.customButtonBackupBrands);
            this.Controls.Add(this.customButtonDeleteBrand);
            this.Controls.Add(this.customButtonRenameBrand);
            this.Controls.Add(this.customButtonCopyBrand);
            this.Controls.Add(this.customButtonLoadBrand);
            this.Controls.Add(this.customButtonSaveCurrent);
            this.Controls.Add(this.customButtonMessage2);
            this.Controls.Add(this.customButtonMessage1);
            this.Controls.Add(this.customButtonClose);
            this.Controls.Add(this.customButtonCaption);
            this.Controls.Add(this.labelMessage);
            this.Name = "BrandControl";
            this.Size = new System.Drawing.Size(1024, 662);
            this.Load += new System.EventHandler(this.BrandControl_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelMessage;
        private CustomButton customButtonCaption;
        private CustomButton customButtonClose;
        private CustomButton customButtonMessage1;
        private CustomButton customButtonMessage2;
        private CustomButton customButtonSaveCurrent;
        private CustomButton customButtonLoadBrand;
        private CustomButton customButtonCopyBrand;
        private CustomButton customButtonRenameBrand;
        private CustomButton customButtonDeleteBrand;
        private CustomButton customButtonBackupBrands;
        private CustomButton customButtonRestoreBrands;
        private CustomButton customButtonNextPage;
        private CustomButton customButtonPreviousPage;
        private CustomList customList;
        private System.Windows.Forms.Timer timerLoadReloadBrand;
        private System.Windows.Forms.Label labelIcon;
    }
}
