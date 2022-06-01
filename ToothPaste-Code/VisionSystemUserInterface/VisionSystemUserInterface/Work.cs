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

原作者：蒋涛
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
using System.Diagnostics;

using System.Net;
using System.Net.Sockets;
using System.Drawing.Imaging;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;

using Emgu.CV;
using Emgu.CV.Structure;
using System.Runtime.InteropServices;

using System.Threading;

using System.Management;

namespace VisionSystemUserInterface
{
    public partial class Work : Template
    {
        //WORK页面

        [DllImport("Kernel32.dll")]
        private extern static Boolean GetLocalTime(ref VisionSystemClassLibrary.Struct.SYSTEMTIME lpSystemTime);  //获取当前系统时间

        [DllImport("Kernel32.dll")]
        private extern static Boolean SetLocalTime(ref VisionSystemClassLibrary.Struct.SYSTEMTIME lpSystemTime);  //设置当前系统时间

        //

        public Boolean LoadOK = false;//程序加载是否完成。取值范围：true，是；false，否

        //

        private Int32 InitState;//初始化标记

        //private Stopwatch stop = new Stopwatch();//监视
        //stop.Reset();//监视
        //stop.Start();//监视
        //stop.Stop();//监视

        //构造函数

        //----------------------------------------------------------------------
        // 功能说明：构造函数，初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Work()
        {
            VisionSystemControlLibrary.GlobalWindows._Create_MessageDisplay_Window();
            VisionSystemControlLibrary.GlobalWindows._Create_StandardKeyboard_Window();
            VisionSystemControlLibrary.GlobalWindows._Create_FaultMessage_Window();
            VisionSystemControlLibrary.GlobalWindows._Create_FaultMessageOption_Window();
            VisionSystemControlLibrary.GlobalWindows._Create_NetDiagnose_Window();

            //

            //载入视觉系统类

            Global.VisionSystem = new VisionSystemClassLibrary.Class.System(".\\");//视觉系统类
            //Global.VisionSystem = new VisionSystemClassLibrary.Class.System(Application.StartupPath + "\\");//视觉系统类

            //

            InitializeComponent();

            //

            InitState = 0;
        }

        //属性

        //----------------------------------------------------------------------
        // 功能说明：CustomControl属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public VisionSystemControlLibrary.WorkControl CustomControl//属性
        {
            get//读取
            {                return workControl;
            }
        }

        //函数

        //系统更新检测

        //----------------------------------------------------------------------
        // 功能说明：窗口函数
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)//系统消息
            {
                case DEVICE_MESSAGE.WM_DEVICECHANGE://设备发生变动
                    //
                    switch (m.WParam.ToInt32())
                    {
                        case DEVICE_MESSAGE.DBT_DEVICEARRIVAL://系统检测到一个新设备
                            //
                            if (LoadOK)//程序加载完成
                            {
                                _CheckUpdate(true);//检测系统更新文件
                            }
                            //
                            break;
                        case DEVICE_MESSAGE.DBT_DEVICEREMOVECOMPLETE://系统完成移除一个设备
                            //
                            if (LoadOK)//程序加载完成
                            {
                                _CheckUpdate(false);//检测系统更新文件
                            }
                            //
                            break;
                        default:
                            break;
                    }
                    //
                    break;
                default:
                    break;
            }

            //

            base.WndProc(ref m);//默认函数
        }

        //----------------------------------------------------------------------
        // 功能说明：检测系统更新文件是否存在
        // 输入参数：1.bDeviceState：接入或移除设备。true，接入；false，移除
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _CheckUpdate(bool bDeviceState)
        {
            if (bDeviceState)//接入设备
            {
                Int32 i = 0;//循环控制变量

                DriveInfo[] driveinfo = DriveInfo.GetDrives();//获取驱动器信息

                FileVersionInfo fileversioninfo;//文件版本信息

                //

                workControl.HMIApplicationVersion = "";//人机界面程序文件版本，用于系统更新
                workControl.ControllerApplicationVersion = "";//控制器程序文件版本，用于系统更新
                workControl.UpdateHMIApplicationVersion = "";//人机界面更新程序文件版本，用于系统更新
                workControl.UpdateControllerApplicationVersion = "";//控制器更新程序文件版本，用于系统更新
                workControl.ReadyToUpdate = false;//是否存在可用更新。取值范围：true，是；false，否

                Global._GetControllerApplicationVersion();//控制器应用程序版本 
                Global.UpdateApplicationPath = "";//更新程序文件路径
                Global.UpdateHMIApplicationVersion = "";//人机界面更新程序文件版本
                Global.UpdateControllerApplicationVersion = "";//控制器更新程序文件版本

                //

                for (i = 0; i < driveinfo.Length; i++)//遍历驱动器
                {
                    if (DriveType.Removable == driveinfo[i].DriveType)//移动存储设备
                    {
                        Global.USBDeviceName = driveinfo[i].Name;//路径

                        Global.UpdateApplicationPath = driveinfo[i] + VisionSystemClassLibrary.Struct.System_UIParameter.UpdateFilePath;//人机界面更新程序文件路径

                        if (File.Exists(Global.UpdateApplicationPath + VisionSystemClassLibrary.Struct.System_UIParameter.HMIFileName))//人机界面程序升级文件存在
                        {
                            fileversioninfo = FileVersionInfo.GetVersionInfo(Global.UpdateApplicationPath + VisionSystemClassLibrary.Struct.System_UIParameter.HMIFileName);//人机界面更新程序文件名称（完整路径）

                            Global.UpdateHMIApplicationVersion = fileversioninfo.FileVersion;//人机界面更新程序文件版本

                            if (fileversioninfo.FileVersion.CompareTo(Global.HMIApplicationVersion) >= 0)//需要升级
                            {
                                workControl.HMIApplicationVersion = Global.HMIApplicationVersion;//人机界面程序文件版本，用于系统更新
                                workControl.UpdateHMIApplicationVersion = Global.UpdateHMIApplicationVersion;//人机界面更新程序文件版本，用于系统更新
                                workControl.ReadyToUpdate = true;//是否存在可用更新。取值范围：true，是；false，否
                            }
                        }

                        //

                        if (File.Exists(Global.UpdateApplicationPath + VisionSystemClassLibrary.Struct.System_UIParameter.ControllerFileName))//控制器程序升级文件存在
                        {
                            fileversioninfo = FileVersionInfo.GetVersionInfo(Global.UpdateApplicationPath + VisionSystemClassLibrary.Struct.System_UIParameter.ControllerFileName);//控制器更新程序文件名称（完整路径）

                            Global.UpdateControllerApplicationVersion = fileversioninfo.FileVersion;//控制器更新程序文件版本

                            if ("" != Global.ControllerApplicationVersion && fileversioninfo.FileVersion.CompareTo(Global.ControllerApplicationVersion) >= 0)//需要升级
                            {
                                workControl.ControllerApplicationVersion = Global.ControllerApplicationVersion;//控制器程序文件版本，用于系统更新
                                workControl.UpdateControllerApplicationVersion = Global.UpdateControllerApplicationVersion;//控制器更新程序文件版本，用于系统更新
                                workControl.ReadyToUpdate = true;//是否存在可用更新。取值范围：true，是；false，否
                            }
                        }

                        //

                        break;
                    }
                }
            }
            else//移除设备
            {
                Global.USBDeviceName = "";//路径

                workControl.HMIApplicationVersion = "";//人机界面程序文件版本，用于系统更新
                workControl.ControllerApplicationVersion = "";//控制器程序文件版本，用于系统更新
                workControl.UpdateHMIApplicationVersion = "";//人机界面更新程序文件版本，用于系统更新
                workControl.UpdateControllerApplicationVersion = "";//控制器更新程序文件版本，用于系统更新
                workControl.ReadyToUpdate = false;//是否存在可用更新。取值范围：true，是；false，否
            }

            //

            if (Global.BackupBrandsWindow.WindowDisplay)//BRAND MANAGEMENT，BACKUP BRANDS页面
            {
                if ("" != Global.USBDeviceName)//存在
                {
                    Global.BackupBrandsWindow.CustomControl._UpdateUSBDevice(Global.USBDeviceName + Global.VisionSystem.MachineType[Global.VisionSystem.SelectedMachineType] + " " + Global.VisionSystem._GetProductName(VisionSystemClassLibrary.Enum.InterfaceLanguage.English) + "\\" + VisionSystemClassLibrary.Class.Brand.USBDeviceBackupBrandPathName);//更新
                }
                else//不存在
                {
                    Global.BackupBrandsWindow.CustomControl._UpdateUSBDevice("");//更新
                }
            }
            else if (Global.RestoreBrandsWindow.WindowDisplay)//BRAND MANAGEMENT，RESTORE BRANDS页面
            {
                if ("" != Global.USBDeviceName)//存在
                {
                    Global.RestoreBrandsWindow.CustomControl._UpdateUSBDevice(Global.USBDeviceName + Global.VisionSystem.MachineType[Global.VisionSystem.SelectedMachineType] + " " + Global.VisionSystem._GetProductName(VisionSystemClassLibrary.Enum.InterfaceLanguage.English) + "\\" + VisionSystemClassLibrary.Class.Brand.USBDeviceBackupBrandPathName);//更新
                }
                else//不存在
                {
                    Global.RestoreBrandsWindow.CustomControl._UpdateUSBDevice("");//更新
                }
            }
            else//其它
            {
                //不执行操作
            }
        }

        //窗口页面

        //----------------------------------------------------------------------
        // 功能说明：设置属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _SetWindow()
        {
            switch (Global.CurrentInterface)
            {
                case ApplicationInterface.Work://WORK页面
                    //
                    if (Global.TopMostWindows)//置顶
                    {
                        this.TopMost = true;//将窗口置于顶层
                    }
                    //
                    break;
                case ApplicationInterface.Load://LOAD页面
                    //
                    Global.CurrentInterface = ApplicationInterface.Work;//当前页面，Work

                    Global.LoadWindow.TopMost = false;

                    if (Global.TopMostWindows)//置顶
                    {
                        this.TopMost = true;//将窗口置于顶层

                        Global.LoadWindow.Dispose();
                    }
                    else//其它
                    {
                        Global.LoadWindow.Visible = false;//隐藏
                    }

                    //启动以太网通信

                    Global.NetServer.ServerData = Global.NetServerData;//初始化以太网通信服务端
                    Global.NetServer._Start();//启动以太网通信服务端

                    //FAULT MESSAGE

                    Thread thread = new Thread(_threadFaultMessage);//加载线程
                    thread.IsBackground = true;
                    thread.Start();//启动线程
                    //
                    break;
                case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                    //
                    Global.CurrentInterface = ApplicationInterface.Work;//当前页面，Work

                    _SendCommand_Cameras(CommunicationInstructionType.Work, VisionSystemClassLibrary.Enum.CameraType.Camera_1);//发送指令

                    //

                    if (Global.TopMostWindows)//置顶
                    {
                        this.TopMost = true;//将窗口置于顶层
                    }
                    else//其它
                    {
                        Global.BrandManagementWindow.Visible = false;//隐藏
                    }

                    //
                    break;
                    //
                case ApplicationInterface.LiveView://LIVE VIEW页面
                    //
                    Global.CurrentInterface = ApplicationInterface.Work;//当前页面，Work

                    try
                    {
                        if (Global.LiveViewWindow.CustomControl.SelfTrigger)//SELF TRIGGER，开启
                        {
                            Global.LiveViewWindow.CustomControl.SelfTrigger = false;//关闭SELF TRIGGER

                            _SendCommand_Value(CommunicationInstructionType.Live_SelfTrigger, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex, 0);//发送指令
                        }
                        //else//SELF TRIGGER，关闭
                        {
                            _SendCommand_Cameras(CommunicationInstructionType.Work, VisionSystemClassLibrary.Enum.CameraType.Camera_1);//发送指令
                        }
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
                        Global.LiveViewWindow.Visible = false;//隐藏
                    }

                    //
                    break;
                    //
                case ApplicationInterface.QualityCheck://QUALITY CHECK页面
                    //
                    Global.CurrentInterface = ApplicationInterface.Work;//当前页面，Work

                    _SendCommand_Value(CommunicationInstructionType.QualityCheck_SaveProduct, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex, 0);//发送指令

                    //

                    if (Global.TopMostWindows)//置顶
                    {
                        this.TopMost = true;//将窗口置于顶层
                    }
                    else//其它
                    {
                        Global.QualityCheckWindow.Visible = false;//隐藏
                    }

                    //
                    break;
                    //
                case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面
                    //
                    Global.CurrentInterface = ApplicationInterface.Work;//当前页面，Work

                    _SendCommand_Value(CommunicationInstructionType.TolerancesSettings_SaveProduct, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex, 0);//发送指令

                    //

                    if (Global.TopMostWindows)//置顶
                    {
                        this.TopMost = true;//将窗口置于顶层
                    }
                    else//其它
                    {
                        Global.TolerancesSettingsWindow.Visible = false;//隐藏
                    }

                    //
                    break;
                    //
                case ApplicationInterface.DevicesSetup://DEVICES SETUP页面
                    //
                    Global.CurrentInterface = ApplicationInterface.Work;//当前页面，Work

                    _SendCommand_Cameras(CommunicationInstructionType.Work, VisionSystemClassLibrary.Enum.CameraType.Camera_1);//发送指令

                    //

                    if (Global.TopMostWindows)//置顶
                    {
                        this.TopMost = true;//将窗口置于顶层
                    }
                    else//其它
                    {
                        Global.DevicesSetupWindow.Visible = false;//隐藏
                    }

                    //
                    break;
                    //
                case ApplicationInterface.SystemConfiguration://SYSTEM页面
                    //
                    Global.CurrentInterface = ApplicationInterface.Work;//当前页面，Work

                    _SendCommand_Cameras(CommunicationInstructionType.Work, VisionSystemClassLibrary.Enum.CameraType.Camera_1);//发送指令

                    //

                    if (Global.TopMostWindows)//置顶
                    {
                        this.TopMost = true;//将窗口置于顶层
                    }
                    else//其它
                    {
                        Global.SystemConfigurationWindow.Visible = false;//隐藏
                    }

                    //
                    break;
                case ApplicationInterface.StatisticsView://STATISTICS VIEW页面
                    //
                    Global.CurrentInterface = ApplicationInterface.Work;//当前页面，Work

                    _SendCommand_Cameras(CommunicationInstructionType.Work, VisionSystemClassLibrary.Enum.CameraType.Camera_1);//发送指令

                    //

                    if (Global.TopMostWindows)//置顶
                    {
                        this.TopMost = true;//将窗口置于顶层
                    }
                    else//其它
                    {
                        Global.StatisticsViewWindow.Visible = false;//隐藏
                    }

                    //
                    break;
                default:
                    break;
            }
        }

        //产品密钥

        //----------------------------------------------------------------------
        // 功能说明：获取磁盘序列号
        // 输入参数：无
        // 输出参数：无
        // 返回值：磁盘序列号
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
        // 功能说明：获取CPU ID
        // 输入参数：无
        // 输出参数：无
        // 返回值：CPU ID
        //----------------------------------------------------------------------
        private String _GetCPUID()
        {
            String sCPUID = "";//返回值

            try
            {
                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();

                foreach (ManagementObject mo in moc)//获取CPU ID
                {
                    sCPUID = mo.Properties["ProcessorId"].Value.ToString();

                    //

                    break;
                }
            }
            catch (System.Exception ex)
            {
                //不执行操作
            }

            //

            return sCPUID;
        }

        //----------------------------------------------------------------------
        // 功能说明：获取产品密钥
        // 输入参数：1.sValue：运算数值
        // 输出参数：无
        // 返回值：产品密钥
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
        // 输入参数：1.sProductKeyPath：产品密钥完整路径（包含文件名）
        //         2.sValue：运算数值
        // 输出参数：无
        // 返回值：是否有效。取值范围：true，是；false，否
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
        // 功能说明：创建产品密钥
        // 输入参数：1.sProductKeyPath：产品密钥完整路径（包含文件名）
        //         2.sProductKey：产品密钥
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _CreateProductKey(String sProductKeyPath, String sProductKey)
        {
            FileStream filestream = null;

            try
            {
                filestream = new FileStream(sProductKeyPath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, FileOptions.WriteThrough); //打开文件

                BinaryWriter binarywriter = new BinaryWriter(filestream);
                binarywriter.Write(sProductKey);//写入

                binarywriter.Close();//关闭文件
                filestream.Close();//关闭流
            }
            catch (Exception ex)
            {
                //不执行操作
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
        private void Work_Load(object sender, EventArgs e)
        {
            //获取程序版本

            FileVersionInfo fileversioninfo = FileVersionInfo.GetVersionInfo(Application.ExecutablePath);
            Global.HMIApplicationVersion = fileversioninfo.FileVersion;//本机应用程序版本

            ////注册表

            //String sRegistryKeyName = Microsoft.Win32.Registry.LocalMachine.Name + "\\SOFTWARE\\VisionSystem\\" + VisionSystemClassLibrary.Struct.System_UIParameter.HMIApplicationName + "\\";
            //String sRegistryValueName_Product = "Product";
            //String sRegistryValueName_ApplicationPath = "Path";
            //String sRegistryValueName_Data = "Data";

            //String sProductName = (String)Microsoft.Win32.Registry.GetValue(sRegistryKeyName, sRegistryValueName_Product, "");
            //String sApplicationPath = (String)Microsoft.Win32.Registry.GetValue(sRegistryKeyName, sRegistryValueName_ApplicationPath, "");
            //Int32 iData_Value = (Int32)Microsoft.Win32.Registry.GetValue(sRegistryKeyName, sRegistryValueName_Data, -1);

            //if (sProductName != Global.VisionSystem.ProductModelNumber)
            //{
            //    Microsoft.Win32.Registry.SetValue(sRegistryKeyName, sRegistryValueName_Product, Global.VisionSystem.ProductModelNumber);
            //}

            //if (sApplicationPath != Application.StartupPath + "\\")
            //{
            //    Microsoft.Win32.Registry.SetValue(sRegistryKeyName, sRegistryValueName_ApplicationPath, Application.StartupPath + "\\");
            //}

            //if (-1 == iData_Value)
            //{
            //    Microsoft.Win32.Registry.SetValue(sRegistryKeyName, sRegistryValueName_Data, Global.Data_Value);
            //}
            //else
            //{
            //    Global.Data_Value = iData_Value;
            //}

            Global.Data_Value = Global.VisionSystem.Data_Value;

            Global.Load_Background_Chinese = new Bitmap(Global.VisionSystem.ConfigDataPath + Global.Load_Background_Chinese_FileName);//设备背景图像（WORK，LIVE）
            Global.Load_Background_English = new Bitmap(Global.VisionSystem.ConfigDataPath + Global.Load_Background_English_FileName);//设备背景图像（WORK，LIVE）
            //Global.About_Background_Chinese = new Bitmap(Global.VisionSystem.ConfigDataPath + Global.About_Background_Chinese_FileName);//设备背景图像（WORK，LIVE）
            //Global.About_Background_English = new Bitmap(Global.VisionSystem.ConfigDataPath + Global.About_Background_English_FileName);//设备背景图像（WORK，LIVE）

            //STARTUP窗口

            Global.StartupWindow = new VisionSystemUserInterface.Startup();//STARTUP窗口
            Global.StartupWindow.StartPosition = FormStartPosition.CenterScreen;
            Global.StartupWindow.TopMost = true;
            Global.StartupWindow.Visible = true;//显示
            Global.StartupWindow.Update();

            Update();
            
            //

            InitState = 1;

            timerLoad.Enabled = true;
        }

        //----------------------------------------------------------------------
        // 功能说明：启动加载定时器事件，显示加载窗口
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private void timerLoad_Tick(object sender, EventArgs e)
        {
            timerLoad.Enabled = false;

            if (InitState == 1)
            {
                VisionSystemControlLibrary.GlobalWindows._Create_DigitalKeyboard_Window();

                InitState++;
                timerLoad.Enabled = true;
            }
            else if (InitState == 2)
            {
                VisionSystemControlLibrary.GlobalWindows._Create_DeviceConfiguration_Window();

                InitState++;
                timerLoad.Enabled = true;
            }
            else if (InitState == 3)
            {
                VisionSystemControlLibrary.GlobalWindows._Create_IOSignalDiagnosis_Window();

                InitState++;
                timerLoad.Enabled = true;
            }
            else if (InitState == 4)
            {
                VisionSystemControlLibrary.GlobalWindows._Create_DateTimePanel_Window();

                InitState++;
                timerLoad.Enabled = true;
            }
            else if (InitState == 5)
            {
                VisionSystemControlLibrary.GlobalWindows._Create_ShiftConfiguration_Window();

                InitState++;
                timerLoad.Enabled = true;
            }
            else if (InitState == 6)
            {
                VisionSystemControlLibrary.GlobalWindows._Create_StatisticsRecord_Window();

                InitState++;
                timerLoad.Enabled = true;
            }
            else if (InitState == 7)
            {
                VisionSystemControlLibrary.GlobalWindows._Create_EditTools_Window();

                InitState++;
                timerLoad.Enabled = true;
            }
            else if (InitState == 8)
            {
                VisionSystemControlLibrary.GlobalWindows._Create_NewTool_Window();

                InitState++;
                timerLoad.Enabled = true;
            }
            else if (InitState == 9)
            {
                VisionSystemControlLibrary.GlobalWindows._Create_ParameterSettings_Window();

                InitState++;
                timerLoad.Enabled = true;
            }
            else if (InitState == 10)
            {
                VisionSystemControlLibrary.GlobalWindows._Create_SensorSelect_Window();

                InitState++;
                timerLoad.Enabled = true;
            }
            else if (InitState == 11)
            {
                VisionSystemControlLibrary.GlobalWindows._Create_CigaretteSort_Window();

                InitState++;
                timerLoad.Enabled = true;
            }
            else if (InitState == 12)
            {
                //LOAD窗口

                Global.LoadWindow = new VisionSystemUserInterface.Load();//LOAD窗口
                Global.LoadWindow.CustomControl.BitmapTrademark = VisionSystemClassLibrary.Class.System.Trademark_1[Global.Data_Value];//商标
                Global.LoadWindow.StartPosition = FormStartPosition.CenterScreen;
                Global.LoadWindow.TopMost = true;//将窗口置于顶层
                Global.LoadWindow.Visible = true;//显示
                Global.LoadWindow.Update();

                Update();

                InitState++;
                timerLoad.Enabled = true;
            }
            else if (InitState == 13)
            {
                //以太网通信服务端事件订阅，数据初始化

                Global.NetServer.ClientConnected += new System.EventHandler(NetServer_ClientConnected);//以太网通信，客户端连接时的事件
                Global.NetServer.DataReceived += new System.EventHandler(NetServer_DataReceived);//以太网通信，接收到一帧完整的数据时的事件
                Global.NetServer.ExceptionHandled += new System.EventHandler(NetServer_ExceptionHandled);//以太网通信，接收和发送数据时产生的异常事件

                Global.NetServerData.Port = Global.NetPort;
                Global.NetServerData.ReceiveBufferSize = Global.NetReceiveBufferSize;
                Global.NetServerData.SendBufferSize = Global.NetSendBufferSize;

                InitState++;
                timerLoad.Enabled = true;
            }
            else if (InitState == 14)
            {
                Global.WorkWindow = this;//WORK窗口

                Global.LoadWindow._LoadData();//加载数据

                InitState++;
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：点击【SYSTEM】按钮事件，打开SYSTEM窗口
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void workControl_System_Click(object sender, EventArgs e)
        {
            Global.SystemConfigurationWindow._SetWindow();//设置窗口
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【DEVICES SETUP】按钮事件，打开DEVICE CONFIGURATION窗口
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void workControl_DevicesSetup_Click(object sender, EventArgs e)
        {
            Global.DevicesSetupWindow._SetWindow();//设置窗口
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【BRAND MANAGEMENT】按钮事件，打开BRAND MANAGEMENT窗口
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void workControl_BrandManagement_Click(object sender, EventArgs e)
        {
            Global.BrandManagementWindow._SetWindow();//设置窗口
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【TOLERANCES】按钮事件，打开TOLERANCES SETTINGS窗口
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void workControl_Tolerances_Click(object sender, EventArgs e)
        {
            if (0 <= Global.VisionSystem.Work.SelectedCameraIndex)//当前选择的相机在相机数组中的索引值有效
            {
                Global.TolerancesSettingsWindow._SetWindow();//设置窗口
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【LIVE】按钮事件，打开LIVE VIEW窗口
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void workControl_Live_Click(object sender, EventArgs e)
        {
            if (0 <= Global.VisionSystem.Work.SelectedCameraIndex)//当前选择的相机在相机数组中的索引值有效
            {
                Global.LiveViewWindow._SetWindow();//设置窗口
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【QUALITY CHECK】按钮事件，打开QUALITY CHECK窗口
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void workControl_QualityCheck_Click(object sender, EventArgs e)
        {
            if (0 <= Global.VisionSystem.Work.SelectedCameraIndex)//当前选择的相机在相机数组中的索引值有效
            {
                Global.QualityCheckWindow._SetWindow();//设置窗口
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【STATISTICS VIEW】按钮事件，打开STATISTICS窗口
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void workControl_Statistics_Click(object sender, EventArgs e)
        {
            if (0 <= Global.VisionSystem.Work.SelectedCameraIndex)//当前选择的相机在相机数组中的索引值有效
            {
                Global.StatisticsViewWindow._SetWindow();//设置窗口
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：设置故障信息使能状态事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void workControl_SetFaultMessageState(object sender, EventArgs e)
        {
            Global.WorkWindow._SendCommand_Cameras(CommunicationInstructionType.SetFaultMessageState);//发送指令
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：点击故障信息窗口中的【CLEAR ALL】事件，清除故障信息
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void workControl_ClearAllFaultMessages(object sender, EventArgs e)
        {
            Global.WorkWindow._SendCommand_Cameras(CommunicationInstructionType.ClearAllFaultMessages);//发送指令
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：点击Trademark控件输入密码事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void workControl_PasswordEnter(object sender, EventArgs e)
        {
            if (1 == workControl.PasswordType)//用户密码
            {
                Global.WorkWindow.TitleBar.UserPassword = Global.VisionSystem.UserPassword;//用户密码
                Global.SystemConfigurationWindow.TitleBar.UserPassword = Global.VisionSystem.UserPassword;//用户密码
                Global.DevicesSetupWindow.TitleBar.UserPassword = Global.VisionSystem.UserPassword;//用户密码
                Global.ImageConfigurationWindow.TitleBar.UserPassword = Global.VisionSystem.UserPassword;//用户密码
                Global.BrandManagementWindow.TitleBar.UserPassword = Global.VisionSystem.UserPassword;//用户密码
                Global.BackupBrandsWindow.TitleBar.UserPassword = Global.VisionSystem.UserPassword;//用户密码
                Global.RestoreBrandsWindow.TitleBar.UserPassword = Global.VisionSystem.UserPassword;//用户密码
                Global.TolerancesSettingsWindow.TitleBar.UserPassword = Global.VisionSystem.UserPassword;//用户密码
                Global.LiveViewWindow.TitleBar.UserPassword = Global.VisionSystem.UserPassword;//用户密码
                Global.QualityCheckWindow.TitleBar.UserPassword = Global.VisionSystem.UserPassword;//用户密码
                Global.StatisticsViewWindow.TitleBar.UserPassword = Global.VisionSystem.UserPassword;//用户密码

            }
            else if (3 == workControl.PasswordType)//维护密码
            {
                Global.NetServer._Stop();
                Global.NetServer.Dispose();

                Global.LoadWindow.Dispose();//LOAD窗口
                Global.SystemConfigurationWindow.Dispose();//SYSTEM窗口
                Global.DevicesSetupWindow.Dispose();//DEVICES SETUP窗口
                Global.BrandManagementWindow.Dispose();//BRAND MANAGEMENT窗口
                Global.BackupBrandsWindow.Dispose();//BRAND MANAGEMENT，BACKUP BRANDS窗口
                Global.RestoreBrandsWindow.Dispose();//BRAND MANAGEMENT，RESTORE BRANDS窗口
                Global.TolerancesSettingsWindow.Dispose();//TOLERANCES SETTINGS窗口
                Global.LiveViewWindow.Dispose();//LIVE VIEW窗口
                 Global.QualityCheckWindow.Dispose();//QUALITY CHECK窗口
                Global.ImageConfigurationWindow.Dispose();//IMAGE CONFIGURATION窗口
                Global.StatisticsViewWindow.Dispose();//STATISTICS VIEW窗口
                Global.WorkWindow.Dispose();//WORK窗口
            }
            else//其它
            {
                //不执行操作
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：双击相机显示控件事件，打开LIVE VIEW窗口
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void workControl_CameraDisplay_DoubleClick(object sender, EventArgs e)
        {
            if (0 <= Global.VisionSystem.Work.SelectedCameraIndex)//当前选择的相机在相机数组中的索引值有效
            {
                Global.LiveViewWindow._SetWindow();//设置窗口
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【系统更新】按钮时产生的事件，打开系统更新窗口
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void workControl_Update_Click(object sender, EventArgs e)
        {
            if (VisionSystemClassLibrary.Enum.UpdateApplicationResult.HMI == workControl.Result)//人机界面程序更新
            {
                //启动更新程序

                Global._UpdateHMIApplication();
            }
            else//其它
            {
                //网络通信，更新控制器程序
                //控制器程序更新完成后，更新人机界面程序

                Int32 i = 0;
                Int32 j = 0;

                //for (i = 0; i < Global.VisionSystem.Camera.Count; i++)//遍历相机
                //{
                //    if (Global.VisionSystem.Camera[i].DeviceInformation.Connected && Global.VisionSystem.Camera[i].DeviceInformation.GetDevInfo)//有效
                //    {
                //        j++;
                //    }
                //}

                for (i = 0; i < Global.VisionSystem.Camera.Count; i++)//遍历相机
                {
                    if ("" != Global.VisionSystem.Camera[i].DeviceInformation.IP)//有效
                    {
                        j++;
                    }
                }

                workControl.UpdateNumber = j;

                //

                if (0 == j)//无效
                {
                    workControl._UpdateControllerApplication(true);
                }
                else//有效
                {
                    for (i = 0; i < Global.VisionSystem.Camera.Count; i++)//遍历相机
                    {
                        Int32 iCameraChooseState = 0;
                        String sControllerENGName = Global.VisionSystem.Camera[i].ControllerENGName;

                        for (j = 0; j < Global.VisionSystem.Camera.Count; j++)
                        {
                            if (sControllerENGName == Global.VisionSystem.Camera[j].ControllerENGName)//有效
                            {
                                iCameraChooseState |= (0x01 << (Global.VisionSystem.Camera[j].DeviceInformation.Port - 1));
                            }
                        }

                        Global.WorkWindow._SendCommand_Value(CommunicationInstructionType.ClientSystem_Update, Global.VisionSystem.Camera[i].Type, iCameraChooseState, i, Global.UpdateApplicationPath, VisionSystemClassLibrary.Struct.System_UIParameter.ControllerFileName);//发送指令
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：控制器更新完成，需要更新UI时产生的事件，更新
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void workControl_UpdateHMI(object sender, EventArgs e)
        {
            Global._UpdateHMIApplication();
        }

        //

        //FAULT MESSAGE

        //----------------------------------------------------------------------
        // 功能说明：FAULT MESSAGE
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _threadFaultMessage()
        {
            while (true)
            {
                try
                {
                    _SendCommand_Cameras(CommunicationInstructionType.CurrentFaultMessage);//发送指令

                    //

                    Thread.Sleep(Global.CurrentFaultMessageTime);//延时
                }
                catch (System.Exception ex)
                {
                    //不执行操作
                }
            }
        }

        //
        
        //以太网通信

        //----------------------------------------------------------------------
        // 功能说明：以太网通信，客户端连接时的事件
        // 输入参数：1.sender：ServerControl控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void NetServer_ClientConnected(object sender, EventArgs e)
        {
            VisionSystemCommunicationLibrary.Ethernet.ServerControl serverControl = (VisionSystemCommunicationLibrary.Ethernet.ServerControl)sender;//ServerControl控件（实际使用中可忽略该变量值）

            VisionSystemCommunicationLibrary.Ethernet.ClientConnectedEventArgs serverData = (VisionSystemCommunicationLibrary.Ethernet.ClientConnectedEventArgs)e;//事件参数（重要）

            //操作

            _RequestClientDeviceInformation(serverData.Client);//发送客户端信息请求
        }

        //----------------------------------------------------------------------
        // 功能说明：接收到一帧完整的数据时的事件
        // 输入参数：1.sender：ServerControl控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void NetServer_DataReceived(object sender, EventArgs e)
        {
            VisionSystemCommunicationLibrary.Ethernet.ServerControl serverControl = (VisionSystemCommunicationLibrary.Ethernet.ServerControl)sender;//ServerControl控件（实际使用中可忽略该变量值）

            VisionSystemCommunicationLibrary.Ethernet.DataReceivedEventArgs serverData = (VisionSystemCommunicationLibrary.Ethernet.DataReceivedEventArgs)e;//事件参数（重要）

            //操作

            if (VisionSystemCommunicationLibrary.Ethernet.NetDataType.DeviceInfo == serverData.DataInfo.DataType)//接收的数据为指令，设备信息
            {
                //获取客户端设备信息

                _GetDeviceInformation(serverData);

                //

                _UpdateTitleBar();//更新标题栏
            }
            else if (VisionSystemCommunicationLibrary.Ethernet.NetDataType.Instruction == serverData.DataInfo.DataType)//接收的数据为指令，设备信息外的一般指令
            {
                switch ((CommunicationInstructionType)serverData.ReceivedData[serverData.DataInfo.InstructionIndex])
                {
                    case CommunicationInstructionType.NetCheck_ConnectCamera:

                        VisionSystemClassLibrary.Enum.CameraType CameraType_NetCheck_ConnectCamera = VisionSystemClassLibrary.Enum.CameraType.Camera_1;//相机类型
                        Int32 iValue_1_NetCheck_ConnectCamera = 0;//传输结果（1，成功；0，失败）

                        _GetInstructionData(serverData, ref CameraType_NetCheck_ConnectCamera, ref iValue_1_NetCheck_ConnectCamera);//解析指令数据

                        workControl.Invoke(new EventHandler(delegate { workControl._UpdateNetCheck_Connect(CameraType_NetCheck_ConnectCamera,1== iValue_1_NetCheck_ConnectCamera); }));//更新页面

                        //
                        break;
                        //
                    case CommunicationInstructionType.Load:

                        //启动载入，格式：

                        //完成文件发送
                        //客户端->服务端：指令类型 + 相机类型数据 + 传输结果（1，成功；0，失败）

                        VisionSystemClassLibrary.Enum.CameraType CameraType_Load = VisionSystemClassLibrary.Enum.CameraType.Camera_1;//相机类型
                        Int32 iValue_1_Load = 0;//传输结果（1，成功；0，失败）

                        _GetInstructionData(serverData, ref CameraType_Load, ref iValue_1_Load);//解析指令数据

                        //

                        Int32 iCameraIndex_Load = _GetSelectedCameraIndex(CameraType_Load);//相机索引值

                        //

                        _GetCheckSum(CameraType_Load);//获取校验和

                        //

                        Int32 iCameraChooseState_DeviceState_Synchronization = 0;
                        String sControllerENGName = Global.VisionSystem.Camera[iCameraIndex_Load].ControllerENGName;

                        for (Int32 j = 0; j < Global.VisionSystem.Camera.Count; j++)
                        {
                            if (sControllerENGName == Global.VisionSystem.Camera[j].ControllerENGName)//有效
                            {
                                iCameraChooseState_DeviceState_Synchronization |= (0x01 << (Global.VisionSystem.Camera[j].DeviceInformation.Port - 1));
                            }
                        }

                        _SendCommand_Value(CommunicationInstructionType.DeviceState_Synchronization, Global.VisionSystem.Camera[iCameraIndex_Load].Type, iCameraChooseState_DeviceState_Synchronization, iCameraIndex_Load, Global.VisionSystem.SelectedMachineType, (Int32)VisionSystemClassLibrary.Class.System.SystemDeviceState, VisionSystemClassLibrary.Class.System.MachineFaultEnableState, Global.VisionSystem.Brand.CURRENTBrandName, VisionSystemClassLibrary.Class.Shift.ShiftDataPath, VisionSystemClassLibrary.Class.Shift.ShiftFileName);//发送指令

                        //

                        workControl.Invoke(new EventHandler(delegate { workControl._SetCameraData(iCameraIndex_Load); }));//更新页面

                        //
                        break;
                        //
                    case CommunicationInstructionType.DevicesSetup_ResetDevice:

                        try
                        {
                            switch (Global.CurrentInterface)
                            {
                                case ApplicationInterface.Work://WORK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.Load://LOAD页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.LiveView://LIVE VIEW页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.QualityCheck://QUALITY CHECK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.DevicesSetup://DEVICES SETUP页面

                                    //DEVICES SETUP页面操作，RESET DEVICE，格式：    
                                    //客户端->服务端：指令类型 + 相机类型数据 + 复位设备结果（1，成功；0，失败）

                                    VisionSystemClassLibrary.Enum.CameraType Cameratype_DevicesSetupResetDevice = VisionSystemClassLibrary.Enum.CameraType.Camera_1;//相机类型数据
                                    Int32 iValue_1_DevicesSetupResetDevice = 0;//复位设备结果（1，成功；0，失败）

                                    _GetInstructionData(serverData, ref Cameratype_DevicesSetupResetDevice, ref iValue_1_DevicesSetupResetDevice);//解析指令数据

                                    Global.DevicesSetupWindow.CustomControl.Invoke(new EventHandler(delegate { Global.DevicesSetupWindow.CustomControl._ResetDevice(Convert.ToBoolean(iValue_1_DevicesSetupResetDevice)); }));//更新页面

                                    break;
                                    //
                                case ApplicationInterface.SystemConfiguration://SYSTEM页面

                                    break;
                                    //
                                default:
                                    break;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            //不执行操作
                        }
                        //
                        break;
                        //
                    case CommunicationInstructionType.DevicesSetup_ConfigDevice:

                        try
                        {
                            switch (Global.CurrentInterface)
                            {
                                case ApplicationInterface.Work://WORK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.Load://LOAD页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.LiveView://LIVE VIEW页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.QualityCheck://QUALITY CHECK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.DevicesSetup://DEVICES SETUP页面

                                    //DEVICES SETUP页面操作，CONFIG DEVICE，格式：

                                    //未完成文件发送
                                    //服务端->客户端（文件）：指令类型 + 相机类型数据 + 设置为的IP地址 + 设置为的相机类型数据 + 设置为的端口 + 设置为的模式 + 设置为的相机故障标记 + 设置为的相机检测使能 + 设置为的相机旋转角度 + 相机颜色 + 相机分辨率 + 是否为串口 + 烟支排列类型 + 设置为的相机数据截取区域缩放 + 设置为的相机数据截取区域缩放后是否居中 + 设置为的相机数据截取区域粘贴区域 + 设置为的相机数据截取区域 + 镜像标记 + 传感器应用场景 + 相机关联信息（关联类型 + 关联数量（相机类型 + 工位索引）） + 文件索引值（从0开始） + 文件
                                    //客户端->服务端（数据）：指令类型 + 相机类型数据 + 设置为的IP地址 + 设置为的相机类型数据 + 设置为的端口 + 文件索引值（从0开始） + 文件传输状态（1，文件发送中；2，文件发送完成） + 文件接收结果（1，成功；0，失败）

                                    //完成文件发送
                                    //服务端->客户端：指令类型 + 相机类型数据 + 设置为的IP地址 + 设置为的相机类型数据 + 设置为的端口 + 设置为的模式 + 设置为的相机故障标记 + 设置为的相机旋转角度 + 相机颜色 + 相机分辨率 + 是否为串口 + 烟支排列类型 + 设置为的相机数据截取区域缩放 + 设置为的相机数据截取区域缩放后是否居中 + 设置为的相机数据截取区域粘贴区域 + 设置为的相机数据截取区域 + 镜像标记 + 传感器应用场景 + 相机关联信息（关联类型 + 关联数量（相机类型 + 工位索引））
                                    //客户端->服务端：指令类型 + 相机类型数据 + 设置为的IP地址 + 设置为的相机类型数据 + 设置为的端口 + 文件索引值（从0开始） + 文件传输状态（1，文件发送中；2，文件发送完成） + 配置结果（1，成功；0，失败）

                                    VisionSystemClassLibrary.Enum.CameraType Cameratype_DevicesSetupConfigDevice = VisionSystemClassLibrary.Enum.CameraType.Camera_1;//相机类型数据
                                    Int32 iSelectedIPAddress_DevicesSetupConfigDevice = 0;//设置为的IP地址
                                    VisionSystemClassLibrary.Enum.CameraType SelectedCameraType_DevicesSetupConfigDevice = VisionSystemClassLibrary.Enum.CameraType.Camera_1;//设置为的相机类型数据
                                    Byte byteSelectedPort_DevicesSetupConfigDevice = 0;//设置为的相机端口
                                    Int32 CameraChooseState_DevicesSetupConfigDevice = 0;//设置为的相机模式
                                    UInt64 CameraFaultState_DevicesSetupConfigDevice = 0;//设置为的相机故障标记
                                    Boolean CheckEnable_DevicesSetupConfigDevice = true;//设置为的相机检测使能
                                    VisionSystemClassLibrary.Enum.CameraRotateAngle CameraAngle_DevicesSetupConfigDevice = VisionSystemClassLibrary.Enum.CameraRotateAngle.Angle_0;//设置为的相机旋转角度
                                    VisionSystemClassLibrary.Enum.VideoColor VideoColor_DevicesSetupConfigDevice = VisionSystemClassLibrary.Enum.VideoColor.RGB32;//设置为的相机颜色
                                    VisionSystemClassLibrary.Enum.VideoResolution VideoResolution_DevicesSetupConfigDevice = VisionSystemClassLibrary.Enum.VideoResolution._744x480;//设置为的相机分辨率
                                    Boolean IsSerialPort_DevicesSetupConfigDevice = false;//设置为的串口标记
                                    VisionSystemClassLibrary.Enum.TobaccoSortType TobaccoSortType_DevicesSetupConfigDevice = 0;//设置为的烟支排列方式
                                    Boolean BitmapResize_DevicesSetupConfigDevice = false;//设置为的相机数据截取区域缩放
                                    Boolean BitmapCenter_DevicesSetupConfigDevice = false;//设置为的相机数据截取区域缩放后是否居中
                                    Point BitmapAxis_DevicesSetupConfigDevice = new Point();//设置为的相机数据截取区域粘贴区域
                                    Rectangle BitmapArea_DevicesSetupConfigDevice = new Rectangle();//设置为的相机数据截取区域
                                    VisionSystemClassLibrary.Enum.CameraFlip CameraFlip_DevicesSetupConfigDevice = 0;//镜像标记
                                    VisionSystemClassLibrary.Enum.SensorProductType SensorProductType_DevicesSetupConfigDevice = 0;//传感器应用场景
                                    VisionSystemClassLibrary.Struct.RelevancyCameraInformation RelevancyCameraInformation_DevicesSetupConfigDevice = new VisionSystemClassLibrary.Struct.RelevancyCameraInformation(); //相机关联信息
                                    RelevancyCameraInformation_DevicesSetupConfigDevice.RelevancyCameraInfo = new Dictionary<VisionSystemClassLibrary.Enum.CameraType, byte>();

                                    Int32 iValue_1_DevicesSetupConfigDevice = 0;//文件索引值（从0开始）
                                    Int32 iValue_2_DevicesSetupConfigDevice = 0;//文件传输状态（1，文件发送中；2，文件发送完成）
                                    Int32 iValue_3_DevicesSetupConfigDevice = 0;//文件接收结果（1，成功；0，失败），配置结果（1，成功；0，失败）
                                    
                                    _GetInstructionData(serverData, ref Cameratype_DevicesSetupConfigDevice, ref iSelectedIPAddress_DevicesSetupConfigDevice, ref SelectedCameraType_DevicesSetupConfigDevice, ref byteSelectedPort_DevicesSetupConfigDevice, ref CameraChooseState_DevicesSetupConfigDevice,ref CameraFaultState_DevicesSetupConfigDevice, ref CheckEnable_DevicesSetupConfigDevice, ref CameraAngle_DevicesSetupConfigDevice, ref VideoColor_DevicesSetupConfigDevice,ref VideoResolution_DevicesSetupConfigDevice,ref IsSerialPort_DevicesSetupConfigDevice,ref TobaccoSortType_DevicesSetupConfigDevice,ref BitmapResize_DevicesSetupConfigDevice,ref BitmapCenter_DevicesSetupConfigDevice,ref BitmapAxis_DevicesSetupConfigDevice, ref BitmapArea_DevicesSetupConfigDevice, ref CameraFlip_DevicesSetupConfigDevice, ref SensorProductType_DevicesSetupConfigDevice,ref RelevancyCameraInformation_DevicesSetupConfigDevice, ref iValue_1_DevicesSetupConfigDevice, ref iValue_2_DevicesSetupConfigDevice, ref iValue_3_DevicesSetupConfigDevice);//解析指令数据
                                
                                    //

                                    Int32 iCameraIndex_DevicesSetupConfigDevice = _GetSelectedCameraIndex(Cameratype_DevicesSetupConfigDevice);//相机索引

                                    String sSelectedCameraName_DevicesSetupConfigDevice = _GetSystemCameraName(SelectedCameraType_DevicesSetupConfigDevice);//相机名称

                                    if (1 == iValue_2_DevicesSetupConfigDevice)//文件发送中
                                    {
                                        String sFileName = "";//文件名称
                                        String sCameraPath = "";//文件路径

                                        Boolean bFileSendOK = false;
                                        if(Global.VisionSystem.Camera[iCameraIndex_DevicesSetupConfigDevice].DeepLearningState)//包含数度学习模块
                                        {
                                            if (6 == iValue_1_DevicesSetupConfigDevice)//文件发送完成
                                            {
                                                bFileSendOK = true;
                                            }
                                        }
                                        else//常规检测模块
                                        {
                                            if (4 == iValue_1_DevicesSetupConfigDevice)//文件发送完成
                                            {
                                                bFileSendOK = true;
                                            }
                                        }

                                        if (bFileSendOK)//文件发送完成
                                        {
                                            //完成文件发送
                                            //服务端->客户端：指令类型 + 相机类型数据 + 设置为的IP地址 + 设置为的相机类型数据 + 设置为的端口 + 设置为的模式 + 设置为的相机故障标记 + 设置为的相机旋转角度 + 相机颜色 + 相机分辨率 + 是否为串口 + 烟支排列类型 + 设置为的相机数据截取区域缩放 + 设置为的相机数据截取区域缩放后是否居中 + 设置为的相机数据截取区域粘贴区域 + 设置为的相机数据截取区域 + 镜像标记 + 传感器应用场景 + 相机关联信息（关联类型 + 关联数量（相机类型 + 工位索引））

                                            Global.WorkWindow._SendCommand_Value(CommunicationInstructionType.DevicesSetup_ConfigDevice, Cameratype_DevicesSetupConfigDevice, iCameraIndex_DevicesSetupConfigDevice, iSelectedIPAddress_DevicesSetupConfigDevice, SelectedCameraType_DevicesSetupConfigDevice, byteSelectedPort_DevicesSetupConfigDevice, CameraChooseState_DevicesSetupConfigDevice, CameraFaultState_DevicesSetupConfigDevice, CheckEnable_DevicesSetupConfigDevice, CameraAngle_DevicesSetupConfigDevice, VideoColor_DevicesSetupConfigDevice, VideoResolution_DevicesSetupConfigDevice, IsSerialPort_DevicesSetupConfigDevice, TobaccoSortType_DevicesSetupConfigDevice, BitmapResize_DevicesSetupConfigDevice, BitmapCenter_DevicesSetupConfigDevice, BitmapAxis_DevicesSetupConfigDevice, BitmapArea_DevicesSetupConfigDevice, CameraFlip_DevicesSetupConfigDevice, SensorProductType_DevicesSetupConfigDevice, RelevancyCameraInformation_DevicesSetupConfigDevice, "", "", 0);//发送指令
                                        }
                                        else//发送文件
                                        {
                                            if (0 == iValue_1_DevicesSetupConfigDevice)//文件索引值（从0开始）
                                            {
                                                iValue_1_DevicesSetupConfigDevice++;//更新文件索引值

                                                //

                                                sFileName = VisionSystemClassLibrary.Class.Camera.ToolFileName;//文件名称
                                                sCameraPath = Global.VisionSystem.Brand.BrandPath + Global.VisionSystem.Brand.CURRENTBrandName + "\\" + sSelectedCameraName_DevicesSetupConfigDevice + "\\";//文件路径
                                            }
                                            else if (1 == iValue_1_DevicesSetupConfigDevice)//文件索引值（从0开始）
                                            {
                                                iValue_1_DevicesSetupConfigDevice++;//更新文件索引值

                                                //

                                                sFileName = VisionSystemClassLibrary.Class.Camera.ParameterFileName;//文件名称
                                                sCameraPath = Global.VisionSystem.Brand.BrandPath + Global.VisionSystem.Brand.CURRENTBrandName + "\\" + sSelectedCameraName_DevicesSetupConfigDevice + "\\";//文件路径
                                            }
                                            else if (2 == iValue_1_DevicesSetupConfigDevice)//文件索引值（从0开始）
                                            {
                                                iValue_1_DevicesSetupConfigDevice++;//更新文件索引值

                                                //

                                                sFileName = VisionSystemClassLibrary.Class.Camera.SampleDataFileName;//文件名称

                                                DirectoryInfo directoryinfo = new DirectoryInfo(Global.VisionSystem.Camera[iCameraIndex_DevicesSetupConfigDevice].SampleImagePath.Substring(0, Global.VisionSystem.Camera[iCameraIndex_DevicesSetupConfigDevice].SampleImagePath.Length - 1));//路径信息
                                                sCameraPath = Global.VisionSystem.Brand.BrandPath + Global.VisionSystem.Brand.CURRENTBrandName + "\\" + sSelectedCameraName_DevicesSetupConfigDevice + "\\" + directoryinfo.Name + "\\";//文件路径
                                            }
                                            else if (3 == iValue_1_DevicesSetupConfigDevice)//文件索引值（从0开始）
                                            {
                                                iValue_1_DevicesSetupConfigDevice++;//更新文件索引值

                                                //

                                                sFileName = VisionSystemClassLibrary.Class.Camera.SampleImageFileName + VisionSystemClassLibrary.Class.Camera.BMPFile;//文件名称

                                                DirectoryInfo directoryinfo = new DirectoryInfo(Global.VisionSystem.Camera[iCameraIndex_DevicesSetupConfigDevice].SampleImagePath.Substring(0, Global.VisionSystem.Camera[iCameraIndex_DevicesSetupConfigDevice].SampleImagePath.Length - 1));//路径信息
                                                sCameraPath = Global.VisionSystem.Brand.BrandPath + Global.VisionSystem.Brand.CURRENTBrandName + "\\" + sSelectedCameraName_DevicesSetupConfigDevice + "\\" + directoryinfo.Name + "\\";//文件路径
                                            }
                                            else if (4 == iValue_1_DevicesSetupConfigDevice)//文件索引值（从0开始）
                                            {
                                                iValue_1_DevicesSetupConfigDevice++;//更新文件索引值

                                                //

                                                sFileName = VisionSystemClassLibrary.Class.Camera.ClassesFile;//文件名称
                                                sCameraPath = Global.VisionSystem.Brand.BrandPath + Global.VisionSystem.Brand.CURRENTBrandName + "\\" + sSelectedCameraName_DevicesSetupConfigDevice + "\\";//文件路径
                                            }
                                            else
                                            {
                                                iValue_1_DevicesSetupConfigDevice++;//更新文件索引值

                                                //

                                                sFileName = VisionSystemClassLibrary.Class.Camera.ModelFileName;//文件名称
                                                sCameraPath = Global.VisionSystem.Brand.BrandPath + Global.VisionSystem.Brand.CURRENTBrandName + "\\" + sSelectedCameraName_DevicesSetupConfigDevice + "\\";//文件路径
                                            }

                                            Global.WorkWindow._SendCommand_Value(CommunicationInstructionType.DevicesSetup_ConfigDevice, Cameratype_DevicesSetupConfigDevice, iCameraIndex_DevicesSetupConfigDevice, iSelectedIPAddress_DevicesSetupConfigDevice, SelectedCameraType_DevicesSetupConfigDevice, byteSelectedPort_DevicesSetupConfigDevice, CameraChooseState_DevicesSetupConfigDevice, CameraFaultState_DevicesSetupConfigDevice, CheckEnable_DevicesSetupConfigDevice, CameraAngle_DevicesSetupConfigDevice, VideoColor_DevicesSetupConfigDevice, VideoResolution_DevicesSetupConfigDevice, IsSerialPort_DevicesSetupConfigDevice, TobaccoSortType_DevicesSetupConfigDevice, BitmapResize_DevicesSetupConfigDevice, BitmapCenter_DevicesSetupConfigDevice, BitmapAxis_DevicesSetupConfigDevice, BitmapArea_DevicesSetupConfigDevice, CameraFlip_DevicesSetupConfigDevice, SensorProductType_DevicesSetupConfigDevice, RelevancyCameraInformation_DevicesSetupConfigDevice, sCameraPath, sFileName, iValue_1_DevicesSetupConfigDevice);//发送指令
                                        }
                                    }
                                    else//文件发送完成
                                    {
                                        //完成文件发送

                                        DevicesSetup.ConfigDeviceNumber--;

                                        if (0 == DevicesSetup.ConfigDeviceNumber)//完成
                                        {
                                            Global.DevicesSetupWindow.CustomControl.Invoke(new EventHandler(delegate { Global.DevicesSetupWindow.CustomControl._ConfigDevice(Convert.ToBoolean(iValue_3_DevicesSetupConfigDevice)); }));//更新页面

                                            //

                                            //serverData.Client._Close();//断开连接
                                        }
                                    }
                                    
                                    break;
                                    //
                                case ApplicationInterface.SystemConfiguration://SYSTEM页面
                                    //
                                    break;
                                    //
                                default:
                                    break;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            //不执行操作
                        }
                        //
                        break;
                        //
                    case CommunicationInstructionType.DevicesSetup_TestIOEnter:

                        try
                        {
                            switch (Global.CurrentInterface)
                            {
                                case ApplicationInterface.Work://WORK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.Load://LOAD页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.LiveView://LIVE VIEW页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.QualityCheck://QUALITY CHECK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.DevicesSetup://DEVICES SETUP页面

                                    //DEVICES SETUP页面操作，TEST I/O，格式：    
                                    //客户端->服务端：指令类型 + 相机类型数据 + 操作结果（1，成功；0，失败）

                                    VisionSystemClassLibrary.Enum.CameraType Cameratype_DevicesSetupTestIOEnter = VisionSystemClassLibrary.Enum.CameraType.Camera_1;//相机类型数据
                                    Int32 iValue_1_DevicesSetupTestIOEnter = 0;//输入数据

                                    _GetInstructionData(serverData, ref Cameratype_DevicesSetupTestIOEnter, ref iValue_1_DevicesSetupTestIOEnter);//解析指令数据

                                    Int32 iCameraIndex_DevicesSetup_TestIOEnter = _GetSelectedCameraIndex(Cameratype_DevicesSetupTestIOEnter);//相机类型索引

                                    //

                                    _SendCommand_Value(CommunicationInstructionType.DevicesSetup_TestIO, Global.VisionSystem.Camera[iCameraIndex_DevicesSetup_TestIOEnter].Type, iCameraIndex_DevicesSetup_TestIOEnter, Global.VisionSystem.IOSignalData.OutputState);//发送指令

                                    break;
                                    //
                                case ApplicationInterface.SystemConfiguration://SYSTEM页面
                                    //
                                    break;
                                    //
                                default:
                                    break;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            //不执行操作
                        }
                        //
                        break;
                        //
                    case CommunicationInstructionType.DevicesSetup_TestIO:

                        try
                        {
                            switch (Global.CurrentInterface)
                            {
                                case ApplicationInterface.Work://WORK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.Load://LOAD页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.LiveView://LIVE VIEW页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.QualityCheck://QUALITY CHECK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.DevicesSetup://DEVICES SETUP页面

                                    //DEVICES SETUP页面操作，TEST I/O，格式：    
                                    //客户端->服务端：指令类型 + 相机类型数据 + 输入数据 + 输出诊断

                                    VisionSystemClassLibrary.Enum.CameraType Cameratype_DevicesSetupTestIO = VisionSystemClassLibrary.Enum.CameraType.Camera_1;//相机类型数据
                                    UInt32 iValue_1_DevicesSetupTestIO = 0;//输入数据
                                    UInt32 iValue_2_DevicesSetupTestIO = 0;//输出诊断

                                    _GetInstructionData(serverData, ref Cameratype_DevicesSetupTestIO, ref iValue_1_DevicesSetupTestIO, ref iValue_2_DevicesSetupTestIO);//解析指令数据

                                    Int32 iCameraIndex_DevicesSetup_TestIO = _GetSelectedCameraIndex(Cameratype_DevicesSetupTestIO);//相机类型索引

                                    //

                                    Global.VisionSystem.IOSignalData.InputState = iValue_1_DevicesSetupTestIO;
                                    Global.VisionSystem.IOSignalData.OutputDiagStateLab = iValue_2_DevicesSetupTestIO;

                                    VisionSystemControlLibrary.GlobalWindows.IOSignalDiagnosis_Window.IOSignalDiagnosisControl.Invoke(new EventHandler(delegate { VisionSystemControlLibrary.GlobalWindows.IOSignalDiagnosis_Window.IOSignalDiagnosisControl._Update(); }));//更新页面

                                    //

                                    if (Global.DevicesSetupWindow.CustomControl.TestIOWindowShow)//DEVICES SETUP，TEST I/O页面
                                    {
                                        _SendCommand_Value(CommunicationInstructionType.DevicesSetup_TestIO, Global.VisionSystem.Camera[iCameraIndex_DevicesSetup_TestIO].Type, iCameraIndex_DevicesSetup_TestIO, Global.VisionSystem.IOSignalData.OutputState);//发送指令
                                    }
                                    else//DEVICES SETUP页面
                                    {
                                        //不执行操作
                                    }

                                    break;
                                    //
                                case ApplicationInterface.SystemConfiguration://SYSTEM页面
                                    //
                                    break;
                                    //
                                default:
                                    break;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            //不执行操作
                        }
                        //
                        break;
                        //
                    case CommunicationInstructionType.DevicesSetup_TestIOExit:

                        try
                        {
                            switch (Global.CurrentInterface)
                            {
                                case ApplicationInterface.Work://WORK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.Load://LOAD页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.LiveView://LIVE VIEW页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.QualityCheck://QUALITY CHECK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.DevicesSetup://DEVICES SETUP页面

                                    //DEVICES SETUP页面操作，TEST I/O，格式：    
                                    //客户端->服务端：指令类型 + 相机类型数据

                                    VisionSystemClassLibrary.Enum.CameraType Cameratype_DevicesSetupTestIOExit = VisionSystemClassLibrary.Enum.CameraType.Camera_1;//相机类型数据

                                    _GetInstructionData(serverData, ref Cameratype_DevicesSetupTestIOExit);//解析指令数据

                                    VisionSystemControlLibrary.GlobalWindows.IOSignalDiagnosis_Window.IOSignalDiagnosisControl.Invoke(new EventHandler(delegate { VisionSystemControlLibrary.GlobalWindows.IOSignalDiagnosis_Window.IOSignalDiagnosisControl._Update(); }));//更新页面

                                    break;
                                case ApplicationInterface.SystemConfiguration://SYSTEM页面
                                    //
                                    break;
                                    //
                                default:
                                    break;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            //不执行操作
                        }
                        //
                        break;
                        //
                    case CommunicationInstructionType.DevicesSetup_ConfigImage_Save:

                        //DeviceSetup ConfigImage页面操作，保存参数，格式：
                        //客户端->服务端：指令类型 + 相机类型数据 + 是否保存数据（1，是；0，否） + 保存数据结果（1，成功；0，失败）

                        VisionSystemClassLibrary.Enum.CameraType Cameratype_DevicesSetup_ConfigImageSaveProduct = VisionSystemClassLibrary.Enum.CameraType.Camera_1;//相机类型数据
                        Int32 iValue_1_DevicesSetup_ConfigImageSaveProduct = 0;//是否保存数据（1，是；0，否）
                        Int32 iValue_2_DevicesSetup_ConfigImageSaveProduct = 0;//保存数据结果（1，成功；0，失败）

                        _GetInstructionData(serverData, ref Cameratype_DevicesSetup_ConfigImageSaveProduct, ref iValue_1_DevicesSetup_ConfigImageSaveProduct, ref iValue_2_DevicesSetup_ConfigImageSaveProduct);//解析指令数据

                        Int32 iCameraIndex_DevicesSetup_ConfigImageSaveProduc = _GetSelectedCameraIndex(Cameratype_DevicesSetup_ConfigImageSaveProduct);//相机索引值

                        if (1 == iValue_1_DevicesSetup_ConfigImageSaveProduct)//保存数据
                        {
                            try
                            {
                                switch (Global.CurrentInterface)
                                {
                                    case ApplicationInterface.Work://WORK页面
                                        //
                                        break;
                                        //
                                    case ApplicationInterface.Load://LOAD页面
                                        //
                                        break;
                                        //
                                    case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                                        //
                                        break;
                                        //
                                    case ApplicationInterface.LiveView://LIVE VIEW页面
                                        //
                                        break;
                                        //
                                    case ApplicationInterface.QualityCheck://QUALITY CHECK页面
                                        //
                                        break;
                                        //
                                    case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面
                                        //
                                        break;
                                        //
                                    case ApplicationInterface.DevicesSetup://DEVICES SETUP页面

                                        Global.VisionSystem.Camera[Global.WorkWindow._GetSelectedCameraIndex(Global.DevicesSetupWindow.CustomControl.CameraSelected)] = Global.ImageConfigurationWindow.CustomControl.SelectedCamera;

                                        Global.ImageConfigurationWindow.CustomControl.Invoke(new EventHandler(delegate { Global.ImageConfigurationWindow.CustomControl._SaveProduct(Convert.ToBoolean(iValue_2_DevicesSetup_ConfigImageSaveProduct)); }));
                                        
                                        break;
                                        //
                                    case ApplicationInterface.SystemConfiguration://SYSTEM页面
                                        //
                                        break;
                                        //
                                    default:
                                        break;
                                }
                            }
                            catch (System.Exception ex)
                            {
                                //不执行操作
                            }
                        }
                        else//不保存数据
                        {
                            try
                            {
                                switch (Global.CurrentInterface)
                                {
                                    case ApplicationInterface.Work://WORK页面
                                        //
                                        break;
                                        //
                                    case ApplicationInterface.Load://LOAD页面
                                        //
                                        break;
                                        //
                                    case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                                        //
                                        break;
                                        //
                                    case ApplicationInterface.LiveView://LIVE VIEW页面
                                        //
                                        break;
                                        //
                                    case ApplicationInterface.QualityCheck://QUALITY CHECK页面
                                        //
                                        break;
                                        //
                                    case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面
                                        //
                                        break;
                                        //
                                    case ApplicationInterface.DevicesSetup://DEVICES SETUP页面
                                        //
                                        break;
                                        //
                                    case ApplicationInterface.SystemConfiguration://SYSTEM页面
                                        //
                                        break;
                                        //
                                    default:
                                        break;
                                }
                            }
                            catch (System.Exception ex)
                            {
                                //不执行操作
                            }
                        }
                        //
                        break;
                        //
                    case CommunicationInstructionType.DevicesSetup_ConfigImage_Focus:

                        //DeviceSetup ConfigImage页面操作，聚焦设置，格式：
                        //客户端->服务端：指令类型 + 相机类型数据 + 聚焦参数更新结果（1，成功；0，不成功）

                        //（待定...）

                        try
                        {
                            switch (Global.CurrentInterface)
                            {
                                case ApplicationInterface.Work://WORK页面
                                    //
                                    break;
                                //
                                case ApplicationInterface.Load://LOAD页面
                                    //
                                    break;
                                //
                                case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                                    //
                                    break;
                                //
                                case ApplicationInterface.LiveView://LIVE VIEW页面
                                    //
                                    break;
                                //
                                case ApplicationInterface.QualityCheck://QUALITY CHECK页面
                                    //
                                    break;
                                //
                                case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面
                                    //
                                    break;
                                //
                                case ApplicationInterface.DevicesSetup://DEVICES SETUP页面
                                    //
                                    break;
                                //
                                case ApplicationInterface.SystemConfiguration://SYSTEM页面
                                    //
                                    break;
                                //
                                default:
                                    break;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            //不执行操作
                        }
                        //
                        break;
                        //
                    case CommunicationInstructionType.DevicesSetup_ConfigImage_White:
                        //DeviceSetup ConfigImage页面操作，白平衡设置，格式：
                        //客户端->服务端：指令类型 + 相机类型数据 + 白平衡参数更新结果（1，成功；0，不成功）

                        //（待定...）

                        try
                        {
                            switch (Global.CurrentInterface)
                            {
                                case ApplicationInterface.Work://WORK页面
                                    //
                                    break;
                                //
                                case ApplicationInterface.Load://LOAD页面
                                    //
                                    break;
                                //
                                case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                                    //
                                    break;
                                //
                                case ApplicationInterface.LiveView://LIVE VIEW页面
                                    //
                                    break;
                                //
                                case ApplicationInterface.QualityCheck://QUALITY CHECK页面
                                    //
                                    break;
                                //
                                case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面
                                    //
                                    break;
                                //
                                case ApplicationInterface.DevicesSetup://DEVICES SETUP页面
                                    //
                                    break;
                                //
                                case ApplicationInterface.SystemConfiguration://SYSTEM页面
                                    //
                                    break;
                                //
                                default:
                                    break;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            //不执行操作
                        }
                        //
                        break;
                        //
                    case CommunicationInstructionType.DevicesSetup_ConfigImage_Parameter:

                        //DeviceSetup ConfigImage页面操作，工具参数，格式：
                        //客户端->服务端：指令类型 + 相机类型数据 + 相机参数更新结果（1，成功；0，不成功） + 白平衡（红） + 白平衡（绿） + 白平衡（蓝）

                        VisionSystemClassLibrary.Enum.CameraType Cameratype_DevicesSetup_ConfigImageParameter = VisionSystemClassLibrary.Enum.CameraType.Camera_1;//相机类型数据
                        Int32 iValue_1_ConfigImageParameter = 0;//相机参数更新结果（1，成功；0，不成功）
                        UInt16 iValue_2_ConfigImageParameter = 0;//白平衡（红）
                        UInt16 iValue_3_ConfigImageParameter = 0;//白平衡（绿）
                        UInt16 iValue_4_ConfigImageParameter = 0;//白平衡（蓝）

                        _GetInstructionData(serverData, ref Cameratype_DevicesSetup_ConfigImageParameter, ref iValue_1_ConfigImageParameter, ref iValue_2_ConfigImageParameter, ref iValue_3_ConfigImageParameter, ref iValue_4_ConfigImageParameter);//解析指令数据

                        Int32 iCameraIndex_DevicesSetup_ConfigImageParameter = _GetSelectedCameraIndex(Cameratype_DevicesSetup_ConfigImageParameter);//相机类型索引

                        //

                        try
                        {
                            switch (Global.CurrentInterface)
                            {
                                case ApplicationInterface.Work://WORK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.Load://LOAD页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.LiveView://LIVE VIEW页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.QualityCheck://QUALITY CHECK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.DevicesSetup://DEVICES SETUP页面
                                    //
                                    if (1 == iValue_1_ConfigImageParameter)//成功
                                    {
                                        if (0 == Global.VisionSystem.Camera[iCameraIndex_DevicesSetup_ConfigImageParameter].DeviceParameter.WhiteBalance)//自动
                                        {
                                            Global.ImageConfigurationWindow.CustomControl.Invoke(new EventHandler(delegate { Global.ImageConfigurationWindow.CustomControl._UpdateParameter(iValue_2_ConfigImageParameter, iValue_3_ConfigImageParameter, iValue_4_ConfigImageParameter); }));
                                        }
                                    }
                                    //
                                    break;
                                    //
                                case ApplicationInterface.SystemConfiguration://SYSTEM页面
                                    //
                                    break;
                                    //
                                default:
                                    break;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            //不执行操作
                        }
                        //
                        break;
                        //
                    case CommunicationInstructionType.DevicesSetup_ConfigSensor_Parameter:

                        //DeviceSetup ConfigImage页面操作，工具参数，格式：
                        //客户端->服务端：指令类型 + 相机类型数据 + 烟支数量（N）+ 传感器校准过程标记（1，校准过程中；0，校准将诶书或未校准、取消校准） + 相机参数更新结果（1，成功；0，不成功） + 烟支校准值（N支）

                        VisionSystemClassLibrary.Enum.CameraType Cameratype_DevicesSetup_ConfigSensor_Parameter = VisionSystemClassLibrary.Enum.CameraType.Camera_1;//相机类型数据
                        Int32 iValue_1_ConfigSensor_Parameter = 0;//传感器校准过程标记（1，校准过程中；0，校准将诶书或未校准、取消校准）
                        Int32 iValue_2_ConfigSensor_Parameter = 0;//相机参数更新结果（1，成功；0，不成功）
                        Byte[] iData_ConfigSensor_Parameter = null;//校准值

                        _GetInstructionData(serverData, ref Cameratype_DevicesSetup_ConfigSensor_Parameter, ref iValue_1_ConfigSensor_Parameter,ref iValue_2_ConfigSensor_Parameter,ref iData_ConfigSensor_Parameter);//解析指令数据

                        Int32 iCameraIndex_DevicesSetup_ConfigSensor_Parameter = _GetSelectedCameraIndex(Cameratype_DevicesSetup_ConfigSensor_Parameter);//相机类型索引

                        //

                        try
                        {
                            switch (Global.CurrentInterface)
                            {
                                case ApplicationInterface.Work://WORK页面
                                    //
                                    break;
                                //
                                case ApplicationInterface.Load://LOAD页面
                                    //
                                    break;
                                //
                                case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                                    //
                                    break;
                                //
                                case ApplicationInterface.LiveView://LIVE VIEW页面
                                    //
                                    break;
                                //
                                case ApplicationInterface.QualityCheck://QUALITY CHECK页面
                                    //
                                    break;
                                //
                                case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面
                                    //
                                    break;
                                //
                                case ApplicationInterface.DevicesSetup://DEVICES SETUP页面
                                    //
                                    if (1 == Global.ImageConfigurationWindow.CustomControl.SensorAdjustState)//校准中
                                    {
                                        Global.ImageConfigurationWindow.CustomControl.Invoke(new EventHandler(delegate { Global.ImageConfigurationWindow.CustomControl._UpdateSensorAdjustValue(iValue_1_ConfigSensor_Parameter, iData_ConfigSensor_Parameter); }));

                                        if (1 == iValue_1_ConfigSensor_Parameter) //校准中
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
                                        else
                                        {
                                            Global.WorkWindow._SendCommand_Value
                                            (
                                                CommunicationInstructionType.DevicesSetup_ConfigSensor_MaxADC,
                                                Global.DevicesSetupWindow.CustomControl.CameraSelected,
                                                Global.WorkWindow._GetSelectedCameraIndex(Global.DevicesSetupWindow.CustomControl.CameraSelected),
                                                Global.VisionSystem.Camera[Global.WorkWindow._GetSelectedCameraIndex(Global.DevicesSetupWindow.CustomControl.CameraSelected)].SensorNumber
                                            );//发送指令
                                        }
                                    }

                                    //
                                    break;
                                //
                                case ApplicationInterface.SystemConfiguration://SYSTEM页面
                                    //
                                    break;
                                //
                                default:
                                    break;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            //不执行操作
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.DevicesSetup_ConfigSensor_MaxADC:

                        //DeviceSetup ConfigImage页面操作，工具参数，格式：
                        //客户端->服务端：指令类型 + 相机类型数据 + 烟支数量（N）+ 最大电压查询标记（1，查询过程中；0，查询结束） + 最大电压值（N支）

                        VisionSystemClassLibrary.Enum.CameraType Cameratype_DevicesSetup_ConfigSensor_MaxADC = VisionSystemClassLibrary.Enum.CameraType.Camera_1;//相机类型数据
                        Int32 iValue_1_ConfigSensor_MaxADC = 0;//最大电压查询标记（1，查询过程中；0，查询结束）
                        Int16[] iData_ConfigSensor_MaxADC = null;//最大电压值

                        _GetInstructionData(serverData, ref Cameratype_DevicesSetup_ConfigSensor_MaxADC, ref iValue_1_ConfigSensor_MaxADC, ref iData_ConfigSensor_MaxADC);//解析指令数据

                        Int32 iCameraIndex_DevicesSetup_ConfigSensor_MaxADC = _GetSelectedCameraIndex(Cameratype_DevicesSetup_ConfigSensor_MaxADC);//相机类型索引

                        //

                        try
                        {
                            switch (Global.CurrentInterface)
                            {
                                case ApplicationInterface.Work://WORK页面
                                    //
                                    break;
                                //
                                case ApplicationInterface.Load://LOAD页面
                                    //
                                    break;
                                //
                                case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                                    //
                                    break;
                                //
                                case ApplicationInterface.LiveView://LIVE VIEW页面
                                    //
                                    break;
                                //
                                case ApplicationInterface.QualityCheck://QUALITY CHECK页面
                                    //
                                    break;
                                //
                                case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面
                                    //
                                    break;
                                //
                                case ApplicationInterface.DevicesSetup://DEVICES SETUP页面
                                    //
                                    if (1 == Global.ImageConfigurationWindow.CustomControl.SensorAdjustState)//校准中
                                    {
                                        if (1 == iValue_1_ConfigSensor_MaxADC) //最大电压查询中
                                        {
                                            Global.WorkWindow._SendCommand_Value
                                            (
                                                CommunicationInstructionType.DevicesSetup_ConfigSensor_MaxADC,
                                                Global.DevicesSetupWindow.CustomControl.CameraSelected,
                                                Global.WorkWindow._GetSelectedCameraIndex(Global.DevicesSetupWindow.CustomControl.CameraSelected),
                                                Global.VisionSystem.Camera[Global.WorkWindow._GetSelectedCameraIndex(Global.DevicesSetupWindow.CustomControl.CameraSelected)].SensorNumber
                                            );//发送指令
                                        }
                                        else //传感器ADC最大值查找结束
                                        {
                                            Global.ImageConfigurationWindow.CustomControl.Invoke(new EventHandler(delegate { Global.ImageConfigurationWindow.CustomControl._UpdateADCMax(iValue_1_ConfigSensor_MaxADC, iData_ConfigSensor_MaxADC); }));
                                        }
                                    }

                                    //
                                    break;
                                //
                                case ApplicationInterface.SystemConfiguration://SYSTEM页面
                                    //
                                    break;
                                //
                                default:
                                    break;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            //不执行操作
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.DevicesSetup_ConfigImage_Enter:

                        //DeviceSetup ConfigImage页面操作，进入界面，格式：
                        ///客户端->服务端：指令类型 + 相机类型数据 + 操作结果（1，成功；0，不成功）
                        VisionSystemClassLibrary.Enum.CameraType Cameratype_DevicesSetup_ConfigImageEnter = VisionSystemClassLibrary.Enum.CameraType.Camera_1;//相机类型数据
                        Int32 iValue_1_DevicesSetup_ConfigImageEnter = 0;//操作结果（1，成功；0，失败）

                        _GetInstructionData(serverData, ref Cameratype_DevicesSetup_ConfigImageEnter, ref iValue_1_DevicesSetup_ConfigImageEnter);//解析指令数据

                        Int32 iCameraIndex_DevicesSetup_ConfigImageEnter = _GetSelectedCameraIndex(Cameratype_DevicesSetup_ConfigImageEnter);//相机类型索引

                        //

                        try
                        {
                            switch (Global.CurrentInterface)
                            {
                                case ApplicationInterface.Work://WORK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.Load://LOAD页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.LiveView://LIVE VIEW页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.QualityCheck://QUALITY CHECK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.DevicesSetup://DEVICES SETUP页面

                                    _SendCommand_Value(CommunicationInstructionType.Live_SelfTrigger, Global.VisionSystem.Camera[iCameraIndex_DevicesSetup_ConfigImageEnter].Type, iCameraIndex_DevicesSetup_ConfigImageEnter, 1);//发送指令

                                    break;
                                    //
                                case ApplicationInterface.SystemConfiguration://SYSTEM页面
                                    //
                                    break;
                                    //
                                default:
                                    break;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            //不执行操作
                        }
                        //
                        break;
                        //
                    case CommunicationInstructionType.DevicesSetup_ConfigImage_Live:

                        //CONFIG IMAGE页面操作，查看实时图像，格式：
                        //客户端->服务端：指令类型 + 相机类型数据 + 图像工具数据（1，包含工具；0，不包含工具） + 图像尺寸类型数据 + 图像宽度数据 + 图像高度数据 + 图像信息数据 + 图像数据

                        VisionSystemClassLibrary.Enum.CameraType Cameratype_DevicesSetupConfigImageLive = VisionSystemClassLibrary.Enum.CameraType.Camera_1;
                        Boolean bViewToolGraphics_DevicesSetupConfigImageLive = true;
                        Double dImageScale_DevicesSetupConfigImageLive = 0.0;
                        Int32 iImageDataLength_DevicesSetupConfigImageLive = 0;
                        VisionSystemClassLibrary.Struct.ImageInformation GraphicsInformation_DevicesSetupConfigImageLive = new VisionSystemClassLibrary.Struct.ImageInformation();
                        MemoryStream imageData_DevicesSetupConfigImageLive = new MemoryStream();//获取的图像数据
                        Int32 imageWidth_DevicesSetupConfigImageLive = 0;//获取图像宽度
                        Int32 imageHeight_DevicesSetupConfigImageLive = 0;//获取图像高度

                        _GetInstructionData(serverData, ref Cameratype_DevicesSetupConfigImageLive, ref bViewToolGraphics_DevicesSetupConfigImageLive, ref dImageScale_DevicesSetupConfigImageLive, ref iImageDataLength_DevicesSetupConfigImageLive, ref GraphicsInformation_DevicesSetupConfigImageLive, ref imageData_DevicesSetupConfigImageLive,ref imageWidth_DevicesSetupConfigImageLive,ref imageHeight_DevicesSetupConfigImageLive);//解析指令数据

                        //

                        try
                        {
                            switch (Global.CurrentInterface)
                            {
                                case ApplicationInterface.Work://WORK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.Load://LOAD页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.LiveView://LIVE VIEW页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.QualityCheck://QUALITY CHECK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.DevicesSetup://DEVICES SETUP页面
                                    //

                                    Int32 iCameraIndex_DevicesSetupConfigImageLive = _GetSelectedCameraIndex(Cameratype_DevicesSetupConfigImageLive);//相机索引值

                                    if (0 < iImageDataLength_DevicesSetupConfigImageLive)//图像有效
                                    {
                                        Bitmap bitmap_DevicesSetupConfigImageLive = new Bitmap(imageData_DevicesSetupConfigImageLive);
                                        BitmapData bitmapData_DevicesSetupConfigImageLive = bitmap_DevicesSetupConfigImageLive.LockBits(new Rectangle(0, 0, imageWidth_DevicesSetupConfigImageLive, imageHeight_DevicesSetupConfigImageLive), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                                        Image<Bgr, Byte> image_DevicesSetupConfigImageLive = new Image<Bgr, Byte>(imageWidth_DevicesSetupConfigImageLive, imageHeight_DevicesSetupConfigImageLive, bitmapData_DevicesSetupConfigImageLive.Stride, bitmapData_DevicesSetupConfigImageLive.Scan0);

                                        Global.VisionSystem.Camera[iCameraIndex_DevicesSetupConfigImageLive].ImageLive = image_DevicesSetupConfigImageLive.Copy();//更新数值

                                        bitmap_DevicesSetupConfigImageLive.UnlockBits(bitmapData_DevicesSetupConfigImageLive);

                                        Global.VisionSystem.Camera[iCameraIndex_DevicesSetupConfigImageLive].Live.GraphicsInformation = GraphicsInformation_DevicesSetupConfigImageLive;//图像信息
                                        if (0 <= GraphicsInformation_DevicesSetupConfigImageLive.ToolsIndex && (VisionSystemClassLibrary.Enum.ImageType.Error == GraphicsInformation_DevicesSetupConfigImageLive.Type))//剔除图像
                                        {
                                            Global.VisionSystem.Camera[iCameraIndex_DevicesSetupConfigImageLive].Live.GraphicsInformation.Name = Global.VisionSystem.Camera[iCameraIndex_DevicesSetupConfigImageLive].Tools[GraphicsInformation_DevicesSetupConfigImageLive.ToolsIndex].Name;//信息名称
                                        }
                                        else//完好图像
                                        {
                                            Global.VisionSystem.Camera[iCameraIndex_DevicesSetupConfigImageLive].Live.GraphicsInformation.Name = "OK";//信息名称
                                        }

                                        //
                                    }
                                    else//图像无效
                                    {
                                        Global.VisionSystem.Camera[iCameraIndex_DevicesSetupConfigImageLive].ImageLive = null;//更新数值

                                        Global.VisionSystem.Camera[iCameraIndex_DevicesSetupConfigImageLive].Live.GraphicsInformation = new VisionSystemClassLibrary.Struct.ImageInformation();//图像信息
                                        Global.VisionSystem.Camera[iCameraIndex_DevicesSetupConfigImageLive].Live.GraphicsInformation.Valid = false;//图像信息
                                        Global.VisionSystem.Camera[iCameraIndex_DevicesSetupConfigImageLive].Live.GraphicsInformation.Name = "";//图像信息
                                        Global.VisionSystem.Camera[iCameraIndex_DevicesSetupConfigImageLive].Live.GraphicsInformation.ValueDisplay = false;//图像信息
                                        Global.VisionSystem.Camera[iCameraIndex_DevicesSetupConfigImageLive].Live.GraphicsInformation.Type = VisionSystemClassLibrary.Enum.ImageType.Error;//图像信息
                                        Global.VisionSystem.Camera[iCameraIndex_DevicesSetupConfigImageLive].Live.GraphicsInformation.Value = new Boolean[VisionSystemClassLibrary.Struct.ImageInformation.TotalNumber];//图像信息
                                    }

                                    //

                                    
                                    Global.ImageConfigurationWindow.CustomControl.Invoke(new EventHandler(delegate { Global.ImageConfigurationWindow.CustomControl._UpdateImage(); }));

                                    //

                                    Thread.Sleep(Global.ImageDataTime);//循环数据传输休眠时间10ms

                                    //

                                    if (Global.ImageConfigurationWindow.WindowDisplay)//IMAGE CONFIGURATION页面
                                    {
                                        _SendCommand_Image(CommunicationInstructionType.DevicesSetup_ConfigImage_Live, Global.DevicesSetupWindow.CustomControl.CameraSelected, Global.WorkWindow._GetSelectedCameraIndex(Global.DevicesSetupWindow.CustomControl.CameraSelected));//发送指令
                                    }

                                    //
                                    break;
                                    //
                                case ApplicationInterface.SystemConfiguration://SYSTEM页面
                                    //
                                    break;
                                    //
                                default:
                                    break;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            //不执行操作
                        }
                        //
                        break;
                        //
                    case CommunicationInstructionType.DevicesSetup_AlignDateTime:

                        try
                        {
                            switch (Global.CurrentInterface)
                            {
                                case ApplicationInterface.Work://WORK页面
                                    //
                                    break;
                                //
                                case ApplicationInterface.Load://LOAD页面
                                    //
                                    break;
                                //
                                case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                                    //
                                    break;
                                //
                                case ApplicationInterface.LiveView://LIVE VIEW页面
                                    //
                                    break;
                                //
                                case ApplicationInterface.QualityCheck://QUALITY CHECK页面
                                    //
                                    break;
                                //
                                case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面
                                    //
                                    break;
                                //
                                case ApplicationInterface.DevicesSetup://DEVICES SETUP页面

                                    //DEVICES SETUP页面，点击【ALIGN DATE/TIME】按钮
                                    //客户端->服务端：指令类型 + 相机类型数据 + 日期时间设置结果（1，成功；0，失败）

                                    if (VisionSystemClassLibrary.Enum.CameraType.None < Global.DevicesSetupWindow.CustomControl.CameraSelected) //有相机被选中，默认为上电第一次自动校准时间
                                    {
                                        Global.DevicesSetupWindow.CustomControl.Invoke(new EventHandler(delegate { Global.DevicesSetupWindow.CustomControl._AlignDateTime(Convert.ToBoolean(serverData.ReceivedData[serverData.DataInfo.InstructionIndex + 2])); }));//操作完成
                                    }
                                    break;
                                //
                                case ApplicationInterface.SystemConfiguration://SYSTEM页面
                                    //
                                    break;
                                //
                                default:
                                    break;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            //不执行操作
                        }
                        //
                        break;
                        //
                    case CommunicationInstructionType.DevicesSetup_ParameterSettings:
                        //
                        try
                        {
                            switch (Global.CurrentInterface)
                            {
                                case ApplicationInterface.Work://WORK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.Load://LOAD页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.LiveView://LIVE VIEW页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.QualityCheck://QUALITY CHECK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.DevicesSetup://DEVICES SETUP页面

                                    //DEVICES SETUP页面，点击【PARAMETER SETTINGS】按钮
                                    //客户端->服务端：指令类型 + 相机类型数据 + 设置结果（1，成功；0，失败）

                                    Global.DevicesSetupWindow.CustomControl.Invoke(new EventHandler(delegate { Global.DevicesSetupWindow.CustomControl._ParameterSettings(Convert.ToBoolean(serverData.ReceivedData[serverData.DataInfo.InstructionIndex + 2])); }));//操作完成

                                    break;
                                    //
                                case ApplicationInterface.SystemConfiguration://SYSTEM页面
                                    //
                                    break;
                                    //
                                default:
                                    break;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            //不执行操作
                        }
                        //
                        break;
                        //
                    case CommunicationInstructionType.Work:

                        //WORK页面操作，格式：
                        //客户端->服务端：指令类型 + 相机类型数据 + 图像工具数据（1，包含工具；0，不包含工具） + 图像尺寸类型数据 + 图像宽度数据 + 图像高度数据  + 图像信息数据 + 图像数据

                        VisionSystemClassLibrary.Enum.CameraType Cameratype_Work = VisionSystemClassLibrary.Enum.CameraType.Camera_1;
                        Boolean bViewToolGraphics_Work = true;
                        Double dImageScale_Work = 0.0;
                        Int32 iImageDataLength_Work = 0;
                        VisionSystemClassLibrary.Struct.ImageInformation GraphicsInformation_Work = new VisionSystemClassLibrary.Struct.ImageInformation();
                        MemoryStream imageData_Work = new  MemoryStream();//获取的图像数据
                        Int32 imageWidth_Work = 0;//获取图像宽度
                        Int32 imageHeight_Work = 0;//获取图像高度

                        _GetInstructionData(serverData, ref Cameratype_Work, ref bViewToolGraphics_Work, ref dImageScale_Work, ref iImageDataLength_Work, ref GraphicsInformation_Work, ref imageData_Work,ref imageWidth_Work,ref imageHeight_Work);//解析指令数据

                        Int32 iCameraIndex_Work = _GetSelectedCameraIndex(Cameratype_Work);//相机索引值

                        //

                        try
                        {
                            switch (Global.CurrentInterface)
                            {
                                case ApplicationInterface.Work://WORK页面
                                    //

                                    //_SendCommand_Image(CommunicationInstructionType.Work, Global.VisionSystem.Camera[iCameraIndex_Work].Type, iCameraIndex_Work, Global.ImageScale_Work);//发送指令

                                    //

                                    if (0 < iImageDataLength_Work)//图像有效
                                    {
                                        Bitmap bitmap_Work = new Bitmap(imageData_Work);
                                        BitmapData bitmapData_Work = bitmap_Work.LockBits(new Rectangle(0, 0, imageWidth_Work, imageHeight_Work), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                                        Image<Bgr, Byte> image_Work = new Image<Bgr, Byte>(imageWidth_Work, imageHeight_Work, bitmapData_Work.Stride, bitmapData_Work.Scan0);

                                        Global.VisionSystem.Camera[iCameraIndex_Work].ImageLive = image_Work.Copy();//更新数值

                                        bitmap_Work.UnlockBits(bitmapData_Work);

                                        Global.VisionSystem.Camera[iCameraIndex_Work].Live.GraphicsInformation = GraphicsInformation_Work;//图像信息
                                        if (0 <= GraphicsInformation_Work.ToolsIndex && (VisionSystemClassLibrary.Enum.ImageType.Error == GraphicsInformation_Work.Type))//剔除图像
                                        {
                                            Global.VisionSystem.Camera[iCameraIndex_Work].Live.GraphicsInformation.Name = Global.VisionSystem.Camera[iCameraIndex_Work].Tools[GraphicsInformation_Work.ToolsIndex].Name;//图像信息
                                        }
                                        else//完好图像
                                        {
                                            Global.VisionSystem.Camera[iCameraIndex_Work].Live.GraphicsInformation.Name = "OK";//图像信息
                                        }

                                        if (VisionSystemClassLibrary.Enum.CameraState.ON != (Global.VisionSystem.Camera[iCameraIndex_Work].DeviceInformation.CAM & VisionSystemClassLibrary.Enum.CameraState.ON))//
                                        {
                                            Global.VisionSystem.Camera[iCameraIndex_Work].DeviceInformation.CAM = Global.VisionSystem.Camera[iCameraIndex_Work].DeviceInformation.CAM & (~(VisionSystemClassLibrary.Enum.CameraState.OFF));//相机状态

                                            Global.VisionSystem.Camera[iCameraIndex_Work].DeviceInformation.CAM = Global.VisionSystem.Camera[iCameraIndex_Work].DeviceInformation.CAM | VisionSystemClassLibrary.Enum.CameraState.ON;//相机状态

                                            for (Int32 i = 0; i < Global.VisionSystem.ConnectionData.Length; i++)//遍历设备
                                            {
                                                if (Global.VisionSystem.ConnectionData[i].Type == Global.VisionSystem.Camera[iCameraIndex_Work].Type)//
                                                {
                                                    Global.VisionSystem.ConnectionData[i].CAM = Global.VisionSystem.Camera[iCameraIndex_Work].DeviceInformation.CAM;

                                                    break;
                                                }
                                            }
                                        }

                                        //
                                    }
                                    else//图像无效
                                    {
                                        if (VisionSystemClassLibrary.Enum.CameraState.OFF != (Global.VisionSystem.Camera[iCameraIndex_Work].DeviceInformation.CAM & VisionSystemClassLibrary.Enum.CameraState.OFF))//
                                        {
                                            Global.VisionSystem.Camera[iCameraIndex_Work].DeviceInformation.CAM = Global.VisionSystem.Camera[iCameraIndex_Work].DeviceInformation.CAM & (~(VisionSystemClassLibrary.Enum.CameraState.ON));//相机状态

                                            Global.VisionSystem.Camera[iCameraIndex_Work].DeviceInformation.CAM = Global.VisionSystem.Camera[iCameraIndex_Work].DeviceInformation.CAM | VisionSystemClassLibrary.Enum.CameraState.OFF;//相机状态

                                            for (Int32 i = 0; i < Global.VisionSystem.ConnectionData.Length; i++)//遍历设备
                                            {
                                                if (Global.VisionSystem.ConnectionData[i].Type == Global.VisionSystem.Camera[iCameraIndex_Work].Type)//
                                                {
                                                    Global.VisionSystem.ConnectionData[i].CAM = Global.VisionSystem.Camera[iCameraIndex_Work].DeviceInformation.CAM;

                                                    break;
                                                }
                                            }
                                        }

                                        //

                                        Global.VisionSystem.Camera[iCameraIndex_Work].ImageLive = null;//更新数值

                                        Global.VisionSystem.Camera[iCameraIndex_Work].Live.GraphicsInformation = new VisionSystemClassLibrary.Struct.ImageInformation();//图像信息
                                        Global.VisionSystem.Camera[iCameraIndex_Work].Live.GraphicsInformation.Valid = false;//图像信息
                                        Global.VisionSystem.Camera[iCameraIndex_Work].Live.GraphicsInformation.Name = "";//图像信息
                                        Global.VisionSystem.Camera[iCameraIndex_Work].Live.GraphicsInformation.ValueDisplay = false;//图像信息
                                        Global.VisionSystem.Camera[iCameraIndex_Work].Live.GraphicsInformation.Type = VisionSystemClassLibrary.Enum.ImageType.Error;//图像信息
                                        Global.VisionSystem.Camera[iCameraIndex_Work].Live.GraphicsInformation.Value = new Boolean[VisionSystemClassLibrary.Struct.ImageInformation.TotalNumber];//图像信息
                                    }

                                    if (Global.VisionSystem.Camera[iCameraIndex_Work].DeviceInformation.CAM_Temp != Global.VisionSystem.Camera[iCameraIndex_Work].DeviceInformation.CAM)//相机状态发生变化
                                    {
                                        Global.VisionSystem.Camera[iCameraIndex_Work].DeviceInformation.CAM_Temp = Global.VisionSystem.Camera[iCameraIndex_Work].DeviceInformation.CAM;

                                        workControl.Invoke(new EventHandler(delegate { workControl._SetCameraData(iCameraIndex_Work); }));
                                    }
                                    else//相机状态未发生变化
                                    {
                                        workControl.Invoke(new EventHandler(delegate { workControl._SetCameraImage(iCameraIndex_Work); }));
                                    }

                                    //

                                    Global.WorkWindow.TitleBar.Invoke(new EventHandler(delegate { Global.WorkWindow.TitleBar.PCTime = Global.SystemConfigurationWindow.CustomControl.PCTime; }));//更新时间

                                    Global.WorkWindow.TitleBar.Invoke(new EventHandler(delegate { Global.WorkWindow.TitleBar.CurrentShift = Global._GetShiftInformation(); }));//更新时间

                                    //

                                    Thread.Sleep(Global.ImageDataTime * 5);//循环数据传输休眠时间50ms

                                    //

                                    _SendCommand_Image(CommunicationInstructionType.Work, Global.VisionSystem.Camera[iCameraIndex_Work].Type, iCameraIndex_Work, Global.ImageScale_Work);//发送指令

                                    //

                                    //


                                    break;
                                    //
                                case ApplicationInterface.Load://LOAD页面

                                    _SendCommand_Image(CommunicationInstructionType.Work, Global.VisionSystem.Camera[iCameraIndex_Work].Type, iCameraIndex_Work, Global.ImageScale_Work);//发送指令

                                    break;
                                    //
                                case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.LiveView://LIVE VIEW页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.QualityCheck://QUALITY CHECK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.DevicesSetup://DEVICES SETUP页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.SystemConfiguration://SYSTEM页面
                                    //
                                    break;
                                    //
                                default:
                                    break;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            //不执行操作
                        }
                        //
                        break;
                        //
                    case CommunicationInstructionType.Live:

                        //WORK，LIVE VIEW页面操作，格式：
                        //客户端->服务端：指令类型 + 相机类型数据 + 图像工具数据（1，包含工具；0，不包含工具） + 图像尺寸类型数据 + 图像宽度数据 + 图像高度数据  + 图像信息数据 + 图像数据

                        VisionSystemClassLibrary.Enum.CameraType Cameratype_Live = VisionSystemClassLibrary.Enum.CameraType.Camera_1;
                        Boolean bViewToolGraphics_Live = true;
                        Double dImageScale_Live = 0.0;
                        Int32 iImageDataLength_Live = 0;
                        VisionSystemClassLibrary.Struct.ImageInformation GraphicsInformation_Live = new VisionSystemClassLibrary.Struct.ImageInformation();
                        MemoryStream imageData_Live = new MemoryStream();//获取的图像数据
                        Int32 imageWidth_Live = 0;//获取图像宽度
                        Int32 imageHeight_Live = 0;//获取图像高度

                        _GetInstructionData(serverData, ref Cameratype_Live, ref bViewToolGraphics_Live, ref dImageScale_Live, ref iImageDataLength_Live, ref GraphicsInformation_Live, ref imageData_Live,ref imageWidth_Live,ref imageHeight_Live);//解析指令数据

                        Int32 iCameraIndex_Live = _GetSelectedCameraIndex(Cameratype_Live);//相机索引值

                        //

                        try
                        {
                            switch (Global.CurrentInterface)
                            {
                                case ApplicationInterface.Work://WORK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.Load://LOAD页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.LiveView://LIVE VIEW页面

                                    //

                                    ////if (!(Global.LiveViewWindow.CustomControl.SelfTrigger))//SELF TRIGGER，未开启
                                    ////{
                                    //    _SendCommand_Image(CommunicationInstructionType.Live, Global.VisionSystem.Camera[iCameraIndex_Live].Type, iCameraIndex_Live);//发送指令
                                    ////}

                                    //

                                    VisionSystemClassLibrary.Enum.CameraState CameraState_Live = Global.VisionSystem.Camera[iCameraIndex_Live].DeviceInformation.CAM;//保存当前相机状态

                                    if (0 < iImageDataLength_Live)//图像有效
                                    {
                                        Bitmap bitmap_Live = new Bitmap(imageData_Live);
                                        BitmapData bitmapData_Live = bitmap_Live.LockBits(new Rectangle(0, 0, imageWidth_Live, imageHeight_Live), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                                        Image<Bgr, Byte> image_Live = new Image<Bgr, Byte>(imageWidth_Live, imageHeight_Live, bitmapData_Live.Stride, bitmapData_Live.Scan0);

                                        Global.VisionSystem.Camera[iCameraIndex_Live].ImageLive = image_Live.Copy();//更新数值

                                        bitmap_Live.UnlockBits(bitmapData_Live);

                                        Global.VisionSystem.Camera[iCameraIndex_Live].Live.GraphicsInformation = GraphicsInformation_Live;//图像信息
                                        if (0 <= GraphicsInformation_Live.ToolsIndex && (VisionSystemClassLibrary.Enum.ImageType.Error == GraphicsInformation_Live.Type))//剔除图像
                                        {
                                            Global.VisionSystem.Camera[iCameraIndex_Live].Live.GraphicsInformation.Name = Global.VisionSystem.Camera[iCameraIndex_Live].Tools[GraphicsInformation_Live.ToolsIndex].Name;//图像信息
                                        }
                                        else//完好图像
                                        {
                                            Global.VisionSystem.Camera[iCameraIndex_Live].Live.GraphicsInformation.Name = "OK";//图像信息
                                        }

                                        if (VisionSystemClassLibrary.Enum.CameraState.ON != (Global.VisionSystem.Camera[iCameraIndex_Live].DeviceInformation.CAM & VisionSystemClassLibrary.Enum.CameraState.ON))//
                                        {
                                            Global.VisionSystem.Camera[iCameraIndex_Live].DeviceInformation.CAM = Global.VisionSystem.Camera[iCameraIndex_Live].DeviceInformation.CAM & (~(VisionSystemClassLibrary.Enum.CameraState.OFF));//相机状态

                                            Global.VisionSystem.Camera[iCameraIndex_Live].DeviceInformation.CAM = Global.VisionSystem.Camera[iCameraIndex_Live].DeviceInformation.CAM | VisionSystemClassLibrary.Enum.CameraState.ON;//相机状态

                                            for (Int32 i = 0; i < Global.VisionSystem.ConnectionData.Length; i++)//遍历设备
                                            {
                                                if (Global.VisionSystem.ConnectionData[i].Type == Global.VisionSystem.Camera[iCameraIndex_Live].Type)//
                                                {
                                                    Global.VisionSystem.ConnectionData[i].CAM = Global.VisionSystem.Camera[iCameraIndex_Live].DeviceInformation.CAM;

                                                    break;
                                                }
                                            }
                                        }

                                        //
                                    }
                                    else//图像无效
                                    {
                                        if (VisionSystemClassLibrary.Enum.CameraState.OFF != (Global.VisionSystem.Camera[iCameraIndex_Live].DeviceInformation.CAM & VisionSystemClassLibrary.Enum.CameraState.OFF))//
                                        {
                                            Global.VisionSystem.Camera[iCameraIndex_Live].DeviceInformation.CAM = Global.VisionSystem.Camera[iCameraIndex_Live].DeviceInformation.CAM & (~(VisionSystemClassLibrary.Enum.CameraState.ON));//相机状态

                                            Global.VisionSystem.Camera[iCameraIndex_Live].DeviceInformation.CAM = Global.VisionSystem.Camera[iCameraIndex_Live].DeviceInformation.CAM | VisionSystemClassLibrary.Enum.CameraState.OFF;//相机状态

                                            for (Int32 i = 0; i < Global.VisionSystem.ConnectionData.Length; i++)//遍历设备
                                            {
                                                if (Global.VisionSystem.ConnectionData[i].Type == Global.VisionSystem.Camera[iCameraIndex_Live].Type)//
                                                {
                                                    Global.VisionSystem.ConnectionData[i].CAM = Global.VisionSystem.Camera[iCameraIndex_Live].DeviceInformation.CAM;

                                                    break;
                                                }
                                            }
                                        }

                                        //

                                        Global.VisionSystem.Camera[iCameraIndex_Live].ImageLive = null;//更新数值

                                        Global.VisionSystem.Camera[iCameraIndex_Live].Live.GraphicsInformation = new VisionSystemClassLibrary.Struct.ImageInformation();//图像信息
                                        Global.VisionSystem.Camera[iCameraIndex_Live].Live.GraphicsInformation.Valid = false;//图像信息
                                        Global.VisionSystem.Camera[iCameraIndex_Live].Live.GraphicsInformation.Name = "";//图像信息
                                        Global.VisionSystem.Camera[iCameraIndex_Live].Live.GraphicsInformation.ValueDisplay = false;//图像信息
                                        Global.VisionSystem.Camera[iCameraIndex_Live].Live.GraphicsInformation.Type = VisionSystemClassLibrary.Enum.ImageType.Error;//图像信息
                                        Global.VisionSystem.Camera[iCameraIndex_Live].Live.GraphicsInformation.Value = new Boolean[VisionSystemClassLibrary.Struct.ImageInformation.TotalNumber];//图像信息
                                    }

                                    //

                                    Global.LiveViewWindow.CustomControl.Invoke(new EventHandler(delegate { Global.LiveViewWindow.CustomControl._SetImageData(); }));

                                    //

                                    //Thread.Sleep(Global.ImageDataTime);//循环数据传输休眠时间10ms

                                    //

                                    //if (!(Global.LiveViewWindow.CustomControl.SelfTrigger))//SELF TRIGGER，未开启
                                    //{
                                    _SendCommand_Image(CommunicationInstructionType.Live, Global.VisionSystem.Camera[iCameraIndex_Live].Type, iCameraIndex_Live);//发送指令
                                    //}

                                    //

                                    break;
                                    //
                                case ApplicationInterface.QualityCheck://QUALITY CHECK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.DevicesSetup://DEVICES SETUP页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.SystemConfiguration://SYSTEM页面
                                    //
                                    break;
                                    //
                                default:
                                    break;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            //不执行操作
                        }
                        //
                        break;
                        //
                    case CommunicationInstructionType.TolerancesSettings_Rejects:

                        //TOLERANCES SETTINGS页面，查询REJECTS图像，格式：
                        //客户端->服务端：指令类型 + 相机类型数据 + 图像工具数据（1，包含工具；0，不包含工具） + 图像尺寸类型数据 + 图像宽度数据 + 图像高度数据  + 图像信息数据 + 图像数据

                        VisionSystemClassLibrary.Enum.CameraType Cameratype_TolerancesSettingsRejects = VisionSystemClassLibrary.Enum.CameraType.Camera_1;
                        Boolean bViewToolGraphics_TolerancesSettingsRejects = true;
                        Double dImageScale_TolerancesSettingsRejects = 0.0;
                        Int32 iImageDataLength_TolerancesSettingsRejects = 0;
                        VisionSystemClassLibrary.Struct.ImageInformation GraphicsInformation_TolerancesSettingsRejects = new VisionSystemClassLibrary.Struct.ImageInformation();
                        MemoryStream imageData_TolerancesSettingsRejects = new MemoryStream();//获取的图像数据
                        Int32 imageWidth_TolerancesSettingsRejects = 0;//获取图像宽度
                        Int32 imageHeight_TolerancesSettingsRejects = 0;//获取图像高度

                        _GetInstructionData(serverData, ref Cameratype_TolerancesSettingsRejects, ref bViewToolGraphics_TolerancesSettingsRejects, ref dImageScale_TolerancesSettingsRejects, ref iImageDataLength_TolerancesSettingsRejects, ref GraphicsInformation_TolerancesSettingsRejects, ref imageData_TolerancesSettingsRejects,ref imageWidth_TolerancesSettingsRejects,ref imageHeight_TolerancesSettingsRejects);//解析指令数据
                        
                        //

                        try
                        {
                            switch (Global.CurrentInterface)
                            {
                                case ApplicationInterface.Work://WORK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.Load://LOAD页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.LiveView://LIVE VIEW页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.QualityCheck://QUALITY CHECK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面
                                    //
                                    
                                    //_SendCommand_Value(CommunicationInstructionType.TolerancesSettings_Graphs, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex);//发送指令

                                    //

                                    Int32 iCameraIndex_TolerancesSettingsRejects = _GetSelectedCameraIndex(Cameratype_TolerancesSettingsRejects);//相机索引值

                                    if (0 < iImageDataLength_TolerancesSettingsRejects)//图像有效
                                    {
                                        Bitmap bitmap_TolerancesSettingsRejects = new Bitmap(imageData_TolerancesSettingsRejects);
                                        BitmapData bitmapData_TolerancesSettingsRejects = bitmap_TolerancesSettingsRejects.LockBits(new Rectangle(0, 0, imageWidth_TolerancesSettingsRejects, imageHeight_TolerancesSettingsRejects), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                                        Image<Bgr, Byte> image_TolerancesSettingsRejects = new Image<Bgr, Byte>(imageWidth_TolerancesSettingsRejects, imageHeight_TolerancesSettingsRejects, bitmapData_TolerancesSettingsRejects.Stride, bitmapData_TolerancesSettingsRejects.Scan0);

                                        Global.VisionSystem.Camera[iCameraIndex_TolerancesSettingsRejects].ImageReject = image_TolerancesSettingsRejects.Copy();//更新数值

                                        bitmap_TolerancesSettingsRejects.UnlockBits(bitmapData_TolerancesSettingsRejects);
                                        
                                        Global.VisionSystem.Camera[iCameraIndex_TolerancesSettingsRejects].Rejects.GraphicsInformation = GraphicsInformation_TolerancesSettingsRejects;//图像信息
                                        if (0 <= GraphicsInformation_TolerancesSettingsRejects.ToolsIndex && (VisionSystemClassLibrary.Enum.ImageType.Error == GraphicsInformation_TolerancesSettingsRejects.Type))//剔除图像
                                        {
                                            Global.VisionSystem.Camera[iCameraIndex_TolerancesSettingsRejects].Rejects.GraphicsInformation.Name = Global.VisionSystem.Camera[iCameraIndex_TolerancesSettingsRejects].Tools[GraphicsInformation_TolerancesSettingsRejects.ToolsIndex].Name;//信息名称
                                        }
                                        else//无效
                                        {
                                            Global.VisionSystem.Camera[iCameraIndex_TolerancesSettingsRejects].Rejects.GraphicsInformation.Name = "";//信息名称
                                        }

                                        //
                                    }
                                    else//图像无效
                                    {
                                        Global.VisionSystem.Camera[iCameraIndex_TolerancesSettingsRejects].ImageReject = null;//更新数值

                                        Global.VisionSystem.Camera[iCameraIndex_TolerancesSettingsRejects].Rejects.GraphicsInformation = new VisionSystemClassLibrary.Struct.ImageInformation();//图像信息
                                        Global.VisionSystem.Camera[iCameraIndex_TolerancesSettingsRejects].Rejects.GraphicsInformation.Valid = false;//图像信息
                                        Global.VisionSystem.Camera[iCameraIndex_TolerancesSettingsRejects].Rejects.GraphicsInformation.Name = "";//图像信息
                                        Global.VisionSystem.Camera[iCameraIndex_TolerancesSettingsRejects].Rejects.GraphicsInformation.ValueDisplay = false;//图像信息
                                        Global.VisionSystem.Camera[iCameraIndex_TolerancesSettingsRejects].Rejects.GraphicsInformation.Type = VisionSystemClassLibrary.Enum.ImageType.Error;//图像信息
                                        Global.VisionSystem.Camera[iCameraIndex_TolerancesSettingsRejects].Rejects.GraphicsInformation.Value = new Boolean[VisionSystemClassLibrary.Struct.ImageInformation.TotalNumber];//图像信息
                                    }

                                    //

                                    Global.TolerancesSettingsWindow.CustomControl.Invoke(new EventHandler(delegate { Global.TolerancesSettingsWindow.CustomControl._SetImageData(); }));

                                    //

                                    //Thread.Sleep(Global.ImageDataTime);//循环数据传输休眠时间10ms

                                    //

                                    _SendCommand_Value(CommunicationInstructionType.TolerancesSettings_Graphs, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex);//发送指令

                                    //
                                    
                                    break;
                                    //
                                case ApplicationInterface.DevicesSetup://DEVICES SETUP页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.SystemConfiguration://SYSTEM页面
                                    //
                                    break;
                                    //
                                default:
                                    break;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            //不执行操作
                        }
                        //
                        break;
                        //
                    case CommunicationInstructionType.TolerancesSettings_Live:

                        //TOLERANCES SETTINGS页面，查询LIVE图像，格式：
                        //客户端->服务端：指令类型 + 相机类型数据 + 图像工具数据（1，包含工具；0，不包含工具） + 图像尺寸类型数据 + 图像宽度数据 + 图像高度数据  + 图像信息数据 + 图像数据

                        VisionSystemClassLibrary.Enum.CameraType Cameratype_TolerancesSettingsLive = VisionSystemClassLibrary.Enum.CameraType.Camera_1;
                        Boolean bViewToolGraphics_TolerancesSettingsLive = true;
                        Double dImageScale_TolerancesSettingsLive = 0.0;
                        Int32 iImageDataLength_TolerancesSettingsLive = 0;
                        VisionSystemClassLibrary.Struct.ImageInformation GraphicsInformation_TolerancesSettingsLive = new VisionSystemClassLibrary.Struct.ImageInformation();
                        MemoryStream imageData_TolerancesSettingsLive = new MemoryStream();//获取的图像数据
                        Int32 imageWidth_TolerancesSettingsLive = 0;//获取图像宽度
                        Int32 imageHeight_TolerancesSettingsLive = 0;//获取图像高度

                        _GetInstructionData(serverData, ref Cameratype_TolerancesSettingsLive, ref bViewToolGraphics_TolerancesSettingsLive, ref dImageScale_TolerancesSettingsLive, ref iImageDataLength_TolerancesSettingsLive, ref GraphicsInformation_TolerancesSettingsLive, ref imageData_TolerancesSettingsLive,ref imageWidth_TolerancesSettingsLive,ref imageHeight_TolerancesSettingsLive);//解析指令数据
                        
                        //

                        try
                        {
                            switch (Global.CurrentInterface)
                            {
                                case ApplicationInterface.Work://WORK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.Load://LOAD页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.LiveView://LIVE VIEW页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.QualityCheck://QUALITY CHECK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面
                                    //

                                    //_SendCommand_Value(CommunicationInstructionType.TolerancesSettings_Graphs, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex);//发送指令

                                    //

                                    Int32 iCameraIndex_TolerancesSettingsLive = _GetSelectedCameraIndex(Cameratype_TolerancesSettingsLive);//相机索引值

                                    if (0 < iImageDataLength_TolerancesSettingsLive)//图像有效
                                    {
                                        Bitmap bitmap_TolerancesSettingsLive = new Bitmap(imageData_TolerancesSettingsLive);
                                        BitmapData bitmapData_TolerancesSettingsLive = bitmap_TolerancesSettingsLive.LockBits(new Rectangle(0, 0, imageWidth_TolerancesSettingsLive, imageHeight_TolerancesSettingsLive), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                                        Image<Bgr, Byte> image_Live = new Image<Bgr, Byte>(imageWidth_TolerancesSettingsLive, imageHeight_TolerancesSettingsLive, bitmapData_TolerancesSettingsLive.Stride, bitmapData_TolerancesSettingsLive.Scan0);

                                        Global.VisionSystem.Camera[iCameraIndex_TolerancesSettingsLive].ImageLive = image_Live.Copy();//更新数值

                                        bitmap_TolerancesSettingsLive.UnlockBits(bitmapData_TolerancesSettingsLive);
                                        
                                        Global.VisionSystem.Camera[iCameraIndex_TolerancesSettingsLive].Live.GraphicsInformation = GraphicsInformation_TolerancesSettingsLive;//图像信息
                                        if (0 <= GraphicsInformation_TolerancesSettingsLive.ToolsIndex && (VisionSystemClassLibrary.Enum.ImageType.Error == GraphicsInformation_TolerancesSettingsLive.Type))//剔除图像
                                        {
                                            Global.VisionSystem.Camera[iCameraIndex_TolerancesSettingsLive].Live.GraphicsInformation.Name = Global.VisionSystem.Camera[iCameraIndex_TolerancesSettingsLive].Tools[GraphicsInformation_TolerancesSettingsLive.ToolsIndex].Name;//信息名称
                                        }
                                        else//完好图像
                                        {
                                            Global.VisionSystem.Camera[iCameraIndex_TolerancesSettingsLive].Live.GraphicsInformation.Name = "OK";//信息名称
                                        }

                                        //
                                    }
                                    else//图像无效
                                    {
                                        Global.VisionSystem.Camera[iCameraIndex_TolerancesSettingsLive].ImageLive = null;//更新数值

                                        Global.VisionSystem.Camera[iCameraIndex_TolerancesSettingsLive].Live.GraphicsInformation = new VisionSystemClassLibrary.Struct.ImageInformation();//图像信息
                                        Global.VisionSystem.Camera[iCameraIndex_TolerancesSettingsLive].Live.GraphicsInformation.Valid = false;//图像信息
                                        Global.VisionSystem.Camera[iCameraIndex_TolerancesSettingsLive].Live.GraphicsInformation.Name = "";//图像信息
                                        Global.VisionSystem.Camera[iCameraIndex_TolerancesSettingsLive].Live.GraphicsInformation.ValueDisplay = false;//图像信息
                                        Global.VisionSystem.Camera[iCameraIndex_TolerancesSettingsLive].Live.GraphicsInformation.Type = VisionSystemClassLibrary.Enum.ImageType.Error;//图像信息
                                        Global.VisionSystem.Camera[iCameraIndex_TolerancesSettingsLive].Live.GraphicsInformation.Value = new Boolean[VisionSystemClassLibrary.Struct.ImageInformation.TotalNumber];//图像信息
                                    }

                                    //

                                    Global.TolerancesSettingsWindow.CustomControl.Invoke(new EventHandler(delegate { Global.TolerancesSettingsWindow.CustomControl._SetImageData(); }));

                                    //

                                    //Thread.Sleep(Global.ImageDataTime);//循环数据传输休眠时间10ms

                                    //

                                    _SendCommand_Value(CommunicationInstructionType.TolerancesSettings_Graphs, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex);//发送指令

                                    //

                                    break;
                                    //
                                case ApplicationInterface.DevicesSetup://DEVICES SETUP页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.SystemConfiguration://SYSTEM页面
                                    //
                                    break;
                                    //
                                default:
                                    break;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            //不执行操作
                        }
                        //
                        break;
                        //
                    case CommunicationInstructionType.TolerancesSettings_Graphs:

                        //TOLERANCES SETTINGS页面，查询曲线图数据，格式：
                        //客户端->服务端：指令类型 + 相机类型 + 公差类数据

                        VisionSystemClassLibrary.Enum.CameraType Cameratype_TolerancesSettingsGraphs = VisionSystemClassLibrary.Enum.CameraType.Camera_1;
                        VisionSystemClassLibrary.Struct.TolerancesGraphData_Value[] TolerancesGraphDataValue_TolerancesSettingsGraphs = null;

                        _GetInstructionData(serverData, ref Cameratype_TolerancesSettingsGraphs, ref TolerancesGraphDataValue_TolerancesSettingsGraphs);//解析指令数据

                        Int32 iCameraIndex_TolerancesSettingsGraphs = _GetSelectedCameraIndex(Cameratype_TolerancesSettingsGraphs);//相机索引值

                        if (null != TolerancesGraphDataValue_TolerancesSettingsGraphs)//有效
                        {
                            for (Int32 i = 0; i < TolerancesGraphDataValue_TolerancesSettingsGraphs.Length; i++)//获取数值
                            {
                                Global.VisionSystem.Camera[iCameraIndex_TolerancesSettingsGraphs].Tolerances.GraphData[i].TolerancesGraphDataValue.CurrentValueIndex = TolerancesGraphDataValue_TolerancesSettingsGraphs[i].CurrentValueIndex;//更新数值
                                Global.VisionSystem.Camera[iCameraIndex_TolerancesSettingsGraphs].Tolerances.GraphData[i].TolerancesGraphDataValue.MeanValue = TolerancesGraphDataValue_TolerancesSettingsGraphs[i].MeanValue;//平均值
                                TolerancesGraphDataValue_TolerancesSettingsGraphs[i].Value.CopyTo(Global.VisionSystem.Camera[iCameraIndex_TolerancesSettingsGraphs].Tolerances.GraphData[i].TolerancesGraphDataValue.Value, 0);//更新数值
                            }
                        }

                        //

                        try
                        {
                            switch (Global.CurrentInterface)
                            {
                                case ApplicationInterface.Work://WORK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.Load://LOAD页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.LiveView://LIVE VIEW页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.QualityCheck://QUALITY CHECK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面

                                    if (null != TolerancesGraphDataValue_TolerancesSettingsGraphs)//有效
                                    {
                                        for (Int32 i = 0; i < TolerancesGraphDataValue_TolerancesSettingsGraphs.Length; i++)//获取数值
                                        {
                                            Global.TolerancesSettingsWindow.CustomControl.Invoke(new EventHandler(delegate { Global.TolerancesSettingsWindow.CustomControl._SetCurveValue(i); }));
                                        }
                                    }

                                    //

                                    //Thread.Sleep(Global.ImageDataTime);//循环数据传输休眠时间10ms

                                    //

                                    if (Global.TolerancesSettingsWindow.CustomControl.View)//View Live
                                    {
                                        _SendCommand_Image(CommunicationInstructionType.TolerancesSettings_Live, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex, Global.ImageScale_TolerancesSettings);//发送指令
                                    }
                                    else//View Reject
                                    {
                                        _SendCommand_Image(CommunicationInstructionType.TolerancesSettings_Rejects, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex, Global.ImageScale_TolerancesSettings);//发送指令
                                    }

                                    break;
                                    //
                                case ApplicationInterface.DevicesSetup://DEVICES SETUP页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.SystemConfiguration://SYSTEM页面
                                    //
                                    break;
                                    //
                                default:
                                    break;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            //不执行操作
                        }
                        //
                        break;
                        //
                    case CommunicationInstructionType.TolerancesSettings_Tool:

                        //TOLERANCES SETTINGS页面，工具开关，格式：
                        //客户端->服务端：指令类型 + 相机类型数据 + 工具开关设置结果（1，成功；0，失败）

                        VisionSystemClassLibrary.Enum.CameraType Cameratype_TolerancesSettingsTool = VisionSystemClassLibrary.Enum.CameraType.Camera_1;//相机类型数据
                        Int32 iValue_1_TolerancesSettingsTool = 0;//工具开关设置结果（1，成功；0，失败）

                        _GetInstructionData(serverData, ref Cameratype_TolerancesSettingsTool, ref iValue_1_TolerancesSettingsTool);//解析指令数据

                        if (1 == iValue_1_TolerancesSettingsTool)//成功
                        {
                            //
                        }
                        else//失败
                        {
                            //
                        }

                        //

                        try
                        {
                            switch (Global.CurrentInterface)
                            {
                                case ApplicationInterface.Work://WORK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.Load://LOAD页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.LiveView://LIVE VIEW页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.QualityCheck://QUALITY CHECK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.DevicesSetup://DEVICES SETUP页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.SystemConfiguration://SYSTEM页面
                                    //
                                    break;
                                    //
                                default:
                                    break;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            //不执行操作
                        }
                        //
                        break;
                        //
                    case CommunicationInstructionType.TolerancesSettings_Learn:

                        //TOLERANCES SETTINGS页面，学习，格式：
                        //客户端->服务端：指令类型 + 相机类型数据 + 工具索引数值 + 学习数值 + 学习中的有效数值数量 + 学习中的无效数值数量

                        VisionSystemClassLibrary.Enum.CameraType Cameratype_TolerancesSettingsLearn = VisionSystemClassLibrary.Enum.CameraType.Camera_1;//相机类型数据
                        Int32 iValue_1_TolerancesSettingsLearn = 0;//工具索引数值
                        Int32 iValue_2_TolerancesSettingsLearn = 0;//学习数值
                        Int32 iValue_3_TolerancesSettingsLearn = 0;//学习中的有效数值数量
                        Int32 iValue_4_TolerancesSettingsLearn = 0;//学习中的无效数值数量

                        _GetInstructionData(serverData, ref Cameratype_TolerancesSettingsLearn, ref iValue_1_TolerancesSettingsLearn, ref iValue_2_TolerancesSettingsLearn, ref iValue_3_TolerancesSettingsLearn, ref iValue_4_TolerancesSettingsLearn);//解析指令数据

                        //

                        try
                        {
                            switch (Global.CurrentInterface)
                            {
                                case ApplicationInterface.Work://WORK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.Load://LOAD页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.LiveView://LIVE VIEW页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.QualityCheck://QUALITY CHECK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面

                                    Global.TolerancesSettingsWindow.CustomControl.Invoke(new EventHandler(delegate { Global.TolerancesSettingsWindow.CustomControl._Learn(true, (Int16)iValue_2_TolerancesSettingsLearn, (UInt16)iValue_3_TolerancesSettingsLearn, (UInt16)iValue_4_TolerancesSettingsLearn); }));

                                    break;
                                    //
                                case ApplicationInterface.DevicesSetup://DEVICES SETUP页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.SystemConfiguration://SYSTEM页面
                                    //
                                    break;
                                    //
                                default:
                                    break;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            //不执行操作
                        }
                        //
                        break;
                        //
                    case CommunicationInstructionType.TolerancesSettings_ToolIndex:

                        //TOLERANCES SETTINGS页面，双击选中工具，格式：
                        //客户端->服务端：指令类型 + 相机类型数据 + 工具索引数值 + 设置结果（1，成功；0，失败）

                        VisionSystemClassLibrary.Enum.CameraType Cameratype_TolerancesSettings_ToolIndex = VisionSystemClassLibrary.Enum.CameraType.Camera_1;//相机类型数据
                        Int32 iValue_1_TolerancesSettings_ToolIndex = 0;//工具索引数值
                        Int32 iValue_2_TolerancesSettings_ToolIndex = 0;//最小值最大值设置结果（1，成功；0，失败）

                        _GetInstructionData(serverData, ref Cameratype_TolerancesSettings_ToolIndex, ref iValue_1_TolerancesSettings_ToolIndex, ref iValue_2_TolerancesSettings_ToolIndex);//解析指令数据

                        if (1 == iValue_2_TolerancesSettings_ToolIndex)//成功
                        {
                            //
                        }
                        else//失败
                        {
                            //
                        }

                        //

                        try
                        {
                            switch (Global.CurrentInterface)
                            {
                                case ApplicationInterface.Work://WORK页面
                                    //
                                    break;
                                //
                                case ApplicationInterface.Load://LOAD页面
                                    //
                                    break;
                                //
                                case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                                    //
                                    break;
                                //
                                case ApplicationInterface.LiveView://LIVE VIEW页面
                                    //
                                    break;
                                //
                                case ApplicationInterface.QualityCheck://QUALITY CHECK页面
                                    //
                                    break;
                                //
                                case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面
                                    //
                                    break;
                                //
                                case ApplicationInterface.DevicesSetup://DEVICES SETUP页面
                                    //
                                    break;
                                //
                                case ApplicationInterface.SystemConfiguration://SYSTEM页面
                                    //
                                    break;
                                //
                                default:
                                    break;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            //不执行操作
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.TolerancesSettings_MinMax:

                        //TOLERANCES SETTINGS页面，曲线图范围数值，格式：
                        //客户端->服务端：指令类型 + 相机类型数据 + 工具索引数值 + 最小值最大值设置结果（1，成功；0，失败）

                        VisionSystemClassLibrary.Enum.CameraType Cameratype_TolerancesSettingsMinMax = VisionSystemClassLibrary.Enum.CameraType.Camera_1;//相机类型数据
                        Int32 iValue_1_TolerancesSettingsMinMax = 0;//工具索引数值
                        Int32 iValue_2_TolerancesSettingsMinMax = 0;//最小值最大值设置结果（1，成功；0，失败）

                        _GetInstructionData(serverData, ref Cameratype_TolerancesSettingsMinMax, ref iValue_1_TolerancesSettingsMinMax, ref iValue_2_TolerancesSettingsMinMax);//解析指令数据

                        if (1 == iValue_2_TolerancesSettingsMinMax)//成功
                        {
                            //
                        }
                        else//失败
                        {
                            //
                        }

                        //

                        try
                        {
                            switch (Global.CurrentInterface)
                            {
                                case ApplicationInterface.Work://WORK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.Load://LOAD页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.LiveView://LIVE VIEW页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.QualityCheck://QUALITY CHECK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.DevicesSetup://DEVICES SETUP页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.SystemConfiguration://SYSTEM页面
                                    //
                                    break;
                                    //
                                default:
                                    break;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            //不执行操作
                        }
                        //
                        break;
                        //
                    case CommunicationInstructionType.TolerancesSettings_ResetGraphs:

                        //TOLERANCES SETTINGS页面，复位曲线图，格式：
                        //客户端->服务端：指令类型 + 相机类型数据 + 复位结果（1，成功；0，失败）

                        VisionSystemClassLibrary.Enum.CameraType Cameratype_TolerancesSettingsResetGraphs = VisionSystemClassLibrary.Enum.CameraType.Camera_1;//相机类型数据
                        Int32 iValue_1_TolerancesSettingsResetGraphs = 0;//复位结果（1，成功；0，失败）

                        _GetInstructionData(serverData, ref Cameratype_TolerancesSettingsResetGraphs, ref iValue_1_TolerancesSettingsResetGraphs);//解析指令数据

                        //

                        try
                        {
                            switch (Global.CurrentInterface)
                            {
                                case ApplicationInterface.Work://WORK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.Load://LOAD页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.LiveView://LIVE VIEW页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.QualityCheck://QUALITY CHECK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面

                                    Global.TolerancesSettingsWindow.CustomControl.Invoke(new EventHandler(delegate { Global.TolerancesSettingsWindow.CustomControl._ResetGraphs(Convert.ToBoolean(iValue_1_TolerancesSettingsResetGraphs)); }));

                                    break;
                                    //
                                case ApplicationInterface.DevicesSetup://DEVICES SETUP页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.SystemConfiguration://SYSTEM页面
                                    //
                                    break;
                                    //
                                default:
                                    break;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            //不执行操作
                        }
                        //
                        break;
                        //
                    case CommunicationInstructionType.TolerancesSettings_Enter:

                        //TOLERANCES SETTINGS页面，进入页面，格式：
                        //客户端->服务端：指令类型 + 相机类型数据 + 操作结果（1，成功；0，失败）

                        VisionSystemClassLibrary.Enum.CameraType Cameratype_TolerancesSettingsEnter = VisionSystemClassLibrary.Enum.CameraType.Camera_1;//相机类型数据
                        Int32 iValue_1_TolerancesSettingsEnter = 0;//操作结果（1，成功；0，失败）

                        _GetInstructionData(serverData, ref Cameratype_TolerancesSettingsEnter, ref iValue_1_TolerancesSettingsEnter);//解析指令数据

                        //

                        try
                        {
                            switch (Global.CurrentInterface)
                            {
                                case ApplicationInterface.Work://WORK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.Load://LOAD页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.LiveView://LIVE VIEW页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.QualityCheck://QUALITY CHECK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面

                                    _SendCommand_Value(CommunicationInstructionType.TolerancesSettings_Graphs, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex);//发送指令

                                    break;
                                    //
                                case ApplicationInterface.DevicesSetup://DEVICES SETUP页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.SystemConfiguration://SYSTEM页面
                                    //
                                    break;
                                    //
                                default:
                                    break;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            //不执行操作
                        }
                        //
                        break;
                        //
                    case CommunicationInstructionType.TolerancesSettings_SaveProduct:

                        //TOLERANCES SETTINGS页面，保存数据，格式：
                        //客户端->服务端：指令类型 + 相机类型数据 + 是否保存数据（1，是；0，否） + 保存数据结果（1，成功；0，失败）

                        VisionSystemClassLibrary.Enum.CameraType Cameratype_TolerancesSettingsSaveProduct = VisionSystemClassLibrary.Enum.CameraType.Camera_1;//相机类型数据
                        Int32 iValue_1_TolerancesSettingsSaveProduct = 0;//是否保存数据（1，是；0，否）
                        Int32 iValue_2_TolerancesSettingsSaveProduct = 0;//保存数据结果（1，成功；0，失败）

                        _GetInstructionData(serverData, ref Cameratype_TolerancesSettingsSaveProduct, ref iValue_1_TolerancesSettingsSaveProduct, ref iValue_2_TolerancesSettingsSaveProduct);//解析指令数据

                        Int32 iCameraIndex_TolerancesSettingsSaveProduc = _GetSelectedCameraIndex(Cameratype_TolerancesSettingsSaveProduct);//相机索引值

                        if (1 == iValue_1_TolerancesSettingsSaveProduct)//保存数据
                        {
                            try
                            {
                                switch (Global.CurrentInterface)
                                {
                                    case ApplicationInterface.Work://WORK页面
                                        //
                                        break;
                                        //
                                    case ApplicationInterface.Load://LOAD页面
                                        //
                                        break;
                                        //
                                    case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                                        //
                                        break;
                                        //
                                    case ApplicationInterface.LiveView://LIVE VIEW页面
                                        //
                                        break;
                                        //
                                    case ApplicationInterface.QualityCheck://QUALITY CHECK页面
                                        //
                                        break;
                                        //
                                    case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面

                                        Global.TolerancesSettingsWindow.CustomControl.Invoke(new EventHandler(delegate { Global.TolerancesSettingsWindow.CustomControl._SaveProduct(Convert.ToBoolean(iValue_2_TolerancesSettingsSaveProduct)); }));

                                        break;
                                        //
                                    case ApplicationInterface.DevicesSetup://DEVICES SETUP页面
                                        //
                                        break;
                                        //
                                    case ApplicationInterface.SystemConfiguration://SYSTEM页面
                                        //
                                        break;
                                        //
                                    default:
                                        break;
                                }
                            }
                            catch (System.Exception ex)
                            {
                                //不执行操作
                            }
                        }
                        else//不保存数据
                        {
                            try
                            {
                                switch (Global.CurrentInterface)
                                {
                                    case ApplicationInterface.Work://WORK页面

                                        _SendCommand_Cameras(CommunicationInstructionType.Work, VisionSystemClassLibrary.Enum.CameraType.Camera_1);//发送指令

                                        break;
                                        //
                                    case ApplicationInterface.Load://LOAD页面
                                        //
                                        break;
                                        //
                                    case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                                        //
                                        break;
                                        //
                                    case ApplicationInterface.LiveView://LIVE VIEW页面
                                        //
                                        break;
                                        //
                                    case ApplicationInterface.QualityCheck://QUALITY CHECK页面
                                        //
                                        break;
                                        //
                                    case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面
                                        //
                                        break;
                                        //
                                    case ApplicationInterface.DevicesSetup://DEVICES SETUP页面
                                        //
                                        break;
                                        //
                                    case ApplicationInterface.SystemConfiguration://SYSTEM页面
                                        //
                                        break;
                                        //
                                    default:
                                        break;
                                }
                            }
                            catch (System.Exception ex)
                            {
                                //不执行操作
                            }
                        }
                        //
                        break;
                        //
                    case CommunicationInstructionType.TolerancesSettings_EjectLevel:

                        //TOLERANCES SETTINGS页面，灵敏度，格式：
                        //客户端->服务端：指令类型 + 相机类型数据 + 灵敏度 + 调节灵敏度标记（0,：true;1：调节光电空头校准值） + 公差个数 + （每个）公差下限、上限

                        VisionSystemClassLibrary.Enum.CameraType Cameratype_TolerancesSettingsEjectLevel = VisionSystemClassLibrary.Enum.CameraType.Camera_1;
                        Int32 iEjectLevel_TolerancesSettingsEjectLevel = 5;
                        Int32 iUpdateTolerances_TolerancesSettingsEjectLevel = 0;
                        Int32[] iMin_TolerancesSettingsEjectLevel = null;
                        Int32[] iMax_TolerancesSettingsEjectLevel = null;

                        _GetInstructionData(serverData, ref Cameratype_TolerancesSettingsEjectLevel, ref iEjectLevel_TolerancesSettingsEjectLevel, ref iUpdateTolerances_TolerancesSettingsEjectLevel, ref iMin_TolerancesSettingsEjectLevel, ref iMax_TolerancesSettingsEjectLevel);//解析指令数据

                        Int32 iCameraIndex_TolerancesSettingsEjectLevel = _GetSelectedCameraIndex(Cameratype_TolerancesSettingsEjectLevel);//相机索引值

                        //

                        try
                        {
                            switch (Global.CurrentInterface)
                            {
                                case ApplicationInterface.Work://WORK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.Load://LOAD页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.LiveView://LIVE VIEW页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.QualityCheck://QUALITY CHECK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面

                                    if (null != iMin_TolerancesSettingsEjectLevel && null != iMax_TolerancesSettingsEjectLevel)//有效
                                    {
                                        for (Int32 i = 0; i < iMin_TolerancesSettingsEjectLevel.Length; i++)//获取数值
                                        {
                                            Global.TolerancesSettingsWindow.CustomControl.Invoke(new EventHandler(delegate { Global.TolerancesSettingsWindow.CustomControl._SetEjectLevel(i, iMin_TolerancesSettingsEjectLevel[i], iMax_TolerancesSettingsEjectLevel[i], iUpdateTolerances_TolerancesSettingsEjectLevel); }));
                                        }
                                    }

                                    break;
                                    //
                                case ApplicationInterface.DevicesSetup://DEVICES SETUP页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.SystemConfiguration://SYSTEM页面
                                    //
                                    break;
                                    //
                                default:
                                    break;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            //不执行操作
                        }
                        //
                        break;
                        //
                    case CommunicationInstructionType.BrandManagement_LoadBrand:

                        //BRAND MANAGEMENT页面操作，载入品牌，格式：

                        //未完成文件发送
                        //客户端->服务端（数据）：指令类型 + 相机类型数据 + 设置相机模式 + 品牌名称长度 + 品牌名称 + 文件索引值（从0开始） + 文件传输状态（1，文件发送中；2，文件发送完成） + 文件接收结果（1，成功；0，失败）

                        //完成文件发送
                        //客户端->服务端：指令类型 + 相机类型数据 + 设置相机模式 + 品牌名称长度 + 品牌名称 + 文件索引值（从0开始） + 文件传输状态（1，文件发送中；2，文件发送完成） + 配置结果（1，成功；0，失败）

                        VisionSystemClassLibrary.Enum.CameraType Cameratype_BrandManagementLoadBrand = VisionSystemClassLibrary.Enum.CameraType.Camera_1;//相机类型数据
                        String sBrandName_BrandManagementLoadBrand = "";//品牌名称
                        Int32 iValue_1_BrandManagementLoadBrand = 0;//文件索引值（从0开始）
                        Int32 iValue_2_BrandManagementLoadBrand = 0;//文件传输状态（1，文件发送中；2，文件发送完成）
                        Int32 iValue_3_BrandManagementLoadBrand = 0;//文件接收结果（1，成功；0，失败），配置结果（1，成功；0，失败）
                        Int32 iCameraChooseState_BrandManagementLoadBrand = 0;//相机设置模式

                        _GetInstructionData(serverData, ref Cameratype_BrandManagementLoadBrand, ref sBrandName_BrandManagementLoadBrand, ref iValue_1_BrandManagementLoadBrand, ref iValue_2_BrandManagementLoadBrand, ref iValue_3_BrandManagementLoadBrand, ref iCameraChooseState_BrandManagementLoadBrand);//解析指令数据

                        //

                        Int32 iCameraIndex_BrandManagementLoadBrand = _GetSelectedCameraIndex(Cameratype_BrandManagementLoadBrand);//相机索引

                        String sCameraName_BrandManagementLoadBrand = Global.VisionSystem.Camera[iCameraIndex_BrandManagementLoadBrand].CameraENGName;//相机名称

                        Byte[] BrandManagementLoadBrand_Data = null;//生成的指令数据

                        Byte[] BrandManagementLoadBrand_ClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iCameraIndex_BrandManagementLoadBrand].DeviceInformation.IP).GetAddressBytes();//获取当前选中的相机的IP地址

                        if (1 == iValue_2_BrandManagementLoadBrand)//文件发送中
                        {
                            Boolean bSendfileOrInstruction = true;//发送文件或指令。取值范围：true，文件；false，指令

                            String sSafeFileName_Send = "";//文件名和扩展名，文件名不包含路径
                            String sFileName_Send = "";//文件名和扩展名，文件名包含路径
                            String sFileName = "";//文件名称
                            String sCameraPath = "";//文件路径

                            Boolean bFileSendOK = false;
                            if (Global.VisionSystem.Camera[iCameraIndex_BrandManagementLoadBrand].DeepLearningState)//包含深度学习模块
                            {
                                if (6 == iValue_1_BrandManagementLoadBrand)//文件索引值（从0开始）文件发送完成
                                {
                                    bFileSendOK = true;
                                }
                            }
                            else//常规模块
                            {
                                if (4 == iValue_1_BrandManagementLoadBrand)//文件索引值（从0开始）文件发送完成
                                {
                                    bFileSendOK = true;
                                }
                            }

                            if (bFileSendOK)//文件索引值（从0开始）文件发送完成
                            {
                                bSendfileOrInstruction = false;//发送文件或指令。取值范围：true，文件；false，指令

                                //完成文件发送
                                //服务端->客户端：指令类型 + 相机类型数据 + 设置相机模式 + 品牌名称长度 + 品牌名称

                                BrandManagementLoadBrand_Data = _GenerateInstruction(CommunicationInstructionType.BrandManagement_LoadBrand, Cameratype_BrandManagementLoadBrand, iCameraChooseState_BrandManagementLoadBrand, sBrandName_BrandManagementLoadBrand);//生成指令数据
                            }
                            else//文件发送中
                            {
                                if (0 == iValue_1_BrandManagementLoadBrand)//文件索引值（从0开始）
                                {
                                    iValue_1_BrandManagementLoadBrand++;//更新文件索引值

                                    //

                                    sFileName = VisionSystemClassLibrary.Class.Camera.ToolFileName;//文件名称

                                    sCameraPath = Global.VisionSystem.Brand.BrandPath + sBrandName_BrandManagementLoadBrand + "\\" + sCameraName_BrandManagementLoadBrand + "\\";//文件路径
                                }
                                else if (1 == iValue_1_BrandManagementLoadBrand)//文件索引值（从0开始）
                                {
                                    iValue_1_BrandManagementLoadBrand++;//更新文件索引值

                                    //

                                    sFileName = VisionSystemClassLibrary.Class.Camera.ParameterFileName;//文件名称

                                    sCameraPath = Global.VisionSystem.Brand.BrandPath + sBrandName_BrandManagementLoadBrand + "\\" + sCameraName_BrandManagementLoadBrand + "\\";//文件路径
                                }
                                else if (2 == iValue_1_BrandManagementLoadBrand)//文件索引值（从0开始）
                                {
                                    iValue_1_BrandManagementLoadBrand++;//更新文件索引值

                                    //

                                    sFileName = VisionSystemClassLibrary.Class.Camera.SampleDataFileName;//文件名称

                                    DirectoryInfo directoryinfo = new DirectoryInfo(Global.VisionSystem.Camera[iCameraIndex_BrandManagementLoadBrand].SampleImagePath.Substring(0, Global.VisionSystem.Camera[iCameraIndex_BrandManagementLoadBrand].SampleImagePath.Length - 1));//路径信息
                                    sCameraPath = Global.VisionSystem.Brand.BrandPath + sBrandName_BrandManagementLoadBrand + "\\" + sCameraName_BrandManagementLoadBrand + "\\" + directoryinfo.Name + "\\";//文件路径
                                }
                                else if (3 == iValue_1_BrandManagementLoadBrand)//文件索引值（从0开始）
                                {
                                    iValue_1_BrandManagementLoadBrand++;//更新文件索引值

                                    //

                                    sFileName = VisionSystemClassLibrary.Class.Camera.SampleImageFileName + VisionSystemClassLibrary.Class.Camera.BMPFile;//文件名称

                                    DirectoryInfo directoryinfo = new DirectoryInfo(Global.VisionSystem.Camera[iCameraIndex_BrandManagementLoadBrand].SampleImagePath.Substring(0, Global.VisionSystem.Camera[iCameraIndex_BrandManagementLoadBrand].SampleImagePath.Length - 1));//路径信息
                                    sCameraPath = Global.VisionSystem.Brand.BrandPath + sBrandName_BrandManagementLoadBrand + "\\" + sCameraName_BrandManagementLoadBrand + "\\" + directoryinfo.Name + "\\";//文件路径
                                }
                                else if (4 == iValue_1_BrandManagementLoadBrand)//文件索引值（从0开始）
                                {
                                    iValue_1_BrandManagementLoadBrand++;//更新文件索引值

                                    //

                                    sFileName = VisionSystemClassLibrary.Class.Camera.ClassesFile;//文件名称

                                    sCameraPath = Global.VisionSystem.Brand.BrandPath + sBrandName_BrandManagementLoadBrand + "\\" + sCameraName_BrandManagementLoadBrand + "\\";//文件路径
                                }
                                else //文件索引值（从0开始）
                                {
                                    iValue_1_BrandManagementLoadBrand++;//更新文件索引值

                                    //

                                    sFileName = VisionSystemClassLibrary.Class.Camera.ModelFileName;//文件名称

                                    sCameraPath = Global.VisionSystem.Brand.BrandPath + sBrandName_BrandManagementLoadBrand + "\\" + sCameraName_BrandManagementLoadBrand + "\\";//文件路径
                                }

                                //

                                sSafeFileName_Send = sFileName;//文件名和扩展名，文件名不包含路径
                                sFileName_Send = sCameraPath + sFileName;//文件名和扩展名，文件名包含路径

                                //未完成文件发送
                                //服务端->客户端（文件）：指令类型 + 相机类型数据 + 设置相机模式 + 品牌名称长度 + 品牌名称 + 文件索引值（从0开始） + 文件

                                BrandManagementLoadBrand_Data = _GenerateInstruction(CommunicationInstructionType.BrandManagement_LoadBrand, Cameratype_BrandManagementLoadBrand, iCameraChooseState_BrandManagementLoadBrand, sBrandName_BrandManagementLoadBrand, iValue_1_BrandManagementLoadBrand);//生成指令数据
                            }

                            //

                            try
                            {
                                switch (Global.CurrentInterface)
                                {
                                    case ApplicationInterface.Work://WORK页面
                                        //
                                        break;
                                        //
                                    case ApplicationInterface.Load://LOAD页面
                                        //
                                        break;
                                        //
                                    case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面

                                        if (bSendfileOrInstruction)//发送文件
                                        {
                                            Global.NetServer.ClientData[BrandManagementLoadBrand_ClientIP[3]]._Send(sSafeFileName_Send, sFileName_Send, BrandManagementLoadBrand_Data);//发送文件
                                        }
                                        else//发送指令
                                        {
                                            Global.NetServer.ClientData[BrandManagementLoadBrand_ClientIP[3]]._Send(BrandManagementLoadBrand_Data);//发送指令
                                        }

                                        break;
                                        //
                                    case ApplicationInterface.LiveView://LIVE VIEW页面
                                        //
                                        break;
                                        //
                                    case ApplicationInterface.QualityCheck://QUALITY CHECK页面
                                        //
                                        break;
                                        //
                                    case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面
                                        //
                                        break;
                                        //
                                    case ApplicationInterface.DevicesSetup://DEVICES SETUP页面
                                        //
                                        break;
                                        //
                                    case ApplicationInterface.SystemConfiguration://SYSTEM页面
                                        //
                                        break;
                                        //
                                    default:
                                        break;
                                }
                            }
                            catch (System.Exception ex)
                            {
                                //不执行操作
                            }
                        }
                        else//文件发送完成
                        {
                            try
                            {
                                switch (Global.CurrentInterface)
                                {
                                    case ApplicationInterface.Work://WORK页面
                                        //
                                        break;
                                        //
                                    case ApplicationInterface.Load://LOAD页面
                                        //
                                        break;
                                        //
                                    case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面

                                        if (Global.RestoreBrandsWindow.WindowDisplay)//BRAND MANAGEMENT，RESTORE BRANDS页面
                                        {
                                            Global.RestoreBrandsWindow.CustomControl.Invoke(new EventHandler(delegate { Global.RestoreBrandsWindow.CustomControl._RestoreBrand(Convert.ToBoolean(iValue_3_BrandManagementLoadBrand)); }));
                                        }
                                        else//BRAND MANAGEMENT页面
                                        {
                                            Global.BrandManagementWindow.CustomControl.Invoke(new EventHandler(delegate { Global.BrandManagementWindow.CustomControl._LoadReloadBrand(Convert.ToBoolean(iValue_3_BrandManagementLoadBrand)); }));
                                        }

                                        break;
                                        //
                                    case ApplicationInterface.LiveView://LIVE VIEW页面
                                        //
                                        break;
                                        //
                                    case ApplicationInterface.QualityCheck://QUALITY CHECK页面
                                        //
                                        break;
                                        //
                                    case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面
                                        //
                                        break;
                                        //
                                    case ApplicationInterface.DevicesSetup://DEVICES SETUP页面
                                        //
                                        break;
                                        //
                                    case ApplicationInterface.SystemConfiguration://SYSTEM页面
                                        //
                                        break;
                                        //
                                    default:
                                        break;
                                }
                            }
                            catch (System.Exception ex)
                            {
                                //不执行操作
                            }
                        }
                        //
                        break;
                        //
                    case CommunicationInstructionType.Live_SelfTrigger:

                        //LIVE VIEW页面操作，SELF TRIGGER，格式：
                        //客户端->服务端：指令类型 + 相机类型 + 操作结果（1，成功；0，失败）

                        VisionSystemClassLibrary.Enum.CameraType Cameratype_LiveSelfTrigger = VisionSystemClassLibrary.Enum.CameraType.Camera_1;//相机类型数据
                        Int32 iValue_1_LiveSelfTrigger = 0;//操作结果（1，成功；0，失败）

                        _GetInstructionData(serverData, ref Cameratype_LiveSelfTrigger, ref iValue_1_LiveSelfTrigger);//解析指令数据

                        Int32 iCameraIndex_LiveSelfTrigger = _GetSelectedCameraIndex(Cameratype_LiveSelfTrigger);//相机索引

                        //

                        try
                        {
                            switch (Global.CurrentInterface)
                            {
                                case ApplicationInterface.Work://WORK页面

                                    //_SendCommand_Cameras(CommunicationInstructionType.Work, VisionSystemClassLibrary.Enum.CameraType.Camera_1);//发送指令

                                    break;
                                    //
                                case ApplicationInterface.Load://LOAD页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.LiveView://LIVE VIEW页面
                                    
                                    //
                                    break;
                                    //
                                case ApplicationInterface.QualityCheck://QUALITY CHECK页面
                                    
                                    if (Global.QualityCheckWindow.CustomControl.LiveViewSelected)//【LIVE VIEW】
                                    {
                                        _SendCommand_Image(CommunicationInstructionType.QualityCheck_LiveView, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex, 1.0);//发送指令
                                    }

                                    break;
                                    //
                                case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.DevicesSetup://DEVICES SETUP页面
                                    
                                    if (Global.ImageConfigurationWindow.WindowDisplay)//IMAGE CONFIGURATION页面
                                    {
                                        _SendCommand_Image(CommunicationInstructionType.DevicesSetup_ConfigImage_Live, Global.DevicesSetupWindow.CustomControl.CameraSelected, Global.WorkWindow._GetSelectedCameraIndex(Global.DevicesSetupWindow.CustomControl.CameraSelected));//发送指令
                                    }

                                    break;
                                    //
                                case ApplicationInterface.SystemConfiguration://SYSTEM页面
                                    //
                                    break;
                                    //
                                default:
                                    break;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            //不执行操作
                        }
                        //
                        break;
                        //
                    case CommunicationInstructionType.QualityCheck_SaveProduct:

                        //QUALITY CHECK页面操作，保存数据参数，格式：
                        //客户端->服务端：指令类型 + 相机类型数据 + 保存数据结果（1，成功；0，失败）

                        VisionSystemClassLibrary.Enum.CameraType Cameratype_QualityCheckSaveProduct = VisionSystemClassLibrary.Enum.CameraType.Camera_1;//相机类型数据
                        Int32 iValue_1_QualityCheckSaveProduct = 0;//保存数据结果（1，成功；0，失败）

                        _GetInstructionData(serverData, ref Cameratype_QualityCheckSaveProduct, ref iValue_1_QualityCheckSaveProduct);//解析指令数据

                        Int32 iCameraIndex_QualityCheckSaveProduct = _GetSelectedCameraIndex(Cameratype_QualityCheckSaveProduct);//相机索引

                        if (1 == iValue_1_QualityCheckSaveProduct)//保存数据
                        {
                            try
                            {
                                switch (Global.CurrentInterface)
                                {
                                    case ApplicationInterface.Work://WORK页面
                                        //
                                        break;
                                        //
                                    case ApplicationInterface.Load://LOAD页面
                                        //
                                        break;
                                        //
                                    case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                                        //
                                        break;
                                        //
                                    case ApplicationInterface.LiveView://LIVE VIEW页面
                                        //
                                        break;
                                        //
                                    case ApplicationInterface.QualityCheck://QUALITY CHECK页面

                                        Global.VisionSystem.Camera[Global.VisionSystem.Work.SelectedCameraIndex] = Global.QualityCheckWindow.CustomControl.SelectedCamera;

                                        Global.QualityCheckWindow.CustomControl.Invoke(new EventHandler(delegate { Global.QualityCheckWindow.CustomControl._SaveProduct(Convert.ToBoolean(iValue_1_QualityCheckSaveProduct)); }));

                                        break;
                                        //
                                    case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面
                                        //
                                        break;
                                        //
                                    case ApplicationInterface.DevicesSetup://DEVICES SETUP页面
                                        //
                                        break;
                                        //
                                    case ApplicationInterface.SystemConfiguration://SYSTEM页面
                                        //
                                        break;
                                        //
                                    default:
                                        break;
                                }
                            }
                            catch (System.Exception ex)
                            {
                                //不执行操作
                            }
                        }
                        else//不保存数据
                        {
                            try
                            {
                                switch (Global.CurrentInterface)
                                {
                                    case ApplicationInterface.Work://WORK页面

                                        _SendCommand_Cameras(CommunicationInstructionType.Work, VisionSystemClassLibrary.Enum.CameraType.Camera_1);//发送指令

                                        break;
                                        //
                                    case ApplicationInterface.Load://LOAD页面
                                        //
                                        break;
                                        //
                                    case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                                        //
                                        break;
                                        //
                                    case ApplicationInterface.LiveView://LIVE VIEW页面
                                        //
                                        break;
                                        //
                                    case ApplicationInterface.QualityCheck://QUALITY CHECK页面
                                        //
                                        break;
                                        //
                                    case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面
                                        //
                                        break;
                                        //
                                    case ApplicationInterface.DevicesSetup://DEVICES SETUP页面
                                        //
                                        break;
                                        //
                                    case ApplicationInterface.SystemConfiguration://SYSTEM页面
                                        //
                                        break;
                                        //
                                    default:
                                        break;
                                }
                            }
                            catch (System.Exception ex)
                            {
                                //不执行操作
                            }
                        }
                        //
                        break;
                        //
                    case CommunicationInstructionType.QualityCheck_LearnSample:

                        //QUALITY CHECK页面操作，自学习阈值或更新，格式：
                        //客户端->服务端：指令类型 + 相机类型数据 + 图像工具数据（1，包含工具；0，不包含工具） + 图像尺寸类型数据 + 图像宽度数据 + 图像高度数据 + 图像信息数据 + 图像数据

                        VisionSystemClassLibrary.Enum.CameraType Cameratype_QualityCheckLearnSample = VisionSystemClassLibrary.Enum.CameraType.Camera_1;
                        Boolean bViewToolGraphics_QualityCheckLearnSample = true;
                        Double dImageScale_QualityCheckLearnSample = 0.0;
                        Int32 iImageDataLength_QualityCheckLearnSample = 0;
                        VisionSystemClassLibrary.Struct.ImageInformation GraphicsInformation_QualityCheckLearnSample = new VisionSystemClassLibrary.Struct.ImageInformation();
                        Image<Bgr, Byte> imageData_QualityCheckLearnSample = null;//获取的图像数据

                        _GetInstructionData(serverData, ref Cameratype_QualityCheckLearnSample, ref bViewToolGraphics_QualityCheckLearnSample, ref dImageScale_QualityCheckLearnSample, ref iImageDataLength_QualityCheckLearnSample, ref GraphicsInformation_QualityCheckLearnSample , ref imageData_QualityCheckLearnSample);//解析指令数据

                        Int32 iCameraIndex_QualityCheckLearnSample = _GetSelectedCameraIndex(Cameratype_QualityCheckLearnSample);//相机索引值

                        if (0 < iImageDataLength_QualityCheckLearnSample)//图像有效
                        {
                            Global.VisionSystem.Camera[iCameraIndex_QualityCheckLearnSample].ImageLearn = imageData_QualityCheckLearnSample.Copy();//更新数值
                            GraphicsInformation_QualityCheckLearnSample._CopyTo(Global.VisionSystem.Camera[iCameraIndex_QualityCheckLearnSample].Learn);//图像信息

                            if (0 <= GraphicsInformation_QualityCheckLearnSample.ToolsIndex && (VisionSystemClassLibrary.Enum.ImageType.Error == GraphicsInformation_QualityCheckLearnSample.Type))//剔除图像
                            {
                                Global.VisionSystem.Camera[iCameraIndex_QualityCheckLearnSample].Learn.Name = Global.VisionSystem.Camera[iCameraIndex_QualityCheckLearnSample].Tools[GraphicsInformation_QualityCheckLearnSample.ToolsIndex].Name;//信息名称
                                GraphicsInformation_QualityCheckLearnSample.Name = Global.VisionSystem.Camera[iCameraIndex_QualityCheckLearnSample].Tools[GraphicsInformation_QualityCheckLearnSample.ToolsIndex].Name;//信息名称
                            }
                            else//完好图像
                            {
                                Global.VisionSystem.Camera[iCameraIndex_QualityCheckLearnSample].Live.GraphicsInformation.Name = "OK";//信息名称
                                GraphicsInformation_QualityCheckLearnSample.Name = "OK";//信息名称
                            }

                            //

                            imageData_QualityCheckLearnSample.Dispose();
                        }
                        else//图像无效
                        {
                            Global.VisionSystem.Camera[iCameraIndex_QualityCheckLearnSample].ImageLearn = null;//更新数值

                            Global.VisionSystem.Camera[iCameraIndex_QualityCheckLearnSample].Learn = new VisionSystemClassLibrary.Struct.ImageInformation();//图像信息
                            Global.VisionSystem.Camera[iCameraIndex_QualityCheckLearnSample].Learn.Valid = false;//图像信息
                            Global.VisionSystem.Camera[iCameraIndex_QualityCheckLearnSample].Learn.Name = "";//图像信息
                            Global.VisionSystem.Camera[iCameraIndex_QualityCheckLearnSample].Learn.ValueDisplay = false;//图像信息
                            Global.VisionSystem.Camera[iCameraIndex_QualityCheckLearnSample].Learn.Type = VisionSystemClassLibrary.Enum.ImageType.Error;//图像信息
                            Global.VisionSystem.Camera[iCameraIndex_QualityCheckLearnSample].Learn.Value = new Boolean[VisionSystemClassLibrary.Struct.ImageInformation.TotalNumber];//图像信息
                        }

                        //

                        try
                        {
                            switch (Global.CurrentInterface)
                            {
                                case ApplicationInterface.Work://WORK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.Load://LOAD页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.LiveView://LIVE VIEW页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.QualityCheck://QUALITY CHECK页面

                                    Global.QualityCheckWindow.CustomControl.Invoke(new EventHandler(delegate { Global.QualityCheckWindow.CustomControl._LearnSample(true, GraphicsInformation_QualityCheckLearnSample); }));

                                    _SendCommand_Value(CommunicationInstructionType.QualityCheck_TolerancesValue, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex);//发送指令

                                    break;
                                    //
                                case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.DevicesSetup://DEVICES SETUP页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.SystemConfiguration://SYSTEM页面
                                    //
                                    break;
                                    //
                                default:
                                    break;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            //不执行操作
                        }
                        //
                        break;
                        //

                    case CommunicationInstructionType.QualityCheck_TolerancesValue:

                        //QUALITY CHECK页面，图像学习完成后，获取公差上下限，格式：
                        //客户端->服务端：指令类型 + 相机类型数据 + 公差个数 + （每个）公差下限、上限

                        VisionSystemClassLibrary.Enum.CameraType Cameratype_QualityCheckTolerancesValue = VisionSystemClassLibrary.Enum.CameraType.Camera_1;
                        Int32[] iMin_QualityCheckTolerancesValue = null;
                        Int32[] iMax_QualityCheckTolerancesValue = null;

                        _GetInstructionData(serverData, ref Cameratype_QualityCheckTolerancesValue, ref iMin_QualityCheckTolerancesValue, ref iMax_QualityCheckTolerancesValue);//解析指令数据

                        Int32 iCameraIndex_QualityCheckTolerancesValue = _GetSelectedCameraIndex(Cameratype_QualityCheckTolerancesValue);//相机索引值

                        //

                        try
                        {
                            switch (Global.CurrentInterface)
                            {
                                case ApplicationInterface.Work://WORK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.Load://LOAD页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.LiveView://LIVE VIEW页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.QualityCheck://QUALITY CHECK页面

                                    if (null != iMin_QualityCheckTolerancesValue && null != iMax_QualityCheckTolerancesValue)//有效
                                    {
                                        for (Int32 i = 0; i < iMin_QualityCheckTolerancesValue.Length; i++)//获取数值
                                        {
                                            Global.QualityCheckWindow.CustomControl.Invoke(new EventHandler(delegate { Global.QualityCheckWindow.CustomControl._SetTolerancesValue(i, iMin_QualityCheckTolerancesValue[i], iMax_QualityCheckTolerancesValue[i]); }));
                                        }
                                    }

                                    break;
                                    //
                                case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.DevicesSetup://DEVICES SETUP页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.SystemConfiguration://SYSTEM页面
                                    //
                                    break;
                                    //
                                default:
                                    break;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            //不执行操作
                        }
                        //
                        break;
                        //
                    case CommunicationInstructionType.QualityCheck_LoadSample:

                        //QUALITY CHECK页面操作，自学习阈值或更新，格式：
                        //客户端->服务端：指令类型 + 相机类型数据 + 图像工具数据（1，包含工具；0，不包含工具） + 图像尺寸类型数据 + 图像宽度数据 + 图像高度数据 + 图像信息数据 + 图像数据

                        VisionSystemClassLibrary.Enum.CameraType Cameratype_QualityCheckLoadSample = VisionSystemClassLibrary.Enum.CameraType.Camera_1;
                        Boolean bViewToolGraphics_QualityCheckLoadSample = true;
                        Double dImageScale_QualityCheckLoadSample = 0.0;
                        Int32 iImageDataLength_QualityCheckLoadSample = 0;
                        VisionSystemClassLibrary.Struct.ImageInformation GraphicsInformation_QualityCheckLoadSample = new VisionSystemClassLibrary.Struct.ImageInformation();
                        Image<Bgr, Byte> imageData_QualityCheckLoadSample = null;//获取的图像数据

                        _GetInstructionData(serverData, ref Cameratype_QualityCheckLoadSample, ref bViewToolGraphics_QualityCheckLoadSample, ref dImageScale_QualityCheckLoadSample, ref iImageDataLength_QualityCheckLoadSample, ref GraphicsInformation_QualityCheckLoadSample, ref imageData_QualityCheckLoadSample);//解析指令数据

                        Int32 iCameraIndex_QualityCheckLoadSample = _GetSelectedCameraIndex(Cameratype_QualityCheckLoadSample);//相机索引值

                        if (0 <= GraphicsInformation_QualityCheckLoadSample.ToolsIndex && (VisionSystemClassLibrary.Enum.ImageType.Error == GraphicsInformation_QualityCheckLoadSample.Type))//剔除图像
                        {
                            GraphicsInformation_QualityCheckLoadSample.Name = Global.VisionSystem.Camera[iCameraIndex_QualityCheckLoadSample].Tools[GraphicsInformation_QualityCheckLoadSample.ToolsIndex].Name;//图像信息
                        }
                        else//完好图像
                        {
                            GraphicsInformation_QualityCheckLoadSample.Name = "OK";//图像信息
                        }
                        
                        //

                        try
                        {
                            switch (Global.CurrentInterface)
                            {
                                case ApplicationInterface.Work://WORK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.Load://LOAD页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.LiveView://LIVE VIEW页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.QualityCheck://QUALITY CHECK页面

                                    Global.QualityCheckWindow.CustomControl.Invoke(new EventHandler(delegate { Global.QualityCheckWindow.CustomControl._UpdateInterface(GraphicsInformation_QualityCheckLoadSample, 2); }));

                                    break;
                                    //
                                case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.DevicesSetup://DEVICES SETUP页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.SystemConfiguration://SYSTEM页面
                                    //
                                    break;
                                    //
                                default:
                                    break;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            //不执行操作
                        }
                        //
                        break;
                        //
                    case CommunicationInstructionType.QualityCheck_GetSelectedRecordData:

                        //QUALITY CHECK页面操作，查询当班缺陷图像，格式：
                        //客户端->服务端：指令类型 + 相机类型数据 + 关联标记 + 统计数据类型（0，最新统计数据；1，指定统计数据） + 班次索引（非0） + 统计数据开始结束时间  +  剔除数量

                        VisionSystemClassLibrary.Enum.CameraType Cameratype_QualityCheck_GetSelectedRecordData = VisionSystemClassLibrary.Enum.CameraType.Camera_1;
                        Int32 iStatisticsDataType_QualityCheck_GetSelectedRecordData = 0;//统计数据类型（0，当前班；1，历史班）
                        Boolean bRelevancy_QualityCheck_GetSelectedRecordData = false;//关联标记
                        Int32 iCurrentIndex_QualityCheck_GetSelectedRecordData = 1;//班次索引
                        VisionSystemClassLibrary.Struct.ShiftTime sShiftTime_GetSelectedRecordData = new VisionSystemClassLibrary.Struct.ShiftTime();//班次起止时间
                        Int32 iRejectImageNumber_QualityCheck_GetSelectedRecordData = 0;//剔除数量

                        _GetInstructionData(serverData, ref Cameratype_QualityCheck_GetSelectedRecordData, ref bRelevancy_QualityCheck_GetSelectedRecordData, ref iStatisticsDataType_QualityCheck_GetSelectedRecordData, ref iCurrentIndex_QualityCheck_GetSelectedRecordData, ref sShiftTime_GetSelectedRecordData, ref iRejectImageNumber_QualityCheck_GetSelectedRecordData);//解析指令数据

                        Int32 iCameraIndex_QualityCheck_GetSelectedRecordData = _GetSelectedCameraIndex(Cameratype_QualityCheck_GetSelectedRecordData);//相机索引值

                        //

                        try
                        {
                            switch (Global.CurrentInterface)
                            {
                                case ApplicationInterface.Work://WORK页面
                                    //
                                    break;
                                //
                                case ApplicationInterface.Load://LOAD页面
                                    //
                                    break;
                                //
                                case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                                    //
                                    break;
                                //
                                case ApplicationInterface.LiveView://LIVE VIEW页面
                                    //
                                    break;
                                //
                                case ApplicationInterface.QualityCheck://QUALITY CHECK页面

                                    Global.QualityCheckWindow.CustomControl.Invoke(new EventHandler(delegate { Global.QualityCheckWindow.CustomControl._UpdateShiftInfo(bRelevancy_QualityCheck_GetSelectedRecordData, iCurrentIndex_QualityCheck_GetSelectedRecordData, sShiftTime_GetSelectedRecordData, iRejectImageNumber_QualityCheck_GetSelectedRecordData); }));

                                    _SendCommand_Value(CommunicationInstructionType.QualityCheck_LoadReject_Click, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex, bRelevancy_QualityCheck_GetSelectedRecordData, iCurrentIndex_QualityCheck_GetSelectedRecordData, sShiftTime_GetSelectedRecordData, -1, Global.QualityCheckWindow.CustomControl.RejectImageIndex, 1.0);
                                    
                                    break;
                                //
                                case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面
                                    //
                                    break;
                                //
                                case ApplicationInterface.DevicesSetup://DEVICES SETUP页面
                                    //
                                    break;
                                //
                                case ApplicationInterface.SystemConfiguration://SYSTEM页面
                                    //
                                    break;
                                //
                                default:
                                    break;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            //不执行操作
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.QualityCheck_LiveView:

                        //QUALITY CHECK页面操作，查看实时图像，格式：
                        //客户端->服务端：指令类型 + 相机类型数据 + 图像工具数据（1，包含工具；0，不包含工具） + 图像尺寸类型数据 + 图像宽度数据 + 图像高度数据 + 图像信息数据 + 图像数据

                        VisionSystemClassLibrary.Enum.CameraType Cameratype_QualityCheckLiveView = VisionSystemClassLibrary.Enum.CameraType.Camera_1;
                        Boolean bViewToolGraphics_QualityCheckLiveView = true;
                        Double dImageScale_QualityCheckLiveView = 0.0;
                        Int32 iImageDataLength_QualityCheckLiveView = 0;
                        VisionSystemClassLibrary.Struct.ImageInformation GraphicsInformation_QualityCheckLiveView = new VisionSystemClassLibrary.Struct.ImageInformation();
                        Image<Bgr, Byte> imageData_QualityCheckLiveView = null;//获取的图像数据

                        _GetInstructionData(serverData, ref Cameratype_QualityCheckLiveView, ref bViewToolGraphics_QualityCheckLiveView, ref dImageScale_QualityCheckLiveView, ref iImageDataLength_QualityCheckLiveView, ref GraphicsInformation_QualityCheckLiveView, ref imageData_QualityCheckLiveView);//解析指令数据

                        Int32 iCameraIndex_QualityCheckLiveView = _GetSelectedCameraIndex(Cameratype_QualityCheckLiveView);//相机索引值

                        if (0 < iImageDataLength_QualityCheckLiveView)//图像有效
                        {
                            Global.VisionSystem.Camera[iCameraIndex_QualityCheckLiveView].ImageLive = imageData_QualityCheckLiveView.Copy();//更新数值
                            Global.VisionSystem.Camera[iCameraIndex_QualityCheckLiveView].Live.GraphicsInformation = GraphicsInformation_QualityCheckLiveView;//图像信息
                            if (0 <= GraphicsInformation_QualityCheckLiveView.ToolsIndex && (VisionSystemClassLibrary.Enum.ImageType.Error == GraphicsInformation_QualityCheckLiveView.Type))//剔除图像
                            {
                                Global.VisionSystem.Camera[iCameraIndex_QualityCheckLiveView].Live.GraphicsInformation.Name = Global.VisionSystem.Camera[iCameraIndex_QualityCheckLiveView].Tools[GraphicsInformation_QualityCheckLiveView.ToolsIndex].Name;//信息名称
                                GraphicsInformation_QualityCheckLiveView.Name = Global.VisionSystem.Camera[iCameraIndex_QualityCheckLiveView].Tools[GraphicsInformation_QualityCheckLiveView.ToolsIndex].Name;//信息名称
                            }
                            else//完好图像
                            {
                                Global.VisionSystem.Camera[iCameraIndex_QualityCheckLiveView].Live.GraphicsInformation.Name = "OK";//信息名称
                                GraphicsInformation_QualityCheckLiveView.Name = "OK";//信息名称
                            }

                            //

                            imageData_QualityCheckLiveView.Dispose();
                        }
                        else//图像无效
                        {
                            Global.VisionSystem.Camera[iCameraIndex_QualityCheckLiveView].ImageLive = null;//更新数值

                            Global.VisionSystem.Camera[iCameraIndex_QualityCheckLiveView].Live.GraphicsInformation = new VisionSystemClassLibrary.Struct.ImageInformation();//图像信息
                            Global.VisionSystem.Camera[iCameraIndex_QualityCheckLiveView].Live.GraphicsInformation.Valid = false;//图像信息
                            Global.VisionSystem.Camera[iCameraIndex_QualityCheckLiveView].Live.GraphicsInformation.Name = "";//图像信息
                            Global.VisionSystem.Camera[iCameraIndex_QualityCheckLiveView].Live.GraphicsInformation.ValueDisplay = false;//图像信息
                            Global.VisionSystem.Camera[iCameraIndex_QualityCheckLiveView].Live.GraphicsInformation.Type = VisionSystemClassLibrary.Enum.ImageType.Error;//图像信息
                            Global.VisionSystem.Camera[iCameraIndex_QualityCheckLiveView].Live.GraphicsInformation.Value = new Boolean[VisionSystemClassLibrary.Struct.ImageInformation.TotalNumber];//图像信息

                            GraphicsInformation_QualityCheckLiveView.Name = "";//信息名称
                        }

                        //

                        try
                        {
                            switch (Global.CurrentInterface)
                            {
                                case ApplicationInterface.Work://WORK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.Load://LOAD页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.LiveView://LIVE VIEW页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.QualityCheck://QUALITY CHECK页面

                                    Global.QualityCheckWindow.CustomControl.Invoke(new EventHandler(delegate { Global.QualityCheckWindow.CustomControl._UpdateInterface(GraphicsInformation_QualityCheckLiveView, 1); }));

                                    //

                                    //Thread.Sleep(Global.ImageDataTime);//循环数据传输休眠时间10ms

                                    //

                                    //if (Global.QualityCheckWindow.CustomControl.LiveViewSelected)//【LIVE VIEW】
                                    //{
                                    if ((false == Global.QualityCheckWindow.CustomControl.LoadRejectSelected) && (false == Global.QualityCheckWindow.CustomControl.LoadSampleSelected))//【LOAD REJECT】和【LOAD Sample】未按下
                                    {
                                        _SendCommand_Image(CommunicationInstructionType.QualityCheck_LiveView, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex, dImageScale_QualityCheckLiveView);//发送指令
                                    }
                                    //}

                                    //

                                    break;
                                    //
                                case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.DevicesSetup://DEVICES SETUP页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.SystemConfiguration://SYSTEM页面
                                    //
                                    break;
                                    //
                                default:
                                    break;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            //不执行操作
                        }
                        //
                        break;
                        //
                    case CommunicationInstructionType.QualityCheck_LoadReject_Click:

                        //QUALITY CHECK页面操作，查看缺陷图像，格式：
                        //客户端->服务端：指令类型 + 相机类型数据 + 关联标记 + 班次索引（非0） + 统计数据开始结束时间 + 剔除图像对应的工具索引值（从0开始） + 剔除图像对应的工具中的索引值（从0开始） + 图像尺寸类型数据 + 图像宽度数据 + 图像高度数据 + 图像信息数据 + 图像数据

                        VisionSystemClassLibrary.Enum.CameraType Cameratype_QualityCheckLoadRejectClick = VisionSystemClassLibrary.Enum.CameraType.Camera_1;
                        Boolean bRelevancy_QualityCheckLoadRejectClick = false;
                        Int32 iShiftIndex_QualityCheckLoadRejectClick = 0;//班次索引（从0开始）
                        VisionSystemClassLibrary.Struct.ShiftTime ShiftTime_QualityCheckLoadRejectClick = new VisionSystemClassLibrary.Struct.ShiftTime();//统计数据开始结束时间
                        Int32 iToolIndex_QualityCheckLoadRejectClick = 0;//剔除图像对应的工具索引值（从0开始）
                        Int32 iImageIndex_QualityCheckLoadRejectClick = 0;//剔除图像对应的工具中的索引值（从0开始）
                        Double dImageScale_QualityCheckLoadRejectClick = 0.0;
                        Int32 iImageDataLength_QualityCheckLoadRejectClick = 0;
                        VisionSystemClassLibrary.Struct.ImageInformation GraphicsInformation_QualityCheckLoadRejectClick = new VisionSystemClassLibrary.Struct.ImageInformation();
                        Image<Bgr, Byte> imageData_QualityCheckLoadRejectClick = null;//获取的图像数据

                        _GetInstructionData(serverData, ref Cameratype_QualityCheckLoadRejectClick, ref bRelevancy_QualityCheckLoadRejectClick, ref iShiftIndex_QualityCheckLoadRejectClick, ref ShiftTime_QualityCheckLoadRejectClick, ref iToolIndex_QualityCheckLoadRejectClick, ref iImageIndex_QualityCheckLoadRejectClick, ref dImageScale_QualityCheckLoadRejectClick, ref iImageDataLength_QualityCheckLoadRejectClick, ref GraphicsInformation_QualityCheckLoadRejectClick, ref imageData_QualityCheckLoadRejectClick);//解析指令数据

                        Int32 iCameraIndex_QualityCheckLoadRejectClick = _GetSelectedCameraIndex(Cameratype_QualityCheckLoadRejectClick);//相机索引值

                        //

                        if (0 < iImageDataLength_QualityCheckLoadRejectClick)//图像有效
                        {
                            Global.VisionSystem.Camera[iCameraIndex_QualityCheckLoadRejectClick].ImageReject = imageData_QualityCheckLoadRejectClick.Copy();//更新数值
                            Global.VisionSystem.Camera[iCameraIndex_QualityCheckLoadRejectClick].Rejects.GraphicsInformation = GraphicsInformation_QualityCheckLoadRejectClick;//图像信息
                            if (0 <= GraphicsInformation_QualityCheckLoadRejectClick.ToolsIndex && (VisionSystemClassLibrary.Enum.ImageType.Error == GraphicsInformation_QualityCheckLoadRejectClick.Type))//剔除图像
                            {
                                Global.VisionSystem.Camera[iCameraIndex_QualityCheckLoadRejectClick].Rejects.GraphicsInformation.Name = Global.VisionSystem.Camera[iCameraIndex_QualityCheckLoadRejectClick].Tools[GraphicsInformation_QualityCheckLoadRejectClick.ToolsIndex].Name;//信息名称
                            }
                            else//完好图像
                            {
                                Global.VisionSystem.Camera[iCameraIndex_QualityCheckLoadRejectClick].Rejects.GraphicsInformation.Name = "OK";//信息名称
                            }
                            imageData_QualityCheckLoadRejectClick.Dispose();
                        }
                        else//图像无效
                        {
                            Global.VisionSystem.Camera[iCameraIndex_QualityCheckLoadRejectClick].ImageReject = null;//更新数值

                            Global.VisionSystem.Camera[iCameraIndex_QualityCheckLoadRejectClick].Rejects.GraphicsInformation = new VisionSystemClassLibrary.Struct.ImageInformation();//图像信息
                            Global.VisionSystem.Camera[iCameraIndex_QualityCheckLoadRejectClick].Rejects.GraphicsInformation.Valid = false;//图像信息
                            Global.VisionSystem.Camera[iCameraIndex_QualityCheckLoadRejectClick].Rejects.GraphicsInformation.Name = "";//图像信息
                            Global.VisionSystem.Camera[iCameraIndex_QualityCheckLoadRejectClick].Rejects.GraphicsInformation.ValueDisplay = false;//图像信息
                            Global.VisionSystem.Camera[iCameraIndex_QualityCheckLoadRejectClick].Rejects.GraphicsInformation.Type = VisionSystemClassLibrary.Enum.ImageType.Error;//图像信息
                            Global.VisionSystem.Camera[iCameraIndex_QualityCheckLoadRejectClick].Rejects.GraphicsInformation.Value = new Boolean[VisionSystemClassLibrary.Struct.ImageInformation.TotalNumber];//图像信息
                        }
                        
                        //

                        try
                        {
                            switch (Global.CurrentInterface)
                            {
                                case ApplicationInterface.Work://WORK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.Load://LOAD页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.LiveView://LIVE VIEW页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.QualityCheck://QUALITY CHECK页面

                                    Global.QualityCheckWindow.CustomControl.Invoke(new EventHandler(delegate { Global.QualityCheckWindow.CustomControl._UpdateInterface(Global.VisionSystem.Camera[iCameraIndex_QualityCheckLoadRejectClick].Rejects.GraphicsInformation, 3); }));

                                    break;
                                    //
                                case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.DevicesSetup://DEVICES SETUP页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.SystemConfiguration://SYSTEM页面
                                    //
                                    break;
                                    //
                                default:
                                    break;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            //不执行操作
                        }
                        //
                        break;
                        //
                    case CommunicationInstructionType.QualityCheck_ManageTools:

                        //QUALITY CHECK页面操作，工具管理，格式：
                        //客户端->服务端：指令类型 + 相机类型数据 + 启用工具标记更新结果（1，成功；0，不成功） + 图像信息数据

                        VisionSystemClassLibrary.Enum.CameraType Cameratype_QualityCheckManageTools = VisionSystemClassLibrary.Enum.CameraType.Camera_1;//相机类型数据
                        Int32 iValue_1_QualityCheckManageTools = 0;//启用工具标记更新结果（1，成功；0，不成功）
                        VisionSystemClassLibrary.Struct.ImageInformation ImageInformation_QualityCheckManageTools = new VisionSystemClassLibrary.Struct.ImageInformation();//图像信息数据

                        _GetInstructionData(serverData, ref Cameratype_QualityCheckManageTools, ref iValue_1_QualityCheckManageTools, ref ImageInformation_QualityCheckManageTools);//解析指令数据

                        Int32 iCameraIndex_QualityCheckManageTools = _GetSelectedCameraIndex(Cameratype_QualityCheckManageTools);//相机索引值

                        if (0 <= ImageInformation_QualityCheckManageTools.ToolsIndex && (VisionSystemClassLibrary.Enum.ImageType.Error == ImageInformation_QualityCheckManageTools.Type))//剔除图像
                        {
                            ImageInformation_QualityCheckManageTools.Name = Global.VisionSystem.Camera[iCameraIndex_QualityCheckManageTools].Tools[ImageInformation_QualityCheckManageTools.ToolsIndex].Name;//图像信息
                        }
                        else//完好图像
                        {
                            ImageInformation_QualityCheckManageTools.Name = "OK";//图像信息
                        }

                        if (1 == iValue_1_QualityCheckManageTools)//保存数据
                        {
                            //
                        }

                        //

                        try
                        {
                            switch (Global.CurrentInterface)
                            {
                                case ApplicationInterface.Work://WORK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.Load://LOAD页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.LiveView://LIVE VIEW页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.QualityCheck://QUALITY CHECK页面

                                    Global.QualityCheckWindow.CustomControl.Invoke(new EventHandler(delegate { Global.QualityCheckWindow.CustomControl._UpdateInterface(ImageInformation_QualityCheckManageTools); }));

                                    break;
                                    //
                                case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.DevicesSetup://DEVICES SETUP页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.SystemConfiguration://SYSTEM页面
                                    //
                                    break;
                                    //
                                default:
                                    break;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            //不执行操作
                        }
                        //
                        break;
                        //
                    case CommunicationInstructionType.QualityCheck_ToolParamter:

                        //QUALITY CHECK页面操作，工具参数，格式：
                        //客户端->服务端：指令类型 + 相机类型数据 + 工具参数更新结果（1，成功；0，不成功） + 图像信息数据

                        VisionSystemClassLibrary.Enum.CameraType Cameratype_QualityCheckToolParamter = VisionSystemClassLibrary.Enum.CameraType.Camera_1;//相机类型数据
                        Int32 iValue_1_QualityCheckToolParamter = 0;//工具参数更新结果（1，成功；0，不成功）
                        VisionSystemClassLibrary.Struct.ImageInformation ImageInformation_QualityCheckToolParamter = new VisionSystemClassLibrary.Struct.ImageInformation();//图像信息数据

                        _GetInstructionData(serverData, ref Cameratype_QualityCheckToolParamter, ref iValue_1_QualityCheckToolParamter, ref ImageInformation_QualityCheckToolParamter);//解析指令数据

                        Int32 iCameraIndex_QualityCheckToolParamter = _GetSelectedCameraIndex(Cameratype_QualityCheckToolParamter);//相机索引值

                        if (0 <= ImageInformation_QualityCheckToolParamter.ToolsIndex && (VisionSystemClassLibrary.Enum.ImageType.Error == ImageInformation_QualityCheckToolParamter.Type))//剔除图像
                        {
                            ImageInformation_QualityCheckToolParamter.Name = Global.VisionSystem.Camera[iCameraIndex_QualityCheckToolParamter].Tools[ImageInformation_QualityCheckToolParamter.ToolsIndex].Name;//图像信息
                        }
                        else//完好图像
                        {
                            ImageInformation_QualityCheckToolParamter.Name = "OK";//图像信息
                        }

                        if (1 == iValue_1_QualityCheckToolParamter)//保存数据
                        {
                            //
                        }

                        //

                        try
                        {
                            switch (Global.CurrentInterface)
                            {
                                case ApplicationInterface.Work://WORK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.Load://LOAD页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.LiveView://LIVE VIEW页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.QualityCheck://QUALITY CHECK页面

                                    Global.QualityCheckWindow.CustomControl.Invoke(new EventHandler(delegate { Global.QualityCheckWindow.CustomControl._UpdateInterface(ImageInformation_QualityCheckToolParamter); }));

                                    break;
                                    //
                                case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.DevicesSetup://DEVICES SETUP页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.SystemConfiguration://SYSTEM页面
                                    //
                                    break;
                                    //
                                default:
                                    break;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            //不执行操作
                        }
                        //
                        break;
                        //
                    case CommunicationInstructionType.QualityCheck_CurrentTool:

                        //QUALITY CHECK页面操作，当前工具设置，格式：
                        //客户端->服务端：指令类型 + 相机类型数据 + 当前工具设置结果（1，成功；0，不成功） + 图像信息数据
                        
                        VisionSystemClassLibrary.Enum.CameraType Cameratype_QualityCheckCurrentTool = VisionSystemClassLibrary.Enum.CameraType.Camera_1;//相机类型数据
                        Int32 iValue_1_QualityCheckCurrentTool = 0;//当前工具设置结果（1，成功；0，不成功）
                        VisionSystemClassLibrary.Struct.ImageInformation ImageInformation_QualityCheckCurrentTool = new VisionSystemClassLibrary.Struct.ImageInformation();//图像信息数据

                        _GetInstructionData(serverData, ref Cameratype_QualityCheckCurrentTool, ref iValue_1_QualityCheckCurrentTool, ref ImageInformation_QualityCheckCurrentTool);//解析指令数据

                        Int32 iCameraIndex_QualityCheckCurrentTool = _GetSelectedCameraIndex(Cameratype_QualityCheckCurrentTool);//相机索引值

                        if (0 <= ImageInformation_QualityCheckCurrentTool.ToolsIndex && (VisionSystemClassLibrary.Enum.ImageType.Error == ImageInformation_QualityCheckCurrentTool.Type))//剔除图像
                        {
                            ImageInformation_QualityCheckCurrentTool.Name = Global.VisionSystem.Camera[iCameraIndex_QualityCheckCurrentTool].Tools[ImageInformation_QualityCheckCurrentTool.ToolsIndex].Name;//图像信息
                        }
                        else//完好图像
                        {
                            ImageInformation_QualityCheckCurrentTool.Name = "OK";//图像信息
                        }

                        if (1 == iValue_1_QualityCheckCurrentTool)//保存数据
                        {
                            //
                        }

                        //

                        try
                        {
                            switch (Global.CurrentInterface)
                            {
                                case ApplicationInterface.Work://WORK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.Load://LOAD页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.LiveView://LIVE VIEW页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.QualityCheck://QUALITY CHECK页面

                                    Global.QualityCheckWindow.CustomControl.Invoke(new EventHandler(delegate { Global.QualityCheckWindow.CustomControl._UpdateInterface(ImageInformation_QualityCheckCurrentTool); }));

                                    break;
                                    //
                                case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.DevicesSetup://DEVICES SETUP页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.SystemConfiguration://SYSTEM页面
                                    //
                                    break;
                                    //
                                default:
                                    break;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            //不执行操作
                        }
                        //
                        break;
                        //
                    case CommunicationInstructionType.QualityCheck_WorkArea:

                        //QUALITY CHECK，工作区域，格式：
                        //客户端->服务端：指令类型 + 相机类型数据 + 设置结果（1，成功；0，不成功） + 图像信息数据

                        VisionSystemClassLibrary.Enum.CameraType Cameratype_QualityCheckWorkArea = VisionSystemClassLibrary.Enum.CameraType.Camera_1;//相机类型数据
                        Int32 iValue_1_QualityCheckWorkArea = 0;//设置结果（1，成功；0，不成功）
                        VisionSystemClassLibrary.Struct.ImageInformation ImageInformation_QualityCheckWorkArea = new VisionSystemClassLibrary.Struct.ImageInformation();//图像信息数据

                        _GetInstructionData(serverData, ref Cameratype_QualityCheckWorkArea, ref iValue_1_QualityCheckWorkArea, ref ImageInformation_QualityCheckWorkArea);//解析指令数据

                        Int32 iCameraIndex_QualityCheckWorkArea = _GetSelectedCameraIndex(Cameratype_QualityCheckWorkArea);//相机索引值

                        if (0 <= ImageInformation_QualityCheckWorkArea.ToolsIndex && (VisionSystemClassLibrary.Enum.ImageType.Error == ImageInformation_QualityCheckWorkArea.Type))//剔除图像
                        {
                            ImageInformation_QualityCheckWorkArea.Name = Global.VisionSystem.Camera[iCameraIndex_QualityCheckWorkArea].Tools[ImageInformation_QualityCheckWorkArea.ToolsIndex].Name;//图像信息
                        }
                        else//完好图像
                        {
                            ImageInformation_QualityCheckWorkArea.Name = "OK";//图像信息
                        }

                        if (1 == iValue_1_QualityCheckWorkArea)//保存数据
                        {
                            //
                        }

                        //

                        try
                        {
                            switch (Global.CurrentInterface)
                            {
                                case ApplicationInterface.Work://WORK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.Load://LOAD页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.LiveView://LIVE VIEW页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.QualityCheck://QUALITY CHECK页面

                                    Global.QualityCheckWindow.CustomControl.Invoke(new EventHandler(delegate { Global.QualityCheckWindow.CustomControl._UpdateInterface(ImageInformation_QualityCheckWorkArea); }));

                                    _SendCommand_Value(CommunicationInstructionType.QualityCheck_TolerancesValue, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex);//发送指令

                                    break;
                                    //
                                case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.DevicesSetup://DEVICES SETUP页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.SystemConfiguration://SYSTEM页面
                                    //
                                    break;
                                    //
                                default:
                                    break;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            //不执行操作
                        }
                        //
                        break;
                        //
                    case CommunicationInstructionType.QualityCheck_Enter:

                        //QUALITY CHECK页面，进入页面，格式：
                        //客户端->服务端：指令类型 + 相机类型数据 + 操作结果（1，成功；0，失败）

                        VisionSystemClassLibrary.Enum.CameraType Cameratype_QualityCheckEnter = VisionSystemClassLibrary.Enum.CameraType.Camera_1;//相机类型数据
                        Int32 iValue_1_QualityCheckEnter = 0;//操作结果（1，成功；0，失败）

                        _GetInstructionData(serverData, ref Cameratype_QualityCheckEnter, ref iValue_1_QualityCheckEnter);//解析指令数据

                        //

                        Int32 iCameraIndex_QualityCheckEnter = _GetSelectedCameraIndex(Cameratype_QualityCheckEnter);//相机类型索引

                        //

                        try
                        {
                            switch (Global.CurrentInterface)
                            {
                                case ApplicationInterface.Work://WORK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.Load://LOAD页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.LiveView://LIVE VIEW页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.QualityCheck://QUALITY CHECK页面

                                    _SendCommand_Image(CommunicationInstructionType.QualityCheck_LiveView, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex,1.0);//发送指令

                                    break;
                                    //
                                case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.DevicesSetup://DEVICES SETUP页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.SystemConfiguration://SYSTEM页面
                                    //
                                    break;
                                    //
                                default:
                                    break;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            //不执行操作
                        }
                        //
                        break;
                        //
                    case CommunicationInstructionType.TitleBar_State:

                        //标题栏【STATE】按钮操作，设备状态，格式：
                        //客户端->服务端：指令类型 + 相机类型数据 + 设置结果（1，成功；0，不成功）

                        VisionSystemClassLibrary.Enum.CameraType Cameratype_TitleBarState = VisionSystemClassLibrary.Enum.CameraType.Camera_1;//相机类型数据
                        Int32 iValue_1_TitleBarState = 0;//设置结果

                        _GetInstructionData(serverData, ref Cameratype_TitleBarState, ref iValue_1_TitleBarState);//解析指令数据

                        if (1 == iValue_1_TitleBarState)//设置结果，成功
                        {
                            //
                        }

                        //

                        Int32 iCameraIndex_TitleBarState = _GetSelectedCameraIndex(Cameratype_TitleBarState);//相机类型索引

                        //

                        try
                        {
                            switch (Global.CurrentInterface)
                            {
                                case ApplicationInterface.Work://WORK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.Load://LOAD页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.LiveView://LIVE VIEW页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.QualityCheck://QUALITY CHECK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.DevicesSetup://DEVICES SETUP页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.SystemConfiguration://SYSTEM页面
                                    //
                                    break;
                                    //
                                default:
                                    break;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            //不执行操作
                        }
                        //
                        break;
                        //
                    case CommunicationInstructionType.ClientSystem_Update:

                        //客户端系统升级，格式：
                        //客户端->服务端：指令类型 + 相机类型数据 + 传输结果（1，成功；0，失败）

                        VisionSystemClassLibrary.Enum.CameraType Cameratype_ClientSystemUpdate = VisionSystemClassLibrary.Enum.CameraType.Camera_1;//相机类型数据
                        Int32 iValue_1_ClientSystemUpdate = 0;//设置结果

                        _GetInstructionData(serverData, ref Cameratype_ClientSystemUpdate, ref iValue_1_ClientSystemUpdate);//解析指令数据

                        //

                        Int32 iCameraIndex_ClientSystemUpdate = _GetSelectedCameraIndex(Cameratype_ClientSystemUpdate);//相机类型索引

                        //

                        try
                        {
                            switch (Global.CurrentInterface)
                            {
                                case ApplicationInterface.Work://WORK页面
                                    
                                    workControl.Invoke(new EventHandler(delegate { workControl._UpdateControllerApplication(Convert.ToBoolean(iValue_1_ClientSystemUpdate)); }));
                                    
                                    break;
                                    //
                                case ApplicationInterface.Load://LOAD页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.LiveView://LIVE VIEW页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.QualityCheck://QUALITY CHECK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.DevicesSetup://DEVICES SETUP页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.SystemConfiguration://SYSTEM页面
                                    //
                                    break;
                                    //
                                default:
                                    break;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            //不执行操作
                        }
                        //
                        break;
                        //
                    case CommunicationInstructionType.Network_Check:

                        //网络检测页面，格式：
                        //客户端->服务端：指令类型 + 数据（上位机，1；下位机，2）

                        Int32 iValue_1_Network_Check = 0;//数值

                        _GetInstructionData(serverData, ref iValue_1_Network_Check);//解析指令数据

                        //

                        if (2 == iValue_1_Network_Check)//下位机发送
                        {
                            _SendCommand_Value(CommunicationInstructionType.Network_Check, serverData.Client.IP, 2);//发送指令
                        }

                        //

                        try
                        {
                            switch (Global.CurrentInterface)
                            {
                                case ApplicationInterface.Work://WORK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.Load://LOAD页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.LiveView://LIVE VIEW页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.QualityCheck://QUALITY CHECK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.DevicesSetup://DEVICES SETUP页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.SystemConfiguration://SYSTEM页面
                                    //
                                    break;
                                    //
                                default:
                                    break;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            //不执行操作
                        }
                        break;
                        //
                    case CommunicationInstructionType.SystemParameter:

                        //相机停用，格式：
                        //客户端->服务端：指令类型 + 相机类型数据 + 设置结果（1，成功；0，不成功）

                        VisionSystemClassLibrary.Enum.CameraType Cameratype_SystemParameter = VisionSystemClassLibrary.Enum.CameraType.Camera_1;//相机类型数据
                        Int32 iValue_1_SystemParameter = 0;//设置结果

                        _GetInstructionData(serverData, ref Cameratype_SystemParameter, ref iValue_1_SystemParameter);//解析指令数据

                        if (1 == iValue_1_SystemParameter)//设置结果，成功
                        {
                            //
                        }

                        //

                        Int32 iCameraIndex_SystemParameter = _GetSelectedCameraIndex(Cameratype_SystemParameter);//相机类型索引

                        //

                        try
                        {
                            switch (Global.CurrentInterface)
                            {
                                case ApplicationInterface.Work://WORK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.Load://LOAD页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.LiveView://LIVE VIEW页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.QualityCheck://QUALITY CHECK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.DevicesSetup://DEVICES SETUP页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.SystemConfiguration://SYSTEM页面

                                    Global.SystemConfigurationWindow.CustomControl.Invoke(new EventHandler(delegate { Global.SystemConfigurationWindow.CustomControl._CommonParameter(Convert.ToBoolean(iValue_1_SystemParameter)); }));

                                    break;
                                    //
                                default:
                                    break;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            //不执行操作
                        }
                        //
                        break;
                        //
                    case CommunicationInstructionType.DeviceState_Synchronization:

                        //系统状态同步，格式：
                        //客户端->服务端：指令类型 + 相机类型数据 + 设置结果（1，成功；0，不成功）

                        VisionSystemClassLibrary.Enum.CameraType Cameratype_DeviceStateSynchronization = VisionSystemClassLibrary.Enum.CameraType.Camera_1;//相机类型数据
                        Int32 iValue_1_DeviceStateSynchronization = 0;//设置结果

                        _GetInstructionData(serverData, ref Cameratype_DeviceStateSynchronization, ref iValue_1_DeviceStateSynchronization);//解析指令数据

                        if (1 == iValue_1_DeviceStateSynchronization)//设置结果，成功
                        {
                            //
                        }

                        //

                        Int32 iCameraIndex_DeviceStateSynchronization = _GetSelectedCameraIndex(Cameratype_DeviceStateSynchronization);//相机类型索引

                        //

                        try
                        {
                            switch (Global.CurrentInterface)
                            {
                                case ApplicationInterface.Work://WORK页面
                                    
                                    //_SendCommand_Value(CommunicationInstructionType.DevicesSetup_AlignDateTime, Global.VisionSystem.Camera[iCameraIndex_DeviceStateSynchronization].Type, iCameraIndex_DeviceStateSynchronization);//发送指令

                                    //Thread.Sleep(Global.CurrentFaultMessageTime); //延时发送图像查询

                                    _SendCommand_Image(CommunicationInstructionType.Work, Global.VisionSystem.Camera[iCameraIndex_DeviceStateSynchronization].Type, iCameraIndex_DeviceStateSynchronization, Global.ImageScale_Work);//发送指令

                                    break;
                                    //
                                case ApplicationInterface.Load://LOAD页面
                                    
                                    _SendCommand_Image(CommunicationInstructionType.Work, Global.VisionSystem.Camera[iCameraIndex_DeviceStateSynchronization].Type, iCameraIndex_DeviceStateSynchronization, Global.ImageScale_Work);//发送指令

                                    break;
                                    //
                                case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.LiveView://LIVE VIEW页面

                                    if (Global.LiveViewWindow.CustomControl.SelectedCamera.Type == Cameratype_DeviceStateSynchronization)//当前相机
                                    {
                                        Global.VisionSystem.Work.SelectedCameraType = Cameratype_DeviceStateSynchronization;//当前选中的相机类型
                                        Global.VisionSystem.Work.SelectedCameraIndex = (Int16)_GetSelectedCameraIndex(Cameratype_DeviceStateSynchronization);//当前选中的相机显示控件所对应的相机在相机数组中的索引值
                                        Global.VisionSystem.Camera[Global.VisionSystem.Work.SelectedCameraIndex].Live.CameraSelected = true;//选择
                                    }

                                    _SendCommand_Image(CommunicationInstructionType.Live, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex);//发送指令

                                    Global.LiveViewWindow.CustomControl.Invoke(new EventHandler(delegate { Global.LiveViewWindow.CustomControl._DeviceStateChanged(true); }));//更新页面

                                    break;
                                    //
                                case ApplicationInterface.QualityCheck://QUALITY CHECK页面

                                    if (Global.QualityCheckWindow.CustomControl.SelectedCamera.Type == Cameratype_DeviceStateSynchronization)//当前相机
                                    {
                                        Global.VisionSystem.Work.SelectedCameraType = Cameratype_DeviceStateSynchronization;//当前选中的相机类型
                                        Global.VisionSystem.Work.SelectedCameraIndex = (Int16)_GetSelectedCameraIndex(Cameratype_DeviceStateSynchronization);//当前选中的相机显示控件所对应的相机在相机数组中的索引值
                                        Global.VisionSystem.Camera[Global.VisionSystem.Work.SelectedCameraIndex].Live.CameraSelected = true;//选择
                                    }

                                    Global.QualityCheckWindow.CustomControl.Invoke(new EventHandler(delegate { _SendCommand_Value(CommunicationInstructionType.QualityCheck_Enter, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex, Global.QualityCheckWindow.CustomControl.CurrentToolIndex); }));//发送指令

                                    Global.QualityCheckWindow.CustomControl.Invoke(new EventHandler(delegate { Global.QualityCheckWindow.CustomControl._DeviceStateChanged(true); }));//更新页面

                                    break;
                                    //
                                case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面

                                    if (Global.TolerancesSettingsWindow.CustomControl.SelectedCamera.Type == Cameratype_DeviceStateSynchronization)//当前相机
                                    {
                                        Global.VisionSystem.Work.SelectedCameraType = Cameratype_DeviceStateSynchronization;//当前选中的相机类型
                                        Global.VisionSystem.Work.SelectedCameraIndex = (Int16)_GetSelectedCameraIndex(Cameratype_DeviceStateSynchronization);//当前选中的相机显示控件所对应的相机在相机数组中的索引值
                                        Global.VisionSystem.Camera[Global.VisionSystem.Work.SelectedCameraIndex].Live.CameraSelected = true;//选择
                                    }

                                    _SendCommand_Value(CommunicationInstructionType.TolerancesSettings_Enter, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex);//发送指令

                                    Global.TolerancesSettingsWindow.CustomControl.Invoke(new EventHandler(delegate { Global.TolerancesSettingsWindow.CustomControl._DeviceStateChanged(true); }));//更新页面

                                    break;
                                    //
                                case ApplicationInterface.DevicesSetup://DEVICES SETUP页面
                                    
                                    Global.DevicesSetupWindow.CustomControl.Invoke(new EventHandler(delegate { Global.DevicesSetupWindow.CustomControl._RefreshList(); }));//操作完成

                                    if (Global.ImageConfigurationWindow.CustomControl.SelectedCamera.Type == Cameratype_DeviceStateSynchronization)//DEVICES SETUP，IMAGE CONFIGURATION页面
                                    {
                                        Global.ImageConfigurationWindow.CustomControl._DeviceStateChanged(true);
                                    }

                                    //

                                    if (Global.DevicesSetupWindow.CustomControl.ConfigDeviceMessageWindowShow)//配置后的等待窗口
                                    {
                                        Global.DevicesSetupWindow.CustomControl.Invoke(new EventHandler(delegate { Global.DevicesSetupWindow.CustomControl._ConfigDevice_RestartOK(Global.VisionSystem.Camera[iCameraIndex_DeviceStateSynchronization].ControllerENGName); }));//更新页面
                                    }

                                    break;
                                    //
                                case ApplicationInterface.SystemConfiguration://SYSTEM页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.StatisticsView://STATISTICS VIEW页面

                                    if (Global.StatisticsViewWindow.CustomControl.SelectedCamera.Type == Cameratype_DeviceStateSynchronization)//当前相机
                                    {
                                        Global.VisionSystem.Work.SelectedCameraType = Cameratype_DeviceStateSynchronization;//当前选中的相机类型
                                        Global.VisionSystem.Work.SelectedCameraIndex = (Int16)_GetSelectedCameraIndex(Cameratype_DeviceStateSynchronization);//当前选中的相机显示控件所对应的相机在相机数组中的索引值
                                        Global.VisionSystem.Camera[Global.VisionSystem.Work.SelectedCameraIndex].Live.CameraSelected = true;//选择
                                    }

                                    if (!Global.StatisticsViewWindow.CustomControl.StatisticsRecordWindowDisplay)//STATISTICS RECORD窗口未显示
                                    {
                                        _SendCommand_Value(CommunicationInstructionType.Statistics_UpdateSelectedRecordData, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex, Global.StatisticsViewWindow.CustomControl.Relevancy, Global.VisionSystem.Shift.DataOfShift.CurrentIndex + 1, Global.VisionSystem.Shift.DataOfShift.CurrentStatisticsInformation.TimeData[0], Global.StatisticsViewWindow.CustomControl.CurrentToolIndex_RejectImage, Global.StatisticsViewWindow.CustomControl.CurrentRejectImageIndex_Tool, 1.0);
                                    }
                                    
                                    break;
                                    //
                                default:
                                    break;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            //不执行操作
                        }
                        //
                        break;
                        //
                    case CommunicationInstructionType.Statistics_GetSelectedRecordData:

                        //STATISTICS页面，获取当前选择的统计数据，格式：
                        //客户端->服务端：指令类型 + 相机类型数据 + 关联标记 + 统计数据类型（0，当前班；1，历史班） + 班次索引（非0） + 统计数据开始结束时间 + 统计数据（品牌名称（包括品牌长度） + 已检测数量 + 合格数量 + 剔除数量 + 工具数量 + 工具统计信息）

                        VisionSystemClassLibrary.Enum.CameraType Cameratype_StatisticsGetSelectedRecordData = VisionSystemClassLibrary.Enum.CameraType.Camera_1;//相机类型数据
                        Int32 iStatisticsDataType_StatisticsGetSelectedRecordData = 0;//统计数据类型（0，当前班；1，历史班）
                        Boolean iRelevancy_StatisticsGetSelectedRecordData = false;//相机关联标记

                        _GetInstructionData(serverData, ref Cameratype_StatisticsGetSelectedRecordData,ref  iRelevancy_StatisticsGetSelectedRecordData, ref iStatisticsDataType_StatisticsGetSelectedRecordData, ref Global.VisionSystem.Shift.DataOfShift.CurrentIndex, ref Global.VisionSystem.Shift.DataOfShift.CurrentStatisticsInformation);//解析指令数据

                        Int32 iCameraIndex_StatisticsGetSelectedRecordData = _GetSelectedCameraIndex(Cameratype_StatisticsGetSelectedRecordData);//相机类型索引

                        //

                        try
                        {
                            switch (Global.CurrentInterface)
                            {
                                case ApplicationInterface.Work://WORK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.Load://LOAD页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.LiveView://LIVE VIEW页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.QualityCheck://QUALITY CHECK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.DevicesSetup://DEVICES SETUP页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.SystemConfiguration://SYSTEM页面
                                    //
                                    break;
                                case ApplicationInterface.StatisticsView://STATISTICS页面
                                    
                                    Global.StatisticsViewWindow.CustomControl.Invoke(new EventHandler(delegate { Global.StatisticsViewWindow.CustomControl._GetStatisticsData(true, iStatisticsDataType_StatisticsGetSelectedRecordData); }));//操作完成

                                    //

                                    if (Global.StatisticsViewWindow.CustomControl.ViewRejectButton) //查询剔除图像
                                    {
                                        _SendCommand_Value(CommunicationInstructionType.Statistics_ClickRejectsListItem, Global.StatisticsViewWindow.CustomControl.ViewRejectImage_CameraSelected, _GetSelectedCameraIndex(Global.StatisticsViewWindow.CustomControl.ViewRejectImage_CameraSelected), Global.StatisticsViewWindow.CustomControl.Relevancy, Global.VisionSystem.Shift.DataOfShift.CurrentIndex + 1, Global.VisionSystem.Shift.DataOfShift.CurrentStatisticsInformation.TimeData[0], Global.StatisticsViewWindow.CustomControl.CurrentToolIndex_RejectImage, Global.StatisticsViewWindow.CustomControl.CurrentRejectImageIndex_Tool, 1.0);
                                    }

                                    //

                                    if (0 == iStatisticsDataType_StatisticsGetSelectedRecordData)//统计数据类型（0，当前班；1，历史班）
                                    {
                                        Thread.Sleep(Global.CurrentFaultMessageTime);//延时查询统计数据

                                        _SendCommand_Value(CommunicationInstructionType.Statistics_UpdateSelectedRecordData, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex, Global.StatisticsViewWindow.CustomControl.Relevancy, Global.VisionSystem.Shift.DataOfShift.CurrentIndex + 1, Global.VisionSystem.Shift.DataOfShift.CurrentStatisticsInformation.TimeData[0], Global.StatisticsViewWindow.CustomControl.CurrentToolIndex_RejectImage, Global.StatisticsViewWindow.CustomControl.CurrentRejectImageIndex_Tool, 1.0);//发送指令
                                    }

                                    break;
                                    //
                                default:
                                    break;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            //不执行操作
                        }
                        //
                        break;
                        //
                    case CommunicationInstructionType.Statistics_UpdateSelectedRecordData:

                        //STATISTICS页面，更新当前选择的统计数据，格式：
                        //客户端->服务端：指令类型 + 相机类型数据 + 关联标记 + 统计数据类型（0，当前班；1，历史班） + 班次索引（非0） + 统计数据开始结束时间 + 统计数据（品牌名称（包括品牌长度） + 已检测数量 + 合格数量 + 剔除数量 + 工具数量 + 工具统计信息）

                        VisionSystemClassLibrary.Enum.CameraType Cameratype_StatisticsUpdateSelectedRecordData = VisionSystemClassLibrary.Enum.CameraType.Camera_1;//相机类型数据
                        Int32 iStatisticsDataType_StatisticsUpdateSelectedRecordData = 0;//统计数据类型（0，最新统计数据；1，指定统计数据）
                        Boolean iRelevancy_StatisticsUpdateSelectedRecordData = false;//相机关联标记

                        _GetInstructionData(serverData, ref Cameratype_StatisticsUpdateSelectedRecordData, ref iRelevancy_StatisticsUpdateSelectedRecordData, ref iStatisticsDataType_StatisticsUpdateSelectedRecordData, ref Global.VisionSystem.Shift.DataOfShift.CurrentIndex, ref Global.VisionSystem.Shift.DataOfShift.CurrentStatisticsInformation);//解析指令数据

                        Int32 iCameraIndex_StatisticsUpdateSelectedRecordData = _GetSelectedCameraIndex(Cameratype_StatisticsUpdateSelectedRecordData);//相机类型索引

                        //

                        try
                        {
                            switch (Global.CurrentInterface)
                            {
                                case ApplicationInterface.Work://WORK页面
                                    //
                                    break;
                                //
                                case ApplicationInterface.Load://LOAD页面
                                    //
                                    break;
                                //
                                case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                                    //
                                    break;
                                //
                                case ApplicationInterface.LiveView://LIVE VIEW页面
                                    //
                                    break;
                                //
                                case ApplicationInterface.QualityCheck://QUALITY CHECK页面
                                    //
                                    break;
                                //
                                case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面
                                    //
                                    break;
                                //
                                case ApplicationInterface.DevicesSetup://DEVICES SETUP页面
                                    //
                                    break;
                                //
                                case ApplicationInterface.SystemConfiguration://SYSTEM页面
                                    //
                                    break;
                                case ApplicationInterface.StatisticsView://STATISTICS页面

                                    Global.StatisticsViewWindow.CustomControl.Invoke(new EventHandler(delegate { Global.StatisticsViewWindow.CustomControl._UpdateStatisticsData(iStatisticsDataType_StatisticsUpdateSelectedRecordData); }));//操作完成

                                    //

                                    if (false == Global.StatisticsViewWindow.CustomControl.StatisticsRecordWindowDisplay)//STATISTICS RECORD窗口未显示
                                    {
                                        _SendCommand_Value(CommunicationInstructionType.Statistics_UpdateSelectedRecordData, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex, Global.StatisticsViewWindow.CustomControl.Relevancy, Global.VisionSystem.Shift.DataOfShift.CurrentIndex + 1, Global.VisionSystem.Shift.DataOfShift.CurrentStatisticsInformation.TimeData[0], Global.StatisticsViewWindow.CustomControl.CurrentToolIndex_RejectImage, Global.StatisticsViewWindow.CustomControl.CurrentRejectImageIndex_Tool, 1.0);//发送指令
                                    }

                                    break;
                                //
                                default:
                                    break;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            //不执行操作
                        }
                        //
                        break;
                    //
                    case CommunicationInstructionType.Statistics_ClickRejectsListItem:

                        //STATISTICS页面，查看剔除图像，格式：
                        //客户端->服务端：指令类型 + 相机类型数据 + 关联标记 + 班次索引（非0） + 统计数据开始结束时间 + 剔除图像对应的工具索引值（从0开始） + 剔除图像对应的工具中的索引值（从0开始） + 图像尺寸类型数据 + 图像宽度数据 + 图像高度数据 + 图像信息数据 + 图像数据

                        VisionSystemClassLibrary.Enum.CameraType Cameratype_StatisticsClickRejectsListItem = VisionSystemClassLibrary.Enum.CameraType.Camera_1;//相机类型数据
                        Boolean bRelevancy_StatisticsClickRejectsListItem = false;//关联标记
                        Int32 iShiftIndex_StatisticsClickRejectsListItem = 0;//班次索引（从0开始）
                        VisionSystemClassLibrary.Struct.ShiftTime ShiftTime_StatisticsClickRejectsListItem = new VisionSystemClassLibrary.Struct.ShiftTime();//统计数据开始结束时间
                        Int32 iToolIndex_StatisticsClickRejectsListItem = 0;//剔除图像对应的工具索引值（从0开始）
                        Int32 iImageIndex_StatisticsClickRejectsListItem = 0;//剔除图像对应的工具中的索引值（从0开始）
                        Double dImageScale_StatisticsClickRejectsListItem = 0;//图像尺寸类型数据
                        Int32 iImageDataLength_StatisticsClickRejectsListItem = 0;//图像数据长度
                        VisionSystemClassLibrary.Struct.ImageInformation GraphicsInformation_StatisticsClickRejectsListItem = new VisionSystemClassLibrary.Struct.ImageInformation();//图像信息
                        Image<Bgr, Byte> imageData_StatisticsClickRejectsListItem = null;//图像数据

                        _GetInstructionData(serverData, ref Cameratype_StatisticsClickRejectsListItem, ref bRelevancy_StatisticsClickRejectsListItem, ref iShiftIndex_StatisticsClickRejectsListItem, ref ShiftTime_StatisticsClickRejectsListItem, ref iToolIndex_StatisticsClickRejectsListItem, ref iImageIndex_StatisticsClickRejectsListItem, ref dImageScale_StatisticsClickRejectsListItem, ref iImageDataLength_StatisticsClickRejectsListItem, ref GraphicsInformation_StatisticsClickRejectsListItem, ref imageData_StatisticsClickRejectsListItem);//解析指令数据

                        Int32 iCameraIndex_StatisticsClickRejectsListItem = _GetSelectedCameraIndex(Cameratype_StatisticsClickRejectsListItem);//相机类型索引

                        if (0 < iImageDataLength_StatisticsClickRejectsListItem)//图像有效
                        {
                            VisionSystemClassLibrary.Struct.StatisticsData.ImageReject = imageData_StatisticsClickRejectsListItem.Copy();//更新数值
                            VisionSystemClassLibrary.Struct.StatisticsData.GraphicsInformation = GraphicsInformation_StatisticsClickRejectsListItem;//图像信息
                            if (0 <= GraphicsInformation_StatisticsClickRejectsListItem.ToolsIndex && (VisionSystemClassLibrary.Enum.ImageType.Error == GraphicsInformation_StatisticsClickRejectsListItem.Type))//剔除图像
                            {
                                VisionSystemClassLibrary.Struct.StatisticsData.GraphicsInformation.Name = Global.VisionSystem.Camera[iCameraIndex_StatisticsClickRejectsListItem].Tools[GraphicsInformation_StatisticsClickRejectsListItem.ToolsIndex].Name;//信息名称
                            }
                            else//完好图像
                            {
                                VisionSystemClassLibrary.Struct.StatisticsData.GraphicsInformation.Name = "OK";//信息名称
                            }

                            //

                            imageData_StatisticsClickRejectsListItem.Dispose();
                        }
                        else//图像无效
                        {
                            VisionSystemClassLibrary.Struct.StatisticsData.ImageReject = null;//更新数值

                            VisionSystemClassLibrary.Struct.StatisticsData.GraphicsInformation = new VisionSystemClassLibrary.Struct.ImageInformation();//图像信息
                            VisionSystemClassLibrary.Struct.StatisticsData.GraphicsInformation.Valid = false;//图像信息
                            VisionSystemClassLibrary.Struct.StatisticsData.GraphicsInformation.Name = "";//图像信息
                            VisionSystemClassLibrary.Struct.StatisticsData.GraphicsInformation.ValueDisplay = false;//图像信息
                            VisionSystemClassLibrary.Struct.StatisticsData.GraphicsInformation.Type = VisionSystemClassLibrary.Enum.ImageType.Error;//图像信息
                            VisionSystemClassLibrary.Struct.StatisticsData.GraphicsInformation.Value = new Boolean[VisionSystemClassLibrary.Struct.ImageInformation.TotalNumber];//图像信息
                        }      

                        //

                        try
                        {
                            switch (Global.CurrentInterface)
                            {
                                case ApplicationInterface.Work://WORK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.Load://LOAD页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.LiveView://LIVE VIEW页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.QualityCheck://QUALITY CHECK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.DevicesSetup://DEVICES SETUP页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.SystemConfiguration://SYSTEM页面
                                    //
                                    break;
                                case ApplicationInterface.StatisticsView://STATISTICS页面

                                    Global.StatisticsViewWindow.CustomControl.Invoke(new EventHandler(delegate { Global.StatisticsViewWindow.CustomControl._SetImageView(); }));//操作完成

                                    break;
                                    //
                                default:
                                    break;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            //不执行操作
                        }
                        //
                        break;
                        //
                    case CommunicationInstructionType.Statistics_GetRecordList:

                        //STATISTICS页面，获取统计数据列表，格式：
                        //客户端->服务端：指令类型 + 相机类型数据 + 统计数据列表（班次个数 + 当前班次索引（从0开始） + 统计数据个数 + 当前统计数据索引（从0开始） + （每个班次）统计数据个数 + （每个班次，每个统计数据）班次开始结束时间 + （每个班次，每个统计数据）品牌（包括品牌长度））

                        VisionSystemClassLibrary.Enum.CameraType Cameratype_StatisticsGetRecordList = VisionSystemClassLibrary.Enum.CameraType.Camera_1;//相机类型数据
                        VisionSystemClassLibrary.Struct.StatisticsRecordList statisticsrecordlist_StatisticsGetRecordList = new VisionSystemClassLibrary.Struct.StatisticsRecordList();//统计数据列表

                        _GetInstructionData(serverData, ref Cameratype_StatisticsGetRecordList, ref statisticsrecordlist_StatisticsGetRecordList);//解析指令数据

                        Int32 iCameraIndex_StatisticsGetRecordList = _GetSelectedCameraIndex(Cameratype_StatisticsGetRecordList);//相机类型索引

                        //

                        _SetShiftStatisticsInformation(statisticsrecordlist_StatisticsGetRecordList);

                        //

                        try
                        {
                            switch (Global.CurrentInterface)
                            {
                                case ApplicationInterface.Work://WORK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.Load://LOAD页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.LiveView://LIVE VIEW页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.QualityCheck://QUALITY CHECK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.DevicesSetup://DEVICES SETUP页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.SystemConfiguration://SYSTEM页面
                                    //
                                    break;
                                case ApplicationInterface.StatisticsView://STATISTICS页面
                                    
                                    VisionSystemControlLibrary.GlobalWindows.StatisticsRecord_Window.StatisticsRecordControl.Invoke(new EventHandler(delegate { VisionSystemControlLibrary.GlobalWindows.StatisticsRecord_Window.StatisticsRecordControl._GetStatisticsData(true); }));//操作完成

                                    break;
                                    //
                                default:
                                    break;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            //不执行操作
                        }
                        //
                        break;
                        //
                    case CommunicationInstructionType.Statistics_DeleteRecord:

                        //STATISTICS页面，删除统计数据，格式
                        //客户端->服务端：指令类型 + 相机类型数据 + 删除数据结果（1，成功；0，不成功）

                        VisionSystemClassLibrary.Enum.CameraType Cameratype_StatisticsDeleteRecord = VisionSystemClassLibrary.Enum.CameraType.Camera_1;//相机类型数据
                        Int32 iValue_StatisticsDeleteRecord = 0;//删除数据结果（1，成功；0，不成功）

                        _GetInstructionData(serverData, ref Cameratype_StatisticsDeleteRecord, ref iValue_StatisticsDeleteRecord);//解析指令数据

                        Int32 iCameraIndex_StatisticsDeleteRecord = _GetSelectedCameraIndex(Cameratype_StatisticsDeleteRecord);//相机类型索引

                        //

                        try
                        {
                            switch (Global.CurrentInterface)
                            {
                                case ApplicationInterface.Work://WORK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.Load://LOAD页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.LiveView://LIVE VIEW页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.QualityCheck://QUALITY CHECK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.DevicesSetup://DEVICES SETUP页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.SystemConfiguration://SYSTEM页面
                                    //
                                    break;
                                case ApplicationInterface.StatisticsView://STATISTICS页面

                                    Boolean bValue_StatisticsDeleteRecord = Convert.ToBoolean(iValue_StatisticsDeleteRecord);

                                    //VisionSystemControlLibrary.GlobalWindows.StatisticsRecord_Window.StatisticsRecordControl.Invoke(new EventHandler(delegate { VisionSystemControlLibrary.GlobalWindows.StatisticsRecord_Window.StatisticsRecordControl._DeleteRecords(bValue_StatisticsDeleteRecord); }));//操作完成

                                    //

                                    if (bValue_StatisticsDeleteRecord)//成功
                                    {
                                        VisionSystemControlLibrary.GlobalWindows.StatisticsRecord_Window.StatisticsRecordControl.Invoke(new EventHandler(delegate { VisionSystemControlLibrary.GlobalWindows.StatisticsRecord_Window.StatisticsRecordControl._DeleteRecords(bValue_StatisticsDeleteRecord); }));//操作完成

                                        _SendCommand_Value(CommunicationInstructionType.Statistics_GetRecordList, Global.VisionSystem.Work.SelectedCameraType, Global.VisionSystem.Work.SelectedCameraIndex);//发送指令
                                    }
                                    else
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

                                    break;
                                    //
                                default:
                                    break;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            //不执行操作
                        }
                        //
                        break;
                        //
                    case CommunicationInstructionType.CurrentFaultMessage:

                        //FAULT MESSAGE，获取当前故障信息，格式
                        //客户端->服务端：指令类型 + 相机类型数据 + 故障信息（时间 + 故障代码索引值） + 机器速度/相位标志（1，速度；0，相位） + 机器速度/相位数值

                        VisionSystemClassLibrary.Enum.CameraType Cameratype_CurrentFaultMessage = VisionSystemClassLibrary.Enum.CameraType.Camera_1;//相机类型数据
                        VisionSystemClassLibrary.Struct.FaultMessage FaultMessage_CurrentFaultMessage = new VisionSystemClassLibrary.Struct.FaultMessage();//故障信息数组
                        Int32 iType_CurrentFaultMessage = 0;//机器速度/相位标志（1，速度；0，相位）
                        Int32 iValue_CurrentFaultMessage = 0;//机器速度/相位数值

                        _GetInstructionData(serverData, ref Cameratype_CurrentFaultMessage, ref FaultMessage_CurrentFaultMessage, ref iType_CurrentFaultMessage, ref iValue_CurrentFaultMessage);//解析指令数据

                        Int32 iCameraIndex_CurrentFaultMessage = _GetSelectedCameraIndex(Cameratype_CurrentFaultMessage);//相机类型索引

                        //

                        try
                        {
                            _SetFaultMessage(iCameraIndex_CurrentFaultMessage, FaultMessage_CurrentFaultMessage);

                            workControl.Invoke(new EventHandler(delegate { workControl._UpdateMachineSpeed(iCameraIndex_CurrentFaultMessage, iType_CurrentFaultMessage, iValue_CurrentFaultMessage); }));
                        }
                        catch (System.Exception ex)
                        {
                            //不执行操作
                        }

                        //
                        break;
                        //
                    case CommunicationInstructionType.SetFaultMessageState:

                        //FAULT MESSAGE状态，格式
                        //客户端->服务端：指令类型 + 相机类型数据 + 设置结果（1，成功；0，不成功）

                        VisionSystemClassLibrary.Enum.CameraType Cameratype_SetFaultMessageState = VisionSystemClassLibrary.Enum.CameraType.Camera_1;//相机类型数据
                        Int32 iValue_SetFaultMessageState = 0;//设置结果（1，成功；0，不成功）

                        _GetInstructionData(serverData, ref Cameratype_SetFaultMessageState, ref iValue_SetFaultMessageState);//解析指令数据

                        Int32 iCameraIndex_SetFaultMessageState = _GetSelectedCameraIndex(Cameratype_SetFaultMessageState);//相机类型索引
                        
                        //
                        break;
                        //
                    case CommunicationInstructionType.GetFaultMessages:

                        //FAULT MESSAGE，获取故障信息，格式
                        //客户端->服务端：指令类型 + 相机类型数据 + 故障信息个数 + 故障信息数组（时间 + 故障代码索引值）

                        VisionSystemClassLibrary.Enum.CameraType Cameratype_GetFaultMessages = VisionSystemClassLibrary.Enum.CameraType.Camera_1;//相机类型数据
                        VisionSystemClassLibrary.Struct.FaultMessage[] FaultMessage_GetFaultMessages = null;//故障信息数组

                        _GetInstructionData(serverData, ref Cameratype_GetFaultMessages, ref FaultMessage_GetFaultMessages);//解析指令数据

                        Int32 iCameraIndex_GetFaultMessages = _GetSelectedCameraIndex(Cameratype_GetFaultMessages);//相机类型索引

                        //

                        try
                        {
                            VisionSystemControlLibrary.GlobalWindows.FaultMessage_Window.FaultMessageControl.Invoke(new EventHandler(delegate { VisionSystemControlLibrary.GlobalWindows.FaultMessage_Window.FaultMessageControl._GetData(true); }));//操作完成

                            //

                            switch (Global.CurrentInterface)
                            {
                                case ApplicationInterface.Work://WORK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.Load://LOAD页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.LiveView://LIVE VIEW页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.QualityCheck://QUALITY CHECK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.DevicesSetup://DEVICES SETUP页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.SystemConfiguration://SYSTEM页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.StatisticsView://STATISTICS页面
                                    //
                                    break;
                                    //
                                default:
                                    break;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            //不执行操作
                        }
                        //
                        break;
                        //
                    case CommunicationInstructionType.ClearAllFaultMessages:

                        //FAULT MESSAGE，清除所有故障信息，格式
                        //客户端->服务端：指令类型 + 相机类型数据 + 清除结果（1，成功；0，不成功）

                        VisionSystemClassLibrary.Enum.CameraType Cameratype_ClearAllFaultMessages = VisionSystemClassLibrary.Enum.CameraType.Camera_1;//相机类型数据
                        Int32 iValue_ClearAllFaultMessages = 0;//清除数据结果（1，成功；0，不成功）

                        _GetInstructionData(serverData, ref Cameratype_ClearAllFaultMessages, ref iValue_ClearAllFaultMessages);//解析指令数据

                        Int32 iCameraIndex_ClearAllFaultMessages = _GetSelectedCameraIndex(Cameratype_ClearAllFaultMessages);//相机类型索引

                        //

                        try
                        {
                            Boolean bValue_ClearAllFaultMessages = Convert.ToBoolean(iValue_ClearAllFaultMessages);

                            VisionSystemControlLibrary.GlobalWindows.FaultMessage_Window.FaultMessageControl.Invoke(new EventHandler(delegate { VisionSystemControlLibrary.GlobalWindows.FaultMessage_Window.FaultMessageControl._ClearAllFaultMessages(bValue_ClearAllFaultMessages); }));//操作完成

                            //

                            if (0 >= VisionSystemControlLibrary.GlobalWindows.FaultMessage_Window.FaultMessageControl.DeviceNumber)//成功
                            {
                                _SendCommand_Cameras(CommunicationInstructionType.GetFaultMessages);//发送指令
                            }

                            //

                            switch (Global.CurrentInterface)
                            {
                                case ApplicationInterface.Work://WORK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.Load://LOAD页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.LiveView://LIVE VIEW页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.QualityCheck://QUALITY CHECK页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.DevicesSetup://DEVICES SETUP页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.SystemConfiguration://SYSTEM页面
                                    //
                                    break;
                                    //
                                case ApplicationInterface.StatisticsView://STATISTICS页面
                                    //
                                    break;
                                    //
                                default:
                                    break;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            //不执行操作
                        }
                        //
                        break;
                        //
                    default:
                        break;
                }
            }
            else//NetDataType.File == ServerData.DataInfo.DataType，接收的数据为文件
            {
                switch ((CommunicationInstructionType)serverData.ReceivedData[serverData.DataInfo.InstructionIndex])
                {
                    case CommunicationInstructionType.Load:

                        //启动载入，格式：

                        //未完成文件发送
                        //客户端->服务端（文件）：指令类型 + 相机类型数据 + 文件传输状态（1，启动发送；2，文件发送中，文件索引值（从2开始）） + 文件

                        VisionSystemClassLibrary.Enum.CameraType Cameratype_Load = VisionSystemClassLibrary.Enum.CameraType.Camera_1;//相机类型数据
                        Int32 iValue_1_Load = 0;//文件传输状态（1，启动发送；2，文件发送中，文件索引值（从2开始））

                        _GetInstructionData(serverData, ref Cameratype_Load, ref iValue_1_Load);//解析指令数据

                        _GetFileData(serverData);//获取文件数据

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
        // 输入参数：1.sender：ServerControl控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void NetServer_ExceptionHandled(object sender, EventArgs e)
        {
            VisionSystemCommunicationLibrary.Ethernet.ServerControl serverControl = (VisionSystemCommunicationLibrary.Ethernet.ServerControl)sender;//ServerControl控件（实际使用中可忽略该变量值）

            VisionSystemCommunicationLibrary.Ethernet.ExceptionHandledEventArgs serverException = (VisionSystemCommunicationLibrary.Ethernet.ExceptionHandledEventArgs)e;//事件参数（重要）

            SocketException socketException = (SocketException)serverException.ExceptionData;//异常

            //

            Int32 i = 0;//循环控制变量

            List<Int32> iCameraIndex = new List<Int32>();//相机索引值

            Boolean bNetworkError = false;//网络接收数据超时，客户端断开连接

            //

            if (2 == serverException.Timeout)//超时，无效
            {
                bNetworkError = true;

                //

                iCameraIndex.Clear();
                if (null != Global.VisionSystem.Camera)
                {
                    for (i = 0; i < Global.VisionSystem.Camera.Count; i++)//遍历
                    {
                        if (Global.VisionSystem.Camera[i].DeviceInformation.Connected && Global.VisionSystem.Camera[i].DeviceInformation.GetDevInfo)//已连接并验证
                        {
                            if (Global.VisionSystem.Camera[i].DeviceInformation.IP == serverException.Client.IP)//目标
                            {
                                iCameraIndex.Add(i);
                            }
                        }
                    }
                }
            }
            else//超时，发送验证数据
            {
                Boolean bSelectedCamera = false;//是否为选中的相机。取值范围：true，是；false，否

                if (null != Global.VisionSystem.Camera)
                {
                    for (i = 0; i < Global.VisionSystem.Camera.Count; i++)//遍历
                    {
                        if (Global.VisionSystem.Camera[i].DeviceInformation.Connected && Global.VisionSystem.Camera[i].DeviceInformation.GetDevInfo)//已连接并验证
                        {
                            if (Global.VisionSystem.Camera[i].DeviceInformation.IP == serverException.Client.IP)//目标
                            {
                                bSelectedCamera = true;

                                //服务端->客户端：指令类型 + 数据（上位机，1；下位机，2）

                                _SendCommand_Value(CommunicationInstructionType.Network_Check, serverException.Client.IP, 1);//发送指令

                                break;
                            }
                        }
                    }
                }

                if (!bSelectedCamera)//相机未选中
                {
                    //服务端->客户端：指令类型 + 相机类型数据

                    _SendCommand_Value(CommunicationInstructionType.Network_Check, serverException.Client.IP, 1);//发送指令
                }
            }

            //

            if (bNetworkError)//网络接收数据超时，客户端断开连接
            {
                Boolean bSelectedCamera = false;//是否为选中的相机。取值范围：true，是；false，否

                if (null != Global.VisionSystem.Camera)
                {
                    for (i = 0; i < Global.VisionSystem.Camera.Count; i++)//遍历
                    {
                        if (Global.VisionSystem.Camera[i].DeviceInformation.Connected && Global.VisionSystem.Camera[i].DeviceInformation.GetDevInfo)//已连接并验证
                        {
                            if (Global.VisionSystem.Camera[i].DeviceInformation.IP == serverException.Client.IP)//目标
                            {
                                bSelectedCamera = true;

                                //

                                break;
                            }
                        }
                    }
                }

                if (bSelectedCamera)//选中的相机
                {
                    _ResetCameraParameter(serverException);//复位相机信息

                    //

                    Global.VisionSystem.Work.ConnectedCameraNumber -= (UInt16)iCameraIndex.Count;//连接的相机数目

                    //VisionSystemClassLibrary.Class.System.SystemDeviceState = VisionSystemClassLibrary.Enum.DeviceState.Stop;//状态

                    //

                    _UpdateTitleBar();//更新标题栏

                    //

                    try
                    {
                        switch (Global.CurrentInterface)
                        {
                            case ApplicationInterface.Work://WORK页面

                                //不执行操作

                                break;
                                //
                            case ApplicationInterface.Load://LOAD页面

                                //不执行操作

                                break;
                                //
                            case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面

                                //不执行操作

                                break;
                                //
                            case ApplicationInterface.LiveView://LIVE VIEW页面

                                Global.LiveViewWindow.CustomControl.SelfTrigger = false;//SELF TRIGGER，关闭

                                Global.LiveViewWindow.CustomControl.Invoke(new EventHandler(delegate { Global.LiveViewWindow.CustomControl._DeviceStateChanged(false); }));//更新程序页面

                                break;
                                //
                            case ApplicationInterface.QualityCheck://QUALITY CHECK页面

                                Global.QualityCheckWindow.CustomControl.Invoke(new EventHandler(delegate { Global.QualityCheckWindow.CustomControl._DeviceStateChanged(false); }));//更新程序页面

                                break;
                                //
                            case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面

                                Global.TolerancesSettingsWindow.CustomControl.Invoke(new EventHandler(delegate { Global.TolerancesSettingsWindow.CustomControl._DeviceStateChanged(false); }));//更新程序页面

                                break;
                                //
                            case ApplicationInterface.DevicesSetup://DEVICES SETUP页面

                                if (null != Global.VisionSystem.Camera)
                                {
                                    for (i = 0; i < iCameraIndex.Count; i++)//遍历已接纳相机
                                    {
                                        if (Global.VisionSystem.Camera[iCameraIndex[i]].Type == Global.DevicesSetupWindow.CustomControl.CameraSelected)//目标
                                        {
                                            if (Global.ImageConfigurationWindow.CustomControl.SelectedCamera.Type == Global.VisionSystem.Camera[iCameraIndex[i]].Type)//DEVICES SETUP，IMAGE CONFIGURATION页面
                                            {
                                                Global.ImageConfigurationWindow.CustomControl._DeviceStateChanged(false);
                                            }

                                            //

                                            if (Global.DevicesSetupWindow.CustomControl.TestIOWindowShow)//DEVICES SETUP，TEST I/O页面
                                            {
                                                Global.DevicesSetupWindow.CustomControl.TestIOWindow._Close();//关闭
                                            }
                                            else if (Global.DevicesSetupWindow.CustomControl.ConfigDeviceWindowShow)//DEVICES SETUP，CONFIG DEVICE页面
                                            {
                                                Global.DevicesSetupWindow.CustomControl.ConfigDeviceWindow._Close();//关闭
                                            }
                                            else if (Global.DevicesSetupWindow.CustomControl.ParameterSettingsWindowShow)//DEVICES SETUP，PARAMETER SETTINGS页面
                                            {
                                                VisionSystemControlLibrary.GlobalWindows.ParameterSettings_Window.ParameterSettingsControl._Close();//关闭
                                            }
                                            else//DEVICES SETUP页面
                                            {
                                                //不执行操作
                                            }

                                            break;
                                        }
                                    }
                                }

                                Global.DevicesSetupWindow.CustomControl.Invoke(new EventHandler(delegate { Global.DevicesSetupWindow.CustomControl._RefreshList(); }));//更新程序页面

                                break;
                                //
                            case ApplicationInterface.SystemConfiguration://SYSTEM页面
                                //
                                break;
                            default:
                                break;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        //不执行操作
                    }

                    for (i = 0; i < iCameraIndex.Count; i++)
                    {
                        workControl.Invoke(new EventHandler(delegate { workControl._SetCameraData(iCameraIndex[i]); }));//更新程序页面

                        _SetFaultMessage(iCameraIndex[i]);

                        //

                        workControl.Invoke(new EventHandler(delegate { workControl._UpdateMachineSpeed(iCameraIndex[i], -1, 0); }));//更新程序页面
                    }
                }
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：向下位机发送
        // 输入参数：1.command：指令类型
        //         2.Cameratype：相机类型
        //         3.iDataIndex：数据索引值（DEVICES SETUP页面为设备信息数据索引值，其它为相机数据索引值）
        //         4.dImageScale：图像尺寸数据
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _SendCommand_Image(CommunicationInstructionType command, VisionSystemClassLibrary.Enum.CameraType Cameratype = VisionSystemClassLibrary.Enum.CameraType.Camera_1, Int32 iDataIndex = 0, Double dImageScale = 1.0)
        {
            try
            {
                switch (command)//指令
                {
                    case CommunicationInstructionType.Work:

                        //WORK页面操作，格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 图像工具数据（1，包含工具；0，不包含工具） + 图像尺寸类型数据 + 图像索引值数据

                        Byte[] Work_Data = _GenerateInstruction(command, Cameratype, true, dImageScale);//生成指令数据

                        Byte[] WorkClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iDataIndex].DeviceInformation.IP).GetAddressBytes();//获取相机IP地址

                        Global.NetServer.ClientData[WorkClientIP[3]]._Send(Work_Data);//发送数据
                        //
                        break;
                        //
                    case CommunicationInstructionType.Live:

                        //LIVE VIEW页面操作，格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 图像工具数据（1，包含工具；0，不包含工具） + 图像尺寸类型数据 + 图像索引值数据

                        Byte[] Live_Data = _GenerateInstruction(command, Cameratype, true, dImageScale);//生成指令数据

                        Byte[] LiveClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iDataIndex].DeviceInformation.IP).GetAddressBytes();//获取相机IP地址

                        Global.NetServer.ClientData[LiveClientIP[3]]._Send(Live_Data);//发送数据
                        //
                        break;
                        //
                    case CommunicationInstructionType.TolerancesSettings_Rejects:

                        //TOLERANCES SETTINGS页面，查询REJECTS图像，格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 图像工具数据（1，包含工具；0，不包含工具） + 图像尺寸类型数据 + 图像索引值数据

                        Byte[] TolerancesSettingsRejects_Data = _GenerateInstruction(command, Cameratype, true, dImageScale);//生成指令数据

                        Byte[] TolerancesSettingsRejects_ClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iDataIndex].DeviceInformation.IP).GetAddressBytes();//获取相机IP地址

                        Global.NetServer.ClientData[TolerancesSettingsRejects_ClientIP[3]]._Send(TolerancesSettingsRejects_Data);//发送数据
                        //
                        break;
                        //
                    case CommunicationInstructionType.TolerancesSettings_Live:

                        //TOLERANCES SETTINGS页面，查询LIVE图像，格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 图像工具数据（1，包含工具；0，不包含工具） + 图像尺寸类型数据 + 图像索引值数据

                        Byte[] TolerancesSettingsLive_Data = _GenerateInstruction(command, Cameratype, true, dImageScale);//生成指令数据

                        Byte[] TolerancesSettingsLive_ClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iDataIndex].DeviceInformation.IP).GetAddressBytes();//获取相机IP地址

                        Global.NetServer.ClientData[TolerancesSettingsLive_ClientIP[3]]._Send(TolerancesSettingsLive_Data);//发送数据
                        //
                        break;
                        //
                    case CommunicationInstructionType.QualityCheck_LoadSample:

                        //QUALITY CHECK页面操作，查看实时图像，格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 图像工具数据（1，包含工具；0，不包含工具） + 图像尺寸类型数据

                        Byte[] QualityCheckLoadSample_Data = _GenerateInstruction(command, Cameratype, false, dImageScale);//生成指令数据

                        Byte[] QualityCheckLoadSample_ClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iDataIndex].DeviceInformation.IP).GetAddressBytes();//获取相机IP地址

                        Global.NetServer.ClientData[QualityCheckLoadSample_ClientIP[3]]._Send(QualityCheckLoadSample_Data);//发送数据
                        //
                        break;
                        //
                    case CommunicationInstructionType.QualityCheck_LiveView:

                        //QUALITY CHECK页面操作，查看实时图像，格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 图像工具数据（1，包含工具；0，不包含工具） + 图像尺寸类型数据

                        Byte[] QualityCheckLiveView_Data = _GenerateInstruction(command, Cameratype, false, dImageScale);//生成指令数据

                        Byte[] QualityCheckLiveView_ClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iDataIndex].DeviceInformation.IP).GetAddressBytes();//获取相机IP地址

                        Global.NetServer.ClientData[QualityCheckLiveView_ClientIP[3]]._Send(QualityCheckLiveView_Data);//发送数据
                        //
                        break;
                        //
                    case CommunicationInstructionType.DevicesSetup_ConfigImage_Live:

                        //CONFIG IMAGE页面操作，实时图像显示，格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 图像工具数据（1，包含工具；0，不包含工具） + 图像尺寸类型数据
                       
                        Byte[] DevicesSetupConfigImageLive_Data = _GenerateInstruction(command, Cameratype, false, dImageScale);//生成指令数据

                        Byte[] DevicesSetupConfigImageLive_ClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iDataIndex].DeviceInformation.IP).GetAddressBytes();//获取相机IP地址

                        Global.NetServer.ClientData[DevicesSetupConfigImageLive_ClientIP[3]]._Send(DevicesSetupConfigImageLive_Data);//发送数据
                        //
                        break;
                        //
                    default:
                        break;
                }
            }
            catch (System.Exception ex)
            {
                //不执行操作
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：向下位机发送
        // 输入参数：1.command：指令类型
        //         2.Cameratype：相机类型
        //         3.iDataIndex：数据索引值（DEVICES SETUP页面为设备信息数据索引值，其它为相机数据索引值）
        //         4.dImageScale：图像尺寸数据
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _SendCommand_Image_Learn(CommunicationInstructionType command, VisionSystemClassLibrary.Enum.CameraType Cameratype, Int32 iDataIndex, Double dImageScale, Int32 iImageType)
        {
            try
            {
                switch (command)//指令
                {
                    case CommunicationInstructionType.QualityCheck_LearnSample:

                        //QUALITY CHECK页面操作，自学习阈值或更新，格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 图像工具数据（1，包含工具；0，不包含工具） + 图像尺寸类型数据  + 图像类型（1，在线；2，学习；3，剔除）

                        Byte[] QualityCheckLearnSample_Data = _GenerateInstruction(command, Cameratype, false, dImageScale, iImageType);//生成指令数据

                        Byte[] QualityCheckLearnSample_ClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iDataIndex].DeviceInformation.IP).GetAddressBytes();//获取相机IP地址

                        Global.NetServer.ClientData[QualityCheckLearnSample_ClientIP[3]]._Send(QualityCheckLearnSample_Data);//发送数据
                        //
                        break;
                        //
                    default:
                        break;
                }
            }
            catch (System.Exception ex)
            {
                //不执行操作
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：向下位机发送
        // 输入参数：1.command：指令类型
        //         2.Cameratype：相机类型
        //         3.iDataIndex：数据索引值（相机数据索引值）
        //         4.iValue_1：数值
        //         5.iValue_2：数值
        //         6.iValue_3：数值
        //         7.iValue_4：数值
        //         8.iValue_5：数值
        //         9.iValue_6：数值
        //         10.iValue_7：数值
        //         11.iValue_8：数值
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _SendCommand_Value(CommunicationInstructionType command, VisionSystemClassLibrary.Enum.CameraType Cameratype = VisionSystemClassLibrary.Enum.CameraType.Camera_1, Int32 iDataIndex = 0, Int32 iValue_1 = 0, Int32 iValue_2 = 0, Int32 iValue_3 = 0, Int32 iValue_4 = 0, Int32 iValue_5 = 0, Int32 iValue_6 = 0, Int32 iValue_7 = 0, Int32 iValue_8 = 0)
        {
            try
            {
                switch (command)//指令
                {
                    case CommunicationInstructionType.Load:

                        //启动载入，格式：

                        //未完成文件发送
                        //服务端->客户端（数据）：指令类型 + 相机类型数据 + 文件传输状态（1，启动发送；2，文件发送中，文件索引值（从2开始））

                        Byte[] Load_Data = _GenerateInstruction(command, Cameratype, (Int32)1);//生成指令数据

                        Byte[] Load_ClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iDataIndex].DeviceInformation.IP).GetAddressBytes();//获取当前选中的相机的IP地址

                        Global.NetServer.ClientData[Load_ClientIP[3]]._Send(Load_Data);//发送数据
                        //
                        break;
                        //
                    case CommunicationInstructionType.DevicesSetup_ConfigSensor_MaxADC:

                        //DeviceSetup ConfigImage页面操作，工具参数，格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 烟支数量（N）

                        Byte[] ConfigSensor_MaxADC = _GenerateInstruction(command, Cameratype, iValue_1);//生成指令数据

                        Byte[] ConfigSensor_MaxADC_ClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iDataIndex].DeviceInformation.IP).GetAddressBytes();//获取当前选中的相机的IP地址

                        Global.NetServer.ClientData[ConfigSensor_MaxADC_ClientIP[3]]._Send(ConfigSensor_MaxADC);//发送数据

                        //
                        break;
                    //
                    case CommunicationInstructionType.DevicesSetup_TestIOExit:

                        //DEVICES SETUP页面操作，TEST I/O，格式：    
                        //服务端->客户端：指令类型 + 相机类型数据

                        Byte[] DevicesSetupTestIOExit_Data = _GenerateInstruction(command, Cameratype);//生成指令数据

                        Byte[] DevicesSetupTestIOExit_ClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iDataIndex].DeviceInformation.IP).GetAddressBytes();//获取当前选中的相机的IP地址

                        Global.NetServer.ClientData[DevicesSetupTestIOExit_ClientIP[3]]._Send(DevicesSetupTestIOExit_Data);//发送数据
                        //
                        break;
                        //
                    case CommunicationInstructionType.DevicesSetup_TestIOEnter:

                        //DEVICES SETUP页面操作，TEST I/O，格式：    
                        //服务端->客户端：指令类型 + 相机类型数据

                        Byte[] DevicesSetupTestIOEnter_Data = _GenerateInstruction(command, Cameratype);//生成指令数据

                        Byte[] DevicesSetupTestIOEnter_ClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iDataIndex].DeviceInformation.IP).GetAddressBytes();//获取当前选中的相机的IP地址

                        Global.NetServer.ClientData[DevicesSetupTestIOEnter_ClientIP[3]]._Send(DevicesSetupTestIOEnter_Data);//发送数据
                        //
                        break;
                        //
                    case CommunicationInstructionType.DevicesSetup_ResetDevice:

                        //DEVICES SETUP页面操作，RESET DEVICE，格式：    
                        //服务端->客户端：指令类型 + 相机类型数据

                        Byte[] DevicesSetupResetDevice_Data = _GenerateInstruction(command, Cameratype);//生成指令数据

                        Byte[] DevicesSetupResetDevice_ClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iDataIndex].DeviceInformation.IP).GetAddressBytes();//获取当前选中的相机的IP地址

                        Global.NetServer.ClientData[DevicesSetupResetDevice_ClientIP[3]]._Send(DevicesSetupResetDevice_Data);//发送数据
                        //
                        break;
                        //
                    case CommunicationInstructionType.DevicesSetup_ConfigImage_Save:

                        //DeviceSetup ConfigImage页面操作，保存数据参数，格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 是否保存数据（1，是；0，否）

                        Byte[] ConfigImageSave_Data = _GenerateInstruction(command, Cameratype, iValue_1);//生成指令数据

                        Byte[] ConfigImageSave_ClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iDataIndex].DeviceInformation.IP).GetAddressBytes();//获取当前选中的相机的IP地址

                        Global.NetServer.ClientData[ConfigImageSave_ClientIP[3]]._Send(ConfigImageSave_Data);//发送数据
                        //
                        break;
                        //
                    case CommunicationInstructionType.DevicesSetup_ConfigImage_Focus:

                        //DeviceSetup ConfigImage页面操作，保存数据参数，格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 聚焦参数

                        //（待定...）

                        //
                        break;
                        //
                    case CommunicationInstructionType.DevicesSetup_ConfigImage_White:

                        //DeviceSetup ConfigImage页面操作，保存数据参数，格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 白平衡参数

                        //（待定...）

                        //
                        break;
                        //
                    case CommunicationInstructionType.DevicesSetup_ConfigImage_Parameter:

                        //DeviceSetup ConfigImage页面操作，工具参数，格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 光照时间 + 光照强度 + 增益 + 曝光时间 + 白平衡 + 白平衡（红） + 白平衡（绿） + 白平衡（蓝）

                        Byte[] ConfigImageParameter_Data = _GenerateInstruction(command, Cameratype, iValue_1, iValue_2, iValue_3, iValue_4, iValue_5, iValue_6, iValue_7, iValue_8);//生成指令数据

                        Byte[] ConfigImageParameter_ClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iDataIndex].DeviceInformation.IP).GetAddressBytes();//获取当前选中的相机的IP地址

                        Global.NetServer.ClientData[ConfigImageParameter_ClientIP[3]]._Send(ConfigImageParameter_Data);//发送数据

                        //
                        break;
                        //
                    case CommunicationInstructionType.DevicesSetup_ConfigImage_Enter:

                        //DeviceSetup ConfigImage页面操作，保存数据参数，格式：
                        //服务端->客户端：指令类型 + 相机类型数据
                        Byte[] ConfigImageEnter_Data = _GenerateInstruction(command, Cameratype);//生成指令数据

                        Byte[] ConfigImageEnter_ClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iDataIndex].DeviceInformation.IP).GetAddressBytes();//获取相机IP地址

                        Global.NetServer.ClientData[ConfigImageEnter_ClientIP[3]]._Send(ConfigImageEnter_Data);//发送数据

                        //
                        break;
                        //
                    case CommunicationInstructionType.DevicesSetup_AlignDateTime:

                        //DEVICES SETUP页面，点击【ALIGN DATE/TIME】按钮，格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 日期时间数据

                        VisionSystemClassLibrary.Struct.SYSTEMTIME SystemTime = new VisionSystemClassLibrary.Struct.SYSTEMTIME();
                        GetLocalTime(ref SystemTime);//获取日期时间

                        Byte[] AlignDateTime_Data = _GenerateInstruction(command, Cameratype, SystemTime);//生成指令数据

                        Byte[] AlignDateTimeClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iDataIndex].DeviceInformation.IP).GetAddressBytes();//获取当前选中的相机的IP地址

                        Global.NetServer.ClientData[AlignDateTimeClientIP[3]]._Send(AlignDateTime_Data);//发送数据
                        //
                        break;
                        //
                    case CommunicationInstructionType.DevicesSetup_ParameterSettings:

                        //DEVICES SETUP页面，点击【PARAMETER SETTINGS】按钮，格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 参数个数（Int32） + 参数数组（Int32）

                        Byte[] ParameterSettings_Data = _GenerateInstruction(command, Cameratype, Global.VisionSystem.Camera[iDataIndex].DeviceParameter.Parameter.ToArray());//生成指令数据

                        Byte[] ParameterSettingsClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iDataIndex].DeviceInformation.IP).GetAddressBytes();//获取当前选中的相机的IP地址

                        Global.NetServer.ClientData[ParameterSettingsClientIP[3]]._Send(ParameterSettings_Data);//发送数据
                        //
                        break;
                        //
                    case CommunicationInstructionType.TolerancesSettings_Graphs:

                        //TOLERANCES SETTINGS页面，查询曲线图数据，格式：
                        //服务端->客户端：指令类型 + 相机类型

                        Byte[] TolerancesSettingsGraphs_Data = _GenerateInstruction(command, Cameratype);//生成指令数据

                        Byte[] TolerancesSettingsGraphs_ClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iDataIndex].DeviceInformation.IP).GetAddressBytes();//获取相机IP地址

                        Global.NetServer.ClientData[TolerancesSettingsGraphs_ClientIP[3]]._Send(TolerancesSettingsGraphs_Data);//发送数据
                        //
                        break;
                        //
                    case CommunicationInstructionType.TolerancesSettings_Tool:

                        //TOLERANCES SETTINGS页面，工具开关，格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 工具索引数值 + 工具开关数值

                        Byte[] TolerancesSettingsTool_Data = _GenerateInstruction(command, Cameratype, iValue_1, iValue_2);//生成指令数据

                        Byte[] TolerancesSettingsTool_ClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iDataIndex].DeviceInformation.IP).GetAddressBytes();//获取相机IP地址

                        Global.NetServer.ClientData[TolerancesSettingsTool_ClientIP[3]]._Send(TolerancesSettingsTool_Data);//发送数据
                        //
                        break;
                        //
                    case CommunicationInstructionType.TolerancesSettings_ToolIndex:

                        //TOLERANCES SETTINGS页面，双击选中工具，格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 工具索引数值

                        Byte[] TolerancesSettings_ToolIndex_Data = _GenerateInstruction(command, Cameratype, iValue_1);//生成指令数据

                        Byte[] TolerancesSettings_ToolIndex_ClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iDataIndex].DeviceInformation.IP).GetAddressBytes();//获取相机IP地址

                        Global.NetServer.ClientData[TolerancesSettings_ToolIndex_ClientIP[3]]._Send(TolerancesSettings_ToolIndex_Data);//发送数据
                        //
                        break;
                    //
                    case CommunicationInstructionType.TolerancesSettings_Learn:

                        //TOLERANCES SETTINGS页面，学习，格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 工具索引数值

                        Byte[] TolerancesSettingsLearn_Data = _GenerateInstruction(command, Cameratype, iValue_1);//生成指令数据

                        Byte[] TolerancesSettingsLearn_ClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iDataIndex].DeviceInformation.IP).GetAddressBytes();//获取相机IP地址

                        Global.NetServer.ClientData[TolerancesSettingsLearn_ClientIP[3]]._Send(TolerancesSettingsLearn_Data);//发送数据
                        //
                        break;
                        //
                    case CommunicationInstructionType.TolerancesSettings_MinMax:

                        //TOLERANCES SETTINGS页面，曲线图范围数值，格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 工具索引数值 + 最小值数值（有效值） + 最大值数值（有效值） + 最小值数值（坐标轴数值） + 最大值数值（坐标轴数值）

                        Byte[] TolerancesSettingsMinMax_Data = _GenerateInstruction(command, Cameratype, iValue_1, iValue_2, iValue_3, iValue_4, iValue_5);//生成指令数据

                        Byte[] TolerancesSettingsMinMax_ClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iDataIndex].DeviceInformation.IP).GetAddressBytes();//获取相机IP地址

                        Global.NetServer.ClientData[TolerancesSettingsMinMax_ClientIP[3]]._Send(TolerancesSettingsMinMax_Data);//发送数据
                        //
                        break;
                        //
                    case CommunicationInstructionType.TolerancesSettings_ResetGraphs:

                        //TOLERANCES SETTINGS页面，复位曲线图，格式：
                        //服务端->客户端：指令类型 + 相机类型数据

                        Byte[] TolerancesSettingsResetGraphs_Data = _GenerateInstruction(command, Cameratype);//生成指令数据

                        Byte[] TolerancesSettingsResetGraphs_ClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iDataIndex].DeviceInformation.IP).GetAddressBytes();//获取相机IP地址

                        Global.NetServer.ClientData[TolerancesSettingsResetGraphs_ClientIP[3]]._Send(TolerancesSettingsResetGraphs_Data);//发送数据
                        //
                        break;
                        //
                    case CommunicationInstructionType.TolerancesSettings_Enter:

                        //TOLERANCES SETTINGS页面，进入页面，格式：
                        //服务端->客户端：指令类型 + 相机类型数据

                        Byte[] TolerancesSettingsEnter_Data = _GenerateInstruction(command, Cameratype);//生成指令数据

                        Byte[] TolerancesSettingsEnter_ClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iDataIndex].DeviceInformation.IP).GetAddressBytes();//获取相机IP地址

                        Global.NetServer.ClientData[TolerancesSettingsEnter_ClientIP[3]]._Send(TolerancesSettingsEnter_Data);//发送数据
                        //
                        break;
                        //
                    case CommunicationInstructionType.TolerancesSettings_SaveProduct:

                        //TOLERANCES SETTINGS页面，保存数据，格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 是否保存数据（1，是；0，否）

                        Byte[] TolerancesSettingsSaveProduct_Data = _GenerateInstruction(command, Cameratype, iValue_1);//生成指令数据

                        Byte[] TolerancesSettingsSaveProduct_ClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iDataIndex].DeviceInformation.IP).GetAddressBytes();//获取相机IP地址

                        Global.NetServer.ClientData[TolerancesSettingsSaveProduct_ClientIP[3]]._Send(TolerancesSettingsSaveProduct_Data);//发送数据
                        //
                        break;
                        //
                    case CommunicationInstructionType.TolerancesSettings_EjectLevel:

                        //TOLERANCES SETTINGS页面，灵敏度，格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 灵敏度 + 调节灵敏度标记（0,：true;1：调节光电空头校准值）

                        Byte[] TolerancesSettingsEjectLevel_Data = _GenerateInstruction(command, Cameratype, iValue_1, iValue_2);//生成指令数据

                        Byte[] TolerancesSettingsEjectLevel_ClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iDataIndex].DeviceInformation.IP).GetAddressBytes();//获取相机IP地址

                        Global.NetServer.ClientData[TolerancesSettingsEjectLevel_ClientIP[3]]._Send(TolerancesSettingsEjectLevel_Data);//发送数据
                        //
                        break;
                        //
                    case CommunicationInstructionType.Live_SelfTrigger:

                        //LIVE VIEW页面操作，SELF TRIGGER，格式：
                        //服务端->客户端：指令类型 + 相机类型 + 操作数据（1，打开；0，关闭）

                        Byte[] LiveSelfTrigger_Data = _GenerateInstruction(command, Cameratype, Convert.ToBoolean(iValue_1));//生成指令数据

                        Byte[] LiveSelfTrigger_ClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iDataIndex].DeviceInformation.IP).GetAddressBytes();//获取当前选中的相机的IP地址

                        Global.NetServer.ClientData[LiveSelfTrigger_ClientIP[3]]._Send(LiveSelfTrigger_Data);//发送数据
                        //
                        break;
                        //
                    case CommunicationInstructionType.QualityCheck_SaveProduct:

                        //QUALITY CHECK页面操作，保存数据参数，格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 是否保存数据（1，是；0，否）

                        Byte[] QualityCheckSaveProduct_Data = _GenerateInstruction(command, Cameratype, iValue_1);//生成指令数据

                        Byte[] QualityCheckSaveProduct_ClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iDataIndex].DeviceInformation.IP).GetAddressBytes();//获取当前选中的相机的IP地址

                        Global.NetServer.ClientData[QualityCheckSaveProduct_ClientIP[3]]._Send(QualityCheckSaveProduct_Data);//发送数据
                        //
                        break;
                        //
                    case CommunicationInstructionType.QualityCheck_ManageTools:

                        //QUALITY CHECK页面操作，工具管理，格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 启用工具标记 + 当前工具索引 + 图像类型（1，在线；2，学习；3，剔除）
                        
                        Byte[] QualityCheckManageTools_Data = _GenerateInstruction(command, Cameratype, Global.VisionSystem.Camera[iDataIndex].Tools, iValue_1, iValue_2);//生成指令数据

                        Byte[] QualityCheckManageTools_ClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iDataIndex].DeviceInformation.IP).GetAddressBytes();//获取当前选中的相机的IP地址

                        Global.NetServer.ClientData[QualityCheckManageTools_ClientIP[3]]._Send(QualityCheckManageTools_Data);//发送数据
                        //
                        break;
                        //
                    case CommunicationInstructionType.QualityCheck_ToolParamter:

                        //QUALITY CHECK页面操作，工具参数，格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 工具索引 + 图像类型（1，在线；2，学习；3，剔除） + 工具参数
                        
                        Byte[] QualityCheckToolParamter_Data = _GenerateInstruction(command, Cameratype, iValue_1, iValue_2, Global.VisionSystem.Camera[iDataIndex].Tools[iValue_1].Arithmetic);//生成指令数据

                        Byte[] QualityCheckToolParamter_ClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iDataIndex].DeviceInformation.IP).GetAddressBytes();//获取当前选中的相机的IP地址

                        Global.NetServer.ClientData[QualityCheckToolParamter_ClientIP[3]]._Send(QualityCheckToolParamter_Data);//发送数据
                        //
                        break;
                        //
                    case CommunicationInstructionType.QualityCheck_CurrentTool:

                        //QUALITY CHECK页面操作，当前工具设置，格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 工具索引 + 图像类型（1，在线；2，学习；3，剔除）

                        Byte[] QualityCheckCurrentTool_Data = _GenerateInstruction(command, Cameratype, iValue_1, iValue_2);//生成指令数据

                        Byte[] QualityCheckCurrentTool_ClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iDataIndex].DeviceInformation.IP).GetAddressBytes();//获取当前选中的相机的IP地址

                        Global.NetServer.ClientData[QualityCheckCurrentTool_ClientIP[3]]._Send(QualityCheckCurrentTool_Data);//发送数据
                        //
                        break;
                        //
                    case CommunicationInstructionType.QualityCheck_Enter:

                        //QUALITY CHECK页面，进入页面，格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 工具索引

                        Byte[] QualityCheckEnter_Data = _GenerateInstruction(command, Cameratype, iValue_1);//生成指令数据

                        Byte[] QualityCheckEnter_ClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iDataIndex].DeviceInformation.IP).GetAddressBytes();//获取相机IP地址

                        Global.NetServer.ClientData[QualityCheckEnter_ClientIP[3]]._Send(QualityCheckEnter_Data);//发送数据
                        //
                        break;
                        //
                    case CommunicationInstructionType.QualityCheck_TolerancesValue:

                        //QUALITY CHECK页面，图像学习完成后，获取公差上下限，格式：
                        //服务端->客户端：指令类型 + 相机类型

                        Byte[] QualityCheckTolerancesValue_Data = _GenerateInstruction(command, Cameratype);//生成指令数据

                        Byte[] QualityCheckTolerancesValue_ClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iDataIndex].DeviceInformation.IP).GetAddressBytes();//获取相机IP地址

                        Global.NetServer.ClientData[QualityCheckTolerancesValue_ClientIP[3]]._Send(QualityCheckTolerancesValue_Data);//发送数据
                        //
                        break;
                        //
                    case CommunicationInstructionType.TitleBar_State:

                        //标题栏【STATE】按钮操作，设备状态，格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 设备状态（VisionSystemClassLibrary.Enum.DeviceState）

                        Byte[] TitleBarState_Data = _GenerateInstruction(command, Cameratype, iValue_1);//生成指令数据

                        Byte[] TitleBarState_ClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iDataIndex].DeviceInformation.IP).GetAddressBytes();//获取当前选中的相机的IP地址

                        Global.NetServer.ClientData[TitleBarState_ClientIP[3]]._Send(TitleBarState_Data);//发送数据
                        //
                        break;
                        //
                    case CommunicationInstructionType.Statistics_GetRecordList:

                        //STATISTICS页面，获取统计数据列表，格式：
                        //服务端->客户端：指令类型 + 相机类型数据

                        Byte[] StatisticsGetRecordList_Data = _GenerateInstruction(command, Cameratype);//生成指令数据

                        Byte[] StatisticsGetRecordList_ClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iDataIndex].DeviceInformation.IP).GetAddressBytes();//获取相机IP地址

                        Global.NetServer.ClientData[StatisticsGetRecordList_ClientIP[3]]._Send(StatisticsGetRecordList_Data);//发送数据
                        //
                        break;
                        //
                    case CommunicationInstructionType.Statistics_DeleteRecord:

                        //STATISTICS页面，删除统计数据，格式：    
                        //服务端->客户端：指令类型 + 相机类型数据 + 删除统计数据方式（0，删除所有；1，删除指定班次；2，删除指定记录）
                        //（0） + 无

                        Byte[] StatisticsDeleteRecord_Data = _GenerateInstruction(command, Cameratype, iValue_1);//生成指令数据

                        Byte[] StatisticsDeleteRecord_ClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iDataIndex].DeviceInformation.IP).GetAddressBytes();//获取当前选中的相机的IP地址

                        Global.NetServer.ClientData[StatisticsDeleteRecord_ClientIP[3]]._Send(StatisticsDeleteRecord_Data);//发送数据
                        //
                        break;
                        //
                    case CommunicationInstructionType.CurrentFaultMessage:

                        //FAULT MESSAGE，获取当前故障信息，格式：
                        //服务端->客户端：指令类型 + 相机类型数据

                        Byte[] CurrentFaultMessage_Data = _GenerateInstruction(command, Cameratype);//生成指令数据

                        Byte[] CurrentFaultMessage_ClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iDataIndex].DeviceInformation.IP).GetAddressBytes();//获取当前选中的相机的IP地址

                        Global.NetServer.ClientData[CurrentFaultMessage_ClientIP[3]]._Send(CurrentFaultMessage_Data);//发送数据
                        //
                        break;
                        //
                    case CommunicationInstructionType.GetFaultMessages:

                        //FAULT MESSAGE，获取故障信息，格式：
                        //服务端->客户端：指令类型 + 相机类型数据

                        Byte[] GetFaultMessages_Data = _GenerateInstruction(command, Cameratype);//生成指令数据

                        Byte[] GetFaultMessages_ClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iDataIndex].DeviceInformation.IP).GetAddressBytes();//获取当前选中的相机的IP地址

                        Global.NetServer.ClientData[GetFaultMessages_ClientIP[3]]._Send(GetFaultMessages_Data);//发送数据
                        //
                        break;
                        //
                    case CommunicationInstructionType.ClearAllFaultMessages:

                        //FAULT MESSAGE，清除所有故障信息，格式：
                        //服务端->客户端：指令类型 + 相机类型数据

                        Byte[] ClearAllFaultMessages_Data = _GenerateInstruction(command, Cameratype);//生成指令数据

                        Byte[] ClearAllFaultMessages_ClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iDataIndex].DeviceInformation.IP).GetAddressBytes();//获取当前选中的相机的IP地址

                        Global.NetServer.ClientData[ClearAllFaultMessages_ClientIP[3]]._Send(ClearAllFaultMessages_Data);//发送数据
                        //
                        break;
                        //
                    default:
                        break;
                }
            }
            catch (System.Exception ex)
            {
                //不执行操作
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：向下位机发送
        // 输入参数：1.command：指令类型
        //         2.Cameratype：相机类型
        //         3.iDataIndex：数据索引值（相机数据索引值）
        //         4.iValue_1：数值
        //         5.iValue_2：数值
        //         6.roi：工作区域
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _SendCommand_Value(CommunicationInstructionType command, VisionSystemClassLibrary.Enum.CameraType Cameratype, Int32 iDataIndex, Int32 iValue_1, Int32 iValue_2, VisionSystemClassLibrary.Struct.ROI roi)
        {
            try
            {
                switch (command)//指令
                {
                    case CommunicationInstructionType.QualityCheck_WorkArea:

                        //QUALITY CHECK页面操作，工作区域，格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 工具索引 + 图像类型（1，在线；2，学习；3，剔除） + 工作区域

                        Byte[] QualityCheckWorkArea_Data = _GenerateInstruction(command, Cameratype, iValue_1, iValue_2, roi);//生成指令数据

                        Byte[] QualityCheckWorkArea_ClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iDataIndex].DeviceInformation.IP).GetAddressBytes();//获取当前选中的相机的IP地址

                        Global.NetServer.ClientData[QualityCheckWorkArea_ClientIP[3]]._Send(QualityCheckWorkArea_Data);//发送数据
                        //
                        break;
                    //
                    default:
                        break;
                }
            }
            catch (System.Exception ex)
            {
                //不执行操作
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：向下位机发送
        // 输入参数：1.command：指令类型
        //         2.Cameratype：相机类型
        //         3.iDataIndex：数据索引值（相机数据索引值）
        //         4.iValue_1：数值
        //         5.iValue_2：数值
        //         6.iValue_3：数值
        //         7.iData：数值
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _SendCommand_Value(CommunicationInstructionType command, VisionSystemClassLibrary.Enum.CameraType Cameratype, Int32 iDataIndex, Int32 iValue_1, Int32 iValue_2, Int32 iValue_3, Byte[] iData)
        {
            try
            {
                switch (command)//指令
                {
                    case CommunicationInstructionType.DevicesSetup_ConfigSensor_Parameter:

                        //DeviceSetup ConfigImage页面操作，工具参数，格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 烟支数量（N）+ 传感器校准选中状态 + 校准标记（0，未校准；1，校准中） + 烟支校准值（N支）

                        Byte[] ConfigImage_Parameter_Data = _GenerateInstruction(command, Cameratype, iValue_1, iValue_2, iValue_3, iData);//生成指令数据

                        Byte[] ConfigImage_Parameter_ClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iDataIndex].DeviceInformation.IP).GetAddressBytes();//获取当前选中的相机的IP地址

                        Global.NetServer.ClientData[ConfigImage_Parameter_ClientIP[3]]._Send(ConfigImage_Parameter_Data);//发送数据

                        //
                        break;
                    //
                    default:
                        break;
                }
            }
            catch (System.Exception ex)
            {
                //不执行操作
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：向下位机发送
        // 输入参数：1.command：指令类型
        //         2.Cameratype：相机类型
        //         3.CameraChooseState：设置相机模式：单/双相机模式
        //         4.iDataIndex：数据索引值（相机数据索引值）
        //         5.iValue_1：数值
        //         60.sValue_2：数值
        //         7.sShiftPath：班次数据路径
        //         8.sFileName：文件名称
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _SendCommand_Value(CommunicationInstructionType command, VisionSystemClassLibrary.Enum.CameraType Cameratype, Int32 CameraChooseState, Int32 iDataIndex, Int32 iValue_1, Int32 iValue_2, UInt64 iValue_3, String sValue_1, String sShiftPath, String sFileName)
        {
            try
            {
                switch (command)//指令
                {
                    case CommunicationInstructionType.DeviceState_Synchronization:

                        //系统状态同步，格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 设置相机模式 + 当前选择机器 + 设备状态（VisionSystemClassLibrary.Enum.DeviceState） + 故障信息使能状态 + 品牌长度 + 品牌名称 + 班次数据（文件）

                        Byte[] DeviceStateSynchronization_Data = _GenerateInstruction(command, Cameratype, CameraChooseState, iValue_1, iValue_2, iValue_3, sValue_1);//生成指令数据

                        Byte[] DeviceStateSynchronization_ClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iDataIndex].DeviceInformation.IP).GetAddressBytes();//获取当前选中的相机的IP地址

                        //

                        String sSafeFileName_Send = sFileName;//文件名和扩展名，文件名不包含路径
                        String sFileName_Send = sShiftPath + sFileName;//文件名和扩展名，文件名包含路径

                        //

                        Global.NetServer.ClientData[DeviceStateSynchronization_ClientIP[3]]._Send(sSafeFileName_Send, sFileName_Send, DeviceStateSynchronization_Data);//发送数据

                        //
                        break;
                        //
                    default:
                        break;
                }
            }
            catch (System.Exception ex)
            {
                //不执行操作
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：向下位机发送
        // 输入参数：1.command：指令类型
        //         2.sIP：IP地址
        //         3.iValue_1：数值
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _SendCommand_Value(CommunicationInstructionType command, String sIP, Int32 iValue_1)
        {
            try
            {
                switch (command)//指令
                {
                    case CommunicationInstructionType.Network_Check:

                        //网络检测，格式：
                        //服务端->客户端：指令类型 + 数据（上位机，1；下位机，2）

                        Byte[] NetworkCheck_Data = _GenerateInstruction(command, iValue_1);//生成指令数据

                        Byte[] NetworkCheck_ClientIP = IPAddress.Parse(sIP).GetAddressBytes();//获取相机IP地址

                        Global.NetServer.ClientData[NetworkCheck_ClientIP[3]]._Send(NetworkCheck_Data);//发送数据
                        //
                        break;
                        //
                    default:
                        break;
                }
            }
            catch (System.Exception ex)
            {
                //不执行操作
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：向下位机发送
        // 输入参数：1.command：指令类型
        //         2.Cameratype：相机类型
        //         3.iDataIndex：数据索引值（相机数据索引值）
        //         4.iValue_1：数值
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _SendCommand_Value(CommunicationInstructionType command, VisionSystemClassLibrary.Enum.CameraType Cameratype, Int32 iDataIndex, UInt32 iValue_1)
        {
            try
            {
                switch (command)//指令
                {
                    case CommunicationInstructionType.DevicesSetup_TestIO:

                        //DEVICES SETUP页面操作，TEST I/O，格式：    
                        //服务端->客户端：指令类型 + 相机类型数据 + 输出数据

                        Byte[] DevicesSetupTestIO_Data = _GenerateInstruction(command, Cameratype, iValue_1);//生成指令数据

                        Byte[] DevicesSetupTestIO_ClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iDataIndex].DeviceInformation.IP).GetAddressBytes();//获取当前选中的相机的IP地址

                        Global.NetServer.ClientData[DevicesSetupTestIO_ClientIP[3]]._Send(DevicesSetupTestIO_Data);//发送数据
                        //
                        break;
                        //
                    default:
                        break;
                }
            }
            catch (System.Exception ex)
            {
                //不执行操作
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：向下位机发送
        // 输入参数：1.command：指令类型
        //         2.Cameratype：相机类型
        //         3.iDataIndex：数据索引值（相机数据索引值）
        //         4.iSelectedIPAddress：设置为的IP地址
        //         5.SelectedCameratype：设置为的相机类型数据
        //         6.byteSelectedPort：设置为的相机端口
        //         7.CameraChooseState：设置相机模式：单/双相机模式
        //         8.uCameraFaultState：设置为的相机故障标记
        //         9.bCheckEnable：社会相机检测使能
        //         10.bCameraAngle：选择为的相机旋转角度
        //         11.vVideoColor：相机颜色
        //         12.vVideoResolution：相机分辨率
        //         13.bSerialPort：是否为串口
        //         14.tTobaccoSortType：烟支排列类型
        //         15.bBitmapResize：选择为的相机数据截取区域缩放
        //         16.bBitmapCenter：选择为的相机数据截取区域缩放后是否居中
        //         17.bBitmapAxis：选择为的相机数据截取区域粘贴区域
        //         18.rBitmapArea：选择为的相机数据截取区域
        //         19.bCameraFlip：相机镜像标记
        //         20.sSensorProductType：传感器应用场景
        //         21.relevancyCameraInformation：相机关联信息
        //         22.sCameraPath：相机文件路径
        //         23.sFileName：文件名称（不包含路径）
        //         24.iFileIndex：文件索引值
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _SendCommand_Value(CommunicationInstructionType command, VisionSystemClassLibrary.Enum.CameraType Cameratype, Int32 iDataIndex, Int32 iSelectedIPAddress, VisionSystemClassLibrary.Enum.CameraType SelectedCameratype, Byte byteSelectedPort, Int32 CameraChooseState, UInt64 uCameraFaultState, Boolean bCheckEnable, VisionSystemClassLibrary.Enum.CameraRotateAngle bCameraAngle, VisionSystemClassLibrary.Enum.VideoColor vVideoColor, VisionSystemClassLibrary.Enum.VideoResolution vVideoResolution, Boolean bSerialPort, VisionSystemClassLibrary.Enum.TobaccoSortType tTobaccoSortType, Boolean bBitmapResize, Boolean bBitmapCenter, Point bBitmapAxis, Rectangle rBitmapArea, VisionSystemClassLibrary.Enum.CameraFlip bCameraFlip, VisionSystemClassLibrary.Enum.SensorProductType sSensorProductType, VisionSystemClassLibrary.Struct.RelevancyCameraInformation relevancyCameraInformation, String sCameraPath, String sFileName, Int32 iFileIndex)
        {
            try
            {
                switch (command)//指令
                {
                    case CommunicationInstructionType.DevicesSetup_ConfigDevice:

                        //DEVICES SETUP页面操作，CONFIG DEVICE，格式：

                        Byte[] DevicesSetupConfigDevice_Data = null;//生成的指令数据

                        Byte[] DevicesSetupConfigDevice_ClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iDataIndex].DeviceInformation.IP).GetAddressBytes();//获取当前选中的相机的IP地址

                        if ("" == sCameraPath)//不发送文件
                        {
                            //完成文件发送
                            //服务端->客户端：指令类型 + 相机类型数据 + 设置为的IP地址 + 设置为的相机类型数据 + 设置为的端口 + 设置为的模式 + 设置为的相机故障标记 + 设置为的相机旋转角度 + 相机颜色 + 相机分辨率 + 是否为串口 + 烟支排列类型 + 设置为的相机数据截取区域缩放 + 设置为的相机数据截取区域缩放后是否居中 + 设置为的相机数据截取区域粘贴区域 + 设置为的相机数据截取区域 + 镜像标记 + 传感器应用场景 + 相机关联信息（关联类型 + 关联数量（相机类型 + 工位索引））

                            DevicesSetupConfigDevice_Data = _GenerateInstruction(command, Cameratype, iSelectedIPAddress, SelectedCameratype, byteSelectedPort, CameraChooseState, uCameraFaultState, bCheckEnable, bCameraAngle, vVideoColor, vVideoResolution, bSerialPort, tTobaccoSortType, bBitmapResize, bBitmapCenter, bBitmapAxis, rBitmapArea, bCameraFlip, sSensorProductType, relevancyCameraInformation);//生成指令数据

                            //

                            Global.NetServer.ClientData[DevicesSetupConfigDevice_ClientIP[3]]._Send(DevicesSetupConfigDevice_Data);//发送指令
                        }
                        else//发送文件
                        {
                            String sSafeFileName_Send = sFileName;//文件名和扩展名，文件名不包含路径
                            String sFileName_Send = sCameraPath + sFileName;//文件名和扩展名，文件名包含路径

                            //未完成文件发送
                            //服务端->客户端（文件）：指令类型 + 相机类型数据 + 设置为的IP地址 + 设置为的相机类型数据 + 设置为的端口 + 设置为的模式 + 设置为的相机故障标记 + 设置为的相机检测使能 + 设置为的相机旋转角度 + 相机颜色 + 相机分辨率 + 是否为串口 + 烟支排列类型 + 设置为的相机数据截取区域缩放 + 设置为的相机数据截取区域缩放后是否居中 + 设置为的相机数据截取区域粘贴区域 + 设置为的相机数据截取区域 + 镜像标记 + 传感器应用场景 + 相机关联信息（关联类型 + 关联数量（相机类型 + 工位索引）） + 文件索引值（从0开始） + 文件

                            DevicesSetupConfigDevice_Data = _GenerateInstruction(command, Cameratype, iSelectedIPAddress, SelectedCameratype, byteSelectedPort, CameraChooseState, uCameraFaultState, bCheckEnable, bCameraAngle, vVideoColor, vVideoResolution, bSerialPort, tTobaccoSortType, bBitmapResize, bBitmapCenter, bBitmapAxis, rBitmapArea, bCameraFlip, sSensorProductType, relevancyCameraInformation, iFileIndex);//生成指令数据

                            //

                            Global.NetServer.ClientData[DevicesSetupConfigDevice_ClientIP[3]]._Send(sSafeFileName_Send, sFileName_Send, DevicesSetupConfigDevice_Data);//发送文件
                        }
                        //
                        break;
                        //
                    default:
                        break;
                }
            }
            catch (System.Exception ex)
            {
                //不执行操作
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：向下位机发送
        // 输入参数：1.command：指令类型
        //         2.Cameratype：相机类型
        //         3.CameraChooseState：设置相机模式：单/双相机模式
        //         4.iDataIndex：数据索引值（相机数据索引值）
        //         5.sCameraPath：相机数据路径
        //         6.sCameraName：相机名称
        //         7.sFileName：文件名称
        //         8.sBrandName：品牌名称
        //         9.iFileIndex：文件索引值（从0开始）
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _SendCommand_Value(CommunicationInstructionType command, VisionSystemClassLibrary.Enum.CameraType Cameratype, Int32 CameraChooseState, Int32 iDataIndex, String sCameraPath, String sCameraName, String sFileName, String sBrandName, Int32 iFileIndex)
        {
            try
            {
                switch (command)//指令
                {
                    case CommunicationInstructionType.BrandManagement_LoadBrand:

                        //BRAND MANAGEMENT页面操作，载入品牌，格式：

                        Byte[] BrandManagementLoadBrand_Data = null;//生成的指令数据

                        Byte[] BrandManagementLoadBrand_ClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iDataIndex].DeviceInformation.IP).GetAddressBytes();//获取当前选中的相机的IP地址

                        if ("" == sCameraPath)//不发送文件
                        {
                            //完成文件发送
                            //服务端->客户端：指令类型 + 相机类型数据 + 设置相机模式 + 品牌名称长度 + 品牌名称

                            BrandManagementLoadBrand_Data = _GenerateInstruction(command, Cameratype, CameraChooseState, sBrandName);//生成指令数据

                            //

                            Global.NetServer.ClientData[BrandManagementLoadBrand_ClientIP[3]]._Send(BrandManagementLoadBrand_Data);//发送指令
                        }
                        else//发送文件
                        {
                            String sSafeFileName_Send = sFileName;//文件名和扩展名，文件名不包含路径
                            String sFileName_Send = sCameraPath + sFileName;//文件名和扩展名，文件名包含路径

                            //未完成文件发送
                            //服务端->客户端（文件）：指令类型 + 相机类型数据 + 设置相机模式 + 品牌名称长度 + 品牌名称 + 文件索引值（从0开始） + 文件

                            BrandManagementLoadBrand_Data = _GenerateInstruction(command, Cameratype, CameraChooseState, sBrandName, iFileIndex);//生成指令数据

                            //

                            Global.NetServer.ClientData[BrandManagementLoadBrand_ClientIP[3]]._Send(sSafeFileName_Send, sFileName_Send, BrandManagementLoadBrand_Data);//发送文件
                        }
                        //
                        break;
                        //
                    default:
                        break;
                }
            }
            catch (System.Exception ex)
            {
                //不执行操作
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：向下位机发送
        // 输入参数：1.command：指令类型
        //         2.Cameratype：相机类型
        //         3.TobaccoSort_E：烟支排列类型
        //         4.CameraChooseState：设置相机模式：单/双相机模式
        //         5.iDataIndex：数据索引值（相机数据索引值）
        //         6.iCameraSelected：相机选择数据
        //         7.iCheckEnable：相机检测使能
        //         8.sShiftPath：班次数据路径
        //         9.sFileName：文件名称
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _SendCommand_Value(CommunicationInstructionType command, VisionSystemClassLibrary.Enum.CameraType Cameratype, Int32 TobaccoSort_E, Int32 CameraChooseState, Int32 iDataIndex, Int32 iCameraSelected, Int32 iCheckEnable, String sShiftPath, String sFileName)
        {
            try
            {
                switch (command)//指令
                {
                    case CommunicationInstructionType.SystemParameter:

                        //系统参数设置（相机选择、班次），格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 设置相机模式 + 相机选择（1，启用；0，禁用） + 相机检测使能（1，启用；0，禁用） + 烟支排列类型 + 机器类型 + 班次数据（文件）

                        Byte[] SystemParameter_Data = null;//生成的指令数据

                        Byte[] SystemParameter_ClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iDataIndex].DeviceInformation.IP).GetAddressBytes();//获取当前选中的相机的IP地址

                        //

                        String sSafeFileName_Send = sFileName;//文件名和扩展名，文件名不包含路径
                        String sFileName_Send = sShiftPath + sFileName;//文件名和扩展名，文件名包含路径

                        SystemParameter_Data = _GenerateInstruction(command, Cameratype, CameraChooseState, iCameraSelected, iCheckEnable, TobaccoSort_E, Global.VisionSystem.SelectedMachineType);//生成指令数据

                        //

                        Global.NetServer.ClientData[SystemParameter_ClientIP[3]]._Send(sSafeFileName_Send, sFileName_Send, SystemParameter_Data);//发送文件
                        //
                        break;
                    //
                    default:
                        break;
                }
            }
            catch (System.Exception ex)
            {
                //不执行操作
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：向下位机发送
        // 输入参数：1.command：指令类型
        //         2.Cameratype：相机类型
        //         3.CameraChooseState：设置相机模式：单/双相机模式
        //         4.iDataIndex：数据索引值（相机数据索引值）
        //         5.sPath：文件路径
        //         6.sFileName：文件名称
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _SendCommand_Value(CommunicationInstructionType command, VisionSystemClassLibrary.Enum.CameraType Cameratype, Int32 CameraChooseState, Int32 iDataIndex, String sPath, String sFileName)
        {
            try
            {
                switch (command)//指令
                {
                    case CommunicationInstructionType.ClientSystem_Update:

                        //客户端系统升级，格式：
                        //服务端->客户端：指令类型 + 相机类型数据 + 设置相机模式 + 文件

                        Byte[] ClientSystemUpdate_Data = null;//生成的指令数据

                        Byte[] ClientSystemUpdate_ClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iDataIndex].DeviceInformation.IP).GetAddressBytes();//获取当前选中的相机的IP地址

                        //

                        String sSafeFileName_Send = sFileName;//文件名和扩展名，文件名不包含路径
                        String sFileName_Send = sPath + sFileName;//文件名和扩展名，文件名包含路径

                        ClientSystemUpdate_Data = _GenerateInstruction(command, Cameratype, CameraChooseState);//生成指令数据

                        //

                        Global.NetServer.ClientData[ClientSystemUpdate_ClientIP[3]]._Send(sSafeFileName_Send, sFileName_Send, ClientSystemUpdate_Data);//发送文件
                        //
                        break;
                        //
                    default:
                        break;
                }
            }
            catch (System.Exception ex)
            {
                //不执行操作
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：向下位机发送
        // 输入参数：1.command：指令类型
        //         2.Cameratype：相机类型
        //         3.iDataIndex：数据索引值（相机数据索引值）
        //         4.bRelevancy： 关联
        //         5.iValue_1：数值
        //         6.iValue_2：数值
        //         7.shifttime：数值
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _SendCommand_Value(CommunicationInstructionType command, VisionSystemClassLibrary.Enum.CameraType Cameratype, Int32 iDataIndex, Boolean bRelevancy, Int32 iValue_1, Int32 iValue_2, VisionSystemClassLibrary.Struct.ShiftTime shifttime)
        {
            try
            {
                switch (command)//指令
                {
                    case CommunicationInstructionType.QualityCheck_GetSelectedRecordData:

                        //QualityCheck页面，获取当前选择的统计数据
                        //服务端->客户端：指令类型 + 相机类型数据 + 关联标记 + 统计数据类型（0，最新统计数据；1，指定统计数据）+ 班次索引（非0） + 统计数据开始结束时间

                        Byte[] QualityCheck_GetSelectedRecordData_Data = _GenerateInstruction(command, Cameratype, bRelevancy, iValue_1, iValue_2, shifttime);//生成指令数据

                        Byte[] QualityCheck_GetSelectedRecordData_ClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iDataIndex].DeviceInformation.IP).GetAddressBytes();//获取当前选中的相机的IP地址

                        Global.NetServer.ClientData[QualityCheck_GetSelectedRecordData_ClientIP[3]]._Send(QualityCheck_GetSelectedRecordData_Data);//发送数据
                        //
                        break;
                    //
                    case CommunicationInstructionType.Statistics_GetSelectedRecordData:

                        //STATISTICS页面，获取当前选择的统计数据，格式：    
                        //服务端->客户端：指令类型 + 相机类型数据 + 关联标记 + 统计数据类型（0，最新统计数据；1，指定统计数据） + 班次索引（非0） + 统计数据开始结束时间

                        Byte[] StatisticsGetSelectedRecordData_Data = _GenerateInstruction(command, Cameratype,  Global.StatisticsViewWindow.CustomControl.Relevancy , iValue_1, iValue_2, shifttime);//生成指令数据

                        Byte[] StatisticsGetSelectedRecordData_ClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iDataIndex].DeviceInformation.IP).GetAddressBytes();//获取当前选中的相机的IP地址

                        Global.NetServer.ClientData[StatisticsGetSelectedRecordData_ClientIP[3]]._Send(StatisticsGetSelectedRecordData_Data);//发送数据
                        //
                        break;
                        //
                    default:
                        break;
                }
            }
            catch (System.Exception ex)
            {
                //不执行操作
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：向下位机发送
        // 输入参数：1.command：指令类型
        //         2.Cameratype：相机类型
        //         3.iDataIndex：数据索引值（相机数据索引值）
        //         4.iValue_1：数值
        //         5.bRelevancy：相机关联标记
        //         5.shifttime：数值
        //         6.iValue_2：数值
        //         7.iValue_3：数值
        //         8.dValue：数值
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _SendCommand_Value(CommunicationInstructionType command, VisionSystemClassLibrary.Enum.CameraType Cameratype, Int32 iDataIndex, Boolean bRelevancy, Int32 iValue_1, VisionSystemClassLibrary.Struct.ShiftTime shifttime, Int32 iValue_2, Int32 iValue_3, Double dValue)
        {
            try
            {
                switch (command)//指令
                {
                    case CommunicationInstructionType.QualityCheck_LoadReject_Click:

                        //QualityCheck页面，查看剔除图像，格式：    
                        //服务端->客户端：指令类型 + 相机类型数据 + 关联标记 + 班次索引（非0） + 统计数据开始结束时间 + 剔除图像对应的工具索引值（从0开始） + 剔除图像对应的工具中的索引值（从0开始） + 图像尺寸类型数据

                        Byte[] QualityCheck_LoadReject_Click_Data = _GenerateInstruction(command, Cameratype, bRelevancy, iValue_1, shifttime, iValue_2, iValue_3, dValue);//生成指令数据

                        Byte[] QualityCheck_LoadReject_Click_ClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iDataIndex].DeviceInformation.IP).GetAddressBytes();//获取当前选中的相机的IP地址

                        Global.NetServer.ClientData[QualityCheck_LoadReject_Click_ClientIP[3]]._Send(QualityCheck_LoadReject_Click_Data);//发送数据
                        //
                        break;
                    //
                    case CommunicationInstructionType.Statistics_UpdateSelectedRecordData:

                        //STATISTICS页面，更新当前选择的统计数据，格式：    
                        //服务端->客户端：指令类型 + 相机类型数据 + 关联标记 + 班次索引（非0） + 统计数据开始结束时间 + 剔除图像对应的工具索引值（从0开始） + 剔除图像对应的工具中的索引值（从0开始） + 图像尺寸类型数据

                        Byte[] StatisticsUpdateSelectedRecordData_Data = _GenerateInstruction(command, Cameratype, bRelevancy, iValue_1, shifttime, iValue_2, iValue_3, dValue);//生成指令数据

                        Byte[] StatisticsUpdateSelectedRecordData_ClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iDataIndex].DeviceInformation.IP).GetAddressBytes();//获取当前选中的相机的IP地址

                        Global.NetServer.ClientData[StatisticsUpdateSelectedRecordData_ClientIP[3]]._Send(StatisticsUpdateSelectedRecordData_Data);//发送数据
                        //
                        break;
                        //
                    case CommunicationInstructionType.Statistics_ClickRejectsListItem:

                        //STATISTICS页面，查看剔除图像，格式：    
                        //服务端->客户端：指令类型 + 相机类型数据 + 关联标记 +  班次索引（非0） + 统计数据开始结束时间 + 剔除图像对应的工具索引值（从0开始） + 剔除图像对应的工具中的索引值（从0开始） + 图像尺寸类型数据
                        
                        Byte[] StatisticsClickRejectsListItem_Data = _GenerateInstruction(command, Cameratype, bRelevancy, iValue_1, shifttime, iValue_2, iValue_3, dValue);//生成指令数据

                        Byte[] StatisticsClickRejectsListItem_ClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iDataIndex].DeviceInformation.IP).GetAddressBytes();//获取当前选中的相机的IP地址

                        Global.NetServer.ClientData[StatisticsClickRejectsListItem_ClientIP[3]]._Send(StatisticsClickRejectsListItem_Data);//发送数据
                        //
                        break;
                        //
                    default:
                        break;
                }
            }
            catch (System.Exception ex)
            {
                //不执行操作
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：向下位机发送
        // 输入参数：1.command：指令类型
        //         2.Cameratype：相机类型
        //         3.iDataIndex：数据索引值（相机数据索引值）
        //         4.iValue_1：数值
        //         5.iValue_2：数值
        //         6.iValue_Array_1：数值
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _SendCommand_Value(CommunicationInstructionType command, VisionSystemClassLibrary.Enum.CameraType Cameratype, Int32 iDataIndex, Int32 iValue_1, Int32 iValue_2, Int32[] iValue_Array_1)
        {
            try
            {
                switch (command)//指令
                {
                    case CommunicationInstructionType.Statistics_DeleteRecord:

                        //STATISTICS页面，删除统计数据，格式：    
                        //服务端->客户端：指令类型 + 相机类型数据 + 删除统计数据方式（0，删除所有；1，删除指定班次；2，删除指定记录）
                        //（1） + 待删除的指定班次个数 + 班次索引值数组（从0开始）

                        Byte[] StatisticsDeleteRecord_Data = _GenerateInstruction(command, Cameratype, iValue_1, iValue_2, iValue_Array_1);//生成指令数据

                        Byte[] StatisticsDeleteRecord_ClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iDataIndex].DeviceInformation.IP).GetAddressBytes();//获取当前选中的相机的IP地址

                        Global.NetServer.ClientData[StatisticsDeleteRecord_ClientIP[3]]._Send(StatisticsDeleteRecord_Data);//发送数据
                        //
                        break;
                        //
                    default:
                        break;
                }
            }
            catch (System.Exception ex)
            {
                //不执行操作
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：向下位机发送
        // 输入参数：1.command：指令类型
        //         2.Cameratype：相机类型
        //         3.iDataIndex：数据索引值（相机数据索引值）
        //         4.iValue_1：数值
        //         5.iValue_2：数值
        //         6.iValue_3：数值
        //         7.ShiftTime_Array_1：数值
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _SendCommand_Value(CommunicationInstructionType command, VisionSystemClassLibrary.Enum.CameraType Cameratype, Int32 iDataIndex, Int32 iValue_1, Int32 iValue_2, Int32 iValue_3, VisionSystemClassLibrary.Struct.ShiftTime[] ShiftTime_Array_1)
        {
            try
            {
                switch (command)//指令
                {
                    case CommunicationInstructionType.Statistics_DeleteRecord:

                        //STATISTICS页面，删除统计数据，格式：    
                        //服务端->客户端：指令类型 + 相机类型数据 + 删除统计数据方式（0，删除所有；1，删除指定班次；2，删除指定记录）
                        //（2） + 班次索引值（从0开始） + 待删除的指定记录个数 + 记录开始结束时间数组

                        Byte[] StatisticsDeleteRecord_Data = _GenerateInstruction(command, Cameratype, iValue_1, iValue_2, iValue_3, ShiftTime_Array_1);//生成指令数据

                        Byte[] StatisticsDeleteRecord_ClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iDataIndex].DeviceInformation.IP).GetAddressBytes();//获取当前选中的相机的IP地址

                        Global.NetServer.ClientData[StatisticsDeleteRecord_ClientIP[3]]._Send(StatisticsDeleteRecord_Data);//发送数据
                        //
                        break;
                        //
                    default:
                        break;
                }
            }
            catch (System.Exception ex)
            {
                //不执行操作
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：向下位机发送
        // 输入参数：1.command：指令类型
        //         2.Cameratype：相机类型
        //         3.iDataIndex：数据索引值（相机数据索引值）
        //         4.iValue_1：数值
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _SendCommand_Value(CommunicationInstructionType command, VisionSystemClassLibrary.Enum.CameraType Cameratype, Int32 iDataIndex, UInt64 iValue_1)
        {
            try
            {
                switch (command)//指令
                {
                    case CommunicationInstructionType.SetFaultMessageState:

                        //FAULT MESSAGE状态，格式：    
                        //服务端->客户端：指令类型 + 相机类型数据 + 故障信息使能状态

                        Byte[] SetFaultMessageState_Data = _GenerateInstruction(command, Cameratype, iValue_1);//生成指令数据

                        Byte[] SetFaultMessageState_ClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iDataIndex].DeviceInformation.IP).GetAddressBytes();//获取当前选中的相机的IP地址

                        Global.NetServer.ClientData[SetFaultMessageState_ClientIP[3]]._Send(SetFaultMessageState_Data);//发送数据
                        //
                        break;
                        //
                    default:
                        break;
                }
            }
            catch (System.Exception ex)
            {
                //不执行操作
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：向每个相机发送指令
        // 输入参数：1.command：指令
        //         2.Cameratype：相机类型
        // 输出参数：无
        // 返回值：发送的数据
        //----------------------------------------------------------------------
        public void _SendCommand_Cameras(CommunicationInstructionType command, VisionSystemClassLibrary.Enum.CameraType Cameratype)
        {
            Int32 i = 0;//循环控制变量

            Int32 iCameraIndex = _GetSelectedCameraIndex(Cameratype);//相机索引值

            try
            {
                switch (Global.CurrentInterface)
                {
                    case ApplicationInterface.Work://WORK页面

                        for (i = iCameraIndex; i < Global.VisionSystem.Camera.Count; i++)//遍历相机
                        {
                            if (VisionSystemClassLibrary.Enum.CameraState.NOTCONNECTED != Global.VisionSystem.Camera[i].DeviceInformation.CAM)//连接
                            {
                                _SendCommand_Image(command, Global.VisionSystem.Camera[i].Type, i, Global.ImageScale_Work);//发送指令
                            }
                        }
                        if (i >= Global.VisionSystem.Camera.Count && 0 != Global.VisionSystem.Camera.Count)//继续遍历
                        {
                            for (i = 0; i < iCameraIndex; i++)//遍历相机
                            {
                                if (VisionSystemClassLibrary.Enum.CameraState.NOTCONNECTED != Global.VisionSystem.Camera[i].DeviceInformation.CAM)//连接
                                {
                                    _SendCommand_Image(command, Global.VisionSystem.Camera[i].Type, i, Global.ImageScale_Work);//发送指令
                                }
                            }
                        }
                        break;
                        //
                    case ApplicationInterface.Load://LOAD页面
                        //
                        break;
                        //
                    case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                        //
                        break;
                        //
                    case ApplicationInterface.LiveView://LIVE VIEW页面
                        //
                        break;
                        //
                    case ApplicationInterface.QualityCheck://QUALITY CHECK页面
                        //
                        break;
                        //
                    case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面
                        //
                        break;
                        //
                    case ApplicationInterface.DevicesSetup://DEVICES SETUP页面
                        //
                        break;
                        //
                    case ApplicationInterface.SystemConfiguration://SYSTEM页面
                        //
                        break;
                        //
                    default:
                        break;
                }
            }
            catch (System.Exception ex)
            {
                //不执行操作
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：向每个相机发送指令
        // 输入参数：1.command：指令
        // 输出参数：无
        // 返回值：发送的数据
        //----------------------------------------------------------------------
        public void _SendCommand_Cameras(CommunicationInstructionType command)
        {
            Int32 i = 0;//循环控制变量
            Int32 iValue = 0;//临时变量

            try
            {
                if (CommunicationInstructionType.CurrentFaultMessage == command)//FAULT MESSAGE，获取当前故障信息
                {
                    for (i = 0; i < Global.VisionSystem.Camera.Count; i++)//遍历相机
                    {
                        if (VisionSystemClassLibrary.Enum.CameraState.NOTCONNECTED != Global.VisionSystem.Camera[i].DeviceInformation.CAM)//连接
                        {
                            _SendCommand_Value(command, Global.VisionSystem.Camera[i].Type, i);//发送指令
                        }
                    }
                }
                else if (CommunicationInstructionType.SetFaultMessageState == command)//FAULT MESSAGE设置
                {
                    for (i = 0; i < Global.VisionSystem.Camera.Count; i++)//遍历相机
                    {
                        if (VisionSystemClassLibrary.Enum.CameraState.NOTCONNECTED != Global.VisionSystem.Camera[i].DeviceInformation.CAM)//连接
                        {
                            _SendCommand_Value(command, Global.VisionSystem.Camera[i].Type, i, VisionSystemClassLibrary.Class.System.MachineFaultEnableState);//发送指令
                        }
                    }

                }
                else//其它
                {
                    for (i = 0; i < Global.VisionSystem.Camera.Count; i++)//遍历相机
                    {
                        if (VisionSystemClassLibrary.Enum.CameraState.NOTCONNECTED != Global.VisionSystem.Camera[i].DeviceInformation.CAM)//连接
                        {
                            iValue++;
                        }
                    }

                    //

                    VisionSystemControlLibrary.GlobalWindows.FaultMessage_Window.FaultMessageControl.DeviceNumber = iValue;

                    for (i = 0; i < Global.VisionSystem.Camera.Count; i++)//遍历相机
                    {
                        if (VisionSystemClassLibrary.Enum.CameraState.NOTCONNECTED != Global.VisionSystem.Camera[i].DeviceInformation.CAM)//连接
                        {
                            _SendCommand_Value(command, Global.VisionSystem.Camera[i].Type, i);//发送指令
                        }
                    }

                    //

                    switch (Global.CurrentInterface)
                    {
                        case ApplicationInterface.Work://WORK页面
                            //
                            break;
                            //
                        case ApplicationInterface.Load://LOAD页面
                            //
                            break;
                            //
                        case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                            //
                            break;
                            //
                        case ApplicationInterface.LiveView://LIVE VIEW页面
                            //
                            break;
                            //
                        case ApplicationInterface.QualityCheck://QUALITY CHECK页面
                            //
                            break;
                            //
                        case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面
                            //
                            break;
                            //
                        case ApplicationInterface.DevicesSetup://DEVICES SETUP页面
                            //
                            break;
                            //
                        case ApplicationInterface.SystemConfiguration://SYSTEM页面
                            //
                            break;
                            //
                        default:
                            break;
                    }
                }
            }
            catch (System.Exception ex)
            {
                //不执行操作
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：请求获取客户端设备信息
        // 输入参数：1.client：客户端
        // 输出参数：无
        // 返回值：发送的数据
        //----------------------------------------------------------------------
        private void _UpdateTitleBar()
        {
            Int32 i = 0;//循环控制变量
            Boolean bUpdate = false;//是否更新按钮。取值范围：true，是；false，否

            for (i = 0; i < Global.VisionSystem.Camera.Count; i++)//遍历相机
            {
                if (VisionSystemClassLibrary.Enum.CameraState.NOTCONNECTED == Global.VisionSystem.Camera[i].DeviceInformation.CAM)//未连接
                {
                    //不执行操作
                }
                else
                {
                    break;
                }
            }
            if (i < Global.VisionSystem.Camera.Count)//存在未更新、关闭或打开的相机
            {
                if (!(TitleBar.StateShow))//【STATE】按钮未显示
                {
                    //显示【STATE】按钮

                    bUpdate = true;

                    //

                    TitleBar.Invoke(new EventHandler(delegate { TitleBar.StateShow = true; }));//WORK页面
                }
            }
            else//所有相机均未连接
            {
                if (TitleBar.StateShow)//【STATE】按钮显示
                {
                    //隐藏【STATE】按钮

                    bUpdate = true;

                    //

                    TitleBar.Invoke(new EventHandler(delegate { TitleBar.StateShow = false; }));//WORK页面
                }
            }

            //

            if (bUpdate)//更新按钮
            {
                Global.SystemConfigurationWindow.TitleBar.Invoke(new EventHandler(delegate { Global.SystemConfigurationWindow.TitleBar.StateShow = TitleBar.StateShow; }));//SYSTEM页面
                Global.DevicesSetupWindow.TitleBar.Invoke(new EventHandler(delegate { Global.DevicesSetupWindow.TitleBar.StateShow = TitleBar.StateShow; }));//DEVICES SETUP页面                        
                Global.BrandManagementWindow.TitleBar.Invoke(new EventHandler(delegate { Global.BrandManagementWindow.TitleBar.StateShow = TitleBar.StateShow; }));//BRAND MANAGEMENT页面                        
                Global.BackupBrandsWindow.TitleBar.Invoke(new EventHandler(delegate { Global.BackupBrandsWindow.TitleBar.StateShow = TitleBar.StateShow; }));//BACKUP BRANDS页面                        
                Global.RestoreBrandsWindow.TitleBar.Invoke(new EventHandler(delegate { Global.RestoreBrandsWindow.TitleBar.StateShow = TitleBar.StateShow; }));//RESTORE BRANDS页面                        
                Global.TolerancesSettingsWindow.TitleBar.Invoke(new EventHandler(delegate { Global.TolerancesSettingsWindow.TitleBar.StateShow = TitleBar.StateShow; }));//TOLERANCES SETTINGS页面                        
                Global.LiveViewWindow.TitleBar.Invoke(new EventHandler(delegate { Global.LiveViewWindow.TitleBar.StateShow = TitleBar.StateShow; }));//LIVE VIEW页面
                Global.QualityCheckWindow.TitleBar.Invoke(new EventHandler(delegate { Global.QualityCheckWindow.TitleBar.StateShow = TitleBar.StateShow; }));//QUALITY CHECK页面
                Global.ImageConfigurationWindow.TitleBar.Invoke(new EventHandler(delegate { Global.ImageConfigurationWindow.TitleBar.StateShow = TitleBar.StateShow; }));//IMAGE CONFIGURATION页面
                Global.StatisticsViewWindow.TitleBar.Invoke(new EventHandler(delegate { Global.StatisticsViewWindow.TitleBar.StateShow = TitleBar.StateShow; }));//STATISTICS VIEW页面
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：请求获取客户端设备信息
        // 输入参数：1.client：客户端
        // 输出参数：无
        // 返回值：发送的数据
        //----------------------------------------------------------------------
        public void _RequestClientDeviceInformation(VisionSystemCommunicationLibrary.Ethernet.CLIENTSOCKET client)
        {
            byte[] byteInstruction = VisionSystemCommunicationLibrary.Ethernet.TestControlFunctions._StringToByte(VisionSystemCommunicationLibrary.Ethernet.TestControlFunctions.sDeviceInformationFlag);//待发送的指令数据

            client._Send(byteInstruction);//发送数据
        }

        //----------------------------------------------------------------------
        // 功能说明：检测校验和数据
        // 输入参数：1.Cameratype：相机类型
        // 输出参数：无
        // 返回值：校验和是否相同。取值范围：true，是；false，否
        //----------------------------------------------------------------------
        private bool _GetCheckSum(VisionSystemClassLibrary.Enum.CameraType Cameratype)
        {
            bool bReturn = true;//函数返回值

            Int32 iCameraIndex = _GetSelectedCameraIndex(Cameratype);//相机数据索引

            //

            Int32 i = 0;//循环控制变量

            DirectoryInfo directoryinfo = new DirectoryInfo(Global.VisionSystem.Camera[iCameraIndex].SampleImagePath.Substring(0, Global.VisionSystem.Camera[iCameraIndex].SampleImagePath.Length - 1));//路径信息

            String sLocalFilePath_Tolerances = Global.VisionSystem.ConfigDataPath + Global.VisionSystem.Camera[iCameraIndex].CameraENGName + "\\" + VisionSystemClassLibrary.Class.Camera.TolerancesFileName;//本机，Tolerances.dat文件完整路径
            String sLocalFilePath_Tool = Global.VisionSystem.ConfigDataPath + Global.VisionSystem.Camera[iCameraIndex].CameraENGName + "\\" + VisionSystemClassLibrary.Class.Camera.ToolFileName;//本机，Tool.dat文件完整路径
            String sLocalFilePath_Parameter = Global.VisionSystem.ConfigDataPath + Global.VisionSystem.Camera[iCameraIndex].CameraENGName + "\\" + VisionSystemClassLibrary.Class.Camera.ParameterFileName;//本机，Parameter.dat文件完整路径
            String sLocalFilePath_Classes = Global.VisionSystem.ConfigDataPath + Global.VisionSystem.Camera[iCameraIndex].CameraENGName + "\\" + VisionSystemClassLibrary.Class.Camera.ClassesFile;//本机，Model.xml文件完整路径
            String sLocalFilePath_Model = Global.VisionSystem.ConfigDataPath + Global.VisionSystem.Camera[iCameraIndex].CameraENGName + "\\" + VisionSystemClassLibrary.Class.Camera.ModelFileName;//本机，Model.bp文件完整路径
            String sLocalFilePath_SampleImage = Global.VisionSystem.ConfigDataPath + Global.VisionSystem.Camera[iCameraIndex].CameraENGName + "\\" + directoryinfo.Name + "\\" + VisionSystemClassLibrary.Class.Camera.SampleImageFileName + VisionSystemClassLibrary.Class.Camera.BMPFile;//本机，Sample.BMP文件完整路径

            String sRemoteFilePath_Tolerances = Global.VisionSystem.ConfigDataPath + Global.VisionSystem.Camera[iCameraIndex].CameraENGName + "\\" + VisionSystemClassLibrary.Class.Camera.ReceivedDataPathName + VisionSystemClassLibrary.Class.Camera.TolerancesFileName;//客户端，Tolerances.dat文件完整路径
            String sRemoteFilePath_Tool = Global.VisionSystem.ConfigDataPath + Global.VisionSystem.Camera[iCameraIndex].CameraENGName + "\\" + VisionSystemClassLibrary.Class.Camera.ReceivedDataPathName + VisionSystemClassLibrary.Class.Camera.ToolFileName;//客户端，Tool.dat文件完整路径
            String sRemoteFilePath_Parameter = Global.VisionSystem.ConfigDataPath + Global.VisionSystem.Camera[iCameraIndex].CameraENGName + "\\" + VisionSystemClassLibrary.Class.Camera.ReceivedDataPathName + VisionSystemClassLibrary.Class.Camera.ParameterFileName;//客户端，Parameter.dat文件完整路径
            String sRemoteFilePath_ModelXML = Global.VisionSystem.ConfigDataPath + Global.VisionSystem.Camera[iCameraIndex].CameraENGName + "\\" + VisionSystemClassLibrary.Class.Camera.ReceivedDataPathName + VisionSystemClassLibrary.Class.Camera.ClassesFile;//客户端，Model.xml文件完整路径
            String sRemoteFilePath_ModelBP = Global.VisionSystem.ConfigDataPath + Global.VisionSystem.Camera[iCameraIndex].CameraENGName + "\\" + VisionSystemClassLibrary.Class.Camera.ReceivedDataPathName + VisionSystemClassLibrary.Class.Camera.ModelFileName;//客户端，Model.bp文件完整路径
            String sRemoteFilePath_SampleImage = Global.VisionSystem.ConfigDataPath + Global.VisionSystem.Camera[iCameraIndex].CameraENGName + "\\" + VisionSystemClassLibrary.Class.Camera.ReceivedDataPathName + VisionSystemClassLibrary.Class.Camera.SampleImageFileName + VisionSystemClassLibrary.Class.Camera.BMPFile;//客户端，Sample.BMP文件完整路径

            String sLocalFilePath = "";//本机，文件完整路径
            String sRemoteFilePath = "";//客户端，文件完整路径

            Int32 iRecicleCount = Global.VisionSystem.Camera[iCameraIndex].DeepLearningState ? 6 : 4;
            for (i = 0; i < iRecicleCount; i++)//遍历
            {
                if (0 == i)//Tolerances.dat文件
                {
                    sLocalFilePath = sLocalFilePath_Tolerances;//本机，文件完整路径
                    sRemoteFilePath = sRemoteFilePath_Tolerances;//客户端，文件完整路径
                }
                else if (1 == i)//Tool.dat文件
                {
                    sLocalFilePath = sLocalFilePath_Tool;//本机，文件完整路径
                    sRemoteFilePath = sRemoteFilePath_Tool;//客户端，文件完整路径
                }
                else if (2 == i)//Parameter.dat文件
                {
                    sLocalFilePath = sLocalFilePath_Parameter;//本机，文件完整路径
                    sRemoteFilePath = sRemoteFilePath_Parameter;//客户端，文件完整路径
                }
                else if (3 == i)//Sample.BMP文件
                {
                    sLocalFilePath = sLocalFilePath_SampleImage;//本机，文件完整路径
                    sRemoteFilePath = sRemoteFilePath_SampleImage;//客户端，文件完整路径
                }
                else if (4 == i)//Model.xml文件
                {
                    sLocalFilePath = sLocalFilePath_Classes;//本机，文件完整路径
                    sRemoteFilePath = sRemoteFilePath_ModelXML;//客户端，文件完整路径
                }
                else if (5 == i)//Model.bp文件
                {
                    sLocalFilePath = sLocalFilePath_Model;//本机，文件完整路径
                    sRemoteFilePath = sRemoteFilePath_ModelBP;//客户端，文件完整路径
                }
                else//其它
                {
                    //不执行操作
                }

                //

                try
                {
                    if (File.Exists(sLocalFilePath) && File.Exists(sRemoteFilePath))//都存在
                    {
                        System.Security.Cryptography.HashAlgorithm hash = System.Security.Cryptography.HashAlgorithm.Create();//创建

                        FileStream filestreamLocalFile = new FileStream(sLocalFilePath, FileMode.Open);//打开文件
                        Byte[] byteHashLocalFile = hash.ComputeHash(filestreamLocalFile);//获取哈希值
                        filestreamLocalFile.Close();//关闭

                        FileStream filestreamRemoteFile = new FileStream(sRemoteFilePath, FileMode.Open);//打开文件
                        Byte[] byteHashRemoteFile = hash.ComputeHash(filestreamRemoteFile);//获取哈希值
                        filestreamRemoteFile.Close();//关闭

                        String sHashLocalFile = BitConverter.ToString(byteHashLocalFile);
                        String sHashRemoteFile = BitConverter.ToString(byteHashRemoteFile);

                        if (sHashLocalFile != sHashRemoteFile)//不相同
                        {
                            bReturn = false;

                            break;
                        }
                    }
                    else if (!File.Exists(sLocalFilePath) && !File.Exists(sRemoteFilePath))//都不存在
                    {
                        //不执行操作
                    }
                    else//其它
                    {
                        bReturn = false;

                        break;
                    }
                }
                catch (Exception ex)
                {
                    bReturn = false;

                    break;
                }
            }

            //

            if (!bReturn)//校验和不相同
            {
                Global.VisionSystem.Camera[iCameraIndex].DeviceInformation.CAM = VisionSystemClassLibrary.Enum.CameraState.CONNECTED | VisionSystemClassLibrary.Enum.CameraState.NOTUPDATED;//更新数值
            }

            if (false == Global.VisionSystem.Camera[iCameraIndex].CheckEnable)//剔除关闭
            {
                Global.VisionSystem.Camera[iCameraIndex].DeviceInformation.CAM = Global.VisionSystem.Camera[iCameraIndex].DeviceInformation.CAM | VisionSystemClassLibrary.Enum.CameraState.REJECTOFF;
            }

            for (i = 0; i < Global.VisionSystem.ConnectionData.Length; i++)//遍历设备
            {
                if (Global.VisionSystem.ConnectionData[i].Type == Global.VisionSystem.Camera[iCameraIndex].Type)//有效
                {
                    Global.VisionSystem.ConnectionData[i].CAM = Global.VisionSystem.Camera[iCameraIndex].DeviceInformation.CAM;

                    break;
                }
            }

            //

            return bReturn;
        }

        //----------------------------------------------------------------------
        // 功能说明：获取客户端信息
        // 输入参数：1.ServerData：客户端发送的数据
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _GetDeviceInformation(VisionSystemCommunicationLibrary.Ethernet.DataReceivedEventArgs ServerData)
        {
            //获取客户端设备信息

            String sMACAddress = "";//通过以太网接收的
            String sFirmwareVersion = "";
            int iCameraNumber = 3;
            VisionSystemClassLibrary.Enum.CameraType[] Cameratype = new VisionSystemClassLibrary.Enum.CameraType[iCameraNumber];
            String[] sSerialNumber = new String[iCameraNumber];
            Byte[] bytePort = new Byte[iCameraNumber];

            _GetDeviceInformation(ServerData, ref sMACAddress, ref sFirmwareVersion, ref iCameraNumber, ref Cameratype, ref sSerialNumber, ref bytePort);//获取客户端设备信息
            ServerData.Client.MAC = sMACAddress;//初始化

            //初始化相机数据

            Int32 iValue = 0;//临时变量

            for (Int32 i = 0; i < iCameraNumber; i++)//遍历接收的相机信息（若客户端包含的相机信息已存在或未配置，则不处理客户端信息，但此时客户端连接是存在的
            {
                iValue = _CheckConnectedClient(Cameratype[i]);

                if (0 == iValue)//选中，不存在
                {
                    _SetCameraParameter(false, ServerData, sMACAddress, sFirmwareVersion, Cameratype[i], sSerialNumber[i], bytePort[i]);//初始化设备信息数据

                    Global.VisionSystem.Work.ConnectedCameraNumber++;//连接的相机数目

                    //

                    _SendCommand_Value(CommunicationInstructionType.Load, Cameratype[i], _GetSelectedCameraIndex(Cameratype[i])/*(Int32)Cameratype[i] - 1*/);//发送指令
                }
                else if (1 == iValue)//选中，存在
                {
                    _SetCameraParameter(true, ServerData, sMACAddress, sFirmwareVersion, Cameratype[i], sSerialNumber[i], bytePort[i]);//初始化设备信息数据

                    //

                    if (ApplicationInterface.DevicesSetup == Global.CurrentInterface)//DEVICES SETUP页面
                    {
                        Global.DevicesSetupWindow.CustomControl.Invoke(new EventHandler(delegate { Global.DevicesSetupWindow.CustomControl._RefreshList(); }));//操作完成
                    }
                }
                else//2，未选中
                {
                    //不执行操作
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：获取客户端信息
        // 输入参数：1.ServerData：客户端发送的数据
        //         2.sMACAddress：客户端MAC地址
        //         3.sFirmwareVersion：客户端固件版本
        //         4.iCameraNumber：客户端相机数量
        //         5.Cameratype：客户端相机类型
        //         6.sSerialNumber：客户端相机序列号
        //         7.bytePort：客户端端口
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _GetDeviceInformation(VisionSystemCommunicationLibrary.Ethernet.DataReceivedEventArgs ServerData, ref String sMACAddress, ref String sFirmwareVersion, ref int iCameraNumber, ref VisionSystemClassLibrary.Enum.CameraType[] Cameratype, ref String[] sSerialNumber, ref Byte[] bytePort)
        {
            Int32 iIndex = 0;//临时变量

            MemoryStream Memorystream = new MemoryStream();//流对象

            iIndex = ServerData.DataInfo.InstructionIndex + VisionSystemCommunicationLibrary.Ethernet.TestControlFunctions.sDeviceInformationFlag.Length;
            Int32 DeviceInformationLength = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//设备信息数据长度
            iIndex += BitConverter.GetBytes(DeviceInformationLength).Length;
            Memorystream.Write(ServerData.ReceivedData, iIndex, DeviceInformationLength);//写入流

            IFormatter formatter = new SoapFormatter();//格式化对象
            Memorystream.Position = 0;//初始化流对象
            VisionSystemCommunicationLibrary.Ethernet.SerializableData DeviceInformation = (VisionSystemCommunicationLibrary.Ethernet.SerializableData)formatter.Deserialize(Memorystream);//反序列化

            int i = 0;//循环控制变量

            sMACAddress = VisionSystemCommunicationLibrary.Ethernet.TestControlFunctions._ByteToString(DeviceInformation.Data_0, 0, DeviceInformation.Data_0.Length);//MAC地址

            sFirmwareVersion = VisionSystemCommunicationLibrary.Ethernet.TestControlFunctions._ByteToString(DeviceInformation.Data_1, 0, DeviceInformation.Data_1.Length);//固件版本

            iCameraNumber = DeviceInformation.Data_00.Length;//获取相机个数

            Cameratype = new VisionSystemClassLibrary.Enum.CameraType[iCameraNumber];//相机类型
            sSerialNumber = new String[iCameraNumber];//序列号
            bytePort = new Byte[iCameraNumber];//设备名称

            for (i = 0; i < iCameraNumber; i++)//获取每个相机的类型
            {
                Cameratype[i] = (VisionSystemClassLibrary.Enum.CameraType)DeviceInformation.Data_00[i][0];
            }

            for (i = 0; i < iCameraNumber; i++)//获取每个相机的序列号
            {
                sSerialNumber[i] = VisionSystemCommunicationLibrary.Ethernet.TestControlFunctions._ByteToString(DeviceInformation.Data_01[i], 0, DeviceInformation.Data_01[i].Length);//序列号
            }

            for (i = 0; i < iCameraNumber; i++)//获取每个相机的端口
            {
                bytePort[i] = DeviceInformation.Data_2[i];
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：获取客户端信息后，检查客户端包含的相机状态
        // 输入参数：1.Cameratype：连接的相机类型
        // 输出参数：无
        // 返回值：客户端包含的相机状态。取值范围：0，选中，不存在；1，选中，存在；2，未选中
        //----------------------------------------------------------------------
        private Int32 _CheckConnectedClient(VisionSystemClassLibrary.Enum.CameraType Cameratype)
        {
            Int32 iReturn = 0;//函数返回值
            int i = 0;//循环控制变量

            //遍历已接纳相机

            for (i = 0; i < Global.VisionSystem.Camera.Count; i++)//遍历已接纳相机
            {
                if (Cameratype == Global.VisionSystem.Camera[i].Type)//目标类型
                {
                    if (Global.VisionSystem.Camera[i].DeviceInformation.Connected && Global.VisionSystem.Camera[i].DeviceInformation.GetDevInfo)//已存在
                    {
                        break;
                    }
                }
            }

            if (i < Global.VisionSystem.Camera.Count)//选中，存在
            {
                iReturn = 1;
            }
            else//其它
            {
                //遍历可配置相机

                for (i = 0; i < Global.VisionSystem.SystemCameraConfiguration.Length; i++)//遍历可配置相机
                {
                    if (Cameratype == Global.VisionSystem.SystemCameraConfiguration[i].Type && Global.VisionSystem.SystemCameraConfiguration[i].Selected)//有效
                    {
                        break;
                    }
                }

                if (i < Global.VisionSystem.SystemCameraConfiguration.Length)//选中，不存在
                {
                    iReturn = 0;
                }
                else//未选中
                {
                    iReturn = 2;
                }
            }

            return iReturn;
        }

        //----------------------------------------------------------------------
        // 功能说明：配置相机参数
        // 输入参数：1.bExist：连接的相机是否存在。取值范围：true，是；false，否
        //         2.ServerData：客户端数据
        //         3.sMACAddress：客户端MAC地址
        //         4.sFirmwareVersion：客户端固件版本
        //         5.Cameratype：客户端相机类型
        //         6.sSerialNumber：客户端相机序列号
        //         7.bytePort：客户端端口
        // 输出参数：无
        // 返回值：分析完成的字符串
        //----------------------------------------------------------------------
        private void _SetCameraParameter(Boolean bExist, VisionSystemCommunicationLibrary.Ethernet.DataReceivedEventArgs ServerData, String sMACAddress, String sFirmwareVersion, VisionSystemClassLibrary.Enum.CameraType Cameratype, String sSerialNumber, Byte bytePort)
        {
            Byte i = 0;

            if (bExist)//连接的相机存在
            {
                for (i = 0; i < Global.VisionSystem.ConnectionData.Length; i++)//遍历设备
                {
                    if (Global.VisionSystem.ConnectionData[i].Type == Cameratype)//目标
                    {
                        Global.VisionSystem.ConnectionData[i].Connected = ServerData.Client.Connected;
                        Global.VisionSystem.ConnectionData[i].Firmware = sFirmwareVersion;
                        Global.VisionSystem.ConnectionData[i].GetDevInfo = ServerData.Client.GetDevInfo;
                        Global.VisionSystem.ConnectionData[i].IP = ServerData.Client.IP;
                        Global.VisionSystem.ConnectionData[i].MAC = ServerData.Client.MAC;

                        Global.VisionSystem.ConnectionData[i].Port = bytePort;
                        Global.VisionSystem.ConnectionData[i].Type = Cameratype;
                        Global.VisionSystem.ConnectionData[i].SerialNumber = sSerialNumber;

                        break;
                    }
                }

                Global.VisionSystem._SetLanguage_Device();//设置设备名称

                //

                if (null != Global.VisionSystem.Camera)
                {
                    for (i = 0; i < Global.VisionSystem.Camera.Count; i++)//遍历已接纳相机
                    {
                        if (Cameratype == Global.VisionSystem.Camera[i].Type)//目标相机
                        {
                            Global.VisionSystem.Camera[i].DeviceInformation.Connected = ServerData.Client.Connected;
                            Global.VisionSystem.Camera[i].DeviceInformation.Firmware = sFirmwareVersion;
                            Global.VisionSystem.Camera[i].DeviceInformation.GetDevInfo = ServerData.Client.GetDevInfo;
                            Global.VisionSystem.Camera[i].DeviceInformation.IP = ServerData.Client.IP;
                            Global.VisionSystem.Camera[i].DeviceInformation.MAC = ServerData.Client.MAC;

                            Global.VisionSystem.Camera[i].DeviceInformation.SerialNumber = sSerialNumber;
                            Global.VisionSystem.Camera[i].DeviceInformation.Port = bytePort;

                            for (Byte j = 0; j < Global.VisionSystem.Camera.Count; j++)//遍历已接纳相机
                            {
                                if (Global.VisionSystem.Camera[j].ControllerENGName == Global.VisionSystem.Camera[i].ControllerENGName) //当前属于同一控制器
                                {
                                    Global.VisionSystem.Camera[j].DeviceInformation.IP = Global.VisionSystem.Camera[i].DeviceInformation.IP;
                                }
                            }
                            break;
                        }
                    }
                }
            }
            else//连接的相机不存在
            {
                for (i = 0; i < Global.VisionSystem.ConnectionData.Length; i++)//遍历设备
                {
                    if (!(Global.VisionSystem.ConnectionData[i].Connected))//未连接
                    {
                        Global.VisionSystem.ConnectionData[i].Connected = ServerData.Client.Connected;
                        Global.VisionSystem.ConnectionData[i].Firmware = sFirmwareVersion;
                        Global.VisionSystem.ConnectionData[i].GetDevInfo = ServerData.Client.GetDevInfo;
                        Global.VisionSystem.ConnectionData[i].IP = ServerData.Client.IP;
                        Global.VisionSystem.ConnectionData[i].MAC = ServerData.Client.MAC;

                        Global.VisionSystem.ConnectionData[i].CAM = VisionSystemClassLibrary.Enum.CameraState.CONNECTED;
                        Global.VisionSystem.ConnectionData[i].Port = bytePort;
                        Global.VisionSystem.ConnectionData[i].Type = Cameratype;
                        Global.VisionSystem.ConnectionData[i].SerialNumber = sSerialNumber;

                        break;
                    }
                }

                Global.VisionSystem._SetLanguage_Device();//设置设备名称

                //

                if (null != Global.VisionSystem.Camera)
                {
                    for (i = 0; i < Global.VisionSystem.Camera.Count; i++)//遍历已接纳相机
                    {
                        if (Cameratype == Global.VisionSystem.Camera[i].Type)//目标相机
                        {
                            Global.VisionSystem.Camera[i].DeviceInformation.Connected = ServerData.Client.Connected;
                            Global.VisionSystem.Camera[i].DeviceInformation.Firmware = sFirmwareVersion;
                            Global.VisionSystem.Camera[i].DeviceInformation.GetDevInfo = ServerData.Client.GetDevInfo;
                            Global.VisionSystem.Camera[i].DeviceInformation.IP = ServerData.Client.IP;
                            Global.VisionSystem.Camera[i].DeviceInformation.MAC = ServerData.Client.MAC;

                            Global.VisionSystem.Camera[i].DeviceInformation.CAM = VisionSystemClassLibrary.Enum.CameraState.CONNECTED;
                            Global.VisionSystem.Camera[i].DeviceInformation.SerialNumber = sSerialNumber;
                            Global.VisionSystem.Camera[i].DeviceInformation.Port = bytePort;

                            for (Byte j = 0; j < Global.VisionSystem.Camera.Count; j++)//遍历已接纳相机
                            {
                                if (Global.VisionSystem.Camera[j].ControllerENGName == Global.VisionSystem.Camera[i].ControllerENGName) //当前属于同一控制器
                                {
                                    Global.VisionSystem.Camera[j].DeviceInformation.IP = Global.VisionSystem.Camera[i].DeviceInformation.IP;
                                }
                            }

                            break;
                        }
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：客户端断开连接后，复位相机信息
        // 输入参数：1.ServerException：异常信息
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _ResetCameraParameter(VisionSystemCommunicationLibrary.Ethernet.ExceptionHandledEventArgs ServerException)
        {
            Int32 i = 0;

            for (i = 0; i < Global.VisionSystem.ConnectionData.Length; i++)//遍历设备
            {
                if (ServerException.Client.IP == Global.VisionSystem.ConnectionData[i].IP)//有效
                {
                    Global.VisionSystem.ConnectionData[i].Connected = false;
                    Global.VisionSystem.ConnectionData[i].Firmware = "";
                    Global.VisionSystem.ConnectionData[i].GetDevInfo = false;
                    Global.VisionSystem.ConnectionData[i].IP = "";
                    Global.VisionSystem.ConnectionData[i].MAC = "";

                    Global.VisionSystem.ConnectionData[i].CAM = VisionSystemClassLibrary.Enum.CameraState.NOTCONNECTED;
                    Global.VisionSystem.ConnectionData[i].DeviceName = "";
                    Global.VisionSystem.ConnectionData[i].ControllerName = "";
                    Global.VisionSystem.ConnectionData[i].Type = VisionSystemClassLibrary.Enum.CameraType.None;
                    Global.VisionSystem.ConnectionData[i].SerialNumber = "";
                    Global.VisionSystem.ConnectionData[i].Port = 0;
                }
            }

            if (null != Global.VisionSystem.Camera)
            {
                for (i = 0; i < Global.VisionSystem.Camera.Count; i++)//遍历已接纳相机
                {
                    if (ServerException.Client.IP == Global.VisionSystem.Camera[i].DeviceInformation.IP)//目标相机
                    {
                        Global.VisionSystem.Camera[i].DeviceInformation.Connected = false;
                        Global.VisionSystem.Camera[i].DeviceInformation.Firmware = "";
                        Global.VisionSystem.Camera[i].DeviceInformation.GetDevInfo = false;
                        Global.VisionSystem.Camera[i].DeviceInformation.IP = "";
                        Global.VisionSystem.Camera[i].DeviceInformation.MAC = "";

                        Global.VisionSystem.Camera[i].DeviceInformation.CAM = VisionSystemClassLibrary.Enum.CameraState.NOTCONNECTED;
                        Global.VisionSystem.Camera[i].DeviceInformation.CAM_Temp = Global.VisionSystem.Camera[i].DeviceInformation.CAM;
                        Global.VisionSystem.Camera[i].DeviceInformation.SerialNumber = "";
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：客户端断开连接后，复位相机信息
        // 输入参数：1.IP：IP地址
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _ResetCameraParameter(String IP)
        {
            Int32 i = 0;

            for (i = 0; i < Global.VisionSystem.ConnectionData.Length; i++)//遍历设备
            {
                if (IP == Global.VisionSystem.ConnectionData[i].IP)//有效
                {
                    Global.VisionSystem.ConnectionData[i].Connected = false;
                    Global.VisionSystem.ConnectionData[i].Firmware = "";
                    Global.VisionSystem.ConnectionData[i].GetDevInfo = false;
                    Global.VisionSystem.ConnectionData[i].IP = "";
                    Global.VisionSystem.ConnectionData[i].MAC = "";

                    Global.VisionSystem.ConnectionData[i].CAM = VisionSystemClassLibrary.Enum.CameraState.NOTCONNECTED;
                    Global.VisionSystem.ConnectionData[i].DeviceName = "";
                    Global.VisionSystem.ConnectionData[i].Type = VisionSystemClassLibrary.Enum.CameraType.None;
                    Global.VisionSystem.ConnectionData[i].SerialNumber = "";
                    Global.VisionSystem.ConnectionData[i].Port = 0;

                    break;
                }
            }

            if (null != Global.VisionSystem.Camera)
            {
                for (i = 0; i < Global.VisionSystem.Camera.Count; i++)//遍历已接纳相机
                {
                    if (IP == Global.VisionSystem.Camera[i].DeviceInformation.IP)//目标相机
                    {
                        Global.VisionSystem.Camera[i].DeviceInformation.Connected = false;
                        Global.VisionSystem.Camera[i].DeviceInformation.Firmware = "";
                        Global.VisionSystem.Camera[i].DeviceInformation.GetDevInfo = false;
                        Global.VisionSystem.Camera[i].DeviceInformation.IP = "";
                        Global.VisionSystem.Camera[i].DeviceInformation.MAC = "";

                        Global.VisionSystem.Camera[i].DeviceInformation.CAM = VisionSystemClassLibrary.Enum.CameraState.NOTCONNECTED;
                        Global.VisionSystem.Camera[i].DeviceInformation.CAM_Temp = Global.VisionSystem.Camera[i].DeviceInformation.CAM;
                        Global.VisionSystem.Camera[i].DeviceInformation.SerialNumber = "";

                        //

                        break;
                    }
                }
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：获取指定类型相机的IP地址
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public String _GetCameraIPAddress(VisionSystemClassLibrary.Enum.CameraType Cameratype)
        {
            String sReturn = "";

            for (int i = 0; i < Global.VisionSystem.Camera.Count; i++)//遍历相机
            {
                if (Cameratype == Global.VisionSystem.Camera[i].Type)//目标相机
                {
                    sReturn = Global.VisionSystem.Camera[i].DeviceInformation.IP;

                    break;
                }
            }

            return sReturn;
        }

        //----------------------------------------------------------------------
        // 功能说明：获取指定类型相机的配置IP地址最后一位
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Int32 _GetCameraConfigurationIPAddress(VisionSystemClassLibrary.Enum.CameraType Cameratype)
        {
            Int32 iReturn = 2;

            for (int i = 0; i < Global.VisionSystem.SystemCameraConfiguration.Length; i++)//遍历系统配置相机
            {
                if (Cameratype == Global.VisionSystem.SystemCameraConfiguration[i].Type)//目标相机
                {
                    iReturn = Global.VisionSystem.SystemCameraConfiguration[i].IPValue;

                    break;
                }
            }

            return iReturn;
        }

        //----------------------------------------------------------------------
        // 功能说明：获取指定类型相机的控制器名称
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public String _GetControllerName(VisionSystemClassLibrary.Enum.CameraType Cameratype)
        {
            String sReturn = "";

            for (int i = 0; i < Global.VisionSystem.SystemCameraConfiguration.Length; i++)//遍历系统配置相机
            {
                if (Cameratype == Global.VisionSystem.SystemCameraConfiguration[i].Type)//目标相机
                {
                    sReturn = Global.VisionSystem.SystemCameraConfiguration[i].ControllerENGName;

                    break;
                }
            }

            return sReturn;
        }

        //----------------------------------------------------------------------
        // 功能说明：获取指定类型相机的端口
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Byte _GetCameraPort(VisionSystemClassLibrary.Enum.CameraType Cameratype)
        {
            Byte byteReturn = 1;

            for (int i = 0; i < Global.VisionSystem.Camera.Count; i++)//遍历相机
            {
                if (Cameratype == Global.VisionSystem.Camera[i].Type)//目标相机
                {
                    byteReturn = Global.VisionSystem.Camera[i].DeviceInformation.Port;

                    break;
                }
            }

            return byteReturn;
        }

        //----------------------------------------------------------------------
        // 功能说明：获取指定类型相机的索引值
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public int _GetSelectedCameraIndex(VisionSystemClassLibrary.Enum.CameraType Cameratype)
        {
            int iReturn = 0;

            for (int i = 0; i < Global.VisionSystem.Camera.Count; i++)//遍历相机
            {
                if (Cameratype == Global.VisionSystem.Camera[i].Type)//目标相机
                {
                    iReturn = i;

                    break;
                }
            }

            return iReturn;
        }

        //----------------------------------------------------------------------
        // 功能说明：获取指定类型相机的索引值
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public int _GetSystemCameraIndex(VisionSystemClassLibrary.Enum.CameraType Cameratype)
        {
            int iReturn = 0;

            for (int i = 0; i < Global.VisionSystem.SystemCameraConfiguration.Length; i++)//遍历相机
            {
                if (Cameratype == Global.VisionSystem.SystemCameraConfiguration[i].Type)//目标相机
                {
                    iReturn = i;

                    break;
                }
            }

            return iReturn;
        }

        //----------------------------------------------------------------------
        // 功能说明：获取指定类型相机名称
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public String _GetSystemCameraName(VisionSystemClassLibrary.Enum.CameraType Cameratype)
        {
            String sReturn = "";

            if (VisionSystemClassLibrary.Enum.CameraType.None == Cameratype)//无效
            {
                sReturn = "None";
            }
            else//有效
            {
                sReturn = Global.VisionSystem.SystemCameraConfiguration[_GetSystemCameraIndex(Cameratype)].CameraENGName;
            }

            return sReturn;
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：设置班次统计数据
        // 输入参数：1.statisticsrecordlist：统计列表
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetShiftStatisticsInformation(VisionSystemClassLibrary.Struct.StatisticsRecordList statisticsrecordlist)
        {
            Int32 i = 0;//循环控制变量
            Int32 j = 0;//循环控制变量

            Global.VisionSystem.Shift.DataOfShift.CurrentIndex = statisticsrecordlist.CurrentShiftIndex;

            Global.VisionSystem.Shift.DataOfShift.InformationOfStatistics = new VisionSystemClassLibrary.Struct.StatisticsInformation[Global.VisionSystem.Shift.DataOfShift.TimeData.Length];

            for (i = 0; i < Global.VisionSystem.Shift.DataOfShift.TimeData.Length; i++)
            {
                Global.VisionSystem.Shift.DataOfShift.InformationOfStatistics[i] = new VisionSystemClassLibrary.Struct.StatisticsInformation();

                if (i == Global.VisionSystem.Shift.DataOfShift.CurrentIndex)//当前班次
                {
                    Global.VisionSystem.Shift.DataOfShift.InformationOfStatistics[i].CurrentIndex = statisticsrecordlist.RecordListData[i].CurrentStatisticsRecordIndex;
                } 
                else//其它班次
                {
                    Global.VisionSystem.Shift.DataOfShift.InformationOfStatistics[i].CurrentIndex = 0;
                }

                //

                if (null != statisticsrecordlist.RecordListData[i].TimeData)//存在统计数据
                {
                    Global.VisionSystem.Shift.DataOfShift.InformationOfStatistics[i].TimeData = new VisionSystemClassLibrary.Struct.ShiftTime[statisticsrecordlist.RecordListData[i].TimeData.Length];
                    Global.VisionSystem.Shift.DataOfShift.InformationOfStatistics[i].DataOfStatistics = new VisionSystemClassLibrary.Struct.StatisticsData[statisticsrecordlist.RecordListData[i].TimeData.Length];

                    for (j = 0; j < statisticsrecordlist.RecordListData[i].TimeData.Length; j++)//遍历统计数据
                    {
                        Global.VisionSystem.Shift.DataOfShift.InformationOfStatistics[i].TimeData[j] = new VisionSystemClassLibrary.Struct.ShiftTime();
                        Global.VisionSystem.Shift.DataOfShift.InformationOfStatistics[i].TimeData[j].Start = statisticsrecordlist.RecordListData[i].TimeData[j].Start;
                        Global.VisionSystem.Shift.DataOfShift.InformationOfStatistics[i].TimeData[j].End = statisticsrecordlist.RecordListData[i].TimeData[j].End;

                        Global.VisionSystem.Shift.DataOfShift.InformationOfStatistics[i].DataOfStatistics[j] = new VisionSystemClassLibrary.Struct.StatisticsData();
                        Global.VisionSystem.Shift.DataOfShift.InformationOfStatistics[i].DataOfStatistics[j].BrandName = statisticsrecordlist.RecordListData[i].BrandName[j];
                        Global.VisionSystem.Shift.DataOfShift.InformationOfStatistics[i].DataOfStatistics[j].CameraStatisticsData = null;
                    }
                } 
                else//不存在统计数据
                {
                    Global.VisionSystem.Shift.DataOfShift.InformationOfStatistics[i].TimeData = null;
                    Global.VisionSystem.Shift.DataOfShift.InformationOfStatistics[i].DataOfStatistics = null;
                }
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：设置故障信息
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetFaultMessage(Int32 iIndex)
        {
            if (Global.VisionSystem.Shift.ShiftState)//使能班次
            {
                if (0 >= Global.VisionSystem.Work.ConnectedCameraNumber)//不存在相机连接
                {
                    Global.WorkWindow.TitleBar.Invoke(new EventHandler(delegate { Global.WorkWindow.TitleBar._ResetFaultMessage(); }));//SYSTEM页面
                    Global.SystemConfigurationWindow.TitleBar.Invoke(new EventHandler(delegate { Global.SystemConfigurationWindow.TitleBar._ResetFaultMessage(); }));//SYSTEM页面
                    Global.DevicesSetupWindow.TitleBar.Invoke(new EventHandler(delegate { Global.DevicesSetupWindow.TitleBar._ResetFaultMessage(); }));//DEVICES SETUP页面                        
                    Global.BrandManagementWindow.TitleBar.Invoke(new EventHandler(delegate { Global.BrandManagementWindow.TitleBar._ResetFaultMessage(); }));//BRAND MANAGEMENT页面                        
                    Global.BackupBrandsWindow.TitleBar.Invoke(new EventHandler(delegate { Global.BackupBrandsWindow.TitleBar._ResetFaultMessage(); }));//BACKUP BRANDS页面                        
                    Global.RestoreBrandsWindow.TitleBar.Invoke(new EventHandler(delegate { Global.RestoreBrandsWindow.TitleBar._ResetFaultMessage(); }));//RESTORE BRANDS页面                        
                    Global.TolerancesSettingsWindow.TitleBar.Invoke(new EventHandler(delegate { Global.TolerancesSettingsWindow.TitleBar._ResetFaultMessage(); }));//TOLERANCES SETTINGS页面                        
                    Global.LiveViewWindow.TitleBar.Invoke(new EventHandler(delegate { Global.LiveViewWindow.TitleBar._ResetFaultMessage(); }));//LIVE VIEW页面
                    Global.QualityCheckWindow.TitleBar.Invoke(new EventHandler(delegate { Global.QualityCheckWindow.TitleBar._ResetFaultMessage(); }));//QUALITY CHECK页面
                    Global.ImageConfigurationWindow.TitleBar.Invoke(new EventHandler(delegate { Global.ImageConfigurationWindow.TitleBar._ResetFaultMessage(); }));//IMAGE CONFIGURATION页面
                    Global.StatisticsViewWindow.TitleBar.Invoke(new EventHandler(delegate { Global.StatisticsViewWindow.TitleBar._ResetFaultMessage(); }));//STATISTICS VIEW页面
                }
                else//存在相机连接
                {
                    Global.WorkWindow.TitleBar.Invoke(new EventHandler(delegate { Global.WorkWindow.TitleBar.FaultExist[iIndex] = false; }));//SYSTEM页面
                    Global.SystemConfigurationWindow.TitleBar.Invoke(new EventHandler(delegate { Global.SystemConfigurationWindow.TitleBar.FaultExist[iIndex] = false; }));//SYSTEM页面
                    Global.DevicesSetupWindow.TitleBar.Invoke(new EventHandler(delegate { Global.DevicesSetupWindow.TitleBar.FaultExist[iIndex] = false; }));//DEVICES SETUP页面                        
                    Global.BrandManagementWindow.TitleBar.Invoke(new EventHandler(delegate { Global.BrandManagementWindow.TitleBar.FaultExist[iIndex] = false; }));//BRAND MANAGEMENT页面                        
                    Global.BackupBrandsWindow.TitleBar.Invoke(new EventHandler(delegate { Global.BackupBrandsWindow.TitleBar.FaultExist[iIndex] = false; }));//BACKUP BRANDS页面                        
                    Global.RestoreBrandsWindow.TitleBar.Invoke(new EventHandler(delegate { Global.RestoreBrandsWindow.TitleBar.FaultExist[iIndex] = false; }));//RESTORE BRANDS页面                        
                    Global.TolerancesSettingsWindow.TitleBar.Invoke(new EventHandler(delegate { Global.TolerancesSettingsWindow.TitleBar.FaultExist[iIndex] = false; }));//TOLERANCES SETTINGS页面                        
                    Global.LiveViewWindow.TitleBar.Invoke(new EventHandler(delegate { Global.LiveViewWindow.TitleBar.FaultExist[iIndex] = false; }));//LIVE VIEW页面
                    Global.QualityCheckWindow.TitleBar.Invoke(new EventHandler(delegate { Global.QualityCheckWindow.TitleBar.FaultExist[iIndex] = false; }));//QUALITY CHECK页面
                    Global.ImageConfigurationWindow.TitleBar.Invoke(new EventHandler(delegate { Global.ImageConfigurationWindow.TitleBar.FaultExist[iIndex] = false; }));//IMAGE CONFIGURATION页面
                    Global.StatisticsViewWindow.TitleBar.Invoke(new EventHandler(delegate { Global.StatisticsViewWindow.TitleBar.FaultExist[iIndex] = false; }));//STATISTICS VIEW页面
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：设置当前故障信息
        // 输入参数：1.iCameraIndex：相机索引值
        //         2.faultmessage：故障索引值
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetFaultMessage(Int32 iCameraIndex, VisionSystemClassLibrary.Struct.FaultMessage faultmessage)
        {
            Global.WorkWindow.TitleBar.Invoke(new EventHandler(delegate { Global.WorkWindow.TitleBar._SetFaultMessage(iCameraIndex, faultmessage); }));//SYSTEM页面
            Global.SystemConfigurationWindow.TitleBar.Invoke(new EventHandler(delegate { Global.SystemConfigurationWindow.TitleBar._SetFaultMessage(iCameraIndex, faultmessage); }));//SYSTEM页面
            Global.DevicesSetupWindow.TitleBar.Invoke(new EventHandler(delegate { Global.DevicesSetupWindow.TitleBar._SetFaultMessage(iCameraIndex, faultmessage); }));//DEVICES SETUP页面                        
            Global.BrandManagementWindow.TitleBar.Invoke(new EventHandler(delegate { Global.BrandManagementWindow.TitleBar._SetFaultMessage(iCameraIndex, faultmessage); }));//BRAND MANAGEMENT页面                        
            Global.BackupBrandsWindow.TitleBar.Invoke(new EventHandler(delegate { Global.BackupBrandsWindow.TitleBar._SetFaultMessage(iCameraIndex, faultmessage); }));//BACKUP BRANDS页面                        
            Global.RestoreBrandsWindow.TitleBar.Invoke(new EventHandler(delegate { Global.RestoreBrandsWindow.TitleBar._SetFaultMessage(iCameraIndex, faultmessage); }));//RESTORE BRANDS页面                        
            Global.TolerancesSettingsWindow.TitleBar.Invoke(new EventHandler(delegate { Global.TolerancesSettingsWindow.TitleBar._SetFaultMessage(iCameraIndex, faultmessage); }));//TOLERANCES SETTINGS页面                        
            Global.LiveViewWindow.TitleBar.Invoke(new EventHandler(delegate { Global.LiveViewWindow.TitleBar._SetFaultMessage(iCameraIndex, faultmessage); }));//LIVE VIEW页面
            Global.QualityCheckWindow.TitleBar.Invoke(new EventHandler(delegate { Global.QualityCheckWindow.TitleBar._SetFaultMessage(iCameraIndex, faultmessage); }));//QUALITY CHECK页面
            Global.ImageConfigurationWindow.TitleBar.Invoke(new EventHandler(delegate { Global.ImageConfigurationWindow.TitleBar._SetFaultMessage(iCameraIndex, faultmessage); }));//IMAGE CONFIGURATION页面
            Global.StatisticsViewWindow.TitleBar.Invoke(new EventHandler(delegate { Global.StatisticsViewWindow.TitleBar._SetFaultMessage(iCameraIndex, faultmessage); }));//STATISTICS VIEW页面

            Global.DevicesSetupWindow.CustomControl.Invoke(new EventHandler(delegate { Global.DevicesSetupWindow.CustomControl._SetFaultMessage(iCameraIndex, faultmessage); }));//DEVICES SETUP页面
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：生成指令
        // 输入参数：1.InstructionType：指令类型
        //         2.Cameratype：相机类型
        //         3.sBrandName：品牌名称
        //         4.iValue_1：文件索引值（从0开始）
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private Byte[] _GenerateInstruction(CommunicationInstructionType InstructionType, VisionSystemClassLibrary.Enum.CameraType Cameratype, Int32 CameraChooseState, String sBrandName, Int32 iValue_1)
        {
            //指令类型 + 相机类型数据 + 设置相机模式 + 品牌名称长度 + 品牌名称 + 文件索引值（从0开始） + 文件

            Int32 iIndex = 0;//临时变量

            Byte[] arrayValue_2 = VisionSystemCommunicationLibrary.Ethernet.TestControlFunctions._StringToByte(sBrandName);//品牌名称
            Byte[] arrayValue_1 = BitConverter.GetBytes(arrayValue_2.Length);//品牌名称长度
            Byte[] arrayValue_3 = BitConverter.GetBytes(iValue_1);//文件索引值（从0开始）
            Byte[] arrayValue_4 = BitConverter.GetBytes(CameraChooseState);//设置相机模式
            
            Byte[] arrayInstructionData = new Byte[1 + 1 + arrayValue_1.Length + arrayValue_2.Length + arrayValue_3.Length + arrayValue_4.Length];//待发送数据

            arrayInstructionData[0] = (Byte)InstructionType;//填充待发送数据，指令标志位
            arrayInstructionData[1] = (Byte)Cameratype;//填充待发送数据，相机类型数据
            arrayValue_4.CopyTo(arrayInstructionData, 2);//填充待发送数据，设置相机模式
            iIndex = 2 + arrayValue_4.Length;
            arrayValue_1.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，品牌名称长度
            iIndex += arrayValue_1.Length;
            arrayValue_2.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，品牌名称
            iIndex += arrayValue_2.Length;
            arrayValue_3.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，文件索引值（从0开始）

            //

            return arrayInstructionData;
        }

        //----------------------------------------------------------------------
        // 功能说明：生成指令
        // 输入参数：1.InstructionType：指令类型
        //         2.Cameratype：相机类型
        //         3.CameraChooseState：设置相机模式：单/双相机模式
        //         4.sBrandName：品牌名称
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private Byte[] _GenerateInstruction(CommunicationInstructionType InstructionType, VisionSystemClassLibrary.Enum.CameraType Cameratype, Int32 CameraChooseState, String sBrandName)
        {
            //指令类型 + 相机类型数据 + 设置相机模式 + 品牌名称长度 + 品牌名称 + 文件索引值（从0开始） + 文件

            Int32 iIndex = 0;//临时变量

            Byte[] arrayValue_2 = VisionSystemCommunicationLibrary.Ethernet.TestControlFunctions._StringToByte(sBrandName);//品牌名称
            Byte[] arrayValue_1 = BitConverter.GetBytes(arrayValue_2.Length);//品牌名称长度
            Byte[] arrayValue_3 = BitConverter.GetBytes(CameraChooseState);//设置相机模式

            Byte[] arrayInstructionData = new Byte[1 + 1 + arrayValue_1.Length + arrayValue_2.Length + arrayValue_3.Length];//待发送数据

            arrayInstructionData[0] = (Byte)InstructionType;//填充待发送数据，指令标志位
            arrayInstructionData[1] = (Byte)Cameratype;//填充待发送数据，相机类型数据
            arrayValue_3.CopyTo(arrayInstructionData, 2);//填充待发送数据，设置相机模式
            iIndex = 2 + arrayValue_3.Length;
            arrayValue_1.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，品牌名称长度
            iIndex += arrayValue_1.Length;
            arrayValue_2.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，品牌名称

            //

            return arrayInstructionData;
        }

        //----------------------------------------------------------------------
        // 功能说明：生成指令
        // 输入参数：1.InstructionType：指令类型
        //         2.Cameratype：相机类型
        //         3.iSelectedIPAddress：选择的IP地址
        //         4.SelectedCameratype：选择的相机类型
        //         5.byteSelectedPort：选择的相机端口
        //         6.CameraChooseState：选择的相机模式
        //         7.uCameraFaultState：选择为的相机故障标记
        //         8.bCheckEnable：选择为的相机检测使能
        //         9.bCameraAngle：选择为的相机旋转角度
        //         10.vVideoColor：相机颜色
        //         11.vVideoResolution：相机分辨率
        //         12.bSerialPort：是否为串口
        //         13.tTobaccoSortType：烟支排列类型
        //         14.bBitmapResize：选择为的相机数据截取区域缩放
        //         15.bBitmapCenter：选择为的相机数据截取区域缩放后是否居中
        //         16.pBitmapAxis：选择为的相机数据截取区域粘贴区域
        //         17.rBitmapArea：选择为的相机数据截取区域
        //         18.bCameraFlip：选择的相机镜像标记
        //         19.sSensorProductType：选择的传感器应用场景
        //         20.relevancyCameraInformation：相机关联信息
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private Byte[] _GenerateInstruction(CommunicationInstructionType InstructionType, VisionSystemClassLibrary.Enum.CameraType Cameratype, Int32 iSelectedIPAddress, VisionSystemClassLibrary.Enum.CameraType SelectedCameratype, Byte byteSelectedPort, Int32 CameraChooseState, UInt64 uCameraFaultState, Boolean bCheckEnable, VisionSystemClassLibrary.Enum.CameraRotateAngle bCameraAngle, VisionSystemClassLibrary.Enum.VideoColor vVideoColor, VisionSystemClassLibrary.Enum.VideoResolution vVideoResolution, Boolean bSerialPort, VisionSystemClassLibrary.Enum.TobaccoSortType tTobaccoSortType, Boolean bBitmapResize, Boolean bBitmapCenter, Point pBitmapAxis, Rectangle rBitmapArea, VisionSystemClassLibrary.Enum.CameraFlip bCameraFlip, VisionSystemClassLibrary.Enum.SensorProductType sSensorProductType, VisionSystemClassLibrary.Struct.RelevancyCameraInformation relevancyCameraInformation)
        {
            //指令类型 + 相机类型数据 + 设置为的IP地址 + 设置为的相机类型数据 + 设置为的端口 + 设置为的模式 + 设置为的相机故障标记 + 设置为的相机旋转角度 + 相机颜色 + 相机分辨率 + 是否为串口 + 烟支排列类型 + 设置为的相机数据截取区域缩放 + 设置为的相机数据截取区域缩放后是否居中 + 设置为的相机数据截取区域粘贴区域 + 设置为的相机数据截取区域 + 镜像标记 + 传感器应用场景 + 相机关联信息（关联类型 + 关联数量（相机类型 + 工位索引））

            Int32 iIndex = 0;//临时变量

            Byte[] arrayValue_1 = BitConverter.GetBytes(iSelectedIPAddress);//设置为的IP地址
            Byte[] arrayValue_2 = BitConverter.GetBytes((Int32)SelectedCameratype);//设置为的相机类型数据
            Byte[] arrayValue_3 = BitConverter.GetBytes((Int32)byteSelectedPort);//设置为的相机端口
            Byte[] arrayValue_4 = BitConverter.GetBytes(CameraChooseState);//设置为的相机模式
            Byte[] arrayValue_5 = BitConverter.GetBytes(uCameraFaultState);//设置为的相机故障标记

            Byte[] arrayValue_6 = null;//设置为的相机旋转角度
            if (bCheckEnable)//设置为的相机检测使能
            {
                arrayValue_6 = BitConverter.GetBytes((Int32)1);
            }
            else
            {
                arrayValue_6 = BitConverter.GetBytes((Int32)0);
            }

            Byte[] arrayValue_7 = BitConverter.GetBytes(Convert.ToInt32(bCameraAngle));//设置为的相机旋转角度

            Byte[] arrayValue_8 = BitConverter.GetBytes(Convert.ToInt32(vVideoColor));//设置为的相机颜色
            Byte[] arrayValue_9 = BitConverter.GetBytes(Convert.ToInt32(vVideoResolution));//设置为的相机分辨率

            Byte[] arrayValue_10 = null;//设置为串口标记
            if (bSerialPort)//设置为串口标记
            {
                arrayValue_10 = BitConverter.GetBytes((Int32)1);
            }
            else
            {
                arrayValue_10 = BitConverter.GetBytes((Int32)0);
            }

            Byte[] arrayValue_11 = BitConverter.GetBytes(Convert.ToInt32(tTobaccoSortType));//设置为烟支排列方式

            Byte[] arrayValue_12 = null;
            if (bBitmapResize)//设置为的相机数据截取区域缩放
            {
                arrayValue_12 = BitConverter.GetBytes((Int32)1);
            }
            else
            {
                arrayValue_12 = BitConverter.GetBytes((Int32)0);
            }

            Byte[] arrayValue_13 = null;
            if (bBitmapCenter)//设置为的相机数据截取区域缩放后是否居中
            {
                arrayValue_13 = BitConverter.GetBytes((Int32)1);
            }
            else
            {
                arrayValue_13 = BitConverter.GetBytes((Int32)0);
            }

            Byte[] arrayValue_14 = BitConverter.GetBytes(pBitmapAxis.X);//设置为的相机数据截取区域粘贴区域X
            Byte[] arrayValue_15 = BitConverter.GetBytes(pBitmapAxis.Y);//设置为的相机数据截取区域粘贴区域Y
            Byte[] arrayValue_16 = BitConverter.GetBytes(rBitmapArea.X);//设置为的相机数据截取区域X
            Byte[] arrayValue_17 = BitConverter.GetBytes(rBitmapArea.Y);//设置为的相机数据截取区域Y
            Byte[] arrayValue_18 = BitConverter.GetBytes(rBitmapArea.Width);//设置为的相机数据截取区域W
            Byte[] arrayValue_19 = BitConverter.GetBytes(rBitmapArea.Height);//设置为的相机数据截取区域H
            Byte[] arrayValue_20 = BitConverter.GetBytes(Convert.ToInt32(bCameraFlip));//设置为的相机镜像标记
            Byte[] arrayValue_21 = BitConverter.GetBytes(Convert.ToInt32(sSensorProductType));//设置为的传感器应用场景

            List<Byte> relevancyBytes = new List<Byte>();
            relevancyBytes.Add((Byte)relevancyCameraInformation.rRelevancyType); //关联类型

            if (VisionSystemClassLibrary.Enum.RelevancyType.None < relevancyCameraInformation.rRelevancyType) //相机存在
            {
                relevancyBytes.Add((Byte)relevancyCameraInformation.RelevancyCameraInfo.Count); //关联相机数量

                for (Int32 i = 0; i < relevancyCameraInformation.RelevancyCameraInfo.Count; i++)  //循环所有关联相机
                {
                    relevancyBytes.Add((Byte)relevancyCameraInformation.RelevancyCameraInfo.ElementAt(i).Key); //关联相机类型
                    relevancyBytes.Add(relevancyCameraInformation.RelevancyCameraInfo.ElementAt(i).Value); //关联相机工位
                }
            }

            Byte[] arrayInstructionData = new Byte[1 + 1 + arrayValue_1.Length + arrayValue_2.Length + arrayValue_3.Length + arrayValue_4.Length + arrayValue_5.Length + arrayValue_6.Length + arrayValue_7.Length + arrayValue_8.Length + arrayValue_9.Length + arrayValue_10.Length + arrayValue_11.Length + arrayValue_12.Length + arrayValue_13.Length + arrayValue_14.Length + arrayValue_15.Length + arrayValue_16.Length + arrayValue_17.Length + arrayValue_18.Length + arrayValue_19.Length + arrayValue_20.Length + arrayValue_21.Length + relevancyBytes.Count];//待发送数据

            arrayInstructionData[0] = (Byte)InstructionType;//填充待发送数据，指令标志位
            arrayInstructionData[1] = (Byte)Cameratype;//填充待发送数据，相机类型数据
            arrayValue_1.CopyTo(arrayInstructionData, 2);//填充待发送数据，设置为的IP地址
            iIndex = 2 + arrayValue_1.Length;
            arrayValue_2.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，设置为的相机类型数据
            iIndex += arrayValue_2.Length;
            arrayValue_3.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，设置为的相机端口
            iIndex += arrayValue_3.Length;
            arrayValue_4.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，设置为的相机模式
            iIndex += arrayValue_4.Length;
            arrayValue_5.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，设置为的相机故障标记
            iIndex += arrayValue_5.Length;
            arrayValue_6.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，设置为的相机检测使能
            iIndex += arrayValue_6.Length;
            arrayValue_7.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，设置为的相机旋转角度
            iIndex += arrayValue_7.Length;
            arrayValue_8.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，设置为的相机颜色
            iIndex += arrayValue_8.Length;
            arrayValue_9.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，设置为的相机分辨率
            iIndex += arrayValue_9.Length;
            arrayValue_10.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，设置为的串口标记
            iIndex += arrayValue_10.Length;
            arrayValue_11.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，设置为的烟支排列方式
            iIndex += arrayValue_11.Length;
            arrayValue_12.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，设置为的相机数据截取区域缩放
            iIndex += arrayValue_12.Length;
            arrayValue_13.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，设置为的相机数据截取区域缩放后是否居中
            iIndex += arrayValue_13.Length;
            arrayValue_14.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，设置为的相机数据截取区域粘贴区域X
            iIndex += arrayValue_14.Length;
            arrayValue_15.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，设置为的相机数据截取区域粘贴区域Y
            iIndex += arrayValue_15.Length;
            arrayValue_16.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，设置为的相机数据截取区域X
            iIndex += arrayValue_16.Length;
            arrayValue_17.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，设置为的相机数据截取区域Y
            iIndex += arrayValue_17.Length;
            arrayValue_18.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，设置为的相机数据截取区域W
            iIndex += arrayValue_18.Length;
            arrayValue_19.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，设置为的相机数据截取区域H
            iIndex += arrayValue_19.Length;
            arrayValue_20.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，设置为的相机镜像标记
            iIndex += arrayValue_20.Length;
            arrayValue_21.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，设置为的传感器应用场景
            iIndex += arrayValue_21.Length;
            relevancyBytes.ToArray().CopyTo(arrayInstructionData, iIndex);//填充待发送数据，设置为的相机关联信息

            //

            return arrayInstructionData;
        }

        //----------------------------------------------------------------------
        // 功能说明：生成指令
        // 输入参数：1.InstructionType：指令类型
        //         2.Cameratype：相机类型
        //         3.iSelectedIPAddress：选择的IP地址
        //         4.SelectedCameratype：选择的相机类型
        //         5.byteSelectedPort：选择的相机端口
        //         6.CameraChooseState：选择的相机模式
        //         7.uCameraFaultState：选择为的相机故障标记
        //         8.bCheckEnable：选择为的相机检测使能
        //         9.bCameraAngle：选择为的相机旋转角度
        //         10.vVideoColor：相机颜色
        //         11.vVideoResolution：相机分辨率
        //         12.bSerialPort：是否为串口
        //         13.tTobaccoSortType：烟支排列类型
        //         14.bBitmapResize：选择为的相机数据截取区域缩放
        //         15.bBitmapCenter：选择为的相机数据截取区域缩放后是否居中
        //         16.pBitmapAxis：选择为的相机数据截取区域粘贴区域
        //         17.rBitmapArea：选择为的相机数据截取区域
        //         18.bCameraFlip：选择的相机镜像标记
        //         19.sSensorProductType：选择的传感器应用场景
        //         20.relevancyCameraInformation：相机关联信息
        //         21.iFileIndex：文件索引值
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private Byte[] _GenerateInstruction(CommunicationInstructionType InstructionType, VisionSystemClassLibrary.Enum.CameraType Cameratype, Int32 iSelectedIPAddress, VisionSystemClassLibrary.Enum.CameraType SelectedCameratype, Byte byteSelectedPort, Int32 CameraChooseState, UInt64 uCameraFaultState, Boolean bCheckEnable, VisionSystemClassLibrary.Enum.CameraRotateAngle bCameraAngle, VisionSystemClassLibrary.Enum.VideoColor vVideoColor, VisionSystemClassLibrary.Enum.VideoResolution vVideoResolution, Boolean bSerialPort, VisionSystemClassLibrary.Enum.TobaccoSortType tTobaccoSortType, Boolean bBitmapResize, Boolean bBitmapCenter, Point pBitmapAxis, Rectangle rBitmapArea, VisionSystemClassLibrary.Enum.CameraFlip bCameraFlip, VisionSystemClassLibrary.Enum.SensorProductType sSensorProductType, VisionSystemClassLibrary.Struct.RelevancyCameraInformation relevancyCameraInformation, Int32 iFileIndex)
        {
            //指令类型 + 相机类型数据 + 设置为的IP地址 + 设置为的相机类型数据 + 设置为的端口 + 设置为的模式 + 设置为的相机故障标记 + 设置为的相机检测使能 + 设置为的相机旋转角度 + 相机颜色 + 相机分辨率 + 是否为串口 + 烟支排列类型 + 设置为的相机数据截取区域缩放 + 设置为的相机数据截取区域缩放后是否居中 + 设置为的相机数据截取区域粘贴区域 + 设置为的相机数据截取区域 + 镜像标记 + 传感器应用场景 + 相机关联信息（关联类型 + 关联数量（相机类型 + 工位索引）） + 文件索引值（从0开始） + 文件

            Int32 iIndex = 0;//临时变量

            Byte[] arrayValue_1 = BitConverter.GetBytes(iSelectedIPAddress);//设置为的IP地址
            Byte[] arrayValue_2 = BitConverter.GetBytes((Int32)SelectedCameratype);//设置为的相机类型数据
            Byte[] arrayValue_3 = BitConverter.GetBytes((Int32)byteSelectedPort);//设置为的相机端口
            Byte[] arrayValue_4 = BitConverter.GetBytes(CameraChooseState);//设置为的相机模式
            Byte[] arrayValue_5 = BitConverter.GetBytes(uCameraFaultState);//设置为的相机故障标记

            Byte[] arrayValue_6 = null;//设置为的相机旋转角度
            if (bCheckEnable)//设置为的相机检测使能
            {
                arrayValue_6 = BitConverter.GetBytes((Int32)1);
            }
            else
            {
                arrayValue_6 = BitConverter.GetBytes((Int32)0);
            }

            Byte[] arrayValue_7 = BitConverter.GetBytes(Convert.ToInt32(bCameraAngle));//设置为的相机旋转角度

            Byte[] arrayValue_8 = BitConverter.GetBytes(Convert.ToInt32(vVideoColor));//设置为的相机颜色
            Byte[] arrayValue_9 = BitConverter.GetBytes(Convert.ToInt32(vVideoResolution));//设置为的相机分辨率

            Byte[] arrayValue_10 = null;//设置为串口标记
            if (bSerialPort)//设置为串口标记
            {
                arrayValue_10 = BitConverter.GetBytes((Int32)1);
            }
            else
            {
                arrayValue_10 = BitConverter.GetBytes((Int32)0);
            }

            Byte[] arrayValue_11 = BitConverter.GetBytes(Convert.ToInt32(tTobaccoSortType));//设置为烟支排列方式

            Byte[] arrayValue_12 = null;
            if (bBitmapResize)//设置为的相机数据截取区域缩放
            {
                arrayValue_12 = BitConverter.GetBytes((Int32)1);
            }
            else
            {
                arrayValue_12 = BitConverter.GetBytes((Int32)0);
            }

            Byte[] arrayValue_13 = null;
            if (bBitmapCenter)//设置为的相机数据截取区域缩放后是否居中
            {
                arrayValue_13 = BitConverter.GetBytes((Int32)1);
            }
            else
            {
                arrayValue_13 = BitConverter.GetBytes((Int32)0);
            }

            Byte[] arrayValue_14 = BitConverter.GetBytes(pBitmapAxis.X);//设置为的相机数据截取区域粘贴区域X
            Byte[] arrayValue_15 = BitConverter.GetBytes(pBitmapAxis.Y);//设置为的相机数据截取区域粘贴区域Y
            Byte[] arrayValue_16 = BitConverter.GetBytes(rBitmapArea.X);//设置为的相机数据截取区域X
            Byte[] arrayValue_17 = BitConverter.GetBytes(rBitmapArea.Y);//设置为的相机数据截取区域Y
            Byte[] arrayValue_18 = BitConverter.GetBytes(rBitmapArea.Width);//设置为的相机数据截取区域W
            Byte[] arrayValue_19 = BitConverter.GetBytes(rBitmapArea.Height);//设置为的相机数据截取区域H
            Byte[] arrayValue_20 = BitConverter.GetBytes(Convert.ToInt32(bCameraFlip));//设置为的相机镜像标记
            Byte[] arrayValue_21 = BitConverter.GetBytes(Convert.ToInt32(sSensorProductType));//设置为的传感器应用场景
            Byte[] arrayValue_22 = BitConverter.GetBytes(iFileIndex);//文件索引值（从0开始）

            List<Byte> relevancyBytes = new List<Byte>();
            relevancyBytes.Add((Byte)relevancyCameraInformation.rRelevancyType); //关联类型

            if (VisionSystemClassLibrary.Enum.RelevancyType.None < relevancyCameraInformation.rRelevancyType) //相机存在
            {
                relevancyBytes.Add((Byte)relevancyCameraInformation.RelevancyCameraInfo.Count); //关联相机数量

                for (Int32 i = 0; i < relevancyCameraInformation.RelevancyCameraInfo.Count; i++)  //循环所有关联相机
                {
                    relevancyBytes.Add((Byte)relevancyCameraInformation.RelevancyCameraInfo.ElementAt(i).Key); //关联相机类型
                    relevancyBytes.Add(relevancyCameraInformation.RelevancyCameraInfo.ElementAt(i).Value); //关联相机工位
                }
            }

            Byte[] arrayInstructionData = new Byte[1 + 1 + arrayValue_1.Length + arrayValue_2.Length + arrayValue_3.Length + arrayValue_4.Length + arrayValue_5.Length + arrayValue_6.Length + arrayValue_7.Length + arrayValue_8.Length + arrayValue_9.Length + arrayValue_10.Length + arrayValue_11.Length + arrayValue_12.Length + arrayValue_13.Length + arrayValue_14.Length + arrayValue_15.Length + arrayValue_16.Length + arrayValue_17.Length + arrayValue_18.Length + arrayValue_19.Length + arrayValue_20.Length + arrayValue_21.Length + arrayValue_22.Length + relevancyBytes.Count];//待发送数据

            arrayInstructionData[0] = (Byte)InstructionType;//填充待发送数据，指令标志位
            arrayInstructionData[1] = (Byte)Cameratype;//填充待发送数据，相机类型数据
            arrayValue_1.CopyTo(arrayInstructionData, 2);//填充待发送数据，设置为的IP地址
            iIndex = 2 + arrayValue_1.Length;
            arrayValue_2.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，设置为的相机类型数据
            iIndex += arrayValue_2.Length;
            arrayValue_3.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，设置为的相机端口
            iIndex += arrayValue_3.Length;
            arrayValue_4.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，设置为的相机模式
            iIndex += arrayValue_4.Length;
            arrayValue_5.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，设置为的相机故障标记
            iIndex += arrayValue_5.Length;
            arrayValue_6.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，设置为的相机检测使能
            iIndex += arrayValue_6.Length;
            arrayValue_7.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，设置为的相机旋转角度
            iIndex += arrayValue_7.Length;
            arrayValue_8.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，设置为的相机颜色
            iIndex += arrayValue_8.Length;
            arrayValue_9.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，设置为的相机分辨率
            iIndex += arrayValue_9.Length;
            arrayValue_10.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，设置为的串口标记
            iIndex += arrayValue_10.Length;
            arrayValue_11.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，设置为的烟支排列方式
            iIndex += arrayValue_11.Length;
            arrayValue_12.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，设置为的相机数据截取区域缩放
            iIndex += arrayValue_12.Length;
            arrayValue_13.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，设置为的相机数据截取区域缩放后是否居中
            iIndex += arrayValue_13.Length;
            arrayValue_14.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，设置为的相机数据截取区域粘贴区域X
            iIndex += arrayValue_14.Length;
            arrayValue_15.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，设置为的相机数据截取区域粘贴区域Y
            iIndex += arrayValue_15.Length;
            arrayValue_16.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，设置为的相机数据截取区域X
            iIndex += arrayValue_16.Length;
            arrayValue_17.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，设置为的相机数据截取区域Y
            iIndex += arrayValue_17.Length;
            arrayValue_18.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，设置为的相机数据截取区域W
            iIndex += arrayValue_18.Length;
            arrayValue_19.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，设置为的相机数据截取区域H
            iIndex += arrayValue_19.Length;
            arrayValue_20.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，设置为的相机镜像标记
            iIndex += arrayValue_20.Length;
            arrayValue_21.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，设置为的传感器应用场景
            iIndex += arrayValue_21.Length;
            relevancyBytes.ToArray().CopyTo(arrayInstructionData, iIndex);//填充待发送数据，设置为的相机关联信息
            iIndex += relevancyBytes.Count;
            arrayValue_22.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，文件索引值（从0开始）

            //

            return arrayInstructionData;
        }

        //----------------------------------------------------------------------
        // 功能说明：生成指令
        // 输入参数：1.InstructionType：指令类型
        //         2.Cameratype：相机类型
        //         3.iValue_1：数据
        //         4.iValue_2：数据
        //         5.ArithmeticData：工具参数
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private Byte[] _GenerateInstruction(CommunicationInstructionType InstructionType, VisionSystemClassLibrary.Enum.CameraType Cameratype, Int32 iValue_1, Int32 iValue_2, VisionSystemClassLibrary.Struct.Arithmetic ArithmeticData)
        {
            //指令类型 + 相机类型数据 + 工具索引 + 图像类型（1，在线；2，学习；3，剔除） + 工具参数

            IFormatter formatter = new SoapFormatter();//格式化对象
            MemoryStream Memorystream = new MemoryStream();//流对象

            VisionSystemCommunicationLibrary.Ethernet.SerializableData ethernetData = new VisionSystemCommunicationLibrary.Ethernet.SerializableData();//待序列化数据

            ethernetData.Data_0 = new Byte[1];
            ethernetData.Data_0[0] = (Byte)Cameratype;//相机类型数据

            ethernetData.Data_1 = BitConverter.GetBytes(iValue_1);//工具索引

            ethernetData.Data_2 = BitConverter.GetBytes(iValue_2);//图像类型（1，在线；2，学习；3，剔除）
            
            IFormatter formatter_ToolParamter = new SoapFormatter();//格式化对象
            MemoryStream Memorystream_ToolParamter = new MemoryStream();//流对象

            formatter_ToolParamter.Serialize(Memorystream_ToolParamter, ArithmeticData);//序列化

            ethernetData.Data_4 = Memorystream_ToolParamter.ToArray();//工具参数

            formatter.Serialize(Memorystream, ethernetData);//序列化

            //

            Byte[] InstructionData_EthernetData = Memorystream.ToArray();//序列化数据
            Byte[] arrayEthernetDataLength = BitConverter.GetBytes(InstructionData_EthernetData.Length);//序列化数据长度

            Byte[] InstructionData = new Byte[1 + arrayEthernetDataLength.Length + InstructionData_EthernetData.Length];//待发送的数据

            InstructionData[0] = (Byte)InstructionType;//填充待发送数据，指令标志位
            arrayEthernetDataLength.CopyTo(InstructionData, 1);//填充待发送数据，序列化数据长度
            InstructionData_EthernetData.CopyTo(InstructionData, 1 + arrayEthernetDataLength.Length);//填充待发送数据，序列化数据

            //

            return InstructionData;
        }

        //----------------------------------------------------------------------
        // 功能说明：生成指令
        // 输入参数：1.InstructionType：指令类型
        //         2.Cameratype：相机类型
        //         3.ToolsData：工具数据
        //         4.iValue_1：当前工具索引
        //         5.iValue_2：当前工具索引
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private Byte[] _GenerateInstruction(CommunicationInstructionType InstructionType, VisionSystemClassLibrary.Enum.CameraType Cameratype, List<VisionSystemClassLibrary.Class.Tools> ToolsData, Int32 iValue_1, Int32 iValue_2)
        {
            //指令类型 + 相机类型数据 + 启用工具标记 + 当前工具索引 + 图像类型（1，在线；2，学习；3，剔除）

            IFormatter formatter = new SoapFormatter();//格式化对象
            MemoryStream Memorystream = new MemoryStream();//流对象

            VisionSystemCommunicationLibrary.Ethernet.SerializableData ethernetData = new VisionSystemCommunicationLibrary.Ethernet.SerializableData();//待序列化数据

            ethernetData.Data_0 = new Byte[1];
            ethernetData.Data_0[0] = (Byte)Cameratype;//相机类型数据

            Int32 iValueLength_Int32 = BitConverter.GetBytes((Int32)1).Length;//数据长度

            ethernetData.Data_1 = new Byte[iValueLength_Int32 * ToolsData.Count];//启用工具标记

            for (Int32 i = 0; i < ToolsData.Count; i++)//获取数据
            {
                BitConverter.GetBytes(Convert.ToInt32(ToolsData[i].ToolState)).CopyTo(ethernetData.Data_1, i * iValueLength_Int32);
            }

            ethernetData.Data_2 = BitConverter.GetBytes(iValue_1);//当前工具索引

            ethernetData.Data_3 = BitConverter.GetBytes(iValue_2);//图像类型（1，在线；2，学习；3，剔除）

            formatter.Serialize(Memorystream, ethernetData);//序列化

            //

            Byte[] InstructionData_EthernetData = Memorystream.ToArray();//序列化数据
            Byte[] arrayEthernetDataLength = BitConverter.GetBytes(InstructionData_EthernetData.Length);//序列化数据长度

            Byte[] InstructionData = new Byte[1 + arrayEthernetDataLength.Length + InstructionData_EthernetData.Length];//待发送的数据

            InstructionData[0] = (Byte)InstructionType;//填充待发送数据，指令标志位
            arrayEthernetDataLength.CopyTo(InstructionData, 1);//填充待发送数据，序列化数据长度
            InstructionData_EthernetData.CopyTo(InstructionData, 1 + arrayEthernetDataLength.Length);//填充待发送数据，序列化数据

            //

            return InstructionData;
        }

        //----------------------------------------------------------------------
        // 功能说明：生成指令
        // 输入参数：1.InstructionType：指令类型
        //         2.iValue_1：数值
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private Byte[] _GenerateInstruction(CommunicationInstructionType InstructionType, Int32 iValue_1)
        {
            //指令类型 + 数据（上位机，1；下位机，2）

            Byte[] arrayValue_1 = BitConverter.GetBytes(iValue_1);//数值

            Byte[] arrayInstructionData = new Byte[1 + arrayValue_1.Length];//待发送数据

            arrayInstructionData[0] = (Byte)InstructionType;//填充待发送数据，指令标志位
            arrayValue_1.CopyTo(arrayInstructionData, 1);//填充待发送数据，数据（上位机，1；下位机，2）

            //

            return arrayInstructionData;
        }

        //----------------------------------------------------------------------
        // 功能说明：生成指令
        // 输入参数：1.InstructionType：指令类型
        //         2.Cameratype：相机类型
        //         3.iValue_1：数值
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private Byte[] _GenerateInstruction(CommunicationInstructionType InstructionType, VisionSystemClassLibrary.Enum.CameraType Cameratype, Int32 iValue_1)
        {
            //指令类型 + 相机类型数据 + 工具索引数值

            Byte[] arrayValue_1 = BitConverter.GetBytes(iValue_1);//数值

            Byte[] arrayInstructionData = new Byte[1 + 1 + arrayValue_1.Length];//待发送数据

            arrayInstructionData[0] = (Byte)InstructionType;//填充待发送数据，指令标志位
            arrayInstructionData[1] = (Byte)Cameratype;//填充待发送数据，相机类型数据
            arrayValue_1.CopyTo(arrayInstructionData, 2);//填充待发送数据，工具索引数值

            //

            return arrayInstructionData;
        }

        //----------------------------------------------------------------------
        // 功能说明：生成指令
        // 输入参数：1.InstructionType：指令类型
        //         2.Cameratype：相机类型
        //         3.iValue_1：数值
        //         4.iValue_2：数值
        //         5.iValue_3：数值
        //         6.iData：数值
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private Byte[] _GenerateInstruction(CommunicationInstructionType InstructionType, VisionSystemClassLibrary.Enum.CameraType Cameratype, Int32 iValue_1, Int32 iValue_2, Int32 iValue_3, Byte[] iData)
        {
            //指令类型 + 相机类型数据 + 工具索引数值

            Byte[] arrayValue_1 = BitConverter.GetBytes(iValue_1);//数值
            Byte[] arrayValue_2 = BitConverter.GetBytes(iValue_2);//数值
            Byte[] arrayValue_3 = BitConverter.GetBytes(iValue_3);//数值

            Byte[] arrayInstructionData = new Byte[1 + 1 + arrayValue_1.Length + arrayValue_2.Length + arrayValue_3.Length + iData.Length];//待发送数据

            arrayInstructionData[0] = (Byte)InstructionType;//填充待发送数据，指令标志位
            arrayInstructionData[1] = (Byte)Cameratype;//填充待发送数据，相机类型数据

            Int32 iIndex = 2;

            arrayValue_1.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，烟支数量
            iIndex += 4;

            arrayValue_2.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，传感器校准选中状态
            iIndex += 4;

            arrayValue_3.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，校准标记
            iIndex += 4;

            iData.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，传感器校准值

            //

            return arrayInstructionData;
        }

        //----------------------------------------------------------------------
        // 功能说明：生成指令
        // 输入参数：1.InstructionType：指令类型
        //         2.Cameratype：相机类型
        //         3.iValue_0：数值
        //         4.iValue_1：数值
        //         5.iValue_2：数值
        //         6.iValue_3：数值
        //         7.sValue_1：数值
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private Byte[] _GenerateInstruction(CommunicationInstructionType InstructionType, VisionSystemClassLibrary.Enum.CameraType Cameratype, Int32 iValue_0, Int32 iValue_1, Int32 iValue_2, UInt64 iValue_3, String sValue_1)
        {
            //指令类型 + 相机类型数据 + 设置相机模式 + 当前选择机器 + 设备状态（VisionSystemClassLibrary.Enum.DeviceState） + 故障信息使能状态 + 品牌长度 + 品牌名称

            Int32 iIndex = 0;//临时变量

            Byte[] arrayValue_0 = BitConverter.GetBytes(iValue_0);//数值
            Byte[] arrayValue_1 = BitConverter.GetBytes(iValue_1);//数值
            Byte[] arrayValue_2 = BitConverter.GetBytes(iValue_2);//数值
            Byte[] arrayValue_3 = BitConverter.GetBytes(iValue_3);//数值

            Byte[] arrayValue_3_2 = VisionSystemCommunicationLibrary.Ethernet.TestControlFunctions._StringToByte(sValue_1);//数值
            Byte[] arrayValue_3_1 = BitConverter.GetBytes(arrayValue_3_2.Length);//数值

            Byte[] arrayInstructionData = new Byte[1 + 1 + arrayValue_0.Length + arrayValue_1.Length + arrayValue_2.Length + arrayValue_3.Length + arrayValue_3_1.Length + arrayValue_3_2.Length];//待发送数据

            arrayInstructionData[0] = (Byte)InstructionType;//填充待发送数据，指令标志位
            arrayInstructionData[1] = (Byte)Cameratype;//填充待发送数据，相机类型数据

            arrayValue_0.CopyTo(arrayInstructionData, 2);//填充待发送数据，相机颜色
            iIndex = 2 + arrayValue_0.Length;

            arrayValue_1.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，相机分辨率
            iIndex += arrayValue_1.Length;

            arrayValue_2.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，是否为串口
            iIndex += arrayValue_2.Length;

            arrayValue_3.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，烟支排列类型
            iIndex += arrayValue_3.Length;

            arrayValue_3_1.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，品牌长度
            iIndex += arrayValue_3_1.Length;

            arrayValue_3_2.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，品牌名称

            //

            return arrayInstructionData;
        }

        //----------------------------------------------------------------------
        // 功能说明：生成指令
        // 输入参数：1.InstructionType：指令类型
        //         2.Cameratype：相机类型
        //         3.iValue_1：数值
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private Byte[] _GenerateInstruction(CommunicationInstructionType InstructionType, VisionSystemClassLibrary.Enum.CameraType Cameratype, UInt32 iValue_1)
        {
            //指令类型 + 相机类型数据 + 工具索引数值

            Byte[] arrayValue_1 = BitConverter.GetBytes(iValue_1);//数值

            Byte[] arrayInstructionData = new Byte[1 + 1 + arrayValue_1.Length];//待发送数据

            arrayInstructionData[0] = (Byte)InstructionType;//填充待发送数据，指令标志位
            arrayInstructionData[1] = (Byte)Cameratype;//填充待发送数据，相机类型数据
            arrayValue_1.CopyTo(arrayInstructionData, 2);//填充待发送数据，工具索引数值

            //

            return arrayInstructionData;
        }

        //----------------------------------------------------------------------
        // 功能说明：生成指令
        // 输入参数：1.InstructionType：指令类型
        //         2.Cameratype：相机类型
        //         3.iValue_1：数值
        //         4.iValue_2：数值
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private Byte[] _GenerateInstruction(CommunicationInstructionType InstructionType, VisionSystemClassLibrary.Enum.CameraType Cameratype, Int32 iValue_1, Int32 iValue_2)
        {
            //指令类型 + 相机类型数据 + 工具索引数值 + 工具开关数值

            Int32 iIndex = 0;//临时变量

            Byte[] arrayValue_1 = BitConverter.GetBytes(iValue_1);//数值
            Byte[] arrayValue_2 = BitConverter.GetBytes(iValue_2);//数值

            Byte[] arrayInstructionData = new Byte[1 + 1 + arrayValue_1.Length + arrayValue_2.Length];//待发送数据

            arrayInstructionData[0] = (Byte)InstructionType;//填充待发送数据，指令标志位
            arrayInstructionData[1] = (Byte)Cameratype;//填充待发送数据，相机类型数据
            arrayValue_1.CopyTo(arrayInstructionData, 2);//填充待发送数据，工具索引数值
            iIndex = 2 + arrayValue_1.Length;
            arrayValue_2.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，工具开关数值

            //

            return arrayInstructionData;
        }

        //----------------------------------------------------------------------
        // 功能说明：生成指令
        // 输入参数：1.InstructionType：指令类型
        //         2.Cameratype：相机类型
        //         3.iValue_1：数值
        //         4.iValue_2：数值
        //         5.roi：工作区域
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private Byte[] _GenerateInstruction(CommunicationInstructionType InstructionType, VisionSystemClassLibrary.Enum.CameraType Cameratype, Int32 iValue_1, Int32 iValue_2, VisionSystemClassLibrary.Struct.ROI roi)
        {
            //指令类型 + 相机类型数据 + 工具索引 + 图像类型（1，在线；2，学习；3，剔除） + 工作区域

            Int32 iIndex = 0;//临时变量

            Byte[] arrayValue_1 = BitConverter.GetBytes(iValue_1);//数值
            Byte[] arrayValue_2 = BitConverter.GetBytes(iValue_2);//数值

            Byte[] arrayInstructionData = new Byte[1 + 1 + arrayValue_1.Length + arrayValue_2.Length + 76];//待发送数据

            arrayInstructionData[0] = (Byte)InstructionType;//填充待发送数据，指令标志位
            arrayInstructionData[1] = (Byte)Cameratype;//填充待发送数据，相机类型数据
            arrayValue_1.CopyTo(arrayInstructionData, 2);//填充待发送数据，工具索引
            iIndex = 2 + arrayValue_1.Length;
            arrayValue_2.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，图像类型（1，在线；2，学习；3，剔除
            iIndex += arrayValue_2.Length;
            BitConverter.GetBytes((Int32)roi.roiExtra.roiType).CopyTo(arrayInstructionData, iIndex);//填充待发送数据
            iIndex += 4;
            BitConverter.GetBytes(roi.roiExtra.Point1.X).CopyTo(arrayInstructionData, iIndex);//填充待发送数据
            iIndex += 4;
            BitConverter.GetBytes(roi.roiExtra.Point1.Y).CopyTo(arrayInstructionData, iIndex);//填充待发送数据
            iIndex += 4;
            BitConverter.GetBytes(roi.roiExtra.Point2.X).CopyTo(arrayInstructionData, iIndex);//填充待发送数据
            iIndex += 4;
            BitConverter.GetBytes(roi.roiExtra.Point2.Y).CopyTo(arrayInstructionData, iIndex);//填充待发送数据
            iIndex += 4;
            BitConverter.GetBytes(roi.roiExtra.Point3.X).CopyTo(arrayInstructionData, iIndex);//填充待发送数据
            iIndex += 4;
            BitConverter.GetBytes(roi.roiExtra.Point3.Y).CopyTo(arrayInstructionData, iIndex);//填充待发送数据
            iIndex += 4;
            BitConverter.GetBytes(roi.roiExtra.Point4.X).CopyTo(arrayInstructionData, iIndex);//填充待发送数据
            iIndex += 4;
            BitConverter.GetBytes(roi.roiExtra.Point4.Y).CopyTo(arrayInstructionData, iIndex);//填充待发送数据
            iIndex += 4;
            if (roi.roiInnerExit)//存在内部区域
            {
                BitConverter.GetBytes(1).CopyTo(arrayInstructionData, iIndex);//填充待发送数据
            }
            else
            {
                BitConverter.GetBytes(0).CopyTo(arrayInstructionData, iIndex);//填充待发送数据
            }
            iIndex += 4;
            BitConverter.GetBytes((Int32)roi.roiInner.roiType).CopyTo(arrayInstructionData, iIndex);//填充待发送数据
            iIndex += 4;
            BitConverter.GetBytes(roi.roiInner.Point1.X).CopyTo(arrayInstructionData, iIndex);//填充待发送数据
            iIndex += 4;
            BitConverter.GetBytes(roi.roiInner.Point1.Y).CopyTo(arrayInstructionData, iIndex);//填充待发送数据
            iIndex += 4;
            BitConverter.GetBytes(roi.roiInner.Point2.X).CopyTo(arrayInstructionData, iIndex);//填充待发送数据
            iIndex += 4;
            BitConverter.GetBytes(roi.roiInner.Point2.Y).CopyTo(arrayInstructionData, iIndex);//填充待发送数据
            iIndex += 4;
            BitConverter.GetBytes(roi.roiInner.Point3.X).CopyTo(arrayInstructionData, iIndex);//填充待发送数据
            iIndex += 4;
            BitConverter.GetBytes(roi.roiInner.Point3.Y).CopyTo(arrayInstructionData, iIndex);//填充待发送数据
            iIndex += 4;
            BitConverter.GetBytes(roi.roiInner.Point4.X).CopyTo(arrayInstructionData, iIndex);//填充待发送数据
            iIndex += 4;
            BitConverter.GetBytes(roi.roiInner.Point4.Y).CopyTo(arrayInstructionData, iIndex);//填充待发送数据
            //

            return arrayInstructionData;
        }

        //----------------------------------------------------------------------
        // 功能说明：生成指令
        // 输入参数：1.InstructionType：指令类型
        //         2.Cameratype：相机类型
        //         3.iValue_1：数值
        //         4.iValue_2：数值
        //         5.iValue_3：数值
        //         6.iValue_4：数值
        //         7.iValue_5：数值
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private Byte[] _GenerateInstruction(CommunicationInstructionType InstructionType, VisionSystemClassLibrary.Enum.CameraType Cameratype, Int32 iValue_1, Int32 iValue_2, Int32 iValue_3, Int32 iValue_4, Int32 iValue_5)
        {
            //指令类型 + 相机类型数据 + 工具索引数值 + 最小值数值（有效值） + 最大值数值（有效值） + 最小值数值（坐标轴数值） + 最大值数值（坐标轴数值）

            Int32 iIndex = 0;//临时变量

            Byte[] arrayValue_1 = BitConverter.GetBytes(iValue_1);//数值
            Byte[] arrayValue_2 = BitConverter.GetBytes(iValue_2);//数值
            Byte[] arrayValue_3 = BitConverter.GetBytes(iValue_3);//数值
            Byte[] arrayValue_4 = BitConverter.GetBytes(iValue_4);//数值
            Byte[] arrayValue_5 = BitConverter.GetBytes(iValue_5);//数值

            Byte[] arrayInstructionData = new Byte[1 + 1 + arrayValue_1.Length + arrayValue_2.Length + arrayValue_3.Length + arrayValue_4.Length + arrayValue_5.Length];//待发送数据

            arrayInstructionData[0] = (Byte)InstructionType;//填充待发送数据，指令标志位
            arrayInstructionData[1] = (Byte)Cameratype;//填充待发送数据，相机类型数据
            arrayValue_1.CopyTo(arrayInstructionData, 2);//填充待发送数据，工具索引
            iIndex = 2 + arrayValue_1.Length;
            arrayValue_2.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，最小值数值（有效值）
            iIndex += arrayValue_2.Length;
            arrayValue_3.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，最大值数值（有效值）
            iIndex += arrayValue_3.Length;
            arrayValue_4.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，最小值数值（坐标轴数值）
            iIndex += arrayValue_4.Length;
            arrayValue_5.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，最大值数值（坐标轴数值）

            //

            return arrayInstructionData;
        }

        //----------------------------------------------------------------------
        // 功能说明：生成指令
        // 输入参数：1.InstructionType：指令类型
        //         2.Cameratype：相机类型
        //         3.iValue_1：数值
        //         4.iValue_2：数值
        //         5.iValue_3：数值
        //         6.iValue_4：数值
        //         7.iValue_5：数值
        //         8.iValue_6：数值
        //         9.iValue_7：数值
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private Byte[] _GenerateInstruction(CommunicationInstructionType InstructionType, VisionSystemClassLibrary.Enum.CameraType Cameratype, Int32 iValue_1, Int32 iValue_2, Int32 iValue_3, Int32 iValue_4, Int32 iValue_5, Int32 iValue_6)
        {
            //指令类型 + 相机类型数据 + 工具索引 + 图像类型（1，在线；2，学习；3，剔除） + 工作区域横坐标 + 工作区域纵坐标 + 工作区域宽度 + 工作区域高度
            
            Int32 iIndex = 0;//临时变量

            Byte[] arrayValue_1 = BitConverter.GetBytes(iValue_1);//数值
            Byte[] arrayValue_2 = BitConverter.GetBytes(iValue_2);//数值
            Byte[] arrayValue_3 = BitConverter.GetBytes(iValue_3);//数值
            Byte[] arrayValue_4 = BitConverter.GetBytes(iValue_4);//数值
            Byte[] arrayValue_5 = BitConverter.GetBytes(iValue_5);//数值
            Byte[] arrayValue_6 = BitConverter.GetBytes(iValue_6);//数值

            Byte[] arrayInstructionData = new Byte[1 + 1 + arrayValue_1.Length + arrayValue_2.Length + arrayValue_3.Length + arrayValue_4.Length + arrayValue_5.Length + arrayValue_6.Length];//待发送数据

            arrayInstructionData[0] = (Byte)InstructionType;//填充待发送数据，指令标志位
            arrayInstructionData[1] = (Byte)Cameratype;//填充待发送数据，相机类型数据
            arrayValue_1.CopyTo(arrayInstructionData, 2);//填充待发送数据，工具索引
            iIndex = 2 + arrayValue_1.Length;
            arrayValue_2.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，图像类型（1，在线；2，学习；3，剔除
            iIndex += arrayValue_2.Length;
            arrayValue_3.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，工作区域横坐标
            iIndex += arrayValue_3.Length;
            arrayValue_4.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，工作区域纵坐标
            iIndex += arrayValue_4.Length;
            arrayValue_5.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，工作区域宽度
            iIndex += arrayValue_5.Length;
            arrayValue_6.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，工作区域高度

            //

            return arrayInstructionData;
        }

        //----------------------------------------------------------------------
        // 功能说明：生成指令
        // 输入参数：1.InstructionType：指令类型
        //         2.Cameratype：相机类型
        //         3.iValue_1：数值
        //         4.iValue_2：数值
        //         5.iValue_3：数值
        //         6.iValue_4：数值
        //         7.iValue_5：数值
        //         8.iValue_6：数值
        //         9.iValue_7：数值
        //         10.iValue_8：数值
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private Byte[] _GenerateInstruction(CommunicationInstructionType InstructionType, VisionSystemClassLibrary.Enum.CameraType Cameratype, Int32 iValue_1, Int32 iValue_2, Int32 iValue_3, Int32 iValue_4, Int32 iValue_5, Int32 iValue_6, Int32 iValue_7, Int32 iValue_8)
        {
            //指令类型 + 相机类型数据 + 光照时间 + 光照强度 + 增益 + 曝光时间 + 白平衡 + 白平衡（红） + 白平衡（绿） + 白平衡（蓝）

            Int32 iIndex = 0;//临时变量

            Byte[] arrayValue_1 = BitConverter.GetBytes(iValue_1);//数值
            Byte[] arrayValue_2 = BitConverter.GetBytes(iValue_2);//数值
            Byte[] arrayValue_3 = BitConverter.GetBytes(iValue_3);//数值
            Byte[] arrayValue_4 = BitConverter.GetBytes(iValue_4);//数值
            Byte[] arrayValue_5 = BitConverter.GetBytes(iValue_5);//数值
            Byte[] arrayValue_6 = BitConverter.GetBytes(iValue_6);//数值
            Byte[] arrayValue_7 = BitConverter.GetBytes(iValue_7);//数值
            Byte[] arrayValue_8 = BitConverter.GetBytes(iValue_8);//数值

            Byte[] arrayInstructionData = new Byte[1 + 1 + arrayValue_1.Length + arrayValue_2.Length + arrayValue_3.Length + arrayValue_4.Length + arrayValue_5.Length + arrayValue_6.Length + arrayValue_7.Length + arrayValue_8.Length];//待发送数据

            arrayInstructionData[0] = (Byte)InstructionType;//填充待发送数据，指令标志位
            arrayInstructionData[1] = (Byte)Cameratype;//填充待发送数据，相机类型数据
            arrayValue_1.CopyTo(arrayInstructionData, 2);//填充待发送数据，工具索引
            iIndex = 2 + arrayValue_1.Length;
            arrayValue_2.CopyTo(arrayInstructionData, iIndex);//填充待发送数据
            iIndex += arrayValue_2.Length;
            arrayValue_3.CopyTo(arrayInstructionData, iIndex);//填充待发送数据
            iIndex += arrayValue_3.Length;
            arrayValue_4.CopyTo(arrayInstructionData, iIndex);//填充待发送数据
            iIndex += arrayValue_4.Length;
            arrayValue_5.CopyTo(arrayInstructionData, iIndex);//填充待发送数据
            iIndex += arrayValue_5.Length;
            arrayValue_6.CopyTo(arrayInstructionData, iIndex);//填充待发送数据
            iIndex += arrayValue_6.Length;
            arrayValue_7.CopyTo(arrayInstructionData, iIndex);//填充待发送数据
            iIndex += arrayValue_7.Length;
            arrayValue_8.CopyTo(arrayInstructionData, iIndex);//填充待发送数据

            //

            return arrayInstructionData;
        }

        //----------------------------------------------------------------------
        // 功能说明：生成指令
        // 输入参数：1.InstructionType：指令类型
        //         2.Cameratype：相机类型
        //         3.SystemTime：数据
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private Byte[] _GenerateInstruction(CommunicationInstructionType InstructionType, VisionSystemClassLibrary.Enum.CameraType Cameratype, VisionSystemClassLibrary.Struct.SYSTEMTIME SystemTime)
        {
            //指令类型 + 相机类型数据 + 日期时间数据

            IFormatter formatter = new SoapFormatter();//格式化对象
            MemoryStream Memorystream = new MemoryStream();//流对象

            VisionSystemCommunicationLibrary.Ethernet.SerializableData ethernetData = new VisionSystemCommunicationLibrary.Ethernet.SerializableData();//待序列化数据

            ethernetData.Data_0 = new Byte[1];
            ethernetData.Data_0[0] = (Byte)Cameratype;//相机类型数据

            IFormatter formatter_AlignDateTime = new SoapFormatter();//格式化对象
            MemoryStream Memorystream_AlignDateTime = new MemoryStream();//流对象

            formatter_AlignDateTime.Serialize(Memorystream_AlignDateTime, SystemTime);//序列化

            ethernetData.Data_1 = Memorystream_AlignDateTime.ToArray();//日期时间数据

            formatter.Serialize(Memorystream, ethernetData);//序列化

            //

            Byte[] InstructionData_EthernetData = Memorystream.ToArray();//序列化数据
            Byte[] arrayEthernetDataLength = BitConverter.GetBytes(InstructionData_EthernetData.Length);//序列化数据长度

            Byte[] InstructionData = new Byte[1 + arrayEthernetDataLength.Length + InstructionData_EthernetData.Length];//待发送的数据

            InstructionData[0] = (Byte)InstructionType;//填充待发送数据，指令标志位
            arrayEthernetDataLength.CopyTo(InstructionData, 1);//填充待发送数据，序列化数据长度
            InstructionData_EthernetData.CopyTo(InstructionData, 1 + arrayEthernetDataLength.Length);//填充待发送数据，序列化数据

            //

            return InstructionData;
        }

        //----------------------------------------------------------------------
        // 功能说明：生成指令
        // 输入参数：1.InstructionType：指令类型
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private Byte[] _GenerateInstruction(CommunicationInstructionType InstructionType)
        {
            //指令类型

            Byte[] arrayInstructionData = new Byte[1];//待发送数据

            arrayInstructionData[0] = (Byte)InstructionType;//填充待发送数据，指令标志位

            //

            return arrayInstructionData;
        }

        //----------------------------------------------------------------------
        // 功能说明：生成指令
        // 输入参数：1.InstructionType：指令类型
        //         2.Cameratype：相机类型
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private Byte[] _GenerateInstruction(CommunicationInstructionType InstructionType, VisionSystemClassLibrary.Enum.CameraType Cameratype)
        {
            //指令类型 + 相机类型

            Byte[] arrayInstructionData = new Byte[2];//待发送数据

            arrayInstructionData[0] = (Byte)InstructionType;//填充待发送数据，指令标志位
            arrayInstructionData[1] = (Byte)Cameratype;//填充待发送数据，相机类型

            //

            return arrayInstructionData;
        }

        //----------------------------------------------------------------------
        // 功能说明：生成指令
        // 输入参数：1.InstructionType：指令类型
        //         2.Cameratype：相机类型
        //         3.bValue：数据
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private Byte[] _GenerateInstruction(CommunicationInstructionType InstructionType, VisionSystemClassLibrary.Enum.CameraType Cameratype, Boolean bValue)
        {
            //指令类型 + 相机类型 + 操作数据（1，打开；0，关闭）

            Byte[] arrayValue = BitConverter.GetBytes(Convert.ToInt32(bValue));//数据

            Byte[] arrayInstructionData = new Byte[1 + 1 + arrayValue.Length];//待发送数据

            arrayInstructionData[0] = (Byte)InstructionType;//填充待发送数据，指令标志位
            arrayInstructionData[1] = (Byte)Cameratype;//填充待发送数据，相机类型
            arrayValue.CopyTo(arrayInstructionData, 2);//填充待发送数据，数据

            //

            return arrayInstructionData;
        }

        //----------------------------------------------------------------------
        // 功能说明：生成指令
        // 输入参数：1.InstructionType：指令类型
        //         2.Cameratype：相机类型
        //         3.bViewToolGraphics：图像工具数据（true，包含工具；false，不包含工具）
        //         4.dImageScale：图像尺寸类型数据
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private Byte[] _GenerateInstruction(CommunicationInstructionType InstructionType, VisionSystemClassLibrary.Enum.CameraType Cameratype, Boolean bViewToolGraphics, Double dImageScale)
        {
            //指令类型 + 相机类型数据 + 图像工具数据（1，包含工具；0，不包含工具） + 图像尺寸类型数据

            IFormatter formatter = new SoapFormatter();//格式化对象
            MemoryStream Memorystream = new MemoryStream();//流对象

            VisionSystemCommunicationLibrary.Ethernet.SerializableData ethernetData = new VisionSystemCommunicationLibrary.Ethernet.SerializableData();//待序列化数据

            ethernetData.Data_0 = new Byte[1];
            ethernetData.Data_0[0] = (Byte)Cameratype;//相机类型数据

            ethernetData.Data_1 = BitConverter.GetBytes(Convert.ToInt32(bViewToolGraphics));//图像工具数据（1，包含工具；0，不包含工具）

            ethernetData.Data_2 = BitConverter.GetBytes(dImageScale);//图像尺寸类型数据
            
            formatter.Serialize(Memorystream, ethernetData);//序列化

            //

            Byte[] InstructionData_EthernetData = Memorystream.ToArray();//序列化数据
            Byte[] arrayEthernetDataLength = BitConverter.GetBytes(InstructionData_EthernetData.Length);//序列化数据长度

            Byte[] InstructionData = new Byte[1 + arrayEthernetDataLength.Length + InstructionData_EthernetData.Length];//待发送的数据

            InstructionData[0] = (Byte)InstructionType;//填充待发送数据，指令标志位
            arrayEthernetDataLength.CopyTo(InstructionData, 1);//填充待发送数据，序列化数据长度
            InstructionData_EthernetData.CopyTo(InstructionData, 1 + arrayEthernetDataLength.Length);//填充待发送数据，序列化数据

            //

            return InstructionData;
        }

        //----------------------------------------------------------------------
        // 功能说明：生成指令
        // 输入参数：1.InstructionType：指令类型
        //         2.Cameratype：相机类型
        //         3.bViewToolGraphics：图像工具数据（true，包含工具；false，不包含工具）
        //         4.dImageScale：图像尺寸类型数据
        //         5.iImageType：图像类型。取值范围：1，在线；2，学习；3，剔除
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private Byte[] _GenerateInstruction(CommunicationInstructionType InstructionType, VisionSystemClassLibrary.Enum.CameraType Cameratype, Boolean bViewToolGraphics, Double dImageScale, Int32 iImageType)
        {
            //指令类型 + 相机类型数据 + 图像工具数据（1，包含工具；0，不包含工具） + 图像尺寸类型数据 + 图像类型（1，在线；2，学习；3，剔除）

            IFormatter formatter = new SoapFormatter();//格式化对象
            MemoryStream Memorystream = new MemoryStream();//流对象

            VisionSystemCommunicationLibrary.Ethernet.SerializableData ethernetData = new VisionSystemCommunicationLibrary.Ethernet.SerializableData();//待序列化数据

            ethernetData.Data_0 = new Byte[1];
            ethernetData.Data_0[0] = (Byte)Cameratype;//相机类型数据

            ethernetData.Data_1 = BitConverter.GetBytes(Convert.ToInt32(bViewToolGraphics));//图像工具数据（1，包含工具；0，不包含工具）

            ethernetData.Data_2 = BitConverter.GetBytes(dImageScale);//图像尺寸类型数据

            ethernetData.Data_3 = BitConverter.GetBytes(iImageType);//图像类型。取值范围：1，在线；2，学习；3，剔除
            
            formatter.Serialize(Memorystream, ethernetData);//序列化

            //

            Byte[] InstructionData_EthernetData = Memorystream.ToArray();//序列化数据
            Byte[] arrayEthernetDataLength = BitConverter.GetBytes(InstructionData_EthernetData.Length);//序列化数据长度

            Byte[] InstructionData = new Byte[1 + arrayEthernetDataLength.Length + InstructionData_EthernetData.Length];//待发送的数据

            InstructionData[0] = (Byte)InstructionType;//填充待发送数据，指令标志位
            arrayEthernetDataLength.CopyTo(InstructionData, 1);//填充待发送数据，序列化数据长度
            InstructionData_EthernetData.CopyTo(InstructionData, 1 + arrayEthernetDataLength.Length);//填充待发送数据，序列化数据

            //

            return InstructionData;
        }

        //----------------------------------------------------------------------
        // 功能说明：解析指令
        // 输入参数：1.ServerData：指令数据
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private void _GetFileData(VisionSystemCommunicationLibrary.Ethernet.DataReceivedEventArgs ServerData)
        {
            //指令类型 + 相机类型数据 + 文件

            Thread thread = new Thread(_threadGetFileData);//加载线程
            thread.IsBackground = true;
            thread.Start((object)ServerData);//启动线程
        }

        //----------------------------------------------------------------------
        // 功能说明：加载线程
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _threadGetFileData(object objectServerData)
        {
            //指令类型 + 相机类型数据 + 文件

            VisionSystemCommunicationLibrary.Ethernet.DataReceivedEventArgs ServerData = (VisionSystemCommunicationLibrary.Ethernet.DataReceivedEventArgs)objectServerData;

            VisionSystemClassLibrary.Enum.CameraType Cameratype = (VisionSystemClassLibrary.Enum.CameraType)ServerData.ReceivedData[ServerData.DataInfo.InstructionIndex + 1];//相机类型
            Int32 iCameraIndex = _GetSelectedCameraIndex(Cameratype);//相机数据索引
            String sCameraName = Global.VisionSystem.Camera[iCameraIndex].CameraENGName;
            String sFilePath = Global.VisionSystem.ConfigDataPath + sCameraName + "\\" + VisionSystemClassLibrary.Class.Camera.ReceivedDataPathName;//文件目录

            Directory.CreateDirectory(sFilePath.Substring(0, sFilePath.Length - 1));//创建路径

            Int32 iValue = BitConverter.ToInt32(ServerData.ReceivedData, ServerData.DataInfo.InstructionIndex + 2);//发送结果（1，成功；0，失败）

            Byte[] byteFileName = new Byte[ServerData.DataInfo.FileNameLength];//文件名称（包含扩展名）
            Array.Copy(ServerData.ReceivedData, ServerData.DataInfo.FileNameIndex, byteFileName, 0, ServerData.DataInfo.FileNameLength);
            String sFileName = VisionSystemCommunicationLibrary.Ethernet.TestControlFunctions._ByteToString(byteFileName, 0, byteFileName.Length);//文件名称（包含扩展名）

            if (0 < ServerData.DataInfo.FileDataLength)//文件有效
            {
                FileStream filestream = new FileStream(sFilePath + sFileName, FileMode.Create, FileAccess.Write, FileShare.None, 8192, FileOptions.WriteThrough); //打开文件
                BinaryWriter binarywriter = new BinaryWriter(filestream);
                binarywriter.Write(ServerData.ReceivedData, ServerData.DataInfo.FileDataIndex, ServerData.DataInfo.FileDataLength);//写入文件

                binarywriter.Close();//关闭Brand文件
                filestream.Close();
            }
            else//文件无效
            {
                if (File.Exists(sFilePath + sFileName))//已存在
                {
                    File.Delete(sFilePath + sFileName);//删除
                }
            }

            //

            //服务端->客户端（数据）：指令类型 + 相机类型数据 + 文件传输状态（1，启动发送；2，文件发送中，文件索引值（从2开始））

            Byte[] Load_Data = new Byte[ServerData.DataInfo.InstructionLength];//生成指令数据
            Array.Copy(ServerData.ReceivedData, ServerData.DataInfo.InstructionIndex, Load_Data, 0, ServerData.DataInfo.InstructionLength);

            Int32 iCameraIndex_Load = _GetSelectedCameraIndex(Cameratype);//相机索引值
            Byte[] Load_ClientIP = IPAddress.Parse(Global.VisionSystem.Camera[iCameraIndex_Load].DeviceInformation.IP).GetAddressBytes();//获取当前选中的相机的IP地址

            Global.NetServer.ClientData[Load_ClientIP[3]]._Send(Load_Data);//发送数据
        }

        //----------------------------------------------------------------------
        // 功能说明：解析指令
        // 输入参数：1.ServerData：客户端发送的数据
        //         2.Cameratype：相机类型数据
        //         3.CameraChooseState：设置相机模式：单/双相机模式
        //         4.sBrandName：品牌名称
        //         5.iValue_1：文件索引值（从0开始）
        //         6.iValue_2：文件传输状态（1，文件发送中；2，文件发送完成）
        //         7.iValue_3：文件接收结果（1，成功；0，失败），配置结果（1，成功；0，失败）
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private void _GetInstructionData(VisionSystemCommunicationLibrary.Ethernet.DataReceivedEventArgs ServerData, ref VisionSystemClassLibrary.Enum.CameraType Cameratype, ref String sBrandName, ref Int32 iValue_1, ref Int32 iValue_2, ref Int32 iValue_3, ref Int32 CameraChooseState)
        {
            //未完成文件发送
            //指令类型 + 相机类型数据 + 设置相机模式 + 品牌名称长度 + 品牌名称 + 文件索引值（从0开始） + 文件传输状态（1，文件发送中；2，文件发送完成） + 文件接收结果（1，成功；0，失败）

            //完成文件发送
            //指令类型 + 相机类型数据 + 设置相机模式 + 品牌名称长度 + 品牌名称 + 文件索引值（从0开始） + 文件传输状态（1，文件发送中；2，文件发送完成） + 配置结果（1，成功；0，失败）

            Int32 iIndex = 0;//临时变量
            Int32 iValueLength = BitConverter.GetBytes((Int32)1).Length;//数据长度

            iIndex = ServerData.DataInfo.InstructionIndex + 1;
            Cameratype = (VisionSystemClassLibrary.Enum.CameraType)ServerData.ReceivedData[iIndex];//相机类型

            iIndex += 1;
            CameraChooseState = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//设置相机模式
            iIndex += iValueLength;
            Int32 iBrandNameLength = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//品牌名称长度
            iIndex += iValueLength;
            Byte[] byteBrandName = new Byte[iBrandNameLength];//品牌名称
            System.Array.Copy(ServerData.ReceivedData, iIndex, byteBrandName, 0, iBrandNameLength);
            sBrandName = VisionSystemCommunicationLibrary.Ethernet.TestControlFunctions._ByteToString(byteBrandName, 0, iBrandNameLength);//品牌名称
            iIndex += iBrandNameLength;
            iValue_1 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//文件索引值（从0开始）
            iIndex += iValueLength;
            iValue_2 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//文件传输状态（1，文件发送中；2，文件发送完成）
            iIndex += iValueLength;
            iValue_3 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//文件接收结果（1，成功；0，失败），配置结果（1，成功；0，失败）
        }

        //----------------------------------------------------------------------
        // 功能说明：解析指令
        // 输入参数：1.ServerData：客户端发送的数据
        //         2.Cameratype：相机类型数据
        //         3.iSelectedIPAddress：设置为的IP地址
        //         4.SelectedCameraType：设置为的相机类型数据
        //         5.byteSelectedPort：设置为的相机端口
        //         6.CameraChooseState：设置为的相机模式
        //         7.uCameraFaultState：设置为的相机故障标记
        //         8.bCheckEanble：设置为的相机检测使能
        //         9.bCameraAngle：设置为的相机旋转角度
        //         10.vVideoColor：相机颜色
        //         11.vVideoResolution：相机分辨率
        //         12.bSerialPort：是否为串口
        //         13.tTobaccoSortType：烟支排列类型
        //         14.bBitmapResize：设置为的相机数据截取区域缩放
        //         15.bBitmapCenter：设置为的相机数据截取区域缩放后是否居中
        //         16.pBitmapAxis：设置为的相机数据截取区域粘贴区域
        //         17.rBitmapArea：设置为的相机数据截取区域
        //         18.bCameraFlip：设置为的相机镜像标记
        //         19.sSensorProductType：设置为的传感器应用场景
        //          20.relevancyCameraInformation：相机关联信息
        //          21.iValue_1：文件索引值（从0开始）
        //          22.iValue_2：文件传输状态（1，文件发送中；2，文件发送完成）
        //          23.iValue_3：文件接收结果（1，成功；0，失败），配置结果（1，成功；0，失败）
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private void _GetInstructionData(VisionSystemCommunicationLibrary.Ethernet.DataReceivedEventArgs ServerData, ref VisionSystemClassLibrary.Enum.CameraType Cameratype, ref Int32 iSelectedIPAddress, ref VisionSystemClassLibrary.Enum.CameraType SelectedCameraType, ref Byte byteSelectedPort, ref Int32 CameraChooseState, ref UInt64 uCameraFaultState, ref Boolean bCheckEanble, ref VisionSystemClassLibrary.Enum.CameraRotateAngle bCameraAngle, ref VisionSystemClassLibrary.Enum.VideoColor vVideoColor, ref VisionSystemClassLibrary.Enum.VideoResolution vVideoResolution, ref Boolean bSerialPort, ref VisionSystemClassLibrary.Enum.TobaccoSortType tTobaccoSortType, ref Boolean bBitmapResize, ref Boolean bBitmapCenter, ref Point pBitmapAxis, ref Rectangle rBitmapArea, ref VisionSystemClassLibrary.Enum.CameraFlip bCameraFlip, ref VisionSystemClassLibrary.Enum.SensorProductType sSensorProductType, ref VisionSystemClassLibrary.Struct.RelevancyCameraInformation relevancyCameraInformation, ref Int32 iValue_1, ref Int32 iValue_2, ref Int32 iValue_3)
        {
            //未完成文件发送
            //指令类型 + 相机类型数据 + 设置为的IP地址 + 设置为的相机类型数据 + 设置为的端口 + 文件索引值（从0开始） + 文件传输状态（1，文件发送中；2，文件发送完成） + 文件接收结果（1，成功；0，失败）

            //完成文件发送
            //指令类型 + 相机类型数据 + 设置为的IP地址 + 设置为的相机类型数据 + 设置为的端口 + 文件索引值（从0开始） + 文件传输状态（1，文件发送中；2，文件发送完成） + 配置结果（1，成功；0，失败）

            Int32 iIndex = 0;//临时变量
            Int32 iValueLength = BitConverter.GetBytes((Int32)1).Length;//数据长度

            iIndex = ServerData.DataInfo.InstructionIndex + 1;
            Cameratype = (VisionSystemClassLibrary.Enum.CameraType)ServerData.ReceivedData[iIndex];//相机类型

            iIndex += 1;
            iSelectedIPAddress = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//设置为的IP地址
            iIndex += iValueLength;
            SelectedCameraType = (VisionSystemClassLibrary.Enum.CameraType)BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//设置为的相机类型数据
            iIndex += iValueLength;
            byteSelectedPort = (Byte)BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//设置为的相机端口
            iIndex += iValueLength;
            CameraChooseState = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//设置为的相机模式
            iIndex += iValueLength;
            uCameraFaultState = BitConverter.ToUInt64(ServerData.ReceivedData, iIndex);//设置为的相机故障标记
            iIndex += iValueLength * 2;

            if (BitConverter.ToInt32(ServerData.ReceivedData, iIndex) != 0) //设置为的相机检测使能
            {
                bCheckEanble = true;
            }
            else
            {
                bCheckEanble = false;
            }

            iIndex += iValueLength;
            bCameraAngle = (VisionSystemClassLibrary.Enum.CameraRotateAngle)BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//设置为的相机旋转角度

            iIndex += iValueLength;
            vVideoColor = (VisionSystemClassLibrary.Enum.VideoColor)BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//设置为的相机颜色

            iIndex += iValueLength;
            vVideoResolution = (VisionSystemClassLibrary.Enum.VideoResolution)BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//设置为的相机分辨率

            iIndex += iValueLength;
            if (BitConverter.ToInt32(ServerData.ReceivedData, iIndex) != 0) //设置为的串口标记
            {
                bSerialPort = true;
            }
            else
            {
                bSerialPort = false;
            }

            iIndex += iValueLength;
            tTobaccoSortType = (VisionSystemClassLibrary.Enum.TobaccoSortType)BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//设置为的烟支排列方式

            iIndex += iValueLength;
            if (BitConverter.ToInt32(ServerData.ReceivedData, iIndex) != 0) //设置为的相机数据截取区域缩放
            {
                bBitmapResize = true;
            }
            else
            {
                bBitmapResize = false;
            }

            iIndex += iValueLength;
            if (BitConverter.ToInt32(ServerData.ReceivedData, iIndex) != 0) //设置为的相机数据截取区域缩放后是否居中
            {
                bBitmapCenter = true;
            }
            else
            {
                bBitmapCenter = false;
            }

            iIndex += iValueLength;
            pBitmapAxis.X = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//设置为的相机数据截取区域粘贴区域X
            iIndex += iValueLength;
            pBitmapAxis.Y = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//设置为的相机数据截取区域粘贴区域Y
            iIndex += iValueLength;
            rBitmapArea.X = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//设置为的相机数据截取区域X
            iIndex += iValueLength;
            rBitmapArea.Y = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//设置为的相机数据截取区域Y
            iIndex += iValueLength;
            rBitmapArea.Width = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//设置为的相机数据截取区域W
            iIndex += iValueLength;
            rBitmapArea.Height = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//设置为的相机数据截取区域H
            iIndex += iValueLength;
            bCameraFlip = (VisionSystemClassLibrary.Enum.CameraFlip)BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//设置为的相机镜像标记
            iIndex += iValueLength;
            sSensorProductType = (VisionSystemClassLibrary.Enum.SensorProductType)BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//设置为的传感器应用场景
            iIndex += iValueLength;
            relevancyCameraInformation.rRelevancyType = (VisionSystemClassLibrary.Enum.RelevancyType)ServerData.ReceivedData[iIndex]; //相机关联类型

            if (VisionSystemClassLibrary.Enum.RelevancyType.None < relevancyCameraInformation.rRelevancyType) //相机存在关联信息
            {
                iIndex += 1;
                Byte iCount = ServerData.ReceivedData[iIndex];

                for (Int32 i = 0; i < iCount; i++)  //循环所有关联相机
                {
                    iIndex += 1;
                    VisionSystemClassLibrary.Enum.CameraType cameraType = (VisionSystemClassLibrary.Enum.CameraType)ServerData.ReceivedData[iIndex]; //关联相机类型
                    iIndex += 1;
                    Byte cameraPosition = ServerData.ReceivedData[iIndex]; //关联相机工位
                    relevancyCameraInformation.RelevancyCameraInfo.Add(cameraType, cameraPosition);
                }
            }

            iIndex += 1;
            iValue_1 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//文件索引值（从0开始）
            iIndex += iValueLength;
            iValue_2 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//文件传输状态（1，文件发送中；2，文件发送完成）
            iIndex += iValueLength;
            iValue_3 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//文件接收结果（1，成功；0，失败），配置结果（1，成功；0，失败）
        }

        //----------------------------------------------------------------------
        // 功能说明：解析指令
        // 输入参数：1.ServerData：客户端发送的数据
        //         2.Cameratype：相机类型数据
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private void _GetInstructionData(VisionSystemCommunicationLibrary.Ethernet.DataReceivedEventArgs ServerData, ref VisionSystemClassLibrary.Enum.CameraType Cameratype)
        {
            //指令类型 + 相机类型数据

            Cameratype = (VisionSystemClassLibrary.Enum.CameraType)ServerData.ReceivedData[ServerData.DataInfo.InstructionIndex + 1];//相机类型
        }

        //----------------------------------------------------------------------
        // 功能说明：解析指令
        // 输入参数：1.ServerData：客户端发送的数据
        //         2.iValue_1：数据
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private void _GetInstructionData(VisionSystemCommunicationLibrary.Ethernet.DataReceivedEventArgs ServerData, ref Int32 iValue_1)
        {
            //指令类型 + 数据（上位机，1；下位机，2）

            Int32 iIndex = 0;//临时变量
            Int32 iValueLength = BitConverter.GetBytes(iValue_1).Length;//数据长度

            iIndex = ServerData.DataInfo.InstructionIndex + 1;
            iValue_1 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//数据
        }

        //----------------------------------------------------------------------
        // 功能说明：解析指令
        // 输入参数：1.ServerData：客户端发送的数据
        //         2.Cameratype：相机类型数据
        //         3.iValue_1：数据
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private void _GetInstructionData(VisionSystemCommunicationLibrary.Ethernet.DataReceivedEventArgs ServerData, ref VisionSystemClassLibrary.Enum.CameraType Cameratype, ref Int32 iValue_1)
        {
            //指令类型 + 相机类型数据 + 工具开关设置结果（1，成功；0，失败）
            //指令类型 + 相机类型数据 + 复位结果（1，成功；0，失败）
            //指令类型 + 相机类型数据 + 文件传输状态（1，启动发送；2，文件发送中，文件索引值（从2开始））

            Int32 iIndex = 0;//临时变量
            Int32 iValueLength = BitConverter.GetBytes(iValue_1).Length;//数据长度

            iIndex = ServerData.DataInfo.InstructionIndex + 1;
            Cameratype = (VisionSystemClassLibrary.Enum.CameraType)ServerData.ReceivedData[iIndex];//相机类型

            iIndex += 1;
            iValue_1 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//数据
        }

        //----------------------------------------------------------------------
        // 功能说明：解析指令
        // 输入参数：1.ServerData：客户端发送的数据
        //         2.Cameratype：相机类型数据
        //         3.iValue_1：数据
        //         4.iValue_2：数据
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private void _GetInstructionData(VisionSystemCommunicationLibrary.Ethernet.DataReceivedEventArgs ServerData, ref VisionSystemClassLibrary.Enum.CameraType Cameratype, ref Int32 iValue_1, ref Int32 iValue_2)
        {
            //指令类型 + 相机类型数据 + 工具索引数值 + 最小值最大值设置结果（1，成功；0，失败）

            Int32 iIndex = 0;//临时变量
            Int32 iValueLength = BitConverter.GetBytes(iValue_1).Length;//数据长度

            iIndex = ServerData.DataInfo.InstructionIndex + 1;
            Cameratype = (VisionSystemClassLibrary.Enum.CameraType)ServerData.ReceivedData[iIndex];//相机类型

            iIndex += 1;
            iValue_1 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//数据
            iIndex += iValueLength;
            iValue_2 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//数据
        }

        //----------------------------------------------------------------------
        // 功能说明：解析指令
        // 输入参数：1.ServerData：客户端发送的数据
        //         2.Cameratype：相机类型数据
        //         3.iValue_1：数据
        //         4.iValue_2：数据
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private void _GetInstructionData(VisionSystemCommunicationLibrary.Ethernet.DataReceivedEventArgs ServerData, ref VisionSystemClassLibrary.Enum.CameraType Cameratype, ref UInt32 iValue_1, ref UInt32 iValue_2)
        {
            //指令类型 + 相机类型数据 + 输入数据 + 输出诊断

            Int32 iIndex = 0;//临时变量
            Int32 iValueLength = BitConverter.GetBytes(iValue_1).Length;//数据长度

            iIndex = ServerData.DataInfo.InstructionIndex + 1;
            Cameratype = (VisionSystemClassLibrary.Enum.CameraType)ServerData.ReceivedData[iIndex];//相机类型

            iIndex += 1;
            iValue_1 = BitConverter.ToUInt32(ServerData.ReceivedData, iIndex);//数据
            iIndex += iValueLength;
            iValue_2 = BitConverter.ToUInt32(ServerData.ReceivedData, iIndex);//数据
        }

        //----------------------------------------------------------------------
        // 功能说明：解析指令
        // 输入参数：1.ServerData：客户端发送的数据
        //         2.Cameratype：相机类型数据
        //         3.iValue_1：数据
        //         4.iValue_2：数据
        //         5.iValue_3：数据
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private void _GetInstructionData(VisionSystemCommunicationLibrary.Ethernet.DataReceivedEventArgs ServerData, ref VisionSystemClassLibrary.Enum.CameraType Cameratype, ref UInt32 iValue_1, ref UInt32 iValue_2, ref UInt32 iValue_3)
        {
            //指令类型 + 相机类型数据 + 已检测数量 + 合格数量 + 剔除数量

            Int32 iIndex = 0;//临时变量
            Int32 iValueLength = BitConverter.GetBytes(iValue_1).Length;//数据长度

            iIndex = ServerData.DataInfo.InstructionIndex + 1;
            Cameratype = (VisionSystemClassLibrary.Enum.CameraType)ServerData.ReceivedData[iIndex];//相机类型

            iIndex += 1;
            iValue_1 = BitConverter.ToUInt32(ServerData.ReceivedData, iIndex);//数据
            iIndex += iValueLength;
            iValue_2 = BitConverter.ToUInt32(ServerData.ReceivedData, iIndex);//数据
            iIndex += iValueLength;
            iValue_3 = BitConverter.ToUInt32(ServerData.ReceivedData, iIndex);//数据
        }

        //----------------------------------------------------------------------
        // 功能说明：解析指令
        // 输入参数：1.ServerData：客户端发送的数据
        //         2.Cameratype：相机类型数据
        //         3.iValue_1：数据
        //         4.iValue_2：数据
        //         5.iValue_3：数据
        //         6.iValue_4：数据
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private void _GetInstructionData(VisionSystemCommunicationLibrary.Ethernet.DataReceivedEventArgs ServerData, ref VisionSystemClassLibrary.Enum.CameraType Cameratype, ref Int32 iValue_1, ref UInt16 iValue_2, ref UInt16 iValue_3, ref UInt16 iValue_4)
        {
            //指令类型 + 相机类型数据 + 相机参数更新结果（1，成功；0，不成功） + 白平衡（红） + 白平衡（绿） + 白平衡（蓝）

            Int32 iIndex = 0;//临时变量
            Int32 iValueLength = BitConverter.GetBytes(iValue_1).Length;//数据长度

            iIndex = ServerData.DataInfo.InstructionIndex + 1;
            Cameratype = (VisionSystemClassLibrary.Enum.CameraType)ServerData.ReceivedData[iIndex];//相机类型

            iIndex += 1;
            iValue_1 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//数据
            iIndex += iValueLength;
            iValue_2 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//数据
            iIndex += iValueLength;
            iValue_3 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//数据
            iIndex += iValueLength;
            iValue_4 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//数据
            iIndex += iValueLength;
        }

        //----------------------------------------------------------------------
        // 功能说明：解析指令
        // 输入参数：1.ServerData：客户端发送的数据
        //           2.Cameratype：相机类型数据
        //           3.iValue_2：数据
        //           4.iValue_3：数据
        //           5.iData：数据
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private void _GetInstructionData(VisionSystemCommunicationLibrary.Ethernet.DataReceivedEventArgs ServerData, ref VisionSystemClassLibrary.Enum.CameraType Cameratype, ref Int32 iValue_2, ref Int32 iValue_3, ref Byte[] iData)
        {
            //客户端->服务端：指令类型 + 相机类型数据 + 烟支数量（N）+ 传感器校准过程标记（1，校准过程中；0，校准结束或未校准、取消校准） + 相机参数更新结果（1，成功；0，不成功） + 烟支校准值（N支）

            Int32 iIndex = 0;//临时变量
            Int32 iValueLength = BitConverter.GetBytes(iValue_2).Length;//数据长度

            iIndex = ServerData.DataInfo.InstructionIndex + 1;
            Cameratype = (VisionSystemClassLibrary.Enum.CameraType)ServerData.ReceivedData[iIndex];//相机类型

            iIndex += 1;
            Int32 iValue_1 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//数据
            iIndex += iValueLength;
            
            iValue_2 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//数据
            iIndex += iValueLength;

            iValue_3 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//数据
            iIndex += iValueLength;

            iData = new Byte[iValue_1];
            Array.Copy(ServerData.ReceivedData, iIndex, iData, 0, iValue_1);
        }

        //----------------------------------------------------------------------
        // 功能说明：解析指令
        // 输入参数：1.ServerData：客户端发送的数据
        //           2.Cameratype：相机类型数据
        //           3.iValue_2：数据
        //           4.iData：数据
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private void _GetInstructionData(VisionSystemCommunicationLibrary.Ethernet.DataReceivedEventArgs ServerData, ref VisionSystemClassLibrary.Enum.CameraType Cameratype, ref Int32 iValue_2, ref Int16[] iData)
        {
            //客户端->服务端：指令类型 + 相机类型数据 + 烟支数量（N）+ 最大电压查询标记（1，查询过程中；0，查询结束） + 最大电压值（N支）

            Int32 iIndex = 0;//临时变量
            Int32 iValueLength = BitConverter.GetBytes(iValue_2).Length;//数据长度

            iIndex = ServerData.DataInfo.InstructionIndex + 1;
            Cameratype = (VisionSystemClassLibrary.Enum.CameraType)ServerData.ReceivedData[iIndex];//相机类型

            iIndex += 1;
            Int32 iValue_1 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//数据
            iIndex += iValueLength;

            iValue_2 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//数据
            iIndex += iValueLength;

            iData = new Int16[iValue_1];
            for (Int32 i = 0; i < iValue_1; i++) //循环所有烟支
            {
                iData[i] = BitConverter.ToInt16(ServerData.ReceivedData, iIndex);

                iIndex += 2;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：解析指令
        // 输入参数：1.ServerData：客户端发送的数据
        //         2.Cameratype：相机类型数据
        //         3.iValue_1：数据
        //         4.iValue_2：数据
        //         5.iValue_3：数据
        //         6.iValue_4：数据
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private void _GetInstructionData(VisionSystemCommunicationLibrary.Ethernet.DataReceivedEventArgs ServerData, ref VisionSystemClassLibrary.Enum.CameraType Cameratype, ref Int32 iValue_1, ref Int32 iValue_2, ref Int32 iValue_3, ref Int32 iValue_4)
        {
            //指令类型 + 相机类型数据 + 工具索引数值 + 学习数值 + 学习中的有效数值数量 + 学习中的无效数值数量

            Int32 iIndex = 0;//临时变量
            Int32 iValueLength = BitConverter.GetBytes(iValue_1).Length;//数据长度

            iIndex = ServerData.DataInfo.InstructionIndex + 1;
            Cameratype = (VisionSystemClassLibrary.Enum.CameraType)ServerData.ReceivedData[iIndex];//相机类型

            iIndex += 1;
            iValue_1 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//数据
            iIndex += iValueLength;
            iValue_2 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//数据
            iIndex += iValueLength;
            iValue_3 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//数据
            iIndex += iValueLength;
            iValue_4 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//数据
            iIndex += iValueLength;
        }

        //----------------------------------------------------------------------
        // 功能说明：解析指令
        // 输入参数：1.ServerData：客户端发送的数据
        //         2.Cameratype：相机类型数据
        //         3.Tolerancesdata：公差数据
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private void _GetInstructionData(VisionSystemCommunicationLibrary.Ethernet.DataReceivedEventArgs ServerData, ref VisionSystemClassLibrary.Enum.CameraType Cameratype, ref VisionSystemClassLibrary.Struct.TolerancesGraphData_Value[] TolerancesGraphDataValue)
        {
            //指令类型 + 相机类型 + 公差类数据

            Int32 iIndex = 0;//临时变量

            MemoryStream Memorystream = new MemoryStream();//流对象

            iIndex = ServerData.DataInfo.InstructionIndex + 1;
            Cameratype = (VisionSystemClassLibrary.Enum.CameraType)ServerData.ReceivedData[iIndex];//相机类型

            iIndex += 1;
            Int32 TolerancesGraphDataValueLength = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//序列化数据长度
            iIndex += BitConverter.GetBytes(TolerancesGraphDataValueLength).Length;
            Memorystream.Write(ServerData.ReceivedData, iIndex, TolerancesGraphDataValueLength);//写入流

            IFormatter formatter = new SoapFormatter();//格式化对象
            Memorystream.Position = 0;//初始化流对象
            TolerancesGraphDataValue = (VisionSystemClassLibrary.Struct.TolerancesGraphData_Value[])formatter.Deserialize(Memorystream);//反序列化
        }

        //----------------------------------------------------------------------
        // 功能说明：解析指令
        // 输入参数：1.ServerData：客户端发送的数据
        //         2.Cameratype：相机类型数据
        //         3.imageInformation：图像信息数据
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private void _GetInstructionData(VisionSystemCommunicationLibrary.Ethernet.DataReceivedEventArgs ServerData, ref VisionSystemClassLibrary.Enum.CameraType Cameratype, ref Int32 iValue_1, ref VisionSystemClassLibrary.Struct.ImageInformation imageInformation)
        {
            //指令类型 + 相机类型数据 + 启用工具标记更新结果（1，成功；0，不成功） + 图像信息数据 + 图像处理信息（绘图）
            //指令类型 + 相机类型数据 + 工具参数更新结果（1，成功；0，不成功） + 图像信息数据 + 图像处理信息（绘图）
            //指令类型 + 相机类型数据 + 当前工具设置结果（1，成功；0，不成功） + 图像信息数据 + 图像处理信息（绘图）
            //指令类型 + 相机类型数据 + 设置结果（1，成功；0，不成功） + 图像信息数据 + 图像处理信息（绘图）

            Int32 iIndex = 0;//临时变量
            Int32 iValueLength = BitConverter.GetBytes(iValue_1).Length;//数据长度

            iIndex = ServerData.DataInfo.InstructionIndex + 1;
            Cameratype = (VisionSystemClassLibrary.Enum.CameraType)ServerData.ReceivedData[iIndex];//相机类型

            iIndex += 1;
            iValue_1 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//数据
            iIndex += iValueLength;

            //序列化数据1

            MemoryStream Memorystream_1 = new MemoryStream();//流对象

            Int32 DrawingInfoLength_1 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//序列化数据长度
            iIndex += BitConverter.GetBytes(DrawingInfoLength_1).Length;
            Memorystream_1.Write(ServerData.ReceivedData, iIndex, DrawingInfoLength_1);//写入流
            iIndex += DrawingInfoLength_1;

            IFormatter formatter_1 = new SoapFormatter();//格式化对象
            Memorystream_1.Position = 0;//初始化流对象
            imageInformation = (VisionSystemClassLibrary.Struct.ImageInformation)formatter_1.Deserialize(Memorystream_1);//反序列化
        }

        //----------------------------------------------------------------------
        // 功能说明：解析指令
        // 输入参数：1.ServerData：客户端发送的数据
        //         2.Cameratype：相机类型
        //         3.bViewToolGraphics：相机类型
        //         4.dImageScale：相机类型
        //         5.iImageDataLength：图像数据长度
        //         6.GraphicsInformation：图像信息
        //         7.memoryStream：图像数据
        //         8.iImageWidth：图像宽度
        //         9.iImageHeight：图像高度
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private void _GetInstructionData(VisionSystemCommunicationLibrary.Ethernet.DataReceivedEventArgs ServerData, ref VisionSystemClassLibrary.Enum.CameraType Cameratype, ref Boolean bViewToolGraphics, ref Double dImageScale, ref Int32 iImageDataLength, ref VisionSystemClassLibrary.Struct.ImageInformation GraphicsInformation, ref MemoryStream memoryStream, ref Int32 iImageWidth, ref Int32 iImageHeight)
        {
            //指令类型 + 相机类型数据 + 图像工具数据（1，包含工具；0，不包含工具） + 图像尺寸类型数据 + 图像宽度数据 + 图像高度数据 + 图像信息数据 + 图像数据

            Int32 iIndex = 0;//临时变量

            iIndex = ServerData.DataInfo.InstructionIndex + 1;

            //序列化数据1

            MemoryStream Memorystream_1 = new MemoryStream();//流对象
            Int32 iSerializableDataLength_1 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//序列化数据长度
            iIndex += BitConverter.GetBytes(iSerializableDataLength_1).Length;
            Memorystream_1.Write(ServerData.ReceivedData, iIndex, iSerializableDataLength_1);//写入流
            iIndex += iSerializableDataLength_1;

            IFormatter formatter_1 = new SoapFormatter();//格式化对象
            Memorystream_1.Position = 0;//初始化流对象
            VisionSystemCommunicationLibrary.Ethernet.SerializableData serializableData_1 = (VisionSystemCommunicationLibrary.Ethernet.SerializableData)formatter_1.Deserialize(Memorystream_1);//反序列化

            Cameratype = (VisionSystemClassLibrary.Enum.CameraType)serializableData_1.Data_0[0];//相机类型数据

            bViewToolGraphics = BitConverter.ToBoolean(serializableData_1.Data_1, 0);//图像工具数据（1，包含工具；0，不包含工具）

            dImageScale = BitConverter.ToDouble(serializableData_1.Data_2, 0);//图像尺寸类型数据

            iImageWidth = BitConverter.ToInt32(serializableData_1.Data_3, 0);//图像宽度数据

            iImageHeight = BitConverter.ToInt32(serializableData_1.Data_4, 0);//图像高度数据

            //序列化数据2

            MemoryStream Memorystream_2 = new MemoryStream();//流对象
            Int32 iSerializableDataLength_2 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//序列化数据长度
            iIndex += BitConverter.GetBytes(iSerializableDataLength_2).Length;
            Memorystream_2.Write(ServerData.ReceivedData, iIndex, iSerializableDataLength_2);//写入流
            iIndex += iSerializableDataLength_2;

            IFormatter formatter_2 = new SoapFormatter();//格式化对象
            Memorystream_2.Position = 0;//初始化流对象
            GraphicsInformation = (VisionSystemClassLibrary.Struct.ImageInformation)formatter_2.Deserialize(Memorystream_2);//反序列化

            //

            Int32 iValueLength = BitConverter.GetBytes(iImageDataLength).Length;//数据长度

            iImageDataLength = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//数据
            iIndex += iValueLength;

            if (0 < iImageDataLength)//图像有效
            {
                memoryStream.Write(ServerData.ReceivedData, iIndex, iImageDataLength);
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：解析指令
        // 输入参数：1.ServerData：客户端发送的数据
        //         2.Cameratype：相机类型
        //         3.bViewToolGraphics：相机类型
        //         4.dImageScale：相机类型
        //         8.iImageDataLength：图像数据长度
        //         9.GraphicsInformation：图像信息
        //         10.imageData：图像数据
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private void _GetInstructionData(VisionSystemCommunicationLibrary.Ethernet.DataReceivedEventArgs ServerData, ref VisionSystemClassLibrary.Enum.CameraType Cameratype, ref Boolean bViewToolGraphics, ref Double dImageScale, ref Int32 iImageDataLength, ref VisionSystemClassLibrary.Struct.ImageInformation GraphicsInformation, ref Image<Bgr, Byte> imageData)
        {
            //指令类型 + 相机类型数据 + 图像工具数据（1，包含工具；0，不包含工具） + 图像尺寸类型数据 + 图像宽度数据 + 图像高度数据 + 图像信息数据 + 图像数据

            Int32 iIndex = 0;//临时变量

            iIndex = ServerData.DataInfo.InstructionIndex + 1;

            //序列化数据1

            MemoryStream Memorystream_1 = new MemoryStream();//流对象
            Int32 iSerializableDataLength_1 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//序列化数据长度
            iIndex += BitConverter.GetBytes(iSerializableDataLength_1).Length;
            Memorystream_1.Write(ServerData.ReceivedData, iIndex, iSerializableDataLength_1);//写入流
            iIndex += iSerializableDataLength_1;

            IFormatter formatter_1 = new SoapFormatter();//格式化对象
            Memorystream_1.Position = 0;//初始化流对象
            VisionSystemCommunicationLibrary.Ethernet.SerializableData serializableData_1 = (VisionSystemCommunicationLibrary.Ethernet.SerializableData)formatter_1.Deserialize(Memorystream_1);//反序列化

            Cameratype = (VisionSystemClassLibrary.Enum.CameraType)serializableData_1.Data_0[0];//相机类型数据

            bViewToolGraphics = BitConverter.ToBoolean(serializableData_1.Data_1, 0);//图像工具数据（1，包含工具；0，不包含工具）

            dImageScale = BitConverter.ToDouble(serializableData_1.Data_2, 0);//图像尺寸类型数据

            Int32 ImageWidth = BitConverter.ToInt32(serializableData_1.Data_3, 0);//图像宽度数据

            Int32 ImageHeight = BitConverter.ToInt32(serializableData_1.Data_4, 0);//图像高度数据

            //序列化数据2

            MemoryStream Memorystream_2 = new MemoryStream();//流对象
            Int32 iSerializableDataLength_2 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//序列化数据长度
            iIndex += BitConverter.GetBytes(iSerializableDataLength_2).Length;
            Memorystream_2.Write(ServerData.ReceivedData, iIndex, iSerializableDataLength_2);//写入流
            iIndex += iSerializableDataLength_2;

            IFormatter formatter_2 = new SoapFormatter();//格式化对象
            Memorystream_2.Position = 0;//初始化流对象
            GraphicsInformation = (VisionSystemClassLibrary.Struct.ImageInformation)formatter_2.Deserialize(Memorystream_2);//反序列化

            //

            Int32 iValueLength = BitConverter.GetBytes(iImageDataLength).Length;//数据长度

            iImageDataLength = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//数据
            iIndex += iValueLength;

            if (0 < iImageDataLength)//图像有效
            {
                Image<Bgr, Byte> imageData_Temp = new Image<Bgr, Byte>(ImageWidth, ImageHeight);//获取的图像数据

                byte[] byteImage = new byte[iImageDataLength];

                System.Array.Copy(ServerData.ReceivedData, iIndex, byteImage, 0, iImageDataLength);

                imageData_Temp.Bytes = byteImage;

                imageData = imageData_Temp.Copy();

                imageData_Temp.Dispose();
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：生成指令
        // 输入参数：1.InstructionType：指令类型
        //         2.Cameratype：相机类型
        //         3.bRelevancy：关联标记
        //         4.iStatisticsDataType：统计数据类型（0，最新统计数据；1，指定统计数据）
        //         5.iShiftIndex：班次索引（从0开始）
        //         6.shifttime：统计数据开始结束时间
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private Byte[] _GenerateInstruction(CommunicationInstructionType InstructionType, VisionSystemClassLibrary.Enum.CameraType Cameratype, Boolean bRelevancy, Int32 iStatisticsDataType, Int32 iShiftIndex, VisionSystemClassLibrary.Struct.ShiftTime shifttime)
        {
            //指令类型 + 相机类型数据 + 关联标记 + 统计数据类型（0，最新统计数据；1，指定统计数据） + 班次索引（从0开始） + 统计数据开始结束时间

            MemoryStream memorystream = new MemoryStream();//可扩展容量数据流

            VisionSystemClassLibrary.Struct.SYSTEMTIME systemtime = new VisionSystemClassLibrary.Struct.SYSTEMTIME();//临时变量
            Int32 iValue = BitConverter.GetBytes(systemtime.Year).Length;//临时变量

            Byte[] byteValue_1 = new Byte[1];//填充待发送数据，指令标志位
            byteValue_1[0] = (Byte)InstructionType;//填充待发送数据，指令标志位
            memorystream.Write(byteValue_1, 0, byteValue_1.Length);//追加写入到可扩容数据流中

            Byte[] byteValue_2 = new Byte[1];//填充待发送数据，相机类型
            byteValue_2[0] = (Byte)Cameratype;//填充待发送数据，相机类型
            memorystream.Write(byteValue_2, 0, byteValue_2.Length);//追加写入到可扩容数据流中

            Byte[] byteValue_22;//关联信息
            if (bRelevancy) //查询关联信息
            {
                byteValue_22 = BitConverter.GetBytes(1);
            }
            else //非关联信息
            {
                byteValue_22 = BitConverter.GetBytes(0);
            }

            memorystream.Write(byteValue_22, 0, byteValue_22.Length);//追加写入到可扩容数据流中

            Byte[] byteValue_3 = BitConverter.GetBytes(iStatisticsDataType);//统计数据类型（0，最新统计数据；1，指定统计数据）
            memorystream.Write(byteValue_3, 0, byteValue_3.Length);//追加写入到可扩容数据流中

            Byte[] byteValue_4 = BitConverter.GetBytes(iShiftIndex);//班次索引（从0开始）
            memorystream.Write(byteValue_4, 0, byteValue_4.Length);//追加写入到可扩容数据流中

            Byte[] byteValue_5 = new Byte[iValue * 8 * 2];//统计数据开始结束时间
            //
            Byte[] byteValue_Temp_1 = BitConverter.GetBytes(shifttime.Start.Year);//统计数据开始时间，年
            byteValue_Temp_1.CopyTo(byteValue_5, 0);//统计数据开始时间，年
            byteValue_Temp_1 = BitConverter.GetBytes(shifttime.Start.Month);//统计数据开始时间，月
            byteValue_Temp_1.CopyTo(byteValue_5, iValue);//统计数据开始时间，月
            byteValue_Temp_1 = BitConverter.GetBytes(shifttime.Start.DayOfWeek);//统计数据开始时间，星期
            byteValue_Temp_1.CopyTo(byteValue_5, iValue * 2);//统计数据开始时间，星期
            byteValue_Temp_1 = BitConverter.GetBytes(shifttime.Start.Day);//统计数据开始时间，日
            byteValue_Temp_1.CopyTo(byteValue_5, iValue * 3);//统计数据开始时间，日
            byteValue_Temp_1 = BitConverter.GetBytes(shifttime.Start.Hour);//统计数据开始时间，时
            byteValue_Temp_1.CopyTo(byteValue_5, iValue * 4);//统计数据始时间，时
            byteValue_Temp_1 = BitConverter.GetBytes(shifttime.Start.Minute);//统计数据开始时间，分
            byteValue_Temp_1.CopyTo(byteValue_5, iValue * 5);//统计数据开始时间，分
            byteValue_Temp_1 = BitConverter.GetBytes(shifttime.Start.Second);//统计数据开始时间，秒
            byteValue_Temp_1.CopyTo(byteValue_5, iValue * 6);//统计数据开始时间，秒
            byteValue_Temp_1 = BitConverter.GetBytes(shifttime.Start.MilliSeconds);//统计数据开始时间，毫秒
            byteValue_Temp_1.CopyTo(byteValue_5, iValue * 7);//统计数据开始时间，毫秒
            //
            byteValue_Temp_1 = BitConverter.GetBytes(shifttime.End.Year);//统计数据结束时间，年
            byteValue_Temp_1.CopyTo(byteValue_5, iValue * 8);//统计数据结束时间，年
            byteValue_Temp_1 = BitConverter.GetBytes(shifttime.End.Month);//统计数据结束时间，月
            byteValue_Temp_1.CopyTo(byteValue_5, iValue * 9);//统计数据结束时间，月
            byteValue_Temp_1 = BitConverter.GetBytes(shifttime.End.DayOfWeek);//统计数据结束时间，星期
            byteValue_Temp_1.CopyTo(byteValue_5, iValue * 10);//统计数据结束时间，星期
            byteValue_Temp_1 = BitConverter.GetBytes(shifttime.End.Day);//统计数据结束时间，日
            byteValue_Temp_1.CopyTo(byteValue_5, iValue * 11);//统计数据结束时间，日
            byteValue_Temp_1 = BitConverter.GetBytes(shifttime.End.Hour);//统计数据结束时间，时
            byteValue_Temp_1.CopyTo(byteValue_5, iValue * 12);//统计数据结束时间，时
            byteValue_Temp_1 = BitConverter.GetBytes(shifttime.End.Minute);//统计数据结束时间，分
            byteValue_Temp_1.CopyTo(byteValue_5, iValue * 13);//统计数据结束时间，分
            byteValue_Temp_1 = BitConverter.GetBytes(shifttime.End.Second);//统计数据结束时间，秒
            byteValue_Temp_1.CopyTo(byteValue_5, iValue * 14);//统计数据结束时间，秒
            byteValue_Temp_1 = BitConverter.GetBytes(shifttime.End.MilliSeconds);//统计数据结束时间，毫秒
            byteValue_Temp_1.CopyTo(byteValue_5, iValue * 15);//统计数据结束时间，毫秒
            //
            memorystream.Write(byteValue_5, 0, byteValue_5.Length);//追加写入到可扩容数据流中

            Byte[] arrayInstructionData = memorystream.ToArray();//待发送数据

            memorystream.Close();

            return arrayInstructionData;
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：解析指令
        // 输入参数：1.ServerData：客户端发送的数据
        //         2.Cameratype：相机类型数据
        //         3.bRelevancy：相机关联标记
        //         4.iStatisticsDataType：统计数据类型（0，最新统计数据；1，指定统计数据）
        //         5.iShiftIndex：班次索引（从0开始）
        //         6.shifttime：统计数据开始结束时间
        //         7.iRejectNumber：剔除统计信息
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private void _GetInstructionData(VisionSystemCommunicationLibrary.Ethernet.DataReceivedEventArgs ServerData, ref VisionSystemClassLibrary.Enum.CameraType Cameratype, ref Boolean bRelevancy, ref Int32 iStatisticsDataType, ref Int32 iShiftIndex,  ref VisionSystemClassLibrary.Struct.ShiftTime sShiftTime, ref Int32 iRejectNumber)
        {
            //指令类型 + 相机类型数据 + 关联标记 + 统计数据类型（0，当前班；1，历史班） + 班次索引（非0） + 统计数据开始结束时间 + 剔除数量

            Int32 iIndex = 0;//临时变量
            Int32 iValueLength_1 = BitConverter.GetBytes((Int32)1).Length;//数据长度

            VisionSystemClassLibrary.Struct.SYSTEMTIME systemtime = new VisionSystemClassLibrary.Struct.SYSTEMTIME();//临时变量
            Int32 iValueLength_2 = BitConverter.GetBytes(systemtime.Year).Length;//数据长度

            Int32 iValueLength_3 = BitConverter.GetBytes((UInt32)1).Length;//数据长度

            //

            iIndex = ServerData.DataInfo.InstructionIndex + 1;
            Cameratype = (VisionSystemClassLibrary.Enum.CameraType)ServerData.ReceivedData[iIndex];//相机类型
            iIndex += 1;

            Int32 iValue_0 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//相机关联标记
            if (0 != iValue_0) //相机关联
            {
                bRelevancy = true;
            }
            else
            {
                bRelevancy = false;
            }
            iIndex += iValueLength_1;

            Int32 iValue_1 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//统计数据类型（0，当前班；1，历史班）
            iStatisticsDataType = iValue_1;
            iIndex += iValueLength_1;

            Int32 iValue_2 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//班次索引（从0开始）
            iShiftIndex = iValue_2 - 1;
            iIndex += iValueLength_1;

            UInt16 iValue_3_1_1 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据开始时间，年
            sShiftTime.Start.Year = iValue_3_1_1;
            iIndex += iValueLength_2;
            UInt16 iValue_3_1_2 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据开始时间，月
            sShiftTime.Start.Month = iValue_3_1_2;
            iIndex += iValueLength_2;
            UInt16 iValue_3_1_3 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据开始时间，星期
            sShiftTime.Start.DayOfWeek = iValue_3_1_3;
            iIndex += iValueLength_2;
            UInt16 iValue_3_1_4 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据开始时间，日
            sShiftTime.Start.Day = iValue_3_1_4;
            iIndex += iValueLength_2;
            UInt16 iValue_3_1_5 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据开始时间，时
            sShiftTime.Start.Hour = iValue_3_1_5;
            iIndex += iValueLength_2;
            UInt16 iValue_3_1_6 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据开始时间，分
            sShiftTime.Start.Minute = iValue_3_1_6;
            iIndex += iValueLength_2;
            UInt16 iValue_3_1_7 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据开始时间，秒
            sShiftTime.Start.Second = iValue_3_1_7;
            iIndex += iValueLength_2;
            UInt16 iValue_3_1_8 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据开始时间，毫秒
            sShiftTime.Start.MilliSeconds = iValue_3_1_8;
            iIndex += iValueLength_2;
            //
            UInt16 iValue_3_2_1 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据结束时间，年
            sShiftTime.End.Year = iValue_3_2_1;
            iIndex += iValueLength_2;
            UInt16 iValue_3_2_2 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据结束时间，月
            sShiftTime.End.Month = iValue_3_2_2;
            iIndex += iValueLength_2;
            UInt16 iValue_3_2_3 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据结束时间，星期
            sShiftTime.End.DayOfWeek = iValue_3_2_3;
            iIndex += iValueLength_2;
            UInt16 iValue_3_2_4 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据结束时间，日
            sShiftTime.End.Day = iValue_3_2_4;
            iIndex += iValueLength_2;
            UInt16 iValue_3_2_5 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据结束时间，时
            sShiftTime.End.Hour = iValue_3_2_5;
            iIndex += iValueLength_2;
            UInt16 iValue_3_2_6 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据结束时间，分
            sShiftTime.End.Minute = iValue_3_2_6;
            iIndex += iValueLength_2;
            UInt16 iValue_3_2_7 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据结束时间，秒
            sShiftTime.End.Second = iValue_3_2_7;
            iIndex += iValueLength_2;
            UInt16 iValue_3_2_8 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据结束时间，毫秒
            sShiftTime.End.MilliSeconds = iValue_3_2_8;
            iIndex += iValueLength_2;
            
            iRejectNumber =BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//剔除数量
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：解析指令
        // 输入参数：1.ServerData：客户端发送的数据
        //         2.Cameratype：相机类型数据
        //         3.bRelevancy：相机关联标记
        //         4.iStatisticsDataType：统计数据类型（0，最新统计数据；1，指定统计数据）
        //         5.iShiftIndex：班次索引（从0开始）
        //         6.shifttime：统计数据开始结束时间
        //         7.statisticsinformation：统计信息
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private void _GetInstructionData(VisionSystemCommunicationLibrary.Ethernet.DataReceivedEventArgs ServerData, ref VisionSystemClassLibrary.Enum.CameraType Cameratype, ref Boolean bRelevancy, ref Int32 iStatisticsDataType, ref Int32 iShiftIndex, ref VisionSystemClassLibrary.Struct.StatisticsInformation statisticsinformation)
        {
            //指令类型 + 相机类型数据 + 关联标记 + 统计数据类型（0，当前班；1，历史班） + 班次索引（非0） + 统计数据开始结束时间 + 统计数据（品牌名称（包括品牌长度） + 已检测数量 + 合格数量 + 剔除数量 + 工具数量 + 工具统计信息）

            Int32 iIndex = 0;//临时变量
            Int32 iValueLength_1 = BitConverter.GetBytes((Int32)1).Length;//数据长度

            VisionSystemClassLibrary.Struct.SYSTEMTIME systemtime = new VisionSystemClassLibrary.Struct.SYSTEMTIME();//临时变量
            Int32 iValueLength_2 = BitConverter.GetBytes(systemtime.Year).Length;//数据长度

            Int32 iValueLength_3 = BitConverter.GetBytes((UInt32)1).Length;//数据长度

            //

            iIndex = ServerData.DataInfo.InstructionIndex + 1;
            Cameratype = (VisionSystemClassLibrary.Enum.CameraType)ServerData.ReceivedData[iIndex];//相机类型
            statisticsinformation.DataOfStatistics[0].CameraStatisticsData[0].TypeOfCamera = Cameratype;
            iIndex += 1;

            Int32 iValue_0 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//相机关联标记
            if (0 != iValue_0) //相机关联
            {
                bRelevancy = true;
            } 
            else
            {
                bRelevancy = false;
            }
            iIndex += iValueLength_1;

            Int32 iValue_1 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//统计数据类型（0，当前班；1，历史班）
            iStatisticsDataType = iValue_1;
            iIndex += iValueLength_1;

            Int32 iValue_2 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//班次索引（从0开始）
            iShiftIndex = iValue_2 - 1;
            iIndex += iValueLength_1;

            UInt16 iValue_3_1_1 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据开始时间，年
            statisticsinformation.TimeData[0].Start.Year = iValue_3_1_1;
            iIndex += iValueLength_2;
            UInt16 iValue_3_1_2 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据开始时间，月
            statisticsinformation.TimeData[0].Start.Month = iValue_3_1_2;
            iIndex += iValueLength_2;
            UInt16 iValue_3_1_3 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据开始时间，星期
            statisticsinformation.TimeData[0].Start.DayOfWeek = iValue_3_1_3;
            iIndex += iValueLength_2;
            UInt16 iValue_3_1_4 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据开始时间，日
            statisticsinformation.TimeData[0].Start.Day = iValue_3_1_4;
            iIndex += iValueLength_2;
            UInt16 iValue_3_1_5 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据开始时间，时
            statisticsinformation.TimeData[0].Start.Hour = iValue_3_1_5;
            iIndex += iValueLength_2;
            UInt16 iValue_3_1_6 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据开始时间，分
            statisticsinformation.TimeData[0].Start.Minute = iValue_3_1_6;
            iIndex += iValueLength_2;
            UInt16 iValue_3_1_7 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据开始时间，秒
            statisticsinformation.TimeData[0].Start.Second = iValue_3_1_7;
            iIndex += iValueLength_2;
            UInt16 iValue_3_1_8 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据开始时间，毫秒
            statisticsinformation.TimeData[0].Start.MilliSeconds = iValue_3_1_8;
            iIndex += iValueLength_2;
            //
            UInt16 iValue_3_2_1 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据结束时间，年
            statisticsinformation.TimeData[0].End.Year = iValue_3_2_1;
            iIndex += iValueLength_2;
            UInt16 iValue_3_2_2 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据结束时间，月
            statisticsinformation.TimeData[0].End.Month = iValue_3_2_2;
            iIndex += iValueLength_2;
            UInt16 iValue_3_2_3 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据结束时间，星期
            statisticsinformation.TimeData[0].End.DayOfWeek = iValue_3_2_3;
            iIndex += iValueLength_2;
            UInt16 iValue_3_2_4 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据结束时间，日
            statisticsinformation.TimeData[0].End.Day = iValue_3_2_4;
            iIndex += iValueLength_2;
            UInt16 iValue_3_2_5 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据结束时间，时
            statisticsinformation.TimeData[0].End.Hour = iValue_3_2_5;
            iIndex += iValueLength_2;
            UInt16 iValue_3_2_6 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据结束时间，分
            statisticsinformation.TimeData[0].End.Minute = iValue_3_2_6;
            iIndex += iValueLength_2;
            UInt16 iValue_3_2_7 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据结束时间，秒
            statisticsinformation.TimeData[0].End.Second = iValue_3_2_7;
            iIndex += iValueLength_2;
            UInt16 iValue_3_2_8 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据结束时间，毫秒
            statisticsinformation.TimeData[0].End.MilliSeconds = iValue_3_2_8;
            iIndex += iValueLength_2;
            
            statisticsinformation.DataOfStatistics[0].BrandName = "";
            Int32 iValue_4_2 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//统计数据（品牌名称（包括品牌长度））
            iIndex += iValueLength_1;
            statisticsinformation.DataOfStatistics[0].BrandName = VisionSystemCommunicationLibrary.Ethernet.TestControlFunctions._ByteToString(ServerData.ReceivedData, iIndex, iValue_4_2);//统计数据（品牌名称（包括品牌长度））
            iIndex += iValue_4_2;

            UInt32 iValue_4_3 = BitConverter.ToUInt32(ServerData.ReceivedData, iIndex);//统计数据（已检测数量）
            statisticsinformation.DataOfStatistics[0].CameraStatisticsData[0].InspectedNumber = iValue_4_3;
            iIndex += iValueLength_3;

            UInt32 iValue_4_4 = BitConverter.ToUInt32(ServerData.ReceivedData, iIndex);//统计数据（合格数量）
            statisticsinformation.DataOfStatistics[0].CameraStatisticsData[0].GoodNumber = iValue_4_4;
            iIndex += iValueLength_3;

            UInt32 iValue_4_5 = BitConverter.ToUInt32(ServerData.ReceivedData, iIndex);//统计数据（剔除数量）
            statisticsinformation.DataOfStatistics[0].CameraStatisticsData[0].RejectedNumber = iValue_4_5;
            iIndex += iValueLength_3;

            Int32 iValue_4_6 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//统计数据（工具数量）
            iIndex += iValueLength_1;
            UInt32[] iRejectedTool = new UInt32[iValue_4_6];
            for (Int32 i = 0; i < statisticsinformation.DataOfStatistics[0].CameraStatisticsData[0].RejectedStatistics_Tool.Length; i++)//遍历
            {
                for (Int32 j = 0; j < iRejectedTool.Length; j++)//遍历
                {
                    if (i == j)//相同工具（不匹配时，上下位机工具数目可能不相同）
                    {
                        iRejectedTool[j] = BitConverter.ToUInt32(ServerData.ReceivedData, iIndex);

                        statisticsinformation.DataOfStatistics[0].CameraStatisticsData[0].RejectedStatistics_Tool[i] = iRejectedTool[j];

                        iIndex += iValueLength_3;

                        break;
                    }
                }
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：解析指令
        // 输入参数：1.ServerData：客户端发送的数据
        //         2.Cameratype：相机类型数据
        //         3.iStatisticsDataType：统计数据类型（0，当前班；1，历史班）
        //         4.iShiftIndex：班次索引（从0开始）
        //         5.shifttime：统计数据开始结束时间
        //         6.statisticsinformation：统计信息
        //         7.iToolIndex：剔除图像对应的工具索引值（从0开始）
        //         8.iImageIndex：剔除图像对应的工具中的索引值（从0开始）
        //         9.dImageScale：图像尺寸类型数据
        //         10.iImageDataLength：图像数据长度
        //         11.GraphicsInformation：图像信息
        //         12.imageData：图像数据
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private void _GetInstructionData(VisionSystemCommunicationLibrary.Ethernet.DataReceivedEventArgs ServerData, ref VisionSystemClassLibrary.Enum.CameraType Cameratype, ref Int32 iStatisticsDataType, ref Int32 iShiftIndex, ref VisionSystemClassLibrary.Struct.StatisticsInformation statisticsinformation, ref Int32 iToolIndex, ref Int32 iImageIndex, ref Double dImageScale, ref Int32 iImageDataLength, ref VisionSystemClassLibrary.Struct.ImageInformation GraphicsInformation, ref Image<Bgr, Byte> imageData)
        {
            //指令类型 + 相机类型数据 + 统计数据类型（0，当前班；1，历史班） + 班次索引（从0开始） + 统计数据开始结束时间 + 统计数据（品牌名称（包括品牌长度） + 已检测数量 + 合格数量 + 剔除数量 + 工具数量 + 工具统计信息） + 剔除图像对应的工具索引值（从0开始） + 剔除图像对应的工具中的索引值（从0开始） + 图像尺寸类型数据 + 图像宽度数据 + 图像高度数据 + 图像信息数据 + 图像数据

            Int32 iIndex = 0;//临时变量
            Int32 iValueLength_1 = BitConverter.GetBytes((Int32)1).Length;//数据长度

            VisionSystemClassLibrary.Struct.SYSTEMTIME systemtime = new VisionSystemClassLibrary.Struct.SYSTEMTIME();//临时变量
            Int32 iValueLength_2 = BitConverter.GetBytes(systemtime.Year).Length;//数据长度

            Int32 iValueLength_3 = BitConverter.GetBytes((UInt32)1).Length;//数据长度

            Int32 iValueLength_4 = BitConverter.GetBytes((Double)1).Length;//数据长度

            //

            iIndex = ServerData.DataInfo.InstructionIndex + 1;
            Cameratype = (VisionSystemClassLibrary.Enum.CameraType)ServerData.ReceivedData[iIndex];//相机类型
            statisticsinformation.DataOfStatistics[0].CameraStatisticsData[0].TypeOfCamera = Cameratype;
            iIndex += 1;

            Int32 iValue_1 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//统计数据类型（0，当前班；1，历史班）
            iStatisticsDataType = iValue_1;
            iIndex += iValueLength_1;

            Int32 iValue_2 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//班次索引（从0开始）
            iShiftIndex = iValue_2 - 1;
            iIndex += iValueLength_1;

            UInt16 iValue_3_1_1 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据开始时间，年
            statisticsinformation.TimeData[0].Start.Year = iValue_3_1_1;
            iIndex += iValueLength_2;
            UInt16 iValue_3_1_2 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据开始时间，月
            statisticsinformation.TimeData[0].Start.Month = iValue_3_1_2;
            iIndex += iValueLength_2;
            UInt16 iValue_3_1_3 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据开始时间，星期
            statisticsinformation.TimeData[0].Start.DayOfWeek = iValue_3_1_3;
            iIndex += iValueLength_2;
            UInt16 iValue_3_1_4 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据开始时间，日
            statisticsinformation.TimeData[0].Start.Day = iValue_3_1_4;
            iIndex += iValueLength_2;
            UInt16 iValue_3_1_5 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据开始时间，时
            statisticsinformation.TimeData[0].Start.Hour = iValue_3_1_5;
            iIndex += iValueLength_2;
            UInt16 iValue_3_1_6 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据开始时间，分
            statisticsinformation.TimeData[0].Start.Minute = iValue_3_1_6;
            iIndex += iValueLength_2;
            UInt16 iValue_3_1_7 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据开始时间，秒
            statisticsinformation.TimeData[0].Start.Second = iValue_3_1_7;
            iIndex += iValueLength_2;
            UInt16 iValue_3_1_8 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据开始时间，毫秒
            statisticsinformation.TimeData[0].Start.MilliSeconds = iValue_3_1_8;
            iIndex += iValueLength_2;
            //
            UInt16 iValue_3_2_1 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据结束时间，年
            statisticsinformation.TimeData[0].End.Year = iValue_3_2_1;
            iIndex += iValueLength_2;
            UInt16 iValue_3_2_2 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据结束时间，月
            statisticsinformation.TimeData[0].End.Month = iValue_3_2_2;
            iIndex += iValueLength_2;
            UInt16 iValue_3_2_3 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据结束时间，星期
            statisticsinformation.TimeData[0].End.DayOfWeek = iValue_3_2_3;
            iIndex += iValueLength_2;
            UInt16 iValue_3_2_4 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据结束时间，日
            statisticsinformation.TimeData[0].End.Day = iValue_3_2_4;
            iIndex += iValueLength_2;
            UInt16 iValue_3_2_5 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据结束时间，时
            statisticsinformation.TimeData[0].End.Hour = iValue_3_2_5;
            iIndex += iValueLength_2;
            UInt16 iValue_3_2_6 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据结束时间，分
            statisticsinformation.TimeData[0].End.Minute = iValue_3_2_6;
            iIndex += iValueLength_2;
            UInt16 iValue_3_2_7 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据结束时间，秒
            statisticsinformation.TimeData[0].End.Second = iValue_3_2_7;
            iIndex += iValueLength_2;
            UInt16 iValue_3_2_8 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据结束时间，毫秒
            statisticsinformation.TimeData[0].End.MilliSeconds = iValue_3_2_8;
            iIndex += iValueLength_2;

            statisticsinformation.DataOfStatistics[0].BrandName = "";
            Int32 iValue_4_2 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//统计数据（品牌名称（包括品牌长度））
            iIndex += iValueLength_1;
            statisticsinformation.DataOfStatistics[0].BrandName = VisionSystemCommunicationLibrary.Ethernet.TestControlFunctions._ByteToString(ServerData.ReceivedData, iIndex, iValue_4_2);//统计数据（品牌名称（包括品牌长度））
            iIndex += iValue_4_2;

            UInt32 iValue_4_3 = BitConverter.ToUInt32(ServerData.ReceivedData, iIndex);//统计数据（已检测数量）
            statisticsinformation.DataOfStatistics[0].CameraStatisticsData[0].InspectedNumber = iValue_4_3;
            iIndex += iValueLength_3;

            UInt32 iValue_4_4 = BitConverter.ToUInt32(ServerData.ReceivedData, iIndex);//统计数据（合格数量）
            statisticsinformation.DataOfStatistics[0].CameraStatisticsData[0].GoodNumber = iValue_4_4;
            iIndex += iValueLength_3;

            UInt32 iValue_4_5 = BitConverter.ToUInt32(ServerData.ReceivedData, iIndex);//统计数据（剔除数量）
            statisticsinformation.DataOfStatistics[0].CameraStatisticsData[0].RejectedNumber = iValue_4_5;
            iIndex += iValueLength_3;

            Int32 iValue_4_6 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//统计数据（工具数量）
            iIndex += iValueLength_1;
            UInt32[] iRejectedTool = new UInt32[iValue_4_6];
            for (Int32 i = 0; i < statisticsinformation.DataOfStatistics[0].CameraStatisticsData[0].RejectedStatistics_Tool.Length; i++)//遍历
            {
                for (Int32 j = 0; j < iRejectedTool.Length; j++)//遍历
                {
                    if (i == j)//相同工具（不匹配时，上下位机工具数目可能不相同）
                    {
                        iRejectedTool[j] = BitConverter.ToUInt32(ServerData.ReceivedData, iIndex);//统计数据（工具数量）

                        statisticsinformation.DataOfStatistics[0].CameraStatisticsData[0].RejectedStatistics_Tool[i] = iRejectedTool[j];

                        iIndex += iValueLength_3;

                        break;
                    }
                }
            }

            Int32 iValue_5 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//剔除图像对应的工具索引值（从0开始）
            iToolIndex = iValue_5;
            iIndex += iValueLength_1;

            Int32 iValue_6 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//剔除图像对应的工具中的索引值（从0开始）
            iImageIndex = iValue_6;
            iIndex += iValueLength_1;

            Double dValue_7 = BitConverter.ToDouble(ServerData.ReceivedData, iIndex);//图像尺寸类型数据
            dImageScale = dValue_7;
            iIndex += iValueLength_4;

            Int32 iValue_8 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//图像宽度数据
            Int32 ImageWidth = iValue_8;
            iIndex += iValueLength_1;

            Int32 iValue_9 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//图像高度数据
            Int32 ImageHeight = iValue_9;
            iIndex += iValueLength_1;

            //序列化数据（图像信息数据）

            MemoryStream Memorystream = new MemoryStream();//流对象
            Int32 iSerializableDataLength = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//序列化数据长度
            iIndex += BitConverter.GetBytes(iSerializableDataLength).Length;
            Memorystream.Write(ServerData.ReceivedData, iIndex, iSerializableDataLength);//写入流
            iIndex += iSerializableDataLength;

            IFormatter formatter = new SoapFormatter();//格式化对象
            Memorystream.Position = 0;//初始化流对象
            GraphicsInformation = (VisionSystemClassLibrary.Struct.ImageInformation)formatter.Deserialize(Memorystream);//反序列化

            //图像数据

            iImageDataLength = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//图像数据大小
            iIndex += iValueLength_1;

            if (0 < iImageDataLength)//图像有效
            {
                Image<Bgr, Byte> imageData_Temp = new Image<Bgr, Byte>(ImageWidth, ImageHeight);//获取的图像数据

                byte[] byteImage = new byte[iImageDataLength];

                System.Array.Copy(ServerData.ReceivedData, iIndex, byteImage, 0, iImageDataLength);

                imageData_Temp.Bytes = byteImage;

                imageData = imageData_Temp.Copy();

                imageData_Temp.Dispose();
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：生成指令
        // 输入参数：1.InstructionType：指令类型
        //         2.Cameratype：相机类型
        //         3.bRelevancy：关联标记
        //         4.iShiftIndex：班次索引（从0开始）
        //         5.shifttime：统计数据开始结束时间
        //         6.iToolIndex：剔除图像对应的工具索引值（从0开始）
        //         7.iImageIndex：剔除图像对应的工具中的索引值（从0开始）
        //         8.dImageScale：图像尺寸类型数据
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private Byte[] _GenerateInstruction(CommunicationInstructionType InstructionType, VisionSystemClassLibrary.Enum.CameraType Cameratype, Boolean bRelevancy, Int32 iShiftIndex, VisionSystemClassLibrary.Struct.ShiftTime shifttime, Int32 iToolIndex, Int32 iImageIndex, Double dImageScale)
        {
            //指令类型 + 相机类型数据 + 关联标记 + 班次索引（非0） + 统计数据开始结束时间 + 剔除图像对应的工具索引值（从0开始） + 剔除图像对应的工具中的索引值（从0开始） + 图像尺寸类型数据

            MemoryStream memorystream = new MemoryStream();//可扩展容量数据流

            VisionSystemClassLibrary.Struct.SYSTEMTIME systemtime = new VisionSystemClassLibrary.Struct.SYSTEMTIME();//临时变量
            Int32 iValue = BitConverter.GetBytes(systemtime.Year).Length;//临时变量

            Byte[] byteValue_1 = new Byte[1];//填充待发送数据，指令标志位
            byteValue_1[0] = (Byte)InstructionType;//填充待发送数据，指令标志位
            memorystream.Write(byteValue_1, 0, byteValue_1.Length);//追加写入到可扩容数据流中

            Byte[] byteValue_2 = new Byte[1];//填充待发送数据，相机类型
            byteValue_2[0] = (Byte)Cameratype;//填充待发送数据，相机类型
            memorystream.Write(byteValue_2, 0, byteValue_2.Length);//追加写入到可扩容数据流中

            Byte[] byteValue_22;//关联信息
            if (bRelevancy) //查询关联信息
            {
                byteValue_22 = BitConverter.GetBytes(1);
            }
            else //非关联信息
            {
                byteValue_22 = BitConverter.GetBytes(0);
            }
            memorystream.Write(byteValue_22, 0, byteValue_22.Length);//追加写入到可扩容数据流中

            Byte[] byteValue_3 = BitConverter.GetBytes(iShiftIndex);//班次索引（从0开始）
            memorystream.Write(byteValue_3, 0, byteValue_3.Length);//追加写入到可扩容数据流中

            Byte[] byteValue_4 = new Byte[iValue * 8 * 2];//统计数据开始结束时间
            //
            Byte[] byteValue_Temp_1 = BitConverter.GetBytes(shifttime.Start.Year);//统计数据开始时间，年
            byteValue_Temp_1.CopyTo(byteValue_4, 0);//统计数据开始时间，年
            byteValue_Temp_1 = BitConverter.GetBytes(shifttime.Start.Month);//统计数据开始时间，月
            byteValue_Temp_1.CopyTo(byteValue_4, iValue);//统计数据开始时间，月
            byteValue_Temp_1 = BitConverter.GetBytes(shifttime.Start.DayOfWeek);//统计数据开始时间，星期
            byteValue_Temp_1.CopyTo(byteValue_4, iValue * 2);//统计数据开始时间，星期
            byteValue_Temp_1 = BitConverter.GetBytes(shifttime.Start.Day);//统计数据开始时间，日
            byteValue_Temp_1.CopyTo(byteValue_4, iValue * 3);//统计数据开始时间，日
            byteValue_Temp_1 = BitConverter.GetBytes(shifttime.Start.Hour);//统计数据开始时间，时
            byteValue_Temp_1.CopyTo(byteValue_4, iValue * 4);//统计数据始时间，时
            byteValue_Temp_1 = BitConverter.GetBytes(shifttime.Start.Minute);//统计数据开始时间，分
            byteValue_Temp_1.CopyTo(byteValue_4, iValue * 5);//统计数据开始时间，分
            byteValue_Temp_1 = BitConverter.GetBytes(shifttime.Start.Second);//统计数据开始时间，秒
            byteValue_Temp_1.CopyTo(byteValue_4, iValue * 6);//统计数据开始时间，秒
            byteValue_Temp_1 = BitConverter.GetBytes(shifttime.Start.MilliSeconds);//统计数据开始时间，毫秒
            byteValue_Temp_1.CopyTo(byteValue_4, iValue * 7);//统计数据开始时间，毫秒
            //
            byteValue_Temp_1 = BitConverter.GetBytes(shifttime.End.Year);//统计数据结束时间，年
            byteValue_Temp_1.CopyTo(byteValue_4, iValue * 8);//统计数据结束时间，年
            byteValue_Temp_1 = BitConverter.GetBytes(shifttime.End.Month);//统计数据结束时间，月
            byteValue_Temp_1.CopyTo(byteValue_4, iValue * 9);//统计数据结束时间，月
            byteValue_Temp_1 = BitConverter.GetBytes(shifttime.End.DayOfWeek);//统计数据结束时间，星期
            byteValue_Temp_1.CopyTo(byteValue_4, iValue * 10);//统计数据结束时间，星期
            byteValue_Temp_1 = BitConverter.GetBytes(shifttime.End.Day);//统计数据结束时间，日
            byteValue_Temp_1.CopyTo(byteValue_4, iValue * 11);//统计数据结束时间，日
            byteValue_Temp_1 = BitConverter.GetBytes(shifttime.End.Hour);//统计数据结束时间，时
            byteValue_Temp_1.CopyTo(byteValue_4, iValue * 12);//统计数据结束时间，时
            byteValue_Temp_1 = BitConverter.GetBytes(shifttime.End.Minute);//统计数据结束时间，分
            byteValue_Temp_1.CopyTo(byteValue_4, iValue * 13);//统计数据结束时间，分
            byteValue_Temp_1 = BitConverter.GetBytes(shifttime.End.Second);//统计数据结束时间，秒
            byteValue_Temp_1.CopyTo(byteValue_4, iValue * 14);//统计数据结束时间，秒
            byteValue_Temp_1 = BitConverter.GetBytes(shifttime.End.MilliSeconds);//统计数据结束时间，毫秒
            byteValue_Temp_1.CopyTo(byteValue_4, iValue * 15);//统计数据结束时间，毫秒
            //
            memorystream.Write(byteValue_4, 0, byteValue_4.Length);//追加写入到可扩容数据流中

            Byte[] byteValue_5 = BitConverter.GetBytes(iToolIndex);//剔除图像对应的工具索引值（从0开始）
            memorystream.Write(byteValue_5, 0, byteValue_5.Length);//追加写入到可扩容数据流中

            Byte[] byteValue_6 = BitConverter.GetBytes(iImageIndex);//剔除图像对应的工具中的索引值（从0开始）
            memorystream.Write(byteValue_6, 0, byteValue_6.Length);//追加写入到可扩容数据流中

            Byte[] byteValue_7 = BitConverter.GetBytes(dImageScale);//图像尺寸类型数据
            memorystream.Write(byteValue_7, 0, byteValue_7.Length);//追加写入到可扩容数据流中

            Byte[] arrayInstructionData = memorystream.ToArray();//待发送数据

            memorystream.Close();

            return arrayInstructionData;
        }

        //----------------------------------------------------------------------
        // 功能说明：解析指令
        // 输入参数：1.ServerData：客户端发送的数据
        //         2.Cameratype：相机类型数据
        //         3.bRelevancy：关联标记
        //         4.iShiftIndex：班次索引（从0开始）
        //         5.shifttime：统计数据开始结束时间
        //         6.iToolIndex：剔除图像对应的工具索引值（从0开始）
        //         7.iImageIndex：剔除图像对应的工具中的索引值（从0开始）
        //         8.dImageScale：图像尺寸类型数据
        //         9.iImageDataLength：图像数据长度
        //         10.GraphicsInformation：图像信息
        //         11.imageData：图像数据
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private void _GetInstructionData(VisionSystemCommunicationLibrary.Ethernet.DataReceivedEventArgs ServerData, ref VisionSystemClassLibrary.Enum.CameraType Cameratype, ref Boolean bRelevancy, ref Int32 iShiftIndex, ref VisionSystemClassLibrary.Struct.ShiftTime shifttime, ref Int32 iToolIndex, ref Int32 iImageIndex, ref Double dImageScale, ref Int32 iImageDataLength, ref VisionSystemClassLibrary.Struct.ImageInformation GraphicsInformation, ref Image<Bgr, Byte> imageData)
        {
            //指令类型 + 相机类型数据 + 关联标记 + 班次索引（非0） + 统计数据开始结束时间 + 剔除图像对应的工具索引值（从0开始） + 剔除图像对应的工具中的索引值（从0开始） + 图像尺寸类型数据 + 图像宽度数据 + 图像高度数据 + 图像信息数据 + 图像数据

            Int32 iIndex = 0;//临时变量
            Int32 iValueLength_1 = BitConverter.GetBytes((Int32)1).Length;//数据长度

            VisionSystemClassLibrary.Struct.SYSTEMTIME systemtime = new VisionSystemClassLibrary.Struct.SYSTEMTIME();//临时变量
            Int32 iValueLength_2 = BitConverter.GetBytes(systemtime.Year).Length;//数据长度

            Int32 iValueLength_3 = BitConverter.GetBytes((Double)1).Length;//数据长度

            //

            iIndex = ServerData.DataInfo.InstructionIndex + 1;
            Cameratype = (VisionSystemClassLibrary.Enum.CameraType)ServerData.ReceivedData[iIndex];//相机类型
            iIndex += 1;

            Int32 iValue_0 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//关联标记
            if (0 != iValue_0) //相机关联
            {
                bRelevancy = true;
            }
            else //相机非关联
            {
                bRelevancy = false;
            }
            iIndex += iValueLength_1;

            Int32 iValue_1 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//班次索引（从0开始）
            iShiftIndex = iValue_1 - 1;
            iIndex += iValueLength_1;

            UInt16 iValue_2_1_1 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据开始时间，年
            shifttime.Start.Year = iValue_2_1_1;
            iIndex += iValueLength_2;
            UInt16 iValue_2_1_2 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据开始时间，月
            shifttime.Start.Month = iValue_2_1_2;
            iIndex += iValueLength_2;
            UInt16 iValue_2_1_3 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据开始时间，星期
            shifttime.Start.DayOfWeek = iValue_2_1_3;
            iIndex += iValueLength_2;
            UInt16 iValue_2_1_4 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据开始时间，日
            shifttime.Start.Day = iValue_2_1_4;
            iIndex += iValueLength_2;
            UInt16 iValue_2_1_5 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据开始时间，时
            shifttime.Start.Hour = iValue_2_1_5;
            iIndex += iValueLength_2;
            UInt16 iValue_2_1_6 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据开始时间，分
            shifttime.Start.Minute = iValue_2_1_6;
            iIndex += iValueLength_2;
            UInt16 iValue_2_1_7 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据开始时间，秒
            shifttime.Start.Second = iValue_2_1_7;
            iIndex += iValueLength_2;
            UInt16 iValue_2_1_8 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据开始时间，毫秒
            shifttime.Start.MilliSeconds = iValue_2_1_8;
            iIndex += iValueLength_2;
            //
            UInt16 iValue_2_2_1 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据结束时间，年
            shifttime.End.Year = iValue_2_2_1;
            iIndex += iValueLength_2;
            UInt16 iValue_2_2_2 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据结束时间，月
            shifttime.End.Month = iValue_2_2_2;
            iIndex += iValueLength_2;
            UInt16 iValue_2_2_3 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据结束时间，星期
            shifttime.End.DayOfWeek = iValue_2_2_3;
            iIndex += iValueLength_2;
            UInt16 iValue_2_2_4 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据结束时间，日
            shifttime.End.Day = iValue_2_2_4;
            iIndex += iValueLength_2;
            UInt16 iValue_2_2_5 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据结束时间，时
            shifttime.End.Hour = iValue_2_2_5;
            iIndex += iValueLength_2;
            UInt16 iValue_2_2_6 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据结束时间，分
            shifttime.End.Minute = iValue_2_2_6;
            iIndex += iValueLength_2;
            UInt16 iValue_2_2_7 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据结束时间，秒
            shifttime.End.Second = iValue_2_2_7;
            iIndex += iValueLength_2;
            UInt16 iValue_2_2_8 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//统计数据结束时间，毫秒
            shifttime.End.MilliSeconds = iValue_2_2_8;
            iIndex += iValueLength_2;

            Int32 iValue_3 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//剔除图像对应的工具索引值（从0开始）
            iToolIndex = iValue_3;
            iIndex += iValueLength_1;

            Int32 iValue_4 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//剔除图像对应的工具中的索引值（从0开始）
            iImageIndex = iValue_4;
            iIndex += iValueLength_1;

            Double dValue_5 = BitConverter.ToDouble(ServerData.ReceivedData, iIndex);//图像尺寸类型数据
            dImageScale = dValue_5;
            iIndex += iValueLength_3;

            Int32 iValue_6 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//图像宽度数据
            Int32 ImageWidth = iValue_6;
            iIndex += iValueLength_1;

            Int32 iValue_7 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//图像高度数据
            Int32 ImageHeight = iValue_7;
            iIndex += iValueLength_1;

            //序列化数据（图像信息数据）

            MemoryStream Memorystream = new MemoryStream();//流对象
            Int32 iSerializableDataLength = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//序列化数据长度
            iIndex += BitConverter.GetBytes(iSerializableDataLength).Length;
            Memorystream.Write(ServerData.ReceivedData, iIndex, iSerializableDataLength);//写入流
            iIndex += iSerializableDataLength;

            IFormatter formatter = new SoapFormatter();//格式化对象
            Memorystream.Position = 0;//初始化流对象
            GraphicsInformation = (VisionSystemClassLibrary.Struct.ImageInformation)formatter.Deserialize(Memorystream);//反序列化

            //图像数据

            iImageDataLength = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//图像数据大小
            iIndex += iValueLength_1;

            if (0 < iImageDataLength)//图像有效
            {
                Image<Bgr, Byte> imageData_Temp = new Image<Bgr, Byte>(ImageWidth, ImageHeight);//获取的图像数据

                byte[] byteImage = new byte[iImageDataLength];

                System.Array.Copy(ServerData.ReceivedData, iIndex, byteImage, 0, iImageDataLength);

                imageData_Temp.Bytes = byteImage;

                imageData = imageData_Temp.Copy();

                imageData_Temp.Dispose();
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：解析指令
        // 输入参数：1.ServerData：客户端发送的数据
        //         2.Cameratype：相机类型数据
        //         3.iMin：最小值
        //         4.iMax：最大值
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private void _GetInstructionData(VisionSystemCommunicationLibrary.Ethernet.DataReceivedEventArgs ServerData, ref VisionSystemClassLibrary.Enum.CameraType Cameratype, ref Int32[] iMin, ref Int32[] iMax)
        {
            //客户端->服务端：指令类型 + 相机类型数据 + 公差个数 + （每个）公差下限、上限

            Int32 iIndex = 0;//临时变量
            Int32 iValueLength = BitConverter.GetBytes((Int32)1).Length;//数据长度

            //

            iIndex = ServerData.DataInfo.InstructionIndex + 1;
            Cameratype = (VisionSystemClassLibrary.Enum.CameraType)ServerData.ReceivedData[iIndex];//相机类型
            iIndex += 1;

            Int32 iValue_1 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//公差个数
            iIndex += iValueLength;

            if (0 < iValue_1)//有效
            {
                iMin = new Int32[iValue_1];
                iMax = new Int32[iValue_1];

                Int32 i = 0;//循环控制变量

                for (i = 0; i < iValue_1; i++)//遍历故障信息
                {
                    iMin[i] = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//最小值
                    iIndex += iValueLength;
                    iMax[i] = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//最大值
                    iIndex += iValueLength;
                }
            }
            else//无效
            {
                iMin = null;
                iMax = null;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：解析指令
        // 输入参数：1.ServerData：客户端发送的数据
        //         2.Cameratype：相机类型数据
        //         3.iEjectLevel：灵敏度
        //         4.iUpdateTolerances：是否UPDATE TOLERANCES
        //         5.iMin：最小值
        //         6.iMax：最大值
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private void _GetInstructionData(VisionSystemCommunicationLibrary.Ethernet.DataReceivedEventArgs ServerData, ref VisionSystemClassLibrary.Enum.CameraType Cameratype,ref Int32 iEjectLevel, ref Int32 iUpdateTolerances, ref Int32[] iMin, ref Int32[] iMax)
        {
            //客户端->服务端：指令类型 + 相机类型数据 + 灵敏度 + 调节灵敏度标记（0,：true;1：调节光电空头校准值） + 公差个数 + （每个）公差下限、上限

            Int32 iIndex = 0;//临时变量
            Int32 iValueLength = BitConverter.GetBytes((Int32)1).Length;//数据长度

            //

            iIndex = ServerData.DataInfo.InstructionIndex + 1;
            Cameratype = (VisionSystemClassLibrary.Enum.CameraType)ServerData.ReceivedData[iIndex];//相机类型
            iIndex += 1;

            iEjectLevel = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//灵敏度
            iIndex += iValueLength;

            iUpdateTolerances = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//UPDATE TOLERANCES
            iIndex += iValueLength;

            Int32 iValue_1 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//公差个数
            iIndex += iValueLength;

            if (0 < iValue_1)//有效
            {
                iMin = new Int32[iValue_1];
                iMax = new Int32[iValue_1];

                Int32 i = 0;//循环控制变量

                for (i = 0; i < iValue_1; i++)//遍历故障信息
                {
                    iMin[i] = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//最小值
                    iIndex += iValueLength;
                    iMax[i] = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//最大值
                    iIndex += iValueLength;
                }
            }
            else//无效
            {
                iMin = null;
                iMax = null;
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：生成指令
        // 输入参数：1.InstructionType：指令类型
        //         2.Cameratype：相机类型
        //         3.Parameters：参数
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private Byte[] _GenerateInstruction(CommunicationInstructionType InstructionType, VisionSystemClassLibrary.Enum.CameraType Cameratype, Int32[] Parameters)
        {
            //指令类型 + 相机类型数据 + 参数个数（Int32） + 参数数组（Int32）

            Int32 i = 0;//循环控制变量

            Int32 iIndex = 0;//临时变量
            Int32 iValueLength_Int32 = BitConverter.GetBytes((Int32)1).Length;//数据长度

            Byte[] arrayValue_1 = BitConverter.GetBytes(Parameters.Length);//参数个数
            Byte[] arrayValue_2 = new Byte[iValueLength_Int32 * Parameters.Length];//参数数组

            Byte[] arrayInstructionData = new Byte[1 + 1 + arrayValue_1.Length + arrayValue_2.Length];//待发送数据

            arrayInstructionData[0] = (Byte)InstructionType;//填充待发送数据，指令标志位

            arrayInstructionData[1] = (Byte)Cameratype;//填充待发送数据，相机类型数据
            
            iIndex = 2;
            arrayValue_1.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，数据长度
            iIndex += arrayValue_1.Length;

            for (i = 0; i < Parameters.Length; i++)
            {
                BitConverter.GetBytes(Parameters[i]).CopyTo(arrayValue_2, i * iValueLength_Int32);
            }
            arrayValue_2.CopyTo(arrayInstructionData, iIndex);//填充待发送数据，参数数组

            //

            return arrayInstructionData;
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：解析指令
        // 输入参数：1.ServerData：客户端发送的数据
        //         2.Cameratype：相机类型数据
        //         3.statisticsrecordlist：统计列表
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private void _GetInstructionData(VisionSystemCommunicationLibrary.Ethernet.DataReceivedEventArgs ServerData, ref VisionSystemClassLibrary.Enum.CameraType Cameratype, ref VisionSystemClassLibrary.Struct.StatisticsRecordList statisticsrecordlist)
        {
            //指令类型 + 相机类型数据 + 统计数据列表（班次个数 + 当前班次索引（从0开始） + 统计数据个数 + 当前统计数据索引（从0开始） + （每个班次）统计数据个数 + （每个班次，每个统计数据）班次开始结束时间 + （每个班次，每个统计数据）品牌（包括品牌长度））

            Int32 iIndex = 0;//临时变量
            Int32 iValueLength_1 = BitConverter.GetBytes((Int32)1).Length;//数据长度

            VisionSystemClassLibrary.Struct.SYSTEMTIME systemtime = new VisionSystemClassLibrary.Struct.SYSTEMTIME();//临时变量
            Int32 iValueLength_2 = BitConverter.GetBytes(systemtime.Year).Length;//数据长度

            //

            iIndex = ServerData.DataInfo.InstructionIndex + 1;
            Cameratype = (VisionSystemClassLibrary.Enum.CameraType)ServerData.ReceivedData[iIndex];//相机类型
            iIndex += 1;

            Int32 iValue_1 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//班次个数
            statisticsrecordlist.RecordListData = new VisionSystemClassLibrary.Struct.StatisticsRecordListData[iValue_1];
            iIndex += iValueLength_1;

            Int32 iValue_2 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//当前班次索引（从0开始）
            statisticsrecordlist.CurrentShiftIndex = iValue_2 - 1;
            iIndex += iValueLength_1;

            Int32 iValue_3 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//统计数据个数
            statisticsrecordlist.RecordNumber = iValue_3;
            iIndex += iValueLength_1;

            Int32 iValue_4 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//当前统计数据索引（从0开始）
            if (0 <= statisticsrecordlist.CurrentShiftIndex)//有效班次
            {
                statisticsrecordlist.RecordListData[statisticsrecordlist.CurrentShiftIndex].CurrentStatisticsRecordIndex = iValue_4;
            }
            iIndex += iValueLength_1;

            if (0 < statisticsrecordlist.RecordNumber)//存在统计数据
            {
                Int32 i = 0;//循环控制变量
                Int32 j = 0;//循环控制变量

                for (i = 0; i < statisticsrecordlist.RecordListData.Length; i++)//遍历班次
                {
                    Int32 iValue_5 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//（每个班次）统计数据个数
                    iIndex += iValueLength_1;

                    if (0 < iValue_5)//存在统计数据
                    {
                        statisticsrecordlist.RecordListData[i].TimeData = new VisionSystemClassLibrary.Struct.ShiftTime[iValue_5];
                        statisticsrecordlist.RecordListData[i].BrandName = new String[iValue_5];

                        for (j = 0; j < statisticsrecordlist.RecordListData[i].TimeData.Length; j++)//遍历统计数据
                        {
                            statisticsrecordlist.RecordListData[i].TimeData[j] = new VisionSystemClassLibrary.Struct.ShiftTime();
                            statisticsrecordlist.RecordListData[i].TimeData[j].Start._InitData();
                            statisticsrecordlist.RecordListData[i].TimeData[j].End._InitData();
                            //
                            UInt16 iValue_6_1_1 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//（每个班次，每个统计数据）班次开始时间，年
                            statisticsrecordlist.RecordListData[i].TimeData[j].Start.Year = iValue_6_1_1;
                            iIndex += iValueLength_2;
                            UInt16 iValue_6_1_2 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//（每个班次，每个统计数据）班次开始时间，月
                            statisticsrecordlist.RecordListData[i].TimeData[j].Start.Month = iValue_6_1_2;
                            iIndex += iValueLength_2;
                            UInt16 iValue_6_1_3 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//（每个班次，每个统计数据）班次开始时间，星期
                            statisticsrecordlist.RecordListData[i].TimeData[j].Start.DayOfWeek = iValue_6_1_3;
                            iIndex += iValueLength_2;
                            UInt16 iValue_6_1_4 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//（每个班次，每个统计数据）班次开始时间，日
                            statisticsrecordlist.RecordListData[i].TimeData[j].Start.Day = iValue_6_1_4;
                            iIndex += iValueLength_2;
                            UInt16 iValue_6_1_5 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//（每个班次，每个统计数据）班次开始时间，时
                            statisticsrecordlist.RecordListData[i].TimeData[j].Start.Hour = iValue_6_1_5;
                            iIndex += iValueLength_2;
                            UInt16 iValue_6_1_6 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//（每个班次，每个统计数据）班次开始时间，分
                            statisticsrecordlist.RecordListData[i].TimeData[j].Start.Minute = iValue_6_1_6;
                            iIndex += iValueLength_2;
                            UInt16 iValue_6_1_7 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//（每个班次，每个统计数据）班次开始时间，秒
                            statisticsrecordlist.RecordListData[i].TimeData[j].Start.Second = iValue_6_1_7;
                            iIndex += iValueLength_2;
                            UInt16 iValue_6_1_8 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//（每个班次，每个统计数据）班次开始时间，毫秒
                            statisticsrecordlist.RecordListData[i].TimeData[j].Start.MilliSeconds = iValue_6_1_8;
                            iIndex += iValueLength_2;
                            //
                            UInt16 iValue_6_2_1 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//（每个班次，每个统计数据）班次结束时间，年
                            statisticsrecordlist.RecordListData[i].TimeData[j].End.Year = iValue_6_2_1;
                            iIndex += iValueLength_2;
                            UInt16 iValue_6_2_2 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//（每个班次，每个统计数据）班次结束时间，月
                            statisticsrecordlist.RecordListData[i].TimeData[j].End.Month = iValue_6_2_2;
                            iIndex += iValueLength_2;
                            UInt16 iValue_6_2_3 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//（每个班次，每个统计数据）班次结束时间，星期
                            statisticsrecordlist.RecordListData[i].TimeData[j].End.DayOfWeek = iValue_6_2_3;
                            iIndex += iValueLength_2;
                            UInt16 iValue_6_2_4 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//（每个班次，每个统计数据）班次结束时间，日
                            statisticsrecordlist.RecordListData[i].TimeData[j].End.Day = iValue_6_2_4;
                            iIndex += iValueLength_2;
                            UInt16 iValue_6_2_5 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//（每个班次，每个统计数据）班次结束时间，时
                            statisticsrecordlist.RecordListData[i].TimeData[j].End.Hour = iValue_6_2_5;
                            iIndex += iValueLength_2;
                            UInt16 iValue_6_2_6 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//（每个班次，每个统计数据）班次结束时间，分
                            statisticsrecordlist.RecordListData[i].TimeData[j].End.Minute = iValue_6_2_6;
                            iIndex += iValueLength_2;
                            UInt16 iValue_6_2_7 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//（每个班次，每个统计数据）班次结束时间，秒
                            statisticsrecordlist.RecordListData[i].TimeData[j].End.Second = iValue_6_2_7;
                            iIndex += iValueLength_2;
                            UInt16 iValue_6_2_8 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//（每个班次，每个统计数据）班次结束时间，毫秒
                            statisticsrecordlist.RecordListData[i].TimeData[j].End.MilliSeconds = iValue_6_2_8;
                            iIndex += iValueLength_2;

                            statisticsrecordlist.RecordListData[i].BrandName[j] = "";
                            Int32 iValue_7_1 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//（每个班次，每个统计数据）品牌（包括品牌长度）
                            iIndex += iValueLength_1;
                            statisticsrecordlist.RecordListData[i].BrandName[j] = VisionSystemCommunicationLibrary.Ethernet.TestControlFunctions._ByteToString(ServerData.ReceivedData, iIndex, iValue_7_1);//（每个班次，每个统计数据）品牌（包括品牌长度）
                            iIndex += iValue_7_1;
                        }
                    }
                    else//不存在统计数据
                    {
                        statisticsrecordlist.RecordListData[i].TimeData = null;
                        statisticsrecordlist.RecordListData[i].BrandName = null;
                    }
                }
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：生成指令
        // 输入参数：1.InstructionType：指令类型
        //         2.Cameratype：相机类型
        //         3.iDeleteType：删除统计数据方式（0，删除所有；1，删除指定班次；2，删除指定记录）
        //         4.iShiftNumber：待删除的指定班次个数
        //         5.iShiftIndex：班次索引值数组（从0开始）
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private Byte[] _GenerateInstruction(CommunicationInstructionType InstructionType, VisionSystemClassLibrary.Enum.CameraType Cameratype, Int32 iDeleteType, Int32 iShiftNumber, Int32[] iShiftIndex)
        {
            //指令类型 + 相机类型数据 + 删除统计数据方式（0，删除所有；1，删除指定班次；2，删除指定记录）
            //（1） + 待删除的指定班次个数 + 班次索引值数组（从0开始）

            MemoryStream memorystream = new MemoryStream();//可扩展容量数据流

            VisionSystemClassLibrary.Struct.SYSTEMTIME systemtime = new VisionSystemClassLibrary.Struct.SYSTEMTIME();//临时变量
            Int32 iValue = BitConverter.GetBytes(systemtime.Year).Length;//临时变量

            Byte[] byteValue_1 = new Byte[1];//填充待发送数据，指令标志位
            byteValue_1[0] = (Byte)InstructionType;//填充待发送数据，指令标志位
            memorystream.Write(byteValue_1, 0, byteValue_1.Length);//追加写入到可扩容数据流中

            Byte[] byteValue_2 = new Byte[1];//填充待发送数据，相机类型
            byteValue_2[0] = (Byte)Cameratype;//填充待发送数据，相机类型
            memorystream.Write(byteValue_2, 0, byteValue_2.Length);//追加写入到可扩容数据流中

            Byte[] byteValue_3 = BitConverter.GetBytes(iDeleteType);//删除统计数据方式（0，删除所有；1，删除指定班次；2，删除指定记录）
            memorystream.Write(byteValue_3, 0, byteValue_3.Length);//追加写入到可扩容数据流中

            Byte[] byteValue_4 = BitConverter.GetBytes(iShiftNumber);//待删除的指定班次个数
            memorystream.Write(byteValue_4, 0, byteValue_4.Length);//追加写入到可扩容数据流中

            for (Int32 i = 0; i < iShiftNumber; i++)//班次索引值数组（从0开始）
            {
                Byte[] byteValue_5 = BitConverter.GetBytes(iShiftIndex[i]);//班次索引值数组（从0开始）
                memorystream.Write(byteValue_5, 0, byteValue_5.Length);//追加写入到可扩容数据流中
            }

            Byte[] arrayInstructionData = memorystream.ToArray();//待发送数据

            memorystream.Close();

            return arrayInstructionData;
        }

        //----------------------------------------------------------------------
        // 功能说明：生成指令
        // 输入参数：1.InstructionType：指令类型
        //         2.Cameratype：相机类型
        //         3.iDeleteType：删除统计数据方式（0，删除所有；1，删除指定班次；2，删除指定记录）
        //         4.iShiftIndex：班次索引值（从0开始）
        //         5.iRecordNumber：待删除的指定记录个数
        //         6.RecordTime：记录开始结束时间数组
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private Byte[] _GenerateInstruction(CommunicationInstructionType InstructionType, VisionSystemClassLibrary.Enum.CameraType Cameratype, Int32 iDeleteType, Int32 iShiftIndex, Int32 iRecordNumber, VisionSystemClassLibrary.Struct.ShiftTime[] RecordTime)
        {
            //指令类型 + 相机类型数据 + 删除统计数据方式（0，删除所有；1，删除指定班次；2，删除指定记录）
            //（2） + 班次索引值（从0开始） + 待删除的指定记录个数 + 记录开始结束时间数组

            MemoryStream memorystream = new MemoryStream();//可扩展容量数据流

            VisionSystemClassLibrary.Struct.SYSTEMTIME systemtime = new VisionSystemClassLibrary.Struct.SYSTEMTIME();//临时变量
            Int32 iValue = BitConverter.GetBytes(systemtime.Year).Length;//临时变量

            Byte[] byteValue_1 = new Byte[1];//填充待发送数据，指令标志位
            byteValue_1[0] = (Byte)InstructionType;//填充待发送数据，指令标志位
            memorystream.Write(byteValue_1, 0, byteValue_1.Length);//追加写入到可扩容数据流中

            Byte[] byteValue_2 = new Byte[1];//填充待发送数据，相机类型
            byteValue_2[0] = (Byte)Cameratype;//填充待发送数据，相机类型
            memorystream.Write(byteValue_2, 0, byteValue_2.Length);//追加写入到可扩容数据流中

            Byte[] byteValue_3 = BitConverter.GetBytes(iDeleteType);//删除统计数据方式（0，删除所有；1，删除指定班次；2，删除指定记录）
            memorystream.Write(byteValue_3, 0, byteValue_3.Length);//追加写入到可扩容数据流中

            Byte[] byteValue_4 = BitConverter.GetBytes(iShiftIndex);//班次索引值（从0开始）
            memorystream.Write(byteValue_4, 0, byteValue_4.Length);//追加写入到可扩容数据流中

            Byte[] byteValue_5 = BitConverter.GetBytes(iRecordNumber);//待删除的指定记录个数
            memorystream.Write(byteValue_5, 0, byteValue_5.Length);//追加写入到可扩容数据流中

            for (Int32 i = 0; i < iRecordNumber; i++)//记录开始结束时间数组
            {
                Byte[] byteValue_6 = new Byte[iValue * 8 * 2];//统计数据开始结束时间
                //
                Byte[] byteValue_Temp_1 = BitConverter.GetBytes(RecordTime[i].Start.Year);//记录开始时间，年
                byteValue_Temp_1.CopyTo(byteValue_6, 0);//记录开始时间，年
                byteValue_Temp_1 = BitConverter.GetBytes(RecordTime[i].Start.Month);//记录开始时间，月
                byteValue_Temp_1.CopyTo(byteValue_6, iValue);//记录开始时间，月
                byteValue_Temp_1 = BitConverter.GetBytes(RecordTime[i].Start.DayOfWeek);//记录开始时间，星期
                byteValue_Temp_1.CopyTo(byteValue_6, iValue * 2);//记录开始时间，星期
                byteValue_Temp_1 = BitConverter.GetBytes(RecordTime[i].Start.Day);//记录开始时间，日
                byteValue_Temp_1.CopyTo(byteValue_6, iValue * 3);//记录开始时间，日
                byteValue_Temp_1 = BitConverter.GetBytes(RecordTime[i].Start.Hour);//记录开始时间，时
                byteValue_Temp_1.CopyTo(byteValue_6, iValue * 4);//记录开始时间，时
                byteValue_Temp_1 = BitConverter.GetBytes(RecordTime[i].Start.Minute);//记录开始时间，分
                byteValue_Temp_1.CopyTo(byteValue_6, iValue * 5);//记录开始时间，分
                byteValue_Temp_1 = BitConverter.GetBytes(RecordTime[i].Start.Second);//记录开始时间，秒
                byteValue_Temp_1.CopyTo(byteValue_6, iValue * 6);//记录开始时间，秒
                byteValue_Temp_1 = BitConverter.GetBytes(RecordTime[i].Start.MilliSeconds);//记录开始时间，毫秒
                byteValue_Temp_1.CopyTo(byteValue_6, iValue * 7);//记录开始时间，毫秒
                //
                byteValue_Temp_1 = BitConverter.GetBytes(RecordTime[i].End.Year);//记录结束时间，年
                byteValue_Temp_1.CopyTo(byteValue_6, iValue * 8);//记录结束时间，年
                byteValue_Temp_1 = BitConverter.GetBytes(RecordTime[i].End.Month);//记录结束时间，月
                byteValue_Temp_1.CopyTo(byteValue_6, iValue * 9);//记录结束时间，月
                byteValue_Temp_1 = BitConverter.GetBytes(RecordTime[i].End.DayOfWeek);//记录结束时间，星期
                byteValue_Temp_1.CopyTo(byteValue_6, iValue * 10);//记录结束时间，星期
                byteValue_Temp_1 = BitConverter.GetBytes(RecordTime[i].End.Day);//记录结束时间，日
                byteValue_Temp_1.CopyTo(byteValue_6, iValue * 11);//记录结束时间，日
                byteValue_Temp_1 = BitConverter.GetBytes(RecordTime[i].End.Hour);//记录结束时间，时
                byteValue_Temp_1.CopyTo(byteValue_6, iValue * 12);//记录结束时间，时
                byteValue_Temp_1 = BitConverter.GetBytes(RecordTime[i].End.Minute);//记录结束时间，分
                byteValue_Temp_1.CopyTo(byteValue_6, iValue * 13);//记录结束时间，分
                byteValue_Temp_1 = BitConverter.GetBytes(RecordTime[i].End.Second);//记录结束时间，秒
                byteValue_Temp_1.CopyTo(byteValue_6, iValue * 14);//记录结束时间，秒
                byteValue_Temp_1 = BitConverter.GetBytes(RecordTime[i].End.MilliSeconds);//记录结束时间，毫秒
                byteValue_Temp_1.CopyTo(byteValue_6, iValue * 15);//记录结束时间，毫秒
                //
                memorystream.Write(byteValue_6, 0, byteValue_6.Length);//追加写入到可扩容数据流中
            }

            Byte[] arrayInstructionData = memorystream.ToArray();//待发送数据

            memorystream.Close();

            return arrayInstructionData;
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：解析指令
        // 输入参数：1.ServerData：客户端发送的数据
        //         2.Cameratype：相机类型数据
        //         3.faultmessage：故障信息
        //         4.iSpeedPhaseType：机器速度/相位类型
        //         5.iSpeedPhaseValue：机器速度/相位数值
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private void _GetInstructionData(VisionSystemCommunicationLibrary.Ethernet.DataReceivedEventArgs ServerData, ref VisionSystemClassLibrary.Enum.CameraType Cameratype, ref VisionSystemClassLibrary.Struct.FaultMessage faultmessage, ref Int32 iSpeedPhaseType, ref Int32 iSpeedPhaseValue)
        {
            //客户端->服务端：指令类型 + 相机类型数据 + 故障信息（时间 + 故障代码索引值） + 机器速度/相位标志（1，速度；0，相位） + 机器速度/相位数值

            Int32 iIndex = 0;//临时变量
            Int32 iValueLength_1 = BitConverter.GetBytes((Int32)1).Length;//数据长度

            VisionSystemClassLibrary.Struct.SYSTEMTIME systemtime = new VisionSystemClassLibrary.Struct.SYSTEMTIME();//临时变量
            Int32 iValueLength_2 = BitConverter.GetBytes(systemtime.Year).Length;//数据长度

            //

            iIndex = ServerData.DataInfo.InstructionIndex + 1;
            Cameratype = (VisionSystemClassLibrary.Enum.CameraType)ServerData.ReceivedData[iIndex];//相机类型
            Int32 iCameraIndex = _GetSelectedCameraIndex(Cameratype);//相机类型索引
            iIndex += 1;

            faultmessage.TimeData = new VisionSystemClassLibrary.Struct.SYSTEMTIME();
            faultmessage.TimeData._InitData();
            //
            UInt16 iValue_2_1_1 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//开始时间，年
            faultmessage.TimeData.Year = iValue_2_1_1;
            iIndex += iValueLength_2;
            UInt16 iValue_2_1_2 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//开始时间，月
            faultmessage.TimeData.Month = iValue_2_1_2;
            iIndex += iValueLength_2;
            UInt16 iValue_2_1_3 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//开始时间，星期
            faultmessage.TimeData.DayOfWeek = iValue_2_1_3;
            iIndex += iValueLength_2;
            UInt16 iValue_2_1_4 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//开始时间，日
            faultmessage.TimeData.Day = iValue_2_1_4;
            iIndex += iValueLength_2;
            UInt16 iValue_2_1_5 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//开始时间，时
            faultmessage.TimeData.Hour = iValue_2_1_5;
            iIndex += iValueLength_2;
            UInt16 iValue_2_1_6 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//开始时间，分
            faultmessage.TimeData.Minute = iValue_2_1_6;
            iIndex += iValueLength_2;
            UInt16 iValue_2_1_7 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//开始时间，秒
            faultmessage.TimeData.Second = iValue_2_1_7;
            iIndex += iValueLength_2;
            UInt16 iValue_2_1_8 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//开始时间，毫秒
            faultmessage.TimeData.MilliSeconds = iValue_2_1_8;
            iIndex += iValueLength_2;

            Int32 iValue_3 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//故障代码索引值
            faultmessage.DataIndex = iValue_3;
            iIndex += iValueLength_1;

            Int32 iValue_4 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//机器速度/相位类型
            iSpeedPhaseType = iValue_4;
            iIndex += iValueLength_1;

            Int32 iValue_5 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//机器速度/相位数值
            iSpeedPhaseValue = iValue_5;
            iIndex += iValueLength_1;
        }

        //----------------------------------------------------------------------
        // 功能说明：解析指令
        // 输入参数：1.ServerData：客户端发送的数据
        //         2.Cameratype：相机类型数据
        //         3.faultmessage：（无意义）
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private void _GetInstructionData(VisionSystemCommunicationLibrary.Ethernet.DataReceivedEventArgs ServerData, ref VisionSystemClassLibrary.Enum.CameraType Cameratype, ref VisionSystemClassLibrary.Struct.FaultMessage[] faultmessage)
        {
            //客户端->服务端：指令类型 + 相机类型数据 + 故障信息个数 + 故障信息数组（时间 + 故障代码索引值））

            Int32 iIndex = 0;//临时变量
            Int32 iValueLength_1 = BitConverter.GetBytes((Int32)1).Length;//数据长度

            VisionSystemClassLibrary.Struct.SYSTEMTIME systemtime = new VisionSystemClassLibrary.Struct.SYSTEMTIME();//临时变量
            Int32 iValueLength_2 = BitConverter.GetBytes(systemtime.Year).Length;//数据长度

            //

            iIndex = ServerData.DataInfo.InstructionIndex + 1;
            Cameratype = (VisionSystemClassLibrary.Enum.CameraType)ServerData.ReceivedData[iIndex];//相机类型
            Int32 iCameraIndex = _GetSelectedCameraIndex(Cameratype);//相机类型索引
            iIndex += 1;

            Int32 iValue_1 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//故障信息个数
            iIndex += iValueLength_1;

            if (0 < iValue_1)//存在统计数据
            {
                Global.VisionSystem.Camera[iCameraIndex].FaultMessages = new VisionSystemClassLibrary.Struct.FaultMessage[iValue_1];

                Int32 i = 0;//循环控制变量

                for (i = 0; i < Global.VisionSystem.Camera[iCameraIndex].FaultMessages.Length; i++)//遍历故障信息
                {
                    Global.VisionSystem.Camera[iCameraIndex].FaultMessages[i].TimeData = new VisionSystemClassLibrary.Struct.SYSTEMTIME();
                    Global.VisionSystem.Camera[iCameraIndex].FaultMessages[i].TimeData._InitData();
                    //
                    UInt16 iValue_2_1_1 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//开始时间，年
                    Global.VisionSystem.Camera[iCameraIndex].FaultMessages[i].TimeData.Year = iValue_2_1_1;
                    iIndex += iValueLength_2;
                    UInt16 iValue_2_1_2 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//开始时间，月
                    Global.VisionSystem.Camera[iCameraIndex].FaultMessages[i].TimeData.Month = iValue_2_1_2;
                    iIndex += iValueLength_2;
                    UInt16 iValue_2_1_3 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//开始时间，星期
                    Global.VisionSystem.Camera[iCameraIndex].FaultMessages[i].TimeData.DayOfWeek = iValue_2_1_3;
                    iIndex += iValueLength_2;
                    UInt16 iValue_2_1_4 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//开始时间，日
                    Global.VisionSystem.Camera[iCameraIndex].FaultMessages[i].TimeData.Day = iValue_2_1_4;
                    iIndex += iValueLength_2;
                    UInt16 iValue_2_1_5 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//开始时间，时
                    Global.VisionSystem.Camera[iCameraIndex].FaultMessages[i].TimeData.Hour = iValue_2_1_5;
                    iIndex += iValueLength_2;
                    UInt16 iValue_2_1_6 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//开始时间，分
                    Global.VisionSystem.Camera[iCameraIndex].FaultMessages[i].TimeData.Minute = iValue_2_1_6;
                    iIndex += iValueLength_2;
                    UInt16 iValue_2_1_7 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//开始时间，秒
                    Global.VisionSystem.Camera[iCameraIndex].FaultMessages[i].TimeData.Second = iValue_2_1_7;
                    iIndex += iValueLength_2;
                    UInt16 iValue_2_1_8 = BitConverter.ToUInt16(ServerData.ReceivedData, iIndex);//开始时间，毫秒
                    Global.VisionSystem.Camera[iCameraIndex].FaultMessages[i].TimeData.MilliSeconds = iValue_2_1_8;
                    iIndex += iValueLength_2;

                    Int32 iValue_3 = BitConverter.ToInt32(ServerData.ReceivedData, iIndex);//故障代码索引值
                    Global.VisionSystem.Camera[iCameraIndex].FaultMessages[i].DataIndex = iValue_3;
                    iIndex += iValueLength_1;
                }
            }
            else//不存在统计数据
            {
                Global.VisionSystem.Camera[iCameraIndex].FaultMessages = null;
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：生成指令
        // 输入参数：1.InstructionType：指令类型
        //         2.Cameratype：相机类型
        //         3.iValue_1：数据
        // 输出参数：无
        // 返回值：生成的指令
        //----------------------------------------------------------------------
        private Byte[] _GenerateInstruction(CommunicationInstructionType InstructionType, VisionSystemClassLibrary.Enum.CameraType Cameratype, UInt64 iValue_1)
        {
            //指令类型 + 相机类型数据 + 故障信息使能状态

            Byte[] arrayValue_1 = BitConverter.GetBytes(iValue_1);//数值

            Byte[] arrayInstructionData = new Byte[1 + 1 + arrayValue_1.Length];//待发送数据

            arrayInstructionData[0] = (Byte)InstructionType;//填充待发送数据，指令标志位
            arrayInstructionData[1] = (Byte)Cameratype;//填充待发送数据，相机类型数据
            arrayValue_1.CopyTo(arrayInstructionData, 2);//填充待发送数据，工具索引数值

            //

            return arrayInstructionData;
        }

        //----------------------------------------------------------------------
        // 功能说明：workControl连接事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void workControl_Connect_Click(object sender, EventArgs e)
        {
            Int32 i = 0; //循环变量

            for (i = 0; i < Global.VisionSystem.SystemCameraConfiguration.Length; i++) //遍历所有配置相机
            {
                if (VisionSystemControlLibrary.GlobalWindows.NetDiagnose_Window.NetDiagnoseControl.CameraSelected == Global.VisionSystem.SystemCameraConfiguration[i].Type) //返回被选中相机
                {
                    try
                    {

                        Byte[] NetCheck_ConnectCamera_Data = _GenerateInstruction(CommunicationInstructionType.NetCheck_ConnectCamera, VisionSystemControlLibrary.GlobalWindows.NetDiagnose_Window.NetDiagnoseControl.CameraSelected);//生成指令数据

                        Byte[] NetCheck_ConnectCamera_ClientIP = IPAddress.Parse(Global.VisionSystem.SystemCameraConfiguration[i].IPAddress).GetAddressBytes();//获取相机IP地址

                        if (true == Global.NetServer.ClientData[NetCheck_ConnectCamera_ClientIP[3]].Connected) //连接有效
                        {
                            Global.NetServer.ClientData[NetCheck_ConnectCamera_ClientIP[3]]._Send(NetCheck_ConnectCamera_Data);//发送数据
                        }
                    }
                    catch (System.Exception ex)
                    {

                    }
                    break;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：workControl Ping事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void workControl_Ping_Click(object sender, EventArgs e)
        {
            Int32 i = 0; //循环变量

            for (i = 0; i < Global.VisionSystem.SystemCameraConfiguration.Length; i++) //遍历所有配置相机
            {
                if ((VisionSystemControlLibrary.GlobalWindows.NetDiagnose_Window.NetDiagnoseControl.ControllerName == Global.VisionSystem.SystemCameraConfiguration[i].ControllerCHNName) || (VisionSystemControlLibrary.GlobalWindows.NetDiagnose_Window.NetDiagnoseControl.ControllerName == Global.VisionSystem.SystemCameraConfiguration[i].ControllerENGName)) //返回被选中相机
                {
                    Boolean bNetCheckResult = _NetCheckState(Global.VisionSystem.SystemCameraConfiguration[i].IPAddress);

                    workControl._UpdateNetCheck_Ping(VisionSystemControlLibrary.GlobalWindows.NetDiagnose_Window.NetDiagnoseControl.ControllerName, bNetCheckResult);

                    break;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：网络检查
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private Boolean _NetCheckState(string clientIP = "127.0.0.1")
        {
            Boolean onLine = false;

            try
            {
                System.Net.NetworkInformation.Ping netPing = new System.Net.NetworkInformation.Ping();
                System.Net.NetworkInformation.PingReply netPingReply = netPing.Send(clientIP);

                if (netPingReply.Status == System.Net.NetworkInformation.IPStatus.Success)
                {
                    onLine = true;
                }
            }
            catch (System.Exception ex)
            {
            	
            }
            return onLine;
        }
    }
}