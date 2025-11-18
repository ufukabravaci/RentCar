using RentCarServer.Domain.Users;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Auth;
public sealed record CheckForgotPasswordCodeCommand(
    Guid ForgotPasswordCode) : IRequest<Result<bool>>;

internal sealed class CheckForgotPasswordCodeCommandHandler(IUserRepository userRepository) :
    IRequestHandler<CheckForgotPasswordCodeCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(CheckForgotPasswordCodeCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FirstOrDefaultAsync(p => p.ForgotPasswordCode != null &&
            p.ForgotPasswordCode.Value == request.ForgotPasswordCode &&
            p.IsForgotPasswordCompleted.Value == false);
        if (user is null)
        {
            return Result<bool>.Failure("Şifre sıfırlama değeriniz geçersiz.");
        }
        var fpDate = user.ForgotPasswordDate!.Value;
        var now = DateTimeOffset.UtcNow;

        if (now > fpDate.AddHours(24))
        {
            return Result<bool>.Failure("Şifre sıfırlama süreniz dolmuş.");
        }
        return true;
    }
}
