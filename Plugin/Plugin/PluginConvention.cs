using Contracts;
using ControlsLibraryFramework48.Data;
using ControlsLibraryFramework48.Models;
using Database;
using Database.Model;
using Logic;
using NonVisualComponentsNETFramework;
using NonVisualComponentsNETFramework.HelperModels;
using NonVisualLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using View;
using WinFormsControlLibrary;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Plugin.Plugin
{
    [Export(typeof(IPluginsConvention))]
    public class PluginConvention : IPluginsConvention
    {
        private ProductLogic _productLogic;
        private VendorStorage _vendorStorage;
        private ControlDataTableRow dataGridViewControl;
        public PluginConvention()
        {
           
            var menu = new ContextMenuStrip();
            var menuItem = new ToolStripMenuItem("Продукты");
            menu.Items.Add(menuItem);
            menuItem.Click += (sender, e) =>
            {
                var formProduct = new MainForm(_productLogic, _vendorStorage);
                formProduct.ShowDialog();
            };
            dataGridViewControl.ContextMenuStrip = menu;

            dataGridViewControl = new ControlDataTableRow();
            List<DataTableColumnConfig> columns = new List<DataTableColumnConfig>() 
            {
                    new DataTableColumnConfig { ColumnHeader = "Id", PropertyName = "Id", Visible = false, Width = 100 },
                    new DataTableColumnConfig { ColumnHeader = "Название", PropertyName = "ProductName", Visible = true, Width = 100 },
                    new DataTableColumnConfig { ColumnHeader = "Изображение", PropertyName = "Image", Visible = false, Width = 100 },
                    new DataTableColumnConfig { ColumnHeader = "Поставщик", PropertyName = "Vendor", Visible = true, Width = 100},
                    new DataTableColumnConfig { ColumnHeader = "Дата поставки", PropertyName = "DeliveryDate", Visible = true, Width = 100}
            };

            dataGridViewControl.LoadColumns(columns);
            _productLogic = new ProductLogic(new ProductStorage());
            _vendorStorage = new VendorStorage();
            ReloadData();

        }
        public string PluginName => "Продукты";

        public UserControl GetControl => dataGridViewControl;

        public PluginsConventionElement GetElement => dataGridViewControl.GetSelectedObject<PluginsConventionElement>();

        public bool CreateChartDocument(PluginsConventionSaveDocument saveDocument)
        {
            try
            {
                List<ProductViewModel> products = _productLogic.Read(null);
                List<Vendor> vendors = _vendorStorage.GetFullList();
                var wordChartComponent = new GistagramWord();
                var data = new List<TestData>();
                vendors.ForEach(v => data.Add(new TestData()
                { name = v.VendorName, value = products.Where(product => product.Vendor.Equals(v.VendorName)).Count() }));

                wordChartComponent.ReportSaveGistogram(saveDocument.FileName, "Отчет поставщиков по продукции", "Гистограмма", LocationLegend.BottomLeft, data);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool CreateSimpleDocument(PluginsConventionSaveDocument saveDocument)
        {
            try
            {
                List<ProductViewModel> products = _productLogic.Read(null);
                var pdfImagesComponent = new PdfImagesComponent();
                pdfImagesComponent.CreateDocument(saveDocument.FileName,
                    "Изображения продуктов",
                    products.Select(product => product.Image).ToArray());
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public bool CreateTableDocument(PluginsConventionSaveDocument saveDocument)
        {
            try
            {
                var products = _productLogic.Read(null).Select(p => p.ToFields()).ToList();
                var excelTableComponent = new ExcelTableComponent();
                excelTableComponent.columnsName = new List<string> { "Id", "ProductName", "Vendor", "DeliveryDate" };
                var mergeCellsList = new List<MergeCells> { new MergeCells("Продукт", new int[] { 1, 2 }) };
                excelTableComponent.CreateTableExcel(
                    saveDocument.FileName, "Отчет по продутам", mergeCellsList,
                    new int[] { 20, 20, 20, 20 }, new string[] { "Идент.", "Название", "Производитель", "Дата поставки" },
                    products
                    );

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool DeleteElement(PluginsConventionElement element)
        {
            var id = dataGridViewControl.GetSelectedObject<ProductViewModel>().Id;

            try
            {
                _productLogic.Delete(new ProductBindingModel { Id = id });
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }


        public Form GetForm(PluginsConventionElement element)
        {
            var form = new AddProductForm(_vendorStorage, _productLogic);
            if (element != null)
            {
                form.Id = element.Id;
            }

            return form;
        }

        public void ReloadData()
        {
            dataGridViewControl.Clear();
            var list = _productLogic.Read(null);
            if (list != null)
            {
                foreach (var p in list)
                {
                    dataGridViewControl.AddRow(p);
                }
            }
        }
    }
}
