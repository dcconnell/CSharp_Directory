# CSharp_Directory
Directory of different C# &amp; .NET concepts, based on what I've learned about them and their best use cases

---
## Modifiers

#### Access

- public
	- Accessible from any code
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

#### Member

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

#### Inheritance / Polymorphism

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

#### Method

- async
	- Method contains `await` expressions and returns a task-like type
- extern
	- Method is implemented externally (e.g., native/unmanaged code via P/Invoke)
- partial
	- Type or method definition is split across multiple files

#### Parameter

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

#### Unsafe / Interop (My favorite)

- unsafe
	- Allows pointer operations and unmanaged memory access in the block or member
- fixed
	- Pins a managed variable so the GC won't move it (used in unsafe contexts)
- stackalloc
	- Allocates memory on the stack instead of the heap (technically an operator, often grouped here)

#### Event

- event
	- Declares a member as an event backed by a delegate

#### Struct / Ref

- ref (type only)
	- Declares a struct that must live on the stack and cannot be boxed
- readonly (on structs)
	- Guarantees no instance member mutates the struct's state
- record
	- Declares a type with value-based equality semantics and synthesized members