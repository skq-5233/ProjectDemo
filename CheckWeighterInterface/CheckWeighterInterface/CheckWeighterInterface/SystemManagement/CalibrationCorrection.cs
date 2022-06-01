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

namespace CheckWeighterInterface.SystemManagement
{
    public partial class CalibrationCorrection : DevExpress.XtraEditors.XtraUserControl
    {
        /*只有dtCalibrationDataSensorValAndWeight和dtCalibrationGradient是绑定页面控件的数据源*/
        private CommonControl.NumberKeyboard numberKeyboard1;

        //数字小键盘当前修改的是哪个参数     
        private enum ModifySensorValueOrCalibrationWeight { curModifySensorValue = 0, curModifyCalibrationWeight};     
        private ModifySensorValueOrCalibrationWeight curModifyValueType;

        //当前模式
        private enum CalibrationMode { singleSectionCalibration = 0, multiSectionCalibration};         
        private CalibrationMode curCalibrationMode = CalibrationMode.singleSectionCalibration;      

        //MySQL表暂存
        //读取：初始化页面时、保存数据时
        private DataTable dtCalibrationModeAndCountSectionQueryMySQL = new DataTable("calibrationModeAndCountSectionQueryMySQL");   //记录上次标定模式、段数、是否被标定过
        private DataTable dtSingleModeCalibrationDataQueryMySQL = new DataTable("singleModeCalibrationDataQueryMySQL");             //记录从MySQL中读取的单段模式标定数据
        private DataTable dtSingleModeGradientQueryMySQL = new DataTable("singleModeGradientQueryMySQL");                           //记录从MySQL中读取的单段模式斜率
        private DataTable dtMultiModeCalibrationDataQueryMySQL = new DataTable("multiModeCalibrationDataQueryMySQL");                //记录从MySQL中读取的多段模式标定数据
        private DataTable dtMultiModeGradientQueryMySQL = new DataTable("multiModeGradientQueryMySQL");                             //记录从MySQL中读取的多段模式斜率

        //上次关闭时是否被标定过，关联dtCalibrationModeAndCountSectionQueryMySQL
        //修改：初始化页面时，读取到时。保存数据时
        private enum HasBeenCalibrated { HasNotBeenCalibrated = 0, hasBeenSingleCalibrated, hasBeenMultiCalibrated, hasBeenBothCalibrated};
        private HasBeenCalibrated hasBeenCalibrated;                //记录之前是否被标定过
        private int countSectionBeenMultiCalibrated;                //记录之前被标定的段数
        

        //记录当前数据是否经过了修改
        //修改：修改重量点击enter、手动标定点击enter
        private bool isCalibrationDataModified = false;             //后续可考虑以bit对每行的变量进行追踪

        //记录是否进行了标定（是否调用calcGradient计算斜率）
        private bool isCalibrated = false;                          


        public CalibrationCorrection()
        {
            InitializeComponent();
            initCalibrationCorrection();
        }

        //初始化标定页面
        //显示模式记忆上次关闭软件时的模式
        //从MySQL分别读取单段标定数据表、多段数据表，若有数据则显示作为2个dt值并在grid中显示，供用户修改。若没有数据，则在grid中显示NaN
        private void initCalibrationCorrection()
        {
            initDt();
            queryHasBeenCalibratedAndDataGradient(true);

            this.gridControl_calibrationDataList.DataSource = Global.dtCalibrationDataSensorValAndWeight;
            ChartLineSettings();
            this.chartControl_calibrationGradient.Series[0].DataSource = Global.dtCalibrationDataSensorValAndWeight;
            this.gridControl_calibrationGradient.DataSource = Global.dtCalibrationGradient;
        }

        //初始化datatable
        private void initDt()
        {
            if (Global.dtCalibrationDataSensorValAndWeight.Columns.Count == 0)
            {
                Global.dtCalibrationDataSensorValAndWeight.Columns.Add("NO", typeof(Int16));
                Global.dtCalibrationDataSensorValAndWeight.Columns.Add("sensorValue", typeof(double));      //传感器值为Int还是double？
                Global.dtCalibrationDataSensorValAndWeight.Columns.Add("calibrationWeight", typeof(double));
            }

            //Global.calibrationDataGradient = new double[1];
            if (Global.dtCalibrationGradient.Columns.Count == 0)
            {
                Global.dtCalibrationGradient.Columns.Add("NO", typeof(Int16));
                Global.dtCalibrationGradient.Columns.Add("section", typeof(String));
                Global.dtCalibrationGradient.Columns.Add("gradient", typeof(double));      //传感器值为Int还是double？
            }

            if(dtCalibrationModeAndCountSectionQueryMySQL.Columns.Count == 0)
            {
                dtCalibrationModeAndCountSectionQueryMySQL.Columns.Add("hasBeenCalibrated", typeof(Int32));   //0表示未标定过，1表示单段标定过，2表示多段标定过，3表示两种模式都标定过
                dtCalibrationModeAndCountSectionQueryMySQL.Columns.Add("countSection", typeof(Int32));
            }

            if (dtSingleModeCalibrationDataQueryMySQL.Columns.Count == 0)
            {
                dtSingleModeCalibrationDataQueryMySQL.Columns.Add("NO", typeof(Int16));
                dtSingleModeCalibrationDataQueryMySQL.Columns.Add("sensorValue", typeof(double));
                dtSingleModeCalibrationDataQueryMySQL.Columns.Add("calibrationWeight", typeof(double));
            }

            if (dtSingleModeGradientQueryMySQL.Columns.Count == 0)
            {
                dtSingleModeGradientQueryMySQL.Columns.Add("NO", typeof(Int16));
                dtSingleModeGradientQueryMySQL.Columns.Add("section", typeof(String));
                dtSingleModeGradientQueryMySQL.Columns.Add("gradient", typeof(double));
            }

            if (dtMultiModeCalibrationDataQueryMySQL.Columns.Count == 0)
            {
                dtMultiModeCalibrationDataQueryMySQL.Columns.Add("NO", typeof(Int16));
                dtMultiModeCalibrationDataQueryMySQL.Columns.Add("sensorValue", typeof(double));
                dtMultiModeCalibrationDataQueryMySQL.Columns.Add("calibrationWeight", typeof(double));
            }

            if (dtMultiModeGradientQueryMySQL.Columns.Count == 0)
            {
                dtMultiModeGradientQueryMySQL.Columns.Add("NO", typeof(Int16));
                dtMultiModeGradientQueryMySQL.Columns.Add("section", typeof(String));
                dtMultiModeGradientQueryMySQL.Columns.Add("gradient", typeof(double));
            }
        }

        //折线图参数设定
        private void ChartLineSettings()
        {
            this.chartControl_calibrationGradient.Series[0].ArgumentScaleType = ScaleType.Numerical;   
            this.chartControl_calibrationGradient.Series[0].ArgumentDataMember = "sensorValue";       
            this.chartControl_calibrationGradient.Series[0].ValueScaleType = ScaleType.Numerical;  
            this.chartControl_calibrationGradient.Series[0].ValueDataMembers.AddRange(new string[] { "calibrationWeight" });
            this.chartControl_calibrationGradient.Series[0].LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;                
            this.chartControl_calibrationGradient.Series[0].Label.ResolveOverlappingMode = ResolveOverlappingMode.JustifyAllAroundPoint;
            this.chartControl_calibrationGradient.Series[0].Label.TextPattern = "({A},{V:F3})";
            ((XYDiagram)(chartControl_calibrationGradient.Diagram)).EnableAxisXScrolling = true;   
            ((XYDiagram)(chartControl_calibrationGradient.Diagram)).EnableAxisYScrolling = true;
        }

        //设定横轴范围为[min-0.2*delta,max+0.2*delta]
        private void setChartLineXYWholeRange(double k)
        {
            double xmin, xmax, ymin, ymax;

            int xminIndex = 0;
            int xmaxIndex = Global.dtCalibrationDataSensorValAndWeight.Rows.Count - 1;
            for (int i = 0; i < Global.dtCalibrationDataSensorValAndWeight.Rows.Count; i++)
            {
                double tempWeight = Convert.ToDouble(Global.dtCalibrationDataSensorValAndWeight.Rows[i]["sensorValue"]);

                if (tempWeight < Convert.ToDouble(Global.dtCalibrationDataSensorValAndWeight.Rows[xminIndex]["sensorValue"]))
                {
                    xminIndex = i;
                }
                else if (tempWeight > Convert.ToDouble(Global.dtCalibrationDataSensorValAndWeight.Rows[xmaxIndex]["sensorValue"]))
                {
                    xmaxIndex = i;
                }
            }
            xmin = Convert.ToDouble(Global.dtCalibrationDataSensorValAndWeight.Rows[xminIndex]["sensorValue"]);
            xmax = Convert.ToDouble(Global.dtCalibrationDataSensorValAndWeight.Rows[xmaxIndex]["sensorValue"]);


            int yminIndex = 0;
            int ymaxIndex = Global.dtCalibrationDataSensorValAndWeight.Rows.Count - 1;
            for (int i = 0; i < Global.dtCalibrationDataSensorValAndWeight.Rows.Count; i++)
            {
                double tempWeight = Convert.ToDouble(Global.dtCalibrationDataSensorValAndWeight.Rows[i]["calibrationWeight"]);

                if (tempWeight < Convert.ToDouble(Global.dtCalibrationDataSensorValAndWeight.Rows[yminIndex]["calibrationWeight"]))
                {
                    yminIndex = i;
                }
                else if (tempWeight > Convert.ToDouble(Global.dtCalibrationDataSensorValAndWeight.Rows[ymaxIndex]["calibrationWeight"]))
                {
                    ymaxIndex = i;
                }
            }
            ymin = Convert.ToDouble(Global.dtCalibrationDataSensorValAndWeight.Rows[xminIndex]["calibrationWeight"]);
            ymax = Convert.ToDouble(Global.dtCalibrationDataSensorValAndWeight.Rows[xmaxIndex]["calibrationWeight"]);


            ((XYDiagram)(chartControl_calibrationGradient.Diagram)).AxisX.WholeRange.SetMinMaxValues(xmin - k * (xmax - xmin), xmax + k * (xmax - xmin));
            ((XYDiagram)(chartControl_calibrationGradient.Diagram)).AxisY.WholeRange.SetMinMaxValues(ymin - k*(ymax - ymin), ymax + k*(ymax - ymin));

            ((XYDiagram)(chartControl_calibrationGradient.Diagram)).AxisX.VisualRange.Auto = true;
            ((XYDiagram)(chartControl_calibrationGradient.Diagram)).AxisY.VisualRange.Auto = true;
        }


        //从MySQL中查询是否被标定过、端点数据、斜率到3个暂存datatable
        private void queryHasBeenCalibratedAndDataGradient(bool isInit)
        {
            //从MySQL读取模式、段数、是否被标定过
            string cmdQueryCalibrationMode = "SELECT * FROM mode_count_section;";
            
            Global.mysqlHelper1._queryTableMySQL(cmdQueryCalibrationMode, ref dtCalibrationModeAndCountSectionQueryMySQL);
            hasBeenCalibrated = (HasBeenCalibrated)Convert.ToInt32(dtCalibrationModeAndCountSectionQueryMySQL.Rows[0]["hasBeenCalibrated"]);
            countSectionBeenMultiCalibrated = Convert.ToInt32(dtCalibrationModeAndCountSectionQueryMySQL.Rows[0]["countSection"]);

            //初始化时的查询，由hasBeenCalibrated决定当前显示模式。单段模式有3种情况，多段模式只有1种情况
            //不是初始化而是切换时查询，由curCalibrationMode决定，2种模式各有4种情况
            if (isInit)
            {
                if (hasBeenCalibrated == HasBeenCalibrated.hasBeenMultiCalibrated)   //只有多段已标定时显示多段，其他情况均显示单段
                {
                    curCalibrationMode = CalibrationMode.multiSectionCalibration;
                }
            }

            //从MySQL读取标定数据
            string cmdQuerySingleCalibrationData = "SELECT * FROM single_mode_sensor_value_weight;";

            string cmdQuerySingleModeGradient = "SELECT * FROM single_mode_gradient;";

            string cmdQueryMultiCalibrationData = "SELECT * FROM multi_mode_sensor_value_weight;";

            string cmdQueryMultiModeGradient = "SELECT * FROM multi_mode_gradient;";

            string[] colsDtCalibrationDataSensorValAndWeight = { "NO", "sensorValue", "calibrationWeight" };
            string[] colsDtCalibrationGradient = { "NO", "gradient" };
            if (curCalibrationMode == CalibrationMode.singleSectionCalibration)
            {
                if (hasBeenCalibrated == HasBeenCalibrated.HasNotBeenCalibrated)
                {
                    //object[] paras1 = { 1, Double.NaN, Double.NaN };
                    //object[] paras2 = { 2, Double.NaN, Double.NaN };
                    ////Global.calibrationDataGradient[i] = delta1 / delta2;
                    //Global.dtRowAdd(ref dtCalibrationDataSensorValAndWeight, 3, colsDtCalibrationDataSensorValAndWeight, paras1);
                    //Global.dtRowAdd(ref dtCalibrationDataSensorValAndWeight, 3, colsDtCalibrationDataSensorValAndWeight, paras2);

                    //object[] parasDtCalibrationGradient1 = { "1-2", Double.NaN };
                    //Global.dtRowAdd(ref Global.dtCalibrationGradient, 2, colsDtCalibrationGradient, parasDtCalibrationGradient1);

                    Global.curCalibrationSectionCount = 1;    
                    switchCalibrationMode(curCalibrationMode, Global.curCalibrationSectionCount);

                    allocateCapacityCalibrationData(Global.curCalibrationSectionCount);
                }
                else if (hasBeenCalibrated == HasBeenCalibrated.hasBeenSingleCalibrated)
                {
                    Global.curCalibrationSectionCount = 1;
                    switchCalibrationMode(curCalibrationMode, Global.curCalibrationSectionCount);

                    allocateCapacityCalibrationData(Global.curCalibrationSectionCount);

                    Global.mysqlHelper1._queryTableMySQL(cmdQuerySingleCalibrationData, ref dtSingleModeCalibrationDataQueryMySQL);
                    Global.mysqlHelper1._queryTableMySQL(cmdQuerySingleModeGradient, ref dtSingleModeGradientQueryMySQL);

                    for(int i=0;i< dtSingleModeCalibrationDataQueryMySQL.Rows.Count; i++)
                    {
                        Global.dtCalibrationDataSensorValAndWeight.Rows[i]["NO"] = Convert.ToInt16(dtSingleModeCalibrationDataQueryMySQL.Rows[i]["NO"]);
                        Global.dtCalibrationDataSensorValAndWeight.Rows[i]["sensorValue"] = Convert.ToDouble(dtSingleModeCalibrationDataQueryMySQL.Rows[i]["sensorValue"]);
                        Global.dtCalibrationDataSensorValAndWeight.Rows[i]["calibrationWeight"] = Convert.ToDouble(dtSingleModeCalibrationDataQueryMySQL.Rows[i]["calibrationWeight"]);
                    }

                    for (int i = 0; i < dtSingleModeGradientQueryMySQL.Rows.Count; i++)
                    {
                        Global.dtCalibrationGradient.Rows[i]["NO"] = Convert.ToInt16(dtSingleModeGradientQueryMySQL.Rows[i]["NO"]);
                        Global.dtCalibrationGradient.Rows[i]["section"] = dtSingleModeGradientQueryMySQL.Rows[i]["section"].ToString();
                        Global.dtCalibrationGradient.Rows[i]["gradient"] = Convert.ToDouble(dtSingleModeGradientQueryMySQL.Rows[i]["gradient"]);
                    }

                    refreshChartLineCalibrationGradient();
                }
                else if(hasBeenCalibrated == HasBeenCalibrated.hasBeenMultiCalibrated)
                {
                    Global.curCalibrationSectionCount = 1;
                    switchCalibrationMode(curCalibrationMode, Global.curCalibrationSectionCount);

                    allocateCapacityCalibrationData(Global.curCalibrationSectionCount);
                }
                else if(hasBeenCalibrated == HasBeenCalibrated.hasBeenBothCalibrated)
                {
                    Global.curCalibrationSectionCount = 1;
                    switchCalibrationMode(curCalibrationMode, Global.curCalibrationSectionCount);

                    allocateCapacityCalibrationData(Global.curCalibrationSectionCount);

                    Global.mysqlHelper1._queryTableMySQL(cmdQuerySingleCalibrationData, ref dtSingleModeCalibrationDataQueryMySQL);
                    Global.mysqlHelper1._queryTableMySQL(cmdQuerySingleModeGradient, ref dtSingleModeGradientQueryMySQL);

                    for (int i = 0; i < dtSingleModeCalibrationDataQueryMySQL.Rows.Count; i++)
                    {
                        Global.dtCalibrationDataSensorValAndWeight.Rows[i]["NO"] = Convert.ToInt16(dtSingleModeCalibrationDataQueryMySQL.Rows[i]["NO"]);
                        Global.dtCalibrationDataSensorValAndWeight.Rows[i]["sensorValue"] = Convert.ToDouble(dtSingleModeCalibrationDataQueryMySQL.Rows[i]["sensorValue"]);
                        Global.dtCalibrationDataSensorValAndWeight.Rows[i]["calibrationWeight"] = Convert.ToDouble(dtSingleModeCalibrationDataQueryMySQL.Rows[i]["calibrationWeight"]);
                    }

                    for (int i = 0; i < dtSingleModeGradientQueryMySQL.Rows.Count; i++)
                    {
                        Global.dtCalibrationGradient.Rows[i]["NO"] = Convert.ToInt16(dtSingleModeGradientQueryMySQL.Rows[i]["NO"]);
                        Global.dtCalibrationGradient.Rows[i]["section"] = dtSingleModeGradientQueryMySQL.Rows[i]["section"].ToString();
                        Global.dtCalibrationGradient.Rows[i]["gradient"] = Convert.ToDouble(dtSingleModeGradientQueryMySQL.Rows[i]["gradient"]);
                    }

                    refreshChartLineCalibrationGradient();
                }
            }
            else if (curCalibrationMode == CalibrationMode.multiSectionCalibration)
            {
                if (hasBeenCalibrated == HasBeenCalibrated.HasNotBeenCalibrated)
                {
                    Global.curCalibrationSectionCount = 2;
                    switchCalibrationMode(curCalibrationMode, Global.curCalibrationSectionCount);

                    allocateCapacityCalibrationData(Global.curCalibrationSectionCount);
                }
                else if (hasBeenCalibrated == HasBeenCalibrated.hasBeenSingleCalibrated)
                {
                    Global.curCalibrationSectionCount = 2;
                    switchCalibrationMode(curCalibrationMode, Global.curCalibrationSectionCount);

                    allocateCapacityCalibrationData(Global.curCalibrationSectionCount);
                }
                else if (hasBeenCalibrated == HasBeenCalibrated.hasBeenMultiCalibrated)
                {
                    Global.curCalibrationSectionCount = countSectionBeenMultiCalibrated;
                    switchCalibrationMode(curCalibrationMode, Global.curCalibrationSectionCount);

                    allocateCapacityCalibrationData(Global.curCalibrationSectionCount);

                    Global.mysqlHelper1._queryTableMySQL(cmdQueryMultiCalibrationData, ref dtMultiModeCalibrationDataQueryMySQL);
                    Global.mysqlHelper1._queryTableMySQL(cmdQueryMultiModeGradient, ref dtMultiModeGradientQueryMySQL);

                    for (int i = 0; i < dtMultiModeCalibrationDataQueryMySQL.Rows.Count; i++)
                    {
                        Global.dtCalibrationDataSensorValAndWeight.Rows[i]["NO"] = Convert.ToInt16(dtMultiModeCalibrationDataQueryMySQL.Rows[i]["NO"]);
                        Global.dtCalibrationDataSensorValAndWeight.Rows[i]["sensorValue"] = Convert.ToDouble(dtMultiModeCalibrationDataQueryMySQL.Rows[i]["sensorValue"]);
                        Global.dtCalibrationDataSensorValAndWeight.Rows[i]["calibrationWeight"] = Convert.ToDouble(dtMultiModeCalibrationDataQueryMySQL.Rows[i]["calibrationWeight"]);
                    }

                    for (int i = 0; i < dtMultiModeGradientQueryMySQL.Rows.Count; i++)
                    {
                        Global.dtCalibrationGradient.Rows[i]["NO"] = Convert.ToInt16(dtMultiModeGradientQueryMySQL.Rows[i]["NO"]);
                        Global.dtCalibrationGradient.Rows[i]["section"] = dtMultiModeGradientQueryMySQL.Rows[i]["section"].ToString();
                        Global.dtCalibrationGradient.Rows[i]["gradient"] = Convert.ToDouble(dtMultiModeGradientQueryMySQL.Rows[i]["gradient"]);
                    }

                    refreshChartLineCalibrationGradient();

                }
                else if (hasBeenCalibrated == HasBeenCalibrated.hasBeenBothCalibrated)
                {
                    Global.curCalibrationSectionCount = countSectionBeenMultiCalibrated;
                    switchCalibrationMode(curCalibrationMode, Global.curCalibrationSectionCount);

                    allocateCapacityCalibrationData(Global.curCalibrationSectionCount);

                    Global.mysqlHelper1._queryTableMySQL(cmdQueryMultiCalibrationData, ref dtMultiModeCalibrationDataQueryMySQL);
                    Global.mysqlHelper1._queryTableMySQL(cmdQueryMultiModeGradient, ref dtMultiModeGradientQueryMySQL);

                    for (int i = 0; i < dtMultiModeCalibrationDataQueryMySQL.Rows.Count; i++)
                    {
                        Global.dtCalibrationDataSensorValAndWeight.Rows[i]["NO"] = Convert.ToInt16(dtMultiModeCalibrationDataQueryMySQL.Rows[i]["NO"]);
                        Global.dtCalibrationDataSensorValAndWeight.Rows[i]["sensorValue"] = Convert.ToDouble(dtMultiModeCalibrationDataQueryMySQL.Rows[i]["sensorValue"]);
                        Global.dtCalibrationDataSensorValAndWeight.Rows[i]["calibrationWeight"] = Convert.ToDouble(dtMultiModeCalibrationDataQueryMySQL.Rows[i]["calibrationWeight"]);
                    }

                    for (int i = 0; i < dtMultiModeGradientQueryMySQL.Rows.Count; i++)
                    {
                        Global.dtCalibrationGradient.Rows[i]["NO"] = Convert.ToInt16(dtMultiModeGradientQueryMySQL.Rows[i]["NO"]);
                        Global.dtCalibrationGradient.Rows[i]["section"] = dtMultiModeGradientQueryMySQL.Rows[i]["section"].ToString();
                        Global.dtCalibrationGradient.Rows[i]["gradient"] = Convert.ToDouble(dtMultiModeGradientQueryMySQL.Rows[i]["gradient"]);
                    }

                    refreshChartLineCalibrationGradient();
                }

            }
        }

        //按照给定模式、段数修改模式，改变label显示、spin显示、按钮使能
        private void switchCalibrationMode(CalibrationMode mode, int countSection)
        {
            if(mode == CalibrationMode.singleSectionCalibration)
            {
                //单段模式
                this.labelControl_changeCalibrationMode.Text = "单段模式";
                //curCalibrationMode = CalibrationMode.singleSectionCalibration;
                //Global.curCalibrationSectionCount = countSection;
                this.spinEdit_countSection.Value = countSection;
                this.spinEdit_countSection.Properties.MinValue = 1;
                this.spinEdit_countSection.Enabled = false;
            }
            else if(mode == CalibrationMode.multiSectionCalibration)
            {
                //多段模式
                this.labelControl_changeCalibrationMode.Text = "多段模式";
                //curCalibrationMode = CalibrationMode.multiSectionCalibration;
                //Global.curCalibrationSectionCount = countSection;
                this.spinEdit_countSection.Value = countSection;
                if (countSection < 2)
                    return;
                this.spinEdit_countSection.Properties.MinValue = 2;
                this.spinEdit_countSection.Enabled = true;
            }


            //saveHasBeenCalibrated();

            changeButtonEnableSpinValueCompareSectionCount();
        }



        //模式发生改变时，保存被标定模式和段数到MySQL
        private void saveHasBeenCalibrated()
        {
            string cmdUpdateModeAndCountSection = String.Empty;
            if (curCalibrationMode == CalibrationMode.multiSectionCalibration)
            {
                
                cmdUpdateModeAndCountSection = "UPDATE mode_count_section SET `hasBeenCalibrated`=" + ((int)hasBeenCalibrated).ToString() + ", `countSection`=" + Global.curCalibrationSectionCount.ToString() + ";";
            }
            else if(curCalibrationMode == CalibrationMode.singleSectionCalibration)
            {
                cmdUpdateModeAndCountSection = "UPDATE mode_count_section SET `hasBeenCalibrated`=" + ((int)hasBeenCalibrated).ToString() + ";";
            }

            Global.mysqlHelper1._updateMySQL(cmdUpdateModeAndCountSection);
        }

        //根据给定的段数，给记录显示数据的dt清空重新分配空间，默认值为NaN
        private void allocateCapacityCalibrationData(int countSection)
        {
            clearChartLineCalibrationGradient();

            clearGridCalibrationGradient();

            Global.dtCalibrationDataSensorValAndWeight.Rows.Clear();
            //Global.dtCalibrationGradient.Rows.Clear();

            //端点数=段数+1
            for(int i = 0; i < countSection + 1; i++)
            {
                DataRow dr = Global.dtCalibrationDataSensorValAndWeight.NewRow();
                dr["NO"] = i + 1;
                dr["sensorValue"] = Double.NaN;
                dr["calibrationWeight"] = Double.NaN;
                Global.dtCalibrationDataSensorValAndWeight.Rows.Add(dr);
            }

            //Global.calibrationDataGradient = new double[countSection];
            for(int i = 0; i < countSection; i++)
            {
                DataRow dr = Global.dtCalibrationGradient.NewRow();
                dr["NO"] = i + 1;
                dr["section"] = (i + 1).ToString() + "-" + (i + 2).ToString();
                dr["gradient"] = Double.NaN;
                Global.dtCalibrationGradient.Rows.Add(dr);
            }

            isCalibrationDataModified = false;      //重新分配空间时被修改标志恢复
            isCalibrated = false;
            setButtonEnableWhenSensorValueOrWeightModified();
        }

        //计算斜率
        private void calcGradient()
        {
            Global.dtCalibrationGradient.Rows.Clear();
            double delta1 = 0.0D;
            double delta2 = 0.0D;
            for(int i = 0; i < Global.curCalibrationSectionCount; i++)
            {
                delta1 = Convert.ToDouble(Global.dtCalibrationDataSensorValAndWeight.Rows[i + 1]["calibrationWeight"]) - Convert.ToDouble(Global.dtCalibrationDataSensorValAndWeight.Rows[i]["calibrationWeight"]);
                delta2 = Convert.ToDouble(Global.dtCalibrationDataSensorValAndWeight.Rows[i + 1]["sensorValue"]) - Convert.ToDouble(Global.dtCalibrationDataSensorValAndWeight.Rows[i]["sensorValue"]);
                DataRow dr = Global.dtCalibrationGradient.NewRow();
                dr["NO"] = i + 1;
                dr["section"] = (i + 1).ToString() + "-" + (i + 2).ToString();
                dr["gradient"] = delta1 / delta2;
                Global.dtCalibrationGradient.Rows.Add(dr);
            }

            isCalibrationDataModified = true;       //数据被修改
            isCalibrated = true;                    //数据经过了标定
            refreshChartLineCalibrationGradient();
            setChartLineXYWholeRange(0.2);

        }

        //spinEdit.Value修改段数时的按钮使能设置
        //判断spinEdit值和实际段数是否相等的按钮使能逻辑。用以处理spin改变值在点击【修改段数】之前又恢复到原值时的情况。
        private void changeButtonEnableSpinValueCompareSectionCount()
        {
            if (Global.curCalibrationSectionCount == Convert.ToInt32(this.spinEdit_countSection.Value))
            {
                this.simpleButton_confirmCountSection.Enabled = false;
                this.simpleButton_changeCalibrationWeight.Enabled = true;
                this.simpleButton_doCalibrationAuto.Enabled = true;
                this.simpleButton_doCalibrationManual.Enabled = true;
            }
            else
            {
                this.simpleButton_confirmCountSection.Enabled = true;
                this.simpleButton_changeCalibrationWeight.Enabled = false;
                this.simpleButton_doCalibrationAuto.Enabled = false;
                this.simpleButton_doCalibrationManual.Enabled = false;
                this.simpleButton_saveDataChange.Enabled = false;
                this.simpleButton_cancelDataChange.Enabled = false;
            }
        }

        //修改标定点时的按钮使能设置
        private void setButtonEnableWhenSensorValueOrWeightModified()
        {
            if (isCalibrationDataModified)
            {
                this.simpleButton_cancelDataChange.Enabled = true;
                this.simpleButton_saveDataChange.Enabled = isCalibrated == true ? true : false;
            }
            else
            {
                this.simpleButton_cancelDataChange.Enabled = false;
                this.simpleButton_saveDataChange.Enabled = false;
            }
        }

        //标定点折线图不显示
        private void clearChartLineCalibrationGradient()
        {
            this.chartControl_calibrationGradient.Series[0].DataSource = null;
        }

        //刷新标定点折线图，显示新的斜率点
        private void refreshChartLineCalibrationGradient()
        {
            this.chartControl_calibrationGradient.Series[0].DataSource = Global.dtCalibrationDataSensorValAndWeight;

            double sensorValueMin = Convert.ToDouble(Global.dtCalibrationDataSensorValAndWeight.Rows[0]["sensorValue"]);
            double calibrationWeightMin = Convert.ToDouble(Global.dtCalibrationDataSensorValAndWeight.Rows[0]["calibrationWeight"]);
            double sensorValueMax = Convert.ToDouble(Global.dtCalibrationDataSensorValAndWeight.Rows[0]["sensorValue"]);
            double calibrationWeightMax = Convert.ToDouble(Global.dtCalibrationDataSensorValAndWeight.Rows[0]["calibrationWeight"]);

            for (int i = 1; i < Global.curCalibrationSectionCount + 1; i++)
            {
                if (Convert.ToDouble(Global.dtCalibrationDataSensorValAndWeight.Rows[i]["sensorValue"]) < sensorValueMin)
                {
                    sensorValueMin = Convert.ToDouble(Global.dtCalibrationDataSensorValAndWeight.Rows[i]["sensorValue"]);
                }
                else if (Convert.ToDouble(Global.dtCalibrationDataSensorValAndWeight.Rows[i]["sensorValue"]) > sensorValueMax)
                {
                    sensorValueMax = Convert.ToDouble(Global.dtCalibrationDataSensorValAndWeight.Rows[i]["sensorValue"]);
                }

                if (Convert.ToDouble(Global.dtCalibrationDataSensorValAndWeight.Rows[i]["calibrationWeight"]) < calibrationWeightMin)
                {
                    calibrationWeightMin = Convert.ToDouble(Global.dtCalibrationDataSensorValAndWeight.Rows[i]["calibrationWeight"]);
                }
                else if (Convert.ToDouble(Global.dtCalibrationDataSensorValAndWeight.Rows[i]["calibrationWeight"]) > calibrationWeightMax)
                {
                    calibrationWeightMax = Convert.ToDouble(Global.dtCalibrationDataSensorValAndWeight.Rows[i]["calibrationWeight"]);
                }
            }

            double deltaX = sensorValueMax - sensorValueMin;
            double deltaY = calibrationWeightMax - calibrationWeightMin;
            double k = 0.2;
            ((XYDiagram)(chartControl_calibrationGradient.Diagram)).AxisX.WholeRange.SetMinMaxValues(sensorValueMin - k * deltaX, sensorValueMax + k * deltaX);
            ((XYDiagram)(chartControl_calibrationGradient.Diagram)).AxisY.WholeRange.SetMinMaxValues(calibrationWeightMin - k * deltaY, calibrationWeightMax + k * deltaY);
        }

        //斜率列表不显示
        private void clearGridCalibrationGradient()
        {
            //this.gridControl_calibrationGradient.DataSource = null;
            Global.dtCalibrationGradient.Rows.Clear();
        }

        //创建数字小键盘对象
        private void createNumberKeyboard(string title, double min, double max)
        {
            if (this.numberKeyboard1 != null)
            {
                this.numberKeyboard1.Visible = false;
            }
            this.numberKeyboard1 = new CommonControl.NumberKeyboard(min, max);
            this.numberKeyboard1.Appearance.BackColor = System.Drawing.Color.White;
            this.numberKeyboard1.Appearance.Options.UseBackColor = true;
            this.numberKeyboard1.Location = new System.Drawing.Point(310, 3);
            this.numberKeyboard1.Name = "numberKeyboard1";
            this.numberKeyboard1.TabIndex = 28;
            this.numberKeyboard1.title = title;
            this.numberKeyboard1.outOfRangeType = CommonControl.NumberKeyboard.OutOfRangeType.minMaxIllegal;    //设定输入值取最值非法
            this.Controls.Add(this.numberKeyboard1);
            this.numberKeyboard1.BringToFront();
            this.numberKeyboard1.Visible = false;
            this.numberKeyboard1.NumberKeyboardEnterClicked += new CheckWeighterInterface.CommonControl.NumberKeyboard.SimpleButtonEnterClickHanlder(this.numberKeyboard1_NumberKeyboardEnterClicked);
        }

        //修改模式
        private void labelControl_changeCalibrationMode_Click(object sender, EventArgs e)
        {
            curCalibrationMode = curCalibrationMode == CalibrationMode.singleSectionCalibration ? CalibrationMode.multiSectionCalibration : CalibrationMode.singleSectionCalibration;
            if (curCalibrationMode == CalibrationMode.singleSectionCalibration)
            {
                //单段模式
                //switchCalibrationMode(CalibrationMode.singleSectionCalibration, 1);
                queryHasBeenCalibratedAndDataGradient(false);
            }
            else
            {
                //多段模式
                //switchCalibrationMode(CalibrationMode.multiSectionCalibration, Global.curCalibrationSectionCount);
                queryHasBeenCalibratedAndDataGradient(false);
            }
        }

        private void spinEdit_countCalibrationSection_ValueChanged(object sender, EventArgs e)
        {
            changeButtonEnableSpinValueCompareSectionCount();
        }

        //确认修改段数
        //修改段数时若段数同已标定的段数相同则显示已标定的数据，否则重新分配
        private void simpleButton_confirmCountSection_Click(object sender, EventArgs e)
        {
            Global.curCalibrationSectionCount = Convert.ToInt32(this.spinEdit_countSection.Value);
            if(Global.curCalibrationSectionCount == countSectionBeenMultiCalibrated)
            {
                queryHasBeenCalibratedAndDataGradient(false);
            }
            else
            {
                allocateCapacityCalibrationData(Global.curCalibrationSectionCount);
            }

            changeButtonEnableSpinValueCompareSectionCount();
        }

        //手动标定：修改传感器值+计算斜率
        private void simpleButton_changeSensorValue_Click(object sender, EventArgs e)
        {
            if (Global.dtCalibrationDataSensorValAndWeight.Rows.Count != 0)
            {
                curModifyValueType = ModifySensorValueOrCalibrationWeight.curModifySensorValue;
                createNumberKeyboard("输入传感器值", -999999.0D, 999999.0D);
                this.numberKeyboard1.Visible = true;
            }
        }

        //修改重量
        private void simpleButton_changeCalibrationWeight_Click(object sender, EventArgs e)
        {
            if (Global.dtCalibrationDataSensorValAndWeight.Rows.Count != 0)
            {
                curModifyValueType = ModifySensorValueOrCalibrationWeight.curModifyCalibrationWeight;
                createNumberKeyboard("输入传感器值", -999999.0D, 999999.0D);
                this.numberKeyboard1.Visible = true;
            }
        }

        //小键盘Enter响应
        private void numberKeyboard1_NumberKeyboardEnterClicked(object sender, EventArgs e)
        {
            //int selIndexTemp = selectRowCurrentGridControl[0];
            int selIndexTemp = this.tileView1.FocusedRowHandle;
            if (curModifyValueType == ModifySensorValueOrCalibrationWeight.curModifySensorValue)
            {
                Global.dtCalibrationDataSensorValAndWeight.Rows[selIndexTemp]["sensorValue"] = this.numberKeyboard1.result;    //从小键盘获取值作为传感器值进行修改
                calcGradient();
            }
            else if (curModifyValueType == ModifySensorValueOrCalibrationWeight.curModifyCalibrationWeight)
            {
                Global.dtCalibrationDataSensorValAndWeight.Rows[selIndexTemp]["calibrationWeight"] = this.numberKeyboard1.result;
                isCalibrationDataModified = true;       //数据被修改
                setChartLineXYWholeRange(0.2);
            }

            setButtonEnableWhenSensorValueOrWeightModified();
        }

        //自动标定。从下位机获取1个传感器值，将其存入dt，并计算斜率
        private void simpleButton_doCalibration_Click(object sender, EventArgs e)
        {
            int selIndexTemp = this.tileView1.FocusedRowHandle;
            Global.dtCalibrationDataSensorValAndWeight.Rows[selIndexTemp]["sensorValue"] = Global.curSensorValue;
            calcGradient();
            setButtonEnableWhenSensorValueOrWeightModified();
        }

        //将2个datatable中内容写入MySQL，修改hasBeenCalibrated标志
        private void simpleButton_saveCalibrationData_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < Global.dtCalibrationGradient.Rows.Count; i++)
            {
                if (Double.IsNaN(Convert.ToDouble(Global.dtCalibrationGradient.Rows[i]["gradient"])))
                {
                    MessageBox.Show("标定数据无效");
                    return;
                }
            }

            string cmdInsertSensorValueAndWeight = String.Empty;
            string cmdInsertCalibrationGradient = String.Empty;
            string cmdClearSensorValueAndWeight = String.Empty;
            string cmdClearCalibrationGradient = String.Empty;
            int NO = 0;
            string sensor_value = String.Empty;
            string calibrationWeight = String.Empty;
            string section = String.Empty;
            string gradient = String.Empty;
            bool flagSaveSucceed = true;

            if (curCalibrationMode == CalibrationMode.singleSectionCalibration)
            {
                //保存单段的数据
                cmdClearSensorValueAndWeight = "TRUNCATE TABLE single_mode_sensor_value_weight;";
                Global.mysqlHelper1._deleteMySQL(cmdClearSensorValueAndWeight);

                cmdInsertSensorValueAndWeight = "INSERT INTO single_mode_sensor_value_weight (`NO`, `sensorValue`, `calibrationWeight`) VALUES (1, " + Global.dtCalibrationDataSensorValAndWeight.Rows[0]["sensorValue"].ToString() + ", " + Global.dtCalibrationDataSensorValAndWeight.Rows[0]["calibrationWeight"].ToString() + ");";
                Global.mysqlHelper1._insertMySQL(cmdInsertSensorValueAndWeight);

                cmdInsertSensorValueAndWeight = "INSERT INTO single_mode_sensor_value_weight (`NO`, `sensorValue`, `calibrationWeight`) VALUES (2, " + Global.dtCalibrationDataSensorValAndWeight.Rows[1]["sensorValue"].ToString() + ", " + Global.dtCalibrationDataSensorValAndWeight.Rows[1]["calibrationWeight"].ToString() + ");";
                Global.mysqlHelper1._insertMySQL(cmdInsertSensorValueAndWeight);


                cmdClearSensorValueAndWeight = "TRUNCATE TABLE single_mode_gradient;";
                Global.mysqlHelper1._deleteMySQL(cmdClearSensorValueAndWeight);

                cmdInsertCalibrationGradient =  "INSERT INTO single_mode_gradient (`NO`,  `section`, `gradient`) VALUES (1, '1-2', " + Global.dtCalibrationGradient.Rows[0]["gradient"].ToString() + ");";
                flagSaveSucceed = Global.mysqlHelper1._insertMySQL(cmdInsertCalibrationGradient);

                if (flagSaveSucceed)
                {
                    isCalibrated = false;

                    MessageBox.Show("保存单段数据成功");
                    isCalibrationDataModified = false;
                    isCalibrated = false;

                    string cmdUpdateHasBeenCalibrated = String.Empty;
                    //保存已标定标志
                    switch (hasBeenCalibrated)
                    {
                        case HasBeenCalibrated.HasNotBeenCalibrated:
                            hasBeenCalibrated = HasBeenCalibrated.hasBeenSingleCalibrated;
                            cmdUpdateHasBeenCalibrated = "UPDATE TABALE mode_count_section SET `hasBeenCalibrated` = " + ((int)hasBeenCalibrated).ToString() + ";"; 
                            break;
                        case HasBeenCalibrated.hasBeenSingleCalibrated:
                            cmdUpdateHasBeenCalibrated = "UPDATE TABALE mode_count_section SET `hasBeenCalibrated` = " + ((int)hasBeenCalibrated).ToString() + ";";
                            break;
                        case HasBeenCalibrated.hasBeenMultiCalibrated:
                            hasBeenCalibrated = HasBeenCalibrated.hasBeenBothCalibrated;
                            cmdUpdateHasBeenCalibrated = "UPDATE TABALE mode_count_section SET `hasBeenCalibrated` = " + ((int)hasBeenCalibrated).ToString() + ", `countSection` = " + Global.curCalibrationSectionCount.ToString() + ";";
                            break;
                        case HasBeenCalibrated.hasBeenBothCalibrated:
                            cmdUpdateHasBeenCalibrated = "UPDATE TABALE mode_count_section SET `hasBeenCalibrated` = " + ((int)hasBeenCalibrated).ToString() + ", `countSection` = " + Global.curCalibrationSectionCount.ToString() + ";";
                            break;
                    }
                    Global.mysqlHelper1._updateMySQL(cmdUpdateHasBeenCalibrated);
                }
                else
                {
                    MessageBox.Show("保存单段数据失败");
                }
                
            }
            else
            {
                //保存多段数据
                cmdClearSensorValueAndWeight = "TRUNCATE TABLE multi_mode_sensor_value_weight;";
                Global.mysqlHelper1._deleteMySQL(cmdClearSensorValueAndWeight);

                for (int i = 0; i < Global.dtCalibrationDataSensorValAndWeight.Rows.Count; i++)
                {
                    NO = i + 1;
                    sensor_value = Global.dtCalibrationDataSensorValAndWeight.Rows[i]["sensorValue"].ToString();
                    calibrationWeight = Global.dtCalibrationDataSensorValAndWeight.Rows[i]["calibrationWeight"].ToString();
                    cmdInsertSensorValueAndWeight = "INSERT INTO multi_mode_sensor_value_weight (`NO`, `sensorValue`, `calibrationWeight`) VALUES (" + NO.ToString() + ", " + sensor_value + ", " + calibrationWeight + ");";
                    flagSaveSucceed = flagSaveSucceed && Global.mysqlHelper1._insertMySQL(cmdInsertSensorValueAndWeight);
                }

                cmdClearSensorValueAndWeight = "TRUNCATE TABLE multi_mode_gradient;";
                Global.mysqlHelper1._deleteMySQL(cmdClearSensorValueAndWeight);

                for (int i = 0;i< Global.dtCalibrationGradient.Rows.Count; i++)
                {
                    NO = i + 1;
                    section = NO.ToString() + "-" + (NO + 1).ToString();
                    gradient = Global.dtCalibrationGradient.Rows[i]["gradient"].ToString();
                    cmdInsertCalibrationGradient = "INSERT INTO multi_mode_gradient (`NO`,  `section`, `gradient`) VALUES (" + NO.ToString() + ", '" + section + "', " + gradient + ");"; 
                    flagSaveSucceed = flagSaveSucceed && Global.mysqlHelper1._insertMySQL(cmdInsertCalibrationGradient);
                }

                if (flagSaveSucceed)
                {
                    MessageBox.Show("保存多段数据成功");
                    isCalibrationDataModified = false;
                    isCalibrated = false;

                    countSectionBeenMultiCalibrated = Global.curCalibrationSectionCount;

                    string cmdUpdateHasBeenCalibrated = String.Empty;
                    //保存已标定标志
                    switch (hasBeenCalibrated)
                    {
                        case HasBeenCalibrated.HasNotBeenCalibrated:
                            hasBeenCalibrated = HasBeenCalibrated.hasBeenMultiCalibrated;
                            cmdUpdateHasBeenCalibrated = "UPDATE TABALE mode_count_section SET `hasBeenCalibrated` = " + ((int)hasBeenCalibrated).ToString() + ", `countSection` = " + countSectionBeenMultiCalibrated.ToString() + ";";
                            break;
                        case HasBeenCalibrated.hasBeenSingleCalibrated:
                            hasBeenCalibrated = HasBeenCalibrated.hasBeenBothCalibrated;
                            cmdUpdateHasBeenCalibrated = "UPDATE TABALE mode_count_section SET `hasBeenCalibrated` = " + ((int)hasBeenCalibrated).ToString() + ", `countSection` = " + countSectionBeenMultiCalibrated.ToString() + ";";
                            break;
                        case HasBeenCalibrated.hasBeenMultiCalibrated:
                            hasBeenCalibrated = HasBeenCalibrated.hasBeenMultiCalibrated;
                            cmdUpdateHasBeenCalibrated = "UPDATE TABALE mode_count_section SET `hasBeenCalibrated` = " + ((int)hasBeenCalibrated).ToString() + ", `countSection` = " + countSectionBeenMultiCalibrated.ToString() + ";";
                            break;
                        case HasBeenCalibrated.hasBeenBothCalibrated:
                            hasBeenCalibrated = HasBeenCalibrated.hasBeenBothCalibrated;
                            cmdUpdateHasBeenCalibrated = "UPDATE TABALE mode_count_section SET `hasBeenCalibrated` = " + ((int)hasBeenCalibrated).ToString() + ", `countSection` = " + countSectionBeenMultiCalibrated.ToString() + ";";
                            break;
                    }
                    Global.mysqlHelper1._updateMySQL(cmdUpdateHasBeenCalibrated);
                }
                else
                {
                    MessageBox.Show("保存多段数据失败");
                }
            }

            saveHasBeenCalibrated();

            setButtonEnableWhenSensorValueOrWeightModified();
        }

        //丢弃已修改的数据，已MySQL暂存表数据还原
        private void simpleButton_cancelCalibrationData_Click(object sender, EventArgs e)
        {
            if(hasBeenCalibrated == HasBeenCalibrated.HasNotBeenCalibrated)
            {
                if(curCalibrationMode == CalibrationMode.singleSectionCalibration)
                {
                    allocateCapacityCalibrationData(1);
                }
                else
                {
                    allocateCapacityCalibrationData(Global.curCalibrationSectionCount);
                }
            }
            else if(hasBeenCalibrated == HasBeenCalibrated.hasBeenSingleCalibrated)
            {
                if (curCalibrationMode == CalibrationMode.singleSectionCalibration)
                {
                    this.gridControl_calibrationDataList.BeginUpdate();

                    for(int i=0;i< dtSingleModeCalibrationDataQueryMySQL.Rows.Count; i++)
                    {
                        Global.dtCalibrationDataSensorValAndWeight.Rows[i]["NO"] = Convert.ToInt16(dtSingleModeCalibrationDataQueryMySQL.Rows[i]["NO"]);
                        Global.dtCalibrationDataSensorValAndWeight.Rows[i]["sensorValue"] = Convert.ToDouble(dtSingleModeCalibrationDataQueryMySQL.Rows[i]["sensorValue"]);
                        Global.dtCalibrationDataSensorValAndWeight.Rows[i]["calibrationWeight"] = Convert.ToDouble(dtSingleModeCalibrationDataQueryMySQL.Rows[i]["calibrationWeight"]);
                    }

                    this.gridControl_calibrationDataList.DataSource = Global.dtCalibrationDataSensorValAndWeight;   //若不重新给DataSource赋值，则grid仍显示dt变化之前的值（绑定时拷贝了一个副本？副本未变？）
                    this.gridControl_calibrationDataList.EndUpdate();


                    this.gridControl_calibrationGradient.BeginUpdate();

                    for (int i = 0; i < dtSingleModeGradientQueryMySQL.Rows.Count; i++)
                    {
                        Global.dtCalibrationGradient.Rows[i]["NO"] = Convert.ToInt16(dtSingleModeGradientQueryMySQL.Rows[i]["NO"]);
                        Global.dtCalibrationGradient.Rows[i]["section"] = dtSingleModeGradientQueryMySQL.Rows[i]["section"].ToString();
                        Global.dtCalibrationGradient.Rows[i]["gradient"] = Convert.ToDouble(dtSingleModeGradientQueryMySQL.Rows[i]["gradient"]);
                    }

                    this.gridControl_calibrationGradient.DataSource = Global.dtCalibrationGradient;
                    this.gridControl_calibrationGradient.EndUpdate();
                }
                else
                {
                    allocateCapacityCalibrationData(Global.curCalibrationSectionCount);
                }
            }
            else if(hasBeenCalibrated == HasBeenCalibrated.hasBeenMultiCalibrated)
            {
                if (curCalibrationMode == CalibrationMode.singleSectionCalibration)
                {
                    allocateCapacityCalibrationData(1);
                }
                else
                {
                    this.gridControl_calibrationDataList.BeginUpdate();

                    for (int i = 0; i < dtMultiModeCalibrationDataQueryMySQL.Rows.Count; i++)
                    {
                        Global.dtCalibrationDataSensorValAndWeight.Rows[i]["NO"] = Convert.ToInt16(dtMultiModeCalibrationDataQueryMySQL.Rows[i]["NO"]);
                        Global.dtCalibrationDataSensorValAndWeight.Rows[i]["sensorValue"] = Convert.ToDouble(dtMultiModeCalibrationDataQueryMySQL.Rows[i]["sensorValue"]);
                        Global.dtCalibrationDataSensorValAndWeight.Rows[i]["calibrationWeight"] = Convert.ToDouble(dtMultiModeCalibrationDataQueryMySQL.Rows[i]["calibrationWeight"]);
                    }

                    this.gridControl_calibrationDataList.DataSource = Global.dtCalibrationDataSensorValAndWeight;
                    this.gridControl_calibrationDataList.EndUpdate();


                    this.gridControl_calibrationGradient.BeginUpdate();

                    for (int i = 0; i < dtMultiModeGradientQueryMySQL.Rows.Count; i++)
                    {
                        Global.dtCalibrationGradient.Rows[i]["NO"] = Convert.ToInt16(dtMultiModeGradientQueryMySQL.Rows[i]["NO"]);
                        Global.dtCalibrationGradient.Rows[i]["section"] = dtMultiModeGradientQueryMySQL.Rows[i]["section"].ToString();
                        Global.dtCalibrationGradient.Rows[i]["gradient"] = Convert.ToDouble(dtMultiModeGradientQueryMySQL.Rows[i]["gradient"]);
                    }
                    this.gridControl_calibrationGradient.DataSource = Global.dtCalibrationGradient;
                    this.gridControl_calibrationGradient.EndUpdate();
                }
            }
            else if(hasBeenCalibrated == HasBeenCalibrated.hasBeenBothCalibrated)
            {
                if (curCalibrationMode == CalibrationMode.singleSectionCalibration)
                {
                    this.gridControl_calibrationDataList.BeginUpdate();

                    for (int i = 0; i < dtSingleModeCalibrationDataQueryMySQL.Rows.Count; i++)
                    {
                        Global.dtCalibrationDataSensorValAndWeight.Rows[i]["NO"] = Convert.ToInt16(dtSingleModeCalibrationDataQueryMySQL.Rows[i]["NO"]);
                        Global.dtCalibrationDataSensorValAndWeight.Rows[i]["sensorValue"] = Convert.ToDouble(dtSingleModeCalibrationDataQueryMySQL.Rows[i]["sensorValue"]);
                        Global.dtCalibrationDataSensorValAndWeight.Rows[i]["calibrationWeight"] = Convert.ToDouble(dtSingleModeCalibrationDataQueryMySQL.Rows[i]["calibrationWeight"]);
                    }

                    this.gridControl_calibrationDataList.DataSource = Global.dtCalibrationDataSensorValAndWeight;
                    this.gridControl_calibrationDataList.EndUpdate();


                    this.gridControl_calibrationGradient.BeginUpdate();

                    for (int i = 0; i < dtSingleModeGradientQueryMySQL.Rows.Count; i++)
                    {
                        Global.dtCalibrationGradient.Rows[i]["NO"] = Convert.ToInt16(dtSingleModeGradientQueryMySQL.Rows[i]["NO"]);
                        Global.dtCalibrationGradient.Rows[i]["section"] = dtSingleModeGradientQueryMySQL.Rows[i]["section"].ToString();
                        Global.dtCalibrationGradient.Rows[i]["gradient"] = Convert.ToDouble(dtSingleModeGradientQueryMySQL.Rows[i]["gradient"]);
                    }

                    this.gridControl_calibrationGradient.DataSource = Global.dtCalibrationGradient;
                    this.gridControl_calibrationGradient.EndUpdate();
                }
                else
                {
                    this.gridControl_calibrationDataList.BeginUpdate();

                    for (int i = 0; i < dtMultiModeCalibrationDataQueryMySQL.Rows.Count; i++)
                    {
                        Global.dtCalibrationDataSensorValAndWeight.Rows[i]["NO"] = Convert.ToInt16(dtMultiModeCalibrationDataQueryMySQL.Rows[i]["NO"]);
                        Global.dtCalibrationDataSensorValAndWeight.Rows[i]["sensorValue"] = Convert.ToDouble(dtMultiModeCalibrationDataQueryMySQL.Rows[i]["sensorValue"]);
                        Global.dtCalibrationDataSensorValAndWeight.Rows[i]["calibrationWeight"] = Convert.ToDouble(dtMultiModeCalibrationDataQueryMySQL.Rows[i]["calibrationWeight"]);
                    }

                    this.gridControl_calibrationDataList.DataSource = Global.dtCalibrationDataSensorValAndWeight;
                    this.gridControl_calibrationDataList.EndUpdate();


                    this.gridControl_calibrationGradient.BeginUpdate();

                    for (int i = 0; i < dtMultiModeGradientQueryMySQL.Rows.Count; i++)
                    {
                        Global.dtCalibrationGradient.Rows[i]["NO"] = Convert.ToInt16(dtMultiModeGradientQueryMySQL.Rows[i]["NO"]);
                        Global.dtCalibrationGradient.Rows[i]["section"] = dtMultiModeGradientQueryMySQL.Rows[i]["section"].ToString();
                        Global.dtCalibrationGradient.Rows[i]["gradient"] = Convert.ToDouble(dtMultiModeGradientQueryMySQL.Rows[i]["gradient"]);
                    }

                    this.gridControl_calibrationGradient.DataSource = Global.dtCalibrationGradient;
                    this.gridControl_calibrationGradient.EndUpdate();
                }
            }

            isCalibrationDataModified = false;
            isCalibrated = false;
            setButtonEnableWhenSensorValueOrWeightModified();
            setChartLineXYWholeRange(0.2);
            MessageBox.Show("修改已丢弃");
        }


    }
}
