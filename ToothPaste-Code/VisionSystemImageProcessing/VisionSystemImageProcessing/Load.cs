/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：Load.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：LOAD页面

原作者：金怀国
完成日期：2014/10/28
特别说明：无

修改者：无
完成日期：无
修改记录：无

****************************************************************/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;

using TIS.Imaging;
using System.Threading;
using System.Diagnostics;

using System.Management;

using System.IO.Ports;
using Emgu.CV;

namespace VisionSystemImageProcessing
{
    public partial class Load : Form
    {
        private Byte CameraFlag;                       　　　　　　　　　　    //当前相机重复上电次数
        private Byte DelayCount;                       　　　　　　　　　　    //当前相机上电延时计数

        private Byte SerialPortDelayCount;                       　　　　　　　//当前串口查询计数

        private Boolean ControllerCommunicationFlag;                   　　　  //控制器串口状态标志
        private Object LockControllerSerialPort;                   　　　　　  //防止控制器串口发送数据冲突
        private SerialPort ControllerSerialPortCommucation;                    //动态创建控制器串口

        private Boolean[] CommunicationFlag;                   　　　          //串口状态标志
        private Object[] LockSerialPort;                             　　　　　//防止串口发送数据冲突
        private SerialPort[] SerialPortCommucation;                            //动态创建串口

        private Byte Timer1State;                                              //定时器标记

        private Byte Timer3State;                                              //定时器标记

        private List<String> SerialPortNames;                                   //查询串口名称数组

        private Byte PowerOnCameraIndex;                                       //已上电相机索引

        private List<Int32> CameraTestTrue;                                     //测试成功相机索引标记

        private List<String> serialPortName;

        public Load()
        {
            InitializeComponent();

            CameraFlag = 0;
            DelayCount = 0;

            Timer1State = 0;

            Timer3State = 0;

            SerialPortDelayCount = 0;

            SerialPortNames = new List<String>();

            CameraTestTrue = new List<Int32>();

            LockControllerSerialPort = new object();

            serialPortName = new List<String>();
        }

        //-----------------------------------------------------------------------
        // 功能说明：窗口初始化加载函数
        // 输入参数： 1、object：sender，timer控件对象
        //            2、EventArgs：e，timer控件事件响应参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void Load_Load(object sender, EventArgs e)
        {
            label1.Visible = false;

            Timer1State = 0;
            timer1.Enabled = true;
        }

        //----------------------------------------------------------
        // 功能说明：相机参数设置测试
        // 输入参数：1、String：SerialPortName，相机名称
        // 输出参数： 无
        // 返 回 值： Boolean，标记串口测试结果
        //----------------------------------------------------------------------
        private Boolean _SerialPortInitTest(String SerialPortName)
        {
            try
            {
                ControllerSerialPortCommucation = new SerialPort();
                ControllerSerialPortCommucation.PortName = SerialPortName;
                ControllerSerialPortCommucation.BaudRate = Global.ControllerSerialPortCommucation_BaudRate;
                ControllerSerialPortCommucation.ReceivedBytesThreshold = Global.ControllerSerialPortCommucation_ReceivedBytesThreshold;
                ControllerSerialPortCommucation.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(_SerialPortCommucation_DataReceived);

                ControllerSerialPortCommucation.Open();

                return true;
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：相机参数设置测试
        // 输入参数：1、String：CameraName，相机名称
        // 输出参数： 无
        // 返 回 值： Boolean，标记相机测试结果
        //----------------------------------------------------------------------
        private Boolean _CameraInitTest(String CameraName, String videoFormat, float deviceFrameRate, float deviceFrameRateRate, UInt16 gain)
        {
            ICImagingControl icImagingControlBuf;
            VCDPropertyItem ExposureItemBuf;                                   //曝光         
            VCDSwitchProperty ExposureAutoBuf;                                 //曝光自动选项
            VCDAbsoluteValueProperty ExposureValueBuf;                         //曝光手动设定值
            VCDPropertyItem WhiteBalanceItemBuf;                               //白平衡
            VCDSwitchProperty WhiteBalanceAutoBuf;                             //白平衡自动选项
            VCDPropertyItem ColorEnhancementItemBuf;                           //色彩增强
            VCDSwitchProperty ColorEnhancementEnableBuf;                       //色彩增强开启
            VCDPropertyItem GainItemBuf;                                       //增益
            VCDSwitchProperty GainAutoBuf;                                     //增益自动选项
            VCDRangeProperty GainValueBuf;                                     //增益手动设定值

            icImagingControlBuf = new ICImagingControl();                      //动态创建相机控件
            icImagingControlBuf.Device = CameraName;

            try                                                                //启动相机
            {
                if (icImagingControlBuf.DeviceValid)                               //相机初始化正常
                {
                    icImagingControlBuf.VideoFormat = videoFormat;
                    if (icImagingControlBuf.VideoFormat != videoFormat)      //设置相机视频格式
                    {
                        icImagingControlBuf.Dispose();
                        return false;
                    }

                    icImagingControlBuf.DeviceFrameRate = deviceFrameRate;                      //设置帧频
                    if (Math.Abs(icImagingControlBuf.DeviceFrameRate - deviceFrameRate) >= deviceFrameRateRate)//设定帧率与实际帧率差值不满足要求
                    {
                        icImagingControlBuf.Dispose();
                        return false;
                    }

                    icImagingControlBuf.DeviceTrigger = true;
                    if (icImagingControlBuf.DeviceTrigger != true)                 //设置触发方式
                    {
                        icImagingControlBuf.Dispose();
                        return false;
                    }

                    icImagingControlBuf.LiveCaptureContinuous = true;
                    if (icImagingControlBuf.LiveCaptureContinuous != true)
                    {
                        icImagingControlBuf.Dispose();
                        return false;
                    }

                    ExposureItemBuf =
                        icImagingControlBuf.VCDPropertyItems.FindItem(VCDIDs.VCDID_Exposure);
                    ExposureAutoBuf = (VCDSwitchProperty)ExposureItemBuf.Elements.FindInterface(
                        VCDIDs.VCDElement_Auto + ":" + VCDIDs.VCDInterface_Switch);
                    ExposureValueBuf = (VCDAbsoluteValueProperty)ExposureItemBuf.Elements.FindInterface(
                        VCDIDs.VCDElement_Value + ":" + VCDIDs.VCDInterface_AbsoluteValue);
                    ExposureAutoBuf.Switch = false;                                //设置相机自动曝光
                    if (ExposureAutoBuf.Switch != false)
                    {
                        icImagingControlBuf.Dispose();
                        return false;
                    }
                    ExposureValueBuf.Value = 0.0010;                               //设置曝光时间  
                    if (ExposureValueBuf.Value != 0.0010)
                    {
                        icImagingControlBuf.Dispose();
                        return false;
                    }

                    if (videoFormat.StartsWith("RGB32")) //相机配置为彩色模式
                    {
                        WhiteBalanceItemBuf =
                            icImagingControlBuf.VCDPropertyItems.FindItem(VCDIDs.VCDID_WhiteBalance);
                        WhiteBalanceAutoBuf = (VCDSwitchProperty)WhiteBalanceItemBuf.Elements.FindInterface(
                            VCDIDs.VCDElement_Auto + ":" + VCDIDs.VCDInterface_Switch);
                        WhiteBalanceAutoBuf.Switch = true;                             //设置白平衡自动
                        if (WhiteBalanceAutoBuf.Switch != true)
                        {
                            icImagingControlBuf.Dispose();
                            return false;
                        }

                        ColorEnhancementItemBuf =
                            icImagingControlBuf.VCDPropertyItems.FindItem(VCDIDs.VCDID_ColorEnhancement);
                        ColorEnhancementEnableBuf = (VCDSwitchProperty)ColorEnhancementItemBuf.Elements.FindInterface(
                            VCDIDs.VCDElement_Value + ":" + VCDIDs.VCDInterface_Switch);
                        ColorEnhancementEnableBuf.Switch = true;                       //设置色彩增强
                        if (ColorEnhancementEnableBuf.Switch != true)
                        {
                            icImagingControlBuf.Dispose();
                            return false;
                        }
                    }

                    GainItemBuf =
                        icImagingControlBuf.VCDPropertyItems.FindItem(VCDIDs.VCDID_Gain);
                    GainAutoBuf = (VCDSwitchProperty)GainItemBuf.Elements.FindInterface(
                        VCDIDs.VCDElement_Auto + ":" + VCDIDs.VCDInterface_Switch);
                    GainValueBuf = (VCDRangeProperty)GainItemBuf.Elements.FindInterface(
                        VCDIDs.VCDElement_Value + ":" + VCDIDs.VCDInterface_Range);
                    GainAutoBuf.Switch = false;                                    //设置增益自动
                    if (GainAutoBuf.Switch != false)
                    {
                        icImagingControlBuf.Dispose();
                        return false;
                    }

                    GainValueBuf.Value = gain;                   //设置相机增益  
                    if (GainValueBuf.Value != gain)
                    {
                        icImagingControlBuf.Dispose();
                        return false;
                    }

                    icImagingControlBuf.ImageRingBufferSize = Global.ImageRingBufferSizeMax;

                    icImagingControlBuf.Dispose();
                    return true;                                                   //各参数设置正确
                }
                else
                {
                    icImagingControlBuf.Dispose();
                    return false;
                }
            }
            catch(Exception ex)
            {
                if (null != icImagingControlBuf)      //设置相机视频格式
                {
                    icImagingControlBuf.Dispose();
                }
                return false;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：发送输入、输出诊断使能状态命令
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _SendDiagEnableState()
        {
            Byte[] Command = new Byte[Global.ControllerSerialPortCommucation_SendBytesThreshold];//保存串口发送指令

            Command[0] = 0x0A;
            Command[1] = 0x07;
            Command[2] = Convert.ToByte(0xFF & (VisionSystemClassLibrary.Class.System.MachineFaultEnableState >> 3));//10路PNP输出诊断使能低8位
            Command[3] = Convert.ToByte((0x03 & (VisionSystemClassLibrary.Class.System.MachineFaultEnableState >> 11))
                | (0xF0 & (VisionSystemClassLibrary.Class.System.MachineFaultEnableState >> 9))); //10路PNP输出诊断使能高2位和4路NPN输出诊断
            Command[4] = Convert.ToByte(0xFF & (VisionSystemClassLibrary.Class.System.MachineFaultEnableState >> 17));//8路PNP输入的使能状态
            Command[5] = Convert.ToByte(0x3F & (VisionSystemClassLibrary.Class.System.MachineFaultEnableState >> 25));//6路NPN输入的使能状态
            Command[6] = Convert.ToByte(0xFF & (VisionSystemClassLibrary.Class.System.MachineFaultEnableState >> 32));//位0-1分别表示相机1、2编码器使能状态
            Command[7] = Global.CameraChooseState;//相机选中状态
            Byte portState = 0;
            for (byte i = 0; i < Global.CameraNumberMax; i++)
            {
                if (Global.Camera[i].IsSerialPort)//当前为串口
                {
                    portState |= (Byte)(1 << i);
                }
            }
            Command[8] = portState;
            Command[9] = (Byte)(Command[0] ^ Command[1] ^ Command[2] ^ Command[3] ^ Command[4] ^ Command[5] ^ Command[6] ^ Command[7] ^ Command[8]);

            for (Byte i = 0; i < 10; i++)                                      //连续向下位机发送10次命令
            {
                if (!ControllerSerialPortCommucation.IsOpen)                                       //当前端口未打开
                {
                    ControllerSerialPortCommucation.Open();
                }
                ControllerSerialPortCommucation.Write(Command, 0, 10);
                ControllerCommunicationFlag = false;
                Thread.Sleep(1000);
                if (ControllerCommunicationFlag == true)                                 //向下位机发送命令成功
                {
                    break;
                }
            }

            if (ControllerCommunicationFlag == false)//下位机通讯故障
            {
                label1.Text = "下位机通讯故障！";
                label1.Visible = true;

                return;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：发送光源校准命令
        // 输入参数：1、Byte：index，数组下标索引
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _LightAdjust(Byte index)
        {
            Byte[] Command = new Byte[Global.ControllerSerialPortCommucation_SendBytesThreshold];//保存串口发送指令

            Command[0] = 0x05;
            Command[1] = 0x04;

            switch (index)
            {
                case 0:
                    Command[2] = 0x01;//01表示相机1
                    break;
                case 1:
                    Command[2] = 0x81;//81表示相机2
                    break;
                case 2:
                    Command[2] = 0x41;//41表示相机3
                    break;
                case 3:
                    Command[2] = 0xC1;//C1表示相机4;
                    break;
                default:
                    Command[2] = 0x01;//01表示相机1
                    break;
            }

            Command[3] = Convert.ToByte(Global.Camera[index].DeviceParameter.StroboCurrent & (0xFF));
            Command[4] = Convert.ToByte(Global.Camera[index].DeviceParameter.StroboCurrent >> 8);
            Command[5] = Convert.ToByte(Global.Camera[index].DeviceParameter.StroboTime);
            Command[6] = (Byte)(Command[0] ^ Command[1] ^ Command[2] ^ Command[3] ^ Command[4] ^ Command[5]);

            for (Byte i = 0; i < 10; i++)                                      //连续向下位机发送10次相机上电命令
            {
                if (!ControllerSerialPortCommucation.IsOpen)                                       //当前端口未打开
                {
                    ControllerSerialPortCommucation.Open();
                }
                ControllerSerialPortCommucation.Write(Command, 0, 7);
                ControllerCommunicationFlag = false;
                Thread.Sleep(1000);
                if (ControllerCommunicationFlag == true)                                 //向下位机发送命令成功
                {
                    break;
                }
            }
            if (ControllerCommunicationFlag == false)//下位机通讯故障
            {
                label1.Text = "光源校准失败！";
                label1.Visible = true;

                return;
            }

            Command[0] = 0x0D;
            Command[1] = 0x01;
            Command[2] = 0x00;
            Command[3] = (Byte)(Command[0] ^ Command[1] ^ Command[2]);

            if (!ControllerSerialPortCommucation.IsOpen)                                       //当前端口未打开
            {
                ControllerSerialPortCommucation.Open();
            }
            ControllerSerialPortCommucation.Write(Command, 0, 4);
        }

        //-----------------------------------------------------------------------
        // 功能说明：发送机器类型和相位信息
        // 输入参数：1、Byte：index，数组下标索引
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _SendPhase(Byte index)
        {
            Byte[] Command = new Byte[Global.ControllerSerialPortCommucation_SendBytesThreshold];//保存串口发送指令

            for (Byte i = 0; i < Command.Length; i++)
            {
                Command[i] = 0;
            }

            Byte dataNumber = 2;
            if (Global.Camera[index].DeviceParameter.Parameter != null)
            {
                dataNumber += Convert.ToByte(Global.Camera[index].DeviceParameter.Parameter.Count * 2);
            }

            Command[0] = 0x10;
            Command[1] = dataNumber;
            switch (index)
            {
                case 0:
                    Command[2] = 0x00;//00表示相机1
                    break;
                case 1:
                    Command[2] = 0x80;//80表示相机2
                    break;
                case 2:
                    Command[2] = 0x40;//40表示相机3
                    break;
                case 3:
                    Command[2] = 0xC0;//C0表示相机4;
                    break;
                default:
                    Command[2] = 0x00;//00表示相机1
                    break;
            }
            Command[3] = Convert.ToByte(1);//机器类型；

            if (Global.Camera[index].DeviceParameter.Parameter != null)
            {
                for (Byte i = 0; i < Global.Camera[index].DeviceParameter.Parameter.Count; i++)
                {
                    Command[i * 2 + 4] = Convert.ToByte(Global.Camera[index].DeviceParameter.Parameter[i] & 0xFF);//相机曝光相位低8位；
                    Command[i * 2 + 5] = Convert.ToByte((Global.Camera[index].DeviceParameter.Parameter[i] >> 8) & 0xFF);//相机曝光相位高8位；
                }
            }

            Int32 checkIndex = dataNumber + 2;
            for (Byte i = 0; i < checkIndex; i++)
            {
                Command[checkIndex] ^= Command[i];
            }

            for (Byte i = 0; i < 10; i++)                                      //连续向下位机发送10次相机上电命令
            {
                if (!ControllerSerialPortCommucation.IsOpen)                                       //当前端口未打开
                {
                    ControllerSerialPortCommucation.Open();
                }
                ControllerSerialPortCommucation.Write(Command, 0, checkIndex + 1);
                ControllerCommunicationFlag = false;
                Thread.Sleep(1000);
                if (ControllerCommunicationFlag == true)                                 //向下位机发送命令成功
                {
                    break;
                }
            }
            if (ControllerCommunicationFlag == false)//下位机通讯故障
            {
                label1.Text = "相位命令发送失败！";
                label1.Visible = true;

                return;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：发送相机上电命令
        // 输入参数： 1、Byte：cameraChooseState，相机开启状态标记
        // 输出参数： 无
        // 返 回 值： 无
        //---------------------------------------------------------------------
        private void _CameraPowerOn(Byte cameraChooseState)
        {
            Byte[] Command = new Byte[Global.ControllerSerialPortCommucation_SendBytesThreshold];//保存串口发送指令

            switch (cameraChooseState)
            {
                case 0:
                    Command[0] = 0x07;
                    Command[1] = 0x01;
                    Command[2] = 0x00;                                         //端口1相机上电
                    break;
                case 1:
                    Command[0] = 0x07;
                    Command[1] = 0x01;
                    Command[2] = 0x01;                                         //端口2相机上电
                    break;
                case 2:
                    Command[0] = 0x07;
                    Command[1] = 0x01;
                    Command[2] = 0x02;                                         //端口1和端口2相机上电
                    break;
                case 3:
                    Command[0] = 0x07;
                    Command[1] = 0x01;
                    Command[2] = 0x03;                                         //端口3相机上电
                    break;
                case 4:
                    Command[0] = 0x07;
                    Command[1] = 0x01;
                    Command[2] = 0x04;                                         //端口4相机上电
                    break;
                case 5:
                    Command[0] = 0x07;
                    Command[1] = 0x01;
                    Command[2] = 0x05;                                         //端口3和端口4相机上电
                    break;
                case 15:
                    Command[0] = 0x07;
                    Command[1] = 0x01;
                    Command[2] = 0x0F;                                         //端口1、端口2、端口3和端口4相机上电
                    break;
                default:
                    break;
            }
            Command[3] = (Byte)(Command[0] ^ Command[1] ^ Command[2]);
            for (Byte i = 0; i < 10; i++)                                      //连续向下位机发送10次相机上电命令
            {
                if (!ControllerSerialPortCommucation.IsOpen)                                       //当前端口未打开
                {
                    ControllerSerialPortCommucation.Open();
                }
                ControllerSerialPortCommucation.Write(Command, 0, 4);
                ControllerCommunicationFlag = false;
                Thread.Sleep(100);
                if (ControllerCommunicationFlag == true)                                 //向下位机发送命令成功
                {
                    break;
                }
            }

            if (ControllerCommunicationFlag == false)//下位机通讯故障
            {
                label1.Text = "相机上电失败！";
                label1.Visible = true;

                return;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：发送相机下电命令
        // 输入参数： 1、Byte：cameraChooseState，相机名称
        // 输出参数： 无
        // 返 回 值： 无
        //---------------------------------------------------------------------
        private void _CameraPowerOff(Byte cameraChooseState)
        {
            Byte[] Command = new Byte[Global.ControllerSerialPortCommucation_SendBytesThreshold];//保存串口发送指令

            switch (cameraChooseState)
            {
                case 0:
                    Command[0] = 0x08;
                    Command[1] = 0x01;
                    Command[2] = 0x00;                                         //端口1相机下电
                    break;
                case 1:
                    Command[0] = 0x08;
                    Command[1] = 0x01;
                    Command[2] = 0x01;                                         //端口2相机下电
                    break;
                case 2:
                    Command[0] = 0x08;
                    Command[1] = 0x01;
                    Command[2] = 0x02;                                         //端口1和端口2相机下电
                    break;
                case 3:
                    Command[0] = 0x08;
                    Command[1] = 0x01;
                    Command[2] = 0x03;                                         //端口3相机下电
                    break;
                case 4:
                    Command[0] = 0x08;
                    Command[1] = 0x01;
                    Command[2] = 0x04;                                         //端口4相机下电
                    break;
                case 5:
                    Command[0] = 0x08;
                    Command[1] = 0x01;
                    Command[2] = 0x05;                                         //端口3和端口4相机下电
                    break;
                case 15:
                    Command[0] = 0x08;
                    Command[1] = 0x01;
                    Command[2] = 0x0F;                                         //端口1、端口2、端口3和端口4相机同时下电
                    break;
                default:
                    break;
            }
            Command[3] = (Byte)(Command[0] ^ Command[1] ^ Command[2]);
            for (Byte i = 0; i < 10; i++)                                      //连续向下位机发送10次相机下电命令
            {
                if (!ControllerSerialPortCommucation.IsOpen)                                       //当前端口未打开
                {
                    ControllerSerialPortCommucation.Open();
                }
                ControllerSerialPortCommucation.Write(Command, 0, 4);
                ControllerCommunicationFlag = false;
                Thread.Sleep(100);
                if (ControllerCommunicationFlag == true)                                 //向下位机发送命令成功
                {
                    break;
                }
            }

            if (ControllerCommunicationFlag == false)//下位机通讯故障
            {
                label1.Text = "相机下电失败！";
                label1.Visible = true;

                return;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：初始化全局变量
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _InitGlobal()
        {
            //系统机器信息初始化
            VisionSystemClassLibrary.Class.System._ReadMachineStateInfoFile();

            //相机使能信息初始化
            Global.CameraChooseState = VisionSystemClassLibrary.Class.Camera._ReadCameraChooseState(".\\");

            //相机类初始化
            Global.Camera = new VisionSystemClassLibrary.Class.Camera[Global.CameraNumberMax];
            Global.CameraTemp = new VisionSystemClassLibrary.Class.Camera[Global.CameraNumberMax];
            Global.Check_CameraPort = new Dictionary<VisionSystemClassLibrary.Enum.CameraType, Byte>();

            //-------------------------------------------------------------------------------------------
            for (Byte i = 0; i < Global.CameraNumberMax; i++)//初始化相机类(相机配置时，可能调用双相机，必须全部初始化)
            {
                Global.Camera[i] = new VisionSystemClassLibrary.Class.Camera(".\\", (VisionSystemClassLibrary.Enum.PortType)(i + 1));

                //相机缓存初始化
                Global.Camera[i].DeviceInformation.CAM = VisionSystemClassLibrary.Enum.CameraState.ON;
                Global.CameraTemp[i] = Global.Camera[i]._Copy();

                if ((Global.CameraChooseState & (0x01 << i)) != 0)//当前相机开启
                {
                    Global.Check_CameraPort.Add(Global.Camera[i].Type, Global.Camera[i].DeviceInformation.Port);
                }
            }
            //-------------------------------------------------------------------------------------此段代码产生40M缓存

            //相机设备信息初始化
            Global.CameraDevice = new VisionSystemClassLibrary.Struct.ClientCamera();

            //硬件设备信息初始化
            Global.DeviceInformation = new VisionSystemClassLibrary.Struct.DeviceData();
            Global.DeviceIOSignal = new VisionSystemClassLibrary.Struct.IOSignal();
            Global.DeviceIOSignal.InputState = 0;//输入状态
            Global.DeviceIOSignal.OutputState = 0;//输出状态

            //客户端设备信息初始化
            Global.ClientData = new VisionSystemCommunicationLibrary.Ethernet.ClientData();
            Global.ClientData.Port = 5000;//端口
            Global.ClientData.ReceiveBufferSize = 8192;//接受缓冲区
            Global.ClientData.SendBufferSize = 8192;//发送缓冲区
            if ("" == VisionSystemClassLibrary.Class.Camera.DeviceIPAddress)
            {
                Global.ClientData.ServerIP = VisionSystemClassLibrary.Class.System.DeviceIPAddress + "1";//服务端IP地址
            }
            else
            {
                Global.ClientData.ServerIP = VisionSystemClassLibrary.Class.Camera.DeviceIPAddress;//服务端IP地址
            }
            Global.ClientData.FirmwareVersion = FileVersionInfo.GetVersionInfo(Application.ExecutablePath).FileVersion;//固件版本

            Global.BrandName = "";//烟包品牌名称

            Global.ExposureTime = new Double[30];
            Global.ExposureTime[0] = 0.0001;//曝光时间1/10000
            Global.ExposureTime[1] = 0.0002;//曝光时间1/5000
            Global.ExposureTime[2] = 0.0003;//曝光时间1/3333
            Global.ExposureTime[3] = 0.0004;//曝光时间1/2500
            Global.ExposureTime[4] = 0.0005;//曝光时间1/2000
            Global.ExposureTime[5] = 0.0006;//曝光时间1/1667
            Global.ExposureTime[6] = 0.0007;//曝光时间1/1429
            Global.ExposureTime[7] = 0.0008;//曝光时间1/1250
            Global.ExposureTime[8] = 0.0009;//曝光时间1/1111
            Global.ExposureTime[9] = 0.0010;//曝光时间1/1000
            Global.ExposureTime[10] = 0.0011;//曝光时间1/909
            Global.ExposureTime[11] = 0.0012;//曝光时间1/833
            Global.ExposureTime[12] = 0.0013;//曝光时间1/769
            Global.ExposureTime[13] = 0.0014;//曝光时间1/714，以下不同相机可能配置不同
            Global.ExposureTime[14] = 0.0015;//曝光时间1/667
            Global.ExposureTime[15] = 0.0017;//曝光时间1/588
            Global.ExposureTime[16] = 0.0018;//曝光时间1/556
            Global.ExposureTime[17] = 0.0020;//曝光时间1/500
            Global.ExposureTime[18] = 0.0021;//曝光时间1/476
            Global.ExposureTime[19] = 0.0023;//曝光时间1/435
            Global.ExposureTime[20] = 0.0025;//曝光时间1/400
            Global.ExposureTime[21] = 0.0027;//曝光时间1/370
            Global.ExposureTime[22] = 0.0029;//曝光时间1/345
            Global.ExposureTime[23] = 0.0031;//曝光时间1/323
            Global.ExposureTime[24] = 0.0034;//曝光时间1/294
            Global.ExposureTime[25] = 0.0037;//曝光时间1/270
            Global.ExposureTime[26] = 0.0040;//曝光时间1/250
            Global.ExposureTime[27] = 0.0043;//曝光时间1/233
            Global.ExposureTime[28] = 0.0046;//曝光时间1/217
            Global.ExposureTime[29] = 0.0050;//曝光时间1/200

            Global.MachineFaultState = 0;
            Global.MachineFaultSaveState = 0;

            Global.MachineStopState = new UInt32[Global.CameraNumberMax];//机器停机状态
            Global.MachineStopStateTemp = new UInt32[Global.CameraNumberMax];//机器停机状态缓存
            Global.MachineStopStaticSaveState = new Boolean[Global.CameraNumberMax];//机器停机数据保存状态标记

            for (Byte i = 0; i < Global.CameraNumberMax; i++)//遍历当前所有相机
            {
                if ((Global.CameraChooseState & (0x01 << i)) != 0)//当前相机开启
                {
                    Global.MachineStopState[i] = 0;
                    Global.MachineStopStateTemp[i] = 0;
                    Global.MachineStopStaticSaveState[i] = false;
                }
            }

            Global.CameraPort = new String[Global.CameraNumberMax];//保存端口1相机名称
            Global.SnPort = new String[Global.CameraNumberMax];//端口1相机序列号

            Global.ImageRingBufferSizeMax = 16; //相机设备图像缓存最大值

            Global.ImageBufferIndex = new Int32[Global.CameraNumberMax];//相机设备图像索引
            Global.ImageSourceBufferIndex = new Int32[Global.CameraNumberMax];//相机图像索引
            Global.ImageSourceBufferSizeMax = 16;//相机图像缓存最大值

            //控制器串口命令表
            Global.ControllerSerialPortCommucationType = new Dictionary<Byte, Byte>();
            Global.ControllerSerialPortCommucationBuffer = new List<Byte>();
            Global.ControllerSerialPortCommucationType.Add(1, 17);
            //Global.ControllerSerialPortCommucationType.Add(5, 2);
            //Global.ControllerSerialPortCommucationType.Add(7, 0);
            //Global.ControllerSerialPortCommucationType.Add(8, 0);
            //Global.ControllerSerialPortCommucationType.Add(9, 0);
            Global.ControllerSerialPortCommucationType.Add(10, 0);
            Global.ControllerSerialPortCommucationType.Add(12, 1);
            //Global.ControllerSerialPortCommucationType.Add(16, 0);

            //传感器串行通讯命令表
            Global.SerialPortCommucationType = new Dictionary<Byte, Byte>[Global.CameraNumberMax];
            Global.SerialPortCommucationBuffer = new List<Byte>[Global.CameraNumberMax];
            for (Byte i = 0; i < Global.CameraNumberMax; i++)//遍历当前所有相机
            {
                if ((Global.CameraChooseState & (0x01 << i)) != 0)//当前相机开启
                {
                    if (Global.Camera[i].IsSerialPort) //当前为串口，进行初始化
                    {
                        Global.SerialPortCommucationType[i] = new Dictionary<Byte, Byte>();
                        Global.SerialPortCommucationBuffer[i] = new List<Byte>();

                        switch (Global.Camera[i].Sensor_ProductType)
                        {
                            case VisionSystemClassLibrary.Enum.SensorProductType._89713FC:
                                Global.SerialPortCommucationType[i].Add(2, 43);
                                Global.SerialPortCommucationType[i].Add(3, 20);
                                Global.SerialPortCommucationType[i].Add(4, 20);
                                Global.SerialPortCommucationType[i].Add(20, 12);
                                break;

                            case VisionSystemClassLibrary.Enum.SensorProductType._89713FA:
                            case VisionSystemClassLibrary.Enum.SensorProductType._89713CF:
                                Byte iTobaccoNumber = 0;
                                for (Byte j = 0; j < Global.Camera[i].PerTobaccoNumber.Count; j++)
                                {
                                    iTobaccoNumber += Global.Camera[i].PerTobaccoNumber[j];
                                }
                                Global.SerialPortCommucationType[i].Add(2, (Byte)(iTobaccoNumber + 2));
                                Global.SerialPortCommucationType[i].Add(3, 93);
                                Global.SerialPortCommucationType[i].Add(4, Global.Camera[i].SensorNumber);
                                Global.SerialPortCommucationType[i].Add(7, 11);
                                Global.SerialPortCommucationType[i].Add(8, 11);
                                Global.SerialPortCommucationType[i].Add(10, Global.Camera[i].SensorNumber);
                                break;

                            case VisionSystemClassLibrary.Enum.SensorProductType._89750A:
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            //初始化串口信息
            Global.SerialPortName = new string[Global.CameraNumberMax];//串口名称
            Global.SerialPortSn = new string[Global.CameraNumberMax];//串口序列号
            CommunicationFlag = new Boolean[Global.CameraNumberMax];//串口通讯标记

            LockSerialPort = new Object[Global.CameraNumberMax];//防止串口发送数据冲突
            SerialPortCommucation = new SerialPort[Global.CameraNumberMax];//动态创建串口

            for (Byte i = 0; i < Global.CameraNumberMax; i++)//遍历当前所有相机
            {
                if ((Global.CameraChooseState & (0x01 << i)) != 0)//当前相机开启
                {
                    if (Global.Camera[i].IsSerialPort) //当前为串口，进行初始化
                    {
                        LockSerialPort[i] = new Object();
                        SerialPortCommucation[i] = new SerialPort();
                    }
                }
            }

            //初始化烟支位置信息
            _InitTobaccoPosion();

            Global.DeepLearningSavingFlag = new Boolean[Global.CameraNumberMax];

            //从INI文件中获取ShiftTest标识，如果为True,设置计算机时间然后重启，验证Shift初始化OK。
            String path = Global.FilePath + Global.SystemSet;
            if (File.Exists(path))//存在该文件
            {
                var bShiftTest = INIFile.Read("System", "CAMERA1", null, path);//读取数值

                if (!String.IsNullOrEmpty(bShiftTest))//不为空
                {
                    switch (bShiftTest.ToUpper())//筛选
                    {
                        case "TRUE":
                            Global.DeepLearningSavingFlag[0] = true;
                            break;
                        default:
                            break;
                    }
                }

                var bShiftTest1 = INIFile.Read("System", "CAMERA2", null, path);//读取数值

                if (!String.IsNullOrEmpty(bShiftTest1))//不为空
                {
                    switch (bShiftTest1.ToUpper())//筛选
                    {
                        case "TRUE":
                            Global.DeepLearningSavingFlag[1] = true;
                            break;
                        default:
                            break;
                    }
                }

                var bShiftTest2 = INIFile.Read("System", "CAMERA3", null, path);//读取数值

                if (!String.IsNullOrEmpty(bShiftTest2))//不为空
                {
                    switch (bShiftTest2.ToUpper())//筛选
                    {
                        case "TRUE":
                            Global.DeepLearningSavingFlag[2] = true;
                            break;
                        default:
                            break;
                    }
                }

                var bShiftTest3 = INIFile.Read("System", "CAMERA4", null, path);//读取数值

                if (!String.IsNullOrEmpty(bShiftTest3))//不为空
                {
                    switch (bShiftTest3.ToUpper())//筛选
                    {
                        case "TRUE":
                            Global.DeepLearningSavingFlag[3] = true;
                            break;
                        default:
                            break;
                    }
                }
            }

            Global.DeepLearningImageCount = new Int32[Global.CameraNumberMax];//深度学习图片计数
            Global.DeepLearningImageMax = 1500;//深度学习图片计数
                        
            //检测删除溢出统计信息
            Thread threadDeleteStatics = new Thread(_threadDeleteStatics);
            threadDeleteStatics.Priority = ThreadPriority.BelowNormal;
            threadDeleteStatics.IsBackground = true;
            threadDeleteStatics.Start();
        }

        //-----------------------------------------------------------------------
        // 功能说明：上电删除统计数据
        // 输入参数：无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _threadDeleteStatics()
        {
            VisionSystemClassLibrary.Class.Shift._LoadDeleteStatics(".\\", Global.CameraNumberMax, Global.CameraChooseState, 0.6, 0.5);//上电删除历史统计数据
        }

        //-----------------------------------------------------------------------
        // 功能说明：串口接收中断函数，读串口数据
        // 输入参数：1、object：sender，serialPort控件对象
        //           2、System.IO.Ports.SerialDataReceivedEventArgs：e，serialport控件事件响应参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _SerialPortCommucation_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            Byte DataType;
            UInt16 DataCount;

            while (ControllerSerialPortCommucation.BytesToRead >= Global.ControllerSerialPortCommucation_ReceivedBytesThreshold) //串口1读取字节长度大于等于16
            {
                Byte[] ReceiveData = new Byte[Global.ControllerSerialPortCommucation_ReceivedBytesThreshold]; //保存串口接收数据
                Byte ReceiveChecksum = 0;                        　　　　　　　　        //保存串口接收校验和
                ControllerSerialPortCommucation.Read(ReceiveData, 0, Global.ControllerSerialPortCommucation_ReceivedBytesThreshold);
                DataType = ReceiveData[0];
                DataCount = (UInt16)(ReceiveData[1] + 2);

                for (UInt16 i = 0; i < DataCount; i++)                         //读取串口数据，计算校验数据
                {
                    ReceiveChecksum ^= ReceiveData[i];
                }

                if (ReceiveChecksum == ReceiveData[DataCount])                 //校验和相同
                {
                    ControllerCommunicationFlag = true;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：获取磁盘序列号
        // 输入参数：无
        // 输出参数：无
        // 返回值：String，返回磁盘序列号
        //----------------------------------------------------------------------
        private String _GetHardDiskSerialNumber()
        {
            String sSerialNumber = "";//返回值

            try
            {
                ManagementObjectSearcher mos = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");

                foreach (ManagementObject mo in mos.Get())//获取第一块磁盘序列号
                {
                    sSerialNumber = mo["SerialNumber"].ToString().Trim();

                    //

                    break;
                }
            }
            catch (System.Exception ex)
            {
                //不执行操作
            }

            //

            return sSerialNumber;
        }

        //----------------------------------------------------------------------
        // 功能说明：获取产品密钥
        // 输入参数：1、String：sValue，运算数值
        // 输出参数：无
        // 返回值：String，返回产品密钥
        //----------------------------------------------------------------------
        private String _GetProductKey(String sValue)
        {
            String sProductKey = "";//产品密钥

            if ("" != sValue)//有效
            {
                Int32 i = 0;//循环控制变量
                Int32 j = 0;//循环控制变量

                Int32 iTemporary_1 = 0;//临时变量
                Int32 iTemporary_2 = 0;//临时变量

                for (i = 0; i < sValue.Length; i++)//运算
                {
                    iTemporary_1 = 0;//复位

                    for (j = 0; j < sValue.Length; j++)//运算
                    {
                        if (i != j)//有效
                        {
                            iTemporary_1 += sValue[j];
                        }
                    }

                    if (1 < sValue.Length)//有效
                    {
                        iTemporary_1 = Math.Abs(sValue[i] - iTemporary_1 / (sValue.Length - 1));
                    }

                    //

                    iTemporary_2 = sValue[i] + iTemporary_1;

                    while (true)
                    {
                        if ((iTemporary_2 >= 0x30 && iTemporary_2 <= 0x39) || (iTemporary_2 >= 0x41 && iTemporary_2 <= 0x5A) || (iTemporary_2 >= 0x61 && iTemporary_2 <= 0x7A))//在数字和大小写字母范围之内
                        {
                            if (iTemporary_2 >= 0x61 && iTemporary_2 <= 0x7A)//若为小写字母，将其转换为大写字母.
                            {
                                iTemporary_2 = iTemporary_2 - 0x20;
                            }

                            //

                            sProductKey += Char.ToString((char)iTemporary_2);

                            //

                            break;
                        }

                        iTemporary_2 += iTemporary_1;

                        if (0x7A < iTemporary_2)
                        {
                            iTemporary_2 = 0x30;
                        }
                    }
                }
            }

            //

            return sProductKey;
        }

        //----------------------------------------------------------------------
        // 功能说明：检查产品密钥
        // 输入参数：1、String：sProductKeyPath，产品密钥完整路径（包含文件名）
        //           2、String：sValue，运算数值
        // 输出参数：无
        // 返回值：Boolean，产品密钥是否有效。取值范围：true，是；false，否
        //----------------------------------------------------------------------
        private Boolean _CheckProductKey(String sProductKeyPath, String sValue)
        {
            Boolean bReturn = false;//返回值

            //

            FileStream filestream = null;

            try
            {
                filestream = new FileStream(sProductKeyPath, FileMode.Open); //打开文件

                BinaryReader binaryreader = new BinaryReader(filestream);
                String sProductKey = binaryreader.ReadString();//读取

                binaryreader.Close();//关闭文件
                filestream.Close();//关闭流

                //

                if (sValue == sProductKey)//有效
                {
                    bReturn = true;
                }
            }
            catch (Exception ex)
            {
                //不执行操作
            }

            //

            return bReturn;
        }

        //----------------------------------------------------------------------
        // 功能说明：定时器1，进入Work界面参数初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;

            if (Timer1State == 0)//检查密码
            {
                //检查产品密钥
                String sSerialNumber = _GetProductKey(_GetHardDiskSerialNumber());//产品序列号
                String sProductKey = _GetProductKey(sSerialNumber);//产品密钥
                String sProductKeyPath = "";//ProductKey.dat文件路径

                sProductKeyPath = VisionSystemClassLibrary.Class.System.ConfigDataPathName + "ProductKey.dat";//ProductKey.dat文件路径

                //if (_CheckProductKey(sProductKeyPath, sProductKey) == false)//检查产品密钥失败,重新启动软件
                //{
                //    Application.Exit();
                //}
                //else
                {
                    Timer1State++;
                    timer1.Enabled = true;
                }
            }
            else if (Timer1State == 1)//查询串口
            {
                if (false == Global.ComputerRunState) //控制器上运行软件
                {
                    Timer3State = 0;
                    SerialPortDelayCount = 0;

                    timer3.Enabled = true;
                }
                else
                {
                    Timer1State++;
                    timer1.Enabled = true;
                }
            }
            else if (Timer1State == 2)//参数初始化
            {
                try
                {
                    _InitGlobal();

                    Timer1State++;
                    timer1.Enabled = true;
                }
                catch (System.Exception ex)
                {
                    label1.Text = "参数初始化异常！";
                    label1.Visible = true;
                }
            }
            else if (Timer1State == 3)//向下位机发送命令
            {
                ControllerCommunicationFlag = true;

                if (false == Global.ComputerRunState) //控制器上运行软件
                {
                    lock (LockControllerSerialPort)
                    {
                        if (ControllerSerialPortCommucation.BytesToWrite > 0)                    //发送缓冲区内未清空
                        {
                            ControllerSerialPortCommucation.DiscardOutBuffer();                       //清空发送缓冲区数据
                        }

                        if (ControllerSerialPortCommucation.BytesToRead > 0)                    //发送缓冲区内未清空
                        {
                            ControllerSerialPortCommucation.DiscardInBuffer();                        //清空接受缓冲区数据
                        }

                        _SendDiagEnableState();                                         //发送输出诊断使能状态
                    }

                    for (Byte i = 0; i < Global.CameraNumberMax; i++)//遍历当前所有相机
                    {
                        if ((Global.CameraChooseState & (0x01 << i)) != 0)//当前相机开启
                        {
                            lock (LockControllerSerialPort)
                            {
                                _LightAdjust(i);

                                if (Global.Camera[i].DeviceParameter.Parameter != null) //当前执行相位命令发送
                                {
                                    _SendPhase(i);
                                }
                            }
                        }
                    }
                    _CameraPowerOff(15);
                }

                Timer1State++;
                timer1.Enabled = true;
            }
            else if (Timer1State == 4)//相机下电开启测试
            {
                if (ControllerCommunicationFlag == true)     //相机下电
                {
                    PowerOnCameraIndex = _GetCameraIndex(0);//查询第一个上电相机索引

                    for (Byte i = 0; i < Global.CameraNumberMax; i++)//遍历当前所有相机
                    {
                        if ((Global.CameraChooseState & (0x01 << i)) != 0)//当前相机开启
                        {
                            if (Global.Camera[i].IsSerialPort && (false == Global.ComputerRunState)) //当前为串口，获取串口名称，控制器上运行软件
                            {
                                SerialPortNames.Add("COM1");
                                break;
                            }
                        }
                    }

                    if (false == Global.ComputerRunState) //控制器上运行软件
                    {
                        lock (LockControllerSerialPort)//相机上电
                        {
                            switch (PowerOnCameraIndex)
                            {
                                case 0:
                                    _CameraPowerOn(0);                                         //端口1相机上电
                                    break;
                                case 1:
                                    _CameraPowerOn(2);                                         //端口1和端口2相机上电
                                    break;
                                case 2:
                                    _CameraPowerOn(3);                                         //端口3相机上电
                                    break;
                                case 3:
                                    _CameraPowerOn(5);                                         //端口3和端口4相机上电
                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                    CameraFlag = 0;                                                //相机重复上电次数清零
                    DelayCount = 0;                                                //相机上电延时计数清零

                    timer2.Enabled = true;
                }
            }
            else if (Timer1State == 5)//读取注册表参数
            {
                if (false == Global.ComputerRunState) //控制器上运行软件
                {
                    if (ControllerSerialPortCommucation.IsOpen)                                     //串口1处于打开状态
                    {
                        ControllerSerialPortCommucation.Close();
                    }
                    ControllerSerialPortCommucation.Dispose();
                }

                for (Byte i = 0; i < Global.CameraNumberMax; i++)//遍历当前所有相机
                {
                    if ((Global.CameraChooseState & (0x01 << i)) != 0)//当前相机开启
                    {
                        if (Global.Camera[i].IsSerialPort) //当前为串口，进行初始化
                        {
                            if (SerialPortCommucation[i].IsOpen)  //串口处于打开状态
                            {
                                SerialPortCommucation[i].Close();
                            }
                            SerialPortCommucation[i].Dispose();
                        }
                    }
                }

                Timer1State++;
                timer1.Enabled = true;
            }
            else if (Timer1State == 6)//相机自触发
            {
                Work.pWork._InitCameraAndControl1();

                Timer1State++;
                timer1.Enabled = true;
            }
            else if (Timer1State == 7)//停止相机自触发，开启外部触发模式
            {
                for (Byte i = 0; i < Global.CameraNumberMax; i++)//初始化相机类
                {
                    if ((Global.CameraChooseState & (0x01 << i)) != 0)//当前相机开启
                    {
                        if (false == Global.Camera[i].IsSerialPort) //当前为相机，停止工作
                        {
                            Work.pWork._StopCamera(i);//停止相机
                        }
                    }
                }

                Timer1State++;
                timer1.Enabled = true;
            }
            else
            {
                Work.pWork._InitCameraAndControl2();

                Work.pWork.Show();
                this.Dispose();
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：测试下次上电相机索引
        // 输入参数：Byte：startPos，上次上电相机索引
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private Byte _GetCameraIndex(Byte startPos)
        {
            for (; startPos < Global.CameraNumberMax; startPos++)//遍历当前所有相机
            {
                if ((Global.CameraChooseState & (0x01 << startPos)) != 0)//当前相机开启
                {
                    break;
                }
            }
            return startPos;
        }

        //----------------------------------------------------------------------
        // 功能说明：测试相机序列号
        // 输入参数：ICImagingControl：icImagingControl1，上次上电相机索引
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _GetCameraSerialNumber(ICImagingControl icImagingControl1, ref String cameraName, ref String snPort)
        {
            Int32 i, j;

            for (i = 0; i < icImagingControl1.Devices.Length; i++)
            {
                String snPortTemp = "";
                icImagingControl1.Devices[i].GetSerialNumber(out snPortTemp);

                for (j = 0; j < CameraTestTrue.Count; j++)
                {
                    if (Global.SnPort[CameraTestTrue[j]] == snPortTemp) //查询相机已存在
                    {
                        Global.CameraPort[CameraTestTrue[j]] = icImagingControl1.Devices[i].Name;
                        break;
                    }
                }

                if (j == CameraTestTrue.Count) //该相机序列号为新
                {
                    cameraName = icImagingControl1.Devices[i].Name;
                    snPort = snPortTemp;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：定时器3，串口识别
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void timer3_Tick(object sender, EventArgs e)
        {
            timer3.Enabled = false;

            if (Timer3State == 0)
            {
                if (SerialPortDelayCount < 10) //10次机会查询串口是否存在
                {
                    if (System.IO.Ports.SerialPort.GetPortNames().Contains("COM1"))//查询到串口，跳出循环
                    {
                        Timer3State++;
                    }
                    SerialPortDelayCount++;

                    timer3.Enabled = true;
                }
                else//
                {
                    label1.Text = "未发现可用串口timer3_Tick";
                    label1.Visible = true;
                }
            }
            else
            {
                if (_SerialPortInitTest("COM1"))//当前串口测试通过
                {
                    Global.ControllerSerialPortName = "COM1";

                    Timer1State++;
                    timer1.Enabled = true;
                }
                else
                {
                    label1.Text = "未发现可用串口timer3_Tick";
                    label1.Visible = true;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：定时器2，相机识别
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Enabled = false;

            if (PowerOnCameraIndex < Global.CameraNumberMax) //相机模式正常
            {
                Boolean bBeCaptured = false;
                ICImagingControl icImagingControl1 = null;

                String sSerialPortName = "";
                if (Global.Camera[PowerOnCameraIndex].IsSerialPort) //当前识别串口
                {
                    if (_USBPortRecognise(ref sSerialPortName)) //串口识别成功
                    {
                        bBeCaptured = true;
                    }
                }
                else  //当前识别相机
                {
                    icImagingControl1 = new ICImagingControl();

                    if (icImagingControl1.Devices.Length > CameraTestTrue.Count)   //识别到端口相机
                    {
                        bBeCaptured = true;
                    }
                }

                String snPort = "", cameraName = "";

                if (bBeCaptured)   //识别到新相机或新串口
                {
                    Boolean bInitResult = false;

                    if (Global.Camera[PowerOnCameraIndex].IsSerialPort) //当前识别串口
                    {
                        bInitResult = true;
                    }
                    else  //当前识别相机
                    {
                        _GetCameraSerialNumber(icImagingControl1, ref cameraName, ref snPort);

                        if (_CameraInitTest(cameraName, Global.Camera[PowerOnCameraIndex].VideoFormat, Global.Camera[PowerOnCameraIndex].DeviceFrameRate, Global.Camera[PowerOnCameraIndex].DeviceFrameRateAbs, Global.Camera[PowerOnCameraIndex].DeviceParameter.Gain)) //端口2相机初始化测试成功
                        {
                            bInitResult = true;
                        }
                    }

                    if (bInitResult) //新相机或新串口识别成功
                    {
                        if (Global.Camera[PowerOnCameraIndex].IsSerialPort) //当前识别串口
                        {
                            Global.SerialPortName[PowerOnCameraIndex] = sSerialPortName;
                        }
                        else  //当前识别相机
                        {
                            Global.CameraPort[PowerOnCameraIndex] = cameraName;
                            Global.SnPort[PowerOnCameraIndex] = snPort;
                            CameraTestTrue.Add(PowerOnCameraIndex);
                        }

                        Byte testValue = _GetCameraIndex((Byte)(PowerOnCameraIndex + 1));

                        if (testValue < Global.CameraNumberMax) //相机模式正常
                        {
                            PowerOnCameraIndex = testValue;

                            if (false == Global.ComputerRunState) //控制器上运行软件
                            {
                                switch (PowerOnCameraIndex)
                                {
                                    case 0:
                                        _CameraPowerOn(0);                                         //端口1相机上电
                                        break;
                                    case 1:
                                        _CameraPowerOn(2);                                         //端口1和端口2相机上电
                                        break;
                                    case 2:
                                        _CameraPowerOn(3);                                         //端口3相机上电
                                        break;
                                    case 3:
                                        _CameraPowerOn(5);                                         //端口3和端口4相机上电
                                        break;
                                    default:
                                        break;
                                }
                            }
                            DelayCount = 0;
                            CameraFlag = 0;
                            timer2.Enabled = true;//继续识别相机
                        }
                        else//相机识别结束
                        {
                            DelayCount = 0;
                            CameraFlag = 0;

                            Timer1State++;
                            timer1.Enabled = true;
                        }
                    }
                    else                                                   //端口2相机初始化测试失败
                    {
                        DelayCount = 5;

                        if (false == Global.ComputerRunState) //控制器上运行软件
                        {
                            switch (PowerOnCameraIndex)
                            {
                                case 0:
                                    lock (LockControllerSerialPort)//相机1下电
                                    {
                                        _CameraPowerOff(0);
                                    }
                                    break;
                                case 1:
                                    lock (LockControllerSerialPort)//相机2下电
                                    {
                                        _CameraPowerOff(1);
                                    }
                                    break;
                                case 2:
                                    lock (LockControllerSerialPort)//相机3下电
                                    {
                                        _CameraPowerOff(3);
                                    }
                                    break;
                                case 3:
                                    lock (LockControllerSerialPort)//相机4下电
                                    {
                                        _CameraPowerOff(4);
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        timer2.Enabled = true;//继续识别相机
                    }
                }
                else                                                       //没有识别到端口2相机
                {
                    DelayCount++;
                    if (DelayCount < 5)                                    //连续5次延时上电，识别端口2相机
                    {
                        timer2.Enabled = true;//继续识别相机
                    }
                    else if (DelayCount == 5)                              //5次延时上电后，仍未识别到端口2相机
                    {
                        if (false == Global.ComputerRunState) //控制器上运行软件
                        {
                            switch (PowerOnCameraIndex)
                            {
                                case 0:
                                    lock (LockControllerSerialPort)//相机1下电
                                    {
                                        _CameraPowerOff(0);
                                    }
                                    break;
                                case 1:
                                    lock (LockControllerSerialPort)//相机2下电
                                    {
                                        _CameraPowerOff(1);
                                    }
                                    break;
                                case 2:
                                    lock (LockControllerSerialPort)//相机3下电
                                    {
                                        _CameraPowerOff(3);
                                    }
                                    break;
                                case 3:
                                    lock (LockControllerSerialPort)//相机4下电
                                    {
                                        _CameraPowerOff(4);
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        timer2.Enabled = true;//继续识别相机
                    }
                    else                                                   //关闭端口2相机后，重新开启
                    {
                        CameraFlag++;
                        if (CameraFlag < 6)                                //端口2相机重复上电次数小于6
                        {
                            if (false == Global.ComputerRunState) //控制器上运行软件
                            {
                                switch (PowerOnCameraIndex)
                                {
                                    case 0:
                                        lock (LockControllerSerialPort)//相机1上电
                                        {
                                            _CameraPowerOn(0);
                                        }
                                        break;
                                    case 1:
                                        lock (LockControllerSerialPort)//相机2上电
                                        {
                                            _CameraPowerOn(2);
                                        }
                                        break;
                                    case 2:
                                        lock (LockControllerSerialPort)//相机3上电
                                        {
                                            _CameraPowerOn(3);
                                        }
                                        break;
                                    case 3:
                                        lock (LockControllerSerialPort)//相机4上电
                                        {
                                            _CameraPowerOn(5);
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }
                            DelayCount = 0;
                            timer2.Enabled = true;//继续识别相机
                        }
                        else                                               //端口2相机重复上电次数6后，仍未识别到
                        {
                            switch (PowerOnCameraIndex)
                            {
                                case 0:
                                    Global.MachineFaultState |= 0x02;
                                    break;
                                case 1:
                                    Global.MachineFaultState |= 0x04;
                                    break;
                                case 2:
                                    Global.MachineFaultState |= 0x010000000000;
                                    break;
                                case 3:
                                    Global.MachineFaultState |= 0x020000000000;
                                    break;
                                default:
                                    break;
                            }

                            Global.MachineFaultStateTemp = Global.MachineFaultState; //标记初始化相机故障状态

                            Byte testValue = _GetCameraIndex((Byte)(PowerOnCameraIndex + 1));

                            if (testValue < Global.CameraNumberMax) //相机模式正常
                            {
                                PowerOnCameraIndex = testValue;

                                if (false == Global.ComputerRunState) //控制器上运行软件
                                {
                                    switch (PowerOnCameraIndex)
                                    {
                                        case 0:
                                            _CameraPowerOn(0);                                         //端口1相机上电
                                            break;
                                        case 1:
                                            _CameraPowerOn(2);                                         //端口1和端口2相机上电
                                            break;
                                        case 2:
                                            _CameraPowerOn(3);                                         //端口3相机上电
                                            break;
                                        case 3:
                                            _CameraPowerOn(5);                                         //端口3和端口4相机上电
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                DelayCount = 0;
                                CameraFlag = 0;
                                timer2.Enabled = true;//继续识别相机
                            }
                            else//相机识别结束
                            {
                                switch (PowerOnCameraIndex)
                                {
                                    case 0:
                                        if (Global.Camera[PowerOnCameraIndex].IsSerialPort) //当前为串口
                                        {

                                            label1.Text = "串口1初始化失败！";
                                        }
                                        else
                                        {
                                            label1.Text = "相机1初始化失败！";
                                        }
                                        break;
                                    case 1:
                                        if (Global.Camera[PowerOnCameraIndex].IsSerialPort) //当前为串口
                                        {

                                            label1.Text = "串口2初始化失败！";
                                        }
                                        else
                                        {
                                            label1.Text = "相机2初始化失败！";
                                        }
                                        break;
                                    case 2:
                                        if (Global.Camera[PowerOnCameraIndex].IsSerialPort) //当前为串口
                                        {

                                            label1.Text = "串口3初始化失败！";
                                        }
                                        else
                                        {
                                            label1.Text = "相机3初始化失败！";
                                        }
                                        break;
                                    case 3:
                                        if (Global.Camera[PowerOnCameraIndex].IsSerialPort) //当前为串口
                                        {

                                            label1.Text = "串口4初始化失败！";
                                        }
                                        else
                                        {
                                            label1.Text = "相机4初始化失败！";
                                        }
                                        break;
                                    default:
                                        break;
                                }

                                label1.Visible = true;

                                DelayCount = 0;
                                CameraFlag = 0;

                                Timer1State++;
                                timer1.Enabled = true;
                            }
                        }
                    }
                }
            }
            else //相机模式
            {
                label1.Text = "模式选择失败！";
                label1.Visible = true;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：USB口测试数设置测试
        // 输入参数：1、VisionSystemClassLibrary.Struct.SYSTEMTIME：sSerialPortName，
        // 输出参数： 无
        // 返 回 值： Boolean：返回Com口查找结果
        //----------------------------------------------------------------------
        private Boolean _USBPortRecognise(ref String sSerialPortName)
        {
            String[] USBPortNames = SerialPort.GetPortNames();

            if (USBPortNames.Length > SerialPortNames.Count)
            {
                for (Int32 i = 0; i < USBPortNames.Length; i++)
                {
                    if (SerialPortNames.Contains(USBPortNames[i]))
                    {
                        continue;
                    }
                    else
                    {
                        SerialPortNames.Add(USBPortNames[i]);

                        try
                        {
                            SerialPortCommucation[PowerOnCameraIndex].PortName = USBPortNames[i];
                            string sBaudRate = Global.Camera[PowerOnCameraIndex].SerialPort_BaudRate.ToString();
                            SerialPortCommucation[PowerOnCameraIndex].BaudRate = Convert.ToInt32(sBaudRate.Substring(1, sBaudRate.Length - 1));
                            SerialPortCommucation[PowerOnCameraIndex].ReceivedBytesThreshold = Global.Camera[PowerOnCameraIndex].SerialPort_ReceivedBytesThreshold;
                            SerialPortCommucation[PowerOnCameraIndex].Open();

                            lock (LockSerialPort[PowerOnCameraIndex])
                            {
                                _SendLabModel(PowerOnCameraIndex);
                            }
                            
                            switch (PowerOnCameraIndex)
                            {
                                case 0:
                                    SerialPortCommucation[PowerOnCameraIndex].DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(_SerialPortCommucation1_DataReceived);
                                    break;
                                case 1:
                                    SerialPortCommucation[PowerOnCameraIndex].DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(_SerialPortCommucation2_DataReceived);
                                    break;
                                case 2:
                                    SerialPortCommucation[PowerOnCameraIndex].DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(_SerialPortCommucation3_DataReceived);
                                    break;
                                case 3:
                                    SerialPortCommucation[PowerOnCameraIndex].DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(_SerialPortCommucation4_DataReceived);
                                    break;
                                default:
                                    break;
                            }

                            sSerialPortName = SerialPortCommucation[PowerOnCameraIndex].PortName;

                            CommunicationFlag[PowerOnCameraIndex] = true;

                            lock (LockSerialPort[PowerOnCameraIndex])
                            {
                                _SendAdjustValue_SerialPortCommucation(PowerOnCameraIndex);  //发送传感器校准值命令
                            }
                            
                            if (VisionSystemClassLibrary.Enum.SensorProductType._89713FA == Global.Camera[PowerOnCameraIndex].Sensor_ProductType)
                            {
                                lock(LockSerialPort[PowerOnCameraIndex])
                                {
                                    for (Byte j = 0; j < Global.Camera[PowerOnCameraIndex].PerTobaccoNumber.Count; j++) 
                                    {
                                        _SendEcPhase_SerialPortCommucation(PowerOnCameraIndex, j);//发送光电各烟支检测相位及区间
                                    }
                                }
                            }

                            lock (LockSerialPort[PowerOnCameraIndex])
                            {
                                _SendSnCheck_SerialPortCommucation(PowerOnCameraIndex);   //发送串号查询命令
                            }

                            return true;
                        }
                        catch (System.Exception ex)
                        {
                            sSerialPortName = "";
                            return false;
                        }
                    }
                }
            }
            sSerialPortName = "";
            return false;
        }

        //-----------------------------------------------------------------------
        // 功能说明：串口1接收中断函数，读串口数据
        // 输入参数：1、object：sender，serialPort控件对象
        //           2、System.IO.Ports.SerialDataReceivedEventArgs：e，serialport控件事件响应参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _SerialPortCommucation1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            Byte DataType;
            UInt16 DataCount;

            while (SerialPortCommucation[0].BytesToRead >= Global.Camera[0].SerialPort_ReceivedBytesThreshold) //串口1读取字节长度大于等于16
            {
                Byte[] ReceiveData = new Byte[Global.Camera[0].SerialPort_ReceivedBytesThreshold]; //保存串口接收数据
                Byte ReceiveChecksum = 0;                        　　　　　　　　        //保存串口接收校验和
                SerialPortCommucation[0].Read(ReceiveData, 0, Global.Camera[0].SerialPort_ReceivedBytesThreshold);
                DataType = ReceiveData[0];
                DataCount = (UInt16)(ReceiveData[1] + 2);

                for (UInt16 i = 0; i < DataCount; i++)                         //读取串口数据，计算校验数据
                {
                    ReceiveChecksum ^= ReceiveData[i];
                }

                if (ReceiveChecksum == ReceiveData[DataCount])                 //校验和相同
                {
                    CommunicationFlag[0] = true;

                    switch (DataType)                                          //接收下位机命令
                    {
                        case 20:
                            Global.SerialPortSn[0] = "";
                            for (Byte i = 2; i < 14; i++) //读取串口序列号
                            {
                                Global.SerialPortSn[0] += ReceiveData[i].ToString();
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：串口2接收中断函数，读串口数据
        // 输入参数：1、object：sender，serialPort控件对象
        //           2、System.IO.Ports.SerialDataReceivedEventArgs：e，serialport控件事件响应参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _SerialPortCommucation2_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            Byte DataType;
            UInt16 DataCount;

            while (SerialPortCommucation[1].BytesToRead >= Global.Camera[1].SerialPort_ReceivedBytesThreshold) //串口1读取字节长度大于等于16
            {
                Byte[] ReceiveData = new Byte[Global.Camera[1].SerialPort_ReceivedBytesThreshold]; //保存串口接收数据
                Byte ReceiveChecksum = 0;                        　　　　　　　　        //保存串口接收校验和
                SerialPortCommucation[1].Read(ReceiveData, 0, Global.Camera[1].SerialPort_ReceivedBytesThreshold);
                DataType = ReceiveData[0];
                DataCount = (UInt16)(ReceiveData[1] + 2);

                for (UInt16 i = 0; i < DataCount; i++)                         //读取串口数据，计算校验数据
                {
                    ReceiveChecksum ^= ReceiveData[i];
                }

                if (ReceiveChecksum == ReceiveData[DataCount])                 //校验和相同
                {
                    CommunicationFlag[1] = true;

                    switch (DataType)                                          //接收下位机命令
                    {
                        case 20:
                            Global.SerialPortSn[1] = "";
                            for (Byte i = 2; i < 14; i++) //读取串口序列号
                            {
                                Global.SerialPortSn[1] += ReceiveData[i].ToString();
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }


        //-----------------------------------------------------------------------
        // 功能说明：串口3接收中断函数，读串口数据
        // 输入参数：1、object：sender，serialPort控件对象
        //           2、System.IO.Ports.SerialDataReceivedEventArgs：e，serialport控件事件响应参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _SerialPortCommucation3_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            Byte DataType;
            UInt16 DataCount;

            while (SerialPortCommucation[2].BytesToRead >= Global.Camera[2].SerialPort_ReceivedBytesThreshold) //串口1读取字节长度大于等于16
            {
                Byte[] ReceiveData = new Byte[Global.Camera[2].SerialPort_ReceivedBytesThreshold]; //保存串口接收数据
                Byte ReceiveChecksum = 0;                        　　　　　　　　        //保存串口接收校验和
                SerialPortCommucation[2].Read(ReceiveData, 0, Global.Camera[2].SerialPort_ReceivedBytesThreshold);
                DataType = ReceiveData[0];
                DataCount = (UInt16)(ReceiveData[1] + 2);

                for (UInt16 i = 0; i < DataCount; i++)                         //读取串口数据，计算校验数据
                {
                    ReceiveChecksum ^= ReceiveData[i];
                }

                if (ReceiveChecksum == ReceiveData[DataCount])                 //校验和相同
                {
                    CommunicationFlag[2] = true;

                    switch (DataType)                                          //接收下位机命令
                    {
                        case 20:
                            Global.SerialPortSn[2] = "";
                            for (Byte i = 2; i < 14; i++) //读取串口序列号
                            {
                                Global.SerialPortSn[2] += ReceiveData[i].ToString();
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：串口4接收中断函数，读串口数据
        // 输入参数：1、object：sender，serialPort控件对象
        //           2、System.IO.Ports.SerialDataReceivedEventArgs：e，serialport控件事件响应参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _SerialPortCommucation4_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            Byte DataType;
            UInt16 DataCount;

            while (SerialPortCommucation[3].BytesToRead >= Global.Camera[3].SerialPort_ReceivedBytesThreshold) //串口1读取字节长度大于等于16
            {
                Byte[] ReceiveData = new Byte[Global.Camera[3].SerialPort_ReceivedBytesThreshold]; //保存串口接收数据
                Byte ReceiveChecksum = 0;                        　　　　　　　　        //保存串口接收校验和
                SerialPortCommucation[3].Read(ReceiveData, 0, Global.Camera[3].SerialPort_ReceivedBytesThreshold);
                DataType = ReceiveData[0];
                DataCount = (UInt16)(ReceiveData[1] + 2);

                for (UInt16 i = 0; i < DataCount; i++)                         //读取串口数据，计算校验数据
                {
                    ReceiveChecksum ^= ReceiveData[i];
                }

                if (ReceiveChecksum == ReceiveData[DataCount])                 //校验和相同
                {
                    CommunicationFlag[3] = true;

                    switch (DataType)                                          //接收下位机命令
                    {
                        case 20:
                            Global.SerialPortSn[3] = "";
                            for (Byte i = 2; i < 14; i++) //读取串口序列号
                            {
                                Global.SerialPortSn[3] += ReceiveData[i].ToString();
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：发送实验室状态命令
        // 输入参数： 1、Byte：index，相机类型
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _SendLabModel(Byte index)
        {
            Byte[] Command = new Byte[Global.Camera[index].SerialPort_SendBytesThreshold];//保存串口发送指令

            Command[0] = 1;
            Command[1] = 1;
            Command[2] = 1; //向下位机发送实验室状态
            Command[3] = (Byte)(Command[0] ^ Command[1] ^ Command[2]);

            if (SerialPortCommucation[index].IsOpen == true) //当前串口1打开，向下位发送命令
            {
                SerialPortCommucation[index].Write(Command, 0, 4);
            }
        }
        
        //-----------------------------------------------------------------------
        // 功能说明：发送传感器校准值命令
        // 输入参数： 1、Byte：index，相机类型
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _SendAdjustValue_SerialPortCommucation(Byte index)
        {
            Byte[] Command = new Byte[Global.Camera[index].SerialPort_SendBytesThreshold];//保存串口发送指令
            Int32 iCheckCount = 0;

            switch (Global.Camera[index].Sensor_ProductType)
            {
                case VisionSystemClassLibrary.Enum.SensorProductType._89713FC:
                    Command[0] = 5;
                    Command[1] = 21;

                    Command[2] = (Byte)Global.Camera[index].TobaccoSortType_E;//0表示767；1表示677；2表示1010；3表示99；4表示88；5表示77；6表示66；6表示55

                    for (Int32 i = 0; i < Global.Camera[index].SensorNumber; i++) //循环所有烟支
                    {
                        Command[3 + i] = Global.Camera[index].DeviceParameter.SensorAdjustValue[i];
                    }

                    iCheckCount = Command[1] + 2;
                    break;

                case VisionSystemClassLibrary.Enum.SensorProductType._89713FA:
                    Command[0] = 5;
                    Command[1] = 15;

                    Command[2] = (Byte)Global.Camera[index].PerTobaccoNumber.Count;//烟支排数

                    for (Int32 i = 0; i < Global.Camera[index].SensorNumber; i++) //循环所有烟支
                    {
                        Command[3 + i] = Global.Camera[index].DeviceParameter.SensorAdjustValue[i];
                    }

                    if (null != Global.Camera[index].DeviceParameter.Parameter) //参数有效
                    {
                        for (Int32 i = 0; i < 4; i++) //循环所有烟支
                        {
                            Command[i * 2 + 8] = Convert.ToByte(Global.Camera[index].DeviceParameter.Parameter[i] * Global.Camera[index].EncoderPer & 0xFF);//相机曝光相位低8位；
                            Command[i * 2 + 9] = Convert.ToByte((Global.Camera[index].DeviceParameter.Parameter[i] * Global.Camera[index].EncoderPer >> 8) & 0xFF);//相机曝光相位高8位；
                        }
                    }
                    Command[16] = Convert.ToByte(Global.Camera[index].DeviceParameter.Parameter[4] & 0xFF);//校准目标电压

                    iCheckCount = Command[1] + 2;

                    break;

                case VisionSystemClassLibrary.Enum.SensorProductType._89750A:
                    break;
                default:
                    break;
            }

            for (Byte i = 0; i < iCheckCount; i++)
            {
                Command[iCheckCount] ^= Command[i];
            }

            for (Byte i = 0; i < 10; i++) //连续向下位机发送10次命令
            {
                if (!SerialPortCommucation[index].IsOpen)  //当前端口未打开
                {
                    SerialPortCommucation[index].Open();
                }
                SerialPortCommucation[index].Write(Command, 0, iCheckCount + 1);
                CommunicationFlag[index] = false;
                Thread.Sleep(1000);
                if (CommunicationFlag[index] == true)                                 //向下位机发送命令成功
                {
                    break;
                }
            }

            if (CommunicationFlag[index] == false)//下位机通讯故障
            {
                switch (PowerOnCameraIndex)
                {
                    case 0:
                        label1.Text = "串口1发送校准值失败！";
                        break;
                    case 1:
                        label1.Text = "串口2发送校准值失败！";
                        break;
                    case 2:
                        label1.Text = "串口3发送校准值失败！";
                        break;
                    case 3:
                        label1.Text = "串口4发送校准值失败！";
                        break;
                    default:
                        break;
                }
                label1.Visible = true;

                return;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：发送光电各烟支检测相位及区间
        // 输入参数： 1、Byte：index，相机类型
        //                     2、Byte：iPerTobaccoIndex，烟支排数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _SendEcPhase_SerialPortCommucation(Byte index, Byte iPerTobaccoIndex)
        {
            if ((null != Global.Camera[index].DeviceParameter.Parameter) && (Global.Camera[index].DeviceParameter.Parameter.Count > 3)) //连续采样数据有效
            {
                Byte[] Command = new Byte[Global.Camera[index].SerialPort_SendBytesThreshold];//保存串口发送指令
                Int32 iCheckCount = 0;

                Int32 iStartPhase = Global.Camera[index].EncoderPer * Global.Camera[index].DeviceParameter.Parameter[2];
                Int32 iEndPhase = Global.Camera[index].EncoderPer * Global.Camera[index].DeviceParameter.Parameter[3] + 1;

                double dPresion = (double)Global.Camera[index].ImageWidth / ((iEndPhase + 1800 - iStartPhase) % 1800);

                Command[0] = 6;
                Command[1] = (Byte)(Global.Camera[index].PerTobaccoNumber[iPerTobaccoIndex] * 3 + 1);
                Command[2] = iPerTobaccoIndex;//光电管索引

                for (Int32 i = 0; i < Global.Camera[index].PerTobaccoNumber[iPerTobaccoIndex]; i++) //循环当前排烟支
                {
                    Rectangle rect = VisionSystemClassLibrary.GeneralFunction._GetMinRect(Global.Camera[index].Tools[i].ROI.roiExtra);
                    UInt16 uiPhase = Convert.ToUInt16(((double)rect.Left + (double)rect.Width / 2) / dPresion + iStartPhase);
                    Command[3 * i + 3] = Convert.ToByte(0xFF & uiPhase);
                    Command[3 * i + 4] = Convert.ToByte(0xFF & (uiPhase >> 8));
                    Command[3 * i + 5] = Convert.ToByte((double)rect.Width / 2 / dPresion);
                }

                iCheckCount = Command[1] + 2;

                for (Byte i = 0; i < iCheckCount; i++)
                {
                    Command[iCheckCount] ^= Command[i];
                }

                for (Byte i = 0; i < 10; i++) //连续向下位机发送10次命令
                {
                    if (!SerialPortCommucation[index].IsOpen)  //当前端口未打开
                    {
                        SerialPortCommucation[index].Open();
                    }
                    SerialPortCommucation[index].Write(Command, 0, iCheckCount + 1);
                    CommunicationFlag[index] = false;
                    Thread.Sleep(1000);
                    if (CommunicationFlag[index] == true)                                 //向下位机发送命令成功
                    {
                        break;
                    }
                }
            }

            if (CommunicationFlag[index] == false)//下位机通讯故障
            {
                switch (PowerOnCameraIndex)
                {
                    case 0:
                        label1.Text = "串口1发送检测相位和区间失败！";
                        break;
                    case 1:
                        label1.Text = "串口2发送检测相位和区间失败！";
                        break;
                    case 2:
                        label1.Text = "串口3发送检测相位和区间失败！";
                        break;
                    case 3:
                        label1.Text = "串口4发送检测相位和区间失败！";
                        break;
                    default:
                        break;
                }
                label1.Visible = true;

                return;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：发送传感器校准值命令
        // 输入参数： 1、Byte：index，相机类型
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _SendSnCheck_SerialPortCommucation(Byte index)
        {
            Byte[] Command = new Byte[Global.Camera[index].SerialPort_SendBytesThreshold];//保存串口发送指令

            Command[0] = 20;
            Command[1] = 0;

            Command[2] = (Byte)(Command[0] ^ Command[1]);

            for (Byte i = 0; i < 10; i++) //连续向下位机发送10次命令
            {
                if (!SerialPortCommucation[index].IsOpen)  //当前端口未打开
                {
                    SerialPortCommucation[index].Open();
                }
                SerialPortCommucation[index].Write(Command, 0, 3);
                CommunicationFlag[index] = false;
                Thread.Sleep(1000);
                if (CommunicationFlag[index] == true)                                 //向下位机发送命令成功
                {
                    break;
                }
            }

            if (CommunicationFlag[index] == false)//下位机通讯故障
            {
                switch (PowerOnCameraIndex)
                {
                    case 0:
                        label1.Text = "串口1串号查询寻失败！";
                        break;
                    case 1:
                        label1.Text = "串口2串号查询寻失败！";
                        break;
                    case 2:
                        label1.Text = "串口3串号查询寻失败！";
                        break;
                    case 3:
                        label1.Text = "串口4串号查询寻失败！";
                        break;
                    default:
                        break;
                }
                label1.Visible = true;

                return;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：初始化烟支坐标信息
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _InitTobaccoPosion()
        {
            //初始化烟支位置信息
            Global.TobaccoPosionInfo = new Rectangle[2][][];
            Int32 iTobaccoSortTypeCount = System.Enum.GetValues(typeof(VisionSystemClassLibrary.Enum.TobaccoSortType)).Length;
            for (Byte k = 0; k < Global.TobaccoPosionInfo.Length; k++)
            {
                Global.TobaccoPosionInfo[k] = new Rectangle[iTobaccoSortTypeCount][];

                for (Int32 i = 1; i <= Global.TobaccoPosionInfo[k].Length; i++) //循环所有烟支排列类型
                {
                    Int32 iDiltaX = 104;
                    Int32 iDiltaY = 125;
                    Int32 iWidth = 30;
                    Int32 iHalfTobaccoPosionInfoLen = 10;

                    if (0 == k) //FOCKE排列
                    {
                        switch ((VisionSystemClassLibrary.Enum.TobaccoSortType)i) //排列类型
                        {
                            case VisionSystemClassLibrary.Enum.TobaccoSortType._767:

                                Global.TobaccoPosionInfo[k][i - 1] = new Rectangle[20];

                                for (Int32 j = 0; j < Global.TobaccoPosionInfo[k][i - 1].Length; j++) //循环所有烟支
                                {
                                    if (j < 7)//第一排
                                    {
                                        Global.TobaccoPosionInfo[k][i - 1][j].X = 60 + iDiltaX * j;
                                        Global.TobaccoPosionInfo[k][i - 1][j].Y = 105;
                                        Global.TobaccoPosionInfo[k][i - 1][j].Width = Global.TobaccoPosionInfo[k][i - 1][j].Height = iWidth;
                                    }
                                    else if (j < 13)//第二排
                                    {
                                        Global.TobaccoPosionInfo[k][i - 1][j].X = 112 + iDiltaX * (j - 7);
                                        Global.TobaccoPosionInfo[k][i - 1][j].Y = 105 + iDiltaY;
                                        Global.TobaccoPosionInfo[k][i - 1][j].Width = Global.TobaccoPosionInfo[k][i - 1][j].Height = iWidth;
                                    }
                                    else//第三排
                                    {
                                        Global.TobaccoPosionInfo[k][i - 1][j].X = 60 + iDiltaX * (j - 13);
                                        Global.TobaccoPosionInfo[k][i - 1][j].Y = 105 + iDiltaY + iDiltaY;
                                        Global.TobaccoPosionInfo[k][i - 1][j].Width = Global.TobaccoPosionInfo[k][i - 1][j].Height = iWidth;
                                    }
                                }
                                break;
                            case VisionSystemClassLibrary.Enum.TobaccoSortType._776:

                                Global.TobaccoPosionInfo[k][i - 1] = new Rectangle[20];
                                for (Int32 j = 0; j < Global.TobaccoPosionInfo[k][i - 1].Length; j++) //循环所有烟支
                                {
                                    if (j < 6)//第一排
                                    {
                                        Global.TobaccoPosionInfo[k][i - 1][j].X = 112 + iDiltaX * j;
                                        Global.TobaccoPosionInfo[k][i - 1][j].Y = 105;
                                        Global.TobaccoPosionInfo[k][i - 1][j].Width = Global.TobaccoPosionInfo[k][i - 1][j].Height = iWidth;
                                    }
                                    else if (j < 13)//第二排
                                    {
                                        Global.TobaccoPosionInfo[k][i - 1][j].X = 60 + iDiltaX * (j - 6);
                                        Global.TobaccoPosionInfo[k][i - 1][j].Y = 105 + iDiltaY;
                                        Global.TobaccoPosionInfo[k][i - 1][j].Width = Global.TobaccoPosionInfo[k][i - 1][j].Height = iWidth;
                                    }
                                    else//第三排
                                    {
                                        Global.TobaccoPosionInfo[k][i - 1][j].X = 60 + iDiltaX * (j - 13);
                                        Global.TobaccoPosionInfo[k][i - 1][j].Y = 105 + iDiltaY + iDiltaY;
                                        Global.TobaccoPosionInfo[k][i - 1][j].Width = Global.TobaccoPosionInfo[k][i - 1][j].Height = iWidth;
                                    }
                                }
                                break;
                            case VisionSystemClassLibrary.Enum.TobaccoSortType._1010:

                                iDiltaX = 70;
                                iDiltaY = 150;
                                Global.TobaccoPosionInfo[k][i - 1] = new Rectangle[20];
                                iHalfTobaccoPosionInfoLen = Global.TobaccoPosionInfo[k][i - 1].Length / 2;
                                for (Int32 j = 0; j < Global.TobaccoPosionInfo[k][i - 1].Length; j++) //循环所有烟支
                                {
                                    if (j < iHalfTobaccoPosionInfoLen)//第一排
                                    {
                                        Global.TobaccoPosionInfo[k][i - 1][j].X = 57 + iDiltaX * j;
                                        Global.TobaccoPosionInfo[k][i - 1][j].Y = 149;
                                        Global.TobaccoPosionInfo[k][i - 1][j].Width = Global.TobaccoPosionInfo[k][i - 1][j].Height = iWidth;
                                    }
                                    else//第二排
                                    {
                                        Global.TobaccoPosionInfo[k][i - 1][j].X = 57 + iDiltaX * (j - iHalfTobaccoPosionInfoLen);
                                        Global.TobaccoPosionInfo[k][i - 1][j].Y = 149 + iDiltaY;
                                        Global.TobaccoPosionInfo[k][i - 1][j].Width = Global.TobaccoPosionInfo[k][i - 1][j].Height = iWidth;
                                    }
                                }
                                break;
                            case VisionSystemClassLibrary.Enum.TobaccoSortType._99:

                                iDiltaX = 80;
                                iDiltaY = 150;
                                Global.TobaccoPosionInfo[k][i - 1] = new Rectangle[18];
                                iHalfTobaccoPosionInfoLen = Global.TobaccoPosionInfo[k][i - 1].Length / 2;
                                for (Int32 j = 0; j < Global.TobaccoPosionInfo[k][i - 1].Length; j++) //循环所有烟支
                                {
                                    if (j < iHalfTobaccoPosionInfoLen)//第一排
                                    {
                                        Global.TobaccoPosionInfo[k][i - 1][j].X = 52 + iDiltaX * j;
                                        Global.TobaccoPosionInfo[k][i - 1][j].Y = 149;
                                        Global.TobaccoPosionInfo[k][i - 1][j].Width = Global.TobaccoPosionInfo[k][i - 1][j].Height = iWidth;
                                    }
                                    else//第二排
                                    {
                                        Global.TobaccoPosionInfo[k][i - 1][j].X = 52 + iDiltaX * (j - iHalfTobaccoPosionInfoLen);
                                        Global.TobaccoPosionInfo[k][i - 1][j].Y = 149 + iDiltaY;
                                        Global.TobaccoPosionInfo[k][i - 1][j].Width = Global.TobaccoPosionInfo[k][i - 1][j].Height = iWidth;
                                    }
                                }
                                break;
                            case VisionSystemClassLibrary.Enum.TobaccoSortType._88:

                                iDiltaX = 80;
                                iDiltaY = 150;
                                Global.TobaccoPosionInfo[k][i - 1] = new Rectangle[16];
                                iHalfTobaccoPosionInfoLen = Global.TobaccoPosionInfo[k][i - 1].Length / 2;
                                for (Int32 j = 0; j < Global.TobaccoPosionInfo[k][i - 1].Length; j++) //循环所有烟支
                                {
                                    if (j < iHalfTobaccoPosionInfoLen)//第一排
                                    {
                                        Global.TobaccoPosionInfo[k][i - 1][j].X = 92 + iDiltaX * j;
                                        Global.TobaccoPosionInfo[k][i - 1][j].Y = 149;
                                        Global.TobaccoPosionInfo[k][i - 1][j].Width = Global.TobaccoPosionInfo[k][i - 1][j].Height = iWidth;
                                    }
                                    else//第二排
                                    {
                                        Global.TobaccoPosionInfo[k][i - 1][j].X = 92 + iDiltaX * (j - iHalfTobaccoPosionInfoLen);
                                        Global.TobaccoPosionInfo[k][i - 1][j].Y = 149 + iDiltaY;
                                        Global.TobaccoPosionInfo[k][i - 1][j].Width = Global.TobaccoPosionInfo[k][i - 1][j].Height = iWidth;
                                    }
                                }
                                break;
                            case VisionSystemClassLibrary.Enum.TobaccoSortType._77:

                                iDiltaX = 80;
                                iDiltaY = 150;
                                Global.TobaccoPosionInfo[k][i - 1] = new Rectangle[14];
                                iHalfTobaccoPosionInfoLen = Global.TobaccoPosionInfo[k][i - 1].Length / 2;
                                for (Int32 j = 0; j < Global.TobaccoPosionInfo[k][i - 1].Length; j++) //循环所有烟支
                                {
                                    if (j < iHalfTobaccoPosionInfoLen)//第一排
                                    {
                                        Global.TobaccoPosionInfo[k][i - 1][j].X = 132 + iDiltaX * j;
                                        Global.TobaccoPosionInfo[k][i - 1][j].Y = 149;
                                        Global.TobaccoPosionInfo[k][i - 1][j].Width = Global.TobaccoPosionInfo[k][i - 1][j].Height = iWidth;
                                    }
                                    else//第二排
                                    {
                                        Global.TobaccoPosionInfo[k][i - 1][j].X = 132 + iDiltaX * (j - iHalfTobaccoPosionInfoLen);
                                        Global.TobaccoPosionInfo[k][i - 1][j].Y = 149 + iDiltaY;
                                        Global.TobaccoPosionInfo[k][i - 1][j].Width = Global.TobaccoPosionInfo[k][i - 1][j].Height = iWidth;
                                    }
                                }
                                break;
                            case VisionSystemClassLibrary.Enum.TobaccoSortType._66:

                                iDiltaX = 80;
                                iDiltaY = 150;
                                Global.TobaccoPosionInfo[k][i - 1] = new Rectangle[12];
                                iHalfTobaccoPosionInfoLen = Global.TobaccoPosionInfo[k][i - 1].Length / 2;
                                for (Int32 j = 0; j < Global.TobaccoPosionInfo[k][i - 1].Length; j++) //循环所有烟支
                                {
                                    if (j < iHalfTobaccoPosionInfoLen)//第一排
                                    {
                                        Global.TobaccoPosionInfo[k][i - 1][j].X = 172 + iDiltaX * j;
                                        Global.TobaccoPosionInfo[k][i - 1][j].Y = 149;
                                        Global.TobaccoPosionInfo[k][i - 1][j].Width = Global.TobaccoPosionInfo[k][i - 1][j].Height = iWidth;
                                    }
                                    else//第二排
                                    {
                                        Global.TobaccoPosionInfo[k][i - 1][j].X = 172 + iDiltaX * (j - iHalfTobaccoPosionInfoLen);
                                        Global.TobaccoPosionInfo[k][i - 1][j].Y = 149 + iDiltaY;
                                        Global.TobaccoPosionInfo[k][i - 1][j].Width = Global.TobaccoPosionInfo[k][i - 1][j].Height = iWidth;
                                    }
                                }
                                break;
                            case VisionSystemClassLibrary.Enum.TobaccoSortType._55:

                                iDiltaX = 80;
                                iDiltaY = 150;
                                Global.TobaccoPosionInfo[k][i - 1] = new Rectangle[10];
                                iHalfTobaccoPosionInfoLen = Global.TobaccoPosionInfo[k][i - 1].Length / 2;
                                for (Int32 j = 0; j < Global.TobaccoPosionInfo[k][i - 1].Length; j++) //循环所有烟支
                                {
                                    if (j < iHalfTobaccoPosionInfoLen)//第一排
                                    {
                                        Global.TobaccoPosionInfo[k][i - 1][j].X = 212 + iDiltaX * j;
                                        Global.TobaccoPosionInfo[k][i - 1][j].Y = 149;
                                        Global.TobaccoPosionInfo[k][i - 1][j].Width = Global.TobaccoPosionInfo[k][i - 1][j].Height = iWidth;
                                    }
                                    else//第二排
                                    {
                                        Global.TobaccoPosionInfo[k][i - 1][j].X = 212 + iDiltaX * (j - iHalfTobaccoPosionInfoLen);
                                        Global.TobaccoPosionInfo[k][i - 1][j].Y = 149 + iDiltaY;
                                        Global.TobaccoPosionInfo[k][i - 1][j].Width = Global.TobaccoPosionInfo[k][i - 1][j].Height = iWidth;
                                    }
                                }
                                break;
                            case VisionSystemClassLibrary.Enum.TobaccoSortType._11:

                                iDiltaX = 248;
                                Global.TobaccoPosionInfo[k][i - 1] = new Rectangle[2];
                                for (Int32 j = 0; j < Global.TobaccoPosionInfo[k][i - 1].Length; j++) //循环所有烟支
                                {
                                    Global.TobaccoPosionInfo[k][i - 1][j].X = 248 + iDiltaX * j;
                                    Global.TobaccoPosionInfo[k][i - 1][j].Y = 105 + iDiltaY;
                                    Global.TobaccoPosionInfo[k][i - 1][j].Width = Global.TobaccoPosionInfo[k][i - 1][j].Height = iWidth;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    else //GD排列
                    {
                        switch ((VisionSystemClassLibrary.Enum.TobaccoSortType)i) //排列类型
                        {
                            case VisionSystemClassLibrary.Enum.TobaccoSortType._767:
                            case VisionSystemClassLibrary.Enum.TobaccoSortType._776:

                                iDiltaX = 186;
                                Global.TobaccoPosionInfo[k][i - 1] = new Rectangle[3];
                                for (Int32 j = 0; j < Global.TobaccoPosionInfo[k][i - 1].Length; j++) //循环所有烟支
                                {
                                    Global.TobaccoPosionInfo[k][i - 1][j].X = 186 + iDiltaX * j;
                                    Global.TobaccoPosionInfo[k][i - 1][j].Y = 105 + iDiltaY;
                                    Global.TobaccoPosionInfo[k][i - 1][j].Width = Global.TobaccoPosionInfo[k][i - 1][j].Height = iWidth;
                                }
                                break;

                            case VisionSystemClassLibrary.Enum.TobaccoSortType._1010:
                            case VisionSystemClassLibrary.Enum.TobaccoSortType._99:
                            case VisionSystemClassLibrary.Enum.TobaccoSortType._88:
                            case VisionSystemClassLibrary.Enum.TobaccoSortType._77:
                            case VisionSystemClassLibrary.Enum.TobaccoSortType._66:
                            case VisionSystemClassLibrary.Enum.TobaccoSortType._55:
                            case VisionSystemClassLibrary.Enum.TobaccoSortType._11:

                                iDiltaX = 248;
                                Global.TobaccoPosionInfo[k][i - 1] = new Rectangle[2];
                                for (Int32 j = 0; j < Global.TobaccoPosionInfo[k][i - 1].Length; j++) //循环所有烟支
                                {
                                    Global.TobaccoPosionInfo[k][i - 1][j].X = 248 + iDiltaX * j;
                                    Global.TobaccoPosionInfo[k][i - 1][j].Y = 105 + iDiltaY;
                                    Global.TobaccoPosionInfo[k][i - 1][j].Width = Global.TobaccoPosionInfo[k][i - 1][j].Height = iWidth;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
    }
}