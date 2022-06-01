/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：StatisticsView.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：STATISTICS页面

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
    public partial class StatisticsView : Template
    {
        //STATISTICS页面



        //构造函数

        //----------------------------------------------------------------------
        // 功能说明：构造函数，初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public StatisticsView()
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
        public VisionSystemControlLibrary.StatisticsControl CustomControl//属性
        {
            get//读取
            {
                return statisticsControl;
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
            Global.CurrentInterface = ApplicationInterface.StatisticsView;//当前页面，STATISTICS VIEW

            try
            {
                Global.VisionSystem.Shift.DataOfShift.CurrentStatisticsInformation = new VisionSystemClassLibrary.Struct.StatisticsInformation();
                Global.VisionSystem.Shift.DataOfShift.CurrentStatisticsInformation.TimeData = new VisionSystemClassLibrary.Struct.ShiftTime[1];
                Global.VisionSystem.Shift.DataOfShift.CurrentStatisticsInformation.DataOfStatistics = new VisionSystemClassLibrary.Struct.StatisticsData[1] ;
                Global.VisionSystem.Shift.DataOfShift.CurrentStatisticsInformation.DataOfStatistics[0].CameraStatisticsData = new VisionSystemClassLibrary.Struct.StatisticsData_Camera[1];
                Global.VisionSystem.Shift.DataOfShift.CurrentStatisticsInformation.DataOfStatistics[0].CameraStatisticsData[0].RejectedStatistics_Tool = new UInt32[Global.VisionSystem.Camera[Global.VisionSystem.Work.SelectedCameraIndex].Tools.Count];

                //

                statisticsControl._Properties(Global.VisionSystem, Global.VisionSystem.Camera[Global.VisionSystem.Work.SelectedCameraIndex], Global.VisionSystem.Shift);//应用属性
            }
            catch (System.Exception ex)
            {
                //不执行操作
            }

            //

            if (Global.TopMostWindows)//置顶
            {
                this.TopMost = true;//将窗口置于顶层
            }
            else//其它
            {
                this.Visible = true;//隐藏
            }

            //

            statisticsControl._StartGetStatisticsData();

            //

            Global.WorkWindow._SendCommand_Value(CommunicationInstructionType.Statistics_GetSelectedRecordData, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex, Global.StatisticsViewWindow.CustomControl.Relevancy, 0, 0 + 1, new VisionSystemClassLibrary.Struct.ShiftTime());//发送指令
        }

        //事件

        //----------------------------------------------------------------------
        // 功能说明：控件加载事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void StatisticsView_Load(object sender, EventArgs e)
        {
            //不执行操作
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：点击【Close】按钮事件，关闭STATISTICS VIEW窗口
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void statisticsControl_Close_Click(object sender, EventArgs e)
        {
            Global.WorkWindow._SetWindow();//设置属性
        }

        //----------------------------------------------------------------------
        // 功能说明：点击工具列表事件，查看剔除图像
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void statisticsControl_ViewRejectImage(object sender, EventArgs e)
        {
            Global.WorkWindow._SendCommand_Value(CommunicationInstructionType.Statistics_ClickRejectsListItem, statisticsControl.ViewRejectImage_CameraSelected, Global.WorkWindow._GetSelectedCameraIndex(statisticsControl.ViewRejectImage_CameraSelected), Global.StatisticsViewWindow.CustomControl.Relevancy, Global.VisionSystem.Shift.DataOfShift.CurrentIndex + 1, Global.VisionSystem.Shift.DataOfShift.CurrentStatisticsInformation.TimeData[0], statisticsControl.CurrentToolIndex_RejectImage, statisticsControl.CurrentRejectImageIndex_Tool, 1.0);//发送指令
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【RECORDS】按钮事件，获取RECORDS页面统计数据列表
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void statisticsControl_GetRecords(object sender, EventArgs e)
        {
            Global.WorkWindow._SendCommand_Value(CommunicationInstructionType.Statistics_GetRecordList, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex);//发送指令
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【DELETE】（【DELETE ALL】）按钮事件，删除统计数据
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void statisticsControl_DeleteRecords(object sender, EventArgs e)
        {
            Int32 iValue = VisionSystemControlLibrary.GlobalWindows.StatisticsRecord_Window.StatisticsRecordControl.DeleteType;

            if (0 == iValue)//删除所有
            {
                Global.WorkWindow._SendCommand_Value(CommunicationInstructionType.Statistics_DeleteRecord, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex, iValue);//发送指令
            }
            else if (1 == iValue)//删除指定班次
            {
                Int32 iShiftNumber = 1;
                Int32[] iShiftIndex = new Int32[1];
                iShiftIndex[0] = VisionSystemControlLibrary.GlobalWindows.StatisticsRecord_Window.StatisticsRecordControl.SelectedShiftIndex + 1;

                Global.WorkWindow._SendCommand_Value(CommunicationInstructionType.Statistics_DeleteRecord, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex, iValue, iShiftNumber, iShiftIndex);//发送指令
            }
            else//2，删除指定记录
            {
                Int32 iShiftIndex = VisionSystemControlLibrary.GlobalWindows.StatisticsRecord_Window.StatisticsRecordControl.SelectedShiftIndex + 1;
                Int32 iRecordNumber = 1;

                VisionSystemClassLibrary.Struct.ShiftTime[] shifttime = new VisionSystemClassLibrary.Struct.ShiftTime[iRecordNumber];
                shifttime[0] = VisionSystemControlLibrary.GlobalWindows.StatisticsRecord_Window.StatisticsRecordControl.ShiftTimeSelectedRecord;

                Global.WorkWindow._SendCommand_Value(CommunicationInstructionType.Statistics_DeleteRecord, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex, iValue, iShiftIndex, iRecordNumber, shifttime);//发送指令
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：STATISTICS RECORD，选择新的统计记录数据事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void statisticsControl_GetRecordData(object sender, EventArgs e)
        {
            //if (VisionSystemControlLibrary.GlobalWindows.StatisticsRecord_Window.StatisticsRecordControl.SelectNewRecord)//选择了新的统计记录
            //{
            try
            {
                Global.VisionSystem.Shift.DataOfShift.CurrentStatisticsInformation = new VisionSystemClassLibrary.Struct.StatisticsInformation();
                Global.VisionSystem.Shift.DataOfShift.CurrentStatisticsInformation.TimeData = new VisionSystemClassLibrary.Struct.ShiftTime[1];
                Global.VisionSystem.Shift.DataOfShift.CurrentStatisticsInformation.DataOfStatistics = new VisionSystemClassLibrary.Struct.StatisticsData[1];
                Global.VisionSystem.Shift.DataOfShift.CurrentStatisticsInformation.DataOfStatistics[0].CameraStatisticsData = new VisionSystemClassLibrary.Struct.StatisticsData_Camera[1];
                Global.VisionSystem.Shift.DataOfShift.CurrentStatisticsInformation.DataOfStatistics[0].CameraStatisticsData[0].RejectedStatistics_Tool = new UInt32[Global.VisionSystem.Camera[Global.VisionSystem.Work.SelectedCameraIndex].Tools.Count];
            }
            catch (System.Exception ex)
            {
                //不执行操作
            }

            statisticsControl._StartGetStatisticsData();

            //

            Global.WorkWindow._SendCommand_Value(CommunicationInstructionType.Statistics_GetSelectedRecordData, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex, Global.StatisticsViewWindow.CustomControl.Relevancy, 1, VisionSystemControlLibrary.GlobalWindows.StatisticsRecord_Window.StatisticsRecordControl.SelectedShiftIndex + 1, VisionSystemControlLibrary.GlobalWindows.StatisticsRecord_Window.StatisticsRecordControl.ShiftTimeSelectedRecordTemp);//发送指令
            //}
        }

        //----------------------------------------------------------------------
        // 功能说明：选择关联/非关联统计记录数据事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void statisticsControl_Relevancy_Click(object sender, EventArgs e)
        {
            if (Global.StatisticsViewWindow.CustomControl.IsCurrentShift) //当前班次
            {
                Global.WorkWindow._SendCommand_Value(CommunicationInstructionType.Statistics_GetSelectedRecordData, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex, Global.StatisticsViewWindow.CustomControl.Relevancy, 0, 0 + 1, new VisionSystemClassLibrary.Struct.ShiftTime());//发送指令
            }
            else
            {
                Global.WorkWindow._SendCommand_Value(CommunicationInstructionType.Statistics_GetSelectedRecordData, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex, Global.StatisticsViewWindow.CustomControl.Relevancy, 1, VisionSystemControlLibrary.GlobalWindows.StatisticsRecord_Window.StatisticsRecordControl.SelectedShiftIndex + 1, VisionSystemControlLibrary.GlobalWindows.StatisticsRecord_Window.StatisticsRecordControl.ShiftTimeSelectedRecordTemp);//发送指令
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击工具列表事件，查看关联剔除图像
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void statisticsControl_ViewRejectImage_Relevancy(object sender, EventArgs e)
        {
            Global.WorkWindow._SendCommand_Value(CommunicationInstructionType.Statistics_ClickRejectsListItem, statisticsControl.ViewRejectImage_CameraSelected, Global.WorkWindow._GetSelectedCameraIndex(statisticsControl.ViewRejectImage_CameraSelected), Global.StatisticsViewWindow.CustomControl.Relevancy, Global.VisionSystem.Shift.DataOfShift.CurrentIndex + 1, Global.VisionSystem.Shift.DataOfShift.CurrentStatisticsInformation.TimeData[0], statisticsControl.CurrentToolIndex_RejectImage, statisticsControl.CurrentRejectImageIndex_Tool, 1.0);//发送指令
        }
    }
}