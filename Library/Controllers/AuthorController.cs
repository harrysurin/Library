using Microsoft.AspNetCore.Mvc;
using LibraryRepository.Models;
using LibraryServices.Interfaces;
using Library.ViewModels;

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
            Author objAuthor = new Author()
            {
                AuthorId = new Guid(),
                Name = author.Name,
                DateOfBirth = author.DateOfBirth
            };
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
            Author objAuthor = new Author()
            {
                AuthorId = author.AuthorId,
                Name = author.Name,
                DateOfBirth = author.DateOfBirth
            };
            await this._authorServices.Update(objAuthor);
            return Ok();
        }

    }

}