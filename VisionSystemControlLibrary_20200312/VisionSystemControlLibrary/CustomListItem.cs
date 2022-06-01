/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：CustomListItem.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：自定义列表中的列表项控件

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
    public partial class CustomListItem : UserControl
    {
        //该控件为自定义的列表项控件
        //设置ColumnNumber属性会重新申请ColumnWidth、ColumnName和RectReal的内存空间，因此ColumnNumber属性应该优先设置
        //修改属性Selected、调用_ClearAll()函数或_Set()函数时会刷新控件

        private CustomListItemData customListItemData = new CustomListItemData(6);//属性，列表项数据

        //

        private Size sizeListItem = new Size();//属性，控件大小

        //

        private Bitmap[] bitmapIcon = new Bitmap[6];//属性，每列的图标（[0]表示最左侧的列）

        //

        private Font fontText = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point, Convert.ToByte(134));//属性，绘制文本所使用的字体

        private SolidBrush solidbrushText_Selected = new SolidBrush(Color.FromArgb(0, 255, 255));//控件选中时，绘制文本所使用的画刷
        private SolidBrush solidbrushText_Unselected = new SolidBrush(Color.FromArgb(255, 255, 255));//控件未选中时，绘制文本所使用的画刷
        private SolidBrush solidbrushText_UnEnable = new SolidBrush(Color.FromArgb(127, 127, 127));//属性，控件未使能时，绘制文本所使用的颜色
        private SolidBrush solidbrushBackground_Selected = new SolidBrush(Color.FromArgb(255, 0, 0));//列表框项选中时，绘制背景所使用的画刷
        private SolidBrush solidbrushBackground_Unselected = new SolidBrush(Color.FromArgb(0, 0, 0));//列表框项未选中时，绘制背景所使用的画刷

        private Color colorText_Selected = Color.FromArgb(0, 255, 255);//属性，控件选中时，绘制文本所使用的颜色
        private Color colorText_Unselected = Color.FromArgb(255, 255, 255);//属性，控件未选中时，绘制文本所使用的颜色
        private Color colorText_UnEnable = Color.FromArgb(127, 127, 127);//属性，控件未使能时，绘制文本所使用的颜色
        private Color colorBackground_Selected = Color.FromArgb(255, 0, 0);//属性，列表框项选中时，绘制背景所使用的颜色
        private Color colorBackground_Unselected = Color.FromArgb(0, 0, 0);//属性，列表框项未选中时，绘制背景所使用的颜色

        private StringFormat stringformatText = new StringFormat();//属性（水平对齐方式），绘制文本所使用的格式

        //

        private Int32 iXOffSetValue = 7;//属性，列表项控件绘制文本的偏移量（绘制区域左侧 + 该数值）

        //

        private bool bSelectedItemType = false;//属性，列表框项选中时的样式。取值范围：
                                              //true，选中时改变背景颜色，此时使用下列变量绘制控件
                                              //solidbrushBackground_Selected，背景
                                              //solidbrushBackground_Unselected，背景
                                              //solidbrushText_Unselected，文本
                                              //false，选中时改变字体颜色，此时使用下列变量绘制控件
                                              //solidbrushText_Selected，文本
                                              //solidbrushText_Unselected，文本
                                              //solidbrushBackground_Unselected，背景

        //

        private Image imageToDraw = null;//控件图像

        private Image imageControl = null;//属性（只读），控件图像

        //

        private Graphics graphicsDisplay;//绘图

        //

        [Browsable(true), Description("点击控件时产生的事件"), Category("CustomListItem 事件")]
        public event EventHandler ClickListItem;//点击控件时产生的事件

        [Browsable(true), Description("双击控件时产生的事件"), Category("CustomListItem 事件")]
        public event EventHandler DoubleClickListItem;//双击控件时产生的事件

        //

        //----------------------------------------------------------------------
        // 功能说明：构造函数，初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public CustomListItem()
        {
            InitializeComponent();

            //

            stringformatText.Alignment = StringAlignment.Near;//设置格式
            stringformatText.LineAlignment = StringAlignment.Center;//设置格式
            stringformatText.Trimming = StringTrimming.EllipsisCharacter;//设置格式
            stringformatText.FormatFlags = StringFormatFlags.NoWrap;//设置格式

            graphicsDisplay = CreateGraphics();//获取绘图资源
        }

        //属性

        //-----------------------------------------------------------------------
        // 功能说明：SizeListItem属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("控件大小"), Category("CustomListItem 通用")]
        public Size SizeListItem
        {
            get//读取
            {
                return sizeListItem;
            }
            set//设置
            {
                if (value != sizeListItem)//设置了新的数值
                {
                    sizeListItem = value;

                    this.Size = value;

                    //

                    graphicsDisplay.Dispose();//释放

                    graphicsDisplay = CreateGraphics();//获取绘图资源

                    //
                    
                    //_GetColumnValue();//获取列初始数值

                    //

                    _CheckItem();//检查列表项是否有效

                    //

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
        [Browsable(false), Description("控件图像"), Category("CustomListItem 通用")]
        public Image ImageControl
        {
            get//读取
            {
                return imageControl;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ListItemEnabled属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("列表项使能状态。取值范围：true，使能；false：禁止"), Category("CustomListItem 通用")]
        public bool ListItemEnabled//属性
        {
            get//读取
            {
                return customListItemData.Enabled;
            }
            set//设置
            {
                if (value != customListItemData.Enabled)//设置了新的数值
                {
                    customListItemData.Enabled = value;

                    //

                    solidbrushText_UnEnable = new SolidBrush(colorText_UnEnable);//控件选中时，绘制文本所使用的画刷

                    //

                    _UpdateBackground();//更新背景图像
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：Selected属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("控件是否被选中。取值范围：true，是；false，否"), Category("CustomListItem 通用")]
        public bool Selected//属性
        {
            get//读取
            {
                return customListItemData.Selected;
            }
            set//设置
            {
                if (value != customListItemData.Selected)//设置了新的数值
                {
                    customListItemData.Selected = value;
                }

                //

                _UpdateBackground();//更新背景图像
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ColumnNumber属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("列表头中包含的列数"), Category("CustomListItem 通用")]
        public int ColumnNumber//属性
        {
            get//读取
            {
                return customListItemData.ColumnNumber;
            }
            set//设置
            {
                if (value != customListItemData.ColumnNumber)//设置了新的数值
                {
                    customListItemData = new CustomListItemData(value);//属性，列表项数据

                    if (0 < value)//有效
                    {
                        Int32 i = 0;//循环控制变量

                        //

                        if (null != bitmapIcon)
                        {
                            for (i = 0; i < bitmapIcon.Length; i++)
                            {
                                if (null != bitmapIcon[i])
                                {
                                    bitmapIcon[i].Dispose();
                                }
                            }
                        }

                        //

                        bitmapIcon = new Bitmap[value];//属性，每列的图标（[0]表示最左侧的列）

                        //

                        for (i = 0; i < customListItemData.ColumnNumber; i++)//创建
                        {
                            bitmapIcon[i] = null;//属性，每列的图标（[0]表示最左侧的列）
                        }

                        //

                        _GetColumnValue();//获取列初始数值

                        //

                        _CheckItem();//检查列表项是否有效

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
        [Browsable(true), Description("每列的宽度（初始显示的每列的宽度，[0]表示最左侧的列）"), Category("CustomListItem 通用")]
        public Int32[] ColumnWidth//属性
        {
            get//读取
            {
                return customListItemData.ColumnWidth;
            }
            set//设置
            {
                if (null != value)//有效
                {
                    //ColumnNumber = value.Length;

                    //

                    value.CopyTo(customListItemData.ColumnWidth, 0);
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：Valid属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("列表项是否有效（即是否为空的列表项）。取值范围：true，是；false，否"), Category("CustomListItem 通用")]
        public Boolean Valid//属性
        {
            get//读取
            {
                return customListItemData.Valid;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ItemFlag属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("特征数据"), Category("CustomListItem 通用")]
        public Int32 ItemFlag//属性
        {
            get//读取
            {
                return customListItemData.ItemFlag;
            }
            set//设置
            {
                if (value != customListItemData.ItemFlag)//设置了新的数值
                {
                    customListItemData.ItemFlag = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：SelectionColumnIndex属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("列表项选择图标索引（取值范围：>= 0）"), Category("CustomListItem 通用")]
        public Int32 SelectionColumnIndex//属性
        {
            get//读取
            {
                return customListItemData.SelectionColumnIndex;
            }
            set//设置
            {
                if (value != customListItemData.SelectionColumnIndex)//设置了新的数值
                {
                    customListItemData.SelectionColumnIndex = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：SelectedItemType属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("列表框项选中时的样式。取值范围：true，选中时改变背景颜色；false，选中时改变字体颜色"), Category("CustomListItem 通用")]
        public Boolean SelectedItemType//属性
        {
            get//读取
            {
                return bSelectedItemType;
            }
            set//设置
            {
                if (value != bSelectedItemType)//设置了新的数值
                {
                    bSelectedItemType = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ColorControlBackground属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("控件背景颜色"), Category("CustomListItem 通用")]
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
        // 功能说明：ItemData属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("列表项数据"), Category("CustomListItem 文本")]
        public CustomListItemData ItemData//属性
        {
            get//读取
            {
                return customListItemData;
            }
            set//设置
            {
                customListItemData = value;

                //

                _CheckItem();//检查列表项是否有效

                //

                _UpdateBackground();//更新背景图像
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ColumnName属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("每列的名称（[0]表示最左侧的列）"), Category("CustomListItem 文本")]
        public string[] ItemText//属性
        {
            get//读取
            {
                return customListItemData.ItemText;
            }
            set//设置
            {
                if (null != value)//有效
                {
                    //ColumnNumber = value.Length;

                    //

                    value.CopyTo(customListItemData.ItemText, 0);

                    //

                    _CheckItem();//检查列表项是否有效
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：FontText属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("绘制文本所使用的字体"), Category("CustomListItem 文本")]
        public Font FontText//属性
        {
            get//读取
            {
                return fontText;
            }
            set//设置
            {
                if (value != fontText)//设置了新的数值
                {
                    fontText = value;

                    //

                    _UpdateBackground();//更新背景图像
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ColorTextSelected属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("列表框项选中时，绘制文本所使用的颜色"), Category("CustomListItem 文本")]
        public Color ColorTextSelected//属性
        {
            get//读取
            {
                return colorText_Selected;
            }
            set//设置
            {
                if (value != colorText_Selected)//设置了新的数值
                {
                    colorText_Selected = value;

                    //

                    solidbrushText_Selected = new SolidBrush(colorText_Selected);//控件选中时，绘制文本所使用的画刷

                    //

                    _UpdateBackground();//更新背景图像
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ColorTextUnselected属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("列表框项未选中时，绘制文本所使用的颜色"), Category("CustomListItem 文本")]
        public Color ColorTextUnselected//属性
        {
            get//读取
            {
                return colorText_Unselected;
            }
            set//设置
            {
                if (value != colorText_Unselected)//设置了新的数值
                {
                    colorText_Unselected = value;

                    //

                    solidbrushText_Unselected = new SolidBrush(colorText_Unselected);//控件未选中时，绘制文本所使用的画刷

                    //

                    _UpdateBackground();//更新背景图像
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：TextAlignment属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("绘制文本的水平对齐方式"), Category("CustomListItem 文本")]
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

                    _UpdateBackground();//更新背景图像
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ItemDataDisplay属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("是否显示列表项数据（指的是是否显示sItemText中的文本数据）。取值范围：true，是；false，否"), Category("CustomListItem 文本")]
        public Boolean[] ItemDataDisplay//属性
        {
            get//读取
            {
                return customListItemData.ItemDataDisplay;
            }
            set//设置
            {
                if (null != value)//有效
                {
                    //ColumnNumber = value.Length;

                    //

                    customListItemData.ItemDataDisplay = value;

                    //

                    _CheckItem();//检查列表项是否有效

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
        [Browsable(true), Description("列表项控件绘制文本的偏移量（绘制区域左侧 + 该数值）"), Category("CustomListItem 文本")]
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

        //----------------------------------------------------------------------
        // 功能说明：BitmapIcon属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("每列的图标（[0]表示最左侧的列）"), Category("CustomListItem 图标")]
        public Bitmap[] BitmapIcon//属性
        {
            get//读取
            {
                return bitmapIcon;
            }
            set//设置
            {
                //
                if (null != value)//有效
                {
                    //ColumnNumber = value.Length;

                    //

                    Int32 i = 0;//循环控制变量

                    if (null != bitmapIcon)
                    {
                        for (i = 0; i < bitmapIcon.Length; i++)
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

                    _CheckItem();//检查列表项是否有效
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ItemIconIndex属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("列表框项图标所在列的索引值"), Category("CustomListItem 图标")]
        public Int32[] ItemIconIndex//属性
        {
            get//读取
            {
                return customListItemData.ItemIconIndex;
            }
            set//设置
            {
                if (null != value)//有效
                {
                    //ColumnNumber = value.Length;

                    //

                    value.CopyTo(customListItemData.ItemIconIndex, 0);

                    //

                    _CheckItem();//检查列表项是否有效
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：SizeItemIcon属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("列表框项图标大小（像素）"), Category("CustomListItem 图标")]
        public Size[] SizeItemIcon//属性
        {
            get//读取
            {
                return customListItemData.SizeItemIcon;
            }
            set//设置
            {
                if (null != value)//有效
                {
                    //ColumnNumber = value.Length;

                    //

                    value.CopyTo(customListItemData.SizeItemIcon, 0);
                }
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：ColorBackgroundSelected属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("列表框项选中时，绘制背景所使用的颜色"), Category("CustomListItem 背景")]
        public Color ColorBackgroundSelected//属性
        {
            get//读取
            {
                return colorBackground_Selected;
            }
            set//设置
            {
                if (value != colorBackground_Selected)//设置了新的数值
                {
                    colorBackground_Selected = value;

                    //

                    solidbrushBackground_Selected = new SolidBrush(colorBackground_Selected);//列表框项选中时，绘制背景所使用的画刷

                    //

                    _UpdateBackground();//更新背景图像
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ColorBackgroundUnselected属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("列表框项未选中时，绘制背景所使用的颜色"), Category("CustomListItem 背景")]
        public Color ColorBackgroundUnselected//属性
        {
            get//读取
            {
                return colorBackground_Unselected;
            }
            set//设置
            {
                if (value != colorBackground_Unselected)//设置了新的数值
                {
                    colorBackground_Unselected = value;

                    //

                    solidbrushBackground_Unselected = new SolidBrush(colorBackground_Unselected);//列表框项未选中时，绘制背景所使用的画刷

                    //

                    _UpdateBackground();//更新背景图像
                }
            }
        }

        //函数

        //----------------------------------------------------------------------
        // 功能说明：设置默认值
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetDefault()
        {
            Size = new Size(835, 25);//更改控件大小

            //

            //第1列
            customListItemData.ColumnWidth[0] = 134;//属性，每列的宽度（初始显示的每列的宽度，[0]表示最左侧的列）
            customListItemData.ItemText[0] = "";//属性，每列的名称（[0]表示最左侧的列）
            bitmapIcon[0] = null;//属性，每列的图标（[0]表示最左侧的列）
            customListItemData.RectReal[0] = new Rectangle(ClientRectangle.Left, ClientRectangle.Top, customListItemData.ColumnWidth[0], ClientRectangle.Height);//属性，每列的实际区域（[0]表示最左侧的列）

            //第2列
            customListItemData.ColumnWidth[1] = 203;//属性，每列的宽度（初始显示的每列的宽度，[0]表示最左侧的列）
            customListItemData.ItemText[1] = "";//属性，每列的名称（[0]表示最左侧的列）
            bitmapIcon[1] = null;//属性，每列的图标（[0]表示最左侧的列）
            customListItemData.RectReal[1] = new Rectangle(customListItemData.RectReal[0].Right + 1, ClientRectangle.Top, customListItemData.ColumnWidth[1] - 1, ClientRectangle.Height);//属性，每列的实际区域（[0]表示最左侧的列）

            //第3列
            customListItemData.ColumnWidth[2] = 127;//属性，每列的宽度（初始显示的每列的宽度，[0]表示最左侧的列）
            customListItemData.ItemText[2] = "";//属性，每列的名称（[0]表示最左侧的列）
            bitmapIcon[2] = null;//属性，每列的图标（[0]表示最左侧的列）
            customListItemData.RectReal[2] = new Rectangle(customListItemData.RectReal[1].Right + 1, ClientRectangle.Top, customListItemData.ColumnWidth[2] - 1, ClientRectangle.Height);//属性，每列的实际区域（[0]表示最左侧的列）

            //第4列
            customListItemData.ColumnWidth[3] = 65;//属性，每列的宽度（初始显示的每列的宽度，[0]表示最左侧的列）
            customListItemData.ItemText[3] = "";//属性，每列的名称（[0]表示最左侧的列）
            bitmapIcon[3] = null;//属性，每列的图标（[0]表示最左侧的列）
            customListItemData.RectReal[3] = new Rectangle(customListItemData.RectReal[2].Right + 1, ClientRectangle.Top, customListItemData.ColumnWidth[3] - 1, ClientRectangle.Height);//属性，每列的实际区域（[0]表示最左侧的列）

            //第5列
            customListItemData.ColumnWidth[4] = 159;//属性，每列的宽度（初始显示的每列的宽度，[0]表示最左侧的列）
            customListItemData.ItemText[4] = "";//属性，每列的名称（[0]表示最左侧的列）
            bitmapIcon[4] = null;//属性，每列的图标（[0]表示最左侧的列）
            customListItemData.RectReal[4] = new Rectangle(customListItemData.RectReal[3].Right + 1, ClientRectangle.Top, customListItemData.ColumnWidth[4] - 1, ClientRectangle.Height);//属性，每列的实际区域（[0]表示最左侧的列）

            //第6列
            customListItemData.ColumnWidth[5] = 147;//属性，每列的宽度（初始显示的每列的宽度，[0]表示最左侧的列）
            customListItemData.ItemText[5] = "";//属性，每列的名称（[0]表示最左侧的列）
            bitmapIcon[5] = null;//属性，每列的图标（[0]表示最左侧的列）
            customListItemData.RectReal[5] = new Rectangle(customListItemData.RectReal[4].Right + 1, ClientRectangle.Top, customListItemData.ColumnWidth[5] - 1, ClientRectangle.Height);//属性，每列的实际区域（[0]表示最左侧的列）

            //

            _CheckItem();//检查列表项是否有效

            //

            stringformatText.Alignment = StringAlignment.Near;//设置格式
            stringformatText.LineAlignment = StringAlignment.Center;//设置格式
            stringformatText.Trimming = StringTrimming.EllipsisCharacter;//设置格式
            stringformatText.FormatFlags = StringFormatFlags.NoWrap;//设置格式

            graphicsDisplay = CreateGraphics();//获取绘图资源
        }

        //----------------------------------------------------------------------
        // 功能说明：应用时调用，应用设置完成的属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _Apply()
        {
            int i = 0;//循环控制变量

            for (i = 0; i < customListItemData.ColumnNumber; i++)//赋值
            {
                if (0 == i)//第1列
                {
                    customListItemData.RectReal[0] = new Rectangle(ClientRectangle.Left, ClientRectangle.Top, customListItemData.ColumnWidth[0], ClientRectangle.Height);//属性，每列的实际区域（[0]表示最左侧的列）
                }
                else//其它列
                {
                    customListItemData.RectReal[i] = new Rectangle(customListItemData.RectReal[i - 1].Right + 1, ClientRectangle.Top, customListItemData.ColumnWidth[i] - 1, ClientRectangle.Height);//属性，每列的实际区域（[0]表示最左侧的列）
                }
            }

            //

            _UpdateBackground();//更新背景图像
        }

        //----------------------------------------------------------------------
        // 功能说明：更改列数、控件大小时调用本函数，获取列初始数值
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _GetColumnValue()
        {
            int i = 0;//循环控制变量

            int iColumnWidth_Temp = Size.Width / customListItemData.ColumnNumber;//每列的宽度
            int iColumnWidthExtra_Temp = Size.Width % customListItemData.ColumnNumber;//每列的宽度余量（针对不能整除的情况，将其加至最后一列中）

            for (i = 0; i < customListItemData.ColumnNumber - 1; i++)//赋值
            {
                customListItemData.ColumnWidth[i] = iColumnWidth_Temp;//属性，每列的宽度（初始显示的每列的宽度，[0]表示最左侧的列）
            }
            customListItemData.ColumnWidth[i] = iColumnWidth_Temp + iColumnWidthExtra_Temp;//属性，每列的宽度（初始显示的每列的宽度，[0]表示最左侧的列）
        }

        //----------------------------------------------------------------------
        // 功能说明：刷新
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _Refresh()
        {
            _UpdateBackground();//更新背景图像
        }

        //----------------------------------------------------------------------
        // 功能说明：清除控件显示的所有文本和图标
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _ClearAll()
        {
            customListItemData.Valid = false;//无效

            //

            _UpdateBackground();//更新背景图像
        }

        //----------------------------------------------------------------------
        // 功能说明：设置每列的实际区域（[0]表示最左侧的列）
        // 输入参数：1.rect：每列的区域
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _Set(Rectangle[] rect)
        {
            for (int i = 0; i < rect.Length; i++)//赋值
            {
                customListItemData.RectReal[i].Location = rect[i].Location;
                customListItemData.RectReal[i].Size = rect[i].Size;
            }

            //

            _UpdateBackground();//更新背景图像
        }

        //----------------------------------------------------------------------
        // 功能说明：检查列表项是否有效
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _CheckItem()
        {
            Int32 i = 0;//循环控制变量

            for (i = 0; i < customListItemData.ColumnNumber; i++)
            {
                if (customListItemData.ItemDataDisplay[i])//显示列表项文本
                {
                    if ("" != customListItemData.ItemText[i])//有效
                    {
                        break;
                    }
                }
                else//不显示列表项文本
                {
                    if (0 <= customListItemData.ItemIconIndex[i] && null != bitmapIcon[i])//有效
                    {
                        break;
                    }
                }
            }

            if (i < customListItemData.ColumnNumber)//列表项有效
            {
                customListItemData.Valid = true;
            }
            else//列表项无效
            {
                customListItemData.Valid = false;
            }
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

        //----------------------------------------------------------------------
        // 功能说明：更新背景图像
        // 输入参数：1.bDraw：是否绘制控件。取值范围：true，是（用于控件内部调用）；false，否（用于控件外部获取控件背景图像时使用）
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _UpdateBackground(Boolean bDraw = true)
        {
            _GetBackgroundImage(bDraw);

            if (bDraw)
            {
                _Draw();

                //

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
        private void _GetBackgroundImage(Boolean bDraw = true)
        {
            //使用双倍缓冲绘图

            Image imageDisplay = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);
            Graphics graphicsDraw = Graphics.FromImage(imageDisplay);

            SolidBrush solidbrushDraw = new SolidBrush(BackColor);

            //绘制背景色

            graphicsDraw.FillRectangle(solidbrushDraw, ClientRectangle);//绘制当前选择的相机控件区域

            //绘制背景图像、文本

            if (customListItemData.Valid)//列表框项有效
            {
                Int32 i = 0;//循环控制变量

                SolidBrush solidbrushText = new SolidBrush(Color.FromArgb(255, 255, 255));//绘制文本的画刷
                SolidBrush solidbrushBackground = new SolidBrush(Color.FromArgb(0, 0, 0));//绘制背景的画刷

                if (customListItemData.Enabled) //控件使能 
                {
                    if (customListItemData.Selected)//控件处于选中状态
                    {
                        if (bSelectedItemType)//选中时改变背景颜色
                        {
                            solidbrushText = solidbrushText_Unselected;
                            solidbrushBackground = solidbrushBackground_Selected;
                        }
                        else//选中时改变字体颜色
                        {
                            solidbrushText = solidbrushText_Selected;
                            solidbrushBackground = solidbrushBackground_Unselected;
                        }
                    }
                    else//控件未选中
                    {
                        solidbrushText = solidbrushText_Unselected;
                        solidbrushBackground = solidbrushBackground_Unselected;
                    }
                }
                else //控件未使能
                {
                    solidbrushText = solidbrushText_UnEnable;
                    solidbrushBackground = solidbrushBackground_Unselected;
                }

                //绘制背景

                graphicsDraw.FillRectangle(solidbrushBackground, ClientRectangle);//绘制背景

                //绘制文本、图标

                for (i = 0; i < customListItemData.ColumnNumber; i++)//绘制文本
                {
                    if (customListItemData.ItemDataDisplay[i])//显示文本
                    {
                        graphicsDraw.DrawString(customListItemData.ItemText[i], fontText, solidbrushText, new Rectangle(customListItemData.RectReal[i].Left + iXOffSetValue, customListItemData.RectReal[i].Top, customListItemData.RectReal[i].Width, customListItemData.RectReal[i].Height), stringformatText);//绘制文本
                    }
                    else//不显示文本
                    {
                        if (null != bitmapIcon[i])//图标有效
                        {
                            graphicsDraw.DrawImage(bitmapIcon[i], new Rectangle(customListItemData.RectReal[i].Left + iXOffSetValue, (customListItemData.RectReal[i].Top + customListItemData.RectReal[i].Bottom - customListItemData.SizeItemIcon[i].Height) / 2, customListItemData.SizeItemIcon[i].Width, customListItemData.SizeItemIcon[i].Height));//绘制图标
                        }
                    }
                }
            }
            else//列表框项无效
            {
                //绘制背景

                graphicsDraw.FillRectangle(solidbrushBackground_Unselected, ClientRectangle);//绘制背景
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
        // 功能说明：绘制文本
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

        //事件

        //----------------------------------------------------------------------
        // 功能说明：绘制控件事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void CustomListItem_Paint(object sender, PaintEventArgs e)
        {
            _UpdateBackground();
        }

        //----------------------------------------------------------------------
        // 功能说明：点击控件时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void CustomListItem_Click(object sender, EventArgs e)
        {
            if (customListItemData.Enabled && customListItemData.Valid)//使能
            {
                //事件

                if (null != ClickListItem)//有效
                {
                    ClickListItem(this, e);
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：双击控件时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void CustomListItem_DoubleClick(object sender, EventArgs e)
        {
            if (customListItemData.Enabled && customListItemData.Valid)//使能
            {
                //事件

                if (null != DoubleClickListItem)//有效
                {
                    DoubleClickListItem(this, e);
                }
            }
        }
    }

    //CustomListItemData类，列表项数据

    public class CustomListItemData
    {
        //列表中的某一列中只可能显示文本或图标之一

        private string[] sItemText = new string[6];//属性，列表项数据
        private Int32[] iItemIconIndex = new Int32[6];//属性，列表项图标索引（取值范围：>= 0）
        private bool[] bItemDataDisplay = new bool[6];//属性，是否显示列表项数据（指的是是否显示sItemText中的文本数据。显示Selected图标的列，其文本必须设置为""）。取值范围：true，是；false，否

        private Int32[] iColumnWidth = new int[6];//属性，每列的宽度（初始显示的每列的宽度，[0]表示最左侧的列）
        private Rectangle[] rectReal = new Rectangle[6];//属性，每列的实际区域（[0]表示最左侧的列）
        private Size[] sizeItemIcon = new Size[6];//属性，列表框项图标大小（像素）（绘制图标时，以该变量表示的大小为准，而不是图标本身的大小）

        private Int32 iSelectionColumnIndex = -1;//属性，列表项选择图标索引（取值范围：>= 0）

        private Int32 iItemFlag = -1;//属性，列表项特征数据

        private Int32 iColumnNumber = 6;//属性（只读），列表头中包含的列数

        private Boolean bEnabled = true;//属性，列表项使能状态。取值范围：true，使能；false：禁止
        private Boolean bSelected = false;//属性，控件是否被选中。取值范围：true，是；false，否
        private Boolean bValid = false;//属性，列表项是否有效（即是否为空的列表项）。取值范围：true，是；false，否

        //----------------------------------------------------------------------
        // 功能说明：构造函数
        // 输入参数：1.deviceItem：设备项
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public CustomListItemData(Int32 icolumnnumber)
        {
            iColumnNumber = icolumnnumber;//属性（只读），列表头中包含的列数

            iSelectionColumnIndex = -1;//属性，列表项选择图标索引（取值范围：>= 0）

            iItemFlag = -1;//属性，列表项特征数据

            //

            if (0 < icolumnnumber)//有效
            {
                sItemText = new string[icolumnnumber];//属性，列表项数据
                iItemIconIndex = new Int32[icolumnnumber];//属性，列表项图标索引（取值范围：>= 0）
                bItemDataDisplay = new bool[icolumnnumber];//属性，是否显示列表项数据（指的是是否显示sItemText中的文本数据）。取值范围：true，是；false，否
                iColumnWidth = new Int32[icolumnnumber];//属性，每列的宽度（初始显示的每列的宽度，[0]表示最左侧的列）
                rectReal = new Rectangle[icolumnnumber];//属性，每列的实际区域（[0]表示最左侧的列）
                sizeItemIcon = new Size[icolumnnumber];//属性，列表框项图标大小（像素）（绘制图标时，以该变量表示的大小为准，而不是图标本身的大小）

                //

                for (Int32 i = 0; i < iColumnNumber; i++)//初始值
                {
                    sItemText[i] = "";//属性，列表项数据
                    iItemIconIndex[i] = -1;//属性，列表项图标索引（取值范围：>= 0）
                    bItemDataDisplay[i] = true;//属性，是否显示列表项数据（指的是是否显示sItemText中的文本数据）。取值范围：true，是；false，否
                    iColumnWidth[i] = 0;//属性，每列的宽度（初始显示的每列的宽度，[0]表示最左侧的列）
                    rectReal[i] = new Rectangle(0, 0, 0, 0);//属性，每列的实际区域（[0]表示最左侧的列）
                    sizeItemIcon[i] = new Size(20, 20);//属性，列表框项图标大小（像素）（绘制图标时，以该变量表示的大小为准，而不是图标本身的大小）
                }
            }
        }

        //属性

        //----------------------------------------------------------------------
        // 功能说明：ItemText属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public string[] ItemText//属性
        {
            get//读取
            {
                return sItemText;
            }
            set//设置
            {
                sItemText = value;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ItemIconIndex属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Int32[] ItemIconIndex//属性
        {
            get//读取
            {
                return iItemIconIndex;
            }
            set//设置
            {
                iItemIconIndex = value;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ItemDataDisplay属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public bool[] ItemDataDisplay//属性
        {
            get//读取
            {
                return bItemDataDisplay;
            }
            set//设置
            {
                bItemDataDisplay = value;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ColumnWidth属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Int32[] ColumnWidth//属性
        {
            get//读取
            {
                return iColumnWidth;
            }
            set//设置
            {
                iColumnWidth = value;
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
                rectReal = value;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：SizeItemIcon属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Size[] SizeItemIcon//属性
        {
            get//读取
            {
                return sizeItemIcon;
            }
            set//设置
            {
                sizeItemIcon = value;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ItemFlag属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Int32 ItemFlag//属性
        {
            get//读取
            {
                return iItemFlag;
            }
            set//设置
            {
                if (iItemFlag != value)//设置了新的数值
                {
                    iItemFlag = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：SelectionColumnIndex属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Int32 SelectionColumnIndex//属性
        {
            get//读取
            {
                return iSelectionColumnIndex;
            }
            set//设置
            {
                if (iSelectionColumnIndex != value)//设置了新的数值
                {
                    iSelectionColumnIndex = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ColumnNumber属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Int32 ColumnNumber//属性
        {
            get//读取
            {
                return iColumnNumber;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：Enabled属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Boolean Enabled//属性
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
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：Selected属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Boolean Selected//属性
        {
            get//读取
            {
                return bSelected;
            }
            set//设置
            {
                if (bSelected != value)//设置了新的数值
                {
                    bSelected = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：Valid属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Boolean Valid//属性
        {
            get//读取
            {
                return bValid;
            }
            set//设置
            {
                if (bValid != value)//设置了新的数值
                {
                    bValid = value;
                }
            }
        }
    }
}