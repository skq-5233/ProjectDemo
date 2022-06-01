using System;
using System.Drawing;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace VisionSystemExtendClassLibrary
{
    public static class MatchTemplate451
    {
        /// <summary>
        /// emgu4.5.1版本模板匹配
        /// </summary>
        /// <param name="image"></param>
        /// <param name="imageTemplate"></param>
        /// <returns></returns>
        public static Point _MatchTemplate(Bitmap bitmap, Bitmap bitmapTemplate)
        {
            Point maxLoc = new Point();

            try
            {
                Image<Bgr, Byte> image = BitmapExtension.ToImage<Bgr, Byte>(bitmap);
                Image<Bgr, Byte> imageTemplate = BitmapExtension.ToImage<Bgr, Byte>(bitmapTemplate);

                Mat matchResult = new Mat();
                CvInvoke.MatchTemplate(image, imageTemplate, matchResult, TemplateMatchingType.CcoeffNormed);

                double minVal = 0;
                double maxVal = 0;
                Point minLoc = new Point();
                CvInvoke.MinMaxLoc(matchResult, ref minVal, ref maxVal, ref minLoc, ref maxLoc);
            }
            catch (Exception ex)
            {
            }
            return maxLoc;
        }
    }
}
