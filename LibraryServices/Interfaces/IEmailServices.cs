using LibraryRepository.Models;

namespace LibraryServices.Interfaces
{
    public interface IEmailServices
    {
        Task SendEmailAsync(string email, string subject, string message, CancellationToken cancellationToken = default);
        Task<List<RentHistory>> OverdueRent(int rentalPeriod, CancellationToken cancellationToken = default);
        Task<string> Message(RentHistory rentHistory);
        Task<string> UserEmail(RentHistory rentHistory);
    }
}