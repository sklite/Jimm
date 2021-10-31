using FluentValidation;
using JimmyCms.Domain.Articles.Commands;

namespace JimmyCms.Domain.Validators
{
    public class DeleteArticleCommandValidator : AbstractValidator<DeleteArticleCommand>
    {
        public DeleteArticleCommandValidator()
        {
            RuleFor(query => query.Id)
                .NotEmpty();
        }
    }
}
