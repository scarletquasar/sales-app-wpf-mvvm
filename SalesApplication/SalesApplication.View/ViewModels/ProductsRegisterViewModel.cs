using SalesApplication.Abstractions;
using SalesApplication.Data.Responses;
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
    public class ProductsRegisterViewModel : INotifyPropertyChanged
    {
        public ProductsRegisterViewModel(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }
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
                IActionResponse result = await RegisterProduct.Persist();
                return result;
            }
            else
            {
                return new ActionResponse();
            }
        }
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
