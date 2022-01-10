using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using SalesApplication.Abstractions;
using SalesApplication.Domain.Business;
using SalesApplication.View.Models;
using SalesApplication.View.Services;
using SalesApplication.View.ViewModels;
using SalesApplication.Domain.Abstractions;

namespace SalesApplication.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {

        private readonly ISaleRepository _reportableSaleRepository;
        public MainWindow(
            SalesRegisterViewModel salesRegisterViewModel,
            ISaleRepository reportableSaleRepository
        )
        {
            InitializeComponent();

            _reportableSaleRepository = reportableSaleRepository;
            salesRegisterViewModel.Initialize();
        }

        private async void GenerateReport(object sender, RoutedEventArgs e)
        {
            int.TryParse(SaleReportCustomerId.Text, out int customerId);

            SalesReport report = new(
                _reportableSaleRepository,
                InitialSaleReportDate.SelectedDate ?? DateTime.Now,
                FinalSaleReportDate.SelectedDate ?? DateTime.Now,
                customerId);

            string reportContent;

            try
            {
                reportContent = await report.ExportReportAsync();
            }
            catch(Exception error)
            {
                reportContent = error.Message;
            }

            SaleReportTarget.NavigateToString(reportContent);
        }
    }
}
