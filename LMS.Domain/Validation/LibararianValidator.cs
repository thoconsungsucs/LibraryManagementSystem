using FluentValidation;
using LMS.Domain.DTOs.Librarian;
using LMS.Domain.Ultilities;

namespace LMS.Domain.Validation
{
    public class LibrarianValidator : AbstractValidator<LibrarianRegisterDTO>
    {
        public LibrarianValidator()
        {
            RuleFor(l => l.FirstName)
                .NotEmpty().WithMessage(SD.ValidationMessage.Required)
                .Matches(@"^[a-zA-Z ]+$").WithMessage(SD.ValidationMessage.UserMessage.NameRegex);

            RuleFor(l => l.LastName)
                .NotEmpty().WithMessage(SD.ValidationMessage.Required)
                .Matches(@"^[a-zA-Z ]+$").WithMessage(SD.ValidationMessage.UserMessage.NameRegex);

            RuleFor(l => l.Email)
                .EmailAddress().WithMessage(SD.ValidationMessage.UserMessage.EmailRegex);

            RuleFor(l => l.PhoneNumber)
                .Matches(@"^\d{10}$").WithMessage(SD.ValidationMessage.UserMessage.PhoneNumberRegex);

            RuleFor(l => l.IdentityId)
                .Length(12).WithMessage(SD.ValidationMessage.UserMessage.IdentityId);

            RuleFor(m => m.Password)
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{12,15}$").WithMessage(SD.ValidationMessage.UserMessage.PasswordRegex);

        }
    }
}

