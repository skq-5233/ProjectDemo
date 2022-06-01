/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：CigaretteSort.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：烟支排列选择控件

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
    public partial class CigaretteSort : UserControl
    {
        //CigaretteSort控件

        private VisionSystemClassLibrary.Enum.InterfaceLanguage language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;//属性，语言

        //

        private VisionSystemClassLibrary.Class.System system = new VisionSystemClassLibrary.Class.System();//属性（只读），系统

        private VisionSystemClassLibrary.Class.System parameter_temp = new VisionSystemClassLibrary.Class.System();//设备（临时变量，保存修改的参数，主要使用其中的系统参数数据）

        //

        [Browsable(true), Description("窗口关闭时产生的事件"), Category("CigaretteSort 事件")]
        public event EventHandler Close_Click;//窗口关闭时产生的事件

        //构造函数

        //----------------------------------------------------------------------
        // 功能说明：构造函数，初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------

        public CigaretteSort()
        {
            InitializeComponent();
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

            system._CopySystemParameterTo(parameter_temp);

            _SetData(); //初始化列表信息
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
            
            customListFault_1.Language = language;//列表1
            customListValueList.Language = language;//列表2
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
            Int32 i = 0;
            Int32 j = 0;

            for (i = 0; i < parameter_temp.SystemCameraConfiguration.Length; i++) 
            {
                if (parameter_temp.SystemCameraConfiguration[i].IsSerialPort) //当前为串口
                {
                    j++;
                }
            }
            customListFault_1._Apply(j);//应用列表属性

            //添加列表项数据

            j = 0;
            for (i = 0; i < customListFault_1.ItemDataNumber; i++)//列表项数据
            {
                while (j < parameter_temp.SystemCameraConfiguration.Length)
                {
                    if (parameter_temp.SystemCameraConfiguration[j].IsSerialPort) //当前为串口
                    {
                        if (VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese == language)//中文
                        {
                            customListFault_1.ItemData[i].ItemText[0] = parameter_temp.SystemCameraConfiguration[j].CameraCHNName;
                            customListFault_1.ItemData[i].ItemFlag = i;
                        }
                        else if (VisionSystemClassLibrary.Enum.InterfaceLanguage.English == language)//英文
                        {
                            customListFault_1.ItemData[i].ItemText[0] = parameter_temp.SystemCameraConfiguration[j].CameraENGName;
                            customListFault_1.ItemData[i].ItemFlag = i;
                        }
                        else//其它，默认中文
                        {
                            customListFault_1.ItemData[i].ItemText[0] = parameter_temp.SystemCameraConfiguration[j].CameraCHNName;
                            customListFault_1.ItemData[i].ItemFlag = i;
                        }

                        j++;
                        break;
                    }
                    else
                    {
                        j++;
                    }
                }
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
            customListValueList.ItemData[iItemIndex].ItemText[0] = ((VisionSystemClassLibrary.Enum.TobaccoSortType)(iItemIndex + 1)).ToString();
            customListValueList.ItemData[iItemIndex].ItemIconIndex[0] = -1;
            customListValueList.ItemData[iItemIndex].ItemDataDisplay[0] = true;//文本
        }

        //-----------------------------------------------------------------------
        // 功能说明：设置FAULT列表2的数值项
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _SetFaultList_2_Item_1(Int32 iItemIndex, Int32 iSystemCameraConfiguration)
        {
            customListValueList.ItemData[iItemIndex].ItemIconIndex[1] = 0;

            if (((Int32)parameter_temp.SystemCameraConfiguration[iSystemCameraConfiguration].TobaccoSortType_E - 1) == iItemIndex) //当前选中相机
            {
                customListValueList.ItemData[iItemIndex].ItemDataDisplay[1] = false;//图标（显示）
            }
            else
            {
                customListValueList.ItemData[iItemIndex].ItemDataDisplay[1] = true;//图标（隐藏）
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：初始化FAULT列表2
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _InitFaultList_2()
        {
            customListValueList._ApplyListHeader();//应用列表头属性
            customListValueList._ApplyListItem();//应用列表项属性
        }

        //-----------------------------------------------------------------------
        // 功能说明：设置FAULT列表2
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _SetFaultList_2()
        {
            Int32 iTobaccoSortTypeCount = System.Enum.GetValues(typeof(VisionSystemClassLibrary.Enum.TobaccoSortType)).Length;

            if ((0 < iTobaccoSortTypeCount) && (customListFault_1.CurrentListIndex >= 0))//列表选择项有效
            {
                Int32 i = 0; //循环变量

                for (i = 0; i < parameter_temp.SystemCameraConfiguration.Length; i++)
                {
                    if (VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese == language)//中文
                    {
                        if (customListFault_1.ItemData[customListFault_1.CurrentListIndex].ItemText[0] == parameter_temp.SystemCameraConfiguration[i].CameraCHNName)
                        {
                            break;
                        }
                    }
                    else if (VisionSystemClassLibrary.Enum.InterfaceLanguage.English == language)//英文
                    {
                        if (customListFault_1.ItemData[customListFault_1.CurrentListIndex].ItemText[0] == parameter_temp.SystemCameraConfiguration[i].CameraENGName)
                        {
                            break;
                        }
                    }
                    else//其它，默认中文
                    {
                        if (customListFault_1.ItemData[customListFault_1.CurrentListIndex].ItemText[0] == parameter_temp.SystemCameraConfiguration[i].CameraCHNName)
                        {
                            break;
                        }
                    }
                }

                customListValueList._Apply(iTobaccoSortTypeCount);//应用列表属性

                //添加列表项数据

                Int32 j = 0;//循环控制变量

                for (j = 0; j < iTobaccoSortTypeCount; j++)//列表项数据
                {
                    _SetFaultList_2_Item_0(j);//设置FAULT列表2的名称

                    _SetFaultList_2_Item_1(j, i);//设置FAULT列表2的数值项

                    //

                    customListValueList.ItemData[j].ItemFlag = j;
                }
            }
            else//无效
            {
                customListValueList._Apply(0);//应用列表属性
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
            customListValueList.ItemIconNumber = 1;
            customListValueList.BitmapIcon[0] = VisionSystemControlLibrary.Properties.Resources.CustomListItem_Marked;
            customListValueList.SelectionColumnIndex = 1;

            customListValueList._SetPage();//设置列表项数据

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
            if (1 < customListValueList.TotalPage)//多于一页
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
            if (customListFault_1.CurrentListIndex >= 0) //列表选择项有效
            {
                _InitFaultList_2();
                _SetFaultList_2();
                _SetPage_FaultList_2();
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击FAULT列表2事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customListValueList_CustomListItem_Click(object sender, EventArgs e)
        {
            if (customListFault_1.CurrentListIndex >= 0) //选择项有效
            {
                Int32 i = 0; //循环变量

                for (i = 0; i < parameter_temp.SystemCameraConfiguration.Length; i++)
                {
                    if (VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese == language)//中文
                    {
                        if (customListFault_1.ItemData[customListFault_1.CurrentListIndex].ItemText[0] == parameter_temp.SystemCameraConfiguration[i].CameraCHNName)
                        {
                            break;
                        }
                    }
                    else if (VisionSystemClassLibrary.Enum.InterfaceLanguage.English == language)//英文
                    {
                        if (customListFault_1.ItemData[customListFault_1.CurrentListIndex].ItemText[0] == parameter_temp.SystemCameraConfiguration[i].CameraENGName)
                        {
                            break;
                        }
                    }
                    else//其它，默认中文
                    {
                        if (customListFault_1.ItemData[customListFault_1.CurrentListIndex].ItemText[0] == parameter_temp.SystemCameraConfiguration[i].CameraCHNName)
                        {
                            break;
                        }
                    }
                }

                parameter_temp.SystemCameraConfiguration[i].TobaccoSortType_E = (VisionSystemClassLibrary.Enum.TobaccoSortType)(customListValueList.CurrentListIndex + 1);

                for (Int32 j = 0; j < customListValueList.ItemDataNumber; j++)//列表项数据
                {
                    _SetFaultList_2_Item_1(j, i);//设置FAULT列表2的数值项
                }

                customListValueList._Refresh();
            }
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
            customListValueList._ClickPage(true);//翻页，上一页
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
            customListValueList._ClickPage(false);//翻页，下一页
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
            parameter_temp._CopySystemParameterTo(system);

            //事件

            if (null != Close_Click)//有效
            {
                Close_Click(this, new CustomEventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：【CANCEL】按钮点击事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonCancel_CustomButton_Click(object sender, EventArgs e)
        {
            //事件

            if (null != Close_Click)//有效
            {
                Close_Click(this, new CustomEventArgs());
            }
        }
    }
}
