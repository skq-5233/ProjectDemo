/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：CustomMeter.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：METER控件

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
    //自定义按钮绘制文本的方式

    public enum MeterLamp
    {
        Invalid = 4,//Invalid
        Disconnected = 3,//Disconnected
        Run = 0,//Run
        Stop = 1,//Stop
        Warning = 2,//Warning
    }

    public partial class CustomMeter : UserControl
    {
        //该控件为METER控件

        private VisionSystemClassLibrary.Enum.InterfaceLanguage language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;//属性，语言

        private MeterLamp meterLamp = MeterLamp.Run;//属性，指示器状态

        private Object meterData = null;//属性，预留数据

        //

        [Browsable(true), Description("点击NAME按钮时产生的事件"), Category("CustomButton 事件")]
        public event EventHandler CustomButtonName_Click;//点击按钮时产生的事件

        [Browsable(true), Description("点击VALUE按钮时产生的事件"), Category("CustomButton 事件")]
        public event EventHandler CustomButtonValue_Click;//点击按钮时产生的事件

        //构造函数

        //----------------------------------------------------------------------
        // 功能说明：构造函数，初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public CustomMeter()
        {
            InitializeComponent();
        }

        //属性

        //-----------------------------------------------------------------------
        // 功能说明：Language属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("语言"), Category("CustomMeter 通用")]
        public VisionSystemClassLibrary.Enum.InterfaceLanguage Language
        {
            get//读取
            {
                return language;
            }
            set//设置
            {
                language = value;

                //

                customButtonName.Language = language;
                customButtonValue.Language = language;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：CustomMeterData属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("指示器状态"), Category("CustomMeter 通用")]
        public MeterLamp MeterLampStatus
        {
            get//读取
            {
                return meterLamp;
            }
            set//设置
            {
                meterLamp = value;

                //

                if (MeterLamp.Invalid == value)//Invalid
                {
                    customButtonLamp.DrawIcon = false;
                } 
                else//其它
                {
                    if (!customButtonLamp.DrawIcon)
                    {
                        customButtonLamp.DrawIcon = true;
                    }

                    customButtonLamp.IconIndex[2] = (Int32)meterLamp;
                }

                customButtonLamp._Refresh();
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：CustomMeterData属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(false), Description("预留数据"), Category("CustomMeter 通用")]
        public Object CustomMeterData
        {
            get//读取
            {
                return meterData;
            }
            set//设置
            {
                meterData = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：SizeMeter属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("控件大小"), Category("CustomMeter 通用")]
        public Size SizeMeter
        {
            get//读取
            {
                return this.Size;
            }
            set//设置
            {
                this.Size = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：SizeMeter_Name属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("仪表名称控件大小"), Category("CustomMeter 通用")]
        public Size SizeMeter_Name
        {
            get//读取
            {
                return customButtonName.Size;
            }
            set//设置
            {
                customButtonName.SizeButton = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：SizeMeter_Lamp属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("仪表指示器控件大小"), Category("CustomMeter 通用")]
        public Size SizeMeter_Lamp
        {
            get//读取
            {
                return customButtonLamp.Size;
            }
            set//设置
            {
                customButtonLamp.SizeButton = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：SizeMeter_Value属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("仪表数值控件大小"), Category("CustomMeter 通用")]
        public Size SizeMeter_Value
        {
            get//读取
            {
                return customButtonValue.Size;
            }
            set//设置
            {
                customButtonValue.SizeButton = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：SizeMeter_Name属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("仪表名称控件模式"), Category("CustomMeter 通用")]
        public Boolean MeterName_LabelControlMode
        {
            get//读取
            {
                return customButtonName.LabelControlMode;
            }
            set//设置
            {
                customButtonName.LabelControlMode = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：SizeMeter_Value属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("仪表数值控件模式"), Category("CustomMeter 通用")]
        public Boolean MeterValue_LabelControlMode
        {
            get//读取
            {
                return customButtonValue.LabelControlMode;
            }
            set//设置
            {
                customButtonValue.LabelControlMode = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：MeterName_CHN属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("仪表中文名称"), Category("CustomMeter 通用")]
        public String[] MeterName_CHN
        {
            get//读取
            {
                return customButtonName.Chinese_TextDisplay;
            }
            set//设置
            {
                customButtonName.Chinese_TextDisplay = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：MeterName_ENG属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("仪表英文名称"), Category("CustomMeter 通用")]
        public String[] MeterName_ENG
        {
            get//读取
            {
                return customButtonName.English_TextDisplay;
            }
            set//设置
            {
                customButtonName.English_TextDisplay = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：MeterValue_CHN属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("仪表数值中文名称"), Category("CustomMeter 通用")]
        public String[] MeterValue_CHN
        {
            get//读取
            {
                return customButtonValue.Chinese_TextDisplay;
            }
            set//设置
            {
                customButtonValue.Chinese_TextDisplay = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：MeterValue_ENG属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("仪表数值英文名称"), Category("CustomMeter 通用")]
        public String[] MeterValue_ENG
        {
            get//读取
            {
                return customButtonValue.English_TextDisplay;
            }
            set//设置
            {
                customButtonValue.English_TextDisplay = value;
            }
        }

        //控件事件

        //----------------------------------------------------------------------
        // 功能说明：点击NAME按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonName_CustomButton_Click(object sender, EventArgs e)
        {
            //事件

            if (null != CustomButtonName_Click)//有效
            {
                CustomButtonName_Click(this, new CustomEventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击VALUE按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonValue_CustomButton_Click(object sender, EventArgs e)
        {
            //事件

            if (null != CustomButtonValue_Click)//有效
            {
                CustomButtonValue_Click(this, new CustomEventArgs());
            }
        }
    }
}
