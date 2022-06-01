using DevExpress.XtraCharts;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckWeighterInterface.DataAnalysis
{
    public partial class TimeDomainAnalysis : DevExpress.XtraEditors.XtraUserControl
    {
        private CommonControl.ConfirmationBox confirmationBox_invalidTime;

        public static DataTable dtStatusHistory = new DataTable("tableLineHistory");        //历史-原始数据表
        string startTime = String.Empty;
        string endTime = String.Empty;


        public TimeDomainAnalysis()
        {
            InitializeComponent();
            initTimeDomainAnalysis();
            SystemManagement.BrandManagement.brandChangedReInitTimeDomainAnalysis += reInitTimeDomainAnalysis;
        }

        private void reInitTimeDomainAnalysis(object sender, EventArgs e)
        {
            dtStatusHistory.Rows.Clear();
            initTimeDomainAnalysis();
            //MessageBox.Show("refresh");

        }

        private void initTimeDomainAnalysis()
        {
            initTimeEditStartAndEnd();
            initDataTable();
            bindLineData();
        }

        private void initDataTable()
        {
            if (dtStatusHistory.Columns.Count == 0)
            {
                dtStatusHistory.Columns.Add("indexDetection", typeof(Int32));     
                dtStatusHistory.Columns.Add("brand", typeof(String));     
                dtStatusHistory.Columns.Add("historyWeight", typeof(double));     
                dtStatusHistory.Columns.Add("status", typeof(String));
                dtStatusHistory.Columns.Add("DateTime", typeof(DateTime));     
            }
        }

        private void bindLineData()
        {
            //this.chartControl_line.Series[0].DataSource = StatusMonitor.StatusMonitor.dtLine;     
            this.chartControl_line.Series[0].DataSource = dtStatusHistory;
            this.chartControl_line.Series[0].ArgumentScaleType = ScaleType.Numerical;   
            this.chartControl_line.Series[0].ArgumentDataMember = "indexDetection";       
            this.chartControl_line.Series[0].ValueScaleType = ScaleType.Numerical;  
            this.chartControl_line.Series[0].ValueDataMembers.AddRange(new string[] { "historyWeight" });
        }

        private void initTimeEditStartAndEnd()
        {
            DateTime nowdt = DateTime.Now;
            DateTime oneMonthAgo = DateTime.Now.AddMonths(-1);  //当前日期的一个月前日期
            this.timeEdit_startTime.Time = oneMonthAgo;
            this.timeEdit_endTime.Time = nowdt;
            startTime = timeEdit_startTime.Time.ToString("yyyy-MM-dd HH:mm:ss");
            endTime = this.timeEdit_endTime.Time.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private bool _timeInterValIllegal()
        {
            if (this.timeEdit_endTime.Time <= this.timeEdit_startTime.Time)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void showCloseConfirmBox(string title, string typeConfirmationBox)
        {
            if (this.confirmationBox_invalidTime != null)
                this.confirmationBox_invalidTime.Dispose();

            this.confirmationBox_invalidTime = new CommonControl.ConfirmationBox();
            this.confirmationBox_invalidTime.Appearance.BackColor = System.Drawing.Color.White;
            this.confirmationBox_invalidTime.Appearance.Options.UseBackColor = true;
            this.confirmationBox_invalidTime.Name = "confirmationBox_invalidTime";
            this.confirmationBox_invalidTime.Size = new System.Drawing.Size(350, 150);
            this.confirmationBox_invalidTime.Location = new Point(337, 100);
            this.confirmationBox_invalidTime.TabIndex = 29;
            this.confirmationBox_invalidTime.titleConfirmationBox = title;
            this.confirmationBox_invalidTime.ConfirmationBoxOKClicked += new CommonControl.ConfirmationBox.SimpleButtonOKClickHanlder(this.confirmationBox_applicationRestart_closeOK);
            this.confirmationBox_invalidTime.ConfirmationBoxCancelClicked += new CommonControl.ConfirmationBox.SimpleButtonCancelClickHanlder(this.confirmationBox_applicationRestart_closeCancel);
            this.Controls.Add(this.confirmationBox_invalidTime);
            this.confirmationBox_invalidTime.Visible = true;
            this.confirmationBox_invalidTime.BringToFront();
        }

        private void confirmationBox_applicationRestart_closeOK(object sender, EventArgs e)
        {
        }

        private void confirmationBox_applicationRestart_closeCancel(object sender, EventArgs e)
        {
        }

        private void simpleButton_endTimeModify_Click(object sender, EventArgs e)
        {
            this.timeEdit_endTime.ShowPopup();
        }

        private void simpleButton_startTimeModify_Click(object sender, EventArgs e)
        {
            this.timeEdit_startTime.ShowPopup();
        }

        private void timeEdit_startTime_EditValueChanged(object sender, EventArgs e)
        {
            startTime = timeEdit_startTime.Time.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void timeEdit_endTime_EditValueChanged(object sender, EventArgs e)
        {
            endTime = this.timeEdit_endTime.Time.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void simpleButton_query1Week_Click(object sender, EventArgs e)
        {
            startTime = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd HH:mm:ss");
            endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Global.queryStatusHistoryMySQL(startTime, endTime, ref dtStatusHistory);
        }

        private void simpleButton_query1Month_Click(object sender, EventArgs e)
        {
            startTime = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd HH:mm:ss");
            endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Global.queryStatusHistoryMySQL(startTime, endTime, ref dtStatusHistory);
        }

        private void simpleButton_query3Months_Click(object sender, EventArgs e)
        {
            startTime = DateTime.Now.AddMonths(-3).ToString("yyyy-MM-dd HH:mm:ss");
            endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Global.queryStatusHistoryMySQL(startTime, endTime, ref dtStatusHistory);
        }

        private void simpleButton_query6Months_Click(object sender, EventArgs e)
        {
            startTime = DateTime.Now.AddMonths(-6).ToString("yyyy-MM-dd HH:mm:ss");
            endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Global.queryStatusHistoryMySQL(startTime, endTime, ref dtStatusHistory);
        }

        private void simpleButton_query_Click(object sender, EventArgs e)
        {
            if (_timeInterValIllegal() == true)
            {
                showCloseConfirmBox("时间区间选择错误！！请重新选择...", "info");
            }
            else
            {
                startTime = this.timeEdit_startTime.Time.ToString("yyyy-MM-dd HH:mm:ss");
                endTime = this.timeEdit_endTime.Time.ToString("yyyy-MM-dd HH:mm:ss");
                //查询改变grid绑定的表
                Global.queryStatusHistoryMySQL(startTime, endTime, ref dtStatusHistory);   
                
            }
        }



    }
}
