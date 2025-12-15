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
        public MainWindow()
        {
            InitializeComponent();
            // ViewModel is already set in XAML via Window.DataContext
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
            if (comboBox.SelectedItem != null && DataContext is MainViewModel viewModel)
            {
                /*viewModel.productName = ((Product)comboBox.SelectedItem).ProductName;
                viewModel.productPrice = (int)((Product)comboBox.SelectedItem).ProductPrice;
                viewModel.OnPropertyChanged(nameof(viewModel.productName));
                viewModel.OnPropertyChanged(nameof(viewModel.productPrice));*/

            }
        }

        // btnAddProduct_Click removed - now using Command binding in XAML
    }
}