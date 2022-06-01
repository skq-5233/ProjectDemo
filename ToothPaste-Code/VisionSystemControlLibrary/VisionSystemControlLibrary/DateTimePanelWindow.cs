/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：DateTimePanelWindow.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：日期时间面板窗口

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
    public partial class DateTimePanelWindow : Form
    {
        //日期时间面板窗口

        private Int32 iWindowParameter = 0;//属性，窗口特征数值，表示调用窗口的父级窗口类型，以便产生相应的事件。取值范围：
        //1.SYSTEM，日期时间修改
        //2.SHIFT CONFIGURATION，班次设置
        //3.STATISTICS SEARCH，统计数据查找

        //

        [Browsable(true), Description("SYSTEM，日期时间修改，窗口关闭时产生的事件"), Category("DateTimePanelWindow 事件")]
        public event EventHandler WindowClose_System_DateTime;//SYSTEM，日期时间修改，窗口关闭时产生的事件

        [Browsable(true), Description("SHIFT CONFIGURATION，班次设置，窗口关闭时产生的事件"), Category("DateTimePanelWindow 事件")]
        public event EventHandler WindowClose_ShiftConfiguration;//SHIFT CONFIGURATION，班次设置，窗口关闭时产生的事件

        [Browsable(true), Description("STATISTICS SEARCH，统计数据查找，窗口关闭时产生的事件"), Category("DateTimePanelWindow 事件")]
        public event EventHandler WindowClose_StatisticsSearch;//STATISTICS SEARCH，统计数据查找，窗口关闭时产生的事件

        //构造函数

        //----------------------------------------------------------------------
        // 功能说明：构造函数，初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public DateTimePanelWindow()
        {
            InitializeComponent();
        }

        //属性

        //----------------------------------------------------------------------
        // 功能说明：DateTimePanelControl属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("控件"), Category("DateTimePanelWindow 通用")]
        public DateTimePanel DateTimePanelControl//属性
        {
            get//读取
            {
                return dateTimePanel;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：WindowParameter属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("窗口特征数值，表示调用窗口的父级窗口类型，以便产生相应的事件"), Category("DateTimePanelWindow 通用")]
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
        // 功能说明：点击【OK】或【CANCEL】时产生的事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void dateTimePanel_Close_Click(object sender, EventArgs e)
        {
            if (1 == iWindowParameter)//SYSTEM，日期时间修改
            {
                if (null != WindowClose_System_DateTime)//有效
                {
                    WindowClose_System_DateTime(this, new CustomEventArgs());
                }
            }
            else if (2 == iWindowParameter)//SHIFT CONFIGURATION，班次设置
            {
                if (null != WindowClose_ShiftConfiguration)//有效
                {
                    WindowClose_ShiftConfiguration(this, new CustomEventArgs());
                }
            }
            else if (3 == iWindowParameter)//STATISTICS SEARCH，统计数据查找
            {
                if (null != WindowClose_StatisticsSearch)//有效
                {
                    WindowClose_StatisticsSearch(this, new CustomEventArgs());
                }
            }
            else//其它
            {
                //不执行操作
            }
        }
    }
}
