using Prism.Commands;
using SalesApplication.Abstractions;
using SalesApplication.Domain.Business;
using SalesApplication.View.Visualization;
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
            CustomersRegisterViewModel.OnCustomerInserted += GetCustomers;
        }

        private readonly IRepository<Customer> _customerRepository;
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
            IEnumerable<ObservableCustomer> rawCustomers = new List<ObservableCustomer>();
            rawCustomers = (from c in await _customerRepository.Search()
                            select new ObservableCustomer
                            {
                                Id = c.Id,
                                Nome = c.Name
                            });

            if (uint.TryParse(Search, out uint id))
            {
                rawCustomers = rawCustomers.Where(customer => customer.Id == id);
            }
            else if(!string.IsNullOrWhiteSpace(Search))
            {
                rawCustomers = rawCustomers.Where(customer => customer.Nome.Contains(Search));
            }

            Customers = new ObservableCollection<ObservableCustomer>(rawCustomers.ToList());
        }
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
