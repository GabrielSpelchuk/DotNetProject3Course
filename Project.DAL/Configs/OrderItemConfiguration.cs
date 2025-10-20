using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Domain.Entities;

namespace Project.DAL.Configs;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> b)
    {
        b.ToTable("order_items", "orders_db");
        b.HasKey(oi => new { oi.OrderId, oi.ProductId }).HasName("pk_order_items");
        b.Property(oi => oi.OrderId).HasColumnName("order_id");
        b.Property(oi => oi.ProductId).HasColumnName("product_id");
        b.Property(oi => oi.Quantity).HasColumnName("quantity").IsRequired();
        b.Property(oi => oi.UnitPrice).HasColumnName("unit_price").HasPrecision(10, 2).IsRequired();
        b.HasOne(oi => oi.Order).WithMany(o => o.Items).HasForeignKey(oi => oi.OrderId).HasConstraintName("fk_orderitems_orders");
    }
}