using SalesApplication.Data.Repositories;
using SalesApplication.Data.Responses;
using SalesApplication.Domain.Exceptions;
using SalesApplication.Domain.Hardcodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesApplication.Domain.Business
{
    public class Sale
    {
        private readonly IRepository<Sale> _saleRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<SoldProduct> _soldProductRepository;
        public int Id { get; set; }
        public List<SoldProduct> Products { get; set; }
        public DateTime CreatedAt { get; set; }
        public Customer CustomerEntity { get; set; }
        public int CustomerId { get; set; }
        public double TotalPrice { get; set; }
        public Sale(
            int customerId,
            double totalPrice,
            DateTime createdAt,
            IRepository<Sale> saleRepository,
            IRepository<Product> productRepository,
            IRepository<Customer> customerRepository,
            IRepository<SoldProduct> soldProductRepository
        )
        {
            this.CustomerId = customerId;
            this.TotalPrice = totalPrice;
            this.CreatedAt = createdAt;
            this._saleRepository = saleRepository;
            this._productRepository = productRepository;
            this._customerRepository = customerRepository;
            this._soldProductRepository = soldProductRepository;
        }
        public async Task<SoldProduct> TryAddProduct(int productId, int quantity)
        {
            SoldProduct soldProduct = await SoldProduct.GenerateValid(productId, quantity, _productRepository);
            Products.Add(soldProduct);
            return soldProduct;
        }
        public async Task<ActionResponse> Persist()
        {
            var result = await _saleRepository.Add(this);

            Products.ForEach(x => {
                x.SaleEntity = this;
                x.SaleId = this.Id;
            });

            await _soldProductRepository.Save();
            return result;
        }
        public async Task<bool> Exists(int saleId)
        {
            var result = (await _saleRepository.Search(x => x.Id == saleId)).FirstOrDefault();
            return result.CustomerEntity != null;
        }
    }
}
