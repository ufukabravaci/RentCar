namespace RentCarServer.Domain.Users.ValueObjects;

public sealed record ForgotPasswordDate(DateTimeOffset Value)
{
    // ForgotPasswordDate -> DateTimeOffset
    public static implicit operator DateTimeOffset(ForgotPasswordDate date) => date.Value;
    // DateTimeOffset -> ForgotPasswordDate
    public static implicit operator ForgotPasswordDate(DateTimeOffset value) => new(value);
}