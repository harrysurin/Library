using Microsoft.AspNetCore.Mvc;
using LibraryRepository.Models;
using LibraryServices.Interfaces;
using Library.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using LibraryRepository.Implementations;

namespace Library.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class BookController : ControllerBase
    {
        private readonly IRentHistoryServices rentHistory;
        private readonly IBookServices bookServices;
        private readonly IBookPicturesServices picturesServices;
        private readonly IWebHostEnvironment webHostEnv;
        private readonly IMapper mapper;

        public BookController(IBookServices _bookServices, IRentHistoryServices _rentHistory, 
                        IBookPicturesServices _picturesServices, IWebHostEnvironment _webHostEnv, IMapper _mapper)
        {
            bookServices = _bookServices;
            rentHistory = _rentHistory;
            picturesServices = _picturesServices;
            webHostEnv = _webHostEnv;
            mapper = _mapper;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateBook(BookViewModel book, CancellationToken token)
        {
            var objBook = mapper.Map<Book>(book);
            await this.bookServices.AddAsync(objBook, token);

            var picture = mapper.Map<BookPictures>(book);

            if (book.BookPicture != null && book.BookPicture.Length > 0)
            {
                using (var stream = new MemoryStream())
                {
                    book.BookPicture.CopyTo(stream);
                    picture.PictureBytes = stream.ToArray();
                }
            }

            var serverRootPath = webHostEnv.ContentRootPath;
            await this.picturesServices.AddPicture(picture, serverRootPath, Constants.ImagesDirectory, token);

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid bookId, CancellationToken token)
        {
            var book = await this.bookServices.GetByIdAsync(bookId);
            await this.bookServices.Delete(book, token);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Update(BookViewModel book, CancellationToken token)
        {
            var objBook = mapper.Map<Book>(book);
            await this.bookServices.Update(objBook, token);
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult<PaginatedList<BookViewModel>> GetAll(int pageIndex, int pageSize)
        {
            var listOfBook =  this.bookServices.GetPaginatedList(pageIndex, pageSize);
            var paginatedViewModelList = new PaginatedList<BookViewModel>(
                mapper.Map<List<BookViewModel>>(listOfBook.Items),
                listOfBook.PageIndex,
                listOfBook.TotalPages);
            
            return Ok(paginatedViewModelList);
        }

        [AllowAnonymous]
        [HttpGet("GetBookByISBN")]
        public async Task<ActionResult<BookViewModel>> GetByISBN(string ISBN, CancellationToken token)
        {
            var objBook = await this.bookServices.GetBookByISBN(ISBN, token);
            var book = mapper.Map<BookViewModel>(objBook);
            return Ok(book);
           
        }

        [AllowAnonymous]
        [HttpGet("GetBookByID")]
        public async Task<ActionResult<BookViewModel>> GetById(Guid bookId)
        {
            var objBook = await this.bookServices.GetByIdAsync(bookId);
            var book = mapper.Map<BookViewModel>(objBook);
            return Ok(book);
        }

        [AllowAnonymous]
        [HttpGet("GetByAuthor")]
        public ActionResult<PaginatedList<BookViewModel>> GetByAuthor(int pageIndex, int pageSize, Guid authorId)
        {
            var listOfBook =  this.bookServices.GetPaginatedListByAuthorId(pageIndex, pageSize, authorId);
            var paginatedViewModelList = new PaginatedList<BookViewModel>(
                mapper.Map<List<BookViewModel>>(listOfBook.Items),
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
                mapper.Map<List<BookViewModel>>(listOfBook.Items),
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
                mapper.Map<List<BookViewModel>>(listOfBook.Items),
                listOfBook.PageIndex,
                listOfBook.TotalPages);
            
            return Ok(paginatedViewModelList);
        }

        [AllowAnonymous]
        [HttpGet("IsAvailableToRent")]
        public async Task<ActionResult<bool>> IsAvailableToRent(Guid bookId, CancellationToken token)
        {
            return Ok(await this.rentHistory.IsAvailableToRent(bookId, token));
        }

        


    }
}