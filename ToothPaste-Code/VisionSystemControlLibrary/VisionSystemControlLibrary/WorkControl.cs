/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：WorkControl.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：主窗口

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
using System.Diagnostics;

using System.Threading;

using System.Runtime.InteropServices;

using System.Reflection;

namespace VisionSystemControlLibrary
{
    public partial class WorkControl : UserControl
    {
        //WORK控件

        //每页最多包含10个相机图像显示控件（iCameraDisplayControlNumber定义）

        //

        private VisionSystemClassLibrary.Enum.InterfaceLanguage language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;//属性，语言

        private VisionSystemClassLibrary.Enum.DeviceState devicestate = VisionSystemClassLibrary.Enum.DeviceState.Stop;//属性，设备状态

        //

        private VisionSystemClassLibrary.Class.System system = new VisionSystemClassLibrary.Class.System();//属性（只读），系统

        private const Int32 iCameraDisplayControlNumber = 10;//相机图像显示控件数目

        private Int32[] iIndexControl = new int[iCameraDisplayControlNumber];//相机显示控件所对应的相机数组索引值，数组序号0 ~ 9对应相机显示控件1 ~ 10


        private Int32 iCameraDisplayControlNumberTemp = -1;//相机控件被选中缓存
        private Int32 iSelectedCameraIndex = -1;//相机被选中缓存

        private Rectangle rectanglePaintBackground = new Rectangle();//绘制区域

        private Int32 iClickTrademark = 0;//点击商标菜单按钮时计数，确定执行的操作（连续点击两次同一商标按钮，显示“关于...”窗口；连续点击两次不同的商标按钮，显示“密码窗口”）。
        private Int32 iClickTrademarkCount = 0;//复位点击商标菜单按钮时计数（大于等于5s，复位）

        private String sVersion = "";//属性，版本

        private String sProductKey = "";//属性，产品密钥

        //
        
        private string sHMIApplicationVersion = "";//属性，人机界面程序文件版本，用于系统更新
        private string sControllerApplicationVersion = "";//属性，控制器程序文件版本，用于系统更新

        private string sUpdateHMIApplicationVersion = "";//属性，人机界面更新程序文件版本，用于系统更新
        private string sUpdateControllerApplicationVersion = "";//属性，控制器更新程序文件版本，用于系统更新

        private Boolean bReadyToUpdate = false;//是否存在可用更新。取值范围：true，是；false，否

        private VisionSystemClassLibrary.Enum.UpdateApplicationResult windowResult = VisionSystemClassLibrary.Enum.UpdateApplicationResult.None;//属性（只读），升级操作结果

        //

        private Int32 iUpdateNumber = 0;//属性，选择的设备数量

        private Boolean bUpdateMessageWindowShow = false;//属性（只读），是否显示升级后的提示信息窗口。取值范围：true，是；false，否

        private const Int32 iTimerUpdateMaxCount = 10;//定时器时间
        private Int32 iTimerUpdateCount = 10;//定时器时间

        private Boolean bNetCheckMessageWindowShow = false;//属性（只读），是否显示网络查询后的提示信息窗口。取值范围：true，是；false，否

        private Int32 iTimerNetCheckMaxCount = 10;//定时器时间
        private Int32 iTimerNetCheckCount = 10;//定时器时间

        //

        private Int32 iPasswordType = 0;//属性（只读），输入的密码类型。取值范围：0.未输入；1.用户密码；2，管理员密码；3，维护密码

        //

        private Bitmap[] bitmapBackground = null;//背景图像

        //

        private Bitmap bitmapNone = null;//无效图像

        //

        private Int32 iData_Value = 0;//数值

        //

        private String[][] sMessageText = new String[2][];//提示信息窗口上显示的文本（[语言][包含的文本]）

        private String[][] sMessageText_1 = new String[2][];//页码控件上显示的文本（[语言][包含的文本]）
        
        //

        private Color colorTextCameraNameState_Enable = Color.FromArgb(255, 255, 255);//相机名称、状态控件使能（包括的状态为打开、未更新）时的文本颜色
        private Color colorTextCameraNameState_Disable = Color.FromArgb(172, 168, 153);//相机名称、状态控件禁止（包括的状态为未连接、关闭）时的文本颜色
        private Color colorBackgroundCameraState_NotUpdated = Color.FromArgb(192, 0, 0);//相机状态控件未更新时的背景颜色

        //

        [Browsable(true), Description("点击【BRAND MANAGEMENT】按钮时产生的事件"), Category("WorkControl 事件")]
        public event EventHandler BrandManagement_Click;//点击【BRAND MANAGEMENT】按钮时产生的事件

        [Browsable(true), Description("点击【LIVE】按钮时产生的事件"), Category("WorkControl 事件")]
        public event EventHandler Live_Click;//点击【LIVE】按钮时产生的事件

        [Browsable(true), Description("点击【REJECTS】按钮时产生的事件"), Category("WorkControl 事件")]
        public event EventHandler Rejects_Click;//点击【REJECTS】按钮时产生的事件

        [Browsable(true), Description("点击【SYSTEM】按钮时产生的事件"), Category("WorkControl 事件")]
        public event EventHandler System_Click;//点击【SYSTEM】按钮时产生的事件

        [Browsable(true), Description("点击【DEVICES SETUP】按钮时产生的事件"), Category("WorkControl 事件")]
        public event EventHandler DevicesSetup_Click;//点击【DEVICES SETUP】按钮时产生的事件

        [Browsable(true), Description("点击【QUALITY CHECK】按钮时产生的事件"), Category("WorkControl 事件")]
        public event EventHandler QualityCheck_Click;//点击【QUALITY CHECK】按钮时产生的事件

        [Browsable(true), Description("点击【TOLERANCES】按钮时产生的事件"), Category("WorkControl 事件")]
        public event EventHandler Tolerances_Click;//点击【TOLERANCES】按钮时产生的事件

        [Browsable(true), Description("点击【STATISTICS】按钮时产生的事件"), Category("WorkControl 事件")]
        public event EventHandler Statistics_Click;//点击【STATISTICS】按钮时产生的事件

        [Browsable(true), Description("点击【Previous Page】按钮时产生的事件"), Category("WorkControl 事件")]
        public event EventHandler PreviousPage_Click;//点击【Previous Page】按钮时产生的事件

        [Browsable(true), Description("点击【Next Page】按钮时产生的事件"), Category("WorkControl 事件")]
        public event EventHandler NextPage_Click;//点击【Next Page】按钮时产生的事件

        [Browsable(true), Description("点击【系统更新】按钮时产生的事件"), Category("WorkControl 事件")]
        public event EventHandler Update_Click;//点击【系统更新】按钮时产生的事件

        [Browsable(true), Description("控制器更新完成，需要更新UI时产生的事件"), Category("WorkControl 事件")]
        public event EventHandler UpdateHMI;//控制器更新完成，需要更新UI时产生的事件

        [Browsable(true), Description("密码输入事件"), Category("WorkControl 事件")]
        public event EventHandler PasswordEnter;//密码输入事件

        [Browsable(true), Description("点击相机图像显示控件时产生的事件"), Category("WorkControl 事件")]
        public event EventHandler CameraDisplay_Click;//点击相机图像显示控件时产生的事件

        [Browsable(true), Description("双击相机图像显示控件时产生的事件"), Category("WorkControl 事件")]
        public event EventHandler CameraDisplay_DoubleClick;//双击相机图像显示控件时产生的事件

        [Browsable(true), Description("点击故障信息窗口中的【CLEAR ALL】时产生的事件"), Category("WorkControl 事件")]
        public event EventHandler ClearAllFaultMessages;//点击故障信息窗口中的【CLEAR ALL】时产生的事件

        [Browsable(true), Description("设置故障信息状态时产生的事件"), Category("WorkControl 事件")]
        public event EventHandler SetFaultMessageState;//设置故障信息状态时产生的事件

        [Browsable(true), Description("点击【关于】按钮时产生的事件"), Category("WorkControl 事件")]
        public event EventHandler About_Click;//点击【关于】按钮时产生的事件

        [Browsable(true), Description("Ping产生的事件"), Category("NetCheck 事件")]
        public event EventHandler Ping_Click;//窗口关闭时产生的事件

        [Browsable(true), Description("Connect产生的事件"), Category("NetCheck 事件")]
        public event EventHandler Connect_Click;//窗口关闭时产生的事件

        //

        //构造函数

        //----------------------------------------------------------------------
        // 功能说明：构造函数，初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public WorkControl()
        {
            InitializeComponent();

            //

            for (Int32 i = 0; i < iCameraDisplayControlNumber; i++)//初始化
            {
                iIndexControl[i] = -1;//相机显示控件所对应的相机数组索引值
            }

            labelCameraName1.Text = "";//相机1名称
            labelCameraName2.Text = "";//相机2名称
            labelCameraName3.Text = "";//相机3名称
            labelCameraName4.Text = "";//相机4名称
            labelCameraName5.Text = "";//相机5名称
            labelCameraName6.Text = "";//相机6名称
            labelCameraName7.Text = "";//相机7名称
            labelCameraName8.Text = "";//相机8名称
            labelCameraName9.Text = "";//相机9名称
            labelCameraName10.Text = "";//相机10名称

            //由于该控件仅有一个实例，因此可以在此处进行事件的订阅

            if (null != GlobalWindows.StandardKeyboard_Window)
            {
                //GlobalWindows.StandardKeyboard_Window.WindowClose_Work_Trademark += new System.EventHandler(standardKeyboardWindow_WindowClose_Work_Trademark);//订阅事件
                GlobalWindows.StandardKeyboard_Window.WindowClose_Work_System += new System.EventHandler(standardKeyboardWindow_WindowClose_Work_System);//订阅事件
                GlobalWindows.StandardKeyboard_Window.WindowClose_Work_DevicesSetup += new System.EventHandler(standardKeyboardWindow_WindowClose_Work_DevicesSetup);//订阅事件
                GlobalWindows.StandardKeyboard_Window.WindowClose_Work_BrandManagement += new System.EventHandler(standardKeyboardWindow_WindowClose_Work_BrandManagement);//订阅事件
                GlobalWindows.StandardKeyboard_Window.WindowClose_Work_QualityCheck += new System.EventHandler(standardKeyboardWindow_WindowClose_Work_QualityCheck);//订阅事件
                GlobalWindows.StandardKeyboard_Window.WindowClose_Work_Tolerances += new System.EventHandler(standardKeyboardWindow_WindowClose_Work_Tolerances);//订阅事件
                GlobalWindows.StandardKeyboard_Window.WindowClose_Work_Live += new System.EventHandler(standardKeyboardWindow_WindowClose_Work_Live);//订阅事件
                GlobalWindows.StandardKeyboard_Window.WindowClose_Work_Rejects += new System.EventHandler(standardKeyboardWindow_WindowClose_Work_Rejects);//订阅事件
                GlobalWindows.StandardKeyboard_Window.WindowClose_Work_Update += new System.EventHandler(standardKeyboardWindow_WindowClose_Work_Update);//订阅事件
                GlobalWindows.StandardKeyboard_Window.WindowClose_Work_Statistics += new System.EventHandler(standardKeyboardWindow_WindowClose_Work_Statistics);//订阅事件
            }

            if (null != GlobalWindows.MessageDisplay_Window)
            {
                GlobalWindows.MessageDisplay_Window.WindowClose_Work_Update_Confirm += new System.EventHandler(messageDisplayWindow_WindowClose_Work_Update_Confirm);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_Work_Update_Wait += new System.EventHandler(messageDisplayWindow_WindowClose_Work_Update_Wait);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_Work_LeftTrademark += new System.EventHandler(messageDisplayWindow_WindowClose_Work_LeftTrademark);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_Work_RightTrademark += new System.EventHandler(messageDisplayWindow_WindowClose_Work_RightTrademark);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_Work_ExitApplication_Confirm += new System.EventHandler(messageDisplayWindow_WindowClose_Work_ExitApplication_Confirm);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_NetCheck += new EventHandler(MessageDisplay_Window_WindowClose_NetCheck);//订阅事件
            }

            if (null != GlobalWindows.FaultMessage_Window)
            {
                GlobalWindows.FaultMessage_Window.ClearAllFaultMessages += new System.EventHandler(faultMessageWindow_ClearAllFaultMessages);//订阅事件
                GlobalWindows.FaultMessage_Window.WindowClose += new System.EventHandler(faultMessageWindow_WindowClose);//订阅事件
            }

            if (null != GlobalWindows.FaultMessageOption_Window)
            {
                GlobalWindows.FaultMessageOption_Window.WindowClose += new System.EventHandler(faultMessageOptionWindow_WindowClose);//订阅事件
            }

            if (null != GlobalWindows.NetDiagnose_Window)
            {
                GlobalWindows.NetDiagnose_Window.Ping_Click += new EventHandler(NetDiagnose_Window_Ping_Click);//订阅事件
                GlobalWindows.NetDiagnose_Window.Connect_Click += new EventHandler(NetDiagnose_Window_Connect_Click);//订阅事件
                GlobalWindows.NetDiagnose_Window.WindowClose += new EventHandler(NetDiagnose_Window_WindowClose); //订阅事件
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
                    sMessageText[i] = new String[13];
                    sMessageText_1[i] = new String[1];
                }

                //

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] = "从版本";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] = "Update from Version";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][1] = "更新到";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][1] = "to";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][2] = "版本：";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][2] = "Version: ";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][3] = "产品密钥";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][3] = "Product Key";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][4] = "密码";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][4] = "Password";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][5] = "确定退出程序";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][5] = "Exit Application";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][6] = "正在升级";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][6] = "Updating";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][7] = "升级完成";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][7] = "Update Completed";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][8] = "升级失败";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][8] = "Update Failed";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][9] = "请等待";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][9] = "Please wait";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][10] = "正在查询网络";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][10] = "Checking Net";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][11] = "网络查询失败";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][11] = "Net Checking Failed";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][12] = "请等待";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][12] = "Please wait";

                //

                sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] = customButtonPage.Chinese_TextDisplay[0];
                sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] = customButtonPage.English_TextDisplay[0];
            }

            //

            bitmapBackground = new Bitmap[1];
            bitmapBackground[0] = Properties.Resources.Camera_Background;//背景图像

            //

            bitmapNone = new Bitmap(imageDisplayCamera1.Width, imageDisplayCamera1.Height);//无效图像

            //

            _SetRectanglePaintBackground();

            //

            customButtonSpeedValue.Chinese_TextDisplay = new String[1] { "0" };//设置显示的文本
            customButtonSpeedValue.English_TextDisplay = new String[1] { "0" };//设置显示的文本

            customButtonCameraSpeedValue1.Chinese_TextDisplay = new String[1] { "0" };//设置显示的文本
            customButtonCameraSpeedValue1.English_TextDisplay = new String[1] { "0" };//设置显示的文本
            customButtonCameraSpeedValue2.Chinese_TextDisplay = new String[1] { "0" };//设置显示的文本
            customButtonCameraSpeedValue2.English_TextDisplay = new String[1] { "0" };//设置显示的文本
            customButtonCameraSpeedValue3.Chinese_TextDisplay = new String[1] { "0" };//设置显示的文本
            customButtonCameraSpeedValue3.English_TextDisplay = new String[1] { "0" };//设置显示的文本
            customButtonCameraSpeedValue4.Chinese_TextDisplay = new String[1] { "0" };//设置显示的文本
            customButtonCameraSpeedValue4.English_TextDisplay = new String[1] { "0" };//设置显示的文本
            customButtonCameraSpeedValue5.Chinese_TextDisplay = new String[1] { "0" };//设置显示的文本
            customButtonCameraSpeedValue5.English_TextDisplay = new String[1] { "0" };//设置显示的文本
            customButtonCameraSpeedValue6.Chinese_TextDisplay = new String[1] { "0" };//设置显示的文本
            customButtonCameraSpeedValue6.English_TextDisplay = new String[1] { "0" };//设置显示的文本
            customButtonCameraSpeedValue7.Chinese_TextDisplay = new String[1] { "0" };//设置显示的文本
            customButtonCameraSpeedValue7.English_TextDisplay = new String[1] { "0" };//设置显示的文本
            customButtonCameraSpeedValue8.Chinese_TextDisplay = new String[1] { "0" };//设置显示的文本
            customButtonCameraSpeedValue8.English_TextDisplay = new String[1] { "0" };//设置显示的文本
            customButtonCameraSpeedValue9.Chinese_TextDisplay = new String[1] { "0" };//设置显示的文本
            customButtonCameraSpeedValue9.English_TextDisplay = new String[1] { "0" };//设置显示的文本
            customButtonCameraSpeedValue10.Chinese_TextDisplay = new String[1] { "0" };//设置显示的文本
            customButtonCameraSpeedValue10.English_TextDisplay = new String[1] { "0" };//设置显示的文本
        }

        //属性

        //----------------------------------------------------------------------
        // 功能说明：BitmapTrademarkLeft属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("商标"), Category("WorkControl 通用")]
        public Bitmap BitmapTrademarkLeft//属性
        {
            get//读取
            {
                return customButtonTrademarkLeft.BitmapIconWhole;
            }
            set//设置
            {
                customButtonTrademarkLeft.BitmapIconWhole = value;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：BitmapTrademarkRight属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("商标"), Category("WorkControl 通用")]
        public Bitmap BitmapTrademarkRight//属性
        {
            get//读取
            {
                return customButtonTrademarkRight.BitmapIconWhole;
            }
            set//设置
            {
                customButtonTrademarkRight.BitmapIconWhole = value;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：BitmapBackground属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("背景图像"), Category("WorkControl 通用")]
        public Bitmap[] BitmapBackground//属性
        {
            get//读取
            {
                return bitmapBackground;
            }
            set//设置
            {
                bitmapBackground = value;

                //

                _SetRectanglePaintBackground();
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ImageDisplayScale_Y属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("图像显示控件比例"), Category("WorkControl 通用")]
        public Double ImageDisplayScale_Y
        {
            get//读取
            {
                return imageDisplayCamera1.ControlScale_Y;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Language属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("语言"), Category("WorkControl 通用")]
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
        [Browsable(true), Description("设备状态"), Category("WorkControl 通用")]
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

        //-----------------------------------------------------------------------
        // 功能说明：VisionSystem属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("系统"), Category("WorkControl 通用")]
        public VisionSystemClassLibrary.Class.System VisionSystem
        {
            get//读取
            {
                return system;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：HMIApplicationVersion属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("人机界面程序文件版本，用于系统更新"), Category("WorkControl 通用")]
        public string HMIApplicationVersion
        {
            get//读取
            {
                return sHMIApplicationVersion;
            }
            set//设置
            {
                if (value != sHMIApplicationVersion)
                {
                    sHMIApplicationVersion = value;
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ControllerApplicationVersion属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("控制器程序文件版本，用于系统更新"), Category("WorkControl 通用")]
        public string ControllerApplicationVersion
        {
            get//读取
            {
                return sControllerApplicationVersion;
            }
            set//设置
            {
                if (value != sControllerApplicationVersion)
                {
                    sControllerApplicationVersion = value;
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：UpdateHMIApplicationVersion属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("人机界面更新程序文件版本，用于系统更新"), Category("WorkControl 通用")]
        public string UpdateHMIApplicationVersion
        {
            get//读取
            {
                return sUpdateHMIApplicationVersion;
            }
            set//设置
            {
                if (value != sUpdateHMIApplicationVersion)
                {
                    sUpdateHMIApplicationVersion = value;
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：UpdateControllerApplicationVersion属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("控制器更新程序文件版本，用于系统更新"), Category("WorkControl 通用")]
        public string UpdateControllerApplicationVersion
        {
            get//读取
            {
                return sUpdateControllerApplicationVersion;
            }
            set//设置
            {
                if (value != sUpdateControllerApplicationVersion)
                {
                    sUpdateControllerApplicationVersion = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：Result属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("升级操作结果"), Category("WorkControl 通用")]
        public VisionSystemClassLibrary.Enum.UpdateApplicationResult Result//属性
        {
            get//读取
            {
                return windowResult;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ReadyToUpdate属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("是否存在可用更新。取值范围：true，是；false，否"), Category("WorkControl 通用")]
        public bool ReadyToUpdate
        {
            get//读取
            {
                return bReadyToUpdate;
            }
            set//设置
            {
                if (value != bReadyToUpdate)
                {
                    bReadyToUpdate = value;

                    //

                    if (bReadyToUpdate)//存在可用更新
                    {
                        customButtonUpdate.Visible = true;//显示【系统更新】按钮
                    }
                    else//不存在可用更新
                    {
                        if (GlobalWindows.TopMostWindows)//置顶
                        {
                            GlobalWindows.MessageDisplay_Window.TopMost = false;//取消置于顶层
                        }
                        else//其它
                        {
                            GlobalWindows.MessageDisplay_Window.Visible = false;//隐藏
                        }

                        customButtonUpdate.Visible = false;//隐藏【系统更新】按钮
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：AppVersion属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("程序版本"), Category("WorkControl 通用")]
        public string AppVersion//属性
        {
            get//读取
            {
                return sVersion;
            }
            set//设置
            {
                if (value != sVersion)
                {
                    sVersion = value;
                }
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：UpdateNumber属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("选择的设备数量"), Category("WorkControl 通用")]
        public Int32 UpdateNumber//属性
        {
            get//读取
            {
                return iUpdateNumber;
            }
            set//设置
            {
                iUpdateNumber = value;
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：AppProductKey属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("产品密钥"), Category("WorkControl 通用")]
        public string AppProductKey//属性
        {
            get//读取
            {
                return sProductKey;
            }
            set//设置
            {
                if (value != sProductKey)
                {
                    sProductKey = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：PasswordType属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("输入的密码类型。取值范围：-1，输入错误；0.未输入；1.用户密码；2，管理员密码；3，维护密码"), Category("WorkControl 通用")]
        public Int32 PasswordType//属性
        {
            get//读取
            {
                return iPasswordType;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：Data_Value属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("数值"), Category("WorkControl 通用")]
        public Int32 Data_Value//属性
        {
            get//读取
            {
                return iData_Value;
            }
            set//设置
            {
                if (value != iData_Value)
                {
                    iData_Value = value;
                }
            }
        }

        //函数

        //----------------------------------------------------------------------
        // 功能说明：应用设置的属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _Properties(VisionSystemClassLibrary.Class.System system_parameter)
        {
            system = system_parameter;

            //

            _SetSystem();//应用属性设置

            _SetDeviceState();//设备状态
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：设置绘制区域
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetRectanglePaintBackground()
        {
            if (bitmapBackground[system.Work.CurrentPage].Width <= pictureBoxBackground.Width && bitmapBackground[system.Work.CurrentPage].Height <= pictureBoxBackground.Height)
            {
                rectanglePaintBackground = new Rectangle(0, 0, pictureBoxBackground.Width, pictureBoxBackground.Height);//绘制区域
            }
            else if (bitmapBackground[system.Work.CurrentPage].Width > pictureBoxBackground.Width && bitmapBackground[system.Work.CurrentPage].Height <= pictureBoxBackground.Height)
            {
                rectanglePaintBackground = new Rectangle(0, 0, bitmapBackground[system.Work.CurrentPage].Width, pictureBoxBackground.Height);//绘制区域
            }
            else if (bitmapBackground[system.Work.CurrentPage].Width <= pictureBoxBackground.Width && bitmapBackground[system.Work.CurrentPage].Height > pictureBoxBackground.Height)
            {
                rectanglePaintBackground = new Rectangle(0, 0, pictureBoxBackground.Width, bitmapBackground[system.Work.CurrentPage].Height);//绘制区域
            }
            else
            {
                rectanglePaintBackground = new Rectangle(0, 0, bitmapBackground[system.Work.CurrentPage].Width, bitmapBackground[system.Work.CurrentPage].Height);//绘制区域
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：更新机器速度或相位
        // 输入参数：1.iCameraIndex：相机索引值
        //         2.iType：< 0，无效（未连接）；0，相位；1，速度
        //         3.iValue：数值
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _UpdateMachineSpeed(Int32 iCameraIndex, Int32 iType, Int32 iValue)
        {
            if (system.Camera[iCameraIndex].UIParameter.SpeedPhase_AsMachine)//
            {
                if (0 == iType)//相位
                {
                    customButtonSpeedValue.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
                    customButtonSpeedUnit.CurrentTextGroupIndex = 1;

                    //

                    customButtonSpeedValue.Chinese_TextDisplay = new String[1] { iValue.ToString() };//设置显示的文本
                    customButtonSpeedValue.English_TextDisplay = new String[1] { iValue.ToString() };//设置显示的文本
                }
                else if (1 == iType)//速度
                {
                    customButtonSpeedValue.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                    customButtonSpeedUnit.CurrentTextGroupIndex = 0;

                    //

                    customButtonSpeedValue.Chinese_TextDisplay = new String[1] { iValue.ToString() };//设置显示的文本
                    customButtonSpeedValue.English_TextDisplay = new String[1] { iValue.ToString() };//设置显示的文本
                }
                else//无效（未连接）
                {
                    customButtonSpeedValue.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                    customButtonSpeedUnit.CurrentTextGroupIndex = 2;

                    //

                    customButtonSpeedValue.Chinese_TextDisplay = new String[1] { "--" };//设置显示的文本
                    customButtonSpeedValue.English_TextDisplay = new String[1] { "--" };//设置显示的文本
                }
            }

            //

            Int32 iCameraNumber = _GetCameraDisplayControlNumber(iCameraIndex);

            if (1 == iCameraNumber)//相机控件1
            {
                _UpdateMachineSpeed(customButtonCameraSpeedValue1, customButtonCameraSpeedUnit1, iCameraIndex, iType, iValue);//
            }
            else if (2 == iCameraNumber)//相机控件2
            {
                _UpdateMachineSpeed(customButtonCameraSpeedValue2, customButtonCameraSpeedUnit2, iCameraIndex, iType, iValue);//
            }
            else if (3 == iCameraNumber)//相机控件3
            {
                _UpdateMachineSpeed(customButtonCameraSpeedValue3, customButtonCameraSpeedUnit3, iCameraIndex, iType, iValue);//
            }
            else if (4 == iCameraNumber)//相机控件4
            {
                _UpdateMachineSpeed(customButtonCameraSpeedValue4, customButtonCameraSpeedUnit4, iCameraIndex, iType, iValue);//
            }
            else if (5 == iCameraNumber)//相机控件5
            {
                _UpdateMachineSpeed(customButtonCameraSpeedValue5, customButtonCameraSpeedUnit5, iCameraIndex, iType, iValue);//
            }
            else if (6 == iCameraNumber)//相机控件6
            {
                _UpdateMachineSpeed(customButtonCameraSpeedValue6, customButtonCameraSpeedUnit6, iCameraIndex, iType, iValue);//
            }
            else if (7 == iCameraNumber)//相机控件7
            {
                _UpdateMachineSpeed(customButtonCameraSpeedValue7, customButtonCameraSpeedUnit7, iCameraIndex, iType, iValue);//
            }
            else if (8 == iCameraNumber)//相机控件8
            {
                _UpdateMachineSpeed(customButtonCameraSpeedValue8, customButtonCameraSpeedUnit8, iCameraIndex, iType, iValue);//
            }
            else if (9 == iCameraNumber)//相机控件9
            {
                _UpdateMachineSpeed(customButtonCameraSpeedValue9, customButtonCameraSpeedUnit9, iCameraIndex, iType, iValue);//
            }
            else if (10 == iCameraNumber)//10 == iCameraNumber，相机控件10
            {
                _UpdateMachineSpeed(customButtonCameraSpeedValue10, customButtonCameraSpeedUnit10, iCameraIndex, iType, iValue);//
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：更新机器速度或相位
        // 输入参数：1.customButtonCameraSpeedValue：速度数值
        //         2.customButtonCameraSpeedUnit：速度单位
        //         3.iCameraIndex：相机索引值
        //         4.iType：< 0，无效（未连接）；0，相位；1，速度
        //         5.iValue：数值
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _UpdateMachineSpeed(CustomButton customButtonCameraSpeedValue, CustomButton customButtonCameraSpeedUnit, Int32 iCameraIndex, Int32 iType, Int32 iValue)
        {
            if (0 == iType)//相位
            {
                customButtonCameraSpeedValue.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
                customButtonCameraSpeedUnit.CurrentTextGroupIndex = 1;

                //

                customButtonCameraSpeedValue.Chinese_TextDisplay = new String[1] { iValue.ToString() };//设置显示的文本
                customButtonCameraSpeedValue.English_TextDisplay = new String[1] { iValue.ToString() };//设置显示的文本
            }
            else if (1 == iType)//速度
            {
                customButtonCameraSpeedValue.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                customButtonCameraSpeedUnit.CurrentTextGroupIndex = 0;

                //

                customButtonCameraSpeedValue.Chinese_TextDisplay = new String[1] { iValue.ToString() };//设置显示的文本
                customButtonCameraSpeedValue.English_TextDisplay = new String[1] { iValue.ToString() };//设置显示的文本
            }
            else//无效（未连接）
            {
                customButtonCameraSpeedValue.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                customButtonCameraSpeedUnit.CurrentTextGroupIndex = 2;

                //

                customButtonCameraSpeedValue.Chinese_TextDisplay = new String[1] { "--" };//设置显示的文本
                customButtonCameraSpeedValue.English_TextDisplay = new String[1] { "--" };//设置显示的文本
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：更新完成
        // 输入参数：1.bSuccess：操作是否成功。取值范围：true，是；false，否
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _UpdateControllerApplication(Boolean bSuccess)
        {
            if (bSuccess)//成功
            {
                iUpdateNumber--;
            }

            if (0 >= iUpdateNumber)//完成
            {
                if (VisionSystemClassLibrary.Enum.UpdateApplicationResult.Controller == windowResult)//控制器
                {
                    bUpdateMessageWindowShow = false;

                    iTimerUpdateCount = iTimerUpdateMaxCount;

                    timerUpdate.Stop();//关闭定时器

                    //显示信息对话框

                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = " ";
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = " ";
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][7];
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][7];
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = " ";
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = " ";

                    GlobalWindows.MessageDisplay_Window.Update();
                }
                else//其它
                {
                    //事件

                    if (null != UpdateHMI)//有效
                    {
                        UpdateHMI(this, new EventArgs());
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：获取产品名称（如G.D X6S PRODUCT CHECK、AV89713C PRODUCT CHECK）
        // 输入参数：1.sMachineType：机器类型
        //         2.sProductName：产品名称
        //         3.sProductModelNumber：产品型号
        // 输出参数：无
        // 返回值：状态栏名称
        //----------------------------------------------------------------------
        public static string _GetProductFullName(String sMachineType, String sProductName, String sProductModelNumber)
        {
            String sReturn = "";

            //

            if ("" == sProductModelNumber)//产品型号为空
            {
                sReturn = sMachineType + " " + sProductName;
            }
            else//产品型号不为空
            {
                sReturn = sProductModelNumber + " " + sProductName;
            }

            //

            return sReturn;
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：用户调用，设置相机数据
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _SetCameraData(Int32 iCameraIndex)
        {
            _SetSelectedCamera();//设置当前选择的相机类型

            //

            _SetCameraDisplayControl(_GetCameraDisplayControlNumber(iCameraIndex), iCameraIndex, false);//设置相机控件

            _SetMenu();//设置菜单按钮控件
        }

        //----------------------------------------------------------------------
        // 功能说明：用户调用，设置相机图像
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _SetCameraImage(Int32 iCameraIndex)
        {
            _SetLiveImage(_GetCameraDisplayControlNumber(iCameraIndex), iCameraIndex);//设置相机显示控件
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：用户调用，设置语言
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetLanguage()
        {
            customButtonSystem.Language = language;//【SYSTEM】
            customButtonDevicesSetup.Language = language;//【DEVICES SETUP】
            customButtonBrandManagement.Language = language;//【BRAND MANAGEMENT】
            customButtonQualityCheck.Language = language;//【QUALITY CHECK】
            customButtonTolerances.Language = language;//【TOLERANCES】
            customButtonLive.Language = language;//【LIVE】
            customButtonStatistics.Language = language;//【STATISTICS】

            //

            imageDisplayCamera1.Language = language;//相机显示器1
            imageDisplayCamera2.Language = language;//相机显示器2
            imageDisplayCamera3.Language = language;//相机显示器3
            imageDisplayCamera4.Language = language;//相机显示器4
            imageDisplayCamera5.Language = language;//相机显示器5
            imageDisplayCamera6.Language = language;//相机显示器6
            imageDisplayCamera7.Language = language;//相机显示器7
            imageDisplayCamera8.Language = language;//相机显示器8
            imageDisplayCamera9.Language = language;//相机显示器9
            imageDisplayCamera10.Language = language;//相机显示器10

            customButtonCameraState1.Language = language;//相机状态控件1
            customButtonCameraState2.Language = language;//相机状态控件2
            customButtonCameraState3.Language = language;//相机状态控件3
            customButtonCameraState4.Language = language;//相机状态控件4
            customButtonCameraState5.Language = language;//相机状态控件5
            customButtonCameraState6.Language = language;//相机状态控件6
            customButtonCameraState7.Language = language;//相机状态控件7
            customButtonCameraState8.Language = language;//相机状态控件8
            customButtonCameraState9.Language = language;//相机状态控件9
            customButtonCameraState10.Language = language;//相机状态控件10

            Int32 i = 0;//循环控制变量
            Int32 iCameraNumber = 1;//当前页中的相机序号

            if (null != system.Camera)
            {
                for (i = 0; i < system.Camera.Count; i++)//遍历相机
                {
                    if (system.Work.CurrentPage == system.Camera[i].UIParameter.Work_Page)//相机属于当前页
                    {
                        if (1 == iCameraNumber)//相机控件1
                        {
                            labelCameraName1.Text = system.Camera[i].Name;//相机名称控件文本名称
                            customButtonCameraState1.CurrentTextGroupIndex = _GetCameraState(i);//相机状态控件文本名称
                        }
                        else if (2 == iCameraNumber)//相机控件2
                        {
                            labelCameraName2.Text = system.Camera[i].Name;//相机名称控件文本名称
                            customButtonCameraState2.CurrentTextGroupIndex = _GetCameraState(i);//相机状态控件文本名称
                        }
                        else if (3 == iCameraNumber)//相机控件3
                        {
                            labelCameraName3.Text = system.Camera[i].Name;//相机名称控件文本名称
                            customButtonCameraState3.CurrentTextGroupIndex = _GetCameraState(i);//相机状态控件文本名称
                        }
                        else if (4 == iCameraNumber)//相机控件4
                        {
                            labelCameraName4.Text = system.Camera[i].Name;//相机名称控件文本名称
                            customButtonCameraState4.CurrentTextGroupIndex = _GetCameraState(i);//相机状态控件文本名称
                        }
                        else if (5 == iCameraNumber)//相机控件5
                        {
                            labelCameraName5.Text = system.Camera[i].Name;//相机名称控件文本名称
                            customButtonCameraState5.CurrentTextGroupIndex = _GetCameraState(i);//相机状态控件文本名称
                        }
                        else if (6 == iCameraNumber)//相机控件6
                        {
                            labelCameraName6.Text = system.Camera[i].Name;//相机名称控件文本名称
                            customButtonCameraState6.CurrentTextGroupIndex = _GetCameraState(i);//相机状态控件文本名称
                        }
                        else if (7 == iCameraNumber)//相机控件7
                        {
                            labelCameraName7.Text = system.Camera[i].Name;//相机名称控件文本名称
                            customButtonCameraState7.CurrentTextGroupIndex = _GetCameraState(i);//相机状态控件文本名称
                        }
                        else if (8 == iCameraNumber)//相机控件8
                        {
                            labelCameraName8.Text = system.Camera[i].Name;//相机名称控件文本名称
                            customButtonCameraState8.CurrentTextGroupIndex = _GetCameraState(i);//相机状态控件文本名称
                        }
                        else if (9 == iCameraNumber)//相机控件9
                        {
                            labelCameraName9.Text = system.Camera[i].Name;//相机名称控件文本名称
                            customButtonCameraState9.CurrentTextGroupIndex = _GetCameraState(i);//相机状态控件文本名称
                        }
                        else if (10 == iCameraNumber)//10 == iCameraNumber，相机控件10
                        {
                            labelCameraName10.Text = system.Camera[i].Name;//相机名称控件文本名称
                            customButtonCameraState10.CurrentTextGroupIndex = _GetCameraState(i);//相机状态控件文本名称
                        }

                        //

                        iCameraNumber++;//更新数值
                    }
                }
            }

            //

            switch (language)
            {
                case VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese://中文

                    customButtonTrademarkLeft.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Down;//商标控件
                    customButtonTrademarkRight.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Down;//商标控件

                    break;
                case VisionSystemClassLibrary.Enum.InterfaceLanguage.English://英文

                    customButtonTrademarkLeft.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//商标控件
                    customButtonTrademarkRight.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//商标控件

                    break;
                default:
                    break;
            }

            customButtonPage.Chinese_TextDisplay = new String[1] { sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] + (system.Work.CurrentPage + 1).ToString() + " / " + system.UIParameter.Work_TotalPage.ToString() };//设置显示的文本
            customButtonPage.English_TextDisplay = new String[1] { sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] + (system.Work.CurrentPage + 1).ToString() + " / " + system.UIParameter.Work_TotalPage.ToString() };//设置显示的文本

            if (VisionSystemClassLibrary.Enum.CameraType.None != system.Work.SelectedCameraType)//当前选择了某一相机显示控件
            {
                labelSelectedCameraText.Text = system.Camera[system.Work.SelectedCameraIndex].Name;//主菜单当前选择相机名称控件
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
            if (VisionSystemClassLibrary.Enum.DeviceState.Run == devicestate)//运行
            {
                //更新按钮状态

                if (VisionSystemClassLibrary.Enum.CameraType.None != system.Work.SelectedCameraType)//当前选择了某一相机显示控件
                {
                    customButtonLive.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//更新【LIVE】按钮的背景
                    customButtonTolerances.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//更新【TOLERANCES】按钮的背景
                    customButtonQualityCheck.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//更新【QUALITY CHECK】按钮的背景
                    customButtonStatistics.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//更新【STATISTICS】按钮的背景
                }
                else//当前未选择任何相机显示控件
                {
                    customButtonLive.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【LIVE】按钮的背景
                    customButtonTolerances.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【TOLERANCES】按钮的背景
                    customButtonQualityCheck.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【QUALITY CHECK】按钮的背景
                    customButtonStatistics.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【STATISTICS】按钮的背景
                }
            }
            else//停止
            {
                //更新按钮状态

                if (VisionSystemClassLibrary.Enum.CameraType.None != system.Work.SelectedCameraType)//当前选择了某一相机显示控件
                {
                    customButtonLive.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//更新【LIVE】按钮的背景
                    customButtonTolerances.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//更新【TOLERANCES】按钮的背景
                    customButtonQualityCheck.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//更新【QUALITY CHECK】按钮的背景
                    customButtonStatistics.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//更新【STATISTICS】按钮的背景
                }
                else//当前未选择任何相机显示控件
                {
                    customButtonLive.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【LIVE】按钮的背景
                    customButtonTolerances.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【TOLERANCES】按钮的背景
                    customButtonQualityCheck.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【QUALITY CHECK】按钮的背景
                    customButtonStatistics.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【STATISTICS】按钮的背景
                }
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：点击Trademark控件显示密码窗口
        // 输入参数：1.iCameraIndex：相机数据数组索引值
        // 输出参数：无
        // 返回值：获取的相机状态文本
        //----------------------------------------------------------------------
        private void _ShowPasswordWindow_ClickTrademark()
        {
            iPasswordType = 3;//3，维护密码

            //显示提示信息窗口

            GlobalWindows.MessageDisplay_Window.WindowParameter = 76;//窗口特征数值
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.OkCancel;//包含【确定】和【取消】按钮
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][5] + "？";
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][5] + "？";

            GlobalWindows.MessageDisplay_Window.StartPosition = FormStartPosition.CenterScreen;
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.MessageDisplay_Window.TopMost = true;//将窗口置于顶层
            }
            else//其它
            {
                GlobalWindows.MessageDisplay_Window.Visible = true;//显示
            }

            //GlobalWindows.StandardKeyboard_Window.WindowParameter = 6;//窗口特征数值
            //GlobalWindows.StandardKeyboard_Window.Language = language;//语言
            //GlobalWindows.StandardKeyboard_Window.Chinese_Caption = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][4];//中文标题文本
            //GlobalWindows.StandardKeyboard_Window.English_Caption = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][4];//英文标题文本
            //GlobalWindows.StandardKeyboard_Window.IsPassword = true;//密码输入窗口
            //if ("" == system.UserPassword)//关闭
            //{
            //    GlobalWindows.StandardKeyboard_Window.PasswordStyle = 2;//属性，密码输入类型。取值范围：0，密码输入（输入完成，正确，关闭窗口）；1，输入当前密码；2，输入新的密码；3，确认密码
            //}
            //else//开启
            //{
            //    GlobalWindows.StandardKeyboard_Window.PasswordStyle = 1;//属性，密码输入类型。取值范围：0，密码输入（输入完成，正确，关闭窗口）；1，输入当前密码；2，输入新的密码；3，确认密码
            //    GlobalWindows.StandardKeyboard_Window.Password = system.UserPassword + "\n" + system.AdministratorPassword;//属性，当前密码
            //}
            //GlobalWindows.StandardKeyboard_Window.CapsLock = true;//Caps Lock打开
            //GlobalWindows.StandardKeyboard_Window.Shift = false;//SHIFT按下
            //GlobalWindows.StandardKeyboard_Window.MaxLength = 8;//数值长度范围
            //GlobalWindows.StandardKeyboard_Window.StringValue = "";//初始显示的数值

            //GlobalWindows.StandardKeyboard_Window.StartPosition = FormStartPosition.CenterScreen;
            //if (GlobalWindows.TopMostWindows)//置顶
            //{
            //    GlobalWindows.StandardKeyboard_Window.TopMost = true;//将窗口置于顶层
            //}
            //else//其它
            //{
            //    GlobalWindows.StandardKeyboard_Window.Visible = true;//显示
            //}
        }

        //----------------------------------------------------------------------
        // 功能说明：点击菜单按钮时，检查密码
        // 输入参数：1.iType：菜单类型。取值范围：
        //7.【SYSTEM】
        //8.【DEVICES SETUP】
        //9.【BRAND MANAGEMENT】
        //10.【QUALITY CHECK】
        //11.【TOLERANCES】
        //12.【LIVE】
        //13.【REJECTS】
        //14.【系统更新】
        //27.【STATISTICS】
        // 输出参数：无
        // 返回值：检查状态。取值范围：true，无密码；false，有密码
        //----------------------------------------------------------------------
        private Boolean _CheckPassword(Int32 iType)
        {
            Boolean bReturn = false;//返回值

            //

            if ("" == system.UserPassword)//无密码
            {
                bReturn = true;
            }
            else//密码保护
            {
                bReturn = false;

                //

                GlobalWindows.StandardKeyboard_Window.WindowParameter = iType;//窗口特征数值
                GlobalWindows.StandardKeyboard_Window.Language = language;//语言
                GlobalWindows.StandardKeyboard_Window.Chinese_Caption = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][4];//中文标题文本
                GlobalWindows.StandardKeyboard_Window.English_Caption = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][4];//英文标题文本
                GlobalWindows.StandardKeyboard_Window.IsPassword = true;//密码输入窗口
                GlobalWindows.StandardKeyboard_Window.PasswordStyle = 0;//属性，密码输入类型。取值范围：0，密码输入（输入完成，正确，关闭窗口）；1，输入当前密码；2，输入新的密码；3，确认密码
                GlobalWindows.StandardKeyboard_Window.Password = system.UserPassword + "\n" + "VISION";//属性，当前密码
                GlobalWindows.StandardKeyboard_Window.CapsLock = true;//Caps Lock打开
                GlobalWindows.StandardKeyboard_Window.Shift = false;//SHIFT按下
                GlobalWindows.StandardKeyboard_Window.MaxLength = 8;//数值长度范围
                GlobalWindows.StandardKeyboard_Window.StringValue = "";//初始显示的数值

                GlobalWindows.StandardKeyboard_Window.StartPosition = FormStartPosition.CenterScreen;
                if (GlobalWindows.TopMostWindows)//置顶
                {
                    GlobalWindows.StandardKeyboard_Window.TopMost = true;//将窗口置于顶层
                }
                else//其它
                {
                    GlobalWindows.StandardKeyboard_Window.Visible = true;//显示
                }
            }

            //

            return bReturn;
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：获取相机状态文本
        // 输入参数：1.iCameraIndex：相机数据数组索引值
        // 输出参数：无
        // 返回值：获取的相机状态索引（0，未连接；1，打开；2，关闭；3，未更新）
        //----------------------------------------------------------------------
        private Int32 _GetCameraState(int iCameraIndex)
        {
            Int32 iReturn = 0;//函数返回值

            if (VisionSystemClassLibrary.Enum.CameraState.NOTCONNECTED == system.Camera[iCameraIndex].DeviceInformation.CAM)//未连接
            {
                iReturn = 0;
            }
            else
            {
                if (VisionSystemClassLibrary.Enum.CameraState.NOTUPDATED == (system.Camera[iCameraIndex].DeviceInformation.CAM & VisionSystemClassLibrary.Enum.CameraState.NOTUPDATED))//未更新
                {
                    if (VisionSystemClassLibrary.Enum.CameraState.ON == (system.Camera[iCameraIndex].DeviceInformation.CAM & VisionSystemClassLibrary.Enum.CameraState.ON))//打开
                    {
                        iReturn = 3;
                    }
                    else//VisionSystemClassLibrary.Enum.CameraState.OFF，关闭
                    {
                        iReturn = 2;
                    }
                }
                else//其它
                {
                    if (VisionSystemClassLibrary.Enum.CameraState.ON == (system.Camera[iCameraIndex].DeviceInformation.CAM & VisionSystemClassLibrary.Enum.CameraState.ON))//打开
                    {
                        if (VisionSystemClassLibrary.Enum.CameraState.REJECTOFF == (system.Camera[iCameraIndex].DeviceInformation.CAM & VisionSystemClassLibrary.Enum.CameraState.REJECTOFF))//打开
                        {
                            iReturn = 4;
                        }
                        else
                        {
                            iReturn = 1;
                        }
                    }
                    else//VisionSystemClassLibrary.Enum.CameraState.OFF，关闭
                    {
                        iReturn = 2;
                    }
                }
            }

            return iReturn;
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：设置相机控件背景
        // 输入参数：1.customButtonCameraBackground：相机背景控件
        //         2.iCameraIndex：相机数据数组索引值
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetLiveBackground(CustomButton customButtonCameraBackground, int iCameraIndex)
        {
            if (VisionSystemClassLibrary.Enum.CameraState.NOTCONNECTED == system.Camera[iCameraIndex].DeviceInformation.CAM || VisionSystemClassLibrary.Enum.CameraState.OFF == (system.Camera[iCameraIndex].DeviceInformation.CAM & VisionSystemClassLibrary.Enum.CameraState.OFF))//未连接或关闭
            {
                customButtonCameraBackground.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
            }
            else//打开或未更新
            {
                if (system.Camera[iCameraIndex].Live.CameraSelected)//选中
                {
                    customButtonCameraBackground.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Down;
                }
                else//未选中
                {
                    customButtonCameraBackground.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                }
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：设置相机图像
        // 输入参数：1.iCameraNumber：当前页中的相机序号
        //         2.iCameraIndex：相机数据数组索引值
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetLiveImage(int iCameraNumber, int iCameraIndex)
        {
            if (1 == iCameraNumber)//相机控件1
            {
                _SetLiveImage(imageDisplayCamera1, iCameraIndex);//设置相机图像
            }
            else if (2 == iCameraNumber)//相机控件2
            {
                _SetLiveImage(imageDisplayCamera2, iCameraIndex);//设置相机图像
            }
            else if (3 == iCameraNumber)//相机控件3
            {
                _SetLiveImage(imageDisplayCamera3, iCameraIndex);//设置相机图像
            }
            else if (4 == iCameraNumber)//相机控件4
            {
                _SetLiveImage(imageDisplayCamera4, iCameraIndex);//设置相机图像
            }
            else if (5 == iCameraNumber)//相机控件5
            {
                _SetLiveImage(imageDisplayCamera5, iCameraIndex);//设置相机图像
            }
            else if (6 == iCameraNumber)//相机控件6
            {
                _SetLiveImage(imageDisplayCamera6, iCameraIndex);//设置相机图像
            }
            else if (7 == iCameraNumber)//相机控件7
            {
                _SetLiveImage(imageDisplayCamera7, iCameraIndex);//设置相机图像
            }
            else if (8 == iCameraNumber)//相机控件8
            {
                _SetLiveImage(imageDisplayCamera8, iCameraIndex);//设置相机图像
            }
            else if (9 == iCameraNumber)//相机控件9
            {
                _SetLiveImage(imageDisplayCamera9, iCameraIndex);//设置相机图像
            }
            else if (10 == iCameraNumber)//10 == iCameraNumber，相机控件10
            {
                _SetLiveImage(imageDisplayCamera10, iCameraIndex);//设置相机图像
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：设置相机控件图像
        // 输入参数：1.imageShowCamera：相机图像控件
        //         2.iCameraIndex：相机数据数组索引值
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetLiveImage(ImageDisplay imageShowCamera, int iCameraIndex)
        {
            imageShowCamera.Information = system.Camera[iCameraIndex].Live.GraphicsInformation;

            if (null != system.Camera[iCameraIndex].ImageLive)//有效
            {
                if (imageShowCamera.ControlSize.Width <= system.Camera[iCameraIndex].ImageLive.Width && imageShowCamera.ControlSize.Height <= system.Camera[iCameraIndex].ImageLive.Height)//有效
                {
                    if (!(imageShowCamera.ShowTitle))//隐藏
                    {
                        imageShowCamera.ShowTitle = true;//显示
                    }

                    //

                    imageShowCamera.BitmapDisplay = system.Camera[iCameraIndex].ImageLive.ToBitmap();//设置图像
                }
            }
            else//无效
            {
                if (imageShowCamera.ShowTitle)//显示
                {
                    imageShowCamera.ShowTitle = false;//隐藏
                }

                //

                imageShowCamera.BitmapDisplay = (Bitmap)bitmapNone.Clone();//图像数据
            }

            imageShowCamera.Update();
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：设置相机设备信息
        // 输入参数：1.imageShowCamera：相机图像控件
        //         2.labelCameraName：相机名称控件
        //         3.customButtonCameraState：相机状态控件
        //         4.iCameraIndex：相机数据数组索引值
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetDeviceData(ImageDisplay imageShowCamera, Label labelCameraName, CustomButton customButtonCameraState, Int32 iCameraIndex)
        {
            if (VisionSystemClassLibrary.Enum.CameraState.NOTCONNECTED == system.Camera[iCameraIndex].DeviceInformation.CAM)//未连接
            {
                imageShowCamera.Visible = false;//隐藏相机图像显示控件

                labelCameraName.ForeColor = colorTextCameraNameState_Disable;//相机名称控件文本颜色

                customButtonCameraState.ColorTextEnable = colorTextCameraNameState_Disable;//相机状态控件文本颜色
                customButtonCameraState.BackgroundColor = BackColor;//相机状态控件背景颜色
            }
            else//其它
            {
                if (VisionSystemClassLibrary.Enum.CameraState.NOTUPDATED == (system.Camera[iCameraIndex].DeviceInformation.CAM & VisionSystemClassLibrary.Enum.CameraState.NOTUPDATED))//未更新
                {
                    if (VisionSystemClassLibrary.Enum.CameraState.ON == (system.Camera[iCameraIndex].DeviceInformation.CAM & VisionSystemClassLibrary.Enum.CameraState.ON))//打开
                    {
                        imageShowCamera.Visible = true;//显示相机图像显示控件

                        labelCameraName.ForeColor = colorTextCameraNameState_Enable;//相机名称控件文本颜色

                        customButtonCameraState.ColorTextEnable = colorTextCameraNameState_Enable;//相机状态控件文本颜色
                        customButtonCameraState.BackgroundColor = colorBackgroundCameraState_NotUpdated;//相机状态控件背景颜色
                    }
                    else//VisionSystemClassLibrary.Enum.CameraState.OFF，关闭
                    {
                        imageShowCamera.Visible = false;//隐藏相机图像显示控件

                        labelCameraName.ForeColor = colorTextCameraNameState_Disable;//相机名称控件文本颜色

                        customButtonCameraState.ColorTextEnable = colorTextCameraNameState_Disable;//相机状态控件文本颜色
                        customButtonCameraState.BackgroundColor = BackColor;//相机状态控件背景颜色
                    }
                }
                else//其它
                {
                    if (VisionSystemClassLibrary.Enum.CameraState.ON == (system.Camera[iCameraIndex].DeviceInformation.CAM & VisionSystemClassLibrary.Enum.CameraState.ON))//打开
                    {
                        if (VisionSystemClassLibrary.Enum.CameraState.REJECTOFF == (system.Camera[iCameraIndex].DeviceInformation.CAM & VisionSystemClassLibrary.Enum.CameraState.REJECTOFF))//剔除关闭
                        {
                            imageShowCamera.Visible = true;//显示相机图像显示控件

                            labelCameraName.ForeColor = colorTextCameraNameState_Enable;//相机名称控件文本颜色

                            customButtonCameraState.ColorTextEnable = colorTextCameraNameState_Enable;//相机状态控件文本颜色
                            customButtonCameraState.BackgroundColor = colorBackgroundCameraState_NotUpdated;//相机状态控件背景颜色
                        }
                        else
                        {
                            imageShowCamera.Visible = true;//显示相机图像显示控件

                            labelCameraName.ForeColor = colorTextCameraNameState_Enable;//相机名称控件文本颜色

                            customButtonCameraState.ColorTextEnable = colorTextCameraNameState_Enable;//相机状态控件文本颜色
                            customButtonCameraState.BackgroundColor = BackColor;//相机状态控件背景颜色
                        }
                    }
                    else//VisionSystemClassLibrary.Enum.CameraState.OFF，关闭
                    {
                        imageShowCamera.Visible = false;//隐藏相机图像显示控件

                        labelCameraName.ForeColor = colorTextCameraNameState_Disable;//相机名称控件文本颜色

                        customButtonCameraState.ColorTextEnable = colorTextCameraNameState_Disable;//相机状态控件文本颜色
                        customButtonCameraState.BackgroundColor = BackColor;//相机状态控件背景颜色
                    }
                }

            }

            //

            labelCameraName.Text = system.Camera[iCameraIndex].Name;//相机名称控件文本名称

            customButtonCameraState.CurrentTextGroupIndex = _GetCameraState(iCameraIndex);//相机状态控件文本名称
        }

        //----------------------------------------------------------------------
        // 功能说明：设置当前选择的相机类型
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetSelectedCamera()
        {
            if (VisionSystemClassLibrary.Enum.CameraType.None != system.Work.SelectedCameraType)//当前选择了某一相机显示控件
            {
                if (VisionSystemClassLibrary.Enum.CameraState.NOTCONNECTED == system.Camera[system.Work.SelectedCameraIndex].DeviceInformation.CAM || VisionSystemClassLibrary.Enum.CameraState.OFF == (system.Camera[system.Work.SelectedCameraIndex].DeviceInformation.CAM & VisionSystemClassLibrary.Enum.CameraState.OFF))//说明该相机当前断开连接或关闭
                {
                    system.Camera[iSelectedCameraIndex].Live.CameraSelected = false;//取消选择
                    
                    //

                    system.Work.SelectedCameraType = VisionSystemClassLibrary.Enum.CameraType.None;
                    system.Work.SelectedCameraIndex = -1;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：设置视觉系统数据
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetSystem()
        {
            if (system.Shift.ShiftState)//使能班次
            {
                customButtonStatistics.Visible = true;//【STATISTICS】
            }
            else//禁用班次
            {
                customButtonStatistics.Visible = false;//【STATISTICS】
            }

            //

            customButtonSpeedValue.Location = system.UIParameter.Work_SpeedPhase_Value_Location;
            customButtonSpeedValue.SizeButton = system.UIParameter.Work_SpeedPhase_Value_Size;
            customButtonSpeedValue.FontText = new Font(customButtonSpeedValue.FontText.Name, system.UIParameter.Work_SpeedPhase_Value_FontSize, customButtonSpeedValue.FontText.Style, customButtonSpeedValue.FontText.Unit, customButtonSpeedValue.FontText.GdiCharSet);

            customButtonSpeedUnit.Location = system.UIParameter.Work_SpeedPhase_Unit_Location;
            customButtonSpeedUnit.SizeButton = system.UIParameter.Work_SpeedPhase_Unit_Size;
            customButtonSpeedUnit.FontText = new Font(customButtonSpeedUnit.FontText.Name, system.UIParameter.Work_SpeedPhase_Unit_FontSize, customButtonSpeedUnit.FontText.Style, customButtonSpeedUnit.FontText.Unit, customButtonSpeedUnit.FontText.GdiCharSet);

            Int32 i = 0;//循环控制变量

            if (null != system.Camera)
            {
                for (i = 0; i < system.Camera.Count; i++)
                {
                    if (system.Camera[i].UIParameter.SpeedPhase_AsMachine)//
                    {
                        break;
                    }
                }
                if (i < system.Camera.Count)//
                {
                    customButtonSpeedValue.Visible = true;
                    customButtonSpeedUnit.Visible = true;
                } 
                else//
                {
                    customButtonSpeedValue.Visible = false;
                    customButtonSpeedUnit.Visible = false;
                }
            }

            //

            _SetLanguage();//设置控件显示的文本

            _SetSelectedCamera();//设置当前选择的相机类型

            //

            _SetPage();//设置页面数据
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【Previous Page】或【Next Page】按钮时调用本函数，执行相应的操作
        // 输入参数：1.bPreviousorNext：点击的按钮类型。取值范围：true，点击了【Previous Page】按钮；false，点击了【Next Page】
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _ClickPage(bool bPreviousorNext)
        {
            _SetPageNumber(bPreviousorNext);//设置当前页码

            _SetPage();//设置页面数据
        }

        //----------------------------------------------------------------------
        // 功能说明：设置当前页码
        // 输入参数：1.bPreviousorNext：点击的按钮类型。取值范围：true，点击了【Previous Page】按钮；false，点击了【Next Page】
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetPageNumber(bool bPreviousorNext)
        {
            if (1 < system.UIParameter.Work_TotalPage)//多于1页
            {
                if (bPreviousorNext)//点击了【Previous Page】按钮
                {
                    if (0 == system.Work.CurrentPage)//首页
                    {
                        system.Work.CurrentPage = (ushort)(system.UIParameter.Work_TotalPage - 1);//设置页码
                    }
                    else//非首页
                    {
                        (system.Work.CurrentPage)--;//设置页码
                    }
                }
                else//点击了【Next Page】
                {
                    if (system.UIParameter.Work_TotalPage - 1 == system.Work.CurrentPage)//末页
                    {
                        system.Work.CurrentPage = 0;//设置页码
                    }
                    else//非末页
                    {
                        (system.Work.CurrentPage)++;//设置页码
                    }
                }
            }
            else//1页
            {
                //不执行操作
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：设置菜单按钮、文本控件
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetMenu()
        {
            if (VisionSystemClassLibrary.Enum.CameraType.None != system.Work.SelectedCameraType)//当前选择了某一相机显示控件
            {
                labelSelectedCameraText.Text = system.Camera[system.Work.SelectedCameraIndex].Name;//主菜单当前选择相机名称控件
            }
            else//当前未选择任何相机显示控件
            {
                labelSelectedCameraText.Text = "";//主菜单当前选择相机名称控件
            }

            //

            _SetDeviceState();
        }

        //----------------------------------------------------------------------
        // 功能说明：设置页面数据
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetPage()
        {
            _SetCameraDisplayControl();//设置相机显示控件

            //

            _SetMenu();//设置菜单按钮控件

            //

            customButtonPage.Chinese_TextDisplay = new String[1] { sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] + (system.Work.CurrentPage + 1).ToString() + " / " + system.UIParameter.Work_TotalPage.ToString() };//设置显示的文本
            customButtonPage.English_TextDisplay = new String[1] { sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] + (system.Work.CurrentPage + 1).ToString() + " / " + system.UIParameter.Work_TotalPage.ToString() };//设置显示的文本

            if (1 < system.UIParameter.Work_TotalPage)//多于1页
            {
                customButtonPage.Visible = true;//显示页码控件

                customButtonPreviousPage.Visible = true;//显示【Previous Page】按钮
                customButtonNextPage.Visible = true;//显示【Next Page】按钮
            }
            else//1页
            {
                customButtonPage.Visible = false;//隐藏页码控件

                customButtonPreviousPage.Visible = false;//隐藏【Previous Page】按钮
                customButtonNextPage.Visible = false;//隐藏【Next Page】按钮
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：设置相机显示控件
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetCameraDisplayControl()
        {
            Int32 i = 0;//循环控制变量
            Int32 iCameraNumber = 0;//当前页中的相机序号

            if (null != system.Camera)
            {
                for (i = 0; i < system.Camera.Count; i++)//遍历相机
                {
                    if (system.Work.CurrentPage == system.Camera[i].UIParameter.Work_Page)//相机属于当前页
                    {
                        iCameraNumber++;//更新数值

                        //

                        iIndexControl[iCameraNumber - 1] = i;//相机显示控件所对应的相机数组索引值，数组序号0 ~ 9对应相机显示控件1 ~ 10

                        //

                        _SetCameraDisplayControl(iCameraNumber, i, true);//设置相机控件
                    }
                }
            }

            _HideCameraDisplayControl(iCameraNumber);//隐藏不属于本页的相机图像显示控件

            _SetBackground();//更新背景图像
        }

        //----------------------------------------------------------------------
        // 功能说明：更新背景图像
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetBackground()
        {
            int i = 0;//循环控制变量

            Image imageDisplay = new Bitmap(rectanglePaintBackground.Width, rectanglePaintBackground.Height);
            Graphics graphicsDraw = Graphics.FromImage(imageDisplay);

            SolidBrush solidbrushDraw = new SolidBrush(BackColor);//画刷
            Pen penDraw = new Pen(Color.FromArgb(255, 255, 0), 2F);//画笔

            //绘制背景

            graphicsDraw.FillRectangle(solidbrushDraw, rectanglePaintBackground);//绘制背景

            //绘制相机图示

            graphicsDraw.DrawImage(bitmapBackground[system.Work.CurrentPage], new Rectangle(system.UIParameter.Work_BackgroundImage_Location[system.Work.CurrentPage].X, system.UIParameter.Work_BackgroundImage_Location[system.Work.CurrentPage].Y, bitmapBackground[system.Work.CurrentPage].Width, bitmapBackground[system.Work.CurrentPage].Height));//绘制

            //绘制相机指示

            if (null != system.Camera)
            {
                for (i = 0; i < system.Camera.Count; i++)//遍历相机
                {
                    if (system.Work.CurrentPage == system.Camera[i].UIParameter.Work_Page)//相机属于当前页
                    {
                        graphicsDraw.DrawLine(penDraw, system.Camera[i].UIParameter.Work_LineStart_Location, system.Camera[i].UIParameter.Work_LineEnd_Location);
                    }
                }
            }

            //

            if (system.UIParameter.Work_BackgroundImage_Zoom[system.Work.CurrentPage])//
            {
                pictureBoxBackground.SizeMode = PictureBoxSizeMode.Zoom;
            }
            else//
            {
                pictureBoxBackground.SizeMode = PictureBoxSizeMode.Normal;
            }

            pictureBoxBackground.Image = (Image)imageDisplay.Clone();
        }

        //----------------------------------------------------------------------
        // 功能说明：显示或隐藏相机图像显示控件
        // 输入参数：1.bVisible：显示或隐藏。取值范围：true，显示；false，隐藏
        //         2.customButtonCameraBackground：相机背景控件
        //         3.imageShowCamera：相机图像控件
        //         4.labelCameraName：相机名称控件
        //         5.customButtonCameraState：相机状态控件
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _ShowCameraDisplayControl(Boolean bVisible, CustomButton customButtonCameraBackground, ImageDisplay imageShowCamera, Label labelCameraName, CustomButton customButtonCameraState, CustomButton customButtonCameraSpeedValue, CustomButton customButtonCameraSpeedUnit)
        {
            customButtonCameraBackground.Visible = bVisible;//显示或隐藏相机背景控件
            imageShowCamera.Visible = bVisible;//显示或隐藏相机图像控件
            labelCameraName.Visible = bVisible;//显示或隐藏相机名称控件
            customButtonCameraState.Visible = bVisible;//显示或隐藏相机状态控件
            customButtonCameraSpeedValue.Visible = bVisible;//显示或隐藏相机速度数值控件
            customButtonCameraSpeedUnit.Visible = bVisible;//显示或隐藏相机速度单位控件
        }

        //----------------------------------------------------------------------
        // 功能说明：设置相机显示控件并显示控件
        // 输入参数：1.iCameraNumber：当前页中的相机序号
        //         2.iCameraIndex：相机数据数组索引值
        //         3.bInitial：是否未控件初始化或翻页。取值范围：true，是；false，否
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetCameraDisplayControl(int iCameraNumber, int iCameraIndex, Boolean bInitial)
        {
            if (1 == iCameraNumber)//相机控件1
            {
                _SetCameraDisplayControl(customButtonCameraBackground1, imageDisplayCamera1, labelCameraName1, customButtonCameraState1, customButtonCameraSpeedValue1, customButtonCameraSpeedUnit1, iCameraIndex, bInitial);//设置相机显示控件并显示控件
            }
            else if (2 == iCameraNumber)//相机控件2
            {
                _SetCameraDisplayControl(customButtonCameraBackground2, imageDisplayCamera2, labelCameraName2, customButtonCameraState2, customButtonCameraSpeedValue2, customButtonCameraSpeedUnit2, iCameraIndex, bInitial);//设置相机显示控件并显示控件
            }
            else if (3 == iCameraNumber)//相机控件3
            {
                _SetCameraDisplayControl(customButtonCameraBackground3, imageDisplayCamera3, labelCameraName3, customButtonCameraState3, customButtonCameraSpeedValue3, customButtonCameraSpeedUnit3, iCameraIndex, bInitial);//设置相机显示控件并显示控件
            }
            else if (4 == iCameraNumber)//相机控件4
            {
                _SetCameraDisplayControl(customButtonCameraBackground4, imageDisplayCamera4, labelCameraName4, customButtonCameraState4, customButtonCameraSpeedValue4, customButtonCameraSpeedUnit4, iCameraIndex, bInitial);//设置相机显示控件并显示控件
            }
            else if (5 == iCameraNumber)//相机控件5
            {
                _SetCameraDisplayControl(customButtonCameraBackground5, imageDisplayCamera5, labelCameraName5, customButtonCameraState5, customButtonCameraSpeedValue5, customButtonCameraSpeedUnit5, iCameraIndex, bInitial);//设置相机显示控件并显示控件
            }
            else if (6 == iCameraNumber)//相机控件6
            {
                _SetCameraDisplayControl(customButtonCameraBackground6, imageDisplayCamera6, labelCameraName6, customButtonCameraState6, customButtonCameraSpeedValue6, customButtonCameraSpeedUnit6, iCameraIndex, bInitial);//设置相机显示控件并显示控件
            }
            else if (7 == iCameraNumber)//相机控件7
            {
                _SetCameraDisplayControl(customButtonCameraBackground7, imageDisplayCamera7, labelCameraName7, customButtonCameraState7, customButtonCameraSpeedValue7, customButtonCameraSpeedUnit7, iCameraIndex, bInitial);//设置相机显示控件并显示控件
            }
            else if (8 == iCameraNumber)//相机控件8
            {
                _SetCameraDisplayControl(customButtonCameraBackground8, imageDisplayCamera8, labelCameraName8, customButtonCameraState8, customButtonCameraSpeedValue8, customButtonCameraSpeedUnit8, iCameraIndex, bInitial);//设置相机显示控件并显示控件
            }
            else if (9 == iCameraNumber)//相机控件9
            {
                _SetCameraDisplayControl(customButtonCameraBackground9, imageDisplayCamera9, labelCameraName9, customButtonCameraState9, customButtonCameraSpeedValue9, customButtonCameraSpeedUnit9, iCameraIndex, bInitial);//设置相机显示控件并显示控件
            }
            else if (10 == iCameraNumber)//10 == iCameraNumber，相机控件10
            {
                _SetCameraDisplayControl(customButtonCameraBackground10, imageDisplayCamera10, labelCameraName10, customButtonCameraState10, customButtonCameraSpeedValue10, customButtonCameraSpeedUnit10, iCameraIndex, bInitial);//设置相机显示控件并显示控件
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：设置相机显示控件并显示控件
        // 输入参数：1.customButtonCameraBackground：相机背景控件
        //         2.imageShowCamera：相机图像控件
        //         3.labelCameraName：相机名称控件
        //         4.customButtonCameraState：相机状态控件
        //         5.iCameraIndex：相机数据数组索引值
        //         6.bInitial：是否未控件初始化或翻页。取值范围：true，是；false，否
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetCameraDisplayControl(CustomButton customButtonCameraBackground, ImageDisplay imageShowCamera, Label labelCameraName, CustomButton customButtonCameraState, CustomButton customButtonCameraSpeedValue, CustomButton customButtonCameraSpeedUnit, Int32 iCameraIndex, Boolean bInitial)
        {
            _SetLiveImage(imageShowCamera, iCameraIndex);//设置相机控件图像数据

            _SetDeviceData(imageShowCamera, labelCameraName, customButtonCameraState, iCameraIndex);//设置相机设备信息
            
            //

            if (bInitial)//初始
            {
                customButtonCameraBackground.Location = new Point(system.Camera[iCameraIndex].UIParameter.Work_ImageDisplay_Location.X - system.Camera[iCameraIndex].UIParameter.Work_ImageDisplayBackground_Left,
                                                                  system.Camera[iCameraIndex].UIParameter.Work_ImageDisplay_Location.Y - system.Camera[iCameraIndex].UIParameter.Work_ImageDisplayBackground_Top
                                                                  );
                customButtonCameraBackground.SizeButton = new Size(system.Camera[iCameraIndex].UIParameter.Work_ImageDisplay_Size.Width + system.Camera[iCameraIndex].UIParameter.Work_ImageDisplayBackground_Left * 2,
                                                                   system.Camera[iCameraIndex].UIParameter.Work_ImageDisplay_Size.Height + system.Camera[iCameraIndex].UIParameter.Work_ImageDisplayBackground_Top * 2
                                                                   );

                imageShowCamera.Location = system.Camera[iCameraIndex].UIParameter.Work_ImageDisplay_Location;
                imageShowCamera.ControlSize = system.Camera[iCameraIndex].UIParameter.Work_ImageDisplay_Size;
                //
                imageShowCamera.MessageLocation = system.Camera[iCameraIndex].UIParameter.Work_ImageDisplay_Message_Location;
                imageShowCamera.MessageSize = system.Camera[iCameraIndex].UIParameter.Work_ImageDisplay_Message_Size;
                imageShowCamera.MessageFont = new Font(imageShowCamera.MessageFont.Name, system.Camera[iCameraIndex].UIParameter.Work_ImageDisplay_Message_FontSize, imageShowCamera.MessageFont.Style, imageShowCamera.MessageFont.Unit, imageShowCamera.MessageFont.GdiCharSet);
                //
                imageShowCamera.SlotLocation = system.Camera[iCameraIndex].UIParameter.Work_ImageDisplay_Slot_Location;
                imageShowCamera.SlotSize = system.Camera[iCameraIndex].UIParameter.Work_ImageDisplay_Slot_Size;
                //
                imageShowCamera.MinValueLocation = system.Camera[iCameraIndex].UIParameter.Work_ImageDisplay_MinValue_Location;
                imageShowCamera.MinValueSize = system.Camera[iCameraIndex].UIParameter.Work_ImageDisplay_MinValue_Size;
                imageShowCamera.MinValueFont = new Font(imageShowCamera.MinValueFont.Name, system.Camera[iCameraIndex].UIParameter.Work_ImageDisplay_MinValue_FontSize, imageShowCamera.MinValueFont.Style, imageShowCamera.MinValueFont.Unit, imageShowCamera.MinValueFont.GdiCharSet);
                //
                imageShowCamera.CurrentValueLocation = system.Camera[iCameraIndex].UIParameter.Work_ImageDisplay_CurrentValue_Location;
                imageShowCamera.CurrentValueSize = system.Camera[iCameraIndex].UIParameter.Work_ImageDisplay_CurrentValue_Size;
                imageShowCamera.CurrentValueFont = new Font(imageShowCamera.CurrentValueFont.Name, system.Camera[iCameraIndex].UIParameter.Work_ImageDisplay_CurrentValue_FontSize, imageShowCamera.CurrentValueFont.Style, imageShowCamera.CurrentValueFont.Unit, imageShowCamera.CurrentValueFont.GdiCharSet);
                //
                imageShowCamera.MaxValueLocation = system.Camera[iCameraIndex].UIParameter.Work_ImageDisplay_MaxValue_Location;
                imageShowCamera.MaxValueSize = system.Camera[iCameraIndex].UIParameter.Work_ImageDisplay_MaxValue_Size;
                imageShowCamera.MaxValueFont = new Font(imageShowCamera.MaxValueFont.Name, system.Camera[iCameraIndex].UIParameter.Work_ImageDisplay_MaxValue_FontSize, imageShowCamera.MaxValueFont.Style, imageShowCamera.MaxValueFont.Unit, imageShowCamera.MaxValueFont.GdiCharSet);
                //
                imageShowCamera.MessageLampLocation = system.Camera[iCameraIndex].UIParameter.Work_ImageDisplay_Lamp_Location;
                imageShowCamera.MessageLampSize = system.Camera[iCameraIndex].UIParameter.Work_ImageDisplay_Lamp_Size;

                labelCameraName.Location = system.Camera[iCameraIndex].UIParameter.Work_Name_Location;
                labelCameraName.Size = system.Camera[iCameraIndex].UIParameter.Work_Name_Size;
                labelCameraName.Font = new Font(labelCameraName.Font.Name, system.Camera[iCameraIndex].UIParameter.Work_Name_FontSize, labelCameraName.Font.Style, labelCameraName.Font.Unit, labelCameraName.Font.GdiCharSet);

                customButtonCameraState.Location = system.Camera[iCameraIndex].UIParameter.Work_State_Location;
                customButtonCameraState.SizeButton = system.Camera[iCameraIndex].UIParameter.Work_State_Size;
                customButtonCameraState.FontText = new Font(customButtonCameraState.FontText.Name, system.Camera[iCameraIndex].UIParameter.Work_State_FontSize, customButtonCameraState.FontText.Style, customButtonCameraState.FontText.Unit, customButtonCameraState.FontText.GdiCharSet);

                customButtonCameraSpeedValue.Location = system.Camera[iCameraIndex].UIParameter.Work_SpeedPhase_Value_Location;
                customButtonCameraSpeedValue.SizeButton = system.Camera[iCameraIndex].UIParameter.Work_SpeedPhase_Value_Size;
                customButtonCameraSpeedValue.FontText = new Font(customButtonCameraSpeedValue.FontText.Name, system.Camera[iCameraIndex].UIParameter.Work_SpeedPhase_Value_FontSize, customButtonCameraSpeedValue.FontText.Style, customButtonCameraSpeedValue.FontText.Unit, customButtonCameraSpeedValue.FontText.GdiCharSet);

                customButtonCameraSpeedUnit.Location = system.Camera[iCameraIndex].UIParameter.Work_SpeedPhase_Unit_Location;
                customButtonCameraSpeedUnit.SizeButton = system.Camera[iCameraIndex].UIParameter.Work_SpeedPhase_Unit_Size;
                customButtonCameraSpeedUnit.FontText = new Font(customButtonCameraSpeedUnit.FontText.Name, system.Camera[iCameraIndex].UIParameter.Work_SpeedPhase_Unit_FontSize, customButtonCameraSpeedUnit.FontText.Style, customButtonCameraSpeedUnit.FontText.Unit, customButtonCameraSpeedUnit.FontText.GdiCharSet);

                //

                customButtonCameraBackground.Visible = true;//显示相机背景控件
                labelCameraName.Visible = true;//显示相机名称控件
                customButtonCameraState.Visible = true;//显示相机状态控件
                if (system.Camera[iCameraIndex].UIParameter.SpeedPhase_Display)//显示
                {
                    customButtonCameraSpeedValue.Visible = true;//显示相机速度数值控件
                    customButtonCameraSpeedUnit.Visible = true;//显示相机速度单位控件
                }
                else
                {
                    customButtonCameraSpeedValue.Visible = false;//显示相机速度数值控件
                    customButtonCameraSpeedUnit.Visible = false;//显示相机速度单位控件
                }
            }
            _SetLiveBackground(customButtonCameraBackground, iCameraIndex);//设置相机控件背景
        }

        //----------------------------------------------------------------------
        // 功能说明：隐藏相机图像显示控件
        // 输入参数：1.iShowCameraDisplayControlNumber：当前显示的相机图像显示控件数目
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _HideCameraDisplayControl(Int32 iShowCameraDisplayControlNumber)
        {
            Int32 i = 0;//循环控制变量

            for (i = iShowCameraDisplayControlNumber; i < iCameraDisplayControlNumber; i++)//恢复初始值
            {
                iIndexControl[i] = -1;//相机显示控件所对应的相机数组索引值，数组序号0 ~ 9对应相机显示控件1 ~ 10
            }

            if (0 == iShowCameraDisplayControlNumber)//隐藏相机图像显示控件1 ~ 10
            {
                _ShowCameraDisplayControl(false, customButtonCameraBackground1, imageDisplayCamera1, labelCameraName1, customButtonCameraState1, customButtonCameraSpeedValue1, customButtonCameraSpeedUnit1);//隐藏显示显示控件1
                _ShowCameraDisplayControl(false, customButtonCameraBackground2, imageDisplayCamera2, labelCameraName2, customButtonCameraState2, customButtonCameraSpeedValue2, customButtonCameraSpeedUnit2);//隐藏显示显示控件2
                _ShowCameraDisplayControl(false, customButtonCameraBackground3, imageDisplayCamera3, labelCameraName3, customButtonCameraState3, customButtonCameraSpeedValue3, customButtonCameraSpeedUnit3);//隐藏显示显示控件3
                _ShowCameraDisplayControl(false, customButtonCameraBackground4, imageDisplayCamera4, labelCameraName4, customButtonCameraState4, customButtonCameraSpeedValue4, customButtonCameraSpeedUnit4);//隐藏显示显示控件4
                _ShowCameraDisplayControl(false, customButtonCameraBackground5, imageDisplayCamera5, labelCameraName5, customButtonCameraState5, customButtonCameraSpeedValue5, customButtonCameraSpeedUnit5);//隐藏显示显示控件5
                _ShowCameraDisplayControl(false, customButtonCameraBackground6, imageDisplayCamera6, labelCameraName6, customButtonCameraState6, customButtonCameraSpeedValue6, customButtonCameraSpeedUnit6);//隐藏显示显示控件6
                _ShowCameraDisplayControl(false, customButtonCameraBackground7, imageDisplayCamera7, labelCameraName7, customButtonCameraState7, customButtonCameraSpeedValue7, customButtonCameraSpeedUnit7);//隐藏显示显示控件7
                _ShowCameraDisplayControl(false, customButtonCameraBackground8, imageDisplayCamera8, labelCameraName8, customButtonCameraState8, customButtonCameraSpeedValue8, customButtonCameraSpeedUnit8);//隐藏显示显示控件8
                _ShowCameraDisplayControl(false, customButtonCameraBackground9, imageDisplayCamera9, labelCameraName9, customButtonCameraState9, customButtonCameraSpeedValue9, customButtonCameraSpeedUnit9);//隐藏显示显示控件9
                _ShowCameraDisplayControl(false, customButtonCameraBackground10, imageDisplayCamera10, labelCameraName10, customButtonCameraState10, customButtonCameraSpeedValue10, customButtonCameraSpeedUnit10);//隐藏显示显示控件10
            }
            else if (1 == iShowCameraDisplayControlNumber)//隐藏相机图像显示控件2 ~ 10
            {
                _ShowCameraDisplayControl(false, customButtonCameraBackground2, imageDisplayCamera2, labelCameraName2, customButtonCameraState2, customButtonCameraSpeedValue2, customButtonCameraSpeedUnit2);//隐藏显示显示控件2
                _ShowCameraDisplayControl(false, customButtonCameraBackground3, imageDisplayCamera3, labelCameraName3, customButtonCameraState3, customButtonCameraSpeedValue3, customButtonCameraSpeedUnit3);//隐藏显示显示控件3
                _ShowCameraDisplayControl(false, customButtonCameraBackground4, imageDisplayCamera4, labelCameraName4, customButtonCameraState4, customButtonCameraSpeedValue4, customButtonCameraSpeedUnit4);//隐藏显示显示控件4
                _ShowCameraDisplayControl(false, customButtonCameraBackground5, imageDisplayCamera5, labelCameraName5, customButtonCameraState5, customButtonCameraSpeedValue5, customButtonCameraSpeedUnit5);//隐藏显示显示控件5
                _ShowCameraDisplayControl(false, customButtonCameraBackground6, imageDisplayCamera6, labelCameraName6, customButtonCameraState6, customButtonCameraSpeedValue6, customButtonCameraSpeedUnit6);//隐藏显示显示控件6
                _ShowCameraDisplayControl(false, customButtonCameraBackground7, imageDisplayCamera7, labelCameraName7, customButtonCameraState7, customButtonCameraSpeedValue7, customButtonCameraSpeedUnit7);//隐藏显示显示控件7
                _ShowCameraDisplayControl(false, customButtonCameraBackground8, imageDisplayCamera8, labelCameraName8, customButtonCameraState8, customButtonCameraSpeedValue8, customButtonCameraSpeedUnit8);//隐藏显示显示控件8
                _ShowCameraDisplayControl(false, customButtonCameraBackground9, imageDisplayCamera9, labelCameraName9, customButtonCameraState9, customButtonCameraSpeedValue9, customButtonCameraSpeedUnit9);//隐藏显示显示控件9
                _ShowCameraDisplayControl(false, customButtonCameraBackground10, imageDisplayCamera10, labelCameraName10, customButtonCameraState10, customButtonCameraSpeedValue10, customButtonCameraSpeedUnit10);//隐藏显示显示控件10
            }
            else if (2 == iShowCameraDisplayControlNumber)//隐藏相机图像显示控件3 ~ 10
            {
                _ShowCameraDisplayControl(false, customButtonCameraBackground3, imageDisplayCamera3, labelCameraName3, customButtonCameraState3, customButtonCameraSpeedValue3, customButtonCameraSpeedUnit3);//隐藏显示显示控件3
                _ShowCameraDisplayControl(false, customButtonCameraBackground4, imageDisplayCamera4, labelCameraName4, customButtonCameraState4, customButtonCameraSpeedValue4, customButtonCameraSpeedUnit4);//隐藏显示显示控件4
                _ShowCameraDisplayControl(false, customButtonCameraBackground5, imageDisplayCamera5, labelCameraName5, customButtonCameraState5, customButtonCameraSpeedValue5, customButtonCameraSpeedUnit5);//隐藏显示显示控件5
                _ShowCameraDisplayControl(false, customButtonCameraBackground6, imageDisplayCamera6, labelCameraName6, customButtonCameraState6, customButtonCameraSpeedValue6, customButtonCameraSpeedUnit6);//隐藏显示显示控件6
                _ShowCameraDisplayControl(false, customButtonCameraBackground7, imageDisplayCamera7, labelCameraName7, customButtonCameraState7, customButtonCameraSpeedValue7, customButtonCameraSpeedUnit7);//隐藏显示显示控件7
                _ShowCameraDisplayControl(false, customButtonCameraBackground8, imageDisplayCamera8, labelCameraName8, customButtonCameraState8, customButtonCameraSpeedValue8, customButtonCameraSpeedUnit8);//隐藏显示显示控件8
                _ShowCameraDisplayControl(false, customButtonCameraBackground9, imageDisplayCamera9, labelCameraName9, customButtonCameraState9, customButtonCameraSpeedValue9, customButtonCameraSpeedUnit9);//隐藏显示显示控件9
                _ShowCameraDisplayControl(false, customButtonCameraBackground10, imageDisplayCamera10, labelCameraName10, customButtonCameraState10, customButtonCameraSpeedValue10, customButtonCameraSpeedUnit10);//隐藏显示显示控件10
            }
            else if (3 == iShowCameraDisplayControlNumber)//隐藏相机图像显示控件4 ~ 10
            {
                _ShowCameraDisplayControl(false, customButtonCameraBackground4, imageDisplayCamera4, labelCameraName4, customButtonCameraState4, customButtonCameraSpeedValue4, customButtonCameraSpeedUnit4);//隐藏显示显示控件4
                _ShowCameraDisplayControl(false, customButtonCameraBackground5, imageDisplayCamera5, labelCameraName5, customButtonCameraState5, customButtonCameraSpeedValue5, customButtonCameraSpeedUnit5);//隐藏显示显示控件5
                _ShowCameraDisplayControl(false, customButtonCameraBackground6, imageDisplayCamera6, labelCameraName6, customButtonCameraState6, customButtonCameraSpeedValue6, customButtonCameraSpeedUnit6);//隐藏显示显示控件6
                _ShowCameraDisplayControl(false, customButtonCameraBackground7, imageDisplayCamera7, labelCameraName7, customButtonCameraState7, customButtonCameraSpeedValue7, customButtonCameraSpeedUnit7);//隐藏显示显示控件7
                _ShowCameraDisplayControl(false, customButtonCameraBackground8, imageDisplayCamera8, labelCameraName8, customButtonCameraState8, customButtonCameraSpeedValue8, customButtonCameraSpeedUnit8);//隐藏显示显示控件8
                _ShowCameraDisplayControl(false, customButtonCameraBackground9, imageDisplayCamera9, labelCameraName9, customButtonCameraState9, customButtonCameraSpeedValue9, customButtonCameraSpeedUnit9);//隐藏显示显示控件9
                _ShowCameraDisplayControl(false, customButtonCameraBackground10, imageDisplayCamera10, labelCameraName10, customButtonCameraState10, customButtonCameraSpeedValue10, customButtonCameraSpeedUnit10);//隐藏显示显示控件10
            }
            else if (4 == iShowCameraDisplayControlNumber)//隐藏相机图像显示控件5 ~ 10
            {
                _ShowCameraDisplayControl(false, customButtonCameraBackground5, imageDisplayCamera5, labelCameraName5, customButtonCameraState5, customButtonCameraSpeedValue5, customButtonCameraSpeedUnit5);//隐藏显示显示控件5
                _ShowCameraDisplayControl(false, customButtonCameraBackground6, imageDisplayCamera6, labelCameraName6, customButtonCameraState6, customButtonCameraSpeedValue6, customButtonCameraSpeedUnit6);//隐藏显示显示控件6
                _ShowCameraDisplayControl(false, customButtonCameraBackground7, imageDisplayCamera7, labelCameraName7, customButtonCameraState7, customButtonCameraSpeedValue7, customButtonCameraSpeedUnit7);//隐藏显示显示控件7
                _ShowCameraDisplayControl(false, customButtonCameraBackground8, imageDisplayCamera8, labelCameraName8, customButtonCameraState8, customButtonCameraSpeedValue8, customButtonCameraSpeedUnit8);//隐藏显示显示控件8
                _ShowCameraDisplayControl(false, customButtonCameraBackground9, imageDisplayCamera9, labelCameraName9, customButtonCameraState9, customButtonCameraSpeedValue9, customButtonCameraSpeedUnit9);//隐藏显示显示控件9
                _ShowCameraDisplayControl(false, customButtonCameraBackground10, imageDisplayCamera10, labelCameraName10, customButtonCameraState10, customButtonCameraSpeedValue10, customButtonCameraSpeedUnit10);//隐藏显示显示控件10
            }
            else if (5 == iShowCameraDisplayControlNumber)//隐藏相机图像显示控件6 ~ 10
            {
                _ShowCameraDisplayControl(false, customButtonCameraBackground6, imageDisplayCamera6, labelCameraName6, customButtonCameraState6, customButtonCameraSpeedValue6, customButtonCameraSpeedUnit6);//隐藏显示显示控件6
                _ShowCameraDisplayControl(false, customButtonCameraBackground7, imageDisplayCamera7, labelCameraName7, customButtonCameraState7, customButtonCameraSpeedValue7, customButtonCameraSpeedUnit7);//隐藏显示显示控件7
                _ShowCameraDisplayControl(false, customButtonCameraBackground8, imageDisplayCamera8, labelCameraName8, customButtonCameraState8, customButtonCameraSpeedValue8, customButtonCameraSpeedUnit8);//隐藏显示显示控件8
                _ShowCameraDisplayControl(false, customButtonCameraBackground9, imageDisplayCamera9, labelCameraName9, customButtonCameraState9, customButtonCameraSpeedValue9, customButtonCameraSpeedUnit9);//隐藏显示显示控件9
                _ShowCameraDisplayControl(false, customButtonCameraBackground10, imageDisplayCamera10, labelCameraName10, customButtonCameraState10, customButtonCameraSpeedValue10, customButtonCameraSpeedUnit10);//隐藏显示显示控件10
            }
            else if (6 == iShowCameraDisplayControlNumber)//隐藏相机图像显示控件7 ~ 10
            {
                _ShowCameraDisplayControl(false, customButtonCameraBackground7, imageDisplayCamera7, labelCameraName7, customButtonCameraState7, customButtonCameraSpeedValue7, customButtonCameraSpeedUnit7);//隐藏显示显示控件7
                _ShowCameraDisplayControl(false, customButtonCameraBackground8, imageDisplayCamera8, labelCameraName8, customButtonCameraState8, customButtonCameraSpeedValue8, customButtonCameraSpeedUnit8);//隐藏显示显示控件8
                _ShowCameraDisplayControl(false, customButtonCameraBackground9, imageDisplayCamera9, labelCameraName9, customButtonCameraState9, customButtonCameraSpeedValue9, customButtonCameraSpeedUnit9);//隐藏显示显示控件9
                _ShowCameraDisplayControl(false, customButtonCameraBackground10, imageDisplayCamera10, labelCameraName10, customButtonCameraState10, customButtonCameraSpeedValue10, customButtonCameraSpeedUnit10);//隐藏显示显示控件10
            }
            else if (7 == iShowCameraDisplayControlNumber)//隐藏相机图像显示控件8 ~ 10
            {
                _ShowCameraDisplayControl(false, customButtonCameraBackground8, imageDisplayCamera8, labelCameraName8, customButtonCameraState8, customButtonCameraSpeedValue8, customButtonCameraSpeedUnit8);//隐藏显示显示控件8
                _ShowCameraDisplayControl(false, customButtonCameraBackground9, imageDisplayCamera9, labelCameraName9, customButtonCameraState9, customButtonCameraSpeedValue9, customButtonCameraSpeedUnit9);//隐藏显示显示控件9
                _ShowCameraDisplayControl(false, customButtonCameraBackground10, imageDisplayCamera10, labelCameraName10, customButtonCameraState10, customButtonCameraSpeedValue10, customButtonCameraSpeedUnit10);//隐藏显示显示控件10
            }
            else if (8 == iShowCameraDisplayControlNumber)//隐藏相机图像显示控件9 ~ 10
            {
                _ShowCameraDisplayControl(false, customButtonCameraBackground9, imageDisplayCamera9, labelCameraName9, customButtonCameraState9, customButtonCameraSpeedValue9, customButtonCameraSpeedUnit9);//隐藏显示显示控件9
                _ShowCameraDisplayControl(false, customButtonCameraBackground10, imageDisplayCamera10, labelCameraName10, customButtonCameraState10, customButtonCameraSpeedValue10, customButtonCameraSpeedUnit10);//隐藏显示显示控件10
            }
            else if (9 == iShowCameraDisplayControlNumber)//隐藏相机图像显示控件10
            {
                _ShowCameraDisplayControl(false, customButtonCameraBackground10, imageDisplayCamera10, labelCameraName10, customButtonCameraState10, customButtonCameraSpeedValue10, customButtonCameraSpeedUnit10);//隐藏显示显示控件10
            }
            else//10 == iShowCameraDisplayControlNumber
            {
                //不执行操作
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击相机图像显示控件时调用该函数，执行相应的操作
        // 输入参数：1.iCameraDisplayControlNumber：点击的相机图像显示控件序号（1 ~ iCameraDisplayControlNumber）
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _ClickCameraDisplay(Int32 iCameraDisplayControlNumber)
        {
            Int32 i = 0;
            bool bSelectNewControl = false;//是否选择了新的相机显示控件。取值范围：true，是；false，否

            if (VisionSystemClassLibrary.Enum.CameraType.None != system.Work.SelectedCameraType)//当前选择了某一相机显示控件
            {
                if (system.Work.SelectedCameraIndex != iIndexControl[iCameraDisplayControlNumber - 1])//选择了不同的控件
                {
                    _SelectCameraDisplayControl(false, iCameraDisplayControlNumberTemp, iSelectedCameraIndex);//取消选择相机图像显示控件
                    
                    bSelectNewControl = true;
                }
            }
            else//当前未选择任何相机控件
            {
                bSelectNewControl = true;
            }

            //

            if (bSelectNewControl)//选择了新的相机显示控件
            {
                system.Work.SelectedCameraType = system.Camera[iIndexControl[iCameraDisplayControlNumber - 1]].Type;//当前选中的相机类型
                system.Work.SelectedCameraIndex = (Int16)_GetCameraIndex(system.Work.SelectedCameraType);//当前选中的相机显示控件所对应的相机在相机数组中的索引值

                _SelectCameraDisplayControl(true, iCameraDisplayControlNumber, system.Work.SelectedCameraIndex);//选择相机图像显示控件
                iCameraDisplayControlNumberTemp = iCameraDisplayControlNumber;//更新被选中相机控件
                iSelectedCameraIndex = system.Work.SelectedCameraIndex;//更新被选中相机

                //

                _SetMenu();//设置菜单按钮控件
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：选择或取消选择相机显示控件
        // 输入参数：1.bSelected：选择或取消选择相机显示控件。取值范围：true，选择；false，取消选择
        //         2.iCameraDisplayControlNumber：相机图像显示控件序号（1 ~ iCameraDisplayControlNumber）
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SelectCameraDisplayControl(Boolean bSelected, Int32 iCameraDisplayControlNumber, Int32 iSelectedCameraIndex)
        {
            system.Camera[iSelectedCameraIndex].Live.CameraSelected = bSelected;//取消选择
            
            if (1 == iCameraDisplayControlNumber)//相机图像显示控件1
            {
                _SetLiveBackground(customButtonCameraBackground1, iSelectedCameraIndex);//设置相控件背景
            }
            else if (2 == iCameraDisplayControlNumber)//相机图像显示控件2
            {
                _SetLiveBackground(customButtonCameraBackground2, iSelectedCameraIndex);//设置相控件背景
            }
            else if (3 == iCameraDisplayControlNumber)//相机图像显示控件3
            {
                _SetLiveBackground(customButtonCameraBackground3, iSelectedCameraIndex);//设置相控件背景
            }
            else if (4 == iCameraDisplayControlNumber)//相机图像显示控件4
            {
                _SetLiveBackground(customButtonCameraBackground4, iSelectedCameraIndex);//设置相控件背景
            }
            else if (5 == iCameraDisplayControlNumber)//相机图像显示控件5
            {
                _SetLiveBackground(customButtonCameraBackground5, iSelectedCameraIndex);//设置相控件背景
            }
            else if (6 == iCameraDisplayControlNumber)//相机图像显示控件6
            {
                _SetLiveBackground(customButtonCameraBackground6, iSelectedCameraIndex);//设置相控件背景
            }
            else if (7 == iCameraDisplayControlNumber)//相机图像显示控件7
            {
                _SetLiveBackground(customButtonCameraBackground7, iSelectedCameraIndex);//设置相控件背景
            }
            else if (8 == iCameraDisplayControlNumber)//相机图像显示控件8
            {
                _SetLiveBackground(customButtonCameraBackground8, iSelectedCameraIndex);//设置相控件背景
            }
            else if (9 == iCameraDisplayControlNumber)//相机图像显示控件9
            {
                _SetLiveBackground(customButtonCameraBackground9, iSelectedCameraIndex);//设置相控件背景
            }
            else if (10 == iCameraDisplayControlNumber)//10 == iCameraDisplayControlNumber，相机图像显示控件10
            {
                _SetLiveBackground(customButtonCameraBackground10, iSelectedCameraIndex);//设置相控件背景
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：获取指定类型的相机在相机数组中的索引值
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private int _GetCameraIndex(VisionSystemClassLibrary.Enum.CameraType type)
        {
            int iReturn = 0;//函数返回值
            int i = 0;//循环控制变量

            if (null != system.Camera)
            {
                for (i = 0; i < system.Camera.Count; i++)//遍历相机
                {
                    if (type == system.Camera[i].Type)//符合要求
                    {
                        iReturn = i;

                        break;
                    }
                }
            }

            return iReturn;
        }

        //----------------------------------------------------------------------
        // 功能说明：获取相机图像显示控件序号
        // 输入参数：1.iCameraIndex：相机数索引值
        // 输出参数：无
        // 返回值：相机显示控件序号（1 ~ iCameraDisplayControlNumber）
        //----------------------------------------------------------------------
        private int _GetCameraDisplayControlNumber(Int32 iCameraIndex)
        {
            int iReturn = 0;//函数返回值
            int i = 0;//循环控制变量

            for (i = 0; i < iCameraDisplayControlNumber; i++)//获取数值
            {
                if (iCameraIndex == iIndexControl[i])//符合要求
                {
                    iReturn = i + 1;

                    break;
                }
            }

            return iReturn;
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【系统更新】按钮
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _ClickUpdate()
        {
            //显示更新窗口

            GlobalWindows.MessageDisplay_Window.WindowParameter = 70;//窗口特征数值
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.OkCancel;//包含【确定】和【取消】按钮
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
            if ("" != sHMIApplicationVersion && "" != sUpdateHMIApplicationVersion)//人机界面程序更新
            {
                if ("" != sControllerApplicationVersion && "" != sUpdateControllerApplicationVersion)//控制器程序更新
                {
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_2 = VisionSystemClassLibrary.Struct.System_UIParameter.HMIApplicationName;
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_2 = VisionSystemClassLibrary.Struct.System_UIParameter.HMIApplicationName;
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] + " " + sHMIApplicationVersion + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][1] + " " + sUpdateHMIApplicationVersion;
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] + " " + sHMIApplicationVersion + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][1] + " " + sUpdateHMIApplicationVersion;

                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = VisionSystemClassLibrary.Struct.System_UIParameter.ControllerApplicationName;
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = VisionSystemClassLibrary.Struct.System_UIParameter.ControllerApplicationName;
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_6 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] + " " + sControllerApplicationVersion + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][1] + " " + sUpdateControllerApplicationVersion;
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_6 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] + " " + sControllerApplicationVersion + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][1] + " " + sUpdateControllerApplicationVersion;

                    //

                    windowResult = VisionSystemClassLibrary.Enum.UpdateApplicationResult.HMI | VisionSystemClassLibrary.Enum.UpdateApplicationResult.Controller;//升级操作结果
                }
                else//控制器程序未更新
                {
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = VisionSystemClassLibrary.Struct.System_UIParameter.HMIApplicationName;
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = VisionSystemClassLibrary.Struct.System_UIParameter.HMIApplicationName;
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] + " " + sHMIApplicationVersion + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][1] + " " + sUpdateHMIApplicationVersion;
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] + " " + sHMIApplicationVersion + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][1] + " " + sUpdateHMIApplicationVersion;

                    //

                    windowResult = VisionSystemClassLibrary.Enum.UpdateApplicationResult.HMI;//升级操作结果
                }

                //

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
            else if ("" != sControllerApplicationVersion && "" != sUpdateControllerApplicationVersion)//控制器程序更新
            {
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = VisionSystemClassLibrary.Struct.System_UIParameter.ControllerApplicationName;
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = VisionSystemClassLibrary.Struct.System_UIParameter.ControllerApplicationName;
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] + " " + sControllerApplicationVersion + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][1] + " " + sUpdateControllerApplicationVersion;
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] + " " + sControllerApplicationVersion + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][1] + " " + sUpdateControllerApplicationVersion;

                //

                windowResult = VisionSystemClassLibrary.Enum.UpdateApplicationResult.Controller;//升级操作结果

                //

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
            else//其它
            {
                //不执行操作
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
        private void WorkControl_Load(object sender, EventArgs e)
        {
            //_SetDefault();//设置默认值

            //

            timerTrademarkClick.Start();//启动
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【BRAND MANAGEMENT】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonBrandManagement_CustomButton_Click(object sender, EventArgs e)
        {
            if (_CheckPassword(9))//密码检查
            {
                if (null != BrandManagement_Click)//有效
                {
                    BrandManagement_Click(this, new EventArgs());
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【LIVE】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonLive_CustomButton_Click(object sender, EventArgs e)
        {
            //if (_CheckPassword(12))//密码检查
            //{
                if (null != Live_Click)//有效
                {
                    Live_Click(this, new EventArgs());
                }
            //}
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【REJECTS】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonRejects_CustomButton_Click(object sender, EventArgs e)
        {
            //if (_CheckPassword(13))//密码检查
            //{
                if (null != Rejects_Click)//有效
                {
                    Rejects_Click(this, new EventArgs());
                }
            //}
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【SYSTEM】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonSystem_CustomButton_Click(object sender, EventArgs e)
        {
            if (_CheckPassword(7))//密码检查
            {
                if (null != System_Click)//有效
                {
                    System_Click(this, new EventArgs());
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【DEVICES SETUP】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonDevicesSetup_CustomButton_Click(object sender, EventArgs e)
        {
            if (_CheckPassword(8))//密码检查
            {
                if (null != DevicesSetup_Click)//有效
                {
                    DevicesSetup_Click(this, new EventArgs());
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【QUALITY CHECK】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonQualityCheck_CustomButton_Click(object sender, EventArgs e)
        {
            if (_CheckPassword(10))//密码检查
            {
                if (null != QualityCheck_Click)//有效
                {
                    QualityCheck_Click(this, new EventArgs());
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【TOLERANCES】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonTolerances_CustomButton_Click(object sender, EventArgs e)
        {
            if (_CheckPassword(11))//密码检查
            {
                if (null != Tolerances_Click)//有效
                {
                    Tolerances_Click(this, new EventArgs());
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【STATISTICS】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonStatistics_CustomButton_Click(object sender, EventArgs e)
        {
            //if (_CheckPassword(27))//密码检查
            //{
                if (null != Statistics_Click)//有效
                {
                    Statistics_Click(this, new EventArgs());
                }
            //}
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【Previous Page】按钮事件，更新按钮背景
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonPreviousPage_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickPage(true);//设置页面数据

            //

            if (null != PreviousPage_Click)//有效
            {
                PreviousPage_Click(this, new EventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【Next Page】按钮事件，更新按钮背景
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonNextPage_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickPage(false);//设置页面数据

            //

            if (null != NextPage_Click)//有效
            {
                NextPage_Click(this, new EventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【系统更新】按钮事件，更新按钮背景
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonUpdate_CustomButton_Click(object sender, EventArgs e)
        {
            if (_CheckPassword(14))//密码检查
            {
                _ClickUpdate();
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击左侧商标按钮事件，执行相应的操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonTrademarkLeft_Click(object sender, EventArgs e)
        {
            iClickTrademark += 1;

            if (2 == iClickTrademark)//显示“关于...”窗口
            {
                //if (0 == iData_Value)
                //{
                //    if (null != About_Click)//有效
                //    {
                //        About_Click(this, new EventArgs());
                //    }
                //}
                //else
                {
                    GlobalWindows.MessageDisplay_Window.WindowParameter = 71;//窗口特征数值
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = _GetProductFullName(system.MachineType[system.SelectedMachineType], system.ProductName, system.ProductModelNumber);
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = _GetProductFullName(system.MachineType[system.SelectedMachineType], system.ProductName, system.ProductModelNumber);
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][3] + " " + sProductKey;
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][3] + " " + sProductKey;
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][2] + " " + sVersion;
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][2] + " " + sVersion;

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

                //

                iClickTrademark = 0;
            }
            else if (3 == iClickTrademark)//显示“密码”窗口
            {
                _ShowPasswordWindow_ClickTrademark();
            }
            else//其它
            {
                //不执行操作
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击右侧商标按钮事件，执行相应的操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonTrademarkRight_Click(object sender, EventArgs e)
        {
            iClickTrademark += 2;

            if (4 == iClickTrademark)//显示“关于...”窗口
            {
                //if (0 == iData_Value)
                //{
                //    if (null != About_Click)//有效
                //    {
                //        About_Click(this, new EventArgs());
                //    }
                //}
                //else
                {
                    GlobalWindows.MessageDisplay_Window.WindowParameter = 72;//窗口特征数值
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = _GetProductFullName(system.MachineType[system.SelectedMachineType], system.ProductName, system.ProductModelNumber);
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = _GetProductFullName(system.MachineType[system.SelectedMachineType], system.ProductName, system.ProductModelNumber);
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][3] + " " + sProductKey;
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][3] + " " + sProductKey;
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][2] + sVersion;
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][2] + sVersion;

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

                //

                iClickTrademark = 0;
            }
            else if (3 == iClickTrademark)//显示“密码”窗口
            {
                _ShowPasswordWindow_ClickTrademark();
            }
            else//其它
            {
                //不执行操作
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：点击相机图像显示控件1事件，更新页面控件并执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void imageDisplayCamera1_Control_Click(object sender, EventArgs e)
        {
            _ClickCameraDisplay(1);//点击相机图像显示控件1

            //

            if (null != CameraDisplay_Click)//有效
            {
                CameraDisplay_Click(this, new EventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：双击相机图像显示控件1事件，更新页面控件并执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void imageDisplayCamera1_Control_DoubleClick(object sender, EventArgs e)
        {
            _ClickCameraDisplay(1);//双击相机图像显示控件1

            //

            if (null != CameraDisplay_DoubleClick)//有效
            {
                CameraDisplay_DoubleClick(this, new EventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击相机图像显示控件2事件，更新页面控件并执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void imageDisplayCamera2_Control_Click(object sender, EventArgs e)
        {
            _ClickCameraDisplay(2);//点击相机图像显示控件2

            //

            if (null != CameraDisplay_Click)//有效
            {
                CameraDisplay_Click(this, new EventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：双击相机图像显示控件2事件，更新页面控件并执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void imageDisplayCamera2_Control_DoubleClick(object sender, EventArgs e)
        {
            _ClickCameraDisplay(2);//双击相机图像显示控件2

            //

            if (null != CameraDisplay_DoubleClick)//有效
            {
                CameraDisplay_DoubleClick(this, new EventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击相机图像显示控件3事件，更新页面控件并执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void imageDisplayCamera3_Control_Click(object sender, EventArgs e)
        {
            _ClickCameraDisplay(3);//点击相机图像显示控件3

            //

            if (null != CameraDisplay_Click)//有效
            {
                CameraDisplay_Click(this, new EventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：双击相机图像显示控件3事件，更新页面控件并执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void imageDisplayCamera3_Control_DoubleClick(object sender, EventArgs e)
        {
            _ClickCameraDisplay(3);//双击相机图像显示控件3

            //

            if (null != CameraDisplay_DoubleClick)//有效
            {
                CameraDisplay_DoubleClick(this, new EventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击相机图像显示控件4事件，更新页面控件并执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void imageDisplayCamera4_Control_Click(object sender, EventArgs e)
        {
            _ClickCameraDisplay(4);//点击相机图像显示控件4

            //

            if (null != CameraDisplay_Click)//有效
            {
                CameraDisplay_Click(this, new EventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：双击相机图像显示控件4事件，更新页面控件并执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void imageDisplayCamera4_Control_DoubleClick(object sender, EventArgs e)
        {
            _ClickCameraDisplay(4);//双击相机图像显示控件4

            //

            if (null != CameraDisplay_DoubleClick)//有效
            {
                CameraDisplay_DoubleClick(this, new EventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击相机图像显示控件5事件，更新页面控件并执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void imageDisplayCamera5_Control_Click(object sender, EventArgs e)
        {
            _ClickCameraDisplay(5);//点击相机图像显示控件5

            //

            if (null != CameraDisplay_Click)//有效
            {
                CameraDisplay_Click(this, new EventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：双击相机图像显示控件5事件，更新页面控件并执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void imageDisplayCamera5_Control_DoubleClick(object sender, EventArgs e)
        {
            _ClickCameraDisplay(5);//双击相机图像显示控件5

            //

            if (null != CameraDisplay_DoubleClick)//有效
            {
                CameraDisplay_DoubleClick(this, new EventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击相机图像显示控件6事件，更新页面控件并执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void imageDisplayCamera6_Control_Click(object sender, EventArgs e)
        {
            _ClickCameraDisplay(6);//点击相机图像显示控件6

            //

            if (null != CameraDisplay_Click)//有效
            {
                CameraDisplay_Click(this, new EventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：双击相机图像显示控件6事件，更新页面控件并执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void imageDisplayCamera6_Control_DoubleClick(object sender, EventArgs e)
        {
            _ClickCameraDisplay(6);//双击相机图像显示控件6

            //

            if (null != CameraDisplay_DoubleClick)//有效
            {
                CameraDisplay_DoubleClick(this, new EventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击相机图像显示控件7事件，更新页面控件并执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void imageDisplayCamera7_Control_Click(object sender, EventArgs e)
        {
            _ClickCameraDisplay(7);//点击相机图像显示控件7

            //

            if (null != CameraDisplay_Click)//有效
            {
                CameraDisplay_Click(this, new EventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：双击相机图像显示控件7事件，更新页面控件并执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void imageDisplayCamera7_Control_DoubleClick(object sender, EventArgs e)
        {
            _ClickCameraDisplay(7);//双击相机图像显示控件7

            //

            if (null != CameraDisplay_DoubleClick)//有效
            {
                CameraDisplay_DoubleClick(this, new EventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击相机图像显示控件8事件，更新页面控件并执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void imageDisplayCamera8_Control_Click(object sender, EventArgs e)
        {
            _ClickCameraDisplay(8);//点击相机图像显示控件8

            //

            if (null != CameraDisplay_Click)//有效
            {
                CameraDisplay_Click(this, new EventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：双击相机图像显示控件8事件，更新页面控件并执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void imageDisplayCamera8_Control_DoubleClick(object sender, EventArgs e)
        {
            _ClickCameraDisplay(8);//双击相机图像显示控件8

            //

            if (null != CameraDisplay_DoubleClick)//有效
            {
                CameraDisplay_DoubleClick(this, new EventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击相机图像显示控件9事件，更新页面控件并执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void imageDisplayCamera9_Control_Click(object sender, EventArgs e)
        {
            _ClickCameraDisplay(9);//点击相机图像显示控件9

            //

            if (null != CameraDisplay_Click)//有效
            {
                CameraDisplay_Click(this, new EventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：双击相机图像显示控件9事件，更新页面控件并执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void imageDisplayCamera9_Control_DoubleClick(object sender, EventArgs e)
        {
            _ClickCameraDisplay(9);//双击相机图像显示控件9

            //

            if (null != CameraDisplay_DoubleClick)//有效
            {
                CameraDisplay_DoubleClick(this, new EventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击相机图像显示控件10事件，更新页面控件并执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void imageDisplayCamera10_Control_Click(object sender, EventArgs e)
        {
            _ClickCameraDisplay(10);//点击相机图像显示控件10

            //

            if (null != CameraDisplay_Click)//有效
            {
                CameraDisplay_Click(this, new EventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：双击相机图像显示控件10事件，更新页面控件并执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void imageDisplayCamera10_Control_DoubleClick(object sender, EventArgs e)
        {
            _ClickCameraDisplay(10);//双击相机图像显示控件10

            //

            if (null != CameraDisplay_DoubleClick)//有效
            {
                CameraDisplay_DoubleClick(this, new EventArgs());
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：定时器，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void timerTrademarkClick_Tick(object sender, EventArgs e)
        {
            if (0 != iClickTrademark)//判断
            {
                iClickTrademarkCount++;//累加

                if (5 <= iClickTrademarkCount)//复位
                {
                    iClickTrademark = 0;

                    iClickTrademarkCount = 0;
                }
            }
            else//恢复
            {
                iClickTrademarkCount = 0;
            }
        }

        //

        ////----------------------------------------------------------------------
        //// 功能说明：WORK，点击TRADEMARK图标显示密码窗口，窗口关闭时产生的事件，执行相关操作
        //// 输入参数：1.sender：控件自身的引用
        ////         2.e：事件传递的参数
        //// 输出参数：无
        //// 返回值：无
        ////----------------------------------------------------------------------
        //private void standardKeyboardWindow_WindowClose_Work_Trademark(object sender, EventArgs e)
        //{
        //    if (GlobalWindows.TopMostWindows)//置顶
        //    {
        //        GlobalWindows.StandardKeyboard_Window.TopMost = false;//取消置于顶层
        //    }
        //    else//其它
        //    {
        //        GlobalWindows.StandardKeyboard_Window.Visible = false;//隐藏
        //    }

        //    //

        //    if (GlobalWindows.StandardKeyboard_Window.EnterNewValue)//输入完成
        //    {
        //        system.UserPassword = GlobalWindows.StandardKeyboard_Window.StringValue;

        //        //

        //        system._WriteSystemParameter();//写文件

        //        //

        //        iPasswordType = 1;//1，用户密码

        //        //事件

        //        if (null != PasswordEnter)//有效
        //        {
        //            PasswordEnter(this, new EventArgs());
        //        }
        //    }
        //    else//未输入
        //    {
        //        if (system.ServicePassword == GlobalWindows.StandardKeyboard_Window.StringValue)//维护密码
        //        {
        //            iPasswordType = 3;//3，维护密码

        //            //显示提示信息窗口

        //            GlobalWindows.MessageDisplay_Window.WindowParameter = 76;//窗口特征数值
        //            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.OkCancel;//包含【确定】和【取消】按钮
        //            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
        //            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][5] + "？";
        //            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][5] + "？";

        //            GlobalWindows.MessageDisplay_Window.StartPosition = FormStartPosition.CenterScreen;
        //            if (GlobalWindows.TopMostWindows)//置顶
        //            {
        //                GlobalWindows.MessageDisplay_Window.TopMost = true;//将窗口置于顶层
        //            }
        //            else//其它
        //            {
        //                GlobalWindows.MessageDisplay_Window.Visible = true;//显示
        //            }
        //        }
        //        else//其它
        //        {
        //            iPasswordType = 0;//0，未输入

        //            //事件

        //            if (null != PasswordEnter)//有效
        //            {
        //                PasswordEnter(this, new EventArgs());
        //            }
        //        }
        //    }

        //    //

        //    iClickTrademark = 0;
        //}

        //----------------------------------------------------------------------
        // 功能说明：WORK，点击【SYSTEM】按钮显示密码窗口，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void standardKeyboardWindow_WindowClose_Work_System(object sender, EventArgs e)
        {
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.StandardKeyboard_Window.TopMost = false;//取消置于顶层
            }
            else//其它
            {
                GlobalWindows.StandardKeyboard_Window.Visible = false;//隐藏
            }

            //

            if (GlobalWindows.StandardKeyboard_Window.EnterNewValue)//
            {
                //事件

                if (null != System_Click)//有效
                {
                    System_Click(this, new EventArgs());
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：WORK，点击【DEVICES SETUP】按钮显示密码窗口，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void standardKeyboardWindow_WindowClose_Work_DevicesSetup(object sender, EventArgs e)
        {
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.StandardKeyboard_Window.TopMost = false;//取消置于顶层
            }
            else//其它
            {
                GlobalWindows.StandardKeyboard_Window.Visible = false;//隐藏
            }

            //

            if (GlobalWindows.StandardKeyboard_Window.EnterNewValue)//
            {
                //事件

                if (null != DevicesSetup_Click)//有效
                {
                    DevicesSetup_Click(this, new EventArgs());
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：WORK，点击【BRAND MANAGEMENT】按钮显示密码窗口，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void standardKeyboardWindow_WindowClose_Work_BrandManagement(object sender, EventArgs e)
        {
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.StandardKeyboard_Window.TopMost = false;//取消置于顶层
            }
            else//其它
            {
                GlobalWindows.StandardKeyboard_Window.Visible = false;//隐藏
            }

            //

            if (GlobalWindows.StandardKeyboard_Window.EnterNewValue)//
            {
                //事件

                if (null != BrandManagement_Click)//有效
                {
                    BrandManagement_Click(this, new EventArgs());
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：WORK，点击【QUALITY CHECK】按钮显示密码窗口，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void standardKeyboardWindow_WindowClose_Work_QualityCheck(object sender, EventArgs e)
        {
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.StandardKeyboard_Window.TopMost = false;//取消置于顶层
            }
            else//其它
            {
                GlobalWindows.StandardKeyboard_Window.Visible = false;//隐藏
            }

            //

            if (GlobalWindows.StandardKeyboard_Window.EnterNewValue)//
            {
                //事件

                if (null != QualityCheck_Click)//有效
                {
                    QualityCheck_Click(this, new EventArgs());
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：WORK，点击【TOLERANCES】按钮显示密码窗口，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void standardKeyboardWindow_WindowClose_Work_Tolerances(object sender, EventArgs e)
        {
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.StandardKeyboard_Window.TopMost = false;//取消置于顶层
            }
            else//其它
            {
                GlobalWindows.StandardKeyboard_Window.Visible = false;//隐藏
            }

            //

            if (GlobalWindows.StandardKeyboard_Window.EnterNewValue)//
            {
                //事件

                if (null != Tolerances_Click)//有效
                {
                    Tolerances_Click(this, new EventArgs());
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：WORK，点击【LIVE】按钮显示密码窗口，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void standardKeyboardWindow_WindowClose_Work_Live(object sender, EventArgs e)
        {
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.StandardKeyboard_Window.TopMost = false;//取消置于顶层
            }
            else//其它
            {
                GlobalWindows.StandardKeyboard_Window.Visible = false;//隐藏
            }

            //

            if (GlobalWindows.StandardKeyboard_Window.EnterNewValue)//
            {
                //事件

                if (null != Live_Click)//有效
                {
                    Live_Click(this, new EventArgs());
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：WORK，点击【REJECTS】按钮显示密码窗口，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void standardKeyboardWindow_WindowClose_Work_Rejects(object sender, EventArgs e)
        {
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.StandardKeyboard_Window.TopMost = false;//取消置于顶层
            }
            else//其它
            {
                GlobalWindows.StandardKeyboard_Window.Visible = false;//隐藏
            }

            //

            if (GlobalWindows.StandardKeyboard_Window.EnterNewValue)//
            {
                //事件

                if (null != Rejects_Click)//有效
                {
                    Rejects_Click(this, new EventArgs());
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：WORK，点击【系统更新】按钮显示密码窗口，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void standardKeyboardWindow_WindowClose_Work_Update(object sender, EventArgs e)
        {
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.StandardKeyboard_Window.TopMost = false;//取消置于顶层
            }
            else//其它
            {
                GlobalWindows.StandardKeyboard_Window.Visible = false;//隐藏
            }

            //

            if (GlobalWindows.StandardKeyboard_Window.EnterNewValue)//
            {
                _ClickUpdate();
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：WORK，点击【STATISTICS】按钮显示密码窗口，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void standardKeyboardWindow_WindowClose_Work_Statistics(object sender, EventArgs e)
        {
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.StandardKeyboard_Window.TopMost = false;//取消置于顶层
            }
            else//其它
            {
                GlobalWindows.StandardKeyboard_Window.Visible = false;//隐藏
            }

            //

            if (GlobalWindows.StandardKeyboard_Window.EnterNewValue)//
            {
                //事件

                if (null != Statistics_Click)//有效
                {
                    Statistics_Click(this, new EventArgs());
                }
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：WORK，【系统更新】确认，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_Work_Update_Confirm(object sender, EventArgs e)
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

            if (GlobalWindows.MessageDisplay_Window.MessageDisplayControl.OkCancel)//启动更新
            {
                bUpdateMessageWindowShow = true;

                GlobalWindows.MessageDisplay_Window.WindowParameter = 92;//窗口特征数值
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.None;//不包含任何按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][6] + "...";
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][6] + "...";
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = " ";
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = " ";
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][9] + "，" + iTimerUpdateCount.ToString();
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][9] + "，" + iTimerUpdateCount.ToString();

                timerUpdate.Start();//启动定时器

                //事件

                if (null != Update_Click)//有效
                {
                    Update_Click(this, new EventArgs());
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：WORK，【系统更新】等待，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_Work_Update_Wait(object sender, EventArgs e)
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

            if (GlobalWindows.MessageDisplay_Window.MessageDisplayControl.OkCancel)//关闭窗口
            {
                //不执行操作
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：WORK，点击左侧TRADEMARK图标，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_Work_LeftTrademark(object sender, EventArgs e)
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
        // 功能说明：WORK，点击右侧TRADEMARK图标，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_Work_RightTrademark(object sender, EventArgs e)
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
        // 功能说明：WORK，退出程序确认，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_Work_ExitApplication_Confirm(object sender, EventArgs e)
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

            if (GlobalWindows.MessageDisplay_Window.MessageDisplayControl.OkCancel)//
            {
                //事件

                if (null != PasswordEnter)//有效
                {
                    PasswordEnter(this, new EventArgs());
                }
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：FAULT MESSAGE，点击【CLEAR ALL】时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void faultMessageWindow_ClearAllFaultMessages(object sender, EventArgs e)
        {
            //事件

            if (null != ClearAllFaultMessages)//有效
            {
                ClearAllFaultMessages(this, new EventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：FAULT MESSAGE，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void faultMessageWindow_WindowClose(object sender, EventArgs e)
        {
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.FaultMessage_Window.TopMost = false;//取消置于顶层
            }
            else//其它
            {
                GlobalWindows.FaultMessage_Window.Visible = false;//隐藏
            }
        }

        //
       
        //----------------------------------------------------------------------
        // 功能说明：FAULT MESSAGE OPTION，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void faultMessageOptionWindow_WindowClose(object sender, EventArgs e)
        {
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.FaultMessageOption_Window.TopMost = false;//取消置于顶层
            }
            else//其它
            {
                GlobalWindows.FaultMessageOption_Window.Visible = false;//隐藏
            }

            //

            if (GlobalWindows.FaultMessageOption_Window.FaultMessageOptionControl.EnterNewValue)//设置
            {
                VisionSystemClassLibrary.Class.System.MachineFaultEnableState = VisionSystemClassLibrary.Class.System._GetMachineFaultEnableState(GlobalWindows.FaultMessageOption_Window.FaultMessageOptionControl.FaultMessageState);
                VisionSystemClassLibrary.Class.System._WriteMachineStateInfoFile();

                //事件

                if (null != SetFaultMessageState)//有效
                {
                    SetFaultMessageState(this, new EventArgs());
                }
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：定时器事件，更新时,执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void timerUpdate_Tick(object sender, EventArgs e)
        {
            if (bUpdateMessageWindowShow)
            {
                iTimerUpdateCount--;

                if (0 >= iTimerUpdateCount)//超时
                {
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = "";
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = "";
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][8];
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][8];
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = "";
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = "";

                    timerUpdate.Stop();//关闭定时器

                    iTimerUpdateCount = iTimerUpdateMaxCount;
                }
                else//计数
                {
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][9] + "，" + iTimerUpdateCount.ToString();
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][9] + "，" + iTimerUpdateCount.ToString();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：NetDiagnose，网络查询产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void pictureBoxBackground_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //显示窗口

            GlobalWindows.NetDiagnose_Window.NetDiagnoseControl.Language = language;//语言
            GlobalWindows.NetDiagnose_Window.NetDiagnoseControl._Properties(system);//设备

            GlobalWindows.NetDiagnose_Window.StartPosition = FormStartPosition.CenterScreen;
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.NetDiagnose_Window.TopMost = true;//将窗口置于顶层
            }
            else//其它
            {
                GlobalWindows.NetDiagnose_Window.Visible = true;//显示
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：NetDiagnose_Window Ping事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void NetDiagnose_Window_Ping_Click(object sender, EventArgs e)
        {
            //事件

            if (null != Ping_Click)//有效
            {
                Ping_Click(this, new CustomEventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：NetDiagnose_Window连接事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void NetDiagnose_Window_Connect_Click(object sender, EventArgs e)
        {
            _StartCheckingNet();

            //事件

            if (null != Connect_Click)//有效
            {
                Connect_Click(this, new CustomEventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：NetDiagnose_Window关闭事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void NetDiagnose_Window_WindowClose(object sender, EventArgs e)
        {
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.NetDiagnose_Window.TopMost = false;//取消置于顶层
            }
            else//其它
            {
                GlobalWindows.NetDiagnose_Window.Visible = false;//隐藏
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：定时器事件，更新时,执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void timerNetCheck_Tick(object sender, EventArgs e)
        {
            if (bNetCheckMessageWindowShow)
            {
                iTimerNetCheckCount--;

                if (0 >= iTimerNetCheckCount)//超时
                {
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl._Reset();

                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = "";
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = "";
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][11];
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][11];
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = "";
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = "";

                    timerNetCheck.Stop();//关闭定时器

                    iTimerNetCheckCount = iTimerNetCheckMaxCount;

                    GlobalWindows.NetDiagnose_Window.NetDiagnoseControl._UpdateConnectState_Camera(GlobalWindows.NetDiagnose_Window.NetDiagnoseControl.CameraSelected, false);
                }
                else//计数
                {
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][10] + "，" + iTimerNetCheckCount.ToString();
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][10] + "，" + iTimerNetCheckCount.ToString();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：启动网络查询
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _StartCheckingNet()
        {
            bNetCheckMessageWindowShow = true;

            //显示信息对话框

            GlobalWindows.MessageDisplay_Window.WindowParameter = 106;//窗口特征数值
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.None;//不包含任何按钮
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][12] + "...";
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][12] + "...";
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = " ";
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = " ";
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][10] + "，" + iTimerNetCheckCount.ToString();
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][10] + "，" + iTimerNetCheckCount.ToString();

            GlobalWindows.MessageDisplay_Window.StartPosition = FormStartPosition.CenterScreen;
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.MessageDisplay_Window.TopMost = true;//将窗口置于顶层
            }
            else//其它
            {
                GlobalWindows.MessageDisplay_Window.Visible = true;//显示
            }

            timerNetCheck.Start();//启动定时器
        }

        //----------------------------------------------------------------------
        // 功能说明：NetDiagnose_Window，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void MessageDisplay_Window_WindowClose_NetCheck(object sender, EventArgs e)
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
        // 功能说明：应用设置完成的属性
        // 输入参数：1、string：sControllerName，控制器名称
        //           2、Boolean：bPingState，ping测试结果
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _UpdateNetCheck_Ping(string sControllerName, Boolean bPingState)
        {
            if (bPingState)//成功
            {
                if (GlobalWindows.TopMostWindows)//置顶
                {
                    GlobalWindows.MessageDisplay_Window.TopMost = false;//取消置于顶层
                }
                else//其它
                {
                    GlobalWindows.MessageDisplay_Window.Visible = false;//隐藏
                }
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl._Reset();
            }
            else//失败
            {
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = " ";
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = " ";
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][11];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][11];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = " ";
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = " ";
            }

            //

            GlobalWindows.NetDiagnose_Window.NetDiagnoseControl._UpdateConnectState_Controller(sControllerName, bPingState);
        }

        //----------------------------------------------------------------------
        // 功能说明：应用设置完成的属性
        // 输入参数：1、VisionSystemClassLibrary.Enum.CameraType：cameraType，相机类型
        //           2、Boolean：bConnectState，connect测试结果
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _UpdateNetCheck_Connect(VisionSystemClassLibrary.Enum.CameraType cameraType, Boolean bConnectState)
        {
            bNetCheckMessageWindowShow = false;

            iTimerNetCheckCount = iTimerNetCheckMaxCount;

            timerNetCheck.Stop();//关闭定时器

            //

            if (bConnectState)//成功
            {
                if (GlobalWindows.TopMostWindows)//置顶
                {
                    GlobalWindows.MessageDisplay_Window.TopMost = false;//取消置于顶层
                }
                else//其它
                {
                    GlobalWindows.MessageDisplay_Window.Visible = false;//隐藏
                }
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl._Reset();
            }
            else//失败
            {
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = " ";
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = " ";
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][11];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][11];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = " ";
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = " ";
            }

            //

            GlobalWindows.NetDiagnose_Window.NetDiagnoseControl._UpdateConnectState_Camera(cameraType, bConnectState);
        }
    }
}