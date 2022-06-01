/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：CustomList.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：自定义列表控件

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
    public partial class CustomList : UserControl
    {
        //该控件为自定义的列表控件（包含列表头控件，17个列表项控件）

        //设置ItemDataNumber属性会重新申请Item_Data的内存空间，因此ItemDataNumber属性应该优先设置
        //设置ColumnNumber属性会重新申请列表头和列表项相关变量的内存空间，因此ColumnNumber属性应该优先设置
        //（ItemDataNumber与ColumnNumber属性设置时无先后关系）

        private VisionSystemClassLibrary.Enum.InterfaceLanguage language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;//属性，语言

        private Size sizeControl = new Size();//属性，控件大小

        private bool bEnabled = true;//属性，列表使能状态。取值范围：true，使能；false：禁止

        private int iItemDataNumber = 0;//属性，有效的列表项数目（0表示无有效的列表项）

        private CustomListItemData[] Item_Data = new CustomListItemData[256];//属性，列表项数据

        private Int32 iBitmapIconNumber = 6;//属性，图标
        private Bitmap[] bitmapIcon = new Bitmap[6];//属性，图标

        private int iTotalPage = 0;//属性（只读），包含的页码总数（取值为0，表示无有效的列表项）
        private const int iItemControlMaxNumber = 17;//属性（只读），列表项控件的最大数目
        private int iItemControlNumber = 17;//属性（只读），列表项控件的数目（最大值为17）

        private int iCurrentPage = 0;//属性（只读），当前页码（从0开始。取值为-1，表示无有效的列表项）

        private int iCurrentDataIndex = -1;//属性（只读），当前选择的项对应的外部数据数组的序号（0 ~ 外部数据数组长度 - 1。取值为-1，表示当前未选择任何项）
        private int iCurrentListIndex = -1;//属性（只读），当前选择的项在列表中的序号（0 ~ iItemDataNumber - 1。取值为-1，表示当前未选择任何项）

        private int iIndex_Page = -1;//属性（只读），当前选择的项在当前页中的索引（0 ~ iItemControlNumber - 1。取值为-1，表示当前未选择任何项）

        private int iStartIndex = 0;//属性（只读），当前页中的起始索引（0 ~ iItemDataNumber - 1）
        private int iEndIndex = 0;//属性（只读），当前页中的结束索引（0 ~ iItemDataNumber - 1）

        private Int32 iSelectedItemNumber = 0;//属性，列表中选择的项的数目

        //

        private Int32 iWidth_ControlWidth_ListHeaderWidth = 0;//控件宽度与列表头宽度之差

        private Int32 iHeight_ControlTop_ListHeaderTop = 0;//控件顶部与列表头顶部之间的距离
        private Int32 iHeight_ListHeaderBottom_FirstListItem = 0;//列表头底部与第一个列表项顶部之间的距离
        private Int32 iHeight_LastListItem_PageTop = 0;//最后一个列表项底部与页码控件顶部之间的距离
        private Int32 iHeight_PageBottom_ControlBottom = 0;//页码控件底部与控件底部之间的距离

        //

        private String[][] sMessageText = new String[2][];//页码控件上显示的文本（[语言][包含的文本]）

        //

        [Browsable(true), Description("点击列表项控件时产生的事件"), Category("CustomList 事件")]
        public event EventHandler CustomListItem_Click;//点击列表项控件时产生的事件

        [Browsable(true), Description("双击列表项控件时产生的事件"), Category("CustomList 事件")]
        public event EventHandler CustomListItem_DoubleClick;//双击列表项控件时产生的事件

        //

        //----------------------------------------------------------------------
        // 功能说明：构造函数，初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public CustomList()
        {
            InitializeComponent();

            //

            iWidth_ControlWidth_ListHeaderWidth = this.Size.Width - customListHeader.Size.Width;//控件宽度与列表头宽度之差

            iHeight_ControlTop_ListHeaderTop = customListHeader.Top - ClientRectangle.Top;//控件顶部与列表头顶部之间的距离
            iHeight_ListHeaderBottom_FirstListItem = customListItem1.Top - customListHeader.Bottom;//列表头底部与第一个列表项顶部之间的距离
            iHeight_LastListItem_PageTop = customButtonPage.Top - customListItem17.Bottom;//最后一个列表项底部与页码控件顶部之间的距离
            iHeight_PageBottom_ControlBottom = ClientRectangle.Bottom - customButtonPage.Bottom;//页码控件底部与控件底部之间的距离

            //

            Int32 i = 0;//循环控制变量

            FieldInfo[] fieldinfo = VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese.GetType().GetFields();//获取公共字段
            if (null != fieldinfo && 1 < fieldinfo.Length)//有效
            {
                sMessageText = new String[fieldinfo.Length - 1][];
                for (i = 0; i < fieldinfo.Length - 1; i++)
                {
                    sMessageText[i] = new String[1];
                }

                //

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] = customButtonPage.Chinese_TextDisplay[0];
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] = customButtonPage.English_TextDisplay[0];
            }

            //

            for (i = 0; i < iItemDataNumber; i++)//创建对象
            {
                Item_Data[i] = new CustomListItemData(customListHeader.ColumnNumber);
            }

            for (i = 0; i < iBitmapIconNumber; i++)//初始化
            {
                bitmapIcon[i] = null;//属性，图标
            }

            sizeControl = this.Size;//控件原始大小
        }

        //属性

        //----------------------------------------------------------------------
        // 功能说明：Language属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("语言"), Category("CustomList 通用")]
        public VisionSystemClassLibrary.Enum.InterfaceLanguage Language//属性
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
                }

                //

                _SetLanguage();//设置语言
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：SizeControl属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("控件大小"), Category("CustomList 通用")]
        public Size SizeControl//属性
        {
            get//读取
            {
                return sizeControl;
            }
            set//设置
            {
                if (value != sizeControl)//设置了新的数值
                {
                    sizeControl = value;

                    this.Size = value;

                    //

                    Size sizeValue = new Size(sizeControl.Width - iWidth_ControlWidth_ListHeaderWidth, customListHeader.Height);//临时变量
                    if (sizeValue != customListHeader.SizeListHeader)//设置了新的数值
                    {
                        customListHeader.SizeListHeader = sizeValue;
                    }

                    sizeValue = new Size(sizeControl.Width - iWidth_ControlWidth_ListHeaderWidth, customListItem1.Height);
                    if (sizeValue != customListItem1.SizeListItem)//设置了新的数值
                    {
                        customListItem1.SizeListItem = sizeValue;//列表项1
                        customListItem2.SizeListItem = sizeValue;//列表项2
                        customListItem3.SizeListItem = sizeValue;//列表项3
                        customListItem4.SizeListItem = sizeValue;//列表项4
                        customListItem5.SizeListItem = sizeValue;//列表项5
                        customListItem6.SizeListItem = sizeValue;//列表项6
                        customListItem7.SizeListItem = sizeValue;//列表项7
                        customListItem8.SizeListItem = sizeValue;//列表项8
                        customListItem9.SizeListItem = sizeValue;//列表项9
                        customListItem10.SizeListItem = sizeValue;//列表项10
                        customListItem11.SizeListItem = sizeValue;//列表项11
                        customListItem12.SizeListItem = sizeValue;//列表项12
                        customListItem13.SizeListItem = sizeValue;//列表项13
                        customListItem14.SizeListItem = sizeValue;//列表项14
                        customListItem15.SizeListItem = sizeValue;//列表项15
                        customListItem16.SizeListItem = sizeValue;//列表项16
                        customListItem17.SizeListItem = sizeValue;//列表项17
                    }

                    //更新页码控件位置

                    customButtonPage.Location = new Point(customButtonPage.Location.X, ClientRectangle.Bottom - iHeight_PageBottom_ControlBottom - customButtonPage.Size.Height);
                    customButtonPage.SizeButton = new Size(customListHeader.Width, customButtonPage.Size.Height);

                    //更新列表项数目

                    Int32 iItemsHeight = this.Size.Height - iHeight_ControlTop_ListHeaderTop - customListHeader.Height - iHeight_ListHeaderBottom_FirstListItem - iHeight_LastListItem_PageTop - customButtonPage.Height - iHeight_PageBottom_ControlBottom;//列表项控件区域
                    iItemControlNumber = iItemsHeight / customListItem1.Height;
                    if (iItemControlMaxNumber < iItemControlNumber)//超出范围
                    {
                        iItemControlNumber = iItemControlMaxNumber;
                    }

                    //更新列表项控件位置

                    _SetListItemsRectangle();

                    //

                    _ApplyListHeader();//应用列表头属性
                    _ApplyListItem();//应用列表项属性
                    _Apply(iItemDataNumber);//应用列表属性
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：PageHeight属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("页码控件高度"), Category("CustomList 通用")]
        public int PageHeight//属性
        {
            get//读取
            {
                return customButtonPage.Height;
            }
            set//设置
            {
                if (customButtonPage.Height != value)//设置了新的数值
                {
                    //更新页码控件位置

                    customButtonPage.Location = new Point(customButtonPage.Location.X, ClientRectangle.Bottom - iHeight_PageBottom_ControlBottom - customButtonPage.Size.Height);
                    customButtonPage.SizeButton = new System.Drawing.Size(customListHeader.Width, value);

                    //更新列表项数目

                    Int32 iItemsHeight = this.Size.Height - iHeight_ControlTop_ListHeaderTop - customListHeader.Height - iHeight_ListHeaderBottom_FirstListItem - iHeight_LastListItem_PageTop - customButtonPage.Height - iHeight_PageBottom_ControlBottom;//列表项控件区域
                    iItemControlNumber = iItemsHeight / customListItem1.Height;
                    if (iItemControlMaxNumber < iItemControlNumber)//超出范围
                    {
                        iItemControlNumber = iItemControlMaxNumber;
                    }

                    //更新列表项控件位置

                    _SetListItemsRectangle();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ListEnabled属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("列表使能状态。取值范围：true，使能；false：禁止"), Category("CustomList 通用")]
        public Boolean ListEnabled//属性
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

                    customListHeader.ListHeaderEnabled = value;//列表头

                    customListItem1.ListItemEnabled = value;//列表项1
                    customListItem2.ListItemEnabled = value;//列表项2
                    customListItem3.ListItemEnabled = value;//列表项3
                    customListItem4.ListItemEnabled = value;//列表项4
                    customListItem5.ListItemEnabled = value;//列表项5
                    customListItem6.ListItemEnabled = value;//列表项6
                    customListItem7.ListItemEnabled = value;//列表项7
                    customListItem8.ListItemEnabled = value;//列表项8
                    customListItem9.ListItemEnabled = value;//列表项9
                    customListItem10.ListItemEnabled = value;//列表项10
                    customListItem11.ListItemEnabled = value;//列表项11
                    customListItem12.ListItemEnabled = value;//列表项12
                    customListItem13.ListItemEnabled = value;//列表项13
                    customListItem14.ListItemEnabled = value;//列表项14
                    customListItem15.ListItemEnabled = value;//列表项15
                    customListItem16.ListItemEnabled = value;//列表项16
                    customListItem17.ListItemEnabled = value;//列表项17
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：TotalPage属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("包含的页码总数（取值为0，表示无有效的列表项）"), Category("CustomList 通用")]
        public int TotalPage//属性
        {
            get//读取
            {
                return iTotalPage;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ItemControlNumber属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("列表项控件的数目"), Category("CustomList 通用")]
        public int ItemControlNumber//属性
        {
            get//读取
            {
                return iItemControlNumber;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：CurrentPage属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("当前页码（从0开始。取值为-1，表示无有效的列表项）"), Category("CustomList 通用")]
        public int CurrentPage//属性
        {
            get//读取
            {
                return iCurrentPage;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：CurrentDataIndex属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("当前选择的项对应的外部数据数组的序号（0 ~ 外部数据数组长度 - 1。取值为-1，表示当前未选择任何项）"), Category("CustomList 通用")]
        public int CurrentDataIndex//属性
        {
            get//读取
            {
                return iCurrentDataIndex;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：CurrentListIndex属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("当前选择的项在列表中的序号（0 ~ iItemDataNumber - 1。取值为-1，表示当前未选择任何项）"), Category("CustomList 通用")]
        public int CurrentListIndex//属性
        {
            get//读取
            {
                return iCurrentListIndex;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：Index_Page属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("当前选择的项在当前页中的索引（0 ~ iItemControlNumber - 1。取值为-1，表示当前未选择任何项）"), Category("CustomList 通用")]
        public int Index_Page//属性
        {
            get//读取
            {
                return iIndex_Page;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：StartIndex属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("当前页中的起始索引（0 ~ iItemDataNumber - 1）"), Category("CustomList 通用")]
        public int StartIndex//属性
        {
            get//读取
            {
                return iStartIndex;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：EndIndex属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("当前页中的结束索引（0 ~ iItemDataNumber - 1）"), Category("CustomList 通用")]
        public int EndIndex//属性
        {
            get//读取
            {
                return iEndIndex;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ColumnNumber属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("列表中包含的列数"), Category("CustomList 通用")]
        public Int32 ColumnNumber//属性
        {
            get//读取
            {
                return customListHeader.ColumnNumber;
            }
            set//设置
            {
                if (value != customListHeader.ColumnNumber)//设置了新的数值
                {
                    customListHeader.ColumnNumber = value;//列表头

                    customListItem1.ColumnNumber = value;//列表项1
                    customListItem2.ColumnNumber = value;//列表项2
                    customListItem3.ColumnNumber = value;//列表项3
                    customListItem4.ColumnNumber = value;//列表项4
                    customListItem5.ColumnNumber = value;//列表项5
                    customListItem6.ColumnNumber = value;//列表项6
                    customListItem7.ColumnNumber = value;//列表项7
                    customListItem8.ColumnNumber = value;//列表项8
                    customListItem9.ColumnNumber = value;//列表项9
                    customListItem10.ColumnNumber = value;//列表项10
                    customListItem11.ColumnNumber = value;//列表项11
                    customListItem12.ColumnNumber = value;//列表项12
                    customListItem13.ColumnNumber = value;//列表项13
                    customListItem14.ColumnNumber = value;//列表项14
                    customListItem15.ColumnNumber = value;//列表项15
                    customListItem16.ColumnNumber = value;//列表项16
                    customListItem17.ColumnNumber = value;//列表项17

                    for (Int32 i = 0; i < iItemDataNumber; i++)//创建对象
                    {
                        Item_Data[i] = new CustomListItemData(value);
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
        [Browsable(true), Description("每列的宽度（初始显示的每列的宽度，[0]表示最左侧的列）"), Category("CustomList 通用")]
        public Int32[] ColumnWidth//属性
        {
            get//读取
            {
                return customListHeader.ColumnWidth;
            }
            set//设置
            {
                if (null != value)//有效
                {
                    customListHeader.ColumnWidth = value;//列表头

                    customListItem1.ColumnWidth = value;//列表项1
                    customListItem2.ColumnWidth = value;//列表项2
                    customListItem3.ColumnWidth = value;//列表项3
                    customListItem4.ColumnWidth = value;//列表项4
                    customListItem5.ColumnWidth = value;//列表项5
                    customListItem6.ColumnWidth = value;//列表项6
                    customListItem7.ColumnWidth = value;//列表项7
                    customListItem8.ColumnWidth = value;//列表项8
                    customListItem9.ColumnWidth = value;//列表项9
                    customListItem10.ColumnWidth = value;//列表项10
                    customListItem11.ColumnWidth = value;//列表项11
                    customListItem12.ColumnWidth = value;//列表项12
                    customListItem13.ColumnWidth = value;//列表项13
                    customListItem14.ColumnWidth = value;//列表项14
                    customListItem15.ColumnWidth = value;//列表项15
                    customListItem16.ColumnWidth = value;//列表项16
                    customListItem17.ColumnWidth = value;//列表项17

                    //

                    //ColumnNumber = value.Length;

                    ////

                    //value.CopyTo(customListHeader.ColumnWidth, 0);//列表头

                    //value.CopyTo(customListItem1.ColumnWidth, 0);//列表项1
                    //value.CopyTo(customListItem2.ColumnWidth, 0);//列表项2
                    //value.CopyTo(customListItem3.ColumnWidth, 0);//列表项3
                    //value.CopyTo(customListItem4.ColumnWidth, 0);//列表项4
                    //value.CopyTo(customListItem5.ColumnWidth, 0);//列表项5
                    //value.CopyTo(customListItem6.ColumnWidth, 0);//列表项6
                    //value.CopyTo(customListItem7.ColumnWidth, 0);//列表项7
                    //value.CopyTo(customListItem8.ColumnWidth, 0);//列表项8
                    //value.CopyTo(customListItem9.ColumnWidth, 0);//列表项9
                    //value.CopyTo(customListItem10.ColumnWidth, 0);//列表项10
                    //value.CopyTo(customListItem11.ColumnWidth, 0);//列表项11
                    //value.CopyTo(customListItem12.ColumnWidth, 0);//列表项12
                    //value.CopyTo(customListItem13.ColumnWidth, 0);//列表项13
                    //value.CopyTo(customListItem14.ColumnWidth, 0);//列表项14
                    //value.CopyTo(customListItem15.ColumnWidth, 0);//列表项15
                    //value.CopyTo(customListItem16.ColumnWidth, 0);//列表项16
                    //value.CopyTo(customListItem17.ColumnWidth, 0);//列表项17
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ColorControlBackground属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("控件背景颜色"), Category("CustomList 通用")]
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

        //----------------------------------------------------------------------
        // 功能说明：ColorPageText属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("列表框页码，绘制文本所使用的颜色"), Category("CustomList 通用")]
        public Color ColorPageText//属性
        {
            get//读取
            {
                return customButtonPage.ForeColor;
            }
            set//设置
            {
                if (value != customButtonPage.ForeColor)//设置了新的数值
                {
                    customButtonPage.ForeColor = value;//列表项1
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ColorPageBackground属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("列表框页码，绘制背景所使用的颜色"), Category("CustomList 通用")]
        public Color ColorPageBackground//属性
        {
            get//读取
            {
                return customButtonPage.BackgroundColor;
            }
            set//设置
            {
                if (value != customButtonPage.BackgroundColor)//设置了新的数值
                {
                    customButtonPage.BackgroundColor = value;//列表项1
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：FontListHeader属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("页码文本所使用的字体"), Category("CustomList 通用")]
        public Font FontPage//属性
        {
            get//读取
            {
                return customButtonPage.Font;
            }
            set//设置
            {
                if (value != customButtonPage.Font)//设置了新的数值
                {
                    customButtonPage.Font = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：SelectedItemNumber属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("列表中选择的项的数目"), Category("CustomList 通用")]
        public Int32 SelectedItemNumber//属性
        {
            get//读取
            {
                return iSelectedItemNumber;
            }
            set//设置
            {
                if (value != iSelectedItemNumber)//设置了新的数值
                {
                    iSelectedItemNumber = value;
                }
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：ColorListHeaderColumnText_Enable属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("控件使能时，绘制每列的名称所使用的颜色（[0]表示最左侧的列）"), Category("CustomList 列表头")]
        public Color[] ColorListHeaderColumnText_Enable//属性
        {
            get//读取
            {
                return customListHeader.ColorColumnText_Enable;
            }
            set//设置
            {
                if (value != null)
                {
                    customListHeader.ColorColumnText_Enable = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ColorListHeaderColumnText_Disable属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("控件禁止时，绘制每列的名称所使用的颜色（[0]表示最左侧的列）"), Category("CustomList 列表头")]
        public Color[] ColorListHeaderColumnText_Disable//属性
        {
            get//读取
            {
                return customListHeader.ColorColumnText_Disable;
            }
            set//设置
            {
                if (value != null)
                {
                    customListHeader.ColorColumnText_Disable = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ColorListHeaderBackground属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("列表头背景颜色"), Category("CustomList 列表头")]
        public Color ColorListHeaderBackground//属性
        {
            get//读取
            {
                return customListHeader.ColorControlBackground;
            }
            set//设置
            {
                if (value != customListHeader.ColorControlBackground)
                {
                    customListHeader.ColorControlBackground = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ListHeaderHeight属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("列表头控件高度"), Category("CustomList 列表头")]
        public int ListHeaderHeight//属性
        {
            get//读取
            {
                return customListHeader.Height;
            }
            set//设置
            {
                if (value != customListHeader.Height)
                {
                    customListHeader.SizeListHeader = new System.Drawing.Size(customListHeader.Width, value);

                    //更新列表项数目

                    Int32 iItemsHeight = this.Size.Height - iHeight_ControlTop_ListHeaderTop - customListHeader.Height - iHeight_ListHeaderBottom_FirstListItem - iHeight_LastListItem_PageTop - customButtonPage.Height - iHeight_PageBottom_ControlBottom;//列表项控件区域
                    iItemControlNumber = iItemsHeight / customListItem1.Height;
                    if (iItemControlMaxNumber < iItemControlNumber)//超出范围
                    {
                        iItemControlNumber = iItemControlMaxNumber;
                    }

                    //更新列表项控件位置

                    _SetListItemsRectangle();
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ListHeaderTextAlignment属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("列表头控件绘制文本的水平对齐方式"), Category("CustomList 列表头")]
        public StringAlignment ListHeaderTextAlignment
        {
            get//读取
            {
                return customListHeader.TextAlignment;
            }
            set//设置
            {
                if (value != customListHeader.TextAlignment)
                {
                    customListHeader.TextAlignment = value;
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Chinese_ColumnNameDisplay属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("列表头控件绘制的中文语言的原始文本数组（数组含义：[语言][[每列各个组]]）"), Category("CustomList 列表头")]
        public String[] Chinese_ColumnNameDisplay
        {
            get//读取
            {
                return customListHeader.Chinese_ColumnNameDisplay;
            }
            set//设置
            {
                if (null != value)//有效
                {
                    //ColumnNumber = value.Length;

                    //

                    customListHeader.Chinese_ColumnNameDisplay = value;
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：English_ColumnNameDisplay属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("列表头控件绘制的英文语言的原始文本数组（数组含义：[语言][[每列各个组]]）"), Category("CustomList 列表头")]
        public String[] English_ColumnNameDisplay
        {
            get//读取
            {
                return customListHeader.English_ColumnNameDisplay;
            }
            set//设置
            {
                if (null != value)//有效
                {
                    //ColumnNumber = value.Length;

                    //

                    customListHeader.English_ColumnNameDisplay = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：CurrentColumnNameGroupIndex属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("列表头控件当前绘制的文本所在组的索引值（从0开始）"), Category("CustomList 列表头")]
        public Int32[] CurrentColumnNameGroupIndex//属性
        {
            get//读取
            {
                return customListHeader.CurrentColumnNameGroupIndex;
            }
            set//设置
            {
                if (null != value)
                {
                    //ColumnNumber = value.Length;

                    //

                    customListHeader.CurrentColumnNameGroupIndex = value;
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ColumnNameXOffSetValue属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("列表头控件绘制文本的偏移量（绘制区域左侧 + 该数值"), Category("CustomList 列表头")]
        public Int32 ColumnNameXOffSetValue
        {
            get//读取
            {
                return customListHeader.XOffSetValue;
            }
            set//设置
            {
                if (value != customListHeader.XOffSetValue)
                {
                    customListHeader.XOffSetValue = value;
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：BitmapBackgroundNumber属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("背景图像数量"), Category("CustomList 列表头")]
        public Int32 BitmapBackgroundNumber
        {
            get//读取
            {
                return customListHeader.BitmapBackgroundNumber;
            }
            set//设置
            {
                if (value != customListHeader.BitmapBackgroundNumber)
                {
                    customListHeader.BitmapBackgroundNumber = value;
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：BitmapBackgroundIndex 属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("控件每列对应的图标索引值（从0开始）"), Category("CustomList 列表头")]
        public Int32[] BitmapBackgroundIndex
        {
            get//读取
            {
                return customListHeader.BitmapBackgroundIndex;
            }
            set//设置
            {
                if (value != customListHeader.BitmapBackgroundIndex)
                {
                    customListHeader.BitmapBackgroundIndex = value;
                }           
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：BitmapBackgroundWhole属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("整体背景图像"), Category("CustomList 列表头")]
        public Bitmap BitmapBackgroundWhole
        {
            get//读取
            {
                return customListHeader.BitmapBackgroundWhole;
            }
            set//设置
            {
                if (value != customListHeader.BitmapBackgroundWhole)
                {
                    customListHeader.BitmapBackgroundWhole = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：DrawType属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("列表头，绘图类型"), Category("CustomList 列表头")]
        public CustomDrawType DrawType//属性
        {
            get//读取
            {
                return customListHeader.DrawType;
            }
            set//设置
            {
                if (value != customListHeader.DrawType)//设置了新的数值
                {
                    customListHeader.DrawType = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：RectLeft属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("列表头，背景图像左部填充区域（CustomDrawType.Horizontal、CustomDrawType.Central时有效）"), Category("CustomList 列表头")]
        public Rectangle RectLeft//属性
        {
            get//读取
            {
                return customListHeader.RectLeft;
            }
            set//设置
            {
                if (value != customListHeader.RectLeft)//设置了新的数值
                {
                    customListHeader.RectLeft = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：RectTop属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("列表头，背景图像上部填充区域（CustomDrawType.Vertical、CustomDrawType.Central时有效）"), Category("CustomList 列表头")]
        public Rectangle RectTop//属性
        {
            get//读取
            {
                return customListHeader.RectTop;
            }
            set//设置
            {
                if (value != customListHeader.RectTop)//设置了新的数值
                {
                    customListHeader.RectTop = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：RectRight属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("列表头，背景图像右部填充区域（CustomDrawType.Horizontal、CustomDrawType.Central时有效）"), Category("CustomList 列表头")]
        public Rectangle RectRight//属性
        {
            get//读取
            {
                return customListHeader.RectRight;
            }
            set//设置
            {
                if (value != customListHeader.RectRight)//设置了新的数值
                {
                    customListHeader.RectRight = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：RectBottom属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("列表头，背景图像下部填充区域（CustomDrawType.Vertical、CustomDrawType.Central时有效）"), Category("CustomList 列表头")]
        public Rectangle RectBottom//属性
        {
            get//读取
            {
                return customListHeader.RectBottom;
            }
            set//设置
            {
                if (value != customListHeader.RectBottom)//设置了新的数值
                {
                    customListHeader.RectBottom = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：RectLeftTop属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("列表头，背景图像左上部矩形区域（CustomDrawType.Central时有效）"), Category("CustomList 列表头")]
        public Rectangle RectLeftTop//属性
        {
            get//读取
            {
                return customListHeader.RectLeftTop;
            }
            set//设置
            {
                if (value != customListHeader.RectLeftTop)//设置了新的数值
                {
                    customListHeader.RectLeftTop = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：RectRightTop属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("列表头，背景图像右上部矩形区域（CustomDrawType.Central时有效）"), Category("CustomList 列表头")]
        public Rectangle RectRightTop//属性
        {
            get//读取
            {
                return customListHeader.RectRightTop;
            }
            set//设置
            {
                if (value != customListHeader.RectRightTop)//设置了新的数值
                {
                    customListHeader.RectRightTop = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：RectLeftBottom属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("列表头，背景图像左下部矩形区域（CustomDrawType.Central时有效）"), Category("CustomList 列表头")]
        public Rectangle RectLeftBottom//属性
        {
            get//读取
            {
                return customListHeader.RectLeftBottom;
            }
            set//设置
            {
                if (value != customListHeader.RectLeftBottom)//设置了新的数值
                {
                    customListHeader.RectLeftBottom = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：RectRightBottom属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("列表头，背景图像右下部矩形区域（CustomDrawType.Central时有效）"), Category("CustomList 列表头")]
        public Rectangle RectRightBottom//属性
        {
            get//读取
            {
                return customListHeader.RectRightBottom;
            }
            set//设置
            {
                if (value != customListHeader.RectRightBottom)//设置了新的数值
                {
                    customListHeader.RectRightBottom = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：RectFill属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("列表头，背景图像中部填充区域（用于填充整个按钮底部背景，CustomDrawType.Vertical、CustomDrawType.Horizontal和CustomDrawType.Central时有效）"), Category("CustomList 列表头")]
        public Rectangle RectFill//属性
        {
            get//读取
            {
                return customListHeader.RectFill;
            }
            set//设置
            {
                if (value != customListHeader.RectFill)//设置了新的数值
                {
                    customListHeader.RectFill = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：FontListHeader属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("列表头绘制文本所使用的字体"), Category("CustomList 列表头")]
        public Font FontListHeader//属性
        {
            get//读取
            {
                return customListHeader.FontText;
            }
            set//设置
            {
                if (value != customListHeader.FontText)//设置了新的数值
                {
                    customListHeader.FontText = value;
                }
            }
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：ListItemXOffSetValue属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("列表项控件绘制文本的偏移量（绘制区域左侧 + 该数值"), Category("CustomList 列表项")]
        public Int32 ListItemXOffSetValue
        {
            get//读取
            {
                return customListItem1.XOffSetValue;
            }
            set//设置
            {
                customListItem1.XOffSetValue = value;
                customListItem2.XOffSetValue = value;
                customListItem3.XOffSetValue = value;
                customListItem4.XOffSetValue = value;
                customListItem5.XOffSetValue = value;
                customListItem6.XOffSetValue = value;
                customListItem7.XOffSetValue = value;
                customListItem8.XOffSetValue = value;
                customListItem9.XOffSetValue = value;
                customListItem10.XOffSetValue = value;
                customListItem11.XOffSetValue = value;
                customListItem12.XOffSetValue = value;
                customListItem13.XOffSetValue = value;
                customListItem14.XOffSetValue = value;
                customListItem15.XOffSetValue = value;
                customListItem16.XOffSetValue = value;
                customListItem17.XOffSetValue = value;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ColorListItemBackground属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("列表项背景颜色"), Category("CustomList 列表项")]
        public Color ColorListItemBackground//属性
        {
            get//读取
            {
                return customListItem1.ColorControlBackground;
            }
            set//设置
            {
                customListItem1.ColorControlBackground = value;
                customListItem2.ColorControlBackground = value;
                customListItem3.ColorControlBackground = value;
                customListItem4.ColorControlBackground = value;
                customListItem5.ColorControlBackground = value;
                customListItem6.ColorControlBackground = value;
                customListItem7.ColorControlBackground = value;
                customListItem8.ColorControlBackground = value;
                customListItem9.ColorControlBackground = value;
                customListItem10.ColorControlBackground = value;
                customListItem11.ColorControlBackground = value;
                customListItem12.ColorControlBackground = value;
                customListItem13.ColorControlBackground = value;
                customListItem14.ColorControlBackground = value;
                customListItem15.ColorControlBackground = value;
                customListItem16.ColorControlBackground = value;
                customListItem17.ColorControlBackground = value;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ListItemHeight属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("列表项控件高度"), Category("CustomList 列表项")]
        public int ListItemHeight//属性
        {
            get//读取
            {
                return customListItem1.Height;
            }
            set//设置
            {
                if (value != customListItem1.Height)//设置了新的数值
                {
                    customListItem1.SizeListItem = new System.Drawing.Size(customListItem1.Width, value);//列表项1
                    customListItem2.SizeListItem = new System.Drawing.Size(customListItem2.Width, value);//列表项2
                    customListItem3.SizeListItem = new System.Drawing.Size(customListItem3.Width, value);//列表项3
                    customListItem4.SizeListItem = new System.Drawing.Size(customListItem4.Width, value);//列表项4
                    customListItem5.SizeListItem = new System.Drawing.Size(customListItem5.Width, value);//列表项5
                    customListItem6.SizeListItem = new System.Drawing.Size(customListItem6.Width, value);//列表项6
                    customListItem7.SizeListItem = new System.Drawing.Size(customListItem7.Width, value);//列表项7
                    customListItem8.SizeListItem = new System.Drawing.Size(customListItem8.Width, value);//列表项8
                    customListItem9.SizeListItem = new System.Drawing.Size(customListItem9.Width, value);//列表项9
                    customListItem10.SizeListItem = new System.Drawing.Size(customListItem10.Width, value);//列表项10
                    customListItem11.SizeListItem = new System.Drawing.Size(customListItem11.Width, value);//列表项11
                    customListItem12.SizeListItem = new System.Drawing.Size(customListItem12.Width, value);//列表项12
                    customListItem13.SizeListItem = new System.Drawing.Size(customListItem13.Width, value);//列表项13
                    customListItem14.SizeListItem = new System.Drawing.Size(customListItem14.Width, value);//列表项14
                    customListItem15.SizeListItem = new System.Drawing.Size(customListItem15.Width, value);//列表项15
                    customListItem16.SizeListItem = new System.Drawing.Size(customListItem16.Width, value);//列表项16
                    customListItem17.SizeListItem = new System.Drawing.Size(customListItem17.Width, value);//列表项17

                    //更新列表项数目

                    Int32 iItemsHeight = this.Size.Height - iHeight_ControlTop_ListHeaderTop - customListHeader.Height - iHeight_ListHeaderBottom_FirstListItem - iHeight_LastListItem_PageTop - customButtonPage.Height - iHeight_PageBottom_ControlBottom;//列表项控件区域
                    iItemControlNumber = iItemsHeight / customListItem1.Height;
                    if (iItemControlMaxNumber < iItemControlNumber)//超出范围
                    {
                        iItemControlNumber = iItemControlMaxNumber;
                    }

                    //更新列表项控件位置

                    _SetListItemsRectangle();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ItemDataNumber属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("有效的列表项数目（0表示无有效的列表项）"), Category("CustomList 列表项")]
        public Int32 ItemDataNumber//属性
        {
            get//读取
            {
                return iItemDataNumber;
            }
            set//设置
            {
                if (iItemDataNumber != value)//设置了新的数值
                {
                    iItemDataNumber = value;

                    //

                    if (0 < value)//有效
                    {
                        Item_Data = new CustomListItemData[iItemDataNumber];//属性，列表项数据

                        for (Int32 i = 0; i < iItemDataNumber; i++)//创建对象
                        {
                            Item_Data[i] = new CustomListItemData(customListHeader.ColumnNumber);
                        }
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ItemData属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("列表项数据"), Category("CustomList 列表项")]
        public CustomListItemData[] ItemData//属性
        {
            get//读取
            {
                return Item_Data;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ListItemTextAlignment属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("列表头控件绘制文本的水平对齐方式"), Category("CustomList 列表项")]
        public StringAlignment ListItemTextAlignment
        {
            get//读取
            {
                return customListItem1.TextAlignment;
            }
            set//设置
            {
                customListItem1.TextAlignment = value;
                customListItem2.TextAlignment = value;
                customListItem3.TextAlignment = value;
                customListItem4.TextAlignment = value;
                customListItem5.TextAlignment = value;
                customListItem6.TextAlignment = value;
                customListItem7.TextAlignment = value;
                customListItem8.TextAlignment = value;
                customListItem9.TextAlignment = value;
                customListItem10.TextAlignment = value;
                customListItem11.TextAlignment = value;
                customListItem12.TextAlignment = value;
                customListItem13.TextAlignment = value;
                customListItem14.TextAlignment = value;
                customListItem15.TextAlignment = value;
                customListItem16.TextAlignment = value;
                customListItem17.TextAlignment = value;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：SelectedItemType属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("列表框项选中时的样式。取值范围：true，选中时改变背景颜色；false，选中时改变字体颜色"), Category("CustomList 列表项")]
        public Boolean SelectedItemType//属性
        {
            get//读取
            {
                return customListItem1.SelectedItemType;
            }
            set//设置
            {
                if (customListItem1.SelectedItemType != value)//设置了新的数值
                {
                    customListItem1.SelectedItemType = value;//列表项1
                    customListItem2.SelectedItemType = value;//列表项2
                    customListItem3.SelectedItemType = value;//列表项3
                    customListItem4.SelectedItemType = value;//列表项4
                    customListItem5.SelectedItemType = value;//列表项5
                    customListItem6.SelectedItemType = value;//列表项6
                    customListItem7.SelectedItemType = value;//列表项7
                    customListItem8.SelectedItemType = value;//列表项8
                    customListItem9.SelectedItemType = value;//列表项9
                    customListItem10.SelectedItemType = value;//列表项10
                    customListItem11.SelectedItemType = value;//列表项11
                    customListItem12.SelectedItemType = value;//列表项12
                    customListItem13.SelectedItemType = value;//列表项13
                    customListItem14.SelectedItemType = value;//列表项14
                    customListItem15.SelectedItemType = value;//列表项15
                    customListItem16.SelectedItemType = value;//列表项16
                    customListItem17.SelectedItemType = value;//列表项17
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ItemIconIndex属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("列表项图标索引（取值范围：>= 0）"), Category("CustomList 列表项")]
        public Int32[] ItemIconIndex//属性
        {
            get//读取
            {
                return customListItem1.ItemIconIndex;
            }
            set//设置
            {
                if (null != value)//有效
                {
                    ColumnNumber = value.Length;

                    //

                    value.CopyTo(customListItem1.ItemIconIndex, 0);//列表项1
                    value.CopyTo(customListItem2.ItemIconIndex, 0);//列表项2
                    value.CopyTo(customListItem3.ItemIconIndex, 0);//列表项3
                    value.CopyTo(customListItem4.ItemIconIndex, 0);//列表项4
                    value.CopyTo(customListItem5.ItemIconIndex, 0);//列表项5
                    value.CopyTo(customListItem6.ItemIconIndex, 0);//列表项6
                    value.CopyTo(customListItem7.ItemIconIndex, 0);//列表项7
                    value.CopyTo(customListItem8.ItemIconIndex, 0);//列表项8
                    value.CopyTo(customListItem9.ItemIconIndex, 0);//列表项9
                    value.CopyTo(customListItem10.ItemIconIndex, 0);//列表项10
                    value.CopyTo(customListItem11.ItemIconIndex, 0);//列表项11
                    value.CopyTo(customListItem12.ItemIconIndex, 0);//列表项12
                    value.CopyTo(customListItem13.ItemIconIndex, 0);//列表项13
                    value.CopyTo(customListItem14.ItemIconIndex, 0);//列表项14
                    value.CopyTo(customListItem15.ItemIconIndex, 0);//列表项15
                    value.CopyTo(customListItem16.ItemIconIndex, 0);//列表项16
                    value.CopyTo(customListItem17.ItemIconIndex, 0);//列表项17
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ItemDataDisplay属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("是否显示列表项数据（指的是是否显示sItemText中的文本数据）。取值范围：true，是；false，否"), Category("CustomList 列表项")]
        public bool[] ItemDataDisplay//属性
        {
            get//读取
            {
                return customListItem1.ItemDataDisplay;
            }
            set//设置
            {
                if (null != value)//有效
                {
                    ColumnNumber = value.Length;

                    //

                    value.CopyTo(customListItem1.ItemDataDisplay, 0);//列表项1
                    value.CopyTo(customListItem2.ItemDataDisplay, 0);//列表项2
                    value.CopyTo(customListItem3.ItemDataDisplay, 0);//列表项3
                    value.CopyTo(customListItem4.ItemDataDisplay, 0);//列表项4
                    value.CopyTo(customListItem5.ItemDataDisplay, 0);//列表项5
                    value.CopyTo(customListItem6.ItemDataDisplay, 0);//列表项6
                    value.CopyTo(customListItem7.ItemDataDisplay, 0);//列表项7
                    value.CopyTo(customListItem8.ItemDataDisplay, 0);//列表项8
                    value.CopyTo(customListItem9.ItemDataDisplay, 0);//列表项9
                    value.CopyTo(customListItem10.ItemDataDisplay, 0);//列表项10
                    value.CopyTo(customListItem11.ItemDataDisplay, 0);//列表项11
                    value.CopyTo(customListItem12.ItemDataDisplay, 0);//列表项12
                    value.CopyTo(customListItem13.ItemDataDisplay, 0);//列表项13
                    value.CopyTo(customListItem14.ItemDataDisplay, 0);//列表项14
                    value.CopyTo(customListItem15.ItemDataDisplay, 0);//列表项15
                    value.CopyTo(customListItem16.ItemDataDisplay, 0);//列表项16
                    value.CopyTo(customListItem17.ItemDataDisplay, 0);//列表项17
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：SelectionColumnIndex属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("列表项选择图标索引（取值范围：>= 0）"), Category("CustomList 列表项")]
        public Int32 SelectionColumnIndex//属性
        {
            get//读取
            {
                return customListItem1.SelectionColumnIndex;
            }
            set//设置
            {
                if (customListItem1.SelectionColumnIndex != value)//设置了新的数值
                {
                    customListItem1.SelectionColumnIndex = value;//列表项1
                    customListItem2.SelectionColumnIndex = value;//列表项2
                    customListItem3.SelectionColumnIndex = value;//列表项3
                    customListItem4.SelectionColumnIndex = value;//列表项4
                    customListItem5.SelectionColumnIndex = value;//列表项5
                    customListItem6.SelectionColumnIndex = value;//列表项6
                    customListItem7.SelectionColumnIndex = value;//列表项7
                    customListItem8.SelectionColumnIndex = value;//列表项8
                    customListItem9.SelectionColumnIndex = value;//列表项9
                    customListItem10.SelectionColumnIndex = value;//列表项10
                    customListItem11.SelectionColumnIndex = value;//列表项11
                    customListItem12.SelectionColumnIndex = value;//列表项12
                    customListItem13.SelectionColumnIndex = value;//列表项13
                    customListItem14.SelectionColumnIndex = value;//列表项14
                    customListItem15.SelectionColumnIndex = value;//列表项15
                    customListItem16.SelectionColumnIndex = value;//列表项16
                    customListItem17.SelectionColumnIndex = value;//列表项17
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ItemIconNumber属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("列表项图标"), Category("CustomList 列表项")]
        public Int32 ItemIconNumber//属性
        {
            get//读取
            {
                return iBitmapIconNumber;
            }
            set//设置
            {
                if (value != iBitmapIconNumber)
                {
                    iBitmapIconNumber = value;

                    //

                    if (0 < value)
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

                        bitmapIcon = new Bitmap[iBitmapIconNumber];

                        for (i = 0; i < bitmapIcon.Length; i++)
                        {
                            bitmapIcon[i] = null;
                        }
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：BitmapIcon属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("列表项图标"), Category("CustomList 列表项")]
        public Bitmap[] BitmapIcon//属性
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

                    ItemIconNumber = value.Length;

                    value.CopyTo(bitmapIcon, 0);
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：SizeItemIcon属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("列表框项图标大小（像素）"), Category("CustomList 列表项")]
        public Size[] SizeItemIcon//属性
        {
            get//读取
            {
                return customListItem1.SizeItemIcon;
            }
            set//设置
            {
                if (null != value)//有效
                {
                    ColumnNumber = value.Length;

                    //

                    customListItem1.SizeItemIcon = value;//列表项1
                    customListItem2.SizeItemIcon = value;//列表项2
                    customListItem3.SizeItemIcon = value;//列表项3
                    customListItem4.SizeItemIcon = value;//列表项4
                    customListItem5.SizeItemIcon = value;//列表项5
                    customListItem6.SizeItemIcon = value;//列表项6
                    customListItem7.SizeItemIcon = value;//列表项7
                    customListItem8.SizeItemIcon = value;//列表项8
                    customListItem9.SizeItemIcon = value;//列表项9
                    customListItem10.SizeItemIcon = value;//列表项10
                    customListItem11.SizeItemIcon = value;//列表项11
                    customListItem12.SizeItemIcon = value;//列表项12
                    customListItem13.SizeItemIcon = value;//列表项13
                    customListItem14.SizeItemIcon = value;//列表项14
                    customListItem15.SizeItemIcon = value;//列表项15
                    customListItem16.SizeItemIcon = value;//列表项16
                    customListItem17.SizeItemIcon = value;//列表项17
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ColorItemTextSelected属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("列表框项选中时，绘制文本所使用的颜色"), Category("CustomList 列表项")]
        public Color ColorItemTextSelected//属性
        {
            get//读取
            {
                return customListItem1.ColorTextSelected;
            }
            set//设置
            {
                if (customListItem1.ColorTextSelected != value)//设置了新的数值
                {
                    customListItem1.ColorTextSelected = value;//列表项1
                    customListItem2.ColorTextSelected = value;//列表项2
                    customListItem3.ColorTextSelected = value;//列表项3
                    customListItem4.ColorTextSelected = value;//列表项4
                    customListItem5.ColorTextSelected = value;//列表项5
                    customListItem6.ColorTextSelected = value;//列表项6
                    customListItem7.ColorTextSelected = value;//列表项7
                    customListItem8.ColorTextSelected = value;//列表项8
                    customListItem9.ColorTextSelected = value;//列表项9
                    customListItem10.ColorTextSelected = value;//列表项10
                    customListItem11.ColorTextSelected = value;//列表项11
                    customListItem12.ColorTextSelected = value;//列表项12
                    customListItem13.ColorTextSelected = value;//列表项13
                    customListItem14.ColorTextSelected = value;//列表项14
                    customListItem15.ColorTextSelected = value;//列表项15
                    customListItem16.ColorTextSelected = value;//列表项16
                    customListItem17.ColorTextSelected = value;//列表项17
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ColorItemTextUnselected属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("列表框项未选中时，绘制文本所使用的颜色"), Category("CustomList 列表项")]
        public Color ColorItemTextUnselected//属性
        {
            get//读取
            {
                return customListItem1.ColorTextUnselected;
            }
            set//设置
            {
                if (customListItem1.ColorTextUnselected != value)//设置了新的数值
                {
                    customListItem1.ColorTextUnselected = value;//列表项1
                    customListItem2.ColorTextUnselected = value;//列表项2
                    customListItem3.ColorTextUnselected = value;//列表项3
                    customListItem4.ColorTextUnselected = value;//列表项4
                    customListItem5.ColorTextUnselected = value;//列表项5
                    customListItem6.ColorTextUnselected = value;//列表项6
                    customListItem7.ColorTextUnselected = value;//列表项7
                    customListItem8.ColorTextUnselected = value;//列表项8
                    customListItem9.ColorTextUnselected = value;//列表项9
                    customListItem10.ColorTextUnselected = value;//列表项10
                    customListItem11.ColorTextUnselected = value;//列表项11
                    customListItem12.ColorTextUnselected = value;//列表项12
                    customListItem13.ColorTextUnselected = value;//列表项13
                    customListItem14.ColorTextUnselected = value;//列表项14
                    customListItem15.ColorTextUnselected = value;//列表项15
                    customListItem16.ColorTextUnselected = value;//列表项16
                    customListItem17.ColorTextUnselected = value;//列表项17
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ColorItemBackgroundSelected属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("列表框项选中时，绘制背景所使用的颜色"), Category("CustomList 列表项")]
        public Color ColorItemBackgroundSelected//属性
        {
            get//读取
            {
                return customListItem1.ColorBackgroundSelected;
            }
            set//设置
            {
                if (customListItem1.ColorBackgroundSelected != value)//设置了新的数值
                {
                    customListItem1.ColorBackgroundSelected = value;//列表项1
                    customListItem2.ColorBackgroundSelected = value;//列表项2
                    customListItem3.ColorBackgroundSelected = value;//列表项3
                    customListItem4.ColorBackgroundSelected = value;//列表项4
                    customListItem5.ColorBackgroundSelected = value;//列表项5
                    customListItem6.ColorBackgroundSelected = value;//列表项6
                    customListItem7.ColorBackgroundSelected = value;//列表项7
                    customListItem8.ColorBackgroundSelected = value;//列表项8
                    customListItem9.ColorBackgroundSelected = value;//列表项9
                    customListItem10.ColorBackgroundSelected = value;//列表项10
                    customListItem11.ColorBackgroundSelected = value;//列表项11
                    customListItem12.ColorBackgroundSelected = value;//列表项12
                    customListItem13.ColorBackgroundSelected = value;//列表项13
                    customListItem14.ColorBackgroundSelected = value;//列表项14
                    customListItem15.ColorBackgroundSelected = value;//列表项15
                    customListItem16.ColorBackgroundSelected = value;//列表项16
                    customListItem17.ColorBackgroundSelected = value;//列表项17
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ColorItemBackgroundUnselected属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("列表框项未选中时，绘制背景所使用的颜色"), Category("CustomList 列表项")]
        public Color ColorItemBackgroundUnselected//属性
        {
            get//读取
            {
                return customListItem1.ColorBackgroundUnselected;
            }
            set//设置
            {
                if (customListItem1.ColorBackgroundUnselected != value)//设置了新的数值
                {
                    customListItem1.ColorBackgroundUnselected = value;//列表项1
                    customListItem2.ColorBackgroundUnselected = value;//列表项2
                    customListItem3.ColorBackgroundUnselected = value;//列表项3
                    customListItem4.ColorBackgroundUnselected = value;//列表项4
                    customListItem5.ColorBackgroundUnselected = value;//列表项5
                    customListItem6.ColorBackgroundUnselected = value;//列表项6
                    customListItem7.ColorBackgroundUnselected = value;//列表项7
                    customListItem8.ColorBackgroundUnselected = value;//列表项8
                    customListItem9.ColorBackgroundUnselected = value;//列表项9
                    customListItem10.ColorBackgroundUnselected = value;//列表项10
                    customListItem11.ColorBackgroundUnselected = value;//列表项11
                    customListItem12.ColorBackgroundUnselected = value;//列表项12
                    customListItem13.ColorBackgroundUnselected = value;//列表项13
                    customListItem14.ColorBackgroundUnselected = value;//列表项14
                    customListItem15.ColorBackgroundUnselected = value;//列表项15
                    customListItem16.ColorBackgroundUnselected = value;//列表项16
                    customListItem17.ColorBackgroundUnselected = value;//列表项17
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：FontListItem属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("列表项绘制文本所使用的字体"), Category("CustomList 列表项")]
        public Font FontListItem//属性
        {
            get//读取
            {
                return customListItem1.FontText;
            }
            set//设置
            {
                if (customListItem1.FontText != value)//设置了新的数值
                {
                    customListItem1.FontText = value;//列表项1
                    customListItem2.FontText = value;//列表项2
                    customListItem3.FontText = value;//列表项3
                    customListItem4.FontText = value;//列表项4
                    customListItem5.FontText = value;//列表项5
                    customListItem6.FontText = value;//列表项6
                    customListItem7.FontText = value;//列表项7
                    customListItem8.FontText = value;//列表项8
                    customListItem9.FontText = value;//列表项9
                    customListItem10.FontText = value;//列表项10
                    customListItem11.FontText = value;//列表项11
                    customListItem12.FontText = value;//列表项12
                    customListItem13.FontText = value;//列表项13
                    customListItem14.FontText = value;//列表项14
                    customListItem15.FontText = value;//列表项15
                    customListItem16.FontText = value;//列表项16
                    customListItem17.FontText = value;//列表项17
                }
            }
        }

        //函数

        //----------------------------------------------------------------------
        // 功能说明：应用设置完成的列表头属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _ApplyListHeader()
        {
            customListHeader._Apply();//列表头
        }

        //----------------------------------------------------------------------
        // 功能说明：应用设置完成的列表项属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _ApplyListItem()
        {
            customListItem1._Apply();//列表项1
            customListItem2._Apply();//列表项2
            customListItem3._Apply();//列表项3
            customListItem4._Apply();//列表项4
            customListItem5._Apply();//列表项5
            customListItem6._Apply();//列表项6
            customListItem7._Apply();//列表项7
            customListItem8._Apply();//列表项8
            customListItem9._Apply();//列表项9
            customListItem10._Apply();//列表项10
            customListItem11._Apply();//列表项11
            customListItem12._Apply();//列表项12
            customListItem13._Apply();//列表项13
            customListItem14._Apply();//列表项14
            customListItem15._Apply();//列表项15
            customListItem16._Apply();//列表项16
            customListItem17._Apply();//列表项17
        }

        //----------------------------------------------------------------------
        // 功能说明：应用设置完成的列表属性
        // 输入参数：1.iItemDataNumber_temp：有效的列表项数目（0表示无有效的列表项）
        //         2.iCurrentDataIndex_temp：当前选择的项对应的外部数据数组的序号（0 ~ 外部数据数组长度 - 1。取值为-1，表示当前未选择任何项）
        //         3.iCurrentListIndex_temp：当前选择的项在设备列表中的序号（0 ~ iItemDataNumber - 1。取值为-1，表示当前未选择任何项）
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _Apply(Int32 iItemDataNumber_temp, Int32 iCurrentDataIndex_temp = -1, Int32 iCurrentListIndex_temp = -1)
        {
            _ClearAllListItem();//清除控件显示的所有文本和图标，并置为未选中状态

            //

            this.ItemDataNumber = iItemDataNumber_temp;

            iCurrentDataIndex = iCurrentDataIndex_temp;//当前选择的项对应的外部数据数组的序号（0 ~ 外部数据数组长度 - 1。取值为-1，表示当前未选择任何项）
            iCurrentListIndex = iCurrentListIndex_temp;//当前选择的项在设备列表中的序号（0 ~ iItemDataNumber - 1。取值为-1，表示当前未选择任何项）

            iSelectedItemNumber = 0;//列表中选择的项的数目

            if (iItemDataNumber > 0)//存在有效项
            {
                iTotalPage = _GetTotalPage(iItemDataNumber);//包含的页码总数

                iCurrentPage = _GetPage(iCurrentListIndex);//当前页码

                if (0 > iCurrentPage)
                {
                    iCurrentPage = 0;//当前页码
                }

                //

                //_SetPage(true);//设置当前页中的列表项
            }
            else//不存在有效项
            {
                iTotalPage = 0;//包含的页码总数

                iCurrentPage = -1;//当前页码

                iIndex_Page = -1;//当前选择的项在当前页中的索引（0 ~ 16。取值为-1，表示当前未选择任何项）

                //

                //_SetPage(false);//设置当前页中的控件
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击列表中的列表项
        // 输入参数：1.iIndex：点击的项在列表当前页中的索引值（0 ~ iItemControlNumber - 1）
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _ClickListItem(Int32 iIndex)
        {
            switch (iIndex)
            {
                case 0:

                    _ClickListItem(customListItem1, 0);

                    break;
                case 1:

                    _ClickListItem(customListItem2, 1);

                    break;
                case 2:

                    _ClickListItem(customListItem3, 2);

                    break;
                case 3:

                    _ClickListItem(customListItem4, 3);

                    break;
                case 4:

                    _ClickListItem(customListItem5, 4);

                    break;
                case 5:

                    _ClickListItem(customListItem6, 5);

                    break;
                case 6:

                    _ClickListItem(customListItem7, 6);

                    break;
                case 7:

                    _ClickListItem(customListItem8, 7);

                    break;
                case 8:

                    _ClickListItem(customListItem9, 8);

                    break;
                case 9:

                    _ClickListItem(customListItem10, 9);

                    break;
                case 10:

                    _ClickListItem(customListItem11, 10);

                    break;
                case 11:

                    _ClickListItem(customListItem12, 11);

                    break;
                case 12:

                    _ClickListItem(customListItem13, 12);

                    break;
                case 13:

                    _ClickListItem(customListItem14, 13);

                    break;
                case 14:

                    _ClickListItem(customListItem15, 14);

                    break;
                case 15:

                    _ClickListItem(customListItem16, 15);

                    break;
                case 16:

                    _ClickListItem(customListItem17, 16);

                    break;
                default:
                    break;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：设置列表数据
        // 输入参数：1.iItemDataNumber_temp：有效的列表项数目（0表示无有效的列表项）
        //         2.iCurrentDataIndex_temp：当前选择的项对应的外部数据数组的序号（0 ~ 外部数据数组长度 - 1。取值为-1，表示当前未选择任何项）
        //         3.iCurrentListIndex_temp：当前选择的项在设备列表中的序号（0 ~ iItemDataNumber - 1。取值为-1，表示当前未选择任何项）
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _SetListData(Int32 iItemDataNumber_temp, Int32 iCurrentDataIndex_temp = -1, Int32 iCurrentListIndex_temp = -1)
        {
            this.ItemDataNumber = iItemDataNumber_temp;

            iCurrentDataIndex = iCurrentDataIndex_temp;//当前选择的项对应的外部数据数组的序号（0 ~ 外部数据数组长度 - 1。取值为-1，表示当前未选择任何项）
            iCurrentListIndex = iCurrentListIndex_temp;//当前选择的项在设备列表中的序号（0 ~ iItemDataNumber - 1。取值为-1，表示当前未选择任何项）

            if (iItemDataNumber > 0)//存在有效项
            {
                iTotalPage = _GetTotalPage(iItemDataNumber);//包含的页码总数

                iCurrentPage = _GetPage(iCurrentListIndex);//当前页码

                if (0 > iCurrentPage)
                {
                    iCurrentPage = 0;//当前页码
                }
            }
            else//不存在有效项
            {
                iTotalPage = 0;//包含的页码总数

                iCurrentPage = -1;//当前页码

                iIndex_Page = -1;//当前选择的项在当前页中的索引（0 ~ 16。取值为-1，表示当前未选择任何项）
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：设置各个列表项的范围
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetListItemsRectangle()
        {
            //更新列表项控件位置

            customListItem1.Location = new Point(customListItem1.Location.X, customListHeader.Bottom + iHeight_ListHeaderBottom_FirstListItem);
            customListItem1.Size = new Size(customListItem1.Width, customListItem1.Height);

            customListItem2.Location = new Point(customListItem2.Location.X, customListItem1.Bottom);
            customListItem2.Size = new Size(customListItem2.Width, customListItem2.Height);

            customListItem3.Location = new Point(customListItem3.Location.X, customListItem2.Bottom);
            customListItem3.Size = new Size(customListItem3.Width, customListItem3.Height);

            customListItem4.Location = new Point(customListItem4.Location.X, customListItem3.Bottom);
            customListItem4.Size = new Size(customListItem4.Width, customListItem4.Height);

            customListItem5.Location = new Point(customListItem5.Location.X, customListItem4.Bottom);
            customListItem5.Size = new Size(customListItem5.Width, customListItem5.Height);

            customListItem6.Location = new Point(customListItem6.Location.X, customListItem5.Bottom);
            customListItem6.Size = new Size(customListItem6.Width, customListItem6.Height);

            customListItem7.Location = new Point(customListItem7.Location.X, customListItem6.Bottom);
            customListItem7.Size = new Size(customListItem7.Width, customListItem7.Height);

            customListItem8.Location = new Point(customListItem8.Location.X, customListItem7.Bottom);
            customListItem8.Size = new Size(customListItem8.Width, customListItem8.Height);

            customListItem9.Location = new Point(customListItem9.Location.X, customListItem8.Bottom);
            customListItem9.Size = new Size(customListItem9.Width, customListItem9.Height);

            customListItem10.Location = new Point(customListItem10.Location.X, customListItem9.Bottom);
            customListItem10.Size = new Size(customListItem10.Width, customListItem10.Height);

            customListItem11.Location = new Point(customListItem11.Location.X, customListItem10.Bottom);
            customListItem11.Size = new Size(customListItem11.Width, customListItem11.Height);

            customListItem12.Location = new Point(customListItem12.Location.X, customListItem11.Bottom);
            customListItem12.Size = new Size(customListItem12.Width, customListItem12.Height);

            customListItem13.Location = new Point(customListItem13.Location.X, customListItem12.Bottom);
            customListItem13.Size = new Size(customListItem13.Width, customListItem13.Height);

            customListItem14.Location = new Point(customListItem14.Location.X, customListItem13.Bottom);
            customListItem14.Size = new Size(customListItem14.Width, customListItem14.Height);

            customListItem15.Location = new Point(customListItem15.Location.X, customListItem14.Bottom);
            customListItem15.Size = new Size(customListItem15.Width, customListItem15.Height);

            customListItem16.Location = new Point(customListItem16.Location.X, customListItem15.Bottom);
            customListItem16.Size = new Size(customListItem16.Width, customListItem16.Height);

            customListItem17.Location = new Point(customListItem17.Location.X, customListItem16.Bottom);
            customListItem17.Size = new Size(customListItem17.Width, customListItem17.Height);
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
            customListHeader.Language = language;

            //

            customButtonPage.Language = language;
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【Previous Page】或【Next Page】按钮对列表控件进行翻页时进行相关操作
        // 输入参数：1.bPreviousNext：点击的按钮的类型。取值范围：true：【Previous Page】按钮；【Next Page】按钮
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _ClickPage(bool bPreviousNext)
        {
            //更新页码

            if (bPreviousNext)//点击了【Previous Page】按钮
            {
                if (iCurrentPage > 0)//非首页
                {
                    iCurrentPage--;//更新页码
                }
                else//首页
                {
                    iCurrentPage = iTotalPage - 1;//末页
                }
            }
            else//点击了【Next Page】按钮
            {
                if (iCurrentPage < iTotalPage - 1)//非末页
                {
                    iCurrentPage++;//更新页码
                }
                else//末页
                {
                    iCurrentPage = 0;//首页
                }
            }

            //设置当前页中的控件

            _SetPage(true);
        }

        //----------------------------------------------------------------------
        // 功能说明：点击列表中的列表项时进行相关操作
        // 输入参数：1.ListItem：点击的列表项
        //         2.iIndex：点击的项在列表当前页中的索引值（0 ~ iItemControlNumber - 1）
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _ClickListItem(CustomListItem ListItem, Int32 iIndex)
        {
            //更新点击前选择的项

            if (0 <= iIndex_Page)//当前页中存在选中的项
            {
                _SelectListItem(iIndex_Page, false);//取消选择当前列表项
            }

            //更新点击后选择的项

            iIndex_Page = iIndex;//当前页列表中选择的项的索引值

            iCurrentListIndex = _GetListIndex(iIndex_Page, iCurrentPage);//列表列表中当前选择的项的索引值
            iCurrentDataIndex = ListItem.ItemFlag;//前选择的项对应的外部数据数组的序号

            if (0 <= ListItem.SelectionColumnIndex)//存在选中图标
            {
                ListItem.ItemData.ItemDataDisplay[ListItem.SelectionColumnIndex] = !(ListItem.ItemData.ItemDataDisplay[ListItem.SelectionColumnIndex]);//更新数值

                //

                if (ListItem.ItemData.ItemDataDisplay[ListItem.SelectionColumnIndex])//未选择
                {
                    iSelectedItemNumber--;//属性，列表中选择的项的数目
                }
                else//选择
                {
                    iSelectedItemNumber++;//属性，列表中选择的项的数目
                }
            }

            ListItem.Selected = true;//选中当前列表项

            //

            customButtonPage.Chinese_TextDisplay = new String[1] { sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] + "：" + (iCurrentPage + 1).ToString() + " / " + iTotalPage.ToString() + "  " + "[ " + (iCurrentListIndex + 1).ToString() + "，" + (iStartIndex + 1).ToString() + ".." + (iEndIndex + 1).ToString() + " ]" };//设置显示的文本
            customButtonPage.English_TextDisplay = new String[1] { sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] + "：" + (iCurrentPage + 1).ToString() + " / " + iTotalPage.ToString() + "  " + "[ " + (iCurrentListIndex + 1).ToString() + "，" + (iStartIndex + 1).ToString() + ".." + (iEndIndex + 1).ToString() + " ]" };//设置显示的文本

            //事件

            if (null != CustomListItem_Click)//有效
            {
                CustomListItem_Click(this, new EventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：双击列表中的列表项时进行相关操作
        // 输入参数：1.ListItem：点击的列表项
        //         2.iIndex：点击的项在列表当前页中的索引值（0 ~ iItemControlNumber - 1）
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _DoubleClickListItem(CustomListItem ListItem, Int32 iIndex)
        {
            //事件

            if (null != CustomListItem_DoubleClick)//有效
            {
                CustomListItem_DoubleClick(this, new EventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：设置页中的列表项
        // 输入参数：1.iPageIndex：页码索引值
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _SetPage(Int32 iPageIndex)
        {
            if (0 < iItemDataNumber)//存在有效列表项
            {
                if (0 <= iPageIndex && iPageIndex < iTotalPage)
                {
                    iCurrentPage = iPageIndex;

                    _SetPage(true);
                }
            }
            else//不存在有效列表项
            {
                _SetPage(false);
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：设置当前页中的列表项
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _SetPage()
        {
            if (0 < iItemDataNumber)//存在有效列表项
            {
                _SetPage(true);
            }
            else//不存在有效列表项
            {
                _SetPage(false);
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：设置当前页中的列表项
        // 输入参数：1.bListItem：是否存在有效的列表项。取值范围：true，是；false，否
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetPage(bool bListItem)
        {
            if (bListItem)//存在有效的列表项
            {
                //获取当前页数据

                iStartIndex = _GetStartIndex(iCurrentPage);//当前页中的起始索引（0 ~ iItemDataNumber - 1）
                iEndIndex = _GetEndIndex(iCurrentPage);//当前页中的结束索引（0 ~ iItemDataNumber - 1）

                //设置设备项

                int i = 0;//循环控制变量
                int j = 0;//循环控制变量
                int iItemNumber = 0;//当前页中的设备项数目

                int[] iDataIndex = new int[iItemControlNumber];//列表项数据索引值，临时变量

                for (i = 0; i < iItemControlNumber; i++)//赋初值
                {
                    iDataIndex[i] = -1;//赋初值
                }

                for (i = 0; i < iItemDataNumber; i++)//获取当前页数据
                {
                    if (j >= iStartIndex && j <= iEndIndex)//当前页中
                    {
                        iDataIndex[iItemNumber] = i;

                        iItemNumber++;
                    }

                    j++;//累加
                }

                _SetListItem(iItemNumber, iDataIndex);//设置各个列表项控件（同时取消选中所有列表项）

                //

                if (-1 != iCurrentListIndex)//选择了列表中的某一项
                {
                    if (iCurrentPage == _GetPage(iCurrentListIndex))//当前选择的项在当前页
                    {
                        iIndex_Page = _GetPageItemIndex(iCurrentListIndex);//当前选择的项在当前页中的索引（0 ~ iItemControlNumber - 1。取值为-1，表示当前未选择任何项）

                        _SelectListItem(iIndex_Page, true);//选中设备项
                    }
                    else//当前选择的项不在当前页
                    {
                        iIndex_Page = -1;//当前选择的项在当前页中的索引（0 ~ iItemControlNumber - 1。取值为-1，表示当前未选择任何项）
                    }

                    customButtonPage.Chinese_TextDisplay = new String[1] { sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] + "：" + (iCurrentPage + 1).ToString() + " / " + iTotalPage.ToString() + "  " + "[ " + (iCurrentListIndex + 1).ToString() + "，" + (iStartIndex + 1).ToString() + ".." + (iEndIndex + 1).ToString() + " ]" };//设置显示的文本
                    customButtonPage.English_TextDisplay = new String[1] { sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] + "：" + (iCurrentPage + 1).ToString() + " / " + iTotalPage.ToString() + "  " + "[ " + (iCurrentListIndex + 1).ToString() + "，" + (iStartIndex + 1).ToString() + ".." + (iEndIndex + 1).ToString() + " ]" };//设置显示的文本
                }
                else//未选择列表中的任何项
                {
                    iIndex_Page = -1;//当前选择的项在当前页中的索引（0 ~ iItemControlNumber - 1。取值为-1，表示当前未选择任何项）

                    customButtonPage.Chinese_TextDisplay = new String[1] { sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] + "：" + (iCurrentPage + 1).ToString() + " / " + (iTotalPage).ToString() + "  " + "[ " + (0).ToString() + "，" + (iStartIndex + 1).ToString() + ".." + (iEndIndex + 1).ToString() + " ]" };//设置显示的文本
                    customButtonPage.English_TextDisplay = new String[1] { sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] + "：" + (iCurrentPage + 1).ToString() + " / " + (iTotalPage).ToString() + "  " + "[ " + (0).ToString() + "，" + (iStartIndex + 1).ToString() + ".." + (iEndIndex + 1).ToString() + " ]" };//设置显示的文本
                }
            }
            else//不存在有效的列表项
            {
                customButtonPage.Chinese_TextDisplay = new String[1] { sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] + "：" + (1).ToString() + " / " + (1).ToString() + "  " + "[ " + (0).ToString() + "，" + (0).ToString() + ".." + (0).ToString() + " ]" };//设置显示的文本
                customButtonPage.English_TextDisplay = new String[1] { sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] + "：" + (1).ToString() + " / " + (1).ToString() + "  " + "[ " + (0).ToString() + "，" + (0).ToString() + ".." + (0).ToString() + " ]" };//设置显示的文本

                //_ClearAllListItem();//清除控件显示的所有文本和图标，并置为未选中状态
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：获取控件中包含的页码总数
        // 输入参数：1.iTotalNumber：有效列表项数目
        // 输出参数：无
        // 返回值：获取的页码
        //----------------------------------------------------------------------
        private int _GetTotalPage(int iTotalNumber)
        {
            int iPage = iTotalNumber / iItemControlNumber;

            if (0 != iTotalNumber % iItemControlNumber)//更新数值
            {
                iPage++;
            }

            return iPage;//包含的页码总数
        }

        //----------------------------------------------------------------------
        // 功能说明：获取当前选择的项（索引值0 ~ iItemDataNumber - 1）所在的页码
        // 输入参数：1.iIndex：列表中当前选择的项的索引值（0 ~ iItemDataNumber - 1）
        // 输出参数：无
        // 返回值：获取的页码
        //----------------------------------------------------------------------
        public int _GetPage(int iIndex)
        {
            if (-1 == iIndex)//无效
            {
                return -1;
            }
            else//有效
            {
                return iIndex / iItemControlNumber;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：根据当前选择的列表中的项的索引值（索引值0 ~ iItemDataNumber - 1），获取其在当前页中的索引值（0 ~ iItemControlNumber - 1）
        // 输入参数：1.iIndex：列表中的项的索引值（索引值0 ~ iItemDataNumber - 1）
        // 输出参数：无
        // 返回值：获取的索引值
        //----------------------------------------------------------------------
        public int _GetPageItemIndex(int iIndex)
        {
            if (-1 == iIndex)//无效
            {
                return -1;
            }
            else//有效
            {
                return iIndex % iItemControlNumber;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：根据当前选择的项在当前页中的索引值（0 ~ iItemControlNumber - 1），获取其在列表中的项的索引值（索引值0 ~ iItemDataNumber - 1）
        // 输入参数：1.iIndex：项在当前页中的索引值（0 ~ iItemControlNumber - 1）
        //         2.iPage：当前页码
        // 输出参数：无
        // 返回值：获取的索引值
        //----------------------------------------------------------------------
        private int _GetListIndex(int iIndex, int iPage)
        {
            if (-1 == iIndex || -1 == iPage)//无效
            {
                return -1;
            }
            else//有效
            {
                return iIndex + iItemControlNumber * iPage;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：获取当前页中的起始索引（0 ~ iItemDataNumber - 1）
        // 输入参数：1.iPage：当前页码
        // 输出参数：无
        // 返回值：获取的索引值
        //----------------------------------------------------------------------
        private int _GetStartIndex(int iPage)
        {
            if (-1 == iPage)//无效
            {
                return -1;
            }
            else//有效
            {
                return iPage * iItemControlNumber;//当前页中的起始索引（0 ~ iItemDataNumber - 1）
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：获取当前页中的结束索引（0 ~ iItemDataNumber - 1）
        // 输入参数：1.iPage：当前页码
        // 输出参数：无
        // 返回值：获取的索引值
        //----------------------------------------------------------------------
        private int _GetEndIndex(int iPage)
        {
            if (-1 == iPage)//无效
            {
                return -1;
            }
            else//有效
            {
                if (iPage == iTotalPage - 1)//末页
                {
                    return iItemDataNumber - 1;//当前页中的结束索引（0 ~ iItemDataNumber - 1）
                }
                else//非末页
                {
                    return iPage * iItemControlNumber + iItemControlNumber - 1;//当前页中的结束索引（0 ~ iItemDataNumber - 1）
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：选中或取消选中某一列表项
        // 输入参数：1.iIndex：列表项索引值（从0开始）
        //         2.bSelected：列表项控件是否被选中。取值范围：true，是；false，否
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _SelectListItem(Int32 iIndex, Boolean bSelected)
        {
            if (0 == iIndex)//列表项1
            {
                customListItem1.Selected = bSelected;//列表项控件是否被选中。取值范围：true，是；false，否
            }
            else if (1 == iIndex)//列表项2
            {
                customListItem2.Selected = bSelected;//列表项控件是否被选中。取值范围：true，是；false，否
            }
            else if (2 == iIndex)//列表项3
            {
                customListItem3.Selected = bSelected;//列表项控件是否被选中。取值范围：true，是；false，否
            }
            else if (3 == iIndex)//列表项4
            {
                customListItem4.Selected = bSelected;//列表项控件是否被选中。取值范围：true，是；false，否
            }
            else if (4 == iIndex)//列表项5
            {
                customListItem5.Selected = bSelected;//列表项控件是否被选中。取值范围：true，是；false，否
            }
            else if (5 == iIndex)//列表项6
            {
                customListItem6.Selected = bSelected;//列表项控件是否被选中。取值范围：true，是；false，否
            }
            else if (6 == iIndex)//列表项7
            {
                customListItem7.Selected = bSelected;//列表项控件是否被选中。取值范围：true，是；false，否
            }
            else if (7 == iIndex)//列表项8
            {
                customListItem8.Selected = bSelected;//列表项控件是否被选中。取值范围：true，是；false，否
            }
            else if (8 == iIndex)//列表项9
            {
                customListItem9.Selected = bSelected;//列表项控件是否被选中。取值范围：true，是；false，否
            }
            else if (9 == iIndex)//列表项10
            {
                customListItem10.Selected = bSelected;//列表项控件是否被选中。取值范围：true，是；false，否
            }
            else if (10 == iIndex)//列表项11
            {
                customListItem11.Selected = bSelected;//列表项控件是否被选中。取值范围：true，是；false，否
            }
            else if (11 == iIndex)//列表项12
            {
                customListItem12.Selected = bSelected;//列表项控件是否被选中。取值范围：true，是；false，否
            }
            else if (12 == iIndex)//列表项13
            {
                customListItem13.Selected = bSelected;//列表项控件是否被选中。取值范围：true，是；false，否
            }
            else if (13 == iIndex)//列表项14
            {
                customListItem14.Selected = bSelected;//列表项控件是否被选中。取值范围：true，是；false，否
            }
            else if (14 == iIndex)//列表项15
            {
                customListItem15.Selected = bSelected;//列表项控件是否被选中。取值范围：true，是；false，否
            }
            else if (15 == iIndex)//列表项16
            {
                customListItem16.Selected = bSelected;//列表项控件是否被选中。取值范围：true，是；false，否
            }
            else//16，列表项17
            {
                customListItem17.Selected = bSelected;//列表项控件是否被选中。取值范围：true，是；false，否
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：选中或取消选中某一列表项
        // 输入参数：1.iIndex：列表项索引值（从0开始）
        //         2.bEnable：列表项控件是否使能。取值范围：true，是；false，否
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _SetListItemEnable(Int32 iIndex, Boolean bEnable)
        {
            if (0 == iIndex)//列表项1
            {
                customListItem1.ListItemEnabled = bEnable;//列表项控件是否使能。取值范围：true，是；false，否
            }
            else if (1 == iIndex)//列表项2
            {
                customListItem2.ListItemEnabled = bEnable;//列表项控件是否使能。取值范围：true，是；false，否
            }
            else if (2 == iIndex)//列表项3
            {
                customListItem3.ListItemEnabled = bEnable;//列表项控件是否使能。取值范围：true，是；false，否
            }
            else if (3 == iIndex)//列表项4
            {
                customListItem4.ListItemEnabled = bEnable;//列表项控件是否使能。取值范围：true，是；false，否
            }
            else if (4 == iIndex)//列表项5
            {
                customListItem5.ListItemEnabled = bEnable;//列表项控件是否使能。取值范围：true，是；false，否
            }
            else if (5 == iIndex)//列表项6
            {
                customListItem6.ListItemEnabled = bEnable;//列表项控件是否使能。取值范围：true，是；false，否
            }
            else if (6 == iIndex)//列表项7
            {
                customListItem7.ListItemEnabled = bEnable;//列表项控件是否使能。取值范围：true，是；false，否
            }
            else if (7 == iIndex)//列表项8
            {
                customListItem8.ListItemEnabled = bEnable;//列表项控件是否使能。取值范围：true，是；false，否
            }
            else if (8 == iIndex)//列表项9
            {
                customListItem9.ListItemEnabled = bEnable;//列表项控件是否使能。取值范围：true，是；false，否
            }
            else if (9 == iIndex)//列表项10
            {
                customListItem10.ListItemEnabled = bEnable;//列表项控件是否使能。取值范围：true，是；false，否
            }
            else if (10 == iIndex)//列表项11
            {
                customListItem11.ListItemEnabled = bEnable;//列表项控件是否使能。取值范围：true，是；false，否
            }
            else if (11 == iIndex)//列表项12
            {
                customListItem12.ListItemEnabled = bEnable;//列表项控件是否使能。取值范围：true，是；false，否
            }
            else if (12 == iIndex)//列表项13
            {
                customListItem13.ListItemEnabled = bEnable;//列表项控件是否使能。取值范围：true，是；false，否
            }
            else if (13 == iIndex)//列表项14
            {
                customListItem14.ListItemEnabled = bEnable;//列表项控件是否使能。取值范围：true，是；false，否
            }
            else if (14 == iIndex)//列表项15
            {
                customListItem15.ListItemEnabled = bEnable;//列表项控件是否使能。取值范围：true，是；false，否
            }
            else if (15 == iIndex)//列表项16
            {
                customListItem16.ListItemEnabled = bEnable;//列表项控件是否使能。取值范围：true，是；false，否
            }
            else//16，列表项17
            {
                customListItem17.ListItemEnabled = bEnable;//列表项控件是否使能。取值范围：true，是；false，否
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：刷新所有列表项
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _Refresh()
        {
            customListItem1._Refresh();//刷新
            customListItem2._Refresh();//刷新
            customListItem3._Refresh();//刷新
            customListItem4._Refresh();//刷新
            customListItem5._Refresh();//刷新
            customListItem6._Refresh();//刷新
            customListItem7._Refresh();//刷新
            customListItem8._Refresh();//刷新
            customListItem9._Refresh();//刷新
            customListItem10._Refresh();//刷新
            customListItem11._Refresh();//刷新
            customListItem12._Refresh();//刷新
            customListItem13._Refresh();//刷新
            customListItem14._Refresh();//刷新
            customListItem15._Refresh();//刷新
            customListItem16._Refresh();//刷新
            customListItem17._Refresh();//刷新
        }

        //----------------------------------------------------------------------
        // 功能说明：刷新指定列表项
        // 输入参数：1.iIndex：列表项索引值（从0开始）
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _Refresh(Int32 iIndex)
        {
            if (0 == iIndex)//列表项1
            {
                customListItem1._Refresh();//刷新
            }
            else if (1 == iIndex)//列表项2
            {
                customListItem2._Refresh();//刷新
            }
            else if (2 == iIndex)//列表项3
            {
                customListItem3._Refresh();//刷新
            }
            else if (3 == iIndex)//列表项4
            {
                customListItem4._Refresh();//刷新
            }
            else if (4 == iIndex)//列表项5
            {
                customListItem5._Refresh();//刷新
            }
            else if (5 == iIndex)//列表项6
            {
                customListItem6._Refresh();//刷新
            }
            else if (6 == iIndex)//列表项7
            {
                customListItem7._Refresh();//刷新
            }
            else if (7 == iIndex)//列表项8
            {
                customListItem8._Refresh();//刷新
            }
            else if (8 == iIndex)//列表项9
            {
                customListItem9._Refresh();//刷新
            }
            else if (9 == iIndex)//列表项10
            {
                customListItem10._Refresh();//刷新
            }
            else if (10 == iIndex)//列表项11
            {
                customListItem11._Refresh();//刷新
            }
            else if (11 == iIndex)//列表项12
            {
                customListItem12._Refresh();//刷新
            }
            else if (12 == iIndex)//列表项13
            {
                customListItem13._Refresh();//刷新
            }
            else if (13 == iIndex)//列表项14
            {
                customListItem14._Refresh();//刷新
            }
            else if (14 == iIndex)//列表项15
            {
                customListItem15._Refresh();//刷新
            }
            else if (15 == iIndex)//列表项16
            {
                customListItem16._Refresh();//刷新
            }
            else//16，列表项17
            {
                customListItem17._Refresh();//刷新
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：选择/取消选择所有项
        // 输入参数：1.bState：选择类型。取值范围：true，选择所有；false，取消选择所有
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _SelectAll(Boolean bState)
        {
            if (0 <= SelectionColumnIndex)//存在选中图标
            {
                Int32 i = 0;//循环控制变量

                for (i = 0; i < iItemDataNumber; i++)
                {
                    Item_Data[i].ItemDataDisplay[SelectionColumnIndex] = !bState;
                }

                //

                if (bState)//全选
                {
                    iSelectedItemNumber = iItemDataNumber;//属性，列表中选择的项的数目
                }
                else//取消选择所有
                {
                    iSelectedItemNumber = 0;//属性，列表中选择的项的数目
                }

                //

                _Refresh();//刷新
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：清除控件显示的所有文本和图标，并置为未选中状态
        // 输入参数：1.ListItem：设备项
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _ClearListItem(CustomListItem ListItem)
        {
            ListItem._ClearAll();//清除控件显示的所有文本和图标

            ListItem.Selected = false;//置为未选中状态
        }

        //----------------------------------------------------------------------
        // 功能说明：清除控件显示的所有文本和图标，并置为未选中状态
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _ClearAllListItem()
        {
            _ClearListItem(customListItem1);//1
            _ClearListItem(customListItem2);//2
            _ClearListItem(customListItem3);//3
            _ClearListItem(customListItem4);//4
            _ClearListItem(customListItem5);//5
            _ClearListItem(customListItem6);//6
            _ClearListItem(customListItem7);//7
            _ClearListItem(customListItem8);//8
            _ClearListItem(customListItem9);//9
            _ClearListItem(customListItem10);//10
            _ClearListItem(customListItem11);//11
            _ClearListItem(customListItem12);//12
            _ClearListItem(customListItem13);//13
            _ClearListItem(customListItem14);//14
            _ClearListItem(customListItem15);//15
            _ClearListItem(customListItem16);//16
            _ClearListItem(customListItem17);//17

            //

            iItemDataNumber = 0;//属性，有效的列表项数目（0表示无有效的列表项）
            Item_Data = null;//属性，列表项数据
            iTotalPage = 0;//属性（只读），包含的页码总数（取值为0，表示无有效的列表项）
            iCurrentPage = 0;//属性（只读），当前页码（从0开始。取值为-1，表示无有效的列表项）
            iCurrentDataIndex = -1;//属性（只读），当前选择的项对应的外部数据数组的序号（0 ~ 外部数据数组长度 - 1。取值为-1，表示当前未选择任何项）
            iCurrentListIndex = -1;//属性（只读），当前选择的项在列表中的序号（0 ~ iItemDataNumber - 1。取值为-1，表示当前未选择任何项）
            iIndex_Page = -1;//属性（只读），当前选择的项在当前页中的索引（0 ~ iItemControlNumber - 1。取值为-1，表示当前未选择任何项）
            iStartIndex = 0;//属性（只读），当前页中的起始索引（0 ~ iItemDataNumber - 1）
            iEndIndex = 0;//属性（只读），当前页中的结束索引（0 ~ iItemDataNumber - 1）
            iSelectedItemNumber = 0;//属性，列表中选择的项的数目

            //

            customButtonPage.Chinese_TextDisplay = new String[1] { sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] + "：" + "--" + " / " + "--" + "  " + "[ " + "--" + "，" + "--" + ".." + "--" + " ]" };//设置显示的文本
            customButtonPage.English_TextDisplay = new String[1] { sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] + "：" + "--" + " / " + "--" + "  " + "[ " + "--" + "，" + "--" + ".." + "--" + " ]" };//设置显示的文本
        }

        //----------------------------------------------------------------------
        // 功能说明：设置列表项
        // 输入参数：1.ListItem：列表项
        //         2.ListItemData：列表数据
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetListItem(CustomListItem ListItem, CustomListItemData ListItemData)
        {
            Int32 i = 0;//循环控制变量

            ListItem.ItemData.ItemText = ListItemData.ItemText;
            ListItem.ItemData.ItemIconIndex = ListItemData.ItemIconIndex;
            ListItem.ItemData.ItemDataDisplay = ListItemData.ItemDataDisplay;

            for (i = 0; i < ListItem.ColumnNumber; i++)
            {
                if (0 <= ListItemData.ItemIconIndex[i])//有效
                {
                    if (null != ListItem.BitmapIcon[i])
                    {
                        ListItem.BitmapIcon[i].Dispose();
                    }
                    ListItem.BitmapIcon[i] = (Bitmap)bitmapIcon[ListItemData.ItemIconIndex[i]].Clone();
                }
            }

            //

            ListItem._CheckItem();//检查列表项是否有效

            //

            ListItem.ItemFlag = ListItemData.ItemFlag;

            ListItem.Selected = false;//未选中
        }

        //----------------------------------------------------------------------
        // 功能说明：设置列表项（所有项都将置为未选中状态）
        // 输入参数：1.iItemNumber：当前页中包含的有效列表项数目
        //         2.iDataIndex：Item_Data数组索引值
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _SetListItem(Int32 iItemNumber, Int32[] iDataIndex)
        {
            if (iItemControlMaxNumber == iItemNumber)//包含17个列表项
            {
                _SetListItem(customListItem1, Item_Data[iDataIndex[iItemControlMaxNumber - 17]]);//ListItem1，设置列表项

                _SetListItem(customListItem2, Item_Data[iDataIndex[iItemControlMaxNumber - 16]]);//ListItem2，设置列表项

                _SetListItem(customListItem3, Item_Data[iDataIndex[iItemControlMaxNumber - 15]]);//ListItem3，设置列表项

                _SetListItem(customListItem4, Item_Data[iDataIndex[iItemControlMaxNumber - 14]]);//ListItem4，设置列表项

                _SetListItem(customListItem5, Item_Data[iDataIndex[iItemControlMaxNumber - 13]]);//ListItem5，设置列表项

                _SetListItem(customListItem6, Item_Data[iDataIndex[iItemControlMaxNumber - 12]]);//ListItem6，设置列表项

                _SetListItem(customListItem7, Item_Data[iDataIndex[iItemControlMaxNumber - 11]]);//ListItem7，设置列表项

                _SetListItem(customListItem8, Item_Data[iDataIndex[iItemControlMaxNumber - 10]]);//ListItem8，设置列表项

                _SetListItem(customListItem9, Item_Data[iDataIndex[iItemControlMaxNumber - 9]]);//ListItem9，设置列表项

                _SetListItem(customListItem10, Item_Data[iDataIndex[iItemControlMaxNumber - 8]]);//ListItem10，设置列表项

                _SetListItem(customListItem11, Item_Data[iDataIndex[iItemControlMaxNumber - 7]]);//ListItem11，设置列表项

                _SetListItem(customListItem12, Item_Data[iDataIndex[iItemControlMaxNumber - 6]]);//ListItem12，设置列表项

                _SetListItem(customListItem13, Item_Data[iDataIndex[iItemControlMaxNumber - 5]]);//ListItem13，设置列表项

                _SetListItem(customListItem14, Item_Data[iDataIndex[iItemControlMaxNumber - 4]]);//ListItem14，设置列表项

                _SetListItem(customListItem15, Item_Data[iDataIndex[iItemControlMaxNumber - 3]]);//ListItem15，设置列表项

                _SetListItem(customListItem16, Item_Data[iDataIndex[iItemControlMaxNumber - 2]]);//ListItem16，设置列表项

                _SetListItem(customListItem17, Item_Data[iDataIndex[iItemControlMaxNumber - 1]]);//ListItem17，设置列表项
            }
            else if (iItemControlMaxNumber - 1 == iItemNumber)//包含16个列表项
            {
                _SetListItem(customListItem1, Item_Data[iDataIndex[iItemControlMaxNumber - 17]]);//ListItem1，设置列表项

                _SetListItem(customListItem2, Item_Data[iDataIndex[iItemControlMaxNumber - 16]]);//ListItem2，设置列表项

                _SetListItem(customListItem3, Item_Data[iDataIndex[iItemControlMaxNumber - 15]]);//ListItem3，设置列表项

                _SetListItem(customListItem4, Item_Data[iDataIndex[iItemControlMaxNumber - 14]]);//ListItem4，设置列表项

                _SetListItem(customListItem5, Item_Data[iDataIndex[iItemControlMaxNumber - 13]]);//ListItem5，设置列表项

                _SetListItem(customListItem6, Item_Data[iDataIndex[iItemControlMaxNumber - 12]]);//ListItem6，设置列表项

                _SetListItem(customListItem7, Item_Data[iDataIndex[iItemControlMaxNumber - 11]]);//ListItem7，设置列表项

                _SetListItem(customListItem8, Item_Data[iDataIndex[iItemControlMaxNumber - 10]]);//ListItem8，设置列表项

                _SetListItem(customListItem9, Item_Data[iDataIndex[iItemControlMaxNumber - 9]]);//ListItem9，设置列表项

                _SetListItem(customListItem10, Item_Data[iDataIndex[iItemControlMaxNumber - 8]]);//ListItem10，设置列表项

                _SetListItem(customListItem11, Item_Data[iDataIndex[iItemControlMaxNumber - 7]]);//ListItem11，设置列表项

                _SetListItem(customListItem12, Item_Data[iDataIndex[iItemControlMaxNumber - 6]]);//ListItem12，设置列表项

                _SetListItem(customListItem13, Item_Data[iDataIndex[iItemControlMaxNumber - 5]]);//ListItem13，设置列表项

                _SetListItem(customListItem14, Item_Data[iDataIndex[iItemControlMaxNumber - 4]]);//ListItem14，设置列表项

                _SetListItem(customListItem15, Item_Data[iDataIndex[iItemControlMaxNumber - 3]]);//ListItem15，设置列表项

                _SetListItem(customListItem16, Item_Data[iDataIndex[iItemControlMaxNumber - 2]]);//ListItem16，设置列表项

                //

                _ClearListItem(customListItem17);//ListItem17，清除控件显示的文本，并置为未选中状态
            }
            else if (iItemControlMaxNumber - 2 == iItemNumber)//包含15个列表项
            {
                _SetListItem(customListItem1, Item_Data[iDataIndex[iItemControlMaxNumber - 17]]);//ListItem1，设置列表项

                _SetListItem(customListItem2, Item_Data[iDataIndex[iItemControlMaxNumber - 16]]);//ListItem2，设置列表项

                _SetListItem(customListItem3, Item_Data[iDataIndex[iItemControlMaxNumber - 15]]);//ListItem3，设置列表项

                _SetListItem(customListItem4, Item_Data[iDataIndex[iItemControlMaxNumber - 14]]);//ListItem4，设置列表项

                _SetListItem(customListItem5, Item_Data[iDataIndex[iItemControlMaxNumber - 13]]);//ListItem5，设置列表项

                _SetListItem(customListItem6, Item_Data[iDataIndex[iItemControlMaxNumber - 12]]);//ListItem6，设置列表项

                _SetListItem(customListItem7, Item_Data[iDataIndex[iItemControlMaxNumber - 11]]);//ListItem7，设置列表项

                _SetListItem(customListItem8, Item_Data[iDataIndex[iItemControlMaxNumber - 10]]);//ListItem8，设置列表项

                _SetListItem(customListItem9, Item_Data[iDataIndex[iItemControlMaxNumber - 9]]);//ListItem9，设置列表项

                _SetListItem(customListItem10, Item_Data[iDataIndex[iItemControlMaxNumber - 8]]);//ListItem10，设置列表项

                _SetListItem(customListItem11, Item_Data[iDataIndex[iItemControlMaxNumber - 7]]);//ListItem11，设置列表项

                _SetListItem(customListItem12, Item_Data[iDataIndex[iItemControlMaxNumber - 6]]);//ListItem12，设置列表项

                _SetListItem(customListItem13, Item_Data[iDataIndex[iItemControlMaxNumber - 5]]);//ListItem13，设置列表项

                _SetListItem(customListItem14, Item_Data[iDataIndex[iItemControlMaxNumber - 4]]);//ListItem14，设置列表项

                _SetListItem(customListItem15, Item_Data[iDataIndex[iItemControlMaxNumber - 3]]);//ListItem15，设置列表项

                //

                _ClearListItem(customListItem16);//ListItem16，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem17);//ListItem17，清除控件显示的文本，并置为未选中状态
            }
            else if (iItemControlMaxNumber - 3 == iItemNumber)//包含14个列表项
            {
                _SetListItem(customListItem1, Item_Data[iDataIndex[iItemControlMaxNumber - 17]]);//ListItem1，设置列表项

                _SetListItem(customListItem2, Item_Data[iDataIndex[iItemControlMaxNumber - 16]]);//ListItem2，设置列表项

                _SetListItem(customListItem3, Item_Data[iDataIndex[iItemControlMaxNumber - 15]]);//ListItem3，设置列表项

                _SetListItem(customListItem4, Item_Data[iDataIndex[iItemControlMaxNumber - 14]]);//ListItem4，设置列表项

                _SetListItem(customListItem5, Item_Data[iDataIndex[iItemControlMaxNumber - 13]]);//ListItem5，设置列表项

                _SetListItem(customListItem6, Item_Data[iDataIndex[iItemControlMaxNumber - 12]]);//ListItem6，设置列表项

                _SetListItem(customListItem7, Item_Data[iDataIndex[iItemControlMaxNumber - 11]]);//ListItem7，设置列表项

                _SetListItem(customListItem8, Item_Data[iDataIndex[iItemControlMaxNumber - 10]]);//ListItem8，设置列表项

                _SetListItem(customListItem9, Item_Data[iDataIndex[iItemControlMaxNumber - 9]]);//ListItem9，设置列表项

                _SetListItem(customListItem10, Item_Data[iDataIndex[iItemControlMaxNumber - 8]]);//ListItem10，设置列表项

                _SetListItem(customListItem11, Item_Data[iDataIndex[iItemControlMaxNumber - 7]]);//ListItem11，设置列表项

                _SetListItem(customListItem12, Item_Data[iDataIndex[iItemControlMaxNumber - 6]]);//ListItem12，设置列表项

                _SetListItem(customListItem13, Item_Data[iDataIndex[iItemControlMaxNumber - 5]]);//ListItem13，设置列表项

                _SetListItem(customListItem14, Item_Data[iDataIndex[iItemControlMaxNumber - 4]]);//ListItem14，设置列表项

                //

                _ClearListItem(customListItem15);//ListItem15，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem16);//ListItem16，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem17);//ListItem17，清除控件显示的文本，并置为未选中状态
            }
            else if (iItemControlMaxNumber - 4 == iItemNumber)//包含13个列表项
            {
                _SetListItem(customListItem1, Item_Data[iDataIndex[iItemControlMaxNumber - 17]]);//ListItem1，设置列表项

                _SetListItem(customListItem2, Item_Data[iDataIndex[iItemControlMaxNumber - 16]]);//ListItem2，设置列表项

                _SetListItem(customListItem3, Item_Data[iDataIndex[iItemControlMaxNumber - 15]]);//ListItem3，设置列表项

                _SetListItem(customListItem4, Item_Data[iDataIndex[iItemControlMaxNumber - 14]]);//ListItem4，设置列表项

                _SetListItem(customListItem5, Item_Data[iDataIndex[iItemControlMaxNumber - 13]]);//ListItem5，设置列表项

                _SetListItem(customListItem6, Item_Data[iDataIndex[iItemControlMaxNumber - 12]]);//ListItem6，设置列表项

                _SetListItem(customListItem7, Item_Data[iDataIndex[iItemControlMaxNumber - 11]]);//ListItem7，设置列表项

                _SetListItem(customListItem8, Item_Data[iDataIndex[iItemControlMaxNumber - 10]]);//ListItem8，设置列表项

                _SetListItem(customListItem9, Item_Data[iDataIndex[iItemControlMaxNumber - 9]]);//ListItem9，设置列表项

                _SetListItem(customListItem10, Item_Data[iDataIndex[iItemControlMaxNumber - 8]]);//ListItem10，设置列表项

                _SetListItem(customListItem11, Item_Data[iDataIndex[iItemControlMaxNumber - 7]]);//ListItem11，设置列表项

                _SetListItem(customListItem12, Item_Data[iDataIndex[iItemControlMaxNumber - 6]]);//ListItem12，设置列表项

                _SetListItem(customListItem13, Item_Data[iDataIndex[iItemControlMaxNumber - 5]]);//ListItem13，设置列表项

                //

                _ClearListItem(customListItem14);//ListItem14，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem15);//ListItem15，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem16);//ListItem16，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem17);//ListItem17，清除控件显示的文本，并置为未选中状态
            }
            else if (iItemControlMaxNumber - 5 == iItemNumber)//包含12个列表项
            {
                _SetListItem(customListItem1, Item_Data[iDataIndex[iItemControlMaxNumber - 17]]);//ListItem1，设置列表项

                _SetListItem(customListItem2, Item_Data[iDataIndex[iItemControlMaxNumber - 16]]);//ListItem2，设置列表项

                _SetListItem(customListItem3, Item_Data[iDataIndex[iItemControlMaxNumber - 15]]);//ListItem3，设置列表项

                _SetListItem(customListItem4, Item_Data[iDataIndex[iItemControlMaxNumber - 14]]);//ListItem4，设置列表项

                _SetListItem(customListItem5, Item_Data[iDataIndex[iItemControlMaxNumber - 13]]);//ListItem5，设置列表项

                _SetListItem(customListItem6, Item_Data[iDataIndex[iItemControlMaxNumber - 12]]);//ListItem6，设置列表项

                _SetListItem(customListItem7, Item_Data[iDataIndex[iItemControlMaxNumber - 11]]);//ListItem7，设置列表项

                _SetListItem(customListItem8, Item_Data[iDataIndex[iItemControlMaxNumber - 10]]);//ListItem8，设置列表项

                _SetListItem(customListItem9, Item_Data[iDataIndex[iItemControlMaxNumber - 9]]);//ListItem9，设置列表项

                _SetListItem(customListItem10, Item_Data[iDataIndex[iItemControlMaxNumber - 8]]);//ListItem10，设置列表项

                _SetListItem(customListItem11, Item_Data[iDataIndex[iItemControlMaxNumber - 7]]);//ListItem11，设置列表项

                _SetListItem(customListItem12, Item_Data[iDataIndex[iItemControlMaxNumber - 6]]);//ListItem12，设置列表项

                //

                _ClearListItem(customListItem13);//ListItem13，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem14);//ListItem14，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem15);//ListItem15，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem16);//ListItem16，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem17);//ListItem17，清除控件显示的文本，并置为未选中状态
            }
            else if (iItemControlMaxNumber - 6 == iItemNumber)//包含11个列表项
            {
                _SetListItem(customListItem1, Item_Data[iDataIndex[iItemControlMaxNumber - 17]]);//ListItem1，设置列表项

                _SetListItem(customListItem2, Item_Data[iDataIndex[iItemControlMaxNumber - 16]]);//ListItem2，设置列表项

                _SetListItem(customListItem3, Item_Data[iDataIndex[iItemControlMaxNumber - 15]]);//ListItem3，设置列表项

                _SetListItem(customListItem4, Item_Data[iDataIndex[iItemControlMaxNumber - 14]]);//ListItem4，设置列表项

                _SetListItem(customListItem5, Item_Data[iDataIndex[iItemControlMaxNumber - 13]]);//ListItem5，设置列表项

                _SetListItem(customListItem6, Item_Data[iDataIndex[iItemControlMaxNumber - 12]]);//ListItem6，设置列表项

                _SetListItem(customListItem7, Item_Data[iDataIndex[iItemControlMaxNumber - 11]]);//ListItem7，设置列表项

                _SetListItem(customListItem8, Item_Data[iDataIndex[iItemControlMaxNumber - 10]]);//ListItem8，设置列表项

                _SetListItem(customListItem9, Item_Data[iDataIndex[iItemControlMaxNumber - 9]]);//ListItem9，设置列表项

                _SetListItem(customListItem10, Item_Data[iDataIndex[iItemControlMaxNumber - 8]]);//ListItem10，设置列表项

                _SetListItem(customListItem11, Item_Data[iDataIndex[iItemControlMaxNumber - 7]]);//ListItem11，设置列表项

                //

                _ClearListItem(customListItem12);//ListItem12，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem13);//ListItem13，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem14);//ListItem14，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem15);//ListItem15，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem16);//ListItem16，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem17);//ListItem17，清除控件显示的文本，并置为未选中状态
            }
            else if (iItemControlMaxNumber - 7 == iItemNumber)//包含10个列表项
            {
                _SetListItem(customListItem1, Item_Data[iDataIndex[iItemControlMaxNumber - 17]]);//ListItem1，设置列表项

                _SetListItem(customListItem2, Item_Data[iDataIndex[iItemControlMaxNumber - 16]]);//ListItem2，设置列表项

                _SetListItem(customListItem3, Item_Data[iDataIndex[iItemControlMaxNumber - 15]]);//ListItem3，设置列表项

                _SetListItem(customListItem4, Item_Data[iDataIndex[iItemControlMaxNumber - 14]]);//ListItem4，设置列表项

                _SetListItem(customListItem5, Item_Data[iDataIndex[iItemControlMaxNumber - 13]]);//ListItem5，设置列表项

                _SetListItem(customListItem6, Item_Data[iDataIndex[iItemControlMaxNumber - 12]]);//ListItem6，设置列表项

                _SetListItem(customListItem7, Item_Data[iDataIndex[iItemControlMaxNumber - 11]]);//ListItem7，设置列表项

                _SetListItem(customListItem8, Item_Data[iDataIndex[iItemControlMaxNumber - 10]]);//ListItem8，设置列表项

                _SetListItem(customListItem9, Item_Data[iDataIndex[iItemControlMaxNumber - 9]]);//ListItem9，设置列表项

                _SetListItem(customListItem10, Item_Data[iDataIndex[iItemControlMaxNumber - 8]]);//ListItem10，设置列表项

                //

                _ClearListItem(customListItem11);//ListItem11，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem12);//ListItem12，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem13);//ListItem13，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem14);//ListItem14，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem15);//ListItem15，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem16);//ListItem16，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem17);//ListItem17，清除控件显示的文本，并置为未选中状态
            }
            else if (iItemControlMaxNumber - 8 == iItemNumber)//包含9个列表项
            {
                _SetListItem(customListItem1, Item_Data[iDataIndex[iItemControlMaxNumber - 17]]);//ListItem1，设置列表项

                _SetListItem(customListItem2, Item_Data[iDataIndex[iItemControlMaxNumber - 16]]);//ListItem2，设置列表项

                _SetListItem(customListItem3, Item_Data[iDataIndex[iItemControlMaxNumber - 15]]);//ListItem3，设置列表项

                _SetListItem(customListItem4, Item_Data[iDataIndex[iItemControlMaxNumber - 14]]);//ListItem4，设置列表项

                _SetListItem(customListItem5, Item_Data[iDataIndex[iItemControlMaxNumber - 13]]);//ListItem5，设置列表项

                _SetListItem(customListItem6, Item_Data[iDataIndex[iItemControlMaxNumber - 12]]);//ListItem6，设置列表项

                _SetListItem(customListItem7, Item_Data[iDataIndex[iItemControlMaxNumber - 11]]);//ListItem7，设置列表项

                _SetListItem(customListItem8, Item_Data[iDataIndex[iItemControlMaxNumber - 10]]);//ListItem8，设置列表项

                _SetListItem(customListItem9, Item_Data[iDataIndex[iItemControlMaxNumber - 9]]);//ListItem9，设置列表项

                //

                _ClearListItem(customListItem10);//ListItem10，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem11);//ListItem11，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem12);//ListItem12，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem13);//ListItem13，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem14);//ListItem14，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem15);//ListItem15，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem16);//ListItem16，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem17);//ListItem17，清除控件显示的文本，并置为未选中状态
            }
            else if (iItemControlMaxNumber - 9 == iItemNumber)//包含8个列表项
            {
                _SetListItem(customListItem1, Item_Data[iDataIndex[iItemControlMaxNumber - 17]]);//ListItem1，设置列表项

                _SetListItem(customListItem2, Item_Data[iDataIndex[iItemControlMaxNumber - 16]]);//ListItem2，设置列表项

                _SetListItem(customListItem3, Item_Data[iDataIndex[iItemControlMaxNumber - 15]]);//ListItem3，设置列表项

                _SetListItem(customListItem4, Item_Data[iDataIndex[iItemControlMaxNumber - 14]]);//ListItem4，设置列表项

                _SetListItem(customListItem5, Item_Data[iDataIndex[iItemControlMaxNumber - 13]]);//ListItem5，设置列表项

                _SetListItem(customListItem6, Item_Data[iDataIndex[iItemControlMaxNumber - 12]]);//ListItem6，设置列表项

                _SetListItem(customListItem7, Item_Data[iDataIndex[iItemControlMaxNumber - 11]]);//ListItem7，设置列表项

                _SetListItem(customListItem8, Item_Data[iDataIndex[iItemControlMaxNumber - 10]]);//ListItem8，设置列表项

                //

                _ClearListItem(customListItem9);//ListItem9，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem10);//ListItem10，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem11);//ListItem11，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem12);//ListItem12，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem13);//ListItem13，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem14);//ListItem14，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem15);//ListItem15，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem16);//ListItem16，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem17);//ListItem17，清除控件显示的文本，并置为未选中状态
            }
            else if (iItemControlMaxNumber - 10 == iItemNumber)//包含7个列表项
            {
                _SetListItem(customListItem1, Item_Data[iDataIndex[iItemControlMaxNumber - 17]]);//ListItem1，设置列表项

                _SetListItem(customListItem2, Item_Data[iDataIndex[iItemControlMaxNumber - 16]]);//ListItem2，设置列表项

                _SetListItem(customListItem3, Item_Data[iDataIndex[iItemControlMaxNumber - 15]]);//ListItem3，设置列表项

                _SetListItem(customListItem4, Item_Data[iDataIndex[iItemControlMaxNumber - 14]]);//ListItem4，设置列表项

                _SetListItem(customListItem5, Item_Data[iDataIndex[iItemControlMaxNumber - 13]]);//ListItem5，设置列表项

                _SetListItem(customListItem6, Item_Data[iDataIndex[iItemControlMaxNumber - 12]]);//ListItem6，设置列表项

                _SetListItem(customListItem7, Item_Data[iDataIndex[iItemControlMaxNumber - 11]]);//ListItem7，设置列表项

                //

                _ClearListItem(customListItem8);//ListItem8，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem9);//ListItem9，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem10);//ListItem10，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem11);//ListItem11，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem12);//ListItem12，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem13);//ListItem13，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem14);//ListItem14，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem15);//ListItem15，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem16);//ListItem16，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem17);//ListItem17，清除控件显示的文本，并置为未选中状态
            }
            else if (iItemControlMaxNumber - 11 == iItemNumber)//包含6个列表项
            {
                _SetListItem(customListItem1, Item_Data[iDataIndex[iItemControlMaxNumber - 17]]);//ListItem1，设置列表项

                _SetListItem(customListItem2, Item_Data[iDataIndex[iItemControlMaxNumber - 16]]);//ListItem2，设置列表项

                _SetListItem(customListItem3, Item_Data[iDataIndex[iItemControlMaxNumber - 15]]);//ListItem3，设置列表项

                _SetListItem(customListItem4, Item_Data[iDataIndex[iItemControlMaxNumber - 14]]);//ListItem4，设置列表项

                _SetListItem(customListItem5, Item_Data[iDataIndex[iItemControlMaxNumber - 13]]);//ListItem5，设置列表项

                _SetListItem(customListItem6, Item_Data[iDataIndex[iItemControlMaxNumber - 12]]);//ListItem6，设置列表项

                //

                _ClearListItem(customListItem7);//ListItem7，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem8);//ListItem8，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem9);//ListItem9，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem10);//ListItem10，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem11);//ListItem11，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem12);//ListItem12，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem13);//ListItem13，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem14);//ListItem14，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem15);//ListItem15，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem16);//ListItem16，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem17);//ListItem17，清除控件显示的文本，并置为未选中状态
            }
            else if (iItemControlMaxNumber - 12 == iItemNumber)//包含5个列表项
            {
                _SetListItem(customListItem1, Item_Data[iDataIndex[iItemControlMaxNumber - 17]]);//ListItem1，设置列表项

                _SetListItem(customListItem2, Item_Data[iDataIndex[iItemControlMaxNumber - 16]]);//ListItem2，设置列表项

                _SetListItem(customListItem3, Item_Data[iDataIndex[iItemControlMaxNumber - 15]]);//ListItem3，设置列表项

                _SetListItem(customListItem4, Item_Data[iDataIndex[iItemControlMaxNumber - 14]]);//ListItem4，设置列表项

                _SetListItem(customListItem5, Item_Data[iDataIndex[iItemControlMaxNumber - 13]]);//ListItem5，设置列表项

                //

                _ClearListItem(customListItem6);//ListItem6，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem7);//ListItem7，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem8);//ListItem8，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem9);//ListItem9，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem10);//ListItem10，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem11);//ListItem11，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem12);//ListItem12，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem13);//ListItem13，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem14);//ListItem14，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem15);//ListItem15，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem16);//ListItem16，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem17);//ListItem17，清除控件显示的文本，并置为未选中状态
            }
            else if (iItemControlMaxNumber - 13 == iItemNumber)//包含4个列表项
            {
                _SetListItem(customListItem1, Item_Data[iDataIndex[iItemControlMaxNumber - 17]]);//ListItem1，设置列表项

                _SetListItem(customListItem2, Item_Data[iDataIndex[iItemControlMaxNumber - 16]]);//ListItem2，设置列表项

                _SetListItem(customListItem3, Item_Data[iDataIndex[iItemControlMaxNumber - 15]]);//ListItem3，设置列表项

                _SetListItem(customListItem4, Item_Data[iDataIndex[iItemControlMaxNumber - 14]]);//ListItem4，设置列表项

                //

                _ClearListItem(customListItem5);//ListItem5，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem6);//ListItem6，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem7);//ListItem7，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem8);//ListItem8，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem9);//ListItem9，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem10);//ListItem10，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem11);//ListItem11，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem12);//ListItem12，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem13);//ListItem13，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem14);//ListItem14，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem15);//ListItem15，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem16);//ListItem16，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem17);//ListItem17，清除控件显示的文本，并置为未选中状态
            }
            else if (iItemControlMaxNumber - 14 == iItemNumber)//包含3个列表项
            {
                _SetListItem(customListItem1, Item_Data[iDataIndex[iItemControlMaxNumber - 17]]);//ListItem1，设置列表项

                _SetListItem(customListItem2, Item_Data[iDataIndex[iItemControlMaxNumber - 16]]);//ListItem2，设置列表项

                _SetListItem(customListItem3, Item_Data[iDataIndex[iItemControlMaxNumber - 15]]);//ListItem3，设置列表项

                //

                _ClearListItem(customListItem4);//ListItem4，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem5);//ListItem5，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem6);//ListItem6，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem7);//ListItem7，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem8);//ListItem8，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem9);//ListItem9，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem10);//ListItem10，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem11);//ListItem11，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem12);//ListItem12，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem13);//ListItem13，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem14);//ListItem14，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem15);//ListItem15，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem16);//ListItem16，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem17);//ListItem17，清除控件显示的文本，并置为未选中状态
            }
            else if (iItemControlMaxNumber - 15 == iItemNumber)//包含2个列表项
            {
                _SetListItem(customListItem1, Item_Data[iDataIndex[iItemControlMaxNumber - 17]]);//ListItem1，设置列表项

                _SetListItem(customListItem2, Item_Data[iDataIndex[iItemControlMaxNumber - 16]]);//ListItem2，设置列表项

                //

                _ClearListItem(customListItem3);//ListItem3，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem4);//ListItem4，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem5);//ListItem5，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem6);//ListItem6，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem7);//ListItem7，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem8);//ListItem8，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem9);//ListItem9，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem10);//ListItem10，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem11);//ListItem11，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem12);//ListItem12，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem13);//ListItem13，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem14);//ListItem14，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem15);//ListItem15，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem16);//ListItem16，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem17);//ListItem17，清除控件显示的文本，并置为未选中状态
            }
            else//iItemControlMaxNumber - 16 == iItemNumber，包含1个列表项
            {
                _SetListItem(customListItem1, Item_Data[iDataIndex[iItemControlMaxNumber - 17]]);//ListItem1，设置列表项

                //

                _ClearListItem(customListItem2);//ListItem2，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem3);//ListItem3，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem4);//ListItem4，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem5);//ListItem5，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem6);//ListItem6，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem7);//ListItem7，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem8);//ListItem8，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem9);//ListItem9，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem10);//ListItem10，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem11);//ListItem11，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem12);//ListItem12，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem13);//ListItem13，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem14);//ListItem14，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem15);//ListItem15，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem16);//ListItem16，清除控件显示的文本，并置为未选中状态
                _ClearListItem(customListItem17);//ListItem17，清除控件显示的文本，并置为未选中状态
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：拖动列表头控件垂直拆分条或单击某一区域，使列表头控件各个区域发生改变时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customListHeader_DragControl(object sender, EventArgs e)
        {
            CustomListHeader_EventArgs EventArgs = (CustomListHeader_EventArgs)e;

            customListItem1._Set(EventArgs.RectReal);//更新列表项1区域
            customListItem2._Set(EventArgs.RectReal);//更新列表项2区域
            customListItem3._Set(EventArgs.RectReal);//更新列表项3区域
            customListItem4._Set(EventArgs.RectReal);//更新列表项4区域
            customListItem5._Set(EventArgs.RectReal);//更新列表项5区域
            customListItem6._Set(EventArgs.RectReal);//更新列表项6区域
            customListItem7._Set(EventArgs.RectReal);//更新列表项7区域
            customListItem8._Set(EventArgs.RectReal);//更新列表项8区域
            customListItem9._Set(EventArgs.RectReal);//更新列表项9区域
            customListItem10._Set(EventArgs.RectReal);//更新列表项10区域
            customListItem11._Set(EventArgs.RectReal);//更新列表项11区域
            customListItem12._Set(EventArgs.RectReal);//更新列表项12区域
            customListItem13._Set(EventArgs.RectReal);//更新列表项13区域
            customListItem14._Set(EventArgs.RectReal);//更新列表项14区域
            customListItem15._Set(EventArgs.RectReal);//更新列表项15区域
            customListItem16._Set(EventArgs.RectReal);//更新列表项16区域
            customListItem17._Set(EventArgs.RectReal);//更新列表项17区域
        }

        //----------------------------------------------------------------------
        // 功能说明：点击customListItem1控件时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customListItem1_Click(object sender, EventArgs e)
        {
            _ClickListItem(customListItem1, 0);//点击列表项1
        }

        //----------------------------------------------------------------------
        // 功能说明：双击customListItem1控件时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customListItem1_DoubleClickListItem(object sender, EventArgs e)
        {
            _DoubleClickListItem(customListItem1, 0);//点击列表项1
        }

        //----------------------------------------------------------------------
        // 功能说明：点击customListItem2控件时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customListItem2_Click(object sender, EventArgs e)
        {
            _ClickListItem(customListItem2, 1);//点击列表项2
        }

        //----------------------------------------------------------------------
        // 功能说明：双击customListItem2控件时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customListItem2_DoubleClickListItem(object sender, EventArgs e)
        {
            _DoubleClickListItem(customListItem2, 1);//点击列表项2
        }

        //----------------------------------------------------------------------
        // 功能说明：点击customListItem3控件时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customListItem3_Click(object sender, EventArgs e)
        {
            _ClickListItem(customListItem3, 2);//点击列表项3
        }

        //----------------------------------------------------------------------
        // 功能说明：双击customListItem3控件时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customListItem3_DoubleClickListItem(object sender, EventArgs e)
        {
            _DoubleClickListItem(customListItem3, 2);//点击列表项3
        }

        //----------------------------------------------------------------------
        // 功能说明：点击customListItem4控件时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customListItem4_Click(object sender, EventArgs e)
        {
            _ClickListItem(customListItem4, 3);//点击列表项4
        }

        //----------------------------------------------------------------------
        // 功能说明：双击customListItem4控件时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customListItem4_DoubleClickListItem(object sender, EventArgs e)
        {
            _DoubleClickListItem(customListItem4, 3);//点击列表项4
        }

        //----------------------------------------------------------------------
        // 功能说明：点击customListItem5控件时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customListItem5_Click(object sender, EventArgs e)
        {
            _ClickListItem(customListItem5, 4);//点击列表项5
        }

        //----------------------------------------------------------------------
        // 功能说明：双击customListItem5控件时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customListItem5_DoubleClickListItem(object sender, EventArgs e)
        {
            _DoubleClickListItem(customListItem5, 4);//点击列表项5
        }

        //----------------------------------------------------------------------
        // 功能说明：点击customListItem6控件时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customListItem6_Click(object sender, EventArgs e)
        {
            _ClickListItem(customListItem6, 5);//点击列表项6
        }

        //----------------------------------------------------------------------
        // 功能说明：双击customListItem6控件时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customListItem6_DoubleClickListItem(object sender, EventArgs e)
        {
            _DoubleClickListItem(customListItem6, 5);//点击列表项6
        }

        //----------------------------------------------------------------------
        // 功能说明：点击customListItem7控件时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customListItem7_Click(object sender, EventArgs e)
        {
            _ClickListItem(customListItem7, 6);//点击列表项7
        }

        //----------------------------------------------------------------------
        // 功能说明：双击customListItem7控件时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customListItem7_DoubleClickListItem(object sender, EventArgs e)
        {
            _DoubleClickListItem(customListItem7, 6);//点击列表项7
        }

        //----------------------------------------------------------------------
        // 功能说明：点击customListItem8控件时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customListItem8_Click(object sender, EventArgs e)
        {
            _ClickListItem(customListItem8, 7);//点击列表项8
        }

        //----------------------------------------------------------------------
        // 功能说明：双击customListItem8控件时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customListItem8_DoubleClickListItem(object sender, EventArgs e)
        {
            _DoubleClickListItem(customListItem8, 7);//点击列表项8
        }

        //----------------------------------------------------------------------
        // 功能说明：点击customListItem9控件时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customListItem9_Click(object sender, EventArgs e)
        {
            _ClickListItem(customListItem9, 8);//点击列表项9
        }

        //----------------------------------------------------------------------
        // 功能说明：双击customListItem9控件时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customListItem9_DoubleClickListItem(object sender, EventArgs e)
        {
            _DoubleClickListItem(customListItem9, 8);//点击列表项9
        }

        //----------------------------------------------------------------------
        // 功能说明：点击customListItem10控件时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customListItem10_Click(object sender, EventArgs e)
        {
            _ClickListItem(customListItem10, 9);//点击列表项10
        }

        //----------------------------------------------------------------------
        // 功能说明：双击customListItem10控件时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customListItem10_DoubleClickListItem(object sender, EventArgs e)
        {
            _DoubleClickListItem(customListItem10, 9);//点击列表项10
        }

        //----------------------------------------------------------------------
        // 功能说明：点击customListItem11控件时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customListItem11_Click(object sender, EventArgs e)
        {
            _ClickListItem(customListItem11, 10);//点击列表项11
        }

        //----------------------------------------------------------------------
        // 功能说明：双击customListItem11控件时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customListItem11_DoubleClickListItem(object sender, EventArgs e)
        {
            _DoubleClickListItem(customListItem11, 10);//点击列表项11
        }

        //----------------------------------------------------------------------
        // 功能说明：点击customListItem12控件时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customListItem12_Click(object sender, EventArgs e)
        {
            _ClickListItem(customListItem12, 11);//点击列表项12
        }

        //----------------------------------------------------------------------
        // 功能说明：双击customListItem12控件时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customListItem12_DoubleClickListItem(object sender, EventArgs e)
        {
            _DoubleClickListItem(customListItem12, 11);//点击列表项12
        }

        //----------------------------------------------------------------------
        // 功能说明：点击customListItem13控件时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customListItem13_Click(object sender, EventArgs e)
        {
            _ClickListItem(customListItem13, 12);//点击列表项13
        }

        //----------------------------------------------------------------------
        // 功能说明：双击customListItem13控件时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customListItem13_DoubleClickListItem(object sender, EventArgs e)
        {
            _DoubleClickListItem(customListItem13, 12);//点击列表项13
        }

        //----------------------------------------------------------------------
        // 功能说明：点击customListItem14控件时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customListItem14_Click(object sender, EventArgs e)
        {
            _ClickListItem(customListItem14, 13);//点击列表项14
        }

        //----------------------------------------------------------------------
        // 功能说明：双击customListItem14控件时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customListItem14_DoubleClickListItem(object sender, EventArgs e)
        {
            _DoubleClickListItem(customListItem14, 13);//点击列表项14
        }

        //----------------------------------------------------------------------
        // 功能说明：点击customListItem15控件时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customListItem15_Click(object sender, EventArgs e)
        {
            _ClickListItem(customListItem15, 14);//点击列表项15
        }

        //----------------------------------------------------------------------
        // 功能说明：双击customListItem15控件时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customListItem15_DoubleClickListItem(object sender, EventArgs e)
        {
            _DoubleClickListItem(customListItem15, 14);//点击列表项15
        }

        //----------------------------------------------------------------------
        // 功能说明：点击customListItem16控件时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customListItem16_Click(object sender, EventArgs e)
        {
            _ClickListItem(customListItem16, 15);//点击列表项16
        }

        //----------------------------------------------------------------------
        // 功能说明：双击customListItem16控件时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customListItem16_DoubleClickListItem(object sender, EventArgs e)
        {
            _DoubleClickListItem(customListItem16, 15);//点击列表项16
        }

        //----------------------------------------------------------------------
        // 功能说明：点击customListItem17控件时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customListItem17_Click(object sender, EventArgs e)
        {
            _ClickListItem(customListItem17, 16);//点击列表项17
        }

        //----------------------------------------------------------------------
        // 功能说明：双击customListItem17控件时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customListItem17_DoubleClickListItem(object sender, EventArgs e)
        {
            _DoubleClickListItem(customListItem17, 16);//点击列表项17
        }
    }
}