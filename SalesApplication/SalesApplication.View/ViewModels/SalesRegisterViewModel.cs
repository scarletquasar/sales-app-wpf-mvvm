using Microsoft.EntityFrameworkCore;
using SalesApplication.Abstractions;
using SalesApplication.Domain.Business;
using SalesApplication.Domain.Visualization;
using SalesApplication.View.Abstractions;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SalesApplication.View.ViewModels
{
    public class SalesRegisterViewModel : INotifyPropertyChanged
    {
        public SalesRegisterViewModel(
            IRepository<Sale> saleRepository,
            IRepository<Product> productRepository,
            IRepository<SoldProduct> soldProductRepository,
            IRepository<Customer> customerRepository,
            IDialogService dialogService
        )
        {
            _saleRepository = saleRepository;
            _productRepository = productRepository;
            _soldProductRepository = soldProductRepository;
            _customerRepository = customerRepository;
            _dialogService = dialogService;
        }
        private readonly IRepository<Sale> _saleRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<SoldProduct> _soldProductRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IDialogService _dialogService;

        public event PropertyChangedEventHandler PropertyChanged;
        private Sale registerSale;
        private ObservableCollection<ObservableProduct> registerSaleProducts;
        public Sale RegisterSale
        {
            get => registerSale;
            set
            {
                registerSale = value;
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
            RegisterSale = new Sale(0, _saleRepository, _productRepository, _soldProductRepository);
        }
        public async Task TryAddProduct(string id, string quantity)
        {
            if (uint.TryParse(id, out uint numId) && uint.TryParse(quantity, out uint numQuantity))
            {
                await RegisterSale.TryAddProduct((int)numId, (int)numQuantity);
                ObservableProduct observableProduct = new();
                await observableProduct.Populate((int)numId, _productRepository);
                observableProduct.QuantidadeUsada = (int)numQuantity;
                registerSaleProducts.Add(observableProduct);
            }
        }
        public async Task FinishSale(string customerId)
        {
            if (int.TryParse(customerId, out int targetCustomerId))
            {
                Customer targetCustomer = (await _customerRepository.Search(x => x.Id == targetCustomerId)).FirstOrDefault();

                if(targetCustomer != null)
                {
                    RegisterSale.CustomerId = targetCustomer.Id;

                    try
                    {
                        await RegisterSale.Persist();
                        _dialogService.Show("Venda registrada com sucesso");
                    }
                    catch(DbUpdateException)
                    {
                        _dialogService.Show("Venda registrada com sucesso");
                    }
                    catch(Exception e)
                    {
                        _dialogService.Show(e.ToString());
                    }
                }
                else
                {
                    _dialogService.Show("Verifique as informações do cliente inseridas");
                }
            }
            else
            {
                _dialogService.Show("Verifique as informações da venda inseridas");
            }
        }
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
