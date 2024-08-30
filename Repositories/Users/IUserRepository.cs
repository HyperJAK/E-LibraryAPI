using ELib_IDSFintech_Internship.Models.Books;
using ELib_IDSFintech_Internship.Models.Books.RequestPayloads;
using ELib_IDSFintech_Internship.Models.Users;
using ELib_IDSFintech_Internship.Models.Users.RequestPayloads;
using ELib_IDSFintech_Internship.Models.Users.Subscriptions;
using ELib_IDSFintech_Internship.Services.Enums;

namespace ELib_IDSFintech_Internship.Repositories.Users
{
    public interface IUserRepository : IDefaultRepository<User>
    {
        //This clears the cached data in memory
        public Task<bool?> ClearCache(string key);

        //This signs in user and returns his data
        public Task<UserActionResponse?> VerifyUser(VerificationRequest verificationObject);

        //this lets user borrow a book
        public Task<BookActionResponse?> BorrowBook(BorrowBookRequest request);

        //this lets user unborrow a book
        public Task<BookActionResponse?> UnborrowBook(BorrowBookRequest request);

        //this lets user assign a subscription
        public Task<ResponseType?> AddSubscription(SubscriptionActionRequest request);

        public new Task<UserActionResponse?> Create(User newObject);

        public new Task<UserActionResponse?> Update(User modifiedObject);

        public new Task<UserActionResponse?> Delete(int id);

        public Task<UserActionResponse?> LogOut(UserActionRequest request);

    }
}
