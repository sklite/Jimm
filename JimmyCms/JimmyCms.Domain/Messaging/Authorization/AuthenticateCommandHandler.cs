using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JimmyCms.Domain.Messaging.Articles;
using JimmyCms.Domain.Settings;
using JimmyCms.Infrastructure;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace JimmyCms.Domain.Messaging.Authorization
{
    public class AuthenticateCommandHandler : IRequestHandler<AuthenticateCommand, BasicResponse>
    {
        private readonly IArticleContext _context;
        private readonly SecuritySettings _settings;

        public AuthenticateCommandHandler(IArticleContext context, IOptions<SecuritySettings> settings)
        {
            _context = context;
            _settings = settings.Value;
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
            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_settings.TokenIssuer,
                _settings.TokenAudience,
                claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: creds);
            
            return new BasicResponse { Value = new JwtSecurityTokenHandler().WriteToken(token) };
        }
    }
}