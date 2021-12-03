using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Microsoft.EntityFrameworkCore;
using SalesApplication.Abstractions;
using SalesApplication.Data.Repositories;
using SalesApplication.Database;
using SalesApplication.Domain.Business;

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
        public static ILifetimeScope GetChildContainer() => _scope.BeginLifetimeScope();
        public static void RegisterDependencies()
        {
            _builder.RegisterType<GeneralContext>().WithParameter(new TypedParameter(typeof(DbContextOptions<GeneralContext>), ContextOptions.Postgres())).InstancePerDependency();
            _builder.RegisterType<Repository<Customer>>().As<IRepository<Customer>>().WithParameter("context", new GeneralContext(ContextOptions.Postgres())).InstancePerDependency();
            _builder.RegisterType<Repository<Product>>().As<IRepository<Product>>().WithParameter("context", new GeneralContext(ContextOptions.Postgres())).InstancePerDependency();
            _builder.RegisterType<Repository<Sale>>().As<IRepository<Sale>>().WithParameter("context", new GeneralContext(ContextOptions.Postgres())).InstancePerDependency();
            _builder.RegisterType<Repository<SoldProduct>>().As<IRepository<SoldProduct>>().WithParameter("context", new GeneralContext(ContextOptions.Postgres())).InstancePerDependency();

            _builder.RegisterType<AppBase>().AsSelf().InstancePerDependency();

            _scope = _builder.Build();
        }

        public static IRepository<Product> ProductService()
        {
            return _scope.Resolve<IRepository<Product>>();
        }

        public static IRepository<Customer> CustomerService()
        {
            return _scope.Resolve<IRepository<Customer>>();
        }

        public static IRepository<Sale> SaleService()
        {
            return _scope.Resolve<IRepository<Sale>>();
        }

        public static IRepository<SoldProduct> SoldProductService()
        {
            return _scope.Resolve<IRepository<SoldProduct>>();
        }
    }
}
