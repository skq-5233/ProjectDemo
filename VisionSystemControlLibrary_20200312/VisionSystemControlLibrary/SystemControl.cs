/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：SystemControl.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：VISION SYSTEM CONGIGURATION控件

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

using System.Runtime.InteropServices;

using System.Reflection;

namespace VisionSystemControlLibrary
{
    public partial class SystemControl : UserControl
    {
        //该控件为VISION SYSTEM CONGIGURATION页面

        [DllImport("Kernel32.dll")]
        private extern static Boolean GetLocalTime(ref VisionSystemClassLibrary.Struct.SYSTEMTIME lpSystemTime);  //获取当前系统时间

        [DllImport("Kernel32.dll")]
        private extern static Int32 SetLocalTime(ref VisionSystemClassLibrary.Struct.SYSTEMTIME lpSystemTime);  //设置当前系统时间

        [DllImport("Shell32.dll")]
        private extern static Int32 ShellExecute(IntPtr hwnd, StringBuilder lpszOp, StringBuilder lpszFile, StringBuilder lpszParams, StringBuilder lpszDir, Int32 FsShowCmd);  //打开系统窗口

        //

        private VisionSystemClassLibrary.Class.System parameter = new VisionSystemClassLibrary.Class.System();//属性（只读），设备（主要使用其中的系统参数数据）

        private VisionSystemClassLibrary.Class.System parameter_temp = new VisionSystemClassLibrary.Class.System();//设备（临时变量，保存修改的参数，主要使用其中的系统参数数据）

        private VisionSystemClassLibrary.Enum.InterfaceLanguage language_temp = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;//语言（临时变量，保存修改的参数）

        //

        private VisionSystemClassLibrary.Enum.InterfaceLanguage language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;//属性，语言

        private VisionSystemClassLibrary.Enum.DeviceState devicestate = VisionSystemClassLibrary.Enum.DeviceState.Stop;//属性，设备状态

        //

        private Boolean bDateTimeChanged = false;//日期时间是否被修改。取值范围：true，是；false，否
        private VisionSystemClassLibrary.Struct.SYSTEMTIME SystemTimeSetting = new VisionSystemClassLibrary.Struct.SYSTEMTIME();//设定的当前时间

        //

        private Boolean bCigaretteSortChanged = false;//属性（只读），烟支排列状态是否被修改。取值范围：true，是；false，否

        private Boolean bCheckEnableChanged = false;//属性（只读），相机使能状态是否被修改。取值范围：true，是；false，否

        private Boolean bCheckLayoutChanged = false;//属性（只读），机器选择是否被修改。取值范围：true，是；false，否

        private Boolean bShiftTimeChanged = false;//属性（只读），班次是否被修改。取值范围：true，是；false，否

        private Boolean bMachineChanged = false;//属性（只读），机器类型是否被修改。取值范围：true，是；false，否

        //

        private Boolean bClickCloseButton = false;//是否点击【Close】按钮。取值范围：true，是；false，否

        //

        private Int32 iCommonParameterNumber = 0;//属性，选择的设备数量

        private Boolean bCommonParameterMessageWindowShow = false;//属性（只读），是否显示机器选择、班次修改后的提示信息窗口。取值范围：true，是；false，否

        private const Int32 iTimerCommonParameterMaxCount = 10;//定时器时间
        private Int32 iTimerCommonParameterCount = 10;//定时器时间

        //

        private String sPCTime = "";//属性（只读），当前时间

        //

        private String[][] sMessageText = new String[2][];//提示信息对话框、列表中显示的文本（[语言][包含的文本]）

        //

        [Browsable(true), Description("点击【Close】按钮时产生的事件"), Category("SystemControl 事件")]
        public event EventHandler Close_Click;//点击【Close】按钮时产生的事件

        [Browsable(true), Description("点击【OK】按钮时产生的事件"), Category("SystemControl 事件")]
        public event EventHandler Ok_Click;//点击【OK】按钮时产生的事件

        [Browsable(true), Description("点击【OK】按钮且机器选择、班次参数已经被修改时产生的事件"), Category("SystemControl 事件")]
        public event EventHandler Ok_CommonParameterChanged_Click;//点击【OK】按钮且机器选择、班次参数已经被修改时产生的事件

        [Browsable(true), Description("点击【Cancel】按钮时产生的事件"), Category("SystemControl 事件")]
        public event EventHandler Cancel_Click;//点击【Cancel】按钮时产生的事件
        
        [Browsable(true), Description("点击Parameter List【Previous Page】按钮时产生的事件"), Category("SystemControl 事件")]
        public event EventHandler PreviousPage_ParameterList_Click;//点击Parameter List【Previous Page】按钮时产生的事件

        [Browsable(true), Description("点击Parameter List【Next Page】按钮时产生的事件"), Category("SystemControl 事件")]
        public event EventHandler NextPage_ParameterList_Click;//点击Parameter List【Next Page】按钮时产生的事件

        [Browsable(true), Description("点击Value List【Previous Page】按钮时产生的事件"), Category("SystemControl 事件")]
        public event EventHandler PreviousPage_ValueList_Click;//点击Value List【Previous Page】按钮时产生的事件

        [Browsable(true), Description("点击Value List【Next Page】按钮时产生的事件"), Category("SystemControl 事件")]
        public event EventHandler NextPage_ValueList_Click;//点击Value List【Next Page】按钮时产生的事件

        [Browsable(true), Description("点击Parameter List控件项时产生的事件"), Category("SystemControl 事件")]
        public event EventHandler ParameterListItem_Click;//点击Parameter List控件项时产生的事件

        [Browsable(true), Description("点击Value List控件项时产生的事件"), Category("SystemControl 事件")]
        public event EventHandler ValueListItem_Click;//点击Value List控件项时产生的事件

        [Browsable(true), Description("改变语言时产生的事件"), Category("SystemControl 事件")]
        public event EventHandler LanguageChanged;//改变语言时产生的事件

        [Browsable(true), Description("定时器产生的事件"), Category("SystemControl 事件")]
        public event EventHandler PCTimerTick;//定时器产生的事件

        //

        //----------------------------------------------------------------------
        // 功能说明：系统默认调用，构造函数，初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public SystemControl()
        {
            InitializeComponent();

            //由于该控件仅有一个实例，因此可以在此处进行事件的订阅

            if (null != GlobalWindows.DateTimePanel_Window)
            {
                GlobalWindows.DateTimePanel_Window.WindowClose_System_DateTime += new System.EventHandler(dateTimePanelWindow_WindowClose_System_DateTime);//订阅事件
            }

            if (null != GlobalWindows.ShiftConfiguration_Window)
            {
                GlobalWindows.ShiftConfiguration_Window.WindowClose += new System.EventHandler(shiftConfigurationWindow_WindowClose);//订阅事件
            }

            if (null != GlobalWindows.StandardKeyboard_Window)
            {
                GlobalWindows.StandardKeyboard_Window.WindowClose_System_Password += new System.EventHandler(standardKeyboardWindow_WindowClose_System_Password);//订阅事件
            }

            if (null != GlobalWindows.MessageDisplay_Window)
            {
                GlobalWindows.MessageDisplay_Window.WindowClose_System_Ok_Confirm += new System.EventHandler(messageDisplayWindow_WindowClose_System_Ok_Confirm);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_System_Ok_Wait += new System.EventHandler(messageDisplayWindow_WindowClose_System_Ok_Wait);//订阅事件
            }

            if (null != GlobalWindows.CigaretteSort_Window)
            {
                GlobalWindows.CigaretteSort_Window.WindowClose += new EventHandler(CigaretteSort_Window_WindowClose);//订阅事件
            }

            //

            FieldInfo[] fieldinfo = VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese.GetType().GetFields();//获取公共字段
            if (null != fieldinfo && 1 < fieldinfo.Length)//有效
            {
                Int32 i = 0;//循环控制变量

                sMessageText = new String[fieldinfo.Length - 1][];
                for (i = 0; i < fieldinfo.Length - 1; i++)
                {
                    sMessageText[i] = new String[22];
                }

                //

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] = "机器类型";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] = "MACHINE TYPE";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][1] = "相机选择";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][1] = "CHECK LAYOUT";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][2] = "时间设置";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][2] = "PC TIME";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][3] = "人机界面程序配置";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][3] = "HMI SETTINGS";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][4] = "机器速度（ppm）";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][4] = "Machine Speed（ppm）";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][5] = "使能IPC控制（品牌更换，机器状态）";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][5] = "Enable IPC Messaging（B.Change，M.State）";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][6] = "使能密码保护";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][6] = "Enable Password Protection";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][7] = "语言";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][7] = "Language";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][8] = "中文";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][8] = "Chinese";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][9] = "英文";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][9] = "English";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][10] = "开启";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][10] = "On";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][11] = "关闭";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][11] = "Off";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][12] = "点击【确定】按钮重新启动程序";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][12] = "Press OK button to Restart Application";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][13] = "确定保存参数";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][13] = "Save Parameters";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][14] = "正在保存参数";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][14] = "Saving Parameters";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][15] = "保存参数成功";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][15] = "Save Parameters Completed";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][16] = "保存参数失败";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][16] = "Save Parameters Failed";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][17] = "请重试";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][17] = "Please try again";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][18] = "请等待";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][18] = "Please wait";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][19] = "班次配置";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][19] = "SHIFT SETTINGS";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][20] = "烟支排列";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][20] = "CIGARETTE RANK";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][21] = "检测使能";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][21] = "CHECK ENABLE";
            }
        }

        //属性

        //-----------------------------------------------------------------------
        // 功能说明：Language属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("语言"), Category("SystemControl 通用")]
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

                    language_temp = value;

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
        [Browsable(true), Description("设备状态"), Category("SystemControl 通用")]
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
        // 功能说明：SystemParameter属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("系统参数"), Category("SystemControl 通用")]
        public VisionSystemClassLibrary.Class.System SystemParameter//属性
        {
            get//读取
            {
                return parameter;
            }
        }

        // 功能说明：CigaretteSortChanged属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("烟支排列是否被修改。取值范围：true，是；false，否"), Category("SystemControl 通用")]
        public Boolean CigaretteSortChanged//属性
        {
            get//读取
            {
                return bCigaretteSortChanged;
            }
        }

        // 功能说明：CheckEnableChanged属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("相机使能状态是否被修改。取值范围：true，是；false，否"), Category("SystemControl 通用")]
        public Boolean CheckEnableChanged//属性
        {
            get//读取
            {
                return bCheckEnableChanged;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：CheckLayoutChanged属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("机器选择是否被修改。取值范围：true，是；false，否"), Category("SystemControl 通用")]
        public Boolean CheckLayoutChanged//属性
        {
            get//读取
            {
                return bCheckLayoutChanged;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ShiftTimeChanged属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("班次是否被修改。取值范围：true，是；false，否"), Category("SystemControl 通用")]
        public Boolean ShiftTimeChanged//属性
        {
            get//读取
            {
                return bShiftTimeChanged;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：MachineChanged属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("机器类型是否被修改。取值范围：true，是；false，否"), Category("SystemControl 通用")]
        public Boolean MachineChanged//属性
        {
            get//读取
            {
                return bMachineChanged;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ChangedLanguage属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("当前语言"), Category("SystemControl 通用")]
        public VisionSystemClassLibrary.Enum.InterfaceLanguage ChangedLanguage//属性
        {
            get//读取
            {
                return language_temp;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：PCTime属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("当前时间"), Category("SystemControl 通用")]
        public String PCTime//属性
        {
            get//读取
            {
                return sPCTime;
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：CommonParameterNumber属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("选择的设备数量"), Category("SystemControl 通用")]
        public Int32 CommonParameterNumber//属性
        {
            get//读取
            {
                return iCommonParameterNumber;
            }
            set//设置
            {
                iCommonParameterNumber = value;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：CommonParameterMessageWindowShow属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("是否显示机器选择后的提示信息窗口。取值范围：true，是；false，否"), Category("SystemControl 通用")]
        public Boolean CommonParameterMessageWindowShow//属性
        {
            get//读取
            {
                return bCommonParameterMessageWindowShow;
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
            parameter = system_parameter;

            parameter._CopySystemParameterTo(parameter_temp);
            
            if (parameter.Shift.ShiftState)//使用班次
            {
                parameter.Shift._CopyTo(ref parameter_temp.Shift.DataOfShift.TimeData);
            }

            //

            _SetLanguage();//设置语言

            _SetParameter();//应用属性设置

            _SetDeviceState();
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：更改完成机器选择、班次参数
        // 输入参数：1.bSuccess：操作是否成功。取值范围：true，是；false，否
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _CommonParameter(Boolean bSuccess)
        {
            if (bSuccess)//成功
            {
                iCommonParameterNumber--;
            }

            if (0 >= iCommonParameterNumber)//完成
            {
                bCommonParameterMessageWindowShow = false;

                iTimerCommonParameterCount = iTimerCommonParameterMaxCount;

                timerCommonParameter.Stop();//关闭定时器

                //显示信息对话框

                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                if (bCheckLayoutChanged || bCigaretteSortChanged)//修改
                {
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][15];
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][15];
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][12];
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][12];
                }
                else//其它
                {
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = " ";
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = " ";
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][15];
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][15];
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = " ";
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = " ";
                }

                GlobalWindows.MessageDisplay_Window.Update();
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
            customButtonClose.Language = language_temp;//【Close】
            customButtonOk.Language = language_temp;//【OK】
            customButtonCancel.Language = language_temp;//【Cancel】
            customButtonPreviousPage_ParameterList.Language = language_temp;//Parameter List，【Previous Page】
            customButtonNextPage_ParameterList.Language = language_temp;//Parameter List，【Next Page】
            customButtonPreviousPage_ValueList.Language = language_temp;//Value List，【Previous Page】
            customButtonNextPage_ValueList.Language = language_temp;//Value List，【Next Page】

            //

            customButtonCaption.Language = language_temp;
            customButtonParameterList.Language = language_temp;//
            customButtonValueList.Language = language_temp;//

            //

            customListParameterList.Language = language_temp;//列表语言
            customListValueList.Language = language_temp;//列表语言
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

            if (VisionSystemClassLibrary.Enum.DeviceState.Run == devicestate && 0 < parameter.Work.ConnectedCameraNumber)//运行，存在相机连接
            {
                //更新按钮状态

                customButtonOk.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【OK】按钮的背景
                customButtonCancel.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【CANCEL】按钮的背景

                //更新列表状态

                //if (customListParameterList.CurrentPage == customListParameterList._GetPage(customListParameterList.CurrentListIndex))//当前选择的项在当前页
                //{
                //    customListParameterList._SelectListItem(customListParameterList.Index_Page, false);
                //}
                customListParameterList.ListEnabled = false;

                //if (customListValueList.CurrentPage == customListValueList._GetPage(customListValueList.CurrentListIndex))//当前选择的项在当前页
                //{
                //    customListValueList._SelectListItem(customListValueList.Index_Page, false);
                //}
                customListValueList.ListEnabled = false;
            }
            else//其它
            {
                //更新按钮状态

                customButtonOk.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//更新【OK】按钮的背景
                customButtonCancel.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//更新【CANCEL】按钮的背景

                //更新列表状态

                //if (customListParameterList.CurrentPage == customListParameterList._GetPage(customListParameterList.CurrentListIndex))//当前选择的项在当前页
                //{
                //    customListParameterList._SelectListItem(customListParameterList.Index_Page, true);
                //}
                customListParameterList.ListEnabled = true;

                //if (customListValueList.CurrentPage == customListValueList._GetPage(customListValueList.CurrentListIndex))//当前选择的项在当前页
                //{
                //    customListValueList._SelectListItem(customListValueList.Index_Page, true);
                //}
                customListValueList.ListEnabled = true;
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：应用设置的属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetParameter()
        {
            bClickCloseButton = false;

            bCigaretteSortChanged = false;

            bCheckEnableChanged = false;

            bCheckLayoutChanged = false;

            bShiftTimeChanged = false;

            bMachineChanged = false;

            //Parameter List

            if (parameter.Shift.ShiftState)//班次有效
            {
                _InitList(customListParameterList, 7, true);//初始化列表
            }
            else
            {
                _InitList(customListParameterList, 6, true);//初始化列表
            }

            //Value List

            _InitList(customListValueList, 0, false);//初始化列表

            //

            _SetControl();//更新控件
        }

        //----------------------------------------------------------------------
        // 功能说明：初始化Parameter / Value List列表
        // 输入参数：1.customList：列表
        //         2.iItemNumber：列表项数目
        //         3.bListType：列表类型。取值范围：true，Parameter List；false，Value List
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _InitList(CustomList customList, Int32 iItemNumber, Boolean bListType)
        {
            customList._ApplyListHeader();//应用列表头属性
            customList._ApplyListItem();//应用列表项属性

            //

            _AddItemData(customList, iItemNumber, bListType);
        }

        //----------------------------------------------------------------------
        // 功能说明：添加Parameter / Value List列表数据
        // 输入参数：1.customList：列表
        //         2.iItemNumber：列表项数目
        //         3.bListType：列表类型。取值范围：true，Parameter List；false，Value List
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _AddItemData(CustomList customList, Int32 iItemNumber, Boolean bListType)
        {
            //列表项数目

            //customList.ItemDataNumber = iItemNumber;

            //

            customList._Apply(iItemNumber);//应用列表属性

            //添加列表项数据

            _AddItemData(bListType);

            //设置列表项数据

            _SetPage(customList, bListType);
        }

        //----------------------------------------------------------------------
        // 功能说明：添加Parameter / Value List列表数据
        // 输入参数：1.bListType：列表类型。取值范围：true，Parameter List；false，Value List
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _AddItemData(Boolean bListType)
        {
            Int32 i = 0;//循环控制变量
            Int32 j = 0;//循环控制变量
            Int32 k = 0;//循环控制变量

            VisionSystemClassLibrary.Struct.SYSTEMTIME systemtime = new VisionSystemClassLibrary.Struct.SYSTEMTIME();

            if (bListType)//Parameter List
            {
                customListParameterList.ItemData[0].ItemText[0] = sMessageText[(Int32)language_temp - 1][0];//MACHINE TYPE，名称
                customListParameterList.ItemData[0].ItemText[1] = parameter_temp.MachineType[parameter_temp.SelectedMachineType];//MACHINE TYPE，数值
                customListParameterList.ItemData[0].ItemFlag = 0;

                customListParameterList.ItemData[1].ItemText[0] = sMessageText[(Int32)language_temp - 1][1];//CHECK LAYOUT，名称
                customListParameterList.ItemData[1].ItemText[1] = "";//CHECK LAYOUT，数值
                for (i = 0; i < parameter_temp.SystemCameraConfiguration.Length; i++)
                {
                    if (parameter_temp.SystemCameraConfiguration[i].Selected)//选择
                    {
                        switch (language_temp)
                        {
                            case VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese://中文
                                //
                                customListParameterList.ItemData[1].ItemText[1] += parameter_temp.SystemCameraConfiguration[i].CameraCHNName + "，";//CHECK LAYOUT，数值
                                //
                                break;
                            case VisionSystemClassLibrary.Enum.InterfaceLanguage.English://英文
                                //
                                customListParameterList.ItemData[1].ItemText[1] += parameter_temp.SystemCameraConfiguration[i].CameraENGName + "，";//CHECK LAYOUT，数值
                                //
                                break;
                            default://其它，默认中文
                                //
                                customListParameterList.ItemData[1].ItemText[1] += parameter_temp.SystemCameraConfiguration[i].CameraCHNName + "，";//CHECK LAYOUT，数值
                                //
                                break;
                        }
                    }
                }
                if ("" != customListParameterList.ItemData[1].ItemText[1])
                {
                    customListParameterList.ItemData[1].ItemText[1] = customListParameterList.ItemData[1].ItemText[1].Substring(0, customListParameterList.ItemData[1].ItemText[1].Length - 1);//CHECK LAYOUT，数值
                }
                customListParameterList.ItemData[1].ItemFlag = 1;

                customListParameterList.ItemData[2].ItemText[0] = sMessageText[(Int32)language_temp - 1][21];//CHECK ENABLE，名称
                customListParameterList.ItemData[2].ItemText[1] = customListParameterList.ItemData[1].ItemText[1];//CHECK ENABLE，数值
                customListParameterList.ItemData[2].ItemFlag = 2;

                customListParameterList.ItemData[3].ItemText[0] = sMessageText[(Int32)language_temp - 1][2];//PC TIME，名称
                if (bDateTimeChanged)//修改
                {
                    systemtime = SystemTimeSetting;
                }
                else//未修改
                {
                    GetLocalTime(ref systemtime);//获取当前日期时间
                }
                customListParameterList.ItemData[3].ItemText[1] = _GetDateTime(systemtime);//PC TIME，数值
                customListParameterList.ItemData[3].ItemFlag = 3;

                customListParameterList.ItemData[4].ItemText[0] = sMessageText[(Int32)language_temp - 1][3];//HMI SETTINGS，名称
                customListParameterList.ItemData[4].ItemText[1] = "...";//HMI SETTINGS，数值
                customListParameterList.ItemData[4].ItemFlag = 4;

                customListParameterList.ItemData[5].ItemText[0] = sMessageText[(Int32)language_temp - 1][20];//HMI SETTINGS，名称
                customListParameterList.ItemData[5].ItemText[1] = "...";//烟支排列，数值
                customListParameterList.ItemData[5].ItemFlag = 5;

                if (parameter.Shift.ShiftState)//班次有效
                {
                    customListParameterList.ItemData[6].ItemText[0] = sMessageText[(Int32)language_temp - 1][19];//SHIFT SETTINGS，名称
                    customListParameterList.ItemData[6].ItemText[1] = "...";//SHIFT SETTINGS，数值
                    customListParameterList.ItemData[6].ItemFlag = 6;
                }
            }
            else//Value List
            {
                switch (customListParameterList.CurrentDataIndex)//Parameter List列表当前选择的项
                {
                    case 0://MACHINE TYPE
                        //
                        for (i = 0; i < parameter_temp.MachineType.Length; i++)//
                        {
                            customListValueList.ItemData[i].ItemText[0] = parameter_temp.MachineType[i];//名称
                            customListValueList.ItemData[i].ItemDataDisplay[0] = true;//文本
                            customListValueList.ItemData[i].ItemDataDisplay[1] = true;//图标（隐藏）
                            customListValueList.ItemData[i].ItemIconIndex[0] = -1;//图标
                            customListValueList.ItemData[i].ItemIconIndex[1] = 0;//图标
                            customListValueList.ItemData[i].ItemFlag = i;
                        }
                        customListValueList.ItemData[parameter_temp.SelectedMachineType].ItemDataDisplay[1] = false;//图标（显示）
                        customListValueList.SelectedItemNumber = 1;
                        //
                        break;
                    case 1://CHECK LAYOUT
                        //
                        for (i = 0; i < parameter_temp.SystemCameraConfiguration.Length; i++)//
                        {
                            switch (language_temp)
                            {
                                case VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese://中文
                                    //
                                    customListValueList.ItemData[i].ItemText[0] = parameter_temp.SystemCameraConfiguration[i].CameraCHNName;//名称
                                    //
                                    break;
                                case VisionSystemClassLibrary.Enum.InterfaceLanguage.English://英文
                                    //
                                    customListValueList.ItemData[i].ItemText[0] = parameter_temp.SystemCameraConfiguration[i].CameraENGName;//名称
                                    //
                                    break;
                                default://其它，默认中文
                                    //
                                    customListValueList.ItemData[i].ItemText[0] = parameter_temp.SystemCameraConfiguration[i].CameraCHNName;//名称
                                    //
                                    break;
                            }
                            customListValueList.ItemData[i].ItemDataDisplay[0] = true;//文本
                            if (parameter_temp.SystemCameraConfiguration[i].Selected)//选择
                            {
                                customListValueList.ItemData[i].ItemDataDisplay[1] = false;//图标（显示）

                                j++;
                            }
                            else//未选择
                            {
                                customListValueList.ItemData[i].ItemDataDisplay[1] = true;//图标（隐藏）
                            }
                            customListValueList.ItemData[i].ItemIconIndex[0] = -1;//图标
                            customListValueList.ItemData[i].ItemIconIndex[1] = 0;//图标
                            customListValueList.ItemData[i].ItemFlag = i;
                        }
                        customListValueList.SelectedItemNumber = j;
                        //
                        break;
                    case 2://CHECK ENABLE
                        //
                        for (i = 0; i < parameter_temp.SystemCameraConfiguration.Length; i++)//
                        {
                            if (parameter_temp.SystemCameraConfiguration[i].Selected)//选择
                            {
                                switch (language_temp)
                                {
                                    case VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese://中文
                                        //
                                        customListValueList.ItemData[k].ItemText[0] = parameter_temp.SystemCameraConfiguration[i].CameraCHNName;//名称
                                        //
                                        break;
                                    case VisionSystemClassLibrary.Enum.InterfaceLanguage.English://英文
                                        //
                                        customListValueList.ItemData[k].ItemText[0] = parameter_temp.SystemCameraConfiguration[i].CameraENGName;//名称
                                        //
                                        break;
                                    default://其它，默认中文
                                        //
                                        customListValueList.ItemData[k].ItemText[0] = parameter_temp.SystemCameraConfiguration[i].CameraCHNName;//名称
                                        //
                                        break;
                                }
                                customListValueList.ItemData[k].ItemDataDisplay[0] = true;//文本

                                if (parameter_temp.SystemCameraConfiguration[i].CheckEnable)//选择
                                {
                                    customListValueList.ItemData[k].ItemDataDisplay[1] = false;//图标（显示）

                                    j++;
                                }
                                else//未选择
                                {
                                    customListValueList.ItemData[k].ItemDataDisplay[1] = true;//图标（隐藏）
                                }
                                customListValueList.ItemData[k].ItemIconIndex[0] = -1;//图标
                                customListValueList.ItemData[k].ItemIconIndex[1] = 0;//图标
                                customListValueList.ItemData[k].ItemFlag = k;

                                k++;
                            }
                        }
                        customListValueList.SelectedItemNumber = j;
                        //
                        break;
                    case 3://PC TIME
                        //
                        break;
                    case 4://HMI SETTINGS
                        //
                        customListValueList.ItemData[0].ItemText[0] = sMessageText[(Int32)language_temp - 1][7];//Language，名称
                        switch (language_temp)
                        {
                            case VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese://中文
                                //
                                customListValueList.ItemData[0].ItemText[1] = sMessageText[(Int32)language_temp - 1][8];//Language，数值
                                //
                                break;
                            case VisionSystemClassLibrary.Enum.InterfaceLanguage.English://英文
                                //
                                customListValueList.ItemData[0].ItemText[1] = sMessageText[(Int32)language_temp - 1][9];//Language，数值
                                //
                                break;
                            default://其它，默认中文
                                //
                                customListValueList.ItemData[0].ItemText[1] = sMessageText[(Int32)language_temp - 1][8];//Language，数值
                                //
                                break;
                        }
                        customListValueList.ItemData[0].ItemFlag = 0;

                        customListValueList.ItemData[1].ItemText[0] = sMessageText[(Int32)language_temp - 1][6];//Enable Password Protection，名称
                        if ("" != parameter_temp.UserPassword)//开启
                        {
                            customListValueList.ItemData[1].ItemText[1] = sMessageText[(Int32)language_temp - 1][10];//Enable Password Protection，数值
                        }
                        else//关闭
                        {
                            customListValueList.ItemData[1].ItemText[1] = sMessageText[(Int32)language_temp - 1][11];//Enable Password Protection，数值
                        }
                        customListValueList.ItemData[1].ItemFlag = 1;
                        //
                        break;
                    case 5://Cigarette
                        //
                        break;
                    case 6://SHIFT SETTINGS
                        //
                        break;
                    default:
                        break;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：获取日期时间的字符串形式
        // 输入参数：1.systemtime：日期时间
        // 输出参数：无
        // 返回值：获取的日期时间
        //----------------------------------------------------------------------
        private String _GetDateTime(VisionSystemClassLibrary.Struct.SYSTEMTIME systemtime)
        {
            String sReturn = "";//函数返回值

            switch (language_temp)
            {
                case VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese://中文

                    sReturn = systemtime.Year.ToString("D4") + "." + systemtime.Month.ToString("D2") + "." + systemtime.Day.ToString("D2") + "  " + systemtime.Hour.ToString("D2") + ":" + systemtime.Minute.ToString("D2") + ":" + systemtime.Second.ToString("D2");

                    break;
                case VisionSystemClassLibrary.Enum.InterfaceLanguage.English://英文

                    sReturn = systemtime.Month.ToString("D2") + "/" + systemtime.Day.ToString("D2") + "/" + systemtime.Year.ToString("D4") + "  " + systemtime.Hour.ToString("D2") + ":" + systemtime.Minute.ToString("D2") + ":" + systemtime.Second.ToString("D2");

                    break;
                default://其它，默认中文

                    sReturn = systemtime.Year.ToString("D4") + "." + systemtime.Month.ToString("D2") + "." + systemtime.Day.ToString("D2") + "  " + systemtime.Hour.ToString("D2") + ":" + systemtime.Minute.ToString("D2") + ":" + systemtime.Second.ToString("D2");

                    break;
            }

            return sReturn;
        }

        //----------------------------------------------------------------------
        // 功能说明：打开日期时间设置面板
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _OpenOSDateTimeSettingsWindow()
        {
            ShellExecute(this.Handle, new StringBuilder("open"), new StringBuilder("rundll32.exe"), new StringBuilder("shell32.dll, Control_RunDLL timedate.cpl,,0"), new StringBuilder(""), 1);
        }

        //----------------------------------------------------------------------
        // 功能说明：设置页面控件的背景图像
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetControl()
        {
            if (parameter_temp._CheckSystemParameter(parameter, language_temp))//修改
            {
                customButtonOk.Visible = true;//【OK】
                customButtonCancel.Visible = true;//【Cancel】
            }
            else//未修改
            {
                if (bDateTimeChanged)//日期时间被修改
                {
                    customButtonOk.Visible = true;//【OK】
                    customButtonCancel.Visible = true;//【Cancel】
                }
                else//日期时间未被修改
                {
                    if (bShiftTimeChanged)//班次修改
                    {
                        customButtonOk.Visible = true;//【OK】
                        customButtonCancel.Visible = true;//【Cancel】
                    }
                    else//班次未修改
                    {
                        customButtonOk.Visible = false;//【OK】
                        customButtonCancel.Visible = false;//【Cancel】
                    }
                }
            }
            customButtonOk.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【OK】
            customButtonCancel.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【Cancel】            }

            //

            customButtonClose.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【Close】

            //

            customButtonPreviousPage_ParameterList.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//Parameter List【Previous Page】
            customButtonNextPage_ParameterList.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//Parameter List【Next Page】

            //

            customButtonPreviousPage_ValueList.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//Value List【Previous Page】
            customButtonNextPage_ValueList.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//Value List【Next Page】
        }

        //----------------------------------------------------------------------
        // 功能说明：设置列表控件
        // 输入参数：1.customList：列表
        //         2.bListType：列表类型。取值范围：true，Parameter List；false，Value List
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetPage(CustomList customList, Boolean bListType)
        {
            customList._SetPage();//设置列表项

            _SetControl();//更新控件

            //

            if (bListType)//Parameter List
            {
                if (1 < customList.TotalPage)//多于一页
                {
                    customButtonPreviousPage_ParameterList.Visible = true;//【Previous Page】
                    customButtonNextPage_ParameterList.Visible = true;//【Next Page】
                }
                else//小于等于一页
                {
                    customButtonPreviousPage_ParameterList.Visible = false;//【Previous Page】
                    customButtonNextPage_ParameterList.Visible = false;//【Next Page】
                }
            }
            else//Value List
            {
                if (1 < customList.TotalPage)//多于一页
                {
                    customButtonPreviousPage_ValueList.Visible = true;//【Previous Page】
                    customButtonNextPage_ValueList.Visible = true;//【Next Page】
                }
                else//小于等于一页
                {
                    customButtonPreviousPage_ValueList.Visible = false;//【Previous Page】
                    customButtonNextPage_ValueList.Visible = false;//【Next Page】
                }
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：恢复原始值
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetOriginalValue()
        {
            Int32 i = 0;//循环控制变量
            Int32 k = 0;//循环控制变量

            //

            Boolean bLanguageChanged = (language_temp != language) ? true : false;//语言是否修改。取值范围：true，是；false，否

            //

            parameter._CopySystemParameterTo(parameter_temp);//恢复

            language_temp = language;//恢复

            if (parameter.Shift.ShiftState)//使用班次
            {
                bShiftTimeChanged = false;

                parameter.Shift._CopyTo(ref parameter_temp.Shift.DataOfShift.TimeData);
            }

            //

            customButtonOk.Visible = false;//【OK】
            customButtonCancel.Visible = false;//【Cancel】

            //

            VisionSystemClassLibrary.Struct.SYSTEMTIME systemtime = new VisionSystemClassLibrary.Struct.SYSTEMTIME();

            //Parameter List

            customListParameterList.ItemData[0].ItemText[1] = parameter_temp.MachineType[parameter_temp.SelectedMachineType];//MACHINE TYPE，数值

            customListParameterList.ItemData[1].ItemText[1] = "";//CHECK LAYOUT，数值
            for (i = 0; i < parameter_temp.SystemCameraConfiguration.Length; i++)
            {
                if (parameter_temp.SystemCameraConfiguration[i].Selected)//选择
                {
                    switch (language_temp)
                    {
                        case VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese://中文
                            //
                            customListParameterList.ItemData[1].ItemText[1] += parameter_temp.SystemCameraConfiguration[i].CameraCHNName + "，";//CHECK LAYOUT，数值
                            //
                            break;
                        case VisionSystemClassLibrary.Enum.InterfaceLanguage.English://英文
                            //
                            customListParameterList.ItemData[1].ItemText[1] += parameter_temp.SystemCameraConfiguration[i].CameraENGName + "，";//CHECK LAYOUT，数值
                            //
                            break;
                        default://其它，默认中文
                            //
                            customListParameterList.ItemData[1].ItemText[1] += parameter_temp.SystemCameraConfiguration[i].CameraCHNName + "，";//CHECK LAYOUT，数值
                            //
                            break;
                    }
                }
            }
            if ("" != customListParameterList.ItemData[1].ItemText[1])
            {
                customListParameterList.ItemData[1].ItemText[1] = customListParameterList.ItemData[1].ItemText[1].Substring(0, customListParameterList.ItemData[1].ItemText[1].Length - 1);//CHECK LAYOUT，数值
            }
            
            customListParameterList.ItemData[2].ItemText[1] = customListParameterList.ItemData[1].ItemText[1];//CHECK ENABLE，数值

            GetLocalTime(ref systemtime);//获取当前日期时间
            customListParameterList.ItemData[3].ItemText[1] = _GetDateTime(systemtime);//PC TIME，数值
            bDateTimeChanged = false;//未修改

            customListParameterList.ItemData[4].ItemText[1] = "...";//HMI SETTINGS，数值

            customListParameterList.ItemData[5].ItemText[1] = "...";//TOUCH SCREEN SETTINGS，数值

            if (parameter.Shift.ShiftState)//班次有效
            {
                customListParameterList.ItemData[6].ItemText[1] = "...";//SHIFT SETTINGS，数值
            }

            //Value List

            switch (customListParameterList.CurrentDataIndex)//Parameter List列表当前选择的项
            {
                case 0://MACHINE TYPE
                    //
                    for (i = 0; i < parameter_temp.MachineType.Length; i++)//
                    {
                        customListValueList.ItemData[i].ItemDataDisplay[1] = true;//图标（隐藏）
                    }
                    customListValueList.ItemData[parameter_temp.SelectedMachineType].ItemDataDisplay[1] = false;//图标（显示）
                    //
                    break;
                case 1://CHECK LAYOUT
                    //
                    for (i = 0; i < parameter_temp.SystemCameraConfiguration.Length; i++)//
                    {
                        if (parameter_temp.SystemCameraConfiguration[i].Selected)//选择
                        {
                            customListValueList.ItemData[i].ItemDataDisplay[1] = false;//图标（显示）
                        }
                        else//未选择
                        {
                            customListValueList.ItemData[i].ItemDataDisplay[1] = true;//图标（隐藏）
                        }
                    }
                    //
                    break;
                case 2://CHECK ENABLE
                    //
                    k = 0;

                    for (i = 0; i < parameter_temp.SystemCameraConfiguration.Length; i++)//
                    {
                        if (parameter_temp.SystemCameraConfiguration[i].Selected)//选择
                        {
                            if (parameter_temp.SystemCameraConfiguration[i].CheckEnable)//选择
                            {
                                customListValueList.ItemData[k].ItemDataDisplay[1] = false;//图标（显示）
                            }
                            else//未选择
                            {
                                customListValueList.ItemData[k].ItemDataDisplay[1] = true;//图标（隐藏）
                            }
                            k++;
                        }
                    }
                    //
                    break;
                case 3://PC TIME
                    //
                    break;
                case 4://HMI SETTINGS
                    //
                    switch (language_temp)
                    {
                        case VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese://中文
                            //
                            customListValueList.ItemData[0].ItemText[1] = sMessageText[(Int32)language_temp - 1][8];//Language，数值
                            //
                            break;
                        case VisionSystemClassLibrary.Enum.InterfaceLanguage.English://英文
                            //
                            customListValueList.ItemData[0].ItemText[1] = sMessageText[(Int32)language_temp - 1][9];//Language，数值
                            //
                            break;
                        default://其它，默认中文
                            //
                            customListValueList.ItemData[0].ItemText[1] = sMessageText[(Int32)language_temp - 1][8];//Language，数值
                            //
                            break;
                    }

                    if ("" != parameter_temp.UserPassword)//开启
                    {
                        customListValueList.ItemData[1].ItemText[1] = sMessageText[(Int32)language_temp - 1][10];//Enable Password Protection，数值
                    }
                    else//关闭
                    {
                        customListValueList.ItemData[1].ItemText[1] = sMessageText[(Int32)language_temp - 1][11];//Enable Password Protection，数值
                    }
                    //
                    break;
                case 5://Cigarette Sort
                    //
                    break;
                case 6://SHIFT SETTINGS
                    //
                    break;
                default:
                    break;
            }

            //

            if (bLanguageChanged)//语言修改
            {
                _SetLanguage();

                //Parameter List

                customListParameterList.ItemData[0].ItemText[0] = sMessageText[(Int32)language_temp - 1][0];//MACHINE TYPE，名称
                customListParameterList.ItemData[1].ItemText[0] = sMessageText[(Int32)language_temp - 1][1];//CHECK LAYOUT，名称
                customListParameterList.ItemData[2].ItemText[0] = sMessageText[(Int32)language_temp - 1][21];//CHECK ENABLE，名称
                customListParameterList.ItemData[3].ItemText[0] = sMessageText[(Int32)language_temp - 1][2];//PC TIME，名称
                customListParameterList.ItemData[4].ItemText[0] = sMessageText[(Int32)language_temp - 1][3];//HMI SETTINGS，名称
                customListParameterList.ItemData[5].ItemText[0] = sMessageText[(Int32)language_temp - 1][20];//TOUCH SCREEN SETTINGS，名称
                if (parameter.Shift.ShiftState)//班次有效
                {
                    customListParameterList.ItemData[6].ItemText[0] = sMessageText[(Int32)language_temp - 1][19];//SHIFT SETTINGS，名称
                }

                //Value List

                switch (customListParameterList.CurrentDataIndex)//Parameter List列表当前选择的项
                {
                    case 0://MACHINE TYPE
                        //
                        //不执行操作
                        //
                        break;
                    case 1://CHECK LAYOUT
                        //
                        for (i = 0; i < parameter_temp.SystemCameraConfiguration.Length; i++)//
                        {
                            switch (language_temp)
                            {
                                case VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese://中文
                                    //
                                    customListValueList.ItemData[i].ItemText[0] = parameter_temp.SystemCameraConfiguration[i].CameraCHNName;//名称
                                    //
                                    break;
                                case VisionSystemClassLibrary.Enum.InterfaceLanguage.English://英文
                                    //
                                    customListValueList.ItemData[i].ItemText[0] = parameter_temp.SystemCameraConfiguration[i].CameraENGName;//名称
                                    //
                                    break;
                                default://其它，默认中文
                                    //
                                    customListValueList.ItemData[i].ItemText[0] = parameter_temp.SystemCameraConfiguration[i].CameraCHNName;//名称
                                    //
                                    break;
                            }
                        }
                        //
                        break;
                    case 2://CHECK ENABLE
                        //
                        k = 0;
                        for (i = 0; i < parameter_temp.SystemCameraConfiguration.Length; i++)//
                        {
                            if (parameter_temp.SystemCameraConfiguration[i].Selected)//选择
                            {
                                switch (language_temp)
                                {
                                    case VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese://中文
                                        //
                                        customListValueList.ItemData[k].ItemText[0] = parameter_temp.SystemCameraConfiguration[i].CameraCHNName;//名称
                                        //
                                        break;
                                    case VisionSystemClassLibrary.Enum.InterfaceLanguage.English://英文
                                        //
                                        customListValueList.ItemData[k].ItemText[0] = parameter_temp.SystemCameraConfiguration[i].CameraENGName;//名称
                                        //
                                        break;
                                    default://其它，默认中文
                                        //
                                        customListValueList.ItemData[k].ItemText[0] = parameter_temp.SystemCameraConfiguration[i].CameraCHNName;//名称
                                        //
                                        break;
                                }
                                k++;
                            }
                        }
                        //
                        break;
                    case 3://PC TIME
                        //
                        break;
                    case 4://HMI SETTINGS
                        //
                        customListValueList.ItemData[0].ItemText[0] = sMessageText[(Int32)language_temp - 1][7];//Language，名称
                        customListValueList.ItemData[1].ItemText[0] = sMessageText[(Int32)language_temp - 1][6];//Enable Password Protection，名称
                        //
                        break;
                    case 5://TOUCH SCREEN SETTINGS
                        //
                        break;
                    case 6://SHIFT SETTINGS
                        //
                        break;
                    default:
                        break;
                }

                //

                //事件

                if (null != LanguageChanged)//有效
                {
                    LanguageChanged(this, new CustomEventArgs());
                }
            }

            //

            customListParameterList._Refresh();//刷新
            customListValueList._Refresh();//刷新
        }

        //事件

        //----------------------------------------------------------------------
        // 功能说明：控件加载事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void SystemControl_Load(object sender, EventArgs e)
        {
            timerPCTime.Start();//启动
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：点击【Close】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonClose_CustomButton_Click(object sender, EventArgs e)
        {
            if (customButtonOk.Visible)//参数被修改
            {
                bClickCloseButton = true;

                //显示信息对话框

                GlobalWindows.MessageDisplay_Window.WindowParameter = 73;//窗口特征数值
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.OkCancel;//包含【确定】和【取消】按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language_temp;//语言
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][13] + "？";
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][13] + "？";

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
            else
            {
                bClickCloseButton = false;

                bCigaretteSortChanged = false;

                bCheckEnableChanged = false;

                bCheckLayoutChanged = false;

                bShiftTimeChanged = false;

                bMachineChanged = false;

                //事件

                if (null != Close_Click)//有效
                {
                    Close_Click(this, new CustomEventArgs());
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击Parameter List【Previous Page】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonPreviousPage_ParameterList_CustomButton_Click(object sender, EventArgs e)
        {
            customListParameterList._ClickPage(true);//翻页

            //事件

            if (null != PreviousPage_ParameterList_Click)//有效
            {
                PreviousPage_ParameterList_Click(this, new CustomEventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击Parameter List【Next Page】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonNextPage_ParameterList_CustomButton_Click(object sender, EventArgs e)
        {
            customListParameterList._ClickPage(false);//翻页

            //事件

            if (null != NextPage_ParameterList_Click)//有效
            {
                NextPage_ParameterList_Click(this, new CustomEventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击Value List【Previous Page】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonPreviousPage_ValueList_CustomButton_Click(object sender, EventArgs e)
        {
            customListValueList._ClickPage(true);//翻页

            //事件

            if (null != PreviousPage_ValueList_Click)//有效
            {
                PreviousPage_ValueList_Click(this, new CustomEventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击Value List【Next Page】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonNextPage_ValueList_CustomButton_Click(object sender, EventArgs e)
        {
            customListValueList._ClickPage(false);//翻页

            //事件

            if (null != NextPage_ValueList_Click)//有效
            {
                NextPage_ValueList_Click(this, new CustomEventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【Ok】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonOk_CustomButton_Click(object sender, EventArgs e)
        {
            //显示信息对话框

            GlobalWindows.MessageDisplay_Window.WindowParameter = 73;//窗口特征数值
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.OkCancel;//包含【确定】和【取消】按钮
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language_temp;//语言
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][13] + "？";
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][13] + "？";

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
        // 功能说明：点击【Cancel】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonCancel_CustomButton_Click(object sender, EventArgs e)
        {
            _SetOriginalValue();//恢复

            //事件

            if (null != Cancel_Click)//有效
            {
                Cancel_Click(this, new CustomEventArgs());
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：点击Parameter List列表事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customListParameterList_CustomListItem_Click(object sender, EventArgs e)
        {
            VisionSystemClassLibrary.Struct.SYSTEMTIME systemtime = new VisionSystemClassLibrary.Struct.SYSTEMTIME();//日期时间

            switch (customListParameterList.CurrentDataIndex)
            {
                case 0://MACHINE TYPE
                    //
                    customListValueList.ItemIconNumber = 1;
                    customListValueList.BitmapIcon[0] = VisionSystemControlLibrary.Properties.Resources.CustomListItem_Marked;
                    customListValueList.ItemIconIndex[0] = -1;
                    customListValueList.ItemIconIndex[1] = 0;
                    customListValueList.SelectionColumnIndex = 1;

                    _AddItemData(customListValueList, parameter_temp.MachineType.Length, false);//添加列表项
                    //
                    break;
                case 1://CHECK LAYOUT
                    //
                    customListValueList.ItemIconNumber = 1;
                    customListValueList.BitmapIcon[0] = VisionSystemControlLibrary.Properties.Resources.CustomListItem_Marked;
                    customListValueList.ItemIconIndex[0] = -1;
                    customListValueList.ItemIconIndex[1] = 0;
                    customListValueList.SelectionColumnIndex = 1;

                    _AddItemData(customListValueList, parameter_temp.SystemCameraConfiguration.Length, false);//添加列表项
                    //
                    break;
                case 2://CHECK ENABLE
                    //
                    customListValueList.ItemIconNumber = 1;
                    customListValueList.BitmapIcon[0] = VisionSystemControlLibrary.Properties.Resources.CustomListItem_Marked;
                    customListValueList.ItemIconIndex[0] = -1;
                    customListValueList.ItemIconIndex[1] = 0;
                    customListValueList.SelectionColumnIndex = 1;

                    _AddItemData(customListValueList, parameter_temp.SystemCameraConfiguration.Length, false);//添加列表项
                    //
                    break;
                case 3://PC TIME
                    //
                    customListValueList.ItemIconNumber = 0;
                    customListValueList.SelectionColumnIndex = -1;

                    _AddItemData(customListValueList, 0, false);//添加列表项

                    //

                    GetLocalTime(ref systemtime);

                    GlobalWindows.DateTimePanel_Window.WindowParameter = 1;//窗口特征数值
                    GlobalWindows.DateTimePanel_Window.DateTimePanelControl.Language = language_temp;//语言
                    GlobalWindows.DateTimePanel_Window.DateTimePanelControl.Chinese_Caption = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][2];//中文标题文本
                    GlobalWindows.DateTimePanel_Window.DateTimePanelControl.English_Caption = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][2];//英文标题文本
                    GlobalWindows.DateTimePanel_Window.DateTimePanelControl.PanelType = DateTimePanelType.DateTime;//日期时间设置面板类型
                    GlobalWindows.DateTimePanel_Window.DateTimePanelControl.DisplayTime_1 = systemtime;//日期时间

                    GlobalWindows.DateTimePanel_Window.StartPosition = FormStartPosition.CenterScreen;
                    if (GlobalWindows.TopMostWindows)//置顶
                    {
                        GlobalWindows.DateTimePanel_Window.TopMost = true;//将窗口置于顶层
                    }
                    else//其它
                    {
                        GlobalWindows.DateTimePanel_Window.Visible = true;//显示
                    }
                    //
                    break;
                case 4://HMI SETTINGS
                    //
                    customListValueList.ItemIconNumber = 0;
                    customListValueList.SelectionColumnIndex = -1;

                    _AddItemData(customListValueList, 2, false);//添加列表项
                    //
                    break;
                case 5://Cigarette
                    //
                    customListValueList.ItemIconNumber = 0;
                    customListValueList.SelectionColumnIndex = -1;

                    _AddItemData(customListValueList, 0, false);//添加列表项

                    //
                    Int32 j = 0;
                    for (Int32 i = 0; i < parameter_temp.SystemCameraConfiguration.Length; i++)
                    {
                        if (parameter_temp.SystemCameraConfiguration[i].IsSerialPort) //当前为串口
                        {
                            j++;
                        }
                    }

                    if (j > 0) //当前有相机被配置成串口
                    {
                        GlobalWindows.CigaretteSort_Window.CigaretteSortControl.Language = language;
                        GlobalWindows.CigaretteSort_Window.CigaretteSortControl._Properties(parameter_temp);

                        GlobalWindows.CigaretteSort_Window.StartPosition = FormStartPosition.CenterScreen;
                        if (GlobalWindows.TopMostWindows)//置顶
                        {
                            GlobalWindows.CigaretteSort_Window.TopMost = true;//将窗口置于顶层
                        }
                        else//其它
                        {
                            GlobalWindows.CigaretteSort_Window.Visible = true;//显示
                        }
                    }
                    //
                    break;
                case 6://SHIFT SETTINGS
                    //
                    customListValueList.ItemIconNumber = 0;
                    customListValueList.SelectionColumnIndex = -1;

                    _AddItemData(customListValueList, 0, false);//添加列表项

                    //

                    GlobalWindows.ShiftConfiguration_Window.ShiftConfigurationControl.Language = language_temp;//语言
                    GlobalWindows.ShiftConfiguration_Window.ShiftConfigurationControl.ShiftTimeConfiguration = new VisionSystemClassLibrary.Struct.ShiftTime[parameter_temp.Shift.DataOfShift.TimeData.Length];//班次数据
                    parameter_temp.Shift.DataOfShift.TimeData.CopyTo(GlobalWindows.ShiftConfiguration_Window.ShiftConfigurationControl.ShiftTimeConfiguration, 0);//班次设置数据
                    GlobalWindows.ShiftConfiguration_Window.ShiftConfigurationControl.ShiftTimeConfiguration_Original = new VisionSystemClassLibrary.Struct.ShiftTime[parameter_temp.Shift.DataOfShift.TimeData.Length];//班次数据
                    parameter_temp.Shift.DataOfShift.TimeData.CopyTo(GlobalWindows.ShiftConfiguration_Window.ShiftConfigurationControl.ShiftTimeConfiguration_Original, 0);//班次设置数据
                    GlobalWindows.ShiftConfiguration_Window.ShiftConfigurationControl.MinShiftNumber = VisionSystemClassLibrary.Class.Shift.MinShiftNumber;//最小值
                    GlobalWindows.ShiftConfiguration_Window.ShiftConfigurationControl.MaxShiftNumber = VisionSystemClassLibrary.Class.Shift.MaxShiftNumber;//最大值
                    GlobalWindows.ShiftConfiguration_Window.ShiftConfigurationControl.ApplySettings = true;//应用设置

                    GlobalWindows.ShiftConfiguration_Window.StartPosition = FormStartPosition.CenterScreen;
                    if (GlobalWindows.TopMostWindows)//置顶
                    {
                        GlobalWindows.ShiftConfiguration_Window.TopMost = true;//将窗口置于顶层
                    }
                    else//其它
                    {
                        GlobalWindows.ShiftConfiguration_Window.Visible = true;//显示
                    }
                    //
                    break;
                default:
                    break;
            }

            //事件

            if (null != ParameterListItem_Click)//有效
            {
                ParameterListItem_Click(this, new CustomEventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击Value List列表事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customListValueList_CustomListItem_Click(object sender, EventArgs e)
        {
            Int32 i = 0;//循环控制变量

            switch (customListParameterList.CurrentDataIndex)
            {
                case 0://MACHINE TYPE
                    //
                    if (customListValueList.ItemData[customListValueList.CurrentDataIndex].ItemDataDisplay[1])//未选择（说明点击之前处于选择状态，需要恢复该状态）
                    {
                        customListValueList.ItemData[customListValueList.CurrentDataIndex].ItemDataDisplay[1] = false;//选择当前项

                        customListValueList.SelectedItemNumber = 1;
                    }
                    else//选择（说明点击之前处于未选择状态，需要取消其他选择的项）
                    {
                        customListValueList._SelectAll(false);//全部取消选择

                        customListValueList.ItemData[customListValueList.CurrentDataIndex].ItemDataDisplay[1] = false;//选择当前项

                        customListValueList.SelectedItemNumber = 1;

                        //

                        parameter_temp.SelectedMachineType = (UInt16)customListValueList.CurrentDataIndex;

                        _SetControl();//更新控件
                    }

                    customListValueList._Refresh();//刷新

                    //

                    customListParameterList.ItemData[0].ItemText[1] = parameter_temp.MachineType[customListValueList.CurrentDataIndex];

                    customListParameterList._Refresh(0);
                    //
                    break;
                case 1://CHECK LAYOUT
                    //
                    parameter_temp.SystemCameraConfiguration[customListValueList.CurrentDataIndex].Selected = !(customListValueList.ItemData[customListValueList.CurrentDataIndex].ItemDataDisplay[1]);

                    _SetControl();//更新控件

                    //

                    customListParameterList.ItemData[1].ItemText[1] = "";//CHECK LAYOUT，数值
                    for (i = 0; i < parameter_temp.SystemCameraConfiguration.Length; i++)
                    {
                        if (parameter_temp.SystemCameraConfiguration[i].Selected)//选择
                        {
                            switch (language_temp)
                            {
                                case VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese://中文
                                    //
                                    customListParameterList.ItemData[1].ItemText[1] += parameter_temp.SystemCameraConfiguration[i].CameraCHNName + "，";//CHECK LAYOUT，数值
                                    //
                                    break;
                                case VisionSystemClassLibrary.Enum.InterfaceLanguage.English://英文
                                    //
                                    customListParameterList.ItemData[1].ItemText[1] += parameter_temp.SystemCameraConfiguration[i].CameraENGName + "，";//CHECK LAYOUT，数值
                                    //
                                    break;
                                default://其它，默认中文
                                    //
                                    customListParameterList.ItemData[1].ItemText[1] += parameter_temp.SystemCameraConfiguration[i].CameraCHNName + "，";//CHECK LAYOUT，数值
                                    //
                                    break;
                            }
                        }
                    }
                    if ("" != customListParameterList.ItemData[1].ItemText[1])
                    {
                        customListParameterList.ItemData[1].ItemText[1] = customListParameterList.ItemData[1].ItemText[1].Substring(0, customListParameterList.ItemData[1].ItemText[1].Length - 1);//CHECK LAYOUT，数值
                    }

                    customListParameterList._Refresh(1);
                    //
                    break;
                case 2://CHECK ENABLE
                    //

                    Int32 currentDataIndex = _GetSystemCameraConfigurationIndex(parameter_temp, customListValueList.CurrentDataIndex);//查询系统相机索引

                    if ((currentDataIndex > -1) && (currentDataIndex < parameter_temp.SystemCameraConfiguration.Length))//相机索引有效
                    {
                        parameter_temp.SystemCameraConfiguration[currentDataIndex].CheckEnable = !(customListValueList.ItemData[customListValueList.CurrentDataIndex].ItemDataDisplay[1]);
                    }

                    _SetControl();//更新控件

                    //
                    
                    customListParameterList._Refresh(2);
                    //
                    break;
                case 3://PC TIME
                    //
                    break;
                case 4://HMI SETTINGS
                    //
                    switch (customListValueList.CurrentDataIndex)
                    {
                        case 0://Language
                            //
                            switch (language_temp)
                            {
                                case VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese://中文
                                    //
                                    language_temp = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
                                    //
                                    break;
                                case VisionSystemClassLibrary.Enum.InterfaceLanguage.English://英文
                                    //
                                    language_temp = VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese;
                                    //
                                    break;
                                default:
                                    break;
                            }

                            //

                            _SetLanguage();

                            //

                            customListParameterList.ItemData[0].ItemText[0] = sMessageText[(Int32)language_temp - 1][0];//MACHINE TYPE，名称
                            customListParameterList.ItemData[1].ItemText[0] = sMessageText[(Int32)language_temp - 1][1];//CHECK LAYOUT，名称
                            customListParameterList.ItemData[2].ItemText[0] = sMessageText[(Int32)language_temp - 1][21];//CHECK ENABLE，名称
                            customListParameterList.ItemData[3].ItemText[0] = sMessageText[(Int32)language_temp - 1][2];//PC TIME，名称
                            customListParameterList.ItemData[4].ItemText[0] = sMessageText[(Int32)language_temp - 1][3];//HMI SETTINGS，名称
                            customListParameterList.ItemData[5].ItemText[0] = sMessageText[(Int32)language_temp - 1][20];//TOUCH SCREEN SETTINGS，名称
                            if (parameter.Shift.ShiftState)//班次有效
                            {
                                customListParameterList.ItemData[6].ItemText[0] = sMessageText[(Int32)language_temp - 1][19];//SHIFT SETTINGS，名称
                            }

                            customListParameterList.ItemData[1].ItemText[1] = "";//清零
                            for (i = 0; i < parameter_temp.SystemCameraConfiguration.Length; i++)//CHECK LAYOUT，数值
                            {
                                if (parameter_temp.SystemCameraConfiguration[i].Selected)//选择
                                {
                                    switch (language_temp)
                                    {
                                        case VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese://中文
                                            //
                                            customListParameterList.ItemData[1].ItemText[1] += parameter_temp.SystemCameraConfiguration[i].CameraCHNName + "，";//CHECK LAYOUT，数值
                                            //
                                            break;
                                        case VisionSystemClassLibrary.Enum.InterfaceLanguage.English://英文
                                            //
                                            customListParameterList.ItemData[1].ItemText[1] += parameter_temp.SystemCameraConfiguration[i].CameraENGName + "，";//CHECK LAYOUT，数值
                                            //
                                            break;
                                        default://其它，默认中文
                                            //
                                            customListParameterList.ItemData[1].ItemText[1] += parameter_temp.SystemCameraConfiguration[i].CameraCHNName + "，";//CHECK LAYOUT，数值
                                            //
                                            break;
                                    }
                                }
                            }
                            if ("" != customListParameterList.ItemData[1].ItemText[1])
                            {
                                customListParameterList.ItemData[1].ItemText[1] = customListParameterList.ItemData[1].ItemText[1].Substring(0, customListParameterList.ItemData[1].ItemText[1].Length - 1);//CHECK LAYOUT，数值
                            }

                            customListParameterList.ItemData[2].ItemText[1] = customListParameterList.ItemData[1].ItemText[1];//CHECK ENABLE，数值

                            //

                            customListValueList.ItemData[0].ItemText[0] = sMessageText[(Int32)language_temp - 1][7];//Language，名称
                            switch (language_temp)
                            {
                                case VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese://中文
                                    //
                                    customListValueList.ItemData[0].ItemText[1] = sMessageText[(Int32)language_temp - 1][8];//Language，数值
                                    //
                                    break;
                                case VisionSystemClassLibrary.Enum.InterfaceLanguage.English://英文
                                    //
                                    customListValueList.ItemData[0].ItemText[1] = sMessageText[(Int32)language_temp - 1][9];//Language，数值
                                    //
                                    break;
                                default://其它，默认中文
                                    //
                                    customListValueList.ItemData[0].ItemText[1] = sMessageText[(Int32)language_temp - 1][8];//Language，数值
                                    //
                                    break;
                            }
                            //
                            customListValueList.ItemData[1].ItemText[0] = sMessageText[(Int32)language_temp - 1][6];//Enable Password Protection，名称
                            if ("" != parameter_temp.UserPassword)//开启
                            {
                                customListValueList.ItemData[1].ItemText[1] = sMessageText[(Int32)language_temp - 1][10];//Enable Password Protection，数值
                            }
                            else//关闭
                            {
                                customListValueList.ItemData[1].ItemText[1] = sMessageText[(Int32)language_temp - 1][11];//Enable Password Protection，数值
                            }

                            customListParameterList._Refresh();//刷新
                            customListValueList._Refresh();//刷新

                            //

                            _SetControl();//更新

                            //

                            //事件

                            if (null != LanguageChanged)//有效
                            {
                                LanguageChanged(this, new CustomEventArgs());
                            }
                            //
                            break;
                        case 1://Enable Password Protection
                            //
                            GlobalWindows.StandardKeyboard_Window.WindowParameter = 5;//窗口特征数值
                            GlobalWindows.StandardKeyboard_Window.Language = language_temp;//语言
                            GlobalWindows.StandardKeyboard_Window.Chinese_Caption = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][6];//中文标题文本
                            GlobalWindows.StandardKeyboard_Window.English_Caption = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][6];//英文标题文本
                            GlobalWindows.StandardKeyboard_Window.IsPassword = true;//密码输入窗口
                            if ("" == parameter_temp.UserPassword)//关闭
                            {
                                GlobalWindows.StandardKeyboard_Window.PasswordStyle = 2;//属性，密码输入类型。取值范围：0，密码输入（输入完成，正确，关闭窗口）；1，输入当前密码；2，输入新的密码；3，确认密码
                            }
                            else//开启
                            {
                                GlobalWindows.StandardKeyboard_Window.PasswordStyle = 0;//属性，密码输入类型。取值范围：0，密码输入（输入完成，正确，关闭窗口）；1，输入当前密码；2，输入新的密码；3，确认密码
                                GlobalWindows.StandardKeyboard_Window.Password = parameter_temp.UserPassword + "\n" + parameter_temp.AdministratorPassword;//属性，当前密码
                            }
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
                            //
                            break;
                        default:
                            break;
                    }
                    //
                    break;
                case 5://TOUCH SCREEN SETTINGS
                    //
                    break;
                case 6://SHIFT SETTINGS
                    //
                    break;
                default:
                    break;
            }

            //事件

            if (null != ValueListItem_Click)//有效
            {
                ValueListItem_Click(this, new CustomEventArgs());
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
        private void timerPCTime_Tick(object sender, EventArgs e)
        {
            if (!bDateTimeChanged)//更新
            {
                try
                {
                    VisionSystemClassLibrary.Struct.SYSTEMTIME systemtime = new VisionSystemClassLibrary.Struct.SYSTEMTIME();//日期时间

                    GetLocalTime(ref systemtime);//获取当前日期时间

                    //

                    sPCTime = _GetDateTime(systemtime);

                    customListParameterList.ItemData[3].ItemText[1] = sPCTime;//PC TIME，数值

                    customListParameterList._Refresh(3);//刷新

                    //

                    if ((DateTimePanelType.DateTime == GlobalWindows.DateTimePanel_Window.DateTimePanelControl.PanelType) && (!GlobalWindows.DateTimePanel_Window.DateTimePanelControl.DateTimeChanged))//日期时间面板
                    {
                        GlobalWindows.DateTimePanel_Window.DateTimePanelControl.DisplayTime_1 = systemtime;//日期时间
                    }
                }
                catch (System.Exception ex)
                {
                    //不执行操作
                }

                //事件

                if (null != PCTimerTick)//有效
                {
                    PCTimerTick(this, new CustomEventArgs());
                }
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：SYSTEM，日期时间修改，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void dateTimePanelWindow_WindowClose_System_DateTime(object sender, EventArgs e)
        {
            VisionSystemClassLibrary.Struct.SYSTEMTIME systemtime = new VisionSystemClassLibrary.Struct.SYSTEMTIME();//日期时间

            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.DateTimePanel_Window.TopMost = false;//取消置于顶层
            }
            else//其它
            {
                GlobalWindows.DateTimePanel_Window.Visible = false;//隐藏
            }

            //

            if (GlobalWindows.DateTimePanel_Window.DateTimePanelControl.EnterNewValue)//输入完成
            {
                bDateTimeChanged = true;

                //

                GetLocalTime(ref systemtime);//获取当前日期时间

                SystemTimeSetting.Year = GlobalWindows.DateTimePanel_Window.DateTimePanelControl.DisplayTime_1.Year;
                SystemTimeSetting.Month = GlobalWindows.DateTimePanel_Window.DateTimePanelControl.DisplayTime_1.Month;
                SystemTimeSetting.Day = GlobalWindows.DateTimePanel_Window.DateTimePanelControl.DisplayTime_1.Day;
                SystemTimeSetting.Hour = GlobalWindows.DateTimePanel_Window.DateTimePanelControl.DisplayTime_1.Hour;
                SystemTimeSetting.Minute = GlobalWindows.DateTimePanel_Window.DateTimePanelControl.DisplayTime_1.Minute;
                SystemTimeSetting.Second = GlobalWindows.DateTimePanel_Window.DateTimePanelControl.DisplayTime_1.Second;

                _SetControl();//更新控件

                //

                customListParameterList.ItemData[3].ItemText[1] = _GetDateTime(SystemTimeSetting);
                customListParameterList._Refresh(3);
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：SHIFT CONFIGURATION，班次设置，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void shiftConfigurationWindow_WindowClose(object sender, EventArgs e)
        {
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.ShiftConfiguration_Window.TopMost = false;//取消置于顶层
            }
            else//其它
            {
                GlobalWindows.ShiftConfiguration_Window.Visible = false;//隐藏
            }

            //

            if (GlobalWindows.ShiftConfiguration_Window.ShiftConfigurationControl.EnterNewValue)//输入完成
            {
                bShiftTimeChanged = true;

                parameter_temp.Shift._CreateNewShift(GlobalWindows.ShiftConfiguration_Window.ShiftConfigurationControl.ShiftTimeConfiguration);

                //

                _SetControl();//更新控件
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：SYSTEM，密码，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void standardKeyboardWindow_WindowClose_System_Password(object sender, EventArgs e)
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

            if (GlobalWindows.StandardKeyboard_Window.EnterNewValue)//输入完成
            {
                if ("" != parameter_temp.UserPassword)//关闭
                {
                    parameter_temp.UserPassword = "";

                    //

                    customListValueList.ItemData[1].ItemText[1] = sMessageText[(Int32)language_temp - 1][11];//Enable Password Protection，数值
                }
                else//开启
                {
                    parameter_temp.UserPassword = GlobalWindows.StandardKeyboard_Window.StringValue;

                    //

                    customListValueList.ItemData[1].ItemText[1] = sMessageText[(Int32)language_temp - 1][10];//Enable Password Protection，数值
                }

                //

                customListValueList._Refresh(1);//刷新

                _SetControl();//更新控件
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：SYSTEM，【OK】（保存参数）提示，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_System_Ok_Confirm(object sender, EventArgs e)
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

            if (GlobalWindows.MessageDisplay_Window.MessageDisplayControl.OkCancel)//保存参数
            {
                //显示等待窗口

                bCommonParameterMessageWindowShow = true;

                GlobalWindows.MessageDisplay_Window.WindowParameter = 53;//窗口特征数值
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.None;//不包含任何按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language_temp;//语言
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][14] + "...";
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][14] + "...";

                GlobalWindows.MessageDisplay_Window.StartPosition = FormStartPosition.CenterScreen;
                if (GlobalWindows.TopMostWindows)//置顶
                {
                    GlobalWindows.MessageDisplay_Window.TopMost = true;//将窗口置于顶层
                }
                else//其它
                {
                    GlobalWindows.MessageDisplay_Window.Visible = true;//显示
                }

                GlobalWindows.MessageDisplay_Window.Update();

                //

                Int32 i = 0;//循环控制变量
                Int32 j = 0;//循环控制变量

                for (i = 0; i < parameter_temp.SystemCameraConfiguration.Length; i++)//遍历
                {
                    if (parameter_temp.SystemCameraConfiguration[i].Selected)//选中
                    {
                        if (parameter_temp.SystemCameraConfiguration[i].CheckEnable != parameter.SystemCameraConfiguration[i].CheckEnable)//修改
                        {
                            break;
                        }
                    }
                }

                if (i < parameter_temp.SystemCameraConfiguration.Length)//修改
                {
                    bCheckEnableChanged = true;
                }
                else//未修改
                {
                    bCheckEnableChanged = false;
                }

                for (i = 0; i < parameter_temp.SystemCameraConfiguration.Length; i++)//遍历
                {
                    if (parameter_temp.SystemCameraConfiguration[i].Selected != parameter.SystemCameraConfiguration[i].Selected)//修改
                    {
                        break;
                    }
                }

                if (i < parameter_temp.SystemCameraConfiguration.Length)//修改
                {
                    bCheckLayoutChanged = true;
                }
                else//未修改
                {
                    bCheckLayoutChanged = false;
                }

                for (i = 0; i < parameter_temp.SystemCameraConfiguration.Length; i++)//遍历
                {
                    if (parameter_temp.SystemCameraConfiguration[i].IsSerialPort) //当前为串口
                    {
                        if (parameter_temp.SystemCameraConfiguration[i].TobaccoSortType_E != parameter.SystemCameraConfiguration[i].TobaccoSortType_E)//修改
                        {
                            break;
                        }
                    }
                }

                if (i < parameter_temp.SystemCameraConfiguration.Length)//修改
                {
                    bCigaretteSortChanged = true;
                }
                else//未修改
                {
                    bCigaretteSortChanged = false;
                }

                //

                if (parameter_temp.SelectedMachineType != parameter.SelectedMachineType)//机器类型修改
                {
                    bMachineChanged = true;
                }
                else//机器类型未修改
                {
                    bMachineChanged = false;
                }

                //

                parameter_temp._CopySystemParameterTo(parameter);//保存

                VisionSystemClassLibrary.Class.System.Language = language_temp;//保存

                parameter._SetLanguage_Device();//更新设备名称

                for (i = 0; i < parameter.Camera.Length; i++)
                {
                    parameter.Camera[i]._SetLanguage();//更新相机名称

                    //

                    for (j = 0; j < parameter.Camera[i].Tools.Count; j++)
                    {
                        parameter.Camera[i].Tools[j]._SetLanguage();//更新工具名称
                    }
                }

                //

                if (parameter.Shift.ShiftState)//使用班次
                {
                    if (bShiftTimeChanged)//班次修改
                    {
                        parameter_temp.Shift._CopyTo(ref parameter.Shift.DataOfShift.TimeData);

                        //

                        parameter.Shift._WriteShiftTime();
                    }
                }

                //

                customButtonOk.Visible = false;//【OK】
                customButtonCancel.Visible = false;//【Cancel】

                //

                parameter._WriteSystemParameter();//写文件

                //

                if (bDateTimeChanged)//修改了日期时间
                {
                    SetLocalTime(ref SystemTimeSetting);

                    //

                    bDateTimeChanged = false;
                }

                //

                if (bCheckEnableChanged || bCheckLayoutChanged || bShiftTimeChanged || bMachineChanged || bCigaretteSortChanged)//修改
                {
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][14] + "...";
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][14] + "...";
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = " ";
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = " ";
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][18] + "，" + iTimerCommonParameterCount.ToString();
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][18] + "，" + iTimerCommonParameterCount.ToString();

                    timerCommonParameter.Start();//启动定时器

                    //事件

                    if (null != Ok_CommonParameterChanged_Click)//有效
                    {
                        Ok_CommonParameterChanged_Click(this, new CustomEventArgs());
                    }
                }
                else//未修改
                {
                    //事件

                    if (null != Ok_Click)//有效
                    {
                        Ok_Click(this, new CustomEventArgs());
                    }

                    //

                    bCommonParameterMessageWindowShow = false;

                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【OK】按钮
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][15];
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][15];
                }
            }
            else//不保存参数
            {
                if (bClickCloseButton)//点击【Close】按钮
                {
                    bClickCloseButton = false;

                    bCigaretteSortChanged = false;

                    bCheckEnableChanged = false;

                    bCheckLayoutChanged = false;

                    bShiftTimeChanged = false;

                    bMachineChanged = false;

                    //

                    _SetOriginalValue();//恢复

                    //事件

                    if (null != Close_Click)//有效
                    {
                        Close_Click(this, new CustomEventArgs());
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：SYSTEM，【OK】等待，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_System_Ok_Wait(object sender, EventArgs e)
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

            if (0 >= iCommonParameterNumber)//成功
            {
                //事件

                if (null != Ok_Click)//有效
                {
                    Ok_Click(this, new CustomEventArgs());
                }
            }
            else//失败
            {
                //事件

                if (null != Close_Click)//有效
                {
                    Close_Click(this, new CustomEventArgs());
                }
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：定时器事件，机器选择完成后,执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void timerCommonParameter_Tick(object sender, EventArgs e)
        {
            if (bCommonParameterMessageWindowShow)
            {
                iTimerCommonParameterCount--;

                if (0 >= iTimerCommonParameterCount)//超时
                {
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][16];
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][16];
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][17];
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][17];

                    timerCommonParameter.Stop();//关闭定时器

                    iTimerCommonParameterCount = iTimerCommonParameterMaxCount;
                }
                else//计数
                {
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][18] + "，" + iTimerCommonParameterCount.ToString();
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][18] + "，" + iTimerCommonParameterCount.ToString();
                }
            }
        }
        
        //----------------------------------------------------------------------
        // 功能说明：获取系统配置类型相机索引
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private Int32 _GetSystemCameraConfigurationIndex(VisionSystemClassLibrary.Class.System systemParameter, Int32 currentDataIndex)
        {
            Int32 k = 0;//循环控制变量
            Int32 byteReturn = -1;

            for (Int32 i = 0; i < systemParameter.SystemCameraConfiguration.Length; i++)//遍历相机
            {
                if (parameter_temp.SystemCameraConfiguration[i].Selected)//选择
                {
                    if (k == currentDataIndex)
                    {
                        byteReturn = i;
                        break;
                    }
                    k++;
                }
            }
            return byteReturn;
        }

        //----------------------------------------------------------------------
        // 功能说明：SYSTEM，CigaretteSort_Window窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void CigaretteSort_Window_WindowClose(object sender, EventArgs e)
        {
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.CigaretteSort_Window.TopMost = false;//取消置于顶层
            }
            else//其它
            {
                GlobalWindows.CigaretteSort_Window.Visible = false;//隐藏
            }

            _SetControl();
        }
    }
}