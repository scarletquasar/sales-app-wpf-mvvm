using SalesApplication.Abstractions;
using SalesApplication.Domain.Business;
using SalesApplication.View.Abstractions;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Prism.Commands;

namespace SalesApplication.View.ViewModels
{
    public delegate void InsertedCustomer();
    public class CustomersRegisterViewModel : INotifyPropertyChanged
    {
        public static event InsertedCustomer OnCustomerInserted = delegate { };
        public DelegateCommand FinishCustomerCommand { get; set; }
        public CustomersRegisterViewModel(
            IRepository<Customer> customerRepository,
            IDialogService dialogService
        )
        {
            _dialogService = dialogService;
            _customerRepository = customerRepository;
            FinishCustomerCommand = new(FinishCustomer);
        }
        private readonly IDialogService _dialogService;
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

        private string customerName;
        public string CustomerName
        {
            get => customerName;
            set
            {
                customerName = value;
                OnPropertyChanged();
            }
        }

        public async void FinishCustomer()
        {
            try
            {
                RegisterCustomer = new Customer(CustomerName, _customerRepository);
            }
            catch(Exception e)
            {
                _dialogService.Show(e.Message);
                return;
            }

            try
            {
                await RegisterCustomer.Persist();
                OnCustomerInserted();
                /* Realiza o reset dos dados da ViewModel */
                RegisterCustomer = default;
                CustomerName = default;

                _dialogService.Show("Cliente registrado com sucesso");
            }
            catch(Exception e)
            {
                _dialogService.Show(e.Message);
            }
        }
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
