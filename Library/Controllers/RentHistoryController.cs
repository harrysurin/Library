using Microsoft.AspNetCore.Mvc;
using LibraryRepository.Models;
using LibraryServices.Interfaces;
using Microsoft.AspNetCore.Identity;
using Library.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace Library.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class RentHistoryController : ControllerBase
    {
        private readonly ILogger<AuthorController> _logger;

        private readonly IRentHistoryServices rentServices;

        private readonly IUserServices userServices;


        public RentHistoryController(ILogger<AuthorController> logger, 
                IRentHistoryServices _rentServices, IUserServices _userServices)
        {
            _logger = logger;
            userServices = _userServices;
            rentServices = _rentServices;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> NewRentHistory(Guid bookId)
        {
            User? user = await userServices.FindByNameAsync(User.Identity.Name);
            Guid userId = user.Id;
            await this.rentServices.BookDistribution(userId, bookId);
            return Ok();
        }

        [Authorize]
        [HttpPost("ReturnBook")]
        public async Task<IActionResult> ReturnBook(Guid rentId)
        {
            User? user = await userServices.FindByNameAsync(User.Identity.Name);
            Guid userId = user.Id;
            var rentHistory = await this.rentServices.GetByIdAsync(rentId);
            await this.rentServices.ReturnBook(rentHistory.BookId, userId);
            return Ok();
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<PaginatedList<RentHistoryViewModel>>> UserRentHistory(int pageIndex, int pageSize)
        {
            var user = await userServices.FindByNameAsync(User.Identity.Name);
            Guid userId = user.Id;
            var listOfRent =  this.rentServices.PaginatedList(pageIndex, pageSize, userId);
            var paginatedViewModelList = new PaginatedList<RentHistoryViewModel>(
                Mapper.Map<List<RentHistory>, List<RentHistoryViewModel>>(listOfRent.Items),
                listOfRent.PageIndex,
                listOfRent.TotalPages);
            return Ok(paginatedViewModelList);
        }
    }
}