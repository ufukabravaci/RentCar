namespace RentCarServer.Domain.Users.ValueObjects;

public sealed record FirstName
{
    public string Value { get; init; }
    public FirstName(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length < 2)
            throw new ArgumentException("İsim en az 2 karakter olmalıdır.");

        Value = value;
    }
    // FirstName -> string
    public static implicit operator string(FirstName name) => name.Value;
    // string -> FirstName
    public static implicit operator FirstName(string value) => new(value);
}