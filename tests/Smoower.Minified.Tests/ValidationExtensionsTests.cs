using FluentValidation;
using Smoower.Minified.Validation;
using Xunit;

namespace Smoower.Minified.Tests;

public class ValidationExtensionsTests
{
    private record Person(string? Name, string? Email, int Age);

    private sealed class PersonValidator : MiniValidator<Person>
    {
        public PersonValidator()
        {
            req(x => x.Name).max(5);
            req(x => x.Email).email();
            rule(x => x.Age).gt(0).lte(120);
        }
    }

    [Fact]
    public void Valid_PassesAllRules()
        => Assert.True(new PersonValidator().Validate(new Person("Ada", "ada@x.com", 36)).IsValid);

    [Theory]
    [InlineData(null, "ada@x.com", 36, "Name")]      // req
    [InlineData("TooLongName", "ada@x.com", 36, "Name")] // max
    [InlineData("Ada", "not-an-email", 36, "Email")] // email
    [InlineData("Ada", "ada@x.com", 0, "Age")]       // gt
    [InlineData("Ada", "ada@x.com", 200, "Age")]     // lte
    public void Invalid_FailsExpectedProperty(string? name, string? email, int age, string property)
    {
        var result = new PersonValidator().Validate(new Person(name, email, age));
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == property);
    }
}
