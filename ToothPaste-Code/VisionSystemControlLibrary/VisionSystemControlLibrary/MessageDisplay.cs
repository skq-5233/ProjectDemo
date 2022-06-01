/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：MessageDisplay.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：提示信息控件

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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Threading;

using System.Diagnostics;

using System.IO;

using System.Runtime.InteropServices;

namespace VisionSystemControlLibrary
{
    public partial class MessageDisplay : UserControl
    {
        //提示信息控件

        private VisionSystemClassLibrary.Enum.InterfaceLanguage language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;//属性，语言

        private VisionSystemClassLibrary.Enum.MessageDisplayType controlType = VisionSystemClassLibrary.Enum.MessageDisplayType.OkCancel;//属性，控件显示类型

        //

        private Point pointOkButton_OkCancel = new Point(126, 228);//显示【OK】【Cancel】按钮时，其位置
        private Point pointCancelButton_OkCancel = new Point(265, 228);//显示【OK】【Cancel】按钮时，其位置

        private Point pointOkButton_Ok = new Point(193, 228);//属性，只显示【OK】按钮时，其位置

        //

        private Boolean bOkCancel = false;//属性（只读），点击【OK】或【Cance】按钮。取值范围：true，【OK】（或，不显示按钮时的控件区域）；false，【Cance】

        //

        [Browsable(true), Description("窗口关闭时产生的事件"), Category("MessageDisplay 事件")]
        public event EventHandler Close_Click;//窗口关闭时产生的事件

        //

        //-----------------------------------------------------------------------
        // 功能说明：构造函数
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public MessageDisplay()
        {
            InitializeComponent();

            //

            pointOkButton_OkCancel = customButtonOk.Location;//显示【OK】【Cancel】按钮时，其位置
            pointCancelButton_OkCancel = customButtonCancel.Location;//显示【OK】【Cancel】按钮时，其位置
        }

        //属性

        //-----------------------------------------------------------------------
        // 功能说明：ControlType属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("控件显示类型"), Category("MessageDisplay 通用")]
        public VisionSystemClassLibrary.Enum.MessageDisplayType ControlType
        {
            get//读取
            {
                return controlType;
            }
            set//设置
            {
                if (value != controlType)
                {
                    controlType = value;

                    //

                    _SetType();//设置控件类型
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：LocationOkButton_Ok属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("只显示【OK】按钮时，其位置"), Category("MessageDisplay 通用")]
        public Point LocationOkButton_Ok
        {
            get//读取
            {
                return pointOkButton_Ok;
            }
            set//设置
            {
                if (value != pointOkButton_Ok)
                {
                    pointOkButton_Ok = value;

                    //

                    _SetType();//设置控件类型
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Language属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("语言"), Category("MessageDisplay 通用")]
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

        //-----------------------------------------------------------------------
        // 功能说明：OkCancel属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("点击【OK】或【Cance】按钮。取值范围：true，【OK】（或，不显示按钮时的控件区域）；false，【Cance】"), Category("MessageDisplay 通用")]
        public Boolean OkCancel
        {
            get//读取
            {
                return bOkCancel;
            }
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：Chinese_Message_1属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("第1行显示的中文文本"), Category("MessageDisplay 通用")]
        public String Chinese_Message_1
        {
            get//读取
            {
                return customButtonMessage_1.Chinese_TextDisplay[0];
            }
            set//设置
            {
                if ("" != value && value != customButtonMessage_1.Chinese_TextDisplay[0])//有效
                {
                    customButtonMessage_1.Chinese_TextDisplay = new String[1] { value };//设置显示的文本
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：English_Message_1属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("第1行显示的英文文本"), Category("MessageDisplay 通用")]
        public String English_Message_1
        {
            get//读取
            {
                return customButtonMessage_1.English_TextDisplay[0];
            }
            set//设置
            {
                if ("" != value && value != customButtonMessage_1.English_TextDisplay[0])//有效
                {
                    customButtonMessage_1.English_TextDisplay = new String[1] { value };//设置显示的文本
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Chinese_Message_2属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("第2行显示的中文文本"), Category("MessageDisplay 通用")]
        public String Chinese_Message_2
        {
            get//读取
            {
                return customButtonMessage_2.Chinese_TextDisplay[0];
            }
            set//设置
            {
                if ("" != value && value != customButtonMessage_2.Chinese_TextDisplay[0])//有效
                {
                    customButtonMessage_2.Chinese_TextDisplay = new String[1] { value };//设置显示的文本
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：English_Message_2属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("第2行显示的英文文本"), Category("MessageDisplay 通用")]
        public String English_Message_2
        {
            get//读取
            {
                return customButtonMessage_2.English_TextDisplay[0];
            }
            set//设置
            {
                if ("" != value && value != customButtonMessage_2.English_TextDisplay[0])//有效
                {
                    customButtonMessage_2.English_TextDisplay = new String[1] { value };//设置显示的文本
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Chinese_Message_3属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("第3行显示的中文文本"), Category("MessageDisplay 通用")]
        public String Chinese_Message_3
        {
            get//读取
            {
                return customButtonMessage_3.Chinese_TextDisplay[0];
            }
            set//设置
            {
                if ("" != value && value != customButtonMessage_3.Chinese_TextDisplay[0])//有效
                {
                    customButtonMessage_3.Chinese_TextDisplay = new String[1] { value };//设置显示的文本
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：English_Message_3属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("第3行显示的英文文本"), Category("MessageDisplay 通用")]
        public String English_Message_3
        {
            get//读取
            {
                return customButtonMessage_3.English_TextDisplay[0];
            }
            set//设置
            {
                if ("" != value && value != customButtonMessage_3.English_TextDisplay[0])//有效
                {
                    customButtonMessage_3.English_TextDisplay = new String[1] { value };//设置显示的文本
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Chinese_Message_4属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("第4行显示的中文文本"), Category("MessageDisplay 通用")]
        public String Chinese_Message_4
        {
            get//读取
            {
                return customButtonMessage_4.Chinese_TextDisplay[0];
            }
            set//设置
            {
                if ("" != value && value != customButtonMessage_4.Chinese_TextDisplay[0])//有效
                {
                    customButtonMessage_4.Chinese_TextDisplay = new String[1] { value };//设置显示的文本
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：English_Message_4属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("第4行显示的英文文本"), Category("MessageDisplay 通用")]
        public String English_Message_4
        {
            get//读取
            {
                return customButtonMessage_4.English_TextDisplay[0];
            }
            set//设置
            {
                if ("" != value && value != customButtonMessage_4.English_TextDisplay[0])//有效
                {
                    customButtonMessage_4.English_TextDisplay = new String[1] { value };//设置显示的文本
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Chinese_Message_5属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("第5行显示的中文文本"), Category("MessageDisplay 通用")]
        public String Chinese_Message_5
        {
            get//读取
            {
                return customButtonMessage_5.Chinese_TextDisplay[0];
            }
            set//设置
            {
                if ("" != value && value != customButtonMessage_5.Chinese_TextDisplay[0])//有效
                {
                    customButtonMessage_5.Chinese_TextDisplay = new String[1] { value };//设置显示的文本
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：English_Message_5属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("第5行显示的英文文本"), Category("MessageDisplay 通用")]
        public String English_Message_5
        {
            get//读取
            {
                return customButtonMessage_5.English_TextDisplay[0];
            }
            set//设置
            {
                if ("" != value && value != customButtonMessage_5.English_TextDisplay[0])//有效
                {
                    customButtonMessage_5.English_TextDisplay = new String[1] { value };//设置显示的文本
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Chinese_Message_6属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("第6行显示的中文文本"), Category("MessageDisplay 通用")]
        public String Chinese_Message_6
        {
            get//读取
            {
                return customButtonMessage_6.Chinese_TextDisplay[0];
            }
            set//设置
            {
                if ("" != value && value != customButtonMessage_6.Chinese_TextDisplay[0])//有效
                {
                    customButtonMessage_6.Chinese_TextDisplay = new String[1] { value };//设置显示的文本
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：English_Message_6属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("第6行显示的英文文本"), Category("MessageDisplay 通用")]
        public String English_Message_6
        {
            get//读取
            {
                return customButtonMessage_6.English_TextDisplay[0];
            }
            set//设置
            {
                if ("" != value && value != customButtonMessage_6.English_TextDisplay[0])//有效
                {
                    customButtonMessage_6.English_TextDisplay = new String[1] { value };//设置显示的文本
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Chinese_Message_7属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("第7行显示的中文文本"), Category("MessageDisplay 通用")]
        public String Chinese_Message_7
        {
            get//读取
            {
                return customButtonMessage_7.Chinese_TextDisplay[0];
            }
            set//设置
            {
                if ("" != value && value != customButtonMessage_7.Chinese_TextDisplay[0])//有效
                {
                    customButtonMessage_7.Chinese_TextDisplay = new String[1] { value };//设置显示的文本
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：English_Message_7属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("第7行显示的英文文本"), Category("MessageDisplay 通用")]
        public String English_Message_7
        {
            get//读取
            {
                return customButtonMessage_7.English_TextDisplay[0];
            }
            set//设置
            {
                if ("" != value && value != customButtonMessage_7.English_TextDisplay[0])//有效
                {
                    customButtonMessage_7.English_TextDisplay = new String[1] { value };//设置显示的文本
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Chinese_Message_8属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("第8行显示的中文文本"), Category("MessageDisplay 通用")]
        public String Chinese_Message_8
        {
            get//读取
            {
                return customButtonMessage_8.Chinese_TextDisplay[0];
            }
            set//设置
            {
                if ("" != value && value != customButtonMessage_8.Chinese_TextDisplay[0])//有效
                {
                    customButtonMessage_8.Chinese_TextDisplay = new String[1] { value };//设置显示的文本
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：English_Message_8属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("第8行显示的英文文本"), Category("MessageDisplay 通用")]
        public String English_Message_8
        {
            get//读取
            {
                return customButtonMessage_8.English_TextDisplay[0];
            }
            set//设置
            {
                if ("" != value && value != customButtonMessage_8.English_TextDisplay[0])//有效
                {
                    customButtonMessage_8.English_TextDisplay = new String[1] { value };//设置显示的文本
                }
            }
        }

        //函数

        //----------------------------------------------------------------------
        // 功能说明：设置语言
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetLanguage()
        {
            customButtonMessage_1.Language = language;//第1行文本
            customButtonMessage_2.Language = language;//第2行文本
            customButtonMessage_3.Language = language;//第3行文本
            customButtonMessage_4.Language = language;//第4行文本
            customButtonMessage_5.Language = language;//第5行文本
            customButtonMessage_6.Language = language;//第6行文本
            customButtonMessage_7.Language = language;//第7行文本
            customButtonMessage_8.Language = language;//第8行文本
        }

        //-----------------------------------------------------------------------
        // 功能说明：恢复控件内容
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        public void _Reset()
        {
            customButtonMessage_1.Chinese_TextDisplay = new String[1] { "" };//设置显示的文本
            customButtonMessage_1.English_TextDisplay = new String[1] { "" };//设置显示的文本

            customButtonMessage_2.Chinese_TextDisplay = new String[1] { "" };//设置显示的文本
            customButtonMessage_2.English_TextDisplay = new String[1] { "" };//设置显示的文本

            customButtonMessage_3.Chinese_TextDisplay = new String[1] { "" };//设置显示的文本
            customButtonMessage_3.English_TextDisplay = new String[1] { "" };//设置显示的文本

            customButtonMessage_4.Chinese_TextDisplay = new String[1] { "" };//设置显示的文本
            customButtonMessage_4.English_TextDisplay = new String[1] { "" };//设置显示的文本

            customButtonMessage_5.Chinese_TextDisplay = new String[1] { "" };//设置显示的文本
            customButtonMessage_5.English_TextDisplay = new String[1] { "" };//设置显示的文本

            customButtonMessage_6.Chinese_TextDisplay = new String[1] { "" };//设置显示的文本
            customButtonMessage_6.English_TextDisplay = new String[1] { "" };//设置显示的文本

            customButtonMessage_7.Chinese_TextDisplay = new String[1] { "" };//设置显示的文本
            customButtonMessage_7.English_TextDisplay = new String[1] { "" };//设置显示的文本

            customButtonMessage_8.Chinese_TextDisplay = new String[1] { "" };//设置显示的文本
            customButtonMessage_8.English_TextDisplay = new String[1] { "" };//设置显示的文本
        }

        //-----------------------------------------------------------------------
        // 功能说明：设置控件类型
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _SetType()
        {
            switch (controlType)
            {
                case VisionSystemClassLibrary.Enum.MessageDisplayType.OkCancel://显示【OK】和【Cancel】按钮
                    //
                    customButtonOk.Visible = true;
                    customButtonCancel.Visible = true;

                    customButtonOk.Location = pointOkButton_OkCancel;
                    customButtonCancel.Location = pointCancelButton_OkCancel;
                    //
                    break;
                case VisionSystemClassLibrary.Enum.MessageDisplayType.Ok://显示【OK】按钮
                    //
                    customButtonOk.Visible = true;
                    customButtonCancel.Visible = false;

                    customButtonOk.Location = pointOkButton_Ok;
                    //
                    break;
                case VisionSystemClassLibrary.Enum.MessageDisplayType.Cancel://显示【CANCEL】按钮
                    //
                    customButtonOk.Visible = false;
                    customButtonCancel.Visible = true;

                    customButtonCancel.Location = pointOkButton_Ok;
                    //
                    break;
                case VisionSystemClassLibrary.Enum.MessageDisplayType.None://不显示按钮
                    //
                    customButtonOk.Visible = false;
                    customButtonCancel.Visible = false;
                    //
                    break;
                default:
                    break;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：设置控件类型
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _ClickButton(Boolean bokcancel)
        {
            bOkCancel = bokcancel;

            //

            _Reset();//恢复

            //事件

            if (null != Close_Click)//有效
            {
                Close_Click(this, new CustomEventArgs());
            }
        }

        //事件

        //----------------------------------------------------------------------
        // 功能说明：控件加载事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void MessageDisplay_Load(object sender, EventArgs e)
        {
            //不执行操作
        }

        //-----------------------------------------------------------------------
        // 功能说明：点击【OK】按钮，执行相应操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonOk_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(true);
        }

        //-----------------------------------------------------------------------
        // 功能说明：点击【Cancel】按钮，执行相应操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonCancel_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(false);
        }

        //-----------------------------------------------------------------------
        // 功能说明：窗口点击函数，不显示按钮时点击退出界面
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void MessageDisplay_Click(object sender, EventArgs e)
        {
            if (VisionSystemClassLibrary.Enum.MessageDisplayType.None == controlType)//不显示按钮
            {
                //_ClickButton(true);
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：label1点击函数，不显示按钮时点击退出界面
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonMessage_1_Click(object sender, EventArgs e)
        {
            if (VisionSystemClassLibrary.Enum.MessageDisplayType.None == controlType)//不显示按钮
            {
                //_ClickButton(true);
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：label2点击函数，不显示按钮时点击退出界面
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonMessage_2_Click(object sender, EventArgs e)
        {
            if (VisionSystemClassLibrary.Enum.MessageDisplayType.None == controlType)//不显示按钮
            {
                //_ClickButton(true);
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：label3点击函数，不显示按钮时点击退出界面
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonMessage_3_Click(object sender, EventArgs e)
        {
            if (VisionSystemClassLibrary.Enum.MessageDisplayType.None == controlType)//不显示按钮
            {
                //_ClickButton(true);
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：label4点击函数，不显示按钮时点击退出界面
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonMessage_4_Click(object sender, EventArgs e)
        {
            if (VisionSystemClassLibrary.Enum.MessageDisplayType.None == controlType)//不显示按钮
            {
                //_ClickButton(true);
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：label5点击函数，不显示按钮时点击退出界面
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonMessage_5_Click(object sender, EventArgs e)
        {
            if (VisionSystemClassLibrary.Enum.MessageDisplayType.None == controlType)//不显示按钮
            {
                //_ClickButton(true);
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：label6点击函数，不显示按钮时点击退出界面
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonMessage_6_Click(object sender, EventArgs e)
        {
            if (VisionSystemClassLibrary.Enum.MessageDisplayType.None == controlType)//不显示按钮
            {
                //_ClickButton(true);
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：label7点击函数，不显示按钮时点击退出界面
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonMessage_7_Click(object sender, EventArgs e)
        {
            if (VisionSystemClassLibrary.Enum.MessageDisplayType.None == controlType)//不显示按钮
            {
                //_ClickButton(true);
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：label8点击函数，不显示按钮时点击退出界面
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonMessage_8_Click(object sender, EventArgs e)
        {
            if (VisionSystemClassLibrary.Enum.MessageDisplayType.None == controlType)//不显示按钮
            {
                //_ClickButton(true);
            }
        }
    }
}