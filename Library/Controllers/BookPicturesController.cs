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


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddPicture(BookPictureViewModel pictureView, CancellationToken token)
        {
            var serverRootPath = webHostEnv.ContentRootPath;
            pictureView.Id = Guid.NewGuid();
            var picture = mapper.Map<BookPictures>(pictureView);
            if (pictureView.Picture.Length > 0)
            {
                using (var stream = new MemoryStream())
                {
                    pictureView.Picture.CopyTo(stream);
                    picture.PictureBytes = stream.ToArray();
                }
            }

            await this.pictureServices.AddPicture(picture, serverRootPath, Constants.ImagesDirectory, token);
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
            var serverRootPath = webHostEnv.ContentRootPath;
            var pictures = await this.pictureServices.GetBookPictures(bookId, token);
            if(pictures != null)
            {
                var picturesViewModels = mapper.Map<List<BookPictureViewModel>>(pictures);
                return Ok(picturesViewModels);
            }
            return NotFound(); 
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> DeletePicture(Guid pictureId, CancellationToken token)
        {
            var serverRootPath = webHostEnv.ContentRootPath;
            var picture = await this.pictureServices.GetPictureAsync(pictureId, serverRootPath);
            await this.pictureServices.Delete(picture, serverRootPath, token);
            return Ok();
        }

    }
}