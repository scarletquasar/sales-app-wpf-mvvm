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
using SalesApplication.View.Services;
using SalesApplication.View.ViewModels;

namespace SalesApplication.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private readonly SalesViewModel salesViewModel;
        public MainWindow()
        {
            InitializeComponent();
            ControlInversion.RegisterDependencies();
            salesViewModel = new(ControlInversion.SaleService(), ControlInversion.CustomerService());
            salesGrid.ItemsSource = salesViewModel.Sales;
        }

        public async void SearchSalesButtonAction(object sender, RoutedEventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "SearchSalesCustomerIdButton":
                    await salesViewModel.GetSales(SearchSalesTextField.Text, true);
                    break;
                case "SearchSalesIdButton":
                    await salesViewModel.GetSales(SearchSalesTextField.Text, false);
                    break;
            }

            salesGrid.ItemsSource = salesViewModel.Sales;
        }
    }
}
