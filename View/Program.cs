using Database;
using Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unity;
using Unity.Lifetime;

namespace View
{
    internal static class Program
    {
        private static IUnityContainer container = null;
        public static IUnityContainer Container
        {
            get
            {
                if (container == null)
                {
                    container = BuildUnityContainer();
                }
                return container;
            }
        }
        private static IUnityContainer BuildUnityContainer()
        {
            var currentContainer = new UnityContainer();
            
            currentContainer.RegisterType<ProductLogic>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<ProductStorage>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<VendorStorage>(new HierarchicalLifetimeManager());

            return currentContainer;
        }
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        ///
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(Container.Resolve<MainFormPlugin>());
        }
    }
}
