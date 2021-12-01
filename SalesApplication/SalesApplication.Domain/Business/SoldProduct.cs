using SalesApplication.Data.Repositories;
using SalesApplication.Domain.Exceptions;
using SalesApplication.Domain.Hardcodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesApplication.Domain.Business
{
    public class SoldProduct
    {
        private readonly IRepository<Sale> _saleRepository;
        private readonly IRepository<SoldProduct> _soldProductRepository;
        public int Id { get; set; }
        public Sale SaleEntity { get; set; }
        public int SaleId { get; set; }
        public double TotalPrice { get; set; }
        public Product ProductEntity { get; set; }
        public int ProductId { get; set; }
        public int ProductQuantity { get; set; }
        public static async Task<SoldProduct> GenerateValid(
            int productId,
            int productQuantity,
            IRepository<Product> productRepository
        )
        {
            SoldProduct soldProduct = new();
            //Verifica se o produto existe no banco de dados
            Product product = (await productRepository.Search(x => x.Id == productId)).FirstOrDefault();
            if (product.Description is null)
            {
                throw new EntityNotFoundException(ExceptionTexts.EntityNotFound(productId.ToString()));
            }

            //Adiciona o produto ao objeto de produto vendido caso tenha estoque suficiente
            if (product.Stock < productQuantity)
            {
                throw new OperationNotValidException(ExceptionTexts.NoStockAvailable(product.Description));
            }

            soldProduct.ProductId = product.Id;
            soldProduct.ProductQuantity = productQuantity;
            product.Stock -= productQuantity;

            return soldProduct;
        }
    }
}
