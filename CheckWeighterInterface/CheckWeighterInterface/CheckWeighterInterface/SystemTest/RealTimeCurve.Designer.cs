
namespace CheckWeighterInterface.SystemTest
{
    partial class RealTimeCurve
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            DevExpress.XtraCharts.XYDiagram xyDiagram3 = new DevExpress.XtraCharts.XYDiagram();
            DevExpress.XtraCharts.Series series3 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.SplineSeriesView splineSeriesView3 = new DevExpress.XtraCharts.SplineSeriesView();
            DevExpress.XtraCharts.ChartTitle chartTitle3 = new DevExpress.XtraCharts.ChartTitle();
            this.chartControl_weighterSensorRealTimeData = new DevExpress.XtraCharts.ChartControl();
            this.labelControl_peakValue = new DevExpress.XtraEditors.LabelControl();
            this.labelControl_valleyValue = new DevExpress.XtraEditors.LabelControl();
            this.labelControl_averageValue = new DevExpress.XtraEditors.LabelControl();
            this.labelControl_peakValueVal = new DevExpress.XtraEditors.LabelControl();
            this.labelControl_valleyValueVal = new DevExpress.XtraEditors.LabelControl();
            this.labelControl_averageValueVal = new DevExpress.XtraEditors.LabelControl();
            this.labelControl_KG1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl_KG2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl_KG3 = new DevExpress.XtraEditors.LabelControl();
            this.panelControl_weightList = new DevExpress.XtraEditors.PanelControl();
            this.simpleButton_changeMode = new DevExpress.XtraEditors.SimpleButton();
            this.spinEdit_setXMinVal = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl_changeMode = new DevExpress.XtraEditors.LabelControl();
            this.simpleButton_modifyAxisRange = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl_yWholeRangeZoom = new DevExpress.XtraEditors.LabelControl();
            this.labelControl_xWholeRangeZoom = new DevExpress.XtraEditors.LabelControl();
            this.zoomTrackBarControl_yWholeRangeZoom = new DevExpress.XtraEditors.ZoomTrackBarControl();
            this.spinEdit_setYMaxVal = new DevExpress.XtraEditors.SpinEdit();
            this.spinEdit_setYMinVal = new DevExpress.XtraEditors.SpinEdit();
            this.spinEdit_setXMaxVal = new DevExpress.XtraEditors.SpinEdit();
            this.zoomTrackBarControl_xWholeRangeZoom = new DevExpress.XtraEditors.ZoomTrackBarControl();
            this.labelControl_yVisualRangeZoom = new DevExpress.XtraEditors.LabelControl();
            this.labelControl_setVisualRangeZoom = new DevExpress.XtraEditors.LabelControl();
            this.labelControl_xVisualRangeZoom = new DevExpress.XtraEditors.LabelControl();
            this.labelControl_setWholeRangeZoom = new DevExpress.XtraEditors.LabelControl();
            this.zoomTrackBarControl_yVisualRangeZoom = new DevExpress.XtraEditors.ZoomTrackBarControl();
            this.zoomTrackBarControl_xVisualRangeZoom = new DevExpress.XtraEditors.ZoomTrackBarControl();
            this.labelControl_setYMax = new DevExpress.XtraEditors.LabelControl();
            this.labelControl_setYMin = new DevExpress.XtraEditors.LabelControl();
            this.labelControl_setXMax = new DevExpress.XtraEditors.LabelControl();
            this.labelControl_setXMin = new DevExpress.XtraEditors.LabelControl();
            this.separatorControl_left = new DevExpress.XtraEditors.SeparatorControl();
            this.timer_getDataOnceFromSensor = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.chartControl_weighterSensorRealTimeData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(splineSeriesView3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl_weightList)).BeginInit();
            this.panelControl_weightList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit_setXMinVal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.zoomTrackBarControl_yWholeRangeZoom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.zoomTrackBarControl_yWholeRangeZoom.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit_setYMaxVal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit_setYMinVal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit_setXMaxVal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.zoomTrackBarControl_xWholeRangeZoom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.zoomTrackBarControl_xWholeRangeZoom.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.zoomTrackBarControl_yVisualRangeZoom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.zoomTrackBarControl_yVisualRangeZoom.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.zoomTrackBarControl_xVisualRangeZoom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.zoomTrackBarControl_xVisualRangeZoom.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.separatorControl_left)).BeginInit();
            this.SuspendLayout();
            // 
            // chartControl_weighterSensorRealTimeData
            // 
            xyDiagram3.AxisX.GridLines.Visible = true;
            xyDiagram3.AxisX.Interlaced = true;
            xyDiagram3.AxisX.Label.Font = new System.Drawing.Font("微软雅黑", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            xyDiagram3.AxisX.Title.Text = "传感器检测次数";
            xyDiagram3.AxisX.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
            xyDiagram3.AxisX.VisibleInPanesSerializable = "-1";
            xyDiagram3.AxisX.VisualRange.Auto = false;
            xyDiagram3.AxisX.VisualRange.AutoSideMargins = false;
            xyDiagram3.AxisX.VisualRange.EndSideMargin = 7D;
            xyDiagram3.AxisX.VisualRange.MaxValueSerializable = "9";
            xyDiagram3.AxisX.VisualRange.MinValueSerializable = "0";
            xyDiagram3.AxisX.VisualRange.StartSideMargin = 0D;
            xyDiagram3.AxisX.WholeRange.AutoSideMargins = false;
            xyDiagram3.AxisX.WholeRange.EndSideMargin = 0D;
            xyDiagram3.AxisX.WholeRange.StartSideMargin = 0D;
            xyDiagram3.AxisY.Label.Font = new System.Drawing.Font("微软雅黑", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            xyDiagram3.AxisY.MinorCount = 4;
            xyDiagram3.AxisY.Title.Text = "传感器实时数据 KG";
            xyDiagram3.AxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
            xyDiagram3.AxisY.VisibleInPanesSerializable = "-1";
            xyDiagram3.AxisY.VisualRange.Auto = false;
            xyDiagram3.AxisY.VisualRange.AutoSideMargins = false;
            xyDiagram3.AxisY.VisualRange.EndSideMargin = 0D;
            xyDiagram3.AxisY.VisualRange.MaxValueSerializable = "20";
            xyDiagram3.AxisY.VisualRange.MinValueSerializable = "0";
            xyDiagram3.AxisY.VisualRange.StartSideMargin = 0D;
            xyDiagram3.AxisY.WholeRange.Auto = false;
            xyDiagram3.AxisY.WholeRange.AutoSideMargins = false;
            xyDiagram3.AxisY.WholeRange.EndSideMargin = 0D;
            xyDiagram3.AxisY.WholeRange.MaxValueSerializable = "20";
            xyDiagram3.AxisY.WholeRange.MinValueSerializable = "0";
            xyDiagram3.AxisY.WholeRange.StartSideMargin = 0D;
            xyDiagram3.EnableAxisXScrolling = true;
            xyDiagram3.EnableAxisXZooming = true;
            xyDiagram3.EnableAxisYScrolling = true;
            xyDiagram3.EnableAxisYZooming = true;
            this.chartControl_weighterSensorRealTimeData.Diagram = xyDiagram3;
            this.chartControl_weighterSensorRealTimeData.Legend.Font = new System.Drawing.Font("微软雅黑", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chartControl_weighterSensorRealTimeData.Legend.Name = "Default Legend";
            this.chartControl_weighterSensorRealTimeData.Location = new System.Drawing.Point(276, 3);
            this.chartControl_weighterSensorRealTimeData.Name = "chartControl_weighterSensorRealTimeData";
            series3.ArgumentScaleType = DevExpress.XtraCharts.ScaleType.Numerical;
            series3.Name = "实时数据";
            splineSeriesView3.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(176)))), ((int)(((byte)(240)))));
            series3.View = splineSeriesView3;
            this.chartControl_weighterSensorRealTimeData.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series3};
            this.chartControl_weighterSensorRealTimeData.Size = new System.Drawing.Size(746, 611);
            this.chartControl_weighterSensorRealTimeData.TabIndex = 4;
            chartTitle3.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartTitle3.Text = "检重传感器实时数据";
            this.chartControl_weighterSensorRealTimeData.Titles.AddRange(new DevExpress.XtraCharts.ChartTitle[] {
            chartTitle3});
            // 
            // labelControl_peakValue
            // 
            this.labelControl_peakValue.Appearance.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelControl_peakValue.Appearance.Options.UseFont = true;
            this.labelControl_peakValue.Appearance.Options.UseTextOptions = true;
            this.labelControl_peakValue.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.labelControl_peakValue.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.labelControl_peakValue.Location = new System.Drawing.Point(28, 10);
            this.labelControl_peakValue.Name = "labelControl_peakValue";
            this.labelControl_peakValue.Size = new System.Drawing.Size(63, 28);
            this.labelControl_peakValue.TabIndex = 8;
            this.labelControl_peakValue.Text = "峰值：";
            // 
            // labelControl_valleyValue
            // 
            this.labelControl_valleyValue.Appearance.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelControl_valleyValue.Appearance.Options.UseFont = true;
            this.labelControl_valleyValue.Appearance.Options.UseTextOptions = true;
            this.labelControl_valleyValue.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.labelControl_valleyValue.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.labelControl_valleyValue.Location = new System.Drawing.Point(28, 50);
            this.labelControl_valleyValue.Name = "labelControl_valleyValue";
            this.labelControl_valleyValue.Size = new System.Drawing.Size(63, 28);
            this.labelControl_valleyValue.TabIndex = 9;
            this.labelControl_valleyValue.Text = "谷值：";
            // 
            // labelControl_averageValue
            // 
            this.labelControl_averageValue.Appearance.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelControl_averageValue.Appearance.Options.UseFont = true;
            this.labelControl_averageValue.Appearance.Options.UseTextOptions = true;
            this.labelControl_averageValue.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.labelControl_averageValue.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.labelControl_averageValue.Location = new System.Drawing.Point(28, 92);
            this.labelControl_averageValue.Name = "labelControl_averageValue";
            this.labelControl_averageValue.Size = new System.Drawing.Size(84, 28);
            this.labelControl_averageValue.TabIndex = 10;
            this.labelControl_averageValue.Text = "平均值：";
            // 
            // labelControl_peakValueVal
            // 
            this.labelControl_peakValueVal.Appearance.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_peakValueVal.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(183)))), ((int)(((byte)(109)))));
            this.labelControl_peakValueVal.Appearance.Options.UseFont = true;
            this.labelControl_peakValueVal.Appearance.Options.UseForeColor = true;
            this.labelControl_peakValueVal.Appearance.Options.UseTextOptions = true;
            this.labelControl_peakValueVal.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.labelControl_peakValueVal.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.labelControl_peakValueVal.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl_peakValueVal.Location = new System.Drawing.Point(109, 10);
            this.labelControl_peakValueVal.Name = "labelControl_peakValueVal";
            this.labelControl_peakValueVal.Size = new System.Drawing.Size(92, 28);
            this.labelControl_peakValueVal.TabIndex = 14;
            // 
            // labelControl_valleyValueVal
            // 
            this.labelControl_valleyValueVal.Appearance.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_valleyValueVal.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(183)))), ((int)(((byte)(109)))));
            this.labelControl_valleyValueVal.Appearance.Options.UseFont = true;
            this.labelControl_valleyValueVal.Appearance.Options.UseForeColor = true;
            this.labelControl_valleyValueVal.Appearance.Options.UseTextOptions = true;
            this.labelControl_valleyValueVal.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.labelControl_valleyValueVal.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.labelControl_valleyValueVal.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl_valleyValueVal.Location = new System.Drawing.Point(109, 50);
            this.labelControl_valleyValueVal.Name = "labelControl_valleyValueVal";
            this.labelControl_valleyValueVal.Size = new System.Drawing.Size(92, 28);
            this.labelControl_valleyValueVal.TabIndex = 15;
            // 
            // labelControl_averageValueVal
            // 
            this.labelControl_averageValueVal.Appearance.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_averageValueVal.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(183)))), ((int)(((byte)(109)))));
            this.labelControl_averageValueVal.Appearance.Options.UseFont = true;
            this.labelControl_averageValueVal.Appearance.Options.UseForeColor = true;
            this.labelControl_averageValueVal.Appearance.Options.UseTextOptions = true;
            this.labelControl_averageValueVal.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.labelControl_averageValueVal.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.labelControl_averageValueVal.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl_averageValueVal.Location = new System.Drawing.Point(109, 92);
            this.labelControl_averageValueVal.Name = "labelControl_averageValueVal";
            this.labelControl_averageValueVal.Size = new System.Drawing.Size(92, 28);
            this.labelControl_averageValueVal.TabIndex = 16;
            // 
            // labelControl_KG1
            // 
            this.labelControl_KG1.Appearance.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_KG1.Appearance.Options.UseFont = true;
            this.labelControl_KG1.Location = new System.Drawing.Point(207, 10);
            this.labelControl_KG1.Name = "labelControl_KG1";
            this.labelControl_KG1.Size = new System.Drawing.Size(29, 28);
            this.labelControl_KG1.TabIndex = 22;
            this.labelControl_KG1.Text = "KG";
            // 
            // labelControl_KG2
            // 
            this.labelControl_KG2.Appearance.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_KG2.Appearance.Options.UseFont = true;
            this.labelControl_KG2.Location = new System.Drawing.Point(207, 50);
            this.labelControl_KG2.Name = "labelControl_KG2";
            this.labelControl_KG2.Size = new System.Drawing.Size(29, 28);
            this.labelControl_KG2.TabIndex = 23;
            this.labelControl_KG2.Text = "KG";
            // 
            // labelControl_KG3
            // 
            this.labelControl_KG3.Appearance.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl_KG3.Appearance.Options.UseFont = true;
            this.labelControl_KG3.Location = new System.Drawing.Point(207, 92);
            this.labelControl_KG3.Name = "labelControl_KG3";
            this.labelControl_KG3.Size = new System.Drawing.Size(29, 28);
            this.labelControl_KG3.TabIndex = 24;
            this.labelControl_KG3.Text = "KG";
            // 
            // panelControl_weightList
            // 
            this.panelControl_weightList.Controls.Add(this.simpleButton_changeMode);
            this.panelControl_weightList.Controls.Add(this.spinEdit_setXMinVal);
            this.panelControl_weightList.Controls.Add(this.labelControl_changeMode);
            this.panelControl_weightList.Controls.Add(this.simpleButton_modifyAxisRange);
            this.panelControl_weightList.Controls.Add(this.labelControl_yWholeRangeZoom);
            this.panelControl_weightList.Controls.Add(this.labelControl_xWholeRangeZoom);
            this.panelControl_weightList.Controls.Add(this.zoomTrackBarControl_yWholeRangeZoom);
            this.panelControl_weightList.Controls.Add(this.spinEdit_setYMaxVal);
            this.panelControl_weightList.Controls.Add(this.spinEdit_setYMinVal);
            this.panelControl_weightList.Controls.Add(this.spinEdit_setXMaxVal);
            this.panelControl_weightList.Controls.Add(this.zoomTrackBarControl_xWholeRangeZoom);
            this.panelControl_weightList.Controls.Add(this.labelControl_yVisualRangeZoom);
            this.panelControl_weightList.Controls.Add(this.labelControl_setVisualRangeZoom);
            this.panelControl_weightList.Controls.Add(this.labelControl_xVisualRangeZoom);
            this.panelControl_weightList.Controls.Add(this.labelControl_setWholeRangeZoom);
            this.panelControl_weightList.Controls.Add(this.zoomTrackBarControl_yVisualRangeZoom);
            this.panelControl_weightList.Controls.Add(this.zoomTrackBarControl_xVisualRangeZoom);
            this.panelControl_weightList.Controls.Add(this.labelControl_setYMax);
            this.panelControl_weightList.Controls.Add(this.labelControl_setYMin);
            this.panelControl_weightList.Controls.Add(this.labelControl_setXMax);
            this.panelControl_weightList.Controls.Add(this.labelControl_setXMin);
            this.panelControl_weightList.Controls.Add(this.separatorControl_left);
            this.panelControl_weightList.Controls.Add(this.labelControl_KG3);
            this.panelControl_weightList.Controls.Add(this.labelControl_KG2);
            this.panelControl_weightList.Controls.Add(this.labelControl_peakValue);
            this.panelControl_weightList.Controls.Add(this.labelControl_KG1);
            this.panelControl_weightList.Controls.Add(this.labelControl_valleyValue);
            this.panelControl_weightList.Controls.Add(this.labelControl_averageValueVal);
            this.panelControl_weightList.Controls.Add(this.labelControl_averageValue);
            this.panelControl_weightList.Controls.Add(this.labelControl_valleyValueVal);
            this.panelControl_weightList.Controls.Add(this.labelControl_peakValueVal);
            this.panelControl_weightList.Location = new System.Drawing.Point(3, 3);
            this.panelControl_weightList.Name = "panelControl_weightList";
            this.panelControl_weightList.Size = new System.Drawing.Size(266, 611);
            this.panelControl_weightList.TabIndex = 25;
            // 
            // simpleButton_changeMode
            // 
            this.simpleButton_changeMode.AllowFocus = false;
            this.simpleButton_changeMode.Appearance.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold);
            this.simpleButton_changeMode.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(152)))), ((int)(((byte)(83)))));
            this.simpleButton_changeMode.Appearance.Options.UseFont = true;
            this.simpleButton_changeMode.Appearance.Options.UseForeColor = true;
            this.simpleButton_changeMode.AppearancePressed.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.simpleButton_changeMode.AppearancePressed.Options.UseFont = true;
            this.simpleButton_changeMode.Location = new System.Drawing.Point(5, 157);
            this.simpleButton_changeMode.LookAndFeel.SkinName = "Seven Classic";
            this.simpleButton_changeMode.LookAndFeel.UseDefaultLookAndFeel = false;
            this.simpleButton_changeMode.Name = "simpleButton_changeMode";
            this.simpleButton_changeMode.Size = new System.Drawing.Size(256, 66);
            this.simpleButton_changeMode.TabIndex = 60;
            this.simpleButton_changeMode.Text = "自动模式";
            this.simpleButton_changeMode.Click += new System.EventHandler(this.simpleButton_changeMode_Click);
            // 
            // spinEdit_setXMinVal
            // 
            this.spinEdit_setXMinVal.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinEdit_setXMinVal.Enabled = false;
            this.spinEdit_setXMinVal.Location = new System.Drawing.Point(5, 250);
            this.spinEdit_setXMinVal.Name = "spinEdit_setXMinVal";
            this.spinEdit_setXMinVal.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.spinEdit_setXMinVal.Properties.Appearance.Options.UseFont = true;
            this.spinEdit_setXMinVal.Properties.Appearance.Options.UseTextOptions = true;
            this.spinEdit_setXMinVal.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.spinEdit_setXMinVal.Properties.AutoHeight = false;
            this.spinEdit_setXMinVal.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinEdit_setXMinVal.Properties.Mask.EditMask = "T";
            this.spinEdit_setXMinVal.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime;
            this.spinEdit_setXMinVal.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.spinEdit_setXMinVal.Size = new System.Drawing.Size(126, 25);
            this.spinEdit_setXMinVal.TabIndex = 37;
            this.spinEdit_setXMinVal.ValueChanged += new System.EventHandler(this.spinEdit_setXMinVal_ValueChanged);
            this.spinEdit_setXMinVal.DoubleClick += new System.EventHandler(this.spinEdit_setXMinVal_DoubleClick);
            // 
            // labelControl_changeMode
            // 
            this.labelControl_changeMode.Appearance.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold);
            this.labelControl_changeMode.Appearance.Options.UseFont = true;
            this.labelControl_changeMode.Appearance.Options.UseTextOptions = true;
            this.labelControl_changeMode.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.labelControl_changeMode.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.labelControl_changeMode.Location = new System.Drawing.Point(5, 132);
            this.labelControl_changeMode.Name = "labelControl_changeMode";
            this.labelControl_changeMode.Size = new System.Drawing.Size(126, 19);
            this.labelControl_changeMode.TabIndex = 59;
            this.labelControl_changeMode.Text = "曲线显示模式切换：";
            // 
            // simpleButton_modifyAxisRange
            // 
            this.simpleButton_modifyAxisRange.AllowFocus = false;
            this.simpleButton_modifyAxisRange.Appearance.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.simpleButton_modifyAxisRange.Appearance.Options.UseFont = true;
            this.simpleButton_modifyAxisRange.AppearancePressed.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.simpleButton_modifyAxisRange.AppearancePressed.Options.UseFont = true;
            this.simpleButton_modifyAxisRange.Enabled = false;
            this.simpleButton_modifyAxisRange.Location = new System.Drawing.Point(5, 335);
            this.simpleButton_modifyAxisRange.LookAndFeel.SkinName = "Seven Classic";
            this.simpleButton_modifyAxisRange.LookAndFeel.UseDefaultLookAndFeel = false;
            this.simpleButton_modifyAxisRange.Name = "simpleButton_modifyAxisRange";
            this.simpleButton_modifyAxisRange.Size = new System.Drawing.Size(256, 66);
            this.simpleButton_modifyAxisRange.TabIndex = 39;
            this.simpleButton_modifyAxisRange.Text = "修改坐标轴范围";
            this.simpleButton_modifyAxisRange.Click += new System.EventHandler(this.simpleButton_modifyAxisRange_Click);
            // 
            // labelControl_yWholeRangeZoom
            // 
            this.labelControl_yWholeRangeZoom.Appearance.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelControl_yWholeRangeZoom.Appearance.ForeColor = DevExpress.LookAndFeel.DXSkinColors.ForeColors.Information;
            this.labelControl_yWholeRangeZoom.Appearance.Options.UseFont = true;
            this.labelControl_yWholeRangeZoom.Appearance.Options.UseForeColor = true;
            this.labelControl_yWholeRangeZoom.Appearance.Options.UseTextOptions = true;
            this.labelControl_yWholeRangeZoom.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.labelControl_yWholeRangeZoom.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.labelControl_yWholeRangeZoom.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl_yWholeRangeZoom.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.labelControl_yWholeRangeZoom.Location = new System.Drawing.Point(74, 431);
            this.labelControl_yWholeRangeZoom.Name = "labelControl_yWholeRangeZoom";
            this.labelControl_yWholeRangeZoom.Size = new System.Drawing.Size(45, 19);
            this.labelControl_yWholeRangeZoom.TabIndex = 57;
            this.labelControl_yWholeRangeZoom.Text = "Y×1";
            // 
            // labelControl_xWholeRangeZoom
            // 
            this.labelControl_xWholeRangeZoom.Appearance.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelControl_xWholeRangeZoom.Appearance.ForeColor = DevExpress.LookAndFeel.DXSkinColors.ForeColors.Information;
            this.labelControl_xWholeRangeZoom.Appearance.Options.UseFont = true;
            this.labelControl_xWholeRangeZoom.Appearance.Options.UseForeColor = true;
            this.labelControl_xWholeRangeZoom.Appearance.Options.UseTextOptions = true;
            this.labelControl_xWholeRangeZoom.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.labelControl_xWholeRangeZoom.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.labelControl_xWholeRangeZoom.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl_xWholeRangeZoom.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.labelControl_xWholeRangeZoom.Location = new System.Drawing.Point(17, 431);
            this.labelControl_xWholeRangeZoom.Name = "labelControl_xWholeRangeZoom";
            this.labelControl_xWholeRangeZoom.Size = new System.Drawing.Size(45, 19);
            this.labelControl_xWholeRangeZoom.TabIndex = 56;
            this.labelControl_xWholeRangeZoom.Text = "X×1";
            // 
            // zoomTrackBarControl_yWholeRangeZoom
            // 
            this.zoomTrackBarControl_yWholeRangeZoom.EditValue = 50;
            this.zoomTrackBarControl_yWholeRangeZoom.Enabled = false;
            this.zoomTrackBarControl_yWholeRangeZoom.Location = new System.Drawing.Point(74, 456);
            this.zoomTrackBarControl_yWholeRangeZoom.Name = "zoomTrackBarControl_yWholeRangeZoom";
            this.zoomTrackBarControl_yWholeRangeZoom.Properties.AutoSize = false;
            this.zoomTrackBarControl_yWholeRangeZoom.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.zoomTrackBarControl_yWholeRangeZoom.Properties.LookAndFeel.SkinName = "Seven Classic";
            this.zoomTrackBarControl_yWholeRangeZoom.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.zoomTrackBarControl_yWholeRangeZoom.Properties.Maximum = 100;
            this.zoomTrackBarControl_yWholeRangeZoom.Properties.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.zoomTrackBarControl_yWholeRangeZoom.Size = new System.Drawing.Size(45, 150);
            this.zoomTrackBarControl_yWholeRangeZoom.TabIndex = 55;
            this.zoomTrackBarControl_yWholeRangeZoom.Value = 50;
            this.zoomTrackBarControl_yWholeRangeZoom.ValueChanged += new System.EventHandler(this.zoomTrackBarControl_yWholeRangeZoom_ValueChanged);
            // 
            // spinEdit_setYMaxVal
            // 
            this.spinEdit_setYMaxVal.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinEdit_setYMaxVal.Enabled = false;
            this.spinEdit_setYMaxVal.Location = new System.Drawing.Point(135, 305);
            this.spinEdit_setYMaxVal.Name = "spinEdit_setYMaxVal";
            this.spinEdit_setYMaxVal.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.spinEdit_setYMaxVal.Properties.Appearance.Options.UseFont = true;
            this.spinEdit_setYMaxVal.Properties.Appearance.Options.UseTextOptions = true;
            this.spinEdit_setYMaxVal.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.spinEdit_setYMaxVal.Properties.AutoHeight = false;
            this.spinEdit_setYMaxVal.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinEdit_setYMaxVal.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.spinEdit_setYMaxVal.Size = new System.Drawing.Size(126, 25);
            this.spinEdit_setYMaxVal.TabIndex = 54;
            this.spinEdit_setYMaxVal.ValueChanged += new System.EventHandler(this.spinEdit_setYMaxVal_ValueChanged);
            this.spinEdit_setYMaxVal.DoubleClick += new System.EventHandler(this.spinEdit_setYMaxVal_DoubleClick);
            // 
            // spinEdit_setYMinVal
            // 
            this.spinEdit_setYMinVal.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinEdit_setYMinVal.Enabled = false;
            this.spinEdit_setYMinVal.Location = new System.Drawing.Point(5, 305);
            this.spinEdit_setYMinVal.Name = "spinEdit_setYMinVal";
            this.spinEdit_setYMinVal.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.spinEdit_setYMinVal.Properties.Appearance.Options.UseFont = true;
            this.spinEdit_setYMinVal.Properties.Appearance.Options.UseTextOptions = true;
            this.spinEdit_setYMinVal.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.spinEdit_setYMinVal.Properties.AutoHeight = false;
            this.spinEdit_setYMinVal.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinEdit_setYMinVal.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.spinEdit_setYMinVal.Size = new System.Drawing.Size(126, 25);
            this.spinEdit_setYMinVal.TabIndex = 53;
            this.spinEdit_setYMinVal.ValueChanged += new System.EventHandler(this.spinEdit_setYMinVal_ValueChanged);
            this.spinEdit_setYMinVal.DoubleClick += new System.EventHandler(this.spinEdit_setYMinVal_DoubleClick);
            // 
            // spinEdit_setXMaxVal
            // 
            this.spinEdit_setXMaxVal.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinEdit_setXMaxVal.Enabled = false;
            this.spinEdit_setXMaxVal.Location = new System.Drawing.Point(135, 250);
            this.spinEdit_setXMaxVal.Name = "spinEdit_setXMaxVal";
            this.spinEdit_setXMaxVal.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.spinEdit_setXMaxVal.Properties.Appearance.Options.UseFont = true;
            this.spinEdit_setXMaxVal.Properties.Appearance.Options.UseTextOptions = true;
            this.spinEdit_setXMaxVal.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.spinEdit_setXMaxVal.Properties.AutoHeight = false;
            this.spinEdit_setXMaxVal.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinEdit_setXMaxVal.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.spinEdit_setXMaxVal.Size = new System.Drawing.Size(126, 25);
            this.spinEdit_setXMaxVal.TabIndex = 49;
            this.spinEdit_setXMaxVal.ValueChanged += new System.EventHandler(this.spinEdit_setXMaxVal_ValueChanged);
            this.spinEdit_setXMaxVal.DoubleClick += new System.EventHandler(this.spinEdit_setXMaxVal_DoubleClick);
            // 
            // zoomTrackBarControl_xWholeRangeZoom
            // 
            this.zoomTrackBarControl_xWholeRangeZoom.EditValue = 50;
            this.zoomTrackBarControl_xWholeRangeZoom.Enabled = false;
            this.zoomTrackBarControl_xWholeRangeZoom.Location = new System.Drawing.Point(17, 456);
            this.zoomTrackBarControl_xWholeRangeZoom.Name = "zoomTrackBarControl_xWholeRangeZoom";
            this.zoomTrackBarControl_xWholeRangeZoom.Properties.AutoSize = false;
            this.zoomTrackBarControl_xWholeRangeZoom.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.zoomTrackBarControl_xWholeRangeZoom.Properties.LookAndFeel.SkinName = "Seven Classic";
            this.zoomTrackBarControl_xWholeRangeZoom.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.zoomTrackBarControl_xWholeRangeZoom.Properties.Maximum = 100;
            this.zoomTrackBarControl_xWholeRangeZoom.Properties.Middle = 50;
            this.zoomTrackBarControl_xWholeRangeZoom.Properties.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.zoomTrackBarControl_xWholeRangeZoom.Size = new System.Drawing.Size(45, 150);
            this.zoomTrackBarControl_xWholeRangeZoom.TabIndex = 46;
            this.zoomTrackBarControl_xWholeRangeZoom.Value = 50;
            this.zoomTrackBarControl_xWholeRangeZoom.ValueChanged += new System.EventHandler(this.zoomTrackBarControl_xWholeRange_ValueChanged);
            // 
            // labelControl_yVisualRangeZoom
            // 
            this.labelControl_yVisualRangeZoom.Appearance.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.labelControl_yVisualRangeZoom.Appearance.ForeColor = DevExpress.LookAndFeel.DXSkinColors.ForeColors.Information;
            this.labelControl_yVisualRangeZoom.Appearance.Options.UseFont = true;
            this.labelControl_yVisualRangeZoom.Appearance.Options.UseForeColor = true;
            this.labelControl_yVisualRangeZoom.Appearance.Options.UseTextOptions = true;
            this.labelControl_yVisualRangeZoom.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.labelControl_yVisualRangeZoom.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.labelControl_yVisualRangeZoom.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl_yVisualRangeZoom.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.labelControl_yVisualRangeZoom.Location = new System.Drawing.Point(204, 431);
            this.labelControl_yVisualRangeZoom.Name = "labelControl_yVisualRangeZoom";
            this.labelControl_yVisualRangeZoom.Size = new System.Drawing.Size(45, 19);
            this.labelControl_yVisualRangeZoom.TabIndex = 43;
            this.labelControl_yVisualRangeZoom.Text = "Y×1";
            // 
            // labelControl_setVisualRangeZoom
            // 
            this.labelControl_setVisualRangeZoom.Appearance.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelControl_setVisualRangeZoom.Appearance.Options.UseFont = true;
            this.labelControl_setVisualRangeZoom.Appearance.Options.UseTextOptions = true;
            this.labelControl_setVisualRangeZoom.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.labelControl_setVisualRangeZoom.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.labelControl_setVisualRangeZoom.Location = new System.Drawing.Point(135, 406);
            this.labelControl_setVisualRangeZoom.Name = "labelControl_setVisualRangeZoom";
            this.labelControl_setVisualRangeZoom.Size = new System.Drawing.Size(98, 19);
            this.labelControl_setVisualRangeZoom.TabIndex = 42;
            this.labelControl_setVisualRangeZoom.Text = "设定缩放比例：";
            // 
            // labelControl_xVisualRangeZoom
            // 
            this.labelControl_xVisualRangeZoom.Appearance.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.labelControl_xVisualRangeZoom.Appearance.ForeColor = DevExpress.LookAndFeel.DXSkinColors.ForeColors.Information;
            this.labelControl_xVisualRangeZoom.Appearance.Options.UseFont = true;
            this.labelControl_xVisualRangeZoom.Appearance.Options.UseForeColor = true;
            this.labelControl_xVisualRangeZoom.Appearance.Options.UseTextOptions = true;
            this.labelControl_xVisualRangeZoom.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.labelControl_xVisualRangeZoom.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.labelControl_xVisualRangeZoom.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl_xVisualRangeZoom.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.labelControl_xVisualRangeZoom.Location = new System.Drawing.Point(147, 431);
            this.labelControl_xVisualRangeZoom.Name = "labelControl_xVisualRangeZoom";
            this.labelControl_xVisualRangeZoom.Size = new System.Drawing.Size(45, 19);
            this.labelControl_xVisualRangeZoom.TabIndex = 41;
            this.labelControl_xVisualRangeZoom.Text = "X×1";
            // 
            // labelControl_setWholeRangeZoom
            // 
            this.labelControl_setWholeRangeZoom.Appearance.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelControl_setWholeRangeZoom.Appearance.Options.UseFont = true;
            this.labelControl_setWholeRangeZoom.Appearance.Options.UseTextOptions = true;
            this.labelControl_setWholeRangeZoom.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.labelControl_setWholeRangeZoom.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.labelControl_setWholeRangeZoom.Location = new System.Drawing.Point(5, 406);
            this.labelControl_setWholeRangeZoom.Name = "labelControl_setWholeRangeZoom";
            this.labelControl_setWholeRangeZoom.Size = new System.Drawing.Size(98, 19);
            this.labelControl_setWholeRangeZoom.TabIndex = 40;
            this.labelControl_setWholeRangeZoom.Text = "设定范围比例：";
            // 
            // zoomTrackBarControl_yVisualRangeZoom
            // 
            this.zoomTrackBarControl_yVisualRangeZoom.EditValue = 1;
            this.zoomTrackBarControl_yVisualRangeZoom.Enabled = false;
            this.zoomTrackBarControl_yVisualRangeZoom.Location = new System.Drawing.Point(204, 456);
            this.zoomTrackBarControl_yVisualRangeZoom.Name = "zoomTrackBarControl_yVisualRangeZoom";
            this.zoomTrackBarControl_yVisualRangeZoom.Properties.AutoSize = false;
            this.zoomTrackBarControl_yVisualRangeZoom.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.zoomTrackBarControl_yVisualRangeZoom.Properties.LookAndFeel.SkinName = "Seven Classic";
            this.zoomTrackBarControl_yVisualRangeZoom.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.zoomTrackBarControl_yVisualRangeZoom.Properties.Maximum = 100;
            this.zoomTrackBarControl_yVisualRangeZoom.Properties.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.zoomTrackBarControl_yVisualRangeZoom.Size = new System.Drawing.Size(45, 150);
            this.zoomTrackBarControl_yVisualRangeZoom.TabIndex = 32;
            this.zoomTrackBarControl_yVisualRangeZoom.Value = 1;
            this.zoomTrackBarControl_yVisualRangeZoom.ValueChanged += new System.EventHandler(this.zoomTrackBarControl_yVisualRange_ValueChanged);
            // 
            // zoomTrackBarControl_xVisualRangeZoom
            // 
            this.zoomTrackBarControl_xVisualRangeZoom.EditValue = 1;
            this.zoomTrackBarControl_xVisualRangeZoom.Enabled = false;
            this.zoomTrackBarControl_xVisualRangeZoom.Location = new System.Drawing.Point(147, 456);
            this.zoomTrackBarControl_xVisualRangeZoom.Name = "zoomTrackBarControl_xVisualRangeZoom";
            this.zoomTrackBarControl_xVisualRangeZoom.Properties.AutoSize = false;
            this.zoomTrackBarControl_xVisualRangeZoom.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.zoomTrackBarControl_xVisualRangeZoom.Properties.LookAndFeel.SkinName = "Seven Classic";
            this.zoomTrackBarControl_xVisualRangeZoom.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.zoomTrackBarControl_xVisualRangeZoom.Properties.Maximum = 100;
            this.zoomTrackBarControl_xVisualRangeZoom.Properties.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.zoomTrackBarControl_xVisualRangeZoom.Size = new System.Drawing.Size(45, 150);
            this.zoomTrackBarControl_xVisualRangeZoom.TabIndex = 33;
            this.zoomTrackBarControl_xVisualRangeZoom.Value = 1;
            this.zoomTrackBarControl_xVisualRangeZoom.ValueChanged += new System.EventHandler(this.zoomTrackBarControl_xVisualRange_ValueChanged);
            // 
            // labelControl_setYMax
            // 
            this.labelControl_setYMax.Appearance.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold);
            this.labelControl_setYMax.Appearance.Options.UseFont = true;
            this.labelControl_setYMax.Appearance.Options.UseTextOptions = true;
            this.labelControl_setYMax.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.labelControl_setYMax.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.labelControl_setYMax.Location = new System.Drawing.Point(135, 280);
            this.labelControl_setYMax.Name = "labelControl_setYMax";
            this.labelControl_setYMax.Size = new System.Drawing.Size(84, 19);
            this.labelControl_setYMax.TabIndex = 36;
            this.labelControl_setYMax.Text = "纵轴最大值：";
            // 
            // labelControl_setYMin
            // 
            this.labelControl_setYMin.Appearance.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold);
            this.labelControl_setYMin.Appearance.Options.UseFont = true;
            this.labelControl_setYMin.Appearance.Options.UseTextOptions = true;
            this.labelControl_setYMin.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.labelControl_setYMin.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.labelControl_setYMin.Location = new System.Drawing.Point(5, 280);
            this.labelControl_setYMin.Name = "labelControl_setYMin";
            this.labelControl_setYMin.Size = new System.Drawing.Size(84, 19);
            this.labelControl_setYMin.TabIndex = 35;
            this.labelControl_setYMin.Text = "纵轴最小值：";
            // 
            // labelControl_setXMax
            // 
            this.labelControl_setXMax.Appearance.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold);
            this.labelControl_setXMax.Appearance.Options.UseFont = true;
            this.labelControl_setXMax.Appearance.Options.UseTextOptions = true;
            this.labelControl_setXMax.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.labelControl_setXMax.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.labelControl_setXMax.Location = new System.Drawing.Point(135, 224);
            this.labelControl_setXMax.Name = "labelControl_setXMax";
            this.labelControl_setXMax.Size = new System.Drawing.Size(84, 19);
            this.labelControl_setXMax.TabIndex = 31;
            this.labelControl_setXMax.Text = "横轴最大值：";
            // 
            // labelControl_setXMin
            // 
            this.labelControl_setXMin.Appearance.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelControl_setXMin.Appearance.Options.UseFont = true;
            this.labelControl_setXMin.Appearance.Options.UseTextOptions = true;
            this.labelControl_setXMin.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.labelControl_setXMin.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.labelControl_setXMin.Location = new System.Drawing.Point(5, 224);
            this.labelControl_setXMin.Name = "labelControl_setXMin";
            this.labelControl_setXMin.Size = new System.Drawing.Size(84, 19);
            this.labelControl_setXMin.TabIndex = 30;
            this.labelControl_setXMin.Text = "横轴最小值：";
            // 
            // separatorControl_left
            // 
            this.separatorControl_left.LineAlignment = DevExpress.XtraEditors.Alignment.Center;
            this.separatorControl_left.Location = new System.Drawing.Point(1, 118);
            this.separatorControl_left.LookAndFeel.SkinName = "Office 2019 Colorful";
            this.separatorControl_left.LookAndFeel.UseDefaultLookAndFeel = false;
            this.separatorControl_left.Name = "separatorControl_left";
            this.separatorControl_left.Size = new System.Drawing.Size(263, 21);
            this.separatorControl_left.TabIndex = 29;
            // 
            // timer_getDataOnceFromSensor
            // 
            this.timer_getDataOnceFromSensor.Enabled = true;
            this.timer_getDataOnceFromSensor.Tick += new System.EventHandler(this.timer_getDataOnceFromSensor_Tick);
            // 
            // RealTimeCurve
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelControl_weightList);
            this.Controls.Add(this.chartControl_weighterSensorRealTimeData);
            this.Name = "RealTimeCurve";
            this.Size = new System.Drawing.Size(1024, 617);
            ((System.ComponentModel.ISupportInitialize)(xyDiagram3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(splineSeriesView3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartControl_weighterSensorRealTimeData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl_weightList)).EndInit();
            this.panelControl_weightList.ResumeLayout(false);
            this.panelControl_weightList.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit_setXMinVal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.zoomTrackBarControl_yWholeRangeZoom.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.zoomTrackBarControl_yWholeRangeZoom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit_setYMaxVal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit_setYMinVal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit_setXMaxVal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.zoomTrackBarControl_xWholeRangeZoom.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.zoomTrackBarControl_xWholeRangeZoom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.zoomTrackBarControl_yVisualRangeZoom.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.zoomTrackBarControl_yVisualRangeZoom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.zoomTrackBarControl_xVisualRangeZoom.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.zoomTrackBarControl_xVisualRangeZoom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.separatorControl_left)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraCharts.ChartControl chartControl_weighterSensorRealTimeData;
        private DevExpress.XtraEditors.LabelControl labelControl_peakValue;
        private DevExpress.XtraEditors.LabelControl labelControl_valleyValue;
        private DevExpress.XtraEditors.LabelControl labelControl_averageValue;
        private DevExpress.XtraEditors.LabelControl labelControl_peakValueVal;
        private DevExpress.XtraEditors.LabelControl labelControl_valleyValueVal;
        private DevExpress.XtraEditors.LabelControl labelControl_averageValueVal;
        private DevExpress.XtraEditors.LabelControl labelControl_KG1;
        private DevExpress.XtraEditors.LabelControl labelControl_KG2;
        private DevExpress.XtraEditors.LabelControl labelControl_KG3;
        private DevExpress.XtraEditors.PanelControl panelControl_weightList;
        private DevExpress.XtraEditors.SeparatorControl separatorControl_left;
        private DevExpress.XtraEditors.LabelControl labelControl_setXMax;
        private DevExpress.XtraEditors.LabelControl labelControl_setXMin;
        private DevExpress.XtraEditors.LabelControl labelControl_setYMax;
        private DevExpress.XtraEditors.LabelControl labelControl_setYMin;
        private System.Windows.Forms.Timer timer_getDataOnceFromSensor;
        private DevExpress.XtraEditors.SimpleButton simpleButton_modifyAxisRange;
        private DevExpress.XtraEditors.SpinEdit spinEdit_setXMaxVal;
        private DevExpress.XtraEditors.ZoomTrackBarControl zoomTrackBarControl_xWholeRangeZoom;
        private DevExpress.XtraEditors.SpinEdit spinEdit_setYMaxVal;
        private DevExpress.XtraEditors.SpinEdit spinEdit_setYMinVal;
        private DevExpress.XtraEditors.ZoomTrackBarControl zoomTrackBarControl_yWholeRangeZoom;
        private DevExpress.XtraEditors.LabelControl labelControl_xWholeRangeZoom;
        private DevExpress.XtraEditors.LabelControl labelControl_yWholeRangeZoom;
        private DevExpress.XtraEditors.LabelControl labelControl_yVisualRangeZoom;
        private DevExpress.XtraEditors.LabelControl labelControl_setVisualRangeZoom;
        private DevExpress.XtraEditors.LabelControl labelControl_xVisualRangeZoom;
        private DevExpress.XtraEditors.LabelControl labelControl_setWholeRangeZoom;
        private DevExpress.XtraEditors.ZoomTrackBarControl zoomTrackBarControl_yVisualRangeZoom;
        private DevExpress.XtraEditors.ZoomTrackBarControl zoomTrackBarControl_xVisualRangeZoom;
        private DevExpress.XtraEditors.LabelControl labelControl_changeMode;
        private DevExpress.XtraEditors.SpinEdit spinEdit_setXMinVal;
        private DevExpress.XtraEditors.SimpleButton simpleButton_changeMode;
    }
}
