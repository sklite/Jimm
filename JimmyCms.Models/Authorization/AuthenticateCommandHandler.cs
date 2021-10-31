using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JimmyCms.Domain.Articles;
using JimmyCms.Infrastructure;
using MediatR;
using Microsoft.IdentityModel.Tokens;

namespace JimmyCms.Domain.Authorization
{
    public class AuthenticateCommandHandler : IRequestHandler<AuthenticateCommand, BasicResponse>
    {
        private readonly IArticleContext _context;

        public AuthenticateCommandHandler(IArticleContext context)
        {
            _context = context;
        }

        public async Task<BasicResponse> Handle(AuthenticateCommand request, CancellationToken cancellationToken)
        {
            //TODO: check that db contains the same hashed user+pass info as in request
            //for simplicity reasons, we omit this validation

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, request.Login),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("quite long string that can be used as a symmetric security key"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken("https://localhost:49154/",
                "https://localhost:49154/",
                claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: creds);
            
            return new BasicResponse { Value = new JwtSecurityTokenHandler().WriteToken(token) };
        }
    }
}