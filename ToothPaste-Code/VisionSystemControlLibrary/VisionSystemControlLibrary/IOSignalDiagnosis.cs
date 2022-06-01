/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：IOSignalDiagnosis.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：I/O TEST控件

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

using System.Reflection;

namespace VisionSystemControlLibrary
{
    public partial class IOSignalDiagnosis : UserControl
    {
        //I/O TEST控件

        private VisionSystemClassLibrary.Enum.InterfaceLanguage language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;//属性，语言

        private String[] sSelectedDeviceName = new String[2];//属性，选择的设备名称

        VisionSystemClassLibrary.Class.System system = new VisionSystemClassLibrary.Class.System();

        //private VisionSystemClassLibrary.Struct.IOSignal iosignal = new VisionSystemClassLibrary.Struct.IOSignal();//I/O

        //

        private Boolean[] bSwitchState = new Boolean[32];//开关状态。取值范围：true，打开；false，关闭

        //

        private String[][] sMessageText = new String[2][];//标题控件上显示的文本（[语言][包含的文本]）

        VisionSystemClassLibrary.Class.System fff = new VisionSystemClassLibrary.Class.System();

        //

        [Browsable(true), Description("点击【开关】按钮时产生的事件"), Category("IOSignalDiagnosis 事件")]
        public event EventHandler Switch_Click;

        [Browsable(true), Description("点击【关闭】按钮时产生的事件"), Category("IOSignalDiagnosis 事件")]
        public event EventHandler Close_Click;

        //构造函数

        //----------------------------------------------------------------------
        // 功能说明：构造函数，初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public IOSignalDiagnosis()
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
                    sMessageText[i] = new String[1];
                }

                //

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] = customButtonCaption.Chinese_TextDisplay[0];
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] = customButtonCaption.English_TextDisplay[0];
            }

            //
            
            bSwitchState = new Boolean[BitConverter.GetBytes(system.IOSignalData.OutputDiagStateLab).Length * 8];

            for (i = 0; i < bSwitchState.Length; i++)
            {
                bSwitchState[i] = false;
            }
        }

        //属性

        //-----------------------------------------------------------------------
        // 功能说明：Language属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("语言"), Category("IOSignalDiagnosis 通用")]
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
        // 功能说明：Chinese_SelectedDeviceName属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("选择的设备中文名称）"), Category("IOSignalDiagnosis 通用")]
        public String Chinese_SelectedDeviceName//属性
        {
            get//读取
            {
                return sSelectedDeviceName[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1];
            }
            set//设置
            {
                if (value != sSelectedDeviceName[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1])//设置了新的数值
                {
                    sSelectedDeviceName[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1] = value;

                    //

                    customButtonCaption.Chinese_TextDisplay = new String[1] { value + " - " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] };//设置显示的文本
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：English_SelectedDeviceName属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("选择的设备英文名称）"), Category("IOSignalDiagnosis 通用")]
        public String English_SelectedDeviceName//属性
        {
            get//读取
            {
                return sSelectedDeviceName[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1];
            }
            set//设置
            {
                if (value != sSelectedDeviceName[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1])//设置了新的数值
                {
                    sSelectedDeviceName[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1] = value;

                    //

                    customButtonCaption.English_TextDisplay = new String[1] { value + " - " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] };//设置显示的文本
                }
            }
        }

        //函数

        //----------------------------------------------------------------------
        // 功能说明：更新页面
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _Properties(VisionSystemClassLibrary.Class.System system_parameter)
        {
            system = system_parameter;
        }

        //----------------------------------------------------------------------
        // 功能说明：更新页面
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _Update()
        {
            if (0 != (system.IOSignalData.OutputDiagStateLab & 0x00000001))//输出诊断0开启
            {
                customButtonDiagnosis0.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Down;
            }
            else//输出诊断0关闭
            {
                customButtonDiagnosis0.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
            }

            if (0 != (system.IOSignalData.OutputDiagStateLab & 0x00000002))//输出诊断1开启
            {
                customButtonDiagnosis1.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Down;
            }
            else//输出诊断1关闭
            {
                customButtonDiagnosis1.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
            }

            if (0 != (system.IOSignalData.OutputDiagStateLab & 0x00000004))//输出诊断2开启
            {
                customButtonDiagnosis2.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Down;
            }
            else//输出诊断2关闭
            {
                customButtonDiagnosis2.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
            }

            if (0 != (system.IOSignalData.OutputDiagStateLab & 0x00000008))//输出诊断3开启
            {
                customButtonDiagnosis3.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Down;
            }
            else//输出诊断3关闭
            {
                customButtonDiagnosis3.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
            }

            if (0 != (system.IOSignalData.OutputDiagStateLab & 0x00000010))//输出诊断4开启
            {
                customButtonDiagnosis4.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Down;
            }
            else//输出诊断4关闭
            {
                customButtonDiagnosis4.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
            }

            if (0 != (system.IOSignalData.OutputDiagStateLab & 0x00000020))//输出诊断5开启
            {
                customButtonDiagnosis5.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Down;
            }
            else//输出诊断5关闭
            {
                customButtonDiagnosis5.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
            }

            if (0 != (system.IOSignalData.OutputDiagStateLab & 0x00000040))//输出诊断6开启
            {
                customButtonDiagnosis6.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Down;
            }
            else//输出诊断6关闭
            {
                customButtonDiagnosis6.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
            }

            if (0 != (system.IOSignalData.OutputDiagStateLab & 0x00000080))//输出诊断7开启
            {
                customButtonDiagnosis7.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Down;
            }
            else//输出诊断7关闭
            {
                customButtonDiagnosis7.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
            }

            if (0 != (system.IOSignalData.OutputDiagStateLab & 0x00000100))//输出诊断8开启
            {
                customButtonDiagnosis8.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Down;
            }
            else//输出诊断8关闭
            {
                customButtonDiagnosis8.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
            }

            if (0 != (system.IOSignalData.OutputDiagStateLab & 0x00000200))//输出诊断9开启
            {
                customButtonDiagnosis9.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Down;
            }
            else//输出诊断9关闭
            {
                customButtonDiagnosis9.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
            }

            if (0 != (system.IOSignalData.OutputDiagStateLab & 0x00000400))//输出诊断10开启
            {
                customButtonDiagnosis10.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Down;
            }
            else//输出诊断10关闭
            {
                customButtonDiagnosis10.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
            }

            if (0 != (system.IOSignalData.OutputDiagStateLab & 0x00000800))//输出诊断11开启
            {
                customButtonDiagnosis11.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Down;
            }
            else//输出诊断11关闭
            {
                customButtonDiagnosis11.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
            }

            if (0 != (system.IOSignalData.OutputDiagStateLab & 0x00001000))//输出诊断12开启
            {
                customButtonDiagnosis12.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Down;
            }
            else//输出诊断12关闭
            {
                customButtonDiagnosis12.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
            }

            if (0 != (system.IOSignalData.OutputDiagStateLab & 0x00002000))//输出诊断13开启
            {
                customButtonDiagnosis13.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Down;
            }
            else//输出诊断13关闭
            {
                customButtonDiagnosis13.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
            }

            //

            if (0 != (system.IOSignalData.InputState & 0x00000001))//输入状态0开启
            {
                customButtonInput0.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
            }
            else//输入状态0关闭
            {
                customButtonInput0.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
            }

            if (0 != (system.IOSignalData.InputState & 0x00000002))//输入状态1开启
            {
                customButtonInput1.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
            }
            else//输入状态1关闭
            {
                customButtonInput1.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
            }

            if (0 != (system.IOSignalData.InputState & 0x00000004))//输入状态2开启
            {
                customButtonInput2.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
            }
            else//输入状态2关闭
            {
                customButtonInput2.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
            }

            if (0 != (system.IOSignalData.InputState & 0x00000008))//输入状态3开启
            {
                customButtonInput3.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
            }
            else//输入状态3关闭
            {
                customButtonInput3.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
            }

            if (0 != (system.IOSignalData.InputState & 0x00000010))//输入状态4开启
            {
                customButtonInput4.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
            }
            else//输入状态4关闭
            {
                customButtonInput4.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
            }

            if (0 != (system.IOSignalData.InputState & 0x00000020))//输入状态5开启
            {
                customButtonInput5.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
            }
            else//输入状态5关闭
            {
                customButtonInput5.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
            }

            if (0 != (system.IOSignalData.InputState & 0x00000040))//输入状态6开启
            {
                customButtonInput6.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
            }
            else//输入状态6关闭
            {
                customButtonInput6.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
            }

            if (0 != (system.IOSignalData.InputState & 0x00000080))//输入状态7开启
            {
                customButtonInput7.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
            }
            else//输入状态7关闭
            {
                customButtonInput7.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
            }

            if (0 != (system.IOSignalData.InputState & 0x00000100))//输入状态8开启
            {
                customButtonInput8.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
            }
            else//输入状态8关闭
            {
                customButtonInput8.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
            }

            if (0 != (system.IOSignalData.InputState & 0x00000200))//输入状态9开启
            {
                customButtonInput9.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
            }
            else//输入状态9关闭
            {
                customButtonInput9.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
            }

            if (0 != (system.IOSignalData.InputState & 0x00000400))//输入状态10开启
            {
                customButtonInput10.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
            }
            else//输入状态10关闭
            {
                customButtonInput10.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
            }

            if (0 != (system.IOSignalData.InputState & 0x00000800))//输入状态11开启
            {
                customButtonInput11.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
            }
            else//输入状态11关闭
            {
                customButtonInput11.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
            }

            if (0 != (system.IOSignalData.InputState & 0x00001000))//输入状态12开启
            {
                customButtonInput12.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
            }
            else//输入状态12关闭
            {
                customButtonInput12.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
            }

            if (0 != (system.IOSignalData.InputState & 0x00002000))//输入状态13开启
            {
                customButtonInput13.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
            }
            else//输入状态13关闭
            {
                customButtonInput13.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
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

            //

            customButtonInText.Language = language;
            customButtonOutText.Language = language;
            customButtonDiagnosisText.Language = language;

            //

            customButtonClose.Language = language;
        }

        //-----------------------------------------------------------------------
        // 功能说明：恢复控件内容
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _Reset()
        {
            Int32 i = 0;//循环控制变量

            system.IOSignalData.OutputState = 0x00000000;

            for (i = 0; i < bSwitchState.Length; i++)
            {
                bSwitchState[i] = false;
            }

            //

            customButtonInput0.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//Input 0
            customButtonOutput0.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//Output 0
            customButtonDiagnosis0.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//Diagnosis 0
            customButtonSwitch0.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//Switch 0

            customButtonInput1.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//Input 1
            customButtonOutput1.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//Output 1
            customButtonDiagnosis1.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//Diagnosis 1
            customButtonSwitch1.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//Switch 1

            customButtonInput2.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//Input 2
            customButtonOutput2.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//Output 2
            customButtonDiagnosis2.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//Diagnosis 2
            customButtonSwitch2.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//Switch 2

            customButtonInput3.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//Input 3
            customButtonOutput3.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//Output 3
            customButtonDiagnosis3.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//Diagnosis 3
            customButtonSwitch3.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//Switch 3

            customButtonInput4.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//Input 4
            customButtonOutput4.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//Output 4
            customButtonDiagnosis4.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//Diagnosis 4
            customButtonSwitch4.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//Switch 4

            customButtonInput5.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//Input 5
            customButtonOutput5.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//Output 5
            customButtonDiagnosis5.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//Diagnosis 5
            customButtonSwitch5.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//Switch 5

            customButtonInput6.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//Input 6
            customButtonOutput6.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//Output 6
            customButtonDiagnosis6.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//Diagnosis 6
            customButtonSwitch6.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//Switch 6

            customButtonInput7.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//Input 7
            customButtonOutput7.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//Output 7
            customButtonDiagnosis7.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//Diagnosis 7
            customButtonSwitch7.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//Switch 7

            customButtonInput8.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//Input 8
            customButtonOutput8.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//Output 8
            customButtonDiagnosis8.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//Diagnosis 8
            customButtonSwitch8.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//Switch 8

            customButtonInput9.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//Input 9
            customButtonOutput9.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//Output 9
            customButtonDiagnosis9.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//Diagnosis 9
            customButtonSwitch9.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//Switch 9

            customButtonInput10.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//Input 10
            customButtonOutput10.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//Output 10
            customButtonDiagnosis10.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//Diagnosis 10
            customButtonSwitch10.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//Switch 10

            customButtonInput11.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//Input 11
            customButtonOutput11.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//Output 11
            customButtonDiagnosis11.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//Diagnosis 11
            customButtonSwitch11.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//Switch 11

            customButtonInput12.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//Input 12
            customButtonOutput12.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//Output 12
            customButtonDiagnosis12.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//Diagnosis 12
            customButtonSwitch12.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//Switch 12

            customButtonInput13.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//Input 13
            customButtonOutput13.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//Output 13
            customButtonDiagnosis13.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//Diagnosis 13
            customButtonSwitch13.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//Switch 13
        }

        //事件

        //-----------------------------------------------------------------------
        // 功能说明：窗口加载函数
        // 输入参数： sender，Form控件对象；e，Form控件事件响应参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void IOSignalDiagnosis_Load(object sender, EventArgs e)
        {
            //不执行操作
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：相机离线时，关闭窗口
        // 输入参数： state:显示或隐藏，true:显示,false:隐藏
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        public void _Close()
        {
            _Reset();

            //事件

            if (null != Close_Click)//有效
            {
                Close_Click(this, new CustomEventArgs());
            }
        }
        
        //

        //-----------------------------------------------------------------------
        // 功能说明：OUT0按钮状态变化响应函数
        // 输入参数： sender，switchSmall控件对象；e，switchSmall控件事件响应参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonSwitch0_CustomButton_Click(object sender, EventArgs e)
        {
            bSwitchState[0] = !bSwitchState[0];

            if (bSwitchState[0])//输出诊断0开启
            {
                customButtonOutput0.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Down;

                customButtonSwitch0.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;

                //

                system.IOSignalData.OutputState = system.IOSignalData.OutputState | 0x00000001;
            }
            else//输出诊断0关闭
            {
                customButtonOutput0.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;

                customButtonSwitch0.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;

                //

                system.IOSignalData.OutputState = system.IOSignalData.OutputState & 0xFFFFFFFE;
            }

            //

            if (null != Switch_Click)//有效
            {
                Switch_Click(this, e);
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：OUT1按钮状态变化响应函数
        // 输入参数： sender，switchSmall控件对象；e，switchSmall控件事件响应参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonSwitch1_CustomButton_Click(object sender, EventArgs e)
        {
            bSwitchState[1] = !bSwitchState[1];

            if (bSwitchState[1])//输出诊断1开启
            {
                customButtonOutput1.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Down;

                customButtonSwitch1.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;

                //

                system.IOSignalData.OutputState = system.IOSignalData.OutputState | 0x00000002;
            }
            else//输出诊断1关闭
            {
                customButtonOutput1.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;

                customButtonSwitch1.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;

                //

                system.IOSignalData.OutputState = system.IOSignalData.OutputState & 0xFFFFFFFD;
            }

            //

            if (null != Switch_Click)//有效
            {
                Switch_Click(this, e);
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：OUT2按钮状态变化响应函数
        // 输入参数： sender，switchSmall控件对象；e，switchSmall控件事件响应参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonSwitch2_CustomButton_Click(object sender, EventArgs e)
        {
            bSwitchState[2] = !bSwitchState[2];

            if (bSwitchState[2])//输出诊断2开启
            {
                customButtonOutput2.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Down;

                customButtonSwitch2.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;

                //

                system.IOSignalData.OutputState = system.IOSignalData.OutputState | 0x00000004;
            }
            else//输出诊断2关闭
            {
                customButtonOutput2.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;

                customButtonSwitch2.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;

                //

                system.IOSignalData.OutputState = system.IOSignalData.OutputState & 0xFFFFFFFB;
            }

            //

            if (null != Switch_Click)//有效
            {
                Switch_Click(this, e);
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：OUT3按钮状态变化响应函数
        // 输入参数： sender，switchSmall控件对象；e，switchSmall控件事件响应参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonSwitch3_CustomButton_Click(object sender, EventArgs e)
        {
            bSwitchState[3] = !bSwitchState[3];

            if (bSwitchState[3])//输出诊断3开启
            {
                customButtonOutput3.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Down;

                customButtonSwitch3.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;

                //

                system.IOSignalData.OutputState = system.IOSignalData.OutputState | 0x00000008;
            }
            else//输出诊断3关闭
            {
                customButtonOutput3.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;

                customButtonSwitch3.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;

                //

                system.IOSignalData.OutputState = system.IOSignalData.OutputState & 0xFFFFFFF7;
            }

            //

            if (null != Switch_Click)//有效
            {
                Switch_Click(this, e);
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：OUT4按钮状态变化响应函数
        // 输入参数： sender，switchSmall控件对象；e，switchSmall控件事件响应参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonSwitch4_CustomButton_Click(object sender, EventArgs e)
        {
            bSwitchState[4] = !bSwitchState[4];

            if (bSwitchState[4])//输出诊断4开启
            {
                customButtonOutput4.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Down;

                customButtonSwitch4.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;

                //

                system.IOSignalData.OutputState = system.IOSignalData.OutputState | 0x00000010;
            }
            else//输出诊断4关闭
            {
                customButtonOutput4.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;

                customButtonSwitch4.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;

                //

                system.IOSignalData.OutputState = system.IOSignalData.OutputState & 0xFFFFFFEF;
            }

            //

            if (null != Switch_Click)//有效
            {
                Switch_Click(this, e);
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：OUT5按钮状态变化响应函数
        // 输入参数： sender，switchSmall控件对象；e，switchSmall控件事件响应参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonSwitch5_CustomButton_Click(object sender, EventArgs e)
        {
            bSwitchState[5] = !bSwitchState[5];

            if (bSwitchState[5])//输出诊断5开启
            {
                customButtonOutput5.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Down;

                customButtonSwitch5.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;

                //

                system.IOSignalData.OutputState = system.IOSignalData.OutputState | 0x00000020;
            }
            else//输出诊断5关闭
            {
                customButtonOutput5.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;

                customButtonSwitch5.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;

                //

                system.IOSignalData.OutputState = system.IOSignalData.OutputState & 0xFFFFFFDF;
            }

            //

            if (null != Switch_Click)//有效
            {
                Switch_Click(this, e);
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：OUT6按钮状态变化响应函数
        // 输入参数： sender，switchSmall控件对象；e，switchSmall控件事件响应参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonSwitch6_CustomButton_Click(object sender, EventArgs e)
        {
            bSwitchState[6] = !bSwitchState[6];

            if (bSwitchState[6])//输出诊断6开启
            {
                customButtonOutput6.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Down;

                customButtonSwitch6.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;

                //

                system.IOSignalData.OutputState = system.IOSignalData.OutputState | 0x00000040;
            }
            else//输出诊断6关闭
            {
                customButtonOutput6.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;

                customButtonSwitch6.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;

                //

                system.IOSignalData.OutputState = system.IOSignalData.OutputState & 0xFFFFFFBF;
            }

            //

            if (null != Switch_Click)//有效
            {
                Switch_Click(this, e);
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：OUT7按钮状态变化响应函数
        // 输入参数： sender，switchSmall控件对象；e，switchSmall控件事件响应参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonSwitch7_CustomButton_Click(object sender, EventArgs e)
        {
            bSwitchState[7] = !bSwitchState[7];

            if (bSwitchState[7])//输出诊断7开启
            {
                customButtonOutput7.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Down;

                customButtonSwitch7.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;

                //

                system.IOSignalData.OutputState = system.IOSignalData.OutputState | 0x00000080;
            }
            else//输出诊断7关闭
            {
                customButtonOutput7.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;

                customButtonSwitch7.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;

                //

                system.IOSignalData.OutputState = system.IOSignalData.OutputState & 0xFFFFFF7F;
            }

            //

            if (null != Switch_Click)//有效
            {
                Switch_Click(this, e);
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：OUT8按钮状态变化响应函数
        // 输入参数： sender，switchSmall控件对象；e，switchSmall控件事件响应参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonSwitch8_CustomButton_Click(object sender, EventArgs e)
        {
            bSwitchState[8] = !bSwitchState[8];

            if (bSwitchState[8])//输出诊断8开启
            {
                customButtonOutput8.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Down;

                customButtonSwitch8.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;

                //

                system.IOSignalData.OutputState = system.IOSignalData.OutputState | 0x00000100;
            }
            else//输出诊断8关闭
            {
                customButtonOutput8.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;

                customButtonSwitch8.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;

                //

                system.IOSignalData.OutputState = system.IOSignalData.OutputState & 0xFFFFFEFF;
            }

            //

            if (null != Switch_Click)//有效
            {
                Switch_Click(this, e);
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：OUT9按钮状态变化响应函数
        // 输入参数： sender，switchSmall控件对象；e，switchSmall控件事件响应参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonSwitch9_CustomButton_Click(object sender, EventArgs e)
        {
            bSwitchState[9] = !bSwitchState[9];

            if (bSwitchState[9])//输出诊断9开启
            {
                customButtonOutput9.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Down;

                customButtonSwitch9.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;

                //

                system.IOSignalData.OutputState = system.IOSignalData.OutputState | 0x00000200;
            }
            else//输出诊断9关闭
            {
                customButtonOutput9.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;

                customButtonSwitch9.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;

                //

                system.IOSignalData.OutputState = system.IOSignalData.OutputState & 0xFFFFFDFF;
            }

            //

            if (null != Switch_Click)//有效
            {
                Switch_Click(this, e);
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：OUT10按钮状态变化响应函数
        // 输入参数： sender，switchSmall控件对象；e，switchSmall控件事件响应参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonSwitch10_CustomButton_Click(object sender, EventArgs e)
        {
            bSwitchState[10] = !bSwitchState[10];

            if (bSwitchState[10])//输出诊断10开启
            {
                customButtonOutput10.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Down;

                customButtonSwitch10.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;

                //

                system.IOSignalData.OutputState = system.IOSignalData.OutputState | 0x00000400;
            }
            else//输出诊断10关闭
            {
                customButtonOutput10.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;

                customButtonSwitch10.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;

                //

                system.IOSignalData.OutputState = system.IOSignalData.OutputState & 0xFFFFFBFF;
            }

            //

            if (null != Switch_Click)//有效
            {
                Switch_Click(this, e);
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：OUT11按钮状态变化响应函数
        // 输入参数： sender，switchSmall控件对象；e，switchSmall控件事件响应参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonSwitch11_CustomButton_Click(object sender, EventArgs e)
        {
            bSwitchState[11] = !bSwitchState[11];

            if (bSwitchState[11])//输出诊断11开启
            {
                customButtonOutput11.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Down;

                customButtonSwitch11.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;

                //

                system.IOSignalData.OutputState = system.IOSignalData.OutputState | 0x00000800;
            }
            else//输出诊断11关闭
            {
                customButtonOutput11.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;

                customButtonSwitch11.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;

                //

                system.IOSignalData.OutputState = system.IOSignalData.OutputState & 0xFFFFF7FF;
            }

            //

            if (null != Switch_Click)//有效
            {
                Switch_Click(this, e);
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：OUT12按钮状态变化响应函数
        // 输入参数： sender，switchSmall控件对象；e，switchSmall控件事件响应参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonSwitch12_CustomButton_Click(object sender, EventArgs e)
        {
            bSwitchState[12] = !bSwitchState[12];

            if (bSwitchState[12])//输出诊断12开启
            {
                customButtonOutput12.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Down;

                customButtonSwitch12.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;

                //

                system.IOSignalData.OutputState = system.IOSignalData.OutputState | 0x00001000;
            }
            else//输出诊断12关闭
            {
                customButtonOutput12.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;

                customButtonSwitch12.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;

                //

                system.IOSignalData.OutputState = system.IOSignalData.OutputState & 0xFFFFEFFF;
            }

            //

            if (null != Switch_Click)//有效
            {
                Switch_Click(this, e);
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：OUT13按钮状态变化响应函数
        // 输入参数： sender，switchSmall控件对象；e，switchSmall控件事件响应参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonSwitch13_CustomButton_Click(object sender, EventArgs e)
        {
            bSwitchState[13] = !bSwitchState[13];

            if (bSwitchState[13])//输出诊断13开启
            {
                customButtonOutput13.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Down;

                customButtonSwitch13.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;

                //

                system.IOSignalData.OutputState = system.IOSignalData.OutputState | 0x00002000;
            }
            else//输出诊断13关闭
            {
                customButtonOutput13.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;

                customButtonSwitch13.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;

                //

                system.IOSignalData.OutputState = system.IOSignalData.OutputState & 0xFFFFDFFF;
            }

            //

            if (null != Switch_Click)//有效
            {
                Switch_Click(this, e);
            }
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：点击【CLOSE】按钮产生的事件，执行相关的操作
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void customButtonClose_CustomButton_Click(object sender, EventArgs e)
        {
            _Reset();

            //事件

            if (null != Close_Click)//有效
            {
                Close_Click(this, e);
            }
        }
    }
}