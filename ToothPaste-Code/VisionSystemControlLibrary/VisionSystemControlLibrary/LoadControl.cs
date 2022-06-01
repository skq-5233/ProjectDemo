/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：LoadControl.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：启动窗口

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
    public partial class LoadControl : UserControl
    {
        //启动页面控件

        private VisionSystemClassLibrary.Enum.InterfaceLanguage language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;//属性，语言

        //

        private String sVersion = "";//属性，版本

        //

        private String[][] sMessageText = new String[2][];//版本信息文本（[语言][包含的文本]）

        //构造函数

        //----------------------------------------------------------------------
        // 功能说明：系统默认调用，构造函数，初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public LoadControl()
        {
            InitializeComponent();
            
            //

            FieldInfo[] fieldinfo = VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese.GetType().GetFields();//获取公共字段
            if (null != fieldinfo && 1 < fieldinfo.Length)//有效
            {
                Int32 i = 0;//循环控制变量

                sMessageText = new String[fieldinfo.Length - 1][];
                for (i = 0; i < fieldinfo.Length - 1; i++)
                {
                    sMessageText[i] = new String[1];
                }

                //

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] = customButtonVersion.Chinese_TextDisplay[0];
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] = customButtonVersion.English_TextDisplay[0];
            }

            //

            labelDeviceName.Text = "";
        }

        //属性

        //----------------------------------------------------------------------
        // 功能说明：BitmapTrademark属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("商标"), Category("LoadControl 通用")]
        public Bitmap BitmapTrademark//属性
        {
            get//读取
            {
                return customButtonTrademark.BitmapIconWhole;
            }
            set//设置
            {
                customButtonTrademark.BitmapIconWhole = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Language属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("语言"), Category("LoadControl 通用")]
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

                    _SetLanguage();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：DeviceName属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("设备名称"), Category("LoadControl 通用")]
        public string DeviceName//属性
        {
            get//读取
            {
                return labelDeviceName.Text;
            }
            set//设置
            {
                if (value != labelDeviceName.Text)
                {
                    labelDeviceName.Text = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：AppVersion属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("程序版本"), Category("LoadControl 通用")]
        public string AppVersion//属性
        {
            get//读取
            {
                return sVersion;
            }
            set//设置
            {
                if (value != sVersion)
                {
                    sVersion = value;

                    customButtonVersion.Chinese_TextDisplay = new String[1] { sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] + sVersion };//设置显示的文本
                    customButtonVersion.English_TextDisplay = new String[1] { sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] + sVersion };//设置显示的文本
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ProgressBarMinimum属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("进度条控件范围的最小值"), Category("LoadControl 通用")]
        public double ProgressBarMinimum
        {
            get//读取
            {
                return progressBar.Minimum;
            }
            set//设置
            {
                progressBar.Minimum = value;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ProgressBarMaximum属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("进度条控件范围的最大值"), Category("LoadControl 通用")]
        public double ProgressBarMaximum
        {
            get//读取
            {
                return progressBar.Maximum;
            }
            set//设置
            {
                progressBar.Maximum = value;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ProgressBarValue属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("进度条控件当前值"), Category("LoadControl 通用")]
        public double ProgressBarValue
        {
            get//读取
            {
                return progressBar.Value;
            }
            set//设置
            {
                progressBar.Value = value;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ProgressBarStepNumber属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("进度条控件步进数量"), Category("LoadControl 通用")]
        public double ProgressBarStepNumber
        {
            get//读取
            {
                return progressBar.StepNumber;
            }
            set//设置
            {
                progressBar.StepNumber = value;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：MessageTextIndex属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("加载信息"), Category("LoadControl 通用")]
        public Int32 MessageTextIndex
        {
            get//读取
            {
                return customButtonMessage.CurrentTextGroupIndex;
            }
            set//设置
            {
                customButtonMessage.CurrentTextGroupIndex = value;
            }
        }

        //函数

        //----------------------------------------------------------------------
        // 功能说明：根据设置的步进数值增加进度条的当前位置
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _ProgressBar_PerformStep()
        {
            progressBar._PerformStep();
        }

        //----------------------------------------------------------------------
        // 功能说明：设置语言
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetLanguage()
        {
            switch (language)
            {
                case VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese://中文

                    customButtonTrademark.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Down;//商标控件

                    break;
                case VisionSystemClassLibrary.Enum.InterfaceLanguage.English://英文

                    customButtonTrademark.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//商标控件

                    break;
                default:
                    break;
            }

            //

            customButtonMessage.Language = language;

            customButtonVersion.Language = language;
            customButtonVersion.Chinese_TextDisplay = new String[1] { sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] + sVersion };//设置显示的文本
            customButtonVersion.English_TextDisplay = new String[1] { sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] + sVersion };//设置显示的文本
        }

        //事件

        //----------------------------------------------------------------------
        // 功能说明：控件加载事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void LoadControl_Load(object sender, EventArgs e)
        {
            //不执行操作
        }
    }
}