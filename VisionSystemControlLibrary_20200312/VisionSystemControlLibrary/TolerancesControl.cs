/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：TolerancesControl.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：TOLERANCES SETTINGS控件

原作者：视觉检测团队
完成日期：2014/08/18
特别说明：经视觉检测团队授权并遵守代码使用条款，方可使用本代码并获得技术支持，否则所产生的一切后果由使用者承担

修改者：无
完成日期：无
修改记录：无

****************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;

using System.Reflection;

namespace VisionSystemControlLibrary
{
    public partial class TolerancesControl : UserControl
    {
        //该控件为Tolerances页面

        private VisionSystemClassLibrary.Enum.InterfaceLanguage language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;//属性，语言

        private VisionSystemClassLibrary.Enum.DeviceState devicestate = VisionSystemClassLibrary.Enum.DeviceState.Stop;//属性，设备状态

        //

        private int iTotalPage = 3;//属性，包含的页码总数
        private int iCurrentPage = 0;//属性，当前页码（从0开始）
        private int iGraphNumber_Total = 9;//属性，曲线图总数目

        private int iGraphDataIndex = 0;//当前选择的曲线图控件数据（取值小于0，表示未选择任何曲线图控件），即GraphData数组序号

        private int iSign_Learn = 0;//点击【学习】按钮时保存的曲线图控件的特征数值，用于在对话框中点击了【确定】按钮启动学习的函数中使用

        private bool bView = true;//属性，【View Live / View Reject】按钮的类型，取值范围：true，【View Live】（该按钮上显示“View Reject”，View控件中显示Live图片）；false，【View Reject】（该按钮上显示“View Live”，View控件中显示Last Reject图片）
        
        private bool bSaveProduct = false;//属性，【Save Product】按钮的状态。取值范围：true,曲线图被修改；false：曲线图未被修改

        private TolerancesControl_GraphData[] GraphData;//属性（只读），曲线图控件数据

        private VisionSystemClassLibrary.Class.Camera camera = new VisionSystemClassLibrary.Class.Camera();//属性（只读），相机

        //

        private Int32 iEjectLevel = 30;//属性（只读），灵敏度

        private Single[] iPrecision;//属性，斜率

        //

        private Boolean bClickCloseButton = false;//是否点击【CLOSE】按钮。取值范围：true，是；false，否

        //

        private VisionSystemClassLibrary.Class.Brand brand = new VisionSystemClassLibrary.Class.Brand();//属性（只读），品牌

        //

        private Bitmap bitmapNone = null;//无效图像

        //

        private String[][] sMessageText = new String[2][];//提示信息对话框上显示的文本（[语言][包含的文本]）
        private String[][] sMessageText_1 = new String[2][];//标题、平均值和学习值控件上显示的文本（[语言][包含的文本]）

        //

        [Browsable(true), Description("双击曲线图控件时产生的事件"), Category("TolerancesControl 事件")]
        public event EventHandler Control_DoubleClick;//双击曲线图控件时产生的事件
        //CustomEventArgs参数含义：
        //1.IntValue[0]：双击之后，选中的曲线图控件对应的TolerancesControl_GraphData数组序号
        //2.IntValue[1]：双击之前，选中的曲线图控件对应的TolerancesControl_GraphData数组序号

        [Browsable(true), Description("点击【运行/停止】按钮时产生的事件"), Category("TolerancesControl 事件")]
        public event EventHandler RunStop_Click;//点击【运行/停止】按钮时产生的事件
        //CustomEventArgs参数含义：
        //1.IntValue[0]：点击的按钮所在的曲线图控件对应的TolerancesControl_GraphData数组序号

        [Browsable(true), Description("点击【学习】按钮时产生的事件"), Category("TolerancesControl 事件")]
        public event EventHandler Learning_Click;//点击【学习】按钮时产生的事件
        //CustomEventArgs参数含义：
        //1.IntValue[0]：点击的按钮所在的曲线图控件对应的TolerancesControl_GraphData数组序号

        [Browsable(true), Description("点击【返回】按钮时产生的事件"), Category("TolerancesControl 事件")]
        public event EventHandler Close_Click;//点击【返回】按钮时产生的事件
        //CustomEventArgs参数含义：
        //无意义
        
        [Browsable(true), Description("点击【View Live】按钮时产生的事件"), Category("TolerancesControl 事件")]
        public event EventHandler ViewLive_Click;//点击【View Live】按钮时产生的事件
        //CustomEventArgs参数含义：
        //无意义

        [Browsable(true), Description("点击【View Reject】按钮时产生的事件"), Category("TolerancesControl 事件")]
        public event EventHandler ViewReject_Click;//点击【View Reject】按钮时产生的事件
        //CustomEventArgs参数含义：
        //无意义

        [Browsable(true), Description("点击【Save Product】按钮时产生的事件"), Category("TolerancesControl 事件")]
        public event EventHandler SaveProduct_Click;//点击【Save Product】按钮时产生的事件
        //CustomEventArgs参数含义：
        //无意义

        [Browsable(true), Description("点击【Reset Graphs】按钮时产生的事件"), Category("TolerancesControl 事件")]
        public event EventHandler ResetGraphs_Click;//点击【Reset Graphs】按钮时产生的事件
        //CustomEventArgs参数含义：
        //无意义

        [Browsable(true), Description("点击【Previous Page】按钮时产生的事件"), Category("TolerancesControl 事件")]
        public event EventHandler PreviousPage_Click;//点击【Previous Page】按钮时产生的事件
        //CustomEventArgs参数含义：
        //1.IntValue[0]：点击按钮之后，显示的页码
        //2.IntValue[1]：点击按钮之后，选中的曲线图控件对应的TolerancesControl_GraphData数组序号（若没有曲线图被选中，则值为-1）
        //3.IntValue[2]：点击按钮之前，显示的页码
        //4.IntValue[3]：点击按钮之前，选中的曲线图控件对应的TolerancesControl_GraphData数组序号（若没有曲线图被选中，则值为-1）

        [Browsable(true), Description("点击【Next Page】按钮时产生的事件"), Category("TolerancesControl 事件")]
        public event EventHandler NextPage_Click;//点击【Next Page】按钮时产生的事件
        //CustomEventArgs参数含义：
        //1.IntValue[0]：点击按钮之后，显示的页码
        //2.IntValue[1]：点击按钮之后，选中的曲线图控件对应的TolerancesControl_GraphData数组序号（若没有曲线图被选中，则值为-1）
        //3.IntValue[2]：点击按钮之前，显示的页码
        //4.IntValue[3]：点击按钮之前，选中的曲线图控件对应的TolerancesControl_GraphData数组序号（若没有曲线图被选中，则值为-1）

        [Browsable(true), Description("点击【Set】（Upper）按钮时产生的事件"), Category("TolerancesControl 事件")]
        public event EventHandler Upper_Set_Click;//点击【Set】（Upper）按钮时产生的事件
        //CustomEventArgs参数含义：
        //1.IntValue[0]：当前选中的曲线图控件对应的TolerancesControl_GraphData数组序号
        //2.IntValue[1]：待设置的数值的当前值
        //3.IntValue[2]：所能设置的数值范围下限
        //4.IntValue[3]：所能设置的数值范围上限

        [Browsable(true), Description("点击【Set】（Lower）按钮时产生的事件"), Category("TolerancesControl 事件")]
        public event EventHandler Lower_Set_Click;//点击【Set】（Lower）按钮时产生的事件
        //CustomEventArgs参数含义：
        //1.IntValue[0]：当前选中的曲线图控件对应的TolerancesControl_GraphData数组序号
        //2.IntValue[1]：待设置的数值的当前值
        //3.IntValue[2]：所能设置的数值范围下限
        //4.IntValue[3]：所能设置的数值范围上限

        [Browsable(true), Description("设置曲线图Y轴坐标值成功时产生的事件"), Category("TolerancesControl 事件")]
        public event EventHandler SetGraphValueSuccess;//设置曲线图Y轴坐标值成功时产生的事件
        //CustomEventArgs参数含义：
        //1.IntValue[0]：当前选中的曲线图控件对应的TolerancesControl_GraphData数组序号
        //2.IntValue[1]：点击的按钮类型。取值范围：1.【+1】按钮；2.【-1】按钮；3.【+5】按钮；4.【-5】按钮；5.【Set】按钮

        [Browsable(true), Description("设置曲线图Y轴坐标值出现错误时产生的事件"), Category("TolerancesControl 事件")]
        public event EventHandler SetGraphValueFailure;//设置曲线图Y轴坐标值出现错误时产生的事件
        //CustomEventArgs参数含义：
        //1.IntValue[0]：当前选中的曲线图控件对应的TolerancesControl_GraphData数组序号
        //2.IntValue[1]：点击的按钮类型。取值范围：1.【+1】按钮；2.【-1】按钮；3.【+5】按钮；4.【-5】按钮；5.【Set】按钮

        [Browsable(true), Description("点击【EJECT LEVEL】按钮时产生的事件"), Category("TolerancesControl 事件")]
        public event EventHandler EjectLevel_Click;//点击【EJECT LEVEL】按钮时产生的事件
        //CustomEventArgs参数含义：

        [Browsable(true), Description("点击【EJECT LEVEL】按钮时产生的事件"), Category("TolerancesControl 事件")]
        public event EventHandler UpdateTolerances_Click;//点击【EJECT LEVEL】按钮时产生的事件

        //构造函数

        //----------------------------------------------------------------------
        // 功能说明：调试模式时系统默认调用，构造函数，初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public TolerancesControl()
        {
            _Init();//初始化

            //由于该控件仅有一个实例，因此可以在此处进行事件的订阅

            if (null != GlobalWindows.DigitalKeyboard_Window)
            {
                GlobalWindows.DigitalKeyboard_Window.WindowClose_Tolerances_Max += new System.EventHandler(digitalKeyboardWindow_WindowClose_Tolerances_Max);//订阅事件
                GlobalWindows.DigitalKeyboard_Window.WindowClose_Tolerances_Min += new System.EventHandler(digitalKeyboardWindow_WindowClose_Tolerances_Min);//订阅事件
                GlobalWindows.DigitalKeyboard_Window.WindowClose_Tolerances_EjectLevel += new System.EventHandler(digitalKeyboardWindow_WindowClose_Tolerances_EjectLevel);//订阅事件
            }

            if (null != GlobalWindows.MessageDisplay_Window)
            {
                GlobalWindows.MessageDisplay_Window.WindowClose_Tolerances_Learn_Graph1_Confirm += new System.EventHandler(messageDisplayWindow_WindowClose_Tolerances_Learn_Graph1_Confirm);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_Tolerances_Learn_Graph1_Failure += new System.EventHandler(messageDisplayWindow_WindowClose_Tolerances_Learn_Graph1_Failure);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_Tolerances_Learn_Graph2_Confirm += new System.EventHandler(messageDisplayWindow_WindowClose_Tolerances_Learn_Graph2_Confirm);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_Tolerances_Learn_Graph2_Failure += new System.EventHandler(messageDisplayWindow_WindowClose_Tolerances_Learn_Graph2_Failure);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_Tolerances_Learn_Graph3_Confirm += new System.EventHandler(messageDisplayWindow_WindowClose_Tolerances_Learn_Graph3_Confirm);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_Tolerances_Learn_Graph3_Failure += new System.EventHandler(messageDisplayWindow_WindowClose_Tolerances_Learn_Graph3_Failure);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_Tolerances_Learn_Graph4_Confirm += new System.EventHandler(messageDisplayWindow_WindowClose_Tolerances_Learn_Graph4_Confirm);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_Tolerances_Learn_Graph4_Failure += new System.EventHandler(messageDisplayWindow_WindowClose_Tolerances_Learn_Graph4_Failure);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_Tolerances_Learn_Success += new System.EventHandler(messageDisplayWindow_WindowClose_Tolerances_Learn_Success);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_Tolerances_Learn_Failure += new System.EventHandler(messageDisplayWindow_WindowClose_Tolerances_Learn_Failure);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_Tolerances_SaveProduct_Confirm += new System.EventHandler(messageDisplayWindow_WindowClose_Tolerances_SaveProduct_Confirm);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_Tolerances_SaveProduct_Success += new System.EventHandler(messageDisplayWindow_WindowClose_Tolerances_SaveProduct_Success);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_Tolerances_SaveProduct_Failure += new System.EventHandler(messageDisplayWindow_WindowClose_Tolerances_SaveProduct_Failure);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_Tolerances_ResetGraphs_Confirm += new System.EventHandler(messageDisplayWindow_WindowClose_Tolerances_ResetGraphs_Confirm);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_Tolerances_ResetGraphs_Success += new System.EventHandler(messageDisplayWindow_WindowClose_Tolerances_ResetGraphs_Success);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_Tolerances_ResetGraphs_Failure += new System.EventHandler(messageDisplayWindow_WindowClose_Tolerances_ResetGraphs_Failure);//订阅事件
            }

            //

            FieldInfo[] fieldinfo = VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese.GetType().GetFields();//获取公共字段
            if (null != fieldinfo && 1 < fieldinfo.Length)//有效
            {
                Int32 i = 0;//循环控制变量

                sMessageText = new String[fieldinfo.Length - 1][];
                sMessageText_1 = new String[fieldinfo.Length - 1][];
                for (i = 0; i < fieldinfo.Length - 1; i++)
                {
                    sMessageText[i] = new String[17];
                    sMessageText_1[i] = new String[1];
                }

                //

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] = "学习完成";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] = "LEARN COMPLETED";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][1] = "有效值";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][1] = "Valid values";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][2] = "无效值";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][2] = "Non valid values";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][3] = "学习数值";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][3] = "New learn value";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][4] = "学习失败";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][4] = "Learn Failed";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][5] = "保存数据完成";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][5] = "Save Product Completed";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][6] = "保存数据失败";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][6] = "Save Product Failed";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][7] = "复位曲线图成功";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][7] = "Reset Graphs Completed";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][8] = "复位曲线图失败";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][8] = "Reset Graphs Failed";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][9] = "确定对工具进行学习";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][9] = "Perform LEARNING on tool";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][10] = "无有效数据";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][10] = "No Valid Data";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][11] = "确定保存数据";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][11] = "Save Product";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][12] = "确定复位曲线图";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][12] = "Reset Graphs";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][13] = "灵敏度（mm）";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][13] = "Eject Level（mm）";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][14] = "确定创建模板";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][14] = "Learn Sample";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][15] = "创建模板成功";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][15] = "Learn Sample Completed";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][16] = "创建模板失败";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][16] = "Learn Sample Failed";

                //

                sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] = customButtonCaption.Chinese_TextDisplay[0];
                sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] = customButtonCaption.English_TextDisplay[0];
            }
        }

        //属性

        //-----------------------------------------------------------------------
        // 功能说明：ImageDisplayScale_Y属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("图像显示控件比例"), Category("TolerancesControl 通用")]
        public Double ImageDisplayScale_Y
        {
            get//读取
            {
                return imageDisplayView.ControlScale_Y;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：EjectLevel属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("灵敏度"), Category("TolerancesControl 通用")]
        public Int32 EjectLevel
        {
            get//读取
            {
                return iEjectLevel;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Language属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("语言"), Category("TolerancesControl 通用")]
        public VisionSystemClassLibrary.Enum.InterfaceLanguage Language
        {
            get//读取
            {
                return language;
            }
            set//设置
            {
                if (value != language)
                {
                    language = value;

                    //

                    _SetLanguage();
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：SystemDeviceState属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("设备状态"), Category("TolerancesControl 通用")]
        public VisionSystemClassLibrary.Enum.DeviceState SystemDeviceState
        {
            get//读取
            {
                return devicestate;
            }
            set//设置
            {
                if (value != devicestate)
                {
                    devicestate = value;

                    //

                    _SetDeviceState();
                }
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：TotalPage属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("包含的页码总数"), Category("TolerancesControl 通用")]
        public int TotalPage //属性
        {
            get//读取
            {
                return iTotalPage;
            }
            set//设置
            {
                if (value != iTotalPage)
                {
                    iTotalPage = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：CurrentPage属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("当前页码（从0开始）"), Category("TolerancesControl 通用")]
        public int CurrentPage //属性
        {
            get//读取
            {
                return iCurrentPage;
            }
            set//设置
            {
                if (value != iCurrentPage)
                {
                    iCurrentPage = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：GraphNumber_Total属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("曲线图总数目"), Category("TolerancesControl 通用")]
        public int GraphNumber_Total //属性
        {
            get//读取
            {
                return iGraphNumber_Total;
            }
            set//设置
            {
                if (value != iGraphNumber_Total)
                {
                    iGraphNumber_Total = value;

                    GraphData = new TolerancesControl_GraphData[iGraphNumber_Total];//申请内存空间
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：View属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("【View Live / View Reject】按钮的类型，取值范围：true，【View Live】（该按钮上显示“View Reject”，View控件中显示Live图片）；false，【View Reject】（该按钮上显示“View Live”，View控件中显示Last Reject图片）"), Category("TolerancesControl 通用")]
        public bool View //属性
        {
            get//读取
            {
                return bView;
            }
            set//设置
            {
                if (value != bView)
                {
                    bView = value;

                    _SetViewButtonAndText();

                    //

                    _SetView();//设置显示的图像
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：SaveProduct属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("【Save Product】按钮的状态。取值范围：true,曲线图被修改；false：曲线图未被修改"), Category("TolerancesControl 通用")]
        public bool SaveProduct //属性
        {
            get//读取
            {
                return bSaveProduct;
            }
            set//设置
            {
                if (value != bSaveProduct)
                {
                    bSaveProduct = value;

                    //

                    if (bSaveProduct)//曲线图被修改
                    {
                        customButtonSaveProduct.BitmapWhole = VisionSystemControlLibrary.Properties.Resources.Button_1;
                    }
                    else//曲线图未被修改
                    {
                        customButtonSaveProduct.BitmapWhole = VisionSystemControlLibrary.Properties.Resources.Button;
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：TolerancesControlGraphData属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("曲线图控件数据"), Category("TolerancesControl 通用")]
        public TolerancesControl_GraphData[] TolerancesControlGraphData//属性
        {
            get//读取
            {
                return GraphData;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：SelectedCamera属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("相机"), Category("TolerancesControl 通用")]
        public VisionSystemClassLibrary.Class.Camera SelectedCamera//属性
        {
            get//读取
            {
                return camera;
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：SystemBrand属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("品牌"), Category("TolerancesControl 通用")]
        public VisionSystemClassLibrary.Class.Brand SystemBrand//属性
        {
            get//读取
            {
                return brand;
            }
        }

        //函数

        //----------------------------------------------------------------------
        // 功能说明：应用设置的属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _Properties(VisionSystemClassLibrary.Class.Camera camera_parameter, VisionSystemClassLibrary.Class.Brand brand_parameter)
        {
            brand = brand_parameter;

            camera = camera_parameter;

            //

            _SetCamera();//应用属性设置

            _SetLanguage();//设置语言

            _SetDeviceState();//设备状态

        }

        //

        //----------------------------------------------------------------------
        // 功能说明：用户或控件内部调用，应用设置的属性（用户对控件的各个属性进行设置后，调用本函数）
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _Apply()
        {
            //设置各个曲线图的属性

            int i = 0;//循环控制变量
            string sErrorMessage = "";//错误信息

            for (i = 0; i < iGraphNumber_Total; i++)//遍历所有曲线图
            {
                GraphData[i].TolerancesGraph._Apply(ref sErrorMessage);//设置属性
            }

            //翻页控件

            if (1 >= iTotalPage)//页码总数为1
            {
                customButtonPreviousPage.Visible = false;//隐藏【Previous Page】按钮
                customButtonNextPage.Visible = false;//隐藏【Next Page】按钮
            }
            else//页码总数不为1
            {
                if (0 == iCurrentPage)//首页
                {
                    customButtonPreviousPage.Visible = false;//隐藏【Previous Page】按钮
                    customButtonNextPage.Visible = true;//显示【Next Page】按钮
                }
                else if (iTotalPage - 1 == iCurrentPage)//末页
                {
                    customButtonPreviousPage.Visible = true;//显示【Previous Page】按钮
                    customButtonNextPage.Visible = false;//隐藏【Next Page】按钮
                }
                else//其它
                {
                    customButtonPreviousPage.Visible = true;//显示【Previous Page】按钮
                    customButtonNextPage.Visible = true;//显示【Next Page】按钮
                }
            }

            //

            _SetPage();//设置页面曲线图和控件

            _SetView();//设置显示的图像
        }

        //----------------------------------------------------------------------
        // 功能说明：设置属性SelectedCamera后调用，应用设置的属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetCamera()
        {
            int i = 0;//循环控制变量
            int j = 0;//循环控制变量

            //1.通用属性

            customButtonCaption.Chinese_TextDisplay = new String[1] { camera.CameraCHNName + sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] };//设置显示的文本
            customButtonCaption.English_TextDisplay = new String[1] { camera.CameraENGName + sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] };//设置显示的文本

            TotalPage = camera.Tolerances.TotalPage;//包含的页码总数（默认值：3）
            CurrentPage = 0;//当前页码（从0开始）（默认值：0）
            GraphNumber_Total = camera.Tolerances.GraphData.Count;//曲线图总数目（默认值：9）（修改该属性值将重新分配tolerancesControl.GraphControlData的内存空间，因此必须在设置tolerancesControl.GraphControlData属性之前完成该属性值的设置）
            View = true;//【View Live / View Reject】按钮的类型，取值范围：true，【View Live】（该按钮上显示“View Reject”，View控件中显示Live图片）；false，【View Reject】（该按钮上显示“View Live”，View控件中显示Last Reject图片）（默认值：true）
            SaveProduct = false;//【Save Product】按钮的状态。取值范围：true,曲线图被修改；false：曲线图未被修改（默认值：false）

            iEjectLevel = camera.Tolerances.EjectLevel;//灵敏度
            labelEjectLevel.Text = ((Single)iEjectLevel / (Single)10).ToString("F1");//显示，灵敏度
            if (camera.Tolerances.EjectLevelDisplay)//
            {
                customButtonEjectLevel.Visible = true;
                labelEjectLevel.Visible = true;
            } 
            else//
            {
                customButtonEjectLevel.Visible = false;
                labelEjectLevel.Visible = false;
            }

            if (camera.IsSerialPort) //当前为串口
            {
                customButtonLearnSample.Visible = true;
            }
            else //当前为相机
            {
                customButtonLearnSample.Visible = false;
            }

            iPrecision = new Single[camera.Tools.Count];//初始化灵敏度数组
            for (i = 0; i < camera.Tools.Count; i++)
            {
                iPrecision[i] = camera.Tools[i].Precision;
            }

            //2.曲线图控件数据属性

            for (i = 0; i < GraphNumber_Total; i++)//各个曲线图赋值
            {
                GraphData[i] = new TolerancesControl_GraphData(_GetTolerancesGraphics(camera.Tolerances.GraphData[i].TolerancesID));//创建对象（默认值：new TolerancesControl_GraphData()）
                GraphData[i].Page = camera.Tolerances.GraphData[i].Page;//曲线图控件所属的页码（从0开始）（默认值：0）

                //

                GraphData[i].TolerancesGraph.Control_Data.RunorStop = camera.Tools[camera.Tolerances.GraphData[i].ToolsIndex].ToolState;//【运行/停止】按钮（或控件）的运行或停止状态。true：运行；false：停止（默认值：true）
                GraphData[i].TolerancesGraph.Control_Data.ControlSelected = false;//控件是否被（双击）选中。仅在控件为使能状态时，控件才能被选中。true：是；false：否（默认值为：false）
                GraphData[i].TolerancesGraph.Control_Data.ButtonRunStopShow = camera.Tolerances.GraphData[i].ButtonONOFFShow;//【启用/禁用】按钮是否显示。true：是；false：否（默认值为：true）
                GraphData[i].TolerancesGraph.Control_Data.ButtonLearningShow = camera.Tolerances.GraphData[i].ButtonLearningShow;//【学习】按钮是否显示。true：是；false：否（默认值为：true）
                GraphData[i].TolerancesGraph.Control_Data.TolerancesTools.Camera = camera.Type;//工具，相机类型（默认值：CameraType.Top）
                GraphData[i].TolerancesGraph.Control_Data.TolerancesTools.ToolsValue = camera.Tools[camera.Tolerances.GraphData[i].ToolsIndex].ToolState;//工具，数值，表示该工具的使能状态。取值范围：true，使能；false，禁止。（默认值：true）
                GraphData[i].TolerancesGraph.Control_Data.TolerancesTools.ToolsSign = camera.Tolerances.GraphData[i].ToolsIndex;//第1个工具，特征数据（工具数组序号）（默认值：0）
                GraphData[i].TolerancesGraph.Control_Data.TolerancesTools.ToolsName = camera.Tools[camera.Tolerances.GraphData[i].ToolsIndex].Name;//工具，名称（默认值：""）
                GraphData[i].TolerancesGraph.Control_Data.TolerancesTools.ToolsUnit = camera.Tools[camera.Tolerances.GraphData[i].ToolsIndex].ToolsUnit;//工具，单位（默认值：""）
                GraphData[i].TolerancesGraph.Control_Data.TolerancesTools.Learned = camera.Tools[camera.Tolerances.GraphData[i].ToolsIndex].Learned;//是否经过学习。true：是；false：否（默认值：false）
                GraphData[i].TolerancesGraph.Control_Data.TolerancesTools.LearnedValue = camera.Tools[camera.Tolerances.GraphData[i].ToolsIndex].LearnedValue;//学习数值（默认值：100）
                GraphData[i].TolerancesGraph.Control_Data.TolerancesTools.ValidValue = camera.Tools[camera.Tolerances.GraphData[i].ToolsIndex].ValidValue;//学习中的有效数值数量（默认值：100）
                GraphData[i].TolerancesGraph.Control_Data.TolerancesTools.NonvalidValue = camera.Tools[camera.Tolerances.GraphData[i].ToolsIndex].NonvalidValue;//学习中的无效数值数量（默认值：0）
                GraphData[i].TolerancesGraph.Control_Data.TolerancesTools.MaxValue = camera.Tools[camera.Tolerances.GraphData[i].ToolsIndex].MaxValue;//工具在对应的曲线图Y坐标轴上所能设置的最大值（若坐标轴数据形式为包含有效数值，则该值指的是有效数值最大值；否则指的是坐标轴最大值）（默认值：1000）
                GraphData[i].TolerancesGraph.Control_Data.TolerancesTools.MinValue = camera.Tools[camera.Tolerances.GraphData[i].ToolsIndex].MinValue;//工具在对应的曲线图Y坐标轴上所能设置的最小值（若坐标轴数据形式为包含有效数值，则该值指的是有效数值最小值；否则指的是坐标轴最小值）（默认值：0）
                GraphData[i].TolerancesGraph.Control_Data.TolerancesTools.EffectiveMin_State = camera.Tools[camera.Tolerances.GraphData[i].ToolsIndex].EffectiveMin_State;//坐标轴最小有效实际数值是否开启
                GraphData[i].TolerancesGraph.Control_Data.TolerancesTools.EffectiveMax_State = camera.Tools[camera.Tolerances.GraphData[i].ToolsIndex].EffectiveMax_State;//坐标轴最大有效实际数值是否开启

                GraphData[i].TolerancesGraph.Graph_YAxis.AxisAdditionalValueDisplay = camera.Tolerances.GraphData[i].AdditionalValueDisplay;//曲线图坐标轴上是否显示附加数值。true：是；false：否（默认值为：false）
                GraphData[i].TolerancesGraph.Graph_YAxis.AdditionalValueRatio = camera.Tolerances.GraphData[i].AdditionalValueRatio;//曲线图坐标轴上显示的附加数值系数
                GraphData[i].TolerancesGraph.Graph_YAxis.AdditionalValueUnit = camera.Tolerances.GraphData[i].AdditionalValueUnit;//曲线图坐标轴上显示的附加数值单位
                GraphData[i].TolerancesGraph.Graph_YAxis.AxisEffectiveMin_Value = camera.Tolerances.GraphData[i].EffectiveMin_Value;//坐标轴最小有效实际数值（默认值为：300）
                GraphData[i].TolerancesGraph.Graph_YAxis.AxisEffectiveMax_Value = camera.Tolerances.GraphData[i].EffectiveMax_Value;//坐标轴最大有效实际数值（默认值为：700）

                if ("" != GraphData[i].TolerancesGraph.Control_Data.TolerancesTools.ToolsUnit)//有效
                {
                    GraphData[i].TolerancesGraph.Caption = GraphData[i].TolerancesGraph.Control_Data.TolerancesTools.ToolsName.ToUpper() + "（" + GraphData[i].TolerancesGraph.Control_Data.TolerancesTools.ToolsUnit + "）";//控件标题文本（默认值为：EXAMPLE）
                } 
                else//无效
                {
                    GraphData[i].TolerancesGraph.Caption = GraphData[i].TolerancesGraph.Control_Data.TolerancesTools.ToolsName.ToUpper();//控件标题文本（默认值为：EXAMPLE）
                }

                GraphData[i].TolerancesGraph.ValueNumber_Curve = camera.Tolerances.GraphData[i].ValueNumber;//绘制区域中的曲线值数目（坐标点数目，根据X轴数值获取）（默认值为：100）
                try
                {
                    for (j = 0; j < GraphData[i].TolerancesGraph.ValueNumber_Curve; j++)//曲线图数值
                    {
                        GraphData[i].TolerancesGraph.Value_Y[0][j] = camera.Tolerances.GraphData[i].TolerancesGraphDataValue.Value[j];//数值
                    }
                }
                catch (System.Exception ex)
                {
                	//不执行操作
                }
                GraphData[i].TolerancesGraph.CurrentValueIndex[0] = camera.Tolerances.GraphData[i].TolerancesGraphDataValue.CurrentValueIndex;//绘制区域中当前显示的曲线值点（从0开始，小于0表示曲线图数值无效，最大值为ValueNumber - 1）（默认值为：99）
                GraphData[i].TolerancesGraph.Graph_YAxis.AxisAdditionalValue = camera.Tolerances.GraphData[i].TolerancesGraphDataValue.AdditionalValue;//曲线图坐标轴上显示的附加数值（根据该值得到sAxisAdditionalValue数值）（默认值为：10）

                //

                if (1 == camera.Tolerances.GraphData[i].TolerancesID)//tolerancesGraph1
                {
                    _InitGraphData(i, tolerancesGraph1);
                }
                else if (2 == camera.Tolerances.GraphData[i].TolerancesID)//tolerancesGraph2
                {
                    _InitGraphData(i, tolerancesGraph2);
                }
                else if (3 == camera.Tolerances.GraphData[i].TolerancesID)//tolerancesGraph3
                {
                    _InitGraphData(i, tolerancesGraph3);
                }
                else//4，tolerancesGraph4
                {
                    _InitGraphData(i, tolerancesGraph4);
                }
            }

            //3.应用设置的属性

            _Apply();
        }

        //----------------------------------------------------------------------
        // 功能说明：初始化控件数据
        // 输入参数：1.iIndex：数组序号
        //         2.tolerancesGraph：控件
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _InitGraphData(Int32 iIndex, Tolerances tolerancesGraph)
        {
            GraphData[iIndex].TolerancesGraph.Graph_XAxis.DataType = tolerancesGraph.XAxisDataType;
            GraphData[iIndex].TolerancesGraph.Graph_XAxis.AxisValueNumber = tolerancesGraph.XAxisValueNumber;
            GraphData[iIndex].TolerancesGraph.Graph_XAxis.AxisValueDisplay = tolerancesGraph.XAxisValueDisplay;
            GraphData[iIndex].TolerancesGraph.Graph_XAxis.AxisAdditionalValueDisplay = tolerancesGraph.XAxisAdditionalValueDisplay;         
            GraphData[iIndex].TolerancesGraph.Graph_XAxis.AxisAdditionalValue = tolerancesGraph.XAxisAdditionalValue;
            GraphData[iIndex].TolerancesGraph.Graph_XAxis.AdditionalValueRatio = tolerancesGraph.XAxisAdditionalValueRatio;
            GraphData[iIndex].TolerancesGraph.Graph_XAxis.AdditionalValueUnit = tolerancesGraph.XAxisAdditionalValueUnit;
            GraphData[iIndex].TolerancesGraph.Graph_XAxis.AxisMin_Value = tolerancesGraph.XAxisMinValue;
            GraphData[iIndex].TolerancesGraph.Graph_XAxis.AxisMax_Value = tolerancesGraph.XAxisMaxValue;
            GraphData[iIndex].TolerancesGraph.Graph_XAxis.AxisEffectiveMin_Value = tolerancesGraph.XAxisEffectiveMinValue;
            GraphData[iIndex].TolerancesGraph.Graph_XAxis.AxisEffectiveMax_Value = tolerancesGraph.XAxisEffectiveMaxValue;

            GraphData[iIndex].TolerancesGraph.Graph_YAxis.DataType = tolerancesGraph.YAxisDataType;
            GraphData[iIndex].TolerancesGraph.Graph_YAxis.AxisValueDisplay = tolerancesGraph.YAxisValueDisplay;
            GraphData[iIndex].TolerancesGraph.Graph_YAxis.AxisValueNumber = tolerancesGraph.YAxisValueNumber;
            GraphData[iIndex].TolerancesGraph.Graph_YAxis.AxisMin_Value = tolerancesGraph.YAxisMinValue;
            GraphData[iIndex].TolerancesGraph.Graph_YAxis.AxisMax_Value = tolerancesGraph.YAxisMaxValue;

            //

            GraphData[iIndex].TolerancesGraph.ColorControlSelected = tolerancesGraph.ColorControlSelected;
            GraphData[iIndex].TolerancesGraph.ColorControlUnselected = tolerancesGraph.ColorControlUnselected;
            GraphData[iIndex].TolerancesGraph.ColorDrawCaptionDisable = tolerancesGraph.ColorDrawCaptionDisable;
            GraphData[iIndex].TolerancesGraph.ColorDrawCaptionEnable = tolerancesGraph.ColorDrawCaptionEnable;
            GraphData[iIndex].TolerancesGraph.SolidbrushDrawCurValue = new SolidBrush(tolerancesGraph.ColorDrawCurValue);
            GraphData[iIndex].TolerancesGraph.ColorDrawGraph = tolerancesGraph.ColorDrawGraph;
            GraphData[iIndex].TolerancesGraph.ColorLine = tolerancesGraph.ColorLine;
            GraphData[iIndex].TolerancesGraph.ColorValueDisable = tolerancesGraph.ColorValueDisable;
            GraphData[iIndex].TolerancesGraph.ColorValueEnable = tolerancesGraph.ColorValueEnable;
            GraphData[iIndex].TolerancesGraph.ColorValueUndefined = tolerancesGraph.ColorValueUndefined;

            GraphData[iIndex].TolerancesGraph.FontCaption = tolerancesGraph.FontCaption;
            GraphData[iIndex].TolerancesGraph.FontCurValue = tolerancesGraph.FontCurValue;
            GraphData[iIndex].TolerancesGraph.LineWidth = tolerancesGraph.LineWidth;
            GraphData[iIndex].TolerancesGraph.RectCaption = new Rectangle(tolerancesGraph.LocationCaption, tolerancesGraph.SizeCaption);
            GraphData[iIndex].TolerancesGraph.RectCurValue = new Rectangle(tolerancesGraph.LocationCurValue, tolerancesGraph.SizeCurValue);
            GraphData[iIndex].TolerancesGraph.RectCurValueIcon = new Rectangle(tolerancesGraph.LocationCurValueIcon, tolerancesGraph.SizeCurValueIcon);
            GraphData[iIndex].TolerancesGraph.RectGraph = new Rectangle(tolerancesGraph.LocationGraph, tolerancesGraph.SizeGraph);

            GraphData[iIndex].TolerancesGraph.XAxisCurValueDisplay = tolerancesGraph.XAxisCurValueDisplay;
            GraphData[iIndex].TolerancesGraph.YAxisCurValueDisplay = tolerancesGraph.YAxisCurValueDisplay;

            GraphData[iIndex].TolerancesGraph.Graph_XAxis.FromAxisMaxToEffectiveMax_Pixel = tolerancesGraph.FromXAxisMaxToEffectiveMax;
            GraphData[iIndex].TolerancesGraph.Graph_XAxis.FromAxisMinToEffectiveMin_Pixel = tolerancesGraph.FromXAxisMinToEffectiveMin;
            GraphData[iIndex].TolerancesGraph.Graph_XAxis.tolerancesDrawing_Axis.FontValue = tolerancesGraph.XAxisFontValue;
            GraphData[iIndex].TolerancesGraph.Graph_XAxis.RectAdditionalValue = new Rectangle(tolerancesGraph.XAxisLocationAdditionalValue, tolerancesGraph.XAxisSizeAdditionalValue);
            GraphData[iIndex].TolerancesGraph.Graph_XAxis.LocationAxisMax_Pixel = tolerancesGraph.XAxisLocationMax;
            GraphData[iIndex].TolerancesGraph.Graph_XAxis.LocationAxisMin_Pixel = tolerancesGraph.XAxisLocationMin;
            GraphData[iIndex].TolerancesGraph.Graph_XAxis.SizePixel_AxisPoint = tolerancesGraph.XAxisSizePoint;
            GraphData[iIndex].TolerancesGraph.Graph_XAxis.tolerancesDrawing_Axis.ColorDrawAdditionalValueDisable = tolerancesGraph.XAxisDrawAdditionalValueDisable;
            GraphData[iIndex].TolerancesGraph.Graph_XAxis.tolerancesDrawing_Axis.ColorDrawAdditionalValueEnable = tolerancesGraph.XAxisDrawAdditionalValueEnable;
            GraphData[iIndex].TolerancesGraph.Graph_XAxis.tolerancesDrawing_Axis.ColorDrawAdditionalValueLineDisable = tolerancesGraph.XAxisDrawAdditionalValueLineDisable;
            GraphData[iIndex].TolerancesGraph.Graph_XAxis.tolerancesDrawing_Axis.ColorDrawAdditionalValueLineEnable = tolerancesGraph.XAxisDrawAdditionalValueLineEnable;
            GraphData[iIndex].TolerancesGraph.Graph_XAxis.tolerancesDrawing_Axis.DrawAdditionalValueLineWidth = tolerancesGraph.XAxisDrawAdditionalValueLineWidth;
            GraphData[iIndex].TolerancesGraph.Graph_XAxis.tolerancesDrawing_Axis.ColorDrawAxisDisable = tolerancesGraph.XAxisDrawAxisDisable;
            GraphData[iIndex].TolerancesGraph.Graph_XAxis.tolerancesDrawing_Axis.ColorDrawAxisEnable = tolerancesGraph.XAxisDrawAxisEnable;
            GraphData[iIndex].TolerancesGraph.Graph_XAxis.tolerancesDrawing_Axis.ColorDrawAxisValueDisable = tolerancesGraph.XAxisDrawAxisValueDisable;
            GraphData[iIndex].TolerancesGraph.Graph_XAxis.tolerancesDrawing_Axis.ColorDrawAxisValueEnable = tolerancesGraph.XAxisDrawAxisValueEnable;
            GraphData[iIndex].TolerancesGraph.Graph_XAxis.tolerancesDrawing_Axis.DrawAxisWidth = tolerancesGraph.XAxisDrawAxisWidth;
            GraphData[iIndex].TolerancesGraph.Graph_XAxis.tolerancesDrawing_Axis.ColorDrawEffectiveLineDisable = tolerancesGraph.XAxisDrawEffectiveLineDisable;
            GraphData[iIndex].TolerancesGraph.Graph_XAxis.tolerancesDrawing_Axis.ColorDrawEffectiveMinLineEnable = tolerancesGraph.XAxisDrawEffectiveMinLineEnable;
            GraphData[iIndex].TolerancesGraph.Graph_XAxis.tolerancesDrawing_Axis.ColorDrawEffectiveMaxLineEnable = tolerancesGraph.XAxisDrawEffectiveMaxLineEnable;
            GraphData[iIndex].TolerancesGraph.Graph_XAxis.tolerancesDrawing_Axis.DrawEffectiveLineWidth = tolerancesGraph.XAxisDrawEffectiveLineWidth;
            GraphData[iIndex].TolerancesGraph.Graph_XAxis.tolerancesDrawing_Axis.ColorDrawEffectiveValueDisable = tolerancesGraph.XAxisDrawEffectiveValueDisable;
            GraphData[iIndex].TolerancesGraph.Graph_XAxis.tolerancesDrawing_Axis.ColorDrawEffectiveMinValueEnable = tolerancesGraph.XAxisDrawEffectiveMinValueEnable;
            GraphData[iIndex].TolerancesGraph.Graph_XAxis.tolerancesDrawing_Axis.ColorDrawEffectiveMaxValueEnable = tolerancesGraph.XAxisDrawEffectiveMaxValueEnable;

            GraphData[iIndex].TolerancesGraph.Graph_YAxis.FromAxisMaxToEffectiveMax_Pixel = tolerancesGraph.FromYAxisMaxToEffectiveMax;
            GraphData[iIndex].TolerancesGraph.Graph_YAxis.FromAxisMinToEffectiveMin_Pixel = tolerancesGraph.FromYAxisMinToEffectiveMin;
            GraphData[iIndex].TolerancesGraph.Graph_YAxis.tolerancesDrawing_Axis.FontValue = tolerancesGraph.YAxisFontValue;
            GraphData[iIndex].TolerancesGraph.Graph_YAxis.RectAdditionalValue = new Rectangle(tolerancesGraph.YAxisLocationAdditionalValue, tolerancesGraph.YAxisSizeAdditionalValue);
            GraphData[iIndex].TolerancesGraph.Graph_YAxis.LocationAxisMax_Pixel = tolerancesGraph.YAxisLocationMax;
            GraphData[iIndex].TolerancesGraph.Graph_YAxis.LocationAxisMin_Pixel = tolerancesGraph.YAxisLocationMin;
            GraphData[iIndex].TolerancesGraph.Graph_YAxis.SizePixel_AxisPoint = tolerancesGraph.YAxisSizePoint;
            GraphData[iIndex].TolerancesGraph.Graph_YAxis.tolerancesDrawing_Axis.ColorDrawAdditionalValueDisable = tolerancesGraph.YAxisDrawAdditionalValueDisable;
            GraphData[iIndex].TolerancesGraph.Graph_YAxis.tolerancesDrawing_Axis.ColorDrawAdditionalValueEnable = tolerancesGraph.YAxisDrawAdditionalValueEnable;
            GraphData[iIndex].TolerancesGraph.Graph_YAxis.tolerancesDrawing_Axis.ColorDrawAdditionalValueLineDisable = tolerancesGraph.YAxisDrawAdditionalValueLineDisable;
            GraphData[iIndex].TolerancesGraph.Graph_YAxis.tolerancesDrawing_Axis.ColorDrawAdditionalValueLineEnable = tolerancesGraph.YAxisDrawAdditionalValueLineEnable;
            GraphData[iIndex].TolerancesGraph.Graph_YAxis.tolerancesDrawing_Axis.DrawAdditionalValueLineWidth = tolerancesGraph.YAxisDrawAdditionalValueLineWidth;
            GraphData[iIndex].TolerancesGraph.Graph_YAxis.tolerancesDrawing_Axis.ColorDrawAxisDisable = tolerancesGraph.YAxisDrawAxisDisable;
            GraphData[iIndex].TolerancesGraph.Graph_YAxis.tolerancesDrawing_Axis.ColorDrawAxisEnable = tolerancesGraph.YAxisDrawAxisEnable;
            GraphData[iIndex].TolerancesGraph.Graph_YAxis.tolerancesDrawing_Axis.ColorDrawAxisValueDisable = tolerancesGraph.YAxisDrawAxisValueDisable;
            GraphData[iIndex].TolerancesGraph.Graph_YAxis.tolerancesDrawing_Axis.ColorDrawAxisValueEnable = tolerancesGraph.YAxisDrawAxisValueEnable;
            GraphData[iIndex].TolerancesGraph.Graph_YAxis.tolerancesDrawing_Axis.DrawAxisWidth = tolerancesGraph.YAxisDrawAxisWidth;
            GraphData[iIndex].TolerancesGraph.Graph_YAxis.tolerancesDrawing_Axis.ColorDrawEffectiveLineDisable = tolerancesGraph.YAxisDrawEffectiveLineDisable;
            GraphData[iIndex].TolerancesGraph.Graph_YAxis.tolerancesDrawing_Axis.ColorDrawEffectiveMinLineEnable = tolerancesGraph.YAxisDrawEffectiveMinLineEnable;
            GraphData[iIndex].TolerancesGraph.Graph_YAxis.tolerancesDrawing_Axis.ColorDrawEffectiveMaxLineEnable = tolerancesGraph.YAxisDrawEffectiveMaxLineEnable;
            GraphData[iIndex].TolerancesGraph.Graph_YAxis.tolerancesDrawing_Axis.DrawEffectiveLineWidth = tolerancesGraph.YAxisDrawEffectiveLineWidth;
            GraphData[iIndex].TolerancesGraph.Graph_YAxis.tolerancesDrawing_Axis.ColorDrawEffectiveValueDisable = tolerancesGraph.YAxisDrawEffectiveValueDisable;
            GraphData[iIndex].TolerancesGraph.Graph_YAxis.tolerancesDrawing_Axis.ColorDrawEffectiveMinValueEnable = tolerancesGraph.YAxisDrawEffectiveMinValueEnable;
            GraphData[iIndex].TolerancesGraph.Graph_YAxis.tolerancesDrawing_Axis.ColorDrawEffectiveMaxValueEnable = tolerancesGraph.YAxisDrawEffectiveMaxValueEnable;
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：设置图像数据
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _SetImageData()
        {
            _SetView();//设置显示的图像
        }

        //----------------------------------------------------------------------
        // 功能说明：用户调用，更新曲线图控件的曲线图数值
        // 输入参数：1.iGraphIndex：待更新的曲线图数据数组序号（GraphData数组序号）
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _SetCurveValue(int iGraphIndex)
        {
            int i = 0;//循环控制变量

            //更新数值

            for (i = 0; i < GraphData[iGraphIndex].TolerancesGraph.ValueNumber_Curve; i++)
            {
                GraphData[iGraphIndex].TolerancesGraph.Value_Y[0][i] = camera.Tolerances.GraphData[iGraphIndex].TolerancesGraphDataValue.Value[i];
            }
            GraphData[iGraphIndex].TolerancesGraph.CurrentValueIndex[0] = camera.Tolerances.GraphData[iGraphIndex].TolerancesGraphDataValue.CurrentValueIndex;

            GraphData[iGraphIndex].TolerancesGraph._SetCurveValue();//配置曲线图
            GraphData[iGraphIndex].TolerancesGraph._SetCurrentValue();//配置当前值

            //刷新控件

            for (i = 0; i < iGraphNumber_Total; i++)//遍历所有曲线图控件
            {
                if (i == iGraphIndex)//当前曲线图数据
                {
                    if (GraphData[i].Page == iCurrentPage)//当前页中的曲线图控件
                    {
                        if (tolerancesGraph1.GraphDataIndex == i)//曲线图1
                        {
                            tolerancesGraph1._Refresh();//刷新控件
                        }
                        else if (tolerancesGraph2.GraphDataIndex == i)//曲线图2
                        {
                            tolerancesGraph2._Refresh();//刷新控件
                        }
                        else if (tolerancesGraph3.GraphDataIndex == i)//曲线图3
                        {
                            tolerancesGraph3._Refresh();//刷新控件
                        }
                        else//tolerancesGraph4.GraphDataIndex == i，曲线图4
                        {
                            tolerancesGraph4._Refresh();//刷新控件
                        }
                        //
                        break;
                    }
                }
            }

            //更新设置数值区域

            _SetSettingsValue();
        }

        //----------------------------------------------------------------------
        // 功能说明：设置灵敏度后，调用本函数配置Y轴坐标和曲线图，并更新绘制区域
        // 输入参数：1.iGraphIndex：待更新的曲线图数据数组序号（GraphData数组序号）
        //         2.iMin：最小值
        //         3.iMax：最大值
        //         4.iUpdateTolerances：最大值
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _SetEjectLevel(int iGraphIndex, Int32 iMin, Int32 iMax, Int32 iUpdateTolerances)
        {
            GraphData[iGraphIndex].TolerancesGraph.Graph_YAxis.AxisEffectiveMax_Value = iMax;
            GraphData[iGraphIndex].TolerancesGraph.Graph_YAxis.AxisEffectiveMin_Value = iMin;

            GraphData[iGraphIndex].TolerancesGraph.Graph_YAxis.AxisAdditionalValue = VisionSystemClassLibrary.Class.TolerancesData._SetAdditionalValue((Int16)iMin, (Int16)iMax, GraphData[iGraphIndex].TolerancesGraph.Graph_YAxis.AdditionalValueRatio);

            if (GraphData[iGraphIndex].TolerancesGraph.ControlSelected)
            {
                labelUpper_SetValue.Text = iMax.ToString();//赋值

                labelLower_SetValue.Text = iMin.ToString();//赋值
            }

            GraphData[iGraphIndex].TolerancesGraph.Graph_YAxis._SetAxisData();//配置Y坐标轴
            GraphData[iGraphIndex].TolerancesGraph._SetCurveValue();//配置曲线图

            if (1 == iUpdateTolerances) //更新公差
            {
                if (camera.Tools[camera.Tolerances.GraphData[iGraphIndex].ToolsIndex].Type == 7)//更改公差下限，且执行烟支检测
                {
                    camera.Tolerances._GetPrecision_Tobacco_D((Int16)iEjectLevel, iMax, camera.Tools[camera.Tolerances.GraphData[iGraphIndex].ToolsIndex].EjectPixelMin, ref iPrecision[camera.Tolerances.GraphData[iGraphIndex].ToolsIndex]);
                }
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：设置【View Live / View Reject】按钮的背景图像及其指示文本
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetViewButtonAndText()
        {
            if (bView)//【View Live】（该按钮上显示“View Reject”，View控件中显示Live图片）
            {
                customButtonView.CurrentTextGroupIndex = 0;

                customButtonView.BitmapIconWhole = VisionSystemControlLibrary.Properties.Resources.Reject;
                customButtonView.IconLocation = new Point[2] { new Point(80, 9), new Point(80, 9) };

                //

                customButtonViewText.CurrentTextGroupIndex = 1;
            }
            else//【View Reject】（该按钮上显示“View Live”，View控件中显示Last Reject图片）
            {
                customButtonView.CurrentTextGroupIndex = 1;

                customButtonView.BitmapIconWhole = VisionSystemControlLibrary.Properties.Resources.ConfigImage;
                customButtonView.IconLocation = new Point[2] { new Point(75, 11), new Point(75, 11) };

                //

                customButtonViewText.CurrentTextGroupIndex = 0;
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：构造函数中调用，初始化数据
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _Init()
        {
            InitializeComponent();

            //

            labelUpper_SetValue.Text = "--";
            labelLower_SetValue.Text = "--";

            bitmapNone = new Bitmap(imageDisplayView.Width, imageDisplayView.Height);//无效图像

            GraphData = new TolerancesControl_GraphData[iGraphNumber_Total];//申请内存空间
        }

        //----------------------------------------------------------------------
        // 功能说明：获取曲线图控件包含的Graphics对象
        // 输入参数：1.iTolerances：曲线图控件序号。取值范围：1 ~ 4，对应于tolerancesGraph1 ~ tolerancesGraph4曲线图控件
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private Graphics _GetTolerancesGraphics(int iTolerances)
        {
            if (1 == iTolerances)//tolerancesGraph1
            {
                return tolerancesGraph1.Control_Graphics;
            }
            else if (2 == iTolerances)//tolerancesGraph2
            {
                return tolerancesGraph2.Control_Graphics;
            }
            else if (3 == iTolerances)//tolerancesGraph3
            {
                return tolerancesGraph3.Control_Graphics;
            }
            else//4 == iTolerances，tolerancesGraph4
            {
                return tolerancesGraph4.Control_Graphics;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：获取曲线图控件包含的Graphics对象
        // 输入参数：1.iTolerances：曲线图控件序号。取值范围：1 ~ 4，对应于tolerancesGraph1 ~ tolerancesGraph4曲线图控件
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private Size _GetTolerancesSize(int iTolerances)
        {
            if (1 == iTolerances)//tolerancesGraph1
            {
                return tolerancesGraph1.Size;
            }
            else if (2 == iTolerances)//tolerancesGraph2
            {
                return tolerancesGraph2.Size;
            }
            else if (3 == iTolerances)//tolerancesGraph3
            {
                return tolerancesGraph3.Size;
            }
            else//4 == iTolerances，tolerancesGraph4
            {
                return tolerancesGraph4.Size;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：设置VIEW图像
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetView()
        {
            if (bView)//【View Live】（该按钮上显示“View Reject”，View控件中显示Live图片）
            {
                imageDisplayView.Information = camera.Live.GraphicsInformation;

                if (null != camera.ImageLive)//有效
                {
                    if (imageDisplayView.ControlSize.Width <= camera.ImageLive.Width && imageDisplayView.ControlSize.Height <= camera.ImageLive.Height)//有效
                    {
                        if (!(imageDisplayView.ShowTitle))//隐藏
                        {
                            imageDisplayView.ShowTitle = true;//显示
                        }

                        //

                        imageDisplayView.BitmapDisplay = camera.ImageLive.ToBitmap();//图像数据
                    }
                }
                else//无效
                {
                    if (imageDisplayView.ShowTitle)//显示
                    {
                        imageDisplayView.ShowTitle = false;//隐藏
                    }

                    //

                    imageDisplayView.BitmapDisplay = (Bitmap)bitmapNone.Clone();//图像数据
                }
            }
            else//【View Reject】（该按钮上显示“View Live”，View控件中显示Last Reject图片）
            {
                imageDisplayView.Information = camera.Rejects.GraphicsInformation[camera.Rejects.ImageNumberTotal - 1];

                if (null != camera.ImageReject)//有效
                {
                    if (imageDisplayView.ControlSize.Width <= camera.ImageReject.Width && imageDisplayView.ControlSize.Height <= camera.ImageReject.Height)//有效
                    {
                        if (!(imageDisplayView.ShowTitle))//隐藏
                        {
                            imageDisplayView.ShowTitle = true;//显示
                        }

                        //

                        imageDisplayView.BitmapDisplay = camera.ImageReject.ToBitmap();//图像文件名称
                    }
                }
                else//无效
                {
                    if (imageDisplayView.ShowTitle)//显示
                    {
                        imageDisplayView.ShowTitle = false;//隐藏
                    }

                    //

                    imageDisplayView.BitmapDisplay = (Bitmap)bitmapNone.Clone();//图像数据
                }
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：设置语言
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetLanguage()
        {
            customButtonView.Language = language;//【View Live / View Reject】
            customButtonSaveProduct.Language = language;//【Save Product】
            customButtonResetGraphs.Language = language;//【Reset Graphs】
            customButtonUpper_Set.Language = language;//【Set】（Upper）
            customButtonLower_Set.Language = language;//【Set】（Lower）
            customButtonLearnSample.Language = language;

            //

            customButtonUpper_Plus1.Language = language;//【+1】（Upper）
            customButtonUpper_Plus5.Language = language;//【+5】（Upper）
            customButtonUpper_Subtract1.Language = language;//【-1】（Upper）
            customButtonUpper_Subtract5.Language = language;//【-5】（Upper）
            customButtonLower_Plus1.Language = language;//【+1】（Lower）
            customButtonLower_Plus5.Language = language;//【+5】（Lower）
            customButtonLower_Subtract1.Language = language;//【-1】（Lower）
            customButtonLower_Subtract5.Language = language;//【-5】（Lower）

            customButtonPreviousPage.Language = language;//【Previous Page】
            customButtonNextPage.Language = language;//【Next Page】

            //

            customButtonCaption.Language = language;//设置显示的文本
            
            customButtonViewText.Language = language;//【View Live / View Reject】指示文本
            customButtonMeanValue.Language = language;//平均值
            customButtonLearnValue.Language = language;//学习值
            customButtonLearnValue_Text.Language = language;//学习值文本

            //

            customButtonEjectLevel.Language = language;//【EJECT LEVEL】

            //

            imageDisplayView.Language = language;//标题栏

            //

            try
            {
                Int32 i = 0;//循环控制变量
                Int32 j = 0;//循环控制变量

                for (i = 0; i < GraphNumber_Total; i++)//各个曲线图赋值
                {
                    GraphData[i].TolerancesGraph.Control_Data.TolerancesTools.ToolsName = camera.Tools[camera.Tolerances.GraphData[i].ToolsIndex].Name;//工具，名称（默认值：""）

                    GraphData[i].TolerancesGraph.Control_Data.TolerancesTools.ToolsName = GraphData[i].TolerancesGraph.Control_Data.TolerancesTools.ToolsName;//工具，名称（默认值：""）

                    if ("" != GraphData[i].TolerancesGraph.Control_Data.TolerancesTools.ToolsUnit)//有效
                    {
                        GraphData[i].TolerancesGraph.Caption = GraphData[i].TolerancesGraph.Control_Data.TolerancesTools.ToolsName.ToUpper() + "（" + GraphData[i].TolerancesGraph.Control_Data.TolerancesTools.ToolsUnit + "）";//控件标题文本（默认值为：EXAMPLE）
                    } 
                    else//无效
                    {
                        GraphData[i].TolerancesGraph.Caption = GraphData[i].TolerancesGraph.Control_Data.TolerancesTools.ToolsName.ToUpper();//控件标题文本（默认值为：EXAMPLE）
                    }
                }

                for (i = 0; i < iGraphNumber_Total; i++)//遍历所有曲线图控件
                {
                    if (GraphData[i].Page == iCurrentPage)//当前页中的曲线图控件
                    {
                        if (0 == j)//曲线图1
                        {
                            tolerancesGraph1._SetCaption(GraphData[i]);//设置曲线图控件1标题（工具名称）                  
                        }
                        else if (1 == j)//曲线图2
                        {
                            tolerancesGraph2._SetCaption(GraphData[i]);//设置曲线图控件2标题（工具名称）                  
                        }
                        else if (2 == j)//曲线图3
                        {
                            tolerancesGraph3._SetCaption(GraphData[i]);//设置曲线图控件3标题（工具名称）                  
                        }
                        else//3，曲线图4
                        {
                            tolerancesGraph4._SetCaption(GraphData[i]);//设置曲线图控件4标题（工具名称）                  
                        }

                        //

                        j++;
                    }
                }
            }
            catch (System.Exception ex)
            {
                //不执行操作
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：用户调用，设备状态设置完成后，更新页面
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetDeviceState()
        {
            customButtonUpper_Plus1.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【+1】（Upper）
            customButtonUpper_Plus5.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【+5】（Upper）
            customButtonUpper_Subtract1.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【-1】（Upper）
            customButtonUpper_Subtract5.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【-5】（Upper）
            customButtonUpper_Set.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【Set】（Upper）
            customButtonLower_Plus1.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【+1】（Lower）
            customButtonLower_Plus5.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【+5】（Lower）
            customButtonLower_Subtract1.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【-1】（Lower）
            customButtonLower_Subtract5.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【-5】（Lower）
            customButtonLower_Set.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【Set】（Lower）
            customButtonView.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【VIEW LIVE】/【VIEW REJECT】
            if (bSaveProduct)//参数被修改
            {
                customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【SAVE PRODUCT】
            }
            else//参数未被修改
            {
                customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【SAVE PRODUCT】
            }
            customButtonResetGraphs.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【RESET GRAPHS】
            customButtonEjectLevel.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【EJECT LEVEL】

            tolerancesGraph1.TolerancesEnabled = true;//曲线图1
            tolerancesGraph2.TolerancesEnabled = true;//曲线图2
            tolerancesGraph3.TolerancesEnabled = true;//曲线图3
            tolerancesGraph4.TolerancesEnabled = true;//曲线图4

            customButtonPreviousPage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【Previous Page】
            customButtonNextPage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【Next Page】
        }

        //----------------------------------------------------------------------
        // 功能说明：用户调用，设备状态改变时（如某一个相机连接或断开），更新页面
        // 输入参数：1.bConnected：设备是否连接。取值范围：true，是；false，否
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _DeviceStateChanged(Boolean bConnected)
        {
            if (bConnected)//连接
            {
                _SetDeviceState();//设备状态
            }
            else//断开
            {
                customButtonUpper_Plus1.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【+1】（Upper）
                customButtonUpper_Plus5.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【+5】（Upper）
                customButtonUpper_Subtract1.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【-1】（Upper）
                customButtonUpper_Subtract5.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【-5】（Upper）
                customButtonUpper_Set.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【Set】（Upper）
                customButtonLower_Plus1.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【+1】（Lower）
                customButtonLower_Plus5.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【+5】（Lower）
                customButtonLower_Subtract1.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【-1】（Lower）
                customButtonLower_Subtract5.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【-5】（Lower）
                customButtonLower_Set.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【Set】（Lower）
                customButtonView.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【VIEW LIVE】/【VIEW REJECT】
                customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【SAVE PRODUCT】
                customButtonResetGraphs.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【RESET GRAPHS】
                customButtonEjectLevel.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【EJECT LEVEL】

                tolerancesGraph1.TolerancesEnabled = false;//曲线图1
                tolerancesGraph2.TolerancesEnabled = false;//曲线图2
                tolerancesGraph3.TolerancesEnabled = false;//曲线图3
                tolerancesGraph4.TolerancesEnabled = false;//曲线图4

                customButtonPreviousPage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【Previous Page】
                customButtonNextPage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【Next Page】
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：获取选中的曲线图控件对应的TolerancesControl_GraphData数组序号（若没有曲线图被选中，则值为-1）
        // 输入参数：无
        // 输出参数：无
        // 返回值：获取的选中的曲线图控件对应的TolerancesControl_GraphData数组序号（若没有曲线图被选中，则值为-1）
        //----------------------------------------------------------------------
        private int _GetGraphDataIndex()
        {
            int iRet = 0;//返回值

            if (tolerancesGraph1.ControlSelected)//选中
            {
                iRet = tolerancesGraph1.GraphDataIndex;//点击按钮之后，选中的曲线图控件对应的TolerancesControl_GraphData数组序号（若没有曲线图被选中，则值为-1）
            }
            else if (tolerancesGraph2.ControlSelected)//选中
            {
                iRet = tolerancesGraph2.GraphDataIndex;//点击按钮之后，选中的曲线图控件对应的TolerancesControl_GraphData数组序号（若没有曲线图被选中，则值为-1）
            }
            else if (tolerancesGraph3.ControlSelected)//选中
            {
                iRet = tolerancesGraph3.GraphDataIndex;//点击按钮之后，选中的曲线图控件对应的TolerancesControl_GraphData数组序号（若没有曲线图被选中，则值为-1）
            }
            else if (tolerancesGraph4.ControlSelected)//选中
            {
                iRet = tolerancesGraph4.GraphDataIndex;//点击按钮之后，选中的曲线图控件对应的TolerancesControl_GraphData数组序号（若没有曲线图被选中，则值为-1）
            }
            else//没有曲线图被选中
            {
                iRet = -1;
            }

            return iRet;
        }

        //----------------------------------------------------------------------
        // 功能说明：更新设置数值区域
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetSettingsValue()
        {
            if (iGraphDataIndex >= 0 && iGraphDataIndex < iGraphNumber_Total)//有效，选择了某一曲线图控件
            {
                labelUpper_SetValue.Text = GraphData[iGraphDataIndex].TolerancesGraph.Graph_YAxis.AxisEffectiveMax_Value.ToString();//赋值
                labelLower_SetValue.Text = GraphData[iGraphDataIndex].TolerancesGraph.Graph_YAxis.AxisEffectiveMin_Value.ToString();//赋值

                //

                if (GraphData[iGraphDataIndex].TolerancesGraph.RunorStop && (GraphData[iGraphDataIndex].TolerancesGraph.CurrentValueIndex[0] >= 0 && GraphData[iGraphDataIndex].TolerancesGraph.CurrentValueIndex[0] < GraphData[iGraphDataIndex].TolerancesGraph.ValueNumber_Curve))//曲线有效
                {
                    customButtonMeanValue.Chinese_TextDisplay = new String[1] { camera.Tolerances.GraphData[iGraphDataIndex].TolerancesGraphDataValue.MeanValue.ToString() };//设置显示的文本
                    customButtonMeanValue.English_TextDisplay = new String[1] { camera.Tolerances.GraphData[iGraphDataIndex].TolerancesGraphDataValue.MeanValue.ToString() };//设置显示的文本
                }
                else//曲线无效
                {
                    customButtonMeanValue.Chinese_TextDisplay = new String[1] { "--" };//设置显示的文本
                    customButtonMeanValue.English_TextDisplay = new String[1] { "--" };//设置显示的文本
                }

                if (GraphData[iGraphDataIndex].TolerancesGraph.Control_Data.TolerancesTools.Learned)//已学习
                {
                    customButtonLearnValue.Chinese_TextDisplay = new String[1] { GraphData[iGraphDataIndex].TolerancesGraph.Control_Data.TolerancesTools.LearnedValue.ToString() };//设置显示的文本
                    customButtonLearnValue.English_TextDisplay = new String[1] { GraphData[iGraphDataIndex].TolerancesGraph.Control_Data.TolerancesTools.LearnedValue.ToString() };//设置显示的文本
                }
                else//未学习
                {
                    customButtonLearnValue.Chinese_TextDisplay = new String[1] { "--" };//设置显示的文本
                    customButtonLearnValue.English_TextDisplay = new String[1] { "--" };//设置显示的文本
                }

                //

                if (camera.Tolerances.GraphData[iGraphDataIndex].EffectiveMax_ReadOnly)//
                {
                    customButtonUpper_Plus1.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【+1】（Lower）
                    customButtonUpper_Subtract1.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【-1】（Lower）
                    customButtonUpper_Plus5.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【+5】（Lower）
                    customButtonUpper_Subtract5.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【-5】（Lower）
                    customButtonUpper_Set.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【Set】（Lower）
                }
                else//
                {
                    customButtonUpper_Plus1.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【+1】（Lower）
                    customButtonUpper_Subtract1.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【-1】（Lower）
                    customButtonUpper_Plus5.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【+5】（Lower）
                    customButtonUpper_Subtract5.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【-5】（Lower）
                    customButtonUpper_Set.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【Set】（Lower）
                }

                if (camera.Tolerances.GraphData[iGraphDataIndex].EffectiveMin_ReadOnly)//
                {
                    customButtonLower_Plus1.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【+1】（Lower）
                    customButtonLower_Subtract1.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【-1】（Lower）
                    customButtonLower_Plus5.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【+5】（Lower）
                    customButtonLower_Subtract5.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【-5】（Lower）
                    customButtonLower_Set.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【Set】（Lower）
                } 
                else//
                {
                    customButtonLower_Plus1.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【+1】（Lower）
                    customButtonLower_Subtract1.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【-1】（Lower）
                    customButtonLower_Plus5.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【+5】（Lower）
                    customButtonLower_Subtract5.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【-5】（Lower）
                    customButtonLower_Set.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【Set】（Lower）
                }

                if (camera.Tolerances.GraphData[iGraphDataIndex].ButtonLearningShow)//
                {
                    customButtonLearnValue_Text.Visible = true;
                    customButtonLearnValue.Visible = true;
                }
                else//
                {
                    customButtonLearnValue_Text.Visible = false;
                    customButtonLearnValue.Visible = false;
                }
            }
            else//无效，即未选择任何曲线图控件
            {
                labelUpper_SetValue.Text = "--";//赋值
                labelLower_SetValue.Text = "--";//赋值

                customButtonMeanValue.Chinese_TextDisplay = new String[1] { "--" };//设置显示的文本
                customButtonMeanValue.English_TextDisplay = new String[1] { "--" };//设置显示的文本

                customButtonLearnValue_Text.Visible = false;
                customButtonLearnValue.Visible = false;

                //

                customButtonUpper_Plus1.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【+1】（Upper）
                customButtonLower_Plus1.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【+1】（Lower）
                customButtonUpper_Subtract1.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【-1】（Upper）
                customButtonLower_Subtract1.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【-1】（Lower）
                customButtonUpper_Plus5.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【+5】（Upper）
                customButtonLower_Plus5.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【+5】（Lower）
                customButtonUpper_Subtract5.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【-5】（Upper）
                customButtonLower_Subtract5.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【-5】（Lower）
                customButtonUpper_Set.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【Set】（Upper）
                customButtonLower_Set.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【Set】（Lower）
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击曲线图控件上的【运行/停止】按钮时进行相关操作
        // 输入参数：1.tolerancesGraph：曲线图控件
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _ClickRunStop(Tolerances tolerancesGraph)
        {
            Int32 i = 0;//循环控制变量
            Int32 j = 0;//循环控制变量
            Boolean bValid = false;//至少一个工具有效。取值范围：true，是；false，否

            for (i = 0; i < camera.Tools.Count; i++)//至少有一个工具选择
            {
                for (j = 0; j < GraphData.Length; j++)
                {
                    if (i == GraphData[j].TolerancesGraph.Control_Data.TolerancesTools.ToolsSign)//存在公差
                    {
                        if (GraphData[j].TolerancesGraph.Control_Data.TolerancesTools.ToolsValue)//开启
                        {
                            bValid = true;
                        }

                        //

                        break;
                    }
                }
                if (j >= GraphData.Length)//不存在公差
                {
                    if (camera.Tools[i].ToolState)//开启
                    {
                        bValid = true;

                        //

                        break;
                    }
                }

                //

                if (bValid)//至少一个工具有效
                {
                    break;
                }
            }

            if (bValid)//至少一个工具有效
            {
                //选中控件

                if (tolerancesGraph.RunorStop)//运行（点击该控件的【运行/停止】按钮之前为禁止状态）
                {
                    //若当前页中其它曲线图控件均未处于运行状态，则将当前控件选中，并更新设置数值区域

                    for (i = 0; i < iGraphNumber_Total; i++)//遍历所有曲线图控件
                    {
                        if (GraphData[i].Page == iCurrentPage)//当前页                      
                        {
                            if (i != tolerancesGraph.GraphDataIndex)//非当前控件
                            {
                                if (!(GraphData[i].TolerancesGraph.RunorStop))//符合要求
                                {
                                    //继续查找
                                }
                                else//不符合要求
                                {
                                    break;
                                }
                            }
                        }
                    }
                    if (i >= iGraphNumber_Total)//当前页中其它曲线图控件均未处于运行状态
                    {
                        tolerancesGraph.ControlSelected = true;//选中

                        iGraphDataIndex = tolerancesGraph.GraphDataIndex;
                    }
                    else//当前页中存在处于运行状态的其它曲线图控件
                    {
                        //不执行操作
                    }
                }
                else//禁止（点击该控件的【运行/停止】按钮之前为运行状态）
                {
                    //将当前页中第一个处于运行状态的曲线图控件选中，并更新设置数值区域

                    for (i = 0; i < iGraphNumber_Total; i++)//遍历所有曲线图控件
                    {
                        if (GraphData[i].Page == iCurrentPage)//当前页
                        {
                            if (GraphData[i].TolerancesGraph.RunorStop)//运行
                            {
                                //选中控件

                                if (GraphData[i].TolerancesGraph.Control_Data.TolerancesTools.ToolsSign == tolerancesGraph1.ToolsSign)
                                {
                                    tolerancesGraph1.ControlSelected = true;//选中
                                }
                                else if (GraphData[i].TolerancesGraph.Control_Data.TolerancesTools.ToolsSign == tolerancesGraph2.ToolsSign)//曲线图控件2
                                {
                                    tolerancesGraph2.ControlSelected = true;//选中
                                }
                                else if (GraphData[i].TolerancesGraph.Control_Data.TolerancesTools.ToolsSign == tolerancesGraph3.ToolsSign)//曲线图控件3
                                {
                                    tolerancesGraph3.ControlSelected = true;//选中
                                }
                                else//曲线图控件4
                                {
                                    tolerancesGraph4.ControlSelected = true;//选中
                                }

                                break;
                            }
                        }
                    }
                    if (i < iGraphNumber_Total)//选中了一个曲线图控件
                    {
                        iGraphDataIndex = i;
                    }
                    else//未选中任何一个曲线图控件
                    {
                        iGraphDataIndex = -1;
                    }
                }

                //更新设置数值区域

                _SetSettingsValue();

                //更新【Save Product】按钮背景

                bSaveProduct = true;

                customButtonSaveProduct.BitmapWhole = VisionSystemControlLibrary.Properties.Resources.Button_1;

                customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;

                //事件

                if (null != RunStop_Click)//有效
                {
                    CustomEventArgs customEventArgs = new CustomEventArgs();//参数

                    customEventArgs.IntValue[0] = tolerancesGraph.GraphDataIndex;//点击的按钮所在的曲线图控件对应的TolerancesControl_GraphData数组序号

                    RunStop_Click(this, customEventArgs);
                }
            }
            else//所有工具都无效
            {
                tolerancesGraph.RunorStop = !(tolerancesGraph.RunorStop);
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：启动学习
        // 输入参数：1.bSuccess：学习是否成功。取值范围：true，是；false，否
        //         2.iLearnedValue：学习数值
        //         3.iValidValue：学习中的有效数值数量
        //         4.iNonvalidValue：学习中的无效数值数量
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _Learn(Boolean bSuccess, Int16 iLearnedValue, UInt16 iValidValue, UInt16 iNonvalidValue)
        {
            string sLearnedValue = iLearnedValue.ToString();//学习数值文本
            string sValidValue = ((Int32)((float)iValidValue / (float)(iValidValue + iNonvalidValue) * 100)).ToString() + " %";//学习中的有效数值数量文本
            string sNonvalidValue = ((Int32)((float)iNonvalidValue / (float)(iValidValue + iNonvalidValue) * 100)).ToString() + " %";//学习中的无效数值数量文本

            if (bSuccess)//成功
            {
                if (tolerancesGraph1.ToolsSign == iSign_Learn)//曲线图1
                {
                    tolerancesGraph1.Control_Graph.Control_Data.TolerancesTools.Learned = true;//是否经过学习。true：是；false：否
                    tolerancesGraph1.Control_Graph.Control_Data.TolerancesTools.LearnedValue = iLearnedValue;//学习数值
                    tolerancesGraph1.Control_Graph.Control_Data.TolerancesTools.ValidValue = iValidValue;//学习中的有效数值数量
                    tolerancesGraph1.Control_Graph.Control_Data.TolerancesTools.NonvalidValue = iNonvalidValue;//学习中的无效数值数量

                    //更新曲线图的选择

                    _UnselectAllTolerancesGraph();//取消选择所有的曲线图控件

                    tolerancesGraph1.ControlSelected = true;//选中

                    //更新当前曲线图数据索引值

                    iGraphDataIndex = tolerancesGraph1.GraphDataIndex;
                }
                else if (tolerancesGraph2.ToolsSign == iSign_Learn)//曲线图2
                {
                    tolerancesGraph2.Control_Graph.Control_Data.TolerancesTools.Learned = true;//是否经过学习。true：是；false：否
                    tolerancesGraph2.Control_Graph.Control_Data.TolerancesTools.LearnedValue = iLearnedValue;//学习数值
                    tolerancesGraph2.Control_Graph.Control_Data.TolerancesTools.ValidValue = iValidValue;//学习中的有效数值数量
                    tolerancesGraph2.Control_Graph.Control_Data.TolerancesTools.NonvalidValue = iNonvalidValue;//学习中的无效数值数量

                    //更新曲线图的选择

                    _UnselectAllTolerancesGraph();//取消选择所有的曲线图控件

                    tolerancesGraph2.ControlSelected = true;//选中

                    //更新当前曲线图数据索引值

                    iGraphDataIndex = tolerancesGraph2.GraphDataIndex;
                }
                else if (tolerancesGraph3.ToolsSign == iSign_Learn)//曲线图3
                {
                    tolerancesGraph3.Control_Graph.Control_Data.TolerancesTools.Learned = true;//是否经过学习。true：是；false：否
                    tolerancesGraph3.Control_Graph.Control_Data.TolerancesTools.LearnedValue = iLearnedValue;//学习数值
                    tolerancesGraph3.Control_Graph.Control_Data.TolerancesTools.ValidValue = iValidValue;//学习中的有效数值数量
                    tolerancesGraph3.Control_Graph.Control_Data.TolerancesTools.NonvalidValue = iNonvalidValue;//学习中的无效数值数量

                    //更新曲线图的选择

                    _UnselectAllTolerancesGraph();//取消选择所有的曲线图控件

                    tolerancesGraph3.ControlSelected = true;//选中

                    //更新当前曲线图数据索引值

                    iGraphDataIndex = tolerancesGraph3.GraphDataIndex;
                }
                else//曲线图4
                {
                    tolerancesGraph4.Control_Graph.Control_Data.TolerancesTools.Learned = true;//是否经过学习。true：是；false：否
                    tolerancesGraph4.Control_Graph.Control_Data.TolerancesTools.LearnedValue = iLearnedValue;//学习数值
                    tolerancesGraph4.Control_Graph.Control_Data.TolerancesTools.ValidValue = iValidValue;//学习中的有效数值数量
                    tolerancesGraph4.Control_Graph.Control_Data.TolerancesTools.NonvalidValue = iNonvalidValue;//学习中的无效数值数量

                    //更新曲线图的选择

                    _UnselectAllTolerancesGraph();//取消选择所有的曲线图控件

                    tolerancesGraph4.ControlSelected = true;//选中

                    //更新当前曲线图数据索引值

                    iGraphDataIndex = tolerancesGraph4.GraphDataIndex;
                }

                //更新设置数值区域

                _SetSettingsValue();

                //

                bSaveProduct = true;

                customButtonSaveProduct.BitmapWhole = VisionSystemControlLibrary.Properties.Resources.Button_1;

                customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;

                //显示信息对话框

                GlobalWindows.MessageDisplay_Window.WindowParameter = 62;//窗口特征数值
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_1 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_1 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][1] + "：" + sValidValue;
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][1] + "：" + sValidValue;
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][2] + "：" + sNonvalidValue;
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][2] + "：" + sNonvalidValue;
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][3] + "：" + sLearnedValue;
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][3] + "：" + sLearnedValue;

                GlobalWindows.MessageDisplay_Window.StartPosition = FormStartPosition.CenterScreen;
                if (GlobalWindows.TopMostWindows)//置顶
                {
                    GlobalWindows.MessageDisplay_Window.TopMost = true;//将窗口置于顶层
                }
                else//其它
                {
                    GlobalWindows.MessageDisplay_Window.Visible = true;//显示
                }
            } 
            else//失败
            {
                //显示信息对话框

                GlobalWindows.MessageDisplay_Window.WindowParameter = 63;//窗口特征数值
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][4];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][4];

                GlobalWindows.MessageDisplay_Window.StartPosition = FormStartPosition.CenterScreen;
                if (GlobalWindows.TopMostWindows)//置顶
                {
                    GlobalWindows.MessageDisplay_Window.TopMost = true;//将窗口置于顶层
                }
                else//其它
                {
                    GlobalWindows.MessageDisplay_Window.Visible = true;//显示
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：保存数据
        // 输入参数：1.bSuccess：保存是否成功。取值范围：true，是；false，否
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _SaveProduct(Boolean bSuccess)
        {
            if (bSuccess)//成功
            {
                int i = 0;//循环控制变量

                for (i = 0; i < GraphNumber_Total; i++)//各个曲线图赋值
                {
                    //工具属性

                    camera.Tools[camera.Tolerances.GraphData[i].ToolsIndex].ToolState = GraphData[i].TolerancesGraph.Control_Data.TolerancesTools.ToolsValue;//工具，数值，表示该工具的使能状态。取值范围：true，使能；false，禁止。（默认值：true）

                    camera.Tools[camera.Tolerances.GraphData[i].ToolsIndex].Learned = GraphData[i].TolerancesGraph.Control_Data.TolerancesTools.Learned;//是否经过学习。true：是；false：否（默认值：false）
                    camera.Tools[camera.Tolerances.GraphData[i].ToolsIndex].LearnedValue = GraphData[i].TolerancesGraph.Control_Data.TolerancesTools.LearnedValue;//学习数值（默认值：100）
                    camera.Tools[camera.Tolerances.GraphData[i].ToolsIndex].ValidValue = GraphData[i].TolerancesGraph.Control_Data.TolerancesTools.ValidValue;//学习中的有效数值数量（默认值：100）
                    camera.Tools[camera.Tolerances.GraphData[i].ToolsIndex].NonvalidValue = GraphData[i].TolerancesGraph.Control_Data.TolerancesTools.NonvalidValue;//学习中的无效数值数量（默认值：0）
                    
                    camera.Tolerances.GraphData[i].EffectiveMin_Value = (Int32)(GraphData[i].TolerancesGraph.Graph_YAxis.AxisEffectiveMin_Value);//坐标轴最小有效实际数值（默认值为：300）
                    camera.Tolerances.GraphData[i].EffectiveMax_Value = (Int32)(GraphData[i].TolerancesGraph.Graph_YAxis.AxisEffectiveMax_Value);//坐标轴最大有效实际数值（默认值为：700）
                    camera.Tolerances.GraphData[i].TolerancesGraphDataValue.AdditionalValue = GraphData[i].TolerancesGraph.Graph_YAxis.AxisAdditionalValue;//曲线图坐标轴上显示的附加数值（根据该值得到sAxisAdditionalValue数值）（默认值为：10）

                    //

                    camera.Tools[camera.Tolerances.GraphData[i].ToolsIndex].Min = (Int32)(GraphData[i].TolerancesGraph.Graph_YAxis.AxisEffectiveMin_Value);
                    camera.Tools[camera.Tolerances.GraphData[i].ToolsIndex].Max = (Int32)(GraphData[i].TolerancesGraph.Graph_YAxis.AxisEffectiveMax_Value);
                }

                for (i = 0; i < camera.Tools.Count; i++)//保存斜率
                {
                    camera.Tools[i].Precision = iPrecision[i];
                }

                //

                camera.Tolerances.EjectLevel = Convert.ToInt16(iEjectLevel);//灵敏度

                //

                camera._SaveTolerances();//保存文件
                camera._SaveTool();//保存文件

                File.Copy(camera.DataPath + VisionSystemClassLibrary.Class.Camera.TolerancesFileName, brand.BrandPath + brand.CURRENTBrandName + "\\" + camera.CameraENGName + "\\" + VisionSystemClassLibrary.Class.Camera.TolerancesFileName, true);
                File.Copy(camera.DataPath + VisionSystemClassLibrary.Class.Camera.ToolFileName, brand.BrandPath + brand.CURRENTBrandName + "\\" + camera.CameraENGName + "\\" + VisionSystemClassLibrary.Class.Camera.ToolFileName, true);

                //VisionSystemClassLibrary.Class.System.FileCopyFun(camera.DataPath + VisionSystemClassLibrary.Class.Camera.TolerancesFileName, brand.BrandPath + brand.CURRENTBrandName + "\\" + camera.CameraENGName + "\\" + VisionSystemClassLibrary.Class.Camera.TolerancesFileName);
                //VisionSystemClassLibrary.Class.System.FileCopyFun(camera.DataPath + VisionSystemClassLibrary.Class.Camera.ToolFileName, brand.BrandPath + brand.CURRENTBrandName + "\\" + camera.CameraENGName + "\\" + VisionSystemClassLibrary.Class.Camera.ToolFileName);

                //

                bSaveProduct = false;//曲线图未被修改

                customButtonSaveProduct.BitmapWhole = VisionSystemControlLibrary.Properties.Resources.Button;

                customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;

                //显示信息对话框

                GlobalWindows.MessageDisplay_Window.WindowParameter = 65;//窗口特征数值
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][5];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][5];

                GlobalWindows.MessageDisplay_Window.StartPosition = FormStartPosition.CenterScreen;
                if (GlobalWindows.TopMostWindows)//置顶
                {
                    GlobalWindows.MessageDisplay_Window.TopMost = true;//将窗口置于顶层
                }
                else//其它
                {
                    GlobalWindows.MessageDisplay_Window.Visible = true;//显示
                }
            } 
            else//失败
            {
                //显示信息对话框

                GlobalWindows.MessageDisplay_Window.WindowParameter = 66;//窗口特征数值
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][6];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][6];

                GlobalWindows.MessageDisplay_Window.StartPosition = FormStartPosition.CenterScreen;
                if (GlobalWindows.TopMostWindows)//置顶
                {
                    GlobalWindows.MessageDisplay_Window.TopMost = true;//将窗口置于顶层
                }
                else//其它
                {
                    GlobalWindows.MessageDisplay_Window.Visible = true;//显示
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：复位曲线图
        // 输入参数：1.bSuccess：复位是否成功。取值范围：true，是；false，否
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _ResetGraphs(bool bSuccess)
        {
            //显示信息对话框

            if (bSuccess)//成功
            {
                GlobalWindows.MessageDisplay_Window.WindowParameter = 68;//窗口特征数值
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][7];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][7];
            }
            else//失败
            {
                GlobalWindows.MessageDisplay_Window.WindowParameter = 69;//窗口特征数值
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][8];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][8];
            }

            GlobalWindows.MessageDisplay_Window.StartPosition = FormStartPosition.CenterScreen;
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.MessageDisplay_Window.TopMost = true;//将窗口置于顶层
            }
            else//其它
            {
                GlobalWindows.MessageDisplay_Window.Visible = true;//显示
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【Previous Page】或【Next Page】按钮时进行相关操作
        // 输入参数：1.bPreviousNext：点击的按钮的类型。取值范围：true：【Previous Page】按钮；【Next Page】按钮
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _ClickPage(bool bPreviousNext)
        {
            //更新页码

            if (bPreviousNext)//点击了【Previous Page】按钮
            {
                if (iCurrentPage > 0)//非首页
                {
                    iCurrentPage--;//更新页码

                    if (!(customButtonNextPage.Visible))//未显示【Next Page】按钮
                    {
                        customButtonNextPage.Visible = true;//显示【Next Page】按钮
                    }

                    if (0 == iCurrentPage)//首页
                    {
                        customButtonPreviousPage.Visible = false;//隐藏【Previous Page】按钮
                    }
                }
            }
            else//点击了【Next Page】按钮
            {
                if (iCurrentPage < iTotalPage - 1)//非末页
                {
                    iCurrentPage++;//更新页码

                    if (!(customButtonPreviousPage.Visible))//未显示【Previous Page】按钮
                    {
                        customButtonPreviousPage.Visible = true;//显示【Previous Page】按钮
                    }

                    if (iTotalPage - 1 == iCurrentPage)//末页
                    {
                        customButtonNextPage.Visible = false;//隐藏【Next Page】按钮
                    }
                }
            }

            //设置页面曲线图和控件

            _SetPage();
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【Previous Page】或【Next Page】按钮，应用模式时控件初始化时调用本函数，设置页面曲线图和控件
        // 输入参数：1.bPreviousNext：点击的按钮的类型。取值范围：true：【Previous Page】按钮；【Next Page】按钮
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetPage()
        {
            //取消选择所有的曲线图控件

            _UnselectAllTolerancesGraph();

            //更新新的页中的曲线图

            int i = 0;//循环控制变量
            int j = 0;//循环控制变量
            bool bSelected = false;//获取新页中第一个处于运行状态的曲线图控件时使用。取值范围：true，已经获取了第一个处于运行状态的曲线图控件；false，未获取第一个处于运行状态的曲线图控件

            for (i = 0; i < iGraphNumber_Total; i++)//遍历所有曲线图控件
            {
                if (GraphData[i].Page == iCurrentPage)//当前页中的曲线图控件
                {
                    if (!bSelected)//未获取第一个处于运行状态的曲线图控件
                    {
                        if (GraphData[i].TolerancesGraph.RunorStop)//运行
                        {
                            bSelected = true;//已经获取了第一个处于运行状态的曲线图控件

                            GraphData[i].TolerancesGraph.ControlSelected = true;//选中

                            iGraphDataIndex = i;
                        }
                    }

                    if (0 == j)//曲线图1
                    {
                        tolerancesGraph1.GraphDataIndex = i;//控件对应的曲线图数据索引（从0开始），即GraphData数组的序号值

                        tolerancesGraph1._Set(GraphData[i]);//设置曲线图控件1数据

                        tolerancesGraph1.Visible = true;//显示曲线图控件1
                    }
                    else if (1 == j)//曲线图2
                    {
                        tolerancesGraph2.GraphDataIndex = i;//控件对应的曲线图数据索引（从0开始），即GraphData数组的序号值

                        tolerancesGraph2._Set(GraphData[i]);//设置曲线图控件1数据

                        tolerancesGraph2.Visible = true;//显示曲线图控件1
                    }
                    else if (2 == j)//曲线图3
                    {
                        tolerancesGraph3.GraphDataIndex = i;//控件对应的曲线图数据索引（从0开始），即GraphData数组的序号值

                        tolerancesGraph3._Set(GraphData[i]);//设置曲线图控件1数据

                        tolerancesGraph3.Visible = true;//显示曲线图控件1
                    }
                    else//3，曲线图4
                    {
                        tolerancesGraph4.GraphDataIndex = i;//控件对应的曲线图数据索引（从0开始），即GraphData数组的序号值

                        tolerancesGraph4._Set(GraphData[i]);//设置曲线图控件1数据

                        tolerancesGraph4.Visible = true;//显示曲线图控件1
                    }

                    //

                    j++;
                }
            }

            //判断是否所有曲线图控件均处于停止状态

            if (!bSelected)//所有的曲线图控件均处于停止状态
            {
                iGraphDataIndex = -1;
            }

            //隐藏不显示的曲线图控件（不存在0 == j，即页面中不包含任何一个曲线图的情况）

            if (1 == j)//隐藏曲线图2，3，4
            {
                tolerancesGraph2.Visible = false;//隐藏曲线图控件2
                tolerancesGraph3.Visible = false;//隐藏曲线图控件3
                tolerancesGraph4.Visible = false;//隐藏曲线图控件4
            }
            else if (2 == j)//隐藏曲线图3，4
            {
                tolerancesGraph3.Visible = false;//隐藏曲线图控件3
                tolerancesGraph4.Visible = false;//隐藏曲线图控件4
            }
            else if (3 == j)//3，隐藏曲线图4
            {
                tolerancesGraph4.Visible = false;//隐藏曲线图控件4
            }
            else//其它
            {
                //不执行操作
            }

            //更新设置数值区域

            _SetSettingsValue();
        }

        //----------------------------------------------------------------------
        // 功能说明：取消选择所有的曲线图控件
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _UnselectAllTolerancesGraph()
        {
            tolerancesGraph1.ControlSelected = false;//曲线图1
            tolerancesGraph2.ControlSelected = false;//曲线图2
            tolerancesGraph3.ControlSelected = false;//曲线图3
            tolerancesGraph4.ControlSelected = false;//曲线图4
        }

        //----------------------------------------------------------------------
        // 功能说明：获取当前选中的曲线图，更改当前选中的曲线图的Y轴的有效值时，调用本函数配置Y轴坐标和曲线图，并更新绘制区域
        // 输入参数：1.bUpperLower：设置的Y轴数据类型。取值范围：true：Y轴最大（有效）值；false：Y轴最小（有效）值
        //         2.bNewValue：设置的数值的类型。取值范围：true：参数iValue为一个新的数值；false：参数iValue为一个需要在当前值上累加的数值
        //         3.iValue：设置的数值
        //         4.iButtonStyle：点击的按钮类型。取值范围：1.【+1】按钮；2.【-1】按钮；3.【+5】按钮；4.【-5】按钮；5.【Set】按钮
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetTolerancesGraph(bool bUpperLower, bool bNewValue, int iValue, int iButtonStyle)
        {
            if (tolerancesGraph1.ControlSelected)//曲线图1
            {
                _SetTolerancesGraph(bUpperLower, bNewValue, iValue, tolerancesGraph1, iButtonStyle);
            }
            else if (tolerancesGraph2.ControlSelected)//曲线图2
            {
                _SetTolerancesGraph(bUpperLower, bNewValue, iValue, tolerancesGraph2, iButtonStyle);
            }
            else if (tolerancesGraph3.ControlSelected)//曲线图3
            {
                _SetTolerancesGraph(bUpperLower, bNewValue, iValue, tolerancesGraph3, iButtonStyle);
            }
            else//曲线图4
            {
                _SetTolerancesGraph(bUpperLower, bNewValue, iValue, tolerancesGraph4, iButtonStyle);
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：更改当前选中的曲线图的Y轴的有效值时，调用本函数配置Y轴坐标和曲线图，并更新绘制区域
        // 输入参数：1.bUpperLower：设置的Y轴数据类型。取值范围：true：Y轴最大（有效）值；false：Y轴最小（有效）值
        //         2.bNewValue：设置的数值的类型。取值范围：true：参数iValue为一个新的数值；false：参数iValue为一个需要在当前值上累加的数值
        //         3.iValue：设置的数值
        //         4.TolerancesGraph：待设置曲线图
        //         5.iButtonStyle：点击的按钮类型。取值范围：1.【+1】按钮；2.【-1】按钮；3.【+5】按钮；4.【-5】按钮；5.【Set】按钮
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetTolerancesGraph(bool bUpperLower, bool bNewValue, int iValue, Tolerances tolerancesGraph, int iButtonStyle)
        {
            //检查待设置的数值，若符合要求，则应用设置的数值

            if (_CheckTolerancesGraph(tolerancesGraph, bUpperLower, bNewValue, iValue))//符合要求并应用数值
            {
                //配置Y轴坐标和曲线图，并更新绘制区域

                tolerancesGraph._SetValue();

                //更新【Save Product】按钮背景

                bSaveProduct = true;

                customButtonSaveProduct.BitmapWhole = VisionSystemControlLibrary.Properties.Resources.Button_1;

                customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;

                //事件

                if (null != SetGraphValueSuccess)//有效
                {
                    CustomEventArgs customEventArgs = new CustomEventArgs();//参数

                    customEventArgs.IntValue[0] = _GetGraphDataIndex();//当前选中的曲线图控件对应的TolerancesControl_GraphData数组序号
                    customEventArgs.IntValue[1] = iButtonStyle;//点击的按钮类型。取值范围：1.【+1】按钮；2.【-1】按钮；3.【+5】按钮；4.【-5】按钮；5.【Set】按钮

                    SetGraphValueSuccess(this, customEventArgs);
                }
            }
            else//不符合要求
            {
                //事件

                if (null != SetGraphValueFailure)//有效
                {
                    CustomEventArgs customEventArgs = new CustomEventArgs();//参数

                    customEventArgs.IntValue[0] = _GetGraphDataIndex();//当前选中的曲线图控件对应的TolerancesControl_GraphData数组序号
                    customEventArgs.IntValue[1] = iButtonStyle;//点击的按钮类型。取值范围：1.【+1】按钮；2.【-1】按钮；3.【+5】按钮；4.【-5】按钮；5.【Set】按钮

                    SetGraphValueFailure(this, customEventArgs);
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：更改当前选中的曲线图的Y轴的有效值时，调用本函数检查设置的数值是否符合要求，若符合要求，则应用设置的数值
        // 输入参数：1.tolerancesGraph：待设置曲线图
        //         2.bUpperLower：设置的Y轴数据类型。取值范围：true：Y轴最大（有效）值；false：Y轴最小（有效）值
        //         3.bNewValue：设置的数值的类型。取值范围：true：参数iValue为一个新的数值；false：参数iValue为一个需要在当前值上累加的数值
        //         4.iValue：设置的数值
        // 输出参数：无
        // 返回值：true：设置的数值符合要求；false：设置的数值不符合要求
        //----------------------------------------------------------------------
        private bool _CheckTolerancesGraph(Tolerances tolerancesGraph, bool bUpperLower, bool bNewValue, int iValue)
        {
            bool bRet = true;//返回值
            int iNewValue = 0;//待设置的新的数值，临时变量

            if (bUpperLower)//Y轴最大（有效）值
            {
                //获取待设置的数值

                if (bNewValue)//参数iValue为一个新的数值
                {
                    iNewValue = iValue;
                }
                else//参数iValue为一个需要在当前值上累加的数值
                {
                    iNewValue = (int)(tolerancesGraph.YAxisEffectiveMaxValue) + iValue;
                }

                //判断设置的数值是否符合要求

                if (iNewValue >= tolerancesGraph.Control_Graph.Control_Data.TolerancesTools.MinValue && iNewValue <= tolerancesGraph.Control_Graph.Control_Data.TolerancesTools.MaxValue)//符合要求
                {
                    if (iNewValue >= tolerancesGraph.YAxisEffectiveMinValue)//符合要求
                    {
                        tolerancesGraph.YAxisEffectiveMaxValue = iNewValue;

                        tolerancesGraph.YAxisAdditionalValue = VisionSystemClassLibrary.Class.TolerancesData._SetAdditionalValue((Int16)tolerancesGraph.YAxisEffectiveMinValue, (Int16)tolerancesGraph.YAxisEffectiveMaxValue, tolerancesGraph.YAxisAdditionalValueRatio);

                        //

                        labelUpper_SetValue.Text = iNewValue.ToString();//赋值

                        if (camera.Tools[camera.Tolerances.GraphData[iGraphDataIndex].ToolsIndex].Type == 7)//更改公差下限，且执行烟支检测
                        {
                            camera.Tolerances._GetPrecision_Tobacco_D((Int16)iEjectLevel, iNewValue, camera.Tools[camera.Tolerances.GraphData[iGraphDataIndex].ToolsIndex].EjectPixelMin, ref iPrecision[camera.Tolerances.GraphData[iGraphDataIndex].ToolsIndex]);
                        }
                    }
                    else//不符合要求
                    {
                        bRet = false;
                    }
                }
                else//不符合要求
                {
                    bRet = false;
                }
            }
            else//Y轴最小（有效）值
            {
                //获取待设置的数值

                if (bNewValue)//参数iValue为一个新的数值
                {
                    iNewValue = iValue;
                }
                else//参数iValue为一个需要在当前值上累加的数值
                {
                    iNewValue = (int)(tolerancesGraph.YAxisEffectiveMinValue) + iValue;
                }

                //判断设置的数值是否符合要求

                if (iNewValue >= tolerancesGraph.Control_Graph.Control_Data.TolerancesTools.MinValue && iNewValue <= tolerancesGraph.Control_Graph.Control_Data.TolerancesTools.MaxValue)//符合要求
                {
                    if (iNewValue <= tolerancesGraph.YAxisEffectiveMaxValue)//符合要求
                    {
                        tolerancesGraph.YAxisEffectiveMinValue = iNewValue;

                        tolerancesGraph.YAxisAdditionalValue = VisionSystemClassLibrary.Class.TolerancesData._SetAdditionalValue((Int16)tolerancesGraph.YAxisEffectiveMinValue, (Int16)tolerancesGraph.YAxisEffectiveMaxValue, tolerancesGraph.YAxisAdditionalValueRatio);

                        //

                        labelLower_SetValue.Text = iNewValue.ToString();//赋值

                        if (camera.Tools[camera.Tolerances.GraphData[iGraphDataIndex].ToolsIndex].Type == 3)//更改公差下限，且执行烟支检测
                        {
                            camera.Tolerances._GetPrecision((Int16)iEjectLevel, iNewValue, camera.Tools[camera.Tolerances.GraphData[iGraphDataIndex].ToolsIndex].EjectPixelMin, ref iPrecision[camera.Tolerances.GraphData[iGraphDataIndex].ToolsIndex]);
                        }
                    }
                    else//不符合要求
                    {
                        bRet = false;
                    }
                }
                else//不符合要求
                {
                    bRet = false;
                }
            }

            return bRet;
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【Set】按钮后，获取待设置的数值的当前值、能够设置的数值范围下限、能够升值的数值范围上限
        // 输入参数：1.bUpperLower：【Set】按钮的类型。取值范围：true，Upper（Y轴数值上限）；false，Lower（Y轴数值下限）
        // 输出参数：1.iMin：能够设置的数值范围下限
        //         2.iMax：能够设置的数值范围上限
        // 返回值：获取的待设置的数值的当前值
        //----------------------------------------------------------------------
        private int _GetSetValue(bool bUpperLower, ref int iMin, ref int iMax)
        {
            if (tolerancesGraph1.ControlSelected)//曲线图1
            {
                return _GetSetValue(tolerancesGraph1, bUpperLower, ref iMin, ref iMax);
            }
            else if (tolerancesGraph2.ControlSelected)//曲线图2
            {
                return _GetSetValue(tolerancesGraph2, bUpperLower, ref iMin, ref iMax);
            }
            else if (tolerancesGraph3.ControlSelected)//曲线图3
            {
                return _GetSetValue(tolerancesGraph3, bUpperLower, ref iMin, ref iMax);
            }
            else//曲线图4
            {
                return _GetSetValue(tolerancesGraph4, bUpperLower, ref iMin, ref iMax);
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【Set】按钮后，获取待设置的数值的当前值、能够设置的数值范围下限、能够升值的数值范围上限
        // 输入参数：1.tolerancesGraph：待设置曲线图
        //         2.bUpperLower：【Set】按钮的类型。取值范围：true，Upper（Y轴数值上限）；false，Lower（Y轴数值下限）
        // 输出参数：1.iMin：能够设置的数值范围下限
        //         2.iMax：能够设置的数值范围上限
        // 返回值：获取的待设置的数值的当前值
        //----------------------------------------------------------------------
        private int _GetSetValue(Tolerances tolerancesGraph, bool bUpperLower, ref int iMin, ref int iMax)
        {
            if (bUpperLower)//Y轴最大（有效）值
            {
                iMin = (int)(tolerancesGraph.YAxisEffectiveMinValue);
                iMax = tolerancesGraph.Control_Graph.Control_Data.TolerancesTools.MaxValue;

                return (int)(tolerancesGraph.YAxisEffectiveMaxValue);
            }
            else//Y轴最小（有效）值
            {
                iMin = tolerancesGraph.Control_Graph.Control_Data.TolerancesTools.MinValue;
                iMax = (int)(tolerancesGraph.YAxisEffectiveMaxValue);

                return (int)(tolerancesGraph.YAxisEffectiveMinValue);
            }
        }

        //事件

        //----------------------------------------------------------------------
        // 功能说明：控件加载事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void TolerancesControl_Load(object sender, EventArgs e)
        {
            //_SetDefault();//使用默认值设置并显示控件
        }

        //----------------------------------------------------------------------
        // 功能说明：双击曲线图控件1时产生的事件
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void tolerancesGraph1_Control_DoubleClick(object sender, EventArgs e)
        {
            CustomEventArgs customEventArgs = (CustomEventArgs)e;//参数

            if (0 == customEventArgs.IntValue[0])//表示该控件在双击时已经被选中
            {
                //获取双击之前选中的曲线图控件对应的TolerancesControl_GraphData数组序号

                customEventArgs.IntValue[1] = tolerancesGraph1.GraphDataIndex;//双击之前，选中的曲线图控件对应的TolerancesControl_GraphData数组序号
            }
            else//表示该控件在双击时未被选中
            {
                //获取双击之前选中的曲线图控件对应的TolerancesControl_GraphData数组序号

                if (tolerancesGraph2.ControlSelected)//选中
                {
                    customEventArgs.IntValue[1] = tolerancesGraph2.GraphDataIndex;//双击之前，选中的曲线图控件对应的TolerancesControl_GraphData数组序号
                }
                else if (tolerancesGraph3.ControlSelected)//选中
                {
                    customEventArgs.IntValue[1] = tolerancesGraph3.GraphDataIndex;//双击之前，选中的曲线图控件对应的TolerancesControl_GraphData数组序号
                }
                else//tolerancesGraph4.ControlSelected，选中
                {
                    customEventArgs.IntValue[1] = tolerancesGraph4.GraphDataIndex;//双击之前，选中的曲线图控件对应的TolerancesControl_GraphData数组序号
                }

                //更新当前控件

                _UnselectAllTolerancesGraph();//取消选择所有的曲线图控件

                tolerancesGraph1.ControlSelected = true;
            }

            //

            iGraphDataIndex = tolerancesGraph1.GraphDataIndex;

            //更新设置数值区域

            _SetSettingsValue();

            //获取双击之后选中的曲线图控件对应的TolerancesControl_GraphData数组序号

            customEventArgs.IntValue[0] = tolerancesGraph1.GraphDataIndex;//双击之后，选中的曲线图控件对应的TolerancesControl_GraphData数组序号

            //事件

            if (null != Control_DoubleClick)//有效
            {
                Control_DoubleClick(this, customEventArgs);
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：双击曲线图控件2时产生的事件
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void tolerancesGraph2_Control_DoubleClick(object sender, EventArgs e)
        {
            CustomEventArgs customEventArgs = (CustomEventArgs)e;//参数

            if (0 == customEventArgs.IntValue[0])//表示该控件在双击时已经被选中
            {
                //获取双击之前选中的曲线图控件对应的TolerancesControl_GraphData数组序号

                customEventArgs.IntValue[1] = tolerancesGraph2.GraphDataIndex;//双击之前，选中的曲线图控件对应的TolerancesControl_GraphData数组序号
            }
            else//表示该控件在双击时未被选中
            {
                //获取双击之前选中的曲线图控件对应的TolerancesControl_GraphData数组序号

                if (tolerancesGraph1.ControlSelected)//选中
                {
                    customEventArgs.IntValue[1] = tolerancesGraph1.GraphDataIndex;//双击之前，选中的曲线图控件对应的TolerancesControl_GraphData数组序号
                }
                else if (tolerancesGraph3.ControlSelected)//选中
                {
                    customEventArgs.IntValue[1] = tolerancesGraph3.GraphDataIndex;//双击之前，选中的曲线图控件对应的TolerancesControl_GraphData数组序号
                }
                else//tolerancesGraph4.ControlSelected，选中
                {
                    customEventArgs.IntValue[1] = tolerancesGraph4.GraphDataIndex;//双击之前，选中的曲线图控件对应的TolerancesControl_GraphData数组序号
                }

                //更新当前控件

                _UnselectAllTolerancesGraph();//取消选择所有的曲线图控件

                tolerancesGraph2.ControlSelected = true;
            }

            //

            iGraphDataIndex = tolerancesGraph2.GraphDataIndex;

            //更新设置数值区域

            _SetSettingsValue();

            //获取双击之后选中的曲线图控件对应的TolerancesControl_GraphData数组序号

            customEventArgs.IntValue[0] = tolerancesGraph2.GraphDataIndex;//双击之后，选中的曲线图控件对应的TolerancesControl_GraphData数组序号

            //事件

            if (null != Control_DoubleClick)//有效
            {
                Control_DoubleClick(this, customEventArgs);
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：双击曲线图控件3时产生的事件
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void tolerancesGraph3_Control_DoubleClick(object sender, EventArgs e)
        {
            CustomEventArgs customEventArgs = (CustomEventArgs)e;//参数

            if (0 == customEventArgs.IntValue[0])//表示该控件在双击时已经被选中
            {
                //获取双击之前选中的曲线图控件对应的TolerancesControl_GraphData数组序号

                customEventArgs.IntValue[1] = tolerancesGraph3.GraphDataIndex;//双击之前，选中的曲线图控件对应的TolerancesControl_GraphData数组序号
            }
            else//表示该控件在双击时未被选中
            {
                //获取双击之前选中的曲线图控件对应的TolerancesControl_GraphData数组序号

                if (tolerancesGraph1.ControlSelected)//选中
                {
                    customEventArgs.IntValue[1] = tolerancesGraph1.GraphDataIndex;//双击之前，选中的曲线图控件对应的TolerancesControl_GraphData数组序号
                }
                else if (tolerancesGraph2.ControlSelected)//选中
                {
                    customEventArgs.IntValue[1] = tolerancesGraph2.GraphDataIndex;//双击之前，选中的曲线图控件对应的TolerancesControl_GraphData数组序号
                }
                else//tolerancesGraph4.ControlSelected，选中
                {
                    customEventArgs.IntValue[1] = tolerancesGraph4.GraphDataIndex;//双击之前，选中的曲线图控件对应的TolerancesControl_GraphData数组序号
                }

                //更新当前控件

                _UnselectAllTolerancesGraph();//取消选择所有的曲线图控件

                tolerancesGraph3.ControlSelected = true;
            }

            //

            iGraphDataIndex = tolerancesGraph3.GraphDataIndex;

            //更新设置数值区域

            _SetSettingsValue();

            //获取双击之后选中的曲线图控件对应的TolerancesControl_GraphData数组序号

            customEventArgs.IntValue[0] = tolerancesGraph3.GraphDataIndex;//双击之后，选中的曲线图控件对应的TolerancesControl_GraphData数组序号

            //事件

            if (null != Control_DoubleClick)//有效
            {
                Control_DoubleClick(this, customEventArgs);
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：双击曲线图控件4时产生的事件
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void tolerancesGraph4_Control_DoubleClick(object sender, EventArgs e)
        {
            CustomEventArgs customEventArgs = (CustomEventArgs)e;//参数

            if (0 == customEventArgs.IntValue[0])//表示该控件在双击时已经被选中
            {
                //获取双击之前选中的曲线图控件对应的TolerancesControl_GraphData数组序号

                customEventArgs.IntValue[1] = tolerancesGraph4.GraphDataIndex;//双击之前，选中的曲线图控件对应的TolerancesControl_GraphData数组序号
            }
            else//表示该控件在双击时未被选中
            {
                //获取双击之前选中的曲线图控件对应的TolerancesControl_GraphData数组序号

                if (tolerancesGraph1.ControlSelected)//选中
                {
                    customEventArgs.IntValue[1] = tolerancesGraph1.GraphDataIndex;//双击之前，选中的曲线图控件对应的TolerancesControl_GraphData数组序号
                }
                else if (tolerancesGraph2.ControlSelected)//选中
                {
                    customEventArgs.IntValue[1] = tolerancesGraph2.GraphDataIndex;//双击之前，选中的曲线图控件对应的TolerancesControl_GraphData数组序号
                }
                else//tolerancesGraph3.ControlSelected，选中
                {
                    customEventArgs.IntValue[1] = tolerancesGraph3.GraphDataIndex;//双击之前，选中的曲线图控件对应的TolerancesControl_GraphData数组序号
                }

                //更新当前控件

                _UnselectAllTolerancesGraph();//取消选择所有的曲线图控件

                tolerancesGraph4.ControlSelected = true;
            }

            //

            iGraphDataIndex = tolerancesGraph4.GraphDataIndex;

            //更新设置数值区域

            _SetSettingsValue();

            //获取双击之后选中的曲线图控件对应的TolerancesControl_GraphData数组序号

            customEventArgs.IntValue[0] = tolerancesGraph4.GraphDataIndex;//双击之后，选中的曲线图控件对应的TolerancesControl_GraphData数组序号

            //事件

            if (null != Control_DoubleClick)//有效
            {
                Control_DoubleClick(this, customEventArgs);
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击曲线图控件1中的【运行/停止】按钮时产生的事件
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void tolerancesGraph1_RunStop_Click(object sender, EventArgs e)
        {
            _ClickRunStop(tolerancesGraph1);//点击【运行/停止】按钮后的操作
        }

        //----------------------------------------------------------------------
        // 功能说明：点击曲线图控件2中的【运行/停止】按钮时产生的事件
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void tolerancesGraph2_RunStop_Click(object sender, EventArgs e)
        {
            _ClickRunStop(tolerancesGraph2);//点击【运行/停止】按钮后的操作
        }

        //----------------------------------------------------------------------
        // 功能说明：点击曲线图控件3中的【运行/停止】按钮时产生的事件
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void tolerancesGraph3_RunStop_Click(object sender, EventArgs e)
        {
            _ClickRunStop(tolerancesGraph3);//点击【运行/停止】按钮后的操作
        }

        //----------------------------------------------------------------------
        // 功能说明：点击曲线图控件4中的【运行/停止】按钮时产生的事件
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void tolerancesGraph4_RunStop_Click(object sender, EventArgs e)
        {
            _ClickRunStop(tolerancesGraph4);//点击【运行/停止】按钮后的操作
        }

        //----------------------------------------------------------------------
        // 功能说明：点击曲线图控件1中的【学习】按钮时产生的事件
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void tolerancesGraph1_Learning_Click(object sender, EventArgs e)
        {
            if (0 <= tolerancesGraph1.Control_Graph.CurrentValueIndex[0])//有效，可以学习
            {
                iSign_Learn = tolerancesGraph1.ToolsSign;//存储曲线图特征数据，在启动学习时使用

                //显示信息对话框

                GlobalWindows.MessageDisplay_Window.WindowParameter = 54;//窗口特征数值
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.OkCancel;//包含【确定】和【取消】按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][9];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][9];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = camera.Tools[tolerancesGraph1.ToolsSign].Name + "？";
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = camera.Tools[tolerancesGraph1.ToolsSign].Name + "？";

                GlobalWindows.MessageDisplay_Window.StartPosition = FormStartPosition.CenterScreen;
                if (GlobalWindows.TopMostWindows)//置顶
                {
                    GlobalWindows.MessageDisplay_Window.TopMost = true;//将窗口置于顶层
                }
                else//其它
                {
                    GlobalWindows.MessageDisplay_Window.Visible = true;//显示
                }
            }
            else//无效，不可以学习
            {
                //显示信息对话框

                GlobalWindows.MessageDisplay_Window.WindowParameter = 55;//窗口特征数值
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][10];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][10];

                GlobalWindows.MessageDisplay_Window.StartPosition = FormStartPosition.CenterScreen;
                if (GlobalWindows.TopMostWindows)//置顶
                {
                    GlobalWindows.MessageDisplay_Window.TopMost = true;//将窗口置于顶层
                }
                else//其它
                {
                    GlobalWindows.MessageDisplay_Window.Visible = true;//显示
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击曲线图控件2中的【学习】按钮时产生的事件
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void tolerancesGraph2_Learning_Click(object sender, EventArgs e)
        {
            if (0 <= tolerancesGraph2.Control_Graph.CurrentValueIndex[0])//有效，可以学习
            {
                iSign_Learn = tolerancesGraph2.ToolsSign;//存储曲线图特征数据，在启动学习时使用

                //显示信息对话框

                GlobalWindows.MessageDisplay_Window.WindowParameter = 56;//窗口特征数值
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.OkCancel;//包含【确定】和【取消】按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][9];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][9];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = camera.Tools[tolerancesGraph2.ToolsSign].Name + "？";
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = camera.Tools[tolerancesGraph2.ToolsSign].Name + "？";

                GlobalWindows.MessageDisplay_Window.StartPosition = FormStartPosition.CenterScreen;
                if (GlobalWindows.TopMostWindows)//置顶
                {
                    GlobalWindows.MessageDisplay_Window.TopMost = true;//将窗口置于顶层
                }
                else//其它
                {
                    GlobalWindows.MessageDisplay_Window.Visible = true;//显示
                }
            }
            else//无效，不可以学习
            {
                //显示信息对话框

                GlobalWindows.MessageDisplay_Window.WindowParameter = 57;//窗口特征数值
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][10];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][10];

                GlobalWindows.MessageDisplay_Window.StartPosition = FormStartPosition.CenterScreen;
                if (GlobalWindows.TopMostWindows)//置顶
                {
                    GlobalWindows.MessageDisplay_Window.TopMost = true;//将窗口置于顶层
                }
                else//其它
                {
                    GlobalWindows.MessageDisplay_Window.Visible = true;//显示
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击曲线图控件3中的【学习】按钮时产生的事件
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void tolerancesGraph3_Learning_Click(object sender, EventArgs e)
        {
            if (0 <= tolerancesGraph3.Control_Graph.CurrentValueIndex[0])//有效，可以学习
            {
                iSign_Learn = tolerancesGraph3.ToolsSign;//存储曲线图特征数据，在启动学习时使用

                //显示信息对话框

                GlobalWindows.MessageDisplay_Window.WindowParameter = 58;//窗口特征数值
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.OkCancel;//包含【确定】和【取消】按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][9];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][9];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = camera.Tools[tolerancesGraph3.ToolsSign].Name + "？";
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = camera.Tools[tolerancesGraph3.ToolsSign].Name + "？";

                GlobalWindows.MessageDisplay_Window.StartPosition = FormStartPosition.CenterScreen;
                if (GlobalWindows.TopMostWindows)//置顶
                {
                    GlobalWindows.MessageDisplay_Window.TopMost = true;//将窗口置于顶层
                }
                else//其它
                {
                    GlobalWindows.MessageDisplay_Window.Visible = true;//显示
                }
            }
            else//无效，不可以学习
            {
                //显示信息对话框

                GlobalWindows.MessageDisplay_Window.WindowParameter = 59;//窗口特征数值
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][10];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][10];

                GlobalWindows.MessageDisplay_Window.StartPosition = FormStartPosition.CenterScreen;
                if (GlobalWindows.TopMostWindows)//置顶
                {
                    GlobalWindows.MessageDisplay_Window.TopMost = true;//将窗口置于顶层
                }
                else//其它
                {
                    GlobalWindows.MessageDisplay_Window.Visible = true;//显示
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击曲线图控件4中的【学习】按钮时产生的事件
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void tolerancesGraph4_Learning_Click(object sender, EventArgs e)
        {
            if (0 <= tolerancesGraph4.Control_Graph.CurrentValueIndex[0])//有效，可以学习
            {
                iSign_Learn = tolerancesGraph4.ToolsSign;//存储曲线图特征数据，在启动学习时使用

                //显示信息对话框

                GlobalWindows.MessageDisplay_Window.WindowParameter = 60;//窗口特征数值
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.OkCancel;//包含【确定】和【取消】按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][9];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][9];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = camera.Tools[tolerancesGraph4.ToolsSign].Name + "？";
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = camera.Tools[tolerancesGraph4.ToolsSign].Name + "？";

                GlobalWindows.MessageDisplay_Window.StartPosition = FormStartPosition.CenterScreen;
                if (GlobalWindows.TopMostWindows)//置顶
                {
                    GlobalWindows.MessageDisplay_Window.TopMost = true;//将窗口置于顶层
                }
                else//其它
                {
                    GlobalWindows.MessageDisplay_Window.Visible = true;//显示
                }
            }
            else//无效，不可以学习
            {
                //显示信息对话框

                GlobalWindows.MessageDisplay_Window.WindowParameter = 61;//窗口特征数值
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][10];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][10];

                GlobalWindows.MessageDisplay_Window.StartPosition = FormStartPosition.CenterScreen;
                if (GlobalWindows.TopMostWindows)//置顶
                {
                    GlobalWindows.MessageDisplay_Window.TopMost = true;//将窗口置于顶层
                }
                else//其它
                {
                    GlobalWindows.MessageDisplay_Window.Visible = true;//显示
                }
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：点击【返回】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonClose_CustomButton_Click(object sender, EventArgs e)
        {
            if (bSaveProduct)//曲线图被修改
            {
                bClickCloseButton = true;//点击【CLOSE】按钮

                //显示信息对话框

                GlobalWindows.MessageDisplay_Window.WindowParameter = 64;//窗口特征数值
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.OkCancel;//包含【确定】和【取消】按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][11] + "？";
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][11] + "？";

                GlobalWindows.MessageDisplay_Window.StartPosition = FormStartPosition.CenterScreen;
                if (GlobalWindows.TopMostWindows)//置顶
                {
                    GlobalWindows.MessageDisplay_Window.TopMost = true;//将窗口置于顶层
                }
                else//其它
                {
                    GlobalWindows.MessageDisplay_Window.Visible = true;//显示
                }
            }
            else//曲线图未被修改
            {
                //事件

                if (null != Close_Click)//有效
                {
                    Close_Click(this, new CustomEventArgs());
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【+1】（Upper）按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonUpper_Plus1_CustomButton_Click(object sender, EventArgs e)
        {
            //更新数值，更新曲线图

            _SetTolerancesGraph(true, false, +1, 1);
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【+1】（Lower）按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonLower_Plus1_CustomButton_Click(object sender, EventArgs e)
        {
            //更新数值，更新曲线图

            _SetTolerancesGraph(false, false, +1, 1);
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【-1】（Upper）按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonUpper_Subtract1_CustomButton_Click(object sender, EventArgs e)
        {
            //更新数值，更新曲线图

            _SetTolerancesGraph(true, false, -1, 2);
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【-1】（Lower）按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonLower_Subtract1_CustomButton_Click(object sender, EventArgs e)
        {
            //更新数值，更新曲线图

            _SetTolerancesGraph(false, false, -1, 2);
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【+5】（Upper）按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonUpper_Plus5_CustomButton_Click(object sender, EventArgs e)
        {
            //更新数值，更新曲线图

            _SetTolerancesGraph(true, false, +5, 3);
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【+5】（Lower）按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonLower_Plus5_CustomButton_Click(object sender, EventArgs e)
        {
            //更新数值，更新曲线图

            _SetTolerancesGraph(false, false, +5, 3);
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【-5】（Upper）按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonUpper_Subtract5_CustomButton_Click(object sender, EventArgs e)
        {
            //更新数值，更新曲线图

            _SetTolerancesGraph(true, false, -5, 4);
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【-5】（Lower）按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonLower_Subtract5_CustomButton_Click(object sender, EventArgs e)
        {
            //更新数值，更新曲线图

            _SetTolerancesGraph(false, false, -5, 4);
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【Set】（Upper）按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonUpper_Set_CustomButton_Click(object sender, EventArgs e)
        {
            int iCurrentValue = 0;//待设置的数值的当前值
            int iMin = 0;//所能设置的数值范围上限
            int iMax = 0;//所能设置的数值范围上限

            iCurrentValue = _GetSetValue(true, ref iMin, ref iMax);

            //事件

            if (null != Upper_Set_Click)//有效
            {
                CustomEventArgs customEventArgs = new CustomEventArgs();//参数

                customEventArgs.IntValue[0] = _GetGraphDataIndex();//当前选中的曲线图控件对应的TolerancesControl_GraphData数组序号
                customEventArgs.IntValue[1] = iCurrentValue;//待设置的数值的当前值
                customEventArgs.IntValue[2] = iMin;//所能设置的数值范围下限
                customEventArgs.IntValue[3] = iMax;//所能设置的数值范围上限

                Upper_Set_Click(this, customEventArgs);
            }

            //显示输入键盘

            GlobalWindows.DigitalKeyboard_Window.WindowParameter = 10;//窗口特征数值
            GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.Language = language;//语言
            GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.Chinese_Caption = camera.Tools[camera.Tolerances.GraphData[iGraphDataIndex].ToolsIndex].ToolsCHNName;//中文标题文本
            GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.English_Caption = camera.Tools[camera.Tolerances.GraphData[iGraphDataIndex].ToolsIndex].ToolsENGName;//英文标题文本
            GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.Precision = 0;//输入的数据类型
            GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.MaxLength = 6;//数值长度范围
            GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.MinValue = iMin + 1;//最小值
            GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.MaxValue = iMax;//最大值
            GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.NumericalValue = iCurrentValue;//初始显示的数值

            GlobalWindows.DigitalKeyboard_Window.StartPosition = FormStartPosition.CenterScreen;
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.DigitalKeyboard_Window.TopMost = true;//将窗口置于顶层
            }
            else//其它
            {
                GlobalWindows.DigitalKeyboard_Window.Visible = true;//显示
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【Set】（Lower）按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonLower_Set_CustomButton_Click(object sender, EventArgs e)
        {
            int iCurrentValue = 0;//待设置的数值的当前值
            int iMin = 0;//所能设置的数值范围上限
            int iMax = 0;//所能设置的数值范围上限

            iCurrentValue = _GetSetValue(false, ref iMin, ref iMax);

            //事件

            if (null != Lower_Set_Click)//有效
            {
                CustomEventArgs customEventArgs = new CustomEventArgs();//参数

                customEventArgs.IntValue[0] = _GetGraphDataIndex();//当前选中的曲线图控件对应的TolerancesControl_GraphData数组序号
                customEventArgs.IntValue[1] = iCurrentValue;//待设置的数值的当前值
                customEventArgs.IntValue[2] = iMin;//所能设置的数值范围下限
                customEventArgs.IntValue[3] = iMax;//所能设置的数值范围上限

                Lower_Set_Click(this, customEventArgs);
            }

            //显示输入键盘

            GlobalWindows.DigitalKeyboard_Window.WindowParameter = 11;//窗口特征数值
            GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.Language = language;//语言
            GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.Chinese_Caption = camera.Tools[camera.Tolerances.GraphData[iGraphDataIndex].ToolsIndex].ToolsCHNName;//中文标题文本
            GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.English_Caption = camera.Tools[camera.Tolerances.GraphData[iGraphDataIndex].ToolsIndex].ToolsENGName;//英文标题文本
            GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.Precision = 0;//输入的数据类型
            GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.MaxLength = 6;//数值长度范围
            GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.MinValue = iMin;//最小值
            GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.MaxValue = iMax - 1;//最大值
            GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.NumericalValue = iCurrentValue;//初始显示的数值

            GlobalWindows.DigitalKeyboard_Window.StartPosition = FormStartPosition.CenterScreen;
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.DigitalKeyboard_Window.TopMost = true;//将窗口置于顶层
            }
            else//其它
            {
                GlobalWindows.DigitalKeyboard_Window.Visible = true;//显示
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：点击【EJECT LEVEL】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonEjectLevel_CustomButton_Click(object sender, EventArgs e)
        {
            //显示输入键盘

            GlobalWindows.DigitalKeyboard_Window.WindowParameter = 14;//窗口特征数值
            GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.Language = language;//语言
            GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.Chinese_Caption = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][13];//中文标题文本
            GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.English_Caption = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][13];//英文标题文本
            GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.Precision = 1;//输入的数据类型
            GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.MaxLength = 3;//数值长度范围
            GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.MinValue = (Single)camera.Tolerances.EjectLevel_Min / (Single)10;//最小值
            GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.MaxValue = (Single)camera.Tolerances.EjectLevel_Max / (Single)10;//最大值
            GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.NumericalValue = (Single)iEjectLevel / (Single)10;//初始显示的数值

            GlobalWindows.DigitalKeyboard_Window.StartPosition = FormStartPosition.CenterScreen;
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.DigitalKeyboard_Window.TopMost = true;//将窗口置于顶层
            }
            else//其它
            {
                GlobalWindows.DigitalKeyboard_Window.Visible = true;//显示
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：点击【View Live / View Reject】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonView_CustomButton_Click(object sender, EventArgs e)
        {
            bView = !bView;

            _SetViewButtonAndText();

            _SetView();//设置显示的图像

            //

            if (bView)//【View Live】（该按钮上显示“View Reject”，View控件中显示Live图片）
            {
                //事件

                if (null != ViewLive_Click)//有效
                {
                    ViewLive_Click(this, new CustomEventArgs());
                }

            }
            else//【View Reject】（该按钮上显示“View Live”，View控件中显示Last Reject图片）
            {
                //事件

                if (null != ViewReject_Click)//有效
                {
                    ViewReject_Click(this, new CustomEventArgs());
                }

            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【Save Product】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonSaveProduct_CustomButton_Click(object sender, EventArgs e)
        {
            if (bSaveProduct)//曲线图被修改
            {
                //显示信息对话框

                GlobalWindows.MessageDisplay_Window.WindowParameter = 64;//窗口特征数值
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.OkCancel;//包含【确定】和【取消】按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][11] + "？";
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][11] + "？";

                GlobalWindows.MessageDisplay_Window.StartPosition = FormStartPosition.CenterScreen;
                if (GlobalWindows.TopMostWindows)//置顶
                {
                    GlobalWindows.MessageDisplay_Window.TopMost = true;//将窗口置于顶层
                }
                else//其它
                {
                    GlobalWindows.MessageDisplay_Window.Visible = true;//显示
                }
            }
            else//曲线图未被修改
            {
                //不执行操作
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【Reset Graphs】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonResetGraphs_CustomButton_Click(object sender, EventArgs e)
        {
            //显示信息对话框

            GlobalWindows.MessageDisplay_Window.WindowParameter = 67;//窗口特征数值
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.OkCancel;//包含【确定】和【取消】按钮
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][12] + "？";
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][12] + "？";

            GlobalWindows.MessageDisplay_Window.StartPosition = FormStartPosition.CenterScreen;
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.MessageDisplay_Window.TopMost = true;//将窗口置于顶层
            }
            else//其它
            {
                GlobalWindows.MessageDisplay_Window.Visible = true;//显示
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【Previous Page】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonPreviousPage_CustomButton_Click(object sender, EventArgs e)
        {
            CustomEventArgs customEventArgs = new CustomEventArgs();//参数

            //获取点击按钮之前的参数信息

            customEventArgs.IntValue[2] = _GetGraphDataIndex();//点击按钮之前，选中的曲线图控件对应的TolerancesControl_GraphData数组序号（若没有曲线图被选中，则值为-1）
            customEventArgs.IntValue[3] = CurrentPage;//点击按钮之前，显示的页码

            //更新页面

            _ClickPage(true);//点击【Previous Page】按钮后的操作

            //获取点击按钮之后的参数信息

            customEventArgs.IntValue[0] = _GetGraphDataIndex();//点击按钮之后，选中的曲线图控件对应的TolerancesControl_GraphData数组序号（若没有曲线图被选中，则值为-1）
            customEventArgs.IntValue[1] = CurrentPage;//点击按钮之后，显示的页码

            //事件

            if (null != PreviousPage_Click)//有效
            {
                PreviousPage_Click(this, customEventArgs);
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【Next Page】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonNextPage_CustomButton_Click(object sender, EventArgs e)
        {
            CustomEventArgs customEventArgs = new CustomEventArgs();//参数

            //获取点击按钮之前的参数信息

            customEventArgs.IntValue[2] = _GetGraphDataIndex();//点击按钮之前，选中的曲线图控件对应的TolerancesControl_GraphData数组序号（若没有曲线图被选中，则值为-1）
            customEventArgs.IntValue[3] = CurrentPage;//点击按钮之前，显示的页码

            //更新页面

            _ClickPage(false);//点击【Next Page】按钮后的操作

            //获取点击按钮之后的参数信息

            customEventArgs.IntValue[0] = _GetGraphDataIndex();//点击按钮之后，选中的曲线图控件对应的TolerancesControl_GraphData数组序号（若没有曲线图被选中，则值为-1）
            customEventArgs.IntValue[1] = CurrentPage;//点击按钮之后，显示的页码

            //事件

            if (null != NextPage_Click)//有效
            {
                NextPage_Click(this, customEventArgs);
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：TOLERANCES，最大值设置，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void digitalKeyboardWindow_WindowClose_Tolerances_Max(object sender, EventArgs e)
        {
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.DigitalKeyboard_Window.TopMost = false;//取消置于顶层
            }
            else//其它
            {
                GlobalWindows.DigitalKeyboard_Window.Visible = false;//隐藏
            }

            //

            if (GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.EnterNewValue)//输入完成
            {
                _SetTolerancesGraph(true, true, Convert.ToInt32(GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.NumericalValue), 5);//应用设置的数值
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：TOLERANCES，最小值设置，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void digitalKeyboardWindow_WindowClose_Tolerances_Min(object sender, EventArgs e)
        {
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.DigitalKeyboard_Window.TopMost = false;//取消置于顶层
            }
            else//其它
            {
                GlobalWindows.DigitalKeyboard_Window.Visible = false;//隐藏
            }

            //

            if (GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.EnterNewValue)//输入完成
            {
                _SetTolerancesGraph(false, true, Convert.ToInt32(GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.NumericalValue), 5);//应用设置的数值
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：TOLERANCES，EJECT LEVEL，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void digitalKeyboardWindow_WindowClose_Tolerances_EjectLevel(object sender, EventArgs e)
        {
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.DigitalKeyboard_Window.TopMost = false;//取消置于顶层
            }
            else//其它
            {
                GlobalWindows.DigitalKeyboard_Window.Visible = false;//隐藏
            }

            //

            if (GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.EnterNewValue)//输入完成
            {
                iEjectLevel = Convert.ToInt32(GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.NumericalValue * (Single)10);

                labelEjectLevel.Text = ((Single)iEjectLevel / (Single)10).ToString("F1");

                //更新【Save Product】按钮背景

                bSaveProduct = true;
                customButtonSaveProduct.BitmapWhole = VisionSystemControlLibrary.Properties.Resources.Button_1;
                customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;

                //事件

                if (null != EjectLevel_Click)//有效
                {
                    EjectLevel_Click(this, new CustomEventArgs());
                }
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：TOLERANCES，【LEARN】（曲线图控件1）确认，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_Tolerances_Learn_Graph1_Confirm(object sender, EventArgs e)
        {
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.MessageDisplay_Window.TopMost = false;//取消置于顶层
            }
            else//其它
            {
                GlobalWindows.MessageDisplay_Window.Visible = false;//隐藏
            }

            //

            if (GlobalWindows.MessageDisplay_Window.MessageDisplayControl.OkCancel)//启动学习
            {
                //事件

                if (null != Learning_Click)//有效
                {
                    CustomEventArgs customEventArgs = new CustomEventArgs();//参数

                    customEventArgs.IntValue[0] = tolerancesGraph1.GraphDataIndex;//点击的按钮所在的曲线图控件对应的TolerancesControl_GraphData数组序号

                    Learning_Click(this, customEventArgs);
                }
            }
            else//不进行学习
            {
                //不执行操作
            }
        }
        
        //----------------------------------------------------------------------
        // 功能说明：TOLERANCES，【LEARN】（曲线图控件1）错误，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_Tolerances_Learn_Graph1_Failure(object sender, EventArgs e)
        {
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.MessageDisplay_Window.TopMost = false;//取消置于顶层
            }
            else//其它
            {
                GlobalWindows.MessageDisplay_Window.Visible = false;//隐藏
            }
        }
        
        //----------------------------------------------------------------------
        // 功能说明：TOLERANCES，【LEARN】（曲线图控件2）确认，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_Tolerances_Learn_Graph2_Confirm(object sender, EventArgs e)
        {
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.MessageDisplay_Window.TopMost = false;//取消置于顶层
            }
            else//其它
            {
                GlobalWindows.MessageDisplay_Window.Visible = false;//隐藏
            }

            //

            if (GlobalWindows.MessageDisplay_Window.MessageDisplayControl.OkCancel)//启动学习
            {
                //事件

                if (null != Learning_Click)//有效
                {
                    CustomEventArgs customEventArgs = new CustomEventArgs();//参数

                    customEventArgs.IntValue[0] = tolerancesGraph2.GraphDataIndex;//点击的按钮所在的曲线图控件对应的TolerancesControl_GraphData数组序号

                    Learning_Click(this, customEventArgs);
                }
            }
            else//不进行学习
            {
                //不执行操作
            }
        }
        
        //----------------------------------------------------------------------
        // 功能说明：TOLERANCES，【LEARN】（曲线图控件2）错误，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_Tolerances_Learn_Graph2_Failure(object sender, EventArgs e)
        {
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.MessageDisplay_Window.TopMost = false;//取消置于顶层
            }
            else//其它
            {
                GlobalWindows.MessageDisplay_Window.Visible = false;//隐藏
            }
        }
        
        //----------------------------------------------------------------------
        // 功能说明：TOLERANCES，【LEARN】（曲线图控件3）确认，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_Tolerances_Learn_Graph3_Confirm(object sender, EventArgs e)
        {
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.MessageDisplay_Window.TopMost = false;//取消置于顶层
            }
            else//其它
            {
                GlobalWindows.MessageDisplay_Window.Visible = false;//隐藏
            }

            //

            if (GlobalWindows.MessageDisplay_Window.MessageDisplayControl.OkCancel)//启动学习
            {
                //事件

                if (null != Learning_Click)//有效
                {
                    CustomEventArgs customEventArgs = new CustomEventArgs();//参数

                    customEventArgs.IntValue[0] = tolerancesGraph3.GraphDataIndex;//点击的按钮所在的曲线图控件对应的TolerancesControl_GraphData数组序号

                    Learning_Click(this, customEventArgs);
                }
            }
            else//不进行学习
            {
                //不执行操作
            }
        }
        
        //----------------------------------------------------------------------
        // 功能说明：TOLERANCES，【LEARN】（曲线图控件3）错误，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_Tolerances_Learn_Graph3_Failure(object sender, EventArgs e)
        {
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.MessageDisplay_Window.TopMost = false;//取消置于顶层
            }
            else//其它
            {
                GlobalWindows.MessageDisplay_Window.Visible = false;//隐藏
            }
        }
        
        //----------------------------------------------------------------------
        // 功能说明：TOLERANCES，【LEARN】（曲线图控件4）确认，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_Tolerances_Learn_Graph4_Confirm(object sender, EventArgs e)
        {
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.MessageDisplay_Window.TopMost = false;//取消置于顶层
            }
            else//其它
            {
                GlobalWindows.MessageDisplay_Window.Visible = false;//隐藏
            }

            //

            if (GlobalWindows.MessageDisplay_Window.MessageDisplayControl.OkCancel)//启动学习
            {
                //事件

                if (null != Learning_Click)//有效
                {
                    CustomEventArgs customEventArgs = new CustomEventArgs();//参数

                    customEventArgs.IntValue[0] = tolerancesGraph4.GraphDataIndex;//点击的按钮所在的曲线图控件对应的TolerancesControl_GraphData数组序号

                    Learning_Click(this, customEventArgs);
                }
            }
            else//不进行学习
            {
                //不执行操作
            }
        }
        
        //----------------------------------------------------------------------
        // 功能说明：TOLERANCES，【LEARN】（曲线图控件4）错误，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_Tolerances_Learn_Graph4_Failure(object sender, EventArgs e)
        {
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.MessageDisplay_Window.TopMost = false;//取消置于顶层
            }
            else//其它
            {
                GlobalWindows.MessageDisplay_Window.Visible = false;//隐藏
            }
        }
        
        //----------------------------------------------------------------------
        // 功能说明：TOLERANCES，【LEARN】成功，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_Tolerances_Learn_Success(object sender, EventArgs e)
        {
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.MessageDisplay_Window.TopMost = false;//取消置于顶层
            }
            else//其它
            {
                GlobalWindows.MessageDisplay_Window.Visible = false;//隐藏
            }
        }
        
        //----------------------------------------------------------------------
        // 功能说明：TOLERANCES，【LEARN】失败，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_Tolerances_Learn_Failure(object sender, EventArgs e)
        {
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.MessageDisplay_Window.TopMost = false;//取消置于顶层
            }
            else//其它
            {
                GlobalWindows.MessageDisplay_Window.Visible = false;//隐藏
            }
        }
        
        //----------------------------------------------------------------------
        // 功能说明：TOLERANCES，【SAVE PRODUCT】确认，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_Tolerances_SaveProduct_Confirm(object sender, EventArgs e)
        {
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.MessageDisplay_Window.TopMost = false;//取消置于顶层
            }
            else//其它
            {
                GlobalWindows.MessageDisplay_Window.Visible = false;//隐藏
            }

            //

            if (GlobalWindows.MessageDisplay_Window.MessageDisplayControl.OkCancel)//保存数据
            {
                //事件

                if (null != SaveProduct_Click)//有效
                {
                    SaveProduct_Click(this, new CustomEventArgs());
                }
            }
            else//不保存数据
            {
                if (bClickCloseButton)//点击【CLOSE】按钮
                {
                    bClickCloseButton = false;

                    bSaveProduct = false;

                    customButtonSaveProduct.BitmapWhole = VisionSystemControlLibrary.Properties.Resources.Button;

                    customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;

                    //事件

                    if (null != Close_Click)//有效
                    {
                        Close_Click(this, new CustomEventArgs());
                    }
                }
            }
        }
        
        //----------------------------------------------------------------------
        // 功能说明：TOLERANCES，【SAVE PRODUCT】成功，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_Tolerances_SaveProduct_Success(object sender, EventArgs e)
        {
            if (bClickCloseButton)//点击【CLOSE】按钮
            {
                bClickCloseButton = false;

                //事件

                if (null != Close_Click)//有效
                {
                    Close_Click(this, new CustomEventArgs());
                }
            }

            //

            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.MessageDisplay_Window.TopMost = false;//取消置于顶层
            }
            else//其它
            {
                GlobalWindows.MessageDisplay_Window.Visible = false;//隐藏
            }
        }
        
        //----------------------------------------------------------------------
        // 功能说明：TOLERANCES，【SAVE PRODUCT】失败，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_Tolerances_SaveProduct_Failure(object sender, EventArgs e)
        {
            if (bClickCloseButton)//点击【CLOSE】按钮
            {
                bClickCloseButton = false;

                bSaveProduct = false;

                customButtonSaveProduct.BitmapWhole = VisionSystemControlLibrary.Properties.Resources.Button;

                customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;

                //事件

                if (null != Close_Click)//有效
                {
                    Close_Click(this, new CustomEventArgs());
                }
            }

            //

            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.MessageDisplay_Window.TopMost = false;//取消置于顶层
            }
            else//其它
            {
                GlobalWindows.MessageDisplay_Window.Visible = false;//隐藏
            }
        }
        
        //----------------------------------------------------------------------
        // 功能说明：TOLERANCES，【RESET GRAPHS】确认，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_Tolerances_ResetGraphs_Confirm(object sender, EventArgs e)
        {
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.MessageDisplay_Window.TopMost = false;//取消置于顶层
            }
            else//其它
            {
                GlobalWindows.MessageDisplay_Window.Visible = false;//隐藏
            }

            //

            if (GlobalWindows.MessageDisplay_Window.MessageDisplayControl.OkCancel)//复位曲线图
            {
                //事件

                if (null != ResetGraphs_Click)//有效
                {
                    ResetGraphs_Click(this, new CustomEventArgs());
                }
            }
            else//取消复位曲线图
            {
                //不执行操作
            }
        }
        
        //----------------------------------------------------------------------
        // 功能说明：TOLERANCES，【RESET GRAPHS】成功，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_Tolerances_ResetGraphs_Success(object sender, EventArgs e)
        {
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.MessageDisplay_Window.TopMost = false;//取消置于顶层
            }
            else//其它
            {
                GlobalWindows.MessageDisplay_Window.Visible = false;//隐藏
            }
        }
        
        //----------------------------------------------------------------------
        // 功能说明：TOLERANCES，【RESET GRAPHS】失败，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_Tolerances_ResetGraphs_Failure(object sender, EventArgs e)
        {
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.MessageDisplay_Window.TopMost = false;//取消置于顶层
            }
            else//其它
            {
                GlobalWindows.MessageDisplay_Window.Visible = false;//隐藏
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【UPDATE TOLERANCES】）按钮事件，执行相应的操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonLearnSample_CustomButton_Click(object sender, EventArgs e)
        {
            //更新【Save Product】按钮背景

            bSaveProduct = true;
            customButtonSaveProduct.BitmapWhole = VisionSystemControlLibrary.Properties.Resources.Button_1;
            customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;

            //事件

            if (null != UpdateTolerances_Click)//有效
            {
                UpdateTolerances_Click(this, e);
            }
        }
    }

    //

    //曲线图控件数据信息
    public class TolerancesControl_GraphData
    {
        private int iPage = 0;//属性，曲线图控件所属的页码（从0开始）
        
        public Tolerances_Graph TolerancesGraph;//曲线图数据
        //public Tolerances_Tools TolerancesTools = new Tolerances_Tools();//所属的工具

        //----------------------------------------------------------------------
        // 功能说明：构造函数，初始化
        // 输入参数：1.graphics：绘图
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public TolerancesControl_GraphData(Graphics graphics)
        {
            TolerancesGraph = new Tolerances_Graph(graphics);
        }

        //----------------------------------------------------------------------
        // 功能说明：Page属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public int Page//属性
        {
            get//读取
            {
                return iPage;
            }
            set//设置
            {
                if (iPage != value)
                {
                    iPage = value;
                }
            }
        }
    }

    //曲线图所属工具信息
    public class Tolerances_Tools
    {
        private VisionSystemClassLibrary.Enum.CameraType Cameratype = VisionSystemClassLibrary.Enum.CameraType.Camera_1;//相机类型

        private bool bToolsValue = true;//工具数值。取值范围：true：使能；false：禁止
        private int iToolsSign = 0;//工具特征数据（工具数组序号）
        private string sToolsName = "";//工具名称
        private string sToolsUnit = "";//工具单位

        //以下学习数值仅对于包含于曲线图中的工具有效

        private bool bLearned = false;//是否经过学习。true：是；false：否
        private int iLearnedValue = 100;//学习数值
        private int iValidValue = 100;//学习中的有效数值数量
        private int iNonvalidValue = 0;//学习中的无效数值数量

        //数值范围

        private int iMaxValue = 1000;//工具在对应的曲线图Y坐标轴上所能设置的最大值（若坐标轴数据形式为包含有效数值，则该值指的是有效数值最大值；否则指的是坐标轴最大值）
        private int iMinValue = 0;//工具在对应的曲线图Y坐标轴上所能设置的最小值（若坐标轴数据形式为包含有效数值，则该值指的是有效数值最小值；否则指的是坐标轴最小值）

        private Boolean bEffectiveMin_State = true;//坐标轴最小有效实际数值是否开启
        private Boolean bEffectiveMax_State = true;//坐标轴最大有效实际数值是否开启

        //属性

        //----------------------------------------------------------------------
        // 功能说明：Camera 属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public VisionSystemClassLibrary.Enum.CameraType Camera //属性
        {
            get//读取
            {
                return Cameratype;
            }
            set//设置
            {
                if (Cameratype != value)
                {
                    Cameratype = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ToolsValue属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public bool ToolsValue//属性
        {
            get//读取
            {
                return bToolsValue;
            }
            set//设置
            {
                if (bToolsValue != value)
                {
                    bToolsValue = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ToolsSign属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public int ToolsSign//属性
        {
            get//读取
            {
                return iToolsSign;
            }
            set//设置
            {
                if (iToolsSign != value)
                {
                    iToolsSign = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ToolsName属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public string ToolsName//属性
        {
            get//读取
            {
                return sToolsName;
            }
            set//设置
            {
                if (sToolsName != value)
                {
                    sToolsName = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ToolsUnit属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public string ToolsUnit//属性
        {
            get//读取
            {
                return sToolsUnit;
            }
            set//设置
            {
                if (sToolsUnit != value)
                {
                    sToolsUnit = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：Learned属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public bool Learned//属性
        {
            get//读取
            {
                return bLearned;
            }
            set//设置
            {
                if (bLearned != value)
                {
                    bLearned = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：LearnedValue属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public int LearnedValue//属性
        {
            get//读取
            {
                return iLearnedValue;
            }
            set//设置
            {
                if (iLearnedValue != value)
                {
                    iLearnedValue = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ValidValue属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public int ValidValue//属性
        {
            get//读取
            {
                return iValidValue;
            }
            set//设置
            {
                if (iValidValue != value)
                {
                    iValidValue = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：NonvalidValue属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public int NonvalidValue//属性
        {
            get//读取
            {
                return iNonvalidValue;
            }
            set//设置
            {
                if (iNonvalidValue != value)
                {
                    iNonvalidValue = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：MaxValue属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public int MaxValue//属性
        {
            get//读取
            {
                return iMaxValue;
            }
            set//设置
            {
                if (iMaxValue != value)
                {
                    iMaxValue = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：MinValue属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public int MinValue//属性
        {
            get//读取
            {
                return iMinValue;
            }
            set//设置
            {
                if (iMinValue != value)
                {
                    iMinValue = value;
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：EffectiveMin_State属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Boolean EffectiveMin_State
        {
            get
            {
                return bEffectiveMin_State;
            }
            set
            {
                bEffectiveMin_State = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：EffectiveMax_State属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Boolean EffectiveMax_State
        {
            get
            {
                return bEffectiveMax_State;
            }
            set
            {
                bEffectiveMax_State = value;
            }
        }
    }

    //

    //对控件中的各个按钮、子控件进行操作时产生的事件中包含的参数
    //用于TolerancesControl、RejectsControl类

    public class CustomEventArgs : EventArgs
    {
        private const int iNumber = 10;//数组长度

        private int[] iValue = new int[iNumber];//参数值
        
        //----------------------------------------------------------------------
        // 功能说明：构造函数
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public CustomEventArgs()
        {
            for (int i = 0; i < iNumber; i++)//赋初值
            {
                iValue[i] = 0;
            }
        }

        //属性

        //----------------------------------------------------------------------
        // 功能说明：IntValue属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public int[] IntValue//属性
        {
            get//读取
            {
                return iValue;
            }
            set//设置
            {
                iValue = value;
            }
        }
    }
}