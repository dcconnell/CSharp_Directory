using SourceGeneratorDemo.App.Models;

Console.WriteLine("=== Source Generator Demo ===");
Console.WriteLine();

// -------------------------------------------------------
// Demo 1: AutoToString Generator
// -------------------------------------------------------
Console.WriteLine("--- [AutoToString] Generator ---");
Console.WriteLine();

var person = new Person
{
    FirstName = "Alice",
    LastName = "Smith",
    Age = 30,
    Email = "alice@example.com"
};

// This ToString() was generated at compile time — no reflection at runtime.
Console.WriteLine(person);

var product = new Product
{
    Id = 42,
    Name = "Mechanical Keyboard",
    Price = 149.99m,
    StockCount = 25
};

Console.WriteLine(product);
Console.WriteLine();

// -------------------------------------------------------
// Demo 2: AutoNotify Generator (INotifyPropertyChanged)
// -------------------------------------------------------
Console.WriteLine("--- [AutoNotify] Generator (INotifyPropertyChanged) ---");
Console.WriteLine();

var observable = new ObservablePerson();

// Subscribe to the generated PropertyChanged event.
observable.PropertyChanged += (sender, e) =>
{
    Console.WriteLine($"  PropertyChanged fired: {e.PropertyName}");
};

Console.WriteLine("Setting FirstName = \"Bob\":");
observable.FirstName = "Bob";

Console.WriteLine("Setting LastName = \"Johnson\":");
observable.LastName = "Johnson";

Console.WriteLine("Setting Age = 25:");
observable.Age = 25;

Console.WriteLine("Setting Age = 25 again (should NOT fire — same value):");
observable.Age = 25;

Console.WriteLine();
Console.WriteLine($"Observable state: {observable.FirstName} {observable.LastName}, age {observable.Age}");
