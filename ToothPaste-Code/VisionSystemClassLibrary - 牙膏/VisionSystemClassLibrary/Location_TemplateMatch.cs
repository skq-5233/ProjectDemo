/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：Location.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：Location工具

原作者：视觉检测团队
完成日期：2020/08/25
特别说明：经视觉检测团队授权并遵守代码使用条款，方可使用本代码并获得技术支持，否则所产生的一切后果由使用者承担

修改者：无
完成日期：无
修改记录：无

****************************************************************/
using System;
using System.Collections.Generic;

using System.Drawing;

using Emgu.CV;
using Emgu.CV.Structure;

using VisionSystemExtendClassLibrary;

namespace VisionSystemClassLibrary
{
    [Serializable]
    class Location_TemplateMatch
    {
        private UInt16 uiTemplatePointX;//区域左边缘
        private UInt16 uiTemplatePointY;//区域右边缘
        private UInt16 uiTemplateWidth;//区域宽度
        private UInt16 uiTemplateHeight;//区域高度

        private Struct.ROI rROI;//定义结构体，传输兴趣区域信息
        private Struct.Arithmetic aArithmetic;//算法结构体

        private Int32[] iSampleValue;//学习值
        private Int32[] iCurrentValue;//当前值

        private Boolean CheckResult = true;//检测结果,true:合格，false：不合格

        private List<Int32> iMin;//公差有效下限
        private List<Int32> iMax;//公差有效上限

        private Boolean bExistTolerance;//存在公差与否,true:存在，false:不存在

        private Image<Bgr, Byte> ImageTemplate;//模板图像

        private Rectangle minRectangle;//最小外接矩形

        public Location_TemplateMatch()
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
                aArithmetic.Number = Convert.ToByte(String.TextData.ArithmeticName_Location_TemplateMatch_CHN.Length);
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
            uiTemplatePointX =  Convert.ToUInt16(aArithmetic.CurrentValue[0]);
            uiTemplatePointY = Convert.ToUInt16(aArithmetic.CurrentValue[1]);
            uiTemplateWidth = Convert.ToUInt16(aArithmetic.CurrentValue[2]);
            uiTemplateHeight = Convert.ToUInt16(aArithmetic.CurrentValue[3]);

            minRectangle = GeneralFunction._GetMinRect(rROI.roiExtra);
        }

        //-----------------------------------------------------------------------
        // 功能说明： 自学习函数
        // 输入参数： 1、Image<Bgr, Byte>：image，待处理的图像
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
                
                image.Draw(new Rectangle(uiTemplatePointX, uiTemplatePointY, uiTemplateWidth, uiTemplateHeight), new Bgr(0, 255, 255), 1);
                image.Draw(new CircleF(new Point(iCurrentValue[0], iCurrentValue[1]), 3), new Bgr(0, 0, 255), -1);
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Location_TemplateMatch类拷贝函数
        // 输入参数：1.Location_TemplateMatch：locationTemplateMatch参数
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _CopyTo(ref Location_TemplateMatch locationTemplateMatch)
        {
            locationTemplateMatch.uiTemplatePointX = uiTemplatePointX;
            locationTemplateMatch.uiTemplatePointY = uiTemplatePointY;
            locationTemplateMatch.uiTemplateWidth = uiTemplateWidth;
            locationTemplateMatch.uiTemplateHeight = uiTemplateHeight;

            locationTemplateMatch.iMin.Clear();
            locationTemplateMatch.iMin.AddRange(iMin);

            locationTemplateMatch.iMax.Clear();
            locationTemplateMatch.iMax.AddRange(iMax);

            locationTemplateMatch.bExistTolerance = bExistTolerance;

            rROI._CopyTo(ref locationTemplateMatch.rROI);
            aArithmetic._CopyTo(ref locationTemplateMatch.aArithmetic);

            locationTemplateMatch.iSampleValue = new Int32[iSampleValue.Length];
            Array.Copy(iSampleValue, locationTemplateMatch.iSampleValue, iSampleValue.Length);

            locationTemplateMatch.iCurrentValue = new Int32[iCurrentValue.Length];
            Array.Copy(iCurrentValue, locationTemplateMatch.iCurrentValue, iCurrentValue.Length);

            locationTemplateMatch.CheckResult = CheckResult;

            if (null != ImageTemplate)
            {
                locationTemplateMatch.ImageTemplate = ImageTemplate.Copy();
            }

            locationTemplateMatch.minRectangle = minRectangle;
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

            if (learnState)
            {
                ImageTemplate = image.Copy(new Rectangle(uiTemplatePointX, uiTemplatePointY, uiTemplateWidth, uiTemplateHeight));//获取兴趣区域图像
            }

            Point maxLoc = MatchTemplate451._MatchTemplate(imageROI.Bitmap, ImageTemplate.Bitmap);

            if (learnState)
            {
                iSampleValue[0] = maxLoc.X + uiTemplateWidth / 2 + minRectangle.Left;
                iSampleValue[1] = maxLoc.Y + uiTemplateHeight / 2 + minRectangle.Top;
            }
            else
            {
                iCurrentValue[0] = maxLoc.X + uiTemplateWidth / 2 + minRectangle.Left;
                iCurrentValue[1] = maxLoc.Y + uiTemplateHeight / 2 + minRectangle.Top;

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
