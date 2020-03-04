using Autofac;
using Init.Impl;
using Init.Interfaces;
using System;
using Telegram.Bot;

namespace StockBot
{
    class Program
    {
        private static IContainer Container { get; set; }
        private static IInitSettings _initSetting;
        private static ITelegramBotClient _botClient;

        static void Main(string[] args)
        {
            RegisterComponents();

            using (var scope = Container.BeginLifetimeScope())
            {
                var initSettings = scope.Resolve<IInitSettings>();
                _initSetting = initSettings;
            }
            BotConfig();
            Console.WriteLine("adsfasdf");
            Console.ReadLine();
        }

        private static void RegisterComponents()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<InitSettings>().As<IInitSettings>();

            Container = builder.Build();
        }

        private static void BotConfig()
        {
            var botToken = _initSetting.GetToken();
            _botClient = new TelegramBotClient(botToken);
        }
    }
}
