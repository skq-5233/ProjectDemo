/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：BackupBrands.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：Brand Management，Backup Brands页面

原作者：蒋涛
完成日期：2014/10/28
特别说明：无

修改者：无
完成日期：无
修改记录：无

****************************************************************/

using System;

namespace VisionSystemUserInterface
{
    public partial class BackupBrands : Template
    {
        //BRAND MANAGEMENT，BACKUP BRANDS页面

        public Boolean WindowDisplay = false;//是否显示窗口。取值范围：true，是；false，否

        //构造函数

        //----------------------------------------------------------------------
        // 功能说明：构造函数，初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public BackupBrands()
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
        public VisionSystemControlLibrary.BackupBrandsControl CustomControl//属性
        {
            get//读取
            {
                return backupBrandsControl;
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
            WindowDisplay = true;//是否显示窗口。取值范围：true，是；false，否

            //

            backupBrandsControl.SystemBrandPath = Global.VisionSystem.Brand.BrandPath;//属性，系统品牌路径
            backupBrandsControl.LocalDiskDefaultPath = Global.VisionSystem.Brand.BackupBrandPath;//属性，LOCAL DISK存储的备份品牌默认路径
            if ("" != Global.USBDeviceName)//存在
            {
                backupBrandsControl.USBDeviceDefaultPath = Global.USBDeviceName + Global.VisionSystem.MachineType[Global.VisionSystem.SelectedMachineType] + " " + Global.VisionSystem._GetProductName(VisionSystemClassLibrary.Enum.InterfaceLanguage.English) + "\\" + VisionSystemClassLibrary.Class.Brand.USBDeviceBackupBrandPathName;//属性，USB DEVICE存储的备份品牌默认路径
            }
            else//不存在
            {
                backupBrandsControl.USBDeviceDefaultPath = "";//属性，USB DEVICE存储的备份品牌默认路径
            }
            backupBrandsControl._Properties(Global.VisionSystem.Brand);//设置属性

            //

            if (Global.TopMostWindows)//置顶
            {
                this.TopMost = true;//将窗口置于顶层
            }
            else//其它
            {
                this.Visible = true;//显示
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
        private void BackupBrands_Load(object sender, EventArgs e)
        {
            //WindowDisplay = true;//是否显示窗口。取值范围：true，是；false，否
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【Close】按钮事件，关闭BRAND MANAGEMENT，BACKUP BRANDS窗口
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void backupBrandsControl_Close_Click(object sender, EventArgs e)
        {
            WindowDisplay = false;//是否显示窗口。取值范围：true，是；false，否

            Global.BrandManagementWindow.CustomControl._BackupBrands();//更新页面

            //

            if (Global.TopMostWindows)//置顶
            {
                Global.BrandManagementWindow.TopMost = true;//将窗口置于顶层
            }
            else//其它
            {
                this.Visible = false;//隐藏
            }
        }
    }
}