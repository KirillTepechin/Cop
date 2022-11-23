using Plugin.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Plugin
{
    public interface IPluginsConvention
    {
        /// <summary>
        /// Название плагина
        /// </summary>
        string PluginName { get; }
        /// <summary>
        /// Получение контрола для вывода набора данных
        /// </summary>
        UserControl GetControl { get; }
        /// <summary>
        /// Получение элемента, выбранного в контроле
        /// </summary>
        PluginsConventionElement GetElement { get; }
        /// <summary>
        /// Получение формы для создания/редактирования объекта
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        Form GetForm(PluginsConventionElement element);
        /// <summary>
        /// Удаление элемента
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        bool DeleteElement(PluginsConventionElement element);
        /// Обновление набора данных в контроле
        /// </summary>
        void ReloadData();
        /// <summary>
        /// Создание простого документа
        /// </summary>
        /// <param name="saveDocument"></param>
        /// <returns></returns>
        bool CreateSimpleDocument(PluginsConventionSaveDocument saveDocument);
        /// <summary>
        /// Создание простого документа
        /// </summary>
        /// <param name="saveDocument"></param>
        /// <returns></returns>
        bool CreateTableDocument(PluginsConventionSaveDocument saveDocument);
        /// <summary>
        /// Создание документа с диаграммой
        /// </summary>
        /// <param name="saveDocument"></param>
        /// <returns></returns>
        bool CreateChartDocument(PluginsConventionSaveDocument saveDocument);
    }
}
