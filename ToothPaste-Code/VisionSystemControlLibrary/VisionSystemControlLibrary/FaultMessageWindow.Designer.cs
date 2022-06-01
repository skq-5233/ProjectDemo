namespace VisionSystemControlLibrary
{
    partial class FaultMessageWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FaultMessageWindow));
            this.faultMessage = new VisionSystemControlLibrary.FaultMessage();
            this.SuspendLayout();
            // 
            // faultMessage
            // 
            this.faultMessage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.faultMessage.DeviceNumber = 0;
            this.faultMessage.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.faultMessage.Location = new System.Drawing.Point(74, 69);
            this.faultMessage.Name = "faultMessage";
            this.faultMessage.Size = new System.Drawing.Size(876, 630);
            this.faultMessage.TabIndex = 0;
            this.faultMessage.ClearAllFaultMessages += new System.EventHandler(this.faultMessage_ClearAllFaultMessages);
            this.faultMessage.Close_Click += new System.EventHandler(this.faultMessage_Close_Click);
            // 
            // FaultMessageWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.faultMessage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FaultMessageWindow";
            this.Text = "FaultMessageWindow";
            this.ResumeLayout(false);

        }

        #endregion

        private FaultMessage faultMessage;



    }
}