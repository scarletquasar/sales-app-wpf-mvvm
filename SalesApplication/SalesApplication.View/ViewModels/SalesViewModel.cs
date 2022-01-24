using SalesApplication.Abstractions;
using SalesApplication.Domain.Business;
using SalesApplication.View.Visualization;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Prism.Commands;
using SalesApplication.Domain.Abstractions;

namespace SalesApplication.View.ViewModels
{
    public class SalesViewModel : INotifyPropertyChanged
    {
        public DelegateCommand GetSalesCommand { get; set; }
        public DelegateCommand GetSalesByCustomerCommand { get; set; }
        public SalesViewModel(ISaleRepository saleRepository, IRepository<Customer> customerRepository)
        {
            _saleRepository = saleRepository;
            _customerRepository = customerRepository;
            GetSalesCommand = new(() => GetSales(false));
            GetSalesByCustomerCommand = new(() => GetSales(true));
            SalesRegisterViewModel.OnSaleInserted += () => GetSales(true);
        }

        private readonly ISaleRepository _saleRepository;
        private readonly IRepository<Customer> _customerRepository;
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

        public async void GetSales(bool byCostumerId)
        {
            IEnumerable<ObservableSale> rawSales = new List<ObservableSale>();
            rawSales = (from s in await _saleRepository.Search()
                        join c in await _customerRepository.Search() on s.CustomerId equals c.Id
                        select new ObservableSale
                        {
                            Id = s.Id,
                            IdCliente = s.CustomerId,
                            NomeCliente = c.Name,
                            PreçoTotal = s.TotalPrice,
                            FeitaEm = s.CreatedAt
                        });

            if (!byCostumerId)
            {
                if(uint.TryParse(Search, out uint id))
                {
                    rawSales = rawSales.Where(sale => sale.Id == id);
                }
            }
            else
            {
                if(uint.TryParse(Search, out uint id))
                {
                    rawSales = rawSales.Where(sale => sale.IdCliente == id);
                }
            }

            Sales = new ObservableCollection<ObservableSale>(rawSales.ToList());
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
