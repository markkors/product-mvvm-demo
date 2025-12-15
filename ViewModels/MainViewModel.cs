using MVVM_DEMO.Commands;
using MVVM_DEMO.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVM_DEMO.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        
    
        private ObservableCollection<Product> _products;
        private Product _selectedProduct;

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
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
            _products.Add(new Product { ProductName = "Product 1",ProductPrice = 10 });
            _products.Add(new Product { ProductName = "Product 2", ProductPrice = 20 });
            _products.Add(new Product { ProductName = "Product 3", ProductPrice = 30 });
            Products = _products;
        }


        // properties
        public ObservableCollection<Product> Products 
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged();
            }
        }

        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged();
            }
        }

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
                ProductPrice = randomPrice
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
