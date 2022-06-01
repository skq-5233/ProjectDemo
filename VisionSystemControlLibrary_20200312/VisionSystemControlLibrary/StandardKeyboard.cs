/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：StandardKeyboard.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：标准键盘控件

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

using System.Reflection;

namespace VisionSystemControlLibrary
{
    public partial class StandardKeyboard : UserControl
    {
        //标准键盘窗口

        private VisionSystemClassLibrary.Enum.InterfaceLanguage language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;//属性，语言

        private String[] sInvalidCharacter = null;//属性，不能包含的文本。取值范围：true，是；false，否

        private Boolean bCapsLock = false;//属性，Caps Lock。取值范围：true，打开；false，关闭

        private Boolean bShift = false;//属性，Shift。取值范围：true，按下；false，弹起

        private Boolean bIsPassword = false;//属性，密码输入窗口。取值范围：true，是；false，否
        private Int32 iPasswordStyle = 0;//属性，密码输入类型。取值范围：0，密码输入（输入完成，正确，关闭窗口）；1，输入当前密码；2，输入新的密码；3，确认密码
        private String sPassword = "";//属性，当前密码（可能包含多个密码）
        private String[] sPasswordArray = null;//当前密码组

        private Byte byteMaxLength = 30;//属性，最大长度

        private String sStringValue = "";//属性，输入数值

        private String sStringValue_Original = "";//原始输入数值

        //

        private Boolean bEnterNewValue = false;//属性（只读），是否输入了新的数值或密码输入是否正确。取值范围：true，是；false，否

        //

        private Boolean bFirstInitialStringValue = false; //是否第一次初始化输入参数 

        //

        [Browsable(true), Description("点击【Esc】或【Enter】时产生的事件"), Category("DigitalKeyboard 事件")]
        public event EventHandler Close_Click;//点击【Esc】或【Enter】时产生的事件

        //构造函数

        //-----------------------------------------------------------------------
        // 功能说明：构造函数
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public StandardKeyboard()
        {
            InitializeComponent();

            //
            dtucTextBoxEx1.TextChanged += new EventHandler(dtucTextBoxEx1_TextChanged);

            customButtonCaption.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonCaption.English_TextDisplay = new String[1] { " " };//设置显示的文本

            labelDisplay.Text = "";
            dtucTextBoxEx1.InputText = "";
        }

        //属性

        //-----------------------------------------------------------------------
        // 功能说明：Language属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("语言"), Category("StandardKeyboard 通用")]
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
        // 功能说明：Chinese_Caption属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("中文标题名称"), Category("StandardKeyboard 通用")]
        public String Chinese_Caption
        {
            get//读取
            {
                return customButtonCaption.Chinese_TextDisplay[0];
            }
            set//设置
            {
                if ("" != value && value != customButtonCaption.Chinese_TextDisplay[0])//有效
                {
                    customButtonCaption.Chinese_TextDisplay = new String[1] { value };//设置显示的文本
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：English_Caption属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("英文标题名称"), Category("StandardKeyboard 通用")]
        public String English_Caption
        {
            get//读取
            {
                return customButtonCaption.English_TextDisplay[0];
            }
            set//设置
            {
                if ("" != value && value != customButtonCaption.English_TextDisplay[0])//有效
                {
                    customButtonCaption.English_TextDisplay = new String[1] { value };//设置显示的文本
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：InvalidCharacter属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("不能包含的文本。取值范围：true，是；false，否"), Category("StandardKeyboard 通用")]
        public String[] InvalidCharacter
        {
            get//读取
            {
                return sInvalidCharacter;
            }
            set//设置
            {
                if (null != value)//有效
                {
                    sInvalidCharacter = new String[value.Length];//

                    value.CopyTo(sInvalidCharacter, 0);

                    //

                    Int32 i = 0;//循环控制变量

                    for (i = 0; i < sInvalidCharacter.Length; i++)//小写
                    {
                        sInvalidCharacter[i] = sInvalidCharacter[i].ToLower();
                    }
                }
                else//无效
                {
                    sInvalidCharacter = null;
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：CapsLock属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("Caps Lock。取值范围：true，打开；false，关闭"), Category("StandardKeyboard 通用")]
        public Boolean CapsLock
        {
            get//读取
            {
                return bCapsLock;
            }
            set//设置
            {
                if (value != bCapsLock)
                {
                    bCapsLock = value;

                    //

                    _SetCapsLock();//键盘按键
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Shift属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("Shift。取值范围：true，按下；false，弹起"), Category("StandardKeyboard 通用")]
        public Boolean Shift
        {
            get//读取
            {
                return bShift;
            }
            set//设置
            {
                if (value != bShift)
                {
                    bShift = value;

                    //

                    _SetShift();//键盘按键

                    _SetKey();//按键设置
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：IsPassword属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("密码输入窗口。取值范围：true，是；false，否"), Category("StandardKeyboard 通用")]
        public Boolean IsPassword
        {
            get//读取
            {
                return bIsPassword;
            }
            set//设置
            {
                bIsPassword = value;

                //

                if (bIsPassword)//密码
                {
                    if (0 == iPasswordStyle || 1 == iPasswordStyle)
                    {
                        customButtonMessage.CurrentTextGroupIndex = 0;
                    }
                    else
                    {
                        customButtonMessage.CurrentTextGroupIndex = 1;
                    }
                    customButtonMessage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                    customButtonMessage.Visible = true;//信息
                }
                else//非密码
                {
                    customButtonMessage.Visible = false;//信息
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：PasswordStyle属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("密码输入类型。取值范围：0，密码输入（输入完成，正确，关闭窗口）；1，输入当前密码；2，输入新的密码；3，确认密码"), Category("StandardKeyboard 通用")]
        public Int32 PasswordStyle
        {
            get//读取
            {
                return iPasswordStyle;
            }
            set//设置
            {
                if (0 <= value && 3 >= value)
                {
                    iPasswordStyle = value;

                    //

                    if (bIsPassword)//密码
                    {
                        if (0 == value || 1 == value)
                        {
                            customButtonMessage.CurrentTextGroupIndex = 0;
                        }
                        else if (3 == value)
                        {
                            customButtonMessage.CurrentTextGroupIndex = 2;
                        }
                        else
                        {
                            customButtonMessage.CurrentTextGroupIndex = 1;
                        }
                        customButtonMessage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                        customButtonMessage.Visible = true;//信息
                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Password属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("当前密码（可能包含多个密码）"), Category("StandardKeyboard 通用")]
        public String Password
        {
            get//读取
            {
                return sPassword;
            }
            set//设置
            {
                if (value != sPassword)
                {
                    sPassword = value;

                    //

                    if ("" != sPassword)//有效
                    {
                        sPasswordArray = sPassword.Split('\n');//当前密码组
                    }
                    else//无效
                    {
                        sPasswordArray = null;//当前密码组
                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：MaxLength属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("最大长度"), Category("StandardKeyboard 通用")]
        public Byte MaxLength
        {
            get//读取
            {
                return byteMaxLength;
            }
            set//设置
            {
                if (value != byteMaxLength)
                {
                    if (0 >= byteMaxLength)//不符合要求
                    {
                        byteMaxLength = 30;
                    }
                    else//符合要求
                    {
                        byteMaxLength = value;
                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：StringValue属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("输入数值"), Category("StandardKeyboard 通用")]
        public String StringValue
        {
            get//读取
            {
                return sStringValue;
            }
            set//设置
            {
                if (value != sStringValue)
                {
                    sStringValue = value;

                    sStringValue_Original = value;

                    //

                    labelDisplay.Text = value;
                    dtucTextBoxEx1.InputText = value;

                    bEnterNewValue = false;

                    bFirstInitialStringValue = true;
                } 
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：StringValueBuf属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("输入数值"), Category("StandardKeyboard 通用")]
        public String StringValueBuf
        {
            get//读取
            {
                return sStringValue;
            }
            set//设置
            {
                if (value != sStringValue)
                {
                    sStringValue = value;
                    
                    //

                    if (bIsPassword)//密码
                    {
                        labelDisplay.Text = "";
                        dtucTextBoxEx1.InputText = "";
                        for (Byte i = 0; i < StringValueBuf.Length; i++)
                        {
                            labelDisplay.Text += "*";
                            dtucTextBoxEx1.InputText += "*";
                        }
                    }
                    else//其它
                    {
                        labelDisplay.Text = sStringValue;
                        dtucTextBoxEx1.InputText = sStringValue;
                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：EnterNewValue属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("是否输入了新的数值或密码输入是否正确。取值范围：true，是；false，否"), Category("StandardKeyboard 通用")]
        public Boolean EnterNewValue
        {
            get//读取
            {
                return bEnterNewValue;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：FirstInitialStringValue属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("是否第一次初始化输入参数。取值范围：true，是；false，否"), Category("HandWriting 通用")]
        public Boolean FirstInitialStringValue
        {
            get//读取
            {
                return bFirstInitialStringValue;
            }
            set
            {
                if (value != bFirstInitialStringValue)
                {
                    bFirstInitialStringValue = value;
                }
            }
        }

        //函数

        //-----------------------------------------------------------------------
        // 功能说明：长度检查
        // 输入参数：1.inputstring：输入的字符串数值
        // 输出参数：无
        // 返 回 值：true，未超过最大长度；false，超过最大长度
        //----------------------------------------------------------------------
        private Boolean _CheckLength(String inputstring)
        {
            if (inputstring.Length < byteMaxLength)//未超过最大长度
            {
                return true;
            }
            else//查过最大长度
            {
                return false;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：文件名检查
        // 输入参数：1.inputstring：输入的字符串数值
        // 输出参数：无
        // 返 回 值：true，合法；false，不合法
        //----------------------------------------------------------------------
        private Boolean _CheckFileName(String inputstring)
        {
            char[] chInvalidCharacter = {'\\', '/', ':', '*', '?', '"', '<', '>', '|'};//非法字符

            if (0 <= inputstring.IndexOfAny(chInvalidCharacter))//合法
            {
                return true;
            }
            else//非法
            {
                return false;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：密码检查
        // 输入参数：1.sEnter：输入的密码
        // 输出参数：无
        // 返 回 值：true，合法；false，不合法
        //----------------------------------------------------------------------
        private Boolean _CheckPassword(String sEnter)
        {
            Int32 i = 0;//循环控制变量
            Boolean bReturn = false;//返回值

            if (null != sPasswordArray)//有效
            {
                for (i = 0; i < sPasswordArray.Length; i++)
                {
                    if (sEnter == sPasswordArray[i])//合法
                    {
                        break;
                    }
                }
                if (i < sPasswordArray.Length)//合法
                {
                    bReturn = true;
                } 
            }
            else//无效
            {
                if (sPassword == sEnter)//合法
                {
                    bReturn = true;
                }
            }

            return bReturn;
        }

        //----------------------------------------------------------------------
        // 功能说明：设置语言
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetLanguage()
        {
            customButtonCaption.Language = language;//标题
            customButtonMessage.Language = language;//信息
        }

        //-----------------------------------------------------------------------
        // 功能说明：按下【Caps Lock】后，设置键盘按键
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        private void _SetCapsLock()
        {
            if (bCapsLock)//Caps Lock，打开
            {
                customButtonQ.CurrentTextGroupIndex = 1;//Q
                customButtonW.CurrentTextGroupIndex = 1;//W
                customButtonE.CurrentTextGroupIndex = 1;//E
                customButtonR.CurrentTextGroupIndex = 1;//R
                customButtonT.CurrentTextGroupIndex = 1;//T
                customButtonY.CurrentTextGroupIndex = 1;//Y
                customButtonU.CurrentTextGroupIndex = 1;//U
                customButtonI.CurrentTextGroupIndex = 1;//I
                customButtonO.CurrentTextGroupIndex = 1;//O
                customButtonP.CurrentTextGroupIndex = 1;//P
                customButtonA.CurrentTextGroupIndex = 1;//A
                customButtonS.CurrentTextGroupIndex = 1;//S
                customButtonD.CurrentTextGroupIndex = 1;//D
                customButtonF.CurrentTextGroupIndex = 1;//F
                customButtonG.CurrentTextGroupIndex = 1;//G
                customButtonH.CurrentTextGroupIndex = 1;//H
                customButtonJ.CurrentTextGroupIndex = 1;//J
                customButtonK.CurrentTextGroupIndex = 1;//K
                customButtonL.CurrentTextGroupIndex = 1;//L
                customButtonZ.CurrentTextGroupIndex = 1;//Z
                customButtonX.CurrentTextGroupIndex = 1;//X
                customButtonC.CurrentTextGroupIndex = 1;//C
                customButtonV.CurrentTextGroupIndex = 1;//V
                customButtonB.CurrentTextGroupIndex = 1;//B
                customButtonN.CurrentTextGroupIndex = 1;//N
                customButtonM.CurrentTextGroupIndex = 1;//M

                //

                customButtonCapsLock.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
            }
            else//Caps Lock，关闭
            {
                customButtonQ.CurrentTextGroupIndex = 0;//q
                customButtonW.CurrentTextGroupIndex = 0;//w
                customButtonE.CurrentTextGroupIndex = 0;//e
                customButtonR.CurrentTextGroupIndex = 0;//r
                customButtonT.CurrentTextGroupIndex = 0;//t
                customButtonY.CurrentTextGroupIndex = 0;//y
                customButtonU.CurrentTextGroupIndex = 0;//u
                customButtonI.CurrentTextGroupIndex = 0;//i
                customButtonO.CurrentTextGroupIndex = 0;//o
                customButtonP.CurrentTextGroupIndex = 0;//p
                customButtonA.CurrentTextGroupIndex = 0;//a
                customButtonS.CurrentTextGroupIndex = 0;//s
                customButtonD.CurrentTextGroupIndex = 0;//d
                customButtonF.CurrentTextGroupIndex = 0;//f
                customButtonG.CurrentTextGroupIndex = 0;//g
                customButtonH.CurrentTextGroupIndex = 0;//h
                customButtonJ.CurrentTextGroupIndex = 0;//j
                customButtonK.CurrentTextGroupIndex = 0;//k
                customButtonL.CurrentTextGroupIndex = 0;//l
                customButtonZ.CurrentTextGroupIndex = 0;//z
                customButtonX.CurrentTextGroupIndex = 0;//x
                customButtonC.CurrentTextGroupIndex = 0;//c
                customButtonV.CurrentTextGroupIndex = 0;//v
                customButtonB.CurrentTextGroupIndex = 0;//b
                customButtonN.CurrentTextGroupIndex = 0;//n
                customButtonM.CurrentTextGroupIndex = 0;//m

                //

                customButtonCapsLock.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：按下【Shift】后，设置键盘按键
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        private void _SetShift()
        {
            if (bShift)//Shift，按下
            {
                customButton1Left1.CurrentTextGroupIndex = 1;//~
                customButton1.CurrentTextGroupIndex = 1;//!
                customButton2.CurrentTextGroupIndex = 1;//@
                customButton3.CurrentTextGroupIndex = 1;//#
                customButton4.CurrentTextGroupIndex = 1;//$
                customButton5.CurrentTextGroupIndex = 1;//%
                customButton6.CurrentTextGroupIndex = 1;//^
                customButton7.CurrentTextGroupIndex = 1;//&
                customButton8.CurrentTextGroupIndex = 1;//*
                customButton9.CurrentTextGroupIndex = 1;//(
                customButton0.CurrentTextGroupIndex = 1;//)
                customButton0Right1.CurrentTextGroupIndex = 1;//_
                customButton0Right2.CurrentTextGroupIndex = 1;//+

                customButtonPRight1.CurrentTextGroupIndex = 1;//{
                customButtonPRight2.CurrentTextGroupIndex = 1;//}
                customButtonPRight3.CurrentTextGroupIndex = 1;//|

                customButtonLRight1.CurrentTextGroupIndex = 1;//:
                customButtonLRight2.CurrentTextGroupIndex = 1;//"

                customButtonMRight1.CurrentTextGroupIndex = 1;//<
                customButtonMRight2.CurrentTextGroupIndex = 1;//>
                customButtonMRight3.CurrentTextGroupIndex = 1;//?

                //

                customButtonLeftShift.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
                customButtonRightShift.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
            }
            else//Shift，弹起
            {
                customButton1Left1.CurrentTextGroupIndex = 0;//`
                customButton1.CurrentTextGroupIndex = 0;//1
                customButton2.CurrentTextGroupIndex = 0;//2
                customButton3.CurrentTextGroupIndex = 0;//3
                customButton4.CurrentTextGroupIndex = 0;//4
                customButton5.CurrentTextGroupIndex = 0;//5
                customButton6.CurrentTextGroupIndex = 0;//6
                customButton7.CurrentTextGroupIndex = 0;//7
                customButton8.CurrentTextGroupIndex = 0;//8
                customButton9.CurrentTextGroupIndex = 0;//9
                customButton0.CurrentTextGroupIndex = 0;//0
                customButton0Right1.CurrentTextGroupIndex = 0;//-
                customButton0Right2.CurrentTextGroupIndex = 0;//=

                customButtonPRight1.CurrentTextGroupIndex = 0;//[
                customButtonPRight2.CurrentTextGroupIndex = 0;//]
                customButtonPRight3.CurrentTextGroupIndex = 0;//\

                customButtonLRight1.CurrentTextGroupIndex = 0;//;
                customButtonLRight2.CurrentTextGroupIndex = 0;//'

                customButtonMRight1.CurrentTextGroupIndex = 0;//,
                customButtonMRight2.CurrentTextGroupIndex = 0;//.
                customButtonMRight3.CurrentTextGroupIndex = 0;///

                //

                customButtonLeftShift.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                customButtonRightShift.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：设置键盘按键
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        private void _SetKey()
        {
            if (null == sInvalidCharacter)//所有键盘字符均有效
            {
                //不执行操作
            }
            else//存在不能输入的字符
            {
                Int32 i = 0;//循环控制变量
                Int32 j = 0;//循环控制变量

                for (i = 0; i < sInvalidCharacter.Length; i++)//遍历
                {
                    switch (sInvalidCharacter[i])
                    {
                        case "`"://`
                            //
                            if (!bShift)//【Shift】，未按下
                            {
                                customButton1Left1.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            }
                            else//【Shift】，按下
                            {
                                for (j = 0; j < sInvalidCharacter.Length; j++)//遍历
                                {
                                    if ("~" == sInvalidCharacter[j])//存在
                                    {
                                        break;
                                    } 
                                }
                                if (j >= sInvalidCharacter.Length)//不存在
                                {
                                    customButton1Left1.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                                }
                            }
                            //
                            break;
                        case "~"://~
                            //
                            if (bShift)//【Shift】，按下
                            {
                                customButton1Left1.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            }
                            else//【Shift】，未按下
                            {
                                for (j = 0; j < sInvalidCharacter.Length; j++)//遍历
                                {
                                    if ("`" == sInvalidCharacter[j])//存在
                                    {
                                        break;
                                    }
                                }
                                if (j >= sInvalidCharacter.Length)//不存在
                                {
                                    customButton8.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                                }
                            }
                            //
                            break;
                        case "1"://1
                            //
                            if (!bShift)//【Shift】，未按下
                            {
                                customButton1.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            }
                            else//【Shift】，按下
                            {
                                for (j = 0; j < sInvalidCharacter.Length; j++)//遍历
                                {
                                    if ("!" == sInvalidCharacter[j])//存在
                                    {
                                        break;
                                    }
                                }
                                if (j >= sInvalidCharacter.Length)//不存在
                                {
                                    customButton8.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                                }
                            }
                            //
                            break;
                        case "!"://!
                            //
                            if (bShift)//【Shift】，按下
                            {
                                customButton1Left1.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            }
                            else//【Shift】，未按下
                            {
                                for (j = 0; j < sInvalidCharacter.Length; j++)//遍历
                                {
                                    if ("1" == sInvalidCharacter[j])//存在
                                    {
                                        break;
                                    }
                                }
                                if (j >= sInvalidCharacter.Length)//不存在
                                {
                                    customButton8.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                                }
                            }
                            //
                            break;
                        case "2"://2
                            //
                            if (!bShift)//【Shift】，未按下
                            {
                                customButton2.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            }
                            else//【Shift】，按下
                            {
                                for (j = 0; j < sInvalidCharacter.Length; j++)//遍历
                                {
                                    if ("@" == sInvalidCharacter[j])//存在
                                    {
                                        break;
                                    }
                                }
                                if (j >= sInvalidCharacter.Length)//不存在
                                {
                                    customButton8.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                                }
                            }
                            //
                            break;
                        case "@"://@
                            //
                            if (bShift)//【Shift】，按下
                            {
                                customButton2.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            }
                            else//【Shift】，未按下
                            {
                                for (j = 0; j < sInvalidCharacter.Length; j++)//遍历
                                {
                                    if ("2" == sInvalidCharacter[j])//存在
                                    {
                                        break;
                                    }
                                }
                                if (j >= sInvalidCharacter.Length)//不存在
                                {
                                    customButton8.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                                }
                            }
                            //
                            break;
                        case "3"://3
                            //
                            if (!bShift)//【Shift】，未按下
                            {
                                customButton3.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            }
                            else//【Shift】，按下
                            {
                                for (j = 0; j < sInvalidCharacter.Length; j++)//遍历
                                {
                                    if ("#" == sInvalidCharacter[j])//存在
                                    {
                                        break;
                                    }
                                }
                                if (j >= sInvalidCharacter.Length)//不存在
                                {
                                    customButton8.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                                }
                            }
                            //
                            break;
                        case "#"://#
                            //
                            if (bShift)//【Shift】，按下
                            {
                                customButton3.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            }
                            else//【Shift】，未按下
                            {
                                for (j = 0; j < sInvalidCharacter.Length; j++)//遍历
                                {
                                    if ("3" == sInvalidCharacter[j])//存在
                                    {
                                        break;
                                    }
                                }
                                if (j >= sInvalidCharacter.Length)//不存在
                                {
                                    customButton8.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                                }
                            }
                            //
                            break;
                        case "4"://4
                            //
                            if (!bShift)//【Shift】，未按下
                            {
                                customButton4.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            }
                            else//【Shift】，按下
                            {
                                for (j = 0; j < sInvalidCharacter.Length; j++)//遍历
                                {
                                    if ("$" == sInvalidCharacter[j])//存在
                                    {
                                        break;
                                    }
                                }
                                if (j >= sInvalidCharacter.Length)//不存在
                                {
                                    customButton8.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                                }
                            }
                            //
                            break;
                        case "$"://$
                            //
                            if (bShift)//【Shift】，按下
                            {
                                customButton4.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            }
                            else//【Shift】，未按下
                            {
                                for (j = 0; j < sInvalidCharacter.Length; j++)//遍历
                                {
                                    if ("4" == sInvalidCharacter[j])//存在
                                    {
                                        break;
                                    }
                                }
                                if (j >= sInvalidCharacter.Length)//不存在
                                {
                                    customButton8.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                                }
                            }
                            //
                            break;
                        case "5"://5
                            //
                            if (!bShift)//【Shift】，未按下
                            {
                                customButton5.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            }
                            else//【Shift】，按下
                            {
                                for (j = 0; j < sInvalidCharacter.Length; j++)//遍历
                                {
                                    if ("%" == sInvalidCharacter[j])//存在
                                    {
                                        break;
                                    }
                                }
                                if (j >= sInvalidCharacter.Length)//不存在
                                {
                                    customButton8.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                                }
                            }
                            //
                            break;
                        case "%"://%
                            //
                            if (bShift)//【Shift】，按下
                            {
                                customButton5.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            }
                            else//【Shift】，未按下
                            {
                                for (j = 0; j < sInvalidCharacter.Length; j++)//遍历
                                {
                                    if ("5" == sInvalidCharacter[j])//存在
                                    {
                                        break;
                                    }
                                }
                                if (j >= sInvalidCharacter.Length)//不存在
                                {
                                    customButton8.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                                }
                            }
                            //
                            break;
                        case "6"://6
                            //
                            if (!bShift)//【Shift】，未按下
                            {
                                customButton6.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            }
                            else//【Shift】，按下
                            {
                                for (j = 0; j < sInvalidCharacter.Length; j++)//遍历
                                {
                                    if ("^" == sInvalidCharacter[j])//存在
                                    {
                                        break;
                                    }
                                }
                                if (j >= sInvalidCharacter.Length)//不存在
                                {
                                    customButton8.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                                }
                            }
                            //
                            break;
                        case "^"://^
                            //
                            if (bShift)//【Shift】，按下
                            {
                                customButton6.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            }
                            else//【Shift】，未按下
                            {
                                for (j = 0; j < sInvalidCharacter.Length; j++)//遍历
                                {
                                    if ("6" == sInvalidCharacter[j])//存在
                                    {
                                        break;
                                    }
                                }
                                if (j >= sInvalidCharacter.Length)//不存在
                                {
                                    customButton8.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                                }
                            }
                            //
                            break;
                        case "7"://7
                            //
                            if (!bShift)//【Shift】，未按下
                            {
                                customButton7.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            }
                            else//【Shift】，按下
                            {
                                for (j = 0; j < sInvalidCharacter.Length; j++)//遍历
                                {
                                    if ("&" == sInvalidCharacter[j])//存在
                                    {
                                        break;
                                    }
                                }
                                if (j >= sInvalidCharacter.Length)//不存在
                                {
                                    customButton8.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                                }
                            }
                            //
                            break;
                        case "&"://&
                            //
                            if (bShift)//【Shift】，按下
                            {
                                customButton7.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            }
                            else//【Shift】，未按下
                            {
                                for (j = 0; j < sInvalidCharacter.Length; j++)//遍历
                                {
                                    if ("7" == sInvalidCharacter[j])//存在
                                    {
                                        break;
                                    }
                                }
                                if (j >= sInvalidCharacter.Length)//不存在
                                {
                                    customButton8.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                                }
                            }
                            //
                            break;
                        case "8"://8
                            //
                            if (!bShift)//【Shift】，未按下
                            {
                                customButton8.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            }
                            else//【Shift】，按下
                            {
                                for (j = 0; j < sInvalidCharacter.Length; j++)//遍历
                                {
                                    if ("*" == sInvalidCharacter[j])//存在
                                    {
                                        break;
                                    }
                                }
                                if (j >= sInvalidCharacter.Length)//不存在
                                {
                                    customButton8.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                                }
                            }
                            //
                            break;
                        case "*"://*
                            //
                            if (bShift)//【Shift】，按下
                            {
                                customButton8.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            }
                            else//【Shift】，未按下
                            {
                                for (j = 0; j < sInvalidCharacter.Length; j++)//遍历
                                {
                                    if ("8" == sInvalidCharacter[j])//存在
                                    {
                                        break;
                                    }
                                }
                                if (j >= sInvalidCharacter.Length)//不存在
                                {
                                    customButton8.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                                }
                            }
                            //
                            break;
                        case "9"://9
                            //
                            if (!bShift)//【Shift】，未按下
                            {
                                customButton9.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            }
                            else//【Shift】，按下
                            {
                                for (j = 0; j < sInvalidCharacter.Length; j++)//遍历
                                {
                                    if ("(" == sInvalidCharacter[j])//存在
                                    {
                                        break;
                                    }
                                }
                                if (j >= sInvalidCharacter.Length)//不存在
                                {
                                    customButton8.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                                }
                            }
                            //
                            break;
                        case "("://(
                            //
                            if (bShift)//【Shift】，按下
                            {
                                customButton9.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            }
                            else//【Shift】，未按下
                            {
                                for (j = 0; j < sInvalidCharacter.Length; j++)//遍历
                                {
                                    if ("9" == sInvalidCharacter[j])//存在
                                    {
                                        break;
                                    }
                                }
                                if (j >= sInvalidCharacter.Length)//不存在
                                {
                                    customButton8.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                                }
                            }
                            //
                            break;
                        case "0"://0
                            //
                            if (!bShift)//【Shift】，未按下
                            {
                                customButton0.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            }
                            else//【Shift】，按下
                            {
                                for (j = 0; j < sInvalidCharacter.Length; j++)//遍历
                                {
                                    if (")" == sInvalidCharacter[j])//存在
                                    {
                                        break;
                                    }
                                }
                                if (j >= sInvalidCharacter.Length)//不存在
                                {
                                    customButton8.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                                }
                            }
                            //
                            break;
                        case ")"://)
                            //
                            if (bShift)//【Shift】，按下
                            {
                                customButton0.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            }
                            else//【Shift】，未按下
                            {
                                for (j = 0; j < sInvalidCharacter.Length; j++)//遍历
                                {
                                    if ("0" == sInvalidCharacter[j])//存在
                                    {
                                        break;
                                    }
                                }
                                if (j >= sInvalidCharacter.Length)//不存在
                                {
                                    customButton8.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                                }
                            }
                            //
                            break;
                        case "-"://-
                            //
                            if (!bShift)//【Shift】，未按下
                            {
                                customButton0Right1.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            }
                            else//【Shift】，按下
                            {
                                for (j = 0; j < sInvalidCharacter.Length; j++)//遍历
                                {
                                    if ("_" == sInvalidCharacter[j])//存在
                                    {
                                        break;
                                    }
                                }
                                if (j >= sInvalidCharacter.Length)//不存在
                                {
                                    customButton8.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                                }
                            }
                            //
                            break;
                        case "_"://_
                            //
                            if (bShift)//【Shift】，按下
                            {
                                customButton0Right1.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            }
                            else//【Shift】，未按下
                            {
                                for (j = 0; j < sInvalidCharacter.Length; j++)//遍历
                                {
                                    if ("-" == sInvalidCharacter[j])//存在
                                    {
                                        break;
                                    }
                                }
                                if (j >= sInvalidCharacter.Length)//不存在
                                {
                                    customButton8.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                                }
                            }
                            //
                            break;
                        case "="://=
                            //
                            if (!bShift)//【Shift】，未按下
                            {
                                customButton0Right2.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            }
                            else//【Shift】，按下
                            {
                                for (j = 0; j < sInvalidCharacter.Length; j++)//遍历
                                {
                                    if ("+" == sInvalidCharacter[j])//存在
                                    {
                                        break;
                                    }
                                }
                                if (j >= sInvalidCharacter.Length)//不存在
                                {
                                    customButton8.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                                }
                            }
                            //
                            break;
                        case "+"://+
                            //
                            if (bShift)//【Shift】，按下
                            {
                                customButton0Right2.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            }
                            else//【Shift】，未按下
                            {
                                for (j = 0; j < sInvalidCharacter.Length; j++)//遍历
                                {
                                    if ("=" == sInvalidCharacter[j])//存在
                                    {
                                        break;
                                    }
                                }
                                if (j >= sInvalidCharacter.Length)//不存在
                                {
                                    customButton8.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                                }
                            }
                            //
                            break;
                        case "q"://q
                            //
                            customButtonQ.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            //
                            break;
                        case "w"://w
                            //
                            customButtonW.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            //
                            break;
                        case "e"://e
                            //
                            customButtonE.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            //
                            break;
                        case "r"://r
                            //
                            customButtonR.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            //
                            break;
                        case "t"://t
                            //
                            customButtonT.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            //
                            break;
                        case "y"://y
                            //
                            customButtonY.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            //
                            break;
                        case "u"://u
                            //
                            customButtonU.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            //
                            break;
                        case "i"://i
                            //
                            customButtonI.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            //
                            break;
                        case "o"://o
                            //
                            customButtonO.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            //
                            break;
                        case "p"://p
                            //
                            customButtonP.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            //
                            break;
                        case "["://[
                            //
                            if (!bShift)//【Shift】，未按下
                            {
                                customButtonPRight1.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            }
                            else//【Shift】，按下
                            {
                                for (j = 0; j < sInvalidCharacter.Length; j++)//遍历
                                {
                                    if ("{" == sInvalidCharacter[j])//存在
                                    {
                                        break;
                                    }
                                }
                                if (j >= sInvalidCharacter.Length)//不存在
                                {
                                    customButton8.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                                }
                            }
                            //
                            break;
                        case "{"://{
                            //
                            if (bShift)//【Shift】，按下
                            {
                                customButtonPRight1.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            }
                            else//【Shift】，未按下
                            {
                                for (j = 0; j < sInvalidCharacter.Length; j++)//遍历
                                {
                                    if ("[" == sInvalidCharacter[j])//存在
                                    {
                                        break;
                                    }
                                }
                                if (j >= sInvalidCharacter.Length)//不存在
                                {
                                    customButton8.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                                }
                            }
                            //
                            break;
                        case "]"://]
                            //
                            if (!bShift)//【Shift】，未按下
                            {
                                customButtonPRight2.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            }
                            else//【Shift】，按下
                            {
                                for (j = 0; j < sInvalidCharacter.Length; j++)//遍历
                                {
                                    if ("}" == sInvalidCharacter[j])//存在
                                    {
                                        break;
                                    }
                                }
                                if (j >= sInvalidCharacter.Length)//不存在
                                {
                                    customButton8.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                                }
                            }
                            //
                            break;
                        case "}"://}
                            //
                            if (bShift)//【Shift】，按下
                            {
                                customButtonPRight2.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            }
                            else//【Shift】，未按下
                            {
                                for (j = 0; j < sInvalidCharacter.Length; j++)//遍历
                                {
                                    if ("]" == sInvalidCharacter[j])//存在
                                    {
                                        break;
                                    }
                                }
                                if (j >= sInvalidCharacter.Length)//不存在
                                {
                                    customButton8.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                                }
                            }
                            //
                            break;
                        case "a"://a
                            //
                            customButtonA.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            //
                            break;
                        case "s"://s
                            //
                            customButtonS.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            //
                            break;
                        case "d"://d
                            //
                            customButtonD.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            //
                            break;
                        case "f"://f
                            //
                            customButtonF.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            //
                            break;
                        case "g"://g
                            //
                            customButtonG.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            //
                            break;
                        case "h"://h
                            //
                            customButtonH.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            //
                            break;
                        case "j"://j
                            //
                            customButtonJ.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            //
                            break;
                        case "k"://k
                            //
                            customButtonK.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            //
                            break;
                        case "l"://l
                            //
                            customButtonL.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            //
                            break;
                        case ";"://;
                            //
                            if (!bShift)//【Shift】，未按下
                            {
                                customButtonLRight1.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            }
                            else//【Shift】，按下
                            {
                                for (j = 0; j < sInvalidCharacter.Length; j++)//遍历
                                {
                                    if (":" == sInvalidCharacter[j])//存在
                                    {
                                        break;
                                    }
                                }
                                if (j >= sInvalidCharacter.Length)//不存在
                                {
                                    customButton8.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                                }
                            }
                            //
                            break;
                        case ":"://:
                            //
                            if (bShift)//【Shift】，按下
                            {
                                customButtonLRight1.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            }
                            else//【Shift】，未按下
                            {
                                for (j = 0; j < sInvalidCharacter.Length; j++)//遍历
                                {
                                    if (";" == sInvalidCharacter[j])//存在
                                    {
                                        break;
                                    }
                                }
                                if (j >= sInvalidCharacter.Length)//不存在
                                {
                                    customButton8.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                                }
                            }
                            //
                            break;
                        case "'"://'
                            //
                            if (!bShift)//【Shift】，未按下
                            {
                                customButtonLRight2.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            }
                            else//【Shift】，按下
                            {
                                for (j = 0; j < sInvalidCharacter.Length; j++)//遍历
                                {
                                    if ("\"" == sInvalidCharacter[j])//存在
                                    {
                                        break;
                                    }
                                }
                                if (j >= sInvalidCharacter.Length)//不存在
                                {
                                    customButton8.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                                }
                            }
                            //
                            break;
                        case "\""://"
                            //
                            if (bShift)//【Shift】，按下
                            {
                                customButtonLRight2.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            }
                            else//【Shift】，未按下
                            {
                                for (j = 0; j < sInvalidCharacter.Length; j++)//遍历
                                {
                                    if ("'" == sInvalidCharacter[j])//存在
                                    {
                                        break;
                                    }
                                }
                                if (j >= sInvalidCharacter.Length)//不存在
                                {
                                    customButton8.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                                }
                            }
                            //
                            break;
                        case "z"://z
                            //
                            customButtonZ.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            //
                            break;
                        case "x"://x
                            //
                            customButtonX.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            //
                            break;
                        case "c"://c
                            //
                            customButtonC.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            //
                            break;
                        case "v"://v
                            //
                            customButtonV.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            //
                            break;
                        case "b"://b
                            //
                            customButtonB.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            //
                            break;
                        case "n"://n
                            //
                            customButtonN.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            //
                            break;
                        case "m"://m
                            //
                            customButtonM.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            //
                            break;
                        case ","://,
                            //
                            if (!bShift)//【Shift】，未按下
                            {
                                customButtonMRight1.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            }
                            else//【Shift】，按下
                            {
                                for (j = 0; j < sInvalidCharacter.Length; j++)//遍历
                                {
                                    if ("<" == sInvalidCharacter[j])//存在
                                    {
                                        break;
                                    }
                                }
                                if (j >= sInvalidCharacter.Length)//不存在
                                {
                                    customButton8.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                                }
                            }
                            //
                            break;
                        case "<"://<
                            //
                            if (bShift)//【Shift】，按下
                            {
                                customButtonMRight1.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            }
                            else//【Shift】，未按下
                            {
                                for (j = 0; j < sInvalidCharacter.Length; j++)//遍历
                                {
                                    if ("," == sInvalidCharacter[j])//存在
                                    {
                                        break;
                                    }
                                }
                                if (j >= sInvalidCharacter.Length)//不存在
                                {
                                    customButton8.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                                }
                            }
                            //
                            break;
                        case "."://.
                            //
                            if (!bShift)//【Shift】，未按下
                            {
                                customButtonMRight2.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            }
                            else//【Shift】，按下
                            {
                                for (j = 0; j < sInvalidCharacter.Length; j++)//遍历
                                {
                                    if (">" == sInvalidCharacter[j])//存在
                                    {
                                        break;
                                    }
                                }
                                if (j >= sInvalidCharacter.Length)//不存在
                                {
                                    customButton8.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                                }
                            }
                            //
                            break;
                        case ">"://>
                            //
                            if (bShift)//【Shift】，按下
                            {
                                customButtonMRight2.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            }
                            else//【Shift】，未按下
                            {
                                for (j = 0; j < sInvalidCharacter.Length; j++)//遍历
                                {
                                    if ("." == sInvalidCharacter[j])//存在
                                    {
                                        break;
                                    }
                                }
                                if (j >= sInvalidCharacter.Length)//不存在
                                {
                                    customButton8.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                                }
                            }
                            //
                            break;
                        case "/":///
                            //
                            if (!bShift)//【Shift】，未按下
                            {
                                customButtonMRight3.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            }
                            else//【Shift】，按下
                            {
                                for (j = 0; j < sInvalidCharacter.Length; j++)//遍历
                                {
                                    if ("?" == sInvalidCharacter[j])//存在
                                    {
                                        break;
                                    }
                                }
                                if (j >= sInvalidCharacter.Length)//不存在
                                {
                                    customButton8.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                                }
                            }
                            //
                            break;
                        case "?"://?
                            //
                            if (bShift)//【Shift】，按下
                            {
                                customButtonMRight3.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            }
                            else//【Shift】，未按下
                            {
                                for (j = 0; j < sInvalidCharacter.Length; j++)//遍历
                                {
                                    if ("/" == sInvalidCharacter[j])//存在
                                    {
                                        break;
                                    }
                                }
                                if (j >= sInvalidCharacter.Length)//不存在
                                {
                                    customButton8.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                                }
                            }
                            //
                            break;
                        case " ":// 
                            //
                            customButtonSpace.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            //
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：点击按钮，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _ClickButton(CustomButton button)
        {
            if (bFirstInitialStringValue) //当前第一次初始化控件参数
            {
                bFirstInitialStringValue = false;

                sStringValue = "";

                //

                labelDisplay.Text = sStringValue;
                dtucTextBoxEx1.InputText = sStringValue;
            }

            if (_CheckLength(sStringValue))//长度有效
            {
                sStringValue += button.English_TextArray[button.CurrentTextGroupIndex];

                if (bIsPassword)//密码
                {
                    labelDisplay.Text += "*";
                    dtucTextBoxEx1.InputText += "*";
                }
                else//其它
                {
                    labelDisplay.Text = sStringValue;
                    dtucTextBoxEx1.InputText = sStringValue;
                }
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
        private void StandardKeyboard_Load(object sender, EventArgs e)
        {
            //不执行操作
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：【Esc】按下，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonEsc_CustomButton_Click(object sender, EventArgs e)
        {
            bFirstInitialStringValue = false;

            customButtonMessage.Visible = false;//信息

            //

            bEnterNewValue = false;

            bIsPassword = false;

            //事件

            if (null != Close_Click)//有效
            {
                Close_Click(this, new CustomEventArgs());
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：【Clr】按下，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonClr_CustomButton_Click(object sender, EventArgs e)
        {
            bFirstInitialStringValue = false;

            sStringValue = "";

            //

            labelDisplay.Text = sStringValue;
            dtucTextBoxEx1.InputText = sStringValue;

            //

            customButtonMessage.Visible = false;//信息
        }

        //-----------------------------------------------------------------------
        // 功能说明：【`（~）】按下，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButton1Left1_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButton1Left1);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：【1（!）】按下，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButton1_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButton1);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：【2（@）】按下，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButton2_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButton2);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：【3（#）】按下，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButton3_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButton3);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：【4（$）】按下，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButton4_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButton4);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：【5（%）】按下，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButton5_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButton5);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：【6（^）】按下，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButton6_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButton6);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：【7（&）】按下，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButton7_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButton7);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：【8（*）】按下，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButton8_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButton8);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：【9（（）】按下，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButton9_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButton9);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：【0（））】按下，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButton0_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButton0);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：【-（_）】按下，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButton0Right1_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButton0Right1);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：【=（+）】按下，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButton0Right2_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButton0Right2);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：【Backspace】按下，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonBackspace_CustomButton_Click(object sender, EventArgs e)
        {
            bFirstInitialStringValue = false;

            if (0 < sStringValue.Length)//有效
            {
                sStringValue = sStringValue.Substring(0, sStringValue.Length - 1);//更新

                if (bIsPassword)//密码
                {
                    labelDisplay.Text = labelDisplay.Text.Substring(0, labelDisplay.Text.Length - 1);
                    dtucTextBoxEx1.InputText = labelDisplay.Text.Substring(0, labelDisplay.Text.Length - 1);
                }
                else//其它
                {
                    labelDisplay.Text = sStringValue;
                    dtucTextBoxEx1.InputText = sStringValue;
                }
            }

            //

            customButtonMessage.Visible = false;//信息
        }

        //-----------------------------------------------------------------------
        // 功能说明：【Tab】按下，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonTab_CustomButton_Click(object sender, EventArgs e)
        {
            //不执行操作
        }

        //-----------------------------------------------------------------------
        // 功能说明：【q（Q）】按下，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonQ_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButtonQ);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：【w（W）】按下，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonW_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButtonW);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：【e（E）】按下，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonE_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButtonE);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：【r（R）】按下，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonR_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButtonR);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：【t（T）】按下，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonT_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButtonT);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：【y（Y）】按下，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonY_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButtonY);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：【u（U）】按下，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonU_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButtonU);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：【i（I）】按下，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonI_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButtonI);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：【o（O）】按下，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonO_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButtonO);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：【p（P）】按下，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonP_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButtonP);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：【[（{）】按下，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonPRight1_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButtonPRight1);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：【]（}）】按下，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonPRight2_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButtonPRight2);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：【\（|）】按下，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonPRight3_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButtonPRight3);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：【Caps Lock】按下，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonCapsLock_CustomButton_Click(object sender, EventArgs e)
        {
            CapsLock = !CapsLock;//更新设置，控件显示
        }

        //-----------------------------------------------------------------------
        // 功能说明：【a（A）】按下，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonA_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButtonA);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：【s（S）】按下，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonS_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButtonS);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：【d（D）】按下，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonD_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButtonD);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：【f（F）】按下，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonF_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButtonF);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：【g（G）】按下，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonG_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButtonG);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：【h（H）】按下，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonH_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButtonH);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：【j（J）】按下，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonJ_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButtonJ);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：【k（K）】按下，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonK_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButtonK);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：【l（L）】按下，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonL_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButtonL);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：【;（:）】按下，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonLRight1_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButtonLRight1);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：【'（"）】按下，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonLRight2_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButtonLRight2);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：【Enter】按下，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonEnter_CustomButton_Click(object sender, EventArgs e)
        {
            bFirstInitialStringValue = false;

            if (bIsPassword)//密码
            {
                switch (iPasswordStyle)
                {
                    case 0://密码输入（输入完成，正确，关闭窗口）
                        //
                        if (_CheckPassword(sStringValue))//输入正确
                        {
                            customButtonMessage.Visible = false;//信息

                            //

                            bEnterNewValue = true;

                            bIsPassword = false;

                            //事件

                            if (null != Close_Click)//有效
                            {
                                Close_Click(this, new CustomEventArgs());
                            }
                        }
                        else//输入错误
                        {
                            sStringValue = "";

                            labelDisplay.Text = "";
                            dtucTextBoxEx1.InputText = "";

                            //

                            customButtonMessage.CurrentTextGroupIndex = 3;
                            customButtonMessage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            customButtonMessage.Visible = true;//信息
                        }
                        //
                        break;
                    case 1://输入当前密码
                        //
                        if (_CheckPassword(sStringValue))//输入正确
                        {
                            iPasswordStyle++;//更新数值

                            //

                            sStringValue = "";

                            labelDisplay.Text = "";
                            dtucTextBoxEx1.InputText = "";

                            //

                            customButtonMessage.CurrentTextGroupIndex = 1;
                            customButtonMessage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                            customButtonMessage.Visible = true;//信息
                        }
                        else//输入错误
                        {
                            sStringValue = "";

                            labelDisplay.Text = "";
                            dtucTextBoxEx1.InputText = "";

                            //

                            customButtonMessage.CurrentTextGroupIndex = 3;
                            customButtonMessage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                            customButtonMessage.Visible = true;//信息
                        }
                        //
                        break;
                    case 2://输入新的密码
                        //
                        if ("" != sStringValue)
                        {
                            sPassword = sStringValue;

                            iPasswordStyle++;//更新数值

                            //

                            sStringValue = "";

                            labelDisplay.Text = "";
                            dtucTextBoxEx1.InputText = "";

                            //

                            customButtonMessage.CurrentTextGroupIndex = 2;
                            customButtonMessage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                            customButtonMessage.Visible = true;//信息
                        } 
                        else
                        {
                            sStringValue = "";

                            labelDisplay.Text = "";
                            dtucTextBoxEx1.InputText = "";

                            //

                            customButtonMessage.CurrentTextGroupIndex = 1;
                            customButtonMessage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                            customButtonMessage.Visible = true;//信息
                        }
                        //
                        break;
                    case 3://确认密码
                        //
                        if (sStringValue == sPassword)//输入正确
                        {
                            customButtonMessage.Visible = false;//信息

                            //

                            bEnterNewValue = true;

                            bIsPassword = false;

                            //事件

                            if (null != Close_Click)//有效
                            {
                                Close_Click(this, new CustomEventArgs());
                            }
                        }
                        else//输入错误
                        {
                            iPasswordStyle = 2;

                            //

                            sStringValue = "";

                            labelDisplay.Text = "";
                            dtucTextBoxEx1.InputText = "";

                            //

                            customButtonMessage.CurrentTextGroupIndex = 1;
                            customButtonMessage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                            customButtonMessage.Visible = true;//信息
                        }
                        //
                        break;
                    default:
                        break;
                }
            }
            else//其它
            {
                customButtonMessage.Visible = false;//信息

                //

                if ("" == sStringValue)//未输入
                {
                    bEnterNewValue = false;
                }
                else//输入
                {
                    if (sStringValue == sStringValue_Original)//未改变
                    {
                        bEnterNewValue = false;
                    }
                    else//改变
                    {
                        bEnterNewValue = true;
                    }
                }

                bIsPassword = false;

                //事件

                if (null != Close_Click)//有效
                {
                    Close_Click(this, new CustomEventArgs());
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：【Shift】按下，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonLeftShift_CustomButton_Click(object sender, EventArgs e)
        {
            Shift = !Shift;//更新设置，控件显示
        }

        //-----------------------------------------------------------------------
        // 功能说明：【z（Z）】按下，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonZ_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButtonZ);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：【x（X）】按下，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonX_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButtonX);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：【c（C）】按下，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonC_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButtonC);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：【v（V）】按下，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonV_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButtonV);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：【b（B）】按下，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonB_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButtonB);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：【n（N）】按下，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonN_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButtonN);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：【m（M）】按下，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonM_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButtonM);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：【,（<）】按下，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonMRight1_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButtonMRight1);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：【.（>）】按下，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonMRight2_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButtonMRight2);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：【/（?）】按下，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonMRight3_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButtonMRight3);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：【Shift】按下，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonRightShift_CustomButton_Click(object sender, EventArgs e)
        {
            Shift = !Shift;//更新设置，控件显示
        }

        //-----------------------------------------------------------------------
        // 功能说明：【Space】按下，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonSpace_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButtonSpace);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：手写内容变化，执行相关操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void dtucTextBoxEx1_TextChanged(object sender, EventArgs e)
        {
            bFirstInitialStringValue = false; //防止_ClickButton中清空已手写内容
            sStringValue = dtucTextBoxEx1.InputText;
        }
    }
}