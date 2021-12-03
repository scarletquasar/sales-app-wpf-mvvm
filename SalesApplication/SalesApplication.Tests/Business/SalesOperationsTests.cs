using System;
using Xunit;
using SalesApplication.Domain.Business;
using System.Collections.Generic;
using System.Threading.Tasks;
using SalesApplication.Abstractions;
using SalesApplication.Data.Repositories;
using SalesApplication.Database;
using SalesApplication.Data.Responses;
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
            Customer recordedCustomer = (Customer)((ActionResponse)await customer.Persist()).Result;

            IRepository<Product> productRepository = new Repository<Product>(new GeneralContext(ContextOptions.InMemory()));
            Product product = new("Produto de teste", 10.0, 9999, productRepository);

            IActionResponse operationResult = await product.Persist();
            Product recordedProduct = (Product)((ActionResponse)operationResult).Result;

            IRepository<Sale> saleRepository = new Repository<Sale>(new GeneralContext(ContextOptions.InMemory()));
            var sale = new Sale
            (
                recordedCustomer.Id,
                saleRepository,
                productRepository,
                new Repository<SoldProduct>(new GeneralContext(ContextOptions.Postgres()))
            );

            var addProductToSale = await sale.TryAddProduct(recordedProduct.Id, recordedProduct.Stock);
            var saleFinish = (ActionResponse)await sale.Persist();

            
            int recordedSaleId = ((Sale)saleFinish.Result).Id;

            var saleFetch = (await saleRepository.Search(x => x.Id == recordedSaleId)).FirstOrDefault();
            int fetchedSaleId = saleFetch.Id;

            //Asserts
            Assert.True(saleFetch.Products.Count == 1);
            Assert.True(recordedSaleId == fetchedSaleId);
            Assert.True(saleFinish.Success);
        }
    }
}
