using FluentValidation;
using LMS.Domain.DTOs.Book;
using LMS.Domain.Ultilities;

namespace LMS.Domain.Validation
{
    public class BookValidator : AbstractValidator<BookDTO>
    {
        public BookValidator()
        {
            RuleFor(x => x.Title)
                .MaximumLength(50).WithMessage(SD.ValidationMessage.BookMessage.TitleLength);

            RuleFor(x => x.Author)
                .Length(3, 50).WithMessage(SD.ValidationMessage.BookMessage.AuthorLength);

            RuleFor(x => x.ISBN)
                .NotEmpty().WithMessage(SD.ValidationMessage.Required);

            RuleFor(x => x.Category)
                .MaximumLength(50).WithMessage(SD.ValidationMessage.BookMessage.CategoryLength);

            RuleFor(x => x.Pages)
                .GreaterThan(0).WithMessage(SD.ValidationMessage.NegativeNumber);

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage(SD.ValidationMessage.NegativeNumber);
        }
    }
}
