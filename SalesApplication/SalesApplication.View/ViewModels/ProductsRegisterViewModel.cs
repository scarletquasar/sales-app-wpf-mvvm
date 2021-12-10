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
        public async Task<IActionResponse> FinishProduct(string description, string price, string stock)
        {
            if(double.TryParse(price, out double numPrice) && int.TryParse(stock, out int numStock))
            {
                RegisterProduct = new Product(description, numPrice, numStock, _productRepository);
                ActionResponse result = (ActionResponse)await RegisterProduct.Persist();

                if(result.Success)
                {
                    _dialogService.Show("Produto registrado com sucesso");
                }
                else
                {
                    _dialogService.Show("Ocorreu um erro ao registrar o produto");
                }

                return result;
            }
            else
            {
                _dialogService.Show("Verifique as informações de registro inseridas");
                return new ActionResponse();
            }
        }
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
