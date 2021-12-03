using MahApps.Metro.Controls;
using SalesApplication.Abstractions;
using SalesApplication.Domain.Business;
using SalesApplication.Domain.Visualization;
using SalesApplication.View.Services;
using System;
using System.Collections.Generic;
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
        private readonly MainWindow _window;
        private readonly IRepository<Sale> _saleRepository;
        private readonly IRepository<Customer> _customerRepository;
        private List<ObservableSale> sales;
        public event PropertyChangedEventHandler PropertyChanged;
        public List<ObservableSale> Sales
        {
            get => sales;
            set
            {
                sales = value;
                OnPropertyChanged();
            }
        }

        public async Task GetSales(string search)
        {
            List<ObservableSale> _obsSales = new();
            List<Sale> rawSales;

            if (uint.TryParse(search, out uint id))
            {
                rawSales = (await _saleRepository.Search(x => x.Id == id)).ToList();
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
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
