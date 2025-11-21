namespace RentCarServer.Domain.Users.ValueObjects;

public sealed record LastName
{
    public string Value { get; init; }
    public LastName(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length < 2)
            throw new ArgumentException("İsim en az 2 karakter olmalıdır.");

        Value = value;
    }
    // LastName -> string
    public static implicit operator string(LastName name) => name.Value;
    // string -> LastName
    public static implicit operator LastName(string value) => new(value);
}