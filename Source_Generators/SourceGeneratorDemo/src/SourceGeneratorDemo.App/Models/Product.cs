using SourceGeneratorDemo.Generators;

namespace SourceGeneratorDemo.App.Models;

[AutoToString]
public partial class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockCount { get; set; }
}
