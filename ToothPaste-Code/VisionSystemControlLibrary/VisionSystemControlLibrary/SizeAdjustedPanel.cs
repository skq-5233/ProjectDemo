/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：CustomButton.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：尺寸调整控件

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

namespace VisionSystemControlLibrary
{
    public enum RegionSelectionType
    {
        None = 0,
        Left = 1,
        LeftTop = 2,
        Top = 3,
        RightTop = 4,
        Right = 5,
        LeftBottom = 6,
        Bottom = 7,
        RightBottom = 8,
    }

    public partial class SizeAdjustedPanel : UserControl
    {
        //尺寸调整控件

        private VisionSystemClassLibrary.Enum.InterfaceLanguage language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;//属性，语言

        //

        private RegionSelectionType regionSelectionType = RegionSelectionType.None;//属性，选择类型

        //

        private Boolean[] bMoveButtonEnabled = new Boolean[8];//属性，各个调整按钮使能状态。取值范围：true，使能；false，禁止
        private Boolean bSettingInnerROI = false;//默认设置外部ROI

        private Rectangle rectangleMaxROI;//属性，兴趣区域最大范围

        private VisionSystemClassLibrary.Struct.ROI rectangleROI = new VisionSystemClassLibrary.Struct.ROI();//属性，操作的兴趣区域

        private ContentAlignment contentalignmentHandleType = ContentAlignment.MiddleCenter;//属性，兴趣区域调整手柄类型（ContentAlignment.MiddleCenter表示无效）

        //

        private bool bEnabled = true;//属性，控件使能状态。取值范围：true，使能；false：禁止

        //

        Thread ThreadMouseDown = null;//Mouse Down，线程

        private Boolean bMouseDown = false;//Mouse Down

        private Int32 iMouseDown_Count = 0;//Mouse Down，Time1
        private Int32 iMouseDown_Count_Max = 20;//Mouse Down，Time1
        private Int32 iMouseDown_Time = 100;//Mouse Down，Time2

        //

        [Browsable(true), Description("点击【Top】按钮时产生的事件"), Category("SizeAdjustedPanel 事件")]
        public event EventHandler Top_Click;

        [Browsable(true), Description("点击【Bottom】按钮时产生的事件"), Category("SizeAdjustedPanel 事件")]
        public event EventHandler Bottom_Click;

        [Browsable(true), Description("点击【Left】按钮时产生的事件"), Category("SizeAdjustedPanel 事件")]
        public event EventHandler Left_Click;

        [Browsable(true), Description("点击【Right】按钮时产生的事件"), Category("SizeAdjustedPanel 事件")]
        public event EventHandler Right_Click;

        [Browsable(true), Description("点击【LeftTop】按钮时产生的事件"), Category("SizeAdjustedPanel 事件")]
        public event EventHandler LeftTop_Click;

        [Browsable(true), Description("点击【RightTop】按钮时产生的事件"), Category("SizeAdjustedPanel 事件")]
        public event EventHandler RightTop_Click;

        [Browsable(true), Description("点击【LeftBottom】按钮时产生的事件"), Category("SizeAdjustedPanel 事件")]
        public event EventHandler LeftBottom_Click;

        [Browsable(true), Description("点击【RightBottom】按钮时产生的事件"), Category("SizeAdjustedPanel 事件")]
        public event EventHandler RightBottom_Click;

        [Browsable(true), Description("点击【Center】按钮时产生的事件"), Category("SizeAdjustedPanel 事件")]
        public event EventHandler Center_Click;

        [Browsable(true), Description("鼠标指针在【Center】按钮上移动时产生的事件"), Category("SizeAdjustedPanel 事件")]
        public event EventHandler Center_MouseMove;

        [Browsable(true), Description("点击【Close】按钮时产生的事件"), Category("SizeAdjustedPanel 事件")]
        public event EventHandler Close_Click;

        [Browsable(true), Description("点击【Home】按钮时产生的事件"), Category("SizeAdjustedPanel 事件")]
        public event EventHandler Home_Click;

        //

        [Browsable(true), Description("点击【REGION SELECTION】按钮时产生的事件"), Category("SizeAdjustedPanel 事件")]
        public event EventHandler RegionSelection_Click;

        [Browsable(true), Description("点击【SHOW/HIDE】按钮时产生的事件"), Category("SizeAdjustedPanel 事件")]
        public event EventHandler ShowHide_Click;

        //构造函数

        //----------------------------------------------------------------------
        // 功能说明：构造函数，初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public SizeAdjustedPanel()
        {
            InitializeComponent();

            //

            Int32 i = 0;//循环控制变量

            for (i = 0; i < bMoveButtonEnabled.Length; i++)
            {
                bMoveButtonEnabled[i] = true;
            }
        }

        //属性

        [Browsable(true), Description("操作外部/内部区域标记"), Category("SizeAdjustedPanel 通用")]
        public Boolean SettingInnerROI
        {
            set//设置
            {
                bSettingInnerROI = value;

                _UpdateSizeAdjustedPanel(bSettingInnerROI);
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Language属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("语言"), Category("SizeAdjustedPanel 通用")]
        public VisionSystemClassLibrary.Enum.InterfaceLanguage Language
        {
            get//读取
            {
                return language;
            }
            set//设置
            {
                if (value != language)//不同
                {
                    language = value;

                    //

                    _SetLanguage();
                }
            }
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：rectangleMaxROI属性
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(false), Description("兴趣区域最大范围"), Category("SizeAdjustedPanel 通用")]
        public Rectangle RectangleMaxROI
        {
            get
            {
                return rectangleMaxROI;
            }
            set
            {
                rectangleMaxROI = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：RectangleROI属性
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(false), Description("操作的兴趣区域"), Category("SizeAdjustedPanel 通用")]
        public VisionSystemClassLibrary.Struct.ROI RectangleROI
        {
            get
            {
                return rectangleROI;
            }
            set
            {
                rectangleROI = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：HandleType属性
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(false), Description("兴趣区域调整手柄类型"), Category("SizeAdjustedPanel 通用")]
        public ContentAlignment HandleType
        {
            get
            {
                return contentalignmentHandleType;
            }
            set
            {
                contentalignmentHandleType = value;

                _UpdateSizeAdjustedPanel(bSettingInnerROI);
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ControlEnabled属性
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(false), Description("控件使能状态。取值范围：true，使能；false：禁止"), Category("SizeAdjustedPanel 通用")]
        public Boolean ControlEnabled
        {
            get
            {
                return bEnabled;
            }
            set
            {
                bEnabled = value;

                //

                if (bEnabled)//使能
                {
                    _SetButtonEnabled(customButtonLeft, bMoveButtonEnabled[0]);
                    _SetButtonEnabled(customButtonLeftTop, bMoveButtonEnabled[1]);
                    _SetButtonEnabled(customButtonTop, bMoveButtonEnabled[2]);
                    _SetButtonEnabled(customButtonRightTop, bMoveButtonEnabled[3]);
                    _SetButtonEnabled(customButtonRight, bMoveButtonEnabled[4]);
                    _SetButtonEnabled(customButtonLeftBottom, bMoveButtonEnabled[5]);
                    _SetButtonEnabled(customButtonBottom, bMoveButtonEnabled[6]);
                    _SetButtonEnabled(customButtonBottom, bMoveButtonEnabled[7]);

                    customButtonMove.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;

                    customButtonHome.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                    customButtonClose.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;

                    customButtonShowHide.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                } 
                else//禁止
                {
                    customButtonLeftTop.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                    customButtonTop.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                    customButtonRightTop.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                    customButtonLeft.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                    customButtonRight.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                    customButtonLeftBottom.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                    customButtonBottom.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                    customButtonRightBottom.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                    customButtonMove.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;

                    customButtonHome.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                    customButtonClose.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;

                    customButtonShowHide.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                }

                //

                _SetSelectionButton();
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：SelectionType属性
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(false), Description("选择类型"), Category("SizeAdjustedPanel 通用")]
        public RegionSelectionType SelectionType
        {
            get
            {
                return regionSelectionType;
            }
            set
            {
                regionSelectionType = value;

                //

                _SetSelectionButton();
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ShowHideButton属性
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(false), Description("【SHOW】/【HIDE】按钮类型。取值范围：true，【SHOW】；false，【HIDE】"), Category("SizeAdjustedPanel 通用")]
        public Boolean ShowHideButton
        {
            get
            {
                if (0 == customButtonShowHide.CurrentTextGroupIndex)//【HIDE】
                {
                    return false;
                }
                else//【SHOW】
                {
                    return true;
                }
            }
            set
            {
                if (value)//【SHOW】
                {
                    customButtonShowHide.CurrentTextGroupIndex = 1;
                }
                else//【HIDE】
                {
                    customButtonShowHide.CurrentTextGroupIndex = 0;
                }
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
            customButtonHome.Language = language;//【HOME】
            customButtonClose.Language = language;//【CLOSE】
            customButtonShowHide.Language = language;//【SHOW】/【HIDE】
        }

        //----------------------------------------------------------------------
        // 功能说明：设置按钮使能状态
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetButtonEnabled(CustomButton custombutton, Boolean benabled)
        {
            if (benabled)//使能
            {
                custombutton.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
            }
            else//禁止
            {
                custombutton.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：设置按钮使能状态
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetSelectionButton()
        {
            switch (regionSelectionType)
            {
                case RegionSelectionType.None:

                    _SetButtonEnabled_1(customButtonLeft_1, false);//【LEFT】
                    _SetButtonEnabled_1(customButtonLeftTop_1, false);//【LEFT TOP】
                    _SetButtonEnabled_1(customButtonTop_1, false);//【TOP】
                    _SetButtonEnabled_1(customButtonRightTop_1, false);//【RIGHT TOP】
                    _SetButtonEnabled_1(customButtonRight_1, false);//【RIGHT】
                    _SetButtonEnabled_1(customButtonLeftBottom_1, false);//【LEFT BOTTOM】
                    _SetButtonEnabled_1(customButtonBottom_1, false);//【BOTTOM】
                    _SetButtonEnabled_1(customButtonRightBottom_1, false);//【RIGHT BOTTOM】

                    break;
                case RegionSelectionType.Left:

                    _SetButtonEnabled_1(customButtonLeft_1, true);//【LEFT】
                    _SetButtonEnabled_1(customButtonLeftTop_1, false);//【LEFT TOP】
                    _SetButtonEnabled_1(customButtonTop_1, false);//【TOP】
                    _SetButtonEnabled_1(customButtonRightTop_1, false);//【RIGHT TOP】
                    _SetButtonEnabled_1(customButtonRight_1, false);//【RIGHT】
                    _SetButtonEnabled_1(customButtonLeftBottom_1, false);//【LEFT BOTTOM】
                    _SetButtonEnabled_1(customButtonBottom_1, false);//【BOTTOM】
                    _SetButtonEnabled_1(customButtonRightBottom_1, false);//【RIGHT BOTTOM】

                    break;
                case RegionSelectionType.LeftTop:

                    _SetButtonEnabled_1(customButtonLeft_1, false);//【LEFT】
                    _SetButtonEnabled_1(customButtonLeftTop_1, true);//【LEFT TOP】
                    _SetButtonEnabled_1(customButtonTop_1, false);//【TOP】
                    _SetButtonEnabled_1(customButtonRightTop_1, false);//【RIGHT TOP】
                    _SetButtonEnabled_1(customButtonRight_1, false);//【RIGHT】
                    _SetButtonEnabled_1(customButtonLeftBottom_1, false);//【LEFT BOTTOM】
                    _SetButtonEnabled_1(customButtonBottom_1, false);//【BOTTOM】
                    _SetButtonEnabled_1(customButtonRightBottom_1, false);//【RIGHT BOTTOM】

                    break;
                case RegionSelectionType.Top:

                    _SetButtonEnabled_1(customButtonLeft_1, false);//【LEFT】
                    _SetButtonEnabled_1(customButtonLeftTop_1, false);//【LEFT TOP】
                    _SetButtonEnabled_1(customButtonTop_1, true);//【TOP】
                    _SetButtonEnabled_1(customButtonRightTop_1, false);//【RIGHT TOP】
                    _SetButtonEnabled_1(customButtonRight_1, false);//【RIGHT】
                    _SetButtonEnabled_1(customButtonLeftBottom_1, false);//【LEFT BOTTOM】
                    _SetButtonEnabled_1(customButtonBottom_1, false);//【BOTTOM】
                    _SetButtonEnabled_1(customButtonRightBottom_1, false);//【RIGHT BOTTOM】

                    break;
                case RegionSelectionType.RightTop:

                    _SetButtonEnabled_1(customButtonLeft_1, false);//【LEFT】
                    _SetButtonEnabled_1(customButtonLeftTop_1, false);//【LEFT TOP】
                    _SetButtonEnabled_1(customButtonTop_1, false);//【TOP】
                    _SetButtonEnabled_1(customButtonRightTop_1, true);//【RIGHT TOP】
                    _SetButtonEnabled_1(customButtonRight_1, false);//【RIGHT】
                    _SetButtonEnabled_1(customButtonLeftBottom_1, false);//【LEFT BOTTOM】
                    _SetButtonEnabled_1(customButtonBottom_1, false);//【BOTTOM】
                    _SetButtonEnabled_1(customButtonRightBottom_1, false);//【RIGHT BOTTOM】

                    break;
                case RegionSelectionType.Right:

                    _SetButtonEnabled_1(customButtonLeft_1, false);//【LEFT】
                    _SetButtonEnabled_1(customButtonLeftTop_1, false);//【LEFT TOP】
                    _SetButtonEnabled_1(customButtonTop_1, false);//【TOP】
                    _SetButtonEnabled_1(customButtonRightTop_1, false);//【RIGHT TOP】
                    _SetButtonEnabled_1(customButtonRight_1, true);//【RIGHT】
                    _SetButtonEnabled_1(customButtonLeftBottom_1, false);//【LEFT BOTTOM】
                    _SetButtonEnabled_1(customButtonBottom_1, false);//【BOTTOM】
                    _SetButtonEnabled_1(customButtonRightBottom_1, false);//【RIGHT BOTTOM】

                    break;
                case RegionSelectionType.LeftBottom:

                    _SetButtonEnabled_1(customButtonLeft_1, false);//【LEFT】
                    _SetButtonEnabled_1(customButtonLeftTop_1, false);//【LEFT TOP】
                    _SetButtonEnabled_1(customButtonTop_1, false);//【TOP】
                    _SetButtonEnabled_1(customButtonRightTop_1, false);//【RIGHT TOP】
                    _SetButtonEnabled_1(customButtonRight_1, false);//【RIGHT】
                    _SetButtonEnabled_1(customButtonLeftBottom_1, true);//【LEFT BOTTOM】
                    _SetButtonEnabled_1(customButtonBottom_1, false);//【BOTTOM】
                    _SetButtonEnabled_1(customButtonRightBottom_1, false);//【RIGHT BOTTOM】

                    break;
                case RegionSelectionType.Bottom:

                    _SetButtonEnabled_1(customButtonLeft_1, false);//【LEFT】
                    _SetButtonEnabled_1(customButtonLeftTop_1, false);//【LEFT TOP】
                    _SetButtonEnabled_1(customButtonTop_1, false);//【TOP】
                    _SetButtonEnabled_1(customButtonRightTop_1, false);//【RIGHT TOP】
                    _SetButtonEnabled_1(customButtonRight_1, false);//【RIGHT】
                    _SetButtonEnabled_1(customButtonLeftBottom_1, false);//【LEFT BOTTOM】
                    _SetButtonEnabled_1(customButtonBottom_1, true);//【BOTTOM】
                    _SetButtonEnabled_1(customButtonRightBottom_1, false);//【RIGHT BOTTOM】

                    break;
                case RegionSelectionType.RightBottom:

                    _SetButtonEnabled_1(customButtonLeft_1, false);//【LEFT】
                    _SetButtonEnabled_1(customButtonLeftTop_1, false);//【LEFT TOP】
                    _SetButtonEnabled_1(customButtonTop_1, false);//【TOP】
                    _SetButtonEnabled_1(customButtonRightTop_1, false);//【RIGHT TOP】
                    _SetButtonEnabled_1(customButtonRight_1, false);//【RIGHT】
                    _SetButtonEnabled_1(customButtonLeftBottom_1, false);//【LEFT BOTTOM】
                    _SetButtonEnabled_1(customButtonBottom_1, false);//【BOTTOM】
                    _SetButtonEnabled_1(customButtonRightBottom_1, true);//【RIGHT BOTTOM】

                    break;
                default:
                    break;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：设置按钮使能状态
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetButtonEnabled_1(CustomButton custombutton, Boolean bSelected)
        {
            if (bEnabled)//使能
            {
                if (bSelected)//选择
                {
                    custombutton.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
                }
                else//未选择
                {
                    custombutton.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                }
            }
            else//禁止
            {
                custombutton.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
            }
        }

        //事件

        //-----------------------------------------------------------------------
        // 功能说明：窗口加载函数
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void SizeAdjustedPanel_Load(object sender, EventArgs e)
        {
            //不执行操作
        }

        //


        //-----------------------------------------------------------------------
        // 功能说明：【LEFT TOP】，MOUSE UP，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonLeftTop_MouseDown(object sender, MouseEventArgs e)
        {
            bMouseDown = true;

            if (null == ThreadMouseDown)//无效
            {
                ThreadMouseDown = new Thread(_threadLeftTopMouseDown);//加载线程
                ThreadMouseDown.IsBackground = true;
                ThreadMouseDown.Start();//启动线程
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：【LEFT TOP】，MOUSE DOWN，线程
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _threadLeftTopMouseDown()
        {
            while (bMouseDown)
            {
                Thread.Sleep(iMouseDown_Time);

                iMouseDown_Count++;

                if (iMouseDown_Count >= iMouseDown_Count_Max)
                {
                    break;
                }
            }

            while (bMouseDown)
            {
                this.Invoke(new EventHandler(delegate { this._CustomButtonLeftTop_MouseUp(customButtonLeftTop, null); }));

                Thread.Sleep(iMouseDown_Time);
            }

            //

            iMouseDown_Count = 0;
            ThreadMouseDown = null;
        }

        //-----------------------------------------------------------------------
        // 功能说明：【LEFT TOP】，MOUSE UP，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _CustomButtonLeftTop_MouseUp(object sender, MouseEventArgs e)
        {
            Boolean bOperateResult = false;
            VisionSystemClassLibrary.Struct.ROI rectangleROI_Temp = new VisionSystemClassLibrary.Struct.ROI();
            rectangleROI._CopyTo(ref rectangleROI_Temp);

            VisionSystemClassLibrary.Struct.ROI_Inner roi_Inner = rectangleROI_Temp.roiExtra;

            if (bSettingInnerROI) //设置内部区域
            {
                roi_Inner = rectangleROI_Temp.roiInner;
            }

            switch (roi_Inner.roiType)
            {
                case VisionSystemClassLibrary.Enum.ROIType.Quadrangle:
                    switch (contentalignmentHandleType)
                    {
                        case ContentAlignment.TopLeft://点击手柄上左按钮
                            roi_Inner.Point1.X--;
                            roi_Inner.Point1.Y--;
                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, true)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                        case ContentAlignment.TopRight:
                            roi_Inner.Point2.X--;
                            roi_Inner.Point2.Y--;
                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, true)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                        case ContentAlignment.BottomLeft:
                            roi_Inner.Point4.X--;
                            roi_Inner.Point4.Y--;
                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, true)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                        case ContentAlignment.BottomRight:
                            roi_Inner.Point3.X--;
                            roi_Inner.Point3.Y--;
                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, true)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                        default:
                            roi_Inner._Offset(-1, -1);
                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, false)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                    }
                    break;
                case VisionSystemClassLibrary.Enum.ROIType.Ellipse:
                    roi_Inner.Point1.X--;
                    roi_Inner.Point1.Y--;

                    if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, false)) //满足条件
                    {
                        bOperateResult = true;
                    }
                    break;
                case VisionSystemClassLibrary.Enum.ROIType.Rectangle:
                    switch (contentalignmentHandleType)
                    {
                        case ContentAlignment.TopLeft:
                            roi_Inner.Point1.X--;
                            roi_Inner.Point1.Y--;
                            roi_Inner.Point2.X++;
                            roi_Inner.Point2.Y++;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, true)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                        case ContentAlignment.BottomRight:
                            roi_Inner.Point2.X--;
                            roi_Inner.Point2.Y--;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, true)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                        default:
                            roi_Inner.Point1.X--;
                            roi_Inner.Point1.Y--;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, false)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                    }
                    break;
                default:
                    break;
            }

            //

            if (bOperateResult)//有效
            {
                rectangleROI_Temp._CopyTo(ref rectangleROI);

                //事件

                if (null != LeftTop_Click)//有效
                {
                    LeftTop_Click(this, e);
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：【LEFT TOP】，MOUSE UP，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonLeftTop_MouseUp(object sender, MouseEventArgs e)
        {
            bMouseDown = false;
            
            //

            _CustomButtonLeftTop_MouseUp(sender, e);
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：【TOP】，MOUSE DOWN，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonTop_MouseDown(object sender, MouseEventArgs e)
        {
            bMouseDown = true;

            if (null == ThreadMouseDown)//无效
            {
                ThreadMouseDown = new Thread(_threadTopMouseDown);//加载线程
                ThreadMouseDown.IsBackground = true;
                ThreadMouseDown.Start();//启动线程
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：【TOP】，MOUSE DOWN，线程
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _threadTopMouseDown()
        {
            while (bMouseDown)
            {
                Thread.Sleep(iMouseDown_Time);

                iMouseDown_Count++;

                if (iMouseDown_Count >= iMouseDown_Count_Max)
                {
                    break;
                }
            }

            while (bMouseDown)
            {
                this.Invoke(new EventHandler(delegate { this._CustomButtonTop_MouseUp(customButtonTop, null); }));

                Thread.Sleep(iMouseDown_Time);
            }

            //

            iMouseDown_Count = 0;
            ThreadMouseDown = null;
        }

        //-----------------------------------------------------------------------
        // 功能说明：【TOP】，MOUSE UP，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _CustomButtonTop_MouseUp(object sender, MouseEventArgs e)
        {
            Boolean bOperateResult = false;
            VisionSystemClassLibrary.Struct.ROI rectangleROI_Temp = new VisionSystemClassLibrary.Struct.ROI();
            rectangleROI._CopyTo(ref rectangleROI_Temp);

            VisionSystemClassLibrary.Struct.ROI_Inner roi_Inner = rectangleROI_Temp.roiExtra;

            if (bSettingInnerROI) //设置内部区域
            {
                roi_Inner = rectangleROI_Temp.roiInner;
            }

            switch (roi_Inner.roiType)
            {
                case VisionSystemClassLibrary.Enum.ROIType.Quadrangle:
                    switch (contentalignmentHandleType)
                    {
                        case ContentAlignment.TopLeft://点击手柄上左按钮
                            roi_Inner.Point1.Y--;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, true)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                        case ContentAlignment.TopRight:
                            roi_Inner.Point2.Y--;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, true)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                        case ContentAlignment.BottomLeft:
                            roi_Inner.Point4.Y--;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, true)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                        case ContentAlignment.BottomRight:
                            roi_Inner.Point3.Y--;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, true)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                        default:
                            roi_Inner._Offset(0, -1);

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, false)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                    }
                    break;
                case VisionSystemClassLibrary.Enum.ROIType.Ellipse:
                    switch (contentalignmentHandleType)
                    {
                        case ContentAlignment.TopCenter:
                            roi_Inner.Point2.Y++;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, true)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                        case ContentAlignment.BottomCenter:
                            roi_Inner.Point2.Y--;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, true)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                        default:
                            roi_Inner.Point1.Y--;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, false)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                    }
                    break;
                case VisionSystemClassLibrary.Enum.ROIType.Rectangle:
                    switch (contentalignmentHandleType)
                    {
                        case ContentAlignment.TopCenter:
                            roi_Inner.Point1.Y--;
                            roi_Inner.Point2.Y++;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, true)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                        case ContentAlignment.BottomCenter:
                            roi_Inner.Point2.Y--;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, true)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                        default:
                            roi_Inner.Point1.Y--;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, false)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                    }
                    break;
                default:
                    break;
            }

            //

            if (bOperateResult)//有效
            {
                rectangleROI_Temp._CopyTo(ref rectangleROI);

                //事件

                if (null != Top_Click)//有效
                {
                    Top_Click(this, e);
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：【TOP】，MOUSE UP，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonTop_MouseUp(object sender, MouseEventArgs e)
        {
            bMouseDown = false;

            //

            _CustomButtonTop_MouseUp(sender, e);
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：【RIGHT TOP】，MOUSE DOWN，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonRightTop_MouseDown(object sender, MouseEventArgs e)
        {
            bMouseDown = true;

            if (null == ThreadMouseDown)//无效
            {
                ThreadMouseDown = new Thread(_threadRightTopMouseDown);//加载线程
                ThreadMouseDown.IsBackground = true;
                ThreadMouseDown.Start();//启动线程
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：【RIGHT TOP】，MOUSE DOWN，线程
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _threadRightTopMouseDown()
        {
            while (bMouseDown)
            {
                Thread.Sleep(iMouseDown_Time);

                iMouseDown_Count++;

                if (iMouseDown_Count >= iMouseDown_Count_Max)
                {
                    break;
                }
            }

            while (bMouseDown)
            {
                this.Invoke(new EventHandler(delegate { this._CustomButtonRightTop_MouseUp(customButtonRightTop, null); }));

                Thread.Sleep(iMouseDown_Time);
            }

            //

            iMouseDown_Count = 0;
            ThreadMouseDown = null;
        }

        //-----------------------------------------------------------------------
        // 功能说明：【RIGHT TOP】，MOUSE UP，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _CustomButtonRightTop_MouseUp(object sender, MouseEventArgs e)
        {
            Boolean bOperateResult = false;
            VisionSystemClassLibrary.Struct.ROI rectangleROI_Temp = new VisionSystemClassLibrary.Struct.ROI();
            rectangleROI._CopyTo(ref rectangleROI_Temp);

            VisionSystemClassLibrary.Struct.ROI_Inner roi_Inner = rectangleROI_Temp.roiExtra;

            if (bSettingInnerROI) //设置内部区域
            {
                roi_Inner = rectangleROI_Temp.roiInner;
            }

            switch (roi_Inner.roiType)
            {
                case VisionSystemClassLibrary.Enum.ROIType.Quadrangle:
                    switch (contentalignmentHandleType)
                    {
                        case ContentAlignment.TopLeft://点击手柄上左按钮
                            roi_Inner.Point1.X++;
                            roi_Inner.Point1.Y--;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, true)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                        case ContentAlignment.TopRight:
                            roi_Inner.Point2.X++;
                            roi_Inner.Point2.Y--;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, true)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                        case ContentAlignment.BottomLeft:
                            roi_Inner.Point4.X++;
                            roi_Inner.Point4.Y--;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, true)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                        case ContentAlignment.BottomRight:
                            roi_Inner.Point3.X++;
                            roi_Inner.Point3.Y--;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, true)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                        default:
                            roi_Inner._Offset(1, -1);

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, false)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                    }
                    break;
                case VisionSystemClassLibrary.Enum.ROIType.Ellipse:
                    roi_Inner.Point1.X++;
                    roi_Inner.Point1.Y--;

                    if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, false)) //满足条件
                    {
                        bOperateResult = true;
                    }
                    break;
                case VisionSystemClassLibrary.Enum.ROIType.Rectangle:
                    switch (contentalignmentHandleType)
                    {
                        case ContentAlignment.TopRight:
                            roi_Inner.Point1.Y--;
                            roi_Inner.Point2.X++;
                            roi_Inner.Point2.Y++;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, true)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                        case ContentAlignment.BottomLeft:
                            roi_Inner.Point1.X++;
                            roi_Inner.Point2.X--;
                            roi_Inner.Point2.Y--;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, true)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                        default:
                            roi_Inner.Point1.X++;
                            roi_Inner.Point1.Y--;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, false)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                    }
                    break;
                default:
                    break;
            }

            //

            if (bOperateResult)//有效
            {
                rectangleROI_Temp._CopyTo(ref rectangleROI);

                //事件

                if (null != RightTop_Click)//有效
                {
                    RightTop_Click(this, e);
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：【RIGHT TOP】，MOUSE UP，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonRightTop_MouseUp(object sender, MouseEventArgs e)
        {
            bMouseDown = false;

            //

            _CustomButtonRightTop_MouseUp(sender, e);
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：【LEFT】，MOUSE DOWN，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonLeft_MouseDown(object sender, MouseEventArgs e)
        {
            bMouseDown = true;

            if (null == ThreadMouseDown)//无效
            {
                ThreadMouseDown = new Thread(_threadLeftMouseDown);//加载线程
                ThreadMouseDown.IsBackground = true;
                ThreadMouseDown.Start();//启动线程
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：【LEFT】，MOUSE DOWN，线程
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _threadLeftMouseDown()
        {
            while (bMouseDown)
            {
                Thread.Sleep(iMouseDown_Time);

                iMouseDown_Count++;

                if (iMouseDown_Count >= iMouseDown_Count_Max)
                {
                    break;
                }
            }

            while (bMouseDown)
            {
                this.Invoke(new EventHandler(delegate { this._CustomButtonLeft_MouseUp(customButtonLeft, null); }));

                Thread.Sleep(iMouseDown_Time);
            }

            //

            iMouseDown_Count = 0;
            ThreadMouseDown = null;
        }

        //-----------------------------------------------------------------------
        // 功能说明：【LEFT】，MOUSE UP，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _CustomButtonLeft_MouseUp(object sender, MouseEventArgs e)
        {
            Boolean bOperateResult = false;
            VisionSystemClassLibrary.Struct.ROI rectangleROI_Temp = new VisionSystemClassLibrary.Struct.ROI();
            rectangleROI._CopyTo(ref rectangleROI_Temp);

            VisionSystemClassLibrary.Struct.ROI_Inner roi_Inner = rectangleROI_Temp.roiExtra;

            if (bSettingInnerROI) //设置内部区域
            {
                roi_Inner = rectangleROI_Temp.roiInner;
            }

            switch (roi_Inner.roiType)
            {
                case VisionSystemClassLibrary.Enum.ROIType.Quadrangle:
                    switch (contentalignmentHandleType)
                    {
                        case ContentAlignment.TopLeft://点击手柄上左按钮
                            roi_Inner.Point1.X--;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, true)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                        case ContentAlignment.TopRight:
                            roi_Inner.Point2.X--;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, true)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                        case ContentAlignment.BottomLeft:
                            roi_Inner.Point4.X--;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, true)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                        case ContentAlignment.BottomRight:
                            roi_Inner.Point3.X--;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, true)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                        default:
                            roi_Inner._Offset(-1, 0);

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, false)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                    }
                    break;
                case VisionSystemClassLibrary.Enum.ROIType.Ellipse:
                    switch (contentalignmentHandleType)
                    {
                        case ContentAlignment.MiddleLeft:
                            roi_Inner.Point2.X++;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, true)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                        case ContentAlignment.MiddleRight:
                            roi_Inner.Point2.X--;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, true)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                        default:
                            roi_Inner.Point1.X--;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, false)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                    }
                    break;
                case VisionSystemClassLibrary.Enum.ROIType.Rectangle:
                    switch (contentalignmentHandleType)
                    {
                        case ContentAlignment.MiddleLeft:
                            roi_Inner.Point1.X--;
                            roi_Inner.Point2.X++;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, true)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                        case ContentAlignment.MiddleRight:
                            roi_Inner.Point2.X--;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, true)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                        default:
                            roi_Inner.Point1.X--;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, false)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                    }
                    break;
                default:
                    break;
            }

            //

            if (bOperateResult)//有效
            {
                rectangleROI_Temp._CopyTo(ref rectangleROI);

                //事件

                if (null != Left_Click)//有效
                {
                    Left_Click(this, e);
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：【LEFT】，MOUSE UP，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonLeft_MouseUp(object sender, MouseEventArgs e)
        {
            bMouseDown = false;

            //

            _CustomButtonLeft_MouseUp(sender, e);
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：【RIGHT】，MOUSE DOWN，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonRight_MouseDown(object sender, MouseEventArgs e)
        {
            bMouseDown = true;

            if (null == ThreadMouseDown)//无效
            {
                ThreadMouseDown = new Thread(_threadRightMouseDown);//加载线程
                ThreadMouseDown.IsBackground = true;
                ThreadMouseDown.Start();//启动线程
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：【RIGHT】，MOUSE DOWN，线程
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _threadRightMouseDown()
        {
            while (bMouseDown)
            {
                Thread.Sleep(iMouseDown_Time);

                iMouseDown_Count++;

                if (iMouseDown_Count >= iMouseDown_Count_Max)
                {
                    break;
                }
            }

            while (bMouseDown)
            {
                this.Invoke(new EventHandler(delegate { this._CustomButtonRight_MouseUp(customButtonRight, null); }));

                Thread.Sleep(iMouseDown_Time);
            }

            //

            iMouseDown_Count = 0;
            ThreadMouseDown = null;
        }

        //-----------------------------------------------------------------------
        // 功能说明：【RIGHT】，MOUSE UP，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _CustomButtonRight_MouseUp(object sender, MouseEventArgs e)
        {
            Boolean bOperateResult = false;
            VisionSystemClassLibrary.Struct.ROI rectangleROI_Temp = new VisionSystemClassLibrary.Struct.ROI();
            rectangleROI._CopyTo(ref rectangleROI_Temp);

            VisionSystemClassLibrary.Struct.ROI_Inner roi_Inner = rectangleROI_Temp.roiExtra;

            if (bSettingInnerROI) //设置内部区域
            {
                roi_Inner = rectangleROI_Temp.roiInner;
            }

            switch (roi_Inner.roiType)
            {
                case VisionSystemClassLibrary.Enum.ROIType.Quadrangle:
                    switch (contentalignmentHandleType)
                    {
                        case ContentAlignment.TopLeft://点击手柄上左按钮
                            roi_Inner.Point1.X++;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, true)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                        case ContentAlignment.TopRight:
                            roi_Inner.Point2.X++;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, true)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                        case ContentAlignment.BottomLeft:
                            roi_Inner.Point4.X++;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, true)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                        case ContentAlignment.BottomRight:
                            roi_Inner.Point3.X++;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, true)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                        default:
                            roi_Inner._Offset(1, 0);

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, false)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                    }
                    break;
                case VisionSystemClassLibrary.Enum.ROIType.Ellipse:
                    switch (contentalignmentHandleType)
                    {
                        case ContentAlignment.MiddleLeft:
                            roi_Inner.Point2.X--;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, true)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                        case ContentAlignment.MiddleRight:
                            roi_Inner.Point2.X++;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, true)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                        default:
                            roi_Inner.Point1.X++;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, false)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                    }
                    break;
                case VisionSystemClassLibrary.Enum.ROIType.Rectangle:
                    switch (contentalignmentHandleType)
                    {
                        case ContentAlignment.MiddleLeft:
                            roi_Inner.Point1.X++;
                            roi_Inner.Point2.X--;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, true)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                        case ContentAlignment.MiddleRight:
                            roi_Inner.Point2.X++;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, true)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                        default:
                            roi_Inner.Point1.X++;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, false)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                    }
                    break;
                default:
                    break;
            }

            //

            if (bOperateResult)//有效
            {
                rectangleROI_Temp._CopyTo(ref rectangleROI);

                //事件

                if (null != Right_Click)//有效
                {
                    Right_Click(this, e);
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：【RIGHT】，MOUSE UP，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonRight_MouseUp(object sender, MouseEventArgs e)
        {
            bMouseDown = false;

            //

            _CustomButtonRight_MouseUp(sender, e);
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：【LEFT BOTTOM】，MOUSE DOWN，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonLeftBottom_MouseDown(object sender, MouseEventArgs e)
        {
            bMouseDown = true;

            if (null == ThreadMouseDown)//无效
            {
                ThreadMouseDown = new Thread(_threadLeftBottomMouseDown);//加载线程
                ThreadMouseDown.IsBackground = true;
                ThreadMouseDown.Start();//启动线程
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：【LEFT BOTTOM】，MOUSE DOWN，线程
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _threadLeftBottomMouseDown()
        {
            while (bMouseDown)
            {
                Thread.Sleep(iMouseDown_Time);

                iMouseDown_Count++;

                if (iMouseDown_Count >= iMouseDown_Count_Max)
                {
                    break;
                }
            }

            while (bMouseDown)
            {
                this.Invoke(new EventHandler(delegate { this._CustomButtonLeftBottom_MouseUp(customButtonLeftBottom, null); }));

                Thread.Sleep(iMouseDown_Time);
            }

            //

            iMouseDown_Count = 0;
            ThreadMouseDown = null;
        }

        //-----------------------------------------------------------------------
        // 功能说明：【LEFT BOTTOM】，MOUSE UP，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _CustomButtonLeftBottom_MouseUp(object sender, MouseEventArgs e)
        {
            Boolean bOperateResult = false;
            VisionSystemClassLibrary.Struct.ROI rectangleROI_Temp = new VisionSystemClassLibrary.Struct.ROI();
            rectangleROI._CopyTo(ref rectangleROI_Temp);

            VisionSystemClassLibrary.Struct.ROI_Inner roi_Inner = rectangleROI_Temp.roiExtra;

            if (bSettingInnerROI) //设置内部区域
            {
                roi_Inner = rectangleROI_Temp.roiInner;
            }

            switch (roi_Inner.roiType)
            {
                case VisionSystemClassLibrary.Enum.ROIType.Quadrangle:
                    switch (contentalignmentHandleType)
                    {
                        case ContentAlignment.TopLeft://点击手柄上左按钮
                            roi_Inner.Point1.X--;
                            roi_Inner.Point1.Y++;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, true)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                        case ContentAlignment.TopRight:
                            roi_Inner.Point2.X--;
                            roi_Inner.Point2.Y++;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, true)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                        case ContentAlignment.BottomLeft:
                            roi_Inner.Point4.X--;
                            roi_Inner.Point4.Y++;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, true)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                        case ContentAlignment.BottomRight:
                            roi_Inner.Point3.X--;
                            roi_Inner.Point3.Y++;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, true)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                        default:
                            roi_Inner._Offset(-1, 1);

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, false)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                    }
                    break;
                case VisionSystemClassLibrary.Enum.ROIType.Ellipse:
                    roi_Inner.Point1.X--;
                    roi_Inner.Point1.Y++;

                    if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, false)) //满足条件
                    {
                        bOperateResult = true;
                    }
                    break;
                case VisionSystemClassLibrary.Enum.ROIType.Rectangle:
                    switch (contentalignmentHandleType)
                    {
                        case ContentAlignment.BottomLeft:
                            roi_Inner.Point1.X--;
                            roi_Inner.Point1.Y++;
                            roi_Inner.Point2.X++;
                            roi_Inner.Point2.Y++;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, true)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                        case ContentAlignment.TopRight:
                            roi_Inner.Point1.Y++;
                            roi_Inner.Point2.X--;
                            roi_Inner.Point2.Y--;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, true)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                        default:
                            roi_Inner.Point1.X--;
                            roi_Inner.Point1.Y++;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, false)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                    }
                    break;
                default:
                    break;
            }

            //

            if (bOperateResult)//有效
            {
                rectangleROI_Temp._CopyTo(ref rectangleROI);

                //事件

                if (null != LeftBottom_Click)//有效
                {
                    LeftBottom_Click(this, e);
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：【LEFT BOTTOM】，MOUSE UP，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonLeftBottom_MouseUp(object sender, MouseEventArgs e)
        {
            bMouseDown = false;

            //

            _CustomButtonLeftBottom_MouseUp(sender, e);
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：【BOTTOM】，MOUSE DOWN，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonBottom_MouseDown(object sender, MouseEventArgs e)
        {
            bMouseDown = true;

            if (null == ThreadMouseDown)//无效
            {
                ThreadMouseDown = new Thread(_threadBottomMouseDown);//加载线程
                ThreadMouseDown.IsBackground = true;
                ThreadMouseDown.Start();//启动线程
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：【BOTTOM】，MOUSE DOWN，线程
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _threadBottomMouseDown()
        {
            while (bMouseDown)
            {
                Thread.Sleep(iMouseDown_Time);

                iMouseDown_Count++;

                if (iMouseDown_Count >= iMouseDown_Count_Max)
                {
                    break;
                }
            }

            while (bMouseDown)
            {
                this.Invoke(new EventHandler(delegate { this._CustomButtonBottom_MouseUp(customButtonBottom, null); }));

                Thread.Sleep(iMouseDown_Time);
            }

            //

            iMouseDown_Count = 0;
            ThreadMouseDown = null;
        }

        //-----------------------------------------------------------------------
        // 功能说明：【BOTTOM】，MOUSE UP，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _CustomButtonBottom_MouseUp(object sender, MouseEventArgs e)
        {
            Boolean bOperateResult = false;
            VisionSystemClassLibrary.Struct.ROI rectangleROI_Temp = new VisionSystemClassLibrary.Struct.ROI();
            rectangleROI._CopyTo(ref rectangleROI_Temp);

            VisionSystemClassLibrary.Struct.ROI_Inner roi_Inner = rectangleROI_Temp.roiExtra;

            if (bSettingInnerROI) //设置内部区域
            {
                roi_Inner = rectangleROI_Temp.roiInner;
            }

            switch (roi_Inner.roiType)
            {
                case VisionSystemClassLibrary.Enum.ROIType.Quadrangle:
                    switch (contentalignmentHandleType)
                    {
                        case ContentAlignment.TopLeft://点击手柄上左按钮
                            roi_Inner.Point1.Y++;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, true)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                        case ContentAlignment.TopRight:
                            roi_Inner.Point2.Y++;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, true)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                        case ContentAlignment.BottomLeft:
                            roi_Inner.Point4.Y++;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, true)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                        case ContentAlignment.BottomRight:
                            roi_Inner.Point3.Y++;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, true)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                        default:
                            roi_Inner._Offset(0, 1);

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, false)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                    }
                    break;
                case VisionSystemClassLibrary.Enum.ROIType.Ellipse:
                    switch (contentalignmentHandleType)
                    {
                        case ContentAlignment.TopCenter:
                            roi_Inner.Point2.Y--;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, true)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                        case ContentAlignment.BottomCenter:
                            roi_Inner.Point2.Y++;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, true)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                        default:
                            roi_Inner.Point1.Y++;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, false)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                    }
                    break;
                case VisionSystemClassLibrary.Enum.ROIType.Rectangle:
                    switch (contentalignmentHandleType)
                    {
                        case ContentAlignment.TopCenter:
                            roi_Inner.Point1.Y++;
                            roi_Inner.Point2.Y--;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, true)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                        case ContentAlignment.BottomCenter:
                            roi_Inner.Point2.Y++;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, true)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                        default:
                            roi_Inner.Point1.Y++;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, false)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                    }
                    break;
                default:
                    break;
            }

            //

            if (bOperateResult)//有效
            {
                rectangleROI_Temp._CopyTo(ref rectangleROI);

                //事件

                if (null != Bottom_Click)//有效
                {
                    Bottom_Click(this, e);
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：【BOTTOM】，MOUSE UP，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonBottom_MouseUp(object sender, MouseEventArgs e)
        {
            bMouseDown = false;

            //

            _CustomButtonBottom_MouseUp(sender, e);
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：【RIGHT BOTTOM】，MOUSE DOWN，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonRightBottom_MouseDown(object sender, MouseEventArgs e)
        {
            bMouseDown = true;

            if (null == ThreadMouseDown)//无效
            {
                ThreadMouseDown = new Thread(_threadRightBottomMouseDown);//加载线程
                ThreadMouseDown.IsBackground = true;
                ThreadMouseDown.Start();//启动线程
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：【RIGHT BOTTOM】，MOUSE DOWN，线程
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _threadRightBottomMouseDown()
        {
            while (bMouseDown)
            {
                Thread.Sleep(iMouseDown_Time);

                iMouseDown_Count++;

                if (iMouseDown_Count >= iMouseDown_Count_Max)
                {
                    break;
                }
            }

            while (bMouseDown)
            {
                this.Invoke(new EventHandler(delegate { this._CustomButtonRightBottom_MouseUp(customButtonRightBottom, null); }));

                Thread.Sleep(iMouseDown_Time);
            }

            //

            iMouseDown_Count = 0;
            ThreadMouseDown = null;
        }

        //-----------------------------------------------------------------------
        // 功能说明：【RIGHT BOTTOM】，MOUSE UP，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _CustomButtonRightBottom_MouseUp(object sender, MouseEventArgs e)
        {
            Boolean bOperateResult = false;
            VisionSystemClassLibrary.Struct.ROI rectangleROI_Temp = new VisionSystemClassLibrary.Struct.ROI();
            rectangleROI._CopyTo(ref rectangleROI_Temp);

            VisionSystemClassLibrary.Struct.ROI_Inner roi_Inner = rectangleROI_Temp.roiExtra;

            if (bSettingInnerROI) //设置内部区域
            {
                roi_Inner = rectangleROI_Temp.roiInner;
            }

            switch (roi_Inner.roiType)
            {
                case VisionSystemClassLibrary.Enum.ROIType.Quadrangle:
                    switch (contentalignmentHandleType)
                    {
                        case ContentAlignment.TopLeft://点击手柄上左按钮
                            roi_Inner.Point1.X++;
                            roi_Inner.Point1.Y++;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, true)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                        case ContentAlignment.TopRight:
                            roi_Inner.Point2.X++;
                            roi_Inner.Point2.Y++;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, true)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                        case ContentAlignment.BottomLeft:
                            roi_Inner.Point4.X++;
                            roi_Inner.Point4.Y++;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, true)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                        case ContentAlignment.BottomRight:
                            roi_Inner.Point3.X++;
                            roi_Inner.Point3.Y++;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, true)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                        default:
                            roi_Inner._Offset(1, 1);

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, false)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                    }
                    break;
                case VisionSystemClassLibrary.Enum.ROIType.Ellipse:
                    roi_Inner.Point1.X++;
                    roi_Inner.Point1.Y++;

                    if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, false)) //满足条件
                    {
                        bOperateResult = true;
                    }
                    break;
                case VisionSystemClassLibrary.Enum.ROIType.Rectangle:
                    switch (contentalignmentHandleType)
                    {
                        case ContentAlignment.TopLeft:
                            roi_Inner.Point1.X++;
                            roi_Inner.Point1.Y++;
                            roi_Inner.Point2.X--;
                            roi_Inner.Point2.Y--;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, true)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                        case ContentAlignment.BottomRight:
                            roi_Inner.Point2.X++;
                            roi_Inner.Point2.Y++;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, true)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                        default:
                            roi_Inner.Point1.X++;
                            roi_Inner.Point1.Y++;

                            if (QualityCheckControl._OperateIsOK(rectangleMaxROI, rectangleROI_Temp, bSettingInnerROI, false)) //满足条件
                            {
                                bOperateResult = true;
                            }
                            break;
                    }
                    break;
                default:
                    break;
            }

            //

            if (bOperateResult)//有效
            {
                rectangleROI_Temp._CopyTo(ref rectangleROI);

                //事件

                if (null != RightBottom_Click)//有效
                {
                    RightBottom_Click(this, e);
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：【RIGHT BOTTOM】，MOUSE UP，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonRightBottom_MouseUp(object sender, MouseEventArgs e)
        {
            bMouseDown = false;

            //

            _CustomButtonRightBottom_MouseUp(sender, e);
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：点击【移动】按钮事件，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonMove_MouseDown(object sender, MouseEventArgs e)
        {
            CustomEventArgs customeventargs = new CustomEventArgs();

            customeventargs.IntValue[0] = e.X + customButtonMove.Location.X;
            customeventargs.IntValue[1] = e.Y + customButtonMove.Location.Y;

            if (null != Center_Click)//有效
            {
                Center_Click(this, customeventargs);
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：鼠标指针在【移动】按钮上移动时的事件，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonMove_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (null != Center_MouseMove)//有效
                {
                    Center_MouseMove(this, e);
                }
            }
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：点击【HOME】按钮事件，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonHome_CustomButton_Click(object sender, EventArgs e)
        {
            if (null != Home_Click)//有效
            {
                Home_Click(this, e);
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：点击【CLOSE】按钮事件，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonClose_CustomButton_Click(object sender, EventArgs e)
        {
            this.Visible = false;

            //

            if (null != Close_Click)//有效
            {
                Close_Click(this, e);
            }
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：点击【LEFT TOP】按钮事件，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonLeftTop_1_CustomButton_Click(object sender, EventArgs e)
        {
            if (RegionSelectionType.LeftTop == regionSelectionType)//【LEFT TOP】
            {
                regionSelectionType = RegionSelectionType.None;
                contentalignmentHandleType = ContentAlignment.MiddleCenter; //add by liumin
            }
            else//其它
            {
                regionSelectionType = RegionSelectionType.LeftTop;
                contentalignmentHandleType = ContentAlignment.TopLeft; //add by liumin
            }

            _SetSelectionButton();

            //事件

            if (null != RegionSelection_Click)//有效
            {
                RegionSelection_Click(this, e);
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：点击【TOP】按钮事件，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonTop_1_CustomButton_Click(object sender, EventArgs e)
        {
            if (RegionSelectionType.Top == regionSelectionType)//【TOP】
            {
                regionSelectionType = RegionSelectionType.None;
                contentalignmentHandleType = ContentAlignment.MiddleCenter; //add by liumin
            }
            else//其它
            {
                regionSelectionType = RegionSelectionType.Top;
                contentalignmentHandleType = ContentAlignment.TopCenter; //add by liumin
            }

            _SetSelectionButton();

            //事件

            if (null != RegionSelection_Click)//有效
            {
                RegionSelection_Click(this, e);
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：点击【RIGHT TOP】按钮事件，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonRightTop_1_CustomButton_Click(object sender, EventArgs e)
        {
            if (RegionSelectionType.RightTop == regionSelectionType)//【RIGHT TOP】
            {
                regionSelectionType = RegionSelectionType.None;
                contentalignmentHandleType = ContentAlignment.MiddleCenter; //add by liumin
            }
            else//其它
            {
                regionSelectionType = RegionSelectionType.RightTop;
                contentalignmentHandleType = ContentAlignment.TopRight; //add by liumin
            }

            _SetSelectionButton();

            //事件

            if (null != RegionSelection_Click)//有效
            {
                RegionSelection_Click(this, e);
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：点击【LEFT】按钮事件，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonLeft_1_CustomButton_Click(object sender, EventArgs e)
        {
            if (RegionSelectionType.Left == regionSelectionType)//【LEFT】
            {
                regionSelectionType = RegionSelectionType.None;
                contentalignmentHandleType = ContentAlignment.MiddleCenter; //add by liumin
            }
            else//其它
            {
                regionSelectionType = RegionSelectionType.Left;
                contentalignmentHandleType = ContentAlignment.MiddleLeft; //add by liumin
            }

            _SetSelectionButton();

            //事件

            if (null != RegionSelection_Click)//有效
            {
                RegionSelection_Click(this, e);
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：点击【RIGHT】按钮事件，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonRight_1_CustomButton_Click(object sender, EventArgs e)
        {
            if (RegionSelectionType.Right == regionSelectionType)//【RIGHT】
            {
                regionSelectionType = RegionSelectionType.None;
                contentalignmentHandleType = ContentAlignment.MiddleCenter; //add by liumin
            }
            else//其它
            {
                regionSelectionType = RegionSelectionType.Right;
                contentalignmentHandleType = ContentAlignment.MiddleRight; //add by liumin
            }

            _SetSelectionButton();

            //事件

            if (null != RegionSelection_Click)//有效
            {
                RegionSelection_Click(this, e);
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：点击【LEFT BOTTOM】按钮事件，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonLeftBottom_1_CustomButton_Click(object sender, EventArgs e)
        {
            if (RegionSelectionType.LeftBottom == regionSelectionType)//【LEFT BOTTOM】
            {
                regionSelectionType = RegionSelectionType.None;
                contentalignmentHandleType = ContentAlignment.MiddleCenter; //add by liumin
            }
            else//其它
            {
                regionSelectionType = RegionSelectionType.LeftBottom;
                contentalignmentHandleType = ContentAlignment.BottomLeft; //add by liumin
            }

            _SetSelectionButton();

            //事件

            if (null != RegionSelection_Click)//有效
            {
                RegionSelection_Click(this, e);
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：点击【BOTTOM】按钮事件，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonBottom_1_CustomButton_Click(object sender, EventArgs e)
        {
            if (RegionSelectionType.Bottom == regionSelectionType)//【BOTTOM】
            {
                regionSelectionType = RegionSelectionType.None;
                contentalignmentHandleType = ContentAlignment.MiddleCenter; //add by liumin
            }
            else//其它
            {
                regionSelectionType = RegionSelectionType.Bottom;
                contentalignmentHandleType = ContentAlignment.BottomCenter; //add by liumin
            }

            _SetSelectionButton();

            //事件

            if (null != RegionSelection_Click)//有效
            {
                RegionSelection_Click(this, e);
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：点击【RIGHT BOTTOM】按钮事件，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonRightBottom_1_CustomButton_Click(object sender, EventArgs e)
        {
            if (RegionSelectionType.RightBottom == regionSelectionType)//【RIGHT BOTTOM】
            {
                regionSelectionType = RegionSelectionType.None;
                contentalignmentHandleType = ContentAlignment.MiddleCenter; //add by liumin
            }
            else//其它
            {
                regionSelectionType = RegionSelectionType.RightBottom;
                contentalignmentHandleType = ContentAlignment.BottomRight; //add by liumin
            }

            _SetSelectionButton();

            //事件

            if (null != RegionSelection_Click)//有效
            {
                RegionSelection_Click(this, e);
            }
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：点击【SHOW】/【HIDE】按钮事件，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonShowHide_CustomButton_Click(object sender, EventArgs e)
        {
            if (0 == customButtonShowHide.CurrentTextGroupIndex)//【HIDE】
            {
                customButtonShowHide.CurrentTextGroupIndex = 1;
            }
            else//【SHOW】
            {
                customButtonShowHide.CurrentTextGroupIndex = 0;
            }

            //事件

            if (null != ShowHide_Click)//有效
            {
                ShowHide_Click(this, e);
            }
        }

        /// <summary>
        /// 刷新控制面板显示
        /// </summary>
        /// <param name="bSettingInnerROI"></param>
        private void _UpdateSizeAdjustedPanel(Boolean bSettingInnerROI)
        {
            VisionSystemClassLibrary.Struct.ROI_Inner roi_Inner = rectangleROI.roiExtra;

            if (bSettingInnerROI) //设置内部区域
            {
                roi_Inner = rectangleROI.roiInner;
            }

            if (roi_Inner.roiType == VisionSystemClassLibrary.Enum.ROIType.Quadrangle) //区域类型为四边形
            {
                switch (contentalignmentHandleType)
                {
                    case ContentAlignment.MiddleCenter:

                        bMoveButtonEnabled[0] = false;
                        _SetButtonEnabled(customButtonLeft, bMoveButtonEnabled[0]);//【LEFT】
                        bMoveButtonEnabled[1] = false;
                        _SetButtonEnabled(customButtonLeftTop, bMoveButtonEnabled[1]);//【LEFT TOP】
                        bMoveButtonEnabled[2] = false;
                        _SetButtonEnabled(customButtonTop, bMoveButtonEnabled[2]);//【TOP】
                        bMoveButtonEnabled[3] = false;
                        _SetButtonEnabled(customButtonRightTop, bMoveButtonEnabled[3]);//【RIGHT TOP】
                        bMoveButtonEnabled[4] = false;
                        _SetButtonEnabled(customButtonRight, bMoveButtonEnabled[4]);//【RIGHT】
                        bMoveButtonEnabled[5] = false;
                        _SetButtonEnabled(customButtonLeftBottom, bMoveButtonEnabled[5]);//【LEFT BOTTOM】
                        bMoveButtonEnabled[6] = false;
                        _SetButtonEnabled(customButtonBottom, bMoveButtonEnabled[6]);//【BOTTOM】
                        bMoveButtonEnabled[7] = false;
                        _SetButtonEnabled(customButtonRightBottom, bMoveButtonEnabled[7]);//【RIGHT BOTTOM】

                        break;
                    default:

                        bMoveButtonEnabled[0] = true;
                        _SetButtonEnabled(customButtonLeft, bMoveButtonEnabled[0]);//【LEFT】
                        bMoveButtonEnabled[1] = true;
                        _SetButtonEnabled(customButtonLeftTop, bMoveButtonEnabled[1]);//【LEFT TOP】
                        bMoveButtonEnabled[2] = true;
                        _SetButtonEnabled(customButtonTop, bMoveButtonEnabled[2]);//【TOP】
                        bMoveButtonEnabled[3] = true;
                        _SetButtonEnabled(customButtonRightTop, bMoveButtonEnabled[3]);//【RIGHT TOP】
                        bMoveButtonEnabled[4] = true;
                        _SetButtonEnabled(customButtonRight, bMoveButtonEnabled[4]);//【RIGHT】
                        bMoveButtonEnabled[5] = true;
                        _SetButtonEnabled(customButtonLeftBottom, bMoveButtonEnabled[5]);//【LEFT BOTTOM】
                        bMoveButtonEnabled[6] = true;
                        _SetButtonEnabled(customButtonBottom, bMoveButtonEnabled[6]);//【BOTTOM】
                        bMoveButtonEnabled[7] = true;
                        _SetButtonEnabled(customButtonRightBottom, bMoveButtonEnabled[7]);//【RIGHT BOTTOM】

                        break;
                }
            }
            else
            {
                switch (contentalignmentHandleType)
                {
                    case ContentAlignment.MiddleCenter:

                        bMoveButtonEnabled[0] = false;
                        _SetButtonEnabled(customButtonLeft, bMoveButtonEnabled[0]);//【LEFT】
                        bMoveButtonEnabled[1] = false;
                        _SetButtonEnabled(customButtonLeftTop, bMoveButtonEnabled[1]);//【LEFT TOP】
                        bMoveButtonEnabled[2] = false;
                        _SetButtonEnabled(customButtonTop, bMoveButtonEnabled[2]);//【TOP】
                        bMoveButtonEnabled[3] = false;
                        _SetButtonEnabled(customButtonRightTop, bMoveButtonEnabled[3]);//【RIGHT TOP】
                        bMoveButtonEnabled[4] = false;
                        _SetButtonEnabled(customButtonRight, bMoveButtonEnabled[4]);//【RIGHT】
                        bMoveButtonEnabled[5] = false;
                        _SetButtonEnabled(customButtonLeftBottom, bMoveButtonEnabled[5]);//【LEFT BOTTOM】
                        bMoveButtonEnabled[6] = false;
                        _SetButtonEnabled(customButtonBottom, bMoveButtonEnabled[6]);//【BOTTOM】
                        bMoveButtonEnabled[7] = false;
                        _SetButtonEnabled(customButtonRightBottom, bMoveButtonEnabled[7]);//【RIGHT BOTTOM】

                        break;
                    case ContentAlignment.MiddleLeft:

                        bMoveButtonEnabled[0] = true;
                        _SetButtonEnabled(customButtonLeft, bMoveButtonEnabled[0]);//【LEFT】
                        bMoveButtonEnabled[1] = false;
                        _SetButtonEnabled(customButtonLeftTop, bMoveButtonEnabled[1]);//【LEFT TOP】
                        bMoveButtonEnabled[2] = false;
                        _SetButtonEnabled(customButtonTop, bMoveButtonEnabled[2]);//【TOP】
                        bMoveButtonEnabled[3] = false;
                        _SetButtonEnabled(customButtonRightTop, bMoveButtonEnabled[3]);//【RIGHT TOP】
                        bMoveButtonEnabled[4] = true;
                        _SetButtonEnabled(customButtonRight, bMoveButtonEnabled[4]);//【RIGHT】
                        bMoveButtonEnabled[5] = false;
                        _SetButtonEnabled(customButtonLeftBottom, bMoveButtonEnabled[5]);//【LEFT BOTTOM】
                        bMoveButtonEnabled[6] = false;
                        _SetButtonEnabled(customButtonBottom, bMoveButtonEnabled[6]);//【BOTTOM】
                        bMoveButtonEnabled[7] = false;
                        _SetButtonEnabled(customButtonRightBottom, bMoveButtonEnabled[7]);//【RIGHT BOTTOM】

                        break;
                    case ContentAlignment.TopLeft:

                        bMoveButtonEnabled[0] = false;
                        _SetButtonEnabled(customButtonLeft, bMoveButtonEnabled[0]);//【LEFT】
                        bMoveButtonEnabled[1] = true;
                        _SetButtonEnabled(customButtonLeftTop, bMoveButtonEnabled[1]);//【LEFT TOP】
                        bMoveButtonEnabled[2] = false;
                        _SetButtonEnabled(customButtonTop, bMoveButtonEnabled[2]);//【TOP】
                        bMoveButtonEnabled[3] = false;
                        _SetButtonEnabled(customButtonRightTop, bMoveButtonEnabled[3]);//【RIGHT TOP】
                        bMoveButtonEnabled[4] = false;
                        _SetButtonEnabled(customButtonRight, bMoveButtonEnabled[4]);//【RIGHT】
                        bMoveButtonEnabled[5] = false;
                        _SetButtonEnabled(customButtonLeftBottom, bMoveButtonEnabled[5]);//【LEFT BOTTOM】
                        bMoveButtonEnabled[6] = false;
                        _SetButtonEnabled(customButtonBottom, bMoveButtonEnabled[6]);//【BOTTOM】
                        bMoveButtonEnabled[7] = true;
                        _SetButtonEnabled(customButtonRightBottom, bMoveButtonEnabled[7]);//【RIGHT BOTTOM】

                        break;
                    case ContentAlignment.TopCenter:

                        bMoveButtonEnabled[0] = false;
                        _SetButtonEnabled(customButtonLeft, bMoveButtonEnabled[0]);//【LEFT】
                        bMoveButtonEnabled[1] = false;
                        _SetButtonEnabled(customButtonLeftTop, bMoveButtonEnabled[1]);//【LEFT TOP】
                        bMoveButtonEnabled[2] = true;
                        _SetButtonEnabled(customButtonTop, bMoveButtonEnabled[2]);//【TOP】
                        bMoveButtonEnabled[3] = false;
                        _SetButtonEnabled(customButtonRightTop, bMoveButtonEnabled[3]);//【RIGHT TOP】
                        bMoveButtonEnabled[4] = false;
                        _SetButtonEnabled(customButtonRight, bMoveButtonEnabled[4]);//【RIGHT】
                        bMoveButtonEnabled[5] = false;
                        _SetButtonEnabled(customButtonLeftBottom, bMoveButtonEnabled[5]);//【LEFT BOTTOM】
                        bMoveButtonEnabled[6] = true;
                        _SetButtonEnabled(customButtonBottom, bMoveButtonEnabled[6]);//【BOTTOM】
                        bMoveButtonEnabled[7] = false;
                        _SetButtonEnabled(customButtonRightBottom, bMoveButtonEnabled[7]);//【RIGHT BOTTOM】

                        break;
                    case ContentAlignment.TopRight:

                        bMoveButtonEnabled[0] = false;
                        _SetButtonEnabled(customButtonLeft, bMoveButtonEnabled[0]);//【LEFT】
                        bMoveButtonEnabled[1] = false;
                        _SetButtonEnabled(customButtonLeftTop, bMoveButtonEnabled[1]);//【LEFT TOP】
                        bMoveButtonEnabled[2] = false;
                        _SetButtonEnabled(customButtonTop, bMoveButtonEnabled[2]);//【TOP】
                        bMoveButtonEnabled[3] = true;
                        _SetButtonEnabled(customButtonRightTop, bMoveButtonEnabled[3]);//【RIGHT TOP】
                        bMoveButtonEnabled[4] = false;
                        _SetButtonEnabled(customButtonRight, bMoveButtonEnabled[4]);//【RIGHT】
                        bMoveButtonEnabled[5] = true;
                        _SetButtonEnabled(customButtonLeftBottom, bMoveButtonEnabled[5]);//【LEFT BOTTOM】
                        bMoveButtonEnabled[6] = false;
                        _SetButtonEnabled(customButtonBottom, bMoveButtonEnabled[6]);//【BOTTOM】
                        bMoveButtonEnabled[7] = false;
                        _SetButtonEnabled(customButtonRightBottom, bMoveButtonEnabled[7]);//【RIGHT BOTTOM】

                        break;
                    case ContentAlignment.MiddleRight:

                        bMoveButtonEnabled[0] = true;
                        _SetButtonEnabled(customButtonLeft, bMoveButtonEnabled[0]);//【LEFT】
                        bMoveButtonEnabled[1] = false;
                        _SetButtonEnabled(customButtonLeftTop, bMoveButtonEnabled[1]);//【LEFT TOP】
                        bMoveButtonEnabled[2] = false;
                        _SetButtonEnabled(customButtonTop, bMoveButtonEnabled[2]);//【TOP】
                        bMoveButtonEnabled[3] = false;
                        _SetButtonEnabled(customButtonRightTop, bMoveButtonEnabled[3]);//【RIGHT TOP】
                        bMoveButtonEnabled[4] = true;
                        _SetButtonEnabled(customButtonRight, bMoveButtonEnabled[4]);//【RIGHT】
                        bMoveButtonEnabled[5] = false;
                        _SetButtonEnabled(customButtonLeftBottom, bMoveButtonEnabled[5]);//【LEFT BOTTOM】
                        bMoveButtonEnabled[6] = false;
                        _SetButtonEnabled(customButtonBottom, bMoveButtonEnabled[6]);//【BOTTOM】
                        bMoveButtonEnabled[7] = false;
                        _SetButtonEnabled(customButtonRightBottom, bMoveButtonEnabled[7]);//【RIGHT BOTTOM】

                        break;
                    case ContentAlignment.BottomLeft:

                        bMoveButtonEnabled[0] = false;
                        _SetButtonEnabled(customButtonLeft, bMoveButtonEnabled[0]);//【LEFT】
                        bMoveButtonEnabled[1] = false;
                        _SetButtonEnabled(customButtonLeftTop, bMoveButtonEnabled[1]);//【LEFT TOP】
                        bMoveButtonEnabled[2] = false;
                        _SetButtonEnabled(customButtonTop, bMoveButtonEnabled[2]);//【TOP】
                        bMoveButtonEnabled[3] = true;
                        _SetButtonEnabled(customButtonRightTop, bMoveButtonEnabled[3]);//【RIGHT TOP】
                        bMoveButtonEnabled[4] = false;
                        _SetButtonEnabled(customButtonRight, bMoveButtonEnabled[4]);//【RIGHT】
                        bMoveButtonEnabled[5] = true;
                        _SetButtonEnabled(customButtonLeftBottom, bMoveButtonEnabled[5]);//【LEFT BOTTOM】
                        bMoveButtonEnabled[6] = false;
                        _SetButtonEnabled(customButtonBottom, bMoveButtonEnabled[6]);//【BOTTOM】
                        bMoveButtonEnabled[7] = false;
                        _SetButtonEnabled(customButtonRightBottom, bMoveButtonEnabled[7]);//【RIGHT BOTTOM】

                        break;
                    case ContentAlignment.BottomCenter:

                        bMoveButtonEnabled[0] = false;
                        _SetButtonEnabled(customButtonLeft, bMoveButtonEnabled[0]);//【LEFT】
                        bMoveButtonEnabled[1] = false;
                        _SetButtonEnabled(customButtonLeftTop, bMoveButtonEnabled[1]);//【LEFT TOP】
                        bMoveButtonEnabled[2] = true;
                        _SetButtonEnabled(customButtonTop, bMoveButtonEnabled[2]);//【TOP】
                        bMoveButtonEnabled[3] = false;
                        _SetButtonEnabled(customButtonRightTop, bMoveButtonEnabled[3]);//【RIGHT TOP】
                        bMoveButtonEnabled[4] = false;
                        _SetButtonEnabled(customButtonRight, bMoveButtonEnabled[4]);//【RIGHT】
                        bMoveButtonEnabled[5] = false;
                        _SetButtonEnabled(customButtonLeftBottom, bMoveButtonEnabled[5]);//【LEFT BOTTOM】
                        bMoveButtonEnabled[6] = true;
                        _SetButtonEnabled(customButtonBottom, bMoveButtonEnabled[6]);//【BOTTOM】
                        bMoveButtonEnabled[7] = false;
                        _SetButtonEnabled(customButtonRightBottom, bMoveButtonEnabled[7]);//【RIGHT BOTTOM】

                        break;
                    case ContentAlignment.BottomRight:

                        bMoveButtonEnabled[0] = false;
                        _SetButtonEnabled(customButtonLeft, bMoveButtonEnabled[0]);//【LEFT】
                        bMoveButtonEnabled[1] = true;
                        _SetButtonEnabled(customButtonLeftTop, bMoveButtonEnabled[1]);//【LEFT TOP】
                        bMoveButtonEnabled[2] = false;
                        _SetButtonEnabled(customButtonTop, bMoveButtonEnabled[2]);//【TOP】
                        bMoveButtonEnabled[3] = false;
                        _SetButtonEnabled(customButtonRightTop, bMoveButtonEnabled[3]);//【RIGHT TOP】
                        bMoveButtonEnabled[4] = false;
                        _SetButtonEnabled(customButtonRight, bMoveButtonEnabled[4]);//【RIGHT】
                        bMoveButtonEnabled[5] = false;
                        _SetButtonEnabled(customButtonLeftBottom, bMoveButtonEnabled[5]);//【LEFT BOTTOM】
                        bMoveButtonEnabled[6] = false;
                        _SetButtonEnabled(customButtonBottom, bMoveButtonEnabled[6]);//【BOTTOM】
                        bMoveButtonEnabled[7] = true;
                        _SetButtonEnabled(customButtonRightBottom, bMoveButtonEnabled[7]);//【RIGHT BOTTOM】

                        break;
                    default:
                        break;
                }
            }

            switch (roi_Inner.roiType)
            {
                case VisionSystemClassLibrary.Enum.ROIType.Quadrangle:
                    customButtonLeftTop_1.Visible = true;
                    customButtonRightTop_1.Visible = true;
                    customButtonLeftBottom_1.Visible = true;
                    customButtonRightBottom_1.Visible = true;
                    customButtonLeft_1.Visible = false;
                    customButtonRight_1.Visible = false;
                    customButtonTop_1.Visible = false;
                    customButtonBottom_1.Visible = false;
                    break;
                case VisionSystemClassLibrary.Enum.ROIType.Ellipse:
                    customButtonLeftTop_1.Visible = false;
                    customButtonRightTop_1.Visible = false;
                    customButtonLeftBottom_1.Visible = false;
                    customButtonRightBottom_1.Visible = false;
                    customButtonLeft_1.Visible = true;
                    customButtonRight_1.Visible = true;
                    customButtonTop_1.Visible = true;
                    customButtonBottom_1.Visible = true;
                    break;
                case VisionSystemClassLibrary.Enum.ROIType.Rectangle:
                    customButtonLeftTop_1.Visible = true;
                    customButtonRightTop_1.Visible = true;
                    customButtonLeftBottom_1.Visible = true;
                    customButtonRightBottom_1.Visible = true;
                    customButtonLeft_1.Visible = true;
                    customButtonRight_1.Visible = true;
                    customButtonTop_1.Visible = true;
                    customButtonBottom_1.Visible = true;
                    break;
                default: break;
            }
        }
    }
}