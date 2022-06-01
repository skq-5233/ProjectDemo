/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：内衬包装检测器
课题令号：41S1337
开发部门：智控事业部

文件名称：Ruler.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：Ruler工具

原作者：视觉检测团队
完成日期：2020/12/28
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
    public class Tobacco
    {
        private Enum.DivideType divide_Type;//创建分割方法枚举对象
        private Enum.Contrast_Color contrast_Color;//创建颜色对比枚举对象

        private UInt16 HlsValue;//HLS空间的H分量
        private UInt16 DevideType;//分割方法参数
        private UInt16 Color;//颜色对比参数
        private UInt16 RBdifference;//红蓝之差参数
        private UInt16 ThresholdValue;//阈值参数

        private Struct.ROI rROI;//定义结构体，传输兴趣区域信息
        private Rectangle ROIInner;//定义结构体，传输内轮廓兴趣区域信息

        private Struct.Arithmetic aArithmetic;//算法结构体

        private Enum.TobaccoType tTobaccoType;//检测类型,0:烟丝侧，1：滤嘴侧
        private Enum.FilterType fFilterType;//滤嘴类型，0：普通滤嘴，1：特殊滤嘴

        private Boolean bFOCKECheck;//FOCKE烟丝滤嘴检测，true:FOCKE，false:G.D

        [NonSerialized]
        private Contour<Point> ContourExtra;//外部轮廓序列
        [NonSerialized]
        private Contour<Point> ContourInner;//内部轮廓序列

        private Int32 iSampleValue;//学习值
        private Int32 SampleValueInner;//自学习内部轮廓面积

        private Int32 iCurrentValue;//当前值
        private Int32 CurrentValueInner;//内部轮廓面积

        private Boolean bSpecialStateInner;//特殊滤嘴内轮廓合格状态，true:合格，false：不合格
        private Int32 iEjectPixel;//检测基准赋值
        private Int32 iEjectPixelMax;//检测基准赋值上限2017.02.08
        private Int32 EjectPixelInner;//内轮廓检测基准像素值
                
        private float fPrecision;//斜率赋值
        private float fPrecisionInner;//内轮廓斜率

        private Int16 iEjectPixelMin;//最小剔除像素值
        private Int16 iEjectPixelMinInner;//内部最小剔除像素值

        private Int16 iEjectLevel;//灵敏度赋值

        private Boolean CheckResult = true;//检测结果,true:合格，false：不合格

        private Rectangle minRectangle;//最小外接矩形

        //构造函数

        //-----------------------------------------------------------------------
        // 功能说明：构造函数
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Tobacco()
        {
            rROI = new Struct.ROI();//定义结构体，传输兴趣区域信息
            ROIInner = new Rectangle();
            aArithmetic = new Struct.Arithmetic();//算法结构体
            minRectangle = new Rectangle();

            if (0 == aArithmetic.Number)
            {
                aArithmetic.Number = Convert.ToByte(String.TextData.ArithmeticName_Tobacco_CHN.Length);
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

        // 功能说明：FOCKECheck属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Boolean FOCKECheck
        {
            set
            {
                bFOCKECheck = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：TobaccoType属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Enum.TobaccoType TobaccoType
        {
            set
            {
                tTobaccoType = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：FilterType属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Enum.FilterType FilterType
        {
            set
            {
                fFilterType = value;
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
        // 功能说明：EjectPixelMinInner属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Int16 EjectPixelMinInner
        {
            set
            {
                iEjectPixelMinInner = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：EjectPixelMax属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Int32 EjectPixelMax
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

        //-----------------------------------------------------------------------
        // 功能说明：PrecisionInner属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public float PrecisionInner
        {
            get
            {
                return fPrecisionInner;
            }
            set
            {
                fPrecisionInner = value;
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
            HlsValue = Convert.ToUInt16(aArithmetic.CurrentValue[0]);//给HLS空间的H分量赋值
            DevideType = Convert.ToUInt16(aArithmetic.EnumCurrent[1]);//给分割方法参数赋值
            Color = aArithmetic.EnumCurrent[2];//给颜色赋值
            //Color = (UInt16)Enum.Contrast_Color.Blue;//默认颜色分量为蓝色
            RBdifference = Convert.ToUInt16(aArithmetic.CurrentValue[3]);//给红蓝之差赋值
            ThresholdValue = Convert.ToUInt16(aArithmetic.CurrentValue[4]);//给阈值赋值

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
                if (tTobaccoType == Enum.TobaccoType.Tobacco)//烟丝侧处理
                {
                    _Process_Tobacco(image, true, flag);
                }
                else if (tTobaccoType == Enum.TobaccoType.Filter)//滤嘴侧处理
                {
                    _Process_Filter(image, true, flag);
                }
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
                if (tTobaccoType == Enum.TobaccoType.Tobacco)//烟丝侧处理
                {
                    result = _Process_Tobacco(image);
                    CheckResult = result;
                }
                else if (tTobaccoType == Enum.TobaccoType.Filter)//滤嘴侧处理
                {
                    result = _Process_Filter(image);
                    CheckResult = result;
                }
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

                if (ContourExtra != null)//当前烟支兴趣区域轮廓面积有效，绘制各烟支有效兴趣区域轮廓
                {
                    image.Draw(ContourExtra, color, color, 2, 1, minRectangle.Location);
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Tobacco类拷贝函数
        // 输入参数：1.Tobacco：tobacco，Tobacco参数
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _CopyTo(ref Tobacco tobacco)
        {
            tobacco.HlsValue = HlsValue;
            tobacco.Color = Color;
            tobacco.ThresholdValue = ThresholdValue;

            tobacco.iSampleValue = iSampleValue;
            tobacco.SampleValueInner = SampleValueInner;
            tobacco.iCurrentValue = iCurrentValue;
            tobacco.CurrentValueInner = CurrentValueInner;

            tobacco.bSpecialStateInner = bSpecialStateInner;
            tobacco.iEjectPixel = iEjectPixel;
            tobacco.iEjectPixelMax = iEjectPixelMax;
            tobacco.EjectPixelInner = EjectPixelInner;

            tobacco.fPrecision = fPrecision;
            tobacco.fPrecisionInner = fPrecisionInner;

            tobacco.iEjectPixelMin = iEjectPixelMin;
            tobacco.iEjectPixelMinInner = iEjectPixelMinInner;
            tobacco.iEjectLevel = iEjectLevel;

            rROI._CopyTo(ref tobacco.rROI);
            tobacco.ROIInner = ROIInner;
            aArithmetic._CopyTo(ref tobacco.aArithmetic);

            tobacco.tTobaccoType = tTobaccoType;
            tobacco.fFilterType = fFilterType;
            tobacco.bFOCKECheck = bFOCKECheck;

            tobacco.ContourExtra = ContourExtra;
            tobacco.ContourInner = ContourInner;

            tobacco.CheckResult = CheckResult;

            tobacco.divide_Type = divide_Type;
            tobacco.contrast_Color = contrast_Color;

            tobacco.DevideType = DevideType;

            tobacco.RBdifference = RBdifference;

            tobacco.minRectangle = minRectangle;
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明： 利用HLS空间的H分量值分割去除模合，将烟丝之外的部分置白色
        // 输入参数： image：待处理的图像
        // 输出参数： image:得到的处理图像
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private Image<Bgr, Byte> _HlsHandle(Image<Bgr, Byte> image)
        {
            Image<Gray, Byte> ImageMask = new Image<Gray, Byte>(1, 1);
            if (bFOCKECheck)//利用红蓝分量BR之差小于14去除模合，将烟丝之外的部分置白色，20170914gc
            {
                Image<Gray, Byte> BRdiskImage = image.Split()[0].AbsDiff(image.Split()[2]);
                ImageMask = BRdiskImage.Cmp(HlsValue, CMP_TYPE.CV_CMP_LT);
            }
            else
            {
                Image<Hls, float> hlsImage = image.Convert<Hls, float>();
                ImageMask = hlsImage.Split()[0].Cmp(HlsValue, CMP_TYPE.CV_CMP_GT);
            }

            ImageMask._Erode(1);
            ImageMask._Dilate(1);
            image.SetValue(new Bgr(255, 255, 255), ImageMask);

            return image;
        }

        //-----------------------------------------------------------------------
        // 功能说明： 烟丝分割方法
        // 输入参数： image：待处理的图像
        // 输出参数： 无
        // 返 回 值： imageGray:分割后的灰度图
        //----------------------------------------------------------------------
        private Image<Gray, Byte> _Divide(Image<Bgr, Byte> image, Enum.DivideType divideType)
        {
            Image<Gray, Byte> imageGray;
            switch (divideType)
            {
                case Enum.DivideType.Single://颜色对比
                    imageGray = GeneralFunction._ContrastColor(image, (Enum.Contrast_Color)Color);//获取颜色分量
                    break;
                case Enum.DivideType.R_B://浅色烟丝识别
                    Image<Gray, Byte> imageMask = image.Split()[2].Cmp((image.Split()[0] + RBdifference), CMP_TYPE.CV_CMP_GT);
                    image.SetValue(new Bgr(0, 0, 0), imageMask);
                    imageGray = image.Convert<Gray, Byte>();
                    break;
                case Enum.DivideType.All://浅色烟丝识别 + 颜色对比
                    Image<Gray, Byte> imageMask2 = image.Split()[2].Cmp((image.Split()[0] + RBdifference), CMP_TYPE.CV_CMP_GT);
                    image.SetValue(new Bgr(0, 0, 0), imageMask2);//浅色烟丝识别
                    imageGray = GeneralFunction._ContrastColor(image, (Enum.Contrast_Color)Color);//获取颜色分量
                    break;
                default:
                    imageGray = GeneralFunction._ContrastColor(image, (Enum.Contrast_Color)Color);//获取颜色分量
                    break;
            }
            return imageGray;
        }

        //-----------------------------------------------------------------------
        // 功能说明： 烟丝侧处理函数函数
        // 输入参数： image：待处理的图像,learnState:自学习状态，true:自学习，false:实时,
        //            initState:初始化状态，true:实时自学习更新斜率等值，false:程序加载自学习，不计算斜率等值
        // 输出参数： 无
        // 返 回 值： result:检测结果，true:合格，false:不合格
        //----------------------------------------------------------------------
        private Boolean _Process_Tobacco(Image<Bgr, Byte> image, Boolean learnState = false, Boolean flag = true)
        {
            Boolean result = true;

            Image<Bgr, Byte> imageROI = image.Copy(minRectangle);//获取兴趣区域图像
            GeneralFunction._AndMask(ref imageROI, rROI, minRectangle.Location);

            imageROI = _HlsHandle(imageROI);//HLS空间H分量处理

            Image<Gray, Byte> imageGray = _Divide(imageROI, (Enum.DivideType)DevideType);

            //imageGray = _AutoPositionTobacco(imageGray);         //烟丝定位，相连烟支分割开

            imageGray = imageGray.ThresholdBinaryInv(new Gray(ThresholdValue), new Gray(255));//阈值分割
            imageGray._And(GeneralFunction._GeneralImageMask(imageGray, rROI, minRectangle.Location));//2021.08.27更改

            if (learnState)
            {
                if (flag)//执行自学习，并保存相关参数
                {
                    ContourExtra = _ContourFind(imageGray, ref iSampleValue);//查找外轮廓，计算模板像素值
                    fPrecision = ((float)(iSampleValue - iEjectPixelMin)) / 50;//更新斜率
                    iEjectPixel = Convert.ToInt32(iSampleValue - fPrecision * iEjectLevel);//更新检测基准值
                }
            }
            else
            {
                ContourExtra = _ContourFind(imageGray, ref iCurrentValue);//查找外轮廓，计算当前像素值
            }

            result = (iCurrentValue >= iEjectPixel) ? true : false;//大于基准值检测结果合格，反之不合格

            return result;//返回检测结果
        }

        //-----------------------------------------------------------------------
        // 功能说明： 滤嘴侧处理函数
        // 输入参数： image：待处理的图像,learnState:自学习状态，true:自学习，false:实时,
        //            initState:初始化状态，true:实时自学习更新斜率等值，false:程序加载自学习，不计算斜率等值
        // 输出参数： 无
        // 返 回 值： result:检测结果，true:合格，false:不合格
        //----------------------------------------------------------------------
        private Boolean _Process_Filter(Image<Bgr, Byte> image, Boolean learnState = false, Boolean flag = true)
        {
            Boolean result = true;
            bSpecialStateInner = true;

            Image<Bgr, Byte> imageROI = image.Copy(minRectangle);//获取兴趣区域图像
            GeneralFunction._AndMask(ref imageROI, rROI, minRectangle.Location);

            Image<Gray, Byte> ExtraImageGray = imageROI.Convert<Gray, Byte>();//灰度化滤嘴侧兴趣区域

            //ExtraImageGray = _AutoPositionFilter(ExtraImageGray); //自动定位烟支位置，将烟支相连处断开

            ExtraImageGray = ExtraImageGray.ThresholdBinary(new Gray(ThresholdValue), new Gray(255));//对滤嘴侧兴趣区域进行阈值分割

            if (learnState)
            {
                if (flag)//执行自学习，并保存相关参数
                {
                    ContourExtra = _ContourFind(ExtraImageGray, ref iSampleValue);//查找外轮廓，计算模板像素值
                    fPrecision = ((float)(iSampleValue - iEjectPixelMin)) / 50;//更新斜率
                    iEjectPixel = Convert.ToInt32(iSampleValue - fPrecision * iEjectLevel);//更新检测基准值
                }
            }
            else
            {
                ContourExtra = _ContourFind(ExtraImageGray, ref iCurrentValue);//查找外轮廓，计算当前像素值
            }
            //

            //查找、计算外轮廓上半部分
            Image<Gray, Byte> ExtraHalfImageGray = ExtraImageGray.Copy(new Rectangle(0, 0, ExtraImageGray.Width, ExtraImageGray.Height / 2));//外轮廓上半部分
            Int32 AreaExtraHalf = 0;
            Contour<Point> ExtraHalfContour = _ContourFind(ExtraHalfImageGray, ref AreaExtraHalf);//查找外轮廓上半部分
            //

            //查找、计算内轮廓
            if (fFilterType == Enum.FilterType.Special)//滤嘴类型为特殊滤嘴时
            {
                ROIInner.X = Convert.ToInt32(0.855 * minRectangle.Left + 0.145 * minRectangle.Width);//计算内轮廓兴趣区域横坐标信息
                ROIInner.Y = Convert.ToInt32(0.855 * minRectangle.Top + 0.145 * minRectangle.Height);//计算内轮廓兴趣区域纵坐标信息
                ROIInner.Width = Convert.ToInt32(0.71 * minRectangle.Width);//计算内轮廓兴趣区域宽度信息
                ROIInner.Height = Convert.ToInt32(0.71 * minRectangle.Height);//计算内轮廓兴趣区域高度信息

                Image<Bgr, Byte> imageROIInner = image.Copy(ROIInner);//获取内轮廓兴趣区域图像

                PointF CicleCenterPointsInner = new PointF(ROIInner.Width / 2, ROIInner.Height / 2);
                CircleF CicleRadirInner = new CircleF(CicleCenterPointsInner, (ROIInner.Width + ROIInner.Height) / 4);
                Image<Gray, Byte> InnerImageMask = new Image<Gray, Byte>(ROIInner.Width, ROIInner.Height);//创建内轮廓兴趣区域大小模板
                InnerImageMask.Draw(CicleRadirInner, new Gray(255), -1);
                InnerImageMask = InnerImageMask.ThresholdBinaryInv(new Gray(10), new Gray(255));//生成滤嘴侧内部兴趣区域模板

                imageROIInner.SetValue(new Bgr(255, 255, 255), InnerImageMask);//将非图案区域像素变为白色

                Image<Gray, Byte> InnerImageGray = imageROIInner.Convert<Gray, Byte>();//灰度化滤嘴侧内轮廓兴趣区域
                InnerImageGray = InnerImageGray.ThresholdBinaryInv(new Gray(ThresholdValue), new Gray(255));//对滤嘴侧内轮廓兴趣区域进行阈值分割
                InnerImageGray._Erode(1);//腐蚀
                InnerImageGray._Dilate(1);//膨胀

                if (learnState)
                {
                    ContourInner = _ContourFind(InnerImageGray, ref SampleValueInner);//查找内轮廓，计算模板像素值
                    fPrecisionInner = ((float)(SampleValueInner - iEjectPixelMinInner)) / 50;//更新斜率
                    EjectPixelInner = Convert.ToInt32(SampleValueInner - fPrecisionInner * iEjectLevel);//更新检测基准值
                }
                else
                {
                    ContourInner = _ContourFind(InnerImageGray, ref CurrentValueInner);//查找内轮廓，计算当前像素值
                }

                bSpecialStateInner = (CurrentValueInner > EjectPixelInner) ? true : false;//给特殊滤嘴内轮廓合格状态赋值，true:合格，false：不合格
            }
            //

            if ((iCurrentValue < iEjectPixel) || (CurrentValueInner < EjectPixelInner)
                || (iCurrentValue > iEjectPixelMax) || (AreaExtraHalf < (iSampleValue / 10)))//滤嘴侧有效像素个数不满足像素个数基准值条件)
            {
                result = false;
            }

            return result;//返回检测结果
        }

        //-----------------------------------------------------------------------
        // 功能说明： 轮廓查询函数
        // 输入参数： image：待处理的图像
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private Contour<Point> _ContourFind(Image<Gray, Byte> imageGray, ref Int32 AreaMax)
        {
            AreaMax = 0;
            Double AreaBuf = 0;
            Contour<Point> ContourBuf = null;
            Contour<Point> ContourBufBuf = null;
            Contour<Point> InnerContourBuf = null;

            ContourBuf = imageGray.FindContours(
                        CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_NONE, RETR_TYPE.CV_RETR_CCOMP);//查找内外轮廓
            ContourBufBuf = ContourBuf;

            for (; ContourBuf != null; ContourBuf = ContourBuf.HNext)//计算所有外轮廓面积
            {
                AreaBuf = ContourBuf.Area;
                InnerContourBuf = ContourBuf.VNext;
                for (; InnerContourBuf != null; InnerContourBuf = InnerContourBuf.HNext)//计算所有内轮廓面积
                {
                    AreaBuf = AreaBuf - InnerContourBuf.Area;
                }
                if (AreaMax < AreaBuf)//计算滤嘴侧有效轮廓面积
                {
                    AreaMax = Convert.ToInt32(AreaBuf);
                }
            }

            return ContourBufBuf;
        }
        
        //-----------------------------------------------------------------------
        // 功能说明： 滤嘴自动定位
        // 输入参数： image：待处理的灰度图像
        // 输出参数： 无
        // 返 回 值： 返回的灰度图像，将烟支相连部分分割开
        //----------------------------------------------------------------------
        private Image<Gray, Byte> _AutoPositionFilter(Image<Gray, Byte> image)
        {
            Image<Gray, Byte> imageResult = image.Copy();

            Image<Gray, Byte> ImageROW = new Image<Gray, Byte>(image.Width, 1);
            image.Reduce(ImageROW, REDUCE_DIMENSION.SINGLE_ROW, REDUCE_TYPE.CV_REDUCE_AVG);     //获取灰度图像
             
            int[] projV = new int[image.Width];           //投影数组
            for (int i = 0; i < image.Width; i++)         //获取投影数组
            {
                projV[i] = Convert.ToInt32(ImageROW[0, i].Intensity);
            }

            int aveProjV = 0;                             //投影曲线平均值
            for (int i = 0; i < projV.Length; i++)        //计算投影曲线均值
            {
                aveProjV += projV[i];
            }
            aveProjV = aveProjV / projV.Length;

            for (int i = 0; i < projV.Length; i++)         //高于均值部分置为均值
            {
                if (projV[i] > aveProjV)
                    projV[i] = aveProjV;
            }

            //从左往右搜索疑似波谷边缘位置
            int leftX = 0, rightX = projV.Length - 1;
            for (int i = 0; i < projV.Length - 1; i++)
            {
                if ((projV[i] == aveProjV && projV[i + 1] < aveProjV) || projV[i] < aveProjV)     //疑似曲线左侧下降位置
                {
                    leftX = i;
                    break;
                }
            }
            //从右往左搜索疑似波谷边缘位置
            for (int i = projV.Length - 1; i > 0; i--)
            {
                if ((projV[i] == aveProjV && projV[i - 1] < aveProjV) || projV[i] < aveProjV)     //疑似曲线右侧下降位置
                {
                    rightX = i;
                    break;
                }
            }
            //将两个疑似下降位置一分为二，分别左右搜索最低点
            int minVV = 1000;
            int down1 = 0;
            for (int i = 0; i < (leftX + rightX) / 2; i++)           //搜索左侧最低点为烟支左侧边缘
            {
                if (minVV > projV[i])
                {
                    minVV = projV[i];
                    down1 = i;
                }
            }
            //从中心往右查找右波谷
            int minGG = 1000;
            int down2 = projV.Length - 1;
            for (int i = (leftX + rightX) / 2; i < projV.Length; i++)    //搜索右侧最低点为烟支右侧边缘
            {
                if (minGG > projV[i])
                {
                    minGG = projV[i];
                    down2 = i;
                }
            }
            //将down1与down2位置处画直线，将烟支边缘部分分离
            imageResult.Draw(new LineSegment2D(new Point(down1, 0), new Point(down1, imageResult.Height)), new Gray(0), 1);
            imageResult.Draw(new LineSegment2D(new Point(down2, 0), new Point(down2, imageResult.Height)), new Gray(0), 1);

            return imageResult;
        }

        //-----------------------------------------------------------------------
        // 功能说明： 颜色对比
        // 输入参数： image：待处理的图像,color:颜色对比枚举参数
        // 输出参数： imageGray:得到的灰度图像
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private Image<Gray, Byte> _AutoPositionTobacco(Image<Gray, Byte> image)
        {
            Image<Gray, Byte> imageResult = image.Copy();

            Image<Gray, Byte> ImageROW = new Image<Gray, Byte>(image.Width, 1);              //创建投影图像
            image.Reduce(ImageROW, REDUCE_DIMENSION.SINGLE_ROW, REDUCE_TYPE.CV_REDUCE_AVG);  //获取投影数据

            int[] projV = new int[image.Width];                         //投影数组
            for (int i = 0; i < image.Width; i++)                       //投影数组取值
            {
                projV[i] = Convert.ToInt32(ImageROW[0, i].Intensity);
            }

            int countN = 0, aveV = 0;               //统计个数及平均值
            for (int i = 0; i < projV.Length; i++)
            {
                if (projV[i] > 40)                  //统计平均灰度大于40的个数，
                {
                    countN++;
                    aveV += projV[i];
                }
            }

            if (countN != 0)                        //计算平均灰度
                aveV = aveV / countN;               

            for (int i = 0; i < projV.Length; i++)  
            {
                if (projV[i] < aveV)                //小于平均值的点置零
                    projV[i] = 0;
            }

            int leftX = 0, rightX = projV.Length - 1;  //左右疑似烟支边缘的点
            for (int i = 0; i < projV.Length - 1; i++)
            {
                if ((projV[i] == 0 && projV[i + 1] != 0) || projV[i] != 0)   //当前点为0且下一个点不为0或当前点不为0
                {
                    leftX = i;
                    break;
                }
            }
            for (int i = projV.Length - 1; i > 0; i--)
            {
                if ((projV[i] == 0 && projV[i - 1] != 0) || projV[i] != 0)   //当前点为0且上一个点不为0或当前点不为0
                {
                    rightX = i;
                    break;
                }
            }

            int maxVV = 0, top1 = 0;            //左侧最大值及位置
            for (int i = 0; i < (leftX + rightX) / 2; i++)
            {
                if (maxVV < projV[i])           //搜索最大值
                {
                    maxVV = projV[i];
                    top1 = i;
                }
            }

            int maxGG = 0, top2 = projV.Length - 1;  //右侧最大值及位置
            for (int i = (leftX + rightX) / 2; i < projV.Length; i++)
            {
                if (maxGG < projV[i])         //搜索最大值
                {
                    maxGG = projV[i];
                    top2 = i;
                }
            }

            imageResult.Draw(new LineSegment2D(new Point(top1, 0), new Point(top1, imageResult.Height)), new Gray(255), 2);
            imageResult.Draw(new LineSegment2D(new Point(top2, 0), new Point(top2, imageResult.Height)), new Gray(255), 2);

            return imageResult;
        }
    }
}