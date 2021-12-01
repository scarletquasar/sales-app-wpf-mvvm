using System;
using System.Linq;
using System.Threading.Tasks;
using SalesApplication.Data.Repositories;
using SalesApplication.Data.Responses;

namespace SalesApplication.Domain.Business
{
    public class Customer
    {
        private readonly IRepository<Customer> _customerRepository;
        public int Id { get; set; }
        public string Name { get; set; }
        public Customer(string name, IRepository<Customer> customerRepository)
        {
            this._customerRepository = customerRepository;
            this.Name = name;
        }
        public async Task<ActionResponse> Persist()
        {
            var result = await _customerRepository.Add(this);
            return result;
        }
        public async Task<bool> Exists(int customerId)
        {
            var result = (await _customerRepository.Search(x => x.Id == customerId)).FirstOrDefault();
            return result.Name != null;
        }
    }
}
