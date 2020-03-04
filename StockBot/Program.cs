using Autofac;
using Init.Impl;
using Init.Interfaces;
using System;

namespace StockBot
{
    class Program
    {
        private IInitSettings _initSetting;
        public Program(IInitSettings initSettings)
        {
            _initSetting = initSettings;
        }

        static void Main(string[] args)
        {
            RegisterComponents();

            Console.WriteLine("adsfasdf");
            Console.ReadLine();
        }
        
        private static void RegisterComponents()
        {
            var builder = new ContainerBuilder();

            builder.RegisterInstance(new InitSettings()).As<IInitSettings>();
        }
    }
}
