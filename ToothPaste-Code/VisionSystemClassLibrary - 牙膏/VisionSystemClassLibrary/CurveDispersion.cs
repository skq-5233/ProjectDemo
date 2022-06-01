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

功能描述：曲线离散度计算工具

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
    public class CurveDispersion
    {
        private UInt16 uiColor;//颜色对比参数
        private UInt16 IntervalWidth;//区间宽度

        private Enum.ProjectionType ProjectionType = new Enum.ProjectionType();//投影类型，0水平，1垂直

        private Struct.ROI rROI;//定义结构体，传输兴趣区域信息
        private Struct.Arithmetic aArithmetic;//算法结构体

        private Int32 iSampleValue;//学习值
        private Int32 iCurrentValue;//当前值

        private Int32 iMin;//公差有效下限
        private Int32 iMax;//公差有效上限

        private Point[] curveProjSrcPoint;//曲线投影点集（画图使用）
        private Point[] curveProjAvePoint;//曲线均值点集（画图使用）

        private int[] curveProjSrc; //曲线投影初始数组
        private int[] curveProjAve; //曲线投影均值数组

        private Rectangle minRectangle;//最小外接矩形

        //-----------------------------------------------------------------------
        // 功能说明：构造函数
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public CurveDispersion()
        {
            rROI = new Struct.ROI();//定义结构体，传输兴趣区域信息
            aArithmetic = new Struct.Arithmetic();//算法结构体
            minRectangle = new Rectangle();

            if (0 == aArithmetic.Number)
            {
                aArithmetic.Number = Convert.ToByte(String.TextData.ArithmeticName_CurveDispersion_CHN.Length);
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
        // 功能说明： 初始化函数,给参数赋值
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        public void _Init()
        {
            uiColor = aArithmetic.EnumCurrent[0];//给颜色赋值
            ProjectionType = (Enum.ProjectionType)(aArithmetic.EnumCurrent[1]);
            IntervalWidth = Convert.ToUInt16(aArithmetic.CurrentValue[2]);

            minRectangle = GeneralFunction._GetMinRect(rROI.roiExtra);
        }

        //-----------------------------------------------------------------------
        // 功能说明： 自学习函数
        // 输入参数： Image<Bgr, Byte>：image，待处理的图像
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        public void _LearnSample(Image<Bgr, Byte> image, Boolean flag = true)
        {
            if ((image != null) && (minRectangle.Right <= image.Width) && (minRectangle.Bottom <= image.Height) && (minRectangle.Width > 0) && (minRectangle.Height > 0))
            {
                Image<Bgr, Byte> imageROI = image.Copy(minRectangle);//获取兴趣区域图像
                GeneralFunction._AndMask(ref imageROI, rROI, minRectangle.Location);

                _Process(imageROI, true, flag);
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
                Image<Bgr, Byte> imageROI = image.Copy(minRectangle);//获取兴趣区域图像
                GeneralFunction._AndMask(ref imageROI, rROI, minRectangle.Location);

                result = _Process(imageROI);
            }
            return result;
        }


        //-----------------------------------------------------------------------
        // 功能说明： 处理函数
        // 输入参数： image:要处理的图像,learnState:自学习状态，true:自学习
        // 输出参数： 无
        // 返 回 值： 处理结果,true:合格,false:不合格
        //----------------------------------------------------------------------
        private Boolean _Process(Image<Bgr, Byte> imageROI, Boolean learnState = false, Boolean flag = true)
        {
            Boolean result = true;
            
            Double currentStandardDeviation = 0.0;

            if (imageROI != null)
            {
                Image<Gray, Byte> imageGray = GeneralFunction._ContrastColor(imageROI, (Enum.Contrast_Color)uiColor);//获取颜色分量

                if (ProjectionType == Enum.ProjectionType.Horizen)  //水平投影
                {
                    Image<Gray, Byte> imageCol = new Image<Gray, byte>(1, imageROI.Height);  //水平投影图像
                    curveProjSrc = new int[imageROI.Height];
                    curveProjAve = new int[imageROI.Height];
                    curveProjSrcPoint = new Point[imageROI.Height];
                    curveProjAvePoint = new Point[imageROI.Height];

                    imageGray.Reduce(imageCol, REDUCE_DIMENSION.SINGLE_COL, REDUCE_TYPE.CV_REDUCE_AVG);//水平投影
                    for (int i = 0; i < imageCol.Height; i++) //图像数据转换为数组
                    {
                        curveProjSrc[i] = Convert.ToInt32(imageCol[i, 0].Intensity);
                    }
                    curveProjAve = _GetAveArray(curveProjSrc, (int)IntervalWidth); //获取平均值数组
                    currentStandardDeviation = _GetStandardDeviation(curveProjSrc, curveProjAve, (int)IntervalWidth);//计算标准差

                    //画图所用点的数组
                    for (int i = 0; i < curveProjSrc.Length; i++)
                    {
                        curveProjSrcPoint[i] = new Point(curveProjSrc[i] + 200, i + minRectangle.Top);
                    }
                    for (int i = 0; i < curveProjAve.Length; i++)
                    {
                        curveProjAvePoint[i] = new Point(curveProjAve[i] + 200, i + minRectangle.Top);
                    }
                }
                else      //垂直投影
                {
                    Image<Gray, Byte> imageRow = new Image<Gray, byte>(imageROI.Width, 1);//垂直投影图像

                    curveProjSrc = new int[imageROI.Width];
                    curveProjAve = new int[imageROI.Width];
                    curveProjSrcPoint = new Point[imageROI.Width];
                    curveProjAvePoint = new Point[imageROI.Width];

                    imageGray.Reduce(imageRow, REDUCE_DIMENSION.SINGLE_ROW, REDUCE_TYPE.CV_REDUCE_AVG);//垂直投影
                    for (int i = 0; i < imageRow.Width; i++) //图像数据转换为数组
                    {
                        curveProjSrc[i] = Convert.ToInt32(imageRow[0, i].Intensity);
                    }
                    curveProjAve = _GetAveArray(curveProjSrc, (int)IntervalWidth); //获取平均值数组
                    currentStandardDeviation = _GetStandardDeviation(curveProjSrc, curveProjAve, (int)IntervalWidth);//计算标准差

                    //画点所用点的数组
                    for (int i = 0; i < curveProjSrc.Length; i++)//原始投影曲线画图
                    {
                        curveProjSrcPoint[i] = new Point(i + minRectangle.Left, (255 - curveProjSrc[i]) + 110);
                    }
                    for (int i = 0; i < curveProjAve.Length; i++)//均值曲线画图
                    {
                        curveProjAvePoint[i] = new Point(i + minRectangle.Left, (255 - curveProjAve[i]) + 110);
                    }
                }

                if (learnState)
                {
                    if (flag)
                    {
                        iSampleValue = (int)(currentStandardDeviation * 10 + 0.5); //扩大10倍整数显示
                    }
                }
                else
                {
                    iCurrentValue = (int)(currentStandardDeviation * 10 + 0.5); //扩大10倍整数显示
                }

                result = ((iCurrentValue >= iMin) && (iCurrentValue <= iMax)) ? true : false;//大于基准值检测结果合格，反之不合格
            }
            return result;
        }


        //-----------------------------------------------------------------------
        // 功能说明： 计算两数组之间的标准差
        // 输入参数： data1：原始数组, data2:均值数组, step:参与计算标准差的数组起始数据位置
        // 输出参数： Double:得到的标准差
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private Double _GetStandardDeviation(int[] data1, int[] data2, int step = 0)
        {
            Double standardDeviation = 0.0;
            //计算标准差
            double sumMM = 0.0;
            for (int i = step; i < data1.Length - step; i++)
            {
                sumMM += (data1[i] - data2[i]) * (data1[i] - data2[i]);
            }
            sumMM = sumMM / (data1.Length - 2 * step);
            standardDeviation = Math.Sqrt(sumMM);

            return standardDeviation;
        }
        
        //-----------------------------------------------------------------------
        // 功能说明： 数组按区间算平均值函数
        // 输入参数： data：原始数组,step:区间宽度
        // 输出参数： int[]:得到的平均值数组
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private int[] _GetAveArray(int[] data, int step)
        {
            int[] resultData = new int[data.Length];

            //区域取平均
            int sumV = 0;
            for (int i = step; i < data.Length - step; i++)
            {
                sumV = 0;
                for (int j = i - step; j < i + step; j++)
                {
                    sumV += data[j];
                }
                resultData[i] = sumV / (2 * step);
            }
            return resultData;
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

                if (null != curveProjSrcPoint)
                {
                    image.DrawPolyline(curveProjSrcPoint, false, new Bgr(0, 255, 0), 1);
                }

                if (null != curveProjAvePoint)
                {
                    image.DrawPolyline(curveProjAvePoint, false, new Bgr(0, 0, 255), 1);
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：CurveDispersion类拷贝函数
        // 输入参数：1.CurveDispersion curvedispersion，curvedispersion参数
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _CopyTo(ref CurveDispersion curvedispersion)
        {
            curvedispersion.uiColor = uiColor;
            curvedispersion.IntervalWidth = IntervalWidth;
            curvedispersion.ProjectionType = ProjectionType;

            curvedispersion.iSampleValue = iSampleValue;
            curvedispersion.iCurrentValue = iCurrentValue;

            curvedispersion.iMin = iMin;
            curvedispersion.iMax = iMax;

            rROI._CopyTo(ref curvedispersion.rROI);
            aArithmetic._CopyTo(ref curvedispersion.aArithmetic);

            curvedispersion.minRectangle = minRectangle;
        }
    }
}
