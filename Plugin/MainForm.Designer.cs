namespace View
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            this.dataGridViewControl = new ControlsLibraryFramework48.Data.ControlDataTableRow();
            this.wordChartComponent = new NonVisualComponentsNETFramework.GistagramWord(this.components);
            this.excelTableComponent = new WinFormsControlLibrary.ExcelTableComponent(this.components);
            this.pdfImagesComponent = new NonVisualLibrary.PdfImagesComponent(this.components);
            this.SuspendLayout();
            // 
            // dataGridViewControl
            // 
            this.dataGridViewControl.Location = new System.Drawing.Point(0, 1);
            this.dataGridViewControl.Name = "dataGridViewControl";
            this.dataGridViewControl.SelectedRowIndex = -1;
            this.dataGridViewControl.Size = new System.Drawing.Size(799, 458);
            this.dataGridViewControl.TabIndex = 2;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dataGridViewControl);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private ControlsLibraryFramework48.Data.ControlDataTableRow dataGridViewControl;
        private NonVisualComponentsNETFramework.GistagramWord wordChartComponent;
        private WinFormsControlLibrary.ExcelTableComponent excelTableComponent;
        private NonVisualLibrary.PdfImagesComponent pdfImagesComponent;
    }
}