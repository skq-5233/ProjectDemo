/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：内衬包装检测器
课题令号：41S1337
开发部门：智控事业部

文件名称：Location.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：Location工具

原作者：视觉检测团队
完成日期：2020/12/28
特别说明：经视觉检测团队授权并遵守代码使用条款，方可使用本代码并获得技术支持，否则所产生的一切后果由使用者承担

修改者：无
完成日期：无
修改记录：无

****************************************************************/
using System;
using System.Collections.Generic;

using System.Drawing;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace VisionSystemClassLibrary.Class
{
    [Serializable]
    public class Location_Cicle
    {
        private UInt16 uiColor;//颜色对比参数
        private Byte bMaxGray;//最大灰度
        private ADAPTIVE_THRESHOLD_TYPE adaptiveType;//自适应类型
        private THRESH thresholdType;//分割类型
        private Byte bTempleteSize;//模板大小
        private Int16 iFilterParameter;//滤波参数
        private UInt16 uiCannyThreashold;//Canny阈值
        private UInt16 uiAccumulatorParameter;//累加阈值
        private UInt16 uiMinDis;//最小中心距
        private UInt16 uiMinRadius;//最小半径
        private UInt16 uiMaxRadius;//最大半径

        private Struct.ROI rROI;//定义结构体，传输兴趣区域信息
        private Struct.Arithmetic aArithmetic;//算法结构体

        private Int32[] iSampleValue;//学习值
        private Int32[] iCurrentValue;//当前值

        private Boolean CheckResult = true;//检测结果,true:合格，false：不合格

        private List<Int32> iMin;//公差有效下限
        private List<Int32> iMax;//公差有效上限

        private Boolean bExistTolerance;//存在公差与否,true:存在，false:不存在
        
        private Image<Gray, Byte> ImageGray;//灰度图像/B/G/R通道图像

        [NonSerialized]
        private CircleF[][] circleF;//查询得出的圆

        private Rectangle minRectangle;//最小外接矩形

        public Location_Cicle()
        {
            iSampleValue = new Int32[2];
            iCurrentValue = new Int32[2];
            iMin = new List<int>();
            iMax = new List<int>();

            rROI = new Struct.ROI();//定义结构体，传输兴趣区域信息
            aArithmetic = new Struct.Arithmetic();//算法结构体
            minRectangle = new Rectangle();

            if (0 == aArithmetic.Number)
            {
                aArithmetic.Number = Convert.ToByte(String.TextData.ArithmeticName_LocationCicle_CHN.Length);
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
        public Int32[] SampleValue
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
        public Int32[] CurrentValue
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
        // 功能说明：ExistTolerance属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Boolean ExistTolerance
        {
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
        public List<Int32> Min
        {
            get
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
        public List<Int32> Max
        {
            get
            {
                return iMax;
            }
            set
            {
                iMax = value;
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
            bMaxGray = Convert.ToByte(aArithmetic.CurrentValue[1]);
            adaptiveType = (ADAPTIVE_THRESHOLD_TYPE)aArithmetic.EnumCurrent[2];
            THRESH thresholdType = (THRESH)aArithmetic.EnumCurrent[3];
            bTempleteSize = Convert.ToByte(aArithmetic.CurrentValue[4]);
            iFilterParameter = aArithmetic.CurrentValue[5];
            uiCannyThreashold = Convert.ToUInt16(aArithmetic.CurrentValue[6]);
            uiAccumulatorParameter = Convert.ToUInt16(aArithmetic.CurrentValue[7]);
            uiMinDis = Convert.ToUInt16(aArithmetic.CurrentValue[8]);
            uiMinRadius = Convert.ToUInt16(aArithmetic.CurrentValue[9]);
            uiMaxRadius = Convert.ToUInt16(aArithmetic.CurrentValue[10]);

            minRectangle = GeneralFunction._GetMinRect(rROI.roiExtra);
        }

        //-----------------------------------------------------------------------
        // 功能说明： 自学习函数
        // 输入参数： 1、Image<Bgr, Byte>：image，待处理的图像
        //            2、Boolean：flag，自学习后是否更新最大最小值
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        public void _LearnSample(Image<Bgr, Byte> image)
        {
            if ((image != null) && (minRectangle.Right <= image.Width) && (minRectangle.Bottom <= image.Height) && (minRectangle.Width > 0) && (minRectangle.Height > 0))
            {
                _Process(image, true);
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
                GeneralFunction._DrawGraphics(ref image, rROI, new Bgr(0, 255, 0));

                Bgr color = CheckResult ? new Bgr(0, 255, 0) : new Bgr(0, 0, 255);//给颜色赋值

                if (false == bExistTolerance) //不存在公差
                {
                    color = new Bgr(0, 255, 0);
                }

                for (Int32 i = 0; i < circleF[0].Length; i++)
                {
                    image.Draw(new CircleF(circleF[0][i].Center, 1), new Bgr(0, 255, 0), 1);
                }
                image.Draw(new Rectangle(iCurrentValue[0] - 5, iCurrentValue[1] - 1, 11, 3), new Bgr(0, 0, 255), -1);
                image.Draw(new Rectangle(iCurrentValue[0] - 1, iCurrentValue[1] - 5, 3, 11), new Bgr(0, 0, 255), -1);
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Location_Cicle类拷贝函数
        // 输入参数：1.Location_Cicle：locationCicle参数
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _CopyTo(ref Location_Cicle locationCicle)
        {
            locationCicle.uiColor = uiColor;
            locationCicle.bMaxGray = bMaxGray;
            locationCicle.adaptiveType = adaptiveType;
            locationCicle.thresholdType = thresholdType;
            locationCicle.bTempleteSize = bTempleteSize;
            locationCicle.iFilterParameter = iFilterParameter;
            locationCicle.uiCannyThreashold = uiCannyThreashold;
            locationCicle.uiAccumulatorParameter = uiAccumulatorParameter;
            locationCicle.uiMinDis= uiMinDis;
            locationCicle.uiMinRadius = uiMinRadius;
            locationCicle.uiMaxRadius = uiMaxRadius;

            locationCicle.iMin.Clear();
            locationCicle.iMin.AddRange(iMin);

            locationCicle.iMax.Clear();
            locationCicle.iMax.AddRange(iMax);

            locationCicle.bExistTolerance = bExistTolerance;
            
            rROI._CopyTo(ref locationCicle.rROI);
            aArithmetic._CopyTo(ref locationCicle.aArithmetic);

            locationCicle.iSampleValue = new Int32[iSampleValue.Length];
            Array.Copy(iSampleValue, locationCicle.iSampleValue, iSampleValue.Length);

            locationCicle.iCurrentValue = new Int32[iCurrentValue.Length];
            Array.Copy(iCurrentValue, locationCicle.iCurrentValue, iCurrentValue.Length);

            locationCicle.CheckResult = CheckResult;

            locationCicle.minRectangle = minRectangle;
        }

        //-----------------------------------------------------------------------
        // 功能说明： 图像处理函数
        // 输入参数： image：待处理的图像,learnState:自学习状态，true:自学习，false:实时
        // 输出参数： 无
        // 返 回 值： result:检测结果，true:合格，false:不合格
        //----------------------------------------------------------------------
        private Boolean _Process(Image<Bgr, Byte> image, Boolean learnState = false)
        {
            if (learnState)
            {
                for (Int32 i = 0; i < iSampleValue.Length; i++)
                {
                    iSampleValue[i] = 0;
                }
            }
            else
            {
                for (Int32 i = 0; i < iCurrentValue.Length; i++)
                {
                    iCurrentValue[i] = 0;
                }
            }

            Boolean result = true;

            Image<Bgr, Byte> imageROI = image.Copy(minRectangle);//获取兴趣区域图像
            GeneralFunction._AndMask(ref imageROI, rROI, minRectangle.Location);

            ImageGray = GeneralFunction._ContrastColor(imageROI, (Enum.Contrast_Color)uiColor);//调用颜色对比函数进行处理

            ImageGray = ImageGray.ThresholdAdaptive(new Gray(bMaxGray), adaptiveType, thresholdType, bTempleteSize, new Gray(iFilterParameter));

            circleF = ImageGray.HoughCircles(new Gray(uiCannyThreashold), new Gray(uiAccumulatorParameter), 1, uiMinDis, uiMinRadius, uiMaxRadius);
            double sum_x = 0, sum_y = 0;
            Int32 count = 0;
            for (Int32 i = 0; i < circleF[0].Length; i++)
            {
                sum_x += circleF[0][i].Center.X;
                sum_y += circleF[0][i].Center.Y;
                count++;
            }

            if (learnState)
            {
                if (count == 0)
                {
                    iSampleValue[0] = 0;
                    iSampleValue[1] = 0;
                }
                else
                {
                    iSampleValue[0] = (Int32)sum_x / count + minRectangle.Left;
                    iSampleValue[1] = (Int32)sum_y / count + minRectangle.Top;
                }
            }
            else
            {
                if (count == 0)
                {
                    iCurrentValue[0] = 0;
                    iCurrentValue[1] = 0;
                }
                else
                {
                    iCurrentValue[0] = (Int32)sum_x / count + minRectangle.Left;
                    iCurrentValue[1] = (Int32)sum_y / count + minRectangle.Top;
                }

                for (Byte i = 0; i < iMin.Count; i++)
                {
                    if ((iCurrentValue[i] >= iMin[i]) && (iCurrentValue[i] <= iMax[i]))//结果在公差范围内
                    {
                        result = true;
                    }
                    else//结果在公差范围外
                    {
                        result = ((!bExistTolerance) || false);//不存在公差时置true
                    }
                }
            }
            return result;//返回检测结果
        }
    }
}
