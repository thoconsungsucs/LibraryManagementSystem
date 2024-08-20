using FluentValidation;
using LMS.Domain.DTOs.Member;
using LMS.Domain.Ultilities;

namespace LMS.Domain.Validation
{
    public class MemberValidator : AbstractValidator<MemberRegisterDTO>
    {
        public MemberValidator()
        {
            RuleFor(m => m.FirstName).NotEmpty().WithMessage(SD.ValidationMessage.Required);
            RuleFor(m => m.LastName).NotEmpty().WithMessage(SD.ValidationMessage.Required);
            RuleFor(m => m.FirstName).Matches(@"^[a-zA-Z ]+$").WithMessage(SD.ValidationMessage.NameRegex);
            RuleFor(m => m.LastName).Matches(@"^[a-zA-Z\s]+$").WithMessage(SD.ValidationMessage.NameRegex);
            RuleFor(m => m.Email).EmailAddress().WithMessage(SD.ValidationMessage.EmailRegex);
            RuleFor(m => m.PhoneNumber).Matches(@"^\d{10}$").WithMessage(SD.ValidationMessage.PhoneNumberRegex);
            RuleFor(m => m.Password).Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{12,15}$").WithMessage(SD.ValidationMessage.PasswordRegex);
        }
    }
}
