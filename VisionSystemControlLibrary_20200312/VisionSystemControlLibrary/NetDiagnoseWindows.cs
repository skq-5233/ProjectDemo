/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：NetCheckWindow.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：网络状态查询窗口

原作者：视觉检测团队
完成日期：2019/12/31 
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
    public partial class NetDiagnoseWindows : Form
    {
        [Browsable(true), Description("Ping产生的事件"), Category("NetDiagnose 事件")]
        public event EventHandler Ping_Click;//窗口关闭时产生的事件

        [Browsable(true), Description("Connect产生的事件"), Category("NetDiagnose 事件")]
        public event EventHandler Connect_Click;//窗口关闭时产生的事件

        [Browsable(true), Description("窗口关闭时产生的事件"), Category("NetDiagnose 事件")]
        public event EventHandler WindowClose;//窗口关闭时产生的事件

        public NetDiagnoseWindows()
        {
            InitializeComponent();
        }

        //属性

        //----------------------------------------------------------------------
        // 功能说明：NetDiagnoseControl属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("控件"), Category("NetDiagnoseWindows 通用")]
        public NetDiagnose NetDiagnoseControl//属性
        {
            get//读取
            {
                return netDiagnose1;
            }
        }

        //事件

        //----------------------------------------------------------------------
        // 功能说明：【Connect】按钮点击事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void netDiagnose1_CameraConnect_Click(object sender, EventArgs e)
        {
            //事件

            if (null != Connect_Click)//有效
            {
                Connect_Click(this, new CustomEventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：【Ping】按钮点击事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void netDiagnose1_ControllerConnect_Click(object sender, EventArgs e)
        {
            //事件

            if (null != Ping_Click)//有效
            {
                Ping_Click(this, new CustomEventArgs());
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void netDiagnose1_Close_Click(object sender, EventArgs e)
        {
            //事件

            if (null != WindowClose)//有效
            {
                WindowClose(this, new CustomEventArgs());
            }
        }
    }
}
