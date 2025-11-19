using GenericRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RentCarServer.Domain.Abstraction;
using RentCarServer.Domain.LoginTokens;
using RentCarServer.Domain.Users;
using System.Security.Claims;

namespace RentCarServer.Infrastructure.Context;
internal class ApplicationDbContext : DbContext, IUnitOfWork
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public ApplicationDbContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public DbSet<User> Users { get; set; }
    public DbSet<LoginToken> LoginTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Tüm configurationları uygulamak için. CarConfig UserConfig vs IEntityTypeConfiguration<T> implemente etmesi yeterli.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        modelBuilder.ApplyGlobalFilters();
        base.OnModelCreating(modelBuilder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<IdentityId>().HaveConversion<IdentityIdValueConverter>();
        configurationBuilder.Properties<decimal>().HaveColumnType("decimal(18,2)");
        configurationBuilder.Properties<string>().HaveColumnType("varchar(MAX)");
        base.ConfigureConventions(configurationBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<Entity>();

        string? userIdString = _httpContextAccessor
            .HttpContext?
            .User
            .Claims
            .FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier)?
            .Value;

        IdentityId? identityId = null;

        if (!string.IsNullOrEmpty(userIdString) && Guid.TryParse(userIdString, out Guid userIdGuid))
        {
            identityId = new IdentityId(userIdGuid);
        }

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property(p => p.CreatedAt).CurrentValue = DateTimeOffset.Now;
                // Sadece kullanıcı varsa set et
                if (identityId is not null)
                {
                    entry.Property(p => p.CreatedBy).CurrentValue = identityId;
                }
            }

            if (entry.State == EntityState.Modified)
            {
                if (entry.Property(p => p.IsDeleted).CurrentValue == true)
                {
                    entry.Property(p => p.DeletedAt).CurrentValue = DateTimeOffset.Now;

                    if (identityId is not null)
                        entry.Property(p => p.DeletedBy).CurrentValue = identityId;
                }
                else
                {
                    entry.Property(p => p.UpdatedAt).CurrentValue = DateTimeOffset.Now;

                    if (identityId is not null)
                        entry.Property(p => p.UpdatedBy).CurrentValue = identityId;
                }
            }

            if (entry.State == EntityState.Deleted)
            {
                throw new ArgumentException("Db'den direkt silme işlemi yapamazsınız (Soft Delete Kullanın)");
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}


internal sealed class IdentityIdValueConverter : ValueConverter<IdentityId, Guid>
{
    //Veritabanına kayıt ederken IdentityId.Value alır.Veritabanından okurken yeni bir IdentityId(guid)nesnesi yaratır
    public IdentityIdValueConverter() : base(m => m.Value, m => new IdentityId(m)) { }
}