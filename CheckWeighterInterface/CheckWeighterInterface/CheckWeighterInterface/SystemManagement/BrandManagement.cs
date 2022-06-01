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
    public partial class BrandManagement : DevExpress.XtraEditors.XtraUserControl
    {
        private BrandInfoBox brandInfoBox1;

        public delegate void BrandChangedReinit(object sender, EventArgs e);
        public static event BrandChangedReinit brandChangedReInitStatusMonitor;
        public static event BrandChangedReinit brandChangedReInitTimeDomainAnalysis;
        public static event BrandChangedReinit brandChangedReInitFrequencyDomainAnalysis;
        public static event BrandChangedReinit brandChangedReInitExcelExport;
        //public static event BrandChangedReinit brandChangedReInitSystemConfig;

        CommonControl.FormStandardKeyboard formStandardKeyboard;

        private int[] selectRow = { 0 };
        
        private DataTable dtBrand = new DataTable("tableBrand");

        Global.Brand addBrand;      //记录添加品牌信息
        Global.Brand delBrand;      //记录删除品牌信息
        Global.Brand copyBrand;     //记录复制品牌信息

        private CommonControl.ConfirmationBox confirmationBox;
        private CommonControl.InformationBox infoBox_successOrFail;

        public BrandManagement()
        {
            InitializeComponent();
            initBrandManagement();
        }
        //private void reInitSystemConfig(object sender, EventArgs e)
        //{
        //    initSystemConfig();
        //}

        private void initBrandManagement()
        {
            initBrandInfoBox();
            initDtBrand();
            bindDtBrand();

            //默认品牌是表brand的第一条
            this.labelControl_curBrand.Text = "当前品牌为：" + Global.curStatus.curBrand;

            //DataRow drSelected = tileView1.GetDataRow(selectRow[0]);    //获取的是grid绑定的表所有列，而不仅仅是显示出来的列
            //curBrand = drSelected["name"].ToString();
            //Global.curStatus.brand = drSelected["name"].ToString();
            //Global.overWeightThreshold = Convert.ToDouble(drSelected["upperLimit"]);
            //Global.underWeightThreshold = Convert.ToDouble(drSelected["lowerLimit"]);
            //brandChangedReInitSystemConfig += reInitSystemConfig;
        }

        private void initBrandInfoBox()
        {
            this.brandInfoBox1 = new CheckWeighterInterface.SystemManagement.BrandInfoBox();
           
            this.brandInfoBox1.Appearance.BackColor = System.Drawing.Color.White;
            this.brandInfoBox1.Appearance.Options.UseBackColor = true;
            this.brandInfoBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.brandInfoBox1.brandName = "";
            this.brandInfoBox1.Location = new System.Drawing.Point(3, 336);
            this.brandInfoBox1.Name = "brandInfoBox1";
            this.brandInfoBox1.Size = new System.Drawing.Size(292, 270);
            this.brandInfoBox1.standardWeight = "";
            this.brandInfoBox1.TabIndex = 29;
            this.brandInfoBox1.title = "";
            this.brandInfoBox1.Visible = false;
            this.brandInfoBox1.weightLowerLimit = "";
            this.brandInfoBox1.weightUpperLimit = "";

            this.panelControl_brandListLeft.Controls.Add(this.brandInfoBox1);
        }

        private void initDtBrand()
        {
            if (dtBrand.Columns.Count == 0)
            {
                dtBrand.Columns.Add("NO");
                dtBrand.Columns.Add("name");
                dtBrand.Columns.Add("standardWeight");
                dtBrand.Columns.Add("upperLimit");
                dtBrand.Columns.Add("lowerLimit");
            }

            string cmdInitDtBrand = "SELECT * FROM brand;";
            Global.mysqlHelper1._queryTableMySQL(cmdInitDtBrand, ref dtBrand);
            Global.reorderDt(ref dtBrand, "NO");
        }

        private void bindDtBrand()
        {
            this.gridControl_brandList.DataSource = dtBrand;
        }

        //更新选中行
        private void gridControl_brandList_Click(object sender, EventArgs e)
        {
            if (dtBrand.Rows.Count > 0)   //防止查询出来的结果为空表，出现越界
            {
                selectRow = this.tileView1.GetSelectedRows();
            }
            if (selectRow.Length > 1)
            {
                MessageBox.Show("当前选中不止一行");
            }
        }

        //当grid.DataSource刷新时，保持selectRow不变
        private void keepSelectRowWhenDataSourceRefresh()
        {
            if (selectRow.Length == 1)
            {
                if (selectRow[0] < this.tileView1.DataRowCount)
                    this.tileView1.FocusedRowHandle = selectRow[0];     //在DataSource发生改变后，手动修改被选中的row
                else
                {
                    this.tileView1.FocusedRowHandle = 0;
                    selectRow[0] = 0;
                }
            }
        }

        //锁定按钮
        private void lockUnlockButton(string lockOrUnlock)
        {
            if (lockOrUnlock == "lockbtn")
            {
                this.simpleButton_switchBrand.Enabled = false;
                this.simpleButton_addBrand.Enabled = false;
                this.simpleButton_deleteBrand.Enabled = false;
                this.simpleButton_copyBrand.Enabled = false;
            }
            else if (lockOrUnlock == "unlockbtn")
            {
                this.simpleButton_switchBrand.Enabled = true;
                this.simpleButton_addBrand.Enabled = true;
                this.simpleButton_deleteBrand.Enabled = true;
                this.simpleButton_copyBrand.Enabled = true;
            }
            else
                throw new ArgumentException();
        }

        private void initStandardKeyboard(string title)
        {
            formStandardKeyboard = new CommonControl.FormStandardKeyboard(title, 756, 290);
            formStandardKeyboard.standardKeyboard_CloseClick += new CommonControl.FormStandardKeyboard.StandardKeyboardCloseClickHanlder(standardKeyboard_ESC);
            formStandardKeyboard.TopMost = true;    //创建时置顶
            formStandardKeyboard.Show();
        }

        private void standardKeyboard_ESC(object sender, EventArgs e)
        {
            if (this.formStandardKeyboard.EnterNewValue == false)
            {
                addBrand.brandName = "";
                addBrand.standardWeight = 0.0D;
                addBrand.lowerLimit = 0.0D;
                addBrand.upperLimit = 0.0D;

                copyBrand.brandName = "";
                copyBrand.standardWeight = 0.0D;
                copyBrand.lowerLimit = 0.0D;
                copyBrand.upperLimit = 0.0D;

                brandInfoBox1.Visible = false;
                brandInfoBox1.clear();

                lockUnlockButton("unlockbtn");
                this.formStandardKeyboard.Close();
            }
        }

        private void initConfirmationBox(string title)
        {
            this.confirmationBox = new CommonControl.ConfirmationBox();
            this.confirmationBox.Appearance.BackColor = System.Drawing.Color.White;
            this.confirmationBox.Appearance.Options.UseBackColor = true;
            this.confirmationBox.Location = new System.Drawing.Point(337, 100);
            this.confirmationBox.Name = "confirmationBox1";
            this.confirmationBox.Size = new System.Drawing.Size(350, 150);
            this.confirmationBox.TabIndex = 29;
            this.Controls.Add(this.confirmationBox);
            this.confirmationBox.Visible = true;
            this.confirmationBox.BringToFront();
            this.confirmationBox.titleConfirmationBox = title;
        }

        private void initInfoBox_successOrFail(string infoMsg, int disappearIntervalMS)
        {
            this.infoBox_successOrFail = new CommonControl.InformationBox();
            this.infoBox_successOrFail.timeDisappear = disappearIntervalMS;
            this.infoBox_successOrFail.infoTitle = infoMsg;
            this.infoBox_successOrFail.Location = new System.Drawing.Point(337, 100);
            this.infoBox_successOrFail.Name = "informationBox1";
            this.infoBox_successOrFail.Size = new System.Drawing.Size(350, 150);
            this.infoBox_successOrFail.TabIndex = 36;
            this.Controls.Add(this.infoBox_successOrFail);
            this.infoBox_successOrFail.BringToFront();
            this.infoBox_successOrFail.disappearEnable = true;
        }

        /**************************************************切换品牌**************************************************************/
        private void simpleButton_switchBrand_Click(object sender, EventArgs e)
        {
            if (dtBrand.Rows.Count != 0)
            {
                lockUnlockButton("lockbtn");

                DataRow drSelected = tileView1.GetDataRow(selectRow[0]);    //获取的是grid绑定的表所有列，而不仅仅是显示出来的列
                if (Global.curStatus.curBrand != drSelected["name"].ToString())
                {
                    //切换品牌，修改当前品牌数据
                    Global.curStatus.curBrand = drSelected["name"].ToString();
                    this.labelControl_curBrand.Text = "当前品牌为：" + Global.curStatus.curBrand;
                    Global.overWeightThreshold = Convert.ToDouble(drSelected["upperLimit"]);
                    Global.underWeightThreshold = Convert.ToDouble(drSelected["lowerLimit"]);

                    //Global.mysqlHelper1._updateMySQL("TRUNCATE TABLE weight_history;");     //清空原先品牌的重量历史

                    //当前品牌发生改变就触发刷新
                    brandChangedReInitStatusMonitor(sender, new EventArgs());
                    brandChangedReInitTimeDomainAnalysis(sender, new EventArgs());
                    brandChangedReInitFrequencyDomainAnalysis(sender, new EventArgs());
                    brandChangedReInitExcelExport(sender, new EventArgs());
                    //brandChangedReInitSystemConfig(sender, new EventArgs());
                }

                lockUnlockButton("unlockbtn");
            }
            
        }

        /**************************************************添加品牌**************************************************************/
        private void simpleButton_addBrand_Click(object sender, EventArgs e)
        {
            lockUnlockButton("lockbtn");
            brandInfoBox1.title = "待添加品牌信息";
            brandInfoBox1.Visible = true;
            initStandardKeyboard("请输入品牌名称");
            this.formStandardKeyboard.standardKeyboard_CloseClick += new CommonControl.FormStandardKeyboard.StandardKeyboardCloseClickHanlder(standardKeyboard1_addBrand_checkOK_brandName);
        }

        private void standardKeyboard1_addBrand_checkOK_brandName(object sender, EventArgs e)
        {
            if (this.formStandardKeyboard.EnterNewValue == true)
            {
                if (this.formStandardKeyboard.StringValue != "")
                {
                    addBrand.brandName = this.formStandardKeyboard.StringValue;
                    
                    brandInfoBox1.brandName = addBrand.brandName;
                    this.formStandardKeyboard.StringValue = "";
                    this.formStandardKeyboard.Close();
                    initStandardKeyboard("请输入标准重量");
                    this.formStandardKeyboard.standardKeyboard_CloseClick += new CommonControl.FormStandardKeyboard.StandardKeyboardCloseClickHanlder(standardKeyboard1_addBrand_checkOK_standardWeight);
                }
            }

        }

        private void standardKeyboard1_addBrand_checkOK_standardWeight(object sender, EventArgs e)
        {
            if (this.formStandardKeyboard.EnterNewValue == true)
            {
                if (this.formStandardKeyboard.StringValue != "")
                {
                    addBrand.standardWeight = Convert.ToDouble(this.formStandardKeyboard.StringValue);
                    brandInfoBox1.standardWeight = addBrand.standardWeight.ToString();
                    this.formStandardKeyboard.StringValue = "";
                    this.formStandardKeyboard.Close();
                    initStandardKeyboard("请输入阈值下限");
                    this.formStandardKeyboard.standardKeyboard_CloseClick += new CommonControl.FormStandardKeyboard.StandardKeyboardCloseClickHanlder(standardKeyboard1_addBrand_checkOK_weightLowerLimit);
                }
            }
        }

        private void standardKeyboard1_addBrand_checkOK_weightLowerLimit(object sender, EventArgs e)
        {
            if (this.formStandardKeyboard.EnterNewValue == true)
            {
                if (this.formStandardKeyboard.StringValue != "")
                {
                    addBrand.lowerLimit = Convert.ToDouble(this.formStandardKeyboard.StringValue);
                    brandInfoBox1.weightLowerLimit = addBrand.lowerLimit.ToString();

                    if(addBrand.lowerLimit < addBrand.standardWeight)
                    {
                        this.formStandardKeyboard.StringValue = "";
                        this.formStandardKeyboard.Close();

                        initStandardKeyboard("请输入阈值上限");
                        this.formStandardKeyboard.standardKeyboard_CloseClick += new CommonControl.FormStandardKeyboard.StandardKeyboardCloseClickHanlder(standardKeyboard1_addBrand_checkOK_weightUpperLimit);
                    }
                    else
                    {
                        this.formStandardKeyboard.StringValue = "";
                        this.formStandardKeyboard.Close();
                        initStandardKeyboard("阈值下限非法，请重新输入");
                        this.formStandardKeyboard.standardKeyboard_CloseClick += new CommonControl.FormStandardKeyboard.StandardKeyboardCloseClickHanlder(standardKeyboard1_addBrand_checkOK_weightLowerLimit);
                    }
                }
            }
        }

        private void standardKeyboard1_addBrand_checkOK_weightUpperLimit(object sender, EventArgs e)
        {
            if (this.formStandardKeyboard.EnterNewValue == true)
            {
                if (this.formStandardKeyboard.StringValue != "")
                {
                    addBrand.upperLimit = Convert.ToDouble(this.formStandardKeyboard.StringValue);
                    brandInfoBox1.weightUpperLimit = addBrand.upperLimit.ToString();

                    if(addBrand.upperLimit > addBrand.standardWeight)
                    {
                        this.formStandardKeyboard.StringValue = "";
                        this.formStandardKeyboard.Close();

                        initConfirmationBox("确认添加：“" + addBrand.brandName + "”?");
                        this.confirmationBox.ConfirmationBoxOKClicked += new CommonControl.ConfirmationBox.SimpleButtonOKClickHanlder(this.confirmationBox_addBrand_ConfirmationBoxOKClicked);
                        this.confirmationBox.ConfirmationBoxCancelClicked += new CommonControl.ConfirmationBox.SimpleButtonCancelClickHanlder(this.confirmationBox_addBrand_ConfirmationBoxCancelClicked);
                    }
                    else
                    {
                        this.formStandardKeyboard.StringValue = "";
                        this.formStandardKeyboard.Close();

                        initStandardKeyboard("阈值上限非法，请重新输入");
                        this.formStandardKeyboard.standardKeyboard_CloseClick += new CommonControl.FormStandardKeyboard.StandardKeyboardCloseClickHanlder(standardKeyboard1_addBrand_checkOK_weightUpperLimit);
                    }
                }
            }
        }

        private void confirmationBox_addBrand_ConfirmationBoxOKClicked(object sender, EventArgs e)
        {
            string cmdAddBrand = "INSERT INTO brand (`name`, `standardWeight`, `lowerLimit`, `upperLimit`) VALUES ('" +
                                 addBrand.brandName + "', '" + addBrand.standardWeight.ToString() + "', '" + addBrand.lowerLimit.ToString() + "', '" + addBrand.upperLimit.ToString() + "');";

            if (Global.mysqlHelper1._insertMySQL(cmdAddBrand))
            {
                initInfoBox_successOrFail("品牌“" + addBrand.brandName + "”添加成功！", 1000);
                initDtBrand();      //添加品牌后，刷新grid
                //若添加的是第一个品牌，就将当前品牌设为它
                if (dtBrand.Rows.Count == 1)
                {
                    selectRow[0] = 0;
                    this.tileView1.FocusedRowHandle = selectRow[0];
                    DataRow drSelected = tileView1.GetDataRow(selectRow[0]);
                    Global.curStatus.curBrand = drSelected["name"].ToString();
                    this.labelControl_curBrand.Text = "当前品牌为：" + Global.curStatus.curBrand;

                    Global.underWeightThreshold = Convert.ToDouble(drSelected["lowerLimit"]);
                    Global.overWeightThreshold = Convert.ToDouble(drSelected["upperLimit"]);

                    //当前品牌发生改变就触发页面刷新
                    brandChangedReInitStatusMonitor(sender, new EventArgs());
                }
            }
            else
            {
                initInfoBox_successOrFail("品牌“" + addBrand.brandName + "”添加失败！", 1000);
            }

            brandInfoBox1.Visible = false;
            brandInfoBox1.clear();
            lockUnlockButton("unlockbtn");
        }

        private void confirmationBox_addBrand_ConfirmationBoxCancelClicked(object sender, EventArgs e)
        {
            addBrand.brandName = "";
            addBrand.standardWeight = 0.0D;
            addBrand.lowerLimit = 0.0D;
            addBrand.upperLimit = 0.0D;

            brandInfoBox1.Visible = false;
            brandInfoBox1.clear();

            lockUnlockButton("unlockbtn");
        }

        /**************************************************删除品牌**************************************************************/
        private void simpleButton_deleteBrand_Click(object sender, EventArgs e)
        {
            if (dtBrand.Rows.Count != 0)
            {
                lockUnlockButton("lockbtn");

                DataRow drSelected = tileView1.GetDataRow(selectRow[0]);
                delBrand.brandName = drSelected["name"].ToString();
                delBrand.standardWeight = Convert.ToDouble(drSelected["standardWeight"]);
                delBrand.lowerLimit = Convert.ToDouble(drSelected["lowerLimit"]);
                delBrand.upperLimit = Convert.ToDouble(drSelected["upperLimit"]);

                initConfirmationBox("确认删除品牌：“" + delBrand.brandName + "”?");
                this.confirmationBox.ConfirmationBoxOKClicked += new CommonControl.ConfirmationBox.SimpleButtonOKClickHanlder(this.confirmationBox_delBrand_ConfirmationBoxOKClicked);
                this.confirmationBox.ConfirmationBoxCancelClicked += new CommonControl.ConfirmationBox.SimpleButtonCancelClickHanlder(this.confirmationBox_delBrand_ConfirmationBoxCancelClicked);
            }
            
        }

        private void confirmationBox_delBrand_ConfirmationBoxOKClicked(object sender, EventArgs e)
        {
            string cmdDelBrand = "DELETE FROM brand WHERE `name`='" + delBrand.brandName + "';";
            if (Global.mysqlHelper1._deleteMySQL(cmdDelBrand))
            {
                initInfoBox_successOrFail("品牌“" + delBrand.brandName + "”删除成功！", 1000);
                initDtBrand();
                keepSelectRowWhenDataSourceRefresh();

                //若删除的是当前选中的品牌，则切换品牌为数据库第一个。若数据库已空无法选中第一个，则令Global.curStatus.brand=""
                if (delBrand.brandName == Global.curStatus.curBrand)
                {
                    if (dtBrand.Rows.Count != 0)
                    {
                        //Global.mysqlHelper1._updateMySQL("TRUNCATE TABLE weight_history;");     //清空原先品牌的重量历史

                        selectRow[0] = 0;
                        this.tileView1.FocusedRowHandle = selectRow[0];
                        DataRow drSelected = tileView1.GetDataRow(selectRow[0]);
                        Global.curStatus.curBrand = drSelected["name"].ToString();
                        this.labelControl_curBrand.Text = "当前品牌为：" + Global.curStatus.curBrand;
                        Global.overWeightThreshold = Convert.ToDouble(drSelected["upperLimit"]);
                        Global.underWeightThreshold = Convert.ToDouble(drSelected["lowerLimit"]);

                        //当前品牌切换就发生页面刷新
                        brandChangedReInitStatusMonitor(sender, new EventArgs());
                        brandChangedReInitTimeDomainAnalysis(sender, new EventArgs());
                        brandChangedReInitFrequencyDomainAnalysis(sender, new EventArgs());
                        brandChangedReInitExcelExport(sender, new EventArgs());
                    }
                    else
                    {
                        Global.curStatus.curBrand = "";
                        this.labelControl_curBrand.Text = "当前品牌为：" + Global.curStatus.curBrand;
                    }

                }
            }
            else
            {
                initInfoBox_successOrFail("品牌“" + delBrand.brandName + "”删除失败！", 1000);
            }

            lockUnlockButton("unlockbtn");
        }

        private void confirmationBox_delBrand_ConfirmationBoxCancelClicked(object sender, EventArgs e)
        {
            lockUnlockButton("unlockbtn");
        }

        /**************************************************复制品牌**************************************************************/
        private void simpleButton_copyBrand_Click(object sender, EventArgs e)
        {
            if (dtBrand.Rows.Count != 0)
            {
                lockUnlockButton("lockbtn");

                DataRow drSelected = tileView1.GetDataRow(selectRow[0]);
                copyBrand.brandName = drSelected["name"].ToString();
                copyBrand.standardWeight = Convert.ToDouble(drSelected["standardWeight"]);
                copyBrand.lowerLimit = Convert.ToDouble(drSelected["lowerLimit"]);
                copyBrand.upperLimit = Convert.ToDouble(drSelected["upperLimit"]);

                brandInfoBox1.title = "待复制品牌信息";
                brandInfoBox1.Visible = true;
                brandInfoBox1.standardWeight = copyBrand.standardWeight.ToString();
                brandInfoBox1.weightLowerLimit = copyBrand.lowerLimit.ToString();
                brandInfoBox1.weightUpperLimit = copyBrand.upperLimit.ToString();

                initConfirmationBox("确认复制品牌：“" + copyBrand.brandName + "”?");
                this.confirmationBox.ConfirmationBoxOKClicked += new CommonControl.ConfirmationBox.SimpleButtonOKClickHanlder(this.confirmationBox_copyBrand_ConfirmationBoxOKClicked);
                this.confirmationBox.ConfirmationBoxCancelClicked += new CommonControl.ConfirmationBox.SimpleButtonCancelClickHanlder(this.confirmationBox_copyBrand_ConfirmationBoxCancelClicked);
            }
        }

        private void confirmationBox_copyBrand_ConfirmationBoxOKClicked(object sender, EventArgs e)
        {
            initStandardKeyboard("请输入新的品牌名称");
            this.formStandardKeyboard.standardKeyboard_CloseClick += new CommonControl.FormStandardKeyboard.StandardKeyboardCloseClickHanlder(confirmationBox_copyBrandEnterNewName);
        }

        private void confirmationBox_copyBrand_ConfirmationBoxCancelClicked(object sender, EventArgs e)
        {
            brandInfoBox1.Visible = false;
            brandInfoBox1.clear();
            lockUnlockButton("unlockbtn");
        }

        private void confirmationBox_copyBrandEnterNewName(object sender, EventArgs e)
        {
            if (this.formStandardKeyboard.EnterNewValue == true)
            {
                if (this.formStandardKeyboard.StringValue != "")
                {
                    copyBrand.brandName = this.formStandardKeyboard.StringValue;    //输入的品牌名替代原品牌名
                    brandInfoBox1.brandName = copyBrand.brandName;
                    this.formStandardKeyboard.StringValue = "";
                    this.formStandardKeyboard.Close();
                }

                string cmdCopyBrand = "INSERT INTO brand (`name`, `standardWeight`, `lowerLimit`, `upperLimit`) VALUES ('" +
                                 copyBrand.brandName + "', '" + copyBrand.standardWeight.ToString() + "', '" + copyBrand.lowerLimit.ToString() + "', '" + copyBrand.upperLimit.ToString() + "');";

                if (Global.mysqlHelper1._insertMySQL(cmdCopyBrand))
                {
                    initInfoBox_successOrFail("品牌“" + copyBrand.brandName + "”添加成功！", 1000);
                }
                else
                {
                    initInfoBox_successOrFail("品牌“" + copyBrand.brandName + "”添加失败！", 1000);
                }

                initDtBrand();      
                keepSelectRowWhenDataSourceRefresh();

                brandInfoBox1.Visible = false;
                brandInfoBox1.clear();
                lockUnlockButton("unlockbtn");
            }
        }
    }
}
