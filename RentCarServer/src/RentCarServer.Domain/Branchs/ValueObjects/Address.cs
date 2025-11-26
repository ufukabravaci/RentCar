namespace RentCarServer.Domain.Branchs.ValueObjects;

public sealed record Address
{
    private Address() { }
    public Address(string city, string district, string fullAddress, string phoneNumber1, string? phoneNumber2, string email)
    {
        if (string.IsNullOrWhiteSpace(city)) throw new ArgumentException("Şehir boş olamaz.");
        if (string.IsNullOrWhiteSpace(district)) throw new ArgumentException("İlçe boş olamaz.");
        if (string.IsNullOrWhiteSpace(fullAddress)) throw new ArgumentException("Adres detayı boş olamaz.");
        if (string.IsNullOrWhiteSpace(phoneNumber1)) throw new ArgumentException("Telefon numarası boş olamaz.");
        if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
            throw new ArgumentException("Geçerli bir şube mail adresi giriniz.");

        City = city;
        District = district;
        FullAddress = fullAddress;
        PhoneNumber1 = phoneNumber1;
        PhoneNumber2 = phoneNumber2;
        Email = email;
    }

    public string City { get; init; } = default!;
    public string District { get; init; } = default!;
    public string FullAddress { get; init; } = default!;
    public string PhoneNumber1 { get; init; } = default!;
    public string? PhoneNumber2 { get; init; }
    public string Email { get; init; } = default!;
}