namespace VisionSystemUserInterface
{
    partial class TolerancesSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tolerancesControl = new VisionSystemControlLibrary.TolerancesControl();
            this.SuspendLayout();
            // 
            // tolerancesControl
            // 
            this.tolerancesControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.tolerancesControl.CurrentPage = 0;
            this.tolerancesControl.GraphNumber_Total = 9;
            this.tolerancesControl.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.tolerancesControl.Location = new System.Drawing.Point(0, 106);
            this.tolerancesControl.Name = "tolerancesControl";
            this.tolerancesControl.SaveProduct = false;
            this.tolerancesControl.Size = new System.Drawing.Size(1024, 662);
            this.tolerancesControl.SystemDeviceState = VisionSystemClassLibrary.Enum.DeviceState.Stop;
            this.tolerancesControl.TabIndex = 1;
            this.tolerancesControl.TotalPage = 3;
            this.tolerancesControl.View = true;
            this.tolerancesControl.Control_DoubleClick += new System.EventHandler(this.tolerancesControl_Control_DoubleClick);
            this.tolerancesControl.RunStop_Click += new System.EventHandler(this.tolerancesControl_RunStop_Click);
            this.tolerancesControl.Learning_Click += new System.EventHandler(this.tolerancesControl_Learning_Click);
            this.tolerancesControl.Close_Click += new System.EventHandler(this.tolerancesControl_Close_Click);
            this.tolerancesControl.SaveProduct_Click += new System.EventHandler(this.tolerancesControl_SaveProduct_Click);
            this.tolerancesControl.ResetGraphs_Click += new System.EventHandler(this.tolerancesControl_ResetGraphs_Click);
            this.tolerancesControl.PreviousPage_Click += new System.EventHandler(this.tolerancesControl_PreviousPage_Click);
            this.tolerancesControl.NextPage_Click += new System.EventHandler(this.tolerancesControl_NextPage_Click);
            this.tolerancesControl.SetGraphValueSuccess += new System.EventHandler(this.tolerancesControl_SetGraphValueSuccess);
            this.tolerancesControl.EjectLevel_Click += new System.EventHandler(this.tolerancesControl_EjectLevel_Click);
            this.tolerancesControl.UpdateTolerances_Click += new System.EventHandler(this.tolerancesControl_UpdateTolerances_Click);
            // 
            // TolerancesSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.tolerancesControl);
            this.Name = "TolerancesSettings";
            this.Text = "TolerancesSettings";
            this.Load += new System.EventHandler(this.TolerancesSettings_Load);
            this.Controls.SetChildIndex(this.tolerancesControl, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private VisionSystemControlLibrary.TolerancesControl tolerancesControl;


    }
}