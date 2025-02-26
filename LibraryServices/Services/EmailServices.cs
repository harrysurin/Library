using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using LibraryServices.Interfaces;
using LibraryRepository.Models;
using LibraryRepository.Interfaces;


public class EmailServices : IEmailServices
{
    private readonly string _emailFrom;
    private readonly string _password;
    private readonly IConfiguration _configuration;

    private IUserServices userServices;
    private IRentHistoryServices rentServices;

    private IBookServices bookServices;
    private readonly IUnitOfWork unitOfWork;

    public EmailServices(IUserServices _userServices, IRentHistoryServices _rentServices,
                        IUnitOfWork _unitOfWork, IBookServices _bookServices, IConfiguration configuration)
    {
        userServices = _userServices;
        rentServices = _rentServices;
        bookServices = _bookServices;
        unitOfWork = _unitOfWork;
        _configuration = configuration;
        _emailFrom = _configuration["Email:Login"];
        _password = _configuration["Email:Password"];
    }


    public async Task SendEmailAsync(string email, string subject, string message, CancellationToken cancellationToken)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Library administration", _emailFrom));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };
            
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(
                    _configuration["Email:SmtpServerAdress"],
                    int.Parse(_configuration["Email:SmtpServerPort"]));
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                await client.AuthenticateAsync(_emailFrom, _password, cancellationToken);
                await client.SendAsync(emailMessage, cancellationToken);

                await client.DisconnectAsync(true);
            }
        }

    public async Task<List<RentHistory>> OverdueRent(int rentalPeriod, CancellationToken cancellationToken)
    {
        DateTime overdue = DateTime.Today.AddDays(-rentalPeriod);
        var ListOfRent = await unitOfWork.RentHistory.ToListByPredicateAsync(x => x.DateOfReturn == null || x.DateOfRent == overdue, cancellationToken);
        return ListOfRent;
    }

    public async Task<string> Message(RentHistory rentHistory)
    {
        var user = await userServices.FindByIdAsync(rentHistory.UserId);
        var book = await bookServices.GetByIdAsync(rentHistory.BookId);
        string message = $"Dear {user.UserName}. We would like to remind you that the rental period for the book {book.Title} has expired. As of today, book is overdue. Please, return the book";
        return message;
    }

    public async Task<string> UserEmail(RentHistory rentHistory)
    {
        var user = await userServices.FindByIdAsync(rentHistory.UserId);
        return user.Email;
    }



    
}