# CSharp_Directory
Directory of different C# &amp; .NET concepts, based on what I've learned about them and their best use cases. If you enjoyed reading this please give it a star 🌟

---
## How to read:

These markdown files are intended to be read with [Obsidian](https://obsidian.md/download), a markdown-specific editor/note taking application. They can be read easily on [Github](https://github.com/dcconnell/CSharp_Directory) as well, just letting you know how they're being written and intended to be viewed if there are any formatting anomalies. I will footnote any and all references I come across while researching. There will also be code examples of implementations and sometimes just full .cs files about the topic if I feel that it's easier to explain through a larger code example.

Follow these steps to read the CSharp directory in Obsidian:
1. Download  [Obsidian](https://obsidian.md/download)
2. Pull down this repo, I.E. open terminal and git clone https://github.com/dcconnell/CSharp_Directory.git Or use [Github Desktop](https://desktop.github.com/download/) for a GUI experience
3. Open  [Obsidian](https://obsidian.md/download) and select "Open folder as Vault"
4. Open the cloned repository folder into  [Obsidian](https://obsidian.md/download)

# Directory Structure:

## Section Title

### Grouping Name
- C# construct
	- Brief Description for as a quick-reference
	- Link to full article going in depth (if not present I haven't written one yet, but since it's listed on the directory it will have an article)

## Hardline between Sections

---
## Concepts and Terminology

- Contravariant
	- `Concepts_and_Terminology/Contravariant.md` [[Contravariant]]
- Covariant
	- `Concepts_and_Terminology/Covariant.md` [[Covariant]]
- Encapsulation
	- `Concepts_and_Terminology/Encapsulation.md` [[Encapsulation]]
- Inheritance
	- `Concepts_and_Terminology/Inheritance.md` [[Inheritance]]
- Invariant
	- `Concepts_and_Terminology/Invariant.md` [[Invariant]]

---
## Keywords

- new
	- `Keywords/New.md` [[New]]

---
## Members

- fields
	- `Members/Fields.md` [[Fields]]
- methods
	- `Members/Methods.md` [[Methods]]
- properties
	- `Members/Properties.md` [[Properties]]

---
## Modifiers

### Access

- public
	- Accessible from any code
	- `Modifiers/Access/public.md` [[public]]
- internal
	- Accessible within the same assembly
- private
	- Accessible only within the containing type
- protected
	- Accessible within the containing type and derived types
- protected internal
	- Both protected & internal apply, accessible within the same assembly or from derived types
- private protected
	- Both private and protected apply, accessible within the containing type or derived types in the same assembly
- file
	- Scopes a top-level type's visibility to the file it's declared in

### Member

- static
	- Belongs to the type itself rather than an instance
- readonly
	- Field can only be assigned at declaration or in a constructor
- const
	- Compile-time constant; value is baked in at every usage site
- volatile
	- Field may be modified by multiple threads; prevents compiler optimizations
- required
	- Property or field must be set via an object initializer by the caller

### Inheritance / Polymorphism

- abstract
	- Member has no implementation and must be overridden; or type cannot be instantiated
- virtual
	- Member can be overridden in a derived class
- override
	- Member provides a new implementation of an inherited virtual/abstract member
- sealed
	- Prevents a class from being inherited or an override from being further overridden
- new
	- Explicitly hides an inherited member (suppresses compiler warning)

### Method

- async
	- Method contains `await` expressions and returns a task-like type
- extern
	- Method is implemented externally (e.g., native/unmanaged code via P/Invoke)
- partial
	- Type or method definition is split across multiple files

### Parameter

- ref
	- Argument is passed by reference; must be initialized before the call
- out
	- Argument is passed by reference; must be assigned inside the method
- in
	- Argument is passed by readonly reference (no copies, no mutation)
- params
	- Allows passing a variable number of arguments as an array (or span in C# 13)
- this
	- First parameter of an extension method, specifying the type being extended
- scoped
	- Restricts the lifetime of a ref struct value to the current method

### Unsafe / Interop (My favorite)

- unsafe
	- Allows pointer operations and unmanaged memory access in the block or member
- fixed
	- Pins a managed variable so the GC won't move it (used in unsafe contexts)
- stackalloc
	- Allocates memory on the stack instead of the heap (technically an operator, often grouped here)

### Event

- event
	- Declares a member as an event backed by a delegate

### Struct / Ref

- ref (type only)
	- Declares a struct that must live on the stack and cannot be boxed
- readonly (on structs)
	- Guarantees no instance member mutates the struct's state
- record
	- Declares a type with value-based equality semantics and synthesized members

---
## Types

### Concurrent

- ConcurrentBag
	- `Concurrent/ConcurrentBag.md` [[ConcurrentBag]]
- ConcurrentDictionary
	- `Concurrent/ConcurrentDictionary.md` [[ConcurrentDictionary]]
- ConcurrentList
	- `Concurrent/ConcurrentList.md` [[ConcurrentList]]
- ConcurrentQueue
	- `Concurrent/ConcurrentQueue.md` [[ConcurrentQueue]]

### Custom Types

- Classes
	- `Custom_Types/Classes.md` [[Classes]]
- Records
	- `Custom_Types/Records.md` [[Records]]
- Structs
	- `Custom_Types/Structs.md` [[Structs]]

---
## Source Generators

- Source Generators
	- Source_Generators/Source_Generators.md [[Source_Generators]]

---
