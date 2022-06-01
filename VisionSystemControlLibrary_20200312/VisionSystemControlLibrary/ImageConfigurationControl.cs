/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：ImageConfigurationControl.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：图像配置控件

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

using System.IO;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;

using System.Runtime.InteropServices;

using System.Reflection;

namespace VisionSystemControlLibrary
{
    public partial class ImageConfigurationControl : UserControl
    {
        private VisionSystemClassLibrary.Enum.InterfaceLanguage language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;//属性，语言

        private VisionSystemClassLibrary.Enum.DeviceState devicestate = VisionSystemClassLibrary.Enum.DeviceState.Stop;//属性，设备状态

        //

        private Boolean bSaveProduct = false;//【Save Product】按钮的状态。取值范围：true,产品数据被修改；false：产品数据未被修改

        //

        private Boolean bClickCloseButton = false;//是否点击【CLOSE】按钮。取值范围：true，是；false，否

        //

        private Boolean bFocusCalibrationSelected = false;//【FOCUS CALIBRATION】按钮是否按下。取值范围：true，是；false，否
        private Boolean bWhiteBalanceSelected = false;//【WHITE BALANCE】按钮是否按下。取值范围：true，是；false，否

        //

        private UInt16 uiSensorAdjustState = 0; //传感器校准状态，默认未校准
        private Int32 iSensorAdjusting = 0;//下位机校准标记：1、校准中；0，取消校准、未校准或者校准结束

        //

        private Bitmap bitmapNone = null;//无效图像

        //

        VisionSystemClassLibrary.Class.Camera camera = new VisionSystemClassLibrary.Class.Camera();   //创建Camera类库的对象
        VisionSystemClassLibrary.Class.Camera cameraTemp = new VisionSystemClassLibrary.Class.Camera();//创建Camera类库的对象

        //

        private VisionSystemClassLibrary.Class.Brand brand = new VisionSystemClassLibrary.Class.Brand();//属性（只读），品牌

        //

        private String[][] sMessageText = new String[2][];//提示信息窗口上显示的文本（[语言][包含的文本]）
        private String[][] sMessageText_1 = new String[2][];//标题控件上显示的文本（[语言][包含的文本]）

        //

        [Browsable(true), Description("点击【返回】按钮时产生的事件"), Category("ImageConfigurationControl 事件")]
        public event EventHandler Close_Click;

        [Browsable(true), Description("点击【Save Product】按钮时产生的事件"), Category("ImageConfigurationControl 事件")]
        public event EventHandler SaveProduct_Click;

        [Browsable(true), Description("点击【FOCUS CALIBRATION】按钮时产生的事件"), Category("ImageConfigurationControl 事件")]
        public event EventHandler FocusCalibration_Click;
        
        [Browsable(true), Description("点击【WHITE BALANCE】按钮时产生的事件"), Category("ImageConfigurationControl 事件")]
        public event EventHandler WhiteBalance_Click;

        [Browsable(true), Description("参数值发生改变的事件"), Category("ImageConfigurationControl 事件")]
        public event EventHandler ParameterValueChanged;

        //构造函数

        //----------------------------------------------------------------------
        // 功能说明：构造函数，初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public ImageConfigurationControl()
        {                            
            InitializeComponent();

            //由于该控件仅有一个实例，因此可以在此处进行事件的订阅

            if (null != GlobalWindows.MessageDisplay_Window)
            {
                GlobalWindows.MessageDisplay_Window.WindowClose_ImageConfiguration_SaveProduct_Confirm += new System.EventHandler(messageDisplayWindow_WindowClose_ImageConfiguration_SaveProduct_Confirm);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_ImageConfiguration_SaveProduct_Success += new System.EventHandler(messageDisplayWindow_WindowClose_ImageConfiguration_SaveProduct_Success);//订阅事件
                GlobalWindows.MessageDisplay_Window.WindowClose_ImageConfiguration_SaveProduct_Failure += new System.EventHandler(messageDisplayWindow_WindowClose_ImageConfiguration_SaveProduct_Failure);//订阅事件
            }

            if (null != GlobalWindows.SensorSelect_Window)
            {
                GlobalWindows.SensorSelect_Window.WindowClose+=new EventHandler(SensorSelect_Window_WindowClose);//订阅事件
            }

            //

            bitmapNone = new Bitmap(imageDisplayView.Width, imageDisplayView.Height);//无效图像

            //

            Int32 i = 0;//循环控制变量

            FieldInfo[] fieldinfo = VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese.GetType().GetFields();//获取公共字段
            if (null != fieldinfo && 1 < fieldinfo.Length)//有效
            {
                sMessageText = new String[fieldinfo.Length - 1][];
                sMessageText_1 = new String[fieldinfo.Length - 1][];
                for (i = 0; i < fieldinfo.Length - 1; i++)
                {
                    sMessageText[i] = new String[4];
                    sMessageText_1[i] = new String[1];
                }

                //

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] = "确定保存数据";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] = "Save Product";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][1] = "保存数据成功";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][1] = "Save Product Completed";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][2] = "保存数据失败";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][2] = "Save Product Failed";

                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][3] = "请低速盘车";
                sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][3] = "Start at low speed";

                //

                sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] = customButtonCaption.Chinese_TextDisplay[0];
                sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] = customButtonCaption.English_TextDisplay[0];
            }
        }

        //属性

        //-----------------------------------------------------------------------
        // 功能说明：相机类
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        [Browsable(false), Description("相机"), Category("ImageConfigurationControl 通用")]
        public VisionSystemClassLibrary.Class.Camera SelectedCamera
        {
            get
            {
                return camera;
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：SystemBrand属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("品牌"), Category("ImageConfigurationControl 通用")]
        public VisionSystemClassLibrary.Class.Brand SystemBrand//属性
        {
            get//读取
            {
                return brand;
            }
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：Language属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("语言"), Category("ImageConfigurationControl 通用")]
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
        [Browsable(true), Description("设备状态"), Category("ImageConfigurationControl 通用")]
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

        //-----------------------------------------------------------------------
        // 功能说明：Language属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        [Browsable(true), Description("传感器校准状态"), Category("ImageConfigurationControl 通用")]
        public UInt16 SensorAdjustState
        {
            get//读取
            {
                return uiSensorAdjustState;
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

            //

            _SetCamera();//载入

            _SetLanguage();//设置语言
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：载入
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _SetCamera()
        {
            camera._CopyTo(ref cameraTemp);

            //

            customButtonCaption.Chinese_TextDisplay = new String[1] { camera.CameraCHNName + sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][0] };//设置显示的文本
            customButtonCaption.English_TextDisplay = new String[1] { camera.CameraENGName + sMessageText_1[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][0] };//设置显示的文本

            //

            customButtonSaveProduct.BitmapWhole = VisionSystemControlLibrary.Properties.Resources.Button;//【SAVE PRODUCT】
            customButtonSaveProduct.BitmapWhole = VisionSystemControlLibrary.Properties.Resources.Button;//【SAVE PRODUCT】

            parameterSettingsPanel.ControlEnabled = true;

            bFocusCalibrationSelected = false;
            customButtonFocusCalibration.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【FOCUS CALIBRATION】

            bWhiteBalanceSelected = false;
            customButtonWhiteBalance.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【WHITE BALANCE】

            if (true == camera.IsSerialPort) //当前为相机
            {
                uiSensorAdjustState = 0;
                customButtonAdjust.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【Adjust】

                customButtonAdjust.Chinese_TextDisplay = new string[] { "校准" };
                customButtonAdjust.English_TextDisplay = new string[] { "Adjust" };

                customButtonAdjust.Visible = true;
            }
            else
            {
                customButtonAdjust.Visible = false;
            }

            //

            _SetParameters();//设置参数
        }

        //-----------------------------------------------------------------------
        // 功能说明：设置语言
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _SetLanguage()
        {
            customButtonCaption.Language = language;//设置显示的文本

            customButtonSaveProduct.Language = language;//【SAVE PRODUCT】
            customButtonFocusCalibration.Language = language;//【FOCUS CALIBRATION】
            customButtonWhiteBalance.Language = language;//【WHITE BALANCE】
            customButtonAdjust.Language = language;
            customButtonMessage.Language = language;

            //

            parameterSettingsPanel.Language = language;//参数设置面板
        }

        //-----------------------------------------------------------------------
        // 功能说明：系统状态
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _SetDeviceState()
        {
            if (VisionSystemClassLibrary.Enum.DeviceState.Run == devicestate)
            {
                customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【SAVE PRODUCT】
                customButtonFocusCalibration.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【FOCUS CALIBRATION】
                customButtonWhiteBalance.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【WHITE BALANCE】
                customButtonAdjust.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【Adjust】

                //

                parameterSettingsPanel.ControlEnabled = false;
            }
            else if (VisionSystemClassLibrary.Enum.DeviceState.Stop == devicestate)
            {
                if (bSaveProduct)//参数被修改
                {
                    customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【SAVE PRODUCT】
                } 
                else//参数未被修改
                {
                    customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【SAVE PRODUCT】
                }

                if (bFocusCalibrationSelected)//【FOCUS CALIBRATION】按下
                {
                    customButtonFocusCalibration.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
                }
                else//【FOCUS CALIBRATION】未按下
                {
                    customButtonFocusCalibration.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                }

                if (bWhiteBalanceSelected)//【WHITE BALANCE】按下
                {
                    customButtonWhiteBalance.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
                }
                else//【WHITE BALANCE】未按下
                {
                    customButtonWhiteBalance.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                }

                if (1 == uiSensorAdjustState)//【Adjust】按下
                {
                    customButtonAdjust.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Selected;
                }
                else//【Adjust】未按下
                {
                    customButtonAdjust.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
                }

                //

                parameterSettingsPanel.ControlEnabled = true;
            }
            else//其它
            {
                //不执行操作
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：设置工具参数
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _SetParameters()
        {
            try
            {
                Int32 i = 0;//循环控制变量
                Int32 j = 0;//循环控制变量

                if (false == camera.IsSerialPort) //当前为相机
                {
                    parameterSettingsPanel.ParameterType = new Int32[] { 2, 2, 2, 2, 1, 2, 2, 2 };//参数类型

                    parameterSettingsPanel._SetParameterValue(new Int32[VisionSystemClassLibrary.String.TextData.ImageParameter_ENG.Length][]);//参数数值（参数类型取值为1时有效）
                    parameterSettingsPanel._SetParameterValueEnabled(new Boolean[VisionSystemClassLibrary.String.TextData.ImageParameter_ENG.Length][]);//参数数值使能情况。取值范围：true，使能；false，禁止（参数类型取值为1时有效）

                    parameterSettingsPanel.Chinese_ParameterName = new String[]
                { VisionSystemClassLibrary.String.TextData.ImageParameter_CHN[0], 
                  VisionSystemClassLibrary.String.TextData.ImageParameter_CHN[1], 
                  VisionSystemClassLibrary.String.TextData.ImageParameter_CHN[2], 
                  VisionSystemClassLibrary.String.TextData.ImageParameter_CHN[3], 
                  VisionSystemClassLibrary.String.TextData.ImageParameter_CHN[4], 
                  VisionSystemClassLibrary.String.TextData.ImageParameter_CHN[5], 
                  VisionSystemClassLibrary.String.TextData.ImageParameter_CHN[6], 
                  VisionSystemClassLibrary.String.TextData.ImageParameter_CHN[7]
                };//参数中文名称

                    parameterSettingsPanel.English_ParameterName = new String[]
                { VisionSystemClassLibrary.String.TextData.ImageParameter_ENG[0], 
                  VisionSystemClassLibrary.String.TextData.ImageParameter_ENG[1], 
                  VisionSystemClassLibrary.String.TextData.ImageParameter_ENG[2], 
                  VisionSystemClassLibrary.String.TextData.ImageParameter_ENG[3],
                  VisionSystemClassLibrary.String.TextData.ImageParameter_ENG[4], 
                  VisionSystemClassLibrary.String.TextData.ImageParameter_ENG[5], 
                  VisionSystemClassLibrary.String.TextData.ImageParameter_ENG[6], 
                  VisionSystemClassLibrary.String.TextData.ImageParameter_ENG[7]
                };//参数英文名称

                    parameterSettingsPanel.ParameterCurrentValue = new Single[]
                { camera.DeviceParameter.StroboTime, 
                  camera.DeviceParameter.StroboCurrent, 
                  camera.DeviceParameter.Gain, 
                  camera.DeviceParameter.ExposureTime,
                  camera.DeviceParameter.WhiteBalance,
                  camera.DeviceParameter.WhiteBalance_Red,
                  camera.DeviceParameter.WhiteBalance_Green,
                  camera.DeviceParameter.WhiteBalance_Blue         
                };//当前值

                    parameterSettingsPanel.ParameterMinValue = new Single[]
                { camera.DeviceParameter.StroboTime_Min, 
                  camera.DeviceParameter.StroboCurrent_Min, 
                  camera.DeviceParameter.Gain_Min, 
                  camera.DeviceParameter.ExposureTime_Min, 
                  0, 
                  camera.DeviceParameter.WhiteBalance_Red_Min, 
                  camera.DeviceParameter.WhiteBalance_Green_Min, 
                  camera.DeviceParameter.WhiteBalance_Blue_Min,     
                };//最小值（参数类型取值为2时有效）

                    parameterSettingsPanel.ParameterMaxValue = new Single[]
                { camera.DeviceParameter.StroboTime_Max, 
                  camera.DeviceParameter.StroboCurrent_Max, 
                  camera.DeviceParameter.Gain_Max, 
                  camera.DeviceParameter.ExposureTime_Max,  
                  VisionSystemClassLibrary.String.TextData.ImageParameter_WhiteBalance_ENG.Length - 1,  
                  camera.DeviceParameter.WhiteBalance_Red_Max,  
                  camera.DeviceParameter.WhiteBalance_Green_Max,  
                  camera.DeviceParameter.WhiteBalance_Blue_Max,  
                };//最大值（参数类型取值为2时有效）

                    parameterSettingsPanel.Chinese_ParameterValueNameDisplay = new String[VisionSystemClassLibrary.String.TextData.ImageParameter_ENG.Length];//原始参数数值中文名称（参数类型取值为1时有效）
                    parameterSettingsPanel.English_ParameterValueNameDisplay = new String[VisionSystemClassLibrary.String.TextData.ImageParameter_ENG.Length];//原始参数数值英文名称（参数类型取值为1时有效）

                    for (i = 0; i < VisionSystemClassLibrary.String.TextData.ImageParameter_ENG.Length - 1; i++)
                    {
                        if (4 == i)//枚举型参数
                        {
                            parameterSettingsPanel.ParameterValue[i] = new Int32[VisionSystemClassLibrary.String.TextData.ImageParameter_WhiteBalance_ENG.Length];//参数数值（参数类型取值为1时有效）
                            parameterSettingsPanel.ParameterValueEnabled[i] = new Boolean[VisionSystemClassLibrary.String.TextData.ImageParameter_WhiteBalance_ENG.Length];//参数数值使能情况。取值范围：true，使能；false，禁止（参数类型取值为1时有效）

                            parameterSettingsPanel.Chinese_ParameterValueNameDisplay[i] = "";
                            parameterSettingsPanel.English_ParameterValueNameDisplay[i] = "";
                            for (j = 0; j < VisionSystemClassLibrary.String.TextData.ImageParameter_WhiteBalance_ENG.Length - 1; j++)
                            {
                                parameterSettingsPanel.ParameterValue[i][j] = j;//参数数值（参数类型取值为1时有效）
                                parameterSettingsPanel.ParameterValueEnabled[i][j] = true;//参数数值使能情况。取值范围：true，使能；false，禁止（参数类型取值为1时有效）

                                parameterSettingsPanel.Chinese_ParameterValueNameDisplay[i] += VisionSystemClassLibrary.String.TextData.ImageParameter_WhiteBalance_CHN[j] + "&";
                                parameterSettingsPanel.English_ParameterValueNameDisplay[i] += VisionSystemClassLibrary.String.TextData.ImageParameter_WhiteBalance_ENG[j] + "&";
                            }

                            parameterSettingsPanel.ParameterValue[i][j] = j;//参数数值（参数类型取值为1时有效）
                            parameterSettingsPanel.ParameterValueEnabled[i][j] = true;//参数数值使能情况。取值范围：true，使能；false，禁止（参数类型取值为1时有效）

                            parameterSettingsPanel.Chinese_ParameterValueNameDisplay[i] += VisionSystemClassLibrary.String.TextData.ImageParameter_WhiteBalance_CHN[j];
                            parameterSettingsPanel.English_ParameterValueNameDisplay[i] += VisionSystemClassLibrary.String.TextData.ImageParameter_WhiteBalance_ENG[j];
                        }
                        else//数字型参数
                        {
                            parameterSettingsPanel.Chinese_ParameterValueNameDisplay[i] = " ";
                            parameterSettingsPanel.English_ParameterValueNameDisplay[i] = " ";
                        }
                    }

                    if (0 == camera.DeviceParameter.WhiteBalance)//自动
                    {
                        parameterSettingsPanel.ParameterEnabled = new Boolean[] { false, false, false, false, true, false, false, false };//参数使能状态
                    }
                    else//手动
                    {
                        parameterSettingsPanel.ParameterEnabled = new Boolean[] { true, true, true, true, true, true, true, true };//参数使能状态
                    }

                    parameterSettingsPanel._Apply(true);
                }
                else //当前为串口
                {
                    Int32 parameterSettingsPanel_Length = camera.SensorNumber;

                    Int32[] ParameterType = new Int32[parameterSettingsPanel_Length];
                    Int32[][] ParameterType_SetParameterValue = new Int32[parameterSettingsPanel_Length][];
                    Boolean[][] ParameterType_SetParameterValueEnabled = new Boolean[parameterSettingsPanel_Length][];

                    string[] Chinese_ParameterName = new string[parameterSettingsPanel_Length];
                    string[] English_ParameterName = new string[parameterSettingsPanel_Length];

                    Single[] ParameterCurrentValue = new Single[parameterSettingsPanel_Length];

                    Single[] ParameterMinValue = new Single[parameterSettingsPanel_Length];
                    Single[] ParameterMaxValue = new Single[parameterSettingsPanel_Length];

                    for (i = 0; i < parameterSettingsPanel_Length; i++) //循环所有烟支
                    {
                        ParameterType[i] = 2;

                        Chinese_ParameterName[i] = VisionSystemClassLibrary.String.TextData.SerialPortParameter_CHN[i];
                        English_ParameterName[i] = VisionSystemClassLibrary.String.TextData.SerialPortParameter_ENG[i];
                        
                        ParameterCurrentValue[i] = camera.DeviceParameter.SensorAdjustValue[i];

                        ParameterMinValue[i] = 0;
                        ParameterMaxValue[i] = 255;
                    }

                    //

                    parameterSettingsPanel.ParameterType = ParameterType;//参数类型

                    parameterSettingsPanel._SetParameterValue(ParameterType_SetParameterValue);//参数数值（参数类型取值为1时有效）
                    parameterSettingsPanel._SetParameterValueEnabled(ParameterType_SetParameterValueEnabled);//参数数值使能情况。取值范围：true，使能；false，禁止（参数类型取值为1时有效）

                    parameterSettingsPanel.Chinese_ParameterName = Chinese_ParameterName;//参数中文名称
                    parameterSettingsPanel.English_ParameterName = English_ParameterName;//参数英文名称

                    parameterSettingsPanel.ParameterCurrentValue = ParameterCurrentValue;//当前值

                    parameterSettingsPanel.ParameterMinValue = ParameterMinValue;//最小值（参数类型取值为2时有效）

                    parameterSettingsPanel.ParameterMaxValue = ParameterMaxValue;//最大值（参数类型取值为2时有效）

                    parameterSettingsPanel.Chinese_ParameterValueNameDisplay = new String[parameterSettingsPanel_Length];//原始参数数值中文名称（参数类型取值为1时有效）
                    parameterSettingsPanel.English_ParameterValueNameDisplay = new String[parameterSettingsPanel_Length];//原始参数数值英文名称（参数类型取值为1时有效）

                    for (i = 0; i < parameterSettingsPanel_Length; i++)
                    {
                        parameterSettingsPanel.Chinese_ParameterValueNameDisplay[i] = " ";
                        parameterSettingsPanel.English_ParameterValueNameDisplay[i] = " ";
                    }

                    Boolean[] ParameterEnabled = new Boolean[parameterSettingsPanel_Length];

                    for (i = 0; i < parameterSettingsPanel_Length; i++) //循环所有烟支
                    {
                        ParameterEnabled[i] = true;
                    }
                    parameterSettingsPanel.ParameterEnabled = ParameterEnabled;//参数使能状态

                    parameterSettingsPanel._Apply(true);
                }
            }
            catch (System.Exception ex)
            {
                //不执行操作
            }
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：点击【CLOSE】按钮的操作
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        private void _Close()
        {
            cameraTemp._CopyTo(ref camera);

            bClickCloseButton = false;

            bSaveProduct = false;

            customButtonSaveProduct.BitmapWhole = VisionSystemControlLibrary.Properties.Resources.Button;//【SAVE PRODUCT】
            customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【SAVE PRODUCT】

            parameterSettingsPanel.ControlEnabled = true;

            bFocusCalibrationSelected = false;
            customButtonFocusCalibration.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【FOCUS CALIBRATION】

            bWhiteBalanceSelected = false;
            customButtonWhiteBalance.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【WHITE BALANCE】

            uiSensorAdjustState = 0;
            customButtonAdjust.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【Adjust】

            customButtonAdjust.Chinese_TextDisplay = new string[] { "校准" };
            customButtonAdjust.English_TextDisplay = new string[] { "Adjust" };
            customButtonAdjust.Visible = false;

            iSensorAdjusting = 0;

            customButtonMessage.Chinese_TextDisplay = new string[] { "请低速盘车..." };
            customButtonMessage.English_TextDisplay = new string[] { "Please Start at Low Speed..." };
            customButtonMessage.Visible = false;

            //事件

            if (null != Close_Click)//有效
            {
                Close_Click(this, new CustomEventArgs());
            }
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：刷新图像
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        public void _UpdateImage()
        {
            if (null != camera.ImageLive)//有效
            {
                if (camera.Live.GraphicsInformation.Scale >= 1.0)
                {
                    imageDisplayView.BitmapDisplay = camera.ImageLive.ToBitmap();
                }
            }
            else//无效
            {
                imageDisplayView.BitmapDisplay = (Bitmap)bitmapNone.Clone();
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：刷新参数
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        public void _UpdateParameter(UInt16 WhiteBalance_Red, UInt16 WhiteBalance_Green, UInt16 WhiteBalance_Blue)
        {
            if (
                WhiteBalance_Red != camera.DeviceParameter.WhiteBalance_Red ||
                WhiteBalance_Green != camera.DeviceParameter.WhiteBalance_Green ||
                WhiteBalance_Blue != camera.DeviceParameter.WhiteBalance_Blue
                )
            {
                camera.DeviceParameter.WhiteBalance_Red = WhiteBalance_Red;
                camera.DeviceParameter.WhiteBalance_Green = WhiteBalance_Green;
                camera.DeviceParameter.WhiteBalance_Blue = WhiteBalance_Blue;

                //

                parameterSettingsPanel.ParameterCurrentValue = new Single[]
                { camera.DeviceParameter.StroboTime, 
                  camera.DeviceParameter.StroboCurrent, 
                  camera.DeviceParameter.Gain, 
                  camera.DeviceParameter.ExposureTime,
                  camera.DeviceParameter.WhiteBalance,
                  camera.DeviceParameter.WhiteBalance_Red,
                  camera.DeviceParameter.WhiteBalance_Green,
                  camera.DeviceParameter.WhiteBalance_Blue         
                };//当前值

                parameterSettingsPanel._ClearSelectState();
                parameterSettingsPanel._Apply(false);
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：刷新参数
        // 输入参数： 1、iSensorAdjusting，传感器最大值查找过程标记（1，校准过程中；0，校准取消或未校准、取消校准）
        //            2、bSensorDACMax，传感器电压最大值
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        public void _UpdateADCMax(Int32 sensorADCMaxChecking, Int16[] bSensorDACMax)
        {
            if ((1 == uiSensorAdjustState) && (camera.DeviceParameter.SensorADCValueMax != bSensorDACMax)) //传感器校准值发生变化
            {
                Int32 i = 0; //循环变量

                for (i = 0; i < camera.DeviceParameter.SensorAdjustValue.Length; i++) //循环所有传感器
                {
                    camera.DeviceParameter.SensorADCValueMax[i] = bSensorDACMax[i];
                }

                //

                bSaveProduct = true;

                customButtonSaveProduct.BitmapWhole = VisionSystemControlLibrary.Properties.Resources.Button_1;//【SAVE PRODUCT】

                customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【SAVE PRODUCT】
            }

            if ((1 == uiSensorAdjustState) && (sensorADCMaxChecking == 0)) //传感器ADC最大值查询结束
            {
                customButtonMessage.Chinese_TextDisplay = new string[] { "电位器校准完成" };
                customButtonMessage.English_TextDisplay = new string[] { "Potentiometer Adjust Finished" };
                customButtonMessage.Visible = true;

                customButtonAdjust.Chinese_TextDisplay = new string[] { "校准完成" };
                customButtonAdjust.English_TextDisplay = new string[] { "Adjust Finished" };
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：刷新参数
        // 输入参数： 1、iSensorAdjusting，传感器校准过程标记（1，校准过程中；0，校准取消或未校准、取消校准）
        //            2、bSensorAdjustValue，传感器校准值
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        public void _UpdateSensorAdjustValue(Int32 sensorAdjusting, Byte[] bSensorAdjustValue)
        {
            iSensorAdjusting = sensorAdjusting;

            if ((1 == uiSensorAdjustState) && (camera.DeviceParameter.SensorAdjustValue != bSensorAdjustValue)) //传感器校准值发生变化
            {
                Int32 i = 0; //循环变量

                for (i = 0; i < camera.DeviceParameter.SensorAdjustValue.Length; i++) //循环所有传感器
                {
                    camera.DeviceParameter.SensorAdjustValue[i] = bSensorAdjustValue[i];
                }

                //

                Int32 parameterSettingsPanel_Length = camera.SensorNumber;

                Single[] ParameterCurrentValue = new Single[parameterSettingsPanel_Length];

                for (i = 0; i < parameterSettingsPanel_Length; i++) //循环所有烟支
                {
                    ParameterCurrentValue[i] = camera.DeviceParameter.SensorAdjustValue[i];
                }

                //

                parameterSettingsPanel.ParameterCurrentValue = ParameterCurrentValue;//当前值             

                parameterSettingsPanel._ClearSelectState();
                parameterSettingsPanel._Apply(false);

                bSaveProduct = true;

                customButtonSaveProduct.BitmapWhole = VisionSystemControlLibrary.Properties.Resources.Button_1;//【SAVE PRODUCT】

                customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【SAVE PRODUCT】
            }

            if ((1 == uiSensorAdjustState) && (iSensorAdjusting == 0)) //传感器校准结束
            {
                customButtonMessage.Chinese_TextDisplay = new string[] { "请低速盘车..." };
                customButtonMessage.English_TextDisplay = new string[] { "Please Start at Low Speed..." };
                customButtonMessage.Visible = true;
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
                customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【SAVE PRODUCT】
                customButtonFocusCalibration.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【FOCUS CALIBRATION】
                customButtonWhiteBalance.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【WHITE BALANCE】
                customButtonAdjust.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【Adjust】

                parameterSettingsPanel.ControlEnabled = false;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：保存数据
        // 输入参数：1.bSuccess：保存是否成功。取值范围：true，是；false，否
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _SaveProduct(Boolean bSuccess)
        {
            if (bSuccess)//成功
            {
                bSaveProduct = false;

                customButtonSaveProduct.BitmapWhole = VisionSystemControlLibrary.Properties.Resources.Button;//【SAVE PRODUCT】

                customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【SAVE PRODUCT】

                //

                camera._SaveParameter();

                File.Copy(camera.DataPath + VisionSystemClassLibrary.Class.Camera.ParameterFileName, brand.BrandPath + brand.CURRENTBrandName + "\\" + camera.CameraENGName + "\\" + VisionSystemClassLibrary.Class.Camera.ParameterFileName, true);

                //VisionSystemClassLibrary.Class.System.FileCopyFun(camera.DataPath + VisionSystemClassLibrary.Class.Camera.ParameterFileName, brand.BrandPath + brand.CURRENTBrandName + "\\" + camera.CameraENGName + "\\" + VisionSystemClassLibrary.Class.Camera.ParameterFileName);

                //

                camera._CopyTo(ref cameraTemp);

                //显示信息窗口

                GlobalWindows.MessageDisplay_Window.WindowParameter = 37;//窗口特征数值
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][1];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][1];

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
            else//失败
            {
                //显示信息窗口

                GlobalWindows.MessageDisplay_Window.WindowParameter = 38;//窗口特征数值
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.Ok;//包含【确定】按钮
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Language = language;//语言
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.Chinese_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese - 1][2];
                GlobalWindows.MessageDisplay_Window.MessageDisplayControl.English_Message_4 = sMessageText[(Int32)VisionSystemClassLibrary.Enum.InterfaceLanguage.English - 1][2];

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
        }

        //事件

        //-----------------------------------------------------------------------
        // 功能说明：窗口加载函数
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void ImageConfigurationControl_Load(object sender, EventArgs e)
        {
            //不执行操作
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【Close】按钮事件，执行相应的操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonClose_CustomButton_Click(object sender, EventArgs e)
        {
            if (bSaveProduct)//参数被修改
            {
                bClickCloseButton = true;

                //显示信息窗口

                GlobalWindows.MessageDisplay_Window.WindowParameter = 36;//窗口特征数值
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
            else//参数未被修改
            {
                _Close();
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【SAVE PRODUCT】按钮事件，执行相应的操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonSaveProduct_CustomButton_Click(object sender, EventArgs e)
        {
            if (bSaveProduct)//参数被修改
            {
                //显示信息窗口

                GlobalWindows.MessageDisplay_Window.WindowParameter = 36;//窗口特征数值
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
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【FOCUS CALIBRATION】按钮事件，执行相应的操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonFocusCalibration_CustomButton_Click(object sender, EventArgs e)
        {
            bFocusCalibrationSelected = !bFocusCalibrationSelected;//【FOCUS CALIBRATION】

            if (bFocusCalibrationSelected)//按下
            {
                customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【SAVE PRODUCT】
                customButtonWhiteBalance.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【WHITE BALANCE】
                customButtonAdjust.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【Adjust】
            }
            else//未按下
            {
                if (bSaveProduct)//参数被修改
                {
                    customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【SAVE PRODUCT】
                } 
                else//参数未修改
                {
                    customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【SAVE PRODUCT】
                }
                customButtonWhiteBalance.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【WHITE BALANCE】
                customButtonAdjust.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【Adjust】
            }

            parameterSettingsPanel.ControlEnabled = !bFocusCalibrationSelected;//参数设置面板

            //事件

            if (null != FocusCalibration_Click)//有效
            {
                FocusCalibration_Click(this, new CustomEventArgs());
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【WHITE BALANCE】按钮事件，执行相应的操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonWhiteBalance_CustomButton_Click(object sender, EventArgs e)
        {
            bWhiteBalanceSelected = !bWhiteBalanceSelected;//【WHITE BALANCE】

            if (bWhiteBalanceSelected)//按下
            {
                customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【SAVE PRODUCT】
                customButtonFocusCalibration.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【FOCUS CALIBRATION】
                customButtonAdjust.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【Adjust】
            }
            else//未按下
            {
                if (bSaveProduct)//参数被修改
                {
                    customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【SAVE PRODUCT】
                }
                else//参数未修改
                {
                    customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【SAVE PRODUCT】
                }
                customButtonFocusCalibration.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【FOCUS CALIBRATION】
                customButtonAdjust.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【Adjust】
            }

            parameterSettingsPanel.ControlEnabled = !bWhiteBalanceSelected;//参数设置面板

            //事件

            if (null != WhiteBalance_Click)//有效
            {
                WhiteBalance_Click(this, new CustomEventArgs());
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：点击图像显示控件的事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void imageDisplayView_Control_Click(object sender, EventArgs e)
        {
            CustomEventArgs customeventargs = (CustomEventArgs)e;

            Point pointValue = new Point(customeventargs.IntValue[0], customeventargs.IntValue[1]);//源图像坐标

            //

            customButtonAxisValue.Chinese_TextDisplay = new String[1] { pointValue.X.ToString() + "，" + pointValue.Y.ToString() };//设置显示的文本
            customButtonAxisValue.English_TextDisplay = new String[1] { pointValue.X.ToString() + "，" + pointValue.Y.ToString() };//设置显示的文本

            customButtonColorValue.Chinese_TextDisplay = new String[1] { camera.ImageLive[pointValue.Y, pointValue.X].Red.ToString() + "，" + camera.ImageLive[pointValue.Y, pointValue.X].Green.ToString() + "，" + camera.ImageLive[pointValue.Y, pointValue.X].Blue.ToString() };//设置显示的文本
            customButtonColorValue.English_TextDisplay = new String[1] { camera.ImageLive[pointValue.Y, pointValue.X].Red.ToString() + "，" + camera.ImageLive[pointValue.Y, pointValue.X].Green.ToString() + "，" + camera.ImageLive[pointValue.Y, pointValue.X].Blue.ToString() };//设置显示的文本
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：参数进行了设置的事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void parameterSettingsPanel_ParameterValueChanged(object sender, EventArgs e)
        {
            try
            {
                UInt16 iValue = (UInt16)parameterSettingsPanel.ParameterCurrentValue[parameterSettingsPanel.CurrentSelectedValueIndex];

                if (false == camera.IsSerialPort) //当前为相机
                {
                    switch (parameterSettingsPanel.CurrentSelectedValueIndex)
                    {
                        case 0://Strobo Time
                            //
                            camera.DeviceParameter.StroboTime = iValue;
                            //
                            break;
                        case 1://Strobo Current
                            //
                            camera.DeviceParameter.StroboCurrent = iValue;
                            //
                            break;
                        case 2://Gain
                            //
                            camera.DeviceParameter.Gain = iValue;
                            //
                            break;
                        case 3://Exposure Time
                            //
                            camera.DeviceParameter.ExposureTime = iValue;
                            //
                            break;
                        case 4://White Balance
                            //
                            camera.DeviceParameter.WhiteBalance = iValue;

                            if (0 == camera.DeviceParameter.WhiteBalance)//自动
                            {
                                parameterSettingsPanel.ParameterEnabled = new Boolean[] { false, false, false, false, true, false, false, false };//参数使能状态
                            }
                            else//手动
                            {
                                parameterSettingsPanel.ParameterEnabled = new Boolean[] { true, true, true, true, true, true, true, true };//参数使能状态
                            }
                            //
                            break;
                        case 5://White Balance（Red）
                            //
                            camera.DeviceParameter.WhiteBalance_Red = iValue;
                            //
                            break;
                        case 6://White Balance（Green）
                            //
                            camera.DeviceParameter.WhiteBalance_Green = iValue;
                            //
                            break;
                        case 7://White Balance（Blue）
                            //
                            camera.DeviceParameter.WhiteBalance_Blue = iValue;
                            //
                            break;
                        default:
                            break;
                    }

                    //事件

                    if (null != ParameterValueChanged)//有效
                    {
                        ParameterValueChanged(this, new CustomEventArgs());
                    }
                }
                else //当前为串口
                {
                    camera.DeviceParameter.SensorAdjustValue[parameterSettingsPanel.CurrentSelectedValueIndex] = Convert.ToByte(iValue);

                    //事件

                    if (null != ParameterValueChanged)//有效
                    {
                        ParameterValueChanged(this, new CustomEventArgs());
                    }
                }
            }
            catch (System.Exception ex)
            {
                //不执行操作
            }

            //

            bSaveProduct = true;

            customButtonSaveProduct.BitmapWhole = VisionSystemControlLibrary.Properties.Resources.Button_1;//【SAVE PRODUCT】

            customButtonSaveProduct.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【SAVE PRODUCT】
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：IMAGE CONFIGURATION，【SAVE PRODUCT】确认，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_ImageConfiguration_SaveProduct_Confirm(object sender, EventArgs e)
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

            if (GlobalWindows.MessageDisplay_Window.MessageDisplayControl.OkCancel)//确定
            {
                //事件

                if (null != SaveProduct_Click)//有效
                {
                    SaveProduct_Click(this, new CustomEventArgs());
                }
            }
            else//取消
            {
                if (bClickCloseButton)//点击【CLOSE】按钮
                {
                    _Close();
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：IMAGE CONFIGURATION，【SAVE PRODUCT】成功，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_ImageConfiguration_SaveProduct_Success(object sender, EventArgs e)
        {
            if (bClickCloseButton)//点击【CLOSE】按钮
            {
                _Close();
            }

            //

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
        // 功能说明：IMAGE CONFIGURATION，【SAVE PRODUCT】失败，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void messageDisplayWindow_WindowClose_ImageConfiguration_SaveProduct_Failure(object sender, EventArgs e)
        {
            if (bClickCloseButton)//点击【CLOSE】按钮
            {
                _Close();
            }

            //

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
        // 功能说明：SensorSelect_Window，窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：控件自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void SensorSelect_Window_WindowClose(object sender, EventArgs e)
        {
            if (GlobalWindows.TopMostWindows)//置顶
            {
                GlobalWindows.SensorSelect_Window.TopMost = false;//取消置于顶层
            }
            else//其它
            {
                GlobalWindows.SensorSelect_Window.Visible = false;//隐藏
            }

            if (GlobalWindows.SensorSelect_Window.SensorSelectControl.EnterNewValue)//执行确认操作
            {
                camera.DeviceParameter.SensorSelectState = GlobalWindows.SensorSelect_Window.SensorSelectControl.SensorSelectState;

                uiSensorAdjustState = 1;

                customButtonAdjust.Chinese_TextDisplay = new string[] { "取消校准" };
                customButtonAdjust.English_TextDisplay = new string[] { "Adjust Cancelled" };

                customButtonWhiteBalance.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【WHITE BALANCE】
                customButtonFocusCalibration.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Disable;//【FOCUS CALIBRATION】

                customButtonMessage.Visible = false;

                parameterSettingsPanel.ParameterEnabled = new Boolean[camera.SensorNumber];//参数使能状态

                //事件

                if (null != ParameterValueChanged)//有效
                {
                    ParameterValueChanged(this, new CustomEventArgs());
                }
            }
            else
            {
                customButtonAdjust.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：点击【校准】按钮事件，执行相应的操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void customButtonAdjust_CustomButton_Click(object sender, EventArgs e)
        {
            if (0 == uiSensorAdjustState) //改变前状态为取消校准
            {
                //显示窗口

                GlobalWindows.SensorSelect_Window.SensorSelectControl.Language = language;//语言
                GlobalWindows.SensorSelect_Window.SensorSelectControl._Properties(camera.DeviceParameter);//设备

                GlobalWindows.SensorSelect_Window.StartPosition = FormStartPosition.CenterScreen;
                if (GlobalWindows.TopMostWindows)//置顶
                {
                    GlobalWindows.SensorSelect_Window.TopMost = true;//将窗口置于顶层
                }
                else//其它
                {
                    GlobalWindows.SensorSelect_Window.Visible = true;//显示
                }
            }
            else  //改变前状态为校准
            {
                uiSensorAdjustState = 0;

                customButtonAdjust.Chinese_TextDisplay = new string[] { "校准" };
                customButtonAdjust.English_TextDisplay = new string[] { "Adjust" };

                customButtonWhiteBalance.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【WHITE BALANCE】
                customButtonFocusCalibration.CustomButtonBackgroundImage = CustomButton_BackgroundImage.Up;//【FOCUS CALIBRATION】

                customButtonMessage.Visible = false;

                Int32 parameterSettingsPanel_Length = camera.SensorNumber;
                
                if (iSensorAdjusting == 1)//校准未完成
                {
                    for (Int32 i = 0; i < camera.DeviceParameter.SensorAdjustValue.Length; i++) //循环所有传感器
                    {
                        camera.DeviceParameter.SensorAdjustValue[i] = cameraTemp.DeviceParameter.SensorAdjustValue[i];
                    }

                    Single[] ParameterCurrentValue = new Single[parameterSettingsPanel_Length];

                    for (Int32 i = 0; i < parameterSettingsPanel_Length; i++) //循环所有烟支
                    {
                        ParameterCurrentValue[i] = camera.DeviceParameter.SensorAdjustValue[i];
                    }

                    //

                    parameterSettingsPanel.ParameterCurrentValue = ParameterCurrentValue;//当前值             

                    parameterSettingsPanel._Apply(false);
                }

                Boolean[] ParameterEnabled = new Boolean[parameterSettingsPanel_Length];

                for (Int32 i = 0; i < parameterSettingsPanel_Length; i++) //循环所有烟支
                {
                    ParameterEnabled[i] = true;
                }
                parameterSettingsPanel.ParameterEnabled = ParameterEnabled;//参数使能状态

                //事件

                if (null != ParameterValueChanged)//有效
                {
                    ParameterValueChanged(this, new CustomEventArgs());
                }
            }
        }
    }
}