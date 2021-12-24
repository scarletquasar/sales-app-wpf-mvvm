using System;
using System.Linq;
using System.Threading.Tasks;
using SalesApplication.Abstractions;

namespace SalesApplication.Domain.Business
{
    public class Customer
    {
        private readonly IRepository<Customer> _customerRepository;
        public int Id { get; set; }
        public string Name { get; set; }
        public Customer() { }
        public Customer(string name, IRepository<Customer> customerRepository)
        {
            this._customerRepository = customerRepository;
            this.Name = name;
        }
        public async Task Persist()
        {
            await _customerRepository.Add(this);
        }
        public async Task<bool> Exists(int customerId)
        {
            var result = (await _customerRepository.Search(x => x.Id == customerId)).FirstOrDefault();
            return result.Name != null;
        }
    }
}
