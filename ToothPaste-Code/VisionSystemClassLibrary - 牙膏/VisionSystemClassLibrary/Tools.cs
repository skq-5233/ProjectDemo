/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：Tools.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：工具

原作者：视觉检测团队
完成日期：2014/08/18
特别说明：经视觉检测团队授权并遵守代码使用条款，方可使用本代码并获得技术支持，否则所产生的一切后果由使用者承担

修改者：无
完成日期：无
修改记录：无

****************************************************************/

using System;
using System.IO;
using System.Drawing;

using Emgu.CV;
using Emgu.CV.Structure;

namespace VisionSystemClassLibrary.Class
{
    [Serializable]
    public class Tools
    {
        private string sDataPath = "";//相机数据路径（如，D:\\VisionSystemUserInterface\\ConfigData\\Top\\）

        //

        private Int16 iTolerancesIndex = 0;//公差索引值（公差数组序号，取值范围：取值为-1，表示该工具没有对应公差；否则从0开始取值）

        private string sName = "";//工具中文名称

        private Enum.ArithmeticType aType;//工具类型。取值范围：0为格子，1为质量，2为直尺，3为烟丝滤嘴检测，4为乱烟

        private string sToolsCHNName;//工具中文名称（用于配制文件程序）
        private string sToolsENGName;//工具英文名称（用于配制文件程序）

        private Boolean bToolState = true;//工具数值。取值范围：true：使能；false：禁止

        private Struct.ROI rROI = new Struct.ROI();
        private Struct.Arithmetic arithmetic = new Struct.Arithmetic();//算法结构体

        private Int32 iMin;
        private Int32 iMax;

        private Grid grid;//创建格子对象
        private Quality quality;//创建质量对象
        private Ruler ruler;//创建直尺对象
        private Tobacco tobacco;//创建烟丝滤嘴检测
        private Disorder disorder;//创建烟支检测
        private Line line;//创建拉线检测
        private CurveDispersion curvedispersion;//创建曲线离散度检测
        private Tobacco_D tobacco_D;//创建光电烟支检测
        private BaleLoosing baleLoosing;//创建散包检测
        private Location_Cicle locationCicle;//创建圆定位
        private Location_TemplateMatch locationTemplateMatch;//创建模板匹配定位
        private Classify classify;//创建分类

        private string sToolsUnit = "";//工具单位，例如：（pixel）

        private Boolean bLearnShowState;//自学习按钮显示状态,true:显示，false:隐藏
        private Boolean bExistTolerance;//存在公差与否,true:存在，false:不存在

        //以下学习数值仅对于包含于曲线图中的工具有效

        private Boolean bLearned = false;//是否经过学习。true：是；false：否
        private Int32 iLearnedValue = 100;//学习数值
        private Int32 iValidValue = 100;//学习中的有效数值数量
        private Int32 iNonvalidValue = 0;//学习中的无效数值数量

        private Int32[] iValue;//图像处理得到的结果值
        private Int32[] iSampleValue;//学习图像处理得到的结果值

        //数值范围

        private Int32 iMaxValue = 100000;//工具在对应的曲线图Y坐标轴上所能设置的最大值（若坐标轴数据形式为包含有效数值，则该值指的是有效数值最大值；否则指的是坐标轴最大值）
        private Int32 iMinValue = -1000;//工具在对应的曲线图Y坐标轴上所能设置的最小值（若坐标轴数据形式为包含有效数值，则该值指的是有效数值最小值；否则指的是坐标轴最小值）

        //

        private Boolean bEffectiveMin_State;//坐标轴最小有效实际数值是否开启
        private Boolean bEffectiveMax_State;//坐标轴最大有效实际数值是否开启

        private Boolean bReferenceH_Exist;//是否存在水平参考基准,true:存在,false:不存在
        private Boolean bReferenceV_Exist;//是否存在垂直参考基准,true:存在,false:不存在

        private Int32 iCompensation_H;//水平方向抖动补偿值
        private Int32 iCompensation_V;//垂直方向抖动补偿值
        
        private Int32 iReferenceHorizenPoint;//基准定位点水平基准坐标
        private Int32 iReferenceVerticalPoint;//基准定位点垂直基准标准

        //

        private Boolean bFilterCheck;//滤嘴检测,true:读取滤嘴检测参数，false:不读取

        private float fPrecision;//斜率
        private float fPrecisionInner;//内轮廓斜率

        private Int16 iEjectPixelMax;//最大剔除像素值
        private Int16 iEjectPixelMin;//最小剔除像素值
        private Int16 EjectPixelMinInner;//内部最小剔除像素值
        private Int16 iEjectLevel;//灵敏度
        
        private Enum.TobaccoType tTobaccoType = new Enum.TobaccoType();//检测类型,0:烟丝侧，1：滤嘴侧
        private Enum.FilterType fFilterType = new Enum.FilterType();//滤嘴类型，0：普通滤嘴，1：特殊滤嘴

        private Boolean bFOCKECheck;//FOCKE烟丝滤嘴检测，true:FOCKE，false:G.D

        private Enum.DisorderType dDisorderType = new Enum.DisorderType();//乱烟检测类型,0:反支，1：大空洞
        private Boolean bDisorderCheck;//乱烟检测,true:读取乱烟检测参数，false:不读取

        private float fPixelPerMm;//每毫米像素数

        //

        private Boolean bCheckTobaccoState;//内衬纸用于判断是否空模盒标志，true:判断，false:不判断

        private Boolean bLineCheck;//拉线检测
        private UInt16 uiSamplePos;//拉线模板位置
        private UInt16 uiLineWidth;//拉线宽度
        
        private Enum.SensorProductType sSensor_ProductType;
        private Byte bSensorNumber;//传感器数量

        private UInt16 uiImageWidth = 744; //图像宽度
        private UInt16 uiImageHeight = 480;//图像高度

        private Boolean bDetect_89713FA;//执行复合型检测

        private Boolean bDeepLearningState;//深度学习标记

        //

        //构造函数

        //-----------------------------------------------------------------------
        // 功能说明：构造函数（默认），初始化
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Tools()
        {
            iValue = new Int32[2];
            iSampleValue = new Int32[2];

            _ArithmeticInit();
        }

        //-----------------------------------------------------------------------
        // 功能说明：构造函数，初始化，读取文件数据
        // 输入参数：1.iToolIndex:工具索引
        //         2.sFilePath：文件数据路径
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Tools(Int32 iToolIndex, string sPath)
        {
            iValue = new Int32[2];
            iSampleValue = new Int32[2];
                        
            FileStream filestream = new FileStream(sPath + Camera.ToolFileName, FileMode.Open);
            BinaryReader binaryreader = new BinaryReader(filestream);

            filestream.Seek((0x10 + iToolIndex), SeekOrigin.Begin);
            bToolState = binaryreader.ReadBoolean();
            
            filestream.Seek((0x70 + iToolIndex * 0x02), SeekOrigin.Begin);
            iTolerancesIndex = binaryreader.ReadInt16();//公差索引值（公差数组序号，取值范围：取值为-1，表示该工具没有对应公差；否则从0开始取值）

            //读取拉线检测参数
            filestream.Seek((0xC0), SeekOrigin.Begin);
            bLineCheck = binaryreader.ReadBoolean();//读取拉线检测状态
            if (bLineCheck)
            {
                uiLineWidth = binaryreader.ReadUInt16();//读取拉线宽度 
            }

            //读取烟支检测参数
            filestream.Seek((0xD0), SeekOrigin.Begin);
            bFilterCheck = binaryreader.ReadBoolean();//读取烟丝滤嘴检测状态          
            if (bFilterCheck)
            {
                filestream.Seek((0xD1 + iToolIndex * 0x1000), SeekOrigin.Begin);
                tTobaccoType = (Enum.TobaccoType)binaryreader.ReadByte();//读取检测类型，0：烟丝，1：滤嘴
                fFilterType = (Enum.FilterType)binaryreader.ReadByte();//读取滤嘴类型,0:普通，1：特殊
                bFOCKECheck = binaryreader.ReadBoolean();//读取FOCKE机型检测，0:G.D，1:FOCKE

                filestream.Seek((0xE0 + iToolIndex * 0x1000), SeekOrigin.Begin);
                iEjectPixelMin = binaryreader.ReadInt16();//读取最小剔除像素值
                EjectPixelMinInner = binaryreader.ReadInt16();//读取内部最小剔除像素值
                iEjectPixelMax = binaryreader.ReadInt16();//读取最大剔除像素数

                filestream.Seek((0xF0 + iToolIndex * 0x1000), SeekOrigin.Begin);
                fPrecision = binaryreader.ReadSingle();//读取斜率
                fPrecisionInner = binaryreader.ReadSingle();//读取内部斜率
            }

            //读取乱烟检测参数
            filestream.Seek((0xD8), SeekOrigin.Begin);
            bDisorderCheck = binaryreader.ReadBoolean();//读取乱烟检测状态  

            if (bDisorderCheck)
            {
                filestream.Seek((0xD9 + iToolIndex * 0x1000), SeekOrigin.Begin);
                dDisorderType = (Enum.DisorderType)binaryreader.ReadByte();//读取乱烟检测类型，0：反支，1：大空洞
            }

            filestream.Seek((0x100 + iToolIndex * 0x1000), SeekOrigin.Begin);
            sToolsENGName = binaryreader.ReadString();
            filestream.Seek((0x120 + iToolIndex * 0x1000), SeekOrigin.Begin);
            sToolsCHNName = binaryreader.ReadString();

            _SetLanguage();//设置工具名称

            filestream.Seek((0x140 + iToolIndex * 0x1000), SeekOrigin.Begin);
            aType = (Enum.ArithmeticType)binaryreader.ReadByte();
            rROI.roiExtra.roiType = (Enum.ROIType)binaryreader.ReadByte();
            rROI.roiInnerExit = binaryreader.ReadBoolean();
            rROI.roiInner.roiType = (Enum.ROIType)binaryreader.ReadByte();

            filestream.Seek((0x148 + iToolIndex * 0x1000), SeekOrigin.Begin);
            rROI.roiExtra.Point3.X = binaryreader.ReadUInt16();
            rROI.roiExtra.Point3.Y = binaryreader.ReadUInt16();
            rROI.roiExtra.Point4.X = binaryreader.ReadUInt16();
            rROI.roiExtra.Point4.Y = binaryreader.ReadUInt16();
            rROI.roiExtra.Point1.X = binaryreader.ReadUInt16();
            rROI.roiExtra.Point1.Y = binaryreader.ReadUInt16();
            rROI.roiExtra.Point2.X = binaryreader.ReadUInt16();
            rROI.roiExtra.Point2.Y = binaryreader.ReadUInt16();

            filestream.Seek((0x15C + iToolIndex * 0x1000), SeekOrigin.Begin);
            bCheckTobaccoState = binaryreader.ReadBoolean();

            if (rROI.roiInnerExit) //存在内部屏蔽区域
            {
                filestream.Seek((0x160 + iToolIndex * 0x1000), SeekOrigin.Begin);
                rROI.roiInner.Point1.X = binaryreader.ReadUInt16();
                rROI.roiInner.Point1.Y = binaryreader.ReadUInt16();
                rROI.roiInner.Point2.X = binaryreader.ReadUInt16();
                rROI.roiInner.Point2.Y = binaryreader.ReadUInt16();
                rROI.roiInner.Point3.X = binaryreader.ReadUInt16();
                rROI.roiInner.Point3.Y = binaryreader.ReadUInt16();
                rROI.roiInner.Point4.X = binaryreader.ReadUInt16();
                rROI.roiInner.Point4.Y = binaryreader.ReadUInt16();
            }

            filestream.Seek((0x170 + iToolIndex * 0x1000), SeekOrigin.Begin);
            arithmetic.Number = binaryreader.ReadByte();

            _ArithmeticInit();

            filestream.Seek((0x180 + iToolIndex * 0x1000), SeekOrigin.Begin);
            for (int k = 0; k < arithmetic.Number; k++)
            {
                arithmetic.State[k] = binaryreader.ReadBoolean();
            }

            filestream.Seek((0x190 + iToolIndex * 0x1000), SeekOrigin.Begin);
            sToolsUnit = binaryreader.ReadString();
            filestream.Seek((0x1B0 + iToolIndex * 0x1000), SeekOrigin.Begin);
            bLearnShowState = binaryreader.ReadBoolean();
            filestream.Seek((0x1B4 + iToolIndex * 0x1000), SeekOrigin.Begin);
            bExistTolerance = binaryreader.ReadBoolean();

            filestream.Seek((0x1C0 + iToolIndex * 0x1000), SeekOrigin.Begin);
            bReferenceH_Exist = binaryreader.ReadBoolean();//读取水平参考基准状态
            bReferenceV_Exist = binaryreader.ReadBoolean();//读取垂直参考基准状态

            for (int j = 0; j < arithmetic.Number; j++)
            {
                filestream.Seek((0x200 + j * 0x100 + iToolIndex * 0x1000), SeekOrigin.Begin);
                arithmetic.ENGName[j] = binaryreader.ReadString();
                filestream.Seek((0x210 + j * 0x100 + iToolIndex * 0x1000), SeekOrigin.Begin);
                arithmetic.CHNName[j] = binaryreader.ReadString();
                filestream.Seek((0x220 + j * 0x100 + iToolIndex * 0x1000), SeekOrigin.Begin);
                arithmetic.Type[j] = binaryreader.ReadByte();

                if (arithmetic.Type[j] == 1)
                {
                    filestream.Seek((0x221 + j * 0x100 + iToolIndex * 0x1000), SeekOrigin.Begin);
                    arithmetic.EnumType[j] = binaryreader.ReadByte();
                    arithmetic.EnumNumber[j] = binaryreader.ReadByte();
                    arithmetic.EnumCurrent[j] = binaryreader.ReadByte();

                    filestream.Seek((0x230 + j * 0x100 + iToolIndex * 0x1000), SeekOrigin.Begin);
                    for (int p = 0; p < arithmetic.EnumNumber[j]; p++)
                    {
                        arithmetic.EnumState[j, p] = binaryreader.ReadBoolean();
                    }
                }
                else if (arithmetic.Type[j] == 2)
                {
                    filestream.Seek((0x221 + j * 0x100 + iToolIndex * 0x1000), SeekOrigin.Begin);
                    arithmetic.MinValue[j] = binaryreader.ReadInt16();
                    arithmetic.MaxValue[j] = binaryreader.ReadInt16();
                    arithmetic.CurrentValue[j] = binaryreader.ReadInt16();
                }
            }

            binaryreader.Close();
            filestream.Close();
            
            //

            switch (aType)
            {
                case Enum.ArithmeticType.Grid:
                    grid = new Grid();//创建格子对象
                    break;
                case Enum.ArithmeticType.Quality:
                    quality = new Quality();//创建质量对象
                    break;
                case Enum.ArithmeticType.Ruler:
                    ruler = new Ruler();//创建直尺对象
                    break;
                case Enum.ArithmeticType.Tobacco:
                    tobacco = new Tobacco();//创建烟丝滤嘴检测
                    break;
                case Enum.ArithmeticType.Disorder:
                    disorder = new Disorder();//创建烟支检测
                    break;
                case Enum.ArithmeticType.Line:
                    line = new Line();//创建拉线检测
                    break;
                case Enum.ArithmeticType.CurveDispersion:
                    curvedispersion = new CurveDispersion();//创建曲线离散度检测
                    break;
                case Enum.ArithmeticType.Tobacco_D:
                    tobacco_D = new Tobacco_D();//创建光电烟支检测
                    break;
                case Enum.ArithmeticType.BaleLoosing:
                    baleLoosing = new BaleLoosing();//创建散包检测
                    baleLoosing.Sensor_ProductType = sSensor_ProductType;
                    baleLoosing.SensorNumber = bSensorNumber;
                    break;
                case Enum.ArithmeticType.LocationCicle:
                    locationCicle = new Location_Cicle();//创建圆定位检测
                    break;
                case Enum.ArithmeticType.LocationTemplateMatch:
                    locationTemplateMatch = new Location_TemplateMatch();//创建模板匹配定位检测
                    break;
                case Enum.ArithmeticType.Classify:
                    classify = new Classify();
                    break;
                default:
                    break;
            }

            //

            _Init(true);//初始化


            switch (aType)
            {
                case Enum.ArithmeticType.Classify:
                    try
                    {
                        classify._NetInit(sPath + Camera.ClassesFile, sPath + Camera.ModelFileName);//初始化网络和类型
                    }
                    catch (Exception ex)//深度学习路径全称不能含中文，否则异常；为配置工具软件添加异常处理
                    {

                    }
                    bDeepLearningState = true;
                    break;
                default:
                    break;
            }
        }

        //属性

        // 功能说明：Detect_89713FA属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Boolean Detect_89713FA
        {
            get
            {
                return bDetect_89713FA;
            }
        }

        // 功能说明：CheckTobaccoState属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Boolean CheckTobaccoState
        {
            get
            {
                return bCheckTobaccoState;
            }
            set
            {
                bCheckTobaccoState = value;
            }
        }

        // 功能说明：LineCheck属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Boolean LineCheck
        {
            get
            {
                return bLineCheck;
            }
            set
            {
                bLineCheck = value;
            }
        }
        
        // 功能说明：SamplePos属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public UInt16 SamplePos
        {
            get
            {
                return uiSamplePos;
            }
            set
            {
                uiSamplePos = value;
            }
        }

        // 功能说明：LineWidth属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public UInt16 LineWidth
        {
            get
            {
                return uiLineWidth;
            }
            set
            {
                uiLineWidth = value;
            }
        }
        
        // 功能说明：Sensor_ProductType属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Enum.SensorProductType Sensor_ProductType
        {
            set
            {
                sSensor_ProductType = value;
            }
        }

        // 功能说明：SensorNumber属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Byte SensorNumber
        {
            set
            {
                bSensorNumber = value;
            }
        }

        // 功能说明：TobaccoType属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Enum.TobaccoType TobaccoType
        {
            get//读取
            {
                return tTobaccoType;
            }
            set
            {
                tTobaccoType = value;
            }
        }

        // 功能说明：FilterType属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Enum.FilterType FilterType
        {
            get//读取
            {
                return fFilterType;
            }
            set
            {
                fFilterType = value;
            }
        }

        // 功能说明：DisorderType属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Enum.DisorderType DisorderType
        {
            get//读取
            {
                return dDisorderType;
            }
            set
            {
                dDisorderType = value;
            }
        }

        // 功能说明：FOCKECheck属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Boolean FOCKECheck
        {
            get//读取
            {
                return bFOCKECheck;
            }
            set
            {
                bFOCKECheck = value;
            }
        }

        // 功能说明：DisorderCheck属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Boolean DisorderCheck
        {
            get//读取
            {
                return bDisorderCheck;
            }
            set
            {
                bDisorderCheck = value;
            }
        }

        // 功能说明：PixelPerMm属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public float PixelPerMm
        {
            set
            {
                fPixelPerMm = value;
            }
        }

        // 功能说明：Precision属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public float Precision
        {
            get//读取
            {
                return fPrecision;
            }
            set
            {
                fPrecision = value;
            }
        }

        // 功能说明：EjectLevel属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Int16 EjectLevel
        {
            get//读取
            {
                return iEjectLevel;
            }
            set
            {
                iEjectLevel = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：FilterCheck属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Boolean FilterCheck
        {
            get//读取
            {
                return bFilterCheck;
            }
            set
            {
                bFilterCheck = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ReferenceHorizenPoint属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Int32 ReferenceHorizenPoint
        {
            set
            {
                iReferenceHorizenPoint = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ReferenceVerticalPoint属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Int32 ReferenceVerticalPoint
        {
            set
            {
                iReferenceVerticalPoint = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ReferenceH_Exist属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Boolean ReferenceH_Exist
        {
            get//读取
            {
                return bReferenceH_Exist;
            }
            set
            {
                bReferenceH_Exist = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ReferenceV_Exist属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Boolean ReferenceV_Exist
        {
            get//读取
            {
                return bReferenceV_Exist;
            }
            set
            {
                bReferenceV_Exist = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Compensation_H属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Int32 Compensation_H
        {
            get//读取
            {
                return iCompensation_H;
            }
            set
            {
                iCompensation_H = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：LearnedValue属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Int32 Compensation_V
        {
            get//读取
            {
                return iCompensation_V;
            }
            set
            {
                iCompensation_V = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Learned属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Boolean Learned
        {
            get//读取
            {
                return bLearned;
            }
            set
            {
                bLearned = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：LearnedValue属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Int32 LearnedValue
        {
            get//读取
            {
                return iLearnedValue;
            }
            set
            {
                iLearnedValue = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ValidValue属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Int32 ValidValue
        {
            get//读取
            {
                return iValidValue;
            }
            set
            {
                iValidValue = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：iNonvalidValue属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Int32 NonvalidValue
        {
            get//读取
            {
                return iNonvalidValue;
            }
            set
            {
                iNonvalidValue = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Value属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Int32[] Value
        {
            get//读取
            {
                return iValue;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：iNonvalidValue属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Int32[] SampleValue
        {
            get//读取
            {
                return iSampleValue;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：MinValue属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Int32 MinValue
        {
            get//读取
            {
                return iMinValue;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：MaxValue属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Int32 MaxValue
        {
            get//读取
            {
                return iMaxValue;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ToolsUnit属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public string ToolsUnit
        {
            get//读取
            {
                return sToolsUnit;
            }
            set
            {
                sToolsUnit = value;
            }
        }
        
        //-----------------------------------------------------------------------
        // 功能说明：LearnShowState属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Boolean LearnShowState
        {
            get//读取
            {
                return bLearnShowState;
            }
            set
            {
                bLearnShowState = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ExistTolerance属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Boolean ExistTolerance
        {
            get//读取
            {
                return bExistTolerance;
            }
            set
            {
                bExistTolerance = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Min属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Int32 Min
        {
            get//读取
            {
                return iMin;
            }
            set
            {
                iMin = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Max属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Int32 Max
        {
            get//读取
            {
                return iMax;
            }
            set
            {
                iMax = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ROI属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Struct.ROI ROI
        {
            get//读取
            {
                return rROI;
            }
            set
            {
                rROI = value;
            }
        }

        // 功能说明：Type属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Enum.ArithmeticType Type
        {
            get//读取
            {
                return aType;
            }
            set
            {
                aType = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ToolsCHNName属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public string ToolsCHNName
        {
            get//读取
            {
                return sToolsCHNName;
            }
            set
            {
                sToolsCHNName = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ToolsENGName属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public string ToolsENGName
        {
            get//读取
            {
                return sToolsENGName;
            }
            set
            {
                sToolsENGName = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ToolState属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Boolean ToolState
        {
            get//读取
            {
                return bToolState;
            }
            set
            {
                bToolState = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：DataPath属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public string DataPath
        {
            get//读取
            {
                return sDataPath;
            }
            set
            {
                sDataPath = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Name属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public string Name
        {
            get//读取
            {
                return sName;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：TolerancesIndex属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Int16 TolerancesIndex
        {
            get//读取
            {
                return iTolerancesIndex;
            }
            set
            {
                iTolerancesIndex = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ImageWidth属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public UInt16 ImageWidth
        {
            get//读取
            {
                return uiImageWidth;
            }
            set
            {
                uiImageWidth = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ImageHeight属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public UInt16 ImageHeight
        {
            get//读取
            {
                return uiImageHeight;
            }
            set
            {
                uiImageHeight = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Ruler属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Ruler Ruler
        {
            get
            {
                return ruler;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Arithmetic属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Struct.Arithmetic Arithmetic
        {
            get
            {
                return arithmetic;
            }
            set
            {
                arithmetic = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：EjectPixelMin属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Int16 EjectPixelMin
        {
            get
            {
                return iEjectPixelMin;
            }
            set
            {
                iEjectPixelMin = value;

                switch (sSensor_ProductType)
                {
                    case Enum.SensorProductType._89713FC:
                        tobacco_D.EjectPixelMin = iEjectPixelMin;
                        break;
                    case Enum.SensorProductType._89713FA:
                        baleLoosing.EjectPixelMin = iEjectPixelMin;
                        break;
                    default:
                        break;
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：EjectPixelMax属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Int16 EjectPixelMax
        {
            get
            {
                return iEjectPixelMax;
            }
            set
            {
                iEjectPixelMax = value;

                switch (sSensor_ProductType)
                {
                    case Enum.SensorProductType._89713FC:
                        tobacco_D.EjectPixelMax = iEjectPixelMax;
                        break;
                    case Enum.SensorProductType._89713FA:
                        baleLoosing.EjectPixelMax = iEjectPixelMax;
                        break;
                    default:
                        break;
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：EffectiveMin_State属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Boolean EffectiveMin_State
        {
            get
            {
                return bEffectiveMin_State;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：EffectiveMin_State属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Boolean EffectiveMax_State
        {
            get
            {
                return bEffectiveMax_State;
            }
        }

        /// <summary>
        /// 深度学习标记
        /// </summary>
        public Boolean DeepLearningState
        {
            get
            {
                return bDeepLearningState;
            }
        }
        
        //函数

        //-----------------------------------------------------------------------
        // 功能说明：设置工具名称（UI）
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _SetLanguage()
        {
            switch (System.Language)
            {
                case Enum.InterfaceLanguage.Chinese:

                    sName = sToolsCHNName;

                    break;
                case Enum.InterfaceLanguage.English:

                    sName = sToolsENGName;

                    break;
                default:

                    sName = sToolsCHNName;

                    break;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：保存函数，提供配置工具使用
        // 输入参数：1.iToolIndex：工具索引
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _SaveReload(Int32 iToolIndex)
        {
            Int32 iIndex = 0;//循环控制变量

            Byte[] byteSelect = new Byte[arithmetic.Number];

            for (Byte byteIndex = 0; byteIndex < arithmetic.Number; byteIndex++)
            {
                byteSelect[iIndex] = byteIndex;

                iIndex++;
            }

            //

            FileStream filestream = new FileStream(sDataPath + Camera.ToolFileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, 8192, FileOptions.WriteThrough);
            BinaryWriter binarywriter = new BinaryWriter(filestream);

            filestream.Seek(0x10 + iToolIndex, SeekOrigin.Begin);
            binarywriter.Write(bToolState);
            
            filestream.Seek((0x70 + iToolIndex * 0x02), SeekOrigin.Begin);
            binarywriter.Write(iTolerancesIndex);//公差索引值（公差数组序号，取值范围：取值为-1，表示该工具没有对应公差；否则从0开始取值）

            //保存拉线检测参数
            filestream.Seek(0xC0, SeekOrigin.Begin);
            binarywriter.Write(bLineCheck);//拉线检测状态
            if (bLineCheck)
            {
                binarywriter.Write(uiLineWidth);//拉线宽度
            }
            
            filestream.Seek((0xD0), SeekOrigin.Begin);
            binarywriter.Write(bFilterCheck);//保存烟丝滤嘴检测状态

            //保存烟支检测参数
            if (bFilterCheck)
            {
                filestream.Seek((0xD1 + iToolIndex * 0x1000), SeekOrigin.Begin);
                binarywriter.Write((Byte)tTobaccoType);//保存检测类型，0：烟丝，1：滤嘴
                binarywriter.Write((Byte)fFilterType);//保存滤嘴类型,0:普通，1：特殊
                binarywriter.Write(bFOCKECheck);//保存FOCKE机型检测，0:G.D，1:FOCKE

                filestream.Seek((0xE0 + iToolIndex * 0x1000), SeekOrigin.Begin);
                binarywriter.Write(iEjectPixelMin);//保存最小剔除像素值
                binarywriter.Write(EjectPixelMinInner);//保存内部最小剔除像素值
                binarywriter.Write(iEjectPixelMax);//保存最大像素数

                filestream.Seek((0xF0 + iToolIndex * 0x1000), SeekOrigin.Begin);
                binarywriter.Write(fPrecision);//保存斜率
                binarywriter.Write(fPrecisionInner);//保存内部斜率
            }
            //

            filestream.Seek((0xD8), SeekOrigin.Begin);
            binarywriter.Write(bDisorderCheck);//保存乱烟检测状态

            if (bDisorderCheck)
            {
                filestream.Seek((0xD9 + iToolIndex * 0x1000), SeekOrigin.Begin);
                binarywriter.Write((Byte)dDisorderType);//保存乱烟检测类型，0：反支，1：大空洞
            }

            filestream.Seek(0x100 + iToolIndex * 0x1000, SeekOrigin.Begin);
            binarywriter.Write(sToolsENGName);
            filestream.Seek(0x120 + iToolIndex * 0x1000, SeekOrigin.Begin);
            binarywriter.Write(sToolsCHNName);
            filestream.Seek(0x140 + iToolIndex * 0x1000, SeekOrigin.Begin);
            binarywriter.Write((Byte)aType);
            binarywriter.Write((Byte)rROI.roiExtra.roiType);
            binarywriter.Write(rROI.roiInnerExit);
            binarywriter.Write((Byte)rROI.roiInner.roiType);

            filestream.Seek((0x148 + iToolIndex * 0x1000), SeekOrigin.Begin);
            binarywriter.Write((UInt16)ROI.roiExtra.Point3.X);
            binarywriter.Write((UInt16)ROI.roiExtra.Point3.Y);
            binarywriter.Write((UInt16)ROI.roiExtra.Point4.X);
            binarywriter.Write((UInt16)ROI.roiExtra.Point4.Y);
            binarywriter.Write((UInt16)ROI.roiExtra.Point1.X);
            binarywriter.Write((UInt16)ROI.roiExtra.Point1.Y);
            binarywriter.Write((UInt16)ROI.roiExtra.Point2.X);
            binarywriter.Write((UInt16)ROI.roiExtra.Point2.Y);

            filestream.Seek((0x15C + iToolIndex * 0x1000), SeekOrigin.Begin);
            binarywriter.Write(bCheckTobaccoState);

            if (rROI.roiInnerExit) //存在内部屏蔽区域
            {
                filestream.Seek(0x160 + iToolIndex * 0x1000, SeekOrigin.Begin);
                binarywriter.Write((UInt16)ROI.roiInner.Point1.X);
                binarywriter.Write((UInt16)ROI.roiInner.Point1.Y);
                binarywriter.Write((UInt16)ROI.roiInner.Point2.X);
                binarywriter.Write((UInt16)ROI.roiInner.Point2.Y);
                binarywriter.Write((UInt16)ROI.roiInner.Point3.X);
                binarywriter.Write((UInt16)ROI.roiInner.Point3.Y);
                binarywriter.Write((UInt16)ROI.roiInner.Point4.X);
                binarywriter.Write((UInt16)ROI.roiInner.Point4.Y);
            }

            filestream.Seek(0x170 + iToolIndex * 0x1000, SeekOrigin.Begin);
            binarywriter.Write(arithmetic.Number);

            filestream.Seek(0x180 + iToolIndex * 0x1000, SeekOrigin.Begin);
            for (Int32 i = 0; i < arithmetic.Number; i++)
            {
                binarywriter.Write(arithmetic.State[i]);
            }

            filestream.Seek(0x190 + iToolIndex * 0x1000, SeekOrigin.Begin);
            binarywriter.Write(sToolsUnit);
            filestream.Seek((0x1B0 + iToolIndex * 0x1000), SeekOrigin.Begin);
            binarywriter.Write(bLearnShowState);
            filestream.Seek((0x1B4 + iToolIndex * 0x1000), SeekOrigin.Begin);
            binarywriter.Write(bExistTolerance);

            filestream.Seek((0x1C0 + iToolIndex * 0x1000), SeekOrigin.Begin);
            binarywriter.Write(bReferenceH_Exist);//保存水平参考基准属性状态
            binarywriter.Write(bReferenceV_Exist);//保存水平参考基准状态

            for (Int32 j = 0; j < iIndex; j++)
            {
                filestream.Seek(0x200 + byteSelect[j] * 0x100 + iToolIndex * 0x1000, SeekOrigin.Begin);
                binarywriter.Write(arithmetic.ENGName[byteSelect[j]]);
                filestream.Seek(0x210 + byteSelect[j] * 0x100 + iToolIndex * 0x1000, SeekOrigin.Begin);
                binarywriter.Write(arithmetic.CHNName[byteSelect[j]]);
                filestream.Seek(0x220 + byteSelect[j] * 0x100 + iToolIndex * 0x1000, SeekOrigin.Begin);
                binarywriter.Write(arithmetic.Type[byteSelect[j]]);

                if (arithmetic.Type[byteSelect[j]] == 1)
                {
                    filestream.Seek(0x221 + byteSelect[j] * 0x100 + iToolIndex * 0x1000, SeekOrigin.Begin);
                    binarywriter.Write(arithmetic.EnumType[byteSelect[j]]);
                    filestream.Seek(0x222 + byteSelect[j] * 0x100 + iToolIndex * 0x1000, SeekOrigin.Begin);
                    binarywriter.Write(arithmetic.EnumNumber[byteSelect[j]]);
                    filestream.Seek(0x223 + byteSelect[j] * 0x100 + iToolIndex * 0x1000, SeekOrigin.Begin);
                    binarywriter.Write(arithmetic.EnumCurrent[byteSelect[j]]);

                    filestream.Seek(0x230 + byteSelect[j] * 0x100 + iToolIndex * 0x1000, SeekOrigin.Begin);
                    for (int m = 0; m < arithmetic.EnumNumber[byteSelect[j]]; m++)
                    {
                        binarywriter.Write(arithmetic.EnumState[byteSelect[j], m]);
                    }
                }
                else if (arithmetic.Type[byteSelect[j]] == 2)
                {
                    filestream.Seek(0x221 + byteSelect[j] * 0x100 + iToolIndex * 0x1000, SeekOrigin.Begin);
                    binarywriter.Write(arithmetic.MinValue[byteSelect[j]]);
                    filestream.Seek(0x223 + byteSelect[j] * 0x100 + iToolIndex * 0x1000, SeekOrigin.Begin);
                    binarywriter.Write(arithmetic.MaxValue[byteSelect[j]]);
                    filestream.Seek(0x225 + byteSelect[j] * 0x100 + iToolIndex * 0x1000, SeekOrigin.Begin);
                    binarywriter.Write(arithmetic.CurrentValue[byteSelect[j]]);
                }
            }
            binarywriter.Close();
            filestream.Close();
        }

        //-----------------------------------------------------------------------
        // 功能说明：保存函数
        // 输入参数：1.iToolIndex：工具索引
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _Save(Int32 iToolIndex,ref Byte[] bData)
        {
            BitConverter.GetBytes(bToolState).CopyTo(bData,0x10 + iToolIndex);
                        
            //保存烟支检测参数
            if (bFilterCheck)
            {
                BitConverter.GetBytes(iEjectPixelMin).CopyTo(bData, 0xE0 + iToolIndex * 0x1000);
                BitConverter.GetBytes(EjectPixelMinInner).CopyTo(bData, 0xE2 + iToolIndex * 0x1000);
                BitConverter.GetBytes(iEjectPixelMax).CopyTo(bData, 0xE4 + iToolIndex * 0x1000);
                BitConverter.GetBytes(fPrecision).CopyTo(bData, 0xF0 + iToolIndex * 0x1000);
                BitConverter.GetBytes(fPrecisionInner).CopyTo(bData, 0xF4 + iToolIndex * 0x1000);
            }
            BitConverter.GetBytes((UInt16)rROI.roiExtra.Point3.X).CopyTo(bData, 0x148 + iToolIndex * 0x1000);
            BitConverter.GetBytes((UInt16)rROI.roiExtra.Point3.Y).CopyTo(bData, 0x14A + iToolIndex * 0x1000);
            BitConverter.GetBytes((UInt16)rROI.roiExtra.Point4.X).CopyTo(bData, 0x14C + iToolIndex * 0x1000);
            BitConverter.GetBytes((UInt16)rROI.roiExtra.Point4.Y).CopyTo(bData, 0x14E + iToolIndex * 0x1000);
            BitConverter.GetBytes((UInt16)rROI.roiExtra.Point1.X).CopyTo(bData, 0x150 + iToolIndex * 0x1000);
            BitConverter.GetBytes((UInt16)rROI.roiExtra.Point1.Y).CopyTo(bData, 0x152 + iToolIndex * 0x1000);
            BitConverter.GetBytes((UInt16)rROI.roiExtra.Point2.X).CopyTo(bData, 0x154 + iToolIndex * 0x1000);
            BitConverter.GetBytes((UInt16)rROI.roiExtra.Point2.Y).CopyTo(bData, 0x156 + iToolIndex * 0x1000);

            if (rROI.roiInnerExit) //存在内部屏蔽区域
            {
                BitConverter.GetBytes((UInt16)rROI.roiInner.Point1.X).CopyTo(bData, 0x160 + iToolIndex * 0x1000);
                BitConverter.GetBytes((UInt16)rROI.roiInner.Point1.Y).CopyTo(bData, 0x162 + iToolIndex * 0x1000);
                BitConverter.GetBytes((UInt16)rROI.roiInner.Point2.X).CopyTo(bData, 0x164 + iToolIndex * 0x1000);
                BitConverter.GetBytes((UInt16)rROI.roiInner.Point2.Y).CopyTo(bData, 0x166 + iToolIndex * 0x1000);
                BitConverter.GetBytes((UInt16)rROI.roiInner.Point3.X).CopyTo(bData, 0x168 + iToolIndex * 0x1000);
                BitConverter.GetBytes((UInt16)rROI.roiInner.Point3.Y).CopyTo(bData, 0x16A + iToolIndex * 0x1000);
                BitConverter.GetBytes((UInt16)rROI.roiInner.Point4.X).CopyTo(bData, 0x16C + iToolIndex * 0x1000);
                BitConverter.GetBytes((UInt16)rROI.roiInner.Point4.Y).CopyTo(bData, 0x16E + iToolIndex * 0x1000);
            }

            Int32 iIndex = 0;//循环控制变量

            Byte[] byteSelect = new Byte[arithmetic.Number];

            for (Byte byteIndex = 0; byteIndex < arithmetic.Number; byteIndex++)
            {
                byteSelect[iIndex] = byteIndex;

                iIndex++;
            }

            for (Int32 j = 0; j < iIndex; j++)
            {
                if (arithmetic.Type[byteSelect[j]] == 1)
                {
                    BitConverter.GetBytes(arithmetic.EnumCurrent[byteSelect[j]]).CopyTo(bData, 0x223 + byteSelect[j] * 0x100 + iToolIndex * 0x1000);
                }
                else if (arithmetic.Type[byteSelect[j]] == 2)
                {
                    BitConverter.GetBytes(arithmetic.CurrentValue[byteSelect[j]]).CopyTo(bData, 0x225 + byteSelect[j] * 0x100 + iToolIndex * 0x1000);
                }
            }
        }
        
        //-----------------------------------------------------------------------
        // 功能说明：工具类拷贝函数
        // 输入参数：1.Tools：tool，工具参数
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _CopyTo(ref Tools tool)
        {
            rROI._CopyTo(ref tool.rROI);
            arithmetic._CopyTo(ref tool.arithmetic);

            tool.bToolState = bToolState;
            tool.iTolerancesIndex = iTolerancesIndex;
            tool.sToolsENGName = sToolsENGName;
            tool.sToolsCHNName = sToolsCHNName;
            tool.aType = aType;
            tool.bCheckTobaccoState = bCheckTobaccoState;
            tool.sToolsUnit = sToolsUnit;
            tool.bLearnShowState = bLearnShowState;
            tool.bExistTolerance = bExistTolerance;

            tool.sName = sName;
            tool.iMin = iMin;
            tool.iMax = iMax;
            tool.bLearned = bLearned;
            tool.iLearnedValue = iLearnedValue;
            tool.iValidValue = iValidValue;
            tool.iNonvalidValue = iNonvalidValue;

            tool.iValue = new Int32[iValue.Length];
            Array.Copy(iValue, tool.iValue, iValue.Length);

            tool.iMaxValue = iMaxValue;
            tool.iMinValue = iMinValue;
            tool.sDataPath = sDataPath;

            tool.bReferenceH_Exist = bReferenceH_Exist;
            tool.bReferenceV_Exist = bReferenceV_Exist;
            tool.iCompensation_H = iCompensation_H;
            tool.iCompensation_V = iCompensation_V;
            
            switch (aType)
            {
                case Enum.ArithmeticType.Grid:
                    if (null == tool.grid)
                    {
                        tool.grid = new Grid();
                    }
                    grid._CopyTo(ref tool.grid);
                    break;
                case Enum.ArithmeticType.Quality:
                    if (null == tool.quality)
                    {
                        tool.quality = new Quality();
                    }
                    quality._CopyTo(ref tool.quality);
                    break;
                case Enum.ArithmeticType.Ruler:
                    if (null == tool.ruler)
                    {
                        tool.ruler = new Ruler();
                    }
                    ruler._CopyTo(ref tool.ruler);
                    break;
                case Enum.ArithmeticType.Tobacco:
                    if (null == tool.tobacco)
                    {
                        tool.tobacco = new Tobacco();
                    }
                    tobacco._CopyTo(ref tool.tobacco);
                    break;
                case Enum.ArithmeticType.Disorder:
                    if (null == tool.disorder)
                    {
                        tool.disorder = new Disorder();
                    }
                    disorder._CopyTo(ref tool.disorder);
                    break;
                case Enum.ArithmeticType.Line:
                    if (null == tool.line)
                    {
                        tool.line = new Line();
                    }
                    line._CopyTo(ref tool.line);
                    break;
                case Enum.ArithmeticType.CurveDispersion:
                    if (null == tool.curvedispersion)
                    {
                        tool.curvedispersion = new CurveDispersion();
                    }
                    curvedispersion._CopyTo(ref tool.curvedispersion);
                    break;
                case Enum.ArithmeticType.Tobacco_D:
                    if (null == tool.tobacco_D)
                    {
                        tool.tobacco_D = new Tobacco_D();
                    }
                    tobacco_D._CopyTo(ref tool.tobacco_D);
                    break;
                case Enum.ArithmeticType.BaleLoosing:
                    if (null == tool.baleLoosing)
                    {
                        tool.baleLoosing = new BaleLoosing();
                    }
                    baleLoosing._CopyTo(ref tool.baleLoosing);
                    break;
                case Enum.ArithmeticType.LocationCicle:
                    if (null == tool.locationCicle)
                    {
                        tool.locationCicle = new Location_Cicle();
                    }
                    locationCicle._CopyTo(ref tool.locationCicle);
                    break;
                case Enum.ArithmeticType.LocationTemplateMatch:
                    if (null == tool.locationTemplateMatch)
                    {
                        tool.locationTemplateMatch = new Location_TemplateMatch();
                    }
                    locationTemplateMatch._CopyTo(ref tool.locationTemplateMatch);
                    break;
                case Enum.ArithmeticType.Classify:
                    if (null == tool.classify)
                    {
                        tool.classify = new Classify();
                    }
                    classify._CopyTo(ref tool.classify);
                    break;
                default:
                    break;
            }

            tool.iSampleValue = new Int32[iSampleValue.Length];
            Array.Copy(iSampleValue, tool.iSampleValue, iSampleValue.Length);

            tool.bFilterCheck = bFilterCheck;

            tool.fPrecision = fPrecision;
            tool.fPrecisionInner = fPrecisionInner;

            tool.iEjectPixelMin = iEjectPixelMin;
            tool.EjectPixelMinInner = EjectPixelMinInner;
            tool.iEjectPixelMax = iEjectPixelMax;

            tool.EjectLevel = EjectLevel;

            tool.tTobaccoType = tTobaccoType;
            tool.fFilterType = fFilterType;
            tool.bFOCKECheck = bFOCKECheck;

            tool.PixelPerMm = fPixelPerMm;

            tool.iReferenceHorizenPoint = iReferenceHorizenPoint;
            tool.EjectPixelMinInner = EjectPixelMinInner;
            tool.iReferenceVerticalPoint = iReferenceVerticalPoint;

            tool.dDisorderType = dDisorderType;
            tool.bDisorderCheck = bDisorderCheck;
            
            tool.bLineCheck = bLineCheck;
            tool.uiSamplePos = uiSamplePos;
            tool.uiLineWidth = uiLineWidth;

            tool.bEffectiveMin_State = bEffectiveMin_State;
            tool.bEffectiveMax_State = bEffectiveMax_State;

            tool.sSensor_ProductType = sSensor_ProductType;
            tool.bSensorNumber = bSensorNumber;

            tool.uiImageWidth = uiImageWidth;
            tool.uiImageHeight = uiImageHeight;

            tool.bDeepLearningState = bDeepLearningState;
        }

        /// <summary>
        /// 更新区域信息（叠加偏移量）
        /// </summary>
        /// <param name="roi"></param>
        /// <returns></returns>
        private Struct.ROI _UpdateROI(Struct.ROI roi)
        {
            Struct.ROI roiTemp = new Struct.ROI();
            roi._CopyTo(ref roiTemp);

            Point point1 = new Point();
            Point point2 = new Point();
            Point point3 = new Point();
            Point point4 = new Point();

            switch (roi.roiExtra.roiType)
            {
                case Enum.ROIType.Ellipse:
                    point1.X = roi.roiExtra.Point1.X + iCompensation_H - roi.roiExtra.Point2.X;
                    point1.Y = roi.roiExtra.Point1.Y + iCompensation_V - roi.roiExtra.Point2.Y;
                    point2.X = roi.roiExtra.Point1.X + iCompensation_H + roi.roiExtra.Point2.X;
                    point2.Y = roi.roiExtra.Point1.Y + iCompensation_V + roi.roiExtra.Point2.Y;

                    if ((point1.X >= 0) && (point2.X >= 0) && (point1.X <= uiImageWidth) && (point2.X <= uiImageWidth)
                        && (point1.Y >= 0) && (point2.Y >= 0) && (point1.Y <= uiImageHeight) && (point2.Y <= uiImageHeight))
                    {
                        roiTemp._Offset(iCompensation_H, iCompensation_V);
                    }
                    break;
                case Enum.ROIType.Quadrangle:
                    point1 = roi.roiExtra.Point1;
                    point2 = roi.roiExtra.Point2;
                    point3 = roi.roiExtra.Point3;
                    point4 = roi.roiExtra.Point4;
                    point1.Offset(iCompensation_H, iCompensation_V);
                    point2.Offset(iCompensation_H, iCompensation_V);
                    point3.Offset(iCompensation_H, iCompensation_V);
                    point4.Offset(iCompensation_H, iCompensation_V);

                    if ((point1.X >= 0) && (point2.X >= 0) && (point3.X >= 0) && (point4.X >= 0)
                        && (point1.X <= uiImageWidth) && (point2.X <= uiImageWidth) && (point3.X <= uiImageWidth) && (point4.X <= uiImageWidth)
                        && (point1.Y >= 0) && (point2.Y >= 0) && (point3.Y >= 0) && (point4.Y >= 0)
                        && (point1.Y <= uiImageHeight) && (point2.Y <= uiImageHeight) && (point3.Y <= uiImageHeight) && (point4.Y <= uiImageHeight))
                    {
                        roiTemp._Offset(iCompensation_H, iCompensation_V);
                    }
                    break;
                default:
                    point1.X = roi.roiExtra.Point1.X + iCompensation_H;
                    point2.X = roi.roiExtra.Point1.X + roi.roiExtra.Point2.X + iCompensation_H;
                    point1.Y = roi.roiExtra.Point1.Y + iCompensation_V;
                    point2.Y = roi.roiExtra.Point1.Y + roi.roiExtra.Point2.Y + iCompensation_V;

                    if ((point1.X >= 0) && (point2.X >= 0) && (point1.X <= uiImageWidth) && (point2.X <= uiImageWidth)
                        && (point1.Y >= 0) && (point2.Y >= 0) && (point1.Y <= uiImageHeight) && (point2.Y <= uiImageHeight))
                    {
                        roiTemp._Offset(iCompensation_H, iCompensation_V);
                    }
                    break;
            }
            return roiTemp;
        }

        //-----------------------------------------------------------------------
        // 功能说明：给相应工具类型参数赋值
        // 输入参数： 1、Boolean：state，是否重新被初始化
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        private void _Init(Boolean state = false)
        {
            if (bReferenceH_Exist || bReferenceV_Exist)//如果是计算水平基准或垂直基准
            {
                iCompensation_H = 0;//水平补偿值清零
                iCompensation_V = 0;//垂直补偿值清零
            }

            switch (aType)
            {
                case Enum.ArithmeticType.Grid:
                    bEffectiveMin_State = true;
                    bEffectiveMax_State = true;

                    ///////////////////////////////////////////////////////////////
                    grid.Arithmetic = arithmetic;
                    grid.ROI = _UpdateROI(rROI);
                    /////////////////////////////////////////////////////////////////////////2021.03.12

                    if (state)
                    {
                        grid._Init(true);
                    }
                    else
                    {
                        grid._Init();
                    }

                    grid.Min = iMin;
                    grid.Max = iMax;
                    break;
                case Enum.ArithmeticType.Quality:
                    bEffectiveMin_State = true;
                    bEffectiveMax_State = true;

                    ///////////////////////////////////////////////////////////////
                    quality.Arithmetic = arithmetic;
                    quality.ROI = _UpdateROI(rROI);
                    /////////////////////////////////////////////////////////////////////////2021.03.12

                    if (state)
                    {
                        quality._Init(true);
                    }
                    else
                    {
                        quality._Init();
                    }

                    quality.Min = iMin;
                    quality.Max = iMax;
                    break;
                case Enum.ArithmeticType.Ruler:
                    bEffectiveMin_State = true;
                    bEffectiveMax_State = true;

                    ///////////////////////////////////////////////////////////////
                    ruler.Arithmetic = arithmetic;
                    ruler.ROI = _UpdateROI(rROI);
                    /////////////////////////////////////////////////////////////////////////2021.03.12

                    ruler.ReferenceH_Exist = bReferenceH_Exist;
                    ruler.ReferenceV_Exist = bReferenceV_Exist;

                    ruler._Init();

                    ruler.ExistTolerance = bExistTolerance;

                    ruler.Min = iMin;
                    ruler.Max = iMax;

                    if (!(bReferenceH_Exist || bReferenceV_Exist))//如果该工具为非水平或垂直基准，给相应参考基准赋值
                    {
                        ruler.ReferenceHorizenPoint = iReferenceHorizenPoint;
                        ruler.ReferenceVerticalPoint = iReferenceVerticalPoint;
                    }
                    break;
                case Enum.ArithmeticType.Tobacco:
                    ///////////////////////////////////////////////////////////////
                    tobacco.Arithmetic = arithmetic;
                    tobacco.ROI = _UpdateROI(rROI);
                    /////////////////////////////////////////////////////////////////////////2021.03.12

                    tobacco._Init();

                    tobacco.TobaccoType = tTobaccoType;//检测类型
                    tobacco.FilterType = fFilterType;//滤嘴类型
                    tobacco.FOCKECheck = bFOCKECheck;//FOCKE检测类型

                    if (Enum.TobaccoType.Filter == tTobaccoType)
                    {
                        bEffectiveMin_State = true;
                        bEffectiveMax_State = true;
                    }
                    else
                    {
                        bEffectiveMin_State = true;
                        bEffectiveMax_State = false;
                    }

                    tobacco.EjectLevel = iEjectLevel;//灵敏度赋值
                    tobacco.EjectPixel = iMin;//检测基准赋值
                    tobacco.EjectPixelMax = iMax;//检测基准赋值上限2017.02.08

                    tobacco.Precision = fPrecision;//斜率赋值
                    tobacco.PrecisionInner = fPrecisionInner;//内部斜率赋值

                    tobacco.EjectPixelMin = iEjectPixelMin;//最小剔除像素值
                    tobacco.EjectPixelMinInner = EjectPixelMinInner;//内部最小剔除像素值
                    break;

                case Enum.ArithmeticType.Disorder:
                    bEffectiveMin_State = false;
                    bEffectiveMax_State = true;

                    ///////////////////////////////////////////////////////////////
                    disorder.Arithmetic = arithmetic;
                    disorder.ROI = _UpdateROI(rROI);
                    /////////////////////////////////////////////////////////////////////////2021.03.12

                    disorder.Init();

                    disorder.DisorderType = dDisorderType;//乱烟检测类型

                    disorder.Max = iMax;
                    break;

                case Enum.ArithmeticType.Line:
                    bEffectiveMin_State = true;
                    bEffectiveMax_State = true;

                    ///////////////////////////////////////////////////////////////
                    line.Arithmetic = arithmetic;
                    line.ROI = rROI;
                    /////////////////////////////////////////////////////////////////////////2021.03.12

                    line.Max = iMax;
                    line.Min = iMin;

                    line.SamplePos = uiSamplePos;
                    line.LineWidth = uiLineWidth;

                    line._Init();
                    break;

                case Enum.ArithmeticType.CurveDispersion:
                    bEffectiveMin_State = true;
                    bEffectiveMax_State = true;

                    ///////////////////////////////////////////////////////////////
                    curvedispersion.Arithmetic = arithmetic;
                    curvedispersion.ROI = _UpdateROI(rROI);
                    /////////////////////////////////////////////////////////////////////////2021.03.12

                    curvedispersion._Init();

                    curvedispersion.Min = iMin;
                    curvedispersion.Max = iMax;
                    break;

                case Enum.ArithmeticType.Tobacco_D:
                    bEffectiveMin_State = false;
                    bEffectiveMax_State = true;

                    ///////////////////////////////////////////////////////////////
                    tobacco_D.Arithmetic = arithmetic;
                    tobacco_D.ROI = rROI;
                    /////////////////////////////////////////////////////////////////////////2021.03.12

                    tobacco_D._Init();

                    tobacco_D.EjectPixel = iMax;//检测基准赋值

                    tobacco_D.EjectLevel = iEjectLevel;//灵敏度赋值
                    tobacco_D.EjectPixelMin = iEjectPixelMin;//最小剔除像素值
                    tobacco_D.EjectPixelMax = iEjectPixelMax;//检测基准赋值上限

                    tobacco_D.Precision = fPrecision;//斜率赋值
                    break;

                case Enum.ArithmeticType.BaleLoosing:
                    ///////////////////////////////////////////////////////////////
                    baleLoosing.Arithmetic = arithmetic;
                    baleLoosing.ROI = rROI;
                    /////////////////////////////////////////////////////////////////////////2021.03.12

                    baleLoosing._Init();

                    baleLoosing.Sensor_ProductType = sSensor_ProductType;
                    baleLoosing.SensorNumber = bSensorNumber;
                    baleLoosing.ImageHeight = uiImageHeight;

                    if ((Enum.Detect_Type_S.Edge == (Enum.Detect_Type_S)(arithmetic.EnumCurrent[2] + 1))) //边缘检测算法
                    {
                        if (Enum.Scan_Direction.Bottom_Top == ((Enum.Scan_Direction)(arithmetic.EnumCurrent[1] + 1))) //查询最小值，89713FA复合型
                        {
                            bEffectiveMin_State = false;
                            bEffectiveMax_State = true;

                            if (Enum.SensorProductType._89713FA == sSensor_ProductType)//89713FA复合型
                            {
                                baleLoosing.EjectPixel = iMax;//检测基准赋值

                                baleLoosing.EjectLevel = iEjectLevel;//灵敏度赋值
                                baleLoosing.EjectPixelMin = iEjectPixelMin;//最小剔除像素值
                                baleLoosing.EjectPixelMax = iEjectPixelMax;//检测基准赋值上限

                                baleLoosing.Precision = fPrecision;//斜率赋值

                                bDetect_89713FA = true;
                            }
                            else
                            {
                                baleLoosing.Max = iMax;
                            }
                        }
                        else if (Enum.Scan_Direction.Top_Bottom == ((Enum.Scan_Direction)(arithmetic.EnumCurrent[1] + 1))) //查询最大值，89713CF散包
                        {
                            bEffectiveMin_State = true;
                            bEffectiveMax_State = false;
                            baleLoosing.Min = iMin;
                        }
                    }
                    else
                    {
                        bEffectiveMin_State = true;
                        bEffectiveMax_State = true;
                        baleLoosing.Min = iMin;
                        baleLoosing.Max = iMax;
                    }
                    break;

                case Enum.ArithmeticType.LocationCicle:
                    bEffectiveMin_State = true;
                    bEffectiveMax_State = true;

                    ///////////////////////////////////////////////////////////////
                    locationCicle.Arithmetic = arithmetic;
                    locationCicle.ROI = rROI;
                    /////////////////////////////////////////////////////////////////////////2021.03.12

                    locationCicle._Init();

                    locationCicle.ExistTolerance = bExistTolerance;

                    locationCicle.Min.Clear();
                    locationCicle.Min.Add(iMin);

                    locationCicle.Max.Clear();
                    locationCicle.Max.Add(iMax);
                    break;

                case Enum.ArithmeticType.LocationTemplateMatch:
                    bEffectiveMin_State = true;
                    bEffectiveMax_State = true;

                    ///////////////////////////////////////////////////////////////
                    locationTemplateMatch.Arithmetic = arithmetic;
                    locationTemplateMatch.ROI = rROI;
                    /////////////////////////////////////////////////////////////////////////2021.03.12

                    locationTemplateMatch._Init();

                    locationTemplateMatch.ExistTolerance = bExistTolerance;

                    locationTemplateMatch.Min.Clear();
                    locationTemplateMatch.Min.Add(iMin);

                    locationTemplateMatch.Max.Clear();
                    locationTemplateMatch.Max.Add(iMax);
                    break;

                case Enum.ArithmeticType.Classify:
                    bEffectiveMin_State = true;
                    bEffectiveMax_State = true;

                    ///////////////////////////////////////////////////////////////
                    classify.Arithmetic = arithmetic;
                    classify.ROI = rROI;
                    /////////////////////////////////////////////////////////////////////////2021.03.12

                    classify._Init();

                    classify.Max = iMax;
                    classify.Min = iMin;
                    break;

                default:
                    break;
            }
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明： 自学习函数
        // 输入参数： 1、Image<Bgr, Byte>：image，待处理的图像
        //            2、Boolean：flag，自学习后是否更新最大最小值
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        public void _LearnSample(Image<Bgr, Byte> image, Boolean flag = true)
        {
            iCompensation_H = 0;//水平方向补偿值清零
            iCompensation_V = 0;//垂直方向补偿值清零

            _Init(true);

            for (Int32 i = 0; i < iSampleValue.Length; i++)
            {
                iSampleValue[i] = 0;
            }

            switch (aType)
            {
                case Enum.ArithmeticType.Grid:
                    grid._LearnSample(image);
                    break;
                case Enum.ArithmeticType.Quality:
                    quality._LearnSample(image);
                    break;
                case Enum.ArithmeticType.Ruler:
                    ruler._LearnSample(image);

                    iSampleValue[0] = ruler.Result_Sample;
                    break;
                case Enum.ArithmeticType.Tobacco:
                    tobacco._LearnSample(image, flag);

                    if (flag)
                    {
                        iMax = Convert.ToInt32(tobacco.SampleValue * 1.2);//更新检测基准值上限2017.02.08
                        iMin = tobacco.EjectPixel;//更新检测基准值

                        fPrecision = tobacco.Precision;//更新斜率
                        fPrecisionInner = tobacco.PrecisionInner;//更新内部斜率
                    } 
                    break;

                case Enum.ArithmeticType.Disorder:
                    disorder._LearnSample(image);
                    break;

                case Enum.ArithmeticType.Line:
                    line._LearnSample(image);

                    uiSamplePos = line.SamplePos;
                    break;

                case Enum.ArithmeticType.CurveDispersion:
                    curvedispersion._LearnSample(image, flag);
                    if (flag)
                    {
                        iMax = Convert.ToInt32(curvedispersion.SampleValue * 1.4);
                        iMin = Convert.ToInt32(curvedispersion.SampleValue * 0.6);
                    }
                    break;

                case Enum.ArithmeticType.Tobacco_D:
                    tobacco_D._LearnSample(image, flag);

                    if (flag)
                    {
                        iMax = tobacco_D.EjectPixel;//更新检测基准值
                        iMin = Convert.ToInt32(tobacco_D.SampleValue * 0.9);

                        iEjectPixelMin = Convert.ToInt16(tobacco_D.SampleValue);

                        fPrecision = tobacco_D.Precision;//更新斜率
                    }
                    break;

                case Enum.ArithmeticType.BaleLoosing:
                    baleLoosing._LearnSample(image, flag);

                    if (flag)
                    {
                        if (Enum.SensorProductType._89713FA == sSensor_ProductType)//89713FA复合型
                        {
                            if ((Enum.Detect_Type_S.Edge == (Enum.Detect_Type_S)(arithmetic.EnumCurrent[2] + 1))) //边缘检测算法
                            {
                                if (Enum.Scan_Direction.Bottom_Top == ((Enum.Scan_Direction)(arithmetic.EnumCurrent[1] + 1))) //查询最小值，89713FA复合型
                                {
                                    iMax = baleLoosing.EjectPixel;//更新检测基准值
                                    iMin = Convert.ToInt32(baleLoosing.SampleValue * 0.9);

                                    iEjectPixelMin = Convert.ToInt16(baleLoosing.SampleValue);

                                    fPrecision = baleLoosing.Precision;//更新斜率
                                }
                            }
                        }
                    }
                    break;

                case Enum.ArithmeticType.LocationCicle:
                    locationCicle._LearnSample(image);
                    Array.Copy(locationTemplateMatch.CurrentValue, iValue, locationCicle.CurrentValue.Length);
                    Array.Copy(locationCicle.SampleValue, iSampleValue, locationCicle.SampleValue.Length);
                    break;

                case Enum.ArithmeticType.LocationTemplateMatch:
                    locationTemplateMatch._LearnSample(image);
                    Array.Copy(locationTemplateMatch.SampleValue, iSampleValue, locationTemplateMatch.SampleValue.Length);
                    break;

                case Enum.ArithmeticType.Classify:
                    classify._LearnSample(image);

                    iSampleValue[0] = classify.SampleValue;
                    break;

                default:
                    break;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明： 图像处理函数
        // 输入参数： image:待处理的图像
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        public Boolean _ImageProcess(Image<Bgr, Byte> image)
        {
            Boolean result = true;

            _Init();

            for (Int32 i = 0; i < iValue.Length; i++)
            {
                iValue[i] = 0;
            }

            switch (aType)
            {
                case Enum.ArithmeticType.Grid:
                    result = grid._ImageProcess(image);
                    iValue[0] = grid.MaxDifferance;
                    break;
                case Enum.ArithmeticType.Quality:
                    result = quality._ImageProcess(image);
                    iValue[0] = quality.MaxDifferance;
                    break;
                case Enum.ArithmeticType.Ruler:
                    result = ruler._ImageProcess(image);
                    iValue[0] = ruler.Result_Current;
                    break;
                case Enum.ArithmeticType.Tobacco:
                    result = tobacco._ImageProcess(image);
                    iValue[0] = tobacco.CurrentValue;
                    break;
                case Enum.ArithmeticType.Disorder:
                    result = disorder._ImageProcess(image);
                    iValue[0] = disorder.CurrentValue;
                    break;
                case Enum.ArithmeticType.Line:
                    result = line._ImageProcess(image);
                    iValue[0] = line.CurrentPos;
                    break;
                case Enum.ArithmeticType.CurveDispersion:
                    result = curvedispersion._ImageProcess(image);
                    iValue[0] = curvedispersion.CurrentValue;
                    break;
                case Enum.ArithmeticType.Tobacco_D:
                    result = tobacco_D._ImageProcess(image);
                    iValue[0] = tobacco_D.CurrentValue;
                    break;
                case Enum.ArithmeticType.BaleLoosing:
                    result = baleLoosing._ImageProcess(image);
                    iValue[0] = baleLoosing.CurrentValue;
                    break;
                case Enum.ArithmeticType.LocationCicle:
                    result = locationCicle._ImageProcess(image);
                    Array.Copy(locationCicle.CurrentValue, iValue, locationCicle.CurrentValue.Length);
                    break;
                case Enum.ArithmeticType.LocationTemplateMatch:
                    result = locationTemplateMatch._ImageProcess(image);
                    Array.Copy(locationTemplateMatch.CurrentValue, iValue, locationTemplateMatch.CurrentValue.Length);
                    break;
                case Enum.ArithmeticType.Classify:
                    result = classify._ImageProcess(image);
                    iValue[0] = classify.CurrentValue;
                    break;
                default:
                    break;
            }

            return result;
        }

        //-----------------------------------------------------------------------
        // 功能说明： 绘图函数
        // 输入参数： image:待处理的图像
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        public void _Drawing(Image<Bgr, Byte> image)
        {
            switch (aType)
            {
                case Enum.ArithmeticType.Grid:
                    grid._Drawing(image);
                    break;
                case Enum.ArithmeticType.Quality:
                    quality._Drawing(image);
                    break;
                case Enum.ArithmeticType.Ruler:
                    ruler._Drawing(image);
                    break;
                case Enum.ArithmeticType.Tobacco:
                    tobacco._Drawing(image);
                    break;
                case Enum.ArithmeticType.Disorder:
                    disorder._Drawing(image);
                    break;
                case Enum.ArithmeticType.Line:
                    line._Drawing(image);
                    break;
                case Enum.ArithmeticType.CurveDispersion:
                    curvedispersion._Drawing(image);
                    break;
                case Enum.ArithmeticType.Tobacco_D:
                    tobacco_D._Drawing(image);
                    break;
                case Enum.ArithmeticType.BaleLoosing:
                    baleLoosing._Drawing(image);
                    break;
                case Enum.ArithmeticType.LocationCicle:
                    locationCicle._Drawing(image);
                    break;
                case Enum.ArithmeticType.LocationTemplateMatch:
                    locationTemplateMatch._Drawing(image);
                    break;
                case Enum.ArithmeticType.Classify:
                    classify._Drawing(image);
                    break;
                default:
                    break;
            }
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：Arithmetic结构体初始化
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        private void _ArithmeticInit()
        {
            if (0 == arithmetic.Number)
            {
                arithmetic.Number = Convert.ToByte(String.TextData.ArithmeticName_Ruler_CHN.Length);
            }

            arithmetic.State = new Boolean[arithmetic.Number];
            arithmetic.ENGName = new string[arithmetic.Number];
            arithmetic.CHNName = new string[arithmetic.Number];
            arithmetic.Type = new Byte[arithmetic.Number];

            arithmetic.CurrentValue = new Int16[arithmetic.Number];
            arithmetic.MinValue = new Int16[arithmetic.Number];
            arithmetic.MaxValue = new Int16[arithmetic.Number];

            arithmetic.EnumType = new Byte[arithmetic.Number];
            arithmetic.EnumNumber = new Byte[arithmetic.Number];
            arithmetic.EnumState = new Boolean[arithmetic.Number, String.TextData.EnumType_CHN.Length];
            arithmetic.EnumCurrent = new Byte[arithmetic.Number];
        }
    }
}