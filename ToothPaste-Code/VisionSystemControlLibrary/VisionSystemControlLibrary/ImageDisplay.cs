/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：ImageDisplay.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：ImageDisplay控件

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

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;

namespace VisionSystemControlLibrary
{
    public partial class ImageDisplay : UserControl
    {
        //ImageFileName和bitmapToDraw属性只能选择其一进行设置，表示了显示的图像的来源
        //ImageInformation属性表示了显示的图像信息，说明如下：
        //1.若仅显示图像，而不显示标题栏和在图像上绘制文本，ImageInformation属性可不进行设置，但请将ShowTitle和DrawingText属性设置为false；
        //2.若需要在图像上绘制文本，请设置ImageInformation属性中的DateTimeImage，ErrorValue和StepValue数值，并将DrawingText属性设置为true
        //3.若需要显示标题栏，请设置ImageInformation属性中的Type、Name、Value、ValueDisplay、MinValue、MaxValue和CurrentValue数值
        //4.ImageInformation属性中的Valid数值为false，则显示默认图像，否则显示设置的来源的图像（若来源未设置，同样显示默认图像）。

        private Double dScale = 1.0;//属性，XY方向尺寸系数
        private Double dScale_X = 1.0;//属性，X方向尺寸系数
        private Double dScale_Y = 1.0;//属性，Y方向尺寸系数

        private VisionSystemClassLibrary.Enum.InterfaceLanguage language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;//属性，语言

        private Boolean bAutoShowTitle = true;//属性，是否自动显示标题栏。取值范围：true，是；false，否

        private Boolean bShowTitle = true;//属性，是否显示标题栏（bAutoShowTitle取值为false时，该变量有效）。取值范围：true，显示；false，不显示

        private Int32 iYOffset = 2;//属性，信息控件距离图像上边沿的距离

        private string sImageFilePath = ".\\ConfigData\\RejectsImage\\";//属性，图像文件路径
        private string sImageFileName = "1.jpg";//属性，图像文件名称

        private Bitmap bitmapDisplay = null;//属性，图像数据

        //

        private Double dImageScale = 1.0;//属性（只读），图像比例

        private Rectangle rectangleImage = new Rectangle(new Point(0, 0), new Size(640, 480));//属性（只读），实际显示的图像区域

        //

        private Size sizeControl = new Size();//存储控件原始大小

        private Size sizePictureBoxBackground = new Size();//存储背景控件原始大小

        private Point locationStatusBar = new Point();//存储标题栏控件原始位置
        private Double scaleStatusBar = 1.0;//存储标题栏控件原始XY方向尺寸系数
        private Double scaleStatusBar_X = 1.0;//存储标题栏控件原始X方向尺寸系数
        private Double scaleStatusBar_Y = 1.0;//存储标题栏控件原始Y方向尺寸系数

        //

        private Image imageControl = null;//属性（只读），控件图像

        //

        [Browsable(true), Description("点击控件时产生的事件"), Category("ImageDisplay 事件")]
        public event EventHandler Control_Click;//点击控件时产生的事件

        [Browsable(true), Description("双击控件时产生的事件"), Category("ImageDisplay 事件")]
        public event EventHandler Control_DoubleClick;//双击控件时产生的事件

        [Browsable(true), Description("鼠标按下时产生的事件"), Category("ImageDisplay 事件")]
        public event EventHandler Control_MouseDown;//点击控件时产生的事件

        [Browsable(true), Description("鼠标指针移动时产生的事件"), Category("ImageDisplay 事件")]
        public event EventHandler Control_MouseMove;//鼠标指针移动时产生的事件

        [Browsable(true), Description("鼠标释放时产生的事件"), Category("ImageDisplay 事件")]
        public event EventHandler Control_MouseUp;//鼠标释放时产生的事件

        //-----------------------------------------------------------------------
        // 功能说明：构造函数，初始化
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public ImageDisplay()
        {
            InitializeComponent();

            //

            sizeControl = ClientRectangle.Size;//存储控件原始大小

            sizePictureBoxBackground = ClientRectangle.Size;//存储背景控件原始大小

            locationStatusBar = statusBar.Location;//存储标题栏控件原始位置
            scaleStatusBar = statusBar.ControlScale;//存储标题栏控件原始XY方向尺寸系数
            scaleStatusBar_X = statusBar.ControlScale_X;//存储标题栏控件原始X方向尺寸系数
            scaleStatusBar_Y = statusBar.ControlScale_Y;//存储标题栏控件原始Y方向尺寸系数

            //

            pictureBoxBackground.SizeMode = PictureBoxSizeMode.Zoom;
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：ImageControl属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(false), Description("控件图像"), Category("ImageDisplay 通用")]
        public Image ImageControl
        {
            get//读取
            {
                return imageControl;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：PictureBoxControl属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(false), Description("图像控件"), Category("ImageDisplay 通用")]
        public PictureBox PictureBoxControl
        {
            get//读取
            {
                return pictureBoxBackground;
            }
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：PictureBoxControl属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(false), Description("图像比例"), Category("ImageDisplay 通用")]
        public Double ImageScale
        {
            get//读取
            {
                return dImageScale;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：PictureBoxControl属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(false), Description("实际显示的图像区域"), Category("ImageDisplay 通用")]
        public Rectangle RectangleImage
        {
            get//读取
            {
                return rectangleImage;
            }
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明： BackgroundColor属性
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("控件图像区域背景颜色"), Category("ImageDisplay 通用")]
        public Color BackgroundColor
        {
            get//读取
            {
                return pictureBoxBackground.BackColor;
            }
            set//设置
            {
                if (value != pictureBoxBackground.BackColor)
                {
                    pictureBoxBackground.BackColor = value;
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ControlSize属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("控件范围"), Category("ImageDisplay 通用")]
        public Size ControlSize
        {
            get//读取
            {
                return this.Size;
            }
            set//设置
            {
                if (value != this.Size)
                {
                    dScale_X = (double)value.Width / (double)sizeControl.Width;
                    dScale_Y = (double)value.Height / (double)sizeControl.Height;

                    statusBar.ControlScale_X = scaleStatusBar_X * dScale_X;//标题栏控件
                    statusBar.ControlScale_Y = scaleStatusBar_Y * dScale_Y;//标题栏控件

                    _SetSize();
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明： ControlScale属性
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("XY方向尺寸系数"), Category("ImageDisplay 通用")]
        public Double ControlScale
        {
            get//读取
            {
                return dScale;
            }
            set//设置
            {
                if (value != dScale)
                {
                    dScale = value;

                    statusBar.ControlScale = scaleStatusBar * value;

                    _SetSize();//设置控件大小
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ControlScale_X属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("X方向尺寸系数"), Category("ImageDisplay 通用")]
        public Double ControlScale_X
        {
            get//读取
            {
                return dScale_X;
            }
            set//设置
            {
                if (value != dScale_X)
                {
                    dScale_X = value;

                    statusBar.ControlScale_X = scaleStatusBar_X * value;

                    _SetSize();//设置控件大小
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ControlScale_Y属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("Y方向尺寸系数"), Category("ImageDisplay 通用")]
        public Double ControlScale_Y
        {
            get//读取
            {
                return dScale_Y;
            }
            set//设置
            {
                if (value != dScale_Y)
                {
                    dScale_Y = value;

                    statusBar.ControlScale_Y = scaleStatusBar_Y * value;

                    _SetSize();//设置控件大小
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Language属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("语言"), Category("ImageDisplay 通用")]
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

                    statusBar.Language = value;
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：AutoShowTitle属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("是否自动显示标题栏。取值范围：true，是；false，否"), Category("ImageDisplay 通用")]
        public Boolean AutoShowTitle
        {
            get//读取
            {
                return bAutoShowTitle;
            }
            set//设置
            {
                if (value != bAutoShowTitle)
                {
                    bAutoShowTitle = value;

                    //

                    if (!bAutoShowTitle)//手动
                    {
                        statusBar.Visible = bShowTitle;
                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ShowTitle属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("是否显示标题栏。取值范围：true，显示；false，不显示"), Category("ImageDisplay 通用")]
        public Boolean ShowTitle
        {
            get//读取
            {
                return bShowTitle;
            }
            set//设置
            {
                if (value != bShowTitle)
                {
                    bShowTitle = value;

                    //

                    if (!bAutoShowTitle)//手动
                    {
                        statusBar.Visible = bShowTitle;
                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ImageInfo属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("图像信息"), Category("ImageDisplay 通用")]
        public VisionSystemClassLibrary.Struct.ImageInformation Information
        {
            get//读取
            {
                return statusBar.Information;
            }
            set//设置
            {
                statusBar.Information = value;

                //

                if (bAutoShowTitle)//自动
                {
                    if (statusBar.Information.Valid)//图像有效
                    {
                        if (VisionSystemClassLibrary.Enum.ImageType.Pure == Information.Type)//纯图像（此时不显示标题栏，信息无意义）
                        {
                            bShowTitle = false;

                            statusBar.Visible = bShowTitle;
                        }
                        else//其它
                        {
                            bShowTitle = true;

                            statusBar.Visible = bShowTitle;
                        }
                    }
                    else//图像无效
                    {
                        bShowTitle = false;

                        statusBar.Visible = bShowTitle;
                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ImageFilePath属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("图像文件路径"), Category("ImageDisplay 通用")]
        public string ImageFilePath
        {
            get//读取
            {
                return sImageFilePath;
            }
            set//设置
            {
                if (value != sImageFilePath)
                {
                    sImageFilePath = value;

                    //

                    string sFileName = sImageFilePath + sImageFileName;

                    if (File.Exists(sFileName))//文件存在
                    {
                        bitmapDisplay = new Bitmap(sFileName);

                        _ShowImage();//显示图像
                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ImageFileName属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("图像文件名称"), Category("ImageDisplay 通用")]
        public string ImageFileName
        {
            get//读取
            {
                return sImageFileName;
            }
            set//设置
            {
                if (value != sImageFileName)
                {
                    sImageFileName = value;

                    //

                    string sFileName = sImageFilePath + sImageFileName;

                    if (File.Exists(sFileName))//文件存在
                    {
                        bitmapDisplay = new Bitmap(sFileName);

                        _ShowImage();//显示图像
                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：BitmapDisplay属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("图像数据"), Category("ImageDisplay 通用")]
        public Bitmap BitmapDisplay
        {
            get//读取
            {
                return bitmapDisplay;
            }
            set//设置
            {
                bitmapDisplay = value;

                //

                _ShowImage();//显示图像
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：YOffset属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("信息控件距离图像上边沿的距离"), Category("ImageDisplay 通用")]
        public Int32 YOffset
        {
            get//读取
            {
                return iYOffset;
            }
            set//设置
            {
                iYOffset = value;

                //

                _ShowImage();//显示图像
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：ColorStatusBarControlBackground属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("信息控件背景颜色"), Category("ImageDisplay 信息控件")]
        public Color ColorStatusBarControlBackground//属性
        {
            get//读取
            {
                return statusBar.ColorControlBackground;
            }
            set//设置
            {
                statusBar.ColorControlBackground = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：StatusBarControlSize属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("信息控件范围"), Category("ImageDisplay 信息控件")]
        public Size StatusBarControlSize
        {
            get//读取
            {
                return statusBar.ControlSize;
            }
            set//设置
            {
                statusBar.ControlSize = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：StatusBarControlScale属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("信息控件中的XY方向尺寸系数"), Category("ImageDisplay 信息控件")]
        public Double StatusBarControlScale
        {
            get//读取
            {
                return statusBar.ControlScale;
            }
            set//设置
            {
                statusBar.ControlScale = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：StatusBarControlScale_X属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("信息控件中的X方向尺寸系数"), Category("ImageDisplay 信息控件")]
        public Double StatusBarControlScale_X
        {
            get//读取
            {
                return statusBar.ControlScale_X;
            }
            set//设置
            {
                statusBar.ControlScale_X = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：StatusBarControlScale_Y属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("信息控件中的Y方向尺寸系数"), Category("ImageDisplay 信息控件")]
        public Double StatusBarControlScale_Y
        {
            get//读取
            {
                return statusBar.ControlScale_Y;
            }
            set//设置
            {
                statusBar.ControlScale_Y = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：MessageLocation属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("信息控件中的信息控件位置"), Category("ImageDisplay 信息控件")]
        public Point MessageLocation
        {
            get//读取
            {
                return statusBar.MessageLocation;
            }
            set//设置
            {
                statusBar.MessageLocation = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：MessageSize属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("信息控件中的信息控件范围"), Category("ImageDisplay 信息控件")]
        public Size MessageSize
        {
            get//读取
            {
                return statusBar.MessageSize;
            }
            set//设置
            {
                statusBar.MessageSize = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：MessageFont属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("信息控件中的信息控件字体"), Category("ImageDisplay 信息控件")]
        public Font MessageFont
        {
            get//读取
            {
                return statusBar.MessageFont;
            }
            set//设置
            {
                statusBar.MessageFont = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：SlotLocation属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("信息控件中的SLOT控件位置"), Category("ImageDisplay 信息控件")]
        public Point SlotLocation
        {
            get//读取
            {
                return statusBar.SlotLocation;
            }
            set//设置
            {
                statusBar.SlotLocation = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：SlotSize属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("信息控件中的SLOT控件范围"), Category("ImageDisplay 信息控件")]
        public Size SlotSize
        {
            get//读取
            {
                return statusBar.SlotSize;
            }
            set//设置
            {
                statusBar._SetSlotWidth(value);//应在下面的设置控件大小语句之前调用该语句，即首先设置每列的大小

                statusBar.SlotSize = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：MinValueLocation属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("信息控件中的最小值控件位置"), Category("ImageDisplay 信息控件")]
        public Point MinValueLocation
        {
            get//读取
            {
                return statusBar.MinValueLocation;
            }
            set//设置
            {
                statusBar.MinValueLocation = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：MinValueSize属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("信息控件中的最小值控件范围"), Category("ImageDisplay 信息控件")]
        public Size MinValueSize
        {
            get//读取
            {
                return statusBar.MinValueSize;
            }
            set//设置
            {
                statusBar.MinValueSize = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：MinValueFont属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("信息控件中的最小值控件字体"), Category("ImageDisplay 信息控件")]
        public Font MinValueFont
        {
            get//读取
            {
                return statusBar.MinValueFont;
            }
            set//设置
            {
                statusBar.MinValueFont = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：CurrentValueLocation属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("信息控件中的当前值控件位置"), Category("ImageDisplay 信息控件")]
        public Point CurrentValueLocation
        {
            get//读取
            {
                return statusBar.CurrentValueLocation;
            }
            set//设置
            {
                statusBar.CurrentValueLocation = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：CurrentValueSize属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("信息控件中的当前值控件范围"), Category("ImageDisplay 信息控件")]
        public Size CurrentValueSize
        {
            get//读取
            {
                return statusBar.CurrentValueSize;
            }
            set//设置
            {
                statusBar.CurrentValueSize = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：CurrentValueFont属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("信息控件中的当前值控件字体"), Category("ImageDisplay 信息控件")]
        public Font CurrentValueFont
        {
            get//读取
            {
                return statusBar.CurrentValueFont;
            }
            set//设置
            {
                statusBar.CurrentValueFont = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：MaxValueLocation属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("信息控件中的最大值控件位置"), Category("ImageDisplay 信息控件")]
        public Point MaxValueLocation
        {
            get//读取
            {
                return statusBar.MaxValueLocation;
            }
            set//设置
            {
                statusBar.MaxValueLocation = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：MaxValueSize属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("信息控件中的最大值控件范围"), Category("ImageDisplay 信息控件")]
        public Size MaxValueSize
        {
            get//读取
            {
                return statusBar.MaxValueSize;
            }
            set//设置
            {
                statusBar.MaxValueSize = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：MaxValueFont属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("信息控件中的最大值控件字体"), Category("ImageDisplay 信息控件")]
        public Font MaxValueFont
        {
            get//读取
            {
                return statusBar.MaxValueFont;
            }
            set//设置
            {
                statusBar.MaxValueFont = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：MessageLampLocation属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("信息控件中的信息指示控件位置"), Category("ImageDisplay 信息控件")]
        public Point MessageLampLocation
        {
            get//读取
            {
                return statusBar.MessageLampLocation;
            }
            set//设置
            {
                statusBar.MessageLampLocation = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：MessageLampSize属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("信息控件中的信息指示控件范围"), Category("ImageDisplay 信息控件")]
        public Size MessageLampSize
        {
            get//读取
            {
                return statusBar.MessageLampSize;
            }
            set//设置
            {
                statusBar.MessageLampSize = value;
            }
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：设置状态栏控件数值区域控件的每列的宽度（用于无法在设计器中设置属性的情况，如图像显示控件中）
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        public void _SetSlotWidth(Size size)
        {
            statusBar._SetSlotWidth(size);
        }

        //-----------------------------------------------------------------------
        // 功能说明：获取背景图像
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        public void _GetImage()
        {
            Image imageDisplay = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);
            Graphics graphicsDraw = Graphics.FromImage(imageDisplay);

            SolidBrush solidbrushDraw = new SolidBrush(BackColor);

            //绘制背景色

            graphicsDraw.FillRectangle(solidbrushDraw, ClientRectangle);//绘制

            //绘制背景

            if (null != bitmapDisplay)//有效
            {
                graphicsDraw.DrawImage(bitmapDisplay, ClientRectangle);//绘制
            } 

            //绘制标题栏

            statusBar._GetImage();//获取

            if (null != statusBar.ImageControl)//有效
            {
                graphicsDraw.DrawImage(statusBar.ImageControl, new Rectangle(statusBar.Location, statusBar.ControlSize));//绘制
            }

            statusBar._ReleaseImage();//释放

            //

            if (null != imageControl)//释放资源
            {
                imageControl.Dispose();
            }

            imageControl = (Image)imageDisplay.Clone();

            //

            graphicsDraw.Dispose();
            imageDisplay.Dispose();
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

        //-----------------------------------------------------------------------
        // 功能说明：设置控件大小
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _SetSize()
        {
            this.Size = new Size((int)(sizeControl.Width * dScale * dScale_X), (int)(sizeControl.Height * dScale * dScale_Y));//控件大小
            
            pictureBoxBackground.Size = new Size((int)(sizePictureBoxBackground.Width * dScale * dScale_X), (int)(sizePictureBoxBackground.Height * dScale * dScale_Y));//背景控件大小

            statusBar.Location = new Point((int)(locationStatusBar.X * dScale * dScale_X), (int)(locationStatusBar.Y * dScale * dScale_Y));//标题栏控件位置
        }

        //-----------------------------------------------------------------------
        // 功能说明：显示图像
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _ShowImage()
        {
            if (null != bitmapDisplay)//有效
            {
                dImageScale = (double)bitmapDisplay.Width / (double)this.Size.Width;

                //statusBar.Location = new Point(statusBar.Location.X, (int)((this.Size.Height - (int)((double)bitmapDisplay.Height / dImageScale)) / 2 + iYOffset));//标题栏控件范围
                statusBar.Location = new Point(statusBar.Location.X, iYOffset);//标题栏控件范围

                //

                pictureBoxBackground.Image = (Image)bitmapDisplay.Clone();//显示图像

                //

                rectangleImage = new Rectangle(new Point(0, (int)((this.Size.Height - (int)((double)bitmapDisplay.Height / dImageScale)) / 2)), new Size(this.Width, (int)((int)((double)bitmapDisplay.Height / dImageScale))));//实际显示的图像区域
            }
        }

        //事件

        //----------------------------------------------------------------------
        // 功能说明：鼠标点击控件事件，产生自定义事件
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void pictureBoxBackground_MouseClick(object sender, MouseEventArgs e)
        {
            if ((e.X >= rectangleImage.Location.X) && (e.X <= rectangleImage.Location.X + rectangleImage.Size.Width) && (e.Y >= rectangleImage.Location.Y) && (e.Y <= rectangleImage.Location.Y + rectangleImage.Size.Height))
            {
                CustomEventArgs customeventargs = new CustomEventArgs();

                customeventargs.IntValue[0] = (Int32)(e.X * dImageScale);
                customeventargs.IntValue[1] = (Int32)((e.Y - rectangleImage.Top) * dImageScale);

                customeventargs.IntValue[2] = e.X;
                customeventargs.IntValue[3] = e.Y - rectangleImage.Top;

                //

                if (null != Control_Click)//有效
                {
                    Control_Click(this, customeventargs);
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：鼠标双击控件事件，产生自定义事件
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void pictureBoxBackground_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if ((e.X >= rectangleImage.Location.X) && (e.X <= rectangleImage.Location.X + rectangleImage.Size.Width) && (e.Y >= rectangleImage.Location.Y) && (e.Y <= rectangleImage.Location.Y + rectangleImage.Size.Height))
            {
                CustomEventArgs customeventargs = new CustomEventArgs();

                customeventargs.IntValue[0] = (Int32)(e.X * dImageScale);
                customeventargs.IntValue[1] = (Int32)((e.Y - rectangleImage.Top) * dImageScale);

                customeventargs.IntValue[2] = e.X;
                customeventargs.IntValue[3] = e.Y - rectangleImage.Top;

                //

                if (null != Control_DoubleClick)//有效
                {
                    Control_DoubleClick(this, customeventargs);
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：鼠标按下事件，产生自定义事件
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void pictureBoxBackground_MouseDown(object sender, MouseEventArgs e)
        {
            if ((e.X >= rectangleImage.Location.X) && (e.X <= rectangleImage.Location.X + rectangleImage.Size.Width) && (e.Y >= rectangleImage.Location.Y) && (e.Y <= rectangleImage.Location.Y + rectangleImage.Size.Height))
            {
                CustomEventArgs customeventargs = new CustomEventArgs();

                customeventargs.IntValue[0] = (Int32)(e.X * dImageScale);
                customeventargs.IntValue[1] = (Int32)((e.Y - rectangleImage.Top) * dImageScale);

                customeventargs.IntValue[2] = e.X;
                customeventargs.IntValue[3] = e.Y - rectangleImage.Top;

                //

                if (null != Control_MouseDown)//有效
                {
                    Control_MouseDown(this, customeventargs);
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：鼠标指针在控件上移动事件，产生自定义事件
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void pictureBoxBackground_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.X >= rectangleImage.Location.X) && (e.X <= rectangleImage.Location.X + rectangleImage.Size.Width) && (e.Y >= rectangleImage.Location.Y) && (e.Y <= rectangleImage.Location.Y + rectangleImage.Size.Height))
            {
                if (e.Button == MouseButtons.Left)
                {
                    CustomEventArgs customeventargs = new CustomEventArgs();

                    customeventargs.IntValue[0] = (Int32)(e.X * dImageScale);
                    customeventargs.IntValue[1] = (Int32)((e.Y - rectangleImage.Top) * dImageScale);

                    customeventargs.IntValue[2] = e.X;
                    customeventargs.IntValue[3] = e.Y - rectangleImage.Top;

                    //

                    if (null != Control_MouseMove)//有效
                    {
                        Control_MouseMove(this, customeventargs);
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：鼠标释放事件，产生自定义事件
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void pictureBoxBackground_MouseUp(object sender, MouseEventArgs e)
        {
            if ((e.X >= rectangleImage.Location.X) && (e.X <= rectangleImage.Location.X + rectangleImage.Size.Width) && (e.Y >= rectangleImage.Location.Y) && (e.Y <= rectangleImage.Location.Y + rectangleImage.Size.Height))
            {
                CustomEventArgs customeventargs = new CustomEventArgs();

                customeventargs.IntValue[0] = (Int32)(e.X * dImageScale);
                customeventargs.IntValue[1] = (Int32)((e.Y - rectangleImage.Top) * dImageScale);

                customeventargs.IntValue[2] = e.X;
                customeventargs.IntValue[3] = e.Y - rectangleImage.Top;

                //

                if (null != Control_MouseUp)//有效
                {
                    Control_MouseUp(this, customeventargs);
                }
            }
        }
    }
}