using LMS.Domain.DTOs.Loan;
using LMS.Domain.Exceptions;
using LMS.Domain.Models;
using LMS.Domain.Ultilities;

namespace LMS.Domain.IService
{
    public interface ILoanService
    {
        public Task<List<LoanDTO>> GetAllLoans(LoanFilter loanFilter);
        public Task<Loan> GetLoan(int id);
        public Task<bool> CanBorrow(int id);
        public Task<Result<Loan>> AddLoan(LoanDTOForPost loanDTOForPost, bool isLibrarian = false);
        public Task<Loan> CancelLoan(int id);
        public Task<Loan> ConfirmLoan(LoanConfirmDTO loanConfirmDTO);
        public Task<Loan> UpdateLoan(LoanDTOForPut loanDTOForPut);
        /*public Task<Loan> ConfirmUpdate(int id);*/
        public Task<Loan> ReturnBook(int id);
        public Task<Loan> ConfirmReturn(int id);
        public Task<Loan> RenewLoan(int id, int days);
        public Task<Loan> ConfirmRenew(LoanConfirmDTO loanConfirmDTO);
        public Task<Loan> DeleteLoan(int id);



    }
}
