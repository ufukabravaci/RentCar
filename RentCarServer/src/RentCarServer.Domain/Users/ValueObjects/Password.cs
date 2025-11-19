namespace RentCarServer.Domain.Users.ValueObjects;

public sealed record Password
{
    private Password()
    {
    }
    public Password(string password)
    {
        if (string.IsNullOrWhiteSpace(password) || password.Length < 2)
            throw new ArgumentException("Şifre en az 2 karakter olmalıdır.");
        CreatePasswordHash(password);
    }
    public byte[] PasswordHash { get; private set; } = default!;
    public byte[] PasswordSalt { get; private set; } = default!;

    private void CreatePasswordHash(string password)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA512();
        PasswordSalt = hmac.Key;
        PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
    }
}
