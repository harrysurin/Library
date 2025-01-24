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
        private readonly IRentHistoryServices rentHistory;
        private readonly IBookServices bookServices;

        public BookController(IBookServices _bookServices, IRentHistoryServices _rentHistory)
        {
            bookServices = _bookServices;
            rentHistory = _rentHistory;
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
            await this.bookServices.Delete(book);
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
            var listOfBook =  this.bookServices.GetPaginatedList(pageIndex, pageSize);
            var paginatedViewModelList = new PaginatedList<BookViewModel>(
                Mapper.Map<List<Book>, List<BookViewModel>>(listOfBook.Items),
                listOfBook.PageIndex,
                listOfBook.TotalPages);
            
            return Ok(paginatedViewModelList);
        }

        [AllowAnonymous]
        [HttpGet("GetBookByISBN")]
        public async Task<ActionResult<BookViewModel>> GetByISBN(string ISBN)
        {
            var objBook = await this.bookServices.GetBookByISBN(ISBN);
            var book = Mapper.Map<Book, BookViewModel>(objBook);
            return Ok(book);
           
        }

        [AllowAnonymous]
        [HttpGet("GetBookByID")]
        public async Task<ActionResult<BookViewModel>> GetById(Guid bookId)
        {
            var objBook = await this.bookServices.GetByIdAsync(bookId);
            var book = Mapper.Map<Book, BookViewModel>(objBook);
            return Ok(book);
        }

        [AllowAnonymous]
        [HttpGet("GetByAuthor")]
        public ActionResult<PaginatedList<BookViewModel>> GetByAuthor(int pageIndex, int pageSize, Guid authorId)
        {
            var listOfBook =  this.bookServices.GetPaginatedListByAuthorId(pageIndex, pageSize, authorId);
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
            var listOfBook =  this.bookServices.GetPaginatedListByGenre(pageIndex, pageSize, genre);
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
            var listOfBook =  this.bookServices.GetPaginatedListByName(pageIndex, pageSize, title);
            var paginatedViewModelList = new PaginatedList<BookViewModel>(
                Mapper.Map<List<Book>, List<BookViewModel>>(listOfBook.Items),
                listOfBook.PageIndex,
                listOfBook.TotalPages);
            
            return Ok(paginatedViewModelList);
        }

        [AllowAnonymous]
        [HttpGet("IsAvailableToRent")]
        public async Task<ActionResult<bool>> IsAvailableToRent(Guid bookId)
        {
            return Ok(await this.rentHistory.IsAvailableToRent(bookId));
        }

        


    }
}