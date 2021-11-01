using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JimmyCms.Infrastructure;
using MediatR;

namespace JimmyCms.Domain.Articles.Queries
{
    public class GetArticlesQueryHandler : IRequestHandler<GetArticlesQuery, BasicResponse>
    {
        private readonly IArticleContext _context;

        public GetArticlesQueryHandler(IArticleContext context)
        {
            _context = context;
        }

        public async Task<BasicResponse> Handle(GetArticlesQuery request, CancellationToken cancellationToken)
        {
            var sorted = request.Ascending
                ? _context.Articles.OrderBy(a => a.CreatedOn)
                : _context.Articles.OrderByDescending(a => a.CreatedOn);

            var result = sorted.Skip(request.Skip).Take(request.Take);
            return new BasicResponse { Value = result };
        }
    }
}