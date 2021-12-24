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
    /// Interação lógica para Customers.xam
    /// </summary>
    public partial class Customers : UserControl
    {
        private readonly CustomersViewModel dataContext;
        public Customers()
        {
            InitializeComponent();
            dataContext = ControlInversion.ResolveDependency<CustomersViewModel>();
            customersContainer.DataContext = dataContext;
        }

        public void OpenFlyout(object sender, RoutedEventArgs e)
        {
            dataContext.NewCustomerFlyoutOpen = true;
        }
    }
}
