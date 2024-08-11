using LMS.Domain.DTOs.Librarian;
using LMS.Domain.Models;
namespace LMS.Domain.Mappers
{
    public static class LibrarianMappers
    {
        public static Librarian ToLibrarian(this LibrarianRegisterDTO LibrarianRegisterDTO)
        {
            return new Librarian
            {
                IdentityId = LibrarianRegisterDTO.IdentityId,
                UserName = LibrarianRegisterDTO.UserName,
                FirstName = LibrarianRegisterDTO.FirstName,
                LastName = LibrarianRegisterDTO.LastName,
                Address = LibrarianRegisterDTO.Address,
                Email = LibrarianRegisterDTO.Email,
                PhoneNumber = LibrarianRegisterDTO.PhoneNumber
            };
        }

        public static LibrarianDTO ToLibrarianDTO(this Librarian Librarian)
        {
            return new LibrarianDTO
            {
                Id = Librarian.Id,
                IdentityId = Librarian.IdentityId,
                Email = Librarian.Email,
                FirstName = Librarian.FirstName,
                LastName = Librarian.LastName,
                Address = Librarian.Address,
                PhoneNumber = Librarian.PhoneNumber
            };
        }
    }
}
