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
using DevExpress.XtraBars.Navigation;

namespace CheckWeighterInterface.DataAnalysis
{
    public partial class DataAnalysis : DevExpress.XtraEditors.XtraUserControl
    {
        private NavigationPage[] dataAnalysisPages = new NavigationPage[2];
        private TimeDomainAnalysis timeDomainAnalysis1;
        private FrequencyDomainAnalysis frequencyDomainAnalysis1;

        public DataAnalysis()
        {
            InitializeComponent();
            initDataAnalysis();
        }

        private void initDataAnalysis()
        {
            loadPages();
            initDataAnalysisPages();
        }

        private void loadPages()
        {
            //timeDomainAnalysis
            this.timeDomainAnalysis1 = new TimeDomainAnalysis();
            this.navigationPage_timeDomainAnalysis.Controls.Add(this.timeDomainAnalysis1);
            this.timeDomainAnalysis1.Location = new System.Drawing.Point(0, 0);
            this.timeDomainAnalysis1.Name = "timeDomainAnalysis1";
            this.timeDomainAnalysis1.Size = new System.Drawing.Size(1024, 617);
            this.timeDomainAnalysis1.TabIndex = 0;
            //frequencyDoaminAnalysis
            this.frequencyDomainAnalysis1 = new FrequencyDomainAnalysis();
            this.navigationPage_frequencyDomainAnalysis.Controls.Add(this.frequencyDomainAnalysis1);
            this.frequencyDomainAnalysis1.Location = new System.Drawing.Point(0, 0);
            this.frequencyDomainAnalysis1.Name = "timeDomainAnalysis1";
            this.frequencyDomainAnalysis1.Size = new System.Drawing.Size(1024, 617);
            this.frequencyDomainAnalysis1.TabIndex = 1;
        }

        private void initDataAnalysisPages()
        {
            dataAnalysisPages[0] = navigationPage_timeDomainAnalysis;
            dataAnalysisPages[1] = navigationPage_frequencyDomainAnalysis;
        }
        public Boolean frameVisible
        {
            get
            {
                return this.navigationFrame_dataAnalysis.Visible;
            }
            set
            {
                this.navigationFrame_dataAnalysis.Visible = value;
            }
        }

        public int selectedFramePage
        {
            get
            {
                //return (NavigationPage)this.navigationFrame_statusMonitor.SelectedPage; //SelectedPage是InavigationPage，时NavigationPage的父类
                for (int i = 0; i < dataAnalysisPages.Length; i++)
                {
                    if (this.navigationFrame_dataAnalysis.SelectedPage == dataAnalysisPages[i])
                    {
                        return i;
                    }
                }
                return -1;
            }
            set
            {
                this.navigationFrame_dataAnalysis.SelectedPage = dataAnalysisPages[value];
            }
        }

        public void setSelectedFramePage(int pageIndex)
        {
            this.navigationFrame_dataAnalysis.SelectedPage = dataAnalysisPages[pageIndex];
        }

    }
}
