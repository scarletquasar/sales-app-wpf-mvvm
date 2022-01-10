using System;
using Xunit;
using SalesApplication.Domain.Business;
using SalesApplication.Data.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using SalesApplication.Abstractions;
using SalesApplication.Database;
using System.Linq;

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

            await customer.Persist();
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

            foreach(var item in customers)
            {
                await item.Persist();
            }
            var customerList = (await customerRepository.Search()).ToList();

            //Asserts
            Assert.True(customerList.Count == 4);
        }
    }
}
