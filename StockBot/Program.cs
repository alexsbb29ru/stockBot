using Autofac;
using Init.Impl;
using Init.Interfaces;
using Services.Impl;
using Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace StockBot
{
    class Program
    {
        private static IContainer Container { get; set; }

        static async Task Main(string[] args)
        {
            RegisterComponents();

            using (var scope = Container.BeginLifetimeScope())
            {
                var indexController = scope.Resolve<IndexController>();
                await indexController.Index();
            }
            Console.ReadLine();
        }

        private static void RegisterComponents()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<InitSettings>().As<IInitSettings>();
            builder.RegisterType<ExchangeService>().As<IExchangeService>();
            builder.RegisterType<SettingsService>().As<ISettingsService>();

            builder.RegisterType<IndexController>();

            Container = builder.Build();
        }
    }
}
