namespace VisionSystemControlLibrary
{
    partial class TitleBarControl
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
            this.labelCaption = new System.Windows.Forms.Label();
            this.labelBrandValue = new System.Windows.Forms.Label();
            this.labelInspectText = new System.Windows.Forms.Label();
            this.labelStopText = new System.Windows.Forms.Label();
            this.labelReadyText = new System.Windows.Forms.Label();
            this.labelRunText = new System.Windows.Forms.Label();
            this.labelBrandChangedText = new System.Windows.Forms.Label();
            this.labelBrandText = new System.Windows.Forms.Label();
            this.labelStopValue = new System.Windows.Forms.Label();
            this.labelReadyValue = new System.Windows.Forms.Label();
            this.labelRunValue = new System.Windows.Forms.Label();
            this.labelBrandChangedValue = new System.Windows.Forms.Label();
            this.labelTime = new System.Windows.Forms.Label();
            this.labelMachineText = new System.Windows.Forms.Label();
            this.labelMachineValue = new System.Windows.Forms.Label();
            this.labelShiftText = new System.Windows.Forms.Label();
            this.labelShiftValue = new System.Windows.Forms.Label();
            this.labelFaultMessage = new System.Windows.Forms.Label();
            this.customButtonState = new VisionSystemControlLibrary.CustomButton();
            this.customButtonNetCheck = new VisionSystemControlLibrary.CustomButton();
            this.SuspendLayout();
            // 
            // labelCaption
            // 
            this.labelCaption.BackColor = System.Drawing.Color.Transparent;
            this.labelCaption.Font = new System.Drawing.Font("微软雅黑", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelCaption.ForeColor = System.Drawing.Color.Cyan;
            this.labelCaption.Location = new System.Drawing.Point(247, 10);
            this.labelCaption.Name = "labelCaption";
            this.labelCaption.Size = new System.Drawing.Size(520, 80);
            this.labelCaption.TabIndex = 2;
            this.labelCaption.Text = "G.D X6S PRODUCT CHECK";
            this.labelCaption.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelCaption.Click += new System.EventHandler(this.labelCaption_Click);
            // 
            // labelBrandValue
            // 
            this.labelBrandValue.BackColor = System.Drawing.Color.Transparent;
            this.labelBrandValue.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelBrandValue.ForeColor = System.Drawing.Color.Orange;
            this.labelBrandValue.Location = new System.Drawing.Point(852, 48);
            this.labelBrandValue.Name = "labelBrandValue";
            this.labelBrandValue.Size = new System.Drawing.Size(159, 25);
            this.labelBrandValue.TabIndex = 13;
            this.labelBrandValue.Text = "X6S-1";
            this.labelBrandValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelBrandValue.DoubleClick += new System.EventHandler(this.labelBrandValue_DoubleClick);
            // 
            // labelInspectText
            // 
            this.labelInspectText.BackColor = System.Drawing.Color.Transparent;
            this.labelInspectText.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelInspectText.ForeColor = System.Drawing.Color.White;
            this.labelInspectText.Location = new System.Drawing.Point(676, 12);
            this.labelInspectText.Name = "labelInspectText";
            this.labelInspectText.Size = new System.Drawing.Size(85, 25);
            this.labelInspectText.TabIndex = 52;
            this.labelInspectText.Text = "Inspect：";
            this.labelInspectText.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelStopText
            // 
            this.labelStopText.BackColor = System.Drawing.Color.Transparent;
            this.labelStopText.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelStopText.ForeColor = System.Drawing.Color.White;
            this.labelStopText.Location = new System.Drawing.Point(767, 12);
            this.labelStopText.Name = "labelStopText";
            this.labelStopText.Size = new System.Drawing.Size(50, 25);
            this.labelStopText.TabIndex = 53;
            this.labelStopText.Text = "STOP";
            this.labelStopText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelReadyText
            // 
            this.labelReadyText.BackColor = System.Drawing.Color.Transparent;
            this.labelReadyText.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelReadyText.ForeColor = System.Drawing.Color.White;
            this.labelReadyText.Location = new System.Drawing.Point(819, 12);
            this.labelReadyText.Name = "labelReadyText";
            this.labelReadyText.Size = new System.Drawing.Size(60, 25);
            this.labelReadyText.TabIndex = 54;
            this.labelReadyText.Text = "READY";
            this.labelReadyText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelRunText
            // 
            this.labelRunText.BackColor = System.Drawing.Color.Transparent;
            this.labelRunText.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelRunText.ForeColor = System.Drawing.Color.White;
            this.labelRunText.Location = new System.Drawing.Point(887, 12);
            this.labelRunText.Name = "labelRunText";
            this.labelRunText.Size = new System.Drawing.Size(45, 25);
            this.labelRunText.TabIndex = 55;
            this.labelRunText.Text = "RUN";
            this.labelRunText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelBrandChangedText
            // 
            this.labelBrandChangedText.BackColor = System.Drawing.Color.Transparent;
            this.labelBrandChangedText.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelBrandChangedText.ForeColor = System.Drawing.Color.White;
            this.labelBrandChangedText.Location = new System.Drawing.Point(938, 12);
            this.labelBrandChangedText.Name = "labelBrandChangedText";
            this.labelBrandChangedText.Size = new System.Drawing.Size(70, 25);
            this.labelBrandChangedText.TabIndex = 56;
            this.labelBrandChangedText.Text = "B.CHG";
            this.labelBrandChangedText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelBrandText
            // 
            this.labelBrandText.BackColor = System.Drawing.Color.Transparent;
            this.labelBrandText.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelBrandText.ForeColor = System.Drawing.Color.White;
            this.labelBrandText.Location = new System.Drawing.Point(767, 48);
            this.labelBrandText.Name = "labelBrandText";
            this.labelBrandText.Size = new System.Drawing.Size(85, 25);
            this.labelBrandText.TabIndex = 57;
            this.labelBrandText.Text = "Brand：";
            this.labelBrandText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelBrandText.DoubleClick += new System.EventHandler(this.labelBrandText_DoubleClick);
            // 
            // labelStopValue
            // 
            this.labelStopValue.BackColor = System.Drawing.Color.Transparent;
            this.labelStopValue.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelStopValue.ForeColor = System.Drawing.Color.White;
            this.labelStopValue.Location = new System.Drawing.Point(783, 38);
            this.labelStopValue.Name = "labelStopValue";
            this.labelStopValue.Size = new System.Drawing.Size(17, 17);
            this.labelStopValue.TabIndex = 58;
            this.labelStopValue.Text = "V";
            this.labelStopValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelReadyValue
            // 
            this.labelReadyValue.BackColor = System.Drawing.Color.Transparent;
            this.labelReadyValue.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelReadyValue.ForeColor = System.Drawing.Color.White;
            this.labelReadyValue.Location = new System.Drawing.Point(841, 38);
            this.labelReadyValue.Name = "labelReadyValue";
            this.labelReadyValue.Size = new System.Drawing.Size(17, 17);
            this.labelReadyValue.TabIndex = 59;
            this.labelReadyValue.Text = "V";
            this.labelReadyValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelRunValue
            // 
            this.labelRunValue.BackColor = System.Drawing.Color.Transparent;
            this.labelRunValue.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelRunValue.ForeColor = System.Drawing.Color.White;
            this.labelRunValue.Location = new System.Drawing.Point(900, 38);
            this.labelRunValue.Name = "labelRunValue";
            this.labelRunValue.Size = new System.Drawing.Size(17, 17);
            this.labelRunValue.TabIndex = 60;
            this.labelRunValue.Text = "V";
            this.labelRunValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelBrandChangedValue
            // 
            this.labelBrandChangedValue.BackColor = System.Drawing.Color.Transparent;
            this.labelBrandChangedValue.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelBrandChangedValue.ForeColor = System.Drawing.Color.White;
            this.labelBrandChangedValue.Location = new System.Drawing.Point(964, 38);
            this.labelBrandChangedValue.Name = "labelBrandChangedValue";
            this.labelBrandChangedValue.Size = new System.Drawing.Size(17, 17);
            this.labelBrandChangedValue.TabIndex = 61;
            this.labelBrandChangedValue.Text = "V";
            this.labelBrandChangedValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelTime
            // 
            this.labelTime.BackColor = System.Drawing.Color.Transparent;
            this.labelTime.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelTime.ForeColor = System.Drawing.Color.White;
            this.labelTime.Location = new System.Drawing.Point(767, 4);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(223, 25);
            this.labelTime.TabIndex = 62;
            this.labelTime.Text = "0001.01.01 00;00:00";
            this.labelTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelTime.DoubleClick += new System.EventHandler(this.labelTime_DoubleClick);
            // 
            // labelMachineText
            // 
            this.labelMachineText.BackColor = System.Drawing.Color.Transparent;
            this.labelMachineText.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelMachineText.ForeColor = System.Drawing.Color.White;
            this.labelMachineText.Location = new System.Drawing.Point(767, 26);
            this.labelMachineText.Name = "labelMachineText";
            this.labelMachineText.Size = new System.Drawing.Size(82, 25);
            this.labelMachineText.TabIndex = 74;
            this.labelMachineText.Text = "Machine：";
            this.labelMachineText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelMachineText.DoubleClick += new System.EventHandler(this.labelMachineText_DoubleClick);
            // 
            // labelMachineValue
            // 
            this.labelMachineValue.BackColor = System.Drawing.Color.Transparent;
            this.labelMachineValue.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelMachineValue.ForeColor = System.Drawing.Color.Orange;
            this.labelMachineValue.Location = new System.Drawing.Point(852, 26);
            this.labelMachineValue.Name = "labelMachineValue";
            this.labelMachineValue.Size = new System.Drawing.Size(159, 25);
            this.labelMachineValue.TabIndex = 73;
            this.labelMachineValue.Tag = "28";
            this.labelMachineValue.Text = "ZB28";
            this.labelMachineValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelMachineValue.DoubleClick += new System.EventHandler(this.labelMachineValue_DoubleClick);
            // 
            // labelShiftText
            // 
            this.labelShiftText.BackColor = System.Drawing.Color.Transparent;
            this.labelShiftText.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelShiftText.ForeColor = System.Drawing.Color.White;
            this.labelShiftText.Location = new System.Drawing.Point(767, 70);
            this.labelShiftText.Name = "labelShiftText";
            this.labelShiftText.Size = new System.Drawing.Size(82, 25);
            this.labelShiftText.TabIndex = 76;
            this.labelShiftText.Text = "Shift：";
            this.labelShiftText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelShiftText.DoubleClick += new System.EventHandler(this.labelShiftText_DoubleClick);
            // 
            // labelShiftValue
            // 
            this.labelShiftValue.BackColor = System.Drawing.Color.Transparent;
            this.labelShiftValue.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelShiftValue.ForeColor = System.Drawing.Color.Orange;
            this.labelShiftValue.Location = new System.Drawing.Point(852, 70);
            this.labelShiftValue.Name = "labelShiftValue";
            this.labelShiftValue.Size = new System.Drawing.Size(159, 25);
            this.labelShiftValue.TabIndex = 75;
            this.labelShiftValue.Text = "Cur. Shift";
            this.labelShiftValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelShiftValue.DoubleClick += new System.EventHandler(this.labelShiftValue_DoubleClick);
            // 
            // labelFaultMessage
            // 
            this.labelFaultMessage.BackColor = System.Drawing.Color.Transparent;
            this.labelFaultMessage.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelFaultMessage.ForeColor = System.Drawing.Color.White;
            this.labelFaultMessage.Location = new System.Drawing.Point(252, 10);
            this.labelFaultMessage.Name = "labelFaultMessage";
            this.labelFaultMessage.Size = new System.Drawing.Size(510, 80);
            this.labelFaultMessage.TabIndex = 77;
            this.labelFaultMessage.Text = "FAULT MESSAGE";
            this.labelFaultMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelFaultMessage.Visible = false;
            this.labelFaultMessage.Click += new System.EventHandler(this.labelFaultMessage_Click);
            // 
            // customButtonState
            // 
            this.customButtonState.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonState.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonState.BitmapIconWhole = global::VisionSystemControlLibrary.Properties.Resources.RunStop;
            this.customButtonState.BitmapWhole = null;
            this.customButtonState.Chinese_TextDisplay = new string[] {
        "状态&状态"};
            this.customButtonState.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(35, 50),
        new System.Drawing.Point(35, 50)};
            this.customButtonState.Chinese_TextNumberInTextGroup = new int[] {
        1,
        1};
            this.customButtonState.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(39, 23),
        new System.Drawing.Size(39, 23)};
            this.customButtonState.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonState.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonState.ColorTextSelected = System.Drawing.Color.White;
            this.customButtonState.CurrentTextGroupIndex = 0;
            this.customButtonState.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Selected;
            this.customButtonState.CustomButtonData = null;
            this.customButtonState.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonState.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Switch;
            this.customButtonState.DrawIcon = true;
            this.customButtonState.DrawText = true;
            this.customButtonState.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonState.English_TextDisplay = new string[] {
        "STATE&STATE"};
            this.customButtonState.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(26, 50),
        new System.Drawing.Point(26, 50)};
            this.customButtonState.English_TextNumberInTextGroup = new int[] {
        1,
        1};
            this.customButtonState.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(58, 23),
        new System.Drawing.Size(58, 23)};
            this.customButtonState.FocusBackgroundDisplay = false;
            this.customButtonState.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonState.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonState.ForeColor = System.Drawing.Color.White;
            this.customButtonState.HoverBackgroundDisplay = false;
            this.customButtonState.IconIndex = new int[] {
        3,
        2,
        1,
        0,
        0,
        1,
        0,
        1,
        0};
            this.customButtonState.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(32, 10),
        new System.Drawing.Point(32, 10),
        new System.Drawing.Point(32, 10),
        new System.Drawing.Point(32, 10)};
            this.customButtonState.IconNumber = 4;
            this.customButtonState.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(53, 37),
        new System.Drawing.Size(53, 37),
        new System.Drawing.Size(53, 37),
        new System.Drawing.Size(53, 37)};
            this.customButtonState.LabelControlMode = false;
            this.customButtonState.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonState.Location = new System.Drawing.Point(121, 9);
            this.customButtonState.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonState.Name = "customButtonState";
            this.customButtonState.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonState.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonState.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonState.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonState.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonState.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonState.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonState.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonState.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonState.Size = new System.Drawing.Size(110, 80);
            this.customButtonState.SizeButton = new System.Drawing.Size(110, 80);
            this.customButtonState.TabIndex = 45;
            this.customButtonState.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.customButtonState.TextGroupNumber = 2;
            this.customButtonState.UpdateControl = true;
            this.customButtonState.CustomButton_Click += new System.EventHandler(this.customButtonState_CustomButton_Click);
            // 
            // customButtonNetCheck
            // 
            this.customButtonNetCheck.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonNetCheck.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonNetCheck.BitmapIconWhole = global::VisionSystemControlLibrary.Properties.Resources.NetCheck;
            this.customButtonNetCheck.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.Button;
            this.customButtonNetCheck.Chinese_TextDisplay = new string[] {
        "网络检测"};
            this.customButtonNetCheck.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(19, 50)};
            this.customButtonNetCheck.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonNetCheck.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(72, 23)};
            this.customButtonNetCheck.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonNetCheck.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonNetCheck.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonNetCheck.CurrentTextGroupIndex = 0;
            this.customButtonNetCheck.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonNetCheck.CustomButtonData = null;
            this.customButtonNetCheck.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonNetCheck.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonNetCheck.DrawIcon = true;
            this.customButtonNetCheck.DrawText = true;
            this.customButtonNetCheck.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonNetCheck.English_TextDisplay = new string[] {
        "NET CHECK"};
            this.customButtonNetCheck.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 50)};
            this.customButtonNetCheck.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonNetCheck.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(101, 23)};
            this.customButtonNetCheck.FocusBackgroundDisplay = false;
            this.customButtonNetCheck.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonNetCheck.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonNetCheck.ForeColor = System.Drawing.Color.White;
            this.customButtonNetCheck.HoverBackgroundDisplay = false;
            this.customButtonNetCheck.IconIndex = new int[] {
        1,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonNetCheck.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(30, 10),
        new System.Drawing.Point(30, 10)};
            this.customButtonNetCheck.IconNumber = 2;
            this.customButtonNetCheck.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(53, 37),
        new System.Drawing.Size(53, 37)};
            this.customButtonNetCheck.LabelControlMode = false;
            this.customButtonNetCheck.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonNetCheck.Location = new System.Drawing.Point(8, 9);
            this.customButtonNetCheck.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonNetCheck.Name = "customButtonNetCheck";
            this.customButtonNetCheck.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonNetCheck.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonNetCheck.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonNetCheck.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonNetCheck.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonNetCheck.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonNetCheck.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonNetCheck.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonNetCheck.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonNetCheck.Size = new System.Drawing.Size(110, 80);
            this.customButtonNetCheck.SizeButton = new System.Drawing.Size(110, 80);
            this.customButtonNetCheck.TabIndex = 44;
            this.customButtonNetCheck.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.customButtonNetCheck.TextGroupNumber = 1;
            this.customButtonNetCheck.UpdateControl = true;
            this.customButtonNetCheck.CustomButton_Click += new System.EventHandler(this.customButtonNetCheck_CustomButton_Click);
            // 
            // TitleBarControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.BackgroundImage = global::VisionSystemControlLibrary.Properties.Resources.TitleBarBackground;
            this.Controls.Add(this.labelFaultMessage);
            this.Controls.Add(this.labelInspectText);
            this.Controls.Add(this.labelTime);
            this.Controls.Add(this.labelMachineText);
            this.Controls.Add(this.labelMachineValue);
            this.Controls.Add(this.labelShiftText);
            this.Controls.Add(this.labelShiftValue);
            this.Controls.Add(this.labelBrandChangedValue);
            this.Controls.Add(this.labelRunValue);
            this.Controls.Add(this.labelReadyValue);
            this.Controls.Add(this.labelStopValue);
            this.Controls.Add(this.labelBrandText);
            this.Controls.Add(this.labelBrandChangedText);
            this.Controls.Add(this.labelRunText);
            this.Controls.Add(this.labelReadyText);
            this.Controls.Add(this.labelStopText);
            this.Controls.Add(this.labelBrandValue);
            this.Controls.Add(this.customButtonState);
            this.Controls.Add(this.customButtonNetCheck);
            this.Controls.Add(this.labelCaption);
            this.Name = "TitleBarControl";
            this.Size = new System.Drawing.Size(1014, 100);
            this.Load += new System.EventHandler(this.TitleBarControl_Load);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.TitleBarControl_MouseDoubleClick);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelCaption;
        private System.Windows.Forms.Label labelBrandValue;
        private CustomButton customButtonNetCheck;
        private CustomButton customButtonState;
        private System.Windows.Forms.Label labelInspectText;
        private System.Windows.Forms.Label labelStopText;
        private System.Windows.Forms.Label labelReadyText;
        private System.Windows.Forms.Label labelRunText;
        private System.Windows.Forms.Label labelBrandChangedText;
        private System.Windows.Forms.Label labelBrandText;
        private System.Windows.Forms.Label labelStopValue;
        private System.Windows.Forms.Label labelReadyValue;
        private System.Windows.Forms.Label labelRunValue;
        private System.Windows.Forms.Label labelBrandChangedValue;
        private System.Windows.Forms.Label labelTime;
        private System.Windows.Forms.Label labelMachineText;
        private System.Windows.Forms.Label labelMachineValue;
        private System.Windows.Forms.Label labelShiftText;
        private System.Windows.Forms.Label labelShiftValue;
        private System.Windows.Forms.Label labelFaultMessage;
    }
}
