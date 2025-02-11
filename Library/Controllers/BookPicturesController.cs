using Microsoft.AspNetCore.Mvc;
using LibraryRepository.Models;
using LibraryServices.Interfaces;
using Library.ViewModels;
using Library;

using Microsoft.AspNetCore.Authorization;
using AutoMapper;

namespace Library.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class BookPicturesController : ControllerBase
    {
        private readonly IBookPicturesServices pictureServices;

        private readonly IWebHostEnvironment webHostEnv;
        private readonly IMapper mapper;

        public BookPicturesController(IBookPicturesServices _pictureServices,
            IWebHostEnvironment _webHostEnv, IMapper _mapper)
        {
            pictureServices = _pictureServices;
            webHostEnv = _webHostEnv;
            mapper = _mapper;
        }


        [Authorize(Policy= "AdminOnly")]
        [HttpPost]
        public async Task<IActionResult> AddPicture(BookPictureViewModel pictureView, CancellationToken token)
        {
            pictureView.Id = Guid.NewGuid();
            var picture = mapper.Map<BookPictures>(pictureView);

            await this.pictureServices
                .AddPicture(picture, webHostEnv.ContentRootPath, Constants.ImagesDirectory, token);
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> GetPicture(Guid pictureId)
        {
            var serverRootPath = webHostEnv.ContentRootPath;
            var picture = await this.pictureServices.GetPictureAsync(pictureId, serverRootPath);
            return File(picture.PictureBytes, "image/" + picture.FileExtension);
        }

        [AllowAnonymous]
        [HttpGet("GetBookPictures")]
        public async Task<ActionResult<List<BookPictureViewModel>>> GetBookPictures(Guid bookId, CancellationToken token)
        {

            var pictures = await this.pictureServices.GetBookPictures(bookId, token);
            var picturesViewModels = mapper.Map<List<BookPictureViewModel>>(pictures);
            return Ok(picturesViewModels); 
        }

        [Authorize(Policy= "AdminOnly")]
        [HttpDelete]
        public async Task<IActionResult> DeletePicture(Guid pictureId, CancellationToken token)
        {
            await this.pictureServices.Delete(pictureId, webHostEnv.ContentRootPath, token);
            return Ok();
        }

    }
}