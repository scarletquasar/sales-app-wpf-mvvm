using SalesApplication.Abstractions;
using SalesApplication.Domain.Business;
using SalesApplication.Domain.Exceptions;
using SalesApplication.Domain.Hardcodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesApplication.Domain.Visualization
{
    public class ObservableCustomer
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public async Task Populate(int id, IRepository<Customer> customerRepository)
        {
            Customer customer = (await customerRepository.Search(x => x.Id == id)).FirstOrDefault();
            if (customer != null)
            {
                Id = customer.Id;
                Nome = customer.Name;
            }
            else
            {
                throw new EntityNotFoundException(ExceptionTexts.EntityNotFound(id.ToString()));
            }
        }
    }
}
