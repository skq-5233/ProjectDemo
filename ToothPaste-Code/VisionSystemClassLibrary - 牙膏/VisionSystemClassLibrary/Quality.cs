/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：内衬包装检测器
课题令号：41S1337
开发部门：智控事业部

文件名称：Quality.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：Quality工具

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
using Emgu.CV.Structure;

namespace VisionSystemClassLibrary.Class
{
    [Serializable]
    public class Quality
    {
        private Point[,] CellLocation;//小单元格坐标

        private UInt16[,] CellIntensity_Sample;//自学习小单元格平均灰度数值
        private UInt16[,] CellIntensity_Current;//当前小单元格平均灰度数值
        private Boolean[,] CellCorrectState;//小单元格正确与否,true:合格，false:不合格

        private UInt16 uiMaxDifferance = 0;//CellIntensity_Current中与CellIntensity_Sample比较差异最大的值

        private UInt16 cellSize;//单元尺寸参数
        private UInt16 cellSizeTemp = 0;//单元尺寸参数
        private UInt16 hSearch = 0;//水平搜索参数
        private UInt16 vSearch = 0;//垂直搜索参数

        private UInt16 color;//颜色对比参数

        private Int32 iMin;//公差有效下限
        private Int32 iMax;//公差有效上限

        private Struct.ROI rROI;//定义结构体，传输兴趣区域信息
        private Struct.ROI ROITemp;//定义结构体，向格子传输工作区域信息

        private Struct.Arithmetic aArithmetic;//算法结构体
        
        private Image<Gray, Byte> ImageGray;//灰度图像/B/G/R通道图像
        private Image<Gray, Double> ImageIntegral;//积分图像
        
        private Rectangle minRectangle;//最小外接矩形

        //构造函数

        //-----------------------------------------------------------------------
        // 功能说明：构造函数
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Quality()
        {
            rROI = new Struct.ROI();//定义结构体，传输兴趣区域信息
            ROITemp = new Struct.ROI();
            aArithmetic = new Struct.Arithmetic();//算法结构体
            minRectangle = new Rectangle();

            if (0 == aArithmetic.Number)
            {
                aArithmetic.Number = Convert.ToByte(String.TextData.ArithmeticName_Quality_CHN.Length);
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
        // 功能说明：MaxDifferance属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public UInt16 MaxDifferance
        {
            get
            {
                return uiMaxDifferance;
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

        //函数

        //-----------------------------------------------------------------------
        // 功能说明：初始化函数,给参数赋值
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _Init(Boolean state = false)
        {
            cellSize = Convert.ToUInt16(aArithmetic.CurrentValue[0]);
            hSearch = aArithmetic.EnumCurrent[1];
            vSearch = aArithmetic.EnumCurrent[2];
            color = aArithmetic.EnumCurrent[4];

            minRectangle = GeneralFunction._GetMinRect(rROI.roiExtra);

            if (cellSize < 5) //单元格最小尺寸为5
            {
                cellSize = 5;
            }

            if (state) //执行初始化
            {
                CellIntensity_Sample = new UInt16[minRectangle.Height / cellSize, minRectangle.Width / cellSize];
            }
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
                Image<Bgr, Byte> imageROI = image.Copy(minRectangle);//获取兴趣区域图像
                GeneralFunction._AndMask(ref imageROI, rROI, minRectangle.Location);

                ImageGray = GeneralFunction._ContrastColor(imageROI, (Enum.Contrast_Color)color);//调用颜色对比函数进行处理
                ImageIntegral = ImageGray.Integral();//图像积分

                CellIntensity_Sample = new UInt16[imageROI.Height / cellSize, imageROI.Width / cellSize];

                Int32 iRecicleExtra = CellIntensity_Sample.GetLength(0);
                Int32 iRecicleInner = CellIntensity_Sample.GetLength(1);
                Int32 remainderW = (imageROI.Width % cellSize) / 2;
                Int32 remainderH = (imageROI.Height % cellSize) / 2;
                for (int j = 0; j < iRecicleExtra; j++)
                {
                    for (int i = 0; i < iRecicleInner; i++)
                    {
                        Int32 iSearchHeight = j * cellSize + remainderH;
                        Int32 iSearchHeightOffset = cellSize + iSearchHeight;
                        if ((iSearchHeight < 0) || (iSearchHeightOffset > ImageIntegral.Height)) //未超出图像高度
                        {
                            continue;
                        }

                        Int32 iSearchWidth = i * cellSize + remainderW;
                        Int32 iSearchWidthOffset = cellSize + iSearchWidth;
                        if ((iSearchWidth < 0) || (iSearchWidthOffset > ImageIntegral.Width)) //未超出图像宽度
                        {
                            continue;
                        }

                        double point1 = ImageIntegral[iSearchHeight, iSearchWidth].Intensity;//到格子左上角顶点积分值
                        double point2 = ImageIntegral[iSearchHeightOffset, iSearchWidth].Intensity;//到格子右上角顶点积分值
                        double point3 = ImageIntegral[iSearchHeightOffset, iSearchWidthOffset].Intensity;//到格子右下角顶点积分值
                        double point4 = ImageIntegral[iSearchHeight, iSearchWidthOffset].Intensity; //到格子左下角顶点积分值

                        CellIntensity_Sample[j, i] = Convert.ToUInt16((point3 - point4 - point2 + point1) / (cellSize * cellSize));//计算单元格亮度平均值
                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明： 图像处理函数
        // 输入参数： image:要处理的图像
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        public Boolean _ImageProcess(Image<Bgr, Byte> image)
        {
            Boolean result = true;

            if ((image != null) && (minRectangle.Right <= image.Width) && (minRectangle.Bottom <= image.Height) && (minRectangle.Width > 0) && (minRectangle.Height > 0))
            {
                Boolean flag = false;//搜寻合格标志
                Int32 remainderW = 0, remainderH = 0;

                Image<Bgr, Byte> imageROI = image.Copy(minRectangle);//获取兴趣区域图像
                GeneralFunction._AndMask(ref imageROI, rROI, minRectangle.Location);

                ImageGray = GeneralFunction._ContrastColor(imageROI, (Enum.Contrast_Color)color);//调用颜色对比函数进行处理
                ImageIntegral = ImageGray.Integral();//图像积分

                if ((cellSize != cellSizeTemp) || (false == GeneralFunction._IsEqual(rROI, ROITemp)))//如果兴趣区域或者单元格尺寸发生变化，重新计算单元格坐标
                {
                    Int32 iHeight = imageROI.Height / cellSize;
                    Int32 iWidth = imageROI.Width / cellSize;

                    CellIntensity_Current = new UInt16[iHeight, iWidth];
                    CellCorrectState = new Boolean[iHeight, iWidth];

                    CellLocation = new Point[iHeight + 1, iWidth + 1];
                    remainderW = (imageROI.Width % cellSize) / 2;
                    remainderH = (imageROI.Height % cellSize) / 2;
                    Int32 bufX = minRectangle.Left + remainderW;
                    Int32 bufY = minRectangle.Top + remainderH;

                    for (int j = 0; j <= iHeight; j++)
                    {
                        for (int i = 0; i <= iWidth; i++)
                        {
                            CellLocation[j, i].X = bufX + i * cellSize;//计算每个小单元格横坐标
                            CellLocation[j, i].Y = bufY + j * cellSize;//计算每个小单元格纵坐标
                        }
                    }
                    cellSizeTemp = cellSize;
                    rROI._CopyTo(ref ROITemp);
                }

                UInt16 max = 0;
                Int32 iRecicleExtra = CellIntensity_Current.GetLength(0);
                Int32 iRecicleInner = CellIntensity_Current.GetLength(1);
                for (int j = 0; j < iRecicleExtra; j++)
                {
                    for (int i = 0; i < iRecicleInner; i++)
                    {
                        UInt16 AbsDiff = 1000;

                        for (int vOffset = -vSearch; vOffset < vSearch + 1; vOffset++)//垂直方向上下搜寻
                        {
                            Int32 iSearchHeight = (j + vOffset) * cellSize + remainderH;
                            Int32 iSearchHeightOffset = cellSize + iSearchHeight;
                            if ((iSearchHeight < 0) || (iSearchHeightOffset > ImageIntegral.Height)) //未超出图像高度
                            {
                                continue;
                            }

                            for (int hOffset = -hSearch; hOffset < hSearch + 1; hOffset++)//水平方向左右搜寻
                            {
                                Int32 iSearchWidth = (i + hOffset) * cellSize + remainderW;
                                Int32 iSearchWidthOffset = cellSize + iSearchWidth;
                                if ((iSearchWidth < 0) || (iSearchWidthOffset > ImageIntegral.Width)) //未超出图像宽度
                                {
                                    continue;
                                }

                                double point1 = ImageIntegral[iSearchHeight, iSearchWidth].Intensity;//到格子左上角顶点积分值
                                double point2 = ImageIntegral[iSearchHeightOffset, iSearchWidth].Intensity;//到格子右上角顶点积分值
                                double point3 = ImageIntegral[iSearchHeightOffset, iSearchWidthOffset].Intensity;//到格子右下角顶点积分值
                                double point4 = ImageIntegral[iSearchHeight, iSearchWidthOffset].Intensity; //到格子左下角顶点积分值

                                CellIntensity_Current[j, i] = Convert.ToUInt16((point3 - point4 - point2 + point1) / (cellSize * cellSize));//计算单元格亮度平均值

                                UInt16 diff = Convert.ToUInt16((Math.Abs(CellIntensity_Current[j, i] - CellIntensity_Sample[j, i])) * 1000 / 255);//计算与模板格子的差异值
                                if (((1000 - iMax) <= diff) && (diff <= (1000 - iMin)))//找到符合公差的格子，跳出循环
                                {
                                    flag = true;
                                    AbsDiff = diff;
                                    break;
                                }
                                else//否则保存差异最小的值
                                {
                                    if (diff < AbsDiff)
                                    {
                                        AbsDiff = diff;
                                    }
                                    flag = false;
                                }
                            }
                            if (flag)
                            {
                                break;
                            }
                        }
                        CellCorrectState[j, i] = flag;

                        //找到最大差异值
                        if (AbsDiff >= max)
                        {
                            max = AbsDiff;
                            uiMaxDifferance = Convert.ToUInt16(1000 - AbsDiff);
                        }
                    }
                }

                for (int s = 0; s < CellCorrectState.GetLength(0); s++)//判断检测结果，只要一个小格子不合格则结果就不合格
                {
                    for (int p = 0; p < CellCorrectState.GetLength(1); p++)
                    {
                        if (!CellCorrectState[s, p])
                        {
                            result = false;
                            break;
                        }
                    }
                    if (!result)
                    {
                        break;
                    }
                }
            }
            else
            {
                uiMaxDifferance = 0;
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
                if (null != CellLocation)
                {
                    for (int i = 0; i < CellLocation.GetLength(1); i++)
                    {
                        image.Draw(new LineSegment2D(CellLocation[0, i], CellLocation[CellLocation.GetLength(0) - 1, i]), new Bgr(0, 255, 0), 1);
                    }
                }

                if (null != CellLocation)
                {
                    for (int j = 0; j < CellLocation.GetLength(0); j++)
                    {
                        image.Draw(new LineSegment2D(CellLocation[j, 0], CellLocation[j, CellLocation.GetLength(1) - 1]), new Bgr(0, 255, 0), 1);
                    }
                }

                if (null != CellLocation)
                {
                    for (int j = 0; j < CellCorrectState.GetLength(0); j++)
                    {
                        for (int i = 0; i < CellCorrectState.GetLength(1); i++)
                        {
                            if (!CellCorrectState[j, i])
                            {
                                _DrawFork(image, new Point(CellLocation[j, i].X,
                                    CellLocation[j, i].Y), cellSize, new Bgr(0, 0, 255));
                            }
                        }
                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Quality类拷贝函数
        // 输入参数：1.Quality：quality，Quality参数
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _CopyTo(ref Quality quality)
        {
            quality.uiMaxDifferance = uiMaxDifferance;
            quality.cellSize = cellSize;
            quality.cellSizeTemp = cellSizeTemp;
            quality.hSearch = hSearch;
            quality.vSearch = vSearch;
            quality.iMin = iMin;
            quality.iMax = iMax;
            quality.color = color;

            if (null != CellLocation)
            {
                quality.CellLocation = new Point[CellLocation.GetLength(0), CellLocation.GetLength(1)];
                Array.Copy(CellLocation, quality.CellLocation, CellLocation.LongLength);
            }

            if (null != CellIntensity_Sample)
            {
                quality.CellIntensity_Sample = new UInt16[CellIntensity_Sample.GetLength(0), CellIntensity_Sample.GetLength(1)];
                Array.Copy(CellIntensity_Sample, quality.CellIntensity_Sample, CellIntensity_Sample.LongLength);
            }

            if (null != CellIntensity_Current)
            {
                quality.CellIntensity_Current = new UInt16[CellIntensity_Current.GetLength(0), CellIntensity_Current.GetLength(1)];
                Array.Copy(CellIntensity_Current, quality.CellIntensity_Current, CellIntensity_Current.LongLength);
            }

            if (null != CellCorrectState)
            {
                quality.CellCorrectState = new Boolean[CellCorrectState.GetLength(0), CellCorrectState.GetLength(1)];
                Array.Copy(CellCorrectState, quality.CellCorrectState, CellCorrectState.LongLength);
            }

            rROI._CopyTo(ref quality.rROI);
            ROITemp._CopyTo(ref quality.ROITemp);
            aArithmetic._CopyTo(ref quality.aArithmetic);

            quality.minRectangle = minRectangle;
        }

        //-----------------------------------------------------------------------
        // 功能说明： 画叉函数
        // 输入参数： image:待处理图像，point:叉的位置点，size:叉的大小，bgr:叉的颜色
        // 输出参数： image：已处理图像
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private Image<Bgr, Byte> _DrawFork(Image<Bgr, Byte> image, Point point, UInt16 size, Bgr bgr)
        {
            image.Draw(new LineSegment2D(new Point(point.X, point.Y),
                new Point(point.X + size, point.Y + size)), bgr, 1);
            image.Draw(new LineSegment2D(new Point(point.X + size, point.Y),
                new Point(point.X, point.Y + size)), bgr, 1);

            return image;
        }
    }
}