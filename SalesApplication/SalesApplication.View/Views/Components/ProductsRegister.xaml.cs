using SalesApplication.Abstractions;
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
    /// Interação lógica para ProductsRegister.xam
    /// </summary>
    public partial class ProductsRegister : UserControl
    {
        private readonly ProductsRegisterViewModel _productsRegisterViewModel;
        private readonly ProductsViewModel _productsViewModel;
        private readonly IDialogService _dialogService;
        public ProductsRegister()
        {
            InitializeComponent();

            _dialogService = ControlInversion.ResolveDependency<IDialogService>();
            _productsRegisterViewModel = ControlInversion.ResolveDependency<ProductsRegisterViewModel>();
            _productsViewModel = ControlInversion.ResolveDependency<ProductsViewModel>();

            CreateProduct.DataContext = _productsViewModel;
            CreateProductContainer.DataContext = _productsRegisterViewModel;
        }

        private async void FinishRegisteredProduct(object sender, RoutedEventArgs e)
        {
            try
            {
                await _productsRegisterViewModel.FinishProduct(ProductDescription.Text, ProductPrice.Text, ProductStock.Text);
            }
            catch(Exception err)
            {
                _dialogService.Show(err.Message);
            }
        }
    }
}
