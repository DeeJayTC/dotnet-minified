using System.Linq.Expressions;
using FluentValidation;

namespace Smoower.Minified.Validation;

// Inherit MiniValidator<T> (instead of AbstractValidator<T>) to get the short
// rule entry points `rule` and `req` without an extension-method receiver:
//   public class UserInValidator : MiniValidator<UserIn> {
//     public UserInValidator() {
//       req(x => x.Name).max(100);
//       req(x => x.Email).email();
//     }
//   }
public abstract class MiniValidator<T> : AbstractValidator<T>
{
    protected IRuleBuilderInitial<T, TProp> rule<TProp>(Expression<Func<T, TProp>> e) => RuleFor(e);

    protected IRuleBuilderOptions<T, TProp> req<TProp>(Expression<Func<T, TProp>> e) => RuleFor(e).NotEmpty();
}

// Rule shorteners that chain onto any RuleFor / rule / req builder.
public static class ValidationExtensions
{
    // String rules.
    public static IRuleBuilderOptions<T, string?> max<T>(this IRuleBuilder<T, string?> r, int n) => r.MaximumLength(n);
    public static IRuleBuilderOptions<T, string?> min<T>(this IRuleBuilder<T, string?> r, int n) => r.MinimumLength(n);
    public static IRuleBuilderOptions<T, string?> len<T>(this IRuleBuilder<T, string?> r, int a, int b) => r.Length(a, b);
    public static IRuleBuilderOptions<T, string?> email<T>(this IRuleBuilder<T, string?> r) => r.EmailAddress();
    public static IRuleBuilderOptions<T, string?> matches<T>(this IRuleBuilder<T, string?> r, string pattern) => r.Matches(pattern);

    // Comparable rules.
    public static IRuleBuilderOptions<T, TProp> gt<T, TProp>(this IRuleBuilder<T, TProp> r, TProp value) where TProp : IComparable<TProp>, IComparable => r.GreaterThan(value);
    public static IRuleBuilderOptions<T, TProp> lt<T, TProp>(this IRuleBuilder<T, TProp> r, TProp value) where TProp : IComparable<TProp>, IComparable => r.LessThan(value);
    public static IRuleBuilderOptions<T, TProp> gte<T, TProp>(this IRuleBuilder<T, TProp> r, TProp value) where TProp : IComparable<TProp>, IComparable => r.GreaterThanOrEqualTo(value);
    public static IRuleBuilderOptions<T, TProp> lte<T, TProp>(this IRuleBuilder<T, TProp> r, TProp value) where TProp : IComparable<TProp>, IComparable => r.LessThanOrEqualTo(value);
    public static IRuleBuilderOptions<T, TProp> rng<T, TProp>(this IRuleBuilder<T, TProp> r, TProp from, TProp to) where TProp : IComparable<TProp>, IComparable => r.InclusiveBetween(from, to);
}
