using System;
using System.Collections.Generic;

using System.Drawing;
using System.IO;

using Emgu.CV;
using Emgu.CV.Dnn;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Drawing.Imaging;

namespace VisionSystemExtendClassLibrary
{
    public class DNN_OpenVINO
    {
        private Net net; //DNN网络
        private List<string> typeList;//类别
        private UInt16 ModelWidth;//模型宽度
        private UInt16 ModelHeight;//模型高度

        public DNN_OpenVINO()
        {
            net = new Net();
            typeList = new List<string>();
        }

        /// <summary>
        /// DNN网络初始化函数
        /// </summary>
        /// <param name="strTypeFileName"></param>
        /// <param name="strTfPbFileName"></param>
        /// <param name="backend"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool _NetInit(string strTypeFileName, string strTfPbFileName, Int32 backend, Byte target, UInt16 width, UInt16 height)
        {
            ModelWidth = width;
            ModelHeight = height;

            StreamReader sr = new StreamReader(strTypeFileName);
            string line;

            while ((line = sr.ReadLine()) != null)
            {
                typeList.Add(line);
            }

            //net = DnnInvoke.ReadNetFromTensorflow(strTfPbFileName);
            FileStream readFileStream = new FileStream(strTfPbFileName, FileMode.Open, FileAccess.Read);
            byte[] bytes = new byte[readFileStream.Length];
            readFileStream.Read(bytes, 0, bytes.Length);
            readFileStream.Close();

            for (int i = 0; i < bytes.Length; i++)
            {
                --bytes[i];
            }
            net = DnnInvoke.ReadNetFromTensorflow(bytes);
            net.SetPreferableBackend((Emgu.CV.Dnn.Backend)backend);//Emgu.CV.Dnn.Backend.InferenceEngine
            net.SetPreferableTarget((Target)target);//Target.Cpu

            return true;
        }

        /// <summary>
        /// DNN网络单张图像处理函数
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="maxVal"></param>
        /// <param name="maxLoc"></param>
        /// <param name="typeName"></param>
        public void _ImageProcessingOne(Bitmap bitmap, ref double maxVal, ref Point maxLoc, ref string typeName)    //处理指定的单张图片
        {
            //MemoryStream stream = new MemoryStream();
            //bitmap.Save(stream, ImageFormat.Jpeg);

            //Image<Bgr, Byte> imageOUT = null;
            //Bitmap bitmapTemp = new Bitmap(stream);
            //BitmapData bitmapData = bitmapTemp.LockBits(new Rectangle(0, 0, bitmapTemp.Width, bitmapTemp.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            //Image<Bgr, Byte> image = new Image<Bgr, Byte>(bitmapTemp.Width, bitmapTemp.Height, bitmapData.Stride, bitmapData.Scan0);
            //imageOUT = image.Copy();//更新数值
            //bitmapTemp.UnlockBits(bitmapData);

            Mat img = bitmap.ToMat();

            //Mat img = bitmap.ToMat();

             //对输入图像数据进行处理
            Mat blob = DnnInvoke.BlobFromImage(img, 1.0f, new Size(ModelWidth, ModelHeight), new MCvScalar(), true, false);

            //进行图像分类预测
            Mat prob;

            net.SetInput(blob);
            prob = net.Forward();

            //得到最可能分类输出
            Mat probMat = prob.Reshape(1, 1);
            double minVal = 0;  //最小可能性
            // double maxVal = 0;  //最大可能性
            Point minLoc = new Point();
            //  Point maxLoc = new Point();

            CvInvoke.MinMaxLoc(probMat, ref minVal, ref maxVal, ref minLoc, ref maxLoc);
            double classProb = maxVal;     //最大可能性
            Point classNumber = maxLoc;    //最大可能性位置

            typeName = typeList[classNumber.X];
        }

        /// <summary>
        /// 拷贝函数
        /// </summary>
        /// <param name="dnn"></param>
        public void _CopyTo(ref DNN_OpenVINO dnn)
        {
            dnn.net = net;
            dnn.typeList = typeList;

            dnn.ModelHeight = ModelHeight;
            dnn.ModelWidth = ModelWidth;
        }
    }
}
