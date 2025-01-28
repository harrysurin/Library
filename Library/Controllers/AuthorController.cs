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

        private readonly IMapper _mapper;

        public AuthorController(IAuthorServices authorServices, IMapper mapper)
        {
            _authorServices = authorServices;
            _mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateAuthor(AuthorViewModel author, CancellationToken token)
        {
            var objAuthor = _mapper.Map<Author>(author);
            await this._authorServices.AddAsync(objAuthor, token);
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult<PaginatedList<AuthorViewModel>> GetAll(int pageIndex, int pageSize)
        {
            var listOfAuthor =  this._authorServices.GetPaginatedList(pageIndex, pageSize);
            var paginatedViewModelList = new PaginatedList<AuthorViewModel>(
                _mapper.Map<List<AuthorViewModel>>(listOfAuthor.Items),
                listOfAuthor.PageIndex,
                listOfAuthor.TotalPages);
            
            return Ok(paginatedViewModelList);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid authorId, CancellationToken token)
        {
            var author = await this._authorServices.GetByIdAsync(authorId);
            await this._authorServices.Delete(author, token);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Update(AuthorViewModel author, CancellationToken token)
        {
            Author objAuthor = _mapper.Map<Author>(author);
            await this._authorServices.Update(objAuthor, token);
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("FindByID")]
        public async Task<ActionResult<AuthorViewModel>> GetById(Guid authorId)
        {
            var objAuthor = await this._authorServices.GetByIdAsync(authorId);
            var author = _mapper.Map<AuthorViewModel>(objAuthor);
            return  Ok(author);
        }

        [AllowAnonymous]
        [HttpGet("FindByName")]
        public async Task<ActionResult<List<AuthorViewModel>>> GetByName(string name, CancellationToken token)
        {
            var authors = (await this._authorServices.GetAuthorByName(name, token)).ToList();

            var authorViewModels = _mapper.Map<List<AuthorViewModel>>(authors);
            return Ok(authorViewModels);
        
        }

    }

}