namespace RentCarServer.Domain.Users.ValueObjects;

public sealed record Email
{
    public string Value { get; init; }
    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email boş olamaz.");

        if (!value.Contains("@"))
            throw new ArgumentException("Geçersiz email formatı.");

        Value = value;
    }
}
