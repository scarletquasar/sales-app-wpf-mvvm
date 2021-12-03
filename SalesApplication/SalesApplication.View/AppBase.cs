using Microsoft.EntityFrameworkCore;
using SalesApplication.Abstractions;
using SalesApplication.Database;
using SalesApplication.Domain.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesApplication.View
{
    public class AppBase
    {
        private readonly GeneralContext _context;
        public readonly IRepository<Customer> CustomerRepositoryService;
        public readonly IRepository<Sale> SaleRepositoryService;
        public readonly IRepository<Product> ProductRepositoryService;

        public AppBase(
            GeneralContext context,
            IRepository<Customer> customerRepository,
            IRepository<Sale> saleRepository,
            IRepository<Product> productRepository
        )
        {
            _context = context;
            CustomerRepositoryService = customerRepository;
            SaleRepositoryService = saleRepository;
            ProductRepositoryService = productRepository;
        }

        public AppBase ExecuteMigration()
        {
            _context.Database.Migrate();
            return this;
        }
    }
}
