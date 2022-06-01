/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：BackupBrandsControl.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述： BACKUP BRANDS控件

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

using System.Threading;

using System.Diagnostics;

using System.IO;

using System.Runtime.InteropServices;

using System.Reflection;

namespace VisionSystemControlLibrary
{
    public partial class BackupBrandsControl : UserControl
    {
        //该控件为Brand Management页面中的BACKUP BRANDS子页面

        private VisionSystemClassLibrary.Enum.InterfaceLanguage language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;//属性，语言

        private VisionSystemClassLibrary.Enum.DeviceState devicestate = VisionSystemClassLibrary.Enum.DeviceState.Stop;//属性，设备状态

        //

        private bool bBackupType = true;//【LOCAL DISK】和【USB DEVICE】按钮状态，取值范围：true，【LOCAL DISK】按下，【USB DEVICE】未按下；false，【USB DEVICE】按下，【LOCAL DISK】未按下

        private bool bSelectAll_BrandsList = false;//Brands List列表【SELECT ALL】按钮状态，取值范围：true，按下；false，未按下
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

        private String[][] sMessageText = new String[2][];//提示信息对话框上显示的文本（[语言][包含的文本]）

        //

        [Browsable(true), Description("点击【Close】按钮时产生的事件"), Category("BackupBrandsControl 事件")]
        public event EventHandler Close_Click;//点击【Close】按钮时产生的事件

        [Browsable(true), Description("点击【LOCAL DISK】按钮时产生的事件"), Category("BackupBrandsControl 事件")]
        public event EventHandler LocalDisk_Click;//点击【LOCAL DISK】按钮时产生的事件

        [Browsable(true), Description("点击【USB DEVICE】按钮时产生的事件"), Category("BackupBrandsControl 事件")]
        public event EventHandler USBDevice_Click;//点击【USB DEVICE】按钮时产生的事件

        [Browsable(true), Description("点击Brands List列表的【SELECT ALL】按钮时产生的事件"), Category("BackupBrandsControl 事件")]
        public event EventHandler SelectAll_BrandsList_Click;//点击Brands List列表的【SELECT ALL】按钮时产生的事件

        [Browsable(true), Description("点击Backup List列表的【SELECT ALL】按钮时产生的事件"), Category("BackupBrandsControl 事件")]
        public event EventHandler SelectAll_BackupList_Click;//点击Backup List列表的【SELECT ALL】按钮时产生的事件

        [Browsable(true), Description("点击【CREATE FOLDER】按钮时产生的事件"), Category("BackupBrandsControl 事件")]
        public event EventHandler CreateFolder_Click;//点击【CREATE FOLDER】按钮时产生的事件

        [Browsable(true), Description("点击【DELETE】按钮时产生的事件"), Category("BackupBrandsControl 事件")]
        public event EventHandler Delete_Click;//点击【DELETE】按钮时产生的事件

        [Browsable(true), Description("点击【BACK UP】按钮时产生的事件"), Category("BackupBrandsControl 事件")]
        public event EventHandler BackUp_Click;//点击【BACK UP】按钮时产生的事件

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

        [Browsable(true), Description("点击Brands List控件项时产生的事件"), Category("BackupBrandsControl 事件")]
        public event EventHandler BrandsListItem_Click;//点击Brands List控件项时产生的事件

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
        public BackupBrandsControl()
        {
            InitializeComponent();

            //由于该控件仅有一个实例，因此可以在此处进行事件的订阅

            if (null != GlobalWindows.StandardKeyboard_Window)
            {
                GlobalWindows.StandardKeyboard_Window.WindowClose_BackupBrands_CreateFolder += new System.EventHandler(standardKeyboardWindow_WindowClose_BackupBrands_CreateFolder);//订阅事件
            }

            if (null != GlobalWindows.MessageDisplay_Window)
            {
                GlobalWindows.MessageDisplay_Window.WindowClose_BackupBrands_CreateFolder_Success += new System.EventHandler(messageDisplayWindow_WindowClose_BackupBrands_CreateFolder_Success);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_BackupBrands_CreateFolder_Failure += new System.EventHandler(messageDisplayWindow_WindowClose_BackupBrands_CreateFolder_Failure);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_BackupBrands_Delete_Confirm += new System.EventHandler(messageDisplayWindow_WindowClose_BackupBrands_Delete_Confirm);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_BackupBrands_Delete_Wait += new System.EventHandler(messageDisplayWindow_WindowClose_BackupBrands_Delete_Wait);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_BackupBrands_Backup_Confirm += new System.EventHandler(messageDisplayWindow_WindowClose_BackupBrands_Backup_Confirm);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_BackupBrands_Backup_Wait += new System.EventHandler(messageDisplayWindow_WindowClose_BackupBrands_Backup_Wait);//订阅事件
            }

            //

            FieldInfo[] fieldinfo = VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese.GetType().GetFields();//获取公共字段
            if (null != fieldinfo && 1 < fieldinfo.Length)//有效
            {
                Int32 i = 0;//循环控制变量

                sMessageText = new String[fieldinfo.Length - 1][];
                for (i = 0; i < fieldinfo.Length - 1; i++)
                {
                    sMessageText[i] = new String[11];
                }

                //

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] = "输入文件夹名称";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] = "Enter Folder Name";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][1] = "创建";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][1] = "Creation of";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][2] = "成功";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][2] = "Completed";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][3] = "已经存在";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][3] = "Already Exists";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][4] = "确定删除选择的品牌";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][4] = "Delete Selected Brands";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][5] = "删除选择的品牌";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][5] = "Delection of Selected Brands";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][6] = "失败";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][6] = "Failed";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][7] = "确定备份选择的品牌";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][7] = "Backup Selected Brands";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][8] = "正在删除品牌";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][8] = "Deleting Brands";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][9] = "正在备份品牌";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][9] = "Backing Up Brands";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][10] = "备份选择的品牌";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][10] = "Backup of Selected Brands";
            }
        }

        //属性

        //-----------------------------------------------------------------------
        // 功能说明：Language属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("语言"), Category("BackupBrandsControl 通用")]
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
        [Browsable(true), Description("设备状态"), Category("BackupBrandsControl 通用")]
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
        [Browsable(true), Description("系统品牌路径"), Category("BackupBrandsControl 通用")]
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
        [Browsable(true), Description("LOCAL DISK存储的备份品牌默认路径"), Category("BackupBrandsControl 通用")]
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
        [Browsable(true), Description("USB DEVICE存储的备份品牌默认路径"), Category("BackupBrandsControl 通用")]
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
        [Browsable(true), Description("系统品牌"), Category("BackupBrandsControl 通用")]
        public VisionSystemClassLibrary.Class.Brand SystemBrand//属性
        {
            get//读取
            {
                return brand;
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

            //

            _SetBrand();//应用属性设置

            _SetLanguage();//设置语言
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
            //Brands List
            
            _InitList(customListBrandsList, brand.BrandNumber);//初始化列表

            _GstSelectAllButtonState(customListBrandsList, ref bSelectAll_BrandsList);//获取列表项选择状态

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
        // 功能说明：设置语言
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetLanguage()
        {
            customButtonLocalDisk.Language = language;//【LOCAL DISK】
            customButtonUSBDevice.Language = language;//设置【USB DEVICE】按钮背景
            customButtonSelectAll_BrandsList.Language = language;//Brands List，【SELECT ALL】
            customButtonSelectAll_BackupList.Language = language;//Backup List，【SELECT ALL】
            customButtonCreateFolder.Language = language;//【CREATE FOLDER】
            customButtonDelete.Language = language;//【DELETE】
            customButtonBackUp.Language = language;//【BACK UP】

            customButtonClose.Language = language;//【Close】
            customButtonPreviousPage_BrandsList.Language = language;//Brands List，【Previous Page】
            customButtonNextPage_BrandsList.Language = language;//Brands List，【Next Page】
            customButtonBack.Language = language;//【Back】
            customButtonOpen.Language = language;//【Open】
            customButtonPreviousPage_BackupList.Language = language;//Backup List，【Previous Page】
            customButtonNextPage_BackupList.Language = language;//Backup List，【Next Page】

            //

            customButtonCaption.Language = language;//标题文本
            customButtonMessage.Language = language;//提示信息文本
            customButtonBrandsList.Language = language;//Brands List列表名称文本
            customButtonBackupList.Language = language;//Backup List列表名称文本

            //

            customListBrandsList.Language = language;//Brands List列表
            customListBackupList.Language = language;//Backup List列表
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
                customButtonSelectAll_BrandsList.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【SELECT ALL】（Brands List）
                customButtonSelectAll_BackupList.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【SELECT ALL】（Backup List）
                customButtonCreateFolder.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【CREATE FOLDER】
                customButtonDelete.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【DELETE】
                customButtonBackUp.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【BACK UP】
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
        // 功能说明：设置页面控件的背景图像
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetControl()
        {
            if (0 < customListBrandsList.ItemDataNumber)//Brands List列表不为空
            {
                if (bSelectAll_BrandsList)//【SELECT ALL】按钮按下
                {
                    customButtonSelectAll_BrandsList.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
                }
                else//【SELECT ALL】按钮未按下
                {
                    customButtonSelectAll_BrandsList.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                }
            }
            else//Brands List列表为空
            {
                customButtonSelectAll_BrandsList.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
            }

            //

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
                        customButtonCreateFolder.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;

                        customButtonBackUp.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;

                        customButtonDelete.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;

                        customButtonSelectAll_BackupList.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                    } 
                    else//其它文件夹
                    {
                        customButtonCreateFolder.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;

                        //

                        if (0 < customListBrandsList.SelectedItemNumber)//选择了品牌
                        {
                            customButtonBackUp.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                        }
                        else//未选择品牌
                        {
                            customButtonBackUp.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                        }

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
                    customButtonCreateFolder.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;

                    customButtonBackUp.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;

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
                        customButtonCreateFolder.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;

                        customButtonBackUp.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;

                        customButtonDelete.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;

                        customButtonSelectAll_BackupList.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                    }
                    else//其它文件夹
                    {
                        customButtonCreateFolder.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;

                        //

                        if (0 < customListBrandsList.SelectedItemNumber)//选择了品牌
                        {
                            customButtonBackUp.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                        }
                        else//未选择品牌
                        {
                            customButtonBackUp.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                        }

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
                    customButtonCreateFolder.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;

                    customButtonBackUp.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;

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
                customListBrandsList.ItemData[i].ItemDataDisplay[1] = true;//图标（Selectd列，初始时不显示）

                //

                customListBrandsList.ItemData[i].ItemIconIndex[0] = -1;//图标
                customListBrandsList.ItemData[i].ItemIconIndex[1] = 0;//图标

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

                DirectoryInfo[] directoryinfoArray = directoryinfo.GetDirectories();//路径中的文件信息
                
                if (null != directoryinfoArray)//有效
                {
                    Int32 i = 0;//循环控制变量

                    BackupBrandData[] backupbranddata_temp = new BackupBrandData[directoryinfoArray.Length];//符合要求的文件数据

                    //

                    if (directoryinfo.Name.StartsWith(BackupBrandStartName))//备份品牌文件夹
                    {
                        for (i = 0; i < directoryinfoArray.Length; i++)//获取路径中的文件夹信息
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
        // 功能说明：检查文件夹是否重名
        // 输入参数：1.sNewFolderName：新创建的文件夹
        //         2.backupbranddata：品牌信息
        // 输出参数：无
        // 返回值：是否重名。取值范围：true，否；false，是
        //----------------------------------------------------------------------
        private Boolean _CheckFolderName(String sNewFolderName, BackupBrandData[] backupbranddata)
        {
            Int32 i = 0;//循环控制变量

            if (null != backupbranddata)//有效
            {
                for (i = 0; i < backupbranddata.Length; i++)
                {
                    if (sNewFolderName == backupbranddata[i].DataInfo.Name)//重名
                    {
                        break;
                    }
                }
                if (i < backupbranddata.Length)//重名
                {
                    return false;
                }
                else//不重名
                {
                    return true;
                }
            }
            else//无效
            {
                return true;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：获取备份文件夹名称
        // 输入参数：1.backupbranddata：品牌信息
        //         2.sCurrentPath：当前路径
        // 输出参数：无
        // 返回值：创建的备份文件夹名称
        //----------------------------------------------------------------------
        private String _GetBackupName(BackupBrandData[] backupbranddata, String sCurrentPath)
        {
            DateTime datetime = DateTime.Now;

            String sBackupFolderName = BackupBrandStartName + VisionSystemClassLibrary.Class.Brand._GetDateTime(datetime);

            if (!_CheckFolderName(sBackupFolderName, backupbranddata))//重名
            {
                Double i = 1.0;//循环控制变量

                sBackupFolderName = BackupBrandStartName + VisionSystemClassLibrary.Class.Brand._GetDateTime(datetime.AddSeconds(i));//更新

                //

                while (!_CheckFolderName(sBackupFolderName, backupbranddata))
                {
                    i++;

                    sBackupFolderName = BackupBrandStartName + VisionSystemClassLibrary.Class.Brand._GetDateTime(datetime.AddSeconds(i));//更新
                }
            }

            Directory.CreateDirectory(sCurrentPath + sBackupFolderName);//创建文件夹

            return sBackupFolderName;
        }

        //事件

        //----------------------------------------------------------------------
        // 功能说明：控件加载事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void BackupBrandsControl_Load(object sender, EventArgs e)
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
        // 功能说明：点击Brands List列表的【SELECT ALL】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonSelectAll_BrandsList_CustomButton_Click(object sender, EventArgs e)
        {
            bSelectAll_BrandsList = !bSelectAll_BrandsList;//更新数值

            //

            customListBrandsList._SelectAll(bSelectAll_BrandsList);//全部选择

            //

            _SetControl();//更新控件

            //事件

            if (null != SelectAll_BrandsList_Click)//有效
            {
                SelectAll_BrandsList_Click(this, new CustomEventArgs());
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
        // 功能说明：点击【CREATE FOLDER】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonCreateFolder_CustomButton_Click(object sender, EventArgs e)
        {
            //显示输入键盘

            GlobalWindows.StandardKeyboard_Window.WindowParameter = 2;//窗口特征数
            GlobalWindows.StandardKeyboard_Window.Language = language;//语言
            GlobalWindows.StandardKeyboard_Window.InvalidCharacter = new String[] { "\\", "/", ":", "*", "?", "\"", "<", ">", "|", BackupBrandStartName };//不能包含的字符
            GlobalWindows.StandardKeyboard_Window.Chinese_Caption = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0];//中文标题文本
            GlobalWindows.StandardKeyboard_Window.English_Caption = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0];//英文标题文本
            GlobalWindows.StandardKeyboard_Window.CapsLock = true;//Caps Lock打开
            GlobalWindows.StandardKeyboard_Window.Shift = false;//SHIFT按下
            GlobalWindows.StandardKeyboard_Window.MaxLength = 30;//数值长度范围
            GlobalWindows.StandardKeyboard_Window.StringValue = "";//初始显示的数值
            GlobalWindows.StandardKeyboard_Window.IsPassword = false;//密码输入窗口

            GlobalWindows.StandardKeyboard_Window.StartPosition = FormStartPosition.CenterScreen;
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.StandardKeyboard_Window.TopMost = true;//将窗口置于顶层
            }
            else//其它
            {
                GlobalWindows.StandardKeyboard_Window.Visible = true;//显示
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

            GlobalWindows.MessageDisplay_Window.WindowParameter = 3;//窗口特征数值
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.OkCancel;//包含【确定】和【取消】按钮
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][4] + "？";
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][4] + "？";

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
        // 功能说明：点击【BACK UP】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonBackUp_CustomButton_Click(object sender, EventArgs e)
        {
            //显示信息对话框

            GlobalWindows.MessageDisplay_Window.WindowParameter = 6;//窗口特征数值
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.OkCancel;//包含【确定】和【取消】按钮
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][7] + "？";
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][7] + "？";

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

        //

        //----------------------------------------------------------------------
        // 功能说明：点击Brands List列表事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customListBrandsList_CustomListItem_Click(object sender, EventArgs e)
        {
            _SetControl();//更新控件

            //事件

            if (null != BrandsListItem_Click)//有效
            {
                BrandsListItem_Click(this, new CustomEventArgs());
            }
        }

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
        // 功能说明：BACKUP BRANDS，CREATE FOLDER，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void standardKeyboardWindow_WindowClose_BackupBrands_CreateFolder(object sender, EventArgs e)
        {
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.StandardKeyboard_Window.TopMost = false;//取消置于顶层
            }
            else//其它
            {
                GlobalWindows.StandardKeyboard_Window.Visible = false;//隐藏
            }

            //

            if (GlobalWindows.StandardKeyboard_Window.EnterNewValue)//创建文件夹
            {
                //事件

                if (null != CreateFolder_Click)//有效
                {
                    CreateFolder_Click(this, new CustomEventArgs());
                }

                //

                Boolean bValid = false;//是否重名。取值范围：true，否；false，是

                if (bBackupType)//【LOCAL DISK】按下
                {
                    bValid = _CheckFolderName(GlobalWindows.StandardKeyboard_Window.StringValue, BackupBrandData_LocalDisk);//检查是否重名

                    //

                    if (bValid)//有效
                    {
                        Directory.CreateDirectory(sCurrentPath_LocalDisk + "\\" + GlobalWindows.StandardKeyboard_Window.StringValue);//创建文件夹

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
                }
                else//【USB DEVICE】按下
                {
                    bValid = _CheckFolderName(GlobalWindows.StandardKeyboard_Window.StringValue, BackupBrandData_USBDevice);//检查是否重名

                    //

                    if (bValid)//有效
                    {
                        Directory.CreateDirectory(sCurrentPath_USBDevice + "\\" + GlobalWindows.StandardKeyboard_Window.StringValue);//创建文件夹

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
                }

                //

                if (bValid)//成功
                {
                    //显示信息对话框

                    GlobalWindows.MessageDisplay_Window.WindowParameter = 1;//窗口特征数值
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][1] + " " + GlobalWindows.StandardKeyboard_Window.StringValue + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][2];
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][1] + " " + GlobalWindows.StandardKeyboard_Window.StringValue + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][2];

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
                else//重名
                {
                    //显示信息对话框

                    GlobalWindows.MessageDisplay_Window.WindowParameter = 2;//窗口特征数值
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = GlobalWindows.StandardKeyboard_Window.StringValue + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][3];
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = GlobalWindows.StandardKeyboard_Window.StringValue + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][3];

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

                _SetControl();//更新按钮背景

                customListBrandsList.ListEnabled = true;//Brands List
                customListBackupList.ListEnabled = true;//Backup List
            }
            else//未创建文件夹
            {
                _SetControl();//更新按钮背景

                customListBrandsList.ListEnabled = true;//Brands List
                customListBackupList.ListEnabled = true;//Backup List
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：BACKUP BRANDS，【CREATE FOLDER】成功，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_BackupBrands_CreateFolder_Success(object sender, EventArgs e)
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
        // 功能说明：BACKUP BRANDS，【CREATE FOLDER】失败，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_BackupBrands_CreateFolder_Failure(object sender, EventArgs e)
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
        // 功能说明：BACKUP BRANDS，【DELETE】确认，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_BackupBrands_Delete_Confirm(object sender, EventArgs e)
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

                //显示提示信息窗口

                GlobalWindows.MessageDisplay_Window.WindowParameter = 4;//窗口特征数值
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.None;//不包含任何按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][8] + "...";
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][8] + "...";

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
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][5] + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][2];
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][5] + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][2];
                }
                else//失败
                {
                    //显示信息对话框

                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][5] + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][6];
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][5] + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][6];
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
        // 功能说明：BACKUP BRANDS，【DELETE】等待，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_BackupBrands_Delete_Wait(object sender, EventArgs e)
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
        // 功能说明：BACKUP BRANDS，【BACK UP】确认，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_BackupBrands_Backup_Confirm(object sender, EventArgs e)
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

            if (GlobalWindows.MessageDisplay_Window.MessageDisplayControl.OkCancel)//备份
            {
                //事件

                if (null != BackUp_Click)//有效
                {
                    BackUp_Click(this, new CustomEventArgs());
                }

                //

                customButtonMessage.Visible = true;//显示提示信息

                //显示提示信息窗口

                GlobalWindows.MessageDisplay_Window.WindowParameter = 7;//窗口特征数值
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.None;//不包含任何按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][9] + "...";
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][9] + "...";

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

                try
                {
                    if (bBackupType)//【LOCAL DISK】按下
                    {
                        String sBackupFolderName = _GetBackupName(BackupBrandData_LocalDisk, sCurrentPath_LocalDisk);//创建备份路径

                        //

                        for (i = 0; i < customListBrandsList.ItemDataNumber; i++)//遍历Brands List
                        {
                            if (!(customListBrandsList.ItemData[i].ItemDataDisplay[1]))//选择
                            {
                                VisionSystemClassLibrary.Class.Brand._CopyDirectory(sSystemBrandPath + brand.SystemBrandData[i].Name, sCurrentPath_LocalDisk + sBackupFolderName + "\\" + brand.SystemBrandData[i].Name);
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
                        String sBackupFolderName = _GetBackupName(BackupBrandData_USBDevice, sCurrentPath_USBDevice);//创建备份路径

                        //

                        for (i = 0; i < customListBrandsList.ItemDataNumber; i++)//遍历Brands List
                        {
                            if (!(customListBrandsList.ItemData[i].ItemDataDisplay[1]))//选择
                            {
                                VisionSystemClassLibrary.Class.Brand._CopyDirectory(sSystemBrandPath + brand.SystemBrandData[i].Name, sCurrentPath_USBDevice + sBackupFolderName + "\\" + brand.SystemBrandData[i].Name);
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

                customButtonMessage.Visible = false;//隐藏提示信息

                //

                if (bValid)//成功
                {
                    //显示信息对话框

                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][10] + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][2];
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][10] + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][2];
                }
                else//失败
                {
                    //显示信息对话框

                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][10] + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][6];
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][10] + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][6];
                }
            }
            else//未备份
            {
                //不执行操作
            }

            //

            _SetControl();//更新按钮背景

            customListBrandsList.ListEnabled = true;//Brands List
            customListBackupList.ListEnabled = true;//Backup List
        }

        //----------------------------------------------------------------------
        // 功能说明：BACKUP BRANDS，【BACK UP】等待，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_BackupBrands_Backup_Wait(object sender, EventArgs e)
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
    }

    //

    //备份品牌数据
    public struct BackupBrandData
    {
        public DirectoryInfo DataInfo;//备份品牌文件信息

        public String Name;//备份品牌名称（若为备份品牌文件夹，则该值未品牌名称；否则为文件夹名称）

        public Int32 IconIndex;//备份品牌文件图标

        public Int32 Type;//备份品牌类型。取值范围：1，备份品牌文件夹中的子文件夹；2，备份品牌文件夹；3，其它文件夹
    }
}