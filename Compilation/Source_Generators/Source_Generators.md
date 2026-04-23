## Description

Introduced in .NET 5, source generators allow you to pre-compile code sections and retrieve a compilation [[object]]. A source generator essentially is a piece of code that inspects your program and produces additional source files (indicated as \*.g.cs during compilation) at compile time reducing boilerplate and repeated overhead. [^1]

---
## Deep Dive

Check out the included SourceGeneratorDemo c# project for an executable version of the following dive.

Source generators [SG] are a useful tool in any .NET engineers toolbox. They allow for code to be auto generated at [[compile]] time for functionality that is repeatable and predictable. A [SG] is a .NET Standard 2.0 [[assembly]] that is loaded by the compiler along with any [[analyzer]]. [SG]'s seek to be a solution to the current inefficiencies introduced at runtime when utilizing runtime [[Reflection]], IL weaving, and juggling MSBuild tasks. Source generators can run [[Reflection]] at [[compile]] time, reducing runtime overhead for those specific situations. 

It is also a type-strict solution to "stringly-typed" implementations where certain logic depends on [[string]] values, like for example ASP.NET core route paths where:

*Figure A*
```csharp
[HttpGet("api/users")]
```

OR using reflection to compare enum values on a description string, essentially the Check1 string type comparer is a "stringly-typed" implementation:

*Figure B*
```csharp
public enum Role
{
	[Description("Finance")]
	Accounting,
	[Description("Board Member")]
	Leadership
}

public static string GetDescription(this Enum value)
{
	ArgumentNullException.ThrowIfNull(value);
	
	var fieldInfo = value.GetType().GetField(value.ToString());
	if (fieldInfo is not null)
	{
		var description = (DescriptionAttribute[])fieldInfo
			.GetCustomAttributes(typeof(DescriptionAttribute), false);
			
		if (description.Length > 0)
			return attributes[0].Description;
	}
	return value.ToString();
}

if (Accounting
	.GetDescription()
	.Equals("Finance", StringComparison.OrdinalIgnoreCase))
{
	DoSomething();
}
```

Because Enum's and their description attributes can be reflected on at compile time using a [SG], this allows for types to be synonymously generated and utilized preventing extra overhead during runtime. This also allows for these description strings to be considered strongly-typed after [SG] implementation. 

All [SG] need to be decorated with the **[Generator]** attribute, as well as inherit the **IIncrementalGenerator** interface which gives access to the **Initialize** method and can utilize the **IncrementalGeneratorInitializationContext**. Doing so allows the compiler to:
1. Add a source
2. Build a pipeline that finds every type annotated with the class name minus Generator *Incrementally :)*
3. Then register the output and emit a compiled source file.
4. All objects that would inherit this example below need to be declared as [[partial]]. *(See Figure H)*.

*Figure C*
```csharp
[Generator]
public class AutoToStringGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // 1) Inject the marker attribute into the compilation so users
        //    don't need a separate "attributes" package.
        context.RegisterPostInitializationOutput(static ctx =>
        {
            ctx.AddSource("AutoToStringAttribute.g.cs", SourceText.From(AttributeSource, Encoding.UTF8));
        });
        
        // 2) Build a pipeline that finds every type annotated with [AutoToString].
        IncrementalValuesProvider<TypeToGenerate> pipeline =
            context.SyntaxProvider
                .ForAttributeWithMetadataName(
                    fullyQualifiedMetadataName: "SourceGeneratorDemo.Generators.AutoToStringAttribute",
                    predicate: static (node, _) => node is TypeDeclarationSyntax,
                    transform: static (ctx, _) => GetTypeToGenerate(ctx))
                .Where(static t => t is not null)
                .Select(static (t, _) => t!.Value);
                
        // 3) Register the output — emit one source file per annotated type.
        context.RegisterSourceOutput(pipeline, static (spc, typeInfo) =>
        {
            var source = GenerateToString(typeInfo);
            spc.AddSource($"{typeInfo.FullyQualifiedName}.AutoToString.g.cs", SourceText.From(source, Encoding.UTF8));
        });
    }
}
```

In 2. you will notice it has a method *GetTypeToGenerate()*. This code determines the type of the object that will be converted to string through reflection, but the reflection occurs at compile time, which is the runtime efficiency portion that was discussed earlier.

*Figure D*
```csharp
private static TypeToGenerate? GetTypeToGenerate(GeneratorAttributeSyntaxContext context)
{
	if (context.TargetSymbol is not INamedTypeSymbol typeSymbol)
		return null;

	var properties = typeSymbol
		.GetMembers()
		.OfType<IPropertySymbol>()
		.Where(p => p.DeclaredAccessibility == Accessibility.Public && !p.IsStatic)
		.Select(p => p.Name)
		.ToImmutableArray();

	var ns = typeSymbol.ContainingNamespace.IsGlobalNamespace
		? null
		: typeSymbol.ContainingNamespace.ToDisplayString();

	return new TypeToGenerate(
		Name: typeSymbol.Name,
		Namespace: ns,
		FullyQualifiedName: typeSymbol.ToDisplayString().Replace(".", "_"),
		IsRecord: typeSymbol.IsRecord,
		TypeKind: typeSymbol.TypeKind == TypeKind.Struct ? "struct" : "class",
		Properties: properties);
}
```

Then finally in 3. if you notice, there is the *GenerateToString()* method call that actually uses the [[Ahead of Time (AOT)]] compiled .ToString() method. Which is basically just a StringBuilder of the file itself. You can define how the string will look based off your own personal implementation.

*Figure E*
```csharp
private static string GenerateToString(TypeToGenerate type)
{
	var sb = new StringBuilder();
	sb.AppendLine("// <auto-generated />");
	sb.AppendLine("#nullable enable");
	sb.AppendLine();

	if (type.Namespace is not null)
	{
		sb.AppendLine($"namespace {type.Namespace}");
		sb.AppendLine("{");
	}

	sb.AppendLine($"    partial {type.TypeKind} {type.Name}");
	sb.AppendLine("    {");
	sb.AppendLine("        public override string ToString()");
	sb.AppendLine("        {");

	if (type.Properties.Length == 0)
	{
		sb.AppendLine($"            return \"{type.Name} {{}}\";");
	}
	else
	{
		sb.Append($"            return $\"{type.Name} {{{{ ");

		for (int i = 0; i < type.Properties.Length; i++)
		{
			var prop = type.Properties[i];
			if (i > 0) sb.Append(", ");
			sb.Append($"{prop} = {{{prop}}}");
		}

		sb.AppendLine(" }}\";");
	}

	sb.AppendLine("        }");
	sb.AppendLine("    }");

	if (type.Namespace is not null)
	{
		sb.AppendLine("}");
	}

	return sb.ToString();
}
```

There will also need to be a corresponding reference inside of the .csproj file that will be using the [SG] as an [[analyzer]].

*Figure F*
```xml
<ItemGroup>
	<ProjectReference Include="..\SourceGeneratorDemo.Generators\SourceGeneratorDemo.Generators.csproj"
                      OutputItemType="Analyzer"
                      ReferenceOutputAssembly="false" />
</ItemGroup>
```

I feel this is a good time to mention that if you would like to see exactly what the files look like that are generated, you can add this to your .csproj Property Group as well. After compiling the files will be located in *obj\Debug\net{version#}}\generated*. This is great for debugging since looking at the string representation is fairly unpleasant.

*Figure G*
```xml
<PropertyGroup>
	<EmitCompilerGeneratedFiles>true</EmitCompilerGeneratedFiles>
</PropertyGroup>
```

The generated files look like what is shown below, as you can see, the compiler essentially created partial classes that associate to the original models with compiler built .ToString() method overrides for those specific models. I've also included the associated Model class for reference and context.

*Figure H*
```csharp
// AutoToStringAttribute.g.cs
// <auto-generated />
namespace SourceGeneratorDemo.Generators
{
    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct)]
    internal sealed class AutoToStringAttribute : System.Attribute { }
}

// Person.cs
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

// SourceGeneratorDemo_App_Models_Person.AutoToString.g.cs
// <auto-generated />
#nullable enable
namespace SourceGeneratorDemo.App.Models
{
    partial class Person
    {
        public override string ToString()
        {
            return $"Person {{ FirstName = {FirstName}, LastName = {LastName}, Age = {Age}, Email = {Email} }}";
        }
    }
}

// Product.cs
[AutoToString]
public partial class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockCount { get; set; }
}

// SourceGeneratorDemo_App_Models_Product.AutoToString.g.cs
// <auto-generated />
#nullable enable
namespace SourceGeneratorDemo.App.Models
{
    partial class Product
    {
        public override string ToString()
        {
            return $"Product {{ Id = {Id}, Name = {Name}, Price = {Price}, StockCount = {StockCount} }}";
        }
    }
}

// accessed by calling
Person.ToString();
Product.ToString();
```

There is a separate inheritance of **ISourceGenerator** that is more or less depreciated and not recommended to use, but is the primary example in the .NET blog back from .NET 5[^1]. I'll list their example here for reference:

*Figure I*
```csharp
namespace SourceGeneratorSamples
{
    [Generator]
    public class HelloWorldGenerator : ISourceGenerator
    {
        public void Execute(SourceGeneratorContext context)
        {
            // begin creating the source we'll inject into the users compilation
            var sourceBuilder = new StringBuilder(@"
			using System;
			namespace HelloWorldGenerated
			{
			    public static class HelloWorld
			    {
			        public static void SayHello() 
			        {
			            Console.WriteLine(""Hello from generated code!"");
			            Console.WriteLine(""The following syntax trees existed in the compilation that created this program:"");
			");

            // using the context, get a list of syntax trees in the users compilation
            var syntaxTrees = context.Compilation.SyntaxTrees;

            // add the filepath of each tree to the class we're building
            foreach (SyntaxTree tree in syntaxTrees)
            {
                sourceBuilder.AppendLine($@"Console.WriteLine(@"" - {tree.FilePath}"");");
            }

            // finish creating the source to inject
            sourceBuilder.Append(@"
			        }
			    }
			}");

            // inject the created source into the users compilation
            context.AddSource("helloWorldGenerator", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
        }

        public void Initialize(InitializationContext context)
        {
            // No initialization required for this one
        }
    }
}

// accessed by calling
HelloWorldGenerated.HelloWorld.SayHello();
```

This is essentially what is happening in the System.Text.Json and why it's more efficient at runtime that other JSON serialization libraries. To use it a [SG] with System.Text.Json simply:
1. Create a partial class that derives from [JsonSerializerContext](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.serialization.jsonserializercontext).
2. Specify the type to serialize or deserialize by applying [JsonSerializableAttribute](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.serialization.jsonserializableattribute) to the context class.
3. 1. Call a [JsonSerializer](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.jsonserializer) method that either: 
	1. Takes a [JsonTypeInfo](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.serialization.metadata.jsontypeinfo-1) instance, or 
	2. Takes a [JsonSerializerContext](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.serialization.jsonserializercontext) instance, or
	3. Takes a [JsonSerializerOptions](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.jsonserializeroptions) instance and you've set its [JsonSerializerOptions.TypeInfoResolver](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.jsonserializeroptions.typeinforesolver#system-text-json-jsonserializeroptions-typeinforesolver) property to the [[default]] property of the context type.

Below is an example [^2]:

*Figure J*
```csharp
// WeatherForecast.cs
public class WeatherForecast
{
    public DateTime Date { get; set; }
    public int TemperatureCelsius { get; set; }
    public string? Summary { get; set; }
}

// SourceGenerationContext.cs
[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(WeatherForecast))]
internal partial class SourceGenerationContext : JsonSerializerContext { }

// source generated with any of these serialized/deserialized calls
// Serialization
// directly using JsonTypeInfo<T>
jsonString = JsonSerializer.Serialize( weatherForecast!, SourceGenerationContext.Default.WeatherForecast);

// using JsonSerializerContext
jsonString = JsonSerializer.Serialize(weatherForecast, typeof(WeatherForecast), SourceGenerationContext.Default);

// cleanest imo, deriving the default serialization option from the default of the declared type in .Serialize<WeatherForecase>
sourceGenOptions = new JsonSerializerOptions 
{ 
	TypeInfoResolver = SourceGenerationContext.Default 
};
jsonString = JsonSerializer.Serialize<WeatherForecast>(weatherForecast, sourceGenOptions)

// Deserialization, same 3 examples
// directly using JsonTypeInfo<T>
weatherForecast = JsonSerializer.Deserialize(jsonString, SourceGenerationContext.Default.WeatherForecast);

// using JsonSerializerContext
weatherForecast = JsonSerializer.Deserialize(jsonString, typeof(WeatherForecast), SourceGenerationContext.Default) as WeatherForecast;

// cleanest imo, deriving the default serialization option from the default of the declared type in .Serialize<WeatherForecase>
var sourceGenOptions = new JsonSerializerOptions
{
    TypeInfoResolver = SourceGenerationContext.Default
};
weatherForecast = JsonSerializer.Deserialize<WeatherForecast>(jsonString, sourceGenOptions);
```

Seeing as how JSON is the predominate language between frontends and backends you can see why the writers of System.Text.Json felt it appropriate to utilize [SG].

---
## References
[^1]: [Introducing C# Source Generators - .NET Blog](https://devblogs.microsoft.com/dotnet/introducing-c-source-generators/)
[2]:[How to use source generation in System.Text.Json - .NET | Microsoft Learn](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/source-generation)