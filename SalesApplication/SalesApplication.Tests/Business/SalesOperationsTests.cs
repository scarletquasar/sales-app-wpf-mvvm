using System;
using Xunit;
using SalesApplication.Domain.Business;
using SalesApplication.Data.Repositories;
using SalesApplication.Data.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SalesApplication.Tests.Business
{
    public class SalesOperationsTests
    {
        public async void CreateSale()
        {
            //Test Data + Fake data operations
            IRepository<Customer> customerRepository = new TestRepository<Customer>();
            var customer = new Customer("Cliente de teste", customerRepository);
            var recordedCustomer = (await customer.Persist()).Result;

            IRepository<Product> productRepository = new TestRepository<Product>();
            var product = new Product("Produto de teste", 10.0, 9999, productRepository);
            var recordedProduct = (await product.Persist()).Result;

            var sale = new Sale
            (
                ((Customer)recordedCustomer).Id,
                new TestRepository<Sale>(),
                new TestRepository<Product>(),
                new TestRepository<SoldProduct>()
            );

            var addProductToSale = await sale.TryAddProduct(((Product)recordedProduct).Id, ((Product)recordedProduct).Stock);
            var saleFinish = await sale.Persist();

            Assert.True(saleFinish.Success);
        }
    }
}
