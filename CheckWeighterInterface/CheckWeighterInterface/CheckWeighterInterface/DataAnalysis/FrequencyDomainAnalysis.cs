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
    public partial class FrequencyDomainAnalysis : DevExpress.XtraEditors.XtraUserControl
    {
        private CommonControl.ConfirmationBox confirmationBox_invalidTime;
        
        string startTime = String.Empty;
        string endTime = String.Empty;

        public static DataTable dtStatusHistory = new DataTable("tableLineHistory");        //历史-原始数据表
        public static DataTable dtPieHistory = new DataTable("tablePieHistory");       
        public static DataTable dtPointHistory = new DataTable("tablePointHistory");

        //每次查询时对原始数据进行处理后记录相关数据
        private int countUnderWeight = 0;   
        private int countOverWeight = 0;
        private int countDetectionHistory = 0;
        
        Dictionary<int, int> weightAndCountGramDtPoint = new Dictionary<int, int>();    //重量,该重量出现个数
        
        //散点图横轴坐标边缘
        private Int32[] minXRangePoint = { 0, 1000, 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000, 10000, 11000, 12000, 13000, 14000, 15000 };
        private Int32[] maxXRangePoint = { 0, 1000, 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000, 10000, 11000, 12000, 13000, 14000, 15000 };
        private Int32 minXPoint = 0;    //横轴范围左侧
        private Int32 maxXPoint = 0;   //横轴范围右侧

        //散点图横轴分辨率档位
        private Int32[] intervalAxisXPoint = { 10, 20, 50, 100 };     //散点图横轴分辨率
        private Int32 gearIntervalAxisXPoint = 0;                    //散点图横轴档位

        public FrequencyDomainAnalysis()
        {
            InitializeComponent();
            initFrequencyDomainAnalysis();
            SystemManagement.BrandManagement.brandChangedReInitFrequencyDomainAnalysis += reInitFrequencyDomainAnalysis;

        }

        private void reInitFrequencyDomainAnalysis(object sender, EventArgs e)
        {
            dtStatusHistory.Rows.Clear();
            dtPieHistory.Rows.Clear();
            dtPointHistory.Rows.Clear();

            initFrequencyDomainAnalysis();
            //MessageBox.Show("refresh");

        }

        private void initFrequencyDomainAnalysis()
        {
            initTimeEditStartAndEnd();
            initDataTable();
            bindPieData();

            setAxisXMinMaxPoint(8, 12);  //设定散点图横轴区间范围
            setGearIntervalAxisXPoint(0);   //设定散点图横轴分辨率
            bindPointData();
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

            if (dtPieHistory.Columns.Count == 0)
            {
                dtPieHistory.Columns.Add("status", typeof(String));                
                dtPieHistory.Columns.Add("countHistory", typeof(Int32));
            }

            if (dtPointHistory.Columns.Count == 0)
            {
                dtPointHistory.Columns.Add("weightSection", typeof(Int32));
                dtPointHistory.Columns.Add("countInSection", typeof(Int32));       
            }

        }

        private void bindPieData()
        {
            this.chartControl_pie.Series[0].DataSource = dtPieHistory;
            this.chartControl_pie.Series[0].ArgumentDataMember = "status";
            this.chartControl_pie.Series[0].ArgumentScaleType = DevExpress.XtraCharts.ScaleType.Auto;
            this.chartControl_pie.Series[0].ValueDataMembers.AddRange(new string[] { "countHistory" });
            this.chartControl_pie.Series[0].ValueScaleType = ScaleType.Numerical;
            this.chartControl_pie.Series[0].LegendTextPattern = "{A}：{VP:p2}";  //图例格式"Argument:Value"。V:n2为Value:numeric小数点后2位，VP:p2为Value:percent小数点后2位
        }

        //绑定点图数据源，以及图表相关设置
        private void bindPointData()
        {
            this.chartControl_point.Series[0].DataSource = dtPointHistory;
            this.chartControl_point.Series[0].ArgumentScaleType = ScaleType.Auto;
            this.chartControl_point.Series[0].ArgumentDataMember = "weightSection";
            this.chartControl_point.Series[0].ValueScaleType = ScaleType.Numerical;
            this.chartControl_point.Series[0].ValueDataMembers.AddRange(new string[] { "countInSection" });
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

        //由从原始数据中分析出的数据更新饼图数据
        private void updatePieHistoryData()
        {

            if (dtPieHistory.Rows.Count == 0)
            {
                DataRow drCountUnderWeight = dtPieHistory.NewRow();
                drCountUnderWeight["status"] = "欠重";
                drCountUnderWeight["countHistory"] = this.countUnderWeight;
                dtPieHistory.Rows.Add(drCountUnderWeight);

                DataRow drCountOverWeight = dtPieHistory.NewRow();
                drCountOverWeight["status"] = "超重";
                drCountOverWeight["countHistory"] = this.countOverWeight;
                dtPieHistory.Rows.Add(drCountOverWeight);

                DataRow drCountNormal = dtPieHistory.NewRow();
                drCountNormal["status"] = "正常";
                drCountNormal["countHistory"] = this.countDetectionHistory - this.countOverWeight - this.countUnderWeight;
                dtPieHistory.Rows.Add(drCountNormal);
            }
            else
            {
                dtPieHistory.Rows[0]["countHistory"] = this.countUnderWeight;
                dtPieHistory.Rows[1]["countHistory"] = this.countOverWeight;
                dtPieHistory.Rows[2]["countHistory"] = this.countDetectionHistory - this.countUnderWeight - this.countOverWeight;
            }
        }

        //由从原始数据中分析出的数据更新点图数据
        private void updatePointHistoryData()
        {
            dtPointHistory.Rows.Clear();
            for(int i = 0; i < weightAndCountGramDtPoint.Count; i++)
            {
                DataRow dr = dtPointHistory.NewRow();
                dr["weightSection"] = weightAndCountGramDtPoint.ElementAt(i).Key;
                dr["countInSection"] = weightAndCountGramDtPoint.ElementAt(i).Value.ToString();
                dtPointHistory.Rows.Add(dr);
            }
        }

        private void updateChartPieAndChartPoint()
        {
            Global.queryStatusHistoryMySQL(startTime, endTime, ref dtStatusHistory);//将起始时间到终止时间内查询（weighthistory）到的数据放在dtStatusHistory；
            pieDataAndPointDataProcessFromStatusHistory();//对dtStatusHistory进行处理；
            updatePieHistoryData();
            updatePointHistoryData();
        }

        //根据原始数据表进行分析得到饼图数据和点图数据
        private void pieDataAndPointDataProcessFromStatusHistory()
        {
            this.countDetectionHistory = dtStatusHistory.Rows.Count;//查询时间段内重量状态数据个数；
            this.countOverWeight = 0;       //将超重计数清0
            this.countUnderWeight = 0;      //将欠重计数清0

            weightAndCountGramDtPoint.Clear();  
           
            for(int i = 0; i < dtStatusHistory.Rows.Count; i++)
            {
                //PieHistory数据
                if (dtStatusHistory.Rows[i]["status"].ToString() == "H-")
                {
                    this.countOverWeight++;
                }
                else if(dtStatusHistory.Rows[i]["status"].ToString() == "L-")
                {
                    this.countUnderWeight++;
                }

                //PointHistory数据
                int weightGram = weightIntervalProcess(Convert.ToDouble(dtStatusHistory.Rows[i]["historyWeight"]));//对查询的重量数据进行处理；

                if (weightAndCountGramDtPoint.ContainsKey(weightGram) == false)//判断查询的重量数据是否第一次出现；
                {
                    weightAndCountGramDtPoint.Add(weightGram, 1);//将查询的重量数据写入字典（ weightAndCountGramDtPoint）；1-个数；
                }
                else
                {
                    weightAndCountGramDtPoint[weightGram]++;//对已出现的重量数据个数加1；
                }

            }
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

        private void setAxisXMinMaxPoint(int minKilogram, int maxKilogram)
        {
            if (minKilogram < maxKilogram)
            {
                minXPoint = minXRangePoint[minKilogram];
                maxXPoint = maxXRangePoint[maxKilogram];
                XYDiagram diagram = (XYDiagram)this.chartControl_point.Diagram;
                diagram.AxisX.WholeRange.SetMinMaxValues(minXPoint, maxXPoint);
            }
            else
            {
                MessageBox.Show("请输入合适的重量显示范围");
            }

        }

        //设置散点图横轴分辨率档位
        private void setGearIntervalAxisXPoint(int gear)
        {
            if (gear <= 3 && gear >= 0)
            {
                gearIntervalAxisXPoint = gear;
            }
            else
            {
                MessageBox.Show("请输入正确的档位");
                throw new Exception();
            }
        }

        private int weightIntervalProcess(double weightKilogram)
        {
            //小于minXPoint的设为minXPoint，大于maxXPoint的设为maxXPoint
            int weightgram = Convert.ToInt32(weightKilogram * 1000);
            int weight = 0;
            if (weightgram <= minXPoint)
            {
                weight = minXPoint;
            }
            else if (weightgram >= maxXPoint)
            {
                weight = maxXPoint;
            }
            else
            {
                //除法效率低，待优化（左右移）
                int countInterval = (weightgram - minXPoint) / intervalAxisXPoint[gearIntervalAxisXPoint];
                int remainder = (weightgram - minXPoint) - countInterval * intervalAxisXPoint[gearIntervalAxisXPoint];
                int intervalDeviceTwo = intervalAxisXPoint[gearIntervalAxisXPoint] / 2;
                if (remainder < intervalDeviceTwo)
                {
                    weight = minXPoint + countInterval * intervalAxisXPoint[gearIntervalAxisXPoint];
                }
                else
                {
                    weight = minXPoint + (1 + countInterval) * intervalAxisXPoint[gearIntervalAxisXPoint];
                }
            }

            return weight;
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

        private void simpleButton_startTimeModify_Click(object sender, EventArgs e)
        {
            this.timeEdit_startTime.ShowPopup();//弹出起始时间滚动窗；
        }

        private void simpleButton_endTimeModify_Click(object sender, EventArgs e)
        {
            this.timeEdit_endTime.ShowPopup();//弹出终止时间滚动窗；
        }

        private void simpleButton_query1Week_Click(object sender, EventArgs e)
        {
            startTime = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd HH:mm:ss");
            endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            updateChartPieAndChartPoint();
        }

        private void simpleButton_query1Month_Click(object sender, EventArgs e)
        {
            startTime = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd HH:mm:ss");
            endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            updateChartPieAndChartPoint();
        }

        private void simpleButton_query3Months_Click(object sender, EventArgs e)
        {
            startTime = DateTime.Now.AddMonths(-3).ToString("yyyy-MM-dd HH:mm:ss");
            endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            updateChartPieAndChartPoint();
        }

        private void simpleButton_query6Months_Click(object sender, EventArgs e)
        {
            startTime = DateTime.Now.AddMonths(-6).ToString("yyyy-MM-dd HH:mm:ss");
            endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            updateChartPieAndChartPoint();
        }

        private void simpleButton_query_Click(object sender, EventArgs e)//查询某一时间段内
        {
            if (_timeInterValIllegal() == true)//判断起始时间与终止时间大小关系；（true:非法；false:合理）
            {
                showCloseConfirmBox("时间区间选择错误！！请重新选择！！", "info");
            }
            else
            {
                startTime = this.timeEdit_startTime.Time.ToString("yyyy-MM-dd HH:mm:ss");
                endTime = this.timeEdit_endTime.Time.ToString("yyyy-MM-dd HH:mm:ss");
                updateChartPieAndChartPoint();

            }
        }


    }
}
