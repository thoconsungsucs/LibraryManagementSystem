using LMS.Domain.DTOs.Loan;
using LMS.Domain.Models;

namespace LMS.Domain.Mappers
{
    public static class LoanMappers
    {
        public static Loan ToLoan(this LoanDTOForPost loanDTOForPost)
        {
            return new Loan
            {
                BookId = loanDTOForPost.BookId,
                MemberId = loanDTOForPost.MemberId,
                LoanDate = loanDTOForPost.LoanDate,
                ReturnDate = loanDTOForPost.LoanDate.AddDays(loanDTOForPost.Duration)
            };
        }
    }
}
