using DevExpress.XtraCharts;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckWeighterInterface.StatusMonitor
{
    public partial class StatusMonitor : DevExpress.XtraEditors.XtraUserControl
    {
        private double lastOverWeght = 0.0D;
        private double lastUnderWeight = 0.0D;

        //创建字典(weightAndIndexGramDtPoint),包含重量数值及行号;
        private Dictionary<int, int> weightAndIndexGramDtPoint = new Dictionary<int, int>();    //(weight(重量数值), totalDtPoint(行号))；
        private int totalDtPoint = 0;       //dtPoint中存的点的个数（行数，重量不同才会增加一行）
        //散点图横轴坐标边缘
        private Int32[] minXRangePoint = { 0, 1000, 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000, 10000, 11000, 12000, 13000, 14000, 15000 };
        private Int32[] maxXRangePoint = { 0, 1000, 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000, 10000, 11000, 12000, 13000, 14000, 15000 };
        private Int32 minXPoint = 0;    //横轴范围左侧起始值；
        private Int32 maxXPoint = 0;   //横轴范围右侧终止值；

        //散点图坐标轴取值范围
        public Int32 minPointXLeft = 9;//默认横坐标起始值是9000；[9--9000];
        public Int32 minPointXRight = 13;//默认横坐标终止值是13000；[13--13000];

        //散点图横轴分辨率档位
        private Int32[] intervalAxisXPoint = {10, 20, 50, 100};     //散点图横轴分辨率（若选10X档位时，9000-13000间共有400个格子，每个格子代表10）；
        private Int32 gearIntervalAxisXPoint = 0;                  //散点图横轴档位[0-10;1-20;2-50;3-100];

        public bool getTimerDetectOnceEnable()
        {
            return this.timer_detectOnce.Enabled;   //default:True;
        }

        public void setTimerDetectOnceEnable(bool val)
        {
            this.timer_detectOnce.Enabled = val;    
        }

        public StatusMonitor()
        {
            InitializeComponent();  //初始化组件;
            initStatusMonitor();    //初始化工作状态；
            SystemManagement.BrandManagement.brandChangedReInitStatusMonitor += reInitStatusMonitor;
        }
        
        //当前品牌发生改变时，刷新页面函数
        private void reInitStatusMonitor(object sender, EventArgs e)
        {
            Global.mysqlHelper1._updateMySQL("TRUNCATE TABLE weight_history;");     //清空原先品牌的重量历史
            weightAndIndexGramDtPoint.Clear();                                      //清空字典
            totalDtPoint = 0;

            if (Global.curStatus.curBrand != "")    //判断当前状态品牌是否为空；
            {
                timer_detectOnce.Enabled = true;    //定时器开启；

                Global.curStatus.countDetection = 0;    //当前状态检测总数量；
                Global.curStatus.countOverWeight = 0;   //当前状态超重数量；
                Global.curStatus.countUnderWeight = 0;  //当前状态欠重数量；
                Global.curStatus.lastOverWeight = 0.0D; //当前状态上次超重数量；
                Global.curStatus.lastUnderWeight = 0.0D;    //当前状态上次欠重数量；
                Global.curStatus.maxWeightInHistory = 0.0D; //当前状态历史数据中最大重量数；
                Global.curStatus.minWeightInHistory = 20.0D;    //当前状态历史数据中最小重量数；

                //清空--initDatatable();
                Global.dtLineStatusMonitor.Rows.Clear();    //清空折线图；
                Global.dtPieStatusMonitor.Rows.Clear(); //清空饼状图；
                Global.dtPointStatusMonitor.Rows.Clear();   //清空散点图；

                initDatatable();    //初始化datatable;
                bindLineData(); //绑定折线图数据；
                bindPieData();  //绑定饼状图数据；

                setAxisXMinMaxPoint(minPointXLeft, minPointXRight);  //设定散点图横轴区间范围
                setGearIntervalAxisXPoint(0);   //设定散点图横轴分辨率
                bindPointData();    //绑定散点图数据；
                labelControl_status.Parent = this.chartControl_line;    //实时显示当前状态（NG or OK）（折线图右上角）;
                labelControl_curWeightVal.Parent = this.chartControl_line;  //实时显示当前重量（折线图右上角）；

                //MessageBox.Show("refresh");
            }
            else
            {
                timer_detectOnce.Enabled = false;//定时器关闭；
            }
        }

        //初始化StatusMonitor
        private void initStatusMonitor()
        {
            //数据库中无品牌时不刷新实时图表
            string cmdInitDtBrand = "SELECT * FROM brand;"; //查询数据库中brand;
            Global.mysqlHelper1._queryTableMySQL(cmdInitDtBrand, ref Global.dtBrand);   //将查询后的数据放在dtBrand中；
            if (Global.dtBrand.Rows.Count == 0) //判断dtBrand表不为空，不可使用dtBrand.Cols.Count(当列为空时，可使用);
            {
                timer_detectOnce.Enabled = false;   //定时器关闭；
            }
            else
            {
                timer_detectOnce.Enabled = true;    //定时器开启；

                Global.curStatus.curBrand = Global.dtBrand.Rows[0]["name"].ToString();  //将0行name转换为字符串给curBrand；
                Global.curStatus.countDetection = 0;
                Global.curStatus.countOverWeight = 0;
                Global.curStatus.countUnderWeight = 0;
                Global.curStatus.lastOverWeight = 0.0D;
                Global.curStatus.lastUnderWeight = 0.0D;
                Global.curStatus.maxWeightInHistory = 0.0D;     //大于0更新（同时比较传入数据大小）；
                Global.curStatus.minWeightInHistory = 20.0D;    //小于20更新（同时比较传入数据大小）；

                Global.overWeightThreshold = Convert.ToDouble(Global.dtBrand.Rows[0]["upperLimit"]);    //初始化重量上限阈值（0行及对应阈值的上限的列）
                Global.underWeightThreshold = Convert.ToDouble(Global.dtBrand.Rows[0]["lowerLimit"]);   //初始化重量下限阈值（0行及对应阈值的下限的列）
                //至此--工作状态左侧panelcontrol全部初始化完成；

                initDatatable();  //初始化Datatable,添加列；
                bindLineData(); //绑定折线图数据源；
                bindPieData();  //绑定饼状图数据源；

                setAxisXMinMaxPoint(minPointXLeft, minPointXRight);  //设定散点图横轴区间范围(minPointXLeft = 9; minPointXRight = 13);
                setGearIntervalAxisXPoint(0);   //设定散点图横轴分辨率;
                bindPointData();    //绑定散点图数据源；
                labelControl_status.Parent = this.chartControl_line;    //实时显示当前状态（NG or OK）（折线图右上角）;
                labelControl_curWeightVal.Parent = this.chartControl_line;  //实时显示当前重量（折线图右上角）；
            }
        }

        //刷新当前页面
        public void refreshStatusMonitor(object sender, EventArgs e)    //定时器-timer_detectOnce-周期性刷新页面；
        {
            if (Global.enableRefreshStatusMonitor)
            {
                labelControl_brandVal.Text = Global.curStatus.curBrand; //刷新当前品牌(左上角)；
                if (Global.curStatus.curBrand != "")    //当前品牌非空，进行刷新；
                {
                    //timer_detectOnce刷新页面；
                    getCurWeight();                             //获取当前重量和显示
                    updateMinWeightAndMaxWeight();              //刷新最值和显示
                    updateLastOverWeightAndLastUnderWeight();   //刷新欠重/超重的重量、数值和显示
                    updateChartLineData();                      //刷新折线图
                    updateChartPieData();                       //刷新饼图
                    updateChartPointData();                     //刷新点图
                    insertCurStatusIntoMySQL();                 //插入MySQL
                }
                else    //当前品牌为空，进行赋初始值；
                {
                    Global.dtLineStatusMonitor.Rows.Clear();
                    Global.dtPieStatusMonitor.Rows.Clear();
                    Global.dtPointStatusMonitor.Rows.Clear();
                    labelControl_lastOverWeightVal.Text = "";
                    labelControl_lastUnderWeightVal.Text = "";
                    labelControl_detectionCountVal.Text = "0";
                    labelControl_overWeightCountVal.Text = "";
                    labelControl_underWeightCountVal.Text = "";
                    labelControl_maxWeightInHistory.Text = "";
                    labelControl_minWeightInHistory.Text = "";
                    labelControl_curWeightVal.Text = "";
                    labelControl_status.Text = "";
                }
            }
        }

        //初始化数据源DataTable
        private void initDatatable()
        {
            //向折线图中添加一次称重数据；
            if (Global.dtLineStatusMonitor.Columns.Count == 0)
            {
                Global.dtLineStatusMonitor.Columns.Add("countDetection", typeof(Int32));        //称重计数（折线图横坐标：称重数量）；
                Global.dtLineStatusMonitor.Columns.Add("currentWeight", typeof(double));        //当前重量（折线图纵坐标：当前重量）；
            }

            //向饼状图中添加一次称重数据；
            if (Global.dtPieStatusMonitor.Columns.Count == 0)
            {
                Global.dtPieStatusMonitor.Columns.Add("status", typeof(String));   //状态：欠重、超重、正常
                Global.dtPieStatusMonitor.Columns.Add("countCur", typeof(Int32));  //状态(计数)：欠重、超重、正常
            }

            //向散点图中添加一次称重数据；
            if (Global.dtPointStatusMonitor.Columns.Count == 0)
            {
                Global.dtPointStatusMonitor.Columns.Add("weightSection", typeof(Int32));        //重量区间
                Global.dtPointStatusMonitor.Columns.Add("countInSection", typeof(Int32));       //计数区间
            }
        }

        //绑定折线图数据源，以及图表相关设置
        private void bindLineData()
        {
            this.chartControl_line.Series[0].DataSource = Global.dtLineStatusMonitor;      //绑定Datatable
            this.chartControl_line.Series[0].ArgumentScaleType = ScaleType.Numerical;   //设定Argument的类型
            this.chartControl_line.Series[0].ArgumentDataMember = "countDetection";       //设定Argument的字段名
            this.chartControl_line.Series[0].ValueScaleType = ScaleType.Numerical;  //设定Value的类型
            this.chartControl_line.Series[0].ValueDataMembers.AddRange(new string[] { "currentWeight" });   
        }

        //绑定饼图数据源，以及图表相关设置
        private void bindPieData()
        {
            this.chartControl_pie.Series[0].DataSource = Global.dtPieStatusMonitor;
            this.chartControl_pie.Series[0].ArgumentDataMember = "status";
            this.chartControl_pie.Series[0].ArgumentScaleType = ScaleType.Auto;
            this.chartControl_pie.Series[0].ValueDataMembers.AddRange(new string[] { "countCur" });
            this.chartControl_pie.Series[0].ValueScaleType = ScaleType.Numerical;
            this.chartControl_pie.Series[0].LegendTextPattern = "{A}：{VP:p2}";  //图例格式"Argument:Value"。V:n2为Value:numeric小数点后2位，VP:p2为Value:percent小数点后2位
        }

        //绑定点图数据源，以及图表相关设置
        private void bindPointData()
        {
            this.chartControl_point.Series[0].DataSource = Global.dtPointStatusMonitor;
            this.chartControl_point.Series[0].ArgumentScaleType = ScaleType.Auto;
            this.chartControl_point.Series[0].ArgumentDataMember = "weightSection";
            this.chartControl_point.Series[0].ValueScaleType = ScaleType.Numerical;
            this.chartControl_point.Series[0].ValueDataMembers.AddRange(new string[] { "countInSection" });
        }

        //获取当前重量和H-/L-/p-状态
        private void getCurWeight()//获取当前重量；
        {
            //数据生成
            Global.curStatus.countDetection++;//对当前状态下检测总数量进行计数；
            Random rnd = new Random();
            //double cw = rnd.Next(8, 12) + rnd.Next(0, 9) * 0.1 + rnd.Next(0, 9) * 0.01 + rnd.Next(0, 9) * 0.001;
            //createRandomProbability(90, 8, 12, 13)---90%的概率出现在[8,12];10%的概率出现在[12,13];
            double cw = createRandomProbability(90, 8, 12, 13) + rnd.Next(0, 9) * 0.1 + rnd.Next(0, 9) * 0.01 + rnd.Next(0, 9) * 0.001;//随机生成一个带三位小数随机数

            //更新当前重量、超重/欠重/正常标志
            Global.curStatus.curWeight = cw;//将cw(重量值)赋值给Global.curStatus.curWeight
            if (Global.curStatus.curWeight > Global.overWeightThreshold)//判断当前重量是否大于数据库brand中的重量上限阈值；
                Global.curStatus.flagOverWeightOrUnderWeight = "H-";
            else if (Global.curStatus.curWeight < Global.underWeightThreshold)//判断当前重量是否小于数据库brand中的重量下限阈值；
                Global.curStatus.flagOverWeightOrUnderWeight = "L-";
            else
                Global.curStatus.flagOverWeightOrUnderWeight = "p-";
            
            labelControl_curWeightVal.Text = Global.curStatus.curWeight.ToString() + "KG";          //刷新重量显示
            labelControl_detectionCountVal.Text = Global.curStatus.countDetection.ToString();       //刷新检测数量
        }

        //刷新最值
        private void updateMinWeightAndMaxWeight()
        {
            //刷新最大值
            if (Global.curStatus.curWeight > Global.curStatus.maxWeightInHistory)//maxWeightInHistory=0;
            {
                Global.curStatus.maxWeightInHistory = Global.curStatus.curWeight;
                labelControl_maxWeightInHistory.Text = Global.curStatus.maxWeightInHistory != 0.0D ? Global.curStatus.maxWeightInHistory.ToString() : "";//判断最大值是否大于0，若大于0，则将其转为字符串，在text中显示，否则不显示；
            }
            //刷新最小值
            if (Global.curStatus.curWeight < Global.curStatus.minWeightInHistory)//minWeightInHistory=20;
            {
                Global.curStatus.minWeightInHistory = Global.curStatus.curWeight;
                labelControl_minWeightInHistory.Text = Global.curStatus.minWeightInHistory != 20.0D ? Global.curStatus.minWeightInHistory.ToString() : "";//判断最小值是否小于20，若小于20，则将其转为字符串，在text中显示，否则不显示；
            }
        }

        //刷新上次欠重、上次超重
        private void updateLastOverWeightAndLastUnderWeight()
        {
            //超重、欠重
            if (Global.curStatus.flagOverWeightOrUnderWeight == "H-")
            {
                Global.curStatus.countOverWeight++;//对当前工作状态下超重数量进行计数；
                labelControl_status.Text = "NG";
                this.labelControl_status.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(49)))), ((int)(((byte)(68)))));//设置前景颜色；

                Global.curStatus.lastOverWeight = lastOverWeght;    //超重、欠重都要更新，否则出现类似场景：第一次是超重，后面一直是欠重，则超重一直得不到刷新；将上次超重重量0赋值给 Global.curStatus.lastOverWeight
                Global.curStatus.lastUnderWeight = lastUnderWeight; //将上次欠重重量0赋值给 Global.curStatus.lastUnderWeight ；

                lastOverWeght = Global.curStatus.curWeight;//把当前状态下随机生成的当前重量赋值给 lastOverWeight；
            }
            else if (Global.curStatus.flagOverWeightOrUnderWeight == "L-")
            {
                Global.curStatus.countUnderWeight++;//对当前工作状态下欠重数量进行计数；
                labelControl_status.Text = "NG";
                this.labelControl_status.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(49)))), ((int)(((byte)(68)))));

                Global.curStatus.lastOverWeight = lastOverWeght;
                Global.curStatus.lastUnderWeight = lastUnderWeight;

                lastUnderWeight = Global.curStatus.curWeight;
            }
            else if (Global.curStatus.flagOverWeightOrUnderWeight == "p-")
            {
                Global.curStatus.lastOverWeight = lastOverWeght;
                Global.curStatus.lastUnderWeight = lastUnderWeight;

                labelControl_status.Text = "OK";
                this.labelControl_status.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(152)))), ((int)(((byte)(83)))));
            }

            labelControl_underWeightCountVal.Text = Global.curStatus.countUnderWeight.ToString();//更新欠重数量；
            labelControl_lastUnderWeightVal.Text = Global.curStatus.lastUnderWeight > 0.0D ? Global.curStatus.lastUnderWeight.ToString() : "";//更新欠重重量；
            labelControl_overWeightCountVal.Text = Global.curStatus.countOverWeight.ToString();//更新超重数量；
            labelControl_lastOverWeightVal.Text = Global.curStatus.lastOverWeight > 0.0D ? Global.curStatus.lastOverWeight.ToString() : "";//更新超重重量；
        }

        //刷新折线图数据源
        public void updateChartLineData()
        {
            //点总数未到200时直接添加，超过200时添加一个点：删掉原有第1行，在最后添加一行
            if(Global.curStatus.countDetection <= 200)
            {
                DataRow drCurWeight = Global.dtLineStatusMonitor.NewRow();
                drCurWeight["countDetection"] = Global.curStatus.countDetection;
                drCurWeight["currentWeight"] = Global.curStatus.curWeight;
                Global.dtLineStatusMonitor.Rows.Add(drCurWeight);
            }
            else
            {
                Global.dtLineStatusMonitor.Rows.RemoveAt(0);//删除第一行；
                DataRow drCurWeight = Global.dtLineStatusMonitor.NewRow();
                drCurWeight["countDetection"] = Global.curStatus.countDetection;
                drCurWeight["currentWeight"] = Global.curStatus.curWeight;
                Global.dtLineStatusMonitor.Rows.Add(drCurWeight);

                for(int i = 0; i < 200; i++)//通过遍历改变当前称重数量区间[0,200];
                {
                    Global.dtLineStatusMonitor.Rows[i]["countDetection"] = (i + 1).ToString();//将横坐标称重数量移动至[0,200];
                }
            }
        }

        //刷新Pie图数据源，Pie图绑定数值，自动显示占比
        public void updateChartPieData()
        {
            if (Global.dtPieStatusMonitor.Rows.Count == 0)
            {
                DataRow drCountUnderWeight = Global.dtPieStatusMonitor.NewRow();
                drCountUnderWeight["status"] = "欠重";
                drCountUnderWeight["countCur"] = Global.curStatus.countUnderWeight;
                Global.dtPieStatusMonitor.Rows.Add(drCountUnderWeight);

                DataRow drCountOverWeight = Global.dtPieStatusMonitor.NewRow();
                drCountOverWeight["status"] = "超重";
                drCountOverWeight["countCur"] = Global.curStatus.countOverWeight;
                Global.dtPieStatusMonitor.Rows.Add(drCountOverWeight);

                DataRow drCountNormal = Global.dtPieStatusMonitor.NewRow();
                drCountNormal["status"] = "正常";
                drCountNormal["countCur"] = Global.curStatus.countDetection - Global.curStatus.countOverWeight - Global.curStatus.countUnderWeight;
                Global.dtPieStatusMonitor.Rows.Add(drCountNormal);
            }
            else
            {
                Global.dtPieStatusMonitor.Rows[0]["countCur"] = Global.curStatus.countUnderWeight;
                Global.dtPieStatusMonitor.Rows[1]["countCur"] = Global.curStatus.countOverWeight;
                Global.dtPieStatusMonitor.Rows[2]["countCur"] = Global.curStatus.countDetection - Global.curStatus.countOverWeight - Global.curStatus.countUnderWeight;
            }
        }

        //刷新Point图数据源
        public void updateChartPointData()
        {
            int weightGram = weightIntervalProcess(Global.curStatus.curWeight);//对当前获取的重量进行处理；
            //int weightGram = Convert.ToInt32(Global.curStatus.curWeight);
            if (weightAndIndexGramDtPoint.ContainsKey(weightGram) == false)//判断当前重量是否第一次出现于字典中（weightAndIndexGramDtPoint）；
            {
                DataRow dr = Global.dtPointStatusMonitor.NewRow();//创建新的行；
                dr["weightSection"] = weightGram;//将当前重量写入新行中；
                dr["countInSection"] = 1;//对当前重量的个数计数为1；
                Global.dtPointStatusMonitor.Rows.Add(dr);//将当前重量及出现个数均写进表（datatable）里；
                weightAndIndexGramDtPoint.Add(weightGram, totalDtPoint);////将当前重量及出现个数均写进字典（dictionary）里,totalDtPoint表示行（row）序号；
                totalDtPoint++;//记录字典中行号；
            }
            else
            {
                int indexDtPointTemp = 0;//datatable中记录当前非首次出现重量的行号；
                indexDtPointTemp = weightAndIndexGramDtPoint[weightGram];//利用字典中的键（key）取值(行序号)（value）;
                Global.dtPointStatusMonitor.Rows[indexDtPointTemp]["countInSection"] = Convert.ToInt32(Global.dtPointStatusMonitor.Rows[indexDtPointTemp]["countInSection"]) + 1;//将datatable中当前重量值出现得次数加1；
            }
        }

        //设置散点图横轴坐标范围
        private void setAxisXMinMaxPoint(int minKilogram, int maxKilogram)
        {
            if (minKilogram < maxKilogram)
            {
                minXPoint = minXRangePoint[minKilogram];
                maxXPoint = maxXRangePoint[maxKilogram];
                XYDiagram diagram = (XYDiagram)this.chartControl_point.Diagram;//设置坐标轴范围（固定用法）；
                diagram.AxisX.WholeRange.SetMinMaxValues(minXPoint, maxXPoint);//设置坐标轴范围（固定用法）；
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

        //将重量（KG)按照区间划归
        private int weightIntervalProcess(double weightKilogram)
        {
            //小于minXPoint的设为minXPoint，大于maxXPoint的设为maxXPoint
            int weightgram = Convert.ToInt32(weightKilogram * 1000);
            int weight = 0;
            if (weightgram <= minXPoint)
            {
                weight = minXPoint;
            }
            else if(weightgram >= maxXPoint)
            {
                weight = maxXPoint;
            }
            else
            {
                int countInterval = (weightgram - minXPoint) / intervalAxisXPoint[gearIntervalAxisXPoint];//(minpoint最小点重量->当前值的间隔个数=（当前重量-最小重量）/散点图分辨率（默认索引0：分辨率：10）)；
                int remainder = (weightgram - minXPoint) % intervalAxisXPoint[gearIntervalAxisXPoint];//对当前传入的重量取余，大小小于当前档位（默认索引0：分辨率：10），单位：g;(对取余的重量进行四舍五入)
                int intervalDeviceTwo = intervalAxisXPoint[gearIntervalAxisXPoint] / 2;//获取当前（数组）分辨率的中间值；和上一步取余值进行比较；
                if (remainder < intervalDeviceTwo)
                {
                    weight = minXPoint + countInterval * intervalAxisXPoint[gearIntervalAxisXPoint];//计算当前重量；
                }
                else
                {
                    weight = minXPoint + (1 + countInterval) * intervalAxisXPoint[gearIntervalAxisXPoint];//计算当前重量；
                }
            }

            return weight;//返回当前重量值；
        }

        //将当前重量状态的原始数据插入MySQL
        private void insertCurStatusIntoMySQL()//插入数据库，把当前重量数据往数据库里写；
        {
            Thread threadInsertMySQL = new Thread(new ThreadStart(Global.insertCurStatusMySQL));
            threadInsertMySQL.IsBackground = true;
            threadInsertMySQL.Name = "threadInsertMySQL" + Global.curStatus.countDetection.ToString();
            threadInsertMySQL.Start();
        }

        private int createRandomProbability(int probability, int a, int b, int c)
        {
            //以概率probablity生成[a,b]区间数，1-probablity概率生成[b,c]区间数
            Random myRandom = new Random();
            int i = myRandom.Next(1, 100);
            if (i <= probability)
            {
                int j = myRandom.Next(a, b);
                return j;
            }
            else
            {
                int j = myRandom.Next(b + 1, c);
                return j;
            }
        }

        //定时器刷新页面
        private void timer_detectOnce_Tick(object sender, EventArgs e)
        {
            refreshStatusMonitor(sender, e);
        }

        private void labelControl_status_Click(object sender, EventArgs e)
        {
            //this.timer_detectOnce.Enabled = !this.timer_detectOnce.Enabled;
            Global.enableRefreshStatusMonitor = !Global.enableRefreshStatusMonitor;
            Global.enableReFreshRealTimeCurve = !Global.enableReFreshRealTimeCurve;
        }

        private void chartControl_line_DoubleClick(object sender, EventArgs e)
        {
            MessageBox.Show("export chartLine");
            //this.chartControl_line.ExportToXlsx(@"C:\Users\eivision\Desktop\a.xlsx");
        }

        private void chartControl_pie_DoubleClick(object sender, EventArgs e)
        {
            MessageBox.Show("export pie");
        }

        private void chartControl_point_DoubleClick(object sender, EventArgs e)
        {
            MessageBox.Show("export point");
        }
    }
}
