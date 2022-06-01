/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：GlobalWindows.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：全局窗口

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

namespace VisionSystemControlLibrary
{
    public partial class GlobalWindows : UserControl
    {
        //本控件仅用于定义全局数据，无需在设计器中使用

        public static Boolean TopMostWindows = false;//各个窗口是否置顶显示。取值范围：true，是；false，否

        //

        public static DigitalKeyboardWindow DigitalKeyboard_Window;//数字键盘窗口

        public static StandardKeyboardWindow StandardKeyboard_Window;//标准键盘窗口

        public static MessageDisplayWindow MessageDisplay_Window;//提示信息窗口

        public static DeviceConfigurationWindow DeviceConfiguration_Window;//CONFIG DEVICE窗口

        public static IOSignalDiagnosisWindow IOSignalDiagnosis_Window;//TEST I/O窗口

        public static DateTimePanelWindow DateTimePanel_Window;//日期时间面板窗口

        public static ShiftConfigurationWindow ShiftConfiguration_Window;//班次设置窗口

        public static StatisticsRecordWindow StatisticsRecord_Window;//统计记录窗口

        public static FaultMessageOptionWindow FaultMessageOption_Window;//故障信息选项窗口

        public static FaultMessageWindow FaultMessage_Window;//故障信息窗口

        public static EditToolsWindow EditTools_Window;//编辑工具窗口

        public static NewToolWindow NewTool_Window;//新建工具窗口

        public static ParameterSettingsWindow ParameterSettings_Window;//参数设置窗口

        public static NetDiagnoseWindows NetDiagnose_Window;//网络检查窗口

        public static SensorSelectWindow SensorSelect_Window;//传感器选择窗口

        public static CigaretteSortWindow CigaretteSort_Window;//传感器选择窗口
                    
        //构造函数

        //-----------------------------------------------------------------------
        // 功能说明：构造函数
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public GlobalWindows()
        {
            InitializeComponent();
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：创建窗口
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static void _Create_StandardKeyboard_Window()
        {
            StandardKeyboard_Window = new StandardKeyboardWindow();//提示信息窗口
            System.Threading.Thread.Sleep(1);
        }

        // 功能说明：创建窗口
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static void _Create_MessageDisplay_Window()
        {
            MessageDisplay_Window = new MessageDisplayWindow();//提示信息窗口
            System.Threading.Thread.Sleep(1);
        }

        // 功能说明：创建窗口
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static void _Create_FaultMessage_Window()
        {
            FaultMessage_Window = new FaultMessageWindow();//故障信息窗口
            System.Threading.Thread.Sleep(1);
        }

        // 功能说明：创建窗口
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static void _Create_FaultMessageOption_Window()
        {
            FaultMessageOption_Window = new FaultMessageOptionWindow();//故障信息选项窗口
            System.Threading.Thread.Sleep(1);
        }

        //-----------------------------------------------------------------------
        // 功能说明：创建窗口
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static void _Create_DigitalKeyboard_Window()
        {
            DigitalKeyboard_Window = new DigitalKeyboardWindow();//数字键盘窗口
            System.Threading.Thread.Sleep(1);
        }

        //-----------------------------------------------------------------------
        // 功能说明：创建窗口
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static void _Create_DeviceConfiguration_Window()
        {
            DeviceConfiguration_Window = new DeviceConfigurationWindow();//CONFIG DEVICE窗口
            System.Threading.Thread.Sleep(1);
        }

        //-----------------------------------------------------------------------
        // 功能说明：创建窗口
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static void _Create_IOSignalDiagnosis_Window()
        {
            IOSignalDiagnosis_Window = new IOSignalDiagnosisWindow();//TEST I/O窗口
            System.Threading.Thread.Sleep(1);
        }

        //-----------------------------------------------------------------------
        // 功能说明：创建窗口
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static void _Create_DateTimePanel_Window()
        {
            DateTimePanel_Window = new DateTimePanelWindow();//日期时间面板窗口
            System.Threading.Thread.Sleep(1);
        }

        //-----------------------------------------------------------------------
        // 功能说明：创建窗口
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static void _Create_ShiftConfiguration_Window()
        {
            ShiftConfiguration_Window = new ShiftConfigurationWindow();//班次设置窗口
            System.Threading.Thread.Sleep(1);
        }

        //-----------------------------------------------------------------------
        // 功能说明：创建窗口
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static void _Create_StatisticsRecord_Window()
        {
            StatisticsRecord_Window = new StatisticsRecordWindow();//统计记录窗口
            System.Threading.Thread.Sleep(1);
        }

        //-----------------------------------------------------------------------
        // 功能说明：创建窗口
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static void _Create_EditTools_Window()
        {
            EditTools_Window = new EditToolsWindow();//编辑工具窗口
            System.Threading.Thread.Sleep(1);
        }

        //-----------------------------------------------------------------------
        // 功能说明：创建窗口
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static void _Create_NewTool_Window()
        {
            NewTool_Window = new NewToolWindow();//新建工具窗口
            System.Threading.Thread.Sleep(1);
        }

        //-----------------------------------------------------------------------
        // 功能说明：创建窗口
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static void _Create_ParameterSettings_Window()
        {
            ParameterSettings_Window = new ParameterSettingsWindow();//参数设置窗口
            System.Threading.Thread.Sleep(1);
        }

        //-----------------------------------------------------------------------
        // 功能说明：创建窗口
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static void _Create_NetDiagnose_Window()
        {
            NetDiagnose_Window = new NetDiagnoseWindows();//提示信息窗口
            System.Threading.Thread.Sleep(1);
        }

        //-----------------------------------------------------------------------
        // 功能说明：创建窗口
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static void _Create_SensorSelect_Window()
        {
            SensorSelect_Window = new SensorSelectWindow();//提示信息窗口
            System.Threading.Thread.Sleep(1);
        }

        //-----------------------------------------------------------------------
        // 功能说明：创建窗口
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static void _Create_CigaretteSort_Window()
        {
            CigaretteSort_Window = new CigaretteSortWindow();//提示信息窗口
            System.Threading.Thread.Sleep(1);
        }
    }
}