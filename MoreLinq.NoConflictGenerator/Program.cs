#region Copyright (C) 2017 Atif Aziz. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion

namespace MoreLinq.NoConflictGenerator
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

    static class Program
    {
        static void Run(IEnumerable<string> args)
        {
            var dir = Directory.GetCurrentDirectory();

            string includePattern = null;
            string excludePattern = null;
            var usings = new List<string>();
            var noClassLead = false;

            Exception MissingArgValue() =>
                new InvalidOperationException("Missing argument value.");

            using (var arg = args.GetEnumerator())
            {
                while (arg.MoveNext())
                {
                    switch (arg.Current)
                    {
                        case "-i":
                        case "--include":
                            includePattern = Read(arg, MissingArgValue);
                            break;
                        case "-x":
                        case "--exclude":
                            excludePattern = Read(arg, MissingArgValue);
                            break;
                        case "-u":
                        case "--using":
                            usings.Add(Read(arg, MissingArgValue));
                            break;
                        case "--no-class-lead":
                            noClassLead = true;
                            break;
                        case "":
                            continue;
                        default:
                            dir = arg.Current[0] != '-'
                                ? arg.Current
                                : throw new Exception("Invalid argument: " + arg.Current);
                            break;
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

            var thisAssemblyName = typeof(Program).GetTypeInfo().Assembly.GetName();

            var q =
                from fp in Directory.EnumerateFiles(dir, "*.cs")
                where !excludePredicate(fp) && includePredicate(fp)
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
                        select md
                };

            var indent = new string('\x20', 4);
            var indent2 = indent + indent;
            var indent3 = indent2 + indent;

            var baseImports = new []
            {
                "System",
                "System.CodeDom.Compiler",
                "System.Collections.Generic",
            };

            var imports =
                from ns in baseImports.Concat(usings)
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
                                            ArgumentList(SeparatedList(
                                                from p in md.ParameterList.Parameters
                                                select Argument(IdentifierName(p.Identifier)))))
                                            .WithLeadingTrivia(Space))
                                        .WithLeadingTrivia(Whitespace(indent3)))
                                .WithSemicolonToken(ParseToken(";").WithTrailingTrivia(LineFeed))
                }
                into m
                select (!noClassLead ? $@"
    /// <summary><c>{m.Name}</c> extension.</summary>

    [GeneratedCode(""{thisAssemblyName.Name}"", ""{thisAssemblyName.Version}"")]" : null) + $@"
    public static partial class {m.Name}Extension
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
        }

        static T Read<T>(IEnumerator<T> e, Func<Exception> errorFactory = null)
        {
            if (!e.MoveNext())
                throw errorFactory?.Invoke() ?? new InvalidOperationException();
            return e.Current;
        }

        static int Main(string[] args)
        {
            try
            {
                Run(args);
                return 0;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.ToString());
                return 0xbad;
            }
        }
    }
}
