/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：RejectsControl.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：REJECTS VIEW控件

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

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;

using System.Reflection;

using System.Threading;

namespace VisionSystemControlLibrary
{
    public partial class RejectsControl : UserControl
    {
        //该控件为Rejects页面

        //某项在页中的索引值范围为：0 ~ 11，其中最大值11对应最上面的控件（labelName1或labelIndex1），最小值0对应最下面的控件（labelName12或labelIndex12）
        //某项在图像数据中的索引值范围为：0 ~ 47，其数值对应于数组imageInformation的索引序号。[0] ~ [11]对应于SLOT1中的1 ~ 12，[36] ~ [47]对应于SLOT4中的37 ~ 48
        //页码3对应于SLOT4，页码0对应于SLOT1

        private VisionSystemClassLibrary.Enum.InterfaceLanguage language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;//属性，语言

        private VisionSystemClassLibrary.Enum.DeviceState devicestate = VisionSystemClassLibrary.Enum.DeviceState.Stop;//属性，设备状态

        //

        private bool bViewToolGraphics = false;//属性，【VIEW TOOL GRAPHICS】按钮状态。true：按钮按下；false：按钮未按下

        private bool bSaving = false;//是否正在保存图像。取值范围：true，是；false，否
        private bool bClearing = false;//是否正在清除图像。取值范围：true，是；false，否

        private bool bSaving_SingleorAll = false;//保存图像的类型。取值范围：true，保存单个图像；false，保存所有图像

        private Int32 iBackupImageIndex = 0;//属性，当前备份的图像索引值（从0开始）
        private Int32 iBackupImageTimeoutCount = 0;//备份图像超时计数

        private Int32 iClearImageTimeoutCount = 0;//清除图像超时计数

        //

        private VisionSystemClassLibrary.Class.Camera camera = new VisionSystemClassLibrary.Class.Camera();//属性（只读），相机

        //

        public const Int32 SlotNumber = 4;//SLOT数量

        //

        private Int32[] iToolsIndex = new Int32[48];//当前列表中存储的图像对应的工具索引值（从0开始；用于更新图像时的判断）

        //

        private Bitmap bitmapNone = null;//无效图像

        //

        private Boolean bClickListItem = false;//属性，是否点击列表项。取值范围：true，是；false，否

        //

        private String[][] sMessageText = new String[2][];//信息控件、提示信息窗口上显示的文本（[语言][包含的文本]）

        private String[][] sMessageText_1 = new String[2][];//标题控件上显示的文本（[语言][包含的文本]）

        //

        [Browsable(true), Description("点击【返回】按钮时产生的事件"), Category("RejectsControl 事件")]
        public event EventHandler Close_Click;//点击【返回】按钮时产生的事件
        //CustomEventArgs参数含义：
        //无意义

        [Browsable(true), Description("点击列表控件时产生的事件"), Category("RejectsControl 事件")]
        public event EventHandler Item_Click;//点击列表控件时产生的事件
        //CustomEventArgs参数含义：无

        [Browsable(true), Description("点击【Previous Page】按钮时产生的事件"), Category("RejectsControl 事件")]
        public event EventHandler PreviousPage_Click;//点击【Previous Page】按钮时产生的事件
        //CustomEventArgs参数含义：无

        [Browsable(true), Description("点击【Next Page】按钮时产生的事件"), Category("RejectsControl 事件")]
        public event EventHandler NextPage_Click;//点击【Next Page】按钮时产生的事件
        //CustomEventArgs参数含义：无

        [Browsable(true), Description("点击【VIEW TOOL GRAPHICS】按钮时产生的事件"), Category("RejectsControl 事件")]
        public event EventHandler ViewToolGraphics_Click;//点击【VIEW TOOL GRAPHICS】按钮时产生的事件
        //CustomEventArgs参数含义：无

        [Browsable(true), Description("点击【BACKUP SINGLE IMAGE】按钮时产生的事件"), Category("RejectsControl 事件")]
        public event EventHandler BackupSingleImage_Click;//点击【BACKUP SINGLE IMAGE】按钮时产生的事件
        //CustomEventArgs参数含义：无

        [Browsable(true), Description("点击【BACKUP ALL IMAGES】按钮时产生的事件"), Category("RejectsControl 事件")]
        public event EventHandler BackupAllImages_Click;//点击【BACKUP ALL IMAGES】按钮时产生的事件
        //CustomEventArgs参数含义：无

        [Browsable(true), Description("点击【CLEAR ALL】按钮时产生的事件"), Category("RejectsControl 事件")]
        public event EventHandler ClearAll_Click;//点击【CLEAR ALL】按钮时产生的事件
        //CustomEventArgs参数含义：无

        [Browsable(true), Description("点击SLOT控件时产生的事件"), Category("RejectsControl 事件")]
        public event EventHandler Slot_Click;//点击SLOT控件时产生的事件
        //CustomEventArgs参数含义：无

        [Browsable(true), Description("备份所有图像时的事件，用于发送指令"), Category("RejectsControl 事件")]
        public event EventHandler BackupAllImages_Event;//备份所有图像时的事件，用于发送指令
        //CustomEventArgs参数含义：无

        //

        //----------------------------------------------------------------------
        // 功能说明：系统默认调用，构造函数，初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public RejectsControl()
        {
            _Init();//初始化

            //由于该控件仅有一个实例，因此可以在此处进行事件的订阅

            if (null != GlobalWindows.MessageDisplay_Window)
            {
                GlobalWindows.MessageDisplay_Window.WindowClose_Rejects_BackupSingleImage_Confirm += new System.EventHandler(messageDisplayWindow_WindowClose_Rejects_BackupSingleImage_Confirm);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_Rejects_BackupAllImages_Confirm += new System.EventHandler(messageDisplayWindow_WindowClose_Rejects_BackupAllImages_Confirm);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_Rejects_BackupAllImages_Wait += new System.EventHandler(messageDisplayWindow_WindowClose_Rejects_BackupAllImages_Wait);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_Rejects_ClearAll_Confirm += new System.EventHandler(messageDisplayWindow_WindowClose_Rejects_ClearAll_Confirm);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_Rejects_ClearAll_Wait += new System.EventHandler(messageDisplayWindow_WindowClose_Rejects_ClearAll_Wait);//订阅事件
            }

            //

            FieldInfo[] fieldinfo = VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese.GetType().GetFields();//获取公共字段
            if (null != fieldinfo && 1 < fieldinfo.Length)//有效
            {
                Int32 i = 0;//循环控制变量

                sMessageText = new String[fieldinfo.Length - 1][];
                sMessageText_1 = new String[fieldinfo.Length - 1][];
                for (i = 0; i < fieldinfo.Length - 1; i++)
                {
                    sMessageText[i] = new String[10];
                    sMessageText_1[i] = new String[1];
                }

                //

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] = "备份所有图像";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] = "SAVING IMAGES";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][1] = "清除图像完成";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][1] = "Clear all images Completed";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][2] = "备份图像";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][2] = "SAVING IMAGE";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][3] = "备份图像完成";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][3] = "Back up Completed";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][4] = "清除图像";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][4] = "CLEARING IMAGES";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][5] = "确定备份选中图像";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][5] = "Back up single image";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][6] = "确定备份所有图像";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][6] = "Back up all images";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][7] = "确定清除图像";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][7] = "Clear all images";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][8] = "清除图像失败";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][8] = "Clear all images Failed";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][9] = "备份图像失败";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][9] = "Back up Failed";

                //

                sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] = customButtonCaption.Chinese_TextDisplay[0];
                sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] = customButtonCaption.English_TextDisplay[0];
            }
        }

        //属性

        //-----------------------------------------------------------------------
        // 功能说明：Language属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("语言"), Category("RejectsControl 通用")]
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
        // 功能说明：SystemDeviceState属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("设备状态"), Category("RejectsControl 通用")]
        public VisionSystemClassLibrary.Enum.DeviceState SystemDeviceState
        {
            get//读取
            {
                return devicestate;
            }
            set//设置
            {
                if (value != devicestate)
                {
                    devicestate = value;

                    //

                    _SetDeviceState();
                }
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：ViewToolGraphics属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("【VIEW TOOL GRAPHICS】按钮状态。true：按钮按下；false：按钮未按下"), Category("RejectsControl 通用")]
        public bool ViewToolGraphics //属性
        {
            get//读取
            {
                return bViewToolGraphics;
            }
            set//设置
            {
                if (bViewToolGraphics != value)
                {
                    bViewToolGraphics = value;

                    //

                    if (bViewToolGraphics)//【VIEW TOOL GRAPHICS】按下
                    {
                        customButtonViewToolGraphics.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
                    }
                    else//【VIEW TOOL GRAPHICS】未按下
                    {
                        customButtonViewToolGraphics.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：SelectedCamera属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("相机"), Category("RejectsControl 通用")]
        public VisionSystemClassLibrary.Class.Camera SelectedCamera//属性
        {
            get//读取
            {
                return camera;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：BackupImageIndex属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("当前备份的图像索引值（从0开始）"), Category("RejectsControl 通用")]
        public int BackupImageIndex //属性
        {
            get//读取
            {
                return iBackupImageIndex;
            }
            set//设置
            {
                if (iBackupImageIndex != value)
                {
                    iBackupImageIndex = value;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：CurrentIndex属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("当前选择的项的索引（0 ~ 47）"), Category("RejectsControl 通用")]
        public int CurrentIndex //属性
        {
            get//读取
            {
                return _ConvertIndex(customList.CurrentListIndex);
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ClickRejectListItem属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("是否点击列表项。取值范围：true，是；false，否"), Category("RejectsControl 通用")]
        public Boolean ClickRejectListItem //属性
        {
            get//读取
            {
                return bClickListItem;
            }
            set//设置
            {
                bClickListItem = value;
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

            _SetDeviceState();

            _SetCamera();//应用属性设置

            _SetLanguage();//设置语言
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：转换索引值
        // 输入参数：1.iIndex：索引值
        // 输出参数：无
        // 返回值：转换完成的索引值
        //----------------------------------------------------------------------
        private Int32 _ConvertIndex(Int32 iIndex)
        {
            if (0 <= iIndex && iIndex < camera.Rejects.GraphicsInformation.Length)//有效
            {
                return camera.Rejects.GraphicsInformation.Length - 1 - iIndex;
            }
            else//无效
            {
                return -1;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：转换页码值
        // 输入参数：1.iIndex：索引值
        // 输出参数：无
        // 返回值：转换完成的页码值
        //----------------------------------------------------------------------
        private Int32 _ConvertPage(Int32 iPageIndex)
        {
            //if (0 <= iPageIndex && iPageIndex < customList.TotalPage)//有效
            //{
            //    return customList.TotalPage - 1 - iPageIndex;
            //}
            //else//无效
            //{
            //    return -1;
            //}

            if (0 <= iPageIndex && iPageIndex < SlotNumber)//有效
            {
                return SlotNumber - 1 - iPageIndex;
            }
            else//无效
            {
                return -1;
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：应用设置的属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _Apply()
        {
            Int32 i = 0;//循环控制变量
            Int32 iItemNumber = 0;//有效的图像数据的数目
            Int32 iCurrentIndex = 0;//临时变量

            for (i = camera.Rejects.GraphicsInformation.Length - 1; i >= 0; i--)
            {
                if (!(camera.Rejects.GraphicsInformation[i].Valid))//图像无效
                {
                    break;
                }
            }
            if (i < camera.Rejects.GraphicsInformation.Length - 1)//存在有效的图像
            {
                iItemNumber = camera.Rejects.GraphicsInformation.Length - 1 - i;//有效的图像数据的数目

                //

                iCurrentIndex = 0;
            }
            else//0 == i，所有图像均无效
            {
                iItemNumber = 0;//有效的图像数据的数目

                //

                iCurrentIndex = -1;
            }

            customList._ApplyListHeader();//应用列表头属性
            customList._ApplyListItem();//应用列表项属性

            customList._Apply(iItemNumber, iCurrentIndex, iCurrentIndex);//应用列表属性

            _AddItemData();//添加列表项数据

            _SetPage();//设置列表项数据

            _SelectSlot();//设置各个SLOT

            //

            _ClearAllSlot();//恢复初始状态

            _SetSlot(_ConvertPage(customList.CurrentPage), customList._GetPageItemIndex(_ConvertIndex(customList.CurrentListIndex)));//设置SLOT中的当前区块

            //

            _SetFunctionalButton();//设置【BACKUP SINGLE IMAGE】、【BACKUP ALL IMAGES】和【CLEAR ALL】按钮

            _SetView();//设置VIEW图像
        }

        //----------------------------------------------------------------------
        // 功能说明：设置属性SelectedCamera后调用，应用设置的属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetCamera()
        {
            //1.通用属性

            customButtonCaption.Chinese_TextDisplay = new String[1] { camera.CameraCHNName + sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] };//设置显示的文本
            customButtonCaption.English_TextDisplay = new String[1] { camera.CameraENGName + sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] };//设置显示的文本

            //

            iToolsIndex = new Int32[camera.Rejects.ImageNumberTotal];//当前列表中存储的图像对应的工具索引值（从0开始；用于更新图像时的判断）

            for (Int32 i = 0; i < iToolsIndex.Length; i++)//初始化
            {
                iToolsIndex[i] = -1;//当前列表中存储的图像对应的工具索引值（从0开始；用于更新图像时的判断）
            }

            //3.应用设置

            _Apply();
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
            customButtonViewToolGraphics.Language = language;//【VIEW TOOL GRAPHICS】
            customButtonBackupSingleImage.Language = language;//【BACKUP SINGLE IMAGE】
            customButtonBackupAllImages.Language = language;//【BACKUP ALL IMAGES】
            customButtonClearAll.Language = language;//【CLEAR ALL】

            //

            customButtonClose.Language = language;//【Close】
            customButtonPreviousPage.Language = language;//Brands List，【Previous Page】
            customButtonNextPage.Language = language;//Brands List，【Next Page】

            //

            customList.Language = language;//列表
            imageDisplayView.Language = language;//标题栏

            //

            customButtonMessage.Language = language;//提示信息文本

            customButtonCaption.Language = language; ;//设置显示的文本

            //

            customButtonSlot_4.Language = language;
            customButtonSlot_3.Language = language;
            customButtonSlot_2.Language = language;
            customButtonSlot_1.Language = language;
        }

        //----------------------------------------------------------------------
        // 功能说明：用户调用，设备状态设置完成后，更新页面
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetDeviceState()
        {
            if (bSaving || bClearing)//备份或清除
            {
                //不执行操作
            }
            else//其它
            {
                rejectsSlot4.Enabled = true;//SLOT4控件
                rejectsSlot3.Enabled = true;//SLOT3控件
                rejectsSlot2.Enabled = true;//SLOT2控件
                rejectsSlot1.Enabled = true;//SLOT1控件

                customList.ListEnabled = true;

                customButtonPreviousPage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【Previous Page】
                customButtonNextPage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【Next Page】
                if (bViewToolGraphics)//【VIEW TOOL GRAPHICS】按下
                {
                    customButtonViewToolGraphics.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//设置【VIEW TOOL GRAPHICS】按钮的背景
                }
                else//【VIEW TOOL GRAPHICS】未按下
                {
                    customButtonViewToolGraphics.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//设置【VIEW TOOL GRAPHICS】按钮的背景
                }
                customButtonBackupSingleImage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【BACKUP SINGLE IMAGE】
                customButtonBackupAllImages.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【BACKUP ALL IMAGES】
                customButtonClearAll.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【CLEAR ALL】
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：用户调用，设备状态改变时（如某一个相机连接或断开），更新页面
        // 输入参数：1.bConnected：设备是否连接。取值范围：true，是；false，否
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _DeviceStateChanged(Boolean bConnected)
        {
            if (bConnected)//连接
            {
                _SetDeviceState();//设备状态
            }
            else//断开
            {
                rejectsSlot4.Enabled = false;//SLOT4控件
                rejectsSlot3.Enabled = false;//SLOT3控件
                rejectsSlot2.Enabled = false;//SLOT2控件
                rejectsSlot1.Enabled = false;//SLOT1控件

                customList.ListEnabled = false;

                customButtonPreviousPage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【Previous Page】
                customButtonNextPage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【Next Page】
                customButtonViewToolGraphics.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//设置【VIEW TOOL GRAPHICS】按钮的背景
                customButtonBackupSingleImage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【BACKUP SINGLE IMAGE】
                customButtonBackupAllImages.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【BACKUP ALL IMAGES】
                customButtonClearAll.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【CLEAR ALL】
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：设置VIEW图像
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetView()
        {
            Int32 iCurIndex = _ConvertIndex(customList.CurrentListIndex);

            if (0 <= iCurIndex && iCurIndex < camera.Rejects.GraphicsInformation.Length)
            {
                imageDisplayView.Information = camera.Rejects.GraphicsInformation[iCurIndex];//当前图像信息
            }

            if (null != camera.ImageReject)//有效
            {
                if (imageDisplayView.ControlSize.Width <= camera.ImageReject.Width && imageDisplayView.ControlSize.Height <= camera.ImageReject.Height)//有效
                {
                    if (!(imageDisplayView.ShowTitle))//隐藏
                    {
                        imageDisplayView.ShowTitle = true;//显示
                    }

                    //

                    imageDisplayView.BitmapDisplay = camera.ImageReject.ToBitmap();//图像数据
                }
            }
            else//无效
            {
                if (imageDisplayView.ShowTitle)//显示
                {
                    imageDisplayView.ShowTitle = false;//隐藏
                }

                //

                imageDisplayView.BitmapDisplay = (Bitmap)bitmapNone.Clone();//图像数据
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：用户调用，REJECTS图像发生变化时，更新控件显示
        // 输入参数：1.iCurIndex：当前图像索引值
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _SetImageInformation(Int32 iCurIndex, Int32 iRejectsNumber)
        {
            Int32 i = 0;//循环控制变量
            Int32 iValue = _ConvertIndex(iCurIndex);//临时变量
            Int32 iCurrentIndex = 0;//临时变量
            Boolean bListIndexUpdate = false;//列表当前索引值是否更新

            if (iValue != customList.CurrentListIndex)//列表当前索引值更新
            {
                iCurrentIndex = iValue;

                //

                bListIndexUpdate = true;
            }

            //
            
            if (_CheckImageListUpdate() || bListIndexUpdate)//图像列表更新，列表当前索引值更新
            {
                customList._SetListData(iRejectsNumber, iCurrentIndex, iCurrentIndex);//更新数据
                //customList._Apply(iItemNumber, iCurrentIndex, iCurrentIndex);//应用列表属性

                _AddItemData();//添加列表项数据

                _SetPage();//设置列表项数据

                _SelectSlot();//设置各个SLOT

                //

                _ClearAllSlot();//恢复初始状态

                _SetSlot(_ConvertPage(customList.CurrentPage), customList._GetPageItemIndex(_ConvertIndex(customList.CurrentListIndex)));//设置SLOT中的当前区块
            }

            _SetFunctionalButton();//设置【BACKUP SINGLE IMAGE】、【BACKUP ALL IMAGES】和【CLEAR ALL】按钮

            //

            _SetView();//设置VIEW图像

            //

            for (i = camera.Rejects.GraphicsInformation.Length - 1; i >= 0; i--)//存储当前
            {
                iToolsIndex[i] = camera.Rejects.GraphicsInformation[i].ToolsIndex;//当前列表中存储的图像对应的工具索引值（从0开始；用于更新图像时的判断）
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：检查图像列表是否更新
        // 输入参数：无
        // 输出参数：无
        // 返回值：图像列表是否更新。取值范围：true，是；false，否
        //----------------------------------------------------------------------
        private Boolean _CheckImageListUpdate()
        {
            Boolean bReturn = true;//函数返回值

            Int32 i = 0;//循环控制变量

            for (i = camera.Rejects.GraphicsInformation.Length - 1; i >= 0; i--)
            {
                if (iToolsIndex[i] != camera.Rejects.GraphicsInformation[i].ToolsIndex)//不同
                {
                    break;
                }
            }

            if (i < 0)//均相同
            {
                bReturn = false;
            }

            //

            return bReturn;
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：获取当前备份的图像的索引值（备份所有图像时调用，在备份完成一幅图像后调用该函数），并判断备份是否完成。
        // 输入参数：无
        // 输出参数：无
        // 返回值：备份操作是否完成。取值范围：true，是；false，否
        //----------------------------------------------------------------------
        private bool _GetBackupIndex()
        {
            iBackupImageIndex--;

            if (iBackupImageIndex < camera.Rejects.ImageNumberTotal - customList.ItemDataNumber)//备份完成
            {
                return true;
            }
            else//备份未完成
            {
                return false;
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：点击【BACKUP SINGLE IMAGE】或【BACKUP ALL IMAGES】按钮后，启动保存图像
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _StartBackup()
        {
            bSaving = true;//保存图像

            //

            if (bSaving_SingleorAll)//【BACKUP SINGLE IMAGE】按钮
            {
                iBackupImageIndex = _ConvertIndex(customList.CurrentListIndex);//当前备份的图像索引值（从0开始）

                //

                customButtonMessage.Chinese_TextDisplay = new String[1] { sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][2] + "：" + (iBackupImageIndex + 1).ToString() };//设置显示的文本
                customButtonMessage.English_TextDisplay = new String[1] { sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][2] + "：" + (iBackupImageIndex + 1).ToString() };//设置显示的文本

                //设置区块

                _SetAllSlot(bSaving);//设置区块数值

                //

                iBackupImageTimeoutCount = 0;

                Thread thread = new Thread(_threadBackupImage);//线程
                thread.IsBackground = true;
                thread.Start();//启动线程

                //

                _BackupImage(true);
            }
            else//【BACKUP ALL IMAGES】按钮
            {
                iBackupImageIndex = camera.Rejects.ImageNumberTotal - 1;//当前备份的图像索引值

                //

                customButtonMessage.Chinese_TextDisplay = new String[1] { sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] + "：" + (iBackupImageIndex + 1).ToString() };//设置显示的文本
                customButtonMessage.English_TextDisplay = new String[1] { sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] + "：" + (iBackupImageIndex + 1).ToString() };//设置显示的文本

                //设置区块

                _SetAllSlot(bSaving);//设置区块数值

                //

                iBackupImageTimeoutCount = 0;

                Thread thread = new Thread(_threadBackupImage);//线程
                thread.IsBackground = true;
                thread.Start();//启动线程

                //事件

                if (null != BackupAllImages_Event)//有效
                {
                    BackupAllImages_Event(this, new EventArgs());
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【CLEAR ALL】按钮，启动清除所有图像
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _StartClearAllImages()
        {
            bClearing = true;//清除所有图像

            //

            customButtonMessage.Chinese_TextDisplay = new String[1] { sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][4] + "..." };//设置显示的文本
            customButtonMessage.English_TextDisplay = new String[1] { sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][4] + "..." };//设置显示的文本

            //

            iClearImageTimeoutCount = 0;

            Thread thread = new Thread(_threadClearImage);//线程
            thread.IsBackground = true;
            thread.Start();//启动线程
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：备份图像线程
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _threadBackupImage()
        {
            while (bSaving)//正在备份
            {
                if (6 >= iBackupImageTimeoutCount)//未超时
                {
                    Thread.Sleep(1000);

                    iBackupImageTimeoutCount++;
                } 
                else//超时
                {
                    iBackupImageTimeoutCount = 0;

                    this.Invoke(new EventHandler(delegate { this._BackupFinish(false); }));

                    break;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：清除图像线程
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _threadClearImage()
        {
            while (bClearing)//正在清除
            {
                if (6 >= iClearImageTimeoutCount)//未超时
                {
                    Thread.Sleep(1000);

                    iClearImageTimeoutCount++;
                }
                else//超时
                {
                    iClearImageTimeoutCount = 0;

                    this.Invoke(new EventHandler(delegate { this._ClearAllImagesFinish(false); }));

                    break;
                }
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：用户调用，备份图像
        // 输入参数：1.bValid：图像是否有效
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _BackupImage(Boolean bValid)
        {
            iBackupImageTimeoutCount = 0;//超时计数清零

            //

            if (bValid)//图像有效
            {
                if (null != camera.ImageReject)//有效
                {
                    Int32 i = 0;//循环控制变量
                    Size size = new Size();//临时变量

                    camera._WriteImage(VisionSystemClassLibrary.Enum.ImageInformationType.Reject, true, iBackupImageIndex);//保存剔除图像信息

                    //

                    ImageDisplay imagedisplay = new VisionSystemControlLibrary.ImageDisplay();
                    imagedisplay.ControlSize = new Size(camera.ImageReject.Width, camera.ImageReject.Height);

                    if (0 != imagedisplay.SlotSize.Width % VisionSystemClassLibrary.Struct.ImageInformation.TotalNumber)//无法整除
                    {
                        for (i = 1; imagedisplay.SlotLocation.X + imagedisplay.SlotSize.Width + i < imagedisplay.MinValueLocation.X; i++)
                        {
                            if (0 == (imagedisplay.SlotSize.Width + i) % VisionSystemClassLibrary.Struct.ImageInformation.TotalNumber)//可以整除
                            {
                                break;
                            }
                        }
                        if (imagedisplay.SlotLocation.X + imagedisplay.SlotSize.Width + i < imagedisplay.MinValueLocation.X)
                        {
                            size = new Size(imagedisplay.SlotSize.Width + i, imagedisplay.SlotSize.Height);
                        }
                        else
                        {
                            for (i = 1; 0 < imagedisplay.SlotSize.Width - i; i++)
                            {
                                if (0 == (imagedisplay.SlotSize.Width - i) % VisionSystemClassLibrary.Struct.ImageInformation.TotalNumber)//可以整除
                                {
                                    break;
                                }
                            }
                            if (0 < imagedisplay.SlotSize.Width - i)
                            {
                                size = new Size(imagedisplay.SlotSize.Width - i, imagedisplay.SlotSize.Height);
                            }
                            else
                            {
                                //使用默认值

                                //不执行操作
                            }
                        }
                    }

                    imagedisplay._SetSlotWidth(size);//设置状态栏控件数值区域控件每列的宽度

                    imagedisplay.SlotSize = size;

                    //

                    imagedisplay.Information = camera.Rejects.GraphicsInformation[iBackupImageIndex];//图像信息
                    imagedisplay.BitmapDisplay = camera.ImageReject.ToBitmap();//图像

                    //

                    imagedisplay._GetImage();//获取

                    if (null != imagedisplay.ImageControl)
                    {
                        camera._WriteImage(VisionSystemClassLibrary.Enum.ImageInformationType.Reject, imagedisplay.ImageControl, iBackupImageIndex);//保存图像
                    }

                    imagedisplay._ReleaseImage();//释放

                    //

                    imagedisplay.Dispose();
                }
            }

            //

            int iPage = customList._GetPage(iBackupImageIndex);//获取保存的图像所在的页码
            int iPageIndex = customList._GetPageItemIndex(iBackupImageIndex);//获取保存的图像在页中的索引值

            //更新SLOT区块

            _SetSlot(iPage, iPageIndex, bValid);

            //

            if (bSaving_SingleorAll)//【BACKUP SINGLE IMAGE】按钮
            {
                _BackupFinish(true);//备份图像完成
            }
            else//【BACKUP ALL IMAGES】按钮
            {
                //判断备份是否完成

                if (_GetBackupIndex())//备份完成
                {
                    _BackupFinish(true);//备份图像完成
                }
                else//备份未完成
                {
                    //更新页面

                    customButtonMessage.Chinese_TextDisplay = new String[1] { sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] + "：" + (iBackupImageIndex + 1).ToString() };//设置显示的文本
                    customButtonMessage.English_TextDisplay = new String[1] { sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] + "：" + (iBackupImageIndex + 1).ToString() };//设置显示的文本

                    //事件

                    if (null != BackupAllImages_Event)//有效
                    {
                        BackupAllImages_Event(this, new EventArgs());

                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：保存图像完成时调用本函数
        // 输入参数：1.bSuccess：操作是否成功。取值范围：true，是；false，否
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _BackupFinish(Boolean bSuccess)
        {
            GlobalWindows.MessageDisplay_Window.WindowParameter = 81;//窗口特征数值
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
            if (bSuccess)//成功
            {
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][3];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][3];
            }
            else//失败
            {
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][9];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][9];
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

            //

            bSaving = false;//恢复数值

            iBackupImageTimeoutCount = 0;

            //

            customButtonMessage.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonMessage.English_TextDisplay = new String[1] { " " };//设置显示的文本

            //

            customList.ListEnabled = true;//列表

            rejectsSlot4.Enabled = true;//SLOT4控件
            rejectsSlot3.Enabled = true;//SLOT3控件
            rejectsSlot2.Enabled = true;//SLOT2控件
            rejectsSlot1.Enabled = true;//SLOT1控件

            customButtonClose.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//更新【返回】按钮的背景
            customButtonPreviousPage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//更新【Previous Page】按钮的背景
            customButtonNextPage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//更新【Next Page】按钮的背景

            _SetFunctionalButton();

            //设置区块

            _SetAllSlot(bSaving);//设置区块数值
        }

        //----------------------------------------------------------------------
        // 功能说明：完成清除所有图像
        // 输入参数：1.bSuccess：操作是否成功。取值范围：true，是；false，否
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _ClearAllImagesFinish(Boolean bSuccess)
        {
            GlobalWindows.MessageDisplay_Window.WindowParameter = 82;//窗口特征数值
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言      
            if (bSuccess)//成功
            {
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][1];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][1];
            } 
            else//失败
            {
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][8];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][8];
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

            //

            bClearing = false;//恢复数值

            iClearImageTimeoutCount = 0;

            //

            customList._ClearAllListItem();//清除控件显示的所有文本和图标，并置为未选中状态

            _SetImageInformation(-1, 0);//更新页面

            customButtonMessage.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonMessage.English_TextDisplay = new String[1] { " " };//设置显示的文本

            //

            customList.ListEnabled = true;//列表

            rejectsSlot4.Enabled = true;//SLOT4控件
            rejectsSlot3.Enabled = true;//SLOT3控件
            rejectsSlot2.Enabled = true;//SLOT2控件
            rejectsSlot1.Enabled = true;//SLOT1控件

            customButtonPreviousPage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//更新【Previous Page】按钮的背景
            customButtonNextPage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//更新【Next Page】按钮的背景
            customButtonClose.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//更新【返回】按钮的背景

            _SetFunctionalButton();//设置按钮
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _Init()
        {
            InitializeComponent();
            
            //

            customButtonMessage.Chinese_TextDisplay = new String[1] { " " };//设置显示的文本
            customButtonMessage.English_TextDisplay = new String[1] { " " };//设置显示的文本

            //

            iToolsIndex = new Int32[camera.Rejects.ImageNumberTotal];//当前列表中存储的图像对应的工具索引值（从0开始；用于更新图像时的判断）

            //

            bitmapNone = new Bitmap(imageDisplayView.Width, imageDisplayView.Height);//无效图像

            //

            int i = 0;//循环控制变量

            for (i = camera.Rejects.ImageNumberTotal - 1; i >= 0; i--)//赋初值
            {
                camera.Rejects.GraphicsInformation[i] = new VisionSystemClassLibrary.Struct.ImageInformation();

                camera.Rejects.GraphicsInformation[i].Valid = false;
                camera.Rejects.GraphicsInformation[i].Name = "";

                //

                iToolsIndex[i] = -1;//当前列表中存储的图像对应的工具索引值（从0开始；用于更新图像时的判断）
            }

            //

            //SLOT 1
            rejectsSlot1.Current = false;//是否为当前Slot。true：是；false：否
            rejectsSlot1.Working = false;//是否正在保存数据。true：是；false：否
            rejectsSlot1.Position = -1;//bSaving取值为false时使用，当前区块的位置（从0开始，取值为-1，表示不存在有效的区块）
            for (i = 0; i < rejectsSlot1.Value.Length; i++)//赋初值
            {
                rejectsSlot1.Value[i] = false;//bSaving取值为true时使用，区块的数值。取值范围：true，表示保存成功；false，表示保存失败
                rejectsSlot1.Valid[i] = false;//属性，bSaving取值为true时使用，区块是否有效。true：是；false：否
            }

            //SLOT 2
            rejectsSlot2.Current = false;//是否为当前Slot。true：是；false：否
            rejectsSlot2.Working = false;//是否正在保存数据。true：是；false：否
            rejectsSlot2.Position = -1;//bSaving取值为false时使用，当前区块的位置（从0开始，取值为-1，表示不存在有效的区块）
            for (i = 0; i < rejectsSlot2.Value.Length; i++)//赋初值
            {
                rejectsSlot2.Value[i] = false;//bSaving取值为true时使用，区块的数值。取值范围：true，表示保存成功；false，表示保存失败
                rejectsSlot2.Valid[i] = false;//属性，bSaving取值为true时使用，区块是否有效。true：是；false：否
            }

            //SLOT 3
            rejectsSlot3.Current = false;//是否为当前Slot。true：是；false：否
            rejectsSlot3.Working = false;//是否正在保存数据。true：是；false：否
            rejectsSlot3.Position = -1;//bSaving取值为false时使用，当前区块的位置（从0开始，取值为-1，表示不存在有效的区块）
            for (i = 0; i < rejectsSlot3.Value.Length; i++)//赋初值
            {
                rejectsSlot3.Value[i] = false;//bSaving取值为true时使用，区块的数值。取值范围：true，表示保存成功；false，表示保存失败
                rejectsSlot3.Valid[i] = false;//属性，bSaving取值为true时使用，区块是否有效。true：是；false：否
            }

            //SLOT 4
            rejectsSlot4.Current = false;//是否为当前Slot。true：是；false：否
            rejectsSlot4.Working = false;//是否正在保存数据。true：是；false：否
            rejectsSlot4.Position = -1;//bSaving取值为false时使用，当前区块的位置（从0开始，取值为-1，表示不存在有效的区块）
            for (i = 0; i < rejectsSlot4.Value.Length; i++)//赋初值
            {
                rejectsSlot4.Value[i] = false;//bSaving取值为true时使用，区块的数值。取值范围：true，表示保存成功；false，表示保存失败
                rejectsSlot4.Valid[i] = false;//属性，bSaving取值为true时使用，区块是否有效。true：是；false：否
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：使用默认值设置并显示控件
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetDefault()
        {
            int i = 0;//循环控制变量

            camera.BackupImagesPath = "C:\\X6S\\" + camera.Name + camera.BackupImagesPath;//备份图像路径

            customButtonCaption.Chinese_TextDisplay = new String[1] { camera.CameraCHNName + sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] };//设置显示的文本
            customButtonCaption.English_TextDisplay = new String[1] { camera.CameraENGName + sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] };//设置显示的文本

            //

            bViewToolGraphics = false;//属性，【VIEW TOOL GRAPHICS】按钮状态。true：按钮按下；false：按钮未按下

            for (i = camera.Rejects.ImageNumberTotal - 1; i >= 0; i--)//赋值
            {
                camera.Rejects.GraphicsInformation[i].Valid = true;
                camera.Rejects.GraphicsInformation[i].Name = "EXAMPLE" + (i + 1).ToString();

                //

                iToolsIndex[i] = 0;//当前列表中存储的图像对应的工具索引值（从0开始；用于更新图像时的判断）
            }

            //

            _Apply();
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：添加列表数据
        // 输入参数：1.iItemNumber：列表项数目
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _AddItemData()
        {
            Int32 i = 0;//循环控制变量
            Int32 j = 0;//循环控制变量

            for (i = camera.Rejects.GraphicsInformation.Length - 1; i >= 0; i--)
            {
                if (camera.Rejects.GraphicsInformation[i].Valid)//图像有效
                {
                    customList.ItemData[j].ItemText[0] = (i + 1).ToString();
                    customList.ItemData[j].ItemText[1] = camera.Rejects.GraphicsInformation[i].Name;

                    customList.ItemData[j].ItemFlag = j;

                    //

                    j++;
                }
            }
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

            _SetSlot();//更新区块状态
        }

        //----------------------------------------------------------------------
        // 功能说明：设置当前页中的控件
        // 输入参数：1.iPageIndex：页码索引值
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetPage(Int32 iPageIndex)
        {
            customList._SetPage(iPageIndex);//设置列表项

            //

            _SetSlot();//更新区块状态
        }

        //----------------------------------------------------------------------
        // 功能说明：点击列表项、更新图像数据后，设置【VIEW TOOL GRAPHICS】、【BACKUP SINGLE IMAGE】、【BACKUP ALL IMAGES】和【CLEAR ALL】按钮
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetFunctionalButton()
        {
            if (bSaving || bClearing)//备份或清除
            {
                //不执行操作
            }
            else//其它
            {
                if (customList.ItemDataNumber > 0)//存在有效的图像数据
                {
                    if (bViewToolGraphics)//【VIEW TOOL GRAPHICS】按钮按下
                    {
                        customButtonViewToolGraphics.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//更新【VIEW TOOL GRAPHICS】按钮的背景
                    }
                    else//【VIEW TOOL GRAPHICS】按钮未按下
                    {
                        customButtonViewToolGraphics.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//更新【VIEW TOOL GRAPHICS】按钮的背景
                    }

                    try
                    {
                        Int32 iCurIndex = _ConvertIndex(customList.CurrentListIndex);

                        if (0 <= iCurIndex && iCurIndex < camera.Rejects.GraphicsInformation.Length)
                        {
                            if (camera.Rejects.GraphicsInformation[iCurIndex].Valid)//当前所选图像数据有效
                            {
                                if (CustomButton_BackgroundImage.Disable == (customButtonBackupSingleImage.CustomButtonBackgroundImage & CustomButton_BackgroundImage.Disable))//无效
                                {
                                    customButtonBackupSingleImage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//更新【BACKUP SINGLE IMAGE】按钮的背景
                                }
                            }
                            else//当前所选图像数据无效
                            {
                                customButtonBackupSingleImage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【BACKUP SINGLE IMAGE】按钮的背景
                            }
                        }
                        else
                        {
                            customButtonBackupSingleImage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【BACKUP SINGLE IMAGE】按钮的背景
                        }
                    }
                    catch (System.Exception ex)
                    {
                        customButtonBackupSingleImage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【BACKUP SINGLE IMAGE】按钮的背景
                    }

                    if (CustomButton_BackgroundImage.Disable == (customButtonBackupAllImages.CustomButtonBackgroundImage & CustomButton_BackgroundImage.Disable))//无效
                    {
                        customButtonBackupAllImages.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//更新【BACKUP ALL IMAGES】按钮的背景
                    }

                    if (CustomButton_BackgroundImage.Disable == (customButtonClearAll.CustomButtonBackgroundImage & CustomButton_BackgroundImage.Disable))//无效
                    {
                        customButtonClearAll.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//更新【CLEAR ALL】按钮的背景
                    }

                    //

                    customButtonPreviousPage.Visible = true;//【Previous Page】
                    customButtonNextPage.Visible = true;//【Next Page】
                }
                else//不存在有效的图像数据
                {
                    customButtonViewToolGraphics.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【VIEW TOOL GRAPHICS】按钮的背景

                    customButtonBackupSingleImage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【BACKUP SINGLE IMAGE】按钮的背景
                    customButtonBackupAllImages.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【BACKUP ALL IMAGES】按钮的背景
                    customButtonClearAll.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【CLEAR ALL】按钮的背景

                    //

                    customButtonPreviousPage.Visible = false;//【Previous Page】
                    customButtonNextPage.Visible = false;//【Next Page】
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：更新各个SLOT区块状态
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetSlot()
        {
            if (4 == customList.TotalPage)
            {
                customButtonSlot_1.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                rejectsSlot1.Enabled = true;

                customButtonSlot_2.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                rejectsSlot2.Enabled = true;

                customButtonSlot_3.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                rejectsSlot3.Enabled = true;

                customButtonSlot_4.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                rejectsSlot4.Enabled = true;
            }
            else if (3 == customList.TotalPage)
            {
                customButtonSlot_1.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                rejectsSlot1.Enabled = false;

                //

                customButtonSlot_2.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                rejectsSlot2.Enabled = true;

                customButtonSlot_3.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                rejectsSlot3.Enabled = true;

                customButtonSlot_4.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                rejectsSlot4.Enabled = true;
            }
            else if (2 == customList.TotalPage)
            {
                customButtonSlot_1.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                rejectsSlot1.Enabled = false;

                customButtonSlot_2.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                rejectsSlot2.Enabled = false;

                //

                customButtonSlot_3.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                rejectsSlot3.Enabled = true;

                customButtonSlot_4.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                rejectsSlot4.Enabled = true;
            }
            else if (1 == customList.TotalPage)
            {
                customButtonSlot_1.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                rejectsSlot1.Enabled = false;

                customButtonSlot_2.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                rejectsSlot2.Enabled = false;

                customButtonSlot_3.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                rejectsSlot3.Enabled = false;

                //

                customButtonSlot_4.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                rejectsSlot4.Enabled = true;
            }
            else//0
            {
                customButtonSlot_1.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                rejectsSlot1.Enabled = false;

                customButtonSlot_2.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                rejectsSlot2.Enabled = false;

                customButtonSlot_3.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                rejectsSlot3.Enabled = false;

                customButtonSlot_4.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;
                rejectsSlot4.Enabled = false;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：更新SLOT中的当前区块
        // 输入参数：1.iPage：当前页面
        //         2.iPageIndex：页中的项的索引值（0 ~ 11）
        //         3.bValue：备份文件或清除数据时使用，数值。取值范围：true，成功；false：失败
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetSlot(int iPage, int iPageIndex, bool bValue = false)
        {
            try
            {
                if (0 <= iPageIndex && iPageIndex < rejectsSlot1.Value.Length)//有效
                {
                    if (bSaving || bClearing)//正在备份文件或清除数据
                    {
                        if (0 == iPage)//SLOT 1
                        {
                            rejectsSlot1.Value[iPageIndex] = bValue;//属性，bSaving取值为true时使用，区块的数值。取值范围：true，表示保存成功；false，表示保存失败
                            rejectsSlot1.Valid[iPageIndex] = true;//属性，bSaving取值为true时使用，区块是否有效。true：是；false：否

                            rejectsSlot1._Update();//更新SLOT
                        }
                        else if (1 == iPage)//SLOT 2
                        {
                            rejectsSlot2.Value[iPageIndex] = bValue;//属性，bSaving取值为true时使用，区块的数值。取值范围：true，表示保存成功；false，表示保存失败
                            rejectsSlot2.Valid[iPageIndex] = true;//属性，bSaving取值为true时使用，区块是否有效。true：是；false：否

                            rejectsSlot2._Update();//更新SLOT
                        }
                        else if (2 == iPage)//SLOT 3
                        {
                            rejectsSlot3.Value[iPageIndex] = bValue;//属性，bSaving取值为true时使用，区块的数值。取值范围：true，表示保存成功；false，表示保存失败
                            rejectsSlot3.Valid[iPageIndex] = true;//属性，bSaving取值为true时使用，区块是否有效。true：是；false：否

                            rejectsSlot3._Update();//更新SLOT
                        }
                        else if (3 == iPage)//SLOT 4
                        {
                            rejectsSlot4.Value[iPageIndex] = bValue;//属性，bSaving取值为true时使用，区块的数值。取值范围：true，表示保存成功；false，表示保存失败
                            rejectsSlot4.Valid[iPageIndex] = true;//属性，bSaving取值为true时使用，区块是否有效。true：是；false：否

                            rejectsSlot4._Update();//更新SLOT
                        }
                        else//无效
                        {
                            //不执行操作
                        }
                    }
                    else//未保存数据
                    {
                        if (0 == iPage)//SLOT 1
                        {
                            rejectsSlot1.Position = iPageIndex;//bSaving取值为false时使用，当前区块的位置（从0开始，取值为-1，表示不存在有效的区块）
                        }
                        else if (1 == iPage)//SLOT 2
                        {
                            rejectsSlot2.Position = iPageIndex;//bSaving取值为false时使用，当前区块的位置（从0开始，取值为-1，表示不存在有效的区块）
                        }
                        else if (2 == iPage)//SLOT 3
                        {
                            rejectsSlot3.Position = iPageIndex;//bSaving取值为false时使用，当前区块的位置（从0开始，取值为-1，表示不存在有效的区块）
                        }
                        else if (3 == iPage)//SLOT 4
                        {
                            rejectsSlot4.Position = iPageIndex;//bSaving取值为false时使用，当前区块的位置（从0开始，取值为-1，表示不存在有效的区块）
                        }
                        else//无效
                        {
                            //不执行操作
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
            	//不执行操作
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：更新各个SLOT区块
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SelectSlot()
        {
            Int32 iPage = _ConvertPage(customList.CurrentPage);

            if (0 == iPage)//SLOT 1
            {
                rejectsSlot1.Current = true;//当前SLOT
                //
                rejectsSlot2.Current = false;//非当前SLOT
                rejectsSlot3.Current = false;//非当前SLOT
                rejectsSlot4.Current = false;//非当前SLOT
            }
            else if (1 == iPage)//SLOT 2
            {
                rejectsSlot2.Current = true;//当前SLOT
                //
                rejectsSlot1.Current = false;//非当前SLOT
                rejectsSlot3.Current = false;//非当前SLOT
                rejectsSlot4.Current = false;//非当前SLOT
            }
            else if (2 == iPage)//SLOT 3
            {
                rejectsSlot3.Current = true;//当前SLOT
                //
                rejectsSlot1.Current = false;//非当前SLOT
                rejectsSlot2.Current = false;//非当前SLOT
                rejectsSlot4.Current = false;//非当前SLOT
            }
            else if (3 == iPage)//SLOT 4
            {
                rejectsSlot4.Current = true;//当前SLOT
                //
                rejectsSlot1.Current = false;//非当前SLOT
                rejectsSlot2.Current = false;//非当前SLOT
                rejectsSlot3.Current = false;//非当前SLOT
            }
            else//无效
            {
                //不执行操作
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：启动备份或清除图像时，设置所有Slot控件中的区块
        // 输入参数：1.bWorking：备份或清除的状态。取值范围：bSaving，bClearing
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetAllSlot(bool bBackupClear)
        {
            for (int i = 0; i < rejectsSlot1.Value.Length; i++)
            {
                //初始值

                rejectsSlot1.Value[i] = false;//属性，bSaving取值为true时使用，区块的数值。取值范围：true，表示保存成功；false，表示保存失败
                rejectsSlot1.Valid[i] = false;//属性，bSaving取值为true时使用，区块是否有效。true：是；false：否

                rejectsSlot2.Value[i] = false;//属性，bSaving取值为true时使用，区块的数值。取值范围：true，表示保存成功；false，表示保存失败
                rejectsSlot2.Valid[i] = false;//属性，bSaving取值为true时使用，区块是否有效。true：是；false：否

                rejectsSlot3.Value[i] = false;//属性，bSaving取值为true时使用，区块的数值。取值范围：true，表示保存成功；false，表示保存失败
                rejectsSlot3.Valid[i] = false;//属性，bSaving取值为true时使用，区块是否有效。true：是；false：否

                rejectsSlot4.Value[i] = false;//属性，bSaving取值为true时使用，区块的数值。取值范围：true，表示保存成功；false，表示保存失败
                rejectsSlot4.Valid[i] = false;//属性，bSaving取值为true时使用，区块是否有效。true：是；false：否
            }

            rejectsSlot1.Working = bBackupClear;//属性，是否正在备份或清除图像数据。true：是；false：否
            rejectsSlot2.Working = bBackupClear;//属性，是否正在备份或清除图像数据。true：是；false：否
            rejectsSlot3.Working = bBackupClear;//属性，是否正在备份或清除图像数据。true：是；false：否
            rejectsSlot4.Working = bBackupClear;//属性，是否正在备份或清除图像数据。true：是；false：否
        }

        //----------------------------------------------------------------------
        // 功能说明：清除所有Slot控件中的区块数值（非备份或清除图像时使用）
        // 输入参数：1.bWorking：备份或清除的状态。
        //         2.bDisplay：是否显示当前选择的列表项对应的区块。取值范围：true，是；false，否
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _ClearAllSlot()
        {
            rejectsSlot1.Position = -1;//不显示任何区块
            rejectsSlot2.Position = -1;//不显示任何区块
            rejectsSlot3.Position = -1;//不显示任何区块
            rejectsSlot4.Position = -1;//不显示任何区块
        }

        //----------------------------------------------------------------------
        // 功能说明：点击列表项控件所进行的操作
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _ClickItem()
        {
            bClickListItem = true;//直至接收到点击的指令，否则不再更新页面

            //

            _ClearAllSlot();//将所有Slot控件中的区块均设置为无效

            _SetSlot(_ConvertPage(customList.CurrentPage), customList._GetPageItemIndex(_ConvertIndex(customList.CurrentListIndex)));//设置SLOT中的当前区块

            //

            _SetFunctionalButton();//设置【BACKUP SINGLE IMAGE】、【BACKUP ALL IMAGES】和【CLEAR ALL】按钮

            //事件

            if (null != Item_Click)//有效
            {
                Item_Click(this, new CustomEventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【Previous Page】或【Next Page】按钮时进行相关操作
        // 输入参数：1.bPreviousNext：点击的按钮的类型。取值范围：true：【Previous Page】按钮；【Next Page】按钮
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _ClickPage(bool bPreviousNext)
        {
            customList._ClickPage(bPreviousNext);

            _SelectSlot();

            //事件

            if (bPreviousNext)//【Previous Page】
            {
                if (null != PreviousPage_Click)//有效
                {
                    PreviousPage_Click(this, new CustomEventArgs());
                }
            }
            else//【Next Page】
            {
                if (null != NextPage_Click)//有效
                {
                    NextPage_Click(this, new CustomEventArgs());
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击SLOT控件时进行相关操作
        // 输入参数：1.iPage：更新后的当前页码
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _ClickSlot(int iPage)
        {
            _SetPage(_ConvertPage(iPage));

            _SelectSlot();//设置各个SLOT

            //事件

            if (null != Slot_Click)//有效
            {
                Slot_Click(this, new CustomEventArgs());
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
        private void RejectsControl_Load(object sender, EventArgs e)
        {
            //_SetDefault();//使用默认值设置并显示控件
        }

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
        // 功能说明：点击列表项事件，更新控件背景，执行相应的操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customList_CustomListItem_Click(object sender, EventArgs e)
        {
            _ClickItem();//点击列表项控件1
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
            _ClickPage(true);//点击【Previous Page】按钮后的操作
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
            _ClickPage(false);//点击【Next Page】按钮后的操作
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：点击【VIEW TOOL GRAPHICS】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonViewToolGraphics_CustomButton_Click(object sender, EventArgs e)
        {
            bViewToolGraphics = !bViewToolGraphics;

            //事件

            if (null != ViewToolGraphics_Click)//有效
            {
                ViewToolGraphics_Click(this, new CustomEventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【BACKUP SINGLE IMAGE】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonBackupSingleImage_CustomButton_Click(object sender, EventArgs e)
        {
            bSaving_SingleorAll = true;//保存图像的类型。取值范围：true：保存单个图像；false：保存所有图像

            //显示信息对话框

            GlobalWindows.MessageDisplay_Window.WindowParameter = 42;//窗口特征数值
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.OkCancel;//包含【确定】和【取消】按钮
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][5];
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_3 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][5];
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_5 = (_ConvertIndex(customList.CurrentListIndex) + 1).ToString() + " " + camera.Rejects.GraphicsInformation[_ConvertIndex(customList.CurrentListIndex)].Name + "？";
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_5 = (_ConvertIndex(customList.CurrentListIndex) + 1).ToString() + " " + camera.Rejects.GraphicsInformation[_ConvertIndex(customList.CurrentListIndex)].Name + "？";

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
        // 功能说明：点击【BACKUP ALL IMAGES】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonBackupAllImages_CustomButton_Click(object sender, EventArgs e)
        {
            bSaving_SingleorAll = false;//保存图像的类型。取值范围：true：保存单个图像；false：保存所有图像

            //显示信息对话框

            GlobalWindows.MessageDisplay_Window.WindowParameter = 43;//窗口特征数值
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.OkCancel;//包含【确定】和【取消】按钮
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][6] + "？";
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][6] + "？";

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
        // 功能说明：点击【CLEAR ALL】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonClearAll_CustomButton_Click(object sender, EventArgs e)
        {
            //显示信息对话框

            GlobalWindows.MessageDisplay_Window.WindowParameter = 44;//窗口特征数值
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.OkCancel;//包含【确定】和【取消】按钮
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][7] + "？";
            GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][7] + "？";

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

        //

        //----------------------------------------------------------------------
        // 功能说明：点击SLOT4控件时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void Slot4_Click(object sender, EventArgs e)
        {
            _ClickSlot(3);//执行操作
        }

        //----------------------------------------------------------------------
        // 功能说明：点击SLOT3控件时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void Slot3_Click(object sender, EventArgs e)
        {
            _ClickSlot(2);//执行操作
        }

        //----------------------------------------------------------------------
        // 功能说明：点击SLOT2控件时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void Slot2_Click(object sender, EventArgs e)
        {
            _ClickSlot(1);//执行操作
        }

        //----------------------------------------------------------------------
        // 功能说明：点击SLOT1控件时产生的事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void Slot1_Click(object sender, EventArgs e)
        {
            _ClickSlot(0);//执行操作
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：REJECTS，【BACKUP SINGLE IMAGE】确认，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_Rejects_BackupSingleImage_Confirm(object sender, EventArgs e)
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

            if (GlobalWindows.MessageDisplay_Window.MessageDisplayControl.OkCancel)//启动备份
            {
                customList.ListEnabled = false;//列表

                rejectsSlot4.Enabled = false;//SLOT4控件
                rejectsSlot3.Enabled = false;//SLOT3控件
                rejectsSlot2.Enabled = false;//SLOT2控件
                rejectsSlot1.Enabled = false;//SLOT1控件

                customButtonViewToolGraphics.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【VIEW TOOL GRAPHICS】按钮的背景
                customButtonBackupSingleImage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【BACKUP SINGLE IMAGE】按钮的背景
                customButtonBackupAllImages.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【BACKUP ALL IMAGES】按钮的背景
                customButtonClearAll.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【CLEAR ALL】按钮的背景
                customButtonClose.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【返回】按钮的背景
                customButtonPreviousPage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【Previous Page】按钮的背景
                customButtonNextPage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【Next Page】按钮的背景

                //

                _StartBackup();//启动备份

                //事件

                if (null != BackupSingleImage_Click)//有效
                {
                    BackupSingleImage_Click(this, new CustomEventArgs());
                }
            }
            else//不进行备份
            {
                //不执行操作
            }
        }
        
        //----------------------------------------------------------------------
        // 功能说明：REJECTS，【BACKUP ALL IMAGES】确认，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_Rejects_BackupAllImages_Confirm(object sender, EventArgs e)
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

            if (GlobalWindows.MessageDisplay_Window.MessageDisplayControl.OkCancel)//启动备份
            {
                customList.ListEnabled = false;//列表

                rejectsSlot4.Enabled = false;//SLOT4控件
                rejectsSlot3.Enabled = false;//SLOT3控件
                rejectsSlot2.Enabled = false;//SLOT2控件
                rejectsSlot1.Enabled = false;//SLOT1控件

                customButtonViewToolGraphics.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【VIEW TOOL GRAPHICS】按钮的背景
                customButtonBackupSingleImage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【BACKUP SINGLE IMAGE】按钮的背景
                customButtonBackupAllImages.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【BACKUP ALL IMAGES】按钮的背景
                customButtonClearAll.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【CLEAR ALL】按钮的背景
                customButtonClose.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【返回】按钮的背景
                customButtonPreviousPage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【Previous Page】按钮的背景
                customButtonNextPage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【Next Page】按钮的背景

                //

                _StartBackup();//启动备份

                //事件

                if (null != BackupAllImages_Click)//有效
                {
                    BackupAllImages_Click(this, new CustomEventArgs());
                }
            }
            else//不进行备份
            {
                //不执行操作
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：REJECTS，【BACKUP ALL IMAGES】等待，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_Rejects_BackupAllImages_Wait(object sender, EventArgs e)
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
        // 功能说明：REJECTS，【CLEAR ALL】确认，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_Rejects_ClearAll_Confirm(object sender, EventArgs e)
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

            if (GlobalWindows.MessageDisplay_Window.MessageDisplayControl.OkCancel)//启动清除数据
            {
                customList.ListEnabled = false;//列表

                rejectsSlot4.Enabled = false;//SLOT4控件
                rejectsSlot3.Enabled = false;//SLOT3控件
                rejectsSlot2.Enabled = false;//SLOT2控件
                rejectsSlot1.Enabled = false;//SLOT1控件

                customButtonViewToolGraphics.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【VIEW TOOL GRAPHICS】按钮的背景
                customButtonBackupSingleImage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【BACKUP SINGLE IMAGE】按钮的背景
                customButtonBackupAllImages.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【BACKUP ALL IMAGES】按钮的背景
                customButtonClearAll.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【CLEAR ALL】按钮的背景
                customButtonClose.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【返回】按钮的背景
                customButtonPreviousPage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【Previous Page】按钮的背景
                customButtonNextPage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//更新【Next Page】按钮的背景

                //

                _StartClearAllImages();//启动清除图像数据

                //事件

                if (null != ClearAll_Click)//有效
                {
                    ClearAll_Click(this, new CustomEventArgs());
                }
            }
            else//不进行清除
            {
                //不执行操作
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：REJECTS，【CLEAR ALL】等待，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_Rejects_ClearAll_Wait(object sender, EventArgs e)
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