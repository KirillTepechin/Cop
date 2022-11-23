using Contracts;
using Database;
using Database.Model;
using Logic;
using NonVisualLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsControlLibrary;

namespace Plugin
{
    public partial class AddProductForm : Form
    {
        private VendorStorage _vendorStorage;
        private ProductLogic _productLogic;
        public int Id { set { id = value; } }
        private int? id;
        public AddProductForm(VendorStorage vendorStorage, ProductLogic productLogic)
        {
            InitializeComponent();
            _vendorStorage = vendorStorage;
            _productLogic = productLogic;
            textBoxControl.Template = "^(?:[012]?[0-9]|3[01])[./-](?:0?[1-9]|1[0-2])[./-](?:[0-9]{2}){1,2}$";
            textBoxControl.CreateToolTip("12.12.2012");
            LoadData();
            
        }
        private void LoadData()
        {
            var vendors = _vendorStorage.GetFullList();
            foreach (var v in vendors)
            {
                comboBoxControl.AddItem(v.VendorName);
            }
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            
            try
            {
                if (string.IsNullOrEmpty(textBoxImage.Text) || string.IsNullOrEmpty(textBoxName.Text) ||
                    string.IsNullOrEmpty(textBoxControl.Value) || string.IsNullOrEmpty((string)comboBoxControl.ItemStr))
                {
                    throw new ArgumentNullException("Заполните поля");
                }
                    _productLogic.CreateOrUpdate(new ProductBindingModel
                {
                    Id=id,
                    DeliveryDate = DateTime.ParseExact(textBoxControl.Value,"dd.mm.yyyy", CultureInfo.InvariantCulture),
                    ProductName = textBoxName.Text,
                    Image = textBoxImage.Text,
                    Vendor = (string)comboBoxControl.ItemStr
                });
                DialogResult = DialogResult.OK;
                Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
            
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
        private void Fill()
        {
            var elem = _productLogic.Read(new ProductBindingModel { Id = id }).First();
            textBoxImage.Text = elem.Image;
            textBoxName.Text = elem.ProductName;
            textBoxControl.Value = elem.DeliveryDate;
            comboBoxControl.ItemStr = elem.Vendor;
        }

        private void AddProductForm_Load(object sender, EventArgs e)
        {
            if (id != null)
            {
                Fill();
            }
            else
            {
                textBoxControl.Value = "";
            }
        }

        private void buttonSelectImage_Click(object sender, EventArgs e)
        {
            string filter = "Image Files|*.jpg;*.jpeg;*.png;";
            using (var dialog = new OpenFileDialog { Filter=filter})
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        textBoxImage.Text = dialog.FileName;
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

        private void AddProductForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (id != null)
            {
                var elem = _productLogic.Read(new ProductBindingModel { Id = id }).First();
                if (elem.Image != textBoxImage.Text || elem.ProductName != textBoxName.Text ||
                    elem.DeliveryDate != textBoxControl.Value || elem.Vendor != (string)comboBoxControl.ItemStr)
                {
                    DialogResult dialogResult = MessageBox.Show("Есть несохраненые изменения, сохранить?", "Изменения", MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);
                    if (dialogResult == DialogResult.Yes)
                    {
                        buttonSave_Click(sender, e);
                    }
                    else
                    {
                        buttonCancel_Click(sender, e);
                    }
                }
            }
        }
    }
}
