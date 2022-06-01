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
    public partial class FormStandardKeyboard : DevExpress.XtraEditors.XtraForm
    {
        private VisionSystemControlLibrary.StandardKeyboard standardKeyboard1;

        private bool flagSign = false;                  //是否调用比较触发Close_Click事件。true为使用
        private bool flagCompare = false;               //作比较是>(true)还是<(false)触发Close_Click事件
        private double compareValue = 0.0D;     //Close_Click事件中作比较的值

        public FormStandardKeyboard()
        {
            InitializeComponent();
        }

        public FormStandardKeyboard(string title, int locationX, int locationY)
        {
            InitializeComponent();
            _initFormStandardKeyboard(title);
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(locationX, locationY);
        }

        public string StringValue
        {
            get
            {
                return this.standardKeyboard1.StringValue;
            }
            set
            {
                this.standardKeyboard1.StringValue = value;
            }
        }

        public bool EnterNewValue
        {
            get
            {
                return this.standardKeyboard1.EnterNewValue;
            }
        }

        public double CompVal
        {
            get
            {
                return this.compareValue;
            }
            set
            {
                this.compareValue = value;  
            }
        }

        public bool FlagSign
        {
            get
            {
                return this.flagSign;
            }
            set
            {
                this.flagSign = value;
            }
        }

        public bool FlagCompare
        {
            get
            {
                return this.flagCompare;
            }
            set
            {
                this.flagCompare = value;
            }
        }

        private void _initFormStandardKeyboard(string title)
        {
            if (this.standardKeyboard1 != null)
                this.standardKeyboard1.Dispose();
            this.standardKeyboard1 = new VisionSystemControlLibrary.StandardKeyboard();
            this.standardKeyboard1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(67)))), ((int)(((byte)(73)))));
            this.standardKeyboard1.CapsLock = false;
            this.standardKeyboard1.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.Chinese;
            this.standardKeyboard1.Chinese_Caption = title;
            this.standardKeyboard1.FirstInitialStringValue = false;
            this.standardKeyboard1.InvalidCharacter = null;
            this.standardKeyboard1.IsPassword = false;
            this.standardKeyboard1.Location = new System.Drawing.Point(0, 0);
            this.standardKeyboard1.MaxLength = ((byte)(30));
            this.standardKeyboard1.Name = "standardKeyboard1";
            this.standardKeyboard1.Password = "";
            this.standardKeyboard1.PasswordStyle = 0;
            this.standardKeyboard1.Shift = false;
            this.standardKeyboard1.Size = new System.Drawing.Size(710, 406);
            this.standardKeyboard1.StringValue = "";
            this.standardKeyboard1.StringValueBuf = "";
            this.standardKeyboard1.TabIndex = 0;
            this.Controls.Add(this.standardKeyboard1);
            this.standardKeyboard1.BringToFront();
            this.standardKeyboard1.Visible = true;
            //this.standardKeyboard1.Close_Click += new System.EventHandler(standardKeyboard_ESC);
            this.standardKeyboard1.Close_Click += new System.EventHandler(standardKeyboard_Close_Click);
        }

        public delegate void StandardKeyboardCloseClickHanlder(object sender, EventArgs e);
        public event StandardKeyboardCloseClickHanlder standardKeyboard_CloseClick;
        //private void standardKeyboard_ESC(object sender, EventArgs e)
        //{
        //    if (this.standardKeyboard1.EnterNewValue == false)
        //    {
        //        //this.standardKeyboard1.Dispose();
        //        this.Dispose();
        //    }

        //    if (standardKeyboardCloseClick != null)
        //    {
        //        standardKeyboardCloseClick(sender, new EventArgs());
        //    }
        //}

        private void standardKeyboard_Close_Click(object sender, EventArgs e)
        {
            if (standardKeyboard_CloseClick != null)
            {
                standardKeyboard_CloseClick(sender, new EventArgs());
            }


            //if(flagSign == true)
            //{
            //    if (flagCompare == true)
            //    {
            //        if (this.standardKeyboard1.EnterNewValue && (Convert.ToDouble(this.standardKeyboard1.StringValue) > this.compareValue))
            //        {
            //            if (standardKeyboard_CloseClick != null)
            //            {
            //                standardKeyboard_CloseClick(sender, new EventArgs());
            //            }
            //        }
            //    }
            //    else
            //    {
            //        if (this.standardKeyboard1.EnterNewValue && (Convert.ToDouble(this.standardKeyboard1.StringValue) < this.compareValue))
            //        {
            //            if (standardKeyboard_CloseClick != null)
            //            {
            //                standardKeyboard_CloseClick(sender, new EventArgs());
            //            }
            //        }
            //    }
            //    flagSign = false;
            //}
            //else
            //{
            //    if (standardKeyboard_CloseClick != null)
            //    {
            //        standardKeyboard_CloseClick(sender, new EventArgs());
            //    }
            //}

        }



    }
}