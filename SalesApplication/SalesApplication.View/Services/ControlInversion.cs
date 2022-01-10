using Autofac;
using Microsoft.EntityFrameworkCore;
using SalesApplication.Abstractions;
using SalesApplication.Data.Repositories;
using SalesApplication.Database;
using SalesApplication.Domain.Business;
using SalesApplication.View.Abstractions;
using SalesApplication.View.ViewModels;
using SalesApplication.Domain.Abstractions;
using SalesApplication.Data.UnityOfWork;

namespace SalesApplication.View.Services
{
    public static class ControlInversion
    {
        private static readonly ContainerBuilder _builder;
        private static ILifetimeScope _scope;
        static ControlInversion()
        {
            _builder ??= new ContainerBuilder();
        }
        public static ILifetimeScope GetChildContainer()
        {
            return _scope.BeginLifetimeScope();
        }

        public static void RegisterDependencies()
        {
            /* Geral */
            _builder.RegisterType<GeneralContext>()
                .WithParameter(new TypedParameter(typeof(DbContextOptions<GeneralContext>), ContextOptions.Postgres()))
                .InstancePerDependency();
            _builder.RegisterGeneric(typeof(Repository<>))
                .As(typeof(IRepository<>))
                .InstancePerDependency();
            _builder.RegisterType<SaleProductUnityOfWork>()
                .As(typeof(IUnitOfWork<Sale, Product>))
                .InstancePerDependency();
            _builder.RegisterType<DialogService>()
                .As<IDialogService>()
                .InstancePerDependency();
            _builder.RegisterType<MainWindow>()
                .AsSelf()
                .InstancePerDependency();

            /* Repositórios */
            _builder.RegisterType<SaleRepository>().As<ISaleRepository>().InstancePerDependency();

            /* ViewModels */
            _builder.RegisterType<SalesViewModel>().AsSelf().SingleInstance();
            _builder.RegisterType<ProductsViewModel>().AsSelf().SingleInstance();
            _builder.RegisterType<CustomersViewModel>().AsSelf().SingleInstance();
            _builder.RegisterType<SalesRegisterViewModel>().AsSelf().SingleInstance();
            _builder.RegisterType<ProductsRegisterViewModel>().AsSelf().SingleInstance();
            _builder.RegisterType<CustomersRegisterViewModel>().AsSelf().SingleInstance();

            _scope = _builder.Build();
        }

        public static T ResolveDependency<T>()
        {
            return GetChildContainer().Resolve<T>();
        }
    }
}
