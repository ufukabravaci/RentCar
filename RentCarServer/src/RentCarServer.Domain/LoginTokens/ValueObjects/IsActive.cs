namespace RentCarServer.Domain.LoginTokens.ValueObjects;

public sealed record IsActive(bool Value)
{
    // IsActive -> bool (if (isActive) diyebilmek için)
    public static implicit operator bool(IsActive isActive) => isActive.Value;
    // bool -> IsActive (IsActive = true diyebilmek için)
    public static implicit operator IsActive(bool value) => new(value);
}
