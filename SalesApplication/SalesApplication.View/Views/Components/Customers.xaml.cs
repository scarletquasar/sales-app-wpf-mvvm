using SalesApplication.View.Services;
using SalesApplication.View.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SalesApplication.View.Utils;

namespace SalesApplication.View.Views.Components
{
    /// <summary>
    /// Interação lógica para Customers.xam
    /// </summary>
    public partial class Customers : UserControl
    {
        public Customers()
        {
            InitializeComponent();
            customersContainer.DataContext = ControlInversion.ResolveDependency<CustomersViewModel>();
        }

        public void OpenFlyout(object sender, RoutedEventArgs e)
        {
            ParentUtil.GetWindow(this).CustomersRegisterArea.CreateCustomer.IsOpen = true;
        }
    }
}
