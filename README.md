
# PointyBoot

A C# version of SpringBoot Depenedency management framework.
      
- This is just a DI framework to solve DI for functions, properties and classes like how SpringBoot works. Needs more performance improvement.
- The name derives from C#'s Sharp and SpringBoot's Boot = SharpBoot = PointyBoot :stuck_out_tongue_winking_eye: 

### Goal
This project is inspired from the DI model of SpringBoot project. It does not intend to be exactly same nor is even close to the vastness of that project but tries to implement its core ideas in C# .NET. It focuses on providing DI in below segments of a application code -

- Properties
- Classes or Constructors
- Functions [coming soon]

### Features
- Supports session based DI service with transient singletons (lasting only for a session) like supported in ASP.NET Core DI with 'AddTransient()' function. This is facilitated by PBContext.
- Each session (context) can have its own factory and class type to interface mapping.
- Factory function and factory classes with automatic ditection of serving types.
- Support for Singleton.

### Run and test
This project is developed in VS2019 so you can use that or any latest version available. Or we can use VS Code editor with .NET developer toolset.

### Status
Works with the basic use-cases and needs more improvements and handling for various error cases.
Supports out of the box sessions to implement one's one Transient (lasting only for a session) feature like ASP.NET Core DI. This is facilitated by PBContext.
No support for Generics yet but would be supported eventually.

### Based on
Plain C# with Reflection.
