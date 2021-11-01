using FluentValidation;
using JimmyCms.Domain.Messaging.Articles.Queries;

namespace JimmyCms.Domain.Validators
{
    public class GetArticlesQueryValidator : AbstractValidator<GetArticlesQuery>
    {
        public GetArticlesQueryValidator()
        {
            var skipMinValue = -1;
            var takeMinValue = 0;

            RuleFor(query => query.Skip)
                .GreaterThan(skipMinValue)
                .WithMessage("The {PropertyName} value must be non-negative");
            RuleFor(query => query.Take)
                .GreaterThan(takeMinValue)
                .WithMessage("The {PropertyName} value must be positive");
        }
    }
}
