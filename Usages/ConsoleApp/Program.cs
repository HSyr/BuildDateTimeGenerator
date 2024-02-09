// Note: This example project does not reference BuildDateTimeGenerator from the NuGet package but as a Project Reference.
namespace ConsoleNetFramework
{
  internal partial class Program
  {
    private static void Main ()
    {
      Console.WriteLine( BuildDateTime );
      Console.WriteLine( ClassLibrary.Class.BuildDateTime );
    }
  }
}
