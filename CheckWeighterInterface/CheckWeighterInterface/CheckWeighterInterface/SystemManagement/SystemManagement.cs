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

namespace CheckWeighterInterface.SystemManagement
{
    public partial class SystemManagement : DevExpress.XtraEditors.XtraUserControl
    {
        private NavigationPage[] systemManagementPages = new NavigationPage[3];
        private BrandManagement brandConfig1;
        private CalibrationCorrection authorityManagement1;
        private SystemConfig systemConfig1;

        public SystemManagement()
        {
            InitializeComponent();
            initSystemConfig();
        }

        private void initSystemConfig()
        {
            loadPages();
            initSystemConfigPages();
        }

        private void loadPages()
        {
            //brandManagement
            this.brandConfig1 = new BrandManagement();
            this.navigationPage_brandManagement.Controls.Add(this.brandConfig1);
            this.brandConfig1.Location = new System.Drawing.Point(0, 0);
            this.brandConfig1.Name = "brandManagement1";
            this.brandConfig1.Size = new System.Drawing.Size(1024, 617);
            this.brandConfig1.TabIndex = 0;
            //calibrationCorrection
            this.authorityManagement1 = new CalibrationCorrection();
            this.navigationPage_calibrationCorrection.Controls.Add(this.authorityManagement1);
            this.authorityManagement1.Location = new System.Drawing.Point(0, 0);
            this.authorityManagement1.Name = "authorityManagement1";
            this.authorityManagement1.Size = new System.Drawing.Size(1024, 617);
            this.authorityManagement1.TabIndex = 1;
            //systemConfig
            systemConfig1 = new SystemConfig();
            this.navigationPage_systemConfig.Controls.Add(this.systemConfig1);
            this.authorityManagement1.Location = new System.Drawing.Point(0, 0);
            this.authorityManagement1.Name = "authorityManagement1";
            this.authorityManagement1.Size = new System.Drawing.Size(1024, 617);
            this.authorityManagement1.TabIndex = 2;
        }

        private void initSystemConfigPages()
        {
            systemManagementPages[0] = navigationPage_brandManagement;
            systemManagementPages[1] = navigationPage_calibrationCorrection;
            systemManagementPages[2] = navigationPage_systemConfig;
        }
        public Boolean frameVisible
        {
            get
            {
                return this.navigationFrame_systemManagement.Visible;
            }
            set
            {
                this.navigationFrame_systemManagement.Visible = value;
            }
        }

        public int selectedFramePage
        {
            get
            {
                //return (NavigationPage)this.navigationFrame_statusMonitor.SelectedPage; //SelectedPage是InavigationPage，时NavigationPage的父类
                for (int i = 0; i < systemManagementPages.Length; i++)
                {
                    if (this.navigationFrame_systemManagement.SelectedPage == systemManagementPages[i])
                    {
                        return i;
                    }
                }
                return -1;
            }
            set
            {
                this.navigationFrame_systemManagement.SelectedPage = systemManagementPages[value];
            }
        }

        public void setSelectedFramePage(int pageIndex)
        {
            this.navigationFrame_systemManagement.SelectedPage = systemManagementPages[pageIndex];
        }


    }
}
