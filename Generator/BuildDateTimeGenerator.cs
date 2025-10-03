#nullable enable

namespace BuildDateTimeGenerator;

using System;

using Microsoft.CodeAnalysis;

[Generator( LanguageNames.CSharp )]
public class BuildDateTimeGenerator : IIncrementalGenerator
{
  public void Initialize ( IncrementalGeneratorInitializationContext context )
  {
    DateTimeOffset now = DateTimeOffset.Now;
    const string propertyComment = "/// Gets <see cref=\"DateTimeOffset\"/>, which is set to the date and time the project was built. Since its value varies from build to build, the <see cref=\"https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-options/code-generation#deterministic\">Deterministic</see> option will be violated.";
    string propertyDeclaration = $"public static System.DateTimeOffset BuildDateTime {{ get; }} = new( {now.Ticks}, new System.TimeSpan({now.Offset.Ticks}) ); // {now:R}";

    string? rootNamespace = null;
    context.RegisterSourceOutput( context.CompilationProvider, ( spc, compilation ) =>
    {
      rootNamespace = compilation.AssemblyName;
    } );

    IncrementalValueProvider<IMethodSymbol?> entryPointProvider = context.CompilationProvider.Select( ( compilation, cancellationToken ) =>
      compilation.GetEntryPoint( cancellationToken ) );

    context.RegisterSourceOutput( entryPointProvider, ( spc, entryPoint ) =>
    {
      string source;
      if ( entryPoint == null )
      {
        if ( !string.IsNullOrEmpty( rootNamespace ) )
        {
          source =
$@"
namespace {rootNamespace}
{{
  /// <summary>
  /// Contains <see cref=""BuildDateTime""/>.
  /// </summary>
  internal static class BuildDateTimeClass
  {{
    /// <summary>
    {propertyComment}
    /// </summary>
    {propertyDeclaration}
  }}
}}
";
        }
        else
        {
          Diagnostic diagnostic = Diagnostic.Create(
              descriptor: new DiagnosticDescriptor(
                id: "BD0000",
                title: "Missing default namespace.",
                messageFormat: "No entry point and default namespace cannot be found.",
                category: "BuildDateTimeGenerator",
                defaultSeverity: DiagnosticSeverity.Error,
                isEnabledByDefault: true ),
                location: null, // Get the location of the class name
                messageArgs: null ); // Pass the class name to the message format

          spc.ReportDiagnostic( diagnostic );
          return;
        }
      }
      else
      {
        source = string.IsNullOrEmpty( entryPoint.ContainingNamespace.Name ) ?
$@"
public static partial class Program
{{
  /// <summary>
  {propertyComment}
  /// </summary>
  {propertyDeclaration}
}}
"
  :
$@"
namespace {entryPoint.ContainingNamespace.Name}
{{
  {( entryPoint.ContainingType.DeclaredAccessibility == Accessibility.NotApplicable ? "" : entryPoint.ContainingType.DeclaredAccessibility.ToString().ToLower() )} {( entryPoint.ContainingType.IsStatic ? "static " : "" )}partial {entryPoint.ContainingType.TypeKind.ToString().ToLower()} {entryPoint.ContainingType.Name}
  {{
    /// <summary>
    {propertyComment}
    /// </summary>
    {propertyDeclaration}
  }}
}}
";
      }

      spc.AddSource( "BuildDateTime.g.cs", source );
    } );
  }
}
