namespace VisionSystemControlLibrary
{
    partial class QualityCheckControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QualityCheckControl));
            this.labelBestContrastValue = new System.Windows.Forms.Label();
            this.labelHandleLeftTop = new System.Windows.Forms.Label();
            this.labelHandleTop = new System.Windows.Forms.Label();
            this.labelHandleRightTop = new System.Windows.Forms.Label();
            this.labelHandleLeft = new System.Windows.Forms.Label();
            this.labelHandleRight = new System.Windows.Forms.Label();
            this.labelHandleLeftBottom = new System.Windows.Forms.Label();
            this.labelHandleBottom = new System.Windows.Forms.Label();
            this.labelHandleRightBottom = new System.Windows.Forms.Label();
            this.labelCheckTimeValue = new System.Windows.Forms.Label();
            this.labelIcon = new System.Windows.Forms.Label();
            this.customButtonEditTools = new VisionSystemControlLibrary.CustomButton();
            this.sizeAdjustedPanel = new VisionSystemControlLibrary.SizeAdjustedPanel();
            this.parameterSettingsPanel = new VisionSystemControlLibrary.ParameterSettingsPanel();
            this.customButtonBestContrastText = new VisionSystemControlLibrary.CustomButton();
            this.customButtonDeltaYValue = new VisionSystemControlLibrary.CustomButton();
            this.customButtonDeltaYText = new VisionSystemControlLibrary.CustomButton();
            this.customButtonDeltaXValue = new VisionSystemControlLibrary.CustomButton();
            this.customButtonDeltaXText = new VisionSystemControlLibrary.CustomButton();
            this.customButtonColorValue = new VisionSystemControlLibrary.CustomButton();
            this.customButtonColorText = new VisionSystemControlLibrary.CustomButton();
            this.customButtonAxisValue = new VisionSystemControlLibrary.CustomButton();
            this.customButtonAxisText = new VisionSystemControlLibrary.CustomButton();
            this.customButtonMeasureTool = new VisionSystemControlLibrary.CustomButton();
            this.customButtonNext = new VisionSystemControlLibrary.CustomButton();
            this.customButtonPrevious = new VisionSystemControlLibrary.CustomButton();
            this.customButtonRejectImageIndex = new VisionSystemControlLibrary.CustomButton();
            this.customButtonStatusBar = new VisionSystemControlLibrary.CustomButton();
            this.customButtonSubtract = new VisionSystemControlLibrary.CustomButton();
            this.customButtonPlus = new VisionSystemControlLibrary.CustomButton();
            this.customButtonLoadReject = new VisionSystemControlLibrary.CustomButton();
            this.customButtonManageTools = new VisionSystemControlLibrary.CustomButton();
            this.customButtonLoadSample = new VisionSystemControlLibrary.CustomButton();
            this.customButtonSaveProduct = new VisionSystemControlLibrary.CustomButton();
            this.customButtonLearnSample = new VisionSystemControlLibrary.CustomButton();
            this.customButtonLiveView = new VisionSystemControlLibrary.CustomButton();
            this.customButtonSelectedCheckValue = new VisionSystemControlLibrary.CustomButton();
            this.customButtonCheckTimeText = new VisionSystemControlLibrary.CustomButton();
            this.customButtonSelectedCheckText = new VisionSystemControlLibrary.CustomButton();
            this.customButtonClose = new VisionSystemControlLibrary.CustomButton();
            this.customButtonCaption = new VisionSystemControlLibrary.CustomButton();
            this.customListTool = new VisionSystemControlLibrary.CustomList();
            this.imageDisplayView = new VisionSystemControlLibrary.ImageDisplay();
            this.customButtonImageDisplayViewBackground = new VisionSystemControlLibrary.CustomButton();
            this.labelROIXY = new System.Windows.Forms.Label();
            this.labelROIXYValue = new System.Windows.Forms.Label();
            this.labelROIWHValue = new System.Windows.Forms.Label();
            this.labelROIWH = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelBestContrastValue
            // 
            this.labelBestContrastValue.BackColor = System.Drawing.Color.Blue;
            this.labelBestContrastValue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelBestContrastValue.ForeColor = System.Drawing.Color.White;
            this.labelBestContrastValue.Location = new System.Drawing.Point(652, 632);
            this.labelBestContrastValue.Name = "labelBestContrastValue";
            this.labelBestContrastValue.Size = new System.Drawing.Size(60, 20);
            this.labelBestContrastValue.TabIndex = 39;
            this.labelBestContrastValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelBestContrastValue.Visible = false;
            // 
            // labelHandleLeftTop
            // 
            this.labelHandleLeftTop.BackColor = System.Drawing.Color.Blue;
            this.labelHandleLeftTop.Location = new System.Drawing.Point(338, 212);
            this.labelHandleLeftTop.Name = "labelHandleLeftTop";
            this.labelHandleLeftTop.Size = new System.Drawing.Size(25, 25);
            this.labelHandleLeftTop.TabIndex = 41;
            this.labelHandleLeftTop.Visible = false;
            this.labelHandleLeftTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.labelHandleLeftTop_MouseDown);
            this.labelHandleLeftTop.MouseMove += new System.Windows.Forms.MouseEventHandler(this.labelHandleLeftTop_MouseMove);
            this.labelHandleLeftTop.MouseUp += new System.Windows.Forms.MouseEventHandler(this.labelHandleLeftTop_MouseUp);
            // 
            // labelHandleTop
            // 
            this.labelHandleTop.BackColor = System.Drawing.Color.Blue;
            this.labelHandleTop.Location = new System.Drawing.Point(446, 212);
            this.labelHandleTop.Name = "labelHandleTop";
            this.labelHandleTop.Size = new System.Drawing.Size(25, 25);
            this.labelHandleTop.TabIndex = 41;
            this.labelHandleTop.Visible = false;
            this.labelHandleTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.labelHandleTop_MouseDown);
            this.labelHandleTop.MouseMove += new System.Windows.Forms.MouseEventHandler(this.labelHandleTop_MouseMove);
            this.labelHandleTop.MouseUp += new System.Windows.Forms.MouseEventHandler(this.labelHandleTop_MouseUp);
            // 
            // labelHandleRightTop
            // 
            this.labelHandleRightTop.BackColor = System.Drawing.Color.Blue;
            this.labelHandleRightTop.Location = new System.Drawing.Point(570, 212);
            this.labelHandleRightTop.Name = "labelHandleRightTop";
            this.labelHandleRightTop.Size = new System.Drawing.Size(25, 25);
            this.labelHandleRightTop.TabIndex = 41;
            this.labelHandleRightTop.Visible = false;
            this.labelHandleRightTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.labelHandleRightTop_MouseDown);
            this.labelHandleRightTop.MouseMove += new System.Windows.Forms.MouseEventHandler(this.labelHandleRightTop_MouseMove);
            this.labelHandleRightTop.MouseUp += new System.Windows.Forms.MouseEventHandler(this.labelHandleRightTop_MouseUp);
            // 
            // labelHandleLeft
            // 
            this.labelHandleLeft.BackColor = System.Drawing.Color.Blue;
            this.labelHandleLeft.Location = new System.Drawing.Point(338, 272);
            this.labelHandleLeft.Name = "labelHandleLeft";
            this.labelHandleLeft.Size = new System.Drawing.Size(25, 25);
            this.labelHandleLeft.TabIndex = 41;
            this.labelHandleLeft.Visible = false;
            this.labelHandleLeft.MouseDown += new System.Windows.Forms.MouseEventHandler(this.labelHandleLeft_MouseDown);
            this.labelHandleLeft.MouseMove += new System.Windows.Forms.MouseEventHandler(this.labelHandleLeft_MouseMove);
            this.labelHandleLeft.MouseUp += new System.Windows.Forms.MouseEventHandler(this.labelHandleLeft_MouseUp);
            // 
            // labelHandleRight
            // 
            this.labelHandleRight.BackColor = System.Drawing.Color.Blue;
            this.labelHandleRight.Location = new System.Drawing.Point(570, 272);
            this.labelHandleRight.Name = "labelHandleRight";
            this.labelHandleRight.Size = new System.Drawing.Size(25, 25);
            this.labelHandleRight.TabIndex = 41;
            this.labelHandleRight.Visible = false;
            this.labelHandleRight.MouseDown += new System.Windows.Forms.MouseEventHandler(this.labelHandleRight_MouseDown);
            this.labelHandleRight.MouseMove += new System.Windows.Forms.MouseEventHandler(this.labelHandleRight_MouseMove);
            this.labelHandleRight.MouseUp += new System.Windows.Forms.MouseEventHandler(this.labelHandleRight_MouseUp);
            // 
            // labelHandleLeftBottom
            // 
            this.labelHandleLeftBottom.BackColor = System.Drawing.Color.Blue;
            this.labelHandleLeftBottom.Location = new System.Drawing.Point(338, 327);
            this.labelHandleLeftBottom.Name = "labelHandleLeftBottom";
            this.labelHandleLeftBottom.Size = new System.Drawing.Size(25, 25);
            this.labelHandleLeftBottom.TabIndex = 41;
            this.labelHandleLeftBottom.Visible = false;
            this.labelHandleLeftBottom.MouseDown += new System.Windows.Forms.MouseEventHandler(this.labelHandleLeftBottom_MouseDown);
            this.labelHandleLeftBottom.MouseMove += new System.Windows.Forms.MouseEventHandler(this.labelHandleLeftBottom_MouseMove);
            this.labelHandleLeftBottom.MouseUp += new System.Windows.Forms.MouseEventHandler(this.labelHandleLeftBottom_MouseUp);
            // 
            // labelHandleBottom
            // 
            this.labelHandleBottom.BackColor = System.Drawing.Color.Blue;
            this.labelHandleBottom.Location = new System.Drawing.Point(446, 327);
            this.labelHandleBottom.Name = "labelHandleBottom";
            this.labelHandleBottom.Size = new System.Drawing.Size(25, 25);
            this.labelHandleBottom.TabIndex = 41;
            this.labelHandleBottom.Visible = false;
            this.labelHandleBottom.MouseDown += new System.Windows.Forms.MouseEventHandler(this.labelHandleBottom_MouseDown);
            this.labelHandleBottom.MouseMove += new System.Windows.Forms.MouseEventHandler(this.labelHandleBottom_MouseMove);
            this.labelHandleBottom.MouseUp += new System.Windows.Forms.MouseEventHandler(this.labelHandleBottom_MouseUp);
            // 
            // labelHandleRightBottom
            // 
            this.labelHandleRightBottom.BackColor = System.Drawing.Color.Blue;
            this.labelHandleRightBottom.Location = new System.Drawing.Point(570, 327);
            this.labelHandleRightBottom.Name = "labelHandleRightBottom";
            this.labelHandleRightBottom.Size = new System.Drawing.Size(25, 25);
            this.labelHandleRightBottom.TabIndex = 41;
            this.labelHandleRightBottom.Visible = false;
            this.labelHandleRightBottom.MouseDown += new System.Windows.Forms.MouseEventHandler(this.labelHandleRightBottom_MouseDown);
            this.labelHandleRightBottom.MouseMove += new System.Windows.Forms.MouseEventHandler(this.labelHandleRightBottom_MouseMove);
            this.labelHandleRightBottom.MouseUp += new System.Windows.Forms.MouseEventHandler(this.labelHandleRightBottom_MouseUp);
            // 
            // labelCheckTimeValue
            // 
            this.labelCheckTimeValue.BackColor = System.Drawing.Color.Black;
            this.labelCheckTimeValue.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelCheckTimeValue.ForeColor = System.Drawing.Color.Yellow;
            this.labelCheckTimeValue.Location = new System.Drawing.Point(940, 69);
            this.labelCheckTimeValue.Name = "labelCheckTimeValue";
            this.labelCheckTimeValue.Size = new System.Drawing.Size(74, 40);
            this.labelCheckTimeValue.TabIndex = 68;
            this.labelCheckTimeValue.Text = "100";
            this.labelCheckTimeValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelIcon
            // 
            this.labelIcon.BackColor = System.Drawing.Color.Transparent;
            this.labelIcon.Image = global::VisionSystemControlLibrary.Properties.Resources.Quality;
            this.labelIcon.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelIcon.Location = new System.Drawing.Point(32, 17);
            this.labelIcon.Name = "labelIcon";
            this.labelIcon.Size = new System.Drawing.Size(53, 37);
            this.labelIcon.TabIndex = 95;
            // 
            // customButtonEditTools
            // 
            this.customButtonEditTools.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonEditTools.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonEditTools.BitmapIconWhole = global::VisionSystemControlLibrary.Properties.Resources.RenameBrand;
            this.customButtonEditTools.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.Button;
            this.customButtonEditTools.Chinese_TextDisplay = new string[] {
        "设置&外部区域"};
            this.customButtonEditTools.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 7),
        new System.Drawing.Point(5, 27)};
            this.customButtonEditTools.Chinese_TextNumberInTextGroup = new int[] {
        2};
            this.customButtonEditTools.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(39, 23),
        new System.Drawing.Size(72, 23)};
            this.customButtonEditTools.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonEditTools.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonEditTools.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonEditTools.CurrentTextGroupIndex = 0;
            this.customButtonEditTools.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonEditTools.CustomButtonData = null;
            this.customButtonEditTools.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonEditTools.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Switch;
            this.customButtonEditTools.DrawIcon = true;
            this.customButtonEditTools.DrawText = true;
            this.customButtonEditTools.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonEditTools.English_TextDisplay = new string[] {
        "SET&Extra ROI"};
            this.customButtonEditTools.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 7),
        new System.Drawing.Point(5, 27)};
            this.customButtonEditTools.English_TextNumberInTextGroup = new int[] {
        2};
            this.customButtonEditTools.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(35, 23),
        new System.Drawing.Size(84, 23)};
            this.customButtonEditTools.FocusBackgroundDisplay = false;
            this.customButtonEditTools.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonEditTools.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonEditTools.ForeColor = System.Drawing.Color.White;
            this.customButtonEditTools.HoverBackgroundDisplay = false;
            this.customButtonEditTools.IconIndex = new int[] {
        1,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonEditTools.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(95, 10),
        new System.Drawing.Point(95, 10)};
            this.customButtonEditTools.IconNumber = 2;
            this.customButtonEditTools.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(53, 37),
        new System.Drawing.Size(53, 37)};
            this.customButtonEditTools.LabelControlMode = false;
            this.customButtonEditTools.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonEditTools.Location = new System.Drawing.Point(878, 432);
            this.customButtonEditTools.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonEditTools.Name = "customButtonEditTools";
            this.customButtonEditTools.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonEditTools.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonEditTools.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonEditTools.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonEditTools.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonEditTools.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonEditTools.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonEditTools.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonEditTools.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonEditTools.Size = new System.Drawing.Size(140, 56);
            this.customButtonEditTools.SizeButton = new System.Drawing.Size(140, 56);
            this.customButtonEditTools.TabIndex = 97;
            this.customButtonEditTools.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonEditTools.TextGroupNumber = 1;
            this.customButtonEditTools.UpdateControl = true;
            this.customButtonEditTools.Visible = false;
            this.customButtonEditTools.CustomButton_Click += new System.EventHandler(this.customButtonEditTools_CustomButton_Click);
            // 
            // sizeAdjustedPanel
            // 
            this.sizeAdjustedPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.sizeAdjustedPanel.ControlEnabled = true;
            this.sizeAdjustedPanel.HandleType = System.Drawing.ContentAlignment.TopLeft;
            this.sizeAdjustedPanel.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.sizeAdjustedPanel.Location = new System.Drawing.Point(7, 117);
            this.sizeAdjustedPanel.Name = "sizeAdjustedPanel";
            this.sizeAdjustedPanel.RectangleMaxROI = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.sizeAdjustedPanel.RectangleROI = ((VisionSystemClassLibrary.Struct.ROI)(resources.GetObject("sizeAdjustedPanel.RectangleROI")));
            this.sizeAdjustedPanel.SelectionType = VisionSystemControlLibrary.RegionSelectionType.None;
            this.sizeAdjustedPanel.ShowHideButton = false;
            this.sizeAdjustedPanel.Size = new System.Drawing.Size(215, 480);
            this.sizeAdjustedPanel.TabIndex = 59;
            this.sizeAdjustedPanel.Visible = false;
            this.sizeAdjustedPanel.Top_Click += new System.EventHandler(this.sizeAdjustedPanel_Top_Click);
            this.sizeAdjustedPanel.Bottom_Click += new System.EventHandler(this.sizeAdjustedPanel_Bottom_Click);
            this.sizeAdjustedPanel.Left_Click += new System.EventHandler(this.sizeAdjustedPanel_Left_Click);
            this.sizeAdjustedPanel.Right_Click += new System.EventHandler(this.sizeAdjustedPanel_Right_Click);
            this.sizeAdjustedPanel.LeftTop_Click += new System.EventHandler(this.sizeAdjustedPanel_LeftTop_Click);
            this.sizeAdjustedPanel.RightTop_Click += new System.EventHandler(this.sizeAdjustedPanel_RightTop_Click);
            this.sizeAdjustedPanel.LeftBottom_Click += new System.EventHandler(this.sizeAdjustedPanel_LeftBottom_Click);
            this.sizeAdjustedPanel.RightBottom_Click += new System.EventHandler(this.sizeAdjustedPanel_RightBottom_Click);
            this.sizeAdjustedPanel.Center_Click += new System.EventHandler(this.sizeAdjustedPanel_Center_Click);
            this.sizeAdjustedPanel.Center_MouseMove += new System.EventHandler(this.sizeAdjustedPanel_Center_MouseMove);
            this.sizeAdjustedPanel.Close_Click += new System.EventHandler(this.sizeAdjustedPanel_Close_Click);
            this.sizeAdjustedPanel.Home_Click += new System.EventHandler(this.sizeAdjustedPanel_Home_Click);
            this.sizeAdjustedPanel.RegionSelection_Click += new System.EventHandler(this.sizeAdjustedPanel_RegionSelection_Click);
            this.sizeAdjustedPanel.ShowHide_Click += new System.EventHandler(this.sizeAdjustedPanel_ShowHide_Click);
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
        null,
        null,
        null,
        null};
            this.parameterSettingsPanel.EnterValueEnabled = true;
            this.parameterSettingsPanel.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.parameterSettingsPanel.Location = new System.Drawing.Point(5, 172);
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
        2,
        2};
            this.parameterSettingsPanel.ShowSettingsButton = true;
            this.parameterSettingsPanel.Size = new System.Drawing.Size(220, 450);
            this.parameterSettingsPanel.TabIndex = 58;
            this.parameterSettingsPanel.WindowParameter = 1;
            this.parameterSettingsPanel.ParameterValueChanged += new System.EventHandler(this.parameterSettingsPanel_ParameterValueChanged);
            // 
            // customButtonBestContrastText
            // 
            this.customButtonBestContrastText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.customButtonBestContrastText.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.customButtonBestContrastText.BitmapIconWhole = null;
            this.customButtonBestContrastText.BitmapWhole = null;
            this.customButtonBestContrastText.Chinese_TextDisplay = new string[] {
        "最佳对比色"};
            this.customButtonBestContrastText.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(14, 1)};
            this.customButtonBestContrastText.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonBestContrastText.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(81, 21)};
            this.customButtonBestContrastText.ColorTextDisable = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
            this.customButtonBestContrastText.ColorTextEnable = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonBestContrastText.ColorTextSelected = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonBestContrastText.CurrentTextGroupIndex = 0;
            this.customButtonBestContrastText.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonBestContrastText.CustomButtonData = null;
            this.customButtonBestContrastText.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonBestContrastText.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonBestContrastText.DrawIcon = false;
            this.customButtonBestContrastText.DrawText = true;
            this.customButtonBestContrastText.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonBestContrastText.English_TextDisplay = new string[] {
        "Best Contrast"};
            this.customButtonBestContrastText.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(1, 1)};
            this.customButtonBestContrastText.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonBestContrastText.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(108, 21)};
            this.customButtonBestContrastText.FocusBackgroundDisplay = false;
            this.customButtonBestContrastText.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonBestContrastText.FontText = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonBestContrastText.ForeColor = System.Drawing.Color.White;
            this.customButtonBestContrastText.HoverBackgroundDisplay = false;
            this.customButtonBestContrastText.IconIndex = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonBestContrastText.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonBestContrastText.IconNumber = 1;
            this.customButtonBestContrastText.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(0, 0)};
            this.customButtonBestContrastText.LabelControlMode = true;
            this.customButtonBestContrastText.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonBestContrastText.Location = new System.Drawing.Point(626, 607);
            this.customButtonBestContrastText.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonBestContrastText.Name = "customButtonBestContrastText";
            this.customButtonBestContrastText.RectBottom = new System.Drawing.Rectangle(3, 23, 237, 3);
            this.customButtonBestContrastText.RectFill = new System.Drawing.Rectangle(3, 3, 237, 20);
            this.customButtonBestContrastText.RectLeft = new System.Drawing.Rectangle(0, 3, 3, 20);
            this.customButtonBestContrastText.RectLeftBottom = new System.Drawing.Rectangle(0, 23, 3, 3);
            this.customButtonBestContrastText.RectLeftTop = new System.Drawing.Rectangle(0, 0, 3, 3);
            this.customButtonBestContrastText.RectRight = new System.Drawing.Rectangle(240, 3, 3, 20);
            this.customButtonBestContrastText.RectRightBottom = new System.Drawing.Rectangle(240, 23, 3, 3);
            this.customButtonBestContrastText.RectRightTop = new System.Drawing.Rectangle(240, 0, 3, 3);
            this.customButtonBestContrastText.RectTop = new System.Drawing.Rectangle(3, 0, 237, 3);
            this.customButtonBestContrastText.Size = new System.Drawing.Size(110, 25);
            this.customButtonBestContrastText.SizeButton = new System.Drawing.Size(110, 25);
            this.customButtonBestContrastText.TabIndex = 91;
            this.customButtonBestContrastText.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.customButtonBestContrastText.TextGroupNumber = 1;
            this.customButtonBestContrastText.UpdateControl = true;
            this.customButtonBestContrastText.Visible = false;
            // 
            // customButtonDeltaYValue
            // 
            this.customButtonDeltaYValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.customButtonDeltaYValue.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.customButtonDeltaYValue.BitmapIconWhole = null;
            this.customButtonDeltaYValue.BitmapWhole = null;
            this.customButtonDeltaYValue.Chinese_TextDisplay = new string[] {
        "1024"};
            this.customButtonDeltaYValue.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 1)};
            this.customButtonDeltaYValue.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonDeltaYValue.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(43, 22)};
            this.customButtonDeltaYValue.ColorTextDisable = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
            this.customButtonDeltaYValue.ColorTextEnable = System.Drawing.Color.Yellow;
            this.customButtonDeltaYValue.ColorTextSelected = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonDeltaYValue.CurrentTextGroupIndex = 0;
            this.customButtonDeltaYValue.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonDeltaYValue.CustomButtonData = null;
            this.customButtonDeltaYValue.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Automatic;
            this.customButtonDeltaYValue.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonDeltaYValue.DrawIcon = false;
            this.customButtonDeltaYValue.DrawText = true;
            this.customButtonDeltaYValue.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonDeltaYValue.English_TextDisplay = new string[] {
        "1024"};
            this.customButtonDeltaYValue.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 1)};
            this.customButtonDeltaYValue.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonDeltaYValue.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(43, 22)};
            this.customButtonDeltaYValue.FocusBackgroundDisplay = false;
            this.customButtonDeltaYValue.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonDeltaYValue.FontText = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonDeltaYValue.ForeColor = System.Drawing.Color.White;
            this.customButtonDeltaYValue.HoverBackgroundDisplay = false;
            this.customButtonDeltaYValue.IconIndex = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonDeltaYValue.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonDeltaYValue.IconNumber = 1;
            this.customButtonDeltaYValue.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(0, 0)};
            this.customButtonDeltaYValue.LabelControlMode = true;
            this.customButtonDeltaYValue.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonDeltaYValue.Location = new System.Drawing.Point(553, 632);
            this.customButtonDeltaYValue.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonDeltaYValue.Name = "customButtonDeltaYValue";
            this.customButtonDeltaYValue.RectBottom = new System.Drawing.Rectangle(3, 23, 237, 3);
            this.customButtonDeltaYValue.RectFill = new System.Drawing.Rectangle(3, 3, 237, 20);
            this.customButtonDeltaYValue.RectLeft = new System.Drawing.Rectangle(0, 3, 3, 20);
            this.customButtonDeltaYValue.RectLeftBottom = new System.Drawing.Rectangle(0, 23, 3, 3);
            this.customButtonDeltaYValue.RectLeftTop = new System.Drawing.Rectangle(0, 0, 3, 3);
            this.customButtonDeltaYValue.RectRight = new System.Drawing.Rectangle(240, 3, 3, 20);
            this.customButtonDeltaYValue.RectRightBottom = new System.Drawing.Rectangle(240, 23, 3, 3);
            this.customButtonDeltaYValue.RectRightTop = new System.Drawing.Rectangle(240, 0, 3, 3);
            this.customButtonDeltaYValue.RectTop = new System.Drawing.Rectangle(3, 0, 237, 3);
            this.customButtonDeltaYValue.Size = new System.Drawing.Size(60, 25);
            this.customButtonDeltaYValue.SizeButton = new System.Drawing.Size(60, 25);
            this.customButtonDeltaYValue.TabIndex = 90;
            this.customButtonDeltaYValue.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonDeltaYValue.TextGroupNumber = 1;
            this.customButtonDeltaYValue.UpdateControl = true;
            this.customButtonDeltaYValue.Visible = false;
            // 
            // customButtonDeltaYText
            // 
            this.customButtonDeltaYText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.customButtonDeltaYText.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.customButtonDeltaYText.BitmapIconWhole = null;
            this.customButtonDeltaYText.BitmapWhole = null;
            this.customButtonDeltaYText.Chinese_TextDisplay = new string[] {
        "Delta Y = "};
            this.customButtonDeltaYText.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(2, 1)};
            this.customButtonDeltaYText.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonDeltaYText.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(76, 21)};
            this.customButtonDeltaYText.ColorTextDisable = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
            this.customButtonDeltaYText.ColorTextEnable = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonDeltaYText.ColorTextSelected = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonDeltaYText.CurrentTextGroupIndex = 0;
            this.customButtonDeltaYText.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonDeltaYText.CustomButtonData = null;
            this.customButtonDeltaYText.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonDeltaYText.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonDeltaYText.DrawIcon = false;
            this.customButtonDeltaYText.DrawText = true;
            this.customButtonDeltaYText.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonDeltaYText.English_TextDisplay = new string[] {
        "Delta Y = "};
            this.customButtonDeltaYText.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(2, 1)};
            this.customButtonDeltaYText.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonDeltaYText.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(76, 21)};
            this.customButtonDeltaYText.FocusBackgroundDisplay = false;
            this.customButtonDeltaYText.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonDeltaYText.FontText = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonDeltaYText.ForeColor = System.Drawing.Color.White;
            this.customButtonDeltaYText.HoverBackgroundDisplay = false;
            this.customButtonDeltaYText.IconIndex = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonDeltaYText.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonDeltaYText.IconNumber = 1;
            this.customButtonDeltaYText.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(0, 0)};
            this.customButtonDeltaYText.LabelControlMode = true;
            this.customButtonDeltaYText.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonDeltaYText.Location = new System.Drawing.Point(470, 632);
            this.customButtonDeltaYText.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonDeltaYText.Name = "customButtonDeltaYText";
            this.customButtonDeltaYText.RectBottom = new System.Drawing.Rectangle(3, 23, 237, 3);
            this.customButtonDeltaYText.RectFill = new System.Drawing.Rectangle(3, 3, 237, 20);
            this.customButtonDeltaYText.RectLeft = new System.Drawing.Rectangle(0, 3, 3, 20);
            this.customButtonDeltaYText.RectLeftBottom = new System.Drawing.Rectangle(0, 23, 3, 3);
            this.customButtonDeltaYText.RectLeftTop = new System.Drawing.Rectangle(0, 0, 3, 3);
            this.customButtonDeltaYText.RectRight = new System.Drawing.Rectangle(240, 3, 3, 20);
            this.customButtonDeltaYText.RectRightBottom = new System.Drawing.Rectangle(240, 23, 3, 3);
            this.customButtonDeltaYText.RectRightTop = new System.Drawing.Rectangle(240, 0, 3, 3);
            this.customButtonDeltaYText.RectTop = new System.Drawing.Rectangle(3, 0, 237, 3);
            this.customButtonDeltaYText.Size = new System.Drawing.Size(80, 25);
            this.customButtonDeltaYText.SizeButton = new System.Drawing.Size(80, 25);
            this.customButtonDeltaYText.TabIndex = 89;
            this.customButtonDeltaYText.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.customButtonDeltaYText.TextGroupNumber = 1;
            this.customButtonDeltaYText.UpdateControl = true;
            this.customButtonDeltaYText.Visible = false;
            // 
            // customButtonDeltaXValue
            // 
            this.customButtonDeltaXValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.customButtonDeltaXValue.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.customButtonDeltaXValue.BitmapIconWhole = null;
            this.customButtonDeltaXValue.BitmapWhole = null;
            this.customButtonDeltaXValue.Chinese_TextDisplay = new string[] {
        "1024"};
            this.customButtonDeltaXValue.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 1)};
            this.customButtonDeltaXValue.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonDeltaXValue.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(43, 22)};
            this.customButtonDeltaXValue.ColorTextDisable = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
            this.customButtonDeltaXValue.ColorTextEnable = System.Drawing.Color.Yellow;
            this.customButtonDeltaXValue.ColorTextSelected = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonDeltaXValue.CurrentTextGroupIndex = 0;
            this.customButtonDeltaXValue.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonDeltaXValue.CustomButtonData = null;
            this.customButtonDeltaXValue.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Automatic;
            this.customButtonDeltaXValue.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonDeltaXValue.DrawIcon = false;
            this.customButtonDeltaXValue.DrawText = true;
            this.customButtonDeltaXValue.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonDeltaXValue.English_TextDisplay = new string[] {
        "1024"};
            this.customButtonDeltaXValue.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 1)};
            this.customButtonDeltaXValue.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonDeltaXValue.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(43, 22)};
            this.customButtonDeltaXValue.FocusBackgroundDisplay = false;
            this.customButtonDeltaXValue.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonDeltaXValue.FontText = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonDeltaXValue.ForeColor = System.Drawing.Color.White;
            this.customButtonDeltaXValue.HoverBackgroundDisplay = false;
            this.customButtonDeltaXValue.IconIndex = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonDeltaXValue.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonDeltaXValue.IconNumber = 1;
            this.customButtonDeltaXValue.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(0, 0)};
            this.customButtonDeltaXValue.LabelControlMode = true;
            this.customButtonDeltaXValue.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonDeltaXValue.Location = new System.Drawing.Point(552, 607);
            this.customButtonDeltaXValue.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonDeltaXValue.Name = "customButtonDeltaXValue";
            this.customButtonDeltaXValue.RectBottom = new System.Drawing.Rectangle(3, 23, 237, 3);
            this.customButtonDeltaXValue.RectFill = new System.Drawing.Rectangle(3, 3, 237, 20);
            this.customButtonDeltaXValue.RectLeft = new System.Drawing.Rectangle(0, 3, 3, 20);
            this.customButtonDeltaXValue.RectLeftBottom = new System.Drawing.Rectangle(0, 23, 3, 3);
            this.customButtonDeltaXValue.RectLeftTop = new System.Drawing.Rectangle(0, 0, 3, 3);
            this.customButtonDeltaXValue.RectRight = new System.Drawing.Rectangle(240, 3, 3, 20);
            this.customButtonDeltaXValue.RectRightBottom = new System.Drawing.Rectangle(240, 23, 3, 3);
            this.customButtonDeltaXValue.RectRightTop = new System.Drawing.Rectangle(240, 0, 3, 3);
            this.customButtonDeltaXValue.RectTop = new System.Drawing.Rectangle(3, 0, 237, 3);
            this.customButtonDeltaXValue.Size = new System.Drawing.Size(60, 25);
            this.customButtonDeltaXValue.SizeButton = new System.Drawing.Size(60, 25);
            this.customButtonDeltaXValue.TabIndex = 88;
            this.customButtonDeltaXValue.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonDeltaXValue.TextGroupNumber = 1;
            this.customButtonDeltaXValue.UpdateControl = true;
            this.customButtonDeltaXValue.Visible = false;
            // 
            // customButtonDeltaXText
            // 
            this.customButtonDeltaXText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.customButtonDeltaXText.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.customButtonDeltaXText.BitmapIconWhole = null;
            this.customButtonDeltaXText.BitmapWhole = null;
            this.customButtonDeltaXText.Chinese_TextDisplay = new string[] {
        "Delta X = "};
            this.customButtonDeltaXText.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(1, 1)};
            this.customButtonDeltaXText.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonDeltaXText.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(77, 21)};
            this.customButtonDeltaXText.ColorTextDisable = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
            this.customButtonDeltaXText.ColorTextEnable = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonDeltaXText.ColorTextSelected = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonDeltaXText.CurrentTextGroupIndex = 0;
            this.customButtonDeltaXText.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonDeltaXText.CustomButtonData = null;
            this.customButtonDeltaXText.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonDeltaXText.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonDeltaXText.DrawIcon = false;
            this.customButtonDeltaXText.DrawText = true;
            this.customButtonDeltaXText.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonDeltaXText.English_TextDisplay = new string[] {
        "Delta X = "};
            this.customButtonDeltaXText.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(1, 1)};
            this.customButtonDeltaXText.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonDeltaXText.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(77, 21)};
            this.customButtonDeltaXText.FocusBackgroundDisplay = false;
            this.customButtonDeltaXText.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonDeltaXText.FontText = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonDeltaXText.ForeColor = System.Drawing.Color.White;
            this.customButtonDeltaXText.HoverBackgroundDisplay = false;
            this.customButtonDeltaXText.IconIndex = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonDeltaXText.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonDeltaXText.IconNumber = 1;
            this.customButtonDeltaXText.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(0, 0)};
            this.customButtonDeltaXText.LabelControlMode = true;
            this.customButtonDeltaXText.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonDeltaXText.Location = new System.Drawing.Point(471, 607);
            this.customButtonDeltaXText.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonDeltaXText.Name = "customButtonDeltaXText";
            this.customButtonDeltaXText.RectBottom = new System.Drawing.Rectangle(3, 23, 237, 3);
            this.customButtonDeltaXText.RectFill = new System.Drawing.Rectangle(3, 3, 237, 20);
            this.customButtonDeltaXText.RectLeft = new System.Drawing.Rectangle(0, 3, 3, 20);
            this.customButtonDeltaXText.RectLeftBottom = new System.Drawing.Rectangle(0, 23, 3, 3);
            this.customButtonDeltaXText.RectLeftTop = new System.Drawing.Rectangle(0, 0, 3, 3);
            this.customButtonDeltaXText.RectRight = new System.Drawing.Rectangle(240, 3, 3, 20);
            this.customButtonDeltaXText.RectRightBottom = new System.Drawing.Rectangle(240, 23, 3, 3);
            this.customButtonDeltaXText.RectRightTop = new System.Drawing.Rectangle(240, 0, 3, 3);
            this.customButtonDeltaXText.RectTop = new System.Drawing.Rectangle(3, 0, 237, 3);
            this.customButtonDeltaXText.Size = new System.Drawing.Size(80, 25);
            this.customButtonDeltaXText.SizeButton = new System.Drawing.Size(80, 25);
            this.customButtonDeltaXText.TabIndex = 87;
            this.customButtonDeltaXText.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.customButtonDeltaXText.TextGroupNumber = 1;
            this.customButtonDeltaXText.UpdateControl = true;
            this.customButtonDeltaXText.Visible = false;
            // 
            // customButtonColorValue
            // 
            this.customButtonColorValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.customButtonColorValue.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.customButtonColorValue.BitmapIconWhole = null;
            this.customButtonColorValue.BitmapWhole = null;
            this.customButtonColorValue.Chinese_TextDisplay = new string[] {
        "255，255，255"};
            this.customButtonColorValue.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 1)};
            this.customButtonColorValue.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonColorValue.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(119, 22)};
            this.customButtonColorValue.ColorTextDisable = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
            this.customButtonColorValue.ColorTextEnable = System.Drawing.Color.Yellow;
            this.customButtonColorValue.ColorTextSelected = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonColorValue.CurrentTextGroupIndex = 0;
            this.customButtonColorValue.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonColorValue.CustomButtonData = null;
            this.customButtonColorValue.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Automatic;
            this.customButtonColorValue.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonColorValue.DrawIcon = false;
            this.customButtonColorValue.DrawText = true;
            this.customButtonColorValue.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonColorValue.English_TextDisplay = new string[] {
        "255，255，255"};
            this.customButtonColorValue.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 1)};
            this.customButtonColorValue.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonColorValue.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(119, 22)};
            this.customButtonColorValue.FocusBackgroundDisplay = false;
            this.customButtonColorValue.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonColorValue.FontText = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonColorValue.ForeColor = System.Drawing.Color.White;
            this.customButtonColorValue.HoverBackgroundDisplay = false;
            this.customButtonColorValue.IconIndex = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonColorValue.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonColorValue.IconNumber = 1;
            this.customButtonColorValue.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(0, 0)};
            this.customButtonColorValue.LabelControlMode = true;
            this.customButtonColorValue.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonColorValue.Location = new System.Drawing.Point(342, 632);
            this.customButtonColorValue.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonColorValue.Name = "customButtonColorValue";
            this.customButtonColorValue.RectBottom = new System.Drawing.Rectangle(3, 23, 237, 3);
            this.customButtonColorValue.RectFill = new System.Drawing.Rectangle(3, 3, 237, 20);
            this.customButtonColorValue.RectLeft = new System.Drawing.Rectangle(0, 3, 3, 20);
            this.customButtonColorValue.RectLeftBottom = new System.Drawing.Rectangle(0, 23, 3, 3);
            this.customButtonColorValue.RectLeftTop = new System.Drawing.Rectangle(0, 0, 3, 3);
            this.customButtonColorValue.RectRight = new System.Drawing.Rectangle(240, 3, 3, 20);
            this.customButtonColorValue.RectRightBottom = new System.Drawing.Rectangle(240, 23, 3, 3);
            this.customButtonColorValue.RectRightTop = new System.Drawing.Rectangle(240, 0, 3, 3);
            this.customButtonColorValue.RectTop = new System.Drawing.Rectangle(3, 0, 237, 3);
            this.customButtonColorValue.Size = new System.Drawing.Size(120, 25);
            this.customButtonColorValue.SizeButton = new System.Drawing.Size(120, 25);
            this.customButtonColorValue.TabIndex = 86;
            this.customButtonColorValue.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonColorValue.TextGroupNumber = 1;
            this.customButtonColorValue.UpdateControl = true;
            // 
            // customButtonColorText
            // 
            this.customButtonColorText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.customButtonColorText.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.customButtonColorText.BitmapIconWhole = null;
            this.customButtonColorText.BitmapWhole = null;
            this.customButtonColorText.Chinese_TextDisplay = new string[] {
        "（R，G，B）= "};
            this.customButtonColorText.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 1)};
            this.customButtonColorText.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonColorText.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(110, 21)};
            this.customButtonColorText.ColorTextDisable = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
            this.customButtonColorText.ColorTextEnable = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonColorText.ColorTextSelected = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonColorText.CurrentTextGroupIndex = 0;
            this.customButtonColorText.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonColorText.CustomButtonData = null;
            this.customButtonColorText.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonColorText.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonColorText.DrawIcon = false;
            this.customButtonColorText.DrawText = true;
            this.customButtonColorText.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonColorText.English_TextDisplay = new string[] {
        "（R，G，B）= "};
            this.customButtonColorText.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 1)};
            this.customButtonColorText.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonColorText.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(110, 21)};
            this.customButtonColorText.FocusBackgroundDisplay = false;
            this.customButtonColorText.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonColorText.FontText = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonColorText.ForeColor = System.Drawing.Color.White;
            this.customButtonColorText.HoverBackgroundDisplay = false;
            this.customButtonColorText.IconIndex = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonColorText.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonColorText.IconNumber = 1;
            this.customButtonColorText.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(0, 0)};
            this.customButtonColorText.LabelControlMode = true;
            this.customButtonColorText.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonColorText.Location = new System.Drawing.Point(227, 632);
            this.customButtonColorText.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonColorText.Name = "customButtonColorText";
            this.customButtonColorText.RectBottom = new System.Drawing.Rectangle(3, 23, 237, 3);
            this.customButtonColorText.RectFill = new System.Drawing.Rectangle(3, 3, 237, 20);
            this.customButtonColorText.RectLeft = new System.Drawing.Rectangle(0, 3, 3, 20);
            this.customButtonColorText.RectLeftBottom = new System.Drawing.Rectangle(0, 23, 3, 3);
            this.customButtonColorText.RectLeftTop = new System.Drawing.Rectangle(0, 0, 3, 3);
            this.customButtonColorText.RectRight = new System.Drawing.Rectangle(240, 3, 3, 20);
            this.customButtonColorText.RectRightBottom = new System.Drawing.Rectangle(240, 23, 3, 3);
            this.customButtonColorText.RectRightTop = new System.Drawing.Rectangle(240, 0, 3, 3);
            this.customButtonColorText.RectTop = new System.Drawing.Rectangle(3, 0, 237, 3);
            this.customButtonColorText.Size = new System.Drawing.Size(110, 25);
            this.customButtonColorText.SizeButton = new System.Drawing.Size(110, 25);
            this.customButtonColorText.TabIndex = 85;
            this.customButtonColorText.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.customButtonColorText.TextGroupNumber = 1;
            this.customButtonColorText.UpdateControl = true;
            // 
            // customButtonAxisValue
            // 
            this.customButtonAxisValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.customButtonAxisValue.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.customButtonAxisValue.BitmapIconWhole = null;
            this.customButtonAxisValue.BitmapWhole = null;
            this.customButtonAxisValue.Chinese_TextDisplay = new string[] {
        "1024，1024"};
            this.customButtonAxisValue.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 1)};
            this.customButtonAxisValue.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonAxisValue.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(95, 22)};
            this.customButtonAxisValue.ColorTextDisable = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
            this.customButtonAxisValue.ColorTextEnable = System.Drawing.Color.Yellow;
            this.customButtonAxisValue.ColorTextSelected = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonAxisValue.CurrentTextGroupIndex = 0;
            this.customButtonAxisValue.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonAxisValue.CustomButtonData = null;
            this.customButtonAxisValue.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Automatic;
            this.customButtonAxisValue.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonAxisValue.DrawIcon = false;
            this.customButtonAxisValue.DrawText = true;
            this.customButtonAxisValue.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonAxisValue.English_TextDisplay = new string[] {
        "1024，1024"};
            this.customButtonAxisValue.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 1)};
            this.customButtonAxisValue.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonAxisValue.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(95, 22)};
            this.customButtonAxisValue.FocusBackgroundDisplay = false;
            this.customButtonAxisValue.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonAxisValue.FontText = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonAxisValue.ForeColor = System.Drawing.Color.White;
            this.customButtonAxisValue.HoverBackgroundDisplay = false;
            this.customButtonAxisValue.IconIndex = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonAxisValue.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonAxisValue.IconNumber = 1;
            this.customButtonAxisValue.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(0, 0)};
            this.customButtonAxisValue.LabelControlMode = true;
            this.customButtonAxisValue.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonAxisValue.Location = new System.Drawing.Point(342, 607);
            this.customButtonAxisValue.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonAxisValue.Name = "customButtonAxisValue";
            this.customButtonAxisValue.RectBottom = new System.Drawing.Rectangle(3, 23, 237, 3);
            this.customButtonAxisValue.RectFill = new System.Drawing.Rectangle(3, 3, 237, 20);
            this.customButtonAxisValue.RectLeft = new System.Drawing.Rectangle(0, 3, 3, 20);
            this.customButtonAxisValue.RectLeftBottom = new System.Drawing.Rectangle(0, 23, 3, 3);
            this.customButtonAxisValue.RectLeftTop = new System.Drawing.Rectangle(0, 0, 3, 3);
            this.customButtonAxisValue.RectRight = new System.Drawing.Rectangle(240, 3, 3, 20);
            this.customButtonAxisValue.RectRightBottom = new System.Drawing.Rectangle(240, 23, 3, 3);
            this.customButtonAxisValue.RectRightTop = new System.Drawing.Rectangle(240, 0, 3, 3);
            this.customButtonAxisValue.RectTop = new System.Drawing.Rectangle(3, 0, 237, 3);
            this.customButtonAxisValue.Size = new System.Drawing.Size(100, 25);
            this.customButtonAxisValue.SizeButton = new System.Drawing.Size(100, 25);
            this.customButtonAxisValue.TabIndex = 84;
            this.customButtonAxisValue.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonAxisValue.TextGroupNumber = 1;
            this.customButtonAxisValue.UpdateControl = true;
            // 
            // customButtonAxisText
            // 
            this.customButtonAxisText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.customButtonAxisText.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.customButtonAxisText.BitmapIconWhole = null;
            this.customButtonAxisText.BitmapWhole = null;
            this.customButtonAxisText.Chinese_TextDisplay = new string[] {
        "（X，Y）= "};
            this.customButtonAxisText.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(3, 1)};
            this.customButtonAxisText.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonAxisText.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(83, 21)};
            this.customButtonAxisText.ColorTextDisable = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
            this.customButtonAxisText.ColorTextEnable = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonAxisText.ColorTextSelected = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonAxisText.CurrentTextGroupIndex = 0;
            this.customButtonAxisText.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonAxisText.CustomButtonData = null;
            this.customButtonAxisText.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonAxisText.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonAxisText.DrawIcon = false;
            this.customButtonAxisText.DrawText = true;
            this.customButtonAxisText.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonAxisText.English_TextDisplay = new string[] {
        "（X，Y）= "};
            this.customButtonAxisText.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(3, 1)};
            this.customButtonAxisText.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonAxisText.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(83, 21)};
            this.customButtonAxisText.FocusBackgroundDisplay = false;
            this.customButtonAxisText.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonAxisText.FontText = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonAxisText.ForeColor = System.Drawing.Color.White;
            this.customButtonAxisText.HoverBackgroundDisplay = false;
            this.customButtonAxisText.IconIndex = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonAxisText.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonAxisText.IconNumber = 1;
            this.customButtonAxisText.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(0, 0)};
            this.customButtonAxisText.LabelControlMode = true;
            this.customButtonAxisText.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonAxisText.Location = new System.Drawing.Point(251, 607);
            this.customButtonAxisText.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonAxisText.Name = "customButtonAxisText";
            this.customButtonAxisText.RectBottom = new System.Drawing.Rectangle(3, 23, 237, 3);
            this.customButtonAxisText.RectFill = new System.Drawing.Rectangle(3, 3, 237, 20);
            this.customButtonAxisText.RectLeft = new System.Drawing.Rectangle(0, 3, 3, 20);
            this.customButtonAxisText.RectLeftBottom = new System.Drawing.Rectangle(0, 23, 3, 3);
            this.customButtonAxisText.RectLeftTop = new System.Drawing.Rectangle(0, 0, 3, 3);
            this.customButtonAxisText.RectRight = new System.Drawing.Rectangle(240, 3, 3, 20);
            this.customButtonAxisText.RectRightBottom = new System.Drawing.Rectangle(240, 23, 3, 3);
            this.customButtonAxisText.RectRightTop = new System.Drawing.Rectangle(240, 0, 3, 3);
            this.customButtonAxisText.RectTop = new System.Drawing.Rectangle(3, 0, 237, 3);
            this.customButtonAxisText.Size = new System.Drawing.Size(90, 25);
            this.customButtonAxisText.SizeButton = new System.Drawing.Size(90, 25);
            this.customButtonAxisText.TabIndex = 83;
            this.customButtonAxisText.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.customButtonAxisText.TextGroupNumber = 1;
            this.customButtonAxisText.UpdateControl = true;
            // 
            // customButtonMeasureTool
            // 
            this.customButtonMeasureTool.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonMeasureTool.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonMeasureTool.BitmapIconWhole = null;
            this.customButtonMeasureTool.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.Button;
            this.customButtonMeasureTool.Chinese_TextDisplay = new string[] {
        "测量工具"};
            this.customButtonMeasureTool.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(13, 9)};
            this.customButtonMeasureTool.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonMeasureTool.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(72, 23)};
            this.customButtonMeasureTool.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonMeasureTool.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonMeasureTool.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonMeasureTool.CurrentTextGroupIndex = 0;
            this.customButtonMeasureTool.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonMeasureTool.CustomButtonData = null;
            this.customButtonMeasureTool.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonMeasureTool.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Switch;
            this.customButtonMeasureTool.DrawIcon = false;
            this.customButtonMeasureTool.DrawText = true;
            this.customButtonMeasureTool.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonMeasureTool.English_TextDisplay = new string[] {
        "MEASURE&TOOL"};
            this.customButtonMeasureTool.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 1),
        new System.Drawing.Point(23, 19)};
            this.customButtonMeasureTool.English_TextNumberInTextGroup = new int[] {
        2};
            this.customButtonMeasureTool.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(88, 23),
        new System.Drawing.Size(52, 23)};
            this.customButtonMeasureTool.FocusBackgroundDisplay = false;
            this.customButtonMeasureTool.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonMeasureTool.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonMeasureTool.ForeColor = System.Drawing.Color.White;
            this.customButtonMeasureTool.HoverBackgroundDisplay = false;
            this.customButtonMeasureTool.IconIndex = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonMeasureTool.IconLocation = new System.Drawing.Point[0];
            this.customButtonMeasureTool.IconNumber = 0;
            this.customButtonMeasureTool.IconSize = new System.Drawing.Size[0];
            this.customButtonMeasureTool.LabelControlMode = false;
            this.customButtonMeasureTool.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonMeasureTool.Location = new System.Drawing.Point(766, 610);
            this.customButtonMeasureTool.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonMeasureTool.Name = "customButtonMeasureTool";
            this.customButtonMeasureTool.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonMeasureTool.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonMeasureTool.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonMeasureTool.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonMeasureTool.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonMeasureTool.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonMeasureTool.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonMeasureTool.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonMeasureTool.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonMeasureTool.Size = new System.Drawing.Size(98, 42);
            this.customButtonMeasureTool.SizeButton = new System.Drawing.Size(98, 42);
            this.customButtonMeasureTool.TabIndex = 82;
            this.customButtonMeasureTool.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.customButtonMeasureTool.TextGroupNumber = 1;
            this.customButtonMeasureTool.UpdateControl = true;
            this.customButtonMeasureTool.CustomButton_Click += new System.EventHandler(this.customButtonMeasureTool_CustomButton_Click);
            // 
            // customButtonNext
            // 
            this.customButtonNext.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonNext.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonNext.BitmapIconWhole = global::VisionSystemControlLibrary.Properties.Resources.Forward;
            this.customButtonNext.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.Button;
            this.customButtonNext.Chinese_TextDisplay = new string[] {
        "下一页"};
            this.customButtonNext.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 5)};
            this.customButtonNext.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonNext.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(55, 23)};
            this.customButtonNext.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonNext.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonNext.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonNext.CurrentTextGroupIndex = 0;
            this.customButtonNext.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonNext.CustomButtonData = null;
            this.customButtonNext.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonNext.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonNext.DrawIcon = true;
            this.customButtonNext.DrawText = false;
            this.customButtonNext.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonNext.English_TextDisplay = new string[] {
        "NEXT&PAGE"};
            this.customButtonNext.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 5),
        new System.Drawing.Point(5, 25)};
            this.customButtonNext.English_TextNumberInTextGroup = new int[] {
        2};
            this.customButtonNext.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(51, 23),
        new System.Drawing.Size(51, 23)};
            this.customButtonNext.FocusBackgroundDisplay = false;
            this.customButtonNext.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonNext.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonNext.ForeColor = System.Drawing.Color.White;
            this.customButtonNext.HoverBackgroundDisplay = false;
            this.customButtonNext.IconIndex = new int[] {
        1,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonNext.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(19, 1),
        new System.Drawing.Point(19, 1)};
            this.customButtonNext.IconNumber = 2;
            this.customButtonNext.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(70, 48),
        new System.Drawing.Size(70, 48)};
            this.customButtonNext.LabelControlMode = false;
            this.customButtonNext.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonNext.Location = new System.Drawing.Point(112, 606);
            this.customButtonNext.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonNext.Name = "customButtonNext";
            this.customButtonNext.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonNext.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonNext.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonNext.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonNext.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonNext.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonNext.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonNext.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonNext.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonNext.Size = new System.Drawing.Size(106, 49);
            this.customButtonNext.SizeButton = new System.Drawing.Size(106, 49);
            this.customButtonNext.TabIndex = 81;
            this.customButtonNext.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonNext.TextGroupNumber = 1;
            this.customButtonNext.UpdateControl = true;
            this.customButtonNext.Visible = false;
            this.customButtonNext.CustomButton_Click += new System.EventHandler(this.customButtonNext_CustomButton_Click);
            // 
            // customButtonPrevious
            // 
            this.customButtonPrevious.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonPrevious.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonPrevious.BitmapIconWhole = global::VisionSystemControlLibrary.Properties.Resources.Close;
            this.customButtonPrevious.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.Button;
            this.customButtonPrevious.Chinese_TextDisplay = new string[] {
        "上一页"};
            this.customButtonPrevious.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 5)};
            this.customButtonPrevious.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonPrevious.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(55, 23)};
            this.customButtonPrevious.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonPrevious.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonPrevious.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonPrevious.CurrentTextGroupIndex = 0;
            this.customButtonPrevious.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonPrevious.CustomButtonData = null;
            this.customButtonPrevious.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonPrevious.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonPrevious.DrawIcon = true;
            this.customButtonPrevious.DrawText = false;
            this.customButtonPrevious.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonPrevious.English_TextDisplay = new string[] {
        "PREV.&PAGE"};
            this.customButtonPrevious.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 5),
        new System.Drawing.Point(5, 25)};
            this.customButtonPrevious.English_TextNumberInTextGroup = new int[] {
        2};
            this.customButtonPrevious.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(54, 23),
        new System.Drawing.Size(51, 23)};
            this.customButtonPrevious.FocusBackgroundDisplay = false;
            this.customButtonPrevious.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonPrevious.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonPrevious.ForeColor = System.Drawing.Color.White;
            this.customButtonPrevious.HoverBackgroundDisplay = false;
            this.customButtonPrevious.IconIndex = new int[] {
        1,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonPrevious.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(15, 1),
        new System.Drawing.Point(15, 1)};
            this.customButtonPrevious.IconNumber = 2;
            this.customButtonPrevious.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(70, 48),
        new System.Drawing.Size(70, 48)};
            this.customButtonPrevious.LabelControlMode = false;
            this.customButtonPrevious.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonPrevious.Location = new System.Drawing.Point(4, 606);
            this.customButtonPrevious.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonPrevious.Name = "customButtonPrevious";
            this.customButtonPrevious.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonPrevious.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonPrevious.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonPrevious.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonPrevious.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonPrevious.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonPrevious.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonPrevious.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonPrevious.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonPrevious.Size = new System.Drawing.Size(106, 49);
            this.customButtonPrevious.SizeButton = new System.Drawing.Size(106, 49);
            this.customButtonPrevious.TabIndex = 80;
            this.customButtonPrevious.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonPrevious.TextGroupNumber = 1;
            this.customButtonPrevious.UpdateControl = true;
            this.customButtonPrevious.Visible = false;
            this.customButtonPrevious.CustomButton_Click += new System.EventHandler(this.customButtonPrevious_CustomButton_Click);
            // 
            // customButtonRejectImageIndex
            // 
            this.customButtonRejectImageIndex.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.customButtonRejectImageIndex.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.customButtonRejectImageIndex.BitmapIconWhole = null;
            this.customButtonRejectImageIndex.BitmapWhole = null;
            this.customButtonRejectImageIndex.Chinese_TextDisplay = new string[] {
        "1"};
            this.customButtonRejectImageIndex.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(13, 12)};
            this.customButtonRejectImageIndex.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonRejectImageIndex.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(13, 19)};
            this.customButtonRejectImageIndex.ColorTextDisable = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
            this.customButtonRejectImageIndex.ColorTextEnable = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonRejectImageIndex.ColorTextSelected = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonRejectImageIndex.CurrentTextGroupIndex = 0;
            this.customButtonRejectImageIndex.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonRejectImageIndex.CustomButtonData = null;
            this.customButtonRejectImageIndex.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Automatic;
            this.customButtonRejectImageIndex.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonRejectImageIndex.DrawIcon = false;
            this.customButtonRejectImageIndex.DrawText = true;
            this.customButtonRejectImageIndex.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonRejectImageIndex.English_TextDisplay = new string[] {
        "1"};
            this.customButtonRejectImageIndex.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(13, 12)};
            this.customButtonRejectImageIndex.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonRejectImageIndex.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(13, 19)};
            this.customButtonRejectImageIndex.FocusBackgroundDisplay = false;
            this.customButtonRejectImageIndex.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonRejectImageIndex.FontText = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonRejectImageIndex.ForeColor = System.Drawing.Color.White;
            this.customButtonRejectImageIndex.HoverBackgroundDisplay = false;
            this.customButtonRejectImageIndex.IconIndex = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonRejectImageIndex.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonRejectImageIndex.IconNumber = 1;
            this.customButtonRejectImageIndex.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(0, 0)};
            this.customButtonRejectImageIndex.LabelControlMode = true;
            this.customButtonRejectImageIndex.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonRejectImageIndex.Location = new System.Drawing.Point(928, 550);
            this.customButtonRejectImageIndex.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonRejectImageIndex.Name = "customButtonRejectImageIndex";
            this.customButtonRejectImageIndex.RectBottom = new System.Drawing.Rectangle(3, 23, 237, 3);
            this.customButtonRejectImageIndex.RectFill = new System.Drawing.Rectangle(3, 3, 237, 20);
            this.customButtonRejectImageIndex.RectLeft = new System.Drawing.Rectangle(0, 3, 3, 20);
            this.customButtonRejectImageIndex.RectLeftBottom = new System.Drawing.Rectangle(0, 23, 3, 3);
            this.customButtonRejectImageIndex.RectLeftTop = new System.Drawing.Rectangle(0, 0, 3, 3);
            this.customButtonRejectImageIndex.RectRight = new System.Drawing.Rectangle(240, 3, 3, 20);
            this.customButtonRejectImageIndex.RectRightBottom = new System.Drawing.Rectangle(240, 23, 3, 3);
            this.customButtonRejectImageIndex.RectRightTop = new System.Drawing.Rectangle(240, 0, 3, 3);
            this.customButtonRejectImageIndex.RectTop = new System.Drawing.Rectangle(3, 0, 237, 3);
            this.customButtonRejectImageIndex.Size = new System.Drawing.Size(40, 44);
            this.customButtonRejectImageIndex.SizeButton = new System.Drawing.Size(40, 44);
            this.customButtonRejectImageIndex.TabIndex = 79;
            this.customButtonRejectImageIndex.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.customButtonRejectImageIndex.TextGroupNumber = 1;
            this.customButtonRejectImageIndex.UpdateControl = true;
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
        new System.Drawing.Point(5, 7),
        new System.Drawing.Point(5, 27)};
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
        new System.Drawing.Point(5, 7),
        new System.Drawing.Point(5, 27)};
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
        new System.Drawing.Point(90, 10),
        new System.Drawing.Point(90, 10),
        new System.Drawing.Point(90, 10),
        new System.Drawing.Point(90, 10)};
            this.customButtonStatusBar.IconNumber = 4;
            this.customButtonStatusBar.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(53, 37),
        new System.Drawing.Size(53, 37),
        new System.Drawing.Size(53, 37),
        new System.Drawing.Size(53, 37)};
            this.customButtonStatusBar.LabelControlMode = false;
            this.customButtonStatusBar.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonStatusBar.Location = new System.Drawing.Point(874, 600);
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
            this.customButtonStatusBar.Size = new System.Drawing.Size(140, 56);
            this.customButtonStatusBar.SizeButton = new System.Drawing.Size(140, 56);
            this.customButtonStatusBar.TabIndex = 78;
            this.customButtonStatusBar.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonStatusBar.TextGroupNumber = 1;
            this.customButtonStatusBar.UpdateControl = true;
            this.customButtonStatusBar.CustomButton_Click += new System.EventHandler(this.customButtonStatusBar_CustomButton_Click);
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
        new System.Drawing.Point(18, 10)};
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
        new System.Drawing.Point(18, 10)};
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
            this.customButtonSubtract.Location = new System.Drawing.Point(878, 550);
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
            this.customButtonSubtract.Size = new System.Drawing.Size(49, 44);
            this.customButtonSubtract.SizeButton = new System.Drawing.Size(49, 44);
            this.customButtonSubtract.TabIndex = 77;
            this.customButtonSubtract.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.customButtonSubtract.TextGroupNumber = 1;
            this.customButtonSubtract.UpdateControl = true;
            this.customButtonSubtract.CustomButton_Click += new System.EventHandler(this.customButtonSubtract_CustomButton_Click);
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
        new System.Drawing.Point(18, 10)};
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
        new System.Drawing.Point(15, 10)};
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
            this.customButtonPlus.Location = new System.Drawing.Point(969, 550);
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
            this.customButtonPlus.Size = new System.Drawing.Size(49, 44);
            this.customButtonPlus.SizeButton = new System.Drawing.Size(49, 44);
            this.customButtonPlus.TabIndex = 76;
            this.customButtonPlus.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.customButtonPlus.TextGroupNumber = 1;
            this.customButtonPlus.UpdateControl = true;
            this.customButtonPlus.CustomButton_Click += new System.EventHandler(this.customButtonPlus_CustomButton_Click);
            // 
            // customButtonLoadReject
            // 
            this.customButtonLoadReject.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonLoadReject.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonLoadReject.BitmapIconWhole = global::VisionSystemControlLibrary.Properties.Resources.LoadReject;
            this.customButtonLoadReject.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.Button;
            this.customButtonLoadReject.Chinese_TextDisplay = new string[] {
        "加载&剔除图像"};
            this.customButtonLoadReject.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 7),
        new System.Drawing.Point(5, 27)};
            this.customButtonLoadReject.Chinese_TextNumberInTextGroup = new int[] {
        2};
            this.customButtonLoadReject.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(39, 23),
        new System.Drawing.Size(72, 23)};
            this.customButtonLoadReject.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonLoadReject.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonLoadReject.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonLoadReject.CurrentTextGroupIndex = 0;
            this.customButtonLoadReject.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonLoadReject.CustomButtonData = null;
            this.customButtonLoadReject.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonLoadReject.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Switch;
            this.customButtonLoadReject.DrawIcon = true;
            this.customButtonLoadReject.DrawText = true;
            this.customButtonLoadReject.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonLoadReject.English_TextDisplay = new string[] {
        "LOAD&REJECT"};
            this.customButtonLoadReject.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 7),
        new System.Drawing.Point(5, 27)};
            this.customButtonLoadReject.English_TextNumberInTextGroup = new int[] {
        2};
            this.customButtonLoadReject.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(54, 23),
        new System.Drawing.Size(65, 23)};
            this.customButtonLoadReject.FocusBackgroundDisplay = false;
            this.customButtonLoadReject.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonLoadReject.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonLoadReject.ForeColor = System.Drawing.Color.White;
            this.customButtonLoadReject.HoverBackgroundDisplay = false;
            this.customButtonLoadReject.IconIndex = new int[] {
        1,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonLoadReject.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(85, 10),
        new System.Drawing.Point(85, 10)};
            this.customButtonLoadReject.IconNumber = 2;
            this.customButtonLoadReject.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(53, 37),
        new System.Drawing.Size(53, 37)};
            this.customButtonLoadReject.LabelControlMode = false;
            this.customButtonLoadReject.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonLoadReject.Location = new System.Drawing.Point(878, 492);
            this.customButtonLoadReject.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonLoadReject.Name = "customButtonLoadReject";
            this.customButtonLoadReject.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonLoadReject.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonLoadReject.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonLoadReject.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonLoadReject.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonLoadReject.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonLoadReject.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonLoadReject.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonLoadReject.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonLoadReject.Size = new System.Drawing.Size(140, 56);
            this.customButtonLoadReject.SizeButton = new System.Drawing.Size(140, 56);
            this.customButtonLoadReject.TabIndex = 75;
            this.customButtonLoadReject.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonLoadReject.TextGroupNumber = 1;
            this.customButtonLoadReject.UpdateControl = true;
            this.customButtonLoadReject.CustomButton_Click += new System.EventHandler(this.customButtonLoadReject_CustomButton_Click);
            // 
            // customButtonManageTools
            // 
            this.customButtonManageTools.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonManageTools.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonManageTools.BitmapIconWhole = global::VisionSystemControlLibrary.Properties.Resources.RunStop;
            this.customButtonManageTools.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.Button;
            this.customButtonManageTools.Chinese_TextDisplay = new string[] {
        "管理工具"};
            this.customButtonManageTools.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 17)};
            this.customButtonManageTools.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonManageTools.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(72, 23)};
            this.customButtonManageTools.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonManageTools.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonManageTools.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonManageTools.CurrentTextGroupIndex = 0;
            this.customButtonManageTools.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonManageTools.CustomButtonData = null;
            this.customButtonManageTools.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonManageTools.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Switch;
            this.customButtonManageTools.DrawIcon = true;
            this.customButtonManageTools.DrawText = true;
            this.customButtonManageTools.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonManageTools.English_TextDisplay = new string[] {
        "MANAGE&TOOLS"};
            this.customButtonManageTools.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 7),
        new System.Drawing.Point(5, 27)};
            this.customButtonManageTools.English_TextNumberInTextGroup = new int[] {
        2};
            this.customButtonManageTools.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(84, 23),
        new System.Drawing.Size(62, 23)};
            this.customButtonManageTools.FocusBackgroundDisplay = false;
            this.customButtonManageTools.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonManageTools.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonManageTools.ForeColor = System.Drawing.Color.White;
            this.customButtonManageTools.HoverBackgroundDisplay = false;
            this.customButtonManageTools.IconIndex = new int[] {
        3,
        2,
        1,
        0,
        0,
        1,
        0,
        1,
        0};
            this.customButtonManageTools.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(88, 10),
        new System.Drawing.Point(88, 10),
        new System.Drawing.Point(88, 10),
        new System.Drawing.Point(88, 10)};
            this.customButtonManageTools.IconNumber = 4;
            this.customButtonManageTools.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(53, 37),
        new System.Drawing.Size(53, 37),
        new System.Drawing.Size(53, 37),
        new System.Drawing.Size(53, 37)};
            this.customButtonManageTools.LabelControlMode = false;
            this.customButtonManageTools.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonManageTools.Location = new System.Drawing.Point(878, 372);
            this.customButtonManageTools.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonManageTools.Name = "customButtonManageTools";
            this.customButtonManageTools.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonManageTools.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonManageTools.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonManageTools.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonManageTools.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonManageTools.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonManageTools.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonManageTools.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonManageTools.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonManageTools.Size = new System.Drawing.Size(140, 56);
            this.customButtonManageTools.SizeButton = new System.Drawing.Size(140, 56);
            this.customButtonManageTools.TabIndex = 74;
            this.customButtonManageTools.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonManageTools.TextGroupNumber = 1;
            this.customButtonManageTools.UpdateControl = true;
            this.customButtonManageTools.CustomButton_Click += new System.EventHandler(this.customButtonManageTools_CustomButton_Click);
            // 
            // customButtonLoadSample
            // 
            this.customButtonLoadSample.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonLoadSample.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonLoadSample.BitmapIconWhole = global::VisionSystemControlLibrary.Properties.Resources.LoadSample;
            this.customButtonLoadSample.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.Button;
            this.customButtonLoadSample.Chinese_TextDisplay = new string[] {
        "加载模板"};
            this.customButtonLoadSample.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 17)};
            this.customButtonLoadSample.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonLoadSample.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(72, 23)};
            this.customButtonLoadSample.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonLoadSample.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonLoadSample.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonLoadSample.CurrentTextGroupIndex = 0;
            this.customButtonLoadSample.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonLoadSample.CustomButtonData = null;
            this.customButtonLoadSample.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonLoadSample.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Switch;
            this.customButtonLoadSample.DrawIcon = true;
            this.customButtonLoadSample.DrawText = true;
            this.customButtonLoadSample.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonLoadSample.English_TextDisplay = new string[] {
        "LOAD&SAMPLE"};
            this.customButtonLoadSample.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 7),
        new System.Drawing.Point(5, 27)};
            this.customButtonLoadSample.English_TextNumberInTextGroup = new int[] {
        2};
            this.customButtonLoadSample.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(54, 23),
        new System.Drawing.Size(74, 23)};
            this.customButtonLoadSample.FocusBackgroundDisplay = false;
            this.customButtonLoadSample.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonLoadSample.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonLoadSample.ForeColor = System.Drawing.Color.White;
            this.customButtonLoadSample.HoverBackgroundDisplay = false;
            this.customButtonLoadSample.IconIndex = new int[] {
        1,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonLoadSample.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(85, 10),
        new System.Drawing.Point(85, 10)};
            this.customButtonLoadSample.IconNumber = 2;
            this.customButtonLoadSample.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(53, 37),
        new System.Drawing.Size(53, 37)};
            this.customButtonLoadSample.LabelControlMode = false;
            this.customButtonLoadSample.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonLoadSample.Location = new System.Drawing.Point(878, 312);
            this.customButtonLoadSample.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonLoadSample.Name = "customButtonLoadSample";
            this.customButtonLoadSample.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonLoadSample.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonLoadSample.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonLoadSample.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonLoadSample.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonLoadSample.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonLoadSample.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonLoadSample.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonLoadSample.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonLoadSample.Size = new System.Drawing.Size(140, 56);
            this.customButtonLoadSample.SizeButton = new System.Drawing.Size(140, 56);
            this.customButtonLoadSample.TabIndex = 73;
            this.customButtonLoadSample.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonLoadSample.TextGroupNumber = 1;
            this.customButtonLoadSample.UpdateControl = true;
            this.customButtonLoadSample.CustomButton_Click += new System.EventHandler(this.customButtonLoadSample_CustomButton_Click);
            // 
            // customButtonSaveProduct
            // 
            this.customButtonSaveProduct.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonSaveProduct.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonSaveProduct.BitmapIconWhole = global::VisionSystemControlLibrary.Properties.Resources.SaveCurrentBrand;
            this.customButtonSaveProduct.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.Button;
            this.customButtonSaveProduct.Chinese_TextDisplay = new string[] {
        "保存产品"};
            this.customButtonSaveProduct.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 17)};
            this.customButtonSaveProduct.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonSaveProduct.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(72, 23)};
            this.customButtonSaveProduct.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonSaveProduct.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonSaveProduct.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonSaveProduct.CurrentTextGroupIndex = 0;
            this.customButtonSaveProduct.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonSaveProduct.CustomButtonData = null;
            this.customButtonSaveProduct.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonSaveProduct.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonSaveProduct.DrawIcon = true;
            this.customButtonSaveProduct.DrawText = true;
            this.customButtonSaveProduct.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonSaveProduct.English_TextDisplay = new string[] {
        "SAVE&PRODUCT"};
            this.customButtonSaveProduct.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 7),
        new System.Drawing.Point(5, 27)};
            this.customButtonSaveProduct.English_TextNumberInTextGroup = new int[] {
        2};
            this.customButtonSaveProduct.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(49, 23),
        new System.Drawing.Size(89, 23)};
            this.customButtonSaveProduct.FocusBackgroundDisplay = false;
            this.customButtonSaveProduct.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonSaveProduct.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonSaveProduct.ForeColor = System.Drawing.Color.White;
            this.customButtonSaveProduct.HoverBackgroundDisplay = false;
            this.customButtonSaveProduct.IconIndex = new int[] {
        1,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonSaveProduct.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(80, 10),
        new System.Drawing.Point(80, 10)};
            this.customButtonSaveProduct.IconNumber = 2;
            this.customButtonSaveProduct.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(53, 37),
        new System.Drawing.Size(53, 37)};
            this.customButtonSaveProduct.LabelControlMode = false;
            this.customButtonSaveProduct.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonSaveProduct.Location = new System.Drawing.Point(878, 132);
            this.customButtonSaveProduct.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonSaveProduct.Name = "customButtonSaveProduct";
            this.customButtonSaveProduct.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonSaveProduct.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonSaveProduct.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonSaveProduct.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonSaveProduct.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonSaveProduct.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonSaveProduct.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonSaveProduct.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonSaveProduct.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonSaveProduct.Size = new System.Drawing.Size(140, 56);
            this.customButtonSaveProduct.SizeButton = new System.Drawing.Size(140, 56);
            this.customButtonSaveProduct.TabIndex = 72;
            this.customButtonSaveProduct.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonSaveProduct.TextGroupNumber = 1;
            this.customButtonSaveProduct.UpdateControl = true;
            this.customButtonSaveProduct.CustomButton_Click += new System.EventHandler(this.customButtonSaveProduct_CustomButton_Click);
            // 
            // customButtonLearnSample
            // 
            this.customButtonLearnSample.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonLearnSample.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonLearnSample.BitmapIconWhole = global::VisionSystemControlLibrary.Properties.Resources.Learn;
            this.customButtonLearnSample.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.Button;
            this.customButtonLearnSample.Chinese_TextDisplay = new string[] {
        "更新阈值&创建模板"};
            this.customButtonLearnSample.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 17),
        new System.Drawing.Point(5, 17)};
            this.customButtonLearnSample.Chinese_TextNumberInTextGroup = new int[] {
        1,
        1};
            this.customButtonLearnSample.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(72, 23),
        new System.Drawing.Size(72, 23)};
            this.customButtonLearnSample.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonLearnSample.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonLearnSample.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonLearnSample.CurrentTextGroupIndex = 0;
            this.customButtonLearnSample.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonLearnSample.CustomButtonData = null;
            this.customButtonLearnSample.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonLearnSample.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonLearnSample.DrawIcon = true;
            this.customButtonLearnSample.DrawText = true;
            this.customButtonLearnSample.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonLearnSample.English_TextDisplay = new string[] {
        "UPDATE&THRESHOLD&LEARN&SAMPLE"};
            this.customButtonLearnSample.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 7),
        new System.Drawing.Point(5, 27),
        new System.Drawing.Point(5, 7),
        new System.Drawing.Point(5, 27)};
            this.customButtonLearnSample.English_TextNumberInTextGroup = new int[] {
        2,
        2};
            this.customButtonLearnSample.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(75, 23),
        new System.Drawing.Size(110, 23),
        new System.Drawing.Size(62, 23),
        new System.Drawing.Size(74, 23)};
            this.customButtonLearnSample.FocusBackgroundDisplay = false;
            this.customButtonLearnSample.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonLearnSample.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonLearnSample.ForeColor = System.Drawing.Color.White;
            this.customButtonLearnSample.HoverBackgroundDisplay = false;
            this.customButtonLearnSample.IconIndex = new int[] {
        1,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonLearnSample.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(85, 10),
        new System.Drawing.Point(85, 10)};
            this.customButtonLearnSample.IconNumber = 2;
            this.customButtonLearnSample.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(53, 37),
        new System.Drawing.Size(53, 37)};
            this.customButtonLearnSample.LabelControlMode = false;
            this.customButtonLearnSample.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonLearnSample.Location = new System.Drawing.Point(878, 192);
            this.customButtonLearnSample.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonLearnSample.Name = "customButtonLearnSample";
            this.customButtonLearnSample.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonLearnSample.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonLearnSample.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonLearnSample.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonLearnSample.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonLearnSample.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonLearnSample.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonLearnSample.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonLearnSample.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonLearnSample.Size = new System.Drawing.Size(140, 56);
            this.customButtonLearnSample.SizeButton = new System.Drawing.Size(140, 56);
            this.customButtonLearnSample.TabIndex = 71;
            this.customButtonLearnSample.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonLearnSample.TextGroupNumber = 2;
            this.customButtonLearnSample.UpdateControl = true;
            this.customButtonLearnSample.CustomButton_Click += new System.EventHandler(this.customButtonLearnSample_CustomButton_Click);
            // 
            // customButtonLiveView
            // 
            this.customButtonLiveView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonLiveView.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(138)))), ((int)(((byte)(206)))));
            this.customButtonLiveView.BitmapIconWhole = global::VisionSystemControlLibrary.Properties.Resources.ConfigImage;
            this.customButtonLiveView.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.Button;
            this.customButtonLiveView.Chinese_TextDisplay = new string[] {
        "在线图像"};
            this.customButtonLiveView.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 17)};
            this.customButtonLiveView.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonLiveView.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(72, 23)};
            this.customButtonLiveView.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonLiveView.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonLiveView.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonLiveView.CurrentTextGroupIndex = 0;
            this.customButtonLiveView.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonLiveView.CustomButtonData = null;
            this.customButtonLiveView.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonLiveView.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Switch;
            this.customButtonLiveView.DrawIcon = true;
            this.customButtonLiveView.DrawText = true;
            this.customButtonLiveView.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonLiveView.English_TextDisplay = new string[] {
        "LIVE&VIEW"};
            this.customButtonLiveView.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 7),
        new System.Drawing.Point(5, 27)};
            this.customButtonLiveView.English_TextNumberInTextGroup = new int[] {
        2};
            this.customButtonLiveView.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(42, 23),
        new System.Drawing.Size(50, 23)};
            this.customButtonLiveView.FocusBackgroundDisplay = false;
            this.customButtonLiveView.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonLiveView.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonLiveView.ForeColor = System.Drawing.Color.White;
            this.customButtonLiveView.HoverBackgroundDisplay = false;
            this.customButtonLiveView.IconIndex = new int[] {
        1,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonLiveView.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(85, 10),
        new System.Drawing.Point(85, 10)};
            this.customButtonLiveView.IconNumber = 2;
            this.customButtonLiveView.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(53, 37),
        new System.Drawing.Size(53, 37)};
            this.customButtonLiveView.LabelControlMode = false;
            this.customButtonLiveView.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonLiveView.Location = new System.Drawing.Point(878, 252);
            this.customButtonLiveView.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonLiveView.Name = "customButtonLiveView";
            this.customButtonLiveView.RectBottom = new System.Drawing.Rectangle(10, 162, 150, 10);
            this.customButtonLiveView.RectFill = new System.Drawing.Rectangle(10, 10, 150, 150);
            this.customButtonLiveView.RectLeft = new System.Drawing.Rectangle(0, 10, 10, 150);
            this.customButtonLiveView.RectLeftBottom = new System.Drawing.Rectangle(0, 162, 10, 10);
            this.customButtonLiveView.RectLeftTop = new System.Drawing.Rectangle(0, 0, 10, 10);
            this.customButtonLiveView.RectRight = new System.Drawing.Rectangle(162, 10, 10, 150);
            this.customButtonLiveView.RectRightBottom = new System.Drawing.Rectangle(162, 162, 10, 10);
            this.customButtonLiveView.RectRightTop = new System.Drawing.Rectangle(162, 0, 10, 10);
            this.customButtonLiveView.RectTop = new System.Drawing.Rectangle(10, 0, 150, 10);
            this.customButtonLiveView.Size = new System.Drawing.Size(140, 56);
            this.customButtonLiveView.SizeButton = new System.Drawing.Size(140, 56);
            this.customButtonLiveView.TabIndex = 69;
            this.customButtonLiveView.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonLiveView.TextGroupNumber = 1;
            this.customButtonLiveView.UpdateControl = true;
            this.customButtonLiveView.CustomButton_Click += new System.EventHandler(this.customButtonLiveView_CustomButton_Click);
            // 
            // customButtonSelectedCheckValue
            // 
            this.customButtonSelectedCheckValue.BackColor = System.Drawing.Color.Black;
            this.customButtonSelectedCheckValue.BackgroundColor = System.Drawing.Color.Black;
            this.customButtonSelectedCheckValue.BitmapIconWhole = null;
            this.customButtonSelectedCheckValue.BitmapWhole = null;
            this.customButtonSelectedCheckValue.Chinese_TextDisplay = new string[] {
        "工具名称"};
            this.customButtonSelectedCheckValue.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 9)};
            this.customButtonSelectedCheckValue.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonSelectedCheckValue.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(66, 22)};
            this.customButtonSelectedCheckValue.ColorTextDisable = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
            this.customButtonSelectedCheckValue.ColorTextEnable = System.Drawing.Color.Yellow;
            this.customButtonSelectedCheckValue.ColorTextSelected = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonSelectedCheckValue.CurrentTextGroupIndex = 0;
            this.customButtonSelectedCheckValue.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonSelectedCheckValue.CustomButtonData = null;
            this.customButtonSelectedCheckValue.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Automatic;
            this.customButtonSelectedCheckValue.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonSelectedCheckValue.DrawIcon = false;
            this.customButtonSelectedCheckValue.DrawText = true;
            this.customButtonSelectedCheckValue.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonSelectedCheckValue.English_TextDisplay = new string[] {
        "Tool Name"};
            this.customButtonSelectedCheckValue.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 9)};
            this.customButtonSelectedCheckValue.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonSelectedCheckValue.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(89, 22)};
            this.customButtonSelectedCheckValue.FocusBackgroundDisplay = false;
            this.customButtonSelectedCheckValue.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonSelectedCheckValue.FontText = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonSelectedCheckValue.ForeColor = System.Drawing.Color.White;
            this.customButtonSelectedCheckValue.HoverBackgroundDisplay = false;
            this.customButtonSelectedCheckValue.IconIndex = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonSelectedCheckValue.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonSelectedCheckValue.IconNumber = 1;
            this.customButtonSelectedCheckValue.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(0, 0)};
            this.customButtonSelectedCheckValue.LabelControlMode = true;
            this.customButtonSelectedCheckValue.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonSelectedCheckValue.Location = new System.Drawing.Point(170, 69);
            this.customButtonSelectedCheckValue.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonSelectedCheckValue.Name = "customButtonSelectedCheckValue";
            this.customButtonSelectedCheckValue.RectBottom = new System.Drawing.Rectangle(3, 23, 237, 3);
            this.customButtonSelectedCheckValue.RectFill = new System.Drawing.Rectangle(3, 3, 237, 20);
            this.customButtonSelectedCheckValue.RectLeft = new System.Drawing.Rectangle(0, 3, 3, 20);
            this.customButtonSelectedCheckValue.RectLeftBottom = new System.Drawing.Rectangle(0, 23, 3, 3);
            this.customButtonSelectedCheckValue.RectLeftTop = new System.Drawing.Rectangle(0, 0, 3, 3);
            this.customButtonSelectedCheckValue.RectRight = new System.Drawing.Rectangle(240, 3, 3, 20);
            this.customButtonSelectedCheckValue.RectRightBottom = new System.Drawing.Rectangle(240, 23, 3, 3);
            this.customButtonSelectedCheckValue.RectRightTop = new System.Drawing.Rectangle(240, 0, 3, 3);
            this.customButtonSelectedCheckValue.RectTop = new System.Drawing.Rectangle(3, 0, 237, 3);
            this.customButtonSelectedCheckValue.Size = new System.Drawing.Size(490, 40);
            this.customButtonSelectedCheckValue.SizeButton = new System.Drawing.Size(490, 40);
            this.customButtonSelectedCheckValue.TabIndex = 66;
            this.customButtonSelectedCheckValue.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonSelectedCheckValue.TextGroupNumber = 1;
            this.customButtonSelectedCheckValue.UpdateControl = true;
            // 
            // customButtonCheckTimeText
            // 
            this.customButtonCheckTimeText.BackColor = System.Drawing.Color.Black;
            this.customButtonCheckTimeText.BackgroundColor = System.Drawing.Color.Black;
            this.customButtonCheckTimeText.BitmapIconWhole = null;
            this.customButtonCheckTimeText.BitmapWhole = null;
            this.customButtonCheckTimeText.Chinese_TextDisplay = new string[] {
        "检测时间（ms）："};
            this.customButtonCheckTimeText.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(147, 9)};
            this.customButtonCheckTimeText.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonCheckTimeText.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(133, 22)};
            this.customButtonCheckTimeText.ColorTextDisable = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
            this.customButtonCheckTimeText.ColorTextEnable = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonCheckTimeText.ColorTextSelected = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonCheckTimeText.CurrentTextGroupIndex = 0;
            this.customButtonCheckTimeText.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonCheckTimeText.CustomButtonData = null;
            this.customButtonCheckTimeText.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Automatic;
            this.customButtonCheckTimeText.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonCheckTimeText.DrawIcon = false;
            this.customButtonCheckTimeText.DrawText = true;
            this.customButtonCheckTimeText.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonCheckTimeText.English_TextDisplay = new string[] {
        "INSPECTION TIME（ms）："};
            this.customButtonCheckTimeText.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(68, 9)};
            this.customButtonCheckTimeText.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonCheckTimeText.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(212, 22)};
            this.customButtonCheckTimeText.FocusBackgroundDisplay = false;
            this.customButtonCheckTimeText.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonCheckTimeText.FontText = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonCheckTimeText.ForeColor = System.Drawing.Color.White;
            this.customButtonCheckTimeText.HoverBackgroundDisplay = false;
            this.customButtonCheckTimeText.IconIndex = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonCheckTimeText.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonCheckTimeText.IconNumber = 1;
            this.customButtonCheckTimeText.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(0, 0)};
            this.customButtonCheckTimeText.LabelControlMode = true;
            this.customButtonCheckTimeText.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonCheckTimeText.Location = new System.Drawing.Point(660, 69);
            this.customButtonCheckTimeText.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonCheckTimeText.Name = "customButtonCheckTimeText";
            this.customButtonCheckTimeText.RectBottom = new System.Drawing.Rectangle(3, 23, 237, 3);
            this.customButtonCheckTimeText.RectFill = new System.Drawing.Rectangle(3, 3, 237, 20);
            this.customButtonCheckTimeText.RectLeft = new System.Drawing.Rectangle(0, 3, 3, 20);
            this.customButtonCheckTimeText.RectLeftBottom = new System.Drawing.Rectangle(0, 23, 3, 3);
            this.customButtonCheckTimeText.RectLeftTop = new System.Drawing.Rectangle(0, 0, 3, 3);
            this.customButtonCheckTimeText.RectRight = new System.Drawing.Rectangle(240, 3, 3, 20);
            this.customButtonCheckTimeText.RectRightBottom = new System.Drawing.Rectangle(240, 23, 3, 3);
            this.customButtonCheckTimeText.RectRightTop = new System.Drawing.Rectangle(240, 0, 3, 3);
            this.customButtonCheckTimeText.RectTop = new System.Drawing.Rectangle(3, 0, 237, 3);
            this.customButtonCheckTimeText.Size = new System.Drawing.Size(280, 40);
            this.customButtonCheckTimeText.SizeButton = new System.Drawing.Size(280, 40);
            this.customButtonCheckTimeText.TabIndex = 65;
            this.customButtonCheckTimeText.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.customButtonCheckTimeText.TextGroupNumber = 1;
            this.customButtonCheckTimeText.UpdateControl = true;
            // 
            // customButtonSelectedCheckText
            // 
            this.customButtonSelectedCheckText.BackColor = System.Drawing.Color.Black;
            this.customButtonSelectedCheckText.BackgroundColor = System.Drawing.Color.Black;
            this.customButtonSelectedCheckText.BitmapIconWhole = null;
            this.customButtonSelectedCheckText.BitmapWhole = null;
            this.customButtonSelectedCheckText.Chinese_TextDisplay = new string[] {
        "选择的检测工具："};
            this.customButtonSelectedCheckText.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(34, 9)};
            this.customButtonSelectedCheckText.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonSelectedCheckText.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(126, 22)};
            this.customButtonSelectedCheckText.ColorTextDisable = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))));
            this.customButtonSelectedCheckText.ColorTextEnable = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonSelectedCheckText.ColorTextSelected = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customButtonSelectedCheckText.CurrentTextGroupIndex = 0;
            this.customButtonSelectedCheckText.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonSelectedCheckText.CustomButtonData = null;
            this.customButtonSelectedCheckText.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Automatic;
            this.customButtonSelectedCheckText.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonSelectedCheckText.DrawIcon = false;
            this.customButtonSelectedCheckText.DrawText = true;
            this.customButtonSelectedCheckText.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonSelectedCheckText.English_TextDisplay = new string[] {
        "SELECTED CHECK："};
            this.customButtonSelectedCheckText.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(8, 9)};
            this.customButtonSelectedCheckText.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonSelectedCheckText.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(152, 22)};
            this.customButtonSelectedCheckText.FocusBackgroundDisplay = false;
            this.customButtonSelectedCheckText.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonSelectedCheckText.FontText = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonSelectedCheckText.ForeColor = System.Drawing.Color.White;
            this.customButtonSelectedCheckText.HoverBackgroundDisplay = false;
            this.customButtonSelectedCheckText.IconIndex = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonSelectedCheckText.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonSelectedCheckText.IconNumber = 1;
            this.customButtonSelectedCheckText.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(0, 0)};
            this.customButtonSelectedCheckText.LabelControlMode = true;
            this.customButtonSelectedCheckText.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonSelectedCheckText.Location = new System.Drawing.Point(10, 69);
            this.customButtonSelectedCheckText.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonSelectedCheckText.Name = "customButtonSelectedCheckText";
            this.customButtonSelectedCheckText.RectBottom = new System.Drawing.Rectangle(3, 23, 237, 3);
            this.customButtonSelectedCheckText.RectFill = new System.Drawing.Rectangle(3, 3, 237, 20);
            this.customButtonSelectedCheckText.RectLeft = new System.Drawing.Rectangle(0, 3, 3, 20);
            this.customButtonSelectedCheckText.RectLeftBottom = new System.Drawing.Rectangle(0, 23, 3, 3);
            this.customButtonSelectedCheckText.RectLeftTop = new System.Drawing.Rectangle(0, 0, 3, 3);
            this.customButtonSelectedCheckText.RectRight = new System.Drawing.Rectangle(240, 3, 3, 20);
            this.customButtonSelectedCheckText.RectRightBottom = new System.Drawing.Rectangle(240, 23, 3, 3);
            this.customButtonSelectedCheckText.RectRightTop = new System.Drawing.Rectangle(240, 0, 3, 3);
            this.customButtonSelectedCheckText.RectTop = new System.Drawing.Rectangle(3, 0, 237, 3);
            this.customButtonSelectedCheckText.Size = new System.Drawing.Size(160, 40);
            this.customButtonSelectedCheckText.SizeButton = new System.Drawing.Size(160, 40);
            this.customButtonSelectedCheckText.TabIndex = 64;
            this.customButtonSelectedCheckText.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.customButtonSelectedCheckText.TextGroupNumber = 1;
            this.customButtonSelectedCheckText.UpdateControl = true;
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
            this.customButtonClose.TabIndex = 63;
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
        " - 质量检测"};
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
        " - QUALITY CHECK"};
            this.customButtonCaption.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(289, 7)};
            this.customButtonCaption.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonCaption.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(198, 29)};
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
            this.customButtonCaption.TabIndex = 62;
            this.customButtonCaption.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.customButtonCaption.TextGroupNumber = 1;
            this.customButtonCaption.UpdateControl = true;
            // 
            // customListTool
            // 
            this.customListTool.BackColor = System.Drawing.Color.Black;
            this.customListTool.BitmapBackgroundIndex = new int[] {
        0,
        0};
            this.customListTool.BitmapBackgroundNumber = 1;
            this.customListTool.BitmapBackgroundWhole = ((System.Drawing.Bitmap)(resources.GetObject("customListTool.BitmapBackgroundWhole")));
            this.customListTool.BitmapIcon = new System.Drawing.Bitmap[] {
        global::VisionSystemControlLibrary.Properties.Resources.CustomListItem_Marked};
            this.customListTool.Chinese_ColumnNameDisplay = new string[] {
        "工具",
        "选择"};
            this.customListTool.ColorControlBackground = System.Drawing.Color.Black;
            this.customListTool.ColorItemBackgroundSelected = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.customListTool.ColorItemBackgroundUnselected = System.Drawing.Color.Black;
            this.customListTool.ColorItemTextSelected = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customListTool.ColorItemTextUnselected = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.customListTool.ColorListHeaderBackground = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.customListTool.ColorListHeaderColumnText_Disable = new System.Drawing.Color[] {
        System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(168)))), ((int)(((byte)(153)))))};
            this.customListTool.ColorListHeaderColumnText_Enable = new System.Drawing.Color[] {
        System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))))};
            this.customListTool.ColorListItemBackground = System.Drawing.Color.Black;
            this.customListTool.ColorPageBackground = System.Drawing.Color.Black;
            this.customListTool.ColorPageText = System.Drawing.Color.Yellow;
            this.customListTool.ColumnNameXOffSetValue = 7;
            this.customListTool.ColumnNumber = 2;
            this.customListTool.ColumnWidth = new int[] {
        154,
        55};
            this.customListTool.CurrentColumnNameGroupIndex = new int[] {
        0,
        0};
            this.customListTool.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customListTool.English_ColumnNameDisplay = new string[] {
        "Tool",
        "Selected"};
            this.customListTool.FontListHeader = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customListTool.FontListItem = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customListTool.FontPage = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customListTool.ForeColor = System.Drawing.Color.White;
            this.customListTool.ItemDataDisplay = new bool[] {
        true,
        true};
            this.customListTool.ItemDataNumber = 0;
            this.customListTool.ItemIconIndex = new int[] {
        -1,
        -1};
            this.customListTool.ItemIconNumber = 1;
            this.customListTool.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customListTool.ListEnabled = true;
            this.customListTool.ListHeaderHeight = 26;
            this.customListTool.ListHeaderTextAlignment = System.Drawing.StringAlignment.Near;
            this.customListTool.ListItemHeight = 25;
            this.customListTool.ListItemTextAlignment = System.Drawing.StringAlignment.Near;
            this.customListTool.ListItemXOffSetValue = 7;
            this.customListTool.Location = new System.Drawing.Point(5, 120);
            this.customListTool.Name = "customListTool";
            this.customListTool.PageHeight = 25;
            this.customListTool.RectBottom = new System.Drawing.Rectangle(3, 23, 237, 3);
            this.customListTool.RectFill = new System.Drawing.Rectangle(3, 3, 237, 20);
            this.customListTool.RectLeft = new System.Drawing.Rectangle(0, 3, 3, 20);
            this.customListTool.RectLeftBottom = new System.Drawing.Rectangle(0, 23, 3, 3);
            this.customListTool.RectLeftTop = new System.Drawing.Rectangle(0, 0, 3, 3);
            this.customListTool.RectRight = new System.Drawing.Rectangle(240, 3, 3, 20);
            this.customListTool.RectRightBottom = new System.Drawing.Rectangle(240, 23, 3, 3);
            this.customListTool.RectRightTop = new System.Drawing.Rectangle(240, 0, 3, 3);
            this.customListTool.RectTop = new System.Drawing.Rectangle(3, 0, 237, 3);
            this.customListTool.SelectedItemNumber = 0;
            this.customListTool.SelectedItemType = false;
            this.customListTool.SelectionColumnIndex = 1;
            this.customListTool.Size = new System.Drawing.Size(220, 480);
            this.customListTool.SizeControl = new System.Drawing.Size(220, 480);
            this.customListTool.SizeItemIcon = new System.Drawing.Size[] {
        new System.Drawing.Size(20, 20),
        new System.Drawing.Size(20, 20)};
            this.customListTool.TabIndex = 61;
            this.customListTool.Visible = false;
            this.customListTool.CustomListItem_Click += new System.EventHandler(this.customListTool_CustomListItem_Click);
            // 
            // imageDisplayView
            // 
            this.imageDisplayView.AutoShowTitle = false;
            this.imageDisplayView.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.imageDisplayView.BitmapDisplay = null;
            this.imageDisplayView.ColorStatusBarControlBackground = System.Drawing.Color.FromArgb(((int)(((byte)(126)))), ((int)(((byte)(126)))), ((int)(((byte)(126)))));
            this.imageDisplayView.ControlScale = 1D;
            this.imageDisplayView.ControlScale_X = 1D;
            this.imageDisplayView.ControlScale_Y = 1D;
            this.imageDisplayView.ControlSize = new System.Drawing.Size(640, 480);
            this.imageDisplayView.CurrentValueFont = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.imageDisplayView.CurrentValueLocation = new System.Drawing.Point(353, 28);
            this.imageDisplayView.CurrentValueSize = new System.Drawing.Size(109, 15);
            this.imageDisplayView.ImageFileName = "1.jpg";
            this.imageDisplayView.ImageFilePath = ".\\ConfigData\\RejectsImage\\";
            this.imageDisplayView.Information = ((VisionSystemClassLibrary.Struct.ImageInformation)(resources.GetObject("imageDisplayView.Information")));
            this.imageDisplayView.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.imageDisplayView.Location = new System.Drawing.Point(231, 120);
            this.imageDisplayView.MaxValueFont = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.imageDisplayView.MaxValueLocation = new System.Drawing.Point(473, 28);
            this.imageDisplayView.MaxValueSize = new System.Drawing.Size(109, 15);
            this.imageDisplayView.MessageFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.imageDisplayView.MessageLampLocation = new System.Drawing.Point(590, 10);
            this.imageDisplayView.MessageLampSize = new System.Drawing.Size(31, 32);
            this.imageDisplayView.MessageLocation = new System.Drawing.Point(8, 8);
            this.imageDisplayView.MessageSize = new System.Drawing.Size(540, 18);
            this.imageDisplayView.MinValueFont = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.imageDisplayView.MinValueLocation = new System.Drawing.Point(234, 28);
            this.imageDisplayView.MinValueSize = new System.Drawing.Size(109, 15);
            this.imageDisplayView.Name = "imageDisplayView";
            this.imageDisplayView.ShowTitle = true;
            this.imageDisplayView.Size = new System.Drawing.Size(640, 480);
            this.imageDisplayView.SlotLocation = new System.Drawing.Point(13, 31);
            this.imageDisplayView.SlotSize = new System.Drawing.Size(216, 12);
            this.imageDisplayView.StatusBarControlScale = 1D;
            this.imageDisplayView.StatusBarControlScale_X = 0.99375D;
            this.imageDisplayView.StatusBarControlScale_Y = 1D;
            this.imageDisplayView.StatusBarControlSize = new System.Drawing.Size(636, 50);
            this.imageDisplayView.TabIndex = 92;
            this.imageDisplayView.YOffset = 2;
            this.imageDisplayView.Control_DoubleClick += new System.EventHandler(this.imageDisplayView_Control_DoubleClick);
            this.imageDisplayView.Control_MouseDown += new System.EventHandler(this.imageDisplayView_Control_MouseDown);
            this.imageDisplayView.Control_MouseMove += new System.EventHandler(this.imageDisplayView_Control_MouseMove);
            this.imageDisplayView.Control_MouseUp += new System.EventHandler(this.imageDisplayView_Control_MouseUp);
            // 
            // customButtonImageDisplayViewBackground
            // 
            this.customButtonImageDisplayViewBackground.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.customButtonImageDisplayViewBackground.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.customButtonImageDisplayViewBackground.BitmapIconWhole = null;
            this.customButtonImageDisplayViewBackground.BitmapWhole = global::VisionSystemControlLibrary.Properties.Resources.CameraBackground;
            this.customButtonImageDisplayViewBackground.Chinese_TextDisplay = new string[] {
        "放大背景"};
            this.customButtonImageDisplayViewBackground.Chinese_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 14)};
            this.customButtonImageDisplayViewBackground.Chinese_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonImageDisplayViewBackground.Chinese_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(39, 23)};
            this.customButtonImageDisplayViewBackground.ColorTextDisable = System.Drawing.Color.Silver;
            this.customButtonImageDisplayViewBackground.ColorTextEnable = System.Drawing.Color.White;
            this.customButtonImageDisplayViewBackground.ColorTextSelected = System.Drawing.Color.SpringGreen;
            this.customButtonImageDisplayViewBackground.CurrentTextGroupIndex = 0;
            this.customButtonImageDisplayViewBackground.CustomButtonBackgroundImage = VisionSystemControlLibrary.CustomButton_BackgroundImage.Up;
            this.customButtonImageDisplayViewBackground.CustomButtonData = null;
            this.customButtonImageDisplayViewBackground.CustomButtonDrawTextType = VisionSystemControlLibrary.CustomButton_DrawTextType.Manual;
            this.customButtonImageDisplayViewBackground.CustomButtonType = VisionSystemControlLibrary.CustomButton_Type.Normal;
            this.customButtonImageDisplayViewBackground.DrawIcon = true;
            this.customButtonImageDisplayViewBackground.DrawText = false;
            this.customButtonImageDisplayViewBackground.DrawType = VisionSystemControlLibrary.CustomDrawType.Central;
            this.customButtonImageDisplayViewBackground.English_TextDisplay = new string[] {
        "ZOOMIN BACKGROUND"};
            this.customButtonImageDisplayViewBackground.English_TextLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(5, 14)};
            this.customButtonImageDisplayViewBackground.English_TextNumberInTextGroup = new int[] {
        1};
            this.customButtonImageDisplayViewBackground.English_TextSize = new System.Drawing.Size[] {
        new System.Drawing.Size(52, 23)};
            this.customButtonImageDisplayViewBackground.FocusBackgroundDisplay = false;
            this.customButtonImageDisplayViewBackground.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonImageDisplayViewBackground.FontText = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customButtonImageDisplayViewBackground.ForeColor = System.Drawing.Color.White;
            this.customButtonImageDisplayViewBackground.HoverBackgroundDisplay = false;
            this.customButtonImageDisplayViewBackground.IconIndex = new int[] {
        1,
        1,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.customButtonImageDisplayViewBackground.IconLocation = new System.Drawing.Point[] {
        new System.Drawing.Point(0, 0)};
            this.customButtonImageDisplayViewBackground.IconNumber = 1;
            this.customButtonImageDisplayViewBackground.IconSize = new System.Drawing.Size[] {
        new System.Drawing.Size(0, 0)};
            this.customButtonImageDisplayViewBackground.LabelControlMode = true;
            this.customButtonImageDisplayViewBackground.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.customButtonImageDisplayViewBackground.Location = new System.Drawing.Point(228, 117);
            this.customButtonImageDisplayViewBackground.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.customButtonImageDisplayViewBackground.Name = "customButtonImageDisplayViewBackground";
            this.customButtonImageDisplayViewBackground.RectBottom = new System.Drawing.Rectangle(3, 103, 134, 3);
            this.customButtonImageDisplayViewBackground.RectFill = new System.Drawing.Rectangle(3, 3, 134, 100);
            this.customButtonImageDisplayViewBackground.RectLeft = new System.Drawing.Rectangle(0, 3, 3, 100);
            this.customButtonImageDisplayViewBackground.RectLeftBottom = new System.Drawing.Rectangle(0, 103, 3, 3);
            this.customButtonImageDisplayViewBackground.RectLeftTop = new System.Drawing.Rectangle(0, 0, 3, 3);
            this.customButtonImageDisplayViewBackground.RectRight = new System.Drawing.Rectangle(137, 3, 3, 100);
            this.customButtonImageDisplayViewBackground.RectRightBottom = new System.Drawing.Rectangle(137, 103, 3, 3);
            this.customButtonImageDisplayViewBackground.RectRightTop = new System.Drawing.Rectangle(137, 0, 3, 3);
            this.customButtonImageDisplayViewBackground.RectTop = new System.Drawing.Rectangle(3, 0, 134, 3);
            this.customButtonImageDisplayViewBackground.Size = new System.Drawing.Size(646, 486);
            this.customButtonImageDisplayViewBackground.SizeButton = new System.Drawing.Size(646, 486);
            this.customButtonImageDisplayViewBackground.TabIndex = 93;
            this.customButtonImageDisplayViewBackground.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.customButtonImageDisplayViewBackground.TextGroupNumber = 1;
            this.customButtonImageDisplayViewBackground.UpdateControl = true;
            // 
            // labelROIXY
            // 
            this.labelROIXY.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Bold);
            this.labelROIXY.ForeColor = System.Drawing.Color.White;
            this.labelROIXY.Location = new System.Drawing.Point(467, 607);
            this.labelROIXY.Name = "labelROIXY";
            this.labelROIXY.Size = new System.Drawing.Size(120, 25);
            this.labelROIXY.TabIndex = 98;
            this.labelROIXY.Text = "ROI（X，Y）=";
            this.labelROIXY.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelROIXYValue
            // 
            this.labelROIXYValue.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Bold);
            this.labelROIXYValue.ForeColor = System.Drawing.Color.Yellow;
            this.labelROIXYValue.Location = new System.Drawing.Point(587, 607);
            this.labelROIXYValue.Name = "labelROIXYValue";
            this.labelROIXYValue.Size = new System.Drawing.Size(100, 25);
            this.labelROIXYValue.TabIndex = 99;
            this.labelROIXYValue.Text = "1024，1024";
            this.labelROIXYValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelROIWHValue
            // 
            this.labelROIWHValue.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Bold);
            this.labelROIWHValue.ForeColor = System.Drawing.Color.Yellow;
            this.labelROIWHValue.Location = new System.Drawing.Point(587, 632);
            this.labelROIWHValue.Name = "labelROIWHValue";
            this.labelROIWHValue.Size = new System.Drawing.Size(100, 25);
            this.labelROIWHValue.TabIndex = 101;
            this.labelROIWHValue.Text = "1024，1024";
            this.labelROIWHValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelROIWH
            // 
            this.labelROIWH.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Bold);
            this.labelROIWH.ForeColor = System.Drawing.Color.White;
            this.labelROIWH.Location = new System.Drawing.Point(467, 632);
            this.labelROIWH.Name = "labelROIWH";
            this.labelROIWH.Size = new System.Drawing.Size(120, 25);
            this.labelROIWH.TabIndex = 100;
            this.labelROIWH.Text = "ROI（W，H）=";
            this.labelROIWH.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // QualityCheckControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.Controls.Add(this.labelROIWHValue);
            this.Controls.Add(this.labelROIWH);
            this.Controls.Add(this.labelROIXYValue);
            this.Controls.Add(this.labelROIXY);
            this.Controls.Add(this.customButtonEditTools);
            this.Controls.Add(this.labelIcon);
            this.Controls.Add(this.sizeAdjustedPanel);
            this.Controls.Add(this.parameterSettingsPanel);
            this.Controls.Add(this.customButtonBestContrastText);
            this.Controls.Add(this.customButtonDeltaYValue);
            this.Controls.Add(this.customButtonDeltaYText);
            this.Controls.Add(this.customButtonDeltaXValue);
            this.Controls.Add(this.customButtonDeltaXText);
            this.Controls.Add(this.customButtonColorValue);
            this.Controls.Add(this.customButtonColorText);
            this.Controls.Add(this.customButtonAxisValue);
            this.Controls.Add(this.customButtonAxisText);
            this.Controls.Add(this.customButtonMeasureTool);
            this.Controls.Add(this.customButtonNext);
            this.Controls.Add(this.customButtonPrevious);
            this.Controls.Add(this.customButtonRejectImageIndex);
            this.Controls.Add(this.customButtonStatusBar);
            this.Controls.Add(this.customButtonSubtract);
            this.Controls.Add(this.customButtonPlus);
            this.Controls.Add(this.customButtonLoadReject);
            this.Controls.Add(this.customButtonManageTools);
            this.Controls.Add(this.customButtonLoadSample);
            this.Controls.Add(this.customButtonSaveProduct);
            this.Controls.Add(this.customButtonLearnSample);
            this.Controls.Add(this.customButtonLiveView);
            this.Controls.Add(this.labelCheckTimeValue);
            this.Controls.Add(this.customButtonSelectedCheckValue);
            this.Controls.Add(this.customButtonCheckTimeText);
            this.Controls.Add(this.customButtonSelectedCheckText);
            this.Controls.Add(this.customButtonClose);
            this.Controls.Add(this.customButtonCaption);
            this.Controls.Add(this.customListTool);
            this.Controls.Add(this.labelHandleRightBottom);
            this.Controls.Add(this.labelHandleBottom);
            this.Controls.Add(this.labelHandleLeftBottom);
            this.Controls.Add(this.labelHandleRight);
            this.Controls.Add(this.labelHandleLeft);
            this.Controls.Add(this.labelHandleRightTop);
            this.Controls.Add(this.labelHandleTop);
            this.Controls.Add(this.labelHandleLeftTop);
            this.Controls.Add(this.labelBestContrastValue);
            this.Controls.Add(this.imageDisplayView);
            this.Controls.Add(this.customButtonImageDisplayViewBackground);
            this.DoubleBuffered = true;
            this.Name = "QualityCheckControl";
            this.Size = new System.Drawing.Size(1024, 662);
            this.Load += new System.EventHandler(this.QualityCheckControl_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelBestContrastValue;
        private System.Windows.Forms.Label labelHandleLeftTop;
        private System.Windows.Forms.Label labelHandleTop;
        private System.Windows.Forms.Label labelHandleRightTop;
        private System.Windows.Forms.Label labelHandleLeft;
        private System.Windows.Forms.Label labelHandleRight;
        private System.Windows.Forms.Label labelHandleLeftBottom;
        private System.Windows.Forms.Label labelHandleBottom;
        private System.Windows.Forms.Label labelHandleRightBottom;
        private ParameterSettingsPanel parameterSettingsPanel;
        private SizeAdjustedPanel sizeAdjustedPanel;
        private CustomList customListTool;
        private CustomButton customButtonCaption;
        private CustomButton customButtonClose;
        private CustomButton customButtonSelectedCheckText;
        private CustomButton customButtonCheckTimeText;
        private CustomButton customButtonSelectedCheckValue;
        private System.Windows.Forms.Label labelCheckTimeValue;
        private CustomButton customButtonLiveView;
        private CustomButton customButtonLearnSample;
        private CustomButton customButtonSaveProduct;
        private CustomButton customButtonLoadSample;
        private CustomButton customButtonManageTools;
        private CustomButton customButtonLoadReject;
        private CustomButton customButtonPlus;
        private CustomButton customButtonSubtract;
        private CustomButton customButtonStatusBar;
        private CustomButton customButtonRejectImageIndex;
        private CustomButton customButtonNext;
        private CustomButton customButtonPrevious;
        private CustomButton customButtonMeasureTool;
        private CustomButton customButtonAxisText;
        private CustomButton customButtonAxisValue;
        private CustomButton customButtonColorText;
        private CustomButton customButtonColorValue;
        private CustomButton customButtonDeltaYValue;
        private CustomButton customButtonDeltaYText;
        private CustomButton customButtonDeltaXValue;
        private CustomButton customButtonDeltaXText;
        private CustomButton customButtonBestContrastText;
        private ImageDisplay imageDisplayView;
        private CustomButton customButtonImageDisplayViewBackground;
        private System.Windows.Forms.Label labelIcon;
        private CustomButton customButtonEditTools;
        private System.Windows.Forms.Label labelROIXY;
        private System.Windows.Forms.Label labelROIXYValue;
        private System.Windows.Forms.Label labelROIWHValue;
        private System.Windows.Forms.Label labelROIWH;
    }
}
