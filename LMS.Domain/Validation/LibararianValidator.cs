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
                .Matches(@"^[a-zA-Z ]+$").WithMessage(SD.ValidationMessage.UserMessage.NameRegex);

            RuleFor(l => l.LastName)
                .Matches(@"^[a-zA-Z ]+$").WithMessage(SD.ValidationMessage.UserMessage.NameRegex);

            RuleFor(l => l.Email)
                .EmailAddress().WithMessage(SD.ValidationMessage.UserMessage.EmailRegex);

            RuleFor(l => l.PhoneNumber)
                .Matches(@"^0\d{9}$").WithMessage(SD.ValidationMessage.UserMessage.PhoneNumberRegex);

            RuleFor(l => l.IdentityId)
                .Length(12).WithMessage(SD.ValidationMessage.UserMessage.IdentityId);

            RuleFor(m => m.Password)
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{12,15}$").WithMessage(SD.ValidationMessage.UserMessage.PasswordRegex);

        }
    }
}

