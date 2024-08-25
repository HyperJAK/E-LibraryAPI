namespace ELib_IDSFintech_Internship.Models.Users
{
    public class AddSubscriptionRequest
    {
        public required int UserId { get; set; }
        public required int SubscriptionId { get; set; }
        public string? SessionId { get; set; }
    }
}
