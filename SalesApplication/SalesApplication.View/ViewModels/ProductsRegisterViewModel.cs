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
        public async Task<IActionResponse> FinishProduct(string name, double price, int stock)
        {
            RegisterProduct.Description = name;
            RegisterProduct.Price = price;
            RegisterProduct.Stock = stock;
            IActionResponse result = await _productRepository.Add(RegisterProduct);
            return result;
        }
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
