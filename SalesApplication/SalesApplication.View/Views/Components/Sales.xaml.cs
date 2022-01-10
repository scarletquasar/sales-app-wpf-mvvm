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
using SalesApplication.View;
using SalesApplication.View.Utils;

namespace SalesApplication.View.Views.Components
{
    /// <summary>
    /// Interação lógica para Sales.xam
    /// </summary>
    public partial class Sales : UserControl
    {
        public Sales()
        {
            InitializeComponent();
            salesContainer.DataContext = ControlInversion.ResolveDependency<SalesViewModel>();
        }

        public void OpenFlyout(object sender, RoutedEventArgs e)
        {
            ParentUtil.GetWindow(this).SalesRegisterArea.CreateSale.IsOpen = true;
        }
    }
}
