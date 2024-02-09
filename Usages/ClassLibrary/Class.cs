// Note: This example project does not reference BuildDateTimeGenerator from the NuGet package but as a Project Reference.
namespace ClassLibrary
{
  using System;

  public class Class
  {
    public static DateTimeOffset BuildDateTime { get; } = BuildDateTimeClass.BuildDateTime;
  }
}
