/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：StandardKeyboardWindow.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：标准键盘窗口

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

namespace VisionSystemControlLibrary
{
    public partial class StandardKeyboardWindow : Form
    {
        //标准键盘窗口

        private Int32 iWindowParameter = 0;//属性，窗口特征数值，表示调用窗口的父级窗口类型，以便产生相应的事件。取值范围：
        //1.APPLICATION REGISTRATION，输入密钥
        //2.BACKUP BRANDS，CREATE FOLDER
        //3.BRAND，COPY BRAND
        //4.BRAND，RENAME BRAND
        //5.SYSTEM，密码
        //6.WORK，点击TRADEMARK图标显示密码窗口
        //7.WORK，点击【SYSTEM】按钮显示密码窗口
        //8.WORK，点击【DEVICES SETUP】按钮显示密码窗口
        //9.WORK，点击【BRAND MANAGEMENT】按钮显示密码窗口
        //10.WORK，点击【QUALITY CHECK】按钮显示密码窗口
        //11.WORK，点击【TOLERANCES】按钮显示密码窗口
        //12.WORK，点击【LIVE】按钮显示密码窗口
        //13.WORK，点击【REJECTS】按钮显示密码窗口
        //14.WORK，点击【系统更新】按钮显示密码窗口
        //15.WORK，点击【STATE】按钮显示密码窗口
        //16.SYSTEM，点击【STATE】按钮显示密码窗口
        //17.DEVICES SETUP，点击【STATE】按钮显示密码窗口
        //18.IMAGE CONFIGURATION，点击【STATE】按钮显示密码窗口
        //19.BRAND MANAGEMENT，点击【STATE】按钮显示密码窗口
        //20.BACKUP BRANDS，点击【STATE】按钮显示密码窗口
        //21.RESTORE BRANDS，点击【STATE】按钮显示密码窗口
        //22.QUALITY CHECK，点击【STATE】按钮显示密码窗口
        //23.TOLERANCES SETTINGS，点击【STATE】按钮显示密码窗口
        //24.LIVE VIEW，点击【STATE】按钮显示密码窗口
        //25.REJECTS VIEW，点击【STATE】按钮显示密码窗口
        //26.STATISTICS，点击【STATE】按钮显示密码窗口
        //27.WORK，点击【STATISTICS】按钮显示密码窗口
        //28.EDIT TOOLS，NEW TOOL
        //29.EDIT TOOLS，COPY TOOL
        //30.EDIT TOOLS，RENAME TOOL
        
        //

        [Browsable(true), Description("APPLICATION REGISTRATION，输入密钥，窗口关闭时产生的事件"), Category("StandardKeyboardWindow 事件")]
        public event EventHandler WindowClose_ApplicationRegistration;//APPLICATION REGISTRATION，输入密钥，窗口关闭时产生的事件

        //

        [Browsable(true), Description("BACKUP BRANDS，CREATE FOLDER，窗口关闭时产生的事件"), Category("StandardKeyboardWindow 事件")]
        public event EventHandler WindowClose_BackupBrands_CreateFolder;//BACKUP BRANDS，CREATE FOLDER，窗口关闭时产生的事件

        //

        [Browsable(true), Description("BRAND，COPY BRAND，窗口关闭时产生的事件"), Category("StandardKeyboardWindow 事件")]
        public event EventHandler WindowClose_Brand_CopyBrand;//BRAND，COPY BRAND，窗口关闭时产生的事件

        [Browsable(true), Description("BRAND，RENAME BRAND，窗口关闭时产生的事件"), Category("StandardKeyboardWindow 事件")]
        public event EventHandler WindowClose_Brand_RenameBrand;//BRAND，RENAME BRAND，窗口关闭时产生的事件

        //

        [Browsable(true), Description("SYSTEM，密码，窗口关闭时产生的事件"), Category("StandardKeyboardWindow 事件")]
        public event EventHandler WindowClose_System_Password;//SYSTEM，密码，窗口关闭时产生的事件

        //

        [Browsable(true), Description("WORK，点击TRADEMARK图标显示密码窗口，窗口关闭时产生的事件"), Category("StandardKeyboardWindow 事件")]
        public event EventHandler WindowClose_Work_Trademark;//WORK，点击TRADEMARK图标显示密码窗口，窗口关闭时产生的事件

        [Browsable(true), Description("WORK，点击【SYSTEM】按钮显示密码窗口，窗口关闭时产生的事件"), Category("StandardKeyboardWindow 事件")]
        public event EventHandler WindowClose_Work_System;//WORK，点击【SYSTEM】按钮显示密码窗口，窗口关闭时产生的事件

        [Browsable(true), Description("WORK，点击【DEVICES SETUP】按钮显示密码窗口，窗口关闭时产生的事件"), Category("StandardKeyboardWindow 事件")]
        public event EventHandler WindowClose_Work_DevicesSetup;//WORK，点击【DEVICES SETUP】按钮显示密码窗口，窗口关闭时产生的事件

        [Browsable(true), Description("WORK，点击【BRAND MANAGEMENT】按钮显示密码窗口，窗口关闭时产生的事件"), Category("StandardKeyboardWindow 事件")]
        public event EventHandler WindowClose_Work_BrandManagement;//WORK，点击【BRAND MANAGEMENT】按钮显示密码窗口，窗口关闭时产生的事件

        [Browsable(true), Description("WORK，点击【QUALITY CHECK】按钮显示密码窗口，窗口关闭时产生的事件"), Category("StandardKeyboardWindow 事件")]
        public event EventHandler WindowClose_Work_QualityCheck;//WORK，点击【QUALITY CHECK】按钮显示密码窗口，窗口关闭时产生的事件

        [Browsable(true), Description("WORK，点击【TOLERANCES】按钮显示密码窗口，窗口关闭时产生的事件"), Category("StandardKeyboardWindow 事件")]
        public event EventHandler WindowClose_Work_Tolerances;//WORK，点击【TOLERANCES】按钮显示密码窗口，窗口关闭时产生的事件

        [Browsable(true), Description("WORK，点击【LIVE】按钮显示密码窗口，窗口关闭时产生的事件"), Category("StandardKeyboardWindow 事件")]
        public event EventHandler WindowClose_Work_Live;//WORK，点击【LIVE】按钮显示密码窗口，窗口关闭时产生的事件

        [Browsable(true), Description("WORK，点击【REJECTS】按钮显示密码窗口，窗口关闭时产生的事件"), Category("StandardKeyboardWindow 事件")]
        public event EventHandler WindowClose_Work_Rejects;//WORK，点击【REJECTS】按钮显示密码窗口，窗口关闭时产生的事件

        [Browsable(true), Description("WORK，点击【系统更新】按钮显示密码窗口，窗口关闭时产生的事件"), Category("StandardKeyboardWindow 事件")]
        public event EventHandler WindowClose_Work_Update;//WORK，点击【系统更新】按钮显示密码窗口，窗口关闭时产生的事件

        [Browsable(true), Description("WORK，点击【STATE】按钮显示密码窗口，窗口关闭时产生的事件"), Category("StandardKeyboardWindow 事件")]
        public event EventHandler WindowClose_Work_State;//WORK，点击【STATE】按钮显示密码窗口，窗口关闭时产生的事件

        [Browsable(true), Description("SYSTEM，点击【STATE】按钮显示密码窗口，窗口关闭时产生的事件"), Category("StandardKeyboardWindow 事件")]
        public event EventHandler WindowClose_System_State;//SYSTEM，点击【STATE】按钮显示密码窗口，窗口关闭时产生的事件

        [Browsable(true), Description("DEVICES SETUP，点击【STATE】按钮显示密码窗口，窗口关闭时产生的事件"), Category("StandardKeyboardWindow 事件")]
        public event EventHandler WindowClose_DevicesSetup_State;//DEVICES SETUP，点击【STATE】按钮显示密码窗口，窗口关闭时产生的事件

        [Browsable(true), Description("IMAGE CONFIGURATION，点击【STATE】按钮显示密码窗口，窗口关闭时产生的事件"), Category("StandardKeyboardWindow 事件")]
        public event EventHandler WindowClose_ImageConfiguration_State;//IMAGE CONFIGURATION，点击【STATE】按钮显示密码窗口，窗口关闭时产生的事件

        [Browsable(true), Description("BRAND MANAGEMENT，点击【STATE】按钮显示密码窗口，窗口关闭时产生的事件"), Category("StandardKeyboardWindow 事件")]
        public event EventHandler WindowClose_BrandManagement_State;//BRAND MANAGEMENT，点击【STATE】按钮显示密码窗口，窗口关闭时产生的事件

        [Browsable(true), Description("BACKUP BRANDS，点击【STATE】按钮显示密码窗口，窗口关闭时产生的事件"), Category("StandardKeyboardWindow 事件")]
        public event EventHandler WindowClose_BackupBrands_State;//BACKUP BRANDS，点击【STATE】按钮显示密码窗口，窗口关闭时产生的事件

        [Browsable(true), Description("RESTORE BRANDS，点击【STATE】按钮显示密码窗口，窗口关闭时产生的事件"), Category("StandardKeyboardWindow 事件")]
        public event EventHandler WindowClose_RestoreBrands_State;//RESTORE BRANDS，点击【STATE】按钮显示密码窗口，窗口关闭时产生的事件

        [Browsable(true), Description("QUALITY CHECK，点击【STATE】按钮显示密码窗口，窗口关闭时产生的事件"), Category("StandardKeyboardWindow 事件")]
        public event EventHandler WindowClose_QualityCheck_State;//QUALITY CHECK，点击【STATE】按钮显示密码窗口，窗口关闭时产生的事件

        [Browsable(true), Description("TOLERANCES SETTINGS，点击【STATE】按钮显示密码窗口，窗口关闭时产生的事件"), Category("StandardKeyboardWindow 事件")]
        public event EventHandler WindowClose_TolerancesSettings_State;//TOLERANCES SETTINGS，点击【STATE】按钮显示密码窗口，窗口关闭时产生的事件

        [Browsable(true), Description("LIVE VIEW，点击【STATE】按钮显示密码窗口，窗口关闭时产生的事件"), Category("StandardKeyboardWindow 事件")]
        public event EventHandler WindowClose_LiveView_State;//LIVE VIEW，点击【STATE】按钮显示密码窗口，窗口关闭时产生的事件

        [Browsable(true), Description("REJECTS VIEW，点击【STATE】按钮显示密码窗口，窗口关闭时产生的事件"), Category("StandardKeyboardWindow 事件")]
        public event EventHandler WindowClose_RejectsView_State;//REJECTS VIEW，点击【STATE】按钮显示密码窗口，窗口关闭时产生的事件

        [Browsable(true), Description("STATISTICS，点击【STATE】按钮显示密码窗口，窗口关闭时产生的事件"), Category("StandardKeyboardWindow 事件")]
        public event EventHandler WindowClose_Statistics_State;//STATISTICS，点击【STATE】按钮显示密码窗口，窗口关闭时产生的事件

        [Browsable(true), Description("WORK，点击【STATISTICS】按钮显示密码窗口，窗口关闭时产生的事件"), Category("StandardKeyboardWindow 事件")]
        public event EventHandler WindowClose_Work_Statistics;//WORK，点击【STATISTICS】按钮显示密码窗口，窗口关闭时产生的事件

        //

        [Browsable(true), Description("EDIT TOOLS，NEW TOOL，窗口关闭时产生的事件"), Category("StandardKeyboardWindow 事件")]
        public event EventHandler WindowClose_EditTools_NewTool;//EDIT TOOLS，NEW TOOL，窗口关闭时产生的事件

        [Browsable(true), Description("EDIT TOOLS，COPY TOOL，窗口关闭时产生的事件"), Category("StandardKeyboardWindow 事件")]
        public event EventHandler WindowClose_EditTools_CopyTool;//EDIT TOOLS，COPY TOOL，窗口关闭时产生的事件

        [Browsable(true), Description("EDIT TOOLS，RENAME TOOL，窗口关闭时产生的事件"), Category("StandardKeyboardWindow 事件")]
        public event EventHandler WindowClose_EditTools_RenameTool;//EDIT TOOLS，RENAME TOOL，窗口关闭时产生的事件

        //构造函数

        //----------------------------------------------------------------------
        // 功能说明：构造函数，初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public StandardKeyboardWindow()
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
        [Browsable(true), Description("语言"), Category("StandardKeyboard 通用")]
        public VisionSystemClassLibrary.Enum.InterfaceLanguage Language
        {
            get
            {
                return standardKeyboard.Language;
            }
            set//设置
            {
                if (value != standardKeyboard.Language)
                {
                    standardKeyboard.Language = value;
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Chinese_Caption属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("中文标题名称"), Category("StandardKeyboard 通用")]
        public String Chinese_Caption
        {
            set//设置
            {
                if ("" != value && value != standardKeyboard.Chinese_Caption)
                {
                    standardKeyboard.Chinese_Caption = value;
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：English_Caption属性的实现
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("英文标题名称"), Category("StandardKeyboard 通用")]
        public String English_Caption
        {
            set//设置
            {
                if ("" != value && value != standardKeyboard.English_Caption)
                {
                    standardKeyboard.English_Caption = value;
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：InvalidCharacter属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("不能包含的文本。取值范围：true，是；false，否"), Category("StandardKeyboard 通用")]
        public String[] InvalidCharacter
        {
            set//设置
            {
                if (value != standardKeyboard.InvalidCharacter)
                {
                    standardKeyboard.InvalidCharacter = value;
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：CapsLock属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("Caps Lock。取值范围：true，打开；false，关闭"), Category("StandardKeyboard 通用")]
        public Boolean CapsLock
        {
            set//设置
            {
                if (value != standardKeyboard.CapsLock)
                {
                    standardKeyboard.CapsLock = value;
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Shift属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("Shift。取值范围：true，按下；false，弹起"), Category("StandardKeyboard 通用")]
        public Boolean Shift
        {
            set//设置
            {
                if (value != standardKeyboard.Shift)
                {
                    standardKeyboard.Shift = value;
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：IsPassword属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("密码输入窗口。取值范围：true，是；false，否"), Category("StandardKeyboard 通用")]
        public Boolean IsPassword
        {
            set//设置
            {
                if (value != standardKeyboard.IsPassword)
                {
                    standardKeyboard.IsPassword = value;
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：PasswordStyle属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("密码输入类型。取值范围：0，密码输入（输入完成，正确，关闭窗口）；1，输入当前密码；2，输入新的密码；3，确认密码"), Category("StandardKeyboard 通用")]
        public Int32 PasswordStyle
        {
            set//设置
            {
                if (value != standardKeyboard.PasswordStyle)
                {
                    standardKeyboard.PasswordStyle = value;
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Password属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("当前密码（可能包含多个密码）"), Category("StandardKeyboard 通用")]
        public String Password
        {
            set//设置
            {
                if (value != standardKeyboard.Password)
                {
                    standardKeyboard.Password = value;
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：MaxLength属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("最大长度"), Category("StandardKeyboard 通用")]
        public Byte MaxLength
        {
            set//设置
            {
                if (value != standardKeyboard.MaxLength)
                {
                    standardKeyboard.MaxLength = value;
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：StringValue属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("输入数值"), Category("StandardKeyboard 通用")]
        public String StringValue
        {
            get
            {
                return standardKeyboard.StringValue;
            }
            set//设置
            {
                standardKeyboard.StringValue = value;
            }
        }

        // 功能说明：EnterNewValue属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("是否输入了新的数值或密码输入是否正确。取值范围：true，是；false，否"), Category("StandardKeyboard 通用")]
        public Boolean EnterNewValue
        {
            get//读取
            {
                return standardKeyboard.EnterNewValue;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：WindowParameter属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("窗口特征数值，表示调用窗口的父级窗口类型，以便产生相应的事件"), Category("StandardKeyboardWindow 通用")]
        public Int32 WindowParameter//属性
        {
            get//读取
            {
                return iWindowParameter;
            }
            set//设置
            {
                iWindowParameter = value;
            }
        }

        //事件

        //-----------------------------------------------------------------------
        // 功能说明：点击【Esc】或【Enter】时产生的事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void standardKeyboard_Close_Click(object sender, EventArgs e)
        {
            //事件

            if (1 == iWindowParameter)//APPLICATION REGISTRATION，输入密钥
            {
                if (null != WindowClose_ApplicationRegistration)//有效
                {
                    WindowClose_ApplicationRegistration(this, new CustomEventArgs());
                }
            }
            else if (2 == iWindowParameter)//BACKUP BRANDS，CREATE FOLDER
            {
                if (null != WindowClose_BackupBrands_CreateFolder)//有效
                {
                    WindowClose_BackupBrands_CreateFolder(this, new CustomEventArgs());
                }
            }
            else if (3 == iWindowParameter)//BRAND，COPY BRAND
            {
                if (null != WindowClose_Brand_CopyBrand)//有效
                {
                    WindowClose_Brand_CopyBrand(this, new CustomEventArgs());
                }
            }
            else if (4 == iWindowParameter)//BRAND，RENAME BRAND
            {
                if (null != WindowClose_Brand_RenameBrand)//有效
                {
                    WindowClose_Brand_RenameBrand(this, new CustomEventArgs());
                }
            }
            else if (5 == iWindowParameter)//SYSTEM，密码
            {
                if (null != WindowClose_System_Password)//有效
                {
                    WindowClose_System_Password(this, new CustomEventArgs());
                }
            }
            else if (6 == iWindowParameter)//WORK，点击TRADEMARK图标显示密码窗口
            {
                if (null != WindowClose_Work_Trademark)//有效
                {
                    WindowClose_Work_Trademark(this, new CustomEventArgs());
                }
            }
            else if (7 == iWindowParameter)//WORK，点击【SYSTEM】按钮显示密码窗口
            {
                if (null != WindowClose_Work_System)//有效
                {
                    WindowClose_Work_System(this, new CustomEventArgs());
                }
            }
            else if (8 == iWindowParameter)//WORK，点击【DEVICES SETUP】按钮显示密码窗口
            {
                if (null != WindowClose_Work_DevicesSetup)//有效
                {
                    WindowClose_Work_DevicesSetup(this, new CustomEventArgs());
                }
            }
            else if (9 == iWindowParameter)//WORK，点击【BRAND MANAGEMENT】按钮显示密码窗口
            {
                if (null != WindowClose_Work_BrandManagement)//有效
                {
                    WindowClose_Work_BrandManagement(this, new CustomEventArgs());
                }
            }
            else if (10 == iWindowParameter)//WORK，点击【QUALITY CHECK】按钮显示密码窗口
            {
                if (null != WindowClose_Work_QualityCheck)//有效
                {
                    WindowClose_Work_QualityCheck(this, new CustomEventArgs());
                }
            }
            else if (11 == iWindowParameter)//WORK，点击【TOLERANCES】按钮显示密码窗口
            {
                if (null != WindowClose_Work_Tolerances)//有效
                {
                    WindowClose_Work_Tolerances(this, new CustomEventArgs());
                }
            }
            else if (12 == iWindowParameter)//WORK，点击【LIVE】按钮显示密码窗口
            {
                if (null != WindowClose_Work_Live)//有效
                {
                    WindowClose_Work_Live(this, new CustomEventArgs());
                }
            }
            else if (13 == iWindowParameter)//WORK，点击【REJECTS】按钮显示密码窗口
            {
                if (null != WindowClose_Work_Rejects)//有效
                {
                    WindowClose_Work_Rejects(this, new CustomEventArgs());
                }
            }
            else if (14 == iWindowParameter)//WORK，点击【系统更新】按钮显示密码窗口
            {
                if (null != WindowClose_Work_Update)//有效
                {
                    WindowClose_Work_Update(this, new CustomEventArgs());
                }
            }
            else if (15 == iWindowParameter)//WORK，点击【STATE】按钮显示密码窗口
            {
                if (null != WindowClose_Work_State)//有效
                {
                    WindowClose_Work_State(this, new CustomEventArgs());
                }
            }
            else if (16 == iWindowParameter)//SYSTEM，点击【STATE】按钮显示密码窗口
            {
                if (null != WindowClose_System_State)//有效
                {
                    WindowClose_System_State(this, new CustomEventArgs());
                }
            }
            else if (17 == iWindowParameter)//DEVICES SETUP，点击【STATE】按钮显示密码窗口
            {
                if (null != WindowClose_DevicesSetup_State)//有效
                {
                    WindowClose_DevicesSetup_State(this, new CustomEventArgs());
                }
            }
            else if (18 == iWindowParameter)//IMAGE CONFIGURATION，点击【STATE】按钮显示密码窗口
            {
                if (null != WindowClose_ImageConfiguration_State)//有效
                {
                    WindowClose_ImageConfiguration_State(this, new CustomEventArgs());
                }
            }
            else if (19 == iWindowParameter)//BRAND MANAGEMENT，点击【STATE】按钮显示密码窗口
            {
                if (null != WindowClose_BrandManagement_State)//有效
                {
                    WindowClose_BrandManagement_State(this, new CustomEventArgs());
                }
            }
            else if (20 == iWindowParameter)//BACKUP BRANDS，点击【STATE】按钮显示密码窗口
            {
                if (null != WindowClose_BackupBrands_State)//有效
                {
                    WindowClose_BackupBrands_State(this, new CustomEventArgs());
                }
            }
            else if (21 == iWindowParameter)//RESTORE BRANDS，点击【STATE】按钮显示密码窗口
            {
                if (null != WindowClose_RestoreBrands_State)//有效
                {
                    WindowClose_RestoreBrands_State(this, new CustomEventArgs());
                }
            }
            else if (22 == iWindowParameter)//QUALITY CHECK，点击【STATE】按钮显示密码窗口
            {
                if (null != WindowClose_QualityCheck_State)//有效
                {
                    WindowClose_QualityCheck_State(this, new CustomEventArgs());
                }
            }
            else if (23 == iWindowParameter)//TOLERANCES SETTINGS，点击【STATE】按钮显示密码窗口
            {
                if (null != WindowClose_TolerancesSettings_State)//有效
                {
                    WindowClose_TolerancesSettings_State(this, new CustomEventArgs());
                }
            }
            else if (24 == iWindowParameter)//LIVE VIEW，点击【STATE】按钮显示密码窗口
            {
                if (null != WindowClose_LiveView_State)//有效
                {
                    WindowClose_LiveView_State(this, new CustomEventArgs());
                }
            }
            else if (25 == iWindowParameter)//REJECTS VIEW，点击【STATE】按钮显示密码窗口
            {
                if (null != WindowClose_RejectsView_State)//有效
                {
                    WindowClose_RejectsView_State(this, new CustomEventArgs());
                }
            }
            else if (26 == iWindowParameter)//STATISTICS，点击【STATE】按钮显示密码窗口
            {
                if (null != WindowClose_Statistics_State)//有效
                {
                    WindowClose_Statistics_State(this, new CustomEventArgs());
                }
            }
            else if (27 == iWindowParameter)//WORK，点击【STATISTICS】按钮显示密码窗口
            {
                if (null != WindowClose_Work_Statistics)//有效
                {
                    WindowClose_Work_Statistics(this, new CustomEventArgs());
                }
            }
            else if (28 == iWindowParameter)//EDIT TOOLS，NEW TOOL
            {
                if (null != WindowClose_EditTools_NewTool)//有效
                {
                    WindowClose_EditTools_NewTool(this, new CustomEventArgs());
                }
            }
            else if (29 == iWindowParameter)//EDIT TOOLS，COPY TOOL
            {
                if (null != WindowClose_EditTools_CopyTool)//有效
                {
                    WindowClose_EditTools_CopyTool(this, new CustomEventArgs());
                }
            }
            else if (30 == iWindowParameter)//EDIT TOOLS，RENAME TOOL
            {
                if (null != WindowClose_EditTools_RenameTool)//有效
                {
                    WindowClose_EditTools_RenameTool(this, new CustomEventArgs());
                }
            }
            else//其它
            {
                //不执行操作
            }
        }
    }
}
