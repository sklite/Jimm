using System;
using MediatR;

namespace JimmyCms.Domain.Messaging.Articles.Queries
{
    public record GetArticleByIdQuery : IRequest<BasicResponse>
    {
        public Guid Id { get; }

        public GetArticleByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}