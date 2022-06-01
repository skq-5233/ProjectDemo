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
    public class Tobacco_D
    {
        private UInt16 Color;//颜色对比参数

        private Struct.ROI rROI;//定义结构体，传输兴趣区域信息
        private Struct.Arithmetic aArithmetic;//算法结构体

        private Int32 iSampleValue;//学习值
        private Int32 iCurrentValue;//当前值

        private Int16 iEjectLevel;//灵敏度赋值
        private Int32 iEjectPixel;//检测基准赋值
        private Int16 iEjectPixelMin;//最小剔除像素值
        private Int16 iEjectPixelMax;//检测基准赋值上限2017.02.08
        private float fPrecision;//斜率赋值

        private Boolean CheckResult = true;//检测结果,true:合格，false：不合格

        private Image<Gray, Byte> ImageGray;//灰度图像/B/G/R通道图像

        private Rectangle minRectangle;//最小外接矩形

        //构造函数

        //-----------------------------------------------------------------------
        // 功能说明：构造函数
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Tobacco_D()
        {
            rROI = new Struct.ROI();//定义结构体，传输兴趣区域信息
            aArithmetic = new Struct.Arithmetic();//算法结构体
            minRectangle = new Rectangle();

            if (0 == aArithmetic.Number)
            {
                aArithmetic.Number = Convert.ToByte(String.TextData.ArithmeticName_Tobacco_D_CHN.Length);
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
            Color = aArithmetic.EnumCurrent[0];

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
                _Process(image, true, flag);
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
                result = _Process(image);
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
                Bgr color = CheckResult ? new Bgr(0, 255, 0) : new Bgr(0, 0, 255);//给颜色赋值
                GeneralFunction._DrawGraphics(ref image, rROI, color, 2);
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Tobacco_D类拷贝函数
        // 输入参数：1.Tobacco_D：tobacco，Tobacco参数
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _CopyTo(ref Tobacco_D tobacco)
        {
            tobacco.Color = Color;

            tobacco.iSampleValue = iSampleValue;
            tobacco.iCurrentValue = iCurrentValue;

            tobacco.iEjectPixel = iEjectPixel;
            tobacco.iEjectPixelMin = iEjectPixelMin;
            tobacco.iEjectPixelMax = iEjectPixelMax;

            tobacco.fPrecision = fPrecision;

            tobacco.iEjectLevel = iEjectLevel;

            rROI._CopyTo(ref tobacco.rROI);
            aArithmetic._CopyTo(ref tobacco.aArithmetic);
            
            tobacco.CheckResult = CheckResult;
            tobacco.minRectangle = minRectangle;
        }
        
        //-----------------------------------------------------------------------
        // 功能说明： 图像处理函数
        // 输入参数： image：待处理的图像,learnState:自学习状态，true:自学习，false:实时,
        //            initState:初始化状态，true:实时自学习更新斜率等值，false:程序加载自学习，不计算斜率等值
        // 输出参数： 无
        // 返 回 值： result:检测结果，true:合格，false:不合格
        //----------------------------------------------------------------------
        private Boolean _Process(Image<Bgr, Byte> image, Boolean learnState = false, Boolean flag = true)
        {
            Boolean result = true;
            
            Image<Bgr, Byte> imageROI = image.Copy(minRectangle);//获取兴趣区域图像
            GeneralFunction._AndMask(ref imageROI, rROI, minRectangle.Location);

            ImageGray = GeneralFunction._ContrastColor(imageROI, (Enum.Contrast_Color)Color);//调用颜色对比函数进行处理

            Int32[] sizes = new Int32[1] { 256 };
            IntPtr hist = CvInvoke.cvCreateHist(1, sizes, HIST_TYPE.CV_HIST_ARRAY, null, true);

            IntPtr[] source = new IntPtr[1];
            source[0] = ImageGray.Ptr;
            Image<Gray, byte> imageMask = GeneralFunction._GeneralImageMask(ImageGray, rROI, minRectangle.Location);
            CvInvoke.cvCalcHist(source, hist, false, imageMask);

            float minValue = 0, maxValue = 255;
            Int32[] minIdx = new Int32[1] { 0 };
            Int32[] maxIdx = new Int32[1] { 255 };
            CvInvoke.cvGetMinMaxHistValue(hist, ref minValue, ref maxValue, minIdx, maxIdx);

            CvInvoke.cvReleaseHist(ref hist);
            
            if (learnState)
            {
                if (flag)//执行自学习，并保存相关参数
                {
                    iSampleValue = Convert.ToInt32((Double)maxIdx[0] * 5000 / 255);//计算灰度值
                    fPrecision = ((float)(iEjectPixelMax - iSampleValue)) / 50;//更新斜率

                    iEjectPixel = Convert.ToInt32(iSampleValue + fPrecision * iEjectLevel);//更新检测基准值
                }
            }
            else
            {
                iCurrentValue = Convert.ToInt32((Double)maxIdx[0] * 5000 / 255);//计算灰度值
                result = (iCurrentValue <= iEjectPixel) ? true : false;//大于基准值检测结果合格，反之不合格
            }
            return result;//返回检测结果
        }
    }
}
