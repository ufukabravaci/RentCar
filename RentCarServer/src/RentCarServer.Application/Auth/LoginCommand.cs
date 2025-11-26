using FluentValidation;
using GenericRepository;
using RentCarServer.Application.Services;
using RentCarServer.Domain.Users;
using RentCarServer.Domain.Users.ValueObjects;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Auth;


public sealed record LoginCommand(string EmailOrUserName, string Password) : IRequest<Result<LoginCommandResponse>>;

public sealed record LoginCommandResponse
{
    public string? Token { get; set; }
    public string? TFACode { get; set; }
};

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(p => p.EmailOrUserName).NotEmpty().WithMessage("Geçerli bir mail veya kullanıcı adı giriniz.");
        RuleFor(p => p.Password).NotEmpty().WithMessage("Geçerli bir şifre giriniz.");
    }
}

public sealed class LoginCommandHandler(IUserRepository userRepository, IJwtProvider jwtProvider,
    IMailService mailService, IUnitOfWork unitOfWork)
    : IRequestHandler<LoginCommand, Result<LoginCommandResponse>>
{
    public async Task<Result<LoginCommandResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
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
            return Result<LoginCommandResponse>.Failure("KUllanıcı bilgisi ya da şifre hatalı.");
        }
        var checkPassword = user.VerifyPasswordHash(request.Password);
        if (!checkPassword)
        {
            return Result<LoginCommandResponse>.Failure("KUllanıcı bilgisi ya da şifre hatalı.");
        }

        if (!user.TFAStatus.Value)
        {
            var token = await jwtProvider.CreateTokenAsync(user, cancellationToken);
            var res = new LoginCommandResponse() { Token = token };
            return res; //Result<string>.Succeed(token) yazmamıza gerek yok implicit operator kullandığımız için
        }
        else
        {
            user.CreateTFACode();
            userRepository.Update(user);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            string to = user.Email;
            string subject = "Giriş onayı";
            string body = @$"Uygulamaya girmek için aşağıdaki kodu kullanabilirsiniz. Bu kod sadece
                5 dakika geçerlidir. <p><h4>{user.TFAConfirmCode!.Value}</h4></p>";
            await mailService.SendAsync(to, subject, body);
            var res = new LoginCommandResponse() { TFACode = user.TFACode!.Value };
            return res;
        }

    }
}
