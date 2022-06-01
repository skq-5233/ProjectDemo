/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：StatusBar.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：STATUSBAR控件

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

namespace VisionSystemControlLibrary
{
    public partial class StatusBar : UserControl
    {
        private Double dScale = 1.0;//属性，XY方向尺寸系数
        private Double dScale_X = 1.0;//属性，X方向尺寸系数
        private Double dScale_Y = 1.0;//属性，Y方向尺寸系数

        private VisionSystemClassLibrary.Enum.InterfaceLanguage language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;//属性，语言

        private VisionSystemClassLibrary.Struct.ImageInformation imageInformation = new VisionSystemClassLibrary.Struct.ImageInformation();//属性，图像信息

        //

        private String[][] sMessageText = new String[2][];//数值控件上显示的文本（[语言][包含的文本]）

        private Color[] colorMessage = new Color[3] { Color.SpringGreen, Color.FromArgb(192, 0, 0), Color.SpringGreen };//信息控件颜色（[图像类型]）

        //

        private Size sizeControl = new Size();//存储控件原始范围

        private Size sizeBackground = new Size();//存储背景控件原始范围

        private Point locationMessage = new Point();//存储信息控件原始位置
        private Size sizeMessage = new Size();//存储信息控件原始范围
        private Font fontMessage = new Font("微软雅黑", 14.25F, FontStyle.Regular, GraphicsUnit.Point, Convert.ToByte(134));//存储信息控件原始字体

        private Point locationSlot = new Point();//存储SLOT控件原始位置
        private Size sizeSlot = new Size();//存储SLOT控件原始范围

        private Point locationMinValue = new Point();//存储最小值控件原始位置
        private Size sizeMinValue = new Size();//存储最小值控件原始范围
        private Font fontMinValue = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point, Convert.ToByte(134));//存储最小值控件原始字体

        private Point locationCurrentValue = new Point();//存储当前值控件原始位置
        private Size sizeCurrentValue = new Size();//存储当前值控件原始范围
        private Font fontCurrentValue = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point, Convert.ToByte(134));//存储当前值控件原始字体

        private Point locationMaxValue = new Point();//存储最大值控件原始位置
        private Size sizeMaxValue = new Size();//存储最大值控件原始范围
        private Font fontMaxValue = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point, Convert.ToByte(134));//存储最大值控件原始字体

        private Point locationMessageLamp = new Point();//存储信息指示控件原始位置
        private Size sizeMessageLamp = new Size();//存储信息指示控件原始范围

        //

        private Image imageControl = null;//属性（只读），控件图像

        //

        //-----------------------------------------------------------------------
        // 功能说明：构造函数，初始化
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public StatusBar()
        {
            InitializeComponent();

            //

            sizeControl = ClientRectangle.Size;//存储控件原始范围

            sizeBackground = customButtonBackground.SizeButton;//存储背景控件原始范围

            locationMessage = labelMessage.Location;//存储信息控件原始位置
            sizeMessage = labelMessage.Size;//存储信息控件原始范围
            fontMessage = new Font(labelMessage.Font.Name, (float)(labelMessage.Font.Size), labelMessage.Font.Style, labelMessage.Font.Unit, labelMessage.Font.GdiCharSet);//信息控件字体

            locationSlot = customListHeaderSlot.Location;//存储SLOT控件原始位置
            sizeSlot = customListHeaderSlot.SizeListHeader;//存储SLOT控件原始范围

            locationMinValue = customButtonMinValue.Location;//存储最小值控件原始位置
            sizeMinValue = customButtonMinValue.SizeButton;//存储最小值控件原始范围
            fontMinValue = new Font(customButtonMinValue.FontText.Name, (float)(customButtonMinValue.FontText.Size), customButtonMinValue.FontText.Style, customButtonMinValue.FontText.Unit, customButtonMinValue.FontText.GdiCharSet);//最小值控件字体

            locationCurrentValue = customButtonCurrentValue.Location;//存储当前值控件原始位置
            sizeCurrentValue = customButtonCurrentValue.SizeButton;//存储当前值控件原始范围
            fontCurrentValue = new Font(customButtonCurrentValue.FontText.Name, (float)(customButtonCurrentValue.FontText.Size), customButtonCurrentValue.FontText.Style, customButtonCurrentValue.FontText.Unit, customButtonCurrentValue.FontText.GdiCharSet);//当前值控件字体

            locationMaxValue = customButtonMaxValue.Location;//存储最大值控件原始位置
            sizeMaxValue = customButtonMaxValue.SizeButton;//存储最大值控件原始范围
            fontMaxValue = new Font(customButtonMaxValue.FontText.Name, (float)(customButtonMaxValue.FontText.Size), customButtonMaxValue.FontText.Style, customButtonMaxValue.FontText.Unit, customButtonMaxValue.FontText.GdiCharSet);//最大值控件字体

            locationMessageLamp = customButtonMessageLamp.Location;//存储信息指示控件原始位置
            sizeMessageLamp = customButtonMessageLamp.SizeButton;//存储信息指示控件原始范围

            //

            FieldInfo[] fieldinfo = VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese.GetType().GetFields();//获取公共字段
            if (null != fieldinfo && 1 < fieldinfo.Length)//有效
            {
                Int32 i = 0;//循环控制变量

                sMessageText = new String[fieldinfo.Length - 1][];
                for (i = 0; i < fieldinfo.Length - 1; i++)
                {
                    sMessageText[i] = new String[3];
                }

                //

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] = customButtonMinValue.Chinese_TextDisplay[0];
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] = customButtonMinValue.English_TextDisplay[0];

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][1] = customButtonCurrentValue.Chinese_TextDisplay[0];
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][1] = customButtonCurrentValue.English_TextDisplay[0];

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][2] = customButtonMaxValue.Chinese_TextDisplay[0];
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][2] = customButtonMaxValue.English_TextDisplay[0];
            }
        }

        //属性

        //----------------------------------------------------------------------
        // 功能说明：ColorControlBackground属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("控件背景颜色"), Category("StatusBar 通用")]
        public Color ColorControlBackground//属性
        {
            get//读取
            {
                return this.BackColor;
            }
            set//设置
            {
                if (value != this.BackColor)
                {
                    this.BackColor = value;
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ControlSize属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("控件范围"), Category("StatusBar 通用")]
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

                    _SetSize();
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ImageControl属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(false), Description("控件图像"), Category("StatusBar 通用")]
        public Image ImageControl
        {
            get//读取
            {
                return imageControl;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ControlScale属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("XY方向尺寸系数"), Category("StatusBar 通用")]
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

                    //

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
        [Browsable(true), Description("X方向尺寸系数"), Category("StatusBar 通用")]
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

                    //

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
        [Browsable(true), Description("Y方向尺寸系数"), Category("StatusBar 通用")]
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

                    //

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
        [Browsable(true), Description("语言"), Category("StatusBar 通用")]
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

                    _SetLanguage();//设置语言
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ImageInformation属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("图像信息"), Category("StatusBar 通用")]
        public VisionSystemClassLibrary.Struct.ImageInformation Information
        {
            get//读取
            {
                return imageInformation;
            }
            set//设置
            {
                try
                {
                    if (null == value.Name && null == value.Value)//无效，用于设计器赋初值
                    {
                        imageInformation.Name = "OK";
                        imageInformation.ValueDisplay = true;
                        imageInformation.Type = VisionSystemClassLibrary.Enum.ImageType.Ok;
                    }
                    else//实际应用
                    {
                        imageInformation = value;
                    }

                    //

                    labelMessage.Text = imageInformation.Name;//信息名称

                    customButtonMinValue.Chinese_TextDisplay = new String[1] { sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] + imageInformation.MinValue.ToString() };//最小值
                    customButtonMinValue.English_TextDisplay = new String[1] { sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] + imageInformation.MinValue.ToString() };//最小值
                    customButtonMinValue.Visible = imageInformation.ValueDisplay;

                    customButtonCurrentValue.Chinese_TextDisplay = new String[1] { sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][1] + imageInformation.CurrentValue.ToString() };//当前值
                    customButtonCurrentValue.English_TextDisplay = new String[1] { sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][1] + imageInformation.CurrentValue.ToString() };//当前值
                    customButtonCurrentValue.Visible = imageInformation.ValueDisplay;

                    customButtonMaxValue.Chinese_TextDisplay = new String[1] { sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][2] + imageInformation.MaxValue.ToString() };//最大值
                    customButtonMaxValue.English_TextDisplay = new String[1] { sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][2] + imageInformation.MaxValue.ToString() };//最大值
                    customButtonMaxValue.Visible = imageInformation.ValueDisplay;

                    //

                    _SetValue();//设置控件数值
                }
                catch (System.Exception ex)
                {
                    //不执行操作
                }
            }
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：MessageLocation属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("信息控件位置"), Category("StatusBar 信息控件")]
        public Point MessageLocation
        {
            get//读取
            {
                return labelMessage.Location;
            }
            set//设置
            {
                if (value != labelMessage.Location)
                {
                    labelMessage.Location = value;
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：MessageSize属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("信息控件范围"), Category("StatusBar 信息控件")]
        public Size MessageSize
        {
            get//读取
            {
                return labelMessage.Size;
            }
            set//设置
            {
                if (value != labelMessage.Size)
                {
                    labelMessage.Size = value;
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：MessageFont属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("信息控件字体"), Category("StatusBar 信息控件")]
        public Font MessageFont
        {
            get//读取
            {
                return labelMessage.Font;
            }
            set//设置
            {
                if (value != labelMessage.Font)
                {
                    labelMessage.Font = value;
                }
            }
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：SlotLocation属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("SLOT控件位置"), Category("StatusBar SLOT控件")]
        public Point SlotLocation
        {
            get//读取
            {
                return customListHeaderSlot.Location;
            }
            set//设置
            {
                if (value != customListHeaderSlot.Location)
                {
                    customListHeaderSlot.Location = value;
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：SlotSize属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("SLOT控件范围"), Category("StatusBar SLOT控件")]
        public Size SlotSize
        {
            get//读取
            {
                return customListHeaderSlot.SizeListHeader;
            }
            set//设置
            {
                if (value != customListHeaderSlot.SizeListHeader)
                {
                    customListHeaderSlot.SizeListHeader = value;
                }
            }
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：MinValueLocation属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("最小值控件位置"), Category("StatusBar 最小值控件")]
        public Point MinValueLocation
        {
            get//读取
            {
                return customButtonMinValue.Location;
            }
            set//设置
            {
                if (value != customButtonMinValue.Location)
                {
                    customButtonMinValue.Location = value;
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：MinValueSize属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("最小值控件范围"), Category("StatusBar 最小值控件")]
        public Size MinValueSize
        {
            get//读取
            {
                return customButtonMinValue.SizeButton;
            }
            set//设置
            {
                if (value != customButtonMinValue.SizeButton)
                {
                    customButtonMinValue.SizeButton = value;
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：MinValueFont属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("最小值控件字体"), Category("StatusBar 最小值控件")]
        public Font MinValueFont
        {
            get//读取
            {
                return customButtonMinValue.FontText;
            }
            set//设置
            {
                if (value != customButtonMinValue.FontText)
                {
                    customButtonMinValue.FontText = value;
                }
            }
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：CurrentValueLocation属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("当前值控件位置"), Category("StatusBar 当前值控件")]
        public Point CurrentValueLocation
        {
            get//读取
            {
                return customButtonCurrentValue.Location;
            }
            set//设置
            {
                if (value != customButtonCurrentValue.Location)
                {
                    customButtonCurrentValue.Location = value;
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：CurrentValueSize属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("当前值控件范围"), Category("StatusBar 当前值控件")]
        public Size CurrentValueSize
        {
            get//读取
            {
                return customButtonCurrentValue.SizeButton;
            }
            set//设置
            {
                if (value != customButtonCurrentValue.SizeButton)
                {
                    customButtonCurrentValue.SizeButton = value;
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：CurrentValueFont属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("当前值控件字体"), Category("StatusBar 当前值控件")]
        public Font CurrentValueFont
        {
            get//读取
            {
                return customButtonCurrentValue.FontText;
            }
            set//设置
            {
                if (value != customButtonCurrentValue.FontText)
                {
                    customButtonCurrentValue.FontText = value;
                }
            }
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：MaxValueLocation属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("最大值控件位置"), Category("StatusBar 最大值控件")]
        public Point MaxValueLocation
        {
            get//读取
            {
                return customButtonMaxValue.Location;
            }
            set//设置
            {
                if (value != customButtonMaxValue.Location)
                {
                    customButtonMaxValue.Location = value;
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：MaxValueSize属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("最大值控件范围"), Category("StatusBar 最大值控件")]
        public Size MaxValueSize
        {
            get//读取
            {
                return customButtonMaxValue.SizeButton;
            }
            set//设置
            {
                if (value != customButtonMaxValue.SizeButton)
                {
                    customButtonMaxValue.SizeButton = value;
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：MaxValueFont属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("最大值控件字体"), Category("StatusBar 最大值控件")]
        public Font MaxValueFont
        {
            get//读取
            {
                return customButtonMaxValue.FontText;
            }
            set//设置
            {
                if (value != customButtonMaxValue.FontText)
                {
                    customButtonMaxValue.FontText = value;
                }
            }
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：MessageLampLocation属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("信息指示控件位置"), Category("StatusBar 信息指示控件")]
        public Point MessageLampLocation
        {
            get//读取
            {
                return customButtonMessageLamp.Location;
            }
            set//设置
            {
                if (value != customButtonMessageLamp.Location)
                {
                    customButtonMessageLamp.Location = value;
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：MessageLampSize属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("信息指示控件范围"), Category("StatusBar 信息指示控件")]
        public Size MessageLampSize
        {
            get//读取
            {
                return customButtonMessageLamp.SizeButton;
            }
            set//设置
            {
                if (value != customButtonMessageLamp.SizeButton)
                {
                    customButtonMessageLamp.SizeButton = value;
                }
            }
        }

        //函数

        //-----------------------------------------------------------------------
        // 功能说明：设置状态栏控件数值区域控件的每列的宽度（用于无法在设计器中设置属性的情况，如图像显示控件中）
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        public void _SetSlotWidth(Size size)
        {
            Int32 i = 0;//循环控制变量

            Int32 iColumnWidth_Temp = size.Width / customListHeaderSlot.ColumnNumber;//每列的宽度
            Int32 iColumnWidthExtra_Temp = size.Width % customListHeaderSlot.ColumnNumber;//每列的宽度余量（针对不能整除的情况，将其加至最后一列中）

            for (i = 0; i < customListHeaderSlot.ColumnNumber - 1; i++)//赋值
            {
                customListHeaderSlot.ColumnWidth[i] = iColumnWidth_Temp;//属性，每列的宽度（初始显示的每列的宽度，[0]表示最左侧的列）
            }
            customListHeaderSlot.ColumnWidth[i] = iColumnWidth_Temp + iColumnWidthExtra_Temp;//属性，每列的宽度（初始显示的每列的宽度，[0]表示最左侧的列）
        }

        //----------------------------------------------------------------------
        // 功能说明：获取背景图像
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _GetImage()
        {
            Image imageDisplay = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);
            Graphics graphicsDraw = Graphics.FromImage(imageDisplay);

            SolidBrush solidbrushDraw = new SolidBrush(BackColor);
            SolidBrush solidbrushText_Message = new SolidBrush(labelMessage.ForeColor);//绘制文本所使用的画刷

            StringFormat stringformatText = new StringFormat();//绘制文本所使用的格式
            stringformatText.Alignment = StringAlignment.Near;//设置格式
            stringformatText.LineAlignment = StringAlignment.Center;//设置格式

            //绘制背景色

            graphicsDraw.FillRectangle(solidbrushDraw, ClientRectangle);//绘制

            //绘制背景控件

            customButtonBackground._GetImage();//获取

            graphicsDraw.DrawImage(customButtonBackground.ImageControl, new Rectangle(customButtonBackground.Location, customButtonBackground.Size));//绘制

            customButtonBackground._ReleaseImage();//释放

            //绘制信息文本

            graphicsDraw.DrawString(labelMessage.Text, labelMessage.Font, solidbrushText_Message, new Rectangle(labelMessage.Location, labelMessage.Size), stringformatText);//信息名称

            //绘制SLOT

            customListHeaderSlot._GetImage();//获取

            graphicsDraw.DrawImage(customListHeaderSlot.ImageControl, new Rectangle(customListHeaderSlot.Location, customListHeaderSlot.Size));//绘制

            customListHeaderSlot._ReleaseImage();//释放

            //绘制最小值、当前值、最大值

            customButtonMinValue._GetImage();//获取

            if (customButtonMinValue.Visible)//最小值，显示
            {
                graphicsDraw.DrawImage(customButtonMinValue.ImageControl, new Rectangle(customButtonMinValue.Location, customButtonMinValue.Size));//绘制
            }

            customButtonMinValue._ReleaseImage();//释放

            //

            customButtonCurrentValue._GetImage();//获取

            if (customButtonCurrentValue.Visible)//当前值，显示
            {
                graphicsDraw.DrawImage(customButtonCurrentValue.ImageControl, new Rectangle(customButtonCurrentValue.Location, customButtonCurrentValue.Size));//绘制
            }

            customButtonCurrentValue._ReleaseImage();//释放

            //

            customButtonMaxValue._GetImage();//获取

            if (customButtonMaxValue.Visible)//最大值，显示
            {
                graphicsDraw.DrawImage(customButtonMaxValue.ImageControl, new Rectangle(customButtonMaxValue.Location, customButtonMaxValue.Size));//绘制
            }

            customButtonMaxValue._ReleaseImage();//释放

            //绘制信息指示

            customButtonMessageLamp._GetImage();//获取

            graphicsDraw.DrawImage(customButtonMessageLamp.ImageControl, new Rectangle(customButtonMessageLamp.Location, customButtonMessageLamp.Size));//绘制

            customButtonMessageLamp._ReleaseImage();//释放

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
        // 功能说明：设置语言
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _SetLanguage()
        {
            customButtonMinValue.Language = language;//最小值
            customButtonCurrentValue.Language = language;//当前值
            customButtonMaxValue.Language = language;//最大值
        }

        //-----------------------------------------------------------------------
        // 功能说明：设置控件数值
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _SetValue()
        {
            try
            {
                Int32 i = 0;//循环控制变量

                if (VisionSystemClassLibrary.Enum.ImageType.Ok == imageInformation.Type)//完好图像
                {
                    labelMessage.ForeColor = colorMessage[(Int32)VisionSystemClassLibrary.Enum.ImageType.Ok - 1];//信息控件

                    for (i = 0; i < imageInformation.Value.Length; i++ )
                    {
                        if (imageInformation.Value[i])//有效
                        {
                            customListHeaderSlot.BitmapBackgroundIndex[i] = 1;
                        } 
                        else//无效
                        {
                            customListHeaderSlot.BitmapBackgroundIndex[i] = 0;
                        }
                    }
                    customListHeaderSlot._Refresh();//刷新控件

                    customButtonMessageLamp.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//信息指示控件
                
                    customButtonCurrentValue.ForeColor = colorMessage[(Int32)VisionSystemClassLibrary.Enum.ImageType.Ok - 1];//当前值
                }
                else if (VisionSystemClassLibrary.Enum.ImageType.Error == imageInformation.Type)//缺陷图像
                {
                    labelMessage.ForeColor = colorMessage[(Int32)VisionSystemClassLibrary.Enum.ImageType.Error - 1];//信息控件

                    for (i = 0; i < imageInformation.Value.Length; i++ )
                    {
                        if (imageInformation.Value[i])//有效
                        {
                            customListHeaderSlot.BitmapBackgroundIndex[i] = 2;
                        } 
                        else//无效
                        {
                            customListHeaderSlot.BitmapBackgroundIndex[i] = 0;
                        }
                    }
                    customListHeaderSlot._Refresh();//刷新控件

                    customButtonMessageLamp.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Down;//信息指示控件

                    customButtonCurrentValue.ForeColor = colorMessage[(Int32)VisionSystemClassLibrary.Enum.ImageType.Error - 1];//当前值
                }
                else//VisionSystemClassLibrary.Enum.ImageType.Pure，纯图像（此时不显示标题栏，信息无意义）
                {
                    labelMessage.ForeColor = colorMessage[(Int32)VisionSystemClassLibrary.Enum.ImageType.Ok - 1];//信息控件

                    for (i = 0; i < imageInformation.Value.Length; i++)
                    {
                        if (imageInformation.Value[i])//有效
                        {
                            customListHeaderSlot.BitmapBackgroundIndex[i] = 1;
                        }
                        else//无效
                        {
                            customListHeaderSlot.BitmapBackgroundIndex[i] = 0;
                        }
                    }
                    customListHeaderSlot._Refresh();//刷新控件

                    customButtonMessageLamp.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//信息指示控件

                    customButtonCurrentValue.ForeColor = colorMessage[(Int32)VisionSystemClassLibrary.Enum.ImageType.Ok - 1];//当前值
                }
            }
            catch (System.Exception ex)
            {
                //不执行操作
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
            this.Size = new Size((int)(sizeControl.Width * dScale * dScale_X), (int)(sizeControl.Height * dScale * dScale_Y));//控件范围
            
            customButtonBackground.SizeButton = new Size((int)(sizeBackground.Width * dScale * dScale_X), (int)(sizeBackground.Height * dScale * dScale_Y));//背景控件范围

            labelMessage.Location = new Point((int)(locationMessage.X * dScale * dScale_X), (int)(locationMessage.Y * dScale * dScale_Y));//信息控件位置
            labelMessage.Size = new Size((int)(sizeMessage.Width * dScale * dScale_X), (int)(sizeMessage.Height * dScale * dScale_Y));//信息控件范围
            labelMessage.Font = new Font(fontMessage.Name, (float)(fontMessage.Size * dScale), fontMessage.Style, fontMessage.Unit, fontMessage.GdiCharSet);//信息控件字体

            customListHeaderSlot.Location = new Point((int)(locationSlot.X * dScale * dScale_X), (int)(locationSlot.Y * dScale * dScale_Y));//SLOT控件位置
            customListHeaderSlot.SizeListHeader = new Size((int)(sizeSlot.Width * dScale * dScale_X), (int)(sizeSlot.Height * dScale * dScale_Y));//SLOT控件范围

            customButtonMinValue.Location = new Point((int)(locationMinValue.X * dScale * dScale_X), (int)(locationMinValue.Y * dScale * dScale_Y));//最小值控件位置
            customButtonMinValue.SizeButton = new Size((int)(sizeMinValue.Width * dScale * dScale_X), (int)(sizeMinValue.Height * dScale * dScale_Y));//最小值控件范围
            customButtonMinValue.FontText = new Font(fontMinValue.Name, (float)(fontMinValue.Size * dScale), fontMinValue.Style, fontMinValue.Unit, fontMinValue.GdiCharSet);//最小值控件字体

            customButtonCurrentValue.Location = new Point((int)(locationCurrentValue.X * dScale * dScale_X), (int)(locationCurrentValue.Y * dScale * dScale_Y));//当前值控件位置
            customButtonCurrentValue.SizeButton = new Size((int)(sizeCurrentValue.Width * dScale * dScale_X), (int)(sizeCurrentValue.Height * dScale * dScale_Y));//当前值控件范围
            customButtonCurrentValue.FontText = new Font(fontCurrentValue.Name, (float)(fontCurrentValue.Size * dScale), fontCurrentValue.Style, fontCurrentValue.Unit, fontCurrentValue.GdiCharSet);//当前值控件字体

            customButtonMaxValue.Location = new Point((int)(locationMaxValue.X * dScale * dScale_X), (int)(locationMaxValue.Y * dScale * dScale_Y));//最大值控件位置
            customButtonMaxValue.SizeButton = new Size((int)(sizeMaxValue.Width * dScale * dScale_X), (int)(sizeMaxValue.Height * dScale * dScale_Y));//最大值控件范围
            customButtonMaxValue.FontText = new Font(fontMaxValue.Name, (float)(fontMaxValue.Size * dScale), fontMaxValue.Style, fontMaxValue.Unit, fontMaxValue.GdiCharSet);//最大值控件字体

            customButtonMessageLamp.Location = new Point((int)(locationMessageLamp.X * dScale * dScale_X), (int)(locationMessageLamp.Y * dScale * dScale_Y));//信息类型指示控件位置
            customButtonMessageLamp.SizeButton = new Size((int)(sizeMessageLamp.Width * dScale * dScale_X), (int)(sizeMessageLamp.Height * dScale * dScale_Y));//信息类型指示控件范围
        }
    }
}