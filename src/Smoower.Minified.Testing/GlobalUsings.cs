// xUnit attribute aliases — for consumers to copy into their OWN test project's
// GlobalUsings.cs (using aliases are not transitive across assemblies, and the
// attribute types live in xunit.core, which this assertion-only package does not
// reference). They compile to the real xUnit attributes, so discovery is unchanged:
//
//   global using F   = Xunit.FactAttribute;        // [F]
//   global using Th  = Xunit.TheoryAttribute;      // [Th]
//   global using In  = Xunit.InlineDataAttribute;  // [In(...)]
//   global using Mem = Xunit.MemberDataAttribute;  // [Mem(...)]
