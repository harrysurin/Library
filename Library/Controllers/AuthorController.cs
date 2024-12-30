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

    public class AuthorController : ControllerBase
    {
        private readonly ILogger<AuthorController> _logger;
        private readonly IAuthorServices _authorServices;

        public AuthorController(ILogger<AuthorController> logger, IAuthorServices authorServices)
        {
            _logger = logger;
            _authorServices = authorServices;
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateAuthor(AuthorViewModel author)
        {
            var objAuthor = Mapper.Map<AuthorViewModel, Author>(author);
            await this._authorServices.AddAsync(objAuthor);
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult<PaginatedList<AuthorViewModel>> GetAll(int pageIndex, int pageSize)
        {
            var listOfAuthor =  this._authorServices.PaginatedList(pageIndex, pageSize);
            var paginatedViewModelList = new PaginatedList<AuthorViewModel>(
                Mapper.Map<List<Author>, List<AuthorViewModel>>(listOfAuthor.Items),
                listOfAuthor.PageIndex,
                listOfAuthor.TotalPages);
            
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid authorId)
        {
            var author = await this._authorServices.GetByIdAsync(authorId);
            if(author != null) await this._authorServices.Delete(author);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Update(AuthorViewModel author)
        {
            Author objAuthor = Mapper.Map<AuthorViewModel, Author>(author);
            await this._authorServices.Update(objAuthor);
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("FindByID")]
        public async Task<ActionResult<AuthorViewModel>> GetById(Guid authorId)
        {
            var objAuthor = await this._authorServices.GetByIdAsync(authorId);
            if(objAuthor != null) 
            {
                var author = Mapper.Map<Author, AuthorViewModel>(objAuthor);
                return author;
            }
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("FindByName")]
        public async Task<ActionResult<AuthorViewModel>> GetByName(string name)
        {
            var objAuthor = await this._authorServices.GetAuthorByName(name);
            if(objAuthor != null) 
            {
                var author = Mapper.Map<Author, AuthorViewModel>(objAuthor);
                return author;
            }
            return Ok();
        
        }

    }

}