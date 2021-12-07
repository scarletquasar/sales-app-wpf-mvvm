using SalesApplication.Abstractions;
using SalesApplication.Domain.Business;
using SalesApplication.Domain.Visualization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SalesApplication.View.ViewModels
{
    public class SalesRegisterViewModel : INotifyPropertyChanged
    {
        public SalesRegisterViewModel(IRepository<Sale> saleRepository)
        {
            _saleRepository = saleRepository;
        }
        private readonly IRepository<Sale> _saleRepository;
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<ObservableProduct> products;
        public ObservableCollection<ObservableProduct> Products
        {
            get => products;
            set
            {
                products = value;
                OnPropertyChanged();
            }
        }
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
