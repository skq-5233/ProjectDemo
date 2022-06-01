/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：ParameterSettingsPanel.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：参数设置面板控件

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
using VisionSystemClassLibrary;

using System.Reflection;

namespace VisionSystemControlLibrary
{
    public partial class ParameterSettingsPanel : UserControl
    {
        //参数设置面板控件

        private VisionSystemClassLibrary.Enum.InterfaceLanguage language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;//属性，语言

        private Boolean bControlEnabled = true;//属性，控件是否使能。取值范围：true，是；false，否

        private Boolean bShowSettingsButton = true;//属性，是否显示设置按钮。取值范围：true，是；false，否

        //

        private Boolean bEnterValueEnabled = true;//属性，是否允许输入数值。取值范围：true，是；false，否

        //

        private Int32 iCurrentSelectedValueIndex = -1;//属性（只读），当前选择的参数控件索引值（从0开始，-1表示未选择任何控件）

        private CustomButton customButtonCurrentSelectedName = null;//当前选择的参数名称控件
        private CustomButton customButtonCurrentSelectedValue = null;//当前选择的参数数值控件

        //

        public const Int32 ParameterNumber = 30;//参数最大数目

        //

        private Int32 iValidParameterNumber = ParameterNumber;//当前有效的参数数目

        //

        private Boolean[] bParameterEnabled = new Boolean[ParameterNumber];//属性，参数使能状态。取值范围：true，使能；false，禁止

        private Int32[] iParameterType = new Int32[ParameterNumber];//属性，参数类型。取值范围：1，枚举类型；2，数字类型

        private String[][] sParameterName = new String[2][];//属性，参数名称（[语言][参数]）

        private Int32[] iParameterPrecision = new Int32[ParameterNumber];//属性，参数精度
        private Single[] fParameterCurrentValue = new Single[ParameterNumber];//属性，参数当前值
        private Single[] fParameterMinValue = new Single[ParameterNumber];//属性，参数最小值（参数类型取值为2时有效）
        private Single[] fParameterMaxValue = new Single[ParameterNumber];//属性，参数最大值（参数类型取值为2时有效）

        public Int32[][] ParameterValue = new Int32[ParameterNumber][];//参数数值（参数类型取值为1时有效）
        public Boolean[][] ParameterValueEnabled = new Boolean[ParameterNumber][];//参数数值使能情况。取值范围：true，使能；false，禁止（参数类型取值为1时有效）
        private String[][][] sParameterValueNameArray = new String[2][][];//参数数值名称（[语言][参数][包含的文本]，参数类型取值为1时有效）
        private String[][] sParameterValueNameDisplay = new String[2][];//属性，原始参数数值名称（[语言][参数]，参数类型取值为1时有效）

        //

        private Int32 iNameBottom_SetTop_Height = 0;//最后一个名称控件底部和设置按钮控件上部的距离
        private Int32 iSetBottom_ControlBottom_Height = 0;//设置按钮控件底部和控件底部的距离

        //

        private Int32 iSetBottom_NextPageTop_Height = 0;//设置按钮控件底部和翻页按钮控件上部的距离
        private Int32 iNextPageBottom_ControlBottom_Height = 0;//翻页按钮控件底部和控件底部的距离

        //

        private Int32 iWindowParameter = 0;//属性，调用的键盘窗口特征数值（由于该控件具有多个实例，因此在设置属性值时进行事件的订阅。若在构造函数中进行订阅，则会出现重复的情况，导致事件多次响应）。取值范围：
        //1.QUALITY CHECK，工具参数修改
        //2.IMAGE CONFIGURATION，工具参数修改
        //3.STATISTICS，工具参数信息

        //

        [Browsable(true), Description("参数值发生改变的事件"), Category("ParameterSettingsPanel 事件")]
        public event EventHandler ParameterValueChanged;

        private int iTotalPage = 1;//属性，包含的页码总数
        private int iCurrentPage = 0;//属性，当前页码（从0开始）

        private int iNumberPerPage = 10;//属性，每页显示数量

        private Int32 customButtonName_1_X = 4;
        private Int32 customButtonName_1_Y = 26;
        private Int32 customButtonValue_1_X = 105;
        private Int32 customButtonValue_1_Y = 25;
        private Int32 customButtonValueBackground_1_X = 102;
        private Int32 customButtonValueBackground_1_Y = 22;

        private Int32 Axis_Y = 32;

        //构造函数

        //----------------------------------------------------------------------
        // 功能说明：构造函数，初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public ParameterSettingsPanel()
        {
            InitializeComponent();

            //

            Int32 i = 0;//循环控制变量
            Int32 j = 0;//循环控制变量

            for (i = 0; i < bParameterEnabled.Length; i++)
            {
                bParameterEnabled[i] = true;//属性，参数使能状态。取值范围：true，使能；false，禁止
            }

            for (i = 0; i < iParameterType.Length; i++)
            {
                iParameterType[i] = 2;//属性，参数类型。取值范围：1，枚举类型；2，数字类型
            }

            for (i = 0; i < iParameterPrecision.Length; i++)
            {
                iParameterPrecision[i] = 0;//属性，参数精度
            }

            for (i = 0; i < fParameterCurrentValue.Length; i++)
            {
                fParameterCurrentValue[i] = 50;//属性，参数当前值
            }

            for (i = 0; i < fParameterMinValue.Length; i++)
            {
                fParameterMinValue[i] = 0;//属性，参数最小值
            }

            for (i = 0; i < fParameterMaxValue.Length; i++)
            {
                fParameterMaxValue[i] = 100;//属性，参数最大值
            }

            //

            FieldInfo[] fieldinfo = VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese.GetType().GetFields();//获取公共字段
            if (null != fieldinfo && 1 < fieldinfo.Length)//有效
            {
                sParameterName = new String[fieldinfo.Length - 1][];//属性，参数名称（[语言][参数]）
                sParameterValueNameArray = new String[fieldinfo.Length - 1][][];//参数数值名称（[语言][参数][包含的文本]，参数类型取值为1时有效）
                sParameterValueNameDisplay = new String[fieldinfo.Length - 1][];//属性，原始参数数值名称（[语言][参数]，参数类型取值为1时有效）

                //

                for (i = 0; i < fieldinfo.Length - 1; i++)
                {
                    sParameterName[i] = new String[ParameterNumber];
                    for (j = 0; j < sParameterName.Length; j++)
                    {
                        sParameterName[i][j] = "";
                    }

                    sParameterValueNameArray[i] = new String[ParameterNumber][];
                    for (j = 0; j < sParameterValueNameArray[i].Length; j++)
                    {
                        sParameterValueNameArray[i][j] = new String[1];

                        sParameterValueNameArray[i][j][0] = "";
                    }

                    sParameterValueNameDisplay[i] = new String[ParameterNumber];
                    for (j = 0; j < sParameterValueNameDisplay.Length; j++)
                    {
                        sParameterValueNameDisplay[i][j] = "";
                    }

                }
            }

            //

            customButtonName_1.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonName_1.English_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_1.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_1.English_TextDisplay = new String[1] { " " };//设置显示的文本

            customButtonName_1.Location = new System.Drawing.Point(customButtonName_1_X, customButtonName_1_Y);
            customButtonValue_1.Location = new System.Drawing.Point(customButtonValue_1_X, customButtonValue_1_Y);
            customButtonValueBackground_1.Location = new System.Drawing.Point(customButtonValueBackground_1_X, customButtonValueBackground_1_Y);
            customButtonValue_1.BringToFront();

            customButtonName_2.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonName_2.English_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_2.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_2.English_TextDisplay = new String[1] { " " };//设置显示的文本

            customButtonName_1_Y += Axis_Y;
            customButtonValue_1_Y += Axis_Y;
            customButtonValueBackground_1_Y += Axis_Y;
            customButtonName_2.Location = new System.Drawing.Point(customButtonName_1_X, customButtonName_1_Y);
            customButtonValue_2.Location = new System.Drawing.Point(customButtonValue_1_X, customButtonValue_1_Y);
            customButtonValueBackground_2.Location = new System.Drawing.Point(customButtonValueBackground_1_X, customButtonValueBackground_1_Y);
            customButtonValue_2.BringToFront();

            customButtonName_3.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonName_3.English_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_3.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_3.English_TextDisplay = new String[1] { " " };//设置显示的文本

            customButtonName_1_Y += Axis_Y;
            customButtonValue_1_Y += Axis_Y;
            customButtonValueBackground_1_Y += Axis_Y;
            customButtonName_3.Location = new System.Drawing.Point(customButtonName_1_X, customButtonName_1_Y);
            customButtonValue_3.Location = new System.Drawing.Point(customButtonValue_1_X, customButtonValue_1_Y);
            customButtonValueBackground_3.Location = new System.Drawing.Point(customButtonValueBackground_1_X, customButtonValueBackground_1_Y);
            customButtonValue_3.BringToFront();

            customButtonName_4.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonName_4.English_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_4.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_4.English_TextDisplay = new String[1] { " " };//设置显示的文本

            customButtonName_1_Y += Axis_Y;
            customButtonValue_1_Y += Axis_Y;
            customButtonValueBackground_1_Y += Axis_Y;
            customButtonName_4.Location = new System.Drawing.Point(customButtonName_1_X, customButtonName_1_Y);
            customButtonValue_4.Location = new System.Drawing.Point(customButtonValue_1_X, customButtonValue_1_Y);
            customButtonValueBackground_4.Location = new System.Drawing.Point(customButtonValueBackground_1_X, customButtonValueBackground_1_Y);
            customButtonValue_4.BringToFront();

            customButtonName_5.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonName_5.English_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_5.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_5.English_TextDisplay = new String[1] { " " };//设置显示的文本

            customButtonName_1_Y += Axis_Y;
            customButtonValue_1_Y += Axis_Y;
            customButtonValueBackground_1_Y += Axis_Y;
            customButtonName_5.Location = new System.Drawing.Point(customButtonName_1_X, customButtonName_1_Y);
            customButtonValue_5.Location = new System.Drawing.Point(customButtonValue_1_X, customButtonValue_1_Y);
            customButtonValueBackground_5.Location = new System.Drawing.Point(customButtonValueBackground_1_X, customButtonValueBackground_1_Y);
            customButtonValue_5.BringToFront();

            customButtonName_6.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonName_6.English_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_6.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_6.English_TextDisplay = new String[1] { " " };//设置显示的文本

            customButtonName_1_Y += Axis_Y;
            customButtonValue_1_Y += Axis_Y;
            customButtonValueBackground_1_Y += Axis_Y;
            customButtonName_6.Location = new System.Drawing.Point(customButtonName_1_X, customButtonName_1_Y);
            customButtonValue_6.Location = new System.Drawing.Point(customButtonValue_1_X, customButtonValue_1_Y);
            customButtonValueBackground_6.Location = new System.Drawing.Point(customButtonValueBackground_1_X, customButtonValueBackground_1_Y);
            customButtonValue_6.BringToFront();

            customButtonName_7.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonName_7.English_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_7.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_7.English_TextDisplay = new String[1] { " " };//设置显示的文本

            customButtonName_1_Y += Axis_Y;
            customButtonValue_1_Y += Axis_Y;
            customButtonValueBackground_1_Y += Axis_Y;
            customButtonName_7.Location = new System.Drawing.Point(customButtonName_1_X, customButtonName_1_Y);
            customButtonValue_7.Location = new System.Drawing.Point(customButtonValue_1_X, customButtonValue_1_Y);
            customButtonValueBackground_7.Location = new System.Drawing.Point(customButtonValueBackground_1_X, customButtonValueBackground_1_Y);
            customButtonValue_7.BringToFront();

            customButtonName_8.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonName_8.English_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_8.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_8.English_TextDisplay = new String[1] { " " };//设置显示的文本

            customButtonName_1_Y += Axis_Y;
            customButtonValue_1_Y += Axis_Y;
            customButtonValueBackground_1_Y += Axis_Y;
            customButtonName_8.Location = new System.Drawing.Point(customButtonName_1_X, customButtonName_1_Y);
            customButtonValue_8.Location = new System.Drawing.Point(customButtonValue_1_X, customButtonValue_1_Y);
            customButtonValueBackground_8.Location = new System.Drawing.Point(customButtonValueBackground_1_X, customButtonValueBackground_1_Y);
            customButtonValue_8.BringToFront();

            customButtonName_9.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonName_9.English_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_9.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_9.English_TextDisplay = new String[1] { " " };//设置显示的文本

            customButtonName_1_Y += Axis_Y;
            customButtonValue_1_Y += Axis_Y;
            customButtonValueBackground_1_Y += Axis_Y;
            customButtonName_9.Location = new System.Drawing.Point(customButtonName_1_X, customButtonName_1_Y);
            customButtonValue_9.Location = new System.Drawing.Point(customButtonValue_1_X, customButtonValue_1_Y);
            customButtonValueBackground_9.Location = new System.Drawing.Point(customButtonValueBackground_1_X, customButtonValueBackground_1_Y);
            customButtonValue_9.BringToFront();

            customButtonName_10.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonName_10.English_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_10.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_10.English_TextDisplay = new String[1] { " " };//设置显示的文本

            customButtonName_1_Y += Axis_Y;
            customButtonValue_1_Y += Axis_Y;
            customButtonValueBackground_1_Y += Axis_Y;
            customButtonName_10.Location = new System.Drawing.Point(customButtonName_1_X, customButtonName_1_Y);
            customButtonValue_10.Location = new System.Drawing.Point(customButtonValue_1_X, customButtonValue_1_Y);
            customButtonValueBackground_10.Location = new System.Drawing.Point(customButtonValueBackground_1_X, customButtonValueBackground_1_Y);
            customButtonValue_10.BringToFront();

            customButtonName_11.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonName_11.English_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_11.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_11.English_TextDisplay = new String[1] { " " };//设置显示的文本

            customButtonName_1_Y = 26;
            customButtonValue_1_Y = 25;
            customButtonValueBackground_1_Y = 22;
            customButtonName_11.Location = new System.Drawing.Point(customButtonName_1_X, customButtonName_1_Y);
            customButtonValue_11.Location = new System.Drawing.Point(customButtonValue_1_X, customButtonValue_1_Y);
            customButtonValueBackground_11.Location = new System.Drawing.Point(customButtonValueBackground_1_X, customButtonValueBackground_1_Y);
            customButtonValue_11.BringToFront();

            customButtonName_12.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonName_12.English_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_12.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_12.English_TextDisplay = new String[1] { " " };//设置显示的文本

            customButtonName_1_Y += Axis_Y;
            customButtonValue_1_Y += Axis_Y;
            customButtonValueBackground_1_Y += Axis_Y;
            customButtonName_12.Location = new System.Drawing.Point(customButtonName_1_X, customButtonName_1_Y);
            customButtonValue_12.Location = new System.Drawing.Point(customButtonValue_1_X, customButtonValue_1_Y);
            customButtonValueBackground_12.Location = new System.Drawing.Point(customButtonValueBackground_1_X, customButtonValueBackground_1_Y);
            customButtonValue_12.BringToFront();

            customButtonName_13.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonName_13.English_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_13.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_13.English_TextDisplay = new String[1] { " " };//设置显示的文本

            customButtonName_1_Y += Axis_Y;
            customButtonValue_1_Y += Axis_Y;
            customButtonValueBackground_1_Y += Axis_Y;
            customButtonName_13.Location = new System.Drawing.Point(customButtonName_1_X, customButtonName_1_Y);
            customButtonValue_13.Location = new System.Drawing.Point(customButtonValue_1_X, customButtonValue_1_Y);
            customButtonValueBackground_13.Location = new System.Drawing.Point(customButtonValueBackground_1_X, customButtonValueBackground_1_Y);
            customButtonValue_13.BringToFront();

            customButtonName_14.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonName_14.English_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_14.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_14.English_TextDisplay = new String[1] { " " };//设置显示的文本

            customButtonName_1_Y += Axis_Y;
            customButtonValue_1_Y += Axis_Y;
            customButtonValueBackground_1_Y += Axis_Y;
            customButtonName_14.Location = new System.Drawing.Point(customButtonName_1_X, customButtonName_1_Y);
            customButtonValue_14.Location = new System.Drawing.Point(customButtonValue_1_X, customButtonValue_1_Y);
            customButtonValueBackground_14.Location = new System.Drawing.Point(customButtonValueBackground_1_X, customButtonValueBackground_1_Y);
            customButtonValue_14.BringToFront();

            customButtonName_15.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonName_15.English_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_15.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_15.English_TextDisplay = new String[1] { " " };//设置显示的文本

            customButtonName_1_Y += Axis_Y;
            customButtonValue_1_Y += Axis_Y;
            customButtonValueBackground_1_Y += Axis_Y;
            customButtonName_15.Location = new System.Drawing.Point(customButtonName_1_X, customButtonName_1_Y);
            customButtonValue_15.Location = new System.Drawing.Point(customButtonValue_1_X, customButtonValue_1_Y);
            customButtonValueBackground_15.Location = new System.Drawing.Point(customButtonValueBackground_1_X, customButtonValueBackground_1_Y);
            customButtonValue_15.BringToFront();

            customButtonName_16.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonName_16.English_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_16.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_16.English_TextDisplay = new String[1] { " " };//设置显示的文本

            customButtonName_1_Y += Axis_Y;
            customButtonValue_1_Y += Axis_Y;
            customButtonValueBackground_1_Y += Axis_Y;
            customButtonName_16.Location = new System.Drawing.Point(customButtonName_1_X, customButtonName_1_Y);
            customButtonValue_16.Location = new System.Drawing.Point(customButtonValue_1_X, customButtonValue_1_Y);
            customButtonValueBackground_16.Location = new System.Drawing.Point(customButtonValueBackground_1_X, customButtonValueBackground_1_Y);
            customButtonValue_16.BringToFront();

            customButtonName_17.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonName_17.English_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_17.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_17.English_TextDisplay = new String[1] { " " };//设置显示的文本

            customButtonName_1_Y += Axis_Y;
            customButtonValue_1_Y += Axis_Y;
            customButtonValueBackground_1_Y += Axis_Y;
            customButtonName_17.Location = new System.Drawing.Point(customButtonName_1_X, customButtonName_1_Y);
            customButtonValue_17.Location = new System.Drawing.Point(customButtonValue_1_X, customButtonValue_1_Y);
            customButtonValueBackground_17.Location = new System.Drawing.Point(customButtonValueBackground_1_X, customButtonValueBackground_1_Y);
            customButtonValue_17.BringToFront();

            customButtonName_18.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonName_18.English_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_18.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_18.English_TextDisplay = new String[1] { " " };//设置显示的文本

            customButtonName_1_Y += Axis_Y;
            customButtonValue_1_Y += Axis_Y;
            customButtonValueBackground_1_Y += Axis_Y;
            customButtonName_18.Location = new System.Drawing.Point(customButtonName_1_X, customButtonName_1_Y);
            customButtonValue_18.Location = new System.Drawing.Point(customButtonValue_1_X, customButtonValue_1_Y);
            customButtonValueBackground_18.Location = new System.Drawing.Point(customButtonValueBackground_1_X, customButtonValueBackground_1_Y);
            customButtonValue_18.BringToFront();

            customButtonName_19.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonName_19.English_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_19.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_19.English_TextDisplay = new String[1] { " " };//设置显示的文本

            customButtonName_1_Y += Axis_Y;
            customButtonValue_1_Y += Axis_Y;
            customButtonValueBackground_1_Y += Axis_Y;
            customButtonName_19.Location = new System.Drawing.Point(customButtonName_1_X, customButtonName_1_Y);
            customButtonValue_19.Location = new System.Drawing.Point(customButtonValue_1_X, customButtonValue_1_Y);
            customButtonValueBackground_19.Location = new System.Drawing.Point(customButtonValueBackground_1_X, customButtonValueBackground_1_Y);
            customButtonValue_19.BringToFront();

            customButtonName_20.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonName_20.English_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_20.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_20.English_TextDisplay = new String[1] { " " };//设置显示的文本

            customButtonName_1_Y += Axis_Y;
            customButtonValue_1_Y += Axis_Y;
            customButtonValueBackground_1_Y += Axis_Y;
            customButtonName_20.Location = new System.Drawing.Point(customButtonName_1_X, customButtonName_1_Y);
            customButtonValue_20.Location = new System.Drawing.Point(customButtonValue_1_X, customButtonValue_1_Y);
            customButtonValueBackground_20.Location = new System.Drawing.Point(customButtonValueBackground_1_X, customButtonValueBackground_1_Y);
            customButtonValue_20.BringToFront();

            customButtonName_21.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonName_21.English_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_21.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_21.English_TextDisplay = new String[1] { " " };//设置显示的文本

            customButtonName_1_Y = 26;
            customButtonValue_1_Y = 25;
            customButtonValueBackground_1_Y = 22;
            customButtonName_21.Location = new System.Drawing.Point(customButtonName_1_X, customButtonName_1_Y);
            customButtonValue_21.Location = new System.Drawing.Point(customButtonValue_1_X, customButtonValue_1_Y);
            customButtonValueBackground_21.Location = new System.Drawing.Point(customButtonValueBackground_1_X, customButtonValueBackground_1_Y);
            customButtonValue_21.BringToFront();

            customButtonName_22.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonName_22.English_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_22.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_22.English_TextDisplay = new String[1] { " " };//设置显示的文本

            customButtonName_1_Y += Axis_Y;
            customButtonValue_1_Y += Axis_Y;
            customButtonValueBackground_1_Y += Axis_Y;
            customButtonName_22.Location = new System.Drawing.Point(customButtonName_1_X, customButtonName_1_Y);
            customButtonValue_22.Location = new System.Drawing.Point(customButtonValue_1_X, customButtonValue_1_Y);
            customButtonValueBackground_22.Location = new System.Drawing.Point(customButtonValueBackground_1_X, customButtonValueBackground_1_Y);
            customButtonValue_22.BringToFront();

            customButtonName_23.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonName_23.English_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_23.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_23.English_TextDisplay = new String[1] { " " };//设置显示的文本

            customButtonName_1_Y += Axis_Y;
            customButtonValue_1_Y += Axis_Y;
            customButtonValueBackground_1_Y += Axis_Y;
            customButtonName_23.Location = new System.Drawing.Point(customButtonName_1_X, customButtonName_1_Y);
            customButtonValue_23.Location = new System.Drawing.Point(customButtonValue_1_X, customButtonValue_1_Y);
            customButtonValueBackground_23.Location = new System.Drawing.Point(customButtonValueBackground_1_X, customButtonValueBackground_1_Y);
            customButtonValue_23.BringToFront();

            customButtonName_24.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonName_24.English_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_24.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_24.English_TextDisplay = new String[1] { " " };//设置显示的文本

            customButtonName_1_Y += Axis_Y;
            customButtonValue_1_Y += Axis_Y;
            customButtonValueBackground_1_Y += Axis_Y;
            customButtonName_24.Location = new System.Drawing.Point(customButtonName_1_X, customButtonName_1_Y);
            customButtonValue_24.Location = new System.Drawing.Point(customButtonValue_1_X, customButtonValue_1_Y);
            customButtonValueBackground_24.Location = new System.Drawing.Point(customButtonValueBackground_1_X, customButtonValueBackground_1_Y);
            customButtonValue_24.BringToFront();

            customButtonName_25.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonName_25.English_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_25.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_25.English_TextDisplay = new String[1] { " " };//设置显示的文本

            customButtonName_1_Y += Axis_Y;
            customButtonValue_1_Y += Axis_Y;
            customButtonValueBackground_1_Y += Axis_Y;
            customButtonName_25.Location = new System.Drawing.Point(customButtonName_1_X, customButtonName_1_Y);
            customButtonValue_25.Location = new System.Drawing.Point(customButtonValue_1_X, customButtonValue_1_Y);
            customButtonValueBackground_25.Location = new System.Drawing.Point(customButtonValueBackground_1_X, customButtonValueBackground_1_Y);
            customButtonValue_25.BringToFront();

            customButtonName_26.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonName_26.English_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_26.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_26.English_TextDisplay = new String[1] { " " };//设置显示的文本

            customButtonName_1_Y += Axis_Y;
            customButtonValue_1_Y += Axis_Y;
            customButtonValueBackground_1_Y += Axis_Y;
            customButtonName_26.Location = new System.Drawing.Point(customButtonName_1_X, customButtonName_1_Y);
            customButtonValue_26.Location = new System.Drawing.Point(customButtonValue_1_X, customButtonValue_1_Y);
            customButtonValueBackground_26.Location = new System.Drawing.Point(customButtonValueBackground_1_X, customButtonValueBackground_1_Y);
            customButtonValue_26.BringToFront();

            customButtonName_27.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonName_27.English_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_27.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_27.English_TextDisplay = new String[1] { " " };//设置显示的文本

            customButtonName_1_Y += Axis_Y;
            customButtonValue_1_Y += Axis_Y;
            customButtonValueBackground_1_Y += Axis_Y;
            customButtonName_27.Location = new System.Drawing.Point(customButtonName_1_X, customButtonName_1_Y);
            customButtonValue_27.Location = new System.Drawing.Point(customButtonValue_1_X, customButtonValue_1_Y);
            customButtonValueBackground_27.Location = new System.Drawing.Point(customButtonValueBackground_1_X, customButtonValueBackground_1_Y);
            customButtonValue_27.BringToFront();

            customButtonName_28.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonName_28.English_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_28.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_28.English_TextDisplay = new String[1] { " " };//设置显示的文本

            customButtonName_1_Y += Axis_Y;
            customButtonValue_1_Y += Axis_Y;
            customButtonValueBackground_1_Y += Axis_Y;
            customButtonName_28.Location = new System.Drawing.Point(customButtonName_1_X, customButtonName_1_Y);
            customButtonValue_28.Location = new System.Drawing.Point(customButtonValue_1_X, customButtonValue_1_Y);
            customButtonValueBackground_28.Location = new System.Drawing.Point(customButtonValueBackground_1_X, customButtonValueBackground_1_Y);
            customButtonValue_28.BringToFront();

            customButtonName_29.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonName_29.English_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_29.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_29.English_TextDisplay = new String[1] { " " };//设置显示的文本

            customButtonName_1_Y += Axis_Y;
            customButtonValue_1_Y += Axis_Y;
            customButtonValueBackground_1_Y += Axis_Y;
            customButtonName_29.Location = new System.Drawing.Point(customButtonName_1_X, customButtonName_1_Y);
            customButtonValue_29.Location = new System.Drawing.Point(customButtonValue_1_X, customButtonValue_1_Y);
            customButtonValueBackground_29.Location = new System.Drawing.Point(customButtonValueBackground_1_X, customButtonValueBackground_1_Y);
            customButtonValue_29.BringToFront();

            customButtonName_30.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonName_30.English_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_30.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonValue_30.English_TextDisplay = new String[1] { " " };//设置显示的文本

            customButtonName_1_Y += Axis_Y;
            customButtonValue_1_Y += Axis_Y;
            customButtonValueBackground_1_Y += Axis_Y;
            customButtonName_30.Location = new System.Drawing.Point(customButtonName_1_X, customButtonName_1_Y);
            customButtonValue_30.Location = new System.Drawing.Point(customButtonValue_1_X, customButtonValue_1_Y);
            customButtonValueBackground_30.Location = new System.Drawing.Point(customButtonValueBackground_1_X, customButtonValueBackground_1_Y);
            customButtonValue_30.BringToFront();

            //

            iNameBottom_SetTop_Height = 13;//最后一个名称控件底部和设置按钮控件上部的距离
            iSetBottom_ControlBottom_Height = 10;//设置按钮控件底部和控件底部的距离

            iSetBottom_NextPageTop_Height = 13;//设置按钮控件底部和翻页按钮控件上部的距离
            iNextPageBottom_ControlBottom_Height = 10;//翻页按钮控件底部和控件底部的距离
        }

        //属性

        //----------------------------------------------------------------------
        // 功能说明：WindowParameter属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("调用的键盘窗口特征数值"), Category("ParameterSettingsPanel 通用")]
        public Int32 WindowParameter//属性
        {
            get//读取
            {
                return iWindowParameter;
            }
            set//设置
            {
                iWindowParameter = value;

                //

                try
                {
                    if (1 == iWindowParameter)//QUALITY CHECK，工具参数修改
                    {
                        GlobalWindows.DigitalKeyboard_Window.WindowClose_QualityCheck_ToolParameter += new System.EventHandler(digitalKeyboardWindow_WindowClose_QualityCheck_ToolParameter);//订阅事件
                    }
                    else if (2 == iWindowParameter)//IMAGE CONFIGURATION，工具参数修改
                    {
                        GlobalWindows.DigitalKeyboard_Window.WindowClose_ImageConfiguration_ToolParameter += new System.EventHandler(digitalKeyboardWindow_WindowClose_ImageConfiguration_ToolParameter);//订阅事件
                    }
                    else if (3 == iWindowParameter)//STATISTICS，工具参数信息
                    {
                        //不执行操作
                    }
                    else//其它
                    {
                        //不执行操作
                    }
                }
                catch (System.Exception ex)
                {
                    //不执行操作
                }
            }
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：Language属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("语言"), Category("ParameterSettingsPanel 通用")]
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

                    _SetLanguage();//设置语言
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ControlEnabled属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("控件是否使能。取值范围：true，是；false，否"), Category("ParameterSettingsPanel 通用")]
        public Boolean ControlEnabled//属性
        {
            get//读取
            {
                return bControlEnabled;
            }
            set//设置
            {
                if (value != bControlEnabled)
                {
                    bControlEnabled = value;

                    //

                    _SetControlEnabled();//设置控件的使能状态
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ControlEnabled属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("是否显示设置按钮。取值范围：true，是；false，否"), Category("ParameterSettingsPanel 通用")]
        public Boolean ShowSettingsButton//属性
        {
            get//读取
            {
                return bShowSettingsButton;
            }
            set//设置
            {
                if (value != bShowSettingsButton)
                {
                    bShowSettingsButton = value;
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：EnterValueEnabled属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("是否允许输入数值"), Category("ParameterSettingsPanel 通用")]
        public Boolean EnterValueEnabled
        {
            get//读取
            {
                return bEnterValueEnabled;
            }
            set//设置
            {
                if (value != bEnterValueEnabled)
                {
                    bEnterValueEnabled = value;

                    //

                    if (bEnterValueEnabled)//允许输入数值
                    {
                        if (bControlEnabled)//控件使能
                        {
                            customButtonSubtract.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【-】
                            customButtonSet.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【Set】
                            customButtonPlus.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【+】
                        }
                    }
                    else//不允许输入数值
                    {
                        customButtonSubtract.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【-】
                        customButtonSet.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【Set】
                        customButtonPlus.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【+】
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：CurrentSelectedValueIndex属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("当前选择的参数控件索引值（从0开始，-1表示未选择任何控件）"), Category("ParameterSettingsPanel 通用")]
        public Int32 CurrentSelectedValueIndex//属性
        {
            get//读取
            {
                return iCurrentSelectedValueIndex;
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：ParameterEnabled属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("参数使能状态。取值范围：true，使能；false，禁止"), Category("ParameterSettingsPanel 通用")]
        public Boolean[] ParameterEnabled//属性
        {
            get//读取
            {
                return bParameterEnabled;
            }
            set//设置
            {
                if (null != value)
                {
                    if (value.Length <= ParameterNumber)//范围之内
                    {
                        Array.Copy(value, 0, bParameterEnabled, 0, value.Length);

                        iValidParameterNumber = value.Length;
                    }
                    else//超出范围
                    {
                        Array.Copy(value, 0, bParameterEnabled, 0, ParameterNumber);

                        iValidParameterNumber = ParameterNumber;
                    }

                    if (value.Length > ParameterNumber - iNumberPerPage) //有效数据大于20 
                    {
                        iTotalPage = 3;
                    }
                    else if (value.Length > ParameterNumber - 2 * iNumberPerPage) //有效数据大于10 
                    {
                        iTotalPage = 2;
                    }
                    else
                    {
                        iTotalPage = 1;
                    }

                    //

                    _SetControlEnabled();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ParameterType属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("参数类型。取值范围：1，枚举类型；2，数字类型"), Category("ParameterSettingsPanel 通用")]
        public Int32[] ParameterType//属性
        {
            get//读取
            {
                return iParameterType;
            }
            set//设置
            {
                if (null != value)
                {
                    if (value.Length <= ParameterNumber)//范围之内
                    {
                        Array.Copy(value, 0, iParameterType, 0, value.Length);

                        iValidParameterNumber = value.Length;
                    }
                    else//超出范围
                    {
                        Array.Copy(value, 0, iParameterType, 0, ParameterNumber);

                        iValidParameterNumber = ParameterNumber;
                    }

                    if (value.Length > ParameterNumber - iNumberPerPage) //有效数据大于20 
                    {
                        iTotalPage = 3;
                    }
                    else if (value.Length > ParameterNumber - 2 * iNumberPerPage) //有效数据大于10 
                    {
                        iTotalPage = 2;
                    }
                    else
                    {
                        iTotalPage = 1;
                    }

                    //

                    //_AddValues();//添加数据
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：Chinese_ParameterName属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("参数中文名称（[语言][参数]）"), Category("ParameterSettingsPanel 通用")]
        public String[] Chinese_ParameterName//属性
        {
            get//读取
            {
                return sParameterName[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1];
            }
            set//设置
            {
                if (null != value)
                {
                    if (value.Length <= ParameterNumber)//范围之内
                    {
                        Array.Copy(value, 0, sParameterName[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1], 0, value.Length);

                        iValidParameterNumber = value.Length;
                    }
                    else//超出范围
                    {
                        Array.Copy(value, 0, sParameterName[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1], 0, ParameterNumber);

                        iValidParameterNumber = ParameterNumber;
                    }

                    if (value.Length > ParameterNumber - iNumberPerPage) //有效数据大于20 
                    {
                        iTotalPage = 3;
                    }
                    else if (value.Length > ParameterNumber - 2 * iNumberPerPage) //有效数据大于10 
                    {
                        iTotalPage = 2;
                    }
                    else
                    {
                        iTotalPage = 1;
                    }

                    //

                    //_AddValues();//添加数据
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：English_ParameterName属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("参数英文名称（[语言][参数]）"), Category("ParameterSettingsPanel 通用")]
        public String[] English_ParameterName//属性
        {
            get//读取
            {
                return sParameterName[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1];
            }
            set//设置
            {
                if (null != value)
                {
                    if (value.Length <= ParameterNumber)//范围之内
                    {
                        Array.Copy(value, 0, sParameterName[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1], 0, value.Length);

                        iValidParameterNumber = value.Length;
                    }
                    else//超出范围
                    {
                        Array.Copy(value, 0, sParameterName[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1], 0, ParameterNumber);

                        iValidParameterNumber = ParameterNumber;
                    }

                    if (value.Length > ParameterNumber - iNumberPerPage) //有效数据大于20 
                    {
                        iTotalPage = 3;
                    }
                    else if (value.Length > ParameterNumber - 2 * iNumberPerPage) //有效数据大于10 
                    {
                        iTotalPage = 2;
                    }
                    else
                    {
                        iTotalPage = 1;
                    }

                    //

                    //_AddValues();//添加数据
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ParameterPrecision属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("参数精度"), Category("ParameterSettingsPanel 通用")]
        public Int32[] ParameterPrecision//属性
        {
            get//读取
            {
                return iParameterPrecision;
            }
            set//设置
            {
                if (null != value)
                {
                    if (value.Length <= ParameterNumber)//范围之内
                    {
                        Array.Copy(value, 0, iParameterPrecision, 0, value.Length);

                        iValidParameterNumber = value.Length;
                    }
                    else//超出范围
                    {
                        Array.Copy(value, 0, iParameterPrecision, 0, ParameterNumber);

                        iValidParameterNumber = ParameterNumber;
                    }

                    if (value.Length > ParameterNumber - iNumberPerPage) //有效数据大于20 
                    {
                        iTotalPage = 3;
                    }
                    else if (value.Length > ParameterNumber - 2 * iNumberPerPage) //有效数据大于10 
                    {
                        iTotalPage = 2;
                    }
                    else
                    {
                        iTotalPage = 1;
                    }

                    //

                    //_AddValues();//添加数据
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ParameterCurrentValue属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("参数当前值"), Category("ParameterSettingsPanel 通用")]
        public Single[] ParameterCurrentValue//属性
        {
            get//读取
            {
                return fParameterCurrentValue;
            }
            set//设置
            {
                if (null != value)
                {
                    if (value.Length <= ParameterNumber)//范围之内
                    {
                        Array.Copy(value, 0, fParameterCurrentValue, 0, value.Length);

                        iValidParameterNumber = value.Length;
                    }
                    else//超出范围
                    {
                        Array.Copy(value, 0, fParameterCurrentValue, 0, ParameterNumber);

                        iValidParameterNumber = ParameterNumber;
                    }

                    if (value.Length > ParameterNumber - iNumberPerPage) //有效数据大于20 
                    {
                        iTotalPage = 3;
                    }
                    else if (value.Length > ParameterNumber - 2 * iNumberPerPage) //有效数据大于10 
                    {
                        iTotalPage = 2;
                    }
                    else
                    {
                        iTotalPage = 1;
                    }

                    //

                    //_AddValues();//添加数据
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ParameterMinValue属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("参数最小值"), Category("ParameterSettingsPanel 通用")]
        public Single[] ParameterMinValue//属性
        {
            get//读取
            {
                return fParameterMinValue;
            }
            set//设置
            {
                if (null != value)
                {
                    if (value.Length <= ParameterNumber)//范围之内
                    {
                        Array.Copy(value, 0, fParameterMinValue, 0, value.Length);

                        iValidParameterNumber = value.Length;
                    }
                    else//超出范围
                    {
                        Array.Copy(value, 0, fParameterMinValue, 0, ParameterNumber);

                        iValidParameterNumber = ParameterNumber;
                    }

                    if (value.Length > ParameterNumber - iNumberPerPage) //有效数据大于20 
                    {
                        iTotalPage = 3;
                    }
                    else if (value.Length > ParameterNumber - 2 * iNumberPerPage) //有效数据大于10 
                    {
                        iTotalPage = 2;
                    }
                    else
                    {
                        iTotalPage = 1;
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ParameterMaxValue属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("参数最大值"), Category("ParameterSettingsPanel 通用")]
        public Single[] ParameterMaxValue//属性
        {
            get//读取
            {
                return fParameterMaxValue;
            }
            set//设置
            {
                if (null != value)
                {
                    if (value.Length <= ParameterNumber)//范围之内
                    {
                        Array.Copy(value, 0, fParameterMaxValue, 0, value.Length);

                        iValidParameterNumber = value.Length;
                    }
                    else//超出范围
                    {
                        Array.Copy(value, 0, fParameterMaxValue, 0, ParameterNumber);

                        iValidParameterNumber = ParameterNumber;
                    }

                    if (value.Length > ParameterNumber - iNumberPerPage) //有效数据大于20 
                    {
                        iTotalPage = 3;
                    }
                    else if (value.Length > ParameterNumber - 2 * iNumberPerPage) //有效数据大于10 
                    {
                        iTotalPage = 2;
                    }
                    else
                    {
                        iTotalPage = 1;
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：Chinese_ParameterValueNameDisplay属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("原始参数数值中文名称（[语言][参数]，参数类型取值为1时有效）"), Category("ParameterSettingsPanel 通用")]
        public String[] Chinese_ParameterValueNameDisplay//属性
        {
            get//读取
            {
                return sParameterValueNameDisplay[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1];
            }
            set//设置
            {
                if (null != value)
                {
                    if (value.Length <= ParameterNumber)//范围之内
                    {
                        Array.Copy(value, 0, sParameterValueNameDisplay[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1], 0, value.Length);

                        iValidParameterNumber = value.Length;
                    }
                    else//超出范围
                    {
                        Array.Copy(value, 0, sParameterValueNameDisplay[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1], 0, ParameterNumber);

                        iValidParameterNumber = ParameterNumber;
                    }

                    if (value.Length > ParameterNumber - iNumberPerPage) //有效数据大于20 
                    {
                        iTotalPage = 3;
                    }
                    else if (value.Length > ParameterNumber - 2 * iNumberPerPage) //有效数据大于10 
                    {
                        iTotalPage = 2;
                    }
                    else
                    {
                        iTotalPage = 1;
                    }

                    ////

                    //_GetText();//获取文本

                    ////

                    //_AddValues();//添加数据
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：English_ParameterValueNameDisplay属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("原始参数数值英文名称（[语言][参数]，参数类型取值为1时有效）"), Category("ParameterSettingsPanel 通用")]
        public String[] English_ParameterValueNameDisplay//属性
        {
            get//读取
            {
                return sParameterValueNameDisplay[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1];
            }
            set//设置
            {
                if (null != value)
                {
                    if (value.Length <= ParameterNumber)//范围之内
                    {
                        Array.Copy(value, 0, sParameterValueNameDisplay[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1], 0, value.Length);

                        iValidParameterNumber = value.Length;
                    }
                    else//超出范围
                    {
                        Array.Copy(value, 0, sParameterValueNameDisplay[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1], 0, ParameterNumber);

                        iValidParameterNumber = ParameterNumber;
                    }

                    if (value.Length > ParameterNumber - iNumberPerPage) //有效数据大于20 
                    {
                        iTotalPage = 3;
                    }
                    else if (value.Length > ParameterNumber - 2 * iNumberPerPage) //有效数据大于10 
                    {
                        iTotalPage = 2;
                    }
                    else
                    {
                        iTotalPage = 1;
                    }

                    ////

                    //_GetText();//获取文本

                    ////

                    //_AddValues();//添加数据
                }
            }
        }

        //函数

        //-----------------------------------------------------------------------
        // 功能说明：设置参数
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        public void _SetParameterValue(Int32[][] parametervalue)
        {
            if (null != parametervalue)
            {
                if (parametervalue.Length <= ParameterNumber)//范围之内
                {
                    iValidParameterNumber = parametervalue.Length;
                }
                else//超出范围
                {
                    iValidParameterNumber = ParameterNumber;
                }

                if (iValidParameterNumber > ParameterNumber - iNumberPerPage) //有效数据大于20 
                {
                    iTotalPage = 3;
                }
                else if (iValidParameterNumber > ParameterNumber - 2 * iNumberPerPage) //有效数据大于10 
                {
                    iTotalPage = 2;
                }
                else
                {
                    iTotalPage = 1;
                }

                for (Int32 i = 0; i < iValidParameterNumber; i++)
                {
                    if (null != parametervalue[i])//有效
                    {
                        ParameterValue[i] = new Int32[parametervalue[i].Length];

                        Array.Copy(parametervalue[i], 0, ParameterValue[i], 0, parametervalue[i].Length);
                    }
                    else//无效
                    {
                        ParameterValue[i] = null;
                    }
                }

                ////

                //_GetText();//获取文本

                ////

                //_AddValues();//添加数据
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：设置参数
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        public void _SetParameterValueEnabled(Boolean[][] parametervalue)
        {
            if (null != parametervalue)
            {
                if (parametervalue.Length <= ParameterNumber)//范围之内
                {
                    iValidParameterNumber = parametervalue.Length;
                }
                else//超出范围
                {
                    iValidParameterNumber = ParameterNumber;
                }

                if (iValidParameterNumber > ParameterNumber - iNumberPerPage) //有效数据大于20 
                {
                    iTotalPage = 3;
                }
                else if (iValidParameterNumber > ParameterNumber - 2 * iNumberPerPage) //有效数据大于10 
                {
                    iTotalPage = 2;
                }
                else
                {
                    iTotalPage = 1;
                }

                for (Int32 i = 0; i < iValidParameterNumber; i++)
                {
                    if (null != parametervalue[i])//有效
                    {
                        ParameterValueEnabled[i] = new Boolean[parametervalue[i].Length];

                        Array.Copy(parametervalue[i], 0, ParameterValueEnabled[i], 0, parametervalue[i].Length);
                    }
                    else//无效
                    {
                        ParameterValueEnabled[i] = null;
                    }
                }

                ////

                //_GetText();//获取文本

                ////

                //_AddValues();//添加数据
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：应用设置
        // 输入参数：1.bResetCurrentSelectedValueIndex：是否复位当前选择的项的索引值。取值范围：true，是；false，否
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _Apply(Boolean bResetCurrentSelectedValueIndex)
        {
            if (bResetCurrentSelectedValueIndex) //复位时，页面同步复位
            {
                iCurrentPage = 0;
            }

            //翻页控件

            if (1 >= iTotalPage)//页码总数为1
            {
                customButtonPreviousPage.Visible = false;//隐藏【Previous Page】按钮
                customButtonNextPage.Visible = false;//隐藏【Next Page】按钮
            }
            else//页码总数不为1
            {
                if (0 == iCurrentPage)//首页
                {
                    customButtonPreviousPage.Visible = false;//隐藏【Previous Page】按钮
                    customButtonNextPage.Visible = true;//显示【Next Page】按钮
                }
                else if (iTotalPage - 1 == iCurrentPage)//末页
                {
                    customButtonPreviousPage.Visible = true;//显示【Previous Page】按钮
                    customButtonNextPage.Visible = false;//隐藏【Next Page】按钮
                }
                else//其它
                {
                    customButtonPreviousPage.Visible = true;//显示【Previous Page】按钮
                    customButtonNextPage.Visible = true;//显示【Next Page】按钮
                }
            }

            _GetText();//获取文本

            _AddValues();//添加数据

            //

            _SetControlEnabled();

            //

            if (bResetCurrentSelectedValueIndex)//复位
            {
                _ClickValueControl(-1);

                customButtonCurrentSelectedName = null;
                customButtonCurrentSelectedValue = null;
            }
            else//不复位
            {
                _ClickValueControl(iCurrentSelectedValueIndex);
            }

            _SetPage();//设置页面曲线图和控件
        }

        //----------------------------------------------------------------------
        // 功能说明：应用设置
        // 输入参数：清除选中状态
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _ClearSelectState()
        {
            _ClickValueControl(-1);

            customButtonCurrentSelectedName = null;
            customButtonCurrentSelectedValue = null;
        }

        //----------------------------------------------------------------------
        // 功能说明：设置语言
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetLanguage()
        {
            customButtonName_1.Language = language;//名称1控件
            customButtonName_2.Language = language;//名称2控件
            customButtonName_3.Language = language;//名称3控件
            customButtonName_4.Language = language;//名称4控件
            customButtonName_5.Language = language;//名称5控件
            customButtonName_6.Language = language;//名称6控件
            customButtonName_7.Language = language;//名称7控件
            customButtonName_8.Language = language;//名称8控件
            customButtonName_9.Language = language;//名称9控件
            customButtonName_10.Language = language;//名称10控件
            customButtonName_11.Language = language;//名称11控件
            customButtonName_12.Language = language;//名称12控件
            customButtonName_13.Language = language;//名称13控件
            customButtonName_14.Language = language;//名称14控件
            customButtonName_15.Language = language;//名称15控件
            customButtonName_16.Language = language;//名称16控件
            customButtonName_17.Language = language;//名称17控件
            customButtonName_18.Language = language;//名称18控件
            customButtonName_19.Language = language;//名称19控件
            customButtonName_20.Language = language;//名称20控件
            customButtonName_21.Language = language;//名称21控件
            customButtonName_22.Language = language;//名称22控件
            customButtonName_23.Language = language;//名称23控件
            customButtonName_24.Language = language;//名称24控件
            customButtonName_25.Language = language;//名称25控件
            customButtonName_16.Language = language;//名称16控件
            customButtonName_27.Language = language;//名称27控件
            customButtonName_28.Language = language;//名称28控件
            customButtonName_29.Language = language;//名称29控件
            customButtonName_30.Language = language;//名称30控件


            customButtonValue_1.Language = language;//数值1控件
            customButtonValue_2.Language = language;//数值2控件
            customButtonValue_3.Language = language;//数值3控件
            customButtonValue_4.Language = language;//数值4控件
            customButtonValue_5.Language = language;//数值5控件
            customButtonValue_6.Language = language;//数值6控件
            customButtonValue_7.Language = language;//数值7控件
            customButtonValue_8.Language = language;//数值8控件
            customButtonValue_9.Language = language;//数值9控件
            customButtonValue_10.Language = language;//数值10控件
            customButtonValue_11.Language = language;//数值11控件
            customButtonValue_12.Language = language;//数值12控件
            customButtonValue_13.Language = language;//数值13控件
            customButtonValue_14.Language = language;//数值14控件
            customButtonValue_15.Language = language;//数值15控件
            customButtonValue_16.Language = language;//数值16控件
            customButtonValue_17.Language = language;//数值17控件
            customButtonValue_18.Language = language;//数值18控件
            customButtonValue_19.Language = language;//数值19控件
            customButtonValue_20.Language = language;//数值20控件
            customButtonValue_21.Language = language;//数值21控件
            customButtonValue_22.Language = language;//数值22控件
            customButtonValue_23.Language = language;//数值23控件
            customButtonValue_24.Language = language;//数值24控件
            customButtonValue_25.Language = language;//数值25控件
            customButtonValue_26.Language = language;//数值26控件
            customButtonValue_27.Language = language;//数值27控件
            customButtonValue_28.Language = language;//数值28控件
            customButtonValue_29.Language = language;//数值29控件
            customButtonValue_30.Language = language;//数值30控件

            customButtonSubtract.Language = language;//【-】
            customButtonSet.Language = language;//【Set】
            customButtonPlus.Language = language;//【+】

            customButtonNextPage.Language = language;
            customButtonPreviousPage.Language = language;
        }

        //-----------------------------------------------------------------------
        // 功能说明：控件整体使能时，设置控件状态
        // 输入参数： 1.buttonValue：数值控件
        //          2.iButtonIndex：数值控件索引值（从0开始）
        //          3.bCurrentSelected：是否为当前选中控件。取值范围：true，是；false，否
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        private void _SetControl_Enabled(CustomButton buttonValue, Int32 iButtonIndex, Boolean bCurrentSelected)
        {
            if (bCurrentSelected)//当前选中控件
            {
                if (bParameterEnabled[iButtonIndex])//使能
                {
                    buttonValue.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
                }
                else//禁止
                {
                    buttonValue.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                }
            }
            else//其它
            {
                if (bParameterEnabled[iButtonIndex])//使能
                {
                    buttonValue.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                }
                else//禁止
                {
                    buttonValue.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：设置控件使能状态
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        public void _SetControlEnabled()
        {
            if (bControlEnabled)//使能
            {
                switch (iCurrentSelectedValueIndex)//当前选择的参数控件索引
                {
                    case 0:
                        //
                        _SetControl_Enabled(customButtonValue_1, 0, true);
                        _SetControl_Enabled(customButtonValue_2, 1, false);
                        _SetControl_Enabled(customButtonValue_3, 2, false);
                        _SetControl_Enabled(customButtonValue_4, 3, false);
                        _SetControl_Enabled(customButtonValue_5, 4, false);
                        _SetControl_Enabled(customButtonValue_6, 5, false);
                        _SetControl_Enabled(customButtonValue_7, 6, false);
                        _SetControl_Enabled(customButtonValue_8, 7, false);
                        _SetControl_Enabled(customButtonValue_9, 8, false);
                        _SetControl_Enabled(customButtonValue_10, 9, false);
                        _SetControl_Enabled(customButtonValue_11, 10, false);
                        _SetControl_Enabled(customButtonValue_12, 11, false);
                        _SetControl_Enabled(customButtonValue_13, 12, false);
                        _SetControl_Enabled(customButtonValue_14, 13, false);
                        _SetControl_Enabled(customButtonValue_15, 14, false);
                        _SetControl_Enabled(customButtonValue_16, 15, false);
                        _SetControl_Enabled(customButtonValue_17, 16, false);
                        _SetControl_Enabled(customButtonValue_18, 17, false);
                        _SetControl_Enabled(customButtonValue_19, 18, false);
                        _SetControl_Enabled(customButtonValue_20, 19, false);
                        _SetControl_Enabled(customButtonValue_21, 20, false);
                        _SetControl_Enabled(customButtonValue_22, 21, false);
                        _SetControl_Enabled(customButtonValue_23, 22, false);
                        _SetControl_Enabled(customButtonValue_24, 23, false);
                        _SetControl_Enabled(customButtonValue_25, 24, false);
                        _SetControl_Enabled(customButtonValue_26, 25, false);
                        _SetControl_Enabled(customButtonValue_27, 26, false);
                        _SetControl_Enabled(customButtonValue_28, 27, false);
                        _SetControl_Enabled(customButtonValue_29, 28, false);
                        _SetControl_Enabled(customButtonValue_30, 29, false);
                        //
                        break;
                    case 1:
                        //
                        _SetControl_Enabled(customButtonValue_1, 0, false);
                        _SetControl_Enabled(customButtonValue_2, 1, true);
                        _SetControl_Enabled(customButtonValue_3, 2, false);
                        _SetControl_Enabled(customButtonValue_4, 3, false);
                        _SetControl_Enabled(customButtonValue_5, 4, false);
                        _SetControl_Enabled(customButtonValue_6, 5, false);
                        _SetControl_Enabled(customButtonValue_7, 6, false);
                        _SetControl_Enabled(customButtonValue_8, 7, false);
                        _SetControl_Enabled(customButtonValue_9, 8, false);
                        _SetControl_Enabled(customButtonValue_10, 9, false);
                        _SetControl_Enabled(customButtonValue_11, 10, false);
                        _SetControl_Enabled(customButtonValue_12, 11, false);
                        _SetControl_Enabled(customButtonValue_13, 12, false);
                        _SetControl_Enabled(customButtonValue_14, 13, false);
                        _SetControl_Enabled(customButtonValue_15, 14, false);
                        _SetControl_Enabled(customButtonValue_16, 15, false);
                        _SetControl_Enabled(customButtonValue_17, 16, false);
                        _SetControl_Enabled(customButtonValue_18, 17, false);
                        _SetControl_Enabled(customButtonValue_19, 18, false);
                        _SetControl_Enabled(customButtonValue_20, 19, false);
                        _SetControl_Enabled(customButtonValue_21, 20, false);
                        _SetControl_Enabled(customButtonValue_22, 21, false);
                        _SetControl_Enabled(customButtonValue_23, 22, false);
                        _SetControl_Enabled(customButtonValue_24, 23, false);
                        _SetControl_Enabled(customButtonValue_25, 24, false);
                        _SetControl_Enabled(customButtonValue_26, 25, false);
                        _SetControl_Enabled(customButtonValue_27, 26, false);
                        _SetControl_Enabled(customButtonValue_28, 27, false);
                        _SetControl_Enabled(customButtonValue_29, 28, false);
                        _SetControl_Enabled(customButtonValue_30, 29, false);
                        //
                        break;
                    case 2:
                        //
                        _SetControl_Enabled(customButtonValue_1, 0, false);
                        _SetControl_Enabled(customButtonValue_2, 1, false);
                        _SetControl_Enabled(customButtonValue_3, 2, true);
                        _SetControl_Enabled(customButtonValue_4, 3, false);
                        _SetControl_Enabled(customButtonValue_5, 4, false);
                        _SetControl_Enabled(customButtonValue_6, 5, false);
                        _SetControl_Enabled(customButtonValue_7, 6, false);
                        _SetControl_Enabled(customButtonValue_8, 7, false);
                        _SetControl_Enabled(customButtonValue_9, 8, false);
                        _SetControl_Enabled(customButtonValue_10, 9, false);
                        _SetControl_Enabled(customButtonValue_11, 10, false);
                        _SetControl_Enabled(customButtonValue_12, 11, false);
                        _SetControl_Enabled(customButtonValue_13, 12, false);
                        _SetControl_Enabled(customButtonValue_14, 13, false);
                        _SetControl_Enabled(customButtonValue_15, 14, false);
                        _SetControl_Enabled(customButtonValue_16, 15, false);
                        _SetControl_Enabled(customButtonValue_17, 16, false);
                        _SetControl_Enabled(customButtonValue_18, 17, false);
                        _SetControl_Enabled(customButtonValue_19, 18, false);
                        _SetControl_Enabled(customButtonValue_20, 19, false);
                        _SetControl_Enabled(customButtonValue_21, 20, false);
                        _SetControl_Enabled(customButtonValue_22, 21, false);
                        _SetControl_Enabled(customButtonValue_23, 22, false);
                        _SetControl_Enabled(customButtonValue_24, 23, false);
                        _SetControl_Enabled(customButtonValue_25, 24, false);
                        _SetControl_Enabled(customButtonValue_26, 25, false);
                        _SetControl_Enabled(customButtonValue_27, 26, false);
                        _SetControl_Enabled(customButtonValue_28, 27, false);
                        _SetControl_Enabled(customButtonValue_29, 28, false);
                        _SetControl_Enabled(customButtonValue_30, 29, false);
                        //
                        break;
                    case 3:
                        //
                        _SetControl_Enabled(customButtonValue_1, 0, false);
                        _SetControl_Enabled(customButtonValue_2, 1, false);
                        _SetControl_Enabled(customButtonValue_3, 2, false);
                        _SetControl_Enabled(customButtonValue_4, 3, true);
                        _SetControl_Enabled(customButtonValue_5, 4, false);
                        _SetControl_Enabled(customButtonValue_6, 5, false);
                        _SetControl_Enabled(customButtonValue_7, 6, false);
                        _SetControl_Enabled(customButtonValue_8, 7, false);
                        _SetControl_Enabled(customButtonValue_9, 8, false);
                        _SetControl_Enabled(customButtonValue_10, 9, false);
                        _SetControl_Enabled(customButtonValue_11, 10, false);
                        _SetControl_Enabled(customButtonValue_12, 11, false);
                        _SetControl_Enabled(customButtonValue_13, 12, false);
                        _SetControl_Enabled(customButtonValue_14, 13, false);
                        _SetControl_Enabled(customButtonValue_15, 14, false);
                        _SetControl_Enabled(customButtonValue_16, 15, false);
                        _SetControl_Enabled(customButtonValue_17, 16, false);
                        _SetControl_Enabled(customButtonValue_18, 17, false);
                        _SetControl_Enabled(customButtonValue_19, 18, false);
                        _SetControl_Enabled(customButtonValue_20, 19, false);
                        _SetControl_Enabled(customButtonValue_21, 20, false);
                        _SetControl_Enabled(customButtonValue_22, 21, false);
                        _SetControl_Enabled(customButtonValue_23, 22, false);
                        _SetControl_Enabled(customButtonValue_24, 23, false);
                        _SetControl_Enabled(customButtonValue_25, 24, false);
                        _SetControl_Enabled(customButtonValue_26, 25, false);
                        _SetControl_Enabled(customButtonValue_27, 26, false);
                        _SetControl_Enabled(customButtonValue_28, 27, false);
                        _SetControl_Enabled(customButtonValue_29, 28, false);
                        _SetControl_Enabled(customButtonValue_30, 29, false);
                        //
                        break;
                    case 4:
                        //
                        _SetControl_Enabled(customButtonValue_1, 0, false);
                        _SetControl_Enabled(customButtonValue_2, 1, false);
                        _SetControl_Enabled(customButtonValue_3, 2, false);
                        _SetControl_Enabled(customButtonValue_4, 3, false);
                        _SetControl_Enabled(customButtonValue_5, 4, true);
                        _SetControl_Enabled(customButtonValue_6, 5, false);
                        _SetControl_Enabled(customButtonValue_7, 6, false);
                        _SetControl_Enabled(customButtonValue_8, 7, false);
                        _SetControl_Enabled(customButtonValue_9, 8, false);
                        _SetControl_Enabled(customButtonValue_10, 9, false);
                        _SetControl_Enabled(customButtonValue_11, 10, false);
                        _SetControl_Enabled(customButtonValue_12, 11, false);
                        _SetControl_Enabled(customButtonValue_13, 12, false);
                        _SetControl_Enabled(customButtonValue_14, 13, false);
                        _SetControl_Enabled(customButtonValue_15, 14, false);
                        _SetControl_Enabled(customButtonValue_16, 15, false);
                        _SetControl_Enabled(customButtonValue_17, 16, false);
                        _SetControl_Enabled(customButtonValue_18, 17, false);
                        _SetControl_Enabled(customButtonValue_19, 18, false);
                        _SetControl_Enabled(customButtonValue_20, 19, false);
                        _SetControl_Enabled(customButtonValue_21, 20, false);
                        _SetControl_Enabled(customButtonValue_22, 21, false);
                        _SetControl_Enabled(customButtonValue_23, 22, false);
                        _SetControl_Enabled(customButtonValue_24, 23, false);
                        _SetControl_Enabled(customButtonValue_25, 24, false);
                        _SetControl_Enabled(customButtonValue_26, 25, false);
                        _SetControl_Enabled(customButtonValue_27, 26, false);
                        _SetControl_Enabled(customButtonValue_28, 27, false);
                        _SetControl_Enabled(customButtonValue_29, 28, false);
                        _SetControl_Enabled(customButtonValue_30, 29, false);
                        //
                        break;
                    case 5:
                        //
                        _SetControl_Enabled(customButtonValue_1, 0, false);
                        _SetControl_Enabled(customButtonValue_2, 1, false);
                        _SetControl_Enabled(customButtonValue_3, 2, false);
                        _SetControl_Enabled(customButtonValue_4, 3, false);
                        _SetControl_Enabled(customButtonValue_5, 4, false);
                        _SetControl_Enabled(customButtonValue_6, 5, true);
                        _SetControl_Enabled(customButtonValue_7, 6, false);
                        _SetControl_Enabled(customButtonValue_8, 7, false);
                        _SetControl_Enabled(customButtonValue_9, 8, false);
                        _SetControl_Enabled(customButtonValue_10, 9, false);
                        _SetControl_Enabled(customButtonValue_11, 10, false);
                        _SetControl_Enabled(customButtonValue_12, 11, false);
                        _SetControl_Enabled(customButtonValue_13, 12, false);
                        _SetControl_Enabled(customButtonValue_14, 13, false);
                        _SetControl_Enabled(customButtonValue_15, 14, false);
                        _SetControl_Enabled(customButtonValue_16, 15, false);
                        _SetControl_Enabled(customButtonValue_17, 16, false);
                        _SetControl_Enabled(customButtonValue_18, 17, false);
                        _SetControl_Enabled(customButtonValue_19, 18, false);
                        _SetControl_Enabled(customButtonValue_20, 19, false);
                        _SetControl_Enabled(customButtonValue_21, 20, false);
                        _SetControl_Enabled(customButtonValue_22, 21, false);
                        _SetControl_Enabled(customButtonValue_23, 22, false);
                        _SetControl_Enabled(customButtonValue_24, 23, false);
                        _SetControl_Enabled(customButtonValue_25, 24, false);
                        _SetControl_Enabled(customButtonValue_26, 25, false);
                        _SetControl_Enabled(customButtonValue_27, 26, false);
                        _SetControl_Enabled(customButtonValue_28, 27, false);
                        _SetControl_Enabled(customButtonValue_29, 28, false);
                        _SetControl_Enabled(customButtonValue_30, 29, false);
                        //
                        break;
                    case 6:
                        //
                        _SetControl_Enabled(customButtonValue_1, 0, false);
                        _SetControl_Enabled(customButtonValue_2, 1, false);
                        _SetControl_Enabled(customButtonValue_3, 2, false);
                        _SetControl_Enabled(customButtonValue_4, 3, false);
                        _SetControl_Enabled(customButtonValue_5, 4, false);
                        _SetControl_Enabled(customButtonValue_6, 5, false);
                        _SetControl_Enabled(customButtonValue_7, 6, true);
                        _SetControl_Enabled(customButtonValue_8, 7, false);
                        _SetControl_Enabled(customButtonValue_9, 8, false);
                        _SetControl_Enabled(customButtonValue_10, 9, false);
                        _SetControl_Enabled(customButtonValue_11, 10, false);
                        _SetControl_Enabled(customButtonValue_12, 11, false);
                        _SetControl_Enabled(customButtonValue_13, 12, false);
                        _SetControl_Enabled(customButtonValue_14, 13, false);
                        _SetControl_Enabled(customButtonValue_15, 14, false);
                        _SetControl_Enabled(customButtonValue_16, 15, false);
                        _SetControl_Enabled(customButtonValue_17, 16, false);
                        _SetControl_Enabled(customButtonValue_18, 17, false);
                        _SetControl_Enabled(customButtonValue_19, 18, false);
                        _SetControl_Enabled(customButtonValue_20, 19, false);
                        _SetControl_Enabled(customButtonValue_21, 20, false);
                        _SetControl_Enabled(customButtonValue_22, 21, false);
                        _SetControl_Enabled(customButtonValue_23, 22, false);
                        _SetControl_Enabled(customButtonValue_24, 23, false);
                        _SetControl_Enabled(customButtonValue_25, 24, false);
                        _SetControl_Enabled(customButtonValue_26, 25, false);
                        _SetControl_Enabled(customButtonValue_27, 26, false);
                        _SetControl_Enabled(customButtonValue_28, 27, false);
                        _SetControl_Enabled(customButtonValue_29, 28, false);
                        _SetControl_Enabled(customButtonValue_30, 29, false);
                        //
                        break;
                    case 7:
                        //
                        _SetControl_Enabled(customButtonValue_1, 0, false);
                        _SetControl_Enabled(customButtonValue_2, 1, false);
                        _SetControl_Enabled(customButtonValue_3, 2, false);
                        _SetControl_Enabled(customButtonValue_4, 3, false);
                        _SetControl_Enabled(customButtonValue_5, 4, false);
                        _SetControl_Enabled(customButtonValue_6, 5, false);
                        _SetControl_Enabled(customButtonValue_7, 6, false);
                        _SetControl_Enabled(customButtonValue_8, 7, true);
                        _SetControl_Enabled(customButtonValue_9, 8, false);
                        _SetControl_Enabled(customButtonValue_10, 9, false);
                        _SetControl_Enabled(customButtonValue_11, 10, false);
                        _SetControl_Enabled(customButtonValue_12, 11, false);
                        _SetControl_Enabled(customButtonValue_13, 12, false);
                        _SetControl_Enabled(customButtonValue_14, 13, false);
                        _SetControl_Enabled(customButtonValue_15, 14, false);
                        _SetControl_Enabled(customButtonValue_16, 15, false);
                        _SetControl_Enabled(customButtonValue_17, 16, false);
                        _SetControl_Enabled(customButtonValue_18, 17, false);
                        _SetControl_Enabled(customButtonValue_19, 18, false);
                        _SetControl_Enabled(customButtonValue_20, 19, false);
                        _SetControl_Enabled(customButtonValue_21, 20, false);
                        _SetControl_Enabled(customButtonValue_22, 21, false);
                        _SetControl_Enabled(customButtonValue_23, 22, false);
                        _SetControl_Enabled(customButtonValue_24, 23, false);
                        _SetControl_Enabled(customButtonValue_25, 24, false);
                        _SetControl_Enabled(customButtonValue_26, 25, false);
                        _SetControl_Enabled(customButtonValue_27, 26, false);
                        _SetControl_Enabled(customButtonValue_28, 27, false);
                        _SetControl_Enabled(customButtonValue_29, 28, false);
                        _SetControl_Enabled(customButtonValue_30, 29, false);
                        //
                        break;
                    case 8:
                        //
                        _SetControl_Enabled(customButtonValue_1, 0, false);
                        _SetControl_Enabled(customButtonValue_2, 1, false);
                        _SetControl_Enabled(customButtonValue_3, 2, false);
                        _SetControl_Enabled(customButtonValue_4, 3, false);
                        _SetControl_Enabled(customButtonValue_5, 4, false);
                        _SetControl_Enabled(customButtonValue_6, 5, false);
                        _SetControl_Enabled(customButtonValue_7, 6, false);
                        _SetControl_Enabled(customButtonValue_8, 7, false);
                        _SetControl_Enabled(customButtonValue_9, 8, true);
                        _SetControl_Enabled(customButtonValue_10, 9, false);
                        _SetControl_Enabled(customButtonValue_11, 10, false);
                        _SetControl_Enabled(customButtonValue_12, 11, false);
                        _SetControl_Enabled(customButtonValue_13, 12, false);
                        _SetControl_Enabled(customButtonValue_14, 13, false);
                        _SetControl_Enabled(customButtonValue_15, 14, false);
                        _SetControl_Enabled(customButtonValue_16, 15, false);
                        _SetControl_Enabled(customButtonValue_17, 16, false);
                        _SetControl_Enabled(customButtonValue_18, 17, false);
                        _SetControl_Enabled(customButtonValue_19, 18, false);
                        _SetControl_Enabled(customButtonValue_20, 19, false);
                        _SetControl_Enabled(customButtonValue_21, 20, false);
                        _SetControl_Enabled(customButtonValue_22, 21, false);
                        _SetControl_Enabled(customButtonValue_23, 22, false);
                        _SetControl_Enabled(customButtonValue_24, 23, false);
                        _SetControl_Enabled(customButtonValue_25, 24, false);
                        _SetControl_Enabled(customButtonValue_26, 25, false);
                        _SetControl_Enabled(customButtonValue_27, 26, false);
                        _SetControl_Enabled(customButtonValue_28, 27, false);
                        _SetControl_Enabled(customButtonValue_29, 28, false);
                        _SetControl_Enabled(customButtonValue_30, 29, false);
                        //
                        break;
                    case 9:
                        //
                        _SetControl_Enabled(customButtonValue_1, 0, false);
                        _SetControl_Enabled(customButtonValue_2, 1, false);
                        _SetControl_Enabled(customButtonValue_3, 2, false);
                        _SetControl_Enabled(customButtonValue_4, 3, false);
                        _SetControl_Enabled(customButtonValue_5, 4, false);
                        _SetControl_Enabled(customButtonValue_6, 5, false);
                        _SetControl_Enabled(customButtonValue_7, 6, false);
                        _SetControl_Enabled(customButtonValue_8, 7, false);
                        _SetControl_Enabled(customButtonValue_9, 8, false);
                        _SetControl_Enabled(customButtonValue_10, 9, true);
                        _SetControl_Enabled(customButtonValue_11, 10, false);
                        _SetControl_Enabled(customButtonValue_12, 11, false);
                        _SetControl_Enabled(customButtonValue_13, 12, false);
                        _SetControl_Enabled(customButtonValue_14, 13, false);
                        _SetControl_Enabled(customButtonValue_15, 14, false);
                        _SetControl_Enabled(customButtonValue_16, 15, false);
                        _SetControl_Enabled(customButtonValue_17, 16, false);
                        _SetControl_Enabled(customButtonValue_18, 17, false);
                        _SetControl_Enabled(customButtonValue_19, 18, false);
                        _SetControl_Enabled(customButtonValue_20, 19, false);
                        _SetControl_Enabled(customButtonValue_21, 20, false);
                        _SetControl_Enabled(customButtonValue_22, 21, false);
                        _SetControl_Enabled(customButtonValue_23, 22, false);
                        _SetControl_Enabled(customButtonValue_24, 23, false);
                        _SetControl_Enabled(customButtonValue_25, 24, false);
                        _SetControl_Enabled(customButtonValue_26, 25, false);
                        _SetControl_Enabled(customButtonValue_27, 26, false);
                        _SetControl_Enabled(customButtonValue_28, 27, false);
                        _SetControl_Enabled(customButtonValue_29, 28, false);
                        _SetControl_Enabled(customButtonValue_30, 29, false);
                        //
                        break;
                    case 10:
                        //
                        _SetControl_Enabled(customButtonValue_1, 0, false);
                        _SetControl_Enabled(customButtonValue_2, 1, false);
                        _SetControl_Enabled(customButtonValue_3, 2, false);
                        _SetControl_Enabled(customButtonValue_4, 3, false);
                        _SetControl_Enabled(customButtonValue_5, 4, false);
                        _SetControl_Enabled(customButtonValue_6, 5, false);
                        _SetControl_Enabled(customButtonValue_7, 6, false);
                        _SetControl_Enabled(customButtonValue_8, 7, false);
                        _SetControl_Enabled(customButtonValue_9, 8, false);
                        _SetControl_Enabled(customButtonValue_10, 9, false);
                        _SetControl_Enabled(customButtonValue_11, 10, true);
                        _SetControl_Enabled(customButtonValue_12, 11, false);
                        _SetControl_Enabled(customButtonValue_13, 12, false);
                        _SetControl_Enabled(customButtonValue_14, 13, false);
                        _SetControl_Enabled(customButtonValue_15, 14, false);
                        _SetControl_Enabled(customButtonValue_16, 15, false);
                        _SetControl_Enabled(customButtonValue_17, 16, false);
                        _SetControl_Enabled(customButtonValue_18, 17, false);
                        _SetControl_Enabled(customButtonValue_19, 18, false);
                        _SetControl_Enabled(customButtonValue_20, 19, false);
                        _SetControl_Enabled(customButtonValue_21, 20, false);
                        _SetControl_Enabled(customButtonValue_22, 21, false);
                        _SetControl_Enabled(customButtonValue_23, 22, false);
                        _SetControl_Enabled(customButtonValue_24, 23, false);
                        _SetControl_Enabled(customButtonValue_25, 24, false);
                        _SetControl_Enabled(customButtonValue_26, 25, false);
                        _SetControl_Enabled(customButtonValue_27, 26, false);
                        _SetControl_Enabled(customButtonValue_28, 27, false);
                        _SetControl_Enabled(customButtonValue_29, 28, false);
                        _SetControl_Enabled(customButtonValue_30, 29, false);
                        //
                        break;
                    case 11:
                        //
                        _SetControl_Enabled(customButtonValue_1, 0, false);
                        _SetControl_Enabled(customButtonValue_2, 1, false);
                        _SetControl_Enabled(customButtonValue_3, 2, false);
                        _SetControl_Enabled(customButtonValue_4, 3, false);
                        _SetControl_Enabled(customButtonValue_5, 4, false);
                        _SetControl_Enabled(customButtonValue_6, 5, false);
                        _SetControl_Enabled(customButtonValue_7, 6, false);
                        _SetControl_Enabled(customButtonValue_8, 7, false);
                        _SetControl_Enabled(customButtonValue_9, 8, false);
                        _SetControl_Enabled(customButtonValue_10, 9, false);
                        _SetControl_Enabled(customButtonValue_11, 10, false);
                        _SetControl_Enabled(customButtonValue_12, 11, true);
                        _SetControl_Enabled(customButtonValue_13, 12, false);
                        _SetControl_Enabled(customButtonValue_14, 13, false);
                        _SetControl_Enabled(customButtonValue_15, 14, false);
                        _SetControl_Enabled(customButtonValue_16, 15, false);
                        _SetControl_Enabled(customButtonValue_17, 16, false);
                        _SetControl_Enabled(customButtonValue_18, 17, false);
                        _SetControl_Enabled(customButtonValue_19, 18, false);
                        _SetControl_Enabled(customButtonValue_20, 19, false);
                        _SetControl_Enabled(customButtonValue_21, 20, false);
                        _SetControl_Enabled(customButtonValue_22, 21, false);
                        _SetControl_Enabled(customButtonValue_23, 22, false);
                        _SetControl_Enabled(customButtonValue_24, 23, false);
                        _SetControl_Enabled(customButtonValue_25, 24, false);
                        _SetControl_Enabled(customButtonValue_26, 25, false);
                        _SetControl_Enabled(customButtonValue_27, 26, false);
                        _SetControl_Enabled(customButtonValue_28, 27, false);
                        _SetControl_Enabled(customButtonValue_29, 28, false);
                        _SetControl_Enabled(customButtonValue_30, 29, false);
                        //
                        break;
                    case 12:
                        //
                        _SetControl_Enabled(customButtonValue_1, 0, false);
                        _SetControl_Enabled(customButtonValue_2, 1, false);
                        _SetControl_Enabled(customButtonValue_3, 2, false);
                        _SetControl_Enabled(customButtonValue_4, 3, false);
                        _SetControl_Enabled(customButtonValue_5, 4, false);
                        _SetControl_Enabled(customButtonValue_6, 5, false);
                        _SetControl_Enabled(customButtonValue_7, 6, false);
                        _SetControl_Enabled(customButtonValue_8, 7, false);
                        _SetControl_Enabled(customButtonValue_9, 8, false);
                        _SetControl_Enabled(customButtonValue_10, 9, false);
                        _SetControl_Enabled(customButtonValue_11, 10, false);
                        _SetControl_Enabled(customButtonValue_12, 11, false);
                        _SetControl_Enabled(customButtonValue_13, 12, true);
                        _SetControl_Enabled(customButtonValue_14, 13, false);
                        _SetControl_Enabled(customButtonValue_15, 14, false);
                        _SetControl_Enabled(customButtonValue_16, 15, false);
                        _SetControl_Enabled(customButtonValue_17, 16, false);
                        _SetControl_Enabled(customButtonValue_18, 17, false);
                        _SetControl_Enabled(customButtonValue_19, 18, false);
                        _SetControl_Enabled(customButtonValue_20, 19, false);
                        _SetControl_Enabled(customButtonValue_21, 20, false);
                        _SetControl_Enabled(customButtonValue_22, 21, false);
                        _SetControl_Enabled(customButtonValue_23, 22, false);
                        _SetControl_Enabled(customButtonValue_24, 23, false);
                        _SetControl_Enabled(customButtonValue_25, 24, false);
                        _SetControl_Enabled(customButtonValue_26, 25, false);
                        _SetControl_Enabled(customButtonValue_27, 26, false);
                        _SetControl_Enabled(customButtonValue_28, 27, false);
                        _SetControl_Enabled(customButtonValue_29, 28, false);
                        _SetControl_Enabled(customButtonValue_30, 29, false);
                        //
                        break;
                    case 13:
                        //
                        _SetControl_Enabled(customButtonValue_1, 0, false);
                        _SetControl_Enabled(customButtonValue_2, 1, false);
                        _SetControl_Enabled(customButtonValue_3, 2, false);
                        _SetControl_Enabled(customButtonValue_4, 3, false);
                        _SetControl_Enabled(customButtonValue_5, 4, false);
                        _SetControl_Enabled(customButtonValue_6, 5, false);
                        _SetControl_Enabled(customButtonValue_7, 6, false);
                        _SetControl_Enabled(customButtonValue_8, 7, false);
                        _SetControl_Enabled(customButtonValue_9, 8, false);
                        _SetControl_Enabled(customButtonValue_10, 9, false);
                        _SetControl_Enabled(customButtonValue_11, 10, false);
                        _SetControl_Enabled(customButtonValue_12, 11, false);
                        _SetControl_Enabled(customButtonValue_13, 12, false);
                        _SetControl_Enabled(customButtonValue_14, 13, true);
                        _SetControl_Enabled(customButtonValue_15, 14, false);
                        _SetControl_Enabled(customButtonValue_16, 15, false);
                        _SetControl_Enabled(customButtonValue_17, 16, false);
                        _SetControl_Enabled(customButtonValue_18, 17, false);
                        _SetControl_Enabled(customButtonValue_19, 18, false);
                        _SetControl_Enabled(customButtonValue_20, 19, false);
                        _SetControl_Enabled(customButtonValue_21, 20, false);
                        _SetControl_Enabled(customButtonValue_22, 21, false);
                        _SetControl_Enabled(customButtonValue_23, 22, false);
                        _SetControl_Enabled(customButtonValue_24, 23, false);
                        _SetControl_Enabled(customButtonValue_25, 24, false);
                        _SetControl_Enabled(customButtonValue_26, 25, false);
                        _SetControl_Enabled(customButtonValue_27, 26, false);
                        _SetControl_Enabled(customButtonValue_28, 27, false);
                        _SetControl_Enabled(customButtonValue_29, 28, false);
                        _SetControl_Enabled(customButtonValue_30, 29, false);
                        //
                        break;
                    case 14:
                        //
                        _SetControl_Enabled(customButtonValue_1, 0, false);
                        _SetControl_Enabled(customButtonValue_2, 1, false);
                        _SetControl_Enabled(customButtonValue_3, 2, false);
                        _SetControl_Enabled(customButtonValue_4, 3, false);
                        _SetControl_Enabled(customButtonValue_5, 4, false);
                        _SetControl_Enabled(customButtonValue_6, 5, false);
                        _SetControl_Enabled(customButtonValue_7, 6, false);
                        _SetControl_Enabled(customButtonValue_8, 7, false);
                        _SetControl_Enabled(customButtonValue_9, 8, false);
                        _SetControl_Enabled(customButtonValue_10, 9, false);
                        _SetControl_Enabled(customButtonValue_11, 10, false);
                        _SetControl_Enabled(customButtonValue_12, 11, false);
                        _SetControl_Enabled(customButtonValue_13, 12, false);
                        _SetControl_Enabled(customButtonValue_14, 13, false);
                        _SetControl_Enabled(customButtonValue_15, 14, true);
                        _SetControl_Enabled(customButtonValue_16, 15, false);
                        _SetControl_Enabled(customButtonValue_17, 16, false);
                        _SetControl_Enabled(customButtonValue_18, 17, false);
                        _SetControl_Enabled(customButtonValue_19, 18, false);
                        _SetControl_Enabled(customButtonValue_20, 19, false);
                        _SetControl_Enabled(customButtonValue_21, 20, false);
                        _SetControl_Enabled(customButtonValue_22, 21, false);
                        _SetControl_Enabled(customButtonValue_23, 22, false);
                        _SetControl_Enabled(customButtonValue_24, 23, false);
                        _SetControl_Enabled(customButtonValue_25, 24, false);
                        _SetControl_Enabled(customButtonValue_26, 25, false);
                        _SetControl_Enabled(customButtonValue_27, 26, false);
                        _SetControl_Enabled(customButtonValue_28, 27, false);
                        _SetControl_Enabled(customButtonValue_29, 28, false);
                        _SetControl_Enabled(customButtonValue_30, 29, false);
                        //
                        break;
                    case 15:
                        //
                        _SetControl_Enabled(customButtonValue_1, 0, false);
                        _SetControl_Enabled(customButtonValue_2, 1, false);
                        _SetControl_Enabled(customButtonValue_3, 2, false);
                        _SetControl_Enabled(customButtonValue_4, 3, false);
                        _SetControl_Enabled(customButtonValue_5, 4, false);
                        _SetControl_Enabled(customButtonValue_6, 5, false);
                        _SetControl_Enabled(customButtonValue_7, 6, false);
                        _SetControl_Enabled(customButtonValue_8, 7, false);
                        _SetControl_Enabled(customButtonValue_9, 8, false);
                        _SetControl_Enabled(customButtonValue_10, 9, false);
                        _SetControl_Enabled(customButtonValue_11, 10, false);
                        _SetControl_Enabled(customButtonValue_12, 11, false);
                        _SetControl_Enabled(customButtonValue_13, 12, false);
                        _SetControl_Enabled(customButtonValue_14, 13, false);
                        _SetControl_Enabled(customButtonValue_15, 14, false);
                        _SetControl_Enabled(customButtonValue_16, 15, true);
                        _SetControl_Enabled(customButtonValue_17, 16, false);
                        _SetControl_Enabled(customButtonValue_18, 17, false);
                        _SetControl_Enabled(customButtonValue_19, 18, false);
                        _SetControl_Enabled(customButtonValue_20, 19, false);
                        _SetControl_Enabled(customButtonValue_21, 20, false);
                        _SetControl_Enabled(customButtonValue_22, 21, false);
                        _SetControl_Enabled(customButtonValue_23, 22, false);
                        _SetControl_Enabled(customButtonValue_24, 23, false);
                        _SetControl_Enabled(customButtonValue_25, 24, false);
                        _SetControl_Enabled(customButtonValue_26, 25, false);
                        _SetControl_Enabled(customButtonValue_27, 26, false);
                        _SetControl_Enabled(customButtonValue_28, 27, false);
                        _SetControl_Enabled(customButtonValue_29, 28, false);
                        _SetControl_Enabled(customButtonValue_30, 29, false);
                        //
                        break;
                    case 16:
                        //
                        _SetControl_Enabled(customButtonValue_1, 0, false);
                        _SetControl_Enabled(customButtonValue_2, 1, false);
                        _SetControl_Enabled(customButtonValue_3, 2, false);
                        _SetControl_Enabled(customButtonValue_4, 3, false);
                        _SetControl_Enabled(customButtonValue_5, 4, false);
                        _SetControl_Enabled(customButtonValue_6, 5, false);
                        _SetControl_Enabled(customButtonValue_7, 6, false);
                        _SetControl_Enabled(customButtonValue_8, 7, false);
                        _SetControl_Enabled(customButtonValue_9, 8, false);
                        _SetControl_Enabled(customButtonValue_10, 9, false);
                        _SetControl_Enabled(customButtonValue_11, 10, false);
                        _SetControl_Enabled(customButtonValue_12, 11, false);
                        _SetControl_Enabled(customButtonValue_13, 12, false);
                        _SetControl_Enabled(customButtonValue_14, 13, false);
                        _SetControl_Enabled(customButtonValue_15, 14, false);
                        _SetControl_Enabled(customButtonValue_16, 15, false);
                        _SetControl_Enabled(customButtonValue_17, 16, true);
                        _SetControl_Enabled(customButtonValue_18, 17, false);
                        _SetControl_Enabled(customButtonValue_19, 18, false);
                        _SetControl_Enabled(customButtonValue_20, 19, false);
                        _SetControl_Enabled(customButtonValue_21, 20, false);
                        _SetControl_Enabled(customButtonValue_22, 21, false);
                        _SetControl_Enabled(customButtonValue_23, 22, false);
                        _SetControl_Enabled(customButtonValue_24, 23, false);
                        _SetControl_Enabled(customButtonValue_25, 24, false);
                        _SetControl_Enabled(customButtonValue_26, 25, false);
                        _SetControl_Enabled(customButtonValue_27, 26, false);
                        _SetControl_Enabled(customButtonValue_28, 27, false);
                        _SetControl_Enabled(customButtonValue_29, 28, false);
                        _SetControl_Enabled(customButtonValue_30, 29, false);
                        //
                        break;
                    case 17:
                        //
                        _SetControl_Enabled(customButtonValue_1, 0, false);
                        _SetControl_Enabled(customButtonValue_2, 1, false);
                        _SetControl_Enabled(customButtonValue_3, 2, false);
                        _SetControl_Enabled(customButtonValue_4, 3, false);
                        _SetControl_Enabled(customButtonValue_5, 4, false);
                        _SetControl_Enabled(customButtonValue_6, 5, false);
                        _SetControl_Enabled(customButtonValue_7, 6, false);
                        _SetControl_Enabled(customButtonValue_8, 7, false);
                        _SetControl_Enabled(customButtonValue_9, 8, false);
                        _SetControl_Enabled(customButtonValue_10, 9, false);
                        _SetControl_Enabled(customButtonValue_11, 10, false);
                        _SetControl_Enabled(customButtonValue_12, 11, false);
                        _SetControl_Enabled(customButtonValue_13, 12, false);
                        _SetControl_Enabled(customButtonValue_14, 13, false);
                        _SetControl_Enabled(customButtonValue_15, 14, false);
                        _SetControl_Enabled(customButtonValue_16, 15, false);
                        _SetControl_Enabled(customButtonValue_17, 16, false);
                        _SetControl_Enabled(customButtonValue_18, 17, true);
                        _SetControl_Enabled(customButtonValue_19, 18, false);
                        _SetControl_Enabled(customButtonValue_20, 19, false);
                        _SetControl_Enabled(customButtonValue_21, 20, false);
                        _SetControl_Enabled(customButtonValue_22, 21, false);
                        _SetControl_Enabled(customButtonValue_23, 22, false);
                        _SetControl_Enabled(customButtonValue_24, 23, false);
                        _SetControl_Enabled(customButtonValue_25, 24, false);
                        _SetControl_Enabled(customButtonValue_26, 25, false);
                        _SetControl_Enabled(customButtonValue_27, 26, false);
                        _SetControl_Enabled(customButtonValue_28, 27, false);
                        _SetControl_Enabled(customButtonValue_29, 28, false);
                        _SetControl_Enabled(customButtonValue_30, 29, false);
                        //
                        break;
                    case 18:
                        //
                        _SetControl_Enabled(customButtonValue_1, 0, false);
                        _SetControl_Enabled(customButtonValue_2, 1, false);
                        _SetControl_Enabled(customButtonValue_3, 2, false);
                        _SetControl_Enabled(customButtonValue_4, 3, false);
                        _SetControl_Enabled(customButtonValue_5, 4, false);
                        _SetControl_Enabled(customButtonValue_6, 5, false);
                        _SetControl_Enabled(customButtonValue_7, 6, false);
                        _SetControl_Enabled(customButtonValue_8, 7, false);
                        _SetControl_Enabled(customButtonValue_9, 8, false);
                        _SetControl_Enabled(customButtonValue_10, 9, false);
                        _SetControl_Enabled(customButtonValue_11, 10, false);
                        _SetControl_Enabled(customButtonValue_12, 11, false);
                        _SetControl_Enabled(customButtonValue_13, 12, false);
                        _SetControl_Enabled(customButtonValue_14, 13, false);
                        _SetControl_Enabled(customButtonValue_15, 14, false);
                        _SetControl_Enabled(customButtonValue_16, 15, false);
                        _SetControl_Enabled(customButtonValue_17, 16, false);
                        _SetControl_Enabled(customButtonValue_18, 17, false);
                        _SetControl_Enabled(customButtonValue_19, 18, true);
                        _SetControl_Enabled(customButtonValue_20, 19, false);
                        _SetControl_Enabled(customButtonValue_21, 20, false);
                        _SetControl_Enabled(customButtonValue_22, 21, false);
                        _SetControl_Enabled(customButtonValue_23, 22, false);
                        _SetControl_Enabled(customButtonValue_24, 23, false);
                        _SetControl_Enabled(customButtonValue_25, 24, false);
                        _SetControl_Enabled(customButtonValue_26, 25, false);
                        _SetControl_Enabled(customButtonValue_27, 26, false);
                        _SetControl_Enabled(customButtonValue_28, 27, false);
                        _SetControl_Enabled(customButtonValue_29, 28, false);
                        _SetControl_Enabled(customButtonValue_30, 29, false);
                        //
                        break;
                    case 19:
                        //
                        _SetControl_Enabled(customButtonValue_1, 0, false);
                        _SetControl_Enabled(customButtonValue_2, 1, false);
                        _SetControl_Enabled(customButtonValue_3, 2, false);
                        _SetControl_Enabled(customButtonValue_4, 3, false);
                        _SetControl_Enabled(customButtonValue_5, 4, false);
                        _SetControl_Enabled(customButtonValue_6, 5, false);
                        _SetControl_Enabled(customButtonValue_7, 6, false);
                        _SetControl_Enabled(customButtonValue_8, 7, false);
                        _SetControl_Enabled(customButtonValue_9, 8, false);
                        _SetControl_Enabled(customButtonValue_10, 9, false);
                        _SetControl_Enabled(customButtonValue_11, 10, false);
                        _SetControl_Enabled(customButtonValue_12, 11, false);
                        _SetControl_Enabled(customButtonValue_13, 12, false);
                        _SetControl_Enabled(customButtonValue_14, 13, false);
                        _SetControl_Enabled(customButtonValue_15, 14, false);
                        _SetControl_Enabled(customButtonValue_16, 15, false);
                        _SetControl_Enabled(customButtonValue_17, 16, false);
                        _SetControl_Enabled(customButtonValue_18, 17, false);
                        _SetControl_Enabled(customButtonValue_19, 18, false);
                        _SetControl_Enabled(customButtonValue_20, 19, true);
                        _SetControl_Enabled(customButtonValue_21, 20, false);
                        _SetControl_Enabled(customButtonValue_22, 21, false);
                        _SetControl_Enabled(customButtonValue_23, 22, false);
                        _SetControl_Enabled(customButtonValue_24, 23, false);
                        _SetControl_Enabled(customButtonValue_25, 24, false);
                        _SetControl_Enabled(customButtonValue_26, 25, false);
                        _SetControl_Enabled(customButtonValue_27, 26, false);
                        _SetControl_Enabled(customButtonValue_28, 27, false);
                        _SetControl_Enabled(customButtonValue_29, 28, false);
                        _SetControl_Enabled(customButtonValue_30, 29, false);
                        //
                        break;
                    case 20:
                        //
                        _SetControl_Enabled(customButtonValue_1, 0, false);
                        _SetControl_Enabled(customButtonValue_2, 1, false);
                        _SetControl_Enabled(customButtonValue_3, 2, false);
                        _SetControl_Enabled(customButtonValue_4, 3, false);
                        _SetControl_Enabled(customButtonValue_5, 4, false);
                        _SetControl_Enabled(customButtonValue_6, 5, false);
                        _SetControl_Enabled(customButtonValue_7, 6, false);
                        _SetControl_Enabled(customButtonValue_8, 7, false);
                        _SetControl_Enabled(customButtonValue_9, 8, false);
                        _SetControl_Enabled(customButtonValue_10, 9, false);
                        _SetControl_Enabled(customButtonValue_11, 10, false);
                        _SetControl_Enabled(customButtonValue_12, 11, false);
                        _SetControl_Enabled(customButtonValue_13, 12, false);
                        _SetControl_Enabled(customButtonValue_14, 13, false);
                        _SetControl_Enabled(customButtonValue_15, 14, false);
                        _SetControl_Enabled(customButtonValue_16, 15, false);
                        _SetControl_Enabled(customButtonValue_17, 16, false);
                        _SetControl_Enabled(customButtonValue_18, 17, false);
                        _SetControl_Enabled(customButtonValue_19, 18, false);
                        _SetControl_Enabled(customButtonValue_20, 19, false);
                        _SetControl_Enabled(customButtonValue_21, 20, true);
                        _SetControl_Enabled(customButtonValue_22, 21, false);
                        _SetControl_Enabled(customButtonValue_23, 22, false);
                        _SetControl_Enabled(customButtonValue_24, 23, false);
                        _SetControl_Enabled(customButtonValue_25, 24, false);
                        _SetControl_Enabled(customButtonValue_26, 25, false);
                        _SetControl_Enabled(customButtonValue_27, 26, false);
                        _SetControl_Enabled(customButtonValue_28, 27, false);
                        _SetControl_Enabled(customButtonValue_29, 28, false);
                        _SetControl_Enabled(customButtonValue_30, 29, false);
                        //
                        break;
                    case 21:
                        //
                        _SetControl_Enabled(customButtonValue_1, 0, false);
                        _SetControl_Enabled(customButtonValue_2, 1, false);
                        _SetControl_Enabled(customButtonValue_3, 2, false);
                        _SetControl_Enabled(customButtonValue_4, 3, false);
                        _SetControl_Enabled(customButtonValue_5, 4, false);
                        _SetControl_Enabled(customButtonValue_6, 5, false);
                        _SetControl_Enabled(customButtonValue_7, 6, false);
                        _SetControl_Enabled(customButtonValue_8, 7, false);
                        _SetControl_Enabled(customButtonValue_9, 8, false);
                        _SetControl_Enabled(customButtonValue_10, 9, false);
                        _SetControl_Enabled(customButtonValue_11, 10, false);
                        _SetControl_Enabled(customButtonValue_12, 11, false);
                        _SetControl_Enabled(customButtonValue_13, 12, false);
                        _SetControl_Enabled(customButtonValue_14, 13, false);
                        _SetControl_Enabled(customButtonValue_15, 14, false);
                        _SetControl_Enabled(customButtonValue_16, 15, false);
                        _SetControl_Enabled(customButtonValue_17, 16, false);
                        _SetControl_Enabled(customButtonValue_18, 17, false);
                        _SetControl_Enabled(customButtonValue_19, 18, false);
                        _SetControl_Enabled(customButtonValue_20, 19, false);
                        _SetControl_Enabled(customButtonValue_21, 20, false);
                        _SetControl_Enabled(customButtonValue_22, 21, true);
                        _SetControl_Enabled(customButtonValue_23, 22, false);
                        _SetControl_Enabled(customButtonValue_24, 23, false);
                        _SetControl_Enabled(customButtonValue_25, 24, false);
                        _SetControl_Enabled(customButtonValue_26, 25, false);
                        _SetControl_Enabled(customButtonValue_27, 26, false);
                        _SetControl_Enabled(customButtonValue_28, 27, false);
                        _SetControl_Enabled(customButtonValue_29, 28, false);
                        _SetControl_Enabled(customButtonValue_30, 29, false);
                        //
                        break;
                    case 22:
                        //
                        _SetControl_Enabled(customButtonValue_1, 0, false);
                        _SetControl_Enabled(customButtonValue_2, 1, false);
                        _SetControl_Enabled(customButtonValue_3, 2, false);
                        _SetControl_Enabled(customButtonValue_4, 3, false);
                        _SetControl_Enabled(customButtonValue_5, 4, false);
                        _SetControl_Enabled(customButtonValue_6, 5, false);
                        _SetControl_Enabled(customButtonValue_7, 6, false);
                        _SetControl_Enabled(customButtonValue_8, 7, false);
                        _SetControl_Enabled(customButtonValue_9, 8, false);
                        _SetControl_Enabled(customButtonValue_10, 9, false);
                        _SetControl_Enabled(customButtonValue_11, 10, false);
                        _SetControl_Enabled(customButtonValue_12, 11, false);
                        _SetControl_Enabled(customButtonValue_13, 12, false);
                        _SetControl_Enabled(customButtonValue_14, 13, false);
                        _SetControl_Enabled(customButtonValue_15, 14, false);
                        _SetControl_Enabled(customButtonValue_16, 15, false);
                        _SetControl_Enabled(customButtonValue_17, 16, false);
                        _SetControl_Enabled(customButtonValue_18, 17, false);
                        _SetControl_Enabled(customButtonValue_19, 18, false);
                        _SetControl_Enabled(customButtonValue_20, 19, false);
                        _SetControl_Enabled(customButtonValue_21, 20, false);
                        _SetControl_Enabled(customButtonValue_22, 21, false);
                        _SetControl_Enabled(customButtonValue_23, 22, true);
                        _SetControl_Enabled(customButtonValue_24, 23, false);
                        _SetControl_Enabled(customButtonValue_25, 24, false);
                        _SetControl_Enabled(customButtonValue_26, 25, false);
                        _SetControl_Enabled(customButtonValue_27, 26, false);
                        _SetControl_Enabled(customButtonValue_28, 27, false);
                        _SetControl_Enabled(customButtonValue_29, 28, false);
                        _SetControl_Enabled(customButtonValue_30, 29, false);
                        //
                        break;
                    case 23:
                        //
                        _SetControl_Enabled(customButtonValue_1, 0, false);
                        _SetControl_Enabled(customButtonValue_2, 1, false);
                        _SetControl_Enabled(customButtonValue_3, 2, false);
                        _SetControl_Enabled(customButtonValue_4, 3, false);
                        _SetControl_Enabled(customButtonValue_5, 4, false);
                        _SetControl_Enabled(customButtonValue_6, 5, false);
                        _SetControl_Enabled(customButtonValue_7, 6, false);
                        _SetControl_Enabled(customButtonValue_8, 7, false);
                        _SetControl_Enabled(customButtonValue_9, 8, false);
                        _SetControl_Enabled(customButtonValue_10, 9, false);
                        _SetControl_Enabled(customButtonValue_11, 10, false);
                        _SetControl_Enabled(customButtonValue_12, 11, false);
                        _SetControl_Enabled(customButtonValue_13, 12, false);
                        _SetControl_Enabled(customButtonValue_14, 13, false);
                        _SetControl_Enabled(customButtonValue_15, 14, false);
                        _SetControl_Enabled(customButtonValue_16, 15, false);
                        _SetControl_Enabled(customButtonValue_17, 16, false);
                        _SetControl_Enabled(customButtonValue_18, 17, false);
                        _SetControl_Enabled(customButtonValue_19, 18, false);
                        _SetControl_Enabled(customButtonValue_20, 19, false);
                        _SetControl_Enabled(customButtonValue_21, 20, false);
                        _SetControl_Enabled(customButtonValue_22, 21, false);
                        _SetControl_Enabled(customButtonValue_23, 22, false);
                        _SetControl_Enabled(customButtonValue_24, 23, true);
                        _SetControl_Enabled(customButtonValue_25, 24, false);
                        _SetControl_Enabled(customButtonValue_26, 25, false);
                        _SetControl_Enabled(customButtonValue_27, 26, false);
                        _SetControl_Enabled(customButtonValue_28, 27, false);
                        _SetControl_Enabled(customButtonValue_29, 28, false);
                        _SetControl_Enabled(customButtonValue_30, 29, false);
                        //
                        break;
                    case 24:
                        //
                        _SetControl_Enabled(customButtonValue_1, 0, false);
                        _SetControl_Enabled(customButtonValue_2, 1, false);
                        _SetControl_Enabled(customButtonValue_3, 2, false);
                        _SetControl_Enabled(customButtonValue_4, 3, false);
                        _SetControl_Enabled(customButtonValue_5, 4, false);
                        _SetControl_Enabled(customButtonValue_6, 5, false);
                        _SetControl_Enabled(customButtonValue_7, 6, false);
                        _SetControl_Enabled(customButtonValue_8, 7, false);
                        _SetControl_Enabled(customButtonValue_9, 8, false);
                        _SetControl_Enabled(customButtonValue_10, 9, false);
                        _SetControl_Enabled(customButtonValue_11, 10, false);
                        _SetControl_Enabled(customButtonValue_12, 11, false);
                        _SetControl_Enabled(customButtonValue_13, 12, false);
                        _SetControl_Enabled(customButtonValue_14, 13, false);
                        _SetControl_Enabled(customButtonValue_15, 14, false);
                        _SetControl_Enabled(customButtonValue_16, 15, false);
                        _SetControl_Enabled(customButtonValue_17, 16, false);
                        _SetControl_Enabled(customButtonValue_18, 17, false);
                        _SetControl_Enabled(customButtonValue_19, 18, false);
                        _SetControl_Enabled(customButtonValue_20, 19, false);
                        _SetControl_Enabled(customButtonValue_21, 20, false);
                        _SetControl_Enabled(customButtonValue_22, 21, false);
                        _SetControl_Enabled(customButtonValue_23, 22, false);
                        _SetControl_Enabled(customButtonValue_24, 23, false);
                        _SetControl_Enabled(customButtonValue_25, 24, true);
                        _SetControl_Enabled(customButtonValue_26, 25, false);
                        _SetControl_Enabled(customButtonValue_27, 26, false);
                        _SetControl_Enabled(customButtonValue_28, 27, false);
                        _SetControl_Enabled(customButtonValue_29, 28, false);
                        _SetControl_Enabled(customButtonValue_30, 29, false);
                        //
                        break;
                    case 25:
                        //
                        _SetControl_Enabled(customButtonValue_1, 0, false);
                        _SetControl_Enabled(customButtonValue_2, 1, false);
                        _SetControl_Enabled(customButtonValue_3, 2, false);
                        _SetControl_Enabled(customButtonValue_4, 3, false);
                        _SetControl_Enabled(customButtonValue_5, 4, false);
                        _SetControl_Enabled(customButtonValue_6, 5, false);
                        _SetControl_Enabled(customButtonValue_7, 6, false);
                        _SetControl_Enabled(customButtonValue_8, 7, false);
                        _SetControl_Enabled(customButtonValue_9, 8, false);
                        _SetControl_Enabled(customButtonValue_10, 9, false);
                        _SetControl_Enabled(customButtonValue_11, 10, false);
                        _SetControl_Enabled(customButtonValue_12, 11, false);
                        _SetControl_Enabled(customButtonValue_13, 12, false);
                        _SetControl_Enabled(customButtonValue_14, 13, false);
                        _SetControl_Enabled(customButtonValue_15, 14, false);
                        _SetControl_Enabled(customButtonValue_16, 15, false);
                        _SetControl_Enabled(customButtonValue_17, 16, false);
                        _SetControl_Enabled(customButtonValue_18, 17, false);
                        _SetControl_Enabled(customButtonValue_19, 18, false);
                        _SetControl_Enabled(customButtonValue_20, 19, false);
                        _SetControl_Enabled(customButtonValue_21, 20, false);
                        _SetControl_Enabled(customButtonValue_22, 21, false);
                        _SetControl_Enabled(customButtonValue_23, 22, false);
                        _SetControl_Enabled(customButtonValue_24, 23, false);
                        _SetControl_Enabled(customButtonValue_25, 24, false);
                        _SetControl_Enabled(customButtonValue_26, 25, true);
                        _SetControl_Enabled(customButtonValue_27, 26, false);
                        _SetControl_Enabled(customButtonValue_28, 27, false);
                        _SetControl_Enabled(customButtonValue_29, 28, false);
                        _SetControl_Enabled(customButtonValue_30, 29, false);
                        //
                        break;
                    case 26:
                        //
                        _SetControl_Enabled(customButtonValue_1, 0, false);
                        _SetControl_Enabled(customButtonValue_2, 1, false);
                        _SetControl_Enabled(customButtonValue_3, 2, false);
                        _SetControl_Enabled(customButtonValue_4, 3, false);
                        _SetControl_Enabled(customButtonValue_5, 4, false);
                        _SetControl_Enabled(customButtonValue_6, 5, false);
                        _SetControl_Enabled(customButtonValue_7, 6, false);
                        _SetControl_Enabled(customButtonValue_8, 7, false);
                        _SetControl_Enabled(customButtonValue_9, 8, false);
                        _SetControl_Enabled(customButtonValue_10, 9, false);
                        _SetControl_Enabled(customButtonValue_11, 10, false);
                        _SetControl_Enabled(customButtonValue_12, 11, false);
                        _SetControl_Enabled(customButtonValue_13, 12, false);
                        _SetControl_Enabled(customButtonValue_14, 13, false);
                        _SetControl_Enabled(customButtonValue_15, 14, false);
                        _SetControl_Enabled(customButtonValue_16, 15, false);
                        _SetControl_Enabled(customButtonValue_17, 16, false);
                        _SetControl_Enabled(customButtonValue_18, 17, false);
                        _SetControl_Enabled(customButtonValue_19, 18, false);
                        _SetControl_Enabled(customButtonValue_20, 19, false);
                        _SetControl_Enabled(customButtonValue_21, 20, false);
                        _SetControl_Enabled(customButtonValue_22, 21, false);
                        _SetControl_Enabled(customButtonValue_23, 22, false);
                        _SetControl_Enabled(customButtonValue_24, 23, false);
                        _SetControl_Enabled(customButtonValue_25, 24, false);
                        _SetControl_Enabled(customButtonValue_26, 25, false);
                        _SetControl_Enabled(customButtonValue_27, 26, true);
                        _SetControl_Enabled(customButtonValue_28, 27, false);
                        _SetControl_Enabled(customButtonValue_29, 28, false);
                        _SetControl_Enabled(customButtonValue_30, 29, false);
                        //
                        break;
                    case 27:
                        //
                        _SetControl_Enabled(customButtonValue_1, 0, false);
                        _SetControl_Enabled(customButtonValue_2, 1, false);
                        _SetControl_Enabled(customButtonValue_3, 2, false);
                        _SetControl_Enabled(customButtonValue_4, 3, false);
                        _SetControl_Enabled(customButtonValue_5, 4, false);
                        _SetControl_Enabled(customButtonValue_6, 5, false);
                        _SetControl_Enabled(customButtonValue_7, 6, false);
                        _SetControl_Enabled(customButtonValue_8, 7, false);
                        _SetControl_Enabled(customButtonValue_9, 8, false);
                        _SetControl_Enabled(customButtonValue_10, 9, false);
                        _SetControl_Enabled(customButtonValue_11, 10, false);
                        _SetControl_Enabled(customButtonValue_12, 11, false);
                        _SetControl_Enabled(customButtonValue_13, 12, false);
                        _SetControl_Enabled(customButtonValue_14, 13, false);
                        _SetControl_Enabled(customButtonValue_15, 14, false);
                        _SetControl_Enabled(customButtonValue_16, 15, false);
                        _SetControl_Enabled(customButtonValue_17, 16, false);
                        _SetControl_Enabled(customButtonValue_18, 17, false);
                        _SetControl_Enabled(customButtonValue_19, 18, false);
                        _SetControl_Enabled(customButtonValue_20, 19, false);
                        _SetControl_Enabled(customButtonValue_21, 20, false);
                        _SetControl_Enabled(customButtonValue_22, 21, false);
                        _SetControl_Enabled(customButtonValue_23, 22, false);
                        _SetControl_Enabled(customButtonValue_24, 23, false);
                        _SetControl_Enabled(customButtonValue_25, 24, false);
                        _SetControl_Enabled(customButtonValue_26, 25, false);
                        _SetControl_Enabled(customButtonValue_27, 26, false);
                        _SetControl_Enabled(customButtonValue_28, 27, true);
                        _SetControl_Enabled(customButtonValue_29, 28, false);
                        _SetControl_Enabled(customButtonValue_30, 29, false);
                        //
                        break;
                    case 28:
                        //
                        _SetControl_Enabled(customButtonValue_1, 0, false);
                        _SetControl_Enabled(customButtonValue_2, 1, false);
                        _SetControl_Enabled(customButtonValue_3, 2, false);
                        _SetControl_Enabled(customButtonValue_4, 3, false);
                        _SetControl_Enabled(customButtonValue_5, 4, false);
                        _SetControl_Enabled(customButtonValue_6, 5, false);
                        _SetControl_Enabled(customButtonValue_7, 6, false);
                        _SetControl_Enabled(customButtonValue_8, 7, false);
                        _SetControl_Enabled(customButtonValue_9, 8, false);
                        _SetControl_Enabled(customButtonValue_10, 9, false);
                        _SetControl_Enabled(customButtonValue_11, 10, false);
                        _SetControl_Enabled(customButtonValue_12, 11, false);
                        _SetControl_Enabled(customButtonValue_13, 12, false);
                        _SetControl_Enabled(customButtonValue_14, 13, false);
                        _SetControl_Enabled(customButtonValue_15, 14, false);
                        _SetControl_Enabled(customButtonValue_16, 15, false);
                        _SetControl_Enabled(customButtonValue_17, 16, false);
                        _SetControl_Enabled(customButtonValue_18, 17, false);
                        _SetControl_Enabled(customButtonValue_19, 18, false);
                        _SetControl_Enabled(customButtonValue_20, 19, false);
                        _SetControl_Enabled(customButtonValue_21, 20, false);
                        _SetControl_Enabled(customButtonValue_22, 21, false);
                        _SetControl_Enabled(customButtonValue_23, 22, false);
                        _SetControl_Enabled(customButtonValue_24, 23, false);
                        _SetControl_Enabled(customButtonValue_25, 24, false);
                        _SetControl_Enabled(customButtonValue_26, 25, false);
                        _SetControl_Enabled(customButtonValue_27, 26, false);
                        _SetControl_Enabled(customButtonValue_28, 27, false);
                        _SetControl_Enabled(customButtonValue_29, 28, true);
                        _SetControl_Enabled(customButtonValue_30, 29, false);
                        //
                        break;
                    case 29:
                        //
                        _SetControl_Enabled(customButtonValue_1, 0, false);
                        _SetControl_Enabled(customButtonValue_2, 1, false);
                        _SetControl_Enabled(customButtonValue_3, 2, false);
                        _SetControl_Enabled(customButtonValue_4, 3, false);
                        _SetControl_Enabled(customButtonValue_5, 4, false);
                        _SetControl_Enabled(customButtonValue_6, 5, false);
                        _SetControl_Enabled(customButtonValue_7, 6, false);
                        _SetControl_Enabled(customButtonValue_8, 7, false);
                        _SetControl_Enabled(customButtonValue_9, 8, false);
                        _SetControl_Enabled(customButtonValue_10, 9, false);
                        _SetControl_Enabled(customButtonValue_11, 10, false);
                        _SetControl_Enabled(customButtonValue_12, 11, false);
                        _SetControl_Enabled(customButtonValue_13, 12, false);
                        _SetControl_Enabled(customButtonValue_14, 13, false);
                        _SetControl_Enabled(customButtonValue_15, 14, false);
                        _SetControl_Enabled(customButtonValue_16, 15, false);
                        _SetControl_Enabled(customButtonValue_17, 16, false);
                        _SetControl_Enabled(customButtonValue_18, 17, false);
                        _SetControl_Enabled(customButtonValue_19, 18, false);
                        _SetControl_Enabled(customButtonValue_20, 19, false);
                        _SetControl_Enabled(customButtonValue_21, 20, false);
                        _SetControl_Enabled(customButtonValue_22, 21, false);
                        _SetControl_Enabled(customButtonValue_23, 22, false);
                        _SetControl_Enabled(customButtonValue_24, 23, false);
                        _SetControl_Enabled(customButtonValue_25, 24, false);
                        _SetControl_Enabled(customButtonValue_26, 25, false);
                        _SetControl_Enabled(customButtonValue_27, 26, false);
                        _SetControl_Enabled(customButtonValue_28, 27, false);
                        _SetControl_Enabled(customButtonValue_29, 28, false);
                        _SetControl_Enabled(customButtonValue_30, 29, true);
                        //
                        break;
                    default:
                        //
                        _SetControl_Enabled(customButtonValue_1, 0, false);
                        _SetControl_Enabled(customButtonValue_2, 1, false);
                        _SetControl_Enabled(customButtonValue_3, 2, false);
                        _SetControl_Enabled(customButtonValue_4, 3, false);
                        _SetControl_Enabled(customButtonValue_5, 4, false);
                        _SetControl_Enabled(customButtonValue_6, 5, false);
                        _SetControl_Enabled(customButtonValue_7, 6, false);
                        _SetControl_Enabled(customButtonValue_8, 7, false);
                        _SetControl_Enabled(customButtonValue_9, 8, false);
                        _SetControl_Enabled(customButtonValue_10, 9, false);
                        _SetControl_Enabled(customButtonValue_11, 10, false);
                        _SetControl_Enabled(customButtonValue_12, 11, false);
                        _SetControl_Enabled(customButtonValue_13, 12, false);
                        _SetControl_Enabled(customButtonValue_14, 13, false);
                        _SetControl_Enabled(customButtonValue_15, 14, false);
                        _SetControl_Enabled(customButtonValue_16, 15, false);
                        _SetControl_Enabled(customButtonValue_17, 16, false);
                        _SetControl_Enabled(customButtonValue_18, 17, false);
                        _SetControl_Enabled(customButtonValue_19, 18, false);
                        _SetControl_Enabled(customButtonValue_20, 19, false);
                        _SetControl_Enabled(customButtonValue_21, 20, false);
                        _SetControl_Enabled(customButtonValue_22, 21, false);
                        _SetControl_Enabled(customButtonValue_23, 22, false);
                        _SetControl_Enabled(customButtonValue_24, 23, false);
                        _SetControl_Enabled(customButtonValue_25, 24, false);
                        _SetControl_Enabled(customButtonValue_26, 25, false);
                        _SetControl_Enabled(customButtonValue_27, 26, false);
                        _SetControl_Enabled(customButtonValue_28, 27, false);
                        _SetControl_Enabled(customButtonValue_29, 28, false);
                        _SetControl_Enabled(customButtonValue_30, 29, false);
                        //
                        break;
                }

                if (bEnterValueEnabled)//允许输入数值
                {
                    customButtonSubtract.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【-】
                    customButtonSet.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【Set】
                    customButtonPlus.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【+】
                }
                customButtonNextPage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                customButtonPreviousPage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
            }
            else//禁止
            {
                customButtonValue_1.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//数值1控件
                customButtonValue_2.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//数值2控件
                customButtonValue_3.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//数值3控件
                customButtonValue_4.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//数值4控件
                customButtonValue_5.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//数值5控件
                customButtonValue_6.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//数值6控件
                customButtonValue_7.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//数值7控件
                customButtonValue_8.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//数值8控件
                customButtonValue_9.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//数值9控件
                customButtonValue_10.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//数值10控件
                customButtonValue_11.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//数值11控件
                customButtonValue_12.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//数值12控件
                customButtonValue_13.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//数值13控件
                customButtonValue_14.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//数值14控件
                customButtonValue_15.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//数值15控件

                customButtonSubtract.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【-】
                customButtonSet.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【Set】
                customButtonPlus.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【+】

                customButtonNextPage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                customButtonPreviousPage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：获取绘制的文本
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _GetText()
        {
            try
            {
                Int32 i = 0;//循环控制变量
                Int32 iParameterIndex = 0;//循环控制变量

                if (null != sParameterValueNameDisplay && null != sParameterValueNameDisplay[0])//有效
                {
                    for (i = 0; i < sParameterValueNameDisplay.Length; i++)
                    {
                        for (iParameterIndex = 0; iParameterIndex < sParameterValueNameDisplay[i].Length; iParameterIndex++)
                        {
                            if (null == sParameterValueNameDisplay[i][iParameterIndex] || "" == sParameterValueNameDisplay[i][iParameterIndex])//无效
                            {
                                sParameterValueNameArray[i][iParameterIndex] = new String[1];

                                sParameterValueNameArray[i][iParameterIndex][0] = "";
                            }
                            else//有效
                            {
                                sParameterValueNameArray[i][iParameterIndex] = sParameterValueNameDisplay[i][iParameterIndex].Split('&');

                                //

                                Int32 j = 0;//循环控制变量
                                Boolean bValue_Temp = false;//临时变量

                                for (j = 0; j < sParameterValueNameArray[i][iParameterIndex].Length - 1; j++)//处理&&情况
                                {
                                    if ("" == sParameterValueNameArray[i][iParameterIndex][j] && "" == sParameterValueNameArray[i][iParameterIndex][j + 1])//&&情况
                                    {
                                        sParameterValueNameArray[i][iParameterIndex][j] = "&";

                                        j += 1;

                                        bValue_Temp = true;
                                    }
                                }
                                if (bValue_Temp)//处理&&情况
                                {
                                    Int32 k = 0;//循环控制变量

                                    for (j = 0; j < sParameterValueNameArray[i][iParameterIndex].Length - 1; j++)
                                    {
                                        if ("" != sParameterValueNameArray[i][iParameterIndex][j])//无效
                                        {
                                            k++;
                                        }
                                    }

                                    String[] sText_Temp = new String[k];
                                    for (j = 0; j < sParameterValueNameArray[i][iParameterIndex].Length - 1; j++)
                                    {
                                        if ("" != sParameterValueNameArray[i][iParameterIndex][j])//无效
                                        {
                                            sText_Temp[j] = sParameterValueNameArray[i][iParameterIndex][j];
                                        }
                                    }

                                    sParameterValueNameArray[i][iParameterIndex] = new String[k];
                                    sText_Temp.CopyTo(sParameterValueNameArray[i][iParameterIndex], 0);
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                //不执行操作
            }
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：设置显示的数值
        // 输入参数： 1.buttonName：名称控件
        //          2.buttonValue：数值控件
        //          3.bValid：数值是否有效。取值范围：true，是；false，否
        //          4.iIndex：参数索引值（从0开始，bValid取值为true时有效）
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _SetValue(CustomButton buttonName, CustomButton buttonValue, Boolean bValid, Int32 iIndex = 0)
        {
            try
            {
                if (null != buttonName && null != buttonValue)//有效
                {
                    if (bValid)//有效
                    {
                        buttonName.Chinese_TextDisplay = new String[1] { sParameterName[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][iIndex] };//设置显示的文本
                        buttonName.English_TextDisplay = new String[1] { sParameterName[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][iIndex] };//设置显示的文本                    
                        if (1 == iParameterType[iIndex])//枚举类型
                        {
                            buttonValue.Chinese_TextDisplay = new String[1] { sParameterValueNameArray[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][iIndex][(Int32)fParameterCurrentValue[iIndex]] };//设置显示的文本
                            buttonValue.English_TextDisplay = new String[1] { sParameterValueNameArray[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][iIndex][(Int32)fParameterCurrentValue[iIndex]] };//设置显示的文本
                        }
                        else//2，数字类型
                        {
                            buttonValue.Chinese_TextDisplay = new String[1] { fParameterCurrentValue[iIndex].ToString("F" + iParameterPrecision[iIndex].ToString()) };//设置显示的文本
                            buttonValue.English_TextDisplay = new String[1] { fParameterCurrentValue[iIndex].ToString("F" + iParameterPrecision[iIndex].ToString()) };//设置显示的文本
                        }
                    }
                    else//无效
                    {
                        buttonName.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
                        buttonName.English_TextDisplay = new String[1] { " " };//设置显示的文本
                        buttonValue.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
                        buttonValue.English_TextDisplay = new String[1] { " " };//设置显示的文本
                    }
                }
            }
            catch (System.Exception ex)
            {
                //不执行操作
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：显示或隐藏控件
        // 输入参数： 1.bShow：显示或隐藏控件。取值范围：true，显示；false，隐藏
        //          2.iButtonIndex：数值控件索引值（从0开始）
        //          3.buttonName：名称控件
        //          4.buttonValue：数值控件
        //          5.buttonValueBackground：数值背景控件
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _ShowControls(Boolean bShow, Int32 iButtonIndex, CustomButton buttonName, CustomButton buttonValue, CustomButton buttonValueBackground)
        {
            buttonName.Visible = bShow;//参数名称控件
            buttonValue.Visible = bShow;//参数数值控件
            buttonValueBackground.Visible = bShow;//参数数值背景控件

            if (bControlEnabled)//控件整体使能
            {
                if (bParameterEnabled[iButtonIndex])//使能
                {
                    buttonValue.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                }
                else//禁止
                {
                    buttonValue.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                }
            }
            else//控件整体禁止
            {
                buttonValue.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
            }
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：添加数值
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _AddValues()
        {
            switch (iValidParameterNumber)//有效的参数数目
            {
                case 1:
                    //
                    _SetValue(customButtonName_1, customButtonValue_1, true, 0);
                    _SetValue(customButtonName_2, customButtonValue_2, false);
                    _SetValue(customButtonName_3, customButtonValue_3, false);
                    _SetValue(customButtonName_4, customButtonValue_4, false);
                    _SetValue(customButtonName_5, customButtonValue_5, false);
                    _SetValue(customButtonName_6, customButtonValue_6, false);
                    _SetValue(customButtonName_7, customButtonValue_7, false);
                    _SetValue(customButtonName_8, customButtonValue_8, false);
                    _SetValue(customButtonName_9, customButtonValue_9, false);
                    _SetValue(customButtonName_10, customButtonValue_10, false);
                    _SetValue(customButtonName_11, customButtonValue_11, false);
                    _SetValue(customButtonName_12, customButtonValue_12, false);
                    _SetValue(customButtonName_13, customButtonValue_13, false);
                    _SetValue(customButtonName_14, customButtonValue_14, false);
                    _SetValue(customButtonName_15, customButtonValue_15, false);
                    _SetValue(customButtonName_16, customButtonValue_16, false);
                    _SetValue(customButtonName_17, customButtonValue_17, false);
                    _SetValue(customButtonName_18, customButtonValue_18, false);
                    _SetValue(customButtonName_19, customButtonValue_19, false);
                    _SetValue(customButtonName_20, customButtonValue_20, false);
                    _SetValue(customButtonName_21, customButtonValue_21, false);
                    _SetValue(customButtonName_22, customButtonValue_22, false);
                    _SetValue(customButtonName_23, customButtonValue_23, false);
                    _SetValue(customButtonName_24, customButtonValue_24, false);
                    _SetValue(customButtonName_25, customButtonValue_25, false);
                    _SetValue(customButtonName_26, customButtonValue_26, false);
                    _SetValue(customButtonName_27, customButtonValue_27, false);
                    _SetValue(customButtonName_28, customButtonValue_28, false);
                    _SetValue(customButtonName_29, customButtonValue_29, false);
                    _SetValue(customButtonName_30, customButtonValue_30, false);
                    
                    //

                    if (bShowSettingsButton)//显示设置按钮
                    {
                        customButtonSubtract.Location = new System.Drawing.Point(customButtonSubtract.Left, customButtonName_1.Bottom + iNameBottom_SetTop_Height);//【-】
                        customButtonSet.Location = new System.Drawing.Point(customButtonSet.Left, customButtonName_1.Bottom + iNameBottom_SetTop_Height);//【Set】
                        customButtonPlus.Location = new System.Drawing.Point(customButtonPlus.Left, customButtonName_1.Bottom + iNameBottom_SetTop_Height);//【+】

                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonSet.Bottom + iSetBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }
                    else//不显示设置按钮
                    {
                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonName_1.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonName_1.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonName_1.Bottom + iNameBottom_SetTop_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }

                    customButtonSubtract.Visible = bShowSettingsButton;//【-】
                    customButtonSet.Visible = bShowSettingsButton;//【Set】
                    customButtonPlus.Visible = bShowSettingsButton;//【+】

                    //
                    break;
                case 2:
                    //
                    _SetValue(customButtonName_1, customButtonValue_1, true, 0);
                    _SetValue(customButtonName_2, customButtonValue_2, true, 1);
                    _SetValue(customButtonName_3, customButtonValue_3, false);
                    _SetValue(customButtonName_4, customButtonValue_4, false);
                    _SetValue(customButtonName_5, customButtonValue_5, false);
                    _SetValue(customButtonName_6, customButtonValue_6, false);
                    _SetValue(customButtonName_7, customButtonValue_7, false);
                    _SetValue(customButtonName_8, customButtonValue_8, false);
                    _SetValue(customButtonName_9, customButtonValue_9, false);
                    _SetValue(customButtonName_10, customButtonValue_10, false);
                    _SetValue(customButtonName_11, customButtonValue_11, false);
                    _SetValue(customButtonName_12, customButtonValue_12, false);
                    _SetValue(customButtonName_13, customButtonValue_13, false);
                    _SetValue(customButtonName_14, customButtonValue_14, false);
                    _SetValue(customButtonName_15, customButtonValue_15, false);
                    _SetValue(customButtonName_16, customButtonValue_16, false);
                    _SetValue(customButtonName_17, customButtonValue_17, false);
                    _SetValue(customButtonName_18, customButtonValue_18, false);
                    _SetValue(customButtonName_19, customButtonValue_19, false);
                    _SetValue(customButtonName_20, customButtonValue_20, false);
                    _SetValue(customButtonName_21, customButtonValue_21, false);
                    _SetValue(customButtonName_22, customButtonValue_22, false);
                    _SetValue(customButtonName_23, customButtonValue_23, false);
                    _SetValue(customButtonName_24, customButtonValue_24, false);
                    _SetValue(customButtonName_25, customButtonValue_25, false);
                    _SetValue(customButtonName_26, customButtonValue_26, false);
                    _SetValue(customButtonName_27, customButtonValue_27, false);
                    _SetValue(customButtonName_28, customButtonValue_28, false);
                    _SetValue(customButtonName_29, customButtonValue_29, false);
                    _SetValue(customButtonName_30, customButtonValue_30, false);

                    //

                    if (bShowSettingsButton)//显示设置按钮
                    {
                        customButtonSubtract.Location = new System.Drawing.Point(customButtonSubtract.Left, customButtonName_2.Bottom + iNameBottom_SetTop_Height);//【-】
                        customButtonSet.Location = new System.Drawing.Point(customButtonSet.Left, customButtonName_2.Bottom + iNameBottom_SetTop_Height);//【Set】
                        customButtonPlus.Location = new System.Drawing.Point(customButtonPlus.Left, customButtonName_2.Bottom + iNameBottom_SetTop_Height);//【+】

                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonSet.Bottom + iSetBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }
                    else//不显示设置按钮
                    {
                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonName_2.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonName_2.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonName_2.Bottom + iNameBottom_SetTop_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }

                    customButtonSubtract.Visible = bShowSettingsButton;//【-】
                    customButtonSet.Visible = bShowSettingsButton;//【Set】
                    customButtonPlus.Visible = bShowSettingsButton;//【+】

                    //
                    break;
                case 3:
                    //
                    _SetValue(customButtonName_1, customButtonValue_1, true, 0);
                    _SetValue(customButtonName_2, customButtonValue_2, true, 1);
                    _SetValue(customButtonName_3, customButtonValue_3, true, 2);
                    _SetValue(customButtonName_4, customButtonValue_4, false);
                    _SetValue(customButtonName_5, customButtonValue_5, false);
                    _SetValue(customButtonName_6, customButtonValue_6, false);
                    _SetValue(customButtonName_7, customButtonValue_7, false);
                    _SetValue(customButtonName_8, customButtonValue_8, false);
                    _SetValue(customButtonName_9, customButtonValue_9, false);
                    _SetValue(customButtonName_10, customButtonValue_10, false);
                    _SetValue(customButtonName_11, customButtonValue_11, false);
                    _SetValue(customButtonName_12, customButtonValue_12, false);
                    _SetValue(customButtonName_13, customButtonValue_13, false);
                    _SetValue(customButtonName_14, customButtonValue_14, false);
                    _SetValue(customButtonName_15, customButtonValue_15, false);
                    _SetValue(customButtonName_16, customButtonValue_16, false);
                    _SetValue(customButtonName_17, customButtonValue_17, false);
                    _SetValue(customButtonName_18, customButtonValue_18, false);
                    _SetValue(customButtonName_19, customButtonValue_19, false);
                    _SetValue(customButtonName_20, customButtonValue_20, false);
                    _SetValue(customButtonName_21, customButtonValue_21, false);
                    _SetValue(customButtonName_22, customButtonValue_22, false);
                    _SetValue(customButtonName_23, customButtonValue_23, false);
                    _SetValue(customButtonName_24, customButtonValue_24, false);
                    _SetValue(customButtonName_25, customButtonValue_25, false);
                    _SetValue(customButtonName_26, customButtonValue_26, false);
                    _SetValue(customButtonName_27, customButtonValue_27, false);
                    _SetValue(customButtonName_28, customButtonValue_28, false);
                    _SetValue(customButtonName_29, customButtonValue_29, false);
                    _SetValue(customButtonName_30, customButtonValue_30, false);

                    //

                    if (bShowSettingsButton)//显示设置按钮
                    {
                        customButtonSubtract.Location = new System.Drawing.Point(customButtonSubtract.Left, customButtonName_3.Bottom + iNameBottom_SetTop_Height);//【-】
                        customButtonSet.Location = new System.Drawing.Point(customButtonSet.Left, customButtonName_3.Bottom + iNameBottom_SetTop_Height);//【Set】
                        customButtonPlus.Location = new System.Drawing.Point(customButtonPlus.Left, customButtonName_3.Bottom + iNameBottom_SetTop_Height);//【+】

                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonSet.Bottom + iSetBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }
                    else//不显示设置按钮
                    {
                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonName_3.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonName_3.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonName_3.Bottom + iNameBottom_SetTop_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }

                    customButtonSubtract.Visible = bShowSettingsButton;//【-】
                    customButtonSet.Visible = bShowSettingsButton;//【Set】
                    customButtonPlus.Visible = bShowSettingsButton;//【+】

                    //
                    break;
                case 4:
                    //
                    _SetValue(customButtonName_1, customButtonValue_1, true, 0);
                    _SetValue(customButtonName_2, customButtonValue_2, true, 1);
                    _SetValue(customButtonName_3, customButtonValue_3, true, 2);
                    _SetValue(customButtonName_4, customButtonValue_4, true, 3);
                    _SetValue(customButtonName_5, customButtonValue_5, false);
                    _SetValue(customButtonName_6, customButtonValue_6, false);
                    _SetValue(customButtonName_7, customButtonValue_7, false);
                    _SetValue(customButtonName_8, customButtonValue_8, false);
                    _SetValue(customButtonName_9, customButtonValue_9, false);
                    _SetValue(customButtonName_10, customButtonValue_10, false);
                    _SetValue(customButtonName_11, customButtonValue_11, false);
                    _SetValue(customButtonName_12, customButtonValue_12, false);
                    _SetValue(customButtonName_13, customButtonValue_13, false);
                    _SetValue(customButtonName_14, customButtonValue_14, false);
                    _SetValue(customButtonName_15, customButtonValue_15, false);
                    _SetValue(customButtonName_16, customButtonValue_16, false);
                    _SetValue(customButtonName_17, customButtonValue_17, false);
                    _SetValue(customButtonName_18, customButtonValue_18, false);
                    _SetValue(customButtonName_19, customButtonValue_19, false);
                    _SetValue(customButtonName_20, customButtonValue_20, false);
                    _SetValue(customButtonName_21, customButtonValue_21, false);
                    _SetValue(customButtonName_22, customButtonValue_22, false);
                    _SetValue(customButtonName_23, customButtonValue_23, false);
                    _SetValue(customButtonName_24, customButtonValue_24, false);
                    _SetValue(customButtonName_25, customButtonValue_25, false);
                    _SetValue(customButtonName_26, customButtonValue_26, false);
                    _SetValue(customButtonName_27, customButtonValue_27, false);
                    _SetValue(customButtonName_28, customButtonValue_28, false);
                    _SetValue(customButtonName_29, customButtonValue_29, false);
                    _SetValue(customButtonName_30, customButtonValue_30, false);

                    //

                    if (bShowSettingsButton)//显示设置按钮
                    {
                        customButtonSubtract.Location = new System.Drawing.Point(customButtonSubtract.Left, customButtonName_4.Bottom + iNameBottom_SetTop_Height);//【-】
                        customButtonSet.Location = new System.Drawing.Point(customButtonSet.Left, customButtonName_4.Bottom + iNameBottom_SetTop_Height);//【Set】
                        customButtonPlus.Location = new System.Drawing.Point(customButtonPlus.Left, customButtonName_4.Bottom + iNameBottom_SetTop_Height);//【+】

                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonSet.Bottom + iSetBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }
                    else//不显示设置按钮
                    {
                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonName_4.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonName_4.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonName_4.Bottom + iNameBottom_SetTop_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }

                    customButtonSubtract.Visible = bShowSettingsButton;//【-】
                    customButtonSet.Visible = bShowSettingsButton;//【Set】
                    customButtonPlus.Visible = bShowSettingsButton;//【+】

                    //
                    break;
                case 5:
                    //
                    _SetValue(customButtonName_1, customButtonValue_1, true, 0);
                    _SetValue(customButtonName_2, customButtonValue_2, true, 1);
                    _SetValue(customButtonName_3, customButtonValue_3, true, 2);
                    _SetValue(customButtonName_4, customButtonValue_4, true, 3);
                    _SetValue(customButtonName_5, customButtonValue_5, true, 4);
                    _SetValue(customButtonName_6, customButtonValue_6, false);
                    _SetValue(customButtonName_7, customButtonValue_7, false);
                    _SetValue(customButtonName_8, customButtonValue_8, false);
                    _SetValue(customButtonName_9, customButtonValue_9, false);
                    _SetValue(customButtonName_10, customButtonValue_10, false);
                    _SetValue(customButtonName_11, customButtonValue_11, false);
                    _SetValue(customButtonName_12, customButtonValue_12, false);
                    _SetValue(customButtonName_13, customButtonValue_13, false);
                    _SetValue(customButtonName_14, customButtonValue_14, false);
                    _SetValue(customButtonName_15, customButtonValue_15, false);
                    _SetValue(customButtonName_16, customButtonValue_16, false);
                    _SetValue(customButtonName_17, customButtonValue_17, false);
                    _SetValue(customButtonName_18, customButtonValue_18, false);
                    _SetValue(customButtonName_19, customButtonValue_19, false);
                    _SetValue(customButtonName_20, customButtonValue_20, false);
                    _SetValue(customButtonName_21, customButtonValue_21, false);
                    _SetValue(customButtonName_22, customButtonValue_22, false);
                    _SetValue(customButtonName_23, customButtonValue_23, false);
                    _SetValue(customButtonName_24, customButtonValue_24, false);
                    _SetValue(customButtonName_25, customButtonValue_25, false);
                    _SetValue(customButtonName_26, customButtonValue_26, false);
                    _SetValue(customButtonName_27, customButtonValue_27, false);
                    _SetValue(customButtonName_28, customButtonValue_28, false);
                    _SetValue(customButtonName_29, customButtonValue_29, false);
                    _SetValue(customButtonName_30, customButtonValue_30, false);

                    //

                    if (bShowSettingsButton)//显示设置按钮
                    {
                        customButtonSubtract.Location = new System.Drawing.Point(customButtonSubtract.Left, customButtonName_5.Bottom + iNameBottom_SetTop_Height);//【-】
                        customButtonSet.Location = new System.Drawing.Point(customButtonSet.Left, customButtonName_5.Bottom + iNameBottom_SetTop_Height);//【Set】
                        customButtonPlus.Location = new System.Drawing.Point(customButtonPlus.Left, customButtonName_5.Bottom + iNameBottom_SetTop_Height);//【+】

                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonSet.Bottom + iSetBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }
                    else//不显示设置按钮
                    {
                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonName_5.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonName_5.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonName_5.Bottom + iNameBottom_SetTop_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }

                    customButtonSubtract.Visible = bShowSettingsButton;//【-】
                    customButtonSet.Visible = bShowSettingsButton;//【Set】
                    customButtonPlus.Visible = bShowSettingsButton;//【+】

                    //
                    break;
                case 6:
                    //
                    _SetValue(customButtonName_1, customButtonValue_1, true, 0);
                    _SetValue(customButtonName_2, customButtonValue_2, true, 1);
                    _SetValue(customButtonName_3, customButtonValue_3, true, 2);
                    _SetValue(customButtonName_4, customButtonValue_4, true, 3);
                    _SetValue(customButtonName_5, customButtonValue_5, true, 4);
                    _SetValue(customButtonName_6, customButtonValue_6, true, 5);
                    _SetValue(customButtonName_7, customButtonValue_7, false);
                    _SetValue(customButtonName_8, customButtonValue_8, false);
                    _SetValue(customButtonName_9, customButtonValue_9, false);
                    _SetValue(customButtonName_10, customButtonValue_10, false);
                    _SetValue(customButtonName_11, customButtonValue_11, false);
                    _SetValue(customButtonName_12, customButtonValue_12, false);
                    _SetValue(customButtonName_13, customButtonValue_13, false);
                    _SetValue(customButtonName_14, customButtonValue_14, false);
                    _SetValue(customButtonName_15, customButtonValue_15, false);
                    _SetValue(customButtonName_16, customButtonValue_16, false);
                    _SetValue(customButtonName_17, customButtonValue_17, false);
                    _SetValue(customButtonName_18, customButtonValue_18, false);
                    _SetValue(customButtonName_19, customButtonValue_19, false);
                    _SetValue(customButtonName_20, customButtonValue_20, false);
                    _SetValue(customButtonName_21, customButtonValue_21, false);
                    _SetValue(customButtonName_22, customButtonValue_22, false);
                    _SetValue(customButtonName_23, customButtonValue_23, false);
                    _SetValue(customButtonName_24, customButtonValue_24, false);
                    _SetValue(customButtonName_25, customButtonValue_25, false);
                    _SetValue(customButtonName_26, customButtonValue_26, false);
                    _SetValue(customButtonName_27, customButtonValue_27, false);
                    _SetValue(customButtonName_28, customButtonValue_28, false);
                    _SetValue(customButtonName_29, customButtonValue_29, false);
                    _SetValue(customButtonName_30, customButtonValue_30, false);
                    
                    //

                    if (bShowSettingsButton)//显示设置按钮
                    {
                        customButtonSubtract.Location = new System.Drawing.Point(customButtonSubtract.Left, customButtonName_6.Bottom + iNameBottom_SetTop_Height);//【-】
                        customButtonSet.Location = new System.Drawing.Point(customButtonSet.Left, customButtonName_6.Bottom + iNameBottom_SetTop_Height);//【Set】
                        customButtonPlus.Location = new System.Drawing.Point(customButtonPlus.Left, customButtonName_6.Bottom + iNameBottom_SetTop_Height);//【+】

                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonSet.Bottom + iSetBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }
                    else//不显示设置按钮
                    {
                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonName_6.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonName_6.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonName_6.Bottom + iNameBottom_SetTop_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }

                    customButtonSubtract.Visible = bShowSettingsButton;//【-】
                    customButtonSet.Visible = bShowSettingsButton;//【Set】
                    customButtonPlus.Visible = bShowSettingsButton;//【+】

                    //
                    break;
                case 7:
                    //
                    _SetValue(customButtonName_1, customButtonValue_1, true, 0);
                    _SetValue(customButtonName_2, customButtonValue_2, true, 1);
                    _SetValue(customButtonName_3, customButtonValue_3, true, 2);
                    _SetValue(customButtonName_4, customButtonValue_4, true, 3);
                    _SetValue(customButtonName_5, customButtonValue_5, true, 4);
                    _SetValue(customButtonName_6, customButtonValue_6, true, 5);
                    _SetValue(customButtonName_7, customButtonValue_7, true, 6);
                    _SetValue(customButtonName_8, customButtonValue_8, false);
                    _SetValue(customButtonName_9, customButtonValue_9, false);
                    _SetValue(customButtonName_10, customButtonValue_10, false);
                    _SetValue(customButtonName_11, customButtonValue_11, false);
                    _SetValue(customButtonName_12, customButtonValue_12, false);
                    _SetValue(customButtonName_13, customButtonValue_13, false);
                    _SetValue(customButtonName_14, customButtonValue_14, false);
                    _SetValue(customButtonName_15, customButtonValue_15, false);
                    _SetValue(customButtonName_16, customButtonValue_16, false);
                    _SetValue(customButtonName_17, customButtonValue_17, false);
                    _SetValue(customButtonName_18, customButtonValue_18, false);
                    _SetValue(customButtonName_19, customButtonValue_19, false);
                    _SetValue(customButtonName_20, customButtonValue_20, false);
                    _SetValue(customButtonName_21, customButtonValue_21, false);
                    _SetValue(customButtonName_22, customButtonValue_22, false);
                    _SetValue(customButtonName_23, customButtonValue_23, false);
                    _SetValue(customButtonName_24, customButtonValue_24, false);
                    _SetValue(customButtonName_25, customButtonValue_25, false);
                    _SetValue(customButtonName_26, customButtonValue_26, false);
                    _SetValue(customButtonName_27, customButtonValue_27, false);
                    _SetValue(customButtonName_28, customButtonValue_28, false);
                    _SetValue(customButtonName_29, customButtonValue_29, false);
                    _SetValue(customButtonName_30, customButtonValue_30, false);

                    //

                    if (bShowSettingsButton)//显示设置按钮
                    {
                        customButtonSubtract.Location = new System.Drawing.Point(customButtonSubtract.Left, customButtonName_7.Bottom + iNameBottom_SetTop_Height);//【-】
                        customButtonSet.Location = new System.Drawing.Point(customButtonSet.Left, customButtonName_7.Bottom + iNameBottom_SetTop_Height);//【Set】
                        customButtonPlus.Location = new System.Drawing.Point(customButtonPlus.Left, customButtonName_7.Bottom + iNameBottom_SetTop_Height);//【+】

                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonSet.Bottom + iSetBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }
                    else//不显示设置按钮
                    {
                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonName_7.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonName_7.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonName_7.Bottom + iNameBottom_SetTop_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }

                    customButtonSubtract.Visible = bShowSettingsButton;//【-】
                    customButtonSet.Visible = bShowSettingsButton;//【Set】
                    customButtonPlus.Visible = bShowSettingsButton;//【+】

                    //
                    break;
                case 8:
                    //
                    _SetValue(customButtonName_1, customButtonValue_1, true, 0);
                    _SetValue(customButtonName_2, customButtonValue_2, true, 1);
                    _SetValue(customButtonName_3, customButtonValue_3, true, 2);
                    _SetValue(customButtonName_4, customButtonValue_4, true, 3);
                    _SetValue(customButtonName_5, customButtonValue_5, true, 4);
                    _SetValue(customButtonName_6, customButtonValue_6, true, 5);
                    _SetValue(customButtonName_7, customButtonValue_7, true, 6);
                    _SetValue(customButtonName_8, customButtonValue_8, true, 7);
                    _SetValue(customButtonName_9, customButtonValue_9, false);
                    _SetValue(customButtonName_10, customButtonValue_10, false);
                    _SetValue(customButtonName_11, customButtonValue_11, false);
                    _SetValue(customButtonName_12, customButtonValue_12, false);
                    _SetValue(customButtonName_13, customButtonValue_13, false);
                    _SetValue(customButtonName_14, customButtonValue_14, false);
                    _SetValue(customButtonName_15, customButtonValue_15, false);
                    _SetValue(customButtonName_16, customButtonValue_16, false);
                    _SetValue(customButtonName_17, customButtonValue_17, false);
                    _SetValue(customButtonName_18, customButtonValue_18, false);
                    _SetValue(customButtonName_19, customButtonValue_19, false);
                    _SetValue(customButtonName_20, customButtonValue_20, false);
                    _SetValue(customButtonName_21, customButtonValue_21, false);
                    _SetValue(customButtonName_22, customButtonValue_22, false);
                    _SetValue(customButtonName_23, customButtonValue_23, false);
                    _SetValue(customButtonName_24, customButtonValue_24, false);
                    _SetValue(customButtonName_25, customButtonValue_25, false);
                    _SetValue(customButtonName_26, customButtonValue_26, false);
                    _SetValue(customButtonName_27, customButtonValue_27, false);
                    _SetValue(customButtonName_28, customButtonValue_28, false);
                    _SetValue(customButtonName_29, customButtonValue_29, false);
                    _SetValue(customButtonName_30, customButtonValue_30, false);

                    //

                    if (bShowSettingsButton)//显示设置按钮
                    {
                        customButtonSubtract.Location = new System.Drawing.Point(customButtonSubtract.Left, customButtonName_8.Bottom + iNameBottom_SetTop_Height);//【-】
                        customButtonSet.Location = new System.Drawing.Point(customButtonSet.Left, customButtonName_8.Bottom + iNameBottom_SetTop_Height);//【Set】
                        customButtonPlus.Location = new System.Drawing.Point(customButtonPlus.Left, customButtonName_8.Bottom + iNameBottom_SetTop_Height);//【+】

                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonSet.Bottom + iSetBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }
                    else//不显示设置按钮
                    {
                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonName_8.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonName_8.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonName_8.Bottom + iNameBottom_SetTop_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }

                    customButtonSubtract.Visible = bShowSettingsButton;//【-】
                    customButtonSet.Visible = bShowSettingsButton;//【Set】
                    customButtonPlus.Visible = bShowSettingsButton;//【+】

                    //
                    break;
                case 9:
                    //
                    _SetValue(customButtonName_1, customButtonValue_1, true, 0);
                    _SetValue(customButtonName_2, customButtonValue_2, true, 1);
                    _SetValue(customButtonName_3, customButtonValue_3, true, 2);
                    _SetValue(customButtonName_4, customButtonValue_4, true, 3);
                    _SetValue(customButtonName_5, customButtonValue_5, true, 4);
                    _SetValue(customButtonName_6, customButtonValue_6, true, 5);
                    _SetValue(customButtonName_7, customButtonValue_7, true, 6);
                    _SetValue(customButtonName_8, customButtonValue_8, true, 7);
                    _SetValue(customButtonName_9, customButtonValue_9, true, 8);
                    _SetValue(customButtonName_10, customButtonValue_10, false);
                    _SetValue(customButtonName_11, customButtonValue_11, false);
                    _SetValue(customButtonName_12, customButtonValue_12, false);
                    _SetValue(customButtonName_13, customButtonValue_13, false);
                    _SetValue(customButtonName_14, customButtonValue_14, false);
                    _SetValue(customButtonName_15, customButtonValue_15, false);
                    _SetValue(customButtonName_16, customButtonValue_16, false);
                    _SetValue(customButtonName_17, customButtonValue_17, false);
                    _SetValue(customButtonName_18, customButtonValue_18, false);
                    _SetValue(customButtonName_19, customButtonValue_19, false);
                    _SetValue(customButtonName_20, customButtonValue_20, false);
                    _SetValue(customButtonName_21, customButtonValue_21, false);
                    _SetValue(customButtonName_22, customButtonValue_22, false);
                    _SetValue(customButtonName_23, customButtonValue_23, false);
                    _SetValue(customButtonName_24, customButtonValue_24, false);
                    _SetValue(customButtonName_25, customButtonValue_25, false);
                    _SetValue(customButtonName_26, customButtonValue_26, false);
                    _SetValue(customButtonName_27, customButtonValue_27, false);
                    _SetValue(customButtonName_28, customButtonValue_28, false);
                    _SetValue(customButtonName_29, customButtonValue_29, false);
                    _SetValue(customButtonName_30, customButtonValue_30, false);

                    //

                    if (bShowSettingsButton)//显示设置按钮
                    {
                        customButtonSubtract.Location = new System.Drawing.Point(customButtonSubtract.Left, customButtonName_9.Bottom + iNameBottom_SetTop_Height);//【-】
                        customButtonSet.Location = new System.Drawing.Point(customButtonSet.Left, customButtonName_9.Bottom + iNameBottom_SetTop_Height);//【Set】
                        customButtonPlus.Location = new System.Drawing.Point(customButtonPlus.Left, customButtonName_9.Bottom + iNameBottom_SetTop_Height);//【+】

                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonSet.Bottom + iSetBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }
                    else//不显示设置按钮
                    {
                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonName_9.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonName_9.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonName_9.Bottom + iNameBottom_SetTop_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }

                    customButtonSubtract.Visible = bShowSettingsButton;//【-】
                    customButtonSet.Visible = bShowSettingsButton;//【Set】
                    customButtonPlus.Visible = bShowSettingsButton;//【+】

                    //
                    break;
                case 10:
                    //
                    _SetValue(customButtonName_1, customButtonValue_1, true, 0);
                    _SetValue(customButtonName_2, customButtonValue_2, true, 1);
                    _SetValue(customButtonName_3, customButtonValue_3, true, 2);
                    _SetValue(customButtonName_4, customButtonValue_4, true, 3);
                    _SetValue(customButtonName_5, customButtonValue_5, true, 4);
                    _SetValue(customButtonName_6, customButtonValue_6, true, 5);
                    _SetValue(customButtonName_7, customButtonValue_7, true, 6);
                    _SetValue(customButtonName_8, customButtonValue_8, true, 7);
                    _SetValue(customButtonName_9, customButtonValue_9, true, 8);
                    _SetValue(customButtonName_10, customButtonValue_10, true, 9);
                    _SetValue(customButtonName_11, customButtonValue_11, false);
                    _SetValue(customButtonName_12, customButtonValue_12, false);
                    _SetValue(customButtonName_13, customButtonValue_13, false);
                    _SetValue(customButtonName_14, customButtonValue_14, false);
                    _SetValue(customButtonName_15, customButtonValue_15, false);
                    _SetValue(customButtonName_16, customButtonValue_16, false);
                    _SetValue(customButtonName_17, customButtonValue_17, false);
                    _SetValue(customButtonName_18, customButtonValue_18, false);
                    _SetValue(customButtonName_19, customButtonValue_19, false);
                    _SetValue(customButtonName_20, customButtonValue_20, false);
                    _SetValue(customButtonName_21, customButtonValue_21, false);
                    _SetValue(customButtonName_22, customButtonValue_22, false);
                    _SetValue(customButtonName_23, customButtonValue_23, false);
                    _SetValue(customButtonName_24, customButtonValue_24, false);
                    _SetValue(customButtonName_25, customButtonValue_25, false);
                    _SetValue(customButtonName_26, customButtonValue_26, false);
                    _SetValue(customButtonName_27, customButtonValue_27, false);
                    _SetValue(customButtonName_28, customButtonValue_28, false);
                    _SetValue(customButtonName_29, customButtonValue_29, false);
                    _SetValue(customButtonName_30, customButtonValue_30, false);

                    //

                    if (bShowSettingsButton)//显示设置按钮
                    {
                        customButtonSubtract.Location = new System.Drawing.Point(customButtonSubtract.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【-】
                        customButtonSet.Location = new System.Drawing.Point(customButtonSet.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【Set】
                        customButtonPlus.Location = new System.Drawing.Point(customButtonPlus.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【+】

                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonSet.Bottom + iSetBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }
                    else//不显示设置按钮
                    {
                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonName_10.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonName_10.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonName_10.Bottom + iNameBottom_SetTop_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }

                    customButtonSubtract.Visible = bShowSettingsButton;//【-】
                    customButtonSet.Visible = bShowSettingsButton;//【Set】
                    customButtonPlus.Visible = bShowSettingsButton;//【+】

                    //
                    break;
                case 11:
                    //
                    _SetValue(customButtonName_1, customButtonValue_1, true, 0);
                    _SetValue(customButtonName_2, customButtonValue_2, true, 1);
                    _SetValue(customButtonName_3, customButtonValue_3, true, 2);
                    _SetValue(customButtonName_4, customButtonValue_4, true, 3);
                    _SetValue(customButtonName_5, customButtonValue_5, true, 4);
                    _SetValue(customButtonName_6, customButtonValue_6, true, 5);
                    _SetValue(customButtonName_7, customButtonValue_7, true, 6);
                    _SetValue(customButtonName_8, customButtonValue_8, true, 7);
                    _SetValue(customButtonName_9, customButtonValue_9, true, 8);
                    _SetValue(customButtonName_10, customButtonValue_10, true, 9);
                    _SetValue(customButtonName_11, customButtonValue_11, true, 10);
                    _SetValue(customButtonName_12, customButtonValue_12, false);
                    _SetValue(customButtonName_13, customButtonValue_13, false);
                    _SetValue(customButtonName_14, customButtonValue_14, false);
                    _SetValue(customButtonName_15, customButtonValue_15, false);
                    _SetValue(customButtonName_16, customButtonValue_16, false);
                    _SetValue(customButtonName_17, customButtonValue_17, false);
                    _SetValue(customButtonName_18, customButtonValue_18, false);
                    _SetValue(customButtonName_19, customButtonValue_19, false);
                    _SetValue(customButtonName_20, customButtonValue_20, false);
                    _SetValue(customButtonName_21, customButtonValue_21, false);
                    _SetValue(customButtonName_22, customButtonValue_22, false);
                    _SetValue(customButtonName_23, customButtonValue_23, false);
                    _SetValue(customButtonName_24, customButtonValue_24, false);
                    _SetValue(customButtonName_25, customButtonValue_25, false);
                    _SetValue(customButtonName_26, customButtonValue_26, false);
                    _SetValue(customButtonName_27, customButtonValue_27, false);
                    _SetValue(customButtonName_28, customButtonValue_28, false);
                    _SetValue(customButtonName_29, customButtonValue_29, false);
                    _SetValue(customButtonName_30, customButtonValue_30, false);

                    //

                    if (bShowSettingsButton)//显示设置按钮
                    {
                        customButtonSubtract.Location = new System.Drawing.Point(customButtonSubtract.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【-】
                        customButtonSet.Location = new System.Drawing.Point(customButtonSet.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【Set】
                        customButtonPlus.Location = new System.Drawing.Point(customButtonPlus.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【+】

                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonSet.Bottom + iSetBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }
                    else//不显示设置按钮
                    {
                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonName_10.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonName_10.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonName_10.Bottom + iNameBottom_SetTop_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }

                    customButtonSubtract.Visible = bShowSettingsButton;//【-】
                    customButtonSet.Visible = bShowSettingsButton;//【Set】
                    customButtonPlus.Visible = bShowSettingsButton;//【+】

                    //
                    break;
                case 12:
                    //
                    _SetValue(customButtonName_1, customButtonValue_1, true, 0);
                    _SetValue(customButtonName_2, customButtonValue_2, true, 1);
                    _SetValue(customButtonName_3, customButtonValue_3, true, 2);
                    _SetValue(customButtonName_4, customButtonValue_4, true, 3);
                    _SetValue(customButtonName_5, customButtonValue_5, true, 4);
                    _SetValue(customButtonName_6, customButtonValue_6, true, 5);
                    _SetValue(customButtonName_7, customButtonValue_7, true, 6);
                    _SetValue(customButtonName_8, customButtonValue_8, true, 7);
                    _SetValue(customButtonName_9, customButtonValue_9, true, 8);
                    _SetValue(customButtonName_10, customButtonValue_10, true, 9);
                    _SetValue(customButtonName_11, customButtonValue_11, true, 10);
                    _SetValue(customButtonName_12, customButtonValue_12, true, 11);
                    _SetValue(customButtonName_13, customButtonValue_13, false);
                    _SetValue(customButtonName_14, customButtonValue_14, false);
                    _SetValue(customButtonName_15, customButtonValue_15, false);
                    _SetValue(customButtonName_16, customButtonValue_16, false);
                    _SetValue(customButtonName_17, customButtonValue_17, false);
                    _SetValue(customButtonName_18, customButtonValue_18, false);
                    _SetValue(customButtonName_19, customButtonValue_19, false);
                    _SetValue(customButtonName_20, customButtonValue_20, false);
                    _SetValue(customButtonName_21, customButtonValue_21, false);
                    _SetValue(customButtonName_22, customButtonValue_22, false);
                    _SetValue(customButtonName_23, customButtonValue_23, false);
                    _SetValue(customButtonName_24, customButtonValue_24, false);
                    _SetValue(customButtonName_25, customButtonValue_25, false);
                    _SetValue(customButtonName_26, customButtonValue_26, false);
                    _SetValue(customButtonName_27, customButtonValue_27, false);
                    _SetValue(customButtonName_28, customButtonValue_28, false);
                    _SetValue(customButtonName_29, customButtonValue_29, false);
                    _SetValue(customButtonName_30, customButtonValue_30, false);

                    //

                    if (bShowSettingsButton)//显示设置按钮
                    {
                        customButtonSubtract.Location = new System.Drawing.Point(customButtonSubtract.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【-】
                        customButtonSet.Location = new System.Drawing.Point(customButtonSet.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【Set】
                        customButtonPlus.Location = new System.Drawing.Point(customButtonPlus.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【+】

                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonSet.Bottom + iSetBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }
                    else//不显示设置按钮
                    {
                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonName_10.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonName_10.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonName_10.Bottom + iNameBottom_SetTop_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }

                    customButtonSubtract.Visible = bShowSettingsButton;//【-】
                    customButtonSet.Visible = bShowSettingsButton;//【Set】
                    customButtonPlus.Visible = bShowSettingsButton;//【+】

                    //
                    break;
                case 13:
                    //
                    _SetValue(customButtonName_1, customButtonValue_1, true, 0);
                    _SetValue(customButtonName_2, customButtonValue_2, true, 1);
                    _SetValue(customButtonName_3, customButtonValue_3, true, 2);
                    _SetValue(customButtonName_4, customButtonValue_4, true, 3);
                    _SetValue(customButtonName_5, customButtonValue_5, true, 4);
                    _SetValue(customButtonName_6, customButtonValue_6, true, 5);
                    _SetValue(customButtonName_7, customButtonValue_7, true, 6);
                    _SetValue(customButtonName_8, customButtonValue_8, true, 7);
                    _SetValue(customButtonName_9, customButtonValue_9, true, 8);
                    _SetValue(customButtonName_10, customButtonValue_10, true, 9);
                    _SetValue(customButtonName_11, customButtonValue_11, true, 10);
                    _SetValue(customButtonName_12, customButtonValue_12, true, 11);
                    _SetValue(customButtonName_13, customButtonValue_13, true, 12);
                    _SetValue(customButtonName_14, customButtonValue_14, false);
                    _SetValue(customButtonName_15, customButtonValue_15, false);
                    _SetValue(customButtonName_16, customButtonValue_16, false);
                    _SetValue(customButtonName_17, customButtonValue_17, false);
                    _SetValue(customButtonName_18, customButtonValue_18, false);
                    _SetValue(customButtonName_19, customButtonValue_19, false);
                    _SetValue(customButtonName_20, customButtonValue_20, false);
                    _SetValue(customButtonName_21, customButtonValue_21, false);
                    _SetValue(customButtonName_22, customButtonValue_22, false);
                    _SetValue(customButtonName_23, customButtonValue_23, false);
                    _SetValue(customButtonName_24, customButtonValue_24, false);
                    _SetValue(customButtonName_25, customButtonValue_25, false);
                    _SetValue(customButtonName_26, customButtonValue_26, false);
                    _SetValue(customButtonName_27, customButtonValue_27, false);
                    _SetValue(customButtonName_28, customButtonValue_28, false);
                    _SetValue(customButtonName_29, customButtonValue_29, false);
                    _SetValue(customButtonName_30, customButtonValue_30, false);

                    //

                    if (bShowSettingsButton)//显示设置按钮
                    {
                        customButtonSubtract.Location = new System.Drawing.Point(customButtonSubtract.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【-】
                        customButtonSet.Location = new System.Drawing.Point(customButtonSet.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【Set】
                        customButtonPlus.Location = new System.Drawing.Point(customButtonPlus.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【+】

                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonSet.Bottom + iSetBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }
                    else//不显示设置按钮
                    {
                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonName_10.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonName_10.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonName_10.Bottom + iNameBottom_SetTop_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }

                    customButtonSubtract.Visible = bShowSettingsButton;//【-】
                    customButtonSet.Visible = bShowSettingsButton;//【Set】
                    customButtonPlus.Visible = bShowSettingsButton;//【+】

                    //
                    break;
                case 14:
                    //
                    _SetValue(customButtonName_1, customButtonValue_1, true, 0);
                    _SetValue(customButtonName_2, customButtonValue_2, true, 1);
                    _SetValue(customButtonName_3, customButtonValue_3, true, 2);
                    _SetValue(customButtonName_4, customButtonValue_4, true, 3);
                    _SetValue(customButtonName_5, customButtonValue_5, true, 4);
                    _SetValue(customButtonName_6, customButtonValue_6, true, 5);
                    _SetValue(customButtonName_7, customButtonValue_7, true, 6);
                    _SetValue(customButtonName_8, customButtonValue_8, true, 7);
                    _SetValue(customButtonName_9, customButtonValue_9, true, 8);
                    _SetValue(customButtonName_10, customButtonValue_10, true, 9);
                    _SetValue(customButtonName_11, customButtonValue_11, true, 10);
                    _SetValue(customButtonName_12, customButtonValue_12, true, 11);
                    _SetValue(customButtonName_13, customButtonValue_13, true, 12);
                    _SetValue(customButtonName_14, customButtonValue_14, true, 13);
                    _SetValue(customButtonName_15, customButtonValue_15, false);
                    _SetValue(customButtonName_16, customButtonValue_16, false);
                    _SetValue(customButtonName_17, customButtonValue_17, false);
                    _SetValue(customButtonName_18, customButtonValue_18, false);
                    _SetValue(customButtonName_19, customButtonValue_19, false);
                    _SetValue(customButtonName_20, customButtonValue_20, false);
                    _SetValue(customButtonName_21, customButtonValue_21, false);
                    _SetValue(customButtonName_22, customButtonValue_22, false);
                    _SetValue(customButtonName_23, customButtonValue_23, false);
                    _SetValue(customButtonName_24, customButtonValue_24, false);
                    _SetValue(customButtonName_25, customButtonValue_25, false);
                    _SetValue(customButtonName_26, customButtonValue_26, false);
                    _SetValue(customButtonName_27, customButtonValue_27, false);
                    _SetValue(customButtonName_28, customButtonValue_28, false);
                    _SetValue(customButtonName_29, customButtonValue_29, false);
                    _SetValue(customButtonName_30, customButtonValue_30, false);

                    //

                    if (bShowSettingsButton)//显示设置按钮
                    {
                        customButtonSubtract.Location = new System.Drawing.Point(customButtonSubtract.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【-】
                        customButtonSet.Location = new System.Drawing.Point(customButtonSet.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【Set】
                        customButtonPlus.Location = new System.Drawing.Point(customButtonPlus.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【+】

                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonSet.Bottom + iSetBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }
                    else//不显示设置按钮
                    {
                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonName_10.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonName_10.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonName_10.Bottom + iNameBottom_SetTop_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }

                    customButtonSubtract.Visible = bShowSettingsButton;//【-】
                    customButtonSet.Visible = bShowSettingsButton;//【Set】
                    customButtonPlus.Visible = bShowSettingsButton;//【+】

                    //
                    break;
                case 15:
                    //
                    _SetValue(customButtonName_1, customButtonValue_1, true, 0);
                    _SetValue(customButtonName_2, customButtonValue_2, true, 1);
                    _SetValue(customButtonName_3, customButtonValue_3, true, 2);
                    _SetValue(customButtonName_4, customButtonValue_4, true, 3);
                    _SetValue(customButtonName_5, customButtonValue_5, true, 4);
                    _SetValue(customButtonName_6, customButtonValue_6, true, 5);
                    _SetValue(customButtonName_7, customButtonValue_7, true, 6);
                    _SetValue(customButtonName_8, customButtonValue_8, true, 7);
                    _SetValue(customButtonName_9, customButtonValue_9, true, 8);
                    _SetValue(customButtonName_10, customButtonValue_10, true, 9);
                    _SetValue(customButtonName_11, customButtonValue_11, true, 10);
                    _SetValue(customButtonName_12, customButtonValue_12, true, 11);
                    _SetValue(customButtonName_13, customButtonValue_13, true, 12);
                    _SetValue(customButtonName_14, customButtonValue_14, true, 13);
                    _SetValue(customButtonName_15, customButtonValue_15, true, 14);
                    _SetValue(customButtonName_16, customButtonValue_16, false);
                    _SetValue(customButtonName_17, customButtonValue_17, false);
                    _SetValue(customButtonName_18, customButtonValue_18, false);
                    _SetValue(customButtonName_19, customButtonValue_19, false);
                    _SetValue(customButtonName_20, customButtonValue_20, false);
                    _SetValue(customButtonName_21, customButtonValue_21, false);
                    _SetValue(customButtonName_22, customButtonValue_22, false);
                    _SetValue(customButtonName_23, customButtonValue_23, false);
                    _SetValue(customButtonName_24, customButtonValue_24, false);
                    _SetValue(customButtonName_25, customButtonValue_25, false);
                    _SetValue(customButtonName_26, customButtonValue_26, false);
                    _SetValue(customButtonName_27, customButtonValue_27, false);
                    _SetValue(customButtonName_28, customButtonValue_28, false);
                    _SetValue(customButtonName_29, customButtonValue_29, false);
                    _SetValue(customButtonName_30, customButtonValue_30, false);

                    //

                    if (bShowSettingsButton)//显示设置按钮
                    {
                        customButtonSubtract.Location = new System.Drawing.Point(customButtonSubtract.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【-】
                        customButtonSet.Location = new System.Drawing.Point(customButtonSet.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【Set】
                        customButtonPlus.Location = new System.Drawing.Point(customButtonPlus.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【+】

                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonSet.Bottom + iSetBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }
                    else//不显示设置按钮
                    {
                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonName_10.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonName_10.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonName_10.Bottom + iNameBottom_SetTop_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }

                    customButtonSubtract.Visible = bShowSettingsButton;//【-】
                    customButtonSet.Visible = bShowSettingsButton;//【Set】
                    customButtonPlus.Visible = bShowSettingsButton;//【+】

                    //
                    break;
                case 16:
                    //
                    _SetValue(customButtonName_1, customButtonValue_1, true, 0);
                    _SetValue(customButtonName_2, customButtonValue_2, true, 1);
                    _SetValue(customButtonName_3, customButtonValue_3, true, 2);
                    _SetValue(customButtonName_4, customButtonValue_4, true, 3);
                    _SetValue(customButtonName_5, customButtonValue_5, true, 4);
                    _SetValue(customButtonName_6, customButtonValue_6, true, 5);
                    _SetValue(customButtonName_7, customButtonValue_7, true, 6);
                    _SetValue(customButtonName_8, customButtonValue_8, true, 7);
                    _SetValue(customButtonName_9, customButtonValue_9, true, 8);
                    _SetValue(customButtonName_10, customButtonValue_10, true, 9);
                    _SetValue(customButtonName_11, customButtonValue_11, true, 10);
                    _SetValue(customButtonName_12, customButtonValue_12, true, 11);
                    _SetValue(customButtonName_13, customButtonValue_13, true, 12);
                    _SetValue(customButtonName_14, customButtonValue_14, true, 13);
                    _SetValue(customButtonName_15, customButtonValue_15, true, 14);
                    _SetValue(customButtonName_16, customButtonValue_16, true, 15);
                    _SetValue(customButtonName_17, customButtonValue_17, false);
                    _SetValue(customButtonName_18, customButtonValue_18, false);
                    _SetValue(customButtonName_19, customButtonValue_19, false);
                    _SetValue(customButtonName_20, customButtonValue_20, false);
                    _SetValue(customButtonName_21, customButtonValue_21, false);
                    _SetValue(customButtonName_22, customButtonValue_22, false);
                    _SetValue(customButtonName_23, customButtonValue_23, false);
                    _SetValue(customButtonName_24, customButtonValue_24, false);
                    _SetValue(customButtonName_25, customButtonValue_25, false);
                    _SetValue(customButtonName_26, customButtonValue_26, false);
                    _SetValue(customButtonName_27, customButtonValue_27, false);
                    _SetValue(customButtonName_28, customButtonValue_28, false);
                    _SetValue(customButtonName_29, customButtonValue_29, false);
                    _SetValue(customButtonName_30, customButtonValue_30, false);

                    //

                    if (bShowSettingsButton)//显示设置按钮
                    {
                        customButtonSubtract.Location = new System.Drawing.Point(customButtonSubtract.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【-】
                        customButtonSet.Location = new System.Drawing.Point(customButtonSet.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【Set】
                        customButtonPlus.Location = new System.Drawing.Point(customButtonPlus.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【+】

                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonSet.Bottom + iSetBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }
                    else//不显示设置按钮
                    {
                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonName_10.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonName_10.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonName_10.Bottom + iNameBottom_SetTop_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }

                    customButtonSubtract.Visible = bShowSettingsButton;//【-】
                    customButtonSet.Visible = bShowSettingsButton;//【Set】
                    customButtonPlus.Visible = bShowSettingsButton;//【+】

                    //
                    break;
                case 17:
                    //
                    _SetValue(customButtonName_1, customButtonValue_1, true, 0);
                    _SetValue(customButtonName_2, customButtonValue_2, true, 1);
                    _SetValue(customButtonName_3, customButtonValue_3, true, 2);
                    _SetValue(customButtonName_4, customButtonValue_4, true, 3);
                    _SetValue(customButtonName_5, customButtonValue_5, true, 4);
                    _SetValue(customButtonName_6, customButtonValue_6, true, 5);
                    _SetValue(customButtonName_7, customButtonValue_7, true, 6);
                    _SetValue(customButtonName_8, customButtonValue_8, true, 7);
                    _SetValue(customButtonName_9, customButtonValue_9, true, 8);
                    _SetValue(customButtonName_10, customButtonValue_10, true, 9);
                    _SetValue(customButtonName_11, customButtonValue_11, true, 10);
                    _SetValue(customButtonName_12, customButtonValue_12, true, 11);
                    _SetValue(customButtonName_13, customButtonValue_13, true, 12);
                    _SetValue(customButtonName_14, customButtonValue_14, true, 13);
                    _SetValue(customButtonName_15, customButtonValue_15, true, 14);
                    _SetValue(customButtonName_16, customButtonValue_16, true, 15);
                    _SetValue(customButtonName_17, customButtonValue_17, true, 16);
                    _SetValue(customButtonName_18, customButtonValue_18, false);
                    _SetValue(customButtonName_19, customButtonValue_19, false);
                    _SetValue(customButtonName_20, customButtonValue_20, false);
                    _SetValue(customButtonName_21, customButtonValue_21, false);
                    _SetValue(customButtonName_22, customButtonValue_22, false);
                    _SetValue(customButtonName_23, customButtonValue_23, false);
                    _SetValue(customButtonName_24, customButtonValue_24, false);
                    _SetValue(customButtonName_25, customButtonValue_25, false);
                    _SetValue(customButtonName_26, customButtonValue_26, false);
                    _SetValue(customButtonName_27, customButtonValue_27, false);
                    _SetValue(customButtonName_28, customButtonValue_28, false);
                    _SetValue(customButtonName_29, customButtonValue_29, false);
                    _SetValue(customButtonName_30, customButtonValue_30, false);

                    //

                    if (bShowSettingsButton)//显示设置按钮
                    {
                        customButtonSubtract.Location = new System.Drawing.Point(customButtonSubtract.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【-】
                        customButtonSet.Location = new System.Drawing.Point(customButtonSet.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【Set】
                        customButtonPlus.Location = new System.Drawing.Point(customButtonPlus.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【+】

                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonSet.Bottom + iSetBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }
                    else//不显示设置按钮
                    {
                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonName_10.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonName_10.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonName_10.Bottom + iNameBottom_SetTop_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }

                    customButtonSubtract.Visible = bShowSettingsButton;//【-】
                    customButtonSet.Visible = bShowSettingsButton;//【Set】
                    customButtonPlus.Visible = bShowSettingsButton;//【+】

                    //
                    break;
                case 18:
                    //
                    _SetValue(customButtonName_1, customButtonValue_1, true, 0);
                    _SetValue(customButtonName_2, customButtonValue_2, true, 1);
                    _SetValue(customButtonName_3, customButtonValue_3, true, 2);
                    _SetValue(customButtonName_4, customButtonValue_4, true, 3);
                    _SetValue(customButtonName_5, customButtonValue_5, true, 4);
                    _SetValue(customButtonName_6, customButtonValue_6, true, 5);
                    _SetValue(customButtonName_7, customButtonValue_7, true, 6);
                    _SetValue(customButtonName_8, customButtonValue_8, true, 7);
                    _SetValue(customButtonName_9, customButtonValue_9, true, 8);
                    _SetValue(customButtonName_10, customButtonValue_10, true, 9);
                    _SetValue(customButtonName_11, customButtonValue_11, true, 10);
                    _SetValue(customButtonName_12, customButtonValue_12, true, 11);
                    _SetValue(customButtonName_13, customButtonValue_13, true, 12);
                    _SetValue(customButtonName_14, customButtonValue_14, true, 13);
                    _SetValue(customButtonName_15, customButtonValue_15, true, 14);
                    _SetValue(customButtonName_16, customButtonValue_16, true, 15);
                    _SetValue(customButtonName_17, customButtonValue_17, true, 16);
                    _SetValue(customButtonName_18, customButtonValue_18, true, 17);
                    _SetValue(customButtonName_19, customButtonValue_19, false);
                    _SetValue(customButtonName_20, customButtonValue_20, false);
                    _SetValue(customButtonName_21, customButtonValue_21, false);
                    _SetValue(customButtonName_22, customButtonValue_22, false);
                    _SetValue(customButtonName_23, customButtonValue_23, false);
                    _SetValue(customButtonName_24, customButtonValue_24, false);
                    _SetValue(customButtonName_25, customButtonValue_25, false);
                    _SetValue(customButtonName_26, customButtonValue_26, false);
                    _SetValue(customButtonName_27, customButtonValue_27, false);
                    _SetValue(customButtonName_28, customButtonValue_28, false);
                    _SetValue(customButtonName_29, customButtonValue_29, false);
                    _SetValue(customButtonName_30, customButtonValue_30, false);

                    //

                    if (bShowSettingsButton)//显示设置按钮
                    {
                        customButtonSubtract.Location = new System.Drawing.Point(customButtonSubtract.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【-】
                        customButtonSet.Location = new System.Drawing.Point(customButtonSet.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【Set】
                        customButtonPlus.Location = new System.Drawing.Point(customButtonPlus.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【+】

                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonSet.Bottom + iSetBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }
                    else//不显示设置按钮
                    {
                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonName_10.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonName_10.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonName_10.Bottom + iNameBottom_SetTop_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }

                    customButtonSubtract.Visible = bShowSettingsButton;//【-】
                    customButtonSet.Visible = bShowSettingsButton;//【Set】
                    customButtonPlus.Visible = bShowSettingsButton;//【+】

                    //
                    break;
                case 19:
                    //
                    _SetValue(customButtonName_1, customButtonValue_1, true, 0);
                    _SetValue(customButtonName_2, customButtonValue_2, true, 1);
                    _SetValue(customButtonName_3, customButtonValue_3, true, 2);
                    _SetValue(customButtonName_4, customButtonValue_4, true, 3);
                    _SetValue(customButtonName_5, customButtonValue_5, true, 4);
                    _SetValue(customButtonName_6, customButtonValue_6, true, 5);
                    _SetValue(customButtonName_7, customButtonValue_7, true, 6);
                    _SetValue(customButtonName_8, customButtonValue_8, true, 7);
                    _SetValue(customButtonName_9, customButtonValue_9, true, 8);
                    _SetValue(customButtonName_10, customButtonValue_10, true, 9);
                    _SetValue(customButtonName_11, customButtonValue_11, true, 10);
                    _SetValue(customButtonName_12, customButtonValue_12, true, 11);
                    _SetValue(customButtonName_13, customButtonValue_13, true, 12);
                    _SetValue(customButtonName_14, customButtonValue_14, true, 13);
                    _SetValue(customButtonName_15, customButtonValue_15, true, 14);
                    _SetValue(customButtonName_16, customButtonValue_16, true, 15);
                    _SetValue(customButtonName_17, customButtonValue_17, true, 16);
                    _SetValue(customButtonName_18, customButtonValue_18, true, 17);
                    _SetValue(customButtonName_19, customButtonValue_19, true, 18);
                    _SetValue(customButtonName_20, customButtonValue_20, false);
                    _SetValue(customButtonName_21, customButtonValue_21, false);
                    _SetValue(customButtonName_22, customButtonValue_22, false);
                    _SetValue(customButtonName_23, customButtonValue_23, false);
                    _SetValue(customButtonName_24, customButtonValue_24, false);
                    _SetValue(customButtonName_25, customButtonValue_25, false);
                    _SetValue(customButtonName_26, customButtonValue_26, false);
                    _SetValue(customButtonName_27, customButtonValue_27, false);
                    _SetValue(customButtonName_28, customButtonValue_28, false);
                    _SetValue(customButtonName_29, customButtonValue_29, false);
                    _SetValue(customButtonName_30, customButtonValue_30, false);

                    //

                    if (bShowSettingsButton)//显示设置按钮
                    {
                        customButtonSubtract.Location = new System.Drawing.Point(customButtonSubtract.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【-】
                        customButtonSet.Location = new System.Drawing.Point(customButtonSet.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【Set】
                        customButtonPlus.Location = new System.Drawing.Point(customButtonPlus.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【+】

                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonSet.Bottom + iSetBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }
                    else//不显示设置按钮
                    {
                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonName_10.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonName_10.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonName_10.Bottom + iNameBottom_SetTop_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }

                    customButtonSubtract.Visible = bShowSettingsButton;//【-】
                    customButtonSet.Visible = bShowSettingsButton;//【Set】
                    customButtonPlus.Visible = bShowSettingsButton;//【+】

                    //
                    break;
                case 20:
                    //
                    _SetValue(customButtonName_1, customButtonValue_1, true, 0);
                    _SetValue(customButtonName_2, customButtonValue_2, true, 1);
                    _SetValue(customButtonName_3, customButtonValue_3, true, 2);
                    _SetValue(customButtonName_4, customButtonValue_4, true, 3);
                    _SetValue(customButtonName_5, customButtonValue_5, true, 4);
                    _SetValue(customButtonName_6, customButtonValue_6, true, 5);
                    _SetValue(customButtonName_7, customButtonValue_7, true, 6);
                    _SetValue(customButtonName_8, customButtonValue_8, true, 7);
                    _SetValue(customButtonName_9, customButtonValue_9, true, 8);
                    _SetValue(customButtonName_10, customButtonValue_10, true, 9);
                    _SetValue(customButtonName_11, customButtonValue_11, true, 10);
                    _SetValue(customButtonName_12, customButtonValue_12, true, 11);
                    _SetValue(customButtonName_13, customButtonValue_13, true, 12);
                    _SetValue(customButtonName_14, customButtonValue_14, true, 13);
                    _SetValue(customButtonName_15, customButtonValue_15, true, 14);
                    _SetValue(customButtonName_16, customButtonValue_16, true, 15);
                    _SetValue(customButtonName_17, customButtonValue_17, true, 16);
                    _SetValue(customButtonName_18, customButtonValue_18, true, 17);
                    _SetValue(customButtonName_19, customButtonValue_19, true, 18);
                    _SetValue(customButtonName_20, customButtonValue_20, true, 19);
                    _SetValue(customButtonName_21, customButtonValue_21, false);
                    _SetValue(customButtonName_22, customButtonValue_22, false);
                    _SetValue(customButtonName_23, customButtonValue_23, false);
                    _SetValue(customButtonName_24, customButtonValue_24, false);
                    _SetValue(customButtonName_25, customButtonValue_25, false);
                    _SetValue(customButtonName_26, customButtonValue_26, false);
                    _SetValue(customButtonName_27, customButtonValue_27, false);
                    _SetValue(customButtonName_28, customButtonValue_28, false);
                    _SetValue(customButtonName_29, customButtonValue_29, false);
                    _SetValue(customButtonName_30, customButtonValue_30, false);

                    //

                    if (bShowSettingsButton)//显示设置按钮
                    {
                        customButtonSubtract.Location = new System.Drawing.Point(customButtonSubtract.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【-】
                        customButtonSet.Location = new System.Drawing.Point(customButtonSet.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【Set】
                        customButtonPlus.Location = new System.Drawing.Point(customButtonPlus.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【+】

                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonSet.Bottom + iSetBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }
                    else//不显示设置按钮
                    {
                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonName_10.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonName_10.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonName_10.Bottom + iNameBottom_SetTop_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }

                    customButtonSubtract.Visible = bShowSettingsButton;//【-】
                    customButtonSet.Visible = bShowSettingsButton;//【Set】
                    customButtonPlus.Visible = bShowSettingsButton;//【+】

                    //
                    break;

                case 21:
                    //
                    _SetValue(customButtonName_1, customButtonValue_1, true, 0);
                    _SetValue(customButtonName_2, customButtonValue_2, true, 1);
                    _SetValue(customButtonName_3, customButtonValue_3, true, 2);
                    _SetValue(customButtonName_4, customButtonValue_4, true, 3);
                    _SetValue(customButtonName_5, customButtonValue_5, true, 4);
                    _SetValue(customButtonName_6, customButtonValue_6, true, 5);
                    _SetValue(customButtonName_7, customButtonValue_7, true, 6);
                    _SetValue(customButtonName_8, customButtonValue_8, true, 7);
                    _SetValue(customButtonName_9, customButtonValue_9, true, 8);
                    _SetValue(customButtonName_10, customButtonValue_10, true, 9);
                    _SetValue(customButtonName_11, customButtonValue_11, true, 10);
                    _SetValue(customButtonName_12, customButtonValue_12, true, 11);
                    _SetValue(customButtonName_13, customButtonValue_13, true, 12);
                    _SetValue(customButtonName_14, customButtonValue_14, true, 13);
                    _SetValue(customButtonName_15, customButtonValue_15, true, 14);
                    _SetValue(customButtonName_16, customButtonValue_16, true, 15);
                    _SetValue(customButtonName_17, customButtonValue_17, true, 16);
                    _SetValue(customButtonName_18, customButtonValue_18, true, 17);
                    _SetValue(customButtonName_19, customButtonValue_19, true, 18);
                    _SetValue(customButtonName_20, customButtonValue_20, true, 19);
                    _SetValue(customButtonName_21, customButtonValue_21, true, 20);
                    _SetValue(customButtonName_22, customButtonValue_22, false);
                    _SetValue(customButtonName_23, customButtonValue_23, false);
                    _SetValue(customButtonName_24, customButtonValue_24, false);
                    _SetValue(customButtonName_25, customButtonValue_25, false);
                    _SetValue(customButtonName_26, customButtonValue_26, false);
                    _SetValue(customButtonName_27, customButtonValue_27, false);
                    _SetValue(customButtonName_28, customButtonValue_28, false);
                    _SetValue(customButtonName_29, customButtonValue_29, false);
                    _SetValue(customButtonName_30, customButtonValue_30, false);

                    //

                    if (bShowSettingsButton)//显示设置按钮
                    {
                        customButtonSubtract.Location = new System.Drawing.Point(customButtonSubtract.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【-】
                        customButtonSet.Location = new System.Drawing.Point(customButtonSet.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【Set】
                        customButtonPlus.Location = new System.Drawing.Point(customButtonPlus.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【+】

                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonSet.Bottom + iSetBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }
                    else//不显示设置按钮
                    {
                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonName_10.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonName_10.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonName_10.Bottom + iNameBottom_SetTop_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }

                    customButtonSubtract.Visible = bShowSettingsButton;//【-】
                    customButtonSet.Visible = bShowSettingsButton;//【Set】
                    customButtonPlus.Visible = bShowSettingsButton;//【+】

                    //
                    break;

                case 22:
                    //
                    _SetValue(customButtonName_1, customButtonValue_1, true, 0);
                    _SetValue(customButtonName_2, customButtonValue_2, true, 1);
                    _SetValue(customButtonName_3, customButtonValue_3, true, 2);
                    _SetValue(customButtonName_4, customButtonValue_4, true, 3);
                    _SetValue(customButtonName_5, customButtonValue_5, true, 4);
                    _SetValue(customButtonName_6, customButtonValue_6, true, 5);
                    _SetValue(customButtonName_7, customButtonValue_7, true, 6);
                    _SetValue(customButtonName_8, customButtonValue_8, true, 7);
                    _SetValue(customButtonName_9, customButtonValue_9, true, 8);
                    _SetValue(customButtonName_10, customButtonValue_10, true, 9);
                    _SetValue(customButtonName_11, customButtonValue_11, true, 10);
                    _SetValue(customButtonName_12, customButtonValue_12, true, 11);
                    _SetValue(customButtonName_13, customButtonValue_13, true, 12);
                    _SetValue(customButtonName_14, customButtonValue_14, true, 13);
                    _SetValue(customButtonName_15, customButtonValue_15, true, 14);
                    _SetValue(customButtonName_16, customButtonValue_16, true, 15);
                    _SetValue(customButtonName_17, customButtonValue_17, true, 16);
                    _SetValue(customButtonName_18, customButtonValue_18, true, 17);
                    _SetValue(customButtonName_19, customButtonValue_19, true, 18);
                    _SetValue(customButtonName_20, customButtonValue_20, true, 19);
                    _SetValue(customButtonName_21, customButtonValue_21, true, 20);
                    _SetValue(customButtonName_22, customButtonValue_22, true, 21);
                    _SetValue(customButtonName_23, customButtonValue_23, false);
                    _SetValue(customButtonName_24, customButtonValue_24, false);
                    _SetValue(customButtonName_25, customButtonValue_25, false);
                    _SetValue(customButtonName_26, customButtonValue_26, false);
                    _SetValue(customButtonName_27, customButtonValue_27, false);
                    _SetValue(customButtonName_28, customButtonValue_28, false);
                    _SetValue(customButtonName_29, customButtonValue_29, false);
                    _SetValue(customButtonName_30, customButtonValue_30, false);

                    //

                    if (bShowSettingsButton)//显示设置按钮
                    {
                        customButtonSubtract.Location = new System.Drawing.Point(customButtonSubtract.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【-】
                        customButtonSet.Location = new System.Drawing.Point(customButtonSet.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【Set】
                        customButtonPlus.Location = new System.Drawing.Point(customButtonPlus.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【+】

                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonSet.Bottom + iSetBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }
                    else//不显示设置按钮
                    {
                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonName_10.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonName_10.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonName_10.Bottom + iNameBottom_SetTop_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }

                    customButtonSubtract.Visible = bShowSettingsButton;//【-】
                    customButtonSet.Visible = bShowSettingsButton;//【Set】
                    customButtonPlus.Visible = bShowSettingsButton;//【+】

                    //
                    break;

                case 23:
                    //
                    _SetValue(customButtonName_1, customButtonValue_1, true, 0);
                    _SetValue(customButtonName_2, customButtonValue_2, true, 1);
                    _SetValue(customButtonName_3, customButtonValue_3, true, 2);
                    _SetValue(customButtonName_4, customButtonValue_4, true, 3);
                    _SetValue(customButtonName_5, customButtonValue_5, true, 4);
                    _SetValue(customButtonName_6, customButtonValue_6, true, 5);
                    _SetValue(customButtonName_7, customButtonValue_7, true, 6);
                    _SetValue(customButtonName_8, customButtonValue_8, true, 7);
                    _SetValue(customButtonName_9, customButtonValue_9, true, 8);
                    _SetValue(customButtonName_10, customButtonValue_10, true, 9);
                    _SetValue(customButtonName_11, customButtonValue_11, true, 10);
                    _SetValue(customButtonName_12, customButtonValue_12, true, 11);
                    _SetValue(customButtonName_13, customButtonValue_13, true, 12);
                    _SetValue(customButtonName_14, customButtonValue_14, true, 13);
                    _SetValue(customButtonName_15, customButtonValue_15, true, 14);
                    _SetValue(customButtonName_16, customButtonValue_16, true, 15);
                    _SetValue(customButtonName_17, customButtonValue_17, true, 16);
                    _SetValue(customButtonName_18, customButtonValue_18, true, 17);
                    _SetValue(customButtonName_19, customButtonValue_19, true, 18);
                    _SetValue(customButtonName_20, customButtonValue_20, true, 19);
                    _SetValue(customButtonName_21, customButtonValue_21, true, 20);
                    _SetValue(customButtonName_22, customButtonValue_22, true, 21);
                    _SetValue(customButtonName_23, customButtonValue_23, true, 22);
                    _SetValue(customButtonName_24, customButtonValue_24, false);
                    _SetValue(customButtonName_25, customButtonValue_25, false);
                    _SetValue(customButtonName_26, customButtonValue_26, false);
                    _SetValue(customButtonName_27, customButtonValue_27, false);
                    _SetValue(customButtonName_28, customButtonValue_28, false);
                    _SetValue(customButtonName_29, customButtonValue_29, false);
                    _SetValue(customButtonName_30, customButtonValue_30, false);

                    //

                    if (bShowSettingsButton)//显示设置按钮
                    {
                        customButtonSubtract.Location = new System.Drawing.Point(customButtonSubtract.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【-】
                        customButtonSet.Location = new System.Drawing.Point(customButtonSet.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【Set】
                        customButtonPlus.Location = new System.Drawing.Point(customButtonPlus.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【+】

                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonSet.Bottom + iSetBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }
                    else//不显示设置按钮
                    {
                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonName_10.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonName_10.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonName_10.Bottom + iNameBottom_SetTop_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }

                    customButtonSubtract.Visible = bShowSettingsButton;//【-】
                    customButtonSet.Visible = bShowSettingsButton;//【Set】
                    customButtonPlus.Visible = bShowSettingsButton;//【+】

                    //
                    break;
                case 24:
                    //
                    _SetValue(customButtonName_1, customButtonValue_1, true, 0);
                    _SetValue(customButtonName_2, customButtonValue_2, true, 1);
                    _SetValue(customButtonName_3, customButtonValue_3, true, 2);
                    _SetValue(customButtonName_4, customButtonValue_4, true, 3);
                    _SetValue(customButtonName_5, customButtonValue_5, true, 4);
                    _SetValue(customButtonName_6, customButtonValue_6, true, 5);
                    _SetValue(customButtonName_7, customButtonValue_7, true, 6);
                    _SetValue(customButtonName_8, customButtonValue_8, true, 7);
                    _SetValue(customButtonName_9, customButtonValue_9, true, 8);
                    _SetValue(customButtonName_10, customButtonValue_10, true, 9);
                    _SetValue(customButtonName_11, customButtonValue_11, true, 10);
                    _SetValue(customButtonName_12, customButtonValue_12, true, 11);
                    _SetValue(customButtonName_13, customButtonValue_13, true, 12);
                    _SetValue(customButtonName_14, customButtonValue_14, true, 13);
                    _SetValue(customButtonName_15, customButtonValue_15, true, 14);
                    _SetValue(customButtonName_16, customButtonValue_16, true, 15);
                    _SetValue(customButtonName_17, customButtonValue_17, true, 16);
                    _SetValue(customButtonName_18, customButtonValue_18, true, 17);
                    _SetValue(customButtonName_19, customButtonValue_19, true, 18);
                    _SetValue(customButtonName_20, customButtonValue_20, true, 19);
                    _SetValue(customButtonName_21, customButtonValue_21, true, 20);
                    _SetValue(customButtonName_22, customButtonValue_22, true, 21);
                    _SetValue(customButtonName_23, customButtonValue_23, true, 22);
                    _SetValue(customButtonName_24, customButtonValue_24, true, 23);
                    _SetValue(customButtonName_25, customButtonValue_25, false);
                    _SetValue(customButtonName_26, customButtonValue_26, false);
                    _SetValue(customButtonName_27, customButtonValue_27, false);
                    _SetValue(customButtonName_28, customButtonValue_28, false);
                    _SetValue(customButtonName_29, customButtonValue_29, false);
                    _SetValue(customButtonName_30, customButtonValue_30, false);

                    //

                    if (bShowSettingsButton)//显示设置按钮
                    {
                        customButtonSubtract.Location = new System.Drawing.Point(customButtonSubtract.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【-】
                        customButtonSet.Location = new System.Drawing.Point(customButtonSet.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【Set】
                        customButtonPlus.Location = new System.Drawing.Point(customButtonPlus.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【+】

                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonSet.Bottom + iSetBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }
                    else//不显示设置按钮
                    {
                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonName_10.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonName_10.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonName_10.Bottom + iNameBottom_SetTop_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }

                    customButtonSubtract.Visible = bShowSettingsButton;//【-】
                    customButtonSet.Visible = bShowSettingsButton;//【Set】
                    customButtonPlus.Visible = bShowSettingsButton;//【+】

                    //
                    break;
                case 25:
                    //
                    _SetValue(customButtonName_1, customButtonValue_1, true, 0);
                    _SetValue(customButtonName_2, customButtonValue_2, true, 1);
                    _SetValue(customButtonName_3, customButtonValue_3, true, 2);
                    _SetValue(customButtonName_4, customButtonValue_4, true, 3);
                    _SetValue(customButtonName_5, customButtonValue_5, true, 4);
                    _SetValue(customButtonName_6, customButtonValue_6, true, 5);
                    _SetValue(customButtonName_7, customButtonValue_7, true, 6);
                    _SetValue(customButtonName_8, customButtonValue_8, true, 7);
                    _SetValue(customButtonName_9, customButtonValue_9, true, 8);
                    _SetValue(customButtonName_10, customButtonValue_10, true, 9);
                    _SetValue(customButtonName_11, customButtonValue_11, true, 10);
                    _SetValue(customButtonName_12, customButtonValue_12, true, 11);
                    _SetValue(customButtonName_13, customButtonValue_13, true, 12);
                    _SetValue(customButtonName_14, customButtonValue_14, true, 13);
                    _SetValue(customButtonName_15, customButtonValue_15, true, 14);
                    _SetValue(customButtonName_16, customButtonValue_16, true, 15);
                    _SetValue(customButtonName_17, customButtonValue_17, true, 16);
                    _SetValue(customButtonName_18, customButtonValue_18, true, 17);
                    _SetValue(customButtonName_19, customButtonValue_19, true, 18);
                    _SetValue(customButtonName_20, customButtonValue_20, true, 19);
                    _SetValue(customButtonName_21, customButtonValue_21, true, 20);
                    _SetValue(customButtonName_22, customButtonValue_22, true, 21);
                    _SetValue(customButtonName_23, customButtonValue_23, true, 22);
                    _SetValue(customButtonName_24, customButtonValue_24, true, 23);
                    _SetValue(customButtonName_25, customButtonValue_25, true, 24);
                    _SetValue(customButtonName_26, customButtonValue_26, false);
                    _SetValue(customButtonName_27, customButtonValue_27, false);
                    _SetValue(customButtonName_28, customButtonValue_28, false);
                    _SetValue(customButtonName_29, customButtonValue_29, false);
                    _SetValue(customButtonName_30, customButtonValue_30, false);

                    //

                    if (bShowSettingsButton)//显示设置按钮
                    {
                        customButtonSubtract.Location = new System.Drawing.Point(customButtonSubtract.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【-】
                        customButtonSet.Location = new System.Drawing.Point(customButtonSet.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【Set】
                        customButtonPlus.Location = new System.Drawing.Point(customButtonPlus.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【+】

                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonSet.Bottom + iSetBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }
                    else//不显示设置按钮
                    {
                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonName_10.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonName_10.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonName_10.Bottom + iNameBottom_SetTop_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }

                    customButtonSubtract.Visible = bShowSettingsButton;//【-】
                    customButtonSet.Visible = bShowSettingsButton;//【Set】
                    customButtonPlus.Visible = bShowSettingsButton;//【+】

                    //
                    break;
                case 26:
                    //
                    _SetValue(customButtonName_1, customButtonValue_1, true, 0);
                    _SetValue(customButtonName_2, customButtonValue_2, true, 1);
                    _SetValue(customButtonName_3, customButtonValue_3, true, 2);
                    _SetValue(customButtonName_4, customButtonValue_4, true, 3);
                    _SetValue(customButtonName_5, customButtonValue_5, true, 4);
                    _SetValue(customButtonName_6, customButtonValue_6, true, 5);
                    _SetValue(customButtonName_7, customButtonValue_7, true, 6);
                    _SetValue(customButtonName_8, customButtonValue_8, true, 7);
                    _SetValue(customButtonName_9, customButtonValue_9, true, 8);
                    _SetValue(customButtonName_10, customButtonValue_10, true, 9);
                    _SetValue(customButtonName_11, customButtonValue_11, true, 10);
                    _SetValue(customButtonName_12, customButtonValue_12, true, 11);
                    _SetValue(customButtonName_13, customButtonValue_13, true, 12);
                    _SetValue(customButtonName_14, customButtonValue_14, true, 13);
                    _SetValue(customButtonName_15, customButtonValue_15, true, 14);
                    _SetValue(customButtonName_16, customButtonValue_16, true, 15);
                    _SetValue(customButtonName_17, customButtonValue_17, true, 16);
                    _SetValue(customButtonName_18, customButtonValue_18, true, 17);
                    _SetValue(customButtonName_19, customButtonValue_19, true, 18);
                    _SetValue(customButtonName_20, customButtonValue_20, true, 19);
                    _SetValue(customButtonName_21, customButtonValue_21, true, 20);
                    _SetValue(customButtonName_22, customButtonValue_22, true, 21);
                    _SetValue(customButtonName_23, customButtonValue_23, true, 22);
                    _SetValue(customButtonName_24, customButtonValue_24, true, 23);
                    _SetValue(customButtonName_25, customButtonValue_25, true, 24);
                    _SetValue(customButtonName_26, customButtonValue_26, true, 25);
                    _SetValue(customButtonName_27, customButtonValue_27, false);
                    _SetValue(customButtonName_28, customButtonValue_28, false);
                    _SetValue(customButtonName_29, customButtonValue_29, false);
                    _SetValue(customButtonName_30, customButtonValue_30, false);

                    //

                    if (bShowSettingsButton)//显示设置按钮
                    {
                        customButtonSubtract.Location = new System.Drawing.Point(customButtonSubtract.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【-】
                        customButtonSet.Location = new System.Drawing.Point(customButtonSet.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【Set】
                        customButtonPlus.Location = new System.Drawing.Point(customButtonPlus.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【+】

                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonSet.Bottom + iSetBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }
                    else//不显示设置按钮
                    {
                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonName_10.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonName_10.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonName_10.Bottom + iNameBottom_SetTop_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }

                    customButtonSubtract.Visible = bShowSettingsButton;//【-】
                    customButtonSet.Visible = bShowSettingsButton;//【Set】
                    customButtonPlus.Visible = bShowSettingsButton;//【+】

                    //
                    break;
                case 27:
                    //
                     _SetValue(customButtonName_1, customButtonValue_1, true, 0);
                    _SetValue(customButtonName_2, customButtonValue_2, true, 1);
                    _SetValue(customButtonName_3, customButtonValue_3, true, 2);
                    _SetValue(customButtonName_4, customButtonValue_4, true, 3);
                    _SetValue(customButtonName_5, customButtonValue_5, true, 4);
                    _SetValue(customButtonName_6, customButtonValue_6, true, 5);
                    _SetValue(customButtonName_7, customButtonValue_7, true, 6);
                    _SetValue(customButtonName_8, customButtonValue_8, true, 7);
                    _SetValue(customButtonName_9, customButtonValue_9, true, 8);
                    _SetValue(customButtonName_10, customButtonValue_10, true, 9);
                    _SetValue(customButtonName_11, customButtonValue_11, true, 10);
                    _SetValue(customButtonName_12, customButtonValue_12, true, 11);
                    _SetValue(customButtonName_13, customButtonValue_13, true, 12);
                    _SetValue(customButtonName_14, customButtonValue_14, true, 13);
                    _SetValue(customButtonName_15, customButtonValue_15, true, 14);
                    _SetValue(customButtonName_16, customButtonValue_16, true, 15);
                    _SetValue(customButtonName_17, customButtonValue_17, true, 16);
                    _SetValue(customButtonName_18, customButtonValue_18, true, 17);
                    _SetValue(customButtonName_19, customButtonValue_19, true, 18);
                    _SetValue(customButtonName_20, customButtonValue_20, true, 19);
                    _SetValue(customButtonName_21, customButtonValue_21, true, 20);
                    _SetValue(customButtonName_22, customButtonValue_22, true, 21);
                    _SetValue(customButtonName_23, customButtonValue_23, true, 22);
                    _SetValue(customButtonName_24, customButtonValue_24, true, 23);
                    _SetValue(customButtonName_25, customButtonValue_25, true, 24);
                    _SetValue(customButtonName_26, customButtonValue_26, true, 25);
                    _SetValue(customButtonName_27, customButtonValue_27, true, 26);
                    _SetValue(customButtonName_28, customButtonValue_28, false);
                    _SetValue(customButtonName_29, customButtonValue_29, false);
                    _SetValue(customButtonName_30, customButtonValue_30, false);

                    //

                    if (bShowSettingsButton)//显示设置按钮
                    {
                        customButtonSubtract.Location = new System.Drawing.Point(customButtonSubtract.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【-】
                        customButtonSet.Location = new System.Drawing.Point(customButtonSet.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【Set】
                        customButtonPlus.Location = new System.Drawing.Point(customButtonPlus.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【+】

                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonSet.Bottom + iSetBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }
                    else//不显示设置按钮
                    {
                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonName_10.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonName_10.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonName_10.Bottom + iNameBottom_SetTop_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }

                    customButtonSubtract.Visible = bShowSettingsButton;//【-】
                    customButtonSet.Visible = bShowSettingsButton;//【Set】
                    customButtonPlus.Visible = bShowSettingsButton;//【+】

                    //
                    break;
                case 28:
                    //
                    _SetValue(customButtonName_1, customButtonValue_1, true, 0);
                    _SetValue(customButtonName_2, customButtonValue_2, true, 1);
                    _SetValue(customButtonName_3, customButtonValue_3, true, 2);
                    _SetValue(customButtonName_4, customButtonValue_4, true, 3);
                    _SetValue(customButtonName_5, customButtonValue_5, true, 4);
                    _SetValue(customButtonName_6, customButtonValue_6, true, 5);
                    _SetValue(customButtonName_7, customButtonValue_7, true, 6);
                    _SetValue(customButtonName_8, customButtonValue_8, true, 7);
                    _SetValue(customButtonName_9, customButtonValue_9, true, 8);
                    _SetValue(customButtonName_10, customButtonValue_10, true, 9);
                    _SetValue(customButtonName_11, customButtonValue_11, true, 10);
                    _SetValue(customButtonName_12, customButtonValue_12, true, 11);
                    _SetValue(customButtonName_13, customButtonValue_13, true, 12);
                    _SetValue(customButtonName_14, customButtonValue_14, true, 13);
                    _SetValue(customButtonName_15, customButtonValue_15, true, 14);
                    _SetValue(customButtonName_16, customButtonValue_16, true, 15);
                    _SetValue(customButtonName_17, customButtonValue_17, true, 16);
                    _SetValue(customButtonName_18, customButtonValue_18, true, 17);
                    _SetValue(customButtonName_19, customButtonValue_19, true, 18);
                    _SetValue(customButtonName_20, customButtonValue_20, true, 19);
                    _SetValue(customButtonName_21, customButtonValue_21, true, 20);
                    _SetValue(customButtonName_22, customButtonValue_22, true, 21);
                    _SetValue(customButtonName_23, customButtonValue_23, true, 22);
                    _SetValue(customButtonName_24, customButtonValue_24, true, 23);
                    _SetValue(customButtonName_25, customButtonValue_25, true, 24);
                    _SetValue(customButtonName_26, customButtonValue_26, true, 25);
                    _SetValue(customButtonName_27, customButtonValue_27, true, 26);
                    _SetValue(customButtonName_28, customButtonValue_28, true, 27);
                    _SetValue(customButtonName_29, customButtonValue_29, false);
                    _SetValue(customButtonName_30, customButtonValue_30, false);

                    //

                    if (bShowSettingsButton)//显示设置按钮
                    {
                        customButtonSubtract.Location = new System.Drawing.Point(customButtonSubtract.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【-】
                        customButtonSet.Location = new System.Drawing.Point(customButtonSet.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【Set】
                        customButtonPlus.Location = new System.Drawing.Point(customButtonPlus.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【+】

                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonSet.Bottom + iSetBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }
                    else//不显示设置按钮
                    {
                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonName_10.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonName_10.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonName_10.Bottom + iNameBottom_SetTop_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }

                    customButtonSubtract.Visible = bShowSettingsButton;//【-】
                    customButtonSet.Visible = bShowSettingsButton;//【Set】
                    customButtonPlus.Visible = bShowSettingsButton;//【+】

                    //
                    break;
                case 29:
                    //
                    _SetValue(customButtonName_1, customButtonValue_1, true, 0);
                    _SetValue(customButtonName_2, customButtonValue_2, true, 1);
                    _SetValue(customButtonName_3, customButtonValue_3, true, 2);
                    _SetValue(customButtonName_4, customButtonValue_4, true, 3);
                    _SetValue(customButtonName_5, customButtonValue_5, true, 4);
                    _SetValue(customButtonName_6, customButtonValue_6, true, 5);
                    _SetValue(customButtonName_7, customButtonValue_7, true, 6);
                    _SetValue(customButtonName_8, customButtonValue_8, true, 7);
                    _SetValue(customButtonName_9, customButtonValue_9, true, 8);
                    _SetValue(customButtonName_10, customButtonValue_10, true, 9);
                    _SetValue(customButtonName_11, customButtonValue_11, true, 10);
                    _SetValue(customButtonName_12, customButtonValue_12, true, 11);
                    _SetValue(customButtonName_13, customButtonValue_13, true, 12);
                    _SetValue(customButtonName_14, customButtonValue_14, true, 13);
                    _SetValue(customButtonName_15, customButtonValue_15, true, 14);
                    _SetValue(customButtonName_16, customButtonValue_16, true, 15);
                    _SetValue(customButtonName_17, customButtonValue_17, true, 16);
                    _SetValue(customButtonName_18, customButtonValue_18, true, 17);
                    _SetValue(customButtonName_19, customButtonValue_19, true, 18);
                    _SetValue(customButtonName_20, customButtonValue_20, true, 19);
                    _SetValue(customButtonName_21, customButtonValue_21, true, 20);
                    _SetValue(customButtonName_22, customButtonValue_22, true, 21);
                    _SetValue(customButtonName_23, customButtonValue_23, true, 22);
                    _SetValue(customButtonName_24, customButtonValue_24, true, 23);
                    _SetValue(customButtonName_25, customButtonValue_25, true, 24);
                    _SetValue(customButtonName_26, customButtonValue_26, true, 25);
                    _SetValue(customButtonName_27, customButtonValue_27, true, 26);
                    _SetValue(customButtonName_28, customButtonValue_28, true, 27);
                    _SetValue(customButtonName_29, customButtonValue_29, true, 28);
                    _SetValue(customButtonName_30, customButtonValue_30, false);

                    //

                    if (bShowSettingsButton)//显示设置按钮
                    {
                        customButtonSubtract.Location = new System.Drawing.Point(customButtonSubtract.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【-】
                        customButtonSet.Location = new System.Drawing.Point(customButtonSet.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【Set】
                        customButtonPlus.Location = new System.Drawing.Point(customButtonPlus.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【+】

                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonSet.Bottom + iSetBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }
                    else//不显示设置按钮
                    {
                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonName_10.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonName_10.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonName_10.Bottom + iNameBottom_SetTop_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }

                    customButtonSubtract.Visible = bShowSettingsButton;//【-】
                    customButtonSet.Visible = bShowSettingsButton;//【Set】
                    customButtonPlus.Visible = bShowSettingsButton;//【+】

                    //
                    break;
                case 30:
                    //
                    _SetValue(customButtonName_1, customButtonValue_1, true, 0);
                    _SetValue(customButtonName_2, customButtonValue_2, true, 1);
                    _SetValue(customButtonName_3, customButtonValue_3, true, 2);
                    _SetValue(customButtonName_4, customButtonValue_4, true, 3);
                    _SetValue(customButtonName_5, customButtonValue_5, true, 4);
                    _SetValue(customButtonName_6, customButtonValue_6, true, 5);
                    _SetValue(customButtonName_7, customButtonValue_7, true, 6);
                    _SetValue(customButtonName_8, customButtonValue_8, true, 7);
                    _SetValue(customButtonName_9, customButtonValue_9, true, 8);
                    _SetValue(customButtonName_10, customButtonValue_10, true, 9);
                    _SetValue(customButtonName_11, customButtonValue_11, true, 10);
                    _SetValue(customButtonName_12, customButtonValue_12, true, 11);
                    _SetValue(customButtonName_13, customButtonValue_13, true, 12);
                    _SetValue(customButtonName_14, customButtonValue_14, true, 13);
                    _SetValue(customButtonName_15, customButtonValue_15, true, 14);
                    _SetValue(customButtonName_16, customButtonValue_16, true, 15);
                    _SetValue(customButtonName_17, customButtonValue_17, true, 16);
                    _SetValue(customButtonName_18, customButtonValue_18, true, 17);
                    _SetValue(customButtonName_19, customButtonValue_19, true, 18);
                    _SetValue(customButtonName_20, customButtonValue_20, true, 19);
                    _SetValue(customButtonName_21, customButtonValue_21, true, 20);
                    _SetValue(customButtonName_22, customButtonValue_22, true, 21);
                    _SetValue(customButtonName_23, customButtonValue_23, true, 22);
                    _SetValue(customButtonName_24, customButtonValue_24, true, 23);
                    _SetValue(customButtonName_25, customButtonValue_25, true, 24);
                    _SetValue(customButtonName_26, customButtonValue_26, true, 25);
                    _SetValue(customButtonName_27, customButtonValue_27, true, 26);
                    _SetValue(customButtonName_28, customButtonValue_28, true, 27);
                    _SetValue(customButtonName_29, customButtonValue_29, true, 28);
                    _SetValue(customButtonName_30, customButtonValue_30, true, 29);

                    //

                    if (bShowSettingsButton)//显示设置按钮
                    {
                        customButtonSubtract.Location = new System.Drawing.Point(customButtonSubtract.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【-】
                        customButtonSet.Location = new System.Drawing.Point(customButtonSet.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【Set】
                        customButtonPlus.Location = new System.Drawing.Point(customButtonPlus.Left, customButtonName_10.Bottom + iNameBottom_SetTop_Height);//【+】

                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonSet.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonSet.Bottom + iSetBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }
                    else//不显示设置按钮
                    {
                        if (iTotalPage > 1) //当前大于一页
                        {
                            customButtonPreviousPage.Location = new System.Drawing.Point(customButtonPreviousPage.Left, customButtonName_10.Bottom + iSetBottom_NextPageTop_Height);
                            customButtonNextPage.Location = new System.Drawing.Point(customButtonNextPage.Left, customButtonName_10.Bottom + iSetBottom_NextPageTop_Height);

                            this.Size = new Size(this.Size.Width, customButtonNextPage.Bottom + iNextPageBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else
                        {
                            this.Size = new Size(this.Size.Width, customButtonName_10.Bottom + iNameBottom_SetTop_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                    }

                    customButtonSubtract.Visible = bShowSettingsButton;//【-】
                    customButtonSet.Visible = bShowSettingsButton;//【Set】
                    customButtonPlus.Visible = bShowSettingsButton;//【+】

                    //
                    break;


                default:
                    break;
            }
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：点击数值控件，执行相关操作
        // 输入参数： 1.iIndex：数值控件序号（从0开始）
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _ClickValueControl(Int32 iIndex)
        {
            switch (iCurrentSelectedValueIndex)//点击之前选择的参数控件索引
            {
                case 0:
                    //
                    customButtonValue_1.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                    //
                    break;
                case 1:
                    //
                    customButtonValue_2.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                    //
                    break;
                case 2:
                    //
                    customButtonValue_3.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                    //
                    break;
                case 3:
                    //
                    customButtonValue_4.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                    //
                    break;
                case 4:
                    //
                    customButtonValue_5.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                    //
                    break;
                case 5:
                    //
                    customButtonValue_6.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                    //
                    break;
                case 6:
                    //
                    customButtonValue_7.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                    //
                    break;
                case 7:
                    //
                    customButtonValue_8.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                    //
                    break;
                case 8:
                    //
                    customButtonValue_9.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                    //
                    break;
                case 9:
                    //
                    customButtonValue_10.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                    //
                    break;
                case 10:
                    //
                    customButtonValue_11.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                    //
                    break;
                case 11:
                    //
                    customButtonValue_12.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                    //
                    break;
                case 12:
                    //
                    customButtonValue_13.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                    //
                    break;
                case 13:
                    //
                    customButtonValue_14.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                    //
                    break;
                case 14:
                    //
                    customButtonValue_15.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                    //
                    break;
                case 15:
                    //
                    customButtonValue_16.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                    //
                    break;
                case 16:
                    //
                    customButtonValue_17.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                    //
                    break;
                case 17:
                    //
                    customButtonValue_18.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                    //
                    break;
                case 18:
                    //
                    customButtonValue_19.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                    //
                    break;
                case 19:
                    //
                    customButtonValue_20.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                    //
                    break;
                case 20:
                    //
                    customButtonValue_21.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                    //
                    break;
                case 21:
                    //
                    customButtonValue_22.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                    //
                    break;
                case 22:
                    //
                    customButtonValue_23.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                    //
                    break;
                case 23:
                    //
                    customButtonValue_24.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                    //
                    break;
                case 24:
                    //
                    customButtonValue_25.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                    //
                    break;
                case 25:
                    //
                    customButtonValue_26.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                    //
                    break;
                case 26:
                    //
                    customButtonValue_27.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                    //
                    break;
                case 27:
                    //
                    customButtonValue_28.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                    //
                    break;
                case 28:
                    //
                    customButtonValue_29.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                    //
                    break;
                case 29:
                    //
                    customButtonValue_30.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                    //
                    break;
                default:
                    break;
            }

            //

            iCurrentSelectedValueIndex = iIndex;

            switch (iCurrentSelectedValueIndex)//当前选择的参数控件索引
            {
                case 0:
                    //
                    customButtonCurrentSelectedName = customButtonName_1;
                    customButtonCurrentSelectedValue = customButtonValue_1;

                    customButtonValue_1.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
                    //
                    break;
                case 1:
                    //
                    customButtonCurrentSelectedName = customButtonName_2;
                    customButtonCurrentSelectedValue = customButtonValue_2;

                    customButtonValue_2.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
                    //
                    break;
                case 2:
                    //
                    customButtonCurrentSelectedName = customButtonName_3;
                    customButtonCurrentSelectedValue = customButtonValue_3;

                    customButtonValue_3.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
                    //
                    break;
                case 3:
                    //
                    customButtonCurrentSelectedName = customButtonName_4;
                    customButtonCurrentSelectedValue = customButtonValue_4;

                    customButtonValue_4.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
                    //
                    break;
                case 4:
                    //
                    customButtonCurrentSelectedName = customButtonName_5;
                    customButtonCurrentSelectedValue = customButtonValue_5;

                    customButtonValue_5.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
                    //
                    break;
                case 5:
                    //
                    customButtonCurrentSelectedName = customButtonName_6;
                    customButtonCurrentSelectedValue = customButtonValue_6;

                    customButtonValue_6.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
                    //
                    break;
                case 6:
                    //
                    customButtonCurrentSelectedName = customButtonName_7;
                    customButtonCurrentSelectedValue = customButtonValue_7;

                    customButtonValue_7.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
                    //
                    break;
                case 7:
                    //
                    customButtonCurrentSelectedName = customButtonName_8;
                    customButtonCurrentSelectedValue = customButtonValue_8;

                    customButtonValue_8.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
                    //
                    break;
                case 8:
                    //
                    customButtonCurrentSelectedName = customButtonName_9;
                    customButtonCurrentSelectedValue = customButtonValue_9;

                    customButtonValue_9.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
                    //
                    break;
                case 9:
                    //
                    customButtonCurrentSelectedName = customButtonName_10;
                    customButtonCurrentSelectedValue = customButtonValue_10;

                    customButtonValue_10.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
                    //
                    break;
                case 10:
                    //
                    customButtonCurrentSelectedName = customButtonName_11;
                    customButtonCurrentSelectedValue = customButtonValue_11;

                    customButtonValue_11.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
                    //
                    break;
                case 11:
                    //
                    customButtonCurrentSelectedName = customButtonName_12;
                    customButtonCurrentSelectedValue = customButtonValue_12;

                    customButtonValue_12.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
                    //
                    break;
                case 12:
                    //
                    customButtonCurrentSelectedName = customButtonName_13;
                    customButtonCurrentSelectedValue = customButtonValue_13;

                    customButtonValue_13.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
                    //
                    break;
                case 13:
                    //
                    customButtonCurrentSelectedName = customButtonName_14;
                    customButtonCurrentSelectedValue = customButtonValue_14;

                    customButtonValue_14.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
                    //
                    break;
                case 14:
                    //
                    customButtonCurrentSelectedName = customButtonName_15;
                    customButtonCurrentSelectedValue = customButtonValue_15;

                    customButtonValue_15.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
                    //
                    break;
                case 15:
                    //
                    customButtonCurrentSelectedName = customButtonName_16;
                    customButtonCurrentSelectedValue = customButtonValue_16;

                    customButtonValue_16.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
                    //
                    break;
                case 16:
                    //
                    customButtonCurrentSelectedName = customButtonName_17;
                    customButtonCurrentSelectedValue = customButtonValue_17;

                    customButtonValue_17.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
                    //
                    break;
                case 17:
                    //
                    customButtonCurrentSelectedName = customButtonName_18;
                    customButtonCurrentSelectedValue = customButtonValue_18;

                    customButtonValue_18.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
                    //
                    break;
                case 18:
                    //
                    customButtonCurrentSelectedName = customButtonName_19;
                    customButtonCurrentSelectedValue = customButtonValue_19;

                    customButtonValue_19.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
                    //
                    break;
                case 19:
                    //
                    customButtonCurrentSelectedName = customButtonName_20;
                    customButtonCurrentSelectedValue = customButtonValue_20;

                    customButtonValue_20.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
                    //
                    break;
                case 20:
                    //
                    customButtonCurrentSelectedName = customButtonName_21;
                    customButtonCurrentSelectedValue = customButtonValue_21;

                    customButtonValue_21.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
                    //
                    break;
                case 21:
                    //
                    customButtonCurrentSelectedName = customButtonName_22;
                    customButtonCurrentSelectedValue = customButtonValue_22;

                    customButtonValue_22.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
                    //
                    break;
                case 22:
                    //
                    customButtonCurrentSelectedName = customButtonName_23;
                    customButtonCurrentSelectedValue = customButtonValue_23;

                    customButtonValue_23.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
                    //
                    break;
                case 23:
                    //
                    customButtonCurrentSelectedName = customButtonName_24;
                    customButtonCurrentSelectedValue = customButtonValue_24;

                    customButtonValue_24.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
                    //
                    break;
                case 24:
                    //
                    customButtonCurrentSelectedName = customButtonName_25;
                    customButtonCurrentSelectedValue = customButtonValue_25;

                    customButtonValue_25.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
                    //
                    break;
                case 25:
                    //
                    customButtonCurrentSelectedName = customButtonName_26;
                    customButtonCurrentSelectedValue = customButtonValue_26;

                    customButtonValue_26.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
                    //
                    break;
                case 26:
                    //
                    customButtonCurrentSelectedName = customButtonName_27;
                    customButtonCurrentSelectedValue = customButtonValue_27;

                    customButtonValue_27.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
                    //
                    break;
                case 27:
                    //
                    customButtonCurrentSelectedName = customButtonName_28;
                    customButtonCurrentSelectedValue = customButtonValue_28;

                    customButtonValue_28.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
                    //
                    break;
                case 28:
                    //
                    customButtonCurrentSelectedName = customButtonName_29;
                    customButtonCurrentSelectedValue = customButtonValue_29;

                    customButtonValue_29.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
                    //
                    break;
                case 29:
                    //
                    customButtonCurrentSelectedName = customButtonName_30;
                    customButtonCurrentSelectedValue = customButtonValue_30;

                    customButtonValue_30.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
                    //
                    break;
                default:
                    break;
            }
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：获取枚举参数当前值的索引值
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：索引值
        //----------------------------------------------------------------------
        private Int32 _GetCurrentEnumValueIndex()
        {
            Int32 iReturn = 0;//返回值

            Int32 i = 0;//循环控制变量

            for (i = 0; i < ParameterValue[iCurrentSelectedValueIndex].Length; i++)
            {
                if ((Int32)fParameterCurrentValue[iCurrentSelectedValueIndex] == ParameterValue[iCurrentSelectedValueIndex][i])//相同
                {
                    iReturn = i;

                    break;
                }
            }

            return iReturn;
        }

        //-----------------------------------------------------------------------
        // 功能说明：点击【-】事件，执行相应的操作
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        private void _ClickSubtract()
        {
            if (0 <= iCurrentSelectedValueIndex)//有效
            {
                if (1 == iParameterType[iCurrentSelectedValueIndex])//枚举型
                {
                    Int32 iCurrentIndex = _GetCurrentEnumValueIndex();

                    Int32 i = 0;//循环控制变量

                    if (0 == iCurrentIndex)//首项
                    {
                        for (i = ParameterValue[iCurrentSelectedValueIndex].Length - 1; 0 <= i; i--)
                        {
                            if (ParameterValueEnabled[iCurrentSelectedValueIndex][i])//使能
                            {
                                fParameterCurrentValue[iCurrentSelectedValueIndex] = ParameterValue[iCurrentSelectedValueIndex][i];

                                break;
                            }
                        }
                    }
                    else//其它
                    {
                        for (i = iCurrentIndex - 1; 0 <= i; i--)
                        {
                            if (ParameterValueEnabled[iCurrentSelectedValueIndex][i])//使能
                            {
                                fParameterCurrentValue[iCurrentSelectedValueIndex] = ParameterValue[iCurrentSelectedValueIndex][i];

                                break;
                            }
                        }
                        if (0 > i)//继续
                        {
                            for (i = ParameterValue[iCurrentSelectedValueIndex].Length - 1; i >= iCurrentIndex; i--)
                            {
                                if (ParameterValueEnabled[iCurrentSelectedValueIndex][i])//使能
                                {
                                    fParameterCurrentValue[iCurrentSelectedValueIndex] = ParameterValue[iCurrentSelectedValueIndex][i];

                                    break;
                                }
                            }
                        }
                    }

                    _SetValue(customButtonCurrentSelectedName, customButtonCurrentSelectedValue, true, iCurrentSelectedValueIndex);

                    //事件

                    if (null != ParameterValueChanged)
                    {
                        ParameterValueChanged(this, new CustomEventArgs());
                    }
                }
                else//数字类型
                {
                    if (fParameterCurrentValue[iCurrentSelectedValueIndex] > fParameterMinValue[iCurrentSelectedValueIndex])//有效
                    {
                        fParameterCurrentValue[iCurrentSelectedValueIndex] -= 1;

                        //

                        _SetValue(customButtonCurrentSelectedName, customButtonCurrentSelectedValue, true, iCurrentSelectedValueIndex);

                        //事件

                        if (null != ParameterValueChanged)
                        {
                            ParameterValueChanged(this, new CustomEventArgs());
                        }
                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：点击【+】事件，执行相应的操作
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        private void _ClickPlus()
        {
            if (0 <= iCurrentSelectedValueIndex)//有效
            {
                if (1 == iParameterType[iCurrentSelectedValueIndex])//枚举型
                {
                    Int32 iCurrentIndex = _GetCurrentEnumValueIndex();

                    Int32 i = 0;//循环控制变量

                    if (ParameterValue[iCurrentSelectedValueIndex].Length - 1 == iCurrentIndex)//末项
                    {
                        for (i = 0; i < ParameterValue[iCurrentSelectedValueIndex].Length; i++)
                        {
                            if (ParameterValueEnabled[iCurrentSelectedValueIndex][i])//使能
                            {
                                fParameterCurrentValue[iCurrentSelectedValueIndex] = ParameterValue[iCurrentSelectedValueIndex][i];

                                break;
                            }
                        }
                    }
                    else//其它
                    {
                        for (i = iCurrentIndex + 1; i < ParameterValue[iCurrentSelectedValueIndex].Length; i++)
                        {
                            if (ParameterValueEnabled[iCurrentSelectedValueIndex][i])//使能
                            {
                                fParameterCurrentValue[iCurrentSelectedValueIndex] = ParameterValue[iCurrentSelectedValueIndex][i];

                                break;
                            }
                        }
                        if (i >= ParameterValue[iCurrentSelectedValueIndex].Length)//继续
                        {
                            for (i = 0; i < iCurrentIndex; i++)
                            {
                                if (ParameterValueEnabled[iCurrentSelectedValueIndex][i])//使能
                                {
                                    fParameterCurrentValue[iCurrentSelectedValueIndex] = ParameterValue[iCurrentSelectedValueIndex][i];

                                    break;
                                }
                            }
                        }
                    }

                    _SetValue(customButtonCurrentSelectedName, customButtonCurrentSelectedValue, true, iCurrentSelectedValueIndex);

                    //事件

                    if (null != ParameterValueChanged)
                    {
                        ParameterValueChanged(this, new CustomEventArgs());
                    }
                }
                else//数字类型
                {
                    if (fParameterCurrentValue[iCurrentSelectedValueIndex] < fParameterMaxValue[iCurrentSelectedValueIndex])//有效
                    {
                        fParameterCurrentValue[iCurrentSelectedValueIndex] += 1;

                        //

                        _SetValue(customButtonCurrentSelectedName, customButtonCurrentSelectedValue, true, iCurrentSelectedValueIndex);

                        //事件

                        if (null != ParameterValueChanged)
                        {
                            ParameterValueChanged(this, new CustomEventArgs());
                        }
                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：点击【Set】事件，执行相应的操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _ClickSet()
        {
            if (0 <= iCurrentSelectedValueIndex)//有效
            {
                if (2 == iParameterType[iCurrentSelectedValueIndex])//数字类型
                {
                    GlobalWindows.DigitalKeyboard_Window.WindowParameter = iWindowParameter;//窗口特征数值
                    GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.Language = language;//语言
                    GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.Chinese_Caption = sParameterName[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][iCurrentSelectedValueIndex];//中文标题文本
                    GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.English_Caption = sParameterName[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][iCurrentSelectedValueIndex];//英文标题文本
                    GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.Precision = iParameterPrecision[iCurrentSelectedValueIndex];//输入的数据类型
                    GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.MaxLength = 4;//数值长度范围
                    GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.MinValue = fParameterMinValue[iCurrentSelectedValueIndex];//最小值
                    GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.MaxValue = fParameterMaxValue[iCurrentSelectedValueIndex];//最大值
                    GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.NumericalValue = fParameterCurrentValue[iCurrentSelectedValueIndex];//初始显示的数值

                    GlobalWindows.DigitalKeyboard_Window.StartPosition = FormStartPosition.CenterScreen;
                    if (GlobalWindows.TopMostWindows)//置顶
                    {
                        GlobalWindows.DigitalKeyboard_Window.TopMost = true;//将窗口置于顶层
                    }
                    else//其它
                    {
                        GlobalWindows.DigitalKeyboard_Window.Visible = true;//显示
                    }
                }
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：数字键盘窗口关闭时执行的操作
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _DigitalKeyboardWindowClose()
        {
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.DigitalKeyboard_Window.TopMost = false;//取消置于顶层
            }
            else//其它
            {
                GlobalWindows.DigitalKeyboard_Window.Visible = false;//隐藏
            }

            //

            if (GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.EnterNewValue)//输入完成
            {
                fParameterCurrentValue[iCurrentSelectedValueIndex] = GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.NumericalValue;

                _SetValue(customButtonCurrentSelectedName, customButtonCurrentSelectedValue, true, iCurrentSelectedValueIndex);

                //事件

                if (null != ParameterValueChanged)
                {
                    ParameterValueChanged(this, new CustomEventArgs());
                }
            }
            else//其它
            {
                //不执行操作
            }
        }

        //事件

        //----------------------------------------------------------------------
        // 功能说明：按下数值1控件事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonValue_1_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickValueControl(0);
        }

        //----------------------------------------------------------------------
        // 功能说明：按下数值2控件事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonValue_2_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickValueControl(1);
        }

        //----------------------------------------------------------------------
        // 功能说明：按下数值3控件事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonValue_3_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickValueControl(2);
        }

        //----------------------------------------------------------------------
        // 功能说明：按下数值4控件事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonValue_4_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickValueControl(3);
        }

        //----------------------------------------------------------------------
        // 功能说明：按下数值5控件事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonValue_5_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickValueControl(4);
        }

        //----------------------------------------------------------------------
        // 功能说明：按下数值6控件事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonValue_6_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickValueControl(5);
        }

        //----------------------------------------------------------------------
        // 功能说明：按下数值7控件事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonValue_7_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickValueControl(6);
        }

        //----------------------------------------------------------------------
        // 功能说明：按下数值8控件事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonValue_8_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickValueControl(7);
        }

        //----------------------------------------------------------------------
        // 功能说明：按下数值9控件事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonValue_9_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickValueControl(8);
        }

        //----------------------------------------------------------------------
        // 功能说明：按下数值10控件事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonValue_10_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickValueControl(9);
        }

        //----------------------------------------------------------------------
        // 功能说明：按下数值11控件事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonValue_11_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickValueControl(10);
        }

        //----------------------------------------------------------------------
        // 功能说明：按下数值12控件事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonValue_12_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickValueControl(11);
        }

        //----------------------------------------------------------------------
        // 功能说明：按下数值13控件事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonValue_13_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickValueControl(12);
        }

        //----------------------------------------------------------------------
        // 功能说明：按下数值14控件事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonValue_14_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickValueControl(13);
        }

        //----------------------------------------------------------------------
        // 功能说明：按下数值15控件事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonValue_15_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickValueControl(14);
        }

        //----------------------------------------------------------------------
        // 功能说明：按下数值16控件事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonValue_16_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickValueControl(15);
        }

        //----------------------------------------------------------------------
        // 功能说明：按下数值17控件事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonValue_17_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickValueControl(16);
        }

        //----------------------------------------------------------------------
        // 功能说明：按下数值18控件事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonValue_18_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickValueControl(17);
        }

        //----------------------------------------------------------------------
        // 功能说明：按下数值19控件事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonValue_19_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickValueControl(18);
        }

        //----------------------------------------------------------------------
        // 功能说明：按下数值20控件事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonValue_20_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickValueControl(19);
        }

        //----------------------------------------------------------------------
        // 功能说明：按下数值21控件事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonValue_21_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickValueControl(20);
        }

        //----------------------------------------------------------------------
        // 功能说明：按下数值22控件事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonValue_22_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickValueControl(21);
        }

        //----------------------------------------------------------------------
        // 功能说明：按下数值23控件事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonValue_23_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickValueControl(22);
        }

        //----------------------------------------------------------------------
        // 功能说明：按下数值24控件事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonValue_24_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickValueControl(23);
        }

        //----------------------------------------------------------------------
        // 功能说明：按下数值25控件事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonValue_25_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickValueControl(24);
        }

        //----------------------------------------------------------------------
        // 功能说明：按下数值26控件事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonValue_26_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickValueControl(25);
        }

        //----------------------------------------------------------------------
        // 功能说明：按下数值27控件事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonValue_27_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickValueControl(26);
        }

        //----------------------------------------------------------------------
        // 功能说明：按下数值28控件事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonValue_28_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickValueControl(27);
        }

        //----------------------------------------------------------------------
        // 功能说明：按下数值29控件事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonValue_29_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickValueControl(28);
        }

        //----------------------------------------------------------------------
        // 功能说明：按下数值30控件事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonValue_30_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickValueControl(29);
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：按下【-】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonSubtract_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickSubtract();
        }

        //----------------------------------------------------------------------
        // 功能说明：按下【+】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonPlus_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickPlus();
        }

        //----------------------------------------------------------------------
        // 功能说明：按下【Set】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonSet_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickSet();
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：QUALITY CHECK，工具参数修改，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void digitalKeyboardWindow_WindowClose_QualityCheck_ToolParameter(object sender, EventArgs e)
        {
            _DigitalKeyboardWindowClose();
        }

        //----------------------------------------------------------------------
        // 功能说明：IMAGE CONFIGURATION，工具参数修改，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void digitalKeyboardWindow_WindowClose_ImageConfiguration_ToolParameter(object sender, EventArgs e)
        {
            _DigitalKeyboardWindowClose();
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【Previous Page】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonPreviousPage_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickPage(true);
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【Next Page】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonNextPage_CustomButton_Click(object sender, EventArgs e)
        {
            _ClickPage(false);
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【Previous Page】或【Next Page】按钮时进行相关操作
        // 输入参数：1.bPreviousNext：点击的按钮的类型。取值范围：true：【Previous Page】按钮；【Next Page】按钮
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _ClickPage(bool bPreviousNext)
        {
            //更新页码

            if (bPreviousNext)//点击了【Previous Page】按钮
            {
                if (iCurrentPage > 0)//非首页
                {
                    iCurrentPage--;//更新页码

                    if (!(customButtonNextPage.Visible))//未显示【Next Page】按钮
                    {
                        customButtonNextPage.Visible = true;//显示【Next Page】按钮
                    }

                    if (0 == iCurrentPage)//首页
                    {
                        customButtonPreviousPage.Visible = false;//隐藏【Previous Page】按钮
                    }
                }
            }
            else//点击了【Next Page】按钮
            {
                if (iCurrentPage < iTotalPage - 1)//非末页
                {
                    iCurrentPage++;//更新页码

                    if (!(customButtonPreviousPage.Visible))//未显示【Previous Page】按钮
                    {
                        customButtonPreviousPage.Visible = true;//显示【Previous Page】按钮
                    }

                    if (iTotalPage - 1 == iCurrentPage)//末页
                    {
                        customButtonNextPage.Visible = false;//隐藏【Next Page】按钮
                    }
                }
            }

            //设置页面

            _SetPage();
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【Previous Page】或【Next Page】按钮，应用模式时控件初始化时调用本函数，设置页面曲线图和控件
        // 输入参数：1.bPreviousNext：点击的按钮的类型。取值范围：true：【Previous Page】按钮；【Next Page】按钮
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetPage()
        {
            Int32 iCurrentPageDataNumber = 0;

            if (iCurrentPage == 0) //当前处于第一页
            {
                if (iTotalPage > 1)
                {
                    iCurrentPageDataNumber = iNumberPerPage;
                }
                else
                {
                    iCurrentPageDataNumber = iValidParameterNumber - iNumberPerPage * iCurrentPage;
                }
            }
            else if (iCurrentPage == 1) //当前处于第二页
            {
                if (iTotalPage == 3)
                {
                    iCurrentPageDataNumber = iNumberPerPage;
                }
                else
                {
                    iCurrentPageDataNumber = iValidParameterNumber - iNumberPerPage * iCurrentPage;
                }
            }
            else if (iCurrentPage == 2) //当前处于第三页
            {
                iCurrentPageDataNumber = iValidParameterNumber - iNumberPerPage * iCurrentPage;
            }

            _ShowControls(iCurrentPageDataNumber);
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【Previous Page】或【Next Page】按钮，应用模式时控件初始化时调用本函数，设置页面曲线图和控件
        // 输入参数：1.bPreviousNext：点击的按钮的类型。取值范围：true：【Previous Page】按钮；【Next Page】按钮
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _ShowControls(Int32 pageNumber)
        {
            if (iCurrentPage == 0) //当前处于第一页
            {
                switch (pageNumber)
                {
                    case 1:
                        _ShowControls(true, 0, customButtonName_1, customButtonValue_1, customButtonValueBackground_1);
                        _ShowControls(false, 1, customButtonName_2, customButtonValue_2, customButtonValueBackground_2);
                        _ShowControls(false, 2, customButtonName_3, customButtonValue_3, customButtonValueBackground_3);
                        _ShowControls(false, 3, customButtonName_4, customButtonValue_4, customButtonValueBackground_4);
                        _ShowControls(false, 4, customButtonName_5, customButtonValue_5, customButtonValueBackground_5);
                        _ShowControls(false, 5, customButtonName_6, customButtonValue_6, customButtonValueBackground_6);
                        _ShowControls(false, 6, customButtonName_7, customButtonValue_7, customButtonValueBackground_7);
                        _ShowControls(false, 7, customButtonName_8, customButtonValue_8, customButtonValueBackground_8);
                        _ShowControls(false, 8, customButtonName_9, customButtonValue_9, customButtonValueBackground_9);
                        _ShowControls(false, 9, customButtonName_10, customButtonValue_10, customButtonValueBackground_10);
                        _ShowControls(false, 10, customButtonName_11, customButtonValue_11, customButtonValueBackground_11);
                        _ShowControls(false, 11, customButtonName_12, customButtonValue_12, customButtonValueBackground_12);
                        _ShowControls(false, 12, customButtonName_13, customButtonValue_13, customButtonValueBackground_13);
                        _ShowControls(false, 13, customButtonName_14, customButtonValue_14, customButtonValueBackground_14);
                        _ShowControls(false, 14, customButtonName_15, customButtonValue_15, customButtonValueBackground_15);
                        _ShowControls(false, 15, customButtonName_16, customButtonValue_16, customButtonValueBackground_16);
                        _ShowControls(false, 16, customButtonName_17, customButtonValue_17, customButtonValueBackground_17);
                        _ShowControls(false, 17, customButtonName_18, customButtonValue_18, customButtonValueBackground_18);
                        _ShowControls(false, 18, customButtonName_19, customButtonValue_19, customButtonValueBackground_19);
                        _ShowControls(false, 19, customButtonName_20, customButtonValue_20, customButtonValueBackground_20);
                        _ShowControls(false, 20, customButtonName_21, customButtonValue_21, customButtonValueBackground_21);
                        _ShowControls(false, 21, customButtonName_22, customButtonValue_22, customButtonValueBackground_22);
                        _ShowControls(false, 22, customButtonName_23, customButtonValue_23, customButtonValueBackground_23);
                        _ShowControls(false, 23, customButtonName_24, customButtonValue_24, customButtonValueBackground_24);
                        _ShowControls(false, 24, customButtonName_25, customButtonValue_25, customButtonValueBackground_25);
                        _ShowControls(false, 25, customButtonName_26, customButtonValue_26, customButtonValueBackground_26);
                        _ShowControls(false, 26, customButtonName_27, customButtonValue_27, customButtonValueBackground_27);
                        _ShowControls(false, 27, customButtonName_28, customButtonValue_28, customButtonValueBackground_28);
                        _ShowControls(false, 28, customButtonName_29, customButtonValue_29, customButtonValueBackground_29);
                        _ShowControls(false, 29, customButtonName_30, customButtonValue_30, customButtonValueBackground_30);

                        //

                        if (bShowSettingsButton)//显示设置按钮
                        {
                            customButtonSubtract.Location = new System.Drawing.Point(customButtonSubtract.Left, customButtonName_1.Bottom + iNameBottom_SetTop_Height);//【-】
                            customButtonSet.Location = new System.Drawing.Point(customButtonSet.Left, customButtonName_1.Bottom + iNameBottom_SetTop_Height);//【Set】
                            customButtonPlus.Location = new System.Drawing.Point(customButtonPlus.Left, customButtonName_1.Bottom + iNameBottom_SetTop_Height);//【+】

                            this.Size = new Size(this.Size.Width, customButtonSet.Bottom + iSetBottom_ControlBottom_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }
                        else//不显示设置按钮
                        {
                            this.Size = new Size(this.Size.Width, customButtonName_1.Bottom + iNameBottom_SetTop_Height);
                            customButtonBackground.SizeButton = this.Size;
                        }

                        customButtonSubtract.Visible = bShowSettingsButton;//【-】
                        customButtonSet.Visible = bShowSettingsButton;//【Set】
                        customButtonPlus.Visible = bShowSettingsButton;//【+】

                        break;

                    case 2:
                        _ShowControls(true, 0, customButtonName_1, customButtonValue_1, customButtonValueBackground_1);
                        _ShowControls(true, 1, customButtonName_2, customButtonValue_2, customButtonValueBackground_2);
                        _ShowControls(false, 2, customButtonName_3, customButtonValue_3, customButtonValueBackground_3);
                        _ShowControls(false, 3, customButtonName_4, customButtonValue_4, customButtonValueBackground_4);
                        _ShowControls(false, 4, customButtonName_5, customButtonValue_5, customButtonValueBackground_5);
                        _ShowControls(false, 5, customButtonName_6, customButtonValue_6, customButtonValueBackground_6);
                        _ShowControls(false, 6, customButtonName_7, customButtonValue_7, customButtonValueBackground_7);
                        _ShowControls(false, 7, customButtonName_8, customButtonValue_8, customButtonValueBackground_8);
                        _ShowControls(false, 8, customButtonName_9, customButtonValue_9, customButtonValueBackground_9);
                        _ShowControls(false, 9, customButtonName_10, customButtonValue_10, customButtonValueBackground_10);
                        _ShowControls(false, 10, customButtonName_11, customButtonValue_11, customButtonValueBackground_11);
                        _ShowControls(false, 11, customButtonName_12, customButtonValue_12, customButtonValueBackground_12);
                        _ShowControls(false, 12, customButtonName_13, customButtonValue_13, customButtonValueBackground_13);
                        _ShowControls(false, 13, customButtonName_14, customButtonValue_14, customButtonValueBackground_14);
                        _ShowControls(false, 14, customButtonName_15, customButtonValue_15, customButtonValueBackground_15);
                        _ShowControls(false, 15, customButtonName_16, customButtonValue_16, customButtonValueBackground_16);
                        _ShowControls(false, 16, customButtonName_17, customButtonValue_17, customButtonValueBackground_17);
                        _ShowControls(false, 17, customButtonName_18, customButtonValue_18, customButtonValueBackground_18);
                        _ShowControls(false, 18, customButtonName_19, customButtonValue_19, customButtonValueBackground_19);
                        _ShowControls(false, 19, customButtonName_20, customButtonValue_20, customButtonValueBackground_20);
                        _ShowControls(false, 20, customButtonName_21, customButtonValue_21, customButtonValueBackground_21);
                        _ShowControls(false, 21, customButtonName_22, customButtonValue_22, customButtonValueBackground_22);
                        _ShowControls(false, 22, customButtonName_23, customButtonValue_23, customButtonValueBackground_23);
                        _ShowControls(false, 23, customButtonName_24, customButtonValue_24, customButtonValueBackground_24);
                        _ShowControls(false, 24, customButtonName_25, customButtonValue_25, customButtonValueBackground_25);
                        _ShowControls(false, 25, customButtonName_26, customButtonValue_26, customButtonValueBackground_26);
                        _ShowControls(false, 26, customButtonName_27, customButtonValue_27, customButtonValueBackground_27);
                        _ShowControls(false, 27, customButtonName_28, customButtonValue_28, customButtonValueBackground_28);
                        _ShowControls(false, 28, customButtonName_29, customButtonValue_29, customButtonValueBackground_29);
                        _ShowControls(false, 29, customButtonName_30, customButtonValue_30, customButtonValueBackground_30);

                        break;

                    case 3:
                        _ShowControls(true, 0, customButtonName_1, customButtonValue_1, customButtonValueBackground_1);
                        _ShowControls(true, 1, customButtonName_2, customButtonValue_2, customButtonValueBackground_2);
                        _ShowControls(true, 2, customButtonName_3, customButtonValue_3, customButtonValueBackground_3);
                        _ShowControls(false, 3, customButtonName_4, customButtonValue_4, customButtonValueBackground_4);
                        _ShowControls(false, 4, customButtonName_5, customButtonValue_5, customButtonValueBackground_5);
                        _ShowControls(false, 5, customButtonName_6, customButtonValue_6, customButtonValueBackground_6);
                        _ShowControls(false, 6, customButtonName_7, customButtonValue_7, customButtonValueBackground_7);
                        _ShowControls(false, 7, customButtonName_8, customButtonValue_8, customButtonValueBackground_8);
                        _ShowControls(false, 8, customButtonName_9, customButtonValue_9, customButtonValueBackground_9);
                        _ShowControls(false, 9, customButtonName_10, customButtonValue_10, customButtonValueBackground_10);
                        _ShowControls(false, 10, customButtonName_11, customButtonValue_11, customButtonValueBackground_11);
                        _ShowControls(false, 11, customButtonName_12, customButtonValue_12, customButtonValueBackground_12);
                        _ShowControls(false, 12, customButtonName_13, customButtonValue_13, customButtonValueBackground_13);
                        _ShowControls(false, 13, customButtonName_14, customButtonValue_14, customButtonValueBackground_14);
                        _ShowControls(false, 14, customButtonName_15, customButtonValue_15, customButtonValueBackground_15);
                        _ShowControls(false, 15, customButtonName_16, customButtonValue_16, customButtonValueBackground_16);
                        _ShowControls(false, 16, customButtonName_17, customButtonValue_17, customButtonValueBackground_17);
                        _ShowControls(false, 17, customButtonName_18, customButtonValue_18, customButtonValueBackground_18);
                        _ShowControls(false, 18, customButtonName_19, customButtonValue_19, customButtonValueBackground_19);
                        _ShowControls(false, 19, customButtonName_20, customButtonValue_20, customButtonValueBackground_20);
                        _ShowControls(false, 20, customButtonName_21, customButtonValue_21, customButtonValueBackground_21);
                        _ShowControls(false, 21, customButtonName_22, customButtonValue_22, customButtonValueBackground_22);
                        _ShowControls(false, 22, customButtonName_23, customButtonValue_23, customButtonValueBackground_23);
                        _ShowControls(false, 23, customButtonName_24, customButtonValue_24, customButtonValueBackground_24);
                        _ShowControls(false, 24, customButtonName_25, customButtonValue_25, customButtonValueBackground_25);
                        _ShowControls(false, 25, customButtonName_26, customButtonValue_26, customButtonValueBackground_26);
                        _ShowControls(false, 26, customButtonName_27, customButtonValue_27, customButtonValueBackground_27);
                        _ShowControls(false, 27, customButtonName_28, customButtonValue_28, customButtonValueBackground_28);
                        _ShowControls(false, 28, customButtonName_29, customButtonValue_29, customButtonValueBackground_29);
                        _ShowControls(false, 29, customButtonName_30, customButtonValue_30, customButtonValueBackground_30);

                        break;

                    case 4:
                        _ShowControls(true, 0, customButtonName_1, customButtonValue_1, customButtonValueBackground_1);
                        _ShowControls(true, 1, customButtonName_2, customButtonValue_2, customButtonValueBackground_2);
                        _ShowControls(true, 2, customButtonName_3, customButtonValue_3, customButtonValueBackground_3);
                        _ShowControls(true, 3, customButtonName_4, customButtonValue_4, customButtonValueBackground_4);
                        _ShowControls(false, 4, customButtonName_5, customButtonValue_5, customButtonValueBackground_5);
                        _ShowControls(false, 5, customButtonName_6, customButtonValue_6, customButtonValueBackground_6);
                        _ShowControls(false, 6, customButtonName_7, customButtonValue_7, customButtonValueBackground_7);
                        _ShowControls(false, 7, customButtonName_8, customButtonValue_8, customButtonValueBackground_8);
                        _ShowControls(false, 8, customButtonName_9, customButtonValue_9, customButtonValueBackground_9);
                        _ShowControls(false, 9, customButtonName_10, customButtonValue_10, customButtonValueBackground_10);
                        _ShowControls(false, 10, customButtonName_11, customButtonValue_11, customButtonValueBackground_11);
                        _ShowControls(false, 11, customButtonName_12, customButtonValue_12, customButtonValueBackground_12);
                        _ShowControls(false, 12, customButtonName_13, customButtonValue_13, customButtonValueBackground_13);
                        _ShowControls(false, 13, customButtonName_14, customButtonValue_14, customButtonValueBackground_14);
                        _ShowControls(false, 14, customButtonName_15, customButtonValue_15, customButtonValueBackground_15);
                        _ShowControls(false, 15, customButtonName_16, customButtonValue_16, customButtonValueBackground_16);
                        _ShowControls(false, 16, customButtonName_17, customButtonValue_17, customButtonValueBackground_17);
                        _ShowControls(false, 17, customButtonName_18, customButtonValue_18, customButtonValueBackground_18);
                        _ShowControls(false, 18, customButtonName_19, customButtonValue_19, customButtonValueBackground_19);
                        _ShowControls(false, 19, customButtonName_20, customButtonValue_20, customButtonValueBackground_20);
                        _ShowControls(false, 20, customButtonName_21, customButtonValue_21, customButtonValueBackground_21);
                        _ShowControls(false, 21, customButtonName_22, customButtonValue_22, customButtonValueBackground_22);
                        _ShowControls(false, 22, customButtonName_23, customButtonValue_23, customButtonValueBackground_23);
                        _ShowControls(false, 23, customButtonName_24, customButtonValue_24, customButtonValueBackground_24);
                        _ShowControls(false, 24, customButtonName_25, customButtonValue_25, customButtonValueBackground_25);
                        _ShowControls(false, 25, customButtonName_26, customButtonValue_26, customButtonValueBackground_26);
                        _ShowControls(false, 26, customButtonName_27, customButtonValue_27, customButtonValueBackground_27);
                        _ShowControls(false, 27, customButtonName_28, customButtonValue_28, customButtonValueBackground_28);
                        _ShowControls(false, 28, customButtonName_29, customButtonValue_29, customButtonValueBackground_29);
                        _ShowControls(false, 29, customButtonName_30, customButtonValue_30, customButtonValueBackground_30);

                        break;

                    case 5:
                        _ShowControls(true, 0, customButtonName_1, customButtonValue_1, customButtonValueBackground_1);
                        _ShowControls(true, 1, customButtonName_2, customButtonValue_2, customButtonValueBackground_2);
                        _ShowControls(true, 2, customButtonName_3, customButtonValue_3, customButtonValueBackground_3);
                        _ShowControls(true, 3, customButtonName_4, customButtonValue_4, customButtonValueBackground_4);
                        _ShowControls(true, 4, customButtonName_5, customButtonValue_5, customButtonValueBackground_5);
                        _ShowControls(false, 5, customButtonName_6, customButtonValue_6, customButtonValueBackground_6);
                        _ShowControls(false, 6, customButtonName_7, customButtonValue_7, customButtonValueBackground_7);
                        _ShowControls(false, 7, customButtonName_8, customButtonValue_8, customButtonValueBackground_8);
                        _ShowControls(false, 8, customButtonName_9, customButtonValue_9, customButtonValueBackground_9);
                        _ShowControls(false, 9, customButtonName_10, customButtonValue_10, customButtonValueBackground_10);
                        _ShowControls(false, 10, customButtonName_11, customButtonValue_11, customButtonValueBackground_11);
                        _ShowControls(false, 11, customButtonName_12, customButtonValue_12, customButtonValueBackground_12);
                        _ShowControls(false, 12, customButtonName_13, customButtonValue_13, customButtonValueBackground_13);
                        _ShowControls(false, 13, customButtonName_14, customButtonValue_14, customButtonValueBackground_14);
                        _ShowControls(false, 14, customButtonName_15, customButtonValue_15, customButtonValueBackground_15);
                        _ShowControls(false, 15, customButtonName_16, customButtonValue_16, customButtonValueBackground_16);
                        _ShowControls(false, 16, customButtonName_17, customButtonValue_17, customButtonValueBackground_17);
                        _ShowControls(false, 17, customButtonName_18, customButtonValue_18, customButtonValueBackground_18);
                        _ShowControls(false, 18, customButtonName_19, customButtonValue_19, customButtonValueBackground_19);
                        _ShowControls(false, 19, customButtonName_20, customButtonValue_20, customButtonValueBackground_20);
                        _ShowControls(false, 20, customButtonName_21, customButtonValue_21, customButtonValueBackground_21);
                        _ShowControls(false, 21, customButtonName_22, customButtonValue_22, customButtonValueBackground_22);
                        _ShowControls(false, 22, customButtonName_23, customButtonValue_23, customButtonValueBackground_23);
                        _ShowControls(false, 23, customButtonName_24, customButtonValue_24, customButtonValueBackground_24);
                        _ShowControls(false, 24, customButtonName_25, customButtonValue_25, customButtonValueBackground_25);
                        _ShowControls(false, 25, customButtonName_26, customButtonValue_26, customButtonValueBackground_26);
                        _ShowControls(false, 26, customButtonName_27, customButtonValue_27, customButtonValueBackground_27);
                        _ShowControls(false, 27, customButtonName_28, customButtonValue_28, customButtonValueBackground_28);
                        _ShowControls(false, 28, customButtonName_29, customButtonValue_29, customButtonValueBackground_29);
                        _ShowControls(false, 29, customButtonName_30, customButtonValue_30, customButtonValueBackground_30);

                        break;

                    case 6:
                        _ShowControls(true, 0, customButtonName_1, customButtonValue_1, customButtonValueBackground_1);
                        _ShowControls(true, 1, customButtonName_2, customButtonValue_2, customButtonValueBackground_2);
                        _ShowControls(true, 2, customButtonName_3, customButtonValue_3, customButtonValueBackground_3);
                        _ShowControls(true, 3, customButtonName_4, customButtonValue_4, customButtonValueBackground_4);
                        _ShowControls(true, 4, customButtonName_5, customButtonValue_5, customButtonValueBackground_5);
                        _ShowControls(true, 5, customButtonName_6, customButtonValue_6, customButtonValueBackground_6);
                        _ShowControls(false, 6, customButtonName_7, customButtonValue_7, customButtonValueBackground_7);
                        _ShowControls(false, 7, customButtonName_8, customButtonValue_8, customButtonValueBackground_8);
                        _ShowControls(false, 8, customButtonName_9, customButtonValue_9, customButtonValueBackground_9);
                        _ShowControls(false, 9, customButtonName_10, customButtonValue_10, customButtonValueBackground_10);
                        _ShowControls(false, 10, customButtonName_11, customButtonValue_11, customButtonValueBackground_11);
                        _ShowControls(false, 11, customButtonName_12, customButtonValue_12, customButtonValueBackground_12);
                        _ShowControls(false, 12, customButtonName_13, customButtonValue_13, customButtonValueBackground_13);
                        _ShowControls(false, 13, customButtonName_14, customButtonValue_14, customButtonValueBackground_14);
                        _ShowControls(false, 14, customButtonName_15, customButtonValue_15, customButtonValueBackground_15);
                        _ShowControls(false, 15, customButtonName_16, customButtonValue_16, customButtonValueBackground_16);
                        _ShowControls(false, 16, customButtonName_17, customButtonValue_17, customButtonValueBackground_17);
                        _ShowControls(false, 17, customButtonName_18, customButtonValue_18, customButtonValueBackground_18);
                        _ShowControls(false, 18, customButtonName_19, customButtonValue_19, customButtonValueBackground_19);
                        _ShowControls(false, 19, customButtonName_20, customButtonValue_20, customButtonValueBackground_20);
                        _ShowControls(false, 20, customButtonName_21, customButtonValue_21, customButtonValueBackground_21);
                        _ShowControls(false, 21, customButtonName_22, customButtonValue_22, customButtonValueBackground_22);
                        _ShowControls(false, 22, customButtonName_23, customButtonValue_23, customButtonValueBackground_23);
                        _ShowControls(false, 23, customButtonName_24, customButtonValue_24, customButtonValueBackground_24);
                        _ShowControls(false, 24, customButtonName_25, customButtonValue_25, customButtonValueBackground_25);
                        _ShowControls(false, 25, customButtonName_26, customButtonValue_26, customButtonValueBackground_26);
                        _ShowControls(false, 26, customButtonName_27, customButtonValue_27, customButtonValueBackground_27);
                        _ShowControls(false, 27, customButtonName_28, customButtonValue_28, customButtonValueBackground_28);
                        _ShowControls(false, 28, customButtonName_29, customButtonValue_29, customButtonValueBackground_29);
                        _ShowControls(false, 29, customButtonName_30, customButtonValue_30, customButtonValueBackground_30);

                        break;

                    case 7:
                        _ShowControls(true, 0, customButtonName_1, customButtonValue_1, customButtonValueBackground_1);
                        _ShowControls(true, 1, customButtonName_2, customButtonValue_2, customButtonValueBackground_2);
                        _ShowControls(true, 2, customButtonName_3, customButtonValue_3, customButtonValueBackground_3);
                        _ShowControls(true, 3, customButtonName_4, customButtonValue_4, customButtonValueBackground_4);
                        _ShowControls(true, 4, customButtonName_5, customButtonValue_5, customButtonValueBackground_5);
                        _ShowControls(true, 5, customButtonName_6, customButtonValue_6, customButtonValueBackground_6);
                        _ShowControls(true, 6, customButtonName_7, customButtonValue_7, customButtonValueBackground_7);
                        _ShowControls(false, 7, customButtonName_8, customButtonValue_8, customButtonValueBackground_8);
                        _ShowControls(false, 8, customButtonName_9, customButtonValue_9, customButtonValueBackground_9);
                        _ShowControls(false, 9, customButtonName_10, customButtonValue_10, customButtonValueBackground_10);
                        _ShowControls(false, 10, customButtonName_11, customButtonValue_11, customButtonValueBackground_11);
                        _ShowControls(false, 11, customButtonName_12, customButtonValue_12, customButtonValueBackground_12);
                        _ShowControls(false, 12, customButtonName_13, customButtonValue_13, customButtonValueBackground_13);
                        _ShowControls(false, 13, customButtonName_14, customButtonValue_14, customButtonValueBackground_14);
                        _ShowControls(false, 14, customButtonName_15, customButtonValue_15, customButtonValueBackground_15);
                        _ShowControls(false, 15, customButtonName_16, customButtonValue_16, customButtonValueBackground_16);
                        _ShowControls(false, 16, customButtonName_17, customButtonValue_17, customButtonValueBackground_17);
                        _ShowControls(false, 17, customButtonName_18, customButtonValue_18, customButtonValueBackground_18);
                        _ShowControls(false, 18, customButtonName_19, customButtonValue_19, customButtonValueBackground_19);
                        _ShowControls(false, 19, customButtonName_20, customButtonValue_20, customButtonValueBackground_20);
                        _ShowControls(false, 20, customButtonName_21, customButtonValue_21, customButtonValueBackground_21);
                        _ShowControls(false, 21, customButtonName_22, customButtonValue_22, customButtonValueBackground_22);
                        _ShowControls(false, 22, customButtonName_23, customButtonValue_23, customButtonValueBackground_23);
                        _ShowControls(false, 23, customButtonName_24, customButtonValue_24, customButtonValueBackground_24);
                        _ShowControls(false, 24, customButtonName_25, customButtonValue_25, customButtonValueBackground_25);
                        _ShowControls(false, 25, customButtonName_26, customButtonValue_26, customButtonValueBackground_26);
                        _ShowControls(false, 26, customButtonName_27, customButtonValue_27, customButtonValueBackground_27);
                        _ShowControls(false, 27, customButtonName_28, customButtonValue_28, customButtonValueBackground_28);
                        _ShowControls(false, 28, customButtonName_29, customButtonValue_29, customButtonValueBackground_29);
                        _ShowControls(false, 29, customButtonName_30, customButtonValue_30, customButtonValueBackground_30);

                        break;

                    case 8:
                        _ShowControls(true, 0, customButtonName_1, customButtonValue_1, customButtonValueBackground_1);
                        _ShowControls(true, 1, customButtonName_2, customButtonValue_2, customButtonValueBackground_2);
                        _ShowControls(true, 2, customButtonName_3, customButtonValue_3, customButtonValueBackground_3);
                        _ShowControls(true, 3, customButtonName_4, customButtonValue_4, customButtonValueBackground_4);
                        _ShowControls(true, 4, customButtonName_5, customButtonValue_5, customButtonValueBackground_5);
                        _ShowControls(true, 5, customButtonName_6, customButtonValue_6, customButtonValueBackground_6);
                        _ShowControls(true, 6, customButtonName_7, customButtonValue_7, customButtonValueBackground_7);
                        _ShowControls(true, 7, customButtonName_8, customButtonValue_8, customButtonValueBackground_8);
                        _ShowControls(false, 8, customButtonName_9, customButtonValue_9, customButtonValueBackground_9);
                        _ShowControls(false, 9, customButtonName_10, customButtonValue_10, customButtonValueBackground_10);
                        _ShowControls(false, 10, customButtonName_11, customButtonValue_11, customButtonValueBackground_11);
                        _ShowControls(false, 11, customButtonName_12, customButtonValue_12, customButtonValueBackground_12);
                        _ShowControls(false, 12, customButtonName_13, customButtonValue_13, customButtonValueBackground_13);
                        _ShowControls(false, 13, customButtonName_14, customButtonValue_14, customButtonValueBackground_14);
                        _ShowControls(false, 14, customButtonName_15, customButtonValue_15, customButtonValueBackground_15);
                        _ShowControls(false, 15, customButtonName_16, customButtonValue_16, customButtonValueBackground_16);
                        _ShowControls(false, 16, customButtonName_17, customButtonValue_17, customButtonValueBackground_17);
                        _ShowControls(false, 17, customButtonName_18, customButtonValue_18, customButtonValueBackground_18);
                        _ShowControls(false, 18, customButtonName_19, customButtonValue_19, customButtonValueBackground_19);
                        _ShowControls(false, 19, customButtonName_20, customButtonValue_20, customButtonValueBackground_20);
                        _ShowControls(false, 20, customButtonName_21, customButtonValue_21, customButtonValueBackground_21);
                        _ShowControls(false, 21, customButtonName_22, customButtonValue_22, customButtonValueBackground_22);
                        _ShowControls(false, 22, customButtonName_23, customButtonValue_23, customButtonValueBackground_23);
                        _ShowControls(false, 23, customButtonName_24, customButtonValue_24, customButtonValueBackground_24);
                        _ShowControls(false, 24, customButtonName_25, customButtonValue_25, customButtonValueBackground_25);
                        _ShowControls(false, 25, customButtonName_26, customButtonValue_26, customButtonValueBackground_26);
                        _ShowControls(false, 26, customButtonName_27, customButtonValue_27, customButtonValueBackground_27);
                        _ShowControls(false, 27, customButtonName_28, customButtonValue_28, customButtonValueBackground_28);
                        _ShowControls(false, 28, customButtonName_29, customButtonValue_29, customButtonValueBackground_29);
                        _ShowControls(false, 29, customButtonName_30, customButtonValue_30, customButtonValueBackground_30);

                        break;

                    case 9:
                        _ShowControls(true, 0, customButtonName_1, customButtonValue_1, customButtonValueBackground_1);
                        _ShowControls(true, 1, customButtonName_2, customButtonValue_2, customButtonValueBackground_2);
                        _ShowControls(true, 2, customButtonName_3, customButtonValue_3, customButtonValueBackground_3);
                        _ShowControls(true, 3, customButtonName_4, customButtonValue_4, customButtonValueBackground_4);
                        _ShowControls(true, 4, customButtonName_5, customButtonValue_5, customButtonValueBackground_5);
                        _ShowControls(true, 5, customButtonName_6, customButtonValue_6, customButtonValueBackground_6);
                        _ShowControls(true, 6, customButtonName_7, customButtonValue_7, customButtonValueBackground_7);
                        _ShowControls(true, 7, customButtonName_8, customButtonValue_8, customButtonValueBackground_8);
                        _ShowControls(true, 8, customButtonName_9, customButtonValue_9, customButtonValueBackground_9);
                        _ShowControls(false, 9, customButtonName_10, customButtonValue_10, customButtonValueBackground_10);
                        _ShowControls(false, 10, customButtonName_11, customButtonValue_11, customButtonValueBackground_11);
                        _ShowControls(false, 11, customButtonName_12, customButtonValue_12, customButtonValueBackground_12);
                        _ShowControls(false, 12, customButtonName_13, customButtonValue_13, customButtonValueBackground_13);
                        _ShowControls(false, 13, customButtonName_14, customButtonValue_14, customButtonValueBackground_14);
                        _ShowControls(false, 14, customButtonName_15, customButtonValue_15, customButtonValueBackground_15);
                        _ShowControls(false, 15, customButtonName_16, customButtonValue_16, customButtonValueBackground_16);
                        _ShowControls(false, 16, customButtonName_17, customButtonValue_17, customButtonValueBackground_17);
                        _ShowControls(false, 17, customButtonName_18, customButtonValue_18, customButtonValueBackground_18);
                        _ShowControls(false, 18, customButtonName_19, customButtonValue_19, customButtonValueBackground_19);
                        _ShowControls(false, 19, customButtonName_20, customButtonValue_20, customButtonValueBackground_20);
                        _ShowControls(false, 20, customButtonName_21, customButtonValue_21, customButtonValueBackground_21);
                        _ShowControls(false, 21, customButtonName_22, customButtonValue_22, customButtonValueBackground_22);
                        _ShowControls(false, 22, customButtonName_23, customButtonValue_23, customButtonValueBackground_23);
                        _ShowControls(false, 23, customButtonName_24, customButtonValue_24, customButtonValueBackground_24);
                        _ShowControls(false, 24, customButtonName_25, customButtonValue_25, customButtonValueBackground_25);
                        _ShowControls(false, 25, customButtonName_26, customButtonValue_26, customButtonValueBackground_26);
                        _ShowControls(false, 26, customButtonName_27, customButtonValue_27, customButtonValueBackground_27);
                        _ShowControls(false, 27, customButtonName_28, customButtonValue_28, customButtonValueBackground_28);
                        _ShowControls(false, 28, customButtonName_29, customButtonValue_29, customButtonValueBackground_29);
                        _ShowControls(false, 29, customButtonName_30, customButtonValue_30, customButtonValueBackground_30);

                        break;

                    case 10:
                        _ShowControls(true, 0, customButtonName_1, customButtonValue_1, customButtonValueBackground_1);
                        _ShowControls(true, 1, customButtonName_2, customButtonValue_2, customButtonValueBackground_2);
                        _ShowControls(true, 2, customButtonName_3, customButtonValue_3, customButtonValueBackground_3);
                        _ShowControls(true, 3, customButtonName_4, customButtonValue_4, customButtonValueBackground_4);
                        _ShowControls(true, 4, customButtonName_5, customButtonValue_5, customButtonValueBackground_5);
                        _ShowControls(true, 5, customButtonName_6, customButtonValue_6, customButtonValueBackground_6);
                        _ShowControls(true, 6, customButtonName_7, customButtonValue_7, customButtonValueBackground_7);
                        _ShowControls(true, 7, customButtonName_8, customButtonValue_8, customButtonValueBackground_8);
                        _ShowControls(true, 8, customButtonName_9, customButtonValue_9, customButtonValueBackground_9);
                        _ShowControls(true, 9, customButtonName_10, customButtonValue_10, customButtonValueBackground_10);
                        _ShowControls(false, 10, customButtonName_11, customButtonValue_11, customButtonValueBackground_11);
                        _ShowControls(false, 11, customButtonName_12, customButtonValue_12, customButtonValueBackground_12);
                        _ShowControls(false, 12, customButtonName_13, customButtonValue_13, customButtonValueBackground_13);
                        _ShowControls(false, 13, customButtonName_14, customButtonValue_14, customButtonValueBackground_14);
                        _ShowControls(false, 14, customButtonName_15, customButtonValue_15, customButtonValueBackground_15);
                        _ShowControls(false, 15, customButtonName_16, customButtonValue_16, customButtonValueBackground_16);
                        _ShowControls(false, 16, customButtonName_17, customButtonValue_17, customButtonValueBackground_17);
                        _ShowControls(false, 17, customButtonName_18, customButtonValue_18, customButtonValueBackground_18);
                        _ShowControls(false, 18, customButtonName_19, customButtonValue_19, customButtonValueBackground_19);
                        _ShowControls(false, 19, customButtonName_20, customButtonValue_20, customButtonValueBackground_20);
                        _ShowControls(false, 20, customButtonName_21, customButtonValue_21, customButtonValueBackground_21);
                        _ShowControls(false, 21, customButtonName_22, customButtonValue_22, customButtonValueBackground_22);
                        _ShowControls(false, 22, customButtonName_23, customButtonValue_23, customButtonValueBackground_23);
                        _ShowControls(false, 23, customButtonName_24, customButtonValue_24, customButtonValueBackground_24);
                        _ShowControls(false, 24, customButtonName_25, customButtonValue_25, customButtonValueBackground_25);
                        _ShowControls(false, 25, customButtonName_26, customButtonValue_26, customButtonValueBackground_26);
                        _ShowControls(false, 26, customButtonName_27, customButtonValue_27, customButtonValueBackground_27);
                        _ShowControls(false, 27, customButtonName_28, customButtonValue_28, customButtonValueBackground_28);
                        _ShowControls(false, 28, customButtonName_29, customButtonValue_29, customButtonValueBackground_29);
                        _ShowControls(false, 29, customButtonName_30, customButtonValue_30, customButtonValueBackground_30);

                        break;

                    default:

                        _ShowControls(false, 0, customButtonName_1, customButtonValue_1, customButtonValueBackground_1);
                        _ShowControls(false, 1, customButtonName_2, customButtonValue_2, customButtonValueBackground_2);
                        _ShowControls(false, 2, customButtonName_3, customButtonValue_3, customButtonValueBackground_3);
                        _ShowControls(false, 3, customButtonName_4, customButtonValue_4, customButtonValueBackground_4);
                        _ShowControls(false, 4, customButtonName_5, customButtonValue_5, customButtonValueBackground_5);
                        _ShowControls(false, 5, customButtonName_6, customButtonValue_6, customButtonValueBackground_6);
                        _ShowControls(false, 6, customButtonName_7, customButtonValue_7, customButtonValueBackground_7);
                        _ShowControls(false, 7, customButtonName_8, customButtonValue_8, customButtonValueBackground_8);
                        _ShowControls(false, 8, customButtonName_9, customButtonValue_9, customButtonValueBackground_9);
                        _ShowControls(false, 9, customButtonName_10, customButtonValue_10, customButtonValueBackground_10);
                        _ShowControls(false, 10, customButtonName_11, customButtonValue_11, customButtonValueBackground_11);
                        _ShowControls(false, 11, customButtonName_12, customButtonValue_12, customButtonValueBackground_12);
                        _ShowControls(false, 12, customButtonName_13, customButtonValue_13, customButtonValueBackground_13);
                        _ShowControls(false, 13, customButtonName_14, customButtonValue_14, customButtonValueBackground_14);
                        _ShowControls(false, 14, customButtonName_15, customButtonValue_15, customButtonValueBackground_15);
                        _ShowControls(false, 15, customButtonName_16, customButtonValue_16, customButtonValueBackground_16);
                        _ShowControls(false, 16, customButtonName_17, customButtonValue_17, customButtonValueBackground_17);
                        _ShowControls(false, 17, customButtonName_18, customButtonValue_18, customButtonValueBackground_18);
                        _ShowControls(false, 18, customButtonName_19, customButtonValue_19, customButtonValueBackground_19);
                        _ShowControls(false, 19, customButtonName_20, customButtonValue_20, customButtonValueBackground_20);
                        _ShowControls(false, 20, customButtonName_21, customButtonValue_21, customButtonValueBackground_21);
                        _ShowControls(false, 21, customButtonName_22, customButtonValue_22, customButtonValueBackground_22);
                        _ShowControls(false, 22, customButtonName_23, customButtonValue_23, customButtonValueBackground_23);
                        _ShowControls(false, 23, customButtonName_24, customButtonValue_24, customButtonValueBackground_24);
                        _ShowControls(false, 24, customButtonName_25, customButtonValue_25, customButtonValueBackground_25);
                        _ShowControls(false, 25, customButtonName_26, customButtonValue_26, customButtonValueBackground_26);
                        _ShowControls(false, 26, customButtonName_27, customButtonValue_27, customButtonValueBackground_27);
                        _ShowControls(false, 27, customButtonName_28, customButtonValue_28, customButtonValueBackground_28);
                        _ShowControls(false, 28, customButtonName_29, customButtonValue_29, customButtonValueBackground_29);
                        _ShowControls(false, 29, customButtonName_30, customButtonValue_30, customButtonValueBackground_30);

                        break;
                }
            }
            else if (iCurrentPage == 1) //当前处于第二页
            {
                switch (pageNumber)
                {
                    case 1:
                        _ShowControls(false, 0, customButtonName_1, customButtonValue_1, customButtonValueBackground_1);
                        _ShowControls(false, 1, customButtonName_2, customButtonValue_2, customButtonValueBackground_2);
                        _ShowControls(false, 2, customButtonName_3, customButtonValue_3, customButtonValueBackground_3);
                        _ShowControls(false, 3, customButtonName_4, customButtonValue_4, customButtonValueBackground_4);
                        _ShowControls(false, 4, customButtonName_5, customButtonValue_5, customButtonValueBackground_5);
                        _ShowControls(false, 5, customButtonName_6, customButtonValue_6, customButtonValueBackground_6);
                        _ShowControls(false, 6, customButtonName_7, customButtonValue_7, customButtonValueBackground_7);
                        _ShowControls(false, 7, customButtonName_8, customButtonValue_8, customButtonValueBackground_8);
                        _ShowControls(false, 8, customButtonName_9, customButtonValue_9, customButtonValueBackground_9);
                        _ShowControls(false, 9, customButtonName_10, customButtonValue_10, customButtonValueBackground_10);
                        _ShowControls(true, 10, customButtonName_11, customButtonValue_11, customButtonValueBackground_11);
                        _ShowControls(false, 11, customButtonName_12, customButtonValue_12, customButtonValueBackground_12);
                        _ShowControls(false, 12, customButtonName_13, customButtonValue_13, customButtonValueBackground_13);
                        _ShowControls(false, 13, customButtonName_14, customButtonValue_14, customButtonValueBackground_14);
                        _ShowControls(false, 14, customButtonName_15, customButtonValue_15, customButtonValueBackground_15);
                        _ShowControls(false, 15, customButtonName_16, customButtonValue_16, customButtonValueBackground_16);
                        _ShowControls(false, 16, customButtonName_17, customButtonValue_17, customButtonValueBackground_17);
                        _ShowControls(false, 17, customButtonName_18, customButtonValue_18, customButtonValueBackground_18);
                        _ShowControls(false, 18, customButtonName_19, customButtonValue_19, customButtonValueBackground_19);
                        _ShowControls(false, 19, customButtonName_20, customButtonValue_20, customButtonValueBackground_20);
                        _ShowControls(false, 20, customButtonName_21, customButtonValue_21, customButtonValueBackground_21);
                        _ShowControls(false, 21, customButtonName_22, customButtonValue_22, customButtonValueBackground_22);
                        _ShowControls(false, 22, customButtonName_23, customButtonValue_23, customButtonValueBackground_23);
                        _ShowControls(false, 23, customButtonName_24, customButtonValue_24, customButtonValueBackground_24);
                        _ShowControls(false, 24, customButtonName_25, customButtonValue_25, customButtonValueBackground_25);
                        _ShowControls(false, 25, customButtonName_26, customButtonValue_26, customButtonValueBackground_26);
                        _ShowControls(false, 26, customButtonName_27, customButtonValue_27, customButtonValueBackground_27);
                        _ShowControls(false, 27, customButtonName_28, customButtonValue_28, customButtonValueBackground_28);
                        _ShowControls(false, 28, customButtonName_29, customButtonValue_29, customButtonValueBackground_29);
                        _ShowControls(false, 29, customButtonName_30, customButtonValue_30, customButtonValueBackground_30);

                        break;

                    case 2:
                        _ShowControls(false, 0, customButtonName_1, customButtonValue_1, customButtonValueBackground_1);
                        _ShowControls(false, 1, customButtonName_2, customButtonValue_2, customButtonValueBackground_2);
                        _ShowControls(false, 2, customButtonName_3, customButtonValue_3, customButtonValueBackground_3);
                        _ShowControls(false, 3, customButtonName_4, customButtonValue_4, customButtonValueBackground_4);
                        _ShowControls(false, 4, customButtonName_5, customButtonValue_5, customButtonValueBackground_5);
                        _ShowControls(false, 5, customButtonName_6, customButtonValue_6, customButtonValueBackground_6);
                        _ShowControls(false, 6, customButtonName_7, customButtonValue_7, customButtonValueBackground_7);
                        _ShowControls(false, 7, customButtonName_8, customButtonValue_8, customButtonValueBackground_8);
                        _ShowControls(false, 8, customButtonName_9, customButtonValue_9, customButtonValueBackground_9);
                        _ShowControls(false, 9, customButtonName_10, customButtonValue_10, customButtonValueBackground_10);
                        _ShowControls(true, 10, customButtonName_11, customButtonValue_11, customButtonValueBackground_11);
                        _ShowControls(true, 11, customButtonName_12, customButtonValue_12, customButtonValueBackground_12);
                        _ShowControls(false, 12, customButtonName_13, customButtonValue_13, customButtonValueBackground_13);
                        _ShowControls(false, 13, customButtonName_14, customButtonValue_14, customButtonValueBackground_14);
                        _ShowControls(false, 14, customButtonName_15, customButtonValue_15, customButtonValueBackground_15);
                        _ShowControls(false, 15, customButtonName_16, customButtonValue_16, customButtonValueBackground_16);
                        _ShowControls(false, 16, customButtonName_17, customButtonValue_17, customButtonValueBackground_17);
                        _ShowControls(false, 17, customButtonName_18, customButtonValue_18, customButtonValueBackground_18);
                        _ShowControls(false, 18, customButtonName_19, customButtonValue_19, customButtonValueBackground_19);
                        _ShowControls(false, 19, customButtonName_20, customButtonValue_20, customButtonValueBackground_20);
                        _ShowControls(false, 20, customButtonName_21, customButtonValue_21, customButtonValueBackground_21);
                        _ShowControls(false, 21, customButtonName_22, customButtonValue_22, customButtonValueBackground_22);
                        _ShowControls(false, 22, customButtonName_23, customButtonValue_23, customButtonValueBackground_23);
                        _ShowControls(false, 23, customButtonName_24, customButtonValue_24, customButtonValueBackground_24);
                        _ShowControls(false, 24, customButtonName_25, customButtonValue_25, customButtonValueBackground_25);
                        _ShowControls(false, 25, customButtonName_26, customButtonValue_26, customButtonValueBackground_26);
                        _ShowControls(false, 26, customButtonName_27, customButtonValue_27, customButtonValueBackground_27);
                        _ShowControls(false, 27, customButtonName_28, customButtonValue_28, customButtonValueBackground_28);
                        _ShowControls(false, 28, customButtonName_29, customButtonValue_29, customButtonValueBackground_29);
                        _ShowControls(false, 29, customButtonName_30, customButtonValue_30, customButtonValueBackground_30);

                        break;

                    case 3:
                        _ShowControls(false, 0, customButtonName_1, customButtonValue_1, customButtonValueBackground_1);
                        _ShowControls(false, 1, customButtonName_2, customButtonValue_2, customButtonValueBackground_2);
                        _ShowControls(false, 2, customButtonName_3, customButtonValue_3, customButtonValueBackground_3);
                        _ShowControls(false, 3, customButtonName_4, customButtonValue_4, customButtonValueBackground_4);
                        _ShowControls(false, 4, customButtonName_5, customButtonValue_5, customButtonValueBackground_5);
                        _ShowControls(false, 5, customButtonName_6, customButtonValue_6, customButtonValueBackground_6);
                        _ShowControls(false, 6, customButtonName_7, customButtonValue_7, customButtonValueBackground_7);
                        _ShowControls(false, 7, customButtonName_8, customButtonValue_8, customButtonValueBackground_8);
                        _ShowControls(false, 8, customButtonName_9, customButtonValue_9, customButtonValueBackground_9);
                        _ShowControls(false, 9, customButtonName_10, customButtonValue_10, customButtonValueBackground_10);
                        _ShowControls(true, 10, customButtonName_11, customButtonValue_11, customButtonValueBackground_11);
                        _ShowControls(true, 11, customButtonName_12, customButtonValue_12, customButtonValueBackground_12);
                        _ShowControls(true, 12, customButtonName_13, customButtonValue_13, customButtonValueBackground_13);
                        _ShowControls(false, 13, customButtonName_14, customButtonValue_14, customButtonValueBackground_14);
                        _ShowControls(false, 14, customButtonName_15, customButtonValue_15, customButtonValueBackground_15);
                        _ShowControls(false, 15, customButtonName_16, customButtonValue_16, customButtonValueBackground_16);
                        _ShowControls(false, 16, customButtonName_17, customButtonValue_17, customButtonValueBackground_17);
                        _ShowControls(false, 17, customButtonName_18, customButtonValue_18, customButtonValueBackground_18);
                        _ShowControls(false, 18, customButtonName_19, customButtonValue_19, customButtonValueBackground_19);
                        _ShowControls(false, 19, customButtonName_20, customButtonValue_20, customButtonValueBackground_20);
                        _ShowControls(false, 20, customButtonName_21, customButtonValue_21, customButtonValueBackground_21);
                        _ShowControls(false, 21, customButtonName_22, customButtonValue_22, customButtonValueBackground_22);
                        _ShowControls(false, 22, customButtonName_23, customButtonValue_23, customButtonValueBackground_23);
                        _ShowControls(false, 23, customButtonName_24, customButtonValue_24, customButtonValueBackground_24);
                        _ShowControls(false, 24, customButtonName_25, customButtonValue_25, customButtonValueBackground_25);
                        _ShowControls(false, 25, customButtonName_26, customButtonValue_26, customButtonValueBackground_26);
                        _ShowControls(false, 26, customButtonName_27, customButtonValue_27, customButtonValueBackground_27);
                        _ShowControls(false, 27, customButtonName_28, customButtonValue_28, customButtonValueBackground_28);
                        _ShowControls(false, 28, customButtonName_29, customButtonValue_29, customButtonValueBackground_29);
                        _ShowControls(false, 29, customButtonName_30, customButtonValue_30, customButtonValueBackground_30);

                        break;

                    case 4:
                        _ShowControls(false, 0, customButtonName_1, customButtonValue_1, customButtonValueBackground_1);
                        _ShowControls(false, 1, customButtonName_2, customButtonValue_2, customButtonValueBackground_2);
                        _ShowControls(false, 2, customButtonName_3, customButtonValue_3, customButtonValueBackground_3);
                        _ShowControls(false, 3, customButtonName_4, customButtonValue_4, customButtonValueBackground_4);
                        _ShowControls(false, 4, customButtonName_5, customButtonValue_5, customButtonValueBackground_5);
                        _ShowControls(false, 5, customButtonName_6, customButtonValue_6, customButtonValueBackground_6);
                        _ShowControls(false, 6, customButtonName_7, customButtonValue_7, customButtonValueBackground_7);
                        _ShowControls(false, 7, customButtonName_8, customButtonValue_8, customButtonValueBackground_8);
                        _ShowControls(false, 8, customButtonName_9, customButtonValue_9, customButtonValueBackground_9);
                        _ShowControls(false, 9, customButtonName_10, customButtonValue_10, customButtonValueBackground_10);
                        _ShowControls(true, 10, customButtonName_11, customButtonValue_11, customButtonValueBackground_11);
                        _ShowControls(true, 11, customButtonName_12, customButtonValue_12, customButtonValueBackground_12);
                        _ShowControls(true, 12, customButtonName_13, customButtonValue_13, customButtonValueBackground_13);
                        _ShowControls(true, 13, customButtonName_14, customButtonValue_14, customButtonValueBackground_14);
                        _ShowControls(false, 14, customButtonName_15, customButtonValue_15, customButtonValueBackground_15);
                        _ShowControls(false, 15, customButtonName_16, customButtonValue_16, customButtonValueBackground_16);
                        _ShowControls(false, 16, customButtonName_17, customButtonValue_17, customButtonValueBackground_17);
                        _ShowControls(false, 17, customButtonName_18, customButtonValue_18, customButtonValueBackground_18);
                        _ShowControls(false, 18, customButtonName_19, customButtonValue_19, customButtonValueBackground_19);
                        _ShowControls(false, 19, customButtonName_20, customButtonValue_20, customButtonValueBackground_20);
                        _ShowControls(false, 20, customButtonName_21, customButtonValue_21, customButtonValueBackground_21);
                        _ShowControls(false, 21, customButtonName_22, customButtonValue_22, customButtonValueBackground_22);
                        _ShowControls(false, 22, customButtonName_23, customButtonValue_23, customButtonValueBackground_23);
                        _ShowControls(false, 23, customButtonName_24, customButtonValue_24, customButtonValueBackground_24);
                        _ShowControls(false, 24, customButtonName_25, customButtonValue_25, customButtonValueBackground_25);
                        _ShowControls(false, 25, customButtonName_26, customButtonValue_26, customButtonValueBackground_26);
                        _ShowControls(false, 26, customButtonName_27, customButtonValue_27, customButtonValueBackground_27);
                        _ShowControls(false, 27, customButtonName_28, customButtonValue_28, customButtonValueBackground_28);
                        _ShowControls(false, 28, customButtonName_29, customButtonValue_29, customButtonValueBackground_29);
                        _ShowControls(false, 29, customButtonName_30, customButtonValue_30, customButtonValueBackground_30);

                        break;

                    case 5:
                        _ShowControls(false, 0, customButtonName_1, customButtonValue_1, customButtonValueBackground_1);
                        _ShowControls(false, 1, customButtonName_2, customButtonValue_2, customButtonValueBackground_2);
                        _ShowControls(false, 2, customButtonName_3, customButtonValue_3, customButtonValueBackground_3);
                        _ShowControls(false, 3, customButtonName_4, customButtonValue_4, customButtonValueBackground_4);
                        _ShowControls(false, 4, customButtonName_5, customButtonValue_5, customButtonValueBackground_5);
                        _ShowControls(false, 5, customButtonName_6, customButtonValue_6, customButtonValueBackground_6);
                        _ShowControls(false, 6, customButtonName_7, customButtonValue_7, customButtonValueBackground_7);
                        _ShowControls(false, 7, customButtonName_8, customButtonValue_8, customButtonValueBackground_8);
                        _ShowControls(false, 8, customButtonName_9, customButtonValue_9, customButtonValueBackground_9);
                        _ShowControls(false, 9, customButtonName_10, customButtonValue_10, customButtonValueBackground_10);
                        _ShowControls(true, 10, customButtonName_11, customButtonValue_11, customButtonValueBackground_11);
                        _ShowControls(true, 11, customButtonName_12, customButtonValue_12, customButtonValueBackground_12);
                        _ShowControls(true, 12, customButtonName_13, customButtonValue_13, customButtonValueBackground_13);
                        _ShowControls(true, 13, customButtonName_14, customButtonValue_14, customButtonValueBackground_14);
                        _ShowControls(true, 14, customButtonName_15, customButtonValue_15, customButtonValueBackground_15);
                        _ShowControls(false, 15, customButtonName_16, customButtonValue_16, customButtonValueBackground_16);
                        _ShowControls(false, 16, customButtonName_17, customButtonValue_17, customButtonValueBackground_17);
                        _ShowControls(false, 17, customButtonName_18, customButtonValue_18, customButtonValueBackground_18);
                        _ShowControls(false, 18, customButtonName_19, customButtonValue_19, customButtonValueBackground_19);
                        _ShowControls(false, 19, customButtonName_20, customButtonValue_20, customButtonValueBackground_20);
                        _ShowControls(false, 20, customButtonName_21, customButtonValue_21, customButtonValueBackground_21);
                        _ShowControls(false, 21, customButtonName_22, customButtonValue_22, customButtonValueBackground_22);
                        _ShowControls(false, 22, customButtonName_23, customButtonValue_23, customButtonValueBackground_23);
                        _ShowControls(false, 23, customButtonName_24, customButtonValue_24, customButtonValueBackground_24);
                        _ShowControls(false, 24, customButtonName_25, customButtonValue_25, customButtonValueBackground_25);
                        _ShowControls(false, 25, customButtonName_26, customButtonValue_26, customButtonValueBackground_26);
                        _ShowControls(false, 26, customButtonName_27, customButtonValue_27, customButtonValueBackground_27);
                        _ShowControls(false, 27, customButtonName_28, customButtonValue_28, customButtonValueBackground_28);
                        _ShowControls(false, 28, customButtonName_29, customButtonValue_29, customButtonValueBackground_29);
                        _ShowControls(false, 29, customButtonName_30, customButtonValue_30, customButtonValueBackground_30);

                        break;

                    case 6:
                        _ShowControls(false, 0, customButtonName_1, customButtonValue_1, customButtonValueBackground_1);
                        _ShowControls(false, 1, customButtonName_2, customButtonValue_2, customButtonValueBackground_2);
                        _ShowControls(false, 2, customButtonName_3, customButtonValue_3, customButtonValueBackground_3);
                        _ShowControls(false, 3, customButtonName_4, customButtonValue_4, customButtonValueBackground_4);
                        _ShowControls(false, 4, customButtonName_5, customButtonValue_5, customButtonValueBackground_5);
                        _ShowControls(false, 5, customButtonName_6, customButtonValue_6, customButtonValueBackground_6);
                        _ShowControls(false, 6, customButtonName_7, customButtonValue_7, customButtonValueBackground_7);
                        _ShowControls(false, 7, customButtonName_8, customButtonValue_8, customButtonValueBackground_8);
                        _ShowControls(false, 8, customButtonName_9, customButtonValue_9, customButtonValueBackground_9);
                        _ShowControls(false, 9, customButtonName_10, customButtonValue_10, customButtonValueBackground_10);
                        _ShowControls(true, 10, customButtonName_11, customButtonValue_11, customButtonValueBackground_11);
                        _ShowControls(true, 11, customButtonName_12, customButtonValue_12, customButtonValueBackground_12);
                        _ShowControls(true, 12, customButtonName_13, customButtonValue_13, customButtonValueBackground_13);
                        _ShowControls(true, 13, customButtonName_14, customButtonValue_14, customButtonValueBackground_14);
                        _ShowControls(true, 14, customButtonName_15, customButtonValue_15, customButtonValueBackground_15);
                        _ShowControls(true, 15, customButtonName_16, customButtonValue_16, customButtonValueBackground_16);
                        _ShowControls(false, 16, customButtonName_17, customButtonValue_17, customButtonValueBackground_17);
                        _ShowControls(false, 17, customButtonName_18, customButtonValue_18, customButtonValueBackground_18);
                        _ShowControls(false, 18, customButtonName_19, customButtonValue_19, customButtonValueBackground_19);
                        _ShowControls(false, 19, customButtonName_20, customButtonValue_20, customButtonValueBackground_20);
                        _ShowControls(false, 20, customButtonName_21, customButtonValue_21, customButtonValueBackground_21);
                        _ShowControls(false, 21, customButtonName_22, customButtonValue_22, customButtonValueBackground_22);
                        _ShowControls(false, 22, customButtonName_23, customButtonValue_23, customButtonValueBackground_23);
                        _ShowControls(false, 23, customButtonName_24, customButtonValue_24, customButtonValueBackground_24);
                        _ShowControls(false, 24, customButtonName_25, customButtonValue_25, customButtonValueBackground_25);
                        _ShowControls(false, 25, customButtonName_26, customButtonValue_26, customButtonValueBackground_26);
                        _ShowControls(false, 26, customButtonName_27, customButtonValue_27, customButtonValueBackground_27);
                        _ShowControls(false, 27, customButtonName_28, customButtonValue_28, customButtonValueBackground_28);
                        _ShowControls(false, 28, customButtonName_29, customButtonValue_29, customButtonValueBackground_29);
                        _ShowControls(false, 29, customButtonName_30, customButtonValue_30, customButtonValueBackground_30);

                        break;

                    case 7:
                        _ShowControls(false, 0, customButtonName_1, customButtonValue_1, customButtonValueBackground_1);
                        _ShowControls(false, 1, customButtonName_2, customButtonValue_2, customButtonValueBackground_2);
                        _ShowControls(false, 2, customButtonName_3, customButtonValue_3, customButtonValueBackground_3);
                        _ShowControls(false, 3, customButtonName_4, customButtonValue_4, customButtonValueBackground_4);
                        _ShowControls(false, 4, customButtonName_5, customButtonValue_5, customButtonValueBackground_5);
                        _ShowControls(false, 5, customButtonName_6, customButtonValue_6, customButtonValueBackground_6);
                        _ShowControls(false, 6, customButtonName_7, customButtonValue_7, customButtonValueBackground_7);
                        _ShowControls(false, 7, customButtonName_8, customButtonValue_8, customButtonValueBackground_8);
                        _ShowControls(false, 8, customButtonName_9, customButtonValue_9, customButtonValueBackground_9);
                        _ShowControls(false, 9, customButtonName_10, customButtonValue_10, customButtonValueBackground_10);
                        _ShowControls(true, 10, customButtonName_11, customButtonValue_11, customButtonValueBackground_11);
                        _ShowControls(true, 11, customButtonName_12, customButtonValue_12, customButtonValueBackground_12);
                        _ShowControls(true, 12, customButtonName_13, customButtonValue_13, customButtonValueBackground_13);
                        _ShowControls(true, 13, customButtonName_14, customButtonValue_14, customButtonValueBackground_14);
                        _ShowControls(true, 14, customButtonName_15, customButtonValue_15, customButtonValueBackground_15);
                        _ShowControls(true, 15, customButtonName_16, customButtonValue_16, customButtonValueBackground_16);
                        _ShowControls(true, 16, customButtonName_17, customButtonValue_17, customButtonValueBackground_17);
                        _ShowControls(false, 17, customButtonName_18, customButtonValue_18, customButtonValueBackground_18);
                        _ShowControls(false, 18, customButtonName_19, customButtonValue_19, customButtonValueBackground_19);
                        _ShowControls(false, 19, customButtonName_20, customButtonValue_20, customButtonValueBackground_20);
                        _ShowControls(false, 20, customButtonName_21, customButtonValue_21, customButtonValueBackground_21);
                        _ShowControls(false, 21, customButtonName_22, customButtonValue_22, customButtonValueBackground_22);
                        _ShowControls(false, 22, customButtonName_23, customButtonValue_23, customButtonValueBackground_23);
                        _ShowControls(false, 23, customButtonName_24, customButtonValue_24, customButtonValueBackground_24);
                        _ShowControls(false, 24, customButtonName_25, customButtonValue_25, customButtonValueBackground_25);
                        _ShowControls(false, 25, customButtonName_26, customButtonValue_26, customButtonValueBackground_26);
                        _ShowControls(false, 26, customButtonName_27, customButtonValue_27, customButtonValueBackground_27);
                        _ShowControls(false, 27, customButtonName_28, customButtonValue_28, customButtonValueBackground_28);
                        _ShowControls(false, 28, customButtonName_29, customButtonValue_29, customButtonValueBackground_29);
                        _ShowControls(false, 29, customButtonName_30, customButtonValue_30, customButtonValueBackground_30);

                        break;

                    case 8:
                        _ShowControls(false, 0, customButtonName_1, customButtonValue_1, customButtonValueBackground_1);
                        _ShowControls(false, 1, customButtonName_2, customButtonValue_2, customButtonValueBackground_2);
                        _ShowControls(false, 2, customButtonName_3, customButtonValue_3, customButtonValueBackground_3);
                        _ShowControls(false, 3, customButtonName_4, customButtonValue_4, customButtonValueBackground_4);
                        _ShowControls(false, 4, customButtonName_5, customButtonValue_5, customButtonValueBackground_5);
                        _ShowControls(false, 5, customButtonName_6, customButtonValue_6, customButtonValueBackground_6);
                        _ShowControls(false, 6, customButtonName_7, customButtonValue_7, customButtonValueBackground_7);
                        _ShowControls(false, 7, customButtonName_8, customButtonValue_8, customButtonValueBackground_8);
                        _ShowControls(false, 8, customButtonName_9, customButtonValue_9, customButtonValueBackground_9);
                        _ShowControls(false, 9, customButtonName_10, customButtonValue_10, customButtonValueBackground_10);
                        _ShowControls(true, 10, customButtonName_11, customButtonValue_11, customButtonValueBackground_11);
                        _ShowControls(true, 11, customButtonName_12, customButtonValue_12, customButtonValueBackground_12);
                        _ShowControls(true, 12, customButtonName_13, customButtonValue_13, customButtonValueBackground_13);
                        _ShowControls(true, 13, customButtonName_14, customButtonValue_14, customButtonValueBackground_14);
                        _ShowControls(true, 14, customButtonName_15, customButtonValue_15, customButtonValueBackground_15);
                        _ShowControls(true, 15, customButtonName_16, customButtonValue_16, customButtonValueBackground_16);
                        _ShowControls(true, 16, customButtonName_17, customButtonValue_17, customButtonValueBackground_17);
                        _ShowControls(true, 17, customButtonName_18, customButtonValue_18, customButtonValueBackground_18);
                        _ShowControls(false, 18, customButtonName_19, customButtonValue_19, customButtonValueBackground_19);
                        _ShowControls(false, 19, customButtonName_20, customButtonValue_20, customButtonValueBackground_20);
                        _ShowControls(false, 20, customButtonName_21, customButtonValue_21, customButtonValueBackground_21);
                        _ShowControls(false, 21, customButtonName_22, customButtonValue_22, customButtonValueBackground_22);
                        _ShowControls(false, 22, customButtonName_23, customButtonValue_23, customButtonValueBackground_23);
                        _ShowControls(false, 23, customButtonName_24, customButtonValue_24, customButtonValueBackground_24);
                        _ShowControls(false, 24, customButtonName_25, customButtonValue_25, customButtonValueBackground_25);
                        _ShowControls(false, 25, customButtonName_26, customButtonValue_26, customButtonValueBackground_26);
                        _ShowControls(false, 26, customButtonName_27, customButtonValue_27, customButtonValueBackground_27);
                        _ShowControls(false, 27, customButtonName_28, customButtonValue_28, customButtonValueBackground_28);
                        _ShowControls(false, 28, customButtonName_29, customButtonValue_29, customButtonValueBackground_29);
                        _ShowControls(false, 29, customButtonName_30, customButtonValue_30, customButtonValueBackground_30);

                        break;

                    case 9:
                        _ShowControls(false, 0, customButtonName_1, customButtonValue_1, customButtonValueBackground_1);
                        _ShowControls(false, 1, customButtonName_2, customButtonValue_2, customButtonValueBackground_2);
                        _ShowControls(false, 2, customButtonName_3, customButtonValue_3, customButtonValueBackground_3);
                        _ShowControls(false, 3, customButtonName_4, customButtonValue_4, customButtonValueBackground_4);
                        _ShowControls(false, 4, customButtonName_5, customButtonValue_5, customButtonValueBackground_5);
                        _ShowControls(false, 5, customButtonName_6, customButtonValue_6, customButtonValueBackground_6);
                        _ShowControls(false, 6, customButtonName_7, customButtonValue_7, customButtonValueBackground_7);
                        _ShowControls(false, 7, customButtonName_8, customButtonValue_8, customButtonValueBackground_8);
                        _ShowControls(false, 8, customButtonName_9, customButtonValue_9, customButtonValueBackground_9);
                        _ShowControls(false, 9, customButtonName_10, customButtonValue_10, customButtonValueBackground_10);
                        _ShowControls(true, 10, customButtonName_11, customButtonValue_11, customButtonValueBackground_11);
                        _ShowControls(true, 11, customButtonName_12, customButtonValue_12, customButtonValueBackground_12);
                        _ShowControls(true, 12, customButtonName_13, customButtonValue_13, customButtonValueBackground_13);
                        _ShowControls(true, 13, customButtonName_14, customButtonValue_14, customButtonValueBackground_14);
                        _ShowControls(true, 14, customButtonName_15, customButtonValue_15, customButtonValueBackground_15);
                        _ShowControls(true, 15, customButtonName_16, customButtonValue_16, customButtonValueBackground_16);
                        _ShowControls(true, 16, customButtonName_17, customButtonValue_17, customButtonValueBackground_17);
                        _ShowControls(true, 17, customButtonName_18, customButtonValue_18, customButtonValueBackground_18);
                        _ShowControls(true, 18, customButtonName_19, customButtonValue_19, customButtonValueBackground_19);
                        _ShowControls(false, 19, customButtonName_20, customButtonValue_20, customButtonValueBackground_20);
                        _ShowControls(false, 20, customButtonName_21, customButtonValue_21, customButtonValueBackground_21);
                        _ShowControls(false, 21, customButtonName_22, customButtonValue_22, customButtonValueBackground_22);
                        _ShowControls(false, 22, customButtonName_23, customButtonValue_23, customButtonValueBackground_23);
                        _ShowControls(false, 23, customButtonName_24, customButtonValue_24, customButtonValueBackground_24);
                        _ShowControls(false, 24, customButtonName_25, customButtonValue_25, customButtonValueBackground_25);
                        _ShowControls(false, 25, customButtonName_26, customButtonValue_26, customButtonValueBackground_26);
                        _ShowControls(false, 26, customButtonName_27, customButtonValue_27, customButtonValueBackground_27);
                        _ShowControls(false, 27, customButtonName_28, customButtonValue_28, customButtonValueBackground_28);
                        _ShowControls(false, 28, customButtonName_29, customButtonValue_29, customButtonValueBackground_29);
                        _ShowControls(false, 29, customButtonName_30, customButtonValue_30, customButtonValueBackground_30);

                        break;

                    case 10:
                        _ShowControls(false, 0, customButtonName_1, customButtonValue_1, customButtonValueBackground_1);
                        _ShowControls(false, 1, customButtonName_2, customButtonValue_2, customButtonValueBackground_2);
                        _ShowControls(false, 2, customButtonName_3, customButtonValue_3, customButtonValueBackground_3);
                        _ShowControls(false, 3, customButtonName_4, customButtonValue_4, customButtonValueBackground_4);
                        _ShowControls(false, 4, customButtonName_5, customButtonValue_5, customButtonValueBackground_5);
                        _ShowControls(false, 5, customButtonName_6, customButtonValue_6, customButtonValueBackground_6);
                        _ShowControls(false, 6, customButtonName_7, customButtonValue_7, customButtonValueBackground_7);
                        _ShowControls(false, 7, customButtonName_8, customButtonValue_8, customButtonValueBackground_8);
                        _ShowControls(false, 8, customButtonName_9, customButtonValue_9, customButtonValueBackground_9);
                        _ShowControls(false, 9, customButtonName_10, customButtonValue_10, customButtonValueBackground_10);
                        _ShowControls(true, 10, customButtonName_11, customButtonValue_11, customButtonValueBackground_11);
                        _ShowControls(true, 11, customButtonName_12, customButtonValue_12, customButtonValueBackground_12);
                        _ShowControls(true, 12, customButtonName_13, customButtonValue_13, customButtonValueBackground_13);
                        _ShowControls(true, 13, customButtonName_14, customButtonValue_14, customButtonValueBackground_14);
                        _ShowControls(true, 14, customButtonName_15, customButtonValue_15, customButtonValueBackground_15);
                        _ShowControls(true, 15, customButtonName_16, customButtonValue_16, customButtonValueBackground_16);
                        _ShowControls(true, 16, customButtonName_17, customButtonValue_17, customButtonValueBackground_17);
                        _ShowControls(true, 17, customButtonName_18, customButtonValue_18, customButtonValueBackground_18);
                        _ShowControls(true, 18, customButtonName_19, customButtonValue_19, customButtonValueBackground_19);
                        _ShowControls(true, 19, customButtonName_20, customButtonValue_20, customButtonValueBackground_20);
                        _ShowControls(false, 20, customButtonName_21, customButtonValue_21, customButtonValueBackground_21);
                        _ShowControls(false, 21, customButtonName_22, customButtonValue_22, customButtonValueBackground_22);
                        _ShowControls(false, 22, customButtonName_23, customButtonValue_23, customButtonValueBackground_23);
                        _ShowControls(false, 23, customButtonName_24, customButtonValue_24, customButtonValueBackground_24);
                        _ShowControls(false, 24, customButtonName_25, customButtonValue_25, customButtonValueBackground_25);
                        _ShowControls(false, 25, customButtonName_26, customButtonValue_26, customButtonValueBackground_26);
                        _ShowControls(false, 26, customButtonName_27, customButtonValue_27, customButtonValueBackground_27);
                        _ShowControls(false, 27, customButtonName_28, customButtonValue_28, customButtonValueBackground_28);
                        _ShowControls(false, 28, customButtonName_29, customButtonValue_29, customButtonValueBackground_29);
                        _ShowControls(false, 29, customButtonName_30, customButtonValue_30, customButtonValueBackground_30);

                        break;

                    default:

                        _ShowControls(false, 0, customButtonName_1, customButtonValue_1, customButtonValueBackground_1);
                        _ShowControls(false, 1, customButtonName_2, customButtonValue_2, customButtonValueBackground_2);
                        _ShowControls(false, 2, customButtonName_3, customButtonValue_3, customButtonValueBackground_3);
                        _ShowControls(false, 3, customButtonName_4, customButtonValue_4, customButtonValueBackground_4);
                        _ShowControls(false, 4, customButtonName_5, customButtonValue_5, customButtonValueBackground_5);
                        _ShowControls(false, 5, customButtonName_6, customButtonValue_6, customButtonValueBackground_6);
                        _ShowControls(false, 6, customButtonName_7, customButtonValue_7, customButtonValueBackground_7);
                        _ShowControls(false, 7, customButtonName_8, customButtonValue_8, customButtonValueBackground_8);
                        _ShowControls(false, 8, customButtonName_9, customButtonValue_9, customButtonValueBackground_9);
                        _ShowControls(false, 9, customButtonName_10, customButtonValue_10, customButtonValueBackground_10);
                        _ShowControls(false, 10, customButtonName_11, customButtonValue_11, customButtonValueBackground_11);
                        _ShowControls(false, 11, customButtonName_12, customButtonValue_12, customButtonValueBackground_12);
                        _ShowControls(false, 12, customButtonName_13, customButtonValue_13, customButtonValueBackground_13);
                        _ShowControls(false, 13, customButtonName_14, customButtonValue_14, customButtonValueBackground_14);
                        _ShowControls(false, 14, customButtonName_15, customButtonValue_15, customButtonValueBackground_15);
                        _ShowControls(false, 15, customButtonName_16, customButtonValue_16, customButtonValueBackground_16);
                        _ShowControls(false, 16, customButtonName_17, customButtonValue_17, customButtonValueBackground_17);
                        _ShowControls(false, 17, customButtonName_18, customButtonValue_18, customButtonValueBackground_18);
                        _ShowControls(false, 18, customButtonName_19, customButtonValue_19, customButtonValueBackground_19);
                        _ShowControls(false, 19, customButtonName_20, customButtonValue_20, customButtonValueBackground_20);
                        _ShowControls(false, 20, customButtonName_21, customButtonValue_21, customButtonValueBackground_21);
                        _ShowControls(false, 21, customButtonName_22, customButtonValue_22, customButtonValueBackground_22);
                        _ShowControls(false, 22, customButtonName_23, customButtonValue_23, customButtonValueBackground_23);
                        _ShowControls(false, 23, customButtonName_24, customButtonValue_24, customButtonValueBackground_24);
                        _ShowControls(false, 24, customButtonName_25, customButtonValue_25, customButtonValueBackground_25);
                        _ShowControls(false, 25, customButtonName_26, customButtonValue_26, customButtonValueBackground_26);
                        _ShowControls(false, 26, customButtonName_27, customButtonValue_27, customButtonValueBackground_27);
                        _ShowControls(false, 27, customButtonName_28, customButtonValue_28, customButtonValueBackground_28);
                        _ShowControls(false, 28, customButtonName_29, customButtonValue_29, customButtonValueBackground_29);
                        _ShowControls(false, 29, customButtonName_30, customButtonValue_30, customButtonValueBackground_30);

                        break;
                }
            }
            else if (iCurrentPage == 2) //当前处于第三页
            {
                switch (pageNumber)
                {
                    case 1:
                        _ShowControls(false, 0, customButtonName_1, customButtonValue_1, customButtonValueBackground_1);
                        _ShowControls(false, 1, customButtonName_2, customButtonValue_2, customButtonValueBackground_2);
                        _ShowControls(false, 2, customButtonName_3, customButtonValue_3, customButtonValueBackground_3);
                        _ShowControls(false, 3, customButtonName_4, customButtonValue_4, customButtonValueBackground_4);
                        _ShowControls(false, 4, customButtonName_5, customButtonValue_5, customButtonValueBackground_5);
                        _ShowControls(false, 5, customButtonName_6, customButtonValue_6, customButtonValueBackground_6);
                        _ShowControls(false, 6, customButtonName_7, customButtonValue_7, customButtonValueBackground_7);
                        _ShowControls(false, 7, customButtonName_8, customButtonValue_8, customButtonValueBackground_8);
                        _ShowControls(false, 8, customButtonName_9, customButtonValue_9, customButtonValueBackground_9);
                        _ShowControls(false, 9, customButtonName_10, customButtonValue_10, customButtonValueBackground_10);
                        _ShowControls(false, 10, customButtonName_11, customButtonValue_11, customButtonValueBackground_11);
                        _ShowControls(false, 11, customButtonName_12, customButtonValue_12, customButtonValueBackground_12);
                        _ShowControls(false, 12, customButtonName_13, customButtonValue_13, customButtonValueBackground_13);
                        _ShowControls(false, 13, customButtonName_14, customButtonValue_14, customButtonValueBackground_14);
                        _ShowControls(false, 14, customButtonName_15, customButtonValue_15, customButtonValueBackground_15);
                        _ShowControls(false, 15, customButtonName_16, customButtonValue_16, customButtonValueBackground_16);
                        _ShowControls(false, 16, customButtonName_17, customButtonValue_17, customButtonValueBackground_17);
                        _ShowControls(false, 17, customButtonName_18, customButtonValue_18, customButtonValueBackground_18);
                        _ShowControls(false, 18, customButtonName_19, customButtonValue_19, customButtonValueBackground_19);
                        _ShowControls(false, 19, customButtonName_20, customButtonValue_20, customButtonValueBackground_20);
                        _ShowControls(true, 20, customButtonName_21, customButtonValue_21, customButtonValueBackground_21);
                        _ShowControls(false, 21, customButtonName_22, customButtonValue_22, customButtonValueBackground_22);
                        _ShowControls(false, 22, customButtonName_23, customButtonValue_23, customButtonValueBackground_23);
                        _ShowControls(false, 23, customButtonName_24, customButtonValue_24, customButtonValueBackground_24);
                        _ShowControls(false, 24, customButtonName_25, customButtonValue_25, customButtonValueBackground_25);
                        _ShowControls(false, 25, customButtonName_26, customButtonValue_26, customButtonValueBackground_26);
                        _ShowControls(false, 26, customButtonName_27, customButtonValue_27, customButtonValueBackground_27);
                        _ShowControls(false, 27, customButtonName_28, customButtonValue_28, customButtonValueBackground_28);
                        _ShowControls(false, 28, customButtonName_29, customButtonValue_29, customButtonValueBackground_29);
                        _ShowControls(false, 29, customButtonName_30, customButtonValue_30, customButtonValueBackground_30);

                        break;

                    case 2:
                        _ShowControls(false, 0, customButtonName_1, customButtonValue_1, customButtonValueBackground_1);
                        _ShowControls(false, 1, customButtonName_2, customButtonValue_2, customButtonValueBackground_2);
                        _ShowControls(false, 2, customButtonName_3, customButtonValue_3, customButtonValueBackground_3);
                        _ShowControls(false, 3, customButtonName_4, customButtonValue_4, customButtonValueBackground_4);
                        _ShowControls(false, 4, customButtonName_5, customButtonValue_5, customButtonValueBackground_5);
                        _ShowControls(false, 5, customButtonName_6, customButtonValue_6, customButtonValueBackground_6);
                        _ShowControls(false, 6, customButtonName_7, customButtonValue_7, customButtonValueBackground_7);
                        _ShowControls(false, 7, customButtonName_8, customButtonValue_8, customButtonValueBackground_8);
                        _ShowControls(false, 8, customButtonName_9, customButtonValue_9, customButtonValueBackground_9);
                        _ShowControls(false, 9, customButtonName_10, customButtonValue_10, customButtonValueBackground_10);
                        _ShowControls(false, 10, customButtonName_11, customButtonValue_11, customButtonValueBackground_11);
                        _ShowControls(false, 11, customButtonName_12, customButtonValue_12, customButtonValueBackground_12);
                        _ShowControls(false, 12, customButtonName_13, customButtonValue_13, customButtonValueBackground_13);
                        _ShowControls(false, 13, customButtonName_14, customButtonValue_14, customButtonValueBackground_14);
                        _ShowControls(false, 14, customButtonName_15, customButtonValue_15, customButtonValueBackground_15);
                        _ShowControls(false, 15, customButtonName_16, customButtonValue_16, customButtonValueBackground_16);
                        _ShowControls(false, 16, customButtonName_17, customButtonValue_17, customButtonValueBackground_17);
                        _ShowControls(false, 17, customButtonName_18, customButtonValue_18, customButtonValueBackground_18);
                        _ShowControls(false, 18, customButtonName_19, customButtonValue_19, customButtonValueBackground_19);
                        _ShowControls(false, 19, customButtonName_20, customButtonValue_20, customButtonValueBackground_20);
                        _ShowControls(true, 20, customButtonName_21, customButtonValue_21, customButtonValueBackground_21);
                        _ShowControls(true, 21, customButtonName_22, customButtonValue_22, customButtonValueBackground_22);
                        _ShowControls(false, 22, customButtonName_23, customButtonValue_23, customButtonValueBackground_23);
                        _ShowControls(false, 23, customButtonName_24, customButtonValue_24, customButtonValueBackground_24);
                        _ShowControls(false, 24, customButtonName_25, customButtonValue_25, customButtonValueBackground_25);
                        _ShowControls(false, 25, customButtonName_26, customButtonValue_26, customButtonValueBackground_26);
                        _ShowControls(false, 26, customButtonName_27, customButtonValue_27, customButtonValueBackground_27);
                        _ShowControls(false, 27, customButtonName_28, customButtonValue_28, customButtonValueBackground_28);
                        _ShowControls(false, 28, customButtonName_29, customButtonValue_29, customButtonValueBackground_29);
                        _ShowControls(false, 29, customButtonName_30, customButtonValue_30, customButtonValueBackground_30);

                        break;

                    case 3:
                        _ShowControls(false, 0, customButtonName_1, customButtonValue_1, customButtonValueBackground_1);
                        _ShowControls(false, 1, customButtonName_2, customButtonValue_2, customButtonValueBackground_2);
                        _ShowControls(false, 2, customButtonName_3, customButtonValue_3, customButtonValueBackground_3);
                        _ShowControls(false, 3, customButtonName_4, customButtonValue_4, customButtonValueBackground_4);
                        _ShowControls(false, 4, customButtonName_5, customButtonValue_5, customButtonValueBackground_5);
                        _ShowControls(false, 5, customButtonName_6, customButtonValue_6, customButtonValueBackground_6);
                        _ShowControls(false, 6, customButtonName_7, customButtonValue_7, customButtonValueBackground_7);
                        _ShowControls(false, 7, customButtonName_8, customButtonValue_8, customButtonValueBackground_8);
                        _ShowControls(false, 8, customButtonName_9, customButtonValue_9, customButtonValueBackground_9);
                        _ShowControls(false, 9, customButtonName_10, customButtonValue_10, customButtonValueBackground_10);
                        _ShowControls(false, 10, customButtonName_11, customButtonValue_11, customButtonValueBackground_11);
                        _ShowControls(false, 11, customButtonName_12, customButtonValue_12, customButtonValueBackground_12);
                        _ShowControls(false, 12, customButtonName_13, customButtonValue_13, customButtonValueBackground_13);
                        _ShowControls(false, 13, customButtonName_14, customButtonValue_14, customButtonValueBackground_14);
                        _ShowControls(false, 14, customButtonName_15, customButtonValue_15, customButtonValueBackground_15);
                        _ShowControls(false, 15, customButtonName_16, customButtonValue_16, customButtonValueBackground_16);
                        _ShowControls(false, 16, customButtonName_17, customButtonValue_17, customButtonValueBackground_17);
                        _ShowControls(false, 17, customButtonName_18, customButtonValue_18, customButtonValueBackground_18);
                        _ShowControls(false, 18, customButtonName_19, customButtonValue_19, customButtonValueBackground_19);
                        _ShowControls(false, 19, customButtonName_20, customButtonValue_20, customButtonValueBackground_20);
                        _ShowControls(true, 20, customButtonName_21, customButtonValue_21, customButtonValueBackground_21);
                        _ShowControls(true, 21, customButtonName_22, customButtonValue_22, customButtonValueBackground_22);
                        _ShowControls(true, 22, customButtonName_23, customButtonValue_23, customButtonValueBackground_23);
                        _ShowControls(false, 23, customButtonName_24, customButtonValue_24, customButtonValueBackground_24);
                        _ShowControls(false, 24, customButtonName_25, customButtonValue_25, customButtonValueBackground_25);
                        _ShowControls(false, 25, customButtonName_26, customButtonValue_26, customButtonValueBackground_26);
                        _ShowControls(false, 26, customButtonName_27, customButtonValue_27, customButtonValueBackground_27);
                        _ShowControls(false, 27, customButtonName_28, customButtonValue_28, customButtonValueBackground_28);
                        _ShowControls(false, 28, customButtonName_29, customButtonValue_29, customButtonValueBackground_29);
                        _ShowControls(false, 29, customButtonName_30, customButtonValue_30, customButtonValueBackground_30);

                        break;

                    case 4:
                        _ShowControls(false, 0, customButtonName_1, customButtonValue_1, customButtonValueBackground_1);
                        _ShowControls(false, 1, customButtonName_2, customButtonValue_2, customButtonValueBackground_2);
                        _ShowControls(false, 2, customButtonName_3, customButtonValue_3, customButtonValueBackground_3);
                        _ShowControls(false, 3, customButtonName_4, customButtonValue_4, customButtonValueBackground_4);
                        _ShowControls(false, 4, customButtonName_5, customButtonValue_5, customButtonValueBackground_5);
                        _ShowControls(false, 5, customButtonName_6, customButtonValue_6, customButtonValueBackground_6);
                        _ShowControls(false, 6, customButtonName_7, customButtonValue_7, customButtonValueBackground_7);
                        _ShowControls(false, 7, customButtonName_8, customButtonValue_8, customButtonValueBackground_8);
                        _ShowControls(false, 8, customButtonName_9, customButtonValue_9, customButtonValueBackground_9);
                        _ShowControls(false, 9, customButtonName_10, customButtonValue_10, customButtonValueBackground_10);
                        _ShowControls(false, 10, customButtonName_11, customButtonValue_11, customButtonValueBackground_11);
                        _ShowControls(false, 11, customButtonName_12, customButtonValue_12, customButtonValueBackground_12);
                        _ShowControls(false, 12, customButtonName_13, customButtonValue_13, customButtonValueBackground_13);
                        _ShowControls(false, 13, customButtonName_14, customButtonValue_14, customButtonValueBackground_14);
                        _ShowControls(false, 14, customButtonName_15, customButtonValue_15, customButtonValueBackground_15);
                        _ShowControls(false, 15, customButtonName_16, customButtonValue_16, customButtonValueBackground_16);
                        _ShowControls(false, 16, customButtonName_17, customButtonValue_17, customButtonValueBackground_17);
                        _ShowControls(false, 17, customButtonName_18, customButtonValue_18, customButtonValueBackground_18);
                        _ShowControls(false, 18, customButtonName_19, customButtonValue_19, customButtonValueBackground_19);
                        _ShowControls(false, 19, customButtonName_20, customButtonValue_20, customButtonValueBackground_20);
                        _ShowControls(true, 20, customButtonName_21, customButtonValue_21, customButtonValueBackground_21);
                        _ShowControls(true, 21, customButtonName_22, customButtonValue_22, customButtonValueBackground_22);
                        _ShowControls(true, 22, customButtonName_23, customButtonValue_23, customButtonValueBackground_23);
                        _ShowControls(true, 23, customButtonName_24, customButtonValue_24, customButtonValueBackground_24);
                        _ShowControls(false, 24, customButtonName_25, customButtonValue_25, customButtonValueBackground_25);
                        _ShowControls(false, 25, customButtonName_26, customButtonValue_26, customButtonValueBackground_26);
                        _ShowControls(false, 26, customButtonName_27, customButtonValue_27, customButtonValueBackground_27);
                        _ShowControls(false, 27, customButtonName_28, customButtonValue_28, customButtonValueBackground_28);
                        _ShowControls(false, 28, customButtonName_29, customButtonValue_29, customButtonValueBackground_29);
                        _ShowControls(false, 29, customButtonName_30, customButtonValue_30, customButtonValueBackground_30);

                        break;

                    case 5:
                        _ShowControls(false, 0, customButtonName_1, customButtonValue_1, customButtonValueBackground_1);
                        _ShowControls(false, 1, customButtonName_2, customButtonValue_2, customButtonValueBackground_2);
                        _ShowControls(false, 2, customButtonName_3, customButtonValue_3, customButtonValueBackground_3);
                        _ShowControls(false, 3, customButtonName_4, customButtonValue_4, customButtonValueBackground_4);
                        _ShowControls(false, 4, customButtonName_5, customButtonValue_5, customButtonValueBackground_5);
                        _ShowControls(false, 5, customButtonName_6, customButtonValue_6, customButtonValueBackground_6);
                        _ShowControls(false, 6, customButtonName_7, customButtonValue_7, customButtonValueBackground_7);
                        _ShowControls(false, 7, customButtonName_8, customButtonValue_8, customButtonValueBackground_8);
                        _ShowControls(false, 8, customButtonName_9, customButtonValue_9, customButtonValueBackground_9);
                        _ShowControls(false, 9, customButtonName_10, customButtonValue_10, customButtonValueBackground_10);
                        _ShowControls(false, 10, customButtonName_11, customButtonValue_11, customButtonValueBackground_11);
                        _ShowControls(false, 11, customButtonName_12, customButtonValue_12, customButtonValueBackground_12);
                        _ShowControls(false, 12, customButtonName_13, customButtonValue_13, customButtonValueBackground_13);
                        _ShowControls(false, 13, customButtonName_14, customButtonValue_14, customButtonValueBackground_14);
                        _ShowControls(false, 14, customButtonName_15, customButtonValue_15, customButtonValueBackground_15);
                        _ShowControls(false, 15, customButtonName_16, customButtonValue_16, customButtonValueBackground_16);
                        _ShowControls(false, 16, customButtonName_17, customButtonValue_17, customButtonValueBackground_17);
                        _ShowControls(false, 17, customButtonName_18, customButtonValue_18, customButtonValueBackground_18);
                        _ShowControls(false, 18, customButtonName_19, customButtonValue_19, customButtonValueBackground_19);
                        _ShowControls(false, 19, customButtonName_20, customButtonValue_20, customButtonValueBackground_20);
                        _ShowControls(true, 20, customButtonName_21, customButtonValue_21, customButtonValueBackground_21);
                        _ShowControls(true, 21, customButtonName_22, customButtonValue_22, customButtonValueBackground_22);
                        _ShowControls(true, 22, customButtonName_23, customButtonValue_23, customButtonValueBackground_23);
                        _ShowControls(true, 23, customButtonName_24, customButtonValue_24, customButtonValueBackground_24);
                        _ShowControls(true, 24, customButtonName_25, customButtonValue_25, customButtonValueBackground_25);
                        _ShowControls(false, 25, customButtonName_26, customButtonValue_26, customButtonValueBackground_26);
                        _ShowControls(false, 26, customButtonName_27, customButtonValue_27, customButtonValueBackground_27);
                        _ShowControls(false, 27, customButtonName_28, customButtonValue_28, customButtonValueBackground_28);
                        _ShowControls(false, 28, customButtonName_29, customButtonValue_29, customButtonValueBackground_29);
                        _ShowControls(false, 29, customButtonName_30, customButtonValue_30, customButtonValueBackground_30);

                        break;

                    case 6:
                        _ShowControls(false, 0, customButtonName_1, customButtonValue_1, customButtonValueBackground_1);
                        _ShowControls(false, 1, customButtonName_2, customButtonValue_2, customButtonValueBackground_2);
                        _ShowControls(false, 2, customButtonName_3, customButtonValue_3, customButtonValueBackground_3);
                        _ShowControls(false, 3, customButtonName_4, customButtonValue_4, customButtonValueBackground_4);
                        _ShowControls(false, 4, customButtonName_5, customButtonValue_5, customButtonValueBackground_5);
                        _ShowControls(false, 5, customButtonName_6, customButtonValue_6, customButtonValueBackground_6);
                        _ShowControls(false, 6, customButtonName_7, customButtonValue_7, customButtonValueBackground_7);
                        _ShowControls(false, 7, customButtonName_8, customButtonValue_8, customButtonValueBackground_8);
                        _ShowControls(false, 8, customButtonName_9, customButtonValue_9, customButtonValueBackground_9);
                        _ShowControls(false, 9, customButtonName_10, customButtonValue_10, customButtonValueBackground_10);
                        _ShowControls(false, 10, customButtonName_11, customButtonValue_11, customButtonValueBackground_11);
                        _ShowControls(false, 11, customButtonName_12, customButtonValue_12, customButtonValueBackground_12);
                        _ShowControls(false, 12, customButtonName_13, customButtonValue_13, customButtonValueBackground_13);
                        _ShowControls(false, 13, customButtonName_14, customButtonValue_14, customButtonValueBackground_14);
                        _ShowControls(false, 14, customButtonName_15, customButtonValue_15, customButtonValueBackground_15);
                        _ShowControls(false, 15, customButtonName_16, customButtonValue_16, customButtonValueBackground_16);
                        _ShowControls(false, 16, customButtonName_17, customButtonValue_17, customButtonValueBackground_17);
                        _ShowControls(false, 17, customButtonName_18, customButtonValue_18, customButtonValueBackground_18);
                        _ShowControls(false, 18, customButtonName_19, customButtonValue_19, customButtonValueBackground_19);
                        _ShowControls(false, 19, customButtonName_20, customButtonValue_20, customButtonValueBackground_20);
                        _ShowControls(true, 20, customButtonName_21, customButtonValue_21, customButtonValueBackground_21);
                        _ShowControls(true, 21, customButtonName_22, customButtonValue_22, customButtonValueBackground_22);
                        _ShowControls(true, 22, customButtonName_23, customButtonValue_23, customButtonValueBackground_23);
                        _ShowControls(true, 23, customButtonName_24, customButtonValue_24, customButtonValueBackground_24);
                        _ShowControls(true, 24, customButtonName_25, customButtonValue_25, customButtonValueBackground_25);
                        _ShowControls(true, 25, customButtonName_26, customButtonValue_26, customButtonValueBackground_26);
                        _ShowControls(false, 26, customButtonName_27, customButtonValue_27, customButtonValueBackground_27);
                        _ShowControls(false, 27, customButtonName_28, customButtonValue_28, customButtonValueBackground_28);
                        _ShowControls(false, 28, customButtonName_29, customButtonValue_29, customButtonValueBackground_29);
                        _ShowControls(false, 29, customButtonName_30, customButtonValue_30, customButtonValueBackground_30);

                        break;

                    case 7:
                        _ShowControls(false, 0, customButtonName_1, customButtonValue_1, customButtonValueBackground_1);
                        _ShowControls(false, 1, customButtonName_2, customButtonValue_2, customButtonValueBackground_2);
                        _ShowControls(false, 2, customButtonName_3, customButtonValue_3, customButtonValueBackground_3);
                        _ShowControls(false, 3, customButtonName_4, customButtonValue_4, customButtonValueBackground_4);
                        _ShowControls(false, 4, customButtonName_5, customButtonValue_5, customButtonValueBackground_5);
                        _ShowControls(false, 5, customButtonName_6, customButtonValue_6, customButtonValueBackground_6);
                        _ShowControls(false, 6, customButtonName_7, customButtonValue_7, customButtonValueBackground_7);
                        _ShowControls(false, 7, customButtonName_8, customButtonValue_8, customButtonValueBackground_8);
                        _ShowControls(false, 8, customButtonName_9, customButtonValue_9, customButtonValueBackground_9);
                        _ShowControls(false, 9, customButtonName_10, customButtonValue_10, customButtonValueBackground_10);
                        _ShowControls(false, 10, customButtonName_11, customButtonValue_11, customButtonValueBackground_11);
                        _ShowControls(false, 11, customButtonName_12, customButtonValue_12, customButtonValueBackground_12);
                        _ShowControls(false, 12, customButtonName_13, customButtonValue_13, customButtonValueBackground_13);
                        _ShowControls(false, 13, customButtonName_14, customButtonValue_14, customButtonValueBackground_14);
                        _ShowControls(false, 14, customButtonName_15, customButtonValue_15, customButtonValueBackground_15);
                        _ShowControls(false, 15, customButtonName_16, customButtonValue_16, customButtonValueBackground_16);
                        _ShowControls(false, 16, customButtonName_17, customButtonValue_17, customButtonValueBackground_17);
                        _ShowControls(false, 17, customButtonName_18, customButtonValue_18, customButtonValueBackground_18);
                        _ShowControls(false, 18, customButtonName_19, customButtonValue_19, customButtonValueBackground_19);
                        _ShowControls(false, 19, customButtonName_20, customButtonValue_20, customButtonValueBackground_20);
                        _ShowControls(true, 20, customButtonName_21, customButtonValue_21, customButtonValueBackground_21);
                        _ShowControls(true, 21, customButtonName_22, customButtonValue_22, customButtonValueBackground_22);
                        _ShowControls(true, 22, customButtonName_23, customButtonValue_23, customButtonValueBackground_23);
                        _ShowControls(true, 23, customButtonName_24, customButtonValue_24, customButtonValueBackground_24);
                        _ShowControls(true, 24, customButtonName_25, customButtonValue_25, customButtonValueBackground_25);
                        _ShowControls(true, 25, customButtonName_26, customButtonValue_26, customButtonValueBackground_26);
                        _ShowControls(true, 26, customButtonName_27, customButtonValue_27, customButtonValueBackground_27);
                        _ShowControls(false, 27, customButtonName_28, customButtonValue_28, customButtonValueBackground_28);
                        _ShowControls(false, 28, customButtonName_29, customButtonValue_29, customButtonValueBackground_29);
                        _ShowControls(false, 29, customButtonName_30, customButtonValue_30, customButtonValueBackground_30);

                        break;

                    case 8:
                        _ShowControls(false, 0, customButtonName_1, customButtonValue_1, customButtonValueBackground_1);
                        _ShowControls(false, 1, customButtonName_2, customButtonValue_2, customButtonValueBackground_2);
                        _ShowControls(false, 2, customButtonName_3, customButtonValue_3, customButtonValueBackground_3);
                        _ShowControls(false, 3, customButtonName_4, customButtonValue_4, customButtonValueBackground_4);
                        _ShowControls(false, 4, customButtonName_5, customButtonValue_5, customButtonValueBackground_5);
                        _ShowControls(false, 5, customButtonName_6, customButtonValue_6, customButtonValueBackground_6);
                        _ShowControls(false, 6, customButtonName_7, customButtonValue_7, customButtonValueBackground_7);
                        _ShowControls(false, 7, customButtonName_8, customButtonValue_8, customButtonValueBackground_8);
                        _ShowControls(false, 8, customButtonName_9, customButtonValue_9, customButtonValueBackground_9);
                        _ShowControls(false, 9, customButtonName_10, customButtonValue_10, customButtonValueBackground_10);
                        _ShowControls(false, 10, customButtonName_11, customButtonValue_11, customButtonValueBackground_11);
                        _ShowControls(false, 11, customButtonName_12, customButtonValue_12, customButtonValueBackground_12);
                        _ShowControls(false, 12, customButtonName_13, customButtonValue_13, customButtonValueBackground_13);
                        _ShowControls(false, 13, customButtonName_14, customButtonValue_14, customButtonValueBackground_14);
                        _ShowControls(false, 14, customButtonName_15, customButtonValue_15, customButtonValueBackground_15);
                        _ShowControls(false, 15, customButtonName_16, customButtonValue_16, customButtonValueBackground_16);
                        _ShowControls(false, 16, customButtonName_17, customButtonValue_17, customButtonValueBackground_17);
                        _ShowControls(false, 17, customButtonName_18, customButtonValue_18, customButtonValueBackground_18);
                        _ShowControls(false, 18, customButtonName_19, customButtonValue_19, customButtonValueBackground_19);
                        _ShowControls(false, 19, customButtonName_20, customButtonValue_20, customButtonValueBackground_20);
                        _ShowControls(true, 20, customButtonName_21, customButtonValue_21, customButtonValueBackground_21);
                        _ShowControls(true, 21, customButtonName_22, customButtonValue_22, customButtonValueBackground_22);
                        _ShowControls(true, 22, customButtonName_23, customButtonValue_23, customButtonValueBackground_23);
                        _ShowControls(true, 23, customButtonName_24, customButtonValue_24, customButtonValueBackground_24);
                        _ShowControls(true, 24, customButtonName_25, customButtonValue_25, customButtonValueBackground_25);
                        _ShowControls(true, 25, customButtonName_26, customButtonValue_26, customButtonValueBackground_26);
                        _ShowControls(true, 26, customButtonName_27, customButtonValue_27, customButtonValueBackground_27);
                        _ShowControls(true, 27, customButtonName_28, customButtonValue_28, customButtonValueBackground_28);
                        _ShowControls(false, 28, customButtonName_29, customButtonValue_29, customButtonValueBackground_29);
                        _ShowControls(false, 29, customButtonName_30, customButtonValue_30, customButtonValueBackground_30);

                        break;

                    case 9:
                        _ShowControls(false, 0, customButtonName_1, customButtonValue_1, customButtonValueBackground_1);
                        _ShowControls(false, 1, customButtonName_2, customButtonValue_2, customButtonValueBackground_2);
                        _ShowControls(false, 2, customButtonName_3, customButtonValue_3, customButtonValueBackground_3);
                        _ShowControls(false, 3, customButtonName_4, customButtonValue_4, customButtonValueBackground_4);
                        _ShowControls(false, 4, customButtonName_5, customButtonValue_5, customButtonValueBackground_5);
                        _ShowControls(false, 5, customButtonName_6, customButtonValue_6, customButtonValueBackground_6);
                        _ShowControls(false, 6, customButtonName_7, customButtonValue_7, customButtonValueBackground_7);
                        _ShowControls(false, 7, customButtonName_8, customButtonValue_8, customButtonValueBackground_8);
                        _ShowControls(false, 8, customButtonName_9, customButtonValue_9, customButtonValueBackground_9);
                        _ShowControls(false, 9, customButtonName_10, customButtonValue_10, customButtonValueBackground_10);
                        _ShowControls(false, 10, customButtonName_11, customButtonValue_11, customButtonValueBackground_11);
                        _ShowControls(false, 11, customButtonName_12, customButtonValue_12, customButtonValueBackground_12);
                        _ShowControls(false, 12, customButtonName_13, customButtonValue_13, customButtonValueBackground_13);
                        _ShowControls(false, 13, customButtonName_14, customButtonValue_14, customButtonValueBackground_14);
                        _ShowControls(false, 14, customButtonName_15, customButtonValue_15, customButtonValueBackground_15);
                        _ShowControls(false, 15, customButtonName_16, customButtonValue_16, customButtonValueBackground_16);
                        _ShowControls(false, 16, customButtonName_17, customButtonValue_17, customButtonValueBackground_17);
                        _ShowControls(false, 17, customButtonName_18, customButtonValue_18, customButtonValueBackground_18);
                        _ShowControls(false, 18, customButtonName_19, customButtonValue_19, customButtonValueBackground_19);
                        _ShowControls(false, 19, customButtonName_20, customButtonValue_20, customButtonValueBackground_20);
                        _ShowControls(true, 20, customButtonName_21, customButtonValue_21, customButtonValueBackground_21);
                        _ShowControls(true, 21, customButtonName_22, customButtonValue_22, customButtonValueBackground_22);
                        _ShowControls(true, 22, customButtonName_23, customButtonValue_23, customButtonValueBackground_23);
                        _ShowControls(true, 23, customButtonName_24, customButtonValue_24, customButtonValueBackground_24);
                        _ShowControls(true, 24, customButtonName_25, customButtonValue_25, customButtonValueBackground_25);
                        _ShowControls(true, 25, customButtonName_26, customButtonValue_26, customButtonValueBackground_26);
                        _ShowControls(true, 26, customButtonName_27, customButtonValue_27, customButtonValueBackground_27);
                        _ShowControls(true, 27, customButtonName_28, customButtonValue_28, customButtonValueBackground_28);
                        _ShowControls(true, 28, customButtonName_29, customButtonValue_29, customButtonValueBackground_29);
                        _ShowControls(false, 29, customButtonName_30, customButtonValue_30, customButtonValueBackground_30);

                        break;

                    case 10:
                        _ShowControls(false, 0, customButtonName_1, customButtonValue_1, customButtonValueBackground_1);
                        _ShowControls(false, 1, customButtonName_2, customButtonValue_2, customButtonValueBackground_2);
                        _ShowControls(false, 2, customButtonName_3, customButtonValue_3, customButtonValueBackground_3);
                        _ShowControls(false, 3, customButtonName_4, customButtonValue_4, customButtonValueBackground_4);
                        _ShowControls(false, 4, customButtonName_5, customButtonValue_5, customButtonValueBackground_5);
                        _ShowControls(false, 5, customButtonName_6, customButtonValue_6, customButtonValueBackground_6);
                        _ShowControls(false, 6, customButtonName_7, customButtonValue_7, customButtonValueBackground_7);
                        _ShowControls(false, 7, customButtonName_8, customButtonValue_8, customButtonValueBackground_8);
                        _ShowControls(false, 8, customButtonName_9, customButtonValue_9, customButtonValueBackground_9);
                        _ShowControls(false, 9, customButtonName_10, customButtonValue_10, customButtonValueBackground_10);
                        _ShowControls(false, 10, customButtonName_11, customButtonValue_11, customButtonValueBackground_11);
                        _ShowControls(false, 11, customButtonName_12, customButtonValue_12, customButtonValueBackground_12);
                        _ShowControls(false, 12, customButtonName_13, customButtonValue_13, customButtonValueBackground_13);
                        _ShowControls(false, 13, customButtonName_14, customButtonValue_14, customButtonValueBackground_14);
                        _ShowControls(false, 14, customButtonName_15, customButtonValue_15, customButtonValueBackground_15);
                        _ShowControls(false, 15, customButtonName_16, customButtonValue_16, customButtonValueBackground_16);
                        _ShowControls(false, 16, customButtonName_17, customButtonValue_17, customButtonValueBackground_17);
                        _ShowControls(false, 17, customButtonName_18, customButtonValue_18, customButtonValueBackground_18);
                        _ShowControls(false, 18, customButtonName_19, customButtonValue_19, customButtonValueBackground_19);
                        _ShowControls(false, 19, customButtonName_20, customButtonValue_20, customButtonValueBackground_20);
                        _ShowControls(true, 20, customButtonName_21, customButtonValue_21, customButtonValueBackground_21);
                        _ShowControls(true, 21, customButtonName_22, customButtonValue_22, customButtonValueBackground_22);
                        _ShowControls(true, 22, customButtonName_23, customButtonValue_23, customButtonValueBackground_23);
                        _ShowControls(true, 23, customButtonName_24, customButtonValue_24, customButtonValueBackground_24);
                        _ShowControls(true, 24, customButtonName_25, customButtonValue_25, customButtonValueBackground_25);
                        _ShowControls(true, 25, customButtonName_26, customButtonValue_26, customButtonValueBackground_26);
                        _ShowControls(true, 26, customButtonName_27, customButtonValue_27, customButtonValueBackground_27);
                        _ShowControls(true, 27, customButtonName_28, customButtonValue_28, customButtonValueBackground_28);
                        _ShowControls(true, 28, customButtonName_29, customButtonValue_29, customButtonValueBackground_29);
                        _ShowControls(true, 29, customButtonName_30, customButtonValue_30, customButtonValueBackground_30);

                        break;

                    default:
                        _ShowControls(false, 0, customButtonName_1, customButtonValue_1, customButtonValueBackground_1);
                        _ShowControls(false, 1, customButtonName_2, customButtonValue_2, customButtonValueBackground_2);
                        _ShowControls(false, 2, customButtonName_3, customButtonValue_3, customButtonValueBackground_3);
                        _ShowControls(false, 3, customButtonName_4, customButtonValue_4, customButtonValueBackground_4);
                        _ShowControls(false, 4, customButtonName_5, customButtonValue_5, customButtonValueBackground_5);
                        _ShowControls(false, 5, customButtonName_6, customButtonValue_6, customButtonValueBackground_6);
                        _ShowControls(false, 6, customButtonName_7, customButtonValue_7, customButtonValueBackground_7);
                        _ShowControls(false, 7, customButtonName_8, customButtonValue_8, customButtonValueBackground_8);
                        _ShowControls(false, 8, customButtonName_9, customButtonValue_9, customButtonValueBackground_9);
                        _ShowControls(false, 9, customButtonName_10, customButtonValue_10, customButtonValueBackground_10);
                        _ShowControls(false, 10, customButtonName_11, customButtonValue_11, customButtonValueBackground_11);
                        _ShowControls(false, 11, customButtonName_12, customButtonValue_12, customButtonValueBackground_12);
                        _ShowControls(false, 12, customButtonName_13, customButtonValue_13, customButtonValueBackground_13);
                        _ShowControls(false, 13, customButtonName_14, customButtonValue_14, customButtonValueBackground_14);
                        _ShowControls(false, 14, customButtonName_15, customButtonValue_15, customButtonValueBackground_15);
                        _ShowControls(false, 15, customButtonName_16, customButtonValue_16, customButtonValueBackground_16);
                        _ShowControls(false, 16, customButtonName_17, customButtonValue_17, customButtonValueBackground_17);
                        _ShowControls(false, 17, customButtonName_18, customButtonValue_18, customButtonValueBackground_18);
                        _ShowControls(false, 18, customButtonName_19, customButtonValue_19, customButtonValueBackground_19);
                        _ShowControls(false, 19, customButtonName_20, customButtonValue_20, customButtonValueBackground_20);
                        _ShowControls(false, 20, customButtonName_21, customButtonValue_21, customButtonValueBackground_21);
                        _ShowControls(false, 21, customButtonName_22, customButtonValue_22, customButtonValueBackground_22);
                        _ShowControls(false, 22, customButtonName_23, customButtonValue_23, customButtonValueBackground_23);
                        _ShowControls(false, 23, customButtonName_24, customButtonValue_24, customButtonValueBackground_24);
                        _ShowControls(false, 24, customButtonName_25, customButtonValue_25, customButtonValueBackground_25);
                        _ShowControls(false, 25, customButtonName_26, customButtonValue_26, customButtonValueBackground_26);
                        _ShowControls(false, 26, customButtonName_27, customButtonValue_27, customButtonValueBackground_27);
                        _ShowControls(false, 27, customButtonName_28, customButtonValue_28, customButtonValueBackground_28);
                        _ShowControls(false, 28, customButtonName_29, customButtonValue_29, customButtonValueBackground_29);
                        _ShowControls(false, 29, customButtonName_30, customButtonValue_30, customButtonValueBackground_30);

                        break;
                }
            }
        }
    }
}