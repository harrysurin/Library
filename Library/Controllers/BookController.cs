using Microsoft.AspNetCore.Mvc;
using LibraryRepository.Models;
using LibraryServices.Interfaces;
using Library.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace Library.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class BookController : ControllerBase
    {
        private readonly ILogger<AuthorController> _logger;

        private readonly IBookServices bookServices;


        public BookController(ILogger<AuthorController> logger, IBookServices _bookServices)
        {
            _logger = logger;
            bookServices = _bookServices;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook(BookViewModel book)
        {
            var objBook = Mapper.Map<BookViewModel, Book>(book);
            await this.bookServices.AddAsync(objBook);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid bookId)
        {
            var book = await this.bookServices.GetByIdAsync(bookId);
            if(book != null) await this.bookServices.Delete(book);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update(BookViewModel book)
        {
            var objBook = Mapper.Map<BookViewModel, Book>(book);
            await this.bookServices.Update(objBook);
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetAll()
        {
            var listOfBook =  await this.bookServices.GetAllAsync();
            var listOfView = Mapper.Map<IEnumerable<Book>, IEnumerable<BookViewModel>>(listOfBook);
            return Ok(listOfView);
        }

        [HttpGet("GetBookByISBN")]
        public async Task<ActionResult<BookViewModel>> GetByISBN(string ISBN)
        {
            var objBook = await this.bookServices.GetBookByISBN(ISBN);
            if(objBook != null)
            {
                var book = Mapper.Map<Book, BookViewModel>(objBook);
                return book;
            }
            return Ok(); 
        }

        [HttpGet("GetBookByID")]
        public async Task<ActionResult<BookViewModel>> GetById(Guid bookId)
        {
            var objBook = await this.bookServices.GetByIdAsync(bookId);
            if(objBook != null)
            {
                var book = Mapper.Map<Book, BookViewModel>(objBook);
                return book;
            }
            return Ok(); 
        }

        [HttpGet("GetByAuthor")]
        public async Task<ActionResult<IEnumerable<Book>>> GetByAuthor(Guid authorId)
        {
            var listOfBook =  await this.bookServices.GetBooksByAuthor(authorId);
            var listOfView = Mapper.Map<IEnumerable<Book>, IEnumerable<BookViewModel>>(listOfBook);
            return Ok(listOfView);
        }


    }
}