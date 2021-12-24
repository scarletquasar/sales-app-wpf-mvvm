using System;
using Xunit;
using SalesApplication.Domain.Business;
using System.Collections.Generic;
using System.Threading.Tasks;
using SalesApplication.Abstractions;
using SalesApplication.Data.Repositories;
using SalesApplication.Database;
using System.Linq;

namespace SalesApplication.Tests
{
    public class SalesOperationsTests
    {
        [Fact(DisplayName = "Deve criar uma venda com sucesso")]
        public async void CreateSale()
        {
            //Test Data + Fake data operations
            IRepository<Customer> customerRepository = new Repository<Customer>(new GeneralContext(ContextOptions.InMemory()));
            Customer customer = new("Cliente de teste", customerRepository);

            IRepository<Product> productRepository = new Repository<Product>(new GeneralContext(ContextOptions.InMemory()));
            Product product = new("Produto de teste", 10.0, 9999, productRepository);

            await product.Persist();

            IRepository<Sale> saleRepository = new Repository<Sale>(new GeneralContext(ContextOptions.InMemory()));
            var sale = new Sale
            (
                customer.Id,
                saleRepository,
                productRepository,
                new Repository<SoldProduct>(new GeneralContext(ContextOptions.Postgres()))
            );

            var addProductToSale = await sale.TryAddProduct(product.Id, product.Stock);
            await sale.Persist();

            //Asserts
            Assert.True((await saleRepository.Search(x => x == sale)).FirstOrDefault().Products.Count == 1);

        }
    }
}
