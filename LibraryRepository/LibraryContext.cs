
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LibraryRepository.Models;


public class LibraryContext : IdentityDbContext<User, Role, Guid>
{
    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<RentHistory> RentHistory => Set<RentHistory>();
    public DbSet<BookPictures> BookPictures => Set<BookPictures>();

    public LibraryContext() : base() {}

    public LibraryContext(DbContextOptions<LibraryContext> options)
        : base(options)
    {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Book>().HasIndex(u => u.ISBN);
        var dbSeeder = new DbSeeder(modelBuilder);
        dbSeeder.SeedRoles();
    }
}

