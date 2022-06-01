using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace VisionSystemImageProcessing
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Boolean createNew;

            //using (System.Threading.Mutex m = new System.Threading.Mutex(true, Application.ProductName, out createNew))
            //{
            //    if (createNew)
            //    {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new Work());
            //    }
            //    else
            //    {
            //        MessageBox.Show("应用程序秩序打开一次!!!");
            //    }
            //}
        }
    }
}
