/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：FaultMessageWindow.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：故障信息窗口

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
    public partial class FaultMessageWindow : Form
    {
        //故障信息窗口

        [Browsable(true), Description("清除所有故障信息时产生的事件"), Category("FaultMessageWindow 事件")]
        public event EventHandler ClearAllFaultMessages;//清除所有故障信息时产生的事件

        [Browsable(true), Description("窗口关闭时产生的事件"), Category("FaultMessageWindow 事件")]
        public event EventHandler WindowClose;//窗口关闭时产生的事件

        //构造函数

        //----------------------------------------------------------------------
        // 功能说明：构造函数，初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public FaultMessageWindow()
        {
            InitializeComponent();
        }

        //属性

        //----------------------------------------------------------------------
        // 功能说明：FaultMessageControl属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("控件"), Category("FaultMessageWindow 通用")]
        public FaultMessage FaultMessageControl//属性
        {
            get//读取
            {
                return faultMessage;
            }
        }

        //事件

        //-----------------------------------------------------------------------
        // 功能说明：点击【OK】时产生的事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void faultMessage_Close_Click(object sender, EventArgs e)
        {
            //事件

            if (null != WindowClose)//有效
            {
                WindowClose(this, new CustomEventArgs());
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：点击【CLEAR ALL】时产生的事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void faultMessage_ClearAllFaultMessages(object sender, EventArgs e)
        {
            //事件

            if (null != ClearAllFaultMessages)//有效
            {
                ClearAllFaultMessages(this, new CustomEventArgs());
            }
        }
    }
}
