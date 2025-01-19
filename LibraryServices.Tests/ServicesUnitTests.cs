using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using LibraryRepository.Models;
using Moq;
using LibraryServices.Validation;
using System.Linq.Expressions;

namespace LibraryServices.Tests;

public class ServicesUnitTests
{
    [Fact]
    public async Task FindByNameTest()
    {
        string FirstName = "FirstNameOfAuthor4";

        var expectedAuthor = new Author
        {
            AuthorId = Guid.NewGuid(),
            FirstName = FirstName,
            LastName = "LastNameOfAuthor4",
            Country = "Australia",
            DateOfBirth = new DateTime(1992, 12, 5)
        };

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockRepository = new Mock<IRepository<Author>>();
        var authorValidator = new AuthorValidator();

        

        mockRepository
            .Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<Author, bool>>>()))
            .ReturnsAsync(expectedAuthor);

        mockUnitOfWork
            .Setup(x => x.Authors)
            .Returns(mockRepository.Object);

        var services = new AuthorService(mockUnitOfWork.Object, authorValidator);

        var result = await services.GetAuthorByName(FirstName);

        Assert.Same(expectedAuthor, result);
    }

    [Fact]
    public async Task GetAllTest()
    {
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockRepository = new Mock<IRepository<Author>>();
        var authorValidator = new AuthorValidator();

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

        var authors = new List<Author>{author1, author2};

        mockUnitOfWork
            .Setup(x => x.Authors)
            .Returns(mockRepository.Object);

        mockRepository
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(authors);
        
        var services = new AuthorService(mockUnitOfWork.Object, authorValidator);

        var result = await services.GetAllAsync();

        Assert.Same(authors, result);
    }

    [Fact]
    public async Task GetByIdTest()
    {
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockRepository = new Mock<IRepository<Author>>();
        var authorValidator = new AuthorValidator();
        
        var author1 = new Author
        {
            AuthorId = Guid.NewGuid(),
            FirstName = "FirstNameOfAuthor1",
            LastName = "LastNameOfAuthor1",
            Country = "USA",
            DateOfBirth = new DateTime(1966, 11, 27)
        };

        Guid authorId = author1.AuthorId;

         mockUnitOfWork
            .Setup(x => x.Authors)
            .Returns(mockRepository.Object);

        mockRepository
            .Setup(repo => repo.GetByIdAsync(authorId))
            .ReturnsAsync(author1);

        var services = new AuthorService(mockUnitOfWork.Object, authorValidator);

        var result = await services.GetByIdAsync(authorId);

        Assert.Same(author1, result);

    }

    [Fact]
    public async Task AddAsyncTest()
    {
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockRepository = new Mock<IRepository<Author>>();
        var authorValidator = new AuthorValidator();

        var author1 = new Author
        {
            AuthorId = Guid.NewGuid(),
            FirstName = "FirstNameOfAuthor1",
            LastName = "LastNameOfAuthor1",
            Country = "USA",
            DateOfBirth = new DateTime(1966, 11, 27)
        };

        var itemsInserted = new List<Author>();

        mockUnitOfWork
            .Setup(x => x.Authors)
            .Returns(mockRepository.Object);

        mockRepository
            .Setup(i => i.AddAsync(It.IsAny<Author>()))
            .Callback((Author author) => itemsInserted.Add(author));
        
         mockUnitOfWork
            .Setup(i => i.CompleteAsync())
            .Callback(() => {});

        var services = new AuthorService(mockUnitOfWork.Object, authorValidator);

        await services.AddAsync(author1);

        Assert.Same(author1, itemsInserted.First());
    }

    [Fact]
    public async Task DeleteTest()
    {
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockRepository = new Mock<IRepository<Author>>();
        var authorValidator = new AuthorValidator();

        var author1 = new Author
        {
            AuthorId = Guid.NewGuid(),
            FirstName = "FirstNameOfAuthor1",
            LastName = "LastNameOfAuthor1",
            Country = "USA",
            DateOfBirth = new DateTime(1966, 11, 27)
        };

        var authors = new List<Author>{author1};

        mockUnitOfWork
            .Setup(x => x.Authors)
            .Returns(mockRepository.Object);

        mockRepository
            .Setup(i => i.Delete(It.IsAny<Author>()))
            .Callback((Author author) => authors.Remove(author));
        
         mockUnitOfWork
            .Setup(i => i.CompleteAsync())
            .Callback(() => {});

        var services = new AuthorService(mockUnitOfWork.Object, authorValidator);

        await services.Delete(author1);

        Assert.Empty(authors);

    }

    [Fact]
    public async Task UpdateTest()
    {
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockRepository = new Mock<IRepository<Author>>();
        var authorValidator = new AuthorValidator();

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
        
        var authors = new List<Author>{author1, author2};

        mockUnitOfWork
            .Setup(x => x.Authors)
            .Returns(mockRepository.Object);

        mockRepository
            .Setup(i => i.Update(It.IsAny<Author>()))
            .Callback((Author author) 
                => authors[authors.FindIndex(x => x.AuthorId == author.AuthorId)] = author);
        
        mockUnitOfWork
            .Setup(i => i.CompleteAsync())
            .Callback(() => {});

        var services = new AuthorService(mockUnitOfWork.Object, authorValidator);

        var author3 = new Author
        {
            AuthorId = author1.AuthorId,
            FirstName = "FirstNameOfAuthor2",
            LastName = "LastNameOfAuthor2",
            Country = "UK",
            DateOfBirth = new DateTime(1970, 3, 15)
        };

        await services.Update(author3);

        Assert.Equal(authors[0].FirstName, authors[1].FirstName);


    }




}
