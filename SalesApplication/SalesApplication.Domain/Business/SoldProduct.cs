using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesApplication.Domain.Business
{
    public class SoldProduct
    {
        public int Id { get; set; }
        public Sale SaleEntity { get; set; }
        public int SaleId { get; set; }
        public double TotalPrice { get; set; }
        public Product ProductEntity { get; set; }
        public int ProductId { get; set; }
        public int ProductQuantity { get; set; }
        public async Task<SoldProduct> GenerateValid()
        {
            //TODO: Implementar funcionalidade de validação e geração de produto vendido
            return null;
        }
    }
}
