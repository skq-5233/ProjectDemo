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
using System.IO;
using NPOI;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System.Diagnostics;
using System.Collections;
using DevExpress.XtraEditors.Popup;
using DevExpress.XtraBars.Docking2010;
using DevExpress.XtraSplashScreen;
using Microsoft.Win32;


namespace CheckWeighterInterface.ExcelExport
{
    public partial class ExcelExport : DevExpress.XtraEditors.XtraUserControl
    {
        private CommonControl.InformationBox informationBox1;
        
        int exportExcelFileNameIndex = 1;

        string startTime = String.Empty;
        string endTime = String.Empty;

        public static DataTable dtStatusHistory = new DataTable("tableLineHistory");        //历史-原始数据表
        public static DataTable dtPieHistory = new DataTable("tablePieHistory");
        public static DataTable dtPointHistory = new DataTable("tablePointHistory");

        //每次查询时对原始数据进行处理后记录相关数据
        private int countUnderWeight = 0;
        private int countOverWeight = 0;
        private int countDetectionHistory = 0;

        struct WeightCountAndSection
        {
            public int count;
            public bool flagIntervalLeftOrRight;
        }
        WeightCountAndSection weightCountAndSection;

        Dictionary<int, WeightCountAndSection> weightAndCountGramDtPoint = new Dictionary<int, WeightCountAndSection>();    //<重量,(该重量出现个数,区间左侧，区间右侧)>

        //散点图横轴坐标边缘
        private Int32[] minXRangePoint = { 0, 1000, 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000, 10000, 11000, 12000, 13000, 14000, 15000 };
        private Int32[] maxXRangePoint = { 0, 1000, 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000, 10000, 11000, 12000, 13000, 14000, 15000 };
        private Int32 minXPoint = 0;    //横轴范围左侧
        private Int32 maxXPoint = 0;    //横轴范围右侧

        //散点图横轴分辨率档位
        private Int32[] intervalAxisXPoint = { 10, 20, 50, 100 };       //散点图横轴分辨率
        private Int32 gearIntervalAxisXPoint = 0;                       //散点图横轴档位0~3

        public ExcelExport()
        {
            InitializeComponent();
            initExcelExport();
        }
        private void reInitExcelExport(object sender, EventArgs e)
        {
            dtStatusHistory.Rows.Clear();
            dtPieHistory.Rows.Clear();
            dtPointHistory.Rows.Clear();

            initExcelExport();
            //MessageBox.Show("refresh");

        }

        private void initExcelExport()
        {
            initTimeEditStartAndEnd();
            initDataTable();
            bindDataGridControl();
            setAxisXMinMaxPoint(8, 12);  //设定散点图横轴区间范围
            setGearIntervalAxisXPoint(0);   //设定散点图横轴分辨率
            SystemManagement.BrandManagement.brandChangedReInitExcelExport += reInitExcelExport;
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
                //dtPointHistory.Columns.Add("weightSection", typeof(String));
                dtPointHistory.Columns.Add("countInSection", typeof(Int32));
                dtPointHistory.Columns.Add("flagIntervalLeftOrRight", typeof(Boolean));
            }

        }

        private void initTimeEditStartAndEnd()
        {
            DateTime nowdt = DateTime.Now;
            DateTime oneMonthAgo = DateTime.Now.AddMonths(-1);  //当前日期的一个月前日期
            this.timeEdit_startTime.Time = oneMonthAgo;
            this.timeEdit_endTime.Time = nowdt;
            startTime = timeEdit_startTime.Time.ToString("yyyy-MM-dd HH:mm:ss");
            endTime = timeEdit_endTime.Time.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void bindDataGridControl()
        {
            this.gridControl_weightList.DataSource = dtStatusHistory;
        }

        private void updateChartPieAndChartPoint()
        {
            Global.queryStatusHistoryMySQL(startTime, endTime, ref dtStatusHistory);
            pieDataAndPointDataProcessFromStatusHistory();
            updatePieHistoryData();
            updatePointHistoryData();
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
            for (int i = 0; i < weightAndCountGramDtPoint.Count; i++)
            {
                DataRow dr = dtPointHistory.NewRow();
                //if(weightAndCountGramDtPoint.ElementAt(i).Value.flagIntervalLeftOrRight == false)
                //{
                //    dr["weightSection"] = weightAndCountGramDtPoint.ElementAt(i).Key.ToString() + "-" + (weightAndCountGramDtPoint.ElementAt(i).Key + intervalAxisXPoint[gearIntervalAxisXPoint]).ToString();
                //}
                //else
                //{
                //    dr["weightSection"] = weightAndCountGramDtPoint.ElementAt(i).Key.ToString() + "-" + (weightAndCountGramDtPoint.ElementAt(i).Key + intervalAxisXPoint[gearIntervalAxisXPoint]).ToString();
                //}
                dr["weightSection"] = weightAndCountGramDtPoint.ElementAt(i).Key;
                dr["countInSection"] = weightAndCountGramDtPoint.ElementAt(i).Value.count;
                dr["flagIntervalLeftOrRight"] = weightAndCountGramDtPoint.ElementAt(i).Value.flagIntervalLeftOrRight;
                dtPointHistory.Rows.Add(dr);
            }
        }

        //根据原始数据表进行分析得到饼图数据和点图数据
        private void pieDataAndPointDataProcessFromStatusHistory()
        {
            this.countDetectionHistory = dtStatusHistory.Rows.Count;
            this.countOverWeight = 0;       //将超重计数清0
            this.countUnderWeight = 0;      //将欠重计数清0

            weightAndCountGramDtPoint.Clear();

            for (int i = 0; i < dtStatusHistory.Rows.Count; i++)
            {
                //PieHistory数据
                if (dtStatusHistory.Rows[i]["status"].ToString() == "H-")
                {
                    this.countOverWeight++;
                    dtStatusHistory.Rows[i]["status"] = "超重";
                }
                else if (dtStatusHistory.Rows[i]["status"].ToString() == "L-")
                {
                    this.countUnderWeight++;
                    dtStatusHistory.Rows[i]["status"] = "欠重";
                }
                else
                {
                    dtStatusHistory.Rows[i]["status"] = "正常";
                }
                Global.reorderDt(ref dtStatusHistory, "indexDetection");

                //PointHistory数据
                bool flagIntervalLeftRight = false;     //weightGram取的是区间左侧值还是右侧值，false=左侧值
                int weightGram = weightIntervalProcess(Convert.ToDouble(dtStatusHistory.Rows[i]["historyWeight"]), ref flagIntervalLeftRight);

                if (weightAndCountGramDtPoint.ContainsKey(weightGram) == false)
                {
                    weightCountAndSection.count = 1;
                    weightCountAndSection.flagIntervalLeftOrRight = flagIntervalLeftRight;
                    weightAndCountGramDtPoint.Add(weightGram, weightCountAndSection);
                }
                else
                {
                    weightCountAndSection.count = weightAndCountGramDtPoint[weightGram].count + 1;
                    weightCountAndSection.flagIntervalLeftOrRight = flagIntervalLeftRight;
                    weightAndCountGramDtPoint[weightGram] = weightCountAndSection;
                    //weightAndCountGramDtPoint.Remove(weightGram);
                    //weightAndCountGramDtPoint.Add(weightGram, weightCountAndSection);
                }

            }
        }

        private void tileView1_ItemCustomize(object sender, DevExpress.XtraGrid.Views.Tile.TileViewItemCustomizeEventArgs e)
        {
            if (e.Item == null || e.Item.Elements.Count == 0)
                return;
            if ((string)tileView1.GetRowCellValue(e.RowHandle, tileView1.Columns["status"]) == "欠重")
            {
                e.Item.AppearanceItem.Normal.ForeColor = Color.FromArgb(111,186,208);
            }
            else if ((string)tileView1.GetRowCellValue(e.RowHandle, tileView1.Columns["status"]) == "超重")
            {
                e.Item.AppearanceItem.Normal.ForeColor = Color.FromArgb(230,108,125);
            }
            else if ((string)tileView1.GetRowCellValue(e.RowHandle, tileView1.Columns["status"]) == "正常")
            {
                e.Item.AppearanceItem.Normal.ForeColor = Color.FromArgb(94, 196, 104);


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

        private int weightIntervalProcess(double weightKilogram,ref bool flagIntervalLeftOrRight)
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
                int countInterval = (weightgram - minXPoint) / intervalAxisXPoint[gearIntervalAxisXPoint];
                int remainder = (weightgram - minXPoint) % intervalAxisXPoint[gearIntervalAxisXPoint];
                int intervalDeviceTwo = intervalAxisXPoint[gearIntervalAxisXPoint] / 2;
                if (remainder < intervalDeviceTwo)
                {
                    weight = minXPoint + countInterval * intervalAxisXPoint[gearIntervalAxisXPoint];
                    flagIntervalLeftOrRight = false;    //false代表取区间左侧值
                }
                else
                {
                    weight = minXPoint + (1 + countInterval) * intervalAxisXPoint[gearIntervalAxisXPoint];
                    flagIntervalLeftOrRight = true;    //false代表取区间右侧值
                }
            }

            return weight;
        }

        private string defaultFolder = String.Empty;
        private void exportExcelDtHistoryQueryGridShow()
        {
            string path = String.Empty;

            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            //注册表法记录上次打开路径（软件重开后仍然有效）
            try
            {

                RegistryKey testKey = Registry.CurrentUser.OpenSubKey("TestKey");
                if (testKey == null)
                {
                    testKey = Registry.CurrentUser.CreateSubKey("TestKey");
                    testKey.SetValue("OpenFolderDir", "");
                    testKey.Close();
                    Registry.CurrentUser.Close();
                }
                else
                {
                    defaultFolder = testKey.GetValue("OpenFolderDir").ToString();
                    testKey.Close();
                    Registry.CurrentUser.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

            //folderDlg.SelectedPath = defaultFolder;     //由上次打开的路径处打开
            if (defaultFolder != "")
            {
                //设置此次默认目录为上一次选中目录                  
                folderDlg.SelectedPath = defaultFolder;
            }

            DialogResult drs = folderDlg.ShowDialog();
            if (drs == DialogResult.OK)
            {
                if (defaultFolder != folderDlg.SelectedPath)
                {
                    defaultFolder = folderDlg.SelectedPath;
                    RegistryKey testKey = Registry.CurrentUser.OpenSubKey("TestKey", true);  //true表示可写，false表示只读
                    testKey.SetValue("OpenFolderDir", defaultFolder);
                    testKey.Close();
                    Registry.CurrentUser.Close();
                }

                //defaultFolder = folderDlg.SelectedPath;      //更新默认路径为上次打开的路径
                path = defaultFolder + "\\称重报表";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string excelFileName = path + "\\称重报表" + exportExcelFileNameIndex++.ToString() + "(" + this.timeEdit_startTime.Time.ToString("yyyyMMddHHmmss") + "—" + this.timeEdit_startTime.Time.ToString("yyyyMMddHHmmss") + ")" + ".xlsx";
                FileStream filestream = new FileStream(excelFileName, FileMode.OpenOrCreate);

                XSSFWorkbook wk = new XSSFWorkbook();

                if(this.checkEdit_exportOriginalData.Checked == true)
                {
                    ISheet isheet = wk.CreateSheet("原始数据");
                    IRow row = null;
                    ICell cell = null;
                    //加表头
                    row = isheet.CreateRow(0);
                    cell = row.CreateCell(0);
                    cell.SetCellValue("序号");
                    cell = row.CreateCell(1);
                    cell.SetCellValue("品牌");
                    cell = row.CreateCell(2);
                    cell.SetCellValue("重量(kg)");
                    cell = row.CreateCell(3);
                    cell.SetCellValue("状态");
                    cell = row.CreateCell(4);
                    cell.SetCellValue("称重时间");

                    for (int i = 1; i < dtStatusHistory.Rows.Count + 1; i++)
                    {
                        row = isheet.CreateRow(i);
                        for (int j = 0; j < dtStatusHistory.Columns.Count; j++)
                        {
                            cell = row.CreateCell(j);
                            cell.SetCellValue(dtStatusHistory.Rows[i - 1][j].ToString());
                        }
                    }
                }

                if(this.checkEdit_exportPieData.Checked == true)
                {
                    ISheet isheet = wk.CreateSheet("状态占比数据");
                    IRow row = null;
                    ICell cell = null;
                    //加表头
                    row = isheet.CreateRow(0);
                    cell = row.CreateCell(0);
                    cell.SetCellValue("状态");
                    cell = row.CreateCell(1);
                    cell.SetCellValue("数量");
                    cell = row.CreateCell(2);
                    cell.SetCellValue("比例(%)");

                    row = isheet.CreateRow(1);
                    cell = row.CreateCell(0);
                    cell.SetCellValue(dtPieHistory.Rows[0]["status"].ToString());
                    cell = row.CreateCell(1);
                    cell.SetCellValue(dtPieHistory.Rows[0]["countHistory"].ToString());
                    cell = row.CreateCell(2);
                    cell.SetCellValue(100 * Convert.ToInt32(dtPieHistory.Rows[0]["countHistory"])/dtStatusHistory.Rows.Count);

                    row = isheet.CreateRow(2);
                    cell = row.CreateCell(0);
                    cell.SetCellValue(dtPieHistory.Rows[1]["status"].ToString());
                    cell = row.CreateCell(1);
                    cell.SetCellValue(dtPieHistory.Rows[1]["countHistory"].ToString());
                    cell = row.CreateCell(2);
                    cell.SetCellValue(100 * Convert.ToInt32(dtPieHistory.Rows[1]["countHistory"]) / dtStatusHistory.Rows.Count);


                    row = isheet.CreateRow(3);
                    cell = row.CreateCell(0);
                    cell.SetCellValue(dtPieHistory.Rows[2]["status"].ToString());
                    cell = row.CreateCell(1);
                    cell.SetCellValue(dtPieHistory.Rows[2]["countHistory"].ToString());
                    cell = row.CreateCell(2);
                    cell.SetCellValue(100 * Convert.ToInt32(dtPieHistory.Rows[2]["countHistory"]) / dtStatusHistory.Rows.Count);
                }

                if (this.checkEdit_exportPointData.Checked == true)
                {
                    ISheet isheet = wk.CreateSheet("重量区间分布数据");
                    IRow row = null;
                    ICell cell = null;
                    //加表头
                    row = isheet.CreateRow(0);
                    cell = row.CreateCell(0);
                    cell.SetCellValue("重量区间(g)");
                    cell = row.CreateCell(1);
                    cell.SetCellValue("数量");
                    //cell = row.CreateCell(2);
                    //cell.SetCellValue("比例(%)");

                    for (int i = 1; i < dtPointHistory.Rows.Count + 1; i++)
                    {
                        row = isheet.CreateRow(i);
                        cell = row.CreateCell(0);

                        if (Convert.ToBoolean(dtPointHistory.Rows[i - 1][2]) == false)
                        {
                            cell.SetCellValue(dtPointHistory.Rows[i - 1][0].ToString() + "-" + (Convert.ToInt32(dtPointHistory.Rows[i - 1][0]) + intervalAxisXPoint[gearIntervalAxisXPoint]).ToString());
                        }
                        else
                        {
                            cell.SetCellValue((Convert.ToInt32(dtPointHistory.Rows[i - 1][0]) - intervalAxisXPoint[gearIntervalAxisXPoint]).ToString() + "-" + dtPointHistory.Rows[i - 1][0].ToString());
                        }

                        cell = row.CreateCell(1);
                        cell.SetCellValue(dtPointHistory.Rows[i - 1][1].ToString());
                    }
                }

                wk.Write(filestream);   //通过流filestream将表wk写入文件
                filestream.Close(); //关闭文件流filestream
                wk.Close(); //关闭Excel表对象wk
                Global.showInforMationBox(this, informationBox1, "Excel导出成功...", 337, 100);
            }
            else if (drs == DialogResult.Cancel)
            {
                folderDlg.Dispose();
            }

        }

        private void simpleButton_startTimeModify_Click(object sender, EventArgs e)
        {
            this.timeEdit_startTime.ShowPopup();
        }

        private void simpleButton_endTimeModify_Click(object sender, EventArgs e)
        {
            this.timeEdit_endTime.ShowPopup();
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

        private void simpleButton_query_Click(object sender, EventArgs e)
        {
            if (_timeInterValIllegal() == true)
            {
                //showInforMationBox("无效时间区间，请重新选择...");
                Global.showInforMationBox(this, informationBox1, "无效时间区间，请重新选择...", 337, 100);
            }
            else
            {
                startTime = this.timeEdit_startTime.Time.ToString("yyyy-MM-dd HH:mm:ss");
                endTime = this.timeEdit_endTime.Time.ToString("yyyy-MM-dd HH:mm:ss");
                updateChartPieAndChartPoint();

            }
        }

        private void simpleButton_exportExcel_Click(object sender, EventArgs e)
        {
            if(this.checkEdit_exportOriginalData.Checked || this.checkEdit_exportPieData.Checked || this.checkEdit_exportPointData.Checked)
            {
                exportExcelDtHistoryQueryGridShow();
            }
            else
            {
                //showInforMationBox("请至少勾选一项数据类型...");
                Global.showInforMationBox(this, informationBox1, "请至少勾选一项数据类型...", 337, 100);
            }
        }

    }
}
