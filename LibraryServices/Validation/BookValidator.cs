using FluentValidation;
using LibraryRepository.Models;

namespace LibraryServices.Validation
{
    public class BookValidator : AbstractValidator<Book>
    {
        public BookValidator()
        {
            RuleFor(x => x.ISBN)
            .Matches("^(?=(?:[^0-9]*[0-9]){10}(?:(?:[^0-9]*[0-9]){3})?$)[\\d-]+$");
            RuleFor(x => x.Title)
            .NotNull();
            RuleFor(x => x.AuthorId)
            .NotNull();
            RuleFor(x => x.Genre)
            .NotNull();
        }
    }
}