using MediatR;

namespace JimmyCms.Domain.Articles.Queries
{
    public record GetArticlesQuery : IRequest<BasicResponse>
    {
        public int Skip { get; }
        public int Take { get; }
        public bool Ascending { get; }

        public GetArticlesQuery(bool ascending, int skip, int take)
        {
            Skip = skip;
            Take = take;
            Ascending = ascending;
        }
    }
}