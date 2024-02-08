# BuildDateTime Generator
This simple C# source code generator adds the `DateTimeOffset BuildDateTime { get; }` property to the compiled project. The property gets the [DateTimeOffset](https://learn.microsoft.com/cs-cz/dotnet/api/system.datetimeoffset) value which is set to the date and time the project was built. 

The way the property is accessible depends on the project:

**1.** **`Main()` style entry point application**
The property is added to the class that contains the `Main()` method. The class must be marked as `partial`.

<pre><code>namespace ApplicationNamespace
{
  internal <i>partial</i> class Program
  {
    private static void Main ()
    {
      Console.WriteLine( <b>BuildDateTime</b> );
    }
  }
}</code></pre>

**2.** **Top-level entry point application**
The property is added to the top-level of the application. The usage is straightforward:

<pre><code>Console.WriteLine( <b>BuildDateTime</b> );</code></pre>

**3.** **Class library**
The property is added to a new class `BuildDateTimeClass` in the default namespace of the library.

<pre><code>namespace ClassLibrary
{
  using System;

  public class Class
  {
    public static DateTimeOffset BuildDateTimeUsage { get; } = <b>BuildDateTimeClass.BuildDateTime</b>;
  }
}</code></pre>

> :memo: Because the value of the `BuildDateTime` property varies from build to build, the [Deterministic](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-options/code-generation#deterministic) option, if set, is violated.
