/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：RejectsSlot.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：REJECTS VIEW，SLOT控件

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
    public partial class RejectsSlot : UserControl
    {
        //该控件为Rejects页面中的Slot控件

        //控件中的区块从左至右对应于bValue数组的[11] ~ [0]

        private const int iTotalNumber = 12;//Slot中包含的区块总数
        private Rectangle rectangleValue = new Rectangle(new Point(2, 2), new Size(5, 13));//左边第一个区块的区域，用于绘制区块
        private const int iWidth = 8;//相邻两个区块间的距离

        private bool bCurrent = false;//属性，是否为当前Slot。true：是；false：否

        private int iPosition = 0;//属性，bSaving取值为false时使用，当前区块的位置（从0开始，取值为-1，表示不存在有效的区块）

        private bool bWorking = false;//属性，是否正在备份或清除图像数据。true：是；false：否
        private bool[] bValue = new bool[iTotalNumber];//属性，bWorking取值为true时使用，区块的数值。取值范围：true，表示保存成功；false，表示保存失败
        private bool[] bValid = new bool[iTotalNumber];//属性，bWorking取值为true时使用，区块是否有效（只有区块有效时，区块数值才有意义）。true：是；false：否

        private Graphics graphicsDisplay;//绘图

        //----------------------------------------------------------------------
        // 功能说明：构造函数，初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public RejectsSlot()
        {
            InitializeComponent();

            //
            
            for (int i = 0; i < iTotalNumber; i++)//初始化
            {
                bValue[i] = false;
                bValid[i] = false;
            }

            graphicsDisplay = CreateGraphics();//获取绘图资源
        }

        //属性

        //----------------------------------------------------------------------
        // 功能说明：Current属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("是否为当前Slot。true：是；false：否"), Category("RejectsSlot 控件")]
        public bool Current//属性
        {
            get//读取
            {
                return bCurrent;
            }
            set//设置
            {
                if (bCurrent != value)//设置了新的数值
                {
                    bCurrent = value;

                    if (bCurrent)//当前Slot
                    {
                        BackgroundImage = VisionSystemControlLibrary.Properties.Resources.RejectsSlot_BackgroundCurrent;//更新控件背景
                    }
                    else//非当前Slot
                    {
                        BackgroundImage = VisionSystemControlLibrary.Properties.Resources.RejectsSlot_Background;//更新控件背景
                    }

                    //

                    _Draw();//绘制
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：Saving属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("是否正在备份或清除图像数据。true：是；false：否"), Category("RejectsSlot 控件")]
        public bool Working//属性
        {
            get//读取
            {
                return bWorking;
            }
            set//设置
            {
                if (bWorking != value)//设置了新的数值
                {
                    bWorking = value;

                    _Draw();//绘制
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：Position属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("bSaving取值为false时使用，当前区块的位置（从0开始，取值为-1，表示不存在有效的区块）"), Category("RejectsSlot 控件")]
        public int Position//属性
        {
            get//读取
            {
                return iPosition;
            }
            set//设置
            {
                if (iPosition != value)//设置了新的数值
                {
                    iPosition = value;

                    _Draw();//绘制
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：Value属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("bWorking取值为true时使用，区块的数值。取值范围：true，表示保存成功；false，表示保存失败"), Category("RejectsSlot 控件")]
        public bool[] Value//属性
        {
            get//读取
            {
                return bValue;
            }
            set//设置
            {
                if (bValue != value)//设置了新的数值
                {
                    bValue = value;

                    if (bWorking)//保存文件
                    {
                        _Draw();//绘制
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：Valid属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("bWorking取值为true时使用，区块是否有效（只有区块有效时，区块数值才有意义）。取值范围：true，是；false，否"), Category("RejectsSlot 控件")]
        public bool[] Valid//属性
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

                    if (bWorking)//保存文件
                    {
                        _Draw();//绘制
                    }
                }
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：更新控件
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _Update()
        {
            _Draw();//绘制
        }

        //

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

            //绘制背景

            if (bCurrent)//当前Slot
            {
                graphicsDraw.DrawImage(Properties.Resources.RejectsSlot_BackgroundCurrent, ClientRectangle);
            }
            else//非当前Slot
            {
                graphicsDraw.DrawImage(Properties.Resources.RejectsSlot_Background, ClientRectangle);
            }

            //绘制区块

            SolidBrush solidbrushDraw = new SolidBrush(Color.FromArgb(255, 255, 255));//绘制区块所使用的画刷

            //当前选择的项

            solidbrushDraw = new SolidBrush(Color.FromArgb(255, 255, 0));//绘制区块所使用的画刷

            if (iPosition >= 0)//有效
            {
                graphicsDraw.FillRectangle(solidbrushDraw, new Rectangle(new Point(rectangleValue.Left + (iTotalNumber - 1 - iPosition) * iWidth, rectangleValue.Top), rectangleValue.Size));//绘制区块
            }

            //绘制保存数据的项

            if (bWorking)//正在保存数据
            {
                for (int i = iTotalNumber - 1; i >= 0; i--)//绘制区块
                {
                    if (bValid[i])//区块有效
                    {
                        if (bValue[i])//保存成功
                        {
                            solidbrushDraw = new SolidBrush(Color.FromArgb(0, 255, 0));//绘制区块所使用的画刷
                        }
                        else//保存失败
                        {
                            solidbrushDraw = new SolidBrush(Color.FromArgb(255, 0, 0));//绘制区块所使用的画刷
                        }

                        graphicsDraw.FillRectangle(solidbrushDraw, new Rectangle(new Point(rectangleValue.Left + (iTotalNumber - 1 - i) * iWidth, rectangleValue.Top), rectangleValue.Size));//绘制区块
                    }
                    else//区块无效
                    {
                        //不执行操作
                    }
                }
            }

            //

            graphicsDisplay.DrawImage(imageDisplay, ClientRectangle);

            graphicsDraw.Dispose();
            imageDisplay.Dispose();
        }

        //----------------------------------------------------------------------
        // 功能说明：绘制控件事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void RejectsSlot_Paint(object sender, PaintEventArgs e)
        {
            _Draw();//绘制控件
        }
    }
}
