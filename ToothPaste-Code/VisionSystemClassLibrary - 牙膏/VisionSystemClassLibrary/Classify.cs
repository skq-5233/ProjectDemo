/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：Cassify.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：Cassify工具

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

using VisionSystemExtendClassLibrary;

namespace VisionSystemClassLibrary
{
    [Serializable]
    class Classify
    {
        private UInt16 uiModelWidth;//区域宽度
        private UInt16 uiModelHeight;//区域高度

        private Struct.ROI rROI;//定义结构体，传输兴趣区域信息
        private Struct.Arithmetic aArithmetic;//算法结构体

        private Int32 iSampleValue;//学习值
        private Int32 iCurrentValue;//当前值

        private Boolean CheckResult = true;//检测结果,true:合格，false：不合格

        private Int32 iMin;//公差有效下限
        private Int32 iMax;//公差有效上限

        private double maxValue;//相似性
        private string typeName;//类别
        
        [NonSerialized]
        private DNN_OpenVINO dnn;//深度学习（分类）

        private Rectangle minRectangle;//最小外接矩形

        public Classify()
        {
            rROI = new Struct.ROI();//定义结构体，传输兴趣区域信息
            aArithmetic = new Struct.Arithmetic();//算法结构体
            minRectangle = new Rectangle();

            if (0 == aArithmetic.Number)
            {
                aArithmetic.Number = Convert.ToByte(String.TextData.ArithmeticName_Classify_CHN.Length);
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

            dnn = new DNN_OpenVINO();
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
        // 功能说明：Min属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Int32 Min
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
        public Int32 Max
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
            uiModelWidth = Convert.ToUInt16(aArithmetic.CurrentValue[0]);
            uiModelHeight = Convert.ToUInt16(aArithmetic.CurrentValue[1]);

            minRectangle = GeneralFunction._GetMinRect(rROI.roiExtra);
        }

        //-----------------------------------------------------------------------
        // 功能说明： 初始化函数,给参数赋值
        // 输入参数： 1、string：strTypeFileName，分类文件路径
        //                    2、string：strTfPbFileName，模型文件路径
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        public void _NetInit(string strTypeFileName, string strTfPbFileName)
        {
            dnn._NetInit(strTypeFileName, strTfPbFileName, (Int32)Enum.Backend.InferenceEngine, (Byte)Enum.Target.Cpu, uiModelWidth, uiModelHeight);
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
                Bgr color = CheckResult ? new Bgr(0, 255, 0) : new Bgr(0, 0, 255);//给颜色赋值

                GeneralFunction._DrawGraphics(ref image, rROI, new Bgr(0, 255, 255));

                MCvFont font = new MCvFont(FONT.CV_FONT_HERSHEY_COMPLEX, 1.5, 1.5);
                MCvFont font1 = new MCvFont(FONT.CV_FONT_HERSHEY_COMPLEX, 1.0, 1.0);

                if (CheckResult)//检测结果完好
                {
                    image.Draw("OK", ref font, new Point(670, 55), color);
                }
                else
                {
                    image.Draw("NG", ref font, new Point(670, 55), color);
                }
                image.Draw(maxValue.ToString("f2"), ref font1, new Point(670, 85), color);
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Classify类拷贝函数
        // 输入参数：1.Classify：classify参数
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _CopyTo(ref Classify classify)
        {
            classify.uiModelWidth = uiModelWidth;
            classify.uiModelHeight = uiModelHeight;

            classify.iMin = iMin;
            classify.iMax = iMax;

            rROI._CopyTo(ref classify.rROI);
            aArithmetic._CopyTo(ref classify.aArithmetic);

            classify.iSampleValue = iSampleValue;
            classify.iCurrentValue = iCurrentValue;

            classify.CheckResult = CheckResult;

            classify.maxValue = maxValue;
            classify.typeName = typeName;

            if (null != dnn)
            {
                dnn._CopyTo(ref classify.dnn);
            }

            classify.minRectangle = minRectangle;
        }

        //-----------------------------------------------------------------------
        // 功能说明： 图像处理函数
        // 输入参数： image：待处理的图像,learnState:自学习状态，true:自学习，false:实时
        // 输出参数： 无
        // 返 回 值： result:检测结果，true:合格，false:不合格
        //----------------------------------------------------------------------
        private Boolean _Process(Image<Bgr, Byte> image, Boolean learnState = false)
        {
            Boolean result = true;

            Image<Bgr, Byte> imageROI = image.Copy(minRectangle);//获取兴趣区域图像
            GeneralFunction._AndMask(ref imageROI, rROI, minRectangle.Location);

            maxValue = 0.0;
            typeName = "";
            Point maxLoc = new Point();

            dnn._ImageProcessingOne(imageROI.ToBitmap(), ref maxValue, ref maxLoc, ref typeName);
           
            Int32 score = Convert.ToInt32(100 * maxValue);

            if (learnState)
            {
                iSampleValue = score;
            }
            else
            {
                iCurrentValue = score;

                if ((iCurrentValue >= iMin) && (iCurrentValue <= iMax) )//结果在公差范围内
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            return result;//返回检测结果
        }
    }
}
