namespace RentCarServer.Domain.Users.ValueObjects;

public sealed record ForgotPasswordCode(Guid Value)
{
    // ForgotPasswordCode -> Guid
    public static implicit operator Guid(ForgotPasswordCode code) => code.Value;
    // Guid -> ForgotPasswordCode
    public static implicit operator ForgotPasswordCode(Guid value) => new(value);
}