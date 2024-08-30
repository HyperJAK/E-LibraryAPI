namespace ELib_IDSFintech_Internship.Models.Users.RequestPayloads
{
    public class VerificationRequest
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
