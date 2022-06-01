using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckWeighterInterface
{
    class Global
    {
        /******************************************************系统设置*********************************************************************/
        public static MySQL.MySQLHelper mysqlHelper1 = new MySQL.MySQLHelper("localhost", "check_weighter_interface", "root", "root");//last("root")：password;



        //public delegate void BrandChangedReinit(object sender, EventArgs e);
        //public static event BrandChangedReinit brandChangedReInitStatusMonitor;
        //public static event BrandChangedReinit brandChangedReInitTimeDomainAnalysis;
        //public static event BrandChangedReinit brandChangedReInitFrequencyDomainAnalysis;
        //public static event BrandChangedReinit brandChangedReInitExcelExport;
        //public static event BrandChangedReinit brandChangedReInitSystemConfig;

        /******************************************************全局参数********************************************************************/

        //状态参数
        public struct Status{
           public string curBrand;                          //当前品牌
           public double curWeight;                         //当前重量
           public string flagOverWeightOrUnderWeight;       //超重、欠重标志字符串
           public double lastOverWeight;                    //上次超重
           public double lastUnderWeight;                   //上次欠重
           public Int32 countDetection;                     //检测数量
           public Int32 countOverWeight;                    //超重数量
           public Int32 countUnderWeight;                   //欠重数量
           public double maxWeightInHistory;                //最大值
           public double minWeightInHistory;                //最小值
        };
        public static Status curStatus = new Status();      //当前状态
        
        //品牌信息
        public struct Brand
        {
            public string brandName;                        //品牌名
            public double standardWeight;                   //品牌标准重量
            public double upperLimit;                       //品牌重量上限
            public double lowerLimit;                       //品牌重量下限
        };


        public static double underWeightThreshold;          //欠重阈值（设定值为初始值）
        public static double overWeightThreshold;           //超重阈值（设定值为初始值）

        
        enum FilteringAlgorithmType { amplitudeLimitingFiltering = 0, medianFiltering, digitalAverageFiltering, recursionAverageFiltering, medianAverageFiltering, amplitudeLimitingRecursionAverageFiltering, firstOrderLagFiltering, weightedRecursionAverageFiltering, ditheringFiltering, amplitudeLimitingDitheringFiltering,}//add_filter--0516;

        /******************************************************全局方法************************************************************************/
        
        //初始化数据库
        public static void initMySQL()
        {
            if (!mysqlHelper1._connectMySQL())
            {
                MessageBox.Show("数据库连接失败");
            }
        }

        //将当前状态写入MySQL
        public static void insertCurStatusMySQL()
        {
            string cmdInsertStatus = "INSERT INTO weight_history (Brand, Weight, Status, DateTime) VALUES ("
                                     + "'" + curStatus.curBrand + "'" + ", " + curStatus.curWeight.ToString() + ", " + "'" + curStatus.flagOverWeightOrUnderWeight + "'" + ", CURRENT_TIMESTAMP());";
            bool flag = mysqlHelper1._insertMySQL(cmdInsertStatus);
        }

        //查询
        public static void queryStatusHistoryMySQL(string st, string et, ref DataTable dt)
        {
            string cmdQueryLineHistoryMySQL = "CALL queryLineHistoryMySQL('" + st + "', '" + et + "');";
            Global.mysqlHelper1._queryTableMySQL(cmdQueryLineHistoryMySQL, ref dt);
        }

        //DataTable序号重排
        public static void reorderDt(ref DataTable dt, string colNOName)
        {
            int lenDt = dt.Rows.Count;
            for (int i = 0; i < lenDt; i++)
            {
                dt.Rows[i][colNOName] = (i + 1).ToString();
            }
        }

        //显示informationBox
        public static void showInforMationBox(XtraUserControl xx, CommonControl.InformationBox informationBox, string title, int locX, int locY)
        {
            if (informationBox != null)
                informationBox.Dispose();

            informationBox = new CommonControl.InformationBox();
            informationBox.Appearance.BackColor = System.Drawing.Color.White;
            informationBox.Appearance.Options.UseBackColor = true;
            informationBox.Name = "confirmationBox_invalidTime";
            informationBox.Size = new System.Drawing.Size(350, 150);
            informationBox.Location = new Point(locX, locY);
            informationBox.TabIndex = 29;
            informationBox.infoTitle = title;
            xx.Controls.Add(informationBox);
            informationBox.Visible = true;
            informationBox.BringToFront();
        }

        //向DataTable中添加行
        //countParams：列数。cols列名数组。paras参数值数组。
        public static bool dtRowAdd(ref DataTable dt, int countParams, string[] cols, object[] paras)
        {
            bool flag = true;
            if(!(countParams == dt.Columns.Count && countParams == cols.Length && countParams == paras.Length))
            {
                flag = false;
            }

            DataRow drTemp = dt.NewRow();
            for(int i = 0; i < countParams; i++)
            {
                drTemp[cols[i]] = paras[i];
            }
            dt.Rows.Add(drTemp);
            return flag;
        }

        //修改DataTable某行值
        public static bool dtRowModify(ref DataTable dt, int rowIndex, string[] cols, object[] vals)
        {
            bool flag = true;
            if (cols.Length != vals.Length || (dt.Rows.Count) < rowIndex)
            {
                flag = false;
            }

            for(int i = 0; i < cols.Length; i++)
            {
                dt.Rows[rowIndex][cols[i]] = vals[i];
            }
            return flag;
        }

        /******************************************************各个页面可能会被其他页面用到的变量***************************************************************************/

        //StatusMonitor
        public static bool enableRefreshStatusMonitor = true;                              //StatusMonitor页面刷新使能标志
        public static DataTable dtBrand = new DataTable("tableBrand");                     //品牌表
        public static DataTable dtLineStatusMonitor = new DataTable("tableLine");          //折线图数据源，只显示200个点
        public static DataTable dtPieStatusMonitor = new DataTable("tablePie");            //饼图数据源，只要不更换brand就一直累计
        public static DataTable dtPointStatusMonitor = new DataTable("tablePoint");        //散点图数据源，只要不更换brand就一直累计

        //RealTimeCurve
        public static bool enableReFreshRealTimeCurve = true;                                   //RealTimeCurve页面刷新使能标志
        public static DataTable dtSensorRealTimeData = new DataTable("dtSensorRealTimeData");   //传感器实时数据dt
        public static double sensorRealTimeDataPeak;                                            //传感器实时数据峰值
        public static double sensorRealTimeDataValley;                                          //传感器实时数据谷值
        public static double sensorRealTimeDataAvg;                                             //传感器实时数据平均值

        //CalibrationCorrection
        //public static double[] calibrationDataGradient;
        //当前显示的标定段数
        public static int curCalibrationSectionCount = 1;
        //当前显示的各端点传感器值和重量
        public static DataTable dtCalibrationDataSensorValAndWeight = new DataTable("calibrationData"); //标定端点数据表
        public static DataTable dtCalibrationGradient = new DataTable("dtCalibrationGradient");         //标定数据列表：各段斜率，用于根据传感器值计算重量。标定即计算斜率。
        public static double curSensorValue = 0.0D;                                                     //记录下位机上传的1个实时传感器值

        //AlgorithmConfig
        public enum AlgorithmName
        {
            amplitudeLimitingFiltering = 0, medianFiltering, digitalAverageFiltering, recursionAverageFiltering,
            medianAverageFiltering, amplitudeLimitingRecursionAverageFiltering, firstOrderLagFiltering, weightedRecursionAverageFiltering,
            ditheringFiltering, amplitudeLimitingDitheringFiltering
        };
        public static AlgorithmName curAlgorithm = AlgorithmName.amplitudeLimitingFiltering;        //当前系统使用的滤波算法





    }
}
