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
            Member member = null;
            IQueryable<Book> books;


            if (loanFilter.LoanDate != DateOnly.MinValue)
            {
                loans = loans.Where(l => l.LoanDate == loanFilter.LoanDate);
            }

            if (loanFilter.ReturnDate != DateOnly.MinValue)
            {
                loans = loans.Where(l => l.ReturnDate == loanFilter.ReturnDate);
            }

            if (loanFilter.MemberId != 0 && loanFilter.MemberId != null)
            {
                loans = loans.Where(l => l.MemberId == loanFilter.MemberId);
                member = await _memberRepository.GetMember(loanFilter.MemberId);
                if (member == null)
                {
                    throw new Exception("Member not found");
                }
            }

            if (!String.IsNullOrEmpty(loanFilter.Status))
            {
                loans = loans.Where(l => l.Status == loanFilter.Status);
            }

            if (!String.IsNullOrEmpty(loanFilter.BookTitle))
            {
                books = _bookRepository.GetAllBooks().Where(b => b.Title.Contains(loanFilter.BookTitle));
            }
            else
            {
                books = _bookRepository.GetAllBooks();
            }

            List<LoanDTO> loanDTOs = await books.Join(loans, b => b.Id, l => l.BookId, (b, l) => new LoanDTO
            {
                Id = l.Id,
                MemberId = l.MemberId,
                MemberName = member != null ? $"{member.FirstName} {member.LastName}" : $"{l.Member.FirstName} {l.Member.LastName}",
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

        public async Task<bool> CanBorrow(int id)
        {
            var outDateLoanNumber = await _loanRepository.GetAllLoans()
                .Where(l => l.MemberId == id && (l.Status == SD.Status_Borrowing) && l.ActualReturnDate != null && l.ReturnDate < DateOnly.FromDateTime(DateTime.Now))
                .CountAsync();
            return outDateLoanNumber < 1;
        }

        private async Task CheckLoanForPost(LoanDTOForPost loanDTOForPost)
        {
            var book = await _bookRepository.GetBook(loanDTOForPost.BookId);
            if (book == null)
            {
                throw new Exception("Book not found");
            }

            if (book.Available == 0)
            {
                throw new Exception("Book is not available");
            }
        }

        public async Task<Loan> AddLoan(LoanDTOForPost loanDTOForPost)
        {
            try
            {
                await CheckLoanForPost(loanDTOForPost);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            var loan = loanDTOForPost.ToLoan();
            loan.Status = SD.Status_Loan_Pending;
            _loanRepository.AddLoan(loan);

            var book = await _bookRepository.GetBook(loanDTOForPost.BookId);
            book.Available = book.Available - 1;
            _bookRepository.UpdateBook(book);

            await _loanRepository.SaveAsync();
            return loan;
        }

        public async Task<Loan> CancelLoan(int id)
        {
            var loan = await _loanRepository.GetLoan(id);
            if (loan == null)
            {
                throw new Exception("Loan not found");
            }
            if (loan.Status != SD.Status_Loan_Pending)
            {
                throw new Exception("Cannot cancel");
            }
            loan.Status = SD.Status_Cancelled;
            _loanRepository.UpdateLoan(loan);
            await _loanRepository.SaveAsync();
            return loan;
        }

        public async Task<Loan> UpdateLoan(LoanDTOForPut loanDTOForPut)
        {
            var loan = await _loanRepository.GetLoan(loanDTOForPut.Id);

            if (loan == null)
            {
                throw new Exception("Loan not found");
            }

            if (loan.Status != SD.Status_Loan_Pending)
            {
                throw new Exception("Cannot update");
            }
            loan.ReturnDate = loan.LoanDate.AddDays(loanDTOForPut.LoanDuration);

            _loanRepository.UpdateLoan(loan);
            await _loanRepository.SaveAsync();
            return loan;
        }

        public async Task<Loan> ConfirmLoan(int id)
        {
            var loan = await _loanRepository.GetLoan(id);
            if (loan == null)
            {
                throw new Exception("Loan not found");
            }
            if (loan.Status != SD.Status_Loan_Pending)
            {
                throw new Exception("Loan is not pending");
            }
            loan.Status = SD.Status_Borrowing;
            _loanRepository.UpdateLoan(loan);
            await _loanRepository.SaveAsync();
            return loan;
        }

        public async Task<Loan> ReturnBook(int id)
        {
            var loan = await _loanRepository.GetLoan(id);
            if (loan == null)
            {
                throw new Exception("Loan not found");
            }
            if (loan.Status != SD.Status_Borrowing || loan.Status != SD.Status_Update_Pending)
            {
                throw new Exception("Return failed");
            }
            loan.Status = SD.Status_Return_Pending;
            _loanRepository.UpdateLoan(loan);
            await _loanRepository.SaveAsync();
            return loan;
        }

        public async Task<Loan> ConfirmReturn(int id)
        {
            var loan = await _loanRepository.GetLoan(id);
            if (loan == null)
            {
                throw new Exception("Loan not found");
            }
            if (loan.Status != SD.Status_Return_Pending)
            {
                throw new Exception("Loan is not return pending");
            }
            loan.Status = SD.Status_Returned;
            _loanRepository.UpdateLoan(loan);
            //Tracking
            _bookRepository.GetBook(loan.BookId).Result.Available += 1;
            await _loanRepository.SaveAsync();
            return loan;
        }

        /*private async Task CheckRenewLoan(Loan loan, int days)
        {
            if (loan == null)
            {
                throw new Exception("Loan not found");
            }

            if (!SD.ValidUpdateStatus.Contains(loan.Status) || loan.ActualReturnDate != null && loan.ReturnDate < DateOnly.FromDateTime(DateTime.Now))
            {
                throw new Exception("Cannot update");
            }

            if (loanDTOForPut.LoanDate < DateOnly.FromDateTime(DateTime.Now))
            {
                throw new Exception("Loan date must be greater than or equal to today");
            }

            if (loanDTOForPut.LoanDate.AddDays(loanDTOForPut.LoanDuration) < loanDTOForPut.LoanDate)
            {
                throw new Exception("Return date must be greater than loan date");
            }
        }*/

        public async Task<Loan> ConfirmUpdate(int id)
        {
            var loan = await _loanRepository.GetLoan(id);

            if (loan == null)
            {
                throw new Exception("Loan not found");
            }

            if (loan.Status != SD.Status_Update_Pending)
            {
                throw new Exception("Loan is not update pending");
            }

            loan.Status = SD.Status_Borrowing;
            loan.ReturnDate = loan.ActualReturnDate.Value;
            loan.ActualReturnDate = null;
            _loanRepository.UpdateLoan(loan);
            await _loanRepository.SaveAsync();
            return loan;
        }

        public Task<Loan> RenewLoan(int id, int days)
        {
            throw new NotImplementedException();
        }

        public Task<Loan> ConfirmRenew(int id)
        {
            throw new NotImplementedException();
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
    }
}
