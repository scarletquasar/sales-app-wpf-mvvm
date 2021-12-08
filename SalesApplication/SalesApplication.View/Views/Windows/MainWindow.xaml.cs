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
using SalesApplication.Data.Responses;
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
        private readonly SalesRegisterViewModel salesRegisterViewModel;

        public MainWindow()
        {
            InitializeComponent();

            productsViewModel = new(ControlInversion.ProductService());
            customersViewModel = new(ControlInversion.CustomerService());
            salesViewModel = new(ControlInversion.SaleService(), ControlInversion.CustomerService());
            salesRegisterViewModel = new SalesRegisterViewModel(
                ControlInversion.SaleService(),
                ControlInversion.ProductService(),
                ControlInversion.SoldProductService(),
                ControlInversion.CustomerService()
            );
            salesRegisterViewModel.Initialize();

            SalesManager.DataContext = salesViewModel;
            ProductsManager.DataContext = productsViewModel;
            CustomersManager.DataContext = customersViewModel;

            CreateSale.DataContext = salesViewModel;
            CreateSaleContainer.DataContext = salesRegisterViewModel;
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
        }

        public async void SearchProductsButtonAction(object sender, RoutedEventArgs e)
        {
            await productsViewModel.GetProducts(SearchProductsTextField.Text);
        }

        public async void SearchCustomersButtonAction(object sender, RoutedEventArgs e)
        {
            await customersViewModel.GetCustomers(SearchCustomersTextField.Text);
        }

        private void OpenSaleCreationFlyout(object sender, RoutedEventArgs e)
        {
            salesViewModel.NewSaleFlyoutOpen = true;
        }

        private void OpenProductCreationFlyout(object sender, RoutedEventArgs e)
        {
            productsViewModel.NewProductFlyoutOpen = true;
        }
        
        private void OpenCustomerCreationFlyout(object sender, RoutedEventArgs e)
        {
            customersViewModel.NewCustomerFlyoutOpen = true;
        }

        private async void TryRegisterProductInSale(object sender, RoutedEventArgs e)
        {
            await salesRegisterViewModel.TryAddProduct(SaleProductId.Text, SaleProductQuantity.Text);
        }

        private async void FinishRegisteredSale(object sender, RoutedEventArgs e)
        {
            IActionResponse result = await salesRegisterViewModel.FinishSale(SaleCustomerId.Text);
            
            //TODO: Alterar maneira de notificação final ao usuário
            System.Windows.MessageBox.Show(((ActionResponse)result).Success.ToString());
        }
    }
}
