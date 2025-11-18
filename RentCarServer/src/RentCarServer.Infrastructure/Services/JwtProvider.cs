using GenericRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RentCarServer.Application.Services;
using RentCarServer.Domain.LoginTokens;
using RentCarServer.Domain.LoginTokens.ValueObjects;
using RentCarServer.Domain.Users;
using RentCarServer.Infrastructure.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RentCarServer.Infrastructure.Services;
internal class JwtProvider(
    IOptions<JwtOptions> options,
    ILoginTokenRepository loginTokenRepository,
    IUnitOfWork unitOfWork) : IJwtProvider
{
    public async Task<string> CreateTokenAsync(User user, CancellationToken cancellationToken = default)
    {
        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id), //explicit operatörü sayesinde string'e çevirmeden verebildik.
            new Claim("fullName", user.FullName.Value),
            new Claim("email", user.Email.Value)
        };

        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(options.Value.SecretKey));
        SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha512);

        var expires = DateTime.Now.AddDays(1);
        JwtSecurityToken securityToken = new(
            issuer: options.Value.Issuer,
            audience: options.Value.Audience,
            claims: claims,
            notBefore: DateTime.Now,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: signingCredentials
            );
        var handler = new JwtSecurityTokenHandler();
        var token = handler.WriteToken(securityToken);

        Token newToken = new(token);
        ExpireDate expireDate = new(expires);
        LoginToken loginToken = new(newToken, user.Id, expireDate);
        loginTokenRepository.Add(loginToken);
        await loginTokenRepository.Where(p => p.UserId == user.Id && p.IsActive.Value == true).
            ExecuteUpdateAsync(setters => setters.SetProperty(u => u.IsActive.Value, false), cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return token;
    }
}
