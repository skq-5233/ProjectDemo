/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：StatisticsControl.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：统计窗口

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
using System.Diagnostics;

using System.Threading;

using System.Runtime.InteropServices;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;

using System.Reflection;

namespace VisionSystemControlLibrary
{
    public partial class StatisticsControl : UserControl
    {
        //STATISTICS控件

        private VisionSystemClassLibrary.Enum.InterfaceLanguage language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;//属性，语言

        private VisionSystemClassLibrary.Enum.DeviceState devicestate = VisionSystemClassLibrary.Enum.DeviceState.Stop;//属性，设备状态

        //

        private VisionSystemClassLibrary.Class.Shift shift = new VisionSystemClassLibrary.Class.Shift();//属性，班次

        private VisionSystemClassLibrary.Class.Camera camera = new VisionSystemClassLibrary.Class.Camera();//属性，相机

        //

        private VisionSystemClassLibrary.Class.System parameter = new VisionSystemClassLibrary.Class.System();//属性（只读），设备（主要使用其中的系统参数数据）

        //

        private VisionSystemClassLibrary.Enum.CameraType cameratypeSelected = VisionSystemClassLibrary.Enum.CameraType.None;//属性（只读），点击列表项时，记录选择的相机类型（防止列表被更新）

        //

        private Boolean bGetStatisticsMessageWindowShow = false;//属性（只读），是否显示正在获取记录时的提示信息窗口。取值范围：true，是；false，否

        private const Int32 iTimerGetStatisticsMaxCount = 30;//定时器时间
        private Int32 iTimerGetStatisticsCount = 30;//定时器时间

        //

        private Int32 iCurrentRejectImageIndex = -1;//当前选择查看的剔除图像索引值（从0开始）

        //

        private Int32 iRejectImageMaxNumber = 0;//剔除图像总数

        //

        private bool bRelevancy = false;//属性，【RELEVANCY】按钮状态。true：按钮按下；false：按钮未按下

        //

        private Boolean bClickSelectAllButton = true;//是否按下【SELECT ALL】按钮。取值范围：true，是；false，否

        private Boolean bClickViewRejectButton = false;//是否按下【VIEW REJECT】按钮。取值范围：true，是；false，否

        private Boolean bClickParameterButton = false;//是否按下【PARAMETER】按钮。取值范围：true，是；false，否

        private Boolean bClickStatusBarButton = false;//是否按下【STATUS BAR】按钮。取值范围：true，是；false，否

        //

        private Boolean bCurrentShift = false;//是否为当前班次。取值范围：true，是；false，否

        //

        private Boolean bStatisticsRecordWindowDisplay = false;//属性（只读），统计记录窗口是否打开。取值范围：true，是；false，否

        //

        private Int32[][] iToolParameters = new Int32[10][];//工具参数（[工具索引值（从0开始）][参数索引值（从0开始）]）
        private Int32[] iToolParameterNumber = new Int32[10];//工具参数数目（[工具索引值（从0开始）]）

        //

        private Bitmap bitmapNone = null;//无效图像

        //

        private String[][] sMessageText = new String[2][];//提示信息对话框、列表中显示的文本（[语言][包含的文本]）
        private String[][] sMessageText_1 = new String[2][];//控件上显示的文本（[语言][包含的文本]）

        //

        [Browsable(true), Description("窗口关闭时产生的事件"), Category("StatisticsRecord 事件")]
        public event EventHandler Close_Click;//窗口关闭时产生的事件

        [Browsable(true), Description("选择统计记录时产生的事件"), Category("StatisticsRecord 事件")]
        public event EventHandler GetRecordData;//选择统计记录时产生的事件

        [Browsable(true), Description("获取统计记录时产生的事件"), Category("StatisticsRecord 事件")]
        public event EventHandler GetRecords;//获取统计记录时产生的事件

        [Browsable(true), Description("删除统计记录时产生的事件"), Category("StatisticsRecord 事件")]
        public event EventHandler DeleteRecords;//窗口关闭时产生的事件

        [Browsable(true), Description("查看剔除图像时产生的事件"), Category("StatisticsRecord 事件")]
        public event EventHandler ViewRejectImage;//查看剔除图像时产生的事件
        
        [Browsable(true), Description("点击【RELEVANCE】按钮时产生的事件"), Category("StatisticsRecord 事件")]
        public event EventHandler Relevancy_Click;//点击【RELEVANCE】按钮时产生的事件

        [Browsable(true), Description("查看关联剔除图像时产生的事件"), Category("StatisticsRecord 事件")]
        public event EventHandler ViewRejectImage_Relevancy;//查看关联剔除图像时产生的事件

        //

        //构造函数

        //----------------------------------------------------------------------
        // 功能说明：构造函数，初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public StatisticsControl()
        {
            InitializeComponent();

            //

            if (null != GlobalWindows.StatisticsRecord_Window)
            {
                GlobalWindows.StatisticsRecord_Window.WindowClose += new System.EventHandler(statisticsRecordWindow_WindowClose);//订阅事件
                GlobalWindows.StatisticsRecord_Window.DeleteRecords += new System.EventHandler(statisticsRecordWindow_DeleteRecords);//订阅事件
            }

            if (null != GlobalWindows.MessageDisplay_Window)
            {
                GlobalWindows.MessageDisplay_Window.WindowClose_Statistics_GetRecordData_Wait += new System.EventHandler(messageDisplayWindow_WindowClose_Statistics_GetRecordData_Wait);//订阅事件
            }

            if (null != GlobalWindows.DigitalKeyboard_Window)
            {
                GlobalWindows.DigitalKeyboard_Window.WindowClose_Statistics_RejectImageSelection += new System.EventHandler(digitalKeyboardWindow_WindowClose_Statistics_RejectImageSelection);//订阅事件
            }

            //

            Int32 i = 0;//循环控制变量
            Int32 j = 0;//循环控制变量

            for (i = 0; i < iToolParameters.Length; i++)
            {
                iToolParameters[i] = new Int32[5];

                for (j = 0; j < iToolParameters[i].Length; j++)
                {
                    iToolParameters[i][j] = 0;
                }

                //

                iToolParameterNumber[i] = 0;//工具参数数目（[工具索引值（从0开始）]）
            }

            //

            bitmapNone = new Bitmap(imageDisplayView.Width, imageDisplayView.Height);//无效图像

            //

            FieldInfo[] fieldinfo = VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese.GetType().GetFields();//获取公共字段
            if (null != fieldinfo && 1 < fieldinfo.Length)//有效
            {
                sMessageText = new String[fieldinfo.Length - 1][];
                for (i = 0; i < fieldinfo.Length - 1; i++)
                {
                    sMessageText[i] = new String[7];
                    sMessageText_1[i] = new String[1];
                }

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] = "正在获取统计数据";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] = "Getting Statistics Records";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][1] = "正在更新统计数据";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][1] = "Updating Statistics Records";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][2] = "获取统计数据失败";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][2] = "Getting Statistics Records Failed";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][3] = "开启";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][3] = "ON";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][4] = "关闭";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][4] = "OFF";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][5] = "剔除图像";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][5] = "REJECT IMAGE";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][6] = "请等待";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][6] = "Please wait";

                //

                sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] = customButtonCaption.Chinese_TextDisplay[0];
                sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] = customButtonCaption.English_TextDisplay[0];
            }

            //

            customButtonShiftValue_1.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonShiftValue_1.English_TextDisplay = new String[1] { " " };//设置显示的文本

            customButtonShiftTimeValue.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonShiftTimeValue.English_TextDisplay = new String[1] { " " };//设置显示的文本

            customButtonBrandValue.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonBrandValue.English_TextDisplay = new String[1] { " " };//设置显示的文本

            customButtonDurationTimeValue.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonDurationTimeValue.English_TextDisplay = new String[1] { " " };//设置显示的文本

            customButtonInspectedValue.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonInspectedValue.English_TextDisplay = new String[1] { " " };//设置显示的文本

            customButtonRejectedValue.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonRejectedValue.English_TextDisplay = new String[1] { " " };//设置显示的文本
        }

        //属性

        //-----------------------------------------------------------------------
        // 功能说明：Language属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("语言"), Category("StatisticsControl 通用")]
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
        [Browsable(true), Description("设备状态"), Category("StatisticsControl 通用")]
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
        // 功能说明：Relevancy属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("【Relevancy】按钮状态。true：按钮按下；false：按钮未按下"), Category("StatisticsControl 通用")]
        public bool Relevancy //属性
        {
            get//读取
            {
                return bRelevancy;
            }
            set//设置
            {
                if (bRelevancy != value)
                {
                    bRelevancy = value;

                    //

                    if (bRelevancy)//【Relevancy】按下
                    {
                        customButtonRelevancy.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
                    }
                    else//【Relevancy】未按下
                    {
                        customButtonRelevancy.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                    }
                }
            }
        }

                //

        //----------------------------------------------------------------------
        // 功能说明：bClickViewRejectButton属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("是否查询缺陷图像"), Category("StatisticsControl 通用")]
        public bool ViewRejectButton //属性
        {
            get//读取
            {
                return bClickViewRejectButton;
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：IsCurrentShift属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("是否当前班次"), Category("StatisticsControl 通用")]
        public bool IsCurrentShift //属性
        {
            get//读取
            {
                return bCurrentShift;
            }
        }
       
        //

        //-----------------------------------------------------------------------
        // 功能说明：SystemShift属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("班次"), Category("StatisticsControl 通用")]
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
        [Browsable(true), Description("相机"), Category("StatisticsControl 通用")]
        public VisionSystemClassLibrary.Class.Camera SelectedCamera
        {
            get//读取
            {
                return camera;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ViewRejectImage_CameraSelected属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("缺陷图像查询相机"), Category("StatisticsControl 通用")]
        public VisionSystemClassLibrary.Enum.CameraType ViewRejectImage_CameraSelected //属性
        {
            get//读取
            {
                return cameratypeSelected;
            }
        }


        //

        //-----------------------------------------------------------------------
        // 功能说明：GetStatisticsMessageWindowShow属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("是否显示正在获取记录时的提示信息窗口。取值范围：true，是；false，否"), Category("StatisticsControl 通用")]
        public Boolean GetStatisticsMessageWindowShow
        {
            get//读取
            {
                return bGetStatisticsMessageWindowShow;
            }
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：CurrentToolIndex_RejectImage属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("当前选择查看的剔除图像所对应的工具索引值（从0开始）"), Category("StatisticsControl 通用")]
        public Int32 CurrentToolIndex_RejectImage
        {
            get//读取
            {
                if (bClickSelectAllButton)//选择所有
                {
                    return -1;
                } 
                else//选择当前
                {
                    return customListRejects.CurrentListIndex;
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：CurrentRejectImageIndex_Tool属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("当前选择查看的剔除图像在其对应的工具中的索引值（从0开始）"), Category("StatisticsControl 通用")]
        public Int32 CurrentRejectImageIndex_Tool
        {
            get//读取
            {
                return iCurrentRejectImageIndex;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：StatisticsRecordWindowDisplay属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("统计记录窗口是否打开。取值范围：true，是；false，否"), Category("StatisticsControl 通用")]
        public Boolean StatisticsRecordWindowDisplay
        {
            get//读取
            {
                return bStatisticsRecordWindowDisplay;
            }
        }

        //函数

        //----------------------------------------------------------------------
        // 功能说明：应用设置的属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _Properties(VisionSystemClassLibrary.Class.System system_parameter, VisionSystemClassLibrary.Class.Camera camera_parameter, VisionSystemClassLibrary.Class.Shift shift_parameter)
        {
            parameter = system_parameter;

            shift = shift_parameter;

            camera = camera_parameter;

            cameratypeSelected = camera.Type;

            //

            customButtonCaption.Chinese_TextDisplay = new String[1] { camera.CameraCHNName + sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] };//设置显示的文本
            customButtonCaption.English_TextDisplay = new String[1] { camera.CameraENGName + sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] };//设置显示的文本

            if (VisionSystemClassLibrary.Enum.RelevancyType.None < camera.RelevancyCameraInfo.rRelevancyType) //当前相机有关联
            {
                customButtonRelevancy.Visible = true;
            }
            else //相机没有关联信息
            {
                customButtonRelevancy.Visible = false;
            }

            customButton_CameraName.Visible = false;
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：启动获取统计数据
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _StartGetStatisticsData()
        {
            bGetStatisticsMessageWindowShow = true;

            //显示信息对话框

            GlobalWindows.MessageDisplay_Window.WindowParameter = 86;//窗口特征数值
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.None;//不包含任何按钮
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] + "...";
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] + "...";
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = " ";
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = " ";
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][6] + "，" + iTimerGetStatisticsCount.ToString();
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][6] + "，" + iTimerGetStatisticsCount.ToString();

            GlobalWindows.MessageDisplay_Window.StartPosition = FormStartPosition.CenterScreen;
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.MessageDisplay_Window.TopMost = true;//将窗口置于顶层
            }
            else//其它
            {
                GlobalWindows.MessageDisplay_Window.Visible = true;//显示
            }

            timerStatistics.Start();//启动定时器
        }

        //----------------------------------------------------------------------
        // 功能说明：获取统计数据
        // 输入参数：1.bSuccess：操作是否成功。取值范围：true，是；false，否
        //         2.iStatisticsDataType：统计数据类型（0，当前班；1，历史班）
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _GetStatisticsData(Boolean bSuccess, Int32 iStatisticsDataType)
        {
            bGetStatisticsMessageWindowShow = false;

            iTimerGetStatisticsCount = iTimerGetStatisticsMaxCount;

            timerStatistics.Stop();//关闭定时器

            //

            if (bSuccess)//成功
            {
                bCurrentShift = !(Convert.ToBoolean(iStatisticsDataType));

                //

                _GetCurrentImageIndex();

                //

                _SetData();

                //

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
            else//失败
            {
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = " ";
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = " ";
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][2];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][2];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = " ";
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = " ";
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：更新统计数据
        // 输入参数：1.iStatisticsDataType：统计数据类型（0，当前班；1，历史班）
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _UpdateStatisticsData(Int32 iStatisticsDataType)
        {
            bCurrentShift = !(Convert.ToBoolean(iStatisticsDataType));

            //

            //_GetCurrentImageIndex();

            //

            _SetInformation();

            _SetViewReject();

            //_SetImageView();

            Int32 i = 0;//循环控制变量

            for (i = 0; i < customListRejects.ItemDataNumber; i++)//列表项数据
            {
                _SetRejectsListItem_2(i);//设置REJECTS列表的数值项
            }
            customListRejects._Refresh();
        }

        //-----------------------------------------------------------------------
        // 功能说明：设置图像
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        public void _SetImageView()
        {
            if (bClickViewRejectButton)//【VIEW REJECT】按下
            {
                imageDisplayView.Information = VisionSystemClassLibrary.Struct.StatisticsData.GraphicsInformation;

                if (null != VisionSystemClassLibrary.Struct.StatisticsData.ImageReject)//有效
                {
                    if (imageDisplayView.ControlSize.Width <= VisionSystemClassLibrary.Struct.StatisticsData.ImageReject.Width && imageDisplayView.ControlSize.Height <= VisionSystemClassLibrary.Struct.StatisticsData.ImageReject.Height)//有效
                    {
                        imageDisplayView.ShowTitle = !bClickStatusBarButton;

                        imageDisplayView.BitmapDisplay = VisionSystemClassLibrary.Struct.StatisticsData.ImageReject.ToBitmap();
                    }
                }
                else//无效
                {
                    imageDisplayView.ShowTitle = false;

                    imageDisplayView.BitmapDisplay = (Bitmap)bitmapNone.Clone();//图像数据
                }
            }
            else//【VIEW REJECT】弹起
            {
                imageDisplayView.Information = camera.Learn;

                if (null != camera.ImageLearn)//有效
                {
                    if (imageDisplayView.ControlSize.Width <= camera.ImageLearn.Width && imageDisplayView.ControlSize.Height <= camera.ImageLearn.Height)//有效
                    {
                        Image<Bgr, Byte> imageShow = camera.ImageLearn.Copy();

                        if (bClickSelectAllButton)//选择所有
                        {
                            Int32 i = 0;//循环控制变量

                            for (i = 0; i < camera.Tools.Count; i++)
                            {
                                if (camera.Tools[i].ToolState)//启用
                                {
                                    VisionSystemClassLibrary.GeneralFunction._DrawGraphics(ref imageShow, camera.Tools[i].ROI, new Bgr(0, 255, 0), 1);
                                }
                            }
                        } 
                        else//选择当前
                        {
                            VisionSystemClassLibrary.GeneralFunction._DrawGraphics(ref imageShow, camera.Tools[customListRejects.CurrentListIndex].ROI, new Bgr(0, 255, 0), 1);
                        }

                        imageDisplayView.BitmapDisplay = imageShow.ToBitmap();
                    }
                }
                else//无效
                {
                    imageDisplayView.BitmapDisplay = (Bitmap)bitmapNone.Clone();//图像数据
                }
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：获取当前图像索引值
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _GetCurrentImageIndex()
        {
            Int32 i = 0;//循环控制变量

            //Int32 iValue = 0;//临时变量

            if (bClickSelectAllButton)//选择所有
            {
                //if (null != shift.DataOfShift.CurrentStatisticsInformation.DataOfStatistics[0].CameraStatisticsData[0].RejectedStatistics_Tool)//有效
                //{
                //    for (i = 0; i < shift.DataOfShift.CurrentStatisticsInformation.DataOfStatistics[0].CameraStatisticsData[0].RejectedStatistics_Tool.Length; i++)
                //    {
                //        iValue += Convert.ToInt32(shift.DataOfShift.CurrentStatisticsInformation.DataOfStatistics[0].CameraStatisticsData[0].RejectedStatistics_Tool[i]);
                //    }
                //}

                iRejectImageMaxNumber = Convert.ToInt32(shift.DataOfShift.CurrentStatisticsInformation.DataOfStatistics[0].CameraStatisticsData[0].RejectedNumber);

                //if (0 > iCurrentRejectImageIndex || iCurrentRejectImageIndex >= iValue)//超出范围
                //{
                //    iCurrentRejectImageIndex = iValue - 1;

                //    customButtonSelection.Chinese_TextDisplay = new String[1] { (iCurrentRejectImageIndex + 1).ToString() };//设置显示的文本
                //    customButtonSelection.English_TextDisplay = new String[1] { (iCurrentRejectImageIndex + 1).ToString() };//设置显示的文本
                //}

                //if (iCurrentRejectImageIndex < iValue)//未超出范围
                //{
                iCurrentRejectImageIndex = iRejectImageMaxNumber - 1;

                    customButtonSelection.Chinese_TextDisplay = new String[1] { (iCurrentRejectImageIndex + 1).ToString() };//设置显示的文本
                    customButtonSelection.English_TextDisplay = new String[1] { (iCurrentRejectImageIndex + 1).ToString() };//设置显示的文本
                //}
                //else
                //{
                //    iCurrentRejectImageIndex = -1;
                //}
            }
            else//选择当前
            {
                iRejectImageMaxNumber = Convert.ToInt32(shift.DataOfShift.CurrentStatisticsInformation.DataOfStatistics[0].CameraStatisticsData[0].RejectedStatistics_Tool[customListRejects.CurrentListIndex]);

                //if (0 > iCurrentRejectImageIndex || iCurrentRejectImageIndex >= iRejectImageMaxNumber)//超出范围
                //{
                //    iCurrentRejectImageIndex = iRejectImageMaxNumber - 1;

                //    customButtonSelection.Chinese_TextDisplay = new String[1] { (iCurrentRejectImageIndex + 1).ToString() };//设置显示的文本
                //    customButtonSelection.English_TextDisplay = new String[1] { (iCurrentRejectImageIndex + 1).ToString() };//设置显示的文本
                //}

                //if (iCurrentRejectImageIndex < iRejectImageMaxNumber)//未超出范围
                //{
                    iCurrentRejectImageIndex = iRejectImageMaxNumber - 1;

                    customButtonSelection.Chinese_TextDisplay = new String[1] { (iCurrentRejectImageIndex + 1).ToString() };//设置显示的文本
                    customButtonSelection.English_TextDisplay = new String[1] { (iCurrentRejectImageIndex + 1).ToString() };//设置显示的文本
                //}
                //else
                //{
                //    iCurrentRejectImageIndex = -1;
                //}
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：设置页面数据
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetData()
        {
            _SetInformation();

            //

            _InitRejectsList();
            _SetRejectsList();
            _SetPage_Rejects();

            _SetViewReject();

            _SetImageView();

            customButtonSelection.Chinese_TextDisplay = new String[1] { (iCurrentRejectImageIndex + 1).ToString() };//设置显示的文本
            customButtonSelection.English_TextDisplay = new String[1] { (iCurrentRejectImageIndex + 1).ToString() };//设置显示的文本

            //

            _GetToolParameters();
            _SetToolParameters();

            if (bClickParameterButton)//【PARAMETER】按下
            {
                parameterSettingsPanel.Visible = true;
            }
            else//【PARAMETER】未按下
            {
                parameterSettingsPanel.Visible = false;
            }
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：设置页面统计总揽信息1
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _SetInformation_1()
        {
            if (0 <= shift.DataOfShift.CurrentIndex)//有效班
            {
                string sRejectedRate = 0.0.ToString("F2") + "%";

                if (shift.DataOfShift.CurrentStatisticsInformation.DataOfStatistics[0].CameraStatisticsData[0].InspectedNumber > 0) //检测数量有效
                {
                    sRejectedRate = ((Double)shift.DataOfShift.CurrentStatisticsInformation.DataOfStatistics[0].CameraStatisticsData[0].RejectedNumber * 100.0 / shift.DataOfShift.CurrentStatisticsInformation.DataOfStatistics[0].CameraStatisticsData[0].InspectedNumber).ToString("F2") + "%";
                }
                customButtonDurationTimeValue.Chinese_TextDisplay = new String[1] { sRejectedRate };//设置显示的文本
                customButtonDurationTimeValue.English_TextDisplay = new String[1] { sRejectedRate };//设置显示的文本

                customButtonInspectedValue.Chinese_TextDisplay = new String[1] { shift.DataOfShift.CurrentStatisticsInformation.DataOfStatistics[0].CameraStatisticsData[0].InspectedNumber.ToString() };//设置显示的文本
                customButtonInspectedValue.English_TextDisplay = new String[1] { shift.DataOfShift.CurrentStatisticsInformation.DataOfStatistics[0].CameraStatisticsData[0].InspectedNumber.ToString() };//设置显示的文本

                customButtonRejectedValue.Chinese_TextDisplay = new String[1] { shift.DataOfShift.CurrentStatisticsInformation.DataOfStatistics[0].CameraStatisticsData[0].RejectedNumber.ToString() };//设置显示的文本
                customButtonRejectedValue.English_TextDisplay = new String[1] { shift.DataOfShift.CurrentStatisticsInformation.DataOfStatistics[0].CameraStatisticsData[0].RejectedNumber.ToString() };//设置显示的文本
            }
            else//无效班
            {
                customButtonDurationTimeValue.Chinese_TextDisplay = new String[1] { "--" };//设置显示的文本
                customButtonDurationTimeValue.English_TextDisplay = new String[1] { "--" };//设置显示的文本

                customButtonInspectedValue.Chinese_TextDisplay = new String[1] { "--" };//设置显示的文本
                customButtonInspectedValue.English_TextDisplay = new String[1] { "--" };//设置显示的文本

                customButtonRejectedValue.Chinese_TextDisplay = new String[1] { "--" };//设置显示的文本
                customButtonRejectedValue.English_TextDisplay = new String[1] { "--" };//设置显示的文本
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：设置页面统计总揽信息
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _SetInformation()
        {
            if (bCurrentShift)//当前班
            {
                customButtonShiftValue_2.CurrentTextGroupIndex = 0;
            }
            else//历史班
            {
                customButtonShiftValue_2.CurrentTextGroupIndex = 1;
            }

            if (0 <= shift.DataOfShift.CurrentIndex)//有效班
            {
                customButtonShiftValue_1.Chinese_TextDisplay = new String[1] { (shift.DataOfShift.CurrentIndex + 1).ToString() };//设置显示的文本
                customButtonShiftValue_1.English_TextDisplay = new String[1] { (shift.DataOfShift.CurrentIndex + 1).ToString() };//设置显示的文本
            } 
            else//无效班
            {
                customButtonShiftValue_1.Chinese_TextDisplay = new String[1] { "--" };//设置显示的文本
                customButtonShiftValue_1.English_TextDisplay = new String[1] { "--" };//设置显示的文本
            }

            customButtonShiftTimeValue.Chinese_TextDisplay = new String[1] { VisionSystemClassLibrary.Class.Shift._GetDateTime(shift.DataOfShift.CurrentStatisticsInformation.TimeData[0].Start, shift.DataOfShift.CurrentStatisticsInformation.TimeData[0].End) };//设置显示的文本
            customButtonShiftTimeValue.English_TextDisplay = new String[1] { VisionSystemClassLibrary.Class.Shift._GetDateTime(shift.DataOfShift.CurrentStatisticsInformation.TimeData[0].Start, shift.DataOfShift.CurrentStatisticsInformation.TimeData[0].End) };//设置显示的文本

            customButtonBrandValue.Chinese_TextDisplay = new String[1] { shift.DataOfShift.CurrentStatisticsInformation.DataOfStatistics[0].BrandName };//设置显示的文本
            customButtonBrandValue.English_TextDisplay = new String[1] { shift.DataOfShift.CurrentStatisticsInformation.DataOfStatistics[0].BrandName };//设置显示的文本

            //

            _SetInformation_1();
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：点击【REJECTS】按钮后，设置相关按钮状态
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _SetViewReject()
        {
            if (0 <= shift.DataOfShift.CurrentIndex)//有效班次
            {
                if (!customButtonViewReject.Visible)//隐藏
                {
                    customButtonViewReject.Visible = true;
                } 

                //

                if (bClickViewRejectButton)//【VIEW REJECT】按下
                {
                    if (0 <= iCurrentRejectImageIndex)//存在
                    {
                        customButtonSubtract.Visible = true;
                        customButtonSelection.Visible = true;
                        customButtonPlus.Visible = true;
                        //customButtonStatusBar.Visible = true;
                    }
                    else//不存在
                    {
                        customButtonSubtract.Visible = false;
                        customButtonSelection.Visible = false;
                        customButtonPlus.Visible = false;
                        //customButtonStatusBar.Visible = false;
                    }
                }
                else//【VIEW REJECT】弹起
                {
                    imageDisplayView.ShowTitle = false;

                    //

                    customButtonSubtract.Visible = false;
                    customButtonSelection.Visible = false;
                    customButtonPlus.Visible = false;
                    //customButtonStatusBar.Visible = false;
                }
            } 
            else//无效班次
            {
                if (customButtonViewReject.Visible)//显示
                {
                    customButtonViewReject.Visible = false;

                    customButtonSubtract.Visible = false;
                    customButtonSelection.Visible = false;
                    customButtonPlus.Visible = false;
                    //customButtonStatusBar.Visible = false;
                } 
            }
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：获取工具参数
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _GetToolParameters()
        {
            Int32 i = 0;//循环控制变量
            Int32 j = 0;//循环控制变量

            //初始化

            iToolParameters = new Int32[camera.Tools.Count][];
            iToolParameterNumber = new Int32[camera.Tools.Count];

            for (i = 0; i < iToolParameters.Length; i++)
            {
                iToolParameters[i] = new Int32[camera.Tools[i].Arithmetic.Number];

                for (j = 0; j < iToolParameters[i].Length; j++)
                {
                    iToolParameters[i][j] = 0;
                }

                //

                iToolParameterNumber[i] = 0;//工具参数数目（[工具索引值（从0开始）]）
            }

            //获取数值

            for (i = 0; i < camera.Tools.Count; i++)//工具
            {
                iToolParameterNumber[i] = 0;

                for (j = 0; j < camera.Tools[i].Arithmetic.Number; j++)//工具参数
                {
                    if (camera.Tools[i].Arithmetic.State[j])//算法使能
                    {
                        iToolParameters[i][iToolParameterNumber[i]] = j;

                        iToolParameterNumber[i]++;
                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：设置工具参数
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _SetToolParameters()
        {
            try
            {
                Int32 i = 0;//循环控制变量
                Int32 j = 0;//循环控制变量

                Int32 iCurrentToolIndex = customListRejects.CurrentListIndex;

                parameterSettingsPanel.ParameterType = new Int32[iToolParameterNumber[iCurrentToolIndex]];//参数类型
                parameterSettingsPanel._SetParameterValue(new Int32[iToolParameterNumber[iCurrentToolIndex]][]);//参数数值（参数类型取值为1时有效）
                parameterSettingsPanel._SetParameterValueEnabled(new Boolean[iToolParameterNumber[iCurrentToolIndex]][]);//参数数值使能情况。取值范围：true，使能；false，禁止（参数类型取值为1时有效）
                parameterSettingsPanel.Chinese_ParameterName = new String[iToolParameterNumber[iCurrentToolIndex]];//参数中文名称
                parameterSettingsPanel.English_ParameterName = new String[iToolParameterNumber[iCurrentToolIndex]];//参数英文名称
                parameterSettingsPanel.Chinese_ParameterValueNameDisplay = new String[iToolParameterNumber[iCurrentToolIndex]];//原始参数数值中文名称（参数类型取值为1时有效）
                parameterSettingsPanel.English_ParameterValueNameDisplay = new String[iToolParameterNumber[iCurrentToolIndex]];//原始参数数值英文名称（参数类型取值为1时有效）
                parameterSettingsPanel.ParameterCurrentValue = new Single[iToolParameterNumber[iCurrentToolIndex]];//当前值
                parameterSettingsPanel.ParameterMinValue = new Single[iToolParameterNumber[iCurrentToolIndex]];//最小值（参数类型取值为2时有效）
                parameterSettingsPanel.ParameterMaxValue = new Single[iToolParameterNumber[iCurrentToolIndex]];//最大值（参数类型取值为2时有效）
                parameterSettingsPanel.ParameterEnabled = new Boolean[iToolParameterNumber[iCurrentToolIndex]];//参数使能状态

                for (i = 0; i < iToolParameterNumber[iCurrentToolIndex]; i++)//工具参数
                {
                    parameterSettingsPanel.ParameterType[i] = camera.Tools[iCurrentToolIndex].Arithmetic.Type[iToolParameters[iCurrentToolIndex][i]];

                    parameterSettingsPanel.Chinese_ParameterName[i] = camera.Tools[iCurrentToolIndex].Arithmetic.CHNName[iToolParameters[iCurrentToolIndex][i]];
                    parameterSettingsPanel.English_ParameterName[i] = camera.Tools[iCurrentToolIndex].Arithmetic.ENGName[iToolParameters[iCurrentToolIndex][i]];

                    if (1 == camera.Tools[iCurrentToolIndex].Arithmetic.Type[iToolParameters[iCurrentToolIndex][i]])//枚举类型
                    {
                        parameterSettingsPanel.ParameterCurrentValue[i] = camera.Tools[iCurrentToolIndex].Arithmetic.EnumCurrent[iToolParameters[iCurrentToolIndex][i]];

                        parameterSettingsPanel.ParameterValue[i] = new Int32[camera.Tools[iCurrentToolIndex].Arithmetic.EnumNumber[iToolParameters[iCurrentToolIndex][i]]];//参数数值（参数类型取值为1时有效）
                        parameterSettingsPanel.ParameterValueEnabled[i] = new Boolean[camera.Tools[iCurrentToolIndex].Arithmetic.EnumNumber[iToolParameters[iCurrentToolIndex][i]]];//参数数值使能情况。取值范围：true，使能；false，禁止（参数类型取值为1时有效）

                        parameterSettingsPanel.Chinese_ParameterValueNameDisplay[i] = "";
                        parameterSettingsPanel.English_ParameterValueNameDisplay[i] = "";
                        for (j = 0; j < camera.Tools[iCurrentToolIndex].Arithmetic.EnumNumber[iToolParameters[iCurrentToolIndex][i]] - 1; j++)
                        {
                            parameterSettingsPanel.ParameterValue[i][j] = j;//参数数值（参数类型取值为1时有效）
                            parameterSettingsPanel.ParameterValueEnabled[i][j] = camera.Tools[iCurrentToolIndex].Arithmetic.EnumState[iToolParameters[iCurrentToolIndex][i], j];//参数数值使能情况。取值范围：true，使能；false，禁止（参数类型取值为1时有效）

                            parameterSettingsPanel.Chinese_ParameterValueNameDisplay[i] += VisionSystemClassLibrary.String.TextData.EnumArithmeticString_CHN[camera.Tools[iCurrentToolIndex].Arithmetic.EnumType[iToolParameters[iCurrentToolIndex][i]]][j] + "&";
                            parameterSettingsPanel.English_ParameterValueNameDisplay[i] += VisionSystemClassLibrary.String.TextData.EnumArithmeticString_ENG[camera.Tools[iCurrentToolIndex].Arithmetic.EnumType[iToolParameters[iCurrentToolIndex][i]]][j] + "&";
                        }

                        parameterSettingsPanel.ParameterValue[i][j] = j;//参数数值（参数类型取值为1时有效）
                        parameterSettingsPanel.ParameterValueEnabled[i][j] = camera.Tools[iCurrentToolIndex].Arithmetic.EnumState[iToolParameters[iCurrentToolIndex][i], j];//参数数值使能情况。取值范围：true，使能；false，禁止（参数类型取值为1时有效）

                        parameterSettingsPanel.Chinese_ParameterValueNameDisplay[i] += VisionSystemClassLibrary.String.TextData.EnumArithmeticString_CHN[camera.Tools[iCurrentToolIndex].Arithmetic.EnumType[iToolParameters[iCurrentToolIndex][i]]][j];
                        parameterSettingsPanel.English_ParameterValueNameDisplay[i] += VisionSystemClassLibrary.String.TextData.EnumArithmeticString_ENG[camera.Tools[iCurrentToolIndex].Arithmetic.EnumType[iToolParameters[iCurrentToolIndex][i]]][j];
                    }
                    else//数字类型
                    {
                        parameterSettingsPanel.ParameterCurrentValue[i] = camera.Tools[iCurrentToolIndex].Arithmetic.CurrentValue[iToolParameters[iCurrentToolIndex][i]];
                        parameterSettingsPanel.ParameterMinValue[i] = camera.Tools[iCurrentToolIndex].Arithmetic.MinValue[iToolParameters[iCurrentToolIndex][i]];
                        parameterSettingsPanel.ParameterMaxValue[i] = camera.Tools[iCurrentToolIndex].Arithmetic.MaxValue[iToolParameters[iCurrentToolIndex][i]];

                        parameterSettingsPanel.Chinese_ParameterValueNameDisplay[i] = " ";
                        parameterSettingsPanel.English_ParameterValueNameDisplay[i] = " ";
                    }

                    parameterSettingsPanel.ParameterEnabled[i] = true;
                }

                parameterSettingsPanel._Apply(true);
            }
            catch (System.Exception ex)
            {
                //不执行操作
            }
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：设置REJECTS列表的工具名称、状态项
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _SetRejectsListItem_0_1(Int32 iIndex)
        {
            if (VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese == language)//中文
            {
                customListRejects.ItemData[iIndex].ItemText[0] = camera.Tools[iIndex].ToolsCHNName;
            }
            else if (VisionSystemClassLibrary.Enum.InterfaceLanguage.English == language)//英文
            {
                customListRejects.ItemData[iIndex].ItemText[0] = camera.Tools[iIndex].ToolsENGName;
            }
            else//其它，默认中文
            {
                customListRejects.ItemData[iIndex].ItemText[0] = camera.Tools[iIndex].ToolsCHNName;
            }

            if (camera.Tools[iIndex].ToolState)//启用
            {
                customListRejects.ItemData[iIndex].ItemText[1] = sMessageText[(Int32)language - 1][3];
            }
            else//禁用
            {
                customListRejects.ItemData[iIndex].ItemText[1] = sMessageText[(Int32)language - 1][4];
            }

        }

        //-----------------------------------------------------------------------
        // 功能说明：设置REJECTS列表的数值项
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _SetRejectsListItem_2(Int32 iIndex)
        {
            UInt32 iValue_1 = 0;//临时变量
            UInt32 iValue_2 = 0;//临时变量

            try//防止上下位机工具不匹配
            {
                if (0 <= shift.DataOfShift.CurrentIndex)//有效班
                {
                    iValue_1 = shift.DataOfShift.CurrentStatisticsInformation.DataOfStatistics[0].CameraStatisticsData[0].RejectedStatistics_Tool[iIndex];
                    iValue_2 = shift.DataOfShift.CurrentStatisticsInformation.DataOfStatistics[0].CameraStatisticsData[0].RejectedNumber;
                    if (0 == iValue_2)//0
                    {
                        customListRejects.ItemData[iIndex].ItemText[2] = 0.ToString() + "（" + 0.0.ToString("F2") + "%）";
                    }
                    else//非0
                    {
                        customListRejects.ItemData[iIndex].ItemText[2] = iValue_1.ToString() + "（" + ((Double)iValue_1 / (Double)iValue_2 * 100.0).ToString("F2") + "%）";
                    }       
                } 
                else//无效班
                {
                    customListRejects.ItemData[iIndex].ItemText[2] = "--";
                }
            }
            catch (System.Exception ex)
            {
            	//不执行操作
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：设置REJECTS列表的选择项
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _SetRejectsListItem_3(Int32 iIndex, Boolean bSelected)
        {
            customListRejects.ItemData[iIndex].ItemDataDisplay[3] = !(bSelected);//图标（Selectd列）
        }

        //-----------------------------------------------------------------------
        // 功能说明：初始化REJECTS列表
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _InitRejectsList()
        {
            customListRejects._ApplyListHeader();//应用列表头属性
            customListRejects._ApplyListItem();//应用列表项属性
        }

        //-----------------------------------------------------------------------
        // 功能说明：设置REJECTS列表
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _SetRejectsList()
        {
            customListRejects._Apply(camera.Tools.Count, 0, 0);//应用列表属性

            //添加列表项数据

            Int32 i = 0;//循环控制变量
            Int32 j = 0;//循环控制变量

            for (i = 0; i < customListRejects.ItemDataNumber; i++)//列表项数据
            {
                _SetRejectsListItem_0_1(i);//设置REJECTS列表的工具名称、状态项

                _SetRejectsListItem_2(i);//设置REJECTS列表的数值项

                //

                customListRejects.ItemData[i].ItemDataDisplay[0] = true;//文本
                customListRejects.ItemData[i].ItemDataDisplay[1] = true;//文本
                customListRejects.ItemData[i].ItemDataDisplay[2] = true;//文本

                customListRejects.ItemData[i].ItemIconIndex[0] = -1;//图标
                customListRejects.ItemData[i].ItemIconIndex[1] = -1;//图标
                customListRejects.ItemData[i].ItemIconIndex[2] = -1;//图标
                customListRejects.ItemData[i].ItemIconIndex[3] = 3;//图标

                if (bClickSelectAllButton)//选择所有
                {
                    _SetRejectsListItem_3(i, true);//设置REJECTS列表的选择项

                    j++;
                } 
                else//选择当前
                {
                    if (i == customListRejects.CurrentListIndex)
                    {
                        _SetRejectsListItem_3(i, true);//设置REJECTS列表的选择项

                        j++;
                    }
                    else
                    {
                        _SetRejectsListItem_3(i, false);//设置REJECTS列表的选择项
                    }
                }

                //

                customListRejects.ItemData[i].ItemFlag = i;
            }

            customListRejects.SelectedItemNumber = j;
        }

        //----------------------------------------------------------------------
        // 功能说明：设置REJECTS列表数据
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetPage_Rejects()
        {
            customListRejects._SetPage();//设置列表项数据

            _SetPreviousNextButton_RejectsList();
        }

        //----------------------------------------------------------------------
        // 功能说明：设置REJECTS列表的【Previous Page】、【Next Page】按钮
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetPreviousNextButton_RejectsList()
        {
            if (1 < customListRejects.TotalPage)//多于一页
            {
                customButtonNextPage_List_1.Visible = true;//【Previous Page】
                customButtonNextPage_List_1.Visible = true;//【Next Page】
            }
            else//小于等于一页
            {
                customButtonPreviousPage_List_1.Visible = false;//【Previous Page】
                customButtonNextPage_List_1.Visible = false;//【Next Page】
            }
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
            customButtonCaption.Language = language;

            customButtonShiftText.Language = language;
            customButtonShiftValue_2.Language = language;
            customButtonBrandText.Language = language;
            customButtonDurationTimeText.Language = language;
            customButtonInspectedText.Language = language;
            customButtonRejectedText.Language = language;

            customButtonRecords.Language = language;
            customButtonParameter.Language = language;
            customButtonSelectAll.Language = language;
            customButtonViewReject.Language = language;
            customButtonRelevancy.Language = language;

            customListRejects.Language = language;

            parameterSettingsPanel.Language = language;

            customButton_CameraName.Language = language;
        }

        //----------------------------------------------------------------------
        // 功能说明：用户调用，设备状态设置完成后，更新页面
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetDeviceState()
        {
            //不执行操作
        }

        //事件

        //----------------------------------------------------------------------
        // 功能说明：点击【CLOSE】按钮事件，执行相应的操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonClose_CustomButton_Click(object sender, EventArgs e)
        {
            bClickSelectAllButton = true;
            customButtonSelectAll.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;

            bClickViewRejectButton = false;
            customButtonViewReject.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;

            bClickParameterButton = false;
            customButtonParameter.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;

            bClickStatusBarButton = false;
            customButtonStatusBar.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;

            bRelevancy = false;
            customButtonRelevancy.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;

            //事件

            if (null != Close_Click)//有效
            {
                Close_Click(this, new CustomEventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【RECORDS】按钮事件，执行相应的操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonRecords_CustomButton_Click(object sender, EventArgs e)
        {
            bStatisticsRecordWindowDisplay = true;

            //

            GlobalWindows.StatisticsRecord_Window.StatisticsRecordControl.Language = language;
            GlobalWindows.StatisticsRecord_Window.StatisticsRecordControl.SelectedCameraType = camera.Type;
            GlobalWindows.StatisticsRecord_Window.StatisticsRecordControl.Chinese_CameraName = camera.CameraCHNName;
            GlobalWindows.StatisticsRecord_Window.StatisticsRecordControl.English_CameraName = camera.CameraENGName;
            GlobalWindows.StatisticsRecord_Window.StatisticsRecordControl.SelectNewRecord = false;
            GlobalWindows.StatisticsRecord_Window.StatisticsRecordControl._Properties(shift);

            GlobalWindows.StatisticsRecord_Window.StartPosition = FormStartPosition.CenterScreen;
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.StatisticsRecord_Window.TopMost = true;//将窗口置于顶层
            }
            else//其它
            {
                GlobalWindows.StatisticsRecord_Window.Visible = true;//显示
            }

            //

            GlobalWindows.StatisticsRecord_Window.StatisticsRecordControl._StartGetStatisticsRecord();

            //事件

            if (null != GetRecords)//有效
            {
                GetRecords(this, new CustomEventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【PARAMETER】按钮事件，执行相应的操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonParameter_CustomButton_Click(object sender, EventArgs e)
        {
            bClickParameterButton = !bClickParameterButton;

            parameterSettingsPanel.Visible = bClickParameterButton;
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【SELECT ALL】按钮事件，执行相应的操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonSelectAll_CustomButton_Click(object sender, EventArgs e)
        {
            bClickSelectAllButton = !bClickSelectAllButton;

            customListRejects._SelectAll(bClickSelectAllButton);

            if (!bClickSelectAllButton)//选择当前
            {
                customListRejects.ItemData[customListRejects.CurrentListIndex].ItemDataDisplay[3] = false;
                customListRejects.SelectedItemNumber = 1;

                customListRejects._Refresh(customListRejects.CurrentListIndex);//刷新
            }

            //

            if (bClickViewRejectButton)//【VIEW REJECT】按钮按下
            {
                _GetCurrentImageIndex();

                _SetViewReject();

                //事件

                if (null != ViewRejectImage)//有效
                {
                    ViewRejectImage(this, new CustomEventArgs());
                }
            }
            else//【VIEW REJECT】按钮弹起
            {
                _SetImageView();
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【VIEW REJECT】按钮事件，执行相应的操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonViewReject_CustomButton_Click(object sender, EventArgs e)
        {
            bClickViewRejectButton = !bClickViewRejectButton;

            //

            if (bClickViewRejectButton)//【VIEW REJECT】按钮按下
            {
                for (Int32 i = 0; i < parameter.Camera.Count; i++) //遍历所有相机
                {
                    if (cameratypeSelected == parameter.Camera[i].Type) //相机被选中
                    {
                        customButton_CameraName.Chinese_TextDisplay = new String[1] { parameter.Camera[i].CameraCHNName };//设置显示的文本
                        customButton_CameraName.English_TextDisplay = new String[1] { parameter.Camera[i].CameraENGName };//设置显示的文本

                        customButton_CameraName.Visible = true;
                        break;
                    }
                }

                _GetCurrentImageIndex();
                
                _SetViewReject();

                //事件

                if (null != ViewRejectImage)//有效
                {
                    ViewRejectImage(this, new CustomEventArgs());
                }
            }
            else//【VIEW REJECT】弹起
            {
                customButton_CameraName.Visible = false;

                _SetImageView();
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【REJECT IMAGE SELECTION】按钮事件，执行相应的操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonSelection_CustomButton_Click(object sender, EventArgs e)
        {
            GlobalWindows.DigitalKeyboard_Window.WindowParameter = 13;//窗口特征数值
            GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.Language = language;//语言
            GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.Chinese_Caption = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][5];//中文标题文本
            GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.English_Caption = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][5];//英文标题文本
            GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.Precision = 0;//输入的数据类型
            GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.MaxLength = 4;//数值长度范围
            GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.MinValue = 1;//最小值
            GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.MaxValue = iRejectImageMaxNumber;//最大值
            GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.StringValue = (iCurrentRejectImageIndex + 1).ToString();//初始显示的数值

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
        // 功能说明：点击【-】按钮事件，执行相应的操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonSubtract_CustomButton_Click(object sender, EventArgs e)
        {
            if (0 == iCurrentRejectImageIndex)//最小值
            {
                iCurrentRejectImageIndex = iRejectImageMaxNumber - 1;
            }
            else//其它
            {
                iCurrentRejectImageIndex--;
            }

            customButtonSelection.Chinese_TextDisplay = new String[1] { (iCurrentRejectImageIndex + 1).ToString() };//设置显示的文本
            customButtonSelection.English_TextDisplay = new String[1] { (iCurrentRejectImageIndex + 1).ToString() };//设置显示的文本

            //事件

            if (null != ViewRejectImage)//有效
            {
                ViewRejectImage(this, new CustomEventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【+】按钮事件，执行相应的操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonPlus_CustomButton_Click(object sender, EventArgs e)
        {
            if (iRejectImageMaxNumber - 1 == iCurrentRejectImageIndex)//最大值
            {
                iCurrentRejectImageIndex = 0;
            }
            else//其它
            {
                iCurrentRejectImageIndex++;
            }

            customButtonSelection.Chinese_TextDisplay = new String[1] { (iCurrentRejectImageIndex + 1).ToString() };//设置显示的文本
            customButtonSelection.English_TextDisplay = new String[1] { (iCurrentRejectImageIndex + 1).ToString() };//设置显示的文本

            //事件

            if (null != ViewRejectImage)//有效
            {
                ViewRejectImage(this, new CustomEventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【STATUS BAR】按钮事件，执行相应的操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonStatusBar_CustomButton_Click(object sender, EventArgs e)
        {
            bClickStatusBarButton = !bClickStatusBarButton;

            imageDisplayView.ShowTitle = !bClickStatusBarButton;
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：点击REJECTS列表事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customListRejects_CustomListItem_Click(object sender, EventArgs e)
        {
            if (bClickSelectAllButton)//选择所有
            {
                customListRejects.ItemData[customListRejects.CurrentListIndex].ItemDataDisplay[3] = false;
                customListRejects.SelectedItemNumber++;

                customListRejects._Refresh(customListRejects.CurrentListIndex);//刷新
            } 
            else//选择当前
            {
                if (customListRejects.ItemData[customListRejects.CurrentDataIndex].ItemDataDisplay[3])//未选择（说明点击之前处于选择状态，需要恢复该状态）
                {
                    customListRejects.ItemData[customListRejects.CurrentDataIndex].ItemDataDisplay[3] = false;//选择当前项
                }
                else//选择（说明点击之前处于未选择状态，需要取消其他选择的项）
                {
                    customListRejects._SelectAll(false);//全部取消选择

                    customListRejects.ItemData[customListRejects.CurrentDataIndex].ItemDataDisplay[3] = false;//选择当前项
                }
                customListRejects.SelectedItemNumber = 1;

                customListRejects._Refresh();//刷新

                //

                if (bClickViewRejectButton)//【VIEW REJECT】按钮按下
                {
                    if (false == bRelevancy) //单工具进支持非关联查询
                    {
                        _GetCurrentImageIndex();

                        _SetViewReject();

                        //事件

                        if (null != ViewRejectImage)//有效
                        {
                            ViewRejectImage(this, new CustomEventArgs());
                        }
                    }
                }
                else//【VIEW REJECT】按钮弹起
                {
                    _SetImageView();
                }
            }

            //

            _SetToolParameters();
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【Previous Page】按钮事件，执行相应的操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonPreviousPage_List_1_CustomButton_Click(object sender, EventArgs e)
        {
            customListRejects._ClickPage(true);//翻页，上一页
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【Next Page】按钮事件，执行相应的操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonNextPage_List_1_CustomButton_Click(object sender, EventArgs e)
        {
            customListRejects._ClickPage(false);//翻页，下一页
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：STATISTICS，获取统计数据，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_Statistics_GetRecordData_Wait(object sender, EventArgs e)
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
                bGetStatisticsMessageWindowShow = false;

                //事件

                if (null != Close_Click)//有效
                {
                    Close_Click(this, new CustomEventArgs());
                }
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：STATISTICS RECORD，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void statisticsRecordWindow_WindowClose(object sender, EventArgs e)
        {
            bStatisticsRecordWindowDisplay = false;

            //

            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.StatisticsRecord_Window.TopMost = false;//取消置于顶层
            }
            else//其它
            {
                GlobalWindows.StatisticsRecord_Window.Visible = false;//隐藏
            }
            
            //事件

            if (null != GetRecordData)//有效
            {
                GetRecordData(this, new CustomEventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：STATISTICS RECORD，删除统计数据，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void statisticsRecordWindow_DeleteRecords(object sender, EventArgs e)
        {
            //事件

            if (null != DeleteRecords)//有效
            {
                DeleteRecords(this, new CustomEventArgs());
            }
        }
 
        //

        //----------------------------------------------------------------------
        // 功能说明：STATISTICS，REJECT IMAGE SELECTION，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void digitalKeyboardWindow_WindowClose_Statistics_RejectImageSelection(object sender, EventArgs e)
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
                iCurrentRejectImageIndex = Convert.ToInt32(GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.NumericalValue) - 1;

                customButtonSelection.Chinese_TextDisplay = new String[1] { (iCurrentRejectImageIndex + 1).ToString() };//设置显示的文本
                customButtonSelection.English_TextDisplay = new String[1] { (iCurrentRejectImageIndex + 1).ToString() };//设置显示的文本

                //事件

                if (null != ViewRejectImage)//有效
                {
                    ViewRejectImage(this, new CustomEventArgs());
                }
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：定时器事件，获取统计记录，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void timerStatistics_Tick(object sender, EventArgs e)
        {
            if (bGetStatisticsMessageWindowShow)//显示
            {
                iTimerGetStatisticsCount--;

                if (0 >= iTimerGetStatisticsCount)//超时
                {
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = " ";
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = " ";
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][2];
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][2];
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = " ";
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = " ";

                    timerStatistics.Stop();//关闭定时器

                    iTimerGetStatisticsCount = iTimerGetStatisticsMaxCount;
                }
                else//计数
                {
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][6] + "，" + iTimerGetStatisticsCount.ToString();
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][6] + "，" + iTimerGetStatisticsCount.ToString();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【RELEVANCY】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonRelevancy_CustomButton_Click(object sender, EventArgs e)
        {
            bRelevancy = !bRelevancy;

            if (false == bRelevancy) //关联信息弹起，相机寻则恢复默认
            {
                cameratypeSelected = camera.Type;

                for (Int32 i = 0; i < parameter.Camera.Count; i++) //遍历所有相机
                {
                    if (cameratypeSelected == parameter.Camera[i].Type) //相机被选中
                    {
                        customButton_CameraName.Chinese_TextDisplay = new String[1] { parameter.Camera[i].CameraCHNName };//设置显示的文本
                        customButton_CameraName.English_TextDisplay = new String[1] { parameter.Camera[i].CameraENGName };//设置显示的文本
                        break;
                    }
                }
            }

            //事件

            if (null != Relevancy_Click)//有效
            {
                Relevancy_Click(this, new CustomEventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：查询关联缺陷图像按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void imageDisplayView_Control_DoubleClick(object sender, EventArgs e)
        {
            if (bClickViewRejectButton && bRelevancy)//【VIEW REJECT】和【Relevancy】按下
            {
                if (VisionSystemClassLibrary.Enum.RelevancyType.Inner == camera.RelevancyCameraInfo.rRelevancyType) //当前相机有关联
                {
                    Int32 selectCameraIndex = -1;
                    for (Byte i = 0; i < camera.RelevancyCameraInfo.RelevancyCameraInfo.Count; i++) //循环所有关联相机
                    {
                        if (cameratypeSelected == camera.RelevancyCameraInfo.RelevancyCameraInfo.ElementAt(i).Key)
                        {
                            selectCameraIndex = i;
                            break;
                        }
                    }

                    if (selectCameraIndex >= 0) //选择相机有效
                    {
                        selectCameraIndex = (selectCameraIndex + 1) % camera.RelevancyCameraInfo.RelevancyCameraInfo.Count;

                        cameratypeSelected = camera.RelevancyCameraInfo.RelevancyCameraInfo.ElementAt(selectCameraIndex).Key;

                        for (Int32 i = 0; i < parameter.Camera.Count; i++) //遍历所有相机
                        {
                            if (cameratypeSelected == parameter.Camera[i].Type) //相机被选中
                            {
                                customButton_CameraName.Chinese_TextDisplay = new String[1] { parameter.Camera[i].CameraCHNName };//设置显示的文本
                                customButton_CameraName.English_TextDisplay = new String[1] { parameter.Camera[i].CameraENGName };//设置显示的文本
                                break;
                            }
                        }

                        //事件

                        if (null != ViewRejectImage_Relevancy)//有效
                        {
                            ViewRejectImage_Relevancy(this, new CustomEventArgs());
                        }
                    }
                }
            }
        }
    }
}