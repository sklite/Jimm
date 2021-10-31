using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JimmyCms.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JimmyCms.Domain.Articles.Commands
{
    public class UpdateArticleCommandHandler : IRequestHandler<UpdateArticleCommand, BasicResponse>
    {
        private readonly IArticleContext _context;

        public UpdateArticleCommandHandler(IArticleContext context)
        {
            _context = context;
        }

        public async Task<BasicResponse> Handle(UpdateArticleCommand request, CancellationToken cancellationToken)
        {
            var articleToUpdate = await _context.Articles.FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

            if (articleToUpdate == null)
                throw new KeyNotFoundException($"Can't find article with id {request.Id}");

            articleToUpdate.Title = request.Title;
            articleToUpdate.Body = request.Body;
            articleToUpdate.UpdatedOn = DateTime.Now;
            
            await _context.SaveChangesAsync(cancellationToken);

            return new BasicResponse();
        }
    }
}