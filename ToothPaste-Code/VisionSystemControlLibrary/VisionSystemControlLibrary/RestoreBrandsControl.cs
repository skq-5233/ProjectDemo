/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：RestoreBrandsControl.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述： RESTORE BRANDS控件

原作者：视觉检测团队
完成日期：2014/08/18
特别说明：经视觉检测团队授权并遵守代码使用条款，方可使用本代码并获得技术支持，否则所产生的一切后果由使用者承担

修改者：无
完成日期：无
修改记录：无

****************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using System.Threading;

using System.Reflection;

namespace VisionSystemControlLibrary
{
    public partial class RestoreBrandsControl : UserControl
    {
        //该控件为Brand Management页面中的RESTORE BRANDS子页面

        private VisionSystemClassLibrary.Enum.InterfaceLanguage language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;//属性，语言

        private VisionSystemClassLibrary.Enum.DeviceState devicestate = VisionSystemClassLibrary.Enum.DeviceState.Stop;//属性，设备状态

        //

        private bool bBackupType = true;//【LOCAL DISK】和【USB DEVICE】按钮状态，取值范围：true，【LOCAL DISK】按下，【USB DEVICE】未按下；false，【USB DEVICE】按下，【LOCAL DISK】未按下

        private bool bSelectAll_BackupList = false;//Backup List列表【SELECT ALL】按钮状态，取值范围：true，按下；false，未按下

        //

        private string sSystemBrandPath = "D:\\Brand\\";//属性，系统品牌路径

        private string sLocalDiskDefaultPath = "D:\\Backup\\Brand\\";//属性，LOCAL DISK存储的备份品牌默认路径
        private string sUSBDeviceDefaultPath = "";//属性，USB DEVICE存储的备份品牌默认路径

        private string sCurrentPath_LocalDisk = "";//LOCAL DISK中当前备份品牌路径
        private string sCurrentPath_USBDevice = "";//LOCAL DISK中当前备份品牌路径

        //

        private Boolean[] bSelectionColumnIconDisplay_LocalDisk_BackupList = null;//Backup List，LOCAL DISK，是否显示选择图标。取值范围：true，是；false，否
        private Boolean[] bSelectionColumnIconDisplay_USBDevice_BackupList = null;//Backup List，USB DEVICE，是否显示选择图标。取值范围：true，是；false，否

        //

        private BackupBrandData[] BackupBrandData_LocalDisk = null;//Backup List中的LOCAL DISK数据
        private BackupBrandData[] BackupBrandData_USBDevice = null;//Backup List中的USB DEVICE数据

        //

        private const string BackupBrandStartName = "VisionSystem_";//备份品牌文件夹的名称开始文本

        //

        private VisionSystemClassLibrary.Class.Brand brand = new VisionSystemClassLibrary.Class.Brand();//属性（只读），系统品牌

        //

        private Int32 iRestoreBrandsNumber = 0;//属性，恢复品牌的设备数量

        private Boolean bRestoreBrandsMessageWindowShow = false;//属性（只读），是否显示恢复品牌后的提示信息窗口。取值范围：true，是；false，否

        private const Int32 iTimerRestoreBrandsMaxCount = 30;//定时器时间
        private Int32 iTimerRestoreBrandsCount = 30;//定时器时间

        //

        private String[][] sMessageText = new String[2][];//提示信息对话框上显示的文本（[语言][包含的文本]）

        //

        private MessageDisplayWindow messageDisplayWindow_RestoreBrands = null;//提示信息窗口（用于恢复品牌时覆盖已存在文件的确认）
        //预留数据：
        //1：【RESTORE】覆盖已存在文件确认（LOCAL DISK）
        //2：【RESTORE】覆盖已存在文件确认（USB DEVICE）

        //

        [Browsable(true), Description("点击【Close】按钮时产生的事件"), Category("BackupBrandsControl 事件")]
        public event EventHandler Close_Click;//点击【Close】按钮时产生的事件

        [Browsable(true), Description("点击【LOCAL DISK】按钮时产生的事件"), Category("BackupBrandsControl 事件")]
        public event EventHandler LocalDisk_Click;//点击【LOCAL DISK】按钮时产生的事件

        [Browsable(true), Description("点击【USB DEVICE】按钮时产生的事件"), Category("BackupBrandsControl 事件")]
        public event EventHandler USBDevice_Click;//点击【USB DEVICE】按钮时产生的事件

        [Browsable(true), Description("点击Backup List列表的【SELECT ALL】按钮时产生的事件"), Category("BackupBrandsControl 事件")]
        public event EventHandler SelectAll_BackupList_Click;//点击Backup List列表的【SELECT ALL】按钮时产生的事件

        [Browsable(true), Description("点击【DELETE】按钮时产生的事件"), Category("BackupBrandsControl 事件")]
        public event EventHandler Delete_Click;//点击【DELETE】按钮时产生的事件

        [Browsable(true), Description("点击【RESTORE】按钮时产生的事件"), Category("BackupBrandsControl 事件")]
        public event EventHandler Restore_Click;//点击【RESTORE】按钮时产生的事件

        [Browsable(true), Description("恢复品牌完成时产生的事件"), Category("BackupBrandsControl 事件")]
        public event EventHandler RestoreBrandsSuccess;//恢复品牌完成时产生的事件件

        [Browsable(true), Description("点击【Back】按钮时产生的事件"), Category("BackupBrandsControl 事件")]
        public event EventHandler Back_Click;//点击【Back】按钮时产生的事件

        [Browsable(true), Description("点击【Open】按钮时产生的事件"), Category("BackupBrandsControl 事件")]
        public event EventHandler Open_Click;//点击【Open】按钮时产生的事件

        [Browsable(true), Description("点击Brands List【Previous Page】按钮时产生的事件"), Category("BackupBrandsControl 事件")]
        public event EventHandler PreviousPage_BrandsList_Click;//点击Brands List【Previous Page】按钮时产生的事件

        [Browsable(true), Description("点击Brands List【Next Page】按钮时产生的事件"), Category("BackupBrandsControl 事件")]
        public event EventHandler NextPage_BrandsList_Click;//点击Brands List【Next Page】按钮时产生的事件

        [Browsable(true), Description("点击Backup List【Previous Page】按钮时产生的事件"), Category("BackupBrandsControl 事件")]
        public event EventHandler PreviousPage_BackupList_Click;//点击Backup List【Previous Page】按钮时产生的事件

        [Browsable(true), Description("点击Backup List【Next Page】按钮时产生的事件"), Category("BackupBrandsControl 事件")]
        public event EventHandler NextPage_BackupList_Click;//点击Backup List【Next Page】按钮时产生的事件

        [Browsable(true), Description("点击Backup List控件项时产生的事件"), Category("BackupBrandsControl 事件")]
        public event EventHandler BackUpListItem_Click;//点击Backup List控件项时产生的事件

        //

        //构造函数

        //----------------------------------------------------------------------
        // 功能说明：构造函数，初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public RestoreBrandsControl()
        {
            InitializeComponent();

            //由于该控件仅有一个实例，因此可以在此处进行事件的订阅

            if (null != GlobalWindows.MessageDisplay_Window)
            {
                GlobalWindows.MessageDisplay_Window.WindowClose_RestoreBrands_Delete_Confirm += new System.EventHandler(messageDisplayWindow_WindowClose_RestoreBrands_Delete_Confirm);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_RestoreBrands_Delete_Wait += new System.EventHandler(messageDisplayWindow_WindowClose_RestoreBrands_Delete_Wait);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_RestoreBrands_Restore_Confirm += new System.EventHandler(messageDisplayWindow_WindowClose_RestoreBrands_Restore_Confirm);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_RestoreBrands_Restore_Wait += new System.EventHandler(messageDisplayWindow_WindowClose_RestoreBrands_Restore_Wait);//订阅事件
            }

            //

            FieldInfo[] fieldinfo = VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese.GetType().GetFields();//获取公共字段
            if (null != fieldinfo && 1 < fieldinfo.Length)//有效
            {
                Int32 i = 0;//循环控制变量

                sMessageText = new String[fieldinfo.Length - 1][];
                for (i = 0; i < fieldinfo.Length - 1; i++)
                {
                    sMessageText[i] = new String[13];
                }

                //

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] = "确定恢复选择的品牌";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] = "Restore Selected Brands";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][1] = "成功";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][1] = "Completed";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][2] = "确定删除选择的品牌";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][2] = "Delete Selected Brands";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][3] = "删除选择的品牌";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][3] = "Delection of Selected Brands";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][4] = "失败";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][4] = "Failed";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][5] = "已经存在";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][5] = "Already Exists";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][6] = "确定替换品牌";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][6] = "Overwrite";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][7] = "正在恢复品牌";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][7] = "Restoring Brands";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][8] = "点击【确定】按钮重新启动程序";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][8] = "Press OK button to Restart Application";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][9] = "请重试";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][9] = "Please try again";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][10] = "请等待";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][10] = "Please wait";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][11] = "正在删除品牌";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][11] = "Deleting Brands";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][12] = "恢复选择的品牌";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][12] = "Restore of Selected Brands";
            }
        }

        //属性

        //-----------------------------------------------------------------------
        // 功能说明：Language属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("语言"), Category("RestoreBrandsControl 通用")]
        public VisionSystemClassLibrary.Enum.InterfaceLanguage Language
        {
            get//读取
            {
                return language;
            }
            set//设置
            {
                if (value != language)
                {
                    language = value;

                    //

                    _SetLanguage();
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：SystemDeviceState属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("设备状态"), Category("RestoreBrandsControl 通用")]
        public VisionSystemClassLibrary.Enum.DeviceState SystemDeviceState
        {
            get//读取
            {
                return devicestate;
            }
            set//设置
            {
                if (value != devicestate)
                {
                    devicestate = value;

                    //

                    _SetDeviceState();
                }
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：SystemBrandPath属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("系统品牌路径"), Category("RestoreBrandsControl 通用")]
        public string SystemBrandPath//属性
        {
            get//读取
            {
                return sSystemBrandPath;
            }
            set//设置
            {
                sSystemBrandPath = value;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：LocalDiskDefaultPath属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("LOCAL DISK存储的备份品牌默认路径"), Category("RestoreBrandsControl 通用")]
        public string LocalDiskDefaultPath//属性
        {
            get//读取
            {
                return sLocalDiskDefaultPath;
            }
            set//设置
            {
                sLocalDiskDefaultPath = value;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：USBDeviceDefaultPath属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("USB DEVICE存储的备份品牌默认路径"), Category("RestoreBrandsControl 通用")]
        public string USBDeviceDefaultPath//属性
        {
            get//读取
            {
                return sUSBDeviceDefaultPath;
            }
            set//设置
            {
                sUSBDeviceDefaultPath = value;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：SystemBrand属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("系统品牌"), Category("RestoreBrandsControl 通用")]
        public VisionSystemClassLibrary.Class.Brand SystemBrand//属性
        {
            get//读取
            {
                return brand;
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：RestoreBrandsNumber属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("恢复品牌的设备数量"), Category("RestoreBrandsControl 通用")]
        public Int32 RestoreBrandsNumber//属性
        {
            get//读取
            {
                return iRestoreBrandsNumber;
            }
            set//设置
            {
                iRestoreBrandsNumber = value;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：RestoreBrandsMessageWindowShow属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("是否显示恢复品牌后的提示信息窗口。取值范围：true，是；false，否"), Category("RestoreBrandsControl 通用")]
        public Boolean RestoreBrandsMessageWindowShow//属性
        {
            get//读取
            {
                return bRestoreBrandsMessageWindowShow;
            }
        }

        //函数

        //----------------------------------------------------------------------
        // 功能说明：应用设置的属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _Properties(VisionSystemClassLibrary.Class.Brand brand_parameter)
        {
            brand = brand_parameter;

            //

            sCurrentPath_LocalDisk = sLocalDiskDefaultPath;//LOCAL DISK中当前备份品牌路径
            sCurrentPath_USBDevice = sUSBDeviceDefaultPath;//LOCAL DISK中当前备份品牌路径

            _SetBrand();//应用属性设置

            _SetLanguage();//设置语言
        }

        //----------------------------------------------------------------------
        // 功能说明：设置语言
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetLanguage()
        {
            customButtonLocalDisk.Language = language;//【LOCAL DISK】
            customButtonUSBDevice.Language = language;//设置【USB DEVICE】按钮背景
            customButtonSelectAll_BackupList.Language = language;//Backup List，【SELECT ALL】
            customButtonDelete.Language = language;//【DELETE】
            customButtonRestore.Language = language;//【RESTORE】

            customButtonClose.Language = language;//【Close】
            customButtonBack.Language = language;//【Back】
            customButtonOpen.Language = language;//【Open】
            customButtonPreviousPage_BackupList.Language = language;//Backup List，【Previous Page】
            customButtonNextPage_BackupList.Language = language;//Backup List，【Next Page】
            customButtonPreviousPage_BrandsList.Language = language;//Brands List，【Previous Page】
            customButtonNextPage_BrandsList.Language = language;//Brands List，【Next Page】

            //

            customButtonCaption.Language = language;//标题文本
            customButtonMessage.Language = language;//提示信息文本
            customButtonBackupList.Language = language;//Backup List列表名称文本
            customButtonBrandsList.Language = language;//Brands List列表名称文本

            //

            customListBackupList.Language = language;//Backup List列表
            customListBrandsList.Language = language;//Brands List列表
        }

        //----------------------------------------------------------------------
        // 功能说明：用户调用，设备状态设置完成后，更新页面
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetDeviceState()
        {
            //运行时无效

            if (VisionSystemClassLibrary.Enum.DeviceState.Run == devicestate)//运行
            {
                //更新按钮状态

                customButtonLocalDisk.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【LOCAL DISK】
                customButtonUSBDevice.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【USB DEVICE】
                customButtonSelectAll_BackupList.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【SELECT ALL】（Backup List）
                customButtonDelete.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【DELETE】
                customButtonRestore.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【RESTORE】
                customButtonBack.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【Back】
                customButtonOpen.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【Open】
                customButtonPreviousPage_BrandsList.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//Brands List【Previous Page】
                customButtonNextPage_BrandsList.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//Brands List【Next Page】
                customButtonPreviousPage_BackupList.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//Backup List【Previous Page】
                customButtonNextPage_BackupList.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//Backup List【Next Page】
                customListBrandsList.ListEnabled = false;//Brands List
                customListBackupList.ListEnabled = false;//Backup List
            }
            else//停止
            {
                //更新按钮状态

                _SetControl();//更新

                //

                customListBrandsList.ListEnabled = true;//Brands List
                customListBackupList.ListEnabled = true;//Backup List
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：应用设置的属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetBrand()
        {
            //Backup List

            _GetBackupListData(sCurrentPath_LocalDisk, ref BackupBrandData_LocalDisk, ref bSelectionColumnIconDisplay_LocalDisk_BackupList);//获取LOCAL DISK备份文件信息
            _GetBackupListData(sCurrentPath_USBDevice, ref BackupBrandData_USBDevice, ref bSelectionColumnIconDisplay_USBDevice_BackupList);//获取USB DEVICE备份文件信息

            if (bBackupType)//【LOCAL DISK】按下
            {
                if (null != BackupBrandData_LocalDisk)//有效
                {
                    _InitList(customListBackupList, BackupBrandData_LocalDisk.Length, BackupBrandData_LocalDisk, bSelectionColumnIconDisplay_LocalDisk_BackupList);//初始化列表
                }
                else//无效
                {
                    _InitList(customListBackupList, 0, BackupBrandData_LocalDisk, bSelectionColumnIconDisplay_LocalDisk_BackupList);//初始化列表
                }

                labelPath.Text = sCurrentPath_LocalDisk;//Backup List路径
            }
            else//【USB DEVICE】按下
            {
                if (null != BackupBrandData_USBDevice)//有效
                {
                    _InitList(customListBackupList, BackupBrandData_USBDevice.Length, BackupBrandData_USBDevice, bSelectionColumnIconDisplay_USBDevice_BackupList);//初始化列表
                }
                else//无效
                {
                    _InitList(customListBackupList, 0, BackupBrandData_USBDevice, bSelectionColumnIconDisplay_USBDevice_BackupList);//初始化列表
                }

                labelPath.Text = sCurrentPath_USBDevice;//Backup List路径
            }

            _GstSelectAllButtonState(customListBackupList, ref bSelectAll_BackupList);

            //Brands List

            _InitList(customListBrandsList, brand.BrandNumber);//初始化列表

            //

            _SetControl();//更新控件
        }

        //----------------------------------------------------------------------
        // 功能说明：初始化Brands List列表
        // 输入参数：1.customList：列表
        //         2.iItemNumber：列表项数目
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _InitList(CustomList customList, Int32 iItemNumber)
        {
            customList._ApplyListHeader();//应用列表头属性
            customList._ApplyListItem();//应用列表项属性

            //

            _AddItemData(customList, iItemNumber);
        }

        //----------------------------------------------------------------------
        // 功能说明：初始化Backup List列表
        // 输入参数：1.customList：列表
        //         2.iItemNumber：列表项数目
        //         3.backupbranddata：备份品牌信息
        //         4.bSelectionColumnIconDisplay_BackupList：是否显示选择图标。取值范围：true，是；false，否
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _InitList(CustomList customList, Int32 iItemNumber, BackupBrandData[] backupbranddata, Boolean[] bSelectionColumnIconDisplay_BackupList)
        {
            customList._ApplyListHeader();//应用列表头属性
            customList._ApplyListItem();//应用列表项属性

            //

            _AddItemData(customList, iItemNumber, backupbranddata, bSelectionColumnIconDisplay_BackupList);
        }

        //----------------------------------------------------------------------
        // 功能说明：USB Device设备发生变化时，调用该函数更新控件
        // 输入参数：1.sPath：设备中保存备份文件的路径。移除设备时，将其值置为""，接入设备时，置为有效路径值
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _UpdateUSBDevice(String sPath = "")
        {
            sUSBDeviceDefaultPath = sPath;
            sCurrentPath_USBDevice = sPath;

            //

            if (!bBackupType)//【USB DEVICE】按下
            {
                _GetBackupListData(sCurrentPath_USBDevice, ref BackupBrandData_USBDevice, ref bSelectionColumnIconDisplay_USBDevice_BackupList);//获取USB DEVICE备份文件信息

                if (null != BackupBrandData_USBDevice)//有效
                {
                    _AddItemData(customListBackupList, BackupBrandData_USBDevice.Length, BackupBrandData_USBDevice, bSelectionColumnIconDisplay_USBDevice_BackupList);
                }
                else//无效
                {
                    _AddItemData(customListBackupList, 0, BackupBrandData_USBDevice, bSelectionColumnIconDisplay_USBDevice_BackupList);
                }

                labelPath.Text = sCurrentPath_USBDevice;//Backup List路径

                //

                _GstSelectAllButtonState(customListBackupList, ref bSelectAll_BackupList);//获取列表项选择状态

                //

                _SetControl();//更新控件
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：恢复品牌完成
        // 输入参数：1.bSuccess：操作是否成功。取值范围：true，是；false，否
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _RestoreBrand(Boolean bSuccess)
        {
            if (bSuccess)//成功
            {
                iRestoreBrandsNumber--;
            }

            if (0 >= iRestoreBrandsNumber)//恢复完成
            {
                bRestoreBrandsMessageWindowShow = false;

                iTimerRestoreBrandsCount = iTimerRestoreBrandsMaxCount;

                timerRestoreBrands.Stop();//关闭定时器

                //显示信息对话框

                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][12] + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][1];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][12] + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][1];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][8];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][8];
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：设置页面控件的背景图像
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetControl()
        {
            if (bBackupType)//【LOCAL DISK】按钮按下
            {
                customButtonLocalDisk.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;

                customButtonUSBDevice.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;

                //

                if (sLocalDiskDefaultPath == sCurrentPath_LocalDisk)//默认路径
                {
                    customButtonBack.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                } 
                else//进入了子目录
                {
                    customButtonBack.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                }

                //

                if ("" != sCurrentPath_LocalDisk && Directory.Exists(sCurrentPath_LocalDisk.Substring(0, sCurrentPath_LocalDisk.Length - 1)))//路径存在
                {
                    DirectoryInfo directoryinfo = new DirectoryInfo(sCurrentPath_LocalDisk.Substring(0, sCurrentPath_LocalDisk.Length - 1));

                    if (directoryinfo.Name.StartsWith(BackupBrandStartName))//备份品牌文件夹
                    {
                        if (0 < customListBackupList.SelectedItemNumber)//选择了一项
                        {
                            customButtonRestore.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                        }
                        else//未选择任何项
                        {
                            customButtonRestore.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                        }

                        //

                        customButtonDelete.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;

                        //

                        if (0 < customListBackupList.ItemDataNumber)//Backup List列表不为空
                        {
                            if (bSelectAll_BackupList)//【SELECT ALL】按钮按下
                            {
                                customButtonSelectAll_BackupList.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
                            }
                            else//【SELECT ALL】按钮未按下
                            {
                                customButtonSelectAll_BackupList.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                            }
                        }
                        else//Backup List列表为空
                        {
                            customButtonSelectAll_BackupList.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                        }
                    } 
                    else//其它文件夹
                    {
                        customButtonRestore.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;

                        //

                        if (0 < customListBackupList.SelectedItemNumber)//选择了列表项
                        {
                            customButtonDelete.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                        }
                        else//未选择列表项
                        {
                            customButtonDelete.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                        }

                        //

                        if (0 < customListBackupList.ItemDataNumber)//Backup List列表不为空
                        {
                            if (bSelectAll_BackupList)//【SELECT ALL】按钮按下
                            {
                                customButtonSelectAll_BackupList.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
                            }
                            else//【SELECT ALL】按钮未按下
                            {
                                customButtonSelectAll_BackupList.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                            }
                        }
                        else//Backup List列表为空
                        {
                            customButtonSelectAll_BackupList.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                        }
                    }
                } 
                else//路径不存在
                {
                    customButtonRestore.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;

                    customButtonDelete.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;

                    customButtonSelectAll_BackupList.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                }
            }
            else//【USB DEVICE】按钮按下
            {
                customButtonUSBDevice.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;

                customButtonLocalDisk.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;

                //

                if (sUSBDeviceDefaultPath == sCurrentPath_USBDevice)//默认路径
                {
                    customButtonBack.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                } 
                else//进入了子目录
                {
                    customButtonBack.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                }

                //

                if ("" != sCurrentPath_USBDevice && Directory.Exists(sCurrentPath_USBDevice.Substring(0, sCurrentPath_USBDevice.Length - 1)))//路径存在
                {
                    DirectoryInfo directoryinfo = new DirectoryInfo(sCurrentPath_USBDevice.Substring(0, sCurrentPath_USBDevice.Length - 1));

                    if (directoryinfo.Name.StartsWith(BackupBrandStartName))//备份品牌文件夹
                    {
                        if (0 < customListBackupList.SelectedItemNumber)//选择了一项
                        {
                            customButtonRestore.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                        }
                        else//其它
                        {
                            customButtonRestore.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                        }

                        //

                        customButtonDelete.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;

                        //

                        if (0 < customListBackupList.ItemDataNumber)//Backup List列表不为空
                        {
                            if (bSelectAll_BackupList)//【SELECT ALL】按钮按下
                            {
                                customButtonSelectAll_BackupList.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
                            }
                            else//【SELECT ALL】按钮未按下
                            {
                                customButtonSelectAll_BackupList.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                            }
                        }
                        else//Backup List列表为空
                        {
                            customButtonSelectAll_BackupList.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                        }
                    }
                    else//其它文件夹
                    {
                        customButtonRestore.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;

                        //

                        if (0 < customListBackupList.SelectedItemNumber)//选择了列表项
                        {
                            customButtonDelete.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                        }
                        else//未选择列表项
                        {
                            customButtonDelete.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                        }

                        //

                        if (0 < customListBackupList.ItemDataNumber)//Backup List列表不为空
                        {
                            if (bSelectAll_BackupList)//【SELECT ALL】按钮按下
                            {
                                customButtonSelectAll_BackupList.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
                            }
                            else//【SELECT ALL】按钮未按下
                            {
                                customButtonSelectAll_BackupList.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                            }
                        }
                        else//Backup List列表为空
                        {
                            customButtonSelectAll_BackupList.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                        }
                    }
                }
                else//路径不存在
                {
                    customButtonRestore.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;

                    customButtonDelete.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;

                    customButtonSelectAll_BackupList.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                }
            }

            //

            if (0 < customListBackupList.ItemDataNumber)//Backup List列表不为空
            {
                if (0 <= customListBackupList.CurrentDataIndex)//选择了列表项
                {
                    if (bBackupType)//【LOCAL DISK】按下
                    {
                        if (1 == BackupBrandData_LocalDisk[customListBackupList.CurrentDataIndex].Type)//备份品牌文件夹中的子文件夹
                        {
                            customButtonOpen.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【Open】
                        } 
                        else//其它
                        {
                            customButtonOpen.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【Open】
                        }
                    }
                    else//【USB DEVICE】按下
                    {
                        if (1 == BackupBrandData_USBDevice[customListBackupList.CurrentDataIndex].Type)//备份品牌文件夹中的子文件夹
                        {
                            customButtonOpen.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【Open】
                        }
                        else//其它
                        {
                            customButtonOpen.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【Open】
                        }
                    }
                } 
                else//未选择列表项
                {
                    customButtonOpen.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【Open】
                }
            }
            else//Backup List列表为空
            {
                customButtonOpen.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【Open】
            }

            //

            customButtonClose.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【Close】

            //

            customButtonPreviousPage_BrandsList.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//Brands List【Previous Page】
            customButtonNextPage_BrandsList.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//Brands List【Next Page】

            if (1 < customListBrandsList.TotalPage)//多于一页
            {
                customButtonPreviousPage_BrandsList.Visible = true;//Brands List【Previous Page】
                customButtonNextPage_BrandsList.Visible = true;//Brands List【Next Page】
            }
            else//小于等于一页
            {
                customButtonPreviousPage_BrandsList.Visible = false;//Brands List【Previous Page】
                customButtonNextPage_BrandsList.Visible = false;//Brands List【Next Page】
            }

            //

            customButtonPreviousPage_BackupList.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//Backup List【Previous Page】
            customButtonNextPage_BackupList.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//Backup List【Next Page】

            if (1 < customListBackupList.TotalPage)//多于一页
            {
                customButtonPreviousPage_BackupList.Visible = true;//Backup List【Previous Page】
                customButtonNextPage_BackupList.Visible = true;//Backup List【Next Page】
            }
            else//小于等于一页
            {
                customButtonPreviousPage_BackupList.Visible = false;//Backup List【Previous Page】
                customButtonNextPage_BackupList.Visible = false;//Backup List【Next Page】
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：添加Brands List列表数据
        // 输入参数：1.customList：列表
        //         2.iItemNumber：列表项数目
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _AddItemData(CustomList customList, Int32 iItemNumber)
        {
            //列表项数目

            //customList.ItemDataNumber = iItemNumber;

            //

            customList._Apply(iItemNumber);//应用列表属性

            //添加列表项数据

            _AddItemData();

            //设置列表项数据

            _SetPage(customList);
        }

        //----------------------------------------------------------------------
        // 功能说明：添加Backup List列表数据
        // 输入参数：1.customList：列表
        //         2.iItemNumber：列表项数目
        //         3.backupbranddata：备份文件信息
        //         4.bSelectionColumnIconDisplay_BackupList：是否显示选择图标。取值范围：true，是；false，否
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _AddItemData(CustomList customList, Int32 iItemNumber, BackupBrandData[] backupbranddata, Boolean[] bSelectionColumnIconDisplay_BackupList)
        {
            //列表项数目

            //customList.ItemDataNumber = iItemNumber;

            //

            customList._Apply(iItemNumber);//应用列表属性

            //添加列表项数据

            _AddItemData(backupbranddata, bSelectionColumnIconDisplay_BackupList);

            //设置列表项数据

            _SetPage(customList);
        }

        //----------------------------------------------------------------------
        // 功能说明：添加Brands List列表数据
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _AddItemData()
        {
            Int32 i = 0;//循环控制变量

            for (i = 0; i < customListBrandsList.ItemDataNumber; i++)//列表项数据
            {
                if (VisionSystemClassLibrary.Enum.BrandType.Master == brand.SystemBrandData[i].Type)//MASTER类型的品牌
                {
                    customListBrandsList.ItemData[i].ItemText[0] = brand.SystemBrandData[i].Name + "（M）";
                }
                else if (VisionSystemClassLibrary.Enum.BrandType.Current == brand.SystemBrandData[i].Type)//CURRENT类型的品牌
                {
                    customListBrandsList.ItemData[i].ItemText[0] = brand.SystemBrandData[i].Name + "（C）";
                }
                else//其它
                {
                    customListBrandsList.ItemData[i].ItemText[0] = brand.SystemBrandData[i].Name;
                }

                //

                customListBrandsList.ItemData[i].ItemDataDisplay[0] = true;//文本

                //

                customListBrandsList.ItemData[i].ItemIconIndex[0] = -1;//图标

                //

                customListBrandsList.ItemData[i].ItemFlag = i;
            }
            customListBrandsList.SelectedItemNumber = 0;
        }

        //----------------------------------------------------------------------
        // 功能说明：添加Backup List列表数据
        // 输入参数：1.backupbranddata：备份文件信息
        //         2.bSelectionColumnIconDisplay_BackupList：是否显示选择图标。取值范围：true，是；false，否
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _AddItemData(BackupBrandData[] backupbranddata, Boolean[] bSelectionColumnIconDisplay_BackupList)
        {
            Int32 i = 0;//循环控制变量
            Int32 j = 0;//循环控制变量

            if (null != backupbranddata)//有效
            {
                for (i = 0; i < backupbranddata.Length; i++)//列表项数据
                {
                    customListBackupList.ItemData[i].ItemText[1] = backupbranddata[i].DataInfo.Name;//文本

                    //

                    customListBackupList.ItemData[i].ItemDataDisplay[0] = false;//图标
                    customListBackupList.ItemData[i].ItemDataDisplay[1] = true;//文本
                    customListBackupList.ItemData[i].ItemDataDisplay[2] = !(bSelectionColumnIconDisplay_BackupList[i]);//图标（Selectd列，初始时不显示）

                    if (bSelectionColumnIconDisplay_BackupList[i])//选择
                    {
                        j++;
                    }

                    //

                    customListBackupList.ItemData[i].ItemIconIndex[0] = backupbranddata[i].IconIndex;//图标
                    customListBackupList.ItemData[i].ItemIconIndex[1] = -1;//图标
                    customListBackupList.ItemData[i].ItemIconIndex[2] = 3;//图标

                    //

                    customListBackupList.ItemData[i].ItemFlag = i;
                }
                customListBackupList.SelectedItemNumber = j;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：设置列表控件
        // 输入参数：1.customList：列表
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetPage(CustomList customList)
        {
            customList._SetPage();//设置列表项

            _SetControl();//更新控件
        }

        //----------------------------------------------------------------------
        // 功能说明：获取Backup List列表数据
        // 输入参数：1.sBackupListPath：备份文件路径
        //         2.backupbranddata：路径中的文件信息
        //         3.bSelectionColumnIconDisplay_BackupList：是否显示选择图标。取值范围：true，是；false，否
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _GetBackupListData(string sBackupListPath, ref BackupBrandData[] backupbranddata, ref Boolean[] bSelectionColumnIconDisplay_BackupList)
        {
            if ("" != sBackupListPath && Directory.Exists(sBackupListPath.Substring(0, sBackupListPath.Length - 1)))
            {
                DirectoryInfo directoryinfo = new DirectoryInfo(sBackupListPath.Substring(0, sBackupListPath.Length - 1));//路径信息

                DirectoryInfo[] directoryinfoArray = directoryinfo.GetDirectories();//路径中的文件夹信息

                if (null != directoryinfoArray)//有效
                {
                    Int32 i = 0;//循环控制变量

                    BackupBrandData[] backupbranddata_temp = new BackupBrandData[directoryinfoArray.Length];//符合要求的文件数据

                    //

                    if (directoryinfo.Name.StartsWith(BackupBrandStartName))//备份品牌文件夹
                    {
                        for (i = 0; i < directoryinfoArray.Length; i++)//获取备份品牌
                        {
                            backupbranddata_temp[i].DataInfo = directoryinfoArray[i];
                            backupbranddata_temp[i].Name = directoryinfoArray[i].Name;
                            backupbranddata_temp[i].IconIndex = 2;
                            backupbranddata_temp[i].Type = 1;
                        }
                    } 
                    else//其它文件夹
                    {
                        for (i = 0; i < directoryinfoArray.Length; i++)//获取文件信息
                        {
                            backupbranddata_temp[i].DataInfo = directoryinfoArray[i];

                            if (directoryinfoArray[i].Name.StartsWith(BackupBrandStartName))//备份品牌文件夹
                            {
                                backupbranddata_temp[i].Name = directoryinfoArray[i].Name.Substring(directoryinfoArray[i].Name.Length - BackupBrandStartName.Length);
                                backupbranddata_temp[i].IconIndex = 0;
                                backupbranddata_temp[i].Type = 2;
                            }
                            else//其它文件夹
                            {
                                backupbranddata_temp[i].Name = directoryinfoArray[i].Name;
                                backupbranddata_temp[i].IconIndex = 1;
                                backupbranddata_temp[i].Type = 3;
                            }
                        }
                    }

                    //

                    bSelectionColumnIconDisplay_BackupList = new Boolean[directoryinfoArray.Length];

                    for (i = 0; i < bSelectionColumnIconDisplay_BackupList.Length; i++)//
                    {
                        bSelectionColumnIconDisplay_BackupList[i] = false;//初始化
                    }

                    //

                    backupbranddata = new BackupBrandData[directoryinfoArray.Length];

                    System.Array.Copy(backupbranddata_temp, 0, backupbranddata, 0, directoryinfoArray.Length);
                }
                else//无效
                {
                    backupbranddata = null;

                    bSelectionColumnIconDisplay_BackupList = null;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：恢复品牌后，调用该函数重新加载系统品牌数据
        // 输入参数：1.sBackupListPath：备份文件路径
        //         2.backupbranddata：路径中的文件信息
        //         3.bSelectionColumnIconDisplay_BackupList：是否显示选择图标。取值范围：true，是；false，否
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _GetBrandsListData()
        {
            if (Directory.Exists(sSystemBrandPath.Substring(0, sSystemBrandPath.Length - 1)))
            {
                DirectoryInfo directoryinfo = new DirectoryInfo(sSystemBrandPath.Substring(0, sSystemBrandPath.Length - 1));//路径信息

                DirectoryInfo[] directoryinfoArray = directoryinfo.GetDirectories();//路径中的文件夹信息

                if (null != directoryinfoArray)//有效
                {
                    Int32 i = 0;//循环控制变量
                    Int32 j = 0;//循环控制变量

                    VisionSystemClassLibrary.Struct.BrandData[] SystemBrandData = new VisionSystemClassLibrary.Struct.BrandData[directoryinfoArray.Length];//申请内存空间

                    //

                    for (i = 0; i < directoryinfoArray.Length; i++)//获取系统品牌
                    {
                        SystemBrandData[i] = new VisionSystemClassLibrary.Struct.BrandData();
                        SystemBrandData[i].Name = directoryinfoArray[i].Name;//品牌名称
                        SystemBrandData[i].Type = VisionSystemClassLibrary.Enum.BrandType.None;//品牌类型
                    }

                    for (i = 0; i < SystemBrandData.Length; i++)//
                    {
                        for (j = 0; j < brand.BrandNumber; j++)//
                        {
                            if (SystemBrandData[i].Name == brand.SystemBrandData[j].Name)//同名
                            {
                                SystemBrandData[i].Type = brand.SystemBrandData[j].Type;//品牌类型

                                if (VisionSystemClassLibrary.Enum.BrandType.Current == brand.SystemBrandData[j].Type)//当前品牌
                                {
                                    brand.CURRENTBrandIndex = i;
                                }
                            }
                        }
                    }

                    //

                    brand.BrandNumber = (UInt16)(SystemBrandData.Length);//品牌数量

                    brand.SystemBrandData = new VisionSystemClassLibrary.Struct.BrandData[brand.BrandNumber];//品牌数据
                    SystemBrandData.CopyTo(brand.SystemBrandData, 0);

                    //

                    brand._Write(brand.CURRENTBrandIndex, 5);//文件操作
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：获取列表项选择状态
        // 输入参数：1.customList：列表
        //         2.bSelectAll：选择状态
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _GstSelectAllButtonState(CustomList customList, ref Boolean bSelectAll)
        {
            if (0 < customList.ItemDataNumber)//存在有效项
            {
                if (customList.ItemDataNumber == customList.SelectedItemNumber)//全选
                {
                    bSelectAll = true;
                }
                else//未全选
                {
                    bSelectAll = false;
                }
            }
            else//不存在有效项
            {
                bSelectAll = false;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：检查恢复的品牌在系统中是否存在
        // 输入参数：1.ListItemData：列表项数据
        // 输出参数：无
        // 返回值：是否存在。取值范围：true，否；false，是
        //----------------------------------------------------------------------
        private Boolean _CheckBrand(CustomListItemData ListItemData)
        {
            Int32 i = 0;//循环控制变量

            for (i = 0; i < brand.BrandNumber; i++)//
            {
                if (ListItemData.ItemText[1] == brand.SystemBrandData[i].Name)//存在
                {
                    break;
                }
            }
            if (i < brand.BrandNumber)//存在
            {
                return false;
            }
            else//不存在
            {
                return true;
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
        private void RestoreBrandsControl_Load(object sender, EventArgs e)
        {
            //不执行操作
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：点击【Close】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonClose_CustomButton_Click(object sender, EventArgs e)
        {
            //事件

            if (null != Close_Click)//有效
            {
                Close_Click(this, new CustomEventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【LOCAL DISK】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonLocalDisk_CustomButton_Click(object sender, EventArgs e)
        {
            if (!bBackupType)//【USB DEVICE】按钮按下
            {
                bBackupType = true;//更新数值

                //

                customButtonUSBDevice.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【USB DEVICE】

                //

                if (null != BackupBrandData_LocalDisk)//有效
                {
                    _AddItemData(customListBackupList, BackupBrandData_LocalDisk.Length, BackupBrandData_LocalDisk, bSelectionColumnIconDisplay_LocalDisk_BackupList);//更新Backup List列表
                }
                else//无效
                {
                    _AddItemData(customListBackupList, 0, BackupBrandData_LocalDisk, bSelectionColumnIconDisplay_LocalDisk_BackupList);//更新Backup List列表
                }

                //

                _GstSelectAllButtonState(customListBackupList, ref bSelectAll_BackupList);//获取列表项选择状态

                customButtonSelectAll_BackupList.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//Backup List列表【SELECT ALL】

                //

                labelPath.Text = sCurrentPath_LocalDisk;//Backup List路径

                //

                _SetControl();//更新控件
            }
            else//【LOCAL DISK】按钮按下
            {
                customButtonLocalDisk.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//【LOCAL DISK】
            }

            //事件

            if (null != LocalDisk_Click)//有效
            {
                LocalDisk_Click(this, new CustomEventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【USB DEVICE】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonUSBDevice_CustomButton_Click(object sender, EventArgs e)
        {
            if (bBackupType)//【LOCAL DISK】按钮按下
            {
                bBackupType = false;//更新数值

                //

                customButtonLocalDisk.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【LOCAL DISK】

                //

                if (null != BackupBrandData_USBDevice)//有效
                {
                    _AddItemData(customListBackupList, BackupBrandData_USBDevice.Length, BackupBrandData_USBDevice, bSelectionColumnIconDisplay_USBDevice_BackupList);//更新Backup List列表
                }
                else//无效
                {
                    _AddItemData(customListBackupList, 0, BackupBrandData_USBDevice, bSelectionColumnIconDisplay_USBDevice_BackupList);//更新Backup List列表
                }

                //

                labelPath.Text = sCurrentPath_USBDevice;//Backup List路径

                //

                _GstSelectAllButtonState(customListBackupList, ref bSelectAll_BackupList);//获取列表项选择状态

                customButtonSelectAll_BackupList.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//Backup List列表【SELECT ALL】

                //

                _SetControl();//更新控件
            }
            else//【USB DEVICE】按钮按下
            {
                customButtonUSBDevice.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//【USB DEVICE】
            }

            //事件

            if (null != USBDevice_Click)//有效
            {
                USBDevice_Click(this, new CustomEventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【Back】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonBack_CustomButton_Click(object sender, EventArgs e)
        {
            if (bBackupType)//【LOCAL DISK】按下
            {
                DirectoryInfo directoryinfo = new DirectoryInfo(sCurrentPath_LocalDisk.Substring(0, sCurrentPath_LocalDisk.Length - 1));//路径信息

                sCurrentPath_LocalDisk = directoryinfo.Parent.FullName + "\\";//更新当前路径

                labelPath.Text = sCurrentPath_LocalDisk;//路径信息控件

                //

                _GetBackupListData(sCurrentPath_LocalDisk, ref BackupBrandData_LocalDisk, ref bSelectionColumnIconDisplay_LocalDisk_BackupList);//获取LOCAL DISK备份文件信息

                if (null != BackupBrandData_LocalDisk)//有效
                {
                    _AddItemData(customListBackupList, BackupBrandData_LocalDisk.Length, BackupBrandData_LocalDisk, bSelectionColumnIconDisplay_LocalDisk_BackupList);
                }
                else//无效
                {
                    _AddItemData(customListBackupList, 0, BackupBrandData_LocalDisk, bSelectionColumnIconDisplay_LocalDisk_BackupList);
                }

                _GstSelectAllButtonState(customListBackupList, ref bSelectAll_BackupList);//获取列表项选择状态

                _SetControl();//更新控件
            }
            else//【USB DEVICE】按钮按下
            {
                DirectoryInfo directoryinfo = new DirectoryInfo(sCurrentPath_USBDevice.Substring(0, sCurrentPath_USBDevice.Length - 1));//路径信息

                sCurrentPath_USBDevice = directoryinfo.Parent.FullName + "\\";//更新当前路径

                labelPath.Text = sCurrentPath_USBDevice;//路径信息控件

                //

                _GetBackupListData(sCurrentPath_USBDevice, ref BackupBrandData_USBDevice, ref bSelectionColumnIconDisplay_USBDevice_BackupList);//获取USB DEVICE备份文件信息

                if (null != BackupBrandData_USBDevice)//有效
                {
                    _AddItemData(customListBackupList, BackupBrandData_USBDevice.Length, BackupBrandData_USBDevice, bSelectionColumnIconDisplay_USBDevice_BackupList);
                }
                else//无效
                {
                    _AddItemData(customListBackupList, 0, BackupBrandData_USBDevice, bSelectionColumnIconDisplay_USBDevice_BackupList);
                }

                _GstSelectAllButtonState(customListBackupList, ref bSelectAll_BackupList);//获取列表项选择状态

                _SetControl();//更新控件
            }

            //事件

            if (null != Back_Click)//有效
            {
                Back_Click(this, new CustomEventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【Open】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonOpen_CustomButton_Click(object sender, EventArgs e)
        {
            if (bBackupType)//【LOCAL DISK】按下
            {
                sCurrentPath_LocalDisk += BackupBrandData_LocalDisk[customListBackupList.CurrentDataIndex].DataInfo.Name + "\\";//更新当前路径

                labelPath.Text = sCurrentPath_LocalDisk;//路径信息控件

                //

                _GetBackupListData(sCurrentPath_LocalDisk, ref BackupBrandData_LocalDisk, ref bSelectionColumnIconDisplay_LocalDisk_BackupList);//获取LOCAL DISK备份文件信息

                if (null != BackupBrandData_LocalDisk)//有效
                {
                    _AddItemData(customListBackupList, BackupBrandData_LocalDisk.Length, BackupBrandData_LocalDisk, bSelectionColumnIconDisplay_LocalDisk_BackupList);
                }
                else//无效
                {
                    _AddItemData(customListBackupList, 0, BackupBrandData_LocalDisk, bSelectionColumnIconDisplay_LocalDisk_BackupList);
                }

                _GstSelectAllButtonState(customListBackupList, ref bSelectAll_BackupList);//获取列表项选择状态

                _SetControl();//更新控件
            }
            else//【USB DEVICE】按钮按下
            {
                sCurrentPath_USBDevice += BackupBrandData_USBDevice[customListBackupList.CurrentDataIndex].DataInfo.Name + "\\";//更新当前路径

                labelPath.Text = sCurrentPath_USBDevice;//路径信息控件

                //

                _GetBackupListData(sCurrentPath_USBDevice, ref BackupBrandData_USBDevice, ref bSelectionColumnIconDisplay_USBDevice_BackupList);//获取USB DEVICE备份文件信息

                if (null != BackupBrandData_USBDevice)//有效
                {
                    _AddItemData(customListBackupList, BackupBrandData_USBDevice.Length, BackupBrandData_USBDevice, bSelectionColumnIconDisplay_USBDevice_BackupList);
                }
                else//无效
                {
                    _AddItemData(customListBackupList, 0, BackupBrandData_USBDevice, bSelectionColumnIconDisplay_USBDevice_BackupList);
                }

                _GstSelectAllButtonState(customListBackupList, ref bSelectAll_BackupList);//获取列表项选择状态

                _SetControl();//更新控件
            }

            //事件

            if (null != Open_Click)//有效
            {
                Open_Click(this, new CustomEventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击Backup List列表的【SELECT ALL】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonSelectAll_BackupList_CustomButton_Click(object sender, EventArgs e)
        {
            bSelectAll_BackupList = !bSelectAll_BackupList;//更新数值

            //

            customListBackupList._SelectAll(bSelectAll_BackupList);//全部选择

            //

            Int32 i = 0;//循环控制变量

            if (bBackupType)//【LOCAL DISK】按下
            {
                for (i = 0; i < bSelectionColumnIconDisplay_LocalDisk_BackupList.Length; i++)//遍历
                {
                    bSelectionColumnIconDisplay_LocalDisk_BackupList[i] = true;//选择
                }
            }
            else//【USB DEVICE】按下
            {
                for (i = 0; i < bSelectionColumnIconDisplay_USBDevice_BackupList.Length; i++)//遍历
                {
                    bSelectionColumnIconDisplay_USBDevice_BackupList[i] = true;//选择
                }
            }

            //

            _SetControl();//更新控件

            //事件

            if (null != SelectAll_BackupList_Click)//有效
            {
                SelectAll_BackupList_Click(this, new CustomEventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【DELETE】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonDelete_CustomButton_Click(object sender, EventArgs e)
        {
            //显示信息对话框

            GlobalWindows.MessageDisplay_Window.WindowParameter = 45;//窗口特征数值
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.OkCancel;//包含【确定】和【取消】按钮
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][2] + "？";
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][2] + "？";

            GlobalWindows.MessageDisplay_Window.StartPosition = FormStartPosition.CenterScreen;
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.MessageDisplay_Window.TopMost = true;//将窗口置于顶层
            }
            else//其它
            {
                GlobalWindows.MessageDisplay_Window.Visible = true;//显示
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【RESTORE】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonRestore_CustomButton_Click(object sender, EventArgs e)
        {
            //显示信息对话框

            GlobalWindows.MessageDisplay_Window.WindowParameter = 48;//窗口特征数值
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.OkCancel;//包含【确定】和【取消】按钮
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] + "？";
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] + "？";

            GlobalWindows.MessageDisplay_Window.StartPosition = FormStartPosition.CenterScreen;
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.MessageDisplay_Window.TopMost = true;//将窗口置于顶层
            }
            else//其它
            {
                GlobalWindows.MessageDisplay_Window.Visible = true;//显示
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击Backup List【Previous Page】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonPreviousPage_BackupList_CustomButton_Click(object sender, EventArgs e)
        {
            customListBackupList._ClickPage(true);//翻页

            //事件

            if (null != PreviousPage_BackupList_Click)//有效
            {
                PreviousPage_BackupList_Click(this, new CustomEventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击Backup List【Next Page】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonNextPage_BackupList_CustomButton_Click(object sender, EventArgs e)
        {
            customListBackupList._ClickPage(false);//翻页

            //事件

            if (null != NextPage_BackupList_Click)//有效
            {
                NextPage_BackupList_Click(this, new CustomEventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击Brands List【Previous Page】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonPreviousPage_BrandsList_CustomButton_Click(object sender, EventArgs e)
        {
            customListBrandsList._ClickPage(true);//翻页

            //事件

            if (null != PreviousPage_BrandsList_Click)//有效
            {
                PreviousPage_BrandsList_Click(this, new CustomEventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击Brands List【Next Page】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonNextPage_BrandsList_CustomButton_Click(object sender, EventArgs e)
        {
            customListBrandsList._ClickPage(false);//翻页

            //事件

            if (null != NextPage_BrandsList_Click)//有效
            {
                NextPage_BrandsList_Click(this, new CustomEventArgs());
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：点击Backup List列表事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customListBackupList_CustomListItem_Click(object sender, EventArgs e)
        {
            if (bBackupType)//【LOCAL DISK】按下
            {
                bSelectionColumnIconDisplay_LocalDisk_BackupList[customListBackupList.CurrentDataIndex] = !(customListBackupList.ItemData[customListBackupList.CurrentDataIndex].ItemDataDisplay[2]);
            }
            else//【USB DEVICE】按下
            {
                bSelectionColumnIconDisplay_USBDevice_BackupList[customListBackupList.CurrentDataIndex] = !(customListBackupList.ItemData[customListBackupList.CurrentDataIndex].ItemDataDisplay[2]);
            }

            //

            _SetControl();//更新控件

            //事件

            if (null != BackUpListItem_Click)//有效
            {
                BackUpListItem_Click(this, new CustomEventArgs());
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：RESTORE BRANDS，【DELETE】确认，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_RestoreBrands_Delete_Confirm(object sender, EventArgs e)
        {
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.MessageDisplay_Window.TopMost = false;//取消置于顶层
            }
            else//其它
            {
                GlobalWindows.MessageDisplay_Window.Visible = false;//隐藏
            }

            //

            if (GlobalWindows.MessageDisplay_Window.MessageDisplayControl.OkCancel)//删除
            {
                //事件

                if (null != Delete_Click)//有效
                {
                    Delete_Click(this, new CustomEventArgs());
                }

                //显示等待窗口

                GlobalWindows.MessageDisplay_Window.WindowParameter = 46;//窗口特征数值
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.None;//不包含任何按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][11] + "...";
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][11] + "...";

                GlobalWindows.MessageDisplay_Window.StartPosition = FormStartPosition.CenterScreen;
                if (GlobalWindows.TopMostWindows)//置顶
                {
                    GlobalWindows.MessageDisplay_Window.TopMost = true;//将窗口置于顶层
                }
                else//其它
                {
                    GlobalWindows.MessageDisplay_Window.Visible = true;//显示
                }

                GlobalWindows.MessageDisplay_Window.Update();//

                //

                Int32 i = 0;//循环控制变量

                Boolean bValid = false;//删除是否成功。取值范围：true，是；false，否

                try
                {
                    if (bBackupType)//【LOCAL DISK】按下
                    {
                        for (i = 0; i < bSelectionColumnIconDisplay_LocalDisk_BackupList.Length; i++)//遍历
                        {
                            if (bSelectionColumnIconDisplay_LocalDisk_BackupList[i])//选择
                            {
                                Directory.Delete(BackupBrandData_LocalDisk[i].DataInfo.FullName, true);//删除
                            }
                        }

                        //

                        _GetBackupListData(sCurrentPath_LocalDisk, ref BackupBrandData_LocalDisk, ref bSelectionColumnIconDisplay_LocalDisk_BackupList);//获取LOCAL DISK备份文件信息

                        if (null != BackupBrandData_LocalDisk)//有效
                        {
                            _AddItemData(customListBackupList, BackupBrandData_LocalDisk.Length, BackupBrandData_LocalDisk, bSelectionColumnIconDisplay_LocalDisk_BackupList);
                        }
                        else//无效
                        {
                            _AddItemData(customListBackupList, 0, BackupBrandData_LocalDisk, bSelectionColumnIconDisplay_LocalDisk_BackupList);
                        }

                        _GstSelectAllButtonState(customListBackupList, ref bSelectAll_BackupList);//获取列表项选择状态
                    }
                    else//【USB DEVICE】按下
                    {
                        for (i = 0; i < bSelectionColumnIconDisplay_USBDevice_BackupList.Length; i++)//遍历
                        {
                            if (bSelectionColumnIconDisplay_USBDevice_BackupList[i])//选择
                            {
                                Directory.Delete(BackupBrandData_USBDevice[i].DataInfo.FullName, true);//删除
                            }
                        }

                        //

                        _GetBackupListData(sCurrentPath_USBDevice, ref BackupBrandData_USBDevice, ref bSelectionColumnIconDisplay_USBDevice_BackupList);//获取USB DEVICE备份文件信息

                        if (null != BackupBrandData_USBDevice)//有效
                        {
                            _AddItemData(customListBackupList, BackupBrandData_USBDevice.Length, BackupBrandData_USBDevice, bSelectionColumnIconDisplay_USBDevice_BackupList);
                        }
                        else//无效
                        {
                            _AddItemData(customListBackupList, 0, BackupBrandData_USBDevice, bSelectionColumnIconDisplay_USBDevice_BackupList);
                        }

                        _GstSelectAllButtonState(customListBackupList, ref bSelectAll_BackupList);//获取列表项选择状态
                    }

                    //

                    bValid = true;
                }
                catch (System.Exception ex)
                {
                    bValid = false;
                }

                //

                if (bValid)//成功
                {
                    //显示信息对话框

                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][3] + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][1];
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][3] + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][1];
                }
                else//失败
                {
                    //显示信息对话框

                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][3] + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][4];
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][3] + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][4];
                }
            }
            else//未删除
            {
                //不执行操作
            }

            //

            _SetControl();//更新按钮背景

            customListBrandsList.ListEnabled = true;//Brands List
            customListBackupList.ListEnabled = true;//Backup List
        }
        
        //----------------------------------------------------------------------
        // 功能说明：RESTORE BRANDS，【DELETE】等待，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_RestoreBrands_Delete_Wait(object sender, EventArgs e)
        {
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.MessageDisplay_Window.TopMost = false;//取消置于顶层
            }
            else//其它
            {
                GlobalWindows.MessageDisplay_Window.Visible = false;//隐藏
            }
        }
                
        //----------------------------------------------------------------------
        // 功能说明：RESTORE BRANDS，【RESTORE】确认，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_RestoreBrands_Restore_Confirm(object sender, EventArgs e)
        {
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.MessageDisplay_Window.TopMost = false;//取消置于顶层
            }
            else//其它
            {
                GlobalWindows.MessageDisplay_Window.Visible = false;//隐藏
            }

            //

            if (GlobalWindows.MessageDisplay_Window.MessageDisplayControl.OkCancel)//恢复
            {
                customButtonMessage.Visible = true;//显示提示信息

                //显示等待窗口

                bRestoreBrandsMessageWindowShow = true;

                GlobalWindows.MessageDisplay_Window.WindowParameter = 80;//窗口特征数值
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.None;//不包含任何按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][7] + "...";
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][7] + "...";

                GlobalWindows.MessageDisplay_Window.StartPosition = FormStartPosition.CenterScreen;
                if (GlobalWindows.TopMostWindows)//置顶
                {
                    GlobalWindows.MessageDisplay_Window.TopMost = true;//将窗口置于顶层
                }
                else//其它
                {
                    GlobalWindows.MessageDisplay_Window.Visible = true;//显示
                }

                GlobalWindows.MessageDisplay_Window.Update();//

                //

                Int32 i = 0;//循环控制变量

                Boolean bValid = false;//备份是否成功。取值范围：true，是；false，否

                Boolean bOverwriteCurrentBrand = false;//是否覆盖当前品牌。取值范围：true，是；false，否

                try
                {
                    if (bBackupType)//【LOCAL DISK】按下
                    {
                        for (i = 0; i < customListBackupList.ItemDataNumber; i++)//遍历Backup List
                        {
                            if (!(customListBackupList.ItemData[i].ItemDataDisplay[2]))//选择
                            {
                                if (!_CheckBrand(customListBackupList.ItemData[i]))//询问是否覆盖
                                {
                                    //显示信息窗口

                                    messageDisplayWindow_RestoreBrands = new MessageDisplayWindow();
                                    messageDisplayWindow_RestoreBrands.WindowParameter = 51;//窗口特征数值
                                    messageDisplayWindow_RestoreBrands.WindowClose_RestoreBrands_Restore_Overwrite_Confirm_LocalDisk += new System.EventHandler(messageDisplayWindow_WindowClose_RestoreBrands_Restore_Overwrite_Confirm_LocalDisk);//订阅事件
                                    messageDisplayWindow_RestoreBrands.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.OkCancel;//包含【确定】和【取消】按钮
                                    messageDisplayWindow_RestoreBrands.MessageDisplayControl.Language = language;//语言
                                    messageDisplayWindow_RestoreBrands.MessageDisplayControl.Chinese_Message_3 = customListBackupList.ItemData[i].ItemText[1] + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][5];
                                    messageDisplayWindow_RestoreBrands.MessageDisplayControl.English_Message_3 = customListBackupList.ItemData[i].ItemText[1] + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][5];
                                    messageDisplayWindow_RestoreBrands.MessageDisplayControl.Chinese_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][6] + "？";
                                    messageDisplayWindow_RestoreBrands.MessageDisplayControl.English_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][6] + "？";

                                    messageDisplayWindow_RestoreBrands.StartPosition = FormStartPosition.CenterScreen;

                                    if (GlobalWindows.TopMostWindows)//置顶
                                    {
                                        messageDisplayWindow_RestoreBrands.TopMost = true;//将窗口置于顶层
                                    }

                                    messageDisplayWindow_RestoreBrands.ShowDialog();

                                    if (messageDisplayWindow_RestoreBrands.MessageDisplayControl.OkCancel)//覆盖
                                    {
                                        VisionSystemClassLibrary.Class.Brand._CopyDirectory(sCurrentPath_LocalDisk + customListBackupList.ItemData[i].ItemText[1], sSystemBrandPath + customListBackupList.ItemData[i].ItemText[1]);

                                        //

                                        if (customListBackupList.ItemData[i].ItemText[1] == brand.CURRENTBrandName)//覆盖当前品牌
                                        {
                                            VisionSystemClassLibrary.Class.Brand._CopyDirectory(sCurrentPath_LocalDisk + customListBackupList.ItemData[i].ItemText[1], brand.ConfigDataPath.Substring(0, brand.ConfigDataPath.Length - 1));

                                            //

                                            bOverwriteCurrentBrand = true;
                                        }
                                    }
                                }
                                else//恢复
                                {
                                    VisionSystemClassLibrary.Class.Brand._CopyDirectory(sCurrentPath_LocalDisk + customListBackupList.ItemData[i].ItemText[1], sSystemBrandPath + customListBackupList.ItemData[i].ItemText[1]);

                                    //

                                    if (customListBackupList.ItemData[i].ItemText[1] == brand.CURRENTBrandName)//覆盖当前品牌
                                    {
                                        VisionSystemClassLibrary.Class.Brand._CopyDirectory(sCurrentPath_LocalDisk + customListBackupList.ItemData[i].ItemText[1], brand.ConfigDataPath.Substring(0, brand.ConfigDataPath.Length - 1));

                                        //

                                        bOverwriteCurrentBrand = true;
                                    }
                                }
                            }
                        }
                    }
                    else//【USB DEVICE】按下
                    {
                        for (i = 0; i < customListBackupList.ItemDataNumber; i++)//遍历Backup List
                        {
                            if (!(customListBackupList.ItemData[i].ItemDataDisplay[2]))//选择
                            {
                                if (!_CheckBrand(customListBackupList.ItemData[i]))//询问是否覆盖
                                {
                                    //显示信息窗口

                                    messageDisplayWindow_RestoreBrands = new MessageDisplayWindow();
                                    messageDisplayWindow_RestoreBrands.WindowParameter = 52;//窗口特征数值
                                    messageDisplayWindow_RestoreBrands.WindowClose_RestoreBrands_Restore_Overwrite_Confirm_USBDevice += new System.EventHandler(messageDisplayWindow_WindowClose_RestoreBrands_Restore_Overwrite_Confirm_USBDevice);//订阅事件
                                    messageDisplayWindow_RestoreBrands.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.OkCancel;//包含【确定】和【取消】按钮
                                    messageDisplayWindow_RestoreBrands.MessageDisplayControl.Language = language;//语言
                                    messageDisplayWindow_RestoreBrands.MessageDisplayControl.Chinese_Message_3 = customListBackupList.ItemData[i].ItemText[1] + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][5];
                                    messageDisplayWindow_RestoreBrands.MessageDisplayControl.English_Message_3 = customListBackupList.ItemData[i].ItemText[1] + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][5];
                                    messageDisplayWindow_RestoreBrands.MessageDisplayControl.Chinese_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][6] + "？";
                                    messageDisplayWindow_RestoreBrands.MessageDisplayControl.English_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][6] + "？";

                                    messageDisplayWindow_RestoreBrands.StartPosition = FormStartPosition.CenterScreen;

                                    if (GlobalWindows.TopMostWindows)//置顶
                                    {
                                        messageDisplayWindow_RestoreBrands.TopMost = true;//将窗口置于顶层
                                    }

                                    messageDisplayWindow_RestoreBrands.ShowDialog();

                                    if (messageDisplayWindow_RestoreBrands.MessageDisplayControl.OkCancel)//覆盖
                                    {
                                        VisionSystemClassLibrary.Class.Brand._CopyDirectory(sCurrentPath_USBDevice + customListBackupList.ItemData[i].ItemText[1], sSystemBrandPath + customListBackupList.ItemData[i].ItemText[1]);

                                        //

                                        if (customListBackupList.ItemData[i].ItemText[1] == brand.CURRENTBrandName)//覆盖当前品牌
                                        {
                                            VisionSystemClassLibrary.Class.Brand._CopyDirectory(sCurrentPath_USBDevice + customListBackupList.ItemData[i].ItemText[1], brand.ConfigDataPath.Substring(0, brand.ConfigDataPath.Length - 1));

                                            //

                                            bOverwriteCurrentBrand = true;
                                        }
                                    }
                                }
                                else//恢复
                                {
                                    VisionSystemClassLibrary.Class.Brand._CopyDirectory(sCurrentPath_USBDevice + customListBackupList.ItemData[i].ItemText[1], sSystemBrandPath + customListBackupList.ItemData[i].ItemText[1]);

                                    //

                                    if (customListBackupList.ItemData[i].ItemText[1] == brand.CURRENTBrandName)//覆盖当前品牌
                                    {
                                        VisionSystemClassLibrary.Class.Brand._CopyDirectory(sCurrentPath_USBDevice + customListBackupList.ItemData[i].ItemText[1], brand.ConfigDataPath.Substring(0, brand.ConfigDataPath.Length - 1));

                                        //

                                        bOverwriteCurrentBrand = true;
                                    }
                                }
                            }
                        }
                    }

                    //

                    _GetBrandsListData();//获取系统品牌信息

                    _AddItemData(customListBrandsList, brand.BrandNumber);

                    //

                    bValid = true;
                }
                catch (System.Exception ex)
                {
                    bValid = false;
                }

                //

                customButtonMessage.Visible = false;//隐藏提示信息

                //

                if (bValid)//成功
                {
                    if (bOverwriteCurrentBrand)//覆盖当前品牌
                    {
                        //事件

                        if (null != Restore_Click)//有效
                        {
                            Restore_Click(this, new CustomEventArgs());
                        }

                        //

                        GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][7] + "...";
                        GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][7] + "...";
                        GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = " ";
                        GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = " ";
                        GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][10] + "，" + iTimerRestoreBrandsCount.ToString();
                        GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][10] + "，" + iTimerRestoreBrandsCount.ToString();

                        timerRestoreBrands.Start();//启动定时器
                    }
                    else//未覆盖当前品牌
                    {
                        //显示信息对话框

                        bRestoreBrandsMessageWindowShow = false;

                        iRestoreBrandsNumber = 1;//此处赋值，在点击【确定】按钮时不执行重启操作

                        GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                        GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][12] + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][1];
                        GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][12] + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][1];
                    }
                }
                else//失败
                {
                    //显示信息对话框

                    bRestoreBrandsMessageWindowShow = false;

                    iRestoreBrandsNumber = 1;//此处赋值，在点击【确定】按钮时不执行重启操作

                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][12] + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][4];
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][12] + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][4];
                }
            }
            else//未恢复
            {
                //不执行操作
            }

            //

            _SetControl();//更新按钮背景

            customListBrandsList.ListEnabled = true;//Brands List
            customListBackupList.ListEnabled = true;//Backup List
        }
        
        //----------------------------------------------------------------------
        // 功能说明：RESTORE BRANDS，【RESTORE】等待，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_RestoreBrands_Restore_Wait(object sender, EventArgs e)
        {
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.MessageDisplay_Window.TopMost = false;//取消置于顶层
            }
            else//其它
            {
                GlobalWindows.MessageDisplay_Window.Visible = false;//隐藏
            }

            //

            if (0 >= iRestoreBrandsNumber)//成功
            {
                //事件

                if (null != RestoreBrandsSuccess)//有效
                {
                    RestoreBrandsSuccess(this, new CustomEventArgs());
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：RESTORE BRANDS，RESTORE（LOCAL DISK），覆盖已存在文件确认，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_RestoreBrands_Restore_Overwrite_Confirm_LocalDisk(object sender, EventArgs e)
        {
            messageDisplayWindow_RestoreBrands.Dispose();
        }

        //----------------------------------------------------------------------
        // 功能说明：RESTORE BRANDS，RESTORE（USB DEVICE），覆盖已存在文件确认，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_RestoreBrands_Restore_Overwrite_Confirm_USBDevice(object sender, EventArgs e)
        {
            messageDisplayWindow_RestoreBrands.Dispose();
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：定时器事件，恢复品牌完成后,执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void timerRestoreBrands_Tick(object sender, EventArgs e)
        {
            if (bRestoreBrandsMessageWindowShow)
            {
                iTimerRestoreBrandsCount--;

                if (0 >= iTimerRestoreBrandsCount)//超时
                {
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][12] + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][4];
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][12] + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][4];
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][9];
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][9];

                    timerRestoreBrands.Stop();//关闭定时器

                    iTimerRestoreBrandsCount = iTimerRestoreBrandsMaxCount;
                }
                else//计数
                {
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][10] + "，" + iTimerRestoreBrandsCount.ToString();
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][10] + "，" + iTimerRestoreBrandsCount.ToString();
                }
            }
        }
    }
}