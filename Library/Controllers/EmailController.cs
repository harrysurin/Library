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

    public class EmailController : ControllerBase
    {
        private readonly IEmailServices emailServices;

        public EmailController(IEmailServices _emailServices)
        {
            emailServices = _emailServices;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> SendEmail(int rentalPeriod)
        {
            var ListOfRent = await this.emailServices.OverdueRent(rentalPeriod);
            for(int i = 0; i < ListOfRent.Count; i++)
            {
                await this.emailServices.SendEmailAsync(
                    await emailServices.UserEmail(ListOfRent[i]), "book rent",
                    await emailServices.Message(ListOfRent[i]));
            }

            return Ok();
        }
    }
}