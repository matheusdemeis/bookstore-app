using Microsoft.EntityFrameworkCore;
using Bookstore.Models;

namespace Bookstore.Data;

public class BookstoreDb : DbContext
{
    public BookstoreDb(DbContextOptions<BookstoreDb> options)
        : base(options)
    {
    }

    public DbSet<Book> Books { get; set; }   
    public DbSet<Order> Orders { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Order>().OwnsMany(e => e.OrderItems);
    }
}