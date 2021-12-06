using MahApps.Metro.Controls;
using SalesApplication.Abstractions;
using SalesApplication.Domain.Business;
using SalesApplication.Domain.Visualization;
using SalesApplication.View.Services;
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
    public class SalesViewModel : INotifyPropertyChanged
    {
        public SalesViewModel(IRepository<Sale> saleRepository, IRepository<Customer> customerRepository)
        {
            _saleRepository = saleRepository;
            _customerRepository = customerRepository;
        }

        private readonly IRepository<Sale> _saleRepository;
        private readonly IRepository<Customer> _customerRepository;
        private ObservableCollection<ObservableSale> sales;
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<ObservableSale> Sales
        {
            get => sales;
            set
            {
                sales = value;
                OnPropertyChanged();
            }
        }

        public async Task GetSales(string search, bool getByCustomerId)
        {
            List<Sale> rawSales;
            ObservableCollection<ObservableSale> _obsSales = new();

            if (uint.TryParse(search, out uint id))
            {
                rawSales = getByCustomerId
                    ? (await _saleRepository.Search(x => x.CustomerId == id)).ToList()
                    : (await _saleRepository.Search(x => x.Id == id)).ToList();
            }
            else
            {
                rawSales = (await _saleRepository.Search()).ToList();
            }
                
            foreach (Sale item in rawSales)
            {
                ObservableSale result = new();
                await result.Populate(item.Id, _saleRepository, _customerRepository);
                _obsSales.Add(result);
            }

            Sales = _obsSales;
            OnPropertyChanged();
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
