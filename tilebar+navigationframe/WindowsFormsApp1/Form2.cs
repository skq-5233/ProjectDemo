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
    public partial class Form2 : DevExpress.XtraEditors.XtraForm
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void tileBarItem1_ItemClick(object sender, TileItemEventArgs e)
        {
            this.navigationFrame1.SelectedPage = navigationPage1;

        }

        private void tileBarItem2_ItemClick(object sender, TileItemEventArgs e)
        {
            this.navigationFrame1.SelectedPage = navigationPage2;

        }

        private void tileBarItem3_ItemClick(object sender, TileItemEventArgs e)
        {
            this.navigationFrame1.SelectedPage = navigationPage3;

        }

        private void tileBarItem4_ItemClick(object sender, TileItemEventArgs e)
        {
            this.navigationFrame1.SelectedPage = navigationPage4;
        }
    }
}