using System;
using System.Threading.Tasks;

namespace SalesApplication.Domain.Business
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Customer(string name)
        {
            this.Name = name;
        }
        public async Task<Customer> Persist()
        {
            //TODO: Implementar funcionalidade de persistência no banco de dados
            return null;
        }
        public async Task<bool> Exists(int customerId)
        {
            //TODO: Implementar funcionalidade de verificação
            return false;
        }
    }
}
