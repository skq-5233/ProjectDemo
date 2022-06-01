/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：ImageConfiguration.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：DEVICES SETUP，Image Configuration页面

原作者：视觉检测团队
完成日期：2014/08/18
特别说明：经视觉检测团队授权并遵守代码使用条款，方可使用本代码并获得技术支持，否则所产生的一切后果由使用者承担

修改者：无
完成日期：无
修改记录：无

****************************************************************/

using System;

namespace VisionSystemUserInterface
{
    public partial class ImageConfiguration : Template
    {
        //DEVICES SETUP，IMAGE CONFIGURATION页面

        public Boolean WindowDisplay = false;//是否显示窗口。取值范围：true，是；false，否

        //构造函数

        //----------------------------------------------------------------------
        // 功能说明：构造函数，初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public ImageConfiguration()
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
        public VisionSystemControlLibrary.ImageConfigurationControl CustomControl//属性
        {
            get//读取
            {
                return imageConfigurationControl;
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
            WindowDisplay = true;//是否显示窗口。取值范围：true，是；false，否

            try
            {
                imageConfigurationControl._Properties(Global.VisionSystem.Camera[Global.WorkWindow._GetSelectedCameraIndex(Global.DevicesSetupWindow.CustomControl.CameraSelected)], Global.VisionSystem.Brand);//设置属性
            }
            catch (System.Exception ex)
            {
            	//不执行操作
            }

            //

            Global.WorkWindow._SendCommand_Value(CommunicationInstructionType.DevicesSetup_ConfigImage_Enter, Global.DevicesSetupWindow.CustomControl.CameraSelected, Global.WorkWindow._GetSelectedCameraIndex(Global.DevicesSetupWindow.CustomControl.CameraSelected));//发送指令

            //

            if (Global.TopMostWindows)//置顶
            {
                this.TopMost = true;//将窗口置于顶层
            }
            else//其它
            {
                this.Visible = true;//显示
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
        private void ImageConfiguration_Load(object sender, EventArgs e)
        {
            //WindowDisplay = true;//是否显示窗口。取值范围：true，是；false，否

            //Global.WorkWindow._SendCommand_Value(CommunicationInstructionType.DevicesSetup_ConfigImage_Enter, Global.DevicesSetupWindow.CustomControl.CameraSelected, Global.WorkWindow._GetSelectedCameraIndex(Global.DevicesSetupWindow.CustomControl.CameraSelected));//发送指令
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：点击【Save Product】按钮时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void imageConfigurationControl_SaveProduct_Click(object sender, EventArgs e)
        {
            //服务端->客户端：指令类型 + 相机类型数据 + 是否保存数据（1，是；0，否）

            Int32 iDataIndex = Global.WorkWindow._GetSelectedCameraIndex(Global.DevicesSetupWindow.CustomControl.CameraSelected);

            Global.WorkWindow._SendCommand_Value(CommunicationInstructionType.DevicesSetup_ConfigImage_Save, Global.DevicesSetupWindow.CustomControl.CameraSelected, iDataIndex, 1);//发送指令
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【Focus Calibration】按钮时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void imageConfigurationControl_FocusCalibration_Click(object sender, EventArgs e)
        {
            //服务端->客户端：指令类型 + 相机类型数据 + 聚焦参数
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【White Balance】按钮时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void imageConfigurationControl_WhiteBalance_Click(object sender, EventArgs e)
        {
            //服务端->客户端：指令类型 + 相机类型数据 + 白平衡参数
        }

        //----------------------------------------------------------------------
        // 功能说明：参数值发生改变的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void imageConfigurationControl_ParameterValueChanged(object sender, EventArgs e)
        {
            //服务端->客户端：指令类型 + 相机类型数据 + 光照时间 + 光照强度 + 增益 + 曝光时间 + 白平衡 + 白平衡（红） + 白平衡（绿） + 白平衡（蓝）

            if (false == Global.VisionSystem.Camera[Global.WorkWindow._GetSelectedCameraIndex(Global.DevicesSetupWindow.CustomControl.CameraSelected)].IsSerialPort) //当前为相机
            {
                Global.WorkWindow._SendCommand_Value
                (
                    CommunicationInstructionType.DevicesSetup_ConfigImage_Parameter,
                    Global.DevicesSetupWindow.CustomControl.CameraSelected,
                    Global.WorkWindow._GetSelectedCameraIndex(Global.DevicesSetupWindow.CustomControl.CameraSelected),
                    Global.VisionSystem.Camera[Global.WorkWindow._GetSelectedCameraIndex(Global.DevicesSetupWindow.CustomControl.CameraSelected)].DeviceParameter.StroboTime,
                    Global.VisionSystem.Camera[Global.WorkWindow._GetSelectedCameraIndex(Global.DevicesSetupWindow.CustomControl.CameraSelected)].DeviceParameter.StroboCurrent,
                    Global.VisionSystem.Camera[Global.WorkWindow._GetSelectedCameraIndex(Global.DevicesSetupWindow.CustomControl.CameraSelected)].DeviceParameter.Gain,
                    Global.VisionSystem.Camera[Global.WorkWindow._GetSelectedCameraIndex(Global.DevicesSetupWindow.CustomControl.CameraSelected)].DeviceParameter.ExposureTime,
                    Global.VisionSystem.Camera[Global.WorkWindow._GetSelectedCameraIndex(Global.DevicesSetupWindow.CustomControl.CameraSelected)].DeviceParameter.WhiteBalance,
                    Global.VisionSystem.Camera[Global.WorkWindow._GetSelectedCameraIndex(Global.DevicesSetupWindow.CustomControl.CameraSelected)].DeviceParameter.WhiteBalance_Red,
                    Global.VisionSystem.Camera[Global.WorkWindow._GetSelectedCameraIndex(Global.DevicesSetupWindow.CustomControl.CameraSelected)].DeviceParameter.WhiteBalance_Green,
                    Global.VisionSystem.Camera[Global.WorkWindow._GetSelectedCameraIndex(Global.DevicesSetupWindow.CustomControl.CameraSelected)].DeviceParameter.WhiteBalance_Blue
                );//发送指令
            }
            else //当前为串口
            {
                Global.WorkWindow._SendCommand_Value
                (
                    CommunicationInstructionType.DevicesSetup_ConfigSensor_Parameter,
                    Global.DevicesSetupWindow.CustomControl.CameraSelected,
                    Global.WorkWindow._GetSelectedCameraIndex(Global.DevicesSetupWindow.CustomControl.CameraSelected),
                    Global.VisionSystem.Camera[Global.WorkWindow._GetSelectedCameraIndex(Global.DevicesSetupWindow.CustomControl.CameraSelected)].SensorNumber,
                    Global.VisionSystem.Camera[Global.WorkWindow._GetSelectedCameraIndex(Global.DevicesSetupWindow.CustomControl.CameraSelected)].DeviceParameter.SensorSelectState,
                    Global.ImageConfigurationWindow.CustomControl.SensorAdjustState,      
                    Global.VisionSystem.Camera[Global.WorkWindow._GetSelectedCameraIndex(Global.DevicesSetupWindow.CustomControl.CameraSelected)].DeviceParameter.SensorAdjustValue
                );//发送指令
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【Close】按钮时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void imageConfigurationControl_Close_Click(object sender, EventArgs e)
        {
            try
            {
                //相机状态若发生变化（如，掉线重连），相机信息会发生变化，此时控件中的相机信息可能会与全局相机信息不匹配，造成主页面无法正确显示相机图像和状态

                VisionSystemClassLibrary.Struct.DeviceData devicedata = new VisionSystemClassLibrary.Struct.DeviceData();
                Global.VisionSystem.Camera[Global.WorkWindow._GetSelectedCameraIndex(Global.DevicesSetupWindow.CustomControl.CameraSelected)].DeviceInformation._CopyTo(devicedata);
                Boolean bCameraSelected = Global.VisionSystem.Camera[Global.WorkWindow._GetSelectedCameraIndex(Global.DevicesSetupWindow.CustomControl.CameraSelected)].Live.CameraSelected;

                Global.VisionSystem.Camera[Global.WorkWindow._GetSelectedCameraIndex(Global.DevicesSetupWindow.CustomControl.CameraSelected)] = imageConfigurationControl.SelectedCamera;
                devicedata._CopyTo(Global.VisionSystem.Camera[Global.WorkWindow._GetSelectedCameraIndex(Global.DevicesSetupWindow.CustomControl.CameraSelected)].DeviceInformation);
                Global.VisionSystem.Camera[Global.WorkWindow._GetSelectedCameraIndex(Global.DevicesSetupWindow.CustomControl.CameraSelected)].Live.CameraSelected = bCameraSelected;
            }
            catch (System.Exception ex)
            {
            	//不执行操作
            }

            //

            WindowDisplay = false;//是否显示窗口。取值范围：true，是；false，否

            //

            Global.WorkWindow._SendCommand_Value(CommunicationInstructionType.DevicesSetup_ConfigImage_Save, Global.DevicesSetupWindow.CustomControl.CameraSelected, Global.WorkWindow._GetSelectedCameraIndex(Global.DevicesSetupWindow.CustomControl.CameraSelected), 0);//发送指令

            //

            if (Global.TopMostWindows)//置顶
            {
                Global.DevicesSetupWindow.TopMost = true;//将窗口置于顶层
            }
            else//其它
            {
                this.Visible = false;//隐藏
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：参数值发生改变的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void imageConfigurationControl_Start_at_LowSpeed_Click(object sender, EventArgs e)
        {

        }
    }
}