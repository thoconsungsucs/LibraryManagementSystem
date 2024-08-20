using FluentValidation;
using LMS.Domain.DTOs.Loan;
using LMS.Domain.Ultilities;

namespace LMS.Domain.Validation
{
    public class LoanValidator : AbstractValidator<LoanToDb>
    {
        public LoanValidator()
        {
            RuleFor(x => x.LoanDate)
                .NotEmpty().WithMessage(SD.ValidationMessage.Required)
                .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now)).WithMessage(SD.ValidationMessage.LoanMessage.LoanDateError);

            RuleFor(x => x.Duration)
                .NotEmpty().WithMessage(SD.ValidationMessage.Required)
                .Must(d => d > 0 && d <= 30).WithMessage(SD.ValidationMessage.LoanMessage.DurationLength);
        }
    }
}
