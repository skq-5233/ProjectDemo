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

namespace CheckWeighterInterface.ParameterConfig
{
    public partial class AlgorithmConfig : DevExpress.XtraEditors.XtraUserControl
    {
        public AlgorithmConfig()
        {
            InitializeComponent();
        }

        private void labelControl1_Click(object sender, EventArgs e)
        {
            int[] a = { 0, 2, 4, 5, 9 };
            FilteringAlgorithm ff = new FilteringAlgorithm();
            int cur = ff.amplitudeLimitingFiltering(a[0], a[1], 2);
            int b = ff.medianFiltering(5, a);
            int c = ff.digitalAverageFiltering(5, a);

            FilteringAlgorithm ff1 = new FilteringAlgorithm();
            for (int i = 0; i < 3; i++)
            {
                int d = ff1.recursionAverageFiltering(3, a[i]);
            }

            int f = ff1.recursionAverageFiltering(3, a[3]);
            f = ff1.recursionAverageFiltering(3, a[4]);


        }


    }
}
