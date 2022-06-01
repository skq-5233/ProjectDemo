/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：CustomProgressBar.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：自定义进度条

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

namespace VisionSystemControlLibrary
{
    public partial class CustomProgressBar : UserControl
    {
        //进度条控件

        //1.Minimum，Maximum属性可以使用默认值而不用进行设置
        //2.设置Value数值即可以设置进度条数值
        //3.设置StepNumber后，使用PerformStep()函数设置进度条数值

        private double iMinimum = 0;//属性，控件范围的最小值
        private double iMaximum = 100;//属性，控件范围的最大值

        private double iValue = 0;//属性，控件当前值

        private double iStepNumber = 10;//属性，控件步进数量
        private double iStep = 10;//控件步进值
        private double iCurrentStep = 1;//控件当前步进
        
        //构造函数

        //-----------------------------------------------------------------------
        // 功能说明：构造函数，初始化
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public CustomProgressBar()
        {
            InitializeComponent();

            //

            Value = 0;//初始化
        }

        //属性

        //----------------------------------------------------------------------
        // 功能说明：Minimum属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("控件范围的最小值"), Category("ProgressBar 控件")]
        public double Minimum
        {
            get//读取
            {
                return iMinimum;
            }
            set//设置
            {
                if (value != iMinimum)
                {
                    if (value < iMaximum)//有效
                    {
                        iMinimum = value;
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：Maximum属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("控件范围的最大值"), Category("ProgressBar 控件")]
        public double Maximum
        {
            get//读取
            {
                return iMaximum;
            }
            set//设置
            {
                if (value != iMaximum)
                {
                    if (value > iMinimum)//有效
                    {
                        iMaximum = value;
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：Value属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("控件当前值"), Category("ProgressBar 控件")]
        public double Value
        {
            get//读取
            {
                return iValue;
            }
            set//设置
            {
                if (iValue >= iMinimum && iValue <= iMaximum)//有效
                {
                    iValue = value;

                    //

                    pictureBox.Width = (int)(ClientRectangle.Width * (iValue / (iMaximum - iMinimum)));//设置进度条控件
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：StepNumber属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("控件步进数量"), Category("ProgressBar 控件")]
        public double StepNumber
        {
            get//读取
            {
                return iStepNumber;
            }
            set//设置
            {
                if (0 < iStepNumber)//有效
                {
                    iStepNumber = value;

                    //

                    iStep = (iMaximum - iMinimum) / iStepNumber;
                    iCurrentStep = 1;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：StepColor属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("控件步进数量"), Category("ProgressBar 控件")]
        public Color StepColor
        {
            get//读取
            {
                return pictureBox.BackColor;
            }
            set//设置
            {
                pictureBox.BackColor = value;
            }
        }

        //函数

        //----------------------------------------------------------------------
        // 功能说明：根据设置的步进数值增加进度条的当前位置
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _PerformStep()
        {
            pictureBox.Width = (int)(ClientRectangle.Width * ((iCurrentStep * iStep) / (iMaximum - iMinimum)));//设置进度条控件

            iCurrentStep++;
        }
    }
}
