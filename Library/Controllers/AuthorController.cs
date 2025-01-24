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
        private readonly IAuthorServices _authorServices;

        public AuthorController(IAuthorServices authorServices)
        {
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
            var listOfAuthor =  this._authorServices.GetPaginatedList(pageIndex, pageSize);
            var paginatedViewModelList = new PaginatedList<AuthorViewModel>(
                Mapper.Map<List<Author>, List<AuthorViewModel>>(listOfAuthor.Items),
                listOfAuthor.PageIndex,
                listOfAuthor.TotalPages);
            
            return Ok(paginatedViewModelList);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid authorId)
        {
            var author = await this._authorServices.GetByIdAsync(authorId);
            await this._authorServices.Delete(author);
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
            var author = Mapper.Map<Author, AuthorViewModel>(objAuthor);
            return  Ok(author);
        }

        [AllowAnonymous]
        [HttpGet("FindByName")]
        public async Task<ActionResult<List<AuthorViewModel>>> GetByName(string name)
        {
            var authors = (await this._authorServices.GetAuthorByName(name)).ToList();

            var authorViewModels = Mapper.Map<List<Author>, List<AuthorViewModel>>(authors);
            return Ok(authorViewModels);
        
        }

    }

}