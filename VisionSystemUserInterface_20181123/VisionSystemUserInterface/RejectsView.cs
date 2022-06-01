/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：RejectsView.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：REJECTS VIEW页面

原作者：蒋涛
完成日期：2014/10/28
特别说明：无

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

using System.Threading;

using System.Diagnostics;

using System.IO;

namespace VisionSystemUserInterface
{
    public partial class RejectsView : Template
    {
        //REJECTS VIEW页面


        //构造函数

        //----------------------------------------------------------------------
        // 功能说明：构造函数，初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public RejectsView()
        {
            InitializeComponent();
        }

        //属性

        //----------------------------------------------------------------------
        // 功能说明：CustomControl属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public VisionSystemControlLibrary.RejectsControl CustomControl//属性
        {
            get//读取
            {
                return rejectsControl;
            }
        }

        //函数

        //----------------------------------------------------------------------
        // 功能说明：设置窗口
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _SetWindow()
        {
            Global.CurrentInterface = ApplicationInterface.RejectsView;//当前页面，RejectsView

            try
            {
                rejectsControl._Properties(Global.VisionSystem.Camera[Global.VisionSystem.Work.SelectedCameraIndex]);//应用属性
            }
            catch (System.Exception ex)
            {
            	//不执行操作
            }

            //

            Global.WorkWindow._SendCommand_Image(CommunicationInstructionType.Rejects_Update, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex);//发送指令

            //

            if (Global.TopMostWindows)//置顶
            {
                this.TopMost = true;//将窗口置于顶层
            }
            else//其它
            {
                this.Visible = true;//隐藏
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
        private void RejectsView_Load(object sender, EventArgs e)
        {
            //Global.CurrentInterface = ApplicationInterface.RejectsView;//当前页面，RejectsView

            //Global.WorkWindow._SendCommand_Image(CommunicationInstructionType.Rejects_Update, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex);//发送指令
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【VIEW TOOL GRAPHICS】按钮事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void rejectsControl_ViewToolGraphics_Click(object sender, EventArgs e)
        {
            //发送指令

            Global.WorkWindow._SendCommand_Image(CommunicationInstructionType.Rejects_ClickListItem, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex);//发送指令
        }

        //----------------------------------------------------------------------
        // 功能说明：备份所有图像时的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void rejectsControl_BackupAllImages_Event(object sender, EventArgs e)
        {
            //发送指令

            Global.WorkWindow._SendCommand_Image(CommunicationInstructionType.Rejects_BackupAll, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex, 1.0, rejectsControl.BackupImageIndex);//发送指令
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【CLEAR ALL】按钮事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void rejectsControl_ClearAll_Click(object sender, EventArgs e)
        {
            //发送指令

            Global.WorkWindow._SendCommand_Value(CommunicationInstructionType.Rejects_ClearAll, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex);//发送指令
        }

        //----------------------------------------------------------------------
        // 功能说明：点击列表项事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void rejectsControl_Item_Click(object sender, EventArgs e)
        {
            //发送指令

            Global.WorkWindow._SendCommand_Image(CommunicationInstructionType.Rejects_ClickListItem, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex);//发送指令
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【Close】按钮事件，关闭REJECTS VIEW窗口
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void rejectsControl_Close_Click(object sender, EventArgs e)
        {
            Global.WorkWindow._SetWindow();//设置窗口
        }
    }
}