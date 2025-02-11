
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LibraryRepository.Models;


public class LibraryContext : IdentityDbContext<User, Role, Guid>
{
    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<RentHistory> RentHistory => Set<RentHistory>();
    public DbSet<BookPictures> BookPictures => Set<BookPictures>();
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public LibraryContext() : base() {}

    public LibraryContext(DbContextOptions<LibraryContext> options)
        : base(options)
    {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Book>().HasIndex(u => u.ISBN);
        modelBuilder.Entity<Book>().Property(x => x.ISBN).IsRequired();
        modelBuilder.Entity<Author>().HasKey(x => x.AuthorId);
        modelBuilder.Entity<Author>().Property(x => x.FirstName).IsRequired();
        modelBuilder.Entity<Book>().HasKey(x => x.BookId);
        modelBuilder.Entity<Book>().Property(x => x.Title).IsRequired();
        modelBuilder.Entity<RentHistory>().HasKey(x => x.Id);
        modelBuilder.Entity<BookPictures>().HasKey(x => x.Id);
        modelBuilder.Entity<BookPictures>().Ignore(x => x.Picture);
        modelBuilder.Entity<BookPictures>().Ignore(x => x.PictureBytes);
        modelBuilder.Entity<BookPictures>().Ignore(x => x.FileExtension);
        var dbSeeder = new DbSeeder(modelBuilder);
        dbSeeder.SeedRoles();
    }
}

