using ELib_IDSFintech_Internship.Models.Books;
using ELib_IDSFintech_Internship.Models.Users;

namespace ELib_IDSFintech_Internship.Repositories.Users
{
    public interface IUserRepository : IDefaultRepository<User>
    {
        //This signs in user and returns his data
        public Task<User?> VerifyUser(VerificationRequest verificationObject);

        //this lets user borrow a book
        public Task<int?> BorrowBook(int userId, Book entity);
    }
}
