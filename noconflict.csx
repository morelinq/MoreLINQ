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

var indent = new string('\x20', 4);
var indent2 = indent + indent;
var indent3 = indent2 + indent;

var imports =
    from nss in new[]
    {
        new []
        {
            "System",
            "System.CodeDom.Compiler",
            "System.Collections.Generic",
        },
        usings.AsEnumerable(),
    }
    from ns in nss
    select indent + $"using {ns};";

var classes =
    from f in q
    from md in f.Methods
    group md by (string) md.Identifier.Value into g
    select new
    {
        Name = g.Key,
        Overloads =
            from md in g
            select
                MethodDeclaration(md.ReturnType, md.Identifier)
                    .WithAttributeLists(md.AttributeLists)
                    .WithModifiers(md.Modifiers)
                    .WithTypeParameterList(md.TypeParameterList)
                    .WithConstraintClauses(md.ConstraintClauses)
                    .WithParameterList(md.ParameterList)
                    .WithExpressionBody(
                        ArrowExpressionClause(
                            InvocationExpression(
                                MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    IdentifierName("MoreEnumerable"),
                                    IdentifierName(md.Identifier)),
                                ArgumentList(SeparatedList<ArgumentSyntax>(
                                    from p in md.ParameterList.Parameters
                                    select Argument(IdentifierName(p.Identifier)))))
                                .WithLeadingTrivia(Space))
                            .WithLeadingTrivia(Whitespace(indent3)))
                    .WithSemicolonToken(ParseToken(";").WithTrailingTrivia(LineFeed))
    }
    into m
    select $@"
    /// <summary><c>{m.Name}</c> extension.</summary>

    [GeneratedCode(""MoreLinq.NoConflictScript"", ""0.1"")]
    public static class {m.Name}Extension
    {{
{string.Join(null, from mo in m.Overloads select mo.ToFullString())}
    }}";

var template = $@"
#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (C) 2008 Jonathan Skeet.
// Portions Copyright (C) 2009 Atif Aziz, Chris Ammerman, Konrad Rudolph.
// Portions Copyright (C) 2010 Johannes Rudolph, Leopold Bushkin.
// Portions Copyright (C) 2015 Felipe Sateler, ""sholland"".
// Portions Copyright (C) 2016 Leandro F. Vieira (leandromoh).
// Portions Copyright (C) Microsoft. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the ""License"");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an ""AS IS"" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

// This code was generated by a tool. Any changes made manually will be lost
// the next time this code is regenerated.
// Generated: {DateTimeOffset.Now.ToString("r")}

namespace MoreLinq.NoConflict
{{
{string.Join("\n", imports)}
{string.Join("\n", classes)}
}}
";

Console.WriteLine(template.Trim()
                          // normalize line endings
                          .Replace("\r", string.Empty)
                          .Replace("\n", Environment.NewLine));
