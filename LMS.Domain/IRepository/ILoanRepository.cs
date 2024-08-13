using LMS.Domain.Models;

namespace LMS.Domain.IRepository
{
    public interface ILoanRepository
    {
        IQueryable<Loan> GetAllLoans();
        Task<Loan> GetLoan(int id);
        void AddLoan(Loan loan);
        void UpdateLoan(Loan loan);
        void DeleteLoan(Loan loan);
        Task SaveAsync();
    }
}
