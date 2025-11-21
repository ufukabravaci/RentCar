using GenericRepository;
using Microsoft.EntityFrameworkCore;
using RentCarServer.Domain.LoginTokens;

namespace RentCarServer.WebAPI;

public class CheckLoginTokenBackgroundService
    (IServiceScopeFactory serviceScopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromDays(1));
        do
        {
            try
            {
                using (var scope = serviceScopeFactory.CreateScope())
                {
                    var loginTokenRepository = scope.ServiceProvider.GetRequiredService<ILoginTokenRepository>();
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                    var now = DateTimeOffset.Now;
                    var activeList = await loginTokenRepository
                        .Where(p => p.IsActive == true && p.ExpireDate < now)
                        .ToListAsync(stoppingToken);

                    if (activeList.Any())
                    {
                        foreach (var loginToken in activeList)
                        {
                            loginToken.SetIsActive(false);
                        }
                        if (activeList.Any())
                        {
                            loginTokenRepository.UpdateRange(activeList);
                            await unitOfWork.SaveChangesAsync(stoppingToken);
                        }

                        Console.WriteLine($"{DateTime.Now}: {activeList.Count} adet token pasife çekildi.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"BackgroundService Hatası: {ex.Message}");
            }
        }
        while (await timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested);
    }
}