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
            await this.pictureServices.AddPicture(picture, serverRootPath, Constants.ImagesDirectory);
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> GetPicture(Guid pictureId)
        {
            var serverRootPath = webHostEnv.ContentRootPath;
            var picture = await this.pictureServices.GetPictureAsync(pictureId);
            if(picture != null)
            {
                return PhysicalFile(Path.Combine(serverRootPath, picture.Path), "image/jpeg");
            }
            return NotFound(); 
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> DeletePicture(Guid pictureId)
        {
            var serverRootPath = webHostEnv.ContentRootPath;
            var picture = await this.pictureServices.GetPictureAsync(pictureId);
            if(picture != null)
            {
                await this.pictureServices.Delete(picture, serverRootPath);
                return Ok();
            }

            return NotFound();
        }

    }
}