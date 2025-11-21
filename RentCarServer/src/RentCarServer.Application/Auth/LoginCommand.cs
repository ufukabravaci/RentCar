using FluentValidation;
using RentCarServer.Application.Services;
using RentCarServer.Domain.Users;
using RentCarServer.Domain.Users.ValueObjects;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Auth;


public sealed record LoginCommand(string EmailOrUserName, string Password) : IRequest<Result<string>>;

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(p => p.EmailOrUserName).NotEmpty().WithMessage("Geçerli bir mail veya kullanıcı adı giriniz.");
        RuleFor(p => p.Password).NotEmpty().WithMessage("Geçerli bir şifre giriniz.");
    }
}

public sealed class LoginCommandHandler(IUserRepository userRepository, IJwtProvider jwtProvider)
    : IRequestHandler<LoginCommand, Result<string>>
{
    public async Task<Result<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        Email? emailToCheck = null;
        UserName? userNameToCheck = null;
        if (request.EmailOrUserName.Contains("@"))
        {
            emailToCheck = new Email(request.EmailOrUserName);
        }
        else
        {
            userNameToCheck = new UserName(request.EmailOrUserName);
        }
        var user = await userRepository.FirstOrDefaultAsync(p =>
            (emailToCheck != null && p.Email == emailToCheck) ||
            (userNameToCheck != null && p.UserName == userNameToCheck),
            cancellationToken
        );
        if (user is null)
        {
            return Result<string>.Failure("KUllanıcı bilgisi ya da şifre hatalı.");
        }
        var checkPassword = user.VerifyPasswordHash(request.Password);
        if (!checkPassword)
        {
            return Result<string>.Failure("KUllanıcı bilgisi ya da şifre hatalı.");
        }
        var token = await jwtProvider.CreateTokenAsync(user, cancellationToken);
        return token; //Result<string>.Succeed(token) yazmamıza gerek yok implicit operator kullandığımız için
    }
}
