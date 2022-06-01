/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：LiveView.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：LIVE VIEW页面

原作者：蒋涛
完成日期：2014/10/28
特别说明：无

修改者：无
完成日期：无
修改记录：无

****************************************************************/

using System;

namespace VisionSystemUserInterface
{
    public partial class LiveView : Template
    {
        //LIVE VIEW页面


        //构造函数

        //----------------------------------------------------------------------
        // 功能说明：构造函数，初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public LiveView()
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
        public VisionSystemControlLibrary.LiveControl CustomControl//属性
        {
            get//读取
            {
                return liveControl;
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
            Global.CurrentInterface = ApplicationInterface.LiveView;//当前页面，LiveView

            try
            {
                liveControl._Properties(Global.VisionSystem.Camera[Global.VisionSystem.Work.SelectedCameraIndex]);//应用属性
            }
            catch (System.Exception ex)
            {
            	//不执行操作
            }

            //

            Global.WorkWindow._SendCommand_Image(CommunicationInstructionType.Live, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex);//发送指令

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
        private void LiveView_Load(object sender, EventArgs e)
        {
            //Global.CurrentInterface = ApplicationInterface.LiveView;//当前页面，LiveView
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【SELF TRIGGER】按钮事件，打开或关闭SELF TRIGGER
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void liveControl_SelfTrigger_Click(object sender, EventArgs e)
        {
            //发送指令

            Global.WorkWindow._SendCommand_Value(CommunicationInstructionType.Live_SelfTrigger, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex, Convert.ToInt32(liveControl.SelfTrigger));//发送指令
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【Close】按钮事件，关闭LIVE VIEW窗口
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void liveControl_Close_Click(object sender, EventArgs e)
        {
            Global.WorkWindow._SetWindow();//设置窗口
        }
    }
}