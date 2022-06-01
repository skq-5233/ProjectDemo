/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：ApplicationRegistration.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：注册控件

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
    public partial class ApplicationRegistration : UserControl
    {
        //注册控件

        private VisionSystemClassLibrary.Enum.InterfaceLanguage language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;//属性，语言

        //

        private Boolean bRegisteredSuccessfully = false;//属性（只读），程序是否注册成功。取值范围：true，是；false，否

        //

        private String sSerialNumber = "";//属性，产品序列号

        private String sProductKey = "";//属性，产品密钥

        //

        private Object controlData = null;//属性，预留数据

        //

        [Browsable(true), Description("窗口关闭时产生的事件"), Category("ApplicationRegistration 事件")]
        public event EventHandler Close_Click;//窗口关闭时产生的事件

        //

        //-----------------------------------------------------------------------
        // 功能说明：构造函数
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public ApplicationRegistration()
        {
            InitializeComponent();

            //由于该控件仅有一个实例，因此可以在此处进行事件的订阅

            if (null != GlobalWindows.StandardKeyboard_Window)
            {
                GlobalWindows.StandardKeyboard_Window.WindowClose_ApplicationRegistration += new System.EventHandler(standardKeyboardWindow_WindowClose_ApplicationRegistration);//订阅事件
            }
        }

        //属性

        //-----------------------------------------------------------------------
        // 功能说明：Language属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("语言"), Category("ApplicationRegistration 通用")]
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
        // 功能说明：SerialNumber属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("产品序列号"), Category("ApplicationRegistration 通用")]
        public String SerialNumber
        {
            get//读取
            {
                return sSerialNumber;
            }
            set//设置
            {
                sSerialNumber = value;

                //

                labelSerialNumber.Text = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ProductKey属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("产品密钥"), Category("ApplicationRegistration 通用")]
        public String ProductKey
        {
            get//读取
            {
                return sProductKey;
            }
            set//设置
            {
                sProductKey = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：RegisteredSuccessfully属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("程序是否注册成功。取值范围：true，是；false，否"), Category("ApplicationRegistration 通用")]
        public Boolean RegisteredSuccessfully
        {
            get//读取
            {
                return bRegisteredSuccessfully;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ControlData属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("预留数据"), Category("ApplicationRegistration 通用")]
        public Object ControlData
        {
            get//读取
            {
                return controlData;
            }
            set//设置
            {
                controlData = value;
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
            customButtonMessage_1.Language = language;
            customButtonMessage_2.Language = language;
            customButtonMessage_3.Language = language;
        }

        //-----------------------------------------------------------------------
        // 功能说明：恢复控件内容
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _Reset()
        {
            labelSerialNumber.Text = "";
            labelProductKey.Text = "";

            customButtonMessage_2.Visible = false;
        }

        //事件

        //----------------------------------------------------------------------
        // 功能说明：窗口加载事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void ApplicationRegistration_Load(object sender, EventArgs e)
        {
            //不执行操作
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：点击Product Key控件，执行相应操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void labelProductKey_Click(object sender, EventArgs e)
        {
            customButtonMessage_2.Visible = false;

            //显示输入键盘

            GlobalWindows.StandardKeyboard_Window.WindowParameter = 1;//窗口特征数值
            GlobalWindows.StandardKeyboard_Window.Language = language;//语言
            GlobalWindows.StandardKeyboard_Window.Chinese_Caption = customButtonMessage_1.Chinese_TextDisplay[0];//中文标题文本
            GlobalWindows.StandardKeyboard_Window.English_Caption = customButtonMessage_1.English_TextDisplay[0];//英文标题文本
            GlobalWindows.StandardKeyboard_Window.CapsLock = true;//Caps Lock打开
            GlobalWindows.StandardKeyboard_Window.MaxLength = 30;//数值长度范围
            GlobalWindows.StandardKeyboard_Window.StringValue = labelProductKey.Text;//初始显示的数值
            GlobalWindows.StandardKeyboard_Window.IsPassword = false;//密码输入窗口

            GlobalWindows.StandardKeyboard_Window.StartPosition = FormStartPosition.CenterScreen;
            GlobalWindows.StandardKeyboard_Window.TopMost = true;//将窗口置于顶层
            GlobalWindows.StandardKeyboard_Window.Visible = true;//显示
        }

        //-----------------------------------------------------------------------
        // 功能说明：点击【OK】按钮，执行相应操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonOK_CustomButton_Click(object sender, EventArgs e)
        {
            if (sProductKey == labelProductKey.Text)//注册成功
            {
                _Reset();//恢复控件内容

                //

                bRegisteredSuccessfully = true;

                //事件

                if (null != Close_Click)//有效
                {
                    Close_Click(this, new CustomEventArgs());
                }
            }
            else//注册失败
            {
                customButtonMessage_2.Visible = true;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：点击【Cancel】按钮，执行相应操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonCancel_CustomButton_Click(object sender, EventArgs e)
        {
            _Reset();//恢复控件内容

            //

            bRegisteredSuccessfully = false;

            //事件

            if (null != Close_Click)//有效
            {
                Close_Click(this, new CustomEventArgs());
            }
        }

        //
        //----------------------------------------------------------------------
        // 功能说明：APPLICATION REGISTRATION，输入密钥，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void standardKeyboardWindow_WindowClose_ApplicationRegistration(object sender, EventArgs e)
        {
            GlobalWindows.StandardKeyboard_Window.Visible = false;//隐藏
            GlobalWindows.StandardKeyboard_Window.TopMost = false;//取消置于顶层

            //

            if (GlobalWindows.StandardKeyboard_Window.EnterNewValue)//输入完成
            {
                labelProductKey.Text = GlobalWindows.StandardKeyboard_Window.StringValue;
            }
            else//输入失败
            {
                //不执行操作
            }
        }
    }
}