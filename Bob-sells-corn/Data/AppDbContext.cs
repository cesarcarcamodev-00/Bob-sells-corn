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

        //Create new Ids without conflicting with seed entities.
        modelBuilder.Entity<Client>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<Client>().HasData(
            new Client { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Bob" },
            new Client { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "Alice" },
            new Client { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), Name = "Charlie" }
        );
    }
}
