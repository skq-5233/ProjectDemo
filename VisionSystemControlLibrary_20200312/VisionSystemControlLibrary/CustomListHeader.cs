/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：CustomListHeader.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：自定义列表中的列表头控件

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
using System.Reflection;

namespace VisionSystemControlLibrary
{
    public partial class CustomListHeader : UserControl
    {
        //该控件为自定义的列表头控件
        //设置ColumnNumber属性会重新申请ColumnWidth、ColumnName、RectReal、ColumnDefaultWidth和Custom_Draw的内存空间，因此ColumnNumber属性应该优先设置

        private VisionSystemClassLibrary.Enum.InterfaceLanguage language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;//属性，列表头控件绘制的文本所使用的语言（bDrawText取值为true时有效）

        private Boolean bLabelControlMode = false;//是否为Label控件模式。取值范围：true，是；false，否

        private Size sizeListHeader = new Size();//属性，控件大小

        private bool bEnabled = true;//属性，列表头使能状态。取值范围：true，使能；false：禁止

        private Int32 iColumnNumber = 6;//属性，列表头中包含的列数
        private Int32[] iColumnWidth = new Int32[6];//属性，每列的宽度（初始显示的每列的宽度，[0]表示最左侧的列）
        private Color[] colorColumnText_Enable = new Color[6];//属性，控件使能时，绘制每列的名称所使用的颜色（[0]表示最左侧的列）
        private Color[] colorColumnText_Disable = new Color[6];//属性，控件禁止时，绘制每列的名称所使用的颜色（[0]表示最左侧的列）
        private Rectangle[] rectReal = new Rectangle[6];//每列的实际区域（[0]表示最左侧的列）

        private Int32 iColumnMinWidth = 20;//向左或向右拖动垂直拆分条时，垂直拆分条距离左侧或右侧的最小距离

        private CustomDraw[] Custom_Draw = new CustomDraw[6];//绘图数据（[0]表示最左侧的列）

        //

        private Image imageToDraw = null;//控件图像

        private Image imageControl = null;//属性（只读），控件图像

        //背景

        private Bitmap[] bitmapBackground = new Bitmap[1] { null };//背景图像
        private Bitmap bitmapBackgroundWhole = null;//属性，整体背景图像
        private Int32 iBitmapBackgroundNumber = 1;//属性，背景图像数量
        private Int32[] iBitmapBackgroundIndex = new Int32[6];//属性，控件每列对应的图标索引值（从0开始）

        //文本

        private Font fontText = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point, Convert.ToByte(134));//属性，绘制文本所使用的字体
        private SolidBrush[] solidbrushColumnText_Enable = new SolidBrush[6];//控件使能时，绘制文本所使用的画刷（[0]表示最左侧的列）
        private SolidBrush[] solidbrushColumnText_Disable = new SolidBrush[6];//控件禁止时，绘制文本所使用的画刷（[0]表示最左侧的列）
        private StringFormat stringformatText = new StringFormat();//属性（水平对齐方式），绘制文本所使用的格式

        private Int32[] iCurrentColumnNameGroupIndex = new Int32[6];//属性，列表头控件当前绘制的文本所在组的索引值（从0开始）

        private String[][][] sColumnNameArray = new String[2][][];//列表头控件绘制的各个语言的文本数组（数组含义：[语言][[列数（[0]表示最左侧的列）][组（每组中仅单行显示文本）]]）
        private String[][] sColumnNameDisplay = new String[2][];//属性，列表头控件绘制的各个语言的原始文本数组（数组含义：[语言][[每列各个组]]）

        private Int32 iXOffSetValue = 7;//属性，列表头控件绘制文本的偏移量（绘制区域左侧 + 该数值）

        //图标

        private Boolean bDrawIcon = false;//属性，是否绘制图标。取值范围：true，是；false，否

        private Bitmap[] bitmapIcon = new Bitmap[1] { null };//属性，图标
        private Bitmap bitmapIconWhole = null;//属性，整体图标
        private Int32 iIconNumber = 1;//属性，图标数量（bDrawIcon取值为true时有效）
        private Int32[] iIconIndex = new Int32[6];//属性，控件每列对应的图标索引值（从0开始，bDrawIcon取值为true时有效）
        private Point[] pointIconLocation = new Point[6];//属性，控件每列的图标的位置（bDrawIcon取值为true时有效，拖动时该位置会改变）
        private Size[] sizeIconSize = new Size[6];//属性，控件每列的图标的范围（bDrawIcon取值为true时有效）

        //

        private Graphics graphicsDisplay;//绘图

        //

        private bool bMouseDown = false;//鼠标指针是否在垂直拆分区域内按下。取值范围：true，是；false，否
        private Point pointMouseDown;//鼠标指针在垂直拆分区域内按下时的位置
        private int iColumn_MouseDown = 0;//鼠标指针在垂直拆分区域内按下时所在的列（该列指的是，垂直拆分区域左侧的列。从0开始）

        [Browsable(true), Description("拖动列表头控件垂直拆分条或单击某一区域，使列表头控件各个区域发生改变时产生的事件"), Category("CustomListHeader 事件")]
        public event EventHandler DragControl;//拖动列表头控件垂直拆分条或单击某一区域，使列表头控件各个区域发生改变时产生的事件

        //----------------------------------------------------------------------
        // 功能说明：构造函数，初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public CustomListHeader()
        {
            InitializeComponent();

            //

            FieldInfo[] fieldinfo = VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese.GetType().GetFields();//获取公共字段
            if (null != fieldinfo && 1 < fieldinfo.Length)//有效
            {
                Int32 i = 0;//循环控制变量
                Int32 j = 0;//循环控制变量

                sColumnNameArray = new String[fieldinfo.Length - 1][][];////属性（只读），列表头控件绘制的各个语言的文本数组（数组含义：[语言][[列数（[0]表示最左侧的列）][组（每组中仅单行显示文本）]]）
                for (i = 0; i < fieldinfo.Length - 1; i++)
                {
                    sColumnNameArray[i] = new String[iColumnNumber][];

                    for (j = 0; j < iColumnNumber; j++)
                    {
                        sColumnNameArray[i][j] = new String[1];

                        sColumnNameArray[i][j][0] = "";
                    }
                }

                sColumnNameDisplay = new String[fieldinfo.Length - 1][];//属性，列表头控件绘制的各个语言的原始文本数组（数组含义：[语言][[每列各个组]]）
                for (i = 0; i < fieldinfo.Length - 1; i++)
                {
                    sColumnNameDisplay[i] = new String[iColumnNumber];

                    for (j = 0; j < iColumnNumber; j++)
                    {
                        sColumnNameDisplay[i][j] = "";
                    }
                }
            }

            //

            Size = new Size(835, 26);//更改控件大小

            sizeListHeader = this.Size;

            if (null != bitmapBackgroundWhole)
            {
                bitmapBackgroundWhole.Dispose();
            }
            bitmapBackgroundWhole = VisionSystemControlLibrary.Properties.Resources.ListHeader;//属性，整体背景图像

            if (null != bitmapBackground[0])
            {
                bitmapBackground[0].Dispose();
            }
            bitmapBackground[0] = VisionSystemControlLibrary.Properties.Resources.ListHeader;//背景图像

            stringformatText.Alignment = StringAlignment.Near;//设置格式
            stringformatText.LineAlignment = StringAlignment.Center;//设置格式
            stringformatText.Trimming = StringTrimming.EllipsisCharacter;//设置格式

            graphicsDisplay = CreateGraphics();//获取绘图资源

            //第1列
            iBitmapBackgroundIndex[0] = 0;//属性，控件每列对应的图标索引值（从0开始）

            iColumnWidth[0] = 134;//属性，每列的宽度（初始显示的每列的宽度，[0]表示最左侧的列）
            colorColumnText_Enable[0] = Color.FromArgb(0, 0, 0);//属性，控件使能时，绘制每列的名称所使用的颜色（[0]表示最左侧的列）
            colorColumnText_Disable[0] = Color.FromArgb(172, 168, 153);//属性，控件禁止时，绘制每列的名称所使用的颜色（[0]表示最左侧的列）
            solidbrushColumnText_Enable[0] = new SolidBrush(colorColumnText_Enable[0]);//控件使能时，绘制文本所使用的画刷（[0]表示最左侧的列）
            solidbrushColumnText_Disable[0] = new SolidBrush(colorColumnText_Disable[0]);//控件禁止时，绘制文本所使用的画刷（[0]表示最左侧的列）

            iCurrentColumnNameGroupIndex[0] = 0;//属性，列表头控件当前绘制的文本所在组的索引值（从0开始）
            sColumnNameDisplay[0][0] = "序列号";//属性，列表头控件绘制的各个语言的原始文本数组（数组含义：[语言][[每列各个组]]）
            sColumnNameDisplay[1][0] = "Serial Number";//属性，列表头控件绘制的各个语言的原始文本数组（数组含义：[语言][[每列各个组]]）

            Custom_Draw[0] = new CustomDraw();//创建
            Custom_Draw[0].RectLeft = new Rectangle(0, 3, 3, 20);//属性，背景图像左部填充区域（CustomDrawType.Horizontal、CustomDrawType.Central时有效）
            Custom_Draw[0].RectTop = new Rectangle(3, 0, 237, 3);//属性，背景图像上部填充区域（CustomDrawType.Vertical、CustomDrawType.Central时有效）
            Custom_Draw[0].RectRight = new Rectangle(240, 3, 3, 20);//属性，背景图像右部填充区域（CustomDrawType.Horizontal、CustomDrawType.Central时有效）
            Custom_Draw[0].RectBottom = new Rectangle(3, 23, 237, 3);//属性，背景图像下部填充区域（CustomDrawType.Vertical、CustomDrawType.Central时有效）
            Custom_Draw[0].RectLeftTop = new Rectangle(0, 0, 3, 3);//属性，背景图像左上部矩形区域（CustomDrawType.Central时有效）
            Custom_Draw[0].RectRightTop = new Rectangle(240, 0, 3, 3);//属性，背景图像右上部矩形区域（CustomDrawType.Central时有效）
            Custom_Draw[0].RectLeftBottom = new Rectangle(0, 23, 3, 3);//属性，背景图像左下部矩形区域（CustomDrawType.Central时有效）
            Custom_Draw[0].RectRightBottom = new Rectangle(240, 23, 3, 3);//属性，背景图像右下部矩形区域（CustomDrawType.Central时有效）
            Custom_Draw[0].RectFill = new Rectangle(3, 3, 237, 20);//属性，背景图像中部填充区域（用于填充整个按钮底部背景，CustomDrawType.Vertical、CustomDrawType.Horizontal和CustomDrawType.Central时有效）

            //第2列
            iBitmapBackgroundIndex[1] = 0;//属性，控件每列对应的图标索引值（从0开始）

            iColumnWidth[1] = 203;//属性，每列的宽度（初始显示的每列的宽度，[0]表示最左侧的列）
            colorColumnText_Enable[1] = Color.FromArgb(0, 0, 0);//属性，控件使能时，绘制每列的名称所使用的颜色（[0]表示最左侧的列）
            colorColumnText_Disable[1] = Color.FromArgb(172, 168, 153);//属性，控件禁止时，绘制每列的名称所使用的颜色（[0]表示最左侧的列）
            solidbrushColumnText_Enable[1] = new SolidBrush(colorColumnText_Enable[1]);//控件使能时，绘制文本所使用的画刷（[0]表示最左侧的列）
            solidbrushColumnText_Disable[1] = new SolidBrush(colorColumnText_Disable[1]);//控件禁止时，绘制文本所使用的画刷（[0]表示最左侧的列）

            iCurrentColumnNameGroupIndex[1] = 0;//属性，列表头控件当前绘制的文本所在组的索引值（从0开始）
            sColumnNameDisplay[0][1] = "MAC地址";//属性，列表头控件绘制的各个语言的原始文本数组（数组含义：[语言][[每列各个组]]）
            sColumnNameDisplay[1][1] = "MAC Address";//属性，列表头控件绘制的各个语言的原始文本数组（数组含义：[语言][[每列各个组]]）

            Custom_Draw[1] = new CustomDraw();//创建
            Custom_Draw[1].RectLeft = Custom_Draw[0].RectLeft;//属性，背景图像左部填充区域（CustomDrawType.Horizontal、CustomDrawType.Central时有效）
            Custom_Draw[1].RectTop = Custom_Draw[0].RectTop;//属性，背景图像上部填充区域（CustomDrawType.Vertical、CustomDrawType.Central时有效）
            Custom_Draw[1].RectRight = Custom_Draw[0].RectRight;//属性，背景图像右部填充区域（CustomDrawType.Horizontal、CustomDrawType.Central时有效）
            Custom_Draw[1].RectBottom = Custom_Draw[0].RectBottom;//属性，背景图像下部填充区域（CustomDrawType.Vertical、CustomDrawType.Central时有效）
            Custom_Draw[1].RectLeftTop = Custom_Draw[0].RectLeftTop;//属性，背景图像左上部矩形区域（CustomDrawType.Central时有效）
            Custom_Draw[1].RectRightTop = Custom_Draw[0].RectRightTop;//属性，背景图像右上部矩形区域（CustomDrawType.Central时有效）
            Custom_Draw[1].RectLeftBottom = Custom_Draw[0].RectLeftBottom;//属性，背景图像左下部矩形区域（CustomDrawType.Central时有效）
            Custom_Draw[1].RectRightBottom = Custom_Draw[0].RectRightBottom;//属性，背景图像右下部矩形区域（CustomDrawType.Central时有效）
            Custom_Draw[1].RectFill = Custom_Draw[0].RectFill;//属性，背景图像中部填充区域（用于填充整个按钮底部背景，CustomDrawType.Vertical、CustomDrawType.Horizontal和CustomDrawType.Central时有效）

            //第3列
            iBitmapBackgroundIndex[2] = 0;//属性，控件每列对应的图标索引值（从0开始）

            iColumnWidth[2] = 127;//属性，每列的宽度（初始显示的每列的宽度，[0]表示最左侧的列）
            colorColumnText_Enable[2] = Color.FromArgb(0, 0, 0);//属性，控件使能时，绘制每列的名称所使用的颜色（[0]表示最左侧的列）
            colorColumnText_Disable[2] = Color.FromArgb(172, 168, 153);//属性，控件禁止时，绘制每列的名称所使用的颜色（[0]表示最左侧的列）
            solidbrushColumnText_Enable[2] = new SolidBrush(colorColumnText_Enable[2]);//控件使能时，绘制文本所使用的画刷（[0]表示最左侧的列）
            solidbrushColumnText_Disable[2] = new SolidBrush(colorColumnText_Disable[2]);//控件禁止时，绘制文本所使用的画刷（[0]表示最左侧的列）

            iCurrentColumnNameGroupIndex[2] = 0;//属性，列表头控件当前绘制的文本所在组的索引值（从0开始）
            sColumnNameDisplay[0][2] = "IP地址";//属性，列表头控件绘制的各个语言的原始文本数组（数组含义：[语言][[每列各个组]]）
            sColumnNameDisplay[1][2] = "IP Address";//属性，列表头控件绘制的各个语言的原始文本数组（数组含义：[语言][[每列各个组]]）

            Custom_Draw[2] = new CustomDraw();//创建
            Custom_Draw[2].RectLeft = Custom_Draw[0].RectLeft;//属性，背景图像左部填充区域（CustomDrawType.Horizontal、CustomDrawType.Central时有效）
            Custom_Draw[2].RectTop = Custom_Draw[0].RectTop;//属性，背景图像上部填充区域（CustomDrawType.Vertical、CustomDrawType.Central时有效）
            Custom_Draw[2].RectRight = Custom_Draw[0].RectRight;//属性，背景图像右部填充区域（CustomDrawType.Horizontal、CustomDrawType.Central时有效）
            Custom_Draw[2].RectBottom = Custom_Draw[0].RectBottom;//属性，背景图像下部填充区域（CustomDrawType.Vertical、CustomDrawType.Central时有效）
            Custom_Draw[2].RectLeftTop = Custom_Draw[0].RectLeftTop;//属性，背景图像左上部矩形区域（CustomDrawType.Central时有效）
            Custom_Draw[2].RectRightTop = Custom_Draw[0].RectRightTop;//属性，背景图像右上部矩形区域（CustomDrawType.Central时有效）
            Custom_Draw[2].RectLeftBottom = Custom_Draw[0].RectLeftBottom;//属性，背景图像左下部矩形区域（CustomDrawType.Central时有效）
            Custom_Draw[2].RectRightBottom = Custom_Draw[0].RectRightBottom;//属性，背景图像右下部矩形区域（CustomDrawType.Central时有效）
            Custom_Draw[2].RectFill = Custom_Draw[0].RectFill;//属性，背景图像中部填充区域（用于填充整个按钮底部背景，CustomDrawType.Vertical、CustomDrawType.Horizontal和CustomDrawType.Central时有效）

            //第4列
            iBitmapBackgroundIndex[3] = 0;//属性，控件每列对应的图标索引值（从0开始）

            iColumnWidth[3] = 65;//属性，每列的宽度（初始显示的每列的宽度，[0]表示最左侧的列）
            colorColumnText_Enable[3] = Color.FromArgb(0, 0, 0);//属性，控件使能时，绘制每列的名称所使用的颜色（[0]表示最左侧的列）
            colorColumnText_Disable[3] = Color.FromArgb(172, 168, 153);//属性，控件禁止时，绘制每列的名称所使用的颜色（[0]表示最左侧的列）
            solidbrushColumnText_Enable[3] = new SolidBrush(colorColumnText_Enable[3]);//控件使能时，绘制文本所使用的画刷（[0]表示最左侧的列）
            solidbrushColumnText_Disable[3] = new SolidBrush(colorColumnText_Disable[3]);//控件禁止时，绘制文本所使用的画刷（[0]表示最左侧的列）

            iCurrentColumnNameGroupIndex[3] = 0;//属性，列表头控件当前绘制的文本所在组的索引值（从0开始）
            sColumnNameDisplay[0][3] = "端口";//属性，列表头控件绘制的各个语言的原始文本数组（数组含义：[语言][[每列各个组]]）
            sColumnNameDisplay[1][3] = "Port";//属性，列表头控件绘制的各个语言的原始文本数组（数组含义：[语言][[每列各个组]]）

            Custom_Draw[3] = new CustomDraw();//创建
            Custom_Draw[3].RectLeft = Custom_Draw[0].RectLeft;//属性，背景图像左部填充区域（CustomDrawType.Horizontal、CustomDrawType.Central时有效）
            Custom_Draw[3].RectTop = Custom_Draw[0].RectTop;//属性，背景图像上部填充区域（CustomDrawType.Vertical、CustomDrawType.Central时有效）
            Custom_Draw[3].RectRight = Custom_Draw[0].RectRight;//属性，背景图像右部填充区域（CustomDrawType.Horizontal、CustomDrawType.Central时有效）
            Custom_Draw[3].RectBottom = Custom_Draw[0].RectBottom;//属性，背景图像下部填充区域（CustomDrawType.Vertical、CustomDrawType.Central时有效）
            Custom_Draw[3].RectLeftTop = Custom_Draw[0].RectLeftTop;//属性，背景图像左上部矩形区域（CustomDrawType.Central时有效）
            Custom_Draw[3].RectRightTop = Custom_Draw[0].RectRightTop;//属性，背景图像右上部矩形区域（CustomDrawType.Central时有效）
            Custom_Draw[3].RectLeftBottom = Custom_Draw[0].RectLeftBottom;//属性，背景图像左下部矩形区域（CustomDrawType.Central时有效）
            Custom_Draw[3].RectRightBottom = Custom_Draw[0].RectRightBottom;//属性，背景图像右下部矩形区域（CustomDrawType.Central时有效）
            Custom_Draw[3].RectFill = Custom_Draw[0].RectFill;//属性，背景图像中部填充区域（用于填充整个按钮底部背景，CustomDrawType.Vertical、CustomDrawType.Horizontal和CustomDrawType.Central时有效）

            //第5列
            iBitmapBackgroundIndex[4] = 0;//属性，控件每列对应的图标索引值（从0开始）

            iColumnWidth[4] = 159;//属性，每列的宽度（初始显示的每列的宽度，[0]表示最左侧的列）
            colorColumnText_Enable[4] = Color.FromArgb(0, 0, 0);//属性，控件使能时，绘制每列的名称所使用的颜色（[0]表示最左侧的列）
            colorColumnText_Disable[4] = Color.FromArgb(172, 168, 153);//属性，控件禁止时，绘制每列的名称所使用的颜色（[0]表示最左侧的列）
            solidbrushColumnText_Enable[4] = new SolidBrush(colorColumnText_Enable[4]);//控件使能时，绘制文本所使用的画刷（[0]表示最左侧的列）
            solidbrushColumnText_Disable[4] = new SolidBrush(colorColumnText_Disable[4]);//控件禁止时，绘制文本所使用的画刷（[0]表示最左侧的列）

            iCurrentColumnNameGroupIndex[4] = 0;//属性，列表头控件当前绘制的文本所在组的索引值（从0开始）
            sColumnNameDisplay[0][4] = "固件版本";//属性，列表头控件绘制的各个语言的原始文本数组（数组含义：[语言][[每列各个组]]）
            sColumnNameDisplay[1][4] = "Firmware Version";//属性，列表头控件绘制的各个语言的原始文本数组（数组含义：[语言][[每列各个组]]）

            Custom_Draw[4] = new CustomDraw();//创建
            Custom_Draw[4].RectLeft = Custom_Draw[0].RectLeft;//属性，背景图像左部填充区域（CustomDrawType.Horizontal、CustomDrawType.Central时有效）
            Custom_Draw[4].RectTop = Custom_Draw[0].RectTop;//属性，背景图像上部填充区域（CustomDrawType.Vertical、CustomDrawType.Central时有效）
            Custom_Draw[4].RectRight = Custom_Draw[0].RectRight;//属性，背景图像右部填充区域（CustomDrawType.Horizontal、CustomDrawType.Central时有效）
            Custom_Draw[4].RectBottom = Custom_Draw[0].RectBottom;//属性，背景图像下部填充区域（CustomDrawType.Vertical、CustomDrawType.Central时有效）
            Custom_Draw[4].RectLeftTop = Custom_Draw[0].RectLeftTop;//属性，背景图像左上部矩形区域（CustomDrawType.Central时有效）
            Custom_Draw[4].RectRightTop = Custom_Draw[0].RectRightTop;//属性，背景图像右上部矩形区域（CustomDrawType.Central时有效）
            Custom_Draw[4].RectLeftBottom = Custom_Draw[0].RectLeftBottom;//属性，背景图像左下部矩形区域（CustomDrawType.Central时有效）
            Custom_Draw[4].RectRightBottom = Custom_Draw[0].RectRightBottom;//属性，背景图像右下部矩形区域（CustomDrawType.Central时有效）
            Custom_Draw[4].RectFill = Custom_Draw[0].RectFill;//属性，背景图像中部填充区域（用于填充整个按钮底部背景，CustomDrawType.Vertical、CustomDrawType.Horizontal和CustomDrawType.Central时有效）

            //第6列
            iBitmapBackgroundIndex[5] = 0;//属性，控件每列对应的图标索引值（从0开始）

            iColumnWidth[5] = 147;//属性，每列的宽度（初始显示的每列的宽度，[0]表示最左侧的列）
            colorColumnText_Enable[5] = Color.FromArgb(0, 0, 0);//属性，控件使能时，绘制每列的名称所使用的颜色（[0]表示最左侧的列）
            colorColumnText_Disable[5] = Color.FromArgb(172, 168, 153);//属性，控件禁止时，绘制每列的名称所使用的颜色（[0]表示最左侧的列）
            solidbrushColumnText_Enable[5] = new SolidBrush(colorColumnText_Enable[5]);//控件使能时，绘制文本所使用的画刷（[0]表示最左侧的列）
            solidbrushColumnText_Disable[5] = new SolidBrush(colorColumnText_Disable[5]);//控件禁止时，绘制文本所使用的画刷（[0]表示最左侧的列）

            iCurrentColumnNameGroupIndex[5] = 0;//属性，列表头控件当前绘制的文本所在组的索引值（从0开始）
            sColumnNameDisplay[0][5] = "设备名称";//属性，列表头控件绘制的各个语言的原始文本数组（数组含义：[语言][[每列各个组]]）
            sColumnNameDisplay[1][5] = "Device Name";//属性，列表头控件绘制的各个语言的原始文本数组（数组含义：[语言][[每列各个组]]）

            Custom_Draw[5] = new CustomDraw();//创建
            Custom_Draw[5].RectLeft = Custom_Draw[0].RectLeft;//属性，背景图像左部填充区域（CustomDrawType.Horizontal、CustomDrawType.Central时有效）
            Custom_Draw[5].RectTop = Custom_Draw[0].RectTop;//属性，背景图像上部填充区域（CustomDrawType.Vertical、CustomDrawType.Central时有效）
            Custom_Draw[5].RectRight = Custom_Draw[0].RectRight;//属性，背景图像右部填充区域（CustomDrawType.Horizontal、CustomDrawType.Central时有效）
            Custom_Draw[5].RectBottom = Custom_Draw[0].RectBottom;//属性，背景图像下部填充区域（CustomDrawType.Vertical、CustomDrawType.Central时有效）
            Custom_Draw[5].RectLeftTop = Custom_Draw[0].RectLeftTop;//属性，背景图像左上部矩形区域（CustomDrawType.Central时有效）
            Custom_Draw[5].RectRightTop = Custom_Draw[0].RectRightTop;//属性，背景图像右上部矩形区域（CustomDrawType.Central时有效）
            Custom_Draw[5].RectLeftBottom = Custom_Draw[0].RectLeftBottom;//属性，背景图像左下部矩形区域（CustomDrawType.Central时有效）
            Custom_Draw[5].RectRightBottom = Custom_Draw[0].RectRightBottom;//属性，背景图像右下部矩形区域（CustomDrawType.Central时有效）
            Custom_Draw[5].RectFill = Custom_Draw[0].RectFill;//属性，背景图像中部填充区域（用于填充整个按钮底部背景，CustomDrawType.Vertical、CustomDrawType.Horizontal和CustomDrawType.Central时有效）

            //

            _GetText();

            _Apply();//应用
        }

        //属性

        //-----------------------------------------------------------------------
        // 功能说明：LabelControlMode属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("是否为Label控件模式。取值范围：true，是；false，否"), Category("CustomListHeader 通用")]
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
        // 功能说明：ImageControl属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(false), Description("控件图像"), Category("CustomListHeader 通用")]
        public Image ImageControl
        {
            get//读取
            {
                return imageControl;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：SizeListHeader属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("控件大小"), Category("CustomListHeader 通用")]
        public Size SizeListHeader
        {
            get//读取
            {
                return sizeListHeader;
            }
            set//设置
            {
                if (value != sizeListHeader)//设置了新的数值
                {
                    sizeListHeader = value;

                    this.Size = value;

                    //

                    graphicsDisplay.Dispose();//释放

                    graphicsDisplay = CreateGraphics();//获取绘图资源

                    //

                    //_GetColumnWidth();//获取列表头宽度数值

                    //

                    Int32 i = 0;//循环控制变量

                    for (i = 0; i < iColumnNumber; i++)//赋值
                    {
                        if (0 == i)//第1列
                        {
                            pointIconLocation[i] = new Point(ClientRectangle.Left, ClientRectangle.Top);//属性，控件每列的图标的位置（bDrawIcon取值为true时有效，拖动时该位置会改变）
                        }
                        else//其它列
                        {
                            pointIconLocation[i] = new Point(pointIconLocation[i - 1].X + iColumnWidth[i - 1], ClientRectangle.Top);//属性，控件每列的图标的位置（bDrawIcon取值为true时有效，拖动时该位置会改变）
                        }
                    }

                    //

                    _GetText();

                    //

                    _Apply();//应用
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Language属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("列表头控件绘制的文本所使用的语言"), Category("CustomListHeader 通用")]
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

                    _UpdateBackground();//更新背景图像
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ListHeaderEnabled属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("列表头使能状态。取值范围：true，使能；false：禁止"), Category("CustomListHeader 通用")]
        public bool ListHeaderEnabled//属性
        {
            get//读取
            {
                return bEnabled;
            }
            set//设置
            {
                if (bEnabled != value)//设置了新的数值
                {
                    bEnabled = value;

                    //

                    _UpdateBackground();//更新背景图像
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ColumnNumber属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("列表头中包含的列数"), Category("CustomListHeader 通用")]
        public int ColumnNumber//属性
        {
            get//读取
            {
                return iColumnNumber;
            }
            set//设置
            {
                if (value != iColumnNumber)//设置了新的数值
                {
                    if (0 < value)//有效
                    {
                        iColumnNumber = value;

                        Int32 i = 0;//循环控制变量
                        Int32 j = 0;//循环控制变量

                        if (iColumnWidth.Length != iColumnNumber)
                        {
                            iColumnWidth = new int[iColumnNumber];//属性，每列的宽度（初始显示的每列的宽度，[0]表示最左侧的列）

                            //

                            _GetColumnWidth();//获取列表头宽度数值
                        }

                        //

                        if (colorColumnText_Enable.Length != iColumnNumber)
                        {
                            colorColumnText_Enable = new Color[iColumnNumber];//属性，控件使能时，绘制每列的名称所使用的颜色（[0]表示最左侧的列）

                            //

                            for (i = 0; i < iColumnNumber; i++)//创建
                            {
                                colorColumnText_Enable[i] = Color.FromArgb(0, 0, 0);//属性，控件使能时，绘制每列的名称所使用的颜色（[0]表示最左侧的列）
                            }
                        }

                        if (colorColumnText_Disable.Length != iColumnNumber)
                        {
                            colorColumnText_Disable = new Color[iColumnNumber];//属性，控件禁止时，绘制每列的名称所使用的颜色（[0]表示最左侧的列）

                            //

                            for (i = 0; i < iColumnNumber; i++)//创建
                            {
                                colorColumnText_Disable[i] = Color.FromArgb(172, 168, 153);//属性，控件禁止时，绘制每列的名称所使用的颜色（[0]表示最左侧的列）
                            }
                        }

                        if (solidbrushColumnText_Enable.Length != iColumnNumber)
                        {
                            solidbrushColumnText_Enable = new SolidBrush[iColumnNumber];//控件使能时，绘制文本所使用的画刷（[0]表示最左侧的列）

                            //

                            for (i = 0; i < iColumnNumber; i++)//创建
                            {
                                solidbrushColumnText_Enable[i] = new SolidBrush(colorColumnText_Enable[i]);//控件使能时，绘制文本所使用的画刷（[0]表示最左侧的列）
                            }
                        }

                        if (solidbrushColumnText_Disable.Length != iColumnNumber)
                        {
                            solidbrushColumnText_Disable = new SolidBrush[iColumnNumber];//控件禁止时，绘制文本所使用的画刷（[0]表示最左侧的列）

                            //

                            for (i = 0; i < iColumnNumber; i++)//创建
                            {
                                solidbrushColumnText_Disable[i] = new SolidBrush(colorColumnText_Disable[i]);//控件禁止时，绘制文本所使用的画刷（[0]表示最左侧的列）
                            }
                        }

                        if (rectReal.Length != iColumnNumber)
                        {
                            rectReal = new Rectangle[iColumnNumber];//每列的实际区域（[0]表示最左侧的列）

                            //

                            for (i = 0; i < iColumnNumber; i++)//创建
                            {
                                rectReal[i] = new Rectangle();//每列的实际区域（[0]表示最左侧的列）
                            }
                        }

                        if (Custom_Draw.Length != iColumnNumber)
                        {
                            Custom_Draw = new CustomDraw[iColumnNumber];//绘图数据（[0]表示最左侧的列）

                            //

                            for (i = 0; i < iColumnNumber; i++)//创建
                            {
                                Custom_Draw[i] = new CustomDraw();
                            }
                        }

                        if (iCurrentColumnNameGroupIndex.Length != iColumnNumber)
                        {
                            iCurrentColumnNameGroupIndex = new Int32[iColumnNumber];//属性，列表头控件当前绘制的文本所在组的索引值（从0开始）

                            //

                            for (i = 0; i < iColumnNumber; i++)//创建
                            {
                                iCurrentColumnNameGroupIndex[i] = 0;//属性，列表头控件当前绘制的文本所在组的索引值（从0开始）
                            }
                        }

                        for (i = 0; i < sColumnNameArray.Length; i++)
                        {
                            if (sColumnNameArray[i].Length != iColumnNumber)
                            {
                                sColumnNameArray[i] = new String[iColumnNumber][];

                                for (j = 0; j < iColumnNumber; j++)
                                {
                                    sColumnNameArray[i][j] = new String[1];

                                    sColumnNameArray[i][j][0] = "";
                                }
                            }
                        }

                        for (i = 0; i < sColumnNameDisplay.Length; i++)
                        {
                            if (sColumnNameDisplay[i].Length != iColumnNumber)
                            {
                                sColumnNameDisplay[i] = new String[iColumnNumber];

                                //

                                if (i == (Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1)
                                {
                                    for (j = 0; j < iColumnNumber; j++)//赋值
                                    {
                                        sColumnNameDisplay[i][j] = "列 " + (j + 1).ToString();//属性，列表头控件绘制的各个语言的原始文本数组（数组含义：[语言][[每列各个组]]）
                                    }
                                }
                                else if (i == (Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1)
                                {
                                    for (j = 0; j < iColumnNumber; j++)//赋值
                                    {
                                        sColumnNameDisplay[i][j] = "COLUMN " + (j + 1).ToString();//属性，列表头控件绘制的各个语言的原始文本数组（数组含义：[语言][[每列各个组]]）
                                    }
                                }
                                else//其它（默认中文）
                                {
                                    for (j = 0; j < iColumnNumber; j++)//赋值
                                    {
                                        sColumnNameDisplay[i][j] = "列 " + (j + 1).ToString();//属性，列表头控件绘制的各个语言的原始文本数组（数组含义：[语言][[每列各个组]]）
                                    }
                                }
                            }
                        }

                        if (iIconIndex.Length != iColumnNumber)
                        {
                            iIconIndex = new Int32[iColumnNumber];//属性，控件每列对应的图标索引值（从0开始，bDrawIcon取值为true时有效）

                            //

                            for (i = 0; i < iColumnNumber; i++)//创建
                            {
                                iIconIndex[i] = 0;//属性，控件每列对应的图标索引值（从0开始，bDrawIcon取值为true时有效）
                            }
                        }

                        if (pointIconLocation.Length != iColumnNumber)
                        {
                            pointIconLocation = new Point[iColumnNumber];//属性，控件每列的图标的位置（bDrawIcon取值为true时有效，拖动时该位置会改变）

                            //

                            for (i = 0; i < iColumnNumber; i++)//赋值
                            {
                                if (0 == i)//第1列
                                {
                                    pointIconLocation[i] = new Point(ClientRectangle.Left, ClientRectangle.Top);//属性，控件每列的图标的位置（bDrawIcon取值为true时有效，拖动时该位置会改变）
                                }
                                else//其它列
                                {
                                    pointIconLocation[i] = new Point(pointIconLocation[i - 1].X + iColumnWidth[i - 1], ClientRectangle.Top);//属性，控件每列的图标的位置（bDrawIcon取值为true时有效，拖动时该位置会改变）
                                }
                            }
                        }

                        if (sizeIconSize.Length != iColumnNumber)
                        {
                            sizeIconSize = new Size[iColumnNumber];//属性，控件每列的图标的范围（bDrawIcon取值为true时有效）
                        }

                        if (iBitmapBackgroundIndex.Length != iColumnNumber)
                        {
                            iBitmapBackgroundIndex = new Int32[iColumnNumber];//属性，控件每列对应的图标索引值（从0开始）

                            //

                            for (i = 0; i < iColumnNumber; i++)//创建
                            {
                                iBitmapBackgroundIndex[i] = 0;//属性，控件每列对应的图标索引值（从0开始）
                            }
                        }

                        //

                        _GetText();

                        //

                        _Apply();//应用
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ColumnWidth属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("每列的宽度（初始显示的每列的宽度，[0]表示最左侧的列）"), Category("CustomListHeader 通用")]
        public int[] ColumnWidth//属性
        {
            get//读取
            {
                return iColumnWidth;
            }
            set//设置
            {
                iColumnWidth = value;

                if (value != null)//有效
                {
                    iColumnWidth = new Int32[value.Length];

                    //

                    value.CopyTo(iColumnWidth, 0);
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ColorControlBackground属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("控件背景颜色"), Category("CustomListHeader 通用")]
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

        //

        //----------------------------------------------------------------------
        // 功能说明：ColorColumnText_Enable属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("控件使能时，绘制每列的名称所使用的颜色（[0]表示最左侧的列）"), Category("CustomListHeader 文本")]
        public Color[] ColorColumnText_Enable//属性
        {
            get//读取
            {
                return colorColumnText_Enable;
            }
            set//设置
            {
                colorColumnText_Enable = value;

                if (value != null)//有效
                {
                    colorColumnText_Enable = new Color[value.Length];

                    solidbrushColumnText_Enable = new SolidBrush[value.Length];

                    //

                    value.CopyTo(colorColumnText_Enable, 0);

                    for (Int32 i = 0; i < value.Length; i++)//创建
                    {
                        solidbrushColumnText_Enable[i] = new SolidBrush(colorColumnText_Enable[i]);//控件使能时，绘制文本所使用的画刷（[0]表示最左侧的列）
                    }

                    //

                    _UpdateBackground();//更新背景图像
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ColorColumnText_Disable属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("控件禁止时，绘制每列的名称所使用的颜色（[0]表示最左侧的列）"), Category("CustomListHeader 文本")]
        public Color[] ColorColumnText_Disable//属性
        {
            get//读取
            {
                return colorColumnText_Disable;
            }
            set//设置
            {
                colorColumnText_Disable = value;

                if (value != null)//有效
                {
                    colorColumnText_Disable = new Color[value.Length];

                    solidbrushColumnText_Disable = new SolidBrush[value.Length];

                    //

                    value.CopyTo(colorColumnText_Disable, 0);

                    for (Int32 i = 0; i < value.Length; i++)//创建
                    {
                        solidbrushColumnText_Disable[i] = new SolidBrush(colorColumnText_Disable[i]);//控件禁止时，绘制文本所使用的画刷（[0]表示最左侧的列）
                    }

                    //

                    _UpdateBackground();//更新背景图像
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：FontText属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("绘制文本所使用的字体"), Category("CustomListHeader 文本")]
        public Font FontText//属性
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

                    _Apply();//应用
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：TextAlignment属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("列表头控件绘制文本的水平对齐方式"), Category("CustomListHeader 文本")]
        public StringAlignment TextAlignment
        {
            get//读取
            {
                return stringformatText.Alignment;
            }
            set//设置
            {
                if (value != stringformatText.Alignment)
                {
                    stringformatText.Alignment = value;

                    //

                    _Apply();//应用
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：CurrentColumnNameGroupIndex属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("列表头控件当前绘制的文本所在组的索引值（从0开始）"), Category("CustomListHeader 文本")]
        public Int32[] CurrentColumnNameGroupIndex//属性
        {
            get//读取
            {
                return iCurrentColumnNameGroupIndex;
            }
            set//设置
            {
                iCurrentColumnNameGroupIndex = value;

                if (value != null)
                {
                    iCurrentColumnNameGroupIndex = new Int32[value.Length];

                    //

                    value.CopyTo(iCurrentColumnNameGroupIndex, 0);

                    //

                    _UpdateBackground();//更新背景图像
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Chinese_ColumnNameDisplay属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("列表头控件绘制的中文语言的原始文本数组（数组含义：[语言][[每列各个组]]）"), Category("CustomListHeader 文本")]
        public String[] Chinese_ColumnNameDisplay
        {
            get//读取
            {
                return sColumnNameDisplay[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1];
            }
            set//设置
            {
                sColumnNameDisplay[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1] = value;

                //

                if (value != null)//有效
                {
                    sColumnNameDisplay[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1] = new String[value.Length];

                    //

                    value.CopyTo(sColumnNameDisplay[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1], 0);

                    //

                    _GetText();//获取文本

                    //

                    _UpdateBackground();//更新背景图像
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：English_ColumnNameDisplay属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("列表头控件绘制的英文语言的原始文本数组（数组含义：[语言][[每列各个组]]）"), Category("CustomListHeader 文本")]
        public String[] English_ColumnNameDisplay
        {
            get//读取
            {
                return sColumnNameDisplay[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1];
            }
            set//设置
            {
                sColumnNameDisplay[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1] = value;

                //

                if (null != value)//有效
                {
                    sColumnNameDisplay[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1] = new String[value.Length];

                    //

                    value.CopyTo(sColumnNameDisplay[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1], 0);

                    //

                    _GetText();//获取文本

                    //

                    _UpdateBackground();//更新背景图像
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：XOffSetValue属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("列表头控件绘制文本的偏移量（绘制区域左侧 + 该数值）"), Category("CustomListHeader 文本")]
        public Int32 XOffSetValue
        {
            get//读取
            {
                return iXOffSetValue;
            }
            set//设置
            {
                if (value != iXOffSetValue)
                {
                    iXOffSetValue = value;

                    //

                    _UpdateBackground();//更新背景图像
                }
            }
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：BitmapBackgroundNumber属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("背景图像数量"), Category("CustomListHeader 背景")]
        public Int32 BitmapBackgroundNumber
        {
            get//读取
            {
                return iBitmapBackgroundNumber;
            }
            set//设置
            {
                if (value != iBitmapBackgroundNumber)
                {
                    if (0 < iBitmapBackgroundNumber)//有效
                    {
                        iBitmapBackgroundNumber = value;

                        //

                        if (iBitmapBackgroundNumber != bitmapBackground.Length)
                        {
                            Int32 i = 0;//循环控制变量

                            //

                            if (null != bitmapBackground)
                            {
                                for (i = 0; i < bitmapBackground.Length; i++)//赋值
                                {
                                    bitmapBackground[i].Dispose();
                                }
                            }

                            //

                            bitmapBackground = new Bitmap[iBitmapBackgroundNumber];

                            for (i = 0; i < bitmapBackground.Length; i++)//赋值
                            {
                                bitmapBackground[i] = null;
                            }
                        }
                    }
                    else//无效
                    {
                        bitmapBackground = null;
                    }

                    _UpdateBackground();//更新
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：BitmapBackgroundIndex属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("控件每列对应的图标索引值（从0开始）"), Category("CustomListHeader 背景")]
        public Int32[] BitmapBackgroundIndex
        {
            get//读取
            {
                return iBitmapBackgroundIndex;
            }
            set//设置
            {
                iBitmapBackgroundIndex = value;

                if (value != null)//有效
                {
                    iBitmapBackgroundIndex = new Int32[value.Length];

                    value.CopyTo(iBitmapBackgroundIndex, 0);

                    //

                    _UpdateBackground();//更新
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：BitmapBackgroundWhole属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("整体背景图像"), Category("CustomListHeader 背景")]
        public Bitmap BitmapBackgroundWhole
        {
            get//读取
            {
                return bitmapBackgroundWhole;
            }
            set//设置
            {
                if (null != bitmapBackgroundWhole)//有效
                {
                    bitmapBackgroundWhole.Dispose();
                }

                bitmapBackgroundWhole = value;

                //

                if (null != bitmapBackgroundWhole)
                {
                    _UpdateBackground();//更新
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：DrawType属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("绘图类型"), Category("CustomListHeader 背景")]
        public CustomDrawType DrawType//属性
        {
            get//读取
            {
                return Custom_Draw[0].DrawType;
            }
            set//设置
            {
                if (value != Custom_Draw[0].DrawType)//设置了新的数值
                {
                    Int32 i = 0;//循环控制变量

                    for (i = 0; i < iColumnNumber; i++)//赋值
                    {
                        Custom_Draw[i].DrawType = value;
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
        [Browsable(true), Description("背景图像左部填充区域（CustomDrawType.Horizontal、CustomDrawType.Central时有效）"), Category("CustomListHeader 背景")]
        public Rectangle RectLeft//属性
        {
            get//读取
            {
                return Custom_Draw[0].RectLeft;
            }
            set//设置
            {
                if (value != Custom_Draw[0].RectLeft)//设置了新的数值
                {
                    Int32 i = 0;//循环控制变量

                    for (i = 0; i < iColumnNumber; i++)//赋值
                    {
                        Custom_Draw[i].RectLeft = value;
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
        [Browsable(true), Description("背景图像上部填充区域（CustomDrawType.Vertical、CustomDrawType.Central时有效）"), Category("CustomListHeader 背景")]
        public Rectangle RectTop//属性
        {
            get//读取
            {
                return Custom_Draw[0].RectTop;
            }
            set//设置
            {
                if (value != Custom_Draw[0].RectTop)//设置了新的数值
                {
                    Int32 i = 0;//循环控制变量

                    for (i = 0; i < iColumnNumber; i++)//赋值
                    {
                        Custom_Draw[i].RectTop = value;
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
        [Browsable(true), Description("背景图像右部填充区域（CustomDrawType.Horizontal、CustomDrawType.Central时有效）"), Category("CustomListHeader 背景")]
        public Rectangle RectRight//属性
        {
            get//读取
            {
                return Custom_Draw[0].RectRight;
            }
            set//设置
            {
                if (value != Custom_Draw[0].RectRight)//设置了新的数值
                {
                    Int32 i = 0;//循环控制变量

                    for (i = 0; i < iColumnNumber; i++)//赋值
                    {
                        Custom_Draw[i].RectRight = value;
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
        [Browsable(true), Description("背景图像下部填充区域（CustomDrawType.Vertical、CustomDrawType.Central时有效）"), Category("CustomListHeader 背景")]
        public Rectangle RectBottom//属性
        {
            get//读取
            {
                return Custom_Draw[0].RectBottom;
            }
            set//设置
            {
                if (value != Custom_Draw[0].RectBottom)//设置了新的数值
                {
                    Int32 i = 0;//循环控制变量

                    for (i = 0; i < iColumnNumber; i++)//赋值
                    {
                        Custom_Draw[i].RectBottom = value;
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
        [Browsable(true), Description("背景图像左上部矩形区域（CustomDrawType.Central时有效）"), Category("CustomListHeader 背景")]
        public Rectangle RectLeftTop//属性
        {
            get//读取
            {
                return Custom_Draw[0].RectLeftTop;
            }
            set//设置
            {
                if (value != Custom_Draw[0].RectLeftTop)//设置了新的数值
                {
                    Int32 i = 0;//循环控制变量

                    for (i = 0; i < iColumnNumber; i++)//赋值
                    {
                        Custom_Draw[i].RectLeftTop = value;
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
        [Browsable(true), Description("背景图像右上部矩形区域（CustomDrawType.Central时有效）"), Category("CustomListHeader 背景")]
        public Rectangle RectRightTop//属性
        {
            get//读取
            {
                return Custom_Draw[0].RectRightTop;
            }
            set//设置
            {
                if (value != Custom_Draw[0].RectRightTop)//设置了新的数值
                {
                    Int32 i = 0;//循环控制变量

                    for (i = 0; i < iColumnNumber; i++)//赋值
                    {
                        Custom_Draw[i].RectRightTop = value;
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
        [Browsable(true), Description("背景图像左下部矩形区域（CustomDrawType.Central时有效）"), Category("CustomListHeader 背景")]
        public Rectangle RectLeftBottom//属性
        {
            get//读取
            {
                return Custom_Draw[0].RectLeftBottom;
            }
            set//设置
            {
                if (value != Custom_Draw[0].RectLeftBottom)//设置了新的数值
                {
                    Int32 i = 0;//循环控制变量

                    for (i = 0; i < iColumnNumber; i++)//赋值
                    {
                        Custom_Draw[i].RectLeftBottom = value;
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
        [Browsable(true), Description("背景图像右下部矩形区域（CustomDrawType.Central时有效）"), Category("CustomListHeader 背景")]
        public Rectangle RectRightBottom//属性
        {
            get//读取
            {
                return Custom_Draw[0].RectRightBottom;
            }
            set//设置
            {
                if (value != Custom_Draw[0].RectRightBottom)//设置了新的数值
                {
                    Int32 i = 0;//循环控制变量

                    for (i = 0; i < iColumnNumber; i++)//赋值
                    {
                        Custom_Draw[i].RectRightBottom = value;
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
        [Browsable(true), Description("背景图像中部填充区域（用于填充整个按钮底部背景，CustomDrawType.Vertical、CustomDrawType.Horizontal和CustomDrawType.Central时有效）"), Category("CustomListHeader 背景")]
        public Rectangle RectFill//属性
        {
            get//读取
            {
                return Custom_Draw[0].RectFill;
            }
            set//设置
            {
                if (value != Custom_Draw[0].RectFill)//设置了新的数值
                {
                    Int32 i = 0;//循环控制变量

                    for (i = 0; i < iColumnNumber; i++)//赋值
                    {
                        Custom_Draw[i].RectFill = value;
                    }
                }
            }
        }

        //图标

        //-----------------------------------------------------------------------
        // 功能说明：IconNumber属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("图标数量"), Category("CustomListHeader 图标")]
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
                    }
                    else//无效
                    {
                        bitmapIcon = null;
                    }

                    _UpdateBackground();//更新
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：IconIndex属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("控件每列对应的图标索引值（从0开始，bDrawIcon取值为true时有效）"), Category("CustomListHeader 图标")]
        public Int32[] IconIndex
        {
            get//读取
            {
                return iIconIndex;
            }
            set//设置
            {
                iIconIndex = value;

                //

                if (value != null)//有效
                {
                    iIconIndex = new Int32[value.Length];

                    value.CopyTo(iIconIndex, 0);

                    //

                    _UpdateBackground();//更新
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：BitmapIcon属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("图标"), Category("CustomListHeader 图标")]
        public Bitmap[] BitmapIcon
        {
            get//读取
            {
                return bitmapIcon;
            }
            set//设置
            {
                if (null != value)//有效
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

                    bitmapIcon = new Bitmap[value.Length];

                    value.CopyTo(bitmapIcon, 0);

                    //

                    if (sizeIconSize.Length != bitmapIcon.Length)
                    {
                        sizeIconSize = new Size[bitmapIcon.Length];
                    }
                    
                    for (i = 0; i < sizeIconSize.Length; i++)//赋值
                    {
                        if (null != bitmapIcon[0])//有效
                        {
                            sizeIconSize[i] = bitmapIcon[0].Size;
                        }
                    }

                    //

                    _UpdateBackground();//更新
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：BitmapIconWhole属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("整体图标"), Category("CustomListHeader 图标")]
        public Bitmap BitmapIconWhole
        {
            get//读取
            {
                return bitmapIconWhole;
            }
            set//设置
            {
                if (null != bitmapIconWhole)//有效
                {
                    bitmapIconWhole.Dispose();
                }

                //

                bitmapIconWhole = value;

                if (null != bitmapIconWhole)//有效
                {
                    Size sizeIcon = new Size(bitmapIconWhole.Width / iIconNumber, bitmapIconWhole.Height);//属性，图标的大小

                    Int32 i = 0;//循环控制变量

                    for (i = 0; i < sizeIconSize.Length; i++)//赋值
                    {
                        sizeIconSize[i] = sizeIcon;
                    }

                    //

                    _UpdateBackground();//更新
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：IconLocation属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("图标位置"), Category("CustomListHeader 图标")]
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

                    _UpdateBackground();//更新
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：IconSize属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("图标的范围"), Category("CustomListHeader 图标")]
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

                    _UpdateBackground();//更新
                }
            }
        }

        //函数

        //----------------------------------------------------------------------
        // 功能说明：应用时调用，更新背景图像
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _Refresh()
        {
            _UpdateBackground();//更新背景图像
        }

        //----------------------------------------------------------------------
        // 功能说明：应用时调用，应用设置完成的属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _Apply()
        {
            Int32 i = 0;//循环控制变量

            for (i = 0; i < iColumnNumber; i++)//赋值
            {
                if (0 == i)//第1列
                {
                    Custom_Draw[0].RectDraw = new Rectangle(ClientRectangle.Left, ClientRectangle.Top, iColumnWidth[0], ClientRectangle.Height);//属性，绘制区域
                }
                else//其它列
                {
                    //Custom_Draw[i].RectDraw = new Rectangle(Custom_Draw[i - 1].RectDraw.Right + 1, ClientRectangle.Top, iColumnWidth[i] - 1, ClientRectangle.Height);//更新属性值
                    Custom_Draw[i].RectDraw = new Rectangle(Custom_Draw[i - 1].RectDraw.Right, ClientRectangle.Top, iColumnWidth[i], ClientRectangle.Height);//更新属性值
                }

                Custom_Draw[i].SizeImage = new Size(bitmapBackgroundWhole.Width / iBitmapBackgroundNumber, bitmapBackgroundWhole.Height);//属性，背景图像的大小

                rectReal[i] = Custom_Draw[i].RectDraw;//每列的实际区域（[0]表示最左侧的列）

                //

                Custom_Draw[i]._GetCustomDraw();//获取绘图数据
            }

            //

            _UpdateBackground();//更新背景图像
        }

        //----------------------------------------------------------------------
        // 功能说明：获取控件图像
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _GetImage()
        {
            _UpdateBackground(false);
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
        // 功能说明：拖动垂直拆分条
        // 输入参数：1.iWidth：拖动的距离
        //         2.iColumn：垂直拆分区域左侧的列的序号
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _DragControl(int iWidth, int iColumn)
        {
            int i = 0;//循环控制变量

            if (iWidth >= 0)//向右侧拖动
            {
                if (rectReal[iColumn].Right + iWidth <= ClientRectangle.Right - iColumnMinWidth)//未超出最小距离范围
                {
                    //更新每列的实际区域

                    rectReal[iColumn] = new Rectangle(rectReal[iColumn].Left, rectReal[iColumn].Top, rectReal[iColumn].Width + iWidth, rectReal[iColumn].Height);
                    pointIconLocation[iColumn] = new Point(pointIconLocation[iColumn].X + iWidth, pointIconLocation[iColumn].Y);//每列的图标的位置

                    for (i = iColumn; i < iColumnNumber - 1; i++)//更新各个区域
                    {
                        rectReal[i + 1] = new Rectangle(rectReal[i + 1].Left + iWidth, rectReal[i + 1].Top, rectReal[i + 1].Width, rectReal[i + 1].Height);
                        pointIconLocation[i + 1] = new Point(pointIconLocation[i + 1].X + iWidth, pointIconLocation[i + 1].Y);//每列的图标的位置
                    }

                    //更新绘制区域

                    //垂直拆分条左侧区域

                    Custom_Draw[iColumn].RectDraw = rectReal[iColumn];
                    Custom_Draw[iColumn]._GetCustomDraw();

                    //垂直拆分条右侧各个区域

                    for (i = iColumn; i < iColumnNumber - 1; i++)//更新各个区域
                    {
                        if (rectReal[i + 1].Left <= ClientRectangle.Right)//显示该区域
                        {
                            Custom_Draw[i + 1].RectDraw = rectReal[i + 1];

                            if (Custom_Draw[i + 1].RectDraw.Right > ClientRectangle.Right)//超出控件范围
                            {
                                Custom_Draw[i + 1].RectDraw = new Rectangle(Custom_Draw[i + 1].RectDraw.Left, Custom_Draw[i + 1].RectDraw.Top, ClientRectangle.Right - Custom_Draw[i + 1].RectDraw.Left, Custom_Draw[i + 1].RectDraw.Height);
                            }
                        }
                        else//不显示该区域
                        {
                            Custom_Draw[i + 1].RectDraw = rectReal[i + 1];
                        }

                        Custom_Draw[i + 1]._GetCustomDraw();
                    }

                    _UpdateBackground();//更新背景图像
                }
            }
            else//向左侧拖动
            {
                if (rectReal[iColumn].Right + iWidth >= rectReal[iColumn].Left + iColumnMinWidth)//判断是否达到最小距离
                {
                    //更新每列的实际区域

                    rectReal[iColumn] = new Rectangle(rectReal[iColumn].Left, rectReal[iColumn].Top, rectReal[iColumn].Width + iWidth, rectReal[iColumn].Height);
                    pointIconLocation[iColumn] = new Point(pointIconLocation[iColumn].X + iWidth, pointIconLocation[iColumn].Y);//每列的图标的位置

                    for (i = iColumn; i < iColumnNumber - 1; i++)//更新各个区域
                    {
                        rectReal[i + 1] = new Rectangle(rectReal[i + 1].Left + iWidth, rectReal[i + 1].Top, rectReal[i + 1].Width, rectReal[i + 1].Height);
                        pointIconLocation[i + 1] = new Point(pointIconLocation[i + 1].X + iWidth, pointIconLocation[i + 1].Y);//每列的图标的位置
                    }

                    //更新绘制区域

                    //垂直拆分条左侧区域

                    Custom_Draw[iColumn].RectDraw = rectReal[iColumn];
                    Custom_Draw[iColumn]._GetCustomDraw();

                    //垂直拆分条右侧各个区域

                    for (i = iColumn; i < iColumnNumber - 1; i++)//更新各个区域
                    {
                        if (rectReal[i + 1].Left < ClientRectangle.Right)//区域左侧显示
                        {
                            if (rectReal[i + 1].Right < ClientRectangle.Right)//区域右侧显示
                            {
                                if (i + 1 == iColumnNumber - 1)
                                {
                                    Custom_Draw[i + 1].RectDraw = new Rectangle(rectReal[i + 1].Left, rectReal[i + 1].Top, ClientRectangle.Right - rectReal[i + 1].Left, rectReal[i + 1].Height);

                                    rectReal[i + 1] = Custom_Draw[i + 1].RectDraw;
                                }
                                else
                                {
                                    Custom_Draw[i + 1].RectDraw = rectReal[i + 1];
                                }
                            }
                            else//区域右侧不显示
                            {
                                Custom_Draw[i + 1].RectDraw = new Rectangle(rectReal[i + 1].Left, rectReal[i + 1].Top, ClientRectangle.Right - rectReal[i + 1].Left, rectReal[i + 1].Height);
                            }
                        }
                        else//区域左侧不显示
                        {
                            Custom_Draw[i + 1].RectDraw = rectReal[i + 1];
                        }

                        Custom_Draw[i + 1]._GetCustomDraw();
                    }

                    _UpdateBackground();//更新背景图像
                }
            }

            //事件

            if (null != DragControl)//有效
            {
                DragControl(this, new CustomListHeader_EventArgs(iColumnNumber, rectReal));
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：更新背景图像
        // 输入参数：1.bDraw：是否绘制控件。取值范围：true，是（用于控件内部调用）；false，否（用于控件外部获取控件背景图像时使用）
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _UpdateBackground(Boolean bDraw = true)
        {
            Int32 i = 0;//循环控制变量

            if (null != bitmapBackgroundWhole)
            {
                Size sizeBitmap = new Size(bitmapBackgroundWhole.Width / iBitmapBackgroundNumber, bitmapBackgroundWhole.Height);//属性，背景图像的大小

                //

                for (i = 0; i < iBitmapBackgroundNumber; i++)//获取图标
                {
                    bitmapBackground[i] = bitmapBackgroundWhole.Clone(new Rectangle(new Point(i * sizeBitmap.Width, 0), sizeBitmap), bitmapBackgroundWhole.PixelFormat);//获取图标
                }
            }

            //

            if (null != bitmapIconWhole)
            {
                Size sizeIcon = new Size(bitmapIconWhole.Width / iIconNumber, bitmapIconWhole.Height);//属性，图标的大小

                for (i = 0; i < iIconNumber; i++)//获取图标
                {
                    bitmapIcon[i] = bitmapIconWhole.Clone(new Rectangle(new Point(i * sizeIcon.Width, 0), sizeIcon), bitmapIconWhole.PixelFormat);//获取图标
                }
            }

            //

            _GetBackgroundImage(bDraw);

            //

            if (null != bitmapBackground)
            {
                for (i = 0; i < bitmapBackground.Length; i++)//获取图标
                {
                    if (null != bitmapBackground[i])
                    {
                        bitmapBackground[i].Dispose();
                    }
                }
            }

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

            if (bDraw)
            {
                _Draw();

                if (null != imageToDraw)//释放资源
                {
                    imageToDraw.Dispose();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：获取背景图像
        // 输入参数：1.bDraw：是否绘制控件。取值范围：true，是（用于控件内部调用）；false，否（用于控件外部获取控件背景图像时使用）
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _GetBackgroundImage(Boolean bDraw)
        {
            //使用双倍缓冲绘图

            Image imageDisplay = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);
            Graphics graphicsDraw = Graphics.FromImage(imageDisplay);
            
            SolidBrush solidbrushDraw = new SolidBrush(BackColor);

            //绘制背景色

            graphicsDraw.FillRectangle(solidbrushDraw, ClientRectangle);//绘制当前选择的相机控件区域

            //绘制背景图像、文本、图标

            try
            {
                Int32 i = 0;//循环控制变量

                if (bEnabled)//使能
                {
                    for (i = 0; i < iColumnNumber; i++)//绘制背景图像
                    {
                        Custom_Draw[i]._Draw(graphicsDraw, bitmapBackground[iBitmapBackgroundIndex[i]]);

                        graphicsDraw.DrawString(sColumnNameArray[(Int32)language - 1][i][iCurrentColumnNameGroupIndex[i]], fontText, solidbrushColumnText_Enable[i], new Rectangle(Custom_Draw[i].RectDraw.Left + iXOffSetValue, Custom_Draw[i].RectDraw.Top, Custom_Draw[i].RectDraw.Width, Custom_Draw[i].RectDraw.Height), stringformatText);//绘制文本

                        //

                        if (bDrawIcon)//绘制图标
                        {
                            if (null != bitmapIcon[iIconIndex[i]])//图标有效
                            {
                                graphicsDraw.DrawImage(bitmapIcon[iIconIndex[i]], new Rectangle(pointIconLocation[i], sizeIconSize[i]));//绘制图标
                            }
                        }
                    }
                }
                else//禁止
                {
                    for (i = 0; i < iColumnNumber; i++)//绘制背景图像
                    {
                        Custom_Draw[i]._Draw(graphicsDraw, bitmapBackground[iBitmapBackgroundIndex[i]]);

                        graphicsDraw.DrawString(sColumnNameArray[(Int32)language - 1][i][iCurrentColumnNameGroupIndex[i]], fontText, solidbrushColumnText_Disable[i], new Rectangle(Custom_Draw[i].RectDraw.Left + iXOffSetValue, Custom_Draw[i].RectDraw.Top, Custom_Draw[i].RectDraw.Width, Custom_Draw[i].RectDraw.Height), stringformatText);//绘制文本

                        //

                        if (bDrawIcon)//绘制图标
                        {
                            if (null != bitmapIcon[iIconIndex[i]])//图标有效
                            {
                                graphicsDraw.DrawImage(bitmapIcon[iIconIndex[i]], new Rectangle(pointIconLocation[i], sizeIconSize[i]));//绘制图标
                            }
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
        // 功能说明：绘制控件、文本
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

        //----------------------------------------------------------------------
        // 功能说明：获取列表头宽度数值
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _GetColumnWidth()
        {
            Int32 i = 0;//循环控制变量

            Int32 iColumnWidth_Temp = Size.Width / iColumnNumber;//每列的宽度
            Int32 iColumnWidthExtra_Temp = Size.Width % iColumnNumber;//每列的宽度余量（针对不能整除的情况，将其加至最后一列中）

            for (i = 0; i < iColumnNumber - 1; i++)//赋值
            {
                iColumnWidth[i] = iColumnWidth_Temp;//属性，每列的宽度（初始显示的每列的宽度，[0]表示最左侧的列）
            }
            iColumnWidth[i] = iColumnWidth_Temp + iColumnWidthExtra_Temp;//属性，每列的宽度（初始显示的每列的宽度，[0]表示最左侧的列）
        }

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
                Int32 i = 0;
                Int32 j = 0;

                if (null != sColumnNameDisplay && null != sColumnNameDisplay[0])//有效
                {
                    //sColumnNameArray = new String[sColumnNameDisplay.Length][][];

                    for (i = 0; i < sColumnNameArray.Length; i++)
                    {
                        sColumnNameArray[i] = new String[iColumnNumber][];

                        for (j = 0; j < sColumnNameArray[i].Length; j++)
                        {
                            if ("" == sColumnNameDisplay[i][j])//无效
                            {
                                sColumnNameArray[i][j] = new String[1];

                                sColumnNameArray[i][j][0] = "";
                            }
                            else//有效
                            {
                                sColumnNameArray[i][j] = sColumnNameDisplay[i][j].Split('&');
                            }
                        }
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
        private void CustomListHeader_Paint(object sender, PaintEventArgs e)
        {
            _UpdateBackground();
        }

        //----------------------------------------------------------------------
        // 功能说明：双击列表头区域时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void CustomListHeader_DoubleClick(object sender, EventArgs e)
        {
            if (!bLabelControlMode)//非Label控件模式
            {
                if (bEnabled)//使能
                {
                    Int32 i = 0;//循环控制变量
                    Int32 iColumnDefaultWidth = 0;//列默认的宽度（双击列表头区域时，将双击的区域的宽度设为该值）

                    //

                    MouseEventArgs MouseEvent = (MouseEventArgs)e;//数值转换

                    for (i = 0; i < iColumnNumber; i++)//判断双击的区域
                    {
                        if (MouseEvent.X >= rectReal[i].Left && MouseEvent.X <= rectReal[i].Right)//所在区域
                        {
                            iColumnDefaultWidth = graphicsDisplay.MeasureString(sColumnNameArray[(Int32)language - 1][i][iCurrentColumnNameGroupIndex[i]], fontText, (SizeF)(ClientRectangle.Size), stringformatText).ToSize().Width;//绘制区域中的当前值（X轴、Y轴当前值在本区域显示）的像素区域
                            iColumnDefaultWidth += 10;//调试过程中发现返回的尺寸偏小，因此进行调整

                            if (rectReal[i].Width < iColumnDefaultWidth)//实际区域小于默认区域
                            {
                                break;
                            }
                        }
                    }
                    if (i < iColumnNumber)//有效区域
                    {
                        _DragControl(iColumnDefaultWidth - rectReal[i].Width, i);//模拟拖动垂直拆分条
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：按下鼠标键事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void CustomListHeader_MouseDown(object sender, MouseEventArgs e)
        {
            if (!bLabelControlMode)//非Label控件模式
            {
                if (bEnabled)//使能
                {
                    pointMouseDown = e.Location;//鼠标指针在垂直拆分区域内按下时的位置
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：释放鼠标键事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void CustomListHeader_MouseUp(object sender, MouseEventArgs e)
        {
            if (!bLabelControlMode)//非Label控件模式
            {
                if (bEnabled)//使能
                {
                    bMouseDown = false;//鼠标指针未在垂直拆分区域内按下

                    Cursor = Cursors.Default;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：鼠标指针移动事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void CustomListHeader_MouseMove(object sender, MouseEventArgs e)
        {
            if (!bLabelControlMode)//非Label控件模式
            {
                if (bEnabled)//使能
                {
                    int i = 0;//循环控制变量

                    if (!bMouseDown)//鼠标指针未在垂直拆分区域内按下
                    {
                        for (i = 0; i < iColumnNumber - 1; i++)//判断鼠标指针是否进入垂直拆分条区域
                        {
                            if (e.X >= rectReal[i].Right - 5 && e.X <= rectReal[i + 1].Left + 5)//垂直拆分区域
                            {
                                //进入垂直拆分区域

                                Cursor = Cursors.VSplit;//改变鼠标指针

                                if (MouseButtons.Left == e.Button)//鼠标左键按下
                                {
                                    iColumn_MouseDown = i;//保存此时的列号

                                    bMouseDown = true;//鼠标指针在垂直拆分区域内按下
                                }

                                break;
                            }
                        }
                        if (i >= iColumnNumber - 1)//未进入垂直拆分条区域
                        {
                            Cursor = Cursors.Default;
                        }
                    }

                    //

                    if (bMouseDown)//鼠标指针在垂直拆分区域内按下
                    {
                        int iWidth = e.X - pointMouseDown.X;//鼠标指针移动的距离

                        _DragControl(iWidth, iColumn_MouseDown);//拖动垂直拆分条

                        pointMouseDown.X = e.X;//更新坐标值
                    }
                }
            }
        }
    }

    //CustomListHeader_EventArgs类定义，拖动列表头控件垂直拆分条或单击某一区域，使列表头控件各个区域发生改变时产生的事件

    public class CustomListHeader_EventArgs : EventArgs
    {
        private int iColumnNumber = 6;//属性，列表头中包含的列数

        private Rectangle[] rectReal = new Rectangle[6];//每列的实际区域（[0]表示最左侧的列）

        //----------------------------------------------------------------------
        // 功能说明：构造函数
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public CustomListHeader_EventArgs()
        {
        }

        //----------------------------------------------------------------------
        // 功能说明：构造函数
        // 输入参数：1.iColumn：列表头中包含的列数
        //         2.rect：每列的实际区域（[0]表示最左侧的列）
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public CustomListHeader_EventArgs(int iColumn, Rectangle[] rect)
        {
            iColumnNumber = iColumn;

            rectReal = rect;
        }

        //属性

        //----------------------------------------------------------------------
        // 功能说明：ColumnNumber属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public int ColumnNumber//属性
        {
            get//读取
            {
                return iColumnNumber;
            }
            set//设置
            {
                if (iColumnNumber != value)
                {
                    iColumnNumber = value;

                    rectReal = new Rectangle[iColumnNumber];
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：RectReal属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Rectangle[] RectReal//属性
        {
            get//读取
            {
                return rectReal;
            }
            set//设置
            {
                if (null != value)
                {
                    ColumnNumber = value.Length;
                }

                //

                rectReal = value;
            }
        }
    }

    //CustomDraw类定义，用于绘制列表头背景、自定义按钮的类

    public class CustomDraw
    {
	    //使用可变长度或宽度的方法创建按钮时，创建的按钮的长度或宽度不可小于左右部或上下部填充区域之和

        private CustomDrawType drawType = CustomDrawType.Central;//属性，绘图类型

        private Rectangle rectDraw = new Rectangle();//属性，绘制区域

        private Size sizeImage = new Size();//属性，背景图像的大小

        //

        private Rectangle rectLeft = new Rectangle(0, 3, 3, 20);//属性，背景图像左部填充区域（CustomDrawType.Horizontal、CustomDrawType.Central时有效）
        private Rectangle rectTop = new Rectangle(3, 0, 237, 3);//属性，背景图像上部填充区域（CustomDrawType.Vertical、CustomDrawType.Central时有效）
        private Rectangle rectRight = new Rectangle(240, 3, 3, 20);//属性，背景图像右部填充区域（CustomDrawType.Horizontal、CustomDrawType.Central时有效）
        private Rectangle rectBottom = new Rectangle(3, 23, 237, 3);//属性，背景图像下部填充区域（CustomDrawType.Vertical、CustomDrawType.Central时有效）

        private Rectangle rectLeftTop = new Rectangle(0, 0, 3, 3);//属性，背景图像左上部矩形区域（CustomDrawType.Central时有效）
        private Rectangle rectRightTop = new Rectangle(240, 0, 3, 3);//属性，背景图像右上部矩形区域（CustomDrawType.Central时有效）
        private Rectangle rectLeftBottom = new Rectangle(0, 23, 3, 3);//属性，背景图像左下部矩形区域（CustomDrawType.Central时有效）
        private Rectangle rectRightBottom = new Rectangle(240, 23, 3, 3);//属性，背景图像右下部矩形区域（CustomDrawType.Central时有效）

        private Rectangle rectFill = new Rectangle(3, 3, 237, 20);//属性，背景图像中部填充区域（用于填充整个按钮底部背景，CustomDrawType.Vertical、CustomDrawType.Horizontal和CustomDrawType.Central时有效）

        //

        private int iFill_Number_H = 0;//填充区域的个数（资源图像中完整区域的个数，表明水平方向上整个绘制区域底部背景，上部和下部边框背景填充的个数。CustomDrawType.Horizontal、CustomDrawType.Central时有效）
        private int iFill_Number_V = 0;//填充区域的个数（资源图像中完整区域的个数，表明垂直方向上整个绘制区域底部背景，左部和右部边框背景填充的个数。CustomDrawType.Vertical、CustomDrawType.Central时有效）

        private int iFill_Size_H = 0;//余量（填充完成完整区域图像后，仍需填充的宽度。表明水平方向上整个绘制区域底部背景、上部和下部边框背景仍需要填充的宽度。CustomDrawType.Horizontal、CustomDrawType.Central时有效）
        private int iFill_Size_V = 0;//余量（填充完成完整区域图像后，仍需填充的高度。表明垂直方向上整个绘制区域底部背景，左部和右部边框背景仍需要填充的高度。CustomDrawType.Vertical、CustomDrawType.Central时有效）

        //属性

        //----------------------------------------------------------------------
        // 功能说明：DrawType属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public CustomDrawType DrawType //属性
        {
            get//读取
            {
                return drawType;
            }
            set//设置
            {
                if (drawType != value)//设置了新的数值
                {
                    drawType = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：RectDraw属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Rectangle RectDraw  //属性
        {
            get//读取
            {
                return rectDraw;
            }
            set//设置
            {
                if (rectDraw != value)//设置了新的数值
                {
                    rectDraw = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：SizeImage属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Size SizeImage   //属性
        {
            get//读取
            {
                return sizeImage;
            }
            set//设置
            {
                if (sizeImage != value)//设置了新的数值
                {
                    sizeImage = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：RectLeft属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Rectangle RectLeft  //属性
        {
            get//读取
            {
                return rectLeft;
            }
            set//设置
            {
                if (rectLeft != value)//设置了新的数值
                {
                    rectLeft = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：RectTop属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Rectangle RectTop  //属性
        {
            get//读取
            {
                return rectTop;
            }
            set//设置
            {
                if (rectTop != value)//设置了新的数值
                {
                    rectTop = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：RectRight属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Rectangle RectRight  //属性
        {
            get//读取
            {
                return rectRight;
            }
            set//设置
            {
                if (rectRight != value)//设置了新的数值
                {
                    rectRight = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：RectBottom属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Rectangle RectBottom  //属性
        {
            get//读取
            {
                return rectBottom;
            }
            set//设置
            {
                if (rectBottom != value)//设置了新的数值
                {
                    rectBottom = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：RectLeftTop属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Rectangle RectLeftTop  //属性
        {
            get//读取
            {
                return rectLeftTop;
            }
            set//设置
            {
                if (rectLeftTop != value)//设置了新的数值
                {
                    rectLeftTop = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：RectRightTop属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Rectangle RectRightTop  //属性
        {
            get//读取
            {
                return rectRightTop;
            }
            set//设置
            {
                if (rectRightTop != value)//设置了新的数值
                {
                    rectRightTop = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：RectLeftBottom属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Rectangle RectLeftBottom  //属性
        {
            get//读取
            {
                return rectLeftBottom;
            }
            set//设置
            {
                if (rectLeftBottom != value)//设置了新的数值
                {
                    rectLeftBottom = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：RectRightBottom属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Rectangle RectRightBottom  //属性
        {
            get//读取
            {
                return rectRightBottom;
            }
            set//设置
            {
                if (rectRightBottom != value)//设置了新的数值
                {
                    rectRightBottom = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：RectFill属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Rectangle RectFill  //属性
        {
            get//读取
            {
                return rectFill;
            }
            set//设置
            {
                if (rectFill != value)//设置了新的数值
                {
                    rectFill = value;
                }
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：获取绘图数据
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _GetCustomDraw()
        {
            try
            {
                if (CustomDrawType.Vertical == drawType)//垂宽度固定，高度可变
                {
                    iFill_Number_V = (int)(rectDraw.Height / rectFill.Height);
                    iFill_Size_V = rectDraw.Height % rectFill.Height;
                }
                else if (CustomDrawType.Horizontal == drawType)//高度固定，宽度可变
                {
                    iFill_Number_H = (int)(rectDraw.Width / rectFill.Width);
                    iFill_Size_H = rectDraw.Width % rectFill.Width;
                }
                else//CustomDrawType.Central == drawType，高度可变，宽度可变
                {
                    iFill_Number_V = (int)(rectDraw.Height / rectFill.Height);
                    iFill_Size_V = rectDraw.Height % rectFill.Height;

                    iFill_Number_H = (int)(rectDraw.Width / rectFill.Width);
                    iFill_Size_H = rectDraw.Width % rectFill.Width;
                }
            }
            catch (System.Exception ex)//异常
            {
            	//不执行操作
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：绘制背景图像
        // 输入参数：1.graphicsDraw：绘图
        //         2.backgroundImage：图像
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _Draw(Graphics graphicsDraw, Image backgroundImage)
        {
            if (null != backgroundImage)//有效
            {
                Int32 i = 0;//循环控制变量
                Int32 j = 0;//循环控制变量

                if (CustomDrawType.Vertical == drawType)//宽度固定，高度可变
                {
                    //绘制背景
                    for (i = 0; i < iFill_Number_V; i++)//绘制
                    {
                        //完整背景填充区域
                        graphicsDraw.DrawImage(backgroundImage, rectDraw.Left, rectDraw.Top + i * rectFill.Height, rectFill, GraphicsUnit.Pixel);
                    }

                    //仍需填充的区域（即最下部一行）
                    graphicsDraw.DrawImage(backgroundImage, rectDraw.Left, rectDraw.Top + i * rectFill.Height, new Rectangle(rectFill.Left, rectFill.Top, rectFill.Width, iFill_Size_V), GraphicsUnit.Pixel);

                    //

                    //绘制背景上部
                    graphicsDraw.DrawImage(backgroundImage, rectDraw.Left, rectDraw.Top, rectTop, GraphicsUnit.Pixel);

                    //绘制背景底部
                    graphicsDraw.DrawImage(backgroundImage, rectDraw.Left, rectDraw.Bottom - rectBottom.Height, rectBottom, GraphicsUnit.Pixel);
                }
                else if (CustomDrawType.Horizontal == drawType)//高度固定，宽度可变
                {
                    //绘制背景
                    for (i = 0; i < iFill_Number_H; i++)//绘制
                    {
                        //完整背景填充区域
                        graphicsDraw.DrawImage(backgroundImage, rectDraw.Left + i * rectFill.Width, rectDraw.Top, rectFill, GraphicsUnit.Pixel);
                    }

                    //仍需填充的区域（即最右部一行）
                    graphicsDraw.DrawImage(backgroundImage, rectDraw.Left + i * rectFill.Width, rectDraw.Top, new Rectangle(rectFill.Left, rectFill.Top, iFill_Size_H, rectFill.Height), GraphicsUnit.Pixel);

                    //

                    //绘制背景左部
                    graphicsDraw.DrawImage(backgroundImage, rectDraw.Left, rectDraw.Top, rectLeft, GraphicsUnit.Pixel);
                    //绘制背景右部
                    graphicsDraw.DrawImage(backgroundImage, rectDraw.Right - rectRight.Width, rectDraw.Top, rectRight, GraphicsUnit.Pixel);
                }
                else//CustomDrawType.Central == drawType，高度可变，宽度可变
                {
                    //绘制背景
                    for (i = 0; i < iFill_Number_H; i++)//绘制
                    {
                        for (j = 0; j < iFill_Number_V; j++)
                        {
                            //完整背景填充区域
                            graphicsDraw.DrawImage(backgroundImage, rectDraw.Left + i * rectFill.Width, rectDraw.Top + j * rectFill.Height, rectFill, GraphicsUnit.Pixel);
                        }
                        //垂直方向仍需填充的区域
                        graphicsDraw.DrawImage(backgroundImage, rectDraw.Left + i * rectFill.Width, rectDraw.Top + j * rectFill.Height, new Rectangle(rectFill.Left, rectFill.Top, rectFill.Width, iFill_Size_V), GraphicsUnit.Pixel);
                    }

                    //水平方向上仍需填充的区域（即最右侧一列）
                    for (j = 0; j < iFill_Number_V; j++)//绘制
                    {
                        graphicsDraw.DrawImage(backgroundImage, rectDraw.Left + i * rectFill.Width, rectDraw.Top + j * rectFill.Height, new Rectangle(rectFill.Left, rectFill.Top, iFill_Size_H, rectFill.Height), GraphicsUnit.Pixel);
                    }

                    //水平方向上仍需填充的区域（即最右侧一列最下部一部分）
                    graphicsDraw.DrawImage(backgroundImage, rectDraw.Left + i * rectFill.Width, rectDraw.Top + j * rectFill.Height, new Rectangle(rectFill.Left, rectFill.Top, iFill_Size_H, iFill_Size_V), GraphicsUnit.Pixel);



                    for (i = 0; i < iFill_Number_H; i++)//绘制
                    {
                        //绘制背景上部
                        graphicsDraw.DrawImage(backgroundImage, rectDraw.Left + i * rectTop.Width, rectDraw.Top, rectTop, GraphicsUnit.Pixel);

                        //绘制背景下部
                        graphicsDraw.DrawImage(backgroundImage, rectDraw.Left + i * rectBottom.Width, rectDraw.Bottom - rectBottom.Height, rectBottom, GraphicsUnit.Pixel);
                    }
                    //绘制背景上部仍需填充区域
                    graphicsDraw.DrawImage(backgroundImage, rectDraw.Left + i * rectTop.Width, rectDraw.Top, new Rectangle(rectTop.Left, rectTop.Top, iFill_Size_H, rectTop.Height), GraphicsUnit.Pixel);
                    //绘制背景下部仍需填充区域
                    graphicsDraw.DrawImage(backgroundImage, rectDraw.Left + i * rectBottom.Width, rectDraw.Bottom - rectBottom.Height, new Rectangle(rectBottom.Left, rectBottom.Top, iFill_Size_H, rectBottom.Height), GraphicsUnit.Pixel);

                    for (j = 0; j < iFill_Number_V; j++)//绘制
                    {
                        //绘制背景左部
                        graphicsDraw.DrawImage(backgroundImage, rectDraw.Left, rectDraw.Top + j * rectLeft.Height, rectLeft, GraphicsUnit.Pixel);
                        //绘制背景右部
                        graphicsDraw.DrawImage(backgroundImage, rectDraw.Right - rectRight.Width, rectDraw.Top + j * rectRight.Height, rectRight, GraphicsUnit.Pixel);
                    }
                    //绘制背景左部仍需填充区域
                    graphicsDraw.DrawImage(backgroundImage, rectDraw.Left, rectDraw.Top + j * rectLeft.Height, new Rectangle(rectLeft.Left, rectLeft.Top, rectLeft.Width, iFill_Size_V), GraphicsUnit.Pixel);
                    //绘制背景右部仍需填充区域
                    graphicsDraw.DrawImage(backgroundImage, rectDraw.Right - rectRight.Width, rectDraw.Top + j * rectRight.Height, new Rectangle(rectRight.Left, rectRight.Top, rectRight.Width, iFill_Size_V), GraphicsUnit.Pixel);

                    //左上部
                    graphicsDraw.DrawImage(backgroundImage, rectDraw.Left, rectDraw.Top, rectLeftTop, GraphicsUnit.Pixel);
                    //右上部
                    graphicsDraw.DrawImage(backgroundImage, rectDraw.Right - rectRightTop.Width, rectDraw.Top, rectRightTop, GraphicsUnit.Pixel);
                    //左下部
                    graphicsDraw.DrawImage(backgroundImage, rectDraw.Left, rectDraw.Bottom - rectLeftBottom.Height, rectLeftBottom, GraphicsUnit.Pixel);
                    //右下部
                    graphicsDraw.DrawImage(backgroundImage, rectDraw.Right - rectRightBottom.Width, rectDraw.Bottom - rectRightBottom.Height, rectRightBottom, GraphicsUnit.Pixel);
                }
            }
        }
    }

    //CustomDrawType枚举定义，CustomDraw中的绘图类型

    public enum CustomDrawType
    {
        Vertical = 1,//背景图像需要增加或减小的部分（去除上部和下部）的像素在垂直方向上对称，此时可以创建宽度固定，高度可变的按钮。按钮构建方法：原始图像上部 + （中间部分 * 个数 + 余量） + 下部
        Horizontal = 2,//背景图像需要增加或减小的部分（去除左部和右部）的像素在水平方向上对称，此时可以创建高度固定，宽度可变的按钮。按钮构建方法：原始图像左部 + （中间部分 * 个数 + 余量） + 右部
        Central = 3,//背景图像需要增加或减小的部分（去除四个角部的矩形框、四边的边框部分）的像素在水平和垂直方向上对称，此时可以创建高度可变，宽度可变的按钮。按钮构建方法：原始图像四个角部矩形 + （四个边框部分 * 个数 + 余量） + （中间部分 * 个数 + 余量）
    }
}