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

        public BookPicturesController(IBookPicturesServices _pictureServices,
            IWebHostEnvironment _webHostEnv)
        {
            pictureServices = _pictureServices;
            webHostEnv = _webHostEnv;
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddPicture(BookPictureViewModel pictureView)
        {
            var serverRootPath = webHostEnv.ContentRootPath;
            pictureView.Id = Guid.NewGuid();
            var picture = Mapper.Map<BookPictureViewModel, BookPictures>(pictureView);
            if (pictureView.Picture.Length > 0)
            {
                using (var stream = new MemoryStream())
                {
                    pictureView.Picture.CopyTo(stream);
                    picture.PictureBytes = stream.ToArray();
                }
            }

            await this.pictureServices.AddPicture(picture, serverRootPath, Constants.ImagesDirectory);
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> GetPicture(Guid pictureId)
        {
            var serverRootPath = webHostEnv.ContentRootPath;
            var picture = await this.pictureServices.GetPictureAsync(pictureId, serverRootPath);
            if(picture != null && picture.PictureBytes != null)
            {
                return File(picture.PictureBytes, "image/" + picture.FileExtension);
            }
            return NotFound(); 
        }

        [AllowAnonymous]
        [HttpGet("GetBookPictures")]
        public async Task<ActionResult<List<BookPictureViewModel>>> GetBookPictures(Guid bookId)
        {
            var serverRootPath = webHostEnv.ContentRootPath;
            var pictures = await this.pictureServices.GetBookPictures(bookId);
            if(pictures != null)
            {
                var picturesViewModels = Mapper.Map<List<BookPictures>, List<BookPictureViewModel>>(pictures);
                return Ok(picturesViewModels);
            }
            return NotFound(); 
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> DeletePicture(Guid pictureId)
        {
            var serverRootPath = webHostEnv.ContentRootPath;
            var picture = await this.pictureServices.GetPictureAsync(pictureId, serverRootPath);
            if(picture != null)
            {
                await this.pictureServices.Delete(picture, serverRootPath);
                return Ok();
            }
            return NotFound();
        }

    }
}