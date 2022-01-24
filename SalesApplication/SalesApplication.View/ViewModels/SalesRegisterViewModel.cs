using SalesApplication.Abstractions;
using SalesApplication.Domain.Business;
using SalesApplication.View.Visualization;
using SalesApplication.View.Abstractions;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Prism.Commands;

namespace SalesApplication.View.ViewModels
{
    public delegate void InsertedSale();
    public class SalesRegisterViewModel : INotifyPropertyChanged
    {
        public static event InsertedSale OnSaleInserted = delegate { };
        public DelegateCommand TryAddProductCommand { get; set; }
        public DelegateCommand FinishSaleCommand { get; set; }
        public SalesRegisterViewModel(
            IUnitOfWork<Sale, Product> saleProductUnitOfWork,
            IRepository<Customer> customerRepository,
            IDialogService dialogService
        )
        {
            _saleProductUnitOfWork = saleProductUnitOfWork;
            _customerRepository = customerRepository;
            _dialogService = dialogService;
            TryAddProductCommand = new(TryAddProduct);
            FinishSaleCommand = new(FinishSale);
        }
        private readonly IUnitOfWork<Sale, Product> _saleProductUnitOfWork;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IDialogService _dialogService;

        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<ObservableProduct> registerSaleProducts;
        private Sale registerSale;
        public Sale RegisterSale
        {
            get => registerSale;
            set
            {
                registerSale = value;
                OnPropertyChanged();
            }
        }

        private int saleProductId;
        public int SaleProductId
        {
            get => saleProductId;
            set
            {
                saleProductId = value;
                OnPropertyChanged();
            }
        }

        private int saleProductQuantity;
        public int SaleProductQuantity
        {
            get => saleProductQuantity;
            set
            {
                saleProductQuantity = value;
                OnPropertyChanged();
            }
        }

        private int saleCustomerId;
        public int SaleCustomerId
        {
            get => saleCustomerId;
            set
            {
                saleCustomerId = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ObservableProduct> RegisterSaleProducts
        {
            get => registerSaleProducts;
            set
            {
                registerSaleProducts = value;
                OnPropertyChanged();
            }
        }
        public void Initialize()
        {
            registerSaleProducts = new();
            RegisterSale = new Sale(_saleProductUnitOfWork, _customerRepository);
        }
        public async void TryAddProduct()
        {
            try
            {
                await RegisterSale.TryAddProduct(SaleProductId, SaleProductQuantity);
                ObservableProduct observableProduct = new();
                await observableProduct.Populate(SaleProductId, _saleProductUnitOfWork.Type2Repository);
                observableProduct.QuantidadeUsada = SaleProductQuantity;
                registerSaleProducts.Add(observableProduct);
            }
            catch (Exception err)
            {
                _dialogService.Show(err.Message);
            }
        }
        public async void FinishSale()
        {
            try
            {
                RegisterSale.CustomerId = saleCustomerId;
                await RegisterSale.Persist();
                OnSaleInserted();

                /* Realiza o reset dos dados da ViewModel */
                RegisterSale = new Sale(_saleProductUnitOfWork, _customerRepository);
                SaleProductId = default;
                SaleCustomerId = default;
                RegisterSaleProducts = new();
                SaleProductQuantity = default;

                _dialogService.Show("Venda registrada com sucesso");
            }
            catch (Exception e)
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
