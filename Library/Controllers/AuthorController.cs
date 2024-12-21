using Microsoft.AspNetCore.Mvc;
using LibraryRepository.Models;
using LibraryServices.Interfaces;
using Library.ViewModels;
using AutoMapper;

namespace Library.AddControllers
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

        [HttpPost]
        public async Task<IActionResult> CreateAuthor(AuthorViewModel author)
        {
            var objAuthor = Mapper.Map<AuthorViewModel, Author>(author);
            await this._authorServices.AddAsync(objAuthor);
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> GetAll()
        {
            return Ok(await this._authorServices.GetAllAsync());
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid authorId)
        {
            var author = await this._authorServices.GetByIdAsync(authorId);
            if(author != null) await this._authorServices.Delete(author);
            else
            throw new ArgumentException("The authoe isn't exist");
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update(AuthorViewModel author)
        {
            Author objAuthor = Mapper.Map<AuthorViewModel, Author>(author);
            await this._authorServices.Update(objAuthor);
            return Ok();
        }

        [HttpGet("FindByID")]
        public async Task<ActionResult<AuthorViewModel>> GetById(Guid authorId)
        {
            var objAuthor = await this._authorServices.GetByIdAsync(authorId);
            if(objAuthor != null) 
            {
                var author = Mapper.Map<Author, AuthorViewModel>(objAuthor);
                return author;
            }
            else
            throw new ArgumentException("The authoe isn't exist");
        }

        [HttpGet("FindByName")]
        public async Task<ActionResult<AuthorViewModel>> GetByName(string name)
        {
            var objAuthor = await this._authorServices.GetAuthorByName(name);
            if(objAuthor != null) 
            {
                var author = Mapper.Map<Author, AuthorViewModel>(objAuthor);
                return author;
            }
            else
            throw new ArgumentException("The authoe isn't exist");
        
        }

    }

}