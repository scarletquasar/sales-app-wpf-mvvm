using SalesApplication.Abstractions;
using SalesApplication.Domain.Business;
using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SalesApplication.View.Visualization;
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
            IEnumerable<ObservableProduct> rawProducts = new List<ObservableProduct>();
            rawProducts = (from p in await _productRepository.Search()
                           select new ObservableProduct
                           {
                               Id = p.Id,
                               Descrição = p.Description,
                               Preço = p.Price,
                               Estoque = p.Stock
                           });

            if (uint.TryParse(Search, out uint id))
            {
                rawProducts = rawProducts.Where(x => x.Id == id);
            }
            else
            {
                rawProducts = rawProducts.Where(x => x.Descrição.Contains(Search));
            }

            Products = new ObservableCollection<ObservableProduct>(rawProducts);
            OnPropertyChanged();
        }
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
