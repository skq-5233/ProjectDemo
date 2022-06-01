/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部
 
文件名称：Enum.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：枚举

原作者：视觉检测团队
完成日期：2014/08/18
特别说明：经视觉检测团队授权并遵守代码使用条款，方可使用本代码并获得技术支持，否则所产生的一切后果由使用者承担

修改者：无
完成日期：无
修改记录：无

****************************************************************/

namespace VisionSystemClassLibrary.Enum
{
    //语言
    public enum InterfaceLanguage : byte
    {
        Chinese = 1,//Chinese
        English = 2,//English
    }

    //相机类型
    public enum CameraType : byte
    {
        None = 0,//无效
        Camera_1 = 1,//相机1（原Top相机）
        Camera_2 = 2,//相机2（原Bottom相机）
        Camera_3 = 3,//相机3（原Side相机）
        Camera_4 = 4,//相机4
        Camera_5 = 5,//相机5
        Camera_6 = 6,//相机6
        Camera_7 = 7,//相机7
        Camera_8 = 8,//相机8
        Camera_9 = 9,//相机9
        Camera_10 = 10,//相机10
        Camera_11 = 11,//相机11
        Camera_12 = 12,//相机12
    }

    //相机类型
    public enum PortType : byte
    {
        None = 0,//无效
        Camera_1 = 1,//端口1
        Camera_2 = 2,//端口2
        Camera_3 = 3,//端口3
        Camera_4 = 4,//端口4
        Camera_Both = 15,//端口1 + 端口2 + 端口3 + 端口4
    }

    //坐标轴类型
    public enum AxisType
    {
        XAxis = 1,//X坐标轴
        YAxis = 2,//Y坐标轴
    }

    //坐标轴数据形式
    public enum AxisDataType
    {
        withoutEffectiveValue = 1,//不指定最小有效实际数值、最大有效实际数值，但指定坐标轴的最小值、最大值
        withEffectiveValue = 2,//指定最小有效实际数值、最大有效实际数值，同时指定坐标轴的最小值、最大值
        withPixelAndEffectiveValue = 3,//指定最小有效实际数值、最大有效实际数值，同时指定距离像素值
    }

    //自定义MessageDisplay窗口类型
    public enum MessageDisplayType
    {
        OkCancel = 1,//包含【OK】和【CANCEL】按钮
        Ok = 2,//包含【OK】按钮
        Cancel = 3,//包含【CANCEL】按钮
        None = 4,//不包含按钮
    }

    //图像信息中的图像类型
    public enum ImageType
    {
        Ok = 1,//完好图像
        Error = 2,//缺陷图像
        Pure = 3,//纯图像
    }

    //图像信息类型
    public enum ImageInformationType
    {
        Sample = 2,//学习
        Reject = 3,//剔除
    }

    //图像显示的方式
    public enum ImageShowMode
    {
        FilewithDrawingText = 1,//从文件中读取图像数据，并且需要在图像中绘制文本
        FilewithoutDrawingText = 2,//从文件中读取图像数据，并且不需要在图像中绘制文本
        ImagewithDrawingText = 3,//内存中获取图像数据，并且需要在图像中绘制文本
        ImagewithoutDrawingText = 4,//内存中获取图像数据，并且不需要在图像中绘制文本
    }

    //品牌类型
    public enum BrandType
    {
        None = 0,//无效
        Master = 1,//MASTER品牌
        Current = 2,//CURRENT品牌
        General = 3,//一般品牌
    }

    //设备状态
    public enum DeviceState
    {
        None = 0,//无效
        Stop = 1,//停止
        Ready = 2,//准备
        Run = 3,//运行
        BrandChanged = 4,//品牌更改
    }

    //相机状态
    public enum CameraState
    {
        NOTCONNECTED = 0x000001,//未连接
        CONNECTED = 0x000010,//连接
        ON = 0x000100,//打开
        OFF = 0x001000,//关闭
        NOTUPDATED = 0x010000,//未更新
        REJECTOFF = 0x100000,//剔除关闭
    }

    //DEVICE CONFIGURATION，CONFIG DEVICE页面执行的操作结果
    public enum DeviceConfigurationResult
    {
        None = 0,//未执行更新操作
        SelectController = 0x01,//选择了新的相机
        UpdateData = 0x10,//更新数据
    }

    //更新程序类型
    public enum UpdateApplicationResult
    {
        None = 0,//无更新程序
        HMI = 0x01,//人机界面程序更新
        Controller = 0x10,//控制器程序更新
    }

    //系统操作
    public enum _COMPUTER_NAME_FORMAT
    {
        ComputerNameNetBIOS,
        ComputerNameDnsHostName,
        ComputerNameDnsDomain,
        ComputerNameDnsFullyQualified,
        ComputerNamePhysicalNetBIOS,
        ComputerNamePhysicalDnsHostName,
        ComputerNamePhysicalDnsDomain,
        ComputerNamePhysicalDnsFullyQualified,
        ComputerNameMax
    }

    //

    public enum Check_Type
    {
        L_D = 1,
        D_L = 2,
        FirstGrad = 3,
    }

    public enum Detect_Type
    {
        Gradient = 1,
        Threshold = 2,
        PixelCount = 3,
        Gray = 4,
    }

    public enum Contrast_Color
    {
        Intensity = 0,
        Red = 1,
        Green = 2,
        Blue = 3,
        Single = 4,
    }

    public enum Scan_Direction
    {
        Left_Right = 1,
        Right_Left = 2,
        Top_Bottom = 3,
        Bottom_Top = 4,
        Horizen = 5,
        Vertical = 6,
    }

    public enum Angle_Adjust
    {
        H_Positive_45 = 1,
        H_Negative_45 = 2,
        V_Positive_45 = 3,
        V_Negative_45 = 4,
    }

    public enum H_Search
    {
        Zero_Step = 0,
        One_Step = 1,
        Two_Step = 2,
    }

    public enum V_Search
    {
        Zero_Step = 0,
        One_Step = 1,
        Two_Step = 2,
    }

    public enum Pos_Reference
    {
        Off = 0,
        On = 1,
    }

    //用户选择的工具类型
    public enum ToolType
    {
        Integrality = 0,//完整性
        HorizenPos = 1,//水平位置
        VerticalPos = 2,//垂直位置
        HorizenLength = 3,//水平长度
        VerticalLength = 4,//垂直长度
        PixelCount = 5,//像素计数
        Contour = 6,//轮廓查询
        EdgeDetection = 7,//边缘检测
    }

    //烟支检测类型
    public enum TobaccoType
    {
        Tobacco = 0,//烟丝侧
        Filter = 1,//滤嘴侧
    }

    //滤嘴类型
    public enum FilterType
    {
        General = 0,//普通滤嘴
        Special = 1,//特殊滤嘴
    }

    //滤嘴分割方法
    public enum DivideType
    {
        Single = 0,//单色分割
        R_B = 1,//红蓝之差
        All = 2,//两种都启用
    }

    //乱烟类型
    public enum DisorderType
    {
        Reverse = 0,//反支
        Hole = 1,//大空洞
    }
    
    //投影曲线类型
    public enum ProjectionType
    {
        Horizen = 0,//水平投影曲线
        Vertical = 1,//垂直投影曲线
    }

    //相机旋转角度
    public enum CameraFlip
    {
        Flip_H = 1,//左右镜像
        Flip_V = 2,//上下镜像
    }

    //相机旋转角度
    public enum CameraRotateAngle
    {
        Angle_0 = 0,//0度
        Angle_90 = 1,//1度
        Angle_180 = 2,//2度
        Angle_270 = 3,//3度
    }

    //烟支排列类型
    public enum TobaccoSortType
    {
        _767 = 1,//767
        _776 = 2,//776
        _1010 = 3,//1010
        _55 = 4,//55
        _88 = 5,//88
        _99 = 6,//99
        _66 = 7,//66
        _77 = 8,//77
        _11 = 9,//2
    }

    public enum VideoColor
    {
        RGB32 = 0,
        RGB24 = 1,
    }

    public enum VideoResolution
    {
        _744x480 = 0,
    }

    public enum RelevancyType
    {
        None = 0,
        Inner = 1,
        Extra = 2,
    }

    public enum SerialPortBaudRate
    {
        _57600 = 0,
        _115200 = 1,
        _230400 = 2,
        _2000000 = 3,
    }

    public enum SensorProductType
    {
        _89713FC = 0,
        _89713FA = 1,
        _89750A = 2,
        _89713CF = 3,
    }

    public enum Detect_Type_S
    {
        Average = 1,
        Edge = 2,
        Similarity = 3,
    }

    public enum ArithmeticType
    {
        Grid = 0,
        Quality = 1,
        Ruler = 2,
        Tobacco = 3,
        Disorder = 4,
        Line = 5,
        CurveDispersion = 6,
        Tobacco_D = 7,
        BaleLoosing = 8,
        LocationCicle = 9,
        LocationTemplateMatch = 10,
        Classify = 11,
    }

    public enum ROIType
    {
        Rectangle = 0,
        Ellipse = 1,
        Quadrangle = 2,
    }

    public enum Backend
    {
        Default = 0,
        Halide = 1,
        InferenceEngine = 2,
        OpenCV = 3,
        VkCom = 4,
        Cuda = 5,
        InferenceEngineNgraph = 1000000,
        InferenceEngineNnBuilder2019 = 1000001
    }

    public enum Target
    {
        Cpu = 0,
        OpenCL = 1,
        OpenCLFp16 = 2,
        Myriad = 3,
        Vulkan = 4,
        FPGA = 5,
        Cuda = 6,
        CudaFp16 = 7
    }
}