using SourceGeneratorDemo.Generators;

namespace SourceGeneratorDemo.App.Models;

/// <summary>
/// The [AutoNotify] attribute on private fields causes the generator to
/// produce public properties with INotifyPropertyChanged support.
/// No hand-written boilerplate needed!
/// </summary>
public partial class ObservablePerson
{
    [AutoNotify]
    private string _firstName = string.Empty;

    [AutoNotify]
    private string _lastName = string.Empty;

    [AutoNotify]
    private int _age;
}
