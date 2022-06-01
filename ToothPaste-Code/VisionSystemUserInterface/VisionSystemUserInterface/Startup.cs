/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：Load.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：STARTUP页面

原作者：蒋涛
完成日期：2014/10/28
特别说明：无

修改者：无
完成日期：无
修改记录：无

****************************************************************/

using System;
using System.Drawing;
using System.Windows.Forms;

namespace VisionSystemUserInterface
{
    public partial class Startup : Form
    {
        //LOAD页面

        //构造函数

        //----------------------------------------------------------------------
        // 功能说明：构造函数，初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Startup()
        {
            InitializeComponent();

            //

            if (0 == Global.Data_Value)
            {
                loadControl.Visible = false;

                BackColor = Color.White;

                label1.Text = VisionSystemControlLibrary.WorkControl._GetProductFullName(Global.VisionSystem.MachineType[Global.VisionSystem.SelectedMachineType], Global.VisionSystem.ProductName, Global.VisionSystem.ProductModelNumber);//设备名称

                switch (VisionSystemClassLibrary.Class.System.Language)//语言
                {
                    case VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese://中文

                        BackgroundImage = Global.Load_Background_Chinese;//

                        label2.Text = "版本：" + Global.HMIApplicationVersion;//程序版本

                        //label1.Location = new System.Drawing.Point(20, 249);
                        //label2.Location = new System.Drawing.Point(57, 593);

                        //customProgressBar.Location = new System.Drawing.Point(354, 495);

                        break;
                    case VisionSystemClassLibrary.Enum.InterfaceLanguage.English://英文

                        BackgroundImage = Global.Load_Background_English;//

                        label2.Text = "Version: " + Global.HMIApplicationVersion;//程序版本

                        //label1.Location = new System.Drawing.Point(20, 259);
                        //label2.Location = new System.Drawing.Point(42, 593);

                        //customProgressBar.Location = new System.Drawing.Point(344, 480);

                        break;
                    default://默认中文

                        BackgroundImage = Global.Load_Background_Chinese;//

                        label2.Text = "版本：" + Global.HMIApplicationVersion;//程序版本

                        //label1.Location = new System.Drawing.Point(20, 249);
                        //label2.Location = new System.Drawing.Point(57, 593);

                        //customProgressBar.Location = new System.Drawing.Point(354, 495);

                        break;
                }

                //customProgressBar.BackgroundImageBlue = true;//进度条背景设置为红色
                customProgressBar.Minimum = 0;//进度条控件范围的最小值
                customProgressBar.Maximum = 100;//进度条控件范围的最大值
                customProgressBar.StepNumber = 10;//进度条控件步进数量   
            }
            else
            {
                customProgressBar.Visible = false;

                label1.Visible = false;
                label2.Visible = false;
            }

            Update();

            //

            loadControl.BitmapTrademark = VisionSystemClassLibrary.Class.System.Trademark_1[Global.Data_Value];//商标
            loadControl.DeviceName = VisionSystemControlLibrary.WorkControl._GetProductFullName(Global.VisionSystem.MachineType[Global.VisionSystem.SelectedMachineType], Global.VisionSystem.ProductName, Global.VisionSystem.ProductModelNumber);//设备名称
            loadControl.AppVersion = Global.HMIApplicationVersion;//程序版本
            loadControl.ProgressBarMinimum = 0;//进度条控件范围的最小值
            loadControl.ProgressBarMaximum = 100;//进度条控件范围的最大值
            loadControl.ProgressBarStepNumber = 10;//进度条控件步进数量
            loadControl.Language = VisionSystemClassLibrary.Class.System.Language;//设置语言

            //

            labelText.Text = "";//清除显示
            
            switch (VisionSystemClassLibrary.Class.System.Language)//语言
            {
                case VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese://中文

                    labelText.Image = VisionSystemUserInterface.Properties.Resources.Startup.Clone(new Rectangle(new Point(0, 0), new Size((Int32)(VisionSystemUserInterface.Properties.Resources.Startup.Width / 2), VisionSystemUserInterface.Properties.Resources.Startup.Height)), VisionSystemUserInterface.Properties.Resources.Startup.PixelFormat);//获取图像

                    break;
                case VisionSystemClassLibrary.Enum.InterfaceLanguage.English://英文

                    labelText.Image = VisionSystemUserInterface.Properties.Resources.Startup.Clone(new Rectangle(new Point((Int32)(VisionSystemUserInterface.Properties.Resources.Startup.Width / 2), 0), new Size((Int32)(VisionSystemUserInterface.Properties.Resources.Startup.Width / 2), VisionSystemUserInterface.Properties.Resources.Startup.Height)), VisionSystemUserInterface.Properties.Resources.Startup.PixelFormat);//获取图像

                    break;
                default://默认中文

                    labelText.Image = VisionSystemUserInterface.Properties.Resources.Startup.Clone(new Rectangle(new Point(0, 0), new Size((Int32)(VisionSystemUserInterface.Properties.Resources.Startup.Width / 2), VisionSystemUserInterface.Properties.Resources.Startup.Height)), VisionSystemUserInterface.Properties.Resources.Startup.PixelFormat);//获取图像

                    break;
            }
        }
    }
}
