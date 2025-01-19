using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using LibraryRepository.Models;
using Moq;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;

namespace LibraryRepository.Tests;

public class LibraryUnitTests
{
    [Fact]
    public async Task AddAsyncTest()
    {
        var options = new DbContextOptionsBuilder<LibraryContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        
        using var context = new LibraryContext(options);
        using var unitOfWork = new UnitOfWork(context);

        var authors = GetAuthors();

        foreach(var author in authors)
        {
            await unitOfWork.Authors.AddAsync(author);
        }
        await unitOfWork.CompleteAsync();

        var authorFromDb = await unitOfWork.Authors.FirstOrDefaultAsync(u => u.AuthorId == authors[1].AuthorId);
        Assert.NotNull(authorFromDb);
        Assert.Equal("FirstNameOfAuthor2", authorFromDb.FirstName);
        Assert.Equal("UK", authorFromDb.Country);

    }

     [Fact]
    public async Task AddAsyncIfObjIsNullTest()
    {
        var options = new DbContextOptionsBuilder<LibraryContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        
        using var context = new LibraryContext(options);
        using var unitOfWork = new UnitOfWork(context);

        await Assert.ThrowsAsync<ArgumentNullException>(() => unitOfWork.Authors.AddAsync(null));
    }

    [Fact]
    public async Task GetByIdTest()
    {
        var options = new DbContextOptionsBuilder<LibraryContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        
        using var context = new LibraryContext(options);
        using var unitOfWork = new UnitOfWork(context);
        var authors = GetAuthors();

        foreach(var author in authors)
        {
            await unitOfWork.Authors.AddAsync(author);
        }
        await unitOfWork.CompleteAsync();

        var authorId = authors[1].AuthorId;

        var authorFromDb = await unitOfWork.Authors.GetByIdAsync(authorId);
        Assert.NotNull(authorFromDb);
        Assert.Same(authors[1], authorFromDb);
    }

    [Fact]
    public async Task GetByWrongIdTest()
    {
        var options = new DbContextOptionsBuilder<LibraryContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        
        using var context = new LibraryContext(options);
        using var unitOfWork = new UnitOfWork(context);
        var authors = GetAuthors();

        foreach(var author in authors)
        {
            await unitOfWork.Authors.AddAsync(author);
        }
        await unitOfWork.CompleteAsync();

        var authorId = Guid.NewGuid();

        var authorFromDb = await unitOfWork.Authors.GetByIdAsync(authorId);
        Assert.Null(authorFromDb);

    }



    private List<Author> GetAuthors()
    {
        var author1 = new Author
        {
            AuthorId = Guid.NewGuid(),
            FirstName = "FirstNameOfAuthor1",
            LastName = "LastNameOfAuthor1",
            Country = "USA",
            DateOfBirth = new DateTime(1966, 11, 27)
        };
        
        var author2 = new Author
        {
            AuthorId = Guid.NewGuid(),
            FirstName = "FirstNameOfAuthor2",
            LastName = "LastNameOfAuthor2",
            Country = "UK",
            DateOfBirth = new DateTime(1970, 3, 15)
        };

        var author3 = new Author
        {
            AuthorId = Guid.NewGuid(),
            FirstName = "FirstNameOfAuthor3",
            LastName = "LastNameOfAuthor3",
            Country = "Canada",
            DateOfBirth = new DateTime(1980, 7, 20)
        };

        var author4 = new Author
        {
            AuthorId = Guid.NewGuid(),
            FirstName = "FirstNameOfAuthor4",
            LastName = "LastNameOfAuthor4",
            Country = "Australia",
            DateOfBirth = new DateTime(1992, 12, 5)
        };

        var author5 = new Author
        {
            AuthorId = Guid.NewGuid(),
            FirstName = "FirstNameOfAuthor5",
            LastName = "LastNameOfAuthor5",
            Country = "India",
            DateOfBirth = new DateTime(1975, 8, 18)
        };


        return new List<Author>()
            { author1, author2, author3, author4, author5 };
    }
}
