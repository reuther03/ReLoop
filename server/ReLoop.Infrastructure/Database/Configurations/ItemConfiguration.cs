using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReLoop.Api.Domain.Item;
using ReLoop.Shared.Abstractions.Kernel.ValueObjects.Ids;

namespace ReLoop.Infrastructure.Database.Configurations;

internal sealed class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.ToTable("Items");

        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id)
            .HasConversion(id => id.Value, value => ItemId.From(value))
            .ValueGeneratedNever();

        builder.Property(i => i.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(i => i.Description)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(i => i.ImageData)
            .IsRequired();

        builder.Property(i => i.Price)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(i => i.Category)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(i => i.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(i => i.SellerId)
            .HasConversion(id => id.Value, value => UserId.From(value))
            .IsRequired();

        builder.Property(i => i.BuyerId)
            .HasConversion(id => id!.Value, value => UserId.From(value));

        builder.Property(i => i.CreatedAt)
            .IsRequired();

        builder.HasOne(i => i.Seller)
            .WithMany()
            .HasForeignKey(i => i.SellerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.Buyer)
            .WithMany()
            .HasForeignKey(i => i.BuyerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(i => i.Status);
        builder.HasIndex(i => i.Category);
        builder.HasIndex(i => i.SellerId);
    }
}
