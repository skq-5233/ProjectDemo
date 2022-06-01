/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：RestoreBrands.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：Brand Management，Restore Brands页面

原作者：蒋涛
完成日期：2014/10/28
特别说明：无

修改者：无
完成日期：无
修改记录：无

****************************************************************/

using System;
using System.Windows.Forms;

namespace VisionSystemUserInterface
{
    public partial class RestoreBrands : Template
    {
        //BRAND MANAGEMENT，RESTORE BRANDS页面

        public Boolean WindowDisplay = false;//是否显示窗口。取值范围：true，是；false，否

        //构造函数

        //----------------------------------------------------------------------
        // 功能说明：构造函数，初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public RestoreBrands()
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
        public VisionSystemControlLibrary.RestoreBrandsControl CustomControl//属性
        {
            get//读取
            {
                return restoreBrandsControl;
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

            restoreBrandsControl.SystemBrandPath = Global.VisionSystem.Brand.BrandPath;//属性，系统品牌路径
            restoreBrandsControl.LocalDiskDefaultPath = Global.VisionSystem.Brand.BackupBrandPath;//属性，LOCAL DISK存储的备份品牌默认路径
            if ("" != Global.USBDeviceName)//存在
            {
                restoreBrandsControl.USBDeviceDefaultPath = Global.USBDeviceName + Global.VisionSystem.MachineType[Global.VisionSystem.SelectedMachineType] + " " + Global.VisionSystem._GetProductName(VisionSystemClassLibrary.Enum.InterfaceLanguage.English) + "\\" + VisionSystemClassLibrary.Class.Brand.USBDeviceBackupBrandPathName;//属性，USB DEVICE存储的备份品牌默认路径
            }
            else//不存在
            {
                restoreBrandsControl.USBDeviceDefaultPath = "";//属性，USB DEVICE存储的备份品牌默认路径
            }
            restoreBrandsControl._Properties(Global.VisionSystem.Brand);//设置属性

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
        private void RestoreBrands_Load(object sender, EventArgs e)
        {
            //WindowDisplay = true;//是否显示窗口。取值范围：true，是；false，否
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【Restore】按钮事件，加载品牌
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void restoreBrandsControl_Restore_Click(object sender, EventArgs e)
        {
            Int32 i = 0;//循环控制变量
            Int32 j = 0;//临时变量

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

            restoreBrandsControl.RestoreBrandsNumber = j;

            //

            if (0 == restoreBrandsControl.RestoreBrandsNumber)//无效
            {
                restoreBrandsControl._RestoreBrand(true);
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

                    Global.WorkWindow._SendCommand_Value(CommunicationInstructionType.BrandManagement_LoadBrand, Global.VisionSystem.Camera[i].Type, iCameraChooseState, i, Global.VisionSystem.Brand.BrandPath + Global.VisionSystem.Brand.CURRENTBrandName + "\\" + Global.VisionSystem.Camera[i].CameraENGName + "\\", Global.VisionSystem.Camera[i].CameraENGName, VisionSystemClassLibrary.Class.Camera.TolerancesFileName, Global.VisionSystem.Brand.CURRENTBrandName, 0);//发送指令
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：加载品牌完成，重新启动程序
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void restoreBrandsControl_RestoreBrandsSuccess(object sender, EventArgs e)
        {
            Application.ExitThread();//退出应用程序所有线程
            Application.Exit();
            System.Diagnostics.Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);//重新启动系统
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【Close】按钮事件，关闭BRAND MANAGEMENT，RESTORE BRANDS窗口
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void restoreBrandsControl_Close_Click(object sender, EventArgs e)
        {
            WindowDisplay = false;//是否显示窗口。取值范围：true，是；false，否

            Global.BrandManagementWindow.CustomControl._RestoreBrands();//更新页面

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