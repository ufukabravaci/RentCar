namespace RentCarServer.Domain.Users.ValueObjects;

public sealed record IsForgotPasswordCompleted(bool Value)
{
    // IsForgotPasswordCompleted -> bool
    public static implicit operator bool(IsForgotPasswordCompleted obj) => obj.Value;
    // bool -> IsForgotPasswordCompleted
    public static implicit operator IsForgotPasswordCompleted(bool value) => new(value);
}