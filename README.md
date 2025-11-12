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
├── Models/
│   └── Product.cs              # Product data model
├── ViewModels/
│   └── MainViewModel.cs        # Hoofd ViewModel met business logica
├── Views/
│   └── MainWindow.xaml         # Hoofd UI window
│   └── MainWindow.xaml.cs      # Code-behind voor UI events
├── ValueConverters/            # (Gereserveerd voor toekomstige converters)
├── App.xaml                    # Application configuratie
└── MVVM_DEMO.csproj           # Project configuratie
```

## Functionaliteit

De applicatie biedt de volgende functionaliteit:

1. **Productenlijst weergeven**: Een ComboBox toont alle beschikbare producten
2. **Product details bekijken**: Bij selectie worden de naam en prijs van het product getoond
3. **Product toevoegen**: Via een knop kunnen nieuwe producten met willekeurige prijzen worden toegevoegd
4. **Data Binding**: Automatische synchronisatie tussen UI en ViewModel

## MVVM Patroon Implementatie

### Model (`Models/Product.cs`)
Bevat de data structuur voor een Product met eigenschappen:
- `ProductName` (string)
- `Price` (double)

### View (`Views/MainWindow.xaml`)
De gebruikersinterface met:
- ComboBox voor productselectie
- TextBoxes voor het weergeven van productdetails
- Button om nieuwe producten toe te voegen

### ViewModel (`ViewModels/MainViewModel.cs`)
Implementeert `INotifyPropertyChanged` en beheert:
- `ObservableCollection<Product>` voor de productenlijst
- Property change notifications
- Data initialisatie via `LoadData()`

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
<TextBox Text="{Binding productName}" />
```

### INotifyPropertyChanged
Het ViewModel implementeert `INotifyPropertyChanged` om de UI te notificeren wanneer data wijzigt:
```csharp
public void OnPropertyChanged(string propertyName)
{
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
```

### ObservableCollection
Gebruikt voor de productenlijst om automatisch UI updates te triggeren bij toevoegen/verwijderen van items.

## Toekomstige Uitbreidingen

Mogelijke verbeteringen voor dit demo project:
- Implementatie van ICommand voor button clicks (in plaats van code-behind events)
- Toevoegen van ValueConverters voor data formatting
- CRUD operaties (Create, Read, Update, Delete)
- Data persistentie (database of file storage)
- Input validatie
- Unit tests voor ViewModels

## Licentie

Dit is een educatief demo project.