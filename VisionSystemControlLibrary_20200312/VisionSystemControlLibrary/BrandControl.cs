/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：BrandControl.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述： BRAND MANAGEMENT控件

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

using System.Reflection;

namespace VisionSystemControlLibrary
{
    public partial class BrandControl : UserControl
    {
        //该控件为Brand Management页面

        private VisionSystemClassLibrary.Enum.InterfaceLanguage language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;//属性，语言

        private VisionSystemClassLibrary.Enum.DeviceState devicestate = VisionSystemClassLibrary.Enum.DeviceState.Stop;//属性，设备状态

        //

        private VisionSystemClassLibrary.Class.Brand brand = new VisionSystemClassLibrary.Class.Brand();//属性（只读），品牌

        //

        private Boolean bLoadReloadBrand = false;//属性（只读），加载或重载品牌。取值范围：true，加载品牌；false，重载品牌

        //

        private Int32 iLoadReloadBrandNumber = 0;//属性，加载品牌的设备数量

        private Boolean bLoadReloadBrandMessageWindowShow = false;//属性（只读），是否显示加载品牌后的提示信息窗口。取值范围：true，是；false，否

        private const Int32 iTimerLoadReloadBrandMaxCount = 30;//定时器时间
        private Int32 iTimerLoadReloadBrandCount = 30;//定时器时间

        //

        private String[][] sMessageText = new String[2][];//提示信息对话框上显示的文本（[语言][包含的文本]）

        //

        [Browsable(true), Description("点击【返回】按钮时产生的事件"), Category("BrandControl 事件")]
        public event EventHandler Close_Click;//点击【返回】按钮时产生的事件
        //CustomEventArgs参数含义：
        //无意义

        [Browsable(true), Description("点击【SAVE CURRENT】按钮时产生的事件"), Category("BrandControl 事件")]
        public event EventHandler SaveCurrent_Click;//点击【SAVE CURRENT】按钮时产生的事件
        //CustomEventArgs参数含义：
        //无意义

        [Browsable(true), Description("点击【LOAD BRAND】（【RELOAD BRAND】）按钮时产生的事件"), Category("BrandControl 事件")]
        public event EventHandler LoadReloadBrand_Click;//点击【LOAD BRAND】（【RELOAD BRAND】）按钮时产生的事件
        //CustomEventArgs参数含义：
        //无意义

        [Browsable(true), Description("LOAD BRAND / RELOAD BRAND完成时产生的事件"), Category("BrandControl 事件")]
        public event EventHandler LoadReloadBrandSuccess;//LOAD BRAND / RELOAD BRAND完成时产生的事件
        //CustomEventArgs参数含义：
        //无意义

        [Browsable(true), Description("点击【COPY BRAND】按钮时产生的事件"), Category("BrandControl 事件")]
        public event EventHandler CopyBrand_Click;//点击【COPY BRAND】按钮时产生的事件
        //CustomEventArgs参数含义：
        //无意义

        [Browsable(true), Description("点击【RENAME BRAND】按钮时产生的事件"), Category("BrandControl 事件")]
        public event EventHandler RenameBrand_Click;//点击【RENAME BRAND】按钮时产生的事件
        //CustomEventArgs参数含义：
        //无意义

        [Browsable(true), Description("点击【DELETE BRAND】按钮时产生的事件"), Category("BrandControl 事件")]
        public event EventHandler DeleteBrand_Click;//点击【DELETE BRAND】按钮时产生的事件
        //CustomEventArgs参数含义：
        //无意义

        [Browsable(true), Description("点击【BACKUP BRANDS】按钮时产生的事件"), Category("BrandControl 事件")]
        public event EventHandler BackupBrands_Click;//点击【BACKUP BRANDS】按钮时产生的事件
        //CustomEventArgs参数含义：
        //无意义

        [Browsable(true), Description("点击【RESTORE BRANDS】按钮时产生的事件"), Category("BrandControl 事件")]
        public event EventHandler RestoreBrands_Click;//点击【RESTORE BRANDS】按钮时产生的事件
        //CustomEventArgs参数含义：
        //无意义

        [Browsable(true), Description("点击【Previous Page】按钮时产生的事件"), Category("BrandControl 事件")]
        public event EventHandler PreviousPage_Click;//点击【Previous Page】按钮时产生的事件
        //CustomEventArgs参数含义：
        //无意义

        [Browsable(true), Description("点击【Next Page】按钮时产生的事件"), Category("BrandControl 事件")]
        public event EventHandler NextPage_Click;//点击【Next Page】按钮时产生的事件
        //CustomEventArgs参数含义：
        //无意义

        [Browsable(true), Description("点击品牌列表项时产生的事件"), Category("BrandControl 事件")]
        public event EventHandler BrandItem_Click;//点击品牌列表项时产生的事件
        //CustomEventArgs参数含义：
        //无意义

        //

        //----------------------------------------------------------------------
        // 功能说明：系统默认调用，构造函数，初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public BrandControl()
        {
            InitializeComponent();

            //由于该控件仅有一个实例，因此可以在此处进行事件的订阅

            if (null != GlobalWindows.StandardKeyboard_Window)
            {
                GlobalWindows.StandardKeyboard_Window.WindowClose_Brand_CopyBrand += new System.EventHandler(standardKeyboardWindow_WindowClose_Brand_CopyBrand);//订阅事件
                GlobalWindows.StandardKeyboard_Window.WindowClose_Brand_RenameBrand += new System.EventHandler(standardKeyboardWindow_WindowClose_Brand_RenameBrand);//订阅事件
            }

            if (null != GlobalWindows.MessageDisplay_Window)
            {
                GlobalWindows.MessageDisplay_Window.WindowClose_Brand_SaveCurrent_Confirm += new System.EventHandler(messageDisplayWindow_WindowClose_Brand_SaveCurrent_Confirm);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_Brand_SaveCurrent_Success += new System.EventHandler(messageDisplayWindow_WindowClose_Brand_SaveCurrent_Success);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_Brand_SaveCurrent_Failure += new System.EventHandler(messageDisplayWindow_WindowClose_Brand_SaveCurrent_Failure);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_Brand_LoadReloadBrand_Confirm += new System.EventHandler(messageDisplayWindow_WindowClose_Brand_LoadReloadBrand_Confirm);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_Brand_LoadReloadBrand_Wait += new System.EventHandler(messageDisplayWindow_WindowClose_Brand_LoadReloadBrand_Wait);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_Brand_CopyBrand_Success += new System.EventHandler(messageDisplayWindow_WindowClose_Brand_CopyBrand_Success);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_Brand_CopyBrand_Failure_1 += new System.EventHandler(messageDisplayWindow_WindowClose_Brand_CopyBrand_Failure_1);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_Brand_CopyBrand_Failure_2 += new System.EventHandler(messageDisplayWindow_WindowClose_Brand_CopyBrand_Failure_2);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_Brand_CopyBrand_Failure_3 += new System.EventHandler(messageDisplayWindow_WindowClose_Brand_CopyBrand_Failure_3);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_Brand_RenameBrand_Success += new System.EventHandler(messageDisplayWindow_WindowClose_Brand_RenameBrand_Success);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_Brand_RenameBrand_Failure_1 += new System.EventHandler(messageDisplayWindow_WindowClose_Brand_RenameBrand_Failure_1);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_Brand_RenameBrand_Failure_2 += new System.EventHandler(messageDisplayWindow_WindowClose_Brand_RenameBrand_Failure_2);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_Brand_DeleteBrand_Confirm += new System.EventHandler(messageDisplayWindow_WindowClose_Brand_DeleteBrand_Confirm);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_Brand_DeleteBrand_Success += new System.EventHandler(messageDisplayWindow_WindowClose_Brand_DeleteBrand_Success);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_Brand_DeleteBrand_Failure += new System.EventHandler(messageDisplayWindow_WindowClose_Brand_DeleteBrand_Failure);//订阅事件
            }

            //

            FieldInfo[] fieldinfo = VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese.GetType().GetFields();//获取公共字段
            if (null != fieldinfo && 1 < fieldinfo.Length)//有效
            {
                Int32 i = 0;//循环控制变量

                sMessageText = new String[fieldinfo.Length - 1][];
                for (i = 0; i < fieldinfo.Length - 1; i++)
                {
                    sMessageText[i] = new String[23];
                }

                //

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] = "保存";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] = "Save of";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][1] = "完成";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][1] = "Completed";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][2] = "失败";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][2] = "Failed";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][3] = "加载";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][3] = "Load of";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][4] = "点击【确定】按钮重新启动程序";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][4] = "Press OK button to Restart Application";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][5] = "重新加载";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][5] = "Reload of";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][6] = "将品牌";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][6] = "Copy of";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][7] = "复制为品牌";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][7] = "to";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][8] = "将品牌";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][8] = "Rename of";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][9] = "重命名为品牌";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][9] = "to";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][10] = "删除";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][10] = "Deletion of";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][11] = "确定保存";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][11] = "Save";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][12] = "确定重新加载";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][12] = "Reload";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][13] = "确定加载";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][13] = "Load";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][14] = "复制";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][14] = "Copy";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][15] = "已经存在";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][15] = "already exists";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][16] = "最大品牌数目为";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][16] = "Maximum brand number is";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][17] = "重命名";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][17] = "Rename";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][18] = "确定删除";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][18] = "Delete";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][19] = "正在加载品牌";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][19] = "Loading Brand";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][20] = "正在重载品牌";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][20] = "Reloading Brand";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][21] = "请重试";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][21] = "Please try again";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][22] = "请等待";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][22] = "Please wait";
            }
        }

        //属性

        //-----------------------------------------------------------------------
        // 功能说明：Language属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("语言"), Category("BrandControl 通用")]
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
        [Browsable(true), Description("设备状态"), Category("BrandControl 通用")]
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
        // 功能说明：SystemBrand属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("品牌"), Category("BrandControl 通用")]
        public VisionSystemClassLibrary.Class.Brand SystemBrand//属性
        {
            get//读取
            {
                return brand;
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：LoadReloadBrand属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("加载或重载品牌。取值范围：true，加载品牌；false，重载品牌"), Category("BrandControl 通用")]
        public Boolean LoadReloadBrand//属性
        {
            get//读取
            {
                return bLoadReloadBrand;
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：LoadReloadBrandNumber属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("加载品牌的设备数量"), Category("BrandControl 通用")]
        public Int32 LoadReloadBrandNumber//属性
        {
            get//读取
            {
                return iLoadReloadBrandNumber;
            }
            set//设置
            {
                iLoadReloadBrandNumber = value;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：LoadReloadBrandMessageWindowShow属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("是否显示加载品牌后的提示信息窗口。取值范围：true，是；false，否"), Category("BrandControl 通用")]
        public Boolean LoadReloadBrandMessageWindowShow//属性
        {
            get//读取
            {
                return bLoadReloadBrandMessageWindowShow;
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：ItemDataNumber属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("品牌数量，即有效的列表项数目（0表示无有效的列表项）"), Category("BrandControl 通用")]
        public int ItemDataNumber//属性
        {
            get//读取
            {
                return customList.ItemDataNumber;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：TotalPage属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("包含的页码总数（取值为0，表示无有效的列表项）"), Category("BrandControl 通用")]
        public int TotalPage//属性
        {
            get//读取
            {
                return customList.TotalPage;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ItemControlNumber属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("列表项控件的数目"), Category("BrandControl 通用")]
        public int ItemControlNumber//属性
        {
            get//读取
            {
                return customList.ItemControlNumber;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：CurrentPage属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("当前页码（从0开始。取值为-1，表示无有效的列表项）"), Category("BrandControl 通用")]
        public int CurrentPage//属性
        {
            get//读取
            {
                return customList.CurrentPage;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：CURRENTBrandIndex属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("类型为CURRENT的品牌的索引值（0 ~ iItemDataNumber - 1。取值为-1，表示该类型的品牌不存在）"), Category("BrandControl 通用")]
        public int CURRENTBrandIndex//属性
        {
            get//读取
            {
                return brand.CURRENTBrandIndex;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：CurrentListIndex属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("当前选择的项在列表中的序号（0 ~ iItemDataNumber - 1。取值为-1，表示当前未选择任何项）"), Category("BrandControl 通用")]
        public int CurrentListIndex//属性
        {
            get//读取
            {
                return customList.CurrentListIndex;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：Index_Page属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("当前选择的项在当前页中的索引（0 ~ iItemControlNumber - 1。取值为-1，表示当前未选择任何项）"), Category("BrandControl 通用")]
        public int Index_Page//属性
        {
            get//读取
            {
                return customList.Index_Page;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：StartIndex属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("当前页中的起始索引（0 ~ iItemDataNumber - 1）"), Category("BrandControl 通用")]
        public int StartIndex//属性
        {
            get//读取
            {
                return customList.StartIndex;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：EndIndex属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("当前页中的结束索引（0 ~ iItemDataNumber - 1）"), Category("BrandControl 通用")]
        public int EndIndex//属性
        {
            get//读取
            {
                return customList.EndIndex;
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

            _SetLanguage();//设置语言

            _SetBrand();//应用属性设置

            _SetDeviceState();//设备状态
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：设置语言
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetLanguage()
        {
            customButtonSaveCurrent.Language = language;//【SAVE CURRENT】
            customButtonLoadBrand.Language = language;//【LOAD BRAND（RELOAD BRAND）】
            customButtonCopyBrand.Language = language;//【COPY BRAND】
            customButtonRenameBrand.Language = language;//【RENAME BRAND】
            customButtonDeleteBrand.Language = language;//【DELETE BRAND】
            customButtonBackupBrands.Language = language;//【BACKUP BRANDS】
            customButtonRestoreBrands.Language = language;//【RESTORE BRANDS】

            //

            customButtonClose.Language = language;//【Close】
            customButtonPreviousPage.Language = language;//【Previous Page】
            customButtonNextPage.Language = language;//【Next Page】

            //

            customButtonCaption.Language = language;//标题文本
            customButtonMessage1.Language = language;//控件1文本
            customButtonMessage2.Language = language;//控件2文本

            //

            customList.Language = language;//列表语言
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

                customButtonSaveCurrent.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【SAVE CURRENT】按钮的背景
                customButtonLoadBrand.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【LOAD BRAND】（【RELOAD BRAND】）按钮的背景
                customButtonCopyBrand.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【COPY BRAND】按钮的背景
                customButtonRenameBrand.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【RENAME BRAND】按钮的背景
                customButtonDeleteBrand.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【DELETE BRAND】按钮的背景
                customButtonBackupBrands.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【BACKUP BRANDS】按钮的背景
                customButtonRestoreBrands.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【RESTORE BRANDS】按钮的背景
                customButtonPreviousPage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【Previous Page】按钮的背景
                customButtonNextPage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【Next Page】按钮的背景

                //

                //if (customList.CurrentPage == customList._GetPage(customList.CurrentListIndex))//当前选择的项在当前页
                //{
                //    customList._SelectListItem(customList.Index_Page, false);
                //}

                customList.ListEnabled = false;
            }
            else//停止
            {
                //更新按钮状态

                customButtonBackupBrands.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//更新【BACKUP BRANDS】按钮的背景
                customButtonRestoreBrands.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//更新【RESTORE BRANDS】按钮的背景
                customButtonPreviousPage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//更新【Previous Page】按钮的背景
                customButtonNextPage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//更新【Next Page】按钮的背景

                _SetFunctionalButton();//设置按钮

                //

                //if (customList.CurrentPage == customList._GetPage(customList.CurrentListIndex))//当前选择的项在当前页
                //{
                //    customList._SelectListItem(customList.Index_Page, true);
                //}

                customList.ListEnabled = true;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：应用设置完成的属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _Apply()
        {
            customList._ApplyListHeader();//应用列表头属性
            customList._ApplyListItem();//应用列表项属性

            customList._Apply(brand.BrandNumber);//应用列表属性

            //

            _AddListItemData();//添加列表项数据

            _SetPage();//设置列表项数据
        }

        //----------------------------------------------------------------------
        // 功能说明：设置属性SystemBrand后调用，应用设置的属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetBrand()
        {
            _SetLanguage();//设置控件显示的文本

            _Apply();//应用设置的属性
        }

        //----------------------------------------------------------------------
        // 功能说明：设置【LOAD BRAND（RELOAD BRAND）】按钮文本
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetLoadBrandButtonText()
        {
            if (VisionSystemClassLibrary.Enum.BrandType.Master == _GetBrandType())//当前选择的项对应的品牌类型为MASTER
            {
                customButtonLoadBrand.CurrentTextGroupIndex = 0;//【LOAD BRAND】
            }
            else if (VisionSystemClassLibrary.Enum.BrandType.Current == _GetBrandType())//当前选择的项对应的品牌类型为CURRENT
            {
                customButtonLoadBrand.CurrentTextGroupIndex = 1;//【RELOAD BRAND】
            }
            else//VisionSystemClassLibrary.Enum.BrandType.General，当前选择的项对应的品牌类型为General
            {
                customButtonLoadBrand.CurrentTextGroupIndex = 0;//【LOAD BRAND】
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：保存CURRENT品牌
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SaveCurrent()
        {
            //显示信息对话框

            if (brand._SaveCurrent())//成功
            {
                GlobalWindows.MessageDisplay_Window.WindowParameter = 10;//窗口特征数值
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] + " " + brand.SystemBrandData[brand.CURRENTBrandIndex].Name + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][1];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] + " " + brand.SystemBrandData[brand.CURRENTBrandIndex].Name + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][1];
            }
            else//失败
            {
                GlobalWindows.MessageDisplay_Window.WindowParameter = 11;//窗口特征数值
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] + " " + brand.SystemBrandData[brand.CURRENTBrandIndex].Name + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][2];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] + " " + brand.SystemBrandData[brand.CURRENTBrandIndex].Name + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][2];
            }

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
        // 功能说明：加载（重载）品牌
        // 输入参数：1.bSuccess：操作是否成功。取值范围：true，是；false，否
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _LoadReloadBrand(Boolean bSuccess)
        {
            if (bSuccess)//成功
            {
                iLoadReloadBrandNumber--;
            }

            if (0 >= iLoadReloadBrandNumber)//加载（重载）完成
            {
                brand._LoadBrand(brand.SystemBrandData[customList.CurrentListIndex].Name);

                //

                brand.SystemBrandData[brand.CURRENTBrandIndex].Type = VisionSystemClassLibrary.Enum.BrandType.General;

                brand.SystemBrandData[customList.CurrentListIndex].Type = VisionSystemClassLibrary.Enum.BrandType.Current;

                //

                brand._Write(customList.CurrentListIndex, 4);//写入当前品牌

                //

                bLoadReloadBrandMessageWindowShow = false;

                iTimerLoadReloadBrandCount = iTimerLoadReloadBrandMaxCount;

                timerLoadReloadBrand.Stop();//关闭定时器

                //显示信息对话框

                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                if (bLoadReloadBrand)//加载
                {
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][3] + " " + brand.SystemBrandData[customList.CurrentListIndex].Name + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][1];
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][3] + " " + brand.SystemBrandData[customList.CurrentListIndex].Name + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][1];
                } 
                else//重载
                {
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][5] + " " + brand.SystemBrandData[customList.CurrentListIndex].Name + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][1];
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][5] + " " + brand.SystemBrandData[customList.CurrentListIndex].Name + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][1];
                }
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][4];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][4];
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：检查拷贝、重命名的品牌是否重名
        // 输入参数：1.sNewBrand：拷贝、重命名后的品牌名称
        // 输出参数：无
        // 返回值：是否重名。取值范围：true，否；false，是
        //----------------------------------------------------------------------
        private bool _CheckBrandName(string sNewBrand)
        {
            int i = 0;//循环控制变量
            bool bReturn = true;//返回值

            for (i = 0; i < customList.ItemDataNumber; i++)
            {
                if (sNewBrand == brand.SystemBrandData[i].Name)//存在重名
                {
                    break;
                }
            }
            if (i < customList.ItemDataNumber)//存在重名
            {
                bReturn = false;
            }
            else//不存在重名
            {
                bReturn = true;
            }

            if ((sNewBrand.IndexOfAny(System.IO.Path.GetInvalidPathChars()) >= 0) || (sNewBrand.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) >= 0)) //文件名或路径存在非法字符
            {
                bReturn = false;
            }

            return bReturn;
        }

        //----------------------------------------------------------------------
        // 功能说明：拷贝品牌
        // 输入参数：1.sNewBrand：新创建的品牌（即，拷贝的目标品牌）
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _CopyBrand(string sNewBrand)
        {
            string sSourceBrand = brand.SystemBrandData[customList.CurrentListIndex].Name;//拷贝的源品牌

            if (brand._CopyBrand(sSourceBrand, sNewBrand))//成功
            {
                brand.SystemBrandData[customList.ItemDataNumber].Name = sNewBrand;//品牌名称
                brand.SystemBrandData[customList.ItemDataNumber].Type = VisionSystemClassLibrary.Enum.BrandType.General;//品牌类型
                
                //

                brand.BrandNumber++;

                brand._Write(customList.ItemDataNumber, 1);//写入文件

                //

                customList._Apply(brand.BrandNumber, customList.ItemDataNumber, customList.ItemDataNumber);//应用列表属性

                _AddListItemData();//添加列表项数据

                _SetPage();//设置列表项数据

                //显示信息对话框

                GlobalWindows.MessageDisplay_Window.WindowParameter = 18;//窗口特征数值
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][6] + " " + sSourceBrand;
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][6] + " " + sSourceBrand;
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][7] + " " + sNewBrand;
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][7] + " " + sNewBrand;
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][1];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][1];

                GlobalWindows.MessageDisplay_Window.StartPosition = FormStartPosition.CenterScreen;
                if (GlobalWindows.TopMostWindows)//置顶
                {
                    GlobalWindows.MessageDisplay_Window.TopMost = true;//将窗口置于顶层
                }
                else//其它
                {
                    GlobalWindows.MessageDisplay_Window.Visible = true;//显示
                }

                //

                _SetFunctionalButton();//设置按钮
            }
            else//失败
            {
                //显示信息对话框

                GlobalWindows.MessageDisplay_Window.WindowParameter = 19;//窗口特征数值
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][6] + " " + sSourceBrand;
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][6] + " " + sSourceBrand;
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][7] + " " + sNewBrand;
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][7] + " " + sNewBrand;
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][2];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][2];

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
        }

        //----------------------------------------------------------------------
        // 功能说明：重命名品牌
        // 输入参数：1.sNewBrand：重命名后的品牌名称
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _RenameBrand(string sNewBrand)
        {
            string sSourceBrand = brand.SystemBrandData[customList.CurrentListIndex].Name;//重命名前的品牌名称

            if (brand._RenameBrand(sSourceBrand, sNewBrand))//成功
            {
                brand.SystemBrandData[customList.CurrentListIndex].Name = sNewBrand;//品牌名称

                //

                brand._Write(customList.CurrentListIndex, 2);//写入文件

                //

                customList.ItemData[customList.CurrentListIndex].ItemText[0] = brand.SystemBrandData[customList.CurrentListIndex].Name;
                customList._Refresh(customList.Index_Page);//设置并选中新的品牌项

                //显示信息对话框

                GlobalWindows.MessageDisplay_Window.WindowParameter = 22;//窗口特征数值
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][8] + " " + sSourceBrand;
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][8] + " " + sSourceBrand;
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][9] + " " + sNewBrand;
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][9] + " " + sNewBrand;
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][1];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][1];

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
            else//失败
            {
                //显示信息对话框

                GlobalWindows.MessageDisplay_Window.WindowParameter = 23;//窗口特征数值
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][8] + " " + sSourceBrand;
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][8] + " " + sSourceBrand;
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][9] + " " + sNewBrand;
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][9] + " " + sNewBrand;
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][2];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][2];

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
        }

        //----------------------------------------------------------------------
        // 功能说明：删除品牌
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _DeleteBrand()
        {
            string sBrandtoDelete = brand.SystemBrandData[customList.CurrentListIndex].Name;//待删除的品牌名称
            
            if (brand._DeleteBrand(sBrandtoDelete))//成功
            {
                Int32 i = 0;//循环控制变量

                if (customList.CurrentListIndex < brand.CURRENTBrandIndex)//当前品牌索引值之前
                {
                    brand.CURRENTBrandIndex--;
                }

                brand.BrandNumber--;

                for (i = customList.CurrentListIndex; i < brand.BrandNumber; i++)//循环控制变量
                {
                    brand.SystemBrandData[i].Name = brand.SystemBrandData[i + 1].Name;//品牌名称
                    brand.SystemBrandData[i].Type = brand.SystemBrandData[i + 1].Type;//品牌类型
                }
                brand.SystemBrandData[i].Name = "";//品牌名称
                brand.SystemBrandData[i].Type = VisionSystemClassLibrary.Enum.BrandType.None;//品牌类型

                brand._Write(brand.CURRENTBrandIndex, 3);//写入文件


                //

                Int32 iValue = 0;//临时变量

                if (brand.BrandNumber > 0)//存在有效项
                {
                    if (0 == customList.CurrentListIndex)//删除的品牌为第一项
                    {
                        iValue = 0;//当前选择的项在设备列表中的序号（0 ~ iItemDataNumber - 1。取值为-1，表示当前未选择任何项）
                    }
                    else//删除的品牌非第一项
                    {
                        iValue = customList.CurrentListIndex - 1;//当前选择的项在设备列表中的序号（0 ~ iItemDataNumber - 1。取值为-1，表示当前未选择任何项）
                    }
                }
                else//不存在有效项（实际操作中执行不到此处，因为MASTER和CURRENT品牌是始终存在的，无法删除）
                {
                    iValue = -1;//当前选择的项在设备列表中的序号（0 ~ iItemDataNumber - 1。取值为-1，表示当前未选择任何项）
                }

                customList._Apply(brand.BrandNumber, iValue, iValue);//应用列表属性

                _AddListItemData();//添加列表项数据

                _SetPage();//设置列表项数据

                //显示信息对话框

                GlobalWindows.MessageDisplay_Window.WindowParameter = 26;//窗口特征数值
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][10] + " " + sBrandtoDelete + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][1];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][10] + " " + sBrandtoDelete + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][1];

                GlobalWindows.MessageDisplay_Window.StartPosition = FormStartPosition.CenterScreen;
                if (GlobalWindows.TopMostWindows)//置顶
                {
                    GlobalWindows.MessageDisplay_Window.TopMost = true;//将窗口置于顶层
                }
                else//其它
                {
                    GlobalWindows.MessageDisplay_Window.Visible = true;//显示
                }

                //

                _SetFunctionalButton();//设置按钮
            } 
            else//失败
            {
                //显示信息对话框

                GlobalWindows.MessageDisplay_Window.WindowParameter = 27;//窗口特征数值
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][10] + " " + sBrandtoDelete + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][2];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][10] + " " + sBrandtoDelete + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][2];

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
        }

        //----------------------------------------------------------------------
        // 功能说明：备份品牌窗口关闭时调用
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _BackupBrands()
        {
            //不执行操作
        }

        //----------------------------------------------------------------------
        // 功能说明：恢复品牌窗口关闭时调用
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _RestoreBrands()
        {
            customList._Apply(brand.BrandNumber, customList.CurrentDataIndex, customList.CurrentListIndex);//应用列表属性

            _AddListItemData();//添加列表项数据

            _SetPage();//设置列表项数据
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：使用默认值设置并显示控件
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetDefault()
        {
            Int32 i = 0;//循环控制变量

            brand.BrandNumber = 1000;//品牌数量

            for (i = 0; i < brand.BrandNumber; i++)//赋初值
            {
                if (0 == i)//MASTER
                {
                    brand.SystemBrandData[i].Name = "X6S";//品牌名称
                    brand.SystemBrandData[i].Type = VisionSystemClassLibrary.Enum.BrandType.Master;//品牌类型，取值范围：BrandStyle
                }
                else if (1 == i)//CURRENT
                {
                    brand.SystemBrandData[i].Name = "X6S-" + i.ToString();//品牌名称
                    brand.SystemBrandData[i].Type = VisionSystemClassLibrary.Enum.BrandType.Current;//品牌类型，取值范围：BrandStyle
                }
                else//GENERAL
                {
                    brand.SystemBrandData[i].Name = "X6S-" + i.ToString();//品牌名称
                    brand.SystemBrandData[i].Type = VisionSystemClassLibrary.Enum.BrandType.General;//品牌类型，取值范围：BrandStyle
                }
            }

            brand.CURRENTBrandIndex = 1;//类型为CURRENT的品牌的索引值（0 ~ iItemDataNumber - 1。取值为-1，表示该类型的品牌不存在）
            brand.CURRENTBrandName = "X6S-1";//类型为CURRENT的品牌名称

            //


            customList._Apply(brand.BrandNumber);//应用列表属性

            _AddListItemData();//添加列表项数据

            _SetPage();//设置列表项数据
        }

        //----------------------------------------------------------------------
        // 功能说明：将设备信息添加至列表中
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _AddListItemData()
        {
            Int32 i = 0;//循环控制变量

            for (i = 0; i < customList.ItemDataNumber; i++)//列表项数据
            {
                if (VisionSystemClassLibrary.Enum.BrandType.Master == brand.SystemBrandData[i].Type)//MASTER类型的品牌
                {
                    customList.ItemData[i].ItemText[0] = brand.SystemBrandData[i].Name + "（M）";
                }
                else if (VisionSystemClassLibrary.Enum.BrandType.Current == brand.SystemBrandData[i].Type)//CURRENT类型的品牌
                {
                    customList.ItemData[i].ItemText[0] = brand.SystemBrandData[i].Name + "（C）";
                }
                else//其它
                {
                    customList.ItemData[i].ItemText[0] = brand.SystemBrandData[i].Name;
                }

                customList.ItemData[i].ItemFlag = i;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：设置当前页中的控件
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetPage()
        {
            customList._SetPage();//设置列表项

            //

            labelMessage.Text = brand.CURRENTBrandName;//设置显示的文本

            //

            _SetLoadBrandButtonText();//设置【LOAD BRAND（RELOAD BRAND）】按钮文本

            _SetFunctionalButton();//设置按钮

            //

            if (1 < customList.TotalPage)//大于1页
            {
                customButtonPreviousPage.Visible = true;//显示【Previous Page】按钮
                customButtonNextPage.Visible = true;//显示【Next Page】按钮
            }
            else//一页
            {
                customButtonPreviousPage.Visible = false;//隐藏【Previous Page】按钮
                customButtonNextPage.Visible = false;//隐藏【Next Page】按钮
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击列表中的列表项时进行相关操作
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _ClickListItem()
        {
            if (VisionSystemClassLibrary.Enum.DeviceState.Stop == devicestate)//系统停止
            {
                _SetLoadBrandButtonText();//设置【LOAD BRAND（RELOAD BRAND）】按钮文本

                _SetFunctionalButton();//设置按钮

                //事件

                if (null != BrandItem_Click)//有效
                {
                    BrandItem_Click(this, new CustomEventArgs());
                }
            }
            else//其它
            {
                //不执行操作
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：翻页，选择列表中的项，复制品牌，删除品牌时，设置【SAVE CURRENT】、【LOAD BRAND】（【RELOAD BRAND】）、【COPY BRAND】、【RENAME BRAND】、【DELETE BRAND】按钮的状态
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetFunctionalButton()
        {
            if (customList.ItemDataNumber > 0)//存在有效的列表项
            {
                if (brand.CURRENTBrandIndex >= 0)//存在CURRENT品牌
                {
                    customButtonSaveCurrent.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//更新【SAVE CURRENT】按钮的背景
                }
                else//不存在CURRENT品牌
                {
                    customButtonSaveCurrent.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【SAVE CURRENT】按钮的背景
                }

                //

                if (0 <= customList.CurrentListIndex)//列表中存在选择的项
                {
                    if (VisionSystemClassLibrary.Enum.BrandType.Master == brand.SystemBrandData[customList.CurrentListIndex].Type)//当前选择的项对应的品牌类型为MASTER
                    {
                        customButtonLoadBrand.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【LOAD BRAND】按钮的背景
                        customButtonRenameBrand.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【RENAME BRAND】按钮的背景
                        customButtonDeleteBrand.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【DELETE BRAND】按钮的背景
                    }
                    else if (VisionSystemClassLibrary.Enum.BrandType.Current == brand.SystemBrandData[customList.CurrentListIndex].Type)//当前选择的项对应的品牌类型为CURRENT
                    {
                        customButtonLoadBrand.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//更新【RELOAD BRAND】按钮的背景
                        customButtonRenameBrand.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【RENAME BRAND】按钮的背景
                        customButtonDeleteBrand.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【DELETE BRAND】按钮的背景
                    }
                    else//当前选择的项对应的品牌类型为General
                    {
                        customButtonLoadBrand.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//更新【LOAD BRAND】按钮的背景
                        customButtonRenameBrand.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//更新【RENAME BRAND】按钮的背景
                        customButtonDeleteBrand.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//更新【DELETE BRAND】按钮的背景
                    }

                    customButtonCopyBrand.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//更新【COPY BRAND】按钮的背景
                }
                else//列表中不存在任何选择的项
                {
                    customButtonLoadBrand.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【LOAD BRAND】按钮的背景
                    customButtonCopyBrand.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【COPY BRAND】按钮的背景
                    customButtonRenameBrand.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【RENAME BRAND】按钮的背景
                    customButtonDeleteBrand.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【DELETE BRAND】按钮的背景
                }
            }
            else//不存在有效的列表项
            {
                customButtonSaveCurrent.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【SAVE CURRENT】按钮的背景
                customButtonLoadBrand.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【LOAD BRAND】按钮的背景
                customButtonCopyBrand.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【COPY BRAND】按钮的背景
                customButtonRenameBrand.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【RENAME BRAND】按钮的背景
                customButtonDeleteBrand.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【DELETE BRAND】按钮的背景
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：获取当前选择的品牌类型
        // 输入参数：无
        // 输出参数：无
        // 返回值：品牌类型。取值范围：BrandType
        //----------------------------------------------------------------------
        private VisionSystemClassLibrary.Enum.BrandType _GetBrandType()
        {
            if (0 <= customList.CurrentListIndex)//列表中选择了某一项
            {
                if (VisionSystemClassLibrary.Enum.BrandType.Master == brand.SystemBrandData[customList.CurrentListIndex].Type)//当前选择的项对应的品牌类型为MASTER
                {
                    return VisionSystemClassLibrary.Enum.BrandType.Master;
                }
                else if (VisionSystemClassLibrary.Enum.BrandType.Current == brand.SystemBrandData[customList.CurrentListIndex].Type)//当前选择的项对应的品牌类型为CURRENT
                {
                    return VisionSystemClassLibrary.Enum.BrandType.Current;
                }
                else//BrandType.General，当前选择的项对应的品牌类型为General
                {
                    return VisionSystemClassLibrary.Enum.BrandType.General;
                }
            }
            else//列表中未选择任何项
            {
                return VisionSystemClassLibrary.Enum.BrandType.None;
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
        private void BrandControl_Load(object sender, EventArgs e)
        {
            //_SetDefault();//使用默认值设置并显示控件
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：点击【返回】按钮事件，执行相关操作
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
        // 功能说明：点击【SAVE CURRENT】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonSaveCurrent_CustomButton_Click(object sender, EventArgs e)
        {
            //显示信息对话框

            GlobalWindows.MessageDisplay_Window.WindowParameter = 9;//窗口特征数值
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.OkCancel;//包含【确定】和【取消】按钮
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][11] + " " + brand.SystemBrandData[brand.CURRENTBrandIndex].Name + "？";
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][11] + " " + brand.SystemBrandData[brand.CURRENTBrandIndex].Name + "？";

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
        // 功能说明：点击【LOAD BRAND】（RELOAD BRAND）按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonLoadBrand_CustomButton_Click(object sender, EventArgs e)
        {
            if (VisionSystemClassLibrary.Enum.BrandType.Master == _GetBrandType())//当前选择的项对应的品牌类型为MASTER
            {
                //不执行操作
            }
            else if (VisionSystemClassLibrary.Enum.BrandType.Current == _GetBrandType())//当前选择的项对应的品牌类型为CURRENT
            {
                //【RELOAD BRAND】

                bLoadReloadBrand = false;

                //显示信息对话框

                GlobalWindows.MessageDisplay_Window.WindowParameter = 12;//窗口特征数值
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.OkCancel;//包含【确定】和【取消】按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][12] + " " + brand.SystemBrandData[customList.CurrentListIndex].Name + "？";
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][12] + " " + brand.SystemBrandData[customList.CurrentListIndex].Name + "？";

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
            else//VisionSystemClassLibrary.Enum.BrandType.General，当前选择的项对应的品牌类型为General
            {
                //【LOAD BRAND】

                bLoadReloadBrand = true;

                //显示信息对话框

                GlobalWindows.MessageDisplay_Window.WindowParameter = 12;//窗口特征数值
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.OkCancel;//包含【确定】和【取消】按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][13] + " " + brand.SystemBrandData[customList.CurrentListIndex].Name + "？";
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][13] + " " + brand.SystemBrandData[customList.CurrentListIndex].Name + "？";

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
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【COPY BRAND】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonCopyBrand_CustomButton_Click(object sender, EventArgs e)
        {
            if (customList.ItemDataNumber < VisionSystemClassLibrary.Class.Brand.BrandNumberMax)//品牌数量未达到最大值
            {
                //事件

                if (null != CopyBrand_Click)//有效
                {
                    CopyBrand_Click(this, new CustomEventArgs());
                }

                //显示输入键盘

                GlobalWindows.StandardKeyboard_Window.WindowParameter = 3;//窗口特征数值
                GlobalWindows.StandardKeyboard_Window.Language = language;//语言
                GlobalWindows.StandardKeyboard_Window.InvalidCharacter = new String[] { "\\", "/", ":", "*", "?", "\"", "<", ">", "|" };//不能包含的字符
                GlobalWindows.StandardKeyboard_Window.Chinese_Caption = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][14] + " " + brand.SystemBrandData[customList.CurrentListIndex].Name;//中文标题文本
                GlobalWindows.StandardKeyboard_Window.English_Caption = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][14] + " " + brand.SystemBrandData[customList.CurrentListIndex].Name;//英文标题文本
                GlobalWindows.StandardKeyboard_Window.CapsLock = true;//Caps Lock打开
                GlobalWindows.StandardKeyboard_Window.MaxLength = 30;//数值长度范围
                GlobalWindows.StandardKeyboard_Window.StringValue = brand.SystemBrandData[customList.CurrentListIndex].Name;//初始显示的数值
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
            else//品牌数量达到最大值
            {
                //显示信息对话框

                GlobalWindows.MessageDisplay_Window.WindowParameter = 21;//窗口特征数值
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][16] + " " + brand.BrandNumber.ToString();
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][16] + " " + brand.BrandNumber.ToString();

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
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【RENAME BRAND】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonRenameBrand_CustomButton_Click(object sender, EventArgs e)
        {
            //当前选择的项对应的品牌类型为General时，可以操作

            //事件

            if (null != RenameBrand_Click)//有效
            {
                RenameBrand_Click(this, new CustomEventArgs());
            }

            //显示输入键盘

            GlobalWindows.StandardKeyboard_Window.WindowParameter = 4;//窗口特征数值
            GlobalWindows.StandardKeyboard_Window.Language = language;//语言
            GlobalWindows.StandardKeyboard_Window.InvalidCharacter = new String[] { "\\", "/", ":", "*", "?", "\"", "<", ">", "|" };//不能包含的字符
            GlobalWindows.StandardKeyboard_Window.Chinese_Caption = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][17] + " " + brand.SystemBrandData[customList.CurrentListIndex].Name;//中文标题文本
            GlobalWindows.StandardKeyboard_Window.English_Caption = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][17] + " " + brand.SystemBrandData[customList.CurrentListIndex].Name;//英文标题文本
            GlobalWindows.StandardKeyboard_Window.CapsLock = true;//Caps Lock打开
            GlobalWindows.StandardKeyboard_Window.MaxLength = 30;//数值长度范围
            GlobalWindows.StandardKeyboard_Window.StringValue = brand.SystemBrandData[customList.CurrentListIndex].Name;//初始显示的数值
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
        // 功能说明：点击【DELETE BRAND】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonDeleteBrand_CustomButton_Click(object sender, EventArgs e)
        {
            //当前选择的项对应的品牌类型为General时，可以操作

            //显示信息对话框

            GlobalWindows.MessageDisplay_Window.WindowParameter = 25;//窗口特征数值
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.OkCancel;//包含【确定】和【取消】按钮
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][18] + " " + brand.SystemBrandData[customList.CurrentListIndex].Name + "？";
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][18] + " " + brand.SystemBrandData[customList.CurrentListIndex].Name + "？";

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
        // 功能说明：点击【BACKUP BRANDS】按钮事件，执行相应的操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonBackupBrands_CustomButton_Click(object sender, EventArgs e)
        {
            //事件

            if (null != BackupBrands_Click)//有效
            {
                BackupBrands_Click(this, new CustomEventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【RESTORE BRANDS】按钮事件，执行相应的操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonRestoreBrands_CustomButton_Click(object sender, EventArgs e)
        {
            //事件

            if (null != RestoreBrands_Click)//有效
            {
                RestoreBrands_Click(this, new CustomEventArgs());
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：点击【Previous Page】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonPreviousPage_CustomButton_Click(object sender, EventArgs e)
        {
            customList._ClickPage(true);//翻页

            //事件

            if (null != PreviousPage_Click)//有效
            {
                PreviousPage_Click(this, new CustomEventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【Next Page】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonNextPage_CustomButton_Click(object sender, EventArgs e)
        {
            customList._ClickPage(false);//翻页

            //事件

            if (null != NextPage_Click)//有效
            {
                NextPage_Click(this, new CustomEventArgs());
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：点击品牌列表项事件，更新控件背景，执行相应的操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customList_CustomListItem_Click(object sender, EventArgs e)
        {
            _ClickListItem();//点击列表项
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：BRAND，COPY BRAND，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void standardKeyboardWindow_WindowClose_Brand_CopyBrand(object sender, EventArgs e)
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

            if (GlobalWindows.StandardKeyboard_Window.EnterNewValue)//拷贝
            {
                if (_CheckBrandName(GlobalWindows.StandardKeyboard_Window.StringValue))//检查是否重名
                {
                    _CopyBrand(GlobalWindows.StandardKeyboard_Window.StringValue);//拷贝品牌
                }
                else//重名
                {
                    //显示信息对话框

                    GlobalWindows.MessageDisplay_Window.WindowParameter = 20;//窗口特征数值
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = GlobalWindows.StandardKeyboard_Window.StringValue + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][15];
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = GlobalWindows.StandardKeyboard_Window.StringValue + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][15];

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
            }
            else//未拷贝或拷贝错误
            {
                //不执行操作
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：BRAND，RENAME BRAND，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void standardKeyboardWindow_WindowClose_Brand_RenameBrand(object sender, EventArgs e)
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

            if (GlobalWindows.StandardKeyboard_Window.EnterNewValue)//重命名
            {
                if (_CheckBrandName(GlobalWindows.StandardKeyboard_Window.StringValue))//检查是否重名
                {
                    _RenameBrand(GlobalWindows.StandardKeyboard_Window.StringValue);//重命名品牌
                }
                else//重名
                {
                    //显示信息对话框

                    GlobalWindows.MessageDisplay_Window.WindowParameter = 24;//窗口特征数值
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = GlobalWindows.StandardKeyboard_Window.StringValue + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][15];
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = GlobalWindows.StandardKeyboard_Window.StringValue + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][15];

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
            }
            else//未重命名或重命名错误
            {
                //不执行操作
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：BRAND，【SAVE CURRENT】确认，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_Brand_SaveCurrent_Confirm(object sender, EventArgs e)
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

            if (GlobalWindows.MessageDisplay_Window.MessageDisplayControl.OkCancel)//保存CURRENT品牌
            {
                //事件

                if (null != SaveCurrent_Click)//有效
                {
                    SaveCurrent_Click(this, new CustomEventArgs());
                }

                //

                _SaveCurrent();//保存CURRENT品牌
            }
            else//不进行保存
            {
                //不执行操作
            }
        }
        
        //----------------------------------------------------------------------
        // 功能说明：BRAND，【SAVE CURRENT】成功，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_Brand_SaveCurrent_Success(object sender, EventArgs e)
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
        // 功能说明：BRAND，【SAVE CURRENT】失败，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_Brand_SaveCurrent_Failure(object sender, EventArgs e)
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
        
        //

        //----------------------------------------------------------------------
        // 功能说明：BRAND，【LOAD BRAND】（【RELOAD BRAND】）确认，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_Brand_LoadReloadBrand_Confirm(object sender, EventArgs e)
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

            if (GlobalWindows.MessageDisplay_Window.MessageDisplayControl.OkCancel)//加载（重载）品牌
            {
                //事件

                if (null != LoadReloadBrand_Click)//有效
                {
                    LoadReloadBrand_Click(this, new CustomEventArgs());
                }

                //显示等待窗口

                bLoadReloadBrandMessageWindowShow = true;

                GlobalWindows.MessageDisplay_Window.WindowParameter = 13;//窗口特征数值
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.None;//不包含任何按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][19] + "...";
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][19] + "...";
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][22] + "，" + iTimerLoadReloadBrandCount.ToString();
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][22] + "，" + iTimerLoadReloadBrandCount.ToString();

                GlobalWindows.MessageDisplay_Window.StartPosition = FormStartPosition.CenterScreen;
                if (GlobalWindows.TopMostWindows)//置顶
                {
                    GlobalWindows.MessageDisplay_Window.TopMost = true;//将窗口置于顶层
                }
                else//其它
                {
                    GlobalWindows.MessageDisplay_Window.Visible = true;//显示
                }

                timerLoadReloadBrand.Start();//启动定时器
            }
            else//不加载（重载）
            {
                //不执行操作
            }
        }
        
        //----------------------------------------------------------------------
        // 功能说明：BRAND，【LOAD BRAND】（【RELOAD BRAND】）等待，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_Brand_LoadReloadBrand_Wait(object sender, EventArgs e)
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

            if (0 >= iLoadReloadBrandNumber)//成功
            {
                //事件

                if (null != LoadReloadBrandSuccess)//有效
                {
                    LoadReloadBrandSuccess(this, new EventArgs());
                }
            }
        }
        
        //----------------------------------------------------------------------
        // 功能说明：BRAND，【COPY BRAND】成功，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_Brand_CopyBrand_Success(object sender, EventArgs e)
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
        // 功能说明：BRAND，【COPY BRAND】失败（拷贝文件），窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_Brand_CopyBrand_Failure_1(object sender, EventArgs e)
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
        // 功能说明：BRAND，【COPY BRAND】失败（重名），窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_Brand_CopyBrand_Failure_2(object sender, EventArgs e)
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
        // 功能说明：BRAND，【COPY BRAND】失败（数量达到最大值），窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_Brand_CopyBrand_Failure_3(object sender, EventArgs e)
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
        // 功能说明：BRAND，【RENAME BRAND】成功，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_Brand_RenameBrand_Success(object sender, EventArgs e)
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
        // 功能说明：BRAND，【RENAME BRAND】失败（拷贝文件），窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_Brand_RenameBrand_Failure_1(object sender, EventArgs e)
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
        // 功能说明：BRAND，【RENAME BRAND】失败（重名），窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_Brand_RenameBrand_Failure_2(object sender, EventArgs e)
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
        // 功能说明：BRAND，【DELETE BRAND】确认，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_Brand_DeleteBrand_Confirm(object sender, EventArgs e)
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

            if (GlobalWindows.MessageDisplay_Window.MessageDisplayControl.OkCancel)//删除所选品牌
            {
                _DeleteBrand();//删除品牌

                //事件

                if (null != DeleteBrand_Click)//有效
                {
                    DeleteBrand_Click(this, new CustomEventArgs());
                }
            }
            else//取消删除品牌
            {
                //不执行操作
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：BRAND，【DELETE BRAND】成功，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_Brand_DeleteBrand_Success(object sender, EventArgs e)
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
        // 功能说明：BRAND，【DELETE BRAND】失败，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_Brand_DeleteBrand_Failure(object sender, EventArgs e)
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

        //

        //----------------------------------------------------------------------
        // 功能说明：定时器事件，加载品牌完成后,执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void timerLoadReloadBrand_Tick(object sender, EventArgs e)
        {
            if (bLoadReloadBrandMessageWindowShow)
            {
                iTimerLoadReloadBrandCount--;

                if (0 >= iTimerLoadReloadBrandCount)//超时
                {
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                    if (bLoadReloadBrand)//加载
                    {
                        GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][3] + " " + brand.SystemBrandData[customList.CurrentListIndex].Name + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][2];
                        GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][3] + " " + brand.SystemBrandData[customList.CurrentListIndex].Name + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][2];
                    }
                    else//重载
                    {
                        GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][5] + " " + brand.SystemBrandData[customList.CurrentListIndex].Name + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][2];
                        GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][5] + " " + brand.SystemBrandData[customList.CurrentListIndex].Name + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][2];
                    }
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][21];
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][21];

                    timerLoadReloadBrand.Stop();//关闭定时器

                    iTimerLoadReloadBrandCount = iTimerLoadReloadBrandMaxCount;
                }
                else//计数
                {
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][22] + "，" + iTimerLoadReloadBrandCount.ToString();
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][22] + "，" + iTimerLoadReloadBrandCount.ToString();
                }
            }
        }
    }
}