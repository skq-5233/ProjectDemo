/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：QualityCheck.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：QUALITY CHECK页面

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
    public partial class QualityCheck : Template
    {
        //QUALITY CHECK页面


        //构造函数

        //----------------------------------------------------------------------
        // 功能说明：构造函数，初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public QualityCheck()
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
        public VisionSystemControlLibrary.QualityCheckControl CustomControl//属性
        {
            get//读取
            {
                return qualityCheckControl;
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
            Global.CurrentInterface = ApplicationInterface.QualityCheck;//当前页面，QualityCheck

            //try
            {
                qualityCheckControl._Properties(Global.VisionSystem.Camera[Global.VisionSystem.Work.SelectedCameraIndex], Global.VisionSystem.Brand);//应用属性

                qualityCheckControl.MachineSpeed = Global.VisionSystem.Camera[Global.VisionSystem.Work.SelectedCameraIndex].UIParameter.Speed;
            }
            //catch (System.Exception ex)
            //{
            //    //不执行操作
            //}

            //

            Global.WorkWindow._SendCommand_Value(CommunicationInstructionType.QualityCheck_Enter, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex, qualityCheckControl.CurrentToolIndex);//发送指令

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
        private void QualityCheck_Load(object sender, EventArgs e)
        {
            //Global.CurrentInterface = ApplicationInterface.QualityCheck;//当前页面，QualityCheck

            //Global.WorkWindow._SendCommand_Value(CommunicationInstructionType.QualityCheck_Enter, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex);//发送指令
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：点击【Close】按钮事件，关闭QUALITY CHECK窗口
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void qualityCheckControl_Close_Click(object sender, EventArgs e)
        {
            try
            {
                //相机状态若发生变化（如，掉线重连），相机信息会发生变化，此时控件中的相机信息可能会与全局相机信息不匹配，造成主页面无法正确显示相机图像和状态

                VisionSystemClassLibrary.Struct.DeviceData devicedata = new VisionSystemClassLibrary.Struct.DeviceData();
                Global.VisionSystem.Camera[Global.VisionSystem.Work.SelectedCameraIndex].DeviceInformation._CopyTo(devicedata);
                
                Global.VisionSystem.Camera[Global.VisionSystem.Work.SelectedCameraIndex] = qualityCheckControl.SelectedCamera;
                devicedata._CopyTo(Global.VisionSystem.Camera[Global.VisionSystem.Work.SelectedCameraIndex].DeviceInformation);
            }
            catch (System.Exception ex)
            {
            	//不执行操作
            }

            //

            Global.WorkWindow._SetWindow();//设置窗口
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：当前选择的工具发生改变的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void qualityCheckControl_ToolChanged(object sender, EventArgs e)
        {
            //服务端->客户端：指令类型 + 相机类型数据 + 工具索引 + 图像类型（1，在线；2，学习；3，剔除）
            
            if (qualityCheckControl.LoadSampleSelected)//【LOAD SAMPLE】按下
            {
                Global.WorkWindow._SendCommand_Value(CommunicationInstructionType.QualityCheck_CurrentTool, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex, qualityCheckControl.CurrentToolIndex, 2);//发送指令
            }
            else if (qualityCheckControl.LoadRejectSelected)//【LOAD REJECT】按下
            {
                Global.WorkWindow._SendCommand_Value(CommunicationInstructionType.QualityCheck_CurrentTool, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex, qualityCheckControl.CurrentToolIndex, 3);//发送指令
            }
            else//LIVE VIEW
            {
                Global.WorkWindow._SendCommand_Value(CommunicationInstructionType.QualityCheck_CurrentTool, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex, qualityCheckControl.CurrentToolIndex, 1);//发送指令
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：工具参数值发生改变的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void qualityCheckControl_ToolParameterValueChanged(object sender, EventArgs e)
        {
            //服务端->客户端：指令类型 + 相机类型数据 + 工具索引 + 图像类型（1，在线；2，学习；3，剔除） + 工具参数

            if (qualityCheckControl.LoadSampleSelected)//【LOAD SAMPLE】按下
            {
                Global.WorkWindow._SendCommand_Value(CommunicationInstructionType.QualityCheck_ToolParamter, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex, qualityCheckControl.CurrentToolIndex, 2);//发送指令
            }
            else if (qualityCheckControl.LoadRejectSelected)//【LOAD REJECT】按下
            {
                Global.WorkWindow._SendCommand_Value(CommunicationInstructionType.QualityCheck_ToolParamter, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex, qualityCheckControl.CurrentToolIndex, 3);//发送指令
            }
            else//LIVE VIEW
            {
                Global.WorkWindow._SendCommand_Value(CommunicationInstructionType.QualityCheck_ToolParamter, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex, qualityCheckControl.CurrentToolIndex, 1);//发送指令
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：工具的兴趣区域发生改变的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void qualityCheckControl_ToolRegionChanged(object sender, EventArgs e)
        {
            //服务端->客户端：指令类型 + 相机类型数据 + 工具索引 + 图像类型（1，在线；2，学习；3，剔除） + 工作区域

            if (qualityCheckControl.LoadSampleSelected)//【LOAD SAMPLE】按下
            {
                Global.WorkWindow._SendCommand_Value(CommunicationInstructionType.QualityCheck_WorkArea, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex, qualityCheckControl.CurrentToolIndex, 2, Global.VisionSystem.Camera[Global.VisionSystem.Work.SelectedCameraIndex].Tools[qualityCheckControl.CurrentToolIndex].ROI);//发送指令
            }
            else if (qualityCheckControl.LoadRejectSelected)//【LOAD REJECT】按下
            {
                Global.WorkWindow._SendCommand_Value(CommunicationInstructionType.QualityCheck_WorkArea, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex, qualityCheckControl.CurrentToolIndex, 3, Global.VisionSystem.Camera[Global.VisionSystem.Work.SelectedCameraIndex].Tools[qualityCheckControl.CurrentToolIndex].ROI);//发送指令
            }
            else//LIVE VIEW
            {
                Global.WorkWindow._SendCommand_Value(CommunicationInstructionType.QualityCheck_WorkArea, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex, qualityCheckControl.CurrentToolIndex, 1, Global.VisionSystem.Camera[Global.VisionSystem.Work.SelectedCameraIndex].Tools[qualityCheckControl.CurrentToolIndex].ROI);//发送指令
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：点击【Save Product】按钮时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void qualityCheckControl_SaveProduct_Click(object sender, EventArgs e)
        {
            //服务端->客户端：指令类型 + 相机类型数据 + 是否保存数据（1，是；0，否）

            Global.WorkWindow._SendCommand_Value(CommunicationInstructionType.QualityCheck_SaveProduct, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex, 1);//发送指令
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【Learn Sample】按钮时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void qualityCheckControl_LearnSample_Click(object sender, EventArgs e)
        {
            //服务端->客户端：指令类型 + 相机类型数据 + 图像工具数据（1，包含工具；0，不包含工具） + 图像尺寸类型数据  + 图像类型（1，在线；2，学习；3，剔除）

            if (qualityCheckControl.LoadSampleSelected)//【LOAD SAMPLE】按下
            {
                Global.WorkWindow._SendCommand_Image_Learn(CommunicationInstructionType.QualityCheck_LearnSample, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex, 1.0, 2);//发送指令
            }
            else if (qualityCheckControl.LoadRejectSelected)//【LOAD REJECT】按下
            {
                Global.WorkWindow._SendCommand_Image_Learn(CommunicationInstructionType.QualityCheck_LearnSample, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex, 1.0, 3);//发送指令
            }
            else//LIVE VIEW
            {
                Global.WorkWindow._SendCommand_Image_Learn(CommunicationInstructionType.QualityCheck_LearnSample, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex, 1.0, 1);//发送指令
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【Live View】按钮时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void qualityCheckControl_LiveView_Click(object sender, EventArgs e)
        {
            //服务端->客户端：指令类型 + 相机类型 + 操作数据（1，打开；0，关闭）

            Global.WorkWindow._SendCommand_Value(CommunicationInstructionType.Live_SelfTrigger, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex, Convert.ToInt32(qualityCheckControl.LiveViewSelected));//发送指令
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【Load Sample】按钮时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void qualityCheckControl_LoadSample_Click(object sender, EventArgs e)
        {
            if (qualityCheckControl.LoadSampleSelected)//【LOAD SAMPLE】按下
            {
                //服务端->客户端：指令类型 + 相机类型数据 + 图像工具数据（1，包含工具；0，不包含工具） + 图像尺寸类型数据

                Global.WorkWindow._SendCommand_Image(CommunicationInstructionType.QualityCheck_LoadSample, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex, 1.0);//发送指令

            }
            else//【LOAD SAMPLE】弹起
            {
                //显示LIVE图像
                //服务端->客户端：指令类型 + 相机类型数据 + 图像工具数据（1，包含工具；0，不包含工具） + 图像尺寸类型数据

                Global.WorkWindow._SendCommand_Image(CommunicationInstructionType.QualityCheck_LiveView, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex, 1.0);//发送指令
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【Manage Tools】按钮时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void qualityCheckControl_ManageTools_Click(object sender, EventArgs e)
        {
            //服务端->客户端：指令类型 + 相机类型数据 + 启用工具标记 + 当前工具索引 + 图像类型（1，在线；2，学习；3，剔除）

            if (qualityCheckControl.LoadSampleSelected)//【LOAD SAMPLE】按下
            {
                Global.WorkWindow._SendCommand_Value(CommunicationInstructionType.QualityCheck_ManageTools, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex, qualityCheckControl.CurrentToolIndex, 2);//发送指令
            }
            else if (qualityCheckControl.LoadRejectSelected)//【LOAD REJECT】按下
            {
                Global.WorkWindow._SendCommand_Value(CommunicationInstructionType.QualityCheck_ManageTools, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex, qualityCheckControl.CurrentToolIndex, 3);//发送指令
            }
            else//LIVE VIEW
            {
                Global.WorkWindow._SendCommand_Value(CommunicationInstructionType.QualityCheck_ManageTools, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex, qualityCheckControl.CurrentToolIndex, 1);//发送指令
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【Load Reject】按钮时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void qualityCheckControl_LoadReject_Click(object sender, EventArgs e)
        {
            if (qualityCheckControl.LoadRejectSelected)//【LOAD REJECT】按下
            {
                //服务端->客户端：指令类型 + 相机类型数据 + 关联标记 + 统计数据类型（0，最新统计数据；1，指定统计数据）+ 班次索引（非0） + 统计数据开始结束时间

                Global.WorkWindow._SendCommand_Value(CommunicationInstructionType.QualityCheck_GetSelectedRecordData, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex, false, 0, 1, new VisionSystemClassLibrary.Struct.ShiftTime());//发送指令
            }
            else//【LOAD REJECT】弹起
            {
                //显示LIVE图像
                //服务端->客户端：指令类型 + 相机类型数据 + 图像工具数据（1，包含工具；0，不包含工具） + 图像尺寸类型数据

                Global.WorkWindow._SendCommand_Image(CommunicationInstructionType.QualityCheck_LiveView, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex, 1.0);//发送指令
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：按下【Load Reject】按钮，选择某一剔除图像时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void qualityCheckControl_LoadReject_ImageSelect_Click(object sender, EventArgs e)
        {
            //服务端->客户端：指令类型 + 相机类型数据 + 关联标记 + 班次索引（非0） + 统计数据开始结束时间 + 剔除图像对应的工具索引值（从0开始） + 剔除图像对应的工具中的索引值（从0开始） + 图像尺寸类型数据
            
            Global.WorkWindow._SendCommand_Value(CommunicationInstructionType.QualityCheck_LoadReject_Click, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex, Global.QualityCheckWindow.CustomControl.Relevancy, Global.QualityCheckWindow.CustomControl.SelectedShiftIndex, Global.QualityCheckWindow.CustomControl.ShiftTimeSelected, -1, Global.QualityCheckWindow.CustomControl.RejectImageIndex, 1.0);
        }
    }
}