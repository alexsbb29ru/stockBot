using Autofac;
using Init.Impl;
using Init.Interfaces;
using Services.Impl;
using Services.Interfaces;
using System;

namespace StockBot
{
    class Program
    {
        private static IContainer Container { get; set; }

        static void Main(string[] args)
        {
            RegisterComponents();

            using (var scope = Container.BeginLifetimeScope())
            {
                var indexController = scope.Resolve<IndexController>();
                indexController.Index();
            }
            Console.ReadLine();
        }

        private static void RegisterComponents()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<InitSettings>().As<IInitSettings>();
            builder.RegisterType<ExchangeService>().As<IExchangeService>();
            builder.RegisterType<IndexController>();

            Container = builder.Build();
        }
    }
}
