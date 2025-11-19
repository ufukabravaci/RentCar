using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentCarServer.Domain.LoginTokens;
using RentCarServer.Domain.LoginTokens.ValueObjects;

namespace RentCarServer.Infrastructure.Configurations;
internal sealed class LoginTokenConfiguration : IEntityTypeConfiguration<LoginToken>
{
    public void Configure(EntityTypeBuilder<LoginToken> builder)
    {
        //Buradaki iş mantığı doğrulamaları burada değil LoginToken entitysinde yapılır.
        //Burası sadece veritabanı şemasını ilgilendirir.
        builder.HasKey(x => x.Id);

        builder.Property(p => p.Token)
            .HasConversion(x => x.Value, v => new Token(v))
            .HasColumnName("Token")
            .IsRequired()
            .HasColumnType("varchar(MAX)");

        builder.Property(p => p.IsActive)
            .HasConversion(x => x.Value, v => new IsActive(v))
            .HasColumnName("IsActive")
            .IsRequired();

        builder.Property(p => p.ExpireDate)
            .HasConversion(x => x.Value, v => new ExpireDate(v))
            .HasColumnName("ExpireDate")
            .IsRequired();
    }
}
