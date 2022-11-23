namespace View
{
    partial class MainFormPlugin
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
            this.panelControl = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // panelControl
            // 
            this.panelControl.Location = new System.Drawing.Point(12, 12);
            this.panelControl.Name = "panelControl";
            this.panelControl.Size = new System.Drawing.Size(776, 440);
            this.panelControl.TabIndex = 0;
            // 
            // MainFormPlugin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panelControl);
            this.Name = "MainFormPlugin";
            this.Text = "MainFormPlugin";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainFormPlugin_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelControl;
    }
}