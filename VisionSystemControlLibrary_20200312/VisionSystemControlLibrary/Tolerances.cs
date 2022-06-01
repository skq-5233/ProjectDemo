/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：Tolerances.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：TOLERANCES SETTINGS，曲线图控件

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
using System.Drawing.Drawing2D;

namespace VisionSystemControlLibrary
{
    public partial class Tolerances : UserControl
    {
        //该控件为Tolerances页面中的曲线图控件

        //【运行/停止】按钮为运行状态时，【学习】按钮（若显示）一定为使能状态，控件一定为使能状态（此时的曲线图有两种状态：若当前值存在，则曲线图、标题文本为使能状态；若当前值不存在，则X坐标及X、Y坐标轴为使能状态，Y坐标及曲线、标题文本为禁止状态）
        //【运行/停止】按钮为停止状态时，【学习】按钮（若显示）一定为禁止状态，控件一定为禁止状态

        private Boolean bEnabled = true;//属性，控件使能状态。取值范围：true，使能；false：禁止

        private Int32 iGraphDataIndex = 0;//属性，控件对应的曲线图数据索引（从0开始）

        public Tolerances_Graph Control_Graph;//控件曲线图

        public Graphics Control_Graphics;//控件曲线图

        //

        private Boolean bCurveMode = false;//属性，是否为曲线图模式。取值范围：true，是；false：否

        //

        private Image imageToDraw = null;//控件图像

        private Image imageControl = null;//属性（只读），控件图像

        //

        [Browsable(true), Description("点击曲线图控件时产生的事件"), Category("Tolerances 事件")]
        public event EventHandler Control_Click;//点击曲线图控件时产生的事件

        [Browsable(true), Description("双击曲线图控件时产生的事件"), Category("Tolerances 事件")]
        public event EventHandler Control_DoubleClick;//双击曲线图控件时产生的事件
        //CustomEventArgs参数含义：
        //1.IntValue[0]：取值为0，表示该控件在双击时已经被选中；取值为1，表示该控件在双击时未被选中

        [Browsable(true), Description("点击【运行/停止】按钮时产生的事件"), Category("Tolerances 事件")]
        public event EventHandler RunStop_Click;//点击【运行/停止】按钮时产生的事件

        [Browsable(true), Description("点击【学习】按钮时产生的事件"), Category("Tolerances 事件")]
        public event EventHandler Learning_Click;//点击【学习】按钮时产生的事件

        //构造函数

        //----------------------------------------------------------------------
        // 功能说明：构造函数，初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Tolerances()
        {
            InitializeComponent();

            //

            Control_Graph = new Tolerances_Graph(Control_Graphics = CreateGraphics());

            //

            _GetBackgroundImage();
            _Draw();
        }

        //属性

        //-----------------------------------------------------------------------
        // 功能说明：ImageControl属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(false), Description("控件图像"), Category("Tolerances 通用")]
        public Image ImageControl
        {
            get//读取
            {
                return imageControl;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：SizeControl属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("控件大小"), Category("Tolerances 通用")]
        public Size SizeControl//属性
        {
            get//读取
            {
                return this.Size;
            }
            set//设置
            {
                this.Size = value;

                //

                Control_Graphics.Dispose();//释放

                Control_Graphics = CreateGraphics();//获取绘图资源

                Control_Graph.Graph_Graphics = Control_Graphics;

                Control_Graph._SetGraphics(Control_Graphics);

                //

                _GetBackgroundImage();
                _Draw();
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：CurveMode属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("是否为曲线图模式。取值范围：true，是；false：否"), Category("Tolerances 通用")]
        public Boolean CurveMode//属性
        {
            get//读取
            {
                return bCurveMode;
            }
            set//设置
            {
                if (value != bCurveMode)//设置了新的数值
                {
                    bCurveMode = value;

                    Control_Graph.CurveMode = bCurveMode;

                    //

                    if (bCurveMode)//曲线图模式
                    {
                        customButtonRunStop.Visible = false;
                        customButtonLearning.Visible = false;
                    }
                    else//非曲线图模式
                    {
                        customButtonRunStop.Visible = true;

                        if (Control_Graph.Control_Data.ButtonLearningShow)//显示
                        {
                            customButtonLearning.Visible = true;
                        }
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：LocationRunStop属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("【运行/停止】按钮位置"), Category("Tolerances 通用")]
        public Point LocationRunStop//属性
        {
            get//读取
            {
                return customButtonRunStop.Location;
            }
            set//设置
            {
                if (value != customButtonRunStop.Location)//设置了新的数值
                {
                    customButtonRunStop.Location = value;

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：LocationLearning属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("【学习】按钮位置"), Category("Tolerances 通用")]
        public Point LocationLearning//属性
        {
            get//读取
            {
                return customButtonLearning.Location;
            }
            set//设置
            {
                if (value != customButtonLearning.Location)//设置了新的数值
                {
                    customButtonLearning.Location = value;

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：TolerancesEnabled属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("控件使能状态。取值范围：true，使能；false：禁止"), Category("Tolerances 通用")]
        public bool TolerancesEnabled//属性
        {
            get//读取
            {
                return bEnabled;
            }
            set//设置
            {
                if (value != bEnabled)//设置了新的数值
                {
                    bEnabled = value;

                    //

                    if (bEnabled)//使能
                    {
                        if (Control_Graph.Control_Data.RunorStop)//运行
                        {
                            customButtonRunStop.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
                        }
                        else//停止
                        {
                            customButtonRunStop.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                        }

                        if (Control_Graph.Control_Data.ButtonLearningShow)//【学习】按钮为显示状态
                        {
                            customButtonLearning.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                        }
                    } 
                    else//禁止
                    {
                        customButtonRunStop.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;

                        if (Control_Graph.Control_Data.ButtonLearningShow)//【学习】按钮为显示状态
                        {
                            customButtonLearning.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                        }
                    }

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：GraphDataIndex属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("控件对应的曲线图数据索引（从0开始）"), Category("Tolerances 通用")]
        public int GraphDataIndex//属性
        {
            get//读取
            {
                return iGraphDataIndex;
            }
            set//设置
            {
                if (value != iGraphDataIndex)//设置了新的数值
                {
                    iGraphDataIndex = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：RunorStop属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("【运行/停止】按钮（或控件）的运行或停止状态。true：运行；false：停止"), Category("Tolerances 通用")]
        public bool RunorStop//属性
        {
            get//读取
            {
                return Control_Graph.Control_Data.RunorStop;
            }
            set//设置
            {
                if (value != Control_Graph.Control_Data.RunorStop)//设置了新的数值
                {
                    Control_Graph.Control_Data.RunorStop = value;

                    if (Control_Graph.Control_Data.RunorStop)//运行
                    {
                        if (Control_Graph.Control_Data.ControlSelected)//控件处于选中状态
                        {
                            //此时控件不会处于选中状态，不执行操作
                        }
                        else//控件未选中
                        {
                            customButtonRunStop.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;

                            //

                            if (Control_Graph.Control_Data.ButtonLearningShow)//【学习】按钮为显示状态
                            {
                                customButtonLearning.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                            }
                            else//学习按钮为隐藏状态
                            {
                                //不执行操作
                            }
                        }
                    }
                    else//停止
                    {
                        if (Control_Graph.Control_Data.ControlSelected)//控件处于选中状态
                        {
                            Control_Graph.Control_Data.ControlSelected = false;//控件置为未选中状态
                        }
                        else//控件未选中
                        {
                            //不执行操作
                        }

                        //

                        customButtonRunStop.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;

                        //

                        if (Control_Graph.Control_Data.ButtonLearningShow)//【学习】按钮为显示状态
                        {
                            customButtonLearning.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                        }
                        else//学习按钮为隐藏状态
                        {
                            //不执行操作
                        }
                    }

                    Control_Graph.Control_Data.TolerancesTools.ToolsValue = !(Control_Graph.Control_Data.TolerancesTools.ToolsValue);//更新数值

                    //

                    Control_Graph._SetControlBackgroundColor();
                    Control_Graph._SetAxisColor();

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ControlSelected属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("控件是否被（双击）选中。仅在控件为使能状态时，控件才能被选中。true：是；false：否"), Category("Tolerances 通用")]
        public bool ControlSelected//属性
        {
            get//读取
            {
                return Control_Graph.Control_Data.ControlSelected;
            }
            set//设置
            {
                if (value != Control_Graph.Control_Data.ControlSelected)//设置了新的数值
                {
                    Control_Graph.Control_Data.ControlSelected = value;

                    if (Control_Graph.Control_Data.RunorStop)//运行（即当前控件处于使能状态）
                    {
                        if (Control_Graph.Control_Data.ControlSelected)//当前控件处于选中状态
                        {
                            //此时【运行/停止】按钮（或控件）一定处于运行状态

                            customButtonRunStop.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;

                            //

                            if (Control_Graph.Control_Data.ButtonLearningShow)//【学习】按钮处于显示状态
                            {
                                //此时【学习】按钮一定处于使能状态

                                customButtonLearning.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                            }
                            else//【学习】按钮处于隐藏状态
                            {
                                //不执行操作
                            }
                        }
                        else//当前控件处于未选中状态
                        {
                            //此时【运行/停止】按钮（或控件）一定处于运行状态

                            customButtonRunStop.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;

                            //

                            if (Control_Graph.Control_Data.ButtonLearningShow)//【学习】按钮处于显示状态
                            {
                                //此时【学习】按钮一定处于使能状态

                                customButtonLearning.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                            }
                            else//【学习】按钮处于隐藏状态
                            {
                                //不执行操作
                            }
                        }
                    }
                    else//停止（即当前控件处于禁止状态）
                    {
                        //不执行操作
                    }

                    //

                    Control_Graph._SetControlBackgroundColor();

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ButtonRunStopShow属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("【运行/停止】按钮是否显示。true：是；false：否"), Category("Tolerances 通用")]
        public bool ButtonRunStopShow//属性
        {
            get//读取
            {
                return Control_Graph.Control_Data.ButtonRunStopShow;
            }
            set//设置
            {
                if (!bCurveMode)//非曲线图模式
                {
                    if (value != Control_Graph.Control_Data.ButtonRunStopShow)//设置了新的数值
                    {
                        Control_Graph.Control_Data.ButtonRunStopShow = value;

                        if (Control_Graph.Control_Data.ButtonRunStopShow)//显示
                        {
                            customButtonRunStop.Visible = true;
                        }
                        else//隐藏
                        {
                            customButtonRunStop.Visible = false;
                        }

                        //

                        _GetBackgroundImage();
                        _Draw();
                    }
                }
                else//曲线图模式
                {
                    Control_Graph.Control_Data.ButtonRunStopShow = false;

                    customButtonRunStop.Visible = false;

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ButtonLearningShow属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("【学习】按钮是否显示。true：是；false：否"), Category("Tolerances 通用")]
        public bool ButtonLearningShow//属性
        {
            get//读取
            {
                return Control_Graph.Control_Data.ButtonLearningShow;
            }
            set//设置
            {
                if (!bCurveMode)//非曲线图模式
                {
                    if (value != Control_Graph.Control_Data.ButtonLearningShow)//设置了新的数值
                    {
                        Control_Graph.Control_Data.ButtonLearningShow = value;

                        if (Control_Graph.Control_Data.ButtonLearningShow)//显示
                        {
                            customButtonLearning.Visible = true;
                        }
                        else//隐藏
                        {
                            customButtonLearning.Visible = false;
                        }

                        //

                        _GetBackgroundImage();
                        _Draw();
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：Caption属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("控件标题文本"), Category("Tolerances 通用")]
        public string Caption//属性
        {
            get//读取
            {
                return Control_Graph.Caption;
            }
            set//设置
            {
                if (value != Control_Graph.Caption)//设置了新的数值
                {
                    Control_Graph.Caption = value;

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：FontCaption 属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("绘制标题文本所使用的字体"), Category("Tolerances 通用")]
        public Font FontCaption //属性
        {
            get//读取
            {
                return Control_Graph.FontCaption;
            }
            set//设置
            {
                if (value != Control_Graph.FontCaption)
                {
                    Control_Graph.FontCaption = value;

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：LocationCaption 属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("标题位置"), Category("Tolerances 通用")]
        public Point LocationCaption //属性
        {
            get//读取
            {
                return Control_Graph.RectCaption.Location;
            }
            set//设置
            {
                if (value != Control_Graph.RectCaption.Location)
                {
                    Control_Graph.RectCaption = new Rectangle(value, Control_Graph.RectCaption.Size);

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：SizeCaption 属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("标题大小"), Category("Tolerances 通用")]
        public Size SizeCaption //属性
        {
            get//读取
            {
                return Control_Graph.RectCaption.Size;
            }
            set//设置
            {
                if (value != Control_Graph.RectCaption.Size)
                {
                    Control_Graph.RectCaption = new Rectangle(Control_Graph.RectCaption.Location, value);

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：LocationGraph 属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("控件绘制区域（曲线图）位置"), Category("Tolerances 通用")]
        public Point LocationGraph //属性
        {
            get//读取
            {
                return Control_Graph.RectGraph.Location;
            }
            set//设置
            {
                if (value != Control_Graph.RectGraph.Location)
                {
                    Control_Graph.RectGraph = new Rectangle(value, Control_Graph.RectGraph.Size);

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：SizeGraph 属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("控件绘制区域（曲线图）大小"), Category("Tolerances 通用")]
        public Size SizeGraph //属性
        {
            get//读取
            {
                return Control_Graph.RectGraph.Size;
            }
            set//设置
            {
                if (value != Control_Graph.RectGraph.Size)
                {
                    Control_Graph.RectGraph = new Rectangle(Control_Graph.RectGraph.Location, value);

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：LocationCurValue 属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("绘制区域中的当前值（X轴、Y轴当前值在本区域显示）的位置"), Category("Tolerances 通用")]
        public Point LocationCurValue //属性
        {
            get//读取
            {
                return Control_Graph.RectCurValue.Location;
            }
            set//设置
            {
                if (value != Control_Graph.RectCurValue.Location)
                {
                    Control_Graph.RectCurValue = new Rectangle(value, Control_Graph.RectCurValue.Size);

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：SizeCurValue 属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("绘制区域中的当前值（X轴、Y轴当前值在本区域显示）的大小"), Category("Tolerances 通用")]
        public Size SizeCurValue //属性
        {
            get//读取
            {
                return Control_Graph.RectCurValue.Size;
            }
            set//设置
            {
                if (value != Control_Graph.RectCurValue.Size)
                {
                    Control_Graph.RectCurValue = new Rectangle(Control_Graph.RectCurValue.Location, value);

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：LocationCurValueIcon 属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("绘制区域中的当前值（X轴、Y轴当前值在本区域显示）的指示图标的位置"), Category("Tolerances 通用")]
        public Point LocationCurValueIcon //属性
        {
            get//读取
            {
                return Control_Graph.RectCurValueIcon.Location;
            }
            set//设置
            {
                if (value != Control_Graph.RectCurValueIcon.Location)
                {
                    Control_Graph.RectCurValueIcon = new Rectangle(value, Control_Graph.RectCurValueIcon.Size);

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：SizeCurValueIcon 属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("绘制区域中的当前值（X轴、Y轴当前值在本区域显示）的指示图标的大小"), Category("Tolerances 通用")]
        public Size SizeCurValueIcon //属性
        {
            get//读取
            {
                return Control_Graph.RectCurValueIcon.Size;
            }
            set//设置
            {
                if (value != Control_Graph.RectCurValueIcon.Size)
                {
                    Control_Graph.RectCurValueIcon = new Rectangle(Control_Graph.RectCurValueIcon.Location, value);

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ColorDrawCurValue 属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("绘制当前值所使用的颜色"), Category("Tolerances 通用")]
        public Color ColorDrawCurValue //属性
        {
            get//读取
            {
                return Control_Graph.SolidbrushDrawCurValue.Color;
            }
            set//设置
            {
                if (value != Control_Graph.SolidbrushDrawCurValue.Color)
                {
                    Control_Graph.SolidbrushDrawCurValue = new SolidBrush(value);

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：FontCurValue 属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("绘制当前值所使用的字体"), Category("Tolerances 通用")]
        public Font FontCurValue //属性
        {
            get//读取
            {
                return Control_Graph.FontCurValue;
            }
            set//设置
            {
                if (value != Control_Graph.FontCurValue)
                {
                    Control_Graph.FontCurValue = value;

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：ToolsSign属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("控件特征数据，用于表示该控件的类型"), Category("Tolerances 通用")]
        public int ToolsSign//属性
        {
            get//读取
            {
                return Control_Graph.Control_Data.TolerancesTools.ToolsSign;
            }
            set//设置
            {
                if (value != Control_Graph.Control_Data.TolerancesTools.ToolsSign)//设置了新的数值
                {
                    Control_Graph.Control_Data.TolerancesTools.ToolsSign = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：Learned属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("是否经过学习。true：是；false：否"), Category("Tolerances 通用")]
        public bool Learned//属性
        {
            get//读取
            {
                return Control_Graph.Control_Data.TolerancesTools.Learned;
            }
            set//设置
            {
                if (value != Control_Graph.Control_Data.TolerancesTools.Learned)//设置了新的数值
                {
                    Control_Graph.Control_Data.TolerancesTools.Learned = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：LearnedValue属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("学习数值"), Category("Tolerances 通用")]
        public int LearnedValue//属性
        {
            get//读取
            {
                return Control_Graph.Control_Data.TolerancesTools.LearnedValue;
            }
            set//设置
            {
                if (value != Control_Graph.Control_Data.TolerancesTools.LearnedValue)//设置了新的数值
                {
                    Control_Graph.Control_Data.TolerancesTools.LearnedValue = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ValidValue属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("学习中的有效数值数量"), Category("Tolerances 通用")]
        public int ValidValue//属性
        {
            get//读取
            {
                return Control_Graph.Control_Data.TolerancesTools.ValidValue;
            }
            set//设置
            {
                if (value != Control_Graph.Control_Data.TolerancesTools.ValidValue)
                {
                    Control_Graph.Control_Data.TolerancesTools.ValidValue = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：NonvalidValue属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("学习中的无效数值数量"), Category("Tolerances 通用")]
        public int NonvalidValue//属性
        {
            get//读取
            {
                return Control_Graph.Control_Data.TolerancesTools.NonvalidValue;
            }
            set//设置
            {
                if (value != Control_Graph.Control_Data.TolerancesTools.NonvalidValue)
                {
                    Control_Graph.Control_Data.TolerancesTools.NonvalidValue = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：EffectiveMin_State属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("下限值是否有效"), Category("Tolerances 通用")]
        public Boolean EffectiveMin_State//属性
        {
            get//读取
            {
                return Control_Graph.Control_Data.TolerancesTools.EffectiveMin_State;
            }
            set//设置
            {
                if (value != Control_Graph.Control_Data.TolerancesTools.EffectiveMin_State)
                {
                    Control_Graph.Control_Data.TolerancesTools.EffectiveMin_State = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：NonvalidValue属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("上限值是否有效"), Category("Tolerances 通用")]
        public Boolean EffectiveMax_State//属性
        {
            get//读取
            {
                return Control_Graph.Control_Data.TolerancesTools.EffectiveMax_State;
            }
            set//设置
            {
                if (value != Control_Graph.Control_Data.TolerancesTools.EffectiveMax_State)
                {
                    Control_Graph.Control_Data.TolerancesTools.EffectiveMax_State = value;
                }
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：ValuePointPixelWidth属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("绘制区域中的曲线值点像素宽度"), Category("Tolerances 通用")]
        public Int32 ValuePointPixelWidth//属性
        {
            get//读取
            {
                return Control_Graph.ValuePointPixelWidth;
            }
            set//设置
            {
                if (value != Control_Graph.ValuePointPixelWidth)
                {
                    Control_Graph.ValuePointPixelWidth = value;

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：AutoXAxisValue属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("曲线值X轴数值是否自动。取值范围：true，是；false，否"), Category("Tolerances 通用")]
        public Boolean AutoXAxisValue//属性
        {
            get//读取
            {
                return Control_Graph.AutoXAxisValue;
            }
            set//设置
            {
                if (value != Control_Graph.AutoXAxisValue)
                {
                    Control_Graph.AutoXAxisValue = value;

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：GridDisplay属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("网格是否显示。取值范围：true，显示；false，不显示"), Category("Tolerances 通用")]
        public Boolean GridDisplay//属性
        {
            get//读取
            {
                return Control_Graph.GridDisplay;
            }
            set//设置
            {
                if (value != Control_Graph.GridDisplay)
                {
                    Control_Graph.GridDisplay = value;

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ColorGridLine属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("网格线的颜色"), Category("Tolerances 通用")]
        public Color ColorGridLine//属性
        {
            get//读取
            {
                return Control_Graph.ColorGridLine;
            }
            set//设置
            {
                if (value != Control_Graph.ColorGridLine)
                {
                    Control_Graph.ColorGridLine = value;

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：GridLineWidth属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("网格线的宽度"), Category("Tolerances 通用")]
        public float GridLineWidth//属性
        {
            get//读取
            {
                return Control_Graph.GridLineWidth;
            }
            set//设置
            {
                if (value != Control_Graph.GridLineWidth)
                {
                    Control_Graph.GridLineWidth = value;

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ValuePointDisplay属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("曲线值点是否显示。取值范围：true，显示；false，不显示"), Category("Tolerances 通用")]
        public Boolean ValuePointDisplay//属性
        {
            get//读取
            {
                return Control_Graph.ValuePointDisplay;
            }
            set//设置
            {
                Control_Graph.ValuePointDisplay = value;

                //

                _GetBackgroundImage();
                _Draw();
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：CurveDisplay属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("曲线是否显示。取值范围：true，显示；false，不显示"), Category("Tolerances 通用")]
        public Boolean[] CurveDisplay//属性
        {
            get//读取
            {
                return Control_Graph.CurveDisplay;
            }
            set//设置
            {
                Control_Graph.CurveDisplay = value;

                //

                _GetBackgroundImage();
                _Draw();
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ColorControlSelected属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("选中控件时的背景颜色"), Category("Tolerances 通用")]
        public Color ColorControlSelected//属性
        {
            get//读取
            {
                return Control_Graph.ColorControlSelected;
            }
            set//设置
            {
                Control_Graph.ColorControlSelected = value;

                //

                _GetBackgroundImage();
                _Draw();
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ColorControlUnselected属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("未选中控件时的背景颜色"), Category("Tolerances 通用")]
        public Color ColorControlUnselected//属性
        {
            get//读取
            {
                return Control_Graph.ColorControlUnselected;
            }
            set//设置
            {
                Control_Graph.ColorControlUnselected = value;

                //

                _GetBackgroundImage();
                _Draw();
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ColorDrawCaptionEnable属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("标题文本有效时的颜色"), Category("Tolerances 通用")]
        public Color ColorDrawCaptionEnable//属性
        {
            get//读取
            {
                return Control_Graph.ColorDrawCaptionEnable;
            }
            set//设置
            {
                Control_Graph.ColorDrawCaptionEnable = value;

                //

                _GetBackgroundImage();
                _Draw();
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ColorDrawCaptionEnable属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("标题文本无效时的颜色"), Category("Tolerances 通用")]
        public Color ColorDrawCaptionDisable//属性
        {
            get//读取
            {
                return Control_Graph.ColorDrawCaptionDisable;
            }
            set//设置
            {
                Control_Graph.ColorDrawCaptionDisable = value;

                //

                _GetBackgroundImage();
                _Draw();
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ColorDrawGraph属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("绘制控件绘制区域（曲线图）所使用的颜色"), Category("Tolerances 通用")]
        public Color ColorDrawGraph//属性
        {
            get//读取
            {
                return Control_Graph.ColorDrawGraph;
            }
            set//设置
            {
                Control_Graph.ColorDrawGraph = value;

                //

                _GetBackgroundImage();
                _Draw();
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ColorValueEnable属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("曲线图数值有效时的颜色"), Category("Tolerances 通用")]
        public Color ColorValueEnable//属性
        {
            get//读取
            {
                return Control_Graph.ColorValueEnable;
            }
            set//设置
            {
                Control_Graph.ColorValueEnable = value;

                //

                _GetBackgroundImage();
                _Draw();
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ColorValueDisable属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("曲线图数值无效时的颜色"), Category("Tolerances 通用")]
        public Color ColorValueDisable//属性
        {
            get//读取
            {
                return Control_Graph.ColorValueDisable;
            }
            set//设置
            {
                Control_Graph.ColorValueDisable = value;

                //

                _GetBackgroundImage();
                _Draw();
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ColorValueUndefined属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("曲线图数值超出范围（未定义）时的颜色"), Category("Tolerances 通用")]
        public Color ColorValueUndefined//属性
        {
            get//读取
            {
                return Control_Graph.ColorValueUndefined;
            }
            set//设置
            {
                Control_Graph.ColorValueUndefined = value;

                //

                _GetBackgroundImage();
                _Draw();
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ColorLine属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("曲线连接线的颜色"), Category("Tolerances 通用")]
        public Color[] ColorLine//属性
        {
            get//读取
            {
                return Control_Graph.ColorLine;
            }
            set//设置
            {
                Control_Graph.ColorLine = value;

                //

                _GetBackgroundImage();
                _Draw();
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：LineWidth属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("曲线连接线的宽度"), Category("Tolerances 通用")]
        public float[] LineWidth//属性
        {
            get//读取
            {
                return Control_Graph.LineWidth;
            }
            set//设置
            {
                Control_Graph.LineWidth = value;

                //

                _GetBackgroundImage();
                _Draw();
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：XAxisValuePrecision属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("曲线图坐标轴上显示的数值精度（0，不显示小数位；大于0，显示相应的小数位）"), Category("Tolerances X坐标轴数据")]
        public int XAxisValuePrecision //属性
        {
            get//读取
            {
                return Control_Graph.Graph_XAxis.AxisValuePrecision;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_XAxis.AxisValuePrecision)
                {
                    Control_Graph.Graph_XAxis.AxisValuePrecision = value;

                    //

                    Control_Graph.Graph_XAxis._SetAxisData();//配置X坐标轴
                    Control_Graph.Graph_YAxis._SetAxisData();//配置Y坐标轴
                    Control_Graph._SetCurveValue();//配置曲线图

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：YAxisValuePrecision属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("曲线图坐标轴上显示的数值精度（0，不显示小数位；大于0，显示相应的小数位）"), Category("Tolerances Y坐标轴数据")]
        public int YAxisValuePrecision //属性
        {
            get//读取
            {
                return Control_Graph.Graph_YAxis.AxisValuePrecision;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_YAxis.AxisValuePrecision)
                {
                    Control_Graph.Graph_YAxis.AxisValuePrecision = value;

                    //

                    Control_Graph.Graph_XAxis._SetAxisData();//配置X坐标轴
                    Control_Graph.Graph_YAxis._SetAxisData();//配置Y坐标轴
                    Control_Graph._SetCurveValue();//配置曲线图

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：XAxisAdditionalValuePrecision属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("曲线图坐标轴上显示的附加数值精度（0，不显示小数位；大于0，显示相应的小数位）"), Category("Tolerances X坐标轴数据")]
        public int XAxisAdditionalValuePrecision //属性
        {
            get//读取
            {
                return Control_Graph.Graph_XAxis.AxisAdditionalValuePrecision;
            }
            set//设置
            {
                Control_Graph.Graph_XAxis.AxisAdditionalValuePrecision = value;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：YAxisAdditionalValuePrecision属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("曲线图坐标轴上显示的附加数值精度（0，不显示小数位；大于0，显示相应的小数位）"), Category("Tolerances Y坐标轴数据")]
        public int YAxisAdditionalValuePrecision //属性
        {
            get//读取
            {
                return Control_Graph.Graph_YAxis.AxisAdditionalValuePrecision;
            }
            set//设置
            {
                Control_Graph.Graph_YAxis.AxisAdditionalValuePrecision = value;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：XAxisDataType属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("坐标轴数据形式。取值范围：AxisDataType.withoutEffectiveValue：不指定最小有效实际数值、最大有效实际数值，但指定坐标轴的最小值、最大值；AxisDataType.withEffectiveValue：指定最小有效实际数值、最大有效实际数值，同时指定坐标轴的最小值、最大值；AxisDataType.withPixelAndEffectiveValue：指定最小有效实际数值、最大有效实际数值，同时指定距离像素值"), Category("Tolerances X坐标轴数据")]
        public VisionSystemClassLibrary.Enum.AxisDataType XAxisDataType //属性
        {
            get//读取
            {
                return Control_Graph.Graph_XAxis.DataType;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_XAxis.DataType)
                {
                    Control_Graph.Graph_XAxis.DataType = value;

                    //

                    Control_Graph.Graph_XAxis._SetAxisData();//配置X坐标轴
                    Control_Graph.Graph_YAxis._SetAxisData();//配置Y坐标轴
                    Control_Graph._SetCurveValue();//配置曲线图

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：YAxisDataType属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("坐标轴数据形式。取值范围：AxisDataType.withoutEffectiveValue：不指定最小有效实际数值、最大有效实际数值，但指定坐标轴的最小值、最大值；AxisDataType.withEffectiveValue：指定最小有效实际数值、最大有效实际数值，同时指定坐标轴的最小值、最大值；AxisDataType.withPixelAndEffectiveValue：指定最小有效实际数值、最大有效实际数值，同时指定距离像素值"), Category("Tolerances Y坐标轴数据")]
        public VisionSystemClassLibrary.Enum.AxisDataType YAxisDataType //属性
        {
            get//读取
            {
                return Control_Graph.Graph_YAxis.DataType;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_YAxis.DataType)
                {
                    Control_Graph.Graph_YAxis.DataType = value;

                    //

                    Control_Graph.Graph_XAxis._SetAxisData();//配置X坐标轴
                    Control_Graph.Graph_YAxis._SetAxisData();//配置Y坐标轴
                    Control_Graph._SetCurveValue();//配置曲线图

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：XAxisValueNumber属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("坐标轴显示的坐标值个数（不包含有效值）"), Category("Tolerances X坐标轴数据")]
        public int XAxisValueNumber //属性
        {
            get//读取
            {
                return Control_Graph.Graph_XAxis.AxisValueNumber;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_XAxis.AxisValueNumber)
                {
                    if (!(Control_Graph.Graph_XAxis.AxisAdditionalValueDisplay))//不显示附加值
                    {
                        Control_Graph.Graph_XAxis.AxisValueNumber = value;

                        //

                        Control_Graph.Graph_XAxis._SetAxisData();//配置X坐标轴
                        Control_Graph.Graph_YAxis._SetAxisData();//配置Y坐标轴
                        Control_Graph._SetCurveValue();//配置曲线图

                        //

                        _GetBackgroundImage();
                        _Draw();
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：YAxisValueNumber属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("坐标轴显示的坐标值个数（不包含有效值）"), Category("Tolerances Y坐标轴数据")]
        public int YAxisValueNumber //属性
        {
            get//读取
            {
                return Control_Graph.Graph_YAxis.AxisValueNumber;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_YAxis.AxisValueNumber)
                {
                    if (!(Control_Graph.Graph_YAxis.AxisAdditionalValueDisplay))//不显示附加值
                    {
                        Control_Graph.Graph_YAxis.AxisValueNumber = value;

                        //

                        Control_Graph.Graph_XAxis._SetAxisData();//配置X坐标轴
                        Control_Graph.Graph_YAxis._SetAxisData();//配置Y坐标轴
                        Control_Graph._SetCurveValue();//配置曲线图

                        //

                        _GetBackgroundImage();
                        _Draw();
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：XAxisValueDisplay属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("曲线图坐标轴上是否显示坐标轴数值。true：是；false：否"), Category("Tolerances X坐标轴数据")]
        public bool XAxisValueDisplay //属性
        {
            get//读取
            {
                return Control_Graph.Graph_XAxis.AxisValueDisplay;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_XAxis.AxisValueDisplay)
                {
                    Control_Graph.Graph_XAxis.AxisValueDisplay = value;

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：YAxisValueDisplay属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("曲线图坐标轴上是否显示坐标轴数值。true：是；false：否"), Category("Tolerances Y坐标轴数据")]
        public bool YAxisValueDisplay //属性
        {
            get//读取
            {
                return Control_Graph.Graph_YAxis.AxisValueDisplay;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_YAxis.AxisValueDisplay)
                {
                    Control_Graph.Graph_YAxis.AxisValueDisplay = value;

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：XAxisAdditionalValueDisplay属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("曲线图坐标轴上是否显示附加数值。true：是；false：否"), Category("Tolerances X坐标轴数据")]
        public bool XAxisAdditionalValueDisplay //属性
        {
            get//读取
            {
                return Control_Graph.Graph_XAxis.AxisAdditionalValueDisplay;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_XAxis.AxisAdditionalValueDisplay)
                {
                    Control_Graph.Graph_XAxis.AxisAdditionalValueDisplay = value;

                    //

                    if (Control_Graph.Graph_XAxis.AxisAdditionalValueDisplay)//曲线图坐标轴上显示附加数值
                    {
                        Control_Graph.Graph_XAxis.AxisValueNumber = 2;
                    }

                    Control_Graph.Graph_XAxis._SetAxisData();//配置X坐标轴
                    Control_Graph.Graph_YAxis._SetAxisData();//配置Y坐标轴
                    Control_Graph._SetCurveValue();//配置曲线图

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：YAxisAdditionalValueDisplay属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("曲线图坐标轴上是否显示附加数值。true：是；false：否"), Category("Tolerances Y坐标轴数据")]
        public bool YAxisAdditionalValueDisplay //属性
        {
            get//读取
            {
                return Control_Graph.Graph_YAxis.AxisAdditionalValueDisplay;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_YAxis.AxisAdditionalValueDisplay)
                {
                    Control_Graph.Graph_YAxis.AxisAdditionalValueDisplay = value;

                    //

                    if (Control_Graph.Graph_YAxis.AxisAdditionalValueDisplay)//曲线图坐标轴上显示附加数值
                    {
                        Control_Graph.Graph_YAxis.AxisValueNumber = 2;
                    }

                    Control_Graph.Graph_XAxis._SetAxisData();//配置X坐标轴
                    Control_Graph.Graph_YAxis._SetAxisData();//配置Y坐标轴
                    Control_Graph._SetCurveValue();//配置曲线图

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：XAxisAdditionalValue属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("曲线图坐标轴上显示的附加数值"), Category("Tolerances X坐标轴数据")]
        public double XAxisAdditionalValue //属性
        {
            get//读取
            {
                return Control_Graph.Graph_XAxis.AxisAdditionalValue;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_XAxis.AxisAdditionalValue)
                {
                    Control_Graph.Graph_XAxis.AxisAdditionalValue = value;

                    //

                    Control_Graph.Graph_XAxis._SetAxisData();//配置X坐标轴
                    Control_Graph.Graph_YAxis._SetAxisData();//配置Y坐标轴
                    Control_Graph._SetCurveValue();//配置曲线图

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：YAxisAdditionalValue属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("曲线图坐标轴上显示的附加数值"), Category("Tolerances Y坐标轴数据")]
        public double YAxisAdditionalValue //属性
        {
            get//读取
            {
                return Control_Graph.Graph_YAxis.AxisAdditionalValue;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_YAxis.AxisAdditionalValue)
                {
                    Control_Graph.Graph_YAxis.AxisAdditionalValue = value;

                    //

                    Control_Graph.Graph_XAxis._SetAxisData();//配置X坐标轴
                    Control_Graph.Graph_YAxis._SetAxisData();//配置Y坐标轴
                    Control_Graph._SetCurveValue();//配置曲线图

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：XAxisAdditionalValueRatio属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("曲线图坐标轴上显示的附加数值系数"), Category("Tolerances X坐标轴数据")]
        public Single XAxisAdditionalValueRatio //属性
        {
            get//读取
            {
                return Control_Graph.Graph_XAxis.AdditionalValueRatio;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_XAxis.AdditionalValueRatio)
                {
                    Control_Graph.Graph_XAxis.AdditionalValueRatio = value;

                    //

                    Control_Graph.Graph_XAxis._SetAxisData();//配置X坐标轴
                    Control_Graph.Graph_XAxis._SetAxisData();//配置Y坐标轴
                    Control_Graph._SetCurveValue();//配置曲线图

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：YAxisAdditionalValueRatio属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("曲线图坐标轴上显示的附加数值系数"), Category("Tolerances Y坐标轴数据")]
        public Single YAxisAdditionalValueRatio //属性
        {
            get//读取
            {
                return Control_Graph.Graph_YAxis.AdditionalValueRatio;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_YAxis.AdditionalValueRatio)
                {
                    Control_Graph.Graph_YAxis.AdditionalValueRatio = value;

                    //

                    Control_Graph.Graph_XAxis._SetAxisData();//配置X坐标轴
                    Control_Graph.Graph_YAxis._SetAxisData();//配置Y坐标轴
                    Control_Graph._SetCurveValue();//配置曲线图

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：XAxisAdditionalValueUnit属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("曲线图坐标轴上显示的附加数值单位"), Category("Tolerances X坐标轴数据")]
        public string XAxisAdditionalValueUnit //属性
        {
            get//读取
            {
                return Control_Graph.Graph_XAxis.AdditionalValueUnit;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_XAxis.AdditionalValueUnit)
                {
                    Control_Graph.Graph_XAxis.AdditionalValueUnit = value;

                    //

                    Control_Graph.Graph_XAxis._SetAxisData();//配置X坐标轴
                    Control_Graph.Graph_XAxis._SetAxisData();//配置Y坐标轴
                    Control_Graph._SetCurveValue();//配置曲线图

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：YAxisAdditionalValueUnit属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("曲线图坐标轴上显示的附加数值单位"), Category("Tolerances Y坐标轴数据")]
        public string YAxisAdditionalValueUnit //属性
        {
            get//读取
            {
                return Control_Graph.Graph_YAxis.AdditionalValueUnit;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_YAxis.AdditionalValueUnit)
                {
                    Control_Graph.Graph_YAxis.AdditionalValueUnit = value;

                    //

                    Control_Graph.Graph_XAxis._SetAxisData();//配置X坐标轴
                    Control_Graph.Graph_XAxis._SetAxisData();//配置Y坐标轴
                    Control_Graph._SetCurveValue();//配置曲线图

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：XAxisLocationAdditionalValue属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("曲线图坐标轴上显示的附加数值的位置"), Category("Tolerances X坐标轴数据")]
        public Point XAxisLocationAdditionalValue //属性
        {
            get//读取
            {
                return Control_Graph.Graph_XAxis.RectAdditionalValue.Location;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_XAxis.RectAdditionalValue.Location)
                {
                    Control_Graph.Graph_XAxis.RectAdditionalValue = new Rectangle(value, Control_Graph.Graph_XAxis.RectAdditionalValue.Size);

                    //

                    Control_Graph.Graph_XAxis._SetAxisData();//配置X坐标轴
                    Control_Graph.Graph_YAxis._SetAxisData();//配置Y坐标轴
                    Control_Graph._SetCurveValue();//配置曲线图

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：YAxisLocationAdditionalValue属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("曲线图坐标轴上显示的附加数值的位置"), Category("Tolerances Y坐标轴数据")]
        public Point YAxisLocationAdditionalValue //属性
        {
            get//读取
            {
                return Control_Graph.Graph_YAxis.RectAdditionalValue.Location;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_YAxis.RectAdditionalValue.Location)
                {
                    Control_Graph.Graph_YAxis.RectAdditionalValue = new Rectangle(value, Control_Graph.Graph_YAxis.RectAdditionalValue.Size);

                    //

                    Control_Graph.Graph_XAxis._SetAxisData();//配置X坐标轴
                    Control_Graph.Graph_YAxis._SetAxisData();//配置Y坐标轴
                    Control_Graph._SetCurveValue();//配置曲线图

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：XAxisSizeAdditionalValue属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("曲线图坐标轴上显示的附加数值的大小"), Category("Tolerances X坐标轴数据")]
        public Size XAxisSizeAdditionalValue //属性
        {
            get//读取
            {
                return Control_Graph.Graph_XAxis.RectAdditionalValue.Size;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_XAxis.RectAdditionalValue.Size)
                {
                    Control_Graph.Graph_XAxis.RectAdditionalValue = new Rectangle(Control_Graph.Graph_XAxis.RectAdditionalValue.Location, value);

                    //

                    Control_Graph.Graph_XAxis._SetAxisData();//配置X坐标轴
                    Control_Graph.Graph_YAxis._SetAxisData();//配置Y坐标轴
                    Control_Graph._SetCurveValue();//配置曲线图

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：YAxisSizeAdditionalValue属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("曲线图坐标轴上显示的附加数值的大小"), Category("Tolerances Y坐标轴数据")]
        public Size YAxisSizeAdditionalValue //属性
        {
            get//读取
            {
                return Control_Graph.Graph_YAxis.RectAdditionalValue.Size;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_YAxis.RectAdditionalValue.Size)
                {
                    Control_Graph.Graph_YAxis.RectAdditionalValue = new Rectangle(Control_Graph.Graph_YAxis.RectAdditionalValue.Location, value);

                    //

                    Control_Graph.Graph_XAxis._SetAxisData();//配置X坐标轴
                    Control_Graph.Graph_YAxis._SetAxisData();//配置Y坐标轴
                    Control_Graph._SetCurveValue();//配置曲线图

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：XAxisSizePoint属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("显示的曲线图坐标轴中每个坐标点的区域大小"), Category("Tolerances X坐标轴数据")]
        public Size XAxisSizePoint //属性
        {
            get//读取
            {
                return Control_Graph.Graph_XAxis.SizePixel_AxisPoint;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_XAxis.SizePixel_AxisPoint)
                {
                    Control_Graph.Graph_XAxis.SizePixel_AxisPoint = value;

                    //

                    Control_Graph.Graph_XAxis._SetAxisData();//配置X坐标轴
                    Control_Graph.Graph_YAxis._SetAxisData();//配置Y坐标轴
                    Control_Graph._SetCurveValue();//配置曲线图

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：SizeYAxisPoint属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("显示的曲线图坐标轴中每个坐标点的区域大小"), Category("Tolerances Y坐标轴数据")]
        public Size YAxisSizePoint //属性
        {
            get//读取
            {
                return Control_Graph.Graph_YAxis.SizePixel_AxisPoint;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_YAxis.SizePixel_AxisPoint)
                {
                    Control_Graph.Graph_YAxis.SizePixel_AxisPoint = value;

                    //

                    Control_Graph.Graph_XAxis._SetAxisData();//配置X坐标轴
                    Control_Graph.Graph_YAxis._SetAxisData();//配置Y坐标轴
                    Control_Graph._SetCurveValue();//配置曲线图

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：XAxisLocationMin属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("坐标轴最小值对应的像素值"), Category("Tolerances X坐标轴数据")]
        public Point XAxisLocationMin //属性
        {
            get//读取
            {
                return Control_Graph.Graph_XAxis.LocationAxisMin_Pixel;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_XAxis.LocationAxisMin_Pixel)
                {
                    Control_Graph.Graph_XAxis.LocationAxisMin_Pixel = value;

                    //

                    Control_Graph.Graph_XAxis._SetAxisData();//配置X坐标轴
                    Control_Graph.Graph_YAxis._SetAxisData();//配置Y坐标轴
                    Control_Graph._SetCurveValue();//配置曲线图

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：YAxisLocationMin属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("坐标轴最小值对应的像素值"), Category("Tolerances Y坐标轴数据")]
        public Point YAxisLocationMin //属性
        {
            get//读取
            {
                return Control_Graph.Graph_YAxis.LocationAxisMin_Pixel;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_YAxis.LocationAxisMin_Pixel)
                {
                    Control_Graph.Graph_YAxis.LocationAxisMin_Pixel = value;

                    //

                    Control_Graph.Graph_XAxis._SetAxisData();//配置X坐标轴
                    Control_Graph.Graph_YAxis._SetAxisData();//配置Y坐标轴
                    Control_Graph._SetCurveValue();//配置曲线图

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：XAxisMinValue属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("坐标轴最小值对应的实际数值"), Category("Tolerances X坐标轴数据")]
        public Single XAxisMinValue //属性
        {
            get//读取
            {
                return Control_Graph.Graph_XAxis.AxisMin_Value;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_XAxis.AxisMin_Value)
                {
                    Control_Graph.Graph_XAxis.AxisMin_Value = value;

                    //

                    Control_Graph._SetValue_XAxis();

                    Control_Graph.Graph_XAxis._SetAxisData();//配置X坐标轴
                    Control_Graph.Graph_YAxis._SetAxisData();//配置Y坐标轴
                    Control_Graph._SetCurveValue();//配置曲线图
                    Control_Graph._SetCurrentValue();//配置当前值

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：YAxisMinValue属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("坐标轴最小值对应的实际数值"), Category("Tolerances Y坐标轴数据")]
        public Single YAxisMinValue //属性
        {
            get//读取
            {
                return Control_Graph.Graph_YAxis.AxisMin_Value;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_YAxis.AxisMin_Value)
                {
                    Control_Graph.Graph_YAxis.AxisMin_Value = value;

                    //

                    Control_Graph.Graph_XAxis._SetAxisData();//配置X坐标轴
                    Control_Graph.Graph_YAxis._SetAxisData();//配置Y坐标轴
                    Control_Graph._SetCurveValue();//配置曲线图
                    Control_Graph._SetCurrentValue();//配置当前值

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：XAxisLocationMax属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("坐标轴最大值对应的像素值"), Category("Tolerances X坐标轴数据")]
        public Point XAxisLocationMax //属性
        {
            get//读取
            {
                return Control_Graph.Graph_XAxis.LocationAxisMax_Pixel;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_XAxis.LocationAxisMax_Pixel)
                {
                    Control_Graph.Graph_XAxis.LocationAxisMax_Pixel = value;

                    //

                    Control_Graph.Graph_XAxis._SetAxisData();//配置X坐标轴
                    Control_Graph.Graph_YAxis._SetAxisData();//配置Y坐标轴
                    Control_Graph._SetCurveValue();//配置曲线图

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：YAxisLocationMax属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("坐标轴最大值对应的像素值"), Category("Tolerances Y坐标轴数据")]
        public Point YAxisLocationMax //属性
        {
            get//读取
            {
                return Control_Graph.Graph_YAxis.LocationAxisMax_Pixel;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_YAxis.LocationAxisMax_Pixel)
                {
                    Control_Graph.Graph_YAxis.LocationAxisMax_Pixel = value;

                    //

                    Control_Graph.Graph_XAxis._SetAxisData();//配置X坐标轴
                    Control_Graph.Graph_YAxis._SetAxisData();//配置Y坐标轴
                    Control_Graph._SetCurveValue();//配置曲线图

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：XAxisMaxValue属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("坐标轴最大值对应的实际数值"), Category("Tolerances X坐标轴数据")]
        public Single XAxisMaxValue //属性
        {
            get//读取
            {
                return Control_Graph.Graph_XAxis.AxisMax_Value;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_XAxis.AxisMax_Value)
                {
                    Control_Graph.Graph_XAxis.AxisMax_Value = value;

                    //

                    Control_Graph._SetValue_XAxis();

                    Control_Graph.Graph_XAxis._SetAxisData();//配置X坐标轴
                    Control_Graph.Graph_YAxis._SetAxisData();//配置Y坐标轴
                    Control_Graph._SetCurveValue();//配置曲线图
                    Control_Graph._SetCurrentValue();//配置当前值

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：YAxisMaxValue属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("坐标轴最大值对应的实际数值"), Category("Tolerances Y坐标轴数据")]
        public Single YAxisMaxValue //属性
        {
            get//读取
            {
                return Control_Graph.Graph_YAxis.AxisMax_Value;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_YAxis.AxisMax_Value)
                {
                    Control_Graph.Graph_YAxis.AxisMax_Value = value;

                    //

                    Control_Graph.Graph_XAxis._SetAxisData();//配置X坐标轴
                    Control_Graph.Graph_YAxis._SetAxisData();//配置Y坐标轴
                    Control_Graph._SetCurveValue();//配置曲线图
                    Control_Graph._SetCurrentValue();//配置当前值

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：XAxisEffectiveMinValue属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("坐标轴最小有效实际数值"), Category("Tolerances X坐标轴数据")]
        public Single XAxisEffectiveMinValue //属性
        {
            get//读取
            {
                return Control_Graph.Graph_XAxis.AxisEffectiveMin_Value;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_XAxis.AxisEffectiveMin_Value)
                {
                    Control_Graph.Graph_XAxis.AxisEffectiveMin_Value = value;

                    //

                    Control_Graph.Graph_XAxis._SetAxisData();//配置X坐标轴
                    Control_Graph.Graph_YAxis._SetAxisData();//配置Y坐标轴
                    Control_Graph._SetCurveValue();//配置曲线图
                    Control_Graph._SetCurrentValue();//配置当前值

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：YAxisEffectiveMinValue属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("坐标轴最小有效实际数值"), Category("Tolerances Y坐标轴数据")]
        public Single YAxisEffectiveMinValue //属性
        {
            get//读取
            {
                return Control_Graph.Graph_YAxis.AxisEffectiveMin_Value;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_YAxis.AxisEffectiveMin_Value)
                {
                    Control_Graph.Graph_YAxis.AxisEffectiveMin_Value = value;

                    //

                    Control_Graph.Graph_XAxis._SetAxisData();//配置X坐标轴
                    Control_Graph.Graph_YAxis._SetAxisData();//配置Y坐标轴
                    Control_Graph._SetCurveValue();//配置曲线图
                    Control_Graph._SetCurrentValue();//配置当前值

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：XAxisEffectiveMaxValue属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("坐标轴最大有效实际数值"), Category("Tolerances X坐标轴数据")]
        public Single XAxisEffectiveMaxValue //属性
        {
            get//读取
            {
                return Control_Graph.Graph_XAxis.AxisEffectiveMax_Value;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_XAxis.AxisEffectiveMax_Value)
                {
                    Control_Graph.Graph_XAxis.AxisEffectiveMax_Value = value;

                    //

                    Control_Graph.Graph_XAxis._SetAxisData();//配置X坐标轴
                    Control_Graph.Graph_YAxis._SetAxisData();//配置Y坐标轴
                    Control_Graph._SetCurveValue();//配置曲线图
                    Control_Graph._SetCurrentValue();//配置当前值

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：YAxisEffectiveMaxValue属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("坐标轴最大有效实际数值"), Category("Tolerances Y坐标轴数据")]
        public Single YAxisEffectiveMaxValue //属性
        {
            get//读取
            {
                return Control_Graph.Graph_YAxis.AxisEffectiveMax_Value;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_YAxis.AxisEffectiveMax_Value)
                {
                    Control_Graph.Graph_YAxis.AxisEffectiveMax_Value = value;

                    //

                    Control_Graph.Graph_XAxis._SetAxisData();//配置X坐标轴
                    Control_Graph.Graph_YAxis._SetAxisData();//配置Y坐标轴
                    Control_Graph._SetCurveValue();//配置曲线图
                    Control_Graph._SetCurrentValue();//配置当前值

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：FromXAxisMinToEffectiveMin属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("坐标轴最小有效实际数值对应的坐标点像素值与坐标轴最小值对应的坐标点像素值之间的距离像素值"), Category("Tolerances X坐标轴数据")]
        public int FromXAxisMinToEffectiveMin //属性
        {
            get//读取
            {
                return Control_Graph.Graph_XAxis.FromAxisMinToEffectiveMin_Pixel;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_XAxis.FromAxisMinToEffectiveMin_Pixel)
                {
                    Control_Graph.Graph_XAxis.FromAxisMinToEffectiveMin_Pixel = value;

                    //

                    Control_Graph.Graph_XAxis._SetAxisData();//配置X坐标轴
                    Control_Graph.Graph_YAxis._SetAxisData();//配置Y坐标轴
                    Control_Graph._SetCurveValue();//配置曲线图

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：FromYAxisMinToEffectiveMin属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("坐标轴最小有效实际数值对应的坐标点像素值与坐标轴最小值对应的坐标点像素值之间的距离像素值"), Category("Tolerances Y坐标轴数据")]
        public int FromYAxisMinToEffectiveMin //属性
        {
            get//读取
            {
                return Control_Graph.Graph_YAxis.FromAxisMinToEffectiveMin_Pixel;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_YAxis.FromAxisMinToEffectiveMin_Pixel)
                {
                    Control_Graph.Graph_YAxis.FromAxisMinToEffectiveMin_Pixel = value;

                    //

                    Control_Graph.Graph_XAxis._SetAxisData();//配置X坐标轴
                    Control_Graph.Graph_YAxis._SetAxisData();//配置Y坐标轴
                    Control_Graph._SetCurveValue();//配置曲线图

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：FromXAxisMaxToEffectiveMax属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("坐标轴最大有效实际数值对应的坐标点像素值与坐标轴最大值对应的坐标点像素值之间的距离像素值"), Category("Tolerances X坐标轴数据")]
        public int FromXAxisMaxToEffectiveMax //属性
        {
            get//读取
            {
                return Control_Graph.Graph_XAxis.FromAxisMaxToEffectiveMax_Pixel;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_XAxis.FromAxisMaxToEffectiveMax_Pixel)
                {
                    Control_Graph.Graph_XAxis.FromAxisMaxToEffectiveMax_Pixel = value;

                    //

                    Control_Graph.Graph_XAxis._SetAxisData();//配置X坐标轴
                    Control_Graph.Graph_YAxis._SetAxisData();//配置Y坐标轴
                    Control_Graph._SetCurveValue();//配置曲线图

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：FromYAxisMaxToEffectiveMax属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("坐标轴最大有效实际数值对应的坐标点像素值与坐标轴最大值对应的坐标点像素值之间的距离像素值"), Category("Tolerances Y坐标轴数据")]
        public int FromYAxisMaxToEffectiveMax //属性
        {
            get//读取
            {
                return Control_Graph.Graph_YAxis.FromAxisMaxToEffectiveMax_Pixel;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_YAxis.FromAxisMaxToEffectiveMax_Pixel)
                {
                    Control_Graph.Graph_YAxis.FromAxisMaxToEffectiveMax_Pixel = value;

                    //

                    Control_Graph.Graph_XAxis._SetAxisData();//配置X坐标轴
                    Control_Graph.Graph_YAxis._SetAxisData();//配置Y坐标轴
                    Control_Graph._SetCurveValue();//配置曲线图

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：XAxisFontValue属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("绘制曲线图中的数值所使用的字体"), Category("Tolerances X坐标轴数据")]
        public Font XAxisFontValue //属性
        {
            get//读取
            {
                return Control_Graph.Graph_XAxis.tolerancesDrawing_Axis.FontValue;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_XAxis.tolerancesDrawing_Axis.FontValue)
                {
                    Control_Graph.Graph_XAxis.tolerancesDrawing_Axis.FontValue = value;

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：YAxisFontValue属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("绘制曲线图中的数值所使用的字体"), Category("Tolerances Y坐标轴数据")]
        public Font YAxisFontValue //属性
        {
            get//读取
            {
                return Control_Graph.Graph_YAxis.tolerancesDrawing_Axis.FontValue;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_YAxis.tolerancesDrawing_Axis.FontValue)
                {
                    Control_Graph.Graph_YAxis.tolerancesDrawing_Axis.FontValue = value;

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：XAxisDrawAxisWidth属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("绘制曲线坐标轴所使用的画笔的宽度"), Category("Tolerances X坐标轴数据")]
        public float XAxisDrawAxisWidth //属性
        {
            get//读取
            {
                return Control_Graph.Graph_XAxis.tolerancesDrawing_Axis.DrawAxisWidth;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_XAxis.tolerancesDrawing_Axis.DrawAxisWidth)
                {
                    Control_Graph.Graph_XAxis.tolerancesDrawing_Axis.DrawAxisWidth = value;

                    //

                    Control_Graph._SetAxisColor();

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：YAxisDrawAxisWidth属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("绘制曲线坐标轴所使用的画笔的宽度"), Category("Tolerances Y坐标轴数据")]
        public float YAxisDrawAxisWidth //属性
        {
            get//读取
            {
                return Control_Graph.Graph_YAxis.tolerancesDrawing_Axis.DrawAxisWidth;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_YAxis.tolerancesDrawing_Axis.DrawAxisWidth)
                {
                    Control_Graph.Graph_YAxis.tolerancesDrawing_Axis.DrawAxisWidth = value;

                    //
                     
                    Control_Graph._SetAxisColor();

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：XAxisDrawAxisEnable属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("曲线坐标轴使能时所使用的颜色"), Category("Tolerances X坐标轴数据")]
        public Color XAxisDrawAxisEnable //属性
        {
            get//读取
            {
                return Control_Graph.Graph_XAxis.tolerancesDrawing_Axis.ColorDrawAxisEnable;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_XAxis.tolerancesDrawing_Axis.ColorDrawAxisEnable)
                {
                    Control_Graph.Graph_XAxis.tolerancesDrawing_Axis.ColorDrawAxisEnable = value;

                    //

                    Control_Graph._SetAxisColor();

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：YAxisDrawAxisEnable属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("曲线坐标轴使能时所使用的颜色"), Category("Tolerances Y坐标轴数据")]
        public Color YAxisDrawAxisEnable //属性
        {
            get//读取
            {
                return Control_Graph.Graph_YAxis.tolerancesDrawing_Axis.ColorDrawAxisEnable;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_YAxis.tolerancesDrawing_Axis.ColorDrawAxisEnable)
                {
                    Control_Graph.Graph_YAxis.tolerancesDrawing_Axis.ColorDrawAxisEnable = value;

                    //

                    Control_Graph._SetAxisColor();

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：XAxisDrawAxisDisable属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("曲线坐标轴禁止时所使用的颜色"), Category("Tolerances X坐标轴数据")]
        public Color XAxisDrawAxisDisable //属性
        {
            get//读取
            {
                return Control_Graph.Graph_XAxis.tolerancesDrawing_Axis.ColorDrawAxisDisable;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_XAxis.tolerancesDrawing_Axis.ColorDrawAxisDisable)
                {
                    Control_Graph.Graph_XAxis.tolerancesDrawing_Axis.ColorDrawAxisDisable = value;

                    //

                    Control_Graph._SetAxisColor();

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：YAxisDrawAxisDisable属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("曲线坐标轴禁止时所使用的颜色"), Category("Tolerances Y坐标轴数据")]
        public Color YAxisDrawAxisDisable //属性
        {
            get//读取
            {
                return Control_Graph.Graph_YAxis.tolerancesDrawing_Axis.ColorDrawAxisDisable;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_YAxis.tolerancesDrawing_Axis.ColorDrawAxisDisable)
                {
                    Control_Graph.Graph_YAxis.tolerancesDrawing_Axis.ColorDrawAxisDisable = value;

                    //

                    Control_Graph._SetAxisColor();

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：XAxisDrawAxisValueEnable属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("曲线坐标轴数值使能时所使用的颜色"), Category("Tolerances X坐标轴数据")]
        public Color XAxisDrawAxisValueEnable //属性
        {
            get//读取
            {
                return Control_Graph.Graph_XAxis.tolerancesDrawing_Axis.ColorDrawAxisValueEnable;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_XAxis.tolerancesDrawing_Axis.ColorDrawAxisValueEnable)
                {
                    Control_Graph.Graph_XAxis.tolerancesDrawing_Axis.ColorDrawAxisValueEnable = value;

                    //

                    Control_Graph._SetAxisColor();

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：YAxisDrawAxisValueEnable属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("曲线坐标轴数值使能时所使用的颜色"), Category("Tolerances Y坐标轴数据")]
        public Color YAxisDrawAxisValueEnable //属性
        {
            get//读取
            {
                return Control_Graph.Graph_YAxis.tolerancesDrawing_Axis.ColorDrawAxisValueEnable;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_YAxis.tolerancesDrawing_Axis.ColorDrawAxisValueEnable)
                {
                    Control_Graph.Graph_YAxis.tolerancesDrawing_Axis.ColorDrawAxisValueEnable = value;

                    //

                    Control_Graph._SetAxisColor();

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：XAxisDrawAxisValueDisable属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("曲线坐标轴数值禁止时所使用的颜色"), Category("Tolerances X坐标轴数据")]
        public Color XAxisDrawAxisValueDisable //属性
        {
            get//读取
            {
                return Control_Graph.Graph_XAxis.tolerancesDrawing_Axis.ColorDrawAxisValueDisable;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_XAxis.tolerancesDrawing_Axis.ColorDrawAxisValueDisable)
                {
                    Control_Graph.Graph_XAxis.tolerancesDrawing_Axis.ColorDrawAxisValueDisable = value;

                    //

                    Control_Graph._SetAxisColor();

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：YAxisDrawAxisValueDisable属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("曲线坐标轴数值使能禁止时所使用的颜色"), Category("Tolerances Y坐标轴数据")]
        public Color YAxisDrawAxisValueDisable //属性
        {
            get//读取
            {
                return Control_Graph.Graph_YAxis.tolerancesDrawing_Axis.ColorDrawAxisValueDisable;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_YAxis.tolerancesDrawing_Axis.ColorDrawAxisValueDisable)
                {
                    Control_Graph.Graph_YAxis.tolerancesDrawing_Axis.ColorDrawAxisValueDisable = value;

                    //

                    Control_Graph._SetAxisColor();

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：XAxisDrawEffectiveLineWidth属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("绘制坐标轴有效值区域分界线所使用的画笔的宽度"), Category("Tolerances X坐标轴数据")]
        public float XAxisDrawEffectiveLineWidth //属性
        {
            get//读取
            {
                return Control_Graph.Graph_XAxis.tolerancesDrawing_Axis.DrawEffectiveLineWidth;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_XAxis.tolerancesDrawing_Axis.DrawEffectiveLineWidth)
                {
                    Control_Graph.Graph_XAxis.tolerancesDrawing_Axis.DrawEffectiveLineWidth = value;

                    //

                    Control_Graph._SetAxisColor();

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：YAxisDrawEffectiveLineWidth属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("绘制坐标轴有效值区域分界线所使用的画笔的宽度"), Category("Tolerances Y坐标轴数据")]
        public float YAxisDrawEffectiveLineWidth //属性
        {
            get//读取
            {
                return Control_Graph.Graph_YAxis.tolerancesDrawing_Axis.DrawEffectiveLineWidth;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_YAxis.tolerancesDrawing_Axis.DrawEffectiveLineWidth)
                {
                    Control_Graph.Graph_YAxis.tolerancesDrawing_Axis.DrawEffectiveLineWidth = value;

                    //

                    Control_Graph._SetAxisColor();

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：XAxisDrawEffectiveMinLineEnable属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("坐标轴最小有效值区域分界线使能时所使用的颜色"), Category("Tolerances X坐标轴数据")]
        public Color XAxisDrawEffectiveMinLineEnable //属性
        {
            get//读取
            {
                return Control_Graph.Graph_XAxis.tolerancesDrawing_Axis.ColorDrawEffectiveMinLineEnable;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_XAxis.tolerancesDrawing_Axis.ColorDrawEffectiveMinLineEnable)
                {
                    Control_Graph.Graph_XAxis.tolerancesDrawing_Axis.ColorDrawEffectiveMinLineEnable = value;

                    //

                    Control_Graph._SetAxisColor();

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：XAxisDrawEffectiveMaxLineEnable属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("坐标轴最大有效值区域分界线使能时所使用的颜色"), Category("Tolerances X坐标轴数据")]
        public Color XAxisDrawEffectiveMaxLineEnable //属性
        {
            get//读取
            {
                return Control_Graph.Graph_XAxis.tolerancesDrawing_Axis.ColorDrawEffectiveMaxLineEnable;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_XAxis.tolerancesDrawing_Axis.ColorDrawEffectiveMaxLineEnable)
                {
                    Control_Graph.Graph_XAxis.tolerancesDrawing_Axis.ColorDrawEffectiveMaxLineEnable = value;

                    //

                    Control_Graph._SetAxisColor();

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：YAxisDrawEffectiveMinLineEnable属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("坐标轴最小有效值区域分界线使能时所使用的颜色"), Category("Tolerances Y坐标轴数据")]
        public Color YAxisDrawEffectiveMinLineEnable //属性
        {
            get//读取
            {
                return Control_Graph.Graph_YAxis.tolerancesDrawing_Axis.ColorDrawEffectiveMinLineEnable;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_YAxis.tolerancesDrawing_Axis.ColorDrawEffectiveMinLineEnable)
                {
                    Control_Graph.Graph_YAxis.tolerancesDrawing_Axis.ColorDrawEffectiveMinLineEnable = value;

                    //

                    Control_Graph._SetAxisColor();

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：YAxisDrawEffectiveMaxLineEnable属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("坐标轴最大有效值区域分界线使能时所使用的颜色"), Category("Tolerances Y坐标轴数据")]
        public Color YAxisDrawEffectiveMaxLineEnable //属性
        {
            get//读取
            {
                return Control_Graph.Graph_YAxis.tolerancesDrawing_Axis.ColorDrawEffectiveMaxLineEnable;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_YAxis.tolerancesDrawing_Axis.ColorDrawEffectiveMaxLineEnable)
                {
                    Control_Graph.Graph_YAxis.tolerancesDrawing_Axis.ColorDrawEffectiveMaxLineEnable = value;

                    //

                    Control_Graph._SetAxisColor();

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：XAxisDrawEffectiveLineDisable属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("坐标轴有效值区域分界线禁止时所使用的颜色"), Category("Tolerances X坐标轴数据")]
        public Color XAxisDrawEffectiveLineDisable //属性
        {
            get//读取
            {
                return Control_Graph.Graph_XAxis.tolerancesDrawing_Axis.ColorDrawEffectiveLineDisable;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_XAxis.tolerancesDrawing_Axis.ColorDrawEffectiveLineDisable)
                {
                    Control_Graph.Graph_XAxis.tolerancesDrawing_Axis.ColorDrawEffectiveLineDisable = value;

                    //

                    Control_Graph._SetAxisColor();

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：YAxisDrawEffectiveLineDisable属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("坐标轴有效值区域分界线禁止时所使用的颜色"), Category("Tolerances Y坐标轴数据")]
        public Color YAxisDrawEffectiveLineDisable //属性
        {
            get//读取
            {
                return Control_Graph.Graph_YAxis.tolerancesDrawing_Axis.ColorDrawEffectiveLineDisable;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_YAxis.tolerancesDrawing_Axis.ColorDrawEffectiveLineDisable)
                {
                    Control_Graph.Graph_YAxis.tolerancesDrawing_Axis.ColorDrawEffectiveLineDisable = value;

                    //

                    Control_Graph._SetAxisColor();

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：XAxisDrawEffectiveMinValueEnable属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("坐标轴最小有效值使能时所使用的颜色"), Category("Tolerances X坐标轴数据")]
        public Color XAxisDrawEffectiveMinValueEnable //属性
        {
            get//读取
            {
                return Control_Graph.Graph_XAxis.tolerancesDrawing_Axis.ColorDrawEffectiveMinValueEnable;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_XAxis.tolerancesDrawing_Axis.ColorDrawEffectiveMinValueEnable)
                {
                    Control_Graph.Graph_XAxis.tolerancesDrawing_Axis.ColorDrawEffectiveMinValueEnable = value;

                    //

                    Control_Graph._SetAxisColor();

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：XAxisDrawEffectiveMaxValueEnable属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("坐标轴最大有效值使能时所使用的颜色"), Category("Tolerances X坐标轴数据")]
        public Color XAxisDrawEffectiveMaxValueEnable //属性
        {
            get//读取
            {
                return Control_Graph.Graph_XAxis.tolerancesDrawing_Axis.ColorDrawEffectiveMaxValueEnable;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_XAxis.tolerancesDrawing_Axis.ColorDrawEffectiveMaxValueEnable)
                {
                    Control_Graph.Graph_XAxis.tolerancesDrawing_Axis.ColorDrawEffectiveMaxValueEnable = value;

                    //

                    Control_Graph._SetAxisColor();

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：YAxisDrawEffectiveMinValueEnable属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("坐标轴最小有效值使能时所使用的颜色"), Category("Tolerances Y坐标轴数据")]
        public Color YAxisDrawEffectiveMinValueEnable //属性
        {
            get//读取
            {
                return Control_Graph.Graph_YAxis.tolerancesDrawing_Axis.ColorDrawEffectiveMinValueEnable;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_YAxis.tolerancesDrawing_Axis.ColorDrawEffectiveMinValueEnable)
                {
                    Control_Graph.Graph_YAxis.tolerancesDrawing_Axis.ColorDrawEffectiveMinValueEnable = value;

                    //

                    Control_Graph._SetAxisColor();

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：YAxisDrawEffectiveMaxValueEnable属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("坐标轴最大有效值使能时所使用的颜色"), Category("Tolerances Y坐标轴数据")]
        public Color YAxisDrawEffectiveMaxValueEnable //属性
        {
            get//读取
            {
                return Control_Graph.Graph_YAxis.tolerancesDrawing_Axis.ColorDrawEffectiveMaxValueEnable;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_YAxis.tolerancesDrawing_Axis.ColorDrawEffectiveMaxValueEnable)
                {
                    Control_Graph.Graph_YAxis.tolerancesDrawing_Axis.ColorDrawEffectiveMaxValueEnable = value;

                    //

                    Control_Graph._SetAxisColor();

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：XAxisDrawEffectiveValueDisable属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("坐标轴有效值禁止时所使用的颜色"), Category("Tolerances X坐标轴数据")]
        public Color XAxisDrawEffectiveValueDisable //属性
        {
            get//读取
            {
                return Control_Graph.Graph_XAxis.tolerancesDrawing_Axis.ColorDrawEffectiveValueDisable;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_XAxis.tolerancesDrawing_Axis.ColorDrawEffectiveValueDisable)
                {
                    Control_Graph.Graph_XAxis.tolerancesDrawing_Axis.ColorDrawEffectiveValueDisable = value;

                    //

                    Control_Graph._SetAxisColor();

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：YAxisDrawEffectiveValueDisable属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("坐标轴有效值禁止时所使用的颜色"), Category("Tolerances Y坐标轴数据")]
        public Color YAxisDrawEffectiveValueDisable //属性
        {
            get//读取
            {
                return Control_Graph.Graph_YAxis.tolerancesDrawing_Axis.ColorDrawEffectiveValueDisable;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_YAxis.tolerancesDrawing_Axis.ColorDrawEffectiveValueDisable)
                {
                    Control_Graph.Graph_YAxis.tolerancesDrawing_Axis.ColorDrawEffectiveValueDisable = value;

                    //

                    Control_Graph._SetAxisColor();

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：XAxisDrawAdditionalValueLineWidth属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("绘制曲线图坐标轴上显示的附加数值指示线所使用的画笔的宽度"), Category("Tolerances X坐标轴数据")]
        public float XAxisDrawAdditionalValueLineWidth //属性
        {
            get//读取
            {
                return Control_Graph.Graph_XAxis.tolerancesDrawing_Axis.DrawAdditionalValueLineWidth;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_XAxis.tolerancesDrawing_Axis.DrawAdditionalValueLineWidth)
                {
                    Control_Graph.Graph_XAxis.tolerancesDrawing_Axis.DrawAdditionalValueLineWidth = value;

                    //

                    Control_Graph._SetAxisColor();

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：YAxisDrawAdditionalValueLineWidth属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("绘制曲线图坐标轴上显示的附加数值指示线所使用的画笔的宽度"), Category("Tolerances Y坐标轴数据")]
        public float YAxisDrawAdditionalValueLineWidth //属性
        {
            get//读取
            {
                return Control_Graph.Graph_YAxis.tolerancesDrawing_Axis.DrawAdditionalValueLineWidth;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_YAxis.tolerancesDrawing_Axis.DrawAdditionalValueLineWidth)
                {
                    Control_Graph.Graph_YAxis.tolerancesDrawing_Axis.DrawAdditionalValueLineWidth = value;

                    //

                    Control_Graph._SetAxisColor();

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：XAxisDrawAdditionalValueLineEnable属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("曲线图坐标轴上显示的附加数值指示线使能时所使用的颜色"), Category("Tolerances X坐标轴数据")]
        public Color XAxisDrawAdditionalValueLineEnable //属性
        {
            get//读取
            {
                return Control_Graph.Graph_XAxis.tolerancesDrawing_Axis.ColorDrawAdditionalValueLineEnable;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_XAxis.tolerancesDrawing_Axis.ColorDrawAdditionalValueLineEnable)
                {
                    Control_Graph.Graph_XAxis.tolerancesDrawing_Axis.ColorDrawAdditionalValueLineEnable = value;

                    //

                    Control_Graph._SetAxisColor();

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：YAxisDrawAdditionalValueLineEnable属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("曲线图坐标轴上显示的附加数值指示线使能时所使用的颜色"), Category("Tolerances Y坐标轴数据")]
        public Color YAxisDrawAdditionalValueLineEnable //属性
        {
            get//读取
            {
                return Control_Graph.Graph_YAxis.tolerancesDrawing_Axis.ColorDrawAdditionalValueLineEnable;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_YAxis.tolerancesDrawing_Axis.ColorDrawAdditionalValueLineEnable)
                {
                    Control_Graph.Graph_YAxis.tolerancesDrawing_Axis.ColorDrawAdditionalValueLineEnable = value;

                    //

                    Control_Graph._SetAxisColor();

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：XAxisDrawAdditionalValueLineDisable属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("曲线图坐标轴上显示的附加数值指示线禁止时所使用的颜色"), Category("Tolerances X坐标轴数据")]
        public Color XAxisDrawAdditionalValueLineDisable //属性
        {
            get//读取
            {
                return Control_Graph.Graph_XAxis.tolerancesDrawing_Axis.ColorDrawAdditionalValueLineDisable;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_XAxis.tolerancesDrawing_Axis.ColorDrawAdditionalValueLineDisable)
                {
                    Control_Graph.Graph_XAxis.tolerancesDrawing_Axis.ColorDrawAdditionalValueLineDisable = value;

                    //

                    Control_Graph._SetAxisColor();

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：YAxisDrawAdditionalValueLineDisable属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("曲线图坐标轴上显示的附加数值指示线禁止时所使用的颜色"), Category("Tolerances Y坐标轴数据")]
        public Color YAxisDrawAdditionalValueLineDisable //属性
        {
            get//读取
            {
                return Control_Graph.Graph_YAxis.tolerancesDrawing_Axis.ColorDrawAdditionalValueLineDisable;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_YAxis.tolerancesDrawing_Axis.ColorDrawAdditionalValueLineDisable)
                {
                    Control_Graph.Graph_YAxis.tolerancesDrawing_Axis.ColorDrawAdditionalValueLineDisable = value;

                    //

                    Control_Graph._SetAxisColor();

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：XAxisDrawAdditionalValueEnable属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("曲线图坐标轴上显示的附加数值使能时所使用的颜色"), Category("Tolerances X坐标轴数据")]
        public Color XAxisDrawAdditionalValueEnable //属性
        {
            get//读取
            {
                return Control_Graph.Graph_XAxis.tolerancesDrawing_Axis.ColorDrawAdditionalValueEnable;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_XAxis.tolerancesDrawing_Axis.ColorDrawAdditionalValueEnable)
                {
                    Control_Graph.Graph_XAxis.tolerancesDrawing_Axis.ColorDrawAdditionalValueEnable = value;

                    //

                    Control_Graph._SetAxisColor();

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：YAxisDrawAdditionalValueEnable属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("曲线图坐标轴上显示的附加数值使能时所使用的颜色"), Category("Tolerances Y坐标轴数据")]
        public Color YAxisDrawAdditionalValueEnable //属性
        {
            get//读取
            {
                return Control_Graph.Graph_YAxis.tolerancesDrawing_Axis.ColorDrawAdditionalValueEnable;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_YAxis.tolerancesDrawing_Axis.ColorDrawAdditionalValueEnable)
                {
                    Control_Graph.Graph_YAxis.tolerancesDrawing_Axis.ColorDrawAdditionalValueEnable = value;

                    //

                    Control_Graph._SetAxisColor();

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：XAxisDrawAdditionalValueDisable属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("曲线图坐标轴上显示的附加数值禁止时所使用的颜色"), Category("Tolerances X坐标轴数据")]
        public Color XAxisDrawAdditionalValueDisable //属性
        {
            get//读取
            {
                return Control_Graph.Graph_XAxis.tolerancesDrawing_Axis.ColorDrawAdditionalValueDisable;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_XAxis.tolerancesDrawing_Axis.ColorDrawAdditionalValueDisable)
                {
                    Control_Graph.Graph_XAxis.tolerancesDrawing_Axis.ColorDrawAdditionalValueDisable = value;

                    //

                    Control_Graph._SetAxisColor();

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：YAxisDrawAdditionalValueDisable属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("曲线图坐标轴上显示的附加数值禁止时所使用的颜色"), Category("Tolerances Y坐标轴数据")]
        public Color YAxisDrawAdditionalValueDisable //属性
        {
            get//读取
            {
                return Control_Graph.Graph_YAxis.tolerancesDrawing_Axis.ColorDrawAdditionalValueDisable;
            }
            set//设置
            {
                if (value != Control_Graph.Graph_YAxis.tolerancesDrawing_Axis.ColorDrawAdditionalValueDisable)
                {
                    Control_Graph.Graph_YAxis.tolerancesDrawing_Axis.ColorDrawAdditionalValueDisable = value;

                    //

                    Control_Graph._SetAxisColor();

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：XAxisCurValueDisplay 属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("曲线图X坐标轴上是否显示当前数值。true：是；false：否"), Category("Tolerances 曲线图数据")]
        public bool XAxisCurValueDisplay //属性
        {
            get//读取
            {
                return Control_Graph.XAxisCurValueDisplay;
            }
            set//设置
            {
                if (value != Control_Graph.XAxisCurValueDisplay)
                {
                    Control_Graph.XAxisCurValueDisplay = value;

                    //

                    Control_Graph._SetCurrentValue();//配置当前值

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：YAxisCurValueDisplay 属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("曲线图Y坐标轴上是否显示当前数值。true：是；false：否"), Category("Tolerances 曲线图数据")]
        public bool YAxisCurValueDisplay //属性
        {
            get//读取
            {
                return Control_Graph.YAxisCurValueDisplay;
            }
            set//设置
            {
                if (value != Control_Graph.YAxisCurValueDisplay)
                {
                    Control_Graph.YAxisCurValueDisplay = value;

                    //

                    Control_Graph._SetCurrentValue();//配置当前值

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ValueNumber_Curve属性的实现（设置完成后将为Value重新分配空间）
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("绘制区域中的曲线值数目（坐标点数目）"), Category("Tolerances 曲线图数据")]
        public int ValueNumber_Curve//属性
        {
            get//读取
            {
                return Control_Graph.ValueNumber_Curve;
            }
            set//设置
            {
                if (value != Control_Graph.ValueNumber_Curve)
                {
                    Int32 i = 0;//循环控制变量

                    for (i = 0; i < Control_Graph.CurrentValueIndex.Length; i++)
                    {
                        if (Control_Graph.CurrentValueIndex[i] < value)//有效
                        {
                            //不执行操作
                        }
                        else//
                        {
                            break;
                        }
                    }
                    if (i >= Control_Graph.CurrentValueIndex.Length)
                    {
                        Control_Graph.ValueNumber_Curve = value;

                        //

                        Control_Graph.Graph_XAxis._SetAxisData();//配置X坐标轴
                        Control_Graph.Graph_YAxis._SetAxisData();//配置Y坐标轴
                        Control_Graph._SetCurveValue();//配置曲线图
                        Control_Graph._SetCurrentValue();//配置当前值

                        //

                        _GetBackgroundImage();
                        _Draw();
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：CurveNumber_Curve属性的实现（设置完成后将为Value重新分配空间）
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("绘制区域中的曲线数目"), Category("Tolerances 曲线图数据")]
        public int CurveNumber_Curve//属性
        {
            get//读取
            {
                return Control_Graph.CurveNumber_Curve;
            }
            set//设置
            {
                if (value != Control_Graph.CurveNumber_Curve)
                {
                    Control_Graph.CurveNumber_Curve = value;

                    //

                    Control_Graph.Graph_XAxis._SetAxisData();//配置X坐标轴
                    Control_Graph.Graph_YAxis._SetAxisData();//配置Y坐标轴
                    Control_Graph._SetCurveValue();//配置曲线图
                    Control_Graph._SetCurrentValue();//配置当前值

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }
        
        //----------------------------------------------------------------------
        // 功能说明：CurrentValueIndex属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("绘制区域中当前显示的曲线值点（从0开始，小于0表示曲线图数值无效）"), Category("Tolerances 曲线图数据")]
        public int[] CurrentValueIndex//属性
        {
            get//读取
            {
                return Control_Graph.CurrentValueIndex;
            }
            set//设置
            {
                Int32 i = 0;//循环控制变量

                for (i = 0; i < value.Length; i++)
                {
                    if (value[i] < Control_Graph.ValueNumber_Curve)
                    {
                        //不执行操作
                    }
                    else//
                    {
                        break;
                    }
                }
                if (i >= Control_Graph.ValueNumber_Curve)//有效
                {
                    Control_Graph.CurrentValueIndex = new int[value.Length];
                    value.CopyTo(Control_Graph.CurrentValueIndex, 0);

                    //

                    Control_Graph._SetCurrentValue();//配置当前值

                    //

                    _GetBackgroundImage();
                    _Draw();
                }
            }
        }

        //函数

        //----------------------------------------------------------------------
        // 功能说明：配置曲线图控件
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetControl()
        {
            if (Control_Graph.Control_Data.RunorStop)//运行
            {
                if (Control_Graph.Control_Data.ControlSelected)//控件处于选中状态
                {
                    customButtonRunStop.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;

                    //此时【学习】按钮一定处于使能状态

                    customButtonLearning.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                }
                else//控件未选中
                {                    
                    customButtonRunStop.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;

                    //此时【学习】按钮一定处于使能状态

                    customButtonLearning.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                }
            }
            else//停止
            {
                customButtonRunStop.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;

                //此时【学习】按钮一定处于禁止状态

                customButtonLearning.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
            }

            if (Control_Graph.Control_Data.ButtonRunStopShow)//显示
            {
                customButtonRunStop.Visible = true;
            }
            else//隐藏
            {
                customButtonRunStop.Visible = false;
            }

            if (Control_Graph.Control_Data.ButtonLearningShow)//显示
            {
                customButtonLearning.Visible = true;
            }
            else//隐藏
            {
                customButtonLearning.Visible = false;
            }

            //

            Control_Graph._SetControlBackgroundColor();

            //

            _GetBackgroundImage();
            _Draw();
        }

        //----------------------------------------------------------------------
        // 功能说明：（通过参数修改属性值时），使用设置完成的属性设置曲线图控件
        // 输入参数：1.Control_GraphData：图形
        // 输出参数：无
        // 返回值：true：属性设置正确；false：属性设置错误
        //----------------------------------------------------------------------
        public void _Set(TolerancesControl_GraphData Control_GraphData)
        {
            Control_Graph = Control_GraphData.TolerancesGraph;

            _SetControl();//配置曲线图控件

            //

            Invalidate(ClientRectangle);
            Update();
        }

        //----------------------------------------------------------------------
        // 功能说明：设置标题（工具名称）
        // 输入参数：1.Control_GraphData：图形
        // 输出参数：无
        // 返回值：true：属性设置正确；false：属性设置错误
        //----------------------------------------------------------------------
        public void _SetCaption(TolerancesControl_GraphData Control_GraphData)
        {
            Control_Graph.Caption = Control_GraphData.TolerancesGraph.Caption;

            Control_Graph.Control_Data.TolerancesTools.ToolsName = Control_GraphData.TolerancesGraph.Control_Data.TolerancesTools.ToolsName;

            //

            Invalidate(ClientRectangle);
            Update();
        }

        //----------------------------------------------------------------------
        // 功能说明：更改曲线图Y轴的有效值时，配置Y轴坐标和曲线图，并更新绘制区域
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _SetValue()
        {
            Control_Graph.Graph_YAxis._SetAxisData();//配置Y坐标轴
            Control_Graph._SetCurveValue();//配置曲线图

            //

            //Invalidate(ClientRectangle);
            //Update();
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：刷新背景图像
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _Refresh()
        {
            _GetBackgroundImage();
            _Draw();
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：获取控件图像
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _GetImage()
        {
            _GetBackgroundImage(false);
        }

        //----------------------------------------------------------------------
        // 功能说明：释放控件图像
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _ReleaseImage()
        {
            if (null != imageControl)
            {
                imageControl.Dispose();
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：获取背景图像
        // 输入参数：1..bDraw：是否绘制控件。取值范围：true，是（用于控件内部调用）；false，否（用于控件外部获取控件背景图像时使用）
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _GetBackgroundImage(Boolean bDraw = true)
        {
            Image imageDisplay = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);
            Graphics graphicsDraw = Graphics.FromImage(imageDisplay);

            SolidBrush solidbrushDraw = new SolidBrush(BackColor);

            //绘制背景色

            graphicsDraw.FillRectangle(solidbrushDraw, ClientRectangle);//绘制当前选择的相机控件区域

            //绘制曲线

            Control_Graph._Draw(graphicsDraw, ClientRectangle);

            //绘制按钮

            if (null != customButtonRunStop.ImageControl)//有效
            {
                if (customButtonRunStop.Visible)
                {
                    customButtonRunStop._GetImage();//获取

                    graphicsDraw.DrawImage(customButtonRunStop.ImageControl, new Rectangle(customButtonRunStop.Location, customButtonRunStop.Size));

                    customButtonRunStop._ReleaseImage();//释放
                }
            }

            if (null != customButtonLearning.ImageControl)//有效
            {
                if (customButtonLearning.Visible)
                {
                    customButtonLearning._GetImage();//获取

                    graphicsDraw.DrawImage(customButtonLearning.ImageControl, new Rectangle(customButtonLearning.Location, customButtonLearning.Size));

                    customButtonLearning._ReleaseImage();//释放
                }
            }

            //

            if (bDraw)//绘制
            {
                if (null != imageToDraw)//释放资源
                {
                    imageToDraw.Dispose();
                }

                imageToDraw = (Image)imageDisplay.Clone();
            } 
            else//获取
            {
                if (null != imageControl)//释放资源
                {
                    imageControl.Dispose();
                }

                imageControl = (Image)imageDisplay.Clone();
            }

            //

            graphicsDraw.Dispose();
            imageDisplay.Dispose();
        }

        //----------------------------------------------------------------------
        // 功能说明：绘制控件
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _Draw()
        {
            //使用双倍缓冲绘图

            Image imageDisplay = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);
            Graphics graphicsDraw = Graphics.FromImage(imageDisplay);

            SolidBrush solidbrushDraw = new SolidBrush(BackColor);

            //绘制背景色

            graphicsDraw.FillRectangle(solidbrushDraw, ClientRectangle);//绘制当前选择的相机控件区域

            //绘制背景图像

            if (null != imageToDraw)//有效
            {
                graphicsDraw.DrawImage(imageToDraw, ClientRectangle);
            }

            //

            Control_Graphics.DrawImage(imageDisplay, ClientRectangle);

            //

            graphicsDraw.Dispose();
            imageDisplay.Dispose();
        }

        //事件

        //----------------------------------------------------------------------
        // 功能说明：点击控件事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void Tolerances_Click(object sender, EventArgs e)
        {
            if (null != Control_Click)//有效
            {
                Control_Click(this, new CustomEventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：双击控件事件，选择控件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void Tolerances_DoubleClick(object sender, EventArgs e)
        {
            if (!bCurveMode)//非曲线图模式
            {
                if (Control_Graph.Control_Data.RunorStop)//运行
                {
                    CustomEventArgs customEventArgs = new CustomEventArgs();//参数
                    //1.IntValue[0]：取值为0，表示该控件在双击时已经被选中；取值为1，表示该控件在双击时未被选中

                    if (Control_Graph.Control_Data.ControlSelected)//控件处于选中状态
                    {
                        customEventArgs.IntValue[0] = 0;
                    }
                    else//控件未选中
                    {
                        ControlSelected = true;//选中

                        //

                        customEventArgs.IntValue[0] = 1;
                    }

                    //

                    if (null != Control_DoubleClick)//有效
                    {
                        Control_DoubleClick(this, customEventArgs);//双击曲线图控件时产生的事件
                    }
                }
                else//停止
                {
                    //不执行操作
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【运行/停止】按钮事件，运行或停止
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonRunStop_CustomButton_Click(object sender, EventArgs e)
        {
            RunorStop = !RunorStop;//更改属性

            //

            if (null != RunStop_Click)//有效
            {
                RunStop_Click(this, e);//点击【运行/停止】按钮时产生的事件
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【学习】按钮事件，学习
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonLearning_CustomButton_Click(object sender, EventArgs e)
        {
            if (null != Learning_Click)//有效
            {
                Learning_Click(this, e);//点击【学习】按钮时产生的事件
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：绘制曲线图和标题文本
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void Tolerances_Paint(object sender, PaintEventArgs e)
        {
            _Draw();
        }
    }

    public class Tolerances_Graph
    {
        private Color colorControlSelected = Color.FromArgb(50, 50, 50);//属性，选中控件时的背景颜色
        private Color colorControlUnselected = Color.FromArgb(100, 100, 100);//属性，未选中控件时的背景颜色
        private SolidBrush solidbrushControl = new SolidBrush(Color.FromArgb(100, 100, 100));//属性（颜色），控件的背景画刷

        //

        private Boolean bCurveMode = false;//属性，是否为曲线图模式。取值范围：true，是；false：否

        public Tolerances_Data Control_Data = new Tolerances_Data();

        public Tolerances_Graph_Axis Graph_XAxis;//X坐标轴
        public Tolerances_Graph_Axis Graph_YAxis;//Y坐标轴

        public Graphics Graph_Graphics;//用于获取绘制当前值的区域范围

        private string sCaption = "EXAMPLE";//属性，控件标题文本
        private Rectangle rectCaption = new Rectangle(new Point(7, 0), new Size(545, 20));//属性，标题绘制区域
        private Rectangle rectGraph = new Rectangle(new Point(9, 22), new Size(541, 117));//属性，控件绘制区域（曲线图）
        private Font fontCaption = new Font("微软雅黑", 10.5F, FontStyle.Bold, GraphicsUnit.Point, Convert.ToByte(134));//属性，绘制标题文本所使用的字体
        private StringFormat stringformatDrawCaption = new StringFormat();//绘制标题文本所使用的格式

        private Color colorDrawCaptionEnable = Color.FromArgb(255, 255, 255);//属性，标题文本有效时的颜色
        private Color colorDrawCaptionDisable = Color.FromArgb(172, 168, 153);//属性，标题文本无效时的颜色
        private SolidBrush solidbrushDrawCaption = new SolidBrush(Color.FromArgb(255, 255, 255));//绘制标题文本所使用的画刷

        private SolidBrush solidbrushDrawGraph = new SolidBrush(Color.FromArgb(30, 30, 30));//属性（颜色），绘制控件绘制区域（曲线图）所使用的画刷

        //

        private Boolean bGridDisplay = false;//属性，网格是否显示。取值范围：true，显示；false，不显示
        private Color colorGridLine = Color.FromArgb(255, 255, 255);//属性，网格线的颜色
        private float fGridLineWidth = 1F;//属性，网格线的宽度
        private Pen penGridLine = new Pen(Color.FromArgb(255, 255, 255));//画笔，绘制曲线连接线

        //曲线值

        private Single fIntervalValue = 1F;//绘制区域中的曲线值相邻坐标点在X轴上的间距

        private Int32 iValuePointPixelWidth = 7;//属性，绘制区域中的曲线值点像素宽度

        private Boolean bValuePointDisplay = true;//属性，曲线值点是否显示。取值范围：true，显示；false，不显示

        private Boolean[] bCurveDisplay = new Boolean[1];//属性，曲线是否显示。取值范围：true，显示；false，不显示

        private int iCurveNumber_Curve = 1;//属性，绘制区域中的曲线图数目
        private int iValueNumber_Curve = 100;//属性，绘制区域中的曲线值数目（坐标点数目，根据X轴数值获取）

        private Boolean bAutoXAxisValue = true;//属性，曲线值X轴数值是否自动。取值范围：true，是；false，否
        public Single[][] Value_X = new Single[1][];//绘制区域中的曲线值（X轴，下位机传送的实际值）
        public Single[][] Value_Y = new Single[1][];//绘制区域中的曲线值（Y轴，下位机传送的实际值）

        private int[] iCurrentValueIndex = new int[1];//属性，绘制区域中当前显示的曲线值点（从0开始，小于0表示曲线图数值无效）
        
        private PointF[][] pointValue = new PointF[1][];//绘制区域中的曲线值坐标
        private Point[][] pointPixelValue = new Point[1][];//绘制区域中的曲线值像素坐标

        private Color colorValueEnable = Color.FromArgb(0, 255, 0);//属性，曲线图数值有效时的颜色
        private Color colorValueDisable = Color.FromArgb(255, 0, 0);//属性，曲线图数值无效时的颜色
        private Color colorValueUndefined = Color.FromArgb(255, 255, 255);//属性，曲线图数值超出范围（未定义）时的颜色

        private SolidBrush[][] solidbrushValue = new SolidBrush[1][];//用于绘制曲线图点的画刷

        private int[][] iStyle = new int[1][];//属性，曲线值类型（只读）。取值：
                            //1.有效值（坐标轴指定有效值范围时，坐标轴范围内的值均算有效值）
                            //2.无效值（必须在指定有效值范围的时候才可能为该值）
                            //3.超出坐标轴范围

        private Color[] colorLine = new Color[1];//属性，曲线连接线的颜色
        private Single[] fLineWidth = new Single[1];//属性，曲线连接线的宽度
        private Pen[] penLine = new Pen[1];//画笔，绘制曲线连接线

        //X轴、Y轴当前值相互关联，即为当前值的坐标值。绘制当前值时，使用Y轴数据。因此：
        //1.两个同时无效，此时不显示当前值（同时曲线图也不显示）
        //2.X轴当前值无效，Y轴当前值有效（此时不显示X轴当前值，只显示Y轴当前值）

        private Boolean bXAxisCurValueDisplay = false;//属性，曲线图X坐标轴上是否显示当前数值。true：是；false：否
        private Boolean bYAxisCurValueDisplay = true;//属性，曲线图Y坐标轴上是否显示当前数值。true：是；false：否
        private String sCurValue = "";//绘制区域中的当前数值（有X轴、Y轴当前值组合而成）      
        private Rectangle rectPixel_CurValue = new Rectangle(new Point(496, 24), new Size(32, 11));//属性，绘制区域中的当前值（X轴、Y轴当前值在本区域显示）的像素区域
        private Rectangle rectPixel_CurValueIcon = new Rectangle(new Point(528, 24), new Size(11, 11));//属性，绘制区域中的当前值（X轴、Y轴当前值在本区域显示）的指示图标像素区域
        private SolidBrush solidbrushDrawCurValue = new SolidBrush(Color.FromArgb(255, 255, 255));//属性，绘制当前值所使用的画刷
        private SolidBrush solidbrushDrawCurValueIcon = new SolidBrush(Color.FromArgb(0, 255, 0));//属性，绘制当前值指示图标的画刷
        private Font fontCurValue = new Font("微软雅黑", 6F, FontStyle.Regular, GraphicsUnit.Point, Convert.ToByte(134));//属性，绘制当前值所使用的字体
        private StringFormat stringformatDrawCurValue = new StringFormat();//绘制当前值所使用的格式

        //构造函数

        //----------------------------------------------------------------------
        // 功能说明：构造函数，初始化
        // 输入参数：1.graphics：绘图
        //         2.bSet：是否需要配置坐标轴、曲线图和当前数值。true：是（创建控件时取该值）；false：否（通过参数修改控件属性值时取该值）
        //         3.size：控件区域
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Tolerances_Graph(Graphics graphics)
        {
            Int32 i = 0;//循环控制变量

            for (i = 0; i < iCurveNumber_Curve; i++)//初始化
            {
                bCurveDisplay[i] = true;//属性，曲线是否显示。取值范围：true，显示；false，不显示

                Value_X[i] = new Single[100];//属性，绘制区域中的曲线值（X轴，下位机传送的实际值）
                Value_Y[i] = new Single[100];//属性，绘制区域中的曲线值（Y轴，下位机传送的实际值）

                iCurrentValueIndex[i] = 99;//属性，绘制区域中当前显示的曲线值点（从0开始，小于0表示曲线图数值无效）
        
                pointValue[i] = new PointF[100];//绘制区域中的曲线值坐标
                pointPixelValue[i] = new Point[100];//绘制区域中的曲线值像素坐标

                solidbrushValue[i] = new SolidBrush[100];//用于绘制曲线图点的画刷

                iStyle[i] = new int[100];//属性，曲线值类型（只读）。取值：
                                    //1.有效值（坐标轴指定有效值范围时，坐标轴范围内的值均算有效值）
                                    //2.无效值（必须在指定有效值范围的时候才可能为该值）
                                    //3.超出坐标轴范围

                colorLine[i] = Color.FromArgb(255, 255, 255);//属性，曲线连接线的颜色
                fLineWidth[i] = 1F;//属性，曲线连接线的宽度
                penLine[i] = new Pen(Color.FromArgb(255, 255, 255));//画笔，绘制曲线连接线
            }

            //

            Graph_Graphics = graphics;

            //

            Graph_XAxis = new Tolerances_Graph_Axis(VisionSystemClassLibrary.Enum.AxisType.XAxis, graphics);//X坐标轴
            Graph_YAxis = new Tolerances_Graph_Axis(VisionSystemClassLibrary.Enum.AxisType.YAxis, graphics);//Y坐标轴

            //

            stringformatDrawCaption.Alignment = StringAlignment.Center;//设置格式
            stringformatDrawCaption.LineAlignment = StringAlignment.Center;//设置格式

            stringformatDrawCurValue.Alignment = StringAlignment.Center;//设置格式
            stringformatDrawCurValue.LineAlignment = StringAlignment.Center;//设置格式

            //

            _InitValue();//初始化曲线数值

            //

            _SetControlBackgroundColor();

            //

            _SetCurveValue();//配置曲线图
            _SetCurrentValue();//配置当前值数值
        }

        //属性

        //----------------------------------------------------------------------
        // 功能说明：CurveMode属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Boolean CurveMode//属性
        {
            get//读取
            {
                return bCurveMode;
            }
            set//设置
            {
                if (value != bCurveMode)//设置了新的数值
                {
                    bCurveMode = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ColorControlSelected属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Color ColorControlSelected//属性
        {
            get//读取
            {
                return colorControlSelected;
            }
            set//设置
            {
                if (value != colorControlSelected)//设置了新的数值
                {
                    colorControlSelected = value;

                    //

                    _SetControlBackgroundColor();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ColorControlUnselected属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Color ColorControlUnselected//属性
        {
            get//读取
            {
                return colorControlUnselected;
            }
            set//设置
            {
                if (value != colorControlUnselected)//设置了新的数值
                {
                    colorControlUnselected = value;

                    //

                    _SetControlBackgroundColor();
                }
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：ColorDrawCaptionEnable属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Color ColorDrawCaptionEnable//属性
        {
            get//读取
            {
                return colorDrawCaptionEnable;
            }
            set//设置
            {
                if (value != colorDrawCaptionEnable)//设置了新的数值
                {
                    colorDrawCaptionEnable = value;

                    //

                    _SetAxisColor();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ColorDrawCaptionEnable属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Color ColorDrawCaptionDisable//属性
        {
            get//读取
            {
                return colorDrawCaptionDisable;
            }
            set//设置
            {
                if (value != colorDrawCaptionDisable)//设置了新的数值
                {
                    colorDrawCaptionDisable = value;

                    //

                    _SetAxisColor();
                }
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：ColorDrawGraph属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Color ColorDrawGraph//属性
        {
            get//读取
            {
                return solidbrushDrawGraph.Color;
            }
            set//设置
            {
                if (value != solidbrushDrawGraph.Color)//设置了新的数值
                {
                    solidbrushDrawGraph = new SolidBrush(value);
                }
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：ColorValueEnable属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Color ColorValueEnable//属性
        {
            get//读取
            {
                return colorValueEnable;
            }
            set//设置
            {
                if (value != colorValueEnable)//设置了新的数值
                {
                    colorValueEnable = value;

                    //

                    _SetCurveValueColor();
                    _SetAxisColor();//更新当前值图标颜色
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ColorValueDisable属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Color ColorValueDisable//属性
        {
            get//读取
            {
                return colorValueDisable;
            }
            set//设置
            {
                if (value != colorValueDisable)//设置了新的数值
                {
                    colorValueDisable = value;

                    //

                    _SetCurveValueColor();
                    _SetAxisColor();//更新当前值图标颜色
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ColorValueUndefined属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Color ColorValueUndefined//属性
        {
            get//读取
            {
                return colorValueUndefined;
            }
            set//设置
            {
                if (value != colorValueUndefined)//设置了新的数值
                {
                    colorValueUndefined = value;

                    //

                    _SetCurveValueColor();
                    _SetAxisColor();//更新当前值图标颜色
                }
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：ValuePointPixelWidth属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Int32 ValuePointPixelWidth//属性
        {
            get//读取
            {
                return iValuePointPixelWidth;
            }
            set//设置
            {
                if (value != iValuePointPixelWidth)
                {
                    iValuePointPixelWidth = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：AutoXAxisValue属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Boolean AutoXAxisValue//属性
        {
            get//读取
            {
                return bAutoXAxisValue;
            }
            set//设置
            {
                if (value != bAutoXAxisValue)
                {
                    bAutoXAxisValue = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：GridDisplay属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Boolean GridDisplay//属性
        {
            get//读取
            {
                return bGridDisplay;
            }
            set//设置
            {
                if (value != bGridDisplay)
                {
                    bGridDisplay = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ColorGridLine属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Color ColorGridLine//属性
        {
            get//读取
            {
                return colorGridLine;
            }
            set//设置
            {
                if (value != colorGridLine)
                {
                    colorGridLine = value;

                    //

                    penGridLine = new Pen(colorGridLine, fGridLineWidth);
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：GridLineWidth属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public float GridLineWidth//属性
        {
            get//读取
            {
                return fGridLineWidth;
            }
            set//设置
            {
                if (value != fGridLineWidth)
                {
                    fGridLineWidth = value;

                    //

                    penGridLine = new Pen(colorGridLine, fGridLineWidth);
                }
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：ColorLine属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Color[] ColorLine//属性
        {
            get//读取
            {
                return colorLine;
            }
            set//设置
            {
                colorLine = new Color[value.Length];
                value.CopyTo(colorLine, 0);

                //

                _SetCurveValueColor();
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：LineWidth属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public float[] LineWidth//属性
        {
            get//读取
            {
                return fLineWidth;
            }
            set//设置
            {
                fLineWidth = new float[value.Length];
                value.CopyTo(fLineWidth, 0);

                //

                _SetCurveValueColor();
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：RunorStop属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public bool RunorStop//属性
        {
            get//读取
            {
                return Control_Data.RunorStop;
            }
            set//设置
            {
                if (value != Control_Data.RunorStop)//设置了新的数值
                {
                    Control_Data.RunorStop = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ControlSelected属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public bool ControlSelected//属性
        {
            get//读取
            {
                return Control_Data.ControlSelected;
            }
            set//设置
            {
                if (value != Control_Data.ControlSelected)//设置了新的数值
                {
                    Control_Data.ControlSelected = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ButtonRunStopShow属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public bool ButtonRunStopShow//属性
        {
            get//读取
            {
                return Control_Data.ButtonRunStopShow;
            }
            set//设置
            {
                if (value != Control_Data.ButtonRunStopShow)//设置了新的数值
                {
                    Control_Data.ButtonRunStopShow = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ButtonLearningShow属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public bool ButtonLearningShow//属性
        {
            get//读取
            {
                return Control_Data.ButtonLearningShow;
            }
            set//设置
            {
                if (value != Control_Data.ButtonLearningShow)//设置了新的数值
                {
                    Control_Data.ButtonLearningShow = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：Caption属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public string Caption//属性
        {
            get//读取
            {
                return sCaption;
            }
            set//设置
            {
                if (value != sCaption)
                {
                    sCaption = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：FontCaption 属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Font FontCaption //属性
        {
            get//读取
            {
                return fontCaption;
            }
            set//设置
            {
                if (value != fontCaption)
                {
                    fontCaption = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：RectCaption 属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Rectangle RectCaption //属性
        {
            get//读取
            {
                return rectCaption;
            }
            set//设置
            {
                if (value != rectCaption)
                {
                    rectCaption = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：RectGraph 属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Rectangle RectGraph //属性
        {
            get//读取
            {
                return rectGraph;
            }
            set//设置
            {
                if (value != rectGraph)
                {
                    rectGraph = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ValuePointDisplay属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Boolean ValuePointDisplay//属性
        {
            get//读取
            {
                return bValuePointDisplay;
            }
            set//设置
            {
                if (value != bValuePointDisplay)
                {
                    bValuePointDisplay = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：CurveDisplay属性的实现（设置完成后将为Value重新分配空间）
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Boolean[] CurveDisplay//属性
        {
            get//读取
            {
                return bCurveDisplay;
            }
            set//设置
            {
                bCurveDisplay = new Boolean[value.Length];
                value.CopyTo(bCurveDisplay, 0);
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：CurveNumber_Curve属性的实现（设置完成后将为Value重新分配空间）
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public int CurveNumber_Curve//属性
        {
            get//读取
            {
                return iCurveNumber_Curve;
            }
            set//设置
            {
                if (value != iCurveNumber_Curve)
                {
                    iCurveNumber_Curve = value;

                    //

                    Int32 i = 0;//循环控制变量

                    if (value != bCurveDisplay.Length)//创建
                    {
                        bCurveDisplay = new Boolean[iCurveNumber_Curve];//属性，曲线是否显示。取值范围：true，显示；false，不显示

                        for (i = 0; i < value; i++)
                        {
                            bCurveDisplay[i] = true;//属性，曲线是否显示。取值范围：true，显示；false，不显示
                        }
                    }

                    if (value != iCurrentValueIndex.Length)//创建
                    {
                        iCurrentValueIndex = new int[iCurveNumber_Curve];//属性，绘制区域中当前显示的曲线值点（从0开始，小于0表示曲线图数值无效）

                        for (i = 0; i < value; i++)
                        {
                            iCurrentValueIndex[i] = 99;//属性，绘制区域中当前显示的曲线值点（从0开始，小于0表示曲线图数值无效）
                        }
                    }

                    if (value != colorLine.Length)//创建
                    {
                        colorLine = new Color[iCurveNumber_Curve];//属性，曲线连接线的颜色

                        for (i = 0; i < value; i++)
                        {
                            colorLine[i] = Color.FromArgb(255, 255, 255);//属性，曲线连接线的颜色
                        }
                    }

                    if (value != fLineWidth.Length)//创建
                    {
                        fLineWidth = new float[iCurveNumber_Curve];//属性，曲线连接线的宽度

                        for (i = 0; i < value; i++)
                        {
                            fLineWidth[i] = 1F;//属性，曲线连接线的宽度
                        }
                    }

                    if (value != penLine.Length)//创建
                    {
                        penLine = new Pen[iCurveNumber_Curve];//画笔，绘制曲线连接线

                        for (i = 0; i < value; i++)
                        {
                            penLine[i] = new Pen(Color.FromArgb(255, 255, 255));//画笔，绘制曲线连接线
                        }
                    }

                    if (value != Value_X.Length)//创建
                    {
                        Value_X = new Single[iCurveNumber_Curve][];

                        for (i = 0; i < value; i++)
                        {
                            Value_X[i] = new Single[iValueNumber_Curve];
                        }
                    }

                    if (value != Value_Y.Length)//创建
                    {
                        Value_Y = new Single[iCurveNumber_Curve][];

                        for (i = 0; i < value; i++)
                        {
                            Value_Y[i] = new Single[iValueNumber_Curve];
                        }
                    }

                    if (value != pointValue.Length)//创建
                    {
                        pointValue = new PointF[iCurveNumber_Curve][];

                        for (i = 0; i < value; i++)
                        {
                            pointValue[i] = new PointF[iValueNumber_Curve];
                        }
                    }

                    if (value != pointPixelValue.Length)//创建
                    {
                        pointPixelValue = new Point[iCurveNumber_Curve][];

                        for (i = 0; i < value; i++)
                        {
                            pointPixelValue[i] = new Point[iValueNumber_Curve];
                        }
                    }

                    if (value != solidbrushValue.Length)//创建
                    {
                        solidbrushValue = new SolidBrush[iCurveNumber_Curve][];

                        for (i = 0; i < value; i++)
                        {
                            solidbrushValue[i] = new SolidBrush[iValueNumber_Curve];
                        }
                    }

                    if (value != iStyle.Length)//创建
                    {
                        iStyle = new int[iCurveNumber_Curve][];

                        for (i = 0; i < value; i++)
                        {
                            iStyle[i] = new int[iValueNumber_Curve];
                        }
                    }

                    //

                    _InitValue();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ValueNumber_Curve属性的实现（设置完成后将为Value重新分配空间）
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public int ValueNumber_Curve//属性
        {
            get//读取
            {
                return iValueNumber_Curve;
            }
            set//设置
            {
                if (value != iValueNumber_Curve)
                {
                    iValueNumber_Curve = value;

                    //

                    Int32 i = 0;//循环控制变量

                    for (i = 0; i < iCurveNumber_Curve; i++)
                    {
                        if (value != pointValue[i].Length)
                        {
                            pointValue[i] = new PointF[iValueNumber_Curve];
                        }

                        if (value != pointPixelValue[i].Length)
                        {
                            pointPixelValue[i] = new Point[iValueNumber_Curve];
                        }

                        if (value != solidbrushValue[i].Length)
                        {
                            solidbrushValue[i] = new SolidBrush[iValueNumber_Curve];
                        }

                        if (value != iStyle[i].Length)
                        {
                            iStyle[i] = new int[iValueNumber_Curve];
                        }

                        //

                        if (value != Value_X[i].Length)
                        {
                            Value_X[i] = new Single[iValueNumber_Curve];
                        }

                        if (value != Value_Y[i].Length)
                        {
                            Value_Y[i] = new Single[iValueNumber_Curve];
                        }
                    }

                    //

                    _InitValue();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：Style属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public int[][] Style//属性
        {
            get//读取
            {
                return iStyle;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：CurrentValueIndex属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public int[] CurrentValueIndex//属性
        {
            get//读取
            {
                return iCurrentValueIndex;
            }
            set//设置
            {
                iCurrentValueIndex = new int[value.Length];
                value.CopyTo(iCurrentValueIndex, 0);
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：XAxisCurValueDisplay 属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public bool XAxisCurValueDisplay //属性
        {
            get//读取
            {
                return bXAxisCurValueDisplay;
            }
            set//设置
            {
                if (value != bXAxisCurValueDisplay)
                {
                    bXAxisCurValueDisplay = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：YAxisCurValueDisplay 属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public bool YAxisCurValueDisplay //属性
        {
            get//读取
            {
                return bYAxisCurValueDisplay;
            }
            set//设置
            {
                if (value != bYAxisCurValueDisplay)
                {
                    bYAxisCurValueDisplay = value;
                }
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：RectCurValue 属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Rectangle RectCurValue //属性
        {
            get//读取
            {
                return rectPixel_CurValue;
            }
            set//设置
            {
                if (value != rectPixel_CurValue)
                {
                    rectPixel_CurValue = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：RectCurValueIcon 属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Rectangle RectCurValueIcon //属性
        {
            get//读取
            {
                return rectPixel_CurValueIcon;
            }
            set//设置
            {
                if (value != rectPixel_CurValueIcon)
                {
                    rectPixel_CurValueIcon = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：SolidbrushDrawCurValue 属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public SolidBrush SolidbrushDrawCurValue //属性
        {
            get//读取
            {
                return solidbrushDrawCurValue;
            }
            set//设置
            {
                if (value != solidbrushDrawCurValue)
                {
                    solidbrushDrawCurValue = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：SolidbrushDrawCurValueIcon 属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public SolidBrush SolidbrushDrawCurValueIcon //属性
        {
            get//读取
            {
                return solidbrushDrawCurValueIcon;
            }
            set//设置
            {
                if (value != solidbrushDrawCurValueIcon)
                {
                    solidbrushDrawCurValueIcon = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：FontCurValue 属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Font FontCurValue //属性
        {
            get//读取
            {
                return fontCurValue;
            }
            set//设置
            {
                if (value != fontCurValue)
                {
                    fontCurValue = value;
                }
            }
        }

        //函数

        //----------------------------------------------------------------------
        // 功能说明：设置曲线图数值X坐标值
        // 输入参数：1.bInitial：是否初始化。取值范围：true，是；false，否
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _SetValue_XAxis(Boolean bInitial = false)
        {
            if (bInitial || bAutoXAxisValue)//自动
            {
                Int32 i = 0;//循环控制变量
                Int32 j = 0;//循环控制变量

                fIntervalValue = (Graph_XAxis.AxisMax_Value - Graph_XAxis.AxisMin_Value) / (iValueNumber_Curve - 1);

                for (i = 0; i < iCurveNumber_Curve; i++)
                {
                    for (j = 0; j < iValueNumber_Curve; j++)//初始化
                    {
                        Value_X[i][j] = Graph_XAxis.AxisMin_Value + j * fIntervalValue;
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：初始化曲线数值
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _InitValue()
        {
            Int32 i = 0;//循环控制变量
            Int32 j = 0;//循环控制变量

            _SetValue_XAxis(true);

            if (VisionSystemClassLibrary.Enum.AxisDataType.withoutEffectiveValue == Graph_YAxis.DataType)//不指定最小有效实际数值、最大有效实际数值，但指定坐标轴的最小值、最大值
            {
                for (i = 0; i < iCurveNumber_Curve; i++)
                {
                    for (j = 0; j < iValueNumber_Curve; j++)//初始化
                    {
                        if (0 == j % 2)
                        {
                            Value_Y[i][j] = Graph_YAxis.AxisMax_Value;//默认值
                        }
                        else
                        {
                            Value_Y[i][j] = Graph_YAxis.AxisMin_Value;//默认值
                        }
                    }
                }
            }
            else//指定最小有效实际数值、最大有效实际数值，同时指定坐标轴的最小值、最大值；指定最小有效实际数值、最大有效实际数值，同时指定距离像素值
            {
                for (i = 0; i < iCurveNumber_Curve; i++)
                {
                    for (j = 0; j < iValueNumber_Curve; j++)//初始化
                    {
                        if (0 == j % 2)
                        {
                            Value_Y[i][j] = Graph_YAxis.AxisEffectiveMax_Value;//默认值
                        }
                        else
                        {
                            Value_Y[i][j] = Graph_YAxis.AxisEffectiveMin_Value;//默认值
                        }
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：（通过参数修改属性值时），设置完成属性后，调用该函数检查设置并使正确的设置生效
        // 输入参数：无
        // 输出参数：无
        // 返回值：true：属性设置正确；false：属性设置错误
        //----------------------------------------------------------------------
        public bool _Apply(ref string sErrorMessage)
        {
            //属性检查

            //if (_Check(ref sErrorMessage))//检查属性设置
            //{
                //属性设置正确

                Graph_XAxis._SetAxisData();//配置X坐标轴
                Graph_YAxis._SetAxisData();//配置Y坐标轴
                _SetCurveValue();//配置曲线图
                _SetCurrentValue();//配置当前值

                return true;
            //}
            //else//属性设置错误
            //{
            //    return false;
            //}
        }

        //----------------------------------------------------------------------
        // 功能说明：控件属性修改后检查属性值的设置是否正确
        // 输入参数：1.sError：错误信息
        // 输出参数：无
        // 返回值：属性值设置是否正确。取值范围：true：是；false：否
        //----------------------------------------------------------------------
        public bool _Check(ref string sErrorMessage)
        {
            bool bReturn_Graph = _CheckGraph(ref sErrorMessage);//检查曲线图属性
            bool bReturn_X = _CheckAxis(Graph_XAxis, ref sErrorMessage);//检查X坐标轴属性
            bool bReturn_Y = _CheckAxis(Graph_YAxis, ref sErrorMessage);//检查Y坐标轴属性

            return bReturn_Graph && bReturn_X && bReturn_Y;
        }

        //----------------------------------------------------------------------
        // 功能说明：检查曲线图属性值的设置是否正确
        // 输入参数：1.sError：错误信息
        // 输出参数：无
        // 返回值：属性值设置是否正确。取值范围：true：是；false：否
        //----------------------------------------------------------------------
        private bool _CheckGraph(ref string sErrorMessage)
        {
            int i = 0;//循环控制变量
            int j = 0;//循环控制变量
            bool bReturn = true;//返回值

            //Tolerances_Graph属性

            if (ValueNumber_Curve <= 0)//绘制区域中的曲线值数目（坐标点数目，根据X轴数值获取）
            {
                sErrorMessage += "ValueNumber取值范围：> 0\n";

                bReturn = false;
            }

            for (i = 0; i < iCurveNumber_Curve; i++)
            {
                for (j = 0; j < iValueNumber_Curve; j++)//绘制区域中的曲线值（下位机传送的实际值）
                {
                    if (Value_Y[i][j] < Graph_YAxis.AxisMin_Value || Value_Y[i][j] > Graph_YAxis.AxisMax_Value)//超出范围
                    {
                        sErrorMessage += "Value取值范围：Control_Graph.Graph_YAxis.iAxisMin_Value ~ Control_Graph.Graph_YAxis.iAxisMax_Value\n";

                        bReturn = false;

                        break;
                    }
                }

                if (CurrentValueIndex[i] >= iValueNumber_Curve || iCurrentValueIndex[i] < 0)//绘制区域中当前显示的曲线值点
                {
                    sErrorMessage += "CurrentValueNumber取值范围：0 ~ Control_Graph.ValueNumber - 1\n";

                    bReturn = false;
                }
            }

            return bReturn;
        }

        //----------------------------------------------------------------------
        // 功能说明：检查坐标轴属性值的设置是否正确
        // 输入参数：1.Graph_Axis：坐标轴
        //         2.sError：错误信息
        // 输出参数：无
        // 返回值：属性值设置是否正确。取值范围：true：是；false：否
        //----------------------------------------------------------------------
        private bool _CheckAxis(Tolerances_Graph_Axis Graph_Axis, ref string sErrorMessage)
        {
            bool bReturn = true;//返回值

            //Tolerances_Graph_Axis属性

            if (Graph_Axis.AxisValueNumber <= 0)//坐标轴显示的坐标值个数
            {
                if (VisionSystemClassLibrary.Enum.AxisType.XAxis == Graph_Axis.type)//X坐标轴
                {
                    sErrorMessage += "XAxisValueNumber取值范围：> 0\n";
                }
                else//Y坐标轴
                {
                    sErrorMessage += "YAxisValueNumber取值范围：> 0\n";
                }

                bReturn = false;
            }

            if (Graph_Axis.SizePixel_AxisPoint.Width <= 0 || Graph_Axis.SizePixel_AxisPoint.Height <= 0)//显示的曲线图坐标轴中每个坐标点的区域大小
            {
                if (VisionSystemClassLibrary.Enum.AxisType.XAxis == Graph_Axis.type)//X坐标轴
                {
                    sErrorMessage += "SizePixel_XAxisPoint取值范围：Width > 0 && Height > 0\n";
                }
                else//Y坐标轴
                {
                    sErrorMessage += "SizePixel_YAxisPoint取值范围：Width > 0 && Height > 0\n";
                }

                bReturn = false;
            }

            if (Graph_Axis.LocationAxisMin_Pixel.X < 0 || Graph_Axis.LocationAxisMin_Pixel.Y < 0)//坐标轴最小值对应的像素值
            {
                if (VisionSystemClassLibrary.Enum.AxisType.XAxis == Graph_Axis.type)//X坐标轴
                {
                    sErrorMessage += "LocationXAxisMin_Pixel取值范围：X >= 0 && Y >= 0\n";
                }
                else//Y坐标轴
                {
                    sErrorMessage += "LocationYAxisMin_Pixel取值范围：X >= 0 && Y >= 0\n";
                }

                bReturn = false;
            }
            else if (Graph_Axis.LocationAxisMax_Pixel.X < 0 || Graph_Axis.LocationAxisMax_Pixel.Y < 0)//坐标轴最大值对应的像素值
            {
                if (VisionSystemClassLibrary.Enum.AxisType.XAxis == Graph_Axis.type)//X坐标轴
                {
                    sErrorMessage += "LocationXAxisMax_Pixel取值范围：X >= 0 && Y >= 0\n";
                }
                else//Y坐标轴
                {
                    sErrorMessage += "LocationYAxisMax_Pixel取值范围：X >= 0 && Y >= 0\n";
                }

                bReturn = false;
            }
            else//继续检查
            {
                if (VisionSystemClassLibrary.Enum.AxisType.XAxis == Graph_Axis.type)//X坐标轴
                {
                    if (Graph_Axis.LocationAxisMin_Pixel.X >= Graph_Axis.LocationAxisMax_Pixel.X)
                    {
                        sErrorMessage += "取值范围：LocationXAxisMin_Pixel.X < LocationXAxisMax_Pixel.X\n";

                        bReturn = false;
                    }
                }
                else//Y坐标轴
                {
                    if (Graph_Axis.LocationAxisMin_Pixel.Y <= Graph_Axis.LocationAxisMax_Pixel.Y)
                    {
                        sErrorMessage += "取值范围：LocationYAxisMin_Pixel.Y > LocationYAxisMax_Pixel.Y\n";

                        bReturn = false;
                    }
                }
            }

            if (Graph_Axis.AxisMin_Value >= Graph_Axis.AxisMax_Value)//坐标轴最小值对应的实际数值，坐标轴最大值对应的实际数值
            {
                if (VisionSystemClassLibrary.Enum.AxisType.XAxis == Graph_Axis.type)//X坐标轴
                {
                    sErrorMessage += "取值范围：XAxisMin_Value < XAxisMax_Value\n";
                }
                else//Y坐标轴
                {
                    sErrorMessage += "取值范围：YAxisMin_Value < YAxisMax_Value\n";
                }

                bReturn = false;
            }

            if (VisionSystemClassLibrary.Enum.AxisDataType.withoutEffectiveValue == Graph_Axis.DataType)//不指定最小有效实际数值、最大有效实际数值，但指定坐标轴的最小值、最大值
            {
                //不执行操作
            }
            else if (VisionSystemClassLibrary.Enum.AxisDataType.withEffectiveValue == Graph_Axis.DataType)//指定最小有效实际数值、最大有效实际数值，同时指定坐标轴的最小值、最大值
            {
                if (Graph_Axis.AxisEffectiveMin_Value > Graph_Axis.AxisEffectiveMax_Value)//坐标轴最小有效实际数值，坐标轴最大有效实际数值
                {
                    if (VisionSystemClassLibrary.Enum.AxisType.XAxis == Graph_Axis.type)//X坐标轴
                    {
                        sErrorMessage += "取值范围：XAxisEffectiveMin_Value <= XAxisEffectiveMax_Value\n";
                    }
                    else//Y坐标轴
                    {
                        sErrorMessage += "取值范围：YAxisEffectiveMin_Value <= YAxisEffectiveMax_Value\n";
                    }

                    bReturn = false;
                }
                else if (Graph_Axis.AxisEffectiveMin_Value < Graph_Axis.AxisMin_Value)//超出范围
                {
                    if (VisionSystemClassLibrary.Enum.AxisType.XAxis == Graph_Axis.type)//X坐标轴
                    {
                        sErrorMessage += "XAxisEffectiveMin_Value取值范围：>= XAxisMin_Value\n";
                    }
                    else//Y坐标轴
                    {
                        sErrorMessage += "YAxisEffectiveMin_Value取值范围：>= YAxisMin_Value\n";
                    }

                    bReturn = false;
                }
                else if (Graph_Axis.AxisEffectiveMax_Value > Graph_Axis.AxisMax_Value)//超出范围
                {
                    if (VisionSystemClassLibrary.Enum.AxisType.XAxis == Graph_Axis.type)//X坐标轴
                    {
                        sErrorMessage += "XAxisEffectiveMax_Value取值范围：<= XAxisMax_Value\n";
                    }
                    else//Y坐标轴
                    {
                        sErrorMessage += "YAxisEffectiveMax_Value取值范围：<= YAxisMax_Value\n";
                    }

                    bReturn = false;
                }
                else//符合要求
                {
                    //不执行操作
                }
            }
            else if (VisionSystemClassLibrary.Enum.AxisDataType.withPixelAndEffectiveValue == Graph_Axis.DataType)//指定最小有效实际数值、最大有效实际数值，同时指定距离像素值
            {
                if (Graph_Axis.AxisEffectiveMin_Value > Graph_Axis.AxisEffectiveMax_Value)//坐标轴最小有效实际数值，坐标轴最大有效实际数值
                {
                    if (VisionSystemClassLibrary.Enum.AxisType.XAxis == Graph_Axis.type)//X坐标轴
                    {
                        sErrorMessage += "XAxisEffectiveMin_Value <= XAxisEffectiveMax_Value\n";
                    }
                    else//Y坐标轴
                    {
                        sErrorMessage += "YAxisEffectiveMin_Value <= YAxisEffectiveMax_Value\n";
                    }

                    bReturn = false;
                }

                if (Graph_Axis.FromAxisMinToEffectiveMin_Pixel < 0 || Graph_Axis.FromAxisMinToEffectiveMin_Pixel > 20)//坐标轴最小有效实际数值对应的坐标点像素值与坐标轴最小值对应的坐标点像素值之间的距离像素值
                {
                    if (VisionSystemClassLibrary.Enum.AxisType.XAxis == Graph_Axis.type)//X坐标轴
                    {
                        sErrorMessage += "FromXAxisMinToEffectiveMin_Pixel取值范围：0 ~ 20\n";
                    }
                    else//Y坐标轴
                    {
                        sErrorMessage += "FromYAxisMinToEffectiveMin_Pixel取值范围：0 ~ 20\n";
                    }

                    bReturn = false;
                }

                if (Graph_Axis.FromAxisMaxToEffectiveMax_Pixel < 0 || Graph_Axis.FromAxisMaxToEffectiveMax_Pixel > 20)//坐标轴最大有效实际数值对应的坐标点像素值与坐标轴最大值对应的坐标点像素值之间的距离像素值
                {
                    if (VisionSystemClassLibrary.Enum.AxisType.XAxis == Graph_Axis.type)//X坐标轴
                    {
                        sErrorMessage += "FromXAxisMaxToEffectiveMax_Pixel取值范围：0 ~ 20\n";
                    }
                    else//Y坐标轴
                    {
                        sErrorMessage += "FromYAxisMaxToEffectiveMax_Pixel取值范围：0 ~ 20\n";
                    }

                    bReturn = false;
                }
            }
            else//超出范围
            {
                if (VisionSystemClassLibrary.Enum.AxisType.XAxis == Graph_Axis.type)//X坐标轴
                {
                    sErrorMessage += "XAxisDataType取值范围：AxisDataType\n";
                }
                else//Y坐标轴
                {
                    sErrorMessage += "YAxisDataType取值范围：AxisDataType\n";
                }

                bReturn = false;
            }

            return bReturn;
        }

        //----------------------------------------------------------------------
        // 功能说明：设置绘图数据
        // 输入参数：1.graphics：绘图
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _SetGraphics(Graphics graphics)
        {
            Graph_Graphics = graphics;
            Graph_XAxis.Graph_Graphics = graphics;
            Graph_YAxis.Graph_Graphics = graphics;
        }

        //----------------------------------------------------------------------
        // 功能说明：配置曲线图和当前值数值（当新的曲线图数值、当前值应用时调用本函数）
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _SetCurveValue()
        {
            Int32 i = 0;//循环控制变量
            Int32 j = 0;//循环控制变量

            if (VisionSystemClassLibrary.Enum.AxisDataType.withoutEffectiveValue == Graph_YAxis.DataType)//无有效值区间
            {
                for (i = 0; i < iCurrentValueIndex.Length; i++)//初始化
                {
                    for (j = 0; j <= iCurrentValueIndex[i]; j++)//初始化
                    {
                        pointValue[i][j].X = Value_X[i][j];
                        pointValue[i][j].Y = Value_Y[i][j];

                        pointPixelValue[i][j].X = (int)(Graph_XAxis.dAxis_K * pointValue[i][j].X + Graph_XAxis.dAxis_B);
                        pointPixelValue[i][j].Y = (int)(Graph_YAxis.dAxis_K * pointValue[i][j].Y + Graph_YAxis.dAxis_B);

                        if (bCurveMode)//曲线图模式
                        {
                            iStyle[i][j] = 1;

                            solidbrushValue[i][j] = new SolidBrush(colorLine[i]);
                        }
                        else//其它
                        {
                            if (pointValue[i][j].Y >= Graph_YAxis.AxisMin_Value && pointValue[i][j].Y <= Graph_YAxis.AxisMax_Value)//坐标轴范围内
                            {
                                iStyle[i][j] = 1;

                                solidbrushValue[i][j] = new SolidBrush(colorValueEnable);
                            }
                            else//超出坐标轴范围（重新赋值，实际数值不变，以坐标轴边界值作为绘制曲线的值）
                            {
                                iStyle[i][j] = 3;

                                solidbrushValue[i][j] = new SolidBrush(colorValueUndefined);

                                if (pointValue[i][j].Y < Graph_YAxis.AxisMin_Value)//小于坐标轴最小值
                                {
                                    pointPixelValue[i][j].Y = Graph_YAxis.LocationAxisMin_Pixel.Y;
                                }
                                else//大于坐标轴最大值
                                {
                                    pointPixelValue[i][j].Y = Graph_YAxis.LocationAxisMax_Pixel.Y;
                                }
                            }
                        }
                    }
                }
            }
            else//包含有效值区间
            {
                if (Graph_YAxis.AxisEffectiveMin_Value == Graph_YAxis.AxisEffectiveMax_Value)//最小有效值和最大有效值相等
                {
                    for (i = 0; i < iCurrentValueIndex.Length; i++)//初始化
                    {
                        for (j = 0; j <= iCurrentValueIndex[i]; j++)//初始化
                        {
                            if (bCurveMode)//曲线图模式
                            {
                                pointValue[i][j].X = Value_X[i][j];
                                pointValue[i][j].Y = Value_Y[i][j];

                                pointPixelValue[i][j].X = (int)(Graph_XAxis.dAxis_K * pointValue[i][j].X + Graph_XAxis.dAxis_B);
                                pointPixelValue[i][j].Y = (int)(Graph_YAxis.dAxis_K * pointValue[i][j].Y + Graph_YAxis.dAxis_B);

                                iStyle[i][j] = 1;

                                solidbrushValue[i][j] = new SolidBrush(colorLine[i]);
                            }
                            else//其它
                            {
                                pointValue[i][j].X = Value_X[i][j];
                                pointValue[i][j].Y = Value_Y[i][j];

                                iStyle[i][j] = 2;

                                solidbrushValue[i][j] = new SolidBrush(colorValueDisable);
                            }
                        }
                    }
                }
                else//最小有效值和最大有效值不相同
                {
                    for (i = 0; i < iCurrentValueIndex.Length; i++)//初始化
                    {
                        for (j = 0; j <= iCurrentValueIndex[i]; j++)//初始化
                        {
                            if (bCurveMode)//曲线图模式
                            {
                                pointValue[i][j].X = Value_X[i][j];
                                pointValue[i][j].Y = Value_Y[i][j];

                                pointPixelValue[i][j].X = (int)(Graph_XAxis.dAxis_K * pointValue[i][j].X + Graph_XAxis.dAxis_B);
                                pointPixelValue[i][j].Y = (int)(Graph_YAxis.dAxis_K * pointValue[i][j].Y + Graph_YAxis.dAxis_B);

                                iStyle[i][j] = 1;

                                solidbrushValue[i][j] = new SolidBrush(colorLine[i]);
                            }
                            else//其它
                            {
                                pointValue[i][j].X = Value_X[i][j];
                                pointValue[i][j].Y = Value_Y[i][j];

                                pointPixelValue[i][j].X = (int)(Graph_XAxis.dAxis_K * pointValue[i][j].X + Graph_XAxis.dAxis_B);
                                pointPixelValue[i][j].Y = (int)(Graph_YAxis.dAxis_K * pointValue[i][j].Y + Graph_YAxis.dAxis_B);

                                if (Control_Data.TolerancesTools.EffectiveMin_State && Control_Data.TolerancesTools.EffectiveMax_State) //上下限都开启
                                {
                                    if (pointValue[i][j].Y >= Graph_YAxis.AxisEffectiveMin_Value && pointValue[i][j].Y <= Graph_YAxis.AxisEffectiveMax_Value)//坐标轴范围内
                                    {
                                        iStyle[i][j] = 1;

                                        solidbrushValue[i][j] = new SolidBrush(colorValueEnable);
                                    }
                                    else if (pointValue[i][j].Y >= Graph_YAxis.AxisMin_Value && pointValue[i][j].Y < Graph_YAxis.AxisEffectiveMin_Value)//无效值
                                    {
                                        iStyle[i][j] = 2;

                                        solidbrushValue[i][j] = new SolidBrush(colorValueDisable);
                                    }
                                    else if (pointValue[i][j].Y > Graph_YAxis.AxisEffectiveMax_Value && pointValue[i][j].Y <= Graph_YAxis.AxisMax_Value)//无效值
                                    {
                                        iStyle[i][j] = 2;

                                        solidbrushValue[i][j] = new SolidBrush(colorValueDisable);
                                    }
                                    else//超出坐标轴范围
                                    {
                                        iStyle[i][j] = 3;

                                        solidbrushValue[i][j] = new SolidBrush(colorValueUndefined);

                                        if (pointValue[i][j].Y < Graph_YAxis.AxisMin_Value)//小于坐标轴最小值
                                        {
                                            pointPixelValue[i][j].Y = Graph_YAxis.LocationAxisMin_Pixel.Y;
                                        }
                                        else//大于坐标轴最大值
                                        {
                                            pointPixelValue[i][j].Y = Graph_YAxis.LocationAxisMax_Pixel.Y;
                                        }
                                    }
                                }
                                else if (Control_Data.TolerancesTools.EffectiveMin_State && (false == Control_Data.TolerancesTools.EffectiveMax_State)) //下限开启
                                {
                                    if (pointValue[i][j].Y >= Graph_YAxis.AxisEffectiveMin_Value && pointValue[i][j].Y <= Graph_YAxis.AxisMax_Value)//坐标轴范围内
                                    {
                                        iStyle[i][j] = 1;

                                        solidbrushValue[i][j] = new SolidBrush(colorValueEnable);
                                    }
                                    else if (pointValue[i][j].Y >= Graph_YAxis.AxisMin_Value && pointValue[i][j].Y < Graph_YAxis.AxisEffectiveMin_Value)//无效值
                                    {
                                        iStyle[i][j] = 2;

                                        solidbrushValue[i][j] = new SolidBrush(colorValueDisable);
                                    }
                                    else//超出坐标轴范围
                                    {
                                        iStyle[i][j] = 3;

                                        solidbrushValue[i][j] = new SolidBrush(colorValueUndefined);

                                        if (pointValue[i][j].Y < Graph_YAxis.AxisMin_Value)//小于坐标轴最小值
                                        {
                                            pointPixelValue[i][j].Y = Graph_YAxis.LocationAxisMin_Pixel.Y;
                                        }
                                        else//大于坐标轴最大值
                                        {
                                            pointPixelValue[i][j].Y = Graph_YAxis.LocationAxisMax_Pixel.Y;
                                        }
                                    }
                                }
                                else if ((false == Control_Data.TolerancesTools.EffectiveMin_State) && Control_Data.TolerancesTools.EffectiveMax_State) //上限开启
                                {
                                    if (pointValue[i][j].Y >= Graph_YAxis.AxisMin_Value && pointValue[i][j].Y <= Graph_YAxis.AxisEffectiveMax_Value)//坐标轴范围内
                                    {
                                        iStyle[i][j] = 1;

                                        solidbrushValue[i][j] = new SolidBrush(colorValueEnable);
                                    }
                                    else if (pointValue[i][j].Y > Graph_YAxis.AxisEffectiveMax_Value && pointValue[i][j].Y <= Graph_YAxis.AxisMax_Value)//无效值
                                    {
                                        iStyle[i][j] = 2;

                                        solidbrushValue[i][j] = new SolidBrush(colorValueDisable);
                                    }
                                    else//超出坐标轴范围
                                    {
                                        iStyle[i][j] = 3;

                                        solidbrushValue[i][j] = new SolidBrush(colorValueUndefined);

                                        if (pointValue[i][j].Y < Graph_YAxis.AxisMin_Value)//小于坐标轴最小值
                                        {
                                            pointPixelValue[i][j].Y = Graph_YAxis.LocationAxisMin_Pixel.Y;
                                        }
                                        else//大于坐标轴最大值
                                        {
                                            pointPixelValue[i][j].Y = Graph_YAxis.LocationAxisMax_Pixel.Y;
                                        }
                                    }
                                }
                                else
                                {

                                }
                            }
                        }
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：配置当前值数值（当新的当前值应用时调用本函数）
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _SetCurrentValue()
        {
            //当前值及其绘制范围

            sCurValue = "";

            if (iCurrentValueIndex[0] < 0)//曲线值无效
            {
                sCurValue = "";
            }
            else//曲线值有效
            {
                string sCurValue_XAxis = "";//临时变量
                string sCurValue_YAxis = "";//临时变量

                if (0 == Graph_XAxis.AxisValuePrecision)//不显示小数位
                {
                    sCurValue_XAxis = ((int)pointValue[0][iCurrentValueIndex[0]].X).ToString();
                } 
                else//显示小数位
                {
                    sCurValue_XAxis = pointValue[0][iCurrentValueIndex[0]].X.ToString("F" + Graph_XAxis.AxisValuePrecision.ToString());
                }

                if (0 == Graph_YAxis.AxisValuePrecision)//不显示小数位
                {
                    sCurValue_YAxis = ((int)pointValue[0][iCurrentValueIndex[0]].Y).ToString();
                }
                else//显示小数位
                {
                    sCurValue_YAxis = pointValue[0][iCurrentValueIndex[0]].Y.ToString("F" + Graph_YAxis.AxisValuePrecision.ToString());
                }

                //

                if (bXAxisCurValueDisplay && bYAxisCurValueDisplay)//显示X轴当前值，显示Y轴当前值
                {
                    sCurValue = sCurValue_XAxis + "，" + sCurValue_YAxis;
                }
                else if (!bXAxisCurValueDisplay && bYAxisCurValueDisplay)//不显示X轴当前值，显示Y轴当前值
                {
                    sCurValue = sCurValue_YAxis;
                }
                else if (bXAxisCurValueDisplay && !bYAxisCurValueDisplay)//显示X轴当前值，不显示Y轴当前值
                {
                    sCurValue = sCurValue_XAxis;
                }
                else//不显示X轴当前值，不显示Y轴当前值
                {
                    sCurValue = "";
                }
            }

            _SetAxisColor();
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：设置曲线图颜色
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _SetCurveValueColor()
        {
            Int32 i = 0;//循环控制变量
            Int32 j = 0;//循环控制变量

            //

            for (i = 0; i < penLine.Length; i++)
            {
                penLine[i] = new Pen(colorLine[i], fLineWidth[i]);//画笔，绘制曲线连接线
            }

            //

            if (VisionSystemClassLibrary.Enum.AxisDataType.withoutEffectiveValue == Graph_YAxis.DataType)//无有效值区间
            {
                for (i = 0; i < iCurrentValueIndex.Length; i++)
                {
                    for (j = 0; j <= iCurrentValueIndex[i]; j++)//初始化
                    {
                        if (pointValue[i][j].Y >= Graph_YAxis.AxisMin_Value && pointValue[i][j].Y <= Graph_YAxis.AxisMax_Value)//坐标轴范围内
                        {
                            solidbrushValue[i][j] = new SolidBrush(colorValueEnable);
                        }
                        else//超出坐标轴范围（重新赋值，实际数值不变，以坐标轴边界值作为绘制曲线的值）
                        {
                            solidbrushValue[i][j] = new SolidBrush(colorValueUndefined);
                        }
                    }
                }
            }
            else//包含有效值区间
            {
                if (Graph_YAxis.AxisEffectiveMin_Value == Graph_YAxis.AxisEffectiveMax_Value)//最小有效值和最大有效值相等
                {
                    for (i = 0; i < iCurrentValueIndex.Length; i++)
                    {
                        for (j = 0; j <= iCurrentValueIndex[i]; j++)//初始化
                        {
                            solidbrushValue[i][j] = new SolidBrush(colorValueDisable);
                        }
                    }
                }
                else//最小有效值和最大有效值不相同
                {
                    for (i = 0; i < iCurrentValueIndex.Length; i++)
                    {
                        for (j = 0; j <= iCurrentValueIndex[i]; j++)//初始化
                        {
                            if (pointValue[i][j].Y >= Graph_YAxis.AxisEffectiveMin_Value && pointValue[i][j].Y <= Graph_YAxis.AxisEffectiveMax_Value)//坐标轴范围内
                            {
                                solidbrushValue[i][j] = new SolidBrush(colorValueEnable);
                            }
                            else if (pointValue[i][j].Y >= Graph_YAxis.AxisMin_Value && pointValue[i][j].Y <= Graph_YAxis.AxisEffectiveMin_Value)//无效值
                            {
                                solidbrushValue[i][j] = new SolidBrush(colorValueDisable);
                            }
                            else if (pointValue[i][j].Y >= Graph_YAxis.AxisEffectiveMax_Value && pointValue[i][j].Y <= Graph_YAxis.AxisMax_Value)//无效值
                            {
                                solidbrushValue[i][j] = new SolidBrush(colorValueDisable);
                            }
                            else//超出坐标轴范围
                            {
                                solidbrushValue[i][j] = new SolidBrush(colorValueUndefined);
                            }
                        }
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：配置绘图工具
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _SetControlBackgroundColor()
        {
            if (Control_Data.ControlSelected)//当前控件处于选中状态
            {
                solidbrushControl = new SolidBrush(colorControlSelected);
            }
            else//当前控件处于未选中状态
            {
                solidbrushControl = new SolidBrush(colorControlUnselected);
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：配置绘图工具
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _SetAxisColor()
        {
            if (iCurrentValueIndex[0] >= 0)//曲线值有效
            {
                if (VisionSystemClassLibrary.Enum.AxisDataType.withoutEffectiveValue == Graph_YAxis.DataType)//不指定最小有效实际数值、最大有效实际数值，但指定坐标轴的最小值、最大值
                {
                    if (pointValue[0][iCurrentValueIndex[0]].Y >= Graph_YAxis.AxisMin_Value && pointValue[0][iCurrentValueIndex[0]].Y <= Graph_YAxis.AxisMax_Value)//有效值范围内
                    {
                        solidbrushDrawCurValueIcon = new SolidBrush(colorValueEnable);
                    }
                    else//超出有效值范围
                    {
                        solidbrushDrawCurValueIcon = new SolidBrush(colorValueDisable);
                    }
                }
                else//指定最小有效实际数值、最大有效实际数值，同时指定坐标轴的最小值、最大值；指定最小有效实际数值、最大有效实际数值，同时指定距离像素值
                {
                    if (Control_Data.TolerancesTools.EffectiveMin_State && Control_Data.TolerancesTools.EffectiveMax_State) //上下限都开启
                    {
                        if (pointValue[0][iCurrentValueIndex[0]].Y >= Graph_YAxis.AxisEffectiveMin_Value && pointValue[0][iCurrentValueIndex[0]].Y <= Graph_YAxis.AxisEffectiveMax_Value)//有效值范围内
                        {
                            solidbrushDrawCurValueIcon = new SolidBrush(colorValueEnable);
                        }
                        else//超出有效值范围
                        {
                            solidbrushDrawCurValueIcon = new SolidBrush(colorValueDisable);
                        }
                    }
                    else if (Control_Data.TolerancesTools.EffectiveMin_State && (false == Control_Data.TolerancesTools.EffectiveMax_State)) //下限开启
                    {
                        if (pointValue[0][iCurrentValueIndex[0]].Y >= Graph_YAxis.AxisEffectiveMin_Value)//有效值范围内
                        {
                            solidbrushDrawCurValueIcon = new SolidBrush(colorValueEnable);
                        }
                        else//超出有效值范围
                        {
                            solidbrushDrawCurValueIcon = new SolidBrush(colorValueDisable);
                        }
                    }
                    else if ((false == Control_Data.TolerancesTools.EffectiveMin_State) && Control_Data.TolerancesTools.EffectiveMax_State) //上限开启
                    {
                        if (pointValue[0][iCurrentValueIndex[0]].Y <= Graph_YAxis.AxisEffectiveMax_Value)//有效值范围内
                        {
                            solidbrushDrawCurValueIcon = new SolidBrush(colorValueEnable);
                        }
                        else//超出有效值范围
                        {
                            solidbrushDrawCurValueIcon = new SolidBrush(colorValueDisable);
                        }
                    }
                }
            }
            else//曲线值无效
            {
                solidbrushDrawCurValueIcon = new SolidBrush(colorValueDisable);
            }
            
            //

            if (Control_Data.RunorStop)//运行
            {
                if (iCurrentValueIndex[0] >= 0)//曲线值有效
                {
                    //曲线图、标题文本为使能状态

                    solidbrushDrawCaption = new SolidBrush(colorDrawCaptionEnable);//绘制标题文本所使用的画刷

                    //X坐标轴
                    Graph_XAxis.tolerancesDrawing_Axis.PenDrawAxis = new Pen(Graph_XAxis.tolerancesDrawing_Axis.ColorDrawAxisEnable, Graph_XAxis.tolerancesDrawing_Axis.DrawAxisWidth);//绘制曲线坐标轴所使用的画笔
                    Graph_XAxis.tolerancesDrawing_Axis.SolidbrushDrawAxisValue = new SolidBrush(Graph_XAxis.tolerancesDrawing_Axis.ColorDrawAxisValueEnable);//绘制曲线坐标轴数值所使用的画刷
                    Graph_XAxis.tolerancesDrawing_Axis.PenDrawEffectiveMinLine = new Pen(Graph_XAxis.tolerancesDrawing_Axis.ColorDrawEffectiveMinLineEnable, Graph_XAxis.tolerancesDrawing_Axis.DrawEffectiveLineWidth);//绘制坐标轴最小有效值区域分界线所使用的画笔
                    Graph_XAxis.tolerancesDrawing_Axis.PenDrawEffectiveMaxLine = new Pen(Graph_XAxis.tolerancesDrawing_Axis.ColorDrawEffectiveMaxLineEnable, Graph_XAxis.tolerancesDrawing_Axis.DrawEffectiveLineWidth);//绘制坐标轴最大有效值区域分界线所使用的画笔
                    Graph_XAxis.tolerancesDrawing_Axis.PenDrawEffectiveMinLine.DashStyle = DashStyle.Dash;//绘制坐标轴最小有效值区域分界线所使用的画笔
                    Graph_XAxis.tolerancesDrawing_Axis.PenDrawEffectiveMaxLine.DashStyle = DashStyle.Dash;//绘制坐标轴最大有效值区域分界线所使用的画笔
                    Graph_XAxis.tolerancesDrawing_Axis.SolidbrushDrawEffectiveMinValue = new SolidBrush(Graph_XAxis.tolerancesDrawing_Axis.ColorDrawEffectiveMinValueEnable);//绘制坐标轴最小有效值所使用的画刷
                    Graph_XAxis.tolerancesDrawing_Axis.SolidbrushDrawEffectiveMaxValue = new SolidBrush(Graph_XAxis.tolerancesDrawing_Axis.ColorDrawEffectiveMaxValueEnable);//绘制坐标轴最大有效值所使用的画刷
                    Graph_XAxis.tolerancesDrawing_Axis.PenDrawAdditionalValueLine = new Pen(Graph_XAxis.tolerancesDrawing_Axis.ColorDrawAdditionalValueLineEnable, Graph_XAxis.tolerancesDrawing_Axis.DrawAdditionalValueLineWidth);//绘制曲线图坐标轴上显示的附加数值指示线所使用的画笔
                    Graph_XAxis.tolerancesDrawing_Axis.SolidbrushDrawAdditionalValue = new SolidBrush(Graph_XAxis.tolerancesDrawing_Axis.ColorDrawAdditionalValueEnable);//绘制曲线图Y坐标轴上显示的附加数值所使用的画笔
                    Graph_XAxis.tolerancesDrawing_Axis.StringformatDrawValue.Alignment = StringAlignment.Center;//设置格式
                    Graph_XAxis.tolerancesDrawing_Axis.StringformatDrawValue.LineAlignment = StringAlignment.Center;//设置格式

                    //Y坐标轴
                    Graph_YAxis.tolerancesDrawing_Axis.PenDrawAxis = new Pen(Graph_YAxis.tolerancesDrawing_Axis.ColorDrawAxisEnable, Graph_YAxis.tolerancesDrawing_Axis.DrawAxisWidth);//绘制曲线坐标轴所使用的画笔
                    Graph_YAxis.tolerancesDrawing_Axis.SolidbrushDrawAxisValue = new SolidBrush(Graph_YAxis.tolerancesDrawing_Axis.ColorDrawAxisValueEnable);//绘制曲线坐标轴数值所使用的画刷
                    Graph_YAxis.tolerancesDrawing_Axis.PenDrawEffectiveMinLine = new Pen(Graph_YAxis.tolerancesDrawing_Axis.ColorDrawEffectiveMinLineEnable, Graph_YAxis.tolerancesDrawing_Axis.DrawEffectiveLineWidth);//绘制坐标轴最小有效值区域分界线所使用的画笔
                    Graph_YAxis.tolerancesDrawing_Axis.PenDrawEffectiveMaxLine = new Pen(Graph_YAxis.tolerancesDrawing_Axis.ColorDrawEffectiveMaxLineEnable, Graph_YAxis.tolerancesDrawing_Axis.DrawEffectiveLineWidth);//绘制坐标轴最大有效值区域分界线所使用的画笔
                    Graph_YAxis.tolerancesDrawing_Axis.PenDrawEffectiveMinLine.DashStyle = DashStyle.Dash;//绘制坐标轴最小有效值区域分界线所使用的画笔
                    Graph_YAxis.tolerancesDrawing_Axis.PenDrawEffectiveMaxLine.DashStyle = DashStyle.Dash;//绘制坐标轴最大有效值区域分界线所使用的画笔
                    Graph_YAxis.tolerancesDrawing_Axis.SolidbrushDrawEffectiveMinValue = new SolidBrush(Graph_YAxis.tolerancesDrawing_Axis.ColorDrawEffectiveMinValueEnable);//绘制坐标轴最小有效值所使用的画刷
                    Graph_YAxis.tolerancesDrawing_Axis.SolidbrushDrawEffectiveMaxValue = new SolidBrush(Graph_YAxis.tolerancesDrawing_Axis.ColorDrawEffectiveMaxValueEnable);//绘制坐标轴最大有效值所使用的画刷
                    Graph_YAxis.tolerancesDrawing_Axis.PenDrawAdditionalValueLine = new Pen(Graph_YAxis.tolerancesDrawing_Axis.ColorDrawAdditionalValueLineEnable, Graph_YAxis.tolerancesDrawing_Axis.DrawAdditionalValueLineWidth);//绘制曲线图坐标轴上显示的附加数值指示线所使用的画笔
                    Graph_YAxis.tolerancesDrawing_Axis.SolidbrushDrawAdditionalValue = new SolidBrush(Graph_YAxis.tolerancesDrawing_Axis.ColorDrawAdditionalValueEnable);//绘制曲线图Y坐标轴上显示的附加数值所使用的画笔
                    Graph_YAxis.tolerancesDrawing_Axis.StringformatDrawValue.Alignment = StringAlignment.Center;//设置格式
                    Graph_YAxis.tolerancesDrawing_Axis.StringformatDrawValue.LineAlignment = StringAlignment.Center;//设置格式
                }
                else//曲线值无效
                {
                    //X坐标及X、Y坐标轴为使能状态，Y坐标及曲线、标题文本为禁止状态

                    solidbrushDrawCaption = new SolidBrush(colorDrawCaptionDisable);//绘制标题文本所使用的画刷

                    //X坐标轴
                    Graph_XAxis.tolerancesDrawing_Axis.PenDrawAxis = new Pen(Graph_XAxis.tolerancesDrawing_Axis.ColorDrawAxisEnable, Graph_XAxis.tolerancesDrawing_Axis.DrawAxisWidth);//绘制曲线坐标轴所使用的画笔
                    Graph_XAxis.tolerancesDrawing_Axis.SolidbrushDrawAxisValue = new SolidBrush(Graph_XAxis.tolerancesDrawing_Axis.ColorDrawAxisValueEnable);//绘制曲线坐标轴数值所使用的画刷
                    Graph_XAxis.tolerancesDrawing_Axis.PenDrawEffectiveMinLine = new Pen(Graph_XAxis.tolerancesDrawing_Axis.ColorDrawEffectiveMinLineEnable, Graph_XAxis.tolerancesDrawing_Axis.DrawEffectiveLineWidth);//绘制坐标轴最小有效值区域分界线所使用的画笔
                    Graph_XAxis.tolerancesDrawing_Axis.PenDrawEffectiveMaxLine = new Pen(Graph_XAxis.tolerancesDrawing_Axis.ColorDrawEffectiveMaxLineEnable, Graph_XAxis.tolerancesDrawing_Axis.DrawEffectiveLineWidth);//绘制坐标轴最大有效值区域分界线所使用的画笔
                    Graph_XAxis.tolerancesDrawing_Axis.PenDrawEffectiveMinLine.DashStyle = DashStyle.Dash;//绘制坐标轴最小有效值区域分界线所使用的画笔
                    Graph_XAxis.tolerancesDrawing_Axis.PenDrawEffectiveMaxLine.DashStyle = DashStyle.Dash;//绘制坐标轴最大有效值区域分界线所使用的画笔
                    Graph_XAxis.tolerancesDrawing_Axis.SolidbrushDrawEffectiveMinValue = new SolidBrush(Graph_XAxis.tolerancesDrawing_Axis.ColorDrawEffectiveMinValueEnable);//绘制坐标轴最小有效值所使用的画刷
                    Graph_XAxis.tolerancesDrawing_Axis.SolidbrushDrawEffectiveMaxValue = new SolidBrush(Graph_XAxis.tolerancesDrawing_Axis.ColorDrawEffectiveMaxValueEnable);//绘制坐标轴最大有效值所使用的画刷
                    Graph_XAxis.tolerancesDrawing_Axis.PenDrawAdditionalValueLine = new Pen(Graph_XAxis.tolerancesDrawing_Axis.ColorDrawAdditionalValueLineEnable, Graph_XAxis.tolerancesDrawing_Axis.DrawAdditionalValueLineWidth);//绘制曲线图坐标轴上显示的附加数值指示线所使用的画笔
                    Graph_XAxis.tolerancesDrawing_Axis.SolidbrushDrawAdditionalValue = new SolidBrush(Graph_XAxis.tolerancesDrawing_Axis.ColorDrawAdditionalValueEnable);//绘制曲线图Y坐标轴上显示的附加数值所使用的画笔
                    Graph_XAxis.tolerancesDrawing_Axis.StringformatDrawValue.Alignment = StringAlignment.Center;//设置格式
                    Graph_XAxis.tolerancesDrawing_Axis.StringformatDrawValue.LineAlignment = StringAlignment.Center;//设置格式

                    //Y坐标轴
                    Graph_YAxis.tolerancesDrawing_Axis.PenDrawAxis = new Pen(Graph_YAxis.tolerancesDrawing_Axis.ColorDrawAxisEnable, Graph_YAxis.tolerancesDrawing_Axis.DrawAxisWidth);//绘制曲线坐标轴所使用的画笔
                    Graph_YAxis.tolerancesDrawing_Axis.SolidbrushDrawAxisValue = new SolidBrush(Graph_YAxis.tolerancesDrawing_Axis.ColorDrawAxisValueDisable);//绘制曲线坐标轴数值所使用的画刷
                    Graph_YAxis.tolerancesDrawing_Axis.PenDrawEffectiveMinLine = new Pen(Graph_YAxis.tolerancesDrawing_Axis.ColorDrawEffectiveLineDisable, Graph_YAxis.tolerancesDrawing_Axis.DrawEffectiveLineWidth);//绘制坐标轴最小有效值区域分界线所使用的画笔
                    Graph_YAxis.tolerancesDrawing_Axis.PenDrawEffectiveMaxLine = new Pen(Graph_YAxis.tolerancesDrawing_Axis.ColorDrawEffectiveLineDisable, Graph_YAxis.tolerancesDrawing_Axis.DrawEffectiveLineWidth);//绘制坐标轴最大有效值区域分界线所使用的画笔
                    Graph_YAxis.tolerancesDrawing_Axis.PenDrawEffectiveMinLine.DashStyle = DashStyle.Dash;//绘制坐标轴最小有效值区域分界线所使用的画笔
                    Graph_YAxis.tolerancesDrawing_Axis.PenDrawEffectiveMaxLine.DashStyle = DashStyle.Dash;//绘制坐标轴最大有效值区域分界线所使用的画笔
                    Graph_YAxis.tolerancesDrawing_Axis.SolidbrushDrawEffectiveMinValue = new SolidBrush(Graph_YAxis.tolerancesDrawing_Axis.ColorDrawEffectiveValueDisable);//绘制坐标轴最小有效值所使用的画刷
                    Graph_YAxis.tolerancesDrawing_Axis.SolidbrushDrawEffectiveMaxValue = new SolidBrush(Graph_YAxis.tolerancesDrawing_Axis.ColorDrawEffectiveValueDisable);//绘制坐标轴最大有效值所使用的画刷
                    Graph_YAxis.tolerancesDrawing_Axis.PenDrawAdditionalValueLine = new Pen(Graph_YAxis.tolerancesDrawing_Axis.ColorDrawAdditionalValueLineDisable, Graph_YAxis.tolerancesDrawing_Axis.DrawAdditionalValueLineWidth);//绘制曲线图坐标轴上显示的附加数值指示线所使用的画笔
                    Graph_YAxis.tolerancesDrawing_Axis.SolidbrushDrawAdditionalValue = new SolidBrush(Graph_YAxis.tolerancesDrawing_Axis.ColorDrawAdditionalValueDisable);//绘制曲线图Y坐标轴上显示的附加数值所使用的画笔
                    Graph_YAxis.tolerancesDrawing_Axis.StringformatDrawValue.Alignment = StringAlignment.Center;//设置格式
                    Graph_YAxis.tolerancesDrawing_Axis.StringformatDrawValue.LineAlignment = StringAlignment.Center;//设置格式
                }
            }
            else//停止
            {
                //曲线图、标题文本为禁止状态

                solidbrushDrawCaption = new SolidBrush(colorDrawCaptionDisable);//绘制标题文本所使用的画刷

                //X坐标轴
                Graph_XAxis.tolerancesDrawing_Axis.PenDrawAxis = new Pen(Graph_XAxis.tolerancesDrawing_Axis.ColorDrawAxisDisable, Graph_XAxis.tolerancesDrawing_Axis.DrawAxisWidth);//绘制曲线坐标轴所使用的画笔
                Graph_XAxis.tolerancesDrawing_Axis.SolidbrushDrawAxisValue = new SolidBrush(Graph_XAxis.tolerancesDrawing_Axis.ColorDrawAxisValueDisable);//绘制曲线坐标轴数值所使用的画刷
                Graph_XAxis.tolerancesDrawing_Axis.PenDrawEffectiveMinLine = new Pen(Graph_XAxis.tolerancesDrawing_Axis.ColorDrawEffectiveLineDisable, Graph_XAxis.tolerancesDrawing_Axis.DrawEffectiveLineWidth);//绘制坐标轴最小有效值区域分界线所使用的画笔
                Graph_XAxis.tolerancesDrawing_Axis.PenDrawEffectiveMaxLine = new Pen(Graph_XAxis.tolerancesDrawing_Axis.ColorDrawEffectiveLineDisable, Graph_XAxis.tolerancesDrawing_Axis.DrawEffectiveLineWidth);//绘制坐标轴最大有效值区域分界线所使用的画笔
                Graph_XAxis.tolerancesDrawing_Axis.PenDrawEffectiveMinLine.DashStyle = DashStyle.Dash;//绘制坐标轴最小有效值区域分界线所使用的画笔
                Graph_XAxis.tolerancesDrawing_Axis.PenDrawEffectiveMaxLine.DashStyle = DashStyle.Dash;//绘制坐标轴最大有效值区域分界线所使用的画笔
                Graph_XAxis.tolerancesDrawing_Axis.SolidbrushDrawEffectiveMinValue = new SolidBrush(Graph_XAxis.tolerancesDrawing_Axis.ColorDrawEffectiveValueDisable);//绘制坐标轴最小有效值所使用的画刷
                Graph_XAxis.tolerancesDrawing_Axis.SolidbrushDrawEffectiveMaxValue = new SolidBrush(Graph_XAxis.tolerancesDrawing_Axis.ColorDrawEffectiveValueDisable);//绘制坐标轴最大有效值所使用的画刷
                Graph_XAxis.tolerancesDrawing_Axis.PenDrawAdditionalValueLine = new Pen(Graph_XAxis.tolerancesDrawing_Axis.ColorDrawAdditionalValueLineDisable, Graph_XAxis.tolerancesDrawing_Axis.DrawAdditionalValueLineWidth);//绘制曲线图坐标轴上显示的附加数值指示线所使用的画笔
                Graph_XAxis.tolerancesDrawing_Axis.SolidbrushDrawAdditionalValue = new SolidBrush(Graph_XAxis.tolerancesDrawing_Axis.ColorDrawAdditionalValueDisable);//绘制曲线图Y坐标轴上显示的附加数值所使用的画笔
                Graph_XAxis.tolerancesDrawing_Axis.StringformatDrawValue.Alignment = StringAlignment.Center;//设置格式
                Graph_XAxis.tolerancesDrawing_Axis.StringformatDrawValue.LineAlignment = StringAlignment.Center;//设置格式

                //Y坐标轴
                Graph_YAxis.tolerancesDrawing_Axis.PenDrawAxis = new Pen(Graph_YAxis.tolerancesDrawing_Axis.ColorDrawAxisDisable, Graph_YAxis.tolerancesDrawing_Axis.DrawAxisWidth);//绘制曲线坐标轴所使用的画笔
                Graph_YAxis.tolerancesDrawing_Axis.SolidbrushDrawAxisValue = new SolidBrush(Graph_YAxis.tolerancesDrawing_Axis.ColorDrawAxisValueDisable);//绘制曲线坐标轴数值所使用的画刷
                Graph_YAxis.tolerancesDrawing_Axis.PenDrawEffectiveMinLine = new Pen(Graph_YAxis.tolerancesDrawing_Axis.ColorDrawEffectiveLineDisable, Graph_YAxis.tolerancesDrawing_Axis.DrawEffectiveLineWidth);//绘制坐标轴最小有效值区域分界线所使用的画笔
                Graph_YAxis.tolerancesDrawing_Axis.PenDrawEffectiveMaxLine = new Pen(Graph_YAxis.tolerancesDrawing_Axis.ColorDrawEffectiveLineDisable, Graph_YAxis.tolerancesDrawing_Axis.DrawEffectiveLineWidth);//绘制坐标轴最大有效值区域分界线所使用的画笔
                Graph_YAxis.tolerancesDrawing_Axis.PenDrawEffectiveMinLine.DashStyle = DashStyle.Dash;//绘制坐标轴最小有效值区域分界线所使用的画笔
                Graph_YAxis.tolerancesDrawing_Axis.PenDrawEffectiveMaxLine.DashStyle = DashStyle.Dash;//绘制坐标轴最大有效值区域分界线所使用的画笔
                Graph_YAxis.tolerancesDrawing_Axis.SolidbrushDrawEffectiveMinValue = new SolidBrush(Graph_YAxis.tolerancesDrawing_Axis.ColorDrawEffectiveValueDisable);//绘制坐标轴最小有效值所使用的画刷
                Graph_YAxis.tolerancesDrawing_Axis.SolidbrushDrawEffectiveMaxValue = new SolidBrush(Graph_YAxis.tolerancesDrawing_Axis.ColorDrawEffectiveValueDisable);//绘制坐标轴最大有效值所使用的画刷
                Graph_YAxis.tolerancesDrawing_Axis.PenDrawAdditionalValueLine = new Pen(Graph_YAxis.tolerancesDrawing_Axis.ColorDrawAdditionalValueLineDisable, Graph_YAxis.tolerancesDrawing_Axis.DrawAdditionalValueLineWidth);//绘制曲线图坐标轴上显示的附加数值指示线所使用的画笔
                Graph_YAxis.tolerancesDrawing_Axis.SolidbrushDrawAdditionalValue = new SolidBrush(Graph_YAxis.tolerancesDrawing_Axis.ColorDrawAdditionalValueDisable);//绘制曲线图Y坐标轴上显示的附加数值所使用的画笔
                Graph_YAxis.tolerancesDrawing_Axis.StringformatDrawValue.Alignment = StringAlignment.Center;//设置格式
                Graph_YAxis.tolerancesDrawing_Axis.StringformatDrawValue.LineAlignment = StringAlignment.Center;//设置格式
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：绘制曲线图
        // 输入参数：1.graphicsDraw：绘图
        //         2.rectangleClient：客户区
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _Draw(Graphics graphicsDraw, Rectangle rectangleClient)
        {
            graphicsDraw.FillRectangle(solidbrushControl, rectangleClient);//绘制背景

            graphicsDraw.FillRectangle(solidbrushDrawGraph, rectGraph);//绘制控件的绘制区域

            //

            _DrawXAxis_Curve(graphicsDraw);//绘制X坐标轴
            _DrawYAxis_Curve(graphicsDraw);//绘制Y坐标轴

            _DrawBackground(graphicsDraw, rectangleClient);//绘制背景

            _DrawXAxis_Value(graphicsDraw);//绘制X坐标轴
            _DrawYAxis_Value(graphicsDraw);//绘制Y坐标轴

            //

            graphicsDraw.DrawString(
                sCaption, 
                fontCaption, 
                solidbrushDrawCaption, 
                new RectangleF(
                    Convert.ToSingle(rectCaption.Left),
                    Convert.ToSingle(rectCaption.Top),
                    Convert.ToSingle(rectCaption.Width),
                    Convert.ToSingle(rectCaption.Height)), 
                stringformatDrawCaption);//绘制标题文本

            //

            //_DrawXAxis(graphicsDraw);//绘制X坐标轴
            //_DrawYAxis(graphicsDraw);//绘制Y坐标轴
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：绘制X坐标轴
        // 输入参数：1.PaintEventArgs：画图事件
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _DrawXAxis_Curve(Graphics graphicsDraw)
        {
            int i = 0;//循环控制变量

            if (bGridDisplay)//显示网格
            {
                for (i = 1; i < Graph_XAxis.AxisValueNumber; i++)//绘制坐标轴数值
                {
                    graphicsDraw.DrawLine(
                        penGridLine,
                        Graph_XAxis.pointPixel_AxisEveryPointValue[i],
                        new Point(Graph_XAxis.pointPixel_AxisEveryPointValue[i].X, Graph_YAxis.LocationAxisMax_Pixel.Y));//坐标轴
                }
            }

            graphicsDraw.DrawLine(
                Graph_XAxis.tolerancesDrawing_Axis.PenDrawAxis,
                Graph_XAxis.rectPixel_AxisAllPointValue.Location,
                new Point(
                    Graph_XAxis.rectPixel_AxisAllPointValue.Right,
                    Graph_XAxis.rectPixel_AxisAllPointValue.Top));//坐标轴
        }

        //----------------------------------------------------------------------
        // 功能说明：绘制X坐标轴
        // 输入参数：1.PaintEventArgs：画图事件
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _DrawXAxis_Value(Graphics graphicsDraw)
        {
            int i = 0;//循环控制变量

            if (Graph_XAxis.AxisValueDisplay)//显示坐标轴数值
            {
                for (i = 0; i < Graph_XAxis.AxisValueNumber; i++)//绘制坐标轴数值
                {
                    graphicsDraw.DrawString(
                        Graph_XAxis.sPointValue[i],
                        Graph_XAxis.tolerancesDrawing_Axis.FontValue,
                        Graph_XAxis.tolerancesDrawing_Axis.SolidbrushDrawAxisValue,
                        new RectangleF(
                            Convert.ToSingle(Graph_XAxis.rectPixel_AxisEveryPointValue[i].Left),
                            Convert.ToSingle(Graph_XAxis.rectPixel_AxisEveryPointValue[i].Top),
                            Convert.ToSingle(Graph_XAxis.rectPixel_AxisEveryPointValue[i].Width),
                            Convert.ToSingle(Graph_XAxis.rectPixel_AxisEveryPointValue[i].Height)),
                        Graph_XAxis.tolerancesDrawing_Axis.StringformatDrawValue);//绘制坐标轴数值
                }
            }

            if (Graph_XAxis.AxisAdditionalValueDisplay)//显示附加数值
            {
                if (VisionSystemClassLibrary.Enum.AxisDataType.withoutEffectiveValue != Graph_XAxis.DataType)//指定了有效实际数值
                {
                    if (Graph_XAxis.pointPixel_EffectiveMinPoint.X >= Graph_XAxis.rectPixel_AxisAllPointValue.Left && Graph_XAxis.pointPixel_EffectiveMinPoint.X <= Graph_XAxis.rectPixel_AxisAllPointValue.Right
                        && Graph_XAxis.pointPixel_EffectiveMaxPoint.X >= Graph_XAxis.rectPixel_AxisAllPointValue.Left && Graph_XAxis.pointPixel_EffectiveMaxPoint.X <= Graph_XAxis.rectPixel_AxisAllPointValue.Right)
                    {
                        graphicsDraw.DrawLine(Graph_XAxis.tolerancesDrawing_Axis.PenDrawAdditionalValueLine,
                            new Point(Graph_XAxis.rectPixel_EffectiveMinPoint.Right,
                                (Graph_XAxis.rectPixel_EffectiveMinPoint.Top + Graph_XAxis.rectPixel_EffectiveMinPoint.Bottom) / 2),
                            new Point(Graph_XAxis.rectPixel_AdditionalValue.Left,
                                (Graph_XAxis.rectPixel_AdditionalValue.Top + Graph_XAxis.rectPixel_AdditionalValue.Bottom) / 2));//附加值指示曲线（左侧）

                        graphicsDraw.DrawLine(Graph_XAxis.tolerancesDrawing_Axis.PenDrawAdditionalValueLine,
                            new Point(Graph_XAxis.rectPixel_AdditionalValue.Right,
                                (Graph_XAxis.rectPixel_AdditionalValue.Top + Graph_XAxis.rectPixel_AdditionalValue.Bottom) / 2),
                            new Point(Graph_XAxis.rectPixel_EffectiveMaxPoint.Left,
                                (Graph_XAxis.rectPixel_EffectiveMaxPoint.Top + Graph_XAxis.rectPixel_EffectiveMaxPoint.Bottom) / 2));//附加值指示曲线（右侧）

                        graphicsDraw.DrawString(
                            Graph_XAxis.sAxisAdditionalValue + " " + Graph_XAxis.sAdditionalValueUnit,
                            Graph_XAxis.tolerancesDrawing_Axis.FontValue,
                            Graph_XAxis.tolerancesDrawing_Axis.SolidbrushDrawAdditionalValue,
                            new RectangleF(
                                Convert.ToSingle(Graph_XAxis.rectPixel_AdditionalValue.Left),
                                Convert.ToSingle(Graph_XAxis.rectPixel_AdditionalValue.Top),
                                Convert.ToSingle(Graph_XAxis.rectPixel_AdditionalValue.Width),
                                Convert.ToSingle(Graph_XAxis.rectPixel_AdditionalValue.Height)),
                            Graph_XAxis.tolerancesDrawing_Axis.StringformatDrawValue);//绘制坐标轴附加数值
                    }
                }
                else//未指定有效实际数值
                {
                    graphicsDraw.DrawLine(Graph_XAxis.tolerancesDrawing_Axis.PenDrawAdditionalValueLine,
                        new Point(Graph_XAxis.rectPixel_AxisEveryPointValue[0].Right,
                            (Graph_XAxis.rectPixel_AxisEveryPointValue[0].Top + Graph_XAxis.rectPixel_AxisEveryPointValue[0].Bottom) / 2),
                        new Point(Graph_XAxis.rectPixel_AdditionalValue.Left,
                            (Graph_XAxis.rectPixel_AdditionalValue.Top + Graph_XAxis.rectPixel_AdditionalValue.Bottom) / 2));//附加值指示曲线（左侧）

                    graphicsDraw.DrawLine(Graph_XAxis.tolerancesDrawing_Axis.PenDrawAdditionalValueLine,
                        new Point(Graph_XAxis.rectPixel_AdditionalValue.Right,
                            (Graph_XAxis.rectPixel_AdditionalValue.Top + Graph_XAxis.rectPixel_AdditionalValue.Bottom) / 2),
                        new Point(Graph_XAxis.rectPixel_AxisEveryPointValue[Graph_XAxis.AxisValueNumber - 1].Left,
                            (Graph_XAxis.rectPixel_AxisEveryPointValue[Graph_XAxis.AxisValueNumber - 1].Top + Graph_XAxis.rectPixel_AxisEveryPointValue[Graph_XAxis.AxisValueNumber - 1].Bottom) / 2));//附加值指示曲线（右侧）

                    graphicsDraw.DrawString(
                        Graph_XAxis.sAxisAdditionalValue + " " + Graph_XAxis.sAdditionalValueUnit,
                        Graph_XAxis.tolerancesDrawing_Axis.FontValue,
                        Graph_XAxis.tolerancesDrawing_Axis.SolidbrushDrawAdditionalValue,
                        new RectangleF(
                            Convert.ToSingle(Graph_XAxis.rectPixel_AdditionalValue.Left),
                            Convert.ToSingle(Graph_XAxis.rectPixel_AdditionalValue.Top),
                            Convert.ToSingle(Graph_XAxis.rectPixel_AdditionalValue.Width),
                            Convert.ToSingle(Graph_XAxis.rectPixel_AdditionalValue.Height)),
                        Graph_XAxis.tolerancesDrawing_Axis.StringformatDrawValue);//绘制坐标轴附加数值
                }
            }

            if (VisionSystemClassLibrary.Enum.AxisDataType.withoutEffectiveValue != Graph_XAxis.DataType)//指定了有效实际数值
            {
                //绘制有效实际值数值、指示线

                if (Graph_XAxis.pointPixel_EffectiveMinPoint.X >= Graph_XAxis.rectPixel_AxisAllPointValue.Left && Graph_XAxis.pointPixel_EffectiveMinPoint.X <= Graph_XAxis.rectPixel_AxisAllPointValue.Right)
                {
                    graphicsDraw.DrawString(
                        Graph_XAxis.sPixel_EffectiveMinPointValue,
                        Graph_XAxis.tolerancesDrawing_Axis.FontValue,
                        Graph_XAxis.tolerancesDrawing_Axis.SolidbrushDrawEffectiveMinValue,
                        new RectangleF(
                            Convert.ToSingle(Graph_XAxis.rectPixel_EffectiveMinPoint.Left),
                            Convert.ToSingle(Graph_XAxis.rectPixel_EffectiveMinPoint.Top),
                            Convert.ToSingle(Graph_XAxis.rectPixel_EffectiveMinPoint.Width),
                            Convert.ToSingle(Graph_XAxis.rectPixel_EffectiveMinPoint.Height)),
                        Graph_XAxis.tolerancesDrawing_Axis.StringformatDrawValue);//绘制坐标轴最小有效数值

                    graphicsDraw.DrawLine(Graph_XAxis.tolerancesDrawing_Axis.PenDrawEffectiveMinLine,
                        new Point((Graph_XAxis.rectPixel_EffectiveMinPoint.Left + Graph_XAxis.rectPixel_EffectiveMinPoint.Right) / 2,
                            Graph_XAxis.rectPixel_EffectiveMinPoint.Top),
                        new Point((Graph_XAxis.rectPixel_EffectiveMinPoint.Left + Graph_XAxis.rectPixel_EffectiveMinPoint.Right) / 2,
                            Graph_YAxis.rectPixel_AxisAllPointValue.Top));//绘制坐标轴最小有效数值指示线
                }

                if (Graph_XAxis.pointPixel_EffectiveMaxPoint.X >= Graph_XAxis.rectPixel_AxisAllPointValue.Left && Graph_XAxis.pointPixel_EffectiveMaxPoint.X <= Graph_XAxis.rectPixel_AxisAllPointValue.Right)
                {
                    graphicsDraw.DrawString(
                        Graph_XAxis.sPixel_EffectiveMaxPointValue,
                        Graph_XAxis.tolerancesDrawing_Axis.FontValue,
                        Graph_XAxis.tolerancesDrawing_Axis.SolidbrushDrawEffectiveMaxValue,
                        new RectangleF(
                            Convert.ToSingle(Graph_XAxis.rectPixel_EffectiveMaxPoint.Left),
                            Convert.ToSingle(Graph_XAxis.rectPixel_EffectiveMaxPoint.Top),
                            Convert.ToSingle(Graph_XAxis.rectPixel_EffectiveMaxPoint.Width),
                            Convert.ToSingle(Graph_XAxis.rectPixel_EffectiveMaxPoint.Height)),
                        Graph_XAxis.tolerancesDrawing_Axis.StringformatDrawValue);//绘制坐标轴最大有效数值

                    graphicsDraw.DrawLine(Graph_XAxis.tolerancesDrawing_Axis.PenDrawEffectiveMaxLine,
                        new Point((Graph_XAxis.rectPixel_EffectiveMaxPoint.Left + Graph_XAxis.rectPixel_EffectiveMaxPoint.Right) / 2,
                            Graph_XAxis.rectPixel_EffectiveMaxPoint.Top),
                        new Point((Graph_XAxis.rectPixel_EffectiveMaxPoint.Left + Graph_XAxis.rectPixel_EffectiveMaxPoint.Right) / 2,
                            Graph_YAxis.rectPixel_AxisAllPointValue.Top));//绘制坐标轴最大有效数值指示线
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：绘制X坐标轴数值、坐标轴、当前值
        // 输入参数：1.PaintEventArgs：画图事件
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _DrawXAxis(Graphics graphicsDraw)
        {
            int i = 0;//循环控制变量

            if (bGridDisplay)//显示网格
            {
                for (i = 1; i < Graph_XAxis.AxisValueNumber; i++)//绘制坐标轴数值
                {
                    graphicsDraw.DrawLine(
                        penGridLine,
                        Graph_XAxis.pointPixel_AxisEveryPointValue[i],
                        new Point(Graph_XAxis.pointPixel_AxisEveryPointValue[i].X, Graph_YAxis.LocationAxisMax_Pixel.Y));//坐标轴
                }
            }

            graphicsDraw.DrawLine(
                Graph_XAxis.tolerancesDrawing_Axis.PenDrawAxis,
                Graph_XAxis.rectPixel_AxisAllPointValue.Location,
                new Point(
                    Graph_XAxis.rectPixel_AxisAllPointValue.Right,
                    Graph_XAxis.rectPixel_AxisAllPointValue.Top));//坐标轴

            if (Graph_XAxis.AxisValueDisplay)//显示坐标轴数值
            {
                for (i = 0; i < Graph_XAxis.AxisValueNumber; i++)//绘制坐标轴数值
                {
                    graphicsDraw.DrawString(
                        Graph_XAxis.sPointValue[i],
                        Graph_XAxis.tolerancesDrawing_Axis.FontValue,
                        Graph_XAxis.tolerancesDrawing_Axis.SolidbrushDrawAxisValue,
                        new RectangleF(
                            Convert.ToSingle(Graph_XAxis.rectPixel_AxisEveryPointValue[i].Left),
                            Convert.ToSingle(Graph_XAxis.rectPixel_AxisEveryPointValue[i].Top),
                            Convert.ToSingle(Graph_XAxis.rectPixel_AxisEveryPointValue[i].Width),
                            Convert.ToSingle(Graph_XAxis.rectPixel_AxisEveryPointValue[i].Height)),
                        Graph_XAxis.tolerancesDrawing_Axis.StringformatDrawValue);//绘制坐标轴数值
                }
            }

            if (Graph_XAxis.AxisAdditionalValueDisplay)//显示附加数值
            {
                graphicsDraw.DrawString(
                    Graph_XAxis.sAxisAdditionalValue + " " + Graph_XAxis.sAdditionalValueUnit,
                    Graph_XAxis.tolerancesDrawing_Axis.FontValue,
                    Graph_XAxis.tolerancesDrawing_Axis.SolidbrushDrawAdditionalValue,
                    new RectangleF(
                        Convert.ToSingle(Graph_XAxis.rectPixel_AdditionalValue.Left),
                        Convert.ToSingle(Graph_XAxis.rectPixel_AdditionalValue.Top),
                        Convert.ToSingle(Graph_XAxis.rectPixel_AdditionalValue.Width),
                        Convert.ToSingle(Graph_XAxis.rectPixel_AdditionalValue.Height)),
                    Graph_XAxis.tolerancesDrawing_Axis.StringformatDrawValue);//绘制坐标轴附加数值

                if (VisionSystemClassLibrary.Enum.AxisDataType.withoutEffectiveValue != Graph_XAxis.DataType)//指定了有效实际数值
                {
                    graphicsDraw.DrawLine(Graph_XAxis.tolerancesDrawing_Axis.PenDrawAdditionalValueLine,
                        new Point(Graph_XAxis.rectPixel_EffectiveMinPoint.Right,
                            (Graph_XAxis.rectPixel_EffectiveMinPoint.Top + Graph_XAxis.rectPixel_EffectiveMinPoint.Bottom) / 2),
                        new Point(Graph_XAxis.rectPixel_AdditionalValue.Left,
                            (Graph_XAxis.rectPixel_AdditionalValue.Top + Graph_XAxis.rectPixel_AdditionalValue.Bottom) / 2));//附加值指示曲线（左侧）

                    graphicsDraw.DrawLine(Graph_XAxis.tolerancesDrawing_Axis.PenDrawAdditionalValueLine,
                        new Point(Graph_XAxis.rectPixel_AdditionalValue.Right,
                            (Graph_XAxis.rectPixel_AdditionalValue.Top + Graph_XAxis.rectPixel_AdditionalValue.Bottom) / 2),
                        new Point(Graph_XAxis.rectPixel_EffectiveMaxPoint.Left,
                            (Graph_XAxis.rectPixel_EffectiveMaxPoint.Top + Graph_XAxis.rectPixel_EffectiveMaxPoint.Bottom) / 2));//附加值指示曲线（右侧）
                }
                else//未指定有效实际数值
                {
                    graphicsDraw.DrawLine(Graph_XAxis.tolerancesDrawing_Axis.PenDrawAdditionalValueLine,
                        new Point(Graph_XAxis.rectPixel_AxisEveryPointValue[0].Right,
                            (Graph_XAxis.rectPixel_AxisEveryPointValue[0].Top + Graph_XAxis.rectPixel_AxisEveryPointValue[0].Bottom) / 2),
                        new Point(Graph_XAxis.rectPixel_AdditionalValue.Left,
                            (Graph_XAxis.rectPixel_AdditionalValue.Top + Graph_XAxis.rectPixel_AdditionalValue.Bottom) / 2));//附加值指示曲线（左侧）

                    graphicsDraw.DrawLine(Graph_XAxis.tolerancesDrawing_Axis.PenDrawAdditionalValueLine,
                        new Point(Graph_XAxis.rectPixel_AdditionalValue.Right,
                            (Graph_XAxis.rectPixel_AdditionalValue.Top + Graph_XAxis.rectPixel_AdditionalValue.Bottom) / 2),
                        new Point(Graph_XAxis.rectPixel_AxisEveryPointValue[Graph_XAxis.AxisValueNumber - 1].Left,
                            (Graph_XAxis.rectPixel_AxisEveryPointValue[Graph_XAxis.AxisValueNumber - 1].Top + Graph_XAxis.rectPixel_AxisEveryPointValue[Graph_XAxis.AxisValueNumber - 1].Bottom) / 2));//附加值指示曲线（右侧）
                }
            }

            if (VisionSystemClassLibrary.Enum.AxisDataType.withoutEffectiveValue != Graph_XAxis.DataType)//指定了有效实际数值
            {
                //绘制有效实际值数值、指示线

                graphicsDraw.DrawString(
                    Graph_XAxis.sPixel_EffectiveMinPointValue,
                    Graph_XAxis.tolerancesDrawing_Axis.FontValue,
                    Graph_XAxis.tolerancesDrawing_Axis.SolidbrushDrawEffectiveMinValue,
                    new RectangleF(
                        Convert.ToSingle(Graph_XAxis.rectPixel_EffectiveMinPoint.Left),
                        Convert.ToSingle(Graph_XAxis.rectPixel_EffectiveMinPoint.Top),
                        Convert.ToSingle(Graph_XAxis.rectPixel_EffectiveMinPoint.Width),
                        Convert.ToSingle(Graph_XAxis.rectPixel_EffectiveMinPoint.Height)),
                    Graph_XAxis.tolerancesDrawing_Axis.StringformatDrawValue);//绘制坐标轴最小有效数值

                graphicsDraw.DrawString(
                    Graph_XAxis.sPixel_EffectiveMaxPointValue,
                    Graph_XAxis.tolerancesDrawing_Axis.FontValue,
                    Graph_XAxis.tolerancesDrawing_Axis.SolidbrushDrawEffectiveMaxValue,
                    new RectangleF(
                        Convert.ToSingle(Graph_XAxis.rectPixel_EffectiveMaxPoint.Left),
                        Convert.ToSingle(Graph_XAxis.rectPixel_EffectiveMaxPoint.Top),
                        Convert.ToSingle(Graph_XAxis.rectPixel_EffectiveMaxPoint.Width),
                        Convert.ToSingle(Graph_XAxis.rectPixel_EffectiveMaxPoint.Height)),
                    Graph_XAxis.tolerancesDrawing_Axis.StringformatDrawValue);//绘制坐标轴最大有效数值

                graphicsDraw.DrawLine(Graph_XAxis.tolerancesDrawing_Axis.PenDrawEffectiveMinLine,
                    new Point((Graph_XAxis.rectPixel_EffectiveMinPoint.Left + Graph_XAxis.rectPixel_EffectiveMinPoint.Right) / 2,
                        Graph_XAxis.rectPixel_EffectiveMinPoint.Top),
                    new Point((Graph_XAxis.rectPixel_EffectiveMinPoint.Left + Graph_XAxis.rectPixel_EffectiveMinPoint.Right) / 2,
                        Graph_YAxis.rectPixel_AxisAllPointValue.Top));//绘制坐标轴最小有效数值指示线

                graphicsDraw.DrawLine(Graph_XAxis.tolerancesDrawing_Axis.PenDrawEffectiveMaxLine,
                    new Point((Graph_XAxis.rectPixel_EffectiveMaxPoint.Left + Graph_XAxis.rectPixel_EffectiveMaxPoint.Right) / 2,
                        Graph_XAxis.rectPixel_EffectiveMaxPoint.Top),
                    new Point((Graph_XAxis.rectPixel_EffectiveMaxPoint.Left + Graph_XAxis.rectPixel_EffectiveMaxPoint.Right) / 2,
                        Graph_YAxis.rectPixel_AxisAllPointValue.Top));//绘制坐标轴最小有效数值指示线
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：绘制背景
        // 输入参数：1.PaintEventArgs：画图事件
        //         2.graphicsDraw：绘图
        //         3.rectangleClient：客户区
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _DrawBackground(Graphics graphicsDraw, Rectangle rectangleClient)
        {
            graphicsDraw.FillRectangle(solidbrushControl, new Rectangle(rectangleClient.Location, new Size(rectGraph.Left - rectangleClient.Left, rectangleClient.Height)));//绘制背景
            graphicsDraw.FillRectangle(solidbrushControl, new Rectangle(rectangleClient.Location, new Size(rectangleClient.Width, rectGraph.Top - rectangleClient.Top)));//绘制背景
            graphicsDraw.FillRectangle(solidbrushControl, new Rectangle(new Point(rectGraph.Right, rectangleClient.Top), new Size(rectangleClient.Right - rectGraph.Right, rectangleClient.Height)));//绘制背景
            graphicsDraw.FillRectangle(solidbrushControl, new Rectangle(new Point(rectangleClient.Left, rectGraph.Bottom), new Size(rectangleClient.Width, rectangleClient.Bottom - rectGraph.Bottom)));//绘制背景

            graphicsDraw.FillRectangle(solidbrushDrawGraph, new Rectangle(rectGraph.Location, new Size(Graph_YAxis.rectPixel_AxisAllPointValue.Right - (Int32)(Graph_YAxis.tolerancesDrawing_Axis.DrawAxisWidth / 2) - rectGraph.Left, rectGraph.Height)));//绘制绘制区域
            graphicsDraw.FillRectangle(solidbrushDrawGraph, new Rectangle(rectGraph.Location, new Size(rectGraph.Width, Graph_YAxis.rectPixel_AxisAllPointValue.Top - rectGraph.Top)));//绘制绘制区域
            graphicsDraw.FillRectangle(solidbrushDrawGraph, new Rectangle(new Point(Graph_XAxis.rectPixel_AxisAllPointValue.Right, rectGraph.Top), new Size(rectGraph.Right - Graph_XAxis.rectPixel_AxisAllPointValue.Right, rectGraph.Height)));//绘制绘制区域
            graphicsDraw.FillRectangle(solidbrushDrawGraph, new Rectangle(new Point(rectGraph.Left, Graph_XAxis.rectPixel_AxisAllPointValue.Top + 1), new Size(rectGraph.Width, rectGraph.Bottom - (Graph_XAxis.rectPixel_AxisAllPointValue.Top + (Int32)(Graph_XAxis.tolerancesDrawing_Axis.DrawAxisWidth / 2)))));//绘制绘制区域
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：绘制Y坐标轴
        // 输入参数：1.PaintEventArgs：画图事件
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _DrawYAxis_Curve(Graphics graphicsDraw)
        {
            int i = 0;//循环控制变量
            int j = 0;//循环控制变量

            if (bGridDisplay)//显示网格
            {
                for (i = 1; i < Graph_YAxis.AxisValueNumber; i++)//绘制坐标轴数值
                {
                    graphicsDraw.DrawLine(
                        penGridLine,
                        Graph_YAxis.pointPixel_AxisEveryPointValue[i],
                        new Point(Graph_XAxis.LocationAxisMax_Pixel.X, Graph_YAxis.pointPixel_AxisEveryPointValue[i].Y));//坐标轴
                }
            }

            graphicsDraw.DrawLine(
                Graph_YAxis.tolerancesDrawing_Axis.PenDrawAxis,
                new Point(
                    Graph_YAxis.rectPixel_AxisAllPointValue.Right,
                    Graph_YAxis.rectPixel_AxisAllPointValue.Bottom),
                new Point(
                    Graph_YAxis.rectPixel_AxisAllPointValue.Right,
                    Graph_YAxis.rectPixel_AxisAllPointValue.Top));//坐标轴

            //

            if (VisionSystemClassLibrary.Enum.AxisDataType.withoutEffectiveValue != Graph_YAxis.DataType)//指定了有效实际数值
            {
                //绘制当前值、（Y）坐标轴曲线

                if (Control_Data.RunorStop)//运行
                {
                    if (iCurrentValueIndex[0] >= 0 && iCurrentValueIndex[0] < iValueNumber_Curve)//显示当前值、（Y）坐标轴曲线
                    {
                        //（Y）坐标轴曲线

                        for (i = 0; i < iCurrentValueIndex.Length; i++)
                        {
                            if (bCurveDisplay[i])//显示
                            {
                                for (j = 0; j < iCurrentValueIndex[i]; j++)//绘制曲线数值
                                {
                                    graphicsDraw.DrawLine(penLine[i], pointPixelValue[i][j], pointPixelValue[i][j + 1]);
                                }
                            }

                            if (bValuePointDisplay)//显示
                            {
                                for (j = 0; j <= iCurrentValueIndex[i]; j++)//绘制曲线数值
                                {
                                    graphicsDraw.FillEllipse(solidbrushValue[i][j], pointPixelValue[i][j].X - (iValuePointPixelWidth - 1) / 2, pointPixelValue[i][j].Y - (iValuePointPixelWidth - 1) / 2, iValuePointPixelWidth, iValuePointPixelWidth);
                                }
                            }
                        }
                    }
                }
            }
            else//未指定有效实际数值
            {
                //绘制当前值、（Y）坐标轴曲线

                if (Control_Data.RunorStop)//运行
                {
                    if (iCurrentValueIndex[0] >= 0 && iCurrentValueIndex[0] < iValueNumber_Curve)//显示当前值、（Y）坐标轴曲线
                    {
                        //（Y）坐标轴曲线

                        for (i = 0; i < iCurrentValueIndex.Length; i++)
                        {
                            if (bCurveDisplay[i])//显示
                            {
                                for (j = 0; j < iCurrentValueIndex[i]; j++)//绘制曲线数值
                                {
                                    graphicsDraw.DrawLine(penLine[i], pointPixelValue[i][j], pointPixelValue[i][j + 1]);
                                }
                            }
                        }
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：绘制Y坐标轴
        // 输入参数：1.PaintEventArgs：画图事件
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _DrawYAxis_Value(Graphics graphicsDraw)
        {
            int i = 0;//循环控制变量
            int j = 0;//循环控制变量

            if (Graph_YAxis.AxisAdditionalValueDisplay)//显示附加数值
            {
                if (VisionSystemClassLibrary.Enum.AxisDataType.withoutEffectiveValue != Graph_YAxis.DataType)//指定了有效实际数值
                {
                    if (Graph_YAxis.pointPixel_EffectiveMinPoint.Y >= Graph_YAxis.rectPixel_AxisAllPointValue.Top && Graph_YAxis.pointPixel_EffectiveMinPoint.Y <= Graph_YAxis.rectPixel_AxisAllPointValue.Bottom
                        && Graph_YAxis.pointPixel_EffectiveMaxPoint.Y >= Graph_YAxis.rectPixel_AxisAllPointValue.Top && Graph_YAxis.pointPixel_EffectiveMaxPoint.Y <= Graph_YAxis.rectPixel_AxisAllPointValue.Bottom)
                    {
                        graphicsDraw.DrawLine(Graph_YAxis.tolerancesDrawing_Axis.PenDrawAdditionalValueLine,
                            new Point((Graph_YAxis.rectPixel_EffectiveMinPoint.Left + Graph_YAxis.rectPixel_EffectiveMinPoint.Right) / 2,
                                Graph_YAxis.rectPixel_EffectiveMinPoint.Top),
                            new Point((Graph_YAxis.rectPixel_AdditionalValue.Left + Graph_YAxis.rectPixel_AdditionalValue.Right) / 2,
                                Graph_YAxis.rectPixel_AdditionalValue.Bottom));//附加值指示曲线（下部）

                        graphicsDraw.DrawLine(Graph_YAxis.tolerancesDrawing_Axis.PenDrawAdditionalValueLine,
                            new Point((Graph_YAxis.rectPixel_AdditionalValue.Left + Graph_YAxis.rectPixel_AdditionalValue.Right) / 2,
                                Graph_YAxis.rectPixel_AdditionalValue.Top),//附加值指示曲线（下部）
                            new Point((Graph_YAxis.rectPixel_EffectiveMaxPoint.Left + Graph_YAxis.rectPixel_EffectiveMaxPoint.Right) / 2,
                                Graph_YAxis.rectPixel_EffectiveMaxPoint.Bottom));//附加值指示曲线（上部）

                        graphicsDraw.DrawString(
                            Graph_YAxis.sAxisAdditionalValue,
                            Graph_YAxis.tolerancesDrawing_Axis.FontValue,
                            Graph_YAxis.tolerancesDrawing_Axis.SolidbrushDrawAdditionalValue,
                            new RectangleF(
                                Convert.ToSingle(Graph_YAxis.rectPixel_AdditionalValue.Left),
                                Convert.ToSingle(Graph_YAxis.rectPixel_AdditionalValue.Top),
                                Convert.ToSingle(Graph_YAxis.rectPixel_AdditionalValue.Width),
                                Convert.ToSingle(Graph_YAxis.rectPixel_AdditionalValue.Height / 2)),
                            Graph_YAxis.tolerancesDrawing_Axis.StringformatDrawValue);//绘制坐标轴附加数值

                        graphicsDraw.DrawString(
                            Graph_YAxis.sAdditionalValueUnit,
                            Graph_YAxis.tolerancesDrawing_Axis.FontValue,
                            Graph_YAxis.tolerancesDrawing_Axis.SolidbrushDrawAdditionalValue,
                            new RectangleF(
                                Convert.ToSingle(Graph_YAxis.rectPixel_AdditionalValue.Left),
                                Convert.ToSingle(Graph_YAxis.rectPixel_AdditionalValue.Top + Graph_YAxis.rectPixel_AdditionalValue.Height / 2),
                                Convert.ToSingle(Graph_YAxis.rectPixel_AdditionalValue.Width),
                                Convert.ToSingle(Graph_YAxis.rectPixel_AdditionalValue.Height / 2)),
                            Graph_YAxis.tolerancesDrawing_Axis.StringformatDrawValue);//绘制坐标轴附加数值
                    }
                }
                else//未指定有效实际数值
                {
                    graphicsDraw.DrawLine(Graph_YAxis.tolerancesDrawing_Axis.PenDrawAdditionalValueLine,
                        new Point((Graph_YAxis.rectPixel_AxisEveryPointValue[0].Left + Graph_YAxis.rectPixel_AxisEveryPointValue[0].Right) / 2,
                            Graph_YAxis.rectPixel_AxisEveryPointValue[0].Top),
                        new Point((Graph_YAxis.rectPixel_AdditionalValue.Left + Graph_YAxis.rectPixel_AdditionalValue.Right) / 2,
                            Graph_YAxis.rectPixel_AdditionalValue.Bottom));//附加值指示曲线（下部）

                    graphicsDraw.DrawLine(Graph_YAxis.tolerancesDrawing_Axis.PenDrawAdditionalValueLine,
                        new Point((Graph_YAxis.rectPixel_AdditionalValue.Left + Graph_YAxis.rectPixel_AdditionalValue.Right) / 2,
                            Graph_YAxis.rectPixel_AdditionalValue.Top),//附加值指示曲线（下部）
                        new Point((Graph_YAxis.rectPixel_AxisEveryPointValue[Graph_YAxis.AxisValueNumber - 1].Left + Graph_YAxis.rectPixel_AxisEveryPointValue[Graph_YAxis.AxisValueNumber - 1].Right) / 2,
                            Graph_YAxis.rectPixel_AxisEveryPointValue[Graph_YAxis.AxisValueNumber - 1].Bottom));//附加值指示曲线（上部）

                    graphicsDraw.DrawString(
                        Graph_YAxis.sAxisAdditionalValue,
                        Graph_YAxis.tolerancesDrawing_Axis.FontValue,
                        Graph_YAxis.tolerancesDrawing_Axis.SolidbrushDrawAdditionalValue,
                        new RectangleF(
                            Convert.ToSingle(Graph_YAxis.rectPixel_AdditionalValue.Left),
                            Convert.ToSingle(Graph_YAxis.rectPixel_AdditionalValue.Top),
                            Convert.ToSingle(Graph_YAxis.rectPixel_AdditionalValue.Width),
                            Convert.ToSingle(Graph_YAxis.rectPixel_AdditionalValue.Height / 2)),
                        Graph_YAxis.tolerancesDrawing_Axis.StringformatDrawValue);//绘制坐标轴附加数值

                    graphicsDraw.DrawString(
                        Graph_YAxis.sAdditionalValueUnit,
                        Graph_YAxis.tolerancesDrawing_Axis.FontValue,
                        Graph_YAxis.tolerancesDrawing_Axis.SolidbrushDrawAdditionalValue,
                        new RectangleF(
                            Convert.ToSingle(Graph_YAxis.rectPixel_AdditionalValue.Left),
                            Convert.ToSingle(Graph_YAxis.rectPixel_AdditionalValue.Top + Graph_YAxis.rectPixel_AdditionalValue.Height / 2),
                            Convert.ToSingle(Graph_YAxis.rectPixel_AdditionalValue.Width),
                            Convert.ToSingle(Graph_YAxis.rectPixel_AdditionalValue.Height / 2)),
                        Graph_YAxis.tolerancesDrawing_Axis.StringformatDrawValue);//绘制坐标轴附加数值
                }
            }

            //

            if (VisionSystemClassLibrary.Enum.AxisDataType.withoutEffectiveValue != Graph_YAxis.DataType)//指定了有效实际数值
            {
                //绘制坐标轴数值

                if (Graph_YAxis.AxisValueDisplay)//显示坐标轴数值
                {
                    for (i = 0; i < Graph_YAxis.AxisValueNumber; i++)//绘制坐标轴数值
                    {
                        graphicsDraw.DrawString(
                            Graph_YAxis.sPointValue[i],
                            Graph_YAxis.tolerancesDrawing_Axis.FontValue,
                            Graph_YAxis.tolerancesDrawing_Axis.SolidbrushDrawAxisValue,
                            new RectangleF(
                                Convert.ToSingle(Graph_YAxis.rectPixel_AxisEveryPointValue[i].Left),
                                Convert.ToSingle(Graph_YAxis.rectPixel_AxisEveryPointValue[i].Top),
                                Convert.ToSingle(Graph_YAxis.rectPixel_AxisEveryPointValue[i].Width),
                                Convert.ToSingle(Graph_YAxis.rectPixel_AxisEveryPointValue[i].Height)),
                            Graph_YAxis.tolerancesDrawing_Axis.StringformatDrawValue);//绘制坐标轴数值
                    }
                }

                //绘制有效实际值数值、指示线

                if (Control_Data.TolerancesTools.EffectiveMin_State &&  Graph_YAxis.pointPixel_EffectiveMinPoint.Y >= Graph_YAxis.rectPixel_AxisAllPointValue.Top && Graph_YAxis.pointPixel_EffectiveMinPoint.Y <= Graph_YAxis.rectPixel_AxisAllPointValue.Bottom)
                {
                    graphicsDraw.DrawString(
                        Graph_YAxis.sPixel_EffectiveMinPointValue,
                        Graph_YAxis.tolerancesDrawing_Axis.FontValue,
                        Graph_YAxis.tolerancesDrawing_Axis.SolidbrushDrawEffectiveMinValue,
                        new RectangleF(
                            Convert.ToSingle(Graph_YAxis.rectPixel_EffectiveMinPoint.Left),
                            Convert.ToSingle(Graph_YAxis.rectPixel_EffectiveMinPoint.Top),
                            Convert.ToSingle(Graph_YAxis.rectPixel_EffectiveMinPoint.Width),
                            Convert.ToSingle(Graph_YAxis.rectPixel_EffectiveMinPoint.Height)),
                        Graph_YAxis.tolerancesDrawing_Axis.StringformatDrawValue);//绘制坐标轴最小有效数值

                    graphicsDraw.DrawLine(Graph_YAxis.tolerancesDrawing_Axis.PenDrawEffectiveMinLine,
                        new Point(Graph_YAxis.rectPixel_EffectiveMinPoint.Right,
                            (Graph_YAxis.rectPixel_EffectiveMinPoint.Top + Graph_YAxis.rectPixel_EffectiveMinPoint.Bottom) / 2),
                        new Point(Graph_XAxis.rectPixel_AxisAllPointValue.Right,
                            (Graph_YAxis.rectPixel_EffectiveMinPoint.Top + Graph_YAxis.rectPixel_EffectiveMinPoint.Bottom) / 2));//绘制坐标轴最小有效数值指示线
                }

                if (Control_Data.TolerancesTools.EffectiveMax_State &&  Graph_YAxis.pointPixel_EffectiveMaxPoint.Y >= Graph_YAxis.rectPixel_AxisAllPointValue.Top && Graph_YAxis.pointPixel_EffectiveMaxPoint.Y <= Graph_YAxis.rectPixel_AxisAllPointValue.Bottom)
                {
                    graphicsDraw.DrawString(
                        Graph_YAxis.sPixel_EffectiveMaxPointValue,
                        Graph_YAxis.tolerancesDrawing_Axis.FontValue,
                        Graph_YAxis.tolerancesDrawing_Axis.SolidbrushDrawEffectiveMaxValue,
                        new RectangleF(
                            Convert.ToSingle(Graph_YAxis.rectPixel_EffectiveMaxPoint.Left),
                            Convert.ToSingle(Graph_YAxis.rectPixel_EffectiveMaxPoint.Top),
                            Convert.ToSingle(Graph_YAxis.rectPixel_EffectiveMaxPoint.Width),
                            Convert.ToSingle(Graph_YAxis.rectPixel_EffectiveMaxPoint.Height)),
                        Graph_YAxis.tolerancesDrawing_Axis.StringformatDrawValue);//绘制坐标轴最大有效数值

                    graphicsDraw.DrawLine(Graph_YAxis.tolerancesDrawing_Axis.PenDrawEffectiveMaxLine,
                        new Point(Graph_YAxis.rectPixel_EffectiveMaxPoint.Right,
                            (Graph_YAxis.rectPixel_EffectiveMaxPoint.Top + Graph_YAxis.rectPixel_EffectiveMaxPoint.Bottom) / 2),
                        new Point(Graph_XAxis.rectPixel_AxisAllPointValue.Right,
                            (Graph_YAxis.rectPixel_EffectiveMaxPoint.Top + Graph_YAxis.rectPixel_EffectiveMaxPoint.Bottom) / 2));//绘制坐标轴最大有效数值指示线
                }

                //绘制当前值、（Y）坐标轴曲线

                if (Control_Data.RunorStop)//运行
                {
                    if (iCurrentValueIndex[0] >= 0 && iCurrentValueIndex[0] < iValueNumber_Curve)//显示当前值、（Y）坐标轴曲线
                    {
                        //当前值

                        graphicsDraw.DrawString(
                            sCurValue,
                            FontCurValue,
                            solidbrushDrawCurValue,
                            new RectangleF(
                                Convert.ToSingle(rectPixel_CurValue.Left),
                                Convert.ToSingle(rectPixel_CurValue.Top),
                                Convert.ToSingle(rectPixel_CurValue.Width),
                                Convert.ToSingle(rectPixel_CurValue.Height)),
                            stringformatDrawCurValue);//绘制坐标轴当前值

                        if ("" != sCurValue)//不显示
                        {
                            graphicsDraw.FillEllipse(solidbrushDrawCurValueIcon, rectPixel_CurValueIcon);//绘制当前值图标
                        }

                        //（Y）坐标轴曲线

                        for (i = 0; i < iCurrentValueIndex.Length; i++)
                        {
                            if (bValuePointDisplay)//显示
                            {
                                for (j = 0; j <= iCurrentValueIndex[i]; j++)//绘制曲线数值
                                {
                                    graphicsDraw.FillEllipse(solidbrushValue[i][j], pointPixelValue[i][j].X - (iValuePointPixelWidth - 1) / 2, pointPixelValue[i][j].Y - (iValuePointPixelWidth - 1) / 2, iValuePointPixelWidth, iValuePointPixelWidth);
                                }
                            }
                        }
                    }
                }
            }
            else//未指定有效实际数值
            {
                //绘制坐标轴数值

                if (Graph_YAxis.AxisValueDisplay)//显示坐标轴数值
                {
                    for (i = 0; i < Graph_YAxis.AxisValueNumber; i++)//绘制坐标轴数值
                    {
                        graphicsDraw.DrawString(
                            Graph_YAxis.sPointValue[i],
                            Graph_YAxis.tolerancesDrawing_Axis.FontValue,
                            Graph_YAxis.tolerancesDrawing_Axis.SolidbrushDrawAxisValue,
                            new RectangleF(
                                Convert.ToSingle(Graph_YAxis.rectPixel_AxisEveryPointValue[i].Left),
                                Convert.ToSingle(Graph_YAxis.rectPixel_AxisEveryPointValue[i].Top),
                                Convert.ToSingle(Graph_YAxis.rectPixel_AxisEveryPointValue[i].Width),
                                Convert.ToSingle(Graph_YAxis.rectPixel_AxisEveryPointValue[i].Height)),
                            Graph_YAxis.tolerancesDrawing_Axis.StringformatDrawValue);//绘制坐标轴数值
                    }
                }

                //绘制当前值、（Y）坐标轴曲线

                if (Control_Data.RunorStop)//运行
                {
                    if (iCurrentValueIndex[0] >= 0 && iCurrentValueIndex[0] < iValueNumber_Curve)//显示当前值、（Y）坐标轴曲线
                    {
                        //当前值

                        graphicsDraw.DrawString(
                            sCurValue,
                            fontCurValue,
                            solidbrushDrawCurValue,
                            new RectangleF(
                                Convert.ToSingle(rectPixel_CurValue.Left),
                                Convert.ToSingle(rectPixel_CurValue.Top),
                                Convert.ToSingle(rectPixel_CurValue.Width),
                                Convert.ToSingle(rectPixel_CurValue.Height)),
                            stringformatDrawCurValue);//绘制坐标轴当前值

                        if ("" != sCurValue)//不显示
                        {
                            graphicsDraw.FillEllipse(solidbrushDrawCurValueIcon, rectPixel_CurValueIcon);//绘制当前值图标
                        }

                        //（Y）坐标轴曲线

                        for (i = 0; i < iCurrentValueIndex.Length; i++)
                        {
                            if (bValuePointDisplay)//显示
                            {
                                for (j = 0; j <= iCurrentValueIndex[i]; j++)//绘制曲线数值
                                {
                                    graphicsDraw.FillEllipse(solidbrushValue[i][j], pointPixelValue[i][j].X - (iValuePointPixelWidth - 1) / 2, pointPixelValue[i][j].Y - (iValuePointPixelWidth - 1) / 2, iValuePointPixelWidth, iValuePointPixelWidth);
                                }
                            }
                        }
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：绘制Y坐标轴
        // 输入参数：1.PaintEventArgs：画图事件
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _DrawYAxis(Graphics graphicsDraw)
        {
            int i = 0;//循环控制变量
            int j = 0;//循环控制变量

            if (bGridDisplay)//显示网格
            {
                for (i = 1; i < Graph_YAxis.AxisValueNumber; i++)//绘制坐标轴数值
                {
                    graphicsDraw.DrawLine(
                        penGridLine,
                        Graph_YAxis.pointPixel_AxisEveryPointValue[i],
                        new Point(Graph_XAxis.LocationAxisMax_Pixel.X, Graph_YAxis.pointPixel_AxisEveryPointValue[i].Y));//坐标轴
                }
            }

            graphicsDraw.DrawLine(
                Graph_YAxis.tolerancesDrawing_Axis.PenDrawAxis,
                new Point(
                    Graph_YAxis.rectPixel_AxisAllPointValue.Right,
                    Graph_YAxis.rectPixel_AxisAllPointValue.Bottom),
                new Point(
                    Graph_YAxis.rectPixel_AxisAllPointValue.Right,
                    Graph_YAxis.rectPixel_AxisAllPointValue.Top));//坐标轴

            if (Graph_YAxis.AxisAdditionalValueDisplay)//显示附加数值
            {
                graphicsDraw.DrawString(
                    Graph_YAxis.sAxisAdditionalValue,
                    Graph_YAxis.tolerancesDrawing_Axis.FontValue,
                    Graph_YAxis.tolerancesDrawing_Axis.SolidbrushDrawAdditionalValue,
                    new RectangleF(
                        Convert.ToSingle(Graph_YAxis.rectPixel_AdditionalValue.Left),
                        Convert.ToSingle(Graph_YAxis.rectPixel_AdditionalValue.Top),
                        Convert.ToSingle(Graph_YAxis.rectPixel_AdditionalValue.Width),
                        Convert.ToSingle(Graph_YAxis.rectPixel_AdditionalValue.Height / 2)),
                    Graph_YAxis.tolerancesDrawing_Axis.StringformatDrawValue);//绘制坐标轴附加数值

                graphicsDraw.DrawString(
                    Graph_YAxis.sAdditionalValueUnit,
                    Graph_YAxis.tolerancesDrawing_Axis.FontValue,
                    Graph_YAxis.tolerancesDrawing_Axis.SolidbrushDrawAdditionalValue,
                    new RectangleF(
                        Convert.ToSingle(Graph_YAxis.rectPixel_AdditionalValue.Left),
                        Convert.ToSingle(Graph_YAxis.rectPixel_AdditionalValue.Top + Graph_YAxis.rectPixel_AdditionalValue.Height / 2),
                        Convert.ToSingle(Graph_YAxis.rectPixel_AdditionalValue.Width),
                        Convert.ToSingle(Graph_YAxis.rectPixel_AdditionalValue.Height / 2)),
                    Graph_YAxis.tolerancesDrawing_Axis.StringformatDrawValue);//绘制坐标轴附加数值

                if (VisionSystemClassLibrary.Enum.AxisDataType.withoutEffectiveValue != Graph_YAxis.DataType)//指定了有效实际数值
                {
                    graphicsDraw.DrawLine(Graph_YAxis.tolerancesDrawing_Axis.PenDrawAdditionalValueLine,
                        new Point((Graph_YAxis.rectPixel_EffectiveMinPoint.Left + Graph_YAxis.rectPixel_EffectiveMinPoint.Right) / 2,
                            Graph_YAxis.rectPixel_EffectiveMinPoint.Top),
                        new Point((Graph_YAxis.rectPixel_AdditionalValue.Left + Graph_YAxis.rectPixel_AdditionalValue.Right) / 2,
                            Graph_YAxis.rectPixel_AdditionalValue.Bottom));//附加值指示曲线（下部）

                    graphicsDraw.DrawLine(Graph_YAxis.tolerancesDrawing_Axis.PenDrawAdditionalValueLine,
                        new Point((Graph_YAxis.rectPixel_AdditionalValue.Left + Graph_YAxis.rectPixel_AdditionalValue.Right) / 2,
                            Graph_YAxis.rectPixel_AdditionalValue.Top),//附加值指示曲线（下部）
                        new Point((Graph_YAxis.rectPixel_EffectiveMaxPoint.Left + Graph_YAxis.rectPixel_EffectiveMaxPoint.Right) / 2,
                            Graph_YAxis.rectPixel_EffectiveMaxPoint.Bottom));//附加值指示曲线（上部）
                }
                else//未指定有效实际数值
                {
                    graphicsDraw.DrawLine(Graph_YAxis.tolerancesDrawing_Axis.PenDrawAdditionalValueLine,
                        new Point((Graph_YAxis.rectPixel_AxisEveryPointValue[0].Left + Graph_YAxis.rectPixel_AxisEveryPointValue[0].Right) / 2,
                            Graph_YAxis.rectPixel_AxisEveryPointValue[0].Top),
                        new Point((Graph_YAxis.rectPixel_AdditionalValue.Left + Graph_YAxis.rectPixel_AdditionalValue.Right) / 2,
                            Graph_YAxis.rectPixel_AdditionalValue.Bottom));//附加值指示曲线（下部）

                    graphicsDraw.DrawLine(Graph_YAxis.tolerancesDrawing_Axis.PenDrawAdditionalValueLine,
                        new Point((Graph_YAxis.rectPixel_AdditionalValue.Left + Graph_YAxis.rectPixel_AdditionalValue.Right) / 2,
                            Graph_YAxis.rectPixel_AdditionalValue.Top),//附加值指示曲线（下部）
                        new Point((Graph_YAxis.rectPixel_AxisEveryPointValue[Graph_YAxis.AxisValueNumber - 1].Left + Graph_YAxis.rectPixel_AxisEveryPointValue[Graph_YAxis.AxisValueNumber - 1].Right) / 2,
                            Graph_YAxis.rectPixel_AxisEveryPointValue[Graph_YAxis.AxisValueNumber - 1].Bottom));//附加值指示曲线（上部）
                }
            }

            //

            if (VisionSystemClassLibrary.Enum.AxisDataType.withoutEffectiveValue != Graph_YAxis.DataType)//指定了有效实际数值
            {
                if (Graph_YAxis.AxisEffectiveMin_Value == Graph_YAxis.AxisEffectiveMax_Value)//最小有效值和最大有效值相等
                {
                    //绘制有效实际值指示线

                    graphicsDraw.DrawLine(Graph_YAxis.tolerancesDrawing_Axis.PenDrawEffectiveMinLine,
                        new Point(Graph_YAxis.rectPixel_AxisAllPointValue.Right,
                            (Graph_YAxis.rectPixel_AxisAllPointValue.Top + Graph_YAxis.rectPixel_AxisAllPointValue.Bottom) / 2),
                        new Point(Graph_XAxis.rectPixel_AxisAllPointValue.Right,
                            (Graph_YAxis.rectPixel_AxisAllPointValue.Top + Graph_YAxis.rectPixel_AxisAllPointValue.Bottom) / 2));//绘制坐标轴有效数值指示线（最小有效值和最大有效值曲线重合）

                    //绘制当前值

                    if (Control_Data.RunorStop)//运行
                    {
                        if (iCurrentValueIndex[0] >= 0 && iCurrentValueIndex[0] < iValueNumber_Curve)//显示当前值、（Y）坐标轴曲线
                        {
                            //当前值

                            graphicsDraw.DrawString(
                                sCurValue,
                                fontCurValue,
                                solidbrushDrawCurValue,
                                new RectangleF(
                                    Convert.ToSingle(rectPixel_CurValue.Left),
                                    Convert.ToSingle(rectPixel_CurValue.Top),
                                    Convert.ToSingle(rectPixel_CurValue.Width),
                                    Convert.ToSingle(rectPixel_CurValue.Height)),
                                stringformatDrawCurValue);//绘制坐标轴当前值

                            if ("" != sCurValue)//不显示
                            {
                                graphicsDraw.FillEllipse(solidbrushDrawCurValueIcon, rectPixel_CurValueIcon);//绘制当前值图标
                            }
                        }
                    }
                }
                else//最小有效值和最大有效值不相同
                {
                    //绘制坐标轴数值

                    if (Graph_YAxis.AxisValueDisplay)//显示坐标轴数值
                    {
                        for (i = 0; i < Graph_YAxis.AxisValueNumber; i++)//绘制坐标轴数值
                        {
                            graphicsDraw.DrawString(
                                Graph_YAxis.sPointValue[i],
                                Graph_YAxis.tolerancesDrawing_Axis.FontValue,
                                Graph_YAxis.tolerancesDrawing_Axis.SolidbrushDrawAxisValue,
                                new RectangleF(
                                    Convert.ToSingle(Graph_YAxis.rectPixel_AxisEveryPointValue[i].Left),
                                    Convert.ToSingle(Graph_YAxis.rectPixel_AxisEveryPointValue[i].Top),
                                    Convert.ToSingle(Graph_YAxis.rectPixel_AxisEveryPointValue[i].Width),
                                    Convert.ToSingle(Graph_YAxis.rectPixel_AxisEveryPointValue[i].Height)),
                                Graph_YAxis.tolerancesDrawing_Axis.StringformatDrawValue);//绘制坐标轴数值
                        }
                    }

                    //绘制有效实际值数值、指示线

                    graphicsDraw.DrawString(
                        Graph_YAxis.sPixel_EffectiveMinPointValue,
                        Graph_YAxis.tolerancesDrawing_Axis.FontValue,
                        Graph_YAxis.tolerancesDrawing_Axis.SolidbrushDrawEffectiveMinValue,
                        new RectangleF(
                            Convert.ToSingle(Graph_YAxis.rectPixel_EffectiveMinPoint.Left),
                            Convert.ToSingle(Graph_YAxis.rectPixel_EffectiveMinPoint.Top),
                            Convert.ToSingle(Graph_YAxis.rectPixel_EffectiveMinPoint.Width),
                            Convert.ToSingle(Graph_YAxis.rectPixel_EffectiveMinPoint.Height)),
                        Graph_YAxis.tolerancesDrawing_Axis.StringformatDrawValue);//绘制坐标轴最小有效数值

                    graphicsDraw.DrawString(
                        Graph_YAxis.sPixel_EffectiveMaxPointValue,
                        Graph_YAxis.tolerancesDrawing_Axis.FontValue,
                        Graph_YAxis.tolerancesDrawing_Axis.SolidbrushDrawEffectiveMaxValue,
                        new RectangleF(
                            Convert.ToSingle(Graph_YAxis.rectPixel_EffectiveMaxPoint.Left),
                            Convert.ToSingle(Graph_YAxis.rectPixel_EffectiveMaxPoint.Top),
                            Convert.ToSingle(Graph_YAxis.rectPixel_EffectiveMaxPoint.Width),
                            Convert.ToSingle(Graph_YAxis.rectPixel_EffectiveMaxPoint.Height)),
                        Graph_YAxis.tolerancesDrawing_Axis.StringformatDrawValue);//绘制坐标轴最大有效数值

                    graphicsDraw.DrawLine(Graph_YAxis.tolerancesDrawing_Axis.PenDrawEffectiveMinLine,
                        new Point(Graph_YAxis.rectPixel_EffectiveMinPoint.Right,
                            (Graph_YAxis.rectPixel_EffectiveMinPoint.Top + Graph_YAxis.rectPixel_EffectiveMinPoint.Bottom) / 2),
                        new Point(Graph_XAxis.rectPixel_AxisAllPointValue.Right,
                            (Graph_YAxis.rectPixel_EffectiveMinPoint.Top + Graph_YAxis.rectPixel_EffectiveMinPoint.Bottom) / 2));//绘制坐标轴最小有效数值指示线

                    graphicsDraw.DrawLine(Graph_YAxis.tolerancesDrawing_Axis.PenDrawEffectiveMaxLine,
                        new Point(Graph_YAxis.rectPixel_EffectiveMaxPoint.Right,
                            (Graph_YAxis.rectPixel_EffectiveMaxPoint.Top + Graph_YAxis.rectPixel_EffectiveMaxPoint.Bottom) / 2),
                        new Point(Graph_XAxis.rectPixel_AxisAllPointValue.Right,
                            (Graph_YAxis.rectPixel_EffectiveMaxPoint.Top + Graph_YAxis.rectPixel_EffectiveMaxPoint.Bottom) / 2));//绘制坐标轴最大有效数值指示线

                    //绘制当前值、（Y）坐标轴曲线

                    if (Control_Data.RunorStop)//运行
                    {
                        if (iCurrentValueIndex[0] >= 0 && iCurrentValueIndex[0] < iValueNumber_Curve)//显示当前值、（Y）坐标轴曲线
                        {
                            //当前值

                            graphicsDraw.DrawString(
                                sCurValue,
                                FontCurValue,
                                solidbrushDrawCurValue,
                                new RectangleF(
                                    Convert.ToSingle(rectPixel_CurValue.Left),
                                    Convert.ToSingle(rectPixel_CurValue.Top),
                                    Convert.ToSingle(rectPixel_CurValue.Width),
                                    Convert.ToSingle(rectPixel_CurValue.Height)),
                                stringformatDrawCurValue);//绘制坐标轴当前值

                            if ("" != sCurValue)//不显示
                            {
                                graphicsDraw.FillEllipse(solidbrushDrawCurValueIcon, rectPixel_CurValueIcon);//绘制当前值图标
                            }

                            //（Y）坐标轴曲线

                            for (i = 0; i < iCurrentValueIndex.Length; i++)
                            {
                                if (bCurveDisplay[i])//显示
                                {
                                    for (j = 0; j < iCurrentValueIndex[i]; j++)//绘制曲线数值
                                    {
                                        graphicsDraw.DrawLine(penLine[i], pointPixelValue[i][j], pointPixelValue[i][j + 1]);
                                    }
                                }

                                if (bValuePointDisplay)//显示
                                {
                                    for (j = 0; j <= iCurrentValueIndex[i]; j++)//绘制曲线数值
                                    {
                                        graphicsDraw.FillEllipse(solidbrushValue[i][j], pointPixelValue[i][j].X - (iValuePointPixelWidth - 1) / 2, pointPixelValue[i][j].Y - (iValuePointPixelWidth - 1) / 2, iValuePointPixelWidth, iValuePointPixelWidth);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else//未指定有效实际数值
            {
                //绘制坐标轴数值

                if (Graph_YAxis.AxisValueDisplay)//显示坐标轴数值
                {
                    for (i = 0; i < Graph_YAxis.AxisValueNumber; i++)//绘制坐标轴数值
                    {
                        graphicsDraw.DrawString(
                            Graph_YAxis.sPointValue[i],
                            Graph_YAxis.tolerancesDrawing_Axis.FontValue,
                            Graph_YAxis.tolerancesDrawing_Axis.SolidbrushDrawAxisValue,
                            new RectangleF(
                                Convert.ToSingle(Graph_YAxis.rectPixel_AxisEveryPointValue[i].Left),
                                Convert.ToSingle(Graph_YAxis.rectPixel_AxisEveryPointValue[i].Top),
                                Convert.ToSingle(Graph_YAxis.rectPixel_AxisEveryPointValue[i].Width),
                                Convert.ToSingle(Graph_YAxis.rectPixel_AxisEveryPointValue[i].Height)),
                            Graph_YAxis.tolerancesDrawing_Axis.StringformatDrawValue);//绘制坐标轴数值
                    }
                }

                //绘制当前值、（Y）坐标轴曲线

                if (Control_Data.RunorStop)//运行
                {
                    if (iCurrentValueIndex[0] >= 0 && iCurrentValueIndex[0] < iValueNumber_Curve)//显示当前值、（Y）坐标轴曲线
                    {
                        //当前值

                        graphicsDraw.DrawString(
                            sCurValue,
                            fontCurValue,
                            solidbrushDrawCurValue,
                            new RectangleF(
                                Convert.ToSingle(rectPixel_CurValue.Left),
                                Convert.ToSingle(rectPixel_CurValue.Top),
                                Convert.ToSingle(rectPixel_CurValue.Width),
                                Convert.ToSingle(rectPixel_CurValue.Height)),
                            stringformatDrawCurValue);//绘制坐标轴当前值

                        if ("" != sCurValue)//不显示
                        {
                            graphicsDraw.FillEllipse(solidbrushDrawCurValueIcon, rectPixel_CurValueIcon);//绘制当前值图标
                        }

                        //（Y）坐标轴曲线

                        for (i = 0; i < iCurrentValueIndex.Length; i++)
                        {
                            if (bCurveDisplay[i])//显示
                            {
                                for (j = 0; j < iCurrentValueIndex[i]; j++)//绘制曲线数值
                                {
                                    graphicsDraw.DrawLine(penLine[i], pointPixelValue[i][j], pointPixelValue[i][j + 1]);
                                }
                            }

                            if (bValuePointDisplay)//显示
                            {
                                for (j = 0; j <= iCurrentValueIndex[i]; j++)//绘制曲线数值
                                {
                                    graphicsDraw.FillEllipse(solidbrushValue[i][j], pointPixelValue[i][j].X - (iValuePointPixelWidth - 1) / 2, pointPixelValue[i][j].Y - (iValuePointPixelWidth - 1) / 2, iValuePointPixelWidth, iValuePointPixelWidth);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    //

    public class Tolerances_Data
    {
        private bool bRunorStop = true;//属性，【运行/停止】按钮（或控件）的运行或停止状态。true：运行；false：停止

        private bool bControlSelected = false;//属性，控件是否被（双击）选中。仅在控件为使能状态时，控件才能被选中。true：是；false：否

        private bool bButtonRunStopShow = true;//属性，【运行/停止】按钮是否显示。true：是；false：否

        private bool bButtonLearningShow = true;//属性，【学习】按钮是否显示。true：是；false：否

        //

        public Tolerances_Tools TolerancesTools = new Tolerances_Tools();//工具

        //属性

        //----------------------------------------------------------------------
        // 功能说明：RunorStop属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public bool RunorStop//属性
        {
            get//读取
            {
                return bRunorStop;
            }
            set//设置
            {
                if (value != bRunorStop)//设置了新的数值
                {
                    bRunorStop = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ControlSelected属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public bool ControlSelected//属性
        {
            get//读取
            {
                return bControlSelected;
            }
            set//设置
            {
                if (value != bControlSelected)//设置了新的数值
                {
                    bControlSelected = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ButtonRunStopShow属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public bool ButtonRunStopShow//属性
        {
            get//读取
            {
                return bButtonRunStopShow;
            }
            set//设置
            {
                if (value != bButtonRunStopShow)//设置了新的数值
                {
                    bButtonRunStopShow = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ButtonLearningShow属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public bool ButtonLearningShow//属性
        {
            get//读取
            {
                return bButtonLearningShow;
            }
            set//设置
            {
                if (value != bButtonLearningShow)//设置了新的数值
                {
                    bButtonLearningShow = value;
                }
            }
        }
    }
    
    public class Tolerances_Graph_Axis
    {
        //坐标轴类型包括：
        //AxisType.XAxis：X坐标轴
        //AxisType.YAxis：Y坐标轴

        //坐标轴数据形式包括：
        //AxisDataType.withoutEffectiveValue：不指定最小有效实际数值、最大有效实际数值，但指定坐标轴的最小值、最大值
        //AxisDataType.withEffectiveValue：指定最小有效实际数值、最大有效实际数值，同时指定坐标轴的最小值、最大值
        //AxisDataType.withPixelAndEffectiveValue：指定最小有效实际数值、最大有效实际数值，同时指定距离像素值
        
        public Tolerances_Drawing tolerancesDrawing_Axis = new Tolerances_Drawing();//绘制曲线图所使用的绘图资源

        public Graphics Graph_Graphics;//用于获取绘制附加值的区域范围

        public double dAxis_K = 0;//坐标轴直线方程的斜率（直线方程中，X表示实际数值，Y表示像素值）
        public double dAxis_B = 0;//坐标轴直线方程的截距（直线方程中，X表示实际数值，Y表示像素值）

        public VisionSystemClassLibrary.Enum.AxisType type = VisionSystemClassLibrary.Enum.AxisType.XAxis;//坐标轴类型

        private VisionSystemClassLibrary.Enum.AxisDataType Datatype = VisionSystemClassLibrary.Enum.AxisDataType.withoutEffectiveValue;//属性，坐标轴数据形式

        private int iAxisValueNumber = 3;//属性，坐标轴显示的坐标值个数（不包含有效值）

        private bool bAxisValueDisplay = true;//属性，曲线图坐标轴上是否显示坐标轴数值。true：是；false：否

        private int iAxisValuePrecision = 0;//属性，曲线图坐标轴上显示的数值精度（0，不显示小数位；大于0，显示相应的小数位）

        private bool bAxisAdditionalValueDisplay = true;//属性，曲线图坐标轴上是否显示附加数值。true：是；false：否
        private double dAxisAdditionalValue = 20;//属性，曲线图坐标轴上显示的附加数值（根据该值得到sAxisAdditionalValue数值）
        private int iAxisAdditionalValuePrecision = 1;//属性，曲线图坐标轴上显示的附加数值精度（0，不显示小数位；大于0，显示相应的小数位）
        public string sAxisAdditionalValue = "";//曲线图坐标轴上显示的附加数值
        public Single fAdditionalValueRatio = 5;//属性，曲线图坐标轴上显示的附加数值系数
        public string sAdditionalValueUnit = "";//属性，曲线图坐标轴上显示的附加数值单位
        public Rectangle rectPixel_AdditionalValue = new Rectangle(new Point(274, 123), new Size(35, 18));//属性，曲线图坐标轴上显示的附加数值的像素区域

        private Size sizePixel_AxisPoint = new Size(30, 18);//属性，显示的曲线图坐标轴中每个坐标点的区域大小

        private Point pointAxisMin_Pixel = new Point(44, 123);//属性，坐标轴最小值对应的像素值
        private Single fAxisMin_Value = 1;//属性，坐标轴最小值对应的实际数值
        private Point pointAxisMax_Pixel = new Point(539, 123);//属性，坐标轴最大值对应的像素值
        private Single fAxisMax_Value = 100;//属性，坐标轴最大值对应的实际数值

        private Single fAxisEffectiveMin_Value = 1;//属性，坐标轴最小有效实际数值
        private int iAxisEffectiveMin_Pixel = 0;//坐标轴最小有效实际数值对应的像素值
        private Single fAxisEffectiveMax_Value = 100;//属性，坐标轴最大有效实际数值
        private int iAxisEffectiveMax_Pixel = 0;//坐标轴最大有效实际数值对应的像素值

        public Point pointPixel_EffectiveMinPoint = new Point();//显示的坐标轴中最小有效实际数值坐标点的位置
        public Point pointPixel_EffectiveMaxPoint = new Point();//显示的坐标轴中最大有效实际数值坐标点的位置
        public Rectangle rectPixel_EffectiveMinPoint = new Rectangle();//显示的坐标轴中最小有效实际数值坐标点的区域
        public Rectangle rectPixel_EffectiveMaxPoint = new Rectangle();//显示的坐标轴中最大有效实际数值坐标点的区域
        public string sPixel_EffectiveMinPointValue = "";//显示的坐标轴中最小有效实际数值的名称
        public string sPixel_EffectiveMaxPointValue = "";//显示的坐标轴中最大有效实际数值的名称

        private int ifromAxisMintoEffectiveMin_Pixel = 15;//属性，坐标轴最小有效实际数值对应的坐标点像素值与坐标轴最小值对应的坐标点像素值之间的距离像素值
        private int ifromAxisMaxtoEffectiveMax_Pixel = 15;//属性，坐标轴最大有效实际数值对应的坐标点像素值与坐标轴最大值对应的坐标点像素值之间的距离像素值

        public Rectangle rectPixel_AxisAllPointValue = new Rectangle();//曲线图坐标轴上显示的所有坐标值的总体像素区域
        public Point[] pointPixel_AxisEveryPointValue = new Point[3];//显示的坐标轴中每个坐标点的像素位置
        public Rectangle[] rectPixel_AxisEveryPointValue = new Rectangle[3];//显示的坐标轴中每个坐标点的像素区域
        public string[] sPointValue = new string[3];//显示的坐标轴中每个坐标点的名称

        //属性

        //----------------------------------------------------------------------
        // 功能说明：AxisDataType属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public VisionSystemClassLibrary.Enum.AxisDataType DataType //属性
        {
            get//读取
            {
                return Datatype;
            }
            set//设置
            {
                if (value != Datatype)
                {
                    Datatype = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：AxisValueNumber属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public int AxisValueNumber //属性
        {
            get//读取
            {
                return iAxisValueNumber;
            }
            set//设置
            {
                if (value != iAxisValueNumber)
                {
                    iAxisValueNumber = value;

                    //

                    pointPixel_AxisEveryPointValue = new Point[iAxisValueNumber];
                    rectPixel_AxisEveryPointValue = new Rectangle[iAxisValueNumber];
                    sPointValue = new string[iAxisValueNumber];
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：AxisValueDisplay属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public bool AxisValueDisplay //属性
        {
            get//读取
            {
                return bAxisValueDisplay;
            }
            set//设置
            {
                if (value != bAxisValueDisplay)
                {
                    bAxisValueDisplay = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：AxisValuePrecision属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public int AxisValuePrecision //属性
        {
            get//读取
            {
                return iAxisValuePrecision;
            }
            set//设置
            {
                if (value != iAxisValuePrecision)
                {
                    if (value >= 0 && value <= 3)
                    {
                        iAxisValuePrecision = value;
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：AxisAdditionalValueDisplay属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public bool AxisAdditionalValueDisplay //属性
        {
            get//读取
            {
                return bAxisAdditionalValueDisplay;
            }
            set//设置
            {
                if (value != bAxisAdditionalValueDisplay)
                {
                    bAxisAdditionalValueDisplay = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：AxisAdditionalValuePrecision属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public int AxisAdditionalValuePrecision //属性
        {
            get//读取
            {
                return iAxisAdditionalValuePrecision;
            }
            set//设置
            {
                if (value != iAxisAdditionalValuePrecision)
                {
                    if (value >= 0 && value <= 3)
                    {
                        iAxisAdditionalValuePrecision = value;
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：AxisAdditionalValue属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public double AxisAdditionalValue //属性
        {
            get//读取
            {
                return dAxisAdditionalValue;
            }
            set//设置
            {
                if (value != dAxisAdditionalValue)
                {
                    dAxisAdditionalValue = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：AdditionalValueRatio属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Single AdditionalValueRatio //属性
        {
            get//读取
            {
                return fAdditionalValueRatio;
            }
            set//设置
            {
                if (value != fAdditionalValueRatio)
                {
                    fAdditionalValueRatio = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：AdditionalValueUnit属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public string AdditionalValueUnit //属性
        {
            get//读取
            {
                return sAdditionalValueUnit;
            }
            set//设置
            {
                if (value != sAdditionalValueUnit)
                {
                    sAdditionalValueUnit = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：RectAdditionalValue属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Rectangle RectAdditionalValue //属性
        {
            get//读取
            {
                return rectPixel_AdditionalValue;
            }
            set//设置
            {
                if (value != rectPixel_AdditionalValue)
                {
                    rectPixel_AdditionalValue = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：SizePixel_AxisPoint属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Size SizePixel_AxisPoint //属性
        {
            get//读取
            {
                return sizePixel_AxisPoint;
            }
            set//设置
            {
                if (value != sizePixel_AxisPoint)
                {
                    sizePixel_AxisPoint = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：LocationAxisMin_Pixel属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Point LocationAxisMin_Pixel //属性
        {
            get//读取
            {
                return pointAxisMin_Pixel;
            }
            set//设置
            {
                if (value != pointAxisMin_Pixel)
                {
                    pointAxisMin_Pixel = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：AxisMin_Value属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Single AxisMin_Value //属性
        {
            get//读取
            {
                return fAxisMin_Value;
            }
            set//设置
            {
                if (value != fAxisMin_Value)
                {
                    fAxisMin_Value = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：LocationAxisMax_Pixel属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Point LocationAxisMax_Pixel //属性
        {
            get//读取
            {
                return pointAxisMax_Pixel;
            }
            set//设置
            {
                if (value != pointAxisMax_Pixel)
                {
                    pointAxisMax_Pixel = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：AxisMax_Value属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Single AxisMax_Value //属性
        {
            get//读取
            {
                return fAxisMax_Value;
            }
            set//设置
            {
                if (value != fAxisMax_Value)
                {
                    fAxisMax_Value = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：AxisEffectiveMin_Value属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Single AxisEffectiveMin_Value //属性
        {
            get//读取
            {
                return fAxisEffectiveMin_Value;
            }
            set//设置
            {
                if (value != fAxisEffectiveMin_Value)
                {
                    fAxisEffectiveMin_Value = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：AxisEffectiveMax_Value属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Single AxisEffectiveMax_Value //属性
        {
            get//读取
            {
                return fAxisEffectiveMax_Value;
            }
            set//设置
            {
                if (value != fAxisEffectiveMax_Value)
                {
                    fAxisEffectiveMax_Value = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：FromAxisMinToEffectiveMin_Pixel属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public int FromAxisMinToEffectiveMin_Pixel //属性
        {
            get//读取
            {
                return ifromAxisMintoEffectiveMin_Pixel;
            }
            set//设置
            {
                if (value != ifromAxisMintoEffectiveMin_Pixel)
                {
                    ifromAxisMintoEffectiveMin_Pixel = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：FromAxisMaxToEffectiveMax_Pixel属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public int FromAxisMaxToEffectiveMax_Pixel //属性
        {
            get//读取
            {
                return ifromAxisMaxtoEffectiveMax_Pixel;
            }
            set//设置
            {
                if (value != ifromAxisMaxtoEffectiveMax_Pixel)
                {
                    ifromAxisMaxtoEffectiveMax_Pixel = value;
                }
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：构造函数，初始化
        // 输入参数：1.Axistype：坐标轴类型
        //         2.graphics：绘图类
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Tolerances_Graph_Axis(VisionSystemClassLibrary.Enum.AxisType Axistype, Graphics graphics)
        {
            type = Axistype;

            Graph_Graphics = graphics;

            //初始化（默认值）

            if (VisionSystemClassLibrary.Enum.AxisType.XAxis == type)//X坐标轴
            {
                Datatype = VisionSystemClassLibrary.Enum.AxisDataType.withoutEffectiveValue;//属性，坐标轴数据形式。

                iAxisValueNumber = 3;//属性，坐标轴显示的坐标值个数

                bAxisValueDisplay = true;//属性，曲线图坐标轴上是否显示坐标轴数值。true：是；false：否

                iAxisValuePrecision = 0;//属性，曲线图坐标轴上显示的数值精度（0，不显示小数位；大于0，显示相应的小数位）

                bAxisAdditionalValueDisplay = false;//属性，曲线图坐标轴上是否显示附加数值。true：是；false：否
                iAxisAdditionalValuePrecision = 1;//属性，曲线图坐标轴上显示的附加数值精度（0，不显示小数位；大于0，显示相应的小数位）
                dAxisAdditionalValue = 20;//属性，曲线图坐标轴上显示的附加数值（根据该值得到sAxisAdditionalValue数值）        
                fAdditionalValueRatio = 5;//属性，曲线图坐标轴上显示的附加数值系数
                sAdditionalValueUnit = "";//属性，曲线图坐标轴上显示的附加数值单位
                rectPixel_AdditionalValue = new Rectangle(new Point(274, 123), new Size(35, 18));//属性，曲线图坐标轴上显示的附加数值的像素区域

                sizePixel_AxisPoint = new Size(30, 18);//属性，显示的曲线图坐标轴中每个坐标点的区域大小

                pointAxisMin_Pixel = new Point(44, 123);//属性，坐标轴最小值对应的像素值
                fAxisMin_Value = 1;//属性，坐标轴最小值对应的实际数值
                pointAxisMax_Pixel = new Point(539, 123);//属性，坐标轴最大值对应的像素值
                fAxisMax_Value = 100;//属性，坐标轴最大值对应的实际数值

                fAxisEffectiveMin_Value = 1;//属性，坐标轴最小有效实际数值
                fAxisEffectiveMax_Value = 100;//属性，坐标轴最大有效实际数值

                ifromAxisMintoEffectiveMin_Pixel = 15;//属性，坐标轴最小有效实际数值对应的坐标点像素值与坐标轴最小值对应的坐标点像素值之间的距离像素值
                ifromAxisMaxtoEffectiveMax_Pixel = 15;//属性，坐标轴最大有效实际数值对应的坐标点像素值与坐标轴最大值对应的坐标点像素值之间的距离像素值

                pointPixel_AxisEveryPointValue = new Point[iAxisValueNumber];
                rectPixel_AxisEveryPointValue = new Rectangle[iAxisValueNumber];
                sPointValue = new string[iAxisValueNumber];
            }
            else//Y坐标轴
            {
                Datatype = VisionSystemClassLibrary.Enum.AxisDataType.withPixelAndEffectiveValue;//属性，坐标轴数据形式。

                iAxisValueNumber = 2;//属性，坐标轴显示的坐标值个数

                bAxisValueDisplay = true;//属性，曲线图坐标轴上是否显示坐标轴数值。true：是；false：否

                iAxisValuePrecision = 0;//属性，曲线图坐标轴上显示的数值精度（0，不显示小数位；大于0，显示相应的小数位）

                bAxisAdditionalValueDisplay = true;//属性，曲线图坐标轴上是否显示附加数值。true：是；false：否
                iAxisAdditionalValuePrecision = 1;//属性，曲线图坐标轴上显示的附加数值精度（0，不显示小数位；大于0，显示相应的小数位）
                dAxisAdditionalValue = 80;//属性，曲线图坐标轴上显示的附加数值（根据该值得到sAxisAdditionalValue数值）
                fAdditionalValueRatio = 5;//属性，曲线图坐标轴上显示的附加数值系数
                sAdditionalValueUnit = "";//属性，曲线图坐标轴上显示的附加数值单位
                rectPixel_AdditionalValue = new Rectangle(new Point(9, 69), new Size(40, 25));//属性，曲线图坐标轴上显示的附加数值的像素区域

                sizePixel_AxisPoint = new Size(30, 18);//属性，显示的曲线图坐标轴中每个坐标点的区域大小

                pointAxisMin_Pixel = new Point(44, 123);//属性，坐标轴最小值对应的像素值
                fAxisMin_Value = 0;//属性，坐标轴最小值对应的实际数值
                pointAxisMax_Pixel = new Point(44, 40);//属性，坐标轴最大值对应的像素值
                fAxisMax_Value = 1000;//属性，坐标轴最大值对应的实际数值

                fAxisEffectiveMin_Value = 300;//属性，坐标轴最小有效实际数值
                fAxisEffectiveMax_Value = 700;//属性，坐标轴最大有效实际数值

                ifromAxisMintoEffectiveMin_Pixel = 15;//属性，坐标轴最小有效实际数值对应的坐标点像素值与坐标轴最小值对应的坐标点像素值之间的距离像素值
                ifromAxisMaxtoEffectiveMax_Pixel = 15;//属性，坐标轴最大有效实际数值对应的坐标点像素值与坐标轴最大值对应的坐标点像素值之间的距离像素值

                pointPixel_AxisEveryPointValue = new Point[iAxisValueNumber];
                rectPixel_AxisEveryPointValue = new Rectangle[iAxisValueNumber];
                sPointValue = new string[iAxisValueNumber];
            }

            //

            _SetAxisData();//配置坐标轴数据
        }
        
        //----------------------------------------------------------------------
        // 功能说明：计算坐标轴数据（Datatype取值为AxisDataType.withoutEffectiveValue时）
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _GetAxisData_DataType_WithoutEffectiveValue()
        {
            try//异常处理
            {
                if (VisionSystemClassLibrary.Enum.AxisType.XAxis == type)//X坐标轴
                {
                    int i = 0;//循环控制变量
                    int iPointValue_Pixel = 0;//坐标轴中每个坐标点的像素值

                    rectPixel_AxisAllPointValue.Location = pointAxisMin_Pixel;//显示坐标轴数值的总体像素区域
                    rectPixel_AxisAllPointValue.Size = new Size(pointAxisMax_Pixel.X - pointAxisMin_Pixel.X, sizePixel_AxisPoint.Height);//显示坐标轴数值的总体像素区域

                    _SetAdditionalValue();//曲线图坐标轴上的附加数值

                    for (i = 0; i < iAxisValueNumber; i++)//初始化
                    {
                        //获取显示的坐标轴中每个坐标点的区域

                        iPointValue_Pixel = rectPixel_AxisAllPointValue.Left + rectPixel_AxisAllPointValue.Width / (iAxisValueNumber - 1) * i;//获取坐标轴中每个坐标点的像素值

                        pointPixel_AxisEveryPointValue[i] = new Point(iPointValue_Pixel, rectPixel_AxisAllPointValue.Top);

                        rectPixel_AxisEveryPointValue[i].Location = new Point(iPointValue_Pixel - (sizePixel_AxisPoint.Width / 2), rectPixel_AxisAllPointValue.Top);
                        rectPixel_AxisEveryPointValue[i].Size = sizePixel_AxisPoint;

                        //获取显示的坐标轴中每个坐标点的名称

                        if (0 == iAxisValuePrecision)//不显示小数位
                        {
                            sPointValue[i] = ((int)(fAxisMin_Value + i * (fAxisMax_Value - fAxisMin_Value) / (iAxisValueNumber - 1))).ToString();
                        }
                        else//显示小数位
                        {
                            sPointValue[i] = (fAxisMin_Value + i * (fAxisMax_Value - fAxisMin_Value) / (iAxisValueNumber - 1)).ToString("F" + iAxisValuePrecision.ToString());
                        }
                    }

                    //计算直线方程
                    //直线方程中，X表示实际数值，Y表示像素值

                    dAxis_K = (double)(pointAxisMax_Pixel.X - pointAxisMin_Pixel.X) / (double)(fAxisMax_Value - fAxisMin_Value);//求解直线方程的斜率
                    dAxis_B = pointAxisMax_Pixel.X - dAxis_K * fAxisMax_Value;//求解直线方程的截距
                }
                else//Y坐标轴
                {
                    int i = 0;//循环控制变量
                    int iPointValue_Pixel = 0;//坐标轴中每个坐标点的像素值

                    rectPixel_AxisAllPointValue.Location = new Point(pointAxisMax_Pixel.X - sizePixel_AxisPoint.Width, pointAxisMax_Pixel.Y);//显示坐标轴数值的总体像素区域
                    rectPixel_AxisAllPointValue.Size = new Size(sizePixel_AxisPoint.Width, pointAxisMin_Pixel.Y - pointAxisMax_Pixel.Y);//显示坐标轴数值的总体像素区域

                    _SetAdditionalValue();//曲线图坐标轴上的附加数值

                    for (i = 0; i < iAxisValueNumber; i++)//初始化
                    {
                        //获取显示的坐标轴中每个坐标点的区域

                        iPointValue_Pixel = rectPixel_AxisAllPointValue.Bottom - rectPixel_AxisAllPointValue.Height / (iAxisValueNumber - 1) * i;//获取坐标轴中每个坐标点的像素值

                        pointPixel_AxisEveryPointValue[i] = new Point(rectPixel_AxisAllPointValue.Right, iPointValue_Pixel);

                        rectPixel_AxisEveryPointValue[i].Location = new Point(rectPixel_AxisAllPointValue.Left, iPointValue_Pixel - (sizePixel_AxisPoint.Height / 2));
                        rectPixel_AxisEveryPointValue[i].Size = sizePixel_AxisPoint;

                        //获取显示的坐标轴中每个坐标点的名称

                        if (0 == iAxisValuePrecision)//不显示小数位
                        {
                            sPointValue[i] = ((int)(fAxisMin_Value + i * (fAxisMax_Value - fAxisMin_Value) / (iAxisValueNumber - 1))).ToString();
                        }
                        else//显示显示位数
                        {
                            sPointValue[i] = (fAxisMin_Value + i * (fAxisMax_Value - fAxisMin_Value) / (iAxisValueNumber - 1)).ToString("F" + iAxisValuePrecision.ToString());
                        }
                    }

                    //计算直线方程
                    //直线方程中，X表示实际数值，Y表示像素值

                    dAxis_K = (double)(pointAxisMax_Pixel.Y - pointAxisMin_Pixel.Y) / (double)(fAxisMax_Value - fAxisMin_Value);//求解直线方程的斜率
                    dAxis_B = pointAxisMax_Pixel.Y - dAxis_K * fAxisMax_Value;//求解直线方程的截距
                }
            }
            catch//异常处理
            {
            	//不执行操作
            }
        }
        
        //----------------------------------------------------------------------
        // 功能说明：计算坐标轴数据（Datatype取值为AxisDataType.withEffectiveValue时）
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _GetAxisData_DataType_WithEffectiveValue()
        {
            try//异常处理
            {
                if (VisionSystemClassLibrary.Enum.AxisType.XAxis == type)//X坐标轴
                {
                    int i = 0;//循环控制变量
                    int iPointValue_Pixel = 0;//坐标轴中每个坐标点的像素值

                    rectPixel_AxisAllPointValue.Location = pointAxisMin_Pixel;//显示坐标轴数值的总体像素区域
                    rectPixel_AxisAllPointValue.Size = new Size(pointAxisMax_Pixel.X - pointAxisMin_Pixel.X, sizePixel_AxisPoint.Height);//显示坐标轴数值的总体像素区域

                    _SetAdditionalValue();//曲线图坐标轴上的附加数值

                    for (i = 0; i < iAxisValueNumber; i++)//初始化
                    {
                        //获取显示的坐标轴中每个坐标点的区域

                        iPointValue_Pixel = rectPixel_AxisAllPointValue.Left + rectPixel_AxisAllPointValue.Width / (iAxisValueNumber - 1) * i;//获取坐标轴中每个坐标点的像素值

                        pointPixel_AxisEveryPointValue[i] = new Point(iPointValue_Pixel, rectPixel_AxisAllPointValue.Top);

                        rectPixel_AxisEveryPointValue[i].Location = new Point(iPointValue_Pixel - (sizePixel_AxisPoint.Width / 2), rectPixel_AxisAllPointValue.Top);
                        rectPixel_AxisEveryPointValue[i].Size = sizePixel_AxisPoint;

                        //获取显示的坐标轴中每个坐标点的名称

                        if (0 == iAxisValuePrecision)//不显示小数位
                        {
                            sPointValue[i] = ((int)(fAxisMin_Value + i * (fAxisMax_Value - fAxisMin_Value) / (iAxisValueNumber - 1))).ToString();
                        }
                        else//显示小数位
                        {
                            sPointValue[i] = (fAxisMin_Value + i * (fAxisMax_Value - fAxisMin_Value) / (iAxisValueNumber - 1)).ToString("F" + iAxisValuePrecision.ToString());
                        }
                    }

                    //计算直线方程
                    //直线方程中，X表示实际数值，Y表示像素值

                    dAxis_K = (double)(pointAxisMax_Pixel.X - pointAxisMin_Pixel.X) / (double)(fAxisMax_Value - fAxisMin_Value);//求解直线方程的斜率
                    dAxis_B = pointAxisMax_Pixel.X - dAxis_K * fAxisMax_Value;//求解直线方程的截距

                    //获取最小有效实际数值、最大有效实际数值的像素坐标值和名称字符串

                    iPointValue_Pixel = Convert.ToInt32(dAxis_K * fAxisEffectiveMin_Value + dAxis_B);//最小有效实际数值的像素坐标值
                    pointPixel_EffectiveMinPoint = new Point(iPointValue_Pixel, rectPixel_AxisAllPointValue.Top);
                    rectPixel_EffectiveMinPoint.Location = new Point(iPointValue_Pixel - (sizePixel_AxisPoint.Width / 2), rectPixel_AxisAllPointValue.Top);//显示的坐标轴中最小有效实际数值坐标点的区域
                    rectPixel_EffectiveMinPoint.Size = sizePixel_AxisPoint;//显示的坐标轴中最小有效实际数值坐标点的区域

                    iPointValue_Pixel = Convert.ToInt32(dAxis_K * fAxisEffectiveMax_Value + dAxis_B);//最大有效实际数值的像素坐标值
                    pointPixel_EffectiveMaxPoint = new Point(iPointValue_Pixel, rectPixel_AxisAllPointValue.Top);
                    rectPixel_EffectiveMaxPoint.Location = new Point(iPointValue_Pixel - (sizePixel_AxisPoint.Width / 2), rectPixel_AxisAllPointValue.Top);//显示的坐标轴中最大有效实际数值坐标点的区域
                    rectPixel_EffectiveMaxPoint.Size = sizePixel_AxisPoint;//显示的坐标轴中最大有效实际数值坐标点的区域

                    if (0 == iAxisValuePrecision)//不显示小数位
                    {
                        sPixel_EffectiveMinPointValue = ((int)fAxisEffectiveMin_Value).ToString();//显示的坐标轴中最小有效实际数值的名称
                        sPixel_EffectiveMaxPointValue = ((int)fAxisEffectiveMax_Value).ToString();//显示的坐标轴中最大有效实际数值的名称
                    }
                    else//显示小数位
                    {
                        sPixel_EffectiveMinPointValue = fAxisEffectiveMin_Value.ToString("F" + iAxisValuePrecision.ToString());//显示的坐标轴中最小有效实际数值的名称
                        sPixel_EffectiveMaxPointValue = fAxisEffectiveMax_Value.ToString("F" + iAxisValuePrecision.ToString());//显示的坐标轴中最大有效实际数值的名称
                    }
                }
                else//Y坐标轴
                {
                    int i = 0;//循环控制变量
                    int iPointValue_Pixel = 0;//坐标轴中每个坐标点的像素值

                    rectPixel_AxisAllPointValue.Location = new Point(pointAxisMax_Pixel.X - sizePixel_AxisPoint.Width, pointAxisMax_Pixel.Y);//显示坐标轴数值的总体像素区域
                    rectPixel_AxisAllPointValue.Size = new Size(sizePixel_AxisPoint.Width, pointAxisMin_Pixel.Y - pointAxisMax_Pixel.Y);//显示坐标轴数值的总体像素区域

                    _SetAdditionalValue();//曲线图坐标轴上的附加数值

                    for (i = 0; i < iAxisValueNumber; i++)//初始化
                    {
                        //获取显示的坐标轴中每个坐标点的区域

                        iPointValue_Pixel = rectPixel_AxisAllPointValue.Bottom - rectPixel_AxisAllPointValue.Height / (iAxisValueNumber - 1) * i;//获取坐标轴中每个坐标点的像素值

                        pointPixel_AxisEveryPointValue[i] = new Point(rectPixel_AxisAllPointValue.Right, iPointValue_Pixel);

                        rectPixel_AxisEveryPointValue[i].Location = new Point(rectPixel_AxisAllPointValue.Left, iPointValue_Pixel - (sizePixel_AxisPoint.Height / 2));
                        rectPixel_AxisEveryPointValue[i].Size = sizePixel_AxisPoint;

                        //获取显示的坐标轴中每个坐标点的名称

                        if (0 == iAxisValuePrecision)//不显示小数位
                        {
                            sPointValue[i] = ((int)(fAxisMin_Value + i * (fAxisMax_Value - fAxisMin_Value) / (iAxisValueNumber - 1))).ToString();
                        }
                        else//显示小数位
                        {
                            sPointValue[i] = (fAxisMin_Value + i * (fAxisMax_Value - fAxisMin_Value) / (iAxisValueNumber - 1)).ToString("F" + iAxisValuePrecision.ToString());
                        }
                    }

                    //计算直线方程
                    //直线方程中，X表示实际数值，Y表示像素值

                    dAxis_K = (double)(pointAxisMax_Pixel.Y - pointAxisMin_Pixel.Y) / (double)(fAxisMax_Value - fAxisMin_Value);//求解直线方程的斜率
                    dAxis_B = pointAxisMax_Pixel.Y - dAxis_K * fAxisMax_Value;//求解直线方程的截距

                    //获取最小有效实际数值、最大有效实际数值的像素坐标值和名称字符串

                    iAxisEffectiveMin_Pixel = Convert.ToInt32(dAxis_K * fAxisEffectiveMin_Value + dAxis_B);//最小有效实际数值的像素坐标值
                    pointPixel_EffectiveMinPoint = new Point(rectPixel_AxisAllPointValue.Right, iAxisEffectiveMin_Pixel);
                    rectPixel_EffectiveMinPoint.Location = new Point(rectPixel_AxisAllPointValue.Left, iAxisEffectiveMin_Pixel - (sizePixel_AxisPoint.Height / 2));//显示的坐标轴中最小有效实际数值坐标点的区域
                    rectPixel_EffectiveMinPoint.Size = sizePixel_AxisPoint;//显示的坐标轴中最小有效实际数值坐标点的区域

                    iAxisEffectiveMax_Pixel = Convert.ToInt32(dAxis_K * fAxisEffectiveMax_Value + dAxis_B);//最大有效实际数值的像素坐标值
                    pointPixel_EffectiveMaxPoint = new Point(rectPixel_AxisAllPointValue.Right, iAxisEffectiveMax_Pixel);
                    rectPixel_EffectiveMaxPoint.Location = new Point(rectPixel_AxisAllPointValue.Left, iAxisEffectiveMax_Pixel - (sizePixel_AxisPoint.Height / 2));//显示的坐标轴中最大有效实际数值坐标点的区域
                    rectPixel_EffectiveMaxPoint.Size = sizePixel_AxisPoint;//显示的坐标轴中最大有效实际数值坐标点的区域

                    if (0 == iAxisValuePrecision)//不显示小数位
                    {
                        sPixel_EffectiveMinPointValue = ((int)fAxisEffectiveMin_Value).ToString();//显示的坐标轴中最小有效实际数值的名称
                        sPixel_EffectiveMaxPointValue = ((int)fAxisEffectiveMax_Value).ToString();//显示的坐标轴中最大有效实际数值的名称
                    }
                    else//显示小数位
                    {
                        sPixel_EffectiveMinPointValue = fAxisEffectiveMin_Value.ToString("F" + iAxisValuePrecision.ToString());//显示的坐标轴中最小有效实际数值的名称
                        sPixel_EffectiveMaxPointValue = fAxisEffectiveMax_Value.ToString("F" + iAxisValuePrecision.ToString());//显示的坐标轴中最大有效实际数值的名称
                    }

                }
            }
            catch//异常处理
            {
            	//不执行操作
            }
        }
        
        //----------------------------------------------------------------------
        // 功能说明：计算坐标轴数据（Datatype取值为AxisDataType.withPixelAndEffectiveValue时）
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _GetAxisData_DataType_WithPixelAndEffectiveValue()
        {
            try//异常处理
            {
                if (VisionSystemClassLibrary.Enum.AxisType.XAxis == type)//X坐标轴
                {
                    int i = 0;//循环控制变量
                    int iPointValue_Pixel = 0;//坐标轴中每个坐标点的像素值

                    iAxisEffectiveMin_Pixel = pointAxisMin_Pixel.X + ifromAxisMintoEffectiveMin_Pixel;//坐标轴最小有效实际数值对应的像素值
                    iAxisEffectiveMax_Pixel = pointAxisMax_Pixel.X - ifromAxisMaxtoEffectiveMax_Pixel;//坐标轴最大有效实际数值对应的像素值

                    rectPixel_AxisAllPointValue.Location = new Point(pointAxisMin_Pixel.X, pointAxisMin_Pixel.Y);//显示坐标轴数值的总体像素区域
                    rectPixel_AxisAllPointValue.Size = new Size(pointAxisMax_Pixel.X - pointAxisMin_Pixel.X, sizePixel_AxisPoint.Height);//显示坐标轴数值的总体像素区域

                    _SetAdditionalValue();//曲线图坐标轴上的附加数值

                    for (i = 0; i < iAxisValueNumber; i++)//初始化
                    {
                        //获取显示的坐标轴中每个坐标点的区域

                        iPointValue_Pixel = rectPixel_AxisAllPointValue.Left + rectPixel_AxisAllPointValue.Width / (iAxisValueNumber - 1) * i;//获取坐标轴中每个坐标点的像素值

                        pointPixel_AxisEveryPointValue[i] = new Point(iPointValue_Pixel, rectPixel_AxisAllPointValue.Top);

                        rectPixel_AxisEveryPointValue[i].Location = new Point(iPointValue_Pixel - (sizePixel_AxisPoint.Width / 2), rectPixel_AxisAllPointValue.Top);
                        rectPixel_AxisEveryPointValue[i].Size = sizePixel_AxisPoint;
                    }

                    //计算直线方程
                    //直线方程中，X表示实际数值，Y表示像素值

                    dAxis_K = (double)(iAxisEffectiveMax_Pixel - iAxisEffectiveMin_Pixel) / (double)(fAxisEffectiveMax_Value - fAxisEffectiveMin_Value);//求解直线方程的斜率
                    dAxis_B = iAxisEffectiveMax_Pixel - dAxis_K * fAxisEffectiveMax_Value;//求解直线方程的截距

                    //获取最小有效实际数值、最大有效实际数值的像素坐标值和名称字符串

                    pointPixel_EffectiveMinPoint = new Point(iAxisEffectiveMin_Pixel, rectPixel_AxisAllPointValue.Top);
                    rectPixel_EffectiveMinPoint.Location = new Point(iAxisEffectiveMin_Pixel - (sizePixel_AxisPoint.Width / 2), rectPixel_AxisAllPointValue.Top);//显示的坐标轴中最小有效实际数值坐标点的区域
                    rectPixel_EffectiveMinPoint.Size = sizePixel_AxisPoint;//显示的坐标轴中最小有效实际数值坐标点的区域

                    pointPixel_EffectiveMaxPoint = new Point(iAxisEffectiveMax_Pixel, rectPixel_AxisAllPointValue.Top);
                    rectPixel_EffectiveMaxPoint.Location = new Point(iAxisEffectiveMax_Pixel - (sizePixel_AxisPoint.Width / 2), rectPixel_AxisAllPointValue.Top);//显示的坐标轴中最大有效实际数值坐标点的区域
                    rectPixel_EffectiveMaxPoint.Size = sizePixel_AxisPoint;//显示的坐标轴中最大有效实际数值坐标点的区域

                    if (0 == iAxisValuePrecision)//不显示小数位
                    {
                        sPixel_EffectiveMinPointValue = ((int)fAxisEffectiveMin_Value).ToString();//显示的坐标轴中最小有效实际数值的名称
                        sPixel_EffectiveMaxPointValue = ((int)fAxisEffectiveMax_Value).ToString();//显示的坐标轴中最大有效实际数值的名称
                    }
                    else//显示小数位
                    {
                        sPixel_EffectiveMinPointValue = fAxisEffectiveMin_Value.ToString("F" + iAxisValuePrecision.ToString());//显示的坐标轴中最小有效实际数值的名称
                        sPixel_EffectiveMaxPointValue = fAxisEffectiveMax_Value.ToString("F" + iAxisValuePrecision.ToString());//显示的坐标轴中最大有效实际数值的名称
                    }

                    //获取坐标轴最小值、最大值对应的实际数值

                    fAxisMin_Value = Convert.ToSingle((pointAxisMin_Pixel.X - dAxis_B) / dAxis_K);//坐标轴最小值对应的实际数值
                    fAxisMax_Value = Convert.ToSingle((pointAxisMax_Pixel.X - dAxis_B) / dAxis_K);//坐标轴最大值对应的实际数值

                    //获取显示的坐标轴中每个坐标点的名称

                    if (0 == iAxisValuePrecision)//不显示小数位
                    {
                        for (i = 0; i < iAxisValueNumber; i++)//初始化
                        {
                            sPointValue[i] = ((int)(fAxisMin_Value + i * (fAxisMax_Value - fAxisMin_Value) / (iAxisValueNumber - 1))).ToString();
                        }
                    }
                    else//显示小数位
                    {
                        for (i = 0; i < iAxisValueNumber; i++)//初始化
                        {
                            sPointValue[i] = (fAxisMin_Value + i * (fAxisMax_Value - fAxisMin_Value) / (iAxisValueNumber - 1)).ToString("F" + iAxisValuePrecision.ToString());
                        }
                    }
                }
                else//Y坐标轴
                {
                    int i = 0;//循环控制变量
                    int iPointValue_Pixel = 0;//坐标轴中每个坐标点的像素值

                    iAxisEffectiveMin_Pixel = pointAxisMin_Pixel.Y - ifromAxisMintoEffectiveMin_Pixel;//坐标轴最小有效实际数值对应的像素值
                    iAxisEffectiveMax_Pixel = pointAxisMax_Pixel.Y + ifromAxisMaxtoEffectiveMax_Pixel;//坐标轴最大有效实际数值对应的像素值

                    rectPixel_AxisAllPointValue.Location = new Point(pointAxisMax_Pixel.X - sizePixel_AxisPoint.Width, pointAxisMax_Pixel.Y);//显示坐标轴数值的总体像素区域
                    rectPixel_AxisAllPointValue.Size = new Size(sizePixel_AxisPoint.Width, pointAxisMin_Pixel.Y - pointAxisMax_Pixel.Y);//显示坐标轴数值的总体像素区域

                    _SetAdditionalValue();//曲线图坐标轴上的附加数值

                    for (i = 0; i < iAxisValueNumber; i++)//初始化
                    {
                        //获取显示的坐标轴中每个坐标点的区域

                        iPointValue_Pixel = rectPixel_AxisAllPointValue.Bottom - rectPixel_AxisAllPointValue.Height / (iAxisValueNumber - 1) * i;//获取坐标轴中每个坐标点的像素值

                        pointPixel_AxisEveryPointValue[i] = new Point(rectPixel_AxisAllPointValue.Right, iPointValue_Pixel);

                        rectPixel_AxisEveryPointValue[i].Location = new Point(rectPixel_AxisAllPointValue.Left, iPointValue_Pixel - (sizePixel_AxisPoint.Height / 2));
                        rectPixel_AxisEveryPointValue[i].Size = sizePixel_AxisPoint;
                    }

                    //计算直线方程
                    //直线方程中，X表示实际数值，Y表示像素值

                    dAxis_K = (double)(iAxisEffectiveMax_Pixel - iAxisEffectiveMin_Pixel) / (double)(fAxisEffectiveMax_Value - fAxisEffectiveMin_Value);//求解直线方程的斜率
                    dAxis_B = iAxisEffectiveMax_Pixel - dAxis_K * fAxisEffectiveMax_Value;//求解直线方程的截距

                    //获取最小有效实际数值、最大有效实际数值的像素坐标值和名称字符串

                    pointPixel_EffectiveMinPoint = new Point(rectPixel_AxisAllPointValue.Right, iAxisEffectiveMin_Pixel);
                    rectPixel_EffectiveMinPoint.Location = new Point(rectPixel_AxisAllPointValue.Left, iAxisEffectiveMin_Pixel - (sizePixel_AxisPoint.Height / 2));//显示的坐标轴中最小有效实际数值坐标点的区域
                    rectPixel_EffectiveMinPoint.Size = sizePixel_AxisPoint;//显示的坐标轴中最小有效实际数值坐标点的区域

                    pointPixel_EffectiveMaxPoint = new Point(rectPixel_AxisAllPointValue.Right, iAxisEffectiveMax_Pixel);
                    rectPixel_EffectiveMaxPoint.Location = new Point(rectPixel_AxisAllPointValue.Left, iAxisEffectiveMax_Pixel - (sizePixel_AxisPoint.Height / 2));//显示的坐标轴中最大有效实际数值坐标点的区域
                    rectPixel_EffectiveMaxPoint.Size = sizePixel_AxisPoint;//显示的坐标轴中最大有效实际数值坐标点的区域

                    if (0 == iAxisValuePrecision)//不显示小数位
                    {
                        sPixel_EffectiveMinPointValue = ((int)fAxisEffectiveMin_Value).ToString();//显示的坐标轴中最小有效实际数值的名称
                        sPixel_EffectiveMaxPointValue = ((int)fAxisEffectiveMax_Value).ToString();//显示的坐标轴中最大有效实际数值的名称
                    }
                    else//显示小数位
                    {
                        sPixel_EffectiveMinPointValue = fAxisEffectiveMin_Value.ToString("F" + iAxisValuePrecision.ToString());//显示的坐标轴中最小有效实际数值的名称
                        sPixel_EffectiveMaxPointValue = fAxisEffectiveMax_Value.ToString("F" + iAxisValuePrecision.ToString());//显示的坐标轴中最大有效实际数值的名称
                    }

                    //获取坐标轴最小值、最大值对应的实际数值

                    fAxisMin_Value = Convert.ToSingle((pointAxisMin_Pixel.Y - dAxis_B) / dAxis_K);//坐标轴最小值对应的实际数值
                    fAxisMax_Value = Convert.ToSingle((pointAxisMax_Pixel.Y - dAxis_B) / dAxis_K);//坐标轴最大值对应的实际数值

                    //获取显示的坐标轴中每个坐标点的名称

                    if (0 == iAxisValuePrecision)//不显示小数位
                    {
                        for (i = 0; i < iAxisValueNumber; i++)//初始化
                        {
                            sPointValue[i] = ((int)(fAxisMin_Value + i * (fAxisMax_Value - fAxisMin_Value) / (iAxisValueNumber - 1))).ToString();
                        }
                    }
                    else//显示小数位
                    {
                        for (i = 0; i < iAxisValueNumber; i++)//初始化
                        {
                            sPointValue[i] = (fAxisMin_Value + i * (fAxisMax_Value - fAxisMin_Value) / (iAxisValueNumber - 1)).ToString("F" + iAxisValuePrecision.ToString());
                        }
                    }

                }
            }
            catch//异常处理
            {
                //不执行操作
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：配置坐标轴数据
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _SetAxisData()
        {
            if (VisionSystemClassLibrary.Enum.AxisDataType.withoutEffectiveValue == Datatype)//不指定最小有效实际数值、最大有效实际数值，但指定坐标轴的最小值、最大值
            {
                _GetAxisData_DataType_WithoutEffectiveValue();
            }
            else if (VisionSystemClassLibrary.Enum.AxisDataType.withEffectiveValue == Datatype)//指定最小有效实际数值、最大有效实际数值，同时指定坐标轴的最小值、最大值
            {
                _GetAxisData_DataType_WithEffectiveValue();
            }
            else//AxisDataType.withPixelAndEffectiveValue，指定最小有效实际数值、最大有效实际数值，同时指定距离像素值
            {
                _GetAxisData_DataType_WithPixelAndEffectiveValue();
            }
        }
  
        //----------------------------------------------------------------------
        // 功能说明：配置坐标轴数据
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetAdditionalValue()
        {
            if (0 == iAxisAdditionalValuePrecision)//不显示小数位
            {
                sAxisAdditionalValue = ((int)dAxisAdditionalValue).ToString();
            } 
            else//显示小数位
            {
                sAxisAdditionalValue = dAxisAdditionalValue.ToString("F" + iAxisAdditionalValuePrecision.ToString());
            }
        }
    }

    public class Tolerances_Drawing
    {
        public Font FontValue = new Font("微软雅黑", 6F, FontStyle.Regular, GraphicsUnit.Point, Convert.ToByte(134));//绘制曲线图中的数值所使用的字体
        public StringFormat StringformatDrawValue = new StringFormat();//绘制文本所使用的格式

        //

        public Color ColorDrawAxisEnable = Color.FromArgb(255, 255, 0);//曲线坐标轴使能时所使用的颜色
        public Color ColorDrawAxisDisable = Color.FromArgb(128, 128, 128);//曲线坐标轴禁止时所使用的颜色
        public float DrawAxisWidth = 1F;//绘制曲线坐标轴所使用的画笔的宽度

        public Color ColorDrawAxisValueEnable = Color.FromArgb(255, 255, 0);//曲线坐标轴数值使能时所使用的颜色
        public Color ColorDrawAxisValueDisable = Color.FromArgb(128, 128, 128);//曲线坐标轴数值禁止时所使用的颜色

        public Color ColorDrawEffectiveMinLineEnable = Color.FromArgb(255, 255, 255);//坐标轴最小有效值区域分界线使能时所使用的颜色
        public Color ColorDrawEffectiveMaxLineEnable = Color.FromArgb(255, 255, 255);//坐标轴最大有效值区域分界线使能时所使用的颜色
        public Color ColorDrawEffectiveLineDisable = Color.FromArgb(128, 128, 128);//坐标轴有效值区域分界线禁止时所使用的颜色
        public float DrawEffectiveLineWidth = 1F;//绘制坐标轴有效值区域分界线所使用的画笔的宽度

        public Color ColorDrawEffectiveMinValueEnable = Color.FromArgb(255, 255, 255);//坐标轴最小有效值使能时所使用的颜色
        public Color ColorDrawEffectiveMaxValueEnable = Color.FromArgb(255, 255, 255);//坐标轴最大有效值使能时所使用的颜色
        public Color ColorDrawEffectiveValueDisable = Color.FromArgb(128, 128, 128);//坐标轴有效值禁止时所使用的颜色

        public Color ColorDrawAdditionalValueLineEnable = Color.FromArgb(255, 128, 0);//曲线图坐标轴上显示的附加数值指示线使能时所使用的颜色
        public Color ColorDrawAdditionalValueLineDisable = Color.FromArgb(128, 128, 128);//曲线图坐标轴上显示的附加数值指示线禁止时所使用的颜色
        public float DrawAdditionalValueLineWidth = 1F;//绘制曲线图坐标轴上显示的附加数值指示线所使用的画笔的宽度

        public Color ColorDrawAdditionalValueEnable = Color.FromArgb(255, 128, 0);//曲线图坐标轴上显示的附加数值使能时所使用的颜色
        public Color ColorDrawAdditionalValueDisable = Color.FromArgb(128, 128, 128);//曲线图坐标轴上显示的附加数值禁止时所使用的颜色

        //

        public Pen PenDrawAxis = new Pen(Color.FromArgb(255, 255, 0), 1F);//绘制曲线坐标轴所使用的画笔
        public SolidBrush SolidbrushDrawAxisValue = new SolidBrush(Color.FromArgb(255, 255, 0));//绘制曲线坐标轴数值所使用的画刷
        public Pen PenDrawEffectiveMinLine = new Pen(Color.FromArgb(255, 255, 255), 1F);//绘制坐标轴最小有效值区域分界线所使用的画笔
        public Pen PenDrawEffectiveMaxLine = new Pen(Color.FromArgb(255, 255, 255), 1F);//绘制坐标轴最大有效值区域分界线所使用的画笔
        public SolidBrush SolidbrushDrawEffectiveMinValue = new SolidBrush(Color.FromArgb(255, 255, 255));//绘制坐标轴最小有效值所使用的画刷
        public SolidBrush SolidbrushDrawEffectiveMaxValue = new SolidBrush(Color.FromArgb(255, 255, 255));//绘制坐标轴最大有效值所使用的画刷
        public Pen PenDrawAdditionalValueLine = new Pen(Color.FromArgb(255, 128, 0), 1F);//绘制曲线图坐标轴上显示的附加数值指示线所使用的画笔
        public SolidBrush SolidbrushDrawAdditionalValue = new SolidBrush(Color.FromArgb(255, 128, 0));//绘制曲线图坐标轴上显示的附加数值所使用的画刷

        //

        //----------------------------------------------------------------------
        // 功能说明：构造函数，初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Tolerances_Drawing()
        {
            StringformatDrawValue.Alignment = StringAlignment.Center;//设置格式
            StringformatDrawValue.LineAlignment = StringAlignment.Center;//设置格式
        }
    }
}