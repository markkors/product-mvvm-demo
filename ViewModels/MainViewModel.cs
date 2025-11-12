using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVVM_DEMO.Models;

namespace MVVM_DEMO.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        
    
        private ObservableCollection<Product> _products;

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // constructor

        public MainViewModel()
        {
            _products = new ObservableCollection<Product>();
            LoadData();
        }

        // read data
        private  void LoadData()
        {
            _products.Add(new Product { ProductName = "Product 1", Price = 10.0 });
            _products.Add(new Product { ProductName = "Product 2", Price = 20.0 });
            _products.Add(new Product { ProductName = "Product 3", Price = 30.0 });
            Products = _products;
        }


        // properties
        public ObservableCollection<Product> Products { get; set; }

        public string productName { get; set; }
        public int productPrice { get; set; }

    }
}
