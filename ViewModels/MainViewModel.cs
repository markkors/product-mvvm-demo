using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MVVM_DEMO.Commands;
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

            // Initialize commands
            AddProductCommand = new RelayCommand(ExecuteAddProduct, CanExecuteAddProduct);
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

        // Commands
        public ICommand AddProductCommand { get; set; }

        // Command methods
        private void ExecuteAddProduct(object? parameter)
        {
            Random random = new Random();
            int randomPrice = random.Next(10, 100);

            Products.Add(new Product
            {
                ProductName = $"Product {Products.Count + 1}",
                Price = randomPrice
            });
        }

        private bool CanExecuteAddProduct(object? parameter)
        {
            // You can add validation logic here
            // For now, always allow adding products
            return true;
        }

    }
}
