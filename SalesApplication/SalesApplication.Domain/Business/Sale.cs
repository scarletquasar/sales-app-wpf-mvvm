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
    public class Sale
    {
        private readonly IRepository<Sale> _saleRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<SoldProduct> _soldProductRepository;
        public int Id { get; set; }
        public List<SoldProduct> Products { get; set; }
        public DateTime CreatedAt { get; set; }
        public Customer CustomerEntity { get; set; }
        public int CustomerId { get; set; }
        public double TotalPrice { get; set; }
        public Sale() { }
        public Sale(
            int customerId,
            IRepository<Sale> saleRepository,
            IRepository<Product> productRepository,
            IRepository<SoldProduct> soldProductRepository
        )
        {
            this.CustomerId = customerId;
            this._saleRepository = saleRepository;
            this._productRepository = productRepository;
            this._soldProductRepository = soldProductRepository;
            Products = new();
        }
        public async Task<SoldProduct> TryAddProduct(int productId, int quantity)
        {
            SoldProduct soldProduct = await SellProduct(productId, quantity);
            Products.Add(soldProduct);
            return soldProduct;
        }
        public async Task<SoldProduct> SellProduct(int productId, int productQuantity)
        {
            SoldProduct soldProduct = new();
            //Verifica se o produto existe no banco de dados
            Product product = (await _productRepository.Search(x => x.Id == productId)).FirstOrDefault();
            if (product == null)
            {
                throw new EntityNotFoundException(ExceptionTexts.EntityNotFound(productId.ToString()));
            }

            //Adiciona o produto ao objeto de produto vendido caso tenha estoque suficiente
            if (product.Stock < productQuantity)
            {
                throw new OperationNotValidException(ExceptionTexts.NoStockAvailable(product.Description));
            }

            soldProduct.TotalPrice = product.Price * productQuantity;
            soldProduct.ProductId = product.Id;
            soldProduct.ProductQuantity = productQuantity;
            product.Stock -= productQuantity;

            return soldProduct;
        }
        public async Task<IActionResponse> Persist()
        {
            //Verifica se o cliente existe
            //Customer customer = (await _customerRepository.Search(x => x.Id == CustomerId)).FirstOrDefault();
            //if (customer == null)
            //{
            //    throw new EntityNotFoundException(ExceptionTexts.EntityNotFound(CustomerId.ToString()));
            //}

            //CustomerEntity = customer;

            await _productRepository.Save();

            CreatedAt = DateTime.Now;
            TotalPrice = Products.Sum(x => x.TotalPrice);
            var result = await _saleRepository.Add(this);

            Products.ForEach(x => {
                x.SaleEntity = this;
                x.SaleId = this.Id;
            });

            foreach (SoldProduct product in Products)
            {
                await _soldProductRepository.Add(product);
            }

            return result;
        }
        public async Task<bool> Exists(int saleId)
        {
            var result = (await _saleRepository.Search(x => x.Id == saleId)).FirstOrDefault();
            return result.CustomerEntity != null;
        }
    }
}
