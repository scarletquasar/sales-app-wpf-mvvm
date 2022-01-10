using SalesApplication.Abstractions;
using SalesApplication.Domain.Business;
using SalesApplication.View.Abstractions;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Prism.Commands;
using System;

namespace SalesApplication.View.ViewModels
{
    public class ProductsRegisterViewModel : INotifyPropertyChanged
    {
        public DelegateCommand RegisterProductCommand { get; set; }
        public ProductsRegisterViewModel(
            IRepository<Product> productRepository,
            IDialogService dialogService
        )
        {
            _dialogService = dialogService;
            _productRepository = productRepository;
            RegisterProductCommand = new(FinishProduct);
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

        private string productDescription;
        public string ProductDescription
        {
            get => productDescription;
            set
            {
                productDescription = value;
                OnPropertyChanged();
            }
        }

        private double productPrice;
        public double ProductPrice
        {
            get => productPrice;
            set
            {
                productPrice = value;
                OnPropertyChanged();
            }
        }

        private int productStock;
        public int ProductStock
        {
            get => productStock;
            set
            {
                productStock = value;
                OnPropertyChanged();
            }
        }

        public async void FinishProduct()
        {
            RegisterProduct = new Product(productDescription, productStock, productStock, _productRepository);
            try
            {
                await RegisterProduct.Persist();
                _dialogService.Show("Produto registrado com sucesso");
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
