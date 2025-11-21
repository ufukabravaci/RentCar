namespace RentCarServer.Domain.LoginTokens.ValueObjects;

public sealed record ExpireDate(DateTimeOffset Value)
{
    // ExpireDate -> DateTimeOffset 
    public static implicit operator DateTimeOffset(ExpireDate expireDate) => expireDate.Value;

    // DateTimeOffset -> ExpireDate
    public static implicit operator ExpireDate(DateTimeOffset value) => new(value);
}