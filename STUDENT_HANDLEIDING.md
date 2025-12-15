# WPF MVVM Tutorial - Stap voor Stap Handleiding

## Inhoudsopgave
1. [Introductie](#introductie)
2. [Vereisten](#vereisten)
3. [Wat is MVVM?](#wat-is-mvvm)
4. [Stap 1: Project Opzetten](#stap-1-project-opzetten)
5. [Stap 2: Projectstructuur Aanmaken](#stap-2-projectstructuur-aanmaken)
6. [Stap 3: Het Model Bouwen](#stap-3-het-model-bouwen)
7. [Stap 4: RelayCommand Implementeren](#stap-4-relaycommand-implementeren)
8. [Stap 5: Het ViewModel Bouwen](#stap-5-het-viewmodel-bouwen)
9. [Stap 6: Application Context en Data Persistentie](#stap-6-application-context-en-data-persistentie)
10. [Stap 7: De View Bouwen](#stap-7-de-view-bouwen)
11. [Stap 8: Testen en Uitvoeren](#stap-8-testen-en-uitvoeren)
12. [Concepten Uitgelegd](#concepten-uitgelegd)
13. [Veelgemaakte Fouten](#veelgemaakte-fouten)

---

## Introductie

Deze handleiding leidt je stap voor stap door het bouwen van een WPF-applicatie met het MVVM (Model-View-ViewModel) ontwerppatroon. Je bouwt een eenvoudige productenbeheer applicatie waarbij je leert over:
- Data Binding
- Observable Collections
- Property Change Notifications
- ICommand Pattern
- Separation of Concerns
- Application Context voor gedeelde state
- Data Persistentie met JSON serialisatie

**Eindresultaat**: Een werkende applicatie waar je producten kunt bekijken, selecteren en toevoegen met automatische UI-updates en data persistentie tussen sessies.

---

## Vereisten

### Software
- Visual Studio 2022 (Community, Professional of Enterprise)
- .NET 8.0 SDK

### Kennis
- Basis kennis van C#
- Object-georiënteerd programmeren
- Basiskennis van XAML (niet vereist maar handig)

---

## Wat is MVVM?

MVVM staat voor **Model-View-ViewModel** en is een ontwerppatroon dat helpt bij het scheiden van:

- **Model**: De data en business logica
- **View**: De gebruikersinterface (XAML)
- **ViewModel**: De tussenpersoon die data voorbereidt voor de View

### Voordelen van MVVM:
- **Scheiding van zorgen**: UI-code gescheiden van business logica
- **Testbaarheid**: ViewModels kunnen eenvoudig worden getest zonder UI
- **Herbruikbaarheid**: Models en ViewModels zijn herbruikbaar
- **Data Binding**: Automatische synchronisatie tussen UI en data

### Diagram:
```
View (XAML) <------ Data Binding ------> ViewModel
                                            |
                                            |
                                          Model
```

---

## Stap 1: Project Opzetten

### 1.1 Nieuw Project Aanmaken

1. Open Visual Studio 2022
2. Klik op **"Create a new project"**
3. Zoek naar **"WPF Application"** (niet WPF App (.NET Framework)!)
4. Selecteer **"WPF Application"** en klik **Next**

### 1.2 Project Configureren

1. **Project name**: `MVVM_DEMO`
2. **Location**: Kies een geschikte locatie
3. **Solution name**: `MVVM_DEMO`
4. Klik **Next**

### 1.3 Framework Selecteren

1. Selecteer **.NET 8.0** als framework
2. Klik **Create**

### 1.4 Wat Krijg Je?

Visual Studio creëert automatisch:
- `App.xaml` en `App.xaml.cs` - De applicatie startpunt
- `MainWindow.xaml` en `MainWindow.xaml.cs` - Het hoofdvenster
- `MVVM_DEMO.csproj` - Project configuratie

---

## Stap 2: Projectstructuur Aanmaken

Een goede mappenstructuur is essentieel voor overzichtelijke MVVM-applicaties.

### 2.1 Mappen Aanmaken

In de **Solution Explorer**:

1. **Rechtsklik** op het project `MVVM_DEMO`
2. Selecteer **Add > New Folder**
3. Maak de volgende mappen aan:
   - `Models`
   - `ViewModels`
   - `Views`
   - `Commands`
   - `ValueConverters` (voorlopig leeg, voor toekomstige uitbreidingen)

### 2.2 MainWindow.xaml Verplaatsen

1. **Sleep** `MainWindow.xaml` naar de `Views` map
2. Visual Studio vraagt of je namespace references wilt updaten - klik **Yes**

### 2.3 Projectstructuur Controleren

Je projectstructuur zou er nu zo uit moeten zien:
```
MVVM_DEMO/
├── Commands/
├── Models/
├── ViewModels/
├── Views/
│   ├── MainWindow.xaml
│   └── MainWindow.xaml.cs
├── ValueConverters/
├── App.xaml
└── MVVM_DEMO.csproj
```

---

## Stap 3: Het Model Bouwen

Het **Model** bevat de data structuur. We gaan een `Product` class maken.

### 3.1 Product Class Aanmaken

1. **Rechtsklik** op de `Models` map
2. Selecteer **Add > Class...**
3. Naam: `Product.cs`
4. Klik **Add**

### 3.2 Product Class Implementeren

Open `Product.cs` en vervang de inhoud met:

```csharp
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_DEMO.Models
{
    public class Product
    {
        private string _productName;
        private decimal _productPrice;

        public string ProductName
        {
            get => _productName;
            set
            {
                _productName = value;
                OnPropertyChanged();
            }
        }

        public decimal ProductPrice
        {
            get => _productPrice;
            set
            {
                _productPrice = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
```

### 3.3 Code Uitleg

#### Private Fields
```csharp
private string _productName;
private decimal _productPrice;
```
- **Backing fields** voor de properties
- Conventie: underscore prefix voor private fields

#### Properties met OnPropertyChanged
```csharp
public string ProductName
{
    get => _productName;
    set
    {
        _productName = value;
        OnPropertyChanged();
    }
}
```
- **get**: Retourneert de waarde van het private field
- **set**: Zet de nieuwe waarde EN roept `OnPropertyChanged()` aan
- Dit zorgt ervoor dat de UI wordt genotificeerd bij wijzigingen

#### INotifyPropertyChanged Event
```csharp
public event PropertyChangedEventHandler PropertyChanged;
```
- Event dat wordt afgevuurd wanneer een property verandert
- De UI kan zich abonneren op dit event

#### OnPropertyChanged Method
```csharp
protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
{
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
```
- **[CallerMemberName]**: Automatisch de naam van de aanroepende property
- **PropertyChanged?.Invoke**: Veilig aanroepen (alleen als er listeners zijn)
- **PropertyChangedEventArgs**: Bevat de naam van de gewijzigde property

### 3.4 Waarom INotifyPropertyChanged?

Zonder `INotifyPropertyChanged` weet de UI niet wanneer data verandert. Met dit pattern:
1. Property wordt gewijzigd
2. `OnPropertyChanged()` wordt aangeroepen
3. Event wordt afgevuurd
4. UI luistert naar het event
5. UI update zichzelf automatisch

---

## Stap 4: RelayCommand Implementeren

**Commands** zijn de MVVM-manier om button clicks en andere UI-acties af te handelen zonder code-behind.

### 4.1 RelayCommand Class Aanmaken

1. **Rechtsklik** op de `Commands` map
2. Selecteer **Add > Class...**
3. Naam: `RelayCommand.cs`
4. Klik **Add**

### 4.2 RelayCommand Implementeren

Open `RelayCommand.cs` en vervang de inhoud met:

```csharp
using System;
using System.Windows.Input;

namespace MVVM_DEMO.Commands
{
    /// <summary>
    /// A command whose sole purpose is to relay its functionality to other objects by invoking delegates.
    /// The default return value for the CanExecute method is 'true'.
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Func<object?, bool>? _canExecute;

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Creates a new command that can always execute.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        public RelayCommand(Action<object?> execute) : this(execute, null)
        {
        }

        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        /// <summary>
        /// Determines whether this command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
        public void Execute(object? parameter)
        {
            _execute(parameter);
        }
    }
}
```

### 4.3 Code Uitleg

#### ICommand Interface
```csharp
public class RelayCommand : ICommand
```
- **ICommand**: WPF interface voor commando's
- Vereist: `Execute()`, `CanExecute()`, en `CanExecuteChanged` event

#### Private Fields
```csharp
private readonly Action<object?> _execute;
private readonly Func<object?, bool>? _canExecute;
```
- **_execute**: De method die wordt uitgevoerd wanneer het commando wordt aangeroepen
- **_canExecute**: Optionele method die bepaalt of het commando kan worden uitgevoerd
- **Action<object?>**: Delegate voor een method zonder return waarde
- **Func<object?, bool>**: Delegate voor een method die een boolean retourneert

#### CanExecuteChanged Event
```csharp
public event EventHandler? CanExecuteChanged
{
    add { CommandManager.RequerySuggested += value; }
    remove { CommandManager.RequerySuggested -= value; }
}
```
- Koppelt aan WPF's `CommandManager`
- WPF controleert automatisch of commando's kunnen worden uitgevoerd
- Buttons worden automatisch disabled als `CanExecute` false retourneert

#### Constructors
```csharp
public RelayCommand(Action<object?> execute) : this(execute, null)
{
}

public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute)
{
    _execute = execute ?? throw new ArgumentNullException(nameof(execute));
    _canExecute = canExecute;
}
```
- **Eerste constructor**: Voor commando's die altijd kunnen worden uitgevoerd
- **Tweede constructor**: Voor commando's met validatie logica
- **Null check**: Gooit exception als `execute` null is

#### Execute Method
```csharp
public void Execute(object? parameter)
{
    _execute(parameter);
}
```
- Roept de `_execute` delegate aan
- Wordt aangeroepen wanneer de gebruiker de actie triggert (bijv. button click)

#### CanExecute Method
```csharp
public bool CanExecute(object? parameter)
{
    return _canExecute == null || _canExecute(parameter);
}
```
- Als `_canExecute` null is: altijd true (commando kan altijd)
- Anders: roep `_canExecute` aan en retourneer het resultaat

### 4.4 Waarom RelayCommand?

Zonder commando's zou je code-behind nodig hebben:
```csharp
// SLECHT: Code-behind (niet MVVM)
private void Button_Click(object sender, RoutedEventArgs e)
{
    // Logic hier
}
```

Met RelayCommand:
```csharp
// GOED: MVVM met Commands
public ICommand AddProductCommand { get; set; }
AddProductCommand = new RelayCommand(ExecuteAddProduct, CanExecuteAddProduct);
```

**Voordelen**:
- Testbaar (zonder UI)
- Herbruikbaar
- Separation of concerns
- Automatische enable/disable logica

---

## Stap 5: Het ViewModel Bouwen

Het **ViewModel** is het hart van MVVM - het verbindt de data (Model) met de UI (View).

### 5.1 MainViewModel Class Aanmaken

1. **Rechtsklik** op de `ViewModels` map
2. Selecteer **Add > Class...**
3. Naam: `MainViewModel.cs`
4. Klik **Add**

### 5.2 MainViewModel Implementeren

Open `MainViewModel.cs` en vervang de inhoud met:

```csharp
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

        public event PropertyChangedEventHandler PropertyChanged;

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
        private void LoadData()
        {
            // Get the products from Application context
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
```

### 5.3 Code Uitleg - Deel 1: Fields en INotifyPropertyChanged

#### Private Fields
```csharp
private Product _selectedProduct;
```
- **_selectedProduct**: Backing field voor het geselecteerde product
- **Opmerking**: De productenlijst wordt nu beheerd in de Application context (`App.products`), niet in het ViewModel

#### INotifyPropertyChanged
```csharp
public event PropertyChangedEventHandler PropertyChanged;

public void OnPropertyChanged([CallerMemberName] string propertyName = null)
{
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
```
- Zelfde pattern als in het Model
- Notificeert de UI over property wijzigingen

### 5.4 Code Uitleg - Deel 2: Constructor en Data Loading

#### Constructor
```csharp
public MainViewModel()
{
    LoadData();

    // Initialize commands
    AddProductCommand = new RelayCommand(ExecuteAddProduct, CanExecuteAddProduct);
}
```
- Laadt de referentie naar de productenlijst via `LoadData()`
- Maakt het `AddProductCommand` aan met execute en canExecute methods
- **Belangrijk**: De ObservableCollection zelf wordt beheerd in `App.xaml.cs`

#### LoadData Method
```csharp
private void LoadData()
{
    // Get the products from Application context
    Products = App.products;
}
```
- Haalt de productenlijst op uit de Application context
- `App.products` wordt beheerd in `App.xaml.cs` en is applicatie-breed beschikbaar
- De initiële data wordt geladen in `App.xaml.cs` (zie volgende sectie)
- Dit patroon zorgt ervoor dat de data persistent is over verschillende ViewModels heen

### 5.5 Code Uitleg - Deel 3: Properties

#### Products Property
```csharp
public ObservableCollection<Product> Products
{
    get => App.products;
    set
    {
        App.products = value;
        OnPropertyChanged();
    }
}
```
- **ObservableCollection**: Speciale collectie die automatisch de UI notificeert bij Add/Remove
- Publieke property voor data binding vanuit XAML
- **Direct gekoppeld aan `App.products`**: Dit zorgt ervoor dat alle ViewModels dezelfde data delen
- Wijzigingen aan de lijst zijn applicatie-breed zichtbaar

#### SelectedProduct Property
```csharp
public Product SelectedProduct
{
    get => _selectedProduct;
    set
    {
        _selectedProduct = value;
        OnPropertyChanged();
    }
}
```
- Houdt bij welk product momenteel geselecteerd is
- Bij wijziging wordt de UI automatisch geüpdatet

### 5.6 Code Uitleg - Deel 4: Command Implementation

#### Command Property
```csharp
public ICommand AddProductCommand { get; set; }
```
- **ICommand**: Interface voor commando's in WPF
- Kan gebonden worden aan buttons in XAML

#### ExecuteAddProduct Method
```csharp
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
```
- **Wordt aangeroepen** wanneer de button wordt geklikt
- **Random prijs** tussen 10 en 100
- **String interpolation**: `$"Product {Products.Count + 1}"`
- **ObservableCollection.Add**: Automatische UI update

#### CanExecuteAddProduct Method
```csharp
private bool CanExecuteAddProduct(object? parameter)
{
    return true;
}
```
- Bepaalt of het commando kan worden uitgevoerd
- `true`: button is enabled
- `false`: button is disabled
- Hier altijd `true`, maar je zou validatie kunnen toevoegen

**Voorbeeld met validatie**:
```csharp
private bool CanExecuteAddProduct(object? parameter)
{
    return Products.Count < 10; // Max 10 producten
}
```

### 5.7 Waarom ObservableCollection?

Vergelijk met een normale List:

```csharp
// SLECHT: Normale List
List<Product> products = new List<Product>();
products.Add(new Product()); // UI wordt NIET geüpdatet!

// GOED: ObservableCollection
ObservableCollection<Product> products = new ObservableCollection<Product>();
products.Add(new Product()); // UI wordt WEL geüpdatet!
```

**ObservableCollection** implementeert `INotifyCollectionChanged` en update de UI automatisch bij:
- Add
- Remove
- Clear
- Replace

---

## Stap 6: Application Context en Data Persistentie

Voordat we de View bouwen, moeten we de Application context configureren voor data beheer en persistentie.

### 6.1 App.xaml.cs Aanpassen

Open `App.xaml.cs` en vervang de inhoud met:

```csharp
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
                }
                else
                {
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
            // serialize products to JSON file
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
```

### 6.2 Code Uitleg - Application Context

#### Static Products Field
```csharp
public static ObservableCollection<Product> products;
```
- **static**: Één gedeelde instantie voor de hele applicatie
- **public**: Toegankelijk vanuit alle ViewModels via `App.products`
- **ObservableCollection**: Automatische UI updates bij wijzigingen

#### Constructor
```csharp
public App()
{
    this.Exit += OnAppExit;
    products = new ObservableCollection<Product>();
    loadData();
}
```
- Abonneert op `Exit` event voor het opslaan bij afsluiten
- Initialiseert de productenlijst
- Laadt data bij opstarten

#### loadData Method
```csharp
private void loadData()
{
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
        }
        else
        {
            loadDemoData();
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Fout bij laden: {ex.Message}");
    }
}
```
- **Environment.SpecialFolder.ApplicationData**: Gebruikersspecifieke data folder (`%AppData%`)
- **Path.Combine**: Platformonafhankelijk paden samenstellen
- **File.Exists**: Controleer of het bestand bestaat
- **JsonSerializer.Deserialize**: Converteer JSON naar objecten
- Als het bestand niet bestaat of leeg is: laad demo data

#### loadDemoData Method
```csharp
private void loadDemoData()
{
    products.Add(new Product { ProductName = "Product 1", ProductPrice = 10 });
    products.Add(new Product { ProductName = "Product 2", ProductPrice = 20 });
    products.Add(new Product { ProductName = "Product 3", ProductPrice = 30 });
}
```
- Laadt 3 standaard producten bij eerste gebruik

#### OnAppExit Method
```csharp
private void OnAppExit(object sender, ExitEventArgs e)
{
    try
    {
        string json = JsonSerializer.Serialize(products, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        string filePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "MVVM_DEMO",
            "products.json"
        );

        Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        File.WriteAllText(filePath, json);
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Fout bij opslaan: {ex.Message}");
    }
}
```
- **JsonSerializer.Serialize**: Converteer objecten naar JSON
- **WriteIndented**: Maak de JSON leesbaar (met inspringing)
- **Directory.CreateDirectory**: Maak de folder aan als deze niet bestaat
- **File.WriteAllText**: Sla de JSON op in een bestand

### 6.3 Waarom Application Context?

**Voordelen**:
- **Centraal data beheer**: Één plek voor applicatie-brede data
- **Persistentie**: Data blijft behouden tussen sessies
- **Gedeelde state**: Meerdere ViewModels kunnen dezelfde data gebruiken
- **Lifecycle management**: Automatisch laden en opslaan

**Waar wordt de data opgeslagen?**
- Windows: `C:\Users\[Username]\AppData\Roaming\MVVM_DEMO\products.json`
- De `AppData\Roaming` folder is bedoeld voor gebruikersspecifieke applicatiedata

---

## Stap 7: De View Bouwen

De **View** is de gebruikersinterface in XAML. We gaan `MainWindow.xaml` aanpassen.

### 7.1 MainWindow.xaml Aanpassen

Open `Views/MainWindow.xaml` en vervang de inhoud met:

```xml
<Window x:Class="MVVM_DEMO.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:local="clr-namespace:MVVM_DEMO"
        xmlns:vm="clr-namespace:MVVM_DEMO.ViewModels"
        mc:Ignorable="d"
        Title="MainWindow"
        Height="450"
        Width="800">
    <Window.DataContext>
        <vm:MainViewModel />
    </Window.DataContext>

    <Grid>
        <StackPanel>
            <ComboBox x:Name="comboBox"
                      Width="200"
                      Height="30"
                      Margin="10"
                      VerticalAlignment="Top"
                      ItemsSource="{Binding Products}"
                      SelectedItem="{Binding SelectedProduct, Mode=TwoWay}"
                      DisplayMemberPath="ProductName" />
            <Button x:Name="btnAddProduct"
                    Content="Add Product"
                    Width="100px"
                    Height="25px"
                    Command="{Binding AddProductCommand}" />
            <Label Content="Selected Product Details"
                   FontWeight="Bold"
                   FontSize="16"
                   Margin="10" />
            <TextBox x:Name="txtProductName"
                     Width="200"
                     Height="30"
                     Text="{Binding SelectedProduct.ProductName, UpdateSourceTrigger=PropertyChanged}"
                     />
            <TextBox x:Name="txtProductPrice"
                     Width="200"
                     Height="30"
                     Text="{Binding SelectedProduct.ProductPrice, UpdateSourceTrigger=PropertyChanged}" />
        </StackPanel>
    </Grid>
</Window>
```

### 7.2 XAML Code Uitleg - Deel 1: Namespaces en Window

#### Namespaces
```xml
xmlns:vm="clr-namespace:MVVM_DEMO.ViewModels"
```
- **xmlns**: XML namespace declaratie
- **vm**: Prefix voor ons ViewModel namespace
- **clr-namespace**: .NET namespace
- Nu kunnen we `vm:MainViewModel` gebruiken in XAML

#### DataContext
```xml
<Window.DataContext>
    <vm:MainViewModel />
</Window.DataContext>
```
- **DataContext**: De data bron voor alle bindings in deze Window
- Maakt een nieuwe instantie van `MainViewModel` aan
- Alle child controls erven deze DataContext

**Alternatief (in code-behind)**:
```csharp
// In MainWindow.xaml.cs
public MainWindow()
{
    InitializeComponent();
    DataContext = new MainViewModel();
}
```

### 7.3 XAML Code Uitleg - Deel 2: Layout

#### Grid en StackPanel
```xml
<Grid>
    <StackPanel>
        <!-- Controls hier -->
    </StackPanel>
</Grid>
```
- **Grid**: Basis layout container
- **StackPanel**: Stapelt child controls verticaal (standaard)

### 7.4 XAML Code Uitleg - Deel 3: ComboBox

```xml
<ComboBox x:Name="comboBox"
          Width="200"
          Height="30"
          Margin="10"
          VerticalAlignment="Top"
          ItemsSource="{Binding Products}"
          SelectedItem="{Binding SelectedProduct, Mode=TwoWay}"
          DisplayMemberPath="ProductName" />
```

#### Properties Uitleg:
- **x:Name**: Identificatie naam (voor code-behind toegang)
- **Width/Height**: Afmetingen in pixels
- **Margin**: Ruimte rondom (hier: 10 pixels aan alle kanten)
- **VerticalAlignment**: Verticale uitlijning

#### Data Binding Properties:
- **ItemsSource**: `{Binding Products}`
  - Bindt aan de `Products` property in het ViewModel
  - Toont alle producten in de lijst

- **SelectedItem**: `{Binding SelectedProduct, Mode=TwoWay}`
  - Bindt aan de `SelectedProduct` property
  - **Mode=TwoWay**: Wijzigingen gaan beide kanten op
    - View → ViewModel: Gebruiker selecteert een product
    - ViewModel → View: Code wijzigt SelectedProduct

- **DisplayMemberPath**: `"ProductName"`
  - Welke property van Product wordt getoond in de ComboBox
  - Toont de naam, niet het hele object

#### Binding Modes Uitgelegd:
```
OneWay:     ViewModel → View (default voor meeste properties)
TwoWay:     ViewModel ↔ View (default voor user input controls)
OneTime:    ViewModel → View (alleen bij initialisatie)
OneWayToSource: View → ViewModel
```

### 7.5 XAML Code Uitleg - Deel 4: Button

```xml
<Button x:Name="btnAddProduct"
        Content="Add Product"
        Width="100px"
        Height="25px"
        Command="{Binding AddProductCommand}" />
```

#### Properties Uitleg:
- **Content**: De tekst op de button
- **Command**: `{Binding AddProductCommand}`
  - Bindt aan het `AddProductCommand` in het ViewModel
  - Geen `Click` event in code-behind nodig!
  - Bij klik wordt `Execute` van het commando aangeroepen

**Verschil met code-behind**:
```xml
<!-- SLECHT: Code-behind -->
<Button Click="Button_Click" />

<!-- GOED: MVVM met Command -->
<Button Command="{Binding AddProductCommand}" />
```

### 7.6 XAML Code Uitleg - Deel 5: TextBoxes

#### Product Naam TextBox
```xml
<TextBox x:Name="txtProductName"
         Width="200"
         Height="30"
         Text="{Binding SelectedProduct.ProductName, UpdateSourceTrigger=PropertyChanged}" />
```

#### Binding Uitleg:
- **Text**: `{Binding SelectedProduct.ProductName}`
  - **Nested binding**: Bindt aan een property van een property
  - `SelectedProduct` is een property van MainViewModel
  - `ProductName` is een property van het Product object

- **UpdateSourceTrigger=PropertyChanged**
  - **PropertyChanged**: Update bij elke toetsaanslag
  - Default zou zijn: Update bij focus verlies
  - Geeft real-time updates

#### UpdateSourceTrigger Opties:
```
PropertyChanged: Bij elke wijziging (real-time)
LostFocus:      Bij verlies van focus (default)
Explicit:       Alleen bij expliciete aanroep
```

#### Product Prijs TextBox
```xml
<TextBox x:Name="txtProductPrice"
         Width="200"
         Height="30"
         Text="{Binding SelectedProduct.ProductPrice, UpdateSourceTrigger=PropertyChanged}" />
```
- Zelfde concept als ProductName
- Bindt aan `ProductPrice` property
- Automatische conversie van `decimal` naar `string` en vice versa

### 7.7 MainWindow.xaml.cs Aanpassen

Open `Views/MainWindow.xaml.cs` en vervang de inhoud met:

```csharp
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
```

#### Code-behind Uitleg:
- **Minimale code-behind**: Dit is het doel van MVVM!
- **SelectionChanged event**: Zorgt voor initiële selectie
- **Commented code**: Oude manier (niet meer nodig dankzij binding)
- **Opmerking over Command**: Button gebruikt nu Command binding

**Belangrijk**: In pure MVVM zou zelfs de `SelectionChanged` event handler vermeden worden, maar voor beginners is dit een acceptabele compromise.

---

## Stap 8: Testen en Uitvoeren

### 8.1 Build het Project

1. Druk op **Ctrl + Shift + B** of
2. Menu: **Build > Build Solution**
3. Controleer de **Output** window voor errors

### 8.2 Project Uitvoeren

1. Druk op **F5** of
2. Klik op de groene **Start** button
3. De applicatie zou moeten opstarten

### 8.3 Functionaliteit Testen

#### Test 1: Producten Bekijken
1. Open de ComboBox dropdown
2. Je zou 3 producten moeten zien:
   - Product 1
   - Product 2
   - Product 3

#### Test 2: Product Selecteren
1. Selecteer een product uit de ComboBox
2. De TextBoxes zouden automatisch moeten updaten met:
   - Product naam
   - Product prijs

#### Test 3: Product Toevoegen
1. Klik op de **"Add Product"** button
2. Een nieuw product wordt toegevoegd aan de lijst
3. Het nieuwe product verschijnt in de ComboBox
4. De naam is "Product 4" (of hoger)
5. De prijs is willekeurig tussen 10 en 100

#### Test 4: Live Editing
1. Selecteer een product
2. Typ in de **Product Name** TextBox
3. Wijzig de naam (bijv. "Laptop")
4. Open de ComboBox weer
5. De naam zou onmiddellijk geüpdatet moeten zijn!

#### Test 5: Prijs Editing
1. Selecteer een product
2. Wijzig de prijs in de **Product Price** TextBox
3. Selecteer een ander product en terug
4. De prijs zou behouden moeten blijven

#### Test 6: Data Persistentie
1. Voeg een paar nieuwe producten toe
2. Wijzig de naam of prijs van bestaande producten
3. Sluit de applicatie volledig af
4. Start de applicatie opnieuw
5. Alle wijzigingen zouden bewaard moeten zijn!
6. De producten worden opgeslagen in: `%AppData%\MVVM_DEMO\products.json`

**Bonus**: Open het JSON bestand in Notepad om de opgeslagen data te bekijken!

### 8.4 Troubleshooting

#### Probleem: Applicatie start niet
- **Oplossing**: Controleer of er build errors zijn
- Check: `Error List` window (View > Error List)

#### Probleem: ComboBox is leeg
- **Oplossing**:
  - Controleer of `LoadData()` wordt aangeroepen in constructor
  - Controleer `ItemsSource` binding in XAML
  - Zet een breakpoint in `LoadData()` method

#### Probleem: TextBoxes updaten niet
- **Oplossing**:
  - Controleer `SelectedProduct` binding
  - Controleer of `OnPropertyChanged()` wordt aangeroepen
  - Controleer `UpdateSourceTrigger=PropertyChanged`

#### Probleem: Button doet niets
- **Oplossing**:
  - Controleer `Command` binding
  - Zet breakpoint in `ExecuteAddProduct` method
  - Controleer of `AddProductCommand` wordt geïnitialiseerd

---

## Concepten Uitgelegd

### 1. Data Binding

**Wat is het?**
Data Binding is het automatisch synchroniseren van data tussen View en ViewModel.

**Syntax**:
```xml
<TextBox Text="{Binding PropertyName}" />
```

**Hoe werkt het?**
1. WPF maakt een binding object
2. Binding abonneert zich op `PropertyChanged` event
3. Bij wijziging update WPF de UI automatisch
4. Bij user input update WPF het ViewModel (TwoWay)

**Binding Path**:
```xml
<!-- Simple binding -->
<TextBox Text="{Binding ProductName}" />

<!-- Nested binding -->
<TextBox Text="{Binding SelectedProduct.ProductName}" />

<!-- Collection binding -->
<ComboBox ItemsSource="{Binding Products}" />
```

### 2. INotifyPropertyChanged

**Waarom nodig?**
WPF moet weten wanneer data verandert om de UI te updaten.

**Implementatie Pattern**:
```csharp
// 1. Implementeer interface
public class MyClass : INotifyPropertyChanged
{
    // 2. Declareer event
    public event PropertyChangedEventHandler PropertyChanged;

    // 3. Helper method
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    // 4. Gebruik in properties
    private string _myProperty;
    public string MyProperty
    {
        get => _myProperty;
        set
        {
            _myProperty = value;
            OnPropertyChanged(); // Automatisch "MyProperty" als naam
        }
    }
}
```

**CallerMemberName Attribute**:
```csharp
// Zonder CallerMemberName
OnPropertyChanged("MyProperty"); // Foutgevoelig!

// Met CallerMemberName
OnPropertyChanged(); // Automatisch de juiste naam
```

### 3. ObservableCollection

**Verschil met List**:
```csharp
// List<T>
List<Product> products = new List<Product>();
products.Add(new Product()); // UI update: ❌

// ObservableCollection<T>
ObservableCollection<Product> products = new ObservableCollection<Product>();
products.Add(new Product()); // UI update: ✅
```

**Events**:
- `CollectionChanged`: Fired bij Add, Remove, Clear, Replace
- Automatisch gedetecteerd door WPF bindings

**Wanneer gebruiken?**:
- Voor collections die gebonden zijn aan de UI
- Niet voor interne data (gebruik List)
- Niet voor read-only data (gebruik IEnumerable)

### 4. ICommand Pattern

**Waarom Commands?**
- Scheiding van UI en logica
- Testbaar zonder UI
- Automatische enable/disable logica
- Herbruikbaar

**ICommand Interface**:
```csharp
public interface ICommand
{
    event EventHandler CanExecuteChanged;
    bool CanExecute(object parameter);
    void Execute(object parameter);
}
```

**RelayCommand Gebruik**:
```csharp
// In ViewModel
public ICommand MyCommand { get; set; }

// Constructor
MyCommand = new RelayCommand(ExecuteMethod, CanExecuteMethod);

// Execute method
private void ExecuteMethod(object? parameter)
{
    // Doe iets
}

// CanExecute method
private bool CanExecuteMethod(object? parameter)
{
    return true; // of false om button te disablen
}
```

**XAML Binding**:
```xml
<Button Command="{Binding MyCommand}" />
```

### 5. DataContext

**Wat is het?**
De data bron voor alle bindings in een control en zijn children.

**Inheritance**:
```xml
<Window> <!-- DataContext = MainViewModel -->
    <StackPanel> <!-- Inherits: MainViewModel -->
        <TextBox Text="{Binding ProductName}" /> <!-- Binds to MainViewModel.ProductName -->
    </StackPanel>
</Window>
```

**Manieren om te zetten**:
```xml
<!-- In XAML -->
<Window.DataContext>
    <vm:MainViewModel />
</Window.DataContext>

<!-- In code-behind -->
DataContext = new MainViewModel();
```

### 6. Binding Modes

**OneWay** (default voor de meeste properties):
```
ViewModel → View
```
```xml
<TextBlock Text="{Binding ProductName, Mode=OneWay}" />
```
- View update bij ViewModel wijziging
- ViewModel update NIET bij View wijziging

**TwoWay** (default voor input controls):
```
ViewModel ↔ View
```
```xml
<TextBox Text="{Binding ProductName, Mode=TwoWay}" />
```
- Wijzigingen gaan beide kanten op
- Gebruikelijk voor user input

**OneTime**:
```
ViewModel → View (only once)
```
- Alleen bij initialisatie
- Geen updates daarna
- Performance voordeel

**OneWayToSource**:
```
View → ViewModel
```
- Omgekeerde van OneWay
- Zeldzaam gebruikt

### 7. UpdateSourceTrigger

Bepaalt **wanneer** de binding wordt geüpdatet.

**PropertyChanged**:
```xml
<TextBox Text="{Binding Name, UpdateSourceTrigger=PropertyChanged}" />
```
- Bij elke toetsaanslag
- Real-time updates
- Hogere load

**LostFocus** (default):
```xml
<TextBox Text="{Binding Name, UpdateSourceTrigger=LostFocus}" />
```
- Bij verlies van focus
- Minder updates
- Betere performance

**Explicit**:
```xml
<TextBox Text="{Binding Name, UpdateSourceTrigger=Explicit}" />
```
- Alleen bij expliciete aanroep
```csharp
BindingExpression binding = txtBox.GetBindingExpression(TextBox.TextProperty);
binding.UpdateSource();
```

---

## Veelgemaakte Fouten

### Fout 1: Vergeten INotifyPropertyChanged te implementeren

**Symptoom**: UI update niet bij data wijziging

**Fout Code**:
```csharp
public class Product
{
    public string Name { get; set; } // ❌
}
```

**Correcte Code**:
```csharp
public class Product : INotifyPropertyChanged
{
    private string _name;
    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged(); // ✅
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
```

### Fout 2: List gebruiken in plaats van ObservableCollection

**Symptoom**: UI update niet bij Add/Remove

**Fout Code**:
```csharp
public List<Product> Products { get; set; } // ❌
```

**Correcte Code**:
```csharp
public ObservableCollection<Product> Products { get; set; } // ✅
```

### Fout 3: Binding Mode vergeten bij TwoWay binding

**Symptoom**: Wijzigingen in View komen niet in ViewModel

**Fout Code**:
```xml
<ComboBox SelectedItem="{Binding SelectedProduct}" /> <!-- ❌ -->
```

**Correcte Code**:
```xml
<ComboBox SelectedItem="{Binding SelectedProduct, Mode=TwoWay}" /> <!-- ✅ -->
```

**Note**: Voor de meeste input controls (TextBox, ComboBox, CheckBox) is TwoWay de default. Maar het is beter om expliciet te zijn!

### Fout 4: OnPropertyChanged vergeten aan te roepen

**Symptoom**: UI update niet

**Fout Code**:
```csharp
public string Name
{
    get => _name;
    set
    {
        _name = value; // ❌ Geen OnPropertyChanged
    }
}
```

**Correcte Code**:
```csharp
public string Name
{
    get => _name;
    set
    {
        _name = value;
        OnPropertyChanged(); // ✅
    }
}
```

### Fout 5: DataContext niet zetten

**Symptoom**: Bindings werken niet, data is null

**Fout Code**:
```xml
<Window>
    <!-- ❌ Geen DataContext! -->
    <TextBox Text="{Binding ProductName}" />
</Window>
```

**Correcte Code**:
```xml
<Window>
    <Window.DataContext>
        <vm:MainViewModel /> <!-- ✅ -->
    </Window.DataContext>
    <TextBox Text="{Binding ProductName}" />
</Window>
```

### Fout 6: Command niet initialiseren

**Symptoom**: Button doet niets bij klik

**Fout Code**:
```csharp
public ICommand MyCommand { get; set; } // ❌ null!

public MainViewModel()
{
    // Vergeten te initialiseren
}
```

**Correcte Code**:
```csharp
public ICommand MyCommand { get; set; }

public MainViewModel()
{
    MyCommand = new RelayCommand(ExecuteMethod); // ✅
}
```

### Fout 7: Property naam typo in CallerMemberName

**Symptoom**: Verkeerde property wordt geüpdatet

**Fout Code**:
```csharp
public string Name
{
    get => _name;
    set
    {
        _name = value;
        OnPropertyChanged("Naam"); // ❌ Verkeerde naam!
    }
}
```

**Correcte Code**:
```csharp
public string Name
{
    get => _name;
    set
    {
        _name = value;
        OnPropertyChanged(); // ✅ Automatisch juiste naam
    }
}
```

### Fout 8: Namespace vergeten in XAML

**Symptoom**: ViewModel niet gevonden

**Fout Code**:
```xml
<Window>
    <Window.DataContext>
        <MainViewModel /> <!-- ❌ Namespace ontbreekt! -->
    </Window.DataContext>
</Window>
```

**Correcte Code**:
```xml
<Window xmlns:vm="clr-namespace:MVVM_DEMO.ViewModels"> <!-- ✅ Namespace declaratie -->
    <Window.DataContext>
        <vm:MainViewModel /> <!-- ✅ Met prefix -->
    </Window.DataContext>
</Window>
```

### Fout 9: UpdateSourceTrigger vergeten voor real-time updates

**Symptoom**: TextBox update pas bij focus verlies

**Fout Code**:
```xml
<TextBox Text="{Binding ProductName}" /> <!-- ❌ -->
```

**Correcte Code (voor real-time)**:
```xml
<TextBox Text="{Binding ProductName, UpdateSourceTrigger=PropertyChanged}" /> <!-- ✅ -->
```

### Fout 10: Null reference bij nested binding

**Symptoom**: Crash bij opstarten of null reference exception

**Fout Code**:
```xml
<TextBox Text="{Binding SelectedProduct.ProductName}" />
```
```csharp
public Product SelectedProduct { get; set; } // null bij opstarten! ❌
```

**Oplossing 1**: Initialiseer met default waarde
```csharp
public Product SelectedProduct { get; set; } = new Product(); // ✅
```

**Oplossing 2**: Gebruik FallbackValue in binding
```xml
<TextBox Text="{Binding SelectedProduct.ProductName, FallbackValue=''}" /> <!-- ✅ -->
```

**Oplossing 3**: Initiële selectie in code
```csharp
if (comboBox.Items.Count > 0)
{
    comboBox.SelectedIndex = 0; // ✅
}
```

---

## Uitbreidingsopdrachten

Nu je de basis begrijpt, probeer deze uitbreidingen:

### Opdracht 1: Delete Product Command
Voeg een "Delete Product" button toe:
1. Maak een `DeleteProductCommand`
2. Implementeer `ExecuteDeleteProduct` method
3. Implementeer `CanExecuteDeleteProduct` (alleen als er een product geselecteerd is)
4. Voeg button toe aan XAML

**Hint**:
```csharp
private bool CanExecuteDeleteProduct(object? parameter)
{
    return SelectedProduct != null;
}
```

### Opdracht 2: Edit Product Command
Maak een dialoog om producten te bewerken:
1. Maak een nieuw Window `EditProductWindow.xaml`
2. Maak een `EditProductViewModel`
3. Gebruik `ShowDialog()` om het window te tonen
4. Sla wijzigingen op

### Opdracht 3: Value Converter
Maak een value converter voor prijs formatting:
1. Maak een class in `ValueConverters` folder
2. Implementeer `IValueConverter`
3. Converteer decimal naar string met currency symbool
4. Gebruik in binding: `{Binding Price, Converter={StaticResource PriceConverter}}`

**Voorbeeld**:
```csharp
public class PriceConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is decimal price)
        {
            return $"€ {price:F2}";
        }
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
```

### Opdracht 4: Input Validatie
Voeg validatie toe voor product naam en prijs:
1. Implementeer `IDataErrorInfo` in Product class
2. Valideer in property setters
3. Toon error messages in UI
4. Disable "Add Product" bij ongeldige input

### Opdracht 5: Geavanceerde Persistentie
Verbeter de bestaande persistentie implementatie:
1. Maak een `ProductRepository` class voor betere separation of concerns
2. Implementeer async `SaveToFileAsync()` en `LoadFromFileAsync()` methods
3. Voeg auto-save toe na elke wijziging (met debouncing)
4. Implementeer backup functionaliteit (meerdere versies bewaren)
5. Voeg export/import functionaliteit toe (CSV of XML)
6. Implementeer error recovery (corrupted file handling)

---

## Samenvatting

Je hebt nu een volledige WPF MVVM applicatie gebouwd! Je hebt geleerd:

### Patronen en Concepten:
- ✅ MVVM architectuur pattern
- ✅ INotifyPropertyChanged voor property change notification
- ✅ ICommand pattern voor button logic
- ✅ Data Binding (OneWay, TwoWay)
- ✅ ObservableCollection voor automatische UI updates
- ✅ Application Context voor applicatie-brede state
- ✅ JSON serialisatie voor data persistentie

### Implementatie Details:
- ✅ Model class met property notification
- ✅ ViewModel met commands en observable collections
- ✅ View met XAML data binding
- ✅ RelayCommand voor commando implementatie
- ✅ Application class met data beheer en lifecycle management
- ✅ Automatisch laden en opslaan van data
- ✅ Separation of concerns

### Best Practices:
- ✅ Minimale code-behind
- ✅ Testbare code
- ✅ Herbruikbare componenten
- ✅ Clean architecture

**Volgende stappen**:
1. Experimenteer met de uitbreidingsopdrachten
2. Lees de officiële Microsoft WPF documentatie
3. Bouw je eigen MVVM applicatie
4. Onderzoek MVVM frameworks zoals Prism of MVVM Light

Succes met je verdere MVVM ontwikkeling!
