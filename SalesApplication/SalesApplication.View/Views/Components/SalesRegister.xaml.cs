using SalesApplication.View.Abstractions;
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
    /// Interação lógica para SalesRegister.xam
    /// </summary>
    public partial class SalesRegister : UserControl
    {
        private readonly SalesRegisterViewModel _salesRegisterViewModel;
        private readonly SalesViewModel _salesViewModel;
        private readonly IDialogService _dialogService;

        public SalesRegister()
        {
            InitializeComponent();

            _salesRegisterViewModel = ControlInversion.ResolveDependency<SalesRegisterViewModel>();
            _salesViewModel = ControlInversion.ResolveDependency<SalesViewModel>();
            _dialogService = ControlInversion.ResolveDependency<IDialogService>();

            CreateSale.DataContext = _salesViewModel;
            CreateSaleContainer.DataContext = _salesRegisterViewModel;
        }

        private async void TryRegisterProductInSale(object sender, RoutedEventArgs e)
        {
            try
            {
                await _salesRegisterViewModel.TryAddProduct(SaleProductId.Text, SaleProductQuantity.Text);
            }
            catch(Exception err)
            {
                _dialogService.Show(err.Message);
            }
        }

        private async void FinishRegisteredSale(object sender, RoutedEventArgs e)
        {
            await _salesRegisterViewModel.FinishSale(SaleCustomerId.Text);
        }
    }
}
