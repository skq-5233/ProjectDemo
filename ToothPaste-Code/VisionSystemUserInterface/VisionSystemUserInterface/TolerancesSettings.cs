/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：TolerancesSettings.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：TOLERANCES SETTINGS页面

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
    public partial class TolerancesSettings : Template
    {
        //TOLERANCES SETTINGS页面


        //构造函数

        //----------------------------------------------------------------------
        // 功能说明：构造函数，初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public TolerancesSettings()
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
        public VisionSystemControlLibrary.TolerancesControl CustomControl//属性
        {
            get//读取
            {
                return tolerancesControl;
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
            Global.CurrentInterface = ApplicationInterface.TolerancesSettings;//当前页面，TolerancesSettings

            try
            {
                tolerancesControl._Properties(Global.VisionSystem.Camera[Global.VisionSystem.Work.SelectedCameraIndex], Global.VisionSystem.Brand);//应用属性
            }
            catch (System.Exception ex)
            {
            	//不执行操作
            }

            //

            Global.WorkWindow._SendCommand_Value(CommunicationInstructionType.TolerancesSettings_Enter, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex);//发送指令

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
        // 功能说明：窗口加载事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void TolerancesSettings_Load(object sender, EventArgs e)
        {
            //Global.CurrentInterface = ApplicationInterface.TolerancesSettings;//当前页面，TolerancesSettings

            //Global.WorkWindow._SendCommand_Value(CommunicationInstructionType.TolerancesSettings_Enter, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex);//发送指令
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【运行/停止】按钮时产生的事件，打开关闭工具
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void tolerancesControl_RunStop_Click(object sender, EventArgs e)
        {
            //服务端->客户端：指令类型 + 相机类型数据 + 工具索引数值 + 工具开关数值

            VisionSystemControlLibrary.CustomEventArgs customEventArgs = (VisionSystemControlLibrary.CustomEventArgs)e;//事件参数

            if ((customEventArgs.IntValue[0] >= 0) && (customEventArgs.IntValue[0] < tolerancesControl.TolerancesControlGraphData.Length)) //索引有效 
            {
                Global.WorkWindow._SendCommand_Value(CommunicationInstructionType.TolerancesSettings_Tool, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex, (Int32)tolerancesControl.TolerancesControlGraphData[customEventArgs.IntValue[0]].TolerancesGraph.Control_Data.TolerancesTools.ToolsSign, Convert.ToInt32(tolerancesControl.TolerancesControlGraphData[customEventArgs.IntValue[0]].TolerancesGraph.Control_Data.TolerancesTools.ToolsValue));//发送指令
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【学习】按钮时产生的事件，学习
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void tolerancesControl_Learning_Click(object sender, EventArgs e)
        {
            //服务端->客户端：指令类型 + 相机类型数据 + 工具索引数值

            VisionSystemControlLibrary.CustomEventArgs customEventArgs = (VisionSystemControlLibrary.CustomEventArgs)e;//事件参数

            if ((customEventArgs.IntValue[0] > 0) && (customEventArgs.IntValue[0] < tolerancesControl.TolerancesControlGraphData.Length)) //索引有效 
            {
                Global.WorkWindow._SendCommand_Value(CommunicationInstructionType.TolerancesSettings_Learn, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex, (Int32)tolerancesControl.TolerancesControlGraphData[customEventArgs.IntValue[0]].TolerancesGraph.Control_Data.TolerancesTools.ToolsSign);//发送指令
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：设置曲线图数据成功时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void tolerancesControl_SetGraphValueSuccess(object sender, EventArgs e)
        {
            //服务端->客户端：指令类型 + 相机类型数据 + 工具索引数值 + 最小值数值（有效值） + 最大值数值（有效值） + 最小值数值（坐标轴数值） + 最大值数值（坐标轴数值）

            VisionSystemControlLibrary.CustomEventArgs customEventArgs = (VisionSystemControlLibrary.CustomEventArgs)e;//事件参数

            if ((customEventArgs.IntValue[0] >= 0) && (customEventArgs.IntValue[0] < tolerancesControl.TolerancesControlGraphData.Length)) //索引有效 
            {
                Global.WorkWindow._SendCommand_Value(CommunicationInstructionType.TolerancesSettings_MinMax,
                                                    Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex,
                                                    (Int32)tolerancesControl.TolerancesControlGraphData[customEventArgs.IntValue[0]].TolerancesGraph.Control_Data.TolerancesTools.ToolsSign,
                                                    (Int32)tolerancesControl.TolerancesControlGraphData[customEventArgs.IntValue[0]].TolerancesGraph.Graph_YAxis.AxisEffectiveMin_Value,
                                                    (Int32)tolerancesControl.TolerancesControlGraphData[customEventArgs.IntValue[0]].TolerancesGraph.Graph_YAxis.AxisEffectiveMax_Value,
                                                    (Int32)tolerancesControl.TolerancesControlGraphData[customEventArgs.IntValue[0]].TolerancesGraph.Graph_YAxis.AxisMin_Value,
                                                    (Int32)tolerancesControl.TolerancesControlGraphData[customEventArgs.IntValue[0]].TolerancesGraph.Graph_YAxis.AxisMax_Value);//发送指令
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【RESET GRAPHS】按钮时产生的事件，复位曲线图
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void tolerancesControl_ResetGraphs_Click(object sender, EventArgs e)
        {
            //服务端->客户端：指令类型 + 相机类型数据

            Global.WorkWindow._SendCommand_Value(CommunicationInstructionType.TolerancesSettings_ResetGraphs, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex);//发送指令
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【SAVE PRODUCT】按钮时产生的事件，保存数据
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void tolerancesControl_SaveProduct_Click(object sender, EventArgs e)
        {
            //服务端->客户端：指令类型 + 相机类型数据 + 是否保存数据（1，是；0，否）

            Global.WorkWindow._SendCommand_Value(CommunicationInstructionType.TolerancesSettings_SaveProduct, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex, 1);//发送指令
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【EJECT LEVEL】按钮时产生的事件，保存数据
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void tolerancesControl_EjectLevel_Click(object sender, EventArgs e)
        {
            //服务端->客户端：指令类型 + 相机类型数据 + 灵敏度 + 调节灵敏度标记（0,：true;1：调节光电空头校准值）

            Global.WorkWindow._SendCommand_Value(CommunicationInstructionType.TolerancesSettings_EjectLevel, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex, tolerancesControl.EjectLevel, 0);//发送指令
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【Close】按钮事件，关闭TOLERANCES SETTINGS窗口
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void tolerancesControl_Close_Click(object sender, EventArgs e)
        {
            Global.WorkWindow._SetWindow();//设置窗口
        }

        //----------------------------------------------------------------------
        // 功能说明：双击按钮时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void tolerancesControl_Control_DoubleClick(object sender, EventArgs e)
        {
            //服务端->客户端：指令类型 + 相机类型数据 + 工具索引数值

            VisionSystemControlLibrary.CustomEventArgs customEventArgs = (VisionSystemControlLibrary.CustomEventArgs)e;//事件参数

            if ((customEventArgs.IntValue[0] >= 0) && (customEventArgs.IntValue[0] < tolerancesControl.TolerancesControlGraphData.Length)) //索引有效 
            {
                Global.WorkWindow._SendCommand_Value(CommunicationInstructionType.TolerancesSettings_ToolIndex, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex, (Int32)tolerancesControl.TolerancesControlGraphData[customEventArgs.IntValue[0]].TolerancesGraph.Control_Data.TolerancesTools.ToolsSign);//发送指令
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击下一页时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void tolerancesControl_NextPage_Click(object sender, EventArgs e)
        {
            //服务端->客户端：指令类型 + 相机类型数据 + 工具索引数值

            VisionSystemControlLibrary.CustomEventArgs customEventArgs = (VisionSystemControlLibrary.CustomEventArgs)e;//事件参数

            if ((customEventArgs.IntValue[0] >= 0) && (customEventArgs.IntValue[0] < tolerancesControl.TolerancesControlGraphData.Length)) //索引有效 
            {
                Global.WorkWindow._SendCommand_Value(CommunicationInstructionType.TolerancesSettings_ToolIndex, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex, (Int32)tolerancesControl.TolerancesControlGraphData[customEventArgs.IntValue[0]].TolerancesGraph.Control_Data.TolerancesTools.ToolsSign);//发送指令
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击上一页时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void tolerancesControl_PreviousPage_Click(object sender, EventArgs e)
        {
            //服务端->客户端：指令类型 + 相机类型数据 + 工具索引数值

            VisionSystemControlLibrary.CustomEventArgs customEventArgs = (VisionSystemControlLibrary.CustomEventArgs)e;//事件参数

            if ((customEventArgs.IntValue[0] >= 0) && (customEventArgs.IntValue[0] < tolerancesControl.TolerancesControlGraphData.Length)) //索引有效 
            {
                Global.WorkWindow._SendCommand_Value(CommunicationInstructionType.TolerancesSettings_ToolIndex, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex, (Int32)tolerancesControl.TolerancesControlGraphData[customEventArgs.IntValue[0]].TolerancesGraph.Control_Data.TolerancesTools.ToolsSign);//发送指令
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【UPDATE TOLERANCES】按钮时产生的事件，保存数据
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void tolerancesControl_UpdateTolerances_Click(object sender, EventArgs e)
        {
            //服务端->客户端：指令类型 + 相机类型数据 + 灵敏度 + 调节灵敏度标记（0,：true;1：调节光电空头校准值）

            Global.WorkWindow._SendCommand_Value(CommunicationInstructionType.TolerancesSettings_EjectLevel, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex, tolerancesControl.EjectLevel, 1);//发送指令
        }
    }
}