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
                .NotEmpty().WithMessage(SD.ValidationMessage.BookMessage.TitleLength)
                .MaximumLength(50).WithMessage(SD.ValidationMessage.BookMessage.TitleLength);

            RuleFor(x => x.Author)
                .NotEmpty().WithMessage(SD.ValidationMessage.BookMessage.AuthorLength)
                .Length(3, 50).WithMessage(SD.ValidationMessage.BookMessage.AuthorLength);

            RuleFor(x => x.ISBN)
                .NotEmpty().WithMessage(SD.ValidationMessage.Required);

            RuleFor(x => x.Category)
                .NotEmpty().WithMessage(SD.ValidationMessage.BookMessage.CategoryLength)
                .MaximumLength(50).WithMessage(SD.ValidationMessage.BookMessage.CategoryLength);

            RuleFor(x => x.Pages)
                .NotEmpty().WithMessage(SD.ValidationMessage.Required)
                .GreaterThan(0).WithMessage(SD.ValidationMessage.NegativeNumber);

            RuleFor(x => x.Quantity)
                .NotEmpty().WithMessage(SD.ValidationMessage.Required)
                .GreaterThan(0).WithMessage(SD.ValidationMessage.NegativeNumber);
        }
    }
}
