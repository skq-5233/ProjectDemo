/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：FaultMessage.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：故障信息窗口

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
using System.Diagnostics;

using System.Threading;

using System.Runtime.InteropServices;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;

using System.Reflection;

namespace VisionSystemControlLibrary
{
    public partial class FaultMessage : UserControl
    {
        //FaultMessage控件

        private VisionSystemClassLibrary.Enum.InterfaceLanguage language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;//属性，语言

        //

        private VisionSystemClassLibrary.Class.System system = new VisionSystemClassLibrary.Class.System();//属性（只读），系统

        //

        private Int32 iDeviceNumber = 0;//属性，相机设备数量（获取故障信息数据时使用）

        //

        private Boolean bClearAllFaultMessages = false;//是否进行了清除所有故障信息数据的操作。取值范围：true，是；false，否

        //

        private Boolean bMessageWindowShow = false;//属性（只读），是否显示正在获取数据时的提示信息窗口。取值范围：true，是；false，否

        private const Int32 iTimerMaxCount = 10;//定时器时间
        private Int32 iTimerCount = 10;//定时器时间

        //

        private Boolean bClearAllFaultMessagesSuccess = false;//清除所有故障信息数据是否成功。取值范围：true，是；false，否

        //

        private String[][] sMessageText = new String[2][];//提示信息对话框、列表中显示的文本（[语言][包含的文本]）

        //

        [Browsable(true), Description("清除所有故障信息时产生的事件"), Category("FaultMessage 事件")]
        public event EventHandler ClearAllFaultMessages;//清除所有故障信息时产生的事件

        [Browsable(true), Description("窗口关闭时产生的事件"), Category("FaultMessage 事件")]
        public event EventHandler Close_Click;//窗口关闭时产生的事件

        //构造函数

        //----------------------------------------------------------------------
        // 功能说明：构造函数，初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public FaultMessage()
        {
            InitializeComponent();

            //

            if (null != GlobalWindows.MessageDisplay_Window)
            {
                GlobalWindows.MessageDisplay_Window.WindowClose_FaultMessage_GetData_Wait += new System.EventHandler(messageDisplayWindow_WindowClose_FaultMessage_GetData_Wait);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_FaultMessage_ClearAll_Confirm += new System.EventHandler(messageDisplayWindow_WindowClose_FaultMessage_ClearAll_Confirm);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_FaultMessage_ClearAll_Wait += new System.EventHandler(messageDisplayWindow_WindowClose_FaultMessage_ClearAll_Wait);//订阅事件
            }

            //

            Int32 i = 0;//循环控制变量

            FieldInfo[] fieldinfo = VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese.GetType().GetFields();//获取公共字段
            if (null != fieldinfo && 1 < fieldinfo.Length)//有效
            {
                sMessageText = new String[fieldinfo.Length - 1][];
                for (i = 0; i < fieldinfo.Length - 1; i++)
                {
                    sMessageText[i] = new String[7];
                }

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] = "正在获取故障信息数据";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] = "Getting Fault Messages";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][1] = "获取故障信息数据失败";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][1] = "Getting Fault Messages Failed";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][2] = "确定清除所有故障信息数据";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][2] = "Clear All Fault Messages";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][3] = "正在清除故障信息数据";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][3] = "Clearing Fault Messages";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][4] = "清除故障信息数据成功";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][4] = "Clearing Fault Messages Completed";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][5] = "清除故障信息数据失败";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][5] = "Clearing Fault Messages Failed";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][6] = "请等待";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][6] = "Please wait";
            }
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：Language属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("语言"), Category("FaultMessage 通用")]
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
        // 功能说明：SystemParameter属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("设备状态"), Category("FaultMessage 通用")]
        public VisionSystemClassLibrary.Class.System SystemParameter
        {
            get//读取
            {
                return system;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：DeviceNumber属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("相机设备数量（获取故障信息数据时使用）"), Category("FaultMessage 通用")]
        public Int32 DeviceNumber
        {
            get//读取
            {
                return iDeviceNumber;
            }
            set//设置
            {
                iDeviceNumber = value;
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
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：启动获取故障信息数据
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _StartGetData()
        {
            bMessageWindowShow = true;

            //显示信息对话框

            GlobalWindows.MessageDisplay_Window.WindowParameter = 89;//窗口特征数值
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.None;//不包含任何按钮
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] + "...";
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] + "...";
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = " ";
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = " ";
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][6] + "，" + iTimerCount.ToString();
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][6] + "，" + iTimerCount.ToString();

            GlobalWindows.MessageDisplay_Window.StartPosition = FormStartPosition.CenterScreen;
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.MessageDisplay_Window.TopMost = true;//将窗口置于顶层
            }
            else//其它
            {
                GlobalWindows.MessageDisplay_Window.Visible = true;//显示
            }

            timerData.Start();//启动定时器
        }

        //----------------------------------------------------------------------
        // 功能说明：获取故障信息数据
        // 输入参数：1.bSuccess：操作是否成功。取值范围：true，是；false，否
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _GetData(Boolean bSuccess)
        {
            if (bSuccess)//成功
            {
                iDeviceNumber--;
            }

            if (0 >= iDeviceNumber)//获取故障信息数据完成
            {
                bMessageWindowShow = false;

                iTimerCount = iTimerMaxCount;

                timerData.Stop();//关闭定时器

                //

                _SetData();

                //

                if (bClearAllFaultMessages)//清除所有故障信息数据
                {
                    bClearAllFaultMessagesSuccess = true;

                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = " ";
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = " ";
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][4];
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][4];
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = " ";
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = " ";
                }
                else
                {
                    if (GlobalWindows.TopMostWindows)//置顶
                    {
                        GlobalWindows.MessageDisplay_Window.TopMost = false;//取消置于顶层
                    }
                    else//其它
                    {
                        GlobalWindows.MessageDisplay_Window.Visible = false;//隐藏
                    }
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl._Reset();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：清除所有故障信息
        // 输入参数：1.bSuccess：操作是否成功。取值范围：true，是；false，否
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _ClearAllFaultMessages(Boolean bSuccess)
        {
            if (bSuccess)//成功
            {
                iDeviceNumber--;
            }

            if (0 >= iDeviceNumber)//清除故障信息数据完成
            {
                //不执行操作
            }
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
            customButtonCaption.Language = language;//标题

            customButtonClearAll.Language = language;//【CLEAR ALL】

            customButtonOption.Language = language;//【OPTION】

            customListFault_1.Language = language;//列表1
            customListFault_2.Language = language;//列表2
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：设置页面数据
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetData()
        {
            _InitFaultList_1();
            _SetFaultList_1();
            _SetPage_FaultList_1();

            _InitFaultList_2();
            _SetFaultList_2();
            _SetPage_FaultList_2();
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：设置FAULT列表1的故障名称项
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _SetFaultList_1_Item_0(Int32 iIndex)
        {
            if (VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese == language)//中文
            {
                customListFault_1.ItemData[iIndex].ItemText[0] = system.Camera[iIndex].CameraCHNName;
            }
            else if (VisionSystemClassLibrary.Enum.InterfaceLanguage.English == language)//英文
            {
                customListFault_1.ItemData[iIndex].ItemText[0] = system.Camera[iIndex].CameraENGName;
            }
            else//其它，默认中文
            {
                customListFault_1.ItemData[iIndex].ItemText[0] = system.Camera[iIndex].CameraCHNName;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：设置FAULT列表1的数值项
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _SetFaultList_1_Item_1(Int32 iIndex)
        {
            if (null == system.Camera[iIndex].FaultMessages)//无效
            {
                customListFault_1.ItemData[iIndex].ItemText[1] = 0.ToString();
            }
            else//有效
            {
                customListFault_1.ItemData[iIndex].ItemText[1] = system.Camera[iIndex].FaultMessages.Length.ToString();
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：初始化FAULT列表1
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _InitFaultList_1()
        {
            customListFault_1._ApplyListHeader();//应用列表头属性
            customListFault_1._ApplyListItem();//应用列表项属性
        }

        //-----------------------------------------------------------------------
        // 功能说明：设置FAULT列表1
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _SetFaultList_1()
        {
            customListFault_1._Apply(system.Camera.Count, 0, 0);//应用列表属性

            //添加列表项数据

            Int32 i = 0;//循环控制变量

            for (i = 0; i < customListFault_1.ItemDataNumber; i++)//列表项数据
            {
                _SetFaultList_1_Item_0(i);//设置FAULT列表1的名称
                _SetFaultList_1_Item_1(i);//设置FAULT列表1的数值
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：设置FAULT列表1数据
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetPage_FaultList_1()
        {
            customListFault_1._SetPage();//设置列表项数据

            _SetPreviousNextButton_FaultList_1();
        }

        //----------------------------------------------------------------------
        // 功能说明：设置FAULT列表1的【Previous Page】、【Next Page】按钮
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetPreviousNextButton_FaultList_1()
        {
            if (1 < customListFault_1.TotalPage)//多于一页
            {
                customButtonPreviousPage_List_1.Visible = true;//【Previous Page】
                customButtonNextPage_List_1.Visible = true;//【Next Page】
            }
            else//小于等于一页
            {
                customButtonPreviousPage_List_1.Visible = false;//【Previous Page】
                customButtonNextPage_List_1.Visible = false;//【Next Page】
            }
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：设置FAULT列表2的故障名称项
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _SetFaultList_2_Item_0(Int32 iItemIndex)
        {
            customListFault_2.ItemData[iItemIndex].ItemText[0] = VisionSystemClassLibrary.Class.System._GetFaultMessage(language, system.Camera[customListFault_1.CurrentListIndex].FaultMessages[iItemIndex]);
        }

        //-----------------------------------------------------------------------
        // 功能说明：设置FAULT列表2的数值项
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _SetFaultList_2_Item_1(Int32 iItemIndex)
        {
            customListFault_2.ItemData[iItemIndex].ItemText[1] = VisionSystemClassLibrary.Class.Shift._GetDateTime(system.Camera[customListFault_1.CurrentListIndex].FaultMessages[iItemIndex].TimeData);
        }

        //-----------------------------------------------------------------------
        // 功能说明：初始化FAULT列表2
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _InitFaultList_2()
        {
            customListFault_2._ApplyListHeader();//应用列表头属性
            customListFault_2._ApplyListItem();//应用列表项属性
        }

        //-----------------------------------------------------------------------
        // 功能说明：设置FAULT列表2
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _SetFaultList_2()
        {
            if (null != system.Camera[customListFault_1.CurrentListIndex].FaultMessages)//有效
            {
                customListFault_2._Apply(system.Camera[customListFault_1.CurrentListIndex].FaultMessages.Length, 0, 0);//应用列表属性

                //添加列表项数据

                Int32 i = 0;//循环控制变量

                for (i = system.Camera[customListFault_1.CurrentListIndex].FaultMessages.Length - 1; i >= 0; i--)//列表项数据
                {
                    _SetFaultList_2_Item_0(i);//设置FAULT列表2的名称

                    _SetFaultList_2_Item_1(i);//设置FAULT列表2的数值项

                    //

                    customListFault_2.ItemData[i].ItemFlag = i;
                }
            }
            else//无效
            {
                customListFault_2._Apply(0, 0, 0);//应用列表属性
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：设置FAULT列表2数据
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetPage_FaultList_2()
        {
            customListFault_2._SetPage();//设置列表项数据

            _SetPreviousNextButton_FaultList_2();
        }

        //----------------------------------------------------------------------
        // 功能说明：设置FAULT列表2的【Previous Page】、【Next Page】按钮
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetPreviousNextButton_FaultList_2()
        {
            if (1 < customListFault_2.TotalPage)//多于一页
            {
                customButtonPreviousPage_List_2.Visible = true;//【Previous Page】
                customButtonNextPage_List_2.Visible = true;//【Next Page】
            }
            else//小于等于一页
            {
                customButtonPreviousPage_List_2.Visible = false;//【Previous Page】
                customButtonNextPage_List_2.Visible = false;//【Next Page】
            }
        }

        //事件

        //----------------------------------------------------------------------
        // 功能说明：点击FAULT列表1事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customListFault_1_CustomListItem_Click(object sender, EventArgs e)
        {
            _InitFaultList_2();
            _SetFaultList_2();
            _SetPage_FaultList_2();
        }

        //----------------------------------------------------------------------
        // 功能说明：点击FAULT列表2事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customListFault_2_CustomListItem_Click(object sender, EventArgs e)
        {
            //不执行操作
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【Previous Page】按钮事件，执行相应的操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonPreviousPage_List_1_CustomButton_Click(object sender, EventArgs e)
        {
            customListFault_1._ClickPage(true);//翻页，上一页
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【Next Page】按钮事件，执行相应的操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonNextPage_List_1_CustomButton_Click(object sender, EventArgs e)
        {
            customListFault_1._ClickPage(false);//翻页，下一页
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【Previous Page】按钮事件，执行相应的操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonPreviousPage_List_2_CustomButton_Click(object sender, EventArgs e)
        {
            customListFault_2._ClickPage(true);//翻页，上一页
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【Next Page】按钮事件，执行相应的操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonNextPage_List_2_CustomButton_Click(object sender, EventArgs e)
        {
            customListFault_2._ClickPage(false);//翻页，下一页
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：点击【CLEAR ALL】按钮事件，执行相应的操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonClearAll_CustomButton_Click(object sender, EventArgs e)
        {
            GlobalWindows.MessageDisplay_Window.WindowParameter = 90;//窗口特征数值
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.OkCancel;//不包含任何按钮
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][2] + "？";
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][2] + "？";

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
        // 功能说明：点击【OPTION】按钮事件，执行相应的操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonOption_CustomButton_Click(object sender, EventArgs e)
        {
            GlobalWindows.FaultMessageOption_Window.FaultMessageOptionControl.Language = language;
            GlobalWindows.FaultMessageOption_Window.FaultMessageOptionControl.FaultMessageState = VisionSystemClassLibrary.Class.System._GetMachineFaultEnableStateArray(VisionSystemClassLibrary.Class.System.MachineFaultEnableState);

            GlobalWindows.FaultMessageOption_Window.StartPosition = FormStartPosition.CenterScreen;
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.FaultMessageOption_Window.TopMost = true;//将窗口置于顶层
            }
            else//其它
            {
                GlobalWindows.FaultMessageOption_Window.Visible = true;//显示
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【OK】按钮事件，执行相应的操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonOk_CustomButton_Click(object sender, EventArgs e)
        {
            //事件

            if (null != Close_Click)//有效
            {
                Close_Click(this, new CustomEventArgs());
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：FAULT MESSAGE，获取故障信息数据，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_FaultMessage_GetData_Wait(object sender, EventArgs e)
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
                //未成功获取故障信息数据

                bMessageWindowShow = false;

                //事件

                if (null != Close_Click)//有效
                {
                    Close_Click(this, new CustomEventArgs());
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：FAULT MESSAGE，清除所有故障信息数据确认，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_FaultMessage_ClearAll_Confirm(object sender, EventArgs e)
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
                bClearAllFaultMessages = true;

                bClearAllFaultMessagesSuccess = false;

                //显示等待窗口

                bMessageWindowShow = true;

                GlobalWindows.MessageDisplay_Window.WindowParameter = 91;//窗口特征数值
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.None;//不包含任何按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][3] + "...";
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][3] + "...";
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = " ";
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = " ";
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][6] + "，" + iTimerCount.ToString();
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][6] + "，" + iTimerCount.ToString();

                GlobalWindows.MessageDisplay_Window.StartPosition = FormStartPosition.CenterScreen;
                if (GlobalWindows.TopMostWindows)//置顶
                {
                    GlobalWindows.MessageDisplay_Window.TopMost = true;//将窗口置于顶层
                }
                else//其它
                {
                    GlobalWindows.MessageDisplay_Window.Visible = true;//显示
                }

                timerData.Start();//启动定时器

                //事件

                if (null != ClearAllFaultMessages)//有效
                {
                    ClearAllFaultMessages(this, new CustomEventArgs());
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：FAULT MESSAGE，清除所有故障信息数据，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_FaultMessage_ClearAll_Wait(object sender, EventArgs e)
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
                bClearAllFaultMessages = false;

                bMessageWindowShow = false;

                //

                if (!bClearAllFaultMessagesSuccess)//失败
                {
                    //事件

                    if (null != Close_Click)//有效
                    {
                        Close_Click(this, new CustomEventArgs());
                    }
                }
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：定时器事件，获取统计记录，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void timerData_Tick(object sender, EventArgs e)
        {
            if (bMessageWindowShow)//显示
            {
                iTimerCount--;

                if (0 >= iTimerCount)//超时
                {
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                    if (bClearAllFaultMessages)//清除故障信息
                    {
                        bClearAllFaultMessages = false;

                        bClearAllFaultMessagesSuccess = true;

                        GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = " ";
                        GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = " ";
                        GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][5];
                        GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][5];
                        GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = " ";
                        GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = " ";
                    } 
                    else
                    {
                        GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = " ";
                        GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = " ";
                        GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][1];
                        GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][1];
                        GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = " ";
                        GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = " ";
                    }

                    timerData.Stop();//关闭定时器

                    iTimerCount = iTimerMaxCount;
                }
                else//计数
                {
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][6] + "，" + iTimerCount.ToString();
                    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][6] + "，" + iTimerCount.ToString();
                }
            }
        }
    }
}