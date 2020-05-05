using Autofac;
using DAL.UOW.Impl;
using DAL.UOW.Interfaces;
using Init.Impl;
using Init.Interfaces;

namespace Bootstrapper
{
    public class Bootstrapper
    {
        public static ContainerBuilder InitContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType(typeof(IRepository<,>));
            builder.RegisterType(typeof(IUnitOfWork<,>));

            return builder;
        }
    }
}