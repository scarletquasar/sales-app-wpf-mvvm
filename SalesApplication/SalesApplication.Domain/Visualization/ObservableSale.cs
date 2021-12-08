﻿using SalesApplication.Abstractions;
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
        public DateTime FeitaEm { get; set; }
        public async Task Populate(int id, IRepository<Sale> saleRepository, IRepository<Customer> customerRepository)
        {
            Sale sale = (await saleRepository.Search(x => x.Id == id)).FirstOrDefault();
            if (sale != null)
            {
                Id = sale.Id;
                IdCliente = sale.CustomerId;
                NomeCliente = (await customerRepository.Search(x => x.Id == sale.CustomerId)).FirstOrDefault().Name;
                PreçoTotal = sale.TotalPrice;
                FeitaEm = sale.CreatedAt;
            }
            else
            {
                throw new EntityNotFoundException(ExceptionTexts.EntityNotFound(id.ToString()));
            }
        }
    }
}
