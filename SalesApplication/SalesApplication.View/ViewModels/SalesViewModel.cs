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
        public SalesViewModel()
        {
            _saleRepository = ControlInversion.SaleService();
        }
        private IRepository<Sale> _saleRepository;
        private List<Sale> sales;
        public event PropertyChangedEventHandler PropertyChanged;
        public List<Sale> Sales
        {
            get => sales;
            set
            {
                sales = value;
                OnPropertyChanged();
            }
        }

        public async void GetSales()
        {
            Sales = (await _saleRepository.Search()).ToList();
        }

        public async void GetSales(int id)
        {
            Sales = (await _saleRepository.Search(x => x.Id == id)).ToList();
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
