using Database;
using Database.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace View
{
    public partial class VendorsForm : Form
    {
        private readonly VendorStorage _logic;

        public VendorsForm(VendorStorage logic)
        {
            _logic = logic;
           
            InitializeComponent();
        }

        private void VendorsForm_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        private void LoadData()
        {
            try
            {
                var list = _logic.GetFullList();
                if (list == null) return;
                dataGridView.DataSource = list;
                dataGridView.Columns[0].Visible = false;
                dataGridView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void InsertMenuItem_Click(object sender,EventArgs e)
        {
            int i = 0;
            string name = "Производитель";
            try
            {
                while (!(_logic.GetElement(new Vendor { VendorName = name }) is null))
                {
                    i++;
                    name = "Производитель " + i;
                }

                _logic.Insert(new Vendor { VendorName = name });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            LoadData();
        }
        private void DeleteMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 1)
            {
                if (MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(dataGridView.SelectedRows[0].Cells[0].Value);
                    try
                    {
                        _logic.Delete(new Vendor { Id = id });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    LoadData();
                }
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Insert)
            {
                InsertMenuItem_Click(null, null);
                return true;
            }
            if (keyData == Keys.Delete)
            {
                DeleteMenuItem_Click(null, null);
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
        

        private void dataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var typeName = dataGridView[e.ColumnIndex, e.RowIndex].Value as string;
            if (!string.IsNullOrEmpty(typeName))
            {
                BeginInvoke(new MethodInvoker(() =>
                {
                    try
                    {
                        var id = (int)dataGridView[0, e.RowIndex].Value;
                        _logic.Update(new Vendor { Id = id, VendorName = typeName });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    LoadData();
                }));
            }
            else
            {
                MessageBox.Show("Введена пустая строка", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            LoadData();
        }
    }
}

