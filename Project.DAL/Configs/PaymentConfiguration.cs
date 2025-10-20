using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Domain.Entities;

namespace Project.DAL.Configs;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> b)
    {
        b.ToTable("payments", "orders_db");
        b.HasKey(p => p.PaymentId).HasName("pk_payments");
        b.Property(p => p.PaymentId).HasColumnName("payment_id");
        b.Property(p => p.OrderId).HasColumnName("order_id");
        b.Property(p => p.Amount).HasColumnName("amount").HasPrecision(10, 2).IsRequired();
        b.Property(p => p.PaidAt).HasColumnName("paid_at").HasDefaultValueSql("CURRENT_TIMESTAMP");
        b.HasOne(p => p.Order).WithOne().HasForeignKey<Payment>(p => p.OrderId).HasConstraintName("fk_payments_order");
    }
}