/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：ShiftConfiguration.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：班次设置控件

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

using System.Runtime.InteropServices;

using System.Reflection;

namespace VisionSystemControlLibrary
{
    public partial class ShiftConfiguration : UserControl
    {
        //该控件为班次设置控件

        private VisionSystemClassLibrary.Enum.InterfaceLanguage language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;//属性，语言

        //

        private VisionSystemClassLibrary.Struct.ShiftTime[] shifttime = new VisionSystemClassLibrary.Struct.ShiftTime[3];//属性，班次数据

        private VisionSystemClassLibrary.Struct.ShiftTime[] shifttime_Original = new VisionSystemClassLibrary.Struct.ShiftTime[3];//属性，原始班次数据

        //

        private Int32 iMinShiftNumber = 1;//属性，班次数目最小值
        private Int32 iMaxShiftNumber = 24;//属性，班次数目最大值

        //

        private Boolean bApplySettings = true;//属性，显示窗口前调用，应用设置。取值范围：true，是；false，否

        //

        private Boolean bEnterNewValue = false;//属性（只读），是否输入了新的数值。取值范围：true，是；false，否

        //

        private String[][] sMessageText = new String[2][];//提示信息对话框、列表中显示的文本（[语言][包含的文本]）
        private String[][] sMessageText_1 = new String[2][];//控件上显示的文本（[语言][包含的文本]）

        //

        [Browsable(true), Description("窗口关闭时产生的事件"), Category("ShiftConfiguration 事件")]
        public event EventHandler Close_Click;//窗口关闭时产生的事件

        //

        //----------------------------------------------------------------------
        // 功能说明：系统默认调用，构造函数，初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public ShiftConfiguration()
        {
            InitializeComponent();

            //

            if (null != GlobalWindows.DigitalKeyboard_Window)
            {
                GlobalWindows.DigitalKeyboard_Window.WindowClose_ShiftConfiguration_ShiftNumber += new System.EventHandler(digitalKeyboardWindow_WindowClose_ShiftConfiguration_ShiftNumber);//订阅事件
            }

            if (null != GlobalWindows.DateTimePanel_Window)
            {
                GlobalWindows.DateTimePanel_Window.WindowClose_ShiftConfiguration += new System.EventHandler(dateTimePanelWindow_WindowClose_ShiftConfiguration);//订阅事件
            }

            //

            FieldInfo[] fieldinfo = VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese.GetType().GetFields();//获取公共字段
            if (null != fieldinfo && 1 < fieldinfo.Length)//有效
            {
                Int32 i = 0;//循环控制变量

                sMessageText = new String[fieldinfo.Length - 1][];
                for (i = 0; i < fieldinfo.Length - 1; i++)
                {
                    sMessageText[i] = new String[2];
                    sMessageText_1[i] = new String[1];
                }

                //

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] = "班次数";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] = "SHIFT NUMBER";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][1] = "班次";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][1] = "SHIFT";

                //

                sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] = customButtonCaption.Chinese_TextDisplay[0];
                sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] = customButtonCaption.English_TextDisplay[0];
            }
        }

        //属性

        //-----------------------------------------------------------------------
        // 功能说明：Language属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("语言"), Category("ShiftConfiguration 通用")]
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
        // 功能说明：MinShiftNumber属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("班次数目最小值"), Category("ShiftConfiguration 通用")]
        public Int32 MinShiftNumber
        {
            get//读取
            {
                return iMinShiftNumber;
            }
            set//设置
            {
                iMinShiftNumber = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：MaxShiftNumber属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("班次数目最大值"), Category("ShiftConfiguration 通用")]
        public Int32 MaxShiftNumber
        {
            get//读取
            {
                return iMaxShiftNumber;
            }
            set//设置
            {
                iMaxShiftNumber = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ShiftTimeConfiguration属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(false), Description("班次设置数据"), Category("ShiftConfiguration 通用")]
        public VisionSystemClassLibrary.Struct.ShiftTime[] ShiftTimeConfiguration
        {
            get//读取
            {
                return shifttime;
            }
            set//设置
            {
                if (null != value)//有效
                {
                    shifttime = new VisionSystemClassLibrary.Struct.ShiftTime[value.Length];

                    value.CopyTo(shifttime, 0);
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ShiftTimeConfiguration_Original属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(false), Description("班次设置原始数据"), Category("ShiftConfiguration 通用")]
        public VisionSystemClassLibrary.Struct.ShiftTime[] ShiftTimeConfiguration_Original
        {
            get//读取
            {
                return shifttime_Original;
            }
            set//设置
            {
                if (null != value)//有效
                {
                    shifttime_Original = new VisionSystemClassLibrary.Struct.ShiftTime[value.Length];

                    value.CopyTo(shifttime_Original, 0);
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ApplySettings属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(false), Description("显示窗口前调用，应用设置。取值范围：true，是；false，否"), Category("ShiftConfiguration 通用")]
        public Boolean ApplySettings
        {
            get//读取
            {
                return bApplySettings;
            }
            set//设置
            {
                bApplySettings = value;

                //

                _InitList();
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：EnterNewValue属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("是否输入了新的数值。取值范围：true，是；false，否"), Category("DateTimePanel 通用")]
        public Boolean EnterNewValue
        {
            get//读取
            {
                return bEnterNewValue;
            }
        }

        //函数

        //----------------------------------------------------------------------
        // 功能说明：设置语言
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetLanguage()
        {
            customButtonCaption.Language = language;//标题

            customButtonMessage.Language = language;//提示信息

            customButtonShiftNumber.Language = language;//【SHIFT NUMBER】

            customList.Language = language;//列表
        }

        //----------------------------------------------------------------------
        // 功能说明：初始化列表
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _InitList()
        {
            customList._ApplyListHeader();//应用列表头属性
            customList._ApplyListItem();//应用列表项属性

            //

            _SetList();
        }

        //----------------------------------------------------------------------
        // 功能说明：设置列表
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetList()
        {
            customList._Apply(shifttime.Length);//应用列表属性

            _AddItemData();//添加列表项数据

            _SetPage();//设置列表数据
        }

        //----------------------------------------------------------------------
        // 功能说明：添加列表项数据
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetItemData_1(Int32 iIndex)
        {
            customList.ItemData[iIndex].ItemText[1] = VisionSystemClassLibrary.Class.Shift._GetShiftTime(shifttime[iIndex].Start, shifttime[iIndex].End);
        }

        //----------------------------------------------------------------------
        // 功能说明：添加列表项数据
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _AddItemData()
        {
            Int32 i = 0;//循环控制变量

            for (i = 0; i < customList.ItemDataNumber; i++)//列表项数据
            {
                customList.ItemData[i].ItemText[0] = (i + 1).ToString();
                _SetItemData_1(i);

                customList.ItemData[i].ItemFlag = i;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：设置列表数据
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetPage()
        {
            customList._SetPage();//设置列表项数据

            if (1 < customList.TotalPage)//多于一页
            {
                customButtonPreviousPage.Visible = true;//【Previous Page】
                customButtonNextPage.Visible = true;//【Next Page】
            }
            else//小于等于一页
            {
                customButtonPreviousPage.Visible = false;//【Previous Page】
                customButtonNextPage.Visible = false;//【Next Page】
            }
        }

        //事件

        //----------------------------------------------------------------------
        // 功能说明：【SHIFT NUMBER】按钮点击事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonShiftNumber_CustomButton_Click(object sender, EventArgs e)
        {
            GlobalWindows.DigitalKeyboard_Window.WindowParameter = 12;//窗口特征数值
            GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.Language = language;//语言
            GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.Chinese_Caption = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0];//中文标题文本
            GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.English_Caption = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0];//英文标题文本
            GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.Precision = 0;//输入的数据类型
            GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.MaxLength = 2;//数值长度范围
            GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.MinValue = iMinShiftNumber;//最小值
            GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.MaxValue = iMaxShiftNumber;//最大值
            GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.StringValue = shifttime.Length.ToString();//初始显示的数值

            GlobalWindows.DigitalKeyboard_Window.StartPosition = FormStartPosition.CenterScreen;
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.DigitalKeyboard_Window.TopMost = true;//将窗口置于顶层
            }
            else//其它
            {
                GlobalWindows.DigitalKeyboard_Window.Visible = true;//显示
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：列表点击事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customList_CustomListItem_Click(object sender, EventArgs e)
        {
            if (0 <= customList.CurrentListIndex)//选择了有效项
            {
                GlobalWindows.DateTimePanel_Window.WindowParameter = 2;//窗口特征数值
                GlobalWindows.DateTimePanel_Window.DateTimePanelControl.Language = language;//语言
                GlobalWindows.DateTimePanel_Window.DateTimePanelControl.Chinese_Caption = sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] + "，" + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][1] + " " + customList.CurrentListIndex.ToString();//中文标题文本
                GlobalWindows.DateTimePanel_Window.DateTimePanelControl.English_Caption = sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] + "，" + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][1] + " " + customList.CurrentListIndex.ToString();//英文标题文本
                GlobalWindows.DateTimePanel_Window.DateTimePanelControl.PanelType = DateTimePanelType.Shift;//日期时间设置面板类型
                GlobalWindows.DateTimePanel_Window.DateTimePanelControl.DisplayTime_1 = shifttime[customList.CurrentListIndex].Start;//日期时间
                GlobalWindows.DateTimePanel_Window.DateTimePanelControl.DisplayTime_2 = shifttime[customList.CurrentListIndex].End;//日期时间
                if (shifttime.Length - 1 == customList.CurrentListIndex)//最后一项
                {
                    GlobalWindows.DateTimePanel_Window.DateTimePanelControl.ShiftTimeCheck = false;//不进行班次时间检查
                }
                else//其它
                {
                    GlobalWindows.DateTimePanel_Window.DateTimePanelControl.ShiftTimeCheck = true;//进行班次时间检查
                }

                GlobalWindows.DateTimePanel_Window.StartPosition = FormStartPosition.CenterScreen;
                if (GlobalWindows.TopMostWindows)//置顶
                {
                    GlobalWindows.DateTimePanel_Window.TopMost = true;//将窗口置于顶层
                }
                else//其它
                {
                    GlobalWindows.DateTimePanel_Window.Visible = true;//显示
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：【Previous Page】按钮点击事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonPreviousPage_CustomButton_Click(object sender, EventArgs e)
        {
            customList._ClickPage(true);//翻页，上一页
        }

        //----------------------------------------------------------------------
        // 功能说明：【Next Page】按钮点击事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonNextPage_CustomButton_Click(object sender, EventArgs e)
        {
            customList._ClickPage(false);//翻页，下一页
        }

        //----------------------------------------------------------------------
        // 功能说明：【OK】按钮点击事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonOk_CustomButton_Click(object sender, EventArgs e)
        {
            if (!VisionSystemClassLibrary.Class.Shift._Compare(shifttime, shifttime_Original))//修改
            {
                if (VisionSystemClassLibrary.Class.Shift._Check(shifttime))//合法
                {
                    bEnterNewValue = true;

                    customButtonMessage.Visible = false;

                    //事件

                    if (null != Close_Click)//有效
                    {
                        Close_Click(this, new CustomEventArgs());
                    }
                }
                else//非法
                {
                    customButtonMessage.Visible = true;
                }
            }
            else//未修改
            {
                bEnterNewValue = false;

                customButtonMessage.Visible = false;

                //事件

                if (null != Close_Click)//有效
                {
                    Close_Click(this, new CustomEventArgs());
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：【CANCEL】按钮点击事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonCancel_CustomButton_Click(object sender, EventArgs e)
        {
            bEnterNewValue = false;

            customButtonMessage.Visible = false;

            //事件

            if (null != Close_Click)//有效
            {
                Close_Click(this, new CustomEventArgs());
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：SHIFT CONFIGURATION，班次数设置，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void digitalKeyboardWindow_WindowClose_ShiftConfiguration_ShiftNumber(object sender, EventArgs e)
        {
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.DigitalKeyboard_Window.TopMost = false;//取消置于顶层
            }
            else//其它
            {
                GlobalWindows.DigitalKeyboard_Window.Visible = false;//隐藏
            }

            //

            if (GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.EnterNewValue)//输入完成
            {
                shifttime = VisionSystemClassLibrary.Class.Shift._CreateNewShift(Convert.ToInt32(GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.NumericalValue));

                //

                _SetList();
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：SHIFT CONFIGURATION，班次设置，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void dateTimePanelWindow_WindowClose_ShiftConfiguration(object sender, EventArgs e)
        {
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.DateTimePanel_Window.TopMost = false;//取消置于顶层
            }
            else//其它
            {
                GlobalWindows.DateTimePanel_Window.Visible = false;//隐藏
            }

            //

            if (GlobalWindows.DateTimePanel_Window.DateTimePanelControl.EnterNewValue)//输入完成
            {
                if (0 <= customList.CurrentListIndex)//有效
                {
                    shifttime[customList.CurrentListIndex].Start = GlobalWindows.DateTimePanel_Window.DateTimePanelControl.DisplayTime_1;
                    shifttime[customList.CurrentListIndex].End = GlobalWindows.DateTimePanel_Window.DateTimePanelControl.DisplayTime_2;

                    _SetItemData_1(customList.CurrentListIndex);
                    customList._Refresh(customList.Index_Page);
                }
            }
        }
    }
}