using DevExpress.XtraBars.Navigation;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void tileBarItem6_ItemClick(object sender, TileItemEventArgs e)
        {
            this.navigationFrame2.SelectedPage = navigationPage4;

        }

        private void tileBarItem7_ItemClick(object sender, TileItemEventArgs e)
        {
            this.navigationFrame2.SelectedPage = navigationPage5;

        }

        private void tileBarItem8_ItemClick(object sender, TileItemEventArgs e)
        {
            this.navigationFrame2.SelectedPage = navigationPage8;

        }

        private void tileBarItem3_ItemClick(object sender, TileItemEventArgs e)
        {
            this.navigationFrame1.SelectedPage = navigationPage3;

        }

        private void tileBarItem1_ItemClick(object sender, TileItemEventArgs e)
        {
            this.navigationFrame1.SelectedPage = navigationPage1;

        }

        private void tileBarItem2_ItemClick(object sender, TileItemEventArgs e)
        {
            this.navigationFrame1.SelectedPage = navigationPage2;

        }

        private void tileBarItem4_ItemClick(object sender, TileItemEventArgs e)
        {
            this.navigationFrame1.SelectedPage = navigationPage14;

        }

        private void tileBarItem5_ItemClick(object sender, TileItemEventArgs e)
        {
            this.navigationFrame1.SelectedPage = navigationPage15;

        }

        private void tileBarItem9_ItemClick(object sender, TileItemEventArgs e)
        {
            this.navigationFrame2.SelectedPage = navigationPage9;

        }

        private void tileBarItem10_ItemClick(object sender, TileItemEventArgs e)
        {
            this.navigationFrame2.SelectedPage = navigationPage10;

        }

        private void tileBarItem11_ItemClick(object sender, TileItemEventArgs e)
        {
            this.navigationFrame3.SelectedPage = navigationPage6;

        }

        private void tileBarItem12_ItemClick(object sender, TileItemEventArgs e)
        {
            this.navigationFrame3.SelectedPage = navigationPage7;

        }

        private void tileBarItem13_ItemClick(object sender, TileItemEventArgs e)
        {
            this.navigationFrame3.SelectedPage = navigationPage11;

        }

        private void tileBarItem14_ItemClick(object sender, TileItemEventArgs e)
        {
            this.navigationFrame3.SelectedPage = navigationPage12;

        }

        private void tileBarItem15_ItemClick(object sender, TileItemEventArgs e)
        {
            this.navigationFrame3.SelectedPage = navigationPage13;

        }
    }
}