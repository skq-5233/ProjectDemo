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


namespace CheckWeighterInterface.SystemTest
{
    public partial class SystemTest : DevExpress.XtraEditors.XtraUserControl
    {
        private NavigationPage[] systemTestPages = new NavigationPage[2];
        private SignalTest signalTest1;
        private RealTimeCurve realTimeCurve1;

        public SystemTest()
        {
            InitializeComponent();
            initSystemTest();

        }


        private void initSystemTest()
        {
            loadPages();
            initSystemTestPages();
        }

        private void loadPages()
        {
            //benchmarkConfig
            this.signalTest1 = new SignalTest();
            this.navigationPage_signalTest.Controls.Add(this.signalTest1);
            this.signalTest1.Location = new System.Drawing.Point(0, 0);
            this.signalTest1.Name = "signalTest1";
            this.signalTest1.Size = new System.Drawing.Size(1024, 617);
            this.signalTest1.TabIndex = 0;
            //algorithmConfig
            this.realTimeCurve1 = new RealTimeCurve();
            this.navigationPage_realTimeCurve.Controls.Add(this.realTimeCurve1);
            this.realTimeCurve1.Location = new System.Drawing.Point(0, 0);
            this.realTimeCurve1.Name = "realTimeCurve1";
            this.realTimeCurve1.Size = new System.Drawing.Size(1024, 617);
            this.realTimeCurve1.TabIndex = 1;
        }

        private void initSystemTestPages()
        {
            systemTestPages[0] = navigationPage_signalTest;
            systemTestPages[1] = navigationPage_realTimeCurve;
        }
        public Boolean frameVisible
        {
            get
            {
                return this.navigationFrame_sysTest.Visible;
            }
            set
            {
                this.navigationFrame_sysTest.Visible = value;
            }
        }

        public int selectedFramePage
        {
            get
            {
                //return (NavigationPage)this.navigationFrame_statusMonitor.SelectedPage; //SelectedPage是InavigationPage，时NavigationPage的父类
                for (int i = 0; i < systemTestPages.Length; i++)
                {
                    if (this.navigationFrame_sysTest.SelectedPage == systemTestPages[i])
                    {
                        return i;
                    }
                }
                return -1;
            }
            set
            {
                this.navigationFrame_sysTest.SelectedPage = systemTestPages[value];
            }
        }

        public void setSelectedFramePage(int pageIndex)
        {
            this.navigationFrame_sysTest.SelectedPage = systemTestPages[pageIndex];
        }
    }
}
