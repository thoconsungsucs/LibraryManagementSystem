namespace LMS.Domain.Ultilities
{
    public static class SD
    {
        public const string Role_Admin = "Admin";
        public const string Role_Librarian = "Librarian";
        public const string Role_Member = "Member";

        public const string Status_Loan_Pending = "LoanPending";
        public const string Status_Borrowing = "Borrowing";
        public const string Status_Cancelled = "Cancelled";
        public const string Status_Returned = "Returned";
        public const string Status_Return_Pending = "ReturnPending";
        public const string Status_Rejected = "Rejected";
        public const string Status_Renew_Pending = "RenewPending";

        public static readonly List<string> ValidRenewStatus = new List<string> { SD.Status_Borrowing, SD.Status_Renew_Pending };

        public static class ValidationMessage
        {
            public const string Required = "Value is required";
            public const string NegativeNumber = "Value must be greater than 0";

            public static class UserMessage
            {
                public const string NotFound = "User not found";
                public const string PhoneNumberRegex = "Phone number just contains numbers, starts with 0 and its length must be 10";
                public const string NameRegex = "Name just contains letters";
                public const string EmailRegex = "Email is invalid";
                public const string IdentityId = "Length must be 12";
                public const string PasswordRegex = "Password must contain at least one uppercase letter, one lowercase letter, one number and be between 12 and 15 characters long";
            }

            public static class BookMessage
            {
                public const string NotFound = "Book not found";
                public const string NotAvailable = "Book is not available";
                public const string TitleLength = "Title must be between 3 and 50 characters";
                public const string AuthorLength = "Author must be between 3 and 50 characters";
                public const string CategoryLength = "Category must be between 3 and 50 characters";

            }

            public static class LoanMessage
            {
                public const string NotFound = "Loan not found";
                public const string LoanDateError = "Loan date must be greater or equal now";
                public const string DurationLength = "Duration must be greater than 0 and less than 30";
            }

        }

        public static class ErrorCode
        {
            public const string ValidationError = "ValidationError";
            public const string NotFound = "NotFound";
            public const string NullObject = "NullObject";
            public const string InsufficientStock = "InsufficientStock";
            public const string Unauthorized = "Unauthorized";
            public const string InternalServerError = "InternalServerError";
        }

    }
}
