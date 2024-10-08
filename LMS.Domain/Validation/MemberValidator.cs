﻿using FluentValidation;
using LMS.Domain.DTOs.Member;
using LMS.Domain.Ultilities;

namespace LMS.Domain.Validation
{
    public class MemberValidator : AbstractValidator<MemberRegisterDTO>
    {
        public MemberValidator()
        {
            RuleFor(m => m.FirstName)
                .NotEmpty().WithMessage(SD.ValidationMessage.Required);
            RuleFor(m => m.LastName)
                .NotEmpty().WithMessage(SD.ValidationMessage.Required);
            RuleFor(m => m.FirstName)
                .Matches(@"^[a-zA-Z ]+$").WithMessage(SD.ValidationMessage.UserMessage.NameRegex);
            RuleFor(m => m.LastName)
                .Matches(@"^[a-zA-Z\s]+$").WithMessage(SD.ValidationMessage.UserMessage.NameRegex);
            RuleFor(m => m.Email)
                .EmailAddress().WithMessage(SD.ValidationMessage.UserMessage.EmailRegex);
            RuleFor(m => m.PhoneNumber)
                .Matches(@"^0\d{9}$")
                .WithMessage(SD.ValidationMessage.UserMessage.PhoneNumberRegex);
            RuleFor(m => m.Password).Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{12,15}$").WithMessage(SD.ValidationMessage.UserMessage.PasswordRegex);
        }
    }
}
