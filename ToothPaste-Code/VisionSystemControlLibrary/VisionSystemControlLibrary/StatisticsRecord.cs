/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：StatisticsRecord.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：统计记录控件

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
    public partial class StatisticsRecord : UserControl
    {
        //该控件为统计记录控件

        private VisionSystemClassLibrary.Enum.InterfaceLanguage language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;//属性，语言

        //

        private VisionSystemClassLibrary.Class.Shift shift = new VisionSystemClassLibrary.Class.Shift();//属性，班次

        //

        private Int32 iDeleteType = 0;//属性（只读），删除统计数据方式（0，删除所有；1，删除指定班次；2，删除指定记录）

        private Int32[] iSelectedRecordIndex = null;//属性（只读），列表2中选择的列表项的数据索引值
        private VisionSystemClassLibrary.Struct.ShiftTime[] shifttimeSelectedRecord = null;//属性（只读），列表2中选择的统计记录的开始结束时间

        private VisionSystemClassLibrary.Struct.ShiftTime shifttimeSelectedRecordTemp = new VisionSystemClassLibrary.Struct.ShiftTime();//属性（只读），列表2中选择的统计记录的开始结束时间

        //

        private VisionSystemClassLibrary.Enum.CameraType cameraType = VisionSystemClassLibrary.Enum.CameraType.Camera_1;//属性，相机类型

        private String sCameraName_Chinese = "";//属性，相机中文名称
        private String sCameraName_English = "";//属性，相机英文名称

        //

        private Boolean bSelectNewRecord = false;//属性（只读），是否选择了新的统计记录。取值范围：true，是；false，否

        //

        private Boolean bClickDeleteButton = true;//点击【DELETE】或【DELETE ALL】按钮。取值范围：true，【DELETE】；false，【DELETE ALL】

        //

        private Boolean bRecordMessageWindowShow = false;//属性（只读），是否显示正在执行操作时的提示信息窗口。取值范围：true，是；false，否

        private const Int32 iTimerRecordMaxCount = 90;//定时器时间
        private Int32 iTimerRecordCount = 90;//定时器时间

        //

        private Boolean bDeleteRecord = false;//是否进行了删除统计数据的操作。取值范围：true，是；false，否

        //

        private Boolean bDeleteRecordSuccess = false;//删除统计数据是否成功。取值范围：true，是；false，否

        //

        private Boolean bClickSearchButton = false;//是否按下【SEARCH】按钮。取值范围：true，是；false，否

        private Boolean bSetSearchDateTime = false;//是否设置了日期时间数值。取值范围：true，是；false，否
        private DateTime SearchDateTime = new DateTime();//查找功能的日期时间数值

        private Int32[] iSearch_RecordNumber = null;//【SEARCH】按钮按下时，每个班次统计记录的个数
        private Int32[][] iSearch_RecordIndex = null;//【SEARCH】按钮按下时，每个班次统计记录的索引值

        //

        private String[][] sMessageText = new String[2][];//提示信息对话框、列表中显示的文本（[语言][包含的文本]）
        private String[][] sMessageText_1 = new String[2][];//控件上显示的文本（[语言][包含的文本]）

        //

        [Browsable(true), Description("点击【DELETE】（【DELETE ALL】）按钮时产生的事件"), Category("StatisticsRecord 事件")]
        public event EventHandler Delete_Click;//点击【DELETE】（【DELETE ALL】）按钮时产生的事件

        [Browsable(true), Description("窗口关闭时产生的事件"), Category("StatisticsRecord 事件")]
        public event EventHandler Close_Click;//窗口关闭时产生的事件

        //构造函数

        //----------------------------------------------------------------------
        // 功能说明：构造函数，初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public StatisticsRecord()
        {
            InitializeComponent();

            //

            if (null != GlobalWindows.MessageDisplay_Window)
            {
                GlobalWindows.MessageDisplay_Window.WindowClose_StatisticsRecord_GetRecord_Wait += new System.EventHandler(messageDisplayWindow_WindowClose_StatisticsRecord_GetRecord_Wait);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_StatisticsRecord_Delete_Confirm += new System.EventHandler(messageDisplayWindow_WindowClose_StatisticsRecord_Delete_Confirm);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_StatisticsRecord_Delete_Wait += new System.EventHandler(messageDisplayWindow_WindowClose_StatisticsRecord_Delete_Wait);//订阅事件

                GlobalWindows.MessageDisplay_Window.WindowClose_StatisticsRecord_Ok_Confirm += new System.EventHandler(messageDisplayWindow_WindowClose_StatisticsRecord_Ok_Confirm);//订阅事件
            }

            if (null != GlobalWindows.DateTimePanel_Window)
            {
                GlobalWindows.DateTimePanel_Window.WindowClose_StatisticsSearch += new System.EventHandler(dateTimePanelWindow_WindowClose_StatisticsSearch);//订阅事件
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
                    sMessageText_1[i] = new String[1];
                }

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] = "确定删除所选记录";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] = "Delete Selected Statistics Records";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][1] = "确定删除所有统计记录";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][1] = "Delete All Statistics Records";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][2] = "班次";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][2] = "Shift";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][3] = "正在删除统计记录";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][3] = "Deleting Statistics Records";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][4] = "删除统计记录成功";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][4] = "Statistics Records Deleted Completed";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][5] = "删除统计记录失败";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][5] = "Statistics Records Deleted Failed";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][6] = "确定选择统计记录";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][6] = "Select the Statistics Record";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][7] = "正在获取统计数据";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][7] = "Getting Statistics Records";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][8] = "获取统计数据失败";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][8] = "Getting Statistics Records Failed";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][9] = "请等待";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][9] = "Please wait";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][10] = "查找";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][10] = "Search";

                //

                sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] = customButtonCaption.Chinese_TextDisplay[0];
                sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] = customButtonCaption.English_TextDisplay[0];
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Language属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("语言"), Category("StatisticsRecord 通用")]
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
        // 功能说明：SystemShift属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("班次"), Category("StatisticsRecord 通用")]
        public VisionSystemClassLibrary.Class.Shift SystemShift
        {
            get//读取
            {
                return shift;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：SelectedCameraType属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("相机类型"), Category("StatisticsRecord 通用")]
        public VisionSystemClassLibrary.Enum.CameraType SelectedCameraType
        {
            get//读取
            {
                return cameraType;
            }
            set//设置
            {
                cameraType = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Chinese_CameraName属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(false), Description("相机中文名称"), Category("StatisticsRecord 通用")]
        public String Chinese_CameraName
        {
            get//读取
            {
                return sCameraName_Chinese;
            }
            set//设置
            {
                if (sCameraName_Chinese != value)//有效
                {
                    sCameraName_Chinese = value;

                    //

                    customButtonCaption.Chinese_TextDisplay = new String[1] { sCameraName_Chinese + sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] };//设置显示的文本
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：English_CameraName属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(false), Description("相机英文名称"), Category("StatisticsRecord 通用")]
        public String English_CameraName
        {
            get//读取
            {
                return sCameraName_English;
            }
            set//设置
            {
                if (sCameraName_English != value)//有效
                {
                    sCameraName_English = value;

                    //

                    customButtonCaption.English_TextDisplay = new String[1] { sCameraName_English + sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] };//设置显示的文本
                }
            }
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：SelectNewRecord属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("是否选择了新的统计记录。取值范围：true，是；false，否"), Category("StatisticsRecord 通用")]
        public Boolean SelectNewRecord
        {
            get//读取
            {
                return bSelectNewRecord;
            }
            set
            {
                bSelectNewRecord = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ShiftTimeSelectedRecord属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("列表2中选择的统计记录的开始结束时间"), Category("StatisticsRecord 通用")]
        public VisionSystemClassLibrary.Struct.ShiftTime ShiftTimeSelectedRecord
        {
            get//读取
            {
                return shifttimeSelectedRecord[customList_1.CurrentListIndex];
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ShiftTimeSelectedRecordTemp属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("列表2中选择的统计记录的开始结束时间"), Category("StatisticsRecord 通用")]
        public VisionSystemClassLibrary.Struct.ShiftTime ShiftTimeSelectedRecordTemp
        {
            get//读取
            {
                return shifttimeSelectedRecordTemp;
            }
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：DeleteType属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("删除统计数据方式（0，删除所有；1，删除指定班次；2，删除指定记录）"), Category("StatisticsRecord 通用")]
        public Int32 DeleteType
        {
            get//读取
            {
                return iDeleteType;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：SelectedShiftIndex属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("列表1中选择的班次索引值"), Category("StatisticsRecord 通用")]
        public Int32 SelectedShiftIndex
        {
            get//读取
            {
                return customList_1.CurrentListIndex;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：SelectedRecordIndex属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("列表2中选择的列表项的数据索引值"), Category("StatisticsRecord 通用")]
        public Int32 SelectedRecordIndex
        {
            get//读取
            {
                return iSelectedRecordIndex[customList_1.CurrentListIndex];
            }
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：RecordMessageWindowShow属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("是否显示正在执行操作时的提示信息窗口。取值范围：true，是；false，否"), Category("StatisticsRecord 通用")]
        public Boolean RecordMessageWindowShow
        {
            get//读取
            {
                return bRecordMessageWindowShow;
            }
        }

        //函数

        //----------------------------------------------------------------------
        // 功能说明：应用设置的属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _Properties(VisionSystemClassLibrary.Class.Shift shift_parameter)
        {
            shift = shift_parameter;

            //

            iSelectedRecordIndex = new Int32[shift.DataOfShift.TimeData.Length];
            shifttimeSelectedRecord = new VisionSystemClassLibrary.Struct.ShiftTime[shift.DataOfShift.TimeData.Length];
            for (Int32 i = 0; i < shifttimeSelectedRecord.Length; i++)
            {
                shifttimeSelectedRecord[i] = new VisionSystemClassLibrary.Struct.ShiftTime();
            }

            iSearch_RecordNumber = new Int32[shift.DataOfShift.TimeData.Length];//【SEARCH】按钮按下时，每个班次统计记录的个数
            iSearch_RecordIndex = new Int32[shift.DataOfShift.TimeData.Length][];//【SEARCH】按钮按下时，每个班次统计记录的索引值
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：启动获取统计数据
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _StartGetStatisticsRecord()
        {
            bRecordMessageWindowShow = true;

            //显示信息对话框

            GlobalWindows.MessageDisplay_Window.WindowParameter = 88;//窗口特征数值
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.None;//不包含任何按钮
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][7] + "...";
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][7] + "...";
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = " ";
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = " ";
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][9] + "，" + iTimerRecordCount.ToString();
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][9] + "，" + iTimerRecordCount.ToString();

            GlobalWindows.MessageDisplay_Window.StartPosition = FormStartPosition.CenterScreen;
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.MessageDisplay_Window.TopMost = true;//将窗口置于顶层
            }
            else//其它
            {
                GlobalWindows.MessageDisplay_Window.Visible = true;//显示
            }

            timerRecord.Start();//启动定时器
        }

        //----------------------------------------------------------------------
        // 功能说明：获取统计数据
        // 输入参数：1.bSuccess：操作是否成功。取值范围：true，是；false，否
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _GetStatisticsData(Boolean bSuccess)
        {
            bRecordMessageWindowShow = false;

            iTimerRecordCount = iTimerRecordMaxCount;

            timerRecord.Stop();//关闭定时器

            //显示信息对话框

            if (bSuccess)//成功
            {
                _SetData();

                //

                if (bDeleteRecord)//删除统计数据
                {
                    bDeleteRecordSuccess = true;

                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = " ";
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = " ";
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][4];
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][4];
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = " ";
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = " ";
                }
                else
                {
                    if (GlobalWindows.TopMostWindows)//置顶
                    {
                        GlobalWindows.MessageDisplay_Window.TopMost = false;//取消置于顶层
                    }
                    else//其它
                    {
                        GlobalWindows.MessageDisplay_Window.Visible = false;//隐藏
                    }
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl._Reset();
                }
            }
            else//失败
            {
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = " ";
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = " ";
                if (bDeleteRecord)//删除统计数据
                {
                    bDeleteRecordSuccess = false;

                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][5];
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][5];
                }
                else
                {
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][8];
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][8];
                }
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = " ";
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = " ";
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：删除统计记录（若删除统计页面当前显示的记录，相对本控件相关变量和统计页面进行操作）
        // 输入参数：1.bSuccess：操作是否成功。取值范围：true，是；false，否
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _DeleteRecords(Boolean bSuccess)
        {
            //显示信息对话框

            if (bSuccess)//成功
            {
                //不执行操作
            }
            else//失败
            {
                bRecordMessageWindowShow = false;

                iTimerRecordCount = iTimerRecordMaxCount;

                timerRecord.Stop();//关闭定时器

                //

                bDeleteRecordSuccess = false;

                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = " ";
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = " ";
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][5];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][5];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = " ";
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = " ";
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：设置页面数据
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetData()
        {
            if (!bSetSearchDateTime)//未设置
            {
                SearchDateTime = DateTime.Now;

                _SetSearchDateTimeButton();
            }

            //

            _GetSearchRecord();

            _GetSelectedListItemIndex();

            //

            _InitList();

            _SetDeleteButton();
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：获取查找时的统计记录信息
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _GetSearchRecord()
        {
            Int32 i = 0;//循环控制变量
            Int32 j = 0;//循环控制变量

            Int32 k = 0;//临时变量
            Int32[] iValue = null;//临时变量

            VisionSystemClassLibrary.Struct.SYSTEMTIME systemtimeStart = new VisionSystemClassLibrary.Struct.SYSTEMTIME();
            systemtimeStart.Year = (UInt16)SearchDateTime.Year;
            systemtimeStart.Month = (UInt16)SearchDateTime.Month;
            systemtimeStart.Day = (UInt16)SearchDateTime.Day;
            systemtimeStart.Hour = 0;
            systemtimeStart.Minute = 0;
            systemtimeStart.Second = 0;

            VisionSystemClassLibrary.Struct.SYSTEMTIME systemtimeEnd = new VisionSystemClassLibrary.Struct.SYSTEMTIME();
            systemtimeEnd.Year = (UInt16)SearchDateTime.Year;
            systemtimeEnd.Month = (UInt16)SearchDateTime.Month;
            systemtimeEnd.Day = (UInt16)SearchDateTime.Day;
            systemtimeEnd.Hour = 23;
            systemtimeEnd.Minute = 59;
            systemtimeEnd.Second = 59;

            //

            for (i = 0; i < shift.DataOfShift.TimeData.Length; i++)//遍历班次
            {
                if (null != shift.DataOfShift.InformationOfStatistics[i].TimeData)//有效
                {
                    k = 0;

                    iValue = new Int32[shift.DataOfShift.InformationOfStatistics[i].TimeData.Length];

                    //

                    for (j = 0; j < shift.DataOfShift.InformationOfStatistics[i].TimeData.Length; j++)
                    {
                        if (0 <= VisionSystemClassLibrary.Class.Shift._Compare(shift.DataOfShift.InformationOfStatistics[i].TimeData[j].Start, systemtimeStart) && 0 >= VisionSystemClassLibrary.Class.Shift._Compare(shift.DataOfShift.InformationOfStatistics[i].TimeData[j].Start, systemtimeEnd))//有效
                        {
                            iValue[k] = j;

                            k++;
                        }
                    }

                    //

                    iSearch_RecordNumber[i] = k;

                    if (0 < k)//存在
                    {
                        iSearch_RecordIndex[i] = new Int32[k];

                        for (j = 0; j < k; j++)//遍历
                        {
                            iSearch_RecordIndex[i][j] = iValue[j];
                        }
                    } 
                    else//不存在
                    {
                        iSearch_RecordIndex[i] = null;
                    }
                }
                else//无效
                {
                    iSearch_RecordNumber[i] = 0;
                    iSearch_RecordIndex[i] = null;
                }
            }
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：获取字符串形式的班次时间
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        private String _GetDateTime(DateTime dateTime)
        {
            return dateTime.Year.ToString("D4") + "." + dateTime.Month.ToString("D2") + "." + dateTime.Day.ToString("D2");
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
            customButtonCaption.Language = language;//标题

            customButtonSearch.Language = language;//【SEARCH】
            customButtonDelete.Language = language;//【DELETE】
            customButtonDeleteAll.Language = language;//【DELETE】

            customList_1.Language = language;//列表1
            customList_2.Language = language;//列表2
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：设置【SEARCH DATE TIME】按钮
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetSearchDateTimeButton()
        {
            customButtonDateTime.Chinese_TextDisplay = new String[1] { _GetDateTime(SearchDateTime) };//设置显示的文本
            customButtonDateTime.English_TextDisplay = new String[1] { _GetDateTime(SearchDateTime) };//设置显示的文本
        }

        //----------------------------------------------------------------------
        // 功能说明：设置【DELETE ALL】，【DELETE】按钮
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetDeleteButton()
        {
            Int32 i = 0;//循环控制变量

            for (i = 0; i < shift.DataOfShift.TimeData.Length; i++)//遍历
            {
                if (null != shift.DataOfShift.InformationOfStatistics[i].TimeData)//存在统计记录
                {
                    break;
                }
            }

            //

            if (i < shift.DataOfShift.TimeData.Length)//存在统计记录
            {
                customButtonDeleteAll.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【DELETE ALL】

                if (0 <= customList_2.CurrentListIndex)//选择
                {
                    customButtonDelete.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【DELETE】
                }
                else//其它
                {
                    customButtonDelete.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【DELETE】
                }
            }
            else//不存在统计记录
            {
                customButtonDelete.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【DELETE】

                customButtonDeleteAll.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【DELETE ALL】
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：获取统计记录名称
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private String _GetSelectedShiftRecordString(VisionSystemClassLibrary.Enum.InterfaceLanguage language_temp)
        {
            return sMessageText[(Int32)language_temp - 1][2] + " " + (shift.DataOfShift.CurrentIndex + 1).ToString() + "，" + VisionSystemClassLibrary.Class.Shift._GetDateTime(shift.DataOfShift.CurrentStatisticsInformation.TimeData[0].Start, shift.DataOfShift.CurrentStatisticsInformation.TimeData[0].End);
        }

        //----------------------------------------------------------------------
        // 功能说明：获取统计记录名称
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private String _GetSelectedShiftRecordString(VisionSystemClassLibrary.Enum.InterfaceLanguage language_temp, Int32 iShiftIndex, Int32 iRecordIndex)
        {
            return sMessageText[(Int32)language_temp - 1][2] + " " + (iShiftIndex + 1).ToString() + "，" + VisionSystemClassLibrary.Class.Shift._GetDateTime(shift.DataOfShift.InformationOfStatistics[iShiftIndex].TimeData[iRecordIndex].Start, shift.DataOfShift.InformationOfStatistics[iShiftIndex].TimeData[iRecordIndex].End);
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：初始化列表2时，获取统计页面当前选择的统计记录在该列表中的项的索引值
        // 输入参数：无
        // 输出参数：无
        // 返回值：索引值
        //----------------------------------------------------------------------
        private Int32 _GetSelectedRecordInListIndex()
        {
            Int32 iReturn = -1;//返回值

            Int32 i = 0;//循环控制变量

            if (0 <= customList_1.CurrentListIndex)//有效
            {
                if (0 <= iSelectedRecordIndex[customList_1.CurrentListIndex])//有效
                {
                    iReturn = 0;//默认选择第一项

                    if (bClickSearchButton)//【SEARCH】按下
                    {
                        for (i = 0; i < iSearch_RecordNumber[customList_1.CurrentListIndex]; i++)//列表项数据
                        {
                            if (iSelectedRecordIndex[customList_1.CurrentListIndex] == iSearch_RecordIndex[customList_1.CurrentListIndex][i])//相同
                            {
                                iReturn = i;

                                //

                                break;
                            }
                        }
                    }
                    else//【SEARCH】弹起
                    {
                        for (i = 0; i < shift.DataOfShift.InformationOfStatistics[customList_1.CurrentListIndex].DataOfStatistics.Length; i++)//列表项数据
                        {
                            if (i == iSelectedRecordIndex[customList_1.CurrentListIndex])//相同
                            {
                                iReturn = i;

                                //

                                break;
                            }
                        }
                    }
                }
            }

            //

            return iReturn;
        }

        //----------------------------------------------------------------------
        // 功能说明：获取查找时的统计记录信息
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _GetSelectedListItemIndex()
        {
            Int32 i = 0;//循环控制变量
            Int32 j = 0;//循环控制变量

            if (bClickSearchButton)//【SEARCH】按下
            {
                for (i = 0; i < iSearch_RecordNumber.Length; i++)//列表项数据
                {
                    if (0 < iSearch_RecordNumber[i])//有效
                    {
                        iSelectedRecordIndex[i] = iSearch_RecordIndex[i][0];
                        shifttimeSelectedRecord[i] = shift.DataOfShift.InformationOfStatistics[i].TimeData[iSearch_RecordIndex[i][0]];

                        //

                        if (i == shift.DataOfShift.CurrentIndex)//当前班次
                        {
                            for (j = 0; j < iSearch_RecordNumber[i]; j++)//列表项数据
                            {
                                if (iSearch_RecordIndex[i][j] == shift.DataOfShift.InformationOfStatistics[i].CurrentIndex)//相同
                                {
                                    iSelectedRecordIndex[i] = shift.DataOfShift.InformationOfStatistics[i].CurrentIndex;

                                    shifttimeSelectedRecord[i] = shift.DataOfShift.InformationOfStatistics[i].TimeData[iSelectedRecordIndex[i]];

                                    //

                                    break;
                                }
                            }
                        }
                    }
                    else//无效
                    {
                        iSelectedRecordIndex[i] = -1;
                        shifttimeSelectedRecord[i] = new VisionSystemClassLibrary.Struct.ShiftTime();
                    }
                }
            }
            else//【SEARCH】弹起
            {
                for (i = 0; i < shift.DataOfShift.TimeData.Length; i++)
                {
                    if (null != shift.DataOfShift.InformationOfStatistics[i].DataOfStatistics)//有效
                    {
                        iSelectedRecordIndex[i] = 0;
                        shifttimeSelectedRecord[i] = shift.DataOfShift.InformationOfStatistics[i].TimeData[0];

                        //

                        if (i == shift.DataOfShift.CurrentIndex)//当前班次
                        {
                            for (j = 0; j < shift.DataOfShift.InformationOfStatistics[i].DataOfStatistics.Length; j++)//列表项数据
                            {
                                if (j == shift.DataOfShift.InformationOfStatistics[i].CurrentIndex)//相同
                                {
                                    iSelectedRecordIndex[i] = shift.DataOfShift.InformationOfStatistics[i].CurrentIndex;

                                    shifttimeSelectedRecord[i] = shift.DataOfShift.InformationOfStatistics[i].TimeData[iSelectedRecordIndex[i]];

                                    //

                                    break;
                                }
                            }
                        }
                    }
                    else//无效
                    {
                        iSelectedRecordIndex[i] = -1;
                        shifttimeSelectedRecord[i] = new VisionSystemClassLibrary.Struct.ShiftTime();
                    }
                }
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：初始化列表
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _InitList()
        {
            customList_1._ApplyListHeader();//应用列表头属性
            customList_1._ApplyListItem();//应用列表项属性

            customList_2._ApplyListHeader();//应用列表头属性
            customList_2._ApplyListItem();//应用列表项属性

            //

            _SetList_1();//设置列表1

            _SetList_2();//设置列表2

            shifttimeSelectedRecordTemp.Start = shifttimeSelectedRecord[customList_1.CurrentListIndex].Start;
            shifttimeSelectedRecordTemp.End = shifttimeSelectedRecord[customList_1.CurrentListIndex].End;
        }

        //----------------------------------------------------------------------
        // 功能说明：设置列表1
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetList_1()
        {
            if (0 <= shift.DataOfShift.CurrentIndex)//有效
            {
                customList_1._Apply(shift.DataOfShift.TimeData.Length, shift.DataOfShift.CurrentIndex, shift.DataOfShift.CurrentIndex);//应用列表属性
            }
            else
            {
                customList_1._Apply(shift.DataOfShift.TimeData.Length, 0, 0);//应用列表属性
            }
            _AddItemData_1();//添加列表项数据
            _SetPage_1();//设置列表数据
        }

        //----------------------------------------------------------------------
        // 功能说明：设置列表2
        // 输入参数：1.bInit：是否进入页面时首次显示。取值范围：true，是；false，否
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetList_2()
        {
            Int32 iIndex = _GetSelectedRecordInListIndex();

            //

            if (bClickSearchButton)//【SEARCH】按钮按下
            {
                if (0 <= iIndex)//有效
                {
                    customList_2._Apply(iSearch_RecordNumber[customList_1.CurrentListIndex], iSearch_RecordIndex[customList_1.CurrentListIndex][iIndex], iIndex);//应用列表属性
                }
                else//无效
                {
                    customList_2._Apply(0);//应用列表属性
                }
            } 
            else//【SEARCH】按钮弹起
            {
                if (0 <= iIndex)//有效
                {
                    customList_2._Apply(shift.DataOfShift.InformationOfStatistics[customList_1.CurrentListIndex].TimeData.Length, iIndex, iIndex);//应用列表属性
                }
                else//无效
                {
                    customList_2._Apply(0);//应用列表属性
                }
            }

            _AddItemData_2();//添加列表项数据
            _SetPage_2();//设置列表数据

            //

            if (0 <= customList_2.CurrentDataIndex)//有效
            {
                shifttimeSelectedRecord[customList_1.CurrentListIndex].Start = shift.DataOfShift.InformationOfStatistics[customList_1.CurrentListIndex].TimeData[customList_2.CurrentDataIndex].Start;
                shifttimeSelectedRecord[customList_1.CurrentListIndex].End = shift.DataOfShift.InformationOfStatistics[customList_1.CurrentListIndex].TimeData[customList_2.CurrentDataIndex].End;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：添加列表1数据
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _AddItemData_1()
        {
            Int32 i = 0;//循环控制变量

            for (i = 0; i < shift.DataOfShift.TimeData.Length; i++)//列表项数据
            {
                customList_1.ItemData[i].ItemText[0] = (i + 1).ToString();//班次
                customList_1.ItemData[i].ItemText[1] = VisionSystemClassLibrary.Class.Shift._GetShiftTime(shift.DataOfShift.TimeData[i].Start, shift.DataOfShift.TimeData[i].End);//班次时间

                //

                customList_1.ItemData[i].ItemFlag = i;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：添加列表2数据
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _AddItemData_2()
        {
            Int32 i = 0;//循环控制变量

            if (bClickSearchButton)//【SEARCH】按下
            {
                for (i = 0; i < iSearch_RecordNumber[customList_1.CurrentListIndex]; i++)//列表项数据
                {
                    customList_2.ItemData[i].ItemText[0] = VisionSystemClassLibrary.Class.Shift._GetDateTime(shift.DataOfShift.InformationOfStatistics[customList_1.CurrentListIndex].TimeData[iSearch_RecordIndex[customList_1.CurrentListIndex][i]].Start, shift.DataOfShift.InformationOfStatistics[customList_1.CurrentListIndex].TimeData[iSearch_RecordIndex[customList_1.CurrentListIndex][i]].End);//班次时间
                    customList_2.ItemData[i].ItemText[1] = shift.DataOfShift.InformationOfStatistics[customList_1.CurrentListIndex].DataOfStatistics[iSearch_RecordIndex[customList_1.CurrentListIndex][i]].BrandName;//品牌

                    //

                    customList_2.ItemData[i].ItemFlag = iSearch_RecordIndex[customList_1.CurrentListIndex][i];
                }
            } 
            else//【SEARCH】弹起
            {
                if (null != shift.DataOfShift.InformationOfStatistics[customList_1.CurrentListIndex].DataOfStatistics)//有效
                {
                    for (i = 0; i < shift.DataOfShift.InformationOfStatistics[customList_1.CurrentListIndex].DataOfStatistics.Length; i++)//列表项数据
                    {
                        customList_2.ItemData[i].ItemText[0] = VisionSystemClassLibrary.Class.Shift._GetDateTime(shift.DataOfShift.InformationOfStatistics[customList_1.CurrentListIndex].TimeData[i].Start, shift.DataOfShift.InformationOfStatistics[customList_1.CurrentListIndex].TimeData[i].End);//班次时间
                        customList_2.ItemData[i].ItemText[1] = shift.DataOfShift.InformationOfStatistics[customList_1.CurrentListIndex].DataOfStatistics[i].BrandName;//品牌

                        //

                        customList_2.ItemData[i].ItemFlag = i;
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：设置列表1数据
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetPage_1()
        {
            customList_1._SetPage();//设置列表项数据

            if (1 < customList_1.TotalPage)//多于一页
            {
                customButtonPreviousPage_List_1.Visible = true;//【Previous Page】
                customButtonNextPage_List_1.Visible = true;//【Next Page】
            }
            else//小于等于一页
            {
                customButtonPreviousPage_List_1.Visible = false;//【Previous Page】
                customButtonNextPage_List_1.Visible = false;//【Next Page】
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：设置列表2数据
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetPage_2()
        {
            customList_2._SetPage();//设置列表项数据

            if (1 < customList_2.TotalPage)//多于一页
            {
                customButtonPreviousPage_List_2.Visible = true;//【Previous Page】
                customButtonNextPage_List_2.Visible = true;//【Next Page】
            }
            else//小于等于一页
            {
                customButtonPreviousPage_List_2.Visible = false;//【Previous Page】
                customButtonNextPage_List_2.Visible = false;//【Next Page】
            }
        }

        //事件

        //----------------------------------------------------------------------
        // 功能说明：【SEARCH】按钮点击事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonSearch_CustomButton_Click(object sender, EventArgs e)
        {
            bClickSearchButton = !bClickSearchButton;

            if (bClickSearchButton)//【SEARCH】按下
            {
                customButtonDateTime.Visible = true;
            }
            else//【SEARCH】弹起
            {
                customButtonDateTime.Visible = false;
            }

            //

            _GetSelectedListItemIndex();

            //

            _SetList_2();

            _SetDeleteButton();
        }

        //----------------------------------------------------------------------
        // 功能说明：【DATE TIME】按钮点击事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonDateTime_CustomButton_Click(object sender, EventArgs e)
        {
            VisionSystemClassLibrary.Struct.SYSTEMTIME systemtime = new VisionSystemClassLibrary.Struct.SYSTEMTIME();
            systemtime.Year = (UInt16)SearchDateTime.Year;
            systemtime.Month = (UInt16)SearchDateTime.Month;
            systemtime.Day = (UInt16)SearchDateTime.Day;
            systemtime.Hour = (UInt16)SearchDateTime.Hour;
            systemtime.Minute = (UInt16)SearchDateTime.Minute;
            systemtime.Second = (UInt16)SearchDateTime.Second;

            GlobalWindows.DateTimePanel_Window.WindowParameter = 3;//窗口特征数值
            GlobalWindows.DateTimePanel_Window.DateTimePanelControl.Language = language;//语言
            GlobalWindows.DateTimePanel_Window.DateTimePanelControl.Chinese_Caption = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][10];//中文标题文本
            GlobalWindows.DateTimePanel_Window.DateTimePanelControl.English_Caption = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][10];//英文标题文本
            GlobalWindows.DateTimePanel_Window.DateTimePanelControl.PanelType = DateTimePanelType.StatisticsTimeSearch_2;//日期时间设置面板类型
            GlobalWindows.DateTimePanel_Window.DateTimePanelControl.DisplayTime_1 = systemtime;//日期时间

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

        //

        //----------------------------------------------------------------------
        // 功能说明：【DELETE】按钮点击事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonDelete_CustomButton_Click(object sender, EventArgs e)
        {
            bClickDeleteButton = true;//【DELETE】

            //显示信息对话框

            GlobalWindows.MessageDisplay_Window.WindowParameter = 83;//窗口特征数值
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
        // 功能说明：【DELETE ALL】按钮点击事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonDeleteAll_CustomButton_Click(object sender, EventArgs e)
        {
            bClickDeleteButton = false;//【DELETE ALL】

            //显示信息对话框

            GlobalWindows.MessageDisplay_Window.WindowParameter = 83;//窗口特征数值
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.OkCancel;//包含【确定】和【取消】按钮
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][1] + "？";
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][1] + "？";

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
        // 功能说明：列表1【Previous Page】按钮点击事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonPreviousPage_List_1_CustomButton_Click(object sender, EventArgs e)
        {
            customList_1._ClickPage(true);//翻页，上一页
        }

        //----------------------------------------------------------------------
        // 功能说明：列表1【Next Page】按钮点击事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonNextPage_List_1_CustomButton_Click(object sender, EventArgs e)
        {
            customList_1._ClickPage(false);//翻页，下一页
        }

        //----------------------------------------------------------------------
        // 功能说明：列表2【Previous Page】按钮点击事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonPreviousPage_List_2_CustomButton_Click(object sender, EventArgs e)
        {
            customList_2._ClickPage(true);//翻页，上一页
        }

        //----------------------------------------------------------------------
        // 功能说明：列表2【Next Page】按钮点击事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonNextPage_List_2_CustomButton_Click(object sender, EventArgs e)
        {
            customList_2._ClickPage(false);//翻页，下一页
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
            if (0 <= customList_2.CurrentListIndex)//有效
            {
                //显示信息对话框

                GlobalWindows.MessageDisplay_Window.WindowParameter = 85;//窗口特征数值
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.OkCancel;//包含【确定】和【取消】按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][6];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][6];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = _GetSelectedShiftRecordString(VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese, customList_1.CurrentListIndex, customList_2.CurrentDataIndex) + "？";
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = _GetSelectedShiftRecordString(VisionSystemClassLibrary.Enum.InterfaceLanguage.English, customList_1.CurrentListIndex, customList_2.CurrentDataIndex) + "？";

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
        // 功能说明：【CANCEL】按钮点击事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonCancel_CustomButton_Click(object sender, EventArgs e)
        {
            bSelectNewRecord = false;

            //事件

            if (null != Close_Click)//有效
            {
                Close_Click(this, new CustomEventArgs());
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：列表1项点击事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customList_1_CustomListItem_Click(object sender, EventArgs e)
        {
            _SetList_2();

            _SetDeleteButton();
        }

        //----------------------------------------------------------------------
        // 功能说明：列表2项点击事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customList_2_CustomListItem_Click(object sender, EventArgs e)
        {
            iSelectedRecordIndex[customList_1.CurrentListIndex] = customList_2.CurrentDataIndex;

            shifttimeSelectedRecord[customList_1.CurrentListIndex].Start = shift.DataOfShift.InformationOfStatistics[customList_1.CurrentListIndex].TimeData[customList_2.CurrentDataIndex].Start;
            shifttimeSelectedRecord[customList_1.CurrentListIndex].End = shift.DataOfShift.InformationOfStatistics[customList_1.CurrentListIndex].TimeData[customList_2.CurrentDataIndex].End;

            //

            _SetDeleteButton();
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：STATISTICS RECORD，【OK】确认，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_StatisticsRecord_Ok_Confirm(object sender, EventArgs e)
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

            if (GlobalWindows.MessageDisplay_Window.MessageDisplayControl.OkCancel)//确认
            {
                if ((0 <= customList_1.CurrentListIndex) && (0 <= customList_2.CurrentDataIndex))//有效
                {
                    if (!bSelectNewRecord)//有效
                    {
                        //if (0 <= shift.DataOfShift.CurrentIndex)//有效
                        //{
                            if ((shift.DataOfShift.CurrentIndex == customList_1.CurrentListIndex) && (VisionSystemClassLibrary.Class.Shift._Compare(shift.DataOfShift.CurrentStatisticsInformation.TimeData[0], shift.DataOfShift.InformationOfStatistics[customList_1.CurrentDataIndex].TimeData[customList_2.CurrentDataIndex])))//相同
                            {
                                bSelectNewRecord = false;
                            }
                            else
                            {
                                shifttimeSelectedRecordTemp.Start = shifttimeSelectedRecord[customList_1.CurrentListIndex].Start;
                                shifttimeSelectedRecordTemp.End = shifttimeSelectedRecord[customList_1.CurrentListIndex].End;

                                bSelectNewRecord = true;
                            }
                        //}
                        //else
                        //{
                        //    bSelectNewRecord = false;
                        //}
                    }
                }
                else
                {
                    bSelectNewRecord = false;
                }

                //事件

                if (null != Close_Click)//有效
                {
                    Close_Click(this, new CustomEventArgs());
                }
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：STATISTICS SEARCH，统计数据查找，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void dateTimePanelWindow_WindowClose_StatisticsSearch(object sender, EventArgs e)
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
                bSetSearchDateTime = true;

                SearchDateTime = new DateTime(GlobalWindows.DateTimePanel_Window.DateTimePanelControl.DisplayTime_1.Year, GlobalWindows.DateTimePanel_Window.DateTimePanelControl.DisplayTime_1.Month, GlobalWindows.DateTimePanel_Window.DateTimePanelControl.DisplayTime_1.Day);

                _SetSearchDateTimeButton();

                //

                _GetSearchRecord();

                _GetSelectedListItemIndex();

                //

                _SetList_2();

                _SetDeleteButton();
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：STATISTICS RECORD，【DELETE】（【DELETE ALL】）确认，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_StatisticsRecord_GetRecord_Wait(object sender, EventArgs e)
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

            if (GlobalWindows.MessageDisplay_Window.MessageDisplayControl.OkCancel)//确认
            {
                bRecordMessageWindowShow = false;

                //

                bSelectNewRecord = false;

                //事件

                if (null != Close_Click)//有效
                {
                    Close_Click(this, new CustomEventArgs());
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：STATISTICS RECORD，【DELETE】（【DELETE ALL】）确认，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_StatisticsRecord_Delete_Confirm(object sender, EventArgs e)
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

            if (GlobalWindows.MessageDisplay_Window.MessageDisplayControl.OkCancel)//确认
            {
                bDeleteRecord = true;

                //

                if (bClickDeleteButton)//【DELETE】
                {
                    iDeleteType = 2;//删除统计数据方式（0，删除所有；1，删除指定班次；2，删除指定记录）

                    //

                    if (VisionSystemClassLibrary.Class.Shift._Compare(shift.DataOfShift.CurrentStatisticsInformation.TimeData[0], shifttimeSelectedRecord[customList_1.CurrentListIndex]))
                    {
                        bSelectNewRecord = true;
                    }
                    else
                    {
                        bSelectNewRecord = false;
                    }   
                }
                else//【DELETE ALL】
                {
                    iDeleteType = 0;//删除统计数据方式（0，删除所有；1，删除指定班次；2，删除指定记录）

                    //

                    bSelectNewRecord = true;
                }

                //显示等待窗口

                bRecordMessageWindowShow = true;

                GlobalWindows.MessageDisplay_Window.WindowParameter = 84;//窗口特征数值
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.None;//不包含任何按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = " ";
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = " ";
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][3] + "...";
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][3] + "...";
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = " ";
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = " ";

                GlobalWindows.MessageDisplay_Window.StartPosition = FormStartPosition.CenterScreen;
                if (GlobalWindows.TopMostWindows)//置顶
                {
                    GlobalWindows.MessageDisplay_Window.TopMost = true;//将窗口置于顶层
                }
                else//其它
                {
                    GlobalWindows.MessageDisplay_Window.Visible = true;//显示
                }

                timerRecord.Start();//启动定时器

                //事件

                if (null != Delete_Click)//有效
                {
                    Delete_Click(this, new CustomEventArgs());
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：STATISTICS RECORD，【DELETE】（【DELETE ALL】）等待，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_StatisticsRecord_Delete_Wait(object sender, EventArgs e)
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

            if (GlobalWindows.MessageDisplay_Window.MessageDisplayControl.OkCancel)//确认
            {
                bDeleteRecord = false;

                bRecordMessageWindowShow = false;

                //

                bSelectNewRecord = false;

                //

                if (!bDeleteRecordSuccess)//失败
                {
                    //事件

                    if (null != Close_Click)//有效
                    {
                        Close_Click(this, new CustomEventArgs());
                    }
                }
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：定时器事件，执行操作时的等待，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void timerRecord_Tick(object sender, EventArgs e)
        {
            if (bRecordMessageWindowShow)//显示
            {
                iTimerRecordCount--;

                if (0 >= iTimerRecordCount)//超时
                {

                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮     
                    if (bDeleteRecord)//删除
                    {
                        bDeleteRecord = false;

                        GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = " ";
                        GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = " ";
                        GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][5];
                        GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][5];
                        GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = " ";
                        GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = " ";
                    }
                    else
                    {
                        GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = " ";
                        GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = " ";
                        GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][8];
                        GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][8];
                        GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = " ";
                        GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = " ";
                    }

                    timerRecord.Stop();//关闭定时器

                    iTimerRecordCount = iTimerRecordMaxCount;
                }
                else//计数
                {
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][9] + "，" + iTimerRecordCount.ToString();
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][9] + "，" + iTimerRecordCount.ToString();
                }
            }
        }
    }
}
