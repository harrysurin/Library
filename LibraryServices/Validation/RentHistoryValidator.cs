using FluentValidation;
using LibraryRepository.Models;


namespace LibraryServices.Validation
{
    public class RentHistoryValidator : AbstractValidator<RentHistory>
    {
        public RentHistoryValidator()
        {
            RuleFor(x => x.BookId)
            .NotNull();
            RuleFor(x => x.UserId)
            .NotNull();
        }

    }
}