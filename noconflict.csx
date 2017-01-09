#r "tools/Microsoft.CodeAnalysis.CSharp.1.3.2/lib/netstandard1.3/Microsoft.CodeAnalysis.CSharp.dll"

using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

static T Read<T>(IEnumerator<T> e, string message = null)
{
    if (!e.MoveNext())
        throw new InvalidOperationException(message);
    return e.Current;
}

var dir = Environment.CurrentDirectory;

string includePattern = null;
string excludePattern = null;
var usings = new List<string>();
const string missingArgValue = "Missing argument value.";

using (var arg = Args.GetEnumerator())
{
    while (arg.MoveNext())
    {
        switch (arg.Current)
        {
            case "-i":
            case "--include":
                includePattern = Read(arg, missingArgValue);
                break;
            case "-x":
            case "--exclude":
                excludePattern = Read(arg, missingArgValue);
                break;
            case "-u":
            case "--using":
                usings.Add(Read(arg, missingArgValue));
                break;
            default:
                throw new Exception("Invalid argument: " + arg.Current);
        }
    }
}

var includePredicate
    = string.IsNullOrEmpty(includePattern)
    ? (_ => true)
    : new Func<string, bool>(new Regex(includePattern).IsMatch);

var excludePredicate
    = string.IsNullOrEmpty(excludePattern)
    ? (_ => false)
    : new Func<string, bool>(new Regex(excludePattern).IsMatch);

var q =
    from fp in Directory.EnumerateFiles(Path.Combine(dir, "MoreLinq"), "*.cs")
    select new
    {
        SourcePath = fp,
        Methods =
            from cd in
                CSharpSyntaxTree
                    .ParseText(File.ReadAllText(fp), CSharpParseOptions.Default.WithPreprocessorSymbols("MORELINQ"))
                    .GetRoot()
                    .SyntaxTree
                    .GetCompilationUnitRoot()
                    .DescendantNodes().OfType<ClassDeclarationSyntax>()
            where (string) cd.Identifier.Value == "MoreEnumerable"
            from md in cd.DescendantNodes().OfType<MethodDeclarationSyntax>()
            let mn = (string) md.Identifier.Value
            where md.ParameterList.Parameters.Count > 0
               && md.ParameterList.Parameters.First().Modifiers.Any(m => (string)m.Value == "this")
               && md.Modifiers.Any(m => (string)m.Value == "public")
               && md.AttributeLists.SelectMany(al => al.Attributes).All(a => a.Name.ToString() != "Obsolete")
               && includePredicate(mn)
               && !excludePredicate(mn)
            select md
    };

var @void = SyntaxFactory.ParseTypeName("void");

var output =
    CompilationUnit()
        .WithMembers(SingletonList<MemberDeclarationSyntax>(
            NamespaceDeclaration(IdentifierName("MoreLinq.NoConflict"))
                .WithUsings(List(
                    from nss in new []
                    {
                        new []
                        {
                            "System",
                            "System.Collections.Generic",
                        },
                        usings.AsEnumerable(),
                    }
                    from ns in nss
                    select UsingDirective(ParseName(ns))
                ))
                .WithMembers(List<MemberDeclarationSyntax>(
                    from f in q
                    from md in f.Methods
                    let call =
                        InvocationExpression(
                            MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                IdentifierName("MoreEnumerable"),
                                IdentifierName(md.Identifier)),
                            ArgumentList(SeparatedList<ArgumentSyntax>(
                                from p in md.ParameterList.Parameters
                                select Argument(IdentifierName(p.Identifier)))))
                    select ClassDeclaration(md.Identifier.Value + "Extension")
                        .WithModifiers(TokenList(SyntaxFactory.ParseTokens("public static partial")))
                        .WithLeadingTrivia(ParseLeadingTrivia($"/// <summary><c>{md.Identifier}</c> extension.</summary>{Environment.NewLine}"))
                        .WithMembers(SingletonList<MemberDeclarationSyntax>(
                            MethodDeclaration(md.ReturnType, md.Identifier)
                                .WithAttributeLists(md.AttributeLists)
                                .WithModifiers(md.Modifiers)
                                .WithTypeParameterList(md.TypeParameterList)
                                .WithConstraintClauses(md.ConstraintClauses)
                                .WithParameterList(md.ParameterList)
                                .WithBody(SyntaxFactory.Block(
                                    md.ReturnType.WithoutTrivia().IsEquivalentTo(@void)
                                    ? (StatementSyntax)ExpressionStatement(call)
                                    : ReturnStatement(call)))))))))
    .NormalizeWhitespace();

Console.WriteLine(output.ToFullString());
