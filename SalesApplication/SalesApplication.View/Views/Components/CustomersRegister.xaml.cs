using SalesApplication.View.Services;
using SalesApplication.View.ViewModels;
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

namespace SalesApplication.View.Views.Components
{
    /// <summary>
    /// Interação lógica para CustomersRegister.xam
    /// </summary>
    public partial class CustomersRegister : UserControl
    {
        private readonly CustomersRegisterViewModel _customersRegisterViewModel;
        private readonly CustomersViewModel _customersViewModel;
        public CustomersRegister()
        {
            InitializeComponent();
            _customersRegisterViewModel = ControlInversion.ResolveDependency<CustomersRegisterViewModel>();
            _customersViewModel = ControlInversion.ResolveDependency<CustomersViewModel>();

            CreateCustomer.DataContext = _customersViewModel;
            CreateCustomerContainer.DataContext = _customersRegisterViewModel;
        }
        private async void FinishRegisteredCustomer(object sender, RoutedEventArgs e)
        {
            await _customersRegisterViewModel.FinishCustomer(CustomerName.Text);
        }
    }
}
