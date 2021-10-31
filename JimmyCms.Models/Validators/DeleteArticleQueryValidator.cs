using FluentValidation;
using JimmyCms.Domain.Articles.Queries;

namespace JimmyCms.Domain.Validators
{
    public class GetArticleByIdQueryValidator : AbstractValidator<GetArticleByIdQuery>
    {
        public GetArticleByIdQueryValidator()
        {
            RuleFor(query => query.Id)
                .NotEmpty();
        }
    }
}
