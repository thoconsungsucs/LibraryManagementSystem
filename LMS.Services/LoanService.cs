using LMS.Domain.DTOs.Loan;
using LMS.Domain.IRepository;
using LMS.Domain.IService;
using LMS.Domain.Mappers;
using LMS.Domain.Models;
using LMS.Domain.Ultilities;
using Microsoft.EntityFrameworkCore;

namespace LMS.Services
{
    public class LoanService : ILoanService
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IMemberRepository _memberRepository;

        public LoanService(ILoanRepository loanRepository, IBookRepository bookRepository, IMemberRepository memberRepository)
        {
            _loanRepository = loanRepository;
            _bookRepository = bookRepository;
            _memberRepository = memberRepository;
        }

        public async Task<List<LoanDTO>> GetAllLoans(LoanFilter loanFilter)
        {
            var loans = _loanRepository.GetAllLoans();
            Member member = new Member();
            IQueryable<Book> books;


            if (loanFilter.LoanDate != DateOnly.MinValue)
            {
                loans = loans.Where(l => l.LoanDate == loanFilter.LoanDate);
            }

            if (loanFilter.ReturnDate != DateOnly.MinValue)
            {
                loans = loans.Where(l => l.ReturnDate == loanFilter.ReturnDate);
            }

            if (!String.IsNullOrEmpty(loanFilter.MemberId))
            {
                loans = loans.Where(l => l.MemberId == loanFilter.MemberId);
                member = await _memberRepository.GetMember(loanFilter.MemberId);
            }

            loans = loans.Where(l => loanFilter.IsReturned ? l.ActualReturnDate.HasValue : !l.ActualReturnDate.HasValue);

            if (!String.IsNullOrEmpty(loanFilter.BookTitle))
            {
                books = _bookRepository.GetAllBooks().Where(b => b.Title.Contains(loanFilter.BookTitle));
            }
            else
            {
                books = _bookRepository.GetAllBooks();
            }
            List<LoanDTO> loanDTOs = await loans.Join(books, l => l.BookId, b => b.Id, (l, b) => new LoanDTO
            {
                Id = l.Id,
                MemberId = l.MemberId,
                MemberName = $"{member.FirstName} {member.LastName}",
                BookId = l.BookId,
                BookTitle = b.Title,
                LoanDate = l.LoanDate,
                ReturnDate = l.ReturnDate,
                ActualReturnDate = l.ActualReturnDate
            }).ToListAsync();
            return loanDTOs;
        }

        public async Task<Loan> GetLoan(int id)
        {
            return await _loanRepository.GetLoan(id);
        }

        public async Task<bool> CanBorrow(string id)
        {
            var outDateLoanNumber = await _loanRepository.GetAllLoans()
                .Where(l => l.MemberId == id && l.ReturnDate > DateOnly.FromDateTime(DateTime.Now))
                .CountAsync();
            return outDateLoanNumber < 1;
        }

        public async Task<Loan> AddLoan(LoanDTOForPost loanDTOForPost)
        {
            try
            {
                CheckLoanForPost(loanDTOForPost);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            var loan = loanDTOForPost.ToLoan();
            _loanRepository.AddLoan(loan);
            await _loanRepository.SaveAsync();
            return loan;
        }

        private void CheckLoanForPost(LoanDTOForPost loanDTOForPost)
        {
            var bookExist = _bookRepository.GetAllBooks().Any(b => b.Id == loanDTOForPost.BookId);
            if (!bookExist)
            {
                throw new Exception("Book not found");
            }

            if (loanDTOForPost.LoanDate < DateOnly.FromDateTime(DateTime.Now))
            {
                throw new Exception("Loan date must be greater than or equal to today");
            }

            if (loanDTOForPost.LoanDate.AddDays(loanDTOForPost.LoanDuration) < loanDTOForPost.LoanDate)
            {
                throw new Exception("Return date must be greater than loan date");
            }
        }

        public async Task<Loan> DeleteLoan(int id)
        {
            var loan = await _loanRepository.GetLoan(id);
            if (loan == null)
            {
                throw new Exception("Loan not found");
            }
            _loanRepository.DeleteLoan(loan);
            await _loanRepository.SaveAsync();
            return loan;
        }

        public async Task<Loan> UpdateLoan(LoanDTOForPut loanDTOForPut)
        {
            try
            {
                CheckLoanForPut(loanDTOForPut);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            var loan = await _loanRepository.GetLoan(loanDTOForPut.Id);
            loan.LoanDate = loanDTOForPut.LoanDate;
            loan.ReturnDate = loanDTOForPut.LoanDate.AddDays(loanDTOForPut.LoanDuration);
            _loanRepository.UpdateLoan(loan);
            await _loanRepository.SaveAsync();
            return loan;
        }

        private void CheckLoanForPut(LoanDTOForPut loanDTOForPut)
        {
            var loan = _loanRepository.GetLoan(loanDTOForPut.Id);
            if (loan == null)
            {
                throw new Exception("Loan not found");
            }

            if (loanDTOForPut.LoanDate < DateOnly.FromDateTime(DateTime.Now))
            {
                throw new Exception("Loan date must be greater than or equal to today");
            }

            if (loanDTOForPut.LoanDate.AddDays(loanDTOForPut.LoanDuration) < loanDTOForPut.LoanDate)
            {
                throw new Exception("Return date must be greater than loan date");
            }
        }

        public Task<Loan> CancelLoan(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Loan> ConfirmLoan(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Loan> ReturnBook(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Loan> ConfirmReturn(int id)
        {
            throw new NotImplementedException();
        }
    }
}
