using System;
using MediatR;

namespace JimmyCms.Domain.Articles.Queries
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