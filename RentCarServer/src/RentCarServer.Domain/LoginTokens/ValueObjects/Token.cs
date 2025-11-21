namespace RentCarServer.Domain.LoginTokens.ValueObjects;

public sealed record Token
{
    public string Value { get; init; }

    public Token(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Token boş olamaz");
        Value = value;
    }

    // Token -> string
    public static implicit operator string(Token token) => token.Value;

    // string -> Token
    public static implicit operator Token(string value) => new(value);
}
