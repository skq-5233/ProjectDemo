/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：Global.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：数据定义

原作者：蒋涛
完成日期：2014/10/28
特别说明：无

修改者：无
完成日期：无
修改记录：无

****************************************************************/

using System;

using System.Diagnostics;

namespace VisionSystemUserInterface
{
    //全局类

    public class Global
    {
        public static Boolean TopMostWindows = true;//各个窗口是否置顶显示。取值范围：true，是；false，否

        //
        
        public static Int32 NetPort = 5000;//以太网通信端口
        public static Int32 NetReceiveBufferSize = 8192;//以太网接收缓冲区
        public static Int32 NetSendBufferSize = 8192;//以太网发送缓冲区

        //

        public static Int32 SelfTriggerTime = 150;//SELF TRIGGER时间

        public static Int32 CurrentFaultMessageTime = 1000;//FAULT MESSAGE时间

        //

        public static Int32 ImageDataTime = 10;//循环数据传输休眠时间10ms

        //
        public static Double ImageScale_Work = 1.0;//WORK页面图像比例（值为图像显示控件的ControlScale_Y）
        public static Double ImageScale_TolerancesSettings = 1.0;//TOLERANCES SETTINGS页面图像比例（值为图像显示控件的ControlScale_Y）

        //

        public static ApplicationInterface CurrentInterface = ApplicationInterface.Work;//当前页面
        
        //

        public static VisionSystemUserInterface.Startup StartupWindow;//STARTUP窗口

        public static VisionSystemUserInterface.Work WorkWindow;//WORK窗口
        public static VisionSystemUserInterface.Load LoadWindow;//LOAD窗口
        public static VisionSystemUserInterface.SystemConfiguration SystemConfigurationWindow;//SYSTEM窗口
        public static VisionSystemUserInterface.DevicesSetup DevicesSetupWindow;//DEVICES SETUP窗口
        public static VisionSystemUserInterface.BrandManagement BrandManagementWindow;//BRAND MANAGEMENT窗口
        public static VisionSystemUserInterface.BackupBrands BackupBrandsWindow;//BRAND MANAGEMENT，BACKUP BRANDS窗口
        public static VisionSystemUserInterface.RestoreBrands RestoreBrandsWindow;//BRAND MANAGEMENT，RESTORE BRANDS窗口
        public static VisionSystemUserInterface.TolerancesSettings TolerancesSettingsWindow;//TOLERANCES SETTINGS窗口
        public static VisionSystemUserInterface.LiveView LiveViewWindow;//LIVE VIEW窗口 
        public static VisionSystemUserInterface.QualityCheck QualityCheckWindow;//QUALITY CHECK窗口
        public static VisionSystemUserInterface.ImageConfiguration ImageConfigurationWindow;//IMAGE CONFIGURATION窗口
        public static VisionSystemUserInterface.StatisticsView StatisticsViewWindow;//STATISTICS VIEW窗口
        
        //

        public static VisionSystemCommunicationLibrary.Ethernet.ServerControl NetServer = new VisionSystemCommunicationLibrary.Ethernet.ServerControl();//以太网通信类
        public static VisionSystemCommunicationLibrary.Ethernet.ServerData NetServerData = new VisionSystemCommunicationLibrary.Ethernet.ServerData();//以太网通信类数据

        public static VisionSystemClassLibrary.Class.System VisionSystem = new VisionSystemClassLibrary.Class.System();//视觉系统类

        public static string HMIApplicationVersion = "";//本机应用程序版本 
        public static string ControllerApplicationVersion = "";//控制器应用程序版本 

        public static string UpdateHMIApplicationVersion = "";//人机界面更新程序文件版本
        public static string UpdateControllerApplicationVersion = "";//控制器更新程序文件版本
        public static string UpdateApplicationPath = "";//更新程序文件路径

        //

        public static string USBDeviceName = "";//USB目录名称（如，C:\）

        //

        public const string Load_Background_Chinese_FileName = "Load_Background_Chinese.png";//商标背景图像1文件名称（LOAD，REG）
        public const string Load_Background_English_FileName = "Load_Background_English.png";//商标背景图像1文件名称（LOAD，REG）
        public const string About_Background_Chinese_FileName = "About_Background_Chinese.png";//商标背景图像1文件名称（LOAD，REG）
        public const string About_Background_English_FileName = "About_Background_English.png";//商标背景图像1文件名称（LOAD，REG）

        public static System.Drawing.Bitmap Load_Background_Chinese = null;//中文背景图像（TEST）
        public static System.Drawing.Bitmap Load_Background_English = null;//英文背景图像（TEST）
        public static System.Drawing.Bitmap About_Background_Chinese = null;//中文背景图像（TEST）
        public static System.Drawing.Bitmap About_Background_English = null;//英文背景图像（TEST）

        public static Int32 Data_Value = 0;//数值

        //函数

        //----------------------------------------------------------------------
        // 功能说明：获取CURRENT品牌名称
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public static String _GetCURRENTBrandName()
        {
            String sReturn = "";//函数返回值

            if (0 <= Global.VisionSystem.Brand.CURRENTBrandIndex)//CURRENT品牌存在
            {
                sReturn = Global.VisionSystem.Brand.SystemBrandData[Global.VisionSystem.Brand.CURRENTBrandIndex].Name;
            }
            else//CURRENT品牌不存在
            {
                if (VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese == VisionSystemClassLibrary.Class.System.Language)//中文
                {
                    sReturn = "No Brand";
                }
                else if (VisionSystemClassLibrary.Enum.InterfaceLanguage.English == VisionSystemClassLibrary.Class.System.Language)//英文
                {
                    sReturn = "No Brand";
                }
                else//其它，中文
                {
                    sReturn = "No Brand";
                }
            }

            return sReturn;
        }

        //----------------------------------------------------------------------
        // 功能说明：获取班次信息
        // 输入参数：无
        // 输出参数：无
        // 返回值：班次信息
        //----------------------------------------------------------------------
        public static String _GetShiftInformation()
        {
            String sReturn = "";//返回值

            if (Global.VisionSystem.Shift.ShiftState)//班次使能
            {
                Int32 iCurrentShiftIndex = Global.VisionSystem.Shift._GetCurrentShift();
                VisionSystemClassLibrary.Struct.ShiftTime CurrentShiftTime = Global.VisionSystem.Shift._GetCurrentShiftTimeData(iCurrentShiftIndex);

                if (0 < iCurrentShiftIndex)//有效班
                {
                    sReturn = iCurrentShiftIndex.ToString() + "，" + VisionSystemClassLibrary.Class.Shift._GetShiftTime(CurrentShiftTime.Start, CurrentShiftTime.End);//更新时间
                }
                else//无效班
                {
                    sReturn = "--" + "，" + VisionSystemClassLibrary.Class.Shift._GetShiftTime(CurrentShiftTime.Start, CurrentShiftTime.End);//更新时间
                }
            }

            return sReturn;
        }

        //----------------------------------------------------------------------
        // 功能说明：获取班次信息
        // 输入参数：无
        // 输出参数：无
        // 返回值：班次信息
        //----------------------------------------------------------------------
        public static void _GetControllerApplicationVersion()
        {
            Int32 i = 0;//循环控制变量

            if (null != VisionSystem.Camera && 0 < VisionSystem.Camera.Count)//有效
            {
                if (Global.VisionSystem.Camera[0].DeviceInformation.Connected && Global.VisionSystem.Camera[0].DeviceInformation.GetDevInfo)
                {
                    ControllerApplicationVersion = Global.VisionSystem.Camera[0].DeviceInformation.Firmware;
                }

                for (i = 0; i < VisionSystem.Camera.Count; i++)
                {
                    if (Global.VisionSystem.Camera[i].DeviceInformation.Connected && Global.VisionSystem.Camera[i].DeviceInformation.GetDevInfo)
                    {
                        if (0 > ControllerApplicationVersion.CompareTo(Global.VisionSystem.Camera[i].DeviceInformation.Firmware))
                        {
                            ControllerApplicationVersion = Global.VisionSystem.Camera[i].DeviceInformation.Firmware;
                        }
                    }
                }
            } 
            else//无效
            {
                ControllerApplicationVersion = "";
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：升级UI程序
        // 输入参数：无
        // 输出参数：无
        // 返回值：班次信息
        //----------------------------------------------------------------------
        public static void _UpdateHMIApplication()
        {
            Process process = Process.Start(Global.UpdateApplicationPath + VisionSystemClassLibrary.Struct.System_UIParameter.HMIFileName);//启动程序
        }
    }

    //WndProc消息

    public struct DEVICE_MESSAGE
    {
        public const int WM_DEVICECHANGE = 0x0219;//设备发生变动

        public const int DBT_DEVICEARRIVAL = 0x8000;//系统检测到一个新设备
        public const int DBT_DEVICEREMOVECOMPLETE = 0X8004;//系统完成移除一个设备
    }

    //程序界面类型

    public enum ApplicationInterface
    {
        Work = 1,//WORK页面
        Load = 2,//LOAD页面
        BrandManagement = 3,//BRAND MANAGEMENT页面
        LiveView = 4,//LIVE VIEW页面
        QualityCheck = 6,//QUALITY CHECK页面
        TolerancesSettings = 7,//TOLERANCES SETTINGS页面
        DevicesSetup = 8,//DEVICES SETUP页面
        SystemConfiguration = 9,//SYSTEM页面
        StatisticsView = 10,//STATISTICS页面
    }

    //以太网通信指令类型

    public enum CommunicationInstructionType : byte
    {
        //启动载入，格式：
        Load = 1,//启动载入，格式
        //未完成文件发送
        //服务端->客户端（数据）：指令类型 + 相机类型数据 + 文件传输状态（1，启动发送；2，文件发送中，文件索引值（从2开始））
        //客户端->服务端（文件）：指令类型 + 相机类型数据 + 文件传输状态（1，启动发送；2，文件发送中，文件索引值（从2开始）） + 文件

        //完成文件发送
        //客户端->服务端：指令类型 + 相机类型数据 + 传输结果（1，成功；0，失败）

        //DEVICES SETUP页面操作，格式：
        DevicesSetup_ResetDevice = 3,//DEVICES SETUP页面，点击【RESET DEVICE】按钮，格式：
        //服务端->客户端：指令类型 + 相机类型数据
        //客户端->服务端：指令类型 + 相机类型数据 + 复位设备结果（1，成功；0，失败）

        DevicesSetup_ConfigDevice = 4,//DEVICES SETUP页面，CONFIG DEVICE操作，格式
        //未完成文件发送
        //服务端->客户端（文件）：指令类型 + 相机类型数据 + 设置为的IP地址 + 设置为的相机类型数据 + 设置为的端口 + 设置为的模式 + 设置为的相机故障标记 + 设置为的相机检测使能 + 设置为的相机旋转角度 + 相机颜色 + 相机分辨率 + 是否为串口 + 烟支排列类型 + 设置为的相机数据截取区域缩放 + 设置为的相机数据截取区域缩放后是否居中 + 设置为的相机数据截取区域粘贴区域 + 设置为的相机数据截取区域 + 镜像标记 + 传感器应用场景 + 相机关联信息（关联类型 + 关联数量（相机类型 + 工位索引）） + 文件索引值（从0开始） + 文件
        //客户端->服务端（数据）：指令类型 + 相机类型数据 + 设置为的IP地址 + 设置为的相机类型数据 + 设置为的端口 + 文件索引值（从0开始） + 文件传输状态（1，文件发送中；2，文件发送完成） + 文件接收结果（1，成功；0，失败）

        //完成文件发送
        //服务端->客户端：指令类型 + 相机类型数据 + 设置为的IP地址 + 设置为的相机类型数据 + 设置为的端口 + 设置为的模式 + 设置为的相机故障标记 + 设置为的相机旋转角度 + 相机颜色 + 相机分辨率 + 是否为串口 + 烟支排列类型 + 设置为的相机数据截取区域缩放 + 设置为的相机数据截取区域缩放后是否居中 + 设置为的相机数据截取区域粘贴区域 + 设置为的相机数据截取区域 + 镜像标记 + 传感器应用场景 + 相机关联信息（关联类型 + 关联数量（相机类型 + 工位索引））
        //客户端->服务端：指令类型 + 相机类型数据 + 设置为的IP地址 + 设置为的相机类型数据 + 设置为的端口 + 文件索引值（从0开始） + 文件传输状态（1，文件发送中；2，文件发送完成） + 配置结果（1，成功；0，失败）

        DevicesSetup_TestIOEnter = 50,//DEVICES SETUP页面，TEST I/O操作
        //服务端->客户端：指令类型 + 相机类型数据
        //客户端->服务端：指令类型 + 相机类型数据 + 操作结果（1，成功；0，失败）

        DevicesSetup_TestIO = 5,//DEVICES SETUP页面，TEST I/O操作
        //服务端->客户端：指令类型 + 相机类型数据 + 输出数据
        //客户端->服务端：指令类型 + 相机类型数据 + 输入数据 + 输出诊断

        DevicesSetup_TestIOExit = 6,//DEVICES SETUP页面，退出TEST I/O页面操作:
        //服务端->客户端：指令类型 + 相机类型数据；
        //客户端->服务端：指令类型 + 相机类型数据；

        DevicesSetup_AlignDateTime = 7,//DEVICES SETUP页面，点击【ALIGN DATE/TIME】按钮，格式：
        //服务端->客户端：指令类型 + 相机类型数据 + 日期时间数据
        //客户端->服务端：指令类型 + 相机类型数据 + 日期时间设置结果（1，成功；0，失败）

        DevicesSetup_ParameterSettings = 53,//DEVICES SETUP页面，点击【PARAMETER SETTINGS】按钮，格式：
        //服务端->客户端：指令类型 + 相机类型数据 + 参数个数（Int32） + 参数数组（Int32）
        //客户端->服务端：指令类型 + 相机类型数据 + 设置结果（1，成功；0，失败）



        Work = 2,//WORK页面操作，格式：
        Live = 8,//LIVE VIEW页面操作，格式：
        //服务端->客户端：指令类型 + 相机类型数据 + 图像工具数据（1，包含工具；0，不包含工具） + 图像尺寸类型数据
        //客户端->服务端：指令类型 + 相机类型数据 + 图像工具数据（1，包含工具；0，不包含工具） + 图像尺寸类型数据 + 图像宽度数据 + 图像高度数据  + 图像信息数据 + 图像数据

        //
        //TOLERANCES SETTINGS页面操作，格式：

        TolerancesSettings_Rejects = 13,//查询REJECTS图像
        TolerancesSettings_Live = 14,//查询LIVE图像
        //服务端->客户端：指令类型 + 相机类型数据 + 图像工具数据（1，包含工具；0，不包含工具） + 图像尺寸类型数据
        //客户端->服务端：指令类型 + 相机类型数据 + 图像工具数据（1，包含工具；0，不包含工具） + 图像尺寸类型数据 + 图像宽度数据 + 图像高度数据  + 图像信息数据 + 图像数据

        TolerancesSettings_Graphs = 15,//查询曲线图数据
        //服务端->客户端：指令类型 + 相机类型
        //客户端->服务端：指令类型 + 相机类型 + 公差类数据

        TolerancesSettings_Tool = 16,//工具开关
        //服务端->客户端：指令类型 + 相机类型数据 + 工具索引数值 + 工具开关数值
        //客户端->服务端：指令类型 + 相机类型数据 + 工具开关设置结果（1，成功；0，失败）

        TolerancesSettings_Learn = 17,//学习
        //服务端->客户端：指令类型 + 相机类型数据 + 工具索引数值
        //客户端->服务端：指令类型 + 相机类型数据 + 工具索引数值 + 学习数值 + 学习中的有效数值数量 + 学习中的无效数值数量

        TolerancesSettings_ToolIndex = 59,//双击选中工具
        //服务端->客户端：指令类型 + 相机类型数据 + 工具索引数值
        //客户端->服务端：指令类型 + 相机类型数据 + 工具索引数值 + 设置结果（1，成功；0，失败）

        TolerancesSettings_MinMax = 18,//曲线图范围数值
        //服务端->客户端：指令类型 + 相机类型数据 + 工具索引数值 + 最小值数值（有效值） + 最大值数值（有效值） + 最小值数值（坐标轴数值） + 最大值数值（坐标轴数值）
        //客户端->服务端：指令类型 + 相机类型数据 + 工具索引数值 + 最小值最大值设置结果（1，成功；0，失败）

        TolerancesSettings_ResetGraphs = 19,//复位曲线图
        //服务端->客户端：指令类型 + 相机类型数据
        //客户端->服务端：指令类型 + 相机类型数据 + 复位结果（1，成功；0，失败）

        TolerancesSettings_Enter = 20,//进入页面
        //服务端->客户端：指令类型 + 相机类型数据
        //客户端->服务端：指令类型 + 相机类型数据 + 操作结果（1，成功；0，失败）

        TolerancesSettings_SaveProduct = 21,//保存数据
        //服务端->客户端：指令类型 + 相机类型数据 + 是否保存数据（1，是；0，否）
        //客户端->服务端：指令类型 + 相机类型数据 + 是否保存数据（1，是；0，否） + 保存数据结果（1，成功；0，失败）

        TolerancesSettings_EjectLevel = 51,//灵敏度
        //服务端->客户端：指令类型 + 相机类型数据 + 灵敏度 + 调节灵敏度标记（0,：true;1：调节光电空头校准值）
        //客户端->服务端：指令类型 + 相机类型数据 + 灵敏度 + 调节灵敏度标记（0,：true;1：调节光电空头校准值） + 公差个数 + （每个）公差下限、上限

        //
        //BRAND MANAGEMENT页面操作，格式：
        BrandManagement_LoadBrand = 22,//载入品牌（RESTORE BRANDS页面，恢复品牌时，载入当前品牌）
        //未完成文件发送
        //服务端->客户端（文件）：指令类型 + 相机类型数据 + 设置相机模式 + 品牌名称长度 + 品牌名称 + 文件索引值（从0开始） + 文件
        //客户端->服务端（数据）：指令类型 + 相机类型数据 + 设置相机模式 + 品牌名称长度 + 品牌名称 + 文件索引值（从0开始） + 文件传输状态（1，文件发送中；2，文件发送完成） + 文件接收结果（1，成功；0，失败）

        //完成文件发送
        //服务端->客户端：指令类型 + 相机类型数据 + 设置相机模式 + 品牌名称长度 + 品牌名称
        //客户端->服务端：指令类型 + 相机类型数据 + 设置相机模式 + 品牌名称长度 + 品牌名称 + 文件索引值（从0开始） + 文件传输状态（1，文件发送中；2，文件发送完成） + 配置结果（1，成功；0，失败）


        //
        //LIVE VIEW页面操作
        Live_SelfTrigger = 23,//SELF TRIGGER，格式：
        //服务端->客户端：指令类型 + 相机类型 + 操作数据（1，打开；0，关闭）
        //客户端->服务端：指令类型 + 相机类型 + 操作结果（1，成功；0，失败）


        //
        //QUALITY CHECK页面操作
        QualityCheck_SaveProduct = 24,//保存数据参数，格式：
        //服务端->客户端：指令类型 + 相机类型数据 + 是否保存数据（1，是；0，否）
        //客户端->服务端：指令类型 + 相机类型数据 + 保存数据结果（1，成功；0，失败）

        QualityCheck_LearnSample = 25,//自学习阈值或更新，格式：
        //服务端->客户端：指令类型 + 相机类型数据 + 图像工具数据（1，包含工具；0，不包含工具） + 图像尺寸类型数据  + 图像类型（1，在线；2，学习；3，剔除）
        //客户端->服务端：指令类型 + 相机类型数据 + 图像工具数据（1，包含工具；0，不包含工具） + 图像尺寸类型数据 + 图像宽度数据 + 图像高度数据 + 图像信息数据 + 图像数据

        QualityCheck_TolerancesValue = 52,//图像学习完成后，获取公差上下限
        //服务端->客户端：指令类型 + 相机类型数据
        //客户端->服务端：指令类型 + 相机类型数据 + 公差个数 + （每个）公差下限、上限

        QualityCheck_LoadSample = 47,//自学习阈值或更新，格式：
        //服务端->客户端：指令类型 + 相机类型数据 + 图像工具数据（1，包含工具；0，不包含工具） + 图像尺寸类型数据
        //客户端->服务端：指令类型 + 相机类型数据 + 图像工具数据（1，包含工具；0，不包含工具） + 图像尺寸类型数据 + 图像宽度数据 + 图像高度数据 + 图像信息数据 + 图像数据

        QualityCheck_GetSelectedRecordData = 27,//QualityCheck页面，获取当前选择的统计数据
        //服务端->客户端：指令类型 + 相机类型数据 + 关联标记 + 统计数据类型（0，最新统计数据；1，指定统计数据）+ 班次索引（非0） + 统计数据开始结束时间
        //客户端->服务端：指令类型 + 相机类型数据 + 关联标记 + 统计数据类型（0，最新统计数据；1，指定统计数据） + 班次索引（非0） + 统计数据开始结束时间  +  剔除数量

        QualityCheck_LiveView = 26,//查看实时图像，格式：
        //服务端->客户端：指令类型 + 相机类型数据 + 图像工具数据（1，包含工具；0，不包含工具） + 图像尺寸类型数据 + 工具索引 + 图像类型（1，在线；2，学习；3，剔除）
        //客户端->服务端：指令类型 + 相机类型数据 + 图像工具数据（1，包含工具；0，不包含工具） + 图像尺寸类型数据 + 图像宽度数据 + 图像高度数据 + 图像信息数据 + 图像数据

        QualityCheck_LoadReject_Click = 28,//查看选定的图像，格式：
        //服务端->客户端：指令类型 + 相机类型数据 + 关联标记 + 班次索引（非0） + 统计数据开始结束时间 + 剔除图像对应的工具索引值（从0开始） + 剔除图像对应的工具中的索引值（从0开始） + 图像尺寸类型数据
        //客户端->服务端：指令类型 + 相机类型数据 + 关联标记 + 班次索引（非0） + 统计数据开始结束时间 + 剔除图像对应的工具索引值（从0开始） + 剔除图像对应的工具中的索引值（从0开始） + 图像尺寸类型数据 + 图像宽度数据 + 图像高度数据 + 图像信息数据 + 图像数据

        QualityCheck_ManageTools = 29,//工具管理，格式：
        //服务端->客户端：指令类型 + 相机类型数据 + 启用工具标记 + 当前工具索引 + 图像类型（1，在线；2，学习；3，剔除）
        //客户端->服务端：指令类型 + 相机类型数据 + 启用工具标记更新结果（1，成功；0，不成功） + 图像信息数据

        QualityCheck_ToolParamter = 30,//工具参数，格式：
        //服务端->客户端：指令类型 + 相机类型数据 + 工具索引 + 图像类型（1，在线；2，学习；3，剔除） + 工具参数
        //客户端->服务端：指令类型 + 相机类型数据 + 工具参数更新结果（1，成功；0，不成功） + 图像信息数据

        QualityCheck_CurrentTool = 46,//当前工具设置，格式：
        //服务端->客户端：指令类型 + 相机类型数据 + 工具索引 + 图像类型（1，在线；2，学习；3，剔除）
        //客户端->服务端：指令类型 + 相机类型数据 + 当前工具设置结果（1，成功；0，不成功） + 图像信息数据

        QualityCheck_WorkArea = 31,//工作区域，格式：
        //服务端->客户端：指令类型 + 相机类型数据 + 工具索引 + 图像类型（1，在线；2，学习；3，剔除） + 工作区域
        //客户端->服务端：指令类型 + 相机类型数据 + 设置结果（1，成功；0，不成功） + 图像信息数据

        QualityCheck_Enter = 32,//进入页面
        //服务端->客户端：指令类型 + 相机类型数据 + 工具索引
        //客户端->服务端：指令类型 + 相机类型数据 + 操作结果（1，成功；0，失败）

        //
        //标题栏【STATE】按钮操作，设备状态，格式：
        TitleBar_State = 33,
        //服务端->客户端：指令类型 + 相机类型数据 + 设备状态（VisionSystemClassLibrary.Enum.DeviceState）
        //客户端->服务端：指令类型 + 相机类型数据 + 设置结果（1，成功；0，不成功）

        //
        //客户端系统升级，格式：
        ClientSystem_Update = 34,
        //服务端->客户端：指令类型 + 相机类型数据 + 设置相机模式 + 文件
        //客户端->服务端：指令类型 + 相机类型数据 + 传输结果（1，成功；0，失败）

        //
        //网络检测，格式：
        Network_Check = 35,
        //服务端->客户端：指令类型 + 数据（上位机，1；下位机，2）
        //客户端->服务端：指令类型 + 数据（上位机，1；下位机，2）

        //

        DevicesSetup_ConfigImage_Save = 36,//DEVICES SETUP页面，保存数据参数，格式：
        //服务端->客户端：指令类型 + 相机类型数据 + 是否保存数据（1，是；0，否）
        //客户端->服务端：指令类型 + 相机类型数据 + 是否保存数据（1，是；0，否） + 保存数据结果（1，成功；0，失败）

        DevicesSetup_ConfigImage_Focus = 37,//DEVICES SETUP页面，相机聚焦，格式：
        //服务端->客户端：指令类型 + 相机类型数据 + 聚焦参数
        //客户端->服务端：指令类型 + 相机类型数据 + 聚焦参数更新结果（1，成功；0，不成功）

        DevicesSetup_ConfigImage_White = 38,//DEVICES SETUP页面，白平衡，格式：
        //服务端->客户端：指令类型 + 相机类型数据 + 白平衡参数
        //客户端->服务端：指令类型 + 相机类型数据 + 白平衡参数更新结果（1，成功；0，不成功）

        DevicesSetup_ConfigImage_Parameter = 39,//DEVICES SETUP页面，相机参数，格式：
        //服务端->客户端：指令类型 + 相机类型数据 + 光照时间 + 光照强度 + 增益 + 曝光时间 + 白平衡 + 白平衡（红） + 白平衡（绿） + 白平衡（蓝）
        //客户端->服务端：指令类型 + 相机类型数据 + 相机参数更新结果（1，成功；0，不成功） + 白平衡（红） + 白平衡（绿） + 白平衡（蓝）

        DevicesSetup_ConfigSensor_Parameter = 57,//DEVICES SETUP页面，传感器参数，格式：
        //服务端->客户端：指令类型 + 相机类型数据 + 烟支数量（N）+ 传感器校准选中状态 + 校准标记（0，未校准；1，校准中） + 烟支校准值（N支）
        //客户端->服务端：指令类型 + 相机类型数据 + 烟支数量（N）+ 传感器校准过程标记（1，校准过程中；0，校准结束或未校准、取消校准） + 相机参数更新结果（1，成功；0，不成功） + 烟支校准值（N支）

        DevicesSetup_ConfigSensor_MaxADC = 58,//DEVICES SETUP页面，最大电压值，格式：
        //服务端->客户端：指令类型 + 相机类型数据 + 烟支数量（N）
        //客户端->服务端：指令类型 + 相机类型数据 + 烟支数量（N）+ 最大电压查询标记（1，查询过程中；0，查询结束） + 最大电压值（N支）

        DevicesSetup_ConfigImage_Enter = 40,//进入DEVICES SETUP页面
        //服务端->客户端：指令类型 + 相机类型数据
        //客户端->服务端：指令类型 + 相机类型数据 + 操作结果（1，成功；0，失败）

        DevicesSetup_ConfigImage_Live = 41,//DEVICES SETUP页面实时图像显示
        //服务端->客户端：指令类型 + 相机类型数据 + 图像工具数据（1，包含工具；0，不包含工具） + 图像尺寸类型数据
        //客户端->服务端：指令类型 + 相机类型数据 + 图像工具数据（1，包含工具；0，不包含工具） + 图像尺寸类型数据 + 图像宽度数据 + 图像高度数据 + 图像信息数据 + 图像数据

        //

        CurrentFaultMessage = 48,//FAULT MESSAGE，获取当前故障信息
        //服务端->客户端：指令类型 + 相机类型数据
        //客户端->服务端：指令类型 + 相机类型数据 + 故障信息（时间 + 故障代码索引值） + 机器速度/相位标志（1，速度；0，相位） + 机器速度/相位数值

        GetFaultMessages = 42,//FAULT MESSAGE，获取故障信息
        //服务端->客户端：指令类型 + 相机类型数据
        //客户端->服务端：指令类型 + 相机类型数据 + 故障信息个数 + 故障信息数组（时间 + 故障代码索引值）

        ClearAllFaultMessages = 43,//FAULT MESSAGE，清除所有故障信息
        //服务端->客户端：指令类型 + 相机类型数据
        //客户端->服务端：指令类型 + 相机类型数据 + 清除结果（1，成功；0，不成功）

        SetFaultMessageState = 49,//FAULT MESSAGE状态
        //服务端->客户端：指令类型 + 相机类型数据 + 故障信息使能状态
        //客户端->服务端：指令类型 + 相机类型数据 + 设置结果（1，成功；0，不成功）


        //

        //
        //系统参数设置（相机选择、班次），格式：
        SystemParameter = 44,
        //服务端->客户端：指令类型 + 相机类型数据 + 设置相机模式 + 相机选择（1，启用；0，禁用） + 相机检测使能（1，启用；0，禁用） + 烟支排列类型 + 机器类型 + 班次数据（文件）
        //客户端->服务端：指令类型 + 相机类型数据 + 设置结果（1，成功；0，不成功）

        //
        //系统状态同步，格式：
        DeviceState_Synchronization = 45,
        //服务端->客户端：指令类型 + 相机类型数据 + 设置相机模式 + 当前选择机器 + 设备状态（VisionSystemClassLibrary.Enum.DeviceState） + 故障信息使能状态 + 品牌长度 + 品牌名称 + 班次数据（文件）
        //客户端->服务端：指令类型 + 相机类型数据 + 设置结果（1，成功；0，不成功）

        Statistics_GetSelectedRecordData = 100,//STATISTICS页面，获取当前选择的统计数据
        //服务端->客户端：指令类型 + 相机类型数据 + 关联标记 + 统计数据类型（0，最新统计数据；1，指定统计数据） + 班次索引（非0） + 统计数据开始结束时间
        //客户端->服务端：指令类型 + 相机类型数据 + 关联标记 + 统计数据类型（0，当前班；1，历史班） + 班次索引（非0） + 统计数据开始结束时间 + 统计数据（品牌名称（包括品牌长度） + 已检测数量 + 合格数量 + 剔除数量 + 工具数量 + 工具统计信息）

        Statistics_UpdateSelectedRecordData = 101,//STATISTICS页面，更新当前选择的统计数据
        //服务端->客户端：指令类型 + 相机类型数据 + 关联标记 + 班次索引（非0） + 统计数据开始结束时间 + 剔除图像对应的工具索引值（从0开始） + 剔除图像对应的工具中的索引值（从0开始） + 图像尺寸类型数据
        //客户端->服务端：指令类型 + 相机类型数据 + 关联标记 + 统计数据类型（0，当前班；1，历史班） + 班次索引（非0） + 统计数据开始结束时间 + 统计数据（品牌名称（包括品牌长度） + 已检测数量 + 合格数量 + 剔除数量 + 工具数量 + 工具统计信息）

        Statistics_ClickRejectsListItem = 102,//STATISTICS页面，查看剔除图像，格式：
        //服务端->客户端：指令类型 + 相机类型数据 + 关联标记 +  班次索引（非0） + 统计数据开始结束时间 + 剔除图像对应的工具索引值（从0开始） + 剔除图像对应的工具中的索引值（从0开始） + 图像尺寸类型数据
        //客户端->服务端：指令类型 + 相机类型数据 + 关联标记 + 班次索引（非0） + 统计数据开始结束时间 + 剔除图像对应的工具索引值（从0开始） + 剔除图像对应的工具中的索引值（从0开始） + 图像尺寸类型数据 + 图像宽度数据 + 图像高度数据 + 图像信息数据 + 图像数据

        Statistics_GetRecordList = 103,//STATISTICS页面，获取统计数据列表
        //服务端->客户端：指令类型 + 相机类型数据
        //客户端->服务端：指令类型 + 相机类型数据 + 统计数据列表（班次个数 + 当前班次索引（非0） + 统计数据个数 + 当前统计数据索引（从0开始） + （每个班次）统计数据个数 + （每个班次，每个统计数据）班次开始结束时间 + （每个班次，每个统计数据）品牌（包括品牌长度））

        Statistics_DeleteRecord = 104,//STATISTICS页面，删除统计数据
        //服务端->客户端：指令类型 + 相机类型数据 + 删除统计数据方式（0，删除所有；1，删除指定班次；2，删除指定记录）
        //             （0） + 无
        //             （1） + 待删除的指定班次个数 + 班次索引值数组（从0开始）
        //             （2） + 班次索引值（从0开始） + 待删除的指定记录个数 + 记录开始结束时间数组
        //客户端->服务端：指令类型 + 相机类型数据 + 删除数据结果（1，成功；0，不成功）

        //
        //网络检测，格式：
        NetCheck_ConnectCamera = 200,
        //服务端->客户端：指令类型 + 相机类型数据
        //客户端->服务端：指令类型 + 相机类型数据 + 相机在线状态（1，成功；0，失败）

        TestMode_Light = 54,//测试程序，光源：
        //服务端->客户端：指令类型 + 相机类型数据 + 电流（UInt32，mA）；
        //客户端->服务端：指令类型 + 相机类型数据；

        TestMode_CameraPort = 55,//测试程序，相机端口：
        //服务端->客户端：指令类型 + 相机类型数据；
        //客户端->服务端：指令类型 + 相机类型数据 + 成功/失败（Int32，1/0）；

        TestMode_CameraTrigger = 56,//测试程序，相机触发：
        //服务端->客户端：指令类型 + 相机类型数据 + 开启/关闭（UInt32，1/0）；
        //客户端->服务端：指令类型 + 相机类型数据；
    }
}