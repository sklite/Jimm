using System;
using System.Threading;
using System.Threading.Tasks;
using JimmyCms.Infrastructure;
using MediatR;

namespace JimmyCms.Domain.Articles.Commands
{
    public class CreateArticleCommandHandler : IRequestHandler<CreateArticleCommand, BasicResponse>
    {
        private readonly IArticleContext _context;

        public CreateArticleCommandHandler(IArticleContext context)
        {
            _context = context;
        }

        public async Task<BasicResponse> Handle(CreateArticleCommand request, CancellationToken cancellationToken)
        {
            var newArticle = new Article
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Body = request.Body,
                CreatedOn = DateTime.Now
            };

            await _context.Articles.AddAsync(newArticle, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new BasicResponse { Value = newArticle };
        }
    }
}