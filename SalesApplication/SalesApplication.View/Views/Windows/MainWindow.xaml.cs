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
        private readonly ProductsViewModel productsViewModel;
        private readonly CustomersViewModel customersViewModel;

        public MainWindow()
        {
            InitializeComponent();
            ControlInversion.RegisterDependencies();
            productsViewModel = new(ControlInversion.ProductService());
            customersViewModel = new(ControlInversion.CustomerService());
            salesViewModel = new(ControlInversion.SaleService(), ControlInversion.CustomerService());

            productsGrid.ItemsSource = productsViewModel.Products;
            salesGrid.ItemsSource = salesViewModel.Sales;

            SalesManager.DataContext = salesViewModel;
            ProductsManager.DataContext = productsViewModel;
            CustomersManager.DataContext = customersViewModel;
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

        public async void SearchProductsButtonAction(object sender, RoutedEventArgs e)
        {
            await productsViewModel.GetProducts(SearchProductsTextField.Text);
            productsGrid.ItemsSource = productsViewModel.Products;
        }

        public async void SearchCustomersButtonAction(object sender, RoutedEventArgs e)
        {
            await customersViewModel.GetCustomers(SearchCustomersTextField.Text);
            customersGrid.ItemsSource = customersViewModel.Customers;
        }
    }
}
