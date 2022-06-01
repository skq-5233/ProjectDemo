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
    public class Ruler
    {
        private Boolean Increase;//图像处理方向，true:增量方向，false:减量方向
        private Boolean In_Decrease;//图像处理方向，true:从两边向中间查找，false:单方向查找
        private Boolean Horizen;//图像处理方向，true:水平方向，false:垂直方向

        public UInt16 PosSmall_Sample;//自学习找到的前缘位置
        public UInt16 PosBig_Sample;//自学习找到的后缘位置
        public UInt16 PosSmall_Current;//当前找到的前缘位置
        public UInt16 PosBig_Current;//当前找到的后缘位置
        private Int32 Result_Current_Temp;//当前找到的长度、位置或计数缓存
        
        private Int32 iResult_Sample;//自学习找到的长度、位置或计数
        private Int32 iResult_Current;//当前找到的长度、位置或计数

        private UInt16 color;//颜色对比参数
        private UInt16 direction;//扫描方向参数
        private Int16 angleRotate;//角度调整参数
        private UInt16 check;//检查类型参数
        private UInt16 edge;//边缘宽度参数
        private UInt16 distance;//距离参数
        private UInt16 delta;//德尔塔值
        private UInt16 intensity_Min = 0;//像素强度下限
        private UInt16 intensity_Max = 255;//像素强度上限
        private UInt16 detect;//检测方法
        
        private Int32 iMin;//公差有效下限
        private Int32 iMax;//公差有效上限

        private Boolean bExistTolerance;//存在公差与否,true:存在，false:不存在

        private Struct.ROI rROI;//定义结构体，传输兴趣区域信息
        private Struct.Arithmetic aArithmetic;//算法结构体

        private Rectangle ROIPro;//定义图像处理区域
        
        [NonSerialized]
        private Contour<Point> ContourPixelCount;//定义像素计数轮廓序列

        private Image<Gray, Byte> ImageGray;//灰度图像/B/G/R通道图像
        
        private Boolean bReferenceH_Exist;//是否存在水平参考基准,true:存在,false:不存在
        private Boolean bReferenceV_Exist;//是否存在垂直参考基准,true:存在,false:不存在

        private Int32 iReferenceHorizenPoint;//基准定位点水平基准坐标
        private Int32 iReferenceVerticalPoint;//基准定位点垂直基准标准
        
        private UInt16 uiImageWidth = 744; //图像宽度
        private UInt16 uiImageHeight = 480;//图像高度

        private Rectangle minRectangle;//最小外接矩形

        //构造函数

        //-----------------------------------------------------------------------
        // 功能说明：构造函数
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Ruler()
        {
            rROI = new Struct.ROI();//定义结构体，传输兴趣区域信息
            ROIPro = new Rectangle();
            minRectangle = new Rectangle();
            aArithmetic = new Struct.Arithmetic();//算法结构体

            if (0 == aArithmetic.Number)
            {
                aArithmetic.Number = Convert.ToByte(VisionSystemClassLibrary.String.TextData.ArithmeticName_Ruler_CHN.Length);
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
            aArithmetic.EnumState = new Boolean[aArithmetic.Number, VisionSystemClassLibrary.String.TextData.EnumType_CHN.Length];
            aArithmetic.EnumCurrent = new Byte[aArithmetic.Number];
        }

        //属性

        //-----------------------------------------------------------------------
        // 功能说明：ReferenceHorizenPoint属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Int32 ReferenceHorizenPoint
        {
            set
            {
                iReferenceHorizenPoint = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ReferenceVerticalPoint属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Int32 ReferenceVerticalPoint
        {
            set
            {
                iReferenceVerticalPoint = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ReferenceH_Exist属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Boolean ReferenceH_Exist
        {
            set
            {
                bReferenceH_Exist = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ReferenceV_Exist属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Boolean ReferenceV_Exist
        {
            set
            {
                bReferenceV_Exist = value;
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
        // 功能说明：ImageWidth属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public UInt16 ImageWidth
        {
            get//读取
            {
                return uiImageWidth;
            }
            set
            {
                uiImageWidth = value;
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
            get//读取
            {
                return uiImageHeight;
            }
            set
            {
                uiImageHeight = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Result_Current属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Int32 Result_Current
        {
            get
            {
                return iResult_Current;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：Result_Sample属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Int32 Result_Sample
        {
            get
            {
                return iResult_Sample;
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
        // 功能说明： 初始化函数,给参数赋值
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        public void _Init()
        {
            color = aArithmetic.EnumCurrent[0];
            direction = aArithmetic.EnumCurrent[1];
            angleRotate = (Int16)aArithmetic.CurrentValue[2];
            detect = Convert.ToUInt16(aArithmetic.EnumCurrent[3]);
            check = aArithmetic.EnumCurrent[4];
            edge = Convert.ToUInt16(aArithmetic.CurrentValue[5]);
            distance = Convert.ToUInt16(aArithmetic.CurrentValue[6]);
            delta = Convert.ToUInt16(aArithmetic.CurrentValue[7]);
            intensity_Min = Convert.ToUInt16(aArithmetic.CurrentValue[8]);
            intensity_Max = Convert.ToUInt16(aArithmetic.CurrentValue[9]);

            if ((Int32)Enum.Detect_Type.Gradient != (detect + 1))
            {
                angleRotate = 0;
            }
            _ROIProCalculate();
        }

        //-----------------------------------------------------------------------
        // 功能说明：Ruler类拷贝函数
        // 输入参数：1.Ruler：ruler，Ruler参数
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _CopyTo(ref Ruler ruler)
        {
            ruler.Increase = Increase;
            ruler.In_Decrease = In_Decrease;
            ruler.Horizen = Horizen;
            ruler.PosSmall_Sample = PosSmall_Sample;
            ruler.PosBig_Sample = PosBig_Sample;
            ruler.iResult_Sample = iResult_Sample;
            ruler.PosSmall_Current = PosSmall_Current;
            ruler.PosBig_Current = PosBig_Current;
            ruler.iResult_Current = iResult_Current;
            ruler.Result_Current_Temp = Result_Current_Temp;
            ruler.color = color;
            ruler.direction = direction;
            ruler.angleRotate = angleRotate;
            ruler.check = check;
            ruler.edge = edge;
            ruler.distance = distance;
            ruler.delta = delta;
            ruler.intensity_Min = intensity_Min;
            ruler.intensity_Max = intensity_Max;
            ruler.detect = detect;

            ruler.iMin = iMin;
            ruler.iMax = iMax;
            ruler.bExistTolerance = bExistTolerance;

            ruler.bReferenceH_Exist = bReferenceH_Exist;
            ruler.bReferenceV_Exist = bReferenceV_Exist;

            rROI._CopyTo(ref ruler.rROI);
            ruler.ROIPro = ROIPro;
            
            aArithmetic._CopyTo(ref ruler.aArithmetic);
            ruler.ContourPixelCount = ContourPixelCount;
            
            ruler.iReferenceHorizenPoint = iReferenceHorizenPoint;
            ruler.iReferenceVerticalPoint = iReferenceVerticalPoint;
            
            ruler.minRectangle = minRectangle;
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
                Image<Bgr, Byte> imageROI;
                if (0 == angleRotate)
                {
                    imageROI = image.Copy(minRectangle);//获取兴趣区域图像
                    GeneralFunction._AndMask(ref imageROI, rROI, minRectangle.Location);
                }
                else
                {
                    Image<Bgr, Byte> imageTemp = image.Copy();

                    imageTemp.ROI = minRectangle;//设置处理图像ROI区域
                    GeneralFunction._AndMask(ref imageTemp, rROI, minRectangle.Location);
                    imageTemp.ROI = Rectangle.Empty;//释放ROI区域

                    imageTemp = _Process_AngleAdjust(imageTemp);//有角度时进行角度转换

                    imageROI = imageTemp.Copy(ROIPro);//获取兴趣区域图像
                }
                ImageGray = GeneralFunction._ContrastColor(imageROI, (Enum.Contrast_Color)color);//调用颜色对比函数进行处理

                iResult_Sample = _DetectType(ImageGray, (Enum.Detect_Type)detect + 1, true);//根据检测类型调用相应处理函数
            }
            else
            {
                iResult_Sample = 0;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明： 图像处理函数
        // 输入参数： image:要处理的图像
        // 输出参数： result:处理结果
        // 返 回 值： 无
        //----------------------------------------------------------------------
        public Boolean _ImageProcess(Image<Bgr, Byte> image)
        {
            Boolean result = false;

            if ((image != null) && (minRectangle.Right <= image.Width) && (minRectangle.Bottom <= image.Height) && (minRectangle.Width > 0) && (minRectangle.Height > 0))
            {
                Image<Bgr, Byte> imageROI;
                if (0 == angleRotate)
                {
                    imageROI = image.Copy(minRectangle);//获取兴趣区域图像
                    GeneralFunction._AndMask(ref imageROI, rROI, minRectangle.Location);
                }
                else
                {
                    Image<Bgr, Byte> imageTemp = image.Copy();

                    imageTemp.ROI = minRectangle;//设置处理图像ROI区域
                    GeneralFunction._AndMask(ref imageTemp, rROI, minRectangle.Location);
                    imageTemp.ROI = Rectangle.Empty;//释放ROI区域

                    imageTemp = _Process_AngleAdjust(imageTemp);//有角度时进行角度转换

                    imageROI = imageTemp.Copy(ROIPro);//获取兴趣区域图像
                }
                ImageGray = GeneralFunction._ContrastColor(imageROI, (Enum.Contrast_Color)color);//调用颜色对比函数进行处理

                iResult_Current = _DetectType(ImageGray, (Enum.Detect_Type)detect + 1, false);//根据检测方式调用相应处理函数

                if ((iResult_Current >= iMin) && (iResult_Current <= iMax))//结果在公差范围内
                {
                    result = true;
                }
                else//结果在公差范围外
                {
                    result = ((!bExistTolerance) || false);//不存在公差时置true
                }
            }
            else
            {
                iResult_Current = 0;
                PosSmall_Current = 0;
                PosBig_Current = 0;
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
                Bgr bgr = ((iResult_Current >= iMin) && (iResult_Current <= iMax)) ? new Bgr(0, 255, 0) : new Bgr(0, 0, 255);//结果在指定公差范围内画绿线，反之画红线

                if (bReferenceH_Exist || bReferenceV_Exist)
                {
                    bgr = new Bgr(0, 255, 0);
                }

                Struct.ROI roiTemp = new Struct.ROI();

                if (0 == angleRotate) //旋转角度为零
                {
                    GeneralFunction._DrawGraphics(ref image, rROI, new Bgr(0, 255, 0), 1);
                }
                else
                {
                    rROI._CopyTo(ref roiTemp);

                    switch (rROI.roiExtra.roiType)
                    {
                        case Enum.ROIType.Quadrangle:
                            roiTemp.roiExtra.roiType = Enum.ROIType.Quadrangle;
                            roiTemp.roiExtra.Point1 = GeneralFunction._ComputeRotatePoint(rROI.roiExtra.Point1, new Point(ROIPro.Left + ROIPro.Width / 2, ROIPro.Top + ROIPro.Height / 2), angleRotate);
                            roiTemp.roiExtra.Point2 = GeneralFunction._ComputeRotatePoint(rROI.roiExtra.Point2, new Point(ROIPro.Left + ROIPro.Width / 2, ROIPro.Top + ROIPro.Height / 2), angleRotate);
                            roiTemp.roiExtra.Point3 = GeneralFunction._ComputeRotatePoint(rROI.roiExtra.Point3, new Point(ROIPro.Left + ROIPro.Width / 2, ROIPro.Top + ROIPro.Height / 2), angleRotate);
                            roiTemp.roiExtra.Point4 = GeneralFunction._ComputeRotatePoint(rROI.roiExtra.Point4, new Point(ROIPro.Left + ROIPro.Width / 2, ROIPro.Top + ROIPro.Height / 2), angleRotate);
                            GeneralFunction._DrawGraphics(ref image, roiTemp, new Bgr(0, 255, 0), 1);
                            break;
                        case Enum.ROIType.Ellipse:
                            CvInvoke.cvEllipse(image, rROI.roiExtra.Point1, new Size(roiTemp.roiExtra.Point2), angleRotate, 0, 360, new MCvScalar(0, 255, 0), 1, LINE_TYPE.EIGHT_CONNECTED, 0);
                            break;
                        case Enum.ROIType.Rectangle:
                            roiTemp.roiExtra.roiType = Enum.ROIType.Quadrangle;
                            roiTemp.roiExtra.Point1 = GeneralFunction._ComputeRotatePoint(rROI.roiExtra.Point1, new Point(ROIPro.Left + ROIPro.Width / 2, ROIPro.Top + ROIPro.Height / 2), angleRotate);
                            roiTemp.roiExtra.Point2 = GeneralFunction._ComputeRotatePoint(new Point(rROI.roiExtra.Point1.X + rROI.roiExtra.Point2.X, rROI.roiExtra.Point1.Y), new Point(ROIPro.Left + ROIPro.Width / 2, ROIPro.Top + ROIPro.Height / 2), angleRotate);
                            roiTemp.roiExtra.Point3 = GeneralFunction._ComputeRotatePoint(new Point(rROI.roiExtra.Point1.X + rROI.roiExtra.Point2.X, rROI.roiExtra.Point1.Y + rROI.roiExtra.Point2.Y), new Point(ROIPro.Left + ROIPro.Width / 2, ROIPro.Top + ROIPro.Height / 2), angleRotate);
                            roiTemp.roiExtra.Point4 = GeneralFunction._ComputeRotatePoint(new Point(rROI.roiExtra.Point1.X, rROI.roiExtra.Point1.Y + rROI.roiExtra.Point2.Y), new Point(ROIPro.Left + ROIPro.Width / 2, ROIPro.Top + ROIPro.Height / 2), angleRotate);
                            GeneralFunction._DrawGraphics(ref image, roiTemp, new Bgr(0, 255, 0), 1);
                            break;
                        default:
                            break;
                    }
                }

                switch ((Enum.Detect_Type)detect + 1)
                {
                    case Enum.Detect_Type.Gradient://梯度检测方式
                    case Enum.Detect_Type.Threshold://阈值检测方式

                        if ((bReferenceH_Exist == false) && (bReferenceV_Exist == false))
                        {
                            image.Draw(new LineSegment2D(new Point(iReferenceHorizenPoint - 2, iReferenceVerticalPoint), new Point(iReferenceHorizenPoint + 2, iReferenceVerticalPoint)), bgr, 1);
                            image.Draw(new LineSegment2D(new Point(iReferenceHorizenPoint, iReferenceVerticalPoint - 2), new Point(iReferenceHorizenPoint, iReferenceVerticalPoint + 2)), bgr, 1);
                        }

                        if (angleRotate == 0)
                        {
                            _DrawingFill(image, minRectangle, bgr);
                        }
                        else
                        {
                            Image<Bgr, Byte> imageROI = new Image<Bgr, Byte>(ROIPro.Size);//创建兴趣区域大小黑色图像
                            Rectangle rect = new Rectangle(ROIPro.Location, ROIPro.Size);
                            rect.Offset(-ROIPro.Left, -ROIPro.Top);
                            _DrawingFill(imageROI, rect, bgr);//给图像填充内容

                            Image<Bgr, Byte> imageMask = new Image<Bgr, Byte>(ROIPro.Size);//创建兴趣区域大小黑色图像
                            roiTemp._Offset(-ROIPro.Left, -ROIPro.Top);
                            GeneralFunction._DrawGraphics(ref imageMask, roiTemp, new Bgr(255, 255, 255), -1);
                            imageROI._And(imageMask);

                            Image<Gray, Byte> imageROIGray = imageROI.Convert<Gray, Byte>();
                            imageROIGray = imageROIGray.Rotate(angleRotate, new Gray(0));//旋转图像

                            image.ROI = ROIPro;
                            image.SetValue(bgr, imageROIGray);
                            image.ROI = Rectangle.Empty;
                        }
                        break;
                    case Enum.Detect_Type.PixelCount://像素计数检测方式
                        image.ROI = minRectangle;//设置兴趣区域
                        for (; ContourPixelCount != null; ContourPixelCount = ContourPixelCount.HNext)//遍历所有内外轮廓
                        {
                            image.Draw(ContourPixelCount, bgr, 1);//画所有内外轮廓
                        }
                        image.ROI = Rectangle.Empty;//释放兴趣区域
                        break;
                    default://错误处理方式
                        break;
                }
            }
        }
        
        //-----------------------------------------------------------------------
        // 功能说明： 计算处理图像待处理区域坐标函数
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _ROIProCalculate()
        {
            minRectangle = ROIPro = GeneralFunction._GetMinRect(rROI.roiExtra);

            if (angleRotate != 0) //旋转角度不为零
            {
                Int32 size = Convert.ToInt32(Math.Sqrt(Math.Pow(ROIPro.Width, 2) + Math.Pow(ROIPro.Height, 2)));//计算背景图像尺寸

                Int32 pointX = ROIPro.Left - (size - ROIPro.Width) / 2;
                Int32 pointY = ROIPro.Top - (size - ROIPro.Height) / 2;

                if ((pointX >= 0) && (pointY >= 0) && ((pointX + size) <= uiImageWidth) && ((pointY + size) < uiImageHeight))
                {
                    ROIPro.X = ROIPro.Left - (size - ROIPro.Width) / 2;
                    ROIPro.Y = ROIPro.Top - (size - ROIPro.Height) / 2;
                    ROIPro.Width = size;
                    ROIPro.Height = size;
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明： 角度调整函数
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private Image<Bgr, Byte> _Process_AngleAdjust(Image<Bgr, Byte> image)
        {
            image.ROI = ROIPro;//设置处理图像ROI区域
            Image<Bgr, Byte> imageRotate = image.Rotate(-angleRotate, new Bgr(0, 0, 0));//给ROI区域旋转
            image.SetZero();
            image._Or(imageRotate);
            image.ROI = Rectangle.Empty;//释放ROI区域

            return image;
        }

        //-----------------------------------------------------------------------
        // 功能说明： 扫描方向
        // 输入参数： 无
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void _ScanDirection()
        {
            switch ((Enum.Scan_Direction)direction + 1)
            {
                case Enum.Scan_Direction.Left_Right://水平方向，增量方向
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
                case Enum.Scan_Direction.Horizen://水平方向，从两边向中间找
                    Horizen = true;
                    In_Decrease = true;
                    break;
                case Enum.Scan_Direction.Vertical://垂直方向，从上下向中间找
                    Horizen = false;
                    In_Decrease = true;
                    break;
                default:
                    Increase = true;
                    Horizen = true;
                    break;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明： 检测方式
        // 输入参数： image:待处理图像，detecttype:检查类型枚举类型
        // 输出参数： pos：位置参数
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private Int32 _DetectType(Image<Gray, Byte> image, Enum.Detect_Type detecttype, Boolean learnSampleState)
        {
            Int32 result = 0;

            switch (detecttype)
            {
                case Enum.Detect_Type.Gradient:
                    result = _Process_Grad(image,learnSampleState);//梯度检测方式处理函数（满足像素亮度差delta，满足边缘宽度edge）
                    break;
                case Enum.Detect_Type.Threshold:
                    result = _Process_Threshold(image,learnSampleState);//阈值检测方式处理函数（满足合适像素个数delta，满足边缘宽度edge）
                    break;
                case Enum.Detect_Type.PixelCount:
                    result = _Process_PixelCount(image);//像素计数检测方式处理函数（满足阈值下限intensity_Min，满足阈值上限intensity_Max，）
                    break;
                case Enum.Detect_Type.Gray:
                    Image<Gray, byte> imageMask = GeneralFunction._GeneralImageMask(ImageGray, rROI, minRectangle.Location);
                    result = (Int32)ImageGray.GetAverage(imageMask).Intensity;//计算平均灰度
                    break;
                default:
                    result = 0;//错误方式处理函数
                    break;
            }

            if ((bReferenceH_Exist == false) && (bReferenceV_Exist == false)
                && (false == In_Decrease) && (((Enum.Detect_Type)(detect + 1) == Enum.Detect_Type.Gradient)
                    || ((Enum.Detect_Type)(detect + 1) == Enum.Detect_Type.Threshold)))
            {
                if (Horizen)
                {
                    result = Convert.ToInt32(result - iReferenceHorizenPoint);
                }
                else
                {
                    result = Convert.ToInt32(result - iReferenceVerticalPoint);
                }
            }

            return result;
        }

        //-----------------------------------------------------------------------
        // 功能说明： 亮度差异计算函数
        // 输入参数： image:待处理图像,startIndex:起始索引,endIndex:截止索引
        // 输出参数： image：已处理图像
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private Double _DifferenceCalculate(Image<Gray, Byte> image, int startIndex, int endIndex)
        {
            Double diffenerce = 0;

            if (Horizen)//水平方向
            {
                switch ((Enum.Check_Type)check + 1)
                {
                    case Enum.Check_Type.L_D://Grad L-D
                        diffenerce = image[0, startIndex].Intensity - image[0, endIndex].Intensity;
                        break;
                    case Enum.Check_Type.D_L://Grad D-L
                        diffenerce = image[0, endIndex].Intensity - image[0, startIndex].Intensity;
                        break;
                    case Enum.Check_Type.FirstGrad://FirstGrad
                        diffenerce = Math.Abs(image[0, startIndex].Intensity - image[0, endIndex].Intensity);
                        break;
                    default:
                        diffenerce = Math.Abs(image[0, startIndex].Intensity - image[0, endIndex].Intensity);
                        break;
                }
            }
            else//垂直方向
            {
                switch ((Enum.Check_Type)check + 1)
                {
                    case Enum.Check_Type.L_D://Grad L-D
                        diffenerce = image[startIndex, 0].Intensity - image[endIndex, 0].Intensity;
                        break;
                    case Enum.Check_Type.D_L://Grad D-L
                        diffenerce = image[endIndex, 0].Intensity - image[startIndex, 0].Intensity;
                        break;
                    case Enum.Check_Type.FirstGrad://FirstGrad
                        diffenerce = Math.Abs(image[startIndex, 0].Intensity - image[endIndex, 0].Intensity);
                        break;
                    default:
                        diffenerce = Math.Abs(image[startIndex, 0].Intensity - image[endIndex, 0].Intensity);
                        break;
                }
            }
            return diffenerce;
        }

        //-----------------------------------------------------------------------
        // 功能说明： 梯度处理函数
        // 输入参数： image:待处理图像
        // 输出参数： pos:位置参数
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private UInt16 _Process_Grad(Image<Gray, Byte> image, Boolean learnSampleState)
        {
            UInt16 pos = 0;
            int length = 0;
            Boolean flag = false;

            _ScanDirection();

            if (false == In_Decrease)//若查找长度，从两边向中间查找
            {
                image._ThresholdBinary(new Gray(80), new Gray(255));
                image._Dilate(1);
                image._Dilate(1);
                image._Erode(1);
                image._Erode(1);
            }

            Image<Gray, Byte> imageProjection;//定义投影函数

            if (Horizen)
            {
                length = image.Cols;//水平方向length为列数
                imageProjection = new Image<Gray, Byte>(image.Width, 1);
                image.Reduce(imageProjection, REDUCE_DIMENSION.SINGLE_ROW, REDUCE_TYPE.CV_REDUCE_AVG);//图像垂直投影                
            }
            else
            {
                length = image.Rows;//垂直方向length为行数
                imageProjection = new Image<Gray, Byte>(1, image.Height);
                image.Reduce(imageProjection, REDUCE_DIMENSION.SINGLE_COL, REDUCE_TYPE.CV_REDUCE_AVG);//图像水平投影    
            }

            if (In_Decrease)//若查找长度，从两边向中间查找
            {
                Increase = true;
                for (int i = 0; i < length - distance - edge - 1; i++)//增量方向查找
                {
                    if (learnSampleState)
                    {
                        pos = _GradCalculate(imageProjection, i, ref flag);//查找前缘位置
                        PosSmall_Sample = Horizen ? GeneralFunction._AxisPositionForward(pos, angleRotate, ROIPro.Left) : GeneralFunction._AxisPositionForward(pos, angleRotate, ROIPro.Top);
                    }
                    else
                    {
                        pos = _GradCalculate(imageProjection, i, ref flag);//查找前缘位置
                        PosSmall_Current = Horizen ? GeneralFunction._AxisPositionForward(pos, angleRotate, ROIPro.Left) : GeneralFunction._AxisPositionForward(pos, angleRotate, ROIPro.Top);
                    }

                    if (flag)
                    {
                        flag = false;
                        break;//找到退出循环
                    }
                }

                Increase = false;
                for (int i = length - 1; i > edge + distance; i--)//减量方向查找
                {
                    if (learnSampleState)
                    {
                        pos = _GradCalculate(imageProjection, i, ref flag);//查找后缘位置
                        PosBig_Sample = Horizen ? GeneralFunction._AxisPositionForward(pos, angleRotate, ROIPro.Left) : GeneralFunction._AxisPositionForward(pos, angleRotate, ROIPro.Top);
                    }
                    else
                    {
                        pos = _GradCalculate(imageProjection, i, ref flag);//查找后缘位置
                        PosBig_Current = Horizen ? GeneralFunction._AxisPositionForward(pos, angleRotate, ROIPro.Left) : GeneralFunction._AxisPositionForward(pos, angleRotate, ROIPro.Top);
                    }
                    
                    if (flag)
                    {
                        break;//找到退出循环
                    }
                }

                if (learnSampleState)
                {
                    pos = Convert.ToUInt16((PosSmall_Sample > PosBig_Sample) ? 0 : (PosBig_Sample - PosSmall_Sample));//计算长度值
                }
                else
                {
                    pos = Convert.ToUInt16((PosSmall_Current > PosBig_Current) ? 0 : (PosBig_Current - PosSmall_Current));//计算长度值
                }
            }
            else//否则按照所设定值单方向查找
            {
                if (Increase)
                {
                    for (int i = 0; i < length - distance - edge - 1; i++)//增量方向查找
                    {
                        pos = _GradCalculate(imageProjection, i, ref flag);//查找位置
                        if (flag)
                        {
                            break;//找到退出循环
                        }
                    }
                }
                else
                {
                    for (int i = length - 1; i > edge + distance; i--)//减量方向查找
                    {
                        pos = _GradCalculate(imageProjection, i, ref flag);//查找位置
                        if (flag)
                        {
                            break;//找到退出循环
                        }
                    }
                }
                Result_Current_Temp = pos = Horizen ? GeneralFunction._AxisPositionForward(pos, angleRotate, ROIPro.Left) : GeneralFunction._AxisPositionForward(pos, angleRotate, ROIPro.Top);
            }
            return pos;
        }

        //-----------------------------------------------------------------------
        // 功能说明： 计算位置处理函数
        // 输入参数： image:待处理图像,x:索引,result:跳出循环标志
        // 输出参数： pos:位置参数
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private UInt16 _GradCalculate(Image<Gray, Byte> image, int x, ref Boolean result)
        {
            UInt16 pos = 0;
            int length = 0;
            Boolean flag = false;
            Double gap1 = 0, count = 0;

            if (Increase)
            {
                gap1 = _DifferenceCalculate(image, x, x + distance + 1);//计算相邻两点亮度差值
            }
            else
            {
                gap1 = _DifferenceCalculate(image, x, x - distance - 1);//计算相邻两点亮度差值
            }

            //flag = ((gap1 >= delta) || (gap1 >= 0.2 * delta));//找到满足条件的第一个像素点,找不到降低标准，防止边缘附近点亮度变化缓慢
            flag = (gap1 >= delta);//找到满足条件的第一个像素点

            if (flag)
            {
                Double[] gap2 = new Double[edge];
                for (int j = 1; j < edge; j++)//判断是否连续edge宽度个像素点都满足
                {
                    if (Increase)
                    {
                        gap2[j] = _DifferenceCalculate(image, x, x + distance + 1 + j);//计算连续edge个点与第一个点的亮度差值
                    }
                    else
                    {
                        gap2[j] = _DifferenceCalculate(image, x, x - distance -1 - j);//计算连续edge个点与第一个点的亮度差值
                    }
                    if (gap2[j] >= delta)
                    {
                        count++;
                    }
                }

                if (count >= (2 * edge / 3))
                {
                    pos = Convert.ToUInt16(x);
                    result = true;
                }
                else//没找到，把头部(增量方向)或者尾部(减量方向)当作前缘位置
                {
                    length = Horizen ? image.Cols : image.Rows;//水平方向length为列数，垂直方向length为行数
                    pos = Convert.ToUInt16(Increase ? (length - 1) : 0);//找到前缘位置，记录位置
                    result = false;
                }
            }
            else//没找到，把头部(增量方向)或者尾部(减量方向)当作前缘位置
            {
                length = Horizen ? image.Cols : image.Rows;//水平方向length为列数，垂直方向length为行数
                pos = Convert.ToUInt16(Increase ? (length - 1) : 0);//找到前缘位置，记录位置
                result = false;
            }
            return pos;
        }    

        //-----------------------------------------------------------------------
        // 功能说明： 阈值处理函数
        // 输入参数： image:待处理图像
        // 输出参数： pos:位置参数
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private UInt16 _Process_Threshold(Image<Gray, Byte> image, Boolean learnSampleState)
        {
            UInt16 pos = 0;
            Boolean flag = false;

            _ScanDirection();
            int length = Horizen ? image.Cols : image.Rows;//水平方向length为列数,垂直方向length为行数

            if (In_Decrease)//若查找长度，从两边向中间查找
            {
                Increase = true;
                for (int i = 0; i < length - edge - 1; i++)//增量方向查找
                {
                    if (learnSampleState)
                    {
                        pos = _ThresholdCalculate(image, i, ref flag);//查找前缘位置
                        PosSmall_Sample = Horizen ? GeneralFunction._AxisPositionForward(pos, angleRotate, ROIPro.Left) : GeneralFunction._AxisPositionForward(pos, angleRotate, ROIPro.Top);
                    }
                    else
                    {
                        pos = _ThresholdCalculate(image, i, ref flag);//查找前缘位置
                        PosSmall_Current = Horizen ? GeneralFunction._AxisPositionForward(pos, angleRotate, ROIPro.Left) : GeneralFunction._AxisPositionForward(pos, angleRotate, ROIPro.Top);
                    }

                    if (flag)
                    {
                        flag = false;
                        break;//找到退出循环
                    }
                }

                Increase = false;
                for (int i = length - 1; i > edge; i--)//减量方向查找
                {
                    if (learnSampleState)
                    {
                        pos = _ThresholdCalculate(image, i, ref flag);//查找后缘位置
                        PosBig_Sample = Horizen ? GeneralFunction._AxisPositionForward(pos, angleRotate, ROIPro.Left) : GeneralFunction._AxisPositionForward(pos, angleRotate, ROIPro.Top);
                    }
                    else
                    {
                        pos = _ThresholdCalculate(image, i, ref flag);//查找后缘位置
                        PosBig_Current = Horizen ? GeneralFunction._AxisPositionForward(pos, angleRotate, ROIPro.Left) : GeneralFunction._AxisPositionForward(pos, angleRotate, ROIPro.Top);
                    }
                    
                    if (flag)
                    {
                        break;//找到退出循环
                    }
                }

                if (learnSampleState)
                {
                    pos = Convert.ToUInt16((PosSmall_Sample > PosBig_Sample) ? 0 : (PosBig_Sample - PosSmall_Sample));//计算长度值
                }
                else
                {
                    pos = Convert.ToUInt16((PosSmall_Current > PosBig_Current) ? 0 : (PosBig_Current - PosSmall_Current));//计算长度值
                }
            }
            else//否则按照所设定值单方向查找
            {
                if (Increase)
                {
                    for (int i = 0; i < length - edge - 1; i++)//增量方向查找
                    {
                        pos = _ThresholdCalculate(image, i, ref flag);//查找位置
                        if (flag)
                        {
                            break;//找到退出循环
                        }
                    }
                }
                else
                {
                    for (int i = length - 1; i > edge; i--)//减量方向查找
                    {
                        pos = _ThresholdCalculate(image, i, ref flag);//查找位置
                        if (flag)
                        {
                            break;//找到退出循环
                        }
                    }
                }
                Result_Current_Temp = pos = Horizen ? GeneralFunction._AxisPositionForward(pos, angleRotate, ROIPro.Left) : GeneralFunction._AxisPositionForward(pos, angleRotate, ROIPro.Top);
            }          

            return pos;
        }

        //-----------------------------------------------------------------------
        // 功能说明： 计算位置处理函数
        // 输入参数： image:待处理图像,x:索引,result:跳出循环标志
        // 输出参数： pos:位置参数
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private UInt16 _ThresholdCalculate(Image<Gray, Byte> image, int x, ref Boolean result)
        {
            UInt16 pos = 0;
            Boolean flag = false;

            if (_ThresholdCount(image, x))//找到满足条件的第一个边缘
            {
                for (int j = 1; j < edge; j++)//继续比较是否连续edge列都满足条件
                {
                    Boolean content = Increase ? _ThresholdCount(image, x + j) : _ThresholdCount(image, x - j);
                    if (!content)//不满足，则退出内循环
                    {
                        flag = false;
                        break;
                    }
                    else
                    {
                        flag = true;
                    }
                }

                if (flag)//满足，记录下此时位置，退出外循环
                {
                    pos = (UInt16)x;
                    result = true;
                }
            }
            else//没找到，把头部(增量方向)或者尾部(减量方向)当作前缘位置
            {
                int length = Horizen ? image.Cols : image.Rows;//水平方向length为列数，垂直方向length为行数
                pos = Convert.ToUInt16(Increase ? (length - 1) : 0);//找到前缘位置，记录位置
                result = false;
            }      

            return pos;
        }

        //-----------------------------------------------------------------------
        // 功能说明： 阈值检测方式时，判断图像每列在范围内的像素个数是否满足delta条件
        // 输入参数： image:待处理图像，index:图像数据索引
        // 输出参数： 无
        // 返 回 值： flag,true:满足,false:不满足
        //----------------------------------------------------------------------
        private Boolean _ThresholdCount(Image<Gray, Byte> image, int index)
        {
            Boolean flag = false;
            int count = 0;

            int height = Horizen ? image.Rows : image.Cols;//计算图像高度

            for (int j = 0; j < height; j++)//在一列上循环比较
            {
                Double inten = Horizen ? (image[j, index].Intensity) : (image[index, j].Intensity);
                if ((inten >= intensity_Min) && (inten <= intensity_Max))
                {
                    count++;//在阈值范围内则计数
                }
            }
            if (count >= delta)//计数大于等于所设置的delta个数，则满足条件,反之则不满足
            {
                flag = true;
            }
            return flag;
        }

        //-----------------------------------------------------------------------
        // 功能说明： 像素计数处理函数
        // 输入参数： image:待处理图像
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private Int32 _Process_PixelCount(Image<Gray, Byte> image)
        {
            if (intensity_Min == 0)           //对当前兴趣区域进行阈值分割
            {
                image = image.ThresholdBinaryInv(new Gray(intensity_Max), new Gray(255));
            }
            else if (intensity_Max == 255)    //对当前兴趣区域进行阈值分割
            {
                image = image.ThresholdBinary(new Gray(intensity_Min - 1), new Gray(255));
            }
            else  //对当前兴趣区域进行阈值分割
            {
                Image<Gray, Byte> ImageIN0 = image.ThresholdBinary(new Gray(intensity_Min - 1), new Gray(255));
                Image<Gray, Byte> ImageIN1 = image.ThresholdBinaryInv(new Gray(intensity_Max), new Gray(255));
                image = ImageIN0.And(ImageIN1);
            }
            image._And(GeneralFunction._GeneralImageMask(image, rROI, minRectangle.Location));//2021.08.27更改
            ContourPixelCount = image.FindContours(

                           CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_NONE, RETR_TYPE.CV_RETR_CCOMP);//查找内外轮廓

            return image.CountNonzero()[0];
        }
                        
        //-----------------------------------------------------------------------
        // 功能说明： 画图填充函数
        // 输入参数： image:要处理的图像,ROIDraw:画图区域信息
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private Image<Bgr, Byte> _DrawingFill(Image<Bgr, Byte> image, Rectangle ROIDraw, Bgr fillColor)
        {
            Int32 offset = 0;
            if (false == Increase)
            {
                offset = distance;
            }
            else
            {
                offset = -distance;
            }

            if (In_Decrease)//长度计算
            {
                Int32 iPosSmall_Current_Temp = PosSmall_Current;
                Int32 iPosBig_Current_Temp = PosBig_Current;

                if (Horizen)//水平方向
                {
                    Int32 temp = GeneralFunction._AxisPositionBackward(iPosSmall_Current_Temp, angleRotate, ROIPro.Left) + offset;
                    image.Draw(new LineSegment2D(new Point(temp, ROIDraw.Top), new Point(temp, ROIDraw.Bottom)), fillColor, 1);
                    _DrawFork(image, new Point(temp, ROIDraw.Top), 5, fillColor);
                    _DrawFork(image, new Point(temp, ROIDraw.Bottom), 5, fillColor);

                    temp = GeneralFunction._AxisPositionBackward(iPosBig_Current_Temp, angleRotate, ROIPro.Left) + offset;
                    image.Draw(new LineSegment2D(new Point(temp, ROIDraw.Top), new Point(temp, ROIDraw.Bottom)), fillColor, 1);
                    _DrawFork(image, new Point(temp, ROIDraw.Top), 5, fillColor);
                    _DrawFork(image, new Point(temp, ROIDraw.Bottom), 5, fillColor);
                }
                else//垂直方向
                {
                    Int32 temp = GeneralFunction._AxisPositionBackward(iPosSmall_Current_Temp, angleRotate, ROIPro.Top) + offset;
                    image.Draw(new LineSegment2D(new Point(ROIDraw.Left, temp), new Point(ROIDraw.Right, temp)), fillColor, 1);
                    _DrawFork(image, new Point(ROIDraw.Left, temp), 5, fillColor);
                    _DrawFork(image, new Point(ROIDraw.Right, temp), 5, fillColor);

                    temp = GeneralFunction._AxisPositionBackward(iPosBig_Current_Temp, angleRotate, ROIPro.Top) + offset;
                    image.Draw(new LineSegment2D(new Point(ROIDraw.Left, temp), new Point(ROIDraw.Right, temp)), fillColor, 1);
                    _DrawFork(image, new Point(ROIDraw.Left, temp), 5, fillColor);
                    _DrawFork(image, new Point(ROIDraw.Right, temp), 5, fillColor);
                }
            }
            else//单方向查找
            {
                Int32 iResult_Current_Temp = Result_Current_Temp;

                if (Horizen)//水平方向
                {
                    Int32 temp = GeneralFunction._AxisPositionBackward(iResult_Current_Temp, angleRotate, ROIPro.Left) + offset;
                    image.Draw(new LineSegment2D(new Point(temp, ROIDraw.Top), new Point(temp, ROIDraw.Bottom)), fillColor, 1);
                    _DrawFork(image, new Point(temp, ROIDraw.Top), 5, fillColor);
                    _DrawFork(image, new Point(temp, ROIDraw.Bottom), 5, fillColor);
                }
                else//垂直方向
                {
                    Int32 temp = GeneralFunction._AxisPositionBackward(iResult_Current_Temp, angleRotate, ROIPro.Top) + offset;
                    image.Draw(new LineSegment2D(new Point(ROIDraw.Left, temp), new Point(ROIDraw.Right, temp)), fillColor, 1);
                    _DrawFork(image, new Point(ROIDraw.Left, temp), 5, fillColor);
                    _DrawFork(image, new Point(ROIDraw.Right, temp), 5, fillColor);
                }
            }
            return image;
        }

        //-----------------------------------------------------------------------
        // 功能说明： 画叉函数
        // 输入参数： image:待处理图像，point:叉的位置点，size:叉的大小，bgr:叉的颜色
        // 输出参数： 无
        // 返 回 值： image：已处理图像
        //----------------------------------------------------------------------
        private Image<Bgr, Byte> _DrawFork(Image<Bgr, Byte> image, Point point, Byte size, Bgr bgr)
        {
            image.Draw(new LineSegment2D(new Point(point.X - size, point.Y - size),
                new Point(point.X + size, point.Y + size)), bgr, 1);
            image.Draw(new LineSegment2D(new Point(point.X + size, point.Y - size),
                new Point(point.X - size, point.Y + size)), bgr, 1);

            return image;
        }

        //-----------------------------------------------------------------------
        // 功能说明： 画叉函数
        // 输入参数： image:待处理图像，point:叉的位置点，size:叉的大小，gray:叉的颜色
        // 输出参数： 无
        // 返 回 值： image：已处理图像
        //----------------------------------------------------------------------
        private Image<Gray, Byte> _DrawFork(Image<Gray, Byte> image, Point point, Byte size, Gray gray)
        {
            image.Draw(new LineSegment2D(new Point(point.X - size, point.Y - size),
                new Point(point.X + size, point.Y + size)), gray, 1);
            image.Draw(new LineSegment2D(new Point(point.X + size, point.Y - size),
                new Point(point.X - size, point.Y + size)), gray, 1);

            return image;
        }
    }    
}