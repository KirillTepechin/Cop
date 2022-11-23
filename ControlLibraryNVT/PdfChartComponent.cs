using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes.Charts;
using MigraDoc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NonVisualLibrary
{
    public partial class PdfChartComponent : Component
    {
        public PdfChartComponent()
        {
            InitializeComponent();
        }

        public PdfChartComponent(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }


        public void CreateDocument(string filepath, string docname,
            string chartname, LegendArea legendArea, Dictionary<string, double> values)
        {
            var document = DefineCharts(docname, chartname, legendArea, values);

            var renderer = new PdfDocumentRenderer(true)
            {
                Document = document
            };
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            renderer.RenderDocument();
            renderer.PdfDocument.Save(filepath);
        }
        public static Document DefineCharts(string docname, string chartname,
            LegendArea legendArea, Dictionary<string, double> values)
        {
            if(string.IsNullOrEmpty(docname)|| string.IsNullOrEmpty(chartname)||  values == null)
            {
                throw new Exception("Недостаточная заполненность данных");
            }

            Document document = new Document();
            document.AddSection();
            document.LastSection.AddParagraph(docname, "Heading1").Format.Font.Bold = true;
            
            Chart chart = new Chart(ChartType.Pie2D);
            chart.HeaderArea.AddParagraph(chartname);
           
            chart.Width = Unit.FromCentimeter(16);
            chart.Height = Unit.FromCentimeter(12);
            Series series = chart.SeriesCollection.AddSeries();
            series.Add(values.Values.ToArray());

            XSeries xseries = chart.XValues.AddXSeries();
            xseries.Add(values.Keys.ToArray());

            switch (legendArea)
            {
                case LegendArea.BOTTOM: chart.FooterArea.AddLegend();
                    break;
                case LegendArea.TOP: chart.TopArea.AddLegend();
                    break;
                case LegendArea.RIGHT: chart.RightArea.AddLegend();
                    break;
                case LegendArea.LEFT: chart.LeftArea.AddLegend();
                    break;
            }
            chart.DataLabel.Type = DataLabelType.Percent;
            chart.DataLabel.Position = DataLabelPosition.OutsideEnd;

            document.LastSection.Add(chart);

            return document;
        }
    }
}
