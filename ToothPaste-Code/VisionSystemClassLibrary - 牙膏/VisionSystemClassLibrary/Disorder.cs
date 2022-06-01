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
    public class Disorder
    {
        private UInt16 uiColor;//颜色对比参数
        private UInt16 edgeThreshold;//Canny算子边缘查找阈值
        private UInt16 linkThreshold;//Canny算子边缘连接阈值
        private UInt16 reverseThreshold;//反支检测阈值

        private Struct.ROI rROI;//定义结构体，向格子传输工作区域信息

        private Struct.Arithmetic aArithmetic;//算法结构体
        private Enum.DisorderType dDisorderType = new Enum.DisorderType();//乱烟类型,0:反支，1：大空洞

        private Image<Gray, Byte> ImageGray;//灰度图像/B/G/R通道图像
        private Image<Gray, Byte> ImageCanny;//Canny算子图像
        private Image<Gray, Byte> ImageThreshold;//阈值图像

        private Int32 iMax;//公差有效上限

        private Int32 iSampleValue;//学习值
        private Int32 iCurrentValue;//当前值

        [NonSerialized]
        private Contour<Point> Contour;//轮廓序列

        private Rectangle minRectangle;//最小外接矩形

        //

        //-----------------------------------------------------------------------
        // 功能说明：构造函数
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Disorder()
        {
            rROI = new Struct.ROI();//定义结构体，传输兴趣区域信息
            aArithmetic = new Struct.Arithmetic();//算法结构体
            minRectangle = new Rectangle();

            if (0 == aArithmetic.Number)
            {
                aArithmetic.Number = Convert.ToByte(String.TextData.ArithmeticName_Disorder_CHN.Length);
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
        // 功能说明：DisorderType属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Enum.DisorderType DisorderType
        {
            set
            {
                dDisorderType = value;
            }
        }

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
        // 功能说明：初始化函数,给参数赋值
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void Init(Boolean state = false)
        {
            uiColor = aArithmetic.EnumCurrent[0];
            edgeThreshold = Convert.ToUInt16(aArithmetic.CurrentValue[1]);
            linkThreshold = Convert.ToUInt16(aArithmetic.CurrentValue[2]);
            reverseThreshold = Convert.ToUInt16(aArithmetic.CurrentValue[3]);

            minRectangle = GeneralFunction._GetMinRect(rROI.roiExtra);
        }

        //-----------------------------------------------------------------------
        // 功能说明： 自学习函数
        // 输入参数： image:要处理的图像
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        public void _LearnSample(Image<Bgr, Byte> image, Boolean state = true)
        {
        }

        //-----------------------------------------------------------------------
        // 功能说明： 图像处理函数
        // 输入参数： image:要处理的图像
        // 输出参数： 无
        // 返 回 值： CheckResult:检测结果，true:合格，false:不合格
        //----------------------------------------------------------------------
        public Boolean _ImageProcess(Image<Bgr, Byte> image)
        {
            Boolean result = true;

            if ((image != null) && (minRectangle.Right <= image.Width) && (minRectangle.Bottom <= image.Height) && (minRectangle.Width > 0) && (minRectangle.Height > 0))
            {
                Image<Bgr, Byte> imageROI = image.Copy(minRectangle);//获取兴趣区域图像
                GeneralFunction._AndMask(ref imageROI, rROI, minRectangle.Location);

                ImageGray = GeneralFunction._ContrastColor(imageROI, (Enum.Contrast_Color)uiColor);//调用颜色对比函数进行处理

                ImageCanny = ImageGray.Canny(edgeThreshold, linkThreshold);//Canny算子
                ImageCanny._Dilate(2);//膨胀

                if (dDisorderType == Enum.DisorderType.Reverse)//反支处理
                {
                    result = _Process_Reverse();
                }

                else if (dDisorderType == Enum.DisorderType.Hole)//大空洞处理
                {
                    ImageCanny.Draw(new LineSegment2D(new Point(0, 0), new Point(0, ImageCanny.Height - 1)), new Gray(255), 2);
                    ImageCanny.Draw(new LineSegment2D(new Point(ImageCanny.Width - 2, 0), new Point(ImageCanny.Width - 2, ImageCanny.Height - 1)), new Gray(255), 2);

                    result = _Process_Hole();
                }
            }
            return result;
        }
        
        //-----------------------------------------------------------------------
        // 功能说明： 反支处理函数
        // 输入参数： image：待处理的图像,learnState:自学习状态，true:自学习，false:实时,
        // 输出参数： 无
        // 返 回 值： result:检测结果，true:合格，false:不合格
        //----------------------------------------------------------------------
        private Boolean _Process_Reverse(Boolean learnState = false)
        {
            Boolean result = true;
            Double AreaBuf = 0;
            iCurrentValue = 0;
            Contour<Point> ContourBuf = null;

            ImageGray.SetValue(new Gray(0), ImageCanny);//以膨胀后的Canny算子为模板将盘纸置黑

            //2017.03.09修订，分块阈值处理上部阈值为reverseThreshold，下部阈值为（reverseThreshold + 70）
            Int32 imageUPHeight = Convert.ToInt32(ImageGray.Height * 0.6);
            ImageGray.ROI = new Rectangle(0, 0, ImageGray.Width, imageUPHeight + 5);
            ImageGray._ThresholdBinary(new Gray(reverseThreshold), new Gray(255));
            ImageGray.ROI = Rectangle.Empty;

            ImageGray.ROI = new Rectangle(0, imageUPHeight - 5, ImageGray.Width, (ImageGray.Height - imageUPHeight + 4));
            Int32 reverseThresholdTemp = (reverseThreshold + 70) > 255 ? 255 : (reverseThreshold + 70);
            ImageGray._ThresholdBinary(new Gray(reverseThresholdTemp), new Gray(255));
            ImageGray.ROI = Rectangle.Empty;

            ImageThreshold = ImageGray;

            ContourBuf = ImageThreshold.FindContours(CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_NONE, RETR_TYPE.CV_RETR_CCOMP);//查找轮廓
            Contour = ContourBuf;

            for (; ContourBuf != null; ContourBuf = ContourBuf.HNext)//计算外轮廓面积
            {
                AreaBuf = ContourBuf.Area;            
                if (iCurrentValue < AreaBuf)//计算有效轮廓面积
                {
                    iCurrentValue = Convert.ToInt32(AreaBuf);
                }
            }
            result = (iCurrentValue < iMax) ? true : false;//大于基准值检测结果合格，反之不合格

            return result;//返回检测结果
        }

        //-----------------------------------------------------------------------
        // 功能说明： 空洞处理函数
        // 输入参数： image：待处理的图像,learnState:自学习状态，true:自学习，false:实时
        // 输出参数： 无
        // 返 回 值： result:检测结果，true:合格，false:不合格
        //----------------------------------------------------------------------
        private Boolean _Process_Hole(Boolean learnState = false)
        {
            Boolean result = true;
            Double AreaBuf = 0;
            iCurrentValue = 0;
            Contour<Point> ContourBuf = null;
            Contour<Point> ContourInnerBuf = null;

            ImageCanny.ThresholdBinaryInv(new Gray(127), new Gray(255));
            ImageCanny._And(GeneralFunction._GeneralImageMask(ImageCanny, rROI, minRectangle.Location));//2021.08.27更改
            ContourBuf = ImageCanny.FindContours(CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_NONE, RETR_TYPE.CV_RETR_CCOMP);//查找轮廓
            Contour = ContourBuf;

            for (; ContourBuf != null; ContourBuf = ContourBuf.HNext)//计算外轮廓面积
            {
                ContourInnerBuf = ContourBuf.VNext;
                for (; ContourInnerBuf != null; ContourInnerBuf = ContourInnerBuf.HNext)//计算内轮廓面积
                {
                    AreaBuf = ContourInnerBuf.Area;

                    if (iCurrentValue < AreaBuf)//计算有效轮廓面积
                    {
                        iCurrentValue = Convert.ToInt32(AreaBuf);
                    }
                }
            }
            result = (iCurrentValue < iMax) ? true : false;//大于基准值检测结果合格，反之不合格

            return result;//返回检测结果
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

                Contour<Point> contourBuf = Contour;
                if (dDisorderType == Enum.DisorderType.Reverse)//画反支轮廓
                {
                    for (; contourBuf != null; contourBuf = contourBuf.HNext)//计算外轮廓面积
                    {
                        if (contourBuf.Area < iMax)//计算有效轮廓面积
                        {
                            image.Draw(contourBuf, new Bgr(0, 255, 0), new Bgr(0, 255, 0), 2, 1, minRectangle.Location);
                        }
                        else
                        {
                            image.Draw(contourBuf, new Bgr(0, 0, 255), new Bgr(0, 0, 255), 2, 1, minRectangle.Location);
                        }
                    }
                }
                else if (dDisorderType == Enum.DisorderType.Hole)//画大空洞轮廓
                {
                    Contour<Point> ContourInnerBuf = null;

                    for (; contourBuf != null; contourBuf = contourBuf.HNext)//计算外轮廓面积
                    {
                        ContourInnerBuf = contourBuf.VNext;
                        for (; ContourInnerBuf != null; ContourInnerBuf = ContourInnerBuf.HNext)//计算内轮廓面积
                        {
                            if (ContourInnerBuf.Area < iMax)//计算有效轮廓面积
                            {
                                image.Draw(ContourInnerBuf, new Bgr(0, 255, 0), new Bgr(0, 255, 0), 2, 1, minRectangle.Location);
                            }
                            else
                            {
                                image.Draw(ContourInnerBuf, new Bgr(0, 0, 255), new Bgr(0, 0, 255), 2, 1, minRectangle.Location);
                            }
                        }
                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Disorder类拷贝函数
        // 输入参数：1.Disorder：disorder，Disorder参数
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _CopyTo(ref Disorder disorder)
        {
            disorder.uiColor = uiColor;
            disorder.edgeThreshold = edgeThreshold;
            disorder.linkThreshold = linkThreshold;
            disorder.reverseThreshold = reverseThreshold;

            rROI._CopyTo(ref disorder.rROI);

            aArithmetic._CopyTo(ref disorder.aArithmetic);

            disorder.dDisorderType = dDisorderType;

            disorder.iMax = iMax;

            disorder.iSampleValue = iSampleValue;
            disorder.iCurrentValue = iCurrentValue;

            disorder.Contour = Contour;

            disorder.minRectangle = minRectangle;
        }
    }
}
