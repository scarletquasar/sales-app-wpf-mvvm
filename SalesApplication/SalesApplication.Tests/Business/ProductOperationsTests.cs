using System;
using Xunit;
using SalesApplication.Domain.Business;
using SalesApplication.Data.Repositories;
using SalesApplication.Data.Responses;
using System.Collections.Generic;


namespace SalesApplication.Tests
{
    public class ProductOperationsTests
    {
        [Fact(DisplayName = "Deve obter sucesso ao criar um produto")]
        public async void CreateProduct()
        {
            //Test Data
            IRepository<Product> productRepository = new TestRepository<Product>();
            var product = new Product("Produto de teste", 10.0, 9999, productRepository);

            //Test Operation
            ActionResponse operationResult = await product.Persist();

            //Asserts
            Assert.NotNull(operationResult.Result);
            Assert.True(operationResult.Success);
        }
    }
}
