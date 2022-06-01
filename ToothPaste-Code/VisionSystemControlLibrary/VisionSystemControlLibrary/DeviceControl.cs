/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：DeviceControl.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：DEVICE CONFIGURATION控件

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

using System.Net;
using System.Net.Sockets;

using System.Reflection;

namespace VisionSystemControlLibrary
{
    public partial class DeviceControl : UserControl
    {
        //该控件为Device Setup页面

        private VisionSystemClassLibrary.Enum.InterfaceLanguage language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;//属性，语言

        private VisionSystemClassLibrary.Enum.DeviceState devicestate = VisionSystemClassLibrary.Enum.DeviceState.Stop;//属性，设备状态

        //

        private int iDeviceNumber_Total = 0;//连接的设备数目（0表示无连接的设备）

        //

        private Boolean bClickConfigDeviceButton = false;//属性（只读），是否按下了【CONFIG DEVICE】按钮。取值范围：true，是；false，否
        private Boolean bClickParameterSettingsButton = false;//属性（只读），是否按下了【PARAMETER SETTINGS】按钮。取值范围：true，是；false，否
        private Boolean bClickTestIOButton = false;//属性（只读），是否按下了【TEST I/O】按钮。取值范围：true，是；false，否

        //

        private Boolean bConfigDeviceMessageWindowShow = false;//属性（只读），是否显示配置设备后的提示信息窗口。取值范围：true，是；false，否
        private Boolean bParameterSettingsMessageWindowShow = false;//属性（只读），是否显示参数设置后的提示信息窗口。取值范围：true，是；false，否

        private String sControllerConfigDevice = "";//属性（只读），配置的控制器

        private const Int32 iTimerConfigDeviceMaxCount = 180;//定时器时间
        private Int32 iTimerConfigDeviceCount = 180;//定时器时间

        //

        private VisionSystemClassLibrary.Enum.CameraType cameratypeSelected = VisionSystemClassLibrary.Enum.CameraType.None;//属性（只读），点击列表项时，记录选择的相机类型（防止列表被更新）

        //

        private VisionSystemClassLibrary.Class.System system = new VisionSystemClassLibrary.Class.System();//属性（只读），系统

        //

        private Point pointParameterSettings = new Point();//【PARAMETER SETTINGS】按钮原始位置
        private Point pointTestIO = new Point();//【TEST I/O】按钮原始位置
        private Point pointConfigImage = new Point();//【CONFIG IMAGE】按钮原始位置
        private Point pointAlignDateTime = new Point();//【ALIGN DATE/TIME】按钮原始位置

        //

        private String[][] sMessageText_1 = new String[2][];//固件版本控件上显示的文本（[语言][包含的文本]）

        private String[][] sMessageText_2 = new String[2][];//提示信息对话框上显示的文本（[语言][包含的文本]）

        //

        private Boolean[] bFaultExist = new Boolean[3] { false, false, false };//属性，每个相机的故障信息是否存在。取值范围：true，存在；false，不存在

        //

        [Browsable(true), Description("点击【返回】按钮时产生的事件"), Category("DeviceControl 事件")]
        public event EventHandler Close_Click;//点击【返回】按钮时产生的事件
        //CustomEventArgs参数含义：
        //无意义

        [Browsable(true), Description("点击【REFRESH LIST】按钮时产生的事件"), Category("DeviceControl 事件")]
        public event EventHandler RefreshList_Click;//点击【REFRESH LIST】按钮时产生的事件
        //CustomEventArgs参数含义：
        //无意义

        [Browsable(true), Description("点击【RESET DEVICE】按钮时产生的事件"), Category("DeviceControl 事件")]
        public event EventHandler ResetDevice_Click;//点击【RESET DEVICE】按钮时产生的事件
        //CustomEventArgs参数含义：
        //无意义

        [Browsable(true), Description("点击【CONFIG DEVICE】按钮时产生的事件"), Category("DeviceControl 事件")]
        public event EventHandler ConfigDevice_Click;//点击【CONFIG DEVICE】按钮时产生的事件
        //CustomEventArgs参数含义：
        //IntValue[0]：操作结果
        //IntValue[1]：配置的相机
        //IntValue[2]：配置的IP地址

        [Browsable(true), Description("点击PARAMETER SETTINGS窗口【保存参数】按钮时产生的事件"), Category("DeviceControl 事件")]
        public event EventHandler ParameterSettings_Save;//点击PARAMETER SETTINGS窗口【保存参数】按钮时产生的事件
        //CustomEventArgs参数含义：
        //无意义

        [Browsable(true), Description("点击【TEST I/O】按钮时产生的事件"), Category("DeviceControl 事件")]
        public event EventHandler TestIO_Click;//点击【TEST I/O】按钮时产生的事件
        //CustomEventArgs参数含义：
        //无意义

        [Browsable(true), Description("关闭TEST I/O窗口时产生的事件"), Category("DeviceControl 事件")]
        public event EventHandler TestIO_Close_Click;//关闭TEST I/O窗口时产生的事件
        //CustomEventArgs参数含义：
        //无意义

        [Browsable(true), Description("点击【CONFIG IMAGE】按钮时产生的事件"), Category("DeviceControl 事件")]
        public event EventHandler ConfigImage_Click;//点击【CONFIG IMAGE】按钮时产生的事件
        //CustomEventArgs参数含义：
        //无意义

        [Browsable(true), Description("点击【ALIGN DATE/TIME】按钮时产生的事件"), Category("DeviceControl 事件")]
        public event EventHandler AlignDateTime_Click;//点击【ALIGN DATE/TIME】按钮时产生的事件
        //CustomEventArgs参数含义：
        //无意义

        [Browsable(true), Description("点击【Previous Page】按钮时产生的事件"), Category("DeviceControl 事件")]
        public event EventHandler PreviousPage_Click;//点击【Previous Page】按钮时产生的事件
        //CustomEventArgs参数含义：
        //无意义

        [Browsable(true), Description("点击【Next Page】按钮时产生的事件"), Category("DeviceControl 事件")]
        public event EventHandler NextPage_Click;//点击【Next Page】按钮时产生的事件
        //CustomEventArgs参数含义：
        //无意义

        [Browsable(true), Description("点击设备项控件时产生的事件"), Category("DeviceControl 事件")]
        public event EventHandler DeviceItem_Click;//点击设备项控件时产生的事件
        //CustomEventArgs参数含义：
        //无意义

        //

        //----------------------------------------------------------------------
        // 功能说明：系统默认调用，构造函数，初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public DeviceControl()
        {
            InitializeComponent();

            //

            pointParameterSettings = customButtonParameterSettings.Location;//【PARAMETER SETTINGS】按钮原始位置
            pointTestIO = customButtonTestIO.Location;//【TEST I/O】按钮原始位置
            pointConfigImage = customButtonConfigImage.Location;//【CONFIG IMAGE】按钮原始位置
            pointAlignDateTime = customButtonAlignDateTime.Location;//【ALIGN DATE/TIME】按钮原始位置

            //

            //由于该控件仅有一个实例，因此可以在此处进行事件的订阅

            if (null != GlobalWindows.MessageDisplay_Window)//有效
            {
                GlobalWindows.MessageDisplay_Window.WindowClose_DevicesSetup_ResetDevice_Confirm += new System.EventHandler(messageDisplayWindow_WindowClose_DevicesSetup_ResetDevice_Confirm);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_DevicesSetup_ResetDevice_Success += new System.EventHandler(messageDisplayWindow_WindowClose_DevicesSetup_ResetDevice_Success);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_DevicesSetup_ResetDevice_Failure += new System.EventHandler(messageDisplayWindow_WindowClose_DevicesSetup_ResetDevice_Failure);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_DevicesSetup_ConfigDevice_Wait += new System.EventHandler(messageDisplayWindow_WindowClose_DevicesSetup_ConfigDevice_Wait);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_DevicesSetup_AlignDateTime_Confirm += new System.EventHandler(messageDisplayWindow_WindowClose_DevicesSetup_AlignDateTime_Confirm);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_DevicesSetup_AlignDateTime_Success += new System.EventHandler(messageDisplayWindow_WindowClose_DevicesSetup_AlignDateTime_Success);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_DevicesSetup_AlignDateTime_Failure += new System.EventHandler(messageDisplayWindow_WindowClose_DevicesSetup_AlignDateTime_Failure);//订阅事件
            }

            if (null != GlobalWindows.DeviceConfiguration_Window)//有效
            {
                GlobalWindows.DeviceConfiguration_Window.WindowClose += new System.EventHandler(deviceConfigurationWindow_WindowClose);//订阅事件
            }

            if (null != GlobalWindows.IOSignalDiagnosis_Window)//有效
            {
                GlobalWindows.IOSignalDiagnosis_Window.WindowClose += new System.EventHandler(iOSignalDiagnosisWindow_WindowClose);//订阅事件
            }

            if (null != GlobalWindows.ParameterSettings_Window)//有效
            {
                GlobalWindows.ParameterSettings_Window.WindowClose += new System.EventHandler(parameterSettingsWindow_WindowClose);//订阅事件
                GlobalWindows.ParameterSettings_Window.SaveParameter += new System.EventHandler(parameterSettingsWindow_SaveParameter);//订阅事件
            }

            //

            FieldInfo[] fieldinfo = VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese.GetType().GetFields();//获取公共字段
            if (null != fieldinfo && 1 < fieldinfo.Length)//有效
            {
                Int32 i = 0;//循环控制变量

                sMessageText_1 = new String[fieldinfo.Length - 1][];
                for (i = 0; i < fieldinfo.Length - 1; i++)
                {
                    sMessageText_1[i] = new String[1];

                    sMessageText_2[i] = new String[14];
                }

                //

                sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] = customButtonMessage2.Chinese_TextDisplay[0];
                sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] = customButtonMessage2.English_TextDisplay[0];

                //

                sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] = "确定复位";
                sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] = "Reset";

                sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][1] = "完成";
                sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][1] = "Completed";

                sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][2] = "失败";
                sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][2] = "Failed";

                sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][3] = "正在配置设备";
                sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][3] = "Configuring device";

                sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][4] = "设备";
                sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][4] = "Date and Time on";

                sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][5] = "中的日期时间已完成同步";
                sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][5] = "have been aligned to PC settings";

                sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][6] = "中的日期时间同步失败";
                sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][6] = "Aligned to PC settings Failed";

                sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][7] = "确定同步";
                sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][7] = "Align Date/Time on ";

                sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][8] = "正在配置设备";
                sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][8] = "Configuring device";

                sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][9] = "配置设备失败";
                sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][9] = "Config device Failed";

                sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][10] = "请等待";
                sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][10] = "Please wait";

                sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][11] = "请重试";
                sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][11] = "Please try again";

                sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][12] = "复位设备";
                sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][12] = "Reset of Device";

                sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][13] = "日期时间";
                sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][13] = "";
            }
        }

        //属性

        //-----------------------------------------------------------------------
        // 功能说明：Language属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("语言"), Category("DeviceControl 通用")]
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
        [Browsable(true), Description("设备状态"), Category("DeviceControl 通用")]
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
        // 功能说明：CameraSelected属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("点击列表项时，记录选择的相机类型（防止列表被更新）"), Category("DeviceControl 通用")]
        public VisionSystemClassLibrary.Enum.CameraType CameraSelected //属性
        {
            get//读取
            {
                return cameratypeSelected;
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：FaultExist属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("每个相机的故障信息是否存在。取值范围：true，存在；false，不存在"), Category("TitleBarControl 通用")]
        public Boolean[] FaultExist//属性
        {
            get//读取
            {
                return bFaultExist;
            }
            set//设置
            {
                if (value != bFaultExist)
                {
                    bFaultExist = value;
                }
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：ConfigDeviceMessageWindowShow属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("是否显示配置设备后的提示信息窗口。取值范围：true，是；false，否"), Category("DeviceControl 通用")]
        public Boolean ConfigDeviceMessageWindowShow //属性
        {
            get//读取
            {
                return bConfigDeviceMessageWindowShow;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ParameterSettingsMessageWindowShow属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("是否显示参数设置后的提示信息窗口。取值范围：true，是；false，否"), Category("DeviceControl 通用")]
        public Boolean ParameterSettingsMessageWindowShow //属性
        {
            get//读取
            {
                return bParameterSettingsMessageWindowShow;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ControllerConfigDevice属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("配置的控制器"), Category("DeviceControl 通用")]
        public String ControllerConfigDevice //属性
        {
            get//读取
            {
                return sControllerConfigDevice;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ConfigDeviceWindowShow属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("是否按下了【CONFIG DEVICE】按钮。取值范围：true，是；false，否"), Category("DeviceControl 通用")]
        public Boolean ConfigDeviceWindowShow //属性
        {
            get//读取
            {
                return bClickConfigDeviceButton;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ParameterSettingsWindowShow属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("是否按下了【PARAMETER SETTINGS】按钮。取值范围：true，是；false，否"), Category("DeviceControl 通用")]
        public Boolean ParameterSettingsWindowShow //属性
        {
            get//读取
            {
                return bClickParameterSettingsButton;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：TestIOWindowShow属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("是否按下了【TEST I/O】按钮。取值范围：true，是；false，否"), Category("DeviceControl 通用")]
        public Boolean TestIOWindowShow //属性
        {
            get//读取
            {
                return bClickTestIOButton;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ConfigDeviceWindow属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("CONFIG DEVICE窗口"), Category("DeviceControl 通用")]
        public DeviceConfigurationWindow ConfigDeviceWindow //属性
        {
            get//读取
            {
                return GlobalWindows.DeviceConfiguration_Window;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：TestIOWindow属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("TEST I/O窗口"), Category("DeviceControl 通用")]
        public IOSignalDiagnosisWindow TestIOWindow //属性
        {
            get//读取
            {
                return GlobalWindows.IOSignalDiagnosis_Window;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：系统类
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(false), Description("系统"), Category("DeviceControl 通用")]
        public VisionSystemClassLibrary.Class.System SystemData
        {
            get//读取
            {
                return system;
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：TotalPage属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("包含的页码总数（取值为0，表示无有效的列表项）"), Category("DeviceControl 通用")]
        public int TotalPage//属性
        {
            get//读取
            {
                return customList.TotalPage;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ItemControlNumber属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("列表项控件的数目"), Category("DeviceControl 通用")]
        public int ItemControlNumber//属性
        {
            get//读取
            {
                return customList.ItemControlNumber;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：CurrentPage属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("当前页码（从0开始。取值为-1，表示无有效的列表项）"), Category("DeviceControl 通用")]
        public int CurrentPage//属性
        {
            get//读取
            {
                return customList.CurrentPage;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：CurrentDataIndex属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("当前选择的项对应的外部数据数组的序号（0 ~ 外部数据数组长度 - 1。取值为-1，表示当前未选择任何项）"), Category("DeviceControl 通用")]
        public int CurrentDataIndex//属性
        {
            get//读取
            {
                return customList.CurrentDataIndex;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：CurrentListIndex属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("当前选择的项在列表中的序号（0 ~ iItemDataNumber - 1。取值为-1，表示当前未选择任何项）"), Category("DeviceControl 通用")]
        public int CurrentListIndex//属性
        {
            get//读取
            {
                return customList.CurrentListIndex;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：Index_Page属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("当前选择的项在当前页中的索引（0 ~ iItemControlNumber - 1。取值为-1，表示当前未选择任何项）"), Category("DeviceControl 通用")]
        public int Index_Page//属性
        {
            get//读取
            {
                return customList.Index_Page;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：StartIndex属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("当前页中的起始索引（0 ~ iItemDataNumber - 1）"), Category("DeviceControl 通用")]
        public int StartIndex//属性
        {
            get//读取
            {
                return customList.StartIndex;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：EndIndex属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("当前页中的结束索引（0 ~ iItemDataNumber - 1）"), Category("DeviceControl 通用")]
        public int EndIndex//属性
        {
            get//读取
            {
                return customList.EndIndex;
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

            _SetLanguage();//设置语言

            _SetDeviceState();//设备状态
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：应用设置完成的属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _Apply()
        {
            int i = 0;//循环控制变量

            //

            iDeviceNumber_Total = 0;//连接的设备数目（0表示无连接的设备）

            for (i = 0; i < system.ConnectionData.Length; i++)//遍历设备数组
            {
                if (system.ConnectionData[i].Connected && system.ConnectionData[i].GetDevInfo)//设备连接
                {
                    iDeviceNumber_Total++;//连接的设备数目（取值范围：0 ~ 256，0表示无连接的设备）
                }
            }

            //customList.ItemDataNumber = iDeviceNumber_Total;

            //

            //应用属性设置

            customList._ApplyListHeader();//应用列表头属性
            customList._ApplyListItem();//应用列表项属性

            customList._Apply(iDeviceNumber_Total);//应用列表属性

            //

            if (customList.ItemDataNumber > 0)//存在连接的设备
            {
                //添加列表项数据

                _AddListItemData();

                //设置列表项数据

                _SetPage();

                //更新页面控件

                customButtonMessage1.CurrentTextGroupIndex = 0;//设置显示的文本
                labelMessage1.Text = "";//设置显示的文本

            }
            else//不存在连接的设备
            {
                //设置列表项数据

                _SetPage();

                //更新页面控件

                customButtonMessage1.CurrentTextGroupIndex = 2;//设置显示的文本
                labelMessage1.Text = "";//设置显示的文本
            }

            //设置页面控件

            customButtonMessage3.CurrentTextGroupIndex = 2;//设置显示的文本

            if (1 >= customList.TotalPage)//小于等于1页
            {
                customButtonPreviousPage.Visible = false;//隐藏【Previous Page】按钮
                customButtonNextPage.Visible = false;//隐藏【Next Page】按钮
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：应用设置的属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetSystem()
        {
            Int32 i = 0;//循环控制变量

            if (null != system.Camera)
            {
                for (i = 0; i < system.Camera.Count; i++)
                {
                    if (null != system.Camera[i].DeviceParameter.Parameter)//
                    {
                        break;
                    }
                }
                if (i >= system.Camera.Count)//
                {
                    customButtonAlignDateTime.Location = pointConfigImage;
                    customButtonConfigImage.Location = pointTestIO;
                    customButtonTestIO.Location = pointParameterSettings;

                    customButtonParameterSettings.Visible = false;
                }
            }
            else
            {
                customButtonAlignDateTime.Location = pointConfigImage;
                customButtonConfigImage.Location = pointTestIO;
                customButtonTestIO.Location = pointParameterSettings;

                customButtonParameterSettings.Visible = false;
            }

            //

            _Apply();
        }

        //----------------------------------------------------------------------
        // 功能说明：设置语言
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetLanguage()
        {
            customButtonRefreshList.Language = language;//【REFRESH LIST】
            customButtonResetDevice.Language = language;//【RESET DEVICE】
            customButtonConfigDevice.Language = language;//【CONFIG DEVICE】
            customButtonParameterSettings.Language = language;//【PARAMETER SETTINGS】
            customButtonTestIO.Language = language;//【TEST I/O】
            customButtonConfigImage.Language = language;//【CONFIG IMAGE】
            customButtonAlignDateTime.Language = language;//【ALIGN DATE/TIME】

            //

            customButtonClose.Language = language;//【Close】
            customButtonPreviousPage.Language = language;//【Previous Page】
            customButtonNextPage.Language = language;//【Next Page】

            //

            customButtonCaption.Language = language;//标题文本
            if (customList.ItemDataNumber > 0)//存在连接的设备
            {
                if (0 <= customList.CurrentListIndex)//选择了列表中的项
                {
                    labelMessage1.Text = system.ConnectionData[customList.CurrentDataIndex].DeviceName;//更新控件文本
                }
            }
            customButtonMessage1.Language = language;//控件1文本

            customButtonMessage3.Language = language;//控件3文本

            //

            customList.Language = language;//列表语言
        }

        //----------------------------------------------------------------------
        // 功能说明：用户调用，设备状态设置完成后，更新页面
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetDeviceState()
        {
            //运行时无效

            if (VisionSystemClassLibrary.Enum.DeviceState.Run == devicestate)//运行
            {
                //更新按钮背景

                customButtonRefreshList.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【REFRESH LIST】按钮的背景
                customButtonPreviousPage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【Previous Page】按钮的背景
                customButtonNextPage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【Next Page】按钮的背景
                customButtonResetDevice.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【RESET DEVICE】按钮的背景
                customButtonParameterSettings.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【PARAMETER SETTINGS】按钮的背景
                customButtonConfigDevice.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【CONFIG DEVICE】按钮的背景
                customButtonTestIO.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【TEST I/O】按钮的背景
                customButtonConfigImage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【CONFIG IMAGE】按钮的背景
                customButtonAlignDateTime.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【ALIGN DATE/TIME】按钮的背景

                //

                //if (customList.CurrentPage == customList._GetPage(customList.CurrentListIndex))//当前选择的项在当前页
                //{
                //    customList._SelectListItem(customList.Index_Page, false);
                //}

                customList.ListEnabled = false;
            }
            else//停止
            {
                //更新按钮背景

                customButtonRefreshList.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//更新【REFRESH LIST】按钮的背景
                customButtonPreviousPage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//更新【Previous Page】按钮的背景
                customButtonNextPage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//更新【Next Page】按钮的背景
                _SetFunctionalButton();//设置功能性按钮背景

                //

                //if (customList.CurrentPage == customList._GetPage(customList.CurrentListIndex))//当前选择的项在当前页
                //{
                //    customList._SelectListItem(customList.Index_Page, true);
                //}

                customList.ListEnabled = true;

                Int32 i = 0;//循环控制变量
                Int32 j = 0;//循环控制变量
                Int32 value = 0;//临时变量

                for (i = 0; i < customList.ItemDataNumber; i++)//列表项数据
                {
                    for (j = value; j < system.ConnectionData.Length; j++)//遍历设备数组
                    {
                        if (system.ConnectionData[j].Connected && system.ConnectionData[j].GetDevInfo)//设备连接
                        {
                            value = j + 1;

                            //

                            break;
                        }
                    }

                    for (Int32 k = 0; k < system.Camera.Count; k++) //循环所有相机
                    {
                        if (system.Camera[k].Type == system.ConnectionData[j].Type) //相机类型相同
                        {
                            if (bFaultExist[k]) //故障相机查询到
                            {
                                if (i == customList.CurrentDataIndex) //当前选项相机故障
                                {
                                    //更新按钮背景

                                    customButtonPreviousPage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【Previous Page】按钮的背景
                                    customButtonNextPage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【Next Page】按钮的背景
                                    customButtonResetDevice.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【RESET DEVICE】按钮的背景
                                    customButtonParameterSettings.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【PARAMETER SETTINGS】按钮的背景
                                    customButtonConfigDevice.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【CONFIG DEVICE】按钮的背景
                                    customButtonTestIO.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【TEST I/O】按钮的背景
                                    customButtonConfigImage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【CONFIG IMAGE】按钮的背景
                                    customButtonAlignDateTime.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【ALIGN DATE/TIME】按钮的背景
                                }
                                customList._SetListItemEnable(i, false);

                                //
                                break;
                            }
                        }
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：获取相机索引值
        // 输入参数：1.cameratype：相机类型
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private Int32 _GetCameraIndex(VisionSystemClassLibrary.Enum.CameraType cameratype)
        {
            Int32 iReturn = 0;

            for (Int32 i = 0; i < system.Camera.Count; i++)
            {
                if (cameratype == system.Camera[i].Type)//目标
                {
                    iReturn = i;

                    break;
                }
            }

            return iReturn;
        }

        //----------------------------------------------------------------------
        // 功能说明：获取相机名称
        // 输入参数：1.cameratype：相机类型
        //         2.language_parameter：语言
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private String _GetCameraName(VisionSystemClassLibrary.Enum.CameraType cameratype, VisionSystemClassLibrary.Enum.InterfaceLanguage language_parameter)
        {
            string sReturn = "";

            for (int i = 0; i < system.SystemCameraConfiguration.Length; i++)
            {
                if (system.SystemCameraConfiguration[i].Type == cameratype)
                {
                    if (VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese == language_parameter)//中文
                    {
                        sReturn = system.SystemCameraConfiguration[i].CameraCHNName;
                    }
                    else if (VisionSystemClassLibrary.Enum.InterfaceLanguage.English == language_parameter)//英文
                    {
                        sReturn = system.SystemCameraConfiguration[i].CameraENGName;
                    }
                    else//默认中文
                    {
                        sReturn = system.SystemCameraConfiguration[i].CameraCHNName;
                    }

                    break;
                }
            }

            return sReturn;
        }

        //----------------------------------------------------------------------
        // 功能说明：用户调用，刷新设备列表
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _RefreshList()
        {
            int i = 0;//循环控制变量

            //

            iDeviceNumber_Total = 0;//连接的设备数目（0表示无连接的设备）

            for (i = 0; i < system.ConnectionData.Length; i++)//遍历设备数组
            {
                if (system.ConnectionData[i].Connected && system.ConnectionData[i].GetDevInfo)//设备连接
                {
                    iDeviceNumber_Total++;//连接的设备数目（取值范围：0 ~ 256，0表示无连接的设备）
                }
            }

            //customList.ItemDataNumber = iDeviceNumber_Total;

            //应用属性设置

            customList._Apply(iDeviceNumber_Total);//应用列表属性

            if (customList.ItemDataNumber > 0)//存在连接的设备
            {
                //添加列表项数据

                _AddListItemData();

                //设置列表项数据

                _SetPage();

                //更新页面控件

                customButtonMessage1.CurrentTextGroupIndex = 0;//设置显示的文本
                labelMessage1.Text = "";//设置显示的文本
            }
            else//不存在连接的设备
            {
                //应用属性设置

                //customList._Apply();//应用列表属性

                //设置列表项数据

                _SetPage();

                //更新页面控件

                customButtonMessage1.CurrentTextGroupIndex = 2;//设置显示的文本
                labelMessage1.Text = "";//设置显示的文本
            }

            //设置页面控件

            customButtonMessage3.CurrentTextGroupIndex = 2;//设置显示的文本

            if (1 >= customList.TotalPage)//小于等于1页
            {
                customButtonPreviousPage.Visible = false;//隐藏【Previous Page】按钮
                customButtonNextPage.Visible = false;//隐藏【Next Page】按钮
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：用户调用，复位设备完成
        // 输入参数：1.bSuccess：复位设备是否成功。取值范围：true：是；false，否
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _ResetDevice(Boolean bSuccess)
        {
            customButtonMessage3.CurrentTextGroupIndex = 2;//设置显示的文本

            //显示信息对话框

            if (bSuccess)//成功
            {
                GlobalWindows.MessageDisplay_Window.WindowParameter = 29;//窗口特征数值
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][12] + " " + system.ConnectionData[customList.CurrentDataIndex].DeviceName + " " + sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][1];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][12] + " " + system.ConnectionData[customList.CurrentDataIndex].DeviceName + " " + sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][1];
            }
            else//失败
            {
                GlobalWindows.MessageDisplay_Window.WindowParameter = 30;//窗口特征数值
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][12] + " " + system.ConnectionData[customList.CurrentDataIndex].DeviceName + " " + sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][2];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][12] + " " + system.ConnectionData[customList.CurrentDataIndex].DeviceName + " " + sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][2];
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
        // 功能说明：用户调用，配置设备完成
        // 输入参数：1.bSuccess：配置是否成功。取值范围：true：是；false，否
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _ConfigDevice(Boolean bSuccess)
        {
            customButtonMessage3.CurrentTextGroupIndex = 2;//设置显示的文本

            //

            if (bSuccess)//成功
            {
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][8] + "...";
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][8] + "...";
            } 
            else//失败
            {
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][9];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][9];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][11];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][11];

                timerConfigDevice.Stop();//关闭定时器

                iTimerConfigDeviceCount = iTimerConfigDeviceMaxCount;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：用户调用，配置设备完成且设备重新启动完成并连接
        // 输入参数：1.sController：控制器
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _ConfigDevice_RestartOK(String sController)
        {
            if (sController == sControllerConfigDevice)//有效
            {
                bConfigDeviceMessageWindowShow = false;

                iTimerConfigDeviceCount = iTimerConfigDeviceMaxCount;

                timerConfigDevice.Stop();//关闭定时器

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
        }

        //----------------------------------------------------------------------
        // 功能说明：保存参数
        // 输入参数：1.bSuccess：保存是否成功。取值范围：true，是；false，否
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _ParameterSettings(Boolean bSuccess)
        {
            bParameterSettingsMessageWindowShow = false;

            iTimerConfigDeviceCount = iTimerConfigDeviceMaxCount;

            timerConfigDevice.Stop();//关闭定时器

            //

            GlobalWindows.ParameterSettings_Window.ParameterSettingsControl._SaveParameter(bSuccess);
        }

        //----------------------------------------------------------------------
        // 功能说明：用户调用，同步日期时间完成
        // 输入参数：1.bSuccess：同步是否成功。取值范围：true：是；false，否
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _AlignDateTime(Boolean bSuccess)
        {
            //显示信息对话框

            if (bSuccess)//成功
            {
                GlobalWindows.MessageDisplay_Window.WindowParameter = 34;//窗口特征数值
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][4] + " " + system.ConnectionData[customList.CurrentDataIndex].DeviceName;
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][4] + " " + system.ConnectionData[customList.CurrentDataIndex].DeviceName;
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][5];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][5];
            }
            else//失败
            {
                GlobalWindows.MessageDisplay_Window.WindowParameter = 35;//窗口特征数值
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][4] + " " + system.ConnectionData[customList.CurrentDataIndex].DeviceName;
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][4] + " " + system.ConnectionData[customList.CurrentDataIndex].DeviceName;
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][6];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][6];
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

        //

        //----------------------------------------------------------------------
        // 功能说明：设置功能性按钮背景
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetFunctionalButton()
        {
            if (-1 != customList.CurrentListIndex)//选择了设备列表中的某一项
            {
                customButtonResetDevice.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//更新【RESET DEVICE】按钮的背景
                customButtonConfigDevice.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//更新【CONFIG DEVICE】按钮的背景
                //
                Int32 i = 0;

                if (null != system.Camera)
                {
                    for (i = 0; i < system.Camera.Count; i++)
                    {
                        if (system.Camera[i].Type == system.ConnectionData[customList.CurrentDataIndex].Type)//
                        {
                            if (null == system.Camera[i].DeviceParameter.Parameter)
                            {
                                customButtonParameterSettings.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【PARAMETER SETTINGS】按钮的背景
                            }
                            else
                            {
                                customButtonParameterSettings.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//更新【PARAMETER SETTINGS】按钮的背景
                            }

                            break;
                        }
                    }
                } 
                else
                {
                    customButtonParameterSettings.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【PARAMETER SETTINGS】按钮的背景
                }
                //
                customButtonTestIO.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//更新【TEST I/O】按钮的背景
                customButtonConfigImage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//更新【CONFIG IMAGE】按钮的背景
                customButtonAlignDateTime.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//更新【ALIGN DATE/TIME】按钮的背景
            }
            else//未选择设备列表中的任何一项
            {
                customButtonResetDevice.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【RESET DEVICE】按钮的背景
                customButtonConfigDevice.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【CONFIG DEVICE】按钮的背景
                customButtonParameterSettings.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【PARAMETER SETTINGS】按钮的背景
                customButtonTestIO.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【TEST I/O】按钮的背景
                customButtonConfigImage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【CONFIG IMAGE】按钮的背景
                customButtonAlignDateTime.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【ALIGN DATE/TIME】按钮的背景
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：将设备信息添加至列表中
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _AddListItemData()
        {
            Int32 i = 0;//循环控制变量
            Int32 j = 0;//循环控制变量
            Int32 value = 0;//临时变量

            for (i = 0; i < customList.ItemDataNumber; i++)//列表项数据
            {
                for (j = value; j < system.ConnectionData.Length; j++)//遍历设备数组
                {
                    if (system.ConnectionData[j].Connected && system.ConnectionData[j].GetDevInfo)//设备连接
                    {
                        customList.ItemData[i].ItemText[0] = system.ConnectionData[j].SerialNumber;//属性，列表项数据
                        customList.ItemData[i].ItemText[1] = system.ConnectionData[j].MAC;//属性，列表项数据
                        customList.ItemData[i].ItemText[2] = system.ConnectionData[j].IP;//属性，列表项数据
                        customList.ItemData[i].ItemText[3] = system.ConnectionData[j].Port.ToString();//属性，列表项数据
                        customList.ItemData[i].ItemText[4] = system.ConnectionData[j].Firmware;//属性，列表项数据
                        customList.ItemData[i].ItemText[5] = system.ConnectionData[j].DeviceName;//属性，列表项数据
                        customList.ItemData[i].ItemText[6] = system.ConnectionData[j].ControllerName;//属性，列表项数据

                        customList.ItemData[i].ItemFlag = j;

                        //

                        value = j + 1;

                        //

                        break;
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：设置故障信息
        // 输入参数：1.iIndex：相机索引值
        //         2.faultmessage：故障信息
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _SetFaultMessage(Int32 iIndex, VisionSystemClassLibrary.Struct.FaultMessage faultmessage)
        {
            //运行时无效

            if (VisionSystemClassLibrary.Enum.DeviceState.Run == devicestate)//运行
            {
            }
            else
            {
                if ((false == bFaultExist[iIndex]) && _CheckCameraFaultState(faultmessage)) //发生相机故障
                {
                    Int32 i = 0;//循环控制变量
                    Int32 j = 0;//循环控制变量
                    Int32 value = 0;//临时变量

                    for (i = 0; i < customList.ItemDataNumber; i++)//列表项数据
                    {
                        for (j = value; j < system.ConnectionData.Length; j++)//遍历设备数组
                        {
                            if (system.ConnectionData[j].Connected && system.ConnectionData[j].GetDevInfo)//设备连接
                            {
                                if (system.Camera[iIndex].Type == system.ConnectionData[j].Type) //相机类型相同
                                {
                                    bFaultExist[iIndex] = true;
                                }

                                //

                                value = j + 1;

                                //

                                break;
                            }
                        }

                        if (bFaultExist[iIndex]) //故障相机查询到
                        {
                            if (i == customList.CurrentDataIndex) //当前选项相机故障
                            {
                                //更新按钮背景

                                customButtonPreviousPage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【Previous Page】按钮的背景
                                customButtonNextPage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【Next Page】按钮的背景
                                customButtonResetDevice.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【RESET DEVICE】按钮的背景
                                customButtonParameterSettings.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【PARAMETER SETTINGS】按钮的背景
                                customButtonConfigDevice.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【CONFIG DEVICE】按钮的背景
                                customButtonTestIO.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【TEST I/O】按钮的背景
                                customButtonConfigImage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【CONFIG IMAGE】按钮的背景
                                customButtonAlignDateTime.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【ALIGN DATE/TIME】按钮的背景
                            }
                            customList._SetListItemEnable(i, false);

                            //
                            break;
                        }
                    }
                }
                else if ((true == bFaultExist[iIndex]) && (false == _CheckCameraFaultState(faultmessage))) //相机故障消失
                {
                    Int32 i = 0;//循环控制变量
                    Int32 j = 0;//循环控制变量
                    Int32 value = 0;//临时变量

                    for (i = 0; i < customList.ItemDataNumber; i++)//列表项数据
                    {
                        for (j = value; j < system.ConnectionData.Length; j++)//遍历设备数组
                        {
                            if (system.ConnectionData[j].Connected && system.ConnectionData[j].GetDevInfo)//设备连接
                            {
                                if (system.Camera[iIndex].Type == system.ConnectionData[j].Type) //相机类型相同
                                {
                                    bFaultExist[iIndex] = false;
                                }

                                //

                                value = j + 1;

                                //

                                break;
                            }
                        }

                        if (false == bFaultExist[iIndex]) //故障相机查询到
                        {
                            customList._SetListItemEnable(i, true);

                            _SetFunctionalButton();

                            //
                            break;
                        }
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：设置当前页中的控件
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetPage()
        {
            //设置列表项

            customList._SetPage();

            //更新页面按钮背景

            _SetFunctionalButton();//设置功能性按钮背景
        }

        //----------------------------------------------------------------------
        // 功能说明：点击设备列表中的列表项后，更新页面控件
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _ClickListItem_UpdateControl()
        {
            _SetFunctionalButton();//设置功能性按钮背景

            //更新当前选择的项

            customButtonMessage1.CurrentTextGroupIndex = 1;//设置显示的文本
            labelMessage1.Text = system.ConnectionData[customList.CurrentDataIndex].DeviceName;//更新控件文本

            //

            cameratypeSelected = system.ConnectionData[customList.CurrentDataIndex].Type;

            //事件

            if (null != DeviceItem_Click)//有效
            {
                DeviceItem_Click(this, new CustomEventArgs());
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
        private void DeviceControl_Load(object sender, EventArgs e)
        {
            //_SetDefault();
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
            cameratypeSelected = VisionSystemClassLibrary.Enum.CameraType.None;

            //事件

            if (null != Close_Click)//有效
            {
                Close_Click(this, new CustomEventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【REFRESH LIST】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonRefreshList_CustomButton_Click(object sender, EventArgs e)
        {
            //事件

            if (null != RefreshList_Click)//有效
            {
                RefreshList_Click(this, new CustomEventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【RESET DEVICE】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonResetDevice_CustomButton_Click(object sender, EventArgs e)
        {
            //显示信息对话框

            GlobalWindows.MessageDisplay_Window.WindowParameter = 28;//窗口特征数值
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.OkCancel;//包含【确定】和【取消】按钮
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] + " " + system.ConnectionData[customList.CurrentDataIndex].DeviceName + "？";
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] + " " + system.ConnectionData[customList.CurrentDataIndex].DeviceName + "？";

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
        // 功能说明：点击【CONFIG DEVICE】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonConfigDevice_CustomButton_Click(object sender, EventArgs e)
        {
            bClickConfigDeviceButton = true;//是否按下了【CONFIG DEVICE】按钮。取值范围：true，是；false，否

            //显示窗口

            GlobalWindows.DeviceConfiguration_Window.DeviceConfigurationControl.Language = language;//语言
            GlobalWindows.DeviceConfiguration_Window.DeviceConfigurationControl.DeviceDataIndex = customList.CurrentDataIndex;//属性，设备信息数组序号（取值为-1，表示无设备信息）
            GlobalWindows.DeviceConfiguration_Window.DeviceConfigurationControl._Properties(system);//设备

            GlobalWindows.DeviceConfiguration_Window.StartPosition = FormStartPosition.CenterScreen;
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.DeviceConfiguration_Window.TopMost = true;//将窗口置于顶层
            }
            else//其它
            {
                GlobalWindows.DeviceConfiguration_Window.Visible = true;//显示
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【PARAMETER SETTINGS】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonParameterSettings_CustomButton_Click(object sender, EventArgs e)
        {
            Int32 i = 0;//循环控制变量

            for (i = 0; i < system.Camera.Count; i++)//遍历相机
            {
                if (cameratypeSelected == system.Camera[i].Type)//目标相机
                {
                    break;
                }
            }
            if (i < system.Camera.Count)//有效
            {
                bClickParameterSettingsButton = true;

                //显示窗口

                GlobalWindows.ParameterSettings_Window.ParameterSettingsControl.Language = language;//语言
                GlobalWindows.ParameterSettings_Window.ParameterSettingsControl._Properties(system.Camera[i], system.Brand);
                GlobalWindows.ParameterSettings_Window.ParameterSettingsControl._Apply();

                GlobalWindows.ParameterSettings_Window.StartPosition = FormStartPosition.CenterScreen;
                if (GlobalWindows.TopMostWindows)//置顶
                {
                    GlobalWindows.ParameterSettings_Window.TopMost = true;//将窗口置于顶层
                }
                else//其它
                {
                    GlobalWindows.ParameterSettings_Window.Visible = true;//显示
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【TEST I/O】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonTestIO_CustomButton_Click(object sender, EventArgs e)
        {
            bClickTestIOButton = true;//是否按下了【TEST I/O】按钮。取值范围：true，是；false，否

            //事件

            if (null != TestIO_Click)//有效
            {
                TestIO_Click(this, new CustomEventArgs());
            }

            //显示窗口

            GlobalWindows.IOSignalDiagnosis_Window.IOSignalDiagnosisControl.Language = language;//语言
            GlobalWindows.IOSignalDiagnosis_Window.IOSignalDiagnosisControl.Chinese_SelectedDeviceName = _GetCameraName(system.ConnectionData[customList.CurrentDataIndex].Type, VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese);//设备中文名称
            GlobalWindows.IOSignalDiagnosis_Window.IOSignalDiagnosisControl.English_SelectedDeviceName = _GetCameraName(system.ConnectionData[customList.CurrentDataIndex].Type, VisionSystemClassLibrary.Enum.InterfaceLanguage.English);//设备英文名称
            system.IOSignalData.OutputDiagStateLab = 0;
            GlobalWindows.IOSignalDiagnosis_Window.IOSignalDiagnosisControl._Properties(system);

            GlobalWindows.IOSignalDiagnosis_Window.StartPosition = FormStartPosition.CenterScreen;
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.IOSignalDiagnosis_Window.TopMost = true;//将窗口置于顶层
            }
            else//其它
            {
                GlobalWindows.IOSignalDiagnosis_Window.Visible = true;//显示
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【CONFIG IMAGE】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonConfigImage_CustomButton_Click(object sender, EventArgs e)
        {
            //事件

            if (null != ConfigImage_Click)//有效
            {
                ConfigImage_Click(this, new CustomEventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【ALIGN DATE/TIME】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonAlignDateTime_CustomButton_Click(object sender, EventArgs e)
        {
            //显示信息对话框

            GlobalWindows.MessageDisplay_Window.WindowParameter = 33;//窗口特征数值
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.OkCancel;//包含【确定】和【取消】按钮
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][7] + " " + system.ConnectionData[customList.CurrentDataIndex].DeviceName + " " + sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][13] + "？";
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][7] + system.ConnectionData[customList.CurrentDataIndex].DeviceName + "？"; ;

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
            customList._ClickPage(true);//翻页

            //事件

            if (null != PreviousPage_Click)//有效
            {
                PreviousPage_Click(this, new CustomEventArgs());
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
            customList._ClickPage(false);//翻页

            //事件

            if (null != NextPage_Click)//有效
            {
                NextPage_Click(this, new CustomEventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击有效的设备列表项控件时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customListItem_Click(object sender, EventArgs e)
        {
            _ClickListItem_UpdateControl();//点击设备列表项
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：DEVICES SETUP，【RESET DEVICE】确认，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //---------------------------------------------------------------------- 
        private void messageDisplayWindow_WindowClose_DevicesSetup_ResetDevice_Confirm(object sender, EventArgs e)
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

            if (GlobalWindows.MessageDisplay_Window.MessageDisplayControl.OkCancel)//启动复位设备
            {
                customButtonMessage3.CurrentTextGroupIndex = 1;//设置显示的文本

                //事件

                if (null != ResetDevice_Click)//有效
                {
                    ResetDevice_Click(this, new CustomEventArgs());
                }
            }
            else//不进行复位
            {
                //不执行操作
            }
        }
        
        //----------------------------------------------------------------------
        // 功能说明：DEVICES SETUP，【RESET DEVICE】成功，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //---------------------------------------------------------------------- 
        private void messageDisplayWindow_WindowClose_DevicesSetup_ResetDevice_Success(object sender, EventArgs e)
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
        // 功能说明：DEVICES SETUP，【RESET DEVICE】失败，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //---------------------------------------------------------------------- 
        private void messageDisplayWindow_WindowClose_DevicesSetup_ResetDevice_Failure(object sender, EventArgs e)
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
        // 功能说明：DEVICES SETUP，【CONFIG DEVICE】等待，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //---------------------------------------------------------------------- 
        private void messageDisplayWindow_WindowClose_DevicesSetup_ConfigDevice_Wait(object sender, EventArgs e)
        {
            bConfigDeviceMessageWindowShow = false;

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
        
        //

        //----------------------------------------------------------------------
        // 功能说明：PARAMETER SETTINGS，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //---------------------------------------------------------------------- 
        private void parameterSettingsWindow_WindowClose(object sender, EventArgs e)
        {
            bClickParameterSettingsButton = false;

            //

            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.ParameterSettings_Window.TopMost = false;//取消置于顶层
            }
            else//其它
            {
                GlobalWindows.ParameterSettings_Window.Visible = false;//隐藏
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：PARAMETER SETTINGS，【保存参数】，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //---------------------------------------------------------------------- 
        private void parameterSettingsWindow_SaveParameter(object sender, EventArgs e)
        {
            //显示等待窗口

            bParameterSettingsMessageWindowShow = true;

            timerConfigDevice.Start();//启动定时器

            //事件

            if (null != ParameterSettings_Save)//有效
            {
                ParameterSettings_Save(this, new CustomEventArgs());
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：DEVICES SETUP，【ALIGN DATE/TIME】确认，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //---------------------------------------------------------------------- 
        private void messageDisplayWindow_WindowClose_DevicesSetup_AlignDateTime_Confirm(object sender, EventArgs e)
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

            if (GlobalWindows.MessageDisplay_Window.MessageDisplayControl.OkCancel)//启动同步
            {
                //事件

                if (null != AlignDateTime_Click)//有效
                {
                    AlignDateTime_Click(this, new CustomEventArgs());
                }
            }
            else//不进行同步
            {
                //不执行操作
            }
        }
        
        //----------------------------------------------------------------------
        // 功能说明：DEVICES SETUP，【ALIGN DATE/TIME】成功，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //---------------------------------------------------------------------- 
        private void messageDisplayWindow_WindowClose_DevicesSetup_AlignDateTime_Success(object sender, EventArgs e)
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
        // 功能说明：DEVICES SETUP，【ALIGN DATE/TIME】失败，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //---------------------------------------------------------------------- 
        private void messageDisplayWindow_WindowClose_DevicesSetup_AlignDateTime_Failure(object sender, EventArgs e)
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

        //

        //----------------------------------------------------------------------
        // 功能说明：配置设备窗口关闭事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void deviceConfigurationWindow_WindowClose(object sender, EventArgs e)
        {
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.DeviceConfiguration_Window.TopMost = false;//取消置于顶层
            }
            else//其它
            {
                GlobalWindows.DeviceConfiguration_Window.Visible = false;//隐藏
            }

            //

            bClickConfigDeviceButton = false;//是否按下了【CONFIG DEVICE】按钮。取值范围：true，是；false，否

            if (VisionSystemClassLibrary.Enum.DeviceConfigurationResult.None != GlobalWindows.DeviceConfiguration_Window.DeviceConfigurationControl.ControlResult)//关闭配置设备窗口，进行了某项操作
            {
                customButtonMessage3.CurrentTextGroupIndex = 0;//设置显示的文本

                //

                sControllerConfigDevice = GlobalWindows.DeviceConfiguration_Window.DeviceConfigurationControl.SelectedController;//选择的控制器

                //事件

                if (null != ConfigDevice_Click)//有效
                {
                    ConfigDevice_Click(this, new CustomEventArgs());
                }

                //显示等待窗口

                bConfigDeviceMessageWindowShow = true;

                GlobalWindows.MessageDisplay_Window.WindowParameter = 31;//窗口特征数值
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.None;//不包含任何按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][3] + "...";
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][3] + "..." ;
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][10] + "，" + iTimerConfigDeviceCount.ToString();
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][10] + "，" + iTimerConfigDeviceCount.ToString();

                GlobalWindows.MessageDisplay_Window.StartPosition = FormStartPosition.CenterScreen;
                if (GlobalWindows.TopMostWindows)//置顶
                {
                    GlobalWindows.MessageDisplay_Window.TopMost = true;//将窗口置于顶层
                }
                else//其它
                {
                    GlobalWindows.MessageDisplay_Window.Visible = true;//显示
                }

                timerConfigDevice.Start();//启动定时器
            }
            else//关闭配置设备窗口，未进行任何操作
            {
                //不执行操作
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：IO测试窗口关闭事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void iOSignalDiagnosisWindow_WindowClose(object sender, EventArgs e)
        {
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.IOSignalDiagnosis_Window.TopMost = false;//取消置于顶层
            }
            else//其它
            {
                GlobalWindows.IOSignalDiagnosis_Window.Visible = false;//隐藏
            }

            //

            bClickTestIOButton = false;//是否按下了【TEST I/O】按钮。取值范围：true，是；false，否

            //

            customButtonTestIO.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【TEST I/O】

            //事件

            if (null != TestIO_Close_Click)//有效
            {
                TestIO_Close_Click(this, new CustomEventArgs());
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：定时器事件，设备配置完成后,执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void timerConfigDevice_Tick(object sender, EventArgs e)
        {
            if (bConfigDeviceMessageWindowShow)
            {
                iTimerConfigDeviceCount--;

                if (0 >= iTimerConfigDeviceCount)//超时
                {
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][9];
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][9];
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][11];
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][11];

                    timerConfigDevice.Stop();//关闭定时器

                    iTimerConfigDeviceCount = iTimerConfigDeviceMaxCount;
                }
                else//计数
                {
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][10] + "，" + iTimerConfigDeviceCount.ToString();
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = sMessageText_2[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][10] + "，" + iTimerConfigDeviceCount.ToString();
                }
            }

            //

            if (bParameterSettingsMessageWindowShow)
            {
                iTimerConfigDeviceCount--;

                if (0 >= iTimerConfigDeviceCount)//超时
                {
                    GlobalWindows.ParameterSettings_Window.ParameterSettingsControl._SaveParameter(false);

                    timerConfigDevice.Stop();//关闭定时器

                    iTimerConfigDeviceCount = iTimerConfigDeviceMaxCount;
                }
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：检查相机故障状态
        // 输入参数：1.faultmessage：故障信息
        // 输出参数：无
        // 返回值：相机故障信息是否存在。取值范围：true，是；false，否
        //----------------------------------------------------------------------
        private Boolean _CheckCameraFaultState(VisionSystemClassLibrary.Struct.FaultMessage faultmessage)
        {
            Boolean bReturn = false;

            if ( (2 == faultmessage.DataIndex)||(3 == faultmessage.DataIndex)||(41 == faultmessage.DataIndex)||(42 == faultmessage.DataIndex)) //村子啊相机1/2/3/4故障
            {
                bReturn=true;
            }

            return bReturn;
        }
    }
}