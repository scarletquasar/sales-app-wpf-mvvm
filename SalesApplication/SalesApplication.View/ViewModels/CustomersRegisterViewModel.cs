using SalesApplication.Abstractions;
using SalesApplication.Data.Responses;
using SalesApplication.Domain.Business;
using SalesApplication.View.Abstractions;
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
        public CustomersRegisterViewModel(
            IRepository<Customer> customerRepository,
            IDialogService dialogService
        )
        {
            _dialogService = dialogService;
            _customerRepository = customerRepository;
        }
        private IDialogService _dialogService;
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
            if(name == "")
            {
                _dialogService.Show("O nome do cliente não pode ser vazio");
                return new ActionResponse();
            }

            RegisterCustomer = new Customer(name, _customerRepository);
            ActionResponse result = (ActionResponse)await RegisterCustomer.Persist();

            if (result.Success)
            {
                _dialogService.Show("Cliente registrado com sucesso");
            }
            else
            {
                _dialogService.Show("Ocorreu um erro ao registrar o cliente");
            }

            return result;
        }
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
