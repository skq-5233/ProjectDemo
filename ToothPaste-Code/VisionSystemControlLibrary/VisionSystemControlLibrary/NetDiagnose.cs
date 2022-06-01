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
    public partial class NetDiagnose : UserControl
    {
        //成员变量
        private List<string> sControllerNames = new List<string>();//控制器名称集合
        private List<VisionSystemClassLibrary.Enum.CameraType>[] sCameraTypes;//相机类型集合
        private List<string>[] sCameraNames;//相机类型集合

        private string sControllerName;//单个控制器名称

        private List<CustomButton> ControllerButtons = new List<CustomButton>();//控制器按钮
        private List<Label> ControllerConnect = new List<Label>();//控制器和人机界面连接状态控件
        private List<CustomButton> CameraButtons = new List<CustomButton>();//相机按钮
        private List<Label> CameraConnect = new List<Label>();//控制器和相机连接状态控件

        private Int32 iPageTotal;//控制器总页码
        private Int32 iCurrentPage = 0;//控制器当前页码

        private List<Int32> StateToInterface_Controller; //当前页人机界面和控制器的连接状态
        private List<Int32>[] StateToController_Camera; //当前页人机界面和控制器的连接状态

        private VisionSystemClassLibrary.Enum.InterfaceLanguage language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;//属性，语言
        private VisionSystemClassLibrary.Class.System system = new VisionSystemClassLibrary.Class.System();//属性（只读），属性
        private VisionSystemClassLibrary.Enum.CameraType cameratypeSelected = VisionSystemClassLibrary.Enum.CameraType.None;//属性（只读），点击列表项时，记录选择的相机类型（防止列表被更新）

        //事件

        [Browsable(true), Description("Controller连接产生的事件"), Category("NetDiagnose 事件")]
        public event EventHandler ControllerConnect_Click;//控制器连接产生的事件

        [Browsable(true), Description("Camera连接产生的事件"), Category("NetDiagnose 事件")]
        public event EventHandler CameraConnect_Click;//相机连接产生的事件

        [Browsable(true), Description("窗口关闭时产生的事件"), Category("NetCheck 事件")]
        public event EventHandler Close_Click;//窗口关闭时产生的事件

        //构造函数
        public NetDiagnose()
        {
            InitializeComponent();
        }

        //属性

        //----------------------------------------------------------------------
        // 功能说明：CameraSelected属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("点击列表项时，记录选择的相机类型（防止列表被更新）"), Category("NetDiagnose 通用")]
        public VisionSystemClassLibrary.Enum.CameraType CameraSelected
        {
            get
            {
                return cameratypeSelected;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：获取被点击控制器的名称
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public string ControllerName
        {
            get
            {
                return sControllerName;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Language属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("语言"), Category("NetDiagnose 通用")]
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

                    _SetLanguage();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：应用设置的属性
        // 输入参数：系统参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _Properties(VisionSystemClassLibrary.Class.System system_parameter)
        {
            system = system_parameter;

            _GetControllerInfo();//控制器信息
            _GetCameraInfo();//相机信息

            _GetTotalPage(sControllerNames.Count);//总页数

            if (iPageTotal <= 1)//总页数为1时，不显示上下翻页按钮
            {
                customButtonPreviousPage.Visible = false;
                customButtonNextPage.Visible = false;
            }
            else//显示上下翻页按钮
            {
                customButtonPreviousPage.Visible = true;
                customButtonNextPage.Visible = true;
            }

            StateToInterface_Controller = new List<Int32>();
            for (var i = 0; i < sControllerNames.Count; i++)//初始控制器连接状态
            {
                StateToInterface_Controller.Add(-1);//-1:表示当前没有判断控制器是否和人机界面连接成功；0:连接失败；1：连接成功
            }

            StateToController_Camera = new List<Int32>[sControllerNames.Count];
            for (var i = 0; i < StateToController_Camera.Length; i++)//初始控制器连接状态
            {
                StateToController_Camera[i] = new List<int>();
                for (var j = 0; j < sCameraTypes[i].Count; j++)//初始相机连接状态
                {
                    StateToController_Camera[i].Add(-1);//-1表示当前没有判断相机是否和控制器连接成功
                }
            }

            _ShowConnectState(iCurrentPage);

            this.Invalidate();
        }

        //----------------------------------------------------------------------
        // 功能说明：设置语言
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _SetLanguage()
        {
            Controller1.Language = language;
            Controller2.Language = language;
            Camera1.Language = language;
            Camera2.Language = language;
            Camera3.Language = language;
            Camera4.Language = language;
            Camera5.Language = language;
            Camera6.Language = language;
            Camera7.Language = language;
            Camera8.Language = language;
        }

        //----------------------------------------------------------------------
        // 功能说明：获取控制器信息
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _GetControllerInfo()
        {
            for (int i = 0; i < system.SystemCameraConfiguration.Length; i++) //遍历相机
            {
                string controllerName = "";
                switch (language)
                {
                    case VisionSystemClassLibrary.Enum.InterfaceLanguage.English:
                        controllerName = system.SystemCameraConfiguration[i].ControllerENGName;
                        break;

                    default:
                        controllerName = system.SystemCameraConfiguration[i].ControllerCHNName;
                        break;
                }

                if (("" != controllerName) && (false == sControllerNames.Contains(controllerName)))//获取控制器名称
                {
                    sControllerNames.Add(controllerName);
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：获取相机信息
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _GetCameraInfo()
        {
            sCameraTypes = new List<VisionSystemClassLibrary.Enum.CameraType>[sControllerNames.Count];
            sCameraNames = new List<string>[sControllerNames.Count];

            for (Byte i = 0; i < sCameraTypes.Length; i++)
            {
                sCameraTypes[i] = new List<VisionSystemClassLibrary.Enum.CameraType>();
                sCameraNames[i] = new List<string>();

                for (Byte j = 0; j < system.SystemCameraConfiguration.Length; j++) //遍历相机
                {
                    if ((sControllerNames[i] == system.SystemCameraConfiguration[j].ControllerCHNName) || (sControllerNames[i] == system.SystemCameraConfiguration[j].ControllerENGName)) //当前控制器
                    {
                        VisionSystemClassLibrary.Enum.CameraType cameraType = VisionSystemClassLibrary.Enum.CameraType.None;

                        cameraType = system.SystemCameraConfiguration[j].Type;

                        if ((VisionSystemClassLibrary.Enum.CameraType.None != cameraType) && (false == sCameraTypes[i].Contains(cameraType)))//获取相机名称
                        {
                            sCameraTypes[i].Add(cameraType);

                            string cameraName ="";
                            switch (language)
                            {
                                case VisionSystemClassLibrary.Enum.InterfaceLanguage.English:
                                    cameraName = system.SystemCameraConfiguration[j].CameraENGName;
                                    break;

                                default:
                                    cameraName = system.SystemCameraConfiguration[j].CameraCHNName;
                                    break;
                            }

                            if (("" != cameraName) && (false == sCameraNames[i].Contains(cameraName)))//获取控制器名称
                            {
                                sCameraNames[i].Add(cameraName);
                            }
                        }
                    }
                }
            }
        }
        
        //----------------------------------------------------------------------
        // 功能说明：获取控制器总页数
        // 输入参数：x,整型，为控制器总个数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _GetTotalPage(Int32 ControllerNumber)
        {
            iPageTotal = (ControllerNumber + ControllerButtons.Count - 1) / ControllerButtons.Count;
        }

        //----------------------------------------------------------------------
        // 功能说明：计算当前页前面所有页中控制器连接的相机总个数
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Int32 _PartialSumCameraNumber(Int32 CurrentPage)
        {
            var CameraIndex = 0;
            for (int k = 0; k < CurrentPage; k++)
            {
                for (int m = 0; m < 2; m++)
                {
                    var index11 = m + k * 2;
                    CameraIndex += sCameraTypes[index11].Count;
                }
            }
            return CameraIndex;
        }
            

        //----------------------------------------------------------------------
        // 功能说明：更新当前页面
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _RefreshCurrentPage()
        {
            Pen linePen = new Pen(Color.Yellow, 2);//定义画笔
            Graphics gfx = this.CreateGraphics();//创建画布
            gfx.DrawLine(linePen, pictureBoxInterface.Location.X + pictureBoxInterface.Size.Width / 2, pictureBoxInterface.Location.Y + pictureBoxInterface.Size.Height, pictureBoxSwitch.Location.X + pictureBoxSwitch.Size.Width / 2, pictureBoxSwitch.Location.Y);//人机界面和交换机的网络连接
            
            for (Byte i = 0; i < ControllerButtons.Count; i++)//遍历0-1个控制器按钮,本页显示的控制器
            {
                var index = i + iCurrentPage * ControllerButtons.Count;

                if (index < sControllerNames.Count) //控制器有效
                {
                    ControllerButtons[i].Chinese_TextDisplay = new String[1] { sControllerNames[index] };//赋值
                    ControllerButtons[i].English_TextDisplay = new String[1] { sControllerNames[index] };//赋值
                    
                    //交换机和控制器i之间的网络连接
                    gfx.DrawLine(linePen, pictureBoxSwitch.Location.X, pictureBoxSwitch.Location.Y + pictureBoxSwitch.Size.Height / 2, ControllerButtons[i].Location.X + ControllerButtons[i].Size.Width / 2, pictureBoxSwitch.Location.Y + pictureBoxSwitch.Size.Height / 2);
                    gfx.DrawLine(linePen, ControllerButtons[i].Location.X + ControllerButtons[i].Size.Width / 2, pictureBoxSwitch.Location.Y + pictureBoxSwitch.Size.Height / 2, ControllerButtons[i].Location.X + ControllerButtons[i].Size.Width / 2, ControllerButtons[i].Location.Y);

                    ControllerButtons[i].Visible = true;

                    for (Byte j = 0; j < sCameraTypes[index].Count; j++)//显示第CurrentPage页相机的连接状态
                    {
                        var cameraConnectIndex = i * 4 + j;

                        CameraButtons[cameraConnectIndex].Chinese_TextDisplay = new String[1] { sCameraNames[index][j] };//赋值
                        CameraButtons[cameraConnectIndex].English_TextDisplay = new String[1] { sCameraNames[index][j] };//赋值

                        gfx.DrawLine(linePen, ControllerButtons[i].Location.X + ControllerButtons[i].Width / 2, ControllerButtons[i].Location.Y + ControllerButtons[i].Height, CameraButtons[cameraConnectIndex].Location.X, CameraButtons[cameraConnectIndex].Location.Y);

                        CameraButtons[cameraConnectIndex].Visible = true;
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：隐藏所有控制器按钮
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _HideControllerButtons()
        {
            for (Byte j = 0; j < ControllerButtons.Count; j++)//显示第CurrentPage页相机的连接状态
            {
                ControllerButtons[j].Visible = false;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：隐藏所有相机按钮
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _HideCameraButtons()
        {
            for (Byte j = 0; j < CameraButtons.Count; j++)//显示第CurrentPage页相机的连接状态
            {
                CameraButtons[j].Visible = false;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：显示第CurrentPage页控制器和相机的连接状态
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _ShowConnectState(Int32 CurrentPage)
        {
            _HideControllerButtons();
            _HideCameraButtons();

            _HideControllerConnect();
            _HideCameraConnect();

            for (Byte i = 0; i < ControllerConnect.Count; i++)//显示第CurrentPage页控制器的连接状态
            {
                var index = i + CurrentPage * ControllerConnect.Count;

                if (index < sControllerNames.Count) //控制器有效
                {
                    if (StateToInterface_Controller[index] == 1)
                    {
                        ControllerConnect[i].Image = global::VisionSystemControlLibrary.Properties.Resources.Connect;
                        ControllerConnect[i].Visible = true;
                    }
                    else if (StateToInterface_Controller[index] == 0)
                    {
                        ControllerConnect[i].Image = global::VisionSystemControlLibrary.Properties.Resources.DisConnect;
                        ControllerConnect[i].Visible = true;
                    }


                    for (Byte j = 0; j < sCameraTypes[index].Count; j++)//显示第CurrentPage页相机的连接状态
                    {
                        var cameraConnectIndex = i * 4 + j; 

                        if (StateToController_Camera[index][j] == 1)
                        {
                            CameraConnect[cameraConnectIndex].Image = VisionSystemControlLibrary.Properties.Resources.Connect;
                            CameraConnect[cameraConnectIndex].Visible = true;
                        }
                        else if (StateToController_Camera[index][j] == 0)
                        {
                            CameraConnect[cameraConnectIndex].Image = VisionSystemControlLibrary.Properties.Resources.DisConnect;
                            CameraConnect[cameraConnectIndex].Visible = true;
                        }
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：隐藏所有控制器状态
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _HideControllerConnect()
        {
            for (Byte j = 0; j < ControllerConnect.Count; j++)//显示第CurrentPage页相机的连接状态
            {
                ControllerConnect[j].Image = null;
                ControllerConnect[j].Visible = true;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：隐藏所有相机状态
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _HideCameraConnect()
        {
            for (Byte j = 0; j < CameraConnect.Count; j++)//显示第CurrentPage页相机的连接状态
            {
                CameraConnect[j].Image = null;
                CameraConnect[j].Visible = true;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：更新控制器连接状态
        // 输入参数：1.ControllerName，当前点击控制器的名称；2.State_Controller，当前点击的控制器和人机界面的连接状态
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _UpdateConnectState_Controller(string controllerName, Boolean State_Controller)
        {
            var i = sControllerNames.IndexOf(controllerName);

            var index = i % ControllerConnect.Count;

            if (State_Controller == true)
            {
                StateToInterface_Controller[i] = 1;

                if ((i / ControllerConnect.Count) == iCurrentPage) //当前页面
                {
                    ControllerConnect[index].Image = VisionSystemControlLibrary.Properties.Resources.Connect;
                    ControllerConnect[index].Visible = true;
                }
            }
            else
            {
                StateToInterface_Controller[i] = 0;

                if ((i / ControllerConnect.Count) == iCurrentPage) //当前页面
                {
                    ControllerConnect[index].Image = VisionSystemControlLibrary.Properties.Resources.DisConnect;
                    ControllerConnect[index].Visible = true;
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：更新相机连接状态
        // 输入参数：1.CameraName，当前点击相机的名称；2.State_Camera，当前点击的相机和控制器的连接状态
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _UpdateConnectState_Camera(VisionSystemClassLibrary.Enum.CameraType cameraType, Boolean State_Camera)
        {
            for (Byte i = 0; i < sCameraTypes.Length; i++) //循环所有控制器
            {
                if (sCameraTypes[i].Contains(cameraType)) //该控制器包含该相机
                {
                    var index = sCameraTypes[i].IndexOf(cameraType);
                    var j = i % ControllerConnect.Count;

                    if (State_Camera == true)
                    {
                        StateToController_Camera[i][index] = 1;

                        if ((i / ControllerConnect.Count) == iCurrentPage) //当前页面
                        {
                            CameraConnect[index + 4 * j].Image = VisionSystemControlLibrary.Properties.Resources.Connect;//索引为i是因为，相机列表Camera和相机连接状态列表CameraConnect的位置是同步的
                            CameraConnect[index + 4 * j].Visible = true;
                        }
                    }
                    else
                    {
                        StateToController_Camera[i][index] = 0;

                        if ((i / ControllerConnect.Count) == iCurrentPage) //当前页面
                        {
                            CameraConnect[index + 4 * j].Image = VisionSystemControlLibrary.Properties.Resources.DisConnect;
                            CameraConnect[index + 4 * j].Visible = true;
                        }
                    }
                }
            }
        }

        //事件

        //----------------------------------------------------------------------
        // 功能说明：控制器向下翻页
        // 输入参数：1.sender：控件自身的引用；2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonNextPage_Click(object sender, EventArgs e)
        {
            if (iCurrentPage != iPageTotal - 1)
            {
                iCurrentPage++;

                _ShowConnectState(iCurrentPage);//显示第iCurrentPage页控制器和相机的连接状态

                this.Invalidate();
            }
            else
            {
                //customButtonNextPage.Enabled = false;
            }
        }
        //----------------------------------------------------------------------
        // 功能说明：控制器向上翻页
        // 输入参数：1.sender：控件自身的引用；2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonPreviousPage_Click(object sender, EventArgs e)
        {
            if (iCurrentPage != 0)
            {
                iCurrentPage--;

                _ShowConnectState(iCurrentPage);//显示第iCurrentPage页控制器和相机的连接状态

                this.Invalidate();
            }
            else
            {
                //customButtonPreviousPage.Enabled = false;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击取消按钮
        // 输入参数：1.sender：控件自身的引用；2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonCancel_Click(object sender, EventArgs e)
        {
            sControllerName = "";
            cameratypeSelected = VisionSystemClassLibrary.Enum.CameraType.None;

            //事件

            if (null != Close_Click)//有效
            {
                Close_Click(this, new CustomEventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：刷新界面
        // 输入参数：1.sender：控件自身的引用；2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void NetDiagnose_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                _RefreshCurrentPage();//刷新当前页面
            }
            catch (System.Exception ex)
            {

            }
        }
                    
        //----------------------------------------------------------------------
        // 功能说明：点击取消按钮
        // 输入参数：1.sender：控件自身的引用；2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButton2_CustomButton_Click(object sender, EventArgs e)
        {
            sControllerName = "";
            cameratypeSelected = VisionSystemClassLibrary.Enum.CameraType.None;

            //事件

            if (null != Close_Click)//有效
            {
                Close_Click(this, new CustomEventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：加载函数
        // 输入参数：1.sender：控件自身的引用；2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void NetDiagnose_Load(object sender, EventArgs e)
        {
            ControllerButtons.Add(Controller1);
            ControllerButtons.Add(Controller2);

            ControllerConnect.Add(labelSC1);
            ControllerConnect.Add(labelSC2);

            CameraButtons.Add(Camera1);
            CameraButtons.Add(Camera2);
            CameraButtons.Add(Camera3);
            CameraButtons.Add(Camera4);
            CameraButtons.Add(Camera5);
            CameraButtons.Add(Camera6);
            CameraButtons.Add(Camera7);
            CameraButtons.Add(Camera8);

            CameraConnect.Add(labelCC1);
            CameraConnect.Add(labelCC2);
            CameraConnect.Add(labelCC3);
            CameraConnect.Add(labelCC4);
            CameraConnect.Add(labelCC5);
            CameraConnect.Add(labelCC6);
            CameraConnect.Add(labelCC7);
            CameraConnect.Add(labelCC8);
        }

        //----------------------------------------------------------------------
        // 功能说明：控制器1单击事件
        // 输入参数：1.sender：控件自身的引用；2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void Controller1_CustomButton_Click(object sender, EventArgs e)
        {
            if (iCurrentPage * ControllerButtons.Count < sControllerNames.Count) //控制器有效
            {
                sControllerName = sControllerNames[iCurrentPage * ControllerButtons.Count];//返回被点击的控制器名称

                if (null != ControllerConnect_Click)//有效
                {
                    ControllerConnect_Click.Invoke(this, new EventArgs());
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：控制器2单击事件
        // 输入参数：1.sender：控件自身的引用；2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void Controller2_CustomButton_Click(object sender, EventArgs e)
        {
            if ((1 + iCurrentPage * ControllerButtons.Count) < sControllerNames.Count) //控制器有效
            {
                sControllerName = sControllerNames[1 + iCurrentPage * ControllerButtons.Count];//返回被点击的控制器名称

                if (null != ControllerConnect_Click)//有效
                {
                    ControllerConnect_Click.Invoke(this, new EventArgs());
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：相机1单击事件
        // 输入参数：1.sender：控件自身的引用；2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void Camera1_CustomButton_Click(object sender, EventArgs e)
        {
            if ((iCurrentPage * ControllerButtons.Count) < sControllerNames.Count) //控制器有效
            {
                if (sCameraTypes[iCurrentPage * ControllerButtons.Count].Count > 0)
                {
                    cameratypeSelected = sCameraTypes[iCurrentPage * ControllerButtons.Count][0];

                    if (null != CameraConnect_Click)//有效
                    {
                        CameraConnect_Click.Invoke(this, new EventArgs());
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：相机2单击事件
        // 输入参数：1.sender：控件自身的引用；2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void Camera2_CustomButton_Click(object sender, EventArgs e)
        {
            if ((iCurrentPage * ControllerButtons.Count) < sControllerNames.Count) //控制器有效
            {
                if (sCameraTypes[iCurrentPage * ControllerButtons.Count].Count > 1)
                {
                    cameratypeSelected = sCameraTypes[iCurrentPage * ControllerButtons.Count][1];

                    if (null != CameraConnect_Click)//有效
                    {
                        CameraConnect_Click.Invoke(this, new EventArgs());
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：相机3单击事件
        // 输入参数：1.sender：控件自身的引用；2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void Camera3_CustomButton_Click(object sender, EventArgs e)
        {
            if ((iCurrentPage * ControllerButtons.Count) < sControllerNames.Count) //控制器有效
            {
                if (sCameraTypes[iCurrentPage * ControllerButtons.Count].Count > 2)
                {
                    cameratypeSelected = sCameraTypes[iCurrentPage * ControllerButtons.Count][2];

                    if (null != CameraConnect_Click)//有效
                    {
                        CameraConnect_Click.Invoke(this, new EventArgs());
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：相机4单击事件
        // 输入参数：1.sender：控件自身的引用；2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void Camera4_CustomButton_Click(object sender, EventArgs e)
        {
            if ((iCurrentPage * ControllerButtons.Count) < sControllerNames.Count) //控制器有效
            {
                if (sCameraTypes[iCurrentPage * ControllerButtons.Count].Count > 3)
                {
                    cameratypeSelected = sCameraTypes[iCurrentPage * ControllerButtons.Count][3];

                    if (null != CameraConnect_Click)//有效
                    {
                        CameraConnect_Click.Invoke(this, new EventArgs());
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：相机5单击事件
        // 输入参数：1.sender：控件自身的引用；2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void Camera5_CustomButton_Click(object sender, EventArgs e)
        {
            if ((1 + iCurrentPage * ControllerButtons.Count) < sControllerNames.Count) //控制器有效
            {
                if (sCameraTypes[1 + iCurrentPage * ControllerButtons.Count].Count > 0)
                {
                    cameratypeSelected = sCameraTypes[1 + iCurrentPage * ControllerButtons.Count][0];

                    if (null != CameraConnect_Click)//有效
                    {
                        CameraConnect_Click.Invoke(this, new EventArgs());
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：相机6单击事件
        // 输入参数：1.sender：控件自身的引用；2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void Camera6_CustomButton_Click(object sender, EventArgs e)
        {
            if ((1 + iCurrentPage * ControllerButtons.Count) < sControllerNames.Count) //控制器有效
            {
                if (sCameraTypes[1 + iCurrentPage * ControllerButtons.Count].Count > 1)
                {
                    cameratypeSelected = sCameraTypes[1 + iCurrentPage * ControllerButtons.Count][1];

                    if (null != CameraConnect_Click)//有效
                    {
                        CameraConnect_Click.Invoke(this, new EventArgs());
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：相机7单击事件
        // 输入参数：1.sender：控件自身的引用；2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void Camera7_CustomButton_Click(object sender, EventArgs e)
        {
            if ((1 + iCurrentPage * ControllerButtons.Count) < sControllerNames.Count) //控制器有效
            {
                if (sCameraTypes[1 + iCurrentPage * ControllerButtons.Count].Count > 2)
                {
                    cameratypeSelected = sCameraTypes[1 + iCurrentPage * ControllerButtons.Count][2];

                    if (null != CameraConnect_Click)//有效
                    {
                        CameraConnect_Click.Invoke(this, new EventArgs());
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：相机8单击事件
        // 输入参数：1.sender：控件自身的引用；2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void Camera8_CustomButton_Click(object sender, EventArgs e)
        {
            if ((1 + iCurrentPage * ControllerButtons.Count) < sControllerNames.Count) //控制器有效
            {
                if (sCameraTypes[1 + iCurrentPage * ControllerButtons.Count].Count > 3)
                {
                    cameratypeSelected = sCameraTypes[1 + iCurrentPage * ControllerButtons.Count][3];

                    if (null != CameraConnect_Click)//有效
                    {
                        CameraConnect_Click.Invoke(this, new EventArgs());
                    }
                }
            }
        }
    }
}
