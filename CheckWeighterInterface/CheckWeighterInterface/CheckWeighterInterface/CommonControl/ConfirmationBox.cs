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

namespace CheckWeighterInterface.CommonControl
{
    public partial class ConfirmationBox : DevExpress.XtraEditors.XtraUserControl
    {
        public ConfirmationBox()
        {
            InitializeComponent();
        }

        public string titleConfirmationBox
        {
            get
            {
                return this.labelControl_confirmationBoxTitle.Text;
            }
            set
            {
                this.labelControl_confirmationBoxTitle.Text = value;
            }
        }

        public delegate void SimpleButtonOKClickHanlder(object sender, EventArgs e);
        public event SimpleButtonOKClickHanlder ConfirmationBoxOKClicked;
        private void simpleButton_confirmationOK_Click(object sender, EventArgs e)  //确定按钮事件；
        {
            ConfirmationBoxOKClicked(sender, new EventArgs());
            this.Dispose();
        }

        public delegate void SimpleButtonCancelClickHanlder(object sender, EventArgs e);
        public event SimpleButtonCancelClickHanlder ConfirmationBoxCancelClicked;
        private void simpleButton_confirmationCancel_Click(object sender, EventArgs e)  //取消按钮事件；
        {
            ConfirmationBoxCancelClicked(sender, new EventArgs());
            this.Dispose();
        }
    }
}
