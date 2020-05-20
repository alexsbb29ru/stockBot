using Autofac;
using Init.Impl;
using Init.Interfaces;
using Services.Impl;
using Services.Interfaces;
using System;
using System.Threading.Tasks;
using BaseTypes;
using Boot;

namespace StockBot
{
    static class Program
    {
        private static IContainer Container { get; set; }

        static async Task Main(string[] args)
        {
            RegisterComponents();

            await using (var scope = Container.BeginLifetimeScope())
            {
                var indexController = scope.Resolve<IndexController>();
                await indexController.Index();
            }

            Console.ReadLine();
        }

        private static void RegisterComponents()
        {
            var builder = InitContainer.GetBuilder();

            builder.RegisterType<InitSettings>().As<IInitSettings>();
            builder.RegisterType<ExchangeService>().As<IExchangeService>();
            builder.RegisterType<SettingsService>().As<ISettingsService>();
            builder.RegisterType<LocalizeService>().As<ILocalizeService>();
            builder.RegisterGeneric(typeof(UserService<,>))
                .As(typeof(IUserService<,>)).InstancePerLifetimeScope();
            
            builder.RegisterType<IndexController>();
            builder.RegisterType<BaseController>();

            Container = builder.Build();
        }
    }
}