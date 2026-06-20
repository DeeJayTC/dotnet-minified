// The test suite eats its own dogfood: it is written with Smoower.Minified.Testing.
// These globals make the compact assertions and the [F]/[Th]/[In] attribute aliases
// available in every test file with no per-file ceremony. The aliases compile to the
// real xUnit attributes, so discovery is unchanged.
global using Xunit;
global using Smoower.Minified.Testing;
global using F = Xunit.FactAttribute;
global using Th = Xunit.TheoryAttribute;
global using In = Xunit.InlineDataAttribute;
global using Mem = Xunit.MemberDataAttribute;
