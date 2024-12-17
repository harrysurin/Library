
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LibraryRepository.Models;


public class LibraryContext : IdentityDbContext<User, Role, Guid>
{
    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<RentHistory> RentHistory => Set<RentHistory>();

    public LibraryContext() => Database.EnsureCreated();

    public LibraryContext(DbContextOptions<LibraryContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
    }
}

