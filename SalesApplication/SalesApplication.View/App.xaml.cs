using SalesApplication.Abstractions;
using SalesApplication.Data.Repositories;
using SalesApplication.View.Abstractions;
using SalesApplication.View.Services;
using SalesApplication.View.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SalesApplication.View
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private MainWindow DefaultWindow;
        protected override void OnStartup(StartupEventArgs e)
        {
            ControlInversion.RegisterDependencies();

            // Create main application window, starting minimized if specified
            DefaultWindow = ControlInversion.ResolveDependency<MainWindow>();
            DefaultWindow.Show();
            base.OnStartup(e);
        }
    }
}
