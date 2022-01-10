using SalesApplication.Abstractions;
using SalesApplication.Domain.Exceptions;
using SalesApplication.Domain.Hardcodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesApplication.Domain.Business
{
    public class Product
    {
        private readonly IRepository<Product> _productRepository;
        public int Id { get; set; }
        public string Description { get; private set; }
        public double Price { get; set; }
        public int Stock { get; set; }
        private Product() { }
        public Product(
            string description,
            double price,
            int initialStock,
            IRepository<Product> productRepository)
        {
            if (string.IsNullOrWhiteSpace(description)) 
                throw new OperationNotValidException(ExceptionTexts.ArgumentNotValid());

            Description = description;
            Price = price;
            Stock = initialStock;
            _productRepository = productRepository;
        }
        public async Task Persist() => await _productRepository.Add(this);
        public async Task<bool> Exists(int productId) =>
            (await _productRepository.Search(x => x.Id == productId)).FirstOrDefault() != null;
    }
}
