using SalesApplication.Abstractions;
using SalesApplication.Domain.Business;
using SalesApplication.Domain.Visualization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FastReport;
using System.IO;
using FastReport.Export.Html;

namespace SalesApplication.View.Models
{
    public class SalesReport
    {
        public SalesReport(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }
        private readonly IRepository<Sale> _saleRepository;
        public List<Sale> Sales { get; set; }
        public static async Task<string> ExportReportAsync(string initialDate, string finalDate, string customerId, ISaleRepository saleRepository)
        {
            SalesReport salesReport = new(saleRepository);

            if (!DateTime.TryParse(initialDate, out DateTime initDate) || !DateTime.TryParse(finalDate, out DateTime finDate))
            {
                return "";
            }

            if (!int.TryParse(customerId, out int custId))
            {
                custId = 0;
            }

            /* Se o id de cliente informado for 0, ele retornará todas as vendas registradas dentro do
             * espaço de tempo informado, caso contrário ele exibirá as vendas do cliente com ID exato,
             * caso exista.
             */
            if(custId > 0)
            {
                IEnumerable<Sale> filter = await saleRepository.SearchWithProducts(x => x.CustomerId == custId);
                salesReport.Sales = filter.Where(x => x.CreatedAt <= finDate && x.CreatedAt >= initDate).ToList();
            }
            else
            {
                salesReport.Sales = (await saleRepository.SearchWithProducts(x => x.CreatedAt <= finDate && x.CreatedAt >= initDate)).ToList();
            }

            /* Verifica se a contagem de vendas encontradas para as informações inseridas é maior do que 0,
             * caso positivo ele efetuará toda a operação de montagem do relatório e exibirá o resultado na tela,
             * caso contrário ele irá exibir uma mensagem de erro.
             */
            if (salesReport.Sales.Count > 0)
            {
                Report fastReport = new();
                fastReport.Load(new MemoryStream(Properties.Resources.salesReport));
                fastReport.RegisterData(new[] { salesReport }, nameof(SalesReport), 10);
                fastReport.GetDataSource(nameof(SalesReport)).Enabled = true;
                fastReport.Save(@"E:\salesReport.frx"); //--> Código direcionado apenas à alteração do design do relatório
                fastReport.Prepare();

                MemoryStream output = new();
                fastReport.Export(new HTMLExport(), output);
                return Encoding.ASCII.GetString(output.ToArray());
            }
            else
            {
                return "Nenhuma venda encontrada.";
            }
        }
    }
}
