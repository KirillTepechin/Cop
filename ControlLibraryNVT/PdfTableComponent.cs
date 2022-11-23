using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Drawing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using BorderStyle = MigraDoc.DocumentObjectModel.BorderStyle;
using Color = MigraDoc.DocumentObjectModel.Color;
using Image = MigraDoc.DocumentObjectModel.Shapes.Image;
using Section = MigraDoc.DocumentObjectModel.Section;
using TabAlignment = MigraDoc.DocumentObjectModel.TabAlignment;

namespace NonVisualLibrary
{
    public partial class PdfTableComponent : Component
    {
        public PdfTableComponent()
        {
            InitializeComponent();
        }

        public PdfTableComponent(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        Document document = new Document();
        Table table = new Table();
        public List<string> Order { get; set; } 

        public void CreateDocument<T>(string filepath, string docname,
            Dictionary<int, int> rowMergeInfo, Dictionary<int, int> rowHeightInfo,
            Dictionary<Tuple<int, string>, List<string>> headers, List<T> values)
        {

            ValidateInputValues(filepath, docname, rowMergeInfo, rowHeightInfo, headers, values);

            DefineStyles();

            CreatePage(values.Count, docname, rowMergeInfo, rowHeightInfo, headers);

            ValidateTable();

            FillTable(values);

            var renderer = new PdfDocumentRenderer(true)
            {
                Document = document
            };
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            renderer.RenderDocument();
            renderer.PdfDocument.Save(filepath);
            document = new Document();           
        }
        void ValidateInputValues<T>(string filepath, string docname,
            Dictionary<int, int> rowMergeInfo, Dictionary<int, int> rowHeightInfo,
            Dictionary<Tuple<int, string>, List<string>> headers, List<T> values)
        {
            if(string.IsNullOrEmpty(filepath) || string.IsNullOrEmpty(docname)|| rowMergeInfo==null || rowHeightInfo==null||
                headers==null || values == null){
                throw new ArgumentNullException("Недостаточная заполненность данных");
            }
        }
        void ValidateTable()
        {
            foreach (Row row in table.Rows)
            {
                if (row.Cells[1].Elements.Count == 0 && row.Cells[0].Elements.Count == 0)
                {
                    throw new Exception("Шапка не заполнена");
                }
            }
            if (table.Rows.Count != Order.Count)
            {
                throw new Exception("Не для каждой строки известно свойство");
            }
        }
        void DefineStyles()
        {
            // Get the predefined style Normal.
            Style style = this.document.Styles["Normal"];
            // Because all styles are derived from Normal, the next line changes the 
            // font of the whole document. Or, more exactly, it changes the font of
            // all styles and paragraphs that do not redefine the font.
            style.Font.Name = "Verdana";

            style = this.document.Styles[StyleNames.Header];
            style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right);

            style = this.document.Styles[StyleNames.Footer];
            style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);

            // Create a new style called Table based on style Normal
            style = this.document.Styles.AddStyle("Table", "Normal");
            style.Font.Name = "Verdana";
            style.Font.Name = "Times New Roman";
            style.Font.Size = 9;

            // Create a new style called Reference based on style Normal
            style = this.document.Styles.AddStyle("Reference", "Normal");
            style.ParagraphFormat.SpaceBefore = "5mm";
            style.ParagraphFormat.SpaceAfter = "5mm";
            style.ParagraphFormat.TabStops.AddTabStop("16cm", TabAlignment.Right);
        }
        void CreatePage(int countOfColums, string docname, Dictionary<int, int> rowMergeInfo,
            Dictionary<int, int> rowHeightInfo, Dictionary<Tuple<int,string>,List<string>> headers)
        {
            // Each MigraDoc document needs at least one section.
            Section section = document.AddSection();

            var paragraph = section.AddParagraph(docname);
            paragraph.Format.SpaceAfter = "1cm";
            paragraph.Format.Alignment = ParagraphAlignment.Center;

            // Create the item table
            table = section.AddTable();
            table.Style = "Table";
            table.Borders.Color = Color.FromArgb(0,0,0,0);
            table.Borders.Width = 0.25;
            table.Borders.Left.Width = 0.5;
            table.Borders.Right.Width = 0.5;
            table.Rows.LeftIndent = 0;

            // Columns
            Column column = table.AddColumn("2.5cm");
            column.Format.Alignment = ParagraphAlignment.Center;
            column.Format.Font.Bold = true;

            column = table.AddColumn("2.5cm");
            column.Format.Alignment = ParagraphAlignment.Right;
            column.Format.Font.Bold = true;

            for (int i = 0; i < countOfColums; i++)
            {
                column = table.AddColumn("2.5cm");
                column.Format.Alignment = ParagraphAlignment.Center;
                column.Format.Font.Bold = false;
            }

            // Create the header of the table
            for(int i=0; i < rowMergeInfo.Count; i++)
            {
                Row row = table.AddRow();
                row.HeightRule = RowHeightRule.Exactly;
                row.Height = rowHeightInfo[i];
                row.HeadingFormat = true;
                row.Format.Alignment = ParagraphAlignment.Center;
                var header = headers.Where(rec => rec.Key.Item1 == i).First();
                row.Cells[0].AddParagraph(header.Key.Item2);
                row.Cells[0].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[0].VerticalAlignment = VerticalAlignment.Center;

                if (header.Value.Count == 0)
                {
                    if (row.Cells[1].Elements.Count != 0)
                    {
                        throw new Exception("Объединенные ячейки накладываются друг на друга");
                    }
                    row.Cells[0].MergeRight = rowMergeInfo[i];
                }
                else
                {
                    row.Cells[0].MergeDown = rowMergeInfo[i];
                }

                foreach (var subheader in header.Value)
                {
                    row.Cells[1].AddParagraph(subheader);
                    row.Cells[1].Format.Alignment = ParagraphAlignment.Left;
                    row.HeadingFormat = true;
                    row.HeightRule = RowHeightRule.Exactly;
                    row.Height = rowHeightInfo[i + header.Value.IndexOf(subheader) +1];
                    row = table.AddRow();
   
                }
                if (header.Value.Count > 1)
                {
                    table.Rows.RemoveObjectAt(table.Rows.Count - 1);
                }
                if (header.Value.Count != 0)
                {
                    for (int j = i + 1; j < rowMergeInfo[i] + i + 1; j++)
                    {
                        if (table.Rows[j].Cells[0].Elements.Count != 0)
                        {
                            throw new Exception("Объединенные ячейки накладываются друг на друга");
                        }
                    }
                }

            }
           
        }
        void FillTable<T>(List<T> values)
        {
            for(int i=0;i<values.Count;i++)
            {
                var value = values[i];
                List<string> listParams = new List<string>();
                var properties = value.GetType().GetProperties();
                
                foreach (var property in Order)
                {
                    var prop = value.GetType().GetProperty(property);
                    if(prop == null)
                    {
                        throw new Exception("Неизвестное свойство " + property);
                    }
                    listParams.Add(prop.GetValue(value, null).ToString());
                }
                
                for(int j=0;j<listParams.Count;j++)
                {
                    Row row = table.Rows[j];
                    row.Cells[2+i].AddParagraph(listParams[j]);
                    row.Cells[2+i].Format.Alignment = ParagraphAlignment.Left;
                }

            }
        }
    }
    
}
