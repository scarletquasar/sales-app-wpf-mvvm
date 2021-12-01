using System;
using Xunit;
using SalesApplication.Domain.Business;
using SalesApplication.Data.Repositories;
using SalesApplication.Data.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;
using SalesApplication.Abstractions;
using SalesApplication.Database;

namespace SalesApplication.Tests
{
    public class CustomerOperationsTests
    {
        [Fact(DisplayName = "Deve obter sucesso ao criar um cliente")]
        public async void CreateCustomer()
        {
            //Test Data
            IRepository<Customer> customerRepository = new Repository<Customer>(new GeneralContext(ContextOptions.InMemory()));
            var customer = new Customer("Cliente de teste", customerRepository);

            //Test Operation
            IActionResponse operationResult = await customer.Persist();

            //Asserts
            Assert.NotNull(((ActionResponse)operationResult).Result);
            Assert.True(((ActionResponse)operationResult).Success);
        }

        [Fact(DisplayName = "Deve obter com sucesso uma lista de clientes existentes")]
        public async void FetchCustomers()
        {
            //Test Data
            IRepository<Customer> customerRepository = new Repository<Customer>(new GeneralContext(ContextOptions.InMemory()));
            var customers = new List<Customer>() {
                new Customer("Cliente de teste", customerRepository),
                new Customer("Cliente de teste", customerRepository),
                new Customer("Cliente de teste", customerRepository),
                new Customer("Cliente de teste", customerRepository)
            };

            //Test Operation

            await Task.Run(() => customers.ForEach(async x => await x.Persist()));
            var customerList = (List<Customer>)await customerRepository.Search();

            //Asserts
            Assert.True(customerList.Count == 4);
        }
    }
}
