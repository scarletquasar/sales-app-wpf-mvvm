using Microsoft.EntityFrameworkCore;
using SalesApplication.Domain.Business;

namespace SalesApplication.Database
{
    public class GeneralContext : DbContext
    {
        public GeneralContext(DbContextOptions<GeneralContext> opt) : base(opt) { }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SoldProduct> SoldProducts { get; set; }
        private void CustomerConfig(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");
                entity.HasKey(c => c.Id).HasName("customer_id");
                entity.Property(c => c.Id).HasColumnName("customer_id").ValueGeneratedOnAdd();
                entity.Property(c => c.Name).HasColumnName("name").HasMaxLength(40);
            });
        }

        private void ProductConfig(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");
                entity.HasKey(p => p.Id).HasName("product_id");
                entity.Property(p => p.Id).HasColumnName("product_id").ValueGeneratedOnAdd();
                entity.Property(p => p.Price).HasColumnName("product_price");
                entity.Property(p => p.Description).HasColumnName("product_description");
                entity.Property(p => p.Stock).HasColumnName("product_stock");
            });
        }

        private void SaleConfig(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sale>(entity =>
            {
                entity.ToTable("Sale");
                entity.HasKey(s => s.Id).HasName("sale_id");
                entity.HasMany(s => s.Products).WithOne();
                entity.Property(s => s.Id).HasColumnName("sale_id").ValueGeneratedOnAdd();
                entity.Property(s => s.TotalPrice).HasColumnName("total_amount");
                entity.Property(s => s.CustomerId).HasColumnName("client_id");
                entity.Property(s => s.CreatedAt).HasColumnName("sale_date");
                entity.Navigation(s => s.Products).UsePropertyAccessMode(PropertyAccessMode.Property);
            });
        }

        private void SoldProductConfig(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SoldProduct>(entity =>
            {
                entity.ToTable("SoldProducts");
                entity.HasKey(p => p.Id).HasName("id");
                entity.Property(p => p.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.Property(p => p.SaleId).HasColumnName("sale_id");
                entity.HasOne(p => p.SaleEntity).WithMany(p => p.Products).HasForeignKey(p => p.SaleId);
                entity.Property(p => p.ProductId).HasColumnName("product_id");
                entity.Property(p => p.TotalPrice).HasColumnName("product_price");
                entity.Property(p => p.ProductQuantity).HasColumnName("product_amount");
            });
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("salesmanagement");
            CustomerConfig(modelBuilder);
            SaleConfig(modelBuilder);
            ProductConfig(modelBuilder);
            SoldProductConfig(modelBuilder);
        }
    }
}
