namespace VisionSystemControlLibrary
{
    partial class CigaretteSortWindow
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
            this.cigaretteSort1 = new VisionSystemControlLibrary.CigaretteSort();
            this.SuspendLayout();
            // 
            // cigaretteSort1
            // 
            this.cigaretteSort1.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.cigaretteSort1.Location = new System.Drawing.Point(242, 69);
            this.cigaretteSort1.Name = "cigaretteSort1";
            this.cigaretteSort1.Size = new System.Drawing.Size(540, 630);
            this.cigaretteSort1.TabIndex = 0;
            this.cigaretteSort1.Close_Click += new System.EventHandler(this.cigaretteSort1_Close_Click);
            // 
            // CigaretteSortWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.cigaretteSort1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CigaretteSortWindow";
            this.Text = "CigaretteSortWindow";
            this.ResumeLayout(false);

        }

        #endregion

        private CigaretteSort cigaretteSort1;
    }
}