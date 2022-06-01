/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：DigitalKeyboardWindow.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：数字键盘窗口

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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VisionSystemControlLibrary
{
    public partial class DigitalKeyboardWindow : Form
    {
        //数字小键盘窗口

        private Int32 iWindowParameter = 0;//属性，窗口特征数值，表示调用窗口的父级窗口类型，以便产生相应的事件。取值范围：
        //1.QUALITY CHECK，工具参数修改
        //2.IMAGE CONFIGURATION，工具参数修改
        //3.DATETIMEPANEL，年
        //4.DATETIMEPANEL，月
        //5.DATETIMEPANEL，日
        //6.DATETIMEPANEL，时
        //7.DATETIMEPANEL，分
        //8.DATETIMEPANEL，秒
        //10.TOLERANCES，最大值设置
        //11.TOLERANCES，最小值设置
        //12.SHIFT CONFIGURATION，班次数
        //13.STATISTICS，REJECT IMAGE SELECTION
        //14.TOLERANCES，EJECT LEVEL
        //15.PARAMETER SETTINGS，参数

        //

        [Browsable(true), Description("QUALITY CHECK，工具参数修改，窗口关闭时产生的事件"), Category("DigitalKeyboardWindow 事件")]
        public event EventHandler WindowClose_QualityCheck_ToolParameter;//QUALITY CHECK，工具参数修改，窗口关闭时产生的事件

        [Browsable(true), Description("IMAGE CONFIGURATION，工具参数修改，窗口关闭时产生的事件"), Category("DigitalKeyboardWindow 事件")]
        public event EventHandler WindowClose_ImageConfiguration_ToolParameter;//IMAGE CONFIGURATION，工具参数修改，窗口关闭时产生的事件

        //

        [Browsable(true), Description("DATETIMEPANEL，年，窗口关闭时产生的事件"), Category("DigitalKeyboardWindow 事件")]
        public event EventHandler WindowClose_DateTimePanel_Year;//DATETIMEPANEL，年，窗口关闭时产生的事件

        [Browsable(true), Description("DATETIMEPANEL，月，窗口关闭时产生的事件"), Category("DigitalKeyboardWindow 事件")]
        public event EventHandler WindowClose_DateTimePanel_Month;//DATETIMEPANEL，月，窗口关闭时产生的事件

        [Browsable(true), Description("DATETIMEPANEL，日，窗口关闭时产生的事件"), Category("DigitalKeyboardWindow 事件")]
        public event EventHandler WindowClose_DateTimePanel_Day;//DATETIMEPANEL，日，窗口关闭时产生的事件

        [Browsable(true), Description("DATETIMEPANEL，时，窗口关闭时产生的事件"), Category("DigitalKeyboardWindow 事件")]
        public event EventHandler WindowClose_DateTimePanel_Hour;//DATETIMEPANEL，时，窗口关闭时产生的事件

        [Browsable(true), Description("DATETIMEPANEL，分，窗口关闭时产生的事件"), Category("DigitalKeyboardWindow 事件")]
        public event EventHandler WindowClose_DateTimePanel_Minute;//DATETIMEPANEL，分，窗口关闭时产生的事件

        [Browsable(true), Description("DATETIMEPANEL，秒，窗口关闭时产生的事件"), Category("DigitalKeyboardWindow 事件")]
        public event EventHandler WindowClose_DateTimePanel_Second;//DATETIMEPANEL，秒，窗口关闭时产生的事件

        //

        [Browsable(true), Description("TOLERANCES，最大值设置，窗口关闭时产生的事件"), Category("DigitalKeyboardWindow 事件")]
        public event EventHandler WindowClose_Tolerances_Max;//TOLERANCES，最大值设置，窗口关闭时产生的事件

        [Browsable(true), Description("TOLERANCES，最小值设置，窗口关闭时产生的事件"), Category("DigitalKeyboardWindow 事件")]
        public event EventHandler WindowClose_Tolerances_Min;//TOLERANCES，最小值设置，窗口关闭时产生的事件

        //

        [Browsable(true), Description("SHIFT CONFIGURATION，班次数，窗口关闭时产生的事件"), Category("DigitalKeyboardWindow 事件")]
        public event EventHandler WindowClose_ShiftConfiguration_ShiftNumber;//SHIFT CONFIGURATION，班次数，窗口关闭时产生的事件

        //

        [Browsable(true), Description("STATISTICS，REJECT IMAGE SELECTION，窗口关闭时产生的事件"), Category("DigitalKeyboardWindow 事件")]
        public event EventHandler WindowClose_Statistics_RejectImageSelection;//STATISTICS，REJECT IMAGE SELECTION，窗口关闭时产生的事件

        //

        [Browsable(true), Description("TOLERANCES，EJECT LEVEL，窗口关闭时产生的事件"), Category("DigitalKeyboardWindow 事件")]
        public event EventHandler WindowClose_Tolerances_EjectLevel;//TOLERANCES，EJECT LEVEL，窗口关闭时产生的事件

        //

        [Browsable(true), Description("PARAMETER SETTINGS，参数，窗口关闭时产生的事件"), Category("ParameterSettingsWindow 事件")]
        public event EventHandler WindowClose_ParameterSettings_Parameter;//PARAMETER SETTINGS，参数，窗口关闭时产生的事件

        //

        //构造函数

        //----------------------------------------------------------------------
        // 功能说明：构造函数，初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public DigitalKeyboardWindow()
        {
            InitializeComponent();
        }
        
        //属性

        //----------------------------------------------------------------------
        // 功能说明：DigitalKeyboardControl属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("控件"), Category("DigitalKeyboardWindow 通用")]
        public DigitalKeyboard DigitalKeyboardControl//属性
        {
            get//读取
            {
                return digitalKeyboard;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：WindowParameter属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("窗口特征数值，表示调用窗口的父级窗口类型，以便产生相应的事件"), Category("DigitalKeyboardWindow 通用")]
        public Int32 WindowParameter//属性
        {
            get//读取
            {
                return iWindowParameter;
            }
            set//设置
            {
                iWindowParameter = value;
            }
        }

        //事件

        //-----------------------------------------------------------------------
        // 功能说明：点击【Esc】或【Enter】时产生的事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void digitalKeyboard_Close_Click(object sender, EventArgs e)
        {
            if (1 == iWindowParameter)//QUALITY CHECK，工具参数修改
            {
                if (null != WindowClose_QualityCheck_ToolParameter)//有效
                {
                    WindowClose_QualityCheck_ToolParameter(this, new CustomEventArgs());
                }
            }
            else if (2 == iWindowParameter)//IMAGE CONFIGURATION，工具参数修改
            {
                if (null != WindowClose_ImageConfiguration_ToolParameter)//有效
                {
                    WindowClose_ImageConfiguration_ToolParameter(this, new CustomEventArgs());
                }
            }
            else if (3 == iWindowParameter)//DATETIMEPANEL，年
            {
                if (null != WindowClose_DateTimePanel_Year)//有效
                {
                    WindowClose_DateTimePanel_Year(this, new CustomEventArgs());
                }
            }
            else if (4 == iWindowParameter)//DATETIMEPANEL，月
            {
                if (null != WindowClose_DateTimePanel_Month)//有效
                {
                    WindowClose_DateTimePanel_Month(this, new CustomEventArgs());
                }
            }
            else if (5 == iWindowParameter)//DATETIMEPANEL，日
            {
                if (null != WindowClose_DateTimePanel_Day)//有效
                {
                    WindowClose_DateTimePanel_Day(this, new CustomEventArgs());
                }
            }
            else if (6 == iWindowParameter)//DATETIMEPANEL，时
            {
                if (null != WindowClose_DateTimePanel_Hour)//有效
                {
                    WindowClose_DateTimePanel_Hour(this, new CustomEventArgs());
                }
            }
            else if (7 == iWindowParameter)//DATETIMEPANEL，分
            {
                if (null != WindowClose_DateTimePanel_Minute)//有效
                {
                    WindowClose_DateTimePanel_Minute(this, new CustomEventArgs());
                }
            }
            else if (8 == iWindowParameter)//DATETIMEPANEL，秒
            {
                if (null != WindowClose_DateTimePanel_Second)//有效
                {
                    WindowClose_DateTimePanel_Second(this, new CustomEventArgs());
                }
            }
            else if (10 == iWindowParameter)//TOLERANCES，最大值设置
            {
                if (null != WindowClose_Tolerances_Max)//有效
                {
                    WindowClose_Tolerances_Max(this, new CustomEventArgs());
                }
            }
            else if (11 == iWindowParameter)//TOLERANCES，最小值设置
            {
                if (null != WindowClose_Tolerances_Min)//有效
                {
                    WindowClose_Tolerances_Min(this, new CustomEventArgs());
                }
            }
            else if (12 == iWindowParameter)//SHIFT CONFIGURATION，班次数
            {
                if (null != WindowClose_ShiftConfiguration_ShiftNumber)//有效
                {
                    WindowClose_ShiftConfiguration_ShiftNumber(this, new CustomEventArgs());
                }
            }
            else if (13 == iWindowParameter)//STATISTICS，REJECT IMAGE SELECTION
            {
                if (null != WindowClose_Statistics_RejectImageSelection)//有效
                {
                    WindowClose_Statistics_RejectImageSelection(this, new CustomEventArgs());
                }
            }
            else if (14 == iWindowParameter)//TOLERANCES，最小值设置
            {
                if (null != WindowClose_Tolerances_EjectLevel)//有效
                {
                    WindowClose_Tolerances_EjectLevel(this, new CustomEventArgs());
                }
            }
            else if (15 == iWindowParameter)//PARAMETER SETTINGS，参数
            {
                if (null != WindowClose_ParameterSettings_Parameter)//有效
                {
                    WindowClose_ParameterSettings_Parameter(this, new CustomEventArgs());
                }
            }
            else//其它
            {
                //不执行操作
            }
        }
    }
}