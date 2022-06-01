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

using System.Threading;

using System.Diagnostics;

using System.IO;

using System.Management;

using System.Reflection;

namespace VisionSystemUserInterface
{
    public partial class Load : Form
    {
        //LOAD页面

        private VisionSystemControlLibrary.ApplicationRegistrationWindow applicationRegistrationWindow = new VisionSystemControlLibrary.ApplicationRegistrationWindow();//注册窗口

        private delegate void StartupConrol_ProgressBar_PerformStep();//定义委托（用于跨线程的控件访问），loadControl._ProgressBar_PerformStep()
        private delegate void Load_Dispose();//定义委托（用于跨线程的控件访问），this.Dispose()

        private Int32 InitState;//初始化标记
        private String sProductKey;//秘钥

        //构造函数

        //----------------------------------------------------------------------
        // 功能说明：构造函数，初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Load()
        {
            InitializeComponent();

            //

            InitState = 0;

            applicationRegistrationWindow.WindowClose += new System.EventHandler(applicationRegistrationWindow_WindowClose);//订阅事件
        }

        //属性

        //----------------------------------------------------------------------
        // 功能说明：CustomControl属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public VisionSystemControlLibrary.LoadControl CustomControl//属性
        {
            get//读取
            {
                return loadControl;
            }
        }

        //函数

        //----------------------------------------------------------------------
        // 功能说明：加载数据
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _LoadData()
        {
            InitState = 1;

            timer1.Enabled = true;
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：更新进度条
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _ProgressBar_PerformStep()
        {
            if (0 == Global.Data_Value)
            {
                customProgressBar._PerformStep();//更新进度条
            }
            else
            {
                loadControl._ProgressBar_PerformStep();//更新进度条
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
        private void Load_Load(object sender, EventArgs e)
        {
            Global.CurrentInterface = ApplicationInterface.Load;//当前页面，LOAD

            //

            if (0 == Global.Data_Value)
            {
                loadControl.Visible = false;

                BackColor = Color.White;

                label1.Text = VisionSystemControlLibrary.WorkControl._GetProductFullName(Global.VisionSystem.MachineType[Global.VisionSystem.SelectedMachineType], Global.VisionSystem.ProductName, Global.VisionSystem.ProductModelNumber);//设备名称

                switch (VisionSystemClassLibrary.Class.System.Language)//语言
                {
                    case VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese://中文

                        BackgroundImage = Global.Load_Background_Chinese;//

                        label2.Text = "版本：" + Global.HMIApplicationVersion;//程序版本

                        //label1.Location = new System.Drawing.Point(20, 249);
                        //label2.Location = new System.Drawing.Point(57, 593);

                        //customProgressBar.Location = new System.Drawing.Point(354, 495);

                        break;
                    case VisionSystemClassLibrary.Enum.InterfaceLanguage.English://英文

                        BackgroundImage = Global.Load_Background_English;//

                        label2.Text = "Version: " + Global.HMIApplicationVersion;//程序版本

                        //label1.Location = new System.Drawing.Point(20, 259);
                        //label2.Location = new System.Drawing.Point(42, 593);

                        //customProgressBar.Location = new System.Drawing.Point(344, 480);

                        break;
                    default://默认中文

                        BackgroundImage = Global.Load_Background_Chinese;//

                        label2.Text = "版本：" + Global.HMIApplicationVersion;//程序版本

                        //label1.Location = new System.Drawing.Point(20, 249);
                        //label2.Location = new System.Drawing.Point(57, 593);

                        //customProgressBar.Location = new System.Drawing.Point(354, 495);

                        break;
                }

                customProgressBar.Minimum = 0;//进度条控件范围的最小值
                customProgressBar.Maximum = 100;//进度条控件范围的最大值
                customProgressBar.StepNumber = 10;//进度条控件步进数量                
            }
            else
            {
                customProgressBar.Visible = false;

                label1.Visible = false;
                label2.Visible = false;
            }

            Update();

            //

            loadControl.DeviceName = VisionSystemControlLibrary.WorkControl._GetProductFullName(Global.VisionSystem.MachineType[Global.VisionSystem.SelectedMachineType], Global.VisionSystem.ProductName, Global.VisionSystem.ProductModelNumber);//设备名称
            loadControl.AppVersion = Global.HMIApplicationVersion;//程序版本
            loadControl.ProgressBarMinimum = 0;//进度条控件范围的最小值
            loadControl.ProgressBarMaximum = 100;//进度条控件范围的最大值
            loadControl.ProgressBarStepNumber = 10;//进度条控件步进数量
            loadControl.Language = VisionSystemClassLibrary.Class.System.Language;//设置语言
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：注册窗口关闭事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void applicationRegistrationWindow_WindowClose(object sender, EventArgs e)
        {
            if (1 == (Int32)applicationRegistrationWindow.ApplicationRegistrationControl.ControlData)//注册窗口
            {
                applicationRegistrationWindow.Dispose();
            }
            else//其它
            {
                //不执行操作
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：Load界面定时初始化
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;

            if (InitState == 1) 
            {
                Global.StartupWindow.TopMost = false;
                Global.StartupWindow.Visible = false;

                //检查产品密钥

                String sSerialNumber = _GetProductKey(_GetHardDiskSerialNumber());//产品序列号

                sProductKey = _GetProductKey(sSerialNumber);//产品密钥

                String sProductKeyPath = Global.VisionSystem.ConfigDataPath + "ProductKey.dat";//ProductKey.dat文件路径

                Boolean bProductKeyOK = false;//产品密钥是否有效。取值范围：true，是；false，否

                if (_CheckProductKey(sProductKeyPath, sProductKey))//检查产品密钥
                {
                    bProductKeyOK = true;
                }
                else//无效，需要用户输入
                {
                    applicationRegistrationWindow.BitmapTrademark = VisionSystemClassLibrary.Class.System.Trademark_1[Global.Data_Value];//商标
                    applicationRegistrationWindow.ApplicationRegistrationControl.Language = VisionSystemClassLibrary.Class.System.Language;//语言
                    applicationRegistrationWindow.DeviceName = VisionSystemControlLibrary.WorkControl._GetProductFullName(Global.VisionSystem.MachineType[Global.VisionSystem.SelectedMachineType], Global.VisionSystem.ProductName, Global.VisionSystem.ProductModelNumber);//标题
                    applicationRegistrationWindow.ApplicationRegistrationControl.SerialNumber = sSerialNumber;//产品密钥
                    applicationRegistrationWindow.ApplicationRegistrationControl.ProductKey = sProductKey;//产品密钥
                    applicationRegistrationWindow.ApplicationRegistrationControl.ControlData = 1;//预留数据

                    applicationRegistrationWindow.StartPosition = FormStartPosition.CenterScreen;//位置

                    applicationRegistrationWindow.TopMost = true;//将窗口置于顶层

                    applicationRegistrationWindow.ShowDialog();//显示注册窗口

                    //

                    if (applicationRegistrationWindow.ApplicationRegistrationControl.RegisteredSuccessfully)//成功
                    {
                        bProductKeyOK = true;

                        //

                        _CreateProductKey(sProductKeyPath, sProductKey);//写入
                    }
                }

                //

                if (bProductKeyOK)//密钥有效
                {
                    InitState++;
                    timer1.Enabled = true;
                }
                else//密钥无效
                {
                    Global.WorkWindow.Dispose();
                }                
            }
            else if (InitState == 2)
            {
                //初始化WORK页面

                Global.WorkWindow.TitleBar.UserPassword = Global.VisionSystem.UserPassword;//用户密码
                Global.WorkWindow.TitleBar.AdministratorPassword = Global.VisionSystem.AdministratorPassword;//管理员密码
                Global.WorkWindow.TitleBar.Language = VisionSystemClassLibrary.Class.System.Language;//页面语言
                Global.WorkWindow.TitleBar.Caption = VisionSystemControlLibrary.WorkControl._GetProductFullName(Global.VisionSystem.MachineType[Global.VisionSystem.SelectedMachineType], Global.VisionSystem.ProductName, Global.VisionSystem.ProductModelNumber);//标题文本
                Global.WorkWindow.TitleBar.CurrentMachineType = Global.VisionSystem.MachineType[Global.VisionSystem.SelectedMachineType];//机器类型
                Global.WorkWindow.TitleBar.CurrentBrand = Global._GetCURRENTBrandName();//当前品牌
                Global.WorkWindow.TitleBar.SystemDeviceState = VisionSystemClassLibrary.Class.System.SystemDeviceState;//设备状态

                if (0 < Global.VisionSystem.Work.ConnectedCameraNumber)//存在连接的设备
                {
                    Global.WorkWindow.TitleBar.StateShow = true;//显示
                }
                else//不存在连接的设备
                {
                    Global.WorkWindow.TitleBar.StateShow = false;//显示
                }

                //

                Global.WorkWindow.CustomControl.BitmapBackground = VisionSystemClassLibrary.Class.System.DeviceBackground;//背景图像
                Global.WorkWindow.CustomControl.BitmapTrademarkLeft = VisionSystemClassLibrary.Class.System.Trademark_2[Global.Data_Value];//商标
                Global.WorkWindow.CustomControl.BitmapTrademarkRight = VisionSystemClassLibrary.Class.System.Trademark_2[Global.Data_Value];//商标
                Global.WorkWindow.CustomControl.AppProductKey = sProductKey;//版本
                Global.WorkWindow.CustomControl.AppVersion = Global.HMIApplicationVersion;//版本
                Global.WorkWindow.CustomControl._Properties(Global.VisionSystem);//初始化WorkControl控件
                Global.WorkWindow.CustomControl.Language = VisionSystemClassLibrary.Class.System.Language;//设置页面语言
                Global.WorkWindow.CustomControl.Data_Value = Global.Data_Value;

                Global.WorkWindow.Update();

                InitState++;
                timer1.Enabled = true;
            }
            else if (InitState == 3)
            {
                //其它页面

                Global.SystemConfigurationWindow = new VisionSystemUserInterface.SystemConfiguration();//SYSTEM窗口
                Global.SystemConfigurationWindow.CustomControl.Language = VisionSystemClassLibrary.Class.System.Language;//设置页面语言
                _ProgressBar_PerformStep();//更新进度条
                Update();//重绘
                
                InitState++;
                timer1.Enabled = true;
            }
            else if (InitState == 4)
            {
                Global.DevicesSetupWindow = new VisionSystemUserInterface.DevicesSetup();//DEVICES SETUP窗口
                Global.DevicesSetupWindow.CustomControl.Language = VisionSystemClassLibrary.Class.System.Language;//设置页面语言
                _ProgressBar_PerformStep();//更新进度条
                Update();//重绘
                
                InitState++;
                timer1.Enabled = true;
            }
            else if (InitState == 5)
            {
                Global.BrandManagementWindow = new VisionSystemUserInterface.BrandManagement();//BRAND MANAGEMENT窗口
                Global.BrandManagementWindow.CustomControl.Language = VisionSystemClassLibrary.Class.System.Language;//设置页面语言
                _ProgressBar_PerformStep();//更新进度条
                Update();//重绘
                
                InitState++;
                timer1.Enabled = true;
            }
            else if (InitState == 6)
            {
                Global.BackupBrandsWindow = new VisionSystemUserInterface.BackupBrands();//BRAND MANAGEMENT，BACKUP BRANDS窗口
                Global.BackupBrandsWindow.CustomControl.Language = VisionSystemClassLibrary.Class.System.Language;//设置页面语言
                _ProgressBar_PerformStep();//更新进度条
                Update();//重绘
                
                InitState++;
                timer1.Enabled = true;
            }
            else if (InitState == 7)
            {
                Global.RestoreBrandsWindow = new VisionSystemUserInterface.RestoreBrands();//BRAND MANAGEMENT，RESTORE BRANDS窗口
                Global.RestoreBrandsWindow.CustomControl.Language = VisionSystemClassLibrary.Class.System.Language;//设置页面语言
                _ProgressBar_PerformStep();//更新进度条
                Update();//重绘
                
                InitState++;
                timer1.Enabled = true;
            }
            else if (InitState == 8)
            {
                Global.TolerancesSettingsWindow = new VisionSystemUserInterface.TolerancesSettings();//TOLERANCES SETTINGS窗口
                Global.TolerancesSettingsWindow.CustomControl.Language = VisionSystemClassLibrary.Class.System.Language;//设置页面语言
                _ProgressBar_PerformStep();//更新进度条
                Update();//重绘
                
                InitState++;
                timer1.Enabled = true;
            }
            else if (InitState == 9)
            {
                Global.LiveViewWindow = new VisionSystemUserInterface.LiveView();//LIVE VIEW窗口
                Global.LiveViewWindow.CustomControl.BitmapBackground = VisionSystemClassLibrary.Class.System.DeviceBackground;//背景图像
                Global.LiveViewWindow.CustomControl.Language = VisionSystemClassLibrary.Class.System.Language;//设置页面语言
                _ProgressBar_PerformStep();//更新进度条
                Update();//重绘
                
                InitState++;
                timer1.Enabled = true;
            }
            else if (InitState == 10)
            {
                Global.RejectsViewWindow = new VisionSystemUserInterface.RejectsView();//REJECTS VIEW窗口
                Global.RejectsViewWindow.CustomControl.Language = VisionSystemClassLibrary.Class.System.Language;//设置页面语言
                _ProgressBar_PerformStep();//更新进度条
                Update();//重绘
                
                InitState++;
                timer1.Enabled = true;
            }
            else if (InitState == 11)
            {
                Global.QualityCheckWindow = new VisionSystemUserInterface.QualityCheck();//QUALITY CHECK窗口
                Global.QualityCheckWindow.CustomControl.Language = VisionSystemClassLibrary.Class.System.Language;//设置页面语言
                _ProgressBar_PerformStep();//更新进度条
                Update();//重绘

                InitState++;
                timer1.Enabled = true;
            }
            else if (InitState == 12)
            {
                Global.ImageConfigurationWindow = new VisionSystemUserInterface.ImageConfiguration();//IMAGE CONFIGURATION窗口
                Global.ImageConfigurationWindow.CustomControl.Language = VisionSystemClassLibrary.Class.System.Language;//设置页面语言
                _ProgressBar_PerformStep();//更新进度条
                Update();//重绘
               
                InitState++;
                timer1.Enabled = true;
            }
            else if (InitState == 13)
            {
                Global.StatisticsViewWindow = new VisionSystemUserInterface.StatisticsView();//STATISTICS VIEW窗口
                Global.StatisticsViewWindow.CustomControl.Language = VisionSystemClassLibrary.Class.System.Language;//设置页面语言
                _ProgressBar_PerformStep();//更新进度条
                Update();//重绘
                
                InitState++;
                timer1.Enabled = true;
            }
            else if (InitState == 14)
            {
                _ProgressBar_PerformStep();//更新进度条
                Update();//重绘

                //

                VisionSystemControlLibrary.GlobalWindows.TopMostWindows = Global.TopMostWindows;

                //

                //此处显示为了调用控件的Load函数创建控件，否则会产生错误

                Global.DevicesSetupWindow.Visible = true;//显示DEVICES SETUP窗口
                Global.DevicesSetupWindow.Update();//

                InitState++;
                timer1.Enabled = true;
            }
            else if (InitState == 15)
            {
                Global.ImageConfigurationWindow.Visible = true;//显示IMAGE CONFIGURATION窗口
                Global.ImageConfigurationWindow.Update();//
                                
                InitState++;
                timer1.Enabled = true;
            }
            else if (InitState == 16)
            {
                Global.BrandManagementWindow.Visible = true;//显示BRAND MANAGEMENT窗口
                Global.BrandManagementWindow.Update();//

                InitState++;
                timer1.Enabled = true;
            }
            else if (InitState == 17)
            {
                Global.BackupBrandsWindow.Visible = true;//BRAND MANAGEMENT，BACKUP BRANDS页面
                Global.BackupBrandsWindow.Update();//

                InitState++;
                timer1.Enabled = true;
            }
            else if (InitState == 18)
            {
                Global.RestoreBrandsWindow.Visible = true;//显示BRAND MANAGEMENT，RESTORE BRANDS窗口
                Global.RestoreBrandsWindow.Update();//
                
                InitState++;
                timer1.Enabled = true;
            }
            else if (InitState == 19)
            {
                Global.TolerancesSettingsWindow.Visible = true;//显示TOLERANCES SETTINGS窗口
                Global.TolerancesSettingsWindow.Update();//
                
                InitState++;
                timer1.Enabled = true;
            }
            else if (InitState == 20)
            {
                Global.LiveViewWindow.Visible = true;//显示LIVE VIEW窗口
                Global.LiveViewWindow.Update();//

                InitState++;
                timer1.Enabled = true;
            }
            else if (InitState == 21)
            {
                Global.RejectsViewWindow.Visible = true;//显示REJECTS VIEW窗口
                Global.RejectsViewWindow.Update();//
                
                InitState++;
                timer1.Enabled = true;
            }
            else if (InitState == 22)
            {
                Global.QualityCheckWindow.Visible = true;//显示QUALITY CHECK窗口
                Global.QualityCheckWindow.Update();//

                InitState++;
                timer1.Enabled = true;
            }
            else if (InitState == 23)
            {
                Global.StatisticsViewWindow.Visible = true;//显示STATISTICS VIEW窗口
                Global.StatisticsViewWindow.Update();//

                InitState++;
                timer1.Enabled = true;
            }
            else if (InitState == 24)
            {
                Global.SystemConfigurationWindow.Visible = true;//显示SYSTEM窗口
                Global.SystemConfigurationWindow.Update();//
                

                InitState++;
                timer1.Enabled = true;
            }
            else if (InitState == 25)
            {
                VisionSystemControlLibrary.GlobalWindows.DigitalKeyboard_Window.Visible = true;//显示DigitalKeyboard窗口
                VisionSystemControlLibrary.GlobalWindows.DigitalKeyboard_Window.Location = Global.WorkWindow.Location;
                VisionSystemControlLibrary.GlobalWindows.DigitalKeyboard_Window.Update();//

                InitState++;
                timer1.Enabled = true;
            }
            else if (InitState == 26)
            {
                VisionSystemControlLibrary.GlobalWindows.StandardKeyboard_Window.Visible = true;//显示StandardKeyboard窗口
                VisionSystemControlLibrary.GlobalWindows.StandardKeyboard_Window.Location = Global.WorkWindow.Location;
                VisionSystemControlLibrary.GlobalWindows.StandardKeyboard_Window.Update();//

                InitState++;
                timer1.Enabled = true;
            }
            else if (InitState == 27)
            {
                VisionSystemControlLibrary.GlobalWindows.MessageDisplay_Window.Visible = true;//显示MessageDisplay窗口
                VisionSystemControlLibrary.GlobalWindows.MessageDisplay_Window.Location = Global.WorkWindow.Location;
                VisionSystemControlLibrary.GlobalWindows.MessageDisplay_Window.Update();//

                InitState++;
                timer1.Enabled = true;
            }
            else if (InitState == 28)
            {
                VisionSystemControlLibrary.GlobalWindows.DeviceConfiguration_Window.Visible = true;//显示DeviceConfiguration窗口
                VisionSystemControlLibrary.GlobalWindows.DeviceConfiguration_Window.Location = Global.WorkWindow.Location;
                VisionSystemControlLibrary.GlobalWindows.DeviceConfiguration_Window.Update();//

                InitState++;
                timer1.Enabled = true;
            }
            else if (InitState == 29)
            {
                VisionSystemControlLibrary.GlobalWindows.IOSignalDiagnosis_Window.Visible = true;//显示IOSignalDiagnosis窗口
                VisionSystemControlLibrary.GlobalWindows.IOSignalDiagnosis_Window.Location = Global.WorkWindow.Location;
                VisionSystemControlLibrary.GlobalWindows.IOSignalDiagnosis_Window.Update();//
                
                InitState++;
                timer1.Enabled = true;
            }
            else if (InitState == 30)
            {
                VisionSystemControlLibrary.GlobalWindows.DateTimePanel_Window.Visible = true;//日期时间面板窗口
                VisionSystemControlLibrary.GlobalWindows.DateTimePanel_Window.Location = Global.WorkWindow.Location;
                VisionSystemControlLibrary.GlobalWindows.DateTimePanel_Window.Update();//

                InitState++;
                timer1.Enabled = true;
            }
            else if (InitState == 31)
            {
                VisionSystemControlLibrary.GlobalWindows.ShiftConfiguration_Window.Visible = true;//班次设置窗口
                VisionSystemControlLibrary.GlobalWindows.ShiftConfiguration_Window.Location = Global.WorkWindow.Location;
                VisionSystemControlLibrary.GlobalWindows.ShiftConfiguration_Window.Update();//

                InitState++;
                timer1.Enabled = true;
            }
            else if (InitState == 32)
            {
                VisionSystemControlLibrary.GlobalWindows.StatisticsRecord_Window.Visible = true;//统计记录窗口
                VisionSystemControlLibrary.GlobalWindows.StatisticsRecord_Window.Location = Global.WorkWindow.Location;
                VisionSystemControlLibrary.GlobalWindows.StatisticsRecord_Window.Update();//

                InitState++;
                timer1.Enabled = true;
            }
            else if (InitState == 33)
            {
                VisionSystemControlLibrary.GlobalWindows.FaultMessage_Window.Visible = true;//故障信息窗口
                VisionSystemControlLibrary.GlobalWindows.FaultMessage_Window.Location = Global.WorkWindow.Location;
                VisionSystemControlLibrary.GlobalWindows.FaultMessage_Window.Update();//

                InitState++;
                timer1.Enabled = true;
            }
            else if (InitState == 34)
            {
                VisionSystemControlLibrary.GlobalWindows.FaultMessageOption_Window.Visible = true;//故障信息设置窗口
                VisionSystemControlLibrary.GlobalWindows.FaultMessageOption_Window.Location = Global.WorkWindow.Location;
                VisionSystemControlLibrary.GlobalWindows.FaultMessageOption_Window.Update();//

                InitState++;
                timer1.Enabled = true;
            }
            else if (InitState == 35)
            {
                VisionSystemControlLibrary.GlobalWindows.EditTools_Window.Visible = true;//编辑工具窗口
                VisionSystemControlLibrary.GlobalWindows.EditTools_Window.Location = Global.WorkWindow.Location;
                VisionSystemControlLibrary.GlobalWindows.EditTools_Window.Update();//

                InitState++;
                timer1.Enabled = true;
            }
            else if (InitState == 36)
            {
                VisionSystemControlLibrary.GlobalWindows.NewTool_Window.Visible = true;//新建工具窗口
                VisionSystemControlLibrary.GlobalWindows.NewTool_Window.Location = Global.WorkWindow.Location;
                VisionSystemControlLibrary.GlobalWindows.NewTool_Window.Update();//
                
                InitState++;
                timer1.Enabled = true;
            }
            else if (InitState == 37)
            {
                VisionSystemControlLibrary.GlobalWindows.ParameterSettings_Window.Visible = true;//参数设置窗口
                VisionSystemControlLibrary.GlobalWindows.ParameterSettings_Window.Location = Global.WorkWindow.Location;
                VisionSystemControlLibrary.GlobalWindows.ParameterSettings_Window.Update();//

                InitState++;
                timer1.Enabled = true;
            }
            else if (InitState == 38)
            {
                VisionSystemControlLibrary.GlobalWindows.NetDiagnose_Window.Visible = true;//网络查询窗口
                VisionSystemControlLibrary.GlobalWindows.NetDiagnose_Window.Location = Global.WorkWindow.Location;
                VisionSystemControlLibrary.GlobalWindows.NetDiagnose_Window.Update();//

                InitState++;
                timer1.Enabled = true;
            }
            else if (InitState == 39)
            {
                VisionSystemControlLibrary.GlobalWindows.SensorSelect_Window.Visible = true;//传感器选择窗口
                VisionSystemControlLibrary.GlobalWindows.SensorSelect_Window.Location = Global.WorkWindow.Location;
                VisionSystemControlLibrary.GlobalWindows.SensorSelect_Window.Update();//

                InitState++;
                timer1.Enabled = true;
            }
            else if (InitState == 40)
            {
                VisionSystemControlLibrary.GlobalWindows.CigaretteSort_Window.Visible = true;//烟支排列窗口
                VisionSystemControlLibrary.GlobalWindows.CigaretteSort_Window.Location = Global.WorkWindow.Location;
                VisionSystemControlLibrary.GlobalWindows.CigaretteSort_Window.Update();//

                InitState++;
                timer1.Enabled = true;
            }
            else if (InitState == 41)
            {
                Global.WorkWindow.TitleBar.WindowParameter = 15;//WORK，点击【STATE】按钮显示密码窗口
                Global.SystemConfigurationWindow.TitleBar.WindowParameter = 16;//SYSTEM，点击【STATE】按钮显示密码窗口
                Global.DevicesSetupWindow.TitleBar.WindowParameter = 17;//DEVICES SETUP，点击【STATE】按钮显示密码窗口
                Global.BrandManagementWindow.TitleBar.WindowParameter = 19;//BRAND MANAGEMENT，点击【STATE】按钮显示密码窗口
                Global.BackupBrandsWindow.TitleBar.WindowParameter = 20;//BACKUP BRANDS，点击【STATE】按钮显示密码窗口
                Global.RestoreBrandsWindow.TitleBar.WindowParameter = 21;//RESTORE BRANDS，点击【STATE】按钮显示密码窗口
                Global.TolerancesSettingsWindow.TitleBar.WindowParameter = 23;//TOLERANCES SETTINGS，点击【STATE】按钮显示密码窗口
                Global.LiveViewWindow.TitleBar.WindowParameter = 24;//LIVE VIEW，点击【STATE】按钮显示密码窗口
                Global.RejectsViewWindow.TitleBar.WindowParameter = 25;//REJECTS VIEW，点击【STATE】按钮显示密码窗口
                Global.QualityCheckWindow.TitleBar.WindowParameter = 22;//QUALITY CHECK，点击【STATE】按钮显示密码窗口
                Global.ImageConfigurationWindow.TitleBar.WindowParameter = 18;//IMAGE CONFIGURATION，点击【STATE】按钮显示密码窗口
                Global.StatisticsViewWindow.TitleBar.WindowParameter = 26;//STATISTICS VIEW，点击【STATE】按钮显示密码窗口

                InitState++;
                timer1.Enabled = true;
            }
            else if (InitState == 42)
            {
                if (Global.TopMostWindows)//置顶
                {
                    //不执行操作
                }
                else//非置顶
                {
                    Global.SystemConfigurationWindow.Visible = false;//隐藏SYSTEM窗口

                    Global.DevicesSetupWindow.Visible = false;//隐藏DEVICES SETUP窗口

                    Global.BrandManagementWindow.Visible = false;//隐藏BRAND MANAGEMENT窗口

                    Global.BackupBrandsWindow.Visible = false;//隐藏BRAND MANAGEMENT，BACKUP BRANDS窗口

                    Global.RestoreBrandsWindow.Visible = false;//隐藏BRAND MANAGEMENT，RESTORE BRANDS窗口

                    Global.TolerancesSettingsWindow.Visible = false;//隐藏TOLERANCES SETTINGS窗口

                    Global.LiveViewWindow.Visible = false;//隐藏LIVE VIEW窗口

                    Global.RejectsViewWindow.Visible = false;//隐藏REJECTS VIEW窗口

                    Global.QualityCheckWindow.Visible = false;//隐藏QUALITY CHECK窗口

                    Global.ImageConfigurationWindow.Visible = false;//隐藏IMAGE CONFIGURATION窗口

                    Global.StatisticsViewWindow.Visible = false;//隐藏STATISTICS VIEW窗口
                    
                    //

                    VisionSystemControlLibrary.GlobalWindows.DigitalKeyboard_Window.Visible = false;//隐藏DigitalKeyboard窗口

                    VisionSystemControlLibrary.GlobalWindows.StandardKeyboard_Window.Visible = false;//隐藏StandardKeyboard窗口

                    VisionSystemControlLibrary.GlobalWindows.MessageDisplay_Window.Visible = false;//隐藏MessageDisplay窗口

                    VisionSystemControlLibrary.GlobalWindows.DeviceConfiguration_Window.Visible = false;//隐藏DeviceConfiguration窗口

                    VisionSystemControlLibrary.GlobalWindows.IOSignalDiagnosis_Window.Visible = false;//隐藏IOSignalDiagnosis窗口

                    VisionSystemControlLibrary.GlobalWindows.DateTimePanel_Window.Visible = false;//隐藏日期时间面板窗口

                    VisionSystemControlLibrary.GlobalWindows.ShiftConfiguration_Window.Visible = false;//隐藏班次设置窗口

                    VisionSystemControlLibrary.GlobalWindows.StatisticsRecord_Window.Visible = false;//隐藏统计记录窗口

                    VisionSystemControlLibrary.GlobalWindows.FaultMessage_Window.Visible = false;//隐藏故障信息窗口

                    VisionSystemControlLibrary.GlobalWindows.FaultMessageOption_Window.Visible = false;//隐藏故障信息设置窗口

                    VisionSystemControlLibrary.GlobalWindows.EditTools_Window.Visible = false;//隐藏编辑工具窗口

                    VisionSystemControlLibrary.GlobalWindows.NewTool_Window.Visible = false;//隐藏新建工具窗口

                    VisionSystemControlLibrary.GlobalWindows.ParameterSettings_Window.Visible = false;//隐藏参数设置窗口

                    VisionSystemControlLibrary.GlobalWindows.NetDiagnose_Window.Visible = false;//隐藏网络查询窗口

                    VisionSystemControlLibrary.GlobalWindows.SensorSelect_Window.Visible = false;//隐藏传感器选择窗口

                    VisionSystemControlLibrary.GlobalWindows.CigaretteSort_Window.Visible = false;//隐藏烟支排列窗口
                }

                //

                Global.ImageScale_Work = Global.WorkWindow.CustomControl.ImageDisplayScale_Y;//WORK页面图像比例（值为图像显示控件的ControlScale_Y）
                Global.ImageScale_TolerancesSettings = Global.TolerancesSettingsWindow.CustomControl.ImageDisplayScale_Y;//TOLERANCES SETTINGS页面图像比例（值为图像显示控件的ControlScale_Y）

                //

                Global.WorkWindow.CustomControl.SystemDeviceState = VisionSystemClassLibrary.Class.System.SystemDeviceState;//更新WORK页面
                Global.BackupBrandsWindow.CustomControl.SystemDeviceState = VisionSystemClassLibrary.Class.System.SystemDeviceState;//更新页面
                Global.RestoreBrandsWindow.CustomControl.SystemDeviceState = VisionSystemClassLibrary.Class.System.SystemDeviceState;//更新页面
                Global.BrandManagementWindow.CustomControl.SystemDeviceState = VisionSystemClassLibrary.Class.System.SystemDeviceState;//更新页面
                Global.LiveViewWindow.CustomControl.SystemDeviceState = VisionSystemClassLibrary.Class.System.SystemDeviceState;//更新页面
                Global.RejectsViewWindow.CustomControl.SystemDeviceState = VisionSystemClassLibrary.Class.System.SystemDeviceState;//更新页面
                Global.QualityCheckWindow.CustomControl.SystemDeviceState = VisionSystemClassLibrary.Class.System.SystemDeviceState;//更新页面
                Global.TolerancesSettingsWindow.CustomControl.SystemDeviceState = VisionSystemClassLibrary.Class.System.SystemDeviceState;//更新页面
                Global.ImageConfigurationWindow.CustomControl.SystemDeviceState = VisionSystemClassLibrary.Class.System.SystemDeviceState;//更新页面
                Global.DevicesSetupWindow.CustomControl.SystemDeviceState = VisionSystemClassLibrary.Class.System.SystemDeviceState;//更新页面
                Global.SystemConfigurationWindow.CustomControl.SystemDeviceState = VisionSystemClassLibrary.Class.System.SystemDeviceState;//更新页面
                Global.StatisticsViewWindow.CustomControl.SystemDeviceState = VisionSystemClassLibrary.Class.System.SystemDeviceState;//更新页面

                InitState++;
                timer1.Enabled = true;
            }
            else if (InitState == 43)
            {
                //切换至WORK页面

                Global.WorkWindow._CheckUpdate(true);//检查更新（用于更新页面）

                InitState++;
                timer1.Enabled = true;
            }
            else if (InitState == 44)
            {
                Global.WorkWindow._SetWindow();//设置窗口

                Global.WorkWindow.LoadOK = true;//程序加载完成

                InitState++;
            }
        }
    }
}