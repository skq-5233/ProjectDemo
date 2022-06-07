using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu;
using Emgu.CV;
using Emgu.Util;
using System.Runtime.Serialization;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV.Cuda;
using System.IO;
using NPOI;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System.Diagnostics;
using System.Threading;

namespace Template_Matching
{
    public partial class Form1 : Form
    {
        Image<Bgr, byte> temp;
        Image<Bgr, byte> match_img;
        //Mat match_img = new Mat();
        Mat convert_img = new Mat();
        Image<Bgr, Byte> match_img1;
        Image<Bgr, Byte> match_area_img;
        Image<Bgr, byte> match_area_img_1;
        Image<Gray, byte> match_area_img_out;//(定义一个灰度图)
        Image<Gray, byte> binary_img;

        Image<Bgr, byte> match_img_rec;//(add-rec-image);
        //Image<Bgr, byte> contour_img; 

        Mat corrosion1 = new Mat();//创建Mat，存储处理后的图像；
        Mat corrosion2 = new Mat();//创建Mat，存储处理后的图像；
        Mat corrosion_img = new Mat();//创建Mat，存储处理后的图像；
        Mat dilate_img = new Mat();//创建Mat，存储处理后的图像；
        Mat img7 = new Mat();//创建Mat，存储处理后的图像；
        Mat binary = new Mat();

        Image<Bgr, byte> binary_rec;

        Mat contour_img = new Mat();
        Mat swell = new Mat();//创建Mat，存储处理后的图像；
        Mat canny_img = new Mat(); //创建dst，用来存储Canny算子处理后的图像；


        //(add,2021-1229,获取文件名);
        string dbf_File = string.Empty;
        OpenFileDialog OpenFileDialog = new OpenFileDialog();
        private int indexOfExcel_j = 1;
        FileStream filestream = null;

        XSSFWorkbook wk = null;
        ISheet isheet = null;
        IRow rowWrite = null;

        double max = 0, min = 0;//创建double极值；
        Point max_loc = new Point(0, 0), min_loc = new Point(0, 0);//创建dPoint类型，表示极值的坐标；

        //加载整个文件夹(2022-0106--start)；
        List<string> typeList = new List<string>();

        String defaultfilepath;
        Int32 picturecount = 0;  //图片总数
        //Int32[] classcount;      //各类图片计数

        Int32 classcount = 1;      //各类图片计数（修改2022-0113）；

        Thread thread1;
        Int32 ComboBoxindex = 0;
        //加载整个文件夹(2022-0106--end)；

        string path = string.Empty;
        string path1 = string.Empty;

        DateTime now = new DateTime();

        //Form form_mask = new Form();//实体化一个Form类; 


        /*******************************
        ******2022-0314-add--start******
        *******************************/
        struct LocRectangle
        {
            //public int locIndex;
            public int x1;
            public int y1;
            public int x2;
            public int y2;
        }

        private bool ifListBoxDel = false;  //标记listbox1是否删除了item，未删除=false，删除=true

        private int deleteindexRectangle;
        private int g_totalRectangle = -1;       //"区域+g_indexRectangle"       

        List<LocRectangle> rectangleLocations = new List<LocRectangle>();

        /*******************************
        ******2022-0314-add--end******
        *******************************/

        //获取和设置包含该应用程序的目录的名称(0228)
        //获取当前程序运行路径；
        string str = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

        public Form1()
        {
            InitializeComponent(); //初始化函数；
            defaultfilepath = "";  //默认文件夹路径；
            //uiHeaderButton1.Selected = true; //设置默认手动模式button打开； 
            label18.Text = "";


        }

        //点击右上角图标，实现退出功能(2022-0118)；
        private void uiHeaderButton3_Click(object sender, EventArgs e) //退出按钮；
        {
            Application.Exit(); //退出程序；
            //showCloseConfirmBox("确认关闭软件？", "close");
        }



        //设置默认手动模式button；
        #region
        //模式切换选择（2022-0113）；
        //bool auto_or_manual = true;

        //模式切换选择（2022-0113--start）；
        //手动模式（2022-0117）；
        //private void uiHeaderButton1_Click(object sender, EventArgs e)
        //{

        //if (auto_or_manual)
        //{
        //uiHeaderButton1.IsPress = true; //设置默认手动模式button打开；
        //uiHeaderButton2.IsPress = false; //设置默认自动模式button关闭；
        //groupBox3.Visible = false;
        //groupBox1.Visible = true;

        //tabPage1.Visible = true;
        //tabPage2.Visible = false;

        //uiTabControl1.SelectedIndex = 0;//从0开始，第一个tabpage(work-2022-0213);

        //    auto_or_manual = !auto_or_manual;
        //}
        //else
        //{
        //    groupBox3.Visible = true;
        //    groupBox1.Visible = false;
        //    auto_or_manual = !auto_or_manual;
        //}
        //}

        //自动模式（2022-0117）；
        //private void uiHeaderButton2_Click(object sender, EventArgs e)
        //{
        //可直接在起始界面点击自动模式(2022-0118)；
        //if (auto_or_manual)
        //{
        //uiHeaderButton1.IsPress = true; //设置默认手动模式button打开；
        //uiHeaderButton2.IsPress = false; //设置默认自动模式button关闭；
        //groupBox3.Visible = false;
        //groupBox1.Visible = true;

        //tabPage1.Visible = true;
        //tabPage2.Visible = false;
        //uiTabControl1.SelectedIndex = 1;//从0开始，第二个tabpage(work-2022-0213)；

        //    auto_or_manual = !auto_or_manual;
        //}
        //可直接在起始界面点击自动模式(2022-0118)；

        //if (!auto_or_manual)
        //{
        //    uiHeaderButton1.IsPress = false; //设置默认手动模式button关闭；
        //    uiHeaderButton2.IsPress = true; //设置默认自动模式button打开；
        //    groupBox3.Visible = true;
        //    groupBox1.Visible = false;

        //    //tabPage1.Visible = false;
        //    //tabPage2.Visible = true;

        //    auto_or_manual = !auto_or_manual;
        //}
        //}
        //模式切换选择（2022-0113--end）；
        #endregion

        private void LoadTemplate1_Click(object sender, EventArgs e)
        {
            /*
            ********手动模式下，测试单张图像--1(2022-0113--start)*********************
            ************************************************************************
            ******************加载模板图像--1(2022-0113--start)**********************
            */

            // 加载模板图像；
            OpenFileDialog OpenFileDialog = new OpenFileDialog();
            OpenFileDialog.Filter = "JPEG;BMP;PNG;JPG;GIF|*.JPEG;*.BMP;*.PNG;*.JPG;*.GIF|ALL|*.*";//（模板和加载图像尺寸不一致时，会报错）;


            if (OpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                //(add-2022-0113,获取图像路径--start);
                dbf_File = OpenFileDialog.FileName;
                //string dbf_File1 = System.IO.Path.GetFileName(dbf_File);

                string dbf_File2 = System.IO.Path.GetFileNameWithoutExtension(dbf_File); //得到文件名
                //(add-2022-0113,获取图像路径--end);

                //string dbf_File2 = System.IO.Path.GetDirectoryName(OpenFileDialog.FileName);//得到路径


                try
                {
                    temp = new Image<Bgr, Byte>(OpenFileDialog.FileName);
                    pictureBox1.Image = temp.ToBitmap();

                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("图像格式错误！");
                }
                

                //获取当前程序运行路径；
                //if (!Directory.Exists(str))
                //{
                //    Directory.CreateDirectory(str);
                //}

                //add--修改路径问题（设为本地路径(与.exe同一路径)--start）
                path = str + "\\Result\\Temp_result";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                //add--修改路径问题（设为本地路径(与.exe同一路径)--end）
                                             
                CvInvoke.Imwrite(path  + "\\" + dbf_File2 + "_temp.bmp", temp); //保存至本地文件夹Result;
                CvInvoke.WaitKey(0); //暂停按键等待               
                /*
                ********手动模式下，测试单张图像--1(2022-0113--end)*********************
                **********************************************************************
                ******************加载模板图像--1(2022-0113--end)**********************
                */
            }
        }

        private void Matchimage2_Click(object sender, EventArgs e)
        {
            /*
            ********手动模式下，测试单张图像--2(2022-0113--start)***************
            ******************************************************************
            ******加载待匹配图像并画出矩形框--2(2022-0113--start)***************
            */


            // 加载待匹配图像并画出矩形框；
            //OpenFileDialog OpenFileDialog = new OpenFileDialog();
            OpenFileDialog.Filter = "JPEG;BMP;PNG;JPG;GIF|*.JPEG;*.BMP;*.PNG;*.JPG;*.GIF|ALL|*.*";//（模板和加载图像尺寸不一致时，会报错）;

            //OpenFileDialog.Multiselect = true;//(2021-1231)该值确定是否可以选择多个文件;

            if (OpenFileDialog.ShowDialog() == DialogResult.OK)
            {

                //string[] files = OpenFileDialog.FileNames;//(2021-1231)该值确定是否可以选择多个文件;

                //(add-2022-0113,获取图像路径--start);
                dbf_File = OpenFileDialog.FileName;
                //string dbf_File1 = System.IO.Path.GetFileName(dbf_File);

                string dbf_File2 = System.IO.Path.GetFileNameWithoutExtension(dbf_File); // for getting only MyFile
                                                                                         //(add-2022-0113,获取图像路径--end);

                try
                {
                    match_img = new Image<Bgr, Byte>(OpenFileDialog.FileName);
                    Mat result = new Mat(new Size(match_img.Width - temp.Width + 1, match_img.Height - temp.Height + 1), DepthType.Cv32F, 1);//创建mat存储输出匹配结果；
                    CvInvoke.MatchTemplate(match_img, temp, result, TemplateMatchingType.CcorrNormed);//采用系数匹配法，数值越大越接近准确图像；


                    // 模板与匹配区域差运算匹配方法;
                    #region 
                    /*模板与匹配区域差运算；        
                    Sqdiff = 0(平方差匹配，最好匹配为0)；
                    SqdiffNormed = 1(归一化平方差匹配，最好结果为0)；
                    Ccorr = 2(相关匹配法，数值越大效果越好）；
                    CcorrNormed = 3(归一化相关匹配法，数值越大效果越好）；
                    Coeff = 4(系数匹配法，数值越大效果越好）；
                    CoeffNormed = 5(归一化系数匹配法，数值越大效果越好）;*/
                    #endregion

                    //CvInvoke.Normalize(result, result, 1, 0, Emgu.CV.CvEnum.NormType.MinMax); //对数据进行（min,max;0-255）归一化；
                    //double max = 0, min = 0;//创建double极值；
                    //Point max_loc = new Point(0, 0), min_loc = new Point(0, 0);//创建dPoint类型，表示极值的坐标；

                    CvInvoke.MinMaxLoc(result, ref min, ref max, ref min_loc, ref max_loc);//获取极值及其坐标；

                    match_img1 = match_img.Copy(); //将原图match_img复制到match_img1中，对match_img1进行画矩形框，避免pictureBox3显示匹配区域出现边框；
                    CvInvoke.Rectangle(match_img1, new Rectangle(max_loc, temp.Size), new MCvScalar(0, 255, 0), 3);//绘制矩形，匹配得到的结果；
                    pictureBox2.Image = match_img1.ToBitmap();//显示找到模板图像的待搜索图像；

                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("图像格式错误！");
                }
                
                //add--修改路径问题（设为本地路径(与.exe同一路径)--start）
                path = str + "\\Result\\Match_result";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                //add--修改路径问题（设为本地路径(与.exe同一路径)--end）

                //显示、保存图像；               
                //CvInvoke.Imshow("img", temp); //显示图片
                CvInvoke.Imwrite(path + "\\" + dbf_File2 + "_match_img1.bmp", match_img1); //保存匹配结果图像(含矩形框)；
                CvInvoke.WaitKey(0); //暂停按键等待
                

                /*
                ********手动模式下，测试单张图像--2(2022-0113--end)*****************
                ******************************************************************
                ******加载待匹配图像并画出矩形框--2(2022-0113--end)*****************
                */

            }

        }

        private void MatchingArea3_Click(object sender, EventArgs e)
        {
            /*
            ********手动模式下，测试单张图像--3(2022-0113--start)************************
            ***************************************************************************
            ******获取匹配区域并打印匹配信息--3(2022-0113--start)************************
            */


            // 模板与匹配区域差运算匹配方法;
            #region 
            /*模板与匹配区域差运算；        
            Sqdiff = 0(平方差匹配，最好匹配为0)；
            SqdiffNormed = 1(归一化平方差匹配，最好结果为0)；
            Ccorr = 2(相关匹配法，数值越大效果越好）；
            CcorrNormed = 3(归一化相关匹配法，数值越大效果越好）；
            Coeff = 4(系数匹配法，数值越大效果越好）；
            CoeffNormed = 5(归一化系数匹配法，数值越大效果越好）;*/
            #endregion

            // 获取匹配区域；
            Mat result = new Mat(new Size(match_img.Width - temp.Width + 1, match_img.Height - temp.Height + 1), DepthType.Cv32F, 1);//创建mat存储输出匹配结果；
            CvInvoke.MatchTemplate(match_img, temp, result, TemplateMatchingType.CcorrNormed);//采用系数匹配法(CcorrNormed)，打印出匹配相似度信息,数值越大越接近准确图像；

            //CvInvoke.Normalize(result, result, 1, 0, Emgu.CV.CvEnum.NormType.MinMax); //对数据进行（min,max;0-255）归一化；
            //double max = 0, min = 0;//创建double极值；
            //Point max_loc = new Point(0, 0), min_loc = new Point(0, 0);//创建dPoint类型，表示极值的坐标；

            CvInvoke.MinMaxLoc(result, ref min, ref max, ref min_loc, ref max_loc);//获取极值及其坐标；

            //(add-2022-0113,获取图像路径--start);
            dbf_File = OpenFileDialog.FileName;
            //string dbf_File1 = System.IO.Path.GetFileName(dbf_File);

            string dbf_File2 = System.IO.Path.GetFileNameWithoutExtension(dbf_File); // for getting only MyFile
            //(add-2022-0113,获取图像路径--end);

            match_area_img = match_img.Copy(new Rectangle(max_loc, temp.Size)); //将原图match_img复制到match_area_img中，显示匹配区域；
            pictureBox3.Image = match_area_img.ToBitmap();//显示匹配区域；
          
            //add--修改路径问题（设为本地路径(与.exe同一路径)--start）
            path = str + "\\Result\\Match_Area_result";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            //add--修改路径问题（设为本地路径(与.exe同一路径)--end）

            //显示、保存图像；           
            //CvInvoke.Imshow("img", temp); //显示图片
            CvInvoke.Imwrite(path + "\\" + dbf_File2 + "_match_area_img.bmp", match_area_img); //保存匹配区域结果图像；
            CvInvoke.WaitKey(0); //暂停按键等待
          
            //add--修改路径问题（设为本地路径(与.exe同一路径)--start）
            path = str + "\\Result\\Text_result";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            //add--修改路径问题（设为本地路径(与.exe同一路径)--end）

            ////datetime格式化；
            DateTime dt = DateTime.Now;

            //打印匹配信息（2021-1228,保存文本信息至指定文件夹）；
            displab.Text = "时间:" + dt.ToLocalTime().ToString() + "\n" + "图像名称："  +"\n" +dbf_File2 + "\n" +
                            "\n" + "匹配信息: X= " +
                             max_loc.X + "," + " Y= " + max_loc.Y + ";" +
                            "\n" +
                            "最大相似度: " + max.ToString("f2") + ";" + "\n" +
                            "最小相似度: " + min.ToString("f2") + ";" + "\n" + "\n";

            //（2021-1228,保存文本信息至指定文件夹）；
            string txt = displab.Text;
            string filename = path + "\\" + "匹配信息.txt";   //文件名，可以带路径

            FileStream fs = new FileStream(filename, FileMode.Append);
            StreamWriter wr = null;
            wr = new StreamWriter(fs);

            //System.IO.StreamWriter sw = new System.IO.StreamWriter(filename);
            wr.Write(displab.Text);
            wr.Close();

            /*
            ********手动模式下，测试单张图像--3(2022-0113--end)************************
            ***************************************************************************
            ******获取匹配区域并打印匹配信息--3(2022-0113--end)************************
            */
        }


        private void MatchDifference4_Click(object sender, EventArgs e)
        {
            /*
            ********手动模式下，测试单张图像--4(2022-0114--start)******************
            *********************************************************************
            ******模板与匹配区域差运算--4(2022-0114--start)************************
            */

            // 模板与匹配区域差运算匹配方法;
            #region 
            /*模板与匹配区域差运算；        
            Sqdiff = 0(平方差匹配，最好匹配为0)；
            SqdiffNormed = 1(归一化平方差匹配，最好结果为0)；
            Ccorr = 2(相关匹配法，数值越大效果越好）；
            CcorrNormed = 3(归一化相关匹配法，数值越大效果越好）；
            Coeff = 4(系数匹配法，数值越大效果越好）；
            CoeffNormed = 5(归一化系数匹配法，数值越大效果越好）;*/
            #endregion

            //(add-2022-0113,获取图像路径--start);
            dbf_File = OpenFileDialog.FileName;
            //string dbf_File1 = System.IO.Path.GetFileName(dbf_File);

            string dbf_File2 = System.IO.Path.GetFileNameWithoutExtension(dbf_File); // for getting only MyFile
            //(add-2022-0113,获取图像路径--end);

            Mat result = new Mat(new Size(match_img.Width - temp.Width + 1, match_img.Height - temp.Height + 1), DepthType.Cv32F, 1);//创建mat存储输出匹配结果；
            CvInvoke.MatchTemplate(match_img, temp, result, TemplateMatchingType.CcorrNormed);//采用系数匹配法，数值越大越接近准确图像；

            //CvInvoke.Normalize(result, result, 255, 0, Emgu.CV.CvEnum.NormType.MinMax); //对数据进行（min,max;0-255）归一化；
            //double max = 0, min = 0;//创建double极值；
            //Point max_loc = new Point(0, 0), min_loc = new Point(0, 0);//创建dPoint类型，表示极值的坐标；

            CvInvoke.MinMaxLoc(result, ref min, ref max, ref min_loc, ref max_loc);//获取极值及其坐标；

            //打印labbel信息；
            #region
            /*displab.Text =
                "匹配信息: max value" + "\n" +
                max.ToString() + "\n" +
                "min value " + min.ToString() + "\n" +
                "min_loc value " + min_loc.ToString() + "\n" +
                "max_loc value " + max_loc.ToString();*/
            #endregion

            match_area_img = match_img.Copy(new Rectangle(max_loc, temp.Size)); //将原图match_img复制到match_area_img中，显示匹配区域；

            match_area_img_1 = match_area_img.AbsDiff(temp);//(1220-add,temp\img2绝对值差值)；
            match_area_img_out = match_area_img_1.Convert<Gray, Byte>();//（将match_area_img_1转化为灰度图）;(image定义的图像通过Convert转化为灰度图);
            pictureBox4.Image = match_area_img_out.ToBitmap();//显示模板图像与匹配区域的差运算结果；

            //(2021-1230)
            binary_img = match_area_img_out;//将模板图像temp-匹配区域imge2的差运算结果（match_area_img_out）拷贝到binary_img;
            //(2021-1230)           

            //add--修改路径问题（设为本地路径(与.exe同一路径)--start）
            path = str + "\\Result\\Match_Area_Difference_result";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            //add--修改路径问题（设为本地路径(与.exe同一路径)--end）

            //显示、保存图像；
            #region
            ////CvInvoke.Imshow("img", temp); //显示图片
            CvInvoke.Imwrite(path + "\\" + dbf_File2 + "_match_area_img_out.bmp", match_area_img_out); //保存模板与匹配区域差运算结果图像；
            CvInvoke.WaitKey(0); //暂停按键等待           
            #endregion

            /*
            ********手动模式下，测试单张图像--4(2022-0114--end)******************
            *******************************************************************
            ******模板与匹配区域差运算--4(2022-0114--end)************************
            */
        }

        private void MatchingBinarization5_Click(object sender, EventArgs e)
        {

            /*
            ********手动模式下，测试单张图像--5(2022-0114--start)************************
            ***************************************************************************
            ******模板与匹配区域差二值化运算--5(2022-0114--start)*************************
            */

            //if (ifListBoxDel == false)
            //{
                //(add-2022-0113,获取图像路径--start);
                dbf_File = OpenFileDialog.FileName;
                //string dbf_File1 = System.IO.Path.GetFileName(dbf_File);

                string dbf_File2 = System.IO.Path.GetFileNameWithoutExtension(dbf_File); // for getting only MyFile
                                                                                         //(add-2022-0113,获取图像路径--start);

                //利用textBox1自适应调整像素阈值；
                int pixel_num = Convert.ToInt32(textBox1.Text);//实现string类型到int类型的转换;

                //add(2021-1227,num>255,num=255;num<0,num=0);
                if (pixel_num > 255)
                {
                    pixel_num = 255;
                }
                if (pixel_num <= 0)
                {
                    pixel_num = 0;
                }
                //add(2021-1227,num>255,num=255;num<0,num=0);

                CvInvoke.Threshold(binary_img, binary, pixel_num, 255, ThresholdType.Binary);//num自适应调节;

                pictureBox5.Image = binary.ToImage<Gray, byte>().ToBitmap();
                corrosion_img = binary;

                //add--修改路径问题（设为本地路径(与.exe同一路径)--start）
                path = str + "\\Result\\Binary_result";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                //add--修改路径问题（设为本地路径(与.exe同一路径)--end）

                //显示、保存图像；          
                ////CvInvoke.Imshow("img", temp); //显示图片
                CvInvoke.Imwrite(path + "\\" + dbf_File2 + "_binary.bmp", binary); //保存模板与匹配区域差二值化运算结果图像；
                //CvInvoke.WaitKey(0); //暂停按键等待

                //ifListBoxDel = !ifListBoxDel;
            //}

            //else
            //{
               
            
            /*******************************
             ******2022-0314-add--start******
             *******************************/
            //首先存储坐标;
            //int point_x1 = Convert.ToInt32(textBox1.Text);//实现string类型到int类型的转换;
            //int point_y1 = Convert.ToInt32(textBox4.Text);//实现string类型到int类型的转换;
            //int point_x2 = Convert.ToInt32(textBox3.Text);//实现string类型到int类型的转换;
            //int point_y2 = Convert.ToInt32(textBox3.Text);//实现string类型到int类型的转换;

            //LocRectangle locRectangle;
            ////locRectangle.locIndex = g_totalRectangle;   //当前添加的坐标是第几个区域的坐标
            //locRectangle.x1 = point_x1;
            //locRectangle.y1 = point_y1;
            //locRectangle.x2 = point_x2;
            //locRectangle.y2 = point_y2;

            //rectangleLocations.Add(locRectangle);   //存储当前四个坐标值
            //binary_rec = match_img_rec;

            //if (ifListBoxDel == false)
            //{
            //    //取坐标、生成框(使用match_img图像无法画出多矩形框，仅可画出一个？？0302)
            //    CvInvoke.Rectangle(binary_rec, new Rectangle(new Point(locRectangle.x1, locRectangle.y1), new Size(locRectangle.x2 - locRectangle.x1, locRectangle.y2 - locRectangle.y1)), new MCvScalar(0, 255, 0), 1);//绘制矩形，匹配得到的结果(1：调整矩形粗细)；
            //    //CvInvoke.Rectangle(temp, new Rectangle(new Point(rectangleLocations[index].x1, rectangleLocations[index].y1), new Size(100, 100)), new MCvScalar(0, 255, 0), 3);//绘制矩形，匹配得到的结果；               

            //    //创建一矩形，左上角坐标为(80,80)，大小为50*50;                      
            //    //CvInvoke.Rectangle(match_img1, new Rectangle(new Point(80, 80), new Size(50, 50)), new MCvScalar(0, 255, 0), 3);//绘制矩形，匹配得到的结果；

            //    //CvInvoke.Rectangle(match_img1, new Rectangle(max_loc, temp.Size), new MCvScalar(0, 255, 0), 3);//绘制矩形，匹配得到的结果；
            //    //pictureBox5.Image = binary_rec.ToBitmap();//显示找到模板图像的待搜索图像；
            //    pictureBox5.Image = binary_rec.ToImage<Gray, byte>().ToBitmap();

            //add--修改路径问题（设为本地路径(与.exe同一路径)--start）
            //path = str + "\\Template-Result\\Match_result";

            //if (!Directory.Exists(path))
            //{
            //    Directory.CreateDirectory(path);
            //}
            //add--修改路径问题（设为本地路径(与.exe同一路径)--end）
            //string dbf_File2 = Path.GetFileNameWithoutExtension(dbf_File); // for getting only MyFile

            //Mat binary_rec = CvInvoke.Imread("Image0025_binary.bmp"); //读取图片
            //pictureBox5.Image = binary_rec.ToBitmap();

            //显示、保存图像；
            //CvInvoke.Imshow("img", temp); //显示图片
            //CvInvoke.Imwrite(path + "\\" + dbf_File2 + "_temp.bmp", temp); //保存匹配结果图像(含矩形框)；
            //CvInvoke.Imwrite(path + "\\" + dbf_File2 + "_binary_rec.bmp", binary_rec); //保存匹配结果图像(含矩形框)；
            //CvInvoke.WaitKey(0); //暂停按键等待
            //}
            //else
            //{
            //    ifListBoxDel = !ifListBoxDel;
            //}
            // ifListBoxDel = !ifListBoxDel;
            //}




            /*******************************
             ******2022-0314-add--end******
             *******************************/













            /*
            ********手动模式下，测试单张图像--5(2022-0114--end)*************************
            **************************************************************************
            ******模板与匹配区域差二值化运算--5(2022-0114--end)*************************
            */
        }

        private void erode6_Click(object sender, EventArgs e)
        {

            /*
            ********手动模式下，测试单张图像--6(2022-0117--start)************************
            ***************************************************************************
            ******模板与匹配区域差二值化运算腐蚀操作--6(2022-0117--start)*****************
            */

            // Tab (代码整体左移)或 Shift+Tab(代码整体右移) ；

            //(add-2022-0117,获取图像路径--start);
            dbf_File = OpenFileDialog.FileName;
            //string dbf_File1 = System.IO.Path.GetFileName(dbf_File);

            string dbf_File2 = System.IO.Path.GetFileNameWithoutExtension(dbf_File); // for getting only MyFile
            //(add-2022-0117,获取图像路径--start);


            Mat struct_element = CvInvoke.GetStructuringElement(ElementShape.Cross, new Size(3, 3), new Point(-1, -1));//指定参数获得结构元素；
            //指定参数进行形态学开（先腐蚀后膨胀）操作；
            //CvInvoke.MorphologyEx(corrosion_img,corrosion1,Emgu.CV.CvEnum.MorphOp.Open, struct_element, new Point(-1, -1), 3, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));

            //new Point(-1, -1),表示结构元素中心，3，表示腐蚀迭代次数；


            int erode_num = Convert.ToInt32(textBox3.Text);//实现string类型到int类型转换（默认腐蚀次数erode_num=1，点击一次腐蚀一次）；

            CvInvoke.Erode(corrosion_img, corrosion1, struct_element, new Point(-1, -1), erode_num, BorderType.Default, new MCvScalar(0, 0, 0));//指定参数进行形态学腐蚀操作（自适应手动调整输入腐蚀次数）；

            //CvInvoke.Erode(corrosion_img, corrosion1, struct_element, new Point(-1, -1), 2, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));//指定参数进行形态学腐蚀操作（2：腐蚀2次）；

            pictureBox5.Image = corrosion1.ToImage<Gray, byte>().ToBitmap();
            corrosion_img = corrosion1;//（将第一次腐蚀的结果作为第二次腐蚀的输入，如此循环，一直腐蚀，直至消失）;

            dilate_img = corrosion1;//（将腐蚀结果corrosion1复制给dilate_img，作为膨胀的输入）;          

            //add--修改路径问题（设为本地路径(与.exe同一路径)--start）
            path = str + "\\Result\\Corrosion_result";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            //add--修改路径问题（设为本地路径(与.exe同一路径)--end）

            //显示、保存图像；
            #region
            //CvInvoke.Imshow("img", temp); //显示图片
            CvInvoke.Imwrite(path + "\\" + dbf_File2 + "_corrosion1.bmp", corrosion1); //保存匹配结果图像；
            CvInvoke.WaitKey(0); //暂停按键等待
            #endregion

            /*
            ********手动模式下，测试单张图像--6(2022-0117--end)*************************
            **************************************************************************
            ******模板与匹配区域差二值化运算腐蚀操作--6(2022-0117--end)******************
            */
        }

        private void dilate7_Click(object sender, EventArgs e)
        {
            /*
            ********手动模式下，测试单张图像--7(2022-0117--start)************************
            ***************************************************************************
            ******模板与匹配区域差二值化结果膨胀运算--7(2022-0117--start)*****************
            */

            //形态学操作类型（add-2021-1228）
            #region
            //Erode = 0;(形态学腐蚀，消除边界点，使边界向内收缩，可消除没有意义的物体。使用CvInvoke.Erode)；
            //Dilate =1;(形态学膨胀，将与物体接触的所有背景点合并到该物体中，使边界向外扩张，可用来填补物体中的空洞。使用 CvInvoke.Dilate)；
            //open = 2;(形态学开操作，先腐蚀，后膨胀。即：OPEN(X)=D(E(X))，（使用CvInvoke.MorphologyEx（Emgu.CV.CvEnum.MorphOp.Open）作用：消除小物体，在纤细点处分离物体，位置和形状总是不变)；
            //Close = 3;(形态学闭操作，先膨胀，后腐蚀。即：CLOSE(X)=E(D(X)),（使用CvInvoke.MorphologyEx（Emgu.CV.CvEnum.MorphOp.Close））作用：可填平小孔，位置、形状不变。)
            //Gradient = 4;(形态学梯度操作，膨胀与腐蚀差。即：Grad(X)=Dilate(X)-Erode(X)。膨胀：扩大图像的边界，腐蚀：缩小图像的边界，膨胀-腐蚀：即为形态学梯度操作，可保存边缘轮廓)；
            //Tophat = 5;(形态学高帽，原始图像-开操作图像，即：Tophat(X)=X-Open（X）)；
            //Blackhat = 6;(形态学地帽，闭操作图像-原始图像，即：Blackhat(X)=Close(X)-X,结果是：输出图像大部分面积均为偏黑色，故称黑帽操作);
            //形态学操作类型（add-2021-1228）
            #endregion

            //(add-2022-0117,获取图像路径--start);
            dbf_File = OpenFileDialog.FileName;
            //string dbf_File1 = System.IO.Path.GetFileName(dbf_File);

            string dbf_File2 = System.IO.Path.GetFileNameWithoutExtension(dbf_File); // for getting only MyFile
            //(add-2022-0117,获取图像路径--start);

            Mat struct_element = CvInvoke.GetStructuringElement(ElementShape.Cross, new Size(3, 3), new Point(-1, -1));//指定参数获得结构元素；
            //指定参数进行形态学开操作； //new Point(-1, -1),表示结构元素中心，3，表示形态学开操作中腐蚀迭代次数；
            //CvInvoke.MorphologyEx(corrosion_img, corrosion1, Emgu.CV.CvEnum.MorphOp.Open, struct_element, new Point(-1, -1), 3, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));

            int dilate_num = Convert.ToInt32(textBox4.Text);//实现string类型到int类型转换（默认腐蚀次数dilate_num=1，点击一次膨胀一次）；

            CvInvoke.Dilate(dilate_img, swell, struct_element, new Point(-1, -1), dilate_num, BorderType.Default, new MCvScalar(0, 0, 0));//指定参数进行膨胀操作（默认腐蚀次数dilate_num=1，点击一次膨胀一次）；

            //CvInvoke.Dilate(dilate_img, swell, struct_element, new Point(-1, -1), 2, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));//指定参数进行膨胀操作（2：膨胀2次）；
            pictureBox5.Image = swell.ToImage<Gray, byte>().ToBitmap();

            dilate_img = swell; //（将第一次膨胀的结果作为第二次膨胀的输入，如此循环，一直膨胀）;
            
            //add--修改路径问题（设为本地路径(与.exe同一路径)--start）
            path = str + "\\Result\\Swell_result";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            //add--修改路径问题（设为本地路径(与.exe同一路径)--end）

            //显示、保存图像；
            #region
            //CvInvoke.Imshow("img", temp); //显示图片
            CvInvoke.Imwrite(path + "\\" + dbf_File2 + "_swell.bmp", swell); //保存匹配结果图像；
            CvInvoke.WaitKey(0); //暂停按键等待
            #endregion

            /*
            ********手动模式下，测试单张图像--7(2022-0117--end)************************
            ***************************************************************************
            ******模板与匹配区域差二值化结果膨胀运算--7(2022-0117--end)*****************
            */
        }


        private void BinarizedContour8_Click(object sender, EventArgs e)
        {
            /*
            ********手动模式下，测试单张图像--8(2022-0117--start)*********************************
            ************************************************************************************
            ******模板与匹配区域差二值化轮廓阈值运算--8********************************************
            *****将两次文本信息打印拼接在一起，使用stream将信息写入Excel表中--8(2022-0117--start)***
            */

            //(add-2022-0117,获取图像路径--start);
            dbf_File = OpenFileDialog.FileName;
            //string dbf_File1 = System.IO.Path.GetFileName(dbf_File);

            string dbf_File2 = System.IO.Path.GetFileNameWithoutExtension(dbf_File); // for getting only MyFile
            //(add-2022-0117,获取图像路径--start);

            contour_img = swell;

            CvInvoke.Canny(contour_img, canny_img, 120, 180); //指定阈值（第一阈值：120，第二阈值：180）进行Canny算子处理(这里是否需要Canny算子？？)；                       

            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();//创建VectorOfVectorOfPoint类型，用于存储查找到的轮廓；
            //add(1215，用于面积筛选)
            VectorOfVectorOfPoint use_contours = new VectorOfVectorOfPoint();//创建VectorOfVectorOfPoint类型，用于存储筛选后的轮廓；
            //add(1215，用于面积筛选)

            //(RetrType.External:提取最外层轮廓；RetrType.List:提取所有轮廓;Emgu.CV.CvEnum.RetrType.Ccomp检索所有轮廓，并将它们组织成两级层级结构：水平是组件的外部边界，二级约束边界的洞）；
            CvInvoke.FindContours(canny_img, contours, null, RetrType.Ccomp, ChainApproxMethod.ChainApproxSimple);//查找dst所有轮廓并保存（contour_img<-->canny_img）；                                                            

            //add(2021-1217，面积筛选)
            //遍历完所有面积，打印总面积；
            double sum_area = 0;
            int ksize = contours.Size;//获取连通区域的个数；
            for (int i = 0; i < ksize; i++)//遍历每个连通轮廓；
            {
                VectorOfPoint contour = contours[i];//获取独立的连通轮廓；
                double area = CvInvoke.ContourArea(contour);//计算连通轮廓的面积；

                double area1 = Convert.ToDouble(textBox5.Text);//实现string类型到double类型的转换;
                //add(2021-1227,当输入面积小于0时，area==0);
                if (area1 <= 0)
                {
                    area1 = 0;
                }
                //add(2021-1227,当输入面积小于0时，num1==0);

                //add(2021-1227，当轮廓面积大于阈值面积时，才执行下列语句，实现面积相加，否则跳出条件语句）；
                if (area > area1)//对每一轮廓区域面积进行面积筛选；                                
                {

                    use_contours.Push(contour);//添加筛选后的连通轮廓；
                    sum_area += area;//遍历结果相加；

                }

            }
            //打印轮廓总面积信息；
            displab1.Text = "轮廓总面积: " + sum_area.ToString() + ";" + "\n" + "\n" + "\n";//打印遍历完的总面积（0044轮廓面积,当阈值选择30时，显示出错,试试RetrType.Ccomp）


            //打印匹配信息（2021-1228,保存文本信息至指定文件夹）；
            #region
            //displab.Text = dbf_File2 + "\n" +
            //                 "轮廓总面积:  " + sum_area.ToString();

            ////（2021-1228,保存文本信息至指定文件夹）；
            //string txt = displab1.Text;
            //string filename = "D:\\SKQ\\VS-Code\\Demo\\Emgu.CV.CvEnum\\Result\\匹配信息1.txt";   //文件名，可以带路径

            //System.IO.StreamWriter sw = new System.IO.StreamWriter(filename);
            //sw.Write(displab1.Text);
            //sw.Close();
            #endregion

            //add--修改路径问题（设为本地路径(与.exe同一路径)--start）
            path = str + "\\Result\\Text_result";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            //add--修改路径问题（设为本地路径(与.exe同一路径)--end）

            //(add,2021-1228,在匹配信息Txt文本中添加轮廓面积信息)；
            string txt = displab1.Text;
            string filename = path + "\\" +"匹配信息.txt";   //文件名，可以带路径

            FileStream fs = new FileStream(filename, FileMode.Append);
            StreamWriter wr = null;
            wr = new StreamWriter(fs);

            //System.IO.StreamWriter sw = new System.IO.StreamWriter(filename);
            wr.Write(displab1.Text);
            wr.Close();
            //(add,2021-1228,在匹配信息Txt文本中添加轮廓面积信息)；

            //(add-2021-1231)用流创建或读取.xlsx文件（同时流关联了文件）
            //add--修改路径问题（设为本地路径(与.exe同一路径)--start）
            path = str + "\\Result\\Excel_result";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            //add--修改路径问题（设为本地路径(与.exe同一路径)--end）

            filestream = new FileStream(path + "\\" + "匹配信息.xlsx", FileMode.OpenOrCreate);
            //(add-2021-1231)用流创建或读取.xlsx文件（同时流关联了文件）


            //创建表和sheet;
            if (indexOfExcel_j == 1)
            {
                wk = new XSSFWorkbook();   //创建表对象wk
                isheet = wk.CreateSheet("Sheet1");   //在wk中创建sheet1
                //创建表头
                IRow rowtitle = null;
                rowtitle = isheet.CreateRow(0); //创建index=j的行

                ICell cellTitle0 = null;
                cellTitle0 = rowtitle.CreateCell(0);  //在index=j的行中创建index=0的单元格
                cellTitle0.SetCellValue("时间"); //给创建的单元格赋值string

                ICell cellTitle1 = null;
                cellTitle1 = rowtitle.CreateCell(1);  //在index=j的行中创建index=0的单元格
                cellTitle1.SetCellValue("图像名称"); //给创建的单元格赋值string

                ICell cellTitle2 = null;
                cellTitle2 = rowtitle.CreateCell(2);  //在index=j的行中创建index=0的单元格
                cellTitle2.SetCellValue("匹配信息坐标X"); //给创建的单元格赋值string

                ICell cellTitle3 = null;
                cellTitle3 = rowtitle.CreateCell(3);  //在index=j的行中创建index=0的单元格
                cellTitle3.SetCellValue("匹配信息坐标Y"); //给创建的单元格赋值string

                ICell cellTitle4 = null;
                cellTitle4 = rowtitle.CreateCell(4);  //在index=j的行中创建index=0的单元格
                cellTitle4.SetCellValue("最大相似度"); //给创建的单元格赋值string


                ICell cellTitle5 = null;
                cellTitle5 = rowtitle.CreateCell(5);  //在index=j的行中创建index=0的单元格
                cellTitle5.SetCellValue("最小相似度"); //给创建的单元格赋值string

                ICell cellTitle6 = null;
                cellTitle6 = rowtitle.CreateCell(6);  //在index=j的行中创建index=0的单元格
                cellTitle6.SetCellValue("轮廓总面积"); //给创建的单元格赋值strin
            }


            //向单元格中写数据(2021-1231,循环写入行、列数据，先定义行数，再定义列数，这里是2行6列);
            #region
            //for (int indexOfExcel_j = 1; indexOfExcel_j < 3; indexOfExcel_j++)//(j:行)；
            //{}

            //IRow row2 = null;
            //row2 = isheet.CreateRow(indexOfExcel_j++); //创建index=j的行

            #endregion
            ////datetime格式化；
            DateTime dt = DateTime.Now;

            rowWrite = isheet.CreateRow(indexOfExcel_j++); //创建index=j的行

            ICell cell0 = null;
            cell0 = rowWrite.CreateCell(0);  //在index=j的行中创建index=0的单元格
            cell0.SetCellValue(dt.ToLocalTime().ToString()); //给创建的单元格赋值string

            ICell cell1 = null;
            cell1 = rowWrite.CreateCell(1);  //在index=j的行中创建index=0的单元格
            cell1.SetCellValue(dbf_File2); //给创建的单元格赋值string

            ICell cell2 = null;
            cell2 = rowWrite.CreateCell(2);  //在index=j的行中创建index=0的单元格
            cell2.SetCellValue("X:" + max_loc.X); //给创建的单元格赋值string

            ICell cell3 = null;
            cell3 = rowWrite.CreateCell(3);  //在index=j的行中创建index=0的单元格
            cell3.SetCellValue("Y:" + max_loc.Y); //给创建的单元格赋值string

            ICell cell4 = null;
            cell4 = rowWrite.CreateCell(4);  //在index=j的行中创建index=0的单元格
            cell4.SetCellValue(max.ToString("f2")); //给创建的单元格赋值string


            ICell cell5 = null;
            cell5 = rowWrite.CreateCell(5);  //在index=j的行中创建index=0的单元格
            cell5.SetCellValue(min.ToString("f2")); //给创建的单元格赋值string

            ICell cell6 = null;
            cell6 = rowWrite.CreateCell(6);  //在index=j的行中创建index=0的单元格
            cell6.SetCellValue(sum_area.ToString()); //给创建的单元格赋值string


            //通过流将表中写入的数据一次性写入文件中;
            wk.Write(filestream); //通过流filestream将表wk写入文件


            //(add,20211231)
            //filestream.Close(); //关闭文件流filestream
            //wk.Close(); //关闭Excel表对象wk


            Mat mask = contour_img.ToImage<Bgr, byte>().CopyBlank().Mat;//获取一张背景为黑色的图像，尺寸大小与contour_img一样(类型Bgr);
            CvInvoke.DrawContours(mask, use_contours, -1, new MCvScalar(0, 0, 255));//采用红色画笔在mask掩膜图像中画出所有轮廓；(MCvScalar是一个具有单元素到四元素间的一个数组)
            pictureBox6.Image = mask.ToImage<Bgr, byte>().ToBitmap();//显示轮廓区域图像；

           
            //add--修改路径问题（设为本地路径(与.exe同一路径)--start）
            path = str + "\\Result\\Contour_result";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            //add--修改路径问题（设为本地路径(与.exe同一路径)--end）
         
            //显示、保存图像；            
            //CvInvoke.Imshow("img", temp); //显示图片
            CvInvoke.Imwrite(path + "\\" + dbf_File2 + "_mask.bmp", mask); //保存匹配结果图像；
            CvInvoke.WaitKey(0); //暂停按键等待
            

            /*
            ********手动模式下，测试单张图像--8(2022-0117--end)**********************************
            ***********************************************************************************
            ******模板与匹配区域差二值化轮廓阈值运算--8*******************************************
            *****将两次文本信息打印拼接在一起，使用stream将信息写入Excel表中--8(2022-0117--end)****
            */
        }


        private void LoadTemplate2_Click(object sender, EventArgs e)
        {

            /*
            ********自动模式下，测试单张图像--2(2022-0118--start)*****************************
            ********************************************************************************
            ******加载模板图像--2(2022-0118--start)******************************************
            */

            OpenFileDialog OpenFileDialog = new OpenFileDialog();
            OpenFileDialog.Filter = "JPEG;BMP;PNG;JPG;GIF|*.JPEG;*.BMP;*.PNG;*.JPG;*.GIF|ALL|*.*";//（模板和加载图像尺寸不一致时，会报错）;
            if (OpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                //(add-2022-0117,获取图像路径--start);
                dbf_File = OpenFileDialog.FileName;
                //string dbf_File1 = System.IO.Path.GetFileName(dbf_File);

                string dbf_File2 = System.IO.Path.GetFileNameWithoutExtension(dbf_File); // for getting only MyFile
                //(add-2022-0117,获取图像路径--start);

                try
                {
                    temp = new Image<Bgr, Byte>(OpenFileDialog.FileName);
                    pictureBox12.Image = temp.ToBitmap();

                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("图像格式错误！");
                }

                //add--修改路径问题（设为本地路径(与.exe同一路径)--start）
                path = str + "\\Auto-Sigle-Image-Result\\Temp_result";
                path1 = str + "\\Auto-Folder-Image-Result\\Temp_result";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                if (!Directory.Exists(path1))
                {
                    Directory.CreateDirectory(path1);
                }
                //add--修改路径问题（设为本地路径(与.exe同一路径)--end）

                //add--修改路径问题（设为本地路径--start）
                #region
                //path = "\\Auto-Sigle-Image-Result\\Temp_result";
                //path1 = "\\Auto-Folder-Image-Result\\Temp_result";
                //if (!Directory.Exists(path))
                //{
                //    Directory.CreateDirectory(path);
                //}
                ////add--修改路径问题（设为本地路径--end）

                //if (!Directory.Exists(path1))
                //{
                //    Directory.CreateDirectory(path1);
                //}

                //(add,2021-1228,在匹配信息Txt文本中添加轮廓面积信息)；
                #endregion

                //显示、保存图像；                
                ////CvInvoke.Imshow("img", temp); //显示图片
                CvInvoke.Imwrite(path + "\\" + dbf_File2+ "_temp.bmp", temp); //保存匹配结果图像；
                CvInvoke.WaitKey(0); //暂停按键等待                

                //显示、保存图像；                
                ////CvInvoke.Imshow("img", temp); //显示图片
                CvInvoke.Imwrite(path1 + "\\" + dbf_File2 + "_temp.bmp", temp); //保存匹配结果图像；
                CvInvoke.WaitKey(0); //暂停按键等待
                

                /*
                ********自动模式下，测试单张图像--2(2022-0118--end)***************************************
                ***************************************************************************************
                ******加载模板图像--2(2022-0118--end)***************************************************
                */
            }
        }


        private void Matchimage2_1Click(object sender, EventArgs e)
        {

            /*
            ********自动模式下，测试单张图像--2(2022-0118--start)******************************************
            ********************************************************************************************
            ******加载待匹配图像并画出矩形框--2(2022-0118--start)******************************************
            */

            //OpenFileDialog OpenFileDialog = new OpenFileDialog();
            OpenFileDialog.Filter = "JPEG;BMP;PNG;JPG;GIF|*.JPEG;*.BMP;*.PNG;*.JPG;*.GIF|ALL|*.*";//（模板和加载图像尺寸不一致时，会报错）;

            OpenFileDialog.Multiselect = true;//(2021-1231)该值确定是否可以选择多个文件;

            if (OpenFileDialog.ShowDialog() == DialogResult.OK)
            {

                string[] files = OpenFileDialog.FileNames;//(2021-1231)该值确定是否可以选择多个文件;

                ///(add-2022-0117,获取图像路径--start);
                dbf_File = OpenFileDialog.FileName;
                //string dbf_File1 = System.IO.Path.GetFileName(dbf_File);

                string dbf_File2 = System.IO.Path.GetFileNameWithoutExtension(dbf_File); // for getting only MyFile
                //(add-2022-0117,获取图像路径--start);

                try
                {
                    match_img = new Image<Bgr, Byte>(OpenFileDialog.FileName);
                    Mat result1 = new Mat(new Size(match_img.Width - temp.Width + 1, match_img.Height - temp.Height + 1), DepthType.Cv32F, 1);//创建mat存储输出匹配结果；
                    CvInvoke.MatchTemplate(match_img, temp, result1, TemplateMatchingType.CcorrNormed);//采用系数匹配法，数值越大越接近准确图像；

                    //CvInvoke.Normalize(result1, result1, 1, 0, Emgu.CV.CvEnum.NormType.MinMax); //对数据进行（min,max;0-255）归一化；
                    //double max = 0, min1 = 0;//创建double极值；
                    //Point max_loc = new Point(0, 0), min_loc1 = new Point(0, 0);//创建dPoint类型，表示极值的坐标；
                    CvInvoke.MinMaxLoc(result1, ref min, ref max, ref min_loc, ref max_loc);//获取极值及其坐标；

                    match_img1 = match_img.Copy(); //将原图match_img复制到match_img1中，对match_img1进行画矩形框，避免pictureBox3显示匹配区域出现边框；
                    CvInvoke.Rectangle(match_img1, new Rectangle(max_loc, temp.Size), new MCvScalar(0, 255, 0), 3);//绘制矩形，匹配得到的结果；
                    pictureBox11.Image = match_img1.ToBitmap();//显示找到模板图像的待搜索图像；

                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("图像格式错误！");
                }
                
                //add--修改路径问题（设为本地路径(与.exe同一路径)--start）
                path = str + "\\Auto-Sigle-Image-Result\\Match_result";
               
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }                
                //add--修改路径问题（设为本地路径(与.exe同一路径)--end）

                //显示、保存图像；                
                //CvInvoke.Imshow("img", temp); //显示图片
                CvInvoke.Imwrite(path + "\\" + dbf_File2 +  "_match_img1.bmp", match_img1); //保存匹配结果图像；
                CvInvoke.WaitKey(0); //暂停按键等待
                

                /*
                ***************自动模式下，测试单张图像---2-加载待匹配图像并画出矩形框--end；***************************
                **********************************（add,2022-0118);************************************************
                **************************（Matchimage_Click）*****************************************************
                */



                /*
                *******************自动模式下，测试单张图像---3-获取匹配区域--start**********************************
                ******************* (add,2022-0118,获取匹配区域并打印匹配信息***************************************
                *************相当于(MatchingArea_Click));*********************************************************
                */

                // 获取匹配区域；
                Mat result = new Mat(new Size(match_img.Width - temp.Width + 1, match_img.Height - temp.Height + 1), DepthType.Cv32F, 1);//创建mat存储输出匹配结果；
                CvInvoke.MatchTemplate(match_img, temp, result, Emgu.CV.CvEnum.TemplateMatchingType.CcorrNormed);//采用系数匹配法(CcorrNormed)，打印出匹配相似度信息,数值越大越接近准确图像；
                //CvInvoke.Normalize(result, result, 1, 0, Emgu.CV.CvEnum.NormType.MinMax); //对数据进行（min,max;0-255）归一化；
                //double max = 0, min = 0;//创建double极值；
                //Point max_loc = new Point(0, 0), min_loc = new Point(0, 0);//创建dPoint类型，表示极值的坐标；
                CvInvoke.MinMaxLoc(result, ref min, ref max, ref min_loc, ref max_loc);//获取极值及其坐标；

                //(add-2021-1228,保存处理后的图像至指定文件夹)
                //dbf_File = OpenFileDialog.FileName;
                //string dbf_File1 = System.IO.Path.GetFileName(dbf_File);

                //string dbf_File2 = System.IO.Path.GetFileNameWithoutExtension(dbf_File); // for getting only MyFile
                //(add-2021-1228,保存处理后的图像至指定文件夹)

                match_area_img = match_img.Copy(new Rectangle(max_loc, temp.Size)); //将原图match_img复制到match_area_img中，显示匹配区域；
                pictureBox13.Image = match_area_img.ToBitmap();//显示匹配区域；
               
                //add--修改路径问题（设为本地路径(与.exe同一路径)--start）
                path = str + "\\Auto-Sigle-Image-Result\\Match_Area_result";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                //add--修改路径问题（设为本地路径(与.exe同一路径)--end）

                //显示、保存图像；               
                //CvInvoke.Imshow("img", temp); //显示图片
                CvInvoke.Imwrite(path + "\\" + dbf_File2 + "_match_area_img.bmp", match_area_img); //保存匹配结果图像；
                CvInvoke.WaitKey(0); //暂停按键等待
                

                ////datetime格式化；
                DateTime dt = DateTime.Now;
                              
                //add--修改路径问题（设为本地路径(与.exe同一路径)--start）
                path = str + "\\Auto-Sigle-Image-Result\\Text_result";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                //add--修改路径问题（设为本地路径(与.exe同一路径)--end）

                //打印匹配信息（2021-1228,保存文本信息至指定文件夹）；
                displab3.Text = "时间:" + dt.ToLocalTime().ToString() + "\n" + "图像名称：" + "\n"  + dbf_File2 + "\n" +
                                "\n" + "匹配信息: X= " +
                                 max_loc.X + "," + " Y= " + max_loc.Y + ";" +
                                "\n" +
                                "最大相似度: " + max.ToString("f2") + ";" + "\n" +
                                "最小相似度: " + min.ToString("f2") + ";" + "\n" + "\n";

                //（2021-1228,保存文本信息至指定文件夹）；
                string txt = displab3.Text;
                string filename = path + "\\" + "匹配信息.txt";   //文件名，可以带路径
               
                FileStream fs = new FileStream(filename, FileMode.Append);
                StreamWriter wr = null;
                wr = new StreamWriter(fs);

                //System.IO.StreamWriter sw = new System.IO.StreamWriter(filename);
                wr.Write(displab3.Text);
                wr.Close();
                //（2021-1228,保存文本信息至指定文件夹）；
                #region
                //（add-2021-1229）将文本信息（dislab.Text）保存至Excel表格中；
                //string sContext = displab.Text;// 控件的内容
                //string sExcelPath = "D:\\SKQ\\VS-Code\\Demo\\Emgu.CV.CvEnum\\Result\\匹配信息.xlsx"; //excel路径
                //int iSheet = 1;

                // 连接Excel
                //Application app = new Application();
                //WorkBook WB = app.Workbooks.Open(sExcelPath, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                //WorkSheet WS = (Worksheet)WB.Worksheets[iSheet];
                //Range range = (Range)worksheet.Cells[iRow, iCol]; ////得到指定行列的单元格
                //range.Value2 = sContext; // 对单元格赋值

                //（add-2021-1229）将文本信息（dislab.Text）保存至Excel表格中；



                //向单元格中写数据(2021-1229,循环写入行、列数据，先定义行数，再定义列数);

                ////datetime格式化；
                //DateTime dt1 = DateTime.Now;

                ////在Excel表中写入时间数据时，设置单元格格式，自定义格式yyyy-MM-dd HH:mm:ss
                //string current_time = dt1.ToString("yyyy-MM-dd HH:mm:ss.fff");   //mm-dd才显示08-01，否则显示8-1
                ////fff个数表示小数点后显示的位数，这里精确到小数点后3位


                //(add-2021-1228,保存处理后的图像至指定文件夹)
                //MessageBox.Show(dbf_File2);//add(filename-2021-1228);
                //(add-2021-1228,保存处理后的图像至指定文件夹)
                #endregion
                /*
                *******************自动模式下，测试单张图像---3-获取匹配区域--start**********************************
                ******************* (add,2022-0118,获取匹配区域并打印匹配信息***************************************
                *************相当于(MatchingArea_Click));*********************************************************
                */



                /*
                ********自动模式下，测试单张图像--4(2022-0120--start)***********************************************
                *************************************************************************************************
                ******模板与匹配区域差运算--4(2022-0120--start)****************************************************
                */

                // 模板与匹配区域差运算匹配方法;
                #region 
                /*模板与匹配区域差运算；        
                Sqdiff = 0(平方差匹配，最好匹配为0)；
                SqdiffNormed = 1(归一化平方差匹配，最好结果为0)；
                Ccorr = 2(相关匹配法，数值越大效果越好）；
                CcorrNormed = 3(归一化相关匹配法，数值越大效果越好）；
                Coeff = 4(系数匹配法，数值越大效果越好）；
                CoeffNormed = 5(归一化系数匹配法，数值越大效果越好）;*/
                #endregion

                //(add-2022-0113,获取图像路径--start);
                //dbf_File = OpenFileDialog.FileName;
                //string dbf_File1 = System.IO.Path.GetFileName(dbf_File);

                //string dbf_File2 = System.IO.Path.GetFileNameWithoutExtension(dbf_File); // for getting only MyFile
                //(add-2022-0113,获取图像路径--end);



                Mat result2 = new Mat(new Size(match_img.Width - temp.Width + 1, match_img.Height - temp.Height + 1), DepthType.Cv32F, 1);//创建mat存储输出匹配结果；
                CvInvoke.MatchTemplate(match_img, temp, result2, TemplateMatchingType.CcorrNormed);//采用系数匹配法，数值越大越接近准确图像；

                //CvInvoke.Normalize(result, result, 255, 0, Emgu.CV.CvEnum.NormType.MinMax); //对数据进行（min,max;0-255）归一化；
                //double max = 0, min = 0;//创建double极值；
                //Point max_loc = new Point(0, 0), min_loc = new Point(0, 0);//创建dPoint类型，表示极值的坐标；

                CvInvoke.MinMaxLoc(result2, ref min, ref max, ref min_loc, ref max_loc);//获取极值及其坐标；

                //打印labbel信息；
                #region
                /*displab.Text =
                    "匹配信息: max value" + "\n" +
                    max.ToString() + "\n" +
                    "min value " + min.ToString() + "\n" +
                    "min_loc value " + min_loc.ToString() + "\n" +
                    "max_loc value " + max_loc.ToString();*/
                #endregion

                match_area_img = match_img.Copy(new Rectangle(max_loc, temp.Size)); //将原图match_img复制到match_area_img中，显示匹配区域；

                match_area_img_1 = match_area_img.AbsDiff(temp);//(1220-add,temp\img2绝对值差值)；
                match_area_img_out = match_area_img_1.Convert<Gray, Byte>();//（将match_area_img_1转化为灰度图）;(image定义的图像通过Convert转化为灰度图);
                pictureBox10.Image = match_area_img_out.ToBitmap();//显示模板图像与匹配区域的差运算结果；

                //(2021-1230)
                binary_img = match_area_img_out;//将模板图像temp-匹配区域imge2的差运算结果（match_area_img_out）拷贝到binary_img;
                                                                      
                //add--修改路径问题（设为本地路径(与.exe同一路径)--start）
                path = str + "\\Auto-Sigle-Image-Result\\Match_Area_Difference_result";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                //add--修改路径问题（设为本地路径(与.exe同一路径)--end）

                //显示、保存图像；                
                ////CvInvoke.Imshow("img", temp); //显示图片
                CvInvoke.Imwrite(path + "\\" + dbf_File2 + "_match_area_img_out.bmp", match_area_img_out); //保存模板与匹配区域差运算结果图像；
                CvInvoke.WaitKey(0); //暂停按键等待           
                

                /*
                ********自动模式下，测试单张图像--4(2022-0120--end)*************************************************
                **************************************************************************************************
                ******模板与匹配区域差运算--4(2022-0120--end)*******************************************************
                */



                /*
                *******************自动模式下，测试单张图像---5-进行二值化运算--start*********************************
                ******************* (aadd,2022-0120,二值化）*******************************************************
                *************相当于(MatchingBinarization_Click));**************************************************
                */

                int pixel_num = Convert.ToInt32(textBox9.Text);//实现string类型到int类型的转换;

                //add(2021-1227,num>255,num=255;num<0,num=0);
                if (pixel_num > 255)
                {
                    pixel_num = 255;
                }
                if (pixel_num <= 0)
                {
                    pixel_num = 0;
                }
                //add(2021-1227,num>255,num=255;num<0,num=0);

                CvInvoke.Threshold(binary_img, binary, pixel_num, 255, ThresholdType.Binary);//num自适应调节;

                pictureBox9.Image = binary.ToImage<Gray, byte>().ToBitmap();
                corrosion_img = binary;
           
                //add--修改路径问题（设为本地路径(与.exe同一路径)--start）
                path = str + "\\Auto-Sigle-Image-Result\\Binary_result";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                //add--修改路径问题（设为本地路径(与.exe同一路径)--end）

                //显示、保存图像；             
                ////CvInvoke.Imshow("img", temp); //显示图片
                CvInvoke.Imwrite(path + "\\" + dbf_File2 + "_binary.bmp", binary); //保存匹配结果图像；
                CvInvoke.WaitKey(0); //暂停按键等待
                

                /*
                *******************自动模式下，测试单张图像---5-进行二值化运算--end**********************************
                ******************* (aadd,2022-0120,二值化）*******************************************************
                *************相当于(MatchingBinarization_Click));**************************************************
                */



                /*
                *******************自动模式下，测试单张图像---6-形态学腐蚀操作运算--start************************
                ******************* add,2022-0120,腐蚀）******************************************************
                *************相当于(erode_Click));************************************************************
                */

                Mat struct_element = CvInvoke.GetStructuringElement(ElementShape.Cross, new Size(3, 3), new Point(-1, -1));//指定参数获得结构元素；
                int erode_num = Convert.ToInt32(textBox7.Text);//实现string类型到int类型转换（默认腐蚀次数erode_num=1，点击一次腐蚀一次）；
                CvInvoke.Erode(corrosion_img, corrosion1, struct_element, new Point(-1, -1), erode_num, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));//指定参数进行形态学腐蚀操作（自适应手动调整输入腐蚀次数）；
                pictureBox9.Image = corrosion1.ToImage<Gray, byte>().ToBitmap();
                corrosion_img = corrosion1;//（将第一次腐蚀的结果作为第二次腐蚀的输入，如此循环，一直腐蚀，直至消失）;
                dilate_img = corrosion1;//（将腐蚀结果corrosion1复制给dilate_img，作为膨胀的输入）;   

               
                //add--修改路径问题（设为本地路径(与.exe同一路径)--start）
                path = str + "\\Auto-Sigle-Image-Result\\Corrosion_result";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                //add--修改路径问题（设为本地路径(与.exe同一路径)--end）

                CvInvoke.Imwrite(path + "\\" + dbf_File2 + "_corrosion1.bmp", corrosion1); //保存匹配结果图像；
                CvInvoke.WaitKey(0); //暂停按键等待

                /*
                *******************自动模式下，测试单张图像---6-形态学腐蚀操作运算--end************************
                ******************* add,2022-0120,腐蚀）******************************************************
                *************相当于(erode_Click));************************************************************
                */


                /*
                *******************自动模式下，测试单张图像---6-形态学膨胀操作运算--start************************
                ******************* add,2022-0120,膨胀）******************************************************
                *************相当于(erode_Click));************************************************************
                */

                Mat struct_element1 = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Cross, new Size(3, 3), new Point(-1, -1));//指定参数获得结构元素；
                int dilate_num = Convert.ToInt32(textBox6.Text);//实现string类型到int类型转换（默认腐蚀次数dilate_num=1，点击一次膨胀一次）；
                CvInvoke.Dilate(dilate_img, swell, struct_element1, new Point(-1, -1), dilate_num, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));//指定参数进行膨胀操作（默认腐蚀次数dilate_num=1，点击一次膨胀一次）；
                //CvInvoke.Dilate(dilate_img, swell, struct_element, new Point(-1, -1), 2, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));//指定参数进行膨胀操作（2：膨胀2次）；
                pictureBox9.Image = swell.ToImage<Gray, byte>().ToBitmap();
                dilate_img = swell; //（将第一次膨胀的结果作为第二次膨胀的输入，如此循环，一直膨胀）;
                
                //add--修改路径问题（设为本地路径(与.exe同一路径)--start）
                path = str + "\\Auto-Sigle-Image-Result\\Swell_result";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                //add--修改路径问题（设为本地路径(与.exe同一路径)--end）

                CvInvoke.Imwrite(path + "\\" + dbf_File2 + "_swell.bmp", swell); //保存匹配结果图像；
                CvInvoke.WaitKey(0); //暂停按键等待


                /*
                *******************自动模式下，测试单张图像---6-形态学膨胀操作运算--end**************************
                ******************* add,2022-0120,膨胀）******************************************************
                *************相当于(erode_Click));************************************************************
                */


                /*
                *******************自动模式下，测试单张图像---8-获取二值化轮廓结果并打印轮廓面积信息及将匹配信息写入Excel表中--start*****
                ******************* (add,2022-0120,获取二值化轮廓信息）*************************************************************
                *************相当于(BinarizedContour_Click));**********************************************************************
                */

                contour_img = swell;
                CvInvoke.Canny(contour_img, canny_img, 120, 180); //指定阈值（第一阈值：120，第二阈值：180）进行Canny算子处理(这里是否需要Canny算子？？)；                       
                VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();//创建VectorOfVectorOfPoint类型，用于存储查找到的轮廓；
                //add(1215，用于面积筛选)
                VectorOfVectorOfPoint use_contours = new VectorOfVectorOfPoint();//创建VectorOfVectorOfPoint类型，用于存储筛选后的轮廓；
                //add(1215，用于面积筛选)
                //(RetrType.External:提取最外层轮廓；RetrType.List:提取所有轮廓;Emgu.CV.CvEnum.RetrType.Ccomp检索所有轮廓，并将它们组织成两级层级结构：水平是组件的外部边界，二级约束边界的洞）；
                CvInvoke.FindContours(canny_img, contours, null, RetrType.Ccomp, ChainApproxMethod.ChainApproxSimple);//查找dst所有轮廓并保存（contour_img<-->canny_img）；                                                            

                //add(2021-1217，面积筛选)
                //遍历完所有面积，打印总面积；
                double sum_area = 0;
                int ksize = contours.Size;//获取连通区域的个数；
                for (int i = 0; i < ksize; i++)//遍历每个连通轮廓；
                {
                    VectorOfPoint contour = contours[i];//获取独立的连通轮廓；
                    double area = CvInvoke.ContourArea(contour);//计算连通轮廓的面积；

                    double area1 = Convert.ToDouble(textBox8.Text);//实现string类型到double类型的转换;
                    //add(2021-1227,当输入面积小于0时，area==0);
                    if (area1 <= 0)
                    {
                        area1 = 0;
                    }
                    //add(2021-1227,当输入面积小于0时，num1==0);

                    //add(2021-1227，当轮廓面积大于阈值面积时，才执行下列语句，实现面积相加，否则跳出条件语句）；
                    if (area > area1)//对每一轮廓区域面积进行面积筛选；                                
                    {

                        use_contours.Push(contour);//添加筛选后的连通轮廓；
                        sum_area += area;//遍历结果相加；

                    }

                }
                //打印轮廓总面积信息；
                displab4.Text = "轮廓总面积: " + sum_area.ToString() + ";" + "\n" + "\n" + "\n";//打印遍历完的总面积（0044轮廓面积,当阈值选择30时，显示出错,试试RetrType.Ccomp）

                //(add,2021-1228,在匹配信息Txt文本中添加轮廓面积信息)；                
                //add--修改路径问题（设为本地路径(与.exe同一路径)--start）
                path = str + "\\Auto-Sigle-Image-Result\\Text_result";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                //add--修改路径问题（设为本地路径(与.exe同一路径)--end）

                string txt1 = displab4.Text;
                string filename1 = path + "\\" + "匹配信息.txt";   //文件名，可以带路径

                FileStream fs1 = new FileStream(filename1, FileMode.Append);
                StreamWriter wr1 = null;
                wr1 = new StreamWriter(fs1);

                //System.IO.StreamWriter sw = new System.IO.StreamWriter(filename);
                wr1.Write(displab4.Text);
                wr1.Close();
                //(add,2021-1228,在匹配信息Txt文本中添加轮廓面积信息)；


                /*
                *******************自动模式下---9-用流创建或读取.xlsx文件（同时流关联了文件）--start************************
                ******************* (add,2022-0120,用流创建或读取.xlsx文件）**********************************************
                *************相当于(BinarizedContour_Click下));**********************************************************
                */           

                //add--修改路径问题（设为本地路径(与.exe同一路径)--start）
                path = str + "\\Auto-Sigle-Image-Result\\Excel_result";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                //add--修改路径问题（设为本地路径(与.exe同一路径)--end）

                filestream = new FileStream(path + "\\" + "匹配信息.xlsx", FileMode.OpenOrCreate);
                //(add-2021-1231)用流创建或读取.xlsx文件（同时流关联了文件）


                //创建表和sheet;
                if (indexOfExcel_j == 1)
                {
                    wk = new XSSFWorkbook();   //创建表对象wk
                    isheet = wk.CreateSheet("Sheet1");   //在wk中创建sheet1
                                                         //创建表头
                    IRow rowtitle = null;
                    rowtitle = isheet.CreateRow(0); //创建index=j的行

                    ICell cellTitle0 = null;
                    cellTitle0 = rowtitle.CreateCell(0);  //在index=j的行中创建index=0的单元格
                    cellTitle0.SetCellValue("时间"); //给创建的单元格赋值string

                    ICell cellTitle1 = null;
                    cellTitle1 = rowtitle.CreateCell(1);  //在index=j的行中创建index=0的单元格
                    cellTitle1.SetCellValue("图像名称"); //给创建的单元格赋值string

                    ICell cellTitle2 = null;
                    cellTitle2 = rowtitle.CreateCell(2);  //在index=j的行中创建index=0的单元格
                    cellTitle2.SetCellValue("匹配信息坐标X"); //给创建的单元格赋值string

                    ICell cellTitle3 = null;
                    cellTitle3 = rowtitle.CreateCell(3);  //在index=j的行中创建index=0的单元格
                    cellTitle3.SetCellValue("匹配信息坐标Y"); //给创建的单元格赋值string

                    ICell cellTitle4 = null;
                    cellTitle4 = rowtitle.CreateCell(4);  //在index=j的行中创建index=0的单元格
                    cellTitle4.SetCellValue("最大相似度"); //给创建的单元格赋值string


                    ICell cellTitle5 = null;
                    cellTitle5 = rowtitle.CreateCell(5);  //在index=j的行中创建index=0的单元格
                    cellTitle5.SetCellValue("最小相似度"); //给创建的单元格赋值string

                    ICell cellTitle6 = null;
                    cellTitle6 = rowtitle.CreateCell(6);  //在index=j的行中创建index=0的单元格
                    cellTitle6.SetCellValue("轮廓总面积"); //给创建的单元格赋值strin
                }


                //向单元格中写数据(2021-1231,循环写入行、列数据，先定义行数，再定义列数，这里是2行6列);
                //for (int indexOfExcel_j = 1; indexOfExcel_j < 3; indexOfExcel_j++)//(j:行)；
                //{

                //IRow row2 = null;
                //row2 = isheet.CreateRow(indexOfExcel_j++); //创建index=j的行

                ////datetime格式化；
                DateTime dt1 = DateTime.Now;

                rowWrite = isheet.CreateRow(indexOfExcel_j++); //创建index=j的行

                ICell cell0 = null;
                cell0 = rowWrite.CreateCell(0);  //在index=j的行中创建index=0的单元格
                cell0.SetCellValue(dt1.ToLocalTime().ToString()); //给创建的单元格赋值string

                ICell cell1 = null;
                cell1 = rowWrite.CreateCell(1);  //在index=j的行中创建index=0的单元格
                cell1.SetCellValue(dbf_File2); //给创建的单元格赋值string

                ICell cell2 = null;
                cell2 = rowWrite.CreateCell(2);  //在index=j的行中创建index=0的单元格
                cell2.SetCellValue("X:" + max_loc.X); //给创建的单元格赋值string

                ICell cell3 = null;
                cell3 = rowWrite.CreateCell(3);  //在index=j的行中创建index=0的单元格
                cell3.SetCellValue("Y:" + max_loc.Y); //给创建的单元格赋值string

                ICell cell4 = null;
                cell4 = rowWrite.CreateCell(4);  //在index=j的行中创建index=0的单元格
                cell4.SetCellValue(max.ToString("f2")); //给创建的单元格赋值string


                ICell cell5 = null;
                cell5 = rowWrite.CreateCell(5);  //在index=j的行中创建index=0的单元格
                cell5.SetCellValue(min.ToString("f2")); //给创建的单元格赋值string

                ICell cell6 = null;
                cell6 = rowWrite.CreateCell(6);  //在index=j的行中创建index=0的单元格
                cell6.SetCellValue(sum_area.ToString()); //给创建的单元格赋值string


                //通过流将表中写入的数据一次性写入文件中;
                wk.Write(filestream); //通过流filestream将表wk写入文件


                //(add,20211231)
                //filestream.Close(); //关闭文件流filestream
                //wk.Close(); //关闭Excel表对象wk

                Mat mask = contour_img.ToImage<Bgr, byte>().CopyBlank().Mat;//获取一张背景为黑色的图像，尺寸大小与contour_img一样(类型Bgr);
                CvInvoke.DrawContours(mask, use_contours, -1, new MCvScalar(0, 0, 255));//采用红色画笔在mask掩膜图像中画出所有轮廓；(MCvScalar是一个具有单元素到四元素间的一个数组)
                pictureBox8.Image = mask.ToImage<Bgr, byte>().ToBitmap();//显示轮廓区域图像；
             
                //add--修改路径问题（设为本地路径(与.exe同一路径)--start）
                path = str + "\\Auto-Sigle-Image-Result\\Contour_result";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                //add--修改路径问题（设为本地路径(与.exe同一路径)--end）

                //显示、保存图像；               
                //CvInvoke.Imshow("img", temp); //显示图片
                CvInvoke.Imwrite(path + "\\" + dbf_File2 + "_mask.bmp", mask); //保存匹配结果图像；
                CvInvoke.WaitKey(0); //暂停按键等待
                



                /*
               *******************自动模式下，测试单张图像---8-获取二值化轮廓结果并打印轮廓面积信息及将匹配信息写入Excel表中--end*******
               ******************* (add,2022-0120,获取二值化轮廓信息）*************************************************************
               *************相当于(BinarizedContour_Click));**********************************************************************
               */


                /*
                *******************自动模式下---9-用流创建或读取.xlsx文件（同时流关联了文件）--end**************************
                ******************* (add,2022-0120,用流创建或读取.xlsx文件）**********************************************
                *************相当于(BinarizedContour_Click下));**********************************************************
                */
            }
        }

        private volatile bool canStop = true;//add(设置bool值canStop，控制测试取消按钮，2022-0214);

        private void Cancel_Click(object sender, EventArgs e)//取消测试按钮（2022-0215）；
        {
            canStop = !canStop;
        }
        
        //手动模式下屏蔽区域设置；
        private void Shielded_Area_Click(object sender, EventArgs e)
        {
            Mask_Form1 form_1 = new Mask_Form1();
            form_1.Show();
        }


        //自动模式下屏蔽区域设置；
        private void Shielded_Area1_Click(object sender, EventArgs e)
        {
            //开启子窗口--0512；
            Mask_Form1 form_2 = new Mask_Form1();
            form_2.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            now = DateTime.Now;
            this.labelControl_time.Text = now.ToString("yyyy-MM-dd  HH:mm:ss");//实时显示日期、时间（add-2022-0512）；
        }

        private void folder_Click(object sender, EventArgs e)
        {
            /*
            ******************************自动模式下遍历文件夹--1--测试整个文件夹(加载待匹配图像)--start************************
            *****************************(add,2022-0121)--start*************************************************************
            *****************************(add,2022-0121)--start*************************************************************
            */

            //文件夹测试(2022-0121--start)；
            picturecount = 0;

            folderBrowserDialog1.SelectedPath = defaultfilepath; //记忆上次打开文件夹路径(2022-0125)；


            //(打开文件夹函数--2022-0106--start)
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                defaultfilepath = folderBrowserDialog1.SelectedPath; //记忆上次打开文件夹路径(2022-0125)；


                thread1 = new Thread(new ThreadStart(ImageProcessingAll));//创建线程(work-0214)
                thread1.Start();//(开启线程--work-0214)
            }
        }
           
        private void ImageProcessingAll()   //处理文件指定文件夹下所有图片
        {

            //nMax = 0;
            //nMin = 1;
            //double dThreValue = Convert.ToDouble(ThreshTb.Text);

            //Mat img;  //待测试图片
            DirectoryInfo folder;

            folder = new DirectoryInfo(defaultfilepath);

            double pic = 0;//(图像计数-0214)；


            //遍历文件夹；
            foreach (FileInfo nextfile in folder.GetFiles())
            {
                //Invoke((EventHandler)delegate { label2.Text = "图片名称：" + Path.GetFileName(nextfile.FullName); });
                //Invoke((EventHandler)delegate { label2.Refresh(); });

                // 使用匿名方法定义线程的执行体;
                #region  
                //add(2022-0214--start);
                //thread1 = new Thread(new ThreadStart(ImageProcessingAll));//创建线程(work-0214)
                //Thread thread = new Thread(delegate (object param)
                /*{*/// 等待“停止”信号，如果没有收到信号则执行 
                     //thread1 = new Thread(new ThreadStart(ImageProcessingAll));//创建线程(work-0214)
                     //thread1.Start();//(work-0214)
                     //if (canStop == true)//有问题，无法运行？
                     //{

                //    thread1 = new Thread(new ThreadStart(ImageProcessingAll));//创建线程(work-0214)
                //    thread1.Start();//(work-0214)

                //    //ImageProcessingAll();
                //}
                // 此时已经收到停止信号，可以在此释放资源并初始化变量;

                //});
                //add(2022-0214--end);
                #endregion

                if (canStop == false)
                {
                    canStop = !canStop;
                    break;
                    //thread1.Abort();//可暂停但不退出；

                }

                pic++;//(图像计数-0214)；

                Point max_loc1 = new Point(0, 0);
                Point classNumber = max_loc1;    //最大可能性位置

                //string typeName = typeList[classNumber.X];

                // DirectoryInfo对象.Name获得文件夹名;.FullName获得文件夹完整的路径名(2022-0125)
                convert_img = CvInvoke.Imread(nextfile.FullName, ImreadModes.AnyColor);

                //Image<Bgr, byte> matToimg = match_img.ToImage<Bgr, byte>();


                Image<Bgr, Byte> match_img = convert_img.ToImage<Bgr, Byte>();//Mat 2 Image;

                if (match_img == null)
                {
                    Invoke((EventHandler)delegate { label18.Text = "无法加载文件！"; });
                    Invoke((EventHandler)delegate { label18.Refresh(); });
                    return;
                }

               
                //(add--2022-0214--遍历文件夹内图像数量--start);
                string path = defaultfilepath;
                string[] files = Directory.GetFiles(path, "*.bmp");


                /*
                *************(设计进度条-2022-0214--start)**************
                **************************************************
                */

                //progressBar1.Value = 0;  //清空进度条
                double sumpic = (double)files.Length;
                //progressBar1.Value = (int)((pic / sumpic) * 100);//(debug-0218--线程间操作无效: 从不是创建控件“progressBar1”的线程访问它。)
                //label26.Text = "当前进度: " + Convert.ToInt32((int)((pic / sumpic) * 100)) + '%' + "\r\n";//(work-0216)

                //Invoke(2022-0221,使用委托delegate解决测试文件夹过程中卡顿现象)；
                Invoke((EventHandler)delegate { progressBar1.Value = (int)((pic / sumpic) * 100); });
                Invoke((EventHandler)delegate { label26.Text = "当前进度: " + Convert.ToInt32((int)((pic / sumpic) * 100)) + '%' + "\r\n"; });

                Thread.Sleep(50);
               

                /*
                *************(设计进度条-2022-0214--end)****************
                **************************************************
                */

                //进度条设计；
                #region
                //for (int i = 0; i < files.Length; i++)
                //{
                //    progressBar1.Value += (i / files.Length);
                //    textBox2.Text = "当前进度:" + i.ToString() + '%'+ "\r\n";
                //    Thread.Sleep(50);
                //}

                //int SumCount = 0;
                //foreach (string file in files)//遍历图像数量;
                //{
                //    SumCount++;
                //}
                //(add--2022-0214--遍历文件夹内图像数量--end);

                //int SumCount = 0;
                //for (int i = 0; i < defaultfilepath.Length; i++) 
                //{
                //    //string[] bb = nextfile[i].Split(new char[] { '.' });
                //    //if (bb[1].ToLower() == "bmp")
                //    SumCount++;

                //}
                #endregion


                //图像计数；
                picturecount++;
                Invoke((EventHandler)delegate { label13.Text = "图片总数：" + picturecount.ToString(); });
                Invoke((EventHandler)delegate { label13.Refresh(); });


                //检测内容
                Invoke((EventHandler)delegate { pictureBox11.Image = BitmapExtension.ToBitmap(match_img); });
                Invoke((EventHandler)delegate { pictureBox11.Refresh(); });


                


                /*
                ***************自动模式下遍历文件夹---2-加载待匹配图像并画出矩形框(--start)；*********************
                **********************************（add,2022-01121--start);************************************
                ************************************相当于(Matchimage_Click)**********************************
                */

                Mat result1 = new Mat(new Size(match_img.Width - temp.Width + 1, match_img.Height - temp.Height + 1), DepthType.Cv32F, 1);//创建mat存储输出匹配结果；
                CvInvoke.MatchTemplate(match_img, temp, result1, TemplateMatchingType.CcorrNormed);//采用系数匹配法，数值越大越接近准确图像；

                CvInvoke.MinMaxLoc(result1, ref min, ref max, ref min_loc, ref max_loc);//获取极值及其坐标；

                match_img1 = match_img.Copy(); //将原图match_img复制到match_img1中，对match_img1进行画矩形框，避免pictureBox3显示匹配区域出现边框；
                CvInvoke.Rectangle(match_img1, new Rectangle(max_loc, temp.Size), new MCvScalar(0, 255, 0), 3);//绘制矩形，匹配得到的结果；
                pictureBox11.Image = match_img1.ToBitmap();//显示找到模板图像的待搜索图像；
               
                //add--修改路径问题（设为本地路径(与.exe同一路径)--start）
                path = str + "\\Auto-Folder-Image-Result\\Match_result";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                //add--修改路径问题（设为本地路径(与.exe同一路径)--end）

                //CvInvoke.Imwrite("test/match_img_result/" + Path.GetFileName(nextfile.FullName), match_img1); //保存匹配结果图像；
                CvInvoke.Imwrite(path + "\\" + Path.GetFileName(nextfile.FullName), match_img1); //保存匹配结果图像；
                CvInvoke.WaitKey(0); //暂停按键等待


                /*
                ***************自动模式下遍历文件夹---3-获取匹配区域并打印匹配文本信息(--start)；*********************
                **********************************（add,2022-0125--start);***************************************
                *********************************相当于(MatchingArea_Click)***************************************
                */

                // 获取匹配区域；
                Mat result = new Mat(new Size(match_img.Width - temp.Width + 1, match_img.Height - temp.Height + 1), DepthType.Cv32F, 1);//创建mat存储输出匹配结果；
                CvInvoke.MatchTemplate(match_img, temp, result, TemplateMatchingType.CcorrNormed);//采用系数匹配法(CcorrNormed)，打印出匹配相似度信息,数值越大越接近准确图像；
                //CvInvoke.Normalize(result, result, 1, 0, Emgu.CV.CvEnum.NormType.MinMax); //对数据进行（min,max;0-255）归一化；
                //double max = 0, min = 0;//创建double极值；
                //Point max_loc = new Point(0, 0), min_loc = new Point(0, 0);//创建dPoint类型，表示极值的坐标；
                CvInvoke.MinMaxLoc(result, ref min, ref max, ref min_loc, ref max_loc);//获取极值及其坐标；
                match_area_img = match_img.Copy(new Rectangle(max_loc, temp.Size)); //将原图match_img复制到match_area_img中，显示匹配区域；
                pictureBox13.Image = match_area_img.ToBitmap();//显示匹配区域；

               
                //add--修改路径问题（设为本地路径(与.exe同一路径)--start）
                path = str + "\\Auto-Folder-Image-Result\\Match_Area_result";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                //add--修改路径问题（设为本地路径(与.exe同一路径)--end）

                CvInvoke.Imwrite(path + "\\" + Path.GetFileName(nextfile.FullName), match_area_img); //保存匹配结果图像；
                CvInvoke.WaitKey(0); //暂停按键等待

                //datetime格式化；
                DateTime dt1 = DateTime.Now;
                
                //add--修改路径问题（设为本地路径(与.exe同一路径)--start）
                path = str + "\\Auto-Folder-Image-Result\\Text_result";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                //add--修改路径问题（设为本地路径(与.exe同一路径)--end）

                //打印匹配信息（2022-0125,保存文本信息至指定文件夹）；(nextFile.Extension:图像名称；nextfile.FullName：完整路径)
                displab3.Text = "时间:" + dt1.ToLocalTime().ToString() + "\n" + "图像名称：" + "\n" +  nextfile.Name + "\n" +
                                "\n" + "\n" +  "匹配信息: X= " +
                                 max_loc.X + "," + " Y= " + max_loc.Y + ";" +
                                "\n" +
                                "最大相似度: " + max.ToString("f2") + ";" + "\n" +
                                "最小相似度: " + min.ToString("f2") + ";" + "\n" + "\n";

                //（2021-1228,保存文本信息至指定文件夹）；
                string txt2 = displab3.Text;
                string filename2 = path + "\\" + "匹配信息.txt";   //文件名，可以带路径

                FileStream fs2 = new FileStream(filename2, FileMode.Append);
                StreamWriter wr2 = null;
                wr2 = new StreamWriter(fs2);

                //System.IO.StreamWriter sw = new System.IO.StreamWriter(filename);
                wr2.Write(displab3.Text);
                wr2.Close();

                /*
                ***************自动模式下遍历文件夹---3-获取匹配区域并打印匹配文本信息(--end)；**********************
                **********************************（add,2022-0125--end);*****************************************
                *********************************相当于(MatchingArea_Click)***************************************
                */


                /*
                ***************************自动模式下遍历文件夹---4-进行差运算(2022-0126--start)*************************
                ***************************(add-2022-0126,进行差运算--start)********************************************
                ********************************相当于(MatchDifference_Click);******************************************
                */

                // 模板与匹配区域差运算方法(2022-0126);
                #region 
                /*模板与匹配区域差运算；        
                Sqdiff = 0(平方差匹配，最好匹配为0)；
                SqdiffNormed = 1(归一化平方差匹配，最好结果为0)；
                Ccorr = 2(相关匹配法，数值越大效果越好）；
                CcorrNormed = 3(归一化相关匹配法，数值越大效果越好）；
                Coeff = 4(系数匹配法，数值越大效果越好）；
                CoeffNormed = 5(归一化系数匹配法，数值越大效果越好）;*/
                #endregion

                Mat result2 = new Mat(new Size(match_img.Width - temp.Width + 1, match_img.Height - temp.Height + 1), DepthType.Cv32F, 1);//创建mat存储输出匹配结果；
                CvInvoke.MatchTemplate(match_img, temp, result2, TemplateMatchingType.CcorrNormed);//采用系数匹配法，数值越大越接近准确图像；
                CvInvoke.MinMaxLoc(result2, ref min, ref max, ref min_loc, ref max_loc);//获取极值及其坐标；

                match_area_img = match_img.Copy(new Rectangle(max_loc, temp.Size)); //将原图img复制到img2中，显示匹配区域；
                match_area_img_1 = match_area_img.AbsDiff(temp);//(1220-add,temp\img2绝对值差值)；
                match_area_img_out = match_area_img_1.Convert<Gray, Byte>();//（将match_area_img_1转化为灰度图）;(image定义的图像通过Convert转化为灰度图);
                pictureBox10.Image = match_area_img_out.ToBitmap();//显示模板图像与匹配区域的差运算结果；
                binary_img = match_area_img_out;//将模板图像temp-匹配区域imge2的差运算结果（match_area_img_out）拷贝到binary_img;
              

                //add--修改路径问题（设为本地路径(与.exe同一路径)--start）
                path = str + "\\Auto-Folder-Image-Result\\Match_Area_Difference_result";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                //add--修改路径问题（设为本地路径(与.exe同一路径)--end）

                CvInvoke.Imwrite(path + "\\" + Path.GetFileName(nextfile.FullName), match_area_img_out); //保存匹配结果图像；
                CvInvoke.WaitKey(0); //暂停按键等待

                /*
                ***************************自动模式下遍历文件夹---4-进行差运算(2022-0126--end)****************************
                ***************************(add-2022-0126,进行差运算--end)**********************************************
                ********************************相当于(MatchDifference_Click);******************************************
                */



                /*
                *******************自动模式下遍历文件夹---5-进行二值化运算(2022-0127--start)*********************************
                ******************* (add-2022-0127,二值化--start）*********************************************************
                *************相当于(MatchingBinarization_Click))***********************************************************
                */

                int pixel_num = Convert.ToInt32(textBox9.Text);//实现string类型到int类型的转换;
                //add(2021-1227,num>255,num=255;num<0,num=0);
                if (pixel_num > 255)
                {
                    pixel_num = 255;
                }
                if (pixel_num <= 0)
                {
                    pixel_num = 0;
                }
                //add(2021-1227,num>255,num=255;num<0,num=0);

                CvInvoke.Threshold(binary_img, binary, pixel_num, 255, ThresholdType.Binary);//num自适应调节;

                pictureBox9.Image = binary.ToImage<Gray, byte>().ToBitmap();
                corrosion_img = binary;
               

                //add--修改路径问题（设为本地路径(与.exe同一路径)--start）
                path = str + "\\Auto-Folder-Image-Result\\Binary_result";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                //add--修改路径问题（设为本地路径(与.exe同一路径)--end）

                CvInvoke.Imwrite(path + "\\" + Path.GetFileName(nextfile.FullName), binary); //保存匹配结果图像；
                CvInvoke.WaitKey(0); //暂停按键等待

                /*
                *******************自动模式下遍历文件夹---5-进行二值化运算(2022-0127--end)*********************************
                ******************* (add-2022-0127,二值化--end）*********************************************************
                *************相当于(MatchingBinarization_Click))*********************************************************
                */



                /*
                *******************自动模式下遍历文件夹---6-进行形态学腐蚀运算(2022-0127--start)******************************
                ******************* (add-2022-0127,腐蚀运算--start）*******************************************************
                *********************相当于(erode_Click--start)************************************************************
                */


                Mat struct_element = CvInvoke.GetStructuringElement(ElementShape.Cross, new Size(3, 3), new Point(-1, -1));//指定参数获得结构元素；

                int erode_num = Convert.ToInt32(textBox7.Text);//实现string类型到int类型转换（默认腐蚀次数erode_num=1，点击一次腐蚀一次）；

                CvInvoke.Erode(corrosion_img, corrosion1, struct_element, new Point(-1, -1), erode_num, BorderType.Default, new MCvScalar(0, 0, 0));//指定参数进行形态学腐蚀操作（自适应手动调整输入腐蚀次数）；
                //new Point(-1, -1),表示结构元素中心，2，表示腐蚀迭代次数；                                                                                                                                                    //CvInvoke.Erode(corrosion_img, corrosion1, struct_element, new Point(-1, -1), 2, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));//指定参数进行形态学腐蚀操作（2：腐蚀2次）；
                pictureBox9.Image = corrosion1.ToImage<Gray, byte>().ToBitmap();
                corrosion_img = corrosion1;//（将第一次腐蚀的结果作为第二次腐蚀的输入，如此循环，一直腐蚀，直至消失）;

                dilate_img = corrosion1;//（将腐蚀结果corrosion1复制给dilate_img，作为膨胀的输入）;

               
                //add--修改路径问题（设为本地路径(与.exe同一路径)--start）
                path = str + "\\Auto-Folder-Image-Result\\Corrosion_result";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                //add--修改路径问题（设为本地路径(与.exe同一路径)--end）

                CvInvoke.Imwrite(path + "\\" + Path.GetFileName(nextfile.FullName), corrosion1); //保存匹配结果图像；
                CvInvoke.WaitKey(0); //暂停按键等待

                /*
                *******************自动模式下遍历文件夹---6-进行形态学腐蚀运算(2022-0127--end)******************************
                ******************* (add-2022-0127,腐蚀运算--end）********************************************************
                *********************相当于(erode_Click--end)*************************************************************
                */



                /*
                *******************自动模式下遍历文件夹---7-进行形态学膨胀运算(2022-0127--start)*******************************
                ******************* (add-2022-0127,膨胀运算--start）********************************************************
                *********************相当于(dilate_Click--start)************************************************************
                */

                //形态学操作类型（add-2022-0127）
                #region
                //Erode = 0;(形态学腐蚀，消除边界点，使边界向内收缩，可消除没有意义的物体。使用CvInvoke.Erode)；
                //Dilate =1;(形态学膨胀，将与物体接触的所有背景点合并到该物体中，使边界向外扩张，可用来填补物体中的空洞。使用 CvInvoke.Dilate)；
                //open = 2;(形态学开操作，先腐蚀，后膨胀。即：OPEN(X)=D(E(X))，（使用CvInvoke.MorphologyEx（Emgu.CV.CvEnum.MorphOp.Open）作用：消除小物体，在纤细点处分离物体，位置和形状总是不变)；
                //Close = 3;(形态学闭操作，先膨胀，后腐蚀。即：CLOSE(X)=E(D(X)),（使用CvInvoke.MorphologyEx（Emgu.CV.CvEnum.MorphOp.Close））作用：可填平小孔，位置、形状不变。)
                //Gradient = 4;(形态学梯度操作，膨胀与腐蚀差。即：Grad(X)=Dilate(X)-Erode(X)。膨胀：扩大图像的边界，腐蚀：缩小图像的边界，膨胀-腐蚀：即为形态学梯度操作，可保存边缘轮廓)；
                //Tophat = 5;(形态学高帽，原始图像-开操作图像，即：Tophat(X)=X-Open（X）)；
                //Blackhat = 6;(形态学地帽，闭操作图像-原始图像，即：Blackhat(X)=Close(X)-X,结果是：输出图像大部分面积均为偏黑色，故称黑帽操作);
                //形态学操作类型（add-2021-1228）
                #endregion

                Mat struct_element1 = CvInvoke.GetStructuringElement(ElementShape.Cross, new Size(3, 3), new Point(-1, -1));//指定参数获得结构元素；

                //实现string类型到int类型转换（默认腐蚀次数dilate_num=1，点击一次膨胀一次）；
                int dilate_num = Convert.ToInt32(textBox6.Text);

                //指定参数进行膨胀操作（默认腐蚀次数dilate_num=1，点击一次膨胀一次）；
                CvInvoke.Dilate(dilate_img, swell, struct_element1, new Point(-1, -1), dilate_num, BorderType.Default, new MCvScalar(0, 0, 0));

                pictureBox9.Image = swell.ToImage<Gray, byte>().ToBitmap();

                dilate_img = swell; //（将第一次膨胀的结果作为第二次膨胀的输入，如此循环，一直膨胀）;

                
                //add--修改路径问题（设为本地路径(与.exe同一路径)--start）
                path = str + "\\Auto-Folder-Image-Result\\Swell_result";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                //add--修改路径问题（设为本地路径(与.exe同一路径)--end）

                CvInvoke.Imwrite(path + "\\" + Path.GetFileName(nextfile.FullName), swell); //保存匹配结果图像；
                CvInvoke.WaitKey(0); //暂停按键等待

                /*
                *******************自动模式下遍历文件夹---7-进行形态学膨胀运算(2022-0127--end)********************************
                ******************* (add-2022-0127,膨胀运算--end）**********************************************************
                *********************相当于(dilate_Click--end)**************************************************************
                */



                /*
                *******************自动模式下遍历文件夹---8-进行二值化轮廓筛选及面积运算(2022-0128--start)***********************
                ******************* (add-2022-01128,轮廓运算--start）*********************************************************
                *********************相当于(BinarizedContour_Click--start)****************************************************
                */

                contour_img = swell;

                CvInvoke.Canny(contour_img, canny_img, 120, 180); //指定阈值（第一阈值：120，第二阈值：180）进行Canny算子处理(这里是否需要Canny算子？？)；                       

                VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();//创建VectorOfVectorOfPoint类型，用于存储查找到的轮廓；
                //add(1215，用于面积筛选)
                VectorOfVectorOfPoint use_contours = new VectorOfVectorOfPoint();//创建VectorOfVectorOfPoint类型，用于存储筛选后的轮廓；

                //(RetrType.External:提取最外层轮廓；RetrType.List:提取所有轮廓;Emgu.CV.CvEnum.RetrType.Ccomp检索所有轮廓，并将它们组织成两级层级结构：水平是组件的外部边界，二级约束边界的洞）；                                                                 //add(1215，用于面积筛选)
                CvInvoke.FindContours(canny_img, contours, null, RetrType.Ccomp, ChainApproxMethod.ChainApproxSimple);//查找dst所有轮廓并保存（contour_img<-->canny_img）；

                //add(2022-0128，面积筛选)
                //遍历完所有面积，打印总面积；
                double sum_area = 0;
                int ksize = contours.Size;//获取连通区域的个数；
                for (int i = 0; i < ksize; i++)//遍历每个连通轮廓；
                {
                    VectorOfPoint contour = contours[i];//获取独立的连通轮廓；
                    double area = CvInvoke.ContourArea(contour);//计算连通轮廓的面积；

                    double area1 = Convert.ToDouble(textBox8.Text);//实现string类型到double类型的转换;
                                                                   //add(2021-1227,当输入面积小于0时，area==0);
                    if (area1 <= 0)
                    {
                        area1 = 0;
                    }
                    //add(2022-0128,当输入面积小于0时，num1==0);


                    //add(2021-1227，当轮廓面积大于阈值面积时，才执行下列语句，实现面积相加，否则跳出条件语句）；
                    if (area > area1)//对每一轮廓区域面积进行面积筛选；                                
                    {

                        use_contours.Push(contour);//添加筛选后的连通轮廓；
                        sum_area += area;//遍历结果相加；

                    }

                }
                //打印轮廓总面积信息；
                displab4.Text = "轮廓总面积: " + sum_area.ToString() + ";" + "\n" + "\n" + "\n";//打印遍历完的总面积（0044轮廓面积,当阈值选择30时，显示出错,试试RetrType.Ccomp）
                //(add,2021-1228,在匹配信息Txt文本中添加轮廓面积信息)；
                

                //add--修改路径问题（设为本地路径(与.exe同一路径)--start）
                path = str + "\\Auto-Folder-Image-Result\\Text_result";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                //add--修改路径问题（设为本地路径(与.exe同一路径)--end）

                string txt3 = displab4.Text;
                string filename3 = path + "\\" + "匹配信息.txt";    //文件名，可以带路径

                FileStream fs3 = new FileStream(filename3, FileMode.Append);
                StreamWriter wr3 = null;
                wr3 = new StreamWriter(fs3);

                //System.IO.StreamWriter sw = new System.IO.StreamWriter(filename);
                wr3.Write(displab4.Text);
                wr3.Close();
                //(add,2021-1228,在匹配信息Txt文本中添加轮廓面积信息)；


                /*
                *******************自动模式下遍历文件夹---9-用流创建或读取.xlsx文件（同时流关联了文件）***************
                ******************* (add-2022-0128,用流创建或读取.xlsx文件）***************************************
                *************(BinarizedContour_Click下));********************************************************
                *******************自动模式下--start**************************************************************
                */

                
                //add--修改路径问题（设为本地路径(与.exe同一路径)--start）
                path = str + "\\Auto-Folder-Image-Result\\Excel_result";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                //add--修改路径问题（设为本地路径(与.exe同一路径)--end）

                filestream = new FileStream(path + "\\" + "匹配信息.xlsx", FileMode.OpenOrCreate);
                DirectoryInfo folder1;
                folder1 = new DirectoryInfo(defaultfilepath);

                //创建表和sheet;
                if (indexOfExcel_j == 1)
                {
                    wk = new XSSFWorkbook();   //创建表对象wk
                    isheet = wk.CreateSheet("Sheet1");   //在wk中创建sheet1
                                                         //创建表头
                    IRow rowtitle = null;
                    rowtitle = isheet.CreateRow(0); //创建index=j的行

                    ICell cellTitle0 = null;
                    cellTitle0 = rowtitle.CreateCell(0); //在index=j的行中创建index=0的单元格
                    cellTitle0.SetCellValue("时间"); //给创建的单元格赋值string

                    ICell cellTitle1 = null;
                    cellTitle1 = rowtitle.CreateCell(1);  //在index=j的行中创建index=0的单元格
                    cellTitle1.SetCellValue("图像名称"); //给创建的单元格赋值string

                    ICell cellTitle2 = null;
                    cellTitle2 = rowtitle.CreateCell(2);  //在index=j的行中创建index=0的单元格
                    cellTitle2.SetCellValue("匹配信息坐标X"); //给创建的单元格赋值string

                    ICell cellTitle3 = null;
                    cellTitle3 = rowtitle.CreateCell(3);  //在index=j的行中创建index=0的单元格
                    cellTitle3.SetCellValue("匹配信息坐标Y"); //给创建的单元格赋值string

                    ICell cellTitle4 = null;
                    cellTitle4 = rowtitle.CreateCell(4);  //在index=j的行中创建index=0的单元格
                    cellTitle4.SetCellValue("最大相似度"); //给创建的单元格赋值string


                    ICell cellTitle5 = null;
                    cellTitle5 = rowtitle.CreateCell(5);  //在index=j的行中创建index=0的单元格
                    cellTitle5.SetCellValue("最小相似度"); //给创建的单元格赋值string

                    ICell cellTitle6 = null;
                    cellTitle6 = rowtitle.CreateCell(6);  //在index=j的行中创建index=0的单元格
                    cellTitle6.SetCellValue("轮廓总面积"); //给创建的单元格赋值strin
                }

                ////datetime格式化；
                DateTime dt2 = DateTime.Now;

                rowWrite = isheet.CreateRow(indexOfExcel_j++); //创建index=j的行

                ICell cell0 = null;
                cell0 = rowWrite.CreateCell(0);  //在index=j的行中创建index=0的单元格
                cell0.SetCellValue(dt2.ToLocalTime().ToString()); //给创建的单元格赋值string

                ICell cell1 = null;
                cell1 = rowWrite.CreateCell(1);  //在index=j的行中创建index=0的单元格
                cell1.SetCellValue(nextfile.FullName); //给创建的单元格赋值string

                ICell cell2 = null;
                cell2 = rowWrite.CreateCell(2);  //在index=j的行中创建index=0的单元格
                cell2.SetCellValue("X:" + max_loc.X); //给创建的单元格赋值string

                ICell cell3 = null;
                cell3 = rowWrite.CreateCell(3);  //在index=j的行中创建index=0的单元格
                cell3.SetCellValue("Y:" + max_loc.Y); //给创建的单元格赋值string

                ICell cell4 = null;
                cell4 = rowWrite.CreateCell(4);  //在index=j的行中创建index=0的单元格
                cell4.SetCellValue(max.ToString("f2")); //给创建的单元格赋值string


                ICell cell5 = null;
                cell5 = rowWrite.CreateCell(5);  //在index=j的行中创建index=0的单元格
                cell5.SetCellValue(min.ToString("f2")); //给创建的单元格赋值string

                ICell cell6 = null;
                cell6 = rowWrite.CreateCell(6);  //在index=j的行中创建index=0的单元格
                cell6.SetCellValue(sum_area.ToString()); //给创建的单元格赋值string

                //通过流将表中写入的数据一次性写入文件中;
                wk.Write(filestream); //通过流filestream将表wk写入文件；

                Mat mask = contour_img.ToImage<Bgr, byte>().CopyBlank().Mat;//获取一张背景为黑色的图像，尺寸大小与contour_img一样(类型Bgr);
                CvInvoke.DrawContours(mask, use_contours, -1, new MCvScalar(0, 0, 255));//采用红色画笔在mask掩膜图像中画出所有轮廓；(MCvScalar是一个具有单元素到四元素间的一个数组)
                pictureBox8.Image = mask.ToImage<Bgr, byte>().ToBitmap();//显示轮廓区域图像；

             
                //add--修改路径问题（设为本地路径(与.exe同一路径)--start）
                path = str + "\\Auto-Folder-Image-Result\\Contour_result";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                //add--修改路径问题（设为本地路径(与.exe同一路径)--end）

                //CvInvoke.Imwrite(@"test/mask_result/" + Path.GetFileName(nextfile.FullName), mask); //保存匹配结果图像；

                CvInvoke.Imwrite(path + "\\" + Path.GetFileName(nextfile.FullName), mask); //保存匹配结果图像；
                CvInvoke.WaitKey(0); //暂停按键等待

                /*
                *******************自动模式下遍历文件夹---8-进行二值化轮廓筛选及面积运算(2022-0128--end)***********************
                ******************* (add-2022-01128,轮廓运算--end）*********************************************************
                *********************相当于(BinarizedContour_Click--end)****************************************************
                */

                /*
                *******************自动模式下遍历文件夹---9-用流创建或读取.xlsx文件（同时流关联了文件）***************
                ******************* (add-2022-0128,用流创建或读取.xlsx文件）***************************************
                *************(BinarizedContour_Click下));********************************************************
                *******************自动模式下--end****************************************************************
                */
                         
            }
        }
    }
}
