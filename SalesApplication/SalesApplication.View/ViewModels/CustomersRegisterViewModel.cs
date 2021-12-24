using SalesApplication.Abstractions;
using SalesApplication.Domain.Business;
using SalesApplication.View.Abstractions;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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
        public async Task FinishCustomer(string name)
        {
            if(name == "")
            {
                _dialogService.Show("O nome do cliente não pode ser vazio");
                return;
            }

            RegisterCustomer = new Customer(name, _customerRepository);

            try
            {
                await RegisterCustomer.Persist();
                _dialogService.Show("Cliente registrado com sucesso");
            }
            catch
            {
                _dialogService.Show("Ocorreu um erro ao registrar o cliente");
            }
            return;
        }
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
