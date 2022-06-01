namespace VisionSystemControlLibrary
{
    partial class DigitalKeyboardWindow
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
            this.digitalKeyboard = new VisionSystemControlLibrary.DigitalKeyboard();
            this.SuspendLayout();
            // 
            // digitalKeyboard
            // 
            this.digitalKeyboard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.digitalKeyboard.Chinese_Caption = " ";
            this.digitalKeyboard.English_Caption = " ";
            this.digitalKeyboard.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.digitalKeyboard.Location = new System.Drawing.Point(394, 153);
            this.digitalKeyboard.MaxLength = ((byte)(5));
            this.digitalKeyboard.MaxValue = ((short)(32767));
            this.digitalKeyboard.MinValue = ((short)(-32768));
            this.digitalKeyboard.Name = "digitalKeyboard";
            this.digitalKeyboard.NumericalValue = ((short)(0));
            this.digitalKeyboard.Size = new System.Drawing.Size(235, 462);
            this.digitalKeyboard.StringValue = "";
            this.digitalKeyboard.TabIndex = 0;
            this.digitalKeyboard.Close_Click += new System.EventHandler(this.digitalKeyboard_Close_Click);
            // 
            // DigitalKeyboardWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.digitalKeyboard);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "DigitalKeyboardWindow";
            this.ResumeLayout(false);

        }

        #endregion

        private DigitalKeyboard digitalKeyboard;
    }
}