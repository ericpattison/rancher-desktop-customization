using Microsoft.EntityFrameworkCore;

namespace classicmodels;

public class ClassicModelsContext : DbContext
{
    public ClassicModelsContext(DbContextOptions<ClassicModelsContext> options) : base(options){}
    public DbSet<ProductLines> ProductLines { get; set; }
    public DbSet<Products> Products { get; set; }
    public DbSet<Orders> Orders { get; set; }
    public DbSet<OrderDetails> OrderDetails { get; set; }
    public DbSet<Customers> Customers { get; set; }
    public DbSet<Employees> Employees { get; set; }
    public DbSet<Offices> Offices { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.Entity<ProductLines>().HasNoKey();
        modelBuilder.Entity<Products>().HasNoKey();
        modelBuilder.Entity<Orders>().HasNoKey();
        modelBuilder.Entity<OrderDetails>().HasNoKey();
        modelBuilder.Entity<Customers>().HasNoKey();
        modelBuilder.Entity<Employees>().HasNoKey();
        modelBuilder.Entity<Offices>().HasNoKey();

        base.OnModelCreating(modelBuilder);
    }
}