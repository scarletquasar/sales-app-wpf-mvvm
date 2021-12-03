using SalesApplication.Abstractions;
using SalesApplication.Domain.Business;
using SalesApplication.Domain.Visualization;
using SalesApplication.View.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SalesApplication.View.ViewModels
{
    public class SalesViewModel : INotifyPropertyChanged
    {
        public SalesViewModel(IRepository<Sale> saleRepository)
        {
            _saleRepository = saleRepository;
        }
        private readonly IRepository<Sale> _saleRepository;
        private List<ObservableSale> sales;
        public event PropertyChangedEventHandler PropertyChanged;
        public List<ObservableSale> Sales
        {
            get => sales;
            set
            {
                sales = value;
                OnPropertyChanged();
            }
        }

        public async Task GetSales()
        {
            List<Sale> rawSales = (await _saleRepository.Search()).ToList();
            foreach(Sale item in rawSales)
            {
                ObservableSale result = new();
                await result.Populate(item.Id, _saleRepository);
                Sales.Add(result);
            }
        }

        public async Task GetSales(int id)
        {
            List<Sale> rawSales = (await _saleRepository.Search(x => x.Id == id)).ToList();
            foreach (Sale item in rawSales)
            {
                ObservableSale result = new();
                await result.Populate(item.Id, _saleRepository);
                Sales.Add(result);
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
