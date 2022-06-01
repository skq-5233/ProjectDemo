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
    public class Line
    {
        private Boolean Increase;//图像处理方向，true:增量方向，false:减量方向
        private Boolean Horizen;//图像处理方向，true:水平方向，false:垂直方向

        private UInt16 Direction;//扫描方向参数
        private UInt16 LineThreshold1;//基准值一
        private UInt16 LineThreshold2;//基准值二
        private UInt16 PaperThreshold;//接头基准

        private UInt16 uiLineWidth;//拉线宽度
        private UInt16 uiSamplePos;//拉线模板位置
        private Int32 iCurrentPos;//拉线实时位置
        private Double LineOffsetMM;//拉线实时偏移毫米距离

        private UInt16 CheckPosLeft;//检测左边界
        private UInt16 CheckPosRight;//检测右边界

        private Point[] CurvePoint = new Point[330];//曲线图点集

        private UInt16 OriginX;//图像起始横坐标
        private UInt16 OriginY;//图像起始纵坐标
        private UInt16 OriginW;//图像起始宽度
        private UInt16 OriginH;//图像起始高度

        private Int32 iMin;//公差有效下限
        private Int32 iMax;//公差有效上限

        private Struct.ROI rROI;//定义结构体，传输兴趣区域信息
        private Struct.Arithmetic aArithmetic;//算法结构体

        private Rectangle minRectangle;//最小外接矩形

        //构造函数

        //-----------------------------------------------------------------------
        // 功能说明：构造函数
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Line()
        {
            rROI = new Struct.ROI();//定义结构体，传输兴趣区域信息
            aArithmetic = new Struct.Arithmetic();//算法结构体
            minRectangle = new Rectangle();

            if (0 == aArithmetic.Number)
            {
                aArithmetic.Number = Convert.ToByte(String.TextData.ArithmeticName_Line_CHN.Length);
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
        // 功能说明：LineWidth属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public UInt16 LineWidth
        {
            set
            {
                uiLineWidth = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：CurrentPos属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Int32 CurrentPos
        {
            get
            {
                return iCurrentPos;
            }
        }

        //-----------------------------------------------------------------------
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

        //函数

        //-----------------------------------------------------------------------
        // 功能说明： 初始化函数,给参数赋值
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        public void _Init()
        {
            Direction = aArithmetic.EnumCurrent[0];//给扫描方向赋值
            LineThreshold1 = Convert.ToUInt16(aArithmetic.CurrentValue[1]);//给基准值一赋值
            LineThreshold2 = Convert.ToUInt16(aArithmetic.CurrentValue[2]);//给基准值二赋值
            PaperThreshold = Convert.ToUInt16(aArithmetic.CurrentValue[3]);//给接头基准赋值

            CheckPosLeft = (UInt16)(uiSamplePos - uiLineWidth - (UInt16)((iMax * uiLineWidth / 25)));
            CheckPosRight = (UInt16)(uiSamplePos + (UInt16)((iMax * uiLineWidth / 25)));

            minRectangle = GeneralFunction._GetMinRect(rROI.roiExtra);
        }

        //-----------------------------------------------------------------------
        // 功能说明： 自学习函数
        // 输入参数： image:要处理的图像
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        public void _LearnSample(Image<Bgr, Byte> image)
        {
            if ((image != null) && (minRectangle.Right <= image.Width) && (minRectangle.Bottom <= image.Height) && (minRectangle.Width > 0) && (minRectangle.Height > 0))
            {
                _ScanDirection();

                Image<Gray, Byte> imageGray = image.Copy(minRectangle).Convert<Gray, Byte>();  //将源图像灰度化

                if (Horizen)//拉线处理
                {
                    Image<Gray, Byte> imageRow = new Image<Gray, Byte>(imageGray.Width, 1);
                    imageGray.Reduce(imageRow, REDUCE_DIMENSION.SINGLE_ROW, REDUCE_TYPE.CV_REDUCE_AVG);//将灰度图像垂直投影

                    int i = 0;
                    if (Increase)//从左向右,增量方向
                    {
                        for (i = 0; i < imageRow.Width; i++)//第一遍在整个兴趣区域内查找
                        {
                            if (Convert.ToUInt16(imageRow[0, i].Intensity) > LineThreshold2)
                            {
                                uiSamplePos = (UInt16)(i + minRectangle.Left);
                                break;
                            }
                        }
                    }
                    else//从右向左,减量方向
                    {
                        for (i = imageRow.Width - 1; i >= 0; i--)//第一遍在整个兴趣区域内查找
                        {
                            if (Convert.ToUInt16(imageRow[0, i].Intensity) > LineThreshold2)
                            {
                                uiSamplePos = (UInt16)(i + minRectangle.Left);
                                break;
                            }
                        }
                    }
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
                Image<Bgr, Byte> imageROI = image.Copy(minRectangle);//获取兴趣区域图像
                GeneralFunction._AndMask(ref imageROI, rROI, minRectangle.Location);

                result = _Process(imageROI);
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
                if (Horizen)//拉线处理
                {
                    image.Draw(new LineSegment2D(new Point(CheckPosLeft, 0), new Point(CheckPosLeft, image.Height)), new Bgr(0, 255, 0), 1);
                    image.Draw(new LineSegment2D(new Point(CheckPosRight, 0), new Point(CheckPosRight, image.Height)), new Bgr(0, 255, 0), 1);
                }

                Point StartPoint = new Point(414, 435);//坐标轴起始点坐标

                Int32 X_Y_Axis_Width = 20;//刻度线长度

                Int32 X_Axis_Pixel = 100;//横坐标刻度距离像素数
                Int32 X_Axis_Height = StartPoint.Y + 25;//横坐标文本纵坐标

                Int32 Y_Axis_Pixel = 80;//纵坐标刻度距离像素数
                Int32 Y_Axis_Width = StartPoint.X - 38;//纵坐标文本横坐标

                Int32 Front_Height = 5;//纵坐标文本向下偏移像素
                Int32 Front_Width = 15;//横坐标文本向左偏移像素12

                Bgr Axis_BGR = new Bgr(0, 255, 255);//坐标轴颜色
                Int32 Axis_Width = 2;//坐标轴线宽

                image.Draw(new LineSegment2D(new Point(StartPoint.X, StartPoint.Y - 5 * Y_Axis_Pixel), StartPoint), Axis_BGR, Axis_Width);
                image.Draw(new LineSegment2D(StartPoint, new Point(image.Width, StartPoint.Y)), Axis_BGR, Axis_Width);

                Emgu.CV.Structure.MCvFont font = new Emgu.CV.Structure.MCvFont(FONT.CV_FONT_HERSHEY_DUPLEX, 0.5, 0.5);
                image.Draw("0", ref font, new Point(StartPoint.X - Front_Width, X_Axis_Height), Axis_BGR);

                image.Draw(new LineSegment2D(new Point(StartPoint.X + X_Axis_Pixel, StartPoint.Y), new Point(StartPoint.X + X_Axis_Pixel, StartPoint.Y - X_Y_Axis_Width)), Axis_BGR, Axis_Width);
                image.Draw("100", ref font, new Point(StartPoint.X + X_Axis_Pixel - Front_Width, X_Axis_Height), new Bgr(0, 255, 255));

                image.Draw(new LineSegment2D(new Point(StartPoint.X + 2 * X_Axis_Pixel, StartPoint.Y), new Point(StartPoint.X + 2 * X_Axis_Pixel, StartPoint.Y - X_Y_Axis_Width)), Axis_BGR, Axis_Width);
                image.Draw("200", ref font, new Point(StartPoint.X + 2 * X_Axis_Pixel - Front_Width, X_Axis_Height), new Bgr(0, 255, 255));

                image.Draw(new LineSegment2D(new Point(StartPoint.X + 3 * X_Axis_Pixel, StartPoint.Y), new Point(StartPoint.X + 3 * X_Axis_Pixel, StartPoint.Y - X_Y_Axis_Width)), Axis_BGR, Axis_Width);
                image.Draw("300", ref font, new Point(StartPoint.X + 3 * X_Axis_Pixel - Front_Width, X_Axis_Height), new Bgr(0, 255, 255));

                image.Draw(new LineSegment2D(new Point(StartPoint.X, StartPoint.Y - Y_Axis_Pixel), new Point(StartPoint.X + X_Y_Axis_Width, StartPoint.Y - Y_Axis_Pixel)), Axis_BGR, Axis_Width);
                image.Draw("50", ref font, new Point(Y_Axis_Width, StartPoint.Y - Y_Axis_Pixel + Front_Height), Axis_BGR);

                image.Draw(new LineSegment2D(new Point(StartPoint.X, StartPoint.Y - 2 * Y_Axis_Pixel), new Point(StartPoint.X + X_Y_Axis_Width, StartPoint.Y - 2 * Y_Axis_Pixel)), Axis_BGR, Axis_Width);
                image.Draw("100", ref font, new Point(Y_Axis_Width, StartPoint.Y - 2 * Y_Axis_Pixel + Front_Height), Axis_BGR);

                image.Draw(new LineSegment2D(new Point(StartPoint.X, StartPoint.Y - 3 * Y_Axis_Pixel), new Point(StartPoint.X + X_Y_Axis_Width, StartPoint.Y - 3 * Y_Axis_Pixel)), Axis_BGR, Axis_Width);
                image.Draw("150", ref font, new Point(Y_Axis_Width, StartPoint.Y - 3 * Y_Axis_Pixel + Front_Height), Axis_BGR);

                image.Draw(new LineSegment2D(new Point(StartPoint.X, StartPoint.Y - 4 * Y_Axis_Pixel), new Point(StartPoint.X + X_Y_Axis_Width, StartPoint.Y - 4 * Y_Axis_Pixel)), Axis_BGR, Axis_Width);
                image.Draw("200", ref font, new Point(Y_Axis_Width, StartPoint.Y - 4 * Y_Axis_Pixel + Front_Height), Axis_BGR);

                image.Draw(new LineSegment2D(new Point(StartPoint.X, StartPoint.Y - 5 * Y_Axis_Pixel), new Point(StartPoint.X + X_Y_Axis_Width, StartPoint.Y - 5 * Y_Axis_Pixel)), Axis_BGR, Axis_Width);
                image.Draw("250", ref font, new Point(Y_Axis_Width, StartPoint.Y - 5 * Y_Axis_Pixel + Front_Height), Axis_BGR);

                image.ROI = new Rectangle(StartPoint.X, StartPoint.Y - 408, minRectangle.Width, 408);
                image.DrawPolyline(CurvePoint, false, new Bgr(0, 0, 255), 1);
                image.ROI = Rectangle.Empty;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明： 扫描方向
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _ScanDirection()
        {
            switch ((Enum.Scan_Direction)Direction + 1)
            {
                case Enum.Scan_Direction.Left_Right://水平方向，增量方向
                default:
                    Increase = true;
                    Horizen = true;
                    break;
                case Enum.Scan_Direction.Right_Left://水平方向，减量方向
                    Increase = false;
                    Horizen = true;
                    break;
                case Enum.Scan_Direction.Top_Bottom://垂直方向，增量方向
                    Increase = true;
                    Horizen = false;
                    break;
                case Enum.Scan_Direction.Bottom_Top://垂直方向，减量方向
                    Increase = false;
                    Horizen = false;
                    break;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明： 处理函数
        // 输入参数： image:要处理的图像
        // 输出参数： 无
        // 返 回 值： 处理结果,true:合格,false:不合格
        //----------------------------------------------------------------------
        private Boolean _Process(Image<Bgr, Byte> imageROI)
        {
            Boolean result = true;

            if (imageROI != null)
            {
                _ScanDirection();

                Image<Gray, Byte> imageGray = imageROI.Convert<Gray, Byte>();  //将源图像灰度化

                if (Horizen)//拉线处理
                {
                    Image<Gray, Byte> imageRow = new Image<Gray, Byte>(imageROI.Width, 1);
                    imageGray.Reduce(imageRow, REDUCE_DIMENSION.SINGLE_ROW, REDUCE_TYPE.CV_REDUCE_AVG);//将灰度图像垂直投影

                    int i = 0;
                    Int32 currentPos = 0;
                    if (Increase)//从左向右,增量方向
                    {
                        for (i = 0; i < imageRow.Width; i++)//第一遍在整个兴趣区域内查找
                        {
                            if (Convert.ToUInt16(imageRow[0, i].Intensity) > LineThreshold1)
                            {
                                currentPos = (UInt16)(i + minRectangle.Left);
                                break;
                            }
                        }

                        if (i >= imageRow.Width)//第二遍在检测边界内查找
                        {
                            for (i = CheckPosLeft - minRectangle.Left; i < CheckPosRight - minRectangle.Left; i++)
                            {
                                if (Convert.ToUInt16(imageRow[0, i].Intensity) > LineThreshold2)
                                {
                                    currentPos = (UInt16)(i + minRectangle.Left);
                                    break;
                                }
                            }
                        }
                    }
                    else//从右向左,减量方向
                    {
                        for (i = imageRow.Width - 1; i >= 0; i--)//第一遍在整个兴趣区域内查找
                        {
                            if (Convert.ToUInt16(imageRow[0, i].Intensity) > LineThreshold1)
                            {
                                currentPos = (UInt16)(i + minRectangle.Left);
                                break;
                            }
                        }

                        if (i < 0)//第二遍在检测边界内查找
                        {
                            for (i = CheckPosRight - minRectangle.Left; i > CheckPosLeft - minRectangle.Left; i--)
                            {
                                if (Convert.ToUInt16(imageRow[0, i].Intensity) > LineThreshold2)
                                {
                                    currentPos = (UInt16)(i + minRectangle.Left);
                                    break;
                                }
                            }
                        }
                    }
                    LineOffsetMM = Math.Round((2.5 * Convert.ToUInt16(Math.Abs(currentPos - uiSamplePos))) / uiLineWidth, 1);
                    iCurrentPos = Convert.ToInt32(10 * LineOffsetMM);

                    result = ((iCurrentPos >= iMin) && (iCurrentPos <= iMax));
                    CurvePoint = new Point[imageRow.Width]; 
                    for (int j = 0; j < imageRow.Width; j++)
                    {
                        CurvePoint[j] = new Point(j, Convert.ToInt32(255 * 1.6 - 1.6 * imageRow[0, j].Intensity));
                    }
                }                                                                                                                                                                                                                                                                                                   
                else//接头处理
                {
                    Double[] MaxVertical, MinVertical;
                    Point[] minVerticalLocations, maxVerticalLocation;

                    Image<Gray, Byte> imageCol = new Image<Gray, Byte>(1, imageROI.Height);
                    imageGray.Reduce(imageCol, REDUCE_DIMENSION.SINGLE_COL, REDUCE_TYPE.CV_REDUCE_AVG);//将灰度图像水平投影
                    imageCol.MinMax(out MinVertical, out MaxVertical, out minVerticalLocations, out maxVerticalLocation);
                    result = (MaxVertical[0] > PaperThreshold + MinVertical[0]) ? false : true;

                    CurvePoint = new Point[imageCol.Height];
                    for (int j = 0; j < imageCol.Height; j++)
                    {
                        CurvePoint[j] = new Point(j, Convert.ToInt32(255 * 1.6 - 1.6 * imageCol[j, 0].Intensity));
                    }
                }
            }
            return result;
        }

        //-----------------------------------------------------------------------
        // 功能说明：Line类拷贝函数
        // 输入参数：1.Line：line，Line参数
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _CopyTo(ref Line line)
        {
            line.Increase = Increase;
            line.Horizen = Horizen;

            line.Direction = Direction;
            line.LineThreshold1 = LineThreshold1;
            line.LineThreshold2 = LineThreshold2;
            line.PaperThreshold = PaperThreshold;

            line.uiLineWidth = uiLineWidth;
            line.uiSamplePos = uiSamplePos;
            line.iCurrentPos = iCurrentPos;
            line.LineOffsetMM = LineOffsetMM;

            line.CheckPosLeft = CheckPosLeft;
            line.CheckPosRight = CheckPosRight;

            line.OriginX = OriginX;
            line.OriginY = OriginY;
            line.OriginW = OriginW;
            line.OriginH = OriginH;

            line.iMin = iMin;
            line.iMax = iMax;

            rROI._CopyTo(ref line.rROI);
            aArithmetic._CopyTo(ref line.aArithmetic);

            line.CurvePoint = new Point[CurvePoint.LongLength];
            Array.Copy(CurvePoint, line.CurvePoint, CurvePoint.Length);

            line.minRectangle = minRectangle;
        }
    }
}
