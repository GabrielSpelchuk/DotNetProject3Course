using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Domain.Entities;

namespace Project.DAL.Configs;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> b)
    {
        b.ToTable("categories", "catalog_db");
        b.HasKey(c => c.CategoryId).HasName("pk_categories");
        b.Property(c => c.CategoryId).HasColumnName("category_id");
        b.Property(c => c.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
    }
}