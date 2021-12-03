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
    public class ObservableProduct
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
        public async void Populate(int id, IRepository<Product> productRepository)
        {
            Product product = (await productRepository.Search(x => x.Id == id)).FirstOrDefault();
            if (product != null)
            {
                Id = product.Id;
                Description = product.Description;
                Price = product.Price;
                Stock = product.Stock;
            }
            else
            {
                throw new EntityNotFoundException(ExceptionTexts.EntityNotFound(id.ToString()));
            }
        }
    }
}
