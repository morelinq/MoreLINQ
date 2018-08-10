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

namespace MoreLinq.ExtensionsGenerator
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Globalization;
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
            var debug = false;
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
                        case "-d":
                        case "--debug":
                            debug = true;
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

            Func<string, bool> PredicateFromPattern(string pattern, bool @default) =>
                string.IsNullOrEmpty(pattern)
                ? delegate { return @default; }
                : new Func<string, bool>(new Regex(pattern).IsMatch);

            var includePredicate = PredicateFromPattern(includePattern, true);
            var excludePredicate = PredicateFromPattern(excludePattern, false);

            var thisAssemblyName = typeof(Program).GetTypeInfo().Assembly.GetName();

            //
            // Type abbreviations are used to abbreviate all generic type
            // parameters into a letter from the alphabet. So for example, a
            // method with generic type parameters `TSource` and `TResult` will
            // become `a` and `b`. This is used later for sorting and stabilizes
            // the sort irrespective of how the type parameters are named or
            // renamed in the source.
            //

            var abbreviatedTypeNodes = Enumerable
                .Range(0, 26)
                .Select(a => (char) ('a' + a))
                .Select(ch => new SimpleTypeKey(ch.ToString()))
                .ToArray();

        var q =

            from ms in new[]
            {
                from fp in Directory.EnumerateFiles(dir, "*.cs")
                where !excludePredicate(fp) && includePredicate(fp)
                orderby fp
                //
                // Find all class declarations where class name is
                // `MoreEnumerable`. Note that this is irrespective of
                // namespace, which is out of sheer laziness.
                //
                from cd in
                    CSharpSyntaxTree
                        .ParseText(File.ReadAllText(fp), CSharpParseOptions.Default.WithPreprocessorSymbols("MORELINQ"))
                        .GetRoot()
                        .SyntaxTree
                        .GetCompilationUnitRoot()
                        .DescendantNodes().OfType<ClassDeclarationSyntax>()
                where (string) cd.Identifier.Value == "MoreEnumerable"
                //
                // Get all method declarations where method:
                //
                // - has at least one parameter
                // - extends a type (first parameter uses the `this` modifier)
                // - is public
                // - isn't marked as being obsolete
                //
                from md in cd.DescendantNodes().OfType<MethodDeclarationSyntax>()
                let mn = (string) md.Identifier.Value
                where md.ParameterList.Parameters.Count > 0
                   && md.ParameterList.Parameters.First().Modifiers.Any(m => (string)m.Value == "this")
                   && md.Modifiers.Any(m => (string)m.Value == "public")
                   && md.AttributeLists.SelectMany(al => al.Attributes).All(a => a.Name.ToString() != "Obsolete")
                //
                // Build a dictionary of type abbreviations (e.g. TSource -> a,
                // TResult -> b, etc.) for the method's type parameters. If the
                // method is non-generic, then this will be null!
                //
                let typeParameterAbbreviationByName =
                    md.TypeParameterList
                     ?.Parameters
                      .Select((e, i) => (Original: e.Identifier.ValueText, Alias: abbreviatedTypeNodes[i]))
                      .ToDictionary(e => e.Original, e => e.Alias)
                //
                // Put everything together. While we mostly care about the
                // method declaration, the rest of the information is captured
                // for the purpose of stabilizing the code generation order and
                // debugging (--debug).
                //
                select new
                {
                    Syntax = md,
                    Name = md.Identifier.ToString(),
                    TypeParameterCount = md.TypeParameterList?.Parameters.Count ?? 0,
                    TypeParameterAbbreviationByName = typeParameterAbbreviationByName,
                    ParameterCount = md.ParameterList.Parameters.Count,
                    SortableParameterTypes =
                        from p in md.ParameterList.Parameters
                        select CreateTypeKey(p.Type,
                                             n => typeParameterAbbreviationByName != null
                                               && typeParameterAbbreviationByName.TryGetValue(n, out var a) ? a : null),
                }
            }
            from e in ms.Select((m, i) => (SourceOrder: i + 1, Method: m))
            orderby
                e.Method.Name,
                e.Method.TypeParameterCount,
                e.Method.ParameterCount,
                new TupleTypeKey(ImmutableList.CreateRange(e.Method.SortableParameterTypes))
            select new
            {
                e.Method,
                e.SourceOrder,
            };

            q = q.ToArray();

            if (debug)
            {
                var ms =
                    //
                    // Example of what this is designed to produce:
                    //
                    // 083: Lag<a, b>(IEnumerable<a>, int, Func<a, a, b>) where a = TSource, b = TResult
                    // 084: Lag<a, b>(IEnumerable<a>, int, a, Func<a, a, b>) where a = TSource, b = TResult
                    // 085: Lead<a, b>(IEnumerable<a>, int, Func<a, a, b>) where a = TSource, b = TResult
                    // 086: Lead<a, b>(IEnumerable<a>, int, a, Func<a, a, b>) where a = TSource, b = TResult
                    //
                    from e in q
                    let m = e.Method
                    select new
                    {
                        m.Name,

                        SourceOrder = e.SourceOrder.ToString("000", CultureInfo.InvariantCulture),

                        TypeParameters =
                            m.TypeParameterCount == 0
                            ? string.Empty
                            : "<" + string.Join(", ", from a in m.TypeParameterAbbreviationByName
                                                      select a.Value) + ">",
                        Parameters =
                            "(" + string.Join(", ", m.SortableParameterTypes) + ")",

                        Abbreviations =
                            m.TypeParameterCount == 0
                            ? string.Empty
                            : " where " + string.Join(", ", from a in m.TypeParameterAbbreviationByName
                                                            select a.Value + " = " + a.Key),
                    }
                    into e
                    select e.SourceOrder + ": "
                         + e.Name + e.TypeParameters + e.Parameters + e.Abbreviations;

                foreach (var m in ms)
                    Console.Error.WriteLine(m);
            }

            var indent = new string(' ', 4);
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
                from md in q
                select md.Method.Syntax into md
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
                                            ArgumentList(
                                                SeparatedList(
                                                    from p in md.ParameterList.Parameters
                                                    select Argument(IdentifierName(p.Identifier)),
                                                    Enumerable.Repeat(ParseToken(",").WithTrailingTrivia(Space),
                                                                      md.ParameterList.Parameters.Count - 1))))
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

namespace MoreLinq.Extensions
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

        public static TypeKey CreateTypeKey(TypeSyntax root,
                                            Func<string, TypeKey> abbreviator = null)
        {
            return Walk(root ?? throw new ArgumentNullException(nameof(root)));

            TypeKey Walk(TypeSyntax ts) =>
                ts is GenericNameSyntax gns
                ? new GenericTypeKey(gns.Identifier.ToString(),
                                     ImmutableList.CreateRange(gns.TypeArgumentList.Arguments.Select(Walk)))
                : ts is IdentifierNameSyntax ins
                ? abbreviator?.Invoke(ins.Identifier.ValueText) ?? new SimpleTypeKey(ins.ToString())
                : ts is PredefinedTypeSyntax pts
                ? new SimpleTypeKey(pts.ToString())
                : ts is ArrayTypeSyntax ats
                ? new ArrayTypeKey(Walk(ats.ElementType),
                                   ImmutableList.CreateRange(from rs in ats.RankSpecifiers select rs.Rank))
                : ts is NullableTypeSyntax nts
                ? new NullableTypeKey(Walk(nts.ElementType))
                : ts is TupleTypeSyntax tts
                ? (TypeKey) new TupleTypeKey(ImmutableList.CreateRange(from te in tts.Elements select Walk(te.Type)))
                : throw new NotSupportedException("Unhandled type: " + ts);
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

    //
    // Logical type nodes designed to be structurally sortable based on:
    //
    // - Type parameter count
    // - Name
    // - Array rank, if an array
    // - Each type parameter (recursively)
    //

    abstract class TypeKey : IComparable<TypeKey>
    {
        protected TypeKey(string name) => Name = name;

        public string Name { get; }
        public abstract ImmutableList<TypeKey> Parameters { get; }

        public virtual int CompareTo(TypeKey other)
            => ReferenceEquals(this, other) ? 0
             : other == null ? 1
             : Parameters.Count.CompareTo(other.Parameters.Count) is int lc && lc != 0 ? lc
             : string.Compare(Name, other.Name, StringComparison.Ordinal) is int nc && nc != 0 ? nc
             : CompareParameters(other);

        protected virtual int CompareParameters(TypeKey other) =>
            Compare(Parameters, other.Parameters);

        protected static int Compare(IEnumerable<TypeKey> a, IEnumerable<TypeKey> b) =>
            a.Zip(b, (us, them) => (Us: us, Them: them))
             .Select(e => e.Us.CompareTo(e.Them))
             .FirstOrDefault(e => e != 0);
    }

    sealed class SimpleTypeKey : TypeKey
    {
        public SimpleTypeKey(string name) : base(name) {}
        public override string ToString() => Name;
        public override ImmutableList<TypeKey> Parameters => ImmutableList<TypeKey>.Empty;
    }

    abstract class ParameterizedTypeKey : TypeKey
    {
        protected ParameterizedTypeKey(string name, TypeKey parameter) :
            this(name, ImmutableList.Create(parameter)) {}

        protected ParameterizedTypeKey(string name, ImmutableList<TypeKey> parameters) :
            base(name) => Parameters = parameters;

        public override ImmutableList<TypeKey> Parameters { get; }
    }

    sealed class GenericTypeKey : ParameterizedTypeKey
    {
        public GenericTypeKey(string name, ImmutableList<TypeKey> parameters) :
            base(name, parameters) {}

        public override string ToString() =>
            Name + "<" + string.Join(", ", Parameters) + ">";
    }

    sealed class NullableTypeKey : ParameterizedTypeKey
    {
        public NullableTypeKey(TypeKey underlying) : base("?", underlying) {}
        public override string ToString() => Parameters.Single() + "?";
    }

    sealed class TupleTypeKey : ParameterizedTypeKey
    {
        public TupleTypeKey(ImmutableList<TypeKey> parameters) :
            base("()", parameters) {}

        public override string ToString() =>
            "(" + string.Join(", ", Parameters) + ")";
    }

    sealed class ArrayTypeKey : ParameterizedTypeKey
    {
        public ArrayTypeKey(TypeKey element, IEnumerable<int> ranks) :
            base("[]", element) => Ranks = ImmutableList.CreateRange(ranks);

        public ImmutableList<int> Ranks { get; }

        public override string ToString() =>
            Parameters.Single() + string.Concat(from r in Ranks
                                                select "[" + string.Concat(Enumerable.Repeat(",", r - 1)) + "]");

        protected override int CompareParameters(TypeKey other)
        {
            if (other is ArrayTypeKey a)
            {
                if (Ranks.Count.CompareTo(a.Ranks.Count) is int rlc && rlc != 0)
                    return rlc;
                if (Ranks.Zip(a.Ranks, (us, them) => (Us: us, Them: them))
                         .Aggregate(0, (c, r) => c == 0 ? r.Us.CompareTo(r.Them) : c) is int rc && rc != 0)
                    return rc;
            }

            return base.CompareParameters(other);
        }
    }
}
