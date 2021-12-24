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
using Prism.Commands;

namespace SalesApplication.View.ViewModels
{
    public class SalesViewModel : INotifyPropertyChanged
    {
        public DelegateCommand GetSalesCommand { get; set; }
        public DelegateCommand GetSalesByCustomerCommand { get; set; }
        public SalesViewModel(IRepository<Sale> saleRepository, IRepository<Customer> customerRepository)
        {
            _saleRepository = saleRepository;
            _customerRepository = customerRepository;
            GetSalesCommand = new(GetSales);
            GetSalesByCustomerCommand = new(GetSalesByCustomerId);
        }

        private readonly IRepository<Sale> _saleRepository;
        private readonly IRepository<Customer> _customerRepository;
        
        private bool newSaleFlyoutOpen;
        public bool NewSaleFlyoutOpen
        {
            get => newSaleFlyoutOpen;
            set
            {
                newSaleFlyoutOpen = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<ObservableSale> sales;
        public ObservableCollection<ObservableSale> Sales
        {
            get => sales;
            set
            {
                sales = value;
                OnPropertyChanged();
            }
        }

        private string search = "";
        public string Search
        {
            get => search;
            set
            {
                search = value;
                OnPropertyChanged();
            }
        }

        public async void GetSales()
        {
            IEnumerable<ObservableSale> rawSales;

            if(uint.TryParse(Search, out uint id))
            {
                rawSales = (from s in await _saleRepository.Search()
                           join c in (await _customerRepository.Search()) on s.CustomerId equals c.Id
                           select new ObservableSale
                           {
                                Id = s.Id,
                                IdCliente = s.CustomerId,
                                NomeCliente = c.Name,
                                PreçoTotal = s.TotalPrice,
                                FeitaEm = s.CreatedAt
                           })
                           .Where(sale => sale.Id == id);
            }
            else
            {
                rawSales = (from s in await _saleRepository.Search()
                           join c in (await _customerRepository.Search()) on s.CustomerId equals c.Id
                           select new ObservableSale
                           {
                                Id = s.Id,
                                IdCliente = s.CustomerId,
                                NomeCliente = c.Name,
                                PreçoTotal = s.TotalPrice,
                                FeitaEm = s.CreatedAt
                           });
            }

            Sales = new ObservableCollection<ObservableSale>(rawSales.ToList());
        }

        public async void GetSalesByCustomerId()
        {
            IEnumerable<ObservableSale> rawSales;

            if (uint.TryParse(Search, out uint id))
            {
                rawSales = (from s in await _saleRepository.Search()
                           join c in (await _customerRepository.Search()) on s.CustomerId equals c.Id
                           select new ObservableSale
                           {
                               Id = s.Id,
                               IdCliente = s.CustomerId,
                               NomeCliente = c.Name,
                               PreçoTotal = s.TotalPrice,
                               FeitaEm = s.CreatedAt
                           })
                           .Where(sale => sale.IdCliente == id);
            }
            else
            {
                rawSales = (from s in await _saleRepository.Search()
                           join c in (await _customerRepository.Search()) on s.CustomerId equals c.Id
                           select new ObservableSale
                           {
                               Id = s.Id,
                               IdCliente = s.CustomerId,
                               NomeCliente = c.Name,
                               PreçoTotal = s.TotalPrice,
                               FeitaEm = s.CreatedAt
                           });
            }

            Sales = new ObservableCollection<ObservableSale>(rawSales.ToList());
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
