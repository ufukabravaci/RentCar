using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentCarServer.Domain.Branchs;
using RentCarServer.Domain.Branchs.ValueObjects;

namespace RentCarServer.Infrastructure.Configurations;
internal sealed class BranchConfiguration : IEntityTypeConfiguration<Branch>
{
    public void Configure(EntityTypeBuilder<Branch> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasConversion(p => p.Value, v => new Name(v))
            .HasColumnName("Name")
            .HasMaxLength(100)
            .IsRequired();

        builder.OwnsOne(x => x.Address, addressBuilder =>
        {
            addressBuilder.Property(p => p.City)
                .HasColumnName("City")
                .HasMaxLength(50)
                .IsRequired();

            addressBuilder.Property(p => p.District)
                .HasColumnName("District")
                .HasMaxLength(50)
                .IsRequired();

            addressBuilder.Property(p => p.FullAddress)
                .HasColumnName("FullAddress")
                .HasMaxLength(500)
                .IsRequired();

            addressBuilder.Property(p => p.Email)
                .HasColumnName("Email")
                .HasMaxLength(150)
                .IsRequired();

            addressBuilder.Property(p => p.PhoneNumber1)
                .HasColumnName("PhoneNumber1")
                .HasMaxLength(20)
                .IsRequired();

            addressBuilder.Property(p => p.PhoneNumber2)
                .HasColumnName("PhoneNumber2")
                .HasMaxLength(20)
                .IsRequired(false); // İkinci numara opsiyonel olsun
        });
    }
}
