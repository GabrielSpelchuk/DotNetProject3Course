using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Domain.Entities;

namespace Project.DAL.Configs;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> b)
    {
        b.ToTable("products", schema: "catalog_db");
        b.HasKey(p => p.ProductId);
        b.Property(p => p.ProductId).HasColumnName("product_id");
        b.Property(p => p.Name).HasMaxLength(150).IsRequired().HasColumnName("name");
        b.Property(p => p.Price).HasColumnName("price").HasPrecision(10,2);
        b.Property(p => p.StockQuantity).HasColumnName("stock_quantity").IsRequired();
        b.Property(p => p.SupplierId).HasColumnName("supplier_id");
        b.HasOne(p => p.Supplier)
            .WithMany()
            .HasForeignKey(p => p.SupplierId)
            .HasConstraintName("fk_products_suppliers");
    }
}