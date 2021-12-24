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
using Prism.Commands;

namespace SalesApplication.View.ViewModels
{
    public class ProductsViewModel : INotifyPropertyChanged
    {
        public DelegateCommand GetProductsCommand { get; set; }
        public ProductsViewModel(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
            GetProductsCommand = new(GetProducts);
        }
        private readonly IRepository<Product> _productRepository;
        private bool newProductFlyoutOpen;
        public bool NewProductFlyoutOpen
        {
            get => newProductFlyoutOpen;
            set
            {
                newProductFlyoutOpen = value;
                OnPropertyChanged();
            }
        }

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

        public async void GetProducts()
        {
            List<Product> rawProducts;
            ObservableCollection<ObservableProduct> _obsProducts = new();

            if (uint.TryParse(Search, out uint id))
            {
                rawProducts = (await _productRepository.Search(x => x.Id == id)).ToList();
            }
            else
            {
                rawProducts = (await _productRepository.Search(x => x.Description.Contains(Search))).ToList();
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
