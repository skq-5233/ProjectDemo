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

namespace CheckWeighterInterface.ParameterConfig
{
    public partial class ParameterConfig : DevExpress.XtraEditors.XtraUserControl
    {
        private NavigationPage[] paraConfigPages = new NavigationPage[2];
        private BenchmarkConfig benchmarkConfig1;
        private AlgorithmConfig algorithmConfig1;

        public ParameterConfig()
        {
            InitializeComponent();
            initParameterConfig();
        }

        private void initParameterConfig()
        {
            loadPages();
            initParamenterConfigPages();
        }

        private void loadPages()
        {
            //benchmarkConfig
            this.benchmarkConfig1 = new BenchmarkConfig();
            this.navigationPage_benchmarkConfig.Controls.Add(this.benchmarkConfig1);
            this.benchmarkConfig1.Location = new System.Drawing.Point(0, 0);
            this.benchmarkConfig1.Name = "benchmarkConfig1";
            this.benchmarkConfig1.Size = new System.Drawing.Size(1024, 617);
            this.benchmarkConfig1.TabIndex = 0;
            //algorithmConfig
            this.algorithmConfig1 = new AlgorithmConfig();
            this.navigationPage_algorithmConfig.Controls.Add(this.algorithmConfig1);
            this.algorithmConfig1.Location = new System.Drawing.Point(0, 0);
            this.algorithmConfig1.Name = "algorithmConfig1";
            this.algorithmConfig1.Size = new System.Drawing.Size(1024, 617);
            this.algorithmConfig1.TabIndex = 1;
        }

        private void initParamenterConfigPages()
        {
            paraConfigPages[0] = navigationPage_benchmarkConfig;
            paraConfigPages[1] = navigationPage_algorithmConfig;
        }
        public Boolean frameVisible
        {
            get
            {
                return this.navigationFrame_paraConfig.Visible;
            }
            set
            {
                this.navigationFrame_paraConfig.Visible = value;
            }
        }

        public int selectedFramePage
        {
            get
            {
                //return (NavigationPage)this.navigationFrame_statusMonitor.SelectedPage; //SelectedPage是InavigationPage，时NavigationPage的父类
                for (int i = 0; i < paraConfigPages.Length; i++)
                {
                    if (this.navigationFrame_paraConfig.SelectedPage == paraConfigPages[i])
                    {
                        return i;
                    }
                }
                return -1;
            }
            set
            {
                this.navigationFrame_paraConfig.SelectedPage = paraConfigPages[value];
            }
        }

        public void setSelectedFramePage(int pageIndex)
        {
            this.navigationFrame_paraConfig.SelectedPage = paraConfigPages[pageIndex];
        }


    }
}
