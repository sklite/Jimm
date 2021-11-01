using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JimmyCms.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JimmyCms.Domain.Articles.Commands
{
    public class DeleteArticleCommandHandler : IRequestHandler<DeleteArticleCommand, BasicResponse>
    {
        private readonly IArticleContext _context;

        public DeleteArticleCommandHandler(IArticleContext context)
        {
            _context = context;
        }

        public async Task<BasicResponse> Handle(DeleteArticleCommand request, CancellationToken cancellationToken)
        {
            var articleToRemove = await _context.Articles.FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

            if (articleToRemove == null)
                throw new KeyNotFoundException($"Can't find article with id {request.Id}");

            _context.Articles.Remove(articleToRemove);
            await _context.SaveChangesAsync(cancellationToken);

            return new BasicResponse();
        }
    }
}