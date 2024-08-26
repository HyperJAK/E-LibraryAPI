using ELib_IDSFintech_Internship.Services.Enums;

namespace ELib_IDSFintech_Internship.Repositories
{
    public interface IDefaultResponsePayloadRepository
    {
        public int Status { get; set; }
        public string? Message { get; set; }
    }
}
