using SalesApplication.Abstractions;
using SalesApplication.Domain.Business;
using SalesApplication.View.Visualization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FastReport;
using System.IO;
using FastReport.Export.Html;
using SalesApplication.Domain.Abstractions;

namespace SalesApplication.View.Models
{
    public class SalesReport
    {
        public SalesReport(ISaleRepository saleRepository, DateTime initialDate, DateTime finalDate, int customerId)
        {
            if (initialDate > finalDate) 
                throw new ArgumentException("A data inicial não pode ser maior que a final");

            if (initialDate > DateTime.Now)
                throw new ArgumentException("A data inicial não pode ser no futuro");

            _saleRepository = saleRepository;
            _initialDate = initialDate;
            _finalDate = finalDate;
            _customerId = customerId;
        }

        private readonly ISaleRepository _saleRepository;
        private readonly DateTime _initialDate;
        private readonly DateTime _finalDate;
        private readonly int _customerId;
        public List<Sale> Sales { get; private set; }
        public async Task<string> ExportReportAsync()
        {
            Sales = 
                (await _saleRepository.SearchWithProducts(x => x.CreatedAt <= _finalDate && x.CreatedAt >= _initialDate))
                .ToList();

            if(_customerId > 0) Sales = Sales.Where(x => x.CustomerId.Equals(_customerId)).ToList();
            if (Sales.Count > 0)
            {
                Report fastReport = new();
                fastReport.Load(new MemoryStream(Properties.Resources.salesReport));
                fastReport.RegisterData(new[] { this }, nameof(SalesReport), 10);
                fastReport.GetDataSource(nameof(SalesReport)).Enabled = true;
                //fastReport.Save(@"E:\salesReport.frx"); //--> Código direcionado apenas à alteração do design do relatório
                fastReport.Prepare();

                MemoryStream output = new();
                fastReport.Export(new HTMLExport(), output);
                return Encoding.ASCII.GetString(output.ToArray());
            }
            else
            {
                throw new Exception("Nenhuma venda encontrada");
            }
        }
    }
}
