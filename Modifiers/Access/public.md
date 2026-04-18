## Description

The public keyword is used to allow entire solution access to that specific type or member. If you have several dll's/assemblies/projects (whatever term is best for you) that share a project reference, all `public` types/members are accessible. It allows for total no permission needed use of this type/member.

---
## Deep Dive

*Figure A*
```csharp
public class Example
{
	public int X;
}
```

Observe in *Figure A* above how the `public` access modifier is enlisted for use in both the type declaration of `public class` and `public int`. Using `public` with type allows for this class to be accessed and initialized in any project that references the assembly this class is housed within. Consequently, it's publicly facing member `x` here is also allowed to be utilized and accessed by any project that references it's parent class.

*Figure B*
```csharp
public class AnyoneCanUseMe
{
	public int X;
	public string Y;
}

~ Same Project or Some Other Project That References AnyoneCanUseMe
class IllUseYou
{
	var anyoneCanUseMe = new AnyoneCanUseMe();
	anyoneCanUseMe.X = 5;
	anyoneCanUseMe.Y = "Hello";
	
	Console.PrintLine($"{anyoneCanUseMe.X} - {anyoneCanUseMe.Y}");
}
```

As you can see in *Figure B* this allows for anyone to access a [[Classes]] and it's members. While implementing this with a [[New]] keyword declaration is safe in most code scenarios, it could prove detrimental with the situations outlined next.

---
## Situations to Avoid With Examples

##### Problem
- Imagine you are running a microservice with a singleton lifetime on a AnyoneCanUseMeService with `public` members. This would allow for several areas of code to access and modify it's members in an unsafe manor. If you set AnyoneCanUseMeService.X = 5, then another async thread from the threadpool sets AnyoneCanUseMeService.X = -1 before you get the chance to re-access X resulting in it being the wrong value. This breaks [[Encapsulation]] and exposes internal state in an uncontrollable manor.
```csharp
// ASP.Net Program.cs
builder.Services.AddSingleton<IAnyoneCanUseMeService, AnyoneCanUseMeService>();
builder.Services.AddScoped<IAdder, Adder>();
builder.Services.AddScoped<IMultiplier, Multiplier>();

// AnyoneCanUseMeService Singleton service
public class AnyoneCanUseMe
{
	public int X;
	public string Y;
}

// Adder Scoped service
public class Adder
{
	public async Task AddAmount(int amount)
	{
		AnyoneCanUseMeService.X += amount;
	}
	
	public async Task<int> ReadAmount()
	{
		return AnyoneCanUseMeService.X;
	}
}

// Multiplier
public class Multiplier
{
	public async Task MultiplyAmount(int amount)
	{
		AnyoneCanUseMeService.X *= amount;
	}
}

// Consequential Project Flow
// Thread 1
Adder.AddAmount(5);
// Thread 2
Multiplier.MultiplyAmount(2);
// Thread 1
Adder.ReadAmount();
```
##### Solution
Either expose the publicly accessible members using types like [[ConcurrentList]], [[ConcurrentDictionary]], [[ConcurrentQueue]], [[ConcurrentBag]], ect. or keep the members [[private]]. You can also set publicly accessible setters that have validation logic to ensure the [[Properties]]/[[Fields]] should change and to ensure it is [[Covariant]].

##### Problem
- Implementing `public` members with business secrets or internal information stored in property/field strings.
```csharp
public class AnyoneCanUseMe
{
	public string apiKey = "you-wish-loser";
}
```
##### Solution
Don't do it.

##### Problem
- Implementing public implementation details, essentially leaking valuable service code.
```csharp
public class EmailService
{
	// consumers now depend on SMTP specifically
    public SmtpClient SmtpClient { get; set; } 
}
```

##### Solution
Keep the transport mechanisms [[private]], only expose members that are necessary to utilize the class from a high level. Prevents future breaks should `SmtpClient` change to `SendGrind` or some other implementation.

##### Problem
- Unintentional API Surface via a method or class that needs to remain secure and internal
```csharp
public class UserService
{
	// now every consumer can call this
    public string HashPassword(string raw) { ... } 
}
```

##### Solution
Once something is `public` in a library/package, removing or renaming it is a breaking change. Keep helpers [[private]] or [[internal]] unless they're intentionally part of your contract.

##### Problem
- [[Inheritance]] Abuse via several derived [[Classes]] when a base depends upon a value being controlled directly.
```csharp
public class BaseService
{
	// any derived class (or anyone) can swap the DbContext
    public DbContext Db { get; set; }
}

public class DerivedService : BaseService
{
	public void ChangeDbContext(IDbContext db)
	{
		Db = db;
	}
}
```

##### Solution
Use [[protected]] (derived class access only) or [[private]] with constructor injection forcing [[Encapsulation]].