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
        public IReadOnlyCollection<SoldProduct> Products { get; private set; }
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
            Products = new List<SoldProduct>();
        }
        public Sale(
            int customerId,
            IUnitOfWork<Sale, Product> saleProductUnitOfWork,
            IRepository<Customer> customerRepository)
        {
            _saleProductUnitOfWork = saleProductUnitOfWork;
            _customerRepository = customerRepository;
            CustomerId = customerId;
            Products = new List<SoldProduct>();
        }

        public async Task<SoldProduct> TryAddProduct(int productId, int quantity)
        {
            SoldProduct operationSoldProduct = await SellProduct(productId, quantity);

            List<SoldProduct> products = (List<SoldProduct>)Products;
            products.Add(operationSoldProduct);
            Products = products;

            CalculateTotalPrice();
            return operationSoldProduct;
        }

        public async Task<SoldProduct> SellProduct(int productId, int productQuantity)
        {
            SoldProduct soldProduct = new();
            Product product = (await _saleProductUnitOfWork.Type2Repository.Search(x => x.Id.Equals(productId))).FirstOrDefault();
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

        public async Task Persist()
        {
            Customer targetCustomer = (await _customerRepository.Search(x => x.Id.Equals(CustomerId))).FirstOrDefault();

            if (targetCustomer == null)
                throw new ArgumentException($"Não foi possível executar a venda. O cliente de ID {CustomerId} não existe ou não pôde ser recuperado.");

            await _saleProductUnitOfWork.BeginTransaction();

            try
            {
                CreatedAt = DateTime.Now;
                await _saleProductUnitOfWork.Type1Repository.Add(this);

                foreach (SoldProduct soldProduct in Products)
                {
                    await _saleProductUnitOfWork.Type2Repository.Update(soldProduct.ProductEntity);
                }

                await _saleProductUnitOfWork.Type1Repository.Save();
                await _saleProductUnitOfWork.Commit();
            }
            catch (Exception e)
            {
                await _saleProductUnitOfWork.Rollback();
                throw new ApplicationException(e.Message);
            }
        }
    }
}
