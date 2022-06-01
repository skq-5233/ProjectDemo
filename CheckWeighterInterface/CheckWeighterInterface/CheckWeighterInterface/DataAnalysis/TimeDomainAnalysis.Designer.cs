﻿
namespace CheckWeighterInterface.DataAnalysis
{
    partial class TimeDomainAnalysis
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
            DevExpress.XtraCharts.XYDiagram xyDiagram1 = new DevExpress.XtraCharts.XYDiagram();
            DevExpress.XtraCharts.Series series1 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.LineSeriesView lineSeriesView1 = new DevExpress.XtraCharts.LineSeriesView();
            DevExpress.XtraCharts.ChartTitle chartTitle1 = new DevExpress.XtraCharts.ChartTitle();
            this.chartControl_line = new DevExpress.XtraCharts.ChartControl();
            this.panelControl_weightList = new DevExpress.XtraEditors.PanelControl();
            this.separatorControl1 = new DevExpress.XtraEditors.SeparatorControl();
            this.simpleButton_query6Months = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton_query3Months = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton_query1Month = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton_query1Week = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton_query = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton_endTimeModify = new DevExpress.XtraEditors.SimpleButton();
            this.timeEdit_endTime = new DevExpress.XtraEditors.TimeEdit();
            this.labelControl_endTime = new DevExpress.XtraEditors.LabelControl();
            this.simpleButton_startTimeModify = new DevExpress.XtraEditors.SimpleButton();
            this.timeEdit_startTime = new DevExpress.XtraEditors.TimeEdit();
            this.labelControl_startTime = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.chartControl_line)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl_weightList)).BeginInit();
            this.panelControl_weightList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.separatorControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeEdit_endTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeEdit_startTime.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // chartControl_line
            // 
            xyDiagram1.AxisX.GridLines.Visible = true;
            xyDiagram1.AxisX.Interlaced = true;
            xyDiagram1.AxisX.Label.Font = new System.Drawing.Font("微软雅黑", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            xyDiagram1.AxisX.Title.Text = "称重数量";
            xyDiagram1.AxisX.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
            xyDiagram1.AxisX.VisibleInPanesSerializable = "-1";
            xyDiagram1.AxisX.WholeRange.AutoSideMargins = false;
            xyDiagram1.AxisX.WholeRange.EndSideMargin = 0.1D;
            xyDiagram1.AxisX.WholeRange.StartSideMargin = 0.1D;
            xyDiagram1.AxisY.Label.Font = new System.Drawing.Font("微软雅黑", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            xyDiagram1.AxisY.Title.Text = "当前重量 KG";
            xyDiagram1.AxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
            xyDiagram1.AxisY.VisibleInPanesSerializable = "-1";
            xyDiagram1.AxisY.WholeRange.Auto = false;
            xyDiagram1.AxisY.WholeRange.AutoSideMargins = false;
            xyDiagram1.AxisY.WholeRange.EndSideMargin = 0D;
            xyDiagram1.AxisY.WholeRange.MaxValueSerializable = "20";
            xyDiagram1.AxisY.WholeRange.MinValueSerializable = "0";
            xyDiagram1.AxisY.WholeRange.StartSideMargin = 0D;
            xyDiagram1.EnableAxisXScrolling = true;
            xyDiagram1.EnableAxisXZooming = true;
            xyDiagram1.EnableAxisYScrolling = true;
            xyDiagram1.EnableAxisYZooming = true;
            this.chartControl_line.Diagram = xyDiagram1;
            this.chartControl_line.Legend.Font = new System.Drawing.Font("微软雅黑", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chartControl_line.Legend.Name = "Default Legend";
            this.chartControl_line.Location = new System.Drawing.Point(307, 3);
            this.chartControl_line.Name = "chartControl_line";
            series1.Name = "当前重量";
            lineSeriesView1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(176)))), ((int)(((byte)(240)))));
            lineSeriesView1.LineMarkerOptions.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(112)))), ((int)(((byte)(192)))));
            lineSeriesView1.LineMarkerOptions.Size = 5;
            lineSeriesView1.MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
            series1.View = lineSeriesView1;
            this.chartControl_line.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series1};
            this.chartControl_line.Size = new System.Drawing.Size(714, 611);
            this.chartControl_line.TabIndex = 3;
            chartTitle1.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartTitle1.Text = "重量变化曲线图";
            this.chartControl_line.Titles.AddRange(new DevExpress.XtraCharts.ChartTitle[] {
            chartTitle1});
            // 
            // panelControl_weightList
            // 
            this.panelControl_weightList.Controls.Add(this.separatorControl1);
            this.panelControl_weightList.Controls.Add(this.simpleButton_query6Months);
            this.panelControl_weightList.Controls.Add(this.simpleButton_query3Months);
            this.panelControl_weightList.Controls.Add(this.simpleButton_query1Month);
            this.panelControl_weightList.Controls.Add(this.simpleButton_query1Week);
            this.panelControl_weightList.Controls.Add(this.simpleButton_query);
            this.panelControl_weightList.Controls.Add(this.simpleButton_endTimeModify);
            this.panelControl_weightList.Controls.Add(this.timeEdit_endTime);
            this.panelControl_weightList.Controls.Add(this.labelControl_endTime);
            this.panelControl_weightList.Controls.Add(this.simpleButton_startTimeModify);
            this.panelControl_weightList.Controls.Add(this.timeEdit_startTime);
            this.panelControl_weightList.Controls.Add(this.labelControl_startTime);
            this.panelControl_weightList.Location = new System.Drawing.Point(3, 3);
            this.panelControl_weightList.Name = "panelControl_weightList";
            this.panelControl_weightList.Size = new System.Drawing.Size(298, 611);
            this.panelControl_weightList.TabIndex = 14;
            // 
            // separatorControl1
            // 
            this.separatorControl1.LineAlignment = DevExpress.XtraEditors.Alignment.Center;
            this.separatorControl1.Location = new System.Drawing.Point(1, 272);
            this.separatorControl1.LookAndFeel.SkinName = "Office 2019 Colorful";
            this.separatorControl1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.separatorControl1.Name = "separatorControl1";
            this.separatorControl1.Size = new System.Drawing.Size(296, 21);
            this.separatorControl1.TabIndex = 29;
            // 
            // simpleButton_query6Months
            // 
            this.simpleButton_query6Months.AllowFocus = false;
            this.simpleButton_query6Months.Appearance.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.simpleButton_query6Months.Appearance.Options.UseFont = true;
            this.simpleButton_query6Months.AppearancePressed.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.simpleButton_query6Months.AppearancePressed.Options.UseFont = true;
            this.simpleButton_query6Months.Location = new System.Drawing.Point(151, 234);
            this.simpleButton_query6Months.LookAndFeel.SkinName = "Seven Classic";
            this.simpleButton_query6Months.LookAndFeel.UseDefaultLookAndFeel = false;
            this.simpleButton_query6Months.Name = "simpleButton_query6Months";
            this.simpleButton_query6Months.Size = new System.Drawing.Size(144, 32);
            this.simpleButton_query6Months.TabIndex = 28;
            this.simpleButton_query6Months.Text = "查询近6月";
            this.simpleButton_query6Months.Click += new System.EventHandler(this.simpleButton_query6Months_Click);
            // 
            // simpleButton_query3Months
            // 
            this.simpleButton_query3Months.AllowFocus = false;
            this.simpleButton_query3Months.Appearance.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.simpleButton_query3Months.Appearance.Options.UseFont = true;
            this.simpleButton_query3Months.AppearancePressed.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.simpleButton_query3Months.AppearancePressed.Options.UseFont = true;
            this.simpleButton_query3Months.Location = new System.Drawing.Point(3, 234);
            this.simpleButton_query3Months.LookAndFeel.SkinName = "Seven Classic";
            this.simpleButton_query3Months.LookAndFeel.UseDefaultLookAndFeel = false;
            this.simpleButton_query3Months.Name = "simpleButton_query3Months";
            this.simpleButton_query3Months.Size = new System.Drawing.Size(144, 32);
            this.simpleButton_query3Months.TabIndex = 27;
            this.simpleButton_query3Months.Text = "查询近3月";
            this.simpleButton_query3Months.Click += new System.EventHandler(this.simpleButton_query3Months_Click);
            // 
            // simpleButton_query1Month
            // 
            this.simpleButton_query1Month.AllowFocus = false;
            this.simpleButton_query1Month.Appearance.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.simpleButton_query1Month.Appearance.Options.UseFont = true;
            this.simpleButton_query1Month.AppearancePressed.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.simpleButton_query1Month.AppearancePressed.Options.UseFont = true;
            this.simpleButton_query1Month.Location = new System.Drawing.Point(151, 199);
            this.simpleButton_query1Month.LookAndFeel.SkinName = "Seven Classic";
            this.simpleButton_query1Month.LookAndFeel.UseDefaultLookAndFeel = false;
            this.simpleButton_query1Month.Name = "simpleButton_query1Month";
            this.simpleButton_query1Month.Size = new System.Drawing.Size(144, 32);
            this.simpleButton_query1Month.TabIndex = 26;
            this.simpleButton_query1Month.Text = "查询近1月";
            this.simpleButton_query1Month.Click += new System.EventHandler(this.simpleButton_query1Month_Click);
            // 
            // simpleButton_query1Week
            // 
            this.simpleButton_query1Week.AllowFocus = false;
            this.simpleButton_query1Week.Appearance.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.simpleButton_query1Week.Appearance.Options.UseFont = true;
            this.simpleButton_query1Week.AppearancePressed.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.simpleButton_query1Week.AppearancePressed.Options.UseFont = true;
            this.simpleButton_query1Week.Location = new System.Drawing.Point(3, 199);
            this.simpleButton_query1Week.LookAndFeel.SkinName = "Seven Classic";
            this.simpleButton_query1Week.LookAndFeel.UseDefaultLookAndFeel = false;
            this.simpleButton_query1Week.Name = "simpleButton_query1Week";
            this.simpleButton_query1Week.Size = new System.Drawing.Size(144, 32);
            this.simpleButton_query1Week.TabIndex = 25;
            this.simpleButton_query1Week.Text = "查询近1周";
            this.simpleButton_query1Week.Click += new System.EventHandler(this.simpleButton_query1Week_Click);
            // 
            // simpleButton_query
            // 
            this.simpleButton_query.AllowFocus = false;
            this.simpleButton_query.Appearance.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.simpleButton_query.Appearance.Options.UseFont = true;
            this.simpleButton_query.AppearancePressed.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.simpleButton_query.AppearancePressed.Options.UseFont = true;
            this.simpleButton_query.Location = new System.Drawing.Point(3, 136);
            this.simpleButton_query.LookAndFeel.SkinName = "Seven Classic";
            this.simpleButton_query.LookAndFeel.UseDefaultLookAndFeel = false;
            this.simpleButton_query.Name = "simpleButton_query";
            this.simpleButton_query.Size = new System.Drawing.Size(292, 60);
            this.simpleButton_query.TabIndex = 24;
            this.simpleButton_query.Text = "查询";
            this.simpleButton_query.Click += new System.EventHandler(this.simpleButton_query_Click);
            // 
            // simpleButton_endTimeModify
            // 
            this.simpleButton_endTimeModify.AllowFocus = false;
            this.simpleButton_endTimeModify.Appearance.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.simpleButton_endTimeModify.Appearance.Options.UseFont = true;
            this.simpleButton_endTimeModify.AppearancePressed.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.simpleButton_endTimeModify.AppearancePressed.Options.UseFont = true;
            this.simpleButton_endTimeModify.Location = new System.Drawing.Point(210, 101);
            this.simpleButton_endTimeModify.LookAndFeel.SkinName = "Seven Classic";
            this.simpleButton_endTimeModify.LookAndFeel.UseDefaultLookAndFeel = false;
            this.simpleButton_endTimeModify.Name = "simpleButton_endTimeModify";
            this.simpleButton_endTimeModify.Size = new System.Drawing.Size(85, 32);
            this.simpleButton_endTimeModify.TabIndex = 23;
            this.simpleButton_endTimeModify.TabStop = false;
            this.simpleButton_endTimeModify.Text = "更改";
            this.simpleButton_endTimeModify.Click += new System.EventHandler(this.simpleButton_endTimeModify_Click);
            // 
            // timeEdit_endTime
            // 
            this.timeEdit_endTime.EditValue = "2021/10/8 0:00:00";
            this.timeEdit_endTime.Location = new System.Drawing.Point(3, 101);
            this.timeEdit_endTime.Name = "timeEdit_endTime";
            this.timeEdit_endTime.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.timeEdit_endTime.Properties.Appearance.Options.UseFont = true;
            this.timeEdit_endTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.timeEdit_endTime.Properties.Mask.EditMask = "G";
            this.timeEdit_endTime.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.timeEdit_endTime.Properties.TimeEditStyle = DevExpress.XtraEditors.Repository.TimeEditStyle.TouchUI;
            this.timeEdit_endTime.Size = new System.Drawing.Size(204, 32);
            this.timeEdit_endTime.TabIndex = 22;
            this.timeEdit_endTime.EditValueChanged += new System.EventHandler(this.timeEdit_endTime_EditValueChanged);
            // 
            // labelControl_endTime
            // 
            this.labelControl_endTime.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.labelControl_endTime.Appearance.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelControl_endTime.Appearance.ForeColor = System.Drawing.Color.White;
            this.labelControl_endTime.Appearance.Options.UseBackColor = true;
            this.labelControl_endTime.Appearance.Options.UseFont = true;
            this.labelControl_endTime.Appearance.Options.UseForeColor = true;
            this.labelControl_endTime.Appearance.Options.UseTextOptions = true;
            this.labelControl_endTime.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.labelControl_endTime.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl_endTime.Location = new System.Drawing.Point(0, 68);
            this.labelControl_endTime.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
            this.labelControl_endTime.LookAndFeel.UseDefaultLookAndFeel = false;
            this.labelControl_endTime.Name = "labelControl_endTime";
            this.labelControl_endTime.Size = new System.Drawing.Size(301, 30);
            this.labelControl_endTime.TabIndex = 21;
            this.labelControl_endTime.Text = "终止时间";
            // 
            // simpleButton_startTimeModify
            // 
            this.simpleButton_startTimeModify.AllowFocus = false;
            this.simpleButton_startTimeModify.Appearance.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.simpleButton_startTimeModify.Appearance.Options.UseFont = true;
            this.simpleButton_startTimeModify.AppearancePressed.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.simpleButton_startTimeModify.AppearancePressed.Options.UseFont = true;
            this.simpleButton_startTimeModify.Location = new System.Drawing.Point(210, 33);
            this.simpleButton_startTimeModify.LookAndFeel.SkinName = "Seven Classic";
            this.simpleButton_startTimeModify.LookAndFeel.UseDefaultLookAndFeel = false;
            this.simpleButton_startTimeModify.Name = "simpleButton_startTimeModify";
            this.simpleButton_startTimeModify.Size = new System.Drawing.Size(85, 32);
            this.simpleButton_startTimeModify.TabIndex = 20;
            this.simpleButton_startTimeModify.TabStop = false;
            this.simpleButton_startTimeModify.Text = "更改";
            this.simpleButton_startTimeModify.Click += new System.EventHandler(this.simpleButton_startTimeModify_Click);
            // 
            // timeEdit_startTime
            // 
            this.timeEdit_startTime.EditValue = "2021/10/8 0:00:00";
            this.timeEdit_startTime.Location = new System.Drawing.Point(3, 33);
            this.timeEdit_startTime.Name = "timeEdit_startTime";
            this.timeEdit_startTime.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.timeEdit_startTime.Properties.Appearance.Options.UseFont = true;
            this.timeEdit_startTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.timeEdit_startTime.Properties.Mask.EditMask = "G";
            this.timeEdit_startTime.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.timeEdit_startTime.Properties.TimeEditStyle = DevExpress.XtraEditors.Repository.TimeEditStyle.TouchUI;
            this.timeEdit_startTime.Size = new System.Drawing.Size(204, 32);
            this.timeEdit_startTime.TabIndex = 15;
            this.timeEdit_startTime.EditValueChanged += new System.EventHandler(this.timeEdit_startTime_EditValueChanged);
            // 
            // labelControl_startTime
            // 
            this.labelControl_startTime.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.labelControl_startTime.Appearance.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelControl_startTime.Appearance.ForeColor = System.Drawing.Color.White;
            this.labelControl_startTime.Appearance.Options.UseBackColor = true;
            this.labelControl_startTime.Appearance.Options.UseFont = true;
            this.labelControl_startTime.Appearance.Options.UseForeColor = true;
            this.labelControl_startTime.Appearance.Options.UseTextOptions = true;
            this.labelControl_startTime.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.labelControl_startTime.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl_startTime.Location = new System.Drawing.Point(0, 0);
            this.labelControl_startTime.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
            this.labelControl_startTime.LookAndFeel.UseDefaultLookAndFeel = false;
            this.labelControl_startTime.Name = "labelControl_startTime";
            this.labelControl_startTime.Size = new System.Drawing.Size(298, 30);
            this.labelControl_startTime.TabIndex = 14;
            this.labelControl_startTime.Text = "起始时间";
            // 
            // TimeDomainAnalysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelControl_weightList);
            this.Controls.Add(this.chartControl_line);
            this.Name = "TimeDomainAnalysis";
            this.Size = new System.Drawing.Size(1024, 617);
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartControl_line)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl_weightList)).EndInit();
            this.panelControl_weightList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.separatorControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeEdit_endTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeEdit_startTime.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraCharts.ChartControl chartControl_line;
        private DevExpress.XtraEditors.PanelControl panelControl_weightList;
        private DevExpress.XtraEditors.LabelControl labelControl_startTime;
        private DevExpress.XtraEditors.TimeEdit timeEdit_startTime;
        private DevExpress.XtraEditors.SimpleButton simpleButton_startTimeModify;
        private DevExpress.XtraEditors.LabelControl labelControl_endTime;
        private DevExpress.XtraEditors.SimpleButton simpleButton_endTimeModify;
        private DevExpress.XtraEditors.TimeEdit timeEdit_endTime;
        private DevExpress.XtraEditors.SimpleButton simpleButton_query;
        private DevExpress.XtraEditors.SimpleButton simpleButton_query1Month;
        private DevExpress.XtraEditors.SimpleButton simpleButton_query1Week;
        private DevExpress.XtraEditors.SimpleButton simpleButton_query6Months;
        private DevExpress.XtraEditors.SimpleButton simpleButton_query3Months;
        private DevExpress.XtraEditors.SeparatorControl separatorControl1;
    }
}
