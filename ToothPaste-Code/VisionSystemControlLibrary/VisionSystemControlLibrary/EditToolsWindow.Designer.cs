namespace VisionSystemControlLibrary
{
    partial class EditToolsWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditToolsWindow));
            this.editTools = new VisionSystemControlLibrary.EditTools();
            this.SuspendLayout();
            // 
            // editTools
            // 
            this.editTools.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.editTools.Language = VisionSystemClassLibrary.Enum.InterfaceLanguage.English;
            this.editTools.Location = new System.Drawing.Point(134, 64);
            this.editTools.Name = "editTools";
            this.editTools.Size = new System.Drawing.Size(757, 640);
            this.editTools.TabIndex = 0;
            this.editTools.Close_Click += new System.EventHandler(this.editTools_Close_Click);
            // 
            // EditToolsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(136)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.editTools);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "EditToolsWindow";
            this.Text = "EditToolsWindow";
            this.ResumeLayout(false);

        }

        #endregion

        private EditTools editTools;


    }
}