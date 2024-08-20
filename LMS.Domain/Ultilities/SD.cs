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
            public static string Required = "Value is required";
            public static string PhoneNumberRegex = "Phone number just contains numbers and its length must be 10";
            public static string NameRegex = "Name just contains letters";
            public static string EmailRegex = "Email is invalid";
            public static string IdentityId = "Length must be 12";
            public static string PasswordRegex = "Password must contain at least one uppercase letter, one lowercase letter, one number and be between 12 and 15 characters long";
        }

    }
}
