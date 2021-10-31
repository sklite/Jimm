using System;
using System.Threading.Tasks;
using JimmyCms.ApiModels;
using JimmyCms.Domain.Articles.Commands;
using JimmyCms.Domain.Articles.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JimmyCms.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ArticlesController : BaseJimmyCmsController
    {
        public ArticlesController(IMediator mediator) 
            : base(mediator)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Get(bool asc = true, int skip = 0, int take = 0)
        {
            return await Execute(new GetArticlesQuery(asc, skip, take));
        }

        [HttpGet]
        [Route("{articleId}")]
        public async Task<IActionResult> Get(Guid articleId)
        {
            return await Execute(new GetArticleByIdQuery(articleId));
        }

        [Authorize]
        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add(AddArticleRequest newArticle)
        {
            return await Execute(new CreateArticleCommand(newArticle.Title, newArticle.Body));
        }
        
        [Authorize]
        [HttpPut]
        [Route("{articleId}")]
        public async Task<IActionResult> Update(Guid articleId, UpdateArticleRequest updatedArticle)
        {
            return await Execute(new UpdateArticleCommand(articleId, updatedArticle.Title, updatedArticle.Body));
        }

        [Authorize]
        [HttpDelete]
        [Route("{articleId}")]
        public async Task<IActionResult> Delete(Guid articleId)
        {
            return await Execute(new DeleteArticleCommand(articleId));
        }


    }
}