using SalesApplication.Abstractions;
using SalesApplication.Domain.Business;
using SalesApplication.View.Abstractions;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SalesApplication.View.ViewModels
{
    public class ProductsRegisterViewModel : INotifyPropertyChanged
    {
        public ProductsRegisterViewModel(
            IRepository<Product> productRepository,
            IDialogService dialogService
        )
        {
            _dialogService = dialogService;
            _productRepository = productRepository;
        }
        private readonly IDialogService _dialogService;
        private readonly IRepository<Product> _productRepository;
        public event PropertyChangedEventHandler PropertyChanged;
        private Product registerProduct;
        public Product RegisterProduct
        {
            get => registerProduct;
            set
            {
                registerProduct = value;
                OnPropertyChanged();
            }
        }
        public async Task FinishProduct(string description, string price, string stock)
        {
            if(double.TryParse(price, out double numPrice) && int.TryParse(stock, out int numStock))
            {
                RegisterProduct = new Product(description, numPrice, numStock, _productRepository);
                try
                {
                    await RegisterProduct.Persist();
                    _dialogService.Show("Produto registrado com sucesso");
                }
                catch
                {
                    _dialogService.Show("Ocorreu um erro ao registrar o produto");
                }
            }
            else
            {
                _dialogService.Show("Verifique as informações de registro inseridas");
            }
        }
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
