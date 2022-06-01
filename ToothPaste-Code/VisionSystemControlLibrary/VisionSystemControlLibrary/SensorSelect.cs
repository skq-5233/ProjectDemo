/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：NetCheck.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：SensorSelect控件

原作者：视觉检测团队
完成日期：2020/01/10
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

namespace VisionSystemControlLibrary
{
    public partial class SensorSelect : UserControl
    {
        //该控件为传感器选择

        private VisionSystemClassLibrary.Enum.InterfaceLanguage language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;//属性，语言

        //

        private Boolean bClickSelectAllButton = true;//是否按下【SELECT ALL】按钮。取值范围：true，是；false，否

        //

        private Boolean bEnterNewValue = false;//属性（只读），是否输入了新的数值或密码输入是否正确。取值范围：true，是；false，否

        //

        VisionSystemClassLibrary.Struct.CameraParameter cameraParameter = new VisionSystemClassLibrary.Struct.CameraParameter();  //创建CameraParameter类库的对象
        VisionSystemClassLibrary.Struct.CameraParameter cameraParameterTemp = new VisionSystemClassLibrary.Struct.CameraParameter();  //创建CameraParameter类库的对象缓存

        //

        [Browsable(true), Description("窗口关闭时产生的事件"), Category("FaultMessageOption 事件")]
        public event EventHandler Close_Click;//窗口关闭时产生的事件

        //

        //----------------------------------------------------------------------
        // 功能说明：系统默认调用，构造函数，初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------

        public SensorSelect()
        {
            InitializeComponent();
        }

        //属性

        //-----------------------------------------------------------------------
        // 功能说明：Language属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("语言"), Category("SensorSelect 通用")]
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
        // 功能说明：EnterNewValue属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("是否输入了新的数值或密码输入是否正确。取值范围：true，是；false，否"), Category("SensorSelect 通用")]
        public Boolean EnterNewValue
        {
            get//读取
            {
                return bEnterNewValue;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：SensorSelectState属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("返回传感器选中标记"), Category("SensorSelect 通用")]
        public Int32 SensorSelectState
        {
            get//读取
            {
                return cameraParameter.SensorSelectState;
            }
        }

        //函数

        //----------------------------------------------------------------------
        // 功能说明：应用设置的属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _Properties(VisionSystemClassLibrary.Struct.CameraParameter camera_parameter)
        {
            bEnterNewValue = false;

            cameraParameter = camera_parameter;

            cameraParameter.SensorSelectState = 0;
            for (Int32 i = 0; i < cameraParameter.SensorAdjustValue.Length; i++)//遍历
            {
                cameraParameter.SensorSelectState |= (0x01 << i);
            }

            cameraParameter._CopyTo(ref cameraParameterTemp);

            //

            _SetSelectAllButton();

            _InitList();

            _SetLanguage();//设置语言
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

            customButtonSelectAll.Language = language;//选择所有

            customList.Language = language;//列表
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：设置【SELECT ALL】按钮
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetSelectAllButton()
        {
            Int32 i = 0;//循环控制变量

            for (i = 0; i < cameraParameter.SensorAdjustValue.Length; i++)//遍历
            {
                if ((cameraParameter.SensorSelectState & (0x01 << i)) == 0) //当前存在未被选中传感器
                {
                    break;
                }
            }

            if (i >= cameraParameter.SensorAdjustValue.Length)//所有使能
            {
                bClickSelectAllButton = true;
            }
            else//存在禁止
            {
                bClickSelectAllButton = false;
            }

            //

            if (bClickSelectAllButton)//按下
            {
                customButtonSelectAll.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
            }
            else//未按下
            {
                customButtonSelectAll.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
            }
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
            customList._Apply(cameraParameter.SensorAdjustValue.Length);//应用列表属性

            _AddItemData();//添加列表项数据

            _SetPage();//设置列表数据
        }

        //----------------------------------------------------------------------
        // 功能说明：添加列表项数据
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetItemData_0(Int32 iIndex)
        {
            try
            {
                switch (language)
                {
                    case VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese://中文

                        customList.ItemData[iIndex].ItemText[0] = VisionSystemClassLibrary.String.TextData.SerialPortParameter_CHN[iIndex];

                        break;
                    case VisionSystemClassLibrary.Enum.InterfaceLanguage.English://英文

                        customList.ItemData[iIndex].ItemText[0] = VisionSystemClassLibrary.String.TextData.SerialPortParameter_ENG[iIndex];

                        break;
                    default://其它，默认中文

                        customList.ItemData[iIndex].ItemText[0] = VisionSystemClassLibrary.String.TextData.SerialPortParameter_CHN[iIndex];

                        break;
                }
            }
            catch (System.Exception ex)
            {
                //不执行操作
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：设置列表的选择项
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _SetItemData_1(Int32 iIndex, Boolean bSelected)
        {
            customList.ItemData[iIndex].ItemDataDisplay[1] = !(bSelected);//图标（Selectd列）
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
            Int32 j = 0;//循环控制变量

            for (i = 0; i < customList.ItemDataNumber; i++)//列表项数据
            {
                _SetItemData_0(i);//设置列表的故障信息名称

                //

                customList.ItemData[i].ItemDataDisplay[0] = true;//文本

                customList.ItemData[i].ItemIconIndex[0] = -1;//图标
                customList.ItemData[i].ItemIconIndex[1] = 3;//图标

                if (bClickSelectAllButton)//选择所有
                {
                    _SetItemData_1(i, true);//设置列表的选择项

                    j++;
                }
                else//选择当前
                {
                    if ((cameraParameter.SensorSelectState & (0x01 << i)) != 0)//传感器被选中
                    {
                        _SetItemData_1(i, true);//设置列表的选择项

                        j++;
                    }
                    else//传感器未被选中
                    {
                        _SetItemData_1(i, false);//设置列表的选择项
                    }
                }

                //

                customList.ItemData[i].ItemFlag = i;
            }

            customList.SelectedItemNumber = j;
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
            if (0 == customList.SelectedItemNumber)//无工具被选择
            {
                //选择当前工具

                customList.ItemData[customList.CurrentListIndex].ItemDataDisplay[1] = false;
                customList.SelectedItemNumber = 1;

                customList._Refresh(customList.Index_Page);//刷新
            }

            Boolean iSelectState = !(customList.ItemData[customList.CurrentListIndex].ItemDataDisplay[1]);

            if (iSelectState)//传感器被选中
            {
                cameraParameter.SensorSelectState |= (0x01 << customList.CurrentListIndex);
            }
            else
            {
                cameraParameter.SensorSelectState &= (~(0x01 << customList.CurrentListIndex));
            }


            //

            _SetSelectAllButton();
        }

        //----------------------------------------------------------------------
        // 功能说明：【PREVIOUS PAGE】按钮点击事件，执行相关操作
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
        // 功能说明：【NEXT PAGE】按钮点击事件，执行相关操作
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
        // 功能说明：【SELECT ALL】按钮点击事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonSelectAll_CustomButton_Click(object sender, EventArgs e)
        {
            bClickSelectAllButton = !bClickSelectAllButton;

            customList._SelectAll(bClickSelectAllButton);

            //

            Int32 i = 0;//循环控制变量
            Boolean bSelected = false;//临时变量

            if (bClickSelectAllButton)//选择所有
            {
                bSelected = true;

                for (i = 0; i < cameraParameter.SensorAdjustValue.Length; i++)//遍历
                {
                    cameraParameter.SensorSelectState |= (0x01 << i);
                }
            }
            else//其它
            {
                bSelected = false;
                
                cameraParameter.SensorSelectState = 0;
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
            cameraParameter._CopyTo(ref cameraParameterTemp);

            bEnterNewValue = true;

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
            cameraParameterTemp._CopyTo(ref cameraParameter);

            bEnterNewValue = false;

            //事件

            if (null != Close_Click)//有效
            {
                Close_Click(this, new CustomEventArgs());
            }
        }
    }
}
