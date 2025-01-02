using FluentValidation;
using LibraryRepository.Models;

namespace LibraryServices.Validation
{
    public class AuthorValidator : AbstractValidator<Author>
    {
        public AuthorValidator()
        {
            RuleFor(x => x.LastName)
            .NotNull()
            .Length(2, 50);
            RuleFor(x => x.Country)
            .NotNull();

        }
    }
}