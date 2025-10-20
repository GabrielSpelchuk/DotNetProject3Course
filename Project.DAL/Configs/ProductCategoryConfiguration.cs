using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Domain.Entities;

namespace Project.DAL.Configs;

public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
{
    public void Configure(EntityTypeBuilder<ProductCategory> b)
    {
        b.ToTable("product_categories", "catalog_db");
        b.HasKey(pc => new { pc.ProductId, pc.CategoryId }).HasName("pk_product_categories");
        b.HasOne(pc => pc.Product).WithMany(p => p.ProductCategories).HasForeignKey(pc => pc.ProductId);
        b.HasOne(pc => pc.Category).WithMany(c => c.ProductCategories).HasForeignKey(pc => pc.CategoryId);
    }
}