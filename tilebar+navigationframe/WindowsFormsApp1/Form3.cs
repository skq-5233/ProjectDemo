using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace WindowsFormsApp1
{
    public partial class Form3 : DevExpress.XtraEditors.XtraForm
    {

        DateTime now = new DateTime();
        public Form3()
        {
            InitializeComponent();
        }

        private void Time_Tick(object sender, EventArgs e)
        {

            now = DateTime.Now;
            this.labelControl_datetime.Text = now.ToString("yyyy-MM-dd  HH:mm:ss");
        }

        private void tileBaritem1_Clik(object sender, TileItemEventArgs e)
        {
            this.navigationFrame1.SelectedPage = navigationPage1;
        }

        private void tileBaritem2_Clik(object sender, TileItemEventArgs e)
        {
            this.navigationFrame1.SelectedPage = navigationPage2;
        }

        private void tileBaritem3_Clik(object sender, TileItemEventArgs e)
        {
            this.navigationFrame1.SelectedPage = navigationPage3;
        }

        private void tileBaritem4_Clik(object sender, TileItemEventArgs e)
        {
            this.navigationFrame1.SelectedPage = navigationPage4;

        }
    }
}