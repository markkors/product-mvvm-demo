# MVVM_DEMO

Een demonstratieproject dat het Model-View-ViewModel (MVVM) ontwerppatroon illustreert in een WPF (Windows Presentation Foundation) applicatie.

## Overzicht

Dit project demonstreert de implementatie van het MVVM-patroon in een WPF-applicatie met een eenvoudige productenbeheer interface. De applicatie toont hoe data binding, observable collections en property change notifications werken binnen het MVVM-framework.

## Technologie Stack

- **.NET 8.0** - Target framework
- **WPF (Windows Presentation Foundation)** - UI framework
- **C#** - Programmeertaal
- **XAML** - Markup language voor UI

## Projectstructuur

```
MVVM_DEMO/
├── Commands/
│   └── RelayCommand.cs         # ICommand implementatie voor MVVM
├── Models/
│   └── Product.cs              # Product data model met INotifyPropertyChanged
├── ViewModels/
│   └── MainViewModel.cs        # Hoofd ViewModel met business logica
├── Views/
│   └── MainWindow.xaml         # Hoofd UI window
│   └── MainWindow.xaml.cs      # Code-behind (minimaal door command pattern)
├── App.xaml                    # Application configuratie
├── App.xaml.cs                 # Application logica en data persistentie
└── MVVM_DEMO.csproj           # Project configuratie
```

## Functionaliteit

De applicatie biedt de volgende functionaliteit:

1. **Productenlijst weergeven**: Een ComboBox toont alle beschikbare producten
2. **Product details bekijken**: Bij selectie worden de naam en prijs van het product getoond
3. **Product toevoegen**: Via een command gebonden knop kunnen nieuwe producten met willekeurige prijzen worden toegevoegd
4. **Data Binding**: Automatische synchronisatie tussen UI en ViewModel
5. **Data Persistentie**: Products worden automatisch opgeslagen in JSON format bij afsluiten en geladen bij opstarten

## MVVM Patroon Implementatie

### Model (`Models/Product.cs`)
Bevat de data structuur voor een Product met eigenschappen:
- `ProductName` (string)
- `ProductPrice` (decimal)

Implementeert `INotifyPropertyChanged` voor automatische UI updates bij property wijzigingen.

### View (`Views/MainWindow.xaml`)
De gebruikersinterface met:
- ComboBox voor productselectie
- TextBoxes voor het weergeven van productdetails
- Button gebonden aan een ICommand om nieuwe producten toe te voegen

### ViewModel (`ViewModels/MainViewModel.cs`)
Implementeert `INotifyPropertyChanged` en beheert:
- Referentie naar `App.products` (ObservableCollection in Application context)
- `SelectedProduct` property met change notification
- `AddProductCommand` (ICommand) voor het toevoegen van producten
- Data initialisatie via `LoadData()`

### Commands (`Commands/RelayCommand.cs`)
Implementatie van het `ICommand` interface voor MVVM command binding:
- Ondersteunt `Execute` en `CanExecute` delegates
- Integreert met WPF's `CommandManager` voor automatische CanExecute updates

### Application Context (`App.xaml.cs`)
Beheert applicatie-brede state:
- `products` ObservableCollection beschikbaar via `App.products`
- JSON serialisatie bij applicatie afsluiten
- JSON deserialisatie bij applicatie opstarten
- Opslag locatie: `%AppData%\MVVM_DEMO\products.json`

## Hoe te gebruiken

### Vereisten
- Visual Studio 2022 of hoger
- .NET 8.0 SDK geïnstalleerd

### Project uitvoeren

1. Clone of download dit repository
2. Open `MVVM_DEMO.sln` in Visual Studio
3. Druk op F5 of klik op "Start" om de applicatie te bouwen en uit te voeren

### Functionaliteiten testen

1. **Producten bekijken**: Selecteer een product uit de dropdown
2. **Details bekijken**: De naam en prijs verschijnen automatisch in de tekstvelden
3. **Product toevoegen**: Klik op "Add Product" om een nieuw product toe te voegen met een willekeurige prijs tussen 10 en 100

## Belangrijke Concepten

### Data Binding
De applicatie gebruikt WPF data binding om de UI automatisch te synchroniseren met het ViewModel:
```xaml
<ComboBox ItemsSource="{Binding Products}" />
<TextBox Text="{Binding SelectedProduct.ProductName}" />
```

### INotifyPropertyChanged
Zowel het Model als ViewModel implementeren `INotifyPropertyChanged` om de UI te notificeren wanneer data wijzigt:
```csharp
public void OnPropertyChanged([CallerMemberName] string propertyName = null)
{
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
```

### ObservableCollection
Gebruikt voor de productenlijst om automatisch UI updates te triggeren bij toevoegen/verwijderen van items. De collectie wordt beheerd in de Application context (`App.products`).

### ICommand Pattern
Commands worden gebruikt voor UI acties in plaats van code-behind event handlers:
```xaml
<Button Command="{Binding AddProductCommand}" Content="Add Product" />
```
```csharp
AddProductCommand = new RelayCommand(ExecuteAddProduct, CanExecuteAddProduct);
```

### Data Persistentie
Products worden automatisch opgeslagen en geladen via JSON serialisatie:
- **Bij afsluiten**: `App.OnAppExit` serialiseert de productenlijst naar JSON
- **Bij opstarten**: `App.loadData` deserialiseert de productenlijst vanuit JSON
- **Opslag**: `%AppData%\MVVM_DEMO\products.json`

## Toekomstige Uitbreidingen

Mogelijke verbeteringen voor dit demo project:
- Toevoegen van ValueConverters voor data formatting (bijv. currency formatting)
- Uitbreiden van CRUD operaties (Update en Delete functionaliteit)
- Input validatie voor nieuwe producten
- Error handling en gebruikersfeedback verbeteren
- Unit tests voor ViewModels en Commands
- Dependency Injection voor loosely coupled architectuur
- Asynchrone data operaties voor betere performance

## Licentie

Dit is een educatief demo project.