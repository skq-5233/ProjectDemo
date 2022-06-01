/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：TitleBarControl.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：页面标题控件

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

//待定，READY和B.CHG的状态含义，及其与【STATE】按钮的关系

namespace VisionSystemControlLibrary
{
    public partial class TitleBarControl : UserControl
    {
        //该控件为页面标题控件

        //若当前系统中无任何相机连接，则应该将devicestate的值设置为VisionSystemClassLibrary.Enum.DeviceState.None，此时不显示【STATE】按钮

        private VisionSystemClassLibrary.Enum.InterfaceLanguage language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;//属性，语言

        private VisionSystemClassLibrary.Enum.DeviceState devicestate = VisionSystemClassLibrary.Enum.DeviceState.Run;//属性，设备状态

        //

        private String[] sCameraName_Chinese = new String[3] { "", "", "" };//属性，相机中文名称
        private String[] sCameraName_English = new String[3] { "", "", "" };//属性，相机英文名称
        private Boolean[] bFaultExist = new Boolean[3] { false, false, false };//属性，每个相机的故障信息是否存在。取值范围：true，存在；false，不存在

        //

        private Int32 iFaultMessageTypeNumber = 2;//故障信息类型数量

        private Bitmap[] bitmapBackground = new Bitmap[2];//控件背景

        //

        private Boolean bNetCheckShow = true;//属性，【NET CHECK】按钮是否显示。取值范围：true，是；false，否
        private Boolean bStateShow = false;//属性，【STATE】按钮是否显示。取值范围：true，是；false，否

        private Boolean bTitleBarStyle = false;//属性，X6S和其它产品风格样式。取值范围：true，是，X6S样式；false，否，其它

        //

        private Boolean bEnabled = true;//属性，控件使能或禁止，主要针对【NET CHECK】和【STATE】按钮。取值范围：true，使能；false，禁止

        //

        private Bitmap[] bitmapInspectValue = new Bitmap[5] { null, null, null, null, null };//Inspect数值图标（Stop，Ready，Run，Brand Changed，Disable）

        //

        private String sUserPassword = "";//属性，用户密码
        private String sAdministratorPassword = "";//属性，管理员密码

        //

        private Int32 iWindowParameter = 0;//属性，调用的键盘窗口特征数值（由于该控件具有多个实例，因此在设置属性值时进行事件的订阅。若在构造函数中进行订阅，则会出现重复的情况，导致事件多次响应）。取值范围：
        //15.WORK，点击【STATE】按钮显示密码窗口
        //16.SYSTEM，点击【STATE】按钮显示密码窗口
        //17.DEVICES SETUP，点击【STATE】按钮显示密码窗口
        //18.IMAGE CONFIGURATION，点击【STATE】按钮显示密码窗口
        //19.BRAND MANAGEMENT，点击【STATE】按钮显示密码窗口
        //20.BACKUP BRANDS，点击【STATE】按钮显示密码窗口
        //21.RESTORE BRANDS，点击【STATE】按钮显示密码窗口
        //22.QUALITY CHECK，点击【STATE】按钮显示密码窗口
        //23.TOLERANCES SETTINGS，点击【STATE】按钮显示密码窗口
        //24.LIVE VIEW，点击【STATE】按钮显示密码窗口
        //25.REJECTS VIEW，点击【STATE】按钮显示密码窗口
        //26.STATISTICS，点击【STATE】按钮显示密码窗口

        //

        private String[][] sMessageText = new String[2][];//控件显示的文本（[语言][包含的文本]）

        //

        [Browsable(true), Description("点击【NET CHECK】按钮时产生的事件"), Category("TitleBarControl 事件")]
        public event EventHandler NetCheck_Click;//点击【NET CHECK】按钮时产生的事件

        [Browsable(true), Description("点击【STATE】按钮时产生的事件"), Category("TitleBarControl 事件")]
        public event EventHandler State_Click;//点击【STATE】按钮时产生的事件

        [Browsable(true), Description("点击故障信息控件时产生的事件"), Category("TitleBarControl 事件")]
        public event EventHandler GetFaultMessages;//点击故障信息控件时产生的事件

        [Browsable(true), Description("双击人机界面切换时产生的事件"), Category("TitleBarControl 事件")]
        public event EventHandler ChangeInterface;//点击人机界面切换时产生的事件

        //构造函数

        //----------------------------------------------------------------------
        // 功能说明：系统默认调用，构造函数，初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public TitleBarControl()
        {
            InitializeComponent();

            //

            if (bTitleBarStyle) //显示X6S样式
            {
                labelInspectText.Visible = true;

                labelStopText.Visible = true;
                labelReadyText.Visible = true;
                labelRunText.Visible = true;
                labelBrandChangedText.Visible = true;

                labelStopValue.Visible = true;
                labelReadyValue.Visible = true;
                labelRunValue.Visible = true;
                labelBrandChangedValue.Visible = true;

                labelMachineText.Visible = false;
                labelMachineValue.Visible = false;

                labelShiftText.Visible = false;
                labelShiftValue.Visible = false;

                labelTime.Visible = false;

                customButtonNetCheck.Visible = true;

                customButtonState.Location = new System.Drawing.Point(121, 9);
                labelBrandText.Location = new System.Drawing.Point(676, 66);
                labelBrandText.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
                labelBrandValue.Location = new System.Drawing.Point(775, 66);
            }
            else
            {
                labelInspectText.Visible = false;

                labelStopText.Visible = false;
                labelReadyText.Visible = false;
                labelRunText.Visible = false;
                labelBrandChangedText.Visible = false;

                labelStopValue.Visible = false;
                labelReadyValue.Visible = false;
                labelRunValue.Visible = false;
                labelBrandChangedValue.Visible = false;

                labelMachineText.Visible = true;
                labelMachineValue.Visible = true;

                labelShiftText.Visible = true;
                labelShiftValue.Visible = true;

                labelTime.Visible = true;

                customButtonNetCheck.Visible = false;

                customButtonState.Location = new System.Drawing.Point(8, 9);

                labelBrandText.Location = new System.Drawing.Point(767, 48);
                labelBrandText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                labelBrandValue.Location = new System.Drawing.Point(852, 48);
            }

            labelStopValue.Text = "";
            labelReadyValue.Text = "";
            labelRunValue.Text = "";
            labelBrandChangedValue.Text = "";

            labelCaption.Text = "";//标题名称
            labelBrandValue.Text = "";//品牌数值

            labelTime.Text = "";//当前时间

            labelMachineValue.Text = "";//机器类型数值

            labelShiftValue.Text = "";//班次数值

            labelFaultMessage.Text = "";//故障信息

            Int32 i = 0;//循环控制变量
            Size sizeIcon = new Size(VisionSystemControlLibrary.Properties.Resources.Inspect.Width / bitmapInspectValue.Length, VisionSystemControlLibrary.Properties.Resources.Inspect.Height);//属性，图标的大小

            for (i = 0; i < bitmapInspectValue.Length; i++)//获取图标
            {
                bitmapInspectValue[i] = VisionSystemControlLibrary.Properties.Resources.Inspect.Clone(new Rectangle(new Point(i * sizeIcon.Width, 0), sizeIcon), VisionSystemControlLibrary.Properties.Resources.Inspect.PixelFormat);//获取图标
            }

            labelStopValue.Image = bitmapInspectValue[4];
            labelReadyValue.Image = bitmapInspectValue[4];
            labelRunValue.Image = bitmapInspectValue[2];
            labelBrandChangedValue.Image = bitmapInspectValue[4];

            //

            FieldInfo[] fieldinfo = VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese.GetType().GetFields();//获取公共字段
            if (null != fieldinfo && 1 < fieldinfo.Length)//有效
            {
                sMessageText = new String[fieldinfo.Length - 1][];
                for (i = 0; i < fieldinfo.Length - 1; i++)
                {
                    sMessageText[i] = new String[9];
                }

                //

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] = "状态";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] = "Inspect";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][1] = "停止";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][1] = "STOP";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][2] = "准备";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][2] = "READY";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][3] = "运行";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][3] = "RUN";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][4] = "品牌更换";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][4] = "B.CHG";

                if (bTitleBarStyle)  //X6S样式
                {
                    sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][5] = "品牌";
                }
                else
                {
                    sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][5] = "当前品牌";
                }
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][5] = "Brand";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][6] = "密码";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][6] = "Password";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][7] = "机器类型";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][7] = "Machine";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][8] = "当前班次";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][8] = "Shift";
            }

            //

            Int32 iWidth = (Int32)(VisionSystemControlLibrary.Properties.Resources.TitleBarBackground.Width / iFaultMessageTypeNumber);
            Int32 iHeight = VisionSystemControlLibrary.Properties.Resources.TitleBarBackground.Height;

            for (i = 0; i < iFaultMessageTypeNumber; i++)
            {
                bitmapBackground[i] = VisionSystemControlLibrary.Properties.Resources.TitleBarBackground.Clone(new Rectangle(new Point(iWidth * i, 0), new Size(iWidth, iHeight)), VisionSystemControlLibrary.Properties.Resources.TitleBarBackground.PixelFormat);//释放
            }
        }

        //属性

        //-----------------------------------------------------------------------
        // 功能说明：TitleBarStyle属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("TitleBar样式"), Category("TitleBarControl 通用")]
        public Boolean TitleBarStyle
        {
            get//读取
            {
                return bTitleBarStyle;
            }
            set//设置
            {
                if (value != bTitleBarStyle)
                {
                    bTitleBarStyle = value;

                    if (bTitleBarStyle) //显示X6S样式
                    {
                        labelInspectText.Visible = true;

                        labelStopText.Visible = true;
                        labelReadyText.Visible = true;
                        labelRunText.Visible = true;
                        labelBrandChangedText.Visible = true;

                        labelStopValue.Visible = true;
                        labelReadyValue.Visible = true;
                        labelRunValue.Visible = true;
                        labelBrandChangedValue.Visible = true;

                        labelMachineText.Visible = false;
                        labelMachineValue.Visible = false;

                        labelShiftText.Visible = false;
                        labelShiftValue.Visible = false;

                        labelTime.Visible = false;

                        customButtonNetCheck.Visible = true;

                        customButtonState.Location = new System.Drawing.Point(121, 9);
                        labelBrandText.Location = new System.Drawing.Point(676, 66);
                        labelBrandText.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
                        labelBrandValue.Location = new System.Drawing.Point(775, 66);
                    }
                    else
                    {
                        labelInspectText.Visible = false;

                        labelStopText.Visible = false;
                        labelReadyText.Visible = false;
                        labelRunText.Visible = false;
                        labelBrandChangedText.Visible = false;

                        labelStopValue.Visible = false;
                        labelReadyValue.Visible = false;
                        labelRunValue.Visible = false;
                        labelBrandChangedValue.Visible = false;

                        labelMachineText.Visible = true;
                        labelMachineValue.Visible = true;

                        labelShiftText.Visible = true;
                        labelShiftValue.Visible = true;

                        labelTime.Visible = true;

                        customButtonNetCheck.Visible = false;

                        customButtonState.Location = new System.Drawing.Point(8, 9);
                        labelBrandText.Location = new System.Drawing.Point(767, 48);
                        labelBrandText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                        labelBrandValue.Location = new System.Drawing.Point(852, 48);
                    }

                    if (bTitleBarStyle)  //X6S样式
                    {
                        sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][5] = "品牌";
                    }
                    else
                    {
                        sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][5] = "当前品牌";
                    }

                    labelBrandText.Text = sMessageText[(Int32)language - 1][5] + "：";
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Language属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("语言"), Category("TitleBarControl 通用")]
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
        [Browsable(true), Description("设备状态"), Category("TitleBarControl 通用")]
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
        // 功能说明：NetCheckShow属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("控件使能或禁止，主要针对【NET CHECK】和【STATE】按钮。取值范围：true，使能；false，禁止"), Category("TitleBarControl 通用")]
        public Boolean ControlEnabled//属性
        {
            get//读取
            {
                return bEnabled;
            }
            set//设置
            {
                if (value != bEnabled)
                {
                    bEnabled = value;

                    //

                    if (bEnabled)//使能
                    {
                        customButtonNetCheck.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【NET CHECK】按钮                           
                        customButtonState.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【STATE】按钮
                    }
                    else//禁止
                    {
                        customButtonNetCheck.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【NET CHECK】按钮
                        customButtonState.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【STATE】按钮
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：NetCheckShow属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("【NET CHECK】按钮是否显示。取值范围：true，是；false，否"), Category("TitleBarControl 通用")]
        public Boolean NetCheckShow//属性
        {
            get//读取
            {
                return bNetCheckShow;
            }
            set//设置
            {
                if (value != bNetCheckShow)
                {
                    bNetCheckShow = value;

                    customButtonNetCheck.Visible = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：StateShow属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("【STATE】按钮是否显示。取值范围：true，是；false，否"), Category("TitleBarControl 通用")]
        public Boolean StateShow//属性
        {
            get//读取
            {
                return bStateShow;
            }
            set//设置
            {
                if (value != bStateShow)
                {
                    bStateShow = value;

                    customButtonState.Visible = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：Caption属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("标题文本"), Category("TitleBarControl 通用")]
        public String Caption//属性
        {
            get//读取
            {
                return labelCaption.Text;
            }
            set//设置
            {
                if (value != labelCaption.Text)
                {
                    labelCaption.Text = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：CurrentBrand属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("当前品牌"), Category("TitleBarControl 通用")]
        public String CurrentBrand//属性
        {
            get//读取
            {
                return labelBrandValue.Text;
            }
            set//设置
            {
                if (value != labelBrandValue.Text)
                {
                    labelBrandValue.Text = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：PCTime属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("当前时间"), Category("TitleBarControl 通用")]
        public String PCTime//属性
        {
            get//读取
            {
                return labelTime.Text;
            }
            set//设置
            {
                if (value != labelTime.Text)
                {
                    labelTime.Text = value;
                    labelTime.Update();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：CurrentMachineType属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("当前机器类型"), Category("TitleBarControl 通用")]
        public String CurrentMachineType//属性
        {
            get//读取
            {
                return labelMachineValue.Text;
            }
            set//设置
            {
                if (value != labelMachineValue.Text)
                {
                    labelMachineValue.Text = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：CurrentShift属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("当前班次"), Category("TitleBarControl 通用")]
        public String CurrentShift//属性
        {
            get//读取
            {
                return labelShiftValue.Text;
            }
            set//设置
            {
                if (value != labelShiftValue.Text)
                {
                    labelShiftValue.Text = value;
                }
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：UserPassword属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("用户密码"), Category("TitleBarControl 通用")]
        public String UserPassword//属性
        {
            get//读取
            {
                return sUserPassword;
            }
            set//设置
            {
                if (value != sUserPassword)
                {
                    sUserPassword = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：AdministratorPassword属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("管理员密码"), Category("TitleBarControl 通用")]
        public String AdministratorPassword//属性
        {
            get//读取
            {
                return sAdministratorPassword;
            }
            set//设置
            {
                if (value != sAdministratorPassword)
                {
                    sAdministratorPassword = value;
                }
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：CameraName_Chinese属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("相机中文名称"), Category("TitleBarControl 通用")]
        public String[] CameraName_Chinese//属性
        {
            get//读取
            {
                return sCameraName_Chinese;
            }
            set//设置
            {
                if (value != sCameraName_Chinese)
                {
                    sCameraName_Chinese = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：CameraName_English属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("相机英文名称"), Category("TitleBarControl 通用")]
        public String[] CameraName_English//属性
        {
            get//读取
            {
                return sCameraName_English;
            }
            set//设置
            {
                if (value != sCameraName_English)
                {
                    sCameraName_English = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：FaultExist属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("每个相机的故障信息是否存在。取值范围：true，存在；false，不存在"), Category("TitleBarControl 通用")]
        public Boolean[] FaultExist//属性
        {
            get//读取
            {
                return bFaultExist;
            }
            set//设置
            {
                if (value != bFaultExist)
                {
                    bFaultExist = value;
                }
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：WindowParameter属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("调用的键盘窗口特征数值"), Category("TitleBarControl 通用")]
        public Int32 WindowParameter//属性
        {
            get//读取
            {
                return iWindowParameter;
            }
            set//设置
            {
                iWindowParameter = value;

                //

                if (15 == iWindowParameter)//WORK，点击【STATE】按钮显示密码窗口
                {
                    GlobalWindows.StandardKeyboard_Window.WindowClose_Work_State += new System.EventHandler(standardKeyboardWindow_WindowClose_Work_State);//订阅事件
                }
                else if (16 == iWindowParameter)//SYSTEM，点击【STATE】按钮显示密码窗口
                {
                    GlobalWindows.StandardKeyboard_Window.WindowClose_System_State += new System.EventHandler(standardKeyboardWindow_WindowClose_System_State);//订阅事件
                }
                else if (17 == iWindowParameter)//DEVICES SETUP，点击【STATE】按钮显示密码窗口
                {
                    GlobalWindows.StandardKeyboard_Window.WindowClose_DevicesSetup_State += new System.EventHandler(standardKeyboardWindow_WindowClose_DevicesSetup_State);//订阅事件
                }
                else if (18 == iWindowParameter)//IMAGE CONFIGURATION，点击【STATE】按钮显示密码窗口
                {
                    GlobalWindows.StandardKeyboard_Window.WindowClose_ImageConfiguration_State += new System.EventHandler(standardKeyboardWindow_WindowClose_ImageConfiguration_State);//订阅事件
                }
                else if (19 == iWindowParameter)//BRAND MANAGEMENT，点击【STATE】按钮显示密码窗口
                {
                    GlobalWindows.StandardKeyboard_Window.WindowClose_BrandManagement_State += new System.EventHandler(standardKeyboardWindow_WindowClose_BrandManagement_State);//订阅事件
                }
                else if (20 == iWindowParameter)//BACKUP BRANDS，点击【STATE】按钮显示密码窗口
                {
                    GlobalWindows.StandardKeyboard_Window.WindowClose_BackupBrands_State += new System.EventHandler(standardKeyboardWindow_WindowClose_BackupBrands_State);//订阅事件
                }
                else if (21 == iWindowParameter)//RESTORE BRANDS，点击【STATE】按钮显示密码窗口
                {
                    GlobalWindows.StandardKeyboard_Window.WindowClose_RestoreBrands_State += new System.EventHandler(standardKeyboardWindow_WindowClose_RestoreBrands_State);//订阅事件
                }
                else if (22 == iWindowParameter)//QUALITY CHECK，点击【STATE】按钮显示密码窗口
                {
                    GlobalWindows.StandardKeyboard_Window.WindowClose_QualityCheck_State += new System.EventHandler(standardKeyboardWindow_WindowClose_QualityCheck_State);//订阅事件
                }
                else if (23 == iWindowParameter)//TOLERANCES SETTINGS，点击【STATE】按钮显示密码窗口
                {
                    GlobalWindows.StandardKeyboard_Window.WindowClose_TolerancesSettings_State += new System.EventHandler(standardKeyboardWindow_WindowClose_TolerancesSettings_State);//订阅事件
                }
                else if (24 == iWindowParameter)//LIVE VIEW，点击【STATE】按钮显示密码窗口
                {
                    GlobalWindows.StandardKeyboard_Window.WindowClose_LiveView_State += new System.EventHandler(standardKeyboardWindow_WindowClose_LiveView_State);//订阅事件
                }
                else if (25 == iWindowParameter)//REJECTS VIEW，点击【STATE】按钮显示密码窗口
                {
                    GlobalWindows.StandardKeyboard_Window.WindowClose_RejectsView_State += new System.EventHandler(standardKeyboardWindow_WindowClose_RejectsView_State);//订阅事件
                }
                else if (26 == iWindowParameter)//STATISTICS，点击【STATE】按钮显示密码窗口
                {
                    GlobalWindows.StandardKeyboard_Window.WindowClose_Statistics_State += new System.EventHandler(standardKeyboardWindow_WindowClose_Statistics_State);//订阅事件
                }
                else//其它
                {
                    //不执行操作
                }
            }
        }

        //函数

        //----------------------------------------------------------------------
        // 功能说明：打开FAULT MESSAGE窗口
        // 输入参数：1.system：系统
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _OpenFaultMessageWindow(VisionSystemClassLibrary.Class.System system)
        {
            GlobalWindows.FaultMessage_Window.FaultMessageControl.Language = language;
            GlobalWindows.FaultMessage_Window.FaultMessageControl._Properties(system);

            GlobalWindows.FaultMessage_Window.StartPosition = FormStartPosition.CenterScreen;
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.FaultMessage_Window.TopMost = true;//将窗口置于顶层
            }
            else//其它
            {
                GlobalWindows.FaultMessage_Window.Visible = true;//显示
            }

            //

            GlobalWindows.FaultMessage_Window.FaultMessageControl._StartGetData();
        }

        //----------------------------------------------------------------------
        // 功能说明：设置故障信息
        // 输入参数：1.faultmessage：故障信息
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetBackground(VisionSystemClassLibrary.Struct.FaultMessage faultmessage)
        {
            if (0 < faultmessage.DataIndex)//存在故障信息
            {
                this.BackgroundImage = bitmapBackground[1];
            }
            else//无故障信息
            {
                this.BackgroundImage = bitmapBackground[0];
            }

            //

            Update();
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：检查故障信息状态
        // 输入参数：无
        // 输出参数：无
        // 返回值：故障信息是否存在。取值范围：true，是；false，否
        //----------------------------------------------------------------------
        private Boolean _CheckFaultMessageState()
        {
            Boolean bReturn = false;

            for (Int32 i = 0; i < bFaultExist.Length; i++)
            {
                if (bFaultExist[i])
                {
                    bReturn = true;

                    break;
                }
            }

            return bReturn;
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：设置故障信息
        // 输入参数：1.iIndex：相机索引值
        //         2.faultmessage：故障信息
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _SetFaultMessage(Int32 iIndex, VisionSystemClassLibrary.Struct.FaultMessage faultmessage)
        {
            if (0 < faultmessage.DataIndex)//故障
            {
                _GetFaultMessage(iIndex, faultmessage);

                bFaultExist[iIndex] = true;

                //

                if (!labelFaultMessage.Visible)//不显示
                {
                    _SetBackground(faultmessage);

                    labelFaultMessage.Visible = true;
                }
            }
            else//无故障
            {
                bFaultExist[iIndex] = false;

                //

                if (_CheckFaultMessageState())//存在故障
                {
                    if (!labelFaultMessage.Visible)//不显示
                    {
                        labelFaultMessage.Visible = true;
                    }
                }
                else//不存在故障
                {
                    _SetBackground(faultmessage);

                    if (labelFaultMessage.Visible)//显示
                    {
                        labelFaultMessage.Visible = false;
                    }
                }
            }

            //

            Update();
        }

        //----------------------------------------------------------------------
        // 功能说明：复位故障信息
        // 输入参数：1.bVisible：控件是否显示。取值范围：true，是；false，否
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _ResetFaultMessage()
        {
            labelFaultMessage.Visible = false;//故障信息

            labelFaultMessage.Text = "";//设置显示的文

            if (null != bFaultExist)//有效
            {
                for (Int32 i = 0; i < bFaultExist.Length; i++)//复位
                {
                    bFaultExist[i] = false;
                }
            }

            VisionSystemClassLibrary.Struct.FaultMessage faultmessage = new VisionSystemClassLibrary.Struct.FaultMessage();
            faultmessage._InitData();

            _SetBackground(faultmessage);
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：获取故障信息
        // 输入参数：1.iIndex：相机索引值
        //         2.faultmessage：故障信息
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _GetFaultMessage(Int32 iIndex, VisionSystemClassLibrary.Struct.FaultMessage faultmessage)
        {
            String sFaultMessage = VisionSystemClassLibrary.Class.System._GetFaultMessage(language, faultmessage);

            switch (language)
            {
                case VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese://中文

                    labelFaultMessage.Text = sCameraName_Chinese[iIndex] + "：" + sFaultMessage;

                    break;
                case VisionSystemClassLibrary.Enum.InterfaceLanguage.English://英文

                    labelFaultMessage.Text = sCameraName_English[iIndex] + "：" + sFaultMessage;

                    break;
                default://其它，默认中文

                    labelFaultMessage.Text = sCameraName_Chinese[iIndex] + "：" + sFaultMessage;

                    break;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：设置语言
        // 输入参数：1.language_parameter：语言
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetLanguage()
        {
            customButtonNetCheck.Language = language;//【NET CHECK】
            customButtonState.Language = language;//【STATE】

            //

            labelInspectText.Text = sMessageText[(Int32)language - 1][0] + "：";
            labelStopText.Text = sMessageText[(Int32)language - 1][1];
            labelReadyText.Text = sMessageText[(Int32)language - 1][2];
            labelRunText.Text = sMessageText[(Int32)language - 1][3];
            labelBrandChangedText.Text = sMessageText[(Int32)language - 1][4];
            labelBrandText.Text = sMessageText[(Int32)language - 1][5] + "：";
            labelMachineText.Text = sMessageText[(Int32)language - 1][7] + "：";
            labelShiftText.Text = sMessageText[(Int32)language - 1][8] + "：";
        }

        //----------------------------------------------------------------------
        // 功能说明：设置设备状态
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetDeviceState()
        {
            if (VisionSystemClassLibrary.Enum.DeviceState.None == devicestate)//无效
            {
                labelStopValue.Image = bitmapInspectValue[4];//STOP
                labelReadyValue.Image = bitmapInspectValue[4];//READY
                labelRunValue.Image = bitmapInspectValue[4];//RUN
                labelBrandChangedValue.Image = bitmapInspectValue[4];//B.CHG

                customButtonState.Visible = false;//隐藏【STATE】按钮
            }
            else if (VisionSystemClassLibrary.Enum.DeviceState.Stop == devicestate)//停止
            {
                labelStopValue.Image = bitmapInspectValue[0];//STOP
                labelReadyValue.Image = bitmapInspectValue[4];//READY
                labelRunValue.Image = bitmapInspectValue[4];//RUN
                labelBrandChangedValue.Image = bitmapInspectValue[4];//B.CHG

                customButtonState.BackgroundColor = System.Drawing.Color.FromArgb(192, 0, 0);

                customButtonState.CurrentTextGroupIndex = 1;
                customButtonState.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//更新背景
                customButtonState.Visible = true;//显示【STATE】按钮
            }
            else if (VisionSystemClassLibrary.Enum.DeviceState.Ready == devicestate)//准备
            {
                labelStopValue.Image = bitmapInspectValue[4];//STOP
                labelReadyValue.Image = bitmapInspectValue[1];//READY
                labelRunValue.Image = bitmapInspectValue[4];//RUN
                labelBrandChangedValue.Image = bitmapInspectValue[4];//B.CHG

                customButtonState.BackgroundColor = System.Drawing.Color.FromArgb(0, 138, 206);

                customButtonState.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//更新背景
                customButtonState.Visible = true;//显示【STATE】按钮
            }
            else if (VisionSystemClassLibrary.Enum.DeviceState.Run == devicestate)//运行
            {
                labelStopValue.Image = bitmapInspectValue[4];//STOP
                labelReadyValue.Image = bitmapInspectValue[4];//READY
                labelRunValue.Image = bitmapInspectValue[2];//RUN
                labelBrandChangedValue.Image = bitmapInspectValue[4];//B.CHG

                customButtonState.BackgroundColor = System.Drawing.Color.FromArgb(0, 138, 206);

                customButtonState.CurrentTextGroupIndex = 0;
                customButtonState.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//更新背景
                customButtonState.Visible = true;//显示【STATE】按钮
            }
            else//VisionSystemClassLibrary.Enum.DeviceState.BrandChanged，品牌更改
            {
                labelStopValue.Image = bitmapInspectValue[4];//STOP
                labelReadyValue.Image = bitmapInspectValue[4];//READY
                labelRunValue.Image = bitmapInspectValue[4];//RUN
                labelBrandChangedValue.Image = bitmapInspectValue[3];//B.CHG

                customButtonState.BackgroundColor = System.Drawing.Color.FromArgb(0, 138, 206);

                customButtonState.Visible = true;//显示【STATE】按钮
            }

            //

            Update();
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：点击【STATE】按钮，执行相关操作
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _ClickStateButton()
        {
            if (VisionSystemClassLibrary.Enum.DeviceState.Run == devicestate)//当前设备状态，运行
            {
                devicestate = VisionSystemClassLibrary.Enum.DeviceState.Stop;//设置为停止

                VisionSystemClassLibrary.Class.System.SystemDeviceState = VisionSystemClassLibrary.Enum.DeviceState.Stop;//设置为停止
            }
            else if (VisionSystemClassLibrary.Enum.DeviceState.Stop == devicestate)//当前设备状态，停止
            {
                devicestate = VisionSystemClassLibrary.Enum.DeviceState.Run;//设置为停止

                VisionSystemClassLibrary.Class.System.SystemDeviceState = VisionSystemClassLibrary.Enum.DeviceState.Run;//设置为停止
            }
            else//其它
            {
                //不执行操作
            }

            //

            _SetDeviceState();//设置设备状态

            //事件

            if (null != State_Click)//有效
            {
                State_Click(this, new EventArgs());
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：检查密码
        // 输入参数：无
        // 输出参数：无
        // 返回值：检查状态。取值范围：true，无密码；false，有密码
        //----------------------------------------------------------------------
        private Boolean _CheckPassword()
        {
            Boolean bReturn = false;//返回值

            //

            if ("" == sUserPassword)//无密码
            {
                bReturn = true;
            }
            else//密码保护
            {
                if (CustomButton_BackgroundImage.Up == (customButtonState.CustomButtonBackgroundImage & CustomButton_BackgroundImage.Up))
                {
                    customButtonState.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
                }
                else
                {
                    customButtonState.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                }

                //

                bReturn = false;

                //

                GlobalWindows.StandardKeyboard_Window.WindowParameter = iWindowParameter;//窗口特征数值
                GlobalWindows.StandardKeyboard_Window.Language = language;//语言
                GlobalWindows.StandardKeyboard_Window.Chinese_Caption = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][6];//中文标题文本
                GlobalWindows.StandardKeyboard_Window.English_Caption = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][6];//英文标题文本
                GlobalWindows.StandardKeyboard_Window.IsPassword = true;//密码输入窗口
                GlobalWindows.StandardKeyboard_Window.PasswordStyle = 0;//属性，密码输入类型。取值范围：0，密码输入（输入完成，正确，关闭窗口）；1，输入当前密码；2，输入新的密码；3，确认密码
                GlobalWindows.StandardKeyboard_Window.Password = sUserPassword + "\n" + sAdministratorPassword;//属性，当前密码
                GlobalWindows.StandardKeyboard_Window.CapsLock = true;//Caps Lock打开
                GlobalWindows.StandardKeyboard_Window.Shift = false;//SHIFT按下
                GlobalWindows.StandardKeyboard_Window.MaxLength = 8;//数值长度范围
                GlobalWindows.StandardKeyboard_Window.StringValue = "";//初始显示的数值

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

            //

            return bReturn;
        }

        //事件

        //----------------------------------------------------------------------
        // 功能说明：控件加载事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void TitleBarControl_Load(object sender, EventArgs e)
        {
            //不执行操作
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【NET CHECK】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonNetCheck_CustomButton_Click(object sender, EventArgs e)
        {
            //事件

            if (null != NetCheck_Click)//有效
            {
                NetCheck_Click(this, new EventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【STATE】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonState_CustomButton_Click(object sender, EventArgs e)
        {
            if (_CheckPassword())//密码检查
            {
                _ClickStateButton();
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：WORK，点击【STATE】按钮显示密码窗口，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void standardKeyboardWindow_WindowClose_Work_State(object sender, EventArgs e)
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

            if (GlobalWindows.StandardKeyboard_Window.EnterNewValue)//
            {
                _ClickStateButton();
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：SYSTEM，点击【STATE】按钮显示密码窗口，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void standardKeyboardWindow_WindowClose_System_State(object sender, EventArgs e)
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

            if (GlobalWindows.StandardKeyboard_Window.EnterNewValue)//
            {
                _ClickStateButton();
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：DEVICES SETUP，点击【STATE】按钮显示密码窗口，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void standardKeyboardWindow_WindowClose_DevicesSetup_State(object sender, EventArgs e)
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

            if (GlobalWindows.StandardKeyboard_Window.EnterNewValue)//
            {
                _ClickStateButton();
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：IMAGE CONFIGURATION，点击【STATE】按钮显示密码窗口，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void standardKeyboardWindow_WindowClose_ImageConfiguration_State(object sender, EventArgs e)
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

            if (GlobalWindows.StandardKeyboard_Window.EnterNewValue)//
            {
                _ClickStateButton();
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：BRAND MANAGEMENT，点击【STATE】按钮显示密码窗口，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void standardKeyboardWindow_WindowClose_BrandManagement_State(object sender, EventArgs e)
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

            if (GlobalWindows.StandardKeyboard_Window.EnterNewValue)//
            {
                _ClickStateButton();
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：BACKUP BRANDS，点击【STATE】按钮显示密码窗口，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void standardKeyboardWindow_WindowClose_BackupBrands_State(object sender, EventArgs e)
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

            if (GlobalWindows.StandardKeyboard_Window.EnterNewValue)//
            {
                _ClickStateButton();
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：RESTORE BRANDS，点击【STATE】按钮显示密码窗口，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void standardKeyboardWindow_WindowClose_RestoreBrands_State(object sender, EventArgs e)
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

            if (GlobalWindows.StandardKeyboard_Window.EnterNewValue)//
            {
                _ClickStateButton();
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：QUALITY CHECK，点击【STATE】按钮显示密码窗口，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void standardKeyboardWindow_WindowClose_QualityCheck_State(object sender, EventArgs e)
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

            if (GlobalWindows.StandardKeyboard_Window.EnterNewValue)//
            {
                _ClickStateButton();
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：TOLERANCES SETTINGS，点击【STATE】按钮显示密码窗口，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void standardKeyboardWindow_WindowClose_TolerancesSettings_State(object sender, EventArgs e)
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

            if (GlobalWindows.StandardKeyboard_Window.EnterNewValue)//
            {
                _ClickStateButton();
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：LIVE VIEW，点击【STATE】按钮显示密码窗口，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void standardKeyboardWindow_WindowClose_LiveView_State(object sender, EventArgs e)
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

            if (GlobalWindows.StandardKeyboard_Window.EnterNewValue)//
            {
                _ClickStateButton();
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：REJECTS VIEW，点击【STATE】按钮显示密码窗口，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void standardKeyboardWindow_WindowClose_RejectsView_State(object sender, EventArgs e)
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

            if (GlobalWindows.StandardKeyboard_Window.EnterNewValue)//
            {
                _ClickStateButton();
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：STATISTICS，点击【STATE】按钮显示密码窗口，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void standardKeyboardWindow_WindowClose_Statistics_State(object sender, EventArgs e)
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

            if (GlobalWindows.StandardKeyboard_Window.EnterNewValue)//
            {
                _ClickStateButton();
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：单击标题控件的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void labelCaption_Click(object sender, EventArgs e)
        {
            if (!customButtonNetCheck.Visible)//不显示
            {
                if (bStateShow)//显示
                {
                    //事件

                    if (null != GetFaultMessages)//有效
                    {
                        GetFaultMessages(this, e);
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：单击FAULT MESSAGE控件的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void labelFaultMessage_Click(object sender, EventArgs e)
        {
            //事件

            if (null != GetFaultMessages)//有效
            {
                GetFaultMessages(this, e);
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：双击控件的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void labelTime_DoubleClick(object sender, EventArgs e)
        {
            //事件

            if (null != ChangeInterface)//有效
            {
                ChangeInterface(this, e);
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：双击控件的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void labelMachineText_DoubleClick(object sender, EventArgs e)
        {
            //事件

            if (null != ChangeInterface)//有效
            {
                ChangeInterface(this, e);
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：双击控件的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void labelMachineValue_DoubleClick(object sender, EventArgs e)
        {
            //事件

            if (null != ChangeInterface)//有效
            {
                ChangeInterface(this, e);
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：双击控件的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void labelBrandText_DoubleClick(object sender, EventArgs e)
        {
            //事件

            if (null != ChangeInterface)//有效
            {
                ChangeInterface(this, e);
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：双击控件的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void labelBrandValue_DoubleClick(object sender, EventArgs e)
        {
            //事件

            if (null != ChangeInterface)//有效
            {
                ChangeInterface(this, e);
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：双击控件的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void labelShiftText_DoubleClick(object sender, EventArgs e)
        {
            //事件

            if (null != ChangeInterface)//有效
            {
                ChangeInterface(this, e);
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：双击控件的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void labelShiftValue_DoubleClick(object sender, EventArgs e)
        {
            //事件

            if (null != ChangeInterface)//有效
            {
                ChangeInterface(this, e);
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：双击控件的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void TitleBarControl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.X > 914) //鼠标点击后面空白区域
            {
                //事件

                if (null != ChangeInterface)//有效
                {
                    ChangeInterface(this, e);
                }
            }
        }
    }
}