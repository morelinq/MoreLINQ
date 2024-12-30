#if NET9_0_OR_GREATER
// https://learn.microsoft.com/dotnet/csharp/language-reference/statements/lock#guidelines
global using Lock = System.Threading.Lock;
#else
// For why "System.Object" instead of "object", see:
// https://github.com/dotnet/runtime/issues/110242
global using Lock = System.Object;
#endif
