using LMS.Domain.DTOs.Loan;
using LMS.Domain.Models;
using LMS.Domain.Ultilities;

namespace LMS.Domain.IService
{
    public interface ILoanService
    {
        public Task<List<LoanDTO>> GetAllLoans(LoanFilter loanFilter);
        public Task<Loan> GetLoan(int id);
        public Task<Loan> AddLoan(LoanDTOForPost loanDTOForPost);
        public Task<Loan> UpdateLoan(LoanDTOForPut loanDTOForPut);
        public Task<Loan> DeleteLoan(int id);
        public Task<bool> CanBorrow(int id);
        public Task<Loan> CancelLoan(int id);
        public Task<Loan> ConfirmLoan(int id);
        public Task<Loan> ReturnBook(int id);
        public Task<Loan> ConfirmReturn(int id);


    }
}
