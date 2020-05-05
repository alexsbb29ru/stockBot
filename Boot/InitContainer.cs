using Autofac;
using DAL.UOW.Impl;
using Init.Interfaces.DAL;

namespace Boot
{
    public class InitContainer
    {
        public static ContainerBuilder GetBuilder()
        {
            var builder = new ContainerBuilder();

            builder.RegisterGeneric(typeof(Repository<,>))
                .As(typeof(IRepository<,>))
                .InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(UnitOfWork<,>))
                .As(typeof(IUnitOfWork<,>))
                .InstancePerLifetimeScope();

            return builder;
        }
    }
}