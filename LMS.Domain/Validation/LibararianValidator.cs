using FluentValidation;
using LMS.Domain.Models;
using LMS.Domain.Ultilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.Validation
{
    public class LibrarianValidator : AbstractValidator<Librarian>
    {
        public LibrarianValidator()
        {
            RuleFor(l => l.FirstName).NotEmpty().WithMessage(SD.ValidationMessage.Required);
            RuleFor(l => l.LastName).NotEmpty().WithMessage(SD.ValidationMessage.Required);
            RuleFor(l => l.FirstName).Matches(@"[a-zA-Z\s]").WithMessage(SD.ValidationMessage.NameRegex);
            RuleFor(l => l.LastName).Matches(@"[a-zA-Z\s]").WithMessage(SD.ValidationMessage.NameRegex);
            RuleFor(l => l.Email).EmailAddress().WithMessage(SD.ValidationMessage.EmailRegex);
            RuleFor(l => l.PhoneNumber).Matches(@"[0-9]").WithMessage(SD.ValidationMessage.PhoneNumberRegex);
            RuleFor(l => l.PhoneNumber).Length(10).WithMessage(SD.ValidationMessage.PhoneNumberRegex);
            RuleFor(l => l.IdentityId).Length(12).WithMessage(SD.ValidationMessage.IdentityId);

        }
    }
}

}
