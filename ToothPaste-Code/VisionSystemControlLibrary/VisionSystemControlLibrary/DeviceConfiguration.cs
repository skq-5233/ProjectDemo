/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：DeviceConfiguration.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：DEVICE CONFIGURATION控件

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
    public partial class DeviceConfiguration : UserControl
    {
        //DEVICE CONFIGURATION控件

        private VisionSystemClassLibrary.Enum.InterfaceLanguage language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;//属性，语言

        //

        private String[] ControllerListed_ENG;//列表中显示的控制器英文名称
        private String[] ControllerListed_CHN;//列表中显示的控制器中文名称

        private int iDeviceDataIndex = -1;//属性，设备信息数组序号（取值为-1，表示无设备信息）

        //

        private VisionSystemClassLibrary.Class.System system = new VisionSystemClassLibrary.Class.System();//属性（只读），属性

        //

        private VisionSystemClassLibrary.Enum.DeviceConfigurationResult controlResult = VisionSystemClassLibrary.Enum.DeviceConfigurationResult.None;//属性（只读），操作结果

        //

        private String sControllerSelected = "";//属性（只读），新选择的控制器类型（英文名称）

        //

        private String[][] sMessageText = new String[2][];//提示信息对话框上显示的文本（[语言][包含的文本]）

        private String[][] sMessageText_1 = new String[2][];//标题控件上显示的文本（[语言][包含的文本]）

        //

        [Browsable(true), Description("窗口关闭时产生的事件"), Category("DeviceConfiguration 事件")]
        public event EventHandler Close_Click;//窗口关闭时产生的事件

        //构造函数

        //----------------------------------------------------------------------
        // 功能说明：构造函数，初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public DeviceConfiguration()
        {
            InitializeComponent();

            //由于该控件仅有一个实例，因此可以在此处进行事件的订阅

            if (null != GlobalWindows.MessageDisplay_Window)//有效
            {
                GlobalWindows.MessageDisplay_Window.WindowClose_DeviceConfiguration_Ok_Confirm += new System.EventHandler(messageDisplayWindow_WindowClose_DeviceConfiguration_Ok_Confirm);//订阅事件
            }

            //

            Int32 i = 0;//循环控制变量

            FieldInfo[] fieldinfo = VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese.GetType().GetFields();//获取公共字段
            if (null != fieldinfo && 1 < fieldinfo.Length)//有效
            {
                sMessageText = new String[fieldinfo.Length - 1][];
                sMessageText_1 = new String[fieldinfo.Length - 1][];
                for (i = 0; i < fieldinfo.Length - 1; i++)
                {
                    sMessageText[i] = new String[2];
                    sMessageText_1[i] = new String[1];
                }

                //

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] = "确定配置设备";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] = "Configure Device";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][1] = "确定更新设备数据";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][1] = "Update Device Data";

                sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] = customButtonCaption.Chinese_TextDisplay[0];
                sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] = customButtonCaption.English_TextDisplay[0];
            }

            //

            ControllerListed_ENG = new String[system.SystemControllerName_ENG.Length];//列表中显示的控制器英文名称
            ControllerListed_CHN = new String[system.SystemControllerName_ENG.Length];//列表中显示的控制器中文名称

            for (i = 0; i < system.SystemCameraConfiguration.Length; i++)//赋初值
            {
                ControllerListed_ENG[i] = "";//列表中显示的控制器英文名称
                ControllerListed_CHN[i] = "";//列表中显示的控制器中文名称
            }

            //

            customButtonMessage.Chinese_TextDisplay = new String[1] { "" };//设置显示的文本
            customButtonMessage.English_TextDisplay = new String[1] { "" };//设置显示的文本
        }

        //属性

        //-----------------------------------------------------------------------
        // 功能说明：Language属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("语言"), Category("DeviceConfiguration 通用")]
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
        // 功能说明：DeviceDataIndex属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("设备信息数组序号（取值为-1，表示无设备信息）"), Category("DeviceConfiguration 通用")]
        public int DeviceDataIndex//属性
        {
            get//读取
            {
                return iDeviceDataIndex;
            }
            set//设置
            {
                if (value != iDeviceDataIndex)//设置了新的数值
                {
                    iDeviceDataIndex = value;

                    //

                    _SetCaption();//标题控件
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ControlResult属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("操作结果"), Category("DeviceConfiguration 通用")]
        public VisionSystemClassLibrary.Enum.DeviceConfigurationResult ControlResult//属性
        {
            get//读取
            {
                return controlResult;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：SelectedController属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("新选择的控制器名称"), Category("DeviceConfiguration 通用")]
        public String SelectedController//属性
        {
            get//读取
            {
                return sControllerSelected;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：SystemData属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("系统"), Category("DeviceConfiguration 通用")]
        public VisionSystemClassLibrary.Class.System SystemData//属性
        {
            get//读取
            {
                return system;
            }
        }

        //函数

        //----------------------------------------------------------------------
        // 功能说明：应用设置的属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _Properties(VisionSystemClassLibrary.Class.System system_parameter)
        {
            system = system_parameter;

            //

            _SetSystem();
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
            controlResult = VisionSystemClassLibrary.Enum.DeviceConfigurationResult.None;//操作结果

            //

            _Reset();

            //事件

            if (null != Close_Click)//有效
            {
                Close_Click(this, new CustomEventArgs());
            }
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：设置标题控件
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _SetCaption()
        {
            try
            {
                if (0 <= iDeviceDataIndex)//DEVICE CONFIGURATION页面选择了某一设备
                {
                    customButtonCaption.Chinese_TextDisplay = new String[1] { _GetControllerName(system.ConnectionData[iDeviceDataIndex].Type, VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese) + "（" + system.ConnectionData[iDeviceDataIndex].IP + "）" + " - " + sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] };//设置显示的文本
                    customButtonCaption.English_TextDisplay = new String[1] { _GetControllerName(system.ConnectionData[iDeviceDataIndex].Type, VisionSystemClassLibrary.Enum.InterfaceLanguage.English) + "（" + system.ConnectionData[iDeviceDataIndex].IP + "）" + " - " + sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] };//设置显示的文本
                }
                else//DEVICE CONFIGURATION页面未选择任何设备（此种情况在实际应用中不存在）
                {
                    customButtonCaption.Chinese_TextDisplay = new String[1] { sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] };//设置显示的文本
                    customButtonCaption.English_TextDisplay = new String[1] { sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] };//设置显示的文本
                }
            }
            catch (System.Exception ex)
            {
            	//不执行操作
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：恢复控件内容
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _Reset()
        {
            customList._ClearAllListItem();

            //

            customButtonPreviousPage.Visible = false;//【Previous Page】
            customButtonNextPage.Visible = false;//【Next Page】

            customButtonCancel.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【Cancel】

            //

            customButtonCaption.Chinese_TextDisplay = new String[1] { sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] };//设置显示的文本
            customButtonCaption.English_TextDisplay = new String[1] { sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] };//设置显示的文本

            customButtonMessage.Chinese_TextDisplay = new String[1] { "" };//设置显示的文本
            customButtonMessage.English_TextDisplay = new String[1] { "" };//设置显示的文本
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：获取相机名称
        // 输入参数：1.cameratype：相机类型
        //         2.language_parameter：语言
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private String _GetCameraName(VisionSystemClassLibrary.Enum.CameraType cameratype, VisionSystemClassLibrary.Enum.InterfaceLanguage language_parameter)
        {
            string sReturn = "";

            for (int i = 0; i < system.SystemCameraConfiguration.Length; i++)
            {
                if (system.SystemCameraConfiguration[i].Type == cameratype)
                {
                    if (VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese == language_parameter)//中文
                    {
                        sReturn = system.SystemCameraConfiguration[i].CameraCHNName;
                    }
                    else if (VisionSystemClassLibrary.Enum.InterfaceLanguage.English == language_parameter)//英文
                    {
                        sReturn = system.SystemCameraConfiguration[i].CameraENGName;
                    }
                    else//默认中文
                    {
                        sReturn = system.SystemCameraConfiguration[i].CameraCHNName;
                    }

                    break;
                }
            }

            return sReturn;
        }

        //----------------------------------------------------------------------
        // 功能说明：获取控制器名称
        // 输入参数：1.cameratype：相机类型
        //         2.language_parameter：语言
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private String _GetControllerName(VisionSystemClassLibrary.Enum.CameraType cameratype, VisionSystemClassLibrary.Enum.InterfaceLanguage language_parameter)
        {
            string sReturn = "";

            for (int i = 0; i < system.SystemCameraConfiguration.Length; i++)
            {
                if (system.SystemCameraConfiguration[i].Type == cameratype)
                {
                    if (VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese == language_parameter)//中文
                    {
                        sReturn = system.SystemCameraConfiguration[i].ControllerCHNName;
                    }
                    else if (VisionSystemClassLibrary.Enum.InterfaceLanguage.English == language_parameter)//英文
                    {
                        sReturn = system.SystemCameraConfiguration[i].ControllerENGName;
                    }
                    else//默认中文
                    {
                        sReturn = system.SystemCameraConfiguration[i].ControllerCHNName;
                    }

                    break;
                }
            }

            return sReturn;
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：设置语言
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetLanguage()
        {
            customButtonCaption.Language = language;//标题控件

            //

            customButtonMessage.Language = language;

            //

            customList.Language = language;//列表语言
        }

        //----------------------------------------------------------------------
        // 功能说明：添加列表项数据
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _AddListItemData()
        {
            Int32 i = 0;//循环控制变量

            if (VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese == language)//中文
            {
                for (i = 0; i < customList.ItemDataNumber; i++)//列表项数据
                {
                    customList.ItemData[i].ItemText[0] = ControllerListed_CHN[i];//属性，列表项数据

                    customList.ItemData[i].ItemFlag = i;
                }
            }
            else if (VisionSystemClassLibrary.Enum.InterfaceLanguage.English == language)//英文
            {
                for (i = 0; i < customList.ItemDataNumber; i++)//列表项数据
                {
                    customList.ItemData[i].ItemText[0] = ControllerListed_ENG[i];//属性，列表项数据

                    customList.ItemData[i].ItemFlag = i;
                }
            }
            else//默认中文
            {
                for (i = 0; i < customList.ItemDataNumber; i++)//列表项数据
                {
                    customList.ItemData[i].ItemText[0] = ControllerListed_CHN[i];//属性，列表项数据

                    customList.ItemData[i].ItemFlag = i;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：设置提示信息
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetMessage()
        {
            Int32 i = 0;//循环控制变量
            String sString_1 = "";//临时变量
            String sString_2 = "";//临时变量
            Int32 iIpIndex = 0;

            for (i = 0; i < system.SystemCameraConfiguration.Length; i++)
            {
                if (sControllerSelected == system.SystemCameraConfiguration[i].ControllerENGName)//相同
                {
                    sString_1 += system.SystemCameraConfiguration[i].CameraCHNName + "，";
                    sString_2 += system.SystemCameraConfiguration[i].CameraENGName + "，";
                    iIpIndex = i;
                }
            }

            if (i > 0) //设备有效
            {
                sString_1 = sString_1.Substring(0, sString_1.Length - 1);
                sString_2 = sString_2.Substring(0, sString_2.Length - 1);

                sString_1 += "（" + system.SystemCameraConfiguration[iIpIndex].IPAddress + "）";
                sString_2 += "（" + system.SystemCameraConfiguration[iIpIndex].IPAddress + "）";
            }

            customButtonMessage.Chinese_TextDisplay = new String[1] { sString_1 };//设置显示的文本
            customButtonMessage.English_TextDisplay = new String[1] { sString_2 };//设置显示的文本
        }

        //----------------------------------------------------------------------
        // 功能说明：设置数据
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetSystem()
        {
            _SetCaption();//标题控件

            //

            if (0 <= iDeviceDataIndex)//DEVICE CONFIGURATION页面选择了某一设备
            {
                sControllerSelected = _GetControllerName(system.ConnectionData[iDeviceDataIndex].Type, VisionSystemClassLibrary.Enum.InterfaceLanguage.English);//属性（只读），新选择的控制器名称（英文名称）

                //

                _SetMessage();

                //

                _GetListData();//获取列表数据

                //

                _InitListData();//初始化列表数据

                _SetListData();//设置列表数据

                //

                customButtonOk.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【OK】
                customButtonCancel.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【Cancel】
            }
            else//DEVICE CONFIGURATION页面未选择任何设备（此种情况在实际应用中不存在）
            {
                sControllerSelected = "";//属性（只读），新选择的控制器名称

                customButtonOk.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【OK】
                customButtonCancel.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【Cancel】
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：获取列表数据
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _GetListData()
        {
            Int32 i = 0;//循环控制变量
            Int32 j = 0;//循环控制变量
            Int32 k = 0;//循环控制变量

            Int32 iItemNumberTotal = 0;//临时变量，列表项数目（0表示无有效列表项）

            //

            String[] sControllerListed_CHN = new String[system.SystemControllerName_CHN.Length];//临时变量，列表中显示的控制器中文名称
            String[] sControllerListed_ENG = new String[system.SystemControllerName_ENG.Length];//临时变量，列表中显示的控制器英文名称

            for (i = 0; i < system.SystemControllerName_ENG.Length; i++)//控制器
            {
                sControllerListed_CHN[i] = "";
                sControllerListed_ENG[i] = "";

                //

                if (sControllerSelected == system.SystemControllerName_ENG[i])//相同
                {
                    sControllerListed_CHN[iItemNumberTotal] = system.SystemControllerName_CHN[i];
                    sControllerListed_ENG[iItemNumberTotal] = system.SystemControllerName_ENG[i];

                    iItemNumberTotal++;
                }
                else//不同
                {
                    Boolean bValue = true;//临时变量

                    for (j = 0; j < system.SystemCameraConfiguration.Length; j++)//遍历
                    {
                        if (system.SystemControllerName_ENG[i] == system.SystemCameraConfiguration[j].ControllerENGName)//相同
                        {
                            for (k = 0; k < system.ConnectionData.Length; k++)//遍历设备
                            {
                                if (system.SystemCameraConfiguration[j].Type == system.ConnectionData[k].Type)//相机类型相同
                                {
                                    if (system.ConnectionData[k].Connected && system.ConnectionData[k].GetDevInfo)//设备连接
                                    {
                                        bValue = false;
                                    }

                                    break;
                                }
                            }
                        }
                    }

                    if (bValue)//有效
                    {
                        sControllerListed_CHN[iItemNumberTotal] = system.SystemControllerName_CHN[i];
                        sControllerListed_ENG[iItemNumberTotal] = system.SystemControllerName_ENG[i];

                        iItemNumberTotal++;
                    }
                }
            }

            ControllerListed_CHN = new String[iItemNumberTotal];//列表中显示的控制器中文名称
            ControllerListed_ENG = new String[iItemNumberTotal];//列表中显示的控制器英文名称
            Array.Copy(sControllerListed_CHN, 0, ControllerListed_CHN, 0, iItemNumberTotal);
            Array.Copy(sControllerListed_ENG, 0, ControllerListed_ENG, 0, iItemNumberTotal);
        }

        //----------------------------------------------------------------------
        // 功能说明：设置列表项数据
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _InitListData()
        {
            Int32 i = 0;//循环控制变量

            customList._ApplyListHeader();//应用设置
            customList._ApplyListItem();//应用列表项属性

            for (i = 0; i < ControllerListed_ENG.Length; i++)
            {
                if (sControllerSelected == ControllerListed_ENG[i])//当前选择的项
                {
                    break;
                }
            }

            customList._Apply(ControllerListed_ENG.Length, i, i);//应用列表属性
        }

        //----------------------------------------------------------------------
        // 功能说明：设置列表项数据
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetListData()
        {
            _AddListItemData();//添加列表项数据

            customList._SetPage();//设置列表项数据

            //

            if (1 >= customList.TotalPage)//小于等于1页
            {
                customButtonPreviousPage.Visible = false;//隐藏【Previous Page】按钮
                customButtonNextPage.Visible = false;//隐藏【Next Page】按钮
            }
            else//大于一页
            {
                customButtonPreviousPage.Visible = true;//【Previous Page】按钮
                customButtonNextPage.Visible = true;//【Next Page】按钮
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：检查是否选择了新的控制器
        // 输入参数：无
        // 输出参数：无
        // 返回值：检查结果数值
        //----------------------------------------------------------------------
        private VisionSystemClassLibrary.Enum.DeviceConfigurationResult _CheckController()
        {
            Int32 i = 0;//循环控制变量
            VisionSystemClassLibrary.Enum.DeviceConfigurationResult ReturnValue = VisionSystemClassLibrary.Enum.DeviceConfigurationResult.None;//返回值

            if (0 <= customList.CurrentDataIndex && 0 <= iDeviceDataIndex)//选择了有效的项
            {
                for (i = 0; i < system.SystemCameraConfiguration.Length; i++)//遍历
                {
                    if (system.ConnectionData[iDeviceDataIndex].Type == system.SystemCameraConfiguration[i].Type)//相同
                    {
                        if (system.SystemCameraConfiguration[i].ControllerENGName != sControllerSelected || system.ConnectionData[iDeviceDataIndex].IP != system.SystemCameraConfiguration[i].IPAddress)//选择了新的控制器
                        {
                            ReturnValue = VisionSystemClassLibrary.Enum.DeviceConfigurationResult.SelectController;
                        }

                        break;
                    }
                }
            }

            return ReturnValue;
        }

        //----------------------------------------------------------------------
        // 功能说明：检查是否需要更新设备数据
        // 输入参数：无
        // 输出参数：无
        // 返回值：检查结果数值
        //----------------------------------------------------------------------
        private VisionSystemClassLibrary.Enum.DeviceConfigurationResult _CheckUpdateData()
        {
            Int32 i = 0;//循环控制变量
            Int32 j = 0;//循环控制变量
            VisionSystemClassLibrary.Enum.DeviceConfigurationResult ReturnValue = VisionSystemClassLibrary.Enum.DeviceConfigurationResult.None;//返回值

            if (0 <= customList.CurrentDataIndex && 0 <= iDeviceDataIndex)//选择了有效的项
            {
                for (i = 0; i < system.SystemCameraConfiguration.Length; i++)//遍历
                {
                    if (system.ConnectionData[iDeviceDataIndex].Type == system.SystemCameraConfiguration[i].Type)//相同
                    {
                        for (j = 0; j < system.ConnectionData.Length; j++)
                        {
                            if (system.ConnectionData[j].Type == system.SystemCameraConfiguration[i].Type && (VisionSystemClassLibrary.Enum.CameraState.NOTUPDATED == (system.ConnectionData[j].CAM & VisionSystemClassLibrary.Enum.CameraState.NOTUPDATED)))//
                            {
                                break;
                            }
                        }
                        if (j < system.ConnectionData.Length)
                        {
                            ReturnValue = VisionSystemClassLibrary.Enum.DeviceConfigurationResult.UpdateData;
                        }

                        break;
                    }
                }
            }

            return ReturnValue;
        }

        //事件

        //----------------------------------------------------------------------
        // 功能说明：控件加载事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void DeviceConfiguration_Load(object sender, EventArgs e)
        {
            //不执行操作
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：点击列表事件，执行相关操作
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customList_CustomListItem_Click(object sender, EventArgs e)
        {
            if (0 <= customList.CurrentDataIndex)//选择了有效的项
            {
                sControllerSelected = ControllerListed_ENG[customList.CurrentDataIndex];//属性（只读），新选择的控制器
            }
            else//未选择有效的项
            {
                sControllerSelected = "";//属性（只读），新选择的控制器
            }

            //

            _SetMessage();
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：点击【Previous Page】按钮产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonPreviousPage_CustomButton_Click(object sender, EventArgs e)
        {
            customList._ClickPage(true);//翻页
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【Next Page】按钮产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonNextPage_CustomButton_Click(object sender, EventArgs e)
        {
            customList._ClickPage(false);//翻页
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：点击【Ok】按钮产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonOk_CustomButton_Click(object sender, EventArgs e)
        {
            controlResult = _CheckController() | _CheckUpdateData();//操作结果

            //显示信息对话框

            GlobalWindows.MessageDisplay_Window.WindowParameter = 75;//窗口特征数值
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.OkCancel;//包含【确定】和【取消】按钮
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
            if (VisionSystemClassLibrary.Enum.DeviceConfigurationResult.UpdateData == (controlResult & VisionSystemClassLibrary.Enum.DeviceConfigurationResult.UpdateData))//
            {
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][1] + "？";
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][1] + "？";
            } 
            else
            {
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] + "？";
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] + "？";
            }

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

        //----------------------------------------------------------------------
        // 功能说明：点击【Cancel】按钮产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonCancel_CustomButton_Click(object sender, EventArgs e)
        {
            controlResult = VisionSystemClassLibrary.Enum.DeviceConfigurationResult.None;//操作结果

            //

            _Reset();

            //事件

            if (null != Close_Click)//有效
            {
                Close_Click(this, new CustomEventArgs());
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：DEVICE CONFIGURATION，【Ok】确认，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //---------------------------------------------------------------------- 
        private void messageDisplayWindow_WindowClose_DeviceConfiguration_Ok_Confirm(object sender, EventArgs e)
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

            if (GlobalWindows.MessageDisplay_Window.MessageDisplayControl.OkCancel)//确认
            {
                _Reset();

                //事件

                if (null != Close_Click)//有效
                {
                    Close_Click(this, new CustomEventArgs());
                }
            }
            else//不进行复位
            {
                //不执行操作
            }
        }
    }
}