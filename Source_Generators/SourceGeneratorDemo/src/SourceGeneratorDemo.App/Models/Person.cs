using SourceGeneratorDemo.Generators;

namespace SourceGeneratorDemo.App.Models;

/// <summary>
/// The [AutoToString] attribute triggers the source generator to produce
/// a ToString() override that prints all public properties.
/// This class MUST be partial so the generator can extend it.
/// </summary>
[AutoToString]
public partial class Person
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Email { get; set; } = string.Empty;
}
