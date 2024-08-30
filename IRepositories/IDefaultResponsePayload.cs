using ELib_IDSFintech_Internship.Services.Enums;

namespace ELib_IDSFintech_Internship.Repositories
{
    public interface IDefaultResponsePayload
    {
        public int Status { get; set; }
        public string? Message { get; set; }
    }
}
