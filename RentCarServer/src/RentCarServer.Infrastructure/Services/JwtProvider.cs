using RentCarServer.Application.Services;
using RentCarServer.Domain.Users;

namespace RentCarServer.Infrastructure.Services;
internal class JwtProvider : IJwtProvider
{
    public string CreateToken(User user)
    {
        return "token";
    }
}
