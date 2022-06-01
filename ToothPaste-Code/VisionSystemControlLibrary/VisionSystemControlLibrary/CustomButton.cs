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

功能描述：自定义按钮控件

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
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Reflection;

namespace VisionSystemControlLibrary
{
    public partial class CustomButton : UserControl
    {
        //自定义按钮控件

        //按钮背景可以通过独立的图片指定，也可以通过整体图片指定
        //属性BitmapWhole为null时，按钮背景通过独立的图片指定，否则通过整体图片指定

        //按钮背景通过整体图片指定时，其图片从左至右依次为：Disable（释放，禁止）、Disable（选择，禁止）、Up（释放）、Down（按下）、Selected（选择）、Focus（释放，焦点）、Focus（选择，焦点）、Hover（释放，停留）、Hover（选择，停留）
        //图标索引值数组的顺序与此相同

        private Size sizeButton = new Size();//属性，控件大小

        //

        private Boolean bUpdateControl = false;//属性，更新控件（设计控件时，通过改变其数值，查看设置结果。编译生成时，必须将其数值置为true）

        //

        private Boolean bLabelControlMode = false;//属性，是否为Label控件模式。取值范围：true，是；false，否

        //

        private Bitmap bitmapWhole = null;//属性，整体

        private const Int32 BitmapWholeNumber = 9;//属性，整体图像包含的子图像数量

        //

        private Boolean bHoverBackgroundDisplay = false;//属性，是否显示Hover背景效果。取值范围：true，是；false，否
        private Boolean bFocusBackgroundDisplay = false;//属性，是否显示Focus背景效果。取值范围：true，是；false，否

        //

        private CustomDraw Custom_Draw = new CustomDraw();//绘图数据

        //

        private CustomButton_Type buttonType = CustomButton_Type.Normal;//属性，自定义按钮类型

        private CustomButton_BackgroundImage buttonBackgroundImage = CustomButton_BackgroundImage.Up;//属性，自定义按钮背景
        private CustomButton_BackgroundImage buttonBackgroundImage_Save = CustomButton_BackgroundImage.Up;//存储临时的自定义按钮背景

        private Boolean bControlSelected = false;//控件是否为选择状态。取值范围：true，是；false，否

        //

        private Image imageToDraw = null;//控件图像

        private Image imageControl = null;//属性（只读），控件图像

        //

        private Boolean bDrawText = false;//属性，是否绘制文本。取值范围：true，是；false，否

        private VisionSystemClassLibrary.Enum.InterfaceLanguage language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;//属性，自定义按钮绘制的文本所使用的语言（bDrawText取值为true时有效）
                                      
        private Graphics graphicsDisplay = null;//绘制文本

        private Font fontText = new Font("微软雅黑", 12F, FontStyle.Bold, GraphicsUnit.Point, Convert.ToByte(134));//属性，绘制文本的字体
        private Color colorTextEnable = Color.FromArgb(255, 255, 255);//属性，绘制文本的颜色（使能）
        private Color colorTextDisable = Color.FromArgb(172, 168, 153);//属性，绘制文本的颜色（禁止）
        private Color colorTextSelected = Color.FromArgb(0, 255, 255);//属性，绘制文本的颜色（选择）
        private SolidBrush solidbrushText = new SolidBrush(Color.FromArgb(255, 255, 255));//绘制文本所使用的画刷
        private StringFormat stringformatText = new StringFormat();//绘制文本所使用的格式

        private ContentAlignment textAlignment = ContentAlignment.MiddleCenter;//属性，自定义按钮绘制文本的对齐方式（bDrawText取值为true，drawTextType取值为CustomButton_DrawTextType.Automatic时有效）

        private CustomButton_DrawTextType drawTextType = CustomButton_DrawTextType.Automatic;//属性，自定义按钮绘制文本的方式（bDrawText取值为true时有效）

        //以下的数组的数组中，数组顶层表示语言（对应的数组序号值为语言枚举字段值 - 1），第一层子数组表示文本组

        private Int32 iTextGroupNumber = 1;//属性，自定义按钮绘制的文本的组的数量（bDrawText取值为true时有效）
        private Int32 iCurrentTextGroupIndex = 0;//属性，自定义按钮当前绘制的文本所在组的索引值（从0开始，bDrawText取值为true时有效）
        private Int32[][] iTextNumberInTextGroup = new Int32[2][];//属性，自定义按钮绘制的各个语言的文本中每组中的文本数量（bDrawText取值为true时有效）
        private Point[][] pointIndexInTextGroup = new Point[2][];//自定义按钮绘制的各个语言的文本的起始（Point.X）和结束（Point.Y）索引值（从0开始，bDrawText取值为true时有效）

        //以下的数组的数组中，数组顶层表示语言（对应的数组序号值为语言枚举字段值 - 1），第一层子数组表示各个文本（sTextDisplay第一层元素个数恒为1）

        private String[][] sTextArray = new String[2][];//属性（只读），自定义按钮绘制的各个语言的文本数组（bDrawText取值为true时有效）
        private String[][] sTextDisplay = new String[2][];//属性，自定义按钮绘制的各个语言的原始文本数组（bDrawText取值为true时有效）

        private Point[][] pointTextLocation = new Point[2][];//属性，自定义按钮绘制的各个语言的文本的位置（bDrawText取值为true时有效）

        private Size[][] sizeTextSize = new Size[2][];//属性，自定义按钮绘制的当前语言的文本的范围（bDrawText取值为true时有效）

        //图标

        private Boolean bDrawIcon = false;//属性，是否绘制图标。取值范围：true，是；false，否

        private Bitmap[] bitmapIcon = new Bitmap[1] { null };//图标
        private Bitmap bitmapIconWhole = null;//属性，整体图标
        private Int32 iIconNumber = 1;//属性，图标数量（bDrawIcon取值为true时有效）
        private Int32[] iIconIndex = new Int32[BitmapWholeNumber];//属性，按钮每个状态对应的图标索引值（从0开始，bDrawIcon取值为true时有效）
        private Int32 iCurrentIconIndex = 0;//当前图标索引值（从0开始，bDrawIcon取值为true时有效）
        private Point[] pointIconLocation = new Point[1];//属性，图标位置（bDrawIcon取值为true时有效）
        private Size[] sizeIconSize = new Size[1];//属性，图标的范围（bDrawIcon取值为true时有效。）

        //

        private Object buttonData = null;//属性，预留数据

        //

        [Browsable(true), Description("点击按钮时产生的事件"), Category("CustomButton 事件")]
        public event EventHandler CustomButton_Click;//点击按钮时产生的事件

        //构造函数

        //----------------------------------------------------------------------
        // 功能说明：构造函数，初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public CustomButton()
        {
            InitializeComponent();

            //

            graphicsDisplay = CreateGraphics();//获取绘图资源

            stringformatText.Alignment = StringAlignment.Near;//设置格式
            stringformatText.LineAlignment = StringAlignment.Center;//设置格式

            FieldInfo[] fieldinfo = VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese.GetType().GetFields();//获取公共字段
            if (null != fieldinfo && 1 < fieldinfo.Length)//有效
            {
                Int32 i = 0;//循环控制变量

                pointIndexInTextGroup = new Point[fieldinfo.Length - 1][];//属性，自定义按钮绘制的各个语言的文本的起始（Point.X）和结束（Point.Y）索引值（从0开始，bDrawText取值为true时有效）
                for (i = 0; i < fieldinfo.Length - 1; i++)
                {
                    pointIndexInTextGroup[i] = new Point[iTextGroupNumber];
                }

                iTextNumberInTextGroup = new Int32[fieldinfo.Length - 1][];//属性，自定义按钮绘制的各个语言的文本中每组中的文本数量（bDrawText取值为true时有效）
                for (i = 0; i < fieldinfo.Length - 1; i++)
                {
                    iTextNumberInTextGroup[i] = new Int32[iTextGroupNumber];
                }

                //

                sTextArray = new String[fieldinfo.Length - 1][];//属性，自定义按钮绘制的各个语言的文本数组（bDrawText取值为true时有效）
                for (i = 0; i < fieldinfo.Length - 1; i++)
                {
                    sTextArray[i] = new String[1];

                    sTextArray[i][0] = "";
                }

                sTextDisplay = new String[fieldinfo.Length - 1][];//属性，自定义按钮绘制的各个语言的原始文本数组（bDrawText取值为true时有效）
                for (i = 0; i < fieldinfo.Length - 1; i++)
                {
                    sTextDisplay[i] = new String[1];

                    sTextDisplay[i][0] = "";
                }

                pointTextLocation = new Point[fieldinfo.Length - 1][];//属性，自定义按钮绘制的各个语言的文本的位置（bDrawText取值为true时有效。drawTextType取值为CustomButton_DrawTextType.Manual时需要用户设定该值）
                for (i = 0; i < fieldinfo.Length - 1; i++)
                {
                    pointTextLocation[i] = new Point[1];
                }

                sizeTextSize = new Size[fieldinfo.Length - 1][];//属性，自定义按钮绘制的当前语言的文本的范围（bDrawText取值为true时有效）
                for (i = 0; i < fieldinfo.Length - 1; i++)
                {
                    sizeTextSize[i] = new Size[1];
                }
            }

            sizeButton = this.Size;
        }

        //属性

        //-----------------------------------------------------------------------
        // 功能说明：SizeButton属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("更新控件（设计控件时，通过改变其数值，查看设置结果。编译生成时，必须将其数值置为true）"), Category("CustomButton 更新")]
        public Boolean UpdateControl
        {
            get//读取
            {
                return bUpdateControl;
            }
            set//设置
            {
                bUpdateControl = value;

                //

                _GetText();//获取文本

                _SetText();//设置文本

                _Apply();//应用
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：SizeButton属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("控件大小"), Category("CustomButton 通用")]
        public Size SizeButton
        {
            get//读取
            {
                return sizeButton;
            }
            set//设置
            {
                sizeButton = value;

                this.Size = value;

                //

                graphicsDisplay.Dispose();//释放

                graphicsDisplay = CreateGraphics();//获取绘图资源

                //

                if (bUpdateControl)//编译生成
                {
                    _GetText();//获取文本

                    _SetText();//设置文本

                    _Apply();//应用
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ImageControl属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(false), Description("控件图像"), Category("CustomButton 通用")]
        public Image ImageControl
        {
            get//读取
            {
                return imageControl;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：LabelControlMode属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("是否为Label控件模式。取值范围：true，是；false，否"), Category("CustomButton 通用")]
        public Boolean LabelControlMode
        {
            get//读取
            {
                return bLabelControlMode;
            }
            set//设置
            {
                if (value != bLabelControlMode)
                {
                    bLabelControlMode = value;
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：BackgroundColor属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("控件背景颜色"), Category("CustomButton 通用")]
        public Color BackgroundColor
        {
            get//读取
            {
                return BackColor;
            }
            set//设置
            {
                if (value != BackColor)
                {
                    BackColor = value;

                    //

                    if (bUpdateControl)//编译生成
                    {
                        _UpdateBackground(buttonBackgroundImage);
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：HoverBackgroundDisplay属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("是否显示Hover背景效果。取值范围：true，是；false，否"), Category("CustomButton 通用")]
        public Boolean HoverBackgroundDisplay//属性
        {
            get//读取
            {
                return bHoverBackgroundDisplay;
            }
            set//设置
            {
                if (value != bHoverBackgroundDisplay)//设置了新的数值
                {
                    bHoverBackgroundDisplay = value;

                    //

                    if (bUpdateControl)//编译生成
                    {
                        _UpdateBackground(buttonBackgroundImage);//更新
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：FocusBackgroundDisplay属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("是否显示Focus背景效果。取值范围：true，是；false，否"), Category("CustomButton 通用")]
        public Boolean FocusBackgroundDisplay//属性
        {
            get//读取
            {
                return bFocusBackgroundDisplay;
            }
            set//设置
            {
                if (value != bFocusBackgroundDisplay)//设置了新的数值
                {
                    bFocusBackgroundDisplay = value;

                    //

                    if (bUpdateControl)//编译生成
                    {
                        _UpdateBackground(buttonBackgroundImage);//更新
                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：CustomButtonType属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("自定义按钮类型"), Category("CustomButton 通用")]
        public CustomButton_Type CustomButtonType
        {
            get//读取
            {
                return buttonType;
            }
            set//设置
            {
                if (value != buttonType)
                {
                    buttonType = value;

                    //

                    if (CustomButton_Type.Normal == buttonType)//常规
                    {
                        bControlSelected = false;
                    }
                    else//开关
                    {
                        if (CustomButton_BackgroundImage.Selected == buttonBackgroundImage)//选择
                        {
                            bControlSelected = true;
                        }
                        else if (CustomButton_BackgroundImage.Up == buttonBackgroundImage)//释放
                        {
                            bControlSelected = false;
                        }
                        else//其它
                        {
                            //不执行操作
                        }
                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：CustomButtonBackgroundImage属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("自定义按钮背景"), Category("CustomButton 通用")]
        public CustomButton_BackgroundImage CustomButtonBackgroundImage
        {
            get//读取
            {
                return buttonBackgroundImage;
            }
            set//设置
            {
                if (value != buttonBackgroundImage)//更新
                {
                    buttonBackgroundImage_Save = buttonBackgroundImage_Save & (~buttonBackgroundImage);

                    if (bHoverBackgroundDisplay && (CustomButton_BackgroundImage.Hover == value))
                    {
                        buttonBackgroundImage_Save = buttonBackgroundImage_Save | value;
                    }
                    else if (bFocusBackgroundDisplay && (CustomButton_BackgroundImage.Focus == value))
                    {
                        buttonBackgroundImage_Save = buttonBackgroundImage_Save | value;
                    }
                    else
                    {
                        buttonBackgroundImage_Save = buttonBackgroundImage_Save | value;
                    }

                    //

                    buttonBackgroundImage = value;

                    //

                    if (CustomButton_Type.Normal == buttonType)//常规
                    {
                        bControlSelected = false;
                    }
                    else//开关
                    {
                        if (CustomButton_BackgroundImage.Selected == buttonBackgroundImage)//选择
                        {
                            bControlSelected = true;
                        }
                        else if (CustomButton_BackgroundImage.Up == buttonBackgroundImage)//释放
                        {
                            bControlSelected = false;
                        }
                        else//其它
                        {
                            //不执行操作
                        }
                    }

                    //

                    if (bUpdateControl)//编译生成
                    {
                        _UpdateBackground(buttonBackgroundImage);//更新
                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Language属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("自定义按钮绘制的文本所使用的语言（bDrawText取值为true时有效）"), Category("CustomButton 通用")]
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

                    if (bUpdateControl)//编译生成
                    {
                        _UpdateBackground(buttonBackgroundImage);//更新
                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：DrawText属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("是否绘制文本。取值范围：true，是；false，否"), Category("CustomButton 通用")]
        public Boolean DrawText
        {
            get//读取
            {
                return bDrawText;
            }
            set//设置
            {
                if (value != bDrawText)
                {
                    bDrawText = value;

                    //

                    if (bUpdateControl)//编译生成
                    {
                        if (bDrawText)//绘制
                        {
                            _GetText();//获取文本

                            _SetText();//设置文本
                        }

                        _UpdateBackground(buttonBackgroundImage);//更新
                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：DrawIcon属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("是否绘制图标。取值范围：true，是；false，否"), Category("CustomButton 通用")]
        public Boolean DrawIcon
        {
            get//读取
            {
                return bDrawIcon;
            }
            set//设置
            {
                if (value != bDrawIcon)
                {
                    bDrawIcon = value;

                    //

                    if (bUpdateControl)//编译生成
                    {
                        _UpdateBackground(buttonBackgroundImage);//更新
                    }
                } 
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：CustomButtonData属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(false), Description("预留数据"), Category("CustomButton 通用")]
        public Object CustomButtonData
        {
            get//读取
            {
                return buttonData;
            }
            set//设置
            {
                buttonData = value;
            }
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：BitmapWhole属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("整体"), Category("CustomButton 背景")]
        public Bitmap BitmapWhole
        {
            get//读取
            {
                return bitmapWhole;
            }
            set//设置
            {
                if (null != bitmapWhole)
                {
                    bitmapWhole.Dispose();
                }

                //

                bitmapWhole = value;

                //

                if (bUpdateControl)//编译生成
                {
                    _Apply();//应用
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：DrawType属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("绘图类型"), Category("CustomButton 背景")]
        public CustomDrawType DrawType//属性
        {
            get//读取
            {
                return Custom_Draw.DrawType;
            }
            set//设置
            {
                if (value != Custom_Draw.DrawType)//设置了新的数值
                {
                    Custom_Draw.DrawType = value;

                    //

                    if (bUpdateControl)//编译生成
                    {
                        _Apply();//对于任意大小按钮，应用设置
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：RectLeft属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("背景图像左部填充区域（CustomDrawType.Horizontal、CustomDrawType.Central时有效）"), Category("CustomButton 背景")]
        public Rectangle RectLeft//属性
        {
            get//读取
            {
                return Custom_Draw.RectLeft;
            }
            set//设置
            {
                if (value != Custom_Draw.RectLeft)//设置了新的数值
                {
                    Custom_Draw.RectLeft = value;

                    //

                    if (bUpdateControl)//编译生成
                    {
                        _Apply();//对于任意大小按钮，应用设置
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：RectTop属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("背景图像上部填充区域（CustomDrawType.Vertical、CustomDrawType.Central时有效）"), Category("CustomButton 背景")]
        public Rectangle RectTop//属性
        {
            get//读取
            {
                return Custom_Draw.RectTop;
            }
            set//设置
            {
                if (value != Custom_Draw.RectTop)//设置了新的数值
                {
                    Custom_Draw.RectTop = value;

                    //

                    if (bUpdateControl)//编译生成
                    {
                        _Apply();//对于任意大小按钮，应用设置
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：RectRight属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("背景图像右部填充区域（CustomDrawType.Horizontal、CustomDrawType.Central时有效）"), Category("CustomButton 背景")]
        public Rectangle RectRight//属性
        {
            get//读取
            {
                return Custom_Draw.RectRight;
            }
            set//设置
            {
                if (value != Custom_Draw.RectRight)//设置了新的数值
                {
                    Custom_Draw.RectRight = value;

                    //

                    if (bUpdateControl)//编译生成
                    {
                        _Apply();//对于任意大小按钮，应用设置
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：RectBottom属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("背景图像下部填充区域（CustomDrawType.Vertical、CustomDrawType.Central时有效）"), Category("CustomButton 背景")]
        public Rectangle RectBottom//属性
        {
            get//读取
            {
                return Custom_Draw.RectBottom;
            }
            set//设置
            {
                if (value != Custom_Draw.RectBottom)//设置了新的数值
                {
                    Custom_Draw.RectBottom = value;

                    //

                    if (bUpdateControl)//编译生成
                    {
                        _Apply();//对于任意大小按钮，应用设置
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：RectLeftTop属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("背景图像左上部矩形区域（CustomDrawType.Central时有效）"), Category("CustomButton 背景")]
        public Rectangle RectLeftTop//属性
        {
            get//读取
            {
                return Custom_Draw.RectLeftTop;
            }
            set//设置
            {
                if (value != Custom_Draw.RectLeftTop)//设置了新的数值
                {
                    Custom_Draw.RectLeftTop = value;

                    //

                    if (bUpdateControl)//编译生成
                    {
                        _Apply();//对于任意大小按钮，应用设置
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：RectRightTop属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("背景图像右上部矩形区域（CustomDrawType.Central时有效）"), Category("CustomButton 背景")]
        public Rectangle RectRightTop//属性
        {
            get//读取
            {
                return Custom_Draw.RectRightTop;
            }
            set//设置
            {
                if (value != Custom_Draw.RectRightTop)//设置了新的数值
                {
                    Custom_Draw.RectRightTop = value;

                    //

                    if (bUpdateControl)//编译生成
                    {
                        _Apply();//对于任意大小按钮，应用设置
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：RectLeftBottom属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("背景图像左下部矩形区域（CustomDrawType.Central时有效）"), Category("CustomButton 背景")]
        public Rectangle RectLeftBottom//属性
        {
            get//读取
            {
                return Custom_Draw.RectLeftBottom;
            }
            set//设置
            {
                if (value != Custom_Draw.RectLeftBottom)//设置了新的数值
                {
                    Custom_Draw.RectLeftBottom = value;

                    //

                    if (bUpdateControl)//编译生成
                    {
                        _Apply();//对于任意大小按钮，应用设置
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：RectRightBottom属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("背景图像右下部矩形区域（CustomDrawType.Central时有效）"), Category("CustomButton 背景")]
        public Rectangle RectRightBottom//属性
        {
            get//读取
            {
                return Custom_Draw.RectRightBottom;
            }
            set//设置
            {
                if (value != Custom_Draw.RectRightBottom)//设置了新的数值
                {
                    Custom_Draw.RectRightBottom = value;

                    //

                    if (bUpdateControl)//编译生成
                    {
                        _Apply();//对于任意大小按钮，应用设置
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：RectFill属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("背景图像中部填充区域（用于填充整个按钮底部背景，CustomDrawType.Vertical、CustomDrawType.Horizontal和CustomDrawType.Central时有效）"), Category("CustomButton 背景")]
        public Rectangle RectFill//属性
        {
            get//读取
            {
                return Custom_Draw.RectFill;
            }
            set//设置
            {
                if (value != Custom_Draw.RectFill)//设置了新的数值
                {
                    Custom_Draw.RectFill = value;

                    //

                    if (bUpdateControl)//编译生成
                    {
                        _Apply();//对于任意大小按钮，应用设置
                    }
                }
            }
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：TextGroupNumber属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("自定义按钮绘制的文本的组的数量（bDrawText取值为true时有效）"), Category("CustomButton 文本")]
        public Int32 TextGroupNumber
        {
            get//读取
            {
                return iTextGroupNumber;
            }
            set//设置
            {
                if (value != iTextGroupNumber)//有效
                {
                    iTextGroupNumber = value;

                    //

                    Int32 i = 0;

                    for (i = 0; i < iTextNumberInTextGroup.Length; i++)
                    {
                        if (iTextGroupNumber != iTextNumberInTextGroup[i].Length)
                        {
                            iTextNumberInTextGroup[i] = new Int32[iTextGroupNumber];//属性，自定义按钮绘制的各个语言的文本中每组中的文本数量（bDrawText取值为true时有效）
                        }

                        if (iTextGroupNumber != pointIndexInTextGroup[i].Length)
                        {
                            pointIndexInTextGroup[i] = new Point[iTextGroupNumber];//属性（只读），自定义按钮绘制的各个语言的文本的起始（Point.X）和结束（Point.Y）索引值（从0开始，bDrawText取值为true时有效）
                        }
                    }

                    //

                    if (bUpdateControl)//编译生成
                    {
                        _SetText();//设置文本

                        _UpdateBackground(buttonBackgroundImage);//更新
                    }
                }

                _GetText();
                _SetText();
                _Apply();
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：CurrentTextGroupIndex属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("自定义按钮当前绘制的文本所在组的索引值（从0开始，bDrawText取值为true时有效）"), Category("CustomButton 文本")]
        public Int32 CurrentTextGroupIndex
        {
            get//读取
            {
                return iCurrentTextGroupIndex;
            }
            set//设置
            {
                if (value != iCurrentTextGroupIndex)
                {
                    iCurrentTextGroupIndex = value;

                    //

                    if (bUpdateControl)//编译生成
                    {
                        _UpdateBackground(buttonBackgroundImage);//更新
                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Chinese_TextNumberInTextGroup 属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("自定义按钮绘制的中文文本中每组中的文本数量（bDrawText取值为true时有效）"), Category("CustomButton 文本")]
        public Int32[] Chinese_TextNumberInTextGroup 
        {
            get//读取
            {
                return iTextNumberInTextGroup[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1];
            }
            set//设置
            {
                iTextNumberInTextGroup[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1] = value;

                //

                if (null != value)//有效
                {
                    iTextNumberInTextGroup[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1] = new Int32[value.Length];
                    pointIndexInTextGroup[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1] = new Point[value.Length];
                    
                    //

                    value.CopyTo(iTextNumberInTextGroup[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1], 0);

                    //

                    Int32 i = 0;//循环控制变量

                    pointIndexInTextGroup[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] = new Point(0, iTextNumberInTextGroup[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0]);
                    for (i = 1; i < value.Length; i++)
                    {
                        pointIndexInTextGroup[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][i] = new Point(pointIndexInTextGroup[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][i - 1].Y, pointIndexInTextGroup[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][i - 1].Y + iTextNumberInTextGroup[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][i]);
                    }

                    //

                    if (bUpdateControl)//编译生成
                    {
                        _SetText();//设置文本

                        _UpdateBackground(buttonBackgroundImage);//更新
                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：English_TextNumberInTextGroup 属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("自定义按钮绘制的英文文本中每组中的文本数量（bDrawText取值为true时有效）"), Category("CustomButton 文本")]
        public Int32[] English_TextNumberInTextGroup
        {
            get//读取
            {
                return iTextNumberInTextGroup[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1];
            }
            set//设置
            {
                iTextNumberInTextGroup[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1] = value;

                //

                if (null != value)//有效
                {
                    iTextNumberInTextGroup[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1] = new Int32[value.Length];
                    pointIndexInTextGroup[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1] = new Point[value.Length];

                    //

                    value.CopyTo(iTextNumberInTextGroup[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1], 0);

                    //

                    Int32 i = 0;//循环控制变量

                    pointIndexInTextGroup[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] = new Point(0, iTextNumberInTextGroup[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0]);
                    for (i = 1; i < value.Length; i++)
                    {
                        pointIndexInTextGroup[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][i] = new Point(pointIndexInTextGroup[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][i - 1].Y, pointIndexInTextGroup[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][i - 1].Y + iTextNumberInTextGroup[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][i]);
                    }

                    //

                    if (bUpdateControl)//编译生成
                    {
                        _SetText();//设置文本

                        _UpdateBackground(buttonBackgroundImage);//更新
                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Chinese_TextDisplay属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("自定义按钮绘制的中文原始文本数组（bDrawText取值为true时有效）"), Category("CustomButton 文本")]
        public String[] Chinese_TextDisplay
        {
            get//读取
            {
                return sTextDisplay[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1];
            }
            set//设置
            {
                sTextDisplay[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1] = value;

                if (null != value)//有效
                {
                    sTextDisplay[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1] = new String[1];

                    value.CopyTo(sTextDisplay[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1], 0);

                    //

                    if (bUpdateControl)//编译生成
                    {
                        _GetText();//获取文本

                        _SetText();//设置文本

                        _UpdateBackground(buttonBackgroundImage);//更新
                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：English_TextDisplay属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("自定义按钮绘制的英文原始文本数组（bDrawText取值为true时有效）"), Category("CustomButton 文本")]
        public String[] English_TextDisplay
        {
            get//读取
            {
                return sTextDisplay[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1];
            }
            set//设置
            {
                sTextDisplay[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1] = value;

                if (null != value)//有效
                {
                    sTextDisplay[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1] = new String[1];

                    value.CopyTo(sTextDisplay[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1], 0);

                    //

                    if (bUpdateControl)//编译生成
                    {
                        _GetText();//获取文本

                        _SetText();//设置文本

                        _UpdateBackground(buttonBackgroundImage);//更新
                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Chinese_TextArray属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(false), Description("自定义按钮绘制的中文语言的文本数组（bDrawText取值为true时有效）"), Category("CustomButton 文本")]
        public String[] Chinese_TextArray
        {
            get//读取
            {
                return sTextArray[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1];
            }
        }
         
        //-----------------------------------------------------------------------
        // 功能说明：English_TextArray属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(false), Description("自定义按钮绘制的英文语言的文本数组（bDrawText取值为true时有效）"), Category("CustomButton 文本")]
        public String[] English_TextArray
        {
            get//读取
            {
                return sTextArray[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1];
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Chinese_TextLocation属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("自定义按钮绘制的中文文本的位置（bDrawText取值为true时有效）"), Category("CustomButton 文本")]
        public Point[] Chinese_TextLocation
        {
            get//读取
            {
                return pointTextLocation[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1];
            }
            set//设置
            {
                pointTextLocation[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1] = value;

                if (null != value)//有效
                {
                    pointTextLocation[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1] = new Point[value.Length];

                    value.CopyTo(pointTextLocation[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1], 0);

                    //

                    if (bUpdateControl)//编译生成
                    {
                        _UpdateBackground(buttonBackgroundImage);//更新
                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：English_TextLocation属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("自定义按钮绘制的英文文本的位置（bDrawText取值为true时有效）"), Category("CustomButton 文本")]
        public Point[] English_TextLocation
        {
            get//读取
            {
                return pointTextLocation[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1];
            }
            set//设置
            {
                pointTextLocation[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1] = value;

                if (null != value)//有效
                {
                    pointTextLocation[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1] = new Point[value.Length];

                    value.CopyTo(pointTextLocation[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1], 0);

                    //

                    if (bUpdateControl)//编译生成
                    {
                        _UpdateBackground(buttonBackgroundImage);//更新
                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Chinese_TextSize属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("自定义按钮绘制的中文文本的范围（bDrawText取值为true时有效）"), Category("CustomButton 文本")]
        public Size[] Chinese_TextSize
        {
            get//读取
            {
                return sizeTextSize[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1];
            }
            set//设置
            {
                sizeTextSize[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1] = value;

                if (null != value)//有效
                {
                    sizeTextSize[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1] = new Size[value.Length];

                    value.CopyTo(sizeTextSize[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1], 0);

                    //

                    if (bUpdateControl)//编译生成
                    {
                        _UpdateBackground(buttonBackgroundImage);//更新
                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：English_TextSize属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("自定义按钮绘制的英文文本的范围（bDrawText取值为true时有效）"), Category("CustomButton 文本")]
        public Size[] English_TextSize
        {
            get//读取
            {
                return sizeTextSize[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1];
            }
            set//设置
            {
                sizeTextSize[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1] = value;

                if (null != value)//有效
                {
                    sizeTextSize[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1] = new Size[value.Length];

                    value.CopyTo(sizeTextSize[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1], 0);

                    //

                    if (bUpdateControl)//编译生成
                    {
                        _UpdateBackground(buttonBackgroundImage);//更新
                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：TextAlignment属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("自定义按钮绘制文本的对齐方式"), Category("CustomButton 文本")]
        public ContentAlignment TextAlignment
        {
            get//读取
            {
                return textAlignment;
            }
            set//设置
            {
                if (value != textAlignment)
                {
                    textAlignment = value;

                    //

                    if (bUpdateControl)//编译生成
                    {
                        _SetText();//设置文本

                        _UpdateBackground(buttonBackgroundImage);//更新
                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：FontText属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("绘制文本的字体"), Category("CustomButton 文本")]
        public Font FontText
        {
            get//读取
            {
                return fontText;
            }
            set//设置
            {
                if (value != fontText)
                {
                    fontText = value;

                    //

                    if (bUpdateControl)//编译生成
                    {
                        _SetText();//设置文本

                        _UpdateBackground(buttonBackgroundImage);//更新
                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ColorTextEnable属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("绘制文本的颜色（使能）"), Category("CustomButton 文本")]
        public Color ColorTextEnable
        {
            get//读取
            {
                return colorTextEnable;
            }
            set//设置
            {
                if (value != colorTextEnable)
                {
                    colorTextEnable = value;

                    //

                    if (bUpdateControl)//编译生成
                    {
                        _UpdateBackground(buttonBackgroundImage);
                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ColorTextDisable属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("绘制文本的颜色（禁止）"), Category("CustomButton 文本")]
        public Color ColorTextDisable
        {
            get//读取
            {
                return colorTextDisable;
            }
            set//设置
            {
                if (value != colorTextDisable)
                {
                    colorTextDisable = value;

                    //

                    if (bUpdateControl)//编译生成
                    {
                        _UpdateBackground(buttonBackgroundImage);
                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ColorTextSelected属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("绘制文本的颜色（禁止）"), Category("CustomButton 文本")]
        public Color ColorTextSelected
        {
            get//读取
            {
                return colorTextSelected;
            }
            set//设置
            {
                if (value != colorTextSelected)
                {
                    colorTextSelected = value;

                    //

                    if (bUpdateControl)//编译生成
                    {
                        _UpdateBackground(buttonBackgroundImage);
                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：CustomButtonDrawTextType属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("自定义按钮绘制文本的方式"), Category("CustomButton 文本")]
        public CustomButton_DrawTextType CustomButtonDrawTextType
        {
            get//读取
            {
                return drawTextType;
            }
            set//设置
            {
                if (value != drawTextType)
                {
                    drawTextType = value;

                    //

                    if (bUpdateControl)//编译生成
                    {
                        _SetText();//设置文本

                        _UpdateBackground(buttonBackgroundImage);//更新
                    }
                }
            }
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：IconNumber属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("图标数量"), Category("CustomButton 图标")]
        public Int32 IconNumber
        {
            get//读取
            {
                return iIconNumber;
            }
            set//设置
            {
                if (value != iIconNumber)
                {
                    if (0 < iIconNumber)//有效
                    {
                        iIconNumber = value;

                        //

                        if (iIconNumber != bitmapIcon.Length)
                        {
                            Int32 i = 0;//循环控制变量

                            //

                            if (null != bitmapIcon)
                            {
                                for (i = 0; i < bitmapIcon.Length; i++)//赋值
                                {
                                    if (null != bitmapIcon[i])
                                    {
                                        bitmapIcon[i].Dispose();
                                    }
                                }
                            }

                            //

                            bitmapIcon = new Bitmap[iIconNumber];

                            for (i = 0; i < bitmapIcon.Length; i++)//赋值
                            {
                                bitmapIcon[i] = null;
                            }
                        }

                        //

                        if (iIconNumber != pointIconLocation.Length)
                        {
                            pointIconLocation = new Point[iIconNumber];
                        }

                        if (iIconNumber != sizeIconSize.Length)
                        {
                            sizeIconSize = new Size[iIconNumber];
                        } 
                    }
                    else//无效
                    {
                        bitmapIcon = null;
                    }

                    //

                    if (bUpdateControl)//编译生成
                    {
                        _UpdateBackground(buttonBackgroundImage);//更新
                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：IconIndex属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("按钮每个状态对应的图标索引值（从0开始，bDrawIcon取值为true时有效）"), Category("CustomButton 图标")]
        public Int32[] IconIndex
        {
            get//读取
            {
                return iIconIndex;
            }
            set//设置
            {
                iIconIndex = value;

                if (null != value)//有效
                {
                    iIconIndex = new Int32[value.Length];

                    value.CopyTo(iIconIndex, 0);

                    //

                    if (bUpdateControl)//编译生成
                    {
                        _UpdateBackground(buttonBackgroundImage);//更新
                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：BitmapIconWhole属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("整体图标"), Category("CustomButton 图标")]
        public Bitmap BitmapIconWhole
        {
            get//读取
            {
                return bitmapIconWhole;
            }
            set//设置
            {
                if (null != bitmapIconWhole)
                {
                    bitmapIconWhole.Dispose();
                }

                //

                bitmapIconWhole = value;

                if (null != bitmapIconWhole)//有效
                {
                    try
                    {
                        Size sizeIcon = new Size(bitmapIconWhole.Width / iIconNumber, bitmapIconWhole.Height);//属性，图标的大小

                        Int32 i = 0;//循环控制变量

                        for (i = 0; i < sizeIconSize.Length; i++)//赋值
                        {
                            sizeIconSize[i] = sizeIcon;
                        }

                        //

                        if (bUpdateControl)//编译生成
                        {
                            _UpdateBackground(buttonBackgroundImage);//更新
                        }
                    }
                    catch (System.Exception ex)
                    {
                        //不执行操作
                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：IconLocation属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("图标位置"), Category("CustomButton 图标")]
        public Point[] IconLocation
        {
            get//读取
            {
                return pointIconLocation;
            }
            set//设置
            {
                pointIconLocation = value;

                if (null != value)//有效
                {
                    pointIconLocation = new Point[value.Length];

                    value.CopyTo(pointIconLocation, 0);

                    //

                    if (bUpdateControl)//编译生成
                    {
                        _UpdateBackground(buttonBackgroundImage);//更新
                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：IconSize属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("图标的范围"), Category("CustomButton 图标")]
        public Size[] IconSize
        {
            get//读取
            {
                return sizeIconSize;
            }
            set//设置
            {
                sizeIconSize = value;

                if (null != value)//有效
                {
                    sizeIconSize = new Size[value.Length];

                    value.CopyTo(sizeIconSize, 0);

                    //

                    if (bUpdateControl)//编译生成
                    {
                        _UpdateBackground(buttonBackgroundImage);//更新
                    }
                }
            }
        }

        //函数

        //----------------------------------------------------------------------
        // 功能说明：更新控件
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _Refresh()
        {
            _UpdateBackground(buttonBackgroundImage);//更新
        }

        //----------------------------------------------------------------------
        // 功能说明：应用设置完成的属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _Apply()
        {
            Custom_Draw.RectDraw = ClientRectangle;//属性，绘制区域

            if (null != bitmapWhole)//整体
            {
                Custom_Draw.SizeImage = new Size(bitmapWhole.Width / BitmapWholeNumber, bitmapWhole.Height);//属性，背景图像的大小

                Custom_Draw._GetCustomDraw();//获取绘图数据
            }

            //

            _UpdateBackground(buttonBackgroundImage);
        }

        //----------------------------------------------------------------------
        // 功能说明：获取控件图像
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _GetImage()
        {
            _UpdateBackground(buttonBackgroundImage, false);
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

        //

        //----------------------------------------------------------------------
        // 功能说明：更新按钮背景图像
        // 输入参数：1.backgroundImage：按钮背景图像类型
        //         2.bDraw：是否绘制控件。取值范围：true，是（用于控件内部调用）；false，否（用于控件外部获取控件背景图像时使用）
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _UpdateBackground(CustomButton_BackgroundImage backgroundImage, Boolean bDraw = true)
        {
            Bitmap bitmapBackground = null;

            Image imageDisplay = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);
            Graphics graphicsDraw = Graphics.FromImage(imageDisplay);

            //

            Int32 i = 0;//循环控制变量

            if (null != bitmapIconWhole)
            {
                Size sizeIcon = new Size(bitmapIconWhole.Width / iIconNumber, bitmapIconWhole.Height);//属性，图标的大小

                for (i = 0; i < iIconNumber; i++)//获取图标
                {
                    bitmapIcon[i] = bitmapIconWhole.Clone(new Rectangle(new Point(i * sizeIcon.Width, 0), sizeIcon), bitmapIconWhole.PixelFormat);//获取图标
                }
            }

            //

            switch (backgroundImage)
            {
                case CustomButton_BackgroundImage.Disable://禁止
                    //
                    solidbrushText = new SolidBrush(colorTextDisable);//绘制文本所使用的画刷
                    //
                    if (bControlSelected)//选择
                    {
                        iCurrentIconIndex = IconIndex[1];//当前图标索引值

                        //

                        if (null != bitmapWhole)
                        {
                            bitmapBackground = bitmapWhole.Clone(new Rectangle(new Point(Custom_Draw.SizeImage.Width, 0), Custom_Draw.SizeImage), bitmapWhole.PixelFormat);//选择，禁止
                        }
                    } 
                    else//释放
                    {
                        iCurrentIconIndex = IconIndex[0];//当前图标索引值

                        //

                        if (null != bitmapWhole)
                        {
                            bitmapBackground = bitmapWhole.Clone(new Rectangle(new Point(0, 0), Custom_Draw.SizeImage), bitmapWhole.PixelFormat);//释放，禁止
                        }
                    }
                    //
                    break;
                case CustomButton_BackgroundImage.Up://释放
                    //
                    solidbrushText = new SolidBrush(colorTextEnable);//绘制文本所使用的画刷
                    //
                    iCurrentIconIndex = IconIndex[2];//当前图标索引值
                    //
                    if (null != bitmapWhole)
                    {
                        bitmapBackground = bitmapWhole.Clone(new Rectangle(new Point(Custom_Draw.SizeImage.Width * 2, 0), Custom_Draw.SizeImage), bitmapWhole.PixelFormat);//释放
                    }
                    //
                    break;
                case CustomButton_BackgroundImage.Down://未选择
                    //
                    solidbrushText = new SolidBrush(colorTextEnable);//绘制文本所使用的画刷
                    //
                    iCurrentIconIndex = IconIndex[3];//当前图标索引值
                    //
                    if (null != bitmapWhole)
                    {
                        bitmapBackground = bitmapWhole.Clone(new Rectangle(new Point(Custom_Draw.SizeImage.Width * 3, 0), Custom_Draw.SizeImage), bitmapWhole.PixelFormat);//按下
                    }
                    //
                    break;
                case CustomButton_BackgroundImage.Selected://选择
                    //
                    solidbrushText = new SolidBrush(colorTextSelected);//绘制文本所使用的画刷
                    //
                    iCurrentIconIndex = IconIndex[4];//当前图标索引值
                    //
                    if (null != bitmapWhole)
                    {
                        bitmapBackground = bitmapWhole.Clone(new Rectangle(new Point(Custom_Draw.SizeImage.Width * 4, 0), Custom_Draw.SizeImage), bitmapWhole.PixelFormat);//选择
                    }
                    //
                    break;
                case CustomButton_BackgroundImage.Focus://焦点
                    //
                    if (bFocusBackgroundDisplay)//显示
                    {
                        if (bControlSelected)//选择
                        {
                            solidbrushText = new SolidBrush(colorTextSelected);//绘制文本所使用的画刷
                            //
                            iCurrentIconIndex = IconIndex[6];//当前图标索引值
                            //
                            if (null != bitmapWhole)
                            {
                                bitmapBackground = bitmapWhole.Clone(new Rectangle(new Point(Custom_Draw.SizeImage.Width * 6, 0), Custom_Draw.SizeImage), bitmapWhole.PixelFormat);//选择，焦点
                            }
                        }
                        else//释放
                        {
                            solidbrushText = new SolidBrush(colorTextEnable);//绘制文本所使用的画刷
                            //
                            iCurrentIconIndex = IconIndex[5];//当前图标索引值
                            //
                            if (null != bitmapWhole)
                            {
                                bitmapBackground = bitmapWhole.Clone(new Rectangle(new Point(Custom_Draw.SizeImage.Width * 5, 0), Custom_Draw.SizeImage), bitmapWhole.PixelFormat);//释放，焦点
                            }
                        }
                    }
                    //
                    break;
                case CustomButton_BackgroundImage.Hover://停留
                    //
                    if (bHoverBackgroundDisplay)//显示
                    {
                        if (bControlSelected)//选择
                        {
                            solidbrushText = new SolidBrush(colorTextSelected);//绘制文本所使用的画刷
                            //
                            iCurrentIconIndex = IconIndex[8];//当前图标索引值
                            //
                            if (null != bitmapWhole)
                            {
                                bitmapBackground = bitmapWhole.Clone(new Rectangle(new Point(Custom_Draw.SizeImage.Width * 8, 0), Custom_Draw.SizeImage), bitmapWhole.PixelFormat);//选择，停留
                            }
                        }
                        else//释放
                        {
                            solidbrushText = new SolidBrush(colorTextEnable);//绘制文本所使用的画刷
                            //
                            iCurrentIconIndex = IconIndex[7];//当前图标索引值
                            //
                            if (null != bitmapWhole)
                            {
                                bitmapBackground = bitmapWhole.Clone(new Rectangle(new Point(Custom_Draw.SizeImage.Width * 7, 0), Custom_Draw.SizeImage), bitmapWhole.PixelFormat);//释放，停留
                            }
                        }
                    }
                    //
                    break;
                default:
                    break;
            }

            //

            if (null != bitmapBackground)//有效
            {
                Custom_Draw._Draw(graphicsDraw, bitmapBackground);

                if (null != bitmapBackground)//释放资源
                {
                    bitmapBackground.Dispose();
                }

                bitmapBackground = (Bitmap)imageDisplay.Clone();
            }

            //

            if (null != bitmapBackground)//有效
            {
                _GetBackgroundImage((Image)bitmapBackground.Clone(), bDraw);
            }
            else//无效
            {
                _GetBackgroundImage(null, bDraw);
            }

            //

            if (null != bitmapIcon)
            {
                for (i = 0; i < bitmapIcon.Length; i++)//赋值
                {
                    if (null != bitmapIcon[i])
                    {
                        bitmapIcon[i].Dispose();
                    }
                }
            }

            if (null != bitmapBackground)//释放资源
            {
                bitmapBackground.Dispose();
            }

            //

            if (bDraw)//绘制
            {
                _Draw();

                //

                if (null != imageToDraw)//释放资源
                {
                    imageToDraw.Dispose();
                }
            }

            //

            graphicsDraw.Dispose();//释放资源
            imageDisplay.Dispose();//释放资源
        }

        //----------------------------------------------------------------------
        // 功能说明：获取背景图像
        // 输入参数：1.background：背景
        //         2.bDraw：是否绘制控件。取值范围：true，是（用于控件内部调用）；false，否（用于控件外部获取控件背景图像时使用）
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _GetBackgroundImage(Image background, Boolean bDraw = true)
        {
            Image imageDisplay = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);
            Graphics graphicsDraw = Graphics.FromImage(imageDisplay);

            SolidBrush solidbrushDraw = new SolidBrush(BackColor);

            //绘制背景色

            graphicsDraw.FillRectangle(solidbrushDraw, ClientRectangle);//绘制当前选择的相机控件区域

            //绘制背景图像

            if (null != background)//有效
            {
                graphicsDraw.DrawImage(background, ClientRectangle);//绘制
            }

            //绘制图标

            try
            {
                if (bDrawIcon)//绘制图标
                {
                    if (null != bitmapIcon[iCurrentIconIndex])//图标有效
                    {
                        graphicsDraw.DrawImage(bitmapIcon[iCurrentIconIndex], new Rectangle(pointIconLocation[iCurrentIconIndex], sizeIconSize[iCurrentIconIndex]));//绘制图标
                    }
                }

                //绘制文本

                if (bDrawText)//绘制文本
                {
                    if (null != sTextArray)//有效
                    {
                        Int32 i = 0;//循环控制变量

                        for (i = pointIndexInTextGroup[(Int32)language - 1][iCurrentTextGroupIndex].X; i < pointIndexInTextGroup[(Int32)language - 1][iCurrentTextGroupIndex].Y; i++)//遍历
                        {
                            graphicsDraw.DrawString(sTextArray[(Int32)language - 1][i], fontText, solidbrushText, new Rectangle(pointTextLocation[(Int32)language - 1][i], sizeTextSize[(Int32)language - 1][i]), stringformatText);//绘制文本
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                //不执行操作
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

            graphicsDisplay.DrawImage(imageDisplay, ClientRectangle);

            //

            graphicsDraw.Dispose();
            imageDisplay.Dispose();
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：获取绘制的文本
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _GetText()
        {
            try
            {
                Int32 i = 0;//循环控制变量

                if (null != sTextDisplay && null != sTextDisplay[0])//有效
                {
                    //sTextArray = new String[sTextDisplay.Length][];

                    if (pointTextLocation.Length != sTextDisplay.Length)
                    {
                        pointTextLocation = new Point[sTextDisplay.Length][];
                    }

                    if (sizeTextSize.Length != sTextDisplay.Length)
                    {
                        sizeTextSize = new Size[sTextDisplay.Length][];
                    }

                    //

                    for (i = 0; i < sTextDisplay.Length; i++)
                    {
                        if ("" == sTextDisplay[i][0])//无效
                        {
                            sTextArray[i] = new String[1];

                            sTextArray[i][0] = "";
                        }
                        else//有效
                        {
                            sTextArray[i] = sTextDisplay[i][0].Split('&');

                            //

                            Int32 j = 0;//循环控制变量
                            Boolean bValue_Temp = false;//临时变量

                            for (j = 0; j < sTextArray[i].Length - 1; j++)//处理&&情况
                            {
                                if ("" == sTextArray[i][j] && "" == sTextArray[i][j + 1])//&&情况
                                {
                                    sTextArray[i][j] = "&";

                                    j += 1;

                                    bValue_Temp = true;
                                } 
                            }
                            if (bValue_Temp)//处理&&情况
                            {
                                Int32 k = 0;//循环控制变量

                                for (j = 0; j < sTextArray[i].Length - 1; j++)
                                {
                                    if ("" != sTextArray[i][j])//无效
                                    {
                                        k++;
                                    }
                                }

                                String[] sText_Temp = new String[k];
                                for (j = 0; j < sTextArray[i].Length - 1; j++)
                                {
                                    if ("" != sTextArray[i][j])//无效
                                    {
                                        sText_Temp[j] = sTextArray[i][j];
                                    }
                                }

                                sTextArray[i] = new String[k];
                                sText_Temp.CopyTo(sTextArray[i], 0);
                            }
                        }

                        //

                        if (pointTextLocation[i].Length != sTextArray[i].Length)//不同
                        {
                            pointTextLocation[i] = new Point[sTextArray[i].Length];
                        }

                        if (sizeTextSize[i].Length != sTextArray[i].Length)//不同
                        {
                            sizeTextSize[i] = new Size[sTextArray[i].Length];
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                //不执行操作
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：设置绘制的文本
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetText()
        {
            try
            {
                if (bDrawText)//绘制
                {
                    Int32 i = 0;//循环控制变量
                    Int32 j = 0;//循环控制变量
                    Int32 k = 0;//循环控制变量
                    Int32 m = 0;//循环控制变量

                    if (CustomButton_DrawTextType.Manual == drawTextType)//手动
                    {
                        if (null != sTextArray)//有效
                        {
                            for (i = 0; i < sTextArray.Length; i++)//遍历
                            {
                                for (j = 0; j < sTextArray[i].Length; j++)
                                {
                                    sizeTextSize[i][j] = graphicsDisplay.MeasureString(sTextArray[i][j], fontText, (SizeF)(ClientRectangle.Size), stringformatText).ToSize();//绘制范围
                                    sizeTextSize[i][j].Width += 1;//调试过程中发现返回的尺寸偏小，因此进行调整
                                }
                            }
                        }
                    }
                    else if (CustomButton_DrawTextType.Automatic == drawTextType)//自动
                    {
                        if (null != sTextArray)//有效
                        {
                            Int32[] iTotalHeight = new Int32[iTextGroupNumber];//临时变量

                            for (m = 0; m < sTextArray.Length; m++)//遍历
                            {
                                k = 0;

                                for (i = 0; i < iTextGroupNumber; i++)//遍历
                                {
                                    iTotalHeight[i] = 0;

                                    //

                                    for (j = k; j < k + iTextNumberInTextGroup[m][i]; j++)//遍历
                                    {
                                        sizeTextSize[m][j] = graphicsDisplay.MeasureString(sTextArray[m][j], fontText, (SizeF)(ClientRectangle.Size), stringformatText).ToSize();//绘制范围

                                        sizeTextSize[m][j].Width += 1;//调试过程中发现返回的尺寸偏小，因此进行调整
                                        sizeTextSize[m][j].Height += 1;//调试过程中发现返回的尺寸偏小，因此进行调整

                                        //

                                        iTotalHeight[i] += sizeTextSize[m][j].Height;//更新
                                    }

                                    //

                                    switch (textAlignment)//对齐方式
                                    {
                                        case ContentAlignment.TopLeft://左上
                                            //
                                            pointTextLocation[m][k] = new Point(ClientRectangle.Left, ClientRectangle.Top);//位置

                                            for (j = k + 1; j < k + iTextNumberInTextGroup[m][i]; j++)//遍历
                                            {
                                                pointTextLocation[m][j] = new Point(ClientRectangle.Left, pointTextLocation[m][j - 1].Y + sizeTextSize[m][j - 1].Height);//位置
                                            }
                                            //
                                            break;
                                        case ContentAlignment.MiddleLeft://左中
                                            //
                                            pointTextLocation[m][k] = new Point(ClientRectangle.Left, (ClientRectangle.Top + ClientRectangle.Bottom - iTotalHeight[i]) / 2);//位置

                                            for (j = k + 1; j < k + iTextNumberInTextGroup[m][i]; j++)//遍历
                                            {
                                                pointTextLocation[m][j] = new Point(ClientRectangle.Left, pointTextLocation[m][j - 1].Y + sizeTextSize[m][j - 1].Height);//位置
                                            }
                                            //
                                            break;
                                        case ContentAlignment.BottomLeft://左下
                                            //
                                            pointTextLocation[m][k] = new Point(ClientRectangle.Left, ClientRectangle.Bottom - iTotalHeight[i]);//位置

                                            for (j = k + 1; j < k + iTextNumberInTextGroup[m][i]; j++)//遍历
                                            {
                                                pointTextLocation[m][j] = new Point(ClientRectangle.Left, pointTextLocation[m][j - 1].Y + sizeTextSize[m][j - 1].Height);//位置
                                            }
                                            //
                                            break;
                                        case ContentAlignment.TopCenter://中上
                                            //
                                            pointTextLocation[m][k] = new Point((ClientRectangle.Left + ClientRectangle.Right - sizeTextSize[m][k].Width) / 2, ClientRectangle.Top);//位置

                                            for (j = k + 1; j < k + iTextNumberInTextGroup[m][i]; j++)//遍历
                                            {
                                                pointTextLocation[m][j] = new Point((ClientRectangle.Left + ClientRectangle.Right - sizeTextSize[m][j].Width) / 2, pointTextLocation[m][j - 1].Y + sizeTextSize[m][j - 1].Height);//位置
                                            }
                                            //
                                            break;
                                        case ContentAlignment.MiddleCenter://中
                                            //
                                            pointTextLocation[m][k] = new Point((ClientRectangle.Left + ClientRectangle.Right - sizeTextSize[m][k].Width) / 2, (ClientRectangle.Top + ClientRectangle.Bottom - iTotalHeight[i]) / 2);//位置

                                            for (j = k + 1; j < k + iTextNumberInTextGroup[m][i]; j++)//遍历
                                            {
                                                pointTextLocation[m][j] = new Point((ClientRectangle.Left + ClientRectangle.Right - sizeTextSize[m][j].Width) / 2, pointTextLocation[m][j - 1].Y + sizeTextSize[m][j - 1].Height);//位置
                                            }
                                            //
                                            break;
                                        case ContentAlignment.BottomCenter://中下
                                            //
                                            pointTextLocation[m][k] = new Point((ClientRectangle.Left + ClientRectangle.Right - sizeTextSize[m][k].Width) / 2, ClientRectangle.Bottom - iTotalHeight[i]);//位置

                                            for (j = k + 1; j < k + iTextNumberInTextGroup[m][i]; j++)//遍历
                                            {
                                                pointTextLocation[m][j] = new Point((ClientRectangle.Left + ClientRectangle.Right - sizeTextSize[m][j].Width) / 2, pointTextLocation[m][j - 1].Y + sizeTextSize[m][j - 1].Height);//位置
                                            }
                                            //
                                            break;
                                        case ContentAlignment.TopRight://右上
                                            //
                                            pointTextLocation[m][k] = new Point(ClientRectangle.Right - sizeTextSize[m][k].Width, ClientRectangle.Top);//位置

                                            for (j = k + 1; j < k + iTextNumberInTextGroup[m][i]; j++)//遍历
                                            {
                                                pointTextLocation[m][j] = new Point(ClientRectangle.Right - sizeTextSize[m][j].Width, pointTextLocation[m][j - 1].Y + sizeTextSize[m][j - 1].Height);//位置
                                            }
                                            //
                                            break;
                                        case ContentAlignment.MiddleRight://右中
                                            //
                                            pointTextLocation[m][k] = new Point(ClientRectangle.Right - sizeTextSize[m][k].Width, (ClientRectangle.Top + ClientRectangle.Bottom - iTotalHeight[i]) / 2);//位置

                                            for (j = k + 1; j < k + iTextNumberInTextGroup[m][i]; j++)//遍历
                                            {
                                                pointTextLocation[m][j] = new Point(ClientRectangle.Right - sizeTextSize[m][j].Width, pointTextLocation[m][j - 1].Y + sizeTextSize[m][j - 1].Height);//位置
                                            }
                                            //
                                            break;
                                        case ContentAlignment.BottomRight://右下
                                            //
                                            pointTextLocation[m][k] = new Point(ClientRectangle.Right - sizeTextSize[m][k].Width, ClientRectangle.Bottom - iTotalHeight[i]);//位置

                                            for (j = k + 1; j < k + iTextNumberInTextGroup[m][i]; j++)//遍历
                                            {
                                                pointTextLocation[m][j] = new Point(ClientRectangle.Right - sizeTextSize[m][j].Width, pointTextLocation[m][j - 1].Y + sizeTextSize[m][j - 1].Height);//位置
                                            }
                                            //
                                            break;
                                        default:
                                            break;
                                    }

                                    //

                                    k += iTextNumberInTextGroup[m][i];
                                }
                            }
                        }
                    }
                    else//其它
                    {
                        //不执行操作
                    }
                }
            }
            catch (System.Exception ex)
            {
                //不执行操作
            }
        }

        //事件

        //----------------------------------------------------------------------
        // 功能说明：绘制控件事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void CustomButton_Paint(object sender, PaintEventArgs e)
        {
            _UpdateBackground(buttonBackgroundImage);
        }

        //----------------------------------------------------------------------
        // 功能说明：按下按钮事件，更新按钮背景
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void CustomButton_MouseDown(object sender, MouseEventArgs e)
        {
            if (!bLabelControlMode)//非Label控件模式
            {
                if (Convert.ToBoolean(buttonBackgroundImage))//按钮使能
                {
                    switch (buttonType)
                    {
                        case CustomButton_Type.Normal://常规
                            //
                            buttonBackgroundImage_Save = buttonBackgroundImage_Save | CustomButton_BackgroundImage.Down;

                            buttonBackgroundImage = CustomButton_BackgroundImage.Down;

                            _UpdateBackground(buttonBackgroundImage);
                            //
                            break;
                        case CustomButton_Type.Switch://开关
                            //
                            if (CustomButton_BackgroundImage.Up == (CustomButton_BackgroundImage.Up & buttonBackgroundImage_Save))//释放
                            {
                                buttonBackgroundImage_Save = buttonBackgroundImage_Save & (~(CustomButton_BackgroundImage.Up));

                                buttonBackgroundImage_Save = buttonBackgroundImage_Save | CustomButton_BackgroundImage.Selected;

                                if (bHoverBackgroundDisplay && (CustomButton_BackgroundImage.Hover == (CustomButton_BackgroundImage.Hover & buttonBackgroundImage_Save)))
                                {
                                    buttonBackgroundImage = CustomButton_BackgroundImage.Hover;
                                }
                                else if (bFocusBackgroundDisplay && (CustomButton_BackgroundImage.Focus == (CustomButton_BackgroundImage.Focus & buttonBackgroundImage_Save)))
                                {
                                    buttonBackgroundImage = CustomButton_BackgroundImage.Focus;
                                }
                                else
                                {
                                    buttonBackgroundImage = CustomButton_BackgroundImage.Selected;
                                }

                                bControlSelected = true;

                                _UpdateBackground(buttonBackgroundImage);
                            }
                            else if (CustomButton_BackgroundImage.Selected == (CustomButton_BackgroundImage.Selected & buttonBackgroundImage_Save))//选择
                            {
                                buttonBackgroundImage_Save = buttonBackgroundImage_Save & (~(CustomButton_BackgroundImage.Selected));

                                buttonBackgroundImage_Save = buttonBackgroundImage_Save | CustomButton_BackgroundImage.Up;

                                if (bHoverBackgroundDisplay && (CustomButton_BackgroundImage.Hover == (CustomButton_BackgroundImage.Hover & buttonBackgroundImage_Save)))
                                {
                                    buttonBackgroundImage = CustomButton_BackgroundImage.Hover;
                                }
                                else if (bFocusBackgroundDisplay && (CustomButton_BackgroundImage.Focus == (CustomButton_BackgroundImage.Focus & buttonBackgroundImage_Save)))
                                {
                                    buttonBackgroundImage = CustomButton_BackgroundImage.Focus;
                                }
                                else
                                {
                                    buttonBackgroundImage = CustomButton_BackgroundImage.Up;
                                }

                                bControlSelected = false;

                                _UpdateBackground(buttonBackgroundImage);
                            }
                            else//其它
                            {
                                //不执行操作
                            }
                            //
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：释放按钮事件，更新按钮背景
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void CustomButton_MouseUp(object sender, MouseEventArgs e)
        {
            if (!bLabelControlMode)//非Label控件模式
            {
                if (Convert.ToBoolean(buttonBackgroundImage))//按钮使能
                {
                    switch (buttonType)
                    {
                        case CustomButton_Type.Normal://常规
                            //
                            buttonBackgroundImage_Save = buttonBackgroundImage_Save & (~(CustomButton_BackgroundImage.Down));

                            if (bHoverBackgroundDisplay && (CustomButton_BackgroundImage.Hover == (CustomButton_BackgroundImage.Hover & buttonBackgroundImage_Save)))
                            {
                                buttonBackgroundImage = CustomButton_BackgroundImage.Hover;
                            }
                            else if (bFocusBackgroundDisplay && (CustomButton_BackgroundImage.Focus == (CustomButton_BackgroundImage.Focus & buttonBackgroundImage_Save)))
                            {
                                buttonBackgroundImage = CustomButton_BackgroundImage.Focus;
                            }
                            else
                            {
                                buttonBackgroundImage = CustomButton_BackgroundImage.Up;
                            }

                            _UpdateBackground(buttonBackgroundImage);

                            //

                            if (null != CustomButton_Click)//有效
                            {
                                CustomButton_Click(this, new EventArgs());
                            }
                            //
                            break;
                        case CustomButton_Type.Switch://开关
                            //
                            if (null != CustomButton_Click)//有效
                            {
                                CustomButton_Click(this, new EventArgs());
                            }
                            //
                            break;
                        default:
                            break;
                    }
                }
            }
            else//Label控件模式
            {
                if (null != CustomButton_Click)//有效
                {
                    CustomButton_Click(this, new EventArgs());
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：鼠标进入控件的可见部分事件，更新按钮背景
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void CustomButton_MouseEnter(object sender, EventArgs e)
        {
            if (!bLabelControlMode)//非Label控件模式
            {
                if (Convert.ToBoolean(buttonBackgroundImage))//按钮使能
                {
                    if (bHoverBackgroundDisplay)//显示
                    {
                        buttonBackgroundImage_Save = buttonBackgroundImage_Save | CustomButton_BackgroundImage.Hover;

                        buttonBackgroundImage = CustomButton_BackgroundImage.Hover;

                        _UpdateBackground(buttonBackgroundImage);
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：鼠标离开控件的可见部分事件，更新按钮背景
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void CustomButton_MouseLeave(object sender, EventArgs e)
        {
            if (!bLabelControlMode)//非Label控件模式
            {
                if (Convert.ToBoolean(buttonBackgroundImage))//按钮使能
                {
                    if (bHoverBackgroundDisplay)//显示
                    {
                        buttonBackgroundImage_Save = buttonBackgroundImage_Save & (~(CustomButton_BackgroundImage.Hover));

                        if (CustomButton_BackgroundImage.Focus == (CustomButton_BackgroundImage.Focus & buttonBackgroundImage_Save))
                        {
                            buttonBackgroundImage = CustomButton_BackgroundImage.Focus;
                        }
                        else
                        {
                            if (bControlSelected)
                            {
                                buttonBackgroundImage = CustomButton_BackgroundImage.Selected;
                            }
                            else
                            {
                                buttonBackgroundImage = CustomButton_BackgroundImage.Up;
                            }
                        }

                        _UpdateBackground(buttonBackgroundImage);
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：成为活动控件事件，更新按钮背景
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void CustomButton_Enter(object sender, EventArgs e)
        {
            if (!bLabelControlMode)//非Label控件模式
            {
                if (Convert.ToBoolean(buttonBackgroundImage))//按钮使能
                {
                    if (bFocusBackgroundDisplay)//显示
                    {
                        buttonBackgroundImage_Save = buttonBackgroundImage_Save | CustomButton_BackgroundImage.Focus;

                        buttonBackgroundImage = CustomButton_BackgroundImage.Focus;

                        _UpdateBackground(buttonBackgroundImage);
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：不再是活动控件事件，更新按钮背景
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void CustomButton_Leave(object sender, EventArgs e)
        {
            if (!bLabelControlMode)//非Label控件模式
            {
                if (Convert.ToBoolean(buttonBackgroundImage))//按钮使能
                {
                    if (bFocusBackgroundDisplay)//显示
                    {
                        buttonBackgroundImage_Save = buttonBackgroundImage_Save & (~(CustomButton_BackgroundImage.Focus));

                        if (CustomButton_BackgroundImage.Hover == (CustomButton_BackgroundImage.Hover & buttonBackgroundImage_Save))
                        {
                            buttonBackgroundImage = CustomButton_BackgroundImage.Hover;
                        }
                        else
                        {
                            if (bControlSelected)
                            {
                                buttonBackgroundImage = CustomButton_BackgroundImage.Selected;
                            }
                            else
                            {
                                buttonBackgroundImage = CustomButton_BackgroundImage.Up;
                            }
                        }

                        _UpdateBackground(buttonBackgroundImage);
                    }
                }
            }
        }
    }

    //

    //自定义按钮类型
    public enum CustomButton_Type
    {
        Normal = 1,//常规
        Switch = 2,//开关
    }

    //自定义按钮背景
    public enum CustomButton_BackgroundImage
    {
        Disable = 0x00000,//禁止（包含两种背景图像，释放和选择）
        Up = 0x00001,//释放
        Down = 0x00010,//按下
        Selected = 0x00100,//选择（包含两种背景图像，释放和选择）
        Focus = 0x01000,//焦点（包含两种背景图像，释放和选择）
        Hover = 0x10000,//停留（包含两种背景图像，释放和选择）
    }

    //自定义按钮绘制文本的方式
    public enum CustomButton_DrawTextType
    {
        Manual = 1,//手动（文本绘制的位置由用户指定）
        Automatic = 2,//自动（文本绘制的位置由系统根据对齐方式进行确定）
    }
}