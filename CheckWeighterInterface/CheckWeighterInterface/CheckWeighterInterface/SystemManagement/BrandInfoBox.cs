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
    public partial class BrandInfoBox : DevExpress.XtraEditors.XtraUserControl
    {
        public BrandInfoBox()
        {
            InitializeComponent();
        }

        public BrandInfoBox(string title, int locX, int locY)
        {
            this.labelControl_title.Text = title;
            this.Location = new Point(locX, locY);
        }

        public string title
        {
            get
            {
                return this.labelControl_title.Text;
            }
            set
            {
                this.labelControl_title.Text = value;
            }
        }

        public string brandName
        {
            get
            {
                return this.labelControl_brandName_val.Text;
            }
            set
            {
                this.labelControl_brandName_val.Text = value;
            }
        }

        public string standardWeight
        {
            get
            {
                return this.labelControl_standardWeight_val.Text;
            }
            set
            {
                this.labelControl_standardWeight_val.Text = value;
            }
        }

        public string weightLowerLimit
        {
            get
            {
                return this.labelControl_weightLowerLimit_val.Text;
            }
            set
            {
                this.labelControl_weightLowerLimit_val.Text = value;
            }
        }

        public string weightUpperLimit
        {
            get
            {
                return this.labelControl_weightUpperLimit_val.Text;
            }
            set
            {
                this.labelControl_weightUpperLimit_val.Text = value;
            }
        }

        public void clear()
        {
            this.brandName = "";
            this.standardWeight = "";
            this.weightLowerLimit = "";
            this.weightUpperLimit = "";
        }


    }
}
