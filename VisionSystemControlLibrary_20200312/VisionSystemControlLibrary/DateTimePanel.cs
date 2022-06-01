/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：DateTimePanel.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：时间设置控件

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
    public partial class DateTimePanel : UserControl
    {
        //该控件为时间设置控件

        //1.确保每次显示控件时，日期时间数值正确有效

        private DateTimePanelType panelType = DateTimePanelType.StatisticsTimeSearch_1;//属性，日期时间设置面板类型

        private VisionSystemClassLibrary.Enum.InterfaceLanguage language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;//属性，语言

        //

        private VisionSystemClassLibrary.Struct.SYSTEMTIME SystemTime_1 = new VisionSystemClassLibrary.Struct.SYSTEMTIME();//属性，待显示的日期时间1
        private VisionSystemClassLibrary.Struct.SYSTEMTIME SystemTime_2 = new VisionSystemClassLibrary.Struct.SYSTEMTIME();//属性，待显示的日期时间2

        private VisionSystemClassLibrary.Struct.SYSTEMTIME SystemTime_1_Original = new VisionSystemClassLibrary.Struct.SYSTEMTIME();//属性，原始日期时间1
        private VisionSystemClassLibrary.Struct.SYSTEMTIME SystemTime_2_Original = new VisionSystemClassLibrary.Struct.SYSTEMTIME();//属性，原始日期时间2

        //

        private Boolean bShiftTimeCheck = true;//属性，是否进行班次时间检查。取值范围：true，是；false，否

        //

        private Boolean bEnterNewValue = false;//属性（只读），是否输入了新的数值。取值范围：true，是；false，否

        //

        private Boolean bDateTimeChanged = false;//属性（只读），日期时间是否被修改。取值范围：true，是；false，否

        //

        private Int32 iClickButtonIndex = 0;//点击的按钮索引值，显示两组时间时有效。取值范围：0，开始时间；1，结束时间

        //

        private String[][] sMessageText = new String[2][];//提示信息对话框、列表中显示的文本（[语言][包含的文本]）
        private String[][] sMessageText_1 = new String[2][];//控件上显示的文本（[语言][包含的文本]）

        //

        [Browsable(true), Description("窗口关闭时产生的事件"), Category("DateTimePanel 事件")]
        public event EventHandler Close_Click;//窗口关闭时产生的事件

        //

        //----------------------------------------------------------------------
        // 功能说明：系统默认调用，构造函数，初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public DateTimePanel()
        {
            InitializeComponent();

            //

            if (null != GlobalWindows.DigitalKeyboard_Window)
            {
                GlobalWindows.DigitalKeyboard_Window.WindowClose_DateTimePanel_Year += new System.EventHandler(digitalKeyboardWindow_WindowClose_DateTimePanel_Year);//订阅事件
                GlobalWindows.DigitalKeyboard_Window.WindowClose_DateTimePanel_Month += new System.EventHandler(digitalKeyboardWindow_WindowClose_DateTimePanel_Month);//订阅事件
                GlobalWindows.DigitalKeyboard_Window.WindowClose_DateTimePanel_Day += new System.EventHandler(digitalKeyboardWindow_WindowClose_DateTimePanel_Day);//订阅事件
                GlobalWindows.DigitalKeyboard_Window.WindowClose_DateTimePanel_Hour += new System.EventHandler(digitalKeyboardWindow_WindowClose_DateTimePanel_Hour);//订阅事件
                GlobalWindows.DigitalKeyboard_Window.WindowClose_DateTimePanel_Minute += new System.EventHandler(digitalKeyboardWindow_WindowClose_DateTimePanel_Minute);//订阅事件
                GlobalWindows.DigitalKeyboard_Window.WindowClose_DateTimePanel_Second += new System.EventHandler(digitalKeyboardWindow_WindowClose_DateTimePanel_Second);//订阅事件
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
                    sMessageText_1[i] = new String[7];
                }

                //

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] = "开始时间";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] = "Begin Time";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][1] = "结束时间";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][1] = "End Time";

                //

                sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] = customButtonCaption.Chinese_TextDisplay[0];
                sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] = customButtonCaption.English_TextDisplay[0];

                sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][1] = customButtonYear_1.Chinese_TextDisplay[0];
                sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][1] = customButtonYear_1.English_TextDisplay[0];

                sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][2] = customButtonMonth_1.Chinese_TextDisplay[0];
                sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][2] = customButtonMonth_1.English_TextDisplay[0];

                sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][3] = customButtonDay_1.Chinese_TextDisplay[0];
                sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][3] = customButtonDay_1.English_TextDisplay[0];

                sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][4] = customButtonHour_1.Chinese_TextDisplay[0];
                sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][4] = customButtonHour_1.English_TextDisplay[0];

                sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][5] = customButtonMinute_1.Chinese_TextDisplay[0];
                sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][5] = customButtonMinute_1.English_TextDisplay[0];

                sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][6] = customButtonSecond_1.Chinese_TextDisplay[0];
                sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][6] = customButtonSecond_1.English_TextDisplay[0];
            }

            //

            customButtonCaption.Chinese_TextDisplay = new String[1] { " " };//标题
            customButtonCaption.English_TextDisplay = new String[1] { " " };//标题

            customButtonYear_1.Chinese_TextDisplay = new String[1] { " " };//【年】，1
            customButtonYear_1.English_TextDisplay = new String[1] { " " };//【年】，1
            customButtonMonth_1.Chinese_TextDisplay = new String[1] { " " };//【月】，1
            customButtonMonth_1.English_TextDisplay = new String[1] { " " };//【月】，1
            customButtonDay_1.Chinese_TextDisplay = new String[1] { " " };//【日】，1
            customButtonDay_1.English_TextDisplay = new String[1] { " " };//【日】，1

            customButtonHour_1.Chinese_TextDisplay = new String[1] { " " };//【时】，1
            customButtonHour_1.English_TextDisplay = new String[1] { " " };//【时】，1
            customButtonMinute_1.Chinese_TextDisplay = new String[1] { " " };//【分】，1
            customButtonMinute_1.English_TextDisplay = new String[1] { " " };//【分】，1
            customButtonSecond_1.Chinese_TextDisplay = new String[1] { " " };//【秒】，1
            customButtonSecond_1.English_TextDisplay = new String[1] { " " };//【秒】，1

            //

            customButtonYear_2.Chinese_TextDisplay = new String[1] { " " };//【年】，2
            customButtonYear_2.English_TextDisplay = new String[1] { " " };//【年】，2
            customButtonMonth_2.Chinese_TextDisplay = new String[1] { " " };//【月】，2
            customButtonMonth_2.English_TextDisplay = new String[1] { " " };//【月】，2
            customButtonDay_2.Chinese_TextDisplay = new String[1] { " " };//【日】，2
            customButtonDay_2.English_TextDisplay = new String[1] { " " };//【日】，2

            customButtonHour_2.Chinese_TextDisplay = new String[1] { " " };//【时】，2
            customButtonHour_2.English_TextDisplay = new String[1] { " " };//【时】，2
            customButtonMinute_2.Chinese_TextDisplay = new String[1] { " " };//【分】，2
            customButtonMinute_2.English_TextDisplay = new String[1] { " " };//【分】，2
            customButtonSecond_2.Chinese_TextDisplay = new String[1] { " " };//【秒】，2
            customButtonSecond_2.English_TextDisplay = new String[1] { " " };//【秒】，2
        }

        //属性

        //-----------------------------------------------------------------------
        // 功能说明：Language属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("语言"), Category("DateTimePanel 通用")]
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
        // 功能说明：PanelType属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("日期时间设置面板类型"), Category("DateTimePanel 通用")]
        public DateTimePanelType PanelType
        {
            get//读取
            {
                return panelType;
            }
            set//设置
            {
                if (value != panelType)
                {
                    panelType = value;

                    //

                    _SetPanel();
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Chinese_Caption属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("中文标题名称"), Category("DateTimePanel 通用")]
        public String Chinese_Caption
        {
            get//读取
            {
                return customButtonCaption.Chinese_TextDisplay[0];
            }
            set//设置
            {
                if ("" != value && value != customButtonCaption.Chinese_TextDisplay[0])//有效
                {
                    customButtonCaption.Chinese_TextDisplay = new String[1] { value };//设置显示的文本

                    sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] = customButtonCaption.Chinese_TextDisplay[0];

                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：English_Caption属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("英文标题名称"), Category("DateTimePanel 通用")]
        public String English_Caption
        {
            get//读取
            {
                return customButtonCaption.English_TextDisplay[0];
            }
            set//设置
            {
                if ("" != value && value != customButtonCaption.English_TextDisplay[0])//有效
                {
                    customButtonCaption.English_TextDisplay = new String[1] { value };//设置显示的文本

                    sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] = customButtonCaption.English_TextDisplay[0];
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：DisplayTime_1属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(false), Description("待显示的日期时间1"), Category("DateTimePanel 通用")]
        public VisionSystemClassLibrary.Struct.SYSTEMTIME DisplayTime_1
        {
            get//读取
            {
                return SystemTime_1;
            }
            set//设置
            {
                SystemTime_1 = value;
                SystemTime_1_Original = value;

                //

                _SetDateTime();
            }
        }
        
        //-----------------------------------------------------------------------
        // 功能说明：DisplayTime_2属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(false), Description("待显示的日期时间2"), Category("DateTimePanel 通用")]
        public VisionSystemClassLibrary.Struct.SYSTEMTIME DisplayTime_2
        {
            get//读取
            {
                return SystemTime_2;
            }
            set//设置
            {
                SystemTime_2 = value;
                SystemTime_2_Original = value;

                //

                _SetDateTime();
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ShiftTimeCheck属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(false), Description("是否进行班次时间检查。取值范围：true，是；false，否"), Category("DateTimePanel 通用")]
        public Boolean ShiftTimeCheck
        {
            get//读取
            {
                return bShiftTimeCheck;
            }
            set//设置
            {
                bShiftTimeCheck = value;
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

        //-----------------------------------------------------------------------
        // 功能说明：DateTimeChanged属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(false), Description("日期时间是否被修改。取值范围：true，是；false，否"), Category("DateTimePanel 通用")]
        public Boolean DateTimeChanged
        {
            get//读取
            {
                return bDateTimeChanged;
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

            customButtonYear_1.Language = language;//【年】，1
            customButtonMonth_1.Language = language;//【月】，1
            customButtonDay_1.Language = language;//【日】，1
            customButtonHour_1.Language = language;//【时】，1
            customButtonMinute_1.Language = language;//【分】，1
            customButtonSecond_1.Language = language;//【秒】，1

            customButtonYear_2.Language = language;//【年】，2
            customButtonMonth_2.Language = language;//【月】，2
            customButtonDay_2.Language = language;//【日】，2
            customButtonHour_2.Language = language;//【时】，2
            customButtonMinute_2.Language = language;//【分】，2
            customButtonSecond_2.Language = language;//【秒】，2
        }

        //----------------------------------------------------------------------
        // 功能说明：设置面板
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetPanel()
        {
            Int32 iValue_1 = 0;//临时变量
            Int32 iValue_2 = 0;//临时变量
            Int32 iValue_3 = 0;//临时变量

            switch (panelType)
            {
                case DateTimePanelType.Shift://班次设置
                    //
                    labelHourMinuteText_1.Location = new Point(labelBeginEndText.Left + labelBeginEndText.Width / 2 - labelHourMinuteText_1.Width / 2, labelBeginEndText.Location.Y - labelHourMinuteText_1.Height);//【：】，1
                    customButtonHour_1.Location = new Point(labelHourMinuteText_1.Left - customButtonHour_1.Width, labelHourMinuteText_1.Location.Y);//【时】，1
                    customButtonMinute_1.Location = new Point(labelHourMinuteText_1.Right, labelHourMinuteText_1.Location.Y);//【分】，1
                    
                    //

                    labelHourMinuteText_2.Location = new Point(labelBeginEndText.Left + labelBeginEndText.Width / 2 - labelHourMinuteText_2.Width / 2, labelBeginEndText.Bottom);//【：】，2
                    customButtonHour_2.Location = new Point(labelHourMinuteText_2.Left - customButtonHour_2.Width, labelHourMinuteText_2.Location.Y);//【时】，2
                    customButtonMinute_2.Location = new Point(labelHourMinuteText_2.Right, labelHourMinuteText_2.Location.Y);//【分】，2
                    
                    //

                    customButtonHour_1.Visible = true;//【时】，1
                    labelHourMinuteText_1.Visible = true;//【：】，1
                    customButtonMinute_1.Visible = true;//【分】，1
                    customButtonYear_1.Visible = false;//【年】，1
                    customButtonMonth_1.Visible = false;//【月】，1
                    customButtonDay_1.Visible = false;//【日】，1
                    labelDateTimeText_1.Visible = false;//【，】，1
                    labelMinuteSecondText_1.Visible = false;//【：】，1
                    customButtonSecond_1.Visible = false;//【秒】，1
                    
                    labelBeginEndText.Visible = true;//【~】

                    customButtonHour_2.Visible = true;//【时】，2
                    labelHourMinuteText_2.Visible = true;//【：】，2
                    customButtonMinute_2.Visible = true;//【分】，2        
                    customButtonYear_2.Visible = false;//【年】，2
                    customButtonMonth_2.Visible = false;//【月】，2
                    customButtonDay_2.Visible = false;//【日】，2
                    labelDateTimeText_2.Visible = false;//【，】，2
                    labelMinuteSecondText_2.Visible = false;//【：】，2
                    customButtonSecond_2.Visible = false;//【秒】，2
                    //
            	    break;
                case DateTimePanelType.DateTime://日期时间设置
                    //
                    customButtonYear_1.Location = new Point(customButtonYear_2.Location.X, labelBeginEndText.Location.Y);//【年】，1
                    customButtonMonth_1.Location = new Point(customButtonMonth_2.Location.X, labelBeginEndText.Location.Y);//【月】，1
                    customButtonDay_1.Location = new Point(customButtonDay_2.Location.X, labelBeginEndText.Location.Y);//【日】，1
                    
                    labelDateTimeText_1.Location = new Point(labelDateTimeText_1.Location.X, labelBeginEndText.Location.Y);//【，】，1

                    customButtonHour_1.Location = new Point(labelDateTimeText_1.Right, labelDateTimeText_1.Location.Y);//【时】，1
                    labelHourMinuteText_1.Location = new Point(customButtonHour_1.Right, labelDateTimeText_1.Location.Y);//【：】，1
                    customButtonMinute_1.Location = new Point(labelHourMinuteText_1.Right, labelDateTimeText_1.Location.Y);//【分】，1         
                    labelMinuteSecondText_1.Location = new Point(labelMinuteSecondText_1.Location.X, labelDateTimeText_1.Location.Y);//【：】，1
                    customButtonSecond_1.Location = new Point(customButtonSecond_1.Location.X, labelDateTimeText_1.Location.Y);//【秒】，1
                    
                    //

                    customButtonYear_1.Visible = true;//【年】，1
                    customButtonMonth_1.Visible = true;//【月】，1
                    customButtonDay_1.Visible = true;//【日】，1
                    labelDateTimeText_1.Visible = true;//【，】，1
                    customButtonHour_1.Visible = true;//【时】，1
                    labelHourMinuteText_1.Visible = true;//【：】，1
                    customButtonMinute_1.Visible = true;//【分】，1
                    labelMinuteSecondText_1.Visible = true;//【：】，1
                    customButtonSecond_1.Visible = true;//【秒】，1
                    
                    labelBeginEndText.Visible = false;//【~】

                    customButtonYear_2.Visible = false;//【年】，2
                    customButtonMonth_2.Visible = false;//【月】，2
                    customButtonDay_2.Visible = false;//【日】，2
                    labelDateTimeText_2.Visible = false;//【，】，2
                    customButtonHour_2.Visible = false;//【时】，2
                    labelHourMinuteText_2.Visible = false;//【：】，2
                    customButtonMinute_2.Visible = false;//【分】，2        
                    labelMinuteSecondText_2.Visible = false;//【：】，2
                    customButtonSecond_2.Visible = false;//【秒】，2
                    //
                    break;
                case DateTimePanelType.StatisticsTimeSearch_1://统计时间查找（开始结束，日期时间）
                    //
                    customButtonYear_1.Location = new Point(customButtonYear_2.Location.X, labelBeginEndText.Location.Y - customButtonYear_1.Height);//【年】，1
                    customButtonMonth_1.Location = new Point(customButtonMonth_2.Location.X, labelBeginEndText.Location.Y - customButtonMonth_1.Height);//【月】，1
                    customButtonDay_1.Location = new Point(customButtonDay_2.Location.X, labelBeginEndText.Location.Y - customButtonDay_1.Height);//【日】，1
                    
                    labelDateTimeText_1.Location = new Point(labelDateTimeText_1.Location.X, labelBeginEndText.Location.Y - labelDateTimeText_1.Height);//【，】，1

                    customButtonHour_1.Location = new Point(labelDateTimeText_1.Right, labelDateTimeText_1.Location.Y);//【时】，1
                    labelHourMinuteText_1.Location = new Point(customButtonHour_1.Right, labelDateTimeText_1.Location.Y);//【：】，1
                    customButtonMinute_1.Location = new Point(labelHourMinuteText_1.Right, labelDateTimeText_1.Location.Y);//【分】，1         
                    labelMinuteSecondText_1.Location = new Point(labelMinuteSecondText_1.Location.X, labelDateTimeText_1.Location.Y);//【：】，1
                    customButtonSecond_1.Location = new Point(customButtonSecond_1.Location.X, labelDateTimeText_1.Location.Y);//【秒】，1
                    
                    //

                    customButtonYear_2.Location = new Point(customButtonYear_2.Location.X, labelBeginEndText.Bottom);//【年】，2
                    customButtonMonth_2.Location = new Point(customButtonMonth_2.Location.X, labelBeginEndText.Bottom);//【月】，2
                    customButtonDay_2.Location = new Point(customButtonDay_2.Location.X, labelBeginEndText.Bottom);//【日】，2
                    
                    labelDateTimeText_2.Location = new Point(labelDateTimeText_2.Location.X, labelBeginEndText.Bottom);//【，】，2

                    customButtonHour_2.Location = new Point(labelDateTimeText_2.Right, labelDateTimeText_2.Location.Y);//【时】，2
                    labelHourMinuteText_2.Location = new Point(customButtonHour_2.Right, labelDateTimeText_2.Location.Y);//【：】，2
                    customButtonMinute_2.Location = new Point(labelHourMinuteText_2.Right, labelDateTimeText_2.Location.Y);//【分】，2         
                    labelMinuteSecondText_2.Location = new Point(labelMinuteSecondText_2.Location.X, labelDateTimeText_2.Location.Y);//【：】，2
                    customButtonSecond_2.Location = new Point(customButtonSecond_2.Location.X, labelDateTimeText_2.Location.Y);//【秒】，2
                    
                    //

                    customButtonYear_1.Visible = true;//【年】，1
                    customButtonMonth_1.Visible = true;//【月】，1
                    customButtonDay_1.Visible = true;//【日】，1
                    labelDateTimeText_1.Visible = true;//【，】，1
                    customButtonHour_1.Visible = true;//【时】，1
                    labelHourMinuteText_1.Visible = true;//【：】，1
                    customButtonMinute_1.Visible = true;//【分】，1
                    labelMinuteSecondText_1.Visible = true;//【：】，1
                    customButtonSecond_1.Visible = true;//【秒】，1
                    
                    labelBeginEndText.Visible = true;//【~】

                    customButtonYear_2.Visible = true;//【年】，2
                    customButtonMonth_2.Visible = true;//【月】，2
                    customButtonDay_2.Visible = true;//【日】，2
                    labelDateTimeText_2.Visible = true;//【，】，2
                    customButtonHour_2.Visible = true;//【时】，2
                    labelHourMinuteText_2.Visible = true;//【：】，2
                    customButtonMinute_2.Visible = true;//【分】，2        
                    labelMinuteSecondText_2.Visible = true;//【：】，2
                    customButtonSecond_2.Visible = true;//【秒】，2
                    //
                    break;
                case DateTimePanelType.StatisticsTimeSearch_2://统计时间查找（日期）
                    //

                    iValue_1 = customButtonMonth_1.Location.X - (customButtonYear_1.Location.X + customButtonYear_1.Width);//临时变量
                    iValue_2 = customButtonDay_1.Location.X - (customButtonMonth_1.Location.X + customButtonMonth_1.Width);//临时变量
                    iValue_3 = (Int32)((this.Width - ((customButtonDay_1.Location.X + customButtonDay_1.Width) - customButtonYear_1.Location.X)) / 2);

                    customButtonYear_1.Location = new Point(iValue_3, labelBeginEndText.Location.Y);//【年】，1
                    customButtonMonth_1.Location = new Point(customButtonYear_1.Location.X + customButtonYear_1.Width + iValue_1, labelBeginEndText.Location.Y);//【月】，1
                    customButtonDay_1.Location = new Point(customButtonMonth_1.Location.X + customButtonMonth_1.Width + iValue_2, labelBeginEndText.Location.Y);//【日】，1

                    //

                    customButtonYear_1.Visible = true;//【年】，1
                    customButtonMonth_1.Visible = true;//【月】，1
                    customButtonDay_1.Visible = true;//【日】，1

                    labelDateTimeText_1.Visible = false;//【，】，1
                    customButtonHour_1.Visible = false;//【时】，1
                    labelHourMinuteText_1.Visible = false;//【：】，1
                    customButtonMinute_1.Visible = false;//【分】，1
                    labelMinuteSecondText_1.Visible = false;//【：】，1
                    customButtonSecond_1.Visible = false;//【秒】，1

                    labelBeginEndText.Visible = false;//【~】

                    customButtonYear_2.Visible = false;//【年】，2
                    customButtonMonth_2.Visible = false;//【月】，2
                    customButtonDay_2.Visible = false;//【日】，2
                    labelDateTimeText_2.Visible = false;//【，】，2
                    customButtonHour_2.Visible = false;//【时】，2
                    labelHourMinuteText_2.Visible = false;//【：】，2
                    customButtonMinute_2.Visible = false;//【分】，2        
                    labelMinuteSecondText_2.Visible = false;//【：】，2
                    customButtonSecond_2.Visible = false;//【秒】，2
                    //
                    break;
                default:
                    break;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：设置日期时间
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetDateTime()
        {
            switch (panelType)
            {
                case DateTimePanelType.Shift://班次设置
                    //
                    customButtonHour_1.Chinese_TextDisplay = new String[1] { SystemTime_1.Hour.ToString("D2") };//【时】，1
                    customButtonHour_1.English_TextDisplay = new String[1] { SystemTime_1.Hour.ToString("D2") };//【时】，1
                    customButtonMinute_1.Chinese_TextDisplay = new String[1] { SystemTime_1.Minute.ToString("D2") };//【分】，1
                    customButtonMinute_1.English_TextDisplay = new String[1] { SystemTime_1.Minute.ToString("D2") };//【分】，1

                    //

                    customButtonHour_2.Chinese_TextDisplay = new String[1] { SystemTime_2.Hour.ToString("D2") };//【时】，2
                    customButtonHour_2.English_TextDisplay = new String[1] { SystemTime_2.Hour.ToString("D2") };//【时】，2
                    customButtonMinute_2.Chinese_TextDisplay = new String[1] { SystemTime_2.Minute.ToString("D2") };//【分】，2
                    customButtonMinute_2.English_TextDisplay = new String[1] { SystemTime_2.Minute.ToString("D2") };//【分】，2
                    //
                    break;
                case DateTimePanelType.DateTime://日期时间设置
                    //
                    customButtonYear_1.Chinese_TextDisplay = new String[1] { SystemTime_1.Year.ToString("D4") };//【年】，1
                    customButtonYear_1.English_TextDisplay = new String[1] { SystemTime_1.Year.ToString("D4") };//【年】，1
                    customButtonMonth_1.Chinese_TextDisplay = new String[1] { SystemTime_1.Month.ToString("D2") };//【月】，1
                    customButtonMonth_1.English_TextDisplay = new String[1] { SystemTime_1.Month.ToString("D2") };//【月】，1
                    customButtonDay_1.Chinese_TextDisplay = new String[1] { SystemTime_1.Day.ToString("D2") };//【日】，1
                    customButtonDay_1.English_TextDisplay = new String[1] { SystemTime_1.Day.ToString("D2") };//【日】，1

                    customButtonHour_1.Chinese_TextDisplay = new String[1] { SystemTime_1.Hour.ToString("D2") };//【时】，1
                    customButtonHour_1.English_TextDisplay = new String[1] { SystemTime_1.Hour.ToString("D2") };//【时】，1
                    customButtonMinute_1.Chinese_TextDisplay = new String[1] { SystemTime_1.Minute.ToString("D2") };//【分】，1         
                    customButtonMinute_1.English_TextDisplay = new String[1] { SystemTime_1.Minute.ToString("D2") };//【分】，1         
                    customButtonSecond_1.Chinese_TextDisplay = new String[1] { SystemTime_1.Second.ToString("D2") };//【秒】，1
                    customButtonSecond_1.English_TextDisplay = new String[1] { SystemTime_1.Second.ToString("D2") };//【秒】，1
                    //
                    break;
                case DateTimePanelType.StatisticsTimeSearch_1://统计时间查找（开始结束，日期时间）
                    //
                    customButtonYear_1.Chinese_TextDisplay = new String[1] { SystemTime_1.Year.ToString("D4") };//【年】，1
                    customButtonYear_1.English_TextDisplay = new String[1] { SystemTime_1.Year.ToString("D4") };//【年】，1
                    customButtonMonth_1.Chinese_TextDisplay = new String[1] { SystemTime_1.Month.ToString("D2") };//【月】，1
                    customButtonMonth_1.English_TextDisplay = new String[1] { SystemTime_1.Month.ToString("D2") };//【月】，1
                    customButtonDay_1.Chinese_TextDisplay = new String[1] { SystemTime_1.Day.ToString("D2") };//【日】，1
                    customButtonDay_1.English_TextDisplay = new String[1] { SystemTime_1.Day.ToString("D2") };//【日】，1

                    customButtonHour_1.Chinese_TextDisplay = new String[1] { SystemTime_1.Hour.ToString("D2") };//【时】，1
                    customButtonHour_1.English_TextDisplay = new String[1] { SystemTime_1.Hour.ToString("D2") };//【时】，1
                    customButtonMinute_1.Chinese_TextDisplay = new String[1] { SystemTime_1.Minute.ToString("D2") };//【分】，1
                    customButtonMinute_1.English_TextDisplay = new String[1] { SystemTime_1.Minute.ToString("D2") };//【分】，1
                    customButtonSecond_1.Chinese_TextDisplay = new String[1] { SystemTime_1.Second.ToString("D2") };//【秒】，1
                    customButtonSecond_1.English_TextDisplay = new String[1] { SystemTime_1.Second.ToString("D2") };//【秒】，1

                    //

                    customButtonYear_2.Chinese_TextDisplay = new String[1] { SystemTime_2.Year.ToString("D4") };//【年】，2
                    customButtonYear_2.English_TextDisplay = new String[1] { SystemTime_2.Year.ToString("D4") };//【年】，2
                    customButtonMonth_2.Chinese_TextDisplay = new String[1] { SystemTime_2.Month.ToString("D2") };//【月】，2
                    customButtonMonth_2.English_TextDisplay = new String[1] { SystemTime_2.Month.ToString("D2") };//【月】，2
                    customButtonDay_2.Chinese_TextDisplay = new String[1] { SystemTime_2.Day.ToString("D2") };//【日】，2
                    customButtonDay_2.English_TextDisplay = new String[1] { SystemTime_2.Day.ToString("D2") };//【日】，2

                    customButtonHour_2.Chinese_TextDisplay = new String[1] { SystemTime_2.Hour.ToString("D2") };//【时】，2
                    customButtonHour_2.English_TextDisplay = new String[1] { SystemTime_2.Hour.ToString("D2") };//【时】，2
                    customButtonMinute_2.Chinese_TextDisplay = new String[1] { SystemTime_2.Minute.ToString("D2") };//【分】，2
                    customButtonMinute_2.English_TextDisplay = new String[1] { SystemTime_2.Minute.ToString("D2") };//【分】，2
                    customButtonSecond_2.Chinese_TextDisplay = new String[1] { SystemTime_2.Second.ToString("D2") };//【秒】，2
                    customButtonSecond_2.English_TextDisplay = new String[1] { SystemTime_2.Second.ToString("D2") };//【秒】，2
                    //
                    break;
                case DateTimePanelType.StatisticsTimeSearch_2://统计时间查找（日期）
                    //
                    customButtonYear_1.Chinese_TextDisplay = new String[1] { SystemTime_1.Year.ToString("D4") };//【年】，1
                    customButtonYear_1.English_TextDisplay = new String[1] { SystemTime_1.Year.ToString("D4") };//【年】，1
                    customButtonMonth_1.Chinese_TextDisplay = new String[1] { SystemTime_1.Month.ToString("D2") };//【月】，1
                    customButtonMonth_1.English_TextDisplay = new String[1] { SystemTime_1.Month.ToString("D2") };//【月】，1
                    customButtonDay_1.Chinese_TextDisplay = new String[1] { SystemTime_1.Day.ToString("D2") };//【日】，1
                    customButtonDay_1.English_TextDisplay = new String[1] { SystemTime_1.Day.ToString("D2") };//【日】，1
                    //
                    break;
                default:
                    break;
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：检查输入的数值是否改变
        // 输入参数：无
        // 输出参数：无
        // 返回值：检查结果。取值范围：true，改变；false，未改变
        //----------------------------------------------------------------------
        public Boolean _CheckEnteredValue()
        {
            Boolean bReturn = false;//返回值

            //

            switch (panelType)
            {
                case DateTimePanelType.Shift://班次设置
                    //

                    bReturn = !((0 == VisionSystemClassLibrary.Class.Shift._Compare(SystemTime_1, SystemTime_1_Original)) && (0 == VisionSystemClassLibrary.Class.Shift._Compare(SystemTime_2, SystemTime_2_Original)));

                    //
                    break;
                case DateTimePanelType.DateTime://日期时间设置
                    //
                    if (bDateTimeChanged)//输入了数值
                    {
                        bReturn = true;
                    }
                    //
                    break;
                case DateTimePanelType.StatisticsTimeSearch_1://统计时间查找（开始结束，日期时间）
                    //

                    bReturn = !((0 == VisionSystemClassLibrary.Class.Shift._Compare(SystemTime_1, SystemTime_1_Original)) && (0 == VisionSystemClassLibrary.Class.Shift._Compare(SystemTime_2, SystemTime_2_Original)));

                    //
                    break;
                case DateTimePanelType.StatisticsTimeSearch_2://统计时间查找（日期）
                    //

                    bReturn = !(0 == VisionSystemClassLibrary.Class.Shift._Compare(SystemTime_1, SystemTime_1_Original));

                    //
                    break;
                default:
                    break;
            }

            //

            return bReturn;
        }

        //事件

        //----------------------------------------------------------------------
        // 功能说明：点击按钮
        // 输入参数：1.iDigitalKeyboardWindowParameter：键盘窗口特征数值
        //         2.iButtonIndex：点击的按钮索引值，显示两组时间时有效。取值范围：1，开始时间；2，结束时间
        //         3.iDateTimeType：按钮类型。取值范围：1，年；2，月；3，日；4，时；5，分；6，秒
        //         4.iMaxLength：最大长度
        //         5.iMinValue：最小值
        //         6.iMaxValue：最大值
        //         7.sCurrentValue：当前数值
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _ClickDateTimeButton(Int32 iDigitalKeyboardWindowParameter, Int32 iButtonIndex, Int32 iDateTimeType, Byte byteMaxLength, Int16 iMinValue, Int16 iMaxValue, String sCurrentValue)
        {
            customButtonMessage.Visible = false;

            //

            iClickButtonIndex = iButtonIndex;

            GlobalWindows.DigitalKeyboard_Window.WindowParameter = iDigitalKeyboardWindowParameter;//窗口特征数值
            GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.Language = language;//语言
            if (DateTimePanelType.DateTime == panelType)//日期时间设置
            {
                GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.Chinese_Caption = sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][iDateTimeType];//中文标题文本
                GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.English_Caption = sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][iDateTimeType];//英文标题文本
            }
            else//其它
            {
                GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.Chinese_Caption = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][iButtonIndex] + "，" + sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][iDateTimeType];//中文标题文本
                GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.English_Caption = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][iButtonIndex] + "，" + sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][iDateTimeType];//英文标题文本
            }
            GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.Precision = 0;//输入的数据类型
            GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.MaxLength = byteMaxLength;//数值长度范围
            GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.MinValue = iMinValue;//最小值
            GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.MaxValue = iMaxValue;//最大值
            GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.StringValue = sCurrentValue;//初始显示的数值

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
        // 功能说明：年，1，点击按钮事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonYear_1_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickDateTimeButton(3, 0, 1, 4, Convert.ToInt16(1980), Convert.ToInt16(2099), SystemTime_1.Year.ToString("D4"));
        }

        //----------------------------------------------------------------------
        // 功能说明：年，2，点击按钮事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonYear_2_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickDateTimeButton(3, 1, 1, 4, Convert.ToInt16(1980), Convert.ToInt16(2099), SystemTime_2.Year.ToString("D4"));
        }

        //----------------------------------------------------------------------
        // 功能说明：月，1，点击按钮事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonMonth_1_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickDateTimeButton(4, 0, 2, 2, Convert.ToInt16(DateTime.MinValue.Month), Convert.ToInt16(DateTime.MaxValue.Month), SystemTime_1.Month.ToString("D2"));
        }

        //----------------------------------------------------------------------
        // 功能说明：月，2，点击按钮事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonMonth_2_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickDateTimeButton(4, 1, 2, 2, Convert.ToInt16(DateTime.MinValue.Month), Convert.ToInt16(DateTime.MaxValue.Month), SystemTime_2.Month.ToString("D2"));
        }

        //----------------------------------------------------------------------
        // 功能说明：日，1，点击按钮事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonDay_1_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickDateTimeButton(5, 0, 3, 2, Convert.ToInt16(DateTime.MinValue.Day), Convert.ToInt16(DateTime.DaysInMonth(SystemTime_1.Year, SystemTime_1.Month)), SystemTime_1.Day.ToString("D2"));
        }

        //----------------------------------------------------------------------
        // 功能说明：日，2，点击按钮事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonDay_2_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickDateTimeButton(5, 1, 3, 2, Convert.ToInt16(DateTime.MinValue.Day), Convert.ToInt16(DateTime.DaysInMonth(SystemTime_2.Year, SystemTime_2.Month)), SystemTime_2.Day.ToString("D2"));
        }

        //----------------------------------------------------------------------
        // 功能说明：时，1，点击按钮事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonHour_1_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickDateTimeButton(6, 0, 4, 2, Convert.ToInt16(DateTime.MinValue.Hour), Convert.ToInt16(DateTime.MaxValue.Hour), SystemTime_1.Hour.ToString("D2"));
        }

        //----------------------------------------------------------------------
        // 功能说明：时，2，点击按钮事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonHour_2_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickDateTimeButton(6, 1, 4, 2, Convert.ToInt16(DateTime.MinValue.Hour), Convert.ToInt16(DateTime.MaxValue.Hour), SystemTime_2.Hour.ToString("D2"));
        }

        //----------------------------------------------------------------------
        // 功能说明：分，1，点击按钮事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonMinute_1_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickDateTimeButton(7, 0, 5, 2, Convert.ToInt16(DateTime.MinValue.Minute), Convert.ToInt16(DateTime.MaxValue.Minute), SystemTime_1.Minute.ToString("D2"));
        }

        //----------------------------------------------------------------------
        // 功能说明：分，2，点击按钮事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonMinute_2_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickDateTimeButton(7, 1, 5, 2, Convert.ToInt16(DateTime.MinValue.Minute), Convert.ToInt16(DateTime.MaxValue.Minute), SystemTime_2.Minute.ToString("D2"));
        }

        //----------------------------------------------------------------------
        // 功能说明：秒，1，点击按钮事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonSecond_1_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickDateTimeButton(8, 0, 6, 2, Convert.ToInt16(DateTime.MinValue.Second), Convert.ToInt16(DateTime.MaxValue.Second), SystemTime_1.Second.ToString("D2"));
        }

        //----------------------------------------------------------------------
        // 功能说明：秒，2，点击按钮事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonSecond_2_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickDateTimeButton(8, 1, 6, 2, Convert.ToInt16(DateTime.MinValue.Second), Convert.ToInt16(DateTime.MaxValue.Second), SystemTime_2.Second.ToString("D2"));
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：点击【OK】按钮事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonOk_CustomButton_Click(object sender, EventArgs e)
        {
            switch (panelType)
            {
                case DateTimePanelType.Shift://班次设置
                    //
                    if (bShiftTimeCheck)//班次时间检查
                    {
                        if (VisionSystemClassLibrary.Class.Shift._Check(SystemTime_1, SystemTime_2))//正确
                        {
                            if (_CheckEnteredValue())//输入新数值
                            {
                                bEnterNewValue = true;
                            }
                            else//未输入新数值
                            {
                                bEnterNewValue = false;
                            }

                            bDateTimeChanged = false;

                            panelType = DateTimePanelType.None;

                            customButtonMessage.Visible = false;

                            //事件

                            if (null != Close_Click)//有效
                            {
                                Close_Click(this, new CustomEventArgs());
                            }
                        }
                        else//错误
                        {
                            customButtonMessage.Visible = true;
                        }
                    } 
                    else//不进行班次时间检查
                    {
                        if (_CheckEnteredValue())//输入新数值
                        {
                            bEnterNewValue = true;
                        }
                        else//未输入新数值
                        {
                            bEnterNewValue = false;
                        }

                        bDateTimeChanged = false;

                        panelType = DateTimePanelType.None;

                        customButtonMessage.Visible = false;

                        //事件

                        if (null != Close_Click)//有效
                        {
                            Close_Click(this, new CustomEventArgs());
                        }
                    }
                    //
                    break;
                case DateTimePanelType.DateTime://日期时间设置
                    //
                    if (VisionSystemClassLibrary.Class.Shift._Check(SystemTime_1))//正确
                    {
                        if (_CheckEnteredValue())//输入新数值
                        {
                            bEnterNewValue = true;
                        }
                        else//未输入新数值
                        {
                            bEnterNewValue = false;
                        }

                        bDateTimeChanged = false;

                        panelType = DateTimePanelType.None;

                        customButtonMessage.Visible = false;

                        //事件

                        if (null != Close_Click)//有效
                        {
                            Close_Click(this, new CustomEventArgs());
                        }
                    }
                    else//错误
                    {
                        customButtonMessage.Visible = true;
                    }
                    //
                    break;
                case DateTimePanelType.StatisticsTimeSearch_1://统计时间查找（开始结束，日期时间）
                    //
                    if (VisionSystemClassLibrary.Class.Shift._Check(SystemTime_1) && VisionSystemClassLibrary.Class.Shift._Check(SystemTime_2))//正确
                    {
                        if (_CheckEnteredValue())//输入新数值
                        {
                            bEnterNewValue = true;
                        }
                        else//未输入新数值
                        {
                            bEnterNewValue = false;
                        }

                        bDateTimeChanged = false;

                        panelType = DateTimePanelType.None;

                        customButtonMessage.Visible = false;

                        //事件

                        if (null != Close_Click)//有效
                        {
                            Close_Click(this, new CustomEventArgs());
                        }
                    }
                    else//错误
                    {
                        customButtonMessage.Visible = true;
                    }
                    //
                    break;
                case DateTimePanelType.StatisticsTimeSearch_2://统计时间查找（日期）
                    //
                    if (VisionSystemClassLibrary.Class.Shift._Check(SystemTime_1))//正确
                    {
                        if (_CheckEnteredValue())//输入新数值
                        {
                            bEnterNewValue = true;
                        }
                        else//未输入新数值
                        {
                            bEnterNewValue = false;
                        }

                        bDateTimeChanged = false;

                        panelType = DateTimePanelType.None;

                        customButtonMessage.Visible = false;

                        //事件

                        if (null != Close_Click)//有效
                        {
                            Close_Click(this, new CustomEventArgs());
                        }
                    }
                    else//错误
                    {
                        customButtonMessage.Visible = true;
                    }
                    //
                    break;
                default:
                    break;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【CANCEL】按钮事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonCancel_CustomButton_Click(object sender, EventArgs e)
        {
            bEnterNewValue = false;

            bDateTimeChanged = false;

            panelType = DateTimePanelType.None;

            customButtonMessage.Visible = false;

            //事件

            if (null != Close_Click)//有效
            {
                Close_Click(this, new CustomEventArgs());
            }
        }
        
        //

        //----------------------------------------------------------------------
        // 功能说明：设置输入完成的数值
        // 输入参数：1.iSystemTimeValue：数值
        //         2.custombutton：按钮
        //         3.sFormat：显示格式
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _EnterDateTimeValue(ref UInt16 iSystemTimeValue, CustomButton custombutton, String sFormat)
        {
            bDateTimeChanged = true;

            iSystemTimeValue = (UInt16)GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.NumericalValue;

            custombutton.Chinese_TextDisplay = new String[1] { iSystemTimeValue.ToString(sFormat) };//年，1
            custombutton.English_TextDisplay = new String[1] { iSystemTimeValue.ToString(sFormat) };//年，1
        }

        //----------------------------------------------------------------------
        // 功能说明：DATETIMEPANEL，年，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void digitalKeyboardWindow_WindowClose_DateTimePanel_Year(object sender, EventArgs e)
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
                if (0 == iClickButtonIndex)//开始时间
                {
                    _EnterDateTimeValue(ref SystemTime_1.Year, customButtonYear_1, "D4");
                }
                else//1，结束时间
                {
                    _EnterDateTimeValue(ref SystemTime_2.Year, customButtonYear_2, "D4");
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：DATETIMEPANEL，月，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void digitalKeyboardWindow_WindowClose_DateTimePanel_Month(object sender, EventArgs e)
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
                if (0 == iClickButtonIndex)//开始时间
                {
                    _EnterDateTimeValue(ref SystemTime_1.Month, customButtonMonth_1, "D2");
                }
                else//1，结束时间
                {
                    _EnterDateTimeValue(ref SystemTime_2.Month, customButtonMonth_2, "D2");
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：DATETIMEPANEL，日，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void digitalKeyboardWindow_WindowClose_DateTimePanel_Day(object sender, EventArgs e)
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
                if (0 == iClickButtonIndex)//开始时间
                {
                    _EnterDateTimeValue(ref SystemTime_1.Day, customButtonDay_1, "D2");
                }
                else//1，结束时间
                {
                    _EnterDateTimeValue(ref SystemTime_2.Day, customButtonDay_2, "D2");
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：DATETIMEPANEL，时，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void digitalKeyboardWindow_WindowClose_DateTimePanel_Hour(object sender, EventArgs e)
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
                if (0 == iClickButtonIndex)//开始时间
                {
                    _EnterDateTimeValue(ref SystemTime_1.Hour, customButtonHour_1, "D2");
                }
                else//1，结束时间
                {
                    _EnterDateTimeValue(ref SystemTime_2.Hour, customButtonHour_2, "D2");
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：DATETIMEPANEL，分，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void digitalKeyboardWindow_WindowClose_DateTimePanel_Minute(object sender, EventArgs e)
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
                if (0 == iClickButtonIndex)//开始时间
                {
                    _EnterDateTimeValue(ref SystemTime_1.Minute, customButtonMinute_1, "D2");
                }
                else//1，结束时间
                {
                    _EnterDateTimeValue(ref SystemTime_2.Minute, customButtonMinute_2, "D2");
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：DATETIMEPANEL，秒，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void digitalKeyboardWindow_WindowClose_DateTimePanel_Second(object sender, EventArgs e)
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
                if (0 == iClickButtonIndex)//开始时间
                {
                    _EnterDateTimeValue(ref SystemTime_1.Second, customButtonSecond_1, "D2");
                }
                else//1，结束时间
                {
                    _EnterDateTimeValue(ref SystemTime_2.Second, customButtonSecond_2, "D2");
                }
            }
        }
    }

    //日期时间设置面板类型
    public enum DateTimePanelType
    {
        None = 0,//无意义
        Shift = 1,//班次设置
        DateTime = 2,//日期时间设置
        StatisticsTimeSearch_1 = 3,//统计时间查找（开始结束，日期时间）
        StatisticsTimeSearch_2 = 4,//统计时间查找（日期）
    }
}