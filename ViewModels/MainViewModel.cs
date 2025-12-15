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
        
    
        
        private Product _selectedProduct;

        public event PropertyChangedEventHandler ?PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // constructor

        public MainViewModel()
        {
            
            LoadData();

            // Initialize commands
            AddProductCommand = new RelayCommand(ExecuteAddProduct, CanExecuteAddProduct);
        }

        // read data
        private  void LoadData()
        {

            // get the product from app
            Products = App.products;
        }


        // properties
        public ObservableCollection<Product> Products 
        {
            get => App.products;
            set
            {
                App.products = value;
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
