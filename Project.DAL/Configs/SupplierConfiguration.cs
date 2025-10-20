using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Domain.Entities;

namespace Project.DAL.Configs;

public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> b)
    {
        b.ToTable("suppliers", "catalog_db");
        b.HasKey(s => s.SupplierId).HasName("pk_suppliers");
        b.Property(s => s.SupplierId).HasColumnName("supplier_id");
        b.Property(s => s.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
        b.Property(s => s.Country).HasColumnName("country").HasMaxLength(100);
        b.Property(s => s.Rating).HasColumnName("rating").HasPrecision(3, 2).HasDefaultValue(0m);
    }
}