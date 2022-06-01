/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：BaleLoosing.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：BaleLoosing工具

原作者：视觉检测团队
完成日期：2020/08/25
特别说明：经视觉检测团队授权并遵守代码使用条款，方可使用本代码并获得技术支持，否则所产生的一切后果由使用者承担

修改者：无
完成日期：无
修改记录：无

****************************************************************/
using System;

using System.Drawing;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace VisionSystemClassLibrary.Class
{
    [Serializable]
    public class BaleLoosing
    {
        private UInt16 uiColor;//颜色对比参数
        private UInt16 uiDirection;//扫描方向参数
        private UInt16 uiDetect;//检测方法
        private UInt16 uiThreshold;//相似性分割阈值
        private UInt16 uiCurveIndex;//曲线索引

        private Struct.ROI rROI;//定义结构体，传输兴趣区域信息
        private Struct.Arithmetic aArithmetic;//算法结构体

        private Int32 iSampleValue;//学习值（电压）
        private Int32 SampleValueTemp;//学习值缓存（像素）

        private Int32 iCurrentValue;//当前值（电压）
        private Int32 CurrentValueTemp;//当前值缓存（像素）

        private Boolean CheckResult = true;//检测结果,true:合格，false：不合格

        private Image<Gray, Byte> ImageGray;//灰度图像/B/G/R通道图像

        private Int32 iMin;//公差有效下限
        private Int32 iMax;//公差有效上限

        private Enum.SensorProductType sSensor_ProductType;
        private Byte bSensorNumber;//传感器数量
        private UInt16 uiImageHeight;//原始图像高度

        private Int16 iEjectLevel;//灵敏度赋值
        private Int32 iEjectPixel;//检测基准赋值
        private Int16 iEjectPixelMin;//最小剔除像素值
        private Int16 iEjectPixelMax;//检测基准赋值上限2017.02.08
        private float fPrecision;//斜率赋值

        private Rectangle minRectangle;//最小外接矩形

        //构造函数

        //-----------------------------------------------------------------------
        // 功能说明：构造函数
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public BaleLoosing()
        {
            rROI = new Struct.ROI();//定义结构体，传输兴趣区域信息
            aArithmetic = new Struct.Arithmetic();//算法结构体
            minRectangle = new Rectangle();

            if (0 == aArithmetic.Number)
            {
                aArithmetic.Number = Convert.ToByte(String.TextData.ArithmeticName_BaleLoosing_CHN.Length);
            }

            aArithmetic.State = new Boolean[aArithmetic.Number];
            aArithmetic.ENGName = new string[aArithmetic.Number];
            aArithmetic.CHNName = new string[aArithmetic.Number];
            aArithmetic.Type = new Byte[aArithmetic.Number];

            aArithmetic.CurrentValue = new Int16[aArithmetic.Number];
            aArithmetic.MinValue = new Int16[aArithmetic.Number];
            aArithmetic.MaxValue = new Int16[aArithmetic.Number];

            aArithmetic.EnumType = new Byte[aArithmetic.Number];
            aArithmetic.EnumNumber = new Byte[aArithmetic.Number];
            aArithmetic.EnumState = new Boolean[aArithmetic.Number, String.TextData.EnumType_CHN.Length];
            aArithmetic.EnumCurrent = new Byte[aArithmetic.Number];
        }

        //属性

        //-----------------------------------------------------------------------
        // 功能说明：SampleValue属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Int32 SampleValue
        {
            get
            {
                return iSampleValue;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：CurrentValue属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Int32 CurrentValue
        {
            get
            {
                return iCurrentValue;
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
            set
            {
                uiImageHeight = value;
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
            set
            {
                iMax = value;
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

        //-----------------------------------------------------------------------
        // 功能说明：Arithmetic属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Struct.Arithmetic Arithmetic
        {
            set
            {
                aArithmetic = value;
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
            get
            {
                return rROI;
            }
            set
            {
                rROI = value;
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
            set
            {
                iEjectPixelMin = value;
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
            set
            {
                iEjectPixelMax = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：EjectPixel属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Int32 EjectPixel
        {
            get
            {
                return iEjectPixel;
            }
            set
            {
                iEjectPixel = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：EjectLevel属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Int16 EjectLevel
        {
            set
            {
                iEjectLevel = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Precision属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public float Precision
        {
            get
            {
                return fPrecision;
            }
            set
            {
                fPrecision = value;
            }
        }

        //函数
        //-----------------------------------------------------------------------
        // 功能说明： 初始化函数,给参数赋值
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        public void _Init()
        {
            uiColor = aArithmetic.EnumCurrent[0];
            uiDirection = aArithmetic.EnumCurrent[1];
            uiDetect = Convert.ToUInt16(aArithmetic.EnumCurrent[2]);
            uiThreshold = Convert.ToUInt16(aArithmetic.CurrentValue[3]);
            uiCurveIndex = Convert.ToUInt16(aArithmetic.CurrentValue[4]);

            minRectangle = GeneralFunction._GetMinRect(rROI.roiExtra);
        }

        //-----------------------------------------------------------------------
        // 功能说明： 自学习函数
        // 输入参数： 1、Image<Bgr, Byte>：image，待处理的图像
        //            2、Boolean：flag，自学习后是否更新最大最小值
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        public void _LearnSample(Image<Bgr, Byte> image, Boolean flag = true)
        {
            if ((image != null) && (minRectangle.Right <= image.Width) && (minRectangle.Bottom <= image.Height) && (minRectangle.Width > 0) && (minRectangle.Height > 0))
            {
                _Process(image, (Enum.Detect_Type_S)(uiDetect + 1), true, flag);
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明： 图像处理函数
        // 输入参数： image:要处理的图像
        // 输出参数： 无
        // 返 回 值： result:检测结果，true:合格，false:不合格
        //----------------------------------------------------------------------
        public Boolean _ImageProcess(Image<Bgr, Byte> image)
        {
            Boolean result = true;

            if ((image != null) && (minRectangle.Right <= image.Width) && (minRectangle.Bottom <= image.Height) && (minRectangle.Width > 0) && (minRectangle.Height > 0))
            {
                result = _Process(image, (Enum.Detect_Type_S)(uiDetect + 1));
                CheckResult = result;
            }

            return result;
        }

        //-----------------------------------------------------------------------
        // 功能说明： 画图函数
        // 输入参数： image:要处理的图像
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        public void _Drawing(Image<Bgr, Byte> image)
        {
            if (null != image) //绘图函数图像不为空
            {
                GeneralFunction._DrawGraphics(ref image, rROI, new Bgr(0, 255, 0));
                Bgr color = CheckResult ? new Bgr(0, 255, 0) : new Bgr(0, 0, 255);//给颜色赋值

                switch ((Enum.Detect_Type_S)(uiDetect + 1))
                {
                    case Enum.Detect_Type_S.Average:
                    case Enum.Detect_Type_S.Edge:
                        image.Draw(new LineSegment2D(new Point(minRectangle.Left, CurrentValueTemp), new Point(minRectangle.Right - 1, CurrentValueTemp)), color, 1);
                        break;
                    case Enum.Detect_Type_S.Similarity:
                        if (uiThreshold < minRectangle.Height)
                        {
                            image.Draw(new LineSegment2D(new Point(minRectangle.Left, minRectangle.Top + uiThreshold), new Point(minRectangle.Right - 1, minRectangle.Top + uiThreshold)), color, 1);
                        }
                        else
                        {
                            image.Draw(new LineSegment2D(new Point(minRectangle.Left, minRectangle.Bottom), new Point(minRectangle.Right - 1, minRectangle.Bottom)), color, 1);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：BaleLoosing类拷贝函数
        // 输入参数：1.BaleLoosing：baseLoosing参数
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _CopyTo(ref BaleLoosing baseLoosing)
        {
            baseLoosing.uiColor = uiColor;
            baseLoosing.uiDirection = uiDirection;
            baseLoosing.uiDetect = uiDetect;
            baseLoosing.uiThreshold = uiThreshold;
            baseLoosing.uiCurveIndex = uiCurveIndex;

            baseLoosing.iSampleValue = iSampleValue;
            baseLoosing.iCurrentValue = iCurrentValue;
            baseLoosing.SampleValueTemp = SampleValueTemp;
            baseLoosing.CurrentValueTemp = CurrentValueTemp;

            rROI._CopyTo(ref baseLoosing.rROI);
            aArithmetic._CopyTo(ref baseLoosing.aArithmetic);

            baseLoosing.CheckResult = CheckResult;

            baseLoosing.iMin = iMin;
            baseLoosing.iMax = iMax;

            baseLoosing.sSensor_ProductType = sSensor_ProductType;
            baseLoosing.bSensorNumber = bSensorNumber;

            baseLoosing.iEjectLevel =iEjectLevel;
            baseLoosing.iEjectPixel = iEjectPixel;
            baseLoosing.iEjectPixelMin = iEjectPixelMin;
            baseLoosing.iEjectPixelMax = iEjectPixelMax;
            baseLoosing.fPrecision = fPrecision;

            baseLoosing.minRectangle = minRectangle;
        }

        //-----------------------------------------------------------------------
        // 功能说明： 图像处理函数
        // 输入参数： image：待处理的图像,learnState:自学习状态，true:自学习，false:实时,
        //            initState:初始化状态，true:实时自学习更新斜率等值，false:程序加载自学习，不计算斜率等值
        // 输出参数： 无
        // 返 回 值： result:检测结果，true:合格，false:不合格
        //----------------------------------------------------------------------
        private Boolean _Process(Image<Bgr, Byte> image, Enum.Detect_Type_S detecttype, Boolean learnState = false, Boolean flag = true)
        {
            Boolean result = true;

            Image<Bgr, Byte> imageROI = image.Copy(minRectangle);//获取兴趣区域图像
            GeneralFunction._AndMask(ref imageROI, rROI, minRectangle.Location);

            double dPresion = (double)uiImageHeight / bSensorNumber;
            Int32 iTop = Convert.ToInt32(dPresion * uiCurveIndex);
            Int32 iBottom = Convert.ToInt32(dPresion * (uiCurveIndex + 1));

            if ((minRectangle.Bottom <= iTop) || (minRectangle.Top >= iBottom)) //兴趣区域不在范围内
            {
                imageROI.SetZero();
            }
            else
            {
                if (minRectangle.Top < iTop)//兴趣区域上边缘超出范围
                {
                    imageROI.ROI = new Rectangle(0, 0, imageROI.Width, iTop - minRectangle.Top);
                    imageROI.SetZero();
                    imageROI.ROI = Rectangle.Empty;
                }

                if (minRectangle.Bottom > iBottom)//兴趣区域下边缘超出范围
                {
                    imageROI.ROI = new Rectangle(0, iBottom - minRectangle.Top, imageROI.Width, minRectangle.Bottom - iBottom);
                    imageROI.SetZero();
                    imageROI.ROI = Rectangle.Empty;
                }
            }

            ImageGray = GeneralFunction._ContrastColor(imageROI, (Enum.Contrast_Color)uiColor);//调用颜色对比函数进行处理
            ImageGray._ThresholdBinary(new Gray(127), new Gray(255));

            Int32 dResult = 0;
            switch (detecttype)
            {
                case Enum.Detect_Type_S.Average:
                    dResult = Convert.ToInt32(_Process_Average(ImageGray));

                    if (learnState)
                    {
                        if (flag)//执行自学习，并保存相关参数
                        {
                            SampleValueTemp = Convert.ToInt32(dResult + minRectangle.Top);//计算灰度值
                            iSampleValue = _ConvertToVoltage(SampleValueTemp, detecttype, iTop, iBottom);
                        }
                    }
                    else
                    {
                        CurrentValueTemp = Convert.ToInt32(dResult + minRectangle.Top);//计算灰度值
                        iCurrentValue = _ConvertToVoltage(CurrentValueTemp, detecttype, iTop, iBottom);

                        if ((iCurrentValue > iMax) || (iCurrentValue < iMin))//结果在公差范围外
                        {
                            result = false;
                        }
                    }
                    break;
                case Enum.Detect_Type_S.Edge:
                    dResult = _Process_Edge(ImageGray);

                    if (learnState)
                    {
                        if (flag)//执行自学习，并保存相关参数
                        {
                            SampleValueTemp = Convert.ToInt32(dResult + minRectangle.Top);//计算灰度值
                            iSampleValue = _ConvertToVoltage(SampleValueTemp, detecttype, iTop, iBottom);

                            if (Enum.Scan_Direction.Bottom_Top == ((Enum.Scan_Direction)(uiDirection + 1))) //查询最小值
                            {
                                if (Enum.SensorProductType._89713FA == sSensor_ProductType)//89713FA复合型
                                {
                                    fPrecision = ((float)(iEjectPixelMax - iSampleValue)) / 50;//更新斜率
                                    iEjectPixel = Convert.ToInt32(iSampleValue + fPrecision * iEjectLevel);//更新检测基准值
                                }
                            }
                        }
                    }
                    else
                    {
                        CurrentValueTemp = Convert.ToInt32(dResult + minRectangle.Top);//计算灰度值
                        iCurrentValue = _ConvertToVoltage(CurrentValueTemp, detecttype, iTop, iBottom);

                        if (Enum.Scan_Direction.Bottom_Top == ((Enum.Scan_Direction)(uiDirection+1))) //查询最小值
                        {
                            if (Enum.SensorProductType._89713FA == sSensor_ProductType)//89713FA复合型
                            {
                                result = (iCurrentValue <= iEjectPixel) ? true : false;//大于基准值检测结果合格，反之不合格
                            }
                            else
                            {
                                if (iCurrentValue > iMax)//结果在公差范围外
                                {
                                    result = false;
                                }
                            }
                        }
                        else if (Enum.Scan_Direction.Top_Bottom == ((Enum.Scan_Direction)(uiDirection + 1))) //查询最大值，89713CF散包
                        {
                            if (iCurrentValue < iMin)//结果在公差范围外
                            {
                                result = false;
                            }
                        }
                    }
                    break;
                case Enum.Detect_Type_S.Similarity:
                    dResult = Convert.ToInt32(_Process_Similarity(ImageGray));

                    if (learnState)
                    {
                        if (flag)//执行自学习，并保存相关参数
                        {
                            iSampleValue = SampleValueTemp = dResult;//计算灰度值
                        }
                    }
                    else
                    {
                        iCurrentValue = CurrentValueTemp = dResult;//计算灰度值

                        if ((iCurrentValue > iMax) || (iCurrentValue < iMin))//结果在公差范围外
                        {
                            result = false;
                        }
                    }
                    break;
                default:
                    break;
            }
            return result;//返回检测结果
        }

        //-----------------------------------------------------------------------
        // 功能说明： 平均值处理函数
        // 输入参数： image:待处理图像
        // 输出参数： pos:位置参数
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private Int32 _ConvertToVoltage(Int32 iData, Enum.Detect_Type_S detecttype, Int32 iTop, Int32 iBottom)
        {
            Int32 iVol = -1;

            if (Enum.Detect_Type_S.Edge == detecttype) //查找边缘
            {
                if (Enum.Scan_Direction.Bottom_Top == ((Enum.Scan_Direction)(uiDirection + 1))) //查询最小值
                {
                    if ((iData < iTop) || (iData > iBottom)) //不许超限
                    {
                        iData = iTop;
                    }
                }
                else
                {
                    if ((iData < iTop) || (iData > iBottom)) //不许超限
                    {
                        iData = iBottom;
                    }
                }
            }

            switch (sSensor_ProductType)
            {
                case Enum.SensorProductType._89713FA:
                    iVol = Convert.ToInt32(((double)uiImageHeight / bSensorNumber * uiCurveIndex + 204 - iData) * 5000 / 255);
                    break;
                case Enum.SensorProductType._89713CF:
                    iVol = Convert.ToInt32(((double)uiImageHeight / bSensorNumber * uiCurveIndex + 255 - iData) * 5000 / 255);
                    break;
                case Enum.SensorProductType._89750A:
                    break;
                default:
                    break;
            }
            return iVol;
        }

        //-----------------------------------------------------------------------
        // 功能说明： 平均值处理函数
        // 输入参数： image:待处理图像
        // 输出参数： pos:位置参数
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private double _Process_Average(Image<Gray, Byte> image)
        {
            Image<Gray, Double> imageProjection = new Image<Gray, Double>(1, image.Height);
            ImageGray.Reduce(imageProjection, REDUCE_DIMENSION.SINGLE_COL, REDUCE_TYPE.CV_REDUCE_SUM);//图像垂直投影

            double dPointCount = imageProjection.GetSum().Intensity / 255;
            double dSum = 0;

            if (0 != dPointCount) //非零点
            {
                for (Int32 i = 0; i < imageProjection.Height; i++)
                {
                    dSum += (imageProjection[i, 0].Intensity / 255) * i;
                }
                dSum /= dPointCount;
            }
            return dSum;
        }

        //-----------------------------------------------------------------------
        // 功能说明： 边缘处理函数
        // 输入参数： image:待处理图像
        // 输出参数： pos:位置参数
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private Int32 _Process_Edge(Image<Gray, Byte> image)
        {
            Image<Gray, Double> imageProjection = new Image<Gray, Double>(1, image.Height);
            ImageGray.Reduce(imageProjection, REDUCE_DIMENSION.SINGLE_COL, REDUCE_TYPE.CV_REDUCE_AVG);//图像垂直投影

            Int32 dSum = 0;

            switch ((Enum.Scan_Direction)(uiDirection + 1))
            {
                case Enum.Scan_Direction.Top_Bottom://垂直方向，增量方向
                    for (Int32 i = 0; i < imageProjection.Height; i++)
                    {
                        if (imageProjection[i, 0].Intensity > 0)
                        {
                            dSum = i;
                            break;
                        }
                    }
                    break;
                default://垂直方向，减量方向
                    for (Int32 i = imageProjection.Height - 1; i > 0; i--)
                    {
                        if (imageProjection[i, 0].Intensity > 0)
                        {
                            dSum = i;
                            break;
                        }
                    }
                    break;
            }
            return dSum;
        }

        //-----------------------------------------------------------------------
        // 功能说明： 相似性处理函数
        // 输入参数： image:待处理图像
        // 输出参数： pos:位置参数
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private double _Process_Similarity(Image<Gray, Byte> image)
        {
            double dSum = 0;
            Image<Gray, Byte> imageGrayTemp = ImageGray.Copy();

            double dAverageGray = ImageGray.CountNonzero()[0];

            if (dAverageGray > 0) 
            {
                Image<Gray, Byte> imageTemp = new Image<Gray, Byte>(ImageGray.Size);

                switch ((Enum.Scan_Direction)(uiDirection + 1))
                {
                    case Enum.Scan_Direction.Top_Bottom://垂直方向，增量方向
                        if (uiThreshold > imageTemp.Height)
                        {
                            imageTemp.ROI = new Rectangle(0, 0, imageTemp.Width, imageTemp.Height);
                            imageTemp.SetValue(new Gray(255));
                            imageTemp.ROI = Rectangle.Empty;
                        }
                        else
                        {
                            imageTemp.ROI = new Rectangle(0, 0, imageTemp.Width, uiThreshold);
                            imageTemp.SetValue(new Gray(255));
                            imageTemp.ROI = Rectangle.Empty;
                        }
                        break;
                    default://垂直方向，减量方向
                        if (uiThreshold < imageTemp.Height)
                        {
                            imageTemp.ROI = new Rectangle(0, uiThreshold, imageTemp.Width, imageTemp.Height - uiThreshold);
                            imageTemp.SetValue(new Gray(255));
                            imageTemp.ROI = Rectangle.Empty;
                        }
                        else
                        {
                            imageTemp.ROI = new Rectangle(0, 0, imageTemp.Width, imageTemp.Height);
                            imageTemp.SetValue(new Gray(255));
                            imageTemp.ROI = Rectangle.Empty;
                        }
                        break;
                }
                ImageGray._And(imageTemp);
                dSum = 100 * ImageGray.CountNonzero()[0] / dAverageGray;
            }
            return dSum > 100 ? 100 : dSum;
        }
    }
}
