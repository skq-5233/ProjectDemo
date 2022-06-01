/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：DigitalKeyboard.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：数字键盘控件

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
    public partial class DigitalKeyboard : UserControl
    {
        //数字小键盘控件

        //点击【Esc】或【Enter】后
        //EnterNewValue取值为true，表示可以使用设置的数值
        //EnterNewValue取值为false，表示忽略设置的数值（输入数值超出范围，未输入数值，输入数值未改变）
        //初始显示的数值可以通过属性NumericalValue或StringValue设置。若初始不显示任何值，可将属性StringValue的值设置为""

        //

        private VisionSystemClassLibrary.Enum.InterfaceLanguage language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;//属性，语言

        private Int32 iPrecision = 0;//属性，输入的数据精度。取值范围：0，整型；大于0，浮点型

        private Single fMinValue = Single.MinValue;//属性，输入范围，最小值
        private Single fMaxValue = Single.MaxValue;//属性，输入范围，最大值
        private Byte byteMaxLength = (Byte)(Int16.MaxValue.ToString().Length);//属性，最大长度

        private Single fNumericalValue = 0;//属性，输入数值
        private String sStringValue = "";//属性，输入数值

        private Single fNumericalValue_Original = 0;//原始输入数值

        //

        private Boolean bEnterNewValue = false;//属性（只读），是否输入了新的数值。取值范围：true，是；false，否

        //

        private String[][] sMessageText = new String[2][];//最小值和最大值控件上显示的文本（[语言][包含的文本]）

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
        public DigitalKeyboard()
        {
            InitializeComponent();

            //

            Int32 i = 0;//循环控制变量

            FieldInfo[] fieldinfo = VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese.GetType().GetFields();//获取公共字段
            if (null != fieldinfo && 1 < fieldinfo.Length)//有效
            {
                sMessageText = new String[fieldinfo.Length - 1][];
                for (i = 0; i < fieldinfo.Length - 1; i++)
                {
                    sMessageText[i] = new String[2];
                }

                //

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] = customButtonMinValue.Chinese_TextDisplay[0];
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] = customButtonMinValue.English_TextDisplay[0];

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][1] = customButtonMaxValue.Chinese_TextDisplay[0];
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][1] = customButtonMaxValue.English_TextDisplay[0];
            }

            //

            customButtonCaption.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonCaption.English_TextDisplay = new String[1] { " " };//设置显示的文本

            labelDisplay.Text = "";

            customButtonMinValue.Chinese_TextDisplay = new String[1] { sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] + fMinValue.ToString("F" + iPrecision.ToString()) };//设置显示的文本
            customButtonMinValue.English_TextDisplay = new String[1] { sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] + fMinValue.ToString("F" + iPrecision.ToString()) };//设置显示的文本

            customButtonMaxValue.Chinese_TextDisplay = new String[1] { sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][1] + fMaxValue.ToString("F" + iPrecision.ToString()) };//设置显示的文本
            customButtonMaxValue.English_TextDisplay = new String[1] { sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][1] + fMaxValue.ToString("F" + iPrecision.ToString()) };//设置显示的文本
        }

        //属性

        //-----------------------------------------------------------------------
        // 功能说明：Language属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("语言"), Category("DigitalKeyboard 通用")]
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
        [Browsable(true), Description("中文标题名称"), Category("DigitalKeyboard 通用")]
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
        [Browsable(true), Description("英文标题名称"), Category("DigitalKeyboard 通用")]
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
        // 功能说明：IntDouble属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("输入的数据精度。取值范围：0，整型；大于0，浮点型"), Category("DigitalKeyboard 通用")]
        public Int32 Precision
        {
            get//读取
            {
                return iPrecision;
            }
            set//设置
            {
                if (value != iPrecision)
                {
                    iPrecision = value;

                    //

                    if (0 == iPrecision)//整型
                    {
                        customButtonDot.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                    }
                    else//浮点型
                    {
                        customButtonDot.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                    }

                    //

                    customButtonMinValue.Chinese_TextDisplay = new String[1] { sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] + fMinValue.ToString("F" + iPrecision.ToString()) };//设置显示的文本
                    customButtonMinValue.English_TextDisplay = new String[1] { sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] + fMinValue.ToString("F" + iPrecision.ToString()) };//设置显示的文本

                    customButtonMaxValue.Chinese_TextDisplay = new String[1] { sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][1] + fMaxValue.ToString("F" + iPrecision.ToString()) };//设置显示的文本
                    customButtonMaxValue.English_TextDisplay = new String[1] { sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][1] + fMaxValue.ToString("F" + iPrecision.ToString()) };//设置显示的文本

                    sStringValue = fNumericalValue.ToString("F" + iPrecision.ToString());
                    labelDisplay.Text = sStringValue;
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：MinValue属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("输入范围，最小值"), Category("DigitalKeyboard 通用")]
        public Single MinValue
        {
            get//读取
            {
                return fMinValue;
            }
            set//设置
            {
                if (value != fMinValue)
                {
                    fMinValue = value;

                    //

                    customButtonMinValue.Chinese_TextDisplay = new String[1] { sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] + fMinValue.ToString("F" + iPrecision.ToString()) };//设置显示的文本
                    customButtonMinValue.English_TextDisplay = new String[1] { sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] + fMinValue.ToString("F" + iPrecision.ToString()) };//设置显示的文本
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：MaxValue属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("输入范围，最大值"), Category("DigitalKeyboard 通用")]
        public Single MaxValue
        {
            get//读取
            {
                return fMaxValue;
            }
            set//设置
            {
                if (value != fMaxValue)
                {
                    fMaxValue = value;

                    //

                    customButtonMaxValue.Chinese_TextDisplay = new String[1] { sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][1] + fMaxValue.ToString("F" + iPrecision.ToString()) };//设置显示的文本
                    customButtonMaxValue.English_TextDisplay = new String[1] { sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][1] + fMaxValue.ToString("F" + iPrecision.ToString()) };//设置显示的文本
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：MinValue属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("最大长度"), Category("DigitalKeyboard 通用")]
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
                    if (0 >= value || (Byte)(Int16.MaxValue.ToString().Length) < value)//不符合要求
                    {
                        byteMaxLength = (Byte)(Int16.MaxValue.ToString().Length);
                    }
                    else//符合要求
                    {
                        byteMaxLength = value;
                    }
                } 
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：NumericalValue属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("输入数值"), Category("DigitalKeyboard 通用")]
        public Single NumericalValue
        {
            get//读取
            {
                return fNumericalValue;
            }
            set//设置
            {
                if (_CheckValue(value))//有效
                {
                    fNumericalValue = value;

                    fNumericalValue_Original = value;

                    //

                    sStringValue = value.ToString("F" + iPrecision.ToString());

                    //

                    labelDisplay.Text = sStringValue;

                    bFirstInitialStringValue = true;
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：StringValue属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("输入数值"), Category("DigitalKeyboard 通用")]
        public String StringValue
        {
            get//读取
            {
                return sStringValue;
            }
            set//设置
            {
                if ("" == value)//无效
                {
                    fNumericalValue = 0;

                    fNumericalValue_Original = fNumericalValue;

                    //

                    labelDisplay.Text = value;

                    bFirstInitialStringValue = true;
                }
                else//有效
                {
                    Single fValue = Convert.ToSingle(value);

                    if (_CheckValue(fValue))//有效
                    {
                        fNumericalValue = fValue;

                        fNumericalValue_Original = fNumericalValue;

                        //

                        sStringValue = fNumericalValue.ToString("F" + iPrecision.ToString()); ;

                        //

                        labelDisplay.Text = sStringValue;

                        bFirstInitialStringValue = true;
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
        [Browsable(false), Description("是否输入了新的数值。取值范围：true，是；false，否"), Category("DigitalKeyboard 通用")]
        public Boolean EnterNewValue
        {
            get//读取
            {
                return bEnterNewValue;
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
        // 功能说明：数值范围检查
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：true，未超出范围；false，超出范围
        //----------------------------------------------------------------------
        private Boolean _CheckValue(Single inputvalue)
        {
            if (inputvalue >= fMinValue && inputvalue <= fMaxValue)//未超出范围
            {
                return true;
            }
            else//超出范围
            {
                return false;
            }
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
            customButtonMinValue.Language = language;//最小值
            customButtonMaxValue.Language = language;//最大值
        }

        //-----------------------------------------------------------------------
        // 功能说明：点击【0】 ~ 【9】按钮，执行相关操作
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
            }

            if (_CheckLength(sStringValue))//长度有效
            {
                fNumericalValue = Convert.ToSingle(sStringValue + button.English_TextDisplay[0]);

                sStringValue += button.English_TextDisplay[0];

                //

                labelDisplay.Text = sStringValue;
            }

            //

            customButtonMessage.Visible = false;//信息
        }

        //事件

        //----------------------------------------------------------------------
        // 功能说明：控件加载事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void DigitalKeyboard_Load(object sender, EventArgs e)
        {
            //不执行操作
        }
        
        //

        //-----------------------------------------------------------------------
        // 功能说明：点击【Esc】按钮操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonEsc_CustomButton_Click(object sender, EventArgs e)
        {
            bFirstInitialStringValue = false;

            customButtonMessage.Visible = false;//信息

            //

            bEnterNewValue = false;

            //事件

            if (null != Close_Click)//有效
            {
                Close_Click(this, new CustomEventArgs());
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：点击【Clr】按钮操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonClr_CustomButton_Click(object sender, EventArgs e)
        {
            bFirstInitialStringValue = false;

            sStringValue = "";
            fNumericalValue = 0;

            //

            labelDisplay.Text = sStringValue;

            //

            customButtonMessage.Visible = false;//信息
        }

        //-----------------------------------------------------------------------
        // 功能说明：点击【Backspace】按钮操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonBackspace_CustomButton_Click(object sender, EventArgs e)
        {
            bFirstInitialStringValue = false;

            if (0 < sStringValue.Length)//有效
            {
                sStringValue = sStringValue.Substring(0, sStringValue.Length - 1);//更新

                if ("" == sStringValue)//无效
                {
                    fNumericalValue = 0;
                }
                else//有效
                {
                    if ("-" == sStringValue)//负号，无效
                    {
                        sStringValue = "";
                        fNumericalValue = 0;
                    }
                    else//有效
                    {
                        fNumericalValue = Convert.ToSingle(sStringValue);
                    }
                }

                //

                labelDisplay.Text = sStringValue;
            }

            //

            customButtonMessage.Visible = false;//信息
        }

        //-----------------------------------------------------------------------
        // 功能说明：点击【Enter】按钮操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonEnter_CustomButton_Click(object sender, EventArgs e)
        {
            bFirstInitialStringValue = false;

            if ("" == sStringValue)//未输入
            {
                customButtonMessage.Visible = false;//信息

                //

                bEnterNewValue = false;

                //事件

                if (null != Close_Click)//有效
                {
                    Close_Click(this, new CustomEventArgs());
                }
            }
            else//输入
            {
                if (fNumericalValue == fNumericalValue_Original)//未改变
                {
                    customButtonMessage.Visible = false;//信息

                    //

                    bEnterNewValue = false;

                    //事件

                    if (null != Close_Click)//有效
                    {
                        Close_Click(this, new CustomEventArgs());
                    }
                }
                else//改变
                {
                    if (_CheckValue(fNumericalValue))//符合要求
                    {
                        customButtonMessage.Visible = false;//信息

                        //

                        bEnterNewValue = true;

                        //事件

                        if (null != Close_Click)//有效
                        {
                            Close_Click(this, new CustomEventArgs());
                        }
                    }
                    else//不符合要求
                    {
                        //sStringValue = "";
                        //fNumericalValue = 0;

                        //

                        customButtonMessage.Visible = true;//信息
                    }
                }
            }
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：点击【0】按钮操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButton0_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButton0);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：点击【1】按钮操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButton1_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButton1);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：点击【2】按钮操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButton2_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButton2);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：点击【3】按钮操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButton3_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButton3);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：点击【4】按钮操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButton4_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButton4);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：点击【5】按钮操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButton5_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButton5);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：点击【6】按钮操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButton6_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButton6);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：点击【7】按钮操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButton7_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButton7);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：点击【8】按钮操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButton8_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButton8);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：点击【9】按钮操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButton9_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickButton(customButton9);//点击按钮
        }

        //-----------------------------------------------------------------------
        // 功能说明：点击【.】按钮操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonDot_CustomButton_Click(object sender, EventArgs e)
        {
            try
            {
                _ClickButton(customButtonDot);//点击按钮
            }
            catch (System.Exception ex)
            {
            	//不执行操作
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：点击【+/-】按钮操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonPlusSubtract_CustomButton_Click(object sender, EventArgs e)
        {
            if ("" != sStringValue)//有效
            {
                if (0 > fNumericalValue)//负数
                {
                    sStringValue = sStringValue.Substring(1);
                }
                else//正数
                {
                    if (_CheckLength(sStringValue))//有效
                    {
                        sStringValue = "-" + sStringValue;
                    }
                }

                fNumericalValue = Convert.ToSingle(sStringValue);

                //

                labelDisplay.Text = sStringValue;
            }
        }
    }
}