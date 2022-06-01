/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：SystemConfiguration.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：SYSTEM页面

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

using System.Runtime.InteropServices;

using System.Reflection;

namespace VisionSystemUserInterface
{
    public partial class SystemConfiguration : Template
    {
        [DllImport("User32.dll")]
        private extern static IntPtr FindWindow(String lpClassName, String lpWindowName);//查找窗口

        [DllImport("User32.dll")]
        private extern static Boolean GetWindowRect(IntPtr hWnd, out Rectangle lpRect);//获取窗口尺寸

        [DllImport("User32.dll")]
        private extern static Boolean SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);//更改窗口位置

        //SYSTEM页面

        //构造函数

        //----------------------------------------------------------------------
        // 功能说明：构造函数，初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public SystemConfiguration()
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
        public VisionSystemControlLibrary.SystemControl CustomControl//属性
        {
            get//读取
            {
                return systemControl;
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
            Global.CurrentInterface = ApplicationInterface.SystemConfiguration;//当前页面，System

            systemControl._Properties(Global.VisionSystem);//应用属性

            //

            if (Global.TopMostWindows)//置顶
            {
                this.TopMost = true;//将窗口置于顶层
            }
            else//其它
            {
                this.Visible = true;//隐藏
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
        private void SystemConfiguration_Load(object sender, EventArgs e)
        {
            //Global.CurrentInterface = ApplicationInterface.SystemConfiguration;//当前页面，System
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【OK】按钮保存参数
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void systemControl_Ok_Click(object sender, EventArgs e)
        {
            if (systemControl.CheckLayoutChanged || systemControl.CigaretteSortChanged)//修改
            {
                Application.ExitThread();//退出应用程序所有线程
                Application.Exit();
                System.Diagnostics.Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);//重新启动系统
            }
            else//未修改
            {
                TitleBar.Caption = VisionSystemControlLibrary.WorkControl._GetProductFullName(Global.VisionSystem.MachineType[Global.VisionSystem.SelectedMachineType], Global.VisionSystem._GetProductName(VisionSystemClassLibrary.Class.System.Language), Global.VisionSystem.ProductModelNumber);//标题文本
                TitleBar.CurrentMachineType = Global.VisionSystem.MachineType[Global.VisionSystem.SelectedMachineType];//机器类型

                Global.VisionSystem.ProductName = Global.VisionSystem._GetProductName(VisionSystemClassLibrary.Class.System.Language);//标题文本

                //

                Global.WorkWindow.TitleBar.UserPassword = Global.VisionSystem.UserPassword;//用户密码
                Global.WorkWindow.TitleBar.Language = VisionSystemClassLibrary.Class.System.Language;//更新
                Global.WorkWindow.TitleBar.Caption = TitleBar.Caption;//标题文本
                Global.WorkWindow.TitleBar.CurrentMachineType = TitleBar.CurrentMachineType;//机器类型
                Global.WorkWindow.CustomControl.Language = VisionSystemClassLibrary.Class.System.Language;//设置语言

                Global.SystemConfigurationWindow.TitleBar.UserPassword = Global.VisionSystem.UserPassword;//用户密码
                Global.SystemConfigurationWindow.TitleBar.Language = VisionSystemClassLibrary.Class.System.Language;//更新
                Global.SystemConfigurationWindow.TitleBar.Caption = Global.WorkWindow.TitleBar.Caption;//SYSTEM窗口
                Global.SystemConfigurationWindow.TitleBar.CurrentMachineType = Global.WorkWindow.TitleBar.CurrentMachineType;//SYSTEM窗口
                Global.SystemConfigurationWindow.CustomControl.Language = VisionSystemClassLibrary.Class.System.Language;//设置语言

                Global.DevicesSetupWindow.TitleBar.UserPassword = Global.VisionSystem.UserPassword;//用户密码
                Global.DevicesSetupWindow.TitleBar.Language = VisionSystemClassLibrary.Class.System.Language;//更新
                Global.DevicesSetupWindow.TitleBar.Caption = Global.WorkWindow.TitleBar.Caption;//DEVICES SETUP窗口
                Global.DevicesSetupWindow.TitleBar.CurrentMachineType = Global.WorkWindow.TitleBar.CurrentMachineType;//DEVICES SETUP窗口
                Global.DevicesSetupWindow.CustomControl.Language = VisionSystemClassLibrary.Class.System.Language;//设置语言

                Global.BrandManagementWindow.TitleBar.UserPassword = Global.VisionSystem.UserPassword;//用户密码
                Global.BrandManagementWindow.TitleBar.Language = VisionSystemClassLibrary.Class.System.Language;//更新
                Global.BrandManagementWindow.TitleBar.Caption = Global.WorkWindow.TitleBar.Caption;//BRAND MANAGEMENT窗口
                Global.BrandManagementWindow.TitleBar.CurrentMachineType = Global.WorkWindow.TitleBar.CurrentMachineType;//BRAND MANAGEMENT窗口
                Global.BrandManagementWindow.CustomControl.Language = VisionSystemClassLibrary.Class.System.Language;//设置语言

                Global.BackupBrandsWindow.TitleBar.UserPassword = Global.VisionSystem.UserPassword;//用户密码
                Global.BackupBrandsWindow.TitleBar.Language = VisionSystemClassLibrary.Class.System.Language;//更新
                Global.BackupBrandsWindow.TitleBar.Caption = Global.WorkWindow.TitleBar.Caption;//BRAND MANAGEMENT，BACKUP BRANDS窗口
                Global.BackupBrandsWindow.TitleBar.CurrentMachineType = Global.WorkWindow.TitleBar.CurrentMachineType;//BRAND MANAGEMENT，BACKUP BRANDS窗口
                Global.BackupBrandsWindow.CustomControl.Language = VisionSystemClassLibrary.Class.System.Language;//设置语言

                Global.RestoreBrandsWindow.TitleBar.UserPassword = Global.VisionSystem.UserPassword;//用户密码
                Global.RestoreBrandsWindow.TitleBar.Language = VisionSystemClassLibrary.Class.System.Language;//更新
                Global.RestoreBrandsWindow.TitleBar.Caption = Global.WorkWindow.TitleBar.Caption;//BRAND MANAGEMENT，RESTORE BRANDS窗口
                Global.RestoreBrandsWindow.TitleBar.CurrentMachineType = Global.WorkWindow.TitleBar.CurrentMachineType;//BRAND MANAGEMENT，RESTORE BRANDS窗口
                Global.RestoreBrandsWindow.CustomControl.Language = VisionSystemClassLibrary.Class.System.Language;//设置语言

                Global.TolerancesSettingsWindow.TitleBar.UserPassword = Global.VisionSystem.UserPassword;//用户密码
                Global.TolerancesSettingsWindow.TitleBar.Language = VisionSystemClassLibrary.Class.System.Language;//更新
                Global.TolerancesSettingsWindow.TitleBar.Caption = Global.WorkWindow.TitleBar.Caption;//TOLERANCES SETTINGS窗口
                Global.TolerancesSettingsWindow.TitleBar.CurrentMachineType = Global.WorkWindow.TitleBar.CurrentMachineType;//TOLERANCES SETTINGS窗口
                Global.TolerancesSettingsWindow.CustomControl.Language = VisionSystemClassLibrary.Class.System.Language;//设置语言

                Global.LiveViewWindow.TitleBar.UserPassword = Global.VisionSystem.UserPassword;//用户密码
                Global.LiveViewWindow.TitleBar.Language = VisionSystemClassLibrary.Class.System.Language;//更新
                Global.LiveViewWindow.TitleBar.Caption = Global.WorkWindow.TitleBar.Caption;//LIVE VIEW窗口
                Global.LiveViewWindow.TitleBar.CurrentMachineType = Global.WorkWindow.TitleBar.CurrentMachineType;//LIVE VIEW窗口
                Global.LiveViewWindow.CustomControl.Language = VisionSystemClassLibrary.Class.System.Language;//设置语言

                Global.RejectsViewWindow.TitleBar.UserPassword = Global.VisionSystem.UserPassword;//用户密码
                Global.RejectsViewWindow.TitleBar.Language = VisionSystemClassLibrary.Class.System.Language;//更新
                Global.RejectsViewWindow.TitleBar.Caption = Global.WorkWindow.TitleBar.Caption;//REJECTS VIEW窗口
                Global.RejectsViewWindow.TitleBar.CurrentMachineType = Global.WorkWindow.TitleBar.CurrentMachineType;//REJECTS VIEW窗口
                Global.RejectsViewWindow.CustomControl.Language = VisionSystemClassLibrary.Class.System.Language;//设置语言

                Global.QualityCheckWindow.TitleBar.UserPassword = Global.VisionSystem.UserPassword;//用户密码
                Global.QualityCheckWindow.TitleBar.Language = VisionSystemClassLibrary.Class.System.Language; ;//更新
                Global.QualityCheckWindow.TitleBar.Caption = Global.WorkWindow.TitleBar.Caption;//QUALITY CHECK窗口
                Global.QualityCheckWindow.TitleBar.CurrentMachineType = Global.WorkWindow.TitleBar.CurrentMachineType;//QUALITY CHECK窗口
                Global.QualityCheckWindow.CustomControl.Language = VisionSystemClassLibrary.Class.System.Language;//设置语言

                Global.ImageConfigurationWindow.TitleBar.UserPassword = Global.VisionSystem.UserPassword;//用户密码
                Global.ImageConfigurationWindow.TitleBar.Language = VisionSystemClassLibrary.Class.System.Language;//更新
                Global.ImageConfigurationWindow.TitleBar.Caption = Global.WorkWindow.TitleBar.Caption;//IMAGE CONFIGURATION窗口
                Global.ImageConfigurationWindow.TitleBar.CurrentMachineType = Global.WorkWindow.TitleBar.CurrentMachineType;//IMAGE CONFIGURATION窗口
                Global.ImageConfigurationWindow.CustomControl.Language = VisionSystemClassLibrary.Class.System.Language;//设置语言

                Global.StatisticsViewWindow.TitleBar.UserPassword = Global.VisionSystem.UserPassword;//用户密码
                Global.StatisticsViewWindow.TitleBar.Language = VisionSystemClassLibrary.Class.System.Language;//更新
                Global.StatisticsViewWindow.TitleBar.Caption = Global.WorkWindow.TitleBar.Caption;//IMAGE CONFIGURATION窗口
                Global.StatisticsViewWindow.TitleBar.CurrentMachineType = Global.WorkWindow.TitleBar.CurrentMachineType;//IMAGE CONFIGURATION窗口
                Global.StatisticsViewWindow.CustomControl.Language = VisionSystemClassLibrary.Class.System.Language;//设置语言

                Global.WorkWindow._SetWindow();
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：更改机器选择参数事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void systemControl_Ok_CommonParameterChanged_Click(object sender, EventArgs e)
        {
            Int32 i = 0;
            Int32 j = 0;
            Int32 k = 0;

            //for (i = 0; i < Global.VisionSystem.Camera.Length; i++)//遍历相机
            //{
            //    if (Global.VisionSystem.Camera[i].DeviceInformation.Connected && Global.VisionSystem.Camera[i].DeviceInformation.GetDevInfo)//有效
            //    {
            //        j++;
            //    }
            //}

            for (i = 0; i < Global.VisionSystem.Camera.Length; i++)//遍历相机
            {
                if ("" != Global.VisionSystem.Camera[i].DeviceInformation.IP)//有效
                {
                    j++;
                }
            }

            systemControl.CommonParameterNumber = j;

            //

            if (0 == j)//无效
            {
                systemControl._CommonParameter(true);
            }
            else//有效
            {
                for (i = 0; i < Global.VisionSystem.SystemCameraConfiguration.Length; i++)
                {
                    for (j = 0; j < Global.VisionSystem.Camera.Length; j++)//遍历相机
                    {
                        if (Global.VisionSystem.SystemCameraConfiguration[i].Type == Global.VisionSystem.Camera[j].Type)//相机存在
                        {
                            //服务端->客户端：指令类型 + 相机类型数据 + 设置相机模式 + 相机选择（1，启用；0，禁用） + 相机检测使能（1，启用；0，禁用） + 烟支排列类型 + 机器类型 + 班次数据（文件）

                            Global.VisionSystem.Camera[j].CheckEnable = Global.VisionSystem.SystemCameraConfiguration[i].CheckEnable;//更新相机检测使能
                            Global.VisionSystem.Camera[j].TobaccoSortType_E = Global.VisionSystem.SystemCameraConfiguration[i].TobaccoSortType_E;//更新烟支排列类型

                            if (Global.VisionSystem.Camera[j].CheckEnable)//检测使能开启
                            {
                                Global.VisionSystem.Camera[j].DeviceInformation.CAM = Global.VisionSystem.Camera[j].DeviceInformation.CAM & (~(VisionSystemClassLibrary.Enum.CameraState.REJECTOFF));
                            }
                            else//检测使能关闭
                            {
                                Global.VisionSystem.Camera[j].DeviceInformation.CAM = Global.VisionSystem.Camera[j].DeviceInformation.CAM | VisionSystemClassLibrary.Enum.CameraState.REJECTOFF;
                            }

                            for (k = 0; k < Global.VisionSystem.ConnectionData.Length; k++)//遍历设备
                            {
                                if (Global.VisionSystem.ConnectionData[k].Type == Global.VisionSystem.Camera[j].Type)//
                                {
                                    Global.VisionSystem.ConnectionData[k].CAM = Global.VisionSystem.Camera[j].DeviceInformation.CAM;

                                    break;
                                }
                            }

                            Int32 iCheckEnable = (Global.VisionSystem.Camera[j].CheckEnable != false) ? 1 : 0;

                            Int32 iCameraChooseState = 0;
                            String sControllerENGName = Global.VisionSystem.Camera[j].ControllerENGName;

                            for (Int32 l = 0; l < Global.VisionSystem.Camera.Length; l++)
                            {
                                if (sControllerENGName == Global.VisionSystem.Camera[l].ControllerENGName)//有效
                                {
                                    iCameraChooseState |= (0x01 << (Global.VisionSystem.Camera[l].DeviceInformation.Port - 1));
                                }
                            }
                            Global.WorkWindow._SendCommand_Value(CommunicationInstructionType.SystemParameter, Global.VisionSystem.Camera[j].Type, (Int32)Global.VisionSystem.Camera[j].TobaccoSortType_E, iCameraChooseState, j, Convert.ToInt32(Global.VisionSystem.SystemCameraConfiguration[i].Selected), iCheckEnable, VisionSystemClassLibrary.Class.Shift.ShiftDataPath, VisionSystemClassLibrary.Class.Shift.ShiftFileName);//发送指令

                            break;
                        }
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【Close】按钮事件，关闭SYSTEM窗口
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void systemControl_Close_Click(object sender, EventArgs e)
        {
            Global.WorkWindow._SetWindow();
        }

        //----------------------------------------------------------------------
        // 功能说明：更改语言事件，更新标题栏
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void systemControl_LanguageChanged(object sender, EventArgs e)
        {
            TitleBar.Language = systemControl.ChangedLanguage;//更新

            TitleBar.Caption = VisionSystemControlLibrary.WorkControl._GetProductFullName(Global.VisionSystem.MachineType[Global.VisionSystem.SelectedMachineType], Global.VisionSystem._GetProductName(systemControl.ChangedLanguage), Global.VisionSystem.ProductModelNumber);//标题文本
        }
        
        //

        //----------------------------------------------------------------------
        // 功能说明：定时器事件，更新标题栏
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void systemControl_PCTimerTick(object sender, EventArgs e)
        {
            String sTime = Global.SystemConfigurationWindow.CustomControl.PCTime;

            Global.WorkWindow.TitleBar.Invoke(new EventHandler(delegate { Global.WorkWindow.TitleBar.PCTime = sTime; }));//WORK页面
            Global.SystemConfigurationWindow.TitleBar.Invoke(new EventHandler(delegate { Global.SystemConfigurationWindow.TitleBar.PCTime = sTime; }));//SYSTEM页面
            Global.DevicesSetupWindow.TitleBar.Invoke(new EventHandler(delegate { Global.DevicesSetupWindow.TitleBar.PCTime = sTime; }));//DEVICES SETUP页面
            Global.ImageConfigurationWindow.TitleBar.Invoke(new EventHandler(delegate { Global.ImageConfigurationWindow.TitleBar.PCTime = sTime; }));//DEVICES SETUP，IMAGE CONFIGURATION页面
            Global.BrandManagementWindow.TitleBar.Invoke(new EventHandler(delegate { Global.BrandManagementWindow.TitleBar.PCTime = sTime; }));//BRAND MANAGEMENT页面
            Global.BackupBrandsWindow.TitleBar.Invoke(new EventHandler(delegate { Global.BackupBrandsWindow.TitleBar.PCTime = sTime; }));//BRAND MANAGEMENT，BACKUP BRANDS页面
            Global.RestoreBrandsWindow.TitleBar.Invoke(new EventHandler(delegate { Global.RestoreBrandsWindow.TitleBar.PCTime = sTime; }));//BRAND MANAGEMENT，RESTORE BRANDS页面
            Global.TolerancesSettingsWindow.TitleBar.Invoke(new EventHandler(delegate { Global.TolerancesSettingsWindow.TitleBar.PCTime = sTime; }));//TOLERANCES SETTINGS页面
            Global.LiveViewWindow.TitleBar.Invoke(new EventHandler(delegate { Global.LiveViewWindow.TitleBar.PCTime = sTime; }));//更新LIVE VIEW页面
            Global.RejectsViewWindow.TitleBar.Invoke(new EventHandler(delegate { Global.RejectsViewWindow.TitleBar.PCTime = sTime; }));//REJECTS VIEW页面
            Global.QualityCheckWindow.TitleBar.Invoke(new EventHandler(delegate { Global.QualityCheckWindow.TitleBar.PCTime = sTime; }));//更新QUALITY CHECK页面
            Global.StatisticsViewWindow.TitleBar.Invoke(new EventHandler(delegate { Global.StatisticsViewWindow.TitleBar.PCTime = sTime; }));//更新STATISTICS页面

            //

            String sShift = Global._GetShiftInformation();

            Global.WorkWindow.TitleBar.Invoke(new EventHandler(delegate { Global.WorkWindow.TitleBar.CurrentShift = sShift; }));//WORK页面
            Global.SystemConfigurationWindow.TitleBar.Invoke(new EventHandler(delegate { Global.SystemConfigurationWindow.TitleBar.CurrentShift = sShift; }));//SYSTEM页面
            Global.DevicesSetupWindow.TitleBar.Invoke(new EventHandler(delegate { Global.DevicesSetupWindow.TitleBar.CurrentShift = sShift; }));//DEVICES SETUP页面
            Global.ImageConfigurationWindow.TitleBar.Invoke(new EventHandler(delegate { Global.ImageConfigurationWindow.TitleBar.CurrentShift = sShift; }));//DEVICES SETUP，IMAGE CONFIGURATION页面
            Global.BrandManagementWindow.TitleBar.Invoke(new EventHandler(delegate { Global.BrandManagementWindow.TitleBar.CurrentShift = sShift; }));//BRAND MANAGEMENT页面
            Global.BackupBrandsWindow.TitleBar.Invoke(new EventHandler(delegate { Global.BackupBrandsWindow.TitleBar.CurrentShift = sShift; }));//BRAND MANAGEMENT，BACKUP BRANDS页面
            Global.RestoreBrandsWindow.TitleBar.Invoke(new EventHandler(delegate { Global.RestoreBrandsWindow.TitleBar.CurrentShift = sShift; }));//BRAND MANAGEMENT，RESTORE BRANDS页面
            Global.TolerancesSettingsWindow.TitleBar.Invoke(new EventHandler(delegate { Global.TolerancesSettingsWindow.TitleBar.CurrentShift = sShift; }));//TOLERANCES SETTINGS页面
            Global.LiveViewWindow.TitleBar.Invoke(new EventHandler(delegate { Global.LiveViewWindow.TitleBar.CurrentShift = sShift; }));//更新LIVE VIEW页面
            Global.RejectsViewWindow.TitleBar.Invoke(new EventHandler(delegate { Global.RejectsViewWindow.TitleBar.CurrentShift = sShift; }));//REJECTS VIEW页面
            Global.QualityCheckWindow.TitleBar.Invoke(new EventHandler(delegate { Global.QualityCheckWindow.TitleBar.CurrentShift = sShift; }));//更新QUALITY CHECK页面
            Global.StatisticsViewWindow.TitleBar.Invoke(new EventHandler(delegate { Global.StatisticsViewWindow.TitleBar.CurrentShift = sShift; }));//更新STATISTICS页面
        }
    }
}
