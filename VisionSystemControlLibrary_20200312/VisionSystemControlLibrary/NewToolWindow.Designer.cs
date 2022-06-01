namespace VisionSystemControlLibrary
{
    partial class NewToolWindow
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
            this.newTool = new VisionSystemControlLibrary.NewTool();
            this.SuspendLayout();
            // 
            // newTool
            // 
            this.newTool.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.newTool.Location = new System.Drawing.Point(105, 19);
            this.newTool.Name = "newTool";
            this.newTool.Size = new System.Drawing.Size(815, 730);
            this.newTool.TabIndex = 0;
            // 
            // NewToolWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.newTool);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "NewToolWindow";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NewToolWindow";
            this.ResumeLayout(false);

        }

        #endregion

        private NewTool newTool;
    }
}