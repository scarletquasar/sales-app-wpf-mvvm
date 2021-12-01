using SalesApplication.Abstractions;
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
        public string Description { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
        public Product(
            string description,
            double price,
            int initialStock,
            IRepository<Product> productRepository
        )
        {
            this.Description = description;
            this.Price = price;
            this.Stock = initialStock;
            this._productRepository = productRepository;
        }
        public async Task<IActionResponse> Persist()
        {
            var result = await _productRepository.Add(this);
            return result;
        }
        public async Task<bool> Exists(int productId)
        {
            var result = (await _productRepository.Search(x => x.Id == productId)).FirstOrDefault();
            return result.Description != null;
        }
    }
}
