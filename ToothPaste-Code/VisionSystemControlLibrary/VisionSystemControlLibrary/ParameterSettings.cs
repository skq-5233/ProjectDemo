/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：ParameterSettings.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：参数设置控件

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

using System.IO;

using System.Runtime.InteropServices;

using System.Reflection;

namespace VisionSystemControlLibrary
{
    public partial class ParameterSettings : UserControl
    {
        //PARAMETER SETTINGS

        private VisionSystemClassLibrary.Enum.InterfaceLanguage language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;//属性，语言

        private VisionSystemClassLibrary.Class.Camera camera = new VisionSystemClassLibrary.Class.Camera();//属性（只读），相机
        private VisionSystemClassLibrary.Class.Brand brand = new VisionSystemClassLibrary.Class.Brand();//属性（只读），品牌

        private VisionSystemClassLibrary.Struct.CameraParameter cameraparameter = new VisionSystemClassLibrary.Struct.CameraParameter();//参数

        //

        private Boolean bEnterNewValue = false;//属性（只读），是否输入了新的数值。取值范围：true，是；false，否

        private Boolean bSaveSuccess = false;//属性（只读），保存是否成功。取值范围：true，是；false，否

        //

        private String[][] sMessageText = new String[2][];//提示信息窗口上显示的文本（[语言][包含的文本]）
        private String[][] sMessageText_1 = new String[2][];//标题控件上显示的文本（[语言][包含的文本]）

        //

        [Browsable(true), Description("窗口关闭时产生的事件"), Category("ParameterSettings 事件")]
        public event EventHandler Close_Click;//窗口关闭时产生的事件

        [Browsable(true), Description("点击【保存参数】按钮时产生的事件"), Category("ParameterSettings 事件")]
        public event EventHandler Save_Click;

        //构造函数

        //----------------------------------------------------------------------
        // 功能说明：构造函数，初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public ParameterSettings()
        {
            InitializeComponent();

            //

            if (null != GlobalWindows.DigitalKeyboard_Window)
            {
                GlobalWindows.DigitalKeyboard_Window.WindowClose_ParameterSettings_Parameter += new System.EventHandler(digitalKeyboardWindow_WindowClose_ParameterSettings_Parameter);//订阅事件
            }

            if (null != GlobalWindows.MessageDisplay_Window)
            {
                GlobalWindows.MessageDisplay_Window.WindowClose_ParameterSettings_Save_Confirm += new System.EventHandler(messageDisplayWindow_WindowClose_ParameterSettings_Save_Confirm);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_ParameterSettings_Save_Wait += new System.EventHandler(messageDisplayWindow_WindowClose_ParameterSettings_Save_Wait);//订阅事件
            }

            //

            FieldInfo[] fieldinfo = VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese.GetType().GetFields();//获取公共字段
            if (null != fieldinfo && 1 < fieldinfo.Length)//有效
            {
                Int32 i = 0;//循环控制变量

                sMessageText = new String[fieldinfo.Length - 1][];
                for (i = 0; i < fieldinfo.Length - 1; i++)
                {
                    sMessageText[i] = new String[4];
                    sMessageText_1[i] = new String[1];
                }

                //

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] = "确定保存数据";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] = "Save Parameters";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][1] = "正在保存数据";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][1] = "Saving Parameters";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][2] = "保存数据成功";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][2] = "Save Parameters Completed";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][3] = "保存数据失败";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][3] = "Save Parameters Failed";

                //

                sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] = customButtonCaption.Chinese_TextDisplay[0];
                sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] = customButtonCaption.English_TextDisplay[0];
            }
        }

        //属性

        //-----------------------------------------------------------------------
        // 功能说明：相机类
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("相机"), Category("ParameterSettings 通用")]
        public VisionSystemClassLibrary.Class.Camera SelectedCamera
        {
            get
            {
                return camera;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：SystemBrand属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("品牌"), Category("ParameterSettings 通用")]
        public VisionSystemClassLibrary.Class.Brand SystemBrand//属性
        {
            get//读取
            {
                return brand;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Language属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("语言"), Category("ParameterSettings 通用")]
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

        //函数

        //----------------------------------------------------------------------
        // 功能说明：应用设置的属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _Properties(VisionSystemClassLibrary.Class.Camera camera_parameter, VisionSystemClassLibrary.Class.Brand brand_parameter)
        {
            brand = brand_parameter;
            camera = camera_parameter;
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：相机离线时，关闭窗口
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        public void _Close()
        {
            bEnterNewValue = false;

            //事件

            if (null != Close_Click)//有效
            {
                Close_Click(this, new CustomEventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：应用设置
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _Apply()
        {
            _SetCameraParameter();//载入

            _SetLanguage();//设置语言
        }

        //----------------------------------------------------------------------
        // 功能说明：保存参数
        // 输入参数：1.bSuccess：保存是否成功。取值范围：true，是；false，否
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _SaveParameter(Boolean bSuccess)
        {
            bSaveSuccess = bSuccess;

            //

            if (bSuccess)//成功
            {
                bEnterNewValue = false;

                //显示信息窗口

                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][2];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][2];
            }
            else//失败
            {
                //显示信息窗口

                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][3];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][3];
            }
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：属性设置
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        private void _SetCameraParameter()
        {
            cameraparameter = camera.DeviceParameter;

            //

            customButtonCaption.Chinese_TextDisplay = new String[1] { camera.CameraCHNName + sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] };//设置显示的文本
            customButtonCaption.English_TextDisplay = new String[1] { camera.CameraENGName + sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] };//设置显示的文本

            bEnterNewValue = false;

            //

            _InitList();
        }

        //-----------------------------------------------------------------------
        // 功能说明：设置语言
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _SetLanguage()
        {
            customButtonCaption.Language = language;

            customList.Language = language;
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：初始化列表
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _InitList()
        {
            customList._ApplyListHeader();//应用列表头属性
            customList._ApplyListItem();//应用列表项属性

            //

            _SetList();
        }

        //----------------------------------------------------------------------
        // 功能说明：设置列表
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetList()
        {
            customList._Apply(cameraparameter.Parameter.Count);//应用列表属性

            _AddItemData();//添加列表项数据

            _SetPage();//设置列表数据
        }

        //----------------------------------------------------------------------
        // 功能说明：添加列表项数据
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _AddItemData()
        {
            Int32 i = 0;//循环控制变量

            switch (language)
            {
                case VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese:

                    for (i = 0; i < cameraparameter.Parameter.Count; i++)
                    {
                        customList.ItemData[i].ItemText[0] = cameraparameter.Parameter_NameCHN[i];
                        customList.ItemData[i].ItemText[1] = cameraparameter.Parameter[i].ToString();
                        customList.ItemData[i].ItemFlag = i;
                    }

                    break;
                case VisionSystemClassLibrary.Enum.InterfaceLanguage.English:

                    for (i = 0; i < cameraparameter.Parameter.Count; i++)
                    {
                        customList.ItemData[i].ItemText[0] = cameraparameter.Parameter_NameENG[i];
                        customList.ItemData[i].ItemText[1] = cameraparameter.Parameter[i].ToString();
                        customList.ItemData[i].ItemFlag = i;
                    }

                    break;
                default:

                    for (i = 0; i < cameraparameter.Parameter.Count; i++)
                    {
                        customList.ItemData[i].ItemText[0] = cameraparameter.Parameter_NameCHN[i];
                        customList.ItemData[i].ItemText[1] = cameraparameter.Parameter[i].ToString();
                        customList.ItemData[i].ItemFlag = i;
                    }

                    break;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：设置列表数据
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetPage()
        {
            customList._SetPage();//设置列表项数据

            if (1 < customList.TotalPage)//多于一页
            {
                customButtonPreviousPage.Visible = true;//【Previous Page】
                customButtonNextPage.Visible = true;//【Next Page】
            }
            else//小于等于一页
            {
                customButtonPreviousPage.Visible = false;//【Previous Page】
                customButtonNextPage.Visible = false;//【Next Page】
            }
        }

        //事件

        //----------------------------------------------------------------------
        // 功能说明：列表点击事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customList_CustomListItem_Click(object sender, EventArgs e)
        {
            GlobalWindows.DigitalKeyboard_Window.WindowParameter = 15;//窗口特征数值
            GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.Language = language;//语言
            GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.Chinese_Caption = cameraparameter.Parameter_NameCHN[customList.CurrentListIndex];//中文标题文本
            GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.English_Caption = cameraparameter.Parameter_NameENG[customList.CurrentListIndex];//英文标题文本
            GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.Precision = 0;//输入的数据类型
            GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.MaxLength = 10;//数值长度范围
            GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.MinValue = cameraparameter.Parameter_Min[customList.CurrentListIndex];//最小值
            GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.MaxValue = cameraparameter.Parameter_Max[customList.CurrentListIndex];//最大值
            GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.NumericalValue = cameraparameter.Parameter[customList.CurrentListIndex];//初始显示的数值

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

        //----------------------------------------------------------------------
        // 功能说明：【Previous Page】按钮点击事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonPreviousPage_CustomButton_Click(object sender, EventArgs e)
        {
            customList._ClickPage(true);//翻页，上一页
        }

        //----------------------------------------------------------------------
        // 功能说明：【Next Page】按钮点击事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonNextPage_CustomButton_Click(object sender, EventArgs e)
        {
            customList._ClickPage(false);//翻页，下一页
        }

        //----------------------------------------------------------------------
        // 功能说明：【Cancel】按钮点击事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonCancel_CustomButton_Click(object sender, EventArgs e)
        {
            bEnterNewValue = false;

            //事件

            if (null != Close_Click)//有效
            {
                Close_Click(this, new CustomEventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：【OK】按钮点击事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonOk_CustomButton_Click(object sender, EventArgs e)
        {
            if (bEnterNewValue)//修改
            {
                //显示信息窗口

                GlobalWindows.MessageDisplay_Window.WindowParameter = 104;//窗口特征数值
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.OkCancel;//包含【确定】和【取消】按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] + "？";
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] + "？";

                GlobalWindows.MessageDisplay_Window.StartPosition = FormStartPosition.CenterScreen;
                if (GlobalWindows.TopMostWindows)//置顶
                {
                    GlobalWindows.MessageDisplay_Window.TopMost = true;//将窗口置于顶层
                }
                else//其它
                {
                    GlobalWindows.MessageDisplay_Window.Visible = true;//显示
                }
            } 
            else//未修改
            {
                bEnterNewValue = false;

                //事件

                if (null != Close_Click)//有效
                {
                    Close_Click(this, new CustomEventArgs());
                }
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：PARAMETER SETTINGS，参数，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void digitalKeyboardWindow_WindowClose_ParameterSettings_Parameter(object sender, EventArgs e)
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
                bEnterNewValue = true;

                //

                cameraparameter.Parameter[customList.CurrentListIndex] = (Int32)GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.NumericalValue;

                //

                customList.ItemData[customList.CurrentListIndex].ItemText[1] = GlobalWindows.DigitalKeyboard_Window.DigitalKeyboardControl.StringValue;
                customList._Refresh(customList.Index_Page);//刷新
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：PARAMETER SETTINGS，【SAVE】确认，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_ParameterSettings_Save_Confirm(object sender, EventArgs e)
        {
            if (GlobalWindows.MessageDisplay_Window.MessageDisplayControl.OkCancel)//确定
            {
                GlobalWindows.MessageDisplay_Window.WindowParameter = 105;//窗口特征数值
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.None;//包含【确定】和【取消】按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][1] + "...";
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][1] + "...";

                //

                camera.DeviceParameter = cameraparameter;

                camera._SaveParameter();//

                File.Copy(camera.DataPath + VisionSystemClassLibrary.Class.Camera.ParameterFileName, brand.BrandPath + brand.CURRENTBrandName + "\\" + camera.CameraENGName + "\\" + VisionSystemClassLibrary.Class.Camera.ParameterFileName, true);

                //VisionSystemClassLibrary.Class.System.FileCopyFun(camera.DataPath + VisionSystemClassLibrary.Class.Camera.ParameterFileName, brand.BrandPath + brand.CURRENTBrandName + "\\" + camera.CameraENGName + "\\" + VisionSystemClassLibrary.Class.Camera.ParameterFileName);

                //事件

                if (null != Save_Click)//有效
                {
                    Save_Click(this, new CustomEventArgs());
                }
            }
            else//取消
            {
                if (GlobalWindows.TopMostWindows)//置顶
                {
                    GlobalWindows.MessageDisplay_Window.TopMost = false;//取消置于顶层
                }
                else//其它
                {
                    GlobalWindows.MessageDisplay_Window.Visible = false;//隐藏
                }        
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：PARAMETER SETTINGS，【SAVE】等待，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_ParameterSettings_Save_Wait(object sender, EventArgs e)
        {
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.MessageDisplay_Window.TopMost = false;//取消置于顶层
            }
            else//其它
            {
                GlobalWindows.MessageDisplay_Window.Visible = false;//隐藏
            }

            //

            if (bSaveSuccess)//成功
            {
                //事件

                if (null != Close_Click)//有效
                {
                    Close_Click(this, new CustomEventArgs());
                }
            } 
        }
    }
}
