namespace VisionSystemControlLibrary
{
    partial class FaultMessageOptionWindow
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
            this.faultMessageOption = new VisionSystemControlLibrary.FaultMessageOption();
            this.SuspendLayout();
            // 
            // faultMessageOption
            // 
            this.faultMessageOption.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.faultMessageOption.FaultMessageState = new bool[] {
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false,
        false};
            this.faultMessageOption.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.faultMessageOption.Location = new System.Drawing.Point(282, 84);
            this.faultMessageOption.Name = "faultMessageOption";
            this.faultMessageOption.Size = new System.Drawing.Size(460, 600);
            this.faultMessageOption.TabIndex = 0;
            this.faultMessageOption.Close_Click += new System.EventHandler(this.faultMessageOption_Close_Click);
            // 
            // FaultMessageOptionWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.faultMessageOption);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FaultMessageOptionWindow";
            this.Text = "FaultMessageOptionWindow";
            this.ResumeLayout(false);

        }

        #endregion

        private FaultMessageOption faultMessageOption;

    }
}