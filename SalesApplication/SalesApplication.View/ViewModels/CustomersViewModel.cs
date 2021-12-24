using Prism.Commands;
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
    public class CustomersViewModel : INotifyPropertyChanged
    {
        public DelegateCommand GetCustomersCommand { get; set; }
        public CustomersViewModel(IRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
            GetCustomersCommand = new(GetCustomers);
        }

        private readonly IRepository<Customer> _customerRepository;
        private bool newCustomerFlyoutOpen;
        public bool NewCustomerFlyoutOpen
        {
            get => newCustomerFlyoutOpen;
            set
            {
                newCustomerFlyoutOpen = value;
                OnPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<ObservableCustomer> customers;
        public ObservableCollection<ObservableCustomer> Customers
        {
            get => customers;
            set
            {
                customers = value;
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

        public async void GetCustomers()
        {
            List<Customer> rawCustomers;
            ObservableCollection<ObservableCustomer> _obsCustomers = new();

            if (uint.TryParse(Search, out uint id))
            {
                rawCustomers = (await _customerRepository.Search(x => x.Id == id)).ToList();
            }
            else
            {
                rawCustomers = (await _customerRepository.Search(x => x.Name.Contains(Search))).ToList();
            }

            foreach (Customer item in rawCustomers)
            {
                ObservableCustomer result = new();
                await result.Populate(item.Id, _customerRepository);
                _obsCustomers.Add(result);
            }

            Customers = _obsCustomers;
        }
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
