using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentCarServer.Domain.Users;
using RentCarServer.Domain.Users.ValueObjects;

namespace RentCarServer.Infrastructure.Configurations;
internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(i => i.FirstName)
            .HasConversion(p => p.Value, v => new FirstName(v))
            .HasColumnName("FirstName")
            .IsRequired();

        builder.Property(i => i.LastName)
            .HasConversion(p => p.Value, v => new LastName(v))
            .HasColumnName("LastName")
            .IsRequired();

        builder.Property(i => i.FullName)
            .HasConversion(p => p.Value, v => new FullName(v))
            .HasColumnName("FullName");

        builder.Property(i => i.Email)
            .HasConversion(p => p.Value, v => new Email(v))
            .HasColumnName("Email")
            .HasMaxLength(100) // Index performansı için
            .IsRequired();

        builder.HasIndex(x => x.Email).IsUnique();

        builder.Property(i => i.UserName)
            .HasConversion(p => p.Value, v => new UserName(v))
            .HasColumnName("UserName")
            .HasMaxLength(100) // Index performansı için
            .IsRequired();

        builder.HasIndex(x => x.UserName).IsUnique();

        // --- DİĞERLERİ ---

        builder.Property(i => i.ForgotPasswordCode)
            .HasConversion(p => p!.Value, v => new ForgotPasswordCode(v))
            .HasColumnName("ForgotPasswordCode")
            .IsRequired(false);

        builder.Property(i => i.ForgotPasswordDate)
            .HasConversion(p => p!.Value, v => new ForgotPasswordDate(v))
            .HasColumnName("ForgotPasswordDate")
            .IsRequired(false);

        builder.Property(i => i.IsForgotPasswordCompleted)
             .HasConversion(p => p.Value, v => new IsForgotPasswordCompleted(v))
             .HasColumnName("IsForgotPasswordCompleted")
             .IsRequired();

        builder.OwnsOne(i => i.Password, passwordBuilder =>
        {
            passwordBuilder.Property(p => p.PasswordHash)
                .HasColumnName("PasswordHash")
                .HasColumnType("varbinary(512)")
                .IsRequired();

            passwordBuilder.Property(p => p.PasswordSalt)
                .HasColumnName("PasswordSalt")
                .HasColumnType("varbinary(512)")
                .IsRequired();
        });
    }
}
