using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JimmyCms.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JimmyCms.Domain.Articles.Queries
{
    public class CreateArticleCommandHandler : IRequestHandler<GetArticleByIdQuery, BasicResponse>
    {
        private readonly IArticleContext _context;

        public CreateArticleCommandHandler(IArticleContext context)
        {
            _context = context;
        }

        public async Task<BasicResponse> Handle(GetArticleByIdQuery request, CancellationToken cancellationToken)
        {
            var article = await _context.Articles.FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);
            if (article == null)
                throw new KeyNotFoundException($"Can't find article with id {request.Id}");

            return new BasicResponse { Value = article };
        }
    }
}