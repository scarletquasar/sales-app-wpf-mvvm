using System;
using Xunit;
using SalesApplication.Domain.Business;
using System.Collections.Generic;
using System.Threading.Tasks;
using SalesApplication.Abstractions;
using SalesApplication.Data.Repositories;
using SalesApplication.Database;

namespace SalesApplication.Tests
{
    //Todo: Implementar banco em memória para permitir testes 
    public class SalesOperationsTests
    {
        [Fact(DisplayName = "Deve criar uma venda com sucesso")]
        public async void CreateSale()
        {
            //Test Data + Fake data operations
            IRepository<Customer> customerRepository = new Repository<Customer>(new GeneralContext(ContextOptions.InMemory()));
            Customer customer = new("Cliente de teste", customerRepository);
            Customer recordedCustomer = (Customer)(await customer.Persist());

            IRepository<Product> productRepository = new Repository<Product>(new GeneralContext(ContextOptions.InMemory()));
            Product product = new("Produto de teste", 10.0, 9999, productRepository);
            Product recordedProduct = (Product)(await product.Persist());

            var sale = new Sale
            (
                recordedCustomer.Id,
                new Repository<Sale>(new GeneralContext(ContextOptions.InMemory())),
                new Repository<Product>(new GeneralContext(ContextOptions.InMemory())),
                new Repository<SoldProduct>(new GeneralContext(ContextOptions.InMemory()))
            );

            var addProductToSale = await sale.TryAddProduct(recordedProduct.Id, recordedProduct.Stock);
            var saleFinish = await sale.Persist();

            Assert.True(((Product)recordedProduct).Id != 0);
            //Assert.True(saleFinish.Success);
        }
    }
}
