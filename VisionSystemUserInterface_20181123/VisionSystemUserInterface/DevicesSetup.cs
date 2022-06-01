/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：DevicesSetup.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：DEVICES SETUP页面

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

using System.Net;
using System.Net.Sockets;

using System.Threading;

using System.Diagnostics;

using System.IO;

namespace VisionSystemUserInterface
{
    public partial class DevicesSetup : Template
    {
        //DEVICES SETUP页面

        public static Byte ConfigDeviceNumber = 0;//CONFIG DEVICE，待配置的相机数量

        //构造函数

        //----------------------------------------------------------------------
        // 功能说明：构造函数，初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public DevicesSetup()
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
        public VisionSystemControlLibrary.DeviceControl CustomControl//属性
        {
            get//读取
            {
                return deviceControl;
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
            Global.CurrentInterface = ApplicationInterface.DevicesSetup;//当前页面，DevicesSetup

            deviceControl._Properties(Global.VisionSystem);//应用属性

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

        //

        //事件

        //----------------------------------------------------------------------
        // 功能说明：控件加载事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void DevicesSetup_Load(object sender, EventArgs e)
        {
            //Global.CurrentInterface = ApplicationInterface.DevicesSetup;//当前页面，DevicesSetup

            if (null != Global.VisionSystem.Camera)
            {
                Global.DevicesSetupWindow.CustomControl.FaultExist = new Boolean[Global.VisionSystem.Camera.Length];
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：点击【REFRESH LIST】按钮事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void deviceControl_RefreshList_Click(object sender, EventArgs e)
        {
            //发送指令

            //DEVICES SETUP页面操作，REFRESH LIST，格式：    
            //服务端->客户端：_RequestClientDeviceInformation();

            try
            {
                for (int i = 0; i < Global.VisionSystem.Camera.Length; i++)
                {
                    Byte[] DevicesSetupRefreshList_ClientIP = IPAddress.Parse(Global.VisionSystem.Camera[i].DeviceInformation.IP).GetAddressBytes();//获取当前选中的相机的IP地址

                    Global.WorkWindow._RequestClientDeviceInformation(Global.NetServer.ClientData[DevicesSetupRefreshList_ClientIP[3]]);//发送数据
                }
            }
            catch (System.Exception ex)
            {
                //不执行操作
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【RESET DEVICE】按钮事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void deviceControl_ResetDevice_Click(object sender, EventArgs e)
        {
            //发送指令

            try
            {
                Global.WorkWindow._SendCommand_Value(CommunicationInstructionType.DevicesSetup_ResetDevice, Global.DevicesSetupWindow.CustomControl.CameraSelected, Global.WorkWindow._GetSelectedCameraIndex(Global.DevicesSetupWindow.CustomControl.CameraSelected));//发送指令
            }
            catch (System.Exception ex)
            {
                //不执行操作
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【CONFIG DEVICE】按钮事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void deviceControl_ConfigDevice_Click(object sender, EventArgs e)
        {
            //发送指令

            Int32 iCameraIndex = Global.WorkWindow._GetSelectedCameraIndex(Global.DevicesSetupWindow.CustomControl.CameraSelected);//相机索引值

            Int32 i = 0;//循环控制变量
            Int32 j = 0;//循环控制变量
            Int32 k = 0;//循环控制变量
            String sValue = "";//临时变量

            ConfigDeviceNumber = 0;//初始化
            Int32 iCameraChooseState = 0;
            for (i = 0; i < Global.VisionSystem.Camera.Length; i++)
            {
                if (Global.DevicesSetupWindow.CustomControl.ControllerConfigDevice == Global.VisionSystem.Camera[i].ControllerENGName)//有效
                {
                    ConfigDeviceNumber++;
                    iCameraChooseState |= (0x01 << (Global.VisionSystem.Camera[i].DeviceInformation.Port - 1));
                }
            }

            for (j = 0; j < Global.VisionSystem.Camera.Length; j++)
            {
                if (Global.DevicesSetupWindow.CustomControl.CameraSelected == Global.VisionSystem.Camera[j].Type)//有效
                {
                    sValue = Global.VisionSystem.Camera[j].ControllerENGName;

                    break;
                }
            }

            k = 0;
            for (i = 0; i < Global.VisionSystem.Camera.Length; i++)
            {
                if (Global.DevicesSetupWindow.CustomControl.ControllerConfigDevice == Global.VisionSystem.Camera[i].ControllerENGName)//有效
                {
                    for (j = k; j < Global.VisionSystem.Camera.Length; j++)
                    {
                        if (sValue == Global.VisionSystem.Camera[j].ControllerENGName)
                        {
                            Global.WorkWindow._SendCommand_Value(CommunicationInstructionType.DevicesSetup_ConfigDevice, Global.VisionSystem.Camera[j].Type, iCameraIndex, Global.WorkWindow._GetCameraConfigurationIPAddress(Global.VisionSystem.Camera[i].Type), Global.VisionSystem.Camera[i].Type, Convert.ToByte(Global.VisionSystem.Camera[i].DeviceInformation.Port - 1), iCameraChooseState, Global.VisionSystem.Camera[i].CameraFaultState, Global.VisionSystem.Camera[i].CheckEnable, Global.VisionSystem.Camera[i].CameraAngle, Global.VisionSystem.Camera[i].VideoColor, Global.VisionSystem.Camera[i].VideoResolution, Global.VisionSystem.Camera[i].IsSerialPort, Global.VisionSystem.Camera[i].TobaccoSortType_E, Global.VisionSystem.Camera[i].BitmapLockBitsResize, Global.VisionSystem.Camera[i].BitmapLockBitsCenter, Global.VisionSystem.Camera[i].BitmapLockBitsAxis, Global.VisionSystem.Camera[i].BitmapLockBitsArea, Global.VisionSystem.Camera[i].Rejects.ImageNumberTotal, Global.VisionSystem.Camera[i].CameraFlip, Global.VisionSystem.Brand.BrandPath + Global.VisionSystem.Brand.CURRENTBrandName + "\\" + Global.WorkWindow._GetSystemCameraName(Global.VisionSystem.Camera[i].Type) + "\\", VisionSystemClassLibrary.Class.Camera.TolerancesFileName, 0);//发送指令

                            k = j;
                            break;
                        }
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【PARAMETER SETTINGS】按钮事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void deviceControl_ParameterSettings_Save(object sender, EventArgs e)
        {
            Global.WorkWindow._SendCommand_Value
            (
            CommunicationInstructionType.DevicesSetup_ParameterSettings, 
            Global.DevicesSetupWindow.CustomControl.CameraSelected, 
            Global.WorkWindow._GetSelectedCameraIndex(Global.DevicesSetupWindow.CustomControl.CameraSelected)
            );//发送指令
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【ALIGN DATE/TIME】按钮事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void deviceControl_AlignDateTime_Click(object sender, EventArgs e)
        {
            //发送指令

            Global.WorkWindow._SendCommand_Value(CommunicationInstructionType.DevicesSetup_AlignDateTime, Global.DevicesSetupWindow.CustomControl.CameraSelected, Global.WorkWindow._GetSelectedCameraIndex(Global.DevicesSetupWindow.CustomControl.CameraSelected));//发送指令
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【Close】按钮事件，关闭TOLERANCES SETTINGS窗口
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void deviceControl_Close_Click(object sender, EventArgs e)
        {
            Global.WorkWindow._SetWindow();//设置窗口
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：点击【CONFIG IMAGE】按钮事件，打开IMAGE CONFIGURATION窗口
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void deviceControl_ConfigImage_Click(object sender, EventArgs e)
        {
            Global.ImageConfigurationWindow._SetWindow();//设置窗口
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：点击【TEST I/O】按钮事件，打开TEST I/O窗口
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void deviceControl_TestIO_Click(object sender, EventArgs e)
        {
            Global.WorkWindow._SendCommand_Value(CommunicationInstructionType.DevicesSetup_TestIOEnter, Global.DevicesSetupWindow.CustomControl.CameraSelected, Global.WorkWindow._GetSelectedCameraIndex(Global.DevicesSetupWindow.CustomControl.CameraSelected));//发送指令
        }

        //----------------------------------------------------------------------
        // 功能说明：关闭TEST I/O窗口时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void deviceControl_TestIO_Close_Click(object sender, EventArgs e)
        {
            Global.WorkWindow._SendCommand_Value(CommunicationInstructionType.DevicesSetup_TestIOExit, Global.DevicesSetupWindow.CustomControl.CameraSelected, Global.WorkWindow._GetSelectedCameraIndex(Global.DevicesSetupWindow.CustomControl.CameraSelected));//发送指令
        }
    }
}