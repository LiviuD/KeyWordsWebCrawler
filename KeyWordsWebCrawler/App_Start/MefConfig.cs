using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace KeyWordsWebCrawler
{
    public static class MefConfig
    {
        public static CompositionContainer Container { get; private set; }

        public static CompositionContainer Build(string filter = "*.dll")
        {
            var files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "\\bin", filter, SearchOption.AllDirectories)
                .Where(_ => !_.Replace(AppDomain.CurrentDomain.BaseDirectory + "\\bin", "").Contains(@"roslyn\"));

            var catalogAggregator = new AggregateCatalog();

            foreach (var file in files)
            {
                catalogAggregator.Catalogs.Add(new AssemblyCatalog(Assembly.LoadFrom(file)));
            }

            Container = new CompositionContainer(catalogAggregator);
            return Container;
        }
    }
}