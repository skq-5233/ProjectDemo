namespace VisionSystemControlLibrary
{
    partial class MessageDisplayWindow
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
            this.messageDisplay = new VisionSystemControlLibrary.MessageDisplay();
            this.SuspendLayout();
            // 
            // messageDisplay
            // 
            this.messageDisplay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.messageDisplay.Chinese_Message_1 = null;
            this.messageDisplay.Chinese_Message_2 = null;
            this.messageDisplay.Chinese_Message_3 = null;
            this.messageDisplay.Chinese_Message_4 = null;
            this.messageDisplay.Chinese_Message_5 = null;
            this.messageDisplay.Chinese_Message_6 = null;
            this.messageDisplay.Chinese_Message_7 = null;
            this.messageDisplay.Chinese_Message_8 = null;
            this.messageDisplay.ControlType = VisionSystemClassLibrary.Enum.MessageDisplayType.OkCancel;
            this.messageDisplay.English_Message_1 = null;
            this.messageDisplay.English_Message_2 = null;
            this.messageDisplay.English_Message_3 = null;
            this.messageDisplay.English_Message_4 = null;
            this.messageDisplay.English_Message_5 = null;
            this.messageDisplay.English_Message_6 = null;
            this.messageDisplay.English_Message_7 = null;
            this.messageDisplay.English_Message_8 = null;
            this.messageDisplay.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.messageDisplay.Location = new System.Drawing.Point(253, 235);
            this.messageDisplay.LocationOkButton_Ok = new System.Drawing.Point(193, 228);
            this.messageDisplay.Name = "messageDisplay";
            this.messageDisplay.Size = new System.Drawing.Size(517, 297);
            this.messageDisplay.TabIndex = 0;
            this.messageDisplay.Close_Click += new System.EventHandler(this.messageDisplay_Close_Click);
            // 
            // MessageDisplayWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.messageDisplay);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MessageDisplayWindow";
            this.Text = "MessageDisplayWindow";
            this.ResumeLayout(false);

        }

        #endregion

        private MessageDisplay messageDisplay;

    }
}