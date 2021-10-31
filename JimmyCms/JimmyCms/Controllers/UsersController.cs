using System.Threading.Tasks;
using JimmyCms.ApiModels;
using JimmyCms.Domain.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JimmyCms.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : BaseJimmyCmsController
    {

        public UsersController(IMediator mediator)
            :base(mediator)
        {
        }

        [HttpPost]
        public async Task<IActionResult> Login(SignInRequest request)
        {
            return await Execute(new AuthenticateCommand(request.Username, request.Password));
        }
    }
}