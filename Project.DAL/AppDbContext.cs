using Microsoft.EntityFrameworkCore;
using Project.Domain.Entities;

namespace Project.DAL
{
    public class AppDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Supplier> Suppliers { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<Payment> Payments { get; set; } = null!;

        public DbSet<OrderItem> OrderItems { get; set; } = null!;
        public DbSet<ProductCategory> ProductCategories { get; set; } = null!;
        public DbSet<ProductImage> ProductImages { get; set; } = null!;
        public DbSet<ProductDetail> ProductDetails { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Supplier>().ToTable("suppliers", "catalog_db");
            modelBuilder.Entity<Category>().ToTable("categories", "catalog_db");
            modelBuilder.Entity<Product>().ToTable("products", "catalog_db");
            modelBuilder.Entity<ProductCategory>().ToTable("product_categories", "catalog_db");
            modelBuilder.Entity<ProductDetail>().ToTable("product_details", "catalog_db");
            modelBuilder.Entity<ProductImage>().ToTable("product_images", "catalog_db");

            modelBuilder.Entity<Customer>().ToTable("customers", "orders_db");
            modelBuilder.Entity<Order>().ToTable("orders", "orders_db");
            modelBuilder.Entity<OrderItem>().ToTable("order_items", "orders_db");
            modelBuilder.Entity<Payment>().ToTable("payments", "orders_db");

            modelBuilder.Entity<OrderItem>().HasKey(oi => new { oi.OrderId, oi.ProductId });
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order).WithMany(o => o.Items).HasForeignKey(oi => oi.OrderId);
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product).WithMany().HasForeignKey(oi => oi.ProductId);

            modelBuilder.Entity<ProductCategory>().HasKey(pc => new { pc.ProductId, pc.CategoryId });
            modelBuilder.Entity<ProductDetail>().HasKey(pd => pd.ProductId);
            modelBuilder.Entity<ProductImage>().HasKey(pi => pi.ImageId);

            modelBuilder.Entity<ProductDetail>()
                .HasOne(pd => pd.Product)
                .WithOne(p => p.Detail)
                .HasForeignKey<ProductDetail>(pd => pd.ProductId);

            modelBuilder.Entity<ProductImage>()
                .HasOne(pi => pi.Product)
                .WithMany(p => p.Images)
                .HasForeignKey(pi => pi.ProductId);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Order)
                .WithOne()
                .HasForeignKey<Payment>(p => p.OrderId);
            
            modelBuilder.Entity<Supplier>().HasData(
                new Supplier { SupplierId = 1, Name = "Tech Global", Country = "USA", Rating = 4.5m },
                new Supplier { SupplierId = 2, Name = "Book World", Country = "UK", Rating = 4.8m }
            );
            
            modelBuilder.Entity<Customer>().HasData(
                new Customer 
                { 
                    CustomerId = 1, 
                    Name = "Alice Johnson", 
                    Email = "alice@mail.com", 
                    CreatedAt = new DateTime(2024, 10, 15, 0, 0, 0, DateTimeKind.Utc)
                },
                new Customer 
                { 
                    CustomerId = 2, 
                    Name = "Bob Williams", 
                    Email = "bob@mail.com", 
                    CreatedAt = new DateTime(2024, 10, 16, 0, 0, 0, DateTimeKind.Utc)
                }
            );

            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Electronics" },
                new Category { CategoryId = 2, Name = "Books" },
                new Category { CategoryId = 3, Name = "Gadgets" }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product { ProductId = 1, SupplierId = 1, Name = "Laptop X1", Price = 999.99m, StockQuantity = 10 },
                new Product
                {
                    ProductId = 2, SupplierId = 2, Name = "EF Core Guide", Price = 49.50m, StockQuantity = 50
                },
                new Product
                {
                    ProductId = 3, SupplierId = 1, Name = "Smartwatch Z", Price = 199.00m, StockQuantity = 25
                }
            );

            modelBuilder.Entity<ProductDetail>().HasData(
                new ProductDetail
                {
                    ProductId = 1, Description = "Powerful laptop for development.", ShippingTime = "2-3 Days",
                    ReturnPolicy = "30 days"
                },
                new ProductDetail
                {
                    ProductId = 2, Description = "Complete guide to Entity Framework Core.", ShippingTime = "1-2 Days",
                    ReturnPolicy = "15 days"
                },
                new ProductDetail
                {
                    ProductId = 3, Description = "Smartwatch with health monitoring.", ShippingTime = "2-3 Days",
                    ReturnPolicy = "30 days"
                }
            );

            modelBuilder.Entity<ProductImage>().HasData(
                new ProductImage { ImageId = 1, ProductId = 1, Url = "/img/laptop-x1.jpg" },
                new ProductImage { ImageId = 2, ProductId = 1, Url = "/img/laptop-x1-side.jpg" },
                new ProductImage { ImageId = 3, ProductId = 3, Url = "/img/smartwatch-z.jpg" }
            );

            modelBuilder.Entity<ProductCategory>().HasData(
                new ProductCategory { ProductId = 1, CategoryId = 1 },
                new ProductCategory { ProductId = 1, CategoryId = 3 },
                new ProductCategory { ProductId = 2, CategoryId = 2 },
                new ProductCategory { ProductId = 3, CategoryId = 1 },
                new ProductCategory { ProductId = 3, CategoryId = 3 }
            );

            modelBuilder.Entity<Order>().HasData(
                new Order 
                { 
                    OrderId = 1, 
                    CustomerId = 1, 
                    SupplierId = 1, 
                    Status = "Completed", 
                    CreatedAt = new DateTime(2024, 10, 17, 0, 0, 0, DateTimeKind.Utc)
                },
                new Order 
                { 
                    OrderId = 2, 
                    CustomerId = 2, 
                    SupplierId = 2, 
                    Status = "Pending", 
                    CreatedAt = new DateTime(2024, 10, 18, 0, 0, 0, DateTimeKind.Utc)
                }
            );

            modelBuilder.Entity<OrderItem>().HasData(
                new OrderItem { OrderId = 1, ProductId = 1, Quantity = 1, UnitPrice = 999.99m },
                new OrderItem { OrderId = 1, ProductId = 3, Quantity = 2, UnitPrice = 199.00m },
                new OrderItem { OrderId = 2, ProductId = 2, Quantity = 5, UnitPrice = 49.50m }
            );

            
            modelBuilder.Entity<Payment>().HasData(
                new Payment 
                { 
                    PaymentId = 1, 
                    OrderId = 1, 
                    Amount = 1397.99m, 
                    PaidAt = new DateTime(2024, 10, 17, 10, 30, 0, DateTimeKind.Utc)
                }
            );
        }
    }
}
