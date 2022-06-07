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
    public partial class Mask_Form1 : Form
    {
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

        Image<Bgr, byte> temp;
        Image<Bgr, byte> match_img;
        

        //Image<Bgr, byte> match_img;
        //Image<Bgr, Byte> match_img1;
        //Mat convert_img = new Mat();

        //double max = 0, min = 0;//创建double极值；
        //Point max_loc = new Point(0, 0), min_loc = new Point(0, 0);//创建dPoint类型，表示极值的坐标；

        string path = string.Empty;
        string path1 = string.Empty;
        string dbf_File = string.Empty;
        String defaultfilepath;

        //获取和设置包含该应用程序的目录的名称(0228)
        //获取当前程序运行路径；
        string str =AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        
        public Mask_Form1()
        {
            InitializeComponent();
            defaultfilepath = "";  //默认文件夹路径；

        }
        //private void uiButton6_Click(object sender, EventArgs e)
        //{
        //    //this.Size = new Size(1024, 768);
        //    Form form1 = new Form();//实体化一个Form类;
        //    form1.Show();//弹出form1;
        //}

        private void Add_Area_Click(object sender, EventArgs e)
        {
            g_totalRectangle++;
            //comboBox1.Items.Add("区域" + g_totalRectangle.ToString());
            listBox1.Items.Add("区域" + g_totalRectangle.ToString());

            //int index = this.comboBox1.SelectedIndex;   //当前选中是第几个区域；
            //首先存储坐标;
            int point_x1 = Convert.ToInt32(textBox1.Text);//实现string类型到int类型的转换;
            int point_y1 = Convert.ToInt32(textBox4.Text);//实现string类型到int类型的转换;
            int point_x2 = Convert.ToInt32(textBox2.Text);//实现string类型到int类型的转换;
            int point_y2 = Convert.ToInt32(textBox3.Text);//实现string类型到int类型的转换;

            //add(2022-0224,point_x>temp.Width,num=temp.Width;point_x<0,point_x=0);
            //match_img = temp.Copy(); //将原图temp复制到match_img中，对match_img进行画矩形框，避免pictureBox显示匹配区域出现边框；

            if (point_x1 > temp.Width)
            {
                point_x1 = temp.Width;
            }
            if (point_x1 <= 0)
            {
                point_x1 = 0;
            }

            //add(2022-0224,point_y>temp.Height,point_y=temp.Height;point_y<0,point_y=0);
            if (point_y1 > temp.Height)
            {
                point_y1 = temp.Height;
            }
            if (point_y1 <= 0)
            {
                point_y1 = 0;
            }

            //add(2022-0224,point_x>temp.Width,num=temp.Width;point_x<0,point_x=0);
            if (point_x2 > temp.Width)
            {
                point_x2 = temp.Width;
            }
            if (point_x2 <= 0)
            {
                point_x2 = 0;
            }

            //add(2022-0224,point_y>temp.Height,point_y=temp.Height;point_y<0,point_y=0);
            if (point_y2 > temp.Height)
            {
                point_y2 = temp.Height;
            }
            if (point_y2 <= 0)
            {
                point_y2 = 0;
            }

            LocRectangle locRectangle;
            //locRectangle.locIndex = g_totalRectangle;   //当前添加的坐标是第几个区域的坐标
            locRectangle.x1 = point_x1;
            locRectangle.y1 = point_y1;
            locRectangle.x2 = point_x2;
            locRectangle.y2 = point_y2;

            rectangleLocations.Add(locRectangle);   //存储当前四个坐标值

            //comboBox2.Items.Add("区域" + g_totalRectangle.ToString());

           

            if (ifListBoxDel == false)
            {                           
                //取坐标、生成框(使用match_img图像无法画出多矩形框，仅可画出一个？？0302)
                CvInvoke.Rectangle(match_img, new Rectangle(new Point(locRectangle.x1, locRectangle.y1), new Size(locRectangle.x2 - locRectangle.x1, locRectangle.y2 - locRectangle.y1)), new MCvScalar(0, 255, 0), 1);//绘制矩形，匹配得到的结果(1：调整矩形粗细)；
                //CvInvoke.Rectangle(temp, new Rectangle(new Point(rectangleLocations[index].x1, rectangleLocations[index].y1), new Size(100, 100)), new MCvScalar(0, 255, 0), 3);//绘制矩形，匹配得到的结果；               

                //创建一矩形，左上角坐标为(80,80)，大小为50*50;                      
                //CvInvoke.Rectangle(match_img1, new Rectangle(new Point(80, 80), new Size(50, 50)), new MCvScalar(0, 255, 0), 3);//绘制矩形，匹配得到的结果；

                //CvInvoke.Rectangle(match_img1, new Rectangle(max_loc, temp.Size), new MCvScalar(0, 255, 0), 3);//绘制矩形，匹配得到的结果；
                pictureBox1.Image = match_img.ToBitmap();//显示找到模板图像的待搜索图像；


                //add--修改路径问题（设为本地路径(与.exe同一路径)--start）
                path = str + "\\Template-Result\\Match_result";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                //add--修改路径问题（设为本地路径(与.exe同一路径)--end）
                string dbf_File2 = Path.GetFileNameWithoutExtension(dbf_File); // for getting only MyFile

                //显示、保存图像；
                //CvInvoke.Imshow("img", temp); //显示图片
                //CvInvoke.Imwrite(path + "\\" + dbf_File2 + "_temp.bmp", temp); //保存匹配结果图像(含矩形框)；
                CvInvoke.Imwrite(path + "\\" + dbf_File2 + "_match_img.bmp", match_img); //保存匹配结果图像(含矩形框)；
                CvInvoke.WaitKey(0); //暂停按键等待
            }
            else
            {
                ifListBoxDel = !ifListBoxDel;
            }

            ////datetime格式化；
            DateTime dt = DateTime.Now;

            ////打印匹配信息（2021-1228,保存文本信息至指定文件夹）；
            //displab.Text = "时间:" + dt.ToLocalTime().ToString() + "\n" + "图像名称：" + "\n" + dbf_File2 + "\n" +
            //                "\n" + "匹配信息: X= " +
            //                 max_loc.X + "," + " Y= " + max_loc.Y + ";" +
            //                "\n" +
            //                "最大相似度: " + max.ToString("f2") + ";" + "\n" +
            //                "最小相似度: " + min.ToString("f2") + ";" + "\n" + "\n";

            displab.Text = "时间:" + dt.ToLocalTime().ToString() + "\n" +
                            "坐标信息: X1=" +
                            textBox1.Text + "," + " Y1=" + textBox4.Text + "; " +
                            "X2=" + textBox2.Text + ", " +
                            "Y2=" + textBox3.Text + ";" + "\n" + "\n";

        }

        private void Delete_Area_Click(object sender, EventArgs e)
        {
            ifListBoxDel = true;//判断是否执行listbox；
            g_totalRectangle--;
            //comboBox2.Items.RemoveAt(deleteindexRectangle);//删除选中区域；
            //comboBox1.Items.RemoveAt(deleteindexRectangle);//删除选中区域；

            //listBox1.Items.RemoveAt(listBox1.SelectedIndex);//删除选中区域；(索引index报错-1??-0228)
            //rectangleLocations.RemoveAt(listBox1.SelectedIndex);

           
            match_img = temp.Copy(); //将原图temp复制到match_img中，对match_img进行画矩形框，避免pictureBox显示匹配区域出现边框；  
            pictureBox1.Image = match_img.ToBitmap();//显示找到模板图像的待搜索图像；


            deleteindexRectangle = this.listBox1.SelectedIndex;   //当前选中是第几个区域
            listBox1.Items.RemoveAt(this.listBox1.SelectedIndex);//删除选中区域(0302)；            
            rectangleLocations.RemoveAt(deleteindexRectangle);

            for (int i = 0; i < rectangleLocations.Count; i++)
            {
                //取坐标、生成框(使用match_img图像无法画出多矩形框，仅可画出一个？？0302)
                CvInvoke.Rectangle(match_img, new Rectangle(new Point(rectangleLocations[i].x1, rectangleLocations[i].y1), new Size(rectangleLocations[i].x2 - rectangleLocations[i].x1, rectangleLocations[i].y2 - rectangleLocations[i].y1)), new MCvScalar(0, 255, 0), 1);//绘制矩形，匹配得到的结果(1：调整矩形粗细)；
                //CvInvoke.Rectangle(temp, new Rectangle(new Point(rectangleLocations[index].x1, rectangleLocations[index].y1), new Size(100, 100)), new MCvScalar(0, 255, 0), 3);//绘制矩形，匹配得到的结果；               

                //创建一矩形，左上角坐标为(80,80)，大小为50*50;                      
                //CvInvoke.Rectangle(match_img1, new Rectangle(new Point(80, 80), new Size(50, 50)), new MCvScalar(0, 255, 0), 3);//绘制矩形，匹配得到的结果；

                //CvInvoke.Rectangle(match_img1, new Rectangle(max_loc, temp.Size), new MCvScalar(0, 255, 0), 3);//绘制矩形，匹配得到的结果；
                


                //add--修改路径问题（设为本地路径(与.exe同一路径)--start）
                path = str + "\\Template-Result\\Match_result";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
               
            }

            //MessageBox.Show("区域参数" + deleteindexRectangle + "删除成功！");
            MessageBox.Show("区域删除成功！");

            //(0302--当前选中区域)；
            //MessageBox.Show("区域" + this.listBox1.SelectedIndex.ToString());

            pictureBox1.Image = match_img.ToBitmap();//显示找到模板图像的待搜索图像；
            //add--修改路径问题（设为本地路径(与.exe同一路径)--end）
            string dbf_File2 = Path.GetFileNameWithoutExtension(dbf_File); // for getting only MyFile

            //显示、保存图像；
            //CvInvoke.Imshow("img", temp); //显示图片
            //CvInvoke.Imwrite(path + "\\" + dbf_File2 + "_temp.bmp", temp); //保存匹配结果图像(含矩形框)；
            CvInvoke.Imwrite(path + "\\" + dbf_File2 + "_match_img.bmp", match_img); //保存匹配结果图像(含矩形框)；
            CvInvoke.WaitKey(0); //暂停按键等待

        }



        //private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    //取坐标、生成框
        //    CvInvoke.Rectangle(temp, new Rectangle(new Point(rectangleLocations[this.comboBox1.SelectedIndex].x1, rectangleLocations[this.comboBox1.SelectedIndex].y1), new Size(rectangleLocations[this.comboBox1.SelectedIndex].x2 - rectangleLocations[this.comboBox1.SelectedIndex].x1, rectangleLocations[this.comboBox1.SelectedIndex].y2 - rectangleLocations[this.comboBox1.SelectedIndex].y1)), new MCvScalar(0, 255, 0), 1);//绘制矩形，匹配得到的结果；
        //    //CvInvoke.Rectangle(temp, new Rectangle(new Point(rectangleLocations[index].x1, rectangleLocations[index].y1), new Size(100, 100)), new MCvScalar(0, 255, 0), 3);//绘制矩形，匹配得到的结果；

        //    //创建一矩形，左上角坐标为(80,80)，大小为50*50;                      
        //    //CvInvoke.Rectangle(match_img1, new Rectangle(new Point(80, 80), new Size(50, 50)), new MCvScalar(0, 255, 0), 3);//绘制矩形，匹配得到的结果；

        //    //CvInvoke.Rectangle(match_img1, new Rectangle(max_loc, temp.Size), new MCvScalar(0, 255, 0), 3);//绘制矩形，匹配得到的结果；
        //    pictureBox1.Image = temp.ToBitmap();//显示找到模板图像的待搜索图像；

        //    path = "\\Template-Result\\Match_result";
        //    if (!Directory.Exists(path))
        //    {
        //        Directory.CreateDirectory(path);
        //    }
        //    //add--修改路径问题（设为本地路径--end）
        //    string dbf_File2 = Path.GetFileNameWithoutExtension(dbf_File); // for getting only MyFile

        //    //显示、保存图像；
        //    //CvInvoke.Imshow("img", temp); //显示图片
        //    CvInvoke.Imwrite(path + "\\" + dbf_File2 + "_match_img1.bmp", temp); //保存匹配结果图像(含矩形框)；
        //    CvInvoke.WaitKey(0); //暂停按键等待

        //}


        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if(ifListBoxDel == false)
            //{
            //    //如何获取余下列表最小索引；               

            //    //取坐标、生成框(使用match_img图像无法画出多矩形框，仅可画出一个？？0302)
            //    CvInvoke.Rectangle(match_img, new Rectangle(new Point(rectangleLocations[this.listBox1.SelectedIndex].x1, rectangleLocations[this.listBox1.SelectedIndex].y1), new Size(rectangleLocations[this.listBox1.SelectedIndex].x2 - rectangleLocations[this.listBox1.SelectedIndex].x1, rectangleLocations[this.listBox1.SelectedIndex].y2 - rectangleLocations[this.listBox1.SelectedIndex].y1)), new MCvScalar(0, 255, 0), 1);//绘制矩形，匹配得到的结果(1：调整矩形粗细)；
            //    //CvInvoke.Rectangle(temp, new Rectangle(new Point(rectangleLocations[index].x1, rectangleLocations[index].y1), new Size(100, 100)), new MCvScalar(0, 255, 0), 3);//绘制矩形，匹配得到的结果；               

            //    //创建一矩形，左上角坐标为(80,80)，大小为50*50;                      
            //    //CvInvoke.Rectangle(match_img1, new Rectangle(new Point(80, 80), new Size(50, 50)), new MCvScalar(0, 255, 0), 3);//绘制矩形，匹配得到的结果；

            //    //CvInvoke.Rectangle(match_img1, new Rectangle(max_loc, temp.Size), new MCvScalar(0, 255, 0), 3);//绘制矩形，匹配得到的结果；
            //    pictureBox1.Image = match_img.ToBitmap();//显示找到模板图像的待搜索图像；


            //    //add--修改路径问题（设为本地路径(与.exe同一路径)--start）
            //    path = str + "\\Template-Result\\Match_result";

            //    if (!Directory.Exists(path))
            //    {
            //        Directory.CreateDirectory(path);
            //    }
            //    //add--修改路径问题（设为本地路径(与.exe同一路径)--end）
            //    string dbf_File2 = Path.GetFileNameWithoutExtension(dbf_File); // for getting only MyFile

            //    //显示、保存图像；
            //    //CvInvoke.Imshow("img", temp); //显示图片
            //    //CvInvoke.Imwrite(path + "\\" + dbf_File2 + "_temp.bmp", temp); //保存匹配结果图像(含矩形框)；
            //    CvInvoke.Imwrite(path + "\\" + dbf_File2 + "_match_img.bmp", match_img); //保存匹配结果图像(含矩形框)；
            //    CvInvoke.WaitKey(0); //暂停按键等待
            //}
            //else
            //{
            //    ifListBoxDel = !ifListBoxDel;
            //}
            if (ifListBoxDel == false)
            {
                match_img = temp.Copy(); //将原图temp复制到match_img中，对match_img进行画矩形框，避免pictureBox显示匹配区域出现边框；  
                pictureBox1.Image = match_img.ToBitmap();//显示找到模板图像的待搜索图像；

                for (int i = 0; i < rectangleLocations.Count; i++)
                {

                    if (rectangleLocations[this.listBox1.SelectedIndex].x1 == rectangleLocations[i].x1 &&
                       rectangleLocations[this.listBox1.SelectedIndex].y1 == rectangleLocations[i].y1 &&
                       rectangleLocations[this.listBox1.SelectedIndex].x2 == rectangleLocations[i].x2 &&
                       rectangleLocations[this.listBox1.SelectedIndex].y2 == rectangleLocations[i].y2
                        )
                    {
                        //取坐标、生成框(使用match_img图像画出多矩形框)
                        CvInvoke.Rectangle(match_img, new Rectangle(new Point(rectangleLocations[i].x1, rectangleLocations[i].y1), new Size(rectangleLocations[i].x2 - rectangleLocations[i].x1, rectangleLocations[i].y2 - rectangleLocations[i].y1)), new MCvScalar(0, 0, 255), 2);//绘制矩形，匹配得到的结果(1：调整矩形粗细)；
                       //CvInvoke.Rectangle(temp, new Rectangle(new Point(rectangleLocations[index].x1, rectangleLocations[index].y1), new Size(100, 100)), new MCvScalar(0, 255, 0), 3);//绘制矩形，匹配得到的结果；               
                    }

                    else
                    {
                        CvInvoke.Rectangle(match_img, new Rectangle(new Point(rectangleLocations[i].x1, rectangleLocations[i].y1), new Size(rectangleLocations[i].x2 - rectangleLocations[i].x1, rectangleLocations[i].y2 - rectangleLocations[i].y1)), new MCvScalar(0, 255, 0), 1);//绘制矩形，匹配得到的结果(1：调整矩形粗细)；
                    }
                    //创建一矩形，左上角坐标为(80,80)，大小为50*50;                      
                    //CvInvoke.Rectangle(match_img1, new Rectangle(new Point(80, 80), new Size(50, 50)), new MCvScalar(0, 255, 0), 3);//绘制矩形，匹配得到的结果；

                    //CvInvoke.Rectangle(match_img1, new Rectangle(max_loc, temp.Size), new MCvScalar(0, 255, 0), 3);//绘制矩形，匹配得到的结果；


                    //add--修改路径问题（设为本地路径(与.exe同一路径)--start）
                    path = str + "\\Template-Result\\Match_result";

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }


                }
                pictureBox1.Image = match_img.ToBitmap();//显示找到模板图像的待搜索图像；
                //add--修改路径问题（设为本地路径(与.exe同一路径)--end）
                string dbf_File2 = Path.GetFileNameWithoutExtension(dbf_File); // for getting only MyFile 
                //显示、保存图像；
                //CvInvoke.Imshow("img", temp); //显示图片
                //CvInvoke.Imwrite(path + "\\" + dbf_File2 + "_temp.bmp", temp); //保存匹配结果图像(含矩形框)；
                CvInvoke.Imwrite(path + "\\" + dbf_File2 + "_match_img.bmp", match_img); //保存匹配结果图像(含矩形框)；
                CvInvoke.WaitKey(0); //暂停按键等待


                //datetime格式化；
                DateTime dt = DateTime.Now;
                displab.Text = "时间:" + dt.ToLocalTime().ToString() + "\n" +
                  "坐标信息: X1=" +
                  rectangleLocations[this.listBox1.SelectedIndex].x1 + "," + " Y1=" + rectangleLocations[this.listBox1.SelectedIndex].y1 + "; " +
                  "X2=" + rectangleLocations[this.listBox1.SelectedIndex].x2 + ", " +
                  "Y2=" + rectangleLocations[this.listBox1.SelectedIndex].y2 + ";" + "\n" + "\n";

            }
            else
            {
                ifListBoxDel = !ifListBoxDel;
            }

            //    ////datetime格式化；
            //DateTime dt = DateTime.Now;
            //    displab.Text = "时间:" + dt.ToLocalTime().ToString() + "\n" +
            //                   "坐标信息: X1=" +
            //                   textBox1.Text + "," + " Y1=" + textBox4.Text + "; " +
            //                   "X2=" + textBox2.Text + ", " +
            //                   "Y2=" + textBox3.Text + ";" + "\n" + "\n";

            //    //（2021-1228,保存文本信息至指定文件夹）；
            //    string txt = displab.Text;

            //    string filename = path + "\\" + "坐标信息.txt";   //文件名，可以带路径

            //    FileStream fs = new FileStream(filename, FileMode.Append);
            //    StreamWriter wr = null;
            //    wr = new StreamWriter(fs);

            //    //System.IO.StreamWriter sw = new System.IO.StreamWriter(filename);

            //    wr.Write(displab.Text);
            //    wr.Close();

            //    //（2021-1228,保存文本信息至指定文件夹）；
            //    if (ifListBoxDel == false)
            //    {
            //        for (int i = 0; i < rectangleLocations.Count; i++)
            //        {

            //            if (rectangleLocations[this.listBox1.SelectedIndex].x1 == rectangleLocations[i].x1 &&
            //               rectangleLocations[this.listBox1.SelectedIndex].y1 == rectangleLocations[i].y1 &&
            //               rectangleLocations[this.listBox1.SelectedIndex].x2 == rectangleLocations[i].x2 &&
            //               rectangleLocations[this.listBox1.SelectedIndex].y2 == rectangleLocations[i].y2
            //                )
            //            {
            //displab.Text = "时间:" + dt.ToLocalTime().ToString() + "\n" +
            //       "坐标信息: X1=" +
            //       rectangleLocations[this.listBox1.SelectedIndex].x1 + "," + " Y1=" + rectangleLocations[this.listBox1.SelectedIndex].y1 + "; " +
            //       "X2=" + rectangleLocations[this.listBox1.SelectedIndex].x2 + ", " +
            //       "Y2=" + rectangleLocations[this.listBox1.SelectedIndex].y2 + ";" + "\n" + "\n";
            //            }

            //        }
            //    }

            //    else
            //    {
            //        ifListBoxDel = !ifListBoxDel;
            //    }


        }


        //private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    deleteindexRectangle = this.comboBox2.SelectedIndex;   //当前选中是第几个区域
        //    MessageBox.Show(this.comboBox2.SelectedIndex.ToString());
        //}

        private void LoadTemplate1_Click(object sender, EventArgs e)
        {
            // 加载模板图像；
            OpenFileDialog OpenFileDialog = new OpenFileDialog();
            OpenFileDialog.Filter = "JPEG;BMP;PNG;JPG;GIF|*.JPEG;*.BMP;*.PNG;*.JPG;*.GIF|ALL|*.*";//（模板和加载图像尺寸不一致时，会报错）;


            if (OpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                //(add-2022-0113,获取图像路径--start);
                dbf_File = OpenFileDialog.FileName;
                //string dbf_File1 = System.IO.Path.GetFileName(dbf_File);

                string dbf_File2 = Path.GetFileNameWithoutExtension(dbf_File); //得到文件名
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

                
                //add--修改路径问题（设为本地路径(与.exe同一路径)--start）
                path = str + "\\Template-Result\\Temp_result";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                //add--修改路径问题（设为本地路径(与.exe同一路径)--end）

                //显示、保存图像；               
                CvInvoke.Imwrite(path + "\\" + dbf_File2 + "_temp.bmp", temp); //保存至本地文件夹Result;
                CvInvoke.WaitKey(0); //暂停按键等待        
                match_img = temp.Copy(); //将原图temp复制到match_img中，对match_img进行画矩形框，避免pictureBox显示匹配区域出现边框；    
            }
        }

        private void Save_Area_Click(object sender, EventArgs e)
        {

            //add--修改路径问题（设为本地路径(与.exe同一路径)--start）
            path = str + "\\Template-Result\\Text_result";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            //add--修改路径问题（设为本地路径(与.exe同一路径)--end）

            ////datetime格式化；
            //DateTime dt = DateTime.Now;

            ////打印匹配信息（2021-1228,保存文本信息至指定文件夹）；
            //displab.Text = "时间:" + dt.ToLocalTime().ToString() + "\n" + "图像名称：" + "\n" + dbf_File2 + "\n" +
            //                "\n" + "匹配信息: X= " +
            //                 max_loc.X + "," + " Y= " + max_loc.Y + ";" +
            //                "\n" +
            //                "最大相似度: " + max.ToString("f2") + ";" + "\n" +
            //                "最小相似度: " + min.ToString("f2") + ";" + "\n" + "\n";

            //displab.Text = "时间:" + dt.ToLocalTime().ToString() + "\n" +
            //                "坐标信息: X1=" +
            //                textBox1.Text + "," + " Y1=" + textBox4.Text + "; " +
            //                "X2=" + textBox2.Text + ", " +
            //                "Y2=" + textBox3.Text + ";" + "\n" + "\n";

            //datetime格式化；
            DateTime dt = DateTime.Now;
            displab.Text = "时间:" + dt.ToLocalTime().ToString() + "\n" +
              "坐标信息: X1=" +
              rectangleLocations[this.listBox1.SelectedIndex].x1 + "," + " Y1=" + rectangleLocations[this.listBox1.SelectedIndex].y1 + "; " +
              "X2=" + rectangleLocations[this.listBox1.SelectedIndex].x2 + ", " +
              "Y2=" + rectangleLocations[this.listBox1.SelectedIndex].y2 + ";" + "\n" + "\n";

            //（2021-1228,保存文本信息至指定文件夹）；
            string txt = displab.Text;

            string filename = path + "\\" + "坐标信息.txt";   //文件名，可以带路径

            FileStream fs = new FileStream(filename, FileMode.Append);
            StreamWriter wr = null;
            wr = new StreamWriter(fs);

            //System.IO.StreamWriter sw = new System.IO.StreamWriter(filename);

            wr.Write(displab.Text);
            wr.Close();

            deleteindexRectangle = this.listBox1.SelectedIndex;   //当前选中是第几个区域
            //MessageBox.Show("区域参数" + deleteindexRectangle + "保存成功！");
            MessageBox.Show("区域参数保存成功！");
        }

        private void uiButton5_Click(object sender, EventArgs e)
        {

            //folderBrowserDialog1.SelectedPath = defaultfilepath; //记忆上次打开文件夹路径(2022-0125)；
            ////(打开文件夹函数--2022-0106--start)
            //if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            //{
            //    defaultfilepath = folderBrowserDialog1.SelectedPath; //记忆上次打开文件夹路径(2022-0125)；


            //    //    //thread1 = new Thread(new ThreadStart(ImageProcessingAll));//创建线程(work-0214)
            //    //    //thread1.Start();//(开启线程--work-0214)
            //}

            OpenFileDialog filename = new OpenFileDialog(); //定义打开文件
            filename.InitialDirectory = Application.StartupPath; //初始路径,这里设置的是程序的起始位置，可自由设置
            filename.Filter = "All files(*.*)|*.*|txt files(*.txt)|*.txt";//设置打开类型,设置个*.*和*.txt就行了
            filename.FilterIndex = 2;                  //文件类型的显示顺序（上一行.txt设为第二位）
            filename.RestoreDirectory = true; //对话框记忆之前打开的目录
            if (filename.ShowDialog() == DialogResult.OK)
            {
                listBox1.Text = filename.FileName.ToString();//获得完整路径在listBox1中显示
                StreamReader sr = new StreamReader(filename.FileName);//将选中的文件在listBox1中显示
                listBox1.Text = sr.ReadToEnd();
                sr.Close();
            }
        }





        //private void Matchimage2_Click(object sender, EventArgs e)
        //{
        //    // 加载待匹配图像并画出矩形框；
        //    OpenFileDialog OpenFileDialog = new OpenFileDialog();
        //    OpenFileDialog.Filter = "JPEG;BMP;PNG;JPG;GIF|*.JPEG;*.BMP;*.PNG;*.JPG;*.GIF|ALL|*.*";//（模板和加载图像尺寸不一致时，会报错）;

        //    //OpenFileDialog.Multiselect = true;//(2021-1231)该值确定是否可以选择多个文件;

        //    if (OpenFileDialog.ShowDialog() == DialogResult.OK)
        //    {

        //        //string[] files = OpenFileDialog.FileNames;//(2021-1231)该值确定是否可以选择多个文件;

        //        //(add-2022-0113,获取图像路径--start);
        //        dbf_File = OpenFileDialog.FileName;
        //        //string dbf_File1 = System.IO.Path.GetFileName(dbf_File);

        //        string dbf_File2 = Path.GetFileNameWithoutExtension(dbf_File); // for getting only MyFile
        //       //(add-2022-0113,获取图像路径--end);

        //        try
        //        {
        //            match_img = new Image<Bgr, Byte>(OpenFileDialog.FileName);
        //            Mat result = new Mat(new Size(match_img.Width - temp.Width + 1, match_img.Height - temp.Height + 1), DepthType.Cv32F, 1);//创建mat存储输出匹配结果；
        //            CvInvoke.MatchTemplate(match_img, temp, result, TemplateMatchingType.CcorrNormed);//采用系数匹配法，数值越大越接近准确图像；


        //            // 模板与匹配区域差运算匹配方法;
        //            #region 
        //            /*模板与匹配区域差运算；        
        //            Sqdiff = 0(平方差匹配，最好匹配为0)；
        //            SqdiffNormed = 1(归一化平方差匹配，最好结果为0)；
        //            Ccorr = 2(相关匹配法，数值越大效果越好）；
        //            CcorrNormed = 3(归一化相关匹配法，数值越大效果越好）；
        //            Coeff = 4(系数匹配法，数值越大效果越好）；
        //            CoeffNormed = 5(归一化系数匹配法，数值越大效果越好）;*/
        //            #endregion

        //            //CvInvoke.Normalize(result, result, 1, 0, Emgu.CV.CvEnum.NormType.MinMax); //对数据进行（min,max;0-255）归一化；
        //            //double max = 0, min = 0;//创建double极值；
        //            //Point max_loc = new Point(0, 0), min_loc = new Point(0, 0);//创建dPoint类型，表示极值的坐标；

        //            CvInvoke.MinMaxLoc(result, ref min, ref max, ref min_loc, ref max_loc);//获取极值及其坐标；

        //            match_img1 = match_img.Copy(); //将原图match_img复制到match_img1中，对match_img1进行画矩形框，避免pictureBox3显示匹配区域出现边框；

        //            //创建一矩形，左上角坐标为(80,80)，大小为40*40
        //            //Rectangle rect = new Rectangle(new Point(80, 80), new Size(40, 40));

        //            //利用textBox1自适应调整像素阈值；
        //            int point_x1 = Convert.ToInt32(textBox1.Text);//实现string类型到int类型的转换;
        //            int point_y1 = Convert.ToInt32(textBox2.Text);//实现string类型到int类型的转换;
        //            int point_x2 = Convert.ToInt32(textBox4.Text);//实现string类型到int类型的转换;
        //            int point_y2 = Convert.ToInt32(textBox3.Text);//实现string类型到int类型的转换;

        //            //add(2022-0224,point_x>temp.Width,num=temp.Width;point_x<0,point_x=0);
        //            if (point_x1 > temp.Width)
        //            {
        //                point_x1 = temp.Width;
        //            }
        //            if (point_x1 <= 0)
        //            {
        //                point_x1 = 0;
        //            }

        //            //add(2022-0224,point_y>temp.Height,point_y=temp.Height;point_y<0,point_y=0);
        //            if (point_y1 > temp.Height)
        //            {
        //                point_y1 = temp.Height;
        //            }
        //            if (point_y1 <= 0)
        //            {
        //                point_y1 = 0;
        //            }

        //            //add(2022-0224,point_x>temp.Width,num=temp.Width;point_x<0,point_x=0);
        //            if (point_x2 > temp.Width)
        //            {
        //                point_x2 = temp.Width;
        //            }
        //            if (point_x2 <= 0)
        //            {
        //                point_x2 = 0;
        //            }

        //            //add(2022-0224,point_y>temp.Height,point_y=temp.Height;point_y<0,point_y=0);
        //            if (point_y2 > temp.Height)
        //            {
        //                point_y2 = temp.Height;
        //            }
        //            if (point_y2 <= 0)
        //            {
        //                point_y2 = 0;
        //            }

        //            CvInvoke.Rectangle(match_img1, new Rectangle(new Point(point_x1, point_y1), new Size(point_x2, point_y2)), new MCvScalar(0, 255, 0), 3);//绘制矩形，匹配得到的结果；
        //             //创建一矩形，左上角坐标为(80,80)，大小为50*50;                      
        //            //CvInvoke.Rectangle(match_img1, new Rectangle(new Point(80, 80), new Size(50, 50)), new MCvScalar(0, 255, 0), 3);//绘制矩形，匹配得到的结果；

        //            //CvInvoke.Rectangle(match_img1, new Rectangle(max_loc, temp.Size), new MCvScalar(0, 255, 0), 3);//绘制矩形，匹配得到的结果；
        //            pictureBox2.Image = match_img1.ToBitmap();//显示找到模板图像的待搜索图像；

        //        }
        //        catch (System.Exception ex)
        //        {
        //            MessageBox.Show("图像格式错误！");
        //        }

        //        //add--修改路径问题（设为本地路径--start）
        //        path = "\\Template-Result\\Match_result";
        //        if (!Directory.Exists(path))
        //        {
        //            Directory.CreateDirectory(path);
        //        }
        //        //add--修改路径问题（设为本地路径--end）

        //        //显示、保存图像；
        //        #region
        //        //CvInvoke.Imshow("img", temp); //显示图片
        //        CvInvoke.Imwrite(path + "\\" + dbf_File2 + "_match_img1.bmp", match_img1); //保存匹配结果图像(含矩形框)；
        //        CvInvoke.WaitKey(0); //暂停按键等待
        //        #endregion




    }
        }
















