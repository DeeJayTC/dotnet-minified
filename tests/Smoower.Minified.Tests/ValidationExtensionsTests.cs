using FluentValidation;
using Smoower.Minified.Validation;

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

    [F]
    public void Valid_PassesAllRules()
        => new PersonValidator().Validate(new Person("Ada", "ada@x.com", 36)).IsValid.tru();

    [Th]
    [In(null, "ada@x.com", 36, "Name")]
    [In("TooLongName", "ada@x.com", 36, "Name")]
    [In("Ada", "not-an-email", 36, "Email")]
    [In("Ada", "ada@x.com", 0, "Age")]
    [In("Ada", "ada@x.com", 200, "Age")]
    public void Invalid_FailsExpectedProperty(string? name, string? email, int age, string property)
    {
        var result = new PersonValidator().Validate(new Person(name, email, age));
        result.IsValid.fls();
        result.Errors.has(e => e.PropertyName == property);
    }
}
