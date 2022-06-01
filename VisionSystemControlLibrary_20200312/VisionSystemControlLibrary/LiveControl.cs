/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：LiveControl.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：LIVE VIEW窗口

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

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;

using System.Reflection;

using System.IO;

namespace VisionSystemControlLibrary
{
    public partial class LiveControl : UserControl
    {
        //LIVE VIEW页面控件

        private VisionSystemClassLibrary.Enum.InterfaceLanguage language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;//属性，语言

        private VisionSystemClassLibrary.Enum.DeviceState devicestate = VisionSystemClassLibrary.Enum.DeviceState.Stop;//属性，设备状态

        //

        private Boolean bSelfTrigger = false;//【SELF TRIGGER】按钮是否按下。取值范围：true，是；false，否
        private Boolean bZoomImage = true;//【ZOOM IMAGE IN】或【ZOOM IMAGE OUT】按钮标识。取值范围：true，【ZOOM IMAGE OUT】按钮；false，【ZOOM IMAGE IN】按钮

        //

        private Rectangle rectanglePaintBackground = new Rectangle();//绘制区域

        private VisionSystemClassLibrary.Class.Camera camera = new VisionSystemClassLibrary.Class.Camera();//属性（只读），相机

        //

        private Bitmap[] bitmapBackground = null;//背景图像

        //

        private Bitmap bitmapNone = null;//无效图像

        //

        private String[][] sMessageText = new String[2][];//标题控件上显示的文本（[语言][包含的文本]）

        //

        [Browsable(true), Description("点击【返回】按钮时产生的事件"), Category("LiveControl 事件")]
        public event EventHandler Close_Click;//点击【返回】按钮时产生的事件

        [Browsable(true), Description("点击【SELF TRIGGER】按钮时产生的事件"), Category("LiveControl 事件")]
        public event EventHandler SelfTrigger_Click;//点击【SELF TRIGGER】按钮时产生的事件

        //

        //构造函数

        //----------------------------------------------------------------------
        // 功能说明：构造函数，初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public LiveControl()
        {
            InitializeComponent();

            //

            FieldInfo[] fieldinfo = VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese.GetType().GetFields();//获取公共字段
            if (null != fieldinfo && 1 < fieldinfo.Length)//有效
            {
                Int32 i = 0;//循环控制变量

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

            bitmapBackground = new Bitmap[1];
            bitmapBackground[0] = Properties.Resources.Camera_Background;//背景图像

            //

            bitmapNone = new Bitmap(imageDisplayLiveViewZoomIn.Width, imageDisplayLiveViewZoomIn.Height);//无效图像

            //

            _SetRectanglePaintBackground();
        }

        //属性

        //----------------------------------------------------------------------
        // 功能说明：BitmapBackground属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("背景图像"), Category("LiveControl 通用")]
        public Bitmap[] BitmapBackground//属性
        {
            get//读取
            {
                return bitmapBackground;
            }
            set//设置
            {
                bitmapBackground = value;

                //

                _SetRectanglePaintBackground();
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Language属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("语言"), Category("LiveControl 通用")]
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
        [Browsable(true), Description("设备状态"), Category("LiveControl 通用")]
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
        // 功能说明：SelfTrigger属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("【SELF TRIGGER】按钮是否按下。取值范围：true，是；false，否"), Category("LiveControl 通用")]
        public bool SelfTrigger //属性
        {
            get//读取
            {
                return bSelfTrigger;
            }
            set//设置
            {
                if (value != bSelfTrigger)
                {
                    bSelfTrigger = value;

                    //

                    _SetSelfTrigger();//设置按钮背景
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：ZoomImage属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("【ZOOM IMAGE IN】或【ZOOM IMAGE OUT】按钮标识。取值范围：true，【ZOOM IMAGE OUT】按钮；false，【ZOOM IMAGE IN】按钮"), Category("LiveControl 通用")]
        public bool ZoomImage //属性
        {
            get//读取
            {
                return bZoomImage;
            }
            set//设置
            {
                if (value != bZoomImage)
                {
                    bZoomImage = value;

                    //

                    _SetZoomImage();//设置按钮背景，更新页面显示
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：相机类
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(true), Description("相机"), Category("LiveControl 通用")]
        public VisionSystemClassLibrary.Class.Camera SelectedCamera
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

            _SetDeviceState();//设置设备状态

            _SetCamera();//应用属性设置

            _SetLanguage();//设置语言
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：设置图像数据
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _SetImageData()
        {
            _SetView();//设置显示的图像
        }

        //----------------------------------------------------------------------
        // 功能说明：设置绘制区域
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetRectanglePaintBackground()
        {
            if (bitmapBackground[camera.UIParameter.Work_Page].Width <= pictureBoxBackground.Width && bitmapBackground[camera.UIParameter.Work_Page].Height <= pictureBoxBackground.Height)
            {
                rectanglePaintBackground = new Rectangle(0, 0, pictureBoxBackground.Width, pictureBoxBackground.Height);//绘制区域
            }
            else if (bitmapBackground[camera.UIParameter.Work_Page].Width > pictureBoxBackground.Width && bitmapBackground[camera.UIParameter.Work_Page].Height <= pictureBoxBackground.Height)
            {
                rectanglePaintBackground = new Rectangle(0, 0, bitmapBackground[camera.UIParameter.Work_Page].Width, pictureBoxBackground.Height);//绘制区域
            }
            else if (bitmapBackground[camera.UIParameter.Work_Page].Width <= pictureBoxBackground.Width && bitmapBackground[camera.UIParameter.Work_Page].Height > pictureBoxBackground.Height)
            {
                rectanglePaintBackground = new Rectangle(0, 0, pictureBoxBackground.Width, bitmapBackground[camera.UIParameter.Work_Page].Height);//绘制区域
            }
            else
            {
                rectanglePaintBackground = new Rectangle(0, 0, bitmapBackground[camera.UIParameter.Work_Page].Width, bitmapBackground[camera.UIParameter.Work_Page].Height);//绘制区域
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
            customButtonSelfTrigger.Language = language;//【SELF TRIGGER】                            
            customButtonZoomImage.Language = language;//【ZOOM IMAGE OUT】 / 【ZOOM IMAGE IN】

            //

            imageDisplayLiveViewZoomOut.Language = language;//ZOOM OUT
            imageDisplayLiveViewZoomIn.Language = language;//ZOOM IN
            imageDisplayLastLearntImage.Language = language;//LAST LEARNT IMAGE

            //

            customButtonCaption.Language = language;//设置显示的文本

            customButtonLiveViewZoomIn.Language = language;//ZoomIn文本控件
            customButtonLiveViewZoomOut.Language = language;//ZoomOut文本控件
            customButtonLastLearntImage.Language = language;//LastLearntImage文本控件
        }

        //----------------------------------------------------------------------
        // 功能说明：用户调用，设备状态设置完成后，更新页面
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetDeviceState()
        {
            //运行时有效

            if (VisionSystemClassLibrary.Enum.DeviceState.Run == devicestate)//运行
            {
                customButtonSelfTrigger.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//设置【SELF TRIGGER】按钮的背景
            }
            else//停止
            {
                _SetSelfTrigger();
            }

            if (bZoomImage)//【ZOOM IMAGE OUT】按钮
            {
                customButtonZoomImage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//设置【ZOOM IMAGE OUT】/ 【ZOOM IMAGE IN】按钮的背景
            }
            else//【ZOOM IMAGE IN】按钮
            {
                customButtonZoomImage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//设置【ZOOM IMAGE OUT】/ 【ZOOM IMAGE IN】按钮的背景
            }
        }

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
                bSelfTrigger = false;//【SELF TRIGGER】未按下
                customButtonSelfTrigger.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//设置【SELF TRIGGER】按钮的背景

                customButtonSelfTrigger.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//设置【SELF TRIGGER】按钮的背景
                customButtonZoomImage.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//设置【ZOOM IMAGE OUT】/ 【ZOOM IMAGE IN】按钮的背景
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：构造函数，初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetDefault()
        {
            customButtonCaption.Chinese_TextDisplay = new String[1] { camera.CameraCHNName + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] };//设置显示的文本
            customButtonCaption.English_TextDisplay = new String[1] { camera.CameraENGName + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] };//设置显示的文本

            bSelfTrigger = false;//属性，【SELF TRIGGER】按钮是否按下。取值范围：true，是；false，否
            bZoomImage = true;//属性，【ZOOM IMAGE IN】或【ZOOM IMAGE OUT】按钮标识。取值范围：true，【ZOOM IMAGE OUT】按钮；false，【ZOOM IMAGE IN】按钮

            camera.Type = VisionSystemClassLibrary.Enum.CameraType.Camera_1;

            bitmapBackground = new Bitmap[1];
            bitmapBackground[0] = Properties.Resources.Camera_Background;

            //FOCKE FX，1280 * 1024
            //↓↓
            camera.Name = "Top";

            camera.UIParameter.LiveView_BackgroundImage_Location = new Point(0, 20);//

            if (VisionSystemClassLibrary.Enum.CameraType.Camera_1 == camera.Type)//Top
            {
                camera.UIParameter.LiveView_LineStart_Location = new Point(280, 160);//
            }
            else if (VisionSystemClassLibrary.Enum.CameraType.Camera_2 == camera.Type)//Bottom
            {
                camera.UIParameter.LiveView_LineStart_Location = new Point(530, 590);//
            }
            else//其它
            {
                //不执行操作
            }
            camera.UIParameter.LiveView_LineEnd_Location = new Point(786, 160);//
            camera.UIParameter.LiveView_BackgroundImage_Zoom = false;//
            //↑↑
            //FOCKE FX，1280 * 1024

            //COMMON、G.DX6S，1024 * 768
            //↓↓
            camera.Name = "Top";

            camera.UIParameter.LiveView_BackgroundImage_Location = new Point(50, 80);//

            if (VisionSystemClassLibrary.Enum.CameraType.Camera_1 == camera.Type)//Top
            {
                camera.UIParameter.LiveView_LineStart_Location = new Point(315, 130);//
            }
            else if (VisionSystemClassLibrary.Enum.CameraType.Camera_2 == camera.Type)//Bottom
            {
                camera.UIParameter.LiveView_LineStart_Location = new Point(225, 425);//
            }
            else if (VisionSystemClassLibrary.Enum.CameraType.Camera_3 == camera.Type)//Side
            {
                camera.UIParameter.LiveView_LineStart_Location = new Point(225, 325);//
            }
            else//其它
            {
                //不执行操作
            }
            camera.UIParameter.LiveView_LineEnd_Location = new Point(684, 130);//相机指示线的终止坐标
            camera.UIParameter.LiveView_BackgroundImage_Zoom = false;//
            //↑↑
            //COMMON、G.DX6S，1024 * 768

            //FOCKE 700S，1024 * 768
            //↓↓
            camera.Name = "Tobacco1";

            camera.UIParameter.LiveView_BackgroundImage_Location = new Point(150, 0);//

            if (VisionSystemClassLibrary.Enum.CameraType.Camera_1 == camera.Type)//Tobacco1
            {
                camera.UIParameter.LiveView_LineStart_Location = new Point(230, 100);//
            }
            else if (VisionSystemClassLibrary.Enum.CameraType.Camera_2 == camera.Type)//Tobacco2
            {
                camera.UIParameter.LiveView_LineStart_Location = new Point(230, 210);//
            }
            else if (VisionSystemClassLibrary.Enum.CameraType.Camera_3 == camera.Type)//Filter1
            {
                camera.UIParameter.LiveView_LineStart_Location = new Point(400, 310);//
            }
            else if (VisionSystemClassLibrary.Enum.CameraType.Camera_4 == camera.Type)//Filter2
            {
                camera.UIParameter.LiveView_LineStart_Location = new Point(400, 410);//
            }
            else//其它
            {
                //不执行操作
            }
            camera.UIParameter.LiveView_LineEnd_Location = new Point(684, 50);//
            camera.UIParameter.LiveView_BackgroundImage_Zoom = false;//
            //↑↑
            //FOCKE 700S，1024 * 768

            //G.D. X1/X2，1024 * 768
            //↓↓
            camera.Name = "Filter";

            camera.UIParameter.LiveView_BackgroundImage_Location = new Point(0, 0);//

            if (VisionSystemClassLibrary.Enum.CameraType.Camera_1 == camera.Type)//Filter
            {
                camera.UIParameter.LiveView_LineStart_Location = new Point(330, 330);//
            }
            else if (VisionSystemClassLibrary.Enum.CameraType.Camera_2 == camera.Type)//Tobaccl
            {
                camera.UIParameter.LiveView_LineStart_Location = new Point(730, 40);//
            }
            else//其它
            {
                //不执行操作
            }
            camera.UIParameter.LiveView_LineEnd_Location = new Point(860, 100);//
            camera.UIParameter.LiveView_BackgroundImage_Zoom = true;//
            //↑↑
            //G.D. X1/X2，1024 * 768

            //

            _SetSelfTrigger();//设置按钮背景
            _SetZoomImage();//设置按钮背景，更新页面显示

            //

            imageDisplayLastLearntImage.Information = camera.Learn;

            if (null != camera.ImageLearn)//有效
            {
                if (imageDisplayLastLearntImage.ControlSize.Width <= camera.ImageLearn.Width && imageDisplayLastLearntImage.ControlSize.Height <= camera.ImageLearn.Height)//有效
                {
                    imageDisplayLastLearntImage.BitmapDisplay = camera.ImageLearn.ToBitmap();//LAST LEARNT IMAGE
                }
            }
            else//无效
            {
                imageDisplayLastLearntImage.BitmapDisplay = (Bitmap)bitmapNone.Clone();//图像数据
            }

            //

            //_SetBackground();//设置背景图像
        }

        //----------------------------------------------------------------------
        // 功能说明：设置属性SelectedCamera后调用，应用设置的属性
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetCamera()
        {
            customButtonCaption.Chinese_TextDisplay = new String[1] { camera.CameraCHNName + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] };//设置显示的文本
            customButtonCaption.English_TextDisplay = new String[1] { camera.CameraENGName + sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] };//设置显示的文本

            _SetSelfTrigger();//设置按钮背景
            _SetZoomImage();//设置按钮背景，更新页面显示

            //

            imageDisplayLastLearntImage.Information = camera.Learn;

            if (null != camera.ImageLearn)//有效
            {
                if (imageDisplayLastLearntImage.ControlSize.Width <= camera.ImageLearn.Width && imageDisplayLastLearntImage.ControlSize.Height <= camera.ImageLearn.Height)//有效
                {
                    imageDisplayLastLearntImage.BitmapDisplay = camera.ImageLearn.ToBitmap();//LAST LEARNT IMAGE
                }
            }
            else//无效
            {
                imageDisplayLastLearntImage.BitmapDisplay = (Bitmap)bitmapNone.Clone();//图像数据
            }

            //

            //_SetBackground();//设置背景图像
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【SELF TRIGGER】按钮后，设置按钮背景
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetSelfTrigger()
        {
            if (bSelfTrigger)//【SELF TRIGGER】按下
            {
                customButtonSelfTrigger.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;//设置【SELF TRIGGER】按钮的背景
            }
            else//【SELF TRIGGER】未按下
            {
                customButtonSelfTrigger.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//设置【SELF TRIGGER】按钮的背景
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【ZOOM IMAGE IN】或【ZOOM IMAGE OUT】按钮后，设置按钮背景，更新页面显示
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetZoomImage()
        {
            if (bZoomImage)//【ZOOM IMAGE OUT】按钮
            {
                customButtonZoomImage.CurrentTextGroupIndex = 1;

                //

                customButtonLiveViewZoomIn.Visible = true;//"ZOOMED LIVE VIEW"文本控件
                imageDisplayLiveViewZoomIn.Visible = true;//"ZOOMED LIVE VIEW"图像控件
                customButtonZoomInBackground.Visible = true;//"ZOOMED LIVE VIEW"背景控件

                //

                customButtonLiveViewZoomOut.Visible = false;//"LIVE VIEW"文本控件
                customButtonZoomOutBackground.Visible = false;//"LIVE VIEW"背景控件
                imageDisplayLiveViewZoomOut.Visible = false;//"LIVE VIEW"图像控件

                pictureBoxBackground.Visible = false;//隐藏背景图像
            }
            else//【ZOOM IMAGE IN】按钮
            {
                customButtonZoomImage.CurrentTextGroupIndex = 0;

                //

                customButtonLiveViewZoomOut.Visible = true;//"LIVE VIEW"文本控件
                customButtonZoomOutBackground.Visible = true;//"LIVE VIEW"背景控件
                imageDisplayLiveViewZoomOut.Visible = true;//"LIVE VIEW"图像控件

                pictureBoxBackground.Visible = true;//显示背景图像

                //

                customButtonLiveViewZoomIn.Visible = false;//"ZOOMED LIVE VIEW"文本控件
                imageDisplayLiveViewZoomIn.Visible = false;//"ZOOMED LIVE VIEW"图像控件
                customButtonZoomInBackground.Visible = false;//"ZOOMED LIVE VIEW"背景控件
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：设置LIVE VIEW显示的图像
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetView()
        {
            if (bZoomImage)//【ZOOM IMAGE OUT】按钮
            {
                imageDisplayLiveViewZoomIn.Information = camera.Live.GraphicsInformation;

                if (null != camera.ImageLive)//有效
                {
                    if (imageDisplayLiveViewZoomIn.ControlSize.Width <= camera.ImageLive.Width && imageDisplayLiveViewZoomIn.ControlSize.Height <= camera.ImageLive.Height)//有效
                    {
                        if (!(imageDisplayLiveViewZoomIn.ShowTitle))//隐藏
                        {
                            imageDisplayLiveViewZoomIn.ShowTitle = true;//显示
                        }

                        //

                        imageDisplayLiveViewZoomIn.BitmapDisplay = camera.ImageLive.ToBitmap();//"ZOOMED LIVE VIEW"图像控件
                    }
                }
                else//无效
                {
                    if (imageDisplayLiveViewZoomIn.ShowTitle)//显示
                    {
                        imageDisplayLiveViewZoomIn.ShowTitle = false;//隐藏
                    }

                    //

                    imageDisplayLiveViewZoomIn.BitmapDisplay = (Bitmap)bitmapNone.Clone();//图像数据
                }
            }
            else//【ZOOM IMAGE IN】按钮
            {
                imageDisplayLiveViewZoomOut.Information = camera.Live.GraphicsInformation;

                if (null != camera.ImageLive)//有效
                {
                    if (imageDisplayLiveViewZoomOut.ControlSize.Width <= camera.ImageLive.Width && imageDisplayLiveViewZoomOut.ControlSize.Height <= camera.ImageLive.Height)//有效
                    {
                        if (!(imageDisplayLiveViewZoomOut.ShowTitle))//隐藏
                        {
                            imageDisplayLiveViewZoomOut.ShowTitle = true;//显示
                        }

                        //

                        imageDisplayLiveViewZoomOut.BitmapDisplay = camera.ImageLive.ToBitmap();//"ZOOMED LIVE VIEW"图像控件
                    }
                }
                else//无效
                {
                    if (imageDisplayLiveViewZoomOut.ShowTitle)//显示
                    {
                        imageDisplayLiveViewZoomOut.ShowTitle = false;//隐藏
                    }

                    //

                    imageDisplayLiveViewZoomOut.BitmapDisplay = (Bitmap)bitmapNone.Clone();//图像数据
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：更新背景图像
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetBackground()
        {
            Image imageDisplay = new Bitmap(rectanglePaintBackground.Width, rectanglePaintBackground.Height);
            Graphics graphicsDraw = Graphics.FromImage(imageDisplay);

            SolidBrush solidbrushDraw = new SolidBrush(BackColor);//画刷
            Pen penDraw = new Pen(Color.FromArgb(255, 255, 0), 2F);//画笔

            //绘制背景

            graphicsDraw.FillRectangle(solidbrushDraw, rectanglePaintBackground);//绘制背景

            //绘制相机图示

            graphicsDraw.DrawImage(bitmapBackground[camera.UIParameter.Work_Page], new Rectangle(camera.UIParameter.LiveView_BackgroundImage_Location.X, camera.UIParameter.LiveView_BackgroundImage_Location.Y, bitmapBackground[camera.UIParameter.Work_Page].Width, bitmapBackground[camera.UIParameter.Work_Page].Height));//绘制

            graphicsDraw.DrawLine(penDraw, camera.UIParameter.LiveView_LineStart_Location, camera.UIParameter.LiveView_LineEnd_Location);

            //

            if (camera.UIParameter.LiveView_BackgroundImage_Zoom)//
            {
                pictureBoxBackground.SizeMode = PictureBoxSizeMode.Zoom;
            }
            else//
            {
                pictureBoxBackground.SizeMode = PictureBoxSizeMode.Normal;
            }

            pictureBoxBackground.Image = (Image)imageDisplay.Clone();
        }

        //事件

        //----------------------------------------------------------------------
        // 功能说明：控件加载事件
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void LiveControl_Load(object sender, EventArgs e)
        {
            //_SetDefault();//设置默认值
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【SELF TRIGGER】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonSelfTrigger_CustomButton_Click(object sender, EventArgs e)
        {
            bSelfTrigger = !bSelfTrigger;

            //事件

            if (null != SelfTrigger_Click)//有效
            {
                SelfTrigger_Click(this, new EventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【ZOOM IMAGE IN】或【ZOOM IMAGE OUT】按钮事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonZoomImage_CustomButton_Click(object sender, EventArgs e)
        {
            bZoomImage = !bZoomImage;

            _SetZoomImage();
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
                Close_Click(this, new EventArgs());
            }
        }
    }
}