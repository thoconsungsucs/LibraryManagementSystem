using LMS.DataAccess.Data;
using LMS.Domain.IRepository;
using LMS.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LMS.DataAccess.Repository
{
    public class LoanRepository : ILoanRepository
    {
        private readonly ApplicationDbContext _context;
        public LoanRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IQueryable<Loan> GetAllLoans()
        {
            return _context.Loans;
        }

        public void AddLoan(Loan loan)
        {
            _context.Loans.Add(loan);
        }

        public void DeleteLoan(Loan loan)
        {
            _context.Loans.Remove(loan);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void UpdateLoan(Loan loan)
        {
            _context.Loans.Update(loan);
        }

        public async Task<Loan> GetLoan(int id)
        {
            return await _context.Loans.FirstOrDefaultAsync(l => l.Id == id);
        }
    }
}
