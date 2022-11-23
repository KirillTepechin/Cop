using Plugin;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pligin.Plugin
{
    public class PluginManager
    {
        [ImportMany(typeof(IPluginsConvention))]
        IEnumerable<IPluginsConvention> Plugins { get; set; }
        public Dictionary<string, IPluginsConvention> PluginsDictionary { get; set; }
        public PluginManager()
        {
            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new DirectoryCatalog(AppDomain.CurrentDomain.BaseDirectory));
            catalog.Catalogs.Add(new DirectoryCatalog(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins")));
            CompositionContainer container = new CompositionContainer(catalog);
            container.ComposeParts(this);

            if (Plugins.Any())
            {
                Plugins.ToList().ForEach(p =>
                {
                    if (!PluginsDictionary.Keys.Contains(p.PluginName))
                    {
                        PluginsDictionary.Add(p.PluginName, p);
                    }
                });
            }

        }

    }
}
