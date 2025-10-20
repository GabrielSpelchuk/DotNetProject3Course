using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Domain.Entities;

namespace Project.DAL.Configs;

public class ProductDetailConfiguration : IEntityTypeConfiguration<ProductDetail>
{
    public void Configure(EntityTypeBuilder<ProductDetail> b)
    {
        b.ToTable("product_details", "catalog_db");
        b.HasKey(d => d.ProductId).HasName("pk_product_details");
        b.Property(d => d.ProductId).HasColumnName("product_id");
        b.Property(d => d.Description).HasColumnName("description");
        b.Property(d => d.ShippingTime).HasColumnName("shipping_time");
        b.Property(d => d.ReturnPolicy).HasColumnName("return_policy");
        b.HasOne(d => d.Product).WithOne(p => p.Detail).HasForeignKey<ProductDetail>(d => d.ProductId);
    }
}