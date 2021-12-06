using SalesApplication.Abstractions;
using SalesApplication.Domain.Business;
using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SalesApplication.Domain.Visualization;
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace SalesApplication.View.ViewModels
{
    public class ProductsViewModel : INotifyPropertyChanged
    {
        public ProductsViewModel(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }
        private readonly IRepository<Product> _productRepository;
        private ObservableCollection<ObservableProduct> products;
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<ObservableProduct> Products
        {
            get => products;
            set
            {
                products = value;
                OnPropertyChanged();
            }
        }
        public async Task GetProducts(string search)
        {
            List<Product> rawProducts;
            ObservableCollection<ObservableProduct> _obsProducts = new();

            if (uint.TryParse(search, out uint id))
            {
                rawProducts = (await _productRepository.Search(x => x.Id == id)).ToList();
            }
            else
            {
                rawProducts = (await _productRepository.Search(x => x.Description.Contains(search))).ToList();
            }

            foreach (Product item in rawProducts)
            {
                ObservableProduct result = new();
                await result.Populate(item.Id, _productRepository);
                _obsProducts.Add(result);
            }

            Products = _obsProducts;
            OnPropertyChanged();
        }
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
