using System.Threading.Tasks;
using JimmyCms.Domain.Messaging.Articles;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JimmyCms.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public abstract class BaseJimmyCmsController : ControllerBase
    {
        private readonly IMediator _mediator;

        protected BaseJimmyCmsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        ObjectResult HandleResult(BasicResponse result)
        {
            return result.Success 
                ? Ok(result.Value) 
                : StatusCode(result.ResponseCode, result.Value);
        }

        protected async Task<IActionResult> Execute(IRequest<BasicResponse> request)
        {
            return HandleResult(await _mediator.Send(request));
        }
    }
}