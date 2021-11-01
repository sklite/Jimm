using FluentValidation;
using JimmyCms.Domain.Messaging.Articles.Queries;

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
