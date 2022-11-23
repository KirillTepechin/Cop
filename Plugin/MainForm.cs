using Contracts;
using Database.Model;
using KopForms;
using Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unity;
using ControlsLibraryFramework48;
using ControlsLibraryFramework48.Models;
using NonVisualLibrary;
using Database;
using NonVisualComponentsNETFramework.HelperModels;
using NonVisualComponentsNETFramework;
using WinFormsControlLibrary;

namespace Plugin
{
    public partial class MainForm : Form
    {
        private ContextMenuStrip contextMenu = new ContextMenuStrip();
        private ProductLogic _productLogic;
        private VendorStorage _vendorStorage;
        public MainForm(ProductLogic productLogic, VendorStorage vendorStorage)
        {
            InitializeComponent();
            _productLogic = productLogic;
            _vendorStorage = vendorStorage;
            InitContextMenu();
        }
        public void InitContextMenu()
        {
            ToolStripMenuItem addMenuItem = new ToolStripMenuItem("Добавить");
            ToolStripMenuItem updateMenuItem = new ToolStripMenuItem("Изменить");
            ToolStripMenuItem deleteMenuItem = new ToolStripMenuItem("Удалить");
            ToolStripMenuItem pdfDocMenuItem = new ToolStripMenuItem("Создать pdf с изображениями");
            ToolStripMenuItem excelDocMenuItem = new ToolStripMenuItem("Создать excel таблицу");
            ToolStripMenuItem wordDocMenuItem = new ToolStripMenuItem("Создать word с гистограммой");
            ToolStripMenuItem vendorsFormCall = new ToolStripMenuItem("Справочник");


            contextMenu.Items.AddRange(new[] { addMenuItem, updateMenuItem, deleteMenuItem,
                pdfDocMenuItem, excelDocMenuItem, wordDocMenuItem, vendorsFormCall });

            dataGridViewControl.ContextMenuStrip = contextMenu;

            addMenuItem.Click += AddMenuItem_Click;
            updateMenuItem.Click += UpdateMenuItem_Click;
            deleteMenuItem.Click += DeleteMenuItem_Click;
            pdfDocMenuItem.Click += PdfDocMenuItem_Click;
            excelDocMenuItem.Click += ExcelDocMenuItem_Click;
            wordDocMenuItem.Click += WordDocMenuItem_Click;
            vendorsFormCall.Click += VendorsForm_Click;
        }
        private void VendorsForm_Click(object sender, EventArgs e)
        {
            var form = Program.Container.Resolve<VendorsForm>();

            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }
        private void AddMenuItem_Click(object sender, EventArgs e)
        {
            var form = Program.Container.Resolve<AddProductForm>();
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }
        private void UpdateMenuItem_Click(object sender, EventArgs e)
        {
            var form = Program.Container.Resolve<AddProductForm>();
            var id = dataGridViewControl.GetSelectedObject<ProductViewModel>().Id;
            form.Id=id;
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }
        private void DeleteMenuItem_Click(object sender, EventArgs e)
        {
            var id = dataGridViewControl.GetSelectedObject<ProductViewModel>().Id;
            DialogResult dialogResult = MessageBox.Show("Удалить запись?", "Удаление", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
            if (dialogResult.Equals(DialogResult.OK))
            {
                _productLogic.Delete(new ProductBindingModel { Id = id });
                LoadData();
            }
            
        }
        private void PdfDocMenuItem_Click(object sender ,EventArgs e)
        {
            using (var dialog = new SaveFileDialog { Filter = "pdf|*.pdf" })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        List<ProductViewModel> products = _productLogic.Read(null);

                        pdfImagesComponent.CreateDocument(dialog.FileName,
                            "Изображения продуктов",
                            products.Select(product => product.Image).ToArray());
                        MessageBox.Show("Выполнено", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void ExcelDocMenuItem_Click(object sender, EventArgs e)
        { 
            using (var dialog = new SaveFileDialog { Filter = "xlsx|*.xlsx" })
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var products = _productLogic.Read(null).Select(p => p.ToFields()).ToList();
                        excelTableComponent.columnsName = new List<string> { "Id", "ProductName", "Vendor", "DeliveryDate" };
                        var mergeCellsList = new List<MergeCells> { new MergeCells("Продукт", new int[] { 1, 2 }) };
                        excelTableComponent.CreateTableExcel(
                            dialog.FileName, "Отчет по продутам", mergeCellsList,
                            new int[] { 20, 20, 20, 20 }, new string[] { "Идент.", "Название", "Производитель", "Дата поставки" },
                            products
                            );

                        MessageBox.Show("Выполнено", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
        }
        private void WordDocMenuItem_Click(object sender, EventArgs e)
        {
            using (var dialog = new SaveFileDialog { Filter = "doc|*.docx" })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        List<ProductViewModel> products = _productLogic.Read(null);
                        List<Vendor> vendors = _vendorStorage.GetFullList();

                        var data = new List<TestData>();
                        vendors.ForEach(v => data.Add(new TestData()
                        { name = v.VendorName, value = products.Where(product => product.Vendor.Equals(v.VendorName)).Count() }));

                        wordChartComponent.ReportSaveGistogram(dialog.FileName, "Отчет поставщиков по продукции", "Гистограмма", LocationLegend.BottomLeft, data);

                        MessageBox.Show("Выполнено", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.A))
            {
                AddMenuItem_Click(null, null);
                return true;
            }
            if (keyData == (Keys.Control | Keys.U))
            {
                UpdateMenuItem_Click(null, null);
                return true;
            }
            if(keyData == (Keys.Control | Keys.D))
            {
                DeleteMenuItem_Click(null, null);
                return true;
            }
            if(keyData == (Keys.Control | Keys.S))
            {
                PdfDocMenuItem_Click(null, null);
                return true;
            }
            if (keyData == (Keys.Control | Keys.T))
            {
                ExcelDocMenuItem_Click(null, null);
                return true;
            }
            if (keyData == (Keys.Control | Keys.C))
            {
                WordDocMenuItem_Click(null, null);
                return true;
            }
            if(keyData == (Keys.Control | Keys.V))
            {
                VendorsForm_Click(null, null);
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void LoadData()
        {
            try
            {
                
                var list = _productLogic.Read(null);
                dataGridViewControl.Clear();
                List<DataTableColumnConfig> columns = new List<DataTableColumnConfig>() {
                    new DataTableColumnConfig { ColumnHeader = "Id", PropertyName = "Id", Visible = false, Width = 100 },
                    new DataTableColumnConfig { ColumnHeader = "Название", PropertyName = "ProductName", Visible = true, Width = 100 },
                    new DataTableColumnConfig { ColumnHeader = "Изображение", PropertyName = "Image", Visible = false, Width = 100 },
                    new DataTableColumnConfig { ColumnHeader = "Поставщик", PropertyName = "Vendor", Visible = true, Width = 100},
                    new DataTableColumnConfig { ColumnHeader = "Дата поставки", PropertyName = "DeliveryDate", Visible = true, Width = 100},};
                
                dataGridViewControl.LoadColumns(columns);
                if (list != null)
                {
                    foreach(var p in list)
                    {
                        dataGridViewControl.AddRow(p);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
