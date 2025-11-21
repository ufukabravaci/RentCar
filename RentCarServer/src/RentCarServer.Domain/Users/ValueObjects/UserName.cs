namespace RentCarServer.Domain.Users.ValueObjects;

public sealed record UserName
{
    public string Value { get; init; }
    public UserName(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length < 2)
            throw new ArgumentException("İsim en az 2 karakter olmalıdır.");

        Value = value;
    }
    // UserName -> string
    public static implicit operator string(UserName name) => name.Value;
    // string -> UserName
    public static implicit operator UserName(string value) => new(value);
}
