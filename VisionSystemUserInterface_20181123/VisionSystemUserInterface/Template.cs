
/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：Template.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：Template页面

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

using AutoIt;

namespace VisionSystemUserInterface
{
    public partial class Template : Form
    {
        //WORK、LIVE VIEW、BRAND MANAGEMENT、REJECTS、QUALITY CHECK、TOLERANCES、DEVICES SETUP、CONFIG IMAGE页面模板

        //构造函数

        //----------------------------------------------------------------------
        // 功能说明：构造函数，初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Template()
        {
            InitializeComponent();
        }

        //属性

        //----------------------------------------------------------------------
        // 功能说明：TitleBar属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("TitleBarControl控件"), Category("Template 窗口")]
        public VisionSystemControlLibrary.TitleBarControl TitleBar//属性
        {
            get//读取
            {
                return titleBarControl;
            }
        }

        //事件

        //----------------------------------------------------------------------
        // 功能说明：窗体加载事件
        // 输入参数：1.sender：窗体自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void Template_Load(object sender, EventArgs e)
        {
            TitleBar.TitleBarStyle = Global.VisionSystem.TitlebarStyle;//标题栏样式
            TitleBar.UserPassword = Global.VisionSystem.UserPassword;//用户密码
            TitleBar.AdministratorPassword = Global.VisionSystem.AdministratorPassword;//管理员密码
            TitleBar.Language = VisionSystemClassLibrary.Class.System.Language;//页面语言
            TitleBar.Caption = VisionSystemControlLibrary.WorkControl._GetProductFullName(Global.VisionSystem.MachineType[Global.VisionSystem.SelectedMachineType], Global.VisionSystem.ProductName, Global.VisionSystem.ProductModelNumber);//标题文本
            TitleBar.CurrentMachineType = Global.VisionSystem.MachineType[Global.VisionSystem.SelectedMachineType];//机器类型
            TitleBar.CurrentBrand = Global._GetCURRENTBrandName();//当前品牌
            TitleBar.SystemDeviceState = VisionSystemClassLibrary.Class.System.SystemDeviceState;//设备状态

            if (null != Global.VisionSystem.Camera)
            {
                TitleBar.FaultExist = new Boolean[Global.VisionSystem.Camera.Length];
                TitleBar.CameraName_Chinese = new String[Global.VisionSystem.Camera.Length];
                TitleBar.CameraName_English = new String[Global.VisionSystem.Camera.Length];
                for (Int32 i = 0; i < TitleBar.FaultExist.Length; i++)//初始化
                {
                    TitleBar.FaultExist[i] = false;
                    TitleBar.CameraName_Chinese[i] = Global.VisionSystem.Camera[i].CameraCHNName;
                    TitleBar.CameraName_English[i] = Global.VisionSystem.Camera[i].CameraENGName;
                }
            }

            if (0 < Global.VisionSystem.Work.ConnectedCameraNumber)//存在连接的设备
            {
                TitleBar.StateShow = true;//显示
            }
            else//不存在连接的设备
            {
                TitleBar.StateShow = false;//显示
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【NET CHECK】按钮事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void titleBarControl_NetCheck_Click(object sender, EventArgs e)
        {
            ////发送指令

            ////标题栏【NET CHECK】按钮操作，网络检查，格式：
            ////服务端->客户端：_RequestClientDeviceInformation();

            //try
            //{
            //    for (int i = 0; i < Global.VisionSystem.Camera.Length; i++)
            //    {
            //        Byte[] DevicesSetupRefreshList_ClientIP = IPAddress.Parse(Global.VisionSystem.Camera[i].DeviceInformation.IP).GetAddressBytes();//获取当前选中的相机的IP地址

            //        Global.WorkWindow._RequestClientDeviceInformation(Global.NetServer.ClientData[DevicesSetupRefreshList_ClientIP[3]]);//发送数据
            //    }
            //}
            //catch (System.Exception ex)
            //{
            //    //不执行操作
            //}
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【STATE】按钮事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void titleBarControl_State_Click(object sender, EventArgs e)
        {
            switch (Global.CurrentInterface)
            {
                case ApplicationInterface.Work://WORK页面

                    Global.WorkWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//WORK页面
                    Global.WorkWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新WORK页面

                    Global.SystemConfigurationWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//SYSTEM页面
                    Global.DevicesSetupWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//DEVICES SETUP页面
                    Global.ImageConfigurationWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//DEVICES SETUP，IMAGE CONFIGURATION页面
                    Global.BrandManagementWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//BRAND MANAGEMENT页面
                    Global.BackupBrandsWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//BRAND MANAGEMENT，BACKUP BRANDS页面
                    Global.RestoreBrandsWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//BRAND MANAGEMENT，RESTORE BRANDS页面
                    Global.TolerancesSettingsWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//TOLERANCES SETTINGS页面
                    Global.LiveViewWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//更新LIVE VIEW页面
                    Global.RejectsViewWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//REJECTS VIEW页面
                    Global.QualityCheckWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//更新QUALITY CHECK页面
                    Global.StatisticsViewWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//更新STATISTICS VIEW页面
                    //
                    Global.BackupBrandsWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面         
                    Global.RestoreBrandsWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.BrandManagementWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.LiveViewWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.RejectsViewWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.QualityCheckWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.TolerancesSettingsWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.ImageConfigurationWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.DevicesSetupWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.SystemConfigurationWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.StatisticsViewWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面

                    break;
                    //
                case ApplicationInterface.Load://LOAD页面
                    //
                    break;
                    //
                case ApplicationInterface.BrandManagement://BRAND MANAGEMENT页面
                    
                    Global.BrandManagementWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//BRAND MANAGEMENT页面
                    Global.BrandManagementWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.BackupBrandsWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//BRAND MANAGEMENT，BACKUP BRANDS页面
                    Global.BackupBrandsWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面         
                    Global.RestoreBrandsWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//BRAND MANAGEMENT，RESTORE BRANDS页面
                    Global.RestoreBrandsWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面

                    Global.WorkWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//WORK页面
                    Global.SystemConfigurationWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//SYSTEM页面
                    Global.DevicesSetupWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//DEVICES SETUP页面
                    Global.ImageConfigurationWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//DEVICES SETUP，IMAGE CONFIGURATION页面
                    Global.TolerancesSettingsWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//TOLERANCES SETTINGS页面
                    Global.LiveViewWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//更新LIVE VIEW页面
                    Global.RejectsViewWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//REJECTS VIEW页面
                    Global.QualityCheckWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//更新QUALITY CHECK页面
                    Global.StatisticsViewWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//更新STATISTICS VIEW页面
                    //
                    Global.WorkWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新WORK页面
                    Global.LiveViewWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.RejectsViewWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.QualityCheckWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.TolerancesSettingsWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.ImageConfigurationWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.DevicesSetupWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.SystemConfigurationWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.StatisticsViewWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面

                    break;
                    //
                case ApplicationInterface.LiveView://LIVE VIEW页面

                    Global.LiveViewWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//更新LIVE VIEW页面
                    Global.LiveViewWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面

                    Global.WorkWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//WORK页面
                    Global.SystemConfigurationWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//SYSTEM页面
                    Global.DevicesSetupWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//DEVICES SETUP页面
                    Global.ImageConfigurationWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//DEVICES SETUP，IMAGE CONFIGURATION页面
                    Global.BrandManagementWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//BRAND MANAGEMENT页面
                    Global.BackupBrandsWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//BRAND MANAGEMENT，BACKUP BRANDS页面
                    Global.RestoreBrandsWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//BRAND MANAGEMENT，RESTORE BRANDS页面
                    Global.TolerancesSettingsWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//TOLERANCES SETTINGS页面
                    Global.RejectsViewWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//REJECTS VIEW页面
                    Global.QualityCheckWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//更新QUALITY CHECK页面
                    Global.StatisticsViewWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//更新STATISTICS VIEW页面
                    //
                    Global.WorkWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新WORK页面
                    Global.BackupBrandsWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面         
                    Global.RestoreBrandsWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.BrandManagementWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.RejectsViewWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.QualityCheckWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.TolerancesSettingsWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.ImageConfigurationWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.DevicesSetupWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.SystemConfigurationWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.StatisticsViewWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面

                    break;
                    //
                case ApplicationInterface.RejectsView://REJECTS VIEW页面                            
                    
                    Global.RejectsViewWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//REJECTS VIEW页面
                    Global.RejectsViewWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面

                    Global.WorkWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//WORK页面
                    Global.SystemConfigurationWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//SYSTEM页面
                    Global.DevicesSetupWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//DEVICES SETUP页面
                    Global.ImageConfigurationWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//DEVICES SETUP，IMAGE CONFIGURATION页面
                    Global.BrandManagementWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//BRAND MANAGEMENT页面
                    Global.BackupBrandsWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//BRAND MANAGEMENT，BACKUP BRANDS页面
                    Global.RestoreBrandsWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//BRAND MANAGEMENT，RESTORE BRANDS页面
                    Global.TolerancesSettingsWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//TOLERANCES SETTINGS页面
                    Global.LiveViewWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//更新LIVE VIEW页面
                    Global.QualityCheckWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//更新QUALITY CHECK页面
                    Global.StatisticsViewWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//更新STATISTICS VIEW页面
                    //
                    Global.WorkWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新WORK页面
                    Global.BackupBrandsWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面         
                    Global.RestoreBrandsWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.BrandManagementWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.LiveViewWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.QualityCheckWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.TolerancesSettingsWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.ImageConfigurationWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.DevicesSetupWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.SystemConfigurationWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.StatisticsViewWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面

                    break;
                    //
                case ApplicationInterface.QualityCheck://QUALITY CHECK页面

                    Global.QualityCheckWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//更新QUALITY CHECK页面
                    Global.QualityCheckWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面

                    Global.WorkWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新WORK页面
                    Global.SystemConfigurationWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//SYSTEM页面
                    Global.DevicesSetupWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//DEVICES SETUP页面
                    Global.ImageConfigurationWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//DEVICES SETUP，IMAGE CONFIGURATION页面
                    Global.BrandManagementWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//BRAND MANAGEMENT页面
                    Global.BackupBrandsWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//BRAND MANAGEMENT，BACKUP BRANDS页面
                    Global.RestoreBrandsWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//BRAND MANAGEMENT，RESTORE BRANDS页面
                    Global.TolerancesSettingsWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//TOLERANCES SETTINGS页面
                    Global.LiveViewWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//更新LIVE VIEW页面
                    Global.RejectsViewWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//REJECTS VIEW页面
                    Global.StatisticsViewWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//更新STATISTICS VIEW页面
                    //
                    Global.WorkWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//WORK页面
                    Global.BackupBrandsWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面         
                    Global.RestoreBrandsWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.BrandManagementWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.LiveViewWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.RejectsViewWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.TolerancesSettingsWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.ImageConfigurationWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.DevicesSetupWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.SystemConfigurationWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.StatisticsViewWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面

                    break;
                    //
                case ApplicationInterface.TolerancesSettings://TOLERANCES SETTINGS页面
                    
                    Global.TolerancesSettingsWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//TOLERANCES SETTINGS页面
                    Global.TolerancesSettingsWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面

                    Global.WorkWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//WORK页面
                    Global.SystemConfigurationWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//SYSTEM页面
                    Global.DevicesSetupWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//DEVICES SETUP页面
                    Global.ImageConfigurationWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//DEVICES SETUP，IMAGE CONFIGURATION页面
                    Global.BrandManagementWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//BRAND MANAGEMENT页面
                    Global.BackupBrandsWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//BRAND MANAGEMENT，BACKUP BRANDS页面
                    Global.RestoreBrandsWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//BRAND MANAGEMENT，RESTORE BRANDS页面
                    Global.LiveViewWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//更新LIVE VIEW页面
                    Global.RejectsViewWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//REJECTS VIEW页面
                    Global.QualityCheckWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//更新QUALITY CHECK页面
                    Global.StatisticsViewWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//更新STATISTICS VIEW页面
                    //
                    Global.WorkWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新WORK页面
                    Global.BackupBrandsWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面         
                    Global.RestoreBrandsWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.BrandManagementWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.LiveViewWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.RejectsViewWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.QualityCheckWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.ImageConfigurationWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.DevicesSetupWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.SystemConfigurationWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.StatisticsViewWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面

                    break;
                    //
                case ApplicationInterface.DevicesSetup://DEVICES SETUP页面

                    Global.DevicesSetupWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//DEVICES SETUP页面
                    Global.DevicesSetupWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.ImageConfigurationWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//DEVICES SETUP，IMAGE CONFIGURATION页面
                    Global.ImageConfigurationWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面

                    Global.WorkWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//WORK页面
                    Global.SystemConfigurationWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//SYSTEM页面
                    Global.BrandManagementWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//BRAND MANAGEMENT页面
                    Global.BackupBrandsWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//BRAND MANAGEMENT，BACKUP BRANDS页面
                    Global.RestoreBrandsWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//BRAND MANAGEMENT，RESTORE BRANDS页面
                    Global.TolerancesSettingsWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//TOLERANCES SETTINGS页面
                    Global.LiveViewWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//更新LIVE VIEW页面
                    Global.RejectsViewWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//REJECTS VIEW页面
                    Global.QualityCheckWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//更新QUALITY CHECK页面
                    Global.StatisticsViewWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//更新STATISTICS VIEW页面
                    //
                    Global.WorkWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新WORK页面
                    Global.BackupBrandsWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面         
                    Global.RestoreBrandsWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.BrandManagementWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.LiveViewWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.RejectsViewWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.QualityCheckWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.TolerancesSettingsWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.SystemConfigurationWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.StatisticsViewWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面

                    break;
                    //
                case ApplicationInterface.SystemConfiguration://SYSTEM页面
                    
                    Global.SystemConfigurationWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//SYSTEM页面
                    Global.SystemConfigurationWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面

                    Global.WorkWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//WORK页面
                    Global.DevicesSetupWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//DEVICES SETUP页面
                    Global.ImageConfigurationWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//DEVICES SETUP，IMAGE CONFIGURATION页面
                    Global.BrandManagementWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//BRAND MANAGEMENT页面
                    Global.BackupBrandsWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//BRAND MANAGEMENT，BACKUP BRANDS页面
                    Global.RestoreBrandsWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//BRAND MANAGEMENT，RESTORE BRANDS页面
                    Global.TolerancesSettingsWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//TOLERANCES SETTINGS页面
                    Global.LiveViewWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//更新LIVE VIEW页面
                    Global.RejectsViewWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//REJECTS VIEW页面
                    Global.QualityCheckWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//更新QUALITY CHECK页面
                    Global.StatisticsViewWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//更新STATISTICS VIEW页面
                    //
                    Global.WorkWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新WORK页面
                    Global.BackupBrandsWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面         
                    Global.RestoreBrandsWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.BrandManagementWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.LiveViewWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.RejectsViewWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.QualityCheckWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.TolerancesSettingsWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.ImageConfigurationWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.DevicesSetupWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.StatisticsViewWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面

                    break;
                    //
                case ApplicationInterface.StatisticsView://STATISTICS页面

                    Global.StatisticsViewWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//更新STATISTICS VIEW页面
                    Global.StatisticsViewWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面

                    Global.WorkWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//WORK页面
                    Global.SystemConfigurationWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//SYSTEM页面
                    Global.DevicesSetupWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//DEVICES SETUP页面
                    Global.ImageConfigurationWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//DEVICES SETUP，IMAGE CONFIGURATION页面
                    Global.BrandManagementWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//BRAND MANAGEMENT页面
                    Global.BackupBrandsWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//BRAND MANAGEMENT，BACKUP BRANDS页面
                    Global.RestoreBrandsWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//BRAND MANAGEMENT，RESTORE BRANDS页面
                    Global.TolerancesSettingsWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//TOLERANCES SETTINGS页面
                    Global.LiveViewWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//更新LIVE VIEW页面
                    Global.RejectsViewWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//REJECTS VIEW页面
                    Global.QualityCheckWindow.TitleBar.SystemDeviceState = titleBarControl.SystemDeviceState;//更新QUALITY CHECK页面
                    //
                    Global.WorkWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新WORK页面
                    Global.BackupBrandsWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面         
                    Global.RestoreBrandsWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.BrandManagementWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.LiveViewWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.RejectsViewWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.QualityCheckWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.TolerancesSettingsWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.ImageConfigurationWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.DevicesSetupWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面
                    Global.SystemConfigurationWindow.CustomControl.SystemDeviceState = titleBarControl.SystemDeviceState;//更新页面

                    break;
                default:
                    break;
            }

            //写文件

            try
            {
                VisionSystemClassLibrary.Class.System._WriteMachineStateInfoFile();
            }
            catch (System.Exception ex)
            {
            	//不执行操作
            }

            //发送指令

            //标题栏【STATE】按钮操作，设备状态，格式：
            //服务端->客户端：指令类型 + 相机类型数据 + 设备状态（VisionSystemClassLibrary.Enum.DeviceState）

            try
            {
                Int32 i = 0;//循环控制变量

                for (i = 0; i < Global.VisionSystem.Camera.Length; i++)//遍历相机
                {
                    Global.WorkWindow._SendCommand_Value(CommunicationInstructionType.TitleBar_State, Global.VisionSystem.Camera[i].Type, i, (Int32)VisionSystemClassLibrary.Class.System.SystemDeviceState);//发送指令
                }
            }
            catch (System.Exception ex)
            {
                //不执行操作
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：单击FAULT MESSAGE控件事件，打开FAULT MESSAGE窗口
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void titleBarControl_GetFaultMessages(object sender, EventArgs e)
        {
            if (Global.VisionSystem.Shift.ShiftState)//使能班次
            {
                titleBarControl._OpenFaultMessageWindow(Global.VisionSystem);

                //

                Global.WorkWindow._SendCommand_Cameras(CommunicationInstructionType.GetFaultMessages);//发送指令
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：双击切换人机界面控件事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void titleBarControl_ChangeInterface(object sender, EventArgs e)
        {
            if (Global.TopMostWindows == false) //当前为非置顶模式
            {
                if (Global.CurrentInterface == ApplicationInterface.Work) //当前处在工作界面
                {
                    if (AutoItX.WinActive("FrmWork") == 0) //激活待启动窗口
                    {
                        AutoItX.WinActivate("FrmWork");
                    }
                }
            }
        }
    }
}