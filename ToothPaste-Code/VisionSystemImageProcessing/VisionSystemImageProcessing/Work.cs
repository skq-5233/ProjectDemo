/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：Work.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：WORK页面

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

using TIS.Imaging;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Drawing.Imaging;

using Microsoft.Win32;
using System.Runtime.InteropServices;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;

using System.IO.Ports;
using DataComTransfer;

namespace VisionSystemImageProcessing
{
    public partial class Work : Form
    {
        private Object[] LockImageProcessing;    　  //防止图像处理冲突

        private Object LockShiftFile = new object();    　　　　　  //防止班次文件读写冲突

        private Object[] LockSerialPort;                             　　　　　//防止串口发送数据冲突
        private SerialPort[] SerialPortCommucation;                            //动态创建串口

        private Object[] LockStaticRejectImageSavePort;    　　　  //防止统计缺陷图像数据读写冲突
        private Object LockCheck_CameraPort;    　　　                   //防止相机信息读写冲突

        private Object LockControllerSerialPort;                   　　　　　  //防止控制器串口发送数据冲突
        private SerialPort ControllerSerialPortCommucation;                    //动态创建控制器串口

        private SerialPort USBPortCommucation;                                 //动态创建串口，用于USB测试

        private Int32 USBPortTestResult;                                       //USB口测试结果
        private Byte[] CommandUSB;                                             //串口接受数据，用于USB测试

        private Int32[] QualityCheckToolIndex;                                 //质量检测界面工具索引
        private Int32[] TolerancesSettingsToolIndex;                           //质公差设置界面工具索引

        private Boolean LabModel;                                              //实验室模式，true:是，false:否

        private BitmapData[][] BmpDataPort;                                    //存储端口相机采集到的图像数据
        private Image<Bgr, Byte>[][] ImageSoursePort;                          //原始图像
        
        private Int32[] RejectImageNumberTotalPort;                            //当前剔除存储数量
        
        private ImageBuffer[][] RejectImageBufferPort;                         //缺陷图像缓冲区

        private Int32[] StaticRejectImageSaveConNumberPort;                    //统计剔除图像连续数量
        private Int32 StaticRejectImageSaveConNumberMax;                       //统计剔除图像连续数量最大值

        private List<Image<Bgr, Byte>>[] StaticRejectImages;//保存剔除图像缓冲区
        private List<VisionSystemClassLibrary.Struct.ImageInformation[]>[] StaticRejectsGraphicsInformations;//属性，图像信息

        private Thread[] StaticRejectImageSaveThread;                          //统计缺陷图像保存线程
        private Boolean[] StaticRejectImageSaveStatePort;                      //统计缺陷图像保存状态标记

        private Object[] LockSourseImageBuffPort;    　　　　　　　　          //防止原始图像保存缓冲区读写冲突
        private Byte[] SourseImageBuffIndexPort;                               //原始图像保存缓冲区索引
        private Boolean[] SourseImageBuff0FlagPort;                            //原始图像保存缓冲区1可操作标记
        private Boolean[] SourseImageBuff1FlagPort;                            //原始图像保存缓冲区2可操作标记
        private Image<Bgr, Byte>[][] SourseImageBuffPort;                      //原始图像保存缓冲区
        private VisionSystemClassLibrary.Struct.ImageInformation[][][] SourseImageBuffGraphicsInformationPort;//公差界面原始图像信息缓存
        private VisionSystemClassLibrary.Class.Tools[][][] SourseImageBuffToolInformationPort; //公差界面原始图像临时工具缓存

        private Object[] LockRejectImageBuffPort;    　　　　　　　　          //防止剔除图像保存缓冲区读写冲突
        private Byte[] RejectImageBuffIndexPort;                               //剔除图像保存缓冲区索引
        private Boolean[] RejectImageBuff0FlagPort;                            //剔除图像保存缓冲区1可操作标记
        private Boolean[] RejectImageBuff1FlagPort;                            //剔除图像保存缓冲区2可操作标记
        private Image<Bgr, Byte>[][] RejectImageBuffPort;                      //剔除图像保存缓冲区
        private VisionSystemClassLibrary.Struct.ImageInformation[][][] RejectImageBuffGraphicsInformationPort;//公差界面剔除图像信息缓存
        private VisionSystemClassLibrary.Class.Tools[][][] RejectImageBuffToolInformationPort; //公差界面剔除图像临时工具缓存
        
        private Byte CommunicationErrCount;              　　　　　　　　　　  //串口通信错误计数

        private Boolean DiagEnableChanged;                                     //诊断状态发生变化

        private Byte MachineType;                                              //机器类型

        private UInt16[] PowerStatePort;                                       //相机上电状态 0为刚下电 1为刚上电 
        private Boolean[] CameraPowerOnFlagPort;                               //相机上电标记
        private Boolean[] CameraPowerOffFlagPort;                              //相机下电标记
        
        private ICImagingControl[] icImagingControlPort;                       //动态创建端口1相机控件

        private ICImagingControl icImagingControlBuff = new ICImagingControl(); //动态创建相机控件缓存

        private VCDPropertyItem[] ExposureItemPort;                            //曝光
        private VCDSwitchProperty[] ExposureAutoPort;                          //曝光自动选项
        private VCDAbsoluteValueProperty[] ExposureValuePort;                  //曝光手动设定值
        private VCDPropertyItem[] WhiteBalanceItemPort;                        //白平衡
        private VCDSwitchProperty[] WhiteBalanceItemAutoPort;                  //白平衡自动选项
        private VCDRangeProperty[] WhiteBalanceRedPort;                        //白平衡（红）
        private VCDRangeProperty[] WhiteBalanceGreenPort;                      //白平衡（绿）
        private VCDRangeProperty[] WhiteBalanceBluePort;                       //白平衡（蓝）
        private VCDPropertyItem[] ColorEnhancementItemPort;                    //色彩增强
        private VCDSwitchProperty[] ColorEnhancementItemEnablePort;            //色彩增强开启
        private VCDPropertyItem[] GainItemPort;                                //增益
        private VCDSwitchProperty[] GainAutoPort;                              //增益自动
        private VCDRangeProperty[] GainValuePort;                              //曝光手动设定

        private Byte CommunicationCount_BrandManagement_LoadBrand;             //加载品牌界面以太网通讯命令计数

        private Boolean ShutDown_DevicesSetup_ConfigDevice;                    //配置设备界面执行计算机重启标记
        private Byte CommunicationCount_DevicesSetup_ConfigDevice;             //配置设备界面以太网通讯命令计数

        private Byte CommunicationCount_ClientSystem_Update;                   //系统参数升级以太网通讯命令计数

        private Boolean ApplicationResart_CommunicationCount_SystemParameter;  //系统参数配置界面执行应用程序重启标记
        private Byte CommunicationCount_SystemParameter;                       //系统参数配置界面以太网通讯命令计数

        private Byte CommunicationCount_DeviceState_Synchronization;           //设备状态同步界面以太网通讯命令计数

        private Boolean CloseSerialPortFlag;                                   //串口关闭标志
        private CommunicationInstructionType CloseSerialPort_ComType;          //关闭串口时执行命令

        private static Load LoadFrm = null;                                    //初始化界面
        public static Work pWork = null;                                       //FrmWork类型的静态变量，通过该变量可以操作本窗口

        [DllImport("Kernel32.dll")]
        private extern static Int32 SetLocalTime(ref VisionSystemClassLibrary.Struct.SYSTEMTIME lpSystemTime);  //设置当前系统时间
        [DllImport("Kernel32.dll")]
        private extern static Boolean SetComputerName(String lpComputerName = "CETC41S");  //设置计算机名
        [DllImport("Kernel32.dll")]
        private extern static Boolean SetComputerNameEx(VisionSystemClassLibrary.Enum._COMPUTER_NAME_FORMAT iType, String lpComputerName = "CETC41S");  //设置计算机名
        [DllImport("Kernel32.dll", EntryPoint = "SetProcessWorkingSetSize")]
        private extern static Int32 SetProcessingWorkingSetSize(IntPtr process, Int32 minSize, Int32 maxSize);
        
        private Image<Bgr, Byte>[] workImage;//工作-配置界面以太网通讯图像缓存
        private Image<Bgr, Byte>[] ImageSourseQualityCheck;//质量检测界面原始图像缓存

        private Int32 DeletingStaticsResult;//是否正在删除成功
        private Thread threadDeleteStatics;  //删除统计数据线程
        private Thread threadStatistics_DeleteRecord;  //删除统计数据线程

        private System.Windows.Forms.Timer[] tCameraRestartCount; //相机上电恢复，自触发后重新初始化进行外部触发
        private Boolean[] bCameraRestartExtraTrigger;//相机定时器一开始外部初始化

        //传感器操作添加参数
        private Boolean[] bSerialPortLabState;//进入实验室界面标记

        private Byte[] SensorAdjusting;//1：传感器在校准过程中；0、传感器校准结束
        private Int32[] SensorAdjustResult;//传感器校准结果 

        private Int32[] SensorAdjustState;//传感器校准状态

        private Byte[][][] SensorADCValue;//传感器ADC值
        private Byte[] SensorADCChecking;//1：传感器最大值查询过程中；0、传感器最大值查询结束

        private Int16[][][] SensorADCValueIndex;//传感器ADC值索引

        //删除统计信息

        private struct DeleteRecord_ThreadParameter
        {
            public Int32 index;
            public Int32 Type;
            public Int32 shiftIndex;
            public VisionSystemClassLibrary.Struct.SYSTEMTIME start;
            public VisionSystemClassLibrary.Struct.SYSTEMTIME end;
        }

        //关联图像信息
        private Object LockRelevancyImageSave; //防止关联图像数据读写冲突
        private Thread RelevancyImageSaveThread; //关联图像保存线程
        private Boolean RelevancyImageSaveState; //关联图像保存状态标记

        private List<Image<Bgr, Byte>>[] RelevancyImages;//关联图像
        private List<VisionSystemClassLibrary.Struct.ImageInformation[]>[] RelevancyImageInformations;//关联图像信息

        private List<Image<Bgr, Byte>>[] Images_totalSave;//全保存图像缓冲区
        private List<DateTime>[] Images_totalSave_DateTime;//属性，图像时间
        private List<Dictionary<string,string>>[] Images_totalSave_Information;//属性，图像信息

        private Thread[] Images_totalSaveThread; //全保存图像保存线程

        //默认传输全黑图像
        private Image<Bgr, Byte> ImageInit = new Image<Bgr, Byte>(744, 480);

        //质量检测界面测速
        private Stopwatch Stopwatch_QualityCheck = new Stopwatch();

        //上电程序启动时间
        private String ApplicationStartTimeStr = "";
        
        private Boolean[] CameraLostState;//相机掉线状态(true为掉线)
        private Byte[] CameraImageCount;//上位机接收到的图像计数
        private Byte[] CameraLostNumber;//相机掉电计数器（5次即报系统故障）
        private Byte CheckDelayCount; //命令12查询计数

        Int32[] ImageCount;
        Int32[] ImageBadCount;
        Int32[] PushImageCount;
        Int32[] PopImageCount;
        Int32[] LostCount;
        Int32[] serialPortError1;

        Stopwatch CAM1_Stopwatch = new Stopwatch();
        Stopwatch CAM2_Stopwatch = new Stopwatch();
        Stopwatch CAM3_Stopwatch = new Stopwatch();
        Stopwatch CAM4_Stopwatch = new Stopwatch();

        double CAM1_TIME = 0, CAM2_TIME = 0, CAM3_TIME = 0, CAM4_TIME = 0;
        double CAM1_TIME_75 = 0, CAM2_TIME_75 = 0, CAM3_TIME_75 = 0, CAM4_TIME_75 = 0;
        Int32 CAM1_Count = 0, CAM2_Count = 0, CAM3_Count = 0, CAM4_Count = 0;
        Byte CAM_TIME_MAX = 75;

        /// <summary>
        /// GPIO设置
        /// </summary>
        /// 
        UIntPtr SemaHandle;

        //[DllImport("semaeapi.dll", EntryPoint = "SemaEApiLibInitialize", CallingConvention = CallingConvention.Cdecl)]
        //public static extern UInt32 SemaEApiLibInitialize(bool sll, IP_Version ipv, string addr, UInt32 port, string pas, out UIntPtr handler);

        //[DllImport("semaeapi.dll", EntryPoint = "SemaEApiGPIOSetDirection", CallingConvention = CallingConvention.Cdecl)]
        //public static extern UInt32 SemaEApiGPIOSetDirection(UInt32 handler, UInt32 id, UInt32 bitMask, UInt32 direction);

        //[DllImport("semaeapi.dll", EntryPoint = "SemaEApiGPIOGetLevel", CallingConvention = CallingConvention.Cdecl)]
        //public static extern UInt32 SemaEApiGPIOGetLevel(UInt32 handler, UInt32 id, UInt32 bitMask, out UIntPtr level);

        //public enum IP_Version
        //{
        //    IP_V4 = 0,
        //    IP_V6 = 1,
        //};

        public Work()
        {
            InitializeComponent();

            pWork = this;

            serialPortError1 = new Int32[Global.CameraNumberMax];

            LockControllerSerialPort = new object();

            USBPortTestResult = 0;

            CommandUSB = new Byte[1];

            LabModel = false;

            CommunicationErrCount = 0;

            DiagEnableChanged = false;

            MachineType = 1;

            ExposureItemPort = new VCDPropertyItem[Global.CameraNumberMax];
            ExposureAutoPort = new VCDSwitchProperty[Global.CameraNumberMax];
            ExposureValuePort = new VCDAbsoluteValueProperty[Global.CameraNumberMax];
            WhiteBalanceItemPort = new VCDPropertyItem[Global.CameraNumberMax];
            WhiteBalanceItemAutoPort = new VCDSwitchProperty[Global.CameraNumberMax];
            WhiteBalanceRedPort = new VCDRangeProperty[Global.CameraNumberMax];
            WhiteBalanceGreenPort = new VCDRangeProperty[Global.CameraNumberMax];
            WhiteBalanceBluePort = new VCDRangeProperty[Global.CameraNumberMax];
            ColorEnhancementItemPort = new VCDPropertyItem[Global.CameraNumberMax];
            ColorEnhancementItemEnablePort = new VCDSwitchProperty[Global.CameraNumberMax];
            GainItemPort = new VCDPropertyItem[Global.CameraNumberMax];
            GainAutoPort = new VCDSwitchProperty[Global.CameraNumberMax];
            GainValuePort = new VCDRangeProperty[Global.CameraNumberMax];

            icImagingControlPort = new ICImagingControl[Global.CameraNumberMax];

            CommunicationCount_BrandManagement_LoadBrand = 0;

            ShutDown_DevicesSetup_ConfigDevice = false;
            CommunicationCount_DevicesSetup_ConfigDevice = 0;

            CommunicationCount_ClientSystem_Update = 0;

            ApplicationResart_CommunicationCount_SystemParameter = false;
            CommunicationCount_SystemParameter = 0;

            CommunicationCount_DeviceState_Synchronization = 0;

            CloseSerialPortFlag = false;

            LockSerialPort = new Object[Global.CameraNumberMax];//防止串口发送数据冲突
            SerialPortCommucation = new SerialPort[Global.CameraNumberMax];//动态创建串口
            
            DeletingStaticsResult = 1;//默认删除成功

            LockRelevancyImageSave =new Object(); //防止关联图像数据读写冲突
            RelevancyImageSaveState = true;

            LockCheck_CameraPort = new Object();
        }

        //-----------------------------------------------------------------------
        // 功能说明： 窗口初始化加载函数
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void Work_Load(object sender, EventArgs e)
        {
            LoadFrm = new Load();
            LoadFrm.ShowDialog();
            LoadFrm.Dispose();

            LockImageProcessing = new Object[Global.CameraNumberMax];
            LockStaticRejectImageSavePort = new Object[Global.CameraNumberMax];

            QualityCheckToolIndex = new Int32[Global.CameraNumberMax];
            TolerancesSettingsToolIndex = new Int32[Global.CameraNumberMax];

            BmpDataPort = new BitmapData[Global.CameraNumberMax][];
            ImageSoursePort = new Image<Bgr, Byte>[Global.CameraNumberMax][];
            
            RejectImageNumberTotalPort = new Int32[Global.CameraNumberMax];

            LockSourseImageBuffPort = new Object[Global.CameraNumberMax];

            SourseImageBuffIndexPort = new Byte[Global.CameraNumberMax];
            SourseImageBuff0FlagPort = new Boolean[Global.CameraNumberMax];
            SourseImageBuff1FlagPort = new Boolean[Global.CameraNumberMax];

            LockRejectImageBuffPort = new Object[Global.CameraNumberMax];
            RejectImageBuffIndexPort = new Byte[Global.CameraNumberMax];
            RejectImageBuff0FlagPort = new Boolean[Global.CameraNumberMax];
            RejectImageBuff1FlagPort = new Boolean[Global.CameraNumberMax];

            SourseImageBuffPort = new Image<Bgr, Byte>[Global.CameraNumberMax][];

            SourseImageBuffGraphicsInformationPort = new VisionSystemClassLibrary.Struct.ImageInformation[Global.CameraNumberMax][][];
            SourseImageBuffToolInformationPort = new VisionSystemClassLibrary.Class.Tools[Global.CameraNumberMax][][];

            RejectImageBuffPort = new Image<Bgr, Byte>[Global.CameraNumberMax][];
            
            RejectImageBuffGraphicsInformationPort = new VisionSystemClassLibrary.Struct.ImageInformation[Global.CameraNumberMax][][];
            RejectImageBuffToolInformationPort = new VisionSystemClassLibrary.Class.Tools[Global.CameraNumberMax][][];

            PowerStatePort = new UInt16[Global.CameraNumberMax];
            CameraPowerOnFlagPort = new Boolean[Global.CameraNumberMax];
            CameraPowerOffFlagPort = new Boolean[Global.CameraNumberMax];
            
            workImage = new Image<Bgr, Byte>[Global.CameraNumberMax];
            ImageSourseQualityCheck = new Image<Bgr, Byte>[Global.CameraNumberMax];

            bSerialPortLabState = new Boolean[Global.CameraNumberMax];

            SensorAdjusting = new Byte[Global.CameraNumberMax];
            SensorAdjustResult = new Int32[Global.CameraNumberMax];

            SensorAdjustState = new Int32[Global.CameraNumberMax];

            SensorADCValue = new Byte[Global.CameraNumberMax][][];
            SensorADCChecking = new Byte[Global.CameraNumberMax];
            SensorADCValueIndex = new Int16[Global.CameraNumberMax][][];

            ImageCount = new Int32[Global.CameraNumberMax];
            ImageBadCount = new Int32[Global.CameraNumberMax];
            PushImageCount = new Int32[Global.CameraNumberMax];
            PushImageCount = new Int32[Global.CameraNumberMax];
            PopImageCount = new Int32[Global.CameraNumberMax];
            LostCount = new Int32[Global.CameraNumberMax];

            CameraLostState = new Boolean[Global.CameraNumberMax];
            CameraImageCount = new Byte[Global.CameraNumberMax];
            CameraLostNumber = new Byte[Global.CameraNumberMax];

            for (Byte i = 0; i < Global.CameraNumberMax; i++)//遍历当前所有相机
            {
                LockImageProcessing[i] = new Object();
                LockStaticRejectImageSavePort[i] = new Object(); //相机配置时，可能调用双相机，必须全部初始化

                if ((Global.CameraChooseState & (0x01 << i)) != 0)//当前相机开启
                {
                    ImageCount[i] = 0;
                    ImageBadCount[i] = 0;
                    PushImageCount[i] = 0;
                    PopImageCount[i] = 0;
                    LostCount[i] = 0;

                    QualityCheckToolIndex[i] = 0;
                    TolerancesSettingsToolIndex[i] = 0;

                    bSerialPortLabState[i] = false;

                    SensorAdjusting[i] = 0;
                    SensorAdjustResult[i] = 0;

                    SensorAdjustState[i] = 0;
                    SensorADCChecking[i] = 255;

                    if (false == Global.Camera[i].IsSerialPort) //当前为相机
                    {
                        BmpDataPort[i] = new BitmapData[Global.ImageRingBufferSizeMax];
                    }
                    else
                    {
                        SensorADCValue[i] = new Byte[Global.Camera[i].SensorNumber][];
                        SensorADCValueIndex[i] = new Int16[Global.Camera[i].SensorNumber][];

                        Int32 matrixLength = 1;
                        if (Global.Camera[i].ContinuousSampling) //连续采样
                        {
                            if ((null != Global.Camera[i].DeviceParameter.Parameter) && (Global.Camera[i].DeviceParameter.Parameter.Count > 3)) //连续采样数据有效
                            {
                                matrixLength = Global.Camera[i].EncoderPer * ((Global.Camera[i].DeviceParameter.Parameter[3] + 360 - Global.Camera[i].DeviceParameter.Parameter[2] % 360)) + 1;
                            }
                        }

                        for (Byte j = 0; j < Global.Camera[i].SensorNumber; j++)//初始化实时图像、剔除图像信息缓冲区
                        {
                            SensorADCValue[i][j] = new Byte[matrixLength];
                            SensorADCValueIndex[i][j] = new Int16[1];
                            SensorADCValueIndex[i][j][0] = 0;

                            for (Int32 k = 0; k < SensorADCValue[i][j].Length; k++)
                            {
                                SensorADCValue[i][j][k] = 0;
                            }
                        }
                    }

                    RejectImageNumberTotalPort[i] = 0;

                    LockSourseImageBuffPort[i] = new Object();

                    SourseImageBuffIndexPort[i] = 0;
                    SourseImageBuff0FlagPort[i] = true;
                    SourseImageBuff1FlagPort[i] = false;

                    LockRejectImageBuffPort[i] = new Object();
                    RejectImageBuffIndexPort[i] = 0;
                    RejectImageBuff0FlagPort[i] = true;
                    RejectImageBuff1FlagPort[i] = false;

                    SourseImageBuffPort[i] = new Image<Bgr, Byte>[2];
                    
                    SourseImageBuffGraphicsInformationPort[i] = new VisionSystemClassLibrary.Struct.ImageInformation[2][];
                    SourseImageBuffToolInformationPort[i] = new VisionSystemClassLibrary.Class.Tools[2][];

                    RejectImageBuffPort[i] = new Image<Bgr, Byte>[2];
                    
                    RejectImageBuffGraphicsInformationPort[i] = new VisionSystemClassLibrary.Struct.ImageInformation[2][];
                    RejectImageBuffToolInformationPort[i] = new VisionSystemClassLibrary.Class.Tools[2][];

                    PowerStatePort[i] = 0;
                    CameraPowerOnFlagPort[i] = false;
                    CameraPowerOffFlagPort[i] = false;

                    for (Byte j = 0; j < 2; j++)//初始化实时图像、剔除图像信息缓冲区
                    {
                        SourseImageBuffGraphicsInformationPort[i][j] = new VisionSystemClassLibrary.Struct.ImageInformation[Global.Camera[i].Tools.Count];
                        RejectImageBuffGraphicsInformationPort[i][j] = new VisionSystemClassLibrary.Struct.ImageInformation[Global.Camera[i].Tools.Count];
                        
                        SourseImageBuffToolInformationPort[i][j] = new VisionSystemClassLibrary.Class.Tools[Global.Camera[i].Tools.Count];
                        RejectImageBuffToolInformationPort[i][j] = new VisionSystemClassLibrary.Class.Tools[Global.Camera[i].Tools.Count];

                        for (Int32 k = 0; k < Global.Camera[i].Tools.Count; k++) //循环所有工具
                        {
                            SourseImageBuffGraphicsInformationPort[i][j][k] = new VisionSystemClassLibrary.Struct.ImageInformation();
                            RejectImageBuffGraphicsInformationPort[i][j][k] = new VisionSystemClassLibrary.Struct.ImageInformation();

                            SourseImageBuffToolInformationPort[i][j][k] = new VisionSystemClassLibrary.Class.Tools();
                            RejectImageBuffToolInformationPort[i][j][k] = new VisionSystemClassLibrary.Class.Tools();
                        }
                    }

                    workImage[i] = new Image<Bgr, Byte>(Global.Camera[i].ImageWidth, Global.Camera[i].ImageHeight);//工作界面以太网通讯图像缓存
                    ImageSourseQualityCheck[i] = new Image<Bgr, Byte>(Global.Camera[i].ImageWidth, Global.Camera[i].ImageHeight);//工作界面以太网通讯图像缓存
                }
            }

            RejectImageBufferPort = new ImageBuffer[Global.CameraNumberMax][];

            RelevancyImages = new List<Image<Bgr, Byte>>[Global.CameraNumberMax];
            RelevancyImageInformations = new List<VisionSystemClassLibrary.Struct.ImageInformation[]>[Global.CameraNumberMax];

            //----------------------------------------------------------------------------------------------------------------
            for (Byte i = 0; i < Global.CameraNumberMax; i++)//初始化相机类
            {
                if ((Global.CameraChooseState & (0x01 << i)) != 0)//当前相机开启
                {
                    RejectImageBufferPort[i] = new ImageBuffer[Global.ImageRingBufferSizeMax];
                    ImageSoursePort[i] = new Image<Bgr, Byte>[Global.ImageSourceBufferSizeMax];

                    RelevancyImages[i] = new List<Image<Bgr, Byte>>();
                    RelevancyImageInformations[i] = new List<VisionSystemClassLibrary.Struct.ImageInformation[]>();

                    for (int j = 0; j < Global.ImageSourceBufferSizeMax; j++)//初始化缺陷图像缓冲
                    {
                        ImageSoursePort[i][j] = new Image<Bgr, Byte>(Global.Camera[i].ImageWidth, Global.Camera[i].ImageHeight);
                    }
                }
            }
            //------------------------------------------------------------------------------------此段代码每个相机产生50M缓存

            //相机设备信息初始化
            Global.CameraDevice.CameraType = new VisionSystemClassLibrary.Enum.CameraType[Global.CameraNumberMax];
            Global.CameraDevice.DeviceName = new System.String[Global.CameraNumberMax];
            Global.CameraDevice.SerialNumber = new System.String[Global.CameraNumberMax];

            for (Byte i = 0; i < Global.CameraNumberMax; i++)//初始化相机类
            {
                if ((Global.CameraChooseState & (0x01 << i)) != 0)//当前相机开启
                {
                    Global.CameraDevice.CameraType[i] = Global.Camera[i].Type;//相机类型
                    Global.CameraDevice.DeviceName[i] = Global.Camera[i].CameraENGName;//相机名称

                    if (false == Global.Camera[i].IsSerialPort) //当前为相机
                    {
                        if (null != Global.SnPort[i]) //相机序列号不为空
                        {
                            Global.CameraDevice.SerialNumber[i] = Global.SnPort[i];//序列号
                        }
                        else
                        {
                            Global.CameraDevice.SerialNumber[i] = "null";//序列号
                        }
                    }
                    else //当前为串口
                    {
                        if (null != Global.SerialPortSn[i]) //串口序列号不为空
                        {
                            Global.CameraDevice.SerialNumber[i] = Global.SerialPortSn[i];//序列号
                        }
                        else
                        {
                            Global.CameraDevice.SerialNumber[i] = "null";//序列号
                        }
                    }
                }
            }

            //班次类初始化
            Byte[] byteIndex = new Byte[Global.CameraNumberMax];
            Int32[] toolNumber = new Int32[Global.CameraNumberMax];

            for (Byte i = 0; i < Global.CameraNumberMax; i++)//遍历当前所有相机
            {
                if ((Global.CameraChooseState & (0x01 << i)) != 0)//当前相机开启
                {
                    byteIndex[i] = Global.Camera[i].DeviceInformation.Port;
                    toolNumber[i] = Global.Camera[i].Tools.Count;
                }
            }

            lock (LockShiftFile)
            {
                Global.ShiftInformation = new VisionSystemClassLibrary.Class.Shift(".\\", byteIndex, toolNumber, Global.CameraNumberMax, Global.CameraChooseState);
            }

            if (Global.ShiftInformation.ShiftState)//班次状态使能
            {
                //班次统计信息统计初始化
                Global.ShiftInformation.ShiftChange += new EventHandler(_ShiftChange);

                StaticRejectImageSaveConNumberPort = new Int32[Global.CameraNumberMax];

                for (Byte i = 0; i < Global.CameraNumberMax; i++) //遍历当前所有相机
                {
                    if ((Global.CameraChooseState & (0x01 << i)) != 0)//当前相机开启
                    {
                        StaticRejectImageSaveConNumberPort[i] = 0;
                    }
                }
                StaticRejectImageSaveConNumberMax = 3;

                StaticRejectImages = new List<Image<Bgr, Byte>>[Global.CameraNumberMax];
                StaticRejectsGraphicsInformations = new List<VisionSystemClassLibrary.Struct.ImageInformation[]>[Global.CameraNumberMax];

                for (Byte i = 0; i < Global.CameraNumberMax; i++)//初始化相机类
                {
                    if ((Global.CameraChooseState & (0x01 << i)) != 0)//当前相机开启
                    {
                        StaticRejectImages[i] = new List<Image<Bgr, Byte>>();
                        StaticRejectsGraphicsInformations[i] = new List<VisionSystemClassLibrary.Struct.ImageInformation[]>();
                    }
                }

                StaticRejectImageSaveThread = new Thread[Global.CameraNumberMax];
                StaticRejectImageSaveStatePort = new Boolean[Global.CameraNumberMax];
                for (Byte i = 0; i < Global.CameraNumberMax; i++)//初始化相机类
                {
                    if ((Global.CameraChooseState & (0x01 << i)) != 0)//当前相机开启
                    {
                        StaticRejectImageSaveStatePort[i] = true;
                    }
                }
            }

            //全保存标记
            Global.ShiftInformation_totalSave = new VisionSystemClassLibrary.Class.Shift_totalSave(".\\");

            if (Global.ShiftInformation_totalSave.ShiftState) //全保存已开启
            {
                Images_totalSave = new List<Image<Bgr, Byte>>[Global.CameraNumberMax];//全保存图像缓冲区
                Images_totalSave_DateTime = new List<DateTime>[Global.CameraNumberMax];//属性，图像时间
                Images_totalSave_Information = new List<Dictionary<string, string>>[Global.CameraNumberMax];//属性，图像时间
                Images_totalSaveThread = new Thread[Global.CameraNumberMax]; //全保存图像保存线程

                //全存储客户端初始化
                //----------------------------------------------------------------------------------------------------------------
                for (Byte i = 0; i < Global.CameraNumberMax; i++)//初始化相机类
                {
                    if ((Global.CameraChooseState & (0x01 << i)) != 0)//当前相机开启
                    {
                        Images_totalSave[i] = new List<Image<Bgr, Byte>>();//全保存图像缓冲区

                        if (Global.ShiftInformation_totalSave.TransMode) //TCP/IP
                        {
                            Images_totalSave_Information[i] = new List<Dictionary<string, string>>();//属性，图像时间
                        }
                        else
                        {
                            Images_totalSave_DateTime[i] = new List<DateTime>();//属性，图像时间
                        }

                        if (Global.ShiftInformation_totalSave.TransMode) //TCP/IP传输模式
                        {
                            ApplicationStartTimeStr = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_";

                            switch (i)
                            {
                                case 0:
                                    tcpClient1.IP = Global.ShiftInformation_totalSave.ServerIP;
                                    tcpClient1.Start();
                                    break;
                                case 1:
                                    tcpClient2.IP = Global.ShiftInformation_totalSave.ServerIP;
                                    tcpClient2.Start();
                                    break;
                                case 2:
                                    tcpClient3.IP = Global.ShiftInformation_totalSave.ServerIP;
                                    tcpClient3.Start();
                                    break;
                                case 3:
                                    tcpClient4.IP = Global.ShiftInformation_totalSave.ServerIP;
                                    tcpClient4.Start();
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }

            //以太网通信服务端事件订阅
            Global.NetClient = new VisionSystemCommunicationLibrary.Ethernet.ClientControl();
            Global.NetClient.DataReceived += new System.EventHandler(_NetClient_DataReceived);//以太网通信，接收到一帧完整的数据时的事件
            Global.NetClient.ExceptionHandled += new EventHandler(_NetClient_ExceptionHandled);//以太网通信，异常时的事件

            //以太网客户端数据初始化

            Global.NetClient.ClientData = Global.ClientData;

            //连接以太网服务端
            Thread ConnectedThread = new Thread(_NetClient_Connect);
            ConnectedThread.IsBackground = true;
            ConnectedThread.Start();

            ////GPIO初始化
            //SemaEApiLibInitialize(false, IP_Version.IP_V4, "127.0.0.1", 9999, "123", out SemaHandle);
            //SemaEApiGPIOSetDirection((UInt32)SemaHandle, 1, 1, 1);
            //timer_GPIO.Enabled = true;

            tCameraRestartCount = new System.Windows.Forms.Timer[Global.CameraNumberMax];
            bCameraRestartExtraTrigger = new Boolean[Global.CameraNumberMax];
            for (Byte i = 0; i < Global.CameraNumberMax; i++)//初始化相机类
            {
                if ((Global.CameraChooseState & (0x01 << i)) != 0)//当前相机开启
                {
                    bCameraRestartExtraTrigger[i] = false;

                    tCameraRestartCount[i] = new System.Windows.Forms.Timer();
                    tCameraRestartCount[i].Enabled = false;
                    tCameraRestartCount[i].Interval = 1000;

                    UInt64 temp = 0;

                    switch (i)
                    {
                        case 0:
                            tCameraRestartCount[i].Tick += new EventHandler(Work_Tick1);
                            temp = 0x02;
                            break;
                        case 1:
                            tCameraRestartCount[i].Tick += new EventHandler(Work_Tick2);
                            temp = 0x04;
                            break;
                        case 2:
                            tCameraRestartCount[i].Tick += new EventHandler(Work_Tick3);
                            temp = 0x010000000000;
                            break;
                        case 3:
                            tCameraRestartCount[i].Tick += new EventHandler(Work_Tick4);
                            temp = 0x020000000000;
                            break;
                        default:
                            break;
                    }

                    if ((Global.MachineFaultState & temp) != 0)//相机初始未查询到，故障信息保存，不再查询相机
                    {
                        switch (i)
                        {
                            case 0:
                                if (Global.Camera[i].IsSerialPort) //当前为串口
                                {
                                    label5.Text = "串口1故障.";
                                }
                                else
                                {
                                    label5.Text = "相机1故障.";
                                }
                                break;
                            case 1:
                                if (Global.Camera[i].IsSerialPort) //当前为串口
                                {
                                    label2.Text = "串口2故障.";
                                }
                                else
                                {
                                    label2.Text = "相机2故障.";
                                }
                                break;
                            case 2:
                                if (Global.Camera[i].IsSerialPort) //当前为串口
                                {
                                    label13.Text = "串口3故障.";
                                }
                                else
                                {
                                    label13.Text = "相机3故障.";
                                }
                                break;
                            case 3:
                                if (Global.Camera[i].IsSerialPort) //当前为串口
                                {
                                    label15.Text = "串口4故障.";
                                }
                                else
                                {
                                    label15.Text = "相机4故障.";
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        if (false == Global.Camera[i].IsSerialPort) //当前为相机，进行初始化
                        {
                            _StartCamera(i);                         //启动端口1、2相机
                        }
                    }
                    CameraImageCount[i] = 1;
                    CameraLostState[i] = false;
                }
            }

            //相机重新启动
            timer5.Enabled = true;

            //相机故障查询
            timerPower.Enabled = true;

            //调试信息刷新
            timer2.Enabled = true;
        }

        //-----------------------------------------------------------------------
        // 功能说明： 初始化相机、缺陷指示控件及部分变量
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        public void _InitCameraAndControl1()
        {
            FrameFilter RotateFlipFilter;

            for (Byte i = 0; i < Global.CameraNumberMax; i++)//遍历当前所有相机
            {
                if ((Global.CameraChooseState & (0x01 << i)) != 0)//当前相机开启
                {
                    if (false == Global.Camera[i].IsSerialPort) //当前为相机，进行初始化
                    {
                        icImagingControlPort[i] = new ICImagingControl();
                        icImagingControlPort[i].Parent = this;
                        icImagingControlPort[i].DeviceLostExecutionMode =
                            TIS.Imaging.EventExecutionMode.AsyncInvoke;
                        icImagingControlPort[i].ImageAvailableExecutionMode =
                            TIS.Imaging.EventExecutionMode.MultiThreaded;
                        icImagingControlPort[i].Visible = false;

                        RotateFlipFilter = icImagingControlPort[i].FrameFilterCreate("Rotate Flip", "");

                        if (RotateFlipFilter == null)
                        {
                            if (Global.ShowInformation)//显示调试信息
                            {
                                switch (i)
                                {
                                    case 0:
                                        Global.MachineFaultState |= 0x02;
                                        label5.Invoke(new EventHandler(delegate { label5.Text = "相机1旋转测试失败."; }));
                                        break;
                                    case 1:
                                        Global.MachineFaultState |= 0x04;
                                        label2.Invoke(new EventHandler(delegate { label2.Text = "相机2旋转测试失败."; }));
                                        break;
                                    case 2:
                                        Global.MachineFaultState |= 0x010000000000;
                                        label13.Invoke(new EventHandler(delegate { label13.Text = "相机3旋转测试失败."; }));
                                        break;
                                    case 3:
                                        Global.MachineFaultState |= 0x020000000000;
                                        label15.Invoke(new EventHandler(delegate { label15.Text = "相机4旋转测试失败."; }));
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                        else
                        {
                            icImagingControlPort[i].DeviceFrameFilters.Add(RotateFlipFilter);
                        }

                        if (!icImagingControlPort[i].LiveVideoRunning)
                        {
                            switch (Global.Camera[i].CameraFlip)
                            {
                                case VisionSystemClassLibrary.Enum.CameraFlip.Flip_H:
                                    RotateFlipFilter.SetBoolParameter("Flip H", true);
                                    break;
                                case VisionSystemClassLibrary.Enum.CameraFlip.Flip_V:
                                    RotateFlipFilter.SetBoolParameter("Flip V", true);
                                    break;
                                default:
                                    break;
                            }

                            switch (Global.Camera[i].CameraAngle)
                            {
                                case VisionSystemClassLibrary.Enum.CameraRotateAngle.Angle_0:
                                    RotateFlipFilter.SetIntParameter("Rotation Angle", 0);
                                    break;
                                case VisionSystemClassLibrary.Enum.CameraRotateAngle.Angle_90:
                                    RotateFlipFilter.SetIntParameter("Rotation Angle", 90);
                                    break;
                                case VisionSystemClassLibrary.Enum.CameraRotateAngle.Angle_180:
                                    RotateFlipFilter.SetIntParameter("Rotation Angle", 180);
                                    break;
                                case VisionSystemClassLibrary.Enum.CameraRotateAngle.Angle_270:
                                    RotateFlipFilter.SetIntParameter("Rotation Angle", 270);
                                    break;
                                default:
                                    break;
                            }
                        }
                        
                        // If the rotation value is 90 or 270, then the resulting video format
                        // is changed. Thus, the new value can only be set while the live video is
                        // stopped. Otherwise, an error is returned.

                        if (false == _InitCamera(i, false, Global.Camera[i].VideoFormat, 76, 1)) //端口相机初始化失败
                        {
                            switch (i)
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
                        }
                    }
                }
            }

            Global.MachineFaultStateTemp = Global.MachineFaultState; //标记初始化相机故障状态

            for (Byte i = 0; i < Global.CameraNumberMax; i++)//初始化相机类
            {
                if ((Global.CameraChooseState & (0x01 << i)) != 0)//当前相机开启
                {
                    UInt64 temp = 0;

                    switch (i)
                    {
                        case 0:
                            temp = 0x02;
                            break;
                        case 1:
                            temp = 0x04;
                            break;
                        case 2:
                            temp = 0x010000000000;
                            break;
                        case 3:
                            temp = 0x020000000000;
                            break;
                        default:
                            break;
                    }

                    if ((Global.MachineFaultState & temp) != 0)//相机初始未查询到，故障信息保存，不再查询相机
                    {
                        switch (i)
                        {
                            case 0:
                                if (Global.Camera[i].IsSerialPort) //当前为串口
                                {
                                    label5.Text = "串口1故障.";
                                }
                                else
                                {
                                    label5.Text = "相机1故障.";
                                }
                                break;
                            case 1:
                                if (Global.Camera[i].IsSerialPort) //当前为串口
                                {
                                    label2.Text = "串口2故障.";
                                }
                                else
                                {
                                    label2.Text = "相机2故障.";
                                }
                                break;
                            case 2:
                                if (Global.Camera[i].IsSerialPort) //当前为串口
                                {
                                    label13.Text = "串口3故障.";
                                }
                                else
                                {
                                    label13.Text = "相机3故障.";
                                }
                                break;
                            case 3:
                                if (Global.Camera[i].IsSerialPort) //当前为串口
                                {
                                    label15.Text = "串口4故障.";
                                }
                                else
                                {
                                    label15.Text = "相机4故障.";
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        if (false == Global.Camera[i].IsSerialPort) //当前为相机，进行初始化
                        {
                            _StartCamera(i);                         //启动端口1、2相机
                        }
                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明： 初始化相机、缺陷指示控件及部分变量
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        public void _InitCameraAndControl2()
        {
            for (Byte i = 0; i < Global.CameraNumberMax; i++)//遍历当前所有相机
            {
                if ((Global.CameraChooseState & (0x01 << i)) != 0)//当前相机开启
                {
                    if (false == Global.Camera[i].IsSerialPort) //当前为相机，进行初始化
                    {
                        switch (i)
                        {
                            case 0:
                                icImagingControlPort[i].ImageAvailable += new System.EventHandler<
                            TIS.Imaging.ICImagingControl.ImageAvailableEventArgs>(_icImagingControlPort1_ImageAvailable);//触发端口1相机捕获图像事件
                                icImagingControlPort[i].DeviceLost += new System.EventHandler<
                                    TIS.Imaging.ICImagingControl.DeviceLostEventArgs>(_icImagingControlPort1_DeviceLost);//触发端口1相机不正常工作事件
                                break;
                            case 1:
                                icImagingControlPort[i].ImageAvailable += new System.EventHandler<
                             TIS.Imaging.ICImagingControl.ImageAvailableEventArgs>(_icImagingControlPort2_ImageAvailable);//触发端口2相机捕获图像事件
                                icImagingControlPort[i].DeviceLost += new System.EventHandler<
                                    TIS.Imaging.ICImagingControl.DeviceLostEventArgs>(_icImagingControlPort2_DeviceLost);//触发端口2相机不正常工作事件
                                break;
                            case 2:
                                icImagingControlPort[i].ImageAvailable += new System.EventHandler<
                            TIS.Imaging.ICImagingControl.ImageAvailableEventArgs>(_icImagingControlPort3_ImageAvailable);//触发端口3相机捕获图像事件
                                icImagingControlPort[i].DeviceLost += new System.EventHandler<
                                    TIS.Imaging.ICImagingControl.DeviceLostEventArgs>(_icImagingControlPort3_DeviceLost);//触发端口3相机不正常工作事件
                                break;
                            case 3:
                                icImagingControlPort[i].ImageAvailable += new System.EventHandler<
                            TIS.Imaging.ICImagingControl.ImageAvailableEventArgs>(_icImagingControlPort4_ImageAvailable);//触发端口4相机捕获图像事件
                                icImagingControlPort[i].DeviceLost += new System.EventHandler<
                                    TIS.Imaging.ICImagingControl.DeviceLostEventArgs>(_icImagingControlPort4_DeviceLost);//触发端口4相机不正常工作事件
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            for (Byte i = 0; i < Global.CameraNumberMax; i++)//初始化相机类
            {
                if ((Global.CameraChooseState & (0x01 << i)) != 0)//当前相机开启
                {
                    Boolean bInitCameraResult = false;

                    if (false == Global.Camera[i].IsSerialPort) //当前为相机，进行初始化1
                    {
                        if (false == Global.ComputerRunState) //控制器上运行软件
                        {
                            bInitCameraResult = _InitCamera(i, !Global.ComputerRunState, Global.Camera[i].VideoFormat, Global.Camera[i].DeviceFrameRate, Global.ImageRingBufferSizeMax);
                        }
                        else
                        {
                            bInitCameraResult = _InitCamera(i, !Global.ComputerRunState, Global.Camera[i].VideoFormat, 10, Global.ImageRingBufferSizeMax);
                        }
                    }
                    else
                    {
                        try
                        {
                            LockSerialPort[i] = new object();

                            SerialPortCommucation[i] = new SerialPort();
                            SerialPortCommucation[i].PortName = Global.SerialPortName[i];
                            string sBaudRate = Global.Camera[i].SerialPort_BaudRate.ToString();
                            SerialPortCommucation[i].BaudRate = Convert.ToInt32(sBaudRate.Substring(1, sBaudRate.Length - 1));
                            SerialPortCommucation[i].ReceivedBytesThreshold = Global.Camera[i].SerialPort_ReceivedBytesThreshold;

                            switch (i)
                            {
                                case 0:
                                    SerialPortCommucation[i].DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(_SerialPortCommucation1_DataReceived);
                                    break;
                                case 1:
                                    SerialPortCommucation[i].DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(_SerialPortCommucation2_DataReceived);
                                    break;
                                case 2:
                                    SerialPortCommucation[i].DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(_SerialPortCommucation3_DataReceived);
                                    break;
                                case 3:
                                    SerialPortCommucation[i].DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(_SerialPortCommucation4_DataReceived);
                                    break;
                                default:
                                    break;
                            }

                            SerialPortCommucation[i].Open();

                            lock (LockSerialPort[i])
                            {
                                _SendCommand_SerialPortCommucation(1, i);
                            }

                            bInitCameraResult = true;
                        }
                        catch (System.Exception ex)
                        {
                        }
                    }

                    if (bInitCameraResult) //端口相机初始化成功
                    {
                        Global.CameraDevice.CameraNumber++;//相机数量
                    }
                    else
                    {
                        switch (i)
                        {
                            case 0:
                                if (Global.Camera[i].IsSerialPort) //当前为串口
                                {
                                    label5.Text = "串口1故障.";
                                }
                                else
                                {
                                    label5.Text = "相机1故障.";
                                }
                                break;
                            case 1:
                                if (Global.Camera[i].IsSerialPort) //当前为串口
                                {
                                    label2.Text = "串口2故障.";
                                }
                                else
                                {
                                    label2.Text = "相机2故障.";
                                }
                                break;
                            case 2:
                                if (Global.Camera[i].IsSerialPort) //当前为串口
                                {
                                    label13.Text = "串口3故障.";
                                }
                                else
                                {
                                    label13.Text = "相机3故障.";
                                }
                                break;
                            case 3:
                                if (Global.Camera[i].IsSerialPort) //当前为串口
                                {
                                    label15.Text = "串口4故障.";
                                }
                                else
                                {
                                    label15.Text = "相机4故障.";
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            Global.MachineFaultStateTemp = Global.MachineFaultState; //标记初始化相机故障状态

            if (false == Global.ComputerRunState) //控制器上运行软件
            {
                try
                {
                    ControllerSerialPortCommucation = new SerialPort();
                    ControllerSerialPortCommucation.PortName = Global.ControllerSerialPortName;
                    ControllerSerialPortCommucation.BaudRate = Global.ControllerSerialPortCommucation_BaudRate;
                    ControllerSerialPortCommucation.ReceivedBytesThreshold = Global.ControllerSerialPortCommucation_ReceivedBytesThreshold;
                    ControllerSerialPortCommucation.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(_SerialPortCommucation_DataReceived);
                    ControllerSerialPortCommucation.Open();

                    if (Global.ShowInformation)//显示调试信息
                    {
                        label6.Text = "串口打开正常";
                    }
                }
                catch (System.Exception ex)
                {
                    if (Global.ShowInformation)//显示调试信息
                    {
                        label6.Text = "串口打开异常";
                    }
                }
            }

            CommunicationErrCount = 0;

            if (false == Global.ComputerRunState) //控制器上运行软件
            {
                timer1.Enabled = true;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：发送相机下电命令
        // 输入参数： 1、Byte：index，数组下标索引
        //            2、Boolean：trigger，触发方式：true，外部触发；false，内部触发
        // 输出参数： 无
        // 返 回 值： Boolean，相机初始化结果
        //---------------------------------------------------------------------
        private Boolean _InitCamera(Byte index, Boolean trigger, string videoFormat, float deviceFrameRate, Int32 iImageRingBufferSizeMax)
        {
            try                                                                //启动相机
            {
                icImagingControlPort[index].Device = Global.CameraPort[index];
                if (icImagingControlPort[index].DeviceValid)                         //端口1相机正常工作
                {
                    icImagingControlPort[index].VideoFormat = videoFormat;         //设置相机视频格式
                    icImagingControlPort[index].DeviceFrameRate = deviceFrameRate;                    //设置帧频
                    icImagingControlPort[index].DeviceTrigger = trigger;            //设置触发方式
                    icImagingControlPort[index].LiveCaptureContinuous = true;

                    ExposureItemPort[index] =
                        icImagingControlPort[index].VCDPropertyItems.FindItem(VCDIDs.VCDID_Exposure);
                    ExposureAutoPort[index] = (VCDSwitchProperty)ExposureItemPort[index].Elements.FindInterface(
                        VCDIDs.VCDElement_Auto + ":" + VCDIDs.VCDInterface_Switch);
                    ExposureValuePort[index] = (VCDAbsoluteValueProperty)ExposureItemPort[index].Elements.FindInterface(
                        VCDIDs.VCDElement_Value + ":" + VCDIDs.VCDInterface_AbsoluteValue);
                    ExposureAutoPort[index].Switch = false;                               //设置相机自动曝光

                    if (Global.Camera[index].DeviceParameter.ExposureTime > Global.ExposureTime.Length)
                    {
                        ExposureValuePort[index].Value =
                            Global.ExposureTime[Global.ExposureTime.Length - 1];//设置曝光时间
                    }
                    else if (Global.Camera[index].DeviceParameter.ExposureTime < 1)
                    {
                        ExposureValuePort[index].Value =
                            Global.ExposureTime[0];//设置曝光时间
                    }
                    else
                    {
                        ExposureValuePort[index].Value =
                            Global.ExposureTime[Global.Camera[index].DeviceParameter.ExposureTime - 1];//设置曝光时间
                    }

                    if (videoFormat.StartsWith("RGB32")) //相机配置为彩色模式
                    {
                        WhiteBalanceItemPort[index] =
                            icImagingControlPort[index].VCDPropertyItems.FindItem(VCDIDs.VCDID_WhiteBalance);
                        WhiteBalanceItemAutoPort[index] = (VCDSwitchProperty)WhiteBalanceItemPort[index].Elements.FindInterface(
                            VCDIDs.VCDElement_Auto + ":" + VCDIDs.VCDInterface_Switch);
                        WhiteBalanceRedPort[index] = (VCDRangeProperty)WhiteBalanceItemPort[index].Elements.FindInterface(
                            VCDIDs.VCDElement_WhiteBalanceRed + ":" + VCDIDs.VCDInterface_Range);
                        WhiteBalanceGreenPort[index] = (VCDRangeProperty)WhiteBalanceItemPort[index].Elements.FindInterface(
                        VCDIDs.VCDElement_WhiteBalanceGreen + ":" + VCDIDs.VCDInterface_Range);
                        WhiteBalanceBluePort[index] = (VCDRangeProperty)WhiteBalanceItemPort[index].Elements.FindInterface(
                        VCDIDs.VCDElement_WhiteBalanceBlue + ":" + VCDIDs.VCDInterface_Range);

                        if (Global.Camera[index].DeviceParameter.WhiteBalance == 0)//设置白平衡状态自动
                        {
                            WhiteBalanceRedPort[index].Value = 0;
                            WhiteBalanceGreenPort[index].Value = 0;
                            WhiteBalanceBluePort[index].Value = 0;

                            WhiteBalanceItemAutoPort[index].Switch = true;
                        }
                        else                                                //设置白平衡状态手动
                        {
                            WhiteBalanceRedPort[index].Value = Global.Camera[index].DeviceParameter.WhiteBalance_Red;
                            WhiteBalanceGreenPort[index].Value = Global.Camera[index].DeviceParameter.WhiteBalance_Green;
                            WhiteBalanceBluePort[index].Value = Global.Camera[index].DeviceParameter.WhiteBalance_Blue;

                            WhiteBalanceItemAutoPort[index].Switch = false;
                        }

                        ColorEnhancementItemPort[index] =
                            icImagingControlPort[index].VCDPropertyItems.FindItem(VCDIDs.VCDID_ColorEnhancement);
                        ColorEnhancementItemEnablePort[index] = (VCDSwitchProperty)ColorEnhancementItemPort[index].Elements.FindInterface(
                            VCDIDs.VCDElement_Value + ":" + VCDIDs.VCDInterface_Switch);
                        ColorEnhancementItemEnablePort[index].Switch = true;                  //设置色彩增强
                    }

                    GainItemPort[index] = icImagingControlPort[index].VCDPropertyItems.FindItem(VCDIDs.VCDID_Gain);
                    GainAutoPort[index] = (VCDSwitchProperty)GainItemPort[index].Elements.FindInterface(
                        VCDIDs.VCDElement_Auto + ":" + VCDIDs.VCDInterface_Switch);
                    GainValuePort[index] = (VCDRangeProperty)GainItemPort[index].Elements.FindInterface(
                        VCDIDs.VCDElement_Value + ":" + VCDIDs.VCDInterface_Range);
                    GainAutoPort[index].Switch = false;                                   //设置增益自动
                    GainValuePort[index].Value = Global.Camera[index].DeviceParameter.Gain;

                    icImagingControlPort[index].ImageRingBufferSize = iImageRingBufferSizeMax;

                    return true;
                }

                if (Global.ShowInformation)//显示调试信息
                {
                    switch (index)
                    {
                        case 0:
                            label5.Invoke(new EventHandler(delegate { label5.Text = "相机1初始化异常_I1." + trigger.ToString(); }));
                            break;
                        case 1:
                            label2.Invoke(new EventHandler(delegate { label2.Text = "相机2初始化异常_I1." + trigger.ToString(); }));
                            break;
                        case 2:
                            label13.Invoke(new EventHandler(delegate { label13.Text = "相机3初始化异常_I1." + trigger.ToString(); }));
                            break;
                        case 3:
                            label15.Invoke(new EventHandler(delegate { label15.Text = "相机4初始化异常_I1." + trigger.ToString(); }));
                            break;
                        default:
                            break;
                    }
                }
                return false;
            }
            catch (Exception ex)                                               //启动相机异常
            {
                if (Global.ShowInformation)//显示调试信息
                {
                    switch (index)
                    {
                        case 0:
                            label5.Invoke(new EventHandler(delegate { label5.Text = "相机1初始化异常_I2." + trigger.ToString(); }));
                            break;
                        case 1:
                            label2.Invoke(new EventHandler(delegate { label2.Text = "相机2初始化异常_I2." + trigger.ToString(); }));
                            break;
                        case 2:
                            label13.Invoke(new EventHandler(delegate { label13.Text = "相机3初始化异常_I2." + trigger.ToString(); }));
                            break;
                        case 3:
                            label15.Invoke(new EventHandler(delegate { label15.Text = "相机4初始化异常_I2." + trigger.ToString(); }));
                            break;
                        default:
                            break;
                    }
                }
                return false;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明： 相机启动函数,从设置窗口退出时调用该函数
        // 输入参数： 1、Byte：index，数组下标索引
        // 输出参数： 无
        // 返 回 值： Boolean，相机启动结果
        //-----------------------------------------------------------------------
        public void _StartCamera(Byte index)
        {
            try                                                                //启动相机
            {
                icImagingControlPort[index].LiveStart();
            }
            catch (Exception ex)
            {
                if (Global.ShowInformation)//显示调试信息
                {
                    switch (index)
                    {
                        case 0:
                            label5.Invoke(new EventHandler(delegate { label5.Text = "相机1启动异常."; }));
                            break;
                        case 1:
                            label2.Invoke(new EventHandler(delegate { label2.Text = "相机2启动异常."; }));
                            break;
                        case 2:
                            label13.Invoke(new EventHandler(delegate { label13.Text = "相机3启动异常."; }));
                            break;
                        case 3:
                            label15.Invoke(new EventHandler(delegate { label15.Text = "相机4启动异常."; }));
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明： 相机停止函数,从设置窗口退出时调用该函数
        // 输入参数： 1、Byte：index，数组下标索引
        // 输出参数： 无
        // 返 回 值： 无
        //-----------------------------------------------------------------------
        public void _StopCamera(Byte index)
        {
            if ((icImagingControlPort[index] != null) && (icImagingControlPort[index].DeviceValid))//相机正常工作，则停用
            {
                icImagingControlPort[index].Invoke(new EventHandler(delegate { icImagingControlPort[index].LiveStop(); }));
            }
            timerPower.Enabled = false;
        }

        //-----------------------------------------------------------------------
        // 功能说明： 端口1相机控件接收到新的图像响应事件
        // 输入参数： 1、object：sender，icImagingControl控件对象
        //          2、ICImagingControl.ImageAvailableEventArgs：e，icImagingControl控件事件响应参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _icImagingControlPort1_ImageAvailable(object sender, ICImagingControl.ImageAvailableEventArgs e)
        {
            try
            {
                if (Global.ImageBufferIndex[0] != e.bufferIndex)
                {
                    Global.ImageBufferIndex[0] = e.bufferIndex;

                    while (true)
                    {
                        CAM1_Stopwatch.Restart();

                        //-------------------------------------------------------------------------------------------------------------------------------------
                        BmpDataPort[0][Global.ImageBufferIndex[0]] = icImagingControlPort[0].ImageBuffers[Global.ImageBufferIndex[0]].Bitmap.LockBits(Global.Camera[0].BitmapLockBitsArea, System.Drawing.Imaging.ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                        //-------------------------------------------------------------------------------------此段代码60个产生87M缓存，平均一个1.45M

                        Global.ImageSourceBufferIndex[0] = (Global.ImageSourceBufferIndex[0] + 1) % Global.ImageSourceBufferSizeMax;

                        DateTime dateTime = DateTime.Now;
                        ImageCount[0]++; CameraImageCount[0]++;
                        _BitmapDataProcessing(ImageSoursePort[0][Global.ImageSourceBufferIndex[0]], new Image<Bgr, Byte>(Global.Camera[0].BitmapLockBitsArea.Width, Global.Camera[0].BitmapLockBitsArea.Height, BmpDataPort[0][Global.ImageBufferIndex[0]].Stride, BmpDataPort[0][Global.ImageBufferIndex[0]].Scan0), Global.Camera[0].BitmapLockBitsResize, Global.Camera[0].BitmapLockBitsCenter, Global.Camera[0].BitmapLockBitsAxis);

                        if (Global.ShiftInformation.ShiftState)//班次状态使能
                        {
                            Global.MachineStopState[0]++;//机器停机状态标记变量
                            Global.MachineStopStaticSaveState[0] = true;//机器停机数据可以执行停机保存
                        }

                        Global.Camera[0].DeviceInformation.CAM = VisionSystemClassLibrary.Enum.CameraState.ON;
                        Global.CameraTemp[0].DeviceInformation.CAM = Global.Camera[0].DeviceInformation.CAM;

                        Int32 iImageRingBufferIndex = Global.ImageSourceBufferIndex[0];

                        Boolean bResultPort = true, bCheckTobaccoState = true;
                        Byte bErrorIndexPort = 0;
                        Boolean[] bToolProcessingResultFlag = null;
                        _SourceImageProcessing(0, iImageRingBufferIndex, ref bResultPort, ref bErrorIndexPort, ref bCheckTobaccoState, ref bToolProcessingResultFlag);//图像处理

                        _CameraImageImformationUpdate(0, bResultPort, bErrorIndexPort, iImageRingBufferIndex, bCheckTobaccoState, bToolProcessingResultFlag, dateTime);

                        CAM1_Stopwatch.Stop();
                        CAM1_TIME = CAM1_Stopwatch.Elapsed.TotalMilliseconds;
                        if (CAM1_TIME > CAM_TIME_MAX)
                        {
                            CAM1_TIME_75 = CAM1_TIME;
                            CAM1_Count++;
                        }

                        if (Global.ShowInformation)//显示调试信息
                        {
                            label5.Invoke(new EventHandler(delegate { label5.Text = "相机1工作正常."; }));
                            label1.Invoke(new EventHandler(delegate { label1.Text = CAM1_TIME.ToString() + "/" + CAM1_TIME_75.ToString() + "/" + CAM1_Count.ToString(); }));
                        }

                        if (Global.ImageBufferIndex[0] != icImagingControlPort[0].ImageActiveBuffer.Index)
                        {
                            Global.ImageBufferIndex[0] = (Global.ImageBufferIndex[0] + 1) % icImagingControlPort[0].ImageRingBufferSize;
                        }

                        if (Global.ImageBufferIndex[0] == icImagingControlPort[0].ImageActiveBuffer.Index)
                        {
                            break;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明： 端口2相机控件接收到新的图像响应事件
        // 输入参数： 1、object：sender，icImagingControl控件对象
        //          2、ICImagingControl.ImageAvailableEventArgs：e，icImagingControl控件事件响应参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _icImagingControlPort2_ImageAvailable(object sender, ICImagingControl.ImageAvailableEventArgs e)
        {
            try
            {
                if (Global.ImageBufferIndex[1] != e.bufferIndex)
                {
                    Global.ImageBufferIndex[1] = e.bufferIndex;

                    while (true)
                    {
                        CAM2_Stopwatch.Restart();

                        //-------------------------------------------------------------------------------------------------------------------------------------
                        BmpDataPort[1][Global.ImageBufferIndex[1]] = icImagingControlPort[1].ImageBuffers[Global.ImageBufferIndex[1]].Bitmap.LockBits(Global.Camera[1].BitmapLockBitsArea, System.Drawing.Imaging.ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                        //-------------------------------------------------------------------------------------此段代码60个产生87M缓存，平均一个1.45M

                        Global.ImageSourceBufferIndex[1] = (Global.ImageSourceBufferIndex[1] + 1) % Global.ImageSourceBufferSizeMax;

                        DateTime dateTime = DateTime.Now;
                        ImageCount[1]++; CameraImageCount[1]++;
                        _BitmapDataProcessing(ImageSoursePort[1][Global.ImageSourceBufferIndex[1]], new Image<Bgr, Byte>(Global.Camera[1].BitmapLockBitsArea.Width, Global.Camera[1].BitmapLockBitsArea.Height, BmpDataPort[1][Global.ImageBufferIndex[1]].Stride, BmpDataPort[1][Global.ImageBufferIndex[1]].Scan0), Global.Camera[1].BitmapLockBitsResize, Global.Camera[1].BitmapLockBitsCenter, Global.Camera[1].BitmapLockBitsAxis);

                        if (Global.ShiftInformation.ShiftState)//班次状态使能
                        {
                            Global.MachineStopState[1]++;//机器停机状态标记变量
                            Global.MachineStopStaticSaveState[1] = true;//机器停机数据可以执行停机保存
                        }

                        Global.Camera[1].DeviceInformation.CAM = VisionSystemClassLibrary.Enum.CameraState.ON;
                        Global.CameraTemp[1].DeviceInformation.CAM = Global.Camera[1].DeviceInformation.CAM;

                        Int32 iImageRingBufferIndex = Global.ImageSourceBufferIndex[1];

                        Boolean bResultPort = true, bCheckTobaccoState = true;
                        Byte bErrorIndexPort = 0;
                        Boolean[] bToolProcessingResultFlag = null;
                        _SourceImageProcessing(1, iImageRingBufferIndex, ref bResultPort, ref bErrorIndexPort, ref bCheckTobaccoState,ref bToolProcessingResultFlag);//图像处理

                        _CameraImageImformationUpdate(1, bResultPort, bErrorIndexPort, iImageRingBufferIndex, bCheckTobaccoState, bToolProcessingResultFlag, dateTime);

                        CAM2_Stopwatch.Stop();
                        CAM2_TIME = CAM2_Stopwatch.Elapsed.TotalMilliseconds;
                        if (CAM2_TIME > CAM_TIME_MAX)
                        {
                            CAM2_TIME_75 = CAM2_TIME;
                            CAM2_Count++;
                        }

                        if (Global.ShowInformation)//显示调试信息
                        {
                            label2.Invoke(new EventHandler(delegate { label2.Text = "相机2工作正常."; }));
                            label12.Invoke(new EventHandler(delegate { label12.Text = CAM2_TIME.ToString() + "/" + CAM2_TIME_75.ToString() + "/" + CAM2_Count.ToString(); }));
                        }

                        if (Global.ImageBufferIndex[1] != icImagingControlPort[1].ImageActiveBuffer.Index)
                        {
                            Global.ImageBufferIndex[1] = (Global.ImageBufferIndex[1] + 1) % icImagingControlPort[1].ImageRingBufferSize;
                        }

                        if (Global.ImageBufferIndex[1] == icImagingControlPort[1].ImageActiveBuffer.Index)
                        {
                            break;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明： 端口3相机控件接收到新的图像响应事件
        // 输入参数： 1、object：sender，icImagingControl控件对象
        //          2、ICImagingControl.ImageAvailableEventArgs：e，icImagingControl控件事件响应参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _icImagingControlPort3_ImageAvailable(object sender, ICImagingControl.ImageAvailableEventArgs e)
        {
            try
            {
                if (Global.ImageBufferIndex[2] != e.bufferIndex)
                {
                    Global.ImageBufferIndex[2] = e.bufferIndex;

                    while (true)
                    {
                        CAM3_Stopwatch.Restart();

                        //-------------------------------------------------------------------------------------------------------------------------------------
                        BmpDataPort[2][Global.ImageBufferIndex[2]] = icImagingControlPort[2].ImageBuffers[Global.ImageBufferIndex[2]].Bitmap.LockBits(Global.Camera[2].BitmapLockBitsArea, System.Drawing.Imaging.ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                        //-------------------------------------------------------------------------------------此段代码60个产生87M缓存，平均一个1.45M

                        Global.ImageSourceBufferIndex[2] = (Global.ImageSourceBufferIndex[2] + 1) % Global.ImageSourceBufferSizeMax;

                        DateTime dateTime = DateTime.Now;
                        ImageCount[2]++; CameraImageCount[2]++;
                        _BitmapDataProcessing(ImageSoursePort[2][Global.ImageSourceBufferIndex[2]], new Image<Bgr, Byte>(Global.Camera[2].BitmapLockBitsArea.Width, Global.Camera[2].BitmapLockBitsArea.Height, BmpDataPort[2][Global.ImageBufferIndex[2]].Stride, BmpDataPort[2][Global.ImageBufferIndex[2]].Scan0), Global.Camera[2].BitmapLockBitsResize, Global.Camera[2].BitmapLockBitsCenter, Global.Camera[2].BitmapLockBitsAxis);

                        if (Global.ShiftInformation.ShiftState)//班次状态使能
                        {
                            Global.MachineStopState[2]++;//机器停机状态标记变量
                            Global.MachineStopStaticSaveState[2] = true;//机器停机数据可以执行停机保存
                        }

                        Global.Camera[2].DeviceInformation.CAM = VisionSystemClassLibrary.Enum.CameraState.ON;
                        Global.CameraTemp[2].DeviceInformation.CAM = Global.Camera[2].DeviceInformation.CAM;

                        Int32 iImageRingBufferIndex = Global.ImageSourceBufferIndex[2];

                        Boolean bResultPort = true, bCheckTobaccoState = true;
                        Byte bErrorIndexPort = 0;
                        Boolean[] bToolProcessingResultFlag = null;
                        _SourceImageProcessing(2, iImageRingBufferIndex, ref bResultPort, ref bErrorIndexPort, ref bCheckTobaccoState,ref bToolProcessingResultFlag);//图像处理

                        _CameraImageImformationUpdate(2, bResultPort, bErrorIndexPort, iImageRingBufferIndex, bCheckTobaccoState, bToolProcessingResultFlag, dateTime);

                        CAM3_Stopwatch.Stop();
                        CAM3_TIME = CAM3_Stopwatch.Elapsed.TotalMilliseconds;
                        if (CAM3_TIME > CAM_TIME_MAX)
                        {
                            CAM3_TIME_75 = CAM3_TIME;
                            CAM3_Count++;
                        }

                        if (Global.ShowInformation)//显示调试信息
                        {
                            label13.Invoke(new EventHandler(delegate { label13.Text = "相机3工作正常."; }));
                            label16.Invoke(new EventHandler(delegate { label16.Text = CAM3_TIME.ToString() + "/" + CAM3_TIME_75.ToString() + "/" + CAM3_Count.ToString(); }));
                        }

                        if (Global.ImageBufferIndex[2] != icImagingControlPort[2].ImageActiveBuffer.Index)
                        {
                            Global.ImageBufferIndex[2] = (Global.ImageBufferIndex[2] + 1) % icImagingControlPort[2].ImageRingBufferSize;
                        }

                        if (Global.ImageBufferIndex[2] == icImagingControlPort[2].ImageActiveBuffer.Index)
                        {
                            break;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {

            }
        }

        //-----------------------------------------------------------------------
        // 功能说明： 端口4相机控件接收到新的图像响应事件
        // 输入参数： 1、object：sender，icImagingControl控件对象
        //          2、ICImagingControl.ImageAvailableEventArgs：e，icImagingControl控件事件响应参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _icImagingControlPort4_ImageAvailable(object sender, ICImagingControl.ImageAvailableEventArgs e)
        {
            try
            {
                if (Global.ImageBufferIndex[3] != e.bufferIndex)
                {
                    Global.ImageBufferIndex[3] = e.bufferIndex;

                    while (true)
                    {
                        CAM4_Stopwatch.Restart();

                        //-------------------------------------------------------------------------------------------------------------------------------------
                        BmpDataPort[3][Global.ImageBufferIndex[3]] = icImagingControlPort[3].ImageBuffers[Global.ImageBufferIndex[3]].Bitmap.LockBits(Global.Camera[3].BitmapLockBitsArea, System.Drawing.Imaging.ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                        //-------------------------------------------------------------------------------------此段代码60个产生87M缓存，平均一个1.45M

                        Global.ImageSourceBufferIndex[3] = (Global.ImageSourceBufferIndex[3] + 1) % Global.ImageSourceBufferSizeMax;

                        DateTime dateTime = DateTime.Now;
                        ImageCount[3]++; CameraImageCount[3]++;
                        _BitmapDataProcessing(ImageSoursePort[3][Global.ImageSourceBufferIndex[3]], new Image<Bgr, Byte>(Global.Camera[3].BitmapLockBitsArea.Width, Global.Camera[3].BitmapLockBitsArea.Height, BmpDataPort[3][Global.ImageBufferIndex[3]].Stride, BmpDataPort[3][Global.ImageBufferIndex[3]].Scan0), Global.Camera[3].BitmapLockBitsResize, Global.Camera[3].BitmapLockBitsCenter, Global.Camera[3].BitmapLockBitsAxis);

                        if (Global.ShiftInformation.ShiftState)//班次状态使能
                        {
                            Global.MachineStopState[3]++;//机器停机状态标记变量
                            Global.MachineStopStaticSaveState[3] = true;//机器停机数据可以执行停机保存
                        }

                        Global.Camera[3].DeviceInformation.CAM = VisionSystemClassLibrary.Enum.CameraState.ON;
                        Global.CameraTemp[3].DeviceInformation.CAM = Global.Camera[3].DeviceInformation.CAM;

                        Int32 iImageRingBufferIndex = Global.ImageSourceBufferIndex[3];

                        Boolean bResultPort = true, bCheckTobaccoState = true;
                        Byte bErrorIndexPort = 0;
                        Boolean[] bToolProcessingResultFlag = null;
                        _SourceImageProcessing(3, iImageRingBufferIndex, ref bResultPort, ref bErrorIndexPort, ref bCheckTobaccoState,ref bToolProcessingResultFlag);//图像处理

                        _CameraImageImformationUpdate(3, bResultPort, bErrorIndexPort, iImageRingBufferIndex, bCheckTobaccoState, bToolProcessingResultFlag, dateTime);

                        CAM4_Stopwatch.Stop();
                        CAM4_TIME = CAM4_Stopwatch.Elapsed.TotalMilliseconds;
                        if (CAM4_TIME > CAM_TIME_MAX)
                        {
                            CAM4_TIME_75 = CAM4_TIME;
                            CAM4_Count++;
                        }

                        if (Global.ShowInformation)//显示调试信息
                        {
                            label15.Invoke(new EventHandler(delegate { label15.Text = "相机4工作正常."; }));
                            label18.Invoke(new EventHandler(delegate { label18.Text = CAM4_TIME.ToString() + "/" + CAM4_TIME_75.ToString() + "/" + CAM4_Count.ToString(); }));
                        }

                        if (Global.ImageBufferIndex[3] != icImagingControlPort[3].ImageActiveBuffer.Index)
                        {
                            Global.ImageBufferIndex[3] = (Global.ImageBufferIndex[3] + 1) % icImagingControlPort[3].ImageRingBufferSize;
                        }

                        if (Global.ImageBufferIndex[3] == icImagingControlPort[3].ImageActiveBuffer.Index)
                        {
                            break;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {

            }
        }

        //-----------------------------------------------------------------------
        // 功能说明： 串口1控件接收到数据的处理函数
        // 输入参数： 1、Byte：iIndex，相机索引
        //           2、Byte[][]：bSensorADCValue，光电数据
        //           3、Byte：bWorkMode，正常工作模式
        //           4、Byte：bSensorAdjusting，校准过程标记
        //           5、Int32：iSensorAdjustResult，校准结果
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _serialPort_Available(Byte iIndex, Byte[][] bSensorADCValue, Byte bWorkMode, Byte bSensorAdjusting = 0, Int32 iSensorAdjustResult = 0xFFFFF)
        {
            try
            {
                switch (iIndex)
                {
                    case 0:
                        CAM1_Stopwatch.Restart();
                        break;
                    case 1:
                        CAM2_Stopwatch.Restart();
                        break;
                    case 2:
                        CAM3_Stopwatch.Restart();
                        break;
                    case 3:
                        CAM4_Stopwatch.Restart();
                        break;
                    default:
                        break;
                }

                Global.ImageSourceBufferIndex[iIndex] = (Global.ImageSourceBufferIndex[iIndex] + 1) % Global.ImageSourceBufferSizeMax;

                DateTime dateTime = DateTime.Now;
                ImageCount[iIndex]++;
                _SensorDataProcessing(iIndex, ImageSoursePort[iIndex][Global.ImageSourceBufferIndex[iIndex]], bSensorADCValue, bWorkMode, bSensorAdjusting, iSensorAdjustResult);

                if (Global.ShiftInformation.ShiftState)//班次状态使能
                {
                    Global.MachineStopState[iIndex]++;//机器停机状态标记变量
                    Global.MachineStopStaticSaveState[iIndex] = true;//机器停机数据可以执行停机保存
                }

                Global.Camera[iIndex].DeviceInformation.CAM = VisionSystemClassLibrary.Enum.CameraState.ON;
                Global.CameraTemp[iIndex].DeviceInformation.CAM = Global.Camera[iIndex].DeviceInformation.CAM;

                Int32 iImageRingBufferIndex = Global.ImageSourceBufferIndex[iIndex];

                Boolean bResultPort = true, bCheckTobaccoState = true;
                Byte bErrorIndexPort = 0;
                Boolean[] bToolProcessingResultFlag = null;
                _SourceImageProcessing(iIndex, iImageRingBufferIndex, ref bResultPort, ref bErrorIndexPort, ref bCheckTobaccoState, ref bToolProcessingResultFlag);//图像处理

                _CameraImageImformationUpdate(iIndex, bResultPort, bErrorIndexPort, iImageRingBufferIndex, bCheckTobaccoState, bToolProcessingResultFlag, dateTime);

                switch (iIndex)
                {
                    case 0:
                        CAM1_Stopwatch.Stop();
                        CAM1_TIME = CAM1_Stopwatch.Elapsed.TotalMilliseconds;
                        if (CAM1_TIME > CAM_TIME_MAX)
                        {
                            CAM1_TIME_75 = CAM1_TIME;
                            CAM1_Count++;
                        }
                        break;
                    case 1:
                        CAM2_Stopwatch.Stop();
                        CAM2_TIME = CAM2_Stopwatch.Elapsed.TotalMilliseconds;
                        if (CAM2_TIME > CAM_TIME_MAX)
                        {
                            CAM2_TIME_75 = CAM2_TIME;
                            CAM2_Count++;
                        }
                        break;
                    case 2:
                        CAM3_Stopwatch.Stop();
                        CAM3_TIME = CAM3_Stopwatch.Elapsed.TotalMilliseconds;
                        if (CAM3_TIME > CAM_TIME_MAX)
                        {
                            CAM3_TIME_75 = CAM3_TIME;
                            CAM3_Count++;
                        }
                        break;
                    case 3:
                        CAM4_Stopwatch.Stop();
                        CAM4_TIME = CAM4_Stopwatch.Elapsed.TotalMilliseconds;
                        if (CAM4_TIME > CAM_TIME_MAX)
                        {
                            CAM4_TIME_75 = CAM4_TIME;
                            CAM4_Count++;
                        }
                        break;
                    default:
                        break;
                }

                if (Global.ShowInformation)//显示调试信息
                {
                    switch (iIndex)
                    {
                        case 0:
                            label5.Invoke(new EventHandler(delegate { label5.Text = "串口1工作正常."; }));
                            label1.Invoke(new EventHandler(delegate { label1.Text = CAM1_TIME.ToString() + "/" + CAM1_TIME_75.ToString() + "/" + CAM1_Count.ToString(); }));
                            break;
                        case 1:
                            label2.Invoke(new EventHandler(delegate { label2.Text = "串口2工作正常."; }));
                            label12.Invoke(new EventHandler(delegate { label12.Text = CAM2_TIME.ToString() + "/" + CAM2_TIME_75.ToString() + "/" + CAM2_Count.ToString(); }));
                            break;
                        case 2:
                            label13.Invoke(new EventHandler(delegate { label13.Text = "串口3工作正常."; }));
                            label16.Invoke(new EventHandler(delegate { label16.Text = CAM3_TIME.ToString() + "/" + CAM3_TIME_75.ToString() + "/" + CAM3_Count.ToString(); }));
                            break;
                        case 3:
                            label15.Invoke(new EventHandler(delegate { label15.Text = "串口4工作正常."; }));
                            label18.Invoke(new EventHandler(delegate { label18.Text = CAM4_TIME.ToString() + "/" + CAM4_TIME_75.ToString() + "/" + CAM4_Count.ToString(); }));
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
        }
        
        //-----------------------------------------------------------------------
        // 功能说明： 图像处理函数
        // 输入参数：1、Image<Bgr, Byte>：imageOUT，输出图像
        //           2、Image<Bgr, Byte>：imageIN，输入图像
        //           3、Boolean：bitmapResize，输入图像缩放标记
        //           4、Boolean：bitmapCenter，输入图像居中标记
        //           5、Boolean：bBitmapAxis，输入图像粘贴位置
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _BitmapDataProcessing(Image<Bgr, Byte> imageOUT, Image<Bgr, Byte> imageIN, Boolean bitmapResize, Boolean bitmapCenter, Point bBitmapAxis)
        {
            Double widthScale = Convert.ToDouble(imageIN.Width) / imageOUT.Width;
            Double heightScale = Convert.ToDouble(imageIN.Height) / imageOUT.Height;

            if (bitmapResize)//图像进行缩放 
            {
                if (widthScale < heightScale)//输入/输出图像宽度比较大
                {
                    imageIN = imageIN.Resize(widthScale, INTER.CV_INTER_CUBIC);//按输入图像宽度缩放
                }
                else
                {
                    imageIN = imageIN.Resize(heightScale, INTER.CV_INTER_CUBIC); //按输入图像高度缩放
                }
            }

            imageOUT.SetZero();

            if (bitmapCenter)//图像居中显示
            {
                imageOUT.ROI = new Rectangle((imageOUT.Width - imageIN.Width) / 2, (imageOUT.Height - imageIN.Height) / 2, imageIN.Width, imageIN.Height);
            }
            else//图像默认显示于左上角
            {
                imageOUT.ROI = new Rectangle(bBitmapAxis.X, bBitmapAxis.Y, imageIN.Width, imageIN.Height);
            }
            imageOUT._Or(imageIN);
            imageOUT.ROI = Rectangle.Empty;
        }

        //-----------------------------------------------------------------------
        // 功能说明： 图像处理函数
        // 输入参数： 1、Byte：index，数组下标索引
        //            2、Image<Bgr, Byte>：imageOUT，输出图像
        //            3、Byte[][]：bSensorADCValue，采集的光电数据
        //            4、Byte：bWorkMode，正常工作模式
        //            5、Byte：bSensorAdjusting，校准过程标记
        //            6、Int32：iSensorAdjustResult，校准结果
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _SensorDataProcessing(Byte iIndex, Image<Bgr, Byte> imageOUT, Byte[][] bSensorADCValue, Byte bWorkMode, Byte bSensorAdjusting, Int32 iSensorAdjustResult = 0xFFFFF)
        {
            imageOUT.SetZero();

            Byte bTobaccoSortType_E = (Byte)Global.Camera[iIndex].TobaccoSortType_E;

            PointF pTobaccoPosionInfo = new PointF(0, 0);
            Rectangle pDrawStringInfo = new Rectangle();

            Brush bBrush = Brushes.White;
            Font fFont = new Font("微软雅黑", 14.0F, FontStyle.Regular);

            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;//水平居中
            format.LineAlignment = StringAlignment.Center;//垂直居中

            if (1 == bWorkMode) //正常工作模式
            {
                if (Global.Camera[iIndex].ContinuousSampling) //连续采样
                {
                    switch (Global.Camera[iIndex].Sensor_ProductType)
                    {
                        case VisionSystemClassLibrary.Enum.SensorProductType._89713FA:
                            for (Byte i = 0; i < Global.Camera[iIndex].SensorNumber; i++) //循环所有烟支
                            {
                                double dPresion = (double)imageOUT.Width / SensorADCValue[iIndex][i].Length;

                                Int32 pointYAxis = Convert.ToInt32((double)imageOUT.Height / Global.Camera[iIndex].SensorNumber);
                                Point[] points = new Point[SensorADCValue[iIndex][i].Length];
                                for (Int32 j = 0; j < SensorADCValue[iIndex][i].Length; j++)
                                {
                                    points[j].X = Convert.ToInt32(dPresion * j);

                                    Double bADCValue = SensorADCValue[iIndex][i][j];
                                    if (SensorADCValue[iIndex][i][j] > 204) //截取4V以上电压
                                    {
                                        bADCValue = 204;
                                    }
                                    else if ((3 == Global.Camera[iIndex].SensorNumber) && (SensorADCValue[iIndex][i][j] < 51)) //截取1V以下电压
                                    {
                                        bADCValue = 51;
                                    }
                                    points[j].Y = Convert.ToInt32(pointYAxis * i + 204 - bADCValue);
                                }
                                imageOUT.DrawPolyline(points, false, new Bgr(255, 255, 255), 1);
                            }
                            break;
                        case VisionSystemClassLibrary.Enum.SensorProductType._89713CF:
                            for (Byte i = 0; i < Global.Camera[iIndex].SensorNumber; i++) //循环所有烟支
                            {
                                double dPresion = (double)imageOUT.Width / SensorADCValue[iIndex][i].Length;

                                Int32 pointYAxis = Convert.ToInt32((double)imageOUT.Height / Global.Camera[iIndex].SensorNumber);
                                Point[] points = new Point[SensorADCValue[iIndex][i].Length];
                                for (Int32 j = 0; j < SensorADCValue[iIndex][i].Length; j++)
                                {
                                    points[j].X = Convert.ToInt32(dPresion * j);

                                    Double bADCValue = SensorADCValue[iIndex][i][j];
                                    if (SensorADCValue[iIndex][i][j] < 51) //截取1V以下电压
                                    {
                                        bADCValue = 51;
                                    }

                                    points[j].Y = Convert.ToInt32(pointYAxis * i + 255 - bADCValue);
                                }
                                imageOUT.DrawPolyline(points, false, new Bgr(255, 255, 255), 1);
                            }
                            break;
                        case VisionSystemClassLibrary.Enum.SensorProductType._89750A:
                            break;
                        default:
                            break;
                    }
                } 
                else //非连续采样
                {
                    for (Byte i = 0; i < Global.Camera[iIndex].SensorNumber; i++) //循环所有烟支
                    {
                        pTobaccoPosionInfo.X = Global.TobaccoPosionInfo[0][bTobaccoSortType_E - 1][i].X;
                        pTobaccoPosionInfo.Y = Global.TobaccoPosionInfo[0][bTobaccoSortType_E - 1][i].Y;

                        pDrawStringInfo = Global.TobaccoPosionInfo[0][bTobaccoSortType_E - 1][i];
                        pDrawStringInfo.Offset(-pDrawStringInfo.Width, -pDrawStringInfo.Height);
                        pDrawStringInfo.Width *= 2;
                        pDrawStringInfo.Height *= 2;

                        imageOUT.Draw(new CircleF(pTobaccoPosionInfo, Global.TobaccoPosionInfo[0][bTobaccoSortType_E - 1][i].Width), new Bgr(bSensorADCValue[i][0], bSensorADCValue[i][0], bSensorADCValue[i][0]), -1);

                        if (bSensorADCValue[i][0] < 128) //暗色
                        {
                            bBrush = Brushes.White;
                        }
                        else
                        {
                            bBrush = Brushes.Black;
                        }
                        Graphics.FromImage(imageOUT.Bitmap).DrawString((Convert.ToInt32((Double)(bSensorADCValue[i][0]) * 5000 / 255)).ToString(), fFont, bBrush, pDrawStringInfo, format);
                    }
                }
            }
            else
            {
                bBrush = Brushes.Black;
                
                Int32 iTobaccoPosionInfoIndex =0;
                if (Global.Camera[iIndex].ContinuousSampling) //连续采样
                {
                    iTobaccoPosionInfoIndex = 1;
                }

                if (1 == bSensorAdjusting) //当前在校准过程中
                {
                    for (Byte i = 0; i < Global.Camera[iIndex].SensorNumber; i++) //循环所有烟支
                    {
                        pTobaccoPosionInfo.X = Global.TobaccoPosionInfo[iTobaccoPosionInfoIndex][bTobaccoSortType_E - 1][i].X;
                        pTobaccoPosionInfo.Y = Global.TobaccoPosionInfo[iTobaccoPosionInfoIndex][bTobaccoSortType_E - 1][i].Y;

                        pDrawStringInfo = Global.TobaccoPosionInfo[iTobaccoPosionInfoIndex][bTobaccoSortType_E - 1][i];
                        pDrawStringInfo.Offset(-pDrawStringInfo.Width, -pDrawStringInfo.Height);
                        pDrawStringInfo.Width *= 2;
                        pDrawStringInfo.Height *= 2;

                        imageOUT.Draw(new CircleF(pTobaccoPosionInfo, Global.TobaccoPosionInfo[iTobaccoPosionInfoIndex][bTobaccoSortType_E - 1][i].Width), new Bgr(0, 255, 255), -1);

                        Graphics.FromImage(imageOUT.Bitmap).DrawString((Convert.ToInt32((Double)(bSensorADCValue[i][0]) * 5000 / 255)).ToString(), fFont, bBrush, pDrawStringInfo, format);
                    }
                }
                else
                {
                    for (Byte i = 0; i < Global.Camera[iIndex].SensorNumber; i++) //循环所有烟支
                    {
                        pTobaccoPosionInfo.X = Global.TobaccoPosionInfo[iTobaccoPosionInfoIndex][bTobaccoSortType_E - 1][i].X;
                        pTobaccoPosionInfo.Y = Global.TobaccoPosionInfo[iTobaccoPosionInfoIndex][bTobaccoSortType_E - 1][i].Y;

                        pDrawStringInfo = Global.TobaccoPosionInfo[iTobaccoPosionInfoIndex][bTobaccoSortType_E - 1][i];
                        pDrawStringInfo.Offset(-pDrawStringInfo.Width, -pDrawStringInfo.Height);
                        pDrawStringInfo.Width *= 2;
                        pDrawStringInfo.Height *= 2;

                        if ((iSensorAdjustResult & (0x01 << i)) != 0)//校准成功
                        {
                            imageOUT.Draw(new CircleF(pTobaccoPosionInfo, Global.TobaccoPosionInfo[iTobaccoPosionInfoIndex][bTobaccoSortType_E - 1][i].Width), new Bgr(0, 255, 0), -1);

                            Graphics.FromImage(imageOUT.Bitmap).DrawString((Convert.ToInt32((Double)(bSensorADCValue[i][0]) * 5000 / 255)).ToString(), fFont, bBrush, pDrawStringInfo, format);
                        }
                        else //校准失败
                        {
                            imageOUT.Draw(new CircleF(pTobaccoPosionInfo, Global.TobaccoPosionInfo[iTobaccoPosionInfoIndex][bTobaccoSortType_E - 1][i].Width), new Bgr(0, 0, 255), -1);

                            Graphics.FromImage(imageOUT.Bitmap).DrawString((Convert.ToInt32((Double)(bSensorADCValue[i][0]) * 5000 / 255)).ToString(), fFont, bBrush, pDrawStringInfo, format);
                        }
                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明： 图像处理函数
        // 输入参数：1、Byte:index，数组下表索引        
        //           2、Int32：iImageRingBufferIndex，处理图像索引
        //           3、Boolean：bResultPort，处理结果
        //           4、Byte：iErrorIndexPort，第一个缺陷工具索引
        //           5、Boolean：bCheckTobaccoState，非空模盒标记:True为非空；FALSE为空
        //           6、Boolean[]：bToolProcessingResultFlag，所有检测工具结果
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _SourceImageProcessing(Byte index, Int32 iImageRingBufferIndex, ref Boolean bResultPort, ref Byte bErrorIndexPort, ref Boolean bCheckTobaccoState,ref Boolean[] bToolProcessingResultFlag)
        {
            lock(LockImageProcessing[index])
            {
                bToolProcessingResultFlag = Global.Camera[index]._ImageProcess(ImageSoursePort[index][iImageRingBufferIndex]);
            }
            
            for (Byte i = 0; i < Global.Camera[index].Tools.Count; i++)//遍历当前所有工具
            {
                if (Global.Camera[index].Tools[i].ToolState)//判断当前工具使用状态
                {
                    _TolerancesGraphDataInit(index, i, Global.Camera[index].Tools[i].Value);//公差数据值更新
                }

                if (Global.Camera[index].Tools[i].CheckTobaccoState)//更新空模盒标记
                {
                    bCheckTobaccoState = bToolProcessingResultFlag[i];
                }
            }

            for (Byte i = 0; i < bToolProcessingResultFlag.Length; i++)//遍历检测结果
            {
                if (false == bToolProcessingResultFlag[i])//当前工具是否存在缺陷
                {
                    bErrorIndexPort = i;
                    bResultPort = false;
                    ImageBadCount[index]++;
                    break;
                }
            }

            if ((false == Global.Camera[index].IsSerialPort) && (false == LabModel) && (false == Global.ComputerRunState) && (false == CloseSerialPortFlag))  //当前处于非实验室界面，控制器上运行软件，当前串口正常工作，无需关闭
            {
                lock (LockControllerSerialPort)
                {
                    _SendCommand(9, index, bResultPort, bErrorIndexPort, bCheckTobaccoState);
                }
            }

            if ((false == LabModel) && bCheckTobaccoState && Global.ShiftInformation.ShiftState)//当前非空模盒，班次状态使能
            {
                if (bResultPort)//当前烟包完好
                {
                    StaticRejectImageSaveConNumberPort[index] = 0;
                }
                else//当前烟包存在缺陷
                {
                    StaticRejectImageSaveConNumberPort[index]++;
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明： 学习图像处理函数
        // 输入参数：1、Byte:index，数组下表索引 
        //           2、Image<Bgr, Byte>：image，待处理的图像
        //           3、Int32：toolIndex，工具索引
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _LearnImageProcessing(Byte index, Image<Bgr, Byte> image, Int32 toolIndex)
        {
            if (image != null)
            {
                Global.CameraTemp[index]._LearnSample(image);

                Stopwatch_QualityCheck.Restart();

                Boolean[] flag = new Boolean[Global.CameraTemp[index].Tools.Count];
                lock (LockImageProcessing[index])
                {
                    flag = Global.CameraTemp[index]._ImageProcess(image);//计算当前工具处理结果
                }

                Stopwatch_QualityCheck.Stop();

                VisionSystemClassLibrary.Struct.ImageInformation iImageInformation = new VisionSystemClassLibrary.Struct.ImageInformation();
                _UpdateLearnImageBuffGraphicsInformation(index, ref iImageInformation, toolIndex, Convert.ToInt32((Stopwatch_QualityCheck.Elapsed.TotalMilliseconds) * 100), flag[toolIndex]);

                iImageInformation._CopyTo(Global.CameraTemp[index].Learn);
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明： 图像信息绘制
        // 输入参数： 1、Image<Bgr, Byte>：image:输入图像
        //          2、VisionSystemClassLibrary.Struct.ImageInformation：imageInformation，图像信息
        //          3、VisionSystemClassLibrary.Class.Tools：imageTool，图像工具
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _DrawImageInformation(Image<Bgr, Byte> image, VisionSystemClassLibrary.Struct.ImageInformation imageInformation, VisionSystemClassLibrary.Class.Tools imageTool)
        {
            DateTime currentTime = imageInformation.DateTimeImage;
            String writerDateTimeOnImage =
                currentTime.ToString("yyyy.MM.dd") + " - " + currentTime.Hour.ToString("00") + ":"
                + currentTime.Minute.ToString("00") + ":" + currentTime.Second.ToString("00") + ":" + currentTime.Millisecond.ToString("0000");

            Brush bBrush = Brushes.Yellow;
            Font fFont = new Font("微软雅黑", 20.0F, FontStyle.Regular);
            Graphics.FromImage(image.Bitmap).DrawString(writerDateTimeOnImage, fFont, bBrush, new PointF(0, image.Height - fFont.Height));

            try
            {
                imageTool._Drawing(image);
            }
            catch (System.Exception ex)
            {

            }
        }

        //-----------------------------------------------------------------------
        // 功能说明： 图像信息绘制
        // 输入参数： 1、Image<Bgr, Byte>：image:输入图像
        //           2、VisionSystemClassLibrary.Struct.ImageInformation：imageInformation，图像信息
        //           3、Boolean：bDeepLearningState，深度学习标记结果
        //           4、string：sTypeName，深度学习结果
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _DrawImageInformation(ref Image<Bgr, Byte> image, VisionSystemClassLibrary.Struct.ImageInformation imageInformation, Boolean bDeepLearningState, string sTypeName)
        {
            DateTime currentTime = imageInformation.DateTimeImage;
            String writerDateTimeOnImage =
                currentTime.ToString("yyyy.MM.dd") + " - " + currentTime.Hour.ToString("00") + ":"
                + currentTime.Minute.ToString("00") + ":" + currentTime.Second.ToString("00") + ":" + currentTime.Millisecond.ToString("0000");

            Brush bBrush = Brushes.Yellow;
            Font fFont = new Font("微软雅黑", 20.0F, FontStyle.Regular);
            Graphics.FromImage(image.Bitmap).DrawString(writerDateTimeOnImage, fFont, bBrush, new PointF(0, image.Height - fFont.Height));

            try
            {
                if (bDeepLearningState)//深度学习工具
                {
                    MCvFont font = new MCvFont(FONT.CV_FONT_HERSHEY_COMPLEX, 1.5, 1.5);
                    if ("OK" == sTypeName)//检测结果完好
                    {
                        image.Draw(sTypeName, ref font, new Point(670, 55), new Bgr(0, 255, 0));
                    }
                    else
                    {
                        image.Draw(sTypeName, ref font, new Point(670, 55), new Bgr(0, 0, 255));
                    }
                }

                VisionSystemClassLibrary.Struct.ROI roiTemp = new VisionSystemClassLibrary.Struct.ROI();
                imageInformation.ROI_StaticsImage._CopyTo(ref roiTemp);
                roiTemp._Offset(imageInformation.Compensation_H, imageInformation.Compensation_V);
                VisionSystemClassLibrary.GeneralFunction._DrawGraphics(ref image, roiTemp, new Bgr(0, 255, 0));
            }
            catch (System.Exception ex)
            {

            }
        }

        //-----------------------------------------------------------------------
        // 功能说明： 更新原始图像信息到缓冲区
        // 输入参数：1、Byte:index，数组下表索引
        //                    2、Int32：iImageRingBufferIndex，处理图像索引
        //                    3、Boolean[]：bToolProcessingResultFlag，所有检测工具结果
        //                    4、DateTime：datetime图像工具索引
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _UpdateSourseImageBuffInformation(Byte index, Int32 iImageRingBufferIndex, Boolean[] bToolProcessingResultFlag, DateTime datetime)
        {
            lock (LockSourseImageBuffPort[index])
            {
                switch (SourseImageBuffIndexPort[index])                              //当前正在执行保存图像缓冲区索引
                {
                    case 0:
                        SourseImageBuff0FlagPort[index] = true;
                        SourseImageBuff1FlagPort[index] = false;

                        SourseImageBuffPort[index][1 - SourseImageBuffIndexPort[index]] = ImageSoursePort[index][iImageRingBufferIndex];

                        for (Int32 i = 0; i < Global.Camera[index].Tools.Count; i++) //循环更新图像和工具信息
                        {
                            _UpdateImageBuffGraphicsInformation(index, ref SourseImageBuffGraphicsInformationPort[index][1 - SourseImageBuffIndexPort[index]][i], bToolProcessingResultFlag[i], i, datetime);
                            Global.Camera[index].Tools[i]._CopyTo(ref SourseImageBuffToolInformationPort[index][1 - SourseImageBuffIndexPort[index]][i]);
                        }
                        SourseImageBuffIndexPort[index] = Convert.ToByte(1 - SourseImageBuffIndexPort[index]);

                        SourseImageBuff0FlagPort[index] = false;
                        SourseImageBuff1FlagPort[index] = true;
                        break;
                    case 1:
                        SourseImageBuff0FlagPort[index] = false;
                        SourseImageBuff1FlagPort[index] = true;

                        SourseImageBuffPort[index][1 - SourseImageBuffIndexPort[index]] = ImageSoursePort[index][iImageRingBufferIndex];

                        for (Int32 i = 0; i < Global.Camera[index].Tools.Count; i++) //循环更新图像和工具信息
                        {
                            _UpdateImageBuffGraphicsInformation(index, ref SourseImageBuffGraphicsInformationPort[index][1 - SourseImageBuffIndexPort[index]][i], bToolProcessingResultFlag[i], i, datetime);
                            Global.Camera[index].Tools[i]._CopyTo(ref SourseImageBuffToolInformationPort[index][1 - SourseImageBuffIndexPort[index]][i]);
                        }
                        SourseImageBuffIndexPort[index] = Convert.ToByte(1 - SourseImageBuffIndexPort[index]);

                        SourseImageBuff0FlagPort[index] = true;
                        SourseImageBuff1FlagPort[index] = false;
                        break;
                    default:
                        break;
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明： 更新剔除图像信息到缓冲区
        // 输入参数：1、Byte:index，数组下标索引
        //                    2、Int32：iImageRingBufferIndex，处理图像索引
        //                    3、Boolean[]：bToolProcessingResultFlag，所有检测工具结果
        //                    4、DateTime：datetime图像工具索引
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _UpdateRejectImageBuffInformation(Byte index, Int32 iImageRingBufferIndex, Boolean[] bToolProcessingResultFlag, DateTime datetime)
        {
            lock (LockRejectImageBuffPort[index])
            {
                switch (RejectImageBuffIndexPort[index])                              //当前正在执行保存图像缓冲区索引
                {
                    case 0:
                        RejectImageBuff0FlagPort[index] = true;
                        RejectImageBuff1FlagPort[index] = false;

                        RejectImageBuffPort[index][1 - RejectImageBuffIndexPort[index]] = ImageSoursePort[index][iImageRingBufferIndex].Copy();

                        RejectImageBuffIndexPort[index] = Convert.ToByte(1 - RejectImageBuffIndexPort[index]);

                        for (Int32 i = 0; i < Global.Camera[index].Tools.Count; i++) //循环更新图像和工具信息
                        {
                            _UpdateImageBuffGraphicsInformation(index, ref RejectImageBuffGraphicsInformationPort[index][1 - RejectImageBuffIndexPort[index]][i], bToolProcessingResultFlag[i], i, datetime);
                            Global.Camera[index].Tools[i]._CopyTo(ref RejectImageBuffToolInformationPort[index][1 - RejectImageBuffIndexPort[index]][i]);
                        }

                        RejectImageBuff0FlagPort[index] = false;
                        RejectImageBuff1FlagPort[index] = true;
                        break;
                    case 1:
                        RejectImageBuff0FlagPort[index] = false;
                        RejectImageBuff1FlagPort[index] = true;

                        RejectImageBuffPort[index][1 - RejectImageBuffIndexPort[index]] = ImageSoursePort[index][iImageRingBufferIndex].Copy();

                        RejectImageBuffIndexPort[index] = Convert.ToByte(1 - RejectImageBuffIndexPort[index]);

                        for (Int32 i = 0; i < Global.Camera[index].Tools.Count; i++) //循环更新图像和工具信息
                        {
                            _UpdateImageBuffGraphicsInformation(index, ref RejectImageBuffGraphicsInformationPort[index][1 - RejectImageBuffIndexPort[index]][i], bToolProcessingResultFlag[i], i, datetime);
                            Global.Camera[index].Tools[i]._CopyTo(ref RejectImageBuffToolInformationPort[index][1 - RejectImageBuffIndexPort[index]][i]);
                        }

                        RejectImageBuff0FlagPort[index] = true;
                        RejectImageBuff1FlagPort[index] = false;
                        break;
                    default:
                        break;
                }
            }
        }
                
        //----------------------------------------------------------------------
        // 功能说明：更新学习图像信息
        // 输入参数：1、Byte:index，数组下表索引
        //           2、VisionSystemClassLibrary.Struct.ImageInformation ：imageInformation图像信息
        //           3、Int32：inspectionTime，图像处理时间
        //           4、Byte：toolIndex图像工具索引
        //           5、Boolean：result图像处理结果
        //           6、Boolean：cameraTemp使用局部变量标记
        //           7、Double：liveScale实时图像尺寸
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _UpdateLearnImageBuffGraphicsInformation(Byte index, ref VisionSystemClassLibrary.Struct.ImageInformation imageInformation, Int32 toolIndex, Int32 inspectionTime = 0, Boolean result = true)
        {
            imageInformation.Valid = true;
            imageInformation.ToolsIndex = toolIndex;
            imageInformation.Scale = 1.0;

            if (result)//当前相机处理结果：完好
            {
                imageInformation.Type = VisionSystemClassLibrary.Enum.ImageType.Ok;
                imageInformation.Name = "OK";
            }
            else//当前相机处理结果：缺陷
            {
                imageInformation.Type = VisionSystemClassLibrary.Enum.ImageType.Error;
                imageInformation.Name = Global.CameraTemp[index].Tools[toolIndex].ToolsENGName;
            }

            imageInformation.Value = new Boolean[VisionSystemClassLibrary.Struct.ImageInformation.TotalNumber];
            for (int i = 0; i < VisionSystemClassLibrary.Struct.ImageInformation.TotalNumber; i++)
            {
                imageInformation.Value[i] = true;
            }

            imageInformation.ToolState = Global.CameraTemp[index].Tools[toolIndex].ToolState;
            imageInformation.MinValue = Global.CameraTemp[index].Tools[toolIndex].Min;
            imageInformation.MaxValue = Global.CameraTemp[index].Tools[toolIndex].Max;

            if ((null != Global.CameraTemp[index].Tools[toolIndex].Value) && (Global.CameraTemp[index].Tools[toolIndex].Value.Length > 0))
            {
                imageInformation.CurrentValue = Global.CameraTemp[index].Tools[toolIndex].Value[0];
            }

            if (false == Global.CameraTemp[index].Tools[toolIndex].DeepLearningState)
            {
                imageInformation.Compensation_H = Global.CameraTemp[index].Tools[toolIndex].Compensation_H;
                imageInformation.Compensation_V = Global.CameraTemp[index].Tools[toolIndex].Compensation_V;
            }
            
            imageInformation.ValueDisplay = true;
            imageInformation.DateTimeImage = DateTime.Now;
            imageInformation.ErrorValue = 0;
            imageInformation.StepValue = 0;

            imageInformation.InspectionTime = inspectionTime;
        }

        //----------------------------------------------------------------------
        // 功能说明：更新图像信息
        // 输入参数：1、Byte:index，数组下表索引
        //                    2、VisionSystemClassLibrary.Struct.ImageInformation[]：imageInformation缺陷图像信息
        //                    3、Boolean：bResultPort图像工具索引
        //                    4、Byte：toolIndex图像工具索引
        //                    5、DateTime：datetime图像工具索引
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _UpdateImageBuffGraphicsInformation(Byte index, ref VisionSystemClassLibrary.Struct.ImageInformation imageInformation, Boolean bResultPort, Int32 toolIndex, DateTime datetime)
        {
            imageInformation.Valid = true;
            imageInformation.ToolsIndex = toolIndex;
            imageInformation.ToolState = Global.Camera[index].Tools[toolIndex].ToolState;
            imageInformation.Scale = 1.0;

            if (bResultPort)//当前相机处理结果：完好
            {
                imageInformation.Type = VisionSystemClassLibrary.Enum.ImageType.Ok;
                imageInformation.Name = "OK";
            }
            else//当前相机处理结果：缺陷
            {
                imageInformation.Type = VisionSystemClassLibrary.Enum.ImageType.Error;
                imageInformation.Name = Global.Camera[index].Tools[toolIndex].ToolsENGName;
            }

            imageInformation.Value = new Boolean[VisionSystemClassLibrary.Struct.ImageInformation.TotalNumber];
            for (int i = 0; i < VisionSystemClassLibrary.Struct.ImageInformation.TotalNumber; i++)
            {
                imageInformation.Value[i] = true;
            }
            imageInformation.ValueDisplay = true;
            imageInformation.ToolState = Global.Camera[index].Tools[toolIndex].ToolState;
            imageInformation.MinValue = Global.Camera[index].Tools[toolIndex].Min;
            imageInformation.MaxValue = Global.Camera[index].Tools[toolIndex].Max;

            if ((null != Global.Camera[index].Tools[toolIndex].Value) && (Global.Camera[index].Tools[toolIndex].Value.Length > 0))
            {
                imageInformation.CurrentValue = Global.Camera[index].Tools[toolIndex].Value[0];
            }
            imageInformation.DateTimeImage = datetime;
            imageInformation.ErrorValue = 0;
            imageInformation.StepValue = 0;

            if (false == Global.Camera[index].Tools[toolIndex].DeepLearningState)
            {
                imageInformation.Compensation_H = Global.Camera[index].Tools[toolIndex].Compensation_H;
                imageInformation.Compensation_V = Global.Camera[index].Tools[toolIndex].Compensation_V;
            }

            imageInformation.DeepLearningState = Global.Camera[index].Tools[toolIndex].DeepLearningState;
            Global.Camera[index].Tools[toolIndex].ROI._CopyTo(ref imageInformation.ROI_StaticsImage);
        }

        //----------------------------------------------------------------------
        // 功能说明：计算质量检测界面图像信息
        // 输入参数：1、Byte:index，数组下表索引
        //           2、VisionSystemClassLibrary.Struct.ImageInformation：imageInformation，图像信息
        //           3、Int32：imageType，图像类型
        // 输出参数：无
        // 返回值： 无
        //----------------------------------------------------------------------
        private void _UpdateQualityCheckGraphicsInformation(Byte index, ref VisionSystemClassLibrary.Struct.ImageInformation imageInformation, Int32 imageType = 1)
        {
            Stopwatch_QualityCheck.Restart();

            Image<Bgr, Byte> image = null;

            switch (imageType)
            {
                case 1:
                    image = ImageSourseQualityCheck[index];
                    break;
                case 2:
                    image = Global.CameraTemp[index].ImageLearn;
                    break;
                case 3:
                    image = Global.CameraTemp[index].ImageReject;
                    break;
                default:
                    break;
            }

            Boolean[] flag = new Boolean[Global.CameraTemp[index].Tools.Count];
            lock (LockImageProcessing[index])
            {
                flag = Global.CameraTemp[index]._ImageProcess(image);//计算当前工具处理结果
            }

            Stopwatch_QualityCheck.Stop();
            _UpdateLearnImageBuffGraphicsInformation(index, ref imageInformation, QualityCheckToolIndex[index], Convert.ToInt32(Stopwatch_QualityCheck.Elapsed.TotalMilliseconds * 100), flag[QualityCheckToolIndex[index]]);
        }

        // 功能说明：质量检测界面工具学习
        // 输入参数：1、Byte:index，数组下表索引
        //           2、Int32：imageType，图像类型
        // 输出参数：无
        // 返回值： 无
        //----------------------------------------------------------------------
        private void _LearnToolProcessing(Byte index, Int32 imageType = 1)
        {
            Image<Bgr, Byte> image = null;

            switch (imageType)
            {
                case 1:
                    image = ImageSourseQualityCheck[index];
                    break;
                case 2:
                    image = Global.CameraTemp[index].ImageLearn;
                    break;
                case 3:
                    image = Global.CameraTemp[index].ImageReject;
                    break;
                default:
                    break;
            }
            Global.CameraTemp[index]._LearnSample(image);
        }

        //-----------------------------------------------------------------------
        // 功能说明： 获取图像信息索引
        // 输入参数： VisionSystemClassLibrary.Struct.ImageInformation[]：imageInformation，图像信息数组
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private Int32 _GetToolIndex(Int32 index, VisionSystemClassLibrary.Struct.ImageInformation[] imageInformation)
        {
            Int32 iToolIndex = 0;

            for (Byte i = 0; i < imageInformation.Length; i++)//遍历当前所有工具
            {
                if (imageInformation[i].ToolState && Global.Camera[index].Tools[i].ExistTolerance)//判断当前工具使用状态
                {
                    iToolIndex = i;
                    break;
                }
            }
            return iToolIndex;
        }

        //-----------------------------------------------------------------------
        // 功能说明： 获取图像信息索引
        // 输入参数： VisionSystemClassLibrary.Struct.ImageInformation[]：imageInformation，图像信息数组
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _GetToolIndex(VisionSystemClassLibrary.Struct.ImageInformation[] imageInformation, ref Boolean bResult,ref Int32 iToolIndex)
        {
            bResult = true;

            for (Int32 i = 0; i < imageInformation.Length; i++) //循环所有图像信息
            {
                if (imageInformation[i].ToolState && (VisionSystemClassLibrary.Enum.ImageType.Error == imageInformation[i].Type)) //当前为缺陷
                {
                    iToolIndex = i;
                    bResult = false;
                    break;
                }
            }

            if (bResult)//当前烟包完好
            {
                for (Byte i = 0; i < imageInformation.Length; i++)//遍历当前所有工具
                {
                    if (imageInformation[i].ToolState && (VisionSystemClassLibrary.Enum.ImageType.Ok == imageInformation[i].Type))//判断当前工具使用状态
                    {
                        iToolIndex = i;
                        break;
                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明： 保存缺陷图像线程
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _Images_totalSave(object parameter)
        {
            while (true)
            {
                Byte index = (Byte)parameter;

                Boolean bContainsKey = true;
                lock (LockCheck_CameraPort)
                {
                    bContainsKey = Global.Check_CameraPort.ContainsKey(Global.Camera[index].Type);
                }

                if (bContainsKey)
                {
                    if (Global.ShiftInformation_totalSave.TransMode)
                    {
                        Boolean bConncetState = false;
                        switch (index)
                        {
                            case 0:
                                if (tcpClient1.dllConnected)
                                {
                                    bConncetState = true;
                                }
                                break;
                            case 1:
                                if (tcpClient2.dllConnected)
                                {
                                    bConncetState = true;
                                }
                                break;
                            case 2:
                                if (tcpClient3.dllConnected)
                                {
                                    bConncetState = true;
                                }
                                break;
                            case 3:
                                if (tcpClient4.dllConnected)
                                {
                                    bConncetState = true;
                                }
                                break;
                            default:
                                break;
                        }

                        if (bConncetState && (null != Images_totalSave[index]) && (Images_totalSave[index].Count > 0) && (null != Images_totalSave_Information[index]) && (Images_totalSave_Information[index].Count > 0)) //以太网连接正常，图像信息满足要求，图像满足要求
                        {
                            MemoryStream memoryStream = new MemoryStream();

                            switch (index)
                            {
                                case 0:
                                    if (Global.ShiftInformation_totalSave.SavingFileFoamat.ToLower() == ".bmp") //
                                    {
                                        Images_totalSave[index].First().Bitmap.Save(memoryStream, ImageFormat.Bmp);
                                        tcpClient1.SendImage(memoryStream, Images_totalSave_Information[index].First(), "", "");

                                        Images_totalSave[index].RemoveAt(0);
                                        Images_totalSave_Information[index].RemoveAt(0);

                                        PopImageCount[index]++;

                                        Thread.Sleep(2);
                                    }
                                    else if (Global.ShiftInformation_totalSave.SavingFileFoamat.ToLower() == ".jpg") //
                                    {
                                        Images_totalSave[index].First().Bitmap.Save(memoryStream, ImageFormat.Jpeg);
                                        tcpClient1.SendImage(memoryStream, Images_totalSave_Information[index].First(), "", "");

                                        Images_totalSave[index].RemoveAt(0);
                                        Images_totalSave_Information[index].RemoveAt(0);

                                        PopImageCount[index]++;

                                        Thread.Sleep(2);
                                    }
                                    else if (Global.ShiftInformation_totalSave.SavingFileFoamat.ToLower() == ".png") //
                                    {
                                        Images_totalSave[index].First().Bitmap.Save(memoryStream, ImageFormat.Png);
                                        tcpClient1.SendImage(memoryStream, Images_totalSave_Information[index].First(), "", "");

                                        Images_totalSave[index].RemoveAt(0);
                                        Images_totalSave_Information[index].RemoveAt(0);

                                        PopImageCount[index]++;

                                        Thread.Sleep(2);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                    break;
                                case 1:
                                    if (Global.ShiftInformation_totalSave.SavingFileFoamat.ToLower() == ".bmp") //
                                    {
                                        Images_totalSave[index].First().Bitmap.Save(memoryStream, ImageFormat.Bmp);
                                        tcpClient2.SendImage(memoryStream, Images_totalSave_Information[index].First(), "", "");

                                        Images_totalSave[index].RemoveAt(0);
                                        Images_totalSave_Information[index].RemoveAt(0);

                                        PopImageCount[index]++;

                                        Thread.Sleep(2);
                                    }
                                    else if (Global.ShiftInformation_totalSave.SavingFileFoamat.ToLower() == ".jpg") //
                                    {
                                        Images_totalSave[index].First().Bitmap.Save(memoryStream, ImageFormat.Jpeg);
                                        tcpClient2.SendImage(memoryStream, Images_totalSave_Information[index].First(), "", "");

                                        Images_totalSave[index].RemoveAt(0);
                                        Images_totalSave_Information[index].RemoveAt(0);

                                        PopImageCount[index]++;

                                        Thread.Sleep(2);
                                    }
                                    else if (Global.ShiftInformation_totalSave.SavingFileFoamat.ToLower() == ".png") //
                                    {
                                        Images_totalSave[index].First().Bitmap.Save(memoryStream, ImageFormat.Png);
                                        tcpClient2.SendImage(memoryStream, Images_totalSave_Information[index].First(), "", "");

                                        Images_totalSave[index].RemoveAt(0);
                                        Images_totalSave_Information[index].RemoveAt(0);

                                        PopImageCount[index]++;

                                        Thread.Sleep(2);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                    break;
                                case 2:
                                    if (Global.ShiftInformation_totalSave.SavingFileFoamat.ToLower() == ".bmp") //
                                    {
                                        Images_totalSave[index].First().Bitmap.Save(memoryStream, ImageFormat.Bmp);
                                        tcpClient3.SendImage(memoryStream, Images_totalSave_Information[index].First(), "", "");

                                        Images_totalSave[index].RemoveAt(0);
                                        Images_totalSave_Information[index].RemoveAt(0);

                                        PopImageCount[index]++;

                                        Thread.Sleep(2);
                                    }
                                    else if (Global.ShiftInformation_totalSave.SavingFileFoamat.ToLower() == ".jpg") //
                                    {
                                        Images_totalSave[index].First().Bitmap.Save(memoryStream, ImageFormat.Jpeg);
                                        tcpClient3.SendImage(memoryStream, Images_totalSave_Information[index].First(), "", "");

                                        Images_totalSave[index].RemoveAt(0);
                                        Images_totalSave_Information[index].RemoveAt(0);

                                        PopImageCount[index]++;

                                        Thread.Sleep(2);
                                    }
                                    else if (Global.ShiftInformation_totalSave.SavingFileFoamat.ToLower() == ".png") //
                                    {
                                        Images_totalSave[index].First().Bitmap.Save(memoryStream, ImageFormat.Png);
                                        tcpClient3.SendImage(memoryStream, Images_totalSave_Information[index].First(), "", "");

                                        Images_totalSave[index].RemoveAt(0);
                                        Images_totalSave_Information[index].RemoveAt(0);

                                        PopImageCount[index]++;

                                        Thread.Sleep(2);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                    break;
                                case 3:
                                    if (Global.ShiftInformation_totalSave.SavingFileFoamat.ToLower() == ".bmp") //
                                    {
                                        Images_totalSave[index].First().Bitmap.Save(memoryStream, ImageFormat.Bmp);
                                        tcpClient4.SendImage(memoryStream, Images_totalSave_Information[index].First(), "", "");

                                        Images_totalSave[index].RemoveAt(0);
                                        Images_totalSave_Information[index].RemoveAt(0);

                                        PopImageCount[index]++;

                                        Thread.Sleep(2);
                                    }
                                    else if (Global.ShiftInformation_totalSave.SavingFileFoamat.ToLower() == ".jpg") //
                                    {
                                        Images_totalSave[index].First().Bitmap.Save(memoryStream, ImageFormat.Jpeg);
                                        tcpClient4.SendImage(memoryStream, Images_totalSave_Information[index].First(), "", "");

                                        Images_totalSave[index].RemoveAt(0);
                                        Images_totalSave_Information[index].RemoveAt(0);

                                        PopImageCount[index]++;

                                        Thread.Sleep(2);
                                    }
                                    else if (Global.ShiftInformation_totalSave.SavingFileFoamat.ToLower() == ".png") //
                                    {
                                        Images_totalSave[index].First().Bitmap.Save(memoryStream, ImageFormat.Png);
                                        tcpClient4.SendImage(memoryStream, Images_totalSave_Information[index].First(), "", "");

                                        Images_totalSave[index].RemoveAt(0);
                                        Images_totalSave_Information[index].RemoveAt(0);

                                        PopImageCount[index]++;

                                        Thread.Sleep(2);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        if (Global.ShiftInformation_totalSave.ImageSavingRootExists)
                        {
                            if ((Images_totalSave[index].Count > 0) && (Images_totalSave_DateTime[index].Count > 0)) //图像信息满足要求，图像满足要求
                            {
                                Global.ShiftInformation_totalSave._UpdateCurrentImageInformation(Global.Camera[index].Type, Images_totalSave_DateTime[index].First(), Images_totalSave[index].First());
                                Images_totalSave[index].RemoveAt(0);
                                Images_totalSave_DateTime[index].RemoveAt(0);
                            }
                            else
                            {
                                break;
                            }
                            Thread.Sleep(2);//休眠2ms
                        }
                        else //保存路径不存在
                        {
                            break;
                        }
                    }
                }
                else //相机在执行删除
                {
                    break;
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明： 保存缺陷图像线程
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _StaticRejectImageSave(object parameter)
        {
            while (true)
            {
                Byte index = (Byte)parameter;

                Boolean bContainsKey = true;
                lock (LockCheck_CameraPort)
                {
                    bContainsKey = Global.Check_CameraPort.ContainsKey(Global.Camera[index].Type);
                }

                if (bContainsKey) //相机有效
                {
                    lock (LockStaticRejectImageSavePort[index])//锁定图像保存，防止删除图像读写冲突
                    {
                        StaticRejectImageSaveStatePort[index] = false;
                    }

                    if ((null != StaticRejectImages[index]) && (StaticRejectImages[index].Count > 0) && (null != StaticRejectsGraphicsInformations[index]) && (StaticRejectsGraphicsInformations[index].Count > 0)) //图像信息满足要求，图像满足要求
                    {
                        Global.ShiftInformation._UpdateCurrentImageInformation(Global.Camera[index].DeviceInformation.Port, Global.BrandName, StaticRejectsGraphicsInformations[index].First(), StaticRejectImages[index].First());
                        StaticRejectImages[index].RemoveAt(0);
                        StaticRejectsGraphicsInformations[index].RemoveAt(0);
                    }
                    else
                    {
                        lock (LockStaticRejectImageSavePort[index])//锁定图像保存，防止删除图像读写冲突
                        {
                            StaticRejectImageSaveStatePort[index] = true;
                        }
                        break;
                    }

                    lock (LockStaticRejectImageSavePort[index])//锁定图像保存，防止删除图像读写冲突
                    {
                        StaticRejectImageSaveStatePort[index] = true;
                    }
                    Thread.Sleep(2);//休眠2ms
                }
                else //相机在执行删除
                {
                    break;
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明： 保存关联图像线程
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _RelevancyImageSave(object parameter)
        {
            while (true)
            {
                Byte index = (Byte) parameter;

                Int32 i = 0;//循环变量

                for (i = 0; i < Global.Camera[index].RelevancyCameraInfo.RelevancyCameraInfo.Count; i++) //遍历所有关联相机
                {
                    Byte bCameraPort;
                    lock (LockCheck_CameraPort)
                    {
                        Global.Check_CameraPort.TryGetValue(Global.Camera[index].RelevancyCameraInfo.RelevancyCameraInfo.ElementAt(i).Key, out bCameraPort);
                    }

                    //if ((bCameraPort >= 1) && (bCameraPort <= 4) && (false == CameraLostState[bCameraPort - 1])) //查询到有效端口号，且相机无故障
                    if ((bCameraPort >= 1) && (bCameraPort <= 4) ) //查询到有效端口号
                    {
                        if ((Global.Camera[index].RelevancyCameraInfo.RelevancyCameraInfo.ElementAt(i).Value >= RelevancyImages[bCameraPort - 1].Count) && (Global.Camera[index].RelevancyCameraInfo.RelevancyCameraInfo.ElementAt(i).Value >= RelevancyImageInformations[bCameraPort - 1].Count)) //缓存图像未满足要求
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                //检查图像信息查看关联图像是否都完好
                if (i == Global.Camera[index].RelevancyCameraInfo.RelevancyCameraInfo.Count) //满足关联保存需求
                {
                    for (i = 0; i < Global.Camera[index].RelevancyCameraInfo.RelevancyCameraInfo.Count; i++) //遍历所有关联相机
                    {
                        Byte bCameraPort;
                        lock (LockCheck_CameraPort)
                        {
                            Global.Check_CameraPort.TryGetValue(Global.Camera[index].RelevancyCameraInfo.RelevancyCameraInfo.ElementAt(i).Key, out bCameraPort);
                        }

                        //if ((bCameraPort >= 1) && (bCameraPort <= 4) && (false == CameraLostState[bCameraPort - 1])) //查询到有效端口号
                        if ((bCameraPort >= 1) && (bCameraPort <= 4)) //查询到有效端口号
                        {
                            Int32 j = 0; //循环变量
                            Byte bValue;
                            Global.Camera[index].RelevancyCameraInfo.RelevancyCameraInfo.TryGetValue(Global.Camera[index].RelevancyCameraInfo.RelevancyCameraInfo.ElementAt(i).Key, out bValue);

                            for (j = 0; j < RelevancyImageInformations[bCameraPort - 1][bValue].Length; j++) //图像信息
                            {
                                if (VisionSystemClassLibrary.Enum.ImageType.Error == RelevancyImageInformations[bCameraPort - 1][bValue][j].Type) //图像为缺陷图像
                                {
                                    break;
                                }
                            }

                            if (j < RelevancyImageInformations[bCameraPort - 1][bValue].Length)
                            {
                                //执行缺陷保存
                                break;
                            }
                        }
                    }

                    if (i == Global.Camera[index].RelevancyCameraInfo.RelevancyCameraInfo.Count) //执行完好图像计数
                    {
                        lock (LockRelevancyImageSave)//锁定图像保存，防止删除图像读写冲突
                        {
                            RelevancyImageSaveState = false;
                        }

                        for (i = 0; i < Global.Camera[index].RelevancyCameraInfo.RelevancyCameraInfo.Count; i++) //遍历所有关联相机
                        {
                            Byte bCameraPort;
                            lock (LockCheck_CameraPort)
                            {
                                Global.Check_CameraPort.TryGetValue(Global.Camera[index].RelevancyCameraInfo.RelevancyCameraInfo.ElementAt(i).Key, out bCameraPort);
                            }

                            if ((bCameraPort >= 1) && (bCameraPort <= 4)) //查询到有效端口号
                            {
                                Byte bValue;
                                Global.Camera[index].RelevancyCameraInfo.RelevancyCameraInfo.TryGetValue(Global.Camera[index].RelevancyCameraInfo.RelevancyCameraInfo.ElementAt(i).Key, out bValue);

                                Global.ShiftInformation._UpdateCurrentImageInformation(Global.Camera[bCameraPort - 1].DeviceInformation.Port, Global.BrandName);

                                if ((RelevancyImageInformations[bCameraPort - 1].Count > bValue) && (RelevancyImages[bCameraPort - 1].Count > bValue)) //图像信息满足要求，图像满足要求
                                {
                                    RelevancyImages[bCameraPort - 1].RemoveAt(bValue);
                                    RelevancyImageInformations[bCameraPort - 1].RemoveAt(bValue);
                                }
                            }
                        }

                        lock (LockRelevancyImageSave)//锁定图像保存，防止删除图像读写冲突
                        {
                            RelevancyImageSaveState = true;
                        }
                    }
                    else
                    {
                        Boolean bSaveState = true;
                        Byte[] bValues = new Byte[Global.Camera[index].RelevancyCameraInfo.RelevancyCameraInfo.Count];
                        DateTime[] dateTimeTemp = new DateTime[Global.Camera[index].RelevancyCameraInfo.RelevancyCameraInfo.Count];
                        for (i = 0; i < Global.Camera[index].RelevancyCameraInfo.RelevancyCameraInfo.Count; i++) //遍历所有关联相机
                        {
                            Byte bCameraPort;
                            lock (LockCheck_CameraPort)
                            {
                                Global.Check_CameraPort.TryGetValue(Global.Camera[index].RelevancyCameraInfo.RelevancyCameraInfo.ElementAt(i).Key, out bCameraPort);
                            }

                            if ((bCameraPort >= 1) && (bCameraPort <= 4)) //查询到有效端口号
                            {
                                Byte bValue;
                                Global.Camera[index].RelevancyCameraInfo.RelevancyCameraInfo.TryGetValue(Global.Camera[index].RelevancyCameraInfo.RelevancyCameraInfo.ElementAt(i).Key, out bValue);

                                if (RelevancyImageInformations[bCameraPort - 1].Count > bValue) //图像信息满足要求，图像满足要求
                                {
                                    dateTimeTemp[i] = RelevancyImageInformations[bCameraPort - 1][bValue].First().DateTimeImage;
                                    bValues[i] = bValue;
                                }
                            }
                        }

                        for (i = 0; i < dateTimeTemp.Length - 1; i++) //循环所有缺陷图像信息
                        {
                            if (bValues[i + 1] < bValues[i]) //关联相机后一位
                            {
                                if (dateTimeTemp[i + 1].CompareTo(dateTimeTemp[i]) > 0) //产生图像时间有误
                                {
                                    bSaveState = false;
                                    break;
                                }
                            }
                            else if (bValues[i + 1] > bValues[i]) //关联相机前一位
                            {
                                if (dateTimeTemp[i + 1].CompareTo(dateTimeTemp[i]) < 0) //产生图像时间有误
                                {
                                    bSaveState = false;
                                    break;
                                }
                            }
                        }

                        if (bSaveState) //执行保存
                        {
                            lock (LockRelevancyImageSave)//锁定图像保存，防止删除图像读写冲突
                            {
                                RelevancyImageSaveState = false;
                            }

                            for (i = 0; i < Global.Camera[index].RelevancyCameraInfo.RelevancyCameraInfo.Count; i++) //遍历所有关联相机
                            {
                                Byte bCameraPort;
                                lock (LockCheck_CameraPort)
                                {
                                    Global.Check_CameraPort.TryGetValue(Global.Camera[index].RelevancyCameraInfo.RelevancyCameraInfo.ElementAt(i).Key, out bCameraPort);
                                }

                                if ((bCameraPort >= 1) && (bCameraPort <= 4)) //查询到有效端口号
                                {
                                    Byte bValue;
                                    Global.Camera[index].RelevancyCameraInfo.RelevancyCameraInfo.TryGetValue(Global.Camera[index].RelevancyCameraInfo.RelevancyCameraInfo.ElementAt(i).Key, out bValue);

                                    if ((RelevancyImageInformations[bCameraPort - 1].Count > bValue) && (RelevancyImages[bCameraPort - 1].Count > bValue)) //图像信息满足要求，图像满足要求
                                    {
                                        Global.ShiftInformation._UpdateCurrentImageInformation(Global.Camera[bCameraPort - 1].DeviceInformation.Port, Global.BrandName, RelevancyImageInformations[bCameraPort - 1][bValue], RelevancyImages[bCameraPort - 1][bValue]);
                                        RelevancyImages[bCameraPort - 1].RemoveAt(bValue);
                                        RelevancyImageInformations[bCameraPort - 1].RemoveAt(bValue);
                                    }

                                    Thread.Sleep(2);//休眠2ms
                                }
                            }

                            lock (LockRelevancyImageSave)//锁定图像保存，防止删除图像读写冲突
                            {
                                RelevancyImageSaveState = true;
                            }
                        } 
                        else //图片关联可能异常
                        {
                            lock (LockRelevancyImageSave)//锁定图像保存，防止删除图像读写冲突
                            {
                                RelevancyImageSaveState = false;
                            }

                            lock (LockCheck_CameraPort)
                            {
                                _ClearRelevancyImageBuffer();
                            }

                            lock (LockRelevancyImageSave)//锁定图像保存，防止删除图像读写冲突
                            {
                                RelevancyImageSaveState = true;
                            }
                        }
                    }
                }
                else //不满足保存需求
                {
                    break;
                }
            }
        }
        
        //-----------------------------------------------------------------------
        // 功能说明： 相机图像信息更新
        // 输入参数： 1、Byte：index，数组下标索引
        //                     2、Int32：inspectionTime，图像处理时间
        //                     3、Boolean：bResultPort，处理结果
        //                     4、Byte：iErrorIndexPort，第一个缺陷工具索引
        //                     5、Int32：iImageRingBufferIndex，处理图像索引
        //                     6、Boolean：bCheckTobaccoState，非空模盒标记:True为非空；FALSE为空
        //                     7、Boolean[]：bToolProcessingResultFlag，所有检测工具结果
        //                     8、DateTime：datetime图像工具索引
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _CameraImageImformationUpdate(Byte index, Boolean bResultPort, Byte bErrorIndexPort, Int32 iImageRingBufferIndex, Boolean bCheckTobaccoState, Boolean[] bToolProcessingResultFlag, DateTime datetime)
        {
            _UpdateSourseImageBuffInformation(index, iImageRingBufferIndex, bToolProcessingResultFlag, datetime);  //更新原始图像信息到缓冲区

            switch (Global.Camera[index].RelevancyCameraInfo.rRelevancyType) //相机关联类型
            {
                case VisionSystemClassLibrary.Enum.RelevancyType.None:
                    if ((false == LabModel) && bCheckTobaccoState) //当前处于非实验室界面，非空模盒
                    {
                        if (false == bResultPort) //当前图像存在缺陷
                        {
                            _UpdateRejectImageBuffInformation(index, iImageRingBufferIndex, bToolProcessingResultFlag, datetime);  //更新剔除图像信息到缓冲区
                        }

                        Boolean bContainsKey = true;
                        lock (LockCheck_CameraPort)
                        {
                            bContainsKey = Global.Check_CameraPort.ContainsKey(Global.Camera[index].Type);
                        }

                        if (bContainsKey && Global.ShiftInformation.ShiftState && (false == Global.StaticsPause)) //查询到有效端口号（反之，相机在执行删除），班次状态使能，统计未暂停
                        {
                            if (bResultPort) //当前图像完好
                            {
                                lock (LockStaticRejectImageSavePort[index])
                                {
                                    Global.ShiftInformation._UpdateCurrentImageInformation(Global.Camera[index].DeviceInformation.Port, Global.BrandName);
                                }
                            }
                            else //当前图像存在缺陷
                            {
                                Int32 iIndex = Global.Camera[index].Tools.Count;
                                for (Int32 i = 0; i < Global.Camera[index].Tools.Count; i++)
                                {
                                    if (Global.Camera[index].Tools[i].ToolState && (VisionSystemClassLibrary.Enum.ArithmeticType.Line == Global.Camera[index].Tools[i].Type)) //当前执行拉线检测
                                    {
                                        iIndex = i;
                                        break;
                                    }
                                }

                                Boolean bSaveState = true;
                                if ((iIndex < Global.Camera[index].Tools.Count) && (StaticRejectImageSaveConNumberPort[index] > StaticRejectImageSaveConNumberMax)) //当前执行拉线检测，且超过连续缺陷保存上限
                                {
                                    bSaveState = false;
                                }

                                if ((StaticRejectImageSaveConNumberPort[index] > 0) && bSaveState && ((null == StaticRejectImages[index]) || (StaticRejectImages[index].Count < Global.ImagesListCountMax)))//当前连续剔除图像在要求内，执行图像保存
                                {
                                    StaticRejectImages[index].Add(ImageSoursePort[index][iImageRingBufferIndex].Copy());

                                    VisionSystemClassLibrary.Struct.ImageInformation[] imageInformationTemp = new VisionSystemClassLibrary.Struct.ImageInformation[bToolProcessingResultFlag.Length];

                                    for (Int32 i = 0; i < bToolProcessingResultFlag.Length; i++) //循环所有工具信息
                                    {
                                        imageInformationTemp[i] = new VisionSystemClassLibrary.Struct.ImageInformation();
                                        _UpdateImageBuffGraphicsInformation(index, ref imageInformationTemp[i], bToolProcessingResultFlag[i], i, datetime);
                                    }
                                    StaticRejectsGraphicsInformations[index].Add(imageInformationTemp);//更新图像信息

                                    if (((StaticRejectImageSaveThread[index] == null) || (false == StaticRejectImageSaveThread[index].IsAlive)))//班次统计线程无效，或是线程未执行，重新启动
                                    {
                                        StaticRejectImageSaveThread[index] = new Thread(_StaticRejectImageSave);
                                        StaticRejectImageSaveThread[index].Priority = System.Threading.ThreadPriority.BelowNormal;
                                        StaticRejectImageSaveThread[index].IsBackground = true;
                                        StaticRejectImageSaveThread[index].Start(index);
                                    }
                                }
                            }
                        }

                        if (("" != Global.BrandName) && Global.DeepLearningSavingFlag[index]) 
                        {
                            DirectoryInfo dirInfo = new DirectoryInfo(".\\deeplearning\\");
                            if (false == dirInfo.Exists) //路径不存在
                            {
                                dirInfo.Create();
                            }

                            DirectoryInfo dirInfo1 = new DirectoryInfo(dirInfo.FullName + Global.BrandName + "\\");
                            if (false == dirInfo1.Exists) //路径不存在
                            {
                                dirInfo1.Create();
                            }

                            DirectoryInfo dirInfo2 = new DirectoryInfo(dirInfo1.FullName + index.ToString() + "\\");
                            if (false == dirInfo2.Exists) //路径不存在
                            {
                                dirInfo2.Create();
                            }

                            if (Global.DeepLearningImageCount[index] < Global.DeepLearningImageMax)//深度学习图像未存满
                            {
                                Global.DeepLearningImageCount[index]++;
                                //VisionSystemClassLibrary.GeneralFunction._SaveImage(ImageSoursePort[index][iImageRingBufferIndex], dirInfo2 + Global.DeepLearningImageCount[index].ToString() + VisionSystemClassLibrary.Class.Camera.JPGFile, VisionSystemClassLibrary.Class.Camera.JPGFile);
                                ImageSoursePort[index][iImageRingBufferIndex].Save(dirInfo2 + Global.DeepLearningImageCount[index].ToString() + VisionSystemClassLibrary.Class.Camera.BMPFile);
                            }
                        }

                        if (bContainsKey && Global.ShiftInformation_totalSave.ShiftState && ((null == Images_totalSave[index]) || (Images_totalSave[index].Count < Global.ImagesListCountMax))) //查询到有效端口号（反之，相机在执行删除）,当前磁盘存在
                        {
                            if (Global.ShiftInformation_totalSave.TransMode) //TCP/IP
                            {
                                Boolean bConncetState = false;
                                switch (index)
                                {
                                    case 0:
                                        if (tcpClient1.dllConnected)
                                        {
                                            bConncetState = true;
                                        }
                                        break;
                                    case 1:
                                        if (tcpClient2.dllConnected)
                                        {
                                            bConncetState = true;
                                        }
                                        break;
                                    case 2:
                                        if (tcpClient3.dllConnected)
                                        {
                                            bConncetState = true;
                                        }
                                        break;
                                    case 3:
                                        if (tcpClient4.dllConnected)
                                        {
                                            bConncetState = true;
                                        }
                                        break;
                                    default:
                                        break;
                                }

                                if (bConncetState && ("" != Global.BrandName)) //以太网连接正常
                                {
                                    Images_totalSave[index].Add(ImageSoursePort[index][iImageRingBufferIndex].Copy());

                                    Dictionary<string, string> dict = new Dictionary<string, string>();
                                    for (Int32 i = 0; i < Global.ShiftInformation_totalSave.RequestInfo[index].Count; i++) //循环所有键值对
                                    {
                                        string key = Global.ShiftInformation_totalSave.RequestInfo[index].ElementAt(i).Key;
                                        string value = Global.ShiftInformation_totalSave.RequestInfo[index].ElementAt(i).Value;

                                        if (false == dict.ContainsKey(key)) //未包含键
                                        {
                                            dict.Add(key, value);
                                        }
                                    }

                                    if (dict.ContainsKey("devicetype")) //包含键
                                    {
                                        dict["devicetype"] = Global.BrandName;
                                    }

                                    if (dict.ContainsKey("comppartno")) //包含键
                                    {
                                        dict["comppartno"] = ApplicationStartTimeStr + ImageCount[index].ToString("00000000");
                                    }

                                    if (dict.ContainsKey("filename")) //包含键
                                    {
                                        if (bResultPort) //检测结果完好
                                        {
                                            switch (Global.Camera[index].Type)
                                            {
                                                case VisionSystemClassLibrary.Enum.CameraType.Camera_1:
                                                    dict["filename"] = datetime.ToString("yyyyMMddHHmmssfff") + "_CAM1_OK" + Global.ShiftInformation_totalSave.SavingFileFoamat;
                                                    break;
                                                case VisionSystemClassLibrary.Enum.CameraType.Camera_2:
                                                    dict["filename"] = datetime.ToString("yyyyMMddHHmmssfff") + "_CAM2_OK" + Global.ShiftInformation_totalSave.SavingFileFoamat;
                                                    break;
                                                case VisionSystemClassLibrary.Enum.CameraType.Camera_3:
                                                    dict["filename"] = datetime.ToString("yyyyMMddHHmmssfff") + "_CAM3_OK" + Global.ShiftInformation_totalSave.SavingFileFoamat;
                                                    break;
                                                case VisionSystemClassLibrary.Enum.CameraType.Camera_4:
                                                    dict["filename"] = datetime.ToString("yyyyMMddHHmmssfff") + "_CAM4_OK" + Global.ShiftInformation_totalSave.SavingFileFoamat;
                                                    break;
                                                default: 
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            switch (Global.Camera[index].Type)
                                            {
                                                case VisionSystemClassLibrary.Enum.CameraType.Camera_1:
                                                    dict["filename"] = datetime.ToString("yyyyMMddHHmmssfff") + "_CAM1_NG" + Global.ShiftInformation_totalSave.SavingFileFoamat;
                                                    break;
                                                case VisionSystemClassLibrary.Enum.CameraType.Camera_2:
                                                    dict["filename"] = datetime.ToString("yyyyMMddHHmmssfff") + "_CAM2_NG" + Global.ShiftInformation_totalSave.SavingFileFoamat;
                                                    break;
                                                case VisionSystemClassLibrary.Enum.CameraType.Camera_3:
                                                    dict["filename"] = datetime.ToString("yyyyMMddHHmmssfff") + "_CAM3_NG" + Global.ShiftInformation_totalSave.SavingFileFoamat;
                                                    break;
                                                case VisionSystemClassLibrary.Enum.CameraType.Camera_4:
                                                    dict["filename"] = datetime.ToString("yyyyMMddHHmmssfff") + "_CAM4_NG" + Global.ShiftInformation_totalSave.SavingFileFoamat;
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }
                                    }

                                    if (dict.ContainsKey("requestid")) //包含键
                                    {
                                        switch (Global.Camera[index].Type)
                                        {
                                            case VisionSystemClassLibrary.Enum.CameraType.Camera_1:
                                                dict["requestid"] = datetime.ToString("yyyyMMddHHmmssfff") + "_CAM1";
                                                break;
                                            case VisionSystemClassLibrary.Enum.CameraType.Camera_2:
                                                dict["requestid"] = datetime.ToString("yyyyMMddHHmmssfff") + "_CAM2";
                                                break;
                                            case VisionSystemClassLibrary.Enum.CameraType.Camera_3:
                                                dict["requestid"] = datetime.ToString("yyyyMMddHHmmssfff") + "_CAM3";
                                                break;
                                            case VisionSystemClassLibrary.Enum.CameraType.Camera_4:
                                                dict["requestid"] = datetime.ToString("yyyyMMddHHmmssfff") + "_CAM4";
                                                break;
                                            default:
                                                break;
                                        }
                                    }

                                    if (dict.ContainsKey("timestamp")) //包含键
                                    {
                                        dict["timestamp"] = datetime.ToString("yyyyMMddHHmmssfff") ;
                                    }

                                    if (dict.ContainsKey("fileSuffix")) //包含键
                                    {
                                        dict["fileSuffix"] = Global.ShiftInformation_totalSave.SavingFileFoamat;
                                    }

                                    Images_totalSave_Information[index].Add(dict);
                                    PushImageCount[index]++;

                                    if (((Images_totalSaveThread[index] == null) || (false == Images_totalSaveThread[index].IsAlive)))//班次统计线程无效，或是线程未执行，重新启动
                                    {
                                        Images_totalSaveThread[index] = new Thread(_Images_totalSave);
                                        Images_totalSaveThread[index].Priority = System.Threading.ThreadPriority.BelowNormal;
                                        Images_totalSaveThread[index].IsBackground = true;
                                        Images_totalSaveThread[index].Start(index);
                                    }
                                }
                            }
                            else
                            {
                                if(Global.ShiftInformation_totalSave.ImageSavingRootExists)
                                {
                                    Images_totalSave[index].Add(ImageSoursePort[index][iImageRingBufferIndex].Copy());
                                    Images_totalSave_DateTime[index].Add(datetime);

                                    if (((Images_totalSaveThread[index] == null) || (false == Images_totalSaveThread[index].IsAlive)))//班次统计线程无效，或是线程未执行，重新启动
                                    {
                                        Images_totalSaveThread[index] = new Thread(_Images_totalSave);
                                        Images_totalSaveThread[index].Priority = System.Threading.ThreadPriority.BelowNormal;
                                        Images_totalSaveThread[index].IsBackground = true;
                                        Images_totalSaveThread[index].Start(index);
                                    }
                                }
                            }
                        }
                    }
                    break;
                case VisionSystemClassLibrary.Enum.RelevancyType.Inner:
                    if (false == bResultPort)//当前图像缺陷
                    {
                        _UpdateRejectImageBuffInformation(index, iImageRingBufferIndex, bToolProcessingResultFlag, datetime);  //更新剔除图像信息到缓冲区
                    }

                    if ((false == LabModel) && Global.ShiftInformation.ShiftState && (false == Global.StaticsPause)) //当前处于非实验室界面，且班次使能，统计未暂停
                    {
                        Boolean bRelevancyCameState = true;
                        for (Int32 i = 0; i < Global.Camera[index].RelevancyCameraInfo.RelevancyCameraInfo.Count; i++) //遍历所有关联相机
                        {
                            Byte bCameraPort;
                            lock (LockCheck_CameraPort)
                            {
                                Global.Check_CameraPort.TryGetValue(Global.Camera[index].RelevancyCameraInfo.RelevancyCameraInfo.ElementAt(i).Key, out bCameraPort);
                            }

                            UInt64 temp = 0;

                            switch (bCameraPort - 1)
                            {
                                case 0:
                                    temp = 0x02;
                                    break;
                                case 1:
                                    temp = 0x04;
                                    break;
                                case 2:
                                    temp = 0x010000000000;
                                    break;
                                case 3:
                                    temp = 0x020000000000;
                                    break;
                                default:
                                    break;
                            }

                            if ((bCameraPort == 0) || ((Global.MachineFaultState & temp) != 0)) //未查询到有效端口号，或相机故障
                            {
                                bRelevancyCameState = false;
                                break;
                            }
                        }

                        if (RelevancyImageSaveState && bRelevancyCameState && ((null == RelevancyImages[index]) || (RelevancyImages[index].Count < Global.ImagesListCountMax))) //当前执行图像保存
                        {
                            Boolean bTrigger = true;

                            //for (Int32 j = 0; j < Global.Camera[index].RelevancyCameraInfo.RelevancyCameraInfo.Count; j++) //遍历当前关联相机之前所有相机
                            //{
                            //    Global.Camera[index].RelevancyCameraInfo.RelevancyCameraInfo.Values
                            //}

                            if (Global.Camera[index].Type != Global.Camera[index].RelevancyCameraInfo.RelevancyCameraInfo.Keys.First()) //非首个关联相机
                            {
                                for (Int32 j = 0; j < Global.Camera[index].RelevancyCameraInfo.RelevancyCameraInfo.Count; j++) //遍历当前关联相机之前所有相机
                                {
                                    if (Global.Camera[index].Type == Global.Camera[index].RelevancyCameraInfo.RelevancyCameraInfo.ElementAt(j).Key) //循环到当前相机，直接退出
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        if ((null == RelevancyImages[j]) || (0 == RelevancyImages[j].Count)) //其他相机未出发，可能触发起点异常
                                        {
                                            bTrigger = false;
                                            break;
                                        }
                                    }
                                }
                            }

                            if (bTrigger) //触发顺序正常
                            {
                                RelevancyImages[index].Add(ImageSoursePort[index][iImageRingBufferIndex].Copy());

                                VisionSystemClassLibrary.Struct.ImageInformation[] imageInformationTemp = new VisionSystemClassLibrary.Struct.ImageInformation[bToolProcessingResultFlag.Length];

                                for (Int32 i = 0; i < bToolProcessingResultFlag.Length; i++) //循环所有工具信息
                                {
                                    imageInformationTemp[i] = new VisionSystemClassLibrary.Struct.ImageInformation();
                                    _UpdateImageBuffGraphicsInformation(index, ref imageInformationTemp[i], bToolProcessingResultFlag[i], i, datetime);
                                }
                                RelevancyImageInformations[index].Add(imageInformationTemp);//更新图像信息

                                if (((RelevancyImageSaveThread == null) || (false == RelevancyImageSaveThread.IsAlive)))//线程无效，或是线程未执行，重新启动
                                {
                                    RelevancyImageSaveThread = new Thread(_RelevancyImageSave);
                                    RelevancyImageSaveThread.Priority = System.Threading.ThreadPriority.BelowNormal;
                                    RelevancyImageSaveThread.IsBackground = true;
                                    RelevancyImageSaveThread.Start(index);
                                }
                            }
                        }
                    }
                    break;
                case VisionSystemClassLibrary.Enum.RelevancyType.Extra:
                    if (false == bResultPort)//当前图像缺陷
                    {
                        _UpdateRejectImageBuffInformation(index, iImageRingBufferIndex, bToolProcessingResultFlag, datetime);  //更新剔除图像信息到缓冲区
                    }                   
                    break;
                default:
                    break;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明： 保存故障信息
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _MachineFaultSave()
        {
            for (Byte i = 0; i < Global.CameraNumberMax; i++)//遍历当前所有相机
            {
                if ((Global.CameraChooseState & (0x01 << i)) != 0)//当前相机开启
                {
                    lock (Global.LockMachineFaultState)//锁定故障信息保存，防止读写冲突
                    {
                        Global.Camera[i]._WriteFaultStaticsFile(ref Global.MachineFaultState, ref Global.MachineFaultSaveState, VisionSystemClassLibrary.Class.System.MachineFaultEnableState);
                    }
                }
            }
        }
        
        //-----------------------------------------------------------------------
        // 功能说明： 公差页面操作
        // 输入参数： 1、Byte:index，数组下标索引
        //            2、Byte：EnterTolerance，进入或退出公差界面标记
        //            3、Byte：bToleranceSave，退出公差界面是否保存标记
        // 输出参数： 无
        // 返 回 值： 1.Int32，公差界面操作执行结果
        //----------------------------------------------------------------------
        private Int32 _ToleranceOperate(Byte index, Byte EnterTolerance, Boolean bToleranceSave = false)
        {
            Int32 flag = 0;
            switch (EnterTolerance)
            {
                case 1://进入公差界面
                    Global.Camera[index]._CopyTo(ref Global.CameraTemp[index]);
                    flag = 1;
                    break;
                case 2://退出公差界面
                    if (bToleranceSave)//执行公差数据保存
                    {
                        Global.Camera[index]._CopyTo(ref Global.CameraTemp[index]);

                        Global.CameraTemp[index]._SaveTolerances();
                        Global.CameraTemp[index]._SaveTool();
                        flag = 1;
                    }
                    else
                    {
                        Global.CameraTemp[index]._CopyTo(ref Global.Camera[index]);
                        flag = 0;
                    }
                    break;
            }
            EnterTolerance = 0;
            return flag;
        }

        //-----------------------------------------------------------------------
        // 功能说明： 质量检测页面操作
        // 输入参数： 1、Byte:index，数组下标索引
        //            2、Byte：EnterQualityCheck，进入或退出质量检测界面标记
        //            3、Byte：bEnterQualityCheck，退出质量检测是否保存标记
        // 输出参数： 无
        // 返 回 值： 1.Int32，质量检测界面操作执行结果
        //----------------------------------------------------------------------
        private Int32 _QualityCheckOperate(Byte index, Byte EnterQualityCheck, Boolean bEnterQualityCheck = false)
        {
            Int32 flag = 0;
            switch (EnterQualityCheck)
            {
                case 1://进入质量检测界面
                    Global.Camera[index]._CopyTo(ref Global.CameraTemp[index]);
                    flag = 1;
                    break;
                case 2://退出质量检测界面
                    if (bEnterQualityCheck)
                    {
                        Global.CameraTemp[index]._CopyTo(ref Global.Camera[index]);

                        Global.Camera[index]._WriteImage(VisionSystemClassLibrary.Enum.ImageInformationType.Sample);
                        Global.Camera[index]._SaveTool();
                        Global.Camera[index]._SaveTolerances();
                        flag = 1;
                    }
                    else
                    {
                        Global.Camera[index]._CopyTo(ref Global.CameraTemp[index]);
                        flag = 0;
                    }
                    break;
            }
            EnterQualityCheck = 0;
            return flag;
        }

        //-----------------------------------------------------------------------
        // 功能说明： 相机配置页面操作
        // 输入参数： 1、Byte:index，数组下标索引
        //            2、Byte：EnterConfigImageCheck，进入或退出相机配置界面标记
        //            3、Byte：bConfigImageSave，退出相机配置界面是否保存标记
        // 输出参数： 无
        // 返 回 值： 1.Int32，相机配置界面操作执行结果
        //----------------------------------------------------------------------
        private Int32 _ConfigImageOperate(Byte index, Byte EnterConfigImageCheck, Boolean bConfigImageSave = false)
        {
            Int32 flag = 0;
            switch (EnterConfigImageCheck)
            {
                case 1://进入相机配置界面
                    Global.Camera[index]._CopyTo(ref Global.CameraTemp[index]);
                    flag = 1;
                    break;
                case 2://退出相机配置界面
                    if (bConfigImageSave)
                    {
                        Global.Camera[index]._CopyTo(ref Global.CameraTemp[index]);

                        Global.CameraTemp[index]._SaveParameter();
                        flag = 1;
                    }
                    else
                    {
                        Global.CameraTemp[index]._CopyTo(ref Global.Camera[index]);

                        if (false == Global.Camera[index].IsSerialPort) //当前为相机
                        {
                            _UpdateCameraParameter(index);

                            if (false == Global.ComputerRunState) //控制器上运行软件
                            {
                                lock (LockControllerSerialPort)
                                {
                                    _SendCommand(5, index);
                                }
                            }

                            if (Global.Camera[index].DeviceParameter.WhiteBalance == 0) //白平衡选择自动模式
                            {
                                Thread.Sleep(1500);
                            }
                        }

                        flag = 0;
                    }
                    break;
            }
            EnterConfigImageCheck = 0;
            return flag;
        }

        //-----------------------------------------------------------------------
        // 功能说明：相机配置参数拷贝
        // 输入参数： 1、Byte:index，数组下标索引
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _CameraFileCopy(Byte index)
        {
            //VisionSystemClassLibrary.Class.System.FileCopyFun(Global.Camera[index].ReceivedDataPath + VisionSystemClassLibrary.Class.Camera.TolerancesFileName, Global.Camera[index].DataPath + VisionSystemClassLibrary.Class.Camera.TolerancesFileName);
            //VisionSystemClassLibrary.Class.System.FileCopyFun(Global.Camera[index].ReceivedDataPath + VisionSystemClassLibrary.Class.Camera.ToolFileName, Global.Camera[index].DataPath + VisionSystemClassLibrary.Class.Camera.ToolFileName);
            //VisionSystemClassLibrary.Class.System.FileCopyFun(Global.Camera[index].ReceivedDataPath + VisionSystemClassLibrary.Class.Camera.ParameterFileName, Global.Camera[index].DataPath + VisionSystemClassLibrary.Class.Camera.ParameterFileName);
            //VisionSystemClassLibrary.Class.System.FileCopyFun(Global.Camera[index].ReceivedDataPath + VisionSystemClassLibrary.Class.Camera.SampleDataFileName, Global.Camera[index].SampleImagePath + VisionSystemClassLibrary.Class.Camera.SampleDataFileName);
            //VisionSystemClassLibrary.Class.System.FileCopyFun(Global.Camera[index].ReceivedDataPath + VisionSystemClassLibrary.Class.Camera.SampleImageFileName + VisionSystemClassLibrary.Class.Camera.BMPFile, Global.Camera[index].SampleImagePath + VisionSystemClassLibrary.Class.Camera.SampleImageFileName + VisionSystemClassLibrary.Class.Camera.BMPFile);

            if(File.Exists(Global.Camera[index].ReceivedDataPath + VisionSystemClassLibrary.Class.Camera.TolerancesFileName))//文件存在
            {
                File.Copy(Global.Camera[index].ReceivedDataPath + VisionSystemClassLibrary.Class.Camera.TolerancesFileName, Global.Camera[index].DataPath + VisionSystemClassLibrary.Class.Camera.TolerancesFileName, true);
            }

            if (File.Exists(Global.Camera[index].ReceivedDataPath + VisionSystemClassLibrary.Class.Camera.ToolFileName))//文件存在
            {
                File.Copy(Global.Camera[index].ReceivedDataPath + VisionSystemClassLibrary.Class.Camera.ToolFileName, Global.Camera[index].DataPath + VisionSystemClassLibrary.Class.Camera.ToolFileName, true);
            }

            if (File.Exists(Global.Camera[index].ReceivedDataPath + VisionSystemClassLibrary.Class.Camera.ParameterFileName))//文件存在
            {
                File.Copy(Global.Camera[index].ReceivedDataPath + VisionSystemClassLibrary.Class.Camera.ParameterFileName, Global.Camera[index].DataPath + VisionSystemClassLibrary.Class.Camera.ParameterFileName, true);
            }

            if (File.Exists(Global.Camera[index].ReceivedDataPath + VisionSystemClassLibrary.Class.Camera.SampleDataFileName))//文件存在
            {
                File.Copy(Global.Camera[index].ReceivedDataPath + VisionSystemClassLibrary.Class.Camera.SampleDataFileName, Global.Camera[index].SampleImagePath + VisionSystemClassLibrary.Class.Camera.SampleDataFileName, true);
            }

            if (File.Exists(Global.Camera[index].ReceivedDataPath + VisionSystemClassLibrary.Class.Camera.SampleImageFileName + VisionSystemClassLibrary.Class.Camera.BMPFile))//文件存在
            {
                File.Copy(Global.Camera[index].ReceivedDataPath + VisionSystemClassLibrary.Class.Camera.SampleImageFileName + VisionSystemClassLibrary.Class.Camera.BMPFile, Global.Camera[index].SampleImagePath + VisionSystemClassLibrary.Class.Camera.SampleImageFileName + VisionSystemClassLibrary.Class.Camera.BMPFile, true);
            }

            if (File.Exists(Global.Camera[index].ReceivedDataPath + VisionSystemClassLibrary.Class.Camera.ClassesFile))//文件存在
            {
                File.Copy(Global.Camera[index].ReceivedDataPath + VisionSystemClassLibrary.Class.Camera.ClassesFile, Global.Camera[index].DataPath + VisionSystemClassLibrary.Class.Camera.ClassesFile, true);
            }

            if (File.Exists(Global.Camera[index].ReceivedDataPath + VisionSystemClassLibrary.Class.Camera.ModelFileName))//文件存在
            {
                File.Copy(Global.Camera[index].ReceivedDataPath + VisionSystemClassLibrary.Class.Camera.ModelFileName, Global.Camera[index].DataPath + VisionSystemClassLibrary.Class.Camera.ModelFileName, true);
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：更新相机参数
        // 输入参数： 1、Byte：index，数组下标索引
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _UpdateCameraParameter(Byte index)
        {
            try                                                                //启动相机
            {
                if (icImagingControlPort[index].DeviceValid)                         //端口1相机正常工作
                {
                    if (Global.Camera[index].DeviceParameter.ExposureTime > Global.ExposureTime.Length)
                    {
                        ExposureValuePort[index].Value =
                            Global.ExposureTime[Global.ExposureTime.Length - 1];//设置曝光时间
                    }
                    else if (Global.Camera[index].DeviceParameter.ExposureTime < 1)
                    {
                        ExposureValuePort[index].Value =
                            Global.ExposureTime[0];//设置曝光时间
                    }
                    else
                    {
                        ExposureValuePort[index].Value =
                            Global.ExposureTime[Global.Camera[index].DeviceParameter.ExposureTime - 1];//设置曝光时间
                    }

                    if (Global.Camera[index].VideoFormat.StartsWith("RGB32")) //相机配置为彩色模式
                    {
                        if (Global.Camera[index].DeviceParameter.WhiteBalance == 0)//设置白平衡状态自动
                        {
                            WhiteBalanceRedPort[index].Value = 0;
                            WhiteBalanceGreenPort[index].Value = 0;
                            WhiteBalanceBluePort[index].Value = 0;

                            WhiteBalanceItemAutoPort[index].Switch = true;
                        }
                        else                                                //设置白平衡状态手动
                        {
                            WhiteBalanceRedPort[index].Value = Global.Camera[index].DeviceParameter.WhiteBalance_Red;
                            WhiteBalanceGreenPort[index].Value = Global.Camera[index].DeviceParameter.WhiteBalance_Green;
                            WhiteBalanceBluePort[index].Value = Global.Camera[index].DeviceParameter.WhiteBalance_Blue;

                            WhiteBalanceItemAutoPort[index].Switch = false;
                        }
                    }
                    GainValuePort[index].Value = Global.Camera[index].DeviceParameter.Gain;
                }
            }
            catch (Exception ex)                                               //启动相机异常
            {
                if (Global.ShowInformation)//显示调试信息
                {
                    switch (index)
                    {
                        case 0:
                            label5.Invoke(new EventHandler(delegate { label5.Text = "相机1配置异常"; }));
                            break;
                        case 1:
                            label2.Invoke(new EventHandler(delegate { label2.Text = "相机2配置异常"; }));
                            break;
                        case 2:
                            label13.Invoke(new EventHandler(delegate { label13.Text = "相机3配置异常"; }));
                            break;
                        case 3:
                            label15.Invoke(new EventHandler(delegate { label15.Text = "相机4配置异常"; }));
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明： 复位公差页面曲线图
        // 输入参数： 1、Byte:index，数组下标索引
        // 输出参数： 无
        // 返 回 值： 1、Int32，公差页面曲线图复位结果
        //----------------------------------------------------------------------
        private Int32 _GraphsReset(Byte index)
        {
            for (int i = 0; i < Global.Camera[index].Tolerances.GraphData.Count; i++)//遍历所有公差曲线图
            {
                for (int j = 0; j < Global.Camera[index].Tolerances.GraphData[i].ValueNumber; j++)//遍历所有公差曲线图数据
                {
                    Global.Camera[index].Tolerances.GraphData[i].TolerancesGraphDataValue.Value[j] = 0;//公差曲线图数据清零
                    Global.CameraTemp[index].Tolerances.GraphData[i].TolerancesGraphDataValue.Value[j] = 0;//公差曲线图数据清零
                }
                Global.Camera[index].Tolerances.GraphData[i].TolerancesGraphDataValue.CurrentValueIndex = -1;//公差曲线图索引清零
                Global.CameraTemp[index].Tolerances.GraphData[i].TolerancesGraphDataValue.CurrentValueIndex = -1;//公差曲线图索引清零
            }
            return 1;
        }

        //-----------------------------------------------------------------------
        // 功能说明：端口1相机出现故障调用函数
        // 输入参数： 1、object：sender，icImagingControl控件对象
        //          2、ICImagingControl.DeviceLostEventArgs：e，icImagingControl控件事件响应参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _icImagingControlPort1_DeviceLost(object sender, ICImagingControl.DeviceLostEventArgs e)
        {
            lock (Global.LockMachineFaultState)
            {
                Global.MachineFaultState |= 0x02;
            }

            Global.Camera[0].DeviceInformation.CAM = VisionSystemClassLibrary.Enum.CameraState.OFF;
            Global.CameraTemp[0].DeviceInformation.CAM = Global.Camera[0].DeviceInformation.CAM;

            lock (LockCheck_CameraPort)
            {
                if (VisionSystemClassLibrary.Enum.RelevancyType.None < Global.Camera[0].RelevancyCameraInfo.rRelevancyType) //相机含有关联信息
                {
                    if (Global.Check_CameraPort.ContainsKey(Global.Camera[0].Type)) //包含相机
                    {
                        Global.Check_CameraPort.Remove(Global.Camera[0].Type);
                    }
                    _ClearRelevancyImageBuffer();
                }
            }

            lock (LockControllerSerialPort)//相机下电
            {
                _CameraPowerOff(0);                                         //端口1相机下电
            }

            PowerStatePort[0] = 10;
            LostCount[0]++;

            if (Global.ShowInformation)//显示调试信息
            {
                label5.Invoke(new EventHandler(delegate { label5.Text = "相机1故障."; }));
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：端口2相机出现故障调用函数
        // 输入参数： 1、object：sender，icImagingControl控件对象
        //          2、ICImagingControl.DeviceLostEventArgs：e，icImagingControl控件事件响应参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _icImagingControlPort2_DeviceLost(object sender, ICImagingControl.DeviceLostEventArgs e)
        {
            lock (Global.LockMachineFaultState)
            {
                Global.MachineFaultState |= 0x04;
            }

            Global.Camera[1].DeviceInformation.CAM = VisionSystemClassLibrary.Enum.CameraState.OFF;
            Global.CameraTemp[1].DeviceInformation.CAM = Global.Camera[1].DeviceInformation.CAM;

            lock (LockCheck_CameraPort)
            {
                if (VisionSystemClassLibrary.Enum.RelevancyType.None < Global.Camera[1].RelevancyCameraInfo.rRelevancyType) //相机含有关联信息
                {
                    if (Global.Check_CameraPort.ContainsKey(Global.Camera[1].Type)) //包含相机
                    {
                        Global.Check_CameraPort.Remove(Global.Camera[1].Type);
                    }
                    _ClearRelevancyImageBuffer();
                }
            }

            lock (LockControllerSerialPort)//相机下电
            {
                _CameraPowerOff(1);                                         //端口2相机下电
            }

            PowerStatePort[1] = 10;
            LostCount[1]++;

            if (Global.ShowInformation)//显示调试信息
            {
                label2.Invoke(new EventHandler(delegate { label2.Text = "相机2故障."; }));
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：端口3相机出现故障调用函数
        // 输入参数： 1、object：sender，icImagingControl控件对象
        //          2、ICImagingControl.DeviceLostEventArgs：e，icImagingControl控件事件响应参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _icImagingControlPort3_DeviceLost(object sender, ICImagingControl.DeviceLostEventArgs e)
        {
            lock (Global.LockMachineFaultState)
            {
                Global.MachineFaultState |= 0x010000000000;
            }

            Global.Camera[2].DeviceInformation.CAM = VisionSystemClassLibrary.Enum.CameraState.OFF;
            Global.CameraTemp[2].DeviceInformation.CAM = Global.Camera[2].DeviceInformation.CAM;

            lock (LockCheck_CameraPort)
            {
                if (VisionSystemClassLibrary.Enum.RelevancyType.None < Global.Camera[2].RelevancyCameraInfo.rRelevancyType) //相机含有关联信息
                {
                    if (Global.Check_CameraPort.ContainsKey(Global.Camera[2].Type)) //包含相机
                    {
                        Global.Check_CameraPort.Remove(Global.Camera[2].Type);
                    }
                    _ClearRelevancyImageBuffer();
                }
            }

            lock (LockControllerSerialPort)//相机下电
            {
                _CameraPowerOff(3);                                         //端口3相机下电
            }

            PowerStatePort[2] = 10;
            LostCount[2]++;

            if (Global.ShowInformation)//显示调试信息
            {
                label13.Invoke(new EventHandler(delegate { label13.Text = "相机3故障."; }));
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：端口4相机出现故障调用函数
        // 输入参数： 1、object：sender，icImagingControl控件对象
        //          2、ICImagingControl.DeviceLostEventArgs：e，icImagingControl控件事件响应参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _icImagingControlPort4_DeviceLost(object sender, ICImagingControl.DeviceLostEventArgs e)
        {
            lock (Global.LockMachineFaultState)
            {
                Global.MachineFaultState |= 0x020000000000;
            }

            Global.Camera[3].DeviceInformation.CAM = VisionSystemClassLibrary.Enum.CameraState.OFF;
            Global.CameraTemp[3].DeviceInformation.CAM = Global.Camera[3].DeviceInformation.CAM;

            lock (LockCheck_CameraPort)
            {
                if (VisionSystemClassLibrary.Enum.RelevancyType.None < Global.Camera[3].RelevancyCameraInfo.rRelevancyType) //相机含有关联信息
                {
                    if (Global.Check_CameraPort.ContainsKey(Global.Camera[3].Type)) //包含相机
                    {
                        Global.Check_CameraPort.Remove(Global.Camera[3].Type);
                    }
                    _ClearRelevancyImageBuffer();
                }
            }

            lock (LockControllerSerialPort)//相机下电
            {
                _CameraPowerOff(4);                                         //端口4相机下电
            }

            PowerStatePort[3] = 10;
            LostCount[3]++;

            if (Global.ShowInformation)//显示调试信息
            {
                label15.Invoke(new EventHandler(delegate { label15.Text = "相机4故障."; }));
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

            try
            {
                ControllerSerialPortCommucation.Write(Command, 0, 4);
            }
            catch (System.Exception ex)
            {
                if (Global.ShowInformation)//显示调试信息
                {
                    label6.Text = "串口异常CameraPowerOn";
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：发送相机下电命令
        // 输入参数： 1、Byte：cameraChooseState，相机开启状态标记
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
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

            try
            {
                ControllerSerialPortCommucation.Write(Command, 0, 4);
            }
            catch (System.Exception ex)
            {
                if (Global.ShowInformation)//显示调试信息
                {
                    label6.Text = "串口异常CameraPowerOff";
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：timerPower响应函数,当相机丢失时重新启动相机
        // 输入参数： 1、object：sender，timer控件对象
        //          2、EventArgs：e，timer控件事件响应参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void timerPower_Tick(object sender, EventArgs e)
        {
            timerPower.Enabled = false;

            for (Byte j = 0; j < Global.CameraNumberMax; j++)//初始化相机类
            {
                if ((Global.CameraChooseState & (0x01 << j)) != 0)//当前相机开启
                {
                    UInt64 temp = 0;

                    switch (j)
                    {
                        case 0:
                            temp = 0x02;
                            break;
                        case 1:
                            temp = 0x04;
                            break;
                        case 2:
                            temp = 0x010000000000;
                            break;
                        case 3:
                            temp = 0x020000000000;
                            break;
                        default:
                            break;
                    }

                    if (((Global.MachineFaultStateTemp & temp) == 0) && ((Global.MachineFaultState & temp) != 0))  //相机存在故障
                    {
                        if (false == Global.Camera[j].IsSerialPort) //当前为相机，进行初始化
                        {
                            #region
                            icImagingControlBuff.Dispose();
                            icImagingControlBuff = new ICImagingControl();

                            if (icImagingControlBuff.Devices.Length > 0)                     //检测到相机
                            {
                                for (Int32 k = 0; k < icImagingControlBuff.Devices.Length; k++)    //遍历所有相机
                                {
                                    String SnBuff = "";
                                    try
                                    {
                                        icImagingControlBuff.Devices[k].GetSerialNumber(out SnBuff);
                                    }
                                    catch (SystemException ex) //相机数量发生异常，推出重新查询
                                    {
                                        break;
                                    }

                                    #region
                                    if (SnBuff == Global.SnPort[j])  //识别到端口相机
                                    {
                                        if (icImagingControlPort[j].DeviceValid) //相机正常工作，则停用
                                        {
                                            icImagingControlPort[j].LiveStop();
                                        }

                                        icImagingControlPort[j].Dispose();
                                        icImagingControlPort[j] = new ICImagingControl();       //动态创建相机控件

                                        try
                                        {
                                            icImagingControlPort[j].Device = icImagingControlBuff.Devices[k].Name;
                                            Global.CameraPort[j] = icImagingControlPort[j].Device;

                                            icImagingControlPort[j].Parent = this;
                                            icImagingControlPort[j].DeviceLostExecutionMode = TIS.Imaging.EventExecutionMode.AsyncInvoke;
                                            icImagingControlPort[j].ImageAvailableExecutionMode = TIS.Imaging.EventExecutionMode.MultiThreaded;
                                            icImagingControlPort[j].Visible = false;

                                            FrameFilter RotateFlipFilter = icImagingControlPort[j].FrameFilterCreate("Rotate Flip", "");

                                            if (RotateFlipFilter == null)
                                            {
                                                if (Global.ShowInformation)//显示调试信息
                                                {
                                                    switch (j)
                                                    {
                                                        case 0:
                                                            label5.Invoke(new EventHandler(delegate { label5.Text = "相机1旋转测试失败."; }));
                                                            break;
                                                        case 1:
                                                            label2.Invoke(new EventHandler(delegate { label2.Text = "相机2旋转测试失败."; }));
                                                            break;
                                                        case 2:
                                                            label13.Invoke(new EventHandler(delegate { label13.Text = "相机3旋转测试失败."; }));
                                                            break;
                                                        case 3:
                                                            label15.Invoke(new EventHandler(delegate { label15.Text = "相机4旋转测试失败."; }));
                                                            break;
                                                        default:
                                                            break;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                icImagingControlPort[j].DeviceFrameFilters.Add(RotateFlipFilter);
                                            }

                                            if (!icImagingControlPort[j].LiveVideoRunning)
                                            {
                                                switch (Global.Camera[j].CameraFlip)
                                                {
                                                    case VisionSystemClassLibrary.Enum.CameraFlip.Flip_H:
                                                        RotateFlipFilter.SetBoolParameter("Flip H", true);
                                                        break;
                                                    case VisionSystemClassLibrary.Enum.CameraFlip.Flip_V:
                                                        RotateFlipFilter.SetBoolParameter("Flip V", true);
                                                        break;
                                                    default:
                                                        break;
                                                }

                                                switch (Global.Camera[j].CameraAngle)
                                                {
                                                    case VisionSystemClassLibrary.Enum.CameraRotateAngle.Angle_0:
                                                        RotateFlipFilter.SetIntParameter("Rotation Angle", 0);
                                                        break;
                                                    case VisionSystemClassLibrary.Enum.CameraRotateAngle.Angle_90:
                                                        RotateFlipFilter.SetIntParameter("Rotation Angle", 90);
                                                        break;
                                                    case VisionSystemClassLibrary.Enum.CameraRotateAngle.Angle_180:
                                                        RotateFlipFilter.SetIntParameter("Rotation Angle", 180);
                                                        break;
                                                    case VisionSystemClassLibrary.Enum.CameraRotateAngle.Angle_270:
                                                        RotateFlipFilter.SetIntParameter("Rotation Angle", 270);
                                                        break;
                                                    default:
                                                        break;
                                                }
                                            }

                                            // If the rotation value is 90 or 270, then the resulting video format
                                            // is changed. Thus, the new value can only be set while the live video is
                                            // stopped. Otherwise, an error is returned.

                                            if (_InitCamera(j, false, Global.Camera[j].VideoFormat, Global.Camera[j].DeviceFrameRate, 1))
                                            {
                                                icImagingControlPort[j].LiveStart();

                                                lock (Global.LockMachineFaultState)
                                                {
                                                    Global.MachineFaultState = (Global.MachineFaultState & (~(UInt64)(temp)));     //消除相机故障标志
                                                }

                                                tCameraRestartCount[j].Enabled = true;

                                                break;
                                            }
                                        }
                                        catch (System.Exception ex)                        //端口1相机启动异常
                                        {
                                            if (Global.ShowInformation)//显示调试信息
                                            {
                                                switch (j)
                                                {
                                                    case 0:
                                                        label5.Invoke(new EventHandler(delegate { label5.Text = "相机1初始化异常ti"; }));
                                                        break;
                                                    case 1:
                                                        label2.Invoke(new EventHandler(delegate { label2.Text = "相机2初始化异常ti"; }));
                                                        break;
                                                    case 2:
                                                        label13.Invoke(new EventHandler(delegate { label13.Text = "相机3初始化异常ti"; }));
                                                        break;
                                                    case 3:
                                                        label15.Invoke(new EventHandler(delegate { label15.Text = "相机4初始化异常ti"; }));
                                                        break;
                                                    default:
                                                        break;
                                                }
                                            }
                                        }
                                        break;
                                    }
                                    #endregion
                                }
                            }
                            #endregion
                        }
                        else //当前为串口
                        {
                            #region
                            if (SerialPort.GetPortNames().Contains(SerialPortCommucation[j].PortName)) //串口是否已更新
                            {
                                try
                                {
                                    SerialPortCommucation[j].Open();

                                    lock (LockSerialPort[j])
                                    {
                                        _SendCommand_SerialPortCommucation(5, j);  //发送传感器校准值命令
                                    }

                                    if (VisionSystemClassLibrary.Enum.SensorProductType._89713FA == Global.Camera[j].Sensor_ProductType)
                                    {
                                        lock (LockSerialPort[j])
                                        {
                                            for (Byte k = 0; k < Global.Camera[j].PerTobaccoNumber.Count; k++)
                                            {
                                                _SendCommand_SerialPortCommucation(6, j, false, k);//发送光电各烟支检测相位及区间
                                            }
                                        }
                                    }

                                    lock (LockSerialPort[j])
                                    {
                                        _SendCommand_SerialPortCommucation(1, j);
                                    }
                                    
                                    lock (Global.LockMachineFaultState)
                                    {
                                        Global.MachineFaultState = (Global.MachineFaultState & (~(UInt64)(temp)));     //消除相机故障标志
                                    }

                                    lock (LockCheck_CameraPort)
                                    {
                                        if (false == Global.Check_CameraPort.ContainsKey(Global.Camera[j].Type)) //未包含该相机
                                        {
                                            Global.Check_CameraPort.Add(Global.Camera[j].Type, Global.Camera[j].DeviceInformation.Port);
                                        }
                                    }
                                }
                                catch (System.Exception ex)
                                {
                                    if (Global.ShowInformation)//显示调试信息
                                    {
                                        switch (j)
                                        {
                                            case 0:
                                                label5.Text = "串口1故障.";
                                                break;
                                            case 1:
                                                label2.Text = "串口2故障.";
                                                break;
                                            case 2:
                                                label13.Text = "串口3故障.";
                                                break;
                                            case 3:
                                                label15.Text = "串口4故障.";
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                }
                            }
                            #endregion
                        }

                        #region
                        if (((Global.MachineFaultStateTemp & temp) == 0) && ((Global.MachineFaultState & temp) != 0))  //相机存在故障
                        {
                            if (CameraPowerOffFlagPort[j] == false)                           //相机下电命令已发送
                            {
                                PowerStatePort[j]++;
                                if (PowerStatePort[j] > 11)                                   //PowerState极值为11
                                {
                                    PowerStatePort[j] = 11;
                                }
                            }

                            if (PowerStatePort[j] < 10)                                       //相机1连续上电10次
                            {
                            }
                            else if (PowerStatePort[j] == 10)                                 //相机上电10次未成功，给相机掉电
                            {
                                CameraPowerOffFlagPort[j] = true;
                            }
                            else                                                       //重新给相机上电
                            {
                                CameraPowerOnFlagPort[j] = true;
                            }
                        }
                        #endregion
                    }
                }
            }
            timerPower.Enabled = true;
        }

        //-----------------------------------------------------------------------
        // 功能说明：定时器1响应函数
        // 输入参数： 1、object：sender，timer控件对象
        //            2、EventArgs：e，timer控件事件响应参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;

            label14.Invoke(new EventHandler(delegate { label14.Text = ImageCount[0].ToString() + "/" + ImageBadCount[0].ToString() + "/" + LostCount[0].ToString() + "/" + PushImageCount[0].ToString() + "/" + PopImageCount[0].ToString(); }));
            label19.Invoke(new EventHandler(delegate { label19.Text = ImageCount[1].ToString() + "/" + ImageBadCount[1].ToString() + "/" + LostCount[1].ToString() + "/" + PushImageCount[1].ToString() + "/" + PopImageCount[1].ToString(); }));
            label21.Invoke(new EventHandler(delegate { label21.Text = ImageCount[2].ToString() + "/" + ImageBadCount[2].ToString() + "/" + LostCount[2].ToString() + "/" + PushImageCount[2].ToString() + "/" + PopImageCount[2].ToString(); }));
            label22.Invoke(new EventHandler(delegate { label22.Text = ImageCount[3].ToString() + "/" + ImageBadCount[3].ToString() + "/" + LostCount[3].ToString() + "/" + PushImageCount[3].ToString() + "/" + PopImageCount[3].ToString(); }));

            label20.Invoke(new EventHandler(delegate { label20.Text = serialPortError1[0].ToString() + "/" + serialPortError1[1].ToString() + "/" + serialPortError1[2].ToString() + "/" + serialPortError1[3].ToString(); }));

            if (false == CloseSerialPortFlag)                                  //当前串口正常工作，无需关闭
            {
                if (DiagEnableChanged == true)                                 //诊断使能发生变化
                {
                    lock (LockControllerSerialPort)
                    {
                        _SendCommand(10);                 　　　　　　 //向下位机发送诊断使能状态信息
                    }
                }
                else if (CameraPowerOffFlagPort[0])                            //端口1相机执行下电命令
                {
                    lock (LockControllerSerialPort)
                    {
                        _CameraPowerOff(0);
                        CameraPowerOffFlagPort[0] = false;
                    }
                }
                else if (CameraPowerOnFlagPort[0])                             //端口1相机执行上电命令
                {
                    lock (LockControllerSerialPort)
                    {
                        _CameraPowerOn(0);
                        CameraPowerOnFlagPort[0] = false;
                        PowerStatePort[0] = 0;
                    }
                }
                else if (CameraPowerOffFlagPort[1])                            //端口2相机执行下电命令
                {
                    lock (LockControllerSerialPort)
                    {
                        _CameraPowerOff(1);
                        CameraPowerOffFlagPort[1] = false;
                    }
                }
                else if (CameraPowerOnFlagPort[1])                             //端口2相机执行上电命令
                {
                    lock (LockControllerSerialPort)
                    {
                        _CameraPowerOn(1);
                        CameraPowerOnFlagPort[1] = false;
                        PowerStatePort[1] = 0;
                    }
                }
                else if (CameraPowerOffFlagPort[2])                            //端口3相机执行下电命令
                {
                    lock (LockControllerSerialPort)
                    {
                        _CameraPowerOff(3);
                        CameraPowerOffFlagPort[2] = false;
                    }
                }
                else if (CameraPowerOnFlagPort[2])                             //端口3相机执行上电命令
                {
                    lock (LockControllerSerialPort)
                    {
                        _CameraPowerOn(3);
                        CameraPowerOnFlagPort[2] = false;
                        PowerStatePort[2] = 0;
                    }
                }
                else if (CameraPowerOffFlagPort[3])                            //端口4相机执行下电命令
                {
                    lock (LockControllerSerialPort)
                    {
                        _CameraPowerOff(4);
                        CameraPowerOffFlagPort[3] = false;
                    }
                }
                else if (CameraPowerOnFlagPort[3])                             //端口1相机执行上电命令
                {
                    lock (LockControllerSerialPort)
                    {
                        _CameraPowerOn(4);
                        CameraPowerOnFlagPort[3] = false;
                        PowerStatePort[3] = 0;
                    }
                }
                else
                {
                    if (CommunicationErrCount < 3)   //检查串口通信错误计数
                    {
                        if (CheckDelayCount >= 12)  //查询延时计数
                        {
                            lock (LockControllerSerialPort)
                            {
                                _SendCommand(12);
                            }
                            CheckDelayCount = 0;
                        }
                        else                                                       //向下位机发送状态查询命令
                        {
                            if ((Global.MachineFaultState & 0x01) != 0) //未发生故障
                            {
                                label6.Text = "串行通讯正常.";
                            }

                            lock (Global.LockMachineFaultState)
                            {
                                Global.MachineFaultState &= (~(UInt64)0x01);
                            }

                            lock (LockControllerSerialPort)
                            {
                                _SendCommand(1);
                                CommunicationErrCount++;
                            }
                            CheckDelayCount++;
                        }
                    }
                    else                                                           //连续2次未接收到下位机命令,标志下位机通讯故障
                    {
                        if ((Global.MachineFaultState & 0x01) == 0) //第一次发生故障
                        {
                            label6.Text = "串行通讯故障.";
                        }

                        lock (Global.LockMachineFaultState)
                        {
                            Global.MachineFaultState |= 0x01;
                        }

                        lock (LockControllerSerialPort)
                        {
                            _SendCommand(1);
                        }
                    }
                }
            }

            for (Byte i = 0; i < Global.CameraNumberMax; i++)//遍历当前所有相机
            {
                if ((Global.CameraChooseState & (0x01 << i)) != 0)//当前相机开启
                {
                    switch (i)
                    {
                        case 0:
                            if (((Global.MachineFaultState & 0x02) == 0) && (null != SerialPortCommucation[i]) && (false == SerialPortCommucation[i].IsOpen)) //串口第一次发生故障
                            {
                                lock (Global.LockMachineFaultState)
                                {
                                    Global.MachineFaultState |= 0x02;
                                }

                                Global.Camera[i].DeviceInformation.CAM = VisionSystemClassLibrary.Enum.CameraState.OFF;
                                Global.CameraTemp[i].DeviceInformation.CAM = Global.Camera[i].DeviceInformation.CAM;

                                lock (LockCheck_CameraPort)
                                {
                                    if (VisionSystemClassLibrary.Enum.RelevancyType.None < Global.Camera[i].RelevancyCameraInfo.rRelevancyType) //相机含有关联信息
                                    {
                                        if (Global.Check_CameraPort.ContainsKey(Global.Camera[i].Type)) //包含相机
                                        {
                                            Global.Check_CameraPort.Remove(Global.Camera[i].Type);
                                        }
                                        _ClearRelevancyImageBuffer();
                                    }
                                }

                                lock (LockControllerSerialPort)//相机下电
                                {
                                    _CameraPowerOff(0);                                         //端口1相机下电
                                }
                                PowerStatePort[i] = 10;
                                LostCount[i]++;

                                if (Global.ShowInformation)//显示调试信息
                                {
                                    label5.Invoke(new EventHandler(delegate { label5.Text = "串口1故障."; }));
                                }
                            }
                            break;
                        case 1:
                            if (((Global.MachineFaultState & 0x04) == 0) && (null != SerialPortCommucation[i]) && (false == SerialPortCommucation[i].IsOpen)) //串口第一次发生故障
                            {
                                lock (Global.LockMachineFaultState)
                                {
                                    Global.MachineFaultState |= 0x04;
                                }

                                Global.Camera[i].DeviceInformation.CAM = VisionSystemClassLibrary.Enum.CameraState.OFF;
                                Global.CameraTemp[i].DeviceInformation.CAM = Global.Camera[i].DeviceInformation.CAM;

                                lock (LockCheck_CameraPort)
                                {
                                    if (VisionSystemClassLibrary.Enum.RelevancyType.None < Global.Camera[i].RelevancyCameraInfo.rRelevancyType) //相机含有关联信息
                                    {
                                        if (Global.Check_CameraPort.ContainsKey(Global.Camera[i].Type)) //包含相机
                                        {
                                            Global.Check_CameraPort.Remove(Global.Camera[i].Type);
                                        }
                                        _ClearRelevancyImageBuffer();
                                    }
                                }

                                lock (LockControllerSerialPort)//相机下电
                                {
                                    _CameraPowerOff(1);                                         //端口2相机下电
                                }
                                PowerStatePort[i] = 10;
                                LostCount[i]++;

                                if (Global.ShowInformation)//显示调试信息
                                {
                                    label2.Invoke(new EventHandler(delegate { label2.Text = "串口2故障."; }));
                                }
                            }
                            break;
                        case 2:
                            if (((Global.MachineFaultState & 0x010000000000) == 0) && (null != SerialPortCommucation[i]) && (false == SerialPortCommucation[i].IsOpen)) //串口第一次发生故障
                            {
                                lock (Global.LockMachineFaultState)
                                {
                                    Global.MachineFaultState |= 0x010000000000;
                                }

                                Global.Camera[i].DeviceInformation.CAM = VisionSystemClassLibrary.Enum.CameraState.OFF;
                                Global.CameraTemp[i].DeviceInformation.CAM = Global.Camera[i].DeviceInformation.CAM;

                                lock (LockCheck_CameraPort)
                                {
                                    if (VisionSystemClassLibrary.Enum.RelevancyType.None < Global.Camera[i].RelevancyCameraInfo.rRelevancyType) //相机含有关联信息
                                    {
                                        if (Global.Check_CameraPort.ContainsKey(Global.Camera[i].Type)) //包含相机
                                        {
                                            Global.Check_CameraPort.Remove(Global.Camera[i].Type);
                                        }
                                        _ClearRelevancyImageBuffer();
                                    }
                                }

                                lock (LockControllerSerialPort)//相机下电
                                {
                                    _CameraPowerOff(3);                                         //端口3相机下电
                                }
                                PowerStatePort[i] = 10;
                                LostCount[i]++;

                                if (Global.ShowInformation)//显示调试信息
                                {
                                    label13.Invoke(new EventHandler(delegate { label13.Text = "串口3故障."; }));
                                }
                            }
                            break;
                        case 3:
                            if (((Global.MachineFaultState & 0x020000000000) == 0) && (null != SerialPortCommucation[i]) && (false == SerialPortCommucation[i].IsOpen)) //串口第一次发生故障
                            {
                                lock (Global.LockMachineFaultState)
                                {
                                    Global.MachineFaultState |= 0x020000000000;
                                }

                                Global.Camera[i].DeviceInformation.CAM = VisionSystemClassLibrary.Enum.CameraState.OFF;
                                Global.CameraTemp[i].DeviceInformation.CAM = Global.Camera[i].DeviceInformation.CAM;

                                lock (LockCheck_CameraPort)
                                {
                                    if (VisionSystemClassLibrary.Enum.RelevancyType.None < Global.Camera[i].RelevancyCameraInfo.rRelevancyType) //相机含有关联信息
                                    {
                                        if (Global.Check_CameraPort.ContainsKey(Global.Camera[i].Type)) //包含相机
                                        {
                                            Global.Check_CameraPort.Remove(Global.Camera[i].Type);
                                        }
                                        _ClearRelevancyImageBuffer();
                                    }
                                }

                                lock (LockControllerSerialPort)//相机下电
                                {
                                    _CameraPowerOff(4);                                         //端口4相机下电
                                }
                                PowerStatePort[i] = 10;
                                LostCount[i]++;

                                if (Global.ShowInformation)//显示调试信息
                                {
                                    label15.Invoke(new EventHandler(delegate { label15.Text = "串口4故障."; }));
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            timer1.Enabled = true;
        }

        //-----------------------------------------------------------------------
        // 功能说明：曲线图数据更新
        // 输入参数：1、Byte:index，数组下标索引 
        //           2、Byte:toolIndex，工具索引值
        //           3、List<Int32>:Value，最新曲线图数据
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _TolerancesGraphDataInit(Byte index, Byte toolIndex, Int32[] Value)
        {
            if (Global.Camera[index].Tools[toolIndex].ExistTolerance) //公差索引值有效
            {
                if (Global.Camera[index].Tolerances.GraphData[Global.Camera[index].Tools[toolIndex].TolerancesIndex].TolerancesGraphDataValue.CurrentValueIndex < Global.Camera[index].Tolerances.GraphData[Global.Camera[index].Tools[toolIndex].TolerancesIndex].ValueNumber - 1)//缺陷图索引值未填满
                {
                    Global.Camera[index].Tolerances.GraphData[Global.Camera[index].Tools[toolIndex].TolerancesIndex].TolerancesGraphDataValue.CurrentValueIndex++;
                    Global.Camera[index].Tolerances.GraphData[Global.Camera[index].Tools[toolIndex].TolerancesIndex].TolerancesGraphDataValue.Value[Global.Camera[index].Tolerances.GraphData[Global.Camera[index].Tools[toolIndex].TolerancesIndex].TolerancesGraphDataValue.CurrentValueIndex] = Value[0];//按位更新曲线图数据
                }
                else//缺陷图索引值填满后，更新数据位
                {
                    System.Array.Copy(Global.Camera[index].Tolerances.GraphData[Global.Camera[index].Tools[toolIndex].TolerancesIndex].TolerancesGraphDataValue.Value, 1, Global.Camera[index].Tolerances.GraphData[Global.Camera[index].Tools[toolIndex].TolerancesIndex].TolerancesGraphDataValue.Value, 0, Global.Camera[index].Tolerances.GraphData[Global.Camera[index].Tools[toolIndex].TolerancesIndex].ValueNumber - 1);
                    Global.Camera[index].Tolerances.GraphData[Global.Camera[index].Tools[toolIndex].TolerancesIndex].TolerancesGraphDataValue.Value[Global.Camera[index].Tolerances.GraphData[Global.Camera[index].Tools[toolIndex].TolerancesIndex].ValueNumber - 1] = Value[0];//缺陷图索引值填满后，曲线图数据移位
                }
                Global.Camera[index].Tolerances.GraphData[Global.Camera[index].Tools[toolIndex].TolerancesIndex].TolerancesGraphDataValue.MeanValue = _TolerancesGraphDataAverage(index, toolIndex, Convert.ToInt16(Global.Camera[index].Tolerances.GraphData[Global.Camera[index].Tools[toolIndex].TolerancesIndex].TolerancesGraphDataValue.CurrentValueIndex + 1));//更新曲线图数据均值
            }
        }
        //-----------------------------------------------------------------------
        // 功能说明：曲线图数据平均值
        // 输入参数：1、Byte：index，数组下标索引
        //           2、Byte：toolIndex，工具索引值
        //           3、Int16：currentValueIndex，曲线数据横坐标有效点个数
        // 输出参数： 无
        // 返 回 值： Int32，曲线数据平均值
        //----------------------------------------------------------------------
        private Int32 _TolerancesGraphDataAverage(Byte index, Byte toolIndex, Int16 currentValueCount)
        {
            Double GraphDataAverage = 0;
            for (Byte i = 0; i < currentValueCount; i++)//遍历当前曲线数据坐标点，求和
            {
                GraphDataAverage += Global.Camera[index].Tolerances.GraphData[Global.Camera[index].Tools[toolIndex].TolerancesIndex].TolerancesGraphDataValue.Value[i];
            }

            if (currentValueCount > 0)
            {
                return Convert.ToInt32(GraphDataAverage / currentValueCount);//计算曲线数据均值
            }
            else
            {
                return 0;//计算曲线数据均值
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明： 向下位机发送命令函数
        // 输入参数： 1、Byte：Type，命令类型
        //            2、Byte：index，相机类型
        //            3、Boolean：bResultPort，处理结果
        //            4、Byte：bErrorIndexPort，第一个缺陷工具索引
        //            5、Boolean：bCheckTobaccoState，非空模盒标记:True为非空；FALSE为空
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _SendCommand(Byte Type, Byte index = 0, Boolean bResultPort = true, Byte bErrorIndexPort = 0, Boolean bCheckTobaccoState = true)
        {
            Byte[] Command = new Byte[Global.ControllerSerialPortCommucation_SendBytesThreshold];//保存串口发送指令

            switch (Type)                                                      //向下位机发送命令
            {
                case 1:
                    Command[0] = 1;
                    Command[1] = 5;

                    if ((Global.MachineFaultState & 0x02) != 0)                //相机1发生故障
                    {
                        Command[2] |= 0x01;
                    }
                    else                                                       //相机1正常工作
                    {
                        Command[2] &= 0xFE;
                    }

                    if ((Global.MachineFaultState & 0x04) != 0)                //相机2发生故障
                    {
                        Command[2] |= 0x02;
                    }
                    else                                                       //相机2正常工作
                    {
                        Command[2] &= 0xFD;
                    }

                    if ((Global.MachineFaultState & 0x010000000000) != 0)      //相机3存在故障
                    {
                        Command[2] |= 0x04;
                    }
                    else                                                       //相机3正常工作
                    {
                        Command[2] &= 0xFB;
                    }

                    if ((Global.MachineFaultState & 0x020000000000) != 0)      //相机4存在故障
                    {
                        Command[2] |= 0x08;
                    }
                    else                                                       //相机4正常工作
                    {
                        Command[2] &= 0xF7;
                    }

                    if (LabModel)                                              //当前处在实验室
                    {
                        Command[3] = (Byte)(Global.DeviceIOSignal.OutputState & 0xFF);            //向下位机发送10路PNP输出低8位

                        UInt32 outputState = Global.DeviceIOSignal.OutputState >> 8;
                        outputState = ((outputState & 0x3C) << 2) | (outputState & 0x03);
                        Command[4] = Convert.ToByte(outputState);                 //向下位机发送10路PNP输出高两位和4路NPN输出                       
                        Command[5] |= 0x80;
                    }
                    else
                    {
                        Command[3] = 0;
                        Command[4] = 0;
                        Command[5] &= 0x7F;
                    }

                    if (VisionSystemClassLibrary.Class.System.SystemDeviceState == VisionSystemClassLibrary.Enum.DeviceState.Run) //系统运行
                    {
                        Command[5] |= 0x20;
                    }
                    else                                                       //系统暂停
                    {
                        Command[5] &= 0xDF;
                    }

                    Command[6] = 0x02;

                    Command[7] = (Byte)(Command[0] ^ Command[1] ^ Command[2] ^ Command[3] ^ Command[4] ^ Command[5] ^ Command[6]);
                    if (ControllerSerialPortCommucation.IsOpen == true)                            //当前串口1打开，向下位发送命令
                    {
                        ControllerSerialPortCommucation.Write(Command, 0, 8);
                    }
                    break;

                case 5:
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
                    if (ControllerSerialPortCommucation.IsOpen == true)                            //当前串口1打开，向下位发送命令
                    {
                        ControllerSerialPortCommucation.Write(Command, 0, 7);
                    }
                    break;

                case 9:
                    Command[0] = 9;
                    Command[1] = 4;

                    if ((VisionSystemClassLibrary.Class.System.SystemDeviceState == VisionSystemClassLibrary.Enum.DeviceState.Run) && (false != Global.Camera[index].CheckEnable))//检测使能开启
                    {
                        if (bResultPort)//检测结果：完好
                        {
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
                            Command[3] = 0;
                        }
                        else//检测结果：缺陷
                        {
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
                            Command[3] = Convert.ToByte(bErrorIndexPort + 1);
                        }
                    }
                    else//检测使能关闭
                    {
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
                        Command[3] = 0;
                    }
                    Command[4] = (Byte)Global.Camera[index].Type;

                    if (bCheckTobaccoState) //当前为非空墨盒
                    {
                        Command[5] = 1;
                    }
                    else  //当前为空墨盒
                    {
                        Command[5] = 0;
                    }

                    Command[6] = (Byte)(Command[0] ^ Command[1] ^ Command[2] ^ Command[3] ^ Command[4] ^ Command[5]);

                    if (ControllerSerialPortCommucation.IsOpen == true)                            //当前串口1打开，向下位发送命令
                    {
                        ControllerSerialPortCommucation.Write(Command, 0, 7);
                    }
                    break;

                case 10:
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

                    if (ControllerSerialPortCommucation.IsOpen == true)                            //当前串口1打开，向下位发送命令
                    {
                        ControllerSerialPortCommucation.Write(Command, 0, 10);
                    }
                    break;

                case 12:
                    Command[0] = 0x0C;
                    Command[1] = 0;
                    Command[2] = 0x0C;

                    if (ControllerSerialPortCommucation.IsOpen == true)                            //当前串口1打开，向下位发送命令
                    {
                        ControllerSerialPortCommucation.Write(Command, 0, 3);
                    }
                    break;

                case 16:
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
                    Command[3] = MachineType;//机器类型；

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

                    if (ControllerSerialPortCommucation.IsOpen == true)                            //当前串口1打开，向下位发送命令
                    {
                        ControllerSerialPortCommucation.Write(Command, 0, checkIndex + 1);
                    }
                    break;
                case 19:
                    Command[0] = 19;
                    Command[1] = 0;
                    Command[2] = (Byte)(Command[0] ^ Command[1]);

                    if (ControllerSerialPortCommucation.IsOpen == true)                            //当前串口1打开，向下位发送命令
                    {
                        ControllerSerialPortCommucation.Write(Command, 0, 3);
                    }
                    break;

                default:
                    break;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明： 向下位机发送命令函数
        // 输入参数： 1、Byte：Type，命令类型
        //            2、Byte：index，相机类型
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _SendCommand_SerialPortCommucation(Byte Type, Byte index, Boolean bAdjustState = false, Byte iPerTobaccoIndex = 0)
        {
            try
            {
                Byte[] Command = new Byte[Global.Camera[index].SerialPort_SendBytesThreshold];//保存串口发送指令

                switch (Type)  //向下位机发送命令
                {
                    case 1:
                        Command[0] = 1;
                        Command[1] = 1;

                        if (LabModel)   //当前处在实验室
                        {
                            Command[2] = 1; //向下位机发送实验室状态
                        }
                        else
                        {
                            Command[2] = 0;
                        }

                        Command[3] = (Byte)(Command[0] ^ Command[1] ^ Command[2]);
                        if (SerialPortCommucation[index].IsOpen == true) //当前串口1打开，向下位发送命令
                        {
                            SerialPortCommucation[index].Write(Command, 0, 4);
                        }
                        break;

                    case 2:
                        switch (Global.Camera[index].Sensor_ProductType)
                        {
                            case VisionSystemClassLibrary.Enum.SensorProductType._89713FC:
                                Command[0] = 2;
                                Command[1] = 3;

                                Command[2] = (Byte)(0xFF & Global.Camera[index].DeviceParameter.SensorSelectState);//1-8个烟支选择校准状态
                                Command[3] = (Byte)(0xFF & (Global.Camera[index].DeviceParameter.SensorSelectState >> 8));//9-16个烟支选择校准状态
                                Command[4] = (Byte)(0x0F & (Global.Camera[index].DeviceParameter.SensorSelectState >> 16));//17-20个烟支选择校准状态，位7为1时取消校准并清零所有变量，电位器恢复原始状态

                                if (false == bAdjustState) //取消校准功能
                                {
                                    Command[4] = 0x80;//17-20个烟支选择校准状态，位7为1时取消校准并清零所有变量，电位器恢复原始状态
                                }
                                Command[5] = (Byte)(Command[0] ^ Command[1] ^ Command[2] ^ Command[3] ^ Command[4]);
                                if (SerialPortCommucation[index].IsOpen == true)  //当前串口1打开，向下位发送命令
                                {
                                    SerialPortCommucation[index].Write(Command, 0, 6);
                                }
                                break;
                            case VisionSystemClassLibrary.Enum.SensorProductType._89713FA:
                            case VisionSystemClassLibrary.Enum.SensorProductType._89713CF:
                                Command[0] = 2;
                                Command[1] = 0;

                                Command[2] = (Byte)(Command[0] ^ Command[1]);
                                if (SerialPortCommucation[index].IsOpen == true) //当前串口1打开，向下位发送命令
                                {
                                    SerialPortCommucation[index].Write(Command, 0, 3);
                                }
                                break;
                            case VisionSystemClassLibrary.Enum.SensorProductType._89750A:
                                break;
                            default:
                                break;
                        }
                        break;

                    case 3:
                        Command[0] = 3;
                        Command[1] = 1;

                        if (true == bAdjustState) //寻找最大值
                        {
                            Command[2] = 1;
                        }

                        Command[3] = (Byte)(Command[0] ^ Command[1] ^ Command[2]);
                        if (SerialPortCommucation[index].IsOpen == true) //当前串口1打开，向下位发送命令
                        {
                            SerialPortCommucation[index].Write(Command, 0, 4);
                        }
                        break;

                    case 4:
                        Command[0] = 4;
                        Command[1] = 0;

                        Command[2] = (Byte)(Command[0] ^ Command[1]);
                        if (SerialPortCommucation[index].IsOpen == true) //当前串口1打开，向下位发送命令
                        {
                            SerialPortCommucation[index].Write(Command, 0, 3);
                        }
                        break;

                    case 5:
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

                                for (Byte i = 0; i < 23; i++)
                                {
                                    Command[23] ^= Command[i];
                                }

                                if (SerialPortCommucation[index].IsOpen == true) //当前串口1打开，向下位发送命令
                                {
                                    SerialPortCommucation[index].Write(Command, 0, 24);
                                }
                                break;
                            case VisionSystemClassLibrary.Enum.SensorProductType._89713CF:
                                Command[0] = 5;
                                Command[1] = 15;

                                Command[2] = (Byte)Global.Camera[index].PerTobaccoNumber.Count;//烟支排数

                                for (Int32 i = 0; i < Global.Camera[index].SensorNumber; i++) //循环所有烟支
                                {
                                    Command[3 + i] = Global.Camera[index].DeviceParameter.SensorAdjustValue[i];
                                }

                                if (null != Global.Camera[index].DeviceParameter.Parameter) //参数有效
                                {
                                    for (Int32 i = 0; i < Global.Camera[index].DeviceParameter.Parameter.Count; i++) //循环所有烟支
                                    {
                                        Command[i * 2 + 8] = Convert.ToByte(Global.Camera[index].DeviceParameter.Parameter[i] * Global.Camera[index].EncoderPer & 0xFF);//相机曝光相位低8位；
                                        Command[i * 2 + 9] = Convert.ToByte((Global.Camera[index].DeviceParameter.Parameter[i] * Global.Camera[index].EncoderPer >> 8) & 0xFF);//相机曝光相位高8位；
                                    }
                                }

                                for (Byte i = 0; i < 17; i++)
                                {
                                    Command[17] ^= Command[i];
                                }

                                if (SerialPortCommucation[index].IsOpen == true) //当前串口1打开，向下位发送命令
                                {
                                    SerialPortCommucation[index].Write(Command, 0, 18);
                                }
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

                                for (Byte i = 0; i < 17; i++)
                                {
                                    Command[17] ^= Command[i];
                                }

                                if (SerialPortCommucation[index].IsOpen == true) //当前串口1打开，向下位发送命令
                                {
                                    SerialPortCommucation[index].Write(Command, 0, 18);
                                }
                                break;
                            case VisionSystemClassLibrary.Enum.SensorProductType._89750A:
                                break;
                            default:
                                break;
                        }
                        break;

                    case 6:
                        if ((null != Global.Camera[index].DeviceParameter.Parameter) && (Global.Camera[index].DeviceParameter.Parameter.Count > 3)) //连续采样数据有效
                        {
                            Command[0] = 6;
                            Command[1] = (Byte)(Global.Camera[index].PerTobaccoNumber[iPerTobaccoIndex] * 3 + 1);
                            Command[2] = iPerTobaccoIndex;//光电管索引

                            Int32 iStartPhase = Global.Camera[index].EncoderPer * Global.Camera[index].DeviceParameter.Parameter[2];
                            Int32 iEndPhase = Global.Camera[index].EncoderPer * Global.Camera[index].DeviceParameter.Parameter[3] + 1;

                            double dPresion = (double)Global.Camera[index].ImageWidth / ((iEndPhase + 1800 - iStartPhase) % 1800);

                            for (Int32 i = 0; i < Global.Camera[index].PerTobaccoNumber[iPerTobaccoIndex]; i++) //循环当前排烟支
                            {
                                Rectangle rect = VisionSystemClassLibrary.GeneralFunction._GetMinRect(Global.Camera[index].Tools[i].ROI.roiExtra);
                                UInt16 uiPhase = Convert.ToUInt16(((double)rect.Left + (double)rect.Width / 2) / dPresion + iStartPhase);
                                Command[3 * i + 3] = Convert.ToByte(0xFF & uiPhase);
                                Command[3 * i + 4] = Convert.ToByte(0xFF & (uiPhase >> 8));
                                Command[3 * i + 5] = Convert.ToByte((double)rect.Width / 2 / dPresion);
                            }

                            Int32 iCheckCount = Command[1] + 2;

                            for (Byte i = 0; i < iCheckCount; i++)
                            {
                                Command[iCheckCount] ^= Command[i];
                            }

                            if (SerialPortCommucation[index].IsOpen == true) //当前串口1打开，向下位发送命令
                            {
                                SerialPortCommucation[index].Write(Command, 0, iCheckCount + 1);
                            }
                        }
                        break;

                    case 7:
                        Command[0] = 7;
                        Command[1] = 1;
                        if (false == bAdjustState) //取消校准功能
                        {
                            Command[2] = 0x80;//17-20个烟支选择校准状态，位7为1时取消校准并清零所有变量，电位器恢复原始状态
                        }
                        else
                        {
                            Command[2] = (Byte)(0x1F & Global.Camera[index].DeviceParameter.SensorSelectState);//1-8个烟支选择校准状态
                        }
                        Command[3] = (Byte)(Command[0] ^ Command[1] ^ Command[2]);

                        if (SerialPortCommucation[index].IsOpen == true) //当前串口1打开，向下位发送命令
                        {
                            SerialPortCommucation[index].Write(Command, 0, 4);
                        }
                        break;

                    case 8:
                        Command[0] = 8;
                        Command[1] = 1;
                        if (false == bAdjustState) //取消校准功能
                        {
                            Command[2] = 0x80;//17-20个烟支选择校准状态，位7为1时取消校准并清零所有变量，电位器恢复原始状态
                        }
                        else
                        {
                            Command[2] = (Byte)(0xFF & Global.Camera[index].DeviceParameter.SensorSelectState);//1-8个烟支选择校准状态
                        }
                        Command[3] = (Byte)(Command[0] ^ Command[1] ^ Command[2]);

                        if (SerialPortCommucation[index].IsOpen == true) //当前串口1打开，向下位发送命令
                        {
                            SerialPortCommucation[index].Write(Command, 0, 4);
                        }
                        break;

                    case 9:
                        Command[0] = 9;
                        Command[1] = 0;
                        Command[2] = (Byte)(Command[0] ^ Command[1]);

                        if (SerialPortCommucation[index].IsOpen == true) //当前串口1打开，向下位发送命令
                        {
                            SerialPortCommucation[index].Write(Command, 0, 3);
                        }
                        break;

                    case 10:
                        Command[0] = 10;
                        Command[1] = 0;
                        Command[2] = (Byte)(Command[0] ^ Command[1]);

                        if (SerialPortCommucation[index].IsOpen == true) //当前串口1打开，向下位发送命令
                        {
                            SerialPortCommucation[index].Write(Command, 0, 3);
                        }
                        break;

                    case 11:
                        Command[0] = 11;
                        Command[1] = 0;

                        Command[2] = (Byte)(Command[0] ^ Command[1]);
                        if (SerialPortCommucation[index].IsOpen == true) //当前串口1打开，向下位发送命令
                        {
                            SerialPortCommucation[index].Write(Command, 0, 3);
                        }
                        break;

                    case 13:
                        Command[0] = 13;
                        Command[1] = 0;

                        Command[2] = (Byte)(Command[0] ^ Command[1]);
                        if (SerialPortCommucation[index].IsOpen == true) //当前串口1打开，向下位发送命令
                        {
                            SerialPortCommucation[index].Write(Command, 0, 3);
                        }
                        break;

                    case 19:
                        Command[0] = 19;
                        Command[1] = 0;
                        Command[2] = (Byte)(Command[0] ^ Command[1]);

                        if (SerialPortCommucation[index].IsOpen == true) //当前串口1打开，向下位发送命令
                        {
                            SerialPortCommucation[index].Write(Command, 0, 3);
                        }
                        break;

                    case 20:
                        Command[0] = 20;
                        Command[1] = 0;

                        Command[2] = (Byte)(Command[0] ^ Command[1]);
                        if (SerialPortCommucation[index].IsOpen == true) //当前串口1打开，向下位发送命令
                        {
                            SerialPortCommucation[index].Write(Command, 0, 3);
                        }
                        break;

                    default:
                        break;
                }
            }
            catch (System.Exception ex)
            {
                switch (index)
                {
                    case 0:
                        if ((Global.MachineFaultState & 0x02) == 0) //串口第之前未发生故障
                        {
                            lock (Global.LockMachineFaultState)
                            {
                                Global.MachineFaultState |= 0x02;
                            }

                            Global.Camera[index].DeviceInformation.CAM = VisionSystemClassLibrary.Enum.CameraState.OFF;
                            Global.CameraTemp[index].DeviceInformation.CAM = Global.Camera[index].DeviceInformation.CAM;

                            lock (LockCheck_CameraPort)
                            {
                                if (VisionSystemClassLibrary.Enum.RelevancyType.None < Global.Camera[index].RelevancyCameraInfo.rRelevancyType) //相机含有关联信息
                                {
                                    if (Global.Check_CameraPort.ContainsKey(Global.Camera[index].Type)) //包含相机
                                    {
                                        Global.Check_CameraPort.Remove(Global.Camera[index].Type);
                                    }
                                    _ClearRelevancyImageBuffer();
                                }
                            }

                            lock (LockControllerSerialPort)//相机下电
                            {
                                _CameraPowerOff(0);                                         //端口1相机下电
                            }
                            PowerStatePort[index] = 10;
                            LostCount[index]++;

                            if (Global.ShowInformation)//显示调试信息
                            {
                                label5.Invoke(new EventHandler(delegate { label5.Text = "串口1故障."; }));
                            }
                        }
                        break;
                    case 1:
                        if (((Global.MachineFaultState & 0x04) == 0)) //串口之前未发生故障
                        {
                            lock (Global.LockMachineFaultState)
                            {
                                Global.MachineFaultState |= 0x04;
                            }

                            Global.Camera[index].DeviceInformation.CAM = VisionSystemClassLibrary.Enum.CameraState.OFF;
                            Global.CameraTemp[index].DeviceInformation.CAM = Global.Camera[index].DeviceInformation.CAM;

                            lock (LockCheck_CameraPort)
                            {
                                if (VisionSystemClassLibrary.Enum.RelevancyType.None < Global.Camera[index].RelevancyCameraInfo.rRelevancyType) //相机含有关联信息
                                {
                                    if (Global.Check_CameraPort.ContainsKey(Global.Camera[index].Type)) //包含相机
                                    {
                                        Global.Check_CameraPort.Remove(Global.Camera[index].Type);
                                    }
                                    _ClearRelevancyImageBuffer();
                                }
                            }

                            lock (LockControllerSerialPort)//相机下电
                            {
                                _CameraPowerOff(1);                                         //端口2相机下电
                            }
                            PowerStatePort[index] = 10;
                            LostCount[index]++;

                            if (Global.ShowInformation)//显示调试信息
                            {
                                label2.Invoke(new EventHandler(delegate { label2.Text = "串口2故障."; }));
                            }
                        }
                        break;
                    case 2:
                        if ((Global.MachineFaultState & 0x010000000000) == 0) //串口之前未发生故障
                        {
                            lock (Global.LockMachineFaultState)
                            {
                                Global.MachineFaultState |= 0x010000000000;
                            }

                            Global.Camera[index].DeviceInformation.CAM = VisionSystemClassLibrary.Enum.CameraState.OFF;
                            Global.CameraTemp[index].DeviceInformation.CAM = Global.Camera[index].DeviceInformation.CAM;

                            lock (LockCheck_CameraPort)
                            {
                                if (VisionSystemClassLibrary.Enum.RelevancyType.None < Global.Camera[index].RelevancyCameraInfo.rRelevancyType) //相机含有关联信息
                                {
                                    if (Global.Check_CameraPort.ContainsKey(Global.Camera[index].Type)) //包含相机
                                    {
                                        Global.Check_CameraPort.Remove(Global.Camera[index].Type);
                                    }
                                    _ClearRelevancyImageBuffer();
                                }
                            }

                            lock (LockControllerSerialPort)//相机下电
                            {
                                _CameraPowerOff(3);                                         //端口3相机下电
                            }
                            PowerStatePort[index] = 10;
                            LostCount[index]++;

                            if (Global.ShowInformation)//显示调试信息
                            {
                                label13.Invoke(new EventHandler(delegate { label13.Text = "串口3故障."; }));
                            }
                        }
                        break;
                    case 3:
                        if ((Global.MachineFaultState & 0x020000000000) == 0) //串口之前未发生故障
                        {
                            lock (Global.LockMachineFaultState)
                            {
                                Global.MachineFaultState |= 0x020000000000;
                            }

                            Global.Camera[index].DeviceInformation.CAM = VisionSystemClassLibrary.Enum.CameraState.OFF;
                            Global.CameraTemp[index].DeviceInformation.CAM = Global.Camera[index].DeviceInformation.CAM;

                            lock (LockCheck_CameraPort)
                            {
                                if (VisionSystemClassLibrary.Enum.RelevancyType.None < Global.Camera[index].RelevancyCameraInfo.rRelevancyType) //相机含有关联信息
                                {
                                    if (Global.Check_CameraPort.ContainsKey(Global.Camera[index].Type)) //包含相机
                                    {
                                        Global.Check_CameraPort.Remove(Global.Camera[index].Type);
                                    }
                                    _ClearRelevancyImageBuffer();
                                }
                            }

                            lock (LockControllerSerialPort)//相机下电
                            {
                                _CameraPowerOff(4);                                         //端口4相机下电
                            }
                            PowerStatePort[index] = 10;
                            LostCount[index]++;

                            if (Global.ShowInformation)//显示调试信息
                            {
                                label15.Invoke(new EventHandler(delegate { label15.Text = "串口4故障."; }));
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：串口接收中断函数，读串口数据
        // 输入参数：1、object：sender，serialPort控件对象
        //           2、System.IO.Ports.SerialDataReceivedEventArgs：e，serialport控件事件响应参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _USBPortCommucation_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            USBPortCommucation.Read(CommandUSB, 0, 1);
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
            try
            {
                while (ControllerSerialPortCommucation.BytesToRead >= Global.ControllerSerialPortCommucation_ReceivedBytesThreshold) //串口1读取字节长度大于等于32
                {
                    Byte[] bReceiveData = new Byte[Global.ControllerSerialPortCommucation_ReceivedBytesThreshold];
                    ControllerSerialPortCommucation.Read(bReceiveData, 0, Global.ControllerSerialPortCommucation_ReceivedBytesThreshold);

                    Global.ControllerSerialPortCommucationBuffer.AddRange(bReceiveData); //加入缓冲区

                    while (true) //循环解析命令
                    {
                        if ((null != Global.ControllerSerialPortCommucationBuffer) && (Global.ControllerSerialPortCommucationBuffer.Count > 2)) //最少包含命令类型、命令长度、校验位 
                        {
                            Byte bType = Global.ControllerSerialPortCommucationBuffer.First();

                            if (Global.ControllerSerialPortCommucationType.ContainsKey(bType)) //命令类型有效
                            {
                                Byte bValue = 0;
                                Global.ControllerSerialPortCommucationType.TryGetValue(bType, out bValue);

                                if (bValue == Global.ControllerSerialPortCommucationBuffer[1]) //命令长度有效
                                {
                                    if (Global.ControllerSerialPortCommucationBuffer.Count >= (bValue + 3)) //包含整条命令
                                    {
                                        Int32 iDataCount = bValue + 2;
                                        Byte bReceiveChecksum = 0;
                                        for (Byte i = 0; i < iDataCount; i++)      //读取串口数据，计算校验数据
                                        {
                                            bReceiveChecksum ^= Global.ControllerSerialPortCommucationBuffer[i];
                                        }

                                        if (bReceiveChecksum == Global.ControllerSerialPortCommucationBuffer[iDataCount]) //校验和相同
                                        {
                                            CommunicationErrCount = 0;

                                            _GetControllerSerialPortCommucationBuffer(Global.ControllerSerialPortCommucationBuffer.GetRange(0, bValue + 3));
                                            Global.ControllerSerialPortCommucationBuffer.RemoveRange(0, bValue + 3);
                                        }
                                        else //数据可能丢失，继续查询命令
                                        {
                                            Global.ControllerSerialPortCommucationBuffer.RemoveAt(0);
                                        }
                                    }
                                    else //数据可能丢失，继续查询命令
                                    {
                                        Global.ControllerSerialPortCommucationBuffer.RemoveAt(0);
                                    }
                                }
                                else //命令长度不匹配
                                {
                                    Global.ControllerSerialPortCommucationBuffer.RemoveAt(0);
                                }
                            }
                            else //命令类型不匹配
                            {
                                Global.ControllerSerialPortCommucationBuffer.RemoveAt(0);
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明： 串口1控件接收到数据的处理函数
        // 输入参数：1、Byte[]：ReceiveData，串口数据
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _GetControllerSerialPortCommucationBuffer(List<Byte> ReceiveData)
        {
            try
            {
                switch (ReceiveData[0])
                {
                    case 1:
                        Global.DeviceIOSignal.InputState = (UInt16)(ReceiveData[3] + (((UInt16)ReceiveData[4]) << 8)); //读取输入状态，低8路PNP，高6路NPN
                        Global.DeviceIOSignal.OutputDiagStateLab = (UInt16)(
                            ReceiveData[5] + (((UInt16)ReceiveData[6]) << 8));//读取输出状态

                        UInt32 oututDiagStateLab = Global.DeviceIOSignal.OutputDiagStateLab;
                        oututDiagStateLab = ((oututDiagStateLab & 0xF000) >> 2) | (oututDiagStateLab & 0x03FF);
                        Global.DeviceIOSignal.OutputDiagStateLab = oututDiagStateLab;

                        Global.DeviceIOSignal.OutputDiagState = (UInt16)(
                           ReceiveData[7] + (((UInt16)ReceiveData[8]) << 8));

                        Boolean speedPhase_AsMachine = true;
                        Int32 iSpeed = ReceiveData[9] + ((UInt16)ReceiveData[10] << 8);      //读取相机1当前速度值

                        if (iSpeed < 30)             //相机1机器运行速度<30
                        {
                            speedPhase_AsMachine = false;
                            iSpeed = ReceiveData[13] + ((UInt16)ReceiveData[14] << 8);          //读取相机1当前相位值
                        }

                        for (Byte i = 0; i < Global.CameraNumberMax; i++)//遍历当前所有相机
                        {
                            if ((Global.CameraChooseState & (0x01 << i)) != 0)//当前相机开启
                            {
                                this.Invoke(new EventHandler(delegate
                                {
                                    Global.Camera[i].UIParameter.SpeedPhase_AsMachine = speedPhase_AsMachine;
                                    Global.Camera[i].UIParameter.Speed = iSpeed;
                                }));
                            }
                        }

                        label10.Invoke(new EventHandler(delegate
                        {
                            label10.Text = Global.Camera[0].UIParameter.Speed.ToString();
                        }));

                        UInt32 outputDiagState = Global.DeviceIOSignal.OutputDiagState;
                        outputDiagState = ((outputDiagState & 0xF000) >> 2) | (outputDiagState & 0x03FF);

                        Int32 inputDiagState = ReceiveData[15] + ((UInt16)ReceiveData[16] << 8);          //读取输入诊断

                        lock (Global.LockMachineFaultState)//锁定故障信息保存，防止读写冲突
                        {
                            Global.MachineFaultState = (Global.MachineFaultState & 0xFFFFFFFFE001FFFF) | (((UInt64)inputDiagState) << 17);//标记输入故障
                            Global.MachineFaultState = (Global.MachineFaultState & 0xFFFFFFFFFFFE0007) | (((UInt64)outputDiagState) << 3);//标记输出故障
                            Global.MachineFaultState = (Global.MachineFaultState & 0xFFFFFF00FFFFFFFF) | (((UInt64)ReceiveData[17]) << 32);//标记编码器故障
                        }
                        break;
                    case 10:
                        DiagEnableChanged = false;
                        break;
                    case 12:
                        if (ReceiveData[2] >= 3)                           //上位机没有接收到的图像，相机故障
                        {
                            for (Byte i = 0; i < Global.CameraNumberMax; i++)//遍历当前所有相机
                            {
                                if (((Global.CameraChooseState & (0x01 << i)) != 0) && (false == Global.Camera[i].IsSerialPort))//当前相机开启
                                {
                                    if (CameraImageCount[i] == 0)             //上位机没有接收到图像，相机丢失
                                    {
                                        CameraLostState[i] = true;
                                    }
                                    else                                           //上位机正常接收到图像，数据清零
                                    {
                                        CameraImageCount[i] = 0;
                                        CameraLostNumber[i] = 0;
                                    }
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明： 串口控件接收到数据的处理函数
        // 输入参数：1、Byte：index，数组下表索引
        //                    2、Byte[][]：bSensorADCValue，光电数据
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _PhotoelectricityDataProcessing_89713FC(Byte index, Byte[][] bSensorADCValue)
        {
            Boolean bResultPort = true;
            Byte bErrorIndexPort = 0;

            try
            {
                for (Byte i = 0; i < bSensorADCValue.Length; i++) //读取实时ADC值
                {
                    if (Global.Camera[index].Tools[i].ToolState) //工具开启
                    {
                        if ((double)(bSensorADCValue[i][0]) * 5000 / 255 > Global.Camera[index].Tools[i].Max) //当前缺陷
                        {
                            bResultPort = false;
                            bErrorIndexPort = i;
                            break;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
            }

            if ((false == LabModel) && (false == CloseSerialPortFlag) && (false == Global.ComputerRunState))   //当前处于非实验室界面
            {
                lock (LockControllerSerialPort)
                {
                    _SendCommand(9, index, bResultPort, bErrorIndexPort);
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明： 串口控件接收到数据的处理函数
        // 输入参数：1、Byte：index，数组下表索引
        //                    2、Byte[][]：bSensorADCValue，光电数据
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _PhotoelectricityDataProcessing_89713CF(Byte index, Byte[][] bSensorADCValue)
        {
            Boolean bResultPort = true;
            Byte bErrorIndexPort = 0;

            try
            {
                for (Byte i = 0; i < bSensorADCValue.Length; i++) //读取实时ADC值
                {
                    if (Global.Camera[index].Tools[i].ToolState) //工具开启
                    {
                        double dPresion = (double)Global.Camera[index].Tools[i].ImageWidth / bSensorADCValue[i].Length;

                        Rectangle rect = VisionSystemClassLibrary.GeneralFunction._GetMinRect(Global.Camera[index].Tools[i].ROI.roiExtra);

                        Int32 startIndex = Convert.ToInt32(rect.Left / dPresion);
                        Int32 endIndex = Convert.ToInt32(rect.Right / dPresion);

                        double sum = 0.0;
                        for (Int32 j = startIndex; (j <= endIndex) && (j < bSensorADCValue[i].Length); j++)
                        {
                            sum += bSensorADCValue[i][j];
                        }
                        sum = sum * 5000 / 255 / (endIndex - startIndex + 1);

                        if ((sum < Global.Camera[index].Tools[i].Min) || (sum > Global.Camera[index].Tools[i].Max)) //当前缺陷
                        {
                            bResultPort = false;
                            bErrorIndexPort = i;
                            break;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
            }

            if ((false == LabModel) && (false == CloseSerialPortFlag) && (false == Global.ComputerRunState))   //当前处于非实验室界面
            {
                lock (LockControllerSerialPort)
                {
                    _SendCommand(9, index, bResultPort, bErrorIndexPort);
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明： 串口控件接收到数据的处理函数
        // 输入参数：1、Byte：index，数组下表索引
        //                    2、Byte[][]：bSensorADCValue，光电数据
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _PhotoelectricityDataProcessing_89713FA(Byte index, Byte[] bSensorADCValue)
        {
            Boolean bResultPort = true;
            Byte bErrorIndexPort = 0;

            try
            {
                for (Byte i = 0; i < bSensorADCValue.Length; i++) //读取实时ADC值
                {
                    if (Global.Camera[index].Tools[i].ToolState) //工具开启
                    {
                        if ((bSensorADCValue[i] * 5000 / 255) > Global.Camera[index].Tools[i].Max) //当前缺陷
                        {
                            bResultPort = false;
                            bErrorIndexPort = i;
                            break;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
            }

            if ((false == LabModel) && (false == CloseSerialPortFlag) && (false == Global.ComputerRunState))   //当前处于非实验室界面
            {
                lock (LockControllerSerialPort)
                {
                    _SendCommand(9, index, bResultPort, bErrorIndexPort);
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明： 串口控件接收到数据的处理函数
        // 输入参数：1、Byte：iIndex，相机索引
        //                    2、Byte[]：ReceiveData，串口数据
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _GetSerialPortCommucationBuffer(Byte iIndex, List<Byte> ReceiveData)
        {
            try
            {
                switch (ReceiveData[0])
                {
                    case 2:
                        switch (Global.Camera[iIndex].Sensor_ProductType)
                        {
                            case VisionSystemClassLibrary.Enum.SensorProductType._89713FC:
                                SensorAdjustResult[iIndex] = 0;
                                SensorAdjustResult[iIndex] |= ReceiveData[2];//烟支1-8校准结果标记

                                Int32 iTemp = ReceiveData[3];
                                SensorAdjustResult[iIndex] |= (iTemp << 8);//烟支9-16校准结果标记

                                iTemp = ReceiveData[4];
                                SensorAdjustResult[iIndex] |= ((iTemp & 0x0F) << 16);//烟支17-20校准结果标记

                                if ((ReceiveData[4] & 0x80) != 0)//校准过程标记，0表示校准中；1表示校准结束
                                {
                                    SensorAdjusting[iIndex] = 0;
                                }
                                else
                                {
                                    SensorAdjusting[iIndex] = 1;
                                }

                                for (Byte i = 0; i < Global.Camera[iIndex].DeviceParameter.SensorAdjustValue.Length; i++) //读取校准值
                                {
                                    Global.Camera[iIndex].DeviceParameter.SensorAdjustValue[i] = ReceiveData[i + 5];
                                }

                                for (Byte i = 0; i < SensorADCValue[iIndex].Length; i++) //读取实时ADC值
                                {
                                    SensorADCValue[iIndex][i][0] = ReceiveData[i + 25];
                                }

                                _serialPort_Available(iIndex, SensorADCValue[iIndex], 0, SensorAdjusting[iIndex], SensorAdjustResult[iIndex]);//光电数据处理
                                break;

                            case VisionSystemClassLibrary.Enum.SensorProductType._89713CF:
                                break;

                            case VisionSystemClassLibrary.Enum.SensorProductType._89713FA:
                                Byte[] bSensorADCValue = new Byte[Global.Camera[iIndex].Tools.Count];
                                for (Byte i = 0; i < bSensorADCValue.Length; i++)
                                {
                                    bSensorADCValue[i] = ReceiveData[2 + i];
                                }
                                _PhotoelectricityDataProcessing_89713FA(iIndex, bSensorADCValue);//发送处理结果
                                break;

                            case VisionSystemClassLibrary.Enum.SensorProductType._89750A:
                                break;

                            default:
                                break;
                        }
                        break;

                    case 3:
                        switch (Global.Camera[iIndex].Sensor_ProductType)
                        {
                            case VisionSystemClassLibrary.Enum.SensorProductType._89713FC:
                                Byte[][] sensorADCValueMaxTemp = new Byte[Global.Camera[iIndex].DeviceParameter.SensorADCValueMax.Length][];

                                for (Byte i = 0; i < Global.Camera[iIndex].DeviceParameter.SensorADCValueMax.Length; i++) //读取实时ADC值最大值
                                {
                                    sensorADCValueMaxTemp[i] = new Byte[1];
                                    sensorADCValueMaxTemp[i][iIndex] = ReceiveData[i + 2];
                                    Global.Camera[iIndex].DeviceParameter.SensorADCValueMax[i] = Convert.ToInt16((Double)(ReceiveData[i + 2]) * 5000 / 255);
                                }
                                _serialPort_Available(iIndex, sensorADCValueMaxTemp, 0, SensorAdjusting[iIndex], SensorAdjustResult[iIndex]);//光电数据处理

                                SensorADCChecking[iIndex] = 0;
                                break;

                            case VisionSystemClassLibrary.Enum.SensorProductType._89713FA:
                            case VisionSystemClassLibrary.Enum.SensorProductType._89713CF:
                                Int32 index = 2;
                                Byte sensorIndex = ReceiveData[index];
                                index++;

                                for (Byte i = 0; i < ReceiveData.Count - 6; i++, index++)   //传感器曲线图数据
                                {
                                    if (SensorADCValueIndex[iIndex][sensorIndex][0] < SensorADCValue[iIndex][sensorIndex].Length)
                                    {
                                        SensorADCValue[iIndex][sensorIndex][SensorADCValueIndex[iIndex][sensorIndex][0]] = ReceiveData[index];
                                        SensorADCValueIndex[iIndex][sensorIndex][0]++;
                                    }
                                    else
                                    {
                                        SensorADCValueIndex[iIndex][sensorIndex][0] = 0;

                                        if (sensorIndex == (SensorADCValueIndex[iIndex].Length - 1)) //所有光电管更新完毕
                                        {
                                            if (VisionSystemClassLibrary.Enum.SensorProductType._89713CF == Global.Camera[iIndex].Sensor_ProductType)//散包检测
                                            {
                                                _PhotoelectricityDataProcessing_89713CF(iIndex, SensorADCValue[iIndex]);//发送处理结果
                                            }
                                            _serialPort_Available(iIndex, SensorADCValue[iIndex], 1);//光电数据处理
                                        }
                                        break;
                                    }
                                }
                                break;

                            case VisionSystemClassLibrary.Enum.SensorProductType._89750A:
                                break;
                            default:
                                break;
                        }
                        break;

                    case 4:
                        switch (Global.Camera[iIndex].Sensor_ProductType)
                        {
                            case VisionSystemClassLibrary.Enum.SensorProductType._89713FC:
                                for (Byte i = 0; i < SensorADCValue[iIndex].Length; i++) //读取实时ADC值
                                {
                                    SensorADCValue[iIndex][i][0] = ReceiveData[i + 2];
                                }

                                _PhotoelectricityDataProcessing_89713FC(iIndex, SensorADCValue[iIndex]);//发送处理结果
                                _serialPort_Available(iIndex, SensorADCValue[iIndex], 1);//光电数据处理
                                break;

                            case VisionSystemClassLibrary.Enum.SensorProductType._89713FA:
                            case VisionSystemClassLibrary.Enum.SensorProductType._89713CF:
                                for (Byte i = 0; i < SensorADCValue[iIndex].Length; i++) //读取实时ADC值
                                {
                                    SensorADCValue[iIndex][i][0] = ReceiveData[i + 2];
                                }

                                _serialPort_Available(iIndex, SensorADCValue[iIndex], 0);//光电数据处理
                                break;

                            case VisionSystemClassLibrary.Enum.SensorProductType._89750A:
                                break;
                            default:
                                break;
                        }
                        break;

                    case 7:
                        SensorAdjustResult[iIndex] = 0;
                        SensorAdjustResult[iIndex] = ReceiveData[2];//烟支1-8校准结果标记

                        if ((ReceiveData[2] & 0x80) != 0)//校准过程标记，0表示校准中；1表示校准结束
                        {
                            SensorAdjusting[iIndex] = 0;
                            this.Invoke(new MethodInvoker(delegate { timer_89713FA.Enabled = false; }));//传感器手动校准
                        }
                        else
                        {
                            SensorAdjusting[iIndex] = 1;
                        }

                        for (Byte i = 0; i < Global.Camera[iIndex].DeviceParameter.SensorAdjustValue.Length; i++) //读取校准值
                        {
                            Global.Camera[iIndex].DeviceParameter.SensorAdjustValue[i] = ReceiveData[i + 3];
                        }

                        for (Byte i = 0; i < SensorADCValue[iIndex].Length; i++) //读取实时ADC值
                        {
                            SensorADCValue[iIndex][i][0] = ReceiveData[i + 8];
                        }

                        _serialPort_Available(iIndex, SensorADCValue[iIndex], 0, SensorAdjusting[iIndex], SensorAdjustResult[iIndex]);//光电数据处理
                        break;

                    case 8:
                        SensorAdjustResult[iIndex] = 0;
                        SensorAdjustResult[iIndex] = ReceiveData[2];//烟支1-8校准结果标记

                        SensorAdjusting[iIndex] = 1;

                        for (Byte i = 0; i < Global.Camera[iIndex].DeviceParameter.SensorAdjustValue.Length; i++) //读取校准值
                        {
                            Global.Camera[iIndex].DeviceParameter.SensorAdjustValue[i] = ReceiveData[i + 3];
                        }

                        for (Byte i = 0; i < SensorADCValue[iIndex].Length; i++) //读取实时ADC值
                        {
                            SensorADCValue[iIndex][i][0] = ReceiveData[i + 8];
                        }

                        _serialPort_Available(iIndex, SensorADCValue[iIndex], 0, SensorAdjusting[iIndex], SensorAdjustResult[iIndex]);//光电数据处理
                        break;

                    case 10:
                        Byte[][] sensorADCValueMaxTemp_89713FA = new Byte[Global.Camera[iIndex].DeviceParameter.SensorADCValueMax.Length][];

                        for (Byte i = 0; i < Global.Camera[iIndex].DeviceParameter.SensorADCValueMax.Length; i++) //读取实时ADC值最大值
                        {
                            sensorADCValueMaxTemp_89713FA[i] = new Byte[1];
                            sensorADCValueMaxTemp_89713FA[i][0] = ReceiveData[i + 2];
                            Global.Camera[iIndex].DeviceParameter.SensorADCValueMax[i] = Convert.ToInt16((Double)(ReceiveData[i + 2]) * 5000 / 255);
                        }
                        _serialPort_Available(iIndex, sensorADCValueMaxTemp_89713FA, 0, SensorAdjusting[iIndex], SensorAdjustResult[iIndex]);//光电数据处理

                        SensorADCChecking[iIndex] = 0;
                        break;

                    case 20:
                        Global.SerialPortSn[iIndex] = "";
                        for (Byte i = 2; i < 14; i++) //读取串口序列号
                        {
                            Global.SerialPortSn[iIndex] += ReceiveData[i].ToString();
                        }
                        break;

                    default:
                        break;
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
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
            try
            {
                while (SerialPortCommucation[0].BytesToRead >= Global.Camera[0].SerialPort_ReceivedBytesThreshold) //串口1读取字节长度大于等于32
                {
                    Byte[] bReceiveData = new Byte[Global.Camera[0].SerialPort_ReceivedBytesThreshold];
                    SerialPortCommucation[0].Read(bReceiveData, 0, Global.Camera[0].SerialPort_ReceivedBytesThreshold);

                    Global.SerialPortCommucationBuffer[0].AddRange(bReceiveData); //加入缓冲区

                    while (true) //循环解析命令
                    {
                        if ((null != Global.SerialPortCommucationBuffer[0]) && (Global.SerialPortCommucationBuffer[0].Count > 2)) //最少包含命令类型、命令长度、校验位 
                        {
                            Byte bType = Global.SerialPortCommucationBuffer[0].First();

                            if (Global.SerialPortCommucationType[0].ContainsKey(bType)) //命令类型有效
                            {
                                Byte bValue = 0;
                                Global.SerialPortCommucationType[0].TryGetValue(bType, out bValue);

                                if (bValue == Global.SerialPortCommucationBuffer[0][1]) //命令长度有效
                                {
                                    if (Global.SerialPortCommucationBuffer[0].Count >= (bValue + 3)) //包含整条命令
                                    {
                                        Int32 iDataCount = bValue + 2;
                                        Byte bReceiveChecksum = 0;
                                        for (Byte i = 0; i < iDataCount; i++)      //读取串口数据，计算校验数据
                                        {
                                            bReceiveChecksum ^= Global.SerialPortCommucationBuffer[0][i];
                                        }

                                        if (bReceiveChecksum == Global.SerialPortCommucationBuffer[0][iDataCount]) //校验和相同
                                        {
                                            _GetSerialPortCommucationBuffer(0, Global.SerialPortCommucationBuffer[0].GetRange(0, bValue + 3));
                                            Global.SerialPortCommucationBuffer[0].RemoveRange(0, bValue + 3);
                                        }
                                        else //数据可能丢失，继续查询命令
                                        {
                                            Global.SerialPortCommucationBuffer[0].RemoveAt(0);
                                        }
                                    }
                                    else //数据可能丢失，继续查询命令
                                    {
                                        Global.SerialPortCommucationBuffer[0].RemoveAt(0);
                                    }
                                }
                                else //命令长度不匹配
                                {
                                    Global.SerialPortCommucationBuffer[0].RemoveAt(0);
                                }
                            }
                            else //命令类型不匹配
                            {
                                Global.SerialPortCommucationBuffer[0].RemoveAt(0);
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
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
            try
            {
                while (SerialPortCommucation[1].BytesToRead >= Global.Camera[1].SerialPort_ReceivedBytesThreshold) //串口1读取字节长度大于等于32
                {
                    Byte[] bReceiveData = new Byte[Global.Camera[1].SerialPort_ReceivedBytesThreshold];
                    SerialPortCommucation[1].Read(bReceiveData, 0, Global.Camera[1].SerialPort_ReceivedBytesThreshold);

                    Global.SerialPortCommucationBuffer[1].AddRange(bReceiveData); //加入缓冲区

                    while (true) //循环解析命令
                    {
                        if ((null != Global.SerialPortCommucationBuffer[1]) && (Global.SerialPortCommucationBuffer[1].Count > 2)) //最少包含命令类型、命令长度、校验位 
                        {
                            Byte bType = Global.SerialPortCommucationBuffer[1].First();

                            if (Global.SerialPortCommucationType[1].ContainsKey(bType)) //命令类型有效
                            {
                                Byte bValue = 0;
                                Global.SerialPortCommucationType[1].TryGetValue(bType, out bValue);

                                if (bValue == Global.SerialPortCommucationBuffer[1][1]) //命令长度有效
                                {
                                    if (Global.SerialPortCommucationBuffer[1].Count >= (bValue + 3)) //包含整条命令
                                    {
                                        Int32 iDataCount = bValue + 2;
                                        Byte bReceiveChecksum = 0;
                                        for (Byte i = 0; i < iDataCount; i++)      //读取串口数据，计算校验数据
                                        {
                                            bReceiveChecksum ^= Global.SerialPortCommucationBuffer[1][i];
                                        }

                                        if (bReceiveChecksum == Global.SerialPortCommucationBuffer[1][iDataCount]) //校验和相同
                                        {
                                            _GetSerialPortCommucationBuffer(1, Global.SerialPortCommucationBuffer[1].GetRange(0, bValue + 3));
                                            Global.SerialPortCommucationBuffer[1].RemoveRange(0, bValue + 3);
                                        }
                                        else //数据可能丢失，继续查询命令
                                        {
                                            Global.SerialPortCommucationBuffer[1].RemoveAt(0);
                                        }
                                    }
                                    else //数据可能丢失，继续查询命令
                                    {
                                        Global.SerialPortCommucationBuffer[1].RemoveAt(0);
                                    }
                                }
                                else //命令长度不匹配
                                {
                                    Global.SerialPortCommucationBuffer[1].RemoveAt(0);
                                }
                            }
                            else //命令类型不匹配
                            {
                                Global.SerialPortCommucationBuffer[1].RemoveAt(0);
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
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
            try
            {
                while (SerialPortCommucation[2].BytesToRead >= Global.Camera[2].SerialPort_ReceivedBytesThreshold) //串口1读取字节长度大于等于32
                {
                    Byte[] bReceiveData = new Byte[Global.Camera[2].SerialPort_ReceivedBytesThreshold];
                    SerialPortCommucation[2].Read(bReceiveData, 0, Global.Camera[2].SerialPort_ReceivedBytesThreshold);

                    Global.SerialPortCommucationBuffer[2].AddRange(bReceiveData); //加入缓冲区

                    while (true) //循环解析命令
                    {
                        if ((null != Global.SerialPortCommucationBuffer[2]) && (Global.SerialPortCommucationBuffer[2].Count > 2)) //最少包含命令类型、命令长度、校验位 
                        {
                            Byte bType = Global.SerialPortCommucationBuffer[2].First();

                            if (Global.SerialPortCommucationType[2].ContainsKey(bType)) //命令类型有效
                            {
                                Byte bValue = 0;
                                Global.SerialPortCommucationType[2].TryGetValue(bType, out bValue);

                                if (bValue == Global.SerialPortCommucationBuffer[2][1]) //命令长度有效
                                {
                                    if (Global.SerialPortCommucationBuffer[2].Count >= (bValue + 3)) //包含整条命令
                                    {
                                        Int32 iDataCount = bValue + 2;
                                        Byte bReceiveChecksum = 0;
                                        for (Byte i = 0; i < iDataCount; i++)      //读取串口数据，计算校验数据
                                        {
                                            bReceiveChecksum ^= Global.SerialPortCommucationBuffer[2][i];
                                        }

                                        if (bReceiveChecksum == Global.SerialPortCommucationBuffer[2][iDataCount]) //校验和相同
                                        {
                                            _GetSerialPortCommucationBuffer(2, Global.SerialPortCommucationBuffer[2].GetRange(0, bValue + 3));
                                            Global.SerialPortCommucationBuffer[2].RemoveRange(0, bValue + 3);
                                        }
                                        else //数据可能丢失，继续查询命令
                                        {
                                            Global.SerialPortCommucationBuffer[2].RemoveAt(0);
                                        }
                                    }
                                    else //数据可能丢失，继续查询命令
                                    {
                                        Global.SerialPortCommucationBuffer[2].RemoveAt(0);
                                    }
                                }
                                else //命令长度不匹配
                                {
                                    Global.SerialPortCommucationBuffer[2].RemoveAt(0);
                                }
                            }
                            else //命令类型不匹配
                            {
                                Global.SerialPortCommucationBuffer[2].RemoveAt(0);
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
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
            try
            {
                while (SerialPortCommucation[3].BytesToRead >= Global.Camera[3].SerialPort_ReceivedBytesThreshold) //串口1读取字节长度大于等于32
                {
                    Byte[] bReceiveData = new Byte[Global.Camera[3].SerialPort_ReceivedBytesThreshold];
                    SerialPortCommucation[3].Read(bReceiveData, 0, Global.Camera[3].SerialPort_ReceivedBytesThreshold);

                    Global.SerialPortCommucationBuffer[3].AddRange(bReceiveData); //加入缓冲区

                    while (true) //循环解析命令
                    {
                        if ((null != Global.SerialPortCommucationBuffer[3]) && (Global.SerialPortCommucationBuffer[3].Count > 2)) //最少包含命令类型、命令长度、校验位 
                        {
                            Byte bType = Global.SerialPortCommucationBuffer[3].First();

                            if (Global.SerialPortCommucationType[3].ContainsKey(bType)) //命令类型有效
                            {
                                Byte bValue = 0;
                                Global.SerialPortCommucationType[3].TryGetValue(bType, out bValue);

                                if (bValue == Global.SerialPortCommucationBuffer[3][1]) //命令长度有效
                                {
                                    if (Global.SerialPortCommucationBuffer[3].Count >= (bValue + 3)) //包含整条命令
                                    {
                                        Int32 iDataCount = bValue + 2;
                                        Byte bReceiveChecksum = 0;
                                        for (Byte i = 0; i < iDataCount; i++)      //读取串口数据，计算校验数据
                                        {
                                            bReceiveChecksum ^= Global.SerialPortCommucationBuffer[3][i];
                                        }

                                        if (bReceiveChecksum == Global.SerialPortCommucationBuffer[3][iDataCount]) //校验和相同
                                        {
                                            _GetSerialPortCommucationBuffer(3, Global.SerialPortCommucationBuffer[3].GetRange(0, bValue + 3));
                                            Global.SerialPortCommucationBuffer[3].RemoveRange(0, bValue + 3);
                                        }
                                        else //数据可能丢失，继续查询命令
                                        {
                                            Global.SerialPortCommucationBuffer[3].RemoveAt(0);
                                            serialPortError1[3]++;
                                        }
                                    }
                                    else //数据可能丢失，继续查询命令
                                    {
                                        Global.SerialPortCommucationBuffer[3].RemoveAt(0);
                                        serialPortError1[2]++;
                                    }
                                }
                                else //命令长度不匹配
                                {
                                    Global.SerialPortCommucationBuffer[3].RemoveAt(0);
                                    serialPortError1[1]++;
                                }
                            }
                            else //命令类型不匹配
                            {
                                Global.SerialPortCommucationBuffer[3].RemoveAt(0);
                                serialPortError1[0]++;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("1:" + ex.ToString());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：班次切换时的事件
        // 输入参数：1.sender：ControlNetClient控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _ShiftChange(object sender, EventArgs e)
        {
            for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
            {
                if ((Global.CameraChooseState & (0x01 << index)) != 0)//当前相机开启
                {
                    Boolean bContainsKey = true;
                    lock (LockCheck_CameraPort)
                    {
                        bContainsKey = Global.Check_CameraPort.ContainsKey(Global.Camera[index].Type);
                    }

                    if (bContainsKey) //相机有效
                    {
                        lock (LockStaticRejectImageSavePort[index])
                        {
                            Global.ShiftInformation._WriteCurrentShiftOldInformation(Global.Camera[index].DeviceInformation.Port, Global.ShiftInformation.CurrentShiftIndexOld);
                        }
                    }
                }
                else
                {
                    Thread.Sleep(100);
                }
            }
        }

        //以太网通信

        //----------------------------------------------------------------------
        // 功能说明：连接服务端
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _NetClient_Connect()
        {
            while (true)
            {
                Global.NetClient._Connect();
                if (Global.NetClient.Connected)//已连接
                {
                    if (Global.ShowInformation)//显示调试信息
                    {
                        label3.Invoke(new EventHandler(delegate { label3.Text = "连接"; }));
                    }
                    break;//停止线程
                }
                else//未连接，继续连接服务器
                {
                    Thread.Sleep(1000);
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：网络数据反序列化
        // 输入参数：1.clientData：ControlNetClient控件自身的引用
        // 输出参数：无
        // 返回值：1.VisionSystemCommunicationLibrary.Ethernet.SerializableData，返回反序列化后数组
        //----------------------------------------------------------------------
        private VisionSystemCommunicationLibrary.Ethernet.SerializableData _EthernetDataDeserialize(VisionSystemCommunicationLibrary.Ethernet.DataReceivedEventArgs clientData)
        {
            VisionSystemCommunicationLibrary.Ethernet.SerializableData EthernetData = new VisionSystemCommunicationLibrary.Ethernet.SerializableData();

            try
            {
                MemoryStream MemoryStream = new MemoryStream();//流对象

                Int32 Length = BitConverter.ToInt32(clientData.ReceivedData, clientData.DataInfo.InstructionIndex + 1);//序列化数据长度
                MemoryStream.Write(clientData.ReceivedData, clientData.DataInfo.InstructionIndex + 1 + (BitConverter.GetBytes(Length)).Length, Length);//序列化数据写入流

                IFormatter Formatter = new SoapFormatter();//格式化对象
                MemoryStream.Position = 0;
                EthernetData = (VisionSystemCommunicationLibrary.Ethernet.SerializableData)Formatter.Deserialize(MemoryStream);//反序列化
            }
            catch (System.Exception ex)
            {

            }
            return EthernetData;
        }

        //----------------------------------------------------------------------
        // 功能说明：接收到一帧完整的数据时的事件
        // 输入参数：1.sender：ControlNetClient控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _NetClient_DataReceived(object sender, EventArgs e)
        {
            VisionSystemCommunicationLibrary.Ethernet.ClientControl ClientControl = (VisionSystemCommunicationLibrary.Ethernet.ClientControl)sender;//ControlNetServer控件（实际使用中可忽略该变量值）

            VisionSystemCommunicationLibrary.Ethernet.DataReceivedEventArgs ClientData = (VisionSystemCommunicationLibrary.Ethernet.DataReceivedEventArgs)e;//事件参数（重要）

            //操作

            if (VisionSystemCommunicationLibrary.Ethernet.NetDataType.DeviceInfo == ClientData.DataInfo.DataType)//接收的数据为指令，设备信息
            {
                _SendDeviceInformation();//向服务端发送客户端设备信息

                if (Global.ShowInformation)//显示调试信息
                {
                    String text1 = "", text2 = "";
                    for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                    {
                        if ((Global.CameraChooseState & (0x01 << index)) != 0)//当前相机开启
                        {
                            text1 += Global.Camera[index].Type.ToString() + "/";
                            text2 += Global.Camera[index].DeviceInformation.Port.ToString() + "/";
                        }
                    }
                    label24.Invoke(new EventHandler(delegate { label24.Text = text1; }));
                    label26.Invoke(new EventHandler(delegate { label26.Text = text2; }));
                }
            }
            else if (VisionSystemCommunicationLibrary.Ethernet.NetDataType.Instruction == ClientData.DataInfo.DataType)//接收的数据为指令，设备信息外的一般指令
            {
                switch ((CommunicationInstructionType)ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex])
                {
                    case CommunicationInstructionType.NetCheck_ConnectCamera:
                        //服务端->客户端：指令类型 + 相机类型数据

                        for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                        {
                            if ((VisionSystemClassLibrary.Enum.CameraType)(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1]) == Global.Camera[index].Type)//相机类型相同
                            {
                                //客户端->服务端：指令类型 + 相机类型数据 + 相机在线状态（1，成功；0，失败）

                                Byte[] Data_NetCheck_ConnectCamera = _GenerateInstruction(ClientData, BitConverter.GetBytes((Int32)1));//生成客户端数据
                                ClientControl._Send(Data_NetCheck_ConnectCamera);//发送数据

                                break;
                            }
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.Load:

                        //未完成文件发送
                        //服务端->客户端（数据）：指令类型 + 相机类型数据 + 文件传输状态（1，启动发送；2，文件发送中，文件索引值（从2开始））
                        for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                        {
                            if ((Global.CameraChooseState & (0x01 << index)) != 0)//当前相机开启
                            {
                                if ((VisionSystemClassLibrary.Enum.CameraType)(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1]) == Global.Camera[index].Type)//相机类型相同
                                {
                                    //客户端->服务端（文件）：指令类型 + 相机类型数据 + 文件传输状态（1，启动发送；2，文件发送中，文件索引值（从2开始）） + 文件
                                    //客户端->服务端：指令类型 + 相机类型数据 + 传输结果（1，成功；0，失败）

                                    String dirString = Global.Camera[index].DataPath;
                                    String dirString1 = Global.Camera[index].SampleImagePath;

                                    Byte[] Data_Load = new Byte[ClientData.DataInfo.InstructionLength];//生成命令数据
                                    Array.Copy(ClientData.ReceivedData, ClientData.DataInfo.InstructionIndex, Data_Load, 0, ClientData.DataInfo.InstructionLength);//生成客户端返回数据

                                    if (1 == BitConverter.ToInt32(ClientData.ReceivedData, ClientData.DataInfo.InstructionIndex + 2))//当前文件处于启动发送过程中
                                    {
                                        BitConverter.GetBytes(2).CopyTo(Data_Load, Data_Load.Length - 4);//生成发送文件索引
                                        ClientControl._Send(VisionSystemClassLibrary.Class.Camera.TolerancesFileName, dirString + VisionSystemClassLibrary.Class.Camera.TolerancesFileName, Data_Load);//发送Tolerances文件
                                    }
                                    else if (2 == BitConverter.ToInt32(ClientData.ReceivedData, ClientData.DataInfo.InstructionIndex + 2))//当前文件处于启动发送过程中
                                    {
                                        BitConverter.GetBytes(3).CopyTo(Data_Load, Data_Load.Length - 4);//生成发送文件索引
                                        ClientControl._Send(VisionSystemClassLibrary.Class.Camera.ToolFileName, dirString + VisionSystemClassLibrary.Class.Camera.ToolFileName, Data_Load);//发送Tool文件
                                    }
                                    else if (3 == BitConverter.ToInt32(ClientData.ReceivedData, ClientData.DataInfo.InstructionIndex + 2))//当前文件处于启动发送过程中
                                    {
                                        BitConverter.GetBytes(4).CopyTo(Data_Load, Data_Load.Length - 4);//生成发送文件索引
                                        ClientControl._Send(VisionSystemClassLibrary.Class.Camera.ParameterFileName, dirString + VisionSystemClassLibrary.Class.Camera.ParameterFileName, Data_Load);//发送Tool文件
                                    }
                                    else if (4 == BitConverter.ToInt32(ClientData.ReceivedData, ClientData.DataInfo.InstructionIndex + 2))//当前文件处于启动发送过程中
                                    {
                                        BitConverter.GetBytes(5).CopyTo(Data_Load, Data_Load.Length - 4);//生成发送文件索引
                                        ClientControl._Send(VisionSystemClassLibrary.Class.Camera.SampleDataFileName, dirString1 + VisionSystemClassLibrary.Class.Camera.SampleDataFileName, Data_Load);//发送Tool文件
                                    }
                                    else if (5 == BitConverter.ToInt32(ClientData.ReceivedData, ClientData.DataInfo.InstructionIndex + 2))//当前文件处于启动发送过程中
                                    {
                                        BitConverter.GetBytes(6).CopyTo(Data_Load, Data_Load.Length - 4);//生成发送文件索引
                                        ClientControl._Send(VisionSystemClassLibrary.Class.Camera.SampleImageFileName + VisionSystemClassLibrary.Class.Camera.BMPFile, dirString1 + VisionSystemClassLibrary.Class.Camera.SampleImageFileName + VisionSystemClassLibrary.Class.Camera.BMPFile, Data_Load);//发送Tool文件
                                    }
                                    else//发送常规模块文件
                                    {
                                        if (Global.Camera[index].DeepLearningState) //含有深度学习模块
                                        {
                                            if (6 == BitConverter.ToInt32(ClientData.ReceivedData, ClientData.DataInfo.InstructionIndex + 2))//当前文件处于启动发送过程中
                                            {
                                                BitConverter.GetBytes(7).CopyTo(Data_Load, Data_Load.Length - 4);//生成发送文件索引
                                                ClientControl._Send(VisionSystemClassLibrary.Class.Camera.ClassesFile, dirString + VisionSystemClassLibrary.Class.Camera.ClassesFile, Data_Load);//发送Model（XML）文件
                                            }
                                            else if (7 == BitConverter.ToInt32(ClientData.ReceivedData, ClientData.DataInfo.InstructionIndex + 2))//当前文件处于启动发送过程中
                                            {
                                                BitConverter.GetBytes(8).CopyTo(Data_Load, Data_Load.Length - 4);//生成发送文件索引
                                                ClientControl._Send(VisionSystemClassLibrary.Class.Camera.ModelFileName, dirString + VisionSystemClassLibrary.Class.Camera.ModelFileName, Data_Load);//发送Model（BP）文件
                                            }
                                            else if (8 == BitConverter.ToInt32(ClientData.ReceivedData, ClientData.DataInfo.InstructionIndex + 2))//当前文件全部发送完毕
                                            {
                                                BitConverter.GetBytes(1).CopyTo(Data_Load, Data_Load.Length - 4);//返回传输结果
                                                ClientControl._Send(Data_Load);//发送数据
                                            }
                                            else//发送文件失败
                                            {
                                                BitConverter.GetBytes(0).CopyTo(Data_Load, Data_Load.Length - 4);//返回传输结果
                                                ClientControl._Send(Data_Load);//发送数据
                                            }
                                        }
                                        else//常规模块
                                        {
                                            if (6 == BitConverter.ToInt32(ClientData.ReceivedData, ClientData.DataInfo.InstructionIndex + 2))//当前文件全部发送完毕
                                            {
                                                BitConverter.GetBytes(1).CopyTo(Data_Load, Data_Load.Length - 4);//返回传输结果
                                                ClientControl._Send(Data_Load);//发送数据
                                            }
                                            else//发送文件失败
                                            {
                                                BitConverter.GetBytes(0).CopyTo(Data_Load, Data_Load.Length - 4);//返回传输结果
                                                ClientControl._Send(Data_Load);//发送数据
                                            }
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.DevicesSetup_ResetDevice:

                        //DEVICES SETUP页面，点击【RESET DEVICE】按钮，格式：
                        //服务器->客户端：指令类型 + 相机类型

                        for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                        {
                            if ((VisionSystemClassLibrary.Enum.CameraType)(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1]) == Global.Camera[index].Type)//相机类型相同
                            {
                                //客户端->服务端：指令类型 + 相机类型数据 + 复位设备结果（1，成功；0，失败）
                                Byte[] Data_DevicesSetup_ResetDevice = _GenerateInstruction(ClientData, BitConverter.GetBytes(1));//生成数据
                                ClientControl._Send(Data_DevicesSetup_ResetDevice);//发送数据

                                if (false == Global.ComputerRunState) //控制器上运行软件
                                {
                                    lock (LockControllerSerialPort)
                                    {
                                        _SendCommand(19);
                                    }
                                }

                                Thread.Sleep(Global.ImageDataTime);//循环数据传输休眠10ms

                                CloseSerialPortFlag = true;
                                CloseSerialPort_ComType = CommunicationInstructionType.DevicesSetup_ResetDevice;

                                if (false == timer3.Enabled) //开启定时器3，执行重启
                                {
                                    this.Invoke(new MethodInvoker(delegate { timer3.Enabled = true; }));
                                }
                                break;
                            }
                        }
                        break;
                    //
                    case CommunicationInstructionType.DevicesSetup_ConfigDevice:

                        //完成文件发送
                        //服务端->客户端：指令类型 + 相机类型数据 + 设置为的IP地址 + 设置为的相机类型数据 + 设置为的端口 + 设置为的模式 + 设置为的相机故障标记 + 设置为的相机旋转角度 + 相机颜色 + 相机分辨率 + 是否为串口 + 烟支排列类型 + 设置为的相机数据截取区域缩放 + 设置为的相机数据截取区域缩放后是否居中 + 设置为的相机数据截取区域粘贴区域 + 设置为的相机数据截取区域 + 镜像标记 + 传感器应用场景 + 相机关联信息（关联类型 + 关联数量（相机类型 + 工位索引））

                        Int32 IPAdress = -1;
                        Int32 iCameraChooseState_DevicesSetup_ConfigDevice = 0;
                        UInt64 uCameraFaultState = 0;
                        Boolean bCheckEnable;//相机检测使能
                        VisionSystemClassLibrary.Enum.CameraRotateAngle bCameraAngle = VisionSystemClassLibrary.Enum.CameraRotateAngle.Angle_0;//设置为的相机旋转角度
                        VisionSystemClassLibrary.Enum.VideoColor VideoColor_DevicesSetupConfigDevice = VisionSystemClassLibrary.Enum.VideoColor.RGB32;//设置为的相机颜色
                        VisionSystemClassLibrary.Enum.VideoResolution VideoResolution_DevicesSetupConfigDevice = VisionSystemClassLibrary.Enum.VideoResolution._744x480;//设置为的相机分辨率
                        Boolean IsSerialPort_DevicesSetupConfigDevice = false;//设置为的串口标记
                        VisionSystemClassLibrary.Enum.TobaccoSortType TobaccoSortType_DevicesSetupConfigDevice = 0;//设置为的烟支排列方式
                        Boolean bBitmapResize = false;//设置为的相机数据截取区域缩放
                        Boolean bBitmapCenter = false;//设置为的相机数据截取区域缩放后是否居中
                        Point pBitmapAxis = new Point();//设置为的相机数据截取区域粘贴区域
                        Rectangle rBitmapArea = new Rectangle();//设置为的相机数据截取区域
                        VisionSystemClassLibrary.Enum.CameraFlip bCameraFlip = 0;//镜像标记
                        VisionSystemClassLibrary.Enum.SensorProductType sSensorProductType = 0;//传感器应用场景
                        VisionSystemClassLibrary.Struct.RelevancyCameraInformation rRelevancyCameraInformation = new VisionSystemClassLibrary.Struct.RelevancyCameraInformation(); //相机关联信息
                        rRelevancyCameraInformation.RelevancyCameraInfo = new System.Collections.Generic.Dictionary<VisionSystemClassLibrary.Enum.CameraType, Byte>();

                        Int32 iIndex_DevicesSetup_ConfigDevice = 0;//临时变量
                        Int32 iValueLength = BitConverter.GetBytes((Int32)1).Length;//数据长度

                        iIndex_DevicesSetup_ConfigDevice = ClientData.DataInfo.InstructionIndex + 2;
                        IPAdress = BitConverter.ToInt32(ClientData.ReceivedData, iIndex_DevicesSetup_ConfigDevice);//解析设置为的IP地址
                        iIndex_DevicesSetup_ConfigDevice += iValueLength;
                        VisionSystemClassLibrary.Enum.CameraType CameraType = (VisionSystemClassLibrary.Enum.CameraType)BitConverter.ToInt32(ClientData.ReceivedData, iIndex_DevicesSetup_ConfigDevice);//解析设置为的相机类型数据
                        iIndex_DevicesSetup_ConfigDevice += iValueLength;
                        Int32 iCameraPort = BitConverter.ToInt32(ClientData.ReceivedData, iIndex_DevicesSetup_ConfigDevice);//设置为的相机端口
                        iIndex_DevicesSetup_ConfigDevice += iValueLength;
                        iCameraChooseState_DevicesSetup_ConfigDevice = BitConverter.ToInt32(ClientData.ReceivedData, iIndex_DevicesSetup_ConfigDevice);//设置为的相机模式
                        iIndex_DevicesSetup_ConfigDevice += iValueLength;
                        uCameraFaultState = BitConverter.ToUInt64(ClientData.ReceivedData, iIndex_DevicesSetup_ConfigDevice);//设置为的相机故障标记

                        iIndex_DevicesSetup_ConfigDevice += iValueLength * 2;
                        if (BitConverter.ToInt32(ClientData.ReceivedData, iIndex_DevicesSetup_ConfigDevice) != 0) //设置为的相机检测使能
                        {
                            bCheckEnable = true;
                        }
                        else
                        {
                            bCheckEnable = false;
                        }

                        iIndex_DevicesSetup_ConfigDevice += iValueLength;
                        bCameraAngle = (VisionSystemClassLibrary.Enum.CameraRotateAngle)BitConverter.ToInt32(ClientData.ReceivedData, iIndex_DevicesSetup_ConfigDevice);//设置为的相机旋转角度

                        iIndex_DevicesSetup_ConfigDevice += iValueLength;
                        VideoColor_DevicesSetupConfigDevice = (VisionSystemClassLibrary.Enum.VideoColor)BitConverter.ToInt32(ClientData.ReceivedData, iIndex_DevicesSetup_ConfigDevice);//设置为的相机颜色

                        iIndex_DevicesSetup_ConfigDevice += iValueLength;
                        VideoResolution_DevicesSetupConfigDevice = (VisionSystemClassLibrary.Enum.VideoResolution)BitConverter.ToInt32(ClientData.ReceivedData, iIndex_DevicesSetup_ConfigDevice);//设置为的相机分辨率

                        iIndex_DevicesSetup_ConfigDevice += iValueLength;
                        if (BitConverter.ToInt32(ClientData.ReceivedData, iIndex_DevicesSetup_ConfigDevice) != 0) //设置为的串口标记
                        {
                            IsSerialPort_DevicesSetupConfigDevice = true;
                        }
                        else
                        {
                            IsSerialPort_DevicesSetupConfigDevice = false;
                        }

                        iIndex_DevicesSetup_ConfigDevice += iValueLength;
                        TobaccoSortType_DevicesSetupConfigDevice = (VisionSystemClassLibrary.Enum.TobaccoSortType)BitConverter.ToInt32(ClientData.ReceivedData, iIndex_DevicesSetup_ConfigDevice);//设置为的烟支排列方式

                        iIndex_DevicesSetup_ConfigDevice += iValueLength;
                        if (BitConverter.ToInt32(ClientData.ReceivedData, iIndex_DevicesSetup_ConfigDevice) != 0) //设置为的相机数据截取区域缩放
                        {
                            bBitmapResize = true;
                        }
                        else
                        {
                            bBitmapResize = false;
                        }

                        iIndex_DevicesSetup_ConfigDevice += iValueLength;
                        if (BitConverter.ToInt32(ClientData.ReceivedData, iIndex_DevicesSetup_ConfigDevice) != 0) //设置为的相机数据截取区域缩放后是否居中
                        {
                            bBitmapCenter = true;
                        }
                        else
                        {
                            bBitmapCenter = false;
                        }

                        iIndex_DevicesSetup_ConfigDevice += iValueLength;
                        pBitmapAxis.X = BitConverter.ToInt32(ClientData.ReceivedData, iIndex_DevicesSetup_ConfigDevice);//设置为的相机数据截取区域粘贴区域X
                        iIndex_DevicesSetup_ConfigDevice += iValueLength;
                        pBitmapAxis.Y = BitConverter.ToInt32(ClientData.ReceivedData, iIndex_DevicesSetup_ConfigDevice);//设置为的相机数据截取区域粘贴区域Y
                        iIndex_DevicesSetup_ConfigDevice += iValueLength;
                        rBitmapArea.X = BitConverter.ToInt32(ClientData.ReceivedData, iIndex_DevicesSetup_ConfigDevice);//设置为的相机数据截取区域X
                        iIndex_DevicesSetup_ConfigDevice += iValueLength;
                        rBitmapArea.Y = BitConverter.ToInt32(ClientData.ReceivedData, iIndex_DevicesSetup_ConfigDevice);//设置为的相机数据截取区域Y
                        iIndex_DevicesSetup_ConfigDevice += iValueLength;
                        rBitmapArea.Width = BitConverter.ToInt32(ClientData.ReceivedData, iIndex_DevicesSetup_ConfigDevice);//设置为的相机数据截取区域W
                        iIndex_DevicesSetup_ConfigDevice += iValueLength;
                        rBitmapArea.Height = BitConverter.ToInt32(ClientData.ReceivedData, iIndex_DevicesSetup_ConfigDevice);//设置为的相机数据截取区域H
                        iIndex_DevicesSetup_ConfigDevice += iValueLength;
                        bCameraFlip = (VisionSystemClassLibrary.Enum.CameraFlip)BitConverter.ToInt32(ClientData.ReceivedData, iIndex_DevicesSetup_ConfigDevice);//设置为的相机镜像标记
                        iIndex_DevicesSetup_ConfigDevice += iValueLength;
                        sSensorProductType = (VisionSystemClassLibrary.Enum.SensorProductType)BitConverter.ToInt32(ClientData.ReceivedData, iIndex_DevicesSetup_ConfigDevice);//设置为的传感器应用场景
                        iIndex_DevicesSetup_ConfigDevice += iValueLength;

                        rRelevancyCameraInformation.rRelevancyType = (VisionSystemClassLibrary.Enum.RelevancyType)ClientData.ReceivedData[iIndex_DevicesSetup_ConfigDevice]; //相机关联类型

                        if (VisionSystemClassLibrary.Enum.RelevancyType.None < rRelevancyCameraInformation.rRelevancyType) //相机存在关联信息
                        {
                            iIndex_DevicesSetup_ConfigDevice += 1;
                            Byte iCount = ClientData.ReceivedData[iIndex_DevicesSetup_ConfigDevice];

                            for (Int32 i = 0; i < iCount; i++)  //循环所有关联相机
                            {
                                iIndex_DevicesSetup_ConfigDevice += 1;
                                VisionSystemClassLibrary.Enum.CameraType cameraType = (VisionSystemClassLibrary.Enum.CameraType)ClientData.ReceivedData[iIndex_DevicesSetup_ConfigDevice]; //关联相机类型
                                iIndex_DevicesSetup_ConfigDevice += 1;
                                Byte cameraPosition = ClientData.ReceivedData[iIndex_DevicesSetup_ConfigDevice]; //关联相机工位
                                rRelevancyCameraInformation.RelevancyCameraInfo.Add(cameraType, cameraPosition);
                            }
                        }

                        //

                        //完成文件发送
                        //客户端->服务端：指令类型 + 相机类型数据 + 设置为的IP地址 + 设置为的相机类型数据 + 设置为的端口 + 文件索引值（从0开始） + 文件传输状态（1，文件发送中；2，文件发送完成） + 配置结果（1，成功；0，失败）

                        Byte[] byteValue_1_DevicesSetup_ConfigDevice = new Byte[ClientData.DataInfo.InstructionLength];
                        System.Array.Copy(ClientData.ReceivedData, ClientData.DataInfo.InstructionIndex, byteValue_1_DevicesSetup_ConfigDevice, 0, ClientData.DataInfo.InstructionLength);

                        Byte[] byteValue_2_DevicesSetup_ConfigDevice = BitConverter.GetBytes((Int32)1);//文件索引值（从0开始）
                        Byte[] byteValue_3_DevicesSetup_ConfigDevice = BitConverter.GetBytes((Int32)2);//文件传输状态（1，文件发送中；2，文件发送完成）
                        Byte[] byteValue_4_DevicesSetup_ConfigDevice = BitConverter.GetBytes((Int32)1);//文件接收结果（1，成功；0，失败）

                        Byte[] Data_DevicesSetupConfigDevice = _GenerateInstruction(byteValue_1_DevicesSetup_ConfigDevice, byteValue_2_DevicesSetup_ConfigDevice, byteValue_3_DevicesSetup_ConfigDevice, byteValue_4_DevicesSetup_ConfigDevice);//生成客户端数据
                        ClientControl._Send(Data_DevicesSetupConfigDevice);//发送数据

                        //需考虑到端口情况
                        Global.Camera[iCameraPort].Type = CameraType;
                        Global.Camera[iCameraPort].CameraFaultState = uCameraFaultState;
                        Global.Camera[iCameraPort].CheckEnable = bCheckEnable;
                        Global.Camera[iCameraPort].CameraAngle = bCameraAngle;
                        Global.Camera[iCameraPort].VideoColor = VideoColor_DevicesSetupConfigDevice;
                        Global.Camera[iCameraPort].VideoResolution = VideoResolution_DevicesSetupConfigDevice;
                        Global.Camera[iCameraPort].IsSerialPort = IsSerialPort_DevicesSetupConfigDevice;
                        Global.Camera[iCameraPort].TobaccoSortType_E = TobaccoSortType_DevicesSetupConfigDevice;
                        Global.Camera[iCameraPort].BitmapLockBitsResize = bBitmapResize;
                        Global.Camera[iCameraPort].BitmapLockBitsCenter = bBitmapCenter;
                        Global.Camera[iCameraPort].BitmapLockBitsAxis = pBitmapAxis;
                        Global.Camera[iCameraPort].BitmapLockBitsArea = rBitmapArea;
                        Global.Camera[iCameraPort].CameraFlip = bCameraFlip;
                        Global.Camera[iCameraPort].Sensor_ProductType = sSensorProductType;
                        Global.Camera[iCameraPort].RelevancyCameraInfo = rRelevancyCameraInformation;
                        Global.Camera[iCameraPort]._Save_CameraType();
                        Global.Camera[iCameraPort]._SaveRelevancy();

                        if (_GetComputerNameRx() != Global.Camera[iCameraPort].CameraENGName)//计算机名发生更新
                        {
                            _SetComputerNameRx(Global.Camera[iCameraPort].CameraENGName);
                            ShutDown_DevicesSetup_ConfigDevice = true;
                        }

                        CommunicationCount_DevicesSetup_ConfigDevice |= (Byte)(0x01 << iCameraPort);

                        if (iCameraChooseState_DevicesSetup_ConfigDevice == CommunicationCount_DevicesSetup_ConfigDevice) //相机参数更新完毕
                        {
                            for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                            {
                                if ((CommunicationCount_DevicesSetup_ConfigDevice & (0x01 << index)) != 0)//当前相机开启
                                {
                                    _CameraFileCopy(index);//更新相机参数
                                }
                            }
                            VisionSystemClassLibrary.Class.Camera._WriteCameraChooseState(".\\", Convert.ToByte(iCameraChooseState_DevicesSetup_ConfigDevice));

                            CommunicationCount_DevicesSetup_ConfigDevice = 0;
                            iCameraChooseState_DevicesSetup_ConfigDevice = 0;

                            if ((!(IPAdress <= 0)) && (ClientControl.ClientIP != (VisionSystemClassLibrary.Class.System.DeviceIPAddress + IPAdress.ToString()))) //IP地址有效且未更新
                            {
                                VisionSystemCommunicationLibrary.Ethernet.Function._SetNetworkAdapter(VisionSystemClassLibrary.Class.System.DeviceIPAddress + IPAdress.ToString());//更改IP地址
                                ShutDown_DevicesSetup_ConfigDevice = true;
                            }
                        
                            CloseSerialPortFlag = true;
                            CloseSerialPort_ComType = CommunicationInstructionType.DevicesSetup_ConfigDevice;

                            if (false == timer3.Enabled) //开启定时器3，执行重启
                            {
                                this.Invoke(new MethodInvoker(delegate { timer3.Enabled = true; }));
                            }
                        }
                        break;
                    //
                    case CommunicationInstructionType.DevicesSetup_TestIOEnter:
                        //DEVICE SETUP页面操作（进入页面），格式：
                        //查询曲线图数据：服务器->客户端：指令类型 + 相机类型数据 

                        for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                        {
                            if ((Global.CameraChooseState & (0x01 << index)) != 0)//当前相机开启
                            {
                                if ((VisionSystemClassLibrary.Enum.CameraType)(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1]) == Global.Camera[index].Type)//相机类型相同
                                {
                                    //客户端->服务端：指令类型 + 相机类型数据 + 操作结果（1，成功；0，失败）

                                    LabModel = true;

                                    if (Global.Camera[index].IsSerialPort) //当前为串口
                                    {
                                        lock (LockSerialPort[index])
                                        {
                                            _SendCommand_SerialPortCommucation(1, index);//发送实验室状态
                                        }

                                        if (false == timer4.Enabled) //定时器4未打开，开启ADC查询
                                        {
                                            this.Invoke(new MethodInvoker(delegate { timer4.Enabled = true; }));
                                        }

                                        bSerialPortLabState[index] = true;
                                    }

                                    Byte[] Data_DevicesSetup_TestIOEnter = _GenerateInstruction(ClientData, BitConverter.GetBytes((Int32)1));//生成客户端数据
                                    ClientControl._Send(Data_DevicesSetup_TestIOEnter);//发送数据

                                    break;
                                }
                            }
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.DevicesSetup_TestIO:

                        //LOAD页面操作，格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 输出数据
                        for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                        {
                            if ((Global.CameraChooseState & (0x01 << index)) != 0)//当前相机开启
                            {
                                if ((VisionSystemClassLibrary.Enum.CameraType)(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1]) == Global.Camera[index].Type)//相机类型相同
                                {
                                    //客户端->服务端：指令类型 + 相机类型数据 + 输入数据 + 输出诊断

                                    Global.DeviceIOSignal.OutputState = BitConverter.ToUInt32(ClientData.ReceivedData, ClientData.DataInfo.InstructionIndex + 2);//解析输出数据

                                    Thread.Sleep(Global.ImageDataTime);//循环数据传输休眠10ms

                                    Byte[] Data_DevicesSetup_TestIO = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1], BitConverter.GetBytes(Global.DeviceIOSignal.InputState), BitConverter.GetBytes(Global.DeviceIOSignal.OutputDiagStateLab));//生成客户端数据
                                    ClientControl._Send(Data_DevicesSetup_TestIO);//发送数据

                                    break;
                                }
                            }
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.DevicesSetup_TestIOExit:

                        //退出TestIO页面操作，格式：
                        //服务端->客户端：指令类型 + 相机类型数据
                        for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                        {
                            if ((Global.CameraChooseState & (0x01 << index)) != 0)//当前相机开启
                            {
                                if ((VisionSystemClassLibrary.Enum.CameraType)(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1]) == Global.Camera[index].Type)//相机类型相同
                                {
                                    //客户端->服务端：指令类型 + 相机类型数据

                                    Global.DeviceIOSignal.OutputState = 0;
                                    LabModel = false;

                                    if (Global.Camera[index].IsSerialPort) //当前为串口
                                    {
                                        bSerialPortLabState[index] = false;

                                        lock (LockSerialPort[index])
                                        {
                                            _SendCommand_SerialPortCommucation(1, index);//发送实验室状态
                                        }

                                        this.Invoke(new MethodInvoker(delegate { timer4.Enabled = false; }));
                                        this.Invoke(new MethodInvoker(delegate { timer_89713FA.Enabled = false; }));
                                    }

                                    Byte[] Data_DevicesSetup_TestIO = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1]);//生成客户端数据
                                    ClientControl._Send(Data_DevicesSetup_TestIO);//发送数据

                                    break;
                                }
                            }
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.DevicesSetup_ConfigImage_Save:
                        //DEVICES SETUP页面，点击【Save】按钮，格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 是否保存数据（1，是；0，否）
                        for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                        {
                            if ((Global.CameraChooseState & (0x01 << index)) != 0)//当前相机开启
                            {
                                if ((VisionSystemClassLibrary.Enum.CameraType)(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1]) == Global.Camera[index].Type)//相机类型相同
                                {
                                    //客户端->服务端：指令类型 + 相机类型数据 + 是否保存数据（1，是；0，否） + 保存数据结果（1，成功；0，失败）
                                    Boolean bConfigImageSave = Convert.ToBoolean(BitConverter.ToInt32(ClientData.ReceivedData, ClientData.DataInfo.InstructionIndex + 2));//解析是否保存数据（1，是；0，否）

                                    if (!bConfigImageSave)//退出相机参数配置界面
                                    {
                                        LabModel = false;

                                        if (Global.Camera[index].IsSerialPort) //当前为串口
                                        {
                                            bSerialPortLabState[index] = false;

                                            lock (LockSerialPort[index])
                                            {
                                                _SendCommand_SerialPortCommucation(1, index);//发送工作状态
                                            }

                                            switch (Global.Camera[index].Sensor_ProductType)
                                            {
                                                case VisionSystemClassLibrary.Enum.SensorProductType._89713FC:
                                                    lock (LockSerialPort[index])
                                                    {
                                                        _SendCommand_SerialPortCommucation(3, index);
                                                    }
                                                    break;
                                                default:
                                                    break;
                                            }
                                            this.Invoke(new MethodInvoker(delegate { timer4.Enabled = false; }));
                                            this.Invoke(new MethodInvoker(delegate { timer_89713FA.Enabled = false; }));
                                        }
                                    }

                                    SensorAdjusting[index] = 0;//校准标记清零
                                    SensorADCChecking[index] = 255;//查找最大电压值标记清零

                                    ClientControl._Send(_GenerateInstruction(ClientData, BitConverter.GetBytes(_ConfigImageOperate(index, 2, bConfigImageSave))));//发送数据

                                    if (!bConfigImageSave)//退出相机参数配置界面
                                    {
                                        if (Global.Camera[index].IsSerialPort) //当前为串口
                                        {
                                            if (VisionSystemClassLibrary.Enum.SensorProductType._89713FA == Global.Camera[index].Sensor_ProductType)//执行89713FA检测
                                            {
                                                lock (LockSerialPort[index])
                                                {
                                                    _SendCommand_SerialPortCommucation(5, index);//发送实验室状态
                                                }
                                            }
                                        }
                                    }

                                    break;
                                }
                            }
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.DevicesSetup_ConfigImage_Focus:
                        //DEVICES SETUP页面，点击【Save】按钮，格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 聚焦参数
                        VisionSystemCommunicationLibrary.Ethernet.SerializableData dateFocusEthernetData = _EthernetDataDeserialize(ClientData);

                        for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                        {
                            if ((Global.CameraChooseState & (0x01 << index)) != 0)//当前相机开启
                            {
                                if ((VisionSystemClassLibrary.Enum.CameraType)(dateFocusEthernetData.Data_0[0]) == Global.Camera[index].Type)//相机类型相同
                                {
                                    //客户端->服务端：指令类型 + 相机类型数据 + 聚焦参数更新结果（1，成功；0，不成功）

                                    break;
                                }
                            }
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.DevicesSetup_ConfigImage_White:
                        //DEVICES SETUP页面，点击【Save】按钮，格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 白平衡参数
                        VisionSystemCommunicationLibrary.Ethernet.SerializableData dateWhiteEthernetData = _EthernetDataDeserialize(ClientData);

                        for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                        {
                            if ((Global.CameraChooseState & (0x01 << index)) != 0)//当前相机开启
                            {
                                if ((VisionSystemClassLibrary.Enum.CameraType)(dateWhiteEthernetData.Data_0[0]) == Global.Camera[index].Type)//相机类型相同
                                {
                                    //客户端->服务端：指令类型 + 相机类型数据 + 白平衡参数更新结果（1，成功；0，不成功）

                                    break;
                                }
                            }
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.DevicesSetup_ConfigImage_Parameter:
                        //DEVICES SETUP页面，点击【Save】按钮，格式：
                        //客户端->服务端：指令类型 + 相机类型数据 + 相机参数更新结果（1，成功；0，不成功） + 白平衡（红） + 白平衡（绿） + 白平衡（蓝）
                        for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                        {
                            if ((Global.CameraChooseState & (0x01 << index)) != 0)//当前相机开启
                            {
                                if ((VisionSystemClassLibrary.Enum.CameraType)(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1]) == Global.Camera[index].Type)//相机类型相同
                                {
                                    //客户端->服务端：指令类型 + 相机类型数据 + 相机参数更新结果（1，成功；0，不成功） + 白平衡（红） + 白平衡（绿） + 白平衡（蓝）
                                    Int32 iIndex = 2;
                                    Global.Camera[index].DeviceParameter.StroboTime = Convert.ToUInt16(BitConverter.ToInt32(ClientData.ReceivedData, ClientData.DataInfo.InstructionIndex + iIndex));//解析光照时间
                                    iIndex += 4;
                                    Global.Camera[index].DeviceParameter.StroboCurrent = Convert.ToUInt16(BitConverter.ToInt32(ClientData.ReceivedData, ClientData.DataInfo.InstructionIndex + iIndex));//解析光照强度
                                    iIndex += 4;
                                    Global.Camera[index].DeviceParameter.Gain = Convert.ToUInt16(BitConverter.ToInt32(ClientData.ReceivedData, ClientData.DataInfo.InstructionIndex + iIndex));//解析增益
                                    iIndex += 4;
                                    Global.Camera[index].DeviceParameter.ExposureTime = Convert.ToUInt16(BitConverter.ToInt32(ClientData.ReceivedData, ClientData.DataInfo.InstructionIndex + iIndex));//解析曝光时间
                                    iIndex += 4;
                                    Global.Camera[index].DeviceParameter.WhiteBalance = Convert.ToUInt16(BitConverter.ToInt32(ClientData.ReceivedData, ClientData.DataInfo.InstructionIndex + iIndex));//解析白平衡设置开关

                                    if (Global.Camera[index].DeviceParameter.WhiteBalance != 0) //白平衡选择手动模式
                                    {
                                        iIndex += 4;
                                        Global.Camera[index].DeviceParameter.WhiteBalance_Red = Convert.ToUInt16(BitConverter.ToInt32(ClientData.ReceivedData, ClientData.DataInfo.InstructionIndex + iIndex));//解析白平衡（红）
                                        iIndex += 4;
                                        Global.Camera[index].DeviceParameter.WhiteBalance_Green = Convert.ToUInt16(BitConverter.ToInt32(ClientData.ReceivedData, ClientData.DataInfo.InstructionIndex + iIndex));//解析白平衡（绿）
                                        iIndex += 4;
                                        Global.Camera[index].DeviceParameter.WhiteBalance_Blue = Convert.ToUInt16(BitConverter.ToInt32(ClientData.ReceivedData, ClientData.DataInfo.InstructionIndex + iIndex));//解析白平衡（蓝）
                                    }

                                    if (false == Global.Camera[index].IsSerialPort) //当前为相机
                                    {
                                        _UpdateCameraParameter(index);

                                        if (false == Global.ComputerRunState) //控制器上运行软件
                                        {
                                            lock (LockControllerSerialPort)
                                            {
                                                _SendCommand(5, index);
                                            }
                                        }

                                        if ((Global.Camera[index].DeviceParameter.WhiteBalance == 0) && Global.Camera[index].VideoFormat.StartsWith("RGB32")) //白平衡选择自动模式
                                        {
                                            Thread.Sleep(1500);
                                        }
                                    }

                                    if (Global.Camera[index].VideoFormat.StartsWith("RGB32")) //相机配置为彩色模式
                                    {
                                        Byte[] Data_DevicesSetup_ConfigImage_Parameter = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1], BitConverter.GetBytes(1), BitConverter.GetBytes(WhiteBalanceRedPort[index].Value), BitConverter.GetBytes(WhiteBalanceGreenPort[index].Value), BitConverter.GetBytes(WhiteBalanceBluePort[index].Value));//生成客户端数据
                                        ClientControl._Send(Data_DevicesSetup_ConfigImage_Parameter);//发送数据
                                    }
                                    else
                                    {
                                        Byte[] Data_DevicesSetup_ConfigImage_Parameter = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1], BitConverter.GetBytes(1), BitConverter.GetBytes(41), BitConverter.GetBytes(41), BitConverter.GetBytes(41));//生成客户端数据
                                        ClientControl._Send(Data_DevicesSetup_ConfigImage_Parameter);//发送数据
                                    }

                                    break;
                                }
                            }
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.DevicesSetup_ConfigSensor_Parameter:
                        //DEVICES SETUP页面，点击【Save】按钮，格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 烟支数量（N）+ 传感器校准选中状态 + 校准标记（0，未校准；1，校准中） + 烟支校准值（N支）

                        for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                        {
                            if ((Global.CameraChooseState & (0x01 << index)) != 0)//当前相机开启
                            {
                                if ((VisionSystemClassLibrary.Enum.CameraType)(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1]) == Global.Camera[index].Type)//相机类型相同
                                {
                                    //客户端->服务端：指令类型 + 相机类型数据 + 烟支数量（N）+ 传感器校准过程标记（1，校准过程中；0，校准结束或未校准、取消校准） + 相机参数更新结果（1，成功；0，不成功） + 烟支校准值（N支）
                                    Int32 iIndex = ClientData.DataInfo.InstructionIndex + 2;

                                    Int32 iCigaretteNumber = BitConverter.ToInt32(ClientData.ReceivedData, iIndex);//解析烟支数量
                                    iIndex += 4;

                                    Global.Camera[index].DeviceParameter.SensorSelectState = BitConverter.ToInt32(ClientData.ReceivedData, iIndex);//解析传感器校准选中状态
                                    iIndex += 4;

                                    Int32 iSensorAdjustState = BitConverter.ToInt32(ClientData.ReceivedData, iIndex);//解析校准标记
                                    iIndex += 4;

                                    Byte[] iSensorAdjustValue = new Byte[iCigaretteNumber];
                                    Array.Copy(ClientData.ReceivedData, iIndex, iSensorAdjustValue, 0, iCigaretteNumber);//解析传感器校准值

                                    if ((SensorAdjustState[index] == 0) && (iSensorAdjustState != 0))  //传感器校准，返回上位机校准电阻
                                    {
                                        SensorAdjustState[index] = iSensorAdjustState;

                                        bSerialPortLabState[index] = false;

                                        this.Invoke(new MethodInvoker(delegate { timer4.Enabled = false; })); ;//传感器校准，暂时取消ADC查询

                                        switch (Global.Camera[index].Sensor_ProductType)
                                        {
                                            case VisionSystemClassLibrary.Enum.SensorProductType._89713FC:
                                                lock (LockSerialPort[index])
                                                {
                                                    _SendCommand_SerialPortCommucation(2, index, true);
                                                }
                                                break;
                                            case VisionSystemClassLibrary.Enum.SensorProductType._89713CF:
                                                lock (LockSerialPort[index])
                                                {
                                                    _SendCommand_SerialPortCommucation(7, index, true);
                                                }
                                                break;
                                            case VisionSystemClassLibrary.Enum.SensorProductType._89713FA:
                                                lock (LockSerialPort[index])
                                                {
                                                    _SendCommand_SerialPortCommucation(7, index, false);
                                                }

                                                if (false == timer_89713FA.Enabled)//定时器89713FA为打开
                                                {
                                                    this.Invoke(new MethodInvoker(delegate { timer_89713FA.Enabled = true; }));//传感器手动校准
                                                }
                                                break;
                                            default:
                                                break;
                                        }

                                        SensorAdjusting[index] = 1;//传感器开始校准
                                        SensorADCChecking[index] = 255;//最大ADC查询标记恢复默认状态
                                    }
                                    else //非校准状态
                                    {
                                        if ((SensorAdjustState[index] != 0) && (iSensorAdjustState == 0))  //取消校准，且在校准过程中，发送传感器校准值
                                        {
                                            SensorAdjustState[index] = iSensorAdjustState;

                                            if (1 == SensorAdjusting[index])  //取消校准，且在校准过程中，发送传感器校准值
                                            {
                                                if (iSensorAdjustValue != Global.Camera[index].DeviceParameter.SensorAdjustValue) //传感器校准值发生变化
                                                {
                                                    for (Int32 i = 0; i < Global.Camera[index].DeviceParameter.SensorAdjustValue.Length; i++)
                                                    {
                                                        Global.Camera[index].DeviceParameter.SensorAdjustValue[i] = iSensorAdjustValue[i];
                                                    }
                                                }

                                                switch (Global.Camera[index].Sensor_ProductType)
                                                {
                                                    case VisionSystemClassLibrary.Enum.SensorProductType._89713FC:
                                                        lock (LockSerialPort[index])
                                                        {
                                                            _SendCommand_SerialPortCommucation(2, index, false);
                                                        }
                                                        break;
                                                    case VisionSystemClassLibrary.Enum.SensorProductType._89713CF:
                                                        lock (LockSerialPort[index])
                                                        {
                                                            _SendCommand_SerialPortCommucation(7, index, false);
                                                        }
                                                        break;
                                                    case VisionSystemClassLibrary.Enum.SensorProductType._89713FA:
                                                        this.Invoke(new MethodInvoker(delegate { timer_89713FA.Enabled = false; }));//传感器手动校准

                                                        lock (LockSerialPort[index])
                                                        {
                                                            _SendCommand_SerialPortCommucation(7, index, false);
                                                        }
                                                        break;
                                                    default:
                                                        break;
                                                }

                                                lock (LockSerialPort[index])
                                                {
                                                    _SendCommand_SerialPortCommucation(5, index);
                                                }
                                            }
                                            else
                                            {
                                                switch (Global.Camera[index].Sensor_ProductType)
                                                {
                                                    case VisionSystemClassLibrary.Enum.SensorProductType._89713FC:
                                                        lock (LockSerialPort[index])
                                                        {
                                                            _SendCommand_SerialPortCommucation(3, index);
                                                        }
                                                        break;
                                                    default:
                                                        break;
                                                }
                                            }

                                            SensorAdjusting[index] = 0;//校准标记清零

                                            if (false == timer4.Enabled)//定时器4为打开
                                            {
                                                this.Invoke(new MethodInvoker(delegate { timer4.Enabled = true; }));
                                            }
                                            bSerialPortLabState[index] = true;
                                        }
                                        else if ((SensorAdjustState[index] == 0) && (iSensorAdjustState == 0))  //未校准，检查传感器校准值
                                        {
                                            if (iSensorAdjustValue != Global.Camera[index].DeviceParameter.SensorAdjustValue) //传感器校准值发生变化
                                            {
                                                for (Int32 i = 0; i < Global.Camera[index].DeviceParameter.SensorAdjustValue.Length; i++)
                                                {
                                                    Global.Camera[index].DeviceParameter.SensorAdjustValue[i] = iSensorAdjustValue[i];
                                                }

                                                switch (Global.Camera[index].Sensor_ProductType)
                                                {
                                                    case VisionSystemClassLibrary.Enum.SensorProductType._89713FC:
                                                        lock (LockSerialPort[index])
                                                        {
                                                            _SendCommand_SerialPortCommucation(3, index);
                                                        }
                                                        break;
                                                    default:
                                                        break;
                                                }

                                                lock (LockSerialPort[index])
                                                {
                                                    _SendCommand_SerialPortCommucation(5, index);
                                                }
                                            }

                                            SensorAdjusting[index] = 0;//校准标记清零

                                            if (false == timer4.Enabled)//定时器4为打开
                                            {
                                                this.Invoke(new MethodInvoker(delegate { timer4.Enabled = true; }));
                                            }
                                            bSerialPortLabState[index] = true;
                                        }
                                    }

                                    Thread.Sleep(Global.ImageDataTime);//循环数据传输休眠10ms

                                    Byte[] Data_DevicesSetup_ConfigSensor_Parameter = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1], BitConverter.GetBytes(iCigaretteNumber), BitConverter.GetBytes((Int32)SensorAdjusting[index]), BitConverter.GetBytes(1), Global.Camera[index].DeviceParameter.SensorAdjustValue);//生成客户端数据
                                    ClientControl._Send(Data_DevicesSetup_ConfigSensor_Parameter);//发送数据

                                    break;
                                }
                            }
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.DevicesSetup_ConfigSensor_MaxADC:
                        //DEVICES SETUP页面，点击【Save】按钮，格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 烟支数量（N）

                        for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                        {
                            if ((Global.CameraChooseState & (0x01 << index)) != 0)//当前相机开启
                            {
                                if ((VisionSystemClassLibrary.Enum.CameraType)(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1]) == Global.Camera[index].Type)//相机类型相同
                                {
                                    //客户端->服务端：指令类型 + 相机类型数据 + 烟支数量（N）+ 最大电压查询标记（1，查询过程中；0，查询结束） + 最大电压值（N支）
                                    Int32 iCigaretteNumber = BitConverter.ToInt32(ClientData.ReceivedData, ClientData.DataInfo.InstructionIndex + 2);//解析烟支数量

                                    if (255 == SensorADCChecking[index]) //初始状态，未开启查询
                                    {
                                        switch (Global.Camera[index].Sensor_ProductType)
                                        {
                                            case VisionSystemClassLibrary.Enum.SensorProductType._89713FC:
                                                lock (LockSerialPort[index])
                                                {
                                                    _SendCommand_SerialPortCommucation(3, index, true);//查询最大电压值
                                                }
                                                break;
                                            case VisionSystemClassLibrary.Enum.SensorProductType._89713FA:
                                            case VisionSystemClassLibrary.Enum.SensorProductType._89713CF:
                                                lock (LockSerialPort[index])
                                                {
                                                    _SendCommand_SerialPortCommucation(10, index);//查询最大电压值
                                                }
                                                break;
                                            default:
                                                break;
                                        }
                                        
                                        SensorADCChecking[index] = 1;//查找最大电压值
                                    }

                                    Thread.Sleep(Global.ImageDataTime);//循环数据传输休眠10ms

                                    Byte[] Data_DevicesSetup_ConfigSensor_Parameter = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1], BitConverter.GetBytes(iCigaretteNumber), BitConverter.GetBytes((Int32)SensorADCChecking[index]), Global.Camera[index].DeviceParameter.SensorADCValueMax);//生成客户端数据
                                    ClientControl._Send(Data_DevicesSetup_ConfigSensor_Parameter);//发送数据

                                    if (0 == SensorADCChecking[index]) //最大值查询结束，恢复原始状态
                                    {
                                        SensorADCChecking[index] = 255;
                                    }

                                    break;
                                }
                            }
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.DevicesSetup_ConfigImage_Enter:
                        //DEVICE SETUP页面操作（进入页面），格式：
                        //查询曲线图数据：服务器->客户端：指令类型 + 相机类型数据 
                        for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                        {
                            if ((Global.CameraChooseState & (0x01 << index)) != 0)//当前相机开启
                            {
                                if ((VisionSystemClassLibrary.Enum.CameraType)(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1]) == Global.Camera[index].Type)//相机类型相同
                                {
                                    //客户端->服务端：指令类型 + 相机类型数据 + 操作结果（1，成功；0，失败）

                                    SensorAdjustState[index] = 0;

                                    Byte[] Data_ConfigImage_Enter = _GenerateInstruction(ClientData, BitConverter.GetBytes(_ConfigImageOperate(index, 1)));//生成客户端数据
                                    ClientControl._Send(Data_ConfigImage_Enter);//发送数据

                                    break;
                                }
                            }
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.DevicesSetup_ConfigImage_Live:
                        //DEVICE SETUP页面操作（进入页面），格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 图像工具数据（1，包含工具；0，不包含工具） + 图像尺寸类型数据
                        
                        VisionSystemCommunicationLibrary.Ethernet.SerializableData configImageLiveEthernetData = _EthernetDataDeserialize(ClientData);

                        for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                        {
                            if ((Global.CameraChooseState & (0x01 << index)) != 0)//当前相机开启
                            {
                                if ((VisionSystemClassLibrary.Enum.CameraType)(configImageLiveEthernetData.Data_0[0]) == Global.Camera[index].Type)//相机类型相同
                                {
                                    //客户端->服务端：指令类型 + 相机类型数据 + 图像工具数据（1，包含工具；0，不包含工具） + 图像尺寸类型数据 + 图像宽度数据 + 图像高度数据 + 图像信息数据 + 图像数据

                                    Boolean originalView = !BitConverter.ToBoolean(configImageLiveEthernetData.Data_1, 0);//解析显示原始图像
                                    Double imageScale = BitConverter.ToDouble(configImageLiveEthernetData.Data_2, 0);//解析图像尺寸

                                    Byte[] Data_ConfigImage_Live;//待发送的数据

                                    if (VisionSystemClassLibrary.Enum.CameraState.OFF == Global.Camera[index].DeviceInformation.CAM)//相机关闭
                                    {
                                        Thread.Sleep(Global.ImageDataTime);//循环数据传输休眠10ms

                                        Data_ConfigImage_Live = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ref configImageLiveEthernetData, new VisionSystemClassLibrary.Struct.ImageInformation());//生成指令
                                        ClientControl._Send(Data_ConfigImage_Live);//发送数据
                                    }
                                    else//相机打开，相机未更新
                                    {
                                        try
                                        {
                                            lock (LockSourseImageBuffPort[index])
                                            {
                                                lock (LockSourseImageBuffPort[index])
                                                {
                                                    switch (SourseImageBuffIndexPort[index])                              //当前正在执行保存图像缓冲区索引
                                                    {
                                                        case 0:
                                                            if (SourseImageBuff0FlagPort[index])
                                                            {
                                                                workImage[index].SetZero();
                                                                workImage[index]._Or(SourseImageBuffPort[index][SourseImageBuffIndexPort[index]]);

                                                                Int32 iToolIndex = 0;
                                                                Boolean bResult = true;

                                                                _GetToolIndex(SourseImageBuffGraphicsInformationPort[index][SourseImageBuffIndexPort[index]], ref bResult, ref iToolIndex);

                                                                if (!originalView)//显示绘制效果图
                                                                {
                                                                    if (bResult) //结果完好
                                                                    {
                                                                        _DrawImageInformation(workImage[index], SourseImageBuffGraphicsInformationPort[index][SourseImageBuffIndexPort[index]][iToolIndex], SourseImageBuffToolInformationPort[index][SourseImageBuffIndexPort[index]][iToolIndex]);
                                                                    }
                                                                    else //检测缺陷
                                                                    {
                                                                        for (Int32 i = 0; i < SourseImageBuffGraphicsInformationPort[index][SourseImageBuffIndexPort[index]].Length; i++)
                                                                        {
                                                                            if (VisionSystemClassLibrary.Enum.ImageType.Error == SourseImageBuffGraphicsInformationPort[index][SourseImageBuffIndexPort[index]][i].Type)
                                                                            {
                                                                                _DrawImageInformation(workImage[index], SourseImageBuffGraphicsInformationPort[index][SourseImageBuffIndexPort[index]][i], SourseImageBuffToolInformationPort[index][SourseImageBuffIndexPort[index]][i]);
                                                                            }
                                                                        }
                                                                    }
                                                                }

                                                                MemoryStream mmemoryStream = new MemoryStream();
                                                                workImage[index].Bitmap.Save(mmemoryStream, ImageFormat.Jpeg);
                                                                Byte[] mmemoryStreamBytes = mmemoryStream.ToArray();

                                                                Thread.Sleep(Global.ImageDataTime);//循环数据传输休眠10ms\

                                                                Data_ConfigImage_Live = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ref configImageLiveEthernetData, workImage[index].Width, workImage[index].Height, SourseImageBuffGraphicsInformationPort[index][SourseImageBuffIndexPort[index]][iToolIndex], mmemoryStreamBytes);//生成指令
                                                                ClientControl._Send(Data_ConfigImage_Live);//发送数据
                                                            }

                                                            if (SourseImageBuff1FlagPort[index])
                                                            {
                                                                workImage[index].SetZero();
                                                                workImage[index]._Or(SourseImageBuffPort[index][1 - SourseImageBuffIndexPort[index]]);

                                                                Int32 iToolIndex = 0;
                                                                Boolean bResult = true;

                                                                _GetToolIndex(SourseImageBuffGraphicsInformationPort[index][1 - SourseImageBuffIndexPort[index]], ref bResult, ref iToolIndex);

                                                                if (!originalView)//显示绘制效果图
                                                                {
                                                                    if (bResult) //结果完好
                                                                    {
                                                                        _DrawImageInformation(workImage[index], SourseImageBuffGraphicsInformationPort[index][1 - SourseImageBuffIndexPort[index]][iToolIndex], SourseImageBuffToolInformationPort[index][1 - SourseImageBuffIndexPort[index]][iToolIndex]);
                                                                    }
                                                                    else //检测缺陷
                                                                    {
                                                                        for (Int32 i = 0; i < SourseImageBuffGraphicsInformationPort[index][1 - SourseImageBuffIndexPort[index]].Length; i++)
                                                                        {
                                                                            if (VisionSystemClassLibrary.Enum.ImageType.Error == SourseImageBuffGraphicsInformationPort[index][1 - SourseImageBuffIndexPort[index]][i].Type)
                                                                            {
                                                                                _DrawImageInformation(workImage[index], SourseImageBuffGraphicsInformationPort[index][1 - SourseImageBuffIndexPort[index]][i], SourseImageBuffToolInformationPort[index][1 - SourseImageBuffIndexPort[index]][i]);
                                                                            }
                                                                        }
                                                                    }
                                                                }

                                                                MemoryStream mmemoryStream = new MemoryStream();
                                                                workImage[index].Bitmap.Save(mmemoryStream, ImageFormat.Jpeg);
                                                                Byte[] mmemoryStreamBytes = mmemoryStream.ToArray();

                                                                Thread.Sleep(Global.ImageDataTime);//循环数据传输休眠10ms

                                                                Data_ConfigImage_Live = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ref configImageLiveEthernetData, workImage[index].Width, workImage[index].Height, SourseImageBuffGraphicsInformationPort[index][1 - SourseImageBuffIndexPort[index]][iToolIndex], mmemoryStreamBytes);//生成指令
                                                                ClientControl._Send(Data_ConfigImage_Live);//发送数据
                                                            }
                                                            break;
                                                        case 1:
                                                            if (SourseImageBuff1FlagPort[index])
                                                            {
                                                                workImage[index].SetZero();
                                                                workImage[index]._Or(SourseImageBuffPort[index][SourseImageBuffIndexPort[index]]);

                                                                Int32 iToolIndex = 0;
                                                                Boolean bResult = true;

                                                                _GetToolIndex(SourseImageBuffGraphicsInformationPort[index][SourseImageBuffIndexPort[index]], ref bResult, ref iToolIndex);

                                                                if (!originalView)//显示绘制效果图
                                                                {
                                                                    if (bResult) //结果完好
                                                                    {
                                                                        _DrawImageInformation(workImage[index], SourseImageBuffGraphicsInformationPort[index][SourseImageBuffIndexPort[index]][iToolIndex], SourseImageBuffToolInformationPort[index][SourseImageBuffIndexPort[index]][iToolIndex]);
                                                                    }
                                                                    else //检测缺陷
                                                                    {
                                                                        for (Int32 i = 0; i < SourseImageBuffGraphicsInformationPort[index][SourseImageBuffIndexPort[index]].Length; i++)
                                                                        {
                                                                            if (VisionSystemClassLibrary.Enum.ImageType.Error == SourseImageBuffGraphicsInformationPort[index][SourseImageBuffIndexPort[index]][i].Type)
                                                                            {
                                                                                _DrawImageInformation(workImage[index], SourseImageBuffGraphicsInformationPort[index][SourseImageBuffIndexPort[index]][i], SourseImageBuffToolInformationPort[index][SourseImageBuffIndexPort[index]][i]);
                                                                            }
                                                                        }
                                                                    }
                                                                }

                                                                MemoryStream mmemoryStream = new MemoryStream();
                                                                workImage[index].Bitmap.Save(mmemoryStream, ImageFormat.Jpeg);
                                                                Byte[] mmemoryStreamBytes = mmemoryStream.ToArray();

                                                                Thread.Sleep(Global.ImageDataTime);//循环数据传输休眠10ms

                                                                Data_ConfigImage_Live = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ref configImageLiveEthernetData, workImage[index].Width, workImage[index].Height, SourseImageBuffGraphicsInformationPort[index][SourseImageBuffIndexPort[index]][iToolIndex], mmemoryStreamBytes);//生成指令
                                                                ClientControl._Send(Data_ConfigImage_Live);//发送数据
                                                            }

                                                            if (SourseImageBuff0FlagPort[index])
                                                            {
                                                                workImage[index].SetZero();
                                                                workImage[index]._Or(SourseImageBuffPort[index][1 - SourseImageBuffIndexPort[index]]);

                                                                Int32 iToolIndex = 0;
                                                                Boolean bResult = true;

                                                                _GetToolIndex(SourseImageBuffGraphicsInformationPort[index][1 - SourseImageBuffIndexPort[index]], ref bResult, ref iToolIndex);

                                                                if (!originalView)//显示绘制效果图
                                                                {
                                                                    if (bResult) //结果完好
                                                                    {
                                                                        _DrawImageInformation(workImage[index], SourseImageBuffGraphicsInformationPort[index][1 - SourseImageBuffIndexPort[index]][iToolIndex], SourseImageBuffToolInformationPort[index][1 - SourseImageBuffIndexPort[index]][iToolIndex]);
                                                                    }
                                                                    else //检测缺陷
                                                                    {
                                                                        for (Int32 i = 0; i < SourseImageBuffGraphicsInformationPort[index][SourseImageBuffIndexPort[index]].Length; i++)
                                                                        {
                                                                            if (VisionSystemClassLibrary.Enum.ImageType.Error == SourseImageBuffGraphicsInformationPort[index][1 - SourseImageBuffIndexPort[index]][i].Type)
                                                                            {
                                                                                _DrawImageInformation(workImage[index], SourseImageBuffGraphicsInformationPort[index][1 - SourseImageBuffIndexPort[index]][i], SourseImageBuffToolInformationPort[index][1 - SourseImageBuffIndexPort[index]][i]);
                                                                            }
                                                                        }
                                                                    }
                                                                }

                                                                MemoryStream mmemoryStream = new MemoryStream();
                                                                workImage[index].Bitmap.Save(mmemoryStream, ImageFormat.Jpeg);
                                                                Byte[] mmemoryStreamBytes = mmemoryStream.ToArray();

                                                                Thread.Sleep(Global.ImageDataTime);//循环数据传输休眠10ms

                                                                Data_ConfigImage_Live = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ref configImageLiveEthernetData, workImage[index].Width, workImage[index].Height, SourseImageBuffGraphicsInformationPort[index][1 - SourseImageBuffIndexPort[index]][iToolIndex], mmemoryStreamBytes);//生成指令
                                                                ClientControl._Send(Data_ConfigImage_Live);//发送数据
                                                            }
                                                            break;
                                                        default:
                                                            Thread.Sleep(Global.ImageDataTime);//循环数据传输休眠10ms

                                                            Data_ConfigImage_Live = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ref configImageLiveEthernetData, new VisionSystemClassLibrary.Struct.ImageInformation());//生成指令
                                                            ClientControl._Send(Data_ConfigImage_Live);//发送数据
                                                            break;
                                                    }
                                                }
                                            }
                                        }
                                        catch (System.Exception ex)
                                        {
                                            Thread.Sleep(Global.ImageDataTime);//循环数据传输休眠10ms

                                            Data_ConfigImage_Live = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ref configImageLiveEthernetData, new VisionSystemClassLibrary.Struct.ImageInformation());//生成指令
                                            ClientControl._Send(Data_ConfigImage_Live);//发送数据
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.DevicesSetup_AlignDateTime:

                        //DEVICES SETUP页面，点击【ALIGN DATE/TIME】按钮，格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 日期时间数据
                        VisionSystemCommunicationLibrary.Ethernet.SerializableData dateEthernetData = _EthernetDataDeserialize(ClientData);

                        for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                        {
                            if ((Global.CameraChooseState & (0x01 << index)) != 0)//当前相机开启
                            {
                                if ((VisionSystemClassLibrary.Enum.CameraType)(dateEthernetData.Data_0[0]) == Global.Camera[index].Type)//相机类型相同
                                {
                                    //客户端->服务端：指令类型 + 相机类型数据 + 日期时间设置结果（1，成功；0，失败）

                                    MemoryStream lpSystemTimeMemoryStream = new MemoryStream();//初始化流对象
                                    lpSystemTimeMemoryStream.Write(dateEthernetData.Data_1, 0, dateEthernetData.Data_1.Length);//系统时间写入流

                                    IFormatter dateFormatter = new SoapFormatter();//格式化对象
                                    lpSystemTimeMemoryStream.Position = 0;
                                    VisionSystemClassLibrary.Struct.SYSTEMTIME lpSystemTime = (VisionSystemClassLibrary.Struct.SYSTEMTIME)dateFormatter.Deserialize(lpSystemTimeMemoryStream);//反序列化

                                    Boolean shiftReInit = false;
                                    DateTime dateTime = DateTime.Now;
                                    if ((lpSystemTime.Year != dateTime.Year) || (lpSystemTime.Month != dateTime.Month) || (lpSystemTime.Day != dateTime.Day)) //年月日发生改变
                                    {
                                        shiftReInit = true;
                                    }
                                    Byte[] Data_DevicesSetup_AlignDateTime = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], dateEthernetData.Data_0[0], BitConverter.GetBytes(SetLocalTime(ref lpSystemTime)));//生成客户端数据
                                    ClientControl._Send(Data_DevicesSetup_AlignDateTime);//发送数据

                                    if (shiftReInit) //执行班次重新初始化
                                    {
                                        lock (LockStaticRejectImageSavePort[index])
                                        {
                                            _reInitShift();
                                        }
                                    }

                                    break;
                                }
                            }
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.DevicesSetup_ParameterSettings:
                        //DEVICES SETUP页面，点击【PARAMETER SETTINGS】按钮，格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 参数个数（Int32） + 曝光相位（Int32） + 诊断相位（Int32） + 剔除相位（Int32） + 延时剔除烟包数（Int32）

                        for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                        {
                            if ((Global.CameraChooseState & (0x01 << index)) != 0)//当前相机开启
                            {
                                if ((VisionSystemClassLibrary.Enum.CameraType)(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1]) == Global.Camera[index].Type)//相机类型相同
                                {
                                    //客户端->服务端：指令类型 + 相机类型数据 + 设置结果（1，成功；0，失败）
                                    Int32 iIndex = 2;
                                    Int32 parameterNumber = BitConverter.ToInt32(ClientData.ReceivedData, ClientData.DataInfo.InstructionIndex + iIndex);

                                    for (Int32 i = 0; i < parameterNumber; i++) //遍历解析当前所有参数
                                    {
                                        iIndex += 4;
                                        Global.Camera[index].DeviceParameter.Parameter[i] = BitConverter.ToInt32(ClientData.ReceivedData, ClientData.DataInfo.InstructionIndex + iIndex);//解析曝光、诊断、剔除相位以及延时剔除烟包数
                                    }

                                    if (false == Global.ComputerRunState) //控制器上运行软件
                                    {
                                        lock (LockControllerSerialPort)
                                        {
                                            _SendCommand(16, index);
                                        }
                                    }

                                    Global.Camera[index]._SaveParameter();

                                    if (true == Global.Camera[index].IsSerialPort) //当前为串口
                                    {
                                        if (Global.Camera[index].ContinuousSampling) //连续采样
                                        {
                                            if ((null != Global.Camera[index].DeviceParameter.Parameter) && (Global.Camera[index].DeviceParameter.Parameter.Count > 3)) //连续采样数据有效
                                            {
                                                Int32 matrixLength = Global.Camera[index].EncoderPer * ((Global.Camera[index].DeviceParameter.Parameter[3] + 360 - Global.Camera[index].DeviceParameter.Parameter[2]) % 360) + 1;

                                                for (Byte j = 0; j < Global.Camera[index].SensorNumber; j++)//初始化实时图像、剔除图像信息缓冲区
                                                {
                                                    SensorADCValue[index][j] = new Byte[matrixLength];
                                                    SensorADCValueIndex[index][j] = new Int16[1];
                                                    SensorADCValueIndex[index][j][0] = 0;

                                                    for (Int32 k = 0; k < SensorADCValue[index][j].Length; k++)
                                                    {
                                                        SensorADCValue[index][j][k] = 0;
                                                    }
                                                }

                                                lock (LockSerialPort[index])
                                                {
                                                    _SendCommand_SerialPortCommucation(5, index);
                                                }
                                            }
                                        }
                                    }

                                    Byte[] Data_DevicesSetup_ParameterSettings = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1], BitConverter.GetBytes(1));//生成客户端数据
                                    ClientControl._Send(Data_DevicesSetup_ParameterSettings);//发送数据

                                    break;
                                }
                            }
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.Work:

                        //WORK，LIVE VIEW页面操作，格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 图像工具数据（1，包含工具；0，不包含工具） + 图像尺寸类型数据
                        
                        VisionSystemCommunicationLibrary.Ethernet.SerializableData workEthernetData = _EthernetDataDeserialize(ClientData);

                        for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                        {
                            if ((Global.CameraChooseState & (0x01 << index)) != 0)//当前相机开启
                            {
                                if ((VisionSystemClassLibrary.Enum.CameraType)(workEthernetData.Data_0[0]) == Global.Camera[index].Type)//相机类型相同
                                {
                                    //客户端->服务端：指令类型 + 相机类型数据 + 图像工具数据（1，包含工具；0，不包含工具） + 图像尺寸类型数据 + 图像宽度数据 + 图像高度数据  + 图像信息数据 + 图像数据

                                    Boolean originalView = !Convert.ToBoolean(BitConverter.ToInt32(workEthernetData.Data_1, 0));
                                    Double imageScale = BitConverter.ToDouble(workEthernetData.Data_2, 0);//解析图像尺寸

                                    Byte[] Data_Work = null;//待发送的数据

                                    if (VisionSystemClassLibrary.Enum.CameraState.OFF == Global.Camera[index].DeviceInformation.CAM)//相机关闭
                                    {
                                        MemoryStream mmemoryStream = new MemoryStream();
                                        ImageInit.Bitmap.Save(mmemoryStream, ImageFormat.Jpeg);
                                        Byte[] mmemoryStreamBytes = mmemoryStream.ToArray();

                                        Data_Work = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ref workEthernetData, ImageInit.Width, ImageInit.Height, new VisionSystemClassLibrary.Struct.ImageInformation(), mmemoryStreamBytes);//生成指令
                                        ClientControl._Send(Data_Work);//发送数据
                                    }
                                    else//相机打开，相机未更新
                                    {
                                        try
                                        {
                                            lock (LockSourseImageBuffPort[index])
                                            {
                                                switch (SourseImageBuffIndexPort[index])                              //当前正在执行保存图像缓冲区索引
                                                {
                                                    case 0:
                                                        if (SourseImageBuff0FlagPort[index])
                                                        {
                                                            workImage[index].SetZero();
                                                            workImage[index]._Or(SourseImageBuffPort[index][SourseImageBuffIndexPort[index]]);

                                                            Int32 iToolIndex = 0;
                                                            Boolean bResult = true;

                                                            _GetToolIndex(SourseImageBuffGraphicsInformationPort[index][SourseImageBuffIndexPort[index]], ref bResult, ref iToolIndex);

                                                            if (!originalView)//显示绘制效果图
                                                            {
                                                                if (bResult) //结果完好
                                                                {
                                                                    _DrawImageInformation(workImage[index], SourseImageBuffGraphicsInformationPort[index][SourseImageBuffIndexPort[index]][iToolIndex], SourseImageBuffToolInformationPort[index][SourseImageBuffIndexPort[index]][iToolIndex]);
                                                                }
                                                                else //检测缺陷
                                                                {
                                                                    for (Int32 i = 0; i < SourseImageBuffGraphicsInformationPort[index][SourseImageBuffIndexPort[index]].Length; i++)
                                                                    {
                                                                        if (VisionSystemClassLibrary.Enum.ImageType.Error == SourseImageBuffGraphicsInformationPort[index][SourseImageBuffIndexPort[index]][i].Type)
                                                                        {
                                                                            _DrawImageInformation(workImage[index], SourseImageBuffGraphicsInformationPort[index][SourseImageBuffIndexPort[index]][i], SourseImageBuffToolInformationPort[index][SourseImageBuffIndexPort[index]][i]);
                                                                        }
                                                                    }
                                                                }
                                                            }

                                                            MemoryStream mmemoryStream = new MemoryStream();
                                                            workImage[index].Bitmap.Save(mmemoryStream, ImageFormat.Jpeg);
                                                            Byte[] mmemoryStreamBytes = mmemoryStream.ToArray();

                                                            Data_Work = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ref workEthernetData, workImage[index].Width, workImage[index].Height, SourseImageBuffGraphicsInformationPort[index][SourseImageBuffIndexPort[index]][iToolIndex], mmemoryStreamBytes);//生成指令
                                                            ClientControl._Send(Data_Work);//发送数据
                                                        }

                                                        if (SourseImageBuff1FlagPort[index])
                                                        {
                                                            workImage[index].SetZero();
                                                            workImage[index]._Or(SourseImageBuffPort[index][1 - SourseImageBuffIndexPort[index]]);

                                                            Int32 iToolIndex = 0;
                                                            Boolean bResult = true;

                                                            _GetToolIndex(SourseImageBuffGraphicsInformationPort[index][1 - SourseImageBuffIndexPort[index]], ref bResult, ref iToolIndex);

                                                            if (!originalView)//显示绘制效果图
                                                            {
                                                                if (bResult) //结果完好
                                                                {
                                                                    _DrawImageInformation(workImage[index], SourseImageBuffGraphicsInformationPort[index][1 - SourseImageBuffIndexPort[index]][iToolIndex], SourseImageBuffToolInformationPort[index][1 - SourseImageBuffIndexPort[index]][iToolIndex]);
                                                                }
                                                                else //检测缺陷
                                                                {
                                                                    for (Int32 i = 0; i < SourseImageBuffGraphicsInformationPort[index][1 - SourseImageBuffIndexPort[index]].Length; i++)
                                                                    {
                                                                        if (VisionSystemClassLibrary.Enum.ImageType.Error == SourseImageBuffGraphicsInformationPort[index][1 - SourseImageBuffIndexPort[index]][i].Type)
                                                                        {
                                                                            _DrawImageInformation(workImage[index], SourseImageBuffGraphicsInformationPort[index][1 - SourseImageBuffIndexPort[index]][i], SourseImageBuffToolInformationPort[index][1 - SourseImageBuffIndexPort[index]][i]);
                                                                        }
                                                                    }
                                                                }
                                                            }

                                                            MemoryStream mmemoryStream = new MemoryStream();
                                                            workImage[index].Bitmap.Save(mmemoryStream, ImageFormat.Jpeg);
                                                            Byte[] mmemoryStreamBytes = mmemoryStream.ToArray();

                                                            Data_Work = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ref workEthernetData, workImage[index].Width, workImage[index].Height, SourseImageBuffGraphicsInformationPort[index][1 - SourseImageBuffIndexPort[index]][iToolIndex], mmemoryStreamBytes);//生成指令
                                                            ClientControl._Send(Data_Work);//发送数据
                                                        }
                                                        break;
                                                    case 1:
                                                        if (SourseImageBuff1FlagPort[index])
                                                        {
                                                            workImage[index].SetZero();
                                                            workImage[index]._Or(SourseImageBuffPort[index][SourseImageBuffIndexPort[index]]);

                                                            Int32 iToolIndex = 0;
                                                            Boolean bResult = true;

                                                            _GetToolIndex(SourseImageBuffGraphicsInformationPort[index][SourseImageBuffIndexPort[index]], ref bResult, ref iToolIndex);

                                                            if (!originalView)//显示绘制效果图
                                                            {
                                                                if (bResult) //结果完好
                                                                {
                                                                    _DrawImageInformation(workImage[index], SourseImageBuffGraphicsInformationPort[index][SourseImageBuffIndexPort[index]][iToolIndex], SourseImageBuffToolInformationPort[index][SourseImageBuffIndexPort[index]][iToolIndex]);
                                                                }
                                                                else //检测缺陷
                                                                {
                                                                    for (Int32 i = 0; i < SourseImageBuffGraphicsInformationPort[index][SourseImageBuffIndexPort[index]].Length; i++)
                                                                    {
                                                                        if (VisionSystemClassLibrary.Enum.ImageType.Error == SourseImageBuffGraphicsInformationPort[index][SourseImageBuffIndexPort[index]][i].Type)
                                                                        {
                                                                            _DrawImageInformation(workImage[index], SourseImageBuffGraphicsInformationPort[index][SourseImageBuffIndexPort[index]][i], SourseImageBuffToolInformationPort[index][SourseImageBuffIndexPort[index]][i]);
                                                                        }
                                                                    }
                                                                }
                                                            }

                                                            MemoryStream mmemoryStream = new MemoryStream();
                                                            workImage[index].Bitmap.Save(mmemoryStream, ImageFormat.Jpeg);
                                                            Byte[] mmemoryStreamBytes = mmemoryStream.ToArray();

                                                            Data_Work = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ref workEthernetData, workImage[index].Width, workImage[index].Height, SourseImageBuffGraphicsInformationPort[index][SourseImageBuffIndexPort[index]][iToolIndex], mmemoryStreamBytes);//生成指令
                                                            ClientControl._Send(Data_Work);//发送数据
                                                        }

                                                        if (SourseImageBuff0FlagPort[index])
                                                        {
                                                            workImage[index].SetZero();
                                                            workImage[index]._Or(SourseImageBuffPort[index][1 - SourseImageBuffIndexPort[index]]);

                                                            Int32 iToolIndex = 0;
                                                            Boolean bResult = true;

                                                            _GetToolIndex(SourseImageBuffGraphicsInformationPort[index][1 - SourseImageBuffIndexPort[index]], ref bResult, ref iToolIndex);

                                                            if (!originalView)//显示绘制效果图
                                                            {
                                                                if (bResult) //结果完好
                                                                {
                                                                    _DrawImageInformation(workImage[index], SourseImageBuffGraphicsInformationPort[index][1 - SourseImageBuffIndexPort[index]][iToolIndex], SourseImageBuffToolInformationPort[index][1 - SourseImageBuffIndexPort[index]][iToolIndex]);
                                                                }
                                                                else //检测缺陷
                                                                {
                                                                    for (Int32 i = 0; i < SourseImageBuffGraphicsInformationPort[index][1 - SourseImageBuffIndexPort[index]].Length; i++)
                                                                    {
                                                                        if (VisionSystemClassLibrary.Enum.ImageType.Error == SourseImageBuffGraphicsInformationPort[index][1 - SourseImageBuffIndexPort[index]][i].Type)
                                                                        {
                                                                            _DrawImageInformation(workImage[index], SourseImageBuffGraphicsInformationPort[index][1 - SourseImageBuffIndexPort[index]][i], SourseImageBuffToolInformationPort[index][1 - SourseImageBuffIndexPort[index]][i]);
                                                                        }
                                                                    }
                                                                }
                                                            }

                                                            MemoryStream mmemoryStream = new MemoryStream();
                                                            workImage[index].Bitmap.Save(mmemoryStream, ImageFormat.Jpeg);
                                                            Byte[] mmemoryStreamBytes = mmemoryStream.ToArray();

                                                            Data_Work = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ref workEthernetData, workImage[index].Width, workImage[index].Height, SourseImageBuffGraphicsInformationPort[index][1 - SourseImageBuffIndexPort[index]][iToolIndex], mmemoryStreamBytes);//生成指令
                                                            ClientControl._Send(Data_Work);//发送数据
                                                        }
                                                        break;
                                                    default:
                                                        MemoryStream memoryStream = new MemoryStream();
                                                        ImageInit.Bitmap.Save(memoryStream, ImageFormat.Jpeg);
                                                        Byte[] memoryStreamBytes = memoryStream.ToArray();

                                                        Data_Work = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ref workEthernetData, ImageInit.Width, ImageInit.Height, new VisionSystemClassLibrary.Struct.ImageInformation(), memoryStreamBytes);//生成指令
                                                        ClientControl._Send(Data_Work);//发送数据
                                                        break;
                                                }
                                            }
                                        }
                                        catch (System.Exception ex)
                                        {
                                            MemoryStream mmemoryStream = new MemoryStream();
                                            ImageInit.Bitmap.Save(mmemoryStream, ImageFormat.Jpeg);
                                            Byte[] mmemoryStreamBytes = mmemoryStream.ToArray();

                                            Data_Work = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ref workEthernetData, ImageInit.Width, ImageInit.Height, new VisionSystemClassLibrary.Struct.ImageInformation(), mmemoryStreamBytes);//生成指令
                                            ClientControl._Send(Data_Work);//发送数据
                                        }
                                    }

                                    break;
                                }
                            }
                        }
                        //                        
                        break;
                    //
                    case CommunicationInstructionType.Live:
                        //WORK，LIVE VIEW页面操作，格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 图像工具数据（1，包含工具；0，不包含工具） + 图像尺寸类型数据
                        
                        VisionSystemCommunicationLibrary.Ethernet.SerializableData liveEthernetData = _EthernetDataDeserialize(ClientData);

                        for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                        {
                            if ((Global.CameraChooseState & (0x01 << index)) != 0)//当前相机开启
                            {
                                if ((VisionSystemClassLibrary.Enum.CameraType)(liveEthernetData.Data_0[0]) == Global.Camera[index].Type)//相机类型相同
                                {
                                    //客户端->服务端：指令类型 + 相机类型数据 + 图像工具数据（1，包含工具；0，不包含工具） + 图像尺寸类型数据 + 图像宽度数据 + 图像高度数据  + 图像信息数据 + 图像数据

                                    Boolean originalView = !Convert.ToBoolean(BitConverter.ToInt32(liveEthernetData.Data_1, 0));
                                    Double imageScale = BitConverter.ToDouble(liveEthernetData.Data_2, 0);//解析图像尺寸

                                    Byte[] Data_Live = null;//待发送的数据

                                    if (VisionSystemClassLibrary.Enum.CameraState.OFF == Global.Camera[index].DeviceInformation.CAM)//相机关闭
                                    {
                                        MemoryStream mmemoryStream = new MemoryStream();
                                        ImageInit.Bitmap.Save(mmemoryStream, ImageFormat.Jpeg);
                                        Byte[] mmemoryStreamBytes = mmemoryStream.ToArray();

                                        Data_Live = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ref liveEthernetData, ImageInit.Width, ImageInit.Height, new VisionSystemClassLibrary.Struct.ImageInformation(), mmemoryStreamBytes);//生成指令
                                        ClientControl._Send(Data_Live);//发送数据
                                    }
                                    else//相机打开，相机未更新
                                    {
                                        try
                                        {
                                            lock (LockSourseImageBuffPort[index])
                                            {
                                                switch (SourseImageBuffIndexPort[index])                              //当前正在执行保存图像缓冲区索引
                                                {
                                                    case 0:
                                                        if (SourseImageBuff0FlagPort[index])
                                                        {
                                                            workImage[index].SetZero();
                                                            workImage[index]._Or(SourseImageBuffPort[index][SourseImageBuffIndexPort[index]]);

                                                            Int32 iToolIndex = 0;
                                                            Boolean bResult = true;

                                                            _GetToolIndex(SourseImageBuffGraphicsInformationPort[index][SourseImageBuffIndexPort[index]], ref bResult, ref iToolIndex);

                                                            if (!originalView)//显示绘制效果图
                                                            {
                                                                if (bResult) //结果完好
                                                                {
                                                                    _DrawImageInformation(workImage[index], SourseImageBuffGraphicsInformationPort[index][SourseImageBuffIndexPort[index]][iToolIndex], SourseImageBuffToolInformationPort[index][SourseImageBuffIndexPort[index]][iToolIndex]);
                                                                }
                                                                else //检测缺陷
                                                                {
                                                                    for (Int32 i = 0; i < SourseImageBuffGraphicsInformationPort[index][SourseImageBuffIndexPort[index]].Length; i++)
                                                                    {
                                                                        if (VisionSystemClassLibrary.Enum.ImageType.Error == SourseImageBuffGraphicsInformationPort[index][SourseImageBuffIndexPort[index]][i].Type)
                                                                        {
                                                                            _DrawImageInformation(workImage[index], SourseImageBuffGraphicsInformationPort[index][SourseImageBuffIndexPort[index]][i], SourseImageBuffToolInformationPort[index][SourseImageBuffIndexPort[index]][i]);
                                                                        }
                                                                    }
                                                                }
                                                            }

                                                            MemoryStream mmemoryStream = new MemoryStream();
                                                            workImage[index].Bitmap.Save(mmemoryStream, ImageFormat.Jpeg);
                                                            Byte[] mmemoryStreamBytes = mmemoryStream.ToArray();

                                                            Data_Live = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ref liveEthernetData, workImage[index].Width, workImage[index].Height, SourseImageBuffGraphicsInformationPort[index][SourseImageBuffIndexPort[index]][iToolIndex], mmemoryStreamBytes);//生成指令
                                                            ClientControl._Send(Data_Live);//发送数据
                                                        }

                                                        if (SourseImageBuff1FlagPort[index])
                                                        {
                                                            workImage[index].SetZero();
                                                            workImage[index]._Or(SourseImageBuffPort[index][1 - SourseImageBuffIndexPort[index]]);

                                                            Int32 iToolIndex = 0;
                                                            Boolean bResult = true;

                                                            _GetToolIndex(SourseImageBuffGraphicsInformationPort[index][1 - SourseImageBuffIndexPort[index]], ref bResult, ref iToolIndex);

                                                            if (!originalView)//显示绘制效果图
                                                            {
                                                                if (bResult) //结果完好
                                                                {
                                                                    _DrawImageInformation(workImage[index], SourseImageBuffGraphicsInformationPort[index][1 - SourseImageBuffIndexPort[index]][iToolIndex], SourseImageBuffToolInformationPort[index][1 - SourseImageBuffIndexPort[index]][iToolIndex]);
                                                                }
                                                                else //检测缺陷
                                                                {
                                                                    for (Int32 i = 0; i < SourseImageBuffGraphicsInformationPort[index][1 - SourseImageBuffIndexPort[index]].Length; i++)
                                                                    {
                                                                        if (VisionSystemClassLibrary.Enum.ImageType.Error == SourseImageBuffGraphicsInformationPort[index][1 - SourseImageBuffIndexPort[index]][i].Type)
                                                                        {
                                                                            _DrawImageInformation(workImage[index], SourseImageBuffGraphicsInformationPort[index][1 - SourseImageBuffIndexPort[index]][i], SourseImageBuffToolInformationPort[index][1 - SourseImageBuffIndexPort[index]][i]);
                                                                        }
                                                                    }
                                                                }
                                                            }

                                                            MemoryStream mmemoryStream = new MemoryStream();
                                                            workImage[index].Bitmap.Save(mmemoryStream, ImageFormat.Jpeg);
                                                            Byte[] mmemoryStreamBytes = mmemoryStream.ToArray();

                                                            Data_Live = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ref liveEthernetData, workImage[index].Width, workImage[index].Height, SourseImageBuffGraphicsInformationPort[index][1 - SourseImageBuffIndexPort[index]][iToolIndex], mmemoryStreamBytes);//生成指令
                                                            ClientControl._Send(Data_Live);//发送数据
                                                        }
                                                        break;
                                                    case 1:
                                                        if (SourseImageBuff1FlagPort[index])
                                                        {
                                                            workImage[index].SetZero();
                                                            workImage[index]._Or(SourseImageBuffPort[index][SourseImageBuffIndexPort[index]]);

                                                            Int32 iToolIndex = 0;
                                                            Boolean bResult = true;

                                                            _GetToolIndex(SourseImageBuffGraphicsInformationPort[index][SourseImageBuffIndexPort[index]], ref bResult, ref iToolIndex);

                                                            if (!originalView)//显示绘制效果图
                                                            {
                                                                if (bResult) //结果完好
                                                                {
                                                                    _DrawImageInformation(workImage[index], SourseImageBuffGraphicsInformationPort[index][SourseImageBuffIndexPort[index]][iToolIndex], SourseImageBuffToolInformationPort[index][SourseImageBuffIndexPort[index]][iToolIndex]);
                                                                }
                                                                else //检测缺陷
                                                                {
                                                                    for (Int32 i = 0; i < SourseImageBuffGraphicsInformationPort[index][SourseImageBuffIndexPort[index]].Length; i++)
                                                                    {
                                                                        if (VisionSystemClassLibrary.Enum.ImageType.Error == SourseImageBuffGraphicsInformationPort[index][SourseImageBuffIndexPort[index]][i].Type)
                                                                        {
                                                                            _DrawImageInformation(workImage[index], SourseImageBuffGraphicsInformationPort[index][SourseImageBuffIndexPort[index]][i], SourseImageBuffToolInformationPort[index][SourseImageBuffIndexPort[index]][i]);
                                                                        }
                                                                    }
                                                                }
                                                            }

                                                            MemoryStream mmemoryStream = new MemoryStream();
                                                            workImage[index].Bitmap.Save(mmemoryStream, ImageFormat.Jpeg);
                                                            Byte[] mmemoryStreamBytes = mmemoryStream.ToArray();

                                                            Data_Live = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ref liveEthernetData, workImage[index].Width, workImage[index].Height, SourseImageBuffGraphicsInformationPort[index][SourseImageBuffIndexPort[index]][iToolIndex], mmemoryStreamBytes);//生成指令
                                                            ClientControl._Send(Data_Live);//发送数据
                                                        }

                                                        if (SourseImageBuff0FlagPort[index])
                                                        {
                                                            workImage[index].SetZero();
                                                            workImage[index]._Or(SourseImageBuffPort[index][1 - SourseImageBuffIndexPort[index]]);

                                                            Int32 iToolIndex = 0;
                                                            Boolean bResult = true;

                                                            _GetToolIndex(SourseImageBuffGraphicsInformationPort[index][1 - SourseImageBuffIndexPort[index]], ref bResult, ref iToolIndex);

                                                            if (!originalView)//显示绘制效果图
                                                            {
                                                                if (bResult) //结果完好
                                                                {
                                                                    _DrawImageInformation(workImage[index], SourseImageBuffGraphicsInformationPort[index][1 - SourseImageBuffIndexPort[index]][iToolIndex], SourseImageBuffToolInformationPort[index][1 - SourseImageBuffIndexPort[index]][iToolIndex]);
                                                                }
                                                                else //检测缺陷
                                                                {
                                                                    for (Int32 i = 0; i < SourseImageBuffGraphicsInformationPort[index][1 - SourseImageBuffIndexPort[index]].Length; i++)
                                                                    {
                                                                        if (VisionSystemClassLibrary.Enum.ImageType.Error == SourseImageBuffGraphicsInformationPort[index][1 - SourseImageBuffIndexPort[index]][i].Type)
                                                                        {
                                                                            _DrawImageInformation(workImage[index], SourseImageBuffGraphicsInformationPort[index][1 - SourseImageBuffIndexPort[index]][i], SourseImageBuffToolInformationPort[index][1 - SourseImageBuffIndexPort[index]][i]);
                                                                        }
                                                                    }
                                                                }
                                                            }

                                                            MemoryStream mmemoryStream = new MemoryStream();
                                                            workImage[index].Bitmap.Save(mmemoryStream, ImageFormat.Jpeg);
                                                            Byte[] mmemoryStreamBytes = mmemoryStream.ToArray();

                                                            Data_Live = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ref liveEthernetData, workImage[index].Width, workImage[index].Height, SourseImageBuffGraphicsInformationPort[index][1 - SourseImageBuffIndexPort[index]][iToolIndex], mmemoryStreamBytes);//生成指令
                                                            ClientControl._Send(Data_Live);//发送数据
                                                        }
                                                        break;
                                                    default:
                                                        MemoryStream memoryStream = new MemoryStream();
                                                        ImageInit.Bitmap.Save(memoryStream, ImageFormat.Jpeg);
                                                        Byte[] memoryStreamBytes = memoryStream.ToArray();

                                                        Data_Live = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ref liveEthernetData, ImageInit.Width, ImageInit.Height, new VisionSystemClassLibrary.Struct.ImageInformation(), memoryStreamBytes);//生成指令
                                                        ClientControl._Send(Data_Live);//发送数据
                                                        break;
                                                }
                                            }
                                        }
                                        catch (System.Exception ex)
                                        {
                                            MemoryStream mmemoryStream = new MemoryStream();
                                            ImageInit.Bitmap.Save(mmemoryStream, ImageFormat.Jpeg);
                                            Byte[] mmemoryStreamBytes = mmemoryStream.ToArray();

                                            Data_Live = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ref liveEthernetData, ImageInit.Width, ImageInit.Height, new VisionSystemClassLibrary.Struct.ImageInformation(),mmemoryStreamBytes);//生成指令
                                            ClientControl._Send(Data_Live);//发送数据
                                        }
                                    }

                                    break;
                                }
                            }
                        }
                        //                        
                        break;
                    //
                    case CommunicationInstructionType.TolerancesSettings_Rejects:

                        //TOLERANCES SETTINGS页面操作（查询REJECTS图像），格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 图像工具数据（1，包含工具；0，不包含工具） + 图像尺寸类型数据
                        
                        for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                        {
                            if ((Global.CameraChooseState & (0x01 << index)) != 0)//当前相机开启
                            {
                                VisionSystemCommunicationLibrary.Ethernet.SerializableData tolerancesSettingsRejectsEthernetData = _EthernetDataDeserialize(ClientData);

                                if ((VisionSystemClassLibrary.Enum.CameraType)(tolerancesSettingsRejectsEthernetData.Data_0[0]) == Global.Camera[index].Type)//相机类型相同
                                {
                                    //客户端->服务端：指令类型 + 相机类型数据 + 图像工具数据（1，包含工具；0，不包含工具） + 图像尺寸类型数据 + 图像宽度数据 + 图像高度数据  + 图像信息数据 + 图像数据

                                    Boolean originalView = !Convert.ToBoolean(BitConverter.ToBoolean(tolerancesSettingsRejectsEthernetData.Data_1, 0));
                                    Double imageScale = BitConverter.ToDouble(tolerancesSettingsRejectsEthernetData.Data_2, 0);//解析图像尺寸

                                    Byte[] Data_TolerancesSettings_Rejects;//待发送的数据

                                    try
                                    {
                                        lock (LockRejectImageBuffPort[index])
                                        {
                                            switch (RejectImageBuffIndexPort[index])                              //当前正在执行保存图像缓冲区索引
                                            {
                                                case 0:
                                                    if (RejectImageBuff0FlagPort[index])
                                                    {
                                                        workImage[index].SetZero();
                                                        workImage[index]._Or(RejectImageBuffPort[index][RejectImageBuffIndexPort[index]]);

                                                        if (!originalView)//显示绘制效果图
                                                        {
                                                            _DrawImageInformation(workImage[index], RejectImageBuffGraphicsInformationPort[index][RejectImageBuffIndexPort[index]][TolerancesSettingsToolIndex[index]], RejectImageBuffToolInformationPort[index][RejectImageBuffIndexPort[index]][TolerancesSettingsToolIndex[index]]);
                                                        }

                                                        MemoryStream mmemoryStream = new MemoryStream();
                                                        workImage[index].Bitmap.Save(mmemoryStream, ImageFormat.Jpeg);
                                                        Byte[] mmemoryStreamBytes = mmemoryStream.ToArray();

                                                        Data_TolerancesSettings_Rejects = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ref tolerancesSettingsRejectsEthernetData, workImage[index].Width, workImage[index].Height, RejectImageBuffGraphicsInformationPort[index][RejectImageBuffIndexPort[index]][TolerancesSettingsToolIndex[index]], mmemoryStreamBytes);//生成指令
                                                        ClientControl._Send(Data_TolerancesSettings_Rejects);//发送数据
                                                    }

                                                    if (RejectImageBuff1FlagPort[index])
                                                    {
                                                        workImage[index].SetZero();
                                                        workImage[index]._Or(RejectImageBuffPort[index][1 - RejectImageBuffIndexPort[index]]);

                                                        if (!originalView)//显示绘制效果图
                                                        {
                                                            _DrawImageInformation(workImage[index], RejectImageBuffGraphicsInformationPort[index][1 - RejectImageBuffIndexPort[index]][TolerancesSettingsToolIndex[index]], RejectImageBuffToolInformationPort[index][1 - RejectImageBuffIndexPort[index]][TolerancesSettingsToolIndex[index]]);
                                                        }

                                                        MemoryStream mmemoryStream = new MemoryStream();
                                                        workImage[index].Bitmap.Save(mmemoryStream, ImageFormat.Jpeg);
                                                        Byte[] mmemoryStreamBytes = mmemoryStream.ToArray();

                                                        Data_TolerancesSettings_Rejects = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ref tolerancesSettingsRejectsEthernetData, workImage[index].Width, workImage[index].Height, RejectImageBuffGraphicsInformationPort[index][1 - RejectImageBuffIndexPort[index]][TolerancesSettingsToolIndex[index]], mmemoryStreamBytes);//生成指令
                                                        ClientControl._Send(Data_TolerancesSettings_Rejects);//发送数据
                                                    }
                                                    break;
                                                case 1:
                                                    if (RejectImageBuff1FlagPort[index])
                                                    {
                                                        workImage[index].SetZero();
                                                        workImage[index]._Or(RejectImageBuffPort[index][RejectImageBuffIndexPort[index]]);

                                                        if (!originalView)//显示绘制效果图
                                                        {
                                                            _DrawImageInformation(workImage[index], RejectImageBuffGraphicsInformationPort[index][RejectImageBuffIndexPort[index]][TolerancesSettingsToolIndex[index]], RejectImageBuffToolInformationPort[index][RejectImageBuffIndexPort[index]][TolerancesSettingsToolIndex[index]]);
                                                        }

                                                        MemoryStream mmemoryStream = new MemoryStream();
                                                        workImage[index].Bitmap.Save(mmemoryStream, ImageFormat.Jpeg);
                                                        Byte[] mmemoryStreamBytes = mmemoryStream.ToArray();

                                                        Data_TolerancesSettings_Rejects = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ref tolerancesSettingsRejectsEthernetData, workImage[index].Width, workImage[index].Height, RejectImageBuffGraphicsInformationPort[index][RejectImageBuffIndexPort[index]][TolerancesSettingsToolIndex[index]], mmemoryStreamBytes);//生成指令
                                                        ClientControl._Send(Data_TolerancesSettings_Rejects);//发送数据
                                                    }

                                                    if (RejectImageBuff0FlagPort[index])
                                                    {
                                                        workImage[index].SetZero();
                                                        workImage[index]._Or(RejectImageBuffPort[index][1 - RejectImageBuffIndexPort[index]]);

                                                        if (!originalView)//显示绘制效果图
                                                        {
                                                            _DrawImageInformation(workImage[index], RejectImageBuffGraphicsInformationPort[index][1 - RejectImageBuffIndexPort[index]][TolerancesSettingsToolIndex[index]], RejectImageBuffToolInformationPort[index][1 - RejectImageBuffIndexPort[index]][TolerancesSettingsToolIndex[index]]);
                                                        }

                                                        MemoryStream mmemoryStream = new MemoryStream();
                                                        workImage[index].Bitmap.Save(mmemoryStream, ImageFormat.Jpeg);
                                                        Byte[] mmemoryStreamBytes = mmemoryStream.ToArray();

                                                        Data_TolerancesSettings_Rejects = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ref tolerancesSettingsRejectsEthernetData, workImage[index].Width, workImage[index].Height, RejectImageBuffGraphicsInformationPort[index][1 - RejectImageBuffIndexPort[index]][TolerancesSettingsToolIndex[index]], mmemoryStreamBytes);//生成指令
                                                        ClientControl._Send(Data_TolerancesSettings_Rejects);//发送数据
                                                    }
                                                    break;
                                                default:
                                                    Data_TolerancesSettings_Rejects = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ref tolerancesSettingsRejectsEthernetData, new VisionSystemClassLibrary.Struct.ImageInformation());//生成指令
                                                    ClientControl._Send(Data_TolerancesSettings_Rejects);//发送数据
                                                    break;
                                            }
                                        }
                                    }
                                    catch (System.Exception ex)
                                    {
                                        Data_TolerancesSettings_Rejects = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ref tolerancesSettingsRejectsEthernetData, new VisionSystemClassLibrary.Struct.ImageInformation());//生成指令
                                        ClientControl._Send(Data_TolerancesSettings_Rejects);//发送数据
                                    }

                                    break;
                                }
                            }
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.TolerancesSettings_Live:

                        //TOLERANCES SETTINGS页面操作（查询LIVE图像），格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 图像工具数据（1，包含工具；0，不包含工具） + 图像尺寸类型数据
                        
                        VisionSystemCommunicationLibrary.Ethernet.SerializableData tolerancesSettingsLiveEthernetData = _EthernetDataDeserialize(ClientData);

                        for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                        {
                            if ((Global.CameraChooseState & (0x01 << index)) != 0)//当前相机开启
                            {
                                if ((VisionSystemClassLibrary.Enum.CameraType)(tolerancesSettingsLiveEthernetData.Data_0[0]) == Global.Camera[index].Type)//相机类型相同
                                {
                                    //客户端->服务端：指令类型 + 相机类型数据 + 图像工具数据（1，包含工具；0，不包含工具） + 图像尺寸类型数据 + 图像宽度数据 + 图像高度数据  + 图像信息数据 + 图像数据

                                    Boolean originalView = !Convert.ToBoolean(BitConverter.ToBoolean(tolerancesSettingsLiveEthernetData.Data_1, 0));
                                    Double imageScale = BitConverter.ToDouble(tolerancesSettingsLiveEthernetData.Data_2, 0);//解析图像尺寸

                                    Byte[] Data_TolerancesSettings_Live;//待发送的数据

                                    if (VisionSystemClassLibrary.Enum.CameraState.OFF == Global.Camera[index].DeviceInformation.CAM)//相机关闭
                                    {
                                        Data_TolerancesSettings_Live = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ref tolerancesSettingsLiveEthernetData, new VisionSystemClassLibrary.Struct.ImageInformation());//生成指令
                                        ClientControl._Send(Data_TolerancesSettings_Live);//发送数据
                                    }
                                    else//相机打开，相机未更新
                                    {
                                        try
                                        {
                                            lock (LockSourseImageBuffPort[index])
                                            {
                                                switch (SourseImageBuffIndexPort[index])                              //当前正在执行保存图像缓冲区索引
                                                {
                                                    case 0:
                                                        if (SourseImageBuff0FlagPort[index])
                                                        {
                                                            workImage[index].SetZero();
                                                            workImage[index]._Or(SourseImageBuffPort[index][SourseImageBuffIndexPort[index]]);

                                                            if (!originalView)//显示绘制效果图
                                                            {
                                                                _DrawImageInformation(workImage[index], SourseImageBuffGraphicsInformationPort[index][SourseImageBuffIndexPort[index]][TolerancesSettingsToolIndex[index]], SourseImageBuffToolInformationPort[index][SourseImageBuffIndexPort[index]][TolerancesSettingsToolIndex[index]]);
                                                            }

                                                            MemoryStream mmemoryStream = new MemoryStream();
                                                            workImage[index].Bitmap.Save(mmemoryStream, ImageFormat.Jpeg);
                                                            Byte[] mmemoryStreamBytes = mmemoryStream.ToArray();

                                                            Data_TolerancesSettings_Live = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ref tolerancesSettingsLiveEthernetData, workImage[index].Width, workImage[index].Height, SourseImageBuffGraphicsInformationPort[index][SourseImageBuffIndexPort[index]][TolerancesSettingsToolIndex[index]], mmemoryStreamBytes);//生成指令
                                                            ClientControl._Send(Data_TolerancesSettings_Live);//发送数据
                                                        }

                                                        if (SourseImageBuff1FlagPort[index])
                                                        {
                                                            workImage[index].SetZero();
                                                            workImage[index]._Or(SourseImageBuffPort[index][1 - SourseImageBuffIndexPort[index]]);

                                                            if (!originalView)//显示绘制效果图
                                                            {
                                                                _DrawImageInformation(workImage[index], SourseImageBuffGraphicsInformationPort[index][1 - SourseImageBuffIndexPort[index]][TolerancesSettingsToolIndex[index]], SourseImageBuffToolInformationPort[index][1 - SourseImageBuffIndexPort[index]][TolerancesSettingsToolIndex[index]]);
                                                            }

                                                            MemoryStream mmemoryStream = new MemoryStream();
                                                            workImage[index].Bitmap.Save(mmemoryStream, ImageFormat.Jpeg);
                                                            Byte[] mmemoryStreamBytes = mmemoryStream.ToArray();

                                                            Data_TolerancesSettings_Live = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ref tolerancesSettingsLiveEthernetData, workImage[index].Width, workImage[index].Height, SourseImageBuffGraphicsInformationPort[index][1 - SourseImageBuffIndexPort[index]][TolerancesSettingsToolIndex[index]], mmemoryStreamBytes);//生成指令
                                                            ClientControl._Send(Data_TolerancesSettings_Live);//发送数据
                                                        }
                                                        break;
                                                    case 1:
                                                        if (SourseImageBuff1FlagPort[index])
                                                        {
                                                            workImage[index].SetZero();
                                                            workImage[index]._Or(SourseImageBuffPort[index][SourseImageBuffIndexPort[index]]);

                                                            if (!originalView)//显示绘制效果图
                                                            {
                                                                _DrawImageInformation(workImage[index], SourseImageBuffGraphicsInformationPort[index][SourseImageBuffIndexPort[index]][TolerancesSettingsToolIndex[index]], SourseImageBuffToolInformationPort[index][SourseImageBuffIndexPort[index]][TolerancesSettingsToolIndex[index]]);
                                                            }

                                                            MemoryStream mmemoryStream = new MemoryStream();
                                                            workImage[index].Bitmap.Save(mmemoryStream, ImageFormat.Jpeg);
                                                            Byte[] mmemoryStreamBytes = mmemoryStream.ToArray();

                                                            Data_TolerancesSettings_Live = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ref tolerancesSettingsLiveEthernetData, workImage[index].Width, workImage[index].Height, SourseImageBuffGraphicsInformationPort[index][SourseImageBuffIndexPort[index]][TolerancesSettingsToolIndex[index]], mmemoryStreamBytes);//生成指令
                                                            ClientControl._Send(Data_TolerancesSettings_Live);//发送数据
                                                        }

                                                        if (SourseImageBuff0FlagPort[index])
                                                        {
                                                            workImage[index].SetZero();
                                                            workImage[index]._Or(SourseImageBuffPort[index][1 - SourseImageBuffIndexPort[index]]);

                                                            if (!originalView)//显示绘制效果图
                                                            {
                                                                _DrawImageInformation(workImage[index], SourseImageBuffGraphicsInformationPort[index][1 - SourseImageBuffIndexPort[index]][TolerancesSettingsToolIndex[index]], SourseImageBuffToolInformationPort[index][1 - SourseImageBuffIndexPort[index]][TolerancesSettingsToolIndex[index]]);
                                                            }

                                                            MemoryStream mmemoryStream = new MemoryStream();
                                                            workImage[index].Bitmap.Save(mmemoryStream, ImageFormat.Jpeg);
                                                            Byte[] mmemoryStreamBytes = mmemoryStream.ToArray();

                                                            Data_TolerancesSettings_Live = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ref tolerancesSettingsLiveEthernetData, workImage[index].Width, workImage[index].Height, SourseImageBuffGraphicsInformationPort[index][1 - SourseImageBuffIndexPort[index]][TolerancesSettingsToolIndex[index]], mmemoryStreamBytes);//生成指令
                                                            ClientControl._Send(Data_TolerancesSettings_Live);//发送数据
                                                        }
                                                        break;
                                                    default:
                                                        Data_TolerancesSettings_Live = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ref tolerancesSettingsLiveEthernetData, new VisionSystemClassLibrary.Struct.ImageInformation());//生成指令
                                                        ClientControl._Send(Data_TolerancesSettings_Live);//发送数据
                                                        break;
                                                }
                                            }
                                        }
                                        catch (System.Exception ex)
                                        {
                                            Data_TolerancesSettings_Live = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ref tolerancesSettingsLiveEthernetData, new VisionSystemClassLibrary.Struct.ImageInformation());//生成指令
                                            ClientControl._Send(Data_TolerancesSettings_Live);//发送数据
                                        }
                                    }

                                    break;
                                }
                            }
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.TolerancesSettings_Graphs:

                        //TOLERANCES SETTINGS页面操作（查询曲线图数据），格式：
                        //查询曲线图数据：服务器->客户端：指令类型 + 相机类型数据

                        for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                        {
                            if ((Global.CameraChooseState & (0x01 << index)) != 0)//当前相机开启
                            {
                                if ((VisionSystemClassLibrary.Enum.CameraType)(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1]) == Global.Camera[index].Type)//相机类型相同
                                {
                                    VisionSystemClassLibrary.Struct.TolerancesGraphData_Value[] tolerancesGraphDataValue = new VisionSystemClassLibrary.Struct.TolerancesGraphData_Value[Global.Camera[index].Tolerances.GraphData.Count];
                                    for (Byte i = 0; i < Global.Camera[index].Tolerances.GraphData.Count; i++)//循环存储公差数据
                                    {
                                        tolerancesGraphDataValue[i] = Global.Camera[index].Tolerances.GraphData[i].TolerancesGraphDataValue;
                                    }

                                    MemoryStream tolerancesSettingsGraphsMemoryStream = new MemoryStream();//流对象
                                    IFormatter tolerancesSettingsGraphsFormatter = new SoapFormatter();//格式化对象

                                    tolerancesSettingsGraphsMemoryStream.Position = 0;
                                    tolerancesSettingsGraphsFormatter.Serialize(tolerancesSettingsGraphsMemoryStream, tolerancesGraphDataValue);//序列化客户端返回数据

                                    Byte[] tolerancesSettingsGraphsMemoryStreamBytes = tolerancesSettingsGraphsMemoryStream.ToArray();//公差数据序列化长度
                                    Int32 tolerancesSettingsGraphsMemoryStreamLength = tolerancesSettingsGraphsMemoryStreamBytes.Length;

                                    //客户端->服务端：指令类型 + 相机类型数据 + 公差类数据

                                    Byte[] Data_TolerancesSettings_Graphs = new byte[2 + BitConverter.GetBytes(tolerancesSettingsGraphsMemoryStreamLength).Length + tolerancesSettingsGraphsMemoryStreamLength];//待发送的指令数据

                                    Data_TolerancesSettings_Graphs[0] = ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex];//命令类型
                                    Data_TolerancesSettings_Graphs[1] = ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1];//命令类型
                                    BitConverter.GetBytes(tolerancesSettingsGraphsMemoryStreamLength).CopyTo(Data_TolerancesSettings_Graphs, 2);//公差数据序列化长度内容
                                    tolerancesSettingsGraphsMemoryStreamBytes.CopyTo(Data_TolerancesSettings_Graphs, 2 + BitConverter.GetBytes(tolerancesSettingsGraphsMemoryStreamLength).Length);//公差数据

                                    Thread.Sleep(Global.ImageDataTime);//循环数据传输休眠10ms

                                    ClientControl._Send(Data_TolerancesSettings_Graphs);//发送数据

                                    break;
                                }
                            }
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.TolerancesSettings_Tool:

                        //TOLERANCES SETTINGS页面操作（工具开关），格式：
                        //查询曲线图数据：服务器->客户端：指令类型 + 相机类型数据 + 工具索引数值 + 工具开关数值

                        for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                        {
                            if ((Global.CameraChooseState & (0x01 << index)) != 0)//当前相机开启
                            {
                                if ((VisionSystemClassLibrary.Enum.CameraType)(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1]) == Global.Camera[index].Type)//相机类型相同
                                {
                                    //客户端->服务端：指令类型 + 相机类型数据 + 工具开关设置结果（1，成功；0，失败）

                                    Int32 iToolIndex = BitConverter.ToInt32(ClientData.ReceivedData, ClientData.DataInfo.InstructionIndex + 2);
                                    Global.Camera[index].Tools[iToolIndex].ToolState = Convert.ToBoolean(BitConverter.ToInt32(ClientData.ReceivedData, ClientData.DataInfo.InstructionIndex + 6));

                                    if ((false == Global.Camera[index].Tools[iToolIndex].ToolState) && (iToolIndex == TolerancesSettingsToolIndex[index])) //当前工具关闭，且被选中工具索引发生变化，更新索引值
                                    {
                                        for (Int32 i = 0; i < Global.Camera[index].Tools.Count; i++) //循环所有工具
                                        {
                                            if (Global.Camera[index].Tools[i].ToolState && Global.Camera[index].Tools[i].ExistTolerance) //工具已开启，且有公差
                                            {
                                                TolerancesSettingsToolIndex[index] = i;
                                                break;
                                            }
                                        }
                                    }

                                    Byte[] Data_TolerancesSettings_Tool = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1], BitConverter.GetBytes(1));//生成客户端数据
                                    ClientControl._Send(Data_TolerancesSettings_Tool);//发送数据

                                    break;
                                }
                            }
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.TolerancesSettings_Learn:

                        //TOLERANCES SETTINGS页面操作（学习），格式：
                        //查询曲线图数据：服务器->客户端：指令类型 + 相机类型数据 + 工具索引数值

                        for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                        {
                            if ((Global.CameraChooseState & (0x01 << index)) != 0)//当前相机开启
                            {
                                if ((VisionSystemClassLibrary.Enum.CameraType)(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1]) == Global.Camera[index].Type)//相机类型相同
                                {
                                    //客户端->服务端：指令类型 + 相机类型数据 + 工具索引数值 + 学习数值 + 学习中的有效数值数量 + 学习中的无效数值数量

                                    Int32 iToolIndex = BitConverter.ToInt32(ClientData.ReceivedData, ClientData.DataInfo.InstructionIndex + 2);

                                    Int32 LearnedValue = 0, ValidValue = 0, NonvalidValue = 0;
                                    VisionSystemClassLibrary.Class.TolerancesData._Learn(Global.Camera[index].Tolerances.GraphData[Global.Camera[index].Tools[iToolIndex].TolerancesIndex].TolerancesGraphDataValue.CurrentValueIndex, Global.Camera[index].Tolerances.GraphData[Global.Camera[index].Tools[iToolIndex].TolerancesIndex].TolerancesGraphDataValue.Value, Global.Camera[index].Tools[iToolIndex].Min, Global.Camera[index].Tools[iToolIndex].Max, ref LearnedValue, ref ValidValue, ref NonvalidValue);

                                    Global.Camera[index].Tools[iToolIndex].LearnedValue = LearnedValue;
                                    Global.Camera[index].Tools[iToolIndex].ValidValue = ValidValue;
                                    Global.Camera[index].Tools[iToolIndex].NonvalidValue = NonvalidValue;

                                    Byte[] Data_TolerancesSettings_Learn = _GenerateInstruction(ClientData, BitConverter.GetBytes(Convert.ToInt32(Global.Camera[index].Tools[iToolIndex].LearnedValue)), BitConverter.GetBytes(Convert.ToInt32(Global.Camera[index].Tools[iToolIndex].ValidValue)), BitConverter.GetBytes(Convert.ToInt32(Global.Camera[index].Tools[iToolIndex].NonvalidValue)));//生成客户端数据
                                    ClientControl._Send(Data_TolerancesSettings_Learn);//发送数据

                                    break;
                                }
                            }
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.TolerancesSettings_ToolIndex:

                        //TOLERANCES SETTINGS页面操作（双击选中工具），格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 工具索引数值

                        for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                        {
                            if ((Global.CameraChooseState & (0x01 << index)) != 0)//当前相机开启
                            {
                                if ((VisionSystemClassLibrary.Enum.CameraType)(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1]) == Global.Camera[index].Type)//相机类型相同
                                {
                                    //客户端->服务端：指令类型 + 相机类型数据 + 工具索引数值 + 设置结果（1，成功；0，失败）

                                    TolerancesSettingsToolIndex[index] = BitConverter.ToInt32(ClientData.ReceivedData, ClientData.DataInfo.InstructionIndex + 2);

                                    Byte[] Data_TolerancesSettings_ToolIndex = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1], BitConverter.GetBytes(TolerancesSettingsToolIndex[index]), BitConverter.GetBytes(1));//生成客户端数据
                                    ClientControl._Send(Data_TolerancesSettings_ToolIndex);//发送数据

                                    break;
                                }
                            }
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.TolerancesSettings_MinMax:

                        //TOLERANCES SETTINGS页面操作（曲线图范围数值），格式：
                        //查询曲线图数据：服务端->客户端：指令类型 + 相机类型数据 + 工具索引数值 + 最小值数值 + 最大值数值 + 坐标轴最小值数值 + 坐标轴最大值数值

                        for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                        {
                            if ((Global.CameraChooseState & (0x01 << index)) != 0)//当前相机开启
                            {
                                if ((VisionSystemClassLibrary.Enum.CameraType)(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1]) == Global.Camera[index].Type)//相机类型相同
                                {
                                    //客户端->服务端：指令类型 + 相机类型数据 + 工具索引数值 + 最小值最大值设置结果（1，成功；0，失败）

                                    Int32 iToolIndex = BitConverter.ToInt32(ClientData.ReceivedData, ClientData.DataInfo.InstructionIndex + 2);
                                    Int32 effectiveMinValue = BitConverter.ToInt32(ClientData.ReceivedData, ClientData.DataInfo.InstructionIndex + 6);
                                    Int32 effectiveMaxValue = BitConverter.ToInt32(ClientData.ReceivedData, ClientData.DataInfo.InstructionIndex + 10);
                                    Int32 minValue = BitConverter.ToInt32(ClientData.ReceivedData, ClientData.DataInfo.InstructionIndex + 14);

                                    if (Global.Camera[index].Tolerances.GraphData[Global.Camera[index].Tools[iToolIndex].TolerancesIndex].EffectiveMin_Value != effectiveMinValue)//最小值更新
                                    {
                                        Global.Camera[index].Tolerances.GraphData[Global.Camera[index].Tools[iToolIndex].TolerancesIndex].EffectiveMin_Value = effectiveMinValue;
                                        Global.Camera[index].Tools[iToolIndex].Min = Global.Camera[index].Tolerances.GraphData[Global.Camera[index].Tools[iToolIndex].TolerancesIndex].EffectiveMin_Value;

                                        if (VisionSystemClassLibrary.Enum.ArithmeticType.Tobacco == Global.Camera[index].Tools[iToolIndex].Type)//当前执行烟支检测
                                        {
                                            float fPrecision=Global.Camera[index].Tools[iToolIndex].Precision;
                                            Global.Camera[index].Tolerances._GetPrecision(Global.Camera[index].Tolerances.EjectLevel, Global.Camera[index].Tools[iToolIndex].Min, Global.Camera[index].Tools[iToolIndex].EjectPixelMin, ref fPrecision);
                                            Global.Camera[index].Tools[iToolIndex].Precision = fPrecision;
                                        }
                                    }

                                    if (Global.Camera[index].Tolerances.GraphData[Global.Camera[index].Tools[iToolIndex].TolerancesIndex].EffectiveMax_Value != effectiveMaxValue)//最大值更新
                                    {
                                        Global.Camera[index].Tolerances.GraphData[Global.Camera[index].Tools[iToolIndex].TolerancesIndex].EffectiveMax_Value = effectiveMaxValue;
                                        Global.Camera[index].Tools[iToolIndex].Max = Global.Camera[index].Tolerances.GraphData[Global.Camera[index].Tools[iToolIndex].TolerancesIndex].EffectiveMax_Value;

                                        if (VisionSystemClassLibrary.Enum.ArithmeticType.Tobacco_D == Global.Camera[index].Tools[iToolIndex].Type)//当前执行烟支检测
                                        {
                                            float fPrecision = Global.Camera[index].Tools[iToolIndex].Precision;
                                            Global.Camera[index].Tolerances._GetPrecision_Tobacco_D(Global.Camera[index].Tolerances.EjectLevel, Global.Camera[index].Tools[iToolIndex].Max, Global.Camera[index].Tools[iToolIndex].EjectPixelMin, ref fPrecision);
                                            Global.Camera[index].Tools[iToolIndex].Precision = fPrecision;
                                        }

                                        if ((Global.Camera[index].Tools[iToolIndex].Detect_89713FA) && (VisionSystemClassLibrary.Enum.ArithmeticType.BaleLoosing == Global.Camera[index].Tools[iToolIndex].Type))//当前执行烟支检测
                                        {
                                            float fPrecision = Global.Camera[index].Tools[iToolIndex].Precision;
                                            Global.Camera[index].Tolerances._GetPrecision_Tobacco_D(Global.Camera[index].Tolerances.EjectLevel, Global.Camera[index].Tools[iToolIndex].Max, Global.Camera[index].Tools[iToolIndex].EjectPixelMin, ref fPrecision);
                                            Global.Camera[index].Tools[iToolIndex].Precision = fPrecision;
                                        }
                                    }

                                    Byte[] Data_TolerancesSettings_MinMax = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1], BitConverter.GetBytes(iToolIndex), BitConverter.GetBytes(1));//生成客户端数据
                                    ClientControl._Send(Data_TolerancesSettings_MinMax);//发送数据

                                    break;
                                }
                            }
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.TolerancesSettings_EjectLevel:

                        //TOLERANCES SETTINGS页面操作（曲线图范围数值），格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 灵敏度 + 调节灵敏度标记（0,：true;1：调节光电空头校准值）

                        for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                        {
                            if ((Global.CameraChooseState & (0x01 << index)) != 0)//当前相机开启
                            {
                                if ((VisionSystemClassLibrary.Enum.CameraType)(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1]) == Global.Camera[index].Type)//相机类型相同
                                {
                                    //客户端->服务端：指令类型 + 相机类型数据 + 灵敏度 + 调节灵敏度标记（0,：true;1：调节光电空头校准值） + 公差个数 + （每个）公差下限、上限

                                    Global.Camera[index].Tolerances.EjectLevel = Convert.ToInt16(BitConverter.ToInt32(ClientData.ReceivedData, ClientData.DataInfo.InstructionIndex + 2));//灵敏度
                                    Int32 iUpdateTolerances = BitConverter.ToInt32(ClientData.ReceivedData, ClientData.DataInfo.InstructionIndex + 6);//调节灵敏度标记（0,：true;1：调节光电空头校准值）

                                    Int32 iIndex = 0;
                                    Byte[] tolerancesDataMinAndMax = new Byte[2 * 4 * Global.Camera[index].Tolerances.GraphData.Count];

                                    if (1 == iUpdateTolerances) //更新公差
                                    {
                                        for (Int32 i = 0; i < Global.Camera[index].Tolerances.GraphData.Count; i++) //遍历当前所有工具
                                        {
                                            if (VisionSystemClassLibrary.Enum.ArithmeticType.Tobacco_D == Global.Camera[index].Tools[i].Type)//当前执行光电烟支检测，基准值上限+40
                                            {
                                                Int32 iCurrentValue = 40;

                                                lock (LockSourseImageBuffPort[index])
                                                {
                                                    switch (SourseImageBuffIndexPort[index])                              //当前正在执行保存图像缓冲区索引
                                                    {
                                                        case 0:
                                                            if (SourseImageBuff0FlagPort[index])
                                                            {
                                                                iCurrentValue += SourseImageBuffGraphicsInformationPort[index][SourseImageBuffIndexPort[index]][Global.Camera[index].Tolerances.GraphData[i].ToolsIndex].CurrentValue;
                                                            }

                                                            if (SourseImageBuff1FlagPort[index])
                                                            {
                                                                iCurrentValue += SourseImageBuffGraphicsInformationPort[index][1 - SourseImageBuffIndexPort[index]][Global.Camera[index].Tolerances.GraphData[i].ToolsIndex].CurrentValue;
                                                            }
                                                            break;
                                                        case 1:
                                                            if (SourseImageBuff1FlagPort[index])
                                                            {

                                                                iCurrentValue += SourseImageBuffGraphicsInformationPort[index][SourseImageBuffIndexPort[index]][Global.Camera[index].Tolerances.GraphData[i].ToolsIndex].CurrentValue;
                                                            }

                                                            if (SourseImageBuff0FlagPort[index])
                                                            {
                                                                iCurrentValue += SourseImageBuffGraphicsInformationPort[index][1 - SourseImageBuffIndexPort[index]][Global.Camera[index].Tolerances.GraphData[i].ToolsIndex].CurrentValue;
                                                            }
                                                            break;
                                                        default:
                                                            break;
                                                    }
                                                }
                                                Global.Camera[index].Tolerances.GraphData[i].EffectiveMax_Value = iCurrentValue;
                                                Global.Camera[index].Tools[Global.Camera[index].Tolerances.GraphData[i].ToolsIndex].Max = Global.Camera[index].Tolerances.GraphData[i].EffectiveMax_Value;

                                                float fPrecision = Global.Camera[index].Tools[Global.Camera[index].Tolerances.GraphData[i].ToolsIndex].Precision;
                                                Global.Camera[index].Tolerances._GetPrecision_Tobacco_D(Global.Camera[index].Tolerances.EjectLevel, Global.Camera[index].Tools[Global.Camera[index].Tolerances.GraphData[i].ToolsIndex].Max, Global.Camera[index].Tools[Global.Camera[index].Tolerances.GraphData[i].ToolsIndex].EjectPixelMin, ref fPrecision);
                                                Global.Camera[index].Tools[Global.Camera[index].Tolerances.GraphData[i].ToolsIndex].Precision = fPrecision;
                                            }

                                            BitConverter.GetBytes((Int32)Global.Camera[index].Tools[Global.Camera[index].Tolerances.GraphData[i].ToolsIndex].Min).CopyTo(tolerancesDataMinAndMax, iIndex);
                                            iIndex += 4;

                                            BitConverter.GetBytes((Int32)Global.Camera[index].Tools[Global.Camera[index].Tolerances.GraphData[i].ToolsIndex].Max).CopyTo(tolerancesDataMinAndMax, iIndex);
                                            iIndex += 4;
                                        }
                                    }
                                    else //更改灵敏度
                                    {
                                        for (Int32 i = 0; i < Global.Camera[index].Tolerances.GraphData.Count; i++) //遍历当前所有工具
                                        {
                                            if (VisionSystemClassLibrary.Enum.ArithmeticType.Tobacco == Global.Camera[index].Tools[i].Type)//当前执行烟支检测
                                            {
                                                Global.Camera[index].Tools[Global.Camera[index].Tolerances.GraphData[i].ToolsIndex].EjectLevel = Global.Camera[index].Tolerances.EjectLevel;

                                                Global.Camera[index].Tolerances.GraphData[i].EffectiveMin_Value = Global.Camera[index].Tolerances._GetEjectPixel(Global.Camera[index].Tolerances.EjectLevel, Global.Camera[index].Tools[Global.Camera[index].Tolerances.GraphData[i].ToolsIndex].EjectPixelMin, Global.Camera[index].Tools[Global.Camera[index].Tolerances.GraphData[i].ToolsIndex].Precision);
                                                Global.Camera[index].Tools[Global.Camera[index].Tolerances.GraphData[i].ToolsIndex].Min = Global.Camera[index].Tolerances.GraphData[i].EffectiveMin_Value;
                                            }
                                            else if (VisionSystemClassLibrary.Enum.ArithmeticType.Tobacco_D == Global.Camera[index].Tools[i].Type)//当前执行光电烟支检测
                                            {
                                                Global.Camera[index].Tools[Global.Camera[index].Tolerances.GraphData[i].ToolsIndex].EjectLevel = Global.Camera[index].Tolerances.EjectLevel;

                                                Global.Camera[index].Tolerances.GraphData[i].EffectiveMax_Value = Global.Camera[index].Tolerances._GetEjectPixel_Tobacco_D(Global.Camera[index].Tolerances.EjectLevel, Global.Camera[index].Tools[Global.Camera[index].Tolerances.GraphData[i].ToolsIndex].EjectPixelMin, Global.Camera[index].Tools[Global.Camera[index].Tolerances.GraphData[i].ToolsIndex].Precision);
                                                Global.Camera[index].Tools[Global.Camera[index].Tolerances.GraphData[i].ToolsIndex].Max = Global.Camera[index].Tolerances.GraphData[i].EffectiveMax_Value;
                                            }
                                            else if ((Global.Camera[index].Tools[i].Detect_89713FA) && (VisionSystemClassLibrary.Enum.ArithmeticType.BaleLoosing == Global.Camera[index].Tools[i].Type))//当前执行光电烟支检测
                                            {
                                                Global.Camera[index].Tools[Global.Camera[index].Tolerances.GraphData[i].ToolsIndex].EjectLevel = Global.Camera[index].Tolerances.EjectLevel;

                                                Global.Camera[index].Tolerances.GraphData[i].EffectiveMax_Value = Global.Camera[index].Tolerances._GetEjectPixel_Tobacco_D(Global.Camera[index].Tolerances.EjectLevel, Global.Camera[index].Tools[Global.Camera[index].Tolerances.GraphData[i].ToolsIndex].EjectPixelMin, Global.Camera[index].Tools[Global.Camera[index].Tolerances.GraphData[i].ToolsIndex].Precision);
                                                Global.Camera[index].Tools[Global.Camera[index].Tolerances.GraphData[i].ToolsIndex].Max = Global.Camera[index].Tolerances.GraphData[i].EffectiveMax_Value;
                                            }

                                            BitConverter.GetBytes((Int32)Global.Camera[index].Tools[Global.Camera[index].Tolerances.GraphData[i].ToolsIndex].Min).CopyTo(tolerancesDataMinAndMax, iIndex);
                                            iIndex += 4;

                                            BitConverter.GetBytes((Int32)Global.Camera[index].Tools[Global.Camera[index].Tolerances.GraphData[i].ToolsIndex].Max).CopyTo(tolerancesDataMinAndMax, iIndex);
                                            iIndex += 4;
                                        }
                                    }

                                    Byte[] Data_TolerancesSettings_EjectLevel = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1], BitConverter.GetBytes((Int32)Global.Camera[index].Tolerances.EjectLevel), BitConverter.GetBytes(iUpdateTolerances), BitConverter.GetBytes((Int32)Global.Camera[index].Tolerances.GraphData.Count), tolerancesDataMinAndMax);//生成客户端数据
                                    ClientControl._Send(Data_TolerancesSettings_EjectLevel);//发送数据

                                    break;
                                }
                            }
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.TolerancesSettings_ResetGraphs:

                        //TOLERANCES SETTINGS页面操作（复位曲线图），格式：
                        //查询曲线图数据：服务器->客户端：指令类型 + 相机类型数据

                        for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                        {
                            if ((Global.CameraChooseState & (0x01 << index)) != 0)//当前相机开启
                            {
                                if ((VisionSystemClassLibrary.Enum.CameraType)(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1]) == Global.Camera[index].Type)//相机类型相同
                                {
                                    //客户端->服务端：指令类型 + 相机类型数据 + 复位结果（1，成功；0，失败）

                                    Byte[] Data_TolerancesSettings_ResetGraphs = _GenerateInstruction(ClientData, BitConverter.GetBytes(_GraphsReset(index)));//生成客户端数据
                                    ClientControl._Send(Data_TolerancesSettings_ResetGraphs);//发送数据

                                    break;
                                }
                            }
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.TolerancesSettings_Enter:

                        //TOLERANCES SETTINGS页面操作（进入页面），格式：
                        //查询曲线图数据：服务器->客户端：指令类型 + 相机类型数据

                        for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                        {
                            if ((Global.CameraChooseState & (0x01 << index)) != 0)//当前相机开启
                            {
                                if ((VisionSystemClassLibrary.Enum.CameraType)(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1]) == Global.Camera[index].Type)//相机类型相同
                                {
                                    //客户端->服务端：指令类型 + 相机类型数据 + 操作结果（1，成功；0，失败）
                                    lock (LockSourseImageBuffPort[index])
                                    {
                                        lock (LockSourseImageBuffPort[index])
                                        {
                                            switch (SourseImageBuffIndexPort[index])                              //当前正在执行保存图像缓冲区索引
                                            {
                                                case 0:
                                                    if (SourseImageBuff0FlagPort[index])
                                                    {
                                                        TolerancesSettingsToolIndex[index] = _GetToolIndex(index, SourseImageBuffGraphicsInformationPort[index][SourseImageBuffIndexPort[index]]);
                                                    }

                                                    if (SourseImageBuff1FlagPort[index])
                                                    {
                                                        TolerancesSettingsToolIndex[index] = _GetToolIndex(index, SourseImageBuffGraphicsInformationPort[index][1 - SourseImageBuffIndexPort[index]]);
                                                    }
                                                    break;
                                                case 1:
                                                    if (SourseImageBuff1FlagPort[index])
                                                    {
                                                        TolerancesSettingsToolIndex[index] = _GetToolIndex(index, SourseImageBuffGraphicsInformationPort[index][SourseImageBuffIndexPort[index]]);
                                                    }

                                                    if (SourseImageBuff0FlagPort[index])
                                                    {
                                                        TolerancesSettingsToolIndex[index] = _GetToolIndex(index, SourseImageBuffGraphicsInformationPort[index][1 - SourseImageBuffIndexPort[index]]);
                                                    }
                                                    break;
                                                default:
                                                    for (Int32 i = 0; i < Global.Camera[index].Tools.Count; i++) //循环所有工具
                                                    {
                                                        if (Global.Camera[index].Tools[i].ToolState && Global.Camera[index].Tools[i].ExistTolerance) //工具已开启，且有公差
                                                        {
                                                            TolerancesSettingsToolIndex[index] = i;
                                                            break;
                                                        }
                                                    }
                                                    break;
                                            }
                                        }
                                    }

                                    Byte[] Data_TolerancesSettings_Enter = _GenerateInstruction(ClientData, BitConverter.GetBytes(_ToleranceOperate(index, 1)));//生成客户端数据
                                    ClientControl._Send(Data_TolerancesSettings_Enter);//发送数据

                                    break;
                                }
                            }
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.TolerancesSettings_SaveProduct:

                        //TOLERANCES SETTINGS页面操作（保存数据），格式：
                        //查询曲线图数据：服务器->客户端：指令类型 + 相机类型数据 + 是否保存数据（1，是；0，否）

                        for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                        {
                            if ((Global.CameraChooseState & (0x01 << index)) != 0)//当前相机开启
                            {
                                if ((VisionSystemClassLibrary.Enum.CameraType)(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1]) == Global.Camera[index].Type)//相机类型相同
                                {
                                    //客户端->服务端：指令类型 + 相机类型数据 + 是否保存数据（1，是；0，否） + 保存数据结果（1，成功；0，失败）

                                    Boolean bToleranceSave = BitConverter.ToBoolean(ClientData.ReceivedData, ClientData.DataInfo.InstructionIndex + 2);

                                    Byte[] Data_TolerancesSettings_SaveProduct = _GenerateInstruction(ClientData, BitConverter.GetBytes(_ToleranceOperate(index, 2, bToleranceSave)));//生成客户端数据
                                    ClientControl._Send(Data_TolerancesSettings_SaveProduct);//发送数据

                                    break;
                                }
                            }
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.BrandManagement_LoadBrand:

                        //完成文件发送
                        //服务端->客户端：指令类型 + 相机类型数据 + 设置相机模式 + 品牌名称长度 + 品牌名称

                        Int32 iCameraChooseState_BrandManagement_LoadBrand = 0;

                        for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                        {
                            if ((VisionSystemClassLibrary.Enum.CameraType)(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1]) == Global.Camera[index].Type)//相机类型相同
                            {
                                Int32 iIndex = ClientData.DataInfo.InstructionIndex + 2;
                                iCameraChooseState_BrandManagement_LoadBrand = BitConverter.ToInt32(ClientData.ReceivedData, iIndex);//相机模式数据

                                iIndex += 4;
                                Int32 iBrandNameLength = BitConverter.ToInt32(ClientData.ReceivedData, iIndex);//品牌名称长度数据

                                if (iBrandNameLength > 0)//文件名长度有效
                                {
                                    iIndex += 4;
                                    Global.BrandName = VisionSystemCommunicationLibrary.Ethernet.TestControlFunctions._ByteToString(ClientData.ReceivedData, iIndex, iBrandNameLength);//品牌名称数据
                                }

                                //完成文件发送
                                //客户端->服务端：指令类型 + 相机类型数据 + 设置相机模式 + 品牌名称长度 + 品牌名称 + 文件索引值（从0开始） + 文件传输状态（1，文件发送中；2，文件发送完成） + 配置结果（1，成功；0，失败）

                                Byte[] byteValue_1 = new Byte[ClientData.DataInfo.InstructionLength];//指令类型 + 相机类型数据 + 品牌名称长度 + 品牌名称
                                System.Array.Copy(ClientData.ReceivedData, ClientData.DataInfo.InstructionIndex, byteValue_1, 0, ClientData.DataInfo.InstructionLength);

                                //文件索引值（从0开始） + 文件传输状态（1，文件发送中；2，文件发送完成） + 配置结果（1，成功；0，失败）

                                Byte[] byteValue_2 = BitConverter.GetBytes((Int32)1);//文件索引值（从0开始）
                                Byte[] byteValue_3 = BitConverter.GetBytes((Int32)2);//文件传输状态（1，文件发送中；2，文件发送完成）
                                Byte[] byteValue_4 = BitConverter.GetBytes((Int32)1);//文件接收结果（1，成功；0，失败）

                                Byte[] Data_BrandManagement_LoadBrand = _GenerateInstruction(byteValue_1, byteValue_2, byteValue_3, byteValue_4);//生成客户端数据
                                ClientControl._Send(Data_BrandManagement_LoadBrand);//发送数据

                                CommunicationCount_BrandManagement_LoadBrand |= (Byte)(0x01 << index);

                                break;
                            }
                        }

                        if (iCameraChooseState_BrandManagement_LoadBrand == CommunicationCount_BrandManagement_LoadBrand) //相机参数更新完毕
                        {
                            for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                            {
                                if ((CommunicationCount_BrandManagement_LoadBrand & (0x01 << index)) != 0)//当前相机开启
                                {
                                    _CameraFileCopy(index);//更新相机参数
                                }
                            }
                            CommunicationCount_BrandManagement_LoadBrand = 0;
                            iCameraChooseState_BrandManagement_LoadBrand = 0;
                            
                            CloseSerialPortFlag = true;
                            CloseSerialPort_ComType = CommunicationInstructionType.BrandManagement_LoadBrand;

                            if (false == timer3.Enabled) //开启定时器3，执行重启
                            {
                                this.Invoke(new MethodInvoker(delegate { timer3.Enabled = true; }));
                            }
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.Live_SelfTrigger:

                        //LIVE VIEW页面操作，格式：
                        //服务端->客户端：指令类型 + 相机类型 + 操作数据（1，打开；0，关闭）

                        for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                        {
                            if ((Global.CameraChooseState & (0x01 << index)) != 0)//当前相机开启
                            {
                                if ((VisionSystemClassLibrary.Enum.CameraType)(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1]) == Global.Camera[index].Type)//相机类型相同
                                {
                                    //客户端->服务端：指令类型 + 相机类型 + 操作结果（1，成功；0，失败）

                                    Boolean deviceTrigger = BitConverter.ToBoolean(ClientData.ReceivedData, ClientData.DataInfo.InstructionIndex + 2);
                                    LabModel = deviceTrigger;

                                    if (Global.Camera[index].IsSerialPort) //当前为串口
                                    {
                                        if (false == LabModel) //当前为非实验室模式
                                        {
                                            bSerialPortLabState[index] = false;

                                            lock (LockSerialPort[index])
                                            {
                                                _SendCommand_SerialPortCommucation(1, index);//发送实验室状态
                                            }

                                            this.Invoke(new MethodInvoker(delegate { timer4.Enabled = false; }));
                                            this.Invoke(new MethodInvoker(delegate { timer_89713FA.Enabled = false; }));
                                        }
                                        else //当前为实验室状态
                                        {
                                            lock (LockSerialPort[index])
                                            {
                                                _SendCommand_SerialPortCommucation(1, index);//发送实验室状态
                                            }

                                            if (false == timer4.Enabled) //定时器4未打开，开启ADC查询
                                            {
                                                this.Invoke(new MethodInvoker(delegate { timer4.Enabled = true; }));
                                            }

                                            bSerialPortLabState[index] = true;
                                        }
                                    }

                                    Byte[] Data_Live_SelfTrigger = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1], BitConverter.GetBytes(1));//生成客户端数据
                                    ClientControl._Send(Data_Live_SelfTrigger);//发送数据

                                    break;
                                }
                            }
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.QualityCheck_SaveProduct:

                        //QUALITY CHECK页面操作（保存数据参数）,格式：
                        //查询曲线图数据：服务器->客户端：指令类型 + 相机类型数据 + 是否保存数据（1，是；0，否）

                        for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                        {
                            if ((Global.CameraChooseState & (0x01 << index)) != 0)//当前相机开启
                            {
                                if ((VisionSystemClassLibrary.Enum.CameraType)(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1]) == Global.CameraTemp[index].Type)//相机类型相同
                                {
                                    //客户端->服务端：指令类型 + 相机类型数据 + 保存数据结果（1，成功；0，失败）
                                    Boolean bQualityCheckSave = Convert.ToBoolean(BitConverter.ToInt32(ClientData.ReceivedData, ClientData.DataInfo.InstructionIndex + 2));//解析是否保存数据（1，是；0，否）

                                    if (!bQualityCheckSave)//退出质量检测界面
                                    {
                                        LabModel = false;

                                        if (Global.Camera[index].IsSerialPort) //当前为串口
                                        {
                                            bSerialPortLabState[index] = false;

                                            lock (LockSerialPort[index])
                                            {
                                                _SendCommand_SerialPortCommucation(1, index);//发送实验室状态
                                            }

                                            this.Invoke(new MethodInvoker(delegate { timer4.Enabled = false; }));
                                            this.Invoke(new MethodInvoker(delegate { timer_89713FA.Enabled = false; }));
                                        }
                                    }
                                    ClientControl._Send(_GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1], BitConverter.GetBytes(_QualityCheckOperate(index, 2, bQualityCheckSave))));//发送数据

                                    break;
                                }
                            }
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.QualityCheck_LearnSample:

                        //QUALITY CHECK页面操作（自学阈值习或更新）,格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 图像工具数据（1，包含工具；0，不包含工具） + 图像尺寸类型数据 + 工具索引 + 图像类型（1，在线；2，学习；3，剔除）
                        
                        VisionSystemCommunicationLibrary.Ethernet.SerializableData learnSampleEthernetData = _EthernetDataDeserialize(ClientData);

                        for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                        {
                            if ((Global.CameraChooseState & (0x01 << index)) != 0)//当前相机开启
                            {
                                if ((VisionSystemClassLibrary.Enum.CameraType)(learnSampleEthernetData.Data_0[0]) == Global.CameraTemp[index].Type)//相机类型相同
                                {
                                    //客户端->服务端：指令类型 + 相机类型数据 + 图像工具数据（1，包含工具；0，不包含工具） + 图像尺寸类型数据 + 图像宽度数据 + 图像高度数据 + 图像信息数据 + 图像数据
                        
                                    Boolean originalView = !Convert.ToBoolean(BitConverter.ToBoolean(learnSampleEthernetData.Data_1, 0));
                                    Double imageScale = BitConverter.ToDouble(learnSampleEthernetData.Data_2, 0);//解析图像尺寸
                                    Int32 imageType = BitConverter.ToInt32(learnSampleEthernetData.Data_3, 0);//解析图像类型

                                    Byte[] Data_QualityCheck_LoadSample = null;//待发送的数据

                                    try
                                    {
                                        switch (imageType)
                                        {
                                            case 1:
                                                Global.CameraTemp[index].ImageLearn.SetZero();
                                                Global.CameraTemp[index].ImageLearn._Or(ImageSourseQualityCheck[index]);
                                                _LearnImageProcessing(index, Global.CameraTemp[index].ImageLearn, QualityCheckToolIndex[index]);

                                                Data_QualityCheck_LoadSample = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ref learnSampleEthernetData, Global.CameraTemp[index].Learn, Global.CameraTemp[index].ImageLearn.Resize(imageScale, INTER.CV_INTER_AREA));//生成指令
                                                ClientControl._Send(Data_QualityCheck_LoadSample);//发送数据
                                                break;
                                            case 2:
                                                _LearnImageProcessing(index, Global.CameraTemp[index].ImageLearn, QualityCheckToolIndex[index]);

                                                Data_QualityCheck_LoadSample = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ref learnSampleEthernetData, Global.CameraTemp[index].Learn, Global.CameraTemp[index].ImageLearn.Resize(imageScale, INTER.CV_INTER_AREA));//生成指令
                                                ClientControl._Send(Data_QualityCheck_LoadSample);//发送数据
                                                break;
                                            case 3:
                                                Global.CameraTemp[index].ImageLearn.SetZero();
                                                Global.CameraTemp[index].ImageLearn._Or(Global.CameraTemp[index].ImageReject);
                                                _LearnImageProcessing(index, Global.CameraTemp[index].ImageLearn, QualityCheckToolIndex[index]);

                                                Data_QualityCheck_LoadSample = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ref learnSampleEthernetData, Global.CameraTemp[index].Learn, Global.CameraTemp[index].ImageLearn.Resize(imageScale, INTER.CV_INTER_AREA));//生成指令
                                                ClientControl._Send(Data_QualityCheck_LoadSample);//发送数据
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                    catch (System.Exception ex)
                                    {
                                        Data_QualityCheck_LoadSample = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ref learnSampleEthernetData, new VisionSystemClassLibrary.Struct.ImageInformation());//生成指令
                                        ClientControl._Send(Data_QualityCheck_LoadSample);//发送数据
                                    }

                                    break;
                                }
                            }
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.QualityCheck_LoadSample:

                        //REJECTS VIEW页面操作，自学习阈值或更新，格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 图像工具数据（1，包含工具；0，不包含工具） + 图像尺寸类型数据
                        
                        VisionSystemCommunicationLibrary.Ethernet.SerializableData loadSampleEthernetData = _EthernetDataDeserialize(ClientData);

                        for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                        {
                            if ((Global.CameraChooseState & (0x01 << index)) != 0)//当前相机开启
                            {
                                if ((VisionSystemClassLibrary.Enum.CameraType)(loadSampleEthernetData.Data_0[0]) == Global.CameraTemp[index].Type)//相机类型相同
                                {
                                    //客户端->服务端：指令类型 + 相机类型数据 + 图像工具数据（1，包含工具；0，不包含工具） + 图像尺寸类型数据 + 图像宽度数据 + 图像高度数据 + 图像信息数据 + 图像数据

                                    Boolean originalView = !Convert.ToBoolean(BitConverter.ToBoolean(loadSampleEthernetData.Data_1, 0));
                                    Double imageScale = BitConverter.ToDouble(loadSampleEthernetData.Data_2, 0);//解析图像尺寸

                                    Byte[] Data_QualityCheck_LoadSample;//待发送的数据

                                    if (VisionSystemClassLibrary.Enum.CameraState.OFF == Global.CameraTemp[index].DeviceInformation.CAM)//相机关闭
                                    {
                                        Data_QualityCheck_LoadSample = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ref loadSampleEthernetData, new VisionSystemClassLibrary.Struct.ImageInformation());//生成指令
                                        ClientControl._Send(Data_QualityCheck_LoadSample);//发送数据
                                    }
                                    else//相机打开，相机未更新
                                    {
                                        try
                                        {
                                            VisionSystemClassLibrary.Struct.ImageInformation loadSampleImageGraphicsInformation = new VisionSystemClassLibrary.Struct.ImageInformation();

                                            _UpdateQualityCheckGraphicsInformation(index, ref loadSampleImageGraphicsInformation, 2);//计算当前工具处理结果

                                            Data_QualityCheck_LoadSample = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ref loadSampleEthernetData, loadSampleImageGraphicsInformation, Global.CameraTemp[index].ImageLearn.Resize(imageScale, INTER.CV_INTER_AREA));//生成指令
                                            ClientControl._Send(Data_QualityCheck_LoadSample);//发送数据
                                        }
                                        catch (System.Exception ex)
                                        {
                                            Data_QualityCheck_LoadSample = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ref loadSampleEthernetData, new VisionSystemClassLibrary.Struct.ImageInformation());//生成指令
                                            ClientControl._Send(Data_QualityCheck_LoadSample);//发送数据
                                        }
                                    }

                                    break;
                                }
                            }
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.QualityCheck_LiveView:

                        //REJECTS VIEW页面操作，查看实时图像，格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 图像工具数据（1，包含工具；0，不包含工具） + 图像尺寸类型数据
                        
                        VisionSystemCommunicationLibrary.Ethernet.SerializableData liveViewEthernetData = _EthernetDataDeserialize(ClientData);

                        for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                        {
                            if ((Global.CameraChooseState & (0x01 << index)) != 0)//当前相机开启
                            {
                                if ((VisionSystemClassLibrary.Enum.CameraType)(liveViewEthernetData.Data_0[0]) == Global.CameraTemp[index].Type)//相机类型相同
                                {
                                    //客户端->服务端：指令类型 + 相机类型数据 + 图像工具数据（1，包含工具；0，不包含工具） + 图像尺寸类型数据 + 图像宽度数据 + 图像高度数据 + 图像信息数据 + 图像数据

                                    Boolean originalView = !Convert.ToBoolean(BitConverter.ToBoolean(liveViewEthernetData.Data_1, 0));
                                    Double imageScale = BitConverter.ToDouble(liveViewEthernetData.Data_2, 0);//解析图像尺寸

                                    Byte[] Data_QualityCheck_LiveView;//待发送的数据

                                    if (VisionSystemClassLibrary.Enum.CameraState.OFF == Global.CameraTemp[index].DeviceInformation.CAM)//相机关闭
                                    {
                                        Data_QualityCheck_LiveView = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ref liveViewEthernetData, new VisionSystemClassLibrary.Struct.ImageInformation());//生成指令
                                        ClientControl._Send(Data_QualityCheck_LiveView);//发送数据
                                    }
                                    else//相机打开，相机未更新
                                    {
                                        try
                                        {
                                            lock (LockSourseImageBuffPort[index])
                                            {
                                                VisionSystemClassLibrary.Struct.ImageInformation liveViewImageGraphicsInformation = new VisionSystemClassLibrary.Struct.ImageInformation();

                                                switch (SourseImageBuffIndexPort[index])                              //当前正在执行保存图像缓冲区索引
                                                {
                                                    case 0:
                                                        if (SourseImageBuff0FlagPort[index])
                                                        {
                                                            ImageSourseQualityCheck[index].SetZero();
                                                            ImageSourseQualityCheck[index]._Or(SourseImageBuffPort[index][SourseImageBuffIndexPort[index]]);
                                                        }

                                                        if (SourseImageBuff1FlagPort[index])
                                                        {
                                                            ImageSourseQualityCheck[index].SetZero();
                                                            ImageSourseQualityCheck[index]._Or(SourseImageBuffPort[index][1 - SourseImageBuffIndexPort[index]]);
                                                        }

                                                        _UpdateQualityCheckGraphicsInformation(index, ref liveViewImageGraphicsInformation);//计算当前工具处理结果

                                                        Data_QualityCheck_LiveView = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ref liveViewEthernetData, liveViewImageGraphicsInformation, ImageSourseQualityCheck[index].Resize(imageScale, INTER.CV_INTER_AREA));//生成指令
                                                        ClientControl._Send(Data_QualityCheck_LiveView);//发送数据
                                                        break;
                                                    case 1:
                                                        if (SourseImageBuff1FlagPort[index])
                                                        {
                                                            ImageSourseQualityCheck[index].SetZero();
                                                            ImageSourseQualityCheck[index]._Or(SourseImageBuffPort[index][SourseImageBuffIndexPort[index]]);
                                                        }

                                                        if (SourseImageBuff0FlagPort[index])
                                                        {
                                                            ImageSourseQualityCheck[index].SetZero();
                                                            ImageSourseQualityCheck[index]._Or(SourseImageBuffPort[index][1 - SourseImageBuffIndexPort[index]]);
                                                        }

                                                        _UpdateQualityCheckGraphicsInformation(index, ref liveViewImageGraphicsInformation);//计算当前工具处理结果

                                                        Data_QualityCheck_LiveView = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ref liveViewEthernetData, liveViewImageGraphicsInformation, ImageSourseQualityCheck[index].Resize(imageScale, INTER.CV_INTER_AREA));//生成指令
                                                        ClientControl._Send(Data_QualityCheck_LiveView);//发送数据
                                                        break;
                                                    default:
                                                        Data_QualityCheck_LiveView = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ref liveViewEthernetData, new VisionSystemClassLibrary.Struct.ImageInformation());//生成指令
                                                        ClientControl._Send(Data_QualityCheck_LiveView);//发送数据
                                                        break;
                                                }
                                            }
                                        }
                                        catch (System.Exception ex)
                                        {
                                            Data_QualityCheck_LiveView = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ref liveViewEthernetData, new VisionSystemClassLibrary.Struct.ImageInformation());//生成指令
                                            ClientControl._Send(Data_QualityCheck_LiveView);//发送数据
                                        }
                                    }

                                    break;
                                }
                            }
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.QualityCheck_GetSelectedRecordData:
                        //QualityCheck页面，获取当前选择的统计数据，格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 关联标记 + 统计数据类型（0，最新统计数据；1，指定统计数据）+ 班次索引（非0） + 统计数据开始结束时间
                       
                        for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                        {
                            if ((Global.CameraChooseState & (0x01 << index)) != 0)//当前相机开启
                            {
                                if ((VisionSystemClassLibrary.Enum.CameraType)(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1]) == Global.Camera[index].Type)//相机类型相同
                                {
                                    //客户端->服务端：指令类型 + 相机类型数据 + 关联标记 + 统计数据类型（0，最新统计数据；1，指定统计数据） + 班次索引（非0） + 统计数据开始结束时间  +  剔除数量

                                    Int32 iIndex = ClientData.DataInfo.InstructionIndex + 2;
                                    Boolean bRelevancy = false;
                                    Byte[] bRelevancyBytes = new Byte[4];
                                    if (0 != BitConverter.ToInt32(ClientData.ReceivedData, iIndex))//解析关联标记
                                    {
                                        bRelevancy = true;
                                        bRelevancyBytes = BitConverter.GetBytes(1);
                                    }

                                    iIndex += 4;
                                    Int32 statisticsType = BitConverter.ToInt32(ClientData.ReceivedData, iIndex);//解析统计类型

                                    iIndex += 4;
                                    Int32 shiftIndex = BitConverter.ToInt32(ClientData.ReceivedData, iIndex);//解析班次索引

                                    VisionSystemClassLibrary.Struct.SYSTEMTIME startTime = new VisionSystemClassLibrary.Struct.SYSTEMTIME();
                                    VisionSystemClassLibrary.Struct.SYSTEMTIME endTime = new VisionSystemClassLibrary.Struct.SYSTEMTIME();

                                    iIndex += 4;
                                    _GetSystemTime(ClientData.ReceivedData, ref iIndex, ref startTime, ref endTime);//解析班次起止时间

                                    VisionSystemClassLibrary.Struct.ShiftTime shiftTimeBuf = new VisionSystemClassLibrary.Struct.ShiftTime();
                                    shiftTimeBuf.Start = startTime;
                                    shiftTimeBuf.End = endTime;

                                    VisionSystemClassLibrary.Struct.ShiftTime shiftTime = Global.ShiftInformation._GetCurrentShiftTimeData(Global.ShiftInformation.CurrentShiftIndex);

                                    if ((statisticsType == 0) || (VisionSystemClassLibrary.Class.Shift._Compare(shiftTimeBuf, shiftTime)))//最新班次
                                    {
                                        statisticsType = 0;
                                        shiftIndex = Global.ShiftInformation.CurrentShiftIndex;
                                        startTime = shiftTime.Start;
                                        endTime = shiftTime.End;
                                    }

                                    string brandName = "";
                                    UInt32 inspectedNumber = 0, rejectedNumber = 0;
                                    VisionSystemClassLibrary.Enum.CameraType typeOfCamera = new VisionSystemClassLibrary.Enum.CameraType();
                                    UInt32[] rejectedStatistics_Tool = null;

                                    if (bRelevancy) //关联查询
                                    {
                                        Global.ShiftInformation._UpdateHistoryShift(Global.Camera[index].DeviceInformation.Port, shiftIndex, startTime, endTime, ref typeOfCamera, ref brandName, ref inspectedNumber, ref rejectedNumber, ref rejectedStatistics_Tool, Global.Camera[index].RelevancyCameraInfo.rRelevancyType);
                                    }
                                    else
                                    {
                                        Global.ShiftInformation._UpdateHistoryShift(Global.Camera[index].DeviceInformation.Port, shiftIndex, startTime, endTime, ref typeOfCamera, ref brandName, ref inspectedNumber, ref rejectedNumber, ref rejectedStatistics_Tool);
                                    }

                                    Byte[] shiftTimeDate = _GenerateSystemTimeBytes(startTime, endTime);//生成班次起止时间数组

                                    Byte[] Data_QualityCheck__GetSelectedRecordData = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1], bRelevancyBytes, BitConverter.GetBytes(statisticsType), BitConverter.GetBytes(shiftIndex), shiftTimeDate, BitConverter.GetBytes(rejectedNumber)); //生成客户端数据
                                    ClientControl._Send(Data_QualityCheck__GetSelectedRecordData);//发送数据

                                    break;
                                }
                            }
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.QualityCheck_LoadReject_Click:

                        //查看剔除图像，格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 关联标记 + 班次索引（非0） + 统计数据开始结束时间 + 剔除图像对应的工具索引值（从0开始） + 剔除图像对应的工具中的索引值（从0开始） + 图像尺寸类型数据

                        for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                        {
                            if ((Global.CameraChooseState & (0x01 << index)) != 0)//当前相机开启
                            {
                                if ((VisionSystemClassLibrary.Enum.CameraType)(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1]) == Global.Camera[index].Type)//相机类型相同
                                {
                                    //客户端->服务端：指令类型 + 相机类型数据 + 关联标记 + 班次索引（非0） + 统计数据开始结束时间 + 剔除图像对应的工具索引值（从0开始） + 剔除图像对应的工具中的索引值（从0开始） + 图像尺寸类型数据 + 图像宽度数据 + 图像高度数据 + 图像信息数据 + 图像数据

                                    Int32 iIndex = ClientData.DataInfo.InstructionIndex + 2;
                                    Boolean bRelevancy = false;
                                    Byte[] bRelevancyBytes = new Byte[4];
                                    if (0 != BitConverter.ToInt32(ClientData.ReceivedData, iIndex))//解析关联标记
                                    {
                                        bRelevancy = true;
                                        bRelevancyBytes = BitConverter.GetBytes(1);
                                    }

                                    iIndex += 4;
                                    Int32 shiftIndex = BitConverter.ToInt32(ClientData.ReceivedData, iIndex);//解析班次索引

                                    VisionSystemClassLibrary.Struct.SYSTEMTIME startTime = new VisionSystemClassLibrary.Struct.SYSTEMTIME();
                                    VisionSystemClassLibrary.Struct.SYSTEMTIME endTime = new VisionSystemClassLibrary.Struct.SYSTEMTIME();

                                    iIndex += 4;
                                    _GetSystemTime(ClientData.ReceivedData, ref iIndex, ref startTime, ref endTime);//解析班次起止时间

                                    iIndex += 2;
                                    Int32 toolIndex = BitConverter.ToInt32(ClientData.ReceivedData, iIndex);//解析工具索引

                                    iIndex += 4;
                                    Int32 imageIndex = BitConverter.ToInt32(ClientData.ReceivedData, iIndex);//解析图像索引

                                    iIndex += 4;
                                    Double imageScale = BitConverter.ToDouble(ClientData.ReceivedData, iIndex);//解析图像尺寸

                                    VisionSystemClassLibrary.Struct.ShiftTime shiftTimeBuf = new VisionSystemClassLibrary.Struct.ShiftTime();
                                    shiftTimeBuf.Start = startTime;
                                    shiftTimeBuf.End = endTime;

                                    VisionSystemClassLibrary.Struct.ShiftTime shiftTime = Global.ShiftInformation._GetCurrentShiftTimeData(Global.ShiftInformation.CurrentShiftIndex);

                                    if (VisionSystemClassLibrary.Class.Shift._Compare(shiftTimeBuf, shiftTime))//最新班次
                                    {
                                        shiftIndex = Global.ShiftInformation.CurrentShiftIndex;
                                        startTime = shiftTime.Start;
                                        endTime = shiftTime.End;
                                    }

                                    Image<Bgr, Byte> toolImage = null;//缺陷图像
                                    VisionSystemClassLibrary.Struct.ImageInformation toolImageInformation = new VisionSystemClassLibrary.Struct.ImageInformation();//缺陷图像信息
                                    toolImageInformation._InitData();

                                    VisionSystemClassLibrary.Struct.ImageInformation loadRejectClickImageGraphicsInformation = new VisionSystemClassLibrary.Struct.ImageInformation();

                                    try
                                    {
                                        Boolean bToolState = false;
                                        Boolean bDeepLearningState = false;
                                        string sTypeName = "OK";

                                        if (bRelevancy) //关联查询
                                        {
                                            Global.ShiftInformation._UpdateHistoryShiftImage(Global.Camera[index].DeviceInformation.Port, shiftIndex, startTime, endTime, toolIndex, imageIndex, ref toolImage, ref toolImageInformation, ref bToolState, ref bDeepLearningState, ref sTypeName, Global.Camera[index].RelevancyCameraInfo.rRelevancyType);
                                        }
                                        else
                                        {
                                            Global.ShiftInformation._UpdateHistoryShiftImage(Global.Camera[index].DeviceInformation.Port, shiftIndex, startTime, endTime, toolIndex, imageIndex, ref toolImage, ref toolImageInformation, ref bToolState, ref bDeepLearningState, ref sTypeName);
                                        }

                                        Global.CameraTemp[index].ImageReject = toolImage.Copy();

                                        _UpdateQualityCheckGraphicsInformation(index, ref loadRejectClickImageGraphicsInformation, 3);//计算当前工具处理结果
                                    }
                                    catch (System.Exception ex)
                                    {

                                    }

                                    Byte[] Data_QualityCheck_LoadReject_Click;

                                    if (toolImage != null)//当前图像有效
                                    {
                                        Data_QualityCheck_LoadReject_Click = _GenerateInstruction(ClientData, loadRejectClickImageGraphicsInformation, toolImage);//生成客户端数据
                                    }
                                    else
                                    {
                                        Data_QualityCheck_LoadReject_Click = _GenerateInstruction(ClientData, loadRejectClickImageGraphicsInformation);//生成客户端数据
                                    }
                                    ClientControl._Send(Data_QualityCheck_LoadReject_Click);//发送数据

                                    break;
                                }
                            }
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.QualityCheck_ManageTools:

                        //REJECTS VIEW页面操作，工具管理，格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 启用工具标记 + 当前工具索引 + 图像类型（1，在线；2，学习；3，剔除）
                        
                        VisionSystemCommunicationLibrary.Ethernet.SerializableData manageToolsEthernetData = _EthernetDataDeserialize(ClientData);

                        for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                        {
                            if ((Global.CameraChooseState & (0x01 << index)) != 0)//当前相机开启
                            {
                                if ((VisionSystemClassLibrary.Enum.CameraType)(manageToolsEthernetData.Data_0[0]) == Global.CameraTemp[index].Type)//相机类型相同
                                {
                                    //客户端->服务端：指令类型 + 相机类型数据 + 启用工具标记更新结果（1，成功；0，不成功） + 图像信息数据
                                    
                                    QualityCheckToolIndex[index] = BitConverter.ToInt32(manageToolsEthernetData.Data_2, 0);//解析工具索引
                                    Int32 imageType = BitConverter.ToInt32(manageToolsEthernetData.Data_3, 0);//解析工具索引

                                    if (manageToolsEthernetData.Data_1 != null)//工具长度有效
                                    {
                                        Int32 toolStateLength = manageToolsEthernetData.Data_1.Length / 4;
                                        for (Int32 i = 0; i < toolStateLength; i++)//设置工具启用状态
                                        {
                                            Global.CameraTemp[index].Tools[i].ToolState = Convert.ToBoolean(BitConverter.ToInt32(manageToolsEthernetData.Data_1, i * 4));
                                        }
                                        VisionSystemClassLibrary.Struct.ImageInformation imageGraphicsInformation = new VisionSystemClassLibrary.Struct.ImageInformation();
                                        _UpdateQualityCheckGraphicsInformation(index, ref imageGraphicsInformation, imageType);

                                        Byte[] Data_QualityCheck_ManageTools = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], manageToolsEthernetData.Data_0[0], BitConverter.GetBytes(1), _GenerateImageInformation(imageGraphicsInformation));//生成客户端数据
                                        ClientControl._Send(Data_QualityCheck_ManageTools);//发送数据
                                    }

                                    break;
                                }
                            }
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.QualityCheck_ToolParamter:

                        //REJECTS VIEW页面操作，工具参数，格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 工具索引 + 图像类型（1，在线；2，学习；3，剔除） + 工具参数
                        
                        VisionSystemCommunicationLibrary.Ethernet.SerializableData toolParamterEthernetData = _EthernetDataDeserialize(ClientData);

                        for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                        {
                            if ((Global.CameraChooseState & (0x01 << index)) != 0)//当前相机开启
                            {
                                if ((VisionSystemClassLibrary.Enum.CameraType)(toolParamterEthernetData.Data_0[0]) == Global.CameraTemp[index].Type)//相机类型相同
                                {
                                    //客户端->服务端：指令类型 + 相机类型数据 + 工具参数更新结果（1，成功；0，不成功） + 图像信息数据

                                    QualityCheckToolIndex[index] = BitConverter.ToInt32(toolParamterEthernetData.Data_1, 0);//解析工具索引
                                    Int32 imageType = BitConverter.ToInt32(toolParamterEthernetData.Data_2, 0);//解析图像尺寸

                                    MemoryStream toolParamterMemoryStream = new MemoryStream();//流对象
                                    toolParamterMemoryStream.Write(toolParamterEthernetData.Data_4, 0, toolParamterEthernetData.Data_4.Length);//序列化数据写入流

                                    IFormatter toolParamterFormatter = new SoapFormatter();//格式化对象
                                    toolParamterMemoryStream.Position = 0;
                                    Global.CameraTemp[index].Tools[QualityCheckToolIndex[index]].Arithmetic = (VisionSystemClassLibrary.Struct.Arithmetic)toolParamterFormatter.Deserialize(toolParamterMemoryStream);//反序列化

                                    VisionSystemClassLibrary.Struct.ImageInformation imageGraphicsInformation = new VisionSystemClassLibrary.Struct.ImageInformation();

                                    if (Global.Camera[index].Tools[QualityCheckToolIndex[index]].Type == 0) //格子工具
                                    {
                                        _LearnToolProcessing(index, 2);
                                    }
                                    _UpdateQualityCheckGraphicsInformation(index, ref imageGraphicsInformation, imageType);

                                    Byte[] Data_QualityCheck_ManageTools = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], toolParamterEthernetData.Data_0[0], BitConverter.GetBytes(1), _GenerateImageInformation(imageGraphicsInformation));//生成客户端数据
                                    ClientControl._Send(Data_QualityCheck_ManageTools);//发送数据

                                    break;
                                }
                            }
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.QualityCheck_WorkArea:

                        //QUALITY CHECK页面操作（工作区域）,格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 工具索引 + 图像类型（1，在线；2，学习；3，剔除） + 工作区域横坐标 + 工作区域纵坐标 + 工作区域宽度 + 工作区域高度
                        
                        for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                        {
                            if ((Global.CameraChooseState & (0x01 << index)) != 0)//当前相机开启
                            {
                                if ((VisionSystemClassLibrary.Enum.CameraType)(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1]) == Global.CameraTemp[index].Type)//相机类型相同
                                {
                                    //客户端->服务端：指令类型 + 相机类型数据 + 设置结果（1，成功；0，不成功） + 图像信息数据

                                    Int32 iIndex = ClientData.DataInfo.InstructionIndex + 2;
                                    QualityCheckToolIndex[index] = BitConverter.ToInt32(ClientData.ReceivedData, iIndex);//解析工具索引
                                    iIndex += 4;
                                    Int32 imageType = BitConverter.ToInt32(ClientData.ReceivedData, iIndex);//解析图像类型
                                    iIndex += 4;
                                    Global.CameraTemp[index].Tools[QualityCheckToolIndex[index]].ROI.roiExtra.roiType = (VisionSystemClassLibrary.Enum.ROIType)BitConverter.ToInt32(ClientData.ReceivedData, iIndex);//解析工作区域类型
                                    iIndex += 4;
                                    Global.CameraTemp[index].Tools[QualityCheckToolIndex[index]].ROI.roiExtra.Point1.X = BitConverter.ToInt32(ClientData.ReceivedData, iIndex);//解析工作区域
                                    iIndex += 4;
                                    Global.CameraTemp[index].Tools[QualityCheckToolIndex[index]].ROI.roiExtra.Point1.Y = BitConverter.ToInt32(ClientData.ReceivedData, iIndex);//解析工作区域
                                    iIndex += 4;
                                    Global.CameraTemp[index].Tools[QualityCheckToolIndex[index]].ROI.roiExtra.Point2.X = BitConverter.ToInt32(ClientData.ReceivedData, iIndex);//解析工作区域
                                    iIndex += 4;
                                    Global.CameraTemp[index].Tools[QualityCheckToolIndex[index]].ROI.roiExtra.Point2.Y = BitConverter.ToInt32(ClientData.ReceivedData, iIndex);//解析工作区域
                                    iIndex += 4;
                                    Global.CameraTemp[index].Tools[QualityCheckToolIndex[index]].ROI.roiExtra.Point3.X = BitConverter.ToInt32(ClientData.ReceivedData, iIndex);//解析工作区域
                                    iIndex += 4;
                                    Global.CameraTemp[index].Tools[QualityCheckToolIndex[index]].ROI.roiExtra.Point3.Y = BitConverter.ToInt32(ClientData.ReceivedData, iIndex);//解析工作区域
                                    iIndex += 4;
                                    Global.CameraTemp[index].Tools[QualityCheckToolIndex[index]].ROI.roiExtra.Point4.X = BitConverter.ToInt32(ClientData.ReceivedData, iIndex);//解析工作区域
                                    iIndex += 4;
                                    Global.CameraTemp[index].Tools[QualityCheckToolIndex[index]].ROI.roiExtra.Point4.Y = BitConverter.ToInt32(ClientData.ReceivedData, iIndex);//解析工作区域
                                    iIndex += 4;
                                    if (0 != BitConverter.ToInt32(ClientData.ReceivedData, iIndex))//解析工作区域
                                    {
                                        Global.CameraTemp[index].Tools[QualityCheckToolIndex[index]].ROI.roiInnerExit = true;
                                    }
                                    else
                                    {
                                        Global.CameraTemp[index].Tools[QualityCheckToolIndex[index]].ROI.roiInnerExit = false;
                                    }
                                    iIndex += 4;
                                    Global.CameraTemp[index].Tools[QualityCheckToolIndex[index]].ROI.roiInner.roiType = (VisionSystemClassLibrary.Enum.ROIType)BitConverter.ToInt32(ClientData.ReceivedData, iIndex);//解析工作区域类型
                                    iIndex += 4;
                                    Global.CameraTemp[index].Tools[QualityCheckToolIndex[index]].ROI.roiInner.Point1.X = BitConverter.ToInt32(ClientData.ReceivedData, iIndex);//解析工作区域
                                    iIndex += 4;
                                    Global.CameraTemp[index].Tools[QualityCheckToolIndex[index]].ROI.roiInner.Point1.Y = BitConverter.ToInt32(ClientData.ReceivedData, iIndex);//解析工作区域
                                    iIndex += 4;
                                    Global.CameraTemp[index].Tools[QualityCheckToolIndex[index]].ROI.roiInner.Point2.X = BitConverter.ToInt32(ClientData.ReceivedData, iIndex);//解析工作区域
                                    iIndex += 4;
                                    Global.CameraTemp[index].Tools[QualityCheckToolIndex[index]].ROI.roiInner.Point2.Y = BitConverter.ToInt32(ClientData.ReceivedData, iIndex);//解析工作区域
                                    iIndex += 4;
                                    Global.CameraTemp[index].Tools[QualityCheckToolIndex[index]].ROI.roiInner.Point3.X = BitConverter.ToInt32(ClientData.ReceivedData, iIndex);//解析工作区域
                                    iIndex += 4;
                                    Global.CameraTemp[index].Tools[QualityCheckToolIndex[index]].ROI.roiInner.Point3.Y = BitConverter.ToInt32(ClientData.ReceivedData, iIndex);//解析工作区域
                                    iIndex += 4;
                                    Global.CameraTemp[index].Tools[QualityCheckToolIndex[index]].ROI.roiInner.Point4.X = BitConverter.ToInt32(ClientData.ReceivedData, iIndex);//解析工作区域
                                    iIndex += 4;
                                    Global.CameraTemp[index].Tools[QualityCheckToolIndex[index]].ROI.roiInner.Point4.Y = BitConverter.ToInt32(ClientData.ReceivedData, iIndex);//解析工作区域

                                    Rectangle rect = VisionSystemClassLibrary.GeneralFunction._GetMinRect(Global.CameraTemp[index].Tools[QualityCheckToolIndex[index]].ROI.roiExtra);

                                    if (Global.Camera[index].Tools[QualityCheckToolIndex[index]].ReferenceH_Exist)//当前工具为水平基准
                                    {
                                        Global.CameraTemp[index].Tools[QualityCheckToolIndex[index]].Min = rect.Left + 2;//给出下限,偏移2像素
                                        Global.CameraTemp[index].Tools[QualityCheckToolIndex[index]].Max = rect.Right - 2;//给出上限,偏移2像素
                                    }

                                    if (Global.Camera[index].Tools[QualityCheckToolIndex[index]].ReferenceV_Exist)//当前工具为垂直基准
                                    {
                                        Global.CameraTemp[index].Tools[QualityCheckToolIndex[index]].Min = rect.Top + 2;//给出下限,偏移2像素
                                        Global.CameraTemp[index].Tools[QualityCheckToolIndex[index]].Max = rect.Bottom - 2;//给出上限,偏移2像素
                                    }

                                    if (Global.Camera[index].IsSerialPort) //当前为串口
                                    {
                                        if (VisionSystemClassLibrary.Enum.SensorProductType._89713FA == Global.Camera[index].Sensor_ProductType)
                                        {
                                            lock (LockSerialPort[index])
                                            {
                                                for (Byte k = 0; k < Global.Camera[index].PerTobaccoNumber.Count; k++)
                                                {
                                                    _SendCommand_SerialPortCommucation(6, index, false, k);//发送光电各烟支检测相位及区间
                                                }
                                            }
                                        }
                                    }
                                    VisionSystemClassLibrary.Struct.ImageInformation imageGraphicsInformation = new VisionSystemClassLibrary.Struct.ImageInformation();

                                    if (Global.Camera[index].Tools[QualityCheckToolIndex[index]].Type == 0) //格子工具
                                    {
                                        _LearnToolProcessing(index, 2);
                                    }
                                    _UpdateQualityCheckGraphicsInformation(index, ref imageGraphicsInformation, imageType);

                                    Byte[] Data_QualityCheck_WorkArea = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1], BitConverter.GetBytes(1), _GenerateImageInformation(imageGraphicsInformation));//生成客户端数据
                                    ClientControl._Send(Data_QualityCheck_WorkArea);//发送数据

                                    break;
                                }
                            }
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.QualityCheck_Enter:

                        //QUALITY CHECK页面操作（进入页面），格式：
                        //查询曲线图数据：服务器->客户端：指令类型 + 相机类型数据 + 工具索引

                        for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                        {
                            if ((Global.CameraChooseState & (0x01 << index)) != 0)//当前相机开启
                            {
                                if ((VisionSystemClassLibrary.Enum.CameraType)(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1]) == Global.CameraTemp[index].Type)//相机类型相同
                                {
                                    //客户端->服务端：指令类型 + 相机类型数据 + 工具索引 + 操作结果（1，成功；0，失败）
                                    QualityCheckToolIndex[index] = BitConverter.ToInt32(ClientData.ReceivedData, ClientData.DataInfo.InstructionIndex + 2);//解析工具索引

                                    Byte[] Data_QualityCheck_Enter = _GenerateInstruction(ClientData, BitConverter.GetBytes(_QualityCheckOperate(index, 1)));//生成客户端数据
                                    ClientControl._Send(Data_QualityCheck_Enter);//发送数据

                                    break;
                                }
                            }
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.QualityCheck_CurrentTool:

                        //QUALITY CHECK页面操作（进入页面），格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 工具索引 + 图像类型（1，在线；2，学习；3，剔除）

                        for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                        {
                            if ((Global.CameraChooseState & (0x01 << index)) != 0)//当前相机开启
                            {
                                if ((VisionSystemClassLibrary.Enum.CameraType)(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1]) == Global.CameraTemp[index].Type)//相机类型相同
                                {
                                    //客户端->服务端：指令类型 + 相机类型数据 + 当前工具设置结果（1，成功；0，不成功） + 图像信息数据

                                    Int32 iIndex = ClientData.DataInfo.InstructionIndex + 2;
                                    QualityCheckToolIndex[index] = BitConverter.ToInt32(ClientData.ReceivedData, iIndex);//解析工具索引

                                    iIndex += 4;
                                    Int32 imageType = BitConverter.ToInt32(ClientData.ReceivedData, iIndex);//解析图像类型
                                    
                                    VisionSystemClassLibrary.Struct.ImageInformation imageGraphicsInformation = new VisionSystemClassLibrary.Struct.ImageInformation();
                                    _UpdateQualityCheckGraphicsInformation(index, ref imageGraphicsInformation, imageType);

                                    Byte[] Data_QualityCheck_CurrentTool = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1], BitConverter.GetBytes(1), _GenerateImageInformation(imageGraphicsInformation));//生成客户端数据
                                    ClientControl._Send(Data_QualityCheck_CurrentTool);//发送数据

                                    break;
                                }
                            }
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.QualityCheck_TolerancesValue:

                        //QUALITY CHECK页面操作（公差上、下限），格式：
                        //图像学习完成后，获取公差上下限：服务端->客户端：指令类型 + 相机类型数据

                        for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                        {
                            if ((Global.CameraChooseState & (0x01 << index)) != 0)//当前相机开启
                            {
                                if ((VisionSystemClassLibrary.Enum.CameraType)(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1]) == Global.CameraTemp[index].Type)//相机类型相同
                                {
                                    //客户端->服务端：指令类型 + 相机类型数据 + 公差个数 + （每个）公差下限、上限

                                    Int32 iIndex = 0;
                                    Byte[] tolerancesDataMinAndMax = new Byte[2 * 4 * Global.CameraTemp[index].Tolerances.GraphData.Count];

                                    for (Int32 i = 0; i < Global.CameraTemp[index].Tolerances.GraphData.Count; i++) //遍历当前所有工具
                                    {
                                        BitConverter.GetBytes((Int32)Global.CameraTemp[index].Tools[Global.CameraTemp[index].Tolerances.GraphData[i].ToolsIndex].Min).CopyTo(tolerancesDataMinAndMax, iIndex);
                                        iIndex += 4;

                                        BitConverter.GetBytes((Int32)Global.CameraTemp[index].Tools[Global.CameraTemp[index].Tolerances.GraphData[i].ToolsIndex].Max).CopyTo(tolerancesDataMinAndMax, iIndex);
                                        iIndex += 4;
                                    }

                                    Byte[] Data_QualityCheck_TolerancesValue = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1], BitConverter.GetBytes((Int32)Global.CameraTemp[index].Tolerances.GraphData.Count), tolerancesDataMinAndMax);//生成客户端数据
                                    ClientControl._Send(Data_QualityCheck_TolerancesValue);//发送数据

                                    break;
                                }
                            }
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.TitleBar_State:

                        //QUALITY CHECK页面操作（设备状态）,格式：
                        //服务器->客户端：指令类型 + 相机类型数据 + 设备状态（VisionSystemClassLibrary.Enum.DeviceState）

                        for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                        {
                            if ((Global.CameraChooseState & (0x01 << index)) != 0)//当前相机开启
                            {
                                if ((VisionSystemClassLibrary.Enum.CameraType)(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1]) == Global.Camera[index].Type)//相机类型相同
                                {
                                    //客户端->服务端：指令类型 + 相机类型数据 + 设置结果（1，成功；0，不成功）

                                    lock (Global.LockMachineFaultState)
                                    {
                                        VisionSystemClassLibrary.Class.System.SystemDeviceState = (VisionSystemClassLibrary.Enum.DeviceState)BitConverter.ToInt32(ClientData.ReceivedData, ClientData.DataInfo.InstructionIndex + 2);//解析设备状态

                                        if (VisionSystemClassLibrary.Class.System.SystemDeviceState == VisionSystemClassLibrary.Enum.DeviceState.Run) //系统运行
                                        {
                                            Global.MachineFaultState = (Global.MachineFaultState & (~(UInt64)(0x80000000)));
                                        }
                                        else                                                       //系统暂停
                                        {
                                            Global.MachineFaultState = Global.MachineFaultState | 0x80000000;
                                        }
                                        VisionSystemClassLibrary.Class.System._WriteMachineStateInfoFile();//保存机器信息状态
                                    }

                                    Byte[] Data_TitleBar_State = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1], BitConverter.GetBytes(1));//生成客户端数据
                                    ClientControl._Send(Data_TitleBar_State);//发送数据

                                    break;
                                }
                            }
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.Network_Check:

                        //服务端->客户端：指令类型 + 数据（上位机，1；下位机，2）
                        if (1 == BitConverter.ToInt32(ClientData.ReceivedData, ClientData.DataInfo.InstructionIndex + 1))//上位机发送网络校验命令
                        {
                            //客户端->服务端：指令类型 + 数据（上位机，1；下位机，2）
                            Byte[] Data_Network_Check = new Byte[5];//待发送数据

                            Data_Network_Check[0] = ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex];//填充待发送数据，指令标志位
                            BitConverter.GetBytes(1).CopyTo(Data_Network_Check, 1);//填充待发送数据，数据

                            ClientControl._Send(Data_Network_Check);//发送数据
                        }
                        //
                        break;
                    // 
                    case CommunicationInstructionType.SetFaultMessageState:
                        //DEVICES SETUP页面，点击【Save】按钮，格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 故障信息使能状态

                        for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                        {
                            if ((Global.CameraChooseState & (0x01 << index)) != 0)//当前相机开启
                            {
                                if ((VisionSystemClassLibrary.Enum.CameraType)(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1]) == Global.Camera[index].Type)//相机类型相同
                                {
                                    //客户端->服务端：指令类型 + 相机类型数据 + 设置结果（1，成功；0，不成功）
                                    lock (Global.LockMachineFaultState)
                                    {
                                        UInt64 machineFaultEnableState = BitConverter.ToUInt64(ClientData.ReceivedData, ClientData.DataInfo.InstructionIndex + 2);//解析故障信息使能状态

                                        if (VisionSystemClassLibrary.Class.System.MachineFaultEnableState != machineFaultEnableState) //机器是使能状态发生更改
                                        {
                                            VisionSystemClassLibrary.Class.System.MachineFaultEnableState = machineFaultEnableState;

                                            VisionSystemClassLibrary.Class.System._WriteMachineStateInfoFile();//保存机器信息状态

                                            DiagEnableChanged = true;
                                        }
                                    }
                                    ClientControl._Send(_GenerateInstruction(ClientData, BitConverter.GetBytes(1)));//发送数据

                                    break;
                                }
                            }
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.CurrentFaultMessage:
                        //FAULT MESSAGE，获取当前故障信息，格式：
                        //服务端->客户端：指令类型 + 相机类型数据

                        for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                        {
                            if ((Global.CameraChooseState & (0x01 << index)) != 0)//当前相机开启
                            {
                                if ((VisionSystemClassLibrary.Enum.CameraType)(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1]) == Global.Camera[index].Type)//相机类型相同
                                {
                                    //客户端->服务端：指令类型 + 相机类型数据 + 故障信息（时间 + 故障代码索引值） + 机器速度/相位标志（1，速度；0，相位） + 机器速度/相位数值

                                    Byte[] FaultMessageBytes;

                                    lock (Global.LockMachineFaultState)
                                    {
                                        Global.Camera[index]._GetCurrentFaultMessage(Global.MachineFaultState, VisionSystemClassLibrary.Class.System.MachineFaultEnableState);

                                        Int32 iIndex = 0;
                                        FaultMessageBytes = new Byte[2 * 8 + 4];
                                        _GenerateFaultMessageBytes(ref FaultMessageBytes, ref iIndex, Global.Camera[index].CurrentFaultMessage.DataIndex, Global.Camera[index].CurrentFaultMessage.TimeData);
                                    }

                                    Byte[] Data_CurrentFaultMessage;

                                    Thread.Sleep(Global.ImageDataTime);//循环数据传输休眠10ms

                                    if (Global.Camera[index].UIParameter.SpeedPhase_AsMachine)//当前显示速度
                                    {
                                        Data_CurrentFaultMessage = _GenerateInstruction(ClientData, FaultMessageBytes, BitConverter.GetBytes(Convert.ToInt32(Global.Camera[index].UIParameter.SpeedPhase_AsMachine)), BitConverter.GetBytes(Global.Camera[index].UIParameter.Speed));//生成客户端数据
                                    }
                                    else
                                    {
                                        Data_CurrentFaultMessage = _GenerateInstruction(ClientData, FaultMessageBytes, BitConverter.GetBytes(Convert.ToInt32(Global.Camera[index].UIParameter.SpeedPhase_AsMachine)), BitConverter.GetBytes((Int32)(Global.Camera[index].UIParameter.Speed * 0.2)));//生成客户端数据
                                    }
                                    ClientControl._Send(Data_CurrentFaultMessage);//发送数据

                                    break;
                                }
                            }
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.GetFaultMessages:
                        //FAULT MESSAGE，获取故障信息，格式：
                        //服务端->客户端：指令类型 + 相机类型数据

                        for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                        {
                            if ((Global.CameraChooseState & (0x01 << index)) != 0)//当前相机开启
                            {
                                if ((VisionSystemClassLibrary.Enum.CameraType)(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1]) == Global.Camera[index].Type)//相机类型相同
                                {
                                    //客户端->服务端：指令类型 + 相机类型数据 + 故障信息个数 + 故障信息数组（开始时间 + 故障代码索引值）

                                    Byte[] FaultMessageBytes;

                                    VisionSystemClassLibrary.Struct.FaultMessage[] faultMessage = null;

                                    lock (Global.LockMachineFaultState)
                                    {
                                        faultMessage = Global.Camera[index]._ReadFaultStaticsFile();
                                    }

                                    if (faultMessage != null)//故障信息有效
                                    {
                                        Int32 iIndex = 0;
                                        FaultMessageBytes = new Byte[4 + faultMessage.Length * (2 * 8 + 4)];
                                        BitConverter.GetBytes(faultMessage.Length).CopyTo(FaultMessageBytes, iIndex);

                                        iIndex += 4;
                                        for (Int32 i = 0; i < faultMessage.Length; i++)//遍历当前所有故障
                                        {
                                            _GenerateFaultMessageBytes(ref FaultMessageBytes, ref iIndex, faultMessage[i].DataIndex, faultMessage[i].TimeData);
                                        }
                                    }
                                    else
                                    {
                                        FaultMessageBytes = new Byte[4];
                                        BitConverter.GetBytes(0).CopyTo(FaultMessageBytes, 0);
                                    }

                                    Byte[] Data_GetFaultMessages = _GenerateInstruction(ClientData, FaultMessageBytes);//生成客户端数据
                                    ClientControl._Send(Data_GetFaultMessages);//发送数据

                                    break;
                                }
                            }
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.ClearAllFaultMessages:
                        //FAULT MESSAGE，清除所有故障信息，格式：
                        //服务端->客户端：指令类型 + 相机类型数据

                        for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                        {
                            if ((Global.CameraChooseState & (0x01 << index)) != 0)//当前相机开启
                            {
                                if ((VisionSystemClassLibrary.Enum.CameraType)(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1]) == Global.Camera[index].Type)//相机类型相同
                                {
                                    //客户端->服务端：指令类型 + 相机类型数据 + 清除结果（1，成功；0，不成功）

                                    lock (Global.LockMachineFaultState)
                                    {
                                        Global.Camera[index]._DeleteFaultStaticsFile();//删除故障信息

                                        Global.MachineFaultState = 0;
                                        Global.MachineFaultSaveState = 0;
                                    }

                                    Byte[] Data_ClearAllFaultMessages = _GenerateInstruction(ClientData, BitConverter.GetBytes(1));//生成客户端数据
                                    ClientControl._Send(Data_ClearAllFaultMessages);//发送数据

                                    break;
                                }
                            }
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.Statistics_GetSelectedRecordData:

                        //STATISTICS页面，获取当前选择的统计数据，格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 关联标记 + 统计数据类型（0，最新统计数据；1，指定统计数据） + 班次索引（非0） + 统计数据开始结束时间
        
                        for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                        {
                            if ((Global.CameraChooseState & (0x01 << index)) != 0)//当前相机开启
                            {
                                if ((VisionSystemClassLibrary.Enum.CameraType)(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1]) == Global.Camera[index].Type)//相机类型相同
                                {
                                    //客户端->服务端：指令类型 + 相机类型数据 + 关联标记 + 统计数据类型（0，当前班；1，历史班） + 班次索引（非0） + 统计数据开始结束时间 + 统计数据（品牌名称（包括品牌长度） + 已检测数量 + 合格数量 + 剔除数量 + 工具数量 + 工具统计信息）

                                    Int32 iIndex = ClientData.DataInfo.InstructionIndex + 2;
                                    Boolean bRelevancy = false;
                                    Byte[] bRelevancyBytes = new Byte[4];
                                    if (0 != BitConverter.ToInt32(ClientData.ReceivedData, iIndex))//解析关联标记
                                    {
                                        bRelevancy = true;
                                        bRelevancyBytes = BitConverter.GetBytes(1);
                                    }

                                    iIndex += 4;
                                    Int32 statisticsType = BitConverter.ToInt32(ClientData.ReceivedData, iIndex);//解析统计类型

                                    iIndex += 4;
                                    Int32 shiftIndex = BitConverter.ToInt32(ClientData.ReceivedData, iIndex);//解析班次索引

                                    VisionSystemClassLibrary.Struct.SYSTEMTIME startTime = new VisionSystemClassLibrary.Struct.SYSTEMTIME();
                                    VisionSystemClassLibrary.Struct.SYSTEMTIME endTime = new VisionSystemClassLibrary.Struct.SYSTEMTIME();

                                    iIndex += 4;
                                    _GetSystemTime(ClientData.ReceivedData, ref iIndex, ref startTime, ref endTime);//解析班次起止时间

                                    VisionSystemClassLibrary.Struct.ShiftTime shiftTimeBuf = new VisionSystemClassLibrary.Struct.ShiftTime();
                                    shiftTimeBuf.Start = startTime;
                                    shiftTimeBuf.End = endTime;

                                    VisionSystemClassLibrary.Struct.ShiftTime shiftTime = Global.ShiftInformation._GetCurrentShiftTimeData(Global.ShiftInformation.CurrentShiftIndex);

                                    if ((statisticsType == 0) || (VisionSystemClassLibrary.Class.Shift._Compare(shiftTimeBuf, shiftTime)))//最新班次
                                    {
                                        statisticsType = 0;
                                        shiftIndex = Global.ShiftInformation.CurrentShiftIndex;
                                        startTime = shiftTime.Start;
                                        endTime = shiftTime.End;
                                    }

                                    string brandName = "";
                                    UInt32 inspectedNumber = 0, rejectedNumber = 0;
                                    VisionSystemClassLibrary.Enum.CameraType typeOfCamera = new VisionSystemClassLibrary.Enum.CameraType();
                                    UInt32[] rejectedStatistics_Tool = null;

                                    if (bRelevancy) //关联查询
                                    {
                                        Global.ShiftInformation._UpdateHistoryShift(Global.Camera[index].DeviceInformation.Port, shiftIndex, startTime, endTime, ref typeOfCamera, ref brandName, ref inspectedNumber, ref rejectedNumber, ref rejectedStatistics_Tool, Global.Camera[index].RelevancyCameraInfo.rRelevancyType);
                                    } 
                                    else
                                    {
                                        Global.ShiftInformation._UpdateHistoryShift(Global.Camera[index].DeviceInformation.Port, shiftIndex, startTime, endTime, ref typeOfCamera, ref brandName, ref inspectedNumber, ref rejectedNumber, ref rejectedStatistics_Tool);
                                    }

                                    if (statisticsType == 0)//最新班次
                                    {
                                        brandName = Global.BrandName;
                                    }
                                    Byte[] brandNameBytes = VisionSystemCommunicationLibrary.Ethernet.TestControlFunctions._StringToByte(brandName);//生成烟包品牌名数据

                                    Byte[] rejectedStatistics_ToolBytes = null;
                                    if (rejectedStatistics_Tool != null) //工具数量有效
                                    {
                                        rejectedStatistics_ToolBytes = new Byte[4 * rejectedStatistics_Tool.Length];
                                        for (Int32 i = 0; i < rejectedStatistics_Tool.Length; i++)//遍历当前所有工具
                                        {
                                            iIndex = i * 4;
                                            BitConverter.GetBytes(rejectedStatistics_Tool[i]).CopyTo(rejectedStatistics_ToolBytes, iIndex);//生成当前所有工具数据
                                        }
                                    }

                                    Byte[] shiftTimeDate = _GenerateSystemTimeBytes(startTime, endTime);//生成班次起止时间数组

                                    Byte[] Data_Statistics_GetSelectedRecordData = null;
                                    if (rejectedStatistics_ToolBytes != null) //工具数量有效
                                    {
                                        Data_Statistics_GetSelectedRecordData = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1], bRelevancyBytes, BitConverter.GetBytes(statisticsType), BitConverter.GetBytes(shiftIndex), shiftTimeDate, BitConverter.GetBytes(brandNameBytes.Length), brandNameBytes, BitConverter.GetBytes(inspectedNumber), BitConverter.GetBytes(inspectedNumber - rejectedNumber), BitConverter.GetBytes(rejectedNumber), BitConverter.GetBytes(rejectedStatistics_Tool.Length), rejectedStatistics_ToolBytes); //生成客户端数据
                                    }
                                    else
                                    {
                                        Data_Statistics_GetSelectedRecordData = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1], bRelevancyBytes, BitConverter.GetBytes(statisticsType), BitConverter.GetBytes(shiftIndex), shiftTimeDate, BitConverter.GetBytes(brandNameBytes.Length), brandNameBytes, BitConverter.GetBytes(inspectedNumber), BitConverter.GetBytes(inspectedNumber - rejectedNumber), BitConverter.GetBytes(rejectedNumber), BitConverter.GetBytes(0), rejectedStatistics_ToolBytes); //生成客户端数据
                                    }
                                    ClientControl._Send(Data_Statistics_GetSelectedRecordData);//发送数据

                                    break;
                                }
                            }
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.Statistics_UpdateSelectedRecordData:

                        //STATISTICS页面，获取当前选择的统计数据，格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 关联标记 +  班次索引（非0） + 统计数据开始结束时间 + 剔除图像对应的工具索引值（从0开始） + 剔除图像对应的工具中的索引值（从0开始） + 图像尺寸类型数据
                       
                        for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                        {
                            if ((Global.CameraChooseState & (0x01 << index)) != 0)//当前相机开启
                            {
                                if ((VisionSystemClassLibrary.Enum.CameraType)(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1]) == Global.Camera[index].Type)//相机类型相同
                                {
                                    //客户端->服务端：指令类型 + 相机类型数据 + 关联标记 + 班次索引（非0） + 统计数据开始结束时间 + 剔除图像对应的工具索引值（从0开始） + 剔除图像对应的工具中的索引值（从0开始） + 图像尺寸类型数据 + 图像宽度数据 + 图像高度数据 + 图像信息数据 + 图像数据

                                    Int32 iIndex = ClientData.DataInfo.InstructionIndex + 2;
                                    Boolean bRelevancy = false;
                                    Byte[] bRelevancyBytes = new Byte[4];
                                    if (0 != BitConverter.ToInt32(ClientData.ReceivedData, iIndex))//解析关联标记
                                    {
                                        bRelevancy = true;
                                        bRelevancyBytes = BitConverter.GetBytes(1);
                                    }

                                    iIndex += 4;
                                    Int32 shiftIndex = BitConverter.ToInt32(ClientData.ReceivedData, iIndex);//解析班次索引

                                    VisionSystemClassLibrary.Struct.SYSTEMTIME startTime = new VisionSystemClassLibrary.Struct.SYSTEMTIME();
                                    VisionSystemClassLibrary.Struct.SYSTEMTIME endTime = new VisionSystemClassLibrary.Struct.SYSTEMTIME();

                                    iIndex += 4;
                                    _GetSystemTime(ClientData.ReceivedData, ref iIndex, ref startTime, ref endTime);//解析班次起止时间

                                    Int32 shiftChange = 0;

                                    if (Global.ShiftInformation.ShiftTimeChangeState)//班次状态发生改变
                                    {
                                        Global.ShiftInformation.ShiftTimeChangeState = false;

                                        shiftChange = 1;
                                        shiftIndex = Global.ShiftInformation.CurrentShiftIndex;

                                        VisionSystemClassLibrary.Struct.ShiftTime shiftTime = Global.ShiftInformation._GetCurrentShiftTimeData(Global.ShiftInformation.CurrentShiftIndex);
                                        startTime = shiftTime.Start;
                                        endTime = shiftTime.End;
                                    }

                                    iIndex += 2;
                                    Int32 toolIndex = BitConverter.ToInt32(ClientData.ReceivedData, iIndex);//解析工具索引

                                    iIndex += 4;
                                    Int32 imageIndex = BitConverter.ToInt32(ClientData.ReceivedData, iIndex);//解析图像索引

                                    iIndex += 4;
                                    Double imageScale = BitConverter.ToDouble(ClientData.ReceivedData, iIndex);//解析图像尺寸

                                    string brandName = "";
                                    UInt32 inspectedNumber = 0, rejectedNumber = 0;
                                    VisionSystemClassLibrary.Enum.CameraType typeOfCamera = new VisionSystemClassLibrary.Enum.CameraType();
                                    UInt32[] rejectedStatistics_Tool = null;

                                    if (bRelevancy) //关联查询
                                    {
                                        Global.ShiftInformation._UpdateHistoryShift(Global.Camera[index].DeviceInformation.Port, shiftIndex, startTime, endTime, ref typeOfCamera, ref brandName, ref inspectedNumber, ref rejectedNumber, ref rejectedStatistics_Tool, Global.Camera[index].RelevancyCameraInfo.rRelevancyType);
                                    }
                                    else
                                    {
                                        Global.ShiftInformation._UpdateHistoryShift(Global.Camera[index].DeviceInformation.Port, shiftIndex, startTime, endTime, ref typeOfCamera, ref brandName, ref inspectedNumber, ref rejectedNumber, ref rejectedStatistics_Tool);
                                    }

                                    brandName = Global.BrandName;
                                    Byte[] brandNameBytes = VisionSystemCommunicationLibrary.Ethernet.TestControlFunctions._StringToByte(brandName);//生成烟包品牌名数据

                                    Byte[] rejectedStatistics_ToolBytes = null;
                                    if (rejectedStatistics_Tool != null) //工具数量有效
                                    {
                                        rejectedStatistics_ToolBytes = new Byte[4 * rejectedStatistics_Tool.Length];
                                        for (Int32 i = 0; i < rejectedStatistics_Tool.Length; i++)//遍历当前所有工具
                                        {
                                            iIndex = i * 4;
                                            BitConverter.GetBytes(rejectedStatistics_Tool[i]).CopyTo(rejectedStatistics_ToolBytes, iIndex);//生成当前所有工具数据
                                        }
                                    }

                                    Byte[] shiftTimeDate = _GenerateSystemTimeBytes(startTime, endTime);//生成班次起止时间数组

                                    Byte[] Data_Statistics_UpdateSelectedRecordData = null;

                                    if (rejectedStatistics_ToolBytes != null) //工具数量有效
                                    {
                                        Data_Statistics_UpdateSelectedRecordData = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1], bRelevancyBytes, BitConverter.GetBytes(shiftChange), BitConverter.GetBytes(shiftIndex), shiftTimeDate, BitConverter.GetBytes(brandNameBytes.Length), brandNameBytes, BitConverter.GetBytes(inspectedNumber), BitConverter.GetBytes(inspectedNumber - rejectedNumber), BitConverter.GetBytes(rejectedNumber), BitConverter.GetBytes(rejectedStatistics_Tool.Length), rejectedStatistics_ToolBytes); //生成客户端数据
                                    }
                                    else
                                    {
                                        Data_Statistics_UpdateSelectedRecordData = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1], bRelevancyBytes, BitConverter.GetBytes(shiftChange), BitConverter.GetBytes(shiftIndex), shiftTimeDate, BitConverter.GetBytes(brandNameBytes.Length), brandNameBytes, BitConverter.GetBytes(inspectedNumber), BitConverter.GetBytes(inspectedNumber - rejectedNumber), BitConverter.GetBytes(rejectedNumber), BitConverter.GetBytes(0), rejectedStatistics_ToolBytes); //生成客户端数据
                                    }
                                    ClientControl._Send(Data_Statistics_UpdateSelectedRecordData);//发送数据

                                    break;
                                }
                            }
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.Statistics_ClickRejectsListItem:

                        //查看剔除图像，格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 关联标记 + 班次索引（非0） + 统计数据开始结束时间 + 剔除图像对应的工具索引值（从0开始） + 剔除图像对应的工具中的索引值（从0开始） + 图像尺寸类型数据
                        
                        for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                        {
                            if ((Global.CameraChooseState & (0x01 << index)) != 0)//当前相机开启
                            {
                                if ((VisionSystemClassLibrary.Enum.CameraType)(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1]) == Global.Camera[index].Type)//相机类型相同
                                {
                                    //客户端->服务端：指令类型 + 相机类型数据 + 关联标记 + 班次索引（非0） + 统计数据开始结束时间 + 剔除图像对应的工具索引值（从0开始） + 剔除图像对应的工具中的索引值（从0开始） + 图像尺寸类型数据 + 图像宽度数据 + 图像高度数据 + 图像信息数据 + 图像数据

                                    Int32 iIndex = ClientData.DataInfo.InstructionIndex + 2;
                                    Boolean bRelevancy = false;
                                    Byte[] bRelevancyBytes = new Byte[4];
                                    if (0 != BitConverter.ToInt32(ClientData.ReceivedData, iIndex))//解析关联标记
                                    {
                                        bRelevancy = true;
                                        bRelevancyBytes = BitConverter.GetBytes(1);
                                    }

                                    iIndex += 4;
                                    Int32 shiftIndex = BitConverter.ToInt32(ClientData.ReceivedData, iIndex);//解析班次索引

                                    VisionSystemClassLibrary.Struct.SYSTEMTIME startTime = new VisionSystemClassLibrary.Struct.SYSTEMTIME();
                                    VisionSystemClassLibrary.Struct.SYSTEMTIME endTime = new VisionSystemClassLibrary.Struct.SYSTEMTIME();

                                    iIndex += 4;
                                    _GetSystemTime(ClientData.ReceivedData, ref iIndex, ref startTime, ref endTime);//解析班次起止时间

                                    iIndex += 2;
                                    Int32 toolIndex = BitConverter.ToInt32(ClientData.ReceivedData, iIndex);//解析工具索引

                                    iIndex += 4;
                                    Int32 imageIndex = BitConverter.ToInt32(ClientData.ReceivedData, iIndex);//解析图像索引

                                    iIndex += 4;
                                    Double imageScale = BitConverter.ToDouble(ClientData.ReceivedData, iIndex);//解析图像尺寸

                                    VisionSystemClassLibrary.Struct.ShiftTime shiftTimeBuf = new VisionSystemClassLibrary.Struct.ShiftTime();
                                    shiftTimeBuf.Start = startTime;
                                    shiftTimeBuf.End = endTime;

                                    VisionSystemClassLibrary.Struct.ShiftTime shiftTime = Global.ShiftInformation._GetCurrentShiftTimeData(Global.ShiftInformation.CurrentShiftIndex);

                                    if (VisionSystemClassLibrary.Class.Shift._Compare(shiftTimeBuf, shiftTime))//最新班次
                                    {
                                        shiftIndex = Global.ShiftInformation.CurrentShiftIndex;
                                        startTime = shiftTime.Start;
                                        endTime = shiftTime.End;
                                    }

                                    Image<Bgr, Byte> toolImage = null;//缺陷图像
                                    VisionSystemClassLibrary.Struct.ImageInformation toolImageInformation = new VisionSystemClassLibrary.Struct.ImageInformation();//缺陷图像信息
                                    toolImageInformation._InitData();

                                    Boolean bToolState = false;
                                    Boolean bDeepLearningState = false;
                                    string sTypeName = "OK";

                                    try
                                    {
                                        if (bRelevancy) //关联查询
                                        {
                                            Global.ShiftInformation._UpdateHistoryShiftImage(Global.Camera[index].DeviceInformation.Port, shiftIndex, startTime, endTime, toolIndex, imageIndex, ref toolImage, ref toolImageInformation, ref bToolState, ref bDeepLearningState, ref sTypeName, Global.Camera[index].RelevancyCameraInfo.rRelevancyType);
                                        }
                                        else
                                        {
                                            Global.ShiftInformation._UpdateHistoryShiftImage(Global.Camera[index].DeviceInformation.Port, shiftIndex, startTime, endTime, toolIndex, imageIndex, ref toolImage, ref toolImageInformation, ref bToolState, ref bDeepLearningState, ref sTypeName);
                                        }
                                    }
                                    catch (System.Exception ex)
                                    {

                                    }

                                    Byte[] Data_Statistics_ClickRejectsListItem;

                                    if (toolImage != null)//当前图像有效
                                    {
                                        _DrawImageInformation(ref toolImage, toolImageInformation, bDeepLearningState, sTypeName);
                                        Data_Statistics_ClickRejectsListItem = _GenerateInstruction(ClientData, toolImageInformation, toolImage);//生成客户端数据
                                    }
                                    else
                                    {
                                        Data_Statistics_ClickRejectsListItem = _GenerateInstruction(ClientData, toolImageInformation);//生成客户端数据
                                    }
                                    ClientControl._Send(Data_Statistics_ClickRejectsListItem);//发送数据

                                    break;
                                }
                            }
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.Statistics_GetRecordList:

                        //STATISTICS页面，获取统计数据列表，格式：
                        //服务端->客户端：指令类型 + 相机类型数据

                        for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                        {
                            if ((Global.CameraChooseState & (0x01 << index)) != 0)//当前相机开启
                            {
                                if ((VisionSystemClassLibrary.Enum.CameraType)(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1]) == Global.Camera[index].Type)//相机类型相同
                                {
                                    //客户端->服务端：指令类型 + 相机类型数据 + 统计数据列表（班次个数 + 当前班次索引（从1开始） + 统计数据个数 + 当前统计数据索引（从0开始） + （每个班次）统计数据个数 + （每个班次，每个统计数据）班次开始结束时间 + （每个班次，每个统计数据）品牌（包括品牌长度））

                                    VisionSystemClassLibrary.Struct.StatisticsRecordList statisticsRecordList;

                                    lock (LockStaticRejectImageSavePort[index])
                                    {
                                        statisticsRecordList = Global.ShiftInformation._UpdateStatisticsRecordList(Global.Camera[index].DeviceInformation.Port);
                                    }

                                    Byte[] Data_Statistics_DeleteRecord = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1], statisticsRecordList);//生成客户端数据
                                    ClientControl._Send(Data_Statistics_DeleteRecord);//发送数据

                                    break;
                                }
                            }
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.Statistics_DeleteRecord:

                        //STATISTICS页面，删除统计数据，格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 删除统计数据方式（0，删除所有；1，删除指定班次；2，删除指定记录）
                        //             （0） + 无
                        //             （1） + 待删除的指定班次个数 + 班次索引值数组（从1开始）
                        //             （2） + 班次索引值（从1开始） + 待删除的指定记录个数 + 记录开始结束时间数组

                        for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                        {
                            if ((Global.CameraChooseState & (0x01 << index)) != 0)//当前相机开启
                            {
                                if ((VisionSystemClassLibrary.Enum.CameraType)(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1]) == Global.Camera[index].Type)//相机类型相同
                                {
                                    //客户端->服务端：指令类型 + 相机类型数据 + 删除数据结果（1，成功；0，不成功）

                                    Int32 iIndex = ClientData.DataInfo.InstructionIndex + 2;
                                    Int32 deleteType = BitConverter.ToInt32(ClientData.ReceivedData, iIndex);//解析删除类型
                                    Int32 deleteShiftIndex = 0;

                                    if (1 == DeletingStaticsResult) //默认删除成功
                                    {
                                        DeleteRecord_ThreadParameter deleteRecord_ThreadParameter = new DeleteRecord_ThreadParameter();

                                        if ((deleteType >= 0) && (deleteType < 3)) //剔除类型合规
                                        {
                                            DeletingStaticsResult = 0;

                                            deleteRecord_ThreadParameter.index = index;
                                            deleteRecord_ThreadParameter.Type = deleteType;

                                            switch (deleteType)
                                            {
                                                case 1:
                                                    iIndex += 8;
                                                    deleteShiftIndex = BitConverter.ToInt32(ClientData.ReceivedData, iIndex);//解析删除班次开始索引

                                                    deleteRecord_ThreadParameter.shiftIndex = deleteShiftIndex;
                                                    break;
                                                case 2:
                                                    iIndex += 4;
                                                    deleteShiftIndex = BitConverter.ToInt32(ClientData.ReceivedData, iIndex);//解析删除班次开始索引

                                                    iIndex += 8;
                                                    VisionSystemClassLibrary.Struct.SYSTEMTIME startTime = new VisionSystemClassLibrary.Struct.SYSTEMTIME();
                                                    VisionSystemClassLibrary.Struct.SYSTEMTIME endTime = new VisionSystemClassLibrary.Struct.SYSTEMTIME();

                                                    _GetSystemTime(ClientData.ReceivedData, ref iIndex, ref startTime, ref endTime);//解析班次起止时间

                                                    deleteRecord_ThreadParameter.shiftIndex = deleteShiftIndex;
                                                    deleteRecord_ThreadParameter.start = startTime;
                                                    deleteRecord_ThreadParameter.end = endTime;
                                                    break;
                                            }

                                            if ((threadStatistics_DeleteRecord == null) || (false == threadStatistics_DeleteRecord.IsAlive))//班次统计线程无效，或是线程未执行，重新启动
                                            {
                                                threadStatistics_DeleteRecord = new Thread(_threadStatistics_DeleteRecord);
                                                threadStatistics_DeleteRecord.Priority = System.Threading.ThreadPriority.BelowNormal;
                                                threadStatistics_DeleteRecord.IsBackground = true;
                                                threadStatistics_DeleteRecord.Start((object)deleteRecord_ThreadParameter);
                                            }
                                        }
                                    }

                                    Thread.Sleep(Global.ImageDataTime * 3);//循环数据传输休眠30ms

                                    Byte[] Data_Statistics_DeleteRecord = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1], BitConverter.GetBytes(DeletingStaticsResult));//生成客户端数据
                                    ClientControl._Send(Data_Statistics_DeleteRecord);//发送数据

                                    break;
                                }
                            }
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.TestMode_Light:

                        //测试程序，光源：
                        //服务端->客户端：指令类型 + 相机类型数据 + 电流（UInt32，mA）

                        //客户端->服务端：指令类型 + 相机类型数据

                        Int32 stroboCurrent = BitConverter.ToInt32(ClientData.ReceivedData, ClientData.DataInfo.InstructionIndex + 2);

                        lock (LockControllerSerialPort)
                        {
                            Byte[] Command = new Byte[Global.ControllerSerialPortCommucation_SendBytesThreshold];

                            if (stroboCurrent == 0)//关闭光源
                            {
                                Command[0] = 0x0D;
                                Command[1] = 0x01;
                                Command[2] = 0x00;
                                Command[3] = (Byte)(Command[0] ^ Command[1] ^ Command[2]);

                                if (ControllerSerialPortCommucation.IsOpen == true)                            //当前串口1打开，向下位发送命令
                                {
                                    ControllerSerialPortCommucation.Write(Command, 0, 4);
                                }
                            }
                            else
                            {
                                Command[0] = 0x05;
                                Command[1] = 0x04;
                                Command[2] = 0x01;
                                Command[3] = Convert.ToByte(stroboCurrent & (0xFF));
                                Command[4] = Convert.ToByte(stroboCurrent >> 8);
                                Command[5] = 2;
                                Command[6] = (Byte)(Command[0] ^ Command[1] ^ Command[2] ^ Command[3] ^ Command[4] ^ Command[5]);
                                if (ControllerSerialPortCommucation.IsOpen == true)                            //当前串口1打开，向下位发送命令
                                {
                                    ControllerSerialPortCommucation.Write(Command, 0, 7);
                                }
                                Thread.Sleep(100);

                                Command[0] = 0x05;
                                Command[1] = 0x04;
                                Command[2] = 0x81;
                                Command[3] = Convert.ToByte(stroboCurrent & (0xFF));
                                Command[4] = Convert.ToByte(stroboCurrent >> 8);
                                Command[5] = 2;
                                Command[6] = (Byte)(Command[0] ^ Command[1] ^ Command[2] ^ Command[3] ^ Command[4] ^ Command[5]);
                                if (ControllerSerialPortCommucation.IsOpen == true)                            //当前串口1打开，向下位发送命令
                                {
                                    ControllerSerialPortCommucation.Write(Command, 0, 7);
                                }
                                Thread.Sleep(100);

                                Command[0] = 0x0D;
                                Command[1] = 0x01;
                                Command[2] = 0x03;
                                Command[3] = (Byte)(Command[0] ^ Command[1] ^ Command[2]);

                                if (ControllerSerialPortCommucation.IsOpen == true)                            //当前串口1打开，向下位发送命令
                                {
                                    ControllerSerialPortCommucation.Write(Command, 0, 4);
                                }
                            }
                        }

                        Byte[] Data_TestMode_Light = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1]);//生成客户端数据
                        ClientControl._Send(Data_TestMode_Light);//发送数据
                        //
                        break;
                    //
                    case CommunicationInstructionType.TestMode_CameraPort:

                        //测试程序，相机端口：
                        //服务端->客户端：指令类型 + 相机类型数据；

                        //客户端->服务端：指令类型 + 相机类型数据；

                        USBPortTestResult = 0;

                        _CameraPowerOff(15);//相机全部下电
                        Thread.Sleep(300);

                        string[] serialPortNames = SerialPort.GetPortNames();

                        _CameraPowerOn(15);//当前相机端口上电
                        Thread.Sleep(2000);

                        _USBPortRecognise(serialPortNames);//执行USB测试

                        Byte[] Data_TestMode_CameraPort = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1], BitConverter.GetBytes(USBPortTestResult));//生成客户端数据
                        ClientControl._Send(Data_TestMode_CameraPort);//发送数据
                        //
                        break;
                    //
                    case CommunicationInstructionType.TestMode_CameraTrigger:

                        //测试程序，相机触发：
                        //服务端->客户端：指令类型 + 相机类型数据 + 开启/关闭（UInt32，1/0）；

                        //客户端->服务端：指令类型 + 相机类型数据

                        UInt32 cameraTrigger = BitConverter.ToUInt32(ClientData.ReceivedData, ClientData.DataInfo.InstructionIndex + 2);

                        lock (LockControllerSerialPort)
                        {
                            Byte[] Command = new Byte[Global.ControllerSerialPortCommucation_SendBytesThreshold];

                            Command[0] = 0x06;
                            Command[1] = 1;

                            if (cameraTrigger == 1)//给相机0、1提供触发信号
                            {
                                Command[2] = 3;
                            }
                            else
                            {
                                Command[2] = 0;
                            }
                            Command[3] = (Byte)(Command[0] ^ Command[1] ^ Command[2]);
                            if (ControllerSerialPortCommucation.IsOpen == true)                            //当前串口1打开，向下位发送命令
                            {
                                ControllerSerialPortCommucation.Write(Command, 0, 4);
                            }
                        }

                        Byte[] Data_TestMode_CameraTrigger = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1]);//生成客户端数据
                        ClientControl._Send(Data_TestMode_CameraTrigger);//发送数据
                        //
                        break;
                    //
                    default:
                        break;
                }
            }
            else//NetDataType.File == ClientData.DataInfo.DataType，接收的数据为文件
            {
                switch ((CommunicationInstructionType)ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex])
                {
                    case CommunicationInstructionType.DevicesSetup_ConfigDevice:

                        //未完成文件发送
                        //服务端->客户端（文件）：指令类型 + 相机类型数据 + 设置为的IP地址 + 设置为的相机类型数据 + 设置为的端口 + 设置为的模式 + 设置为的相机故障标记 + 设置为的相机检测使能 + 设置为的相机旋转角度 + 相机颜色 + 相机分辨率 + 是否为串口 + 烟支排列类型 + 设置为的相机数据截取区域缩放 + 设置为的相机数据截取区域缩放后是否居中 + 设置为的相机数据截取区域粘贴区域 + 设置为的相机数据截取区域 + 镜像标记 + 传感器应用场景 + 相机关联信息（关联类型 + 关联数量（相机类型 + 工位索引）） + 文件索引值（从0开始） + 文件

                        Int32 IPAdress = -1;

                        Int32 iIndex_DevicesSetup_ConfigDevice = 0;//临时变量
                        Int32 iValueLength = BitConverter.GetBytes((Int32)1).Length;//数据长度

                        iIndex_DevicesSetup_ConfigDevice = ClientData.DataInfo.InstructionIndex + 2;
                        IPAdress = BitConverter.ToInt32(ClientData.ReceivedData, iIndex_DevicesSetup_ConfigDevice);//解析设置为的IP地址
                        iIndex_DevicesSetup_ConfigDevice += iValueLength;
                        VisionSystemClassLibrary.Enum.CameraType CameraType = (VisionSystemClassLibrary.Enum.CameraType)BitConverter.ToInt32(ClientData.ReceivedData, iIndex_DevicesSetup_ConfigDevice);//解析设置为的相机类型数据
                        iIndex_DevicesSetup_ConfigDevice += iValueLength;
                        Int32 iCameraPort = BitConverter.ToInt32(ClientData.ReceivedData, iIndex_DevicesSetup_ConfigDevice);//设置为的相机端口

                        _StopCamera((Byte)iCameraPort);

                        Int32 iValue_DevicesSetup_ConfigDevice = 0;//文件接收结果（1，成功；0，失败）

                        if (ClientData.DataInfo.InstructionLength != 0) //命令数据有效
                        {
                            if (ClientData.DataInfo.FileDataLength != 0) //解析文件传送状态
                            {
                                iValue_DevicesSetup_ConfigDevice = 1;//文件接收结果（1，成功；0，失败）

                                //

                                _saveFileData(ClientData, Global.Camera[iCameraPort].ReceivedDataPath); ;//保存文件数据
                            }
                            else
                            {
                                Byte[] byteFileName = new Byte[ClientData.DataInfo.FileNameLength];//文件名称（包含扩展名）
                                System.Array.Copy(ClientData.ReceivedData, ClientData.DataInfo.FileNameIndex, byteFileName, 0, ClientData.DataInfo.FileNameLength);
                                String sFileName = VisionSystemCommunicationLibrary.Ethernet.TestControlFunctions._ByteToString(byteFileName, 0, byteFileName.Length);//文件名称（包含扩展名）

                                File.Delete(Global.Camera[iCameraPort].ReceivedDataPath + sFileName);
                                File.Delete(Global.Camera[iCameraPort].SampleImagePath + sFileName);
                            }
                        }

                        //

                        //客户端->服务端（数据）：指令类型 + 相机类型数据 + 设置为的IP地址 + 设置为的相机类型数据 + 设置为的端口 + 文件索引值（从0开始） + 文件传输状态（1，文件发送中；2，文件发送完成） + 文件接收结果（1，成功；0，失败）

                        Byte[] byteValue_1_DevicesSetup_ConfigDevice = new Byte[ClientData.DataInfo.InstructionLength];
                        System.Array.Copy(ClientData.ReceivedData, ClientData.DataInfo.InstructionIndex, byteValue_1_DevicesSetup_ConfigDevice, 0, ClientData.DataInfo.InstructionLength);

                        Byte[] byteValue_2_DevicesSetup_ConfigDevice = BitConverter.GetBytes((Int32)1);//文件传输状态（1，文件发送中；2，文件发送完成）
                        Byte[] byteValue_3_DevicesSetup_ConfigDevice = BitConverter.GetBytes(iValue_DevicesSetup_ConfigDevice);//文件接收结果（1，成功；0，失败）

                        //

                        Byte[] Data_DevicesSetupConfigDevice = _GenerateInstruction(byteValue_1_DevicesSetup_ConfigDevice, byteValue_2_DevicesSetup_ConfigDevice, byteValue_3_DevicesSetup_ConfigDevice);//生成客户端数据
                        ClientControl._Send(Data_DevicesSetupConfigDevice);//发送数据

                        //
                        break;
                    // 
                    case CommunicationInstructionType.BrandManagement_LoadBrand:

                        //未完成文件发送
                        //服务端->客户端（文件）：指令类型 + 相机类型数据 + 设置相机模式 + 品牌名称长度 + 品牌名称 + 文件索引值（从0开始） + 文件

                        for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                        {
                            if ((VisionSystemClassLibrary.Enum.CameraType)(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1]) == Global.Camera[index].Type)//相机类型相同
                            {
                                _StopCamera(index);

                                Int32 iIndex = ClientData.DataInfo.InstructionIndex + 6;
                                Int32 iBrandNameLength = BitConverter.ToInt32(ClientData.ReceivedData, iIndex);//解析烟包品牌名称长度

                                if (iBrandNameLength > 0)//文件名长度有效
                                {
                                    iIndex += 4;
                                    Global.BrandName = VisionSystemCommunicationLibrary.Ethernet.TestControlFunctions._ByteToString(ClientData.ReceivedData, iIndex, iBrandNameLength);//烟包品牌名称
                                }

                                Int32 iValue = 0;//文件接收结果（1，成功；0，失败）

                                if (ClientData.DataInfo.InstructionLength != 0) //命令数据有效
                                {
                                    if (ClientData.DataInfo.FileDataLength != 0) //解析文件传送状态
                                    {
                                        iValue = 1;//文件接收结果（1，成功；0，失败）

                                        _saveFileData(ClientData, Global.Camera[index].ReceivedDataPath);
                                    }
                                    else
                                    {
                                        Byte[] byteFileName = new Byte[ClientData.DataInfo.FileNameLength];//文件名称（包含扩展名）
                                        System.Array.Copy(ClientData.ReceivedData, ClientData.DataInfo.FileNameIndex, byteFileName, 0, ClientData.DataInfo.FileNameLength);
                                        String sFileName = VisionSystemCommunicationLibrary.Ethernet.TestControlFunctions._ByteToString(byteFileName, 0, byteFileName.Length);//文件名称（包含扩展名）

                                        File.Delete(Global.Camera[index].ReceivedDataPath + sFileName);
                                        File.Delete(Global.Camera[index].SampleImagePath + sFileName);
                                    }
                                }

                                //

                                //客户端->服务端（数据）：指令类型 + 相机类型数据 + 设置相机模式 + 品牌名称长度 + 品牌名称 + 文件索引值（从0开始） + 文件传输状态（1，文件发送中；2，文件发送完成） + 文件接收结果（1，成功；0，失败）

                                Byte[] byteValue_1 = new Byte[ClientData.DataInfo.InstructionLength];//指令类型 + 相机类型数据 + 品牌名称长度 + 品牌名称 + 文件索引值（从0开始）
                                System.Array.Copy(ClientData.ReceivedData, ClientData.DataInfo.InstructionIndex, byteValue_1, 0, ClientData.DataInfo.InstructionLength);

                                Byte[] byteValue_2 = BitConverter.GetBytes((Int32)1);//文件传输状态（1，文件发送中；2，文件发送完成）
                                Byte[] byteValue_3 = BitConverter.GetBytes(iValue);//文件接收结果（1，成功；0，失败）

                                //

                                Byte[] Data_BrandManagement_LoadBrand = _GenerateInstruction(byteValue_1, byteValue_2, byteValue_3);//生成客户端数据
                                ClientControl._Send(Data_BrandManagement_LoadBrand);//发送数据

                                break;
                            }
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.ClientSystem_Update:

                        //客户端系统升级，格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 设置相机模式 + 文件

                        Int32 iCameraChooseState_ClientSystem_Update = 0;

                        for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                        {
                            if ((VisionSystemClassLibrary.Enum.CameraType)(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1]) == Global.Camera[index].Type)//相机类型相同
                            {
                                //客户端->服务端：指令类型 + 相机类型数据 + 传输结果（1，成功；0，失败）

                                Int32 iIndex = ClientData.DataInfo.InstructionIndex + 2;
                                iCameraChooseState_ClientSystem_Update = BitConverter.ToInt32(ClientData.ReceivedData, iIndex);//相机模式数据

                                Int32 iValue = 0;//文件接收结果（1，成功；0，失败）

                                if (ClientData.DataInfo.InstructionLength != 0) //命令数据有效
                                {
                                    if (ClientData.DataInfo.FileDataLength != 0) //解析文件传送状态
                                    {
                                        iValue = 1;//文件接收结果（1，成功；0，失败）

                                        _saveFileData(ClientData, Global.Camera[index].ConfigDataPath);//保存文件数据
                                    }
                                }

                                //

                                Byte[] Data_DeviceStop = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1], BitConverter.GetBytes(iValue));//生成客户端数据
                                ClientControl._Send(Data_DeviceStop);//发送数据

                                CommunicationCount_ClientSystem_Update |= (Byte)(0x01 << index);

                                break;
                            }
                        }

                        if (iCameraChooseState_ClientSystem_Update == CommunicationCount_ClientSystem_Update) //数据发送完毕
                        {
                            iCameraChooseState_ClientSystem_Update = 0;
                            CommunicationCount_ClientSystem_Update = 0;

                            CloseSerialPortFlag = true;
                            CloseSerialPort_ComType = CommunicationInstructionType.ClientSystem_Update;

                            if (false == timer3.Enabled) //开启定时器3，执行重启
                            {
                                this.Invoke(new MethodInvoker(delegate { timer3.Enabled = true; }));
                            }
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.SystemParameter:

                        //系统参数设置（相机选择、班次），格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 设置相机模式 + 相机选择（1，启用；0，禁用） + 相机检测使能（1，启用；0，禁用） + 烟支排列类型 + 机器类型 + 班次数据（文件）

                        Int32 iCameraChooseState_SystemParameter = 0;

                        for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                        {
                            if ((VisionSystemClassLibrary.Enum.CameraType)(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1]) == Global.Camera[index].Type)//相机类型相同
                            {
                                //客户端->服务端：指令类型 + 相机类型数据 + 设置结果（1，成功；0，不成功）

                                Int32 iIndex = ClientData.DataInfo.InstructionIndex + 2;
                                iCameraChooseState_SystemParameter = BitConverter.ToInt32(ClientData.ReceivedData, iIndex);//相机模式数据

                                iIndex += 4;
                                Boolean bCameraChooseState = (BitConverter.ToInt32(ClientData.ReceivedData, iIndex) != 0) ? true : false;//解析相机模式

                                iIndex += 4;
                                Boolean bCheckEnable = (BitConverter.ToInt32(ClientData.ReceivedData, iIndex) != 0) ? true : false;//解析相机检测使能

                                iIndex += 4;
                                VisionSystemClassLibrary.Enum.TobaccoSortType TobaccoSortType_E = (VisionSystemClassLibrary.Enum.TobaccoSortType)BitConverter.ToInt32(ClientData.ReceivedData, iIndex);//解析烟支排列类型

                                if (Global.Camera[index].TobaccoSortType_E != TobaccoSortType_E) //烟支排列类型变化
                                {
                                    ApplicationResart_CommunicationCount_SystemParameter = true;
                                }

                                if ((Global.Camera[index].CheckEnable != bCheckEnable) || (Global.Camera[index].TobaccoSortType_E != TobaccoSortType_E)) //相机使能标记变化
                                {
                                    Global.Camera[index].CheckEnable = bCheckEnable;
                                    Global.Camera[index].TobaccoSortType_E = TobaccoSortType_E;

                                    Global.Camera[index]._Save_CameraType();
                                }

                                iIndex += 4;
                                Int32 machineType = BitConverter.ToInt32(ClientData.ReceivedData, iIndex) + 1;//解析机器类型

                                if ((MachineType != machineType) && (Global.Camera[index].DeviceParameter.Parameter != null)) //当前执行相位命令发送))
                                {
                                    MachineType = Convert.ToByte(machineType);

                                    if (false == Global.ComputerRunState) //控制器上运行软件
                                    {
                                        lock (LockControllerSerialPort)
                                        {
                                            _SendCommand(16, index);
                                        }
                                    }
                                }

                                Int32 iValue = 0;//文件接收结果（1，成功；0，失败）

                                if (ClientData.DataInfo.InstructionLength != 0) //命令数据有效
                                {
                                    if (ClientData.DataInfo.FileDataLength != 0) //解析文件传送状态
                                    {
                                        iValue = 1;//文件接收结果（1，成功；0，失败）
                                    }
                                }

                                //

                                Byte[] Data_DeviceStop = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1], BitConverter.GetBytes(iValue));//生成客户端数据
                                ClientControl._Send(Data_DeviceStop);//发送数据

                                CommunicationCount_SystemParameter |= (Byte)(0x01 << index);

                                break;
                            }
                        }

                        if (iCameraChooseState_SystemParameter == CommunicationCount_SystemParameter) //数据发送完毕
                        {
                            if (ClientData.DataInfo.InstructionLength != 0) //命令数据有效
                            {
                                if (ClientData.DataInfo.FileDataLength != 0) //解析文件传送状态
                                {
                                    lock (LockShiftFile)
                                    {
                                        for (Byte i = 0; i < Global.CameraNumberMax; i++) //遍历循环所有相机
                                        {
                                            if ((iCameraChooseState_SystemParameter & (0x01 << i)) != 0) //当前相机开启
                                            {
                                                _saveFileData(ClientData, Global.Camera[i].ReceivedDataPath);

                                                if (false == ApplicationResart_CommunicationCount_SystemParameter)//应用软件不需要重启
                                                {
                                                    lock (LockStaticRejectImageSavePort[i])
                                                    {
                                                        if (!VisionSystemClassLibrary.Class.Shift._Compare(Global.ShiftInformation._ReadShiftTimeNew(Global.Camera[i].ReceivedDataPath), Global.ShiftInformation._GetCurrentShiftTimeArray()))
                                                        {
                                                            File.Copy(Global.Camera[i].ReceivedDataPath + VisionSystemClassLibrary.Class.Shift.ShiftFileName, VisionSystemClassLibrary.Class.Shift.ShiftDataPath + VisionSystemClassLibrary.Class.Shift.ShiftFileName, true);
                                                            //VisionSystemClassLibrary.Class.System.FileCopyFun(Global.Camera[i].ReceivedDataPath + VisionSystemClassLibrary.Class.Shift.ShiftFileName, VisionSystemClassLibrary.Class.Shift.ShiftDataPath + VisionSystemClassLibrary.Class.Shift.ShiftFileName);
                                                            _reInitShift();
                                                        }
                                                    }
                                                }
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                            iCameraChooseState_SystemParameter = 0;
                            CommunicationCount_SystemParameter = 0;

                            if (true == ApplicationResart_CommunicationCount_SystemParameter) //应用程序重启
                            {
                                CloseSerialPortFlag = true;
                                CloseSerialPort_ComType = CommunicationInstructionType.SystemParameter;

                                if (false == timer3.Enabled) //开启定时器3，执行重启
                                {
                                    this.Invoke(new MethodInvoker(delegate { timer3.Enabled = true; }));
                                }
                            }
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.DeviceState_Synchronization:

                        //系统状态同步,格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 设置相机模式 + 当前选择机器 + 设备状态（VisionSystemClassLibrary.Enum.DeviceState） + 故障信息使能状态 + 品牌长度 + 品牌名称 + 班次数据（文件）

                        Int32 iCameraChooseState_DeviceState_Synchronization = 0;

                        for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                        {
                            if ((VisionSystemClassLibrary.Enum.CameraType)(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1]) == Global.Camera[index].Type)//相机类型相同
                            {
                                //客户端->服务端：指令类型 + 相机类型数据 + 设置结果（1，成功；0，不成功）

                                Int32 iIndex = ClientData.DataInfo.InstructionIndex + 2;
                                iCameraChooseState_DeviceState_Synchronization = BitConverter.ToInt32(ClientData.ReceivedData, iIndex);//相机模式数据

                                iIndex += 4;
                                Int32 machineType = BitConverter.ToInt32(ClientData.ReceivedData, iIndex) + 1;//解析机器类型

                                if ((MachineType != machineType) && (Global.Camera[index].DeviceParameter.Parameter != null)) //当前执行相位命令发送))
                                {
                                    MachineType = Convert.ToByte(machineType);

                                    if (false == Global.ComputerRunState) //控制器上运行软件
                                    {
                                        lock (LockControllerSerialPort)
                                        {
                                            _SendCommand(16, index);
                                        }
                                    }
                                }

                                iIndex += 4;

                                lock (Global.LockMachineFaultState)
                                {
                                    VisionSystemClassLibrary.Class.System.SystemDeviceState = (VisionSystemClassLibrary.Enum.DeviceState)BitConverter.ToInt32(ClientData.ReceivedData, iIndex);//解析设备状态

                                    iIndex += 4;

                                    UInt64 machineFaultEnableState = BitConverter.ToUInt64(ClientData.ReceivedData, iIndex);//解析故障信息使能状态

                                    if (VisionSystemClassLibrary.Class.System.MachineFaultEnableState != machineFaultEnableState) //机器是使能状态发生更改
                                    {
                                        VisionSystemClassLibrary.Class.System.MachineFaultEnableState = machineFaultEnableState;

                                        VisionSystemClassLibrary.Class.System._WriteMachineStateInfoFile();//保存机器信息状态

                                        DiagEnableChanged = true;
                                    }

                                    if (VisionSystemClassLibrary.Class.System.SystemDeviceState == VisionSystemClassLibrary.Enum.DeviceState.Run) //系统运行
                                    {
                                        Global.MachineFaultState = (Global.MachineFaultState & (~(UInt64)(0x80000000)));
                                    }
                                    else                                                       //系统暂停
                                    {
                                        Global.MachineFaultState = Global.MachineFaultState | 0x80000000;
                                    }
                                }

                                iIndex += 8;
                                Int32 iBrandNameLength = BitConverter.ToInt32(ClientData.ReceivedData, iIndex);//解析烟包品牌名称长度

                                if (iBrandNameLength > 0)//文件名长度有效
                                {
                                    iIndex += 4;
                                    Global.BrandName = VisionSystemCommunicationLibrary.Ethernet.TestControlFunctions._ByteToString(ClientData.ReceivedData, iIndex, iBrandNameLength);//烟包品牌名称
                                    label27.Invoke(new EventHandler(delegate { label27.Text = Global.BrandName; }));
                                }

                                Int32 iValue = 0;//文件接收结果（1，成功；0，失败）

                                if (ClientData.DataInfo.InstructionLength != 0) //命令数据有效
                                {
                                    if (ClientData.DataInfo.FileDataLength != 0) //解析文件传送状态
                                    {
                                        iValue = 1;//文件接收结果（1，成功；0，失败）
                                    }
                                }

                                //

                                Byte[] Data_DeviceState_Synchronization = _GenerateInstruction(ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex], ClientData.ReceivedData[ClientData.DataInfo.InstructionIndex + 1], BitConverter.GetBytes(iValue));//生成客户端数据
                                ClientControl._Send(Data_DeviceState_Synchronization);//发送数据

                                CommunicationCount_DeviceState_Synchronization |= (Byte)(0x01 << index);

                                break;
                            }
                        }

                        if (0 <= CommunicationCount_DeviceState_Synchronization) //数据发送完毕
                        {
                            if (ClientData.DataInfo.InstructionLength != 0) //命令数据有效
                            {
                                if (ClientData.DataInfo.FileDataLength != 0) //解析文件传送状态
                                {
                                    lock (LockShiftFile)
                                    {
                                        for (Byte i = 0; i < Global.CameraNumberMax; i++) //遍历循环所有相机
                                        {
                                            if ((iCameraChooseState_DeviceState_Synchronization & (0x01 << i)) != 0) //当前相机开启
                                            {
                                                _saveFileData(ClientData, Global.Camera[i].ReceivedDataPath);

                                                lock (LockStaticRejectImageSavePort[i])
                                                {
                                                    if (!VisionSystemClassLibrary.Class.Shift._Compare(Global.ShiftInformation._ReadShiftTimeNew(Global.Camera[i].ReceivedDataPath), Global.ShiftInformation._GetCurrentShiftTimeArray()))
                                                    {
                                                        File.Copy(Global.Camera[i].ReceivedDataPath + VisionSystemClassLibrary.Class.Shift.ShiftFileName, VisionSystemClassLibrary.Class.Shift.ShiftDataPath + VisionSystemClassLibrary.Class.Shift.ShiftFileName, true);
                                                        //VisionSystemClassLibrary.Class.System.FileCopyFun(Global.Camera[i].ReceivedDataPath + VisionSystemClassLibrary.Class.Shift.ShiftFileName, VisionSystemClassLibrary.Class.Shift.ShiftDataPath + VisionSystemClassLibrary.Class.Shift.ShiftFileName);
                                                        _reInitShift();
                                                    }
                                                }
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                            iCameraChooseState_DeviceState_Synchronization = 0;
                            CommunicationCount_DeviceState_Synchronization = 0;
                        }
                        //
                        break;
                    //
                    default:
                        break;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：接收和发送数据时产生的异常事件
        // 输入参数：1.sender：ControlNetClient控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _NetClient_ExceptionHandled(object sender, EventArgs e)
        {
            VisionSystemCommunicationLibrary.Ethernet.ClientControl ClientDataControl = (VisionSystemCommunicationLibrary.Ethernet.ClientControl)sender;//ClientControl控件（实际使用中可忽略该变量值）

            VisionSystemCommunicationLibrary.Ethernet.ExceptionHandledEventArgs ClientDataException = (VisionSystemCommunicationLibrary.Ethernet.ExceptionHandledEventArgs)e;//事件参数（重要）

            //

            Boolean bNetworkError = false;//网络接收数据超时，客户端断开连接

            if (2 == ClientDataException.Timeout)//超时，无效
            {
                bNetworkError = true;
            }
            else//超时，发送验证数据
            {
                //客户端->服务端：指令类型 + 数据

                try
                {
                    Byte[] arrayValue_1 = BitConverter.GetBytes(2);//数值

                    Byte[] Data_Network_Check = new Byte[1 + arrayValue_1.Length];//待发送数据

                    Data_Network_Check[0] = (Byte)CommunicationInstructionType.Network_Check;//填充待发送数据，指令标志位
                    arrayValue_1.CopyTo(Data_Network_Check, 1);//填充待发送数据，数据

                    //

                    ClientDataControl._Send(Data_Network_Check);//发送数据
                }
                catch (System.Exception ex)
                {

                }
            }

            //

            if (bNetworkError)//网络接收数据超时，服务器端强制退出
            {
                LabModel = false;

                Thread ConnectedThread = new Thread(_NetClient_Connect);
                ConnectedThread.IsBackground = true;
                ConnectedThread.Start();

                if (Global.ShowInformation)//显示调试信息
                {
                    label3.Invoke(new EventHandler(delegate { label3.Text = "断开"; }));
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：解析命令得到班次起止时间信息
        // 输入参数：1、Byte[]：receivedData，命令内容
        //         2、Int32 index，数据解析索引
        //         3、VisionSystemClassLibrary.Struct.SYSTEMTIME：startTime，开始时间
        //         4、VisionSystemClassLibrary.Struct.SYSTEMTIME：endTime，结束时间
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _GetSystemTime(Byte[] receivedData, ref Int32 index, ref VisionSystemClassLibrary.Struct.SYSTEMTIME startTime, ref VisionSystemClassLibrary.Struct.SYSTEMTIME endTime)
        {
            startTime.Year = BitConverter.ToUInt16(receivedData, index);//解析班次开始时间年

            index += 2;
            startTime.Month = BitConverter.ToUInt16(receivedData, index);//解析班次开始时间月

            index += 2;
            startTime.DayOfWeek = BitConverter.ToUInt16(receivedData, index);//解析班次开始时间周

            index += 2;
            startTime.Day = BitConverter.ToUInt16(receivedData, index);//解析班次开始时间日

            index += 2;
            startTime.Hour = BitConverter.ToUInt16(receivedData, index);//解析班次开始时间小时

            index += 2;
            startTime.Minute = BitConverter.ToUInt16(receivedData, index);//解析班次开始时间分钟

            index += 2;
            startTime.Second = BitConverter.ToUInt16(receivedData, index);//解析班次开始时间秒钟

            index += 2;
            startTime.MilliSeconds = BitConverter.ToUInt16(receivedData, index);//解析班次开始时间毫秒

            index += 2;
            endTime.Year = BitConverter.ToUInt16(receivedData, index);//解析班次结束时间年

            index += 2;
            endTime.Month = BitConverter.ToUInt16(receivedData, index);//解析班次结束时间月

            index += 2;
            endTime.DayOfWeek = BitConverter.ToUInt16(receivedData, index);//解析班次结束时间周

            index += 2;
            endTime.Day = BitConverter.ToUInt16(receivedData, index);//解析班次结束时间日

            index += 2;
            endTime.Hour = BitConverter.ToUInt16(receivedData, index);//解析班次结束时间小时

            index += 2;
            endTime.Minute = BitConverter.ToUInt16(receivedData, index);//解析班次结束时分钟

            index += 2;
            endTime.Second = BitConverter.ToUInt16(receivedData, index);//解析班次结束时间秒钟

            index += 2;
            endTime.MilliSeconds = BitConverter.ToUInt16(receivedData, index);//解析班次结束时间毫秒
        }

        //----------------------------------------------------------------------
        // 功能说明：生成故障信息数组
        // 输入参数：1、Byte[]：shiftTimeDate，命令内容
        //         2、Int32：index，数据生成索引
        //         3、Int32：dataIndex，故障索引
        //         4、VisionSystemClassLibrary.Struct.SYSTEMTIME：time，时间
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _GenerateFaultMessageBytes(ref Byte[] shiftTimeDate, ref Int32 index, Int32 dataIndex, VisionSystemClassLibrary.Struct.SYSTEMTIME time)
        {
            BitConverter.GetBytes(time.Year).CopyTo(shiftTimeDate, index);

            index += 2;
            BitConverter.GetBytes(time.Month).CopyTo(shiftTimeDate, index);

            index += 2;
            BitConverter.GetBytes(time.DayOfWeek).CopyTo(shiftTimeDate, index);

            index += 2;
            BitConverter.GetBytes(time.Day).CopyTo(shiftTimeDate, index);

            index += 2;
            BitConverter.GetBytes(time.Hour).CopyTo(shiftTimeDate, index);

            index += 2;
            BitConverter.GetBytes(time.Minute).CopyTo(shiftTimeDate, index);

            index += 2;
            BitConverter.GetBytes(time.Second).CopyTo(shiftTimeDate, index);//生成故障时间数据

            index += 2;
            BitConverter.GetBytes(time.MilliSeconds).CopyTo(shiftTimeDate, index);//生成故障时间数据

            index += 2;

            BitConverter.GetBytes(dataIndex).CopyTo(shiftTimeDate, index);//生成故障索引数据

            index += 4;
        }

        //----------------------------------------------------------------------
        // 功能说明：生成班次起止时间信息
        // 输入参数：1、VisionSystemClassLibrary.Struct.SYSTEMTIME：startTime，开始时间
        //           2、VisionSystemClassLibrary.Struct.SYSTEMTIME：endTime，结束时间
        // 输出参数：无
        // 返回值：Byte[]，班次起止时间字节数组
        //----------------------------------------------------------------------
        private Byte[] _GenerateSystemTimeBytes(VisionSystemClassLibrary.Struct.SYSTEMTIME startTime, VisionSystemClassLibrary.Struct.SYSTEMTIME endTime)
        {
            Int32 index = 0;
            Byte[] shiftTimeDate = new Byte[32];

            BitConverter.GetBytes(startTime.Year).CopyTo(shiftTimeDate, index);

            index += 2;
            BitConverter.GetBytes(startTime.Month).CopyTo(shiftTimeDate, index);

            index += 2;
            BitConverter.GetBytes(startTime.DayOfWeek).CopyTo(shiftTimeDate, index);

            index += 2;
            BitConverter.GetBytes(startTime.Day).CopyTo(shiftTimeDate, index);

            index += 2;
            BitConverter.GetBytes(startTime.Hour).CopyTo(shiftTimeDate, index);

            index += 2;
            BitConverter.GetBytes(startTime.Minute).CopyTo(shiftTimeDate, index);

            index += 2;
            BitConverter.GetBytes(startTime.Second).CopyTo(shiftTimeDate, index);

            index += 2;
            BitConverter.GetBytes(startTime.MilliSeconds).CopyTo(shiftTimeDate, index);//生成班次开始时间数据

            index += 2;
            BitConverter.GetBytes(endTime.Year).CopyTo(shiftTimeDate, index);

            index += 2;
            BitConverter.GetBytes(endTime.Month).CopyTo(shiftTimeDate, index);

            index += 2;
            BitConverter.GetBytes(endTime.DayOfWeek).CopyTo(shiftTimeDate, index);

            index += 2;
            BitConverter.GetBytes(endTime.Day).CopyTo(shiftTimeDate, index);

            index += 2;
            BitConverter.GetBytes(endTime.Hour).CopyTo(shiftTimeDate, index);

            index += 2;
            BitConverter.GetBytes(endTime.Minute).CopyTo(shiftTimeDate, index);

            index += 2;
            BitConverter.GetBytes(endTime.Second).CopyTo(shiftTimeDate, index);

            index += 2;
            BitConverter.GetBytes(endTime.MilliSeconds).CopyTo(shiftTimeDate, index);//生成班次结束时间数据

            return shiftTimeDate;
        }

        //----------------------------------------------------------------------
        // 功能说明：发送设备信息指令数据
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SendDeviceInformation()
        {
            VisionSystemCommunicationLibrary.Ethernet.SerializableData ethernetData = new VisionSystemCommunicationLibrary.Ethernet.SerializableData();

            ethernetData.Data_0 = VisionSystemCommunicationLibrary.Ethernet.TestControlFunctions._StringToByte(Global.ClientData.MACAddress);//MAC地址
            ethernetData.Data_1 = VisionSystemCommunicationLibrary.Ethernet.TestControlFunctions._StringToByte(Global.ClientData.FirmwareVersion);//固件版本

            Int32 i = 0;

            ethernetData.Data_2 = new Byte[Global.CameraDevice.CameraNumber];
            ethernetData.Data_00 = new Byte[Global.CameraDevice.CameraNumber][];
            ethernetData.Data_01 = new Byte[Global.CameraDevice.CameraNumber][];
            ethernetData.Data_02 = new Byte[Global.CameraDevice.CameraNumber][];

            for (Byte j = 0; j < Global.CameraNumberMax; j++)//初始化相机类
            {
                if ((Global.CameraChooseState & (0x01 << j)) != 0)//当前相机开启
                {
                    UInt64 temp = 0;

                    switch (j)
                    {
                        case 0:
                            temp = 0x02;
                            break;
                        case 1:
                            temp = 0x04;
                            break;
                        case 2:
                            temp = 0x010000000000;
                            break;
                        case 3:
                            temp = 0x020000000000;
                            break;
                        default:
                            break;
                    }

                    if ((Global.MachineFaultStateTemp & temp) == 0)//相机初始未查询到，故障信息保存，不再查询相机
                    {
                        ethernetData.Data_2[i] = Global.Camera[j].DeviceInformation.Port;//端口号

                        ethernetData.Data_00[i] = new Byte[1];
                        ethernetData.Data_00[i][0] = (Byte)(Global.CameraDevice.CameraType[j]);//写入相机类型数据

                        ethernetData.Data_01[i] = VisionSystemCommunicationLibrary.Ethernet.TestControlFunctions._StringToByte(Global.CameraDevice.SerialNumber[j]);//序列号
                        ethernetData.Data_02[i] = VisionSystemCommunicationLibrary.Ethernet.TestControlFunctions._StringToByte(Global.CameraDevice.DeviceName[j]);//设备名称

                        i++;
                    }
                }
            }

            IFormatter formatter = new SoapFormatter();//格式化对象
            MemoryStream memoryStream = new MemoryStream();//流对象
            formatter.Serialize(memoryStream, ethernetData);

            byte[] byteSignNumber = VisionSystemCommunicationLibrary.Ethernet.TestControlFunctions._StringToByte(VisionSystemCommunicationLibrary.Ethernet.TestControlFunctions.sDeviceInformationFlag);//设备信息指令标志
            Int32 byteSignNumberLength = byteSignNumber.Length;//设备信息指令标志数据长度

            Byte[] memoryStreamBytes = memoryStream.ToArray();//待发送的指令数据长度
            Int32 memoryStreamLength = memoryStreamBytes.Length;

            byte[] byteData = new byte[byteSignNumber.Length + 4 + memoryStreamLength];//待发送的指令数据  

            byteSignNumber.CopyTo(byteData, 0);
            BitConverter.GetBytes(memoryStreamLength).CopyTo(byteData, byteSignNumberLength);//写入待发送的指令数据长度数据的长度信息
            memoryStreamBytes.CopyTo(byteData, byteSignNumberLength + 4);//写入待发送的指令数据

            Global.NetClient._Send(byteData);//发送指令
        }

        //----------------------------------------------------------------------
        // 功能说明：生成指令
        // 输入参数：1.byteValue_1：数值
        //         2.byteValue_2：数值
        //         3.byteValue_3：数值
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private Byte[] _GenerateInstruction(Byte[] byteValue_1, Byte[] byteValue_2, Byte[] byteValue_3)
        {
            Byte[] InstructionData = new Byte[byteValue_1.Length + byteValue_2.Length + byteValue_3.Length];//待发送数据

            //

            Int32 iIndex = 0;//循环控制变量

            byteValue_1.CopyTo(InstructionData, iIndex);//填充待发送数据
            iIndex += byteValue_1.Length;
            byteValue_2.CopyTo(InstructionData, iIndex);//填充待发送数据
            iIndex += byteValue_2.Length;
            byteValue_3.CopyTo(InstructionData, iIndex);//填充待发送数据

            //

            return InstructionData;
        }

        //----------------------------------------------------------------------
        // 功能说明：生成指令
        // 输入参数：1.byteValue_1：数值
        //         2.byteValue_2：数值
        //         3.byteValue_3：数值
        //         4.byteValue_4：数值
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private Byte[] _GenerateInstruction(Byte[] byteValue_1, Byte[] byteValue_2, Byte[] byteValue_3, Byte[] byteValue_4)
        {
            Byte[] InstructionData = new Byte[byteValue_1.Length + byteValue_2.Length + byteValue_3.Length + byteValue_4.Length];//待发送数据

            //

            Int32 iIndex = 0;//循环控制变量

            byteValue_1.CopyTo(InstructionData, iIndex);//填充待发送数据
            iIndex += byteValue_1.Length;
            byteValue_2.CopyTo(InstructionData, iIndex);//填充待发送数据
            iIndex += byteValue_2.Length;
            byteValue_3.CopyTo(InstructionData, iIndex);//填充待发送数据
            iIndex += byteValue_3.Length;
            byteValue_4.CopyTo(InstructionData, iIndex);//填充待发送数据

            //

            return InstructionData;
        }

        //----------------------------------------------------------------------
        // 功能说明：生成指令
        // 输入参数：1.commandType：指令类型
        //         2.Cameratype：相机类型
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private Byte[] _GenerateInstruction(Byte commandType, Byte cameraType)
        {
            //指令类型 + 相机类型数据 + 工具索引数值
            Byte[] InstructionData = new Byte[2];//待发送数据

            InstructionData[0] = commandType;//填充待发送数据，指令标志位
            InstructionData[1] = (Byte)cameraType;//填充待发送数据，相机类型数据

            //
            return InstructionData;
        }

        //----------------------------------------------------------------------
        // 功能说明：生成指令
        // 输入参数：1.commandType：指令类型
        //         2.Cameratype：相机类型
        //         3.iValue_1：数值
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private Byte[] _GenerateInstruction(Byte commandType, Byte cameraType, Byte[] iValue_1)
        {
            //指令类型 + 相机类型数据 + 工具索引数值
            Byte[] InstructionData = new Byte[2 + iValue_1.Length];//待发送数据

            InstructionData[0] = commandType;//填充待发送数据，指令标志位
            InstructionData[1] = (Byte)cameraType;//填充待发送数据，相机类型数据
            iValue_1.CopyTo(InstructionData, 2);//填充待发送数据，工具索引数值

            //
            return InstructionData;
        }

        //----------------------------------------------------------------------
        // 功能说明：生成指令
        // 输入参数：1.commandType：指令类型
        //         2.Cameratype：相机类型
        //         3.iValue_1：数值
        //         4.iValue_2：数值
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private Byte[] _GenerateInstruction(Byte commandType, Byte cameraType, Byte[] iValue_1, Byte[] iValue_2)
        {
            //指令类型 + 相机类型数据 + 工具索引数值 + 工具开关数值

            Int32 iIndex = 0;//临时变量
            Byte[] InstructionData = new Byte[1 + 1 + iValue_1.Length + iValue_2.Length];//待发送数据

            InstructionData[0] = commandType;//填充待发送数据，指令标志位
            iIndex += 1;
            InstructionData[1] = (Byte)cameraType;//填充待发送数据，相机类型数据
            iIndex += 1;
            iValue_1.CopyTo(InstructionData, iIndex);//填充待发送数据，工具索引数值
            iIndex += iValue_1.Length;
            iValue_2.CopyTo(InstructionData, iIndex);//填充待发送数据，工具开关数值

            //
            return InstructionData;
        }

        //----------------------------------------------------------------------
        // 功能说明：生成指令
        // 输入参数：1.commandType：指令类型
        //         2.Cameratype：相机类型
        //         3.iValue_1：数值
        //         4.iValue_2：数值
        //         5.iValue_3：数值
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private Byte[] _GenerateInstruction(Byte commandType, Byte cameraType, Byte[] iValue_1, Byte[] iValue_2, Byte[] iValue_3)
        {
            //指令类型 + 相机类型数据 + 工具索引数值 + 工具开关数值 + 操作结果

            Int32 iIndex = 0;//临时变量
            Byte[] InstructionData = new Byte[2 + iValue_1.Length + iValue_2.Length + iValue_3.Length];//待发送数据

            InstructionData[0] = commandType;//填充待发送数据，指令标志位
            iIndex += 1;
            InstructionData[1] = (Byte)cameraType;//填充待发送数据，相机类型数据
            iIndex += 1;
            iValue_1.CopyTo(InstructionData, iIndex);//填充待发送数据，工具索引数值
            iIndex += iValue_1.Length;
            iValue_2.CopyTo(InstructionData, iIndex);//填充待发送数据，工具开关数值
            iIndex += iValue_2.Length;
            iValue_3.CopyTo(InstructionData, iIndex);//填充待发送数据，工具开关数值
            //
            return InstructionData;
        }

        //----------------------------------------------------------------------
        // 功能说明：生成指令
        // 输入参数：1.commandType：指令类型
        //         2.Cameratype：相机类型
        //         3.iValue_1：数值
        //         4.iValue_2：数值
        //         5.iValue_3：数值
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private Byte[] _GenerateInstruction(Byte commandType, Byte cameraType, Byte[] iValue_1, Byte[] iValue_2, Int16[] iValue_3)
        {
            //指令类型 + 相机类型数据 + 工具索引数值 + 工具开关数值 + 操作结果

            Int32 iIndex = 0;//临时变量
            Byte[] InstructionData = new Byte[2 + iValue_1.Length + iValue_2.Length + iValue_3.Length * 2];//待发送数据

            InstructionData[0] = commandType;//填充待发送数据，指令标志位
            iIndex += 1;
            InstructionData[1] = (Byte)cameraType;//填充待发送数据，相机类型数据
            iIndex += 1;
            iValue_1.CopyTo(InstructionData, iIndex);//填充待发送数据，工具索引数值
            iIndex += iValue_1.Length;
            iValue_2.CopyTo(InstructionData, iIndex);//填充待发送数据，工具开关数值
            iIndex += iValue_2.Length;

            for (Int32 i = 0; i < iValue_3.Length; i++) //循环所有烟支
            {
                BitConverter.GetBytes(iValue_3[i]).CopyTo(InstructionData, iIndex);//填充待发送数据，工具开关数值

                iIndex += 2;
            }

            //
            return InstructionData;
        }

        //----------------------------------------------------------------------
        // 功能说明：生成指令
        // 输入参数：1.commandType：指令类型
        //         2.Cameratype：相机类型
        //         3.iValue_1：数值
        //         4.iValue_2：数值
        //         5.iValue_3：数值
        //         6.iValue_4：数值
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private Byte[] _GenerateInstruction(Byte commandType, Byte cameraType, Byte[] iValue_1, Byte[] iValue_2, Byte[] iValue_3, Byte[] iValue_4)
        {
            //指令类型 + 相机类型数据 + 工具索引数值 + 工具开关数值 + 操作结果

            Int32 iIndex = 0;//临时变量
            Byte[] InstructionData = new Byte[2 + iValue_1.Length + iValue_2.Length + iValue_3.Length + iValue_4.Length];//待发送数据

            InstructionData[0] = commandType;//填充待发送数据，指令标志位
            iIndex += 1;
            InstructionData[1] = (Byte)cameraType;//填充待发送数据，相机类型数据
            iIndex += 1;
            iValue_1.CopyTo(InstructionData, iIndex);//填充待发送数据，工具索引数值
            iIndex += iValue_1.Length;
            iValue_2.CopyTo(InstructionData, iIndex);//填充待发送数据，工具开关数值
            iIndex += iValue_2.Length;
            iValue_3.CopyTo(InstructionData, iIndex);//填充待发送数据，工具开关数值
            iIndex += iValue_3.Length;
            iValue_4.CopyTo(InstructionData, iIndex);//填充待发送数据，工具开关数值
            //
            return InstructionData;
        }

        //----------------------------------------------------------------------
        // 功能说明：生成指令
        // 输入参数：1.commandType：指令类型
        //         2.Cameratype：相机类型
        //         3.iValue_0：数值
        //         4.iValue_1：数值
        //         5.iValue_2：数值
        //         6.iValue_3：数值
        //         7.iValue_7：数值
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private Byte[] _GenerateInstruction(Byte commandType, Byte cameraType, Byte[] iValue_0, Byte[] iValue_1, Byte[] iValue_2, Byte[] iValue_3, Byte[] iValue_7)
        {
            //客户端->服务端：指令类型 + 相机类型数据 + 关联标记 + 统计数据类型（0，最新统计数据；1，指定统计数据） + 班次索引（非0） + 统计数据开始结束时间  +  剔除数量

            Int32 iIndex = 0;//临时变量
            Byte[] InstructionData = new Byte[2 + iValue_0.Length + iValue_1.Length + iValue_2.Length + iValue_3.Length + iValue_7.Length];//待发送数据

            InstructionData[0] = commandType;//填充待发送数据，指令标志位
            iIndex += 1;
            InstructionData[1] = (Byte)cameraType;//填充待发送数据，相机类型数据
            iIndex += 1;
            iValue_0.CopyTo(InstructionData, iIndex);//关联标记
            iIndex += iValue_0.Length;
            iValue_1.CopyTo(InstructionData, iIndex);//统计数据类型（0，当前班；1，历史班）
            iIndex += iValue_1.Length;
            iValue_2.CopyTo(InstructionData, iIndex);//班次索引（从0开始）
            iIndex += iValue_2.Length;
            iValue_3.CopyTo(InstructionData, iIndex);//统计数据开始结束时间
            iIndex += iValue_3.Length;
            iValue_7.CopyTo(InstructionData, iIndex);//剔除数量

            return InstructionData;
        }

        //----------------------------------------------------------------------
        // 功能说明：生成指令
        // 输入参数：1.commandType：指令类型
        //         2.Cameratype：相机类型
        //         3.iValue_0：数值
        //         4.iValue_1：数值
        //         5.iValue_2：数值
        //         6.iValue_3：数值
        //         7.iValue_7：数值
        //         8.iValue_8：数值
        //         9.iValue_9：数值
        //         10.iValue_10：数值
        //         11.iValue_11：数值
        //         12.iValue_12：数值
        //         13.iValue_13：数值
        //         14.iValue_14：数值
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private Byte[] _GenerateInstruction(Byte commandType, Byte cameraType, Byte[] iValue_0, Byte[] iValue_1, Byte[] iValue_2, Byte[] iValue_3, Byte[] iValue_7, Byte[] iValue_8, Byte[] iValue_9, Byte[] iValue_10, Byte[] iValue_11, Byte[] iValue_12, Byte[] iValue_13 = null, Byte[] iValue_14 = null)
        {
            //客户端->服务端：指令类型 + 相机类型数据 + 关联标记 + 统计数据类型（0，当前班；1，历史班） + 班次索引（非0） + 统计数据开始结束时间 + 统计数据（品牌名称（包括品牌长度） + 已检测数量 + 合格数量 + 剔除数量 + 工具数量 + 工具统计信息）

            Int32 iIndex = 0;//临时变量
            Byte[] InstructionData;

            Int32 InstructionDataLength = 2 + iValue_0.Length + iValue_1.Length + iValue_2.Length + iValue_3.Length + iValue_7.Length + iValue_8.Length + iValue_9.Length + iValue_10.Length + iValue_11.Length + iValue_12.Length;//待发送数据

            if (iValue_13 != null) //工具数量不为空
            {
                InstructionDataLength += iValue_13.Length;
            }

            if (iValue_14 != null)//缺陷图像信息
            {
                InstructionDataLength += iValue_14.Length;
            }

            InstructionData = new Byte[InstructionDataLength];

            InstructionData[0] = commandType;//填充待发送数据，指令标志位
            iIndex += 1;
            InstructionData[1] = (Byte)cameraType;//填充待发送数据，相机类型数据
            iIndex += 1;
            iValue_0.CopyTo(InstructionData, iIndex);//关联标记
            iIndex += iValue_0.Length;
            iValue_1.CopyTo(InstructionData, iIndex);//统计数据类型（0，当前班；1，历史班）
            iIndex += iValue_1.Length;
            iValue_2.CopyTo(InstructionData, iIndex);//班次索引（从0开始）
            iIndex += iValue_2.Length;
            iValue_3.CopyTo(InstructionData, iIndex);//统计数据开始结束时间
            iIndex += iValue_3.Length;
            iValue_7.CopyTo(InstructionData, iIndex);//品牌名称长度
            iIndex += iValue_7.Length;
            iValue_8.CopyTo(InstructionData, iIndex);//品牌名称
            iIndex += iValue_8.Length;
            iValue_9.CopyTo(InstructionData, iIndex);//已检测数量
            iIndex += iValue_9.Length;
            iValue_10.CopyTo(InstructionData, iIndex);//合格数量
            iIndex += iValue_10.Length;
            iValue_11.CopyTo(InstructionData, iIndex);//剔除数量
            iIndex += iValue_11.Length;
            iValue_12.CopyTo(InstructionData, iIndex);//工具数量
            iIndex += iValue_12.Length;

            if (iValue_13 != null)
            {
                iValue_13.CopyTo(InstructionData, iIndex);//工具统计信息
                iIndex += iValue_13.Length;
            }

            if (iValue_14 != null)
            {
                iValue_14.CopyTo(InstructionData, iIndex);//缺陷图像信息
            }
            return InstructionData;
        }

        //----------------------------------------------------------------------
        // 功能说明：生成指令
        // 输入参数：1.clientData：服务器端接收数据
        //         2.iValue_1：数值
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private Byte[] _GenerateInstruction(VisionSystemCommunicationLibrary.Ethernet.DataReceivedEventArgs clientData, Byte[] iValue_1)
        {
            //指令类型 + 相机类型数据 + 工具索引数值
            Byte[] InstructionData = new Byte[clientData.DataInfo.InstructionLength + iValue_1.Length];//待发送数据

            System.Array.Copy(clientData.ReceivedData, clientData.DataInfo.InstructionIndex, InstructionData, 0, clientData.DataInfo.InstructionLength);//写入服务器端数据
            iValue_1.CopyTo(InstructionData, clientData.DataInfo.InstructionLength);//填充待发送数据，工具索引数值

            //
            return InstructionData;
        }

        //----------------------------------------------------------------------
        // 功能说明：生成指令
        // 输入参数：1.clientData：服务器端接收数据
        //         2.iValue_1：数值
        //         3.iValue_2：数值
        //         4.iValue_3：数值
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private Byte[] _GenerateInstruction(VisionSystemCommunicationLibrary.Ethernet.DataReceivedEventArgs clientData, Byte[] iValue_1, Byte[] iValue_2, Byte[] iValue_3)
        {
            //指令类型 + 相机类型数据 + 工具索引数值 + 工具开关数值 + 操作结果

            Int32 iIndex = 0;//临时变量
            Byte[] InstructionData = new Byte[clientData.DataInfo.InstructionLength + iValue_1.Length + iValue_2.Length + iValue_3.Length];//待发送数据

            System.Array.Copy(clientData.ReceivedData, clientData.DataInfo.InstructionIndex, InstructionData, iIndex, clientData.DataInfo.InstructionLength);//写入服务器端数据
            iIndex += clientData.DataInfo.InstructionLength;
            iValue_1.CopyTo(InstructionData, iIndex);//填充待发送数据，工具索引数值
            iIndex += iValue_1.Length;
            iValue_2.CopyTo(InstructionData, iIndex);//填充待发送数据，工具开关数值
            iIndex += iValue_2.Length;
            iValue_3.CopyTo(InstructionData, iIndex);//填充待发送数据，操作结果

            //
            return InstructionData;
        }

        //----------------------------------------------------------------------
        // 功能说明：生成图像信息
        // 输入参数：1.VisionSystemClassLibrary.Struct.ImageInformation：imageInformation，绘图信息
        // 输出参数：无
        // 返回值：Byte[]，生成图像信息
        //----------------------------------------------------------------------
        private Byte[] _GenerateImageInformation(VisionSystemClassLibrary.Struct.ImageInformation imageInformation)
        {
            IFormatter formatterImageInformation = new SoapFormatter();//格式化对象
            MemoryStream memorystreamformatterImageInformation = new MemoryStream();//流对象
            formatterImageInformation.Serialize(memorystreamformatterImageInformation, imageInformation);//序列化图像信息

            Byte[] memorystreamformatterImageInformationBytes = memorystreamformatterImageInformation.ToArray();//图像信息数据
            Int32 formatterDrawingInfoLength = memorystreamformatterImageInformationBytes.Length;

            Byte[] InstructionData = new Byte[BitConverter.GetBytes(formatterDrawingInfoLength).Length + formatterDrawingInfoLength];

            Int32 index = 0;
            BitConverter.GetBytes(formatterDrawingInfoLength).CopyTo(InstructionData, index);

            index = index + BitConverter.GetBytes(formatterDrawingInfoLength).Length;
            memorystreamformatterImageInformationBytes.CopyTo(InstructionData, index);

            return InstructionData;
        }

        //----------------------------------------------------------------------
        // 功能说明：生成图像数据指令
        // 输入参数：1.Byte：commandType，命令类型
        //         2.VisionSystemCommunicationLibrary.Ethernet.SerializableData：ethernetData，服务端发送的数据
        //         3.Int32：imageWidth，图像宽度
        //         4.Int32：imageHeight，图像高度
        //         5.Byte[]：liveImageBytes，图像数据
        //         6.VisionSystemClassLibrary.Struct.ImageInformation：graphicsInformation，图像信息
        // 输出参数：无
        // 返回值：Byte[]，生成的指令
        //----------------------------------------------------------------------
        private Byte[] _GenerateInstruction(Byte commandType, ref VisionSystemCommunicationLibrary.Ethernet.SerializableData ethernetData, Int32 imageWidth, Int32 imageHeight, VisionSystemClassLibrary.Struct.ImageInformation graphicsInformation, Byte[] liveImageBytes = null)
        {
            Byte[] InstructionData = null;//待发送的数据

            //指令类型 + 相机类型数据 + 图像工具数据（1，包含工具；0，不包含工具）  + 图像尺寸类型数据  + 图像宽度数据 + 图像高度数据 + 图像信息数据 + 剔除图像索引值数据 + 剔除图像个数数据 + 剔除图像名称索引值数据 + 图像数据

            MemoryStream liveMemoryStream = new MemoryStream();//流对象
            IFormatter liveFormatter = new SoapFormatter();//格式化对象

            if (null != liveImageBytes)//图像有效
            {
                ethernetData.Data_3 = BitConverter.GetBytes(imageWidth);//图像宽度
                ethernetData.Data_4 = BitConverter.GetBytes(imageHeight);//图像高度
            }
            else//图像无效
            {
                ethernetData.Data_3 = BitConverter.GetBytes(0);//图像宽度
                ethernetData.Data_4 = BitConverter.GetBytes(0);//图像高度
            }

            liveMemoryStream.Position = 0;
            liveFormatter.Serialize(liveMemoryStream, ethernetData);//序列化客户端返回数据

            Byte[] liveMemoryStreamBytes = liveMemoryStream.ToArray();
            Int32 liveMemoryStreamLength = liveMemoryStreamBytes.Length;

            IFormatter formatterGraphicsInformation = new SoapFormatter();//格式化对象
            MemoryStream memorystreamGraphicsInformation = new MemoryStream();//流对象
            formatterGraphicsInformation.Serialize(memorystreamGraphicsInformation, graphicsInformation);//序列化图像信息

            Byte[] memorystreamGraphicsInformationBytes = memorystreamGraphicsInformation.ToArray();//图像信息数据
            Int32 formatterGraphicsInformationLength = memorystreamGraphicsInformationBytes.Length;

            if (null != liveImageBytes)//图像有效
            {
                InstructionData = new byte[1 + BitConverter.GetBytes(liveMemoryStreamLength).Length + liveMemoryStreamLength + BitConverter.GetBytes(formatterGraphicsInformationLength).Length + formatterGraphicsInformationLength + BitConverter.GetBytes(liveImageBytes.Length).Length + liveImageBytes.Length];//待发送的指令数据  
            }
            else
            {
                InstructionData = new byte[1 + BitConverter.GetBytes(liveMemoryStreamLength).Length + liveMemoryStreamLength + BitConverter.GetBytes(formatterGraphicsInformationLength).Length + formatterGraphicsInformationLength + +BitConverter.GetBytes(0).Length];//待发送的指令数据  
            }

            Int32 index = 0;
            InstructionData[0] = commandType;

            index = index + 1;
            BitConverter.GetBytes(liveMemoryStreamLength).CopyTo(InstructionData, index);

            index = index + BitConverter.GetBytes(liveMemoryStreamLength).Length;
            liveMemoryStreamBytes.CopyTo(InstructionData, index);

            index = index + liveMemoryStreamLength;
            BitConverter.GetBytes(formatterGraphicsInformationLength).CopyTo(InstructionData, index);

            index = index + BitConverter.GetBytes(formatterGraphicsInformationLength).Length;
            memorystreamGraphicsInformationBytes.CopyTo(InstructionData, index);

            index = index + formatterGraphicsInformationLength;

            if (null != liveImageBytes)//图像有效
            {
                BitConverter.GetBytes(liveImageBytes.Length).CopyTo(InstructionData, index);

                index = index + BitConverter.GetBytes(liveImageBytes.Length).Length;
                liveImageBytes.CopyTo(InstructionData, index);
            }
            else
            {
                BitConverter.GetBytes(0).CopyTo(InstructionData, index);
            }
            return InstructionData;
        }

        //----------------------------------------------------------------------
        // 功能说明：生成图像数据指令
        // 输入参数：1.Byte：commandType，命令类型
        //         2.VisionSystemCommunicationLibrary.Ethernet.SerializableData：ethernetData，服务端发送的数据
        //         3.Image<Bgr, Byte>：image，图像数据
        //         4.VisionSystemClassLibrary.Struct.ImageInformation：graphicsInformation，图像信息
        // 输出参数：无
        // 返回值：Byte[]，生成的指令
        //----------------------------------------------------------------------
        private Byte[] _GenerateInstruction(Byte commandType, ref VisionSystemCommunicationLibrary.Ethernet.SerializableData ethernetData, VisionSystemClassLibrary.Struct.ImageInformation graphicsInformation, Image<Bgr, Byte> image = null)
        {
            Byte[] InstructionData = null;//待发送的数据

            //指令类型 + 相机类型数据 + 图像工具数据（1，包含工具；0，不包含工具） +  图像尺寸类型数据 + 图像宽度数据 +  图像高度数据 + 图像信息数据 + 图像处理信息（绘图） + 图像数据

            MemoryStream liveMemoryStream = new MemoryStream();//流对象
            IFormatter liveFormatter = new SoapFormatter();//格式化对象

            if (null != image)//图像有效
            {
                ethernetData.Data_3 = BitConverter.GetBytes(image.Width);//图像宽度
                ethernetData.Data_4 = BitConverter.GetBytes(image.Height);//图像高度
            }
            else//图像无效
            {
                ethernetData.Data_3 = BitConverter.GetBytes(0);//图像宽度
                ethernetData.Data_4 = BitConverter.GetBytes(0);//图像高度
            }
            liveMemoryStream.Position = 0;
            liveFormatter.Serialize(liveMemoryStream, ethernetData);//序列化客户端返回数据

            Byte[] liveMemoryStreamBytes = liveMemoryStream.ToArray();
            Int32 liveMemoryStreamLength = liveMemoryStreamBytes.Length;

            IFormatter formatterGraphicsInformation = new SoapFormatter();//格式化对象
            MemoryStream memorystreamGraphicsInformation = new MemoryStream();//流对象
            formatterGraphicsInformation.Serialize(memorystreamGraphicsInformation, graphicsInformation);//序列化图像信息

            Byte[] memorystreamGraphicsInformationBytes = memorystreamGraphicsInformation.ToArray();//图像信息数据
            Int32 formatterGraphicsInformationLength = memorystreamGraphicsInformationBytes.Length;

            if (null != image)//图像有效
            {
                InstructionData = new byte[1 + BitConverter.GetBytes(liveMemoryStreamLength).Length + liveMemoryStreamLength + BitConverter.GetBytes(formatterGraphicsInformationLength).Length + formatterGraphicsInformationLength + BitConverter.GetBytes(image.Bytes.Length).Length + image.Bytes.Length];//待发送的指令数据  
            }
            else
            {
                InstructionData = new byte[1 + BitConverter.GetBytes(liveMemoryStreamLength).Length + liveMemoryStreamLength + BitConverter.GetBytes(formatterGraphicsInformationLength).Length + formatterGraphicsInformationLength + +BitConverter.GetBytes(0).Length];//待发送的指令数据  
            }

            Int32 index = 0;
            InstructionData[0] = commandType;

            index = index + 1;
            BitConverter.GetBytes(liveMemoryStreamLength).CopyTo(InstructionData, index);

            index = index + BitConverter.GetBytes(liveMemoryStreamLength).Length;
            liveMemoryStreamBytes.CopyTo(InstructionData, index);

            index = index + liveMemoryStreamLength;
            BitConverter.GetBytes(formatterGraphicsInformationLength).CopyTo(InstructionData, index);

            index = index + BitConverter.GetBytes(formatterGraphicsInformationLength).Length;
            memorystreamGraphicsInformationBytes.CopyTo(InstructionData, index);

            index = index + formatterGraphicsInformationLength;

            if (null != image)//图像有效
            {
                BitConverter.GetBytes(image.Bytes.Length).CopyTo(InstructionData, index);

                index = index + BitConverter.GetBytes(image.Bytes.Length).Length;
                image.Bytes.CopyTo(InstructionData, index);
            }
            else
            {
                BitConverter.GetBytes(0).CopyTo(InstructionData, index);
            }
            return InstructionData;
        }
        
        //----------------------------------------------------------------------
        // 功能说明：生成图像数据指令
        // 输入参数：1.VisionSystemCommunicationLibrary.Ethernet.DataReceivedEventArgs：clientData，服务端发送的数据
        //         2.VisionSystemClassLibrary.Struct.ImageInformation：graphicsInformation，图像信息
        //         3.Image<Bgr, Byte>：image，图像数据
        // 输出参数：无
        // 返回值：Byte[]，生成的指令
        //----------------------------------------------------------------------
        private Byte[] _GenerateInstruction(VisionSystemCommunicationLibrary.Ethernet.DataReceivedEventArgs clientData, VisionSystemClassLibrary.Struct.ImageInformation graphicsInformation, Image<Bgr, Byte> image = null)
        {
            Int32 index = 0;
            Byte[] InstructionData = null;//待发送的数据

            //指令类型 + 相机类型数据 + 关联标记 + 班次索引（非0） + 统计数据开始结束时间 + 剔除图像对应的工具索引值（从0开始） + 剔除图像对应的工具中的索引值（从0开始） + 图像尺寸类型数据 + 图像宽度数据 + 图像高度数据 + 图像信息数据 + 图像数据

            Byte[] imageWidthBytes;
            Byte[] imageHeightBytes;
            Byte[] imageLengthBytes;
            Byte[] imageInformationBytes = _GenerateImageInformation(graphicsInformation);//图像信息

            if (null != image)//图像有效
            {
                imageWidthBytes = BitConverter.GetBytes(image.Width);//图像宽度
                imageHeightBytes = BitConverter.GetBytes(image.Height);//图像高度
                imageLengthBytes = BitConverter.GetBytes(image.Bytes.Length);

                InstructionData = new Byte[clientData.DataInfo.InstructionLength + imageWidthBytes.Length + imageHeightBytes.Length + imageInformationBytes.Length + imageLengthBytes.Length + image.Bytes.Length];
            }
            else//图像无效
            {
                imageWidthBytes = BitConverter.GetBytes(0);//图像宽度
                imageHeightBytes = BitConverter.GetBytes(0);//图像高度
                imageLengthBytes = BitConverter.GetBytes(0);

                InstructionData = new Byte[clientData.DataInfo.InstructionLength + imageWidthBytes.Length + imageHeightBytes.Length + imageInformationBytes.Length + imageLengthBytes.Length];
            }

            Array.Copy(clientData.ReceivedData, clientData.DataInfo.InstructionIndex, InstructionData, index, clientData.DataInfo.InstructionLength); 
            index += clientData.DataInfo.InstructionLength;
            imageWidthBytes.CopyTo(InstructionData, index); //图像宽度数据

            index += imageWidthBytes.Length;
            imageHeightBytes.CopyTo(InstructionData, index);//图像高度数据 + 图像信息数据 + 图像数据

            index += imageHeightBytes.Length;
            imageInformationBytes.CopyTo(InstructionData, index);//图像信息数据 + 图像数据

            index += imageInformationBytes.Length;
            imageLengthBytes.CopyTo(InstructionData, index);//图像数据长度

            if (null != image)//图像有效
            {
                index += imageLengthBytes.Length;
                image.Bytes.CopyTo(InstructionData, index);//图像数据
            }
            return InstructionData;
        }

        //----------------------------------------------------------------------
        // 功能说明：生成指令
        // 输入参数：1.Byte：InstructionType，指令类型
        //         2.Byte：Cameratype，相机类型
        //         3.VisionSystemClassLibrary.Struct.StatisticsRecordList：statisticsrecordlist，统计列表
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private Byte[] _GenerateInstruction(Byte InstructionType, Byte Cameratype, VisionSystemClassLibrary.Struct.StatisticsRecordList statisticsrecordlist)
        {
            //指令类型 + 相机类型数据 + 统计数据列表（班次个数 + 当前班次索引（从0开始） + 统计数据个数 + 当前统计数据索引（从0开始） + （每个班次）统计数据个数 + （每个班次，每个统计数据）班次开始结束时间 + （每个班次，每个统计数据）品牌（包括品牌长度））

            MemoryStream memorystream = new MemoryStream();//可扩展容量数据流

            VisionSystemClassLibrary.Struct.SYSTEMTIME systemtime = new VisionSystemClassLibrary.Struct.SYSTEMTIME();//临时变量
            Int32 iValue = BitConverter.GetBytes(systemtime.Year).Length;//临时变量

            Byte[] byteValue_1 = new Byte[1];//填充待发送数据，指令标志位
            byteValue_1[0] = InstructionType;//填充待发送数据，指令标志位
            memorystream.Write(byteValue_1, 0, byteValue_1.Length);//追加写入到可扩容数据流中

            Byte[] byteValue_2 = new Byte[1];//填充待发送数据，相机类型
            byteValue_2[0] = Cameratype;//填充待发送数据，相机类型
            memorystream.Write(byteValue_2, 0, byteValue_2.Length);//追加写入到可扩容数据流中

            Byte[] byteValue_3 = BitConverter.GetBytes(statisticsrecordlist.RecordListData.Length);//班次个数
            memorystream.Write(byteValue_3, 0, byteValue_3.Length);//追加写入到可扩容数据流中

            Byte[] byteValue_4 = BitConverter.GetBytes(statisticsrecordlist.CurrentShiftIndex);//当前班次索引（从0开始）
            memorystream.Write(byteValue_4, 0, byteValue_4.Length);//追加写入到可扩容数据流中

            Byte[] byteValue_5 = BitConverter.GetBytes(statisticsrecordlist.RecordNumber);//统计数据个数
            memorystream.Write(byteValue_5, 0, byteValue_5.Length);//追加写入到可扩容数据流中

            Byte[] byteValue_6;

            if (statisticsrecordlist.CurrentShiftIndex <= 0)//当前未空班次
            {
                byteValue_6 = BitConverter.GetBytes(-1);//当前统计数据索引（从0开始）
            }
            else
            {
                byteValue_6 = BitConverter.GetBytes(statisticsrecordlist.RecordListData[statisticsrecordlist.CurrentShiftIndex - 1].CurrentStatisticsRecordIndex);//当前统计数据索引（从0开始）
            }
            memorystream.Write(byteValue_6, 0, byteValue_6.Length);//追加写入到可扩容数据流中

            if (0 < statisticsrecordlist.RecordNumber)//存在统计数据
            {
                Int32 i = 0;//循环控制变量
                Int32 j = 0;//循环控制变量

                for (i = 0; i < statisticsrecordlist.RecordListData.Length; i++)//遍历班次
                {
                    if (null != statisticsrecordlist.RecordListData[i].TimeData)//存在统计数据
                    {
                        Byte[] byteValue_7 = BitConverter.GetBytes(statisticsrecordlist.RecordListData[i].TimeData.Length);//（每个班次）统计数据个数
                        memorystream.Write(byteValue_7, 0, byteValue_7.Length);//追加写入到可扩容数据流中

                        for (j = 0; j < statisticsrecordlist.RecordListData[i].TimeData.Length; j++)//遍历统计数据
                        {
                            Byte[] byteValue_8 = new Byte[iValue * 8 * 2];//（每个班次，每个统计数据）班次开始结束时间
                            //
                            Byte[] byteValue_Temp_1 = BitConverter.GetBytes(statisticsrecordlist.RecordListData[i].TimeData[j].Start.Year);//（每个班次，每个统计数据）班次开始时间，年
                            byteValue_Temp_1.CopyTo(byteValue_8, 0);//（每个班次，每个统计数据）班次开始时间，年
                            byteValue_Temp_1 = BitConverter.GetBytes(statisticsrecordlist.RecordListData[i].TimeData[j].Start.Month);//（每个班次，每个统计数据）班次开始时间，月
                            byteValue_Temp_1.CopyTo(byteValue_8, iValue);//（每个班次，每个统计数据）班次开始时间，月
                            byteValue_Temp_1 = BitConverter.GetBytes(statisticsrecordlist.RecordListData[i].TimeData[j].Start.DayOfWeek);//（每个班次，每个统计数据）班次开始时间，星期
                            byteValue_Temp_1.CopyTo(byteValue_8, iValue * 2);//（每个班次，每个统计数据）班次开始时间，星期
                            byteValue_Temp_1 = BitConverter.GetBytes(statisticsrecordlist.RecordListData[i].TimeData[j].Start.Day);//（每个班次，每个统计数据）班次开始时间，日
                            byteValue_Temp_1.CopyTo(byteValue_8, iValue * 3);//（每个班次，每个统计数据）班次开始时间，日
                            byteValue_Temp_1 = BitConverter.GetBytes(statisticsrecordlist.RecordListData[i].TimeData[j].Start.Hour);//（每个班次，每个统计数据）班次开始时间，时
                            byteValue_Temp_1.CopyTo(byteValue_8, iValue * 4);//（每个班次，每个统计数据）班次开始时间，时
                            byteValue_Temp_1 = BitConverter.GetBytes(statisticsrecordlist.RecordListData[i].TimeData[j].Start.Minute);//（每个班次，每个统计数据）班次开始时间，分
                            byteValue_Temp_1.CopyTo(byteValue_8, iValue * 5);//（每个班次，每个统计数据）班次开始时间，分
                            byteValue_Temp_1 = BitConverter.GetBytes(statisticsrecordlist.RecordListData[i].TimeData[j].Start.Second);//（每个班次，每个统计数据）班次开始时间，秒
                            byteValue_Temp_1.CopyTo(byteValue_8, iValue * 6);//（每个班次，每个统计数据）班次开始时间，秒
                            byteValue_Temp_1 = BitConverter.GetBytes(statisticsrecordlist.RecordListData[i].TimeData[j].Start.MilliSeconds);//（每个班次，每个统计数据）班次开始时间，毫秒
                            byteValue_Temp_1.CopyTo(byteValue_8, iValue * 7);//（每个班次，每个统计数据）班次开始时间，毫秒
                            //
                            byteValue_Temp_1 = BitConverter.GetBytes(statisticsrecordlist.RecordListData[i].TimeData[j].End.Year);//（每个班次，每个统计数据）班次结束时间，年
                            byteValue_Temp_1.CopyTo(byteValue_8, iValue * 8);//（每个班次，每个统计数据）班次结束时间，年
                            byteValue_Temp_1 = BitConverter.GetBytes(statisticsrecordlist.RecordListData[i].TimeData[j].End.Month);//（每个班次，每个统计数据）班次结束时间，月
                            byteValue_Temp_1.CopyTo(byteValue_8, iValue * 9);//（每个班次，每个统计数据）班次结束时间，月
                            byteValue_Temp_1 = BitConverter.GetBytes(statisticsrecordlist.RecordListData[i].TimeData[j].End.DayOfWeek);//（每个班次，每个统计数据）班次结束时间，星期
                            byteValue_Temp_1.CopyTo(byteValue_8, iValue * 10);//（每个班次，每个统计数据）班次结束时间，星期
                            byteValue_Temp_1 = BitConverter.GetBytes(statisticsrecordlist.RecordListData[i].TimeData[j].End.Day);//（每个班次，每个统计数据）班次结束时间，日
                            byteValue_Temp_1.CopyTo(byteValue_8, iValue * 11);//（每个班次，每个统计数据）班次结束时间，日
                            byteValue_Temp_1 = BitConverter.GetBytes(statisticsrecordlist.RecordListData[i].TimeData[j].End.Hour);//（每个班次，每个统计数据）班次结束时间，时
                            byteValue_Temp_1.CopyTo(byteValue_8, iValue * 12);//（每个班次，每个统计数据）班次结束时间，时
                            byteValue_Temp_1 = BitConverter.GetBytes(statisticsrecordlist.RecordListData[i].TimeData[j].End.Minute);//（每个班次，每个统计数据）班次结束时间，分
                            byteValue_Temp_1.CopyTo(byteValue_8, iValue * 13);//（每个班次，每个统计数据）班次结束时间，分
                            byteValue_Temp_1 = BitConverter.GetBytes(statisticsrecordlist.RecordListData[i].TimeData[j].End.Second);//（每个班次，每个统计数据）班次结束时间，秒
                            byteValue_Temp_1.CopyTo(byteValue_8, iValue * 14);//（每个班次，每个统计数据）班次结束时间，秒
                            byteValue_Temp_1 = BitConverter.GetBytes(statisticsrecordlist.RecordListData[i].TimeData[j].End.MilliSeconds);//（每个班次，每个统计数据）班次结束时间，毫秒
                            byteValue_Temp_1.CopyTo(byteValue_8, iValue * 15);//（每个班次，每个统计数据）班次结束时间，毫秒
                            //
                            memorystream.Write(byteValue_8, 0, byteValue_8.Length);//追加写入到可扩容数据流中

                            Byte[] byteValue_Temp_2 = VisionSystemCommunicationLibrary.Ethernet.TestControlFunctions._StringToByte(statisticsrecordlist.RecordListData[i].BrandName[j]);//（每个班次，每个统计数据）品牌（包括品牌长度）
                            byteValue_Temp_1 = BitConverter.GetBytes(byteValue_Temp_2.Length);//（每个班次，每个统计数据）品牌（包括品牌长度）

                            //
                            Byte[] byteValue_9 = new Byte[byteValue_Temp_1.Length + byteValue_Temp_2.Length];//（每个班次，每个统计数据）品牌（包括品牌长度）
                            byteValue_Temp_1.CopyTo(byteValue_9, 0);
                            byteValue_Temp_2.CopyTo(byteValue_9, byteValue_Temp_1.Length);
                            memorystream.Write(byteValue_9, 0, byteValue_9.Length);//追加写入到可扩容数据流中
                        }
                    }
                    else//不存在统计数据
                    {
                        Byte[] byteValue_7 = BitConverter.GetBytes((Int32)0);//（每个班次）统计数据个数
                        memorystream.Write(byteValue_7, 0, byteValue_7.Length);//追加写入到可扩容数据流中
                    }
                }
            }

            Byte[] arrayInstructionData = memorystream.ToArray();//待发送数据

            memorystream.Close();

            return arrayInstructionData;
        }

        //----------------------------------------------------------------------
        // 功能说明：保存文件
        // 输入参数：1、Byte：index，数组下表索引
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _reInitShift()
        {
            //班次类初始化
            Byte[] byteIndex = new Byte[Global.CameraNumberMax];
            Int32[] toolNumber = new Int32[Global.CameraNumberMax];

            for (Byte i = 0; i < Global.CameraNumberMax; i++)//遍历当前所有相机
            {
                if ((Global.CameraChooseState & (0x01 << i)) != 0)//当前相机开启
                {
                    byteIndex[i] = Global.Camera[i].DeviceInformation.Port;
                    toolNumber[i] = Global.Camera[i].Tools.Count;
                }
            }
            Global.ShiftInformation = new VisionSystemClassLibrary.Class.Shift(".\\", byteIndex, toolNumber, Global.CameraNumberMax, Global.CameraChooseState);
            Global.ShiftInformation.ShiftChange += new EventHandler(_ShiftChange);
        }

        //----------------------------------------------------------------------
        // 功能说明：保存文件
        // 输入参数：1.ClientData，客户端数据
        //         2.sFilePath,文件目录
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _saveFileData(VisionSystemCommunicationLibrary.Ethernet.DataReceivedEventArgs ClientData, String sFilePath)
        {
            Directory.CreateDirectory(sFilePath);//创建路径

            Byte[] byteFileName = new Byte[ClientData.DataInfo.FileNameLength];//文件名称（包含扩展名）
            System.Array.Copy(ClientData.ReceivedData, ClientData.DataInfo.FileNameIndex, byteFileName, 0, ClientData.DataInfo.FileNameLength);
            String sFileName = VisionSystemCommunicationLibrary.Ethernet.TestControlFunctions._ByteToString(byteFileName, 0, byteFileName.Length);//文件名称（包含扩展名）

            FileStream filestream = new FileStream(sFilePath + sFileName, FileMode.Create, FileAccess.Write, FileShare.None, 8192, FileOptions.WriteThrough); //打开文件
            BinaryWriter binarywriter = new BinaryWriter(filestream);
            binarywriter.Write(ClientData.ReceivedData, ClientData.DataInfo.FileDataIndex, ClientData.DataInfo.FileDataLength);//写入文件

            binarywriter.Close();//关闭文件
            filestream.Close();
        }

        //----------------------------------------------------------------------
        // 功能说明：设置计算机名
        // 输入参数：1.computerNewName：修改后计算机名称
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetComputerNameRx(String computerNewName)
        {
            RegistryKey myRKCN = Registry.LocalMachine.OpenSubKey("SYSTEM\\ControlSet001\\Services\\Tcpip\\Parameters", true);
            foreach (string site in myRKCN.GetValueNames())
            {
                if (site == "NV Hostname")
                {
                    myRKCN.DeleteValue(site, false);
                    myRKCN.SetValue("NV Hostname", computerNewName);
                }
                else
                {
                    continue;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：获取计算机名
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private String _GetComputerNameRx()
        {
            RegistryKey myRKCN = Registry.LocalMachine.OpenSubKey("SYSTEM\\ControlSet001\\Services\\Tcpip\\Parameters", true);
            foreach (string site in myRKCN.GetValueNames())
            {
                if (site == "NV Hostname")
                {
                    return site;
                }
                else
                {
                    continue;
                }
            }
            return "";
        }

        //-----------------------------------------------------------------------
        // 功能说明：定时器2响应函数，停车保存、删除统计信息
        // 输入参数： 1、object：sender，timer控件对象
        //            2、EventArgs：e，timer控件事件响应参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Enabled = false;

            if (Global.ShiftInformation.ShiftState)//班次状态使能
            {
                for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
                {
                    if ((Global.CameraChooseState & (0x01 << index)) != 0)//当前相机开启
                    {
                        if (Global.MachineStopState[index] != Global.MachineStopStateTemp[index])//当前机器正常运行
                        {
                            Global.MachineStopStateTemp[index] = Global.MachineStopState[index];
                        }
                        else//当前机器停机
                        {
                            Global.MachineStopStateTemp[index] = Global.MachineStopState[index] = 0;

                            if (Global.MachineStopStaticSaveState[index])//当前停机保存统计数据
                            {
                                Global.MachineStopStaticSaveState[index] = false;

                                if ((threadDeleteStatics == null) || (false == threadDeleteStatics.IsAlive))//班次统计线程无效，或是线程未执行，重新启动
                                {
                                    threadDeleteStatics = new Thread(_threadDeleteStatics);
                                    threadDeleteStatics.Priority = System.Threading.ThreadPriority.BelowNormal;
                                    threadDeleteStatics.IsBackground = true;
                                    threadDeleteStatics.Start();
                                }

                                Boolean bContainsKey = true;
                                lock (LockCheck_CameraPort)
                                {
                                    bContainsKey = Global.Check_CameraPort.ContainsKey(Global.Camera[index].Type);
                                }

                                if (bContainsKey) //相机有效
                                {
                                    lock (LockStaticRejectImageSavePort[index])
                                    {
                                        Global.ShiftInformation._WriteCurrentShiftOldInformation(Global.Camera[index].DeviceInformation.Port, Global.ShiftInformation.CurrentShiftIndex);
                                    }
                                }
                            }
                        }
                    }
                }

            }
            _MachineFaultSave();//保存机器故障状态

            //ClearMemory();

            timer2.Enabled = true;
        }

        //-----------------------------------------------------------------------
        // 功能说明：上电删除统计数据
        // 输入参数：无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _threadDeleteStatics()
        {
            lock (LockCheck_CameraPort)
            {
                if (Global.Check_CameraPort.Count > 0)  //包含相机
                {
                    Global.Check_CameraPort.Clear();
                }
                _ClearRelevancyImageBuffer();
            }

            Global.ShiftInformation._DeleteStatics(Global.CameraNumberMax, Global.CameraChooseState, 0.7, 0.6);//删除历史统计数据

            lock (LockCheck_CameraPort)
            {
                for (Byte i = 0; i < Global.CameraNumberMax; i++)//循环
                {
                    if ((Global.CameraChooseState & (0x01 << i)) != 0)//当前相机开启
                    {
                        Global.Check_CameraPort.Add(Global.Camera[i].Type, Global.Camera[i].DeviceInformation.Port);
                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：统计界面删除统计数据
        // 输入参数：无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _threadStatistics_DeleteRecord(object oObject)
        {
            try
            {
                DeleteRecord_ThreadParameter deleteRecord_ThreadParameter = (DeleteRecord_ThreadParameter)oObject;

                switch (deleteRecord_ThreadParameter.Type)
                {
                    case 0:
                        lock (LockCheck_CameraPort)
                        {
                            if (Global.Check_CameraPort.ContainsKey(Global.Camera[deleteRecord_ThreadParameter.index].Type)) //包含相机
                            {
                                Global.Check_CameraPort.Remove(Global.Camera[deleteRecord_ThreadParameter.index].Type);
                            }
                            _ClearRelevancyImageBuffer();
                        }

                        while (true)
                        {
                            if (VisionSystemClassLibrary.Enum.RelevancyType.None < Global.Camera[deleteRecord_ThreadParameter.index].RelevancyCameraInfo.rRelevancyType) //相机含有关联信息
                            {
                                if (RelevancyImageSaveState) //未在执行缺陷保存
                                {
                                    break;
                                }
                            }
                            else //相机未包含关联信息
                            {
                                if (StaticRejectImageSaveStatePort[deleteRecord_ThreadParameter.index]) //未在执行缺陷保存
                                {
                                    break;
                                }
                            }
                            Thread.Sleep(Global.ImageDataTime);//循环数据传输休眠10ms
                        }

                        lock (LockStaticRejectImageSavePort[deleteRecord_ThreadParameter.index])
                        {
                            Global.ShiftInformation._DeleteSingleCameraInformation(Global.Camera[deleteRecord_ThreadParameter.index].DeviceInformation.Port);//清除相机统计信息
                        }

                        lock (LockCheck_CameraPort)
                        {
                            if (false == Global.Check_CameraPort.ContainsKey(Global.Camera[deleteRecord_ThreadParameter.index].Type)) //未包含该相机
                            {
                                Global.Check_CameraPort.Add(Global.Camera[deleteRecord_ThreadParameter.index].Type, Global.Camera[deleteRecord_ThreadParameter.index].DeviceInformation.Port);
                            }
                        }
                        break;
                    case 1:
                        lock (LockCheck_CameraPort)
                        {
                            if (Global.Check_CameraPort.ContainsKey(Global.Camera[deleteRecord_ThreadParameter.index].Type)) //包含相机
                            {
                                Global.Check_CameraPort.Remove(Global.Camera[deleteRecord_ThreadParameter.index].Type);
                            }
                            _ClearRelevancyImageBuffer();
                        }

                        while (true)
                        {
                            if (VisionSystemClassLibrary.Enum.RelevancyType.None < Global.Camera[deleteRecord_ThreadParameter.index].RelevancyCameraInfo.rRelevancyType) //相机含有关联信息
                            {
                                if (RelevancyImageSaveState) //未在执行缺陷保存
                                {
                                    break;
                                }
                            }
                            else //相机未包含关联信息
                            {
                                if (StaticRejectImageSaveStatePort[deleteRecord_ThreadParameter.index]) //未在执行缺陷保存
                                {
                                    break;
                                }
                            }
                            Thread.Sleep(Global.ImageDataTime);//循环数据传输休眠10ms
                        }

                        lock (LockStaticRejectImageSavePort[deleteRecord_ThreadParameter.index])
                        {
                            Global.ShiftInformation._DeleteShiftInformation(Global.Camera[deleteRecord_ThreadParameter.index].DeviceInformation.Port, deleteRecord_ThreadParameter.shiftIndex);//清除班次统计信息
                        }

                        lock (LockCheck_CameraPort)
                        {
                            if (false == Global.Check_CameraPort.ContainsKey(Global.Camera[deleteRecord_ThreadParameter.index].Type)) //未包含该相机
                            {
                                Global.Check_CameraPort.Add(Global.Camera[deleteRecord_ThreadParameter.index].Type, Global.Camera[deleteRecord_ThreadParameter.index].DeviceInformation.Port);
                            }
                        }
                        break;
                    case 2:
                        lock (LockCheck_CameraPort)
                        {
                            if (Global.Check_CameraPort.ContainsKey(Global.Camera[deleteRecord_ThreadParameter.index].Type)) //包含相机
                            {
                                Global.Check_CameraPort.Remove(Global.Camera[deleteRecord_ThreadParameter.index].Type);
                            }
                            _ClearRelevancyImageBuffer();
                        }

                        while (true)
                        {
                            if (VisionSystemClassLibrary.Enum.RelevancyType.None < Global.Camera[deleteRecord_ThreadParameter.index].RelevancyCameraInfo.rRelevancyType) //相机含有关联信息
                            {
                                if (RelevancyImageSaveState) //未在执行缺陷保存
                                {
                                    break;
                                }
                            }
                            else //相机未包含关联信息
                            {
                                if (StaticRejectImageSaveStatePort[deleteRecord_ThreadParameter.index]) //未在执行缺陷保存
                                {
                                    break;
                                }
                            }
                            Thread.Sleep(Global.ImageDataTime);//循环数据传输休眠10ms
                        }

                        lock (LockStaticRejectImageSavePort[deleteRecord_ThreadParameter.index])
                        {
                            Global.ShiftInformation._DeleteHistoryShiftInformation(Global.Camera[deleteRecord_ThreadParameter.index].DeviceInformation.Port, deleteRecord_ThreadParameter.shiftIndex, deleteRecord_ThreadParameter.start, deleteRecord_ThreadParameter.end);//清除历史班次统计信息
                        }

                        lock (LockCheck_CameraPort)
                        {
                            if (false == Global.Check_CameraPort.ContainsKey(Global.Camera[deleteRecord_ThreadParameter.index].Type)) //未包含该相机
                            {
                                Global.Check_CameraPort.Add(Global.Camera[deleteRecord_ThreadParameter.index].Type, Global.Camera[deleteRecord_ThreadParameter.index].DeviceInformation.Port);
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (System.Exception ex)
            {

            }
            DeletingStaticsResult = 1;//删除成功
        }

        //-----------------------------------------------------------------------
        // 功能说明：清楚关联缓存
        // 输入参数：无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _ClearRelevancyImageBuffer()
        {
            for (Byte k = 0; k < Global.CameraNumberMax; k++) //删除统计数据时，清空所有缓存，保证图像对应
            {
                if (null != RelevancyImageInformations[k]) //图像信息不为空
                {
                    RelevancyImageInformations[k].Clear();
                }

                if (null != RelevancyImages[k]) //图像信息不为空
                {
                    RelevancyImages[k].Clear();
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：USB口测试数设置测试
        // 输入参数：1、String[]：serialPortNames，串口名称集合
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _USBPortRecognise(string[] serialPortNames)
        {
            string[] USBPortNames = SerialPort.GetPortNames();

            Int32 i, j;

            for (i = 0; i < USBPortNames.Length; i++)//遍历查询新USB口
            {
                for (j = 0; j < serialPortNames.Length; j++)//遍历比较是否发现新的USB口
                {
                    if (USBPortNames[i] == serialPortNames[j])//发现该USB口已存在，则跳出
                    {
                        break;
                    }
                }

                if (j == serialPortNames.Length)//发现新的USB口
                {
                    try
                    {
                        USBPortCommucation = new SerialPort();
                        USBPortCommucation.PortName = USBPortNames[i];
                        USBPortCommucation.BaudRate = 57600;
                        USBPortCommucation.ReceivedBytesThreshold = 1;
                        USBPortCommucation.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(_USBPortCommucation_DataReceived);

                        USBPortCommucation.Open();

                        CommandUSB[0] = 0;
                        Byte[] sendUSB = new byte[1] { 10 };
                        USBPortCommucation.Write(sendUSB, 0, 1);
                    }
                    catch
                    {

                    }

                    Thread.Sleep(100);//暂停100ms，后读取返回值

                    if (CommandUSB[0] == 10)//命令查询成功
                    {
                        USBPortTestResult = 1;
                    }
                    else
                    {
                        USBPortTestResult = 0;
                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：定时清除无效内存
        // 输入参数：无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private static void ClearMemory()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                SetProcessingWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：定时器3响应函数，软件重启
        // 输入参数： 1、object：sender，timer控件对象
        //            2、EventArgs：e，timer控件事件响应参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void timer3_Tick(object sender, EventArgs e)
        {
            timer3.Enabled = false;

            if (true == CloseSerialPortFlag) //当前执行关闭串口操作，软件或系统发生重启启动
            {
                if (Global.ComputerRunState) //非控制器上运行软件
                {
                    switch (CloseSerialPort_ComType)  //执行命令类型
                    {
                        case CommunicationInstructionType.ClientSystem_Update:
                        case CommunicationInstructionType.BrandManagement_LoadBrand:
                        case CommunicationInstructionType.SystemParameter:

                            //Application.ExitThread();//退出应用程序所有线程
                            Application.Exit();//退出应用程序

                            Process.Start(
                                System.Reflection.Assembly.GetExecutingAssembly().Location);//重新启动应用程序

                            Process.GetCurrentProcess().Kill();
                            break;
                        case CommunicationInstructionType.DevicesSetup_ConfigDevice:

                            if (ShutDown_DevicesSetup_ConfigDevice)//执行计算机重启
                            {
                                Process.Start("shutdown", "-r -t 1");//重启
                            }
                            else
                            {
                                Application.Exit();//退出应用程序

                                Process.Start(
                                System.Reflection.Assembly.GetExecutingAssembly().Location);//重新启动应用程序

                                Process.GetCurrentProcess().Kill();
                            }
                            break;
                        case CommunicationInstructionType.DevicesSetup_ResetDevice:

                            Process.Start("shutdown", "-r -t 1");//重启
                            break;
                    }
                }
                else//控制器上运行软件
                {
                    if ((ControllerSerialPortCommucation.BytesToWrite == 0)) //发送缓冲区数据已发送完毕
                    {
                        lock (LockControllerSerialPort)
                        {
                            ControllerSerialPortCommucation.Close();
                        }

                        switch (CloseSerialPort_ComType)  //执行命令类型
                        {
                            case CommunicationInstructionType.ClientSystem_Update:
                            case CommunicationInstructionType.BrandManagement_LoadBrand:
                            case CommunicationInstructionType.SystemParameter:

                                //Application.ExitThread();//退出应用程序所有线程
                                Application.Exit();//退出应用程序

                                Process.Start(
                                    System.Reflection.Assembly.GetExecutingAssembly().Location);//重新启动应用程序

                                Process.GetCurrentProcess().Kill();
                                break;
                            case CommunicationInstructionType.DevicesSetup_ConfigDevice:

                                if (ShutDown_DevicesSetup_ConfigDevice)//执行计算机重启
                                {
                                    Process.Start("shutdown", "-r -t 1");//重启
                                }
                                else
                                {
                                    Application.Exit();//退出应用程序

                                    Process.Start(
                                    System.Reflection.Assembly.GetExecutingAssembly().Location);//重新启动应用程序

                                    Process.GetCurrentProcess().Kill();
                                }
                                break;
                            case CommunicationInstructionType.DevicesSetup_ResetDevice:

                                Process.Start("shutdown", "-r -t 1");//重启
                                break;
                        }
                    }
                    else  //发送缓冲区还有为发送数据，继续查询
                    {
                        timer3.Enabled = true;
                    }
                }
            }
            else
            {
                timer3.Enabled = true;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：相机1恢复外部触发响应函数
        // 输入参数： 1、object：sender，timer控件对象
        //            2、EventArgs：e，timer控件事件响应参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void Work_Tick1(object sender, EventArgs e)
        {
            if ((Global.MachineFaultState & 0x02) == 0)  //相机不存在故障
            {
                try
                {
                    tCameraRestartCount[0].Enabled = false;

                    if ((icImagingControlPort[0] != null) && (icImagingControlPort[0].DeviceValid))//相机正常工作，则停用
                    {
                        icImagingControlPort[0].Invoke(new EventHandler(delegate { icImagingControlPort[0].LiveStop(); }));
                    }

                    icImagingControlPort[0].ImageAvailable += new System.EventHandler<
                        TIS.Imaging.ICImagingControl.ImageAvailableEventArgs>(_icImagingControlPort1_ImageAvailable);//触发端口1相机捕获图像事件
                    icImagingControlPort[0].DeviceLost += new System.EventHandler<
                        TIS.Imaging.ICImagingControl.DeviceLostEventArgs>(_icImagingControlPort1_DeviceLost);//触发端口1相机不正常工作事件

                    Boolean bInitCameraResult = false;

                    if (false == Global.ComputerRunState) //控制器上运行软件
                    {
                        bInitCameraResult = _InitCamera(0, !Global.ComputerRunState, Global.Camera[0].VideoFormat, Global.Camera[0].DeviceFrameRate, Global.ImageRingBufferSizeMax);
                    }
                    else
                    {
                        bInitCameraResult = _InitCamera(0, !Global.ComputerRunState, Global.Camera[0].VideoFormat, 10, Global.ImageRingBufferSizeMax);
                    }

                    if (bInitCameraResult)//测试成功
                    {
                        icImagingControlPort[0].LiveStart();

                        lock (LockCheck_CameraPort)
                        {
                            if (false == Global.Check_CameraPort.ContainsKey(Global.Camera[0].Type)) //未包含该相机
                            {
                                Global.Check_CameraPort.Add(Global.Camera[0].Type, Global.Camera[0].DeviceInformation.Port);
                            }
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    lock (Global.LockMachineFaultState)
                    {
                        Global.MachineFaultState |= 0x02;
                    }

                    Global.Camera[0].DeviceInformation.CAM = VisionSystemClassLibrary.Enum.CameraState.OFF;
                    Global.CameraTemp[0].DeviceInformation.CAM = Global.Camera[0].DeviceInformation.CAM;

                    lock (LockCheck_CameraPort)
                    {
                        if (VisionSystemClassLibrary.Enum.RelevancyType.None < Global.Camera[0].RelevancyCameraInfo.rRelevancyType) //相机含有关联信息
                        {
                            if (Global.Check_CameraPort.ContainsKey(Global.Camera[0].Type)) //包含相机
                            {
                                Global.Check_CameraPort.Remove(Global.Camera[0].Type);
                            }
                            _ClearRelevancyImageBuffer();
                        }
                    }

                    lock (LockControllerSerialPort)//相机下电
                    {
                        _CameraPowerOff(0);                                         //端口1相机下电
                    }
                    PowerStatePort[0] = 10;
                    
                    label5.Invoke(new EventHandler(delegate { label5.Text = "相机1初始化异常tick"; }));
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：相机2恢复外部触发响应函数
        // 输入参数： 1、object：sender，timer控件对象
        //            2、EventArgs：e，timer控件事件响应参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void Work_Tick2(object sender, EventArgs e)
        {
            if ((Global.MachineFaultState & 0x04) == 0)  //相机不存在故障
            {
                try
                {
                    tCameraRestartCount[1].Enabled = false;

                    if ((icImagingControlPort[1] != null) && (icImagingControlPort[1].DeviceValid))//相机正常工作，则停用
                    {
                        icImagingControlPort[1].Invoke(new EventHandler(delegate { icImagingControlPort[1].LiveStop(); }));
                    }

                    icImagingControlPort[1].ImageAvailable += new System.EventHandler<
                        TIS.Imaging.ICImagingControl.ImageAvailableEventArgs>(_icImagingControlPort2_ImageAvailable);//触发端口1相机捕获图像事件
                    icImagingControlPort[1].DeviceLost += new System.EventHandler<
                        TIS.Imaging.ICImagingControl.DeviceLostEventArgs>(_icImagingControlPort2_DeviceLost);//触发端口1相机不正常工作事件

                    Boolean bInitCameraResult = false;

                    if (false == Global.ComputerRunState) //控制器上运行软件
                    {
                        bInitCameraResult = _InitCamera(1, !Global.ComputerRunState, Global.Camera[1].VideoFormat, Global.Camera[1].DeviceFrameRate, Global.ImageRingBufferSizeMax);
                    }
                    else
                    {
                        bInitCameraResult = _InitCamera(1, !Global.ComputerRunState, Global.Camera[1].VideoFormat, 10, Global.ImageRingBufferSizeMax);
                    }

                    if (bInitCameraResult)//测试成功
                    {
                        icImagingControlPort[1].LiveStart();

                        lock (LockCheck_CameraPort)
                        {
                            if (false == Global.Check_CameraPort.ContainsKey(Global.Camera[1].Type)) //未包含该相机
                            {
                                Global.Check_CameraPort.Add(Global.Camera[1].Type, Global.Camera[1].DeviceInformation.Port);
                            }
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    lock (Global.LockMachineFaultState)
                    {
                        Global.MachineFaultState |= 0x04;
                    }

                    Global.Camera[1].DeviceInformation.CAM = VisionSystemClassLibrary.Enum.CameraState.OFF;
                    Global.CameraTemp[1].DeviceInformation.CAM = Global.Camera[1].DeviceInformation.CAM;

                    lock (LockCheck_CameraPort)
                    {
                        if (VisionSystemClassLibrary.Enum.RelevancyType.None < Global.Camera[1].RelevancyCameraInfo.rRelevancyType) //相机含有关联信息
                        {
                            if (Global.Check_CameraPort.ContainsKey(Global.Camera[1].Type)) //包含相机
                            {
                                Global.Check_CameraPort.Remove(Global.Camera[1].Type);
                            }
                            _ClearRelevancyImageBuffer();
                        }
                    }

                    lock (LockControllerSerialPort)//相机下电
                    {
                        _CameraPowerOff(1);                                         //端口2相机下电
                    }
                    PowerStatePort[1] = 10;
                    
                    label2.Invoke(new EventHandler(delegate { label2.Text = "相机2初始化异常tick"; }));
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：相机3恢复外部触发响应函数
        // 输入参数： 1、object：sender，timer控件对象
        //            2、EventArgs：e，timer控件事件响应参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void Work_Tick3(object sender, EventArgs e)
        {
            if ((Global.MachineFaultState & 0x010000000000) == 0)  //相机不存在故障
            {
                try
                {
                    tCameraRestartCount[2].Enabled = false;

                    if ((icImagingControlPort[2] != null) && (icImagingControlPort[2].DeviceValid))//相机正常工作，则停用
                    {
                        icImagingControlPort[2].Invoke(new EventHandler(delegate { icImagingControlPort[2].LiveStop(); }));
                    }

                    icImagingControlPort[2].ImageAvailable += new System.EventHandler<
                        TIS.Imaging.ICImagingControl.ImageAvailableEventArgs>(_icImagingControlPort3_ImageAvailable);//触发端口1相机捕获图像事件
                    icImagingControlPort[2].DeviceLost += new System.EventHandler<
                        TIS.Imaging.ICImagingControl.DeviceLostEventArgs>(_icImagingControlPort3_DeviceLost);//触发端口1相机不正常工作事件

                    Boolean bInitCameraResult = false;

                    if (false == Global.ComputerRunState) //控制器上运行软件
                    {
                        bInitCameraResult = _InitCamera(2, !Global.ComputerRunState, Global.Camera[2].VideoFormat, Global.Camera[2].DeviceFrameRate, Global.ImageRingBufferSizeMax);
                    }
                    else
                    {
                        bInitCameraResult = _InitCamera(2, !Global.ComputerRunState, Global.Camera[2].VideoFormat, 10, Global.ImageRingBufferSizeMax);
                    }

                    if (bInitCameraResult)//测试成功
                    {
                        icImagingControlPort[2].LiveStart();

                        lock (LockCheck_CameraPort)
                        {
                            if (false == Global.Check_CameraPort.ContainsKey(Global.Camera[2].Type)) //未包含该相机
                            {
                                Global.Check_CameraPort.Add(Global.Camera[2].Type, Global.Camera[2].DeviceInformation.Port);
                            }
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    lock (Global.LockMachineFaultState)
                    {
                        Global.MachineFaultState |= 0x010000000000;
                    }

                    Global.Camera[2].DeviceInformation.CAM = VisionSystemClassLibrary.Enum.CameraState.OFF;
                    Global.CameraTemp[2].DeviceInformation.CAM = Global.Camera[2].DeviceInformation.CAM;

                    lock (LockCheck_CameraPort)
                    {
                        if (VisionSystemClassLibrary.Enum.RelevancyType.None < Global.Camera[2].RelevancyCameraInfo.rRelevancyType) //相机含有关联信息
                        {
                            if (Global.Check_CameraPort.ContainsKey(Global.Camera[2].Type)) //包含相机
                            {
                                Global.Check_CameraPort.Remove(Global.Camera[2].Type);
                            }
                            _ClearRelevancyImageBuffer();
                        }
                    }

                    lock (LockControllerSerialPort)//相机下电
                    {
                        _CameraPowerOff(3);                                         //端口3相机下电
                    }
                    PowerStatePort[2] = 10;

                    label13.Invoke(new EventHandler(delegate { label13.Text = "相机3初始化异常tick"; }));
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：相机4恢复外部触发响应函数
        // 输入参数： 1、object：sender，timer控件对象
        //            2、EventArgs：e，timer控件事件响应参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void Work_Tick4(object sender, EventArgs e)
        {
            if ((Global.MachineFaultState & 0x020000000000) == 0)  //相机不存在故障
            {
                try
                {
                    tCameraRestartCount[3].Enabled = false;

                    if ((icImagingControlPort[3] != null) && (icImagingControlPort[3].DeviceValid))//相机正常工作，则停用
                    {
                        icImagingControlPort[3].Invoke(new EventHandler(delegate { icImagingControlPort[3].LiveStop(); }));
                    }

                    icImagingControlPort[3].ImageAvailable += new System.EventHandler<
                        TIS.Imaging.ICImagingControl.ImageAvailableEventArgs>(_icImagingControlPort4_ImageAvailable);//触发端口1相机捕获图像事件
                    icImagingControlPort[3].DeviceLost += new System.EventHandler<
                        TIS.Imaging.ICImagingControl.DeviceLostEventArgs>(_icImagingControlPort4_DeviceLost);//触发端口1相机不正常工作事件

                    Boolean bInitCameraResult = false;

                    if (false == Global.ComputerRunState) //控制器上运行软件
                    {
                        bInitCameraResult = _InitCamera(3, !Global.ComputerRunState, Global.Camera[3].VideoFormat, Global.Camera[3].DeviceFrameRate, Global.ImageRingBufferSizeMax);
                    }
                    else
                    {
                        bInitCameraResult = _InitCamera(3, !Global.ComputerRunState, Global.Camera[3].VideoFormat, 10, Global.ImageRingBufferSizeMax);
                    }

                    if (bInitCameraResult)//测试成功
                    {
                        icImagingControlPort[3].LiveStart();

                        lock (LockCheck_CameraPort)
                        {
                            if (false == Global.Check_CameraPort.ContainsKey(Global.Camera[3].Type)) //未包含该相机
                            {
                                Global.Check_CameraPort.Add(Global.Camera[3].Type, Global.Camera[3].DeviceInformation.Port);
                            }
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    lock (Global.LockMachineFaultState)
                    {
                        Global.MachineFaultState |= 0x020000000000;
                    }

                    Global.Camera[3].DeviceInformation.CAM = VisionSystemClassLibrary.Enum.CameraState.OFF;
                    Global.CameraTemp[3].DeviceInformation.CAM = Global.Camera[3].DeviceInformation.CAM;

                    lock (LockCheck_CameraPort)
                    {
                        if (VisionSystemClassLibrary.Enum.RelevancyType.None < Global.Camera[3].RelevancyCameraInfo.rRelevancyType) //相机含有关联信息
                        {
                            if (Global.Check_CameraPort.ContainsKey(Global.Camera[3].Type)) //包含相机
                            {
                                Global.Check_CameraPort.Remove(Global.Camera[3].Type);
                            }
                            _ClearRelevancyImageBuffer();
                        }
                    }

                    lock (LockControllerSerialPort)//相机下电
                    {
                        _CameraPowerOff(4);                                         //端口4相机下电
                    }
                    PowerStatePort[3] = 10;

                    label15.Invoke(new EventHandler(delegate { label15.Text = "相机4初始化异常tick"; }));
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：定时器1响应函数，实验室串口查询实时电压
        // 输入参数： 1、object：sender，timer控件对象
        //            2、EventArgs：e，timer控件事件响应参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void timer4_Tick(object sender, EventArgs e)
        {
            for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
            {
                if ((Global.CameraChooseState & (0x01 << index)) != 0)//当前相机开启
                {
                    if (Global.Camera[index].IsSerialPort && bSerialPortLabState[index]) //当前为串口,且进入实验室模式
                    {
                        lock (LockSerialPort[index])
                        {
                            _SendCommand_SerialPortCommucation(4, index);
                        }
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：连接服务端
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _tcpCleints_Connect1()
        {
            Thread.Sleep(1000);

            if (!tcpClient1.dllConnected) //客户端未连接
            {
                tcpClient1.Connect2();
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：连接服务器
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void tcpClient1_ConnectWithServerSuccess(object sender, EventArgs e)
        {
            if (Global.ShowInformation)//显示调试信息
            {
                label4.Invoke(new EventHandler(delegate { label4.Text = "连接"; }));
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：断开连接
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void tcpClient1_DisConnectWithServer(object sender, SocketErrorEventArgs e)
        {
             if (Global.ShowInformation)//显示调试信息
            {
                label4.Invoke(new EventHandler(delegate { label4.Text = "断开"; }));
            }

             Thread.Sleep(1000);

            //连接以太网服务端
            Thread tcpCleintsConnectedThread1 = new Thread(_tcpCleints_Connect1);
            tcpCleintsConnectedThread1.IsBackground = true;
            tcpCleintsConnectedThread1.Start();
        }

        //----------------------------------------------------------------------
        // 功能说明：连接服务端
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _tcpCleints_Connect2()
        {
            Thread.Sleep(1000);

            if (!tcpClient2.dllConnected) //客户端未连接
            {
                tcpClient2.Connect2();
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：连接服务器
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void tcpClient2_ConnectWithServerSuccess(object sender, EventArgs e)
        {
            if (Global.ShowInformation)//显示调试信息
            {
                label8.Invoke(new EventHandler(delegate { label8.Text = "连接"; }));
            }
        }

        /// <summary>
        /// 89713FA光电手动校准定时器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_89713FA_Tick(object sender, EventArgs e)
        {
            for (Byte index = 0; index < Global.CameraNumberMax; index++)//遍历当前所有相机
            {
                if ((Global.CameraChooseState & (0x01 << index)) != 0)//当前相机开启
                {
                    if (Global.Camera[index].IsSerialPort) //当前为串口
                    {
                        lock (LockSerialPort[index])
                        {
                            _SendCommand_SerialPortCommucation(7, index, true);
                        }
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：断开连接
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void tcpClient2_DisConnectWithServer(object sender, SocketErrorEventArgs e)
        {
            if (Global.ShowInformation)//显示调试信息
            {
                label8.Invoke(new EventHandler(delegate { label8.Text = "断开"; }));
            }

            Thread.Sleep(1000);

            //连接以太网服务端
            Thread tcpCleintsConnectedThread2 = new Thread(_tcpCleints_Connect2);
            tcpCleintsConnectedThread2.IsBackground = true;
            tcpCleintsConnectedThread2.Start();
        }

        //----------------------------------------------------------------------
        // 功能说明：连接服务端
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _tcpCleints_Connect3()
        {
            Thread.Sleep(1000);

            if (!tcpClient3.dllConnected) //客户端未连接
            {
                tcpClient3.Connect2();
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：连接服务器
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void tcpClient3_ConnectWithServerSuccess(object sender, EventArgs e)
        {
            if (Global.ShowInformation)//显示调试信息
            {
                label9.Invoke(new EventHandler(delegate { label9.Text = "连接"; }));
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：断开连接
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void tcpClient3_DisConnectWithServer(object sender, SocketErrorEventArgs e)
        {
            if (Global.ShowInformation)//显示调试信息
            {
                label9.Invoke(new EventHandler(delegate { label9.Text = "断开"; }));
            }

            Thread.Sleep(1000);

            //连接以太网服务端
            Thread tcpCleintsConnectedThread3 = new Thread(_tcpCleints_Connect3);
            tcpCleintsConnectedThread3.IsBackground = true;
            tcpCleintsConnectedThread3.Start();
        }

        //----------------------------------------------------------------------
        // 功能说明：连接服务端
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _tcpCleints_Connect4()
        {
            Thread.Sleep(1000);

            if (!tcpClient4.dllConnected) //客户端未连接
            {
                tcpClient4.Connect2();
            }
        }


        //----------------------------------------------------------------------
        // 功能说明：连接服务器
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void tcpClient4_ConnectWithServerSuccess(object sender, EventArgs e)
        {
            if (Global.ShowInformation)//显示调试信息
            {
                label11.Invoke(new EventHandler(delegate { label11.Text = "连接"; }));
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：断开连接
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void tcpClient4_DisConnectWithServer(object sender, SocketErrorEventArgs e)
        {
            if (Global.ShowInformation)//显示调试信息
            {
                label11.Invoke(new EventHandler(delegate { label11.Text = "断开"; }));
            }

            Thread.Sleep(1000);

            //连接以太网服务端
            Thread tcpCleintsConnectedThread4 = new Thread(_tcpCleints_Connect4);
            tcpCleintsConnectedThread4.IsBackground = true;
            tcpCleintsConnectedThread4.Start();
        }

        //-----------------------------------------------------------------------
        // 功能说明：定时器5响应函数，相机图像丢失时重新启动相机
        // 输入参数： 1、object：sender，timer控件对象
        //            2、EventArgs：e，timer控件事件响应参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void timer5_Tick(object sender, EventArgs e)
        {
            for (Byte i = 0; i < Global.CameraNumberMax; i++)//遍历当前所有相机
            {
                if (((Global.CameraChooseState & (0x01 << i)) != 0) && (false == Global.Camera[i].IsSerialPort))//当前相机开启
                {
                    if (true == CameraLostState[i])  //相机掉线
                    {
                        UInt64 temp = 0;

                        switch (i)
                        {
                            case 0:
                                temp = 0x02;
                                break;
                            case 1:
                                temp = 0x04;
                                break;
                            case 2:
                                temp = 0x010000000000;
                                break;
                            case 3:
                                temp = 0x020000000000;
                                break;
                            default:
                                break;
                        }

                        if (0 == (Global.MachineFaultState & temp))  //相机不存在故障
                        {
                            if ((null != icImagingControlPort[i]) && (icImagingControlPort[i].DeviceValid)) //相机在工作过程中
                            {
                                icImagingControlPort[i].LiveStop();

                                try
                                {
                                    icImagingControlPort[i].LiveStart();//重新启动相机
                                    CameraImageCount[i] = 1;
                                }
                                catch (Exception ex)
                                {
                                    if (Global.ShowInformation)//显示调试信息
                                    {
                                        switch (i)
                                        {
                                            case 0:
                                                label5.Invoke(new EventHandler(delegate { label5.Text = "相机1重启异常"; }));
                                                break;
                                            case 1:
                                                label2.Invoke(new EventHandler(delegate { label2.Text = "相机2重启异常"; }));
                                                break;
                                            case 2:
                                                label13.Invoke(new EventHandler(delegate { label13.Text = "相机3重启异常"; }));
                                                break;
                                            case 3:
                                                label15.Invoke(new EventHandler(delegate { label15.Text = "相机4重启异常"; }));
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                }

                                CameraLostNumber[i]++;
                                if (CameraLostNumber[i] >= 5)                         //烟丝侧相机掉电5次，标记相机故障
                                {
                                    Global.MachineFaultState = Global.MachineFaultState | temp;
                                    CameraLostNumber[i] = 0;
                                }
                            }
                        }
                        CameraLostState[i] = false;
                    }
                }
            }
        }

        /// <summary>
        /// 刷新状态查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_GPIO_Tick(object sender, EventArgs e)
        {
            UIntPtr level;
            //SemaEApiGPIOGetLevel((UInt32)SemaHandle, 1, 1, out level);

            //if (0 == (UInt32)level)              //刷新信号有效，恢复信息统计
            //{
            //    Global.StaticsPause = false;
            //}
            //else                                           //刷新信号无效，故障信息清零，统计暂停
            //{
            //    Global.StaticsPause = true;
            //}
        }
    }
}