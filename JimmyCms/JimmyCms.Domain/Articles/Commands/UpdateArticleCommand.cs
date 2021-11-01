using System;
using MediatR;

namespace JimmyCms.Domain.Articles.Commands
{
    public record UpdateArticleCommand : IRequest<BasicResponse>
    {
        public Guid Id { get; set; }
        public string Title { get; }
        public string Body { get; }

        public UpdateArticleCommand(Guid id, string title, string body)
        {
            Id = id;
            Title = title;
            Body = body;
        }
    }
}