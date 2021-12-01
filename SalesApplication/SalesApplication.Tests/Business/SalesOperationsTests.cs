using System;
using Xunit;
using SalesApplication.Domain.Business;
using SalesApplication.Data.Repositories;
using SalesApplication.Data.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SalesApplication.Tests
{
    //Todo: Implementar banco em memória para permitir testes 
    public class SalesOperationsTests
    {
        [Fact(DisplayName = "Deve criar uma venda com sucesso")]
        public async void CreateSale()
        {
            //Test Data + Fake data operations
            IRepository<Customer> customerRepository = new TestRepository<Customer>();
            Customer customer = new("Cliente de teste", customerRepository);
            Customer recordedCustomer = (Customer)(await customer.Persist()).Result;

            IRepository<Product> productRepository = new TestRepository<Product>();
            Product product = new("Produto de teste", 10.0, 9999, productRepository);
            Product recordedProduct = (Product)(await product.Persist()).Result;

            var sale = new Sale
            (
                ((Customer)recordedCustomer).Id,
                new TestRepository<Sale>(),
                new TestRepository<Product>(),
                new TestRepository<SoldProduct>()
            );

            var addProductToSale = await sale.TryAddProduct(((Product)recordedProduct).Id, ((Product)recordedProduct).Stock);
            var saleFinish = await sale.Persist();

            Assert.True(((Product)recordedProduct).Id != 0);
            //Assert.True(saleFinish.Success);
        }
    }
}
