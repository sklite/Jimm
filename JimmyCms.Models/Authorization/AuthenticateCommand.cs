using JimmyCms.Domain.Articles;
using MediatR;

namespace JimmyCms.Domain.Authorization
{
    public record AuthenticateCommand : IRequest<BasicResponse>
    {
        public string Login { get; }
        public string Password { get; }

        public AuthenticateCommand(string login, string password)
        {
            Login = login;
            Password = password;
        }
    }
}