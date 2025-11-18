using RentCarServer.Domain.Abstraction;
using RentCarServer.Domain.LoginTokens.ValueObjects;

namespace RentCarServer.Domain.LoginTokens;
public sealed class LoginToken
{
    public LoginToken()
    {
    }
    public LoginToken(Token token, IdentityId userId, ExpireDate expireDate)
    {
        Id = new(Guid.CreateVersion7());
        SetToken(token);
        SetUserId(userId);
        SetIsActive(new(true));
        SetExpireDate(expireDate);
    }


    public IdentityId Id { get; private set; } = default!;
    public Token Token { get; private set; } = default!;
    public IdentityId UserId { get; private set; } = default!;
    public IsActive IsActive { get; private set; } = default!;
    public ExpireDate ExpireDate { get; private set; } = default!;
    #region Behaviors
    public void SetIsActive(IsActive isActive)
    {
        IsActive = isActive;
    }
    public void SetToken(Token token)
    {
        Token = token;
    }
    public void SetUserId(IdentityId userId)
    {
        UserId = userId;
    }
    public void SetExpireDate(ExpireDate expireDate)
    {
        ExpireDate = expireDate;
    }
    #endregion
}