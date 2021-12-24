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
            SaleReportTarget.NavigateToString(await SalesReport.ExportReportAsync(
                InitialSaleReportDate.Text,
                FinalSaleReportDate.Text,
                SaleReportCustomerId.Text,
                _reportableSaleRepository));
        }
    }
}
