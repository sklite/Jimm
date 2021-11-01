using FluentValidation;
using JimmyCms.Domain.Messaging.Articles.Commands;
using JimmyCms.Domain.Settings;
using Microsoft.Extensions.Options;

namespace JimmyCms.Domain.Validators
{
    public class CreateArticleCommandValidator : AbstractValidator<CreateArticleCommand>
    {
        public CreateArticleCommandValidator(IOptions<ValidationSettings> settings)
        {
            RuleFor(query => query.Title)
                .NotEmpty()
                .MaximumLength(settings.Value.ArticleTitleMaxLength);

            RuleFor(query => query.Body)
                .NotEmpty()
                .MaximumLength(settings.Value.ArticleBodyMaxLength);
        }
    }
}
