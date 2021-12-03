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
    public class ObservableSale
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public string NomeCliente { get; set; }
        public double PreçoTotal { get; set; }
        public async Task Populate(int id, IRepository<Sale> saleRepository)
        {
            Sale sale = (await saleRepository.Search(x => x.Id == id)).FirstOrDefault();
            if (sale != null)
            {
                Id = sale.Id;
                IdCliente = sale.CustomerId;
                NomeCliente = sale.CustomerEntity.Name;
                PreçoTotal = sale.TotalPrice;
            }
            else
            {
                throw new EntityNotFoundException(ExceptionTexts.EntityNotFound(id.ToString()));
            }
        }
    }
}
