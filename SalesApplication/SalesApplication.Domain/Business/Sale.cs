using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesApplication.Domain.Business
{
    public class Sale
    {
        public int Id { get; set; }
        public List<SoldProduct> Products { get; set; }
        public DateTime CreatedAt { get; set; }
        public Customer CustomerEntity { get; set; }
        public int CustomerId { get; set; }
        public double TotalPrice { get; set; }
        public Sale(
            int customerId,
            double totalPrice,
            List<SoldProduct> products,
            DateTime createdAt
        )
        {
            this.CustomerId = customerId;
            this.TotalPrice = totalPrice;
            this.Products = products;
            this.CreatedAt = createdAt;
        }
        public async Task<bool> TryAddProduct()
        {
            //TODO: Implementar funcionalidade de persistência no banco de dados
            return false;
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
