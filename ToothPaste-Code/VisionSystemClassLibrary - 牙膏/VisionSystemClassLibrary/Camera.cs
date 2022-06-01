/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：Camera.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：相机

原作者：视觉检测团队
完成日期：2014/08/18
特别说明：经视觉检测团队授权并遵守代码使用条款，方可使用本代码并获得技术支持，否则所产生的一切后果由使用者承担

修改者：无
完成日期：无
修改记录：无

****************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.IO;

using Emgu.CV;
using Emgu.CV.Structure;

using System.Runtime.Serialization.Formatters.Binary;

namespace VisionSystemClassLibrary.Class
{
    [Serializable]
    public class Camera
    {
        private Enum.CameraType cType;//相机类型

        private string sName = "";//相机名称

        private string sCameraENGName = "";//相机英文名
        private string sCameraCHNName = "";//相机中文名

        private string sControllerENGName = "";//相机所属控制器英文名
        private string sControllerCHNName = "";//相机所属控制器中文名

        //

        private string sConfigDataPath = "";//配置文件路径（如，D:\\VisionSystemUserInterface\\ConfigData\\）
        private string sDataPath = "";//相机数据路径（如，D:\\VisionSystemUserInterface\\ConfigData\\Top\\）
        private string sBackupImagesPath = "";//备份图像路径（如，D:\\VisionSystemUserInterface\\ConfigData\\Top\\RejectsImage\\）
        private string sReceivedDataPath = "";//接收文件路径（如，D:\\VisionSystemUserInterface\\ConfigData\\Top\\ReceivedData\\）
        private string sSampleImagePath = "";//学习图像路径（如，D:\\VisionSystemUserInterface\\ConfigData\\Top\\SampleImage\\）

        //

        private static string CameraFileName = "Camera.dat";//相机数据文件名称
        private static string FaultStaticsFileName = "FaultStatistics.dat";//故障统计文件名称（Controller）
        private static string sToolFileName = "Tool.dat";//工具数据文件名称
        private static string sTolerancesFileName = "Tolerances.dat";//公差数据文件名称
        private static string sParameterFileName = "Parameter.dat";//相机参数数据文件名称
        private static string sSampleDataFileName = "Sample.dat";//学习图像数据文件名称
        private static string sSampleImageFileName = "Sample";//学习图像文件名称
        private static string sClassesFile = "classes.txt";//深度学习模型文件
        private static string sModelFileName = "classify.dat";//深度学习模型文件

        private static string sDatFile = ".dat";//.dat扩展名
        private static string sJPGFile = ".JPG";//.JPG扩展名
        private static string sPNGFile = ".PNG";//.PNG扩展名
        private static string sBMPFile = ".BMP";//.BMP扩展名

        private static string sReceivedDataPathName = "ReceivedData\\";//接收文件路径
        private static string sSampleImagePathName = "SampleImage\\";//学习图像路径

        //
        private Image<Bgr, Byte> iImageLive;//实时图像
        private Struct.LiveData lLive = new Struct.LiveData();

        private Image<Bgr, Byte> iImageReject;
        private Struct.RejectsData rRejects = new Struct.RejectsData();//剔除图像

        private Image<Bgr, Byte> iImageLearn;
        private Struct.ImageInformation lLearn = new Struct.ImageInformation();//学习图像

        private List<Tools> tools;//属性，相机的工具

        private TolerancesData tolerances;//属性，TolerancesData

        private Struct.DeviceData dDeviceInformation = new Struct.DeviceData();//属性，设备信息

        private Struct.FaultMessage fCurrentFaultMessage;//当前相机故障
        private Struct.FaultMessage[] fFaultMessages;//相机故障信息
        private UInt64 uiCameraFaultState;//相机故障标记

        private Struct.CameraParameter cDeviceParameter = new Struct.CameraParameter();//相机设备参数

        //

        private Struct.Camera_UIParameter cUIParameter = new Struct.Camera_UIParameter();//数据

        //

        private Int32 ReferenceHorizenL;//水平基准左边缘值
        private Int32 ReferenceHorizenR;//水平基准右边缘值
        private Int32 ReferenceVerticalL;//垂直基准左边缘值
        private Int32 ReferenceVerticalR;//垂直基准右边缘值

        private Int32 ReferenceHorizenL_Sample;//自学习水平基准左边缘值
        private Int32 ReferenceHorizenR_Sample;//自学习水平基准右边缘值
        private Int32 ReferenceVerticalL_Sample;//自学习垂直基准左边缘值
        private Int32 ReferenceVerticalR_Sample;//自学习垂直基准右边缘值

        private Int32 ReferenceHorizenPoint;//基准定位点水平基准坐标
        private Int32 ReferenceVerticalPoint;//基准定位点垂直基准标准
        
        //

        private Boolean bCheckEnable;//相机检测使能标记
        private Enum.CameraRotateAngle bCameraAngle;//相机旋转角度。0:0度;1:90度;2:180度;3:270度（控制器程序使用）

        private Boolean bBitmapLockBitsResize;//原始图像数据截取区域缩放（控制器程序使用）
        private Boolean bBitmapLockBitsCenter;//原始图像数据截取区域缩放后是否居中（控制器程序使用）
        private Point pBitmapLockBitsAxis;//原始图像数据截取区域粘贴位置（控制器程序使用）
        private Rectangle rBitmapLockBitsArea;//原始图像数据截取区域（控制器程序使用）

        private Boolean bSerialPort;//是否为串口
        private Enum.TobaccoSortType tTobaccoSortType_E;//烟支排列类型

        private Enum.VideoColor vVideoColor;//相机颜色
        private Enum.VideoResolution vVideoResolution;//相机分辨率

        private Enum.CameraFlip bCameraFlip;//相机镜像

        private string sVideoFormat;//相机分辨率

        private Byte bSensorNumber;//传感器数量

        private Struct.RelevancyCameraInformation dRelevancyCameraInfo;//关联相机信息

        private Enum.SensorProductType eSensor_ProductType;//传感器应用产品类型

        private Boolean bContinuousSampling;//传感器连续采集，需要采集开始/结束相位
        private Byte bEncoderPer;//相位步长

        private Enum.SerialPortBaudRate bSerialPort_BaudRate; //串口波特率
        private Byte bSerialPort_ReceivedBytesThreshold; //串口接受缓冲区
        private Byte bSerialPort_SendBytesThreshold; //串口接受缓冲区

        private List<Byte> lPerTobaccoNumber;//每排烟支数量

        private static string sDeviceIPAddress = "10.11.15.1";//系统使用的IP地址段（前三段），描述如下：
        //服务端IP地址末段固定为1
        //客户端IP地址末段分配范围为2 ~ 相机数量（可使用的最大数量） + 1

        private float fDeviceFrameRate = 76; //相机帧率
        private float fDeviceFrameRateAbs = 1; //相机测试帧率误差上线

        private UInt16 uiImageWidth = 744; //图像宽度
        private UInt16 uiImageHeight = 480;//图像高度

        private Boolean bDeepLearningState;//深度学习标记

        //构造函数
        
        //-----------------------------------------------------------------------
        // 功能说明：构造函数（默认），初始化
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Camera()
        {
            _Init();//初始化
        }

        //-----------------------------------------------------------------------
        // 功能说明：构造函数，初始化，读取文件数据（HMI）
        // 输入参数：1.sConfigDataPath：配置文件路径
        //         2.cameratype：相机类型
        //         3.bytePort：端口
        //         4.sCameraName_CHN：相机中文名称
        //         5.sCameraName_ENG：相机英文名称
        //         6.sControllerName_CHN：相机中文名称
        //         7.sControllerName_ENG：相机英文名称
        //         8.bBitmapResize：选择为的相机数据截取区域缩放
        //         9.bBitmapCenter：选择为的相机数据截取区域缩放后是否居中
        //         10.rBitmapArea：选择为的相机数据截取区域
        //         11.bAngle：相机图像旋转角度
        //         12.bEnable：相机使能标记
        //         13.pBitmapAxis;原始图像数据截取区域粘贴位置（控制器程序使用）
        //         14.bFlip;镜像标记
        //         15.relevancyCameraInformation：相机关联信息
        //         16.bSampling：连续采样标记
        //         17.bPer：编码器步长
        //         18.bBaudRate：波特率
        //         19.bReceivedBytesThreshold：接收缓冲区大小
        //         20.bSendBytesThreshold：发送缓冲区大小
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Camera(global::System.String configDataPath, Enum.CameraType cameratype, Byte bytePort, UInt64 iCameraFaultState, global::System.String sCameraName_CHN, global::System.String sCameraName_ENG, global::System.String sControllerName_CHN, global::System.String sControllerName_ENG, Boolean bBitmapResize, Boolean bBitmapCenter, Rectangle rBitmapArea, Enum.CameraRotateAngle bAngle, Boolean bEnable, Point pBitmapAxis, Boolean bIsSerialPort, Enum.TobaccoSortType ttTobaccoSortType_E, Enum.VideoColor vvVideoColor, Enum.VideoResolution vvVideoResolution, Enum.CameraFlip bFlip, Struct.RelevancyCameraInformation relevancyCameraInformation, Enum.SensorProductType eProductType)
        {
            _Init();//初始化

            //

            sConfigDataPath = configDataPath;//配置文件路径（如，D:\\VisionSystemUserInterface\\ConfigData\\）

            cType = cameratype;//相机类型

            dDeviceInformation.Port = bytePort;//端口

            uiCameraFaultState = iCameraFaultState;//相机故障标记

            sCameraENGName = sCameraName_ENG;//相机英文名称
            sCameraCHNName = sCameraName_CHN;//相机中文名称
            sControllerENGName = sControllerName_ENG;//控制器英文名称
            sControllerCHNName = sControllerName_CHN;//控制器中文名称

            bCheckEnable = bEnable;//相机检测使能。取值范围：true，是；false，否
            bBitmapLockBitsResize = bBitmapResize;
            bBitmapLockBitsCenter = bBitmapCenter;
            rBitmapLockBitsArea = rBitmapArea;
            bCameraAngle = bAngle;
            pBitmapLockBitsAxis = pBitmapAxis;

            bSerialPort = bIsSerialPort;
            tTobaccoSortType_E = ttTobaccoSortType_E;

            vVideoColor = vvVideoColor;
            vVideoResolution = vvVideoResolution;

            bCameraFlip = bFlip;
            dRelevancyCameraInfo = relevancyCameraInformation;

            eSensor_ProductType = eProductType;

            //串口参数初始化
            _Init_SerialPort();

            sDataPath = sConfigDataPath + sCameraENGName + "\\";//相机数据路径（如，D:\\VisionSystemUserInterface\\ConfigData\\Top\\）
            sReceivedDataPath = sConfigDataPath + sCameraENGName + "\\" + sReceivedDataPathName;//接收文件路径（如，D:\\VisionSystemUserInterface\\ConfigData\\Top\\ReceivedData\\）
            sSampleImagePath = sConfigDataPath + sCameraENGName + "\\" + sSampleImagePathName;//学习图像路径（如，D:\\VisionSystemUserInterface\\ConfigData\\Top\\SampleImage\\）

            Int32 iCameraIndex = (Int32)cameratype - 1;//临时变量

            //

            FileStream filestream = new FileStream(sConfigDataPath + CameraFileName, FileMode.Open);
            BinaryReader binaryreader = new BinaryReader(filestream);

            filestream.Seek(0x00 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.SpeedPhase_Display = binaryreader.ReadBoolean();//速度/相位显示与否。取值范围：true，是；false，否
            filestream.Seek(0x10 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.SpeedPhase_AsMachine = binaryreader.ReadBoolean();//是否作为机器速度/相位。取值范围：true，是；false，否
            filestream.Seek(0x20 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Speed = binaryreader.ReadInt32();//速度（计算）

            filestream.Seek(0x30 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.LiveView_BackgroundImage_Zoom = binaryreader.ReadBoolean();//背景图像是否缩放
            filestream.Seek(0x40 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.LiveView_BackgroundImage_Location.X = binaryreader.ReadInt32();//背景图像的位置
            filestream.Seek(0x50 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.LiveView_BackgroundImage_Location.Y = binaryreader.ReadInt32();//背景图像的位置
            filestream.Seek(0x60 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.LiveView_LineStart_Location.X = binaryreader.ReadInt32();//指示线的起始坐标
            filestream.Seek(0x70 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.LiveView_LineStart_Location.Y = binaryreader.ReadInt32();//指示线的起始坐标
            filestream.Seek(0x80 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.LiveView_LineEnd_Location.X = binaryreader.ReadInt32();//指示线的终止坐标
            filestream.Seek(0x90 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.LiveView_LineEnd_Location.Y = binaryreader.ReadInt32();//指示线的终止坐标

            filestream.Seek(0xA0 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_Page = binaryreader.ReadInt32();//所属的页码（从0开始）
            filestream.Seek(0xB0 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_LineStart_Location.X = binaryreader.ReadInt32();//指示线的起始坐标
            filestream.Seek(0xC0 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_LineStart_Location.Y = binaryreader.ReadInt32();//指示线的起始坐标
            filestream.Seek(0xD0 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_LineEnd_Location.X = binaryreader.ReadInt32();//指示线的终止坐标
            filestream.Seek(0xE0 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_LineEnd_Location.Y = binaryreader.ReadInt32();//指示线的终止坐标
            filestream.Seek(0xF0 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_ImageDisplayBackground_Left = binaryreader.ReadInt32();//相机图像显示控件背景，左边距
            filestream.Seek(0x100 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_ImageDisplayBackground_Top = binaryreader.ReadInt32();//相机图像显示控件背景，上边距
            filestream.Seek(0x110 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_ImageDisplay_Location.X = binaryreader.ReadInt32();//相机图像显示控件位置
            filestream.Seek(0x120 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_ImageDisplay_Location.Y = binaryreader.ReadInt32();//相机图像显示控件位置
            filestream.Seek(0x130 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_ImageDisplay_Size.Width = binaryreader.ReadInt32();//相机图像显示控件大小
            filestream.Seek(0x140 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_ImageDisplay_Size.Height = binaryreader.ReadInt32();//相机图像显示控件大小
            filestream.Seek(0x150 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_ImageDisplay_Message_FontSize = binaryreader.ReadSingle();//相机图像显示控件，Message，字体大小
            filestream.Seek(0x160 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_ImageDisplay_Message_Location.X = binaryreader.ReadInt32();//相机图像显示控件，Message，位置
            filestream.Seek(0x170 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_ImageDisplay_Message_Location.Y = binaryreader.ReadInt32();//相机图像显示控件，Message，位置
            filestream.Seek(0x180 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_ImageDisplay_Message_Size.Width = binaryreader.ReadInt32();//相机图像显示控件，Message，大小
            filestream.Seek(0x190 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_ImageDisplay_Message_Size.Height = binaryreader.ReadInt32();//相机图像显示控件，Message，大小
            filestream.Seek(0x1A0 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_ImageDisplay_Slot_Location.X = binaryreader.ReadInt32();//相机图像显示控件，Slot，位置
            filestream.Seek(0x1B0 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_ImageDisplay_Slot_Location.Y = binaryreader.ReadInt32();//相机图像显示控件，Slot，位置
            filestream.Seek(0x1C0 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_ImageDisplay_Slot_Size.Width = binaryreader.ReadInt32();//相机图像显示控件，Slot，大小
            filestream.Seek(0x1D0 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_ImageDisplay_Slot_Size.Height = binaryreader.ReadInt32();//相机图像显示控件，Slot，大小
            filestream.Seek(0x1E0 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_ImageDisplay_MinValue_FontSize = binaryreader.ReadSingle();//相机图像显示控件，MinValue，字体大小
            filestream.Seek(0x1F0 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_ImageDisplay_MinValue_Location.X = binaryreader.ReadInt32();//相机图像显示控件，MinValue，位置
            filestream.Seek(0x200 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_ImageDisplay_MinValue_Location.Y = binaryreader.ReadInt32();//相机图像显示控件，MinValue，位置
            filestream.Seek(0x210 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_ImageDisplay_MinValue_Size.Width = binaryreader.ReadInt32();//相机图像显示控件，MinValue，大小
            filestream.Seek(0x220 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_ImageDisplay_MinValue_Size.Height = binaryreader.ReadInt32();//相机图像显示控件，MinValue，大小
            filestream.Seek(0x230 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_ImageDisplay_CurrentValue_FontSize = binaryreader.ReadSingle();//相机图像显示控件，CurrentValue，字体大小
            filestream.Seek(0x240 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_ImageDisplay_CurrentValue_Location.X = binaryreader.ReadInt32();//相机图像显示控件，CurrentValue，位置
            filestream.Seek(0x250 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_ImageDisplay_CurrentValue_Location.Y = binaryreader.ReadInt32();//相机图像显示控件，CurrentValue，位置
            filestream.Seek(0x260 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_ImageDisplay_CurrentValue_Size.Width = binaryreader.ReadInt32();//相机图像显示控件，CurrentValue，大小
            filestream.Seek(0x270 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_ImageDisplay_CurrentValue_Size.Height = binaryreader.ReadInt32();//相机图像显示控件，CurrentValue，大小
            filestream.Seek(0x280 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_ImageDisplay_MaxValue_FontSize = binaryreader.ReadSingle();//相机图像显示控件，MaxValue，字体大小
            filestream.Seek(0x290 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_ImageDisplay_MaxValue_Location.X = binaryreader.ReadInt32();//相机图像显示控件，MaxValue，位置
            filestream.Seek(0x2A0 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_ImageDisplay_MaxValue_Location.Y = binaryreader.ReadInt32();//相机图像显示控件，MaxValue，位置
            filestream.Seek(0x2B0 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_ImageDisplay_MaxValue_Size.Width = binaryreader.ReadInt32();//相机图像显示控件，MaxValue，大小
            filestream.Seek(0x2C0 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_ImageDisplay_MaxValue_Size.Height = binaryreader.ReadInt32();//相机图像显示控件，MaxValue，大小
            filestream.Seek(0x2D0 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_ImageDisplay_Lamp_Location.X = binaryreader.ReadInt32();//相机图像显示控件，Lamp，位置
            filestream.Seek(0x2E0 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_ImageDisplay_Lamp_Location.Y = binaryreader.ReadInt32();//相机图像显示控件，Lamp，位置
            filestream.Seek(0x2F0 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_ImageDisplay_Lamp_Size.Width = binaryreader.ReadInt32();//相机图像显示控件，Lamp，大小
            filestream.Seek(0x300 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_ImageDisplay_Lamp_Size.Height = binaryreader.ReadInt32();//相机图像显示控件，Lamp，大小
            filestream.Seek(0x310 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_Name_FontSize = binaryreader.ReadSingle();//名称，字体大小
            filestream.Seek(0x320 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_Name_Location.X = binaryreader.ReadInt32();//名称，位置
            filestream.Seek(0x330 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_Name_Location.Y = binaryreader.ReadInt32();//名称，位置
            filestream.Seek(0x340 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_Name_Size.Width = binaryreader.ReadInt32();//名称，大小
            filestream.Seek(0x350 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_Name_Size.Height = binaryreader.ReadInt32();//名称，大小
            filestream.Seek(0x360 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_State_FontSize = binaryreader.ReadSingle();//状态，字体大小
            filestream.Seek(0x370 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_State_Location.X = binaryreader.ReadInt32();//状态，位置
            filestream.Seek(0x380 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_State_Location.Y = binaryreader.ReadInt32();//状态，位置
            filestream.Seek(0x390 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_State_Size.Width = binaryreader.ReadInt32();//状态，大小
            filestream.Seek(0x3A0 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_State_Size.Height = binaryreader.ReadInt32();//状态，大小
            filestream.Seek(0x3B0 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_SpeedPhase_Value_FontSize = binaryreader.ReadSingle();//速度/相位数值，字体大小
            filestream.Seek(0x3C0 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_SpeedPhase_Value_Location.X = binaryreader.ReadInt32();//速度/相位数值，位置
            filestream.Seek(0x3D0 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_SpeedPhase_Value_Location.Y = binaryreader.ReadInt32();//速度/相位数值，位置
            filestream.Seek(0x3E0 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_SpeedPhase_Value_Size.Width = binaryreader.ReadInt32();//速度/相位数值，大小
            filestream.Seek(0x3F0 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_SpeedPhase_Value_Size.Height = binaryreader.ReadInt32();//速度/相位数值，大小
            filestream.Seek(0x400 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_SpeedPhase_Unit_FontSize = binaryreader.ReadSingle();//速度/相位单位，字体大小
            filestream.Seek(0x410 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_SpeedPhase_Unit_Location.X = binaryreader.ReadInt32();//速度/相位单位，位置
            filestream.Seek(0x420 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_SpeedPhase_Unit_Location.Y = binaryreader.ReadInt32();//速度/相位单位，位置
            filestream.Seek(0x430 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_SpeedPhase_Unit_Size.Width = binaryreader.ReadInt32();//速度/相位单位，大小
            filestream.Seek(0x440 + iCameraIndex * 0x450, SeekOrigin.Begin);
            cUIParameter.Work_SpeedPhase_Unit_Size.Height = binaryreader.ReadInt32();//速度/相位单位，大小

            binaryreader.Close();
            filestream.Close();

            //

            _SetLanguage();//相机，控制器名称名称

            _Camera_Read_Init();
        }

        //-----------------------------------------------------------------------
        // 功能说明：构造函数，初始化，读取文件数据（Controller）
        // 输入参数：1.sAppPath：应用程序路径
        //         2.porttype：相机端口
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Camera(global::System.String sAppPath, Enum.PortType porttype)
        {
            _Init();//初始化

            //

            sConfigDataPath = sAppPath + System.ConfigDataPathName;//配置文件路径（如，D:\\VisionSystemUserInterface\\ConfigData\\）

            sDataPath = sConfigDataPath + porttype.ToString() + "\\";//相机数据路径（如，D:\\VisionSystemUserInterface\\ConfigData\\Top\\）
            sReceivedDataPath = sConfigDataPath + porttype.ToString() + "\\" + sReceivedDataPathName;//接收文件路径（如，D:\\VisionSystemUserInterface\\ConfigData\\Top\\ReceivedData\\）
            sSampleImagePath = sConfigDataPath + porttype.ToString() + "\\" + sSampleImagePathName;//学习图像路径（如，D:\\VisionSystemUserInterface\\ConfigData\\Top\\SampleImage\\）

            dDeviceInformation.Port = (Byte)porttype;//端口

            _ReadRelevancy();

            //

            Int32 i = 0;//循环控制变量
            Int32 iPortIndex = (Byte)porttype - 1;//端口索引值

            FileStream filestream = new FileStream(sConfigDataPath + CameraFileName, FileMode.Open);
            BinaryReader binaryreader = new BinaryReader(filestream);

            //0x00，模式（总体）
            filestream.Seek(0x01, SeekOrigin.Begin);
            sDeviceIPAddress = binaryreader.ReadString();//读取服务器地址

            filestream.Seek(0x10 + iPortIndex * 0x30, SeekOrigin.Begin);
            cType = (Enum.CameraType)binaryreader.ReadByte();//相机类型索引值
            bCheckEnable = binaryreader.ReadBoolean();//相机检测使能。取值范围：true，是；false，否
            bCameraAngle = (Enum.CameraRotateAngle)binaryreader.ReadByte(); //读取相机旋转角度。0:0度;1:90度;2:180度;3:270度（控制器程序使用）
            vVideoColor = (Enum.VideoColor)binaryreader.ReadByte();//读取相机颜色
            vVideoResolution = (Enum.VideoResolution)binaryreader.ReadByte();//读取相机分辨率
            bSerialPort = binaryreader.ReadBoolean();//读取是否为串口
            tTobaccoSortType_E = (Enum.TobaccoSortType)binaryreader.ReadByte();//读取烟支排列类型
            bCameraFlip = (Enum.CameraFlip)binaryreader.ReadByte();//读取镜像标记
            eSensor_ProductType = (Enum.SensorProductType)binaryreader.ReadByte();//读取传感器应用场景

            //串口参数初始化
            _Init_SerialPort();

            filestream.Seek(0x1E + iPortIndex * 0x30, SeekOrigin.Begin);
            bBitmapLockBitsResize = binaryreader.ReadBoolean();//读取原始图像数据截取区域缩放标记
            bBitmapLockBitsCenter = binaryreader.ReadBoolean();//读取原始图像数据截取区域缩放后是否居中（控制器程序使用）

            filestream.Seek(0x20 + iPortIndex * 0x30, SeekOrigin.Begin);
            uiCameraFaultState = binaryreader.ReadUInt64();//相机故障标记

            filestream.Seek(0x34 + iPortIndex * 0x30, SeekOrigin.Begin);
            pBitmapLockBitsAxis.X = binaryreader.ReadInt16();//原始图像数据截取区域粘贴位置（控制器程序使用）
            pBitmapLockBitsAxis.Y = binaryreader.ReadInt16();
            rBitmapLockBitsArea.X = binaryreader.ReadInt16();//读取原始图像数据截取区域
            rBitmapLockBitsArea.Y = binaryreader.ReadInt16();
            rBitmapLockBitsArea.Width = binaryreader.ReadInt16();
            rBitmapLockBitsArea.Height = binaryreader.ReadInt16();

            binaryreader.Close();
            filestream.Close();

            //

            _Camera_Read_Init();
        }

        //-----------------------------------------------------------------------
        // 功能说明：串口参数初始化
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        private void _Init_SerialPort()
        {
            if (IsSerialPort)//当前为串口
            {
                vVideoColor = Enum.VideoColor.RGB24;
                vVideoResolution = Enum.VideoResolution._744x480;

                switch (eSensor_ProductType)
                {
                    case Enum.SensorProductType._89713FC:
                        bContinuousSampling = false;
                        bEncoderPer = 1;

                        bSerialPort_BaudRate = Enum.SerialPortBaudRate._57600;
                        bSerialPort_ReceivedBytesThreshold = 46;
                        bSerialPort_SendBytesThreshold = 24;
                        break;

                    case Enum.SensorProductType._89713FA:
                        bContinuousSampling = true;
                        bEncoderPer = 5;

                        bSerialPort_BaudRate = Enum.SerialPortBaudRate._2000000;
                        bSerialPort_ReceivedBytesThreshold = 96;
                        bSerialPort_SendBytesThreshold = 48;
                        break;

                    case Enum.SensorProductType._89750A:
                        bContinuousSampling = true;
                        bEncoderPer = 2;

                        bSerialPort_BaudRate = Enum.SerialPortBaudRate._115200;
                        bSerialPort_ReceivedBytesThreshold = 170;
                        bSerialPort_SendBytesThreshold = 36;
                        break;

                    case Enum.SensorProductType._89713CF:
                        bContinuousSampling = true;
                        bEncoderPer = 2;

                        bSerialPort_BaudRate = Enum.SerialPortBaudRate._2000000;
                        bSerialPort_ReceivedBytesThreshold = 96;
                        bSerialPort_SendBytesThreshold = 48;
                        break;

                    default:
                        break;
                }

                if (false == bContinuousSampling) //单点采样
                {
                    switch (tTobaccoSortType_E)
                    {
                        case Enum.TobaccoSortType._11:
                            bSensorNumber = 2;
                            break;
                        case Enum.TobaccoSortType._55:
                            bSensorNumber = 10;
                            break;
                        case Enum.TobaccoSortType._66:
                            bSensorNumber = 12;
                            break;
                        case Enum.TobaccoSortType._77:
                            bSensorNumber = 14;
                            break;
                        case Enum.TobaccoSortType._88:
                            bSensorNumber = 16;
                            break;
                        case Enum.TobaccoSortType._99:
                            bSensorNumber = 18;
                            break;
                        case Enum.TobaccoSortType._1010:
                        case Enum.TobaccoSortType._776:
                        case Enum.TobaccoSortType._767:
                            bSensorNumber = 20;
                            break;
                        default:
                            bSensorNumber = 0;
                            break;
                    }
                }
                else //连续采样
                {
                    switch (tTobaccoSortType_E)
                    {
                        case Enum.TobaccoSortType._55:
                        case Enum.TobaccoSortType._66:
                        case Enum.TobaccoSortType._77:
                        case Enum.TobaccoSortType._88:
                        case Enum.TobaccoSortType._99:
                        case Enum.TobaccoSortType._1010:
                        case Enum.TobaccoSortType._11:
                            bSensorNumber = 2;
                            break;
                        case Enum.TobaccoSortType._776:
                        case Enum.TobaccoSortType._767:
                            bSensorNumber = 3;
                            break;
                        default:
                            bSensorNumber = 0;
                            break;
                    }
                }

                switch (tTobaccoSortType_E)
                {
                    case Enum.TobaccoSortType._11:
                        lPerTobaccoNumber.Add(1);
                        lPerTobaccoNumber.Add(1);
                        break;
                    case Enum.TobaccoSortType._55:
                        lPerTobaccoNumber.Add(5);
                        lPerTobaccoNumber.Add(5);
                        break;
                    case Enum.TobaccoSortType._66:
                        lPerTobaccoNumber.Add(6);
                        lPerTobaccoNumber.Add(6);
                        break;
                    case Enum.TobaccoSortType._77:
                        lPerTobaccoNumber.Add(7);
                        lPerTobaccoNumber.Add(7);
                        break;
                    case Enum.TobaccoSortType._88:
                        lPerTobaccoNumber.Add(8);
                        lPerTobaccoNumber.Add(8);
                        break;
                    case Enum.TobaccoSortType._99:
                        lPerTobaccoNumber.Add(9);
                        lPerTobaccoNumber.Add(9);
                        break;
                    case Enum.TobaccoSortType._1010:
                        lPerTobaccoNumber.Add(10);
                        lPerTobaccoNumber.Add(10);
                        break;
                    case Enum.TobaccoSortType._776:
                        lPerTobaccoNumber.Add(7);
                        lPerTobaccoNumber.Add(7);
                        lPerTobaccoNumber.Add(6);
                        break;
                    case Enum.TobaccoSortType._767:
                        lPerTobaccoNumber.Add(7);
                        lPerTobaccoNumber.Add(6);
                        lPerTobaccoNumber.Add(7);
                        break;
                    default:
                        break;
                }
            }
            sVideoFormat = vVideoColor.ToString() + " " + (vVideoResolution.ToString()).Replace("_", "(") + ")";

            Int32 iWidthStartPos = sVideoFormat.IndexOf("(");
            Int32 iHeightStartPos = sVideoFormat.IndexOf("x");
            Int32 iHeightEndPos = sVideoFormat.IndexOf(")");
            uiImageWidth = Convert.ToUInt16(sVideoFormat.Substring(iWidthStartPos + 1, iHeightStartPos - iWidthStartPos - 1));
            uiImageHeight = Convert.ToUInt16(sVideoFormat.Substring(iHeightStartPos + 1, iHeightEndPos - iHeightStartPos - 1));
        }

        //属性

        //-----------------------------------------------------------------------
        // 功能说明：DeviceIPAddress属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static string DeviceIPAddress
        {
            get//读取
            {
                return sDeviceIPAddress;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：DeviceFrameRate属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public float DeviceFrameRate
        {
            get//读取
            {
                return fDeviceFrameRate;
            }
        }
        
        //-----------------------------------------------------------------------
        // 功能说明：DeviceFrameRateAbs属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public float DeviceFrameRateAbs
        {
            get//读取
            {
                return fDeviceFrameRateAbs;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ImageWidth属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public UInt16 ImageWidth
        {
            get//读取
            {
                return uiImageWidth;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ImageHeight属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public UInt16 ImageHeight
        {
            get//读取
            {
                return uiImageHeight;
            }
        }

        /// <summary>
        /// 深度学习标记
        /// </summary>
        public Boolean DeepLearningState
        {
            get
            {
                return bDeepLearningState;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：DeviceParameter属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Struct.CameraParameter DeviceParameter
        {
            get//读取
            {
                return cDeviceParameter;
            }
            set
            {
                cDeviceParameter = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：UIParameter属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Struct.Camera_UIParameter UIParameter
        {
            get//读取
            {
                return cUIParameter;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：DeviceInformation属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Struct.DeviceData DeviceInformation
        {
            get//读取
            {
                return dDeviceInformation;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：CurrentFaultMessage属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Struct.FaultMessage CurrentFaultMessage
        {
            get//读取
            {
                return fCurrentFaultMessage;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：FaultMessages属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Struct.FaultMessage[] FaultMessages
        {
            get//读取
            {
                return fFaultMessages;
            }
            set
            {
                fFaultMessages = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：CameraFaultState属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public UInt64 CameraFaultState
        {
            get//读取
            {
                return uiCameraFaultState;
            }
            set
            {
                uiCameraFaultState = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ImageLive属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Image<Bgr, Byte> ImageLive
        {
            get//读取
            {
                return iImageLive;
            }
            set
            {
                iImageLive = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ImageReject属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Image<Bgr, Byte> ImageReject
        {
            get//读取
            {
                return iImageReject;
            }
            set
            {
                iImageReject = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ImageLearn属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Image<Bgr, Byte> ImageLearn
        {
            get//读取
            {
                return iImageLearn;
            }
            set
            {
                iImageLearn = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Live属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Struct.LiveData Live
        {
            get//读取
            {
                return lLive;
            }
            set
            {
                lLive = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Rejects属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Struct.RejectsData Rejects
        {
            get//读取
            {
                return rRejects;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Learn属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Struct.ImageInformation Learn
        {
            get//读取
            {
                return lLearn;
            }
            set
            {
                lLearn = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ToolFileName属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static string ToolFileName
        {
            get//读取
            {
                return sToolFileName;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：TolerancesFileName属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static string TolerancesFileName
        {
            get//读取
            {
                return sTolerancesFileName;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ParameterFileName属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static string ParameterFileName
        {
            get//读取
            {
                return sParameterFileName;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：SampleDataFileName属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static string SampleDataFileName
        {
            get//读取
            {
                return sSampleDataFileName;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：SampleImageFileName属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static string SampleImageFileName
        {
            get//读取
            {
                return sSampleImageFileName;
            }
        }

        /// <summary>
        /// 分类文件
        /// </summary>
        public static string ClassesFile
        {
            get//读取
            {
                return sClassesFile;
            }
        }

        /// <summary>
        /// 模型文件
        /// </summary>
        public static string ModelFileName
        {
            get//读取
            {
                return sModelFileName;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：DatFile属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static string DatFile
        {
            get//读取
            {
                return sDatFile;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：JPGFile属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static string JPGFile
        {
            get//读取
            {
                return sJPGFile;
            }
        }

        /// <summary>
        /// PNGFile属性
        /// </summary>
        public static string PNGFile
        {
            get//读取
            {
                return sPNGFile;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：BMPFile属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static string BMPFile
        {
            get//读取
            {
                return sBMPFile;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ReceivedDataPathName属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static string ReceivedDataPathName
        {
            get//读取
            {
                return sReceivedDataPathName;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：SampleImagePathName属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static string SampleImagePathName
        {
            get//读取
            {
                return sSampleImagePathName;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：DataPath属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public string DataPath
        {
            get//读取
            {
                return sDataPath;
            }
            set
            {
                sDataPath = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ReceivedDataPath属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public string ReceivedDataPath
        {
            get//读取
            {
                return sReceivedDataPath;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：SampleImagePath属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public string SampleImagePath
        {
            get//读取
            {
                return sSampleImagePath;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ConfigDataPath属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public string ConfigDataPath
        {
            get//读取
            {
                return sConfigDataPath;
            }
            set
            {
                sConfigDataPath = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Type属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Enum.CameraType Type
        {
            get//读取
            {
                return cType;
            }
            set
            {
                cType = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：CameraENGName属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public string CameraENGName
        {
            get//读取
            {
                return sCameraENGName;
            }
            set
            {
                sCameraENGName = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：CameraCHNName属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public string CameraCHNName
        {
            get//读取
            {
                return sCameraCHNName;
            }
            set
            {
                sCameraCHNName = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ControllerENGName属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public string ControllerENGName
        {
            get//读取
            {
                return sControllerENGName;
            }
            set
            {
                sControllerENGName = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ControllerCHNName属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public string ControllerCHNName
        {
            get//读取
            {
                return sControllerCHNName;
            }
            set
            {
                sControllerCHNName = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Name属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public string Name
        {
            get//读取
            {
                return sName;
            }
            set
            {
                sName = value;
            }
        }
        
        //-----------------------------------------------------------------------
        // 功能说明：相机检测使能标记
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Boolean CheckEnable
        {
            get//读取
            {
                return bCheckEnable;
            }
            set//设置
            {
                bCheckEnable = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：相机旋转角度。0:0度;1:90度;2:180度;3:270度（控制器程序使用）
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Enum.CameraRotateAngle CameraAngle
        {
            get//读取
            {
                return bCameraAngle;
            }
            set//设置
            {
                bCameraAngle = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：相机镜像
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Enum.CameraFlip CameraFlip
        {
            get//读取
            {
                return bCameraFlip;
            }
            set//设置
            {
                bCameraFlip = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：原始图像数据截取区域缩放后是否居中（控制器程序使用）
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Boolean BitmapLockBitsCenter
        {
            get//读取
            {
                return bBitmapLockBitsCenter;
            }
            set//设置
            {
                bBitmapLockBitsCenter = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：原始图像数据截取区域缩放（控制器程序使用）
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Boolean BitmapLockBitsResize
        {
            get//读取
            {
                return bBitmapLockBitsResize;
            }
            set//设置
            {
                bBitmapLockBitsResize = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：原始图像数据截取区域粘贴区域（控制器程序使用）
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Point BitmapLockBitsAxis
        {
            get//读取
            {
                return pBitmapLockBitsAxis;
            }
            set//设置
            {
                pBitmapLockBitsAxis = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：原始图像数据截取区域（控制器程序使用）
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Rectangle BitmapLockBitsArea
        {
            get//读取
            {
                return rBitmapLockBitsArea;
            }
            set//设置
            {
                rBitmapLockBitsArea = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Tools属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public List<Tools> Tools
        {
            get//读取
            {
                return tools;
            }
            set//设置
            {
                tools = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：RelevancyCameraInfo属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Struct.RelevancyCameraInformation RelevancyCameraInfo
        {
            get//读取
            {
                return dRelevancyCameraInfo;
            }
            set//设置
            {
                dRelevancyCameraInfo = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明： Tolerances属性
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        public TolerancesData Tolerances
        {
            get//读取
            {
                return tolerances;
            }
            set//设置
            {
                if (value != tolerances)
                {
                    tolerances = value;
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：是否为串口
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Boolean IsSerialPort
        {
            get//读取
            {
                return bSerialPort;
            }
            set//设置
            {
                bSerialPort = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：烟支排列方式
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Enum.TobaccoSortType TobaccoSortType_E
        {
            get//读取
            {
                return tTobaccoSortType_E;
            }
            set//设置
            {
                tTobaccoSortType_E = value;
            }
        }
        //-----------------------------------------------------------------------
        // 功能说明：相机颜色
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Enum.VideoColor VideoColor
        {
            get//读取
            {
                return vVideoColor;
            }
            set//设置
            {
                vVideoColor = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：相机分辨率
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Enum.VideoResolution VideoResolution
        {
            get//读取
            {
                return vVideoResolution;
            }
            set//设置
            {
                vVideoResolution = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：相机格式
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public string VideoFormat
        {
            get//读取
            {
                return sVideoFormat;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：传感器数量
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Byte SensorNumber
        {
            get//读取
            {
                return bSensorNumber;
            }
            set
            {
                bSensorNumber = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：传感器应用场景
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Enum.SensorProductType Sensor_ProductType
        {
            get//读取
            {
                return eSensor_ProductType;
            }
            set//设置
            {
                eSensor_ProductType = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：连续采样标记
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Boolean ContinuousSampling
        {
            get//读取
            {
                return bContinuousSampling;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：编码器步长
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Byte EncoderPer
        {
            get//读取
            {
                return bEncoderPer;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：串口波特率
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Enum.SerialPortBaudRate SerialPort_BaudRate
        {
            get//读取
            {
                return bSerialPort_BaudRate;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：串口接收缓冲区大小
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Byte SerialPort_ReceivedBytesThreshold
        {
            get//读取
            {
                return bSerialPort_ReceivedBytesThreshold;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：串口发送缓冲区大小
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Byte SerialPort_SendBytesThreshold
        {
            get//读取
            {
                return bSerialPort_SendBytesThreshold;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：每排烟支数量
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public List<Byte> PerTobaccoNumber
        {
            get//读取
            {
                return lPerTobaccoNumber;
            }
        }

        //函数

        //-----------------------------------------------------------------------
        // 功能说明：写入图像、图像信息
        // 输入参数：1.type：图像信息类型
        //         2.bType：REJECTS图像信息或图像。取值范围：true，图像信息；false，图像
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _WriteImage(Enum.ImageInformationType type, Boolean bType = true)
        {
            FileStream fileStream = null; //打开SAMPLE文件

            BinaryWriter binaryWriter = null;

            //

            try
            {
                if (Enum.ImageInformationType.Sample == type)//SAMPLE
                {
                    //GeneralFunction._SaveImage(iImageLearn, sSampleImagePath + SampleImageFileName + sBMPFile, sBMPFile);
                    iImageLearn.Save(sSampleImagePath + SampleImageFileName + sBMPFile);

                    //

                    fileStream = new FileStream(sSampleImagePath + sSampleDataFileName, FileMode.Create, FileAccess.Write, FileShare.None, 8192, FileOptions.WriteThrough); //打开SAMPLE文件

                    binaryWriter = new BinaryWriter(fileStream);

                    _WriteImageInformation(fileStream, binaryWriter, lLearn);//写文件
                }
                else//其它
                {
                    //不执行操作
                }
            }
            catch (Exception ex)
            {
                //不执行操作
            }

            //

            if (null != binaryWriter)//有效
            {
                binaryWriter.Close();//关闭文件
            }

            if (null != fileStream)
            {
                fileStream.Close();//关闭文件
            }
        }
        
        //-----------------------------------------------------------------------
        // 功能说明：读取图像、图像信息
        // 输入参数：1.type：图像信息类型
        //         2.bType：REJECTS图像信息或图像。取值范围：true，图像信息；false，图像
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        private void _ReadImage(Enum.ImageInformationType type, Boolean bType = true)
        {
            FileStream fileStream = null; //打开SAMPLE文件

            BinaryReader binaryReader = null;

            //

            try
            {
                if (Enum.ImageInformationType.Sample == type)//SAMPLE
                {
                    iImageLearn = new Image<Bgr, byte>(sSampleImagePath + SampleImageFileName + sBMPFile);//学习图像
                    //iImageLearn = GeneralFunction._ReadImage(sSampleImagePath + SampleImageFileName + sBMPFile);//学习图像

                    //

                    fileStream = new FileStream(sSampleImagePath + sSampleDataFileName, FileMode.Open); //打开SAMPLE文件

                    binaryReader = new BinaryReader(fileStream);

                    _ReadImageInformation(fileStream, binaryReader, lLearn);//读文件
                }
                else//其它
                {
                    //不执行操作
                }
            }
            catch (Exception ex)
            {
                //不执行操作
            }

            //

            if (null != binaryReader)//有效
            {
                binaryReader.Close();//关闭文件
            }

            if (null != fileStream)
            {
                fileStream.Close();//关闭文件
            }
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：设置相机名称（UI）
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _SetLanguage()
        {
            switch (System.Language)
            {
                case Enum.InterfaceLanguage.Chinese:

                    sName = sCameraCHNName;

                    dDeviceInformation.DeviceName = sCameraCHNName;

                    dDeviceInformation.ControllerName = sControllerCHNName;

                    break;
                case Enum.InterfaceLanguage.English:

                    sName = sCameraENGName;

                    dDeviceInformation.DeviceName = sCameraENGName;

                    dDeviceInformation.ControllerName = sControllerENGName;

                    break;
                default:

                    sName = sCameraCHNName;

                    dDeviceInformation.DeviceName = sCameraCHNName;

                    dDeviceInformation.ControllerName = sControllerCHNName;

                    break;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：相机类拷贝函数
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：1、Camera，相机参数
        //----------------------------------------------------------------------
        public Camera _Copy()
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, this);
            stream.Position = 0;
            return formatter.Deserialize(stream) as Camera;
        }

        //-----------------------------------------------------------------------
        // 功能说明：相机类拷贝函数
        // 输入参数：1.Camera：camera，相机参数
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _CopyTo(ref Camera camera)
        {
            camera.tools.Clear();
            for (Int32 i = 0; i < tools.Count; i++)
            {
                Class.Tools toolTemp = new Class.Tools();
                tools[i]._CopyTo(ref toolTemp);
                camera.tools.Add(toolTemp);
            }

            tolerances._CopyTo(ref camera.tolerances);

            if (iImageLearn != null)//图像不为空
            {
                camera.iImageLearn = iImageLearn.Copy();
                lLearn._CopyTo(camera.lLearn);
            }

            cDeviceParameter._CopyTo(ref camera.cDeviceParameter);

            camera.ReferenceHorizenL = ReferenceHorizenL;
            camera.ReferenceHorizenR = ReferenceHorizenR;
            camera.ReferenceVerticalL = ReferenceVerticalL;
            camera.ReferenceVerticalR = ReferenceVerticalR;

            camera.ReferenceHorizenL_Sample = ReferenceHorizenL_Sample;
            camera.ReferenceHorizenR_Sample = ReferenceHorizenR_Sample;
            camera.ReferenceVerticalL_Sample = ReferenceVerticalL_Sample;
            camera.ReferenceVerticalR_Sample = ReferenceVerticalR_Sample;

            camera.ReferenceHorizenPoint = ReferenceHorizenPoint;
            camera.ReferenceVerticalPoint = ReferenceVerticalPoint;

            camera.bCheckEnable = bCheckEnable;

            camera.bDeepLearningState = bDeepLearningState;
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：保存相机界面数据（UI使用）
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _SaveCameraParameter()
        {
            Int32 iCameraIndex = (Int32)cType - 1;//临时变量

            FileStream filestream = new FileStream(sConfigDataPath + CameraFileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, 8192, FileOptions.WriteThrough);
            BinaryWriter binarywriter = new BinaryWriter(filestream);

            filestream.Seek(0x00 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.SpeedPhase_Display);//保存速度/相位显示与否。取值范围：true，是；false，否
            filestream.Seek(0x10 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.SpeedPhase_AsMachine);//保存是否作为机器速度/相位。取值范围：true，是；false，否
            filestream.Seek(0x20 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Speed);//保存速度（计算）

            filestream.Seek(0x30 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.LiveView_BackgroundImage_Zoom);//保存背景图像是否缩放
            filestream.Seek(0x40 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.LiveView_BackgroundImage_Location.X);//保存背景图像的位置
            filestream.Seek(0x50 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.LiveView_BackgroundImage_Location.Y);//保存背景图像的位置
            filestream.Seek(0x60 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.LiveView_LineStart_Location.X);//保存指示线的起始坐标
            filestream.Seek(0x70 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.LiveView_LineStart_Location.Y);//保存指示线的起始坐标
            filestream.Seek(0x80 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.LiveView_LineEnd_Location.X);//保存指示线的终止坐标
            filestream.Seek(0x90 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.LiveView_LineEnd_Location.Y);//保存指示线的终止坐标

            filestream.Seek(0xA0 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_Page);//保存所属的页码（从0开始）
            filestream.Seek(0xB0 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_LineStart_Location.X);//保存指示线的起始坐标
            filestream.Seek(0xC0 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_LineStart_Location.Y);//保存指示线的起始坐标
            filestream.Seek(0xD0 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_LineEnd_Location.X);//保存指示线的终止坐标
            filestream.Seek(0xE0 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_LineEnd_Location.Y);//保存指示线的终止坐标
            filestream.Seek(0xF0 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_ImageDisplayBackground_Left);//保存相机图像显示控件背景，左边距
            filestream.Seek(0x100 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_ImageDisplayBackground_Top);//保存相机图像显示控件背景，上边距
            filestream.Seek(0x110 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_ImageDisplay_Location.X);//保存相机图像显示控件位置
            filestream.Seek(0x120 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_ImageDisplay_Location.Y);//保存相机图像显示控件位置
            filestream.Seek(0x130 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_ImageDisplay_Size.Width);//保存相机图像显示控件大小
            filestream.Seek(0x140 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_ImageDisplay_Size.Height);//保存相机图像显示控件大小
            filestream.Seek(0x150 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_ImageDisplay_Message_FontSize);//保存相机图像显示控件，Message，字体大小
            filestream.Seek(0x160 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_ImageDisplay_Message_Location.X);//保存相机图像显示控件，Message，位置
            filestream.Seek(0x170 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_ImageDisplay_Message_Location.Y);//保存相机图像显示控件，Message，位置
            filestream.Seek(0x180 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_ImageDisplay_Message_Size.Width);//保存相机图像显示控件，Message，大小
            filestream.Seek(0x190 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_ImageDisplay_Message_Size.Height);//保存相机图像显示控件，Message，大小
            filestream.Seek(0x1A0 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_ImageDisplay_Slot_Location.X);//保存相机图像显示控件，Slot，位置
            filestream.Seek(0x1B0 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_ImageDisplay_Slot_Location.Y);//保存相机图像显示控件，Slot，位置
            filestream.Seek(0x1C0 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_ImageDisplay_Slot_Size.Width);//保存相机图像显示控件，Slot，大小
            filestream.Seek(0x1D0 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_ImageDisplay_Slot_Size.Height);//保存相机图像显示控件，Slot，大小
            filestream.Seek(0x1E0 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_ImageDisplay_MinValue_FontSize);//保存相机图像显示控件，MinValue，字体大小
            filestream.Seek(0x1F0 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_ImageDisplay_MinValue_Location.X);//保存相机图像显示控件，MinValue，位置
            filestream.Seek(0x200 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_ImageDisplay_MinValue_Location.Y);//保存相机图像显示控件，MinValue，位置
            filestream.Seek(0x210 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_ImageDisplay_MinValue_Size.Width);//保存相机图像显示控件，MinValue，大小
            filestream.Seek(0x220 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_ImageDisplay_MinValue_Size.Height);//保存相机图像显示控件，MinValue，大小
            filestream.Seek(0x230 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_ImageDisplay_CurrentValue_FontSize);//保存相机图像显示控件，CurrentValue，字体大小
            filestream.Seek(0x240 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_ImageDisplay_CurrentValue_Location.X);//保存相机图像显示控件，CurrentValue，位置
            filestream.Seek(0x250 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_ImageDisplay_CurrentValue_Location.Y);//保存相机图像显示控件，CurrentValue，位置
            filestream.Seek(0x260 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_ImageDisplay_CurrentValue_Size.Width);//保存相机图像显示控件，CurrentValue，大小
            filestream.Seek(0x270 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_ImageDisplay_CurrentValue_Size.Height);//保存相机图像显示控件，CurrentValue，大小
            filestream.Seek(0x280 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_ImageDisplay_MaxValue_FontSize);//保存相机图像显示控件，MaxValue，字体大小
            filestream.Seek(0x290 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_ImageDisplay_MaxValue_Location.X);//保存相机图像显示控件，MaxValue，位置
            filestream.Seek(0x2A0 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_ImageDisplay_MaxValue_Location.Y);//保存相机图像显示控件，MaxValue，位置
            filestream.Seek(0x2B0 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_ImageDisplay_MaxValue_Size.Width);//保存相机图像显示控件，MaxValue，大小
            filestream.Seek(0x2C0 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_ImageDisplay_MaxValue_Size.Height);//保存相机图像显示控件，MaxValue，大小
            filestream.Seek(0x2D0 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_ImageDisplay_Lamp_Location.X);//保存相机图像显示控件，Lamp，位置
            filestream.Seek(0x2E0 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_ImageDisplay_Lamp_Location.Y);//保存相机图像显示控件，Lamp，位置
            filestream.Seek(0x2F0 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_ImageDisplay_Lamp_Size.Width);//保存相机图像显示控件，Lamp，大小
            filestream.Seek(0x300 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_ImageDisplay_Lamp_Size.Height);//保存相机图像显示控件，Lamp，大小
            filestream.Seek(0x310 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_Name_FontSize);//保存名称，字体大小
            filestream.Seek(0x320 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_Name_Location.X);//保存名称，位置
            filestream.Seek(0x330 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_Name_Location.Y);//保存名称，位置
            filestream.Seek(0x340 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_Name_Size.Width);//保存名称，大小
            filestream.Seek(0x350 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_Name_Size.Height);//保存名称，大小
            filestream.Seek(0x360 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_State_FontSize);//保存状态，字体大小
            filestream.Seek(0x370 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_State_Location.X);//保存状态，位置
            filestream.Seek(0x380 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_State_Location.Y);//保存状态，位置
            filestream.Seek(0x390 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_State_Size.Width);//保存状态，大小
            filestream.Seek(0x3A0 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_State_Size.Height);//保存状态，大小
            filestream.Seek(0x3B0 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_SpeedPhase_Value_FontSize);//保存速度/相位数值，字体大小
            filestream.Seek(0x3C0 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_SpeedPhase_Value_Location.X);//保存速度/相位数值，位置
            filestream.Seek(0x3D0 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_SpeedPhase_Value_Location.Y);//保存速度/相位数值，位置
            filestream.Seek(0x3E0 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_SpeedPhase_Value_Size.Width);//保存速度/相位数值，大小
            filestream.Seek(0x3F0 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_SpeedPhase_Value_Size.Height);//保存速度/相位数值，大小
            filestream.Seek(0x400 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_SpeedPhase_Unit_FontSize);//保存速度/相位单位，字体大小
            filestream.Seek(0x410 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_SpeedPhase_Unit_Location.X);//保存速度/相位单位，位置
            filestream.Seek(0x420 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_SpeedPhase_Unit_Location.Y);//保存速度/相位单位，位置
            filestream.Seek(0x430 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_SpeedPhase_Unit_Size.Width);//保存速度/相位单位，大小
            filestream.Seek(0x440 + iCameraIndex * 0x450, SeekOrigin.Begin);
            binarywriter.Write(cUIParameter.Work_SpeedPhase_Unit_Size.Height);//保存速度/相位单位，大小

            binarywriter.Close();
            filestream.Close();
        }

        //-----------------------------------------------------------------------
        // 功能说明：保存端口数据（Controller使用）
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _Save_CameraType()
        {
            Int32 iPortIndex = DeviceInformation.Port - 1;

            FileStream filestream = new FileStream(sConfigDataPath + CameraFileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, 8192, FileOptions.WriteThrough);
            BinaryWriter binarywriter = new BinaryWriter(filestream);

            filestream.Seek(0x10 + iPortIndex * 0x30, SeekOrigin.Begin);
            binarywriter.Write((Byte)cType);//相机类型
            binarywriter.Write(bCheckEnable);//保存相机检测使能状态
            binarywriter.Write((Byte)bCameraAngle); //写入相机旋转角度。0:0度;1:90度;2:180度;3:270度（控制器程序使用）
            binarywriter.Write((Byte)vVideoColor);//写入相机颜色
            binarywriter.Write((Byte)vVideoResolution);//写入相机分辨率
            binarywriter.Write(IsSerialPort);//写入是否为串口
            binarywriter.Write((Byte)tTobaccoSortType_E);//写入烟支排列类型
            binarywriter.Write((Byte)bCameraFlip);//写入镜像标记
            binarywriter.Write((Byte)eSensor_ProductType);//写入传感器应用场景

            filestream.Seek(0x1E + iPortIndex * 0x30, SeekOrigin.Begin);
            binarywriter.Write(bBitmapLockBitsResize);//写入原始图像数据截取区域缩放标记
            binarywriter.Write(bBitmapLockBitsCenter);//写入原始图像数据截取区域缩放后是否居中（控制器程序使用）

            filestream.Seek(0x20 + iPortIndex * 0x30, SeekOrigin.Begin);
            binarywriter.Write(uiCameraFaultState);//相机故障标记

            filestream.Seek(0x34 + iPortIndex * 0x30, SeekOrigin.Begin);
            binarywriter.Write(Convert.ToInt16(pBitmapLockBitsAxis.X));//写入原始图像数据截取区域粘贴区域
            binarywriter.Write(Convert.ToInt16(pBitmapLockBitsAxis.Y));//写入原始图像数据截取区域粘贴区域
            binarywriter.Write(Convert.ToInt16(rBitmapLockBitsArea.X));//写入原始图像数据截取区域
            binarywriter.Write(Convert.ToInt16(rBitmapLockBitsArea.Y));
            binarywriter.Write(Convert.ToInt16(rBitmapLockBitsArea.Width));
            binarywriter.Write(Convert.ToInt16(rBitmapLockBitsArea.Height));

            binarywriter.Close();
            filestream.Close();
        }

        //-----------------------------------------------------------------------
        // 功能说明：保存端口数据（Controller使用）
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static void _WriteCameraChooseState(string sPath, Byte cameraChooseState)
        {
            FileStream filestream = new FileStream(sPath + System.ConfigDataPathName + CameraFileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, 8192, FileOptions.WriteThrough);
            BinaryWriter binarywriter = new BinaryWriter(filestream);

            binarywriter.Write(cameraChooseState);//相机模式

            binarywriter.Close();
            filestream.Close();
        }

        //-----------------------------------------------------------------------
        // 功能说明：保存关联信息，提供配置工具使用
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _SaveRelevancy()
        {
            Int32 iPortIndex = DeviceInformation.Port - 1;

            FileStream filestream = new FileStream(sConfigDataPath + Class.System.RelevancyFileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, 8192, FileOptions.WriteThrough);
            BinaryWriter binarywriter = new BinaryWriter(filestream);

            filestream.Seek(0x000 + iPortIndex * 0x210, SeekOrigin.Begin);
            binarywriter.Write((Byte)cType);//相机类型
            binarywriter.Write((Byte)dRelevancyCameraInfo.rRelevancyType);//关联类型

            if (Enum.RelevancyType.None < dRelevancyCameraInfo.rRelevancyType) //存在关联相机
            {
                filestream.Seek(0x002 + iPortIndex * 0x210, SeekOrigin.Begin);
                binarywriter.Write((Byte)dRelevancyCameraInfo.RelevancyCameraInfo.Count);//关联数量

                filestream.Seek(0x010 + iPortIndex * 0x210, SeekOrigin.Begin);
                for (Byte i = 0; i < dRelevancyCameraInfo.RelevancyCameraInfo.Count; i++)
                {
                    binarywriter.Write((Byte)dRelevancyCameraInfo.RelevancyCameraInfo.ElementAt(i).Key);//关联相机类型
                    binarywriter.Write((Byte)dRelevancyCameraInfo.RelevancyCameraInfo.ElementAt(i).Value);//关联相机类型
                }
            }
            binarywriter.Close();
            filestream.Close();
        }

        //-----------------------------------------------------------------------
        // 功能说明：保存相机参数数据，提供配置工具使用
        // 输入参数：1.sPath：文件数据路径
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _SaveParameterReload()
        {
            FileStream filestream = new FileStream(sDataPath + sParameterFileName, FileMode.Create, FileAccess.Write, FileShare.None, 8192, FileOptions.WriteThrough);
            BinaryWriter binarywriter = new BinaryWriter(filestream);

            if (false == IsSerialPort) //当前为相机
            {
                filestream.Seek(0x000, SeekOrigin.Begin);
                binarywriter.Write(cDeviceParameter.StroboTime_Valid);
                filestream.Seek(0x010, SeekOrigin.Begin);
                binarywriter.Write(cDeviceParameter.StroboTime);
                filestream.Seek(0x020, SeekOrigin.Begin);
                binarywriter.Write(cDeviceParameter.StroboTime_Min);
                filestream.Seek(0x030, SeekOrigin.Begin);
                binarywriter.Write(cDeviceParameter.StroboTime_Max);

                filestream.Seek(0x040, SeekOrigin.Begin);
                binarywriter.Write(cDeviceParameter.StroboCurrent_Valid);
                filestream.Seek(0x050, SeekOrigin.Begin);
                binarywriter.Write(cDeviceParameter.StroboCurrent);
                filestream.Seek(0x060, SeekOrigin.Begin);
                binarywriter.Write(cDeviceParameter.StroboCurrent_Min);
                filestream.Seek(0x070, SeekOrigin.Begin);
                binarywriter.Write(cDeviceParameter.StroboCurrent_Max);

                filestream.Seek(0x080, SeekOrigin.Begin);
                binarywriter.Write(cDeviceParameter.Gain_Valid);
                filestream.Seek(0x090, SeekOrigin.Begin);
                binarywriter.Write(cDeviceParameter.Gain);
                filestream.Seek(0x0A0, SeekOrigin.Begin);
                binarywriter.Write(cDeviceParameter.Gain_Min);
                filestream.Seek(0x0B0, SeekOrigin.Begin);
                binarywriter.Write(cDeviceParameter.Gain_Max);

                filestream.Seek(0x0C0, SeekOrigin.Begin);
                binarywriter.Write(cDeviceParameter.ExposureTime_Valid);
                filestream.Seek(0x0D0, SeekOrigin.Begin);
                binarywriter.Write(cDeviceParameter.ExposureTime);
                filestream.Seek(0x0E0, SeekOrigin.Begin);
                binarywriter.Write(cDeviceParameter.ExposureTime_Min);
                filestream.Seek(0x0F0, SeekOrigin.Begin);
                binarywriter.Write(cDeviceParameter.ExposureTime_Max);

                filestream.Seek(0x100, SeekOrigin.Begin);
                binarywriter.Write(cDeviceParameter.WhiteBalance_Valid);
                filestream.Seek(0x110, SeekOrigin.Begin);
                binarywriter.Write(cDeviceParameter.WhiteBalance);

                filestream.Seek(0x120, SeekOrigin.Begin);
                binarywriter.Write(cDeviceParameter.WhiteBalance_Red_Valid);
                filestream.Seek(0x130, SeekOrigin.Begin);
                binarywriter.Write(cDeviceParameter.WhiteBalance_Red);
                filestream.Seek(0x140, SeekOrigin.Begin);
                binarywriter.Write(cDeviceParameter.WhiteBalance_Red_Min);
                filestream.Seek(0x150, SeekOrigin.Begin);
                binarywriter.Write(cDeviceParameter.WhiteBalance_Red_Max);

                filestream.Seek(0x160, SeekOrigin.Begin);
                binarywriter.Write(cDeviceParameter.WhiteBalance_Green_Valid);
                filestream.Seek(0x170, SeekOrigin.Begin);
                binarywriter.Write(cDeviceParameter.WhiteBalance_Green);
                filestream.Seek(0x180, SeekOrigin.Begin);
                binarywriter.Write(cDeviceParameter.WhiteBalance_Green_Min);
                filestream.Seek(0x190, SeekOrigin.Begin);
                binarywriter.Write(cDeviceParameter.WhiteBalance_Green_Max);

                filestream.Seek(0x1A0, SeekOrigin.Begin);
                binarywriter.Write(cDeviceParameter.WhiteBalance_Blue_Valid);
                filestream.Seek(0x1B0, SeekOrigin.Begin);
                binarywriter.Write(cDeviceParameter.WhiteBalance_Blue);
                filestream.Seek(0x1C0, SeekOrigin.Begin);
                binarywriter.Write(cDeviceParameter.WhiteBalance_Blue_Min);
                filestream.Seek(0x1D0, SeekOrigin.Begin);
                binarywriter.Write(cDeviceParameter.WhiteBalance_Blue_Max);
            }
            else //当前为串口
            {
                filestream.Seek(0x1C0, SeekOrigin.Begin);
                binarywriter.Write(cDeviceParameter.SensorAdjustValue);
            }

            //

            if (null != cDeviceParameter.Parameter)
            {
                Int32 i = 0;

                for (i = 0; i < cDeviceParameter.Parameter.Count; i++)
                {
                    filestream.Seek(0x1E0 + i * 0xB0, SeekOrigin.Begin);
                    binarywriter.Write(cDeviceParameter.Parameter[i]);
                    filestream.Seek(0x1F0 + i * 0xB0, SeekOrigin.Begin);
                    binarywriter.Write(cDeviceParameter.Parameter_Min[i]);
                    filestream.Seek(0x200 + i * 0xB0, SeekOrigin.Begin);
                    binarywriter.Write(cDeviceParameter.Parameter_Max[i]);
                    filestream.Seek(0x210 + i * 0xB0, SeekOrigin.Begin);
                    binarywriter.Write(cDeviceParameter.Parameter_NameCHN[i]);
                    filestream.Seek(0x250 + i * 0xB0, SeekOrigin.Begin);
                    binarywriter.Write(cDeviceParameter.Parameter_NameENG[i]);

                    if (cDeviceParameter.Parameter_NameENG[i].Length < 48)
                    {
                        filestream.Seek(0x28F + i * 0xB0, SeekOrigin.Begin);
                        binarywriter.Write(0);
                    }
                }
            }

            binarywriter.Close();
            filestream.Close();
        }

        //-----------------------------------------------------------------------
        // 功能说明：保存相机参数数据
        // 输入参数：1.sPath：文件数据路径
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _SaveParameter()
        {
            FileStream filestream1 = null;
            BinaryReader binaryreader = null;

            FileStream filestream = null;
            BinaryWriter binarywriter = null;

            try
            {
                filestream1 = new FileStream(sDataPath + sParameterFileName, FileMode.Open);
                binaryreader = new BinaryReader(filestream1);//读取文件数据

                Byte[] bData = binaryreader.ReadBytes((Int32)filestream1.Length);

                binaryreader.Close();
                filestream1.Close();

                filestream = new FileStream(sDataPath + sParameterFileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, 8192, FileOptions.WriteThrough);
                binarywriter = new BinaryWriter(filestream);

                if (false == IsSerialPort) //当前为相机
                {
                    BitConverter.GetBytes(cDeviceParameter.StroboTime).CopyTo(bData, 0x010);
                    BitConverter.GetBytes(cDeviceParameter.StroboCurrent).CopyTo(bData, 0x050);
                    BitConverter.GetBytes(cDeviceParameter.Gain).CopyTo(bData, 0x090);
                    BitConverter.GetBytes(cDeviceParameter.ExposureTime).CopyTo(bData, 0x0D0);
                    BitConverter.GetBytes(cDeviceParameter.WhiteBalance).CopyTo(bData, 0x110);
                    BitConverter.GetBytes(cDeviceParameter.WhiteBalance_Red).CopyTo(bData, 0x130);
                    BitConverter.GetBytes(cDeviceParameter.WhiteBalance_Green).CopyTo(bData, 0x170);
                    BitConverter.GetBytes(cDeviceParameter.WhiteBalance_Blue).CopyTo(bData, 0x1B0);
                }
                else //当前为串口
                {
                    cDeviceParameter.SensorAdjustValue.CopyTo(bData, 0x1C0);
                }

                //

                if (null != cDeviceParameter.Parameter)
                {
                    Int32 i = 0;

                    for (i = 0; i < cDeviceParameter.Parameter.Count; i++)
                    {
                        BitConverter.GetBytes(cDeviceParameter.Parameter[i]).CopyTo(bData, 0x1E0 + i * 0xB0);
                    }
                }
                binarywriter.Write(bData);

                binarywriter.Close();
                filestream.Close();

                _UpdateEjectPixelMax();//当前串口，更新基准值上限
            }
            catch (Exception ex)
            {
            	
            }

            if (null != binaryreader)
            {
                binaryreader.Close();
            }

            if (null != filestream1)
            {
                filestream1.Close();
            }

            if (null != binarywriter)
            {
                binarywriter.Close();
            }

            if (null != filestream)
            {
                filestream.Close();
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：保存工具数据
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _SaveTool()
        {
            FileStream filestream1 = null;
            BinaryReader binaryreader = null;

            FileStream filestream = null;
            BinaryWriter binarywriter = null;

            try
            {
                filestream1 = new FileStream(sDataPath + sToolFileName, FileMode.Open);
                binaryreader = new BinaryReader(filestream1);//读取文件数据

                Byte[] bData = binaryreader.ReadBytes((Int32)filestream1.Length);

                binaryreader.Close();
                filestream1.Close();

                for (Int32 i = 0; i < tools.Count; i++)
                {
                    tools[i]._Save(i, ref bData);

                    if (tools[i].ExistTolerance) //当前工具存在公差
                    {
                        tolerances.GraphData[tools[i].TolerancesIndex].EffectiveMax_Value = tools[i].Max;//更新公差上限
                        tolerances.GraphData[tools[i].TolerancesIndex].EffectiveMin_Value = tools[i].Min;//更新公差下限
                    }
                }

                filestream = new FileStream(sDataPath + sToolFileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, 8192, FileOptions.WriteThrough);
                binarywriter = new BinaryWriter(filestream);

                binarywriter.Write(bData);

                binarywriter.Close();
                filestream.Close();
            }
            catch (Exception ex)
            {
            	
            }

            if (null != binaryreader)
            {
                binaryreader.Close();
            }

            if (null != filestream1)
            {
                filestream1.Close();
            }

            if (null != binarywriter)
            {
                binarywriter.Close();
            }

            if (null != filestream)
            {
                filestream.Close();
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：保存公差数据
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _SaveTolerances()
        {
            if (tolerances._SaveParameter())
            {
                for (Int32 i = 0; i < tools.Count; i++)//获取公差有效上下限
                {
                    if (tools[i].ExistTolerance)//获取公差有效上下限
                    {
                        tools[i].Min = tolerances.GraphData[tools[i].TolerancesIndex].EffectiveMin_Value;
                        tools[i].Max = tolerances.GraphData[tools[i].TolerancesIndex].EffectiveMax_Value;
                    }
                }
            }
        }

        //
        //-----------------------------------------------------------------------
        // 功能说明：读取关联信息
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        private void _ReadRelevancy()
        {
            Int32 iPortIndex = DeviceInformation.Port - 1;

            FileStream filestream = null;
            BinaryReader binaryReader = null;
            
            try
            {
                filestream = new FileStream(sConfigDataPath + Class.System.RelevancyFileName, FileMode.Open);
                binaryReader = new BinaryReader(filestream);

                filestream.Seek(0x001 + iPortIndex * 0x210, SeekOrigin.Begin);
                dRelevancyCameraInfo.rRelevancyType = (Enum.RelevancyType)binaryReader.ReadByte();//读取关联

                if (Enum.RelevancyType.None < dRelevancyCameraInfo.rRelevancyType) //存在关联相机
                {
                    filestream.Seek(0x002 + iPortIndex * 0x210, SeekOrigin.Begin);
                    Int32 iCount = binaryReader.ReadByte();//关联个数

                    filestream.Seek(0x010 + iPortIndex * 0x210, SeekOrigin.Begin);
                    for (Int32 i = 0; i < iCount; i++)
                    {
                        Enum.CameraType cameraType = (Enum.CameraType)binaryReader.ReadByte();
                        dRelevancyCameraInfo.RelevancyCameraInfo.Add(cameraType, binaryReader.ReadByte());
                    }
                }
                binaryReader.Close();
                filestream.Close();
            }
            catch (Exception ex)
            {
                dRelevancyCameraInfo.rRelevancyType = Enum.RelevancyType.None;
            }

            if (null != binaryReader)
            {
                binaryReader.Close();
            }

            if (null != filestream)
            {
                filestream.Close();
            }
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：读取客户端挂载相机使能信息（Controller使用）
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：Byte，返回客户端挂载相机使能信息
        //----------------------------------------------------------------------
        public static Byte _ReadCameraChooseState(string sPath)
        {
            Byte cameraChooseState = 0;

            FileStream filestream = new FileStream(sPath + System.ConfigDataPathName + CameraFileName, FileMode.Open);
            BinaryReader binaryReader = new BinaryReader(filestream);

            filestream.Seek(0x00, SeekOrigin.Begin);
            cameraChooseState = binaryReader.ReadByte();//读取配置相机个数

            binaryReader.Close();
            filestream.Close();

            //

            return cameraChooseState;
        }

        //-----------------------------------------------------------------------
        // 功能说明：读取故障统计文件数据
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：Struct.FaultMessage[]，返回所有故障信息
        //----------------------------------------------------------------------
        public Struct.FaultMessage[] _ReadFaultStaticsFile()
        {
            FileStream filestream = null;
            BinaryReader binaryreader = null;

            try
            {
                string faultStaticsFilePath = sDataPath + FaultStaticsFileName;

                if (File.Exists(faultStaticsFilePath))//故障统计文件存在
                {
                    filestream = new FileStream(faultStaticsFilePath, FileMode.Open);
                    binaryreader = new BinaryReader(filestream);

                    Int32 faultNumber = Convert.ToInt32(filestream.Length / 14);//读入故障信息总数目

                    if (faultNumber > 0)//存在故障统计信息
                    {
                        fFaultMessages = new Struct.FaultMessage[faultNumber];

                        for (Int32 i = 0; i < faultNumber; i++)//读取故障信息
                        {
                            fFaultMessages[i] = new Struct.FaultMessage();
                            fFaultMessages[i]._InitData();

                            fFaultMessages[i].DataIndex = binaryreader.ReadInt32();//读取故障类型索引
                            fFaultMessages[i].TimeData.Year = binaryreader.ReadUInt16();//读取故障发生时间年
                            fFaultMessages[i].TimeData.Month = binaryreader.ReadUInt16();//读取故障发生时间月
                            fFaultMessages[i].TimeData.Day = binaryreader.ReadUInt16();//读取故障发生时间日
                            fFaultMessages[i].TimeData.Hour = binaryreader.ReadUInt16();//读取故障发生时间小时
                            fFaultMessages[i].TimeData.Minute = binaryreader.ReadUInt16();//读取故障发生时间分钟
                        }
                    }
                    binaryreader.Close();//关闭故障统计文件
                    filestream.Close();
                }
            }
            catch (Exception ex)
            {
            }

            if (null != binaryreader)
            {
                binaryreader.Close();
            }

            if (null != filestream)
            {
                filestream.Close();
            }

            return fFaultMessages;
        }

        //-----------------------------------------------------------------------
        // 功能说明：删除故障统计文件
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _DeleteFaultStaticsFile()
        {
            string faultStaticsFilePath = sDataPath + FaultStaticsFileName;

            if (File.Exists(faultStaticsFilePath))//故障统计文件存在，删除故障统计文件
            {
                File.Delete(faultStaticsFilePath);
            }

            fFaultMessages = null;
            fCurrentFaultMessage._InitData();
        }

        //-----------------------------------------------------------------------
        // 功能说明：写入故障统计信息
        // 输入参数：1、UInt64：machineFaultState，机器故障标记
        //           2、UInt64：machineFaultEnbleState，机器故障标记
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _GetCurrentFaultMessage(UInt64 machineFaultState, UInt64 machineFaultEnbleState)
        {
            if (machineFaultState != 0)//当前存在故障信息
            {
                UInt64 machineFaultStateTemp = machineFaultState & uiCameraFaultState & machineFaultEnbleState;

                fCurrentFaultMessage._InitData();

                if ((((machineFaultStateTemp >> 1) & 1) != 0)) //工业相机1故障
                {
                    fCurrentFaultMessage.DataIndex = 2;
                    return;
                }
                else if ((((machineFaultStateTemp >> 2) & 1) != 0)) //工业相机2故障
                {
                    fCurrentFaultMessage.DataIndex = 3;
                    return;
                }
                else if ((((machineFaultStateTemp >> 40) & 1) != 0)) //工业相机3故障
                {
                    fCurrentFaultMessage.DataIndex = 41;
                    return;
                }
                else if ((((machineFaultStateTemp >> 41) & 1) != 0)) //工业相机4故障
                {
                    fCurrentFaultMessage.DataIndex = 42;
                    return;
                }
                else
                {
                    for (Int32 i = 63; i > 0; i--)//查询故障信息
                    {
                        if (((machineFaultStateTemp >> i) & 1) != 0)//当前系统存在故障
                        {
                            fCurrentFaultMessage.DataIndex = i + 1;
                            return;
                        }
                    }
                }
            }
            else
            {
                fCurrentFaultMessage._InitData();
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：写入故障统计信息
        // 输入参数：1、UInt64：machineFaultState，机器故障标记
        //           2、UInt64：machineFaultSaveState，机器故障标记
        //           3、UInt64：machineFaultEnbleState，机器故障标记
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _WriteFaultStaticsFile(ref UInt64 machineFaultState, ref UInt64 machineFaultSaveState, UInt64 machineFaultEnbleState)
        {
            if (machineFaultState != 0)//当前存在故障信息
            {
                UInt64 machineFaultStateTemp = machineFaultState & uiCameraFaultState & machineFaultEnbleState;

                if ((machineFaultStateTemp != 0) && ((machineFaultStateTemp ^ machineFaultSaveState) != 0)) //当前存在故障信息未保存
                {
                    FileStream filestream = new FileStream(sDataPath + FaultStaticsFileName, FileMode.Append, FileAccess.Write, FileShare.None, 8192, FileOptions.WriteThrough); //打开故障信息统计文件
                    BinaryWriter binarywriter = new BinaryWriter(filestream);//写入系统文件数据

                    for (Int32 i = 0; i < 64; i++)//查询故障信息
                    {
                        if (((machineFaultStateTemp >> i) & 1) != 0)//当前系统存在故障
                        {
                            if (((machineFaultSaveState >> i) & 1) == 0)//当前故障信息未保存
                            {
                                machineFaultSaveState = machineFaultSaveState | ((UInt64)1 << i);//标记当前故障保存

                                Struct.SYSTEMTIME systemTime = new Struct.SYSTEMTIME();
                                systemTime._InitData(DateTime.Now);

                                binarywriter.Write(i + 1);//写入故障索引
                                binarywriter.Write(systemTime.Year);//写入故障发生时间年
                                binarywriter.Write(systemTime.Month);//写入故障发生时间月
                                binarywriter.Write(systemTime.Day);//写入故障发生时间日
                                binarywriter.Write(systemTime.Hour);//写入故障发生时间小时
                                binarywriter.Write(systemTime.Minute);//写入故障发生时间分钟
                            }
                        }
                        else  //当前系统运行正常，没有故障
                        {
                            if (((uiCameraFaultState >> i) & 1) != 0)//清除当前相机故障标记
                            {
                                machineFaultSaveState = machineFaultSaveState & (~((UInt64)1 << i));       //当前故障标志清零
                            }
                        }
                    }
                    binarywriter.Close();//关闭故障统计文件
                    filestream.Close();
                }
            }
            else
            {
                machineFaultSaveState = 0;
            }
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明： 自学习函数
        // 输入参数： 1、Image<Bgr, Byte>：image，待处理的图像
        //            2、Boolean：flag，自学习后是否更新最大最小值
        //            3、Boolean：state，烟支处理时是否执行重新自学习
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        public void _LearnSample(Image<Bgr, Byte> image, Boolean flag = true, Boolean state = true)
        {
            ReferenceHorizenL_Sample = 0;
            ReferenceHorizenR_Sample = 0;

            ReferenceVerticalL_Sample = 0;
            ReferenceVerticalR_Sample = 0;

            ReferenceHorizenPoint = 0;
            ReferenceVerticalPoint = 0;

            ReferenceHorizenL = 0;
            ReferenceHorizenR = 0;

            ReferenceVerticalL = 0;
            ReferenceVerticalR = 0;

            for (int i = 0; i < tools.Count; i++)
            {
                if (tools[i].ReferenceH_Exist && tools[i].ReferenceV_Exist)//如果存在水平基准属性
                {
                    tools[i]._LearnSample(image, flag);

                    if (tools[i].ToolState)
                    {
                        ReferenceHorizenPoint = ReferenceHorizenL_Sample = ReferenceHorizenR_Sample = tools[i].SampleValue[0];//自学习水平基准左边缘值更新
                        ReferenceVerticalPoint = ReferenceVerticalL_Sample = ReferenceVerticalR_Sample = tools[i].SampleValue[1];//自学习垂直基准上边缘值更新
                    }
                }
                else if (tools[i].ReferenceH_Exist)//如果存在水平基准属性
                {
                    tools[i]._LearnSample(image, flag);

                    if (tools[i].ToolState)
                    {
                        switch ((Enum.Scan_Direction)tools[i].Arithmetic.EnumCurrent[1] + 1)
                        {
                            case Enum.Scan_Direction.Left_Right://水平方向，增量方向
                                ReferenceHorizenPoint = ReferenceHorizenL_Sample = ReferenceHorizenR_Sample = tools[i].SampleValue[0];//自学习水平基准左边缘值更新
                                break;
                            case Enum.Scan_Direction.Right_Left://水平方向，减量方向
                                ReferenceHorizenPoint = ReferenceHorizenR_Sample = ReferenceHorizenL_Sample = tools[i].SampleValue[0];//自学习水平基准右边缘值更新
                                break;
                            case Enum.Scan_Direction.Horizen://水平方向，从两边向中间找
                                ReferenceHorizenPoint = ReferenceHorizenL_Sample = tools[i].Ruler.PosSmall_Sample;//自学习水平基准左边缘值更新
                                ReferenceHorizenR_Sample = tools[i].Ruler.PosBig_Sample;//自学习水平基准右边缘值更新
                                break;
                            default:
                                ReferenceHorizenPoint = ReferenceHorizenL_Sample = ReferenceHorizenR_Sample = tools[i].SampleValue[0];//自学习水平基准左边缘值更新
                                break;
                        }
                    }
                }
                else if (tools[i].ReferenceV_Exist)//如果存在垂直基准属性
                {
                    tools[i]._LearnSample(image, flag);

                    if (tools[i].ToolState)
                    {
                        switch ((Enum.Scan_Direction)tools[i].Arithmetic.EnumCurrent[1] + 1)
                        {
                            case Enum.Scan_Direction.Top_Bottom://垂直方向，增量方向
                                ReferenceVerticalPoint = ReferenceVerticalL_Sample = ReferenceVerticalR_Sample = tools[i].SampleValue[0];//自学习垂直基准上边缘值更新
                                break;
                            case Enum.Scan_Direction.Bottom_Top://垂直方向，减量方向
                                ReferenceVerticalPoint = ReferenceVerticalR_Sample = ReferenceVerticalL_Sample = tools[i].SampleValue[0];//自学习垂直基准下边缘值更新
                                break;
                            case Enum.Scan_Direction.Vertical://垂直方向，从上下向中间找
                                ReferenceVerticalPoint = ReferenceVerticalL_Sample = tools[i].Ruler.PosSmall_Sample;//自学习垂直基准上边缘值更新
                                ReferenceVerticalR_Sample = tools[i].Ruler.PosBig_Sample;//自学习垂直基准下边缘值更新
                                break;
                            default:
                                ReferenceVerticalPoint = ReferenceVerticalL_Sample = ReferenceVerticalR_Sample = tools[i].SampleValue[0];//自学习垂直基准上边缘值更新
                                break;
                        }
                    }
                }
                else
                {
                    if (tools[i].ToolState)
                    {
                        tools[i].ReferenceHorizenPoint = ReferenceHorizenPoint;
                        tools[i].ReferenceVerticalPoint = ReferenceVerticalPoint;
                    }

                    tools[i]._LearnSample(image, flag);
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明： 图像处理函数
        // 输入参数： image:待处理的图像
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        public Boolean[] _ImageProcess(Image<Bgr, Byte> image)
        {
            ReferenceHorizenPoint = 0;
            ReferenceVerticalPoint = 0;

            ReferenceHorizenL = 0;
            ReferenceHorizenR = 0;

            ReferenceVerticalL = 0;
            ReferenceVerticalR = 0;

            Boolean[] result = new Boolean[tools.Count];

            for (int i = 0; i < tools.Count; i++)
            {
                if (tools[i].ToolState)
                {
                    if (tools[i].ReferenceH_Exist && tools[i].ReferenceV_Exist)//如果存在水平基准属性
                    {
                        result[i] = tools[i]._ImageProcess(image);

                        ReferenceHorizenPoint = ReferenceHorizenL = ReferenceHorizenR = tools[i].Value[0];//水平基准左边缘值更新
                        ReferenceVerticalPoint = ReferenceVerticalL = ReferenceVerticalR = tools[i].Value[1];//垂直基准上边缘值更新
                    }
                    else if (tools[i].ReferenceH_Exist)//如果存在水平基准属性
                    {
                        result[i] = tools[i]._ImageProcess(image);

                        switch ((Enum.Scan_Direction)tools[i].Arithmetic.EnumCurrent[1] + 1)
                        {
                            case Enum.Scan_Direction.Left_Right://水平方向，增量方向
                                ReferenceHorizenPoint = ReferenceHorizenL = ReferenceHorizenR = tools[i].Value[0];//水平基准左边缘值更新
                                break;
                            case Enum.Scan_Direction.Right_Left://水平方向，减量方向
                                ReferenceHorizenPoint = ReferenceHorizenR = ReferenceHorizenL = tools[i].Value[0];//水平基准右边缘值更新
                                break;
                            case Enum.Scan_Direction.Horizen://水平方向，从两边向中间找
                                ReferenceHorizenPoint = ReferenceHorizenL = tools[i].Ruler.PosSmall_Current;//水平基准左边缘值更新
                                ReferenceHorizenR = tools[i].Ruler.PosBig_Current;//水平基准右边缘值更新
                                break;
                            default:
                               ReferenceHorizenPoint = ReferenceHorizenL = ReferenceHorizenR = tools[i].Value[0];//水平基准左边缘值更新
                                break;
                        }
                    }
                    else if (tools[i].ReferenceV_Exist)//如果存在垂直基准属性
                    {
                        result[i] = tools[i]._ImageProcess(image);

                        switch ((Enum.Scan_Direction)tools[i].Arithmetic.EnumCurrent[1] + 1)
                        {
                            case Enum.Scan_Direction.Top_Bottom://垂直方向，增量方向
                                ReferenceVerticalPoint = ReferenceVerticalL = ReferenceVerticalR = tools[i].Value[0];//垂直基准上边缘值更新
                                break;
                            case Enum.Scan_Direction.Bottom_Top://垂直方向，减量方向
                                ReferenceVerticalPoint = ReferenceVerticalR = ReferenceVerticalL = tools[i].Value[0];//垂直基准下边缘值更新
                                break;
                            case Enum.Scan_Direction.Vertical://垂直方向，从上下向中间找
                                ReferenceVerticalPoint = ReferenceVerticalL = tools[i].Ruler.PosSmall_Current;//垂直基准上边缘值更新
                                ReferenceVerticalR = tools[i].Ruler.PosBig_Current;//垂直基准下边缘值更新
                                break;
                            default:
                                ReferenceVerticalPoint = ReferenceVerticalL = ReferenceVerticalR = tools[i].Value[0];//垂直基准上边缘值更新
                                break;
                        }
                    }
                    else
                    {
                        //if ((ReferenceHorizenPoint - ReferenceHorizenL_Sample) <= 20)
                        //{
                        //    tools[i].Compensation_H = ReferenceHorizenL - ReferenceHorizenL_Sample;//水平方向补偿值更新
                        //}
                        //else
                        //{
                        //    tools[i].Compensation_H = 0;
                        //}

                        //if ((ReferenceVerticalPoint - ReferenceVerticalL_Sample) <= 30)
                        //{
                        //    tools[i].Compensation_V = ReferenceVerticalL - ReferenceVerticalL_Sample;//垂直方向补偿值更新
                        //}
                        //else
                        //{
                        //    tools[i].Compensation_V = 0;
                        //}

                        tools[i].Compensation_H = ReferenceHorizenL - ReferenceHorizenL_Sample;//水平方向补偿值更新
                        tools[i].Compensation_V = ReferenceVerticalL - ReferenceVerticalL_Sample;//垂直方向补偿值更新

                        tools[i].ReferenceHorizenPoint = ReferenceHorizenPoint;
                        tools[i].ReferenceVerticalPoint = ReferenceVerticalPoint;
                        
                        result[i] = tools[i]._ImageProcess(image);
                    }
                }
                else
                {
                    result[i] = true;
                }
            }

            return result;
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：初始化函数
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        private void _Init()
        {
            Int32 i = 0;//循环控制变量
            Int32 j = 0;//循环控制变量

            //初始化

            tools = new List<Tools>();

            tolerances = new TolerancesData();

            dDeviceInformation.Connected = false;//是否与客户端建立了连接。取值范围：true，是；false，否
            dDeviceInformation.GetDevInfo = false;//是否存储了客户端的设备信息。取值范围：true，是；false，否
            dDeviceInformation.SerialNumber = "";//序列号
            dDeviceInformation.MAC = "";//MAC地址
            dDeviceInformation.IP = "";//IP地址
            dDeviceInformation.Firmware = "";//固件版本
            dDeviceInformation.DeviceName = "";//设备（相机）名称
            dDeviceInformation.ControllerName = "";//控制器名称
            dDeviceInformation.DeviceName = "";//设备名称
            dDeviceInformation.Type = Enum.CameraType.None;//相机类型
            dDeviceInformation.CAM = Enum.CameraState.NOTCONNECTED;//相机状态
            dDeviceInformation.Port = 1;//相机端口

            lLive.CameraSelected = false;//相机显示控件是否被选中。true：是；false：否
            lLive.GraphicsInformation = new Struct.ImageInformation();//图像信息
            lLive.GraphicsInformation.Valid = false;//图像是否有效。true：是；false：否
            lLive.GraphicsInformation.Scale = 1.0;//缩放比例
            lLive.GraphicsInformation.ToolsIndex = 0;//图像所属的工具索引值（从0开始）
            lLive.GraphicsInformation.Type = Enum.ImageType.Error;//图像类型
            lLive.GraphicsInformation.Name = "";//信息名称
            lLive.GraphicsInformation.Value = new bool[Struct.ImageInformation.TotalNumber];//区块的数值。取值范围：true，表示区块有效；false，表示区块无效
            for (i = 0; i < Struct.ImageInformation.TotalNumber; i++)
            {
                lLive.GraphicsInformation.Value[i] = false;
            }
            lLive.GraphicsInformation.ValueDisplay = true;//在显示图像的标题栏中是否显示最小值、最大值和当前值。true：是；false：否
            lLive.GraphicsInformation.MinValue = 0;//最小值
            lLive.GraphicsInformation.MaxValue = 0;//最大值
            lLive.GraphicsInformation.CurrentValue = 0;//当前值
            lLive.GraphicsInformation.DateTimeImage = new DateTime();//图像产生的时间
            lLive.GraphicsInformation.ErrorValue = 0;//图像显示的Error数值（取值为-1，表示该数值无意义，即在图像上不显示）
            lLive.GraphicsInformation.StepValue = 0;//图像显示的Step数值（取值为-1，表示该数值无意义，即在图像上不显示）

            rRejects.Cameratype = Enum.CameraType.Camera_1;//相机类型（默认值：CameraType.Top）
            rRejects.GraphicsInformation = new Struct.ImageInformation();//属性，图像信息
            rRejects.GraphicsInformation.Valid = false;//图像是否有效。true：是；false：否
            rRejects.GraphicsInformation.Scale = 1.0;//缩放比例
            rRejects.GraphicsInformation.ToolsIndex = 0;//图像所属的工具索引值（从0开始）
            rRejects.GraphicsInformation.Type = Enum.ImageType.Error;//图像类型
            rRejects.GraphicsInformation.Name = "";//信息名称
            rRejects.GraphicsInformation.Value = new bool[Struct.ImageInformation.TotalNumber];//区块的数值。取值范围：true，表示区块有效；false，表示区块无效
            for (i = 0; i < Struct.ImageInformation.TotalNumber; i++)
            {
                rRejects.GraphicsInformation.Value[i] = false;
            }
            rRejects.GraphicsInformation.ValueDisplay = true;//在显示图像的标题栏中是否显示最小值、最大值和当前值。true：是；false：否
            rRejects.GraphicsInformation.MinValue = 0;//最小值
            rRejects.GraphicsInformation.MaxValue = 0;//最大值
            rRejects.GraphicsInformation.CurrentValue = 0;//当前值
            rRejects.GraphicsInformation.DateTimeImage = new DateTime();//图像产生的时间
            rRejects.GraphicsInformation.ErrorValue = 0;//图像显示的Error数值（取值为-1，表示该数值无意义，即在图像上不显示）
            rRejects.GraphicsInformation.StepValue = 0;//图像显示的Step数值（取值为-1，表示该数值无意义，即在图像上不显示）

            lLearn = new Struct.ImageInformation();//图像信息
            lLearn.Valid = false;//图像是否有效。true：是；false：否
            lLearn.Scale = 1.0;//缩放比例
            lLearn.ToolsIndex = 0;//图像所属的工具索引值（从0开始）
            lLearn.Type = Enum.ImageType.Error;//图像类型
            lLearn.Name = "";//信息名称
            lLearn.Value = new bool[Struct.ImageInformation.TotalNumber];//区块的数值。取值范围：true，表示区块有效；false，表示区块无效
            for (i = 0; i < Struct.ImageInformation.TotalNumber; i++)
            {
                lLearn.Value[i] = false;
            }
            lLearn.ValueDisplay = true;//在显示图像的标题栏中是否显示最小值、最大值和当前值。true：是；false：否
            lLearn.MinValue = 0;//最小值
            lLearn.MaxValue = 0;//最大值
            lLearn.CurrentValue = 0;//当前值
            lLearn.DateTimeImage = new DateTime();//图像产生的时间
            lLearn.ErrorValue = 0;//图像显示的Error数值（取值为-1，表示该数值无意义，即在图像上不显示）
            lLearn.StepValue = 0;//图像显示的Step数值（取值为-1，表示该数值无意义，即在图像上不显示）

            cDeviceParameter = new Struct.CameraParameter();//相机设备参数

            cDeviceParameter.StroboTime_Valid = true;
            cDeviceParameter.StroboTime = 2;//照明灯启动信号的持续时间
            cDeviceParameter.StroboTime_Min = 1;//照明灯启动信号的持续时间最小值
            cDeviceParameter.StroboTime_Max = 10;//照明灯启动信号的持续时间最大值

            cDeviceParameter.StroboCurrent_Valid = true;
            cDeviceParameter.StroboCurrent = 1000;//照明灯启动信号的强度
            cDeviceParameter.StroboCurrent_Min = 1;//照明灯启动信号的强度最小值
            cDeviceParameter.StroboCurrent_Max = 3600;//照明灯启动信号的强度最大值

            cDeviceParameter.Gain_Valid = true;
            cDeviceParameter.Gain = 20;//增益
            cDeviceParameter.Gain_Min = 16;//增益最小值
            cDeviceParameter.Gain_Max = 63;//增益最大值

            cDeviceParameter.ExposureTime_Valid = true;
            cDeviceParameter.ExposureTime = 1;//曝光时间
            cDeviceParameter.ExposureTime_Min = 1;//曝光时间最小值
            cDeviceParameter.ExposureTime_Max = 30;//曝光时间最大值

            cDeviceParameter.WhiteBalance_Valid = true;
            cDeviceParameter.WhiteBalance = 0;//白平衡（自动/手动）

            cDeviceParameter.WhiteBalance_Red_Valid = true;
            cDeviceParameter.WhiteBalance_Red = 60;//白平衡（红）
            cDeviceParameter.WhiteBalance_Red_Min = 0;//白平衡（红）最小值
            cDeviceParameter.WhiteBalance_Red_Max = 255;//白平衡（红）最大值

            cDeviceParameter.WhiteBalance_Green_Valid = true;
            cDeviceParameter.WhiteBalance_Green = 60;//白平衡（绿）
            cDeviceParameter.WhiteBalance_Green_Min = 0;//白平衡（绿）最小值
            cDeviceParameter.WhiteBalance_Green_Max = 255;//白平衡（绿）最大值

            cDeviceParameter.WhiteBalance_Blue_Valid = true;
            cDeviceParameter.WhiteBalance_Blue = 60;//白平衡（蓝）
            cDeviceParameter.WhiteBalance_Blue_Min = 0;//白平衡（蓝）最小值
            cDeviceParameter.WhiteBalance_Blue_Max = 255;//白平衡（蓝）最大值

            cDeviceParameter.Parameter = new List<int>();//参数
            cDeviceParameter.Parameter_Min = new List<int>();//参数，最小值
            cDeviceParameter.Parameter_Max = new List<int>();//参数，最大值
            cDeviceParameter.Parameter_NameCHN = new List<string>();//参数，中文名称
            cDeviceParameter.Parameter_NameENG = new List<string>();//参数，英文名称

            cDeviceParameter.SensorSelectState = 0xFFFFF;

            cDeviceParameter.SensorAdjustValue = null;//传感器校准值
            cDeviceParameter.SensorADCValueMax = null;//传感器ADC值
            
            uiCameraFaultState = 0;

            ReferenceHorizenPoint = 0;//基准定位点水平基准坐标
            ReferenceVerticalPoint = 0;//基准定位点垂直基准标准

            fCurrentFaultMessage = new Struct.FaultMessage();

            bCheckEnable = true;
            bCameraAngle = Enum.CameraRotateAngle.Angle_0;

            bBitmapLockBitsResize = false;
            bBitmapLockBitsCenter = false;
            pBitmapLockBitsAxis = new Point();
            rBitmapLockBitsArea = new Rectangle(0, 0, 744, 480);

            bSerialPort = false;
            tTobaccoSortType_E = 0;

            vVideoColor = Enum.VideoColor.RGB32;
            vVideoResolution = Enum.VideoResolution._744x480;

            sVideoFormat = "RGB32 (744x480)";

            bCameraFlip = 0;
            bSensorNumber = 20;

            dRelevancyCameraInfo = new Struct.RelevancyCameraInformation();
            dRelevancyCameraInfo.RelevancyCameraInfo = new Dictionary<Enum.CameraType, Byte>();

            eSensor_ProductType = Enum.SensorProductType._89713FC;
            bContinuousSampling = false;
            bEncoderPer = 1;

            bSerialPort_BaudRate = Enum.SerialPortBaudRate._57600;
            bSerialPort_ReceivedBytesThreshold = 46;
            bSerialPort_SendBytesThreshold = 24;

            lPerTobaccoNumber = new List<Byte>();
        }
        
        //-----------------------------------------------------------------------
        // 功能说明：构造函数中调用，初始化，读取文件数据
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        private void _Camera_Read_Init()
        {
            Int32 i = 0;//循环控制变量

            //剔除

            rRejects.Cameratype = cType;

            //工具

            FileStream filestream = new FileStream(sDataPath + sToolFileName, FileMode.Open);
            BinaryReader binaryreader = new BinaryReader(filestream);

            filestream.Seek(0x00, SeekOrigin.Begin);
            Byte iToolNumber = binaryreader.ReadByte();//工具数目

            binaryreader.Close();
            filestream.Close();

            //

            tools = new List<Tools>();
            for (i = 0; i < iToolNumber; i++)//创建对象
            {
                Tools toolTemp = new Tools(i, sDataPath);//创建对象，读取文件数据

                toolTemp.Sensor_ProductType = eSensor_ProductType;
                toolTemp.SensorNumber = bSensorNumber;

                toolTemp.ImageWidth = uiImageWidth;
                toolTemp.ImageHeight = uiImageHeight;

                tools.Add(toolTemp);

                if(toolTemp.DeepLearningState)//包含/深度学习工具
                {
                    bDeepLearningState = true;
                }
            }

            //公差

            tolerances = new TolerancesData(sDataPath);

            for (i = 0; i < iToolNumber; i++)//获取公差有效上下限
            {
                if (tools[i].ExistTolerance)
                {
                    tools[i].Min = tolerances.GraphData[tools[i].TolerancesIndex].EffectiveMin_Value;
                    tools[i].Max = tolerances.GraphData[tools[i].TolerancesIndex].EffectiveMax_Value;
                }

                if (tools[i].FilterCheck)//烟支检测
                {
                    tools[i].EjectLevel = tolerances.EjectLevel;//更新灵敏度                 
                }

                tools[i].PixelPerMm = tolerances.GraphData[0].AdditionalValueRatio;
            }

            //读取相机参数
            _ReadParameter();

            //更新传感器最大ADC值
            _UpdateSensorADCValueMax();
            
            //读取学习图像、剔除图像

            if (File.Exists(sSampleImagePath + SampleImageFileName + sBMPFile) && File.Exists(sSampleImagePath + sSampleDataFileName))//文件存在
            {
                _ReadImage(Enum.ImageInformationType.Sample);//学习图像信息

                _LearnSample(iImageLearn, false);//图像学习
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：读取相机光源参数信息
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _ReadParameter()
        {
            FileStream filestream = null;
            BinaryReader binaryreader = null;

            try
            {
                filestream = new FileStream(sDataPath + sParameterFileName, FileMode.Open);
                binaryreader = new BinaryReader(filestream);

                if (false == IsSerialPort) //当前为相机
                {
                    filestream.Seek(0x000, SeekOrigin.Begin);
                    cDeviceParameter.StroboTime_Valid = binaryreader.ReadBoolean();
                    filestream.Seek(0x010, SeekOrigin.Begin);
                    cDeviceParameter.StroboTime = binaryreader.ReadUInt16();
                    filestream.Seek(0x020, SeekOrigin.Begin);
                    cDeviceParameter.StroboTime_Min = binaryreader.ReadUInt16();
                    filestream.Seek(0x030, SeekOrigin.Begin);
                    cDeviceParameter.StroboTime_Max = binaryreader.ReadUInt16();

                    filestream.Seek(0x040, SeekOrigin.Begin);
                    cDeviceParameter.StroboCurrent_Valid = binaryreader.ReadBoolean();
                    filestream.Seek(0x050, SeekOrigin.Begin);
                    cDeviceParameter.StroboCurrent = binaryreader.ReadUInt16();
                    filestream.Seek(0x060, SeekOrigin.Begin);
                    cDeviceParameter.StroboCurrent_Min = binaryreader.ReadUInt16();
                    filestream.Seek(0x070, SeekOrigin.Begin);
                    cDeviceParameter.StroboCurrent_Max = binaryreader.ReadUInt16();

                    filestream.Seek(0x080, SeekOrigin.Begin);
                    cDeviceParameter.Gain_Valid = binaryreader.ReadBoolean();
                    filestream.Seek(0x090, SeekOrigin.Begin);
                    cDeviceParameter.Gain = binaryreader.ReadUInt16();
                    filestream.Seek(0x0A0, SeekOrigin.Begin);
                    cDeviceParameter.Gain_Min = binaryreader.ReadUInt16();
                    filestream.Seek(0x0B0, SeekOrigin.Begin);
                    cDeviceParameter.Gain_Max = binaryreader.ReadUInt16();

                    filestream.Seek(0x0C0, SeekOrigin.Begin);
                    cDeviceParameter.ExposureTime_Valid = binaryreader.ReadBoolean();
                    filestream.Seek(0x0D0, SeekOrigin.Begin);
                    cDeviceParameter.ExposureTime = binaryreader.ReadUInt16();
                    filestream.Seek(0x0E0, SeekOrigin.Begin);
                    cDeviceParameter.ExposureTime_Min = binaryreader.ReadUInt16();
                    filestream.Seek(0x0F0, SeekOrigin.Begin);
                    cDeviceParameter.ExposureTime_Max = binaryreader.ReadUInt16();

                    filestream.Seek(0x100, SeekOrigin.Begin);
                    cDeviceParameter.WhiteBalance_Valid = binaryreader.ReadBoolean();
                    filestream.Seek(0x110, SeekOrigin.Begin);
                    cDeviceParameter.WhiteBalance = binaryreader.ReadUInt16();

                    filestream.Seek(0x120, SeekOrigin.Begin);
                    cDeviceParameter.WhiteBalance_Red_Valid = binaryreader.ReadBoolean();
                    filestream.Seek(0x130, SeekOrigin.Begin);
                    cDeviceParameter.WhiteBalance_Red = binaryreader.ReadUInt16();
                    filestream.Seek(0x140, SeekOrigin.Begin);
                    cDeviceParameter.WhiteBalance_Red_Min = binaryreader.ReadUInt16();
                    filestream.Seek(0x150, SeekOrigin.Begin);
                    cDeviceParameter.WhiteBalance_Red_Max = binaryreader.ReadUInt16();

                    filestream.Seek(0x160, SeekOrigin.Begin);
                    cDeviceParameter.WhiteBalance_Green_Valid = binaryreader.ReadBoolean();
                    filestream.Seek(0x170, SeekOrigin.Begin);
                    cDeviceParameter.WhiteBalance_Green = binaryreader.ReadUInt16();
                    filestream.Seek(0x180, SeekOrigin.Begin);
                    cDeviceParameter.WhiteBalance_Green_Min = binaryreader.ReadUInt16();
                    filestream.Seek(0x190, SeekOrigin.Begin);
                    cDeviceParameter.WhiteBalance_Green_Max = binaryreader.ReadUInt16();

                    filestream.Seek(0x1A0, SeekOrigin.Begin);
                    cDeviceParameter.WhiteBalance_Blue_Valid = binaryreader.ReadBoolean();
                    filestream.Seek(0x1B0, SeekOrigin.Begin);
                    cDeviceParameter.WhiteBalance_Blue = binaryreader.ReadUInt16();
                    filestream.Seek(0x1C0, SeekOrigin.Begin);
                    cDeviceParameter.WhiteBalance_Blue_Min = binaryreader.ReadUInt16();
                    filestream.Seek(0x1D0, SeekOrigin.Begin);
                    cDeviceParameter.WhiteBalance_Blue_Max = binaryreader.ReadUInt16();
                }
                else //当前为串口
                {
                    cDeviceParameter._Init(bSensorNumber);
                    filestream.Seek(0x1C0, SeekOrigin.Begin);
                    for (Byte j = 0; j < bSensorNumber; j++) //循环所有烟支，读取校准值
                    {
                        cDeviceParameter.SensorAdjustValue[j] = binaryreader.ReadByte();
                    }
                }

                Int32 i = 0;
                Int32 length = Convert.ToInt32((filestream.Length - filestream.Position) / 176);

                if (length > 0)
                {
                    cDeviceParameter.Parameter = new List<int>();
                    cDeviceParameter.Parameter_Min = new List<int>();
                    cDeviceParameter.Parameter_Max = new List<int>();
                    cDeviceParameter.Parameter_NameCHN = new List<string>();
                    cDeviceParameter.Parameter_NameENG = new List<string>();
                }

                while (i < length)
                {
                    filestream.Seek(0x1E0 + i * 0xB0, SeekOrigin.Begin);
                    cDeviceParameter.Parameter.Add(binaryreader.ReadInt32());
                    filestream.Seek(0x1F0 + i * 0xB0, SeekOrigin.Begin);
                    cDeviceParameter.Parameter_Min.Add(binaryreader.ReadInt32());
                    filestream.Seek(0x200 + i * 0xB0, SeekOrigin.Begin);
                    cDeviceParameter.Parameter_Max.Add(binaryreader.ReadInt32());
                    filestream.Seek(0x210 + i * 0xB0, SeekOrigin.Begin);
                    cDeviceParameter.Parameter_NameCHN.Add(binaryreader.ReadString());
                    filestream.Seek(0x250 + i * 0xB0, SeekOrigin.Begin);
                    cDeviceParameter.Parameter_NameENG.Add(binaryreader.ReadString());

                    i++;
                }

                binaryreader.Close();
                filestream.Close();
            }
            catch (Exception ex)
            {

            }

            if (null != binaryreader)
            {
                binaryreader.Close();
            }

            if (null != filestream)
            {
                filestream.Close();
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：读取图像信息
        // 输入参数：1.fileStream：文件流
        //         2.binaryReader：文件读
        //         3.information：图像信息
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        private void _ReadImageInformation(FileStream fileStream, BinaryReader binaryReader, Struct.ImageInformation information)
        {
            Int32 i = 0;//循环控制变量

            //

            fileStream.Seek(0x000, SeekOrigin.Begin);
            information.Valid = binaryReader.ReadBoolean();//图像是否有效。true：是；false：否

            fileStream.Seek(0x010, SeekOrigin.Begin);
            information.ToolsIndex = binaryReader.ReadInt32();//图像所属的工具索引值（从0开始）
            information.ToolState = binaryReader.ReadBoolean();//工具状态

            fileStream.Seek(0x020, SeekOrigin.Begin);
            information.Type = (Enum.ImageType)binaryReader.ReadByte();//图像类型

            fileStream.Seek(0x030, SeekOrigin.Begin);
            information.Scale = binaryReader.ReadDouble();//缩放比例

            fileStream.Seek(0x040, SeekOrigin.Begin);
            information.Name = binaryReader.ReadString();//信息名称

            if (null == information.Value)//无效
            {
                information.Value = new Boolean[Struct.ImageInformation.TotalNumber];

                for (i = 0; i < Struct.ImageInformation.TotalNumber; i++)//
                {
                    fileStream.Seek((0x070 + i * 0x010), SeekOrigin.Begin);
                    information.Value[i] = binaryReader.ReadBoolean();//区块的数值。取值范围：true，表示区块有效；false，表示区块无效
                }
            }

            fileStream.Seek(0x070 + i * 0x010, SeekOrigin.Begin);
            information.ValueDisplay = binaryReader.ReadBoolean();//在显示图像的标题栏中是否显示最小值、最大值和当前值。true：是；false：否

            fileStream.Seek(0x070 + (i + 1) * 0x010, SeekOrigin.Begin);
            information.MinValue = binaryReader.ReadInt32();//最小值

            fileStream.Seek(0x070 + (i + 2) * 0x010, SeekOrigin.Begin);
            information.MaxValue = binaryReader.ReadInt32();//最大值

            fileStream.Seek(0x070 + (i + 3) * 0x010, SeekOrigin.Begin);
            information.CurrentValue = binaryReader.ReadInt32();//当前值

            fileStream.Seek(0x070 + (i + 4) * 0x010, SeekOrigin.Begin);
            information.DateTimeImage = DateTime.FromBinary(binaryReader.ReadInt64());//图像产生的时间

            fileStream.Seek(0x070 + (i + 5) * 0x010, SeekOrigin.Begin);
            information.ErrorValue = binaryReader.ReadInt16();//图像显示的Error数值（取值为-1，表示该数值无意义，即在图像上不显示）

            fileStream.Seek(0x070 + (i + 6) * 0x010, SeekOrigin.Begin);
            information.StepValue = binaryReader.ReadInt16();//图像显示的Step数值（取值为-1，表示该数值无意义，即在图像上不显示）
        }

        //-----------------------------------------------------------------------
        // 功能说明：写入图像信息
        // 输入参数：1.fileStream：文件流
        //         2.binaryWriter：文件写
        //         3.information：图像信息
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        private void _WriteImageInformation(FileStream fileStream, BinaryWriter binaryWriter, Struct.ImageInformation information)
        {
            Int32 i = 0;//循环控制变量

            //

            fileStream.Seek(0x000, SeekOrigin.Begin);
            binaryWriter.Write(information.Valid);//图像是否有效。true：是；false：否

            fileStream.Seek(0x010, SeekOrigin.Begin);
            binaryWriter.Write(information.ToolsIndex);//图像所属的工具索引值（从0开始）
            binaryWriter.Write(information.ToolState);//工具状态

            fileStream.Seek(0x020, SeekOrigin.Begin);
            binaryWriter.Write((Byte)information.Type);//图像类型

            fileStream.Seek(0x030, SeekOrigin.Begin);
            binaryWriter.Write(information.Scale);//缩放比例

            fileStream.Seek(0x040, SeekOrigin.Begin);
            binaryWriter.Write(information.Name);//信息名称

            if (null == information.Value)//无效
            {
                information.Value = new Boolean[Struct.ImageInformation.TotalNumber];

                for (i = 0; i < Struct.ImageInformation.TotalNumber; i++)//
                {
                    fileStream.Seek((0x070 + i * 0x010), SeekOrigin.Begin);
                    binaryWriter.Write(information.Value[i]);//区块的数值。取值范围：true，表示区块有效；false，表示区块无效
                }
            }

            fileStream.Seek(0x070 + i * 0x010, SeekOrigin.Begin);
            binaryWriter.Write(information.ValueDisplay);//在显示图像的标题栏中是否显示最小值、最大值和当前值。true：是；false：否

            fileStream.Seek(0x070 + (i + 1) * 0x010, SeekOrigin.Begin);
            binaryWriter.Write(information.MinValue);//最小值

            fileStream.Seek(0x070 + (i + 2) * 0x010, SeekOrigin.Begin);
            binaryWriter.Write(information.MaxValue);//最大值

            fileStream.Seek(0x070 + (i + 3) * 0x010, SeekOrigin.Begin);
            binaryWriter.Write(information.CurrentValue);//当前值

            fileStream.Seek(0x070 + (i + 4) * 0x010, SeekOrigin.Begin);
            binaryWriter.Write(information.DateTimeImage.ToBinary());//图像产生的时间

            fileStream.Seek(0x070 + (i + 5) * 0x010, SeekOrigin.Begin);
            binaryWriter.Write(information.ErrorValue);//图像显示的Error数值（取值为-1，表示该数值无意义，即在图像上不显示）

            fileStream.Seek(0x070 + (i + 6) * 0x010, SeekOrigin.Begin);
            binaryWriter.Write(information.StepValue);//图像显示的Step数值（取值为-1，表示该数值无意义，即在图像上不显示）
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：更新检测工具基准值最大值
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        private void _UpdateEjectPixelMax()
        {
            if (IsSerialPort) //当前为串口
            {
                switch (eSensor_ProductType)
                {
                    case Enum.SensorProductType._89713FC:
                        if (cDeviceParameter.SensorADCValueMax.Length > tools.Count) //传感器数量超过工具数量，配置文件检测工具数量较少
                        {
                            for (Byte i = 0; i < tools.Count; i++) //取工具数量为循环上限
                            {
                                tools[i].EjectPixelMax = cDeviceParameter.SensorADCValueMax[i];
                            }
                        }
                        else //传感器数量小于等于工具数量，配置文件检测工具数量较多
                        {
                            for (Byte i = 0; i < cDeviceParameter.SensorADCValueMax.Length; i++) //取传感器数量为循环上限
                            {
                                tools[i].EjectPixelMax = cDeviceParameter.SensorADCValueMax[i];
                            }
                        }
                        break;
                    case Enum.SensorProductType._89713FA:
                        Int32 iToolIndex = 0;
                        for (Byte i = 0; i < lPerTobaccoNumber.Count; i++) //循环所有排数
                        {
                            for (Byte j = 0; j < lPerTobaccoNumber[i]; j++, iToolIndex++) //循环每排烟支数
                            {
                                if (iToolIndex < tools.Count)//未超出工具索引
                                {
                                    tools[iToolIndex].EjectPixelMax = cDeviceParameter.SensorADCValueMax[i];
                                }
                            }
                        }
                        break;

                    case Enum.SensorProductType._89750A:
                        break;

                    default:
                        break;
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：更新传感器ADC最大值参数
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        private void _UpdateSensorADCValueMax()
        {
            if (IsSerialPort) //当前为串口
            {
                switch (eSensor_ProductType)
                {
                    case Enum.SensorProductType._89713FC:
                        if (cDeviceParameter.SensorADCValueMax.Length > tools.Count) //传感器数量超过工具数量，配置文件检测工具数量较少
                        {
                            for (Byte i = 0; i < tools.Count; i++) //取工具数量为循环上限
                            {
                                cDeviceParameter.SensorADCValueMax[i] = tools[i].EjectPixelMax;
                            }
                        }
                        else //传感器数量小于等于工具数量，配置文件检测工具数量较多
                        {
                            for (Byte i = 0; i < cDeviceParameter.SensorADCValueMax.Length; i++) //取传感器数量为循环上限
                            {
                                cDeviceParameter.SensorADCValueMax[i] = tools[i].EjectPixelMax;
                            }
                        }
                        break;

                    case Enum.SensorProductType._89713FA:
                        Int32 iToolIndex = 0;
                        for (Byte i = 0; i < lPerTobaccoNumber.Count; i++) //循环所有排数
                        {
                            if (iToolIndex < tools.Count)//未超出工具索引
                            {
                                cDeviceParameter.SensorADCValueMax[i] = tools[iToolIndex].EjectPixelMax;
                                iToolIndex += lPerTobaccoNumber[i];
                            }
                            else
                            {
                                break;
                            }
                        }
                        break;

                    case Enum.SensorProductType._89750A:
                        break;

                    default:
                        break;
                }
            }
        }
    }
}