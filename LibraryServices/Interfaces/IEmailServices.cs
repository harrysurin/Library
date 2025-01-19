using LibraryRepository.Models;

namespace LibraryServices.Interfaces
{
    public interface IEmailServices
    {
        Task SendEmailAsync(string email, string subject, string message);
        Task<List<RentHistory>> OverdueRent(int rentalPeriod);
        Task<string> Message(RentHistory rentHistory);
        Task<string> UserEmail(RentHistory rentHistory);
    }
}