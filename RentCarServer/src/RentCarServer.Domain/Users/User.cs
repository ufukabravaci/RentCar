using RentCarServer.Domain.Abstraction;
using RentCarServer.Domain.Users.ValueObjects;

namespace RentCarServer.Domain.Users;
public sealed class User : Entity
{
    private User() { }

    public User(FirstName firstName,
        LastName lastName,
        Email email,
        UserName userName,
        Password password)
    {
        SetFirstName(firstName);
        SetLastName(lastName);
        SetEmail(email);
        SetUserName(userName);
        SetPassword(password);
        SetIsForgotPasswordCompleted(new(true));
    }

    public FirstName FirstName { get; private set; } = default!;
    public LastName LastName { get; private set; } = default!;
    public FullName FullName { get; private set; } = default!;
    public Email Email { get; private set; } = default!;
    public UserName UserName { get; private set; } = default!;
    public Password Password { get; private set; } = default!;
    public ForgotPasswordCode? ForgotPasswordCode { get; private set; }
    public ForgotPasswordDate? ForgotPasswordDate { get; private set; }
    public IsForgotPasswordCompleted IsForgotPasswordCompleted { get; private set; } = default!;

    #region Behaviors

    public bool VerifyPasswordHash(string password)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA512(Password.PasswordSalt);
        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(Password.PasswordHash);
    }
    public void CreateForgotPasswordId()
    {
        ForgotPasswordCode = new(Guid.CreateVersion7());
        ForgotPasswordDate = new(DateTimeOffset.Now);
        IsForgotPasswordCompleted = new(false);
    }

    public void SetFirstName(FirstName firstName) //user'a set edilme şartlarını kontrol eder.            
    {                                          //FirstName Classı firstname'in oluşturulma şartlarını kontrol eder.
        if (FirstName == firstName) return;     //Validasyon => FirstName kuralları => Usera set edilme kuralları
        FirstName = firstName;
        SetFullName();
    }
    public void SetLastName(LastName lastName)
    {
        if (LastName == lastName) return;
        LastName = lastName;
        SetFullName();
    }
    public void SetEmail(Email email)
    {
        if (Email == email) return;
        Email = email;
        SetFullName();
    }
    public void SetUserName(UserName userName)
    {
        UserName = userName;
    }

    public void SetPassword(Password password)
    {
        Password = password;
    }
    public void SetFullName()
    {
        FullName = new(FirstName.Value + " " + LastName.Value + " (" + Email.Value + ")");
    }
    public void SetIsForgotPasswordCompleted(IsForgotPasswordCompleted isForgotPasswordCompleted)
    {
        IsForgotPasswordCompleted = isForgotPasswordCompleted;
    }

    #endregion
}
