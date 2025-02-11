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

        [Authorize(Policy = "Authorize")]
        [HttpPost]
        public async Task<IActionResult> NewRentHistory(Guid bookId, CancellationToken token)
        {
            string username = User.Identity.Name;
            await this.rentServices.BookRent(username, bookId, token);
            return Ok();
        }

        [Authorize(Policy = "Authorize")]
        [HttpPost("ReturnBook")]
        public async Task<IActionResult> ReturnBook(Guid rentId, CancellationToken token)
        {
            await this.rentServices.ReturnBook(User.Identity.Name, rentId, token);
            return Ok();
        }

        [Authorize(Policy = "Authorize")]
        [HttpGet]
        public async Task<ActionResult<PaginatedList<RentHistoryViewModel>>> GetUserRentHistory(int pageIndex, int pageSize)
        {
            var listOfRent = await this.rentServices.GetPaginatedList(pageIndex, pageSize, User.Identity.Name);
            var paginatedViewModelList = new PaginatedList<RentHistoryViewModel>(
                mapper.Map<List<RentHistoryViewModel>>(listOfRent.Items),
                listOfRent.PageIndex,
                listOfRent.TotalPages);
            return Ok(paginatedViewModelList);
            
        }

        [Authorize(Policy= "AdminOnly")]
        [HttpGet("GetUserRentHistoryByAdmin")]
        public async Task<ActionResult<PaginatedList<RentHistoryViewModel>>> GetUserRentHistoryByAdmin(int pageIndex, int pageSize, Guid userId)
        {
            var listOfRent =  this.rentServices.GetPaginatedList(pageIndex, pageSize, userId);
            var paginatedViewModelList = new PaginatedList<RentHistoryViewModel>(
                mapper.Map<List<RentHistoryViewModel>>(listOfRent.Items),
                listOfRent.PageIndex,
                listOfRent.TotalPages);
            return Ok(paginatedViewModelList);
        }


    }
}