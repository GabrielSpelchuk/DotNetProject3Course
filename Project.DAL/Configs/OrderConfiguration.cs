using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Domain.Entities;

namespace Project.DAL.Configs;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> b)
    {
        b.ToTable("orders", schema: "orders_db");
        b.HasKey(o => o.OrderId);
        b.Property(o => o.OrderId).HasColumnName("order_id");
        b.Property(o => o.CustomerId).HasColumnName("customer_id");
        b.Property(o => o.Status).HasColumnName("status").HasMaxLength(50).HasDefaultValue("Pending");
        b.Property(o => o.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP");
        b.HasOne(o => o.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CustomerId)
            .HasConstraintName("fk_orders_customers");
    }
}
