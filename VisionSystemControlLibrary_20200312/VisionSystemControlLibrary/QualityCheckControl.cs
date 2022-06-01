/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：QualityCheckControl.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：质量检测控件

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
using System.IO;

using VisionSystemClassLibrary;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;

using System.Reflection;

namespace VisionSystemControlLibrary
{
    public partial class QualityCheckControl : UserControl
    {
        private Int32 iImageType = 1;//图像类型。取值范围：1，实时图像；2，自学习图像；3，剔除图像

        //

        private VisionSystemClassLibrary.Enum.InterfaceLanguage language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;//属性，语言

        private VisionSystemClassLibrary.Enum.DeviceState devicestate = VisionSystemClassLibrary.Enum.DeviceState.Stop;//属性，设备状态

        //

        private Single fMachineSpeed = 800F;//属性，机器速度（PPM）

        //

        private Int32 iCurrentToolIndex;//属性（只读），当前工具索引值（从0开始）

        private Int32[] iSelectedToolsArray;//选择的工具数组（[工具索引值（从0开始）]）
        private Int32 iCurrentIndex_SelectedToolsArray;//选择的工具数组中当前选择的工具索引值（从0开始）

        //

        private Boolean bSaveProduct = false;//【Save Product】按钮的状态。取值范围：true，产品数据被修改；false，产品数据未被修改

        //
        
        private Boolean bClickCloseButton = false;//是否点击【CLOSE】按钮。取值范围：true，是；false，否

        //

        //private Boolean bLearnSampleSelected = false;//【LEARN SAMPLE】（【UPDATE THRESHOLD】）按钮是否按下。取值范围：true，是；false，否
        private Boolean bLiveViewSelected = false;//【LIVE VIEW】按钮是否按下。取值范围：true，是；false，否
        private Boolean bLoadSampleSelected = false;//【LOAD SAMPLE】按钮是否按下。取值范围：true，是；false，否
        private Boolean bManageToolsSelected = false;//【MANAGE TOOLS】按钮是否按下。取值范围：true，是；false，否
        private Boolean bLoadRejectSelected = false;//【LOAD REJECT】按钮是否按下。取值范围：true，是；false，否
        private Boolean bStatusBarSelected = false;//【STATUS BAR】按钮是否按下。取值范围：true，是；false，否
        private Boolean bMeasureToolSelected = false;//【MEASURE TOOL】按钮是否按下。取值范围：true，是；false，否

        //

        public const Int32 RejectImageMaxNumber = 48;//剔除图像最大值

        private Int32 iRejectImageIndex = RejectImageMaxNumber - 1;//属性（只读），剔除图像索引值（0 ~ 47）

        //

        private String[][] sMessageText = new String[2][];//提示信息窗口上显示的文本（[语言][包含的文本]）
        private String[][] sMessageText_1 = new String[2][];//标题控件上显示的文本（[语言][包含的文本]）

        //

        private Int32[][] iToolParameters = new Int32[10][];//工具参数（[工具索引值（从0开始）][参数索引值（从0开始）]）
        private Int32[] iToolParameterNumber = new Int32[10];//工具参数数目（[工具索引值（从0开始）]）

        //

        private Point pointFirst_MeasureTool;//【MEASURE TOOL】按下，在图像显示控件上画直线时记录第一个点的坐标
        private Point pointSecond_MeasureTool;//【MEASURE TOOL】按下，在图像显示控件上画直线时记录第二个点的坐标
        private Int32 iImageDisplayClickTime = 0;//图像显示控件点击的次数

        private Point pointROI = new Point();//单击兴趣区域内部时坐标

        //

        private Boolean bHandleVisible = false;//手柄可见性。取值范围：true，显示；false，隐藏
        private Boolean bSizeAdjustedPanelVisible = false;//区域调整控制面板可见性。取值范围：true，显示；false，隐藏

        private Rectangle drawROI;//绘图兴趣区域
        private Rectangle drawROIOriginal;//原始绘图兴趣区域

        private Int32 iCompensation_H = 0;//水平方向抖动补偿值
        private Int32 iCompensation_V = 0;//垂直方向抖动补偿值

        //

        private Point pointMouse_Move;//拖动手柄或整个兴趣区域时，记录鼠标的位置

        private Point pointMouse_SizeAdjustedPanel;//拖动兴趣区域调整控件时，记录鼠标的位置

        private ContentAlignment contentalignmentHandleType = ContentAlignment.MiddleCenter;//兴趣区域调整手柄类型（ContentAlignment.MiddleCenter表示无效）

        private Boolean bMouseMove = false;//鼠标指针是否移动。取值范围：true，是；false，否

        private Boolean bMouseMove_MouseIn = false;//鼠标指针是否在兴趣区域内。取值范围：true，是；false，否

        //

        private Size SizeAdjustedPanelDefaultSize = new Size(0, 0);//兴趣区域调整控件默认大小

        private Point SizeAdjustedPanelDefaultLocation = new Point(0, 0);//兴趣区域调整控件默认位置

        private Point ParameterSettingsPanelLocation = new Point(0, 0);//参数设置面板控件默认位置

        //

        private Boolean bDrawImageProcessingResult = true;//是否绘制图像处理结果。取值范围：true，是；false，否

        //

        private Bitmap bitmapNone = null;//无效图像

        //

        VisionSystemClassLibrary.Class.Camera camera = new VisionSystemClassLibrary.Class.Camera();   //创建Camera类库的对象
        VisionSystemClassLibrary.Class.Camera cameraTemp = new VisionSystemClassLibrary.Class.Camera();//创建Camera类库暂存的对象

        //

        private VisionSystemClassLibrary.Class.Brand brand = new VisionSystemClassLibrary.Class.Brand();//属性（只读），品牌

        //

        private Image<Bgr, Byte> imageShow;                                     //显示图像

        //

        [Browsable(true), Description("当前选择的工具发生改变的事件"), Category("QualityCheckControl 事件")]
        public event EventHandler ToolChanged;

        [Browsable(true), Description("工具参数值发生改变的事件"), Category("QualityCheckControl 事件")]
        public event EventHandler ToolParameterValueChanged;

        [Browsable(true), Description("工具的兴趣区域发生改变的事件"), Category("QualityCheckControl 事件")]
        public event EventHandler ToolRegionChanged;

        //

        [Browsable(true), Description("点击【返回】按钮时产生的事件"), Category("QualityCheckControl 事件")]
        public event EventHandler Close_Click;

        [Browsable(true), Description("点击【Save Product】按钮时产生的事件"), Category("QualityCheckControl 事件")]
        public event EventHandler SaveProduct_Click;

        [Browsable(true), Description("点击【Learn Sample】按钮时产生的事件"), Category("QualityCheckControl 事件")]
        public event EventHandler LearnSample_Click;

        [Browsable(true), Description("点击【Live View】按钮时产生的事件"), Category("QualityCheckControl 事件")]
        public event EventHandler LiveView_Click;

        [Browsable(true), Description("点击【Load Sample】按钮时产生的事件"), Category("QualityCheckControl 事件")]
        public event EventHandler LoadSample_Click;

        [Browsable(true), Description("点击【Manage Tools】按钮时产生的事件"), Category("QualityCheckControl 事件")]
        public event EventHandler ManageTools_Click;

        [Browsable(true), Description("点击【Load Reject】按钮时产生的事件"), Category("QualityCheckControl 事件")]
        public event EventHandler LoadReject_Click;

        [Browsable(true), Description("【Load Reject】按钮按下，选择某一剔除图像时产生的事件"), Category("QualityCheckControl 事件")]
        public event EventHandler LoadReject_ImageSelect_Click;

        //构造函数

        //----------------------------------------------------------------------
        // 功能说明：构造函数，初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public QualityCheckControl()
        {
            InitializeComponent();

            //由于该控件仅有一个实例，因此可以在此处进行事件的订阅

            if (null != GlobalWindows.MessageDisplay_Window)
            {
                GlobalWindows.MessageDisplay_Window.WindowClose_QualityCheck_SaveProduct_Confirm += new System.EventHandler(messageDisplayWindow_WindowClose_QualityCheck_SaveProduct_Confirm);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_QualityCheck_SaveProduct_Success += new System.EventHandler(messageDisplayWindow_WindowClose_QualityCheck_SaveProduct_Success);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_QualityCheck_SaveProduct_Failure += new System.EventHandler(messageDisplayWindow_WindowClose_QualityCheck_SaveProduct_Failure);//订阅事件

                GlobalWindows.MessageDisplay_Window.WindowClose_QualityCheck_LearnSample_Confirm += new System.EventHandler(messageDisplayWindow_WindowClose_QualityCheck_LearnSample_Confirm);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_QualityCheck_LearnSample_Success += new System.EventHandler(messageDisplayWindow_WindowClose_QualityCheck_LearnSample_Success);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_QualityCheck_LearnSample_Failure += new System.EventHandler(messageDisplayWindow_WindowClose_QualityCheck_LearnSample_Failure);//订阅事件
            }

            //

            customButtonAxisValue.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonAxisValue.English_TextDisplay = new String[1] { " " };//设置显示的文本

            customButtonColorValue.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonColorValue.English_TextDisplay = new String[1] { " " };//设置显示的文本

            customButtonDeltaXValue.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonDeltaXValue.English_TextDisplay = new String[1] { " " };//设置显示的文本

            customButtonDeltaYValue.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonDeltaYValue.English_TextDisplay = new String[1] { " " };//设置显示的文本

            //

            SizeAdjustedPanelDefaultSize = new Size(sizeAdjustedPanel.Width, sizeAdjustedPanel.Height);//兴趣区域调整控件默认大小

            SizeAdjustedPanelDefaultLocation = new Point(sizeAdjustedPanel.Location.X, sizeAdjustedPanel.Location.Y);

            ParameterSettingsPanelLocation = new Point(parameterSettingsPanel.Location.X, parameterSettingsPanel.Location.Y);

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

            drawROI = new Rectangle();
            iCurrentToolIndex = 0;

            //

            FieldInfo[] fieldinfo = VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese.GetType().GetFields();//获取公共字段
            if (null != fieldinfo && 1 < fieldinfo.Length)//有效
            {
                sMessageText = new String[fieldinfo.Length - 1][];
                sMessageText_1 = new String[fieldinfo.Length - 1][];
                for (i = 0; i < fieldinfo.Length - 1; i++)
                {
                    sMessageText[i] = new String[9];
                    sMessageText_1[i] = new String[1];
                }

                //

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] = "确定保存数据";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] = "Save Product";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][1] = "保存数据成功";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][1] = "Save Product Completed";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][2] = "保存数据失败";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][2] = "Save Product Failed";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][3] = "确定创建模板";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][3] = "Learn Sample";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][4] = "创建模板成功";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][4] = "Learn Sample Completed";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][5] = "创建模板失败";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][5] = "Learn Sample Failed";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][6] = "确定更新阈值";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][6] = "Update Threshold";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][7] = "更新阈值成功";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][7] = "Update Threshold Completed";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][8] = "更新阈值失败";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][8] = "Update Threshold Failed";

                //

                sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] = customButtonCaption.Chinese_TextDisplay[0];
                sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] = customButtonCaption.English_TextDisplay[0];
            }

            //

            bitmapNone = new Bitmap(imageDisplayView.Width, imageDisplayView.Height);//无效图像
        }

        //属性

        //-----------------------------------------------------------------------
        // 功能说明：SelectedCamera属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(false), Description("相机"), Category("QualityCheckControl 通用")]
        public VisionSystemClassLibrary.Class.Camera SelectedCamera
        {
            get
            {
                return camera;
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：SystemBrand属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("品牌"), Category("QualityCheckControl 通用")]
        public VisionSystemClassLibrary.Class.Brand SystemBrand//属性
        {
            get//读取
            {
                return brand;
            }
        }
        
        //

        //-----------------------------------------------------------------------
        // 功能说明：Language属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("语言"), Category("QualityCheckControl 通用")]
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
        [Browsable(true), Description("设备状态"), Category("QualityCheckControl 通用")]
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

        //-----------------------------------------------------------------------
        // 功能说明：MachineSpeed属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("机器速度（PPM）"), Category("QualityCheckControl 通用")]
        public Single MachineSpeed
        {
            get//读取
            {
                return fMachineSpeed;
            }
            set//设置
            {
                if (0 < fMachineSpeed)//有效
                {
                    fMachineSpeed = (Single)1 / ((Single)value / (Single)60) * (Single)1000;
                }
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：RejectImageIndex属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("剔除图像索引值（从0开始）"), Category("QualityCheckControl 通用")]
        public Int32 RejectImageIndex//属性
        {
            get//读取
            {
                return iRejectImageIndex;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：CurrentToolIndex属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("当前工具索引值（从0开始）"), Category("QualityCheckControl 通用")]
        public Int32 CurrentToolIndex//属性
        {
            get//读取
            {
                return iCurrentToolIndex;
            }
        }

        //

        ////----------------------------------------------------------------------
        //// 功能说明：LearnSampleSelected属性的实现
        //// 输入参数：无
        //// 输出参数：无
        //// 返回值：无
        ////----------------------------------------------------------------------
        //[Browsable(false), Description("【LEARN SAMPLE】（【UPDATE THRESHOLD】）按钮是否按下。取值范围：true，是；false，否"), Category("QualityCheckControl 通用")]
        //public Boolean LearnSampleSelected//属性
        //{
        //    get//读取
        //    {
        //        return bLearnSampleSelected;
        //    }
        //}

        //----------------------------------------------------------------------
        // 功能说明：LiveViewSelected属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("【LIVE VIEW】按钮是否按下。取值范围：true，是；false，否"), Category("QualityCheckControl 通用")]
        public Boolean LiveViewSelected//属性
        {
            get//读取
            {
                return bLiveViewSelected;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：LoadSampleSelected属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("【LOAD SAMPLE】按钮是否按下。取值范围：true，是；false，否"), Category("QualityCheckControl 通用")]
        public Boolean LoadSampleSelected//属性
        {
            get//读取
            {
                return bLoadSampleSelected;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ManageToolsSelected属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("【MANAGE TOOLS】按钮是否按下。取值范围：true，是；false，否"), Category("QualityCheckControl 通用")]
        public Boolean ManageToolsSelected//属性
        {
            get//读取
            {
                return bManageToolsSelected;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：LoadRejectSelected属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("【LOAD REJECT】按钮是否按下。取值范围：true，是；false，否"), Category("QualityCheckControl 通用")]
        public Boolean LoadRejectSelected//属性
        {
            get//读取
            {
                return bLoadRejectSelected;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：StatusBarSelected属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("【STATUS BAR】按钮是否按下。取值范围：true，是；false，否"), Category("QualityCheckControl 通用")]
        public Boolean StatusBarSelected//属性
        {
            get//读取
            {
                return bStatusBarSelected;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：MeasureToolSelected属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("【MEASURE TOOL】按钮是否按下。取值范围：true，是；false，否"), Category("QualityCheckControl 通用")]
        public Boolean MeasureToolSelected//属性
        {
            get//读取
            {
                return bMeasureToolSelected;
            }
        }

        //函数

        //----------------------------------------------------------------------
        // 功能说明：应用设置的属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _Properties(VisionSystemClassLibrary.Class.Camera camera_parameter, VisionSystemClassLibrary.Class.Brand brand_parameter)
        {
            brand = brand_parameter;

            camera = camera_parameter;

            //

            _SetCamera();//载入

            _SetLanguage();//设置语言
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：载入
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _SetCamera()
        {
            customButtonCaption.Chinese_TextDisplay = new String[1] { camera.CameraCHNName + sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] };//设置显示的文本
            customButtonCaption.English_TextDisplay = new String[1] { camera.CameraENGName + sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] };//设置显示的文本

            labelCheckTimeValue.Text = "";

            //

            iCompensation_H = 0;//水平方向抖动补偿值
            iCompensation_V = 0;//垂直方向抖动补偿值

            //

            bDrawImageProcessingResult = true;

            bHandleVisible = false;
            bSizeAdjustedPanelVisible = false;

            sizeAdjustedPanel.Location = SizeAdjustedPanelDefaultLocation;
            contentalignmentHandleType = ContentAlignment.MiddleCenter;

            //

            _ShowHandle(false);
            _ResetHandleColor();

            sizeAdjustedPanel.Visible = false;//兴趣区域调整面板

            parameterSettingsPanel.Visible = true;//显示参数面板

            customListTool.Visible = false;//隐藏工具列表

            customButtonSaveProduct.BitmapWhole = VisionSystemControlLibrary.Properties.Resources.Button;//【SAVE PRODUCT】

            //bLearnSampleSelected = false;
            customButtonLearnSample.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【LEARN SAMPLE】（UPDATE THRESHOLD）

            bLiveViewSelected = false;
            customButtonLiveView.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【LIVE VIEW】

            bLoadSampleSelected = false;
            customButtonLoadSample.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【LOAD SAMPLE】

            bManageToolsSelected = false;
            customButtonManageTools.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【MANAGE TOOLS】
            customButtonEditTools.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【EDIT TOOLS】

            bLoadRejectSelected = false;
            customButtonLoadReject.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【LOAD REJECT】

            bStatusBarSelected = false;
            customButtonStatusBar.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【STATUS BAR】
            imageDisplayView.ShowTitle = true;

            customButtonSubtract.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【-】
            customButtonPlus.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【+】

            customButtonRejectImageIndex.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//剔除图像索引控件

            //

            iRejectImageIndex = RejectImageMaxNumber - 1;
            customButtonRejectImageIndex.Chinese_TextDisplay = new String[1] { (iRejectImageIndex + 1).ToString() };//设置显示的文本
            customButtonRejectImageIndex.English_TextDisplay = new String[1] { (iRejectImageIndex + 1).ToString() };//设置显示的文本

            //

            _SetDeviceState();

            //

            imageShow = new Image<Bgr, Byte>(744, 480);

            //

            _GetToolParameters();//获取工具参数

            //

            _GetSelectedTools();//获取选择的工具

            iCurrentIndex_SelectedToolsArray = 0;//选择的工具数组中当前选择的工具索引值（从0开始）

            iCurrentToolIndex = iSelectedToolsArray[iCurrentIndex_SelectedToolsArray];//属性（只读），当前工具索引值（从0开始）

            _AddListItemData_SelectedTools();//添加列表项数据

            //

            _SelectTool();

            //

            if (1 < iSelectedToolsArray.Length)//多于1个工具
            {
                if (iCurrentToolIndex == iSelectedToolsArray[0])//首页
                {
                    customButtonPrevious.Visible = false;
                    customButtonNext.Visible = true;
                }
                else if (iCurrentToolIndex == iSelectedToolsArray[iSelectedToolsArray.Length - 1])//末页
                {
                    customButtonPrevious.Visible = true;
                    customButtonNext.Visible = false;
                }
                else//其它
                {
                    customButtonPrevious.Visible = true;
                    customButtonNext.Visible = true;
                }
            }
            else//其它
            {
                customButtonPrevious.Visible = false;
                customButtonNext.Visible = false;
            }

            //

            camera._CopyTo(ref cameraTemp);
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：工具学习
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _LearnToolProcessing()
        {
            if (0 == camera.Tools[CurrentToolIndex].Type)//Grid
            {
                Image<Bgr, Byte> image = camera.ImageLearn.Copy();

                camera._LearnSample(image);
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：学习图像处理函数
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        private void _LearnImageProcessing()
        {
            if (null != camera.ImageLearn.Data)
            {
                camera._LearnSample(camera.ImageLearn);
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
                    if (camera.Tools[i].Arithmetic.State[j])//使能
                    {
                        iToolParameters[i][iToolParameterNumber[i]] = j;

                        iToolParameterNumber[i]++;
                    }
                }
            }
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：设置工具参数
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _SetInspectionTime(Int32 iInspectionTime)
        {
            //if (0 < fMachineSpeed)//有效
            //{
                labelCheckTimeValue.Text = Convert.ToInt32((double)iInspectionTime / 100).ToString("000");
                //labelCheckTimeValue.Text = ((Single)iInspectionTime / fMachineSpeed).ToString("F2");
            //}
        }

        //

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
        // 功能说明：获取选择的工具数组
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _GetSelectedTools()
        {
            Int32 i = 0;//循环控制变量
            Int32 j = 0;//循环控制变量

            for (i = 0; i < camera.Tools.Count; i++)
            {
                if (camera.Tools[i].ToolState)//选择
                {
                    j++;
                }
            }
            //
            if (0 == j)//未选择任何工具
            {
                //默认至少有一个工具被选择

                iSelectedToolsArray = new Int32[1];

                iSelectedToolsArray[0] = 0;

                iCurrentIndex_SelectedToolsArray = 0;

                camera.Tools[0].ToolState = true;
            }
            else//至少有一个工具被选择
            {
                iSelectedToolsArray = new Int32[j];

                j = 0;
                for (i = 0; i < camera.Tools.Count; i++)
                {
                    if (camera.Tools[i].ToolState)
                    {
                        iSelectedToolsArray[j] = i;
                        j++;
                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：设置选择的工具
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _SetSelectedTools()
        {
            Int32 i = 0;//循环控制变量
            Int32 j = 0;//循环控制变量

            if (0 == customListTool.SelectedItemNumber)//未选择任何工具
            {
                //默认至少有一个工具被选择

                iSelectedToolsArray = new Int32[1];

                iSelectedToolsArray[0] = 0;

                iCurrentIndex_SelectedToolsArray = 0;

                camera.Tools[0].ToolState = true;
            }
            else//至少有一个工具被选择
            {
                iSelectedToolsArray = new Int32[customListTool.SelectedItemNumber];

                j = 0;
                for (i = 0; i < customListTool.ItemDataNumber; i++)
                {
                    camera.Tools[i].ToolState = !(customListTool.ItemData[i].ItemDataDisplay[1]);
                    
                    if (!(customListTool.ItemData[i].ItemDataDisplay[1]))//选择
                    {
                        iSelectedToolsArray[j] = i;
                        j++;
                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：添加工具选择列表项数据
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _AddListItemData_SelectedTools()
        {
            customListTool._ApplyListHeader();//应用列表头属性
            customListTool._ApplyListItem();//应用列表项属性

            //

            customListTool._Apply(camera.Tools.Count);//应用列表属性

            //添加列表项数据

            Int32 i = 0;//循环控制变量
            Int32 j = 0;//循环控制变量

            for (i = 0; i < customListTool.ItemDataNumber; i++)//列表项数据
            {
                if (VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese == language)//中文
                {
                    customListTool.ItemData[i].ItemText[0] = camera.Tools[i].ToolsCHNName;
                }
                else if (VisionSystemClassLibrary.Enum.InterfaceLanguage.English == language)//英文
                {
                    customListTool.ItemData[i].ItemText[0] = camera.Tools[i].ToolsENGName;
                }
                else//其它，默认中文
                {
                    customListTool.ItemData[i].ItemText[0] = camera.Tools[i].ToolsCHNName;
                }

                //

                customListTool.ItemData[i].ItemDataDisplay[0] = true;//文本
                customListTool.ItemData[i].ItemIconIndex[0] = -1;//图标
                customListTool.ItemData[i].ItemIconIndex[1] = 0;//图标

                if (camera.Tools[i].ToolState)//选择
                {
                    customListTool.ItemData[i].ItemDataDisplay[1] = false;//图标（Selectd列）

                    j++;
                } 
                else//未选择
                {
                    customListTool.ItemData[i].ItemDataDisplay[1] = true;//图标（Selectd列）
                }

                //

                customListTool.ItemData[i].ItemFlag = i;
            }
            customListTool.SelectedItemNumber = j;

            //设置列表项数据

            customListTool._SetPage();
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：设置语言
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _SetLanguage()
        {
            imageDisplayView.Language = language;//图像显示控件

            //

            customButtonSelectedCheckText.Language = language;//当前工具文本控件
            customButtonSelectedCheckValue.Language = language;//当前工具名称控件

            customButtonCheckTimeText.Language = language;//当前工具检测时间文本控件

            //

            sizeAdjustedPanel.Language = language;//区域调整面板

            parameterSettingsPanel.Language = language;//参数设置面板

            customListTool.Language = language;//工具选择列表

            //

            customButtonCaption.Language = language;//设置显示的文本

            //

            customButtonClose.Language = language;//【Close】
            customButtonSaveProduct.Language = language;//【SAVE PRODUCT】
            customButtonLearnSample.Language = language;//【LEARN SAMPLE】（【UPDATE THRESHOLD】）
            customButtonLiveView.Language = language;//【LIVW VIEW】
            customButtonLoadSample.Language = language;//【LOAD SAMPLE】
            customButtonManageTools.Language = language;//【MANAGE TOOLS】
            customButtonLoadReject.Language = language;//【LOAD REJECT】
            customButtonStatusBar.Language = language;//【STATUS BAR】
            customButtonMeasureTool.Language = language;//【MEASURE TOOL】

            //

            customButtonBestContrastText.Language = language;//Best Contrast

            //

            Int32 i = 0;//循环控制变量

            for (i = 0; i < customListTool.ItemDataNumber; i++)//列表项数据
            {
                if (VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese == language)//中文
                {
                    customListTool.ItemData[i].ItemText[0] = camera.Tools[i].ToolsCHNName;
                }
                else if (VisionSystemClassLibrary.Enum.InterfaceLanguage.English == language)//英文
                {
                    customListTool.ItemData[i].ItemText[0] = camera.Tools[i].ToolsENGName;
                }
                else//其它，默认中文
                {
                    customListTool.ItemData[i].ItemText[0] = camera.Tools[i].ToolsCHNName;
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：系统状态
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _SetDeviceState()
        {
            if (VisionSystemClassLibrary.Enum.DeviceState.Run == devicestate)//运行
            {
                if (bManageToolsSelected)//【MANAGE TOOLS】按下
                {
                    customButtonClose.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【CLOSE】
                }
                else//【MANAGE TOOLS】未按下
                {
                    customButtonClose.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【CLOSE】
                }

                //

                customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【SAVE PRODUCT】
                customButtonLearnSample.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【LEARN SAMPLE】（【UPDATE THRESHOLD】）
                customButtonLiveView.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【LIVE VIEW】
                customButtonLoadSample.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【LOAD SAMPLE】
                customButtonManageTools.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【MANAGE TOOLS】
                customButtonEditTools.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【EDIT TOOLS】
                customButtonLoadReject.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【LOAD REJECT】
                customButtonSubtract.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【-】
                customButtonPlus.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【+】
                customButtonRejectImageIndex.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//剔除图像索引值控件
                customButtonStatusBar.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【STATUS BAR】
                customButtonMeasureTool.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【MEASURE TOOL】

                parameterSettingsPanel.ControlEnabled = false;
                customListTool.ListEnabled = false;
                imageDisplayView.Enabled = false;

                //

                _ShowHandle(false);
                _ResetHandleColor();

                bSizeAdjustedPanelVisible = false;
                sizeAdjustedPanel.Visible = false;
                sizeAdjustedPanel.Location = SizeAdjustedPanelDefaultLocation;

                if (null != iSelectedToolsArray)
                {
                    if (1 < iSelectedToolsArray.Length)//多于1个工具
                    {
                        if (iCurrentToolIndex == iSelectedToolsArray[0])//首页
                        {
                            customButtonPrevious.Visible = false;
                            customButtonNext.Visible = true;
                        }
                        else if (iCurrentToolIndex == iSelectedToolsArray[iSelectedToolsArray.Length - 1])//末页
                        {
                            customButtonPrevious.Visible = true;
                            customButtonNext.Visible = false;
                        }
                        else//其它
                        {
                            customButtonPrevious.Visible = true;
                            customButtonNext.Visible = true;
                        }
                    }
                    else//其它
                    {
                        customButtonPrevious.Visible = false;
                        customButtonNext.Visible = false;
                    }
                }
            }
            else if (VisionSystemClassLibrary.Enum.DeviceState.Stop == devicestate)//停止
            {
                parameterSettingsPanel.ControlEnabled = true;

                imageDisplayView.Enabled = true;

                //

                if (bManageToolsSelected)//按下
                {
                    customButtonClose.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【CLOSE】

                    //

                    customButtonManageTools.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//【MANAGE TOOLS】
                    customButtonEditTools.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【EDIT TOOLS】

                    customListTool.ListEnabled = true;

                    //

                    customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【SAVE PRODUCT】
                    customButtonLearnSample.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【LEARN SAMPLE】（【UPDATE THRESHOLD】）
                    customButtonLiveView.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【LIVE VIEW】
                    customButtonLoadSample.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【LOAD SAMPLE】
                    customButtonLoadReject.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【LOAD REJECT】
                    customButtonSubtract.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【-】
                    customButtonPlus.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【+】
                    customButtonRejectImageIndex.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//剔除图像索引值控件
                    customButtonStatusBar.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【STATUS BAR】
                }
                else//未按下
                {
                    customButtonClose.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【CLOSE】

                    //

                    customButtonManageTools.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【MANAGE TOOLS】
                    customButtonEditTools.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【EDIT TOOLS】

                    //

                    if (bSaveProduct)//参数被修改
                    {
                        customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【SAVE PRODUCT】
                    }
                    else//参数未修改
                    {
                        customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【SAVE PRODUCT】
                    }

                    //if (bLearnSampleSelected)//按下
                    //{
                    //    customButtonLearnSample.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//【LEARN SAMPLE】（【UPDATE THRESHOLD】）
                    //}
                    //else//未按下
                    //{
                    //    customButtonLearnSample.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【LEARN SAMPLE】（【UPDATE THRESHOLD】）
                    //}
                    customButtonLearnSample.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【LEARN SAMPLE】（【UPDATE THRESHOLD】）

                    if (bLiveViewSelected)//按下
                    {
                        customButtonLiveView.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//【LIVE VIEW】
                    }
                    else//未按下
                    {
                        customButtonLiveView.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【LIVE VIEW】
                    }

                    if (bLoadSampleSelected)//按下
                    {
                        customButtonLoadSample.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//【LOAD SAMPLE】
                    }
                    else//未按下
                    {
                        customButtonLoadSample.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【LOAD SAMPLE】
                    }

                    if (bManageToolsSelected)//按下
                    {
                        customButtonManageTools.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//【MANAGE TOOLS】
                    }
                    else//未按下
                    {
                        customButtonManageTools.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【MANAGE TOOLS】
                    }

                    if (bLoadRejectSelected)//按下
                    {
                        customButtonLoadReject.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//【LOAD REJECT】

                        customButtonSubtract.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【-】
                        customButtonPlus.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【+】
                        customButtonRejectImageIndex.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//剔除图像索引值控件

                    }
                    else//未按下
                    {
                        customButtonLoadReject.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【LOAD REJECT】

                        customButtonSubtract.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【-】
                        customButtonPlus.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【+】
                        customButtonRejectImageIndex.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//剔除图像索引值控件
                    }

                    if (bStatusBarSelected)//按下
                    {
                        customButtonStatusBar.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//【STATUS BAR】
                    }
                    else//未按下
                    {
                        customButtonStatusBar.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【STATUS BAR】
                    }

                    if (bMeasureToolSelected)//按下
                    {
                        customButtonMeasureTool.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//【MEASURE TOOL】
                    }
                    else//未按下
                    {
                        customButtonMeasureTool.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【MEASURE TOOL】
                    }

                    //

                    customListTool.ListEnabled = true;
                }
            }
            else//其它
            {
                //不执行操作
            }
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：点击【CLOSE】按钮的操作
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        private void _Close()
        {
            cameraTemp._CopyTo(ref camera);

            bClickCloseButton = false;

            bSaveProduct = false;

            customButtonSaveProduct.BitmapWhole = VisionSystemControlLibrary.Properties.Resources.Button;//【SAVE PRODUCT】
            customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【SAVE PRODUCT】

            parameterSettingsPanel.ControlEnabled = true;
            customListTool.ListEnabled = true;

            //bLearnSampleSelected = false;
            customButtonLearnSample.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【LEARN SAMPLE】（【UPDATE THRESHOLD】）

            bLiveViewSelected = false;
            customButtonLiveView.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【LIVE VIEW】

            bLoadSampleSelected = false;
            customButtonLoadSample.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【LOAD SAMPLE】

            bManageToolsSelected = false;
            customButtonManageTools.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【MANAGE TOOLS】

            bLoadRejectSelected = false;
            customButtonLoadReject.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【LOAD REJECT】

            bStatusBarSelected = false;
            customButtonStatusBar.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【STATUS BAR】
            imageDisplayView.ShowTitle = true;

            customButtonSubtract.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【-】
            customButtonPlus.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【+】

            iRejectImageIndex = RejectImageMaxNumber - 1;
            customButtonRejectImageIndex.Chinese_TextDisplay = new String[1] { (iRejectImageIndex + 1).ToString() };//设置显示的文本
            customButtonRejectImageIndex.English_TextDisplay = new String[1] { (iRejectImageIndex + 1).ToString() };//设置显示的文本
            customButtonRejectImageIndex.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//剔除图像索引控件

            iImageDisplayClickTime = 0;
            _ShowHandle(false);

            //事件

            if (null != Close_Click)//有效
            {
                Close_Click(this, new CustomEventArgs());
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：学习图像
        // 输入参数：1.bSuccess：是否成功。取值范围：true，成功；false，失败
        //         2.imageInformation：图像信息数据
        //         3.drawingInfo：图像处理信息（绘图）
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _LearnSample(Boolean bSuccess, VisionSystemClassLibrary.Struct.ImageInformation imageInformation)
        {
            if (bSuccess)//成功
            {
                _LearnImageProcessing();//图像学习

                //

                iCompensation_H = imageInformation.Compensation_H;
                iCompensation_V = imageInformation.Compensation_V;

                //

                _UpdateROI();

                _SetImageDisplayTitle(imageInformation);//更新标题栏

                _SetInspectionTime(imageInformation.InspectionTime);

                Update();

                //

                bSaveProduct = true;

                customButtonSaveProduct.BitmapWhole = VisionSystemControlLibrary.Properties.Resources.Button_1;//【SAVE PRODUCT】

                customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【SAVE PRODUCT】

                //显示信息窗口

                GlobalWindows.MessageDisplay_Window.WindowParameter = 78;//窗口特征数值
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言

                //if (1 < customButtonLearnSample.TextGroupNumber)//【LEARN SAMPLE】（【UPDATE THRESHOLD】）
                //{
                //    switch (camera.Tools[iCurrentToolIndex].Type)//工具类型
                //    {
                //        case 0://格子
                //            //
                //            //【LEARN SAMPLE】
                //            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][4];
                //            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][4];
                //            //
                //            break;
                //        case 1://质量
                //            //
                //            //【LEARN SAMPLE】
                //            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][4];
                //            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][4];
                //            //
                //            break;
                //        case 2://直尺
                //            //
                //            //【UPDATE THRESHOLD】
                //            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][7];
                //            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][7];
                //            //
                //            break;
                //        case 3://烟支
                //            //
                //            //【UPDATE THRESHOLD】
                //            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][7];
                //            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][7];
                //            break;
                //            //
                //        case 4://乱烟
                //            //
                //            //【UPDATE THRESHOLD】
                //            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][7];
                //            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][7];
                //            //
                //            break;
                //        case 5://拉线
                //            //
                //            //【UPDATE THRESHOLD】
                //            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][7];
                //            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][7];
                //            //
                //            break;
                //        default:
                //            break;
                //    }
                //}
                //else//【LEARN SAMPLE】
                //{
                    //【LEARN SAMPLE】

                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][4];
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][4];
                //}

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
                //显示信息窗口

                GlobalWindows.MessageDisplay_Window.WindowParameter = 79;//窗口特征数值
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言

                //if (1 < customButtonLearnSample.TextGroupNumber)//【LEARN SAMPLE】（【UPDATE THRESHOLD】）
                //{
                //    switch (camera.Tools[iCurrentToolIndex].Type)//工具类型
                //    {
                //        case 0://格子
                //            //
                //            //【LEARN SAMPLE】
                //            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][5];
                //            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][5];
                //            //
                //            break;
                //        case 1://质量
                //            //
                //            //【LEARN SAMPLE】
                //            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][5];
                //            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][5];
                //            //
                //            break;
                //        case 2://直尺
                //            //
                //            //【UPDATE THRESHOLD】
                //            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][8];
                //            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][8];
                //            //
                //            break;
                //        case 3://烟支
                //            //
                //            //【UPDATE THRESHOLD】
                //            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][8];
                //            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][8];
                //            //
                //            break;
                //        case 4://乱烟
                //            //
                //            //【UPDATE THRESHOLD】
                //            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][8];
                //            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][8];
                //            //
                //            break;
                //        case 5://拉线
                //            //
                //            //【UPDATE THRESHOLD】
                //            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][8];
                //            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][8];
                //            //
                //            break;
                //        default:
                //            break;
                //    }
                //}
                //else//【LEARN SAMPLE】
                //{
                    //【LEARN SAMPLE】

                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][5];
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][5];
                //}

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
        // 功能说明：保存数据
        // 输入参数：1.bSuccess：保存是否成功。取值范围：true，是；false，否
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _SaveProduct(Boolean bSuccess)
        {
            if (bSuccess)//成功
            {
                bSaveProduct = false;

                customButtonSaveProduct.BitmapWhole = VisionSystemControlLibrary.Properties.Resources.Button;//【SAVE PRODUCT】

                customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【SAVE PRODUCT】

                //

                camera._WriteImage(VisionSystemClassLibrary.Enum.ImageInformationType.Sample);//写入图像、图像信息

                camera._SaveTool();
                camera._SaveTolerances();

                File.Copy(camera.DataPath + VisionSystemClassLibrary.Class.Camera.TolerancesFileName, brand.BrandPath + brand.CURRENTBrandName + "\\" + camera.CameraENGName + "\\" + VisionSystemClassLibrary.Class.Camera.TolerancesFileName, true);
                File.Copy(camera.DataPath + VisionSystemClassLibrary.Class.Camera.ToolFileName, brand.BrandPath + brand.CURRENTBrandName + "\\" + camera.CameraENGName + "\\" + VisionSystemClassLibrary.Class.Camera.ToolFileName, true);
                //VisionSystemClassLibrary.Class.System.FileCopyFun(camera.DataPath + VisionSystemClassLibrary.Class.Camera.TolerancesFileName, brand.BrandPath + brand.CURRENTBrandName + "\\" + camera.CameraENGName + "\\" + VisionSystemClassLibrary.Class.Camera.TolerancesFileName);
                //VisionSystemClassLibrary.Class.System.FileCopyFun(camera.DataPath + VisionSystemClassLibrary.Class.Camera.ToolFileName, brand.BrandPath + brand.CURRENTBrandName + "\\" + camera.CameraENGName + "\\" + VisionSystemClassLibrary.Class.Camera.ToolFileName);
                VisionSystemClassLibrary.Class.Brand._CopyDirectory(camera.SampleImagePath.Substring(0, camera.SampleImagePath.Length - 1), brand.BrandPath + brand.CURRENTBrandName + "\\" + camera.CameraENGName + "\\" + VisionSystemClassLibrary.Class.Camera.SampleImagePathName.Substring(0, VisionSystemClassLibrary.Class.Camera.SampleImagePathName.Length - 1));

                //

                camera._CopyTo(ref cameraTemp);

                //显示信息窗口

                GlobalWindows.MessageDisplay_Window.WindowParameter = 40;//窗口特征数值
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][1];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][1];

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
                //显示信息窗口

                GlobalWindows.MessageDisplay_Window.WindowParameter = 41;//窗口特征数值
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][2];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][2];

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
        // 功能说明：用户调用，设备状态改变时（如某一个相机连接或断开），更新页面
        // 输入参数：1.bConnected：设备是否连接。取值范围：true，是；false，否
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _DeviceStateChanged(Boolean bConnected)
        {
            if (bConnected)//连接
            {
                _SetDeviceState();//设备状态
            }
            else//断开
            {
                customButtonClose.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【CLOSE】

                //

                customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【SAVE PRODUCT】
                customButtonLearnSample.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【LEARN SAMPLE】（【UPDATE THRESHOLD】）
                customButtonLiveView.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【LIVE VIEW】
                customButtonLoadSample.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【LOAD SAMPLE】
                customButtonManageTools.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【MANAGE TOOLS】
                customButtonEditTools.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【EDIT TOOLS】
                customButtonLoadReject.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【LOAD REJECT】
                customButtonSubtract.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【-】
                customButtonPlus.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【+】
                customButtonRejectImageIndex.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//剔除图像索引值控件
                customButtonStatusBar.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【STATUS BAR】
                customButtonMeasureTool.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【MEASURE TOOL】

                parameterSettingsPanel.ControlEnabled = false;
                customListTool.ListEnabled = false;
                imageDisplayView.Enabled = false;

                //

                _ShowHandle(false);
                _ResetHandleColor();

                bSizeAdjustedPanelVisible = false;
                sizeAdjustedPanel.Visible = false;
                sizeAdjustedPanel.Location = SizeAdjustedPanelDefaultLocation;

                if (1 < iSelectedToolsArray.Length)//多于1个工具
                {
                    if (iCurrentToolIndex == iSelectedToolsArray[0])//首页
                    {
                        customButtonPrevious.Visible = false;
                        customButtonNext.Visible = true;
                    }
                    else if (iCurrentToolIndex == iSelectedToolsArray[iSelectedToolsArray.Length - 1])//末页
                    {
                        customButtonPrevious.Visible = true;
                        customButtonNext.Visible = false;
                    }
                    else//其它
                    {
                        customButtonPrevious.Visible = true;
                        customButtonNext.Visible = true;
                    }
                }
                else//其它
                {
                    customButtonPrevious.Visible = false;
                    customButtonNext.Visible = false;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：学习后，调用本函数更新公差值，并更新绘制区域
        // 输入参数：1.iGraphIndex：待更新的曲线图数据数组序号（GraphData数组序号）
        //         2.iMin：最小值
        //         3.iMax：最大值
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _SetTolerancesValue(int iGraphIndex, Int32 iMin, Int32 iMax)
        {
            camera.Tolerances.GraphData[iGraphIndex].EffectiveMax_Value = iMax;

            camera.Tolerances.GraphData[iGraphIndex].EffectiveMin_Value = iMin;

            //

            camera.Tools[camera.Tolerances.GraphData[iGraphIndex].ToolsIndex].Min = iMin;
            camera.Tools[camera.Tolerances.GraphData[iGraphIndex].ToolsIndex].Max = iMax;

            camera.Tolerances.GraphData[iGraphIndex].TolerancesGraphDataValue.AdditionalValue = VisionSystemClassLibrary.Class.TolerancesData._SetAdditionalValue(iMin, iMax, camera.Tolerances.GraphData[iGraphIndex].AdditionalValueRatio);
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：图像来源
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _GetImage()
        {
            switch (iImageType)
            {
                case 1://显示处理图像来自实时图像
                    //
                    //imageDisplayView.Information = camera.Live.GraphicsInformation;

                    if (camera.ImageLive != null)
                    {
                        if (camera.Live.GraphicsInformation.Scale >= 1.0)
                        {
                            imageShow = camera.ImageLive.Copy();
                        }
                    }
                    else
                    {
                        imageShow.Bitmap = (Bitmap)bitmapNone.Clone();//图像数据
                    }
                    break;
                    //
                case 2://显示处理图像来自自学习图像
                    //
                    //imageDisplayView.Information = camera.Learn;

                    if (camera.ImageLearn != null)
                    {
                        if (camera.Learn.Scale >= 1.0)
                        {
                            imageShow = camera.ImageLearn.Copy();
                        }
                    }
                    else
                    {
                        imageShow.Bitmap = (Bitmap)bitmapNone.Clone();//图像数据
                    }
                    break;
                    //
                default://3，显示处理图像来自剔除图像
                    //
                    //imageDisplayView.Information = camera.Rejects.GraphicsInformation[iRejectImageIndex];

                    if (camera.ImageReject != null)
                    {
                        if (camera.Rejects.GraphicsInformation[iRejectImageIndex].Scale >= 1.0)
                        {
                            imageShow = camera.ImageReject.Copy();
                        }
                    }
                    else
                    {
                        imageShow.Bitmap = (Bitmap)bitmapNone.Clone();//图像数据
                    }
                    break;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：设置图像控件标题栏
        // 输入参数： 1.imageInformation：图像信息数据
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _SetImageDisplayTitle(VisionSystemClassLibrary.Struct.ImageInformation imageInformation)
        {
            //if (0 <= imageInformation.ToolsIndex && (VisionSystemClassLibrary.Enum.ImageType.Error == imageInformation.Type))//剔除图像
            //{
            //    //不执行操作
            //}
            //else//完好图像
            //{
            //    imageInformation.Name = "OK";//图像信息
            //}

            imageDisplayView.Information = imageInformation;
        }

        //-----------------------------------------------------------------------
        // 功能说明：刷新界面函数
        // 输入参数：1.imageInformation：图像信息数据
        //         2.iImageType_Temp：刷新图像类型。取值范围：1，实时；2，学习；3，剔除；-1，无意义
        //         3.iIndex_RejectImage：剔除图像索引
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _UpdateInterface(VisionSystemClassLibrary.Struct.ImageInformation imageInformation, Int32 iImageType_Temp = -1, Int32 iIndex_RejectImage = 0)
        {
            if (0 <= iImageType_Temp)//有效
            {
                iImageType = iImageType_Temp;
            }

            if (3 == iImageType)//剔除图像
            {
                if (iIndex_RejectImage != iRejectImageIndex)
                {
                    iRejectImageIndex = iIndex_RejectImage;

                    //

                    customButtonRejectImageIndex.Chinese_TextDisplay = new String[1] { (iRejectImageIndex + 1).ToString() };//设置显示的文本
                    customButtonRejectImageIndex.English_TextDisplay = new String[1] { (iRejectImageIndex + 1).ToString() };//设置显示的文本
                }
            }

            //

            iCompensation_H = imageInformation.Compensation_H;
            iCompensation_V = imageInformation.Compensation_V;

            //

            _UpdateROI();

            _SetImageDisplayTitle(imageInformation);//更新标题栏

            _SetInspectionTime(imageInformation.InspectionTime);

            Update();
        }

        //-----------------------------------------------------------------------
        // 功能说明：设置【LEARN SAMPLE】（【UPDATE THRESHOLD】）按钮
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _SetLearnSampleButton()
        {
            if (camera.Tools[iCurrentToolIndex].LearnShowState)//显示【LEARN SAMPLE】（【UPDATE THRESHOLD】）
            {
                if (1 < customButtonLearnSample.TextGroupNumber)//【LEARN SAMPLE】（【UPDATE THRESHOLD】）
                {
                    //
                    customButtonLearnSample.CurrentTextGroupIndex = 1;//【LEARN SAMPLE】
                    //

                    //switch (camera.Tools[iCurrentToolIndex].Type)//工具类型
                    //{
                    //    case 0://格子
                    //        //
                    //        customButtonLearnSample.CurrentTextGroupIndex = 1;//【LEARN SAMPLE】
                    //        //
                    //        break;
                    //    case 1://质量
                    //        //
                    //        customButtonLearnSample.CurrentTextGroupIndex = 1;//【LEARN SAMPLE】
                    //        //
                    //        break;
                    //    case 2://直尺
                    //        //
                    //        customButtonLearnSample.CurrentTextGroupIndex = 0;//【UPDATE THRESHOLD】
                    //        //
                    //        break;
                    //    case 3://烟支
                    //        //
                    //        customButtonLearnSample.CurrentTextGroupIndex = 0;//【UPDATE THRESHOLD】
                    //        //
                    //        break;
                    //    case 4://乱烟
                    //        //
                    //        customButtonLearnSample.CurrentTextGroupIndex = 0;//【UPDATE THRESHOLD】
                    //        //
                    //        break;
                    //    case 5://拉线
                    //        //
                    //        customButtonLearnSample.CurrentTextGroupIndex = 0;//【UPDATE THRESHOLD】
                    //        //
                    //        break;
                    //    default:
                    //        break;
                    //}
                }
                else//【LEARN SAMPLE】
                {
                    //不执行操作
                }

                //

                customButtonLearnSample.Visible = true;
            }
            else//隐藏【LEARN SAMPLE】（【UPDATE THRESHOLD】）
            {
                customButtonLearnSample.Visible = false;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Tools切换时调用函数
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _SelectTool()
        {
            customButtonSelectedCheckValue.Chinese_TextDisplay = new String[1] { (iCurrentToolIndex + 1).ToString() + " - " + camera.Tools[iCurrentToolIndex].ToolsCHNName };//设置显示的文本
            customButtonSelectedCheckValue.English_TextDisplay = new String[1] { (iCurrentToolIndex + 1).ToString() + " - " + camera.Tools[iCurrentToolIndex].ToolsENGName };//设置显示的文本

            //

            _SetToolParameters();

            //

            drawROI.X = camera.Tools[iCurrentToolIndex].WorkAreaPointX;
            drawROI.Y = camera.Tools[iCurrentToolIndex].WorkAreaPointY;
            drawROI.Width = camera.Tools[iCurrentToolIndex].WorkAreaWidth;
            drawROI.Height = camera.Tools[iCurrentToolIndex].WorkAreaHeight;

            _SetSizeAdjustedPanel();

            _SetLearnSampleButton();//设置【LEARN SAMPLE】按钮

            //

            _UpdateROI();
        }

        //-----------------------------------------------------------------------
        // 功能说明：最佳对比颜色
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _BestContrast(double X, double Y, double Z)
        {
            if ((X > Y) && (X > Z))//Blue颜色平均值最大，选择最佳对比颜色为Blue                                  
            {
                labelBestContrastValue.BackColor = System.Drawing.Color.Blue;
            }
            else if ((Y > X) && (Y > Z))//Green颜色平均值最大，选择最佳对比颜色为Green
            {
                labelBestContrastValue.BackColor = System.Drawing.Color.Green;
            }
            else if ((Z > X) && (Z > Y))//Red颜色平均值最大，选择最佳对比颜色为Red
            {
                labelBestContrastValue.BackColor = System.Drawing.Color.Red;
            }
            else//其它
            {
                //不执行操作
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：手柄位置计算
        // 输入参数： rect:处理区域
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _HandleLocation(Rectangle rect)
        {
            Rectangle rectangle = new Rectangle();

            rectangle.X = imageDisplayView.Location.X + (Int32)(rect.X / imageDisplayView.ImageScale);
            rectangle.Y = imageDisplayView.Location.Y + imageDisplayView.RectangleImage.Location.Y + (Int32)(rect.Y / imageDisplayView.ImageScale);
            rectangle.Width = (Int32)(rect.Width / imageDisplayView.ImageScale);
            rectangle.Height = (Int32)(rect.Height / imageDisplayView.ImageScale);

            labelHandleLeftTop.Location = new Point(rectangle.X, rectangle.Y);
            labelHandleTop.Location = new Point(rectangle.X + (rectangle.Width - labelHandleTop.Width) / 2, rectangle.Y);
            labelHandleRightTop.Location = new Point(rectangle.X + rectangle.Width - labelHandleRightTop.Width, rectangle.Y);
            labelHandleLeft.Location = new Point(rectangle.X, rectangle.Y + (rectangle.Height - labelHandleLeft.Height) / 2);
            labelHandleRight.Location = new Point(rectangle.X + rectangle.Width - labelHandleRight.Width, rectangle.Y + (rectangle.Height - labelHandleRight.Height) / 2);
            labelHandleLeftBottom.Location = new Point(rectangle.X, rectangle.Y + rectangle.Height - labelHandleLeftBottom.Height);
            labelHandleBottom.Location = new Point(rectangle.X + (rectangle.Width - labelHandleBottom.Width) / 2, rectangle.Y + rectangle.Height - labelHandleBottom.Height);
            labelHandleRightBottom.Location = new Point(rectangle.X + rectangle.Width - labelHandleRightBottom.Width, rectangle.Y + rectangle.Height - labelHandleRightBottom.Height);
        }

        //-----------------------------------------------------------------------
        // 功能说明：复位手柄背景颜色
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        private void _ResetHandleColor()
        {
            labelHandleLeftTop.BackColor = System.Drawing.Color.Blue;
            labelHandleTop.BackColor = System.Drawing.Color.Blue;
            labelHandleRightTop.BackColor = System.Drawing.Color.Blue;
            labelHandleLeft.BackColor = System.Drawing.Color.Blue;
            labelHandleRight.BackColor = System.Drawing.Color.Blue;
            labelHandleLeftBottom.BackColor = System.Drawing.Color.Blue;
            labelHandleBottom.BackColor = System.Drawing.Color.Blue;
            labelHandleRightBottom.BackColor = System.Drawing.Color.Blue;

            contentalignmentHandleType = ContentAlignment.MiddleCenter;//兴趣区域调整手柄类型（ContentAlignment.MiddleCenter表示无效）
        }

        //-----------------------------------------------------------------------
        // 功能说明：显示或隐藏手柄
        // 输入参数：1.bVisable：显示或隐藏。取值范围：true，显示；false，隐藏
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        private void _ShowHandle(Boolean bVisible)
        {
            bDrawImageProcessingResult = true;

            //

            bHandleVisible = bVisible;

            labelHandleLeftTop.Visible = bVisible;
            labelHandleTop.Visible = bVisible;
            labelHandleRightTop.Visible = bVisible;
            labelHandleLeft.Visible = bVisible;
            labelHandleRight.Visible = bVisible;
            labelHandleLeftBottom.Visible = bVisible;
            labelHandleBottom.Visible = bVisible;
            labelHandleRightBottom.Visible = bVisible;
        }

        //-----------------------------------------------------------------------
        // 功能说明：手柄和控制板状态
        // 输入参数：1.pointValue：源图像坐标
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        private void _SetHandle(Point pointValue)
        {
            if ((!bHandleVisible) && (!bSizeAdjustedPanelVisible))//手柄HIDE，控制板HIDE
            {
                if ((pointValue.X >= drawROI.X) && (pointValue.X <= (drawROI.X + drawROI.Width))
                    && (pointValue.Y >= drawROI.Y) && (pointValue.Y <= (drawROI.Y + drawROI.Height)))//单击轮廓内部
                {
                    pointROI.X = pointValue.X;
                    pointROI.Y = pointValue.Y;

                    //

                    drawROIOriginal = drawROI;
                }
                else//单击轮廓外部
                {
                    //不执行操作
                }
            }
            else if (bHandleVisible && (!bSizeAdjustedPanelVisible))//手柄SHOW、控制板HIDE
            {
                if ((pointValue.X < drawROI.X) || (pointValue.X > (drawROI.X + drawROI.Width))
                    || (pointValue.Y < drawROI.Y) || (pointValue.Y > (drawROI.Y + drawROI.Height)))//单击轮廓外部
                {
                    _ShowHandle(false);
                    _ResetHandleColor();
                }
                else//单击轮廓内部
                {
                    _ResetHandleColor();
                }
            }
            else if (bHandleVisible && bSizeAdjustedPanelVisible)//手柄SHOW，控制板SHOW
            {
                if ((pointValue.X < drawROI.X) || (pointValue.X > (drawROI.X + drawROI.Width))
                    || (pointValue.Y < drawROI.Y) || (pointValue.Y > (drawROI.Y + drawROI.Height)))//单击轮廓外部
                {
                    //不执行操作
                }
                else//单击轮廓内部
                {
                    _ResetHandleColor();

                    //

                    sizeAdjustedPanel.SelectionType = VisionSystemControlLibrary.RegionSelectionType.None;
                }

                _SetSizeAdjustedPanel();
            }
            else//手柄HIDE、控制板SHOW
            {
                if ((pointValue.X < drawROI.X) || (pointValue.X > (drawROI.X + drawROI.Width))
                    || (pointValue.Y < drawROI.Y) || (pointValue.Y > (drawROI.Y + drawROI.Height)))//单击轮廓外部
                {
                    //不执行操作
                }
                else//单击轮廓内部
                {
                    pointROI.X = pointValue.X;
                    pointROI.Y = pointValue.Y;

                    //

                    drawROIOriginal = drawROI;
                }
            }

            //

            _UpdateROI();
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：检查工具是否包含角度算法
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：true，是；false，否
        //----------------------------------------------------------------------
        private Boolean _CheckArithmetic_Angle()
        {
            Boolean bReturn = false;

            //

            if (2 == camera.Tools[iCurrentToolIndex].Type)//直尺
            {
                if (camera.Tools[iCurrentToolIndex].Arithmetic.State[2])//角度，启用
                {
                    bReturn = true;
                }
            }

            //

            return bReturn;
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：更新显示的兴趣区域
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        private void _UpdateROI()
        {
            _GetImage();

            camera._ImageProcess(imageShow);
            camera.Tools[iCurrentToolIndex]._Drawing(imageShow);

            //

            //if (_CheckArithmetic_Angle())//角度
            //{
            //    if (bHandleVisible || sizeAdjustedPanel.Visible)//拖动手柄控件显示
            //    {
            //        imageShow.Draw(new Rectangle(drawROI.X + iCompensation_H, drawROI.Y + iCompensation_V, drawROI.Width, drawROI.Height), new Bgr(0, 255, 0), 1);
            //    }
            //}
            //else//其它
            //{
            //    imageShow.Draw(new Rectangle(drawROI.X + iCompensation_H, drawROI.Y + iCompensation_V, drawROI.Width, drawROI.Height), new Bgr(0, 255, 0), 1);
            //}

            //

            if (bMeasureToolSelected && 0 == iImageDisplayClickTime)//【MEASURE TOOL】按下，显示测量工具
            {
                imageShow.Draw(new LineSegment2D(pointFirst_MeasureTool, pointSecond_MeasureTool), new Bgr(0, 255, 255), 1);
            }

            //

            imageDisplayView.BitmapDisplay = imageShow.ToBitmap();

            //

            if (bHandleVisible || sizeAdjustedPanel.Visible)//拖动手柄控件显示
            {
                _HandleLocation(drawROI);
            }

            //

            Update();
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：移动手柄改变区域大小
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _ROIChange()
        {
            Point pointImageDisplayControl = imageDisplayView.PictureBoxControl.PointToScreen(new Point(0, 0));//屏幕坐标

            Point pointMouse = new Point();//光标位置

            Rectangle rectangleROI_Temp = drawROI;//兴趣区域临时变量

            Rectangle rectangleImage_Temp = new Rectangle(0, 0, 0, 0);//图像区域
            if (null != imageDisplayView.BitmapDisplay)//有效
            {
                rectangleImage_Temp = new Rectangle(new Point(0, 0), new Size(imageDisplayView.BitmapDisplay.Width, imageDisplayView.BitmapDisplay.Height));
            }

            //

            if (bHandleVisible)//显示
            {
                switch (contentalignmentHandleType)
                {
                    case ContentAlignment.TopCenter://拖动手柄上中按钮
                        //

                        pointMouse.Y = (Int32)((Control.MousePosition.Y - pointMouse_Move.Y - pointImageDisplayControl.Y - imageDisplayView.RectangleImage.Location.Y) * imageDisplayView.ImageScale);

                        rectangleROI_Temp.Height = Math.Abs(pointMouse.Y - drawROIOriginal.Bottom);

                        if (pointMouse.Y > drawROIOriginal.Bottom)
                        {
                            rectangleROI_Temp.Y = drawROIOriginal.Bottom;
                        }
                        else
                        {
                            rectangleROI_Temp.Y = drawROIOriginal.Bottom - rectangleROI_Temp.Height;
                        }

                        break;
                        //
                    case ContentAlignment.MiddleLeft://拖动手柄中左按钮
                        //

                        pointMouse.X = (Int32)((Control.MousePosition.X - pointMouse_Move.X - pointImageDisplayControl.X) * imageDisplayView.ImageScale);

                        if (pointMouse.X < drawROIOriginal.X + drawROIOriginal.Width)
                        {
                            rectangleROI_Temp.Width = Math.Abs(pointMouse.X - (drawROIOriginal.X + drawROIOriginal.Width));

                            rectangleROI_Temp.X = pointMouse.X;
                        }
                        else
                        {
                            rectangleROI_Temp.Width = Math.Abs(pointMouse.X - drawROIOriginal.X);

                            rectangleROI_Temp.X = drawROIOriginal.X;
                        }

                        break;
                        //
                    case ContentAlignment.MiddleRight://拖动手柄中右按钮
                        //

                        pointMouse.X = (Int32)((Control.MousePosition.X + pointMouse_Move.X - pointImageDisplayControl.X) * imageDisplayView.ImageScale);

                        rectangleROI_Temp.Width = Math.Abs(pointMouse.X - drawROIOriginal.X);
                        
                        if (pointMouse.X < drawROIOriginal.X)
                        {
                            rectangleROI_Temp.X = drawROIOriginal.X - rectangleROI_Temp.Width;
                        }
                        else
                        {
                            rectangleROI_Temp.X = drawROIOriginal.X;
                        }

                        break;
                        //
                    case ContentAlignment.BottomCenter://拖动手柄下中按钮
                        //

                        pointMouse.Y = (Int32)((Control.MousePosition.Y + pointMouse_Move.Y - pointImageDisplayControl.Y - imageDisplayView.RectangleImage.Location.Y) * imageDisplayView.ImageScale);

                        rectangleROI_Temp.Height = Math.Abs(pointMouse.Y - drawROIOriginal.Y);

                        if (pointMouse.Y < drawROIOriginal.Y)
                        {
                            rectangleROI_Temp.Y = drawROIOriginal.Y - rectangleROI_Temp.Height;
                        }
                        else
                        {
                            rectangleROI_Temp.Y = drawROIOriginal.Y;
                        }

                        break;
                        //
                    case ContentAlignment.TopLeft://拖动手柄上左按钮
                        //

                        pointMouse.X = (Int32)((Control.MousePosition.X - pointMouse_Move.X - pointImageDisplayControl.X) * imageDisplayView.ImageScale);
                        pointMouse.Y = (Int32)((Control.MousePosition.Y - pointMouse_Move.Y - pointImageDisplayControl.Y - imageDisplayView.RectangleImage.Location.Y) * imageDisplayView.ImageScale);

                        rectangleROI_Temp.Height = Math.Abs(pointMouse.Y - drawROIOriginal.Bottom);

                        if (pointMouse.X < drawROIOriginal.X + drawROIOriginal.Width)
                        {
                            rectangleROI_Temp.Width = Math.Abs(pointMouse.X - (drawROIOriginal.X + drawROIOriginal.Width));

                            rectangleROI_Temp.X = pointMouse.X;
                        }
                        else
                        {
                            rectangleROI_Temp.Width = Math.Abs(pointMouse.X - drawROIOriginal.X);

                            rectangleROI_Temp.X = drawROIOriginal.X;
                        }

                        if (pointMouse.Y > drawROIOriginal.Bottom)
                        {
                            rectangleROI_Temp.Y = drawROIOriginal.Bottom;
                        }
                        else
                        {
                            rectangleROI_Temp.Y = drawROIOriginal.Bottom - rectangleROI_Temp.Height;
                        }

                        break;
                        //
                    case ContentAlignment.TopRight://拖动手柄上右按钮
                        //

                        pointMouse.X = (Int32)((Control.MousePosition.X + pointMouse_Move.X - pointImageDisplayControl.X) * imageDisplayView.ImageScale);
                        pointMouse.Y = (Int32)((Control.MousePosition.Y - pointMouse_Move.Y - pointImageDisplayControl.Y - imageDisplayView.RectangleImage.Location.Y) * imageDisplayView.ImageScale);

                        rectangleROI_Temp.Width = Math.Abs(pointMouse.X - drawROIOriginal.X);
                        rectangleROI_Temp.Height = Math.Abs(pointMouse.Y - drawROIOriginal.Bottom);

                        if (pointMouse.X < drawROIOriginal.X)
                        {
                            rectangleROI_Temp.X = drawROIOriginal.X - rectangleROI_Temp.Width;
                        }
                        else
                        {
                            rectangleROI_Temp.X = drawROIOriginal.X;
                        }

                        if (pointMouse.Y > drawROIOriginal.Bottom)
                        {
                            rectangleROI_Temp.Y = drawROIOriginal.Bottom;
                        }
                        else
                        {
                            rectangleROI_Temp.Y = drawROIOriginal.Bottom - rectangleROI_Temp.Height;
                        }

                        break;
                        //
                    case ContentAlignment.BottomLeft://拖动手柄下左按钮
                        //

                        pointMouse.X = (Int32)((Control.MousePosition.X - pointMouse_Move.X - pointImageDisplayControl.X) * imageDisplayView.ImageScale);
                        pointMouse.Y = (Int32)((Control.MousePosition.Y + pointMouse_Move.Y - pointImageDisplayControl.Y - imageDisplayView.RectangleImage.Location.Y) * imageDisplayView.ImageScale);

                        rectangleROI_Temp.Height = Math.Abs(pointMouse.Y - drawROIOriginal.Y);

                        if (pointMouse.X < drawROIOriginal.X + drawROIOriginal.Width)
                        {
                            rectangleROI_Temp.Width = Math.Abs(pointMouse.X - (drawROIOriginal.X + drawROIOriginal.Width));

                            rectangleROI_Temp.X = pointMouse.X;
                        }
                        else
                        {
                            rectangleROI_Temp.Width = Math.Abs(pointMouse.X - drawROIOriginal.X);

                            rectangleROI_Temp.X = drawROIOriginal.X;
                        }

                        if (pointMouse.Y < drawROIOriginal.Y)
                        {
                            rectangleROI_Temp.Y = drawROIOriginal.Y - rectangleROI_Temp.Height;
                        }
                        else
                        {
                            rectangleROI_Temp.Y = drawROIOriginal.Y;
                        }

                        break;
                        //
                    case ContentAlignment.BottomRight://拖动手柄下右按钮
                        //

                        pointMouse.X = (Int32)((Control.MousePosition.X + pointMouse_Move.X - pointImageDisplayControl.X) * imageDisplayView.ImageScale);
                        pointMouse.Y = (Int32)((Control.MousePosition.Y + pointMouse_Move.Y - pointImageDisplayControl.Y - imageDisplayView.RectangleImage.Location.Y) * imageDisplayView.ImageScale);

                        rectangleROI_Temp.Width = Math.Abs(pointMouse.X - drawROIOriginal.X);
                        rectangleROI_Temp.Height = Math.Abs(pointMouse.Y - drawROIOriginal.Y);

                        if (pointMouse.X < drawROIOriginal.X)
                        {
                            rectangleROI_Temp.X = drawROIOriginal.X - rectangleROI_Temp.Width;
                        }
                        else
                        {
                            rectangleROI_Temp.X = drawROIOriginal.X;
                        }

                        if (pointMouse.Y < drawROIOriginal.Y)
                        {
                            rectangleROI_Temp.Y = drawROIOriginal.Y - rectangleROI_Temp.Height;
                        }
                        else
                        {
                            rectangleROI_Temp.Y = drawROIOriginal.Y;
                        }

                        break;
                        //
                    default:
                        break;
                }

                //

                if (rectangleImage_Temp.Contains(rectangleROI_Temp))//有效
                {
                    drawROI = rectangleROI_Temp;

                    //

                    camera.Tools[iCurrentToolIndex].WorkAreaPointX = Convert.ToUInt16(Math.Abs(drawROI.X));
                    camera.Tools[iCurrentToolIndex].WorkAreaPointY = Convert.ToUInt16(Math.Abs(drawROI.Y));
                    camera.Tools[iCurrentToolIndex].WorkAreaWidth = Convert.ToUInt16(Math.Abs(drawROI.Width));
                    camera.Tools[iCurrentToolIndex].WorkAreaHeight = Convert.ToUInt16(Math.Abs(drawROI.Height));

                    //

                    _LearnToolProcessing();

                    //

                    _UpdateROI();
                }
            }
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：控制面板改变函数
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _SetSizeAdjustedPanel()
        {
            Rectangle rectangleImage_Temp = new Rectangle(0, 0, 0, 0);//图像区域
            if (null != imageDisplayView.BitmapDisplay)//有效
            {
                rectangleImage_Temp = new Rectangle(new Point(0, 0), new Size(imageDisplayView.BitmapDisplay.Width, imageDisplayView.BitmapDisplay.Height));
            }

            //

            sizeAdjustedPanel.HandleType = contentalignmentHandleType;

            sizeAdjustedPanel.RectangleMaxROI = rectangleImage_Temp;

            sizeAdjustedPanel.RectangleROI = drawROI;
        }

        //事件

        //-----------------------------------------------------------------------
        // 功能说明：QualityCheckControl窗口加载函数
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void QualityCheckControl_Load(object sender, EventArgs e)
        {
            //不执行操作
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【返回】按钮事件，执行相应的操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonClose_CustomButton_Click(object sender, EventArgs e)
        {
            if (bSaveProduct)//参数被修改
            {
                bClickCloseButton = true;//点击了【CLOSE】按钮

                GlobalWindows.MessageDisplay_Window.WindowParameter = 39;//窗口特征数值
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
            else//参数未被修改
            {
                _Close();
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【SAVE PRODUCT】按钮事件，执行相应的操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonSaveProduct_CustomButton_Click(object sender, EventArgs e)
        {
            if (bSaveProduct)//参数被修改
            {
                GlobalWindows.MessageDisplay_Window.WindowParameter = 39;//窗口特征数值
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
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【LEARN SAMPLE】（【UPDATE THRESHOLD】）按钮事件，执行相应的操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonLearnSample_CustomButton_Click(object sender, EventArgs e)
        {
            //bLearnSampleSelected = !bLearnSampleSelected;

            ////

            //if (bLearnSampleSelected)//按下
            //{
            //    bLiveViewSelected = false;
            //    customButtonLiveView.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【LIVE VIEW】

            //    bLoadSampleSelected = false;
            //    customButtonLoadSample.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【LOAD SAMPLE】

            //    bManageToolsSelected = false;
            //    customButtonManageTools.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【MANAGE TOOLS】

            //    bLoadRejectSelected = false;
            //    customButtonLoadReject.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【LOAD REJECT】

            //    customButtonSubtract.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【-】
            //    customButtonPlus.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【+】

            //    customButtonRejectImageIndex.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//剔除图像索引控件
            //}
            //else//未按下
            //{
            //    //不执行操作
            //}

            //

            GlobalWindows.MessageDisplay_Window.WindowParameter = 77;//窗口特征数值
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.OkCancel;//包含【确定】和【取消】按钮
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言

            //if (1 < customButtonLearnSample.TextGroupNumber)//【LEARN SAMPLE】（【UPDATE THRESHOLD】）
            //{
            //    switch (camera.Tools[iCurrentToolIndex].Type)//工具类型
            //    {
            //        case 0://格子
            //            //
            //            //【LEARN SAMPLE】
            //            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][3] + "？";
            //            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][3] + "？";
            //            //
            //            break;
            //        case 1://质量
            //            //
            //            //【LEARN SAMPLE】
            //            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][3] + "？";
            //            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][3] + "？";
            //            //
            //            break;
            //        case 2://直尺
            //            //
            //            //【UPDATE THRESHOLD】
            //            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][6] + "？";
            //            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][6] + "？";
            //            //
            //            break;
            //        case 3://烟支
            //            //
            //            //【UPDATE THRESHOLD】
            //            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][6] + "？";
            //            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][6] + "？";
            //            //
            //            break;
            //        case 4://乱烟
            //            //
            //            //【UPDATE THRESHOLD】
            //            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][6] + "？";
            //            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][6] + "？";
            //            //
            //            break;
            //        case 5://拉线
            //            //
            //            //【UPDATE THRESHOLD】
            //            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][6] + "？";
            //            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][6] + "？";
            //            //
            //            break;
            //        default:
            //            break;
            //    }
            //}
            //else//【LEARN SAMPLE】
            //{
                //【LEARN SAMPLE】

                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][3] + "？";
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][3] + "？";
            //}

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
        // 功能说明：点击【LIVE VIEW】按钮事件，执行相应的操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonLiveView_CustomButton_Click(object sender, EventArgs e)
        {
            bLiveViewSelected = !bLiveViewSelected;//【LIVE VIEW】

            //

            if (bLiveViewSelected)//按下
            {
                //bLearnSampleSelected = false;
                //customButtonLearnSample.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【LEARN SAMPLE】（【UPDATE THRESHOLD】）

                bLoadSampleSelected = false;
                customButtonLoadSample.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【LOAD SAMPLE】

                bManageToolsSelected = false;
                customButtonManageTools.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【MANAGE TOOLS】

                bLoadRejectSelected = false;
                customButtonLoadReject.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【LOAD REJECT】

                customButtonSubtract.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【-】
                customButtonPlus.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【+】

                customButtonRejectImageIndex.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//剔除图像索引控件
            } 
            else//未按下
            {
                //不执行操作
            }

            //事件

            if (null != LiveView_Click)//有效
            {
                LiveView_Click(this, e);
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【LOAD SAMPLE】按钮事件，执行相应的操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonLoadSample_CustomButton_Click(object sender, EventArgs e)
        {
            bLoadSampleSelected = !bLoadSampleSelected;//【LOAD SAMPLE】

            //

            if (bLoadSampleSelected)//按下
            {
                //bLearnSampleSelected = false;
                //customButtonLearnSample.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【LEARN SAMPLE】（【UPDATE THRESHOLD】）

                bLiveViewSelected = false;
                customButtonLiveView.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【LIVE VIEW】

                bManageToolsSelected = false;
                customButtonManageTools.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【MANAGE TOOLS】

                bLoadRejectSelected = false;
                customButtonLoadReject.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【LOAD REJECT】

                customButtonSubtract.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【-】
                customButtonPlus.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【+】

                customButtonRejectImageIndex.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//剔除图像索引控件
            }
            else//未按下
            {
                //不执行操作
            }

            //

            iImageType = 2;

            _UpdateROI();

            //事件

            if (null != LoadSample_Click)//有效
            {
                LoadSample_Click(this, e);
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【MANAGE TOOLS】按钮事件，执行相应的操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonManageTools_CustomButton_Click(object sender, EventArgs e)
        {
            bManageToolsSelected = !bManageToolsSelected;//【MANAGE TOOLS】

            //

            if (bManageToolsSelected)//按下
            {
                customButtonClose.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【CLOSE】

                customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【SAVE PRODUCT】
                customButtonLearnSample.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【LEARN SAMPLE】（【UPDATE THRESHOLD】）
                customButtonLiveView.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【LIVE VIEW】
                customButtonLoadSample.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【LOAD SAMPLE】
                customButtonLoadReject.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【LOAD REJECT】
                customButtonSubtract.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【-】
                customButtonPlus.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【+】
                customButtonRejectImageIndex.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//剔除图像索引控件
                customButtonStatusBar.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【STATUS BAR】

                //

                _ShowHandle(false);
                _ResetHandleColor();

                //

                bSizeAdjustedPanelVisible = false;

                sizeAdjustedPanel.Location = SizeAdjustedPanelDefaultLocation;

                customListTool.Visible = true;//显示Tool Selection面板
                parameterSettingsPanel.Visible = false;//隐藏工具参数设置面板
                sizeAdjustedPanel.Visible = false;//隐藏控制面板

                customButtonEditTools.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【EDIT TOOLS】

                if (1 < customListTool.TotalPage)//大于1页
                {
                    customButtonPrevious.Visible = true;//显示【Previous】按钮
                    customButtonNext.Visible = true;//显示【Next】按钮
                }
                else//一页
                {
                    customButtonPrevious.Visible = false;//隐藏【Previous】按钮
                    customButtonNext.Visible = false;//隐藏【Next】按钮
                }
            }
            else//未按下
            {
                customButtonClose.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【CLOSE】

                //

                if (bSaveProduct)//参数被修改
                {
                    customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【SAVE PRODUCT】
                }
                else//参数未修改
                {
                    customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【SAVE PRODUCT】
                }
                
                //if (bLearnSampleSelected)//按下
                //{
                //    customButtonLearnSample.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//【LEARN SAMPLE】（【UPDATE THRESHOLD】）
                //} 
                //else//未按下
                //{
                //    customButtonLearnSample.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【LEARN SAMPLE】（【UPDATE THRESHOLD】）
                //}
                customButtonLearnSample.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【LEARN SAMPLE】（【UPDATE THRESHOLD】）

                if (bLiveViewSelected)//按下
                {
                    customButtonLiveView.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//【LIVE VIEW】
                } 
                else//未按下
                {
                    customButtonLiveView.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【LIVE VIEW】
                }

                if (bLoadSampleSelected)//按下
                {
                    customButtonLoadSample.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//【LOAD SAMPLE】
                }
                else//未按下
                {
                    customButtonLoadSample.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【LOAD SAMPLE】
                }

                if (bLoadRejectSelected)//按下
                {
                    customButtonLoadReject.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//【LOAD REJECT】

                    customButtonSubtract.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【-】
                    customButtonPlus.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【+】
                    customButtonRejectImageIndex.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//剔除图像索引控件
                }
                else//未按下
                {
                    customButtonLoadReject.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【LOAD REJECT】
                }

                if (bStatusBarSelected)//按下
                {
                    customButtonStatusBar.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//【STATUS BAR】
                }
                else//未按下
                {
                    customButtonStatusBar.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【STATUS BAR】
                }

                customButtonEditTools.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【EDIT TOOLS】

                //

                customListTool.Visible = false;//隐藏Tool Selection面板

                parameterSettingsPanel.Visible = true;//显示工具参数设置面板

                //

                _SetSelectedTools();//设置选择的工具

                if (iCurrentToolIndex < iSelectedToolsArray[0])//若当前显示的序号小于第一个有效工具，则显示向后选择按钮
                {
                    iCurrentToolIndex = iSelectedToolsArray[0];
                    iCurrentIndex_SelectedToolsArray = 0;
                    _SelectTool();

                    //

                    if (1 < iSelectedToolsArray.Length)//多于1个工具
                    {
                        customButtonPrevious.Visible = false;
                        customButtonNext.Visible = true;
                    } 
                    else//其它
                    {
                        customButtonPrevious.Visible = false;
                        customButtonNext.Visible = false;
                    }
                }
                else if (iCurrentToolIndex == iSelectedToolsArray[0])//若当前显示的是第一个有效工具，则隐藏向前选择按钮，显示向后选择按钮
                {
                    if (1 < iSelectedToolsArray.Length)//多于1个工具
                    {
                        customButtonPrevious.Visible = false;
                        customButtonNext.Visible = true;
                    }
                    else//其它
                    {
                        customButtonPrevious.Visible = false;
                        customButtonNext.Visible = false;
                    }
                }
                else if (iCurrentToolIndex == iSelectedToolsArray[iSelectedToolsArray.Length - 1])//若当前显示的是最后一个有效工具，则隐藏向后选择按钮，显示向前选择按钮
                {
                    iCurrentIndex_SelectedToolsArray = (Byte)(iSelectedToolsArray.Length - 1);

                    //

                    if (1 < iSelectedToolsArray.Length)//多于1个工具
                    {
                        customButtonPrevious.Visible = true;
                        customButtonNext.Visible = false;
                    }
                    else//其它
                    {
                        customButtonPrevious.Visible = false;
                        customButtonNext.Visible = false;
                    }
                }
                else if (iCurrentToolIndex > iSelectedToolsArray[iSelectedToolsArray.Length - 1])//若当前显示的序号大于最后一个有效工具，则显示向前选择按钮
                {
                    iCurrentToolIndex = iSelectedToolsArray[iSelectedToolsArray.Length - 1];
                    iCurrentIndex_SelectedToolsArray = (Byte)(iSelectedToolsArray.Length - 1);
                    _SelectTool();

                    //

                    if (1 < iSelectedToolsArray.Length)//多于1个工具
                    {
                        customButtonPrevious.Visible = true;
                        customButtonNext.Visible = false;
                    }
                    else//其它
                    {
                        customButtonPrevious.Visible = false;
                        customButtonNext.Visible = false;
                    }
                }
                else//若当前显示的不是第一个有效工具，则显示向前选择按钮
                {
                    Int32 i = 0;//循环控制变量

                    for (i = 0; i < iSelectedToolsArray.Length; i++)//获得当前工具的index
                    {
                        if (iCurrentToolIndex == iSelectedToolsArray[i])
                        {
                            iCurrentIndex_SelectedToolsArray = i;
                            break;
                        }
                    }

                    //

                    if (1 < iSelectedToolsArray.Length)//多于1个工具
                    {
                        customButtonPrevious.Visible = true;
                        customButtonNext.Visible = true;
                    }
                    else//其它
                    {
                        customButtonPrevious.Visible = false;
                        customButtonNext.Visible = false;
                    }
                }

                if (!camera.Tools[iCurrentToolIndex].ToolState)                  //若当前工具没被选中，则显示下一个工具
                {
                    iCurrentToolIndex = iSelectedToolsArray[iCurrentIndex_SelectedToolsArray];
                    _SelectTool();
                }
            }

            //事件

            if (null != ManageTools_Click)//有效
            {
                ManageTools_Click(this, e);
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【EDIT TOOLS】按钮事件，执行相应的操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonEditTools_CustomButton_Click(object sender, EventArgs e)
        {

        }

        //----------------------------------------------------------------------
        // 功能说明：点击【LOAD REJECT】按钮事件，执行相应的操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonLoadReject_CustomButton_Click(object sender, EventArgs e)
        {
            bLoadRejectSelected = !bLoadRejectSelected;//【LOAD REJECT】

            //

            if (bLoadRejectSelected)//按下
            {
                //bLearnSampleSelected = false;
                //customButtonLearnSample.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【LEARN SAMPLE】（【UPDATE THRESHOLD】）

                bLiveViewSelected = false;
                customButtonLiveView.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【LIVE VIEW】

                bManageToolsSelected = false;
                customButtonManageTools.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【MANAGE TOOLS】

                bLoadSampleSelected = false;
                customButtonLoadSample.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【LOAD SAMPLE】

                //

                customButtonSubtract.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【-】
                customButtonPlus.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【+】

                customButtonRejectImageIndex.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//剔除图像索引控件
            }
            else//未按下
            {
                customButtonSubtract.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【-】
                customButtonPlus.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【+】

                customButtonRejectImageIndex.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//剔除图像索引控件
            }

            //事件

            if (null != LoadReject_Click)//有效
            {
                LoadReject_Click(this, e);
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：点击【-】按钮事件，执行相应的操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonSubtract_CustomButton_Click(object sender, EventArgs e)
        {
            if (0 < iRejectImageIndex)
            {
                iRejectImageIndex--;

                //

                customButtonRejectImageIndex.Chinese_TextDisplay = new String[1] { (iRejectImageIndex + 1).ToString() };//设置显示的文本
                customButtonRejectImageIndex.English_TextDisplay = new String[1] { (iRejectImageIndex + 1).ToString() };//设置显示的文本
            }

            //事件

            if (null != LoadReject_ImageSelect_Click)//有效
            {
                LoadReject_ImageSelect_Click(this, e);
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
            if (iRejectImageIndex < RejectImageMaxNumber - 1)
            {
                iRejectImageIndex++;

                //

                customButtonRejectImageIndex.Chinese_TextDisplay = new String[1] { (iRejectImageIndex + 1).ToString() };//设置显示的文本
                customButtonRejectImageIndex.English_TextDisplay = new String[1] { (iRejectImageIndex + 1).ToString() };//设置显示的文本
            }

            //事件

            if (null != LoadReject_ImageSelect_Click)//有效
            {
                LoadReject_ImageSelect_Click(this, e);
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：点击【STATUS BAR】按钮事件，执行相应的操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonStatusBar_CustomButton_Click(object sender, EventArgs e)
        {
            bStatusBarSelected = !bStatusBarSelected;

            imageDisplayView.ShowTitle = !bStatusBarSelected;
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：点击【Previous】按钮事件，执行相应的操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonPrevious_CustomButton_Click(object sender, EventArgs e)
        {
            if (bManageToolsSelected)//【MANAGE TOOL】按下
            {
                customListTool._ClickPage(true);//翻页
            }
            else//【MANAGE TOOL】未按下
            {
                _ShowHandle(false);
                _ResetHandleColor();

                bSizeAdjustedPanelVisible = false;
                sizeAdjustedPanel.Visible = false;
                sizeAdjustedPanel.Location = SizeAdjustedPanelDefaultLocation;

                //

                if (iCurrentIndex_SelectedToolsArray > 0)
                {
                    iCurrentIndex_SelectedToolsArray--;
                    iCurrentToolIndex = iSelectedToolsArray[iCurrentIndex_SelectedToolsArray];
                }

                //

                _SelectTool();//显示前一个有效工具及相关处理内容                              

                //

                if (!(customButtonNext.Visible))//未显示【Next Tool】按钮
                {
                    customButtonNext.Visible = true;//显示【Next Tool】按钮
                }

                if (iCurrentToolIndex == iSelectedToolsArray[0])//首页
                {
                    customButtonPrevious.Visible = false;//隐藏【Previous Tool】按钮
                }

                //事件

                if (null != ToolChanged)//有效
                {
                    ToolChanged(this, new CustomEventArgs());
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【Next】按钮事件，执行相应的操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonNext_CustomButton_Click(object sender, EventArgs e)
        {
            if (bManageToolsSelected)//【MANAGE TOOL】按下
            {
                customListTool._ClickPage(false);//翻页
            }
            else//【MANAGE TOOL】未按下
            {
                _ShowHandle(false);
                _ResetHandleColor();

                bSizeAdjustedPanelVisible = false;
                sizeAdjustedPanel.Visible = false;
                sizeAdjustedPanel.Location = SizeAdjustedPanelDefaultLocation;

                //

                if (iCurrentIndex_SelectedToolsArray < iSelectedToolsArray.Length - 1)
                {
                    iCurrentIndex_SelectedToolsArray++;
                    iCurrentToolIndex = iSelectedToolsArray[iCurrentIndex_SelectedToolsArray];
                }

                //

                _SelectTool();//显示下一个有效工具及相关处理内容

                //

                if (!(customButtonPrevious.Visible))//未显示【Previous Tool】按钮
                {
                    customButtonPrevious.Visible = true;//显示【Previous Tool】按钮
                }

                if (iCurrentToolIndex == iSelectedToolsArray[iSelectedToolsArray.Length - 1])//末页
                {
                    customButtonNext.Visible = false;//隐藏【Next Tool】按钮
                }

                //事件

                if (null != ToolChanged)//有效
                {
                    ToolChanged(this, new CustomEventArgs());
                }
            }
        }
        
        //

        //----------------------------------------------------------------------
        // 功能说明：点击【MEASURE TOOL】按钮事件，执行相应的操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonMeasureTool_CustomButton_Click(object sender, EventArgs e)
        {
            bMeasureToolSelected = !bMeasureToolSelected;

            iImageDisplayClickTime = 0;

            if (bMeasureToolSelected)//按下
            {
                customButtonDeltaXText.Visible = true;//Delta X文本
                customButtonDeltaXValue.Visible = true;//Delta X数值
                customButtonDeltaYText.Visible = true;//Delta Y文本
                customButtonDeltaYValue.Visible = true;//Delta Y数值
                customButtonBestContrastText.Visible = true;//Best Contrast文本
                labelBestContrastValue.Visible = true;//Best Contrast数值
            }
            else//未按下
            {
                customButtonDeltaXText.Visible = false;//Delta X文本
                customButtonDeltaXValue.Visible = false;//Delta X数值
                customButtonDeltaYText.Visible = false;//Delta Y文本
                customButtonDeltaYValue.Visible = false;//Delta Y数值
                customButtonBestContrastText.Visible = false;//Best Contrast文本
                labelBestContrastValue.Visible = false;//Best Contrast数值
            }

            _UpdateROI();
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：双击图像显示控件的事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void imageDisplayView_Control_DoubleClick(object sender, EventArgs e)
        {
            if (!bManageToolsSelected)//【MANAGE TOOLS】未按下
            {
                CustomEventArgs customeventargs = (CustomEventArgs)e;

                Point pointValue = new Point(customeventargs.IntValue[0], customeventargs.IntValue[1]);

                //

                if (!bSizeAdjustedPanelVisible)//控制板不可见
                {
                    if ((pointValue.X >= drawROI.X) && (pointValue.X <= (drawROI.X + drawROI.Width))
                       && (pointValue.Y >= drawROI.Y) && (pointValue.Y <= (drawROI.Y + drawROI.Height)))  //双击轮廓内部
                    {
                        sizeAdjustedPanel.SelectionType = VisionSystemControlLibrary.RegionSelectionType.None;

                        if (sizeAdjustedPanel.ShowHideButton)//【SHOW】
                        {
                            //不执行操作
                        } 
                        else//【HIDE】
                        {
                            _ShowHandle(true);
                            _HandleLocation(drawROI);
                            _ResetHandleColor();
                        }

                        //

                        bSizeAdjustedPanelVisible = true;
                        sizeAdjustedPanel.Visible = true;

                        //

                        drawROIOriginal = drawROI;

                        _SetSizeAdjustedPanel();

                        _UpdateROI();
                    }
                }
                else//控制板可见
                {
                    if ((pointValue.X < drawROI.X) || (pointValue.X > (drawROI.X + drawROI.Width))
                       || (pointValue.Y < drawROI.Y) || (pointValue.Y > (drawROI.Y + drawROI.Height)))//双击轮廓外部
                    {
                        _ShowHandle(false);
                        _ResetHandleColor();

                        //

                        bSizeAdjustedPanelVisible = false;
                        sizeAdjustedPanel.Visible = false;
                        sizeAdjustedPanel.Location = SizeAdjustedPanelDefaultLocation;

                        //

                        if (1 < iSelectedToolsArray.Length)//多于1个工具
                        {
                            if (iCurrentToolIndex == iSelectedToolsArray[0])//首页
                            {
                                customButtonPrevious.Visible = false;
                                customButtonNext.Visible = true;
                            }
                            else if (iCurrentToolIndex == iSelectedToolsArray[iSelectedToolsArray.Length - 1])//末页
                            {
                                customButtonPrevious.Visible = true;
                                customButtonNext.Visible = false;
                            }
                            else//其它
                            {
                                customButtonPrevious.Visible = true;
                                customButtonNext.Visible = true;
                            }
                        }
                        else//其它
                        {
                            customButtonPrevious.Visible = false;
                            customButtonNext.Visible = false;
                        }
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：鼠标点击产生的事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void imageDisplayView_Control_MouseDown(object sender, EventArgs e)
        {
            if (!bManageToolsSelected)//【MANAGE TOOLS】未按下
            {
                bMouseMove = false;

                //

                CustomEventArgs customeventargs = (CustomEventArgs)e;

                Point pointValue = new Point(customeventargs.IntValue[0], customeventargs.IntValue[1]);//源图像坐标

                pointMouse_Move.X = (Int32)((pointValue.X - drawROI.X) / imageDisplayView.ImageScale);
                pointMouse_Move.Y = (Int32)((pointValue.Y - drawROI.Y) / imageDisplayView.ImageScale);

                //

                if ((pointValue.X >= drawROI.X) && (pointValue.X <= (drawROI.X + drawROI.Width))
                    && (pointValue.Y >= drawROI.Y) && (pointValue.Y <= (drawROI.Y + drawROI.Height)))//单击轮廓内部
                {
                    bMouseMove_MouseIn = true;
                }
                else
                {
                    bMouseMove_MouseIn = false;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：鼠标指针在图像显示控件上移动的事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void imageDisplayView_Control_MouseMove(object sender, EventArgs e)
        {
            if (!bManageToolsSelected && bMouseMove_MouseIn)//【MANAGE TOOLS】未按下
            {
                bMouseMove = true;

                //

                CustomEventArgs customeventargs = (CustomEventArgs)e;

                Point pointValue = new Point(customeventargs.IntValue[0], customeventargs.IntValue[1]);//源图像坐标

                //

                Point pointImageDisplayControl = imageDisplayView.PictureBoxControl.PointToScreen(new Point(0, 0));

                //

                if (bHandleVisible || sizeAdjustedPanel.Visible)//手柄可见
                {
                    bDrawImageProcessingResult = false;

                    //

                    Rectangle rectangleImage_Temp = new Rectangle(0, 0, 0, 0);//图像区域
                    if (null != imageDisplayView.BitmapDisplay)//有效
                    {
                        rectangleImage_Temp = new Rectangle(new Point(0, 0), new Size(imageDisplayView.BitmapDisplay.Width, imageDisplayView.BitmapDisplay.Height));
                    }

                    Rectangle rectangleROI_Temp = drawROI;

                    rectangleROI_Temp.X = (Int32)((Control.MousePosition.X - pointMouse_Move.X - pointImageDisplayControl.X) * imageDisplayView.ImageScale);
                    rectangleROI_Temp.Y = (Int32)((Control.MousePosition.Y - pointMouse_Move.Y - pointImageDisplayControl.Y - imageDisplayView.RectangleImage.Location.Y) * imageDisplayView.ImageScale);

                    if (rectangleImage_Temp.Contains(rectangleROI_Temp))//有效
                    {
                        drawROI = rectangleROI_Temp;

                        //

                        bSaveProduct = true;

                        customButtonSaveProduct.BitmapWhole = VisionSystemControlLibrary.Properties.Resources.Button_1;

                        customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【SAVE PRODUCT】

                        //

                        drawROIOriginal = drawROI;

                        sizeAdjustedPanel.RectangleMaxROI = rectangleImage_Temp;

                        sizeAdjustedPanel.RectangleROI = drawROI;

                        //

                        camera.Tools[iCurrentToolIndex].WorkAreaPointX = Convert.ToUInt16(Math.Abs(drawROI.X));
                        camera.Tools[iCurrentToolIndex].WorkAreaPointY = Convert.ToUInt16(Math.Abs(drawROI.Y));
                        camera.Tools[iCurrentToolIndex].WorkAreaWidth = Convert.ToUInt16(Math.Abs(drawROI.Width));
                        camera.Tools[iCurrentToolIndex].WorkAreaHeight = Convert.ToUInt16(Math.Abs(drawROI.Height));
                    }

                    //

                    _UpdateROI();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：鼠标释放产生的事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void imageDisplayView_Control_MouseUp(object sender, EventArgs e)
        {
            if (!bManageToolsSelected)//【MANAGE TOOLS】未按下
            {
                CustomEventArgs customeventargs = (CustomEventArgs)e;

                Point pointValue = new Point(customeventargs.IntValue[0], customeventargs.IntValue[1]);//源图像坐标

                //

                if (bMouseMove)
                {
                    Point pointImageDisplayControl = imageDisplayView.PictureBoxControl.PointToScreen(new Point(0, 0));

                    //

                    if (bHandleVisible || sizeAdjustedPanel.Visible)//手柄可见
                    {
                        bDrawImageProcessingResult = true;

                        //

                        Rectangle rectangleImage_Temp = new Rectangle(0, 0, 0, 0);//图像区域
                        if (null != imageDisplayView.BitmapDisplay)//有效
                        {
                            rectangleImage_Temp = new Rectangle(new Point(0, 0), new Size(imageDisplayView.BitmapDisplay.Width, imageDisplayView.BitmapDisplay.Height));
                        }

                        Rectangle rectangleROI_Temp = drawROI;

                        rectangleROI_Temp.X = (Int32)((Control.MousePosition.X - pointMouse_Move.X - pointImageDisplayControl.X) * imageDisplayView.ImageScale);
                        rectangleROI_Temp.Y = (Int32)((Control.MousePosition.Y - pointMouse_Move.Y - pointImageDisplayControl.Y - imageDisplayView.RectangleImage.Location.Y) * imageDisplayView.ImageScale);

                        if (rectangleImage_Temp.Contains(rectangleROI_Temp))//有效
                        {
                            drawROI = rectangleROI_Temp;

                            //

                            bSaveProduct = true;

                            customButtonSaveProduct.BitmapWhole = VisionSystemControlLibrary.Properties.Resources.Button_1;

                            customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【SAVE PRODUCT】

                            //

                            drawROIOriginal = drawROI;

                            sizeAdjustedPanel.RectangleMaxROI = rectangleImage_Temp;

                            sizeAdjustedPanel.RectangleROI = drawROI;

                            //

                            camera.Tools[iCurrentToolIndex].WorkAreaPointX = Convert.ToUInt16(Math.Abs(drawROI.X));
                            camera.Tools[iCurrentToolIndex].WorkAreaPointY = Convert.ToUInt16(Math.Abs(drawROI.Y));
                            camera.Tools[iCurrentToolIndex].WorkAreaWidth = Convert.ToUInt16(Math.Abs(drawROI.Width));
                            camera.Tools[iCurrentToolIndex].WorkAreaHeight = Convert.ToUInt16(Math.Abs(drawROI.Height));
                        }

                        //

                        _LearnToolProcessing();//工具学习

                        //

                        _UpdateROI();

                        //事件

                        if (null != ToolRegionChanged)//有效
                        {
                            ToolRegionChanged(this, new CustomEventArgs());
                        }
                    }
                }

                //【MEASURE TOOL】

                iImageDisplayClickTime = (Int32)((iImageDisplayClickTime + 1) % 2);

                if (1 == iImageDisplayClickTime)
                {
                    pointFirst_MeasureTool = pointValue;
                    pointSecond_MeasureTool = pointValue;
                }
                else
                {
                    pointFirst_MeasureTool = pointSecond_MeasureTool;
                    pointSecond_MeasureTool = pointValue;

                    if (bMeasureToolSelected)//【MEASURE TOOL】按下
                    {
                        customButtonDeltaXValue.Chinese_TextDisplay = new String[1] { Math.Abs(pointSecond_MeasureTool.X - pointFirst_MeasureTool.X).ToString() };//设置显示的文本
                        customButtonDeltaXValue.English_TextDisplay = new String[1] { Math.Abs(pointSecond_MeasureTool.X - pointFirst_MeasureTool.X).ToString() };//设置显示的文本

                        customButtonDeltaYValue.Chinese_TextDisplay = new String[1] { Math.Abs(pointSecond_MeasureTool.Y - pointFirst_MeasureTool.Y).ToString() };//设置显示的文本
                        customButtonDeltaYValue.English_TextDisplay = new String[1] { Math.Abs(pointSecond_MeasureTool.Y - pointFirst_MeasureTool.Y).ToString() };//设置显示的文本

                        //

                        Image<Gray, Byte> imageMask = new Image<Gray, byte>(imageShow.Size);
                        imageMask.Draw(new LineSegment2D(pointFirst_MeasureTool, pointSecond_MeasureTool), new Gray(255), 1); //全黑图像中画白线

                        Image<Gray, Byte>[] imageShowBuf = imageShow.Split();                                    //对线条区域的图像进行撕裂
                        Image<Gray, Byte> B = imageShowBuf[0].Convert<Gray, Byte>();
                        Image<Gray, Byte> G = imageShowBuf[1].Convert<Gray, Byte>();
                        Image<Gray, Byte> R = imageShowBuf[2].Convert<Gray, Byte>();

                        Gray grayB, grayG, grayR;
                        MCvScalar scalarB, scalarG, scalarR;
                        B.AvgSdv(out grayB, out scalarB, imageMask);
                        G.AvgSdv(out grayG, out scalarG, imageMask);
                        R.AvgSdv(out grayR, out scalarR, imageMask);

                        //

                        _BestContrast(scalarB.v0, scalarG.v0, scalarR.v0);                                    //选择最佳对比颜色
                    }
                }

                customButtonAxisValue.Chinese_TextDisplay = new String[1] { pointSecond_MeasureTool.X.ToString() + "，" + pointSecond_MeasureTool.Y.ToString() };//设置显示的文本
                customButtonAxisValue.English_TextDisplay = new String[1] { pointSecond_MeasureTool.X.ToString() + "，" + pointSecond_MeasureTool.Y.ToString() };//设置显示的文本

                customButtonColorValue.Chinese_TextDisplay = new String[1] { imageShow[pointSecond_MeasureTool.Y, pointSecond_MeasureTool.X].Red.ToString() + "，" + imageShow[pointSecond_MeasureTool.Y, pointSecond_MeasureTool.X].Green.ToString() + "，" + imageShow[pointSecond_MeasureTool.Y, pointSecond_MeasureTool.X].Blue.ToString() };//设置显示的文本
                customButtonColorValue.English_TextDisplay = new String[1] { imageShow[pointSecond_MeasureTool.Y, pointSecond_MeasureTool.X].Red.ToString() + "，" + imageShow[pointSecond_MeasureTool.Y, pointSecond_MeasureTool.X].Green.ToString() + "，" + imageShow[pointSecond_MeasureTool.Y, pointSecond_MeasureTool.X].Blue.ToString() };//设置显示的文本

                _UpdateROI();

                //

                _SetHandle(pointValue);

                //

                bMouseMove = false;

                bMouseMove_MouseIn = false;
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：工具参数进行了设置的事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void parameterSettingsPanel_ParameterValueChanged(object sender, EventArgs e)
        {
            if (1 == parameterSettingsPanel.ParameterType[parameterSettingsPanel.CurrentSelectedValueIndex])//枚举类型
            {
                camera.Tools[iCurrentToolIndex].Arithmetic.EnumCurrent[iToolParameters[iCurrentToolIndex][parameterSettingsPanel.CurrentSelectedValueIndex]] = (Byte)parameterSettingsPanel.ParameterCurrentValue[parameterSettingsPanel.CurrentSelectedValueIndex];
            }
            else//数字类型
            {
                camera.Tools[iCurrentToolIndex].Arithmetic.CurrentValue[iToolParameters[iCurrentToolIndex][parameterSettingsPanel.CurrentSelectedValueIndex]] = (Int16)parameterSettingsPanel.ParameterCurrentValue[parameterSettingsPanel.CurrentSelectedValueIndex];
            }

            //

            bSaveProduct = true;

            customButtonSaveProduct.BitmapWhole = VisionSystemControlLibrary.Properties.Resources.Button_1;//【SAVE PRODUCT】

            customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【SAVE PRODUCT】

            //

            _LearnToolProcessing();//工具学习

            //

            _UpdateROI();

            //事件

            if (null != ToolParameterValueChanged)//有效
            {
                ToolParameterValueChanged(this, new CustomEventArgs());
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：点击区域调整控件的【左】按钮产生的事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void sizeAdjustedPanel_Left_Click(object sender, EventArgs e)
        {
            bSaveProduct = true;

            customButtonSaveProduct.BitmapWhole = VisionSystemControlLibrary.Properties.Resources.Button_1;

            customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【SAVE PRODUCT】

            //

            drawROI = sizeAdjustedPanel.RectangleROI;

            camera.Tools[iCurrentToolIndex].WorkAreaPointX = Convert.ToUInt16(Math.Abs(drawROI.X));
            camera.Tools[iCurrentToolIndex].WorkAreaPointY = Convert.ToUInt16(Math.Abs(drawROI.Y));
            camera.Tools[iCurrentToolIndex].WorkAreaWidth = Convert.ToUInt16(Math.Abs(drawROI.Width));
            camera.Tools[iCurrentToolIndex].WorkAreaHeight = Convert.ToUInt16(Math.Abs(drawROI.Height));

            //

            _LearnToolProcessing();//工具学习

            //

            _UpdateROI();

            //

            if (null != ToolRegionChanged)//有效
            {
                ToolRegionChanged(this, new CustomEventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击区域调整控件的【上】按钮产生的事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void sizeAdjustedPanel_Top_Click(object sender, EventArgs e)
        {
            bSaveProduct = true;

            customButtonSaveProduct.BitmapWhole = VisionSystemControlLibrary.Properties.Resources.Button_1;

            customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【SAVE PRODUCT】

            //

            drawROI = sizeAdjustedPanel.RectangleROI;

            camera.Tools[iCurrentToolIndex].WorkAreaPointX = Convert.ToUInt16(Math.Abs(drawROI.X));
            camera.Tools[iCurrentToolIndex].WorkAreaPointY = Convert.ToUInt16(Math.Abs(drawROI.Y));
            camera.Tools[iCurrentToolIndex].WorkAreaWidth = Convert.ToUInt16(Math.Abs(drawROI.Width));
            camera.Tools[iCurrentToolIndex].WorkAreaHeight = Convert.ToUInt16(Math.Abs(drawROI.Height));

            //

            _LearnToolProcessing();//工具学习

            //

            _UpdateROI();

            //

            if (null != ToolRegionChanged)//有效
            {
                ToolRegionChanged(this, new CustomEventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击区域调整控件的【右】按钮产生的事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void sizeAdjustedPanel_Right_Click(object sender, EventArgs e)
        {
            bSaveProduct = true;

            customButtonSaveProduct.BitmapWhole = VisionSystemControlLibrary.Properties.Resources.Button_1;

            customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【SAVE PRODUCT】

            //

            drawROI = sizeAdjustedPanel.RectangleROI;

            camera.Tools[iCurrentToolIndex].WorkAreaPointX = Convert.ToUInt16(Math.Abs(drawROI.X));
            camera.Tools[iCurrentToolIndex].WorkAreaPointY = Convert.ToUInt16(Math.Abs(drawROI.Y));
            camera.Tools[iCurrentToolIndex].WorkAreaWidth = Convert.ToUInt16(Math.Abs(drawROI.Width));
            camera.Tools[iCurrentToolIndex].WorkAreaHeight = Convert.ToUInt16(Math.Abs(drawROI.Height));

            //

            _LearnToolProcessing();//工具学习

            //

            _UpdateROI();

            //

            if (null != ToolRegionChanged)//有效
            {
                ToolRegionChanged(this, new CustomEventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击区域调整控件的【下】按钮产生的事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void sizeAdjustedPanel_Bottom_Click(object sender, EventArgs e)
        {
            bSaveProduct = true;

            customButtonSaveProduct.BitmapWhole = VisionSystemControlLibrary.Properties.Resources.Button_1;

            customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【SAVE PRODUCT】

            //

            drawROI = sizeAdjustedPanel.RectangleROI;

            camera.Tools[iCurrentToolIndex].WorkAreaPointX = Convert.ToUInt16(Math.Abs(drawROI.X));
            camera.Tools[iCurrentToolIndex].WorkAreaPointY = Convert.ToUInt16(Math.Abs(drawROI.Y));
            camera.Tools[iCurrentToolIndex].WorkAreaWidth = Convert.ToUInt16(Math.Abs(drawROI.Width));
            camera.Tools[iCurrentToolIndex].WorkAreaHeight = Convert.ToUInt16(Math.Abs(drawROI.Height));

            //

            _LearnToolProcessing();//工具学习

            //

            _UpdateROI();

            //

            if (null != ToolRegionChanged)//有效
            {
                ToolRegionChanged(this, new CustomEventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击区域调整控件的【左上】按钮产生的事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void sizeAdjustedPanel_LeftTop_Click(object sender, EventArgs e)
        {
            bSaveProduct = true;

            customButtonSaveProduct.BitmapWhole = VisionSystemControlLibrary.Properties.Resources.Button_1;

            customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【SAVE PRODUCT】

            //

            drawROI = sizeAdjustedPanel.RectangleROI;

            camera.Tools[iCurrentToolIndex].WorkAreaPointX = Convert.ToUInt16(Math.Abs(drawROI.X));
            camera.Tools[iCurrentToolIndex].WorkAreaPointY = Convert.ToUInt16(Math.Abs(drawROI.Y));
            camera.Tools[iCurrentToolIndex].WorkAreaWidth = Convert.ToUInt16(Math.Abs(drawROI.Width));
            camera.Tools[iCurrentToolIndex].WorkAreaHeight = Convert.ToUInt16(Math.Abs(drawROI.Height));

            //

            _LearnToolProcessing();//工具学习

            //

            _UpdateROI();

            //

            if (null != ToolRegionChanged)//有效
            {
                ToolRegionChanged(this, new CustomEventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击区域调整控件的【右上】按钮产生的事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void sizeAdjustedPanel_RightTop_Click(object sender, EventArgs e)
        {
            bSaveProduct = true;

            customButtonSaveProduct.BitmapWhole = VisionSystemControlLibrary.Properties.Resources.Button_1;

            customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【SAVE PRODUCT】

            //

            drawROI = sizeAdjustedPanel.RectangleROI;

            camera.Tools[iCurrentToolIndex].WorkAreaPointX = Convert.ToUInt16(Math.Abs(drawROI.X));
            camera.Tools[iCurrentToolIndex].WorkAreaPointY = Convert.ToUInt16(Math.Abs(drawROI.Y));
            camera.Tools[iCurrentToolIndex].WorkAreaWidth = Convert.ToUInt16(Math.Abs(drawROI.Width));
            camera.Tools[iCurrentToolIndex].WorkAreaHeight = Convert.ToUInt16(Math.Abs(drawROI.Height));

            //

            _LearnToolProcessing();//工具学习

            //

            _UpdateROI();

            //

            if (null != ToolRegionChanged)//有效
            {
                ToolRegionChanged(this, new CustomEventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击区域调整控件的【左下】按钮产生的事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void sizeAdjustedPanel_LeftBottom_Click(object sender, EventArgs e)
        {
            bSaveProduct = true;

            customButtonSaveProduct.BitmapWhole = VisionSystemControlLibrary.Properties.Resources.Button_1;

            customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【SAVE PRODUCT】

            //

            drawROI = sizeAdjustedPanel.RectangleROI;

            camera.Tools[iCurrentToolIndex].WorkAreaPointX = Convert.ToUInt16(Math.Abs(drawROI.X));
            camera.Tools[iCurrentToolIndex].WorkAreaPointY = Convert.ToUInt16(Math.Abs(drawROI.Y));
            camera.Tools[iCurrentToolIndex].WorkAreaWidth = Convert.ToUInt16(Math.Abs(drawROI.Width));
            camera.Tools[iCurrentToolIndex].WorkAreaHeight = Convert.ToUInt16(Math.Abs(drawROI.Height));

            //

            _LearnToolProcessing();//工具学习

            //

            _UpdateROI();

            //

            if (null != ToolRegionChanged)//有效
            {
                ToolRegionChanged(this, new CustomEventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击区域调整控件的【右下】按钮产生的事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void sizeAdjustedPanel_RightBottom_Click(object sender, EventArgs e)
        {
            bSaveProduct = true;

            customButtonSaveProduct.BitmapWhole = VisionSystemControlLibrary.Properties.Resources.Button_1;

            customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【SAVE PRODUCT】

            //

            drawROI = sizeAdjustedPanel.RectangleROI;

            camera.Tools[iCurrentToolIndex].WorkAreaPointX = Convert.ToUInt16(Math.Abs(drawROI.X));
            camera.Tools[iCurrentToolIndex].WorkAreaPointY = Convert.ToUInt16(Math.Abs(drawROI.Y));
            camera.Tools[iCurrentToolIndex].WorkAreaWidth = Convert.ToUInt16(Math.Abs(drawROI.Width));
            camera.Tools[iCurrentToolIndex].WorkAreaHeight = Convert.ToUInt16(Math.Abs(drawROI.Height));

            //

            _LearnToolProcessing();//工具学习

            //

            _UpdateROI();

            //

            if (null != ToolRegionChanged)//有效
            {
                ToolRegionChanged(this, new CustomEventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击区域调整控件的【移动】按钮产生的事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void sizeAdjustedPanel_Center_Click(object sender, EventArgs e)
        {
            CustomEventArgs customeventargs = (CustomEventArgs)e;

            pointMouse_SizeAdjustedPanel = new Point(customeventargs.IntValue[0], customeventargs.IntValue[1]);
        }

        //----------------------------------------------------------------------
        // 功能说明：鼠标指针在区域调整控件的【移动】按钮上移动时产生的事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void sizeAdjustedPanel_Center_MouseMove(object sender, EventArgs e)
        {
            Point pointWhole = this.PointToScreen(new Point(0, 0));

            Rectangle rectangleWhole = new Rectangle(0, imageDisplayView.Location.Y, imageDisplayView.Location.X + imageDisplayView.Width, imageDisplayView.Location.Y + imageDisplayView.Height);

            //

            Rectangle rectangleSizeAdjustedPanel = new Rectangle(sizeAdjustedPanel.Location, sizeAdjustedPanel.Size);

            rectangleSizeAdjustedPanel.X = Control.MousePosition.X - pointWhole.X - pointMouse_SizeAdjustedPanel.X;
            rectangleSizeAdjustedPanel.Y = Control.MousePosition.Y - pointWhole.Y - pointMouse_SizeAdjustedPanel.Y;

            if (rectangleWhole.Contains(rectangleSizeAdjustedPanel))//有效
            {
                sizeAdjustedPanel.Location = new Point(rectangleSizeAdjustedPanel.X, rectangleSizeAdjustedPanel.Y);

                Update();
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击区域调整控件的【HOME】按钮产生的事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void sizeAdjustedPanel_Home_Click(object sender, EventArgs e)
        {
            sizeAdjustedPanel.Location = SizeAdjustedPanelDefaultLocation;
        }

        //----------------------------------------------------------------------
        // 功能说明：点击区域调整控件的【CLOSE】按钮产生的事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void sizeAdjustedPanel_Close_Click(object sender, EventArgs e)
        {
            bSizeAdjustedPanelVisible = false;
            sizeAdjustedPanel.Location = SizeAdjustedPanelDefaultLocation;

            _ShowHandle(false);
            _ResetHandleColor();
        }

        //----------------------------------------------------------------------
        // 功能说明：点击区域选择控件的【SHOW】/【HIDE】按钮产生的事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void sizeAdjustedPanel_ShowHide_Click(object sender, EventArgs e)
        {
            _ShowHandle(!sizeAdjustedPanel.ShowHideButton);
        }

        //----------------------------------------------------------------------
        // 功能说明：点击区域选择控件的【区域选择】按钮产生的事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void sizeAdjustedPanel_RegionSelection_Click(object sender, EventArgs e)
        {
            switch (sizeAdjustedPanel.SelectionType)
            {
                case RegionSelectionType.None:

                    _ResetHandleColor();

                    break;
                case RegionSelectionType.Left:

                    _ClickRegionSelectionButton(labelHandleLeft,ContentAlignment.MiddleLeft);

                    break;
                case RegionSelectionType.LeftTop:

                    _ClickRegionSelectionButton(labelHandleLeftTop, ContentAlignment.TopLeft);

                    break;
                case RegionSelectionType.Top:

                    _ClickRegionSelectionButton(labelHandleTop, ContentAlignment.TopCenter);

                    break;
                case RegionSelectionType.RightTop:

                    _ClickRegionSelectionButton(labelHandleRightTop, ContentAlignment.TopRight);

                    break;
                case RegionSelectionType.Right:

                    _ClickRegionSelectionButton(labelHandleRight, ContentAlignment.MiddleRight);

                    break;
                case RegionSelectionType.LeftBottom:

                    _ClickRegionSelectionButton(labelHandleLeftBottom, ContentAlignment.BottomLeft);

                    break;
                case RegionSelectionType.Bottom:

                    _ClickRegionSelectionButton(labelHandleBottom, ContentAlignment.BottomCenter);

                    break;
                case RegionSelectionType.RightBottom:

                    _ClickRegionSelectionButton(labelHandleRightBottom, ContentAlignment.BottomRight);

                    break;
                default:
                    break;
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：点击区域选择按钮
        // 输入参数：1.label：控件
        //         2.type：类型
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _ClickRegionSelectionButton(Label label, ContentAlignment type)
        {
            _ResetHandleColor();
            label.BackColor = Color.Yellow;

            contentalignmentHandleType = type;
            sizeAdjustedPanel.HandleType = contentalignmentHandleType;
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：点击区域调整【左上】手柄产生的事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void labelHandleLeftTop_MouseDown(object sender, MouseEventArgs e)
        {
            drawROIOriginal = drawROI;

            pointMouse_Move.X = e.X;
            pointMouse_Move.Y = e.Y;

            //

            sizeAdjustedPanel.SelectionType = VisionSystemControlLibrary.RegionSelectionType.LeftTop;

            _ClickRegionSelectionButton(labelHandleLeftTop, ContentAlignment.TopLeft);
        }

        //----------------------------------------------------------------------
        // 功能说明：拖动区域调整【左上】手柄产生的事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void labelHandleLeftTop_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                bDrawImageProcessingResult = false;

                //

                bSaveProduct = true;

                customButtonSaveProduct.BitmapWhole = VisionSystemControlLibrary.Properties.Resources.Button_1;

                customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【SAVE PRODUCT】

                //

                _ROIChange();
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：释放区域调整【左上】手柄产生的事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void labelHandleLeftTop_MouseUp(object sender, MouseEventArgs e)
        {
            bDrawImageProcessingResult = true;

            //

            _LearnToolProcessing();//工具学习

            //

            _SetSizeAdjustedPanel();

            _UpdateROI();

            //事件

            if (null != ToolRegionChanged)//有效
            {
                ToolRegionChanged(this, new CustomEventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击区域调整【上】手柄产生的事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void labelHandleTop_MouseDown(object sender, MouseEventArgs e)
        {
            drawROIOriginal = drawROI;

            pointMouse_Move.Y = e.Y;

            //

            sizeAdjustedPanel.SelectionType = VisionSystemControlLibrary.RegionSelectionType.Top;

            _ClickRegionSelectionButton(labelHandleTop, ContentAlignment.TopCenter);
        }

        //----------------------------------------------------------------------
        // 功能说明：拖动区域调整【上】手柄产生的事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void labelHandleTop_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                bDrawImageProcessingResult = false;

                //

                bSaveProduct = true;

                customButtonSaveProduct.BitmapWhole = VisionSystemControlLibrary.Properties.Resources.Button_1;

                customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【SAVE PRODUCT】

                //

                _ROIChange();
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：释放区域调整【上】手柄产生的事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void labelHandleTop_MouseUp(object sender, MouseEventArgs e)
        {
            bDrawImageProcessingResult = true;

            //

            _LearnToolProcessing();//工具学习

            //

            _SetSizeAdjustedPanel();

            _UpdateROI();

            //事件

            if (null != ToolRegionChanged)//有效
            {
                ToolRegionChanged(this, new CustomEventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击区域调整【右上】手柄产生的事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void labelHandleRightTop_MouseDown(object sender, MouseEventArgs e)
        {
            drawROIOriginal = drawROI;

            pointMouse_Move.X = labelHandleRight.Width - e.X;
            pointMouse_Move.Y = e.Y;

            //

            sizeAdjustedPanel.SelectionType = VisionSystemControlLibrary.RegionSelectionType.RightTop;

            _ClickRegionSelectionButton(labelHandleRightTop, ContentAlignment.TopRight);
        }

        //----------------------------------------------------------------------
        // 功能说明：拖动区域调整【右上】手柄产生的事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void labelHandleRightTop_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                bDrawImageProcessingResult = false;

                //

                bSaveProduct = true;

                customButtonSaveProduct.BitmapWhole = VisionSystemControlLibrary.Properties.Resources.Button_1;

                customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【SAVE PRODUCT】

                //

                _ROIChange();
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：释放区域调整【右上】手柄产生的事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void labelHandleRightTop_MouseUp(object sender, MouseEventArgs e)
        {
            bDrawImageProcessingResult = true;

            //

            _LearnToolProcessing();//工具学习

            //

            _SetSizeAdjustedPanel();

            _UpdateROI();

            //事件

            if (null != ToolRegionChanged)//有效
            {
                ToolRegionChanged(this, new CustomEventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击区域调整【左】手柄产生的事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void labelHandleLeft_MouseDown(object sender, MouseEventArgs e)
        {
            drawROIOriginal = drawROI;

            pointMouse_Move.X = e.X;

            //

            sizeAdjustedPanel.SelectionType = VisionSystemControlLibrary.RegionSelectionType.Left;

            _ClickRegionSelectionButton(labelHandleLeft, ContentAlignment.MiddleLeft);
        }

        //----------------------------------------------------------------------
        // 功能说明：拖动区域调整【左】手柄产生的事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void labelHandleLeft_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                bDrawImageProcessingResult = false;

                //

                bSaveProduct = true;

                customButtonSaveProduct.BitmapWhole = VisionSystemControlLibrary.Properties.Resources.Button_1;

                customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【SAVE PRODUCT】

                //

                _ROIChange();
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：释放区域调整【左】手柄产生的事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void labelHandleLeft_MouseUp(object sender, MouseEventArgs e)
        {
            bDrawImageProcessingResult = true;

            //

            _LearnToolProcessing();//工具学习

            //

            _SetSizeAdjustedPanel();

            _UpdateROI();

            //事件

            if (null != ToolRegionChanged)//有效
            {
                ToolRegionChanged(this, new CustomEventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击区域调整【右】手柄产生的事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void labelHandleRight_MouseDown(object sender, MouseEventArgs e)
        {
            drawROIOriginal = drawROI;

            pointMouse_Move.X = labelHandleRight.Width - e.X;

            //

            sizeAdjustedPanel.SelectionType = VisionSystemControlLibrary.RegionSelectionType.Right;

            _ClickRegionSelectionButton(labelHandleRight, ContentAlignment.MiddleRight);
        }

        //----------------------------------------------------------------------
        // 功能说明：拖动区域调整【右】手柄产生的事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void labelHandleRight_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                bDrawImageProcessingResult = false;

                //

                bSaveProduct = true;

                customButtonSaveProduct.BitmapWhole = VisionSystemControlLibrary.Properties.Resources.Button_1;

                customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【SAVE PRODUCT】

                //

                _ROIChange();
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：释放区域调整【右】手柄产生的事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void labelHandleRight_MouseUp(object sender, MouseEventArgs e)
        {
            bDrawImageProcessingResult = true;

            //

            _LearnToolProcessing();//工具学习

            //

            _SetSizeAdjustedPanel();

            _UpdateROI();

            //事件

            if (null != ToolRegionChanged)//有效
            {
                ToolRegionChanged(this, new CustomEventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击区域调整【左下】手柄产生的事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void labelHandleLeftBottom_MouseDown(object sender, MouseEventArgs e)
        {
            drawROIOriginal = drawROI;

            pointMouse_Move.X = e.X;
            pointMouse_Move.Y = labelHandleRight.Height - e.Y;

            //

            sizeAdjustedPanel.SelectionType = VisionSystemControlLibrary.RegionSelectionType.LeftBottom;

            _ClickRegionSelectionButton(labelHandleLeftBottom, ContentAlignment.BottomLeft);
        }

        //----------------------------------------------------------------------
        // 功能说明：拖动区域调整【左下】手柄产生的事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void labelHandleLeftBottom_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                bDrawImageProcessingResult = false;

                //

                bSaveProduct = true;

                customButtonSaveProduct.BitmapWhole = VisionSystemControlLibrary.Properties.Resources.Button_1;

                customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【SAVE PRODUCT】

                //

                _ROIChange();
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：释放区域调整【左下】手柄产生的事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void labelHandleLeftBottom_MouseUp(object sender, MouseEventArgs e)
        {
            bDrawImageProcessingResult = true;

            //

            _LearnToolProcessing();//工具学习

            //

            _SetSizeAdjustedPanel();

            _UpdateROI();

            //事件

            if (null != ToolRegionChanged)//有效
            {
                ToolRegionChanged(this, new CustomEventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击区域调整【下】手柄产生的事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void labelHandleBottom_MouseDown(object sender, MouseEventArgs e)
        {
            drawROIOriginal = drawROI;

            pointMouse_Move.Y = labelHandleRight.Height - e.Y;

            //

            sizeAdjustedPanel.SelectionType = VisionSystemControlLibrary.RegionSelectionType.Bottom;

            _ClickRegionSelectionButton(labelHandleBottom, ContentAlignment.BottomCenter);
        }

        //----------------------------------------------------------------------
        // 功能说明：拖动区域调整【下】手柄产生的事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void labelHandleBottom_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                bDrawImageProcessingResult = false;

                //

                bSaveProduct = true;

                customButtonSaveProduct.BitmapWhole = VisionSystemControlLibrary.Properties.Resources.Button_1;

                customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【SAVE PRODUCT】

                //

                _ROIChange();
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：释放区域调整【下】手柄产生的事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void labelHandleBottom_MouseUp(object sender, MouseEventArgs e)
        {
            bDrawImageProcessingResult = true;

            //

            _LearnToolProcessing();//工具学习

            //

            _SetSizeAdjustedPanel();

            _UpdateROI();

            //事件

            if (null != ToolRegionChanged)//有效
            {
                ToolRegionChanged(this, new CustomEventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击区域调整【右下】手柄产生的事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void labelHandleRightBottom_MouseDown(object sender, MouseEventArgs e)
        {
            drawROIOriginal = drawROI;

            pointMouse_Move.X = labelHandleRight.Width - e.X;
            pointMouse_Move.Y = labelHandleRight.Height - e.Y;

            //

            sizeAdjustedPanel.SelectionType = VisionSystemControlLibrary.RegionSelectionType.RightBottom;

            _ClickRegionSelectionButton(labelHandleRightBottom, ContentAlignment.BottomRight);
        }

        //----------------------------------------------------------------------
        // 功能说明：拖动区域调整【右下】手柄产生的事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void labelHandleRightBottom_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                bDrawImageProcessingResult = false;

                //

                bSaveProduct = true;

                customButtonSaveProduct.BitmapWhole = VisionSystemControlLibrary.Properties.Resources.Button_1;

                customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【SAVE PRODUCT】

                //

                _ROIChange();
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：释放区域调整【右下】手柄产生的事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void labelHandleRightBottom_MouseUp(object sender, MouseEventArgs e)
        {
            bDrawImageProcessingResult = true;

            //

            _LearnToolProcessing();//工具学习

            //

            _SetSizeAdjustedPanel();

            _UpdateROI();

            //事件

            if (null != ToolRegionChanged)//有效
            {
                ToolRegionChanged(this, new CustomEventArgs());
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：点击工具选择列表事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customListTool_CustomListItem_Click(object sender, EventArgs e)
        {
            bSaveProduct = true;

            customButtonSaveProduct.BitmapWhole = VisionSystemControlLibrary.Properties.Resources.Button_1;

            if (bManageToolsSelected)//按下
            {
                customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【SAVE PRODUCT】
            }
            else//未按下
            {
                customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【SAVE PRODUCT】
            }

            //

            if (0 == customListTool.SelectedItemNumber)//无工具被选择
            {
                //选择当前工具

                customListTool.ItemData[customListTool.CurrentListIndex].ItemDataDisplay[1] = false;
                customListTool.SelectedItemNumber = 1;

                customListTool._Refresh(customListTool.CurrentListIndex);//刷新
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：QUALITY CHECK，【SAVE PRODUCT】确认，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_QualityCheck_SaveProduct_Confirm(object sender, EventArgs e)
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

            if (GlobalWindows.MessageDisplay_Window.MessageDisplayControl.OkCancel)//保存数据
            {
                //事件

                if (null != SaveProduct_Click)//有效
                {
                    SaveProduct_Click(this, new CustomEventArgs());
                }
            }
            else//不保存数据
            {
                if (bClickCloseButton)//点击【CLOSE】按钮
                {
                    _Close();
                }
            }
        }
        
        //----------------------------------------------------------------------
        // 功能说明：QUALITY CHECK，【SAVE PRODUCT】成功，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_QualityCheck_SaveProduct_Success(object sender, EventArgs e)
        {
            if (bClickCloseButton)//点击【CLOSE】按钮
            {
                _Close();
            }

            //

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
        // 功能说明：QUALITY CHECK，【SAVE PRODUCT】失败，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_QualityCheck_SaveProduct_Failure(object sender, EventArgs e)
        {
            if (bClickCloseButton)//点击【CLOSE】按钮
            {
                _Close();
            }

            //

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
        // 功能说明：QUALITY CHECK，【LEARN SAMPLE】确认，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_QualityCheck_LearnSample_Confirm(object sender, EventArgs e)
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

            if (GlobalWindows.MessageDisplay_Window.MessageDisplayControl.OkCancel)//保存数据
            {
                //事件

                if (null != LearnSample_Click)//有效
                {
                    LearnSample_Click(this, e);
                }
            }
            else//不保存数据
            {
                //不执行操作
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：QUALITY CHECK，【LEARN SAMPLE】成功，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_QualityCheck_LearnSample_Success(object sender, EventArgs e)
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
        // 功能说明：QUALITY CHECK，【LEARN SAMPLE】失败，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_QualityCheck_LearnSample_Failure(object sender, EventArgs e)
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
}