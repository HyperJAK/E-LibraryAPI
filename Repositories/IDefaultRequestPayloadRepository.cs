using ELib_IDSFintech_Internship.Models.Books.Authors;
using ELib_IDSFintech_Internship.Models.Tools;

namespace ELib_IDSFintech_Internship.Repositories
{
    public interface IDefaultRequestPayloadRepository<T>
    {
        public string SessionID { get; set; }
        public T? EntityObject { get; set; }
        public int? Id { get; set; }
    }
}
