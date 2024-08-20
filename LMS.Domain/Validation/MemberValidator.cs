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
    public class MemberValidator : AbstractValidator<Member>
    {
        public MemberValidator() { 
            RuleFor(m => m.FirstName).NotEmpty().WithMessage(SD.ValidationMessage.Required);
            RuleFor(m => m.LastName).NotEmpty().WithMessage(SD.ValidationMessage.Required);
            RuleFor(m => m.FirstName).Matches(@"[a-zA-Z\s]").WithMessage(SD.ValidationMessage.NameRegex);
            RuleFor(m => m.LastName).Matches(@"[a-zA-Z\s]").WithMessage(SD.ValidationMessage.NameRegex);
            RuleFor(m => m.Email).EmailAddress().WithMessage(SD.ValidationMessage.EmailRegex);
            RuleFor(m => m.PhoneNumber).Matches(@"[0-9]").WithMessage(SD.ValidationMessage.PhoneNumberRegex);
            RuleFor(m => m.PhoneNumber).Length(10).WithMessage(SD.ValidationMessage.PhoneNumberRegex);

        }
    }
}
