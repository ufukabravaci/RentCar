using FluentValidation;
using GenericRepository;
using Microsoft.EntityFrameworkCore;
using RentCarServer.Domain.LoginTokens;
using RentCarServer.Domain.Users;
using RentCarServer.Domain.Users.ValueObjects;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Auth;
public sealed record ResetPasswordCommand(
    Guid ForgotPasswordCode,
    string NewPassword,
    bool LogoutAllDevices) : IRequest<Result<string>>;

public sealed class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(p => p.NewPassword)
            .NotEmpty().WithMessage("Yeni şifre alanı boş olamaz.")
            .MinimumLength(8).WithMessage("Şifre en az 8 karakter olmalıdır.")
            .Matches("[A-Z]").WithMessage("Şifre en az bir büyük harf içermelidir.")
            .Matches("[a-z]").WithMessage("Şifre en az bir küçük harf içermelidir.")
            .Matches("[0-9]").WithMessage("Şifre en az bir rakam içermelidir.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Şifre en az bir özel karakter içermelidir.");
    }
}

internal sealed class ResetPasswordCommandHandler(
    IUserRepository userRepository,
    ILoginTokenRepository loginTokenRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<ResetPasswordCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        ForgotPasswordCode codeObj = request.ForgotPasswordCode;
        var user = await userRepository.FirstOrDefaultAsync(p => p.ForgotPasswordCode != null &&
        p.ForgotPasswordCode == codeObj &&
        p.IsForgotPasswordCompleted == new IsForgotPasswordCompleted(false));
        if (user is null)
        {
            return Result<string>.Failure("Şifre sıfırlama değeriniz geçersiz.");
        }
        var fpDate = user.ForgotPasswordDate!.Value;
        var now = DateTimeOffset.UtcNow;

        if (now > fpDate.AddHours(24))
        {
            return Result<string>.Failure("Şifre sıfırlama süreniz dolmuş.");
        }
        Password password = new(request.NewPassword);
        user.SetPassword(password);
        userRepository.Update(user);
        if (request.LogoutAllDevices)
        {
            var loginTokens = await loginTokenRepository.Where(p => p.UserId == user.Id &
                p.IsActive == true).ToListAsync(cancellationToken);

            foreach (var item in loginTokens)
            {
                item.SetIsActive(false);
            }
            loginTokenRepository.UpdateRange(loginTokens);
        }
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return "Şifre başarıyla sıfırlandı.Yeni şifrenizle giriş yapabilirsiniz.";
    }
}
