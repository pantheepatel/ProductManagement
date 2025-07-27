namespace ProductManagement.Api;
public class ProductDbContext : DbContext
{
    public ProductDbContext(DbContextOptions<ProductDbContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductPrice> ProductPrices { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<InvoiceDetail> InvoiceDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Category
        modelBuilder.Entity<Category>(entity =>
        {
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Description)
                .HasMaxLength(500);
        });

        // Product
        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Description)
                .HasMaxLength(500);
            entity.Property(e => e.Tax)
                .HasPrecision(18, 2)
                .IsRequired();
            entity.HasOne(e => e.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ProductPrice
        modelBuilder.Entity<ProductPrice>(entity =>
        {
            entity.Property(e => e.SeasonalPrice)
                .HasPrecision(18, 2)
                .IsRequired();
            entity.Property(e => e.StartDate)
                .IsRequired();
            entity.Property(e => e.EndDate)
                .IsRequired();
        });

        // Customer
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(200);
        });

        // Invoice
        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.Property(e => e.Date)
                .IsRequired();
            entity.Property(e => e.PriceTotal)
                .HasPrecision(18, 2)
                .IsRequired();
            entity.Property(e => e.TaxTotal)
                .HasPrecision(18, 2)
                .IsRequired();
            entity.Property(e => e.GrandTotal)
                .HasPrecision(18, 2)
                .IsRequired();
            entity.HasOne(e => e.Customer)
                .WithMany(c => c.Invoices)
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // InvoiceDetail
        modelBuilder.Entity<InvoiceDetail>(entity =>
        {
            entity.Property(e => e.Quantity)
                .IsRequired();
            entity.Property(e => e.UnitPrice)
                .HasPrecision(18, 2)
                .IsRequired();
            entity.Property(e => e.SubTotal)
                .HasPrecision(18, 2)
                .IsRequired();
            entity.Property(e => e.Tax)
                .HasPrecision(18, 2)
                .IsRequired();
            entity.Property(e => e.TaxTotal)
                .HasPrecision(18, 2)
                .IsRequired();
            entity.Property(e => e.GrandTotal)
                .HasPrecision(18, 2)
                .IsRequired();
            entity.HasOne(e => e.Invoice)
                .WithMany(i => i.InvoiceDetails)
                .HasForeignKey(e => e.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Product)
                .WithMany()
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Product>()
            .HasMany(p => p.Prices)
            .WithOne(pp => pp.Product)
            .HasForeignKey(pp => pp.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
