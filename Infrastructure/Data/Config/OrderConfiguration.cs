using Core.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

// Give order entity some configuration.
public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        // Configure the relationship where the target entity is owned by or part of this entity.
        //builder.Property(p => p.ShipToAddress.FirstName).IsRequired().HasMaxLength(100);
        builder.OwnsOne(o => o.ShipToAddress, a =>
        {
            a.WithOwner();
        });
        builder.Property(s => s.Status).HasConversion(
            o => o.ToString(),
            // Cast the OrderStatus and pass the o as a string now.
            o => (OrderStatus) Enum.Parse(typeof(OrderStatus), o)
            );
        
        // If we delete an order we also delete any order items that are part of this particular order and
        // the order it's going to have one to many relationship with the order items.
        builder.HasMany(o => o.OrderItems).WithOne().OnDelete(DeleteBehavior.Cascade);

    }
}