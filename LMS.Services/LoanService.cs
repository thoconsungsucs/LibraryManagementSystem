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
        private readonly IEmailSender _emailSender;

        public LoanService(
            ILoanRepository loanRepository,
            IBookRepository bookRepository,
            IMemberRepository memberRepository,
            IEmailSender emailSender)
        {
            _loanRepository = loanRepository;
            _bookRepository = bookRepository;
            _memberRepository = memberRepository;
            _emailSender = emailSender;
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

        public async Task<Loan> AddLoan(LoanDTOForPost loanDTOForPost, bool isLibrarian = false)
        {
            try
            {
                await CheckLoanForPost(loanDTOForPost);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            var member = await _memberRepository.GetMemberInformation(loanDTOForPost.MemberId);
            var loan = loanDTOForPost.ToLoan();


            loan.Status = isLibrarian ? SD.Status_Borrowing : SD.Status_Loan_Pending;

            _loanRepository.AddLoan(loan);

            var book = await _bookRepository.GetBook(loanDTOForPost.BookId);
            book.Available = book.Available - 1;
            _bookRepository.UpdateBook(book);

            await _loanRepository.SaveAsync();

            await _emailSender.Send(new MailInformation
            {
                Name = member.FirstName + " " + member.LastName,
                Email = member.Email,
                Subject = "Loan request",
                Content = $"Your loan request for {book.Title} has been sent. \n" +
                    $"Loan Date: {loan.LoanDate}\n" +
                    $"Return Date: {loan.ReturnDate}\n" +
                    $" Please wait for librarian's confirmation"
            });
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

            var member = await _memberRepository.GetMemberInformation(loan.MemberId);
            var book = await _bookRepository.GetBook(loan.BookId);
            book.Available = book.Available + 1;
            _bookRepository.UpdateBook(book);
            await _loanRepository.SaveAsync();

            await _emailSender.Send(new MailInformation
            {
                Name = member.FirstName + " " + member.LastName,
                Email = member.Email,
                Subject = "Loan cancel",
                Content = $"Your loan request for {book.Title} has been cancelled"
            });
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

            var bookTitle = await _bookRepository.GetBookTitle(loan.BookId);
            var member = await _memberRepository.GetMemberInformation(loan.MemberId);
            await _emailSender.Send(new MailInformation
            {
                Name = member.FirstName + " " + member.LastName,
                Email = member.Email,
                Subject = "Loan update",
                Content = $"Your loan request for {bookTitle} has been updated. \n" +
                    $"Loan Date: {loan.LoanDate}\n" +
                    $"Return Date: {loan.ReturnDate}\n" +
                    $" Please wait for librarian's confirmation"
            });
            return loan;
        }

        public async Task<Loan> ConfirmLoan(LoanConfirmDTO loanConfirmDTO)
        {
            var loan = await _loanRepository.GetLoan(loanConfirmDTO.LoanId);
            if (loan == null)
            {
                throw new Exception("Loan not found");
            }
            if (loan.Status != SD.Status_Loan_Pending)
            {
                throw new Exception("Loan is not pending");
            }

            var bookTitle = await _bookRepository.GetBookTitle(loan.BookId);
            var member = await _memberRepository.GetMemberInformation(loan.MemberId);
            string content = "";
            if (!loanConfirmDTO.IsAccepted)
            {
                loan.Status = SD.Status_Rejected;
                content = $"Your loan request for {bookTitle} has been rejected";
            }
            else
            {
                content = $"Your loan request for {bookTitle} has been confrimed. Please go to library to get it.";
                loan.Status = SD.Status_Borrowing;
            }

            _loanRepository.UpdateLoan(loan);
            await _loanRepository.SaveAsync();

            await _emailSender.Send(new MailInformation
            {
                Name = member.FirstName + " " + member.LastName,
                Email = member.Email,
                Subject = "Loan confirmation",
                Content = content
            });

            return loan;
        }

        public async Task<Loan> ReturnBook(int id)
        {
            var loan = await _loanRepository.GetLoan(id);
            if (loan == null)
            {
                throw new Exception("Loan not found");
            }
            if (loan.Status != SD.Status_Borrowing && loan.Status != SD.Status_Renew_Pending)
            {
                throw new Exception("Return failed");
            }
            loan.Status = SD.Status_Return_Pending;
            var member = await _memberRepository.GetMemberInformation(loan.MemberId);

            _loanRepository.UpdateLoan(loan);
            await _loanRepository.SaveAsync();

            var bookTitle = await _bookRepository.GetBookTitle(loan.BookId);
            await _emailSender.Send(new MailInformation
            {
                Name = member.FirstName + " " + member.LastName,
                Email = member.Email,
                Subject = "Loan Returning",
                Content = $"Please go to library to return the {bookTitle}."
            });
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
            loan.ActualReturnDate = DateOnly.FromDateTime(DateTime.Now);
            _loanRepository.UpdateLoan(loan);
            //Tracking
            _bookRepository.GetBook(loan.BookId).Result.Available += 1;
            await _loanRepository.SaveAsync();

            var member = await _memberRepository.GetMemberInformation(loan.MemberId);
            await _emailSender.Send(new MailInformation
            {
                Name = member.FirstName + " " + member.LastName,
                Email = member.Email,
                Subject = "Loan Returned",
                Content = $"Your loan for {loan.Book.Title} has been returned. Thank you for using our service."
            });
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

        /*public async Task<Loan> ConfirmUpdate(int id)
        {
            var loan = await _loanRepository.GetLoan(id);

            if (loan == null)
            {
                throw new Exception("Loan not found");
            }

            if (loan.Status != SD.Status_Renew_Pending)
            {
                throw new Exception("Loan is not renew pending");
            }

            loan.Status = SD.Status_Borrowing;
            loan.ReturnDate = loan.ActualReturnDate.Value;
            loan.ActualReturnDate = null;
            _loanRepository.UpdateLoan(loan);
            await _loanRepository.SaveAsync();
            return loan;
        }*/

        public async Task<Loan> RenewLoan(int id, int days)
        {
            var loan = await _loanRepository.GetLoan(id);
            if (loan == null)
            {
                throw new Exception("Loan not found");
            }

            if (!SD.ValidRenewStatus.Contains(loan.Status) || loan.ActualReturnDate != null && loan.ReturnDate < DateOnly.FromDateTime(DateTime.Now))
            {
                throw new Exception("Cannot renew");
            }

            loan.Status = SD.Status_Renew_Pending;
            loan.RenewReturnDate = loan.ReturnDate.AddDays(days);
            _loanRepository.UpdateLoan(loan);
            await _loanRepository.SaveAsync();

            var member = await _memberRepository.GetMemberInformation(loan.MemberId);
            var bookTitle = await _bookRepository.GetBookTitle(loan.BookId);
            await _emailSender.Send(new MailInformation
            {
                Name = member.FirstName + " " + member.LastName,
                Email = member.Email,
                Subject = "Loan Renewing",
                Content = $"Your renewing request for {bookTitle} has been sent. Please wait for librarian's confirmation.\n" +
                $"Loan Return Date: {loan.RenewReturnDate}"
            });
            return loan;
        }

        public async Task<Loan> ConfirmRenew(LoanConfirmDTO loanConfirmDTO)
        {
            var loan = await _loanRepository.GetLoan(loanConfirmDTO.LoanId);
            var bookTitle = await _bookRepository.GetBookTitle(loan.BookId);
            string content = "";
            // Reject renew
            if (!loanConfirmDTO.IsAccepted)
            {
                loan.Status = SD.Status_Borrowing;
                loan.RenewReturnDate = null;
                content = $"Your renew request for {bookTitle} has been rejected\n" +
                    $"Return Date: {loan.ReturnDate}";
            }
            // Accept renew
            else
            {
                if (loan == null)
                {
                    throw new Exception("Loan not found");
                }
                if (loan.Status != SD.Status_Renew_Pending)
                {
                    throw new Exception("Loan is not renew pending");
                }
                loan.ReturnDate = loan.RenewReturnDate.Value;
                loan.RenewReturnDate = null;
                loan.Status = SD.Status_Borrowing;

                content = $"Your loan for {bookTitle} has been renewed.\n" +
                $"Loan Return Date: {loan.ReturnDate}";
            }
            _loanRepository.UpdateLoan(loan);
            await _loanRepository.SaveAsync();

            var member = await _memberRepository.GetMemberInformation(loan.MemberId);
            await _emailSender.Send(new MailInformation
            {
                Name = member.FirstName + " " + member.LastName,
                Email = member.Email,
                Subject = "Loan Renewing",
                Content = content
            });
            return loan;
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
