using MigraDoc.DocumentObjectModel;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NonVisualLibrary
{
    public partial class PdfImagesComponent : Component
    {
        public PdfImagesComponent()
        {
            InitializeComponent();
        }

        public PdfImagesComponent(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
        public void CreateDocument(string filepath, string docname, string[] images)
        {
            if(string.IsNullOrEmpty(filepath) || string.IsNullOrEmpty(docname) || images.Length == 0){
                throw new ArgumentNullException("Недостаточная заполненость данных");
            }

            PdfDocument document = new PdfDocument();

            PdfPage page = document.AddPage();

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            XFont font = new XFont("Verdana", 20, XFontStyle.BoldItalic);

            XGraphics gfx = XGraphics.FromPdfPage(page);
            gfx.DrawString(docname, font, XBrushes.Black,
                          new XRect(0, 0, page.Width, page.Height),
                          XStringFormats.TopCenter);

            foreach (string image in images)
            {
                DrawImage(gfx, image, 50, 50);
                page = document.AddPage();
                gfx = XGraphics.FromPdfPage(page);
            }
            document.Pages.RemoveAt(document.PageCount - 1);
            document.Save(filepath);
        }

        private void DrawImage(XGraphics gfx, string jpegSamplePath, int x, int y)
        {
            XImage image = XImage.FromFile(jpegSamplePath);
            gfx.DrawImage(image, x, y);
        }
    }
}
