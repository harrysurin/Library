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

        private readonly IRentHistoryServices rentHistory;
        private readonly IBookServices bookServices;
        private readonly IAuthorServices authorServices;



        public BookController(ILogger<AuthorController> logger, 
                IBookServices _bookServices, IRentHistoryServices _rentHistory, IAuthorServices _authorServices)
        {
            _logger = logger;
            bookServices = _bookServices;
            rentHistory = _rentHistory;
            authorServices = _authorServices;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateBook(BookViewModel book)
        {
            var objBook = Mapper.Map<BookViewModel, Book>(book);
            await this.bookServices.AddAsync(objBook);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid bookId)
        {
            var book = await this.bookServices.GetByIdAsync(bookId);
            if(book != null) await this.bookServices.Delete(book);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Update(BookViewModel book)
        {
            var objBook = Mapper.Map<BookViewModel, Book>(book);
            await this.bookServices.Update(objBook);
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult<PaginatedList<BookViewModel>> GetAll(int pageIndex, int pageSize)
        {
            var listOfBook =  this.bookServices.PaginatedList(pageIndex, pageSize);
            var paginatedViewModelList = new PaginatedList<BookViewModel>(
                Mapper.Map<List<Book>, List<BookViewModel>>(listOfBook.Items),
                listOfBook.PageIndex,
                listOfBook.TotalPages);
            
            return Ok();
        }

        [AllowAnonymous]
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

        [AllowAnonymous]
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

        [AllowAnonymous]
        [HttpGet("GetByAuthor")]
        public async Task<ActionResult<PaginatedList<BookViewModel>>> GetByAuthor(int pageIndex, int pageSize, string name)
        {
            Author? author = await this.authorServices.GetAuthorByName(name);
            var listOfBook =  this.bookServices.PaginatedListByAuthorId(pageIndex, pageSize, author.AuthorId);
            var paginatedViewModelList = new PaginatedList<BookViewModel>(
                Mapper.Map<List<Book>, List<BookViewModel>>(listOfBook.Items),
                listOfBook.PageIndex,
                listOfBook.TotalPages);
            
            return Ok(paginatedViewModelList);
        }

        [AllowAnonymous]
        [HttpGet("GetByGenre")]
        public ActionResult<PaginatedList<BookViewModel>> GetByGenre(int pageIndex, int pageSize, string genre)
        {
            var listOfBook =  this.bookServices.PaginatedListByGenre(pageIndex, pageSize, genre);
            var paginatedViewModelList = new PaginatedList<BookViewModel>(
                Mapper.Map<List<Book>, List<BookViewModel>>(listOfBook.Items),
                listOfBook.PageIndex,
                listOfBook.TotalPages);
            
            return Ok(paginatedViewModelList);
        }

        [AllowAnonymous]
        [HttpGet("GetByTitle")]
        public ActionResult<PaginatedList<BookViewModel>> GetByTitle(int pageIndex, int pageSize, string title)
        {
            var listOfBook =  this.bookServices.PaginatedListByName(pageIndex, pageSize, title);
            var paginatedViewModelList = new PaginatedList<BookViewModel>(
                Mapper.Map<List<Book>, List<BookViewModel>>(listOfBook.Items),
                listOfBook.PageIndex,
                listOfBook.TotalPages);
            
            return Ok(paginatedViewModelList);
        }

        [AllowAnonymous]
        [HttpGet("AccessToRent")]
        public async Task<ActionResult<Boolean>> AccessToRent(Guid bookId)
        {
            return Ok(await this.rentHistory.AccessToRent(bookId));
        }

        


    }
}