/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：System.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：系统

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

using System.Reflection;//

namespace VisionSystemClassLibrary.Class
{
    [Serializable]
    public class System
    {
        private string sConfigDataPath = "";//配置文件路径（如，D:\\VisionSystemUserInterface\\ConfigData\\）

        private static string sStatisticsPathName = "Statistics\\";//统计文件夹路径
        private static string sConfigDataPathName = "ConfigData\\";//配置文件路径
        private static string sSystemFileName = "System.dat";//System数据文件名称
        private static string sRelevancyFileName = "Relevancy.dat";//关联文件名称

        //

        private static Bitmap[] bDeviceBackground = null;//设备背景图像（WORK，LIVE）
        private static Bitmap[] bTrademark_1 = null;//商标背景图像1（LOAD，REG）
        private static Bitmap[] bTrademark_2 = null;//商标背景图像2（WORK）

        //

        private List<Camera> camera = null;//属性，相机
        private Brand brand = new Brand();//属性，品牌
        private Shift shift = new Shift();//属性，班次

        //

        private static Enum.InterfaceLanguage language = Enum.InterfaceLanguage.English;//语言

        private static Enum.DeviceState systemDeviceState = Enum.DeviceState.Run;//设备状态

        //

        private string sProductModelNumber = "";//产品型号

        private string sProductName = "";//产品名称
        private string sProductName_CHN = "";//产品名称（中文）
        private string sProductName_ENG = "";//产品名称（英文）


        private Struct.WorkData wWork = new Struct.WorkData();//WORK页面数据

        private Struct.System_UIParameter uUIParameter = new Struct.System_UIParameter();//WORK页面数据

        private Struct.CameraConfiguration[] sSystemCameraConfiguration;//相机配置信息

        private string[] sSystemControllerName_ENG;//控制器英文名称
        private string[] sSystemControllerName_CHN;//控制器中文名称

        private Struct.DeviceData[] dConnectionData = new Struct.DeviceData[256];//设备信息

        private const string sDeviceIPAddress = "10.11.15.";//系统使用的IP地址段（前三段），描述如下：
                                                          //服务端IP地址末段固定为1
                                                          //客户端IP地址末段分配范围为2 ~ 相机数量（可使用的最大数量） + 1
        private string[] sMachineType = new string[2];//机器类型名称
        private Int32 iSelectedMachineType = 0;//系统设置，当前选择的机器类型（从0开始）

        private string sUserPassword = "";//用户密码
        
        //

        private static string[] sFaultMessage_CHN;//故障信息（中文）
        private static string[] sFaultMessage_ENG;//故障信息（英文）
        
        private static UInt64 uiMachineFaultEnableState;//故障信息使能标记

        //

        private Struct.IOSignal ioIOSignalData = new Struct.IOSignal();
        
        //

        private Boolean bTitlebarStyle; //标题栏风格，默认FALSE，TRUE为X6S风格，FALSE为其它
        private Int32 iMachineTypeNumber;//机器类型数量(配置工具使用)
        private Int32 iSystemCameraNumber;//相机数量(配置工具使用)

        private Int32 iData_Value;//界面切换标记：0，默认41所；1，上海烟机

        //

        //构造函数

        //----------------------------------------------------------------------
        // 功能说明：默认构造函数，初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public System()
        {
            _Init();
        }

        //----------------------------------------------------------------------
        // 功能说明：构造函数（实际使用），初始化
        // 输入参数：1.path：应用程序路径
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public System(string sApplicationPath)
        {
            Int32 i = 0;//循环控制变量
            Int32 j = 0;//循环控制变量

            Int32 iValue = 0;

            sConfigDataPath = sApplicationPath + ConfigDataPathName;//配置文件路径（如，D:\\VisionSystemUserInterface\\ConfigData\\）

            //

            _Init();

            _ReadMachineStateInfoFile(sApplicationPath);//读取机器状态信息文件
            _ReadFaultInfoFile(sApplicationPath);//读取故障信息文件

            //

            FileStream filestream = new FileStream(sConfigDataPath + sSystemFileName, FileMode.Open);//打开系统文件
            BinaryReader binaryreader = new BinaryReader(filestream);//读取系统文件数据

            filestream.Seek(0x00, SeekOrigin.Begin);
            language = (Enum.InterfaceLanguage)binaryreader.ReadByte();//语言
            bTitlebarStyle = binaryreader.ReadBoolean(); //标题栏风格
            iData_Value = binaryreader.ReadByte();//界面图标

            filestream.Seek(0x10, SeekOrigin.Begin);
            sProductName_ENG = binaryreader.ReadString();//产品名称（英文）

            filestream.Seek(0x50, SeekOrigin.Begin);
            sProductName_CHN = binaryreader.ReadString(); ;//产品名称（中文）

            filestream.Seek(0x90, SeekOrigin.Begin);
            iMachineTypeNumber = binaryreader.ReadInt32();//系统设置，机器类型数量

            sMachineType = new string[iMachineTypeNumber];
            for (i = 0; i < sMachineType.Length; i++)
            {
                filestream.Seek(0xA0 + i * 0x40, SeekOrigin.Begin);
                sMachineType[i] = binaryreader.ReadString();
            }

            iValue = iMachineTypeNumber * 0x40;

            filestream.Seek(0xA0 + iValue, SeekOrigin.Begin);
            iSelectedMachineType = binaryreader.ReadInt32();//系统设置，当前选择的机器类型（从0开始）

            filestream.Seek(0xB0 + iValue, SeekOrigin.Begin);
            sUserPassword = binaryreader.ReadString();//用户密码

            filestream.Seek(0xF0 + iValue, SeekOrigin.Begin);
            sProductModelNumber = binaryreader.ReadString();//产品型号

            filestream.Seek(0x130 + iValue, SeekOrigin.Begin);
            uUIParameter.Work_TotalPage = binaryreader.ReadInt32();
            uUIParameter.Work_BackgroundImage_Zoom = new Boolean[uUIParameter.Work_TotalPage];
            uUIParameter.Work_BackgroundImage_Location = new Point[uUIParameter.Work_TotalPage];

            for (i = 0; i < uUIParameter.Work_TotalPage; i++)
            {
                filestream.Seek(0x140 + i * 0x01 + iValue, SeekOrigin.Begin);
                uUIParameter.Work_BackgroundImage_Zoom[i] = binaryreader.ReadBoolean();
                filestream.Seek(0x150 + i * 0x04 + iValue, SeekOrigin.Begin);
                uUIParameter.Work_BackgroundImage_Location[i].X = binaryreader.ReadInt32();
                filestream.Seek(0x160 + i * 0x04 + iValue, SeekOrigin.Begin);
                uUIParameter.Work_BackgroundImage_Location[i].Y = binaryreader.ReadInt32();
            }

            filestream.Seek(0x170 + iValue, SeekOrigin.Begin);
            uUIParameter.Work_SpeedPhase_Value_FontSize = binaryreader.ReadSingle();
            filestream.Seek(0x180 + iValue, SeekOrigin.Begin);
            uUIParameter.Work_SpeedPhase_Value_Location.X = binaryreader.ReadInt32();
            filestream.Seek(0x190 + iValue, SeekOrigin.Begin);
            uUIParameter.Work_SpeedPhase_Value_Location.Y = binaryreader.ReadInt32();
            filestream.Seek(0x1A0 + iValue, SeekOrigin.Begin);
            uUIParameter.Work_SpeedPhase_Value_Size.Width = binaryreader.ReadInt32();
            filestream.Seek(0x1B0 + iValue, SeekOrigin.Begin);
            uUIParameter.Work_SpeedPhase_Value_Size.Height = binaryreader.ReadInt32();

            filestream.Seek(0x1C0 + iValue, SeekOrigin.Begin);
            uUIParameter.Work_SpeedPhase_Unit_FontSize = binaryreader.ReadSingle();
            filestream.Seek(0x1D0 + iValue, SeekOrigin.Begin);
            uUIParameter.Work_SpeedPhase_Unit_Location.X = binaryreader.ReadInt32();
            filestream.Seek(0x1E0 + iValue, SeekOrigin.Begin);
            uUIParameter.Work_SpeedPhase_Unit_Location.Y = binaryreader.ReadInt32();
            filestream.Seek(0x1F0 + iValue, SeekOrigin.Begin);
            uUIParameter.Work_SpeedPhase_Unit_Size.Width = binaryreader.ReadInt32();
            filestream.Seek(0x200 + iValue, SeekOrigin.Begin);
            uUIParameter.Work_SpeedPhase_Unit_Size.Height = binaryreader.ReadInt32();
            
            //

            filestream.Seek(0x410 + iValue, SeekOrigin.Begin);
            iSystemCameraNumber = binaryreader.ReadInt32();//相机配置信息，相机数量
            sSystemCameraConfiguration = new VisionSystemClassLibrary.Struct.CameraConfiguration[iSystemCameraNumber];//相机配置信息

            for (i = 0; i < iSystemCameraNumber; i++)//读取数值
            {
                filestream.Seek(0x420 + iValue + i * 0x160, SeekOrigin.Begin);
                sSystemCameraConfiguration[i].Type = (Enum.CameraType)binaryreader.ReadByte();//相机类型

                filestream.Seek(0x430 + iValue + i * 0x160, SeekOrigin.Begin);
                sSystemCameraConfiguration[i].Selected = binaryreader.ReadBoolean();//是否选中
                sSystemCameraConfiguration[i].CheckEnable = binaryreader.ReadBoolean();//检测是否使能
                sSystemCameraConfiguration[i].CameraAngle = (VisionSystemClassLibrary.Enum.CameraRotateAngle)binaryreader.ReadByte(); //读取相机旋转角度。0:0度;1:90度;2:180度;3:270度（控制器程序使用
                sSystemCameraConfiguration[i].VideoColor = (VisionSystemClassLibrary.Enum.VideoColor)binaryreader.ReadByte();//读取相机颜色
                sSystemCameraConfiguration[i].VideoResolution = (VisionSystemClassLibrary.Enum.VideoResolution)binaryreader.ReadByte();//读取相机分辨率
                sSystemCameraConfiguration[i].IsSerialPort = binaryreader.ReadBoolean();//读取是否为串口
                sSystemCameraConfiguration[i].TobaccoSortType_E = (VisionSystemClassLibrary.Enum.TobaccoSortType)binaryreader.ReadByte();//读取烟支排列类型
                sSystemCameraConfiguration[i].CameraFlip = (VisionSystemClassLibrary.Enum.CameraFlip)binaryreader.ReadByte();//读取镜像标记
                sSystemCameraConfiguration[i].Sensor_ProductType = (VisionSystemClassLibrary.Enum.SensorProductType)binaryreader.ReadByte();//读取传感器应用场景

                filestream.Seek(0x43E + iValue + i * 0x160, SeekOrigin.Begin);
                sSystemCameraConfiguration[i].BitmapLockBitsResize = binaryreader.ReadBoolean();//读取原始图像数据截取区域缩放标记
                sSystemCameraConfiguration[i].BitmapLockBitsCenter = binaryreader.ReadBoolean();//读取原始图像数据截取区域缩放后是否居中（控制器程序使用）

                filestream.Seek(0x440 + iValue + i * 0x160, SeekOrigin.Begin);
                sSystemCameraConfiguration[i].IPValue = binaryreader.ReadByte();//相机IP地址最后一位（绑定时使用）
                sSystemCameraConfiguration[i].IPAddress = sDeviceIPAddress + sSystemCameraConfiguration[i].IPValue.ToString();//相机IP地址（绑定时使用）

                filestream.Seek(0x450 + iValue + i * 0x160, SeekOrigin.Begin);
                sSystemCameraConfiguration[i].CameraENGName = binaryreader.ReadString();//相机英文名称

                filestream.Seek(0x490 + iValue + i * 0x160, SeekOrigin.Begin);
                sSystemCameraConfiguration[i].CameraCHNName = binaryreader.ReadString();//相机中文名称

                filestream.Seek(0x4D0 + iValue + i * 0x160, SeekOrigin.Begin);
                sSystemCameraConfiguration[i].ControllerENGName = binaryreader.ReadString();//控制器英文名称

                filestream.Seek(0x510 + iValue + i * 0x160, SeekOrigin.Begin);
                sSystemCameraConfiguration[i].ControllerCHNName = binaryreader.ReadString();//控制器中文名称

                filestream.Seek(0x550 + iValue + i * 0x160, SeekOrigin.Begin);
                sSystemCameraConfiguration[i].Port = binaryreader.ReadByte();//端口

                filestream.Seek(0x560 + iValue + i * 0x160, SeekOrigin.Begin);
                sSystemCameraConfiguration[i].CameraFaultState = binaryreader.ReadUInt64();//相机故障标记

                sSystemCameraConfiguration[i].BitmapLockBitsAxis = new Point();
                sSystemCameraConfiguration[i].BitmapLockBitsArea = new Rectangle();

                filestream.Seek(0x574 + iValue + i * 0x160, SeekOrigin.Begin);
                sSystemCameraConfiguration[i].BitmapLockBitsAxis.X = binaryreader.ReadInt16();//读取原始图像数据截取区域粘贴区域
                sSystemCameraConfiguration[i].BitmapLockBitsAxis.Y = binaryreader.ReadInt16();
                sSystemCameraConfiguration[i].BitmapLockBitsArea.X = binaryreader.ReadInt16();//读取原始图像数据截取区域
                sSystemCameraConfiguration[i].BitmapLockBitsArea.Y = binaryreader.ReadInt16();
                sSystemCameraConfiguration[i].BitmapLockBitsArea.Width = binaryreader.ReadInt16();
                sSystemCameraConfiguration[i].BitmapLockBitsArea.Height = binaryreader.ReadInt16();
            }

            binaryreader.Close();
            filestream.Close();

            for (i = 0; i < iSystemCameraNumber; i++)//读取数值
            {
                sSystemCameraConfiguration[i].RelevancyCameraInfo = new VisionSystemClassLibrary.Struct.RelevancyCameraInformation();
                sSystemCameraConfiguration[i].RelevancyCameraInfo.RelevancyCameraInfo = new Dictionary<Enum.CameraType, Byte>();
            }

            _ReadRelevancy();//读取关联信息

            //

            try
            {
                bDeviceBackground = new Bitmap[uUIParameter.Work_TotalPage];

                for (i = 0; i < uUIParameter.Work_TotalPage; i++)
                {
                    if (null == bDeviceBackground[i])//无效
                    {
                        bDeviceBackground[i] = new Bitmap(sConfigDataPath + "Camera_Background" + (i + 1).ToString() + ".png");//设备背景图像（WORK，LIVE）
                    }
                }

                bTrademark_1 = new Bitmap[2];
                bTrademark_2 = new Bitmap[2];

                for (i = 0; i < 2; i++)
                {
                    if (null == bTrademark_1[i])//无效
                    {
                        bTrademark_1[i] = new Bitmap(sConfigDataPath + "Company_1" + (i + 1).ToString() + ".png");//商标背景图像1（LOAD，REG）
                    }

                    if (null == bTrademark_2[i])//无效
                    {
                        bTrademark_2[i] = new Bitmap(sConfigDataPath + "Company_2" + (i + 1).ToString() + ".png");//商标背景图像2（WORK）
                    }
                }
            }
            catch (Exception ex)
            {
                //不执行操作
            }

            //

            switch (language)
            {
                case Enum.InterfaceLanguage.Chinese://中文
                    //
                    sProductName = sProductName_CHN;
                    //
                    break;
                case Enum.InterfaceLanguage.English://英文
                    //
                    sProductName = sProductName_ENG;
                    //
                    break;
                default://默认，中文
                    //
                    sProductName = sProductName_CHN;
                    //
                    break;
            }

            wWork.CurrentPage = 0;//初始化
            wWork.SelectedCameraType = Enum.CameraType.None;//初始化，当前选中的相机显示控件所对应的相机类型（取值为CameraType.None，表示未选择任何相机显示控件）
            wWork.SelectedCameraIndex = -1;//初始化，当前选中的相机显示控件所对应的相机在相机数组中的索引值（取值为-1，表示未选择任何相机显示控件）
            wWork.ConnectedCameraNumber = 0;//初始化，系统中连接的相机数量（0表示无连接的相机）

            //

            iValue = 0;//临时变量
            string[] sControllerName_ENG = new string[iSystemCameraNumber];//临时变量
            string[] sControllerName_CHN = new string[iSystemCameraNumber];//临时变量

            for (i = 0; i < iSystemCameraNumber; i++)//初始化
            {
                sControllerName_ENG[i] = "";
                sControllerName_CHN[i] = "";
            }

            for (i = 0; i < iSystemCameraNumber; i++)//获取数值
            {
                for (j = 0; j < iSystemCameraNumber; j++)//获取数值
                {
                    if (sControllerName_ENG[j] != sSystemCameraConfiguration[i].ControllerENGName)//不同
                    {
                        if ("" == sControllerName_ENG[j])//赋值
                        {
                            sControllerName_ENG[j] = sSystemCameraConfiguration[i].ControllerENGName;
                            sControllerName_CHN[j] = sSystemCameraConfiguration[i].ControllerCHNName;

                            //

                            iValue++;

                            break;
                        }
                        else//其它
                        {
                            //不执行操作
                        }
                    }
                    else//相同
                    {
                        break;
                    }
                }
            }

            sSystemControllerName_ENG = new string[iValue];
            sSystemControllerName_CHN = new string[iValue];
            Array.Copy(sControllerName_ENG, 0, sSystemControllerName_ENG, 0, iValue);
            Array.Copy(sControllerName_CHN, 0, sSystemControllerName_CHN, 0, iValue);

            //
            
            camera = new List<Camera>();//创建相机对象，赋初值
            for (i = 0; i < sSystemCameraConfiguration.Length; i++)//加载相机参数
            {
                if (sSystemCameraConfiguration[i].Selected)//选择
                {
                    Camera cameraTemp = new Camera(sConfigDataPath,
                        sSystemCameraConfiguration[i].Type,
                        sSystemCameraConfiguration[i].Port,
                        sSystemCameraConfiguration[i].CameraFaultState,
                        sSystemCameraConfiguration[i].CameraCHNName,
                        sSystemCameraConfiguration[i].CameraENGName,
                        sSystemCameraConfiguration[i].ControllerCHNName,
                        sSystemCameraConfiguration[i].ControllerENGName,
                        sSystemCameraConfiguration[i].BitmapLockBitsResize,
                        sSystemCameraConfiguration[i].BitmapLockBitsCenter,
                        sSystemCameraConfiguration[i].BitmapLockBitsArea,
                        sSystemCameraConfiguration[i].CameraAngle,
                        sSystemCameraConfiguration[i].CheckEnable,
                        sSystemCameraConfiguration[i].BitmapLockBitsAxis,
                        sSystemCameraConfiguration[i].IsSerialPort,
                        sSystemCameraConfiguration[i].TobaccoSortType_E,
                        sSystemCameraConfiguration[i].VideoColor,
                        sSystemCameraConfiguration[i].VideoResolution,
                        sSystemCameraConfiguration[i].CameraFlip,
                        sSystemCameraConfiguration[i].RelevancyCameraInfo,
                        sSystemCameraConfiguration[i].Sensor_ProductType);//创建对象，读取文件数据

                    //

                    camera.Add(cameraTemp);
                }
            }

            brand = new Brand(sApplicationPath);//创建对象，读取文件数据

            shift = new Shift(sApplicationPath);
        }

        //属性

        //----------------------------------------------------------------------
        // 功能说明：FaultMessage_CHN属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public static string[] FaultMessage_CHN
        {
            get
            {
                return sFaultMessage_CHN;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：FaultMessage_ENG属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public static string[] FaultMessage_ENG
        {
            get
            {
                return sFaultMessage_ENG;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：MachineFaultEnableState属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public static UInt64 MachineFaultEnableState
        {
            get
            {
                return uiMachineFaultEnableState;
            }
            set
            {
                uiMachineFaultEnableState = value;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：TitlebarStyle属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Boolean TitlebarStyle
        {
            get
            {
                return bTitlebarStyle;
            }
            set
            {
                bTitlebarStyle = value;
            }
        }

        // 功能说明：MachineTypeNumber属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Int32 MachineTypeNumber
        {
            get
            {
                return iMachineTypeNumber;
            }
            set
            {
                iMachineTypeNumber = value;
            }
        }

        // 功能说明：SystemCameraNumber属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Int32 SystemCameraNumber
        {
            get
            {
                return iSystemCameraNumber;
            }
            set
            {
                iSystemCameraNumber = value;
            }
        }
        
        //----------------------------------------------------------------------
        // 功能说明：IOSignalData属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Struct.IOSignal IOSignalData
        {
            get
            {
                return ioIOSignalData;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：Data_Value属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Int32 Data_Value
        {
            get
            {
                return iData_Value;
            }
            set
            {
                iData_Value = value;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：UserPassword属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public string UserPassword
        {
            get
            {
                return sUserPassword;
            }
            set
            {
                sUserPassword = value;
            }
        }
        
        //----------------------------------------------------------------------
        // 功能说明：SelectedMachineType属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Int32 SelectedMachineType
        {
            get
            {
                return iSelectedMachineType;
            }
            set
            {
                iSelectedMachineType = value;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：MachineType属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public string[] MachineType
        {
            get
            {
                return sMachineType;
            }
            set
            {
                sMachineType = value;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ConnectionData属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Struct.DeviceData[] ConnectionData
        {
            get
            {
                return dConnectionData;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：DeviceIPAddress属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public static string DeviceIPAddress
        {
            get
            {
                return sDeviceIPAddress;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ProductModelNumber属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public string ProductModelNumber
        {
            get
            {
                return sProductModelNumber;
            }
            set
            {
                sProductModelNumber = value;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ProductName属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public string ProductName
        {
            get
            {
                return sProductName;
            }
            set
            {
                sProductName = value;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ProductName_CHN属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public string ProductName_CHN
        {
            get
            {
                return sProductName_CHN;
            }
            set
            {
                sProductName_CHN = value;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ProductName_ENG属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public string ProductName_ENG
        {
            get
            {
                return sProductName_ENG;
            }
            set
            {
                sProductName_ENG = value;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：Work属性(相机配置信息)
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Struct.WorkData Work
        {
            get
            {
                return wWork;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：UIParameter属性(相机配置信息)
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Struct.System_UIParameter UIParameter
        {
            get
            {
                return uUIParameter;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：SystemCameraConfiguration属性(相机配置信息)
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Struct.CameraConfiguration[] SystemCameraConfiguration
        {
            get
            {
                return sSystemCameraConfiguration;
            }
            set
            {
                sSystemCameraConfiguration = value;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：SystemControllerName_ENG属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public string[] SystemControllerName_ENG
        {
            get
            {
                return sSystemControllerName_ENG;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：SystemControllerName_CHN属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public string[] SystemControllerName_CHN
        {
            get
            {
                return sSystemControllerName_CHN;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：SystemDeviceState属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public static Enum.DeviceState SystemDeviceState
        {
            get
            {
                return systemDeviceState;
            }
            set
            {
                systemDeviceState = value;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：Language属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public static Enum.InterfaceLanguage Language
        {
            get
            {
                return language;
            }
            set
            {
                language = value;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：DeviceBackground属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public static Bitmap[] DeviceBackground
        {
            get
            {
                return bDeviceBackground;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：Trademark_1属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public static Bitmap[] Trademark_1
        {
            get
            {
                return bTrademark_1;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：Trademark_2属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public static Bitmap[] Trademark_2
        {
            get
            {
                return bTrademark_2;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ConfigDataPathName属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public static string ConfigDataPathName
        {
            get
            {
                return sConfigDataPathName;
            }
            set
            {
                sConfigDataPathName = value;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：RelevancyFileName属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public static string RelevancyFileName
        {
            get
            {
                return sRelevancyFileName;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：StatisticsPathName属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public static string StatisticsPathName
        {
            get
            {
                return sStatisticsPathName;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ConfigDataPath属性，配置文件路径
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public string ConfigDataPath
        {
            get
            {
                return sConfigDataPath;
            }
            set
            {
                sConfigDataPath = value;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：Camera属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public List<Camera> Camera
        {
            get//读取
            {
                return camera;
            }
            set//设置
            {
                camera = value;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：Brand属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Brand Brand
        {
            get//读取
            {
                return brand;
            }
            set//设置
            {
                brand = value;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：Shift属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Shift Shift
        {
            get//读取
            {
                return shift;
            }
            set//设置
            {
                shift = value;
            }
        }

        //函数

        //-----------------------------------------------------------------------
        // 功能说明：设置设备名称（UI，DEVICE）
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _SetLanguage_Device()
        {
            for (Int32 i = 0; i < dConnectionData.Length; i++)//赋初值
            {
                dConnectionData[i].DeviceName = _GetCameraName(dConnectionData[i].Type);//设备（相机）名称
                dConnectionData[i].ControllerName = _GetControllerName(dConnectionData[i].Type);//控制器名称
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：拷贝系统参数（UI，SYSTEM）
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _CopySystemParameterTo(System system_parameter)//17.02
        {
            system_parameter.sMachineType = new string[sMachineType.Length];
            sMachineType.CopyTo(system_parameter.sMachineType, 0);//机器类型名称
            system_parameter.iSelectedMachineType = iSelectedMachineType;//系统设置，当前选择的机器类型（从0开始）

            system_parameter.sSystemCameraConfiguration = new VisionSystemClassLibrary.Struct.CameraConfiguration[sSystemCameraConfiguration.Length];
            sSystemCameraConfiguration.CopyTo(system_parameter.sSystemCameraConfiguration, 0);//相机配置信息

            system_parameter.sUserPassword = sUserPassword;//用户密码
        }


        //-----------------------------------------------------------------------
        // 功能说明：检查系统参数是否被修改（UI，SYSTEM）
        // 输入参数：1.device：设备
        //         2.language：语言
        // 输出参数：无
        // 返 回 值：系统参数是否被修改。取值范围：true，是；false，否
        //----------------------------------------------------------------------
        public Boolean _CheckSystemParameter(System system_parameter, VisionSystemClassLibrary.Enum.InterfaceLanguage lLanguage)
        {
            Int32 i = 0;//循环控制变量
            Int32 j = 0;//循环控制变量
            Int32 k = 0;//循环控制变量

            if (iSelectedMachineType != system_parameter.iSelectedMachineType)//修改
            {
                return true;
            }

            for (i = 0; i < sSystemCameraConfiguration.Length; i++)//
            {
                if (sSystemCameraConfiguration[i].Selected != system_parameter.sSystemCameraConfiguration[i].Selected)//修改
                {
                    break;
                }
            }

            for (j = 0; j < sSystemCameraConfiguration.Length; j++)//
            {
                if (sSystemCameraConfiguration[j].Selected)//选择
                {
                    if (sSystemCameraConfiguration[j].CheckEnable != system_parameter.sSystemCameraConfiguration[j].CheckEnable)//修改
                    {
                        break;
                    }
                }
            }

            for (k = 0; k < sSystemCameraConfiguration.Length; k++)//
            {
                if (sSystemCameraConfiguration[k].IsSerialPort)//当前为串口
                {
                    if (sSystemCameraConfiguration[k].TobaccoSortType_E != system_parameter.sSystemCameraConfiguration[k].TobaccoSortType_E)//修改
                    {
                        break;
                    }
                }
            }

            if ((i < sSystemCameraConfiguration.Length) || (j < sSystemCameraConfiguration.Length) || (k < sSystemCameraConfiguration.Length))//修改
            {
                return true;
            }

            if (language != lLanguage)//修改
            {
                return true;
            }

            if (sUserPassword != system_parameter.sUserPassword)//修改
            {
                return true;
            }

            return false;
        }

        //-----------------------------------------------------------------------
        // 功能说明：保存系统参数（UI，SYSTEM）
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _WriteSystemParameter()
        {
            Int32 i = 0;//循环控制变量
            Int32 iValue = 0;//临时变量

            FileStream filestream = new FileStream(sConfigDataPath + System.sSystemFileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, 8192, FileOptions.WriteThrough);//打开系统文件
            BinaryWriter binarywriter = new BinaryWriter(filestream);//写入系统文件数据

            filestream.Seek(0x00, SeekOrigin.Begin);
            binarywriter.Write(Convert.ToByte(language));//语言

            iValue = sMachineType.Length * 0x40;

            filestream.Seek(0xA0 + iValue, SeekOrigin.Begin);
            binarywriter.Write(iSelectedMachineType);//系统设置，当前选择的机器类型（从0开始）

            filestream.Seek(0xB0 + iValue, SeekOrigin.Begin);
            binarywriter.Write(sUserPassword);//用户密码

            for (i = 0; i < sSystemCameraConfiguration.Length; i++)//
            {
                filestream.Seek(0x430 + iValue + i * 0x160, SeekOrigin.Begin);
                binarywriter.Write(sSystemCameraConfiguration[i].Selected);//是否选中
                binarywriter.Write(sSystemCameraConfiguration[i].CheckEnable);//检测使能标记

                filestream.Seek(0x436 + iValue + i * 0x160, SeekOrigin.Begin);
                binarywriter.Write((Byte)sSystemCameraConfiguration[i].TobaccoSortType_E);//写入烟支排列类型
            }

            binarywriter.Close();
            filestream.Close();
        }

        //-----------------------------------------------------------------------
        // 功能说明：获取产品名称（UI）
        // 输入参数：1.language_parameter：语言
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public string _GetProductName(Enum.InterfaceLanguage language_parameter)
        {
            string sReturn = "";//返回值

            //

            switch (language_parameter)
            {
                case Enum.InterfaceLanguage.Chinese://中文
                    //
                    sReturn = sProductName_CHN;
                    //
                    break;
                case Enum.InterfaceLanguage.English://英文
                    //
                    sReturn = sProductName_ENG;
                    //
                    break;
                default://默认，中文
                    //
                    sReturn = sProductName_CHN;
                    //
                    break;
            }

            //

            return sReturn;
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：获取故障信
        // 输入参数：1、Enum.InterfaceLanguage：language，语言
        //           2、Struct.FaultMessage：faultmessage，故障索引值
        // 输出参数：无
        // 返回值：string，返回故障信息中英文名称
        //----------------------------------------------------------------------
        public static string _GetFaultMessage(Enum.InterfaceLanguage language, Struct.FaultMessage faultmessage)
        {
            string faultStaticsName = "";

            switch (language)
            {
                case Enum.InterfaceLanguage.Chinese:
                    faultStaticsName = sFaultMessage_CHN[faultmessage.DataIndex];
                    break;
                case Enum.InterfaceLanguage.English:
                    faultStaticsName = sFaultMessage_ENG[faultmessage.DataIndex];
                    break;
                default:
                    break;
            }
            return faultStaticsName;
        }
        
        //----------------------------------------------------------------------
        // 功能说明：获取故障信息使能状态数组
        // 输入参数：1、UInt64：machineFaultEnableState，故障信息使能状态
        // 输出参数：无
        // 返回值：Boolean[]，返回故障信息使能状态数组
        //----------------------------------------------------------------------
        public static Boolean[] _GetMachineFaultEnableStateArray(UInt64 machineFaultEnableState)
        {
            Boolean[] machineFaultEnableStateArray = new Boolean[64];

            for (Int32 i = 0; i < 64; i++)//查询故障信息
            {
                if (((machineFaultEnableState >> i) & 1) != 0)//当前故障使能打开
                {
                    machineFaultEnableStateArray[i] = true;
                }
                else  //当前故障使能关闭
                {
                    machineFaultEnableStateArray[i] = false;
                }
            }
            return machineFaultEnableStateArray;
        }

        //----------------------------------------------------------------------
        // 功能说明：获取故障信息使能状态
        // 输入参数：1、Boolean[]：machineFaultEnableStateArray，故障信息使能状态数组
        // 输出参数：无
        // 返回值：UInt64，返回故障信息使能状态
        //----------------------------------------------------------------------
        public static UInt64 _GetMachineFaultEnableState(Boolean[] machineFaultEnableStateArray)
        {
            UInt64 machineFaultEnableState = 0;

            for (Int32 i = 0; i < machineFaultEnableStateArray.Length; i++)//查询故障信息
            {
                if (machineFaultEnableStateArray[i])//当前故障使能打开
                {
                    machineFaultEnableState = machineFaultEnableState | ((UInt64)1 << i);
                }
                else  //当前故障使能关闭
                {
                    machineFaultEnableState = machineFaultEnableState & (~((UInt64)1 << i));
                }
            }
            return machineFaultEnableState;
        }

        //----------------------------------------------------------------------
        // 功能说明：读取机器状态信息
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public static void _ReadMachineStateInfoFile(string sPath = "")//
        {
            FileStream filestream = null;
            BinaryReader binaryReader = null;

            try
            {
                filestream = new FileStream(sPath + ConfigDataPathName + "MachineStateInfo.dat", FileMode.Open);//打开机器状态信息文件
                binaryReader = new BinaryReader(filestream);

                systemDeviceState = (VisionSystemClassLibrary.Enum.DeviceState)(binaryReader.ReadByte());//读取机器状态
                uiMachineFaultEnableState = binaryReader.ReadUInt64();//读取故障信息使能状态

                binaryReader.Close();//关闭机器状态信息文件
                filestream.Close();
            }
            catch (Exception ex)
            {
                //
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

        //----------------------------------------------------------------------
        // 功能说明：读取故障信息使能状态
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private static void _ReadFaultInfoFile(string sPath = "")//
        {
            FileStream filestream = null;
            StreamReader streamReader = null;

            try
            {
                filestream = new FileStream(sPath + ConfigDataPathName + "FaultInfo.dat", FileMode.Open);//
                streamReader = new StreamReader(filestream);

                Int32 faultInfoNumber = Convert.ToInt32(streamReader.ReadLine());//读入故障信息总数目

                faultInfoNumber++;
                sFaultMessage_CHN = new string[faultInfoNumber];
                sFaultMessage_ENG = new string[faultInfoNumber];

                for (Int32 i = 1; i < faultInfoNumber; i++)  //读取所有故障信息中文名称       
                {
                    sFaultMessage_CHN[i] = streamReader.ReadLine();
                }

                for (Int32 i = 1; i < faultInfoNumber; i++)  //读取所有故障信息英文名称       
                {
                    sFaultMessage_ENG[i] = streamReader.ReadLine();
                }
                streamReader.Close();//关闭故障信息文件
                filestream.Close();
            }
            catch (Exception ex)
            {

            }

            if (null != streamReader)
            {
                streamReader.Close();
            }

            if (null != filestream)
            {
                filestream.Close();
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：写入故障信息使能状态
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public static void _WriteMachineStateInfoFile(string sPath = "")//
        {
            FileStream filestream = new FileStream(sPath + ConfigDataPathName + "MachineStateInfo.dat", FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, 8192, FileOptions.WriteThrough); //打开机器状态信息文件
            BinaryWriter binarywriter = new BinaryWriter(filestream);//写入系统文件数据

            binarywriter.Write((Byte)systemDeviceState);//保存机器状态
            binarywriter.Write((UInt64)uiMachineFaultEnableState);//保存故障信息使能状态

            binarywriter.Close();//关闭机器状态信息文件
            filestream.Close();
        }
        
        //

        //-----------------------------------------------------------------------
        // 功能说明：获取系统相机数量
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        private Int32 _GetSystemCameraNumber()//
        {
            VisionSystemClassLibrary.Enum.CameraType cameratype = VisionSystemClassLibrary.Enum.CameraType.None;//相机类型

            FieldInfo[] fieldinfo = cameratype.GetType().GetFields();//获取字段信息

            return (fieldinfo.Length - 2);//系统相机数量（出去数组[0]和[1]）
        }

        //-----------------------------------------------------------------------
        // 功能说明：获取控制器名称
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        private string _GetControllerName(Enum.CameraType type)
        {
            string sReturn = "";//返回值

            for (Int32 i = 0; i < sSystemCameraConfiguration.Length; i++)//
            {
                if (type == sSystemCameraConfiguration[i].Type)//相同
                {
                    switch (language)
                    {
                        case Enum.InterfaceLanguage.Chinese:

                            sReturn = sSystemCameraConfiguration[i].ControllerCHNName;

                            break;
                        case Enum.InterfaceLanguage.English:

                            sReturn = sSystemCameraConfiguration[i].ControllerENGName;

                            break;
                        default:

                            sReturn = sSystemCameraConfiguration[i].ControllerCHNName;

                            break;
                    }
                }
            }

            return sReturn;
        }

        //-----------------------------------------------------------------------
        // 功能说明：获取相机名称
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        private string _GetCameraName(Enum.CameraType type)
        {
            string sReturn = "";//返回值

            for (Int32 i = 0; i < sSystemCameraConfiguration.Length; i++)//
            {
                if (type == sSystemCameraConfiguration[i].Type)//相同
                {
                    switch (language)
                    {
                        case Enum.InterfaceLanguage.Chinese:

                            sReturn = sSystemCameraConfiguration[i].CameraCHNName;

                            break;
                        case Enum.InterfaceLanguage.English:

                            sReturn = sSystemCameraConfiguration[i].CameraENGName;

                            break;
                        default:

                            sReturn = sSystemCameraConfiguration[i].CameraCHNName;

                            break;
                    }
                }
            }

            return sReturn;
        }

        //-----------------------------------------------------------------------
        // 功能说明：获取系统使用的相机数量
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：系统使用的相机数量
        //----------------------------------------------------------------------
        private Int32 _GetCameraNumber()
        {
            Int32 iReturn = 0;//循环控制变量

            Int32 i = 0;//循环控制变量

            for (i = 0; i < sSystemCameraConfiguration.Length; i++)//相机参数
            {
                if (sSystemCameraConfiguration[i].Selected)//选择
                {
                    iReturn++;
                }
            }

            return iReturn;
        }

        //-----------------------------------------------------------------------
        // 功能说明：设置默认设备信息
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        private void _SetDefaultDeviceData()
        {
            for (Int32 i = 0; i < dConnectionData.Length; i++)//赋初值
            {
                dConnectionData[i] = new VisionSystemClassLibrary.Struct.DeviceData();

                //赋初值

                dConnectionData[i].Connected = false;//是否与客户端建立了连接。取值范围：true，是；false，否
                dConnectionData[i].GetDevInfo = false;//是否存储了客户端的设备信息。取值范围：true，是；false，否
                dConnectionData[i].SerialNumber = "";//序列号
                dConnectionData[i].MAC = "";//MAC地址
                dConnectionData[i].IP = "";//IP地址
                dConnectionData[i].Firmware = "";//固件版本
                dConnectionData[i].DeviceName = "";//设备（相机）名称
                dConnectionData[i].ControllerName = "";//控制器名称
                dConnectionData[i].Type = Enum.CameraType.None;//相机类型
                dConnectionData[i].CAM = Enum.CameraState.NOTCONNECTED;//相机状态
                dConnectionData[i].Port = 1;//相机端口
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：设置默认设备信息
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        private void _SetDefaultSystemCameraConfiguration()
        {
            Int32 iSystemCameraNumber = _GetSystemCameraNumber();//

            sSystemCameraConfiguration = new VisionSystemClassLibrary.Struct.CameraConfiguration[iSystemCameraNumber];//相机配置信息

            sSystemControllerName_ENG = new string[iSystemCameraNumber];//控制器英文名称
            sSystemControllerName_CHN = new string[iSystemCameraNumber];//控制器中文名称

            for (Int32 i = 0; i < sSystemCameraConfiguration.Length; i++)//赋初值
            {
                sSystemCameraConfiguration[i].Type = (VisionSystemClassLibrary.Enum.CameraType)(i + 1);//相机类型
                sSystemCameraConfiguration[i].Selected = true;//是否被选择。取值范围：true，是；false，否
                sSystemCameraConfiguration[i].IPValue = (Byte)(i + 2);
                sSystemCameraConfiguration[i].IPAddress = sDeviceIPAddress + sSystemCameraConfiguration[i].IPValue.ToString();

                sSystemCameraConfiguration[i].CameraENGName = "CAM." + (i + 1).ToString();//相机英文名
                sSystemCameraConfiguration[i].CameraCHNName = "相机" + (i + 1).ToString();//相机中文名

                sSystemCameraConfiguration[i].ControllerENGName = "Ctrl." + (i + 1).ToString();//控制器英文名
                sSystemCameraConfiguration[i].ControllerCHNName = "控制器" + (i + 1).ToString();//控制器中文名

                sSystemCameraConfiguration[i].Port = 1;//端口

                sSystemCameraConfiguration[i].CameraFaultState = 0;//相机故障标记

                sSystemCameraConfiguration[i].CheckEnable = true;

                sSystemCameraConfiguration[i].CameraAngle = 0;

                sSystemCameraConfiguration[i].BitmapLockBitsResize = false;
                sSystemCameraConfiguration[i].BitmapLockBitsCenter = false;
                sSystemCameraConfiguration[i].BitmapLockBitsAxis = new Point();
                sSystemCameraConfiguration[i].BitmapLockBitsArea = new Rectangle(0, 0, 744, 480);

                sSystemCameraConfiguration[i].IsSerialPort = false;
                sSystemCameraConfiguration[i].TobaccoSortType_E = 0;

                sSystemCameraConfiguration[i].VideoColor = VisionSystemClassLibrary.Enum.VideoColor.RGB32;
                sSystemCameraConfiguration[i].VideoResolution = VisionSystemClassLibrary.Enum.VideoResolution._744x480;
                
                sSystemCameraConfiguration[i].CameraFlip = 0;
                
                sSystemCameraConfiguration[i].RelevancyCameraInfo = new Struct.RelevancyCameraInformation();
                sSystemCameraConfiguration[i].RelevancyCameraInfo.RelevancyCameraInfo = new Dictionary<Enum.CameraType, Byte>();

                //

                sSystemControllerName_ENG[i] = "";//控制器英文名称
                sSystemControllerName_CHN[i] = "";//控制器中文名称


            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：设置默认设备信息
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        private void _SetDefaultMachineType()
        {
            Int32 i = 0;

            for (i = 0; i < sMachineType.Length; i++)
            {
                sMachineType[i] = "MACHINE_" + (i + 1).ToString();
            }

            iSelectedMachineType = 0;
        }

        //----------------------------------------------------------------------
        // 功能说明：初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _Init()//
        {
            _SetDefaultDeviceData();//设置默认设备信息
            _SetDefaultSystemCameraConfiguration();//设置默认设备信息
            _SetDefaultMachineType();//设置默认机器信息

            //

            camera = new List<Camera>();
        }

        //----------------------------------------------------------------------
        // 功能说明：保存函数，提供配置工具使用
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _ReadRelevancy()
        {
            FileStream filestream = null;
            BinaryReader binaryReader = null;

            try
            {
                filestream = new FileStream(sConfigDataPath + RelevancyFileName, FileMode.Open);
                binaryReader = new BinaryReader(filestream);

                for (int i = 0; i < iSystemCameraNumber; i++)//保存数值
                {
                    filestream.Seek(0x001 + i * 0x210, SeekOrigin.Begin);
                    sSystemCameraConfiguration[i].RelevancyCameraInfo.rRelevancyType = (VisionSystemClassLibrary.Enum.RelevancyType)binaryReader.ReadByte();//读取关联

                    if (VisionSystemClassLibrary.Enum.RelevancyType.None < sSystemCameraConfiguration[i].RelevancyCameraInfo.rRelevancyType) //存在关联相机
                    {
                        filestream.Seek(0x002 + i * 0x210, SeekOrigin.Begin);
                        Int32 iCount = binaryReader.ReadByte();//关联个数

                        filestream.Seek(0x010 + i * 0x210, SeekOrigin.Begin);
                        for (Int32 j = 0; j < iCount; j++)
                        {
                            VisionSystemClassLibrary.Enum.CameraType relevancyCameraType = (VisionSystemClassLibrary.Enum.CameraType)binaryReader.ReadByte();
                            sSystemCameraConfiguration[i].RelevancyCameraInfo.RelevancyCameraInfo.Add(relevancyCameraType, binaryReader.ReadByte());
                        }
                    }
                }
                binaryReader.Close();
                filestream.Close();
            }
            catch (Exception ex)
            {
                for (int i = 0; i < iSystemCameraNumber; i++)//保存数值
                {
                    sSystemCameraConfiguration[i].RelevancyCameraInfo.rRelevancyType = Enum.RelevancyType.None;//读取关联
                }
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

        //----------------------------------------------------------------------
        // 功能说明：保存函数，提供配置工具使用
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SaveRelevancy()
        {
            FileStream filestream = new FileStream(sConfigDataPath + RelevancyFileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, 8192, FileOptions.WriteThrough);//打开系统文件
            BinaryWriter binarywriter = new BinaryWriter(filestream);//保存系统文件数据

            for (int i = 0; i < iSystemCameraNumber; i++)//保存数值
            {
                filestream.Seek(0x000 + i * 0x210, SeekOrigin.Begin);
                binarywriter.Write((Byte)sSystemCameraConfiguration[i].Type);//保存相机类型
                binarywriter.Write((Byte)sSystemCameraConfiguration[i].RelevancyCameraInfo.rRelevancyType);//关联类型

                if (VisionSystemClassLibrary.Enum.RelevancyType.None < sSystemCameraConfiguration[i].RelevancyCameraInfo.rRelevancyType) //存在关联相机
                {
                    filestream.Seek(0x002 + i * 0x210, SeekOrigin.Begin);
                    binarywriter.Write((Byte)sSystemCameraConfiguration[i].RelevancyCameraInfo.RelevancyCameraInfo.Count);//关联数量

                    filestream.Seek(0x010 + i * 0x210, SeekOrigin.Begin);
                    for (Byte j = 0; j < sSystemCameraConfiguration[i].RelevancyCameraInfo.RelevancyCameraInfo.Count; j++)
                    {
                        binarywriter.Write((Byte)sSystemCameraConfiguration[i].RelevancyCameraInfo.RelevancyCameraInfo.ElementAt(j).Key);//关联相机类型
                        binarywriter.Write((Byte)sSystemCameraConfiguration[i].RelevancyCameraInfo.RelevancyCameraInfo.ElementAt(j).Value);//关联相机类型
                    }
                }
            }
            binarywriter.Close();
            filestream.Close();
        }

        //----------------------------------------------------------------------
        // 功能说明：保存函数，提供配置工具使用
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _Save()
        {
            FileStream filestream = new FileStream(sConfigDataPath + sSystemFileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, 8192, FileOptions.WriteThrough);//打开系统文件
            BinaryWriter binarywriter = new BinaryWriter(filestream);//保存系统文件数据

            filestream.Seek(0x00, SeekOrigin.Begin);
            binarywriter.Write((Byte)language);//保存语言
            binarywriter.Write(bTitlebarStyle);//保存标题栏风格
            binarywriter.Write(iData_Value);

            filestream.Seek(0x10, SeekOrigin.Begin);
            binarywriter.Write(sProductName_ENG);//保存产品名称（英文）

            filestream.Seek(0x50, SeekOrigin.Begin);
            binarywriter.Write(sProductName_CHN);//保存产品名称（中文）

            filestream.Seek(0x90, SeekOrigin.Begin);
            binarywriter.Write(iMachineTypeNumber);//保存系统设置，机器类型数量

            for (int i = 0; i < sMachineType.Length; i++)
            {
                filestream.Seek(0xA0 + i * 0x40, SeekOrigin.Begin);
                binarywriter.Write(sMachineType[i]);
            }

            Int32 iValue = iMachineTypeNumber * 0x40;

            filestream.Seek(0xA0 + iValue, SeekOrigin.Begin);
            binarywriter.Write(iSelectedMachineType);//保存系统设置，当前选择的机器类型（从0开始）

            filestream.Seek(0xB0 + iValue, SeekOrigin.Begin);
            binarywriter.Write(sUserPassword);//保存用户密码

            filestream.Seek(0xF0 + iValue, SeekOrigin.Begin);
            binarywriter.Write(sProductModelNumber);//保存产品型号

            filestream.Seek(0x130 + iValue, SeekOrigin.Begin);
            binarywriter.Write(uUIParameter.Work_TotalPage);

            for (Byte i = 0; i < uUIParameter.Work_TotalPage; i++)
            {
                filestream.Seek(0x140 + i * 0x01 + iValue, SeekOrigin.Begin);
                binarywriter.Write(uUIParameter.Work_BackgroundImage_Zoom[i]);
                filestream.Seek(0x150 + i * 0x04 + iValue, SeekOrigin.Begin);
                binarywriter.Write(uUIParameter.Work_BackgroundImage_Location[i].X);
                filestream.Seek(0x160 + i * 0x04 + iValue, SeekOrigin.Begin);
                binarywriter.Write(uUIParameter.Work_BackgroundImage_Location[i].Y);
            }

            filestream.Seek(0x170 + iValue, SeekOrigin.Begin);
            binarywriter.Write(uUIParameter.Work_SpeedPhase_Value_FontSize);
            filestream.Seek(0x180 + iValue, SeekOrigin.Begin);
            binarywriter.Write(uUIParameter.Work_SpeedPhase_Value_Location.X);
            filestream.Seek(0x190 + iValue, SeekOrigin.Begin);
            binarywriter.Write(uUIParameter.Work_SpeedPhase_Value_Location.Y);
            filestream.Seek(0x1A0 + iValue, SeekOrigin.Begin);
            binarywriter.Write(uUIParameter.Work_SpeedPhase_Value_Size.Width);
            filestream.Seek(0x1B0 + iValue, SeekOrigin.Begin);
            binarywriter.Write(uUIParameter.Work_SpeedPhase_Value_Size.Height);

            filestream.Seek(0x1C0 + iValue, SeekOrigin.Begin);
            binarywriter.Write(uUIParameter.Work_SpeedPhase_Unit_FontSize);
            filestream.Seek(0x1D0 + iValue, SeekOrigin.Begin);
            binarywriter.Write(uUIParameter.Work_SpeedPhase_Unit_Location.X);
            filestream.Seek(0x1E0 + iValue, SeekOrigin.Begin);
            binarywriter.Write(uUIParameter.Work_SpeedPhase_Unit_Location.Y);
            filestream.Seek(0x1F0 + iValue, SeekOrigin.Begin);
            binarywriter.Write(uUIParameter.Work_SpeedPhase_Unit_Size.Width);
            filestream.Seek(0x200 + iValue, SeekOrigin.Begin);
            binarywriter.Write(uUIParameter.Work_SpeedPhase_Unit_Size.Height);

            //

            filestream.Seek(0x410 + iValue, SeekOrigin.Begin);
            binarywriter.Write(iSystemCameraNumber);//保存相机配置信息，相机数量

            for (int i = 0; i < iSystemCameraNumber; i++)//保存数值
            {
                filestream.Seek(0x420 + iValue + i * 0x160, SeekOrigin.Begin);
                binarywriter.Write((Byte)sSystemCameraConfiguration[i].Type);//保存相机类型

                filestream.Seek(0x430 + iValue + i * 0x160, SeekOrigin.Begin);
                binarywriter.Write(sSystemCameraConfiguration[i].Selected);//是否选中
                binarywriter.Write(sSystemCameraConfiguration[i].CheckEnable);//检测使能标记
                binarywriter.Write((Byte)sSystemCameraConfiguration[i].CameraAngle); //写入相机旋转角度。0:0度;1:90度;2:180度;3:270度（控制器程序使用）
                binarywriter.Write((Byte)sSystemCameraConfiguration[i].VideoColor);//写入相机颜色
                binarywriter.Write((Byte)sSystemCameraConfiguration[i].VideoResolution);//写入相机分辨率
                binarywriter.Write(sSystemCameraConfiguration[i].IsSerialPort);//写入是否为串口
                binarywriter.Write((Byte)sSystemCameraConfiguration[i].TobaccoSortType_E);//写入烟支排列类型
                binarywriter.Write((Byte)sSystemCameraConfiguration[i].CameraFlip);//写入镜像标记
                binarywriter.Write((Byte)sSystemCameraConfiguration[i].Sensor_ProductType);//写入传感器应用场景

                filestream.Seek(0x43E + iValue + i * 0x160, SeekOrigin.Begin);
                binarywriter.Write(sSystemCameraConfiguration[i].BitmapLockBitsResize);//写入原始图像数据截取区域缩放标记
                binarywriter.Write(sSystemCameraConfiguration[i].BitmapLockBitsCenter);//写入原始图像数据截取区域缩放后是否居中（控制器程序使用）


                filestream.Seek(0x440 + iValue + i * 0x160, SeekOrigin.Begin);
                binarywriter.Write(sSystemCameraConfiguration[i].IPValue);//保存机IP地址最后一位（绑定时使用）
                binarywriter.Write(sSystemCameraConfiguration[i].IPAddress);//保存相机IP地址（绑定时使用）

                filestream.Seek(0x450 + iValue + i * 0x160, SeekOrigin.Begin);
                binarywriter.Write(sSystemCameraConfiguration[i].CameraENGName);//保存相机英文名称

                filestream.Seek(0x490 + iValue + i * 0x160, SeekOrigin.Begin);
                binarywriter.Write(sSystemCameraConfiguration[i].CameraCHNName);//保存相机中文名称

                filestream.Seek(0x4D0 + iValue + i * 0x160, SeekOrigin.Begin);
                binarywriter.Write(sSystemCameraConfiguration[i].ControllerENGName);//保存控制器英文名称

                filestream.Seek(0x510 + iValue + i * 0x160, SeekOrigin.Begin);
                binarywriter.Write(sSystemCameraConfiguration[i].ControllerCHNName);//保存控制器中文名称

                filestream.Seek(0x550 + iValue + i * 0x160, SeekOrigin.Begin);
                binarywriter.Write(sSystemCameraConfiguration[i].Port);//保存端口

                filestream.Seek(0x560 + iValue + i * 0x160, SeekOrigin.Begin);
                binarywriter.Write(sSystemCameraConfiguration[i].CameraFaultState);//保存相机故障标记

                filestream.Seek(0x574 + iValue + i * 0x160, SeekOrigin.Begin);
                binarywriter.Write(Convert.ToInt16(sSystemCameraConfiguration[i].BitmapLockBitsAxis.X));//写入原始图像数据截取区域粘贴区域
                binarywriter.Write(Convert.ToInt16(sSystemCameraConfiguration[i].BitmapLockBitsAxis.Y));
                binarywriter.Write(Convert.ToInt16(sSystemCameraConfiguration[i].BitmapLockBitsArea.X));//写入原始图像数据截取区域
                binarywriter.Write(Convert.ToInt16(sSystemCameraConfiguration[i].BitmapLockBitsArea.Y));
                binarywriter.Write(Convert.ToInt16(sSystemCameraConfiguration[i].BitmapLockBitsArea.Width));
                binarywriter.Write(Convert.ToInt16(sSystemCameraConfiguration[i].BitmapLockBitsArea.Height));
            }
            binarywriter.Close();
            filestream.Close();
        }
    }
}