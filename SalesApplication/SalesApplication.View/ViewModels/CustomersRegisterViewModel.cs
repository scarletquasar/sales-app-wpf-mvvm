using SalesApplication.Abstractions;
using SalesApplication.Domain.Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SalesApplication.View.ViewModels
{
    public class CustomersRegisterViewModel : INotifyPropertyChanged
    {
        public CustomersRegisterViewModel(IRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly IRepository<Customer> _customerRepository;
        private Customer registerCustomer;
        public Customer RegisterCustomer
        {
            get => registerCustomer;
            set
            {
                registerCustomer = value;
                OnPropertyChanged();
            }
        }
        public async Task<IActionResponse> FinishCustomer(string name)
        {
            RegisterCustomer = new Customer(name, _customerRepository);
            IActionResponse result = await RegisterCustomer.Persist();
            return result;
        }
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
