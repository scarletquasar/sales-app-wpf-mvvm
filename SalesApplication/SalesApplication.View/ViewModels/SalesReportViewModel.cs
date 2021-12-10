using SalesApplication.Abstractions;
using SalesApplication.Domain.Business;
using SalesApplication.View.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SalesApplication.View.ViewModels
{
    public class SalesReportViewModel
    {
        public SalesReportViewModel(IRepository<Sale> salesRepository)
        {
            _saleRepository = salesRepository;
        }
        private IRepository<Sale> _saleRepository;
    }
}
