namespace ELib_IDSFintech_Internship.Models.Users.Subscriptions
{
    public class SubscriptionActionRequest
    {
        public required int UserId { get; set; }
        public required int SubscriptionId { get; set; }
        public string? SessionId { get; set; }
    }
}
