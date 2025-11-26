namespace RentCarServer.Domain.Branchs.ValueObjects;

public sealed record Name
{
    public string Value { get; init; }

    public Name(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length < 2)
            throw new ArgumentException("Şube adı en az 2 karakter olmalıdır.");

        Value = value;
    }

    // Name -> string
    public static implicit operator string(Name name) => name.Value;

    // string -> Name
    public static implicit operator Name(string value) => new(value);
}