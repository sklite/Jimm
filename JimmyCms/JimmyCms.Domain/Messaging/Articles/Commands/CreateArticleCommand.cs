using MediatR;

namespace JimmyCms.Domain.Messaging.Articles.Commands
{
    public record CreateArticleCommand : IRequest<BasicResponse>
    {
        public string Title { get; }
        public string Body { get; }

        public CreateArticleCommand(string title, string body)
        {
            Title = title;
            Body = body;
        }
    }
}