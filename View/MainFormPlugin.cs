using DocumentFormat.OpenXml.Office2010.CustomUI;
using Pligin.Plugin;
using Plugin;
using Plugin.Plugin;
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
    public partial class MainFormPlugin : Form
    {
        private readonly Dictionary<string, IPluginsConvention> _plugins;
        private string _selectedPlugin;
        public MainFormPlugin()
        {
            InitializeComponent();
            _plugins = LoadPlugins();
            _selectedPlugin = string.Empty;
        }
        private Dictionary<string, IPluginsConvention> LoadPlugins()
        {
            PluginManager manager = new PluginManager();
            var dictionary = manager.PluginsDictionary;
            ToolStripItem[] toolsStripItems = new ToolStripItem[dictionary.Count];
            int i = 0;
            foreach (var plugin in dictionary)
            {
                ToolStripMenuItem itemMenu = new ToolStripMenuItem();
                itemMenu.Text = plugin.Value.PluginName;
                itemMenu.Click += (sender, e) =>
                {
                    _selectedPlugin = plugin.Value.PluginName;
                    panelControl.Controls.Clear();
                    panelControl.Controls.Add(_plugins[_selectedPlugin].GetControl);
                    panelControl.Controls[0].Dock = DockStyle.Fill;
                };
                toolsStripItems[i] = itemMenu;
                i++;

            }
            var menuStrip = new MenuStrip();
            menuStrip.Items.AddRange(toolsStripItems);
            panelControl.Controls.Add(menuStrip);
            return dictionary;

        }

        private void MainFormPlugin_KeyDown(object sender, KeyEventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedPlugin) || !_plugins.ContainsKey(_selectedPlugin))
            {
                return;
            }
            if (!e.Control)
            {
                return;
            }
            switch (e.KeyCode)
            {
                case System.Windows.Forms.Keys.A:
                    AddNewElement();
                    break;
                case System.Windows.Forms.Keys.U:
                    UpdateElement();
                    break;
                case Keys.D:
                    DeleteElement();
                    break;
                case Keys.S:
                    CreateSimpleDoc();
                    break;
                case Keys.T:
                    CreateTableDoc();
                    break;
                case Keys.C:
                    CreateChartDoc();
                    break;
            }
        }
        private void AddNewElement()
        {
            var form = _plugins[_selectedPlugin].GetForm(null);
            if (form != null && form.ShowDialog() == DialogResult.OK)
            {
                _plugins[_selectedPlugin].ReloadData();
            }
        }
        private void UpdateElement()
        {
            var element = _plugins[_selectedPlugin].GetElement;
            if (element == null)
            {
                MessageBox.Show("Нет выбранного элемента", "Ошибка",
               MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var form = _plugins[_selectedPlugin].GetForm(element);
            if (form != null && form.ShowDialog() == DialogResult.OK)
            {
                _plugins[_selectedPlugin].ReloadData();
            }
        }
        private void DeleteElement()
        {
            if (MessageBox.Show("Удалить выбранный элемент", "Удаление",
           MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }
            var element = _plugins[_selectedPlugin].GetElement;
            if (element == null)
            {
                MessageBox.Show("Нет выбранного элемента", "Ошибка",
               MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_plugins[_selectedPlugin].DeleteElement(element))
            {
                _plugins[_selectedPlugin].ReloadData();
            }
        }
        private void CreateSimpleDoc()
        {
            if (_plugins[_selectedPlugin].CreateSimpleDocument(new PluginsConventionSaveDocument()))
            {
                MessageBox.Show("Документ сохранен", "Создание документа",
               MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Ошибка при создании документа", "Ошибка",
               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CreateTableDoc()
        {
            // TODO узнать где сохранять
            if (_plugins[_selectedPlugin].CreateTableDocument(new
           PluginsConventionSaveDocument()))
            {
                MessageBox.Show("Документ сохранен", "Создание документа",
               MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Ошибка при создании документа", "Ошибка",
               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CreateChartDoc()
        {
            // TODO узнать где сохранять
            if (_plugins[_selectedPlugin].CreateChartDocument(new
           PluginsConventionSaveDocument()))
            {
                MessageBox.Show("Документ сохранен", "Создание документа",
               MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Ошибка при создании документа", "Ошибка",
               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void AddElementToolStripMenuItem_Click(object sender, EventArgs e) =>
       AddNewElement();
        private void UpdElementToolStripMenuItem_Click(object sender, EventArgs e) =>
       UpdateElement();
        private void DelElementToolStripMenuItem_Click(object sender, EventArgs e) =>
       DeleteElement();
        private void SimpleDocToolStripMenuItem_Click(object sender, EventArgs e) =>
       CreateSimpleDoc();
        private void TableDocToolStripMenuItem_Click(object sender, EventArgs e) =>
       CreateTableDoc();
        private void ChartDocToolStripMenuItem_Click(object sender, EventArgs e) =>
       CreateChartDoc();
    }
}
