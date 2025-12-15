using MVVM_DEMO.Models;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows;

namespace MVVM_DEMO
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
       public static ObservableCollection<Product> products;

        public App()
        {
            
            this.Exit += OnAppExit;
            products = new ObservableCollection<Product>();
            loadData();
           
        }


        private void loadData()
        {
            

            // deserialize products from JSON file
            try
            {
                string filePath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "MVVM_DEMO",
                    "products.json"
                );
                if (File.Exists(filePath))
                {
                    string json = File.ReadAllText(filePath);
                    var loadedProducts = JsonSerializer.Deserialize<ObservableCollection<Product>>(json);
                    if (loadedProducts != null)
                    {
                        products = loadedProducts;
                    }

                    if (products.Count == 0)
                    {
                        loadDemoData();
                    }   


                } else {
                    loadDemoData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fout bij laden: {ex.Message}");
            }

        }

        private void loadDemoData()
        {
            products.Add(new Product { ProductName = "Product 1", ProductPrice = 10 });
            products.Add(new Product { ProductName = "Product 2", ProductPrice = 20 });
            products.Add(new Product { ProductName = "Product 3", ProductPrice = 30 });
        }

        private void OnAppExit(object sender, ExitEventArgs e)
        {
            // seralize products to JSON file
            try
            {
                string json = JsonSerializer.Serialize(products, new JsonSerializerOptions
                {
                    WriteIndented = true // Mooi geformatteerd
                });

                string filePath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "MVVM_DEMO",
                    "products.json"
                );

                // Zorg dat de directory bestaat
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fout bij opslaan: {ex.Message}");
            }

        }
    }

}
