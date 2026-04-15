using Bob_sells_corn.Models;
using Microsoft.EntityFrameworkCore;

namespace Bob_sells_corn.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Client> Clients { get; set; } = null!;
    public DbSet<Purchase> Purchases { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Client>().HasData(
            new Client { Id = 1, Name = "Bob" },
            new Client { Id = 2, Name = "Alice" },
            new Client { Id = 3, Name = "Charlie" }
        );
    }
}
