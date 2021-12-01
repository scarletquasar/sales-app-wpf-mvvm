using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesApplication.Domain.Business
{
    public class Product
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
        public Product(
            string description,
            double price,
            int initialStock
        )
        {
            this.Description = description;
            this.Price = price;
            this.Stock = initialStock;
        }
        public async Task<Customer> Persist()
        {
            //TODO: Implementar funcionalidade de persistência no banco de dados
            return null;
        }
    }
}
