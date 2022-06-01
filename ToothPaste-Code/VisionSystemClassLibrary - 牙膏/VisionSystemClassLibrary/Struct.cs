/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：Structure.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：结构体

原作者：视觉检测团队
完成日期：2014/08/18
特别说明：经视觉检测团队授权并遵守代码使用条款，方可使用本代码并获得技术支持，否则所产生的一切后果由使用者承担

修改者：无
完成日期：无
修改记录：无

****************************************************************/

using System;
using System.Collections.Generic;
using System.Drawing;

using VisionSystemClassLibrary.Enum;

using Emgu.CV;
using Emgu.CV.Structure;

namespace VisionSystemClassLibrary.Struct
{
    [Serializable]
    public class ROI
    {
        public Boolean roiInnerExit;
        public ROI_Inner roiExtra = new ROI_Inner();
        public ROI_Inner roiInner = new ROI_Inner();

        /// <summary>
        /// 区域坐标拷贝函数
        /// </summary>
        /// <param name="roi"></param>
        public void _CopyTo(ref ROI roi)
        {
            roi.roiInnerExit = roiInnerExit;

            roiExtra._CopyTo(ref roi.roiExtra);
            roiInner._CopyTo(ref roi.roiInner);
        }

        /// <summary>
        /// 计算偏移
        /// </summary>
        /// <param name="point"></param>
        public void _Offset(Point point)
        {
            roiExtra._Offset(point);

            if (roiInnerExit)//内部ROI存在
            {
                roiInner._Offset(point);
            }
        }

        /// <summary>
        /// 计算偏移
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void _Offset(Int32 x, Int32 y)
        {
            roiExtra._Offset(x, y);

            if (roiInnerExit)//内部ROI存在
            {
                roiInner._Offset(x, y);
            }
        }
    }

    [Serializable]
    public class ROI_Inner
    {
        public ROIType roiType;
        public Point Point1;//矩形：左上角横坐标；椭圆：中心横坐标；四边形：左上角横坐标;/矩形：左上角纵坐标；椭圆：中心纵坐标；四边形：左上角纵坐标
        public Point Point2;//矩形：宽度；椭圆：横坐标半径；四边形：右上角横坐标;矩形：高度；椭圆：纵坐标半径；四边形：右上角纵坐标
        public Point Point3;//四边形：右下角横坐标;四边形：右下角纵坐标
        public Point Point4;//四边形：左下角横坐标;//四边形：左下角纵坐标

        /// <summary>
        /// 拷贝函数
        /// </summary>
        /// <param name="roi_Inner"></param>
        public void _CopyTo(ref ROI_Inner roi_Inner)
        {
            roi_Inner.roiType = roiType;
            roi_Inner.Point1 = Point1;
            roi_Inner.Point2 = Point2;
            roi_Inner.Point3 = Point3;
            roi_Inner.Point4 = Point4;
        }

        /// <summary>
        /// 计算偏移
        /// </summary>
        /// <param name="point"></param>
        public void _Offset(Point point)
        {
            switch (roiType)
            {
                case ROIType.Quadrangle:
                    Point1.Offset(point);
                    Point2.Offset(point);
                    Point3.Offset(point);
                    Point4.Offset(point);
                    break;
                default:
                    Point1.Offset(point);
                    break;
            }
        }

        /// <summary>
        /// 计算偏移
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void _Offset(Int32 x, Int32 y)
        {
            switch (roiType)
            {
                case ROIType.Quadrangle:
                    Point1.Offset(x, y);
                    Point2.Offset(x, y);
                    Point3.Offset(x, y);
                    Point4.Offset(x, y);
                    break;
                default:
                    Point1.Offset(x, y);
                    break;
            }
        }
    }

    [Serializable]
    public class Arithmetic
    {
        public Byte Number;//算法数量
        public Boolean[] State;//算法启用状态
        public string[] ENGName;//算法英文名
        public string[] CHNName;//算法中文名
        public Byte[] Type;//算法类型1为枚举类型，2为数字类型

        public Int16[] CurrentValue;//算法参数数字类型当前值
        public Int16[] MinValue;//算法参数数字类型最小值
        public Int16[] MaxValue;//算法参数数字类型最大值

        public Byte[] EnumType;//枚举类型
        public Byte[] EnumNumber;//枚举参数长度
        public Boolean[,] EnumState;//枚举参数选中状态
        public Byte[] EnumCurrent;//当前枚举参数索引

        //-----------------------------------------------------------------------
        // 功能说明：算法类拷贝函数
        // 输入参数：1.Arithmetic：arithmetic，工具参数
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _CopyTo(ref Arithmetic arithmetic)
        {
            arithmetic.Number = Number;
            arithmetic.State = new Boolean[arithmetic.Number];
            arithmetic.ENGName = new string[arithmetic.Number];
            arithmetic.CHNName = new string[arithmetic.Number];
            arithmetic.Type = new Byte[arithmetic.Number];
            arithmetic.CurrentValue = new Int16[arithmetic.Number];
            arithmetic.MinValue = new Int16[arithmetic.Number];
            arithmetic.MaxValue = new Int16[arithmetic.Number];
            arithmetic.EnumType = new Byte[arithmetic.Number];
            arithmetic.EnumNumber = new Byte[arithmetic.Number];
            arithmetic.EnumCurrent = new Byte[arithmetic.Number];
            arithmetic.EnumState = new Boolean[arithmetic.Number, VisionSystemClassLibrary.String.TextData.EnumType_CHN.Length];

            Array.Copy(State, arithmetic.State, State.Length);
            Array.Copy(ENGName, arithmetic.ENGName, ENGName.Length);
            Array.Copy(CHNName, arithmetic.CHNName, CHNName.Length);
            Array.Copy(Type, arithmetic.Type, Type.Length);
            Array.Copy(CurrentValue, arithmetic.CurrentValue, CurrentValue.Length);
            Array.Copy(MinValue, arithmetic.MinValue, MinValue.Length);
            Array.Copy(MaxValue, arithmetic.MaxValue, MaxValue.Length);
            Array.Copy(EnumType, arithmetic.EnumType, EnumType.Length);
            Array.Copy(EnumNumber, arithmetic.EnumNumber, EnumNumber.Length);
            Array.Copy(EnumCurrent, arithmetic.EnumCurrent, EnumCurrent.Length);
            Array.Copy(EnumState, arithmetic.EnumState, EnumState.LongLength);
        }
    }

    [Serializable]
    public struct BrandData
    {
        public string Name;//品牌名称
        public VisionSystemClassLibrary.Enum.BrandType Type;//品牌类型
    }

    [Serializable]
    public class DeviceData
    {
        public Boolean Connected;//是否与客户端建立了连接。取值范围：true，是；false，否
        public Boolean GetDevInfo;//是否存储了客户端的设备信息。取值范围：true，是；false，否
        public string SerialNumber;//序列号
        public string MAC;//MAC地址
        public string IP;//IP地址
        public string Firmware;//固件版本
        public string DeviceName;//设备（相机）名称
        public string ControllerName;//控制器名称

        public CameraType Type;//相机类型
        public CameraState CAM;//相机状态
        public Byte Port;//相机端口

        public CameraState CAM_Temp;//相机状态缓存

        //-----------------------------------------------------------------------
        // 功能说明：DeviceData类拷贝函数
        // 输入参数：1.DeviceData：deviceData，DeviceData参数
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _CopyTo(DeviceData deviceData)
        {
            deviceData.Connected = Connected;
            deviceData.GetDevInfo = GetDevInfo;
            deviceData.SerialNumber = SerialNumber;
            deviceData.MAC = MAC;
            deviceData.IP = IP;
            deviceData.Firmware = Firmware;
            deviceData.DeviceName = DeviceName;
            deviceData.ControllerName = ControllerName;
            deviceData.Type = Type;
            deviceData.CAM = CAM;
            deviceData.Port = Port;

            deviceData.CAM_Temp = CAM_Temp;
        }
    }

    [Serializable]
    public struct CameraConfiguration
    {
        public VisionSystemClassLibrary.Enum.CameraType Type;//相机类型
        public Boolean Selected;//是否被选择。取值范围：true，是；false，否

        public string CameraENGName;//相机英文名
        public string CameraCHNName;//相机中文名

        public string ControllerENGName;//控制器英文名
        public string ControllerCHNName;//控制器中文名

        public Byte IPValue;//相机IP地址最后一位
        public string IPAddress;//相机IP地址

        public Byte Port;//相机端口（用于生成Controller配置文件）
        public UInt64 CameraFaultState;//相机故障标记（用于生成Controller配置文件）

        public Boolean CheckEnable;//相机检测使能
        public VisionSystemClassLibrary.Enum.CameraRotateAngle CameraAngle;//相机旋转角度。0:0度;1:90度;2:180度;3:270度

        public Boolean BitmapLockBitsResize;//原始图像数据截取区域缩放（用于生成Controller配置文件）
        public Boolean BitmapLockBitsCenter;//原始图像数据截取区域缩放后是否居中（用于生成Controller配置文件）
        public Point BitmapLockBitsAxis;//原始图像数据截取区域粘贴位置（控制器程序使用）
        public Rectangle BitmapLockBitsArea;//原始图像数据截取区域（用于生成Controller配置文件）

        public Boolean IsSerialPort; //是否为串口

        public VisionSystemClassLibrary.Enum.TobaccoSortType TobaccoSortType_E;//烟支排列类型

        public VisionSystemClassLibrary.Enum.VideoColor VideoColor;//相机颜色
        public VisionSystemClassLibrary.Enum.VideoResolution VideoResolution;//相机分辨率

        public CameraFlip CameraFlip;//相机镜像

        public RelevancyCameraInformation RelevancyCameraInfo;//关联相机信息

        public SensorProductType Sensor_ProductType;//传感器应用产品类型
    }

    [Serializable]
    public struct ImageData
    {
        //public Bitmap GraphicsData;//图像数据
        public Image<Bgr, Byte> GraphicsData;//图像数据

        public ImageInformation Information;//图像信息
    }

    [Serializable]
    public class ImageInformation
    {
        public Boolean Valid;//图像是否有效。true：是；false：否

        public int ToolsIndex;//图像所属的工具索引值（从0开始）

        public Boolean ToolState;//工具状态

        public ImageType Type;//图像类型

        public Double Scale;//缩放比例

        public string Name;//信息名称

        public const int TotalNumber = 12;//Slot中包含的区块总数
        public Boolean[] Value;//区块的数值。取值范围：true，表示区块有效；false，表示区块无效

        public Boolean ValueDisplay;//在显示图像的标题栏中是否显示最小值、最大值和当前值。true：是；false：否

        public Int32 MinValue;//最小值
        public Int32 MaxValue;//最大值
        public Int32 CurrentValue;//当前值

        public DateTime DateTimeImage;//图像产生的时间

        public Int16 ErrorValue;//图像显示的Error数值（取值为-1，表示该数值无意义，即在图像上不显示）
        public Int16 StepValue;//图像显示的Step数值（取值为-1，表示该数值无意义，即在图像上不显示）

        public Int32 InspectionTime;//质量检测页面，图像处理时间（ms）

        public Int32 Compensation_H;
        public Int32 Compensation_V;

        public ROI ROI_StaticsImage = new ROI();//兴趣区域

        public Boolean DeepLearningState;//深度学习标记

        //-----------------------------------------------------------------------
        // 功能说明：ImageInformation类拷贝函数
        // 输入参数：1.ImageInformation：imageInformation，ImageInformation参数
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _CopyTo(ImageInformation imageInformation)
        {
            imageInformation.Valid = Valid;
            imageInformation.ToolsIndex = ToolsIndex;
            imageInformation.ToolState = ToolState;
            imageInformation.Type = Type;
            imageInformation.Scale = Scale;
            imageInformation.Name = Name;
            imageInformation.ValueDisplay = ValueDisplay;
            imageInformation.MinValue = MinValue;
            imageInformation.MaxValue = MaxValue;
            imageInformation.CurrentValue = CurrentValue;
            imageInformation.ErrorValue = ErrorValue;
            imageInformation.StepValue = StepValue;
            imageInformation.InspectionTime = InspectionTime;
            imageInformation.DateTimeImage = new DateTime(DateTimeImage.Year, DateTimeImage.Month, DateTimeImage.Day, DateTimeImage.Hour, DateTimeImage.Minute, DateTimeImage.Second, DateTimeImage.Millisecond);

            imageInformation.Value = new Boolean[TotalNumber];
            Array.Copy(Value, imageInformation.Value, Value.Length);

            imageInformation.Compensation_H = Compensation_H;
            imageInformation.Compensation_V = Compensation_V;

            ROI_StaticsImage._CopyTo(ref imageInformation.ROI_StaticsImage);
            imageInformation.DeepLearningState = DeepLearningState;
        }

        //-----------------------------------------------------------------------
        // 功能说明：初始化函数
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _InitData()
        {
            Int32 i = 0;//循环控制变量

            Valid = false;//图像是否有效。true：是；false：否
            ToolsIndex = 0;//图像所属的工具索引值（从0开始）
            ToolState = false;//工具状态
            Type = ImageType.Error;//图像类型
            Scale = 1.0;//缩放比例
            Name = "";//信息名称
            Value = new Boolean[TotalNumber];//区块的数值。取值范围：true，表示区块有效；false，表示区块无效
            for (i = 0; i < TotalNumber; i++)
            {
                Value[i] = false;
            }
            ValueDisplay = true;//在显示图像的标题栏中是否显示最小值、最大值和当前值。true：是；false：否
            MinValue = 0;//最小值
            MaxValue = 0;//最大值
            CurrentValue = 0;//当前值
            DateTimeImage = new DateTime();//图像产生的时间
            ErrorValue = 0;//图像显示的Error数值（取值为-1，表示该数值无意义，即在图像上不显示）
            StepValue = 0;//图像显示的Step数值（取值为-1，表示该数值无意义，即在图像上不显示）
            InspectionTime = 0;
            Compensation_H = 0;
            Compensation_V = 0;
            ROI_StaticsImage = new ROI();
            DeepLearningState = false;
        }
    }

    [Serializable]
    public class IOSignal
    {
        public UInt32 OutputState;//输出通道状态
        public UInt32 OutputDiagStateLab;//输出通道诊断状态（实验室）
        public UInt32 OutputDiagState;//输出通道诊断状态（工作）
        public UInt32 InputState;//输入通道状态
    }

    [Serializable]
    public class LiveData
    {
        public Boolean CameraSelected;//WORK，相机显示控件是否被选中。true：是；false：否

        public ImageInformation GraphicsInformation;//图像信息
    }

    [Serializable]
    public class RejectsData
    {
        public CameraType Cameratype;//相机类型（默认值：CameraType.Top）

        public ImageInformation GraphicsInformation;//属性，图像信息
    }

    [Serializable]
    public struct TolerancesGraphData_Value
    {
        public Int32[] Value;//数值
        public Int32 MeanValue;//平均值
        public Int32 CurrentValueIndex;//绘制区域中当前显示的曲线值点（从0开始，小于0表示曲线图数值无效，最大值为ValueNumber - 1）
        public Double AdditionalValue;//曲线图坐标轴上显示的附加数值

        //-----------------------------------------------------------------------
        // 功能说明：TolerancesGraphData_Value类拷贝函数
        // 输入参数：1.tolerancesGraphDataValue：参数
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _CopyTo(ref TolerancesGraphData_Value tolerancesGraphDataValue)
        {
            tolerancesGraphDataValue.MeanValue = MeanValue;
            tolerancesGraphDataValue.CurrentValueIndex = CurrentValueIndex;
            tolerancesGraphDataValue.AdditionalValue = AdditionalValue;
            Array.Copy(Value, tolerancesGraphDataValue.Value, Value.Length);
        }
    }

    [Serializable]
    public class TolerancesGraphData
    {
        public Int32 ToolsIndex;//工具序号（工具数组序号，取值范围：从0开始）

        public Int32 TolerancesID;//曲线图控件序号。取值范围：1 ~ 4（TOLERANCES页面中，每页最多显示4个曲线图，从上到下序号为1 ~ 4，该变量即表示所对应的曲线图序号）

        public Int32 Page;//曲线图控件所属的页码（从0开始）

        public Boolean ButtonONOFFShow;//【启用/禁用】按钮是否显示。true：是；false：否
        public Boolean ButtonLearningShow;//【学习】按钮是否显示。true：是；false：否
        public Boolean EffectiveMin_ReadOnly;//坐标轴最小有效实际数值是否只读。true：是；false：否
        public Boolean EffectiveMax_ReadOnly;//坐标轴最大有效实际数值是否只读。true：是；false：否

        public Boolean AdditionalValueDisplay;//曲线图坐标轴上是否显示附加数值。true：是；false：否
        public Single AdditionalValueRatio;//曲线图坐标轴上显示的附加数值系数
        public System.String AdditionalValueUnit;//曲线图坐标轴上显示的附加数值单位
        public Int32 EffectiveMin_Value;//坐标轴最小有效实际数值
        public Int32 EffectiveMax_Value;//坐标轴最大有效实际数值

        public Int32 ValueNumber;//绘制区域中的曲线值数目

        //

        public TolerancesGraphData_Value TolerancesGraphDataValue;//绘制区域信息

        //

        //

        //-----------------------------------------------------------------------
        // 功能说明：初始化
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _Init()
        {
            Int32 i = 0;//循环控制变量

            ToolsIndex = 0;//工具序号（工具数组序号，取值范围：从0开始）
            TolerancesID = 1;//曲线图控件序号。取值范围：1 ~ 4（TOLERANCES页面中，每页最多显示4个曲线图，从上到下序号为1 ~ 4，该变量即表示所对应的曲线图序号）
            Page = 0;//曲线图控件所属的页码（从0开始）
            ButtonONOFFShow = false;//【启用/禁用】按钮是否显示。true：是；false：否
            ButtonLearningShow = false;//【学习】按钮是否显示。true：是；false：否
            EffectiveMin_ReadOnly = false;//坐标轴最小有效实际数值是否只读。true：是；false：否
            EffectiveMax_ReadOnly = false;//坐标轴最大有效实际数值是否只读。true：是；false：否

            AdditionalValueDisplay = false;
            AdditionalValueRatio = (Single)6.36;
            AdditionalValueUnit = "pixel";
            EffectiveMin_Value = 0;//坐标轴最小有效实际数值
            EffectiveMax_Value = 100000;//坐标轴最大有效实际数值
            ValueNumber = 100;//绘制区域中的曲线值数目（坐标点数目，根据X轴数值获取）
            TolerancesGraphDataValue.Value = new Int32[ValueNumber];//数值
            for (i = 0; i < TolerancesGraphDataValue.Value.Length; i++)
            {
                TolerancesGraphDataValue.Value[i] = 500;
            }
            TolerancesGraphDataValue.CurrentValueIndex = 99;//绘制区域中当前显示的曲线值点（从0开始，小于0表示曲线图数值无效，最大值为ValueNumber - 1）
            TolerancesGraphDataValue.MeanValue = 500;//平均值
            TolerancesGraphDataValue.AdditionalValue = 0;//
        }

        //-----------------------------------------------------------------------
        // 功能说明：工具类拷贝函数
        // 输入参数：1.Tools：tool，工具参数
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _CopyTo(ref TolerancesGraphData tolerancesGraphData)
        {
            tolerancesGraphData.ToolsIndex=ToolsIndex;

            tolerancesGraphData.TolerancesID=TolerancesID;

            tolerancesGraphData.Page=Page;

            tolerancesGraphData.ButtonONOFFShow=ButtonONOFFShow;
            tolerancesGraphData.ButtonLearningShow=ButtonLearningShow;
            tolerancesGraphData.EffectiveMin_ReadOnly=EffectiveMin_ReadOnly;
            tolerancesGraphData.EffectiveMax_ReadOnly=EffectiveMax_ReadOnly;

            tolerancesGraphData.AdditionalValueDisplay=AdditionalValueDisplay;
            tolerancesGraphData.AdditionalValueRatio=AdditionalValueRatio;
            tolerancesGraphData.AdditionalValueUnit=AdditionalValueUnit;
            tolerancesGraphData.EffectiveMin_Value=EffectiveMin_Value;
            tolerancesGraphData.EffectiveMax_Value=EffectiveMax_Value;

            tolerancesGraphData.ValueNumber = ValueNumber;

            TolerancesGraphDataValue._CopyTo(ref tolerancesGraphData.TolerancesGraphDataValue);
        }
    }

    [Serializable]
    public class WorkData
    {
        public UInt16 CurrentPage;//当前页码（从0开始）

        public CameraType SelectedCameraType;//当前选中的相机显示控件所对应的相机类型（取值为CameraType.None，表示未选择任何相机显示控件）
        public Int16 SelectedCameraIndex;//当前选中的相机显示控件所对应的相机在相机数组中的索引值（取值为-1，表示未选择任何相机显示控件）

        public UInt16 ConnectedCameraNumber;//系统中连接的相机数量（0表示无连接的相机）
    }

    //

    [Serializable]
    public class CameraParameter
    {
        public Boolean StroboTime_Valid;//参数有效与否。取值范围：true，有效；false，无效
        public UInt16 StroboTime;//照明灯启动信号的持续时间
        public UInt16 StroboTime_Min;//照明灯启动信号的持续时间最小值
        public UInt16 StroboTime_Max;//照明灯启动信号的持续时间最大值

        public Boolean StroboCurrent_Valid;//参数有效与否。取值范围：true，有效；false，无效
        public UInt16 StroboCurrent;//照明灯启动信号的强度
        public UInt16 StroboCurrent_Min;//照明灯启动信号的强度最小值
        public UInt16 StroboCurrent_Max;//照明灯启动信号的强度最大值

        public Boolean Gain_Valid;//参数有效与否。取值范围：true，有效；false，无效
        public UInt16 Gain;//增益
        public UInt16 Gain_Min;//增益最小值
        public UInt16 Gain_Max;//增益最大值

        public Boolean ExposureTime_Valid;//参数有效与否。取值范围：true，有效；false，无效
        public UInt16 ExposureTime;//曝光时间
        public UInt16 ExposureTime_Min;//曝光时间最小值
        public UInt16 ExposureTime_Max;//曝光时间最大值

        public Boolean WhiteBalance_Valid;//参数有效与否。取值范围：true，有效；false，无效
        public UInt16 WhiteBalance;//白平衡（自动/手动）

        public Boolean WhiteBalance_Red_Valid;//参数有效与否。取值范围：true，有效；false，无效
        public UInt16 WhiteBalance_Red;//白平衡（红）
        public UInt16 WhiteBalance_Red_Min;//白平衡（红）最小值
        public UInt16 WhiteBalance_Red_Max;//白平衡（红）最大值

        public Boolean WhiteBalance_Green_Valid;//参数有效与否。取值范围：true，有效；false，无效
        public UInt16 WhiteBalance_Green;//白平衡（绿）
        public UInt16 WhiteBalance_Green_Min;//白平衡（绿）最小值
        public UInt16 WhiteBalance_Green_Max;//白平衡（绿）最大值

        public Boolean WhiteBalance_Blue_Valid;//参数有效与否。取值范围：true，有效；false，无效
        public UInt16 WhiteBalance_Blue;//白平衡（蓝）
        public UInt16 WhiteBalance_Blue_Min;//白平衡（蓝）最小值
        public UInt16 WhiteBalance_Blue_Max;//白平衡（蓝）最大值

        //

        public List<Int32> Parameter;//参数
        public List<Int32> Parameter_Min;//参数，最小值
        public List<Int32> Parameter_Max;//参数，最大值
        public List<string> Parameter_NameCHN;//参数，中文名称
        public List<string> Parameter_NameENG;//参数，英文名称

        //
        public Int32 SensorSelectState;//传感器校准选中状态

        public Byte[] SensorAdjustValue;//传感器校准值

        public Int16[] SensorADCValueMax;//传感器ADC值最大值

        //
        
        //-----------------------------------------------------------------------
        // 功能说明：CameraParameter构造函数
        // 输入参数：1.bSensorNumber：传感器数量
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _Init(Byte bSensorNumber)
        {
            SensorAdjustValue = new Byte[bSensorNumber];//传感器校准值

            SensorADCValueMax = new Int16[bSensorNumber];//传感器ADC值最大值
        }

        //-----------------------------------------------------------------------
        // 功能说明：CameraParameter类拷贝函数
        // 输入参数：1.cameraParameter：参数
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _CopyTo(ref CameraParameter cameraParameter)
        {
            cameraParameter.StroboTime_Valid = StroboTime_Valid;
            cameraParameter.StroboTime = StroboTime;
            cameraParameter.StroboTime_Min = StroboTime_Min;
            cameraParameter.StroboTime_Max = StroboTime_Max;

            cameraParameter.StroboCurrent_Valid = StroboCurrent_Valid;
            cameraParameter.StroboCurrent = StroboCurrent;
            cameraParameter.StroboCurrent_Min = StroboCurrent_Min;
            cameraParameter.StroboCurrent_Max = StroboCurrent_Max;

            cameraParameter.Gain_Valid = Gain_Valid;
            cameraParameter.Gain = Gain;
            cameraParameter.Gain_Min = Gain_Min;
            cameraParameter.Gain_Max = Gain_Max;

            cameraParameter.ExposureTime_Valid = ExposureTime_Valid;
            cameraParameter.ExposureTime = ExposureTime;
            cameraParameter.ExposureTime_Min = ExposureTime_Min;
            cameraParameter.ExposureTime_Max = ExposureTime_Max;

            cameraParameter.WhiteBalance_Valid = WhiteBalance_Valid;
            cameraParameter.WhiteBalance = WhiteBalance;

            cameraParameter.WhiteBalance_Red_Valid = WhiteBalance_Red_Valid;
            cameraParameter.WhiteBalance_Red = WhiteBalance_Red;
            cameraParameter.WhiteBalance_Red_Min = WhiteBalance_Red_Min;
            cameraParameter.WhiteBalance_Red_Max = WhiteBalance_Red_Max;

            cameraParameter.WhiteBalance_Green_Valid = WhiteBalance_Green_Valid;
            cameraParameter.WhiteBalance_Green = WhiteBalance_Green;
            cameraParameter.WhiteBalance_Green_Min = WhiteBalance_Green_Min;
            cameraParameter.WhiteBalance_Green_Max = WhiteBalance_Green_Max;

            cameraParameter.WhiteBalance_Blue_Valid = WhiteBalance_Blue_Valid;
            cameraParameter.WhiteBalance_Blue = WhiteBalance_Blue;
            cameraParameter.WhiteBalance_Blue_Min = WhiteBalance_Blue_Min;
            cameraParameter.WhiteBalance_Blue_Max = WhiteBalance_Blue_Max;
            
            //

            Int32 i = 0;//循环控制变量

            if (Parameter != null)
            {
                cameraParameter.Parameter.Clear();
                cameraParameter.Parameter.AddRange(Parameter);
                cameraParameter.Parameter_Min.Clear();
                cameraParameter.Parameter_Min.AddRange(Parameter_Min);
                cameraParameter.Parameter_Max.Clear();
                cameraParameter.Parameter_Max.AddRange(Parameter_Max);
                cameraParameter.Parameter_NameCHN.Clear();
                cameraParameter.Parameter_NameCHN.AddRange(Parameter_NameCHN);
                cameraParameter.Parameter_NameENG.Clear();
                cameraParameter.Parameter_NameENG.AddRange(Parameter_NameENG);
            }

            if (SensorAdjustValue != null)
            {
                cameraParameter.SensorAdjustValue = new Byte[SensorAdjustValue.Length];//参数
                Array.Copy(SensorAdjustValue, cameraParameter.SensorAdjustValue, SensorAdjustValue.Length);
            }

            if (SensorADCValueMax != null)
            {
                cameraParameter.SensorADCValueMax = new Int16[SensorADCValueMax.Length];//参数
                Array.Copy(SensorADCValueMax, cameraParameter.SensorADCValueMax, SensorADCValueMax.Length);
            }
        }
    }

    [Serializable]
    public class System_UIParameter
    {
        public Int32 Work_TotalPage;//页码总数

        public Boolean[] Work_BackgroundImage_Zoom;//背景图像是否缩放
        public Point[] Work_BackgroundImage_Location;//背景图像的位置

        public Single Work_SpeedPhase_Value_FontSize;//速度/相位数值，字体大小
        public Point Work_SpeedPhase_Value_Location;//速度/相位数值，位置
        public Size Work_SpeedPhase_Value_Size;//速度/相位数值，大小

        public Single Work_SpeedPhase_Unit_FontSize;//速度/相位单位，字体大小
        public Point Work_SpeedPhase_Unit_Location;//速度/相位单位，位置
        public Size Work_SpeedPhase_Unit_Size;//速度/相位单位，大小

        //

        public const string HMIApplicationName = "VisionSystemUserInterface";//人机界面程序名称
        public const string ControllerApplicationName = "VisionSystemImageProcessing";//控制器程序名称

        public const string HMIFileName = "VisionSystemUserInterfaceUpdate.exe";//人机界面程序更新程序名称
        public const string ControllerFileName = "VisionSystemImageProcessingUpdate.exe";//控制器程序更新程序名称

        public const string UpdateFilePath = "VisionSystem\\Update\\";//程序更新程序路径（不包含驱动器根路径名称）
    }

    [Serializable]
    public class Camera_UIParameter
    {
        public Boolean SpeedPhase_Display;//速度/相位显示与否。取值范围：true，是；false，否
        public Boolean SpeedPhase_AsMachine;//是否作为机器速度/相位。取值范围：true，是；false，否
        public Int32 Speed;//速度（计算）

        public Boolean LiveView_BackgroundImage_Zoom;//背景图像是否缩放
        public Point LiveView_BackgroundImage_Location;//背景图像的位置
        public Point LiveView_LineStart_Location;//指示线的起始坐标
        public Point LiveView_LineEnd_Location;//指示线的终止坐标

        public Int32 Work_Page;//所属的页码（从0开始）
        public Point Work_LineStart_Location;//指示线的起始坐标
        public Point Work_LineEnd_Location;//指示线的终止坐标
        public Int32 Work_ImageDisplayBackground_Left;//相机图像显示控件背景，左边距
        public Int32 Work_ImageDisplayBackground_Top;//相机图像显示控件背景，上边距
        public Point Work_ImageDisplay_Location;//相机图像显示控件位置
        public Size Work_ImageDisplay_Size;//相机图像显示控件大小
        public Single Work_ImageDisplay_Message_FontSize;//相机图像显示控件，Message，字体大小
        public Point Work_ImageDisplay_Message_Location;//相机图像显示控件，Message，位置
        public Size Work_ImageDisplay_Message_Size;//相机图像显示控件，Message，大小
        public Point Work_ImageDisplay_Slot_Location;//相机图像显示控件，Slot，位置
        public Size Work_ImageDisplay_Slot_Size;//相机图像显示控件，Slot，大小
        public Single Work_ImageDisplay_MinValue_FontSize;//相机图像显示控件，MinValue，字体大小
        public Point Work_ImageDisplay_MinValue_Location;//相机图像显示控件，MinValue，位置
        public Size Work_ImageDisplay_MinValue_Size;//相机图像显示控件，MinValue，大小
        public Single Work_ImageDisplay_CurrentValue_FontSize;//相机图像显示控件，CurrentValue，字体大小
        public Point Work_ImageDisplay_CurrentValue_Location;//相机图像显示控件，CurrentValue，位置
        public Size Work_ImageDisplay_CurrentValue_Size;//相机图像显示控件，CurrentValue，大小
        public Single Work_ImageDisplay_MaxValue_FontSize;//相机图像显示控件，MaxValue，字体大小
        public Point Work_ImageDisplay_MaxValue_Location;//相机图像显示控件，MaxValue，位置
        public Size Work_ImageDisplay_MaxValue_Size;//相机图像显示控件，MaxValue，大小
        public Point Work_ImageDisplay_Lamp_Location;//相机图像显示控件，Lamp，位置
        public Size Work_ImageDisplay_Lamp_Size;//相机图像显示控件，Lamp，大小
        public Single Work_Name_FontSize;//名称，字体大小
        public Point Work_Name_Location;//名称，位置
        public Size Work_Name_Size;//名称，大小
        public Single Work_State_FontSize;//状态，字体大小
        public Point Work_State_Location;//状态，位置
        public Size Work_State_Size;//状态，大小
        public Single Work_SpeedPhase_Value_FontSize;//速度/相位数值，字体大小
        public Point Work_SpeedPhase_Value_Location;//速度/相位数值，位置
        public Size Work_SpeedPhase_Value_Size;//速度/相位数值，大小
        public Single Work_SpeedPhase_Unit_FontSize;//速度/相位单位，字体大小
        public Point Work_SpeedPhase_Unit_Location;//速度/相位单位，位置
        public Size Work_SpeedPhase_Unit_Size;//速度/相位单位，大小
    }

    [Serializable]
    public struct SYSTEMTIME
    {
        public ushort Year;
        public ushort Month;
        public ushort DayOfWeek;
        public ushort Day;
        public ushort Hour;
        public ushort Minute;
        public ushort Second;
        public ushort MilliSeconds;

        //-----------------------------------------------------------------------
        // 功能说明：初始化函数
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _InitData()
        {
            Year = 0;
            Month = 0;
            DayOfWeek = 0;
            Day = 0;
            Hour = 0;
            Minute = 0;
            Second = 0;
            MilliSeconds = 0;
        }

        //-----------------------------------------------------------------------
        // 功能说明：初始化函数
        // 输入参数：1、DateTime：dateTime，系统时间
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _InitData(DateTime dateTime)
        {
            Year = (UInt16)dateTime.Year;
            Month = (UInt16)dateTime.Month;
            Day = (UInt16)dateTime.Day;
            Hour = (UInt16)dateTime.Hour;
            Minute = (UInt16)dateTime.Minute;
        }
    }

    [Serializable]
    public struct ClientCamera
    {
        public UInt16 CameraNumber;//属性，相机数量

        public CameraType[] CameraType;//属性，相机类型
        public string[] SerialNumber;//属性，客户端设备信息，序列号
        public string[] DeviceName;//属性，客户端设备信息，设备名称
    }

    [Serializable]
    public class ShiftData
    {
        public Int32 CurrentIndex;//属性，当前班次索引（从1开始；小于0表示空班次；0表示无效值）
        public Int32 CurrentIndexOld;//属性，当前班次索引（从1开始；小于0表示空班次；0表示无效值）

        public ShiftTime[] TimeData;
        public ShiftTime[] TimeDataOld;

        public StatisticsInformation[] InformationOfStatistics;

        public Struct.StatisticsInformation CurrentStatisticsInformation;//当前班次统计信息

        //-----------------------------------------------------------------------
        // 功能说明：初始化函数
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        public void _InitData(Int32 iCameraNumber, Int32 iDataNumber, Int32 iToolNumber)
        {
            Int32 i = 0;

            CurrentIndex = 0;//当前统计记录

            CurrentStatisticsInformation = new Struct.StatisticsInformation();

            TimeData = new ShiftTime[3];
            TimeDataOld = new ShiftTime[3];
            InformationOfStatistics = new StatisticsInformation[3];
            for (i = 0; i < 3; i++)
            {
                TimeData[i] = new ShiftTime();
                TimeDataOld[i] = new ShiftTime();

                TimeData[i]._InitData();
                TimeDataOld[i]._InitData();

                InformationOfStatistics[i] = new StatisticsInformation();
                InformationOfStatistics[i]._InitData(iCameraNumber, iDataNumber, iToolNumber);
            }
        }
    }

    [Serializable]
    public struct ShiftTime
    {
        public SYSTEMTIME Start;//班次开始时间
        public SYSTEMTIME End;//班次结束时间

        //-----------------------------------------------------------------------
        // 功能说明：初始化函数
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _InitData()
        {
            Start = new SYSTEMTIME();
            Start._InitData();

            End = new SYSTEMTIME();
            End._InitData();
        }
    }

    [Serializable]
    public struct StatisticsInformation
    {
        public Int32 CurrentIndex;//有效的统计数据索引

        //

        public ShiftTime[] TimeData;

        public StatisticsData[] DataOfStatistics;

        //-----------------------------------------------------------------------
        // 功能说明：初始化函数
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _InitData(Int32 iCameraNumber, Int32 iDataNumber, Int32 iToolNumber)
        {
            Int32 i = 0;//循环控制变量

            CurrentIndex = 0;//当前统计记录
        
            TimeData = new ShiftTime[iDataNumber];
            DataOfStatistics = new StatisticsData[iDataNumber];
            for (i = 0; i < iDataNumber; i++)
            {
                TimeData[i] = new ShiftTime();
                TimeData[i]._InitData();

                DataOfStatistics[i] = new StatisticsData();
                DataOfStatistics[i]._InitData(iCameraNumber, iToolNumber);
            }
        }
    }

    [Serializable]
    public struct StatisticsData
    {
        public string BrandName;//品牌

        //

        public StatisticsData_Camera[] CameraStatisticsData;//每个相机统计数据

        //

        public static Image<Bgr, Byte> ImageReject;
        public static ImageInformation GraphicsInformation;//01.04

        //-----------------------------------------------------------------------
        // 功能说明：初始化函数
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _InitData(Int32 iCameraNumber, Int32 iToolNumber)
        {
            Int32 i = 0;//循环控制变量
            
            BrandName = "";

            CameraStatisticsData = new StatisticsData_Camera[iCameraNumber];//故障信息
            for (i = 0; i < iCameraNumber; i++)
            {
                CameraStatisticsData[i] = new StatisticsData_Camera();
                CameraStatisticsData[i]._InitData(iToolNumber);
            }
        }
    }

    [Serializable]
    public struct StatisticsData_Camera
    {
        public CameraType TypeOfCamera;

        public UInt32 InspectedNumber;//已检测数量
        public UInt32 GoodNumber;//合格产品
        public UInt32 RejectedNumber;//剔除数量

        public UInt32[] RejectedStatistics_Tool;//每个工具的统计信息

        public UInt32 RejectedNumber_Relevancy;//剔除数量（关联）

        //-----------------------------------------------------------------------
        // 功能说明：初始化函数
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _InitData(Int32 iToolNumber)
        {
            Int32 i = 0;//循环控制变量

            TypeOfCamera = CameraType.None;

            InspectedNumber = 0;//已检测数量
            GoodNumber = 0;//合格产品
            RejectedNumber = 0;//剔除数量

            RejectedStatistics_Tool = new UInt32[iToolNumber];//图像信息
            for (i = 0; i < iToolNumber; i++)
            {
                RejectedStatistics_Tool[i] = 0;
            }

            RejectedNumber_Relevancy = 0;//剔除数量（关联）
        }
    }

    [Serializable]
    public struct FaultMessage
    {
        public SYSTEMTIME TimeData;//开始结束时间

        public Int32 DataIndex;//索引值

        //-----------------------------------------------------------------------
        // 功能说明：初始化函数
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _InitData()
        {
            DataIndex = 0;
            TimeData = new SYSTEMTIME();
        }
    }

    [Serializable]
    public struct StatisticsRecordList
    {
        public Int32 RecordNumber;

        public Int32 CurrentShiftIndex;//当前班次索引

        public StatisticsRecordListData[] RecordListData;

        //-----------------------------------------------------------------------
        // 功能说明：初始化函数
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _InitData(Int32 iShiftNumber, Int32 iRecordNumber)
        {
            Int32 i = 0;//循环控制变量

            RecordNumber = 0;
            CurrentShiftIndex = 0;

            RecordListData = new StatisticsRecordListData[iShiftNumber];

            for (i = 0; i < iShiftNumber; i++)
            {
                RecordListData[i] = new StatisticsRecordListData();
                RecordListData[i]._InitData(iRecordNumber);
            }
        }
    }

    [Serializable]
    public struct StatisticsRecordListData
    {
        public Int32 CurrentStatisticsRecordIndex;//当前统计数据索引索引

        public ShiftTime[] TimeData;

        public string[] BrandName;

        //-----------------------------------------------------------------------
        // 功能说明：初始化函数
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _InitData(Int32 iRecordNumber)
        {
            Int32 i = 0;//循环控制变量

            CurrentStatisticsRecordIndex = 0;

            TimeData = new ShiftTime[iRecordNumber];
            BrandName = new string[iRecordNumber];

            for (i = 0; i < iRecordNumber; i++)
            {
                TimeData[i] = new ShiftTime();
                TimeData[i]._InitData();

                BrandName[i] = "";
            }
        }
    }

    [Serializable]
    public struct RelevancyCameraInformation
    {
        public RelevancyType rRelevancyType;//关联类型

        public Dictionary<CameraType, Byte> RelevancyCameraInfo;//关联相机信息,value值从0开始

        //-----------------------------------------------------------------------
        // 功能说明：初始化函数
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _InitData()
        {
            rRelevancyType = RelevancyType.None;

            RelevancyCameraInfo = new Dictionary<CameraType, Byte>();
        }
    }
}