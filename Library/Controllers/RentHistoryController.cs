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
        private readonly IRentHistoryServices rentServices;

        private readonly IUserServices userServices;

        private readonly IMapper mapper;


        public RentHistoryController(IRentHistoryServices _rentServices, IUserServices _userServices, IMapper _mapper)
        {
            userServices = _userServices;
            rentServices = _rentServices;
            mapper = _mapper;
        }

        private ActionResult<PaginatedList<RentHistoryViewModel>> GetRentHistory(int pageIndex, int pageSize, Guid userId)
        {
            var listOfRent =  this.rentServices.GetPaginatedList(pageIndex, pageSize, userId);
            var paginatedViewModelList = new PaginatedList<RentHistoryViewModel>(
                mapper.Map<List<RentHistoryViewModel>>(listOfRent.Items),
                listOfRent.PageIndex,
                listOfRent.TotalPages);
            return paginatedViewModelList;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> NewRentHistory(Guid bookId, CancellationToken token)
        {
            User? user = await userServices.FindByNameAsync(User.Identity.Name);
            Guid userId = user.Id;
            await this.rentServices.BookRent(userId, bookId, token);
            return Ok();
        }

        [Authorize]
        [HttpPost("ReturnBook")]
        public async Task<IActionResult> ReturnBook(Guid rentId, CancellationToken token)
        {
            User? user = await userServices.FindByNameAsync(User.Identity.Name);
            Guid userId = user.Id;
            var rentHistory = await this.rentServices.GetByIdAsync(rentId);
            await this.rentServices.ReturnBook(rentHistory.BookId, userId, token);
            return Ok();
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<PaginatedList<RentHistoryViewModel>>> GetUserRentHistory(int pageIndex, int pageSize)
        {
            var user = await userServices.FindByNameAsync(User.Identity.Name);
            Guid userId = user.Id;
            var paginatedViewModelList = this.GetRentHistory(pageIndex, pageSize, userId);
            return Ok(paginatedViewModelList);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetUserRentHistoryByAdmin")]
        public async Task<ActionResult<PaginatedList<RentHistoryViewModel>>> GetUserRentHistoryByAdmin(int pageIndex, int pageSize, Guid userId)
        {
            var paginatedViewModelList = this.GetRentHistory(pageIndex, pageSize, userId);
            return Ok(paginatedViewModelList);
        }
    }
}