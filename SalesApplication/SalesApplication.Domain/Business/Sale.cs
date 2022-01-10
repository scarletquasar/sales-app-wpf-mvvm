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
        private readonly IRepository<Customer> _customerRepository;
        private readonly IUnitOfWork<Sale, Product> _saleProductUnitOfWork;
        public int Id { get; set; }
        public List<SoldProduct> Products { get; set; }
        public DateTime CreatedAt { get; private set; }
        public Customer CustomerEntity { get; set; }
        public int CustomerId { get; set; }
        public double TotalPrice { get; private set; }
        private Sale() { }
        public Sale(
            IUnitOfWork<Sale, Product> saleProductUnitOfWork, 
            IRepository<Customer> customerRepository)
        {
            _saleProductUnitOfWork = saleProductUnitOfWork;
            _customerRepository = customerRepository;
            Products = new();
        }
        public Sale(
            int customerId, 
            IUnitOfWork<Sale, Product> saleProductUnitOfWork, 
            IRepository<Customer> customerRepository)
        {
            CustomerId = customerId;
            _saleProductUnitOfWork = saleProductUnitOfWork;
            _customerRepository = customerRepository;
            Products = new();
        }

        public async Task<SoldProduct> TryAddProduct(int productId, int quantity)
        {
            SoldProduct soldProduct = await SellProduct(productId, quantity);
            Products.Add(soldProduct);
            CalculateTotalPrice();
            return soldProduct;
        }

        public async Task<SoldProduct> SellProduct(int productId, int productQuantity)
        {
            SoldProduct soldProduct = new();
            Product product = (await _saleProductUnitOfWork.Type2Repository.Search(x => x.Id == productId)).FirstOrDefault();
            bool productExits = product != null;
            if (!productExits) throw new EntityNotFoundException(ExceptionTexts.EntityNotFound(productId.ToString()));

            //Dispara uma exceção caso não haja estoque suficiente do produto para realizar a operação
            if (product.Stock < productQuantity)
            {
                throw new OperationNotValidException(ExceptionTexts.NoStockAvailable(product.Description));
            } 

            soldProduct.TotalPrice = product.Price * productQuantity;
            soldProduct.ProductId = product.Id;
            soldProduct.ProductEntity = product;
            soldProduct.ProductQuantity = productQuantity;
            product.Stock -= productQuantity;

            return soldProduct;
        }

        public void CalculateTotalPrice()
        {
            TotalPrice = Products.Sum(x => x.TotalPrice);
        }

        public async Task Persist(int customerId)
        {
            Customer targetCustomer = (await _customerRepository.Search(x => x.Id == customerId)).FirstOrDefault();

            if (targetCustomer == null)
                throw new ArgumentException("Verifique as informações do cliente inseridas");

            CustomerId = customerId;

            await _saleProductUnitOfWork.BeginTransaction();
            try
            {
                CreatedAt = DateTime.Now;
                await _saleProductUnitOfWork.Type1Repository.Add(this);

                foreach (var soldProduct in Products)
                    await _saleProductUnitOfWork.Type2Repository.Update(soldProduct.ProductEntity);

                await _saleProductUnitOfWork.Type1Repository.Save();
                await _saleProductUnitOfWork.Commit();
            }
            catch (Exception e)
            {
                await _saleProductUnitOfWork.Rollback();
                throw new Exception(e.Message);
            }
        }
        public async Task<bool> Exists(int saleId)
        {
            return (await _saleProductUnitOfWork.Type1Repository.Search(x => x.Id == saleId)).FirstOrDefault() != null;
        }    
    }
}
