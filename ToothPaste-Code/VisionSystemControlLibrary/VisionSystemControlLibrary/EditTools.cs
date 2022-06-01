/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：EditTools.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述： EDIT TOOLS控件

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
    public partial class EditTools : UserControl
    {
        //该控件为QUALITY CHECK页面中的EDIT TOOLS窗口

        private VisionSystemClassLibrary.Enum.InterfaceLanguage language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;//属性，语言

        private VisionSystemClassLibrary.Class.Camera camera = new VisionSystemClassLibrary.Class.Camera();//属性（只读），相机

        //

        private String[][] sMessageText = new String[2][];//提示信息对话框上显示的文本（[语言][包含的文本]）

        //

        [Browsable(true), Description("点击【返回】按钮时产生的事件"), Category("EditTools 事件")]
        public event EventHandler Close_Click;//点击【返回】按钮时产生的事件

        //

        //----------------------------------------------------------------------
        // 功能说明：系统默认调用，构造函数，初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public EditTools()
        {
            InitializeComponent();

            //由于该控件仅有一个实例，因此可以在此处进行事件的订阅

            if (null != GlobalWindows.StandardKeyboard_Window)
            {
                GlobalWindows.StandardKeyboard_Window.WindowClose_EditTools_NewTool += new System.EventHandler(standardKeyboardWindow_WindowClose_EditTools_NewTool);//订阅事件
                GlobalWindows.StandardKeyboard_Window.WindowClose_EditTools_CopyTool += new System.EventHandler(standardKeyboardWindow_WindowClose_EditTools_CopyTool);//订阅事件
                GlobalWindows.StandardKeyboard_Window.WindowClose_EditTools_RenameTool += new System.EventHandler(standardKeyboardWindow_WindowClose_EditTools_RenameTool);//订阅事件
            }

            if (null != GlobalWindows.MessageDisplay_Window)
            {
                GlobalWindows.MessageDisplay_Window.WindowClose_EditTools_NewTool_Success += new System.EventHandler(messageDisplayWindow_WindowClose_EditTools_NewTool_Success);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_EditTools_NewTool_Failure_1 += new System.EventHandler(messageDisplayWindow_WindowClose_EditTools_NewTool_Failure_1);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_EditTools_NewTool_Failure_2 += new System.EventHandler(messageDisplayWindow_WindowClose_EditTools_NewTool_Failure_2);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_EditTools_CopyTool_Success += new System.EventHandler(messageDisplayWindow_WindowClose_EditTools_CopyTool_Success);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_EditTools_CopyTool_Failure_1 += new System.EventHandler(messageDisplayWindow_WindowClose_EditTools_CopyTool_Failure_1);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_EditTools_CopyTool_Failure_2 += new System.EventHandler(messageDisplayWindow_WindowClose_EditTools_CopyTool_Failure_2);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_EditTools_RenameTool_Success += new System.EventHandler(messageDisplayWindow_WindowClose_EditTools_RenameTool_Success);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_EditTools_RenameTool_Failure_1 += new System.EventHandler(messageDisplayWindow_WindowClose_EditTools_RenameTool_Failure_1);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_EditTools_DeleteTool_Confirm += new System.EventHandler(messageDisplayWindow_WindowClose_EditTools_DeleteTool_Confirm);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_EditTools_DeleteTool_Success += new System.EventHandler(messageDisplayWindow_WindowClose_EditTools_DeleteTool_Success);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_EditTools_DeleteTool_Failure += new System.EventHandler(messageDisplayWindow_WindowClose_EditTools_DeleteTool_Failure);//订阅事件
            }
            //

            FieldInfo[] fieldinfo = VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese.GetType().GetFields();//获取公共字段
            if (null != fieldinfo && 1 < fieldinfo.Length)//有效
            {
                Int32 i = 0;//循环控制变量

                sMessageText = new String[fieldinfo.Length - 1][];
                for (i = 0; i < fieldinfo.Length - 1; i++)
                {
                    sMessageText[i] = new String[13];
                }

                //

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] = "完成";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] = "Completed";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][1] = "失败";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][1] = "Failed";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][2] = "将工具";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][2] = "Copy of";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][3] = "复制为工具";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][3] = "to";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][4] = "将工具";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][4] = "Rename of";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][5] = "重命名为工具";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][5] = "to";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][6] = "新建工具";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][6] = "New Tool";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][7] = "复制";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][7] = "Copy";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][8] = "重命名";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][8] = "Rename";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][9] = "已经存在";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][9] = "already exists";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][10] = "最大品牌数目为";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][10] = "Maximum brand number is";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][11] = "确定删除";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][11] = "Delete";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][12] = "删除";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][12] = "Deletion of";
            }
        }

        //属性

        //-----------------------------------------------------------------------
        // 功能说明：Language属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("语言"), Category("EditTools 通用")]
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
        // 功能说明：SelectedCamera属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("相机"), Category("EditTools 通用")]
        public VisionSystemClassLibrary.Class.Camera SelectedCamera//属性
        {
            get//读取
            {
                return camera;
            }
        }

        //函数

        //----------------------------------------------------------------------
        // 功能说明：应用设置的属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _Properties(VisionSystemClassLibrary.Class.Camera camera_parameter)
        {
            camera = camera_parameter;

            //

            _SetTools();//应用属性设置
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
            customButtonNewTool.Language = language;//【NEW TOOL】
            customButtonCopyTool.Language = language;//【COPY TOOL】
            customButtonRenameTool.Language = language;//【RENAME TOOL】
            customButtonDeleteTool.Language = language;//【DELETE TOOL】

            //

            customButtonClose.Language = language;//【Close】
            customButtonPreviousPage.Language = language;//【Previous Page】
            customButtonNextPage.Language = language;//【Next Page】

            //

            customButtonCaption.Language = language;//标题文本
            customButtonMessage.Language = language;//控件文本

            //

            customList.Language = language;//列表语言
        }

        //----------------------------------------------------------------------
        // 功能说明：应用设置完成的属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _Apply()
        {
            customList._ApplyListHeader();//应用列表头属性
            customList._ApplyListItem();//应用列表项属性

            customList._Apply(camera.Tools.Count);//应用列表属性

            //

            _AddListItemData();//添加列表项数据

            _SetPage();//设置列表项数据
        }

        //----------------------------------------------------------------------
        // 功能说明：应用设置的属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetTools()
        {
            _SetLanguage();//设置控件显示的文本

            _Apply();//应用设置的属性
        }

        //----------------------------------------------------------------------
        // 功能说明：将设备信息添加至列表中
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _AddListItemData()
        {
            //Int32 i = 0;//循环控制变量

            //for (i = 0; i < customList.ItemDataNumber; i++)//列表项数据
            //{
            //    if (VisionSystemClassLibrary.Enum.BrandType.Master == brand.SystemBrandData[i].Type)//MASTER类型的品牌
            //    {
            //        customList.ItemData[i].ItemText[0] = brand.SystemBrandData[i].Name + "（M）";
            //    }
            //    else//其它
            //    {
            //        customList.ItemData[i].ItemText[0] = brand.SystemBrandData[i].Name;
            //    }

            //    customList.ItemData[i].ItemFlag = i;
            //}
        }

        //----------------------------------------------------------------------
        // 功能说明：设置当前页中的控件
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetPage()
        {
            customList._SetPage();//设置列表项

            //

            _SetFunctionalButton();//设置按钮

            //

            if (1 < customList.TotalPage)//大于1页
            {
                customButtonPreviousPage.Visible = true;//显示【Previous Page】按钮
                customButtonNextPage.Visible = true;//显示【Next Page】按钮
            }
            else//1页
            {
                customButtonPreviousPage.Visible = false;//隐藏【Previous Page】按钮
                customButtonNextPage.Visible = false;//隐藏【Next Page】按钮
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：翻页，选择列表中的项，新建工具、复制工具，重命名工具、删除工具时，设置【NEW TOOL】、【COPY BRAND】、【RENAME BRAND】、【DELETE BRAND】按钮的状态
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetFunctionalButton()
        {
            //if (customList.ItemDataNumber > 0)//存在有效的列表项
            //{
            //    if (0 <= customList.CurrentListIndex)//列表中存在选择的项
            //    {
            //        if (VisionSystemClassLibrary.Enum.BrandType.Master == brand.SystemBrandData[customList.CurrentListIndex].Type)//当前选择的项对应的品牌类型为MASTER
            //        {
            //            customButtonRenameTool.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【RENAME TOOL】
            //            customButtonDeleteTool.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【DELETE TOOL】
            //        }
            //        else//当前选择的项对应的品牌类型为General
            //        {
            //            customButtonRenameTool.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【RENAME TOOL】
            //            customButtonDeleteTool.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【DELETE TOOL】
            //        }

            //        customButtonCopyTool.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【COPY TOOL】
            //    }
            //    else//列表中不存在任何选择的项
            //    {
            //        customButtonCopyTool.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【COPY TOOL】
            //        customButtonRenameTool.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【RENAME TOOL】
            //        customButtonDeleteTool.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【DELETE TOOL】
            //    }
            //}
            //else//不存在有效的列表项
            //{
            //    customButtonCopyTool.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【COPY TOOL】
            //    customButtonRenameTool.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【RENAME TOOL】
            //    customButtonDeleteTool.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【DELETE TOOL】
            //}
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：检查新建、拷贝、重命名的工具是否重名
        // 输入参数：1.sNewTool：新建、拷贝、重命名后的品牌名称
        // 输出参数：无
        // 返回值：是否重名。取值范围：true，否；false，是
        //----------------------------------------------------------------------
        private bool _CheckToolName(string sNewTool)
        {
            Int32 i = 0;//循环控制变量
            Boolean bReturn = true;//返回值

            for (i = 0; i < customList.ItemDataNumber; i++)
            {
                if ((sNewTool == camera.Tools[i].ToolsENGName) || (sNewTool == camera.Tools[i].ToolsCHNName))//存在重名
                {
                    break;
                }
            }
            if (i < customList.ItemDataNumber)//存在重名
            {
                bReturn = false;
            }
            else//不存在重名
            {
                bReturn = true;
            }

            return bReturn;
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：新建工具
        // 输入参数：1.sNewTool：新创建的工具
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _NewTool(string sNewTool)
        {
            //if (brand._CopyBrand(sSourceBrand, sNewBrand))//成功
            //{
            //    brand.SystemBrandData[customList.ItemDataNumber].Name = sNewBrand;//品牌名称
            //    brand.SystemBrandData[customList.ItemDataNumber].Type = VisionSystemClassLibrary.Enum.BrandType.General;//品牌类型

            //    //

            //    brand.BrandNumber++;

            //    brand._Write(customList.ItemDataNumber, 1);//写入文件

            //    //

            //    customList._Apply(brand.BrandNumber, customList.ItemDataNumber, customList.ItemDataNumber);//应用列表属性

            //    _AddListItemData();//添加列表项数据

            //    _SetPage();//设置列表项数据

            //    //显示信息对话框

            //    GlobalWindows.MessageDisplay_Window.WindowParameter = 18;//窗口特征数值
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][6] + " " + sSourceBrand;
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][6] + " " + sSourceBrand;
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][7] + " " + sNewBrand;
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][7] + " " + sNewBrand;
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][1];
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][1];

            //    GlobalWindows.MessageDisplay_Window.StartPosition = FormStartPosition.CenterScreen;
            //    if (GlobalWindows.TopMostWindows)//置顶
            //    {
            //        GlobalWindows.MessageDisplay_Window.TopMost = true;//将窗口置于顶层
            //    }
            //    else//其它
            //    {
            //        GlobalWindows.MessageDisplay_Window.Visible = true;//显示
            //    }

            //    //

            //    _SetFunctionalButton();//设置按钮
            //}
            //else//失败
            //{
            //    //显示信息对话框

            //    GlobalWindows.MessageDisplay_Window.WindowParameter = 19;//窗口特征数值
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][6] + " " + sSourceBrand;
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][6] + " " + sSourceBrand;
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][7] + " " + sNewBrand;
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][7] + " " + sNewBrand;
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][2];
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][2];

            //    GlobalWindows.MessageDisplay_Window.StartPosition = FormStartPosition.CenterScreen;
            //    if (GlobalWindows.TopMostWindows)//置顶
            //    {
            //        GlobalWindows.MessageDisplay_Window.TopMost = true;//将窗口置于顶层
            //    }
            //    else//其它
            //    {
            //        GlobalWindows.MessageDisplay_Window.Visible = true;//显示
            //    }
            //}
        }

        //----------------------------------------------------------------------
        // 功能说明：拷贝工具
        // 输入参数：1.sNewTool：新创建的工具
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _CopyTool(string sNewTool)
        {
            //string sSourceBrand = brand.SystemBrandData[customList.CurrentListIndex].Name;//拷贝的源品牌

            //if (brand._CopyBrand(sSourceBrand, sNewBrand))//成功
            //{
            //    brand.SystemBrandData[customList.ItemDataNumber].Name = sNewBrand;//品牌名称
            //    brand.SystemBrandData[customList.ItemDataNumber].Type = VisionSystemClassLibrary.Enum.BrandType.General;//品牌类型

            //    //

            //    brand.BrandNumber++;

            //    brand._Write(customList.ItemDataNumber, 1);//写入文件

            //    //

            //    customList._Apply(brand.BrandNumber, customList.ItemDataNumber, customList.ItemDataNumber);//应用列表属性

            //    _AddListItemData();//添加列表项数据

            //    _SetPage();//设置列表项数据

            //    //显示信息对话框

            //    GlobalWindows.MessageDisplay_Window.WindowParameter = 18;//窗口特征数值
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][6] + " " + sSourceBrand;
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][6] + " " + sSourceBrand;
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][7] + " " + sNewBrand;
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][7] + " " + sNewBrand;
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][1];
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][1];

            //    GlobalWindows.MessageDisplay_Window.StartPosition = FormStartPosition.CenterScreen;
            //    if (GlobalWindows.TopMostWindows)//置顶
            //    {
            //        GlobalWindows.MessageDisplay_Window.TopMost = true;//将窗口置于顶层
            //    }
            //    else//其它
            //    {
            //        GlobalWindows.MessageDisplay_Window.Visible = true;//显示
            //    }

            //    //

            //    _SetFunctionalButton();//设置按钮
            //}
            //else//失败
            //{
            //    //显示信息对话框

            //    GlobalWindows.MessageDisplay_Window.WindowParameter = 19;//窗口特征数值
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][6] + " " + sSourceBrand;
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][6] + " " + sSourceBrand;
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][7] + " " + sNewBrand;
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][7] + " " + sNewBrand;
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][2];
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][2];

            //    GlobalWindows.MessageDisplay_Window.StartPosition = FormStartPosition.CenterScreen;
            //    if (GlobalWindows.TopMostWindows)//置顶
            //    {
            //        GlobalWindows.MessageDisplay_Window.TopMost = true;//将窗口置于顶层
            //    }
            //    else//其它
            //    {
            //        GlobalWindows.MessageDisplay_Window.Visible = true;//显示
            //    }
            //}
        }

        //----------------------------------------------------------------------
        // 功能说明：重命名工具
        // 输入参数：1.sNewTool：重命名后的工具名称
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _RenameTool(string sNewTool)
        {
            //string sSourceBrand = brand.SystemBrandData[customList.CurrentListIndex].Name;//重命名前的品牌名称

            //if (brand._RenameBrand(sSourceBrand, sNewBrand))//成功
            //{
            //    brand.SystemBrandData[customList.CurrentListIndex].Name = sNewBrand;//品牌名称

            //    //

            //    brand._Write(customList.CurrentListIndex, 2);//写入文件

            //    //

            //    customList.ItemData[customList.CurrentListIndex].ItemText[0] = brand.SystemBrandData[customList.CurrentListIndex].Name;
            //    customList._Refresh(customList.Index_Page);//设置并选中新的品牌项

            //    //显示信息对话框

            //    GlobalWindows.MessageDisplay_Window.WindowParameter = 22;//窗口特征数值
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][8] + " " + sSourceBrand;
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][8] + " " + sSourceBrand;
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][9] + " " + sNewBrand;
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][9] + " " + sNewBrand;
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][1];
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][1];

            //    GlobalWindows.MessageDisplay_Window.StartPosition = FormStartPosition.CenterScreen;
            //    if (GlobalWindows.TopMostWindows)//置顶
            //    {
            //        GlobalWindows.MessageDisplay_Window.TopMost = true;//将窗口置于顶层
            //    }
            //    else//其它
            //    {
            //        GlobalWindows.MessageDisplay_Window.Visible = true;//显示
            //    }
            //}
            //else//失败
            //{
            //    //显示信息对话框

            //    GlobalWindows.MessageDisplay_Window.WindowParameter = 23;//窗口特征数值
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][8] + " " + sSourceBrand;
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][8] + " " + sSourceBrand;
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][9] + " " + sNewBrand;
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][9] + " " + sNewBrand;
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][2];
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][2];

            //    GlobalWindows.MessageDisplay_Window.StartPosition = FormStartPosition.CenterScreen;
            //    if (GlobalWindows.TopMostWindows)//置顶
            //    {
            //        GlobalWindows.MessageDisplay_Window.TopMost = true;//将窗口置于顶层
            //    }
            //    else//其它
            //    {
            //        GlobalWindows.MessageDisplay_Window.Visible = true;//显示
            //    }
            //}
        }

        //----------------------------------------------------------------------
        // 功能说明：删除工具
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _DeleteTool()
        {
            //string sBrandtoDelete = brand.SystemBrandData[customList.CurrentListIndex].Name;//待删除的品牌名称

            //if (brand._DeleteBrand(sBrandtoDelete))//成功
            //{
            //    Int32 i = 0;//循环控制变量

            //    brand.BrandNumber--;

            //    for (i = customList.CurrentListIndex; i < brand.BrandNumber; i++)//循环控制变量
            //    {
            //        brand.SystemBrandData[i].Name = brand.SystemBrandData[i + 1].Name;//品牌名称
            //        brand.SystemBrandData[i].Type = brand.SystemBrandData[i + 1].Type;//品牌类型
            //    }
            //    brand.SystemBrandData[i].Name = "";//品牌名称
            //    brand.SystemBrandData[i].Type = VisionSystemClassLibrary.Enum.BrandType.None;//品牌类型

            //    brand._Write(0, 3);//写入文件


            //    //

            //    Int32 iValue = 0;//临时变量

            //    if (brand.BrandNumber > 0)//存在有效项
            //    {
            //        if (0 == customList.CurrentListIndex)//删除的品牌为第一项
            //        {
            //            iValue = 0;//当前选择的项在设备列表中的序号（0 ~ iItemDataNumber - 1。取值为-1，表示当前未选择任何项）
            //        }
            //        else//删除的品牌非第一项
            //        {
            //            iValue = customList.CurrentListIndex - 1;//当前选择的项在设备列表中的序号（0 ~ iItemDataNumber - 1。取值为-1，表示当前未选择任何项）
            //        }
            //    }
            //    else//不存在有效项（实际操作中执行不到此处，因为MASTER和CURRENT品牌是始终存在的，无法删除）
            //    {
            //        iValue = -1;//当前选择的项在设备列表中的序号（0 ~ iItemDataNumber - 1。取值为-1，表示当前未选择任何项）
            //    }

            //    customList._Apply(brand.BrandNumber, iValue, iValue);//应用列表属性

            //    _AddListItemData();//添加列表项数据

            //    _SetPage();//设置列表项数据

            //    //显示信息对话框

            //    GlobalWindows.MessageDisplay_Window.WindowParameter = 26;//窗口特征数值
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][10] + " " + sBrandtoDelete + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][1];
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][10] + " " + sBrandtoDelete + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][1];

            //    GlobalWindows.MessageDisplay_Window.StartPosition = FormStartPosition.CenterScreen;
            //    if (GlobalWindows.TopMostWindows)//置顶
            //    {
            //        GlobalWindows.MessageDisplay_Window.TopMost = true;//将窗口置于顶层
            //    }
            //    else//其它
            //    {
            //        GlobalWindows.MessageDisplay_Window.Visible = true;//显示
            //    }

            //    //

            //    _SetFunctionalButton();//设置按钮
            //}
            //else//失败
            //{
            //    //显示信息对话框

            //    GlobalWindows.MessageDisplay_Window.WindowParameter = 27;//窗口特征数值
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][10] + " " + sBrandtoDelete + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][2];
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][10] + " " + sBrandtoDelete + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][2];

            //    GlobalWindows.MessageDisplay_Window.StartPosition = FormStartPosition.CenterScreen;
            //    if (GlobalWindows.TopMostWindows)//置顶
            //    {
            //        GlobalWindows.MessageDisplay_Window.TopMost = true;//将窗口置于顶层
            //    }
            //    else//其它
            //    {
            //        GlobalWindows.MessageDisplay_Window.Visible = true;//显示
            //    }
            //}
        }

        //事件

        //----------------------------------------------------------------------
        // 功能说明：点击【返回】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonClose_CustomButton_Click(object sender, EventArgs e)
        {
            //事件

            if (null != Close_Click)//有效
            {
                Close_Click(this, new CustomEventArgs());
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：点击【NEW TOOL】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonNewTool_CustomButton_Click(object sender, EventArgs e)
        {
            //if (customList.ItemDataNumber < VisionSystemClassLibrary.Class.Brand.BrandNumberMax)//品牌数量未达到最大值
            //{
            //    //事件

            //    if (null != CopyBrand_Click)//有效
            //    {
            //        CopyBrand_Click(this, new CustomEventArgs());
            //    }

            //    //显示输入键盘

            //    GlobalWindows.StandardKeyboard_Window.WindowParameter = 3;//窗口特征数值
            //    GlobalWindows.StandardKeyboard_Window.Language = language;//语言
            //    GlobalWindows.StandardKeyboard_Window.InvalidCharacter = new String[] { "\\", "/", ":", "*", "?", "\"", "<", ">", "|" };//不能包含的字符
            //    GlobalWindows.StandardKeyboard_Window.Chinese_Caption = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][14] + " " + brand.SystemBrandData[customList.CurrentListIndex].Name;//中文标题文本
            //    GlobalWindows.StandardKeyboard_Window.English_Caption = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][14] + " " + brand.SystemBrandData[customList.CurrentListIndex].Name;//英文标题文本
            //    GlobalWindows.StandardKeyboard_Window.CapsLock = true;//Caps Lock打开
            //    GlobalWindows.StandardKeyboard_Window.MaxLength = 30;//数值长度范围
            //    GlobalWindows.StandardKeyboard_Window.StringValue = brand.SystemBrandData[customList.CurrentListIndex].Name;//初始显示的数值

            //    GlobalWindows.StandardKeyboard_Window.StartPosition = FormStartPosition.CenterScreen;
            //    if (GlobalWindows.TopMostWindows)//置顶
            //    {
            //        GlobalWindows.StandardKeyboard_Window.TopMost = true;//将窗口置于顶层
            //    }
            //    else//其它
            //    {
            //        GlobalWindows.StandardKeyboard_Window.Visible = true;//显示
            //    }
            //}
            //else//品牌数量达到最大值
            //{
            //    //显示信息对话框

            //    GlobalWindows.MessageDisplay_Window.WindowParameter = 21;//窗口特征数值
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][16] + " " + brand.BrandNumber.ToString();
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][16] + " " + brand.BrandNumber.ToString();

            //    GlobalWindows.MessageDisplay_Window.StartPosition = FormStartPosition.CenterScreen;
            //    if (GlobalWindows.TopMostWindows)//置顶
            //    {
            //        GlobalWindows.MessageDisplay_Window.TopMost = true;//将窗口置于顶层
            //    }
            //    else//其它
            //    {
            //        GlobalWindows.MessageDisplay_Window.Visible = true;//显示
            //    }
            //}
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【COPY TOOL】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonCopyTool_CustomButton_Click(object sender, EventArgs e)
        {
            //if (customList.ItemDataNumber < VisionSystemClassLibrary.Class.Brand.BrandNumberMax)//品牌数量未达到最大值
            //{
            //    //事件

            //    if (null != CopyBrand_Click)//有效
            //    {
            //        CopyBrand_Click(this, new CustomEventArgs());
            //    }

            //    //显示输入键盘

            //    GlobalWindows.StandardKeyboard_Window.WindowParameter = 3;//窗口特征数值
            //    GlobalWindows.StandardKeyboard_Window.Language = language;//语言
            //    GlobalWindows.StandardKeyboard_Window.InvalidCharacter = new String[] { "\\", "/", ":", "*", "?", "\"", "<", ">", "|" };//不能包含的字符
            //    GlobalWindows.StandardKeyboard_Window.Chinese_Caption = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][14] + " " + brand.SystemBrandData[customList.CurrentListIndex].Name;//中文标题文本
            //    GlobalWindows.StandardKeyboard_Window.English_Caption = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][14] + " " + brand.SystemBrandData[customList.CurrentListIndex].Name;//英文标题文本
            //    GlobalWindows.StandardKeyboard_Window.CapsLock = true;//Caps Lock打开
            //    GlobalWindows.StandardKeyboard_Window.MaxLength = 30;//数值长度范围
            //    GlobalWindows.StandardKeyboard_Window.StringValue = brand.SystemBrandData[customList.CurrentListIndex].Name;//初始显示的数值

            //    GlobalWindows.StandardKeyboard_Window.StartPosition = FormStartPosition.CenterScreen;
            //    if (GlobalWindows.TopMostWindows)//置顶
            //    {
            //        GlobalWindows.StandardKeyboard_Window.TopMost = true;//将窗口置于顶层
            //    }
            //    else//其它
            //    {
            //        GlobalWindows.StandardKeyboard_Window.Visible = true;//显示
            //    }
            //}
            //else//品牌数量达到最大值
            //{
            //    //显示信息对话框

            //    GlobalWindows.MessageDisplay_Window.WindowParameter = 21;//窗口特征数值
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][16] + " " + brand.BrandNumber.ToString();
            //    GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][16] + " " + brand.BrandNumber.ToString();

            //    GlobalWindows.MessageDisplay_Window.StartPosition = FormStartPosition.CenterScreen;
            //    if (GlobalWindows.TopMostWindows)//置顶
            //    {
            //        GlobalWindows.MessageDisplay_Window.TopMost = true;//将窗口置于顶层
            //    }
            //    else//其它
            //    {
            //        GlobalWindows.MessageDisplay_Window.Visible = true;//显示
            //    }
            //}
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【RENAME TOOL】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonRenameTool_CustomButton_Click(object sender, EventArgs e)
        {
            ////事件

            //if (null != RenameBrand_Click)//有效
            //{
            //    RenameBrand_Click(this, new CustomEventArgs());
            //}

            ////显示输入键盘

            //GlobalWindows.StandardKeyboard_Window.WindowParameter = 4;//窗口特征数值
            //GlobalWindows.StandardKeyboard_Window.Language = language;//语言
            //GlobalWindows.StandardKeyboard_Window.InvalidCharacter = new String[] { "\\", "/", ":", "*", "?", "\"", "<", ">", "|" };//不能包含的字符
            //GlobalWindows.StandardKeyboard_Window.Chinese_Caption = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][17] + " " + brand.SystemBrandData[customList.CurrentListIndex].Name;//中文标题文本
            //GlobalWindows.StandardKeyboard_Window.English_Caption = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][17] + " " + brand.SystemBrandData[customList.CurrentListIndex].Name;//英文标题文本
            //GlobalWindows.StandardKeyboard_Window.CapsLock = true;//Caps Lock打开
            //GlobalWindows.StandardKeyboard_Window.MaxLength = 30;//数值长度范围
            //GlobalWindows.StandardKeyboard_Window.StringValue = brand.SystemBrandData[customList.CurrentListIndex].Name;//初始显示的数值

            //GlobalWindows.StandardKeyboard_Window.StartPosition = FormStartPosition.CenterScreen;
            //if (GlobalWindows.TopMostWindows)//置顶
            //{
            //    GlobalWindows.StandardKeyboard_Window.TopMost = true;//将窗口置于顶层
            //}
            //else//其它
            //{
            //    GlobalWindows.StandardKeyboard_Window.Visible = true;//显示
            //}
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【DELETE TOOL】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonDeleteTool_CustomButton_Click(object sender, EventArgs e)
        {
            ////显示信息对话框

            //GlobalWindows.MessageDisplay_Window.WindowParameter = 25;//窗口特征数值
            //GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.OkCancel;//包含【确定】和【取消】按钮
            //GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
            //GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][18] + " " + brand.SystemBrandData[customList.CurrentListIndex].Name + "？";
            //GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][18] + " " + brand.SystemBrandData[customList.CurrentListIndex].Name + "？";

            //GlobalWindows.MessageDisplay_Window.StartPosition = FormStartPosition.CenterScreen;
            //if (GlobalWindows.TopMostWindows)//置顶
            //{
            //    GlobalWindows.MessageDisplay_Window.TopMost = true;//将窗口置于顶层
            //}
            //else//其它
            //{
            //    GlobalWindows.MessageDisplay_Window.Visible = true;//显示
            //}
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：点击【Previous Page】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonPreviousPage_CustomButton_Click(object sender, EventArgs e)
        {
            customList._ClickPage(true);//翻页
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
            customList._ClickPage(false);//翻页
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：点击工具列表项事件，执行相应的操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customList_CustomListItem_Click(object sender, EventArgs e)
        {
            _SetFunctionalButton();//设置按钮
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：EDIT TOOLS，NEW TOOL，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void standardKeyboardWindow_WindowClose_EditTools_NewTool(object sender, EventArgs e)
        {
            //if (GlobalWindows.TopMostWindows)//置顶
            //{
            //    GlobalWindows.StandardKeyboard_Window.TopMost = false;//取消置于顶层
            //}
            //else//其它
            //{
            //    GlobalWindows.StandardKeyboard_Window.Visible = false;//隐藏
            //}

            ////

            //if (GlobalWindows.StandardKeyboard_Window.EnterNewValue)//拷贝
            //{
            //    if (_CheckBrandName(GlobalWindows.StandardKeyboard_Window.StringValue))//检查是否重名
            //    {
            //        _CopyBrand(GlobalWindows.StandardKeyboard_Window.StringValue);//拷贝品牌
            //    }
            //    else//重名
            //    {
            //        //显示信息对话框

            //        GlobalWindows.MessageDisplay_Window.WindowParameter = 20;//窗口特征数值
            //        GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
            //        GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
            //        GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = GlobalWindows.StandardKeyboard_Window.StringValue + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][15];
            //        GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = GlobalWindows.StandardKeyboard_Window.StringValue + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][15];

            //        GlobalWindows.MessageDisplay_Window.StartPosition = FormStartPosition.CenterScreen;
            //        if (GlobalWindows.TopMostWindows)//置顶
            //        {
            //            GlobalWindows.MessageDisplay_Window.TopMost = true;//将窗口置于顶层
            //        }
            //        else//其它
            //        {
            //            GlobalWindows.MessageDisplay_Window.Visible = true;//显示
            //        }
            //    }
            //}
            //else//未拷贝或拷贝错误
            //{
            //    //不执行操作
            //}
        }

        //----------------------------------------------------------------------
        // 功能说明：EDIT TOOLS，COPY TOOL，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void standardKeyboardWindow_WindowClose_EditTools_CopyTool(object sender, EventArgs e)
        {
            //if (GlobalWindows.TopMostWindows)//置顶
            //{
            //    GlobalWindows.StandardKeyboard_Window.TopMost = false;//取消置于顶层
            //}
            //else//其它
            //{
            //    GlobalWindows.StandardKeyboard_Window.Visible = false;//隐藏
            //}

            ////

            //if (GlobalWindows.StandardKeyboard_Window.EnterNewValue)//拷贝
            //{
            //    if (_CheckBrandName(GlobalWindows.StandardKeyboard_Window.StringValue))//检查是否重名
            //    {
            //        _CopyBrand(GlobalWindows.StandardKeyboard_Window.StringValue);//拷贝品牌
            //    }
            //    else//重名
            //    {
            //        //显示信息对话框

            //        GlobalWindows.MessageDisplay_Window.WindowParameter = 20;//窗口特征数值
            //        GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
            //        GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
            //        GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = GlobalWindows.StandardKeyboard_Window.StringValue + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][15];
            //        GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = GlobalWindows.StandardKeyboard_Window.StringValue + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][15];

            //        GlobalWindows.MessageDisplay_Window.StartPosition = FormStartPosition.CenterScreen;
            //        if (GlobalWindows.TopMostWindows)//置顶
            //        {
            //            GlobalWindows.MessageDisplay_Window.TopMost = true;//将窗口置于顶层
            //        }
            //        else//其它
            //        {
            //            GlobalWindows.MessageDisplay_Window.Visible = true;//显示
            //        }
            //    }
            //}
            //else//未拷贝或拷贝错误
            //{
            //    //不执行操作
            //}
        }

        //----------------------------------------------------------------------
        // 功能说明：EDIT TOOLS，RENAME TOOL，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void standardKeyboardWindow_WindowClose_EditTools_RenameTool(object sender, EventArgs e)
        {
            //if (GlobalWindows.TopMostWindows)//置顶
            //{
            //    GlobalWindows.StandardKeyboard_Window.TopMost = false;//取消置于顶层
            //}
            //else//其它
            //{
            //    GlobalWindows.StandardKeyboard_Window.Visible = false;//隐藏
            //}

            ////

            //if (GlobalWindows.StandardKeyboard_Window.EnterNewValue)//重命名
            //{
            //    if (_CheckBrandName(GlobalWindows.StandardKeyboard_Window.StringValue))//检查是否重名
            //    {
            //        _RenameBrand(GlobalWindows.StandardKeyboard_Window.StringValue);//重命名品牌
            //    }
            //    else//重名
            //    {
            //        //显示信息对话框

            //        GlobalWindows.MessageDisplay_Window.WindowParameter = 24;//窗口特征数值
            //        GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
            //        GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
            //        GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = GlobalWindows.StandardKeyboard_Window.StringValue + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][15];
            //        GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = GlobalWindows.StandardKeyboard_Window.StringValue + " " + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][15];

            //        GlobalWindows.MessageDisplay_Window.StartPosition = FormStartPosition.CenterScreen;
            //        if (GlobalWindows.TopMostWindows)//置顶
            //        {
            //            GlobalWindows.MessageDisplay_Window.TopMost = true;//将窗口置于顶层
            //        }
            //        else//其它
            //        {
            //            GlobalWindows.MessageDisplay_Window.Visible = true;//显示
            //        }
            //    }
            //}
            //else//未重命名或重命名错误
            //{
            //    //不执行操作
            //}
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：EDIT TOOLS，【NEW TOOL】成功，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_EditTools_NewTool_Success(object sender, EventArgs e)
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

        //----------------------------------------------------------------------
        // 功能说明：EDIT TOOLS，【NEW TOOL】失败（重名），窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_EditTools_NewTool_Failure_1(object sender, EventArgs e)
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

        //----------------------------------------------------------------------
        // 功能说明：EDIT TOOLS，【NEW TOOL】失败（数量达到最大值），窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_EditTools_NewTool_Failure_2(object sender, EventArgs e)
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

        //----------------------------------------------------------------------
        // 功能说明：EDIT TOOLS，【COPY TOOL】成功，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_EditTools_CopyTool_Success(object sender, EventArgs e)
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

        //----------------------------------------------------------------------
        // 功能说明：EDIT TOOLS，【COPY TOOL】失败（重名），窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_EditTools_CopyTool_Failure_1(object sender, EventArgs e)
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

        //----------------------------------------------------------------------
        // 功能说明：EDIT TOOLS，【COPY TOOL】失败（数量达到最大值），窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_EditTools_CopyTool_Failure_2(object sender, EventArgs e)
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

        //----------------------------------------------------------------------
        // 功能说明：EDIT TOOLS，【RENAME TOOL】成功，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_EditTools_RenameTool_Success(object sender, EventArgs e)
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

        //----------------------------------------------------------------------
        // 功能说明：EDIT TOOLS，【RENAME TOOL】失败（重名），窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_EditTools_RenameTool_Failure_1(object sender, EventArgs e)
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

        //----------------------------------------------------------------------
        // 功能说明：EDIT TOOLS，【DELETE TOOL】确认，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_EditTools_DeleteTool_Confirm(object sender, EventArgs e)
        {
            //if (GlobalWindows.TopMostWindows)//置顶
            //{
            //    GlobalWindows.MessageDisplay_Window.TopMost = false;//取消置于顶层
            //}
            //else//其它
            //{
            //    GlobalWindows.MessageDisplay_Window.Visible = false;//隐藏
            //}

            ////

            //if (GlobalWindows.MessageDisplay_Window.MessageDisplayControl.OkCancel)//删除所选品牌
            //{
            //    _DeleteBrand();//删除品牌

            //    //事件

            //    if (null != DeleteBrand_Click)//有效
            //    {
            //        DeleteBrand_Click(this, new CustomEventArgs());
            //    }
            //}
            //else//取消删除品牌
            //{
            //    //不执行操作
            //}
        }

        //----------------------------------------------------------------------
        // 功能说明：EDIT TOOLS，【DELETE TOOL】成功，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_EditTools_DeleteTool_Success(object sender, EventArgs e)
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

        //----------------------------------------------------------------------
        // 功能说明：EDIT TOOLS，【DELETE TOOL】失败，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_EditTools_DeleteTool_Failure(object sender, EventArgs e)
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
}
