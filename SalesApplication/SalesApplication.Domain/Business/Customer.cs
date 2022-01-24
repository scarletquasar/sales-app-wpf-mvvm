using System;
using System.Linq;
using System.Threading.Tasks;
using SalesApplication.Abstractions;
using SalesApplication.Domain.Exceptions;
using SalesApplication.Domain.Hardcodes;

namespace SalesApplication.Domain.Business
{
    public class Customer
    {
        private readonly IRepository<Customer> _customerRepository;
        public int Id { get; set; }
        public string Name { get; private set; }
        private Customer() { }
        public Customer(string name, IRepository<Customer> customerRepository)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new OperationNotValidException("O nome do cliente não pode ser vazio");

            _customerRepository = customerRepository;
            Name = name;
        }
        public async Task Persist()
        {
            await _customerRepository.Add(this);
        }
    }
}
