namespace Smoower.Minified.Tests;

// Proves the [F]/[Th]/[In] aliases are discovered as real xUnit tests, and uses
// the compact assertion helpers in the same breath.
public class AttributeAliasTests
{
    [F]
    public void Fact_AliasIsDiscovered() => true.tru();

    [Th]
    [In(2, 3, 5)]
    [In(-1, 1, 0)]
    public void Theory_AliasIsDiscovered(int a, int b, int sum) => (a + b).eq(sum);
}
