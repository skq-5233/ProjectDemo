using DevExpress.XtraBars.Navigation;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckWeighterInterface
{
    public partial class MainForm : DevExpress.XtraEditors.XtraForm
    {
        DateTime now = new DateTime();  //时间设置；      

        private StatusMonitor.StatusMonitor statusMonitor1; //工作状态；
        private DataAnalysis.DataAnalysis dataAnalysis1;    //数据分析；
        private ExcelExport.ExcelExport excelExport1;   //报表生成；
        private ParameterConfig.ParameterConfig paraConfig1;    //参数设置；
        private SystemManagement.SystemManagement systemManagement1;    //系统管理；
        private SystemTest.SystemTest systemTest1;  //系统测试；

        private CommonControl.ConfirmationBox confirmationBox_applicationClose; //确认关闭提示框；

        enum modulePage { statusMonitor = 0, dataAnalysis, exportExcel, paraConfig, sysConfig, sysTest};    //六个主界面；
        private NavigationPage[] modulePages = new NavigationPage[6];   //上述六个主界面所对应的翻页界面；
        enum DataAnalysisPage { timeDomainAnalysis = 0, frequencyDomainAnalysis};   //数据分析中对应的时域分析、频域分析子界面；
        enum ParaConfigPage { benchmarkConfig = 0, algorithmConfig};    //参数设置中对应的基准设置，算法设置子界面；
        enum SysManagementPage { brandManagement = 0, calibrationCorrection, systemConfig}; //系统管理中对应的品牌管理、标定校正、系统设置子界面；
        enum SysTestPage { signalTest = 0, realTimeCurve};  //系统测试中对应的信号测试，实时曲线子界面；

        public MainForm()
        {
            InitializeComponent();
            initMainForm();
        }

        private void timer_datetime_Tick(object sender, EventArgs e)
        {
            now = DateTime.Now;
            this.labelControl_datetime.Text = now.ToString("yyyy-MM-dd  HH:mm:ss"); //实时显示时间；
        }

        private void initMainForm() //初始化主界面；
        {
            Global.initMySQL(); //初始化数据库；

            Global.mysqlHelper1._updateMySQL("TRUNCATE TABLE weight_history;");     //清空原先品牌的重量历史
            //Global.mysqlHelper1._queryTableMySQL("SELECT * FROM brand", ref dtBrand);
            //if(dtBrand.Rows.Count == 0)
            //{
            //    MessageBox.Show("请在数据库中至少添加一个品牌");
            //    Process.GetCurrentProcess().Kill();
            //}

            _loadModules(); 
            _initPages();
        }

        private void _loadModules()
        {
            //statusMonitor
            statusMonitor1 = new StatusMonitor.StatusMonitor();
            this.statusMonitor1.Location = new System.Drawing.Point(0, 0);
            this.statusMonitor1.Name = "statusMonitor2";//可有可无；
            this.statusMonitor1.Size = new System.Drawing.Size(1024, 617);
            this.statusMonitor1.TabIndex = 0;//可有可无；
            this.navigationPage_statusMonitor.Controls.Add(statusMonitor1);
            //dataAnalysis
            dataAnalysis1 = new DataAnalysis.DataAnalysis();
            this.dataAnalysis1.Location = new System.Drawing.Point(0, 0);
            this.dataAnalysis1.Name = "statusMonitor2";//可有可无；
            this.dataAnalysis1.Size = new System.Drawing.Size(1024, 617);
            this.dataAnalysis1.TabIndex = 1;//可有可无；
            this.navigationPage_dataAnalysis.Controls.Add(dataAnalysis1);
            //excelExport
            excelExport1 = new ExcelExport.ExcelExport();
            this.excelExport1.Location = new System.Drawing.Point(0, 0);
            this.excelExport1.Name = "statusMonitor2";//可有可无；
            this.excelExport1.Size = new System.Drawing.Size(1024, 617);
            this.excelExport1.TabIndex = 2;//可有可无；
            this.navigationPage_exportExcel.Controls.Add(excelExport1);
            //paraConfig
            paraConfig1 = new ParameterConfig.ParameterConfig();
            this.paraConfig1.Location = new System.Drawing.Point(0, 0);
            this.paraConfig1.Name = "statusMonitor2";//可有可无；
            this.paraConfig1.Size = new System.Drawing.Size(1024, 617);
            this.paraConfig1.TabIndex = 2;//可有可无；
            this.navigationPage_paraConfig.Controls.Add(paraConfig1);
            //systemConfig
            systemManagement1 = new SystemManagement.SystemManagement();
            this.systemManagement1.Location = new System.Drawing.Point(0, 0);
            this.systemManagement1.Name = "statusMonitor2";//可有可无；
            this.systemManagement1.Size = new System.Drawing.Size(1024, 617);
            this.systemManagement1.TabIndex = 3;//可有可无；
            this.navigationPage_sysConfig.Controls.Add(systemManagement1);
            //systemTest
            systemTest1 = new SystemTest.SystemTest();
            this.systemTest1.Location = new System.Drawing.Point(0, 0);
            this.systemTest1.Name = "statusMonitor2";//可有可无；
            this.systemTest1.Size = new System.Drawing.Size(1024, 617);
            this.systemTest1.TabIndex = 3;//可有可无；
            this.navigationPage_sysTest.Controls.Add(systemTest1);

        }

        private void _initPages()
        {
            modulePages[0] = navigationPage_statusMonitor;  //工作状态；
            modulePages[1] = navigationPage_dataAnalysis;   //数据分析；
            modulePages[2] = navigationPage_exportExcel;    //报表生成；
            modulePages[3] = navigationPage_paraConfig; //参数设置；
            modulePages[4] = navigationPage_sysConfig;  //系统管理；
            modulePages[5] = navigationPage_sysTest;    //系统测试；
        }

        private void showCloseConfirmBox(string title, string typeConfirmationBox)
        {
            if (this.confirmationBox_applicationClose != null)
                this.confirmationBox_applicationClose.Dispose();    //释放窗体;

            this.confirmationBox_applicationClose = new CommonControl.ConfirmationBox();
            this.confirmationBox_applicationClose.Appearance.BackColor = System.Drawing.Color.White;
            this.confirmationBox_applicationClose.Appearance.Options.UseBackColor = true;
            this.confirmationBox_applicationClose.Name = "confirmationBox_applicationClose";
            this.confirmationBox_applicationClose.Size = new System.Drawing.Size(350, 150);
            this.confirmationBox_applicationClose.Location = new Point(337, 250);
            this.confirmationBox_applicationClose.TabIndex = 29;
            this.confirmationBox_applicationClose.titleConfirmationBox = title;
            this.confirmationBox_applicationClose.ConfirmationBoxOKClicked += new CommonControl.ConfirmationBox.SimpleButtonOKClickHanlder(this.confirmationBox_applicationRestart_closeOK);
            this.confirmationBox_applicationClose.ConfirmationBoxCancelClicked += new CommonControl.ConfirmationBox.SimpleButtonCancelClickHanlder(this.confirmationBox_applicationRestart_closeCancel);
            this.Controls.Add(this.confirmationBox_applicationClose);
            this.confirmationBox_applicationClose.Visible = true;
            this.confirmationBox_applicationClose.BringToFront();
        }

        private void confirmationBox_applicationRestart_closeOK(object sender, EventArgs e)
        {
            Process.GetCurrentProcess().Kill(); //Kill方法会直接结束整个进程;
        }

        private void confirmationBox_applicationRestart_closeCancel(object sender, EventArgs e)
        {
        }

        private void tileBarItem_statusMonitor_ItemClick(object sender, TileItemEventArgs e)    //工作状态事件按钮；
        {
            this.navigationFrame_mainForm.SelectedPage = modulePages[(int)modulePage.statusMonitor];    //显示工作状态主界面--modulePages[0]；
        }

        private void tileBarItem_exportExcel_ItemClick(object sender, TileItemEventArgs e)  //报表生成事件按钮；
        {
            this.navigationFrame_mainForm.SelectedPage = modulePages[(int)modulePage.exportExcel];  //显示报表生成主界面--modulePages[2]；
        }

        private void pictureEdit_CETC_DoubleClick(object sender, EventArgs e)   //双击退出程序；
        {
            showCloseConfirmBox("确认关闭软件？", "close");
        }

        /**
       **********************************************点击磁贴，显示二级子菜单按钮***********************************************************
       */
        private void tileBarItem_dataAnalysis_ItemClick(object sender, TileItemEventArgs e) //数据分析事件按钮；
        {
            this.tileBar_mainMenu.ShowDropDown(this.tileBarItem_dataAnalysis);  //显示二级子菜单(包括时域分析、频域分析)；
        }

        private void tileBarItem_paraConfig_ItemClick(object sender, TileItemEventArgs e)   //参数设置事件按钮；
        {
            this.tileBar_mainMenu.ShowDropDown(this.tileBarItem_paraConfig);    //显示二级子菜单(包括基准设置、算法设置)；
        }

        private void tileBarItem_sysConfig_ItemClick(object sender, TileItemEventArgs e)    //系统管理事件按钮；
        {
            this.tileBar_mainMenu.ShowDropDown(this.tileBarItem_sysManagement); //显示二级子菜单(包括品牌管理、标定校正、系统设置）；
        }

        private void tileBarItem_sysTest_ItemClick(object sender, TileItemEventArgs e)  //系统测试事件按钮；
        {
            this.tileBar_mainMenu.ShowDropDown(this.tileBarItem_sysTest);   //显示二级子菜单(包括信号测试、实时曲线）；
        }

        /**
       **********************************************点击二级子菜单按钮后，子菜单按钮隐藏***********************************************************
       */
        private void tileBar_dataAnalysis_ItemClick(object sender, TileItemEventArgs e)
        {
            this.tileBar_mainMenu.HideDropDownWindow(false);    //数据分析二级子菜单；
        }

        private void tileBar_paraConfig_ItemClick(object sender, TileItemEventArgs e)
        {
            this.tileBar_mainMenu.HideDropDownWindow(false);     //参数设置二级子菜单；
        }

        private void tileBar_sysConfig_ItemClick(object sender, TileItemEventArgs e)
        {
            this.tileBar_mainMenu.HideDropDownWindow(false);     //系统管理二级子菜单；
        }

        private void tileBar_sysTest_ItemClick(object sender, TileItemEventArgs e)
        {
            this.tileBar_mainMenu.HideDropDownWindow(false);     //系统测试二级子菜单；
        }

        /**
        *******************************************************二级子菜单显示***************************************************************
        */
        private void tileBarItem_dataAnalysis_timeDomainAnalysis_ItemClick(object sender, TileItemEventArgs e)  //数据分析；
        {
            this.navigationFrame_mainForm.SelectedPage = modulePages[(int)modulePage.dataAnalysis]; //modulePages[1]--数据分析;
            this.dataAnalysis1.selectedFramePage = (int)DataAnalysisPage.timeDomainAnalysis;    //时域分析--0;
        }

        private void tileBarItem_dataAnalysis_frequencyDomainAnalysis_ItemClick(object sender, TileItemEventArgs e)
        {
            this.navigationFrame_mainForm.SelectedPage = modulePages[(int)modulePage.dataAnalysis]; //modulePages[1]--数据分析;
            this.dataAnalysis1.selectedFramePage = (int)DataAnalysisPage.frequencyDomainAnalysis;   //频域分析--1；
        }
        
        private void tileBarItem_paraConfig_benchmarkConfig_ItemClick(object sender, TileItemEventArgs e)   //参数设置（基准设置）；
        {
            this.navigationFrame_mainForm.SelectedPage = modulePages[(int)modulePage.paraConfig];   //modulePages[3]--参数设置；
            this.paraConfig1.selectedFramePage = (int)ParaConfigPage.benchmarkConfig;   //基准设置--0；
        }

        private void tileBarItem_paraConfig_algorithmConfig_ItemClick(object sender, TileItemEventArgs e)   //参数设置（算法设置）；
        {
            this.navigationFrame_mainForm.SelectedPage = modulePages[(int)modulePage.paraConfig];    //modulePages[3]--参数设置；
            this.paraConfig1.selectedFramePage = (int)ParaConfigPage.algorithmConfig;   //算法设置--1；
        }

        private void tileBarItem_sysManagement_brandManagement_ItemClick(object sender, TileItemEventArgs e)    //系统管理（品牌管理）；
        {
            this.navigationFrame_mainForm.SelectedPage = modulePages[(int)modulePage.sysConfig];    //modulePages[4]--系统管理；
            this.systemManagement1.selectedFramePage = (int)SysManagementPage.brandManagement;  //品牌管理--0；
        }

        private void tileBarItem_sysManagement_calibrationCorrection_ItemClick(object sender, TileItemEventArgs e)  //系统管理（标定校正）；
        {
            this.navigationFrame_mainForm.SelectedPage = modulePages[(int)modulePage.sysConfig];    //modulePages[4]--系统管理；
            this.systemManagement1.selectedFramePage = (int)SysManagementPage.calibrationCorrection;   //标定校正--1;     
        }

        private void tileBarItem_sysManagement_sysConfig_ItemClick(object sender, TileItemEventArgs e)   //系统管理（系统设置）；
        {
            this.navigationFrame_mainForm.SelectedPage = modulePages[(int)modulePage.sysConfig];    //modulePages[4]--系统管理；
            this.systemManagement1.selectedFramePage = (int)SysManagementPage.systemConfig; //系统管理--2；
        }

        private void tileBarItem_sysTest_signalTest_ItemClick(object sender, TileItemEventArgs e)   //系统测试（信号测试）；
        {
            this.navigationFrame_mainForm.SelectedPage = modulePages[(int)modulePage.sysTest];  //modulePages[5]--系统测试；
            this.systemTest1.selectedFramePage = (int)SysTestPage.signalTest;   //信号测试--0；
        }

        private void tileBarItem_sysTest_realTimeCurve_ItemClick(object sender, TileItemEventArgs e)    //系统测试（实时曲线）；
        {
            this.navigationFrame_mainForm.SelectedPage = modulePages[(int)modulePage.sysTest];  //modulePages[5]--系统测试；
            this.systemTest1.selectedFramePage = (int)SysTestPage.realTimeCurve;    //实时曲线--1；
        }

    }
}