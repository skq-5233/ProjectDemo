using System;

using System.Drawing;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

using System.IO;
using System.Drawing.Imaging;

namespace VisionSystemClassLibrary
{
    public static class GeneralFunction
    {
        /// <summary>
        /// 查找最小外接矩形
        /// </summary>
        /// <param name="roi_Inner"></param>
        /// <returns></returns>
        public static Rectangle _GetMinRect(Struct.ROI_Inner roi_Inner)
        {
            Rectangle rect;

            switch (roi_Inner.roiType)
            {
                case Enum.ROIType.Quadrangle:
                    PointF[] points = new PointF[4];
                    points[0] = roi_Inner.Point1;
                    points[1] = roi_Inner.Point2;
                    points[2] = roi_Inner.Point3;
                    points[3] = roi_Inner.Point4;
                    rect = PointCollection.BoundingRectangle(points);
                    break;
                case Enum.ROIType.Ellipse:
                    PointF[] points_Ellipse = new PointF[4];
                    points_Ellipse[0] = new PointF(roi_Inner.Point1.X - roi_Inner.Point2.X, roi_Inner.Point1.Y - roi_Inner.Point2.Y);
                    points_Ellipse[1] = new PointF(roi_Inner.Point1.X + roi_Inner.Point2.X, roi_Inner.Point1.Y - roi_Inner.Point2.Y);
                    points_Ellipse[2] = new PointF(roi_Inner.Point1.X + roi_Inner.Point2.X, roi_Inner.Point1.Y + roi_Inner.Point2.Y);
                    points_Ellipse[3] = new PointF(roi_Inner.Point1.X - roi_Inner.Point2.X, roi_Inner.Point1.Y + roi_Inner.Point2.Y);
                    rect = PointCollection.BoundingRectangle(points_Ellipse);
                    break;
                default:
                    rect = new Rectangle(roi_Inner.Point1, new Size(roi_Inner.Point2));
                    break;
            }
            return rect;
        }

        /// <summary>
        /// 绘制区域
        /// </summary>
        /// <param name="image"></param>
        /// <param name="roi"></param>
        public static void _DrawGraphics(ref Image<Bgr, Byte> image, Struct.ROI roi, Bgr bgr, Int32 thinkness = 1)
        {
            _DrawGraphics(ref image, roi.roiExtra, bgr, thinkness);

            if (roi.roiInnerExit)
            {
                _DrawGraphics(ref image, roi.roiInner, bgr, thinkness);
            }
        }

        /// <summary>
        /// 绘制区域
        /// </summary>
        /// <param name="image"></param>
        /// <param name="roi_Inner"></param>
        /// <param name="bgr"></param>
        /// <param name="thinkness"></param>
        private static void _DrawGraphics(ref Image<Bgr, Byte> image, Struct.ROI_Inner roi_Inner, Bgr bgr, Int32 thinkness)
        {
            switch (roi_Inner.roiType)
            {
                case Enum.ROIType.Quadrangle:
                    Point[] points = new Point[4];
                    points[0] = roi_Inner.Point1;
                    points[1] = roi_Inner.Point2;
                    points[2] = roi_Inner.Point3;
                    points[3] = roi_Inner.Point4;
                    if (thinkness < 0) //填充
                    {
                        CvInvoke.cvFillConvexPoly(image, points, points.Length, bgr.MCvScalar, LINE_TYPE.EIGHT_CONNECTED, 0);
                    }
                    else
                    {
                        image.DrawPolyline(points, true, bgr, thinkness);
                    }
                    break;
                case Enum.ROIType.Ellipse:
                    CvInvoke.cvEllipse(image, roi_Inner.Point1, new Size(roi_Inner.Point2), 0, 0, 360, bgr.MCvScalar, thinkness, LINE_TYPE.EIGHT_CONNECTED, 0);
                    break;
                default:
                    image.Draw(new Rectangle(roi_Inner.Point1, new Size(roi_Inner.Point2)), bgr, thinkness);
                    break;
            }
        }

        /// <summary>
        /// 绘制区域
        /// </summary>
        /// <param name="image"></param>
        /// <param name="roi_Inner"></param>
        /// <param name="bgr"></param>
        /// <param name="thinkness"></param>
        private static void _DrawGraphics(ref Image<Gray, Byte> image, Struct.ROI_Inner roi_Inner, Gray gray, Int32 thinkness)
        {
            switch (roi_Inner.roiType)
            {
                case Enum.ROIType.Quadrangle:
                    Point[] points = new Point[4];
                    points[0] = roi_Inner.Point1;
                    points[1] = roi_Inner.Point2;
                    points[2] = roi_Inner.Point3;
                    points[3] = roi_Inner.Point4;
                    if (thinkness < 0) //填充
                    {
                        CvInvoke.cvFillConvexPoly(image, points, points.Length, gray.MCvScalar, LINE_TYPE.EIGHT_CONNECTED, 0);
                    }
                    else
                    {
                        image.DrawPolyline(points, true, gray, thinkness);
                    }
                    break;
                case Enum.ROIType.Ellipse:
                    CvInvoke.cvEllipse(image, roi_Inner.Point1, new Size(roi_Inner.Point2), 0, 0, 360, gray.MCvScalar, thinkness, LINE_TYPE.EIGHT_CONNECTED, 0);
                    break;
                default:
                    image.Draw(new Rectangle(roi_Inner.Point1, new Size(roi_Inner.Point2)), gray, thinkness);
                    break;
            }
        }

        /// <summary>
        /// 生成图像蒙版
        /// </summary>
        /// <param name="image"></param>
        /// <param name="roi"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static Image<Gray, Byte> _GeneralImageMask(Image<Gray, Byte> image, Struct.ROI roi, Point offset)
        {
            Image<Gray, Byte> imageTemp = new Image<Gray, byte>(image.Size);
            Struct.ROI roiTemp = new Struct.ROI();
            roi._CopyTo(ref roiTemp);

            roiTemp._Offset(-offset.X, -offset.Y);//计算偏移

            _DrawGraphics(ref imageTemp, roiTemp.roiExtra, new Gray(255), -1);
            if (roi.roiInnerExit)
            {
                _DrawGraphics(ref imageTemp, roiTemp.roiInner, new Gray(0), -1);//2021.08.27更改
            }
            return imageTemp;
        }

        /// <summary>
        /// 生成图像蒙版
        /// </summary>
        /// <param name="image"></param>
        /// <param name="roi"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private static Image<Bgr, Byte> _GeneralImageMask(Image<Bgr, Byte> image, Struct.ROI roi,Point offset)
        {
            Image<Bgr, Byte> imageTemp = new Image<Bgr, byte>(image.Size);
            Struct.ROI roiTemp = new Struct.ROI();
            roi._CopyTo(ref roiTemp);

            roiTemp._Offset(-offset.X, -offset.Y);//计算偏移

            _DrawGraphics(ref imageTemp, roiTemp.roiExtra, new Bgr(255, 255, 255), -1);
            if (roi.roiInnerExit)
            {
                _DrawGraphics(ref imageTemp, roiTemp.roiInner, new Bgr(0, 0, 0), -1);
            }
            return imageTemp;
        }
        
        /// <summary>
        /// 与蒙版图像进行与操作
        /// </summary>
        /// <param name="image"></param>
        /// <param name="roi"></param>
        /// <returns></returns>
        public static void _AndMask(ref Image<Bgr, Byte> image, Struct.ROI roi, Point offset)
        {
            if ((roi.roiExtra.roiType != Enum.ROIType.Rectangle) || roi.roiInnerExit) //非矩形区域或者包含内部区域
            {
                Image<Bgr, Byte> imageTemp = _GeneralImageMask(image, roi, offset);
                image._And(imageTemp);
            }
        }

        /// <summary>
        /// 颜色对比
        /// </summary>
        /// <param name="image"></param>
        /// <param name="color"></param>
        public static Image<Gray, Byte> _ContrastColor(Image<Bgr, Byte> image, Enum.Contrast_Color color)
        {
            Image<Gray, Byte> imageTemp;

            switch (color)
            {
                case Enum.Contrast_Color.Intensity:
                    imageTemp = image.Convert<Gray, Byte>();
                    break;
                case Enum.Contrast_Color.Blue:
                    imageTemp = image.Split()[0];
                    break;
                case Enum.Contrast_Color.Green:
                    imageTemp = image.Split()[1];
                    break;
                case Enum.Contrast_Color.Red:
                    imageTemp = image.Split()[2];
                    break;
                case Enum.Contrast_Color.Single:
                    Image<Gray, Byte>[] imageBuff = image.Split();
                    imageTemp = (imageBuff[2].Sub(imageBuff[1])).AbsDiff(new Gray(0));
                    imageTemp._ThresholdBinary(new Gray(20), new Gray(255));
                    imageTemp._Erode(1);

                    //imageTemp = new Image<Gray, Byte>(image.Size);
                    //CvInvoke.cvMax(imageBuff[0].Ptr, imageBuff[1].Ptr, imageTemp.Ptr);
                    //CvInvoke.cvMax(imageTemp.Ptr, imageBuff[2].Ptr, imageTemp.Ptr);
                    break;
                default:
                    imageTemp = image.Convert<Gray, Byte>();
                    break;
            }
            return imageTemp;
        }

        /// <summary>
        /// 比较兴趣区域是否相等
        /// </summary>
        /// <param name="roi1"></param>
        /// <param name="roi2"></param>
        /// <returns></returns>
        public static Boolean _IsEqual(Struct.ROI roi1, Struct.ROI roi2)
        {
            return (roi1.roiInnerExit == roi2.roiInnerExit) && _IsEqual(roi1.roiExtra, roi2.roiExtra) && _IsEqual(roi1.roiInner, roi2.roiInner);
        }

        /// <summary>
        /// 比较兴趣区域是否相等
        /// </summary>
        /// <param name="roi_Inner1"></param>
        /// <param name="roi_Inner2"></param>
        /// <returns></returns>
        private static Boolean _IsEqual(Struct.ROI_Inner roi_Inner1, Struct.ROI_Inner roi_Inner2)
        {
            return roi_Inner1.Point1.Equals(roi_Inner2.Point1) && roi_Inner1.Point2.Equals(roi_Inner2.Point2) && roi_Inner1.Point3.Equals(roi_Inner2.Point3) && roi_Inner1.Point4.Equals(roi_Inner2.Point4);
        }

        /// <summary>
        /// 计算旋转坐标
        /// </summary>
        /// <param name="point"></param>
        /// <param name="center"></param>
        /// <param name="angle"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Point _ComputeRotatePoint(Point point, Point center, Int16 angle)
        {
            //转换角度
            Point temp = new Point();

            double dangle = angle * Microsoft.JScript.MathObject.PI / 180;
            double dSin = Microsoft.JScript.MathObject.sin(dangle);
            double dCos = Microsoft.JScript.MathObject.cos(dangle);

            temp.X = Convert.ToInt32((point.X - center.X) * dCos - (point.Y - center.Y) * dSin + center.X);
            temp.Y = Convert.ToInt32((point.X - center.X) * dSin + (point.Y - center.Y) * dCos + center.Y);

            return temp;
        }
        
        /// <summary>
        /// 计算坐标位置（正向）
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="angleRotate"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static UInt16 _AxisPositionForward(UInt16 pos, Int16 angleRotate, Int32 offset)
        {
            double temp = (double)pos / Microsoft.JScript.MathObject.cos(Microsoft.JScript.MathObject.abs(angleRotate) * Microsoft.JScript.MathObject.PI / 180);
            return Convert.ToUInt16(temp + offset);
        }

        /// <summary>
        /// 计算坐标位置（反向）
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="angleRotate"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static Int32 _AxisPositionBackward(Int32 pos, Int16 angleRotate, Int32 offset)
        {
            if (0 == angleRotate)
            {
                offset = 0;
            }
            double temp = (pos - offset) * Microsoft.JScript.MathObject.cos(Microsoft.JScript.MathObject.abs(angleRotate) * Microsoft.JScript.MathObject.PI / 180);
            return Convert.ToInt32(temp);
        }

        //-----------------------------------------------------------------------
        // 功能说明：更新并保存当前班次统计信息
        // 输入参数：1、Image<Bgr, Byte>：image，缺陷图像
        //                    2、string：filename，图像信息
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static void _SaveImage(Image<Bgr, Byte> img, string filename, string imageFormat)
        {
            if (imageFormat.ToLower() == ".bmp") //
            {
                MemoryStream memoryStream = new MemoryStream();//流对象
                img.Bitmap.Save(memoryStream, global::System.Drawing.Imaging.ImageFormat.Bmp);

                memoryStream.Position = 0;
                Byte[] memoryStreamBytes = new Byte[memoryStream.Length];
                memoryStream.Read(memoryStreamBytes, 0, (Int32)memoryStream.Length);

                FileStream fileStream = new FileStream(filename, FileMode.OpenOrCreate); //打开REJECT文件
                BinaryWriter binaryWriter = new BinaryWriter(fileStream);

                binaryWriter.Write(memoryStreamBytes);

                binaryWriter.Close();//关闭图像信息文件
                fileStream.Close();
            }
            else if (imageFormat.ToLower() == ".jpg") //
            {
                MemoryStream memoryStream = new MemoryStream();//流对象
                img.Bitmap.Save(memoryStream, global::System.Drawing.Imaging.ImageFormat.Jpeg);

                memoryStream.Position = 0;
                Byte[] memoryStreamBytes = new Byte[memoryStream.Length];
                memoryStream.Read(memoryStreamBytes, 0, (Int32)memoryStream.Length);

                FileStream fileStream = new FileStream(filename, FileMode.OpenOrCreate); //打开REJECT文件
                BinaryWriter binaryWriter = new BinaryWriter(fileStream);

                binaryWriter.Write(memoryStreamBytes);

                binaryWriter.Close();//关闭图像信息文件
                fileStream.Close();
            }
            else if (imageFormat.ToLower() == ".png") //
            {
                MemoryStream memoryStream = new MemoryStream();//流对象
                img.Bitmap.Save(memoryStream, global::System.Drawing.Imaging.ImageFormat.Png);

                memoryStream.Position = 0;
                Byte[] memoryStreamBytes = new Byte[memoryStream.Length];
                memoryStream.Read(memoryStreamBytes, 0, (Int32)memoryStream.Length);

                FileStream fileStream = new FileStream(filename, FileMode.OpenOrCreate); //打开REJECT文件
                BinaryWriter binaryWriter = new BinaryWriter(fileStream);

                binaryWriter.Write(memoryStreamBytes);

                binaryWriter.Close();//关闭图像信息文件
                fileStream.Close();
            }
            else
            {
            }
        }

        ///// <summary>
        ///// 读取图像
        ///// </summary>
        ///// <param name="filename"></param>
        //public static Image<Bgr, Byte> _ReadImage(string filename)
        //{
        //    Image<Bgr, Byte> imageOUT = null;

        //    Bitmap bitmap = new Bitmap(filename);
        //    BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
        //    Image<Bgr, Byte> image = new Image<Bgr, Byte>(bitmap.Width, bitmap.Height, bitmapData.Stride, bitmapData.Scan0);
        //    imageOUT = image.Copy();//更新数值
        //    bitmap.UnlockBits(bitmapData);

        //    return imageOUT;
        //}
    }
}
