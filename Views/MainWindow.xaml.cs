using MVVM_DEMO.Models;
using MVVM_DEMO.ViewModels;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MVVM_DEMO
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainViewModel viewModel = new MainViewModel();

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = viewModel;
            comboBox.SelectionChanged += ComboBox_SelectionChanged;
            // initial selection
            if (comboBox.Items.Count > 0)
            {
                comboBox.SelectedIndex = 0;
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // display selected product details
            if (comboBox.SelectedItem != null)
            {
                viewModel.productName= ((Product)comboBox.SelectedItem).ProductName;
                viewModel.productPrice = (int)((Product)comboBox.SelectedItem).Price;
                viewModel.OnPropertyChanged("productName");
                viewModel.OnPropertyChanged("productPrice");
            }

        }

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            // toevoegen van een product in het viewmodel
            Product p = new Product();
            p.ProductName = $"Product {viewModel.Products.Count + 1}";
            // generate a random price between 10 and 100
            Random rand = new Random();
            p.Price = rand.Next(10, 100);
            viewModel.Products.Add(p);
        }
    }
}