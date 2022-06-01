namespace VisionSystemControlLibrary
{
    partial class StandardKeyboardWindow
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
            this.standardKeyboard = new VisionSystemControlLibrary.StandardKeyboard();
            this.SuspendLayout();
            // 
            // standardKeyboard
            // 
            this.standardKeyboard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.standardKeyboard.CapsLock = false;
            this.standardKeyboard.Chinese_Caption = " ";
            this.standardKeyboard.English_Caption = " ";
            this.standardKeyboard.FirstInitialStringValue = false;
            this.standardKeyboard.InvalidCharacter = null;
            this.standardKeyboard.IsPassword = false;
            this.standardKeyboard.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.standardKeyboard.Location = new System.Drawing.Point(157, 181);
            this.standardKeyboard.MaxLength = ((byte)(30));
            this.standardKeyboard.Name = "standardKeyboard";
            this.standardKeyboard.Password = "";
            this.standardKeyboard.PasswordStyle = 0;
            this.standardKeyboard.Shift = false;
            this.standardKeyboard.Size = new System.Drawing.Size(710, 406);
            this.standardKeyboard.StringValue = "";
            this.standardKeyboard.StringValueBuf = "";
            this.standardKeyboard.TabIndex = 1;
            this.standardKeyboard.Close_Click += new System.EventHandler(this.standardKeyboard_Close_Click);
            // 
            // StandardKeyboardWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.standardKeyboard);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "StandardKeyboardWindow";
            this.Text = "StandardKeyboardWindow";
            this.ResumeLayout(false);

        }

        #endregion

        private StandardKeyboard standardKeyboard;
    }
}