using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Domain.Entities;

namespace Project.DAL.Configs;

public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> b)
    {
        b.ToTable("product_images", "catalog_db");
        b.HasKey(i => i.ImageId).HasName("pk_product_images");
        b.Property(i => i.ImageId).HasColumnName("image_id");
        b.Property(i => i.Url).HasColumnName("url").IsRequired();
        b.Property(i => i.ProductId).HasColumnName("product_id");
        b.HasOne(i => i.Product).WithMany(p => p.Images).HasForeignKey(i => i.ProductId);
    }
}