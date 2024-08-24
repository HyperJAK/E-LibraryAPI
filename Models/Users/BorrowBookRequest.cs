namespace ELib_IDSFintech_Internship.Models.Users
{
    public class BorrowBookRequest
    {
        public required int UserId { get; set; }
        public required int BookId { get; set; }
        public string? SessionId { get; set; }
    }
}
