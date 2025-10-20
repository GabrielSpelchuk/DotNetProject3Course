using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Domain.Entities;

namespace Project.DAL.Configs;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> b)
    {
        b.ToTable("customers", "orders_db");
        b.HasKey(c => c.CustomerId).HasName("pk_customers");
        b.Property(c => c.CustomerId).HasColumnName("customer_id");
        b.Property(c => c.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
        b.Property(c => c.Email).HasColumnName("email").HasMaxLength(100).IsRequired();
        b.Property(c => c.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP");
        b.HasIndex(c => c.Email).IsUnique().HasDatabaseName("ux_customers_email");
    }
}