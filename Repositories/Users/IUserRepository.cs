﻿using ELib_IDSFintech_Internship.Models.Books;
using ELib_IDSFintech_Internship.Models.Users;
using ELib_IDSFintech_Internship.Services.Enums;

namespace ELib_IDSFintech_Internship.Repositories.Users
{
    public interface IUserRepository : IDefaultRepository<User>
    {
        //This clears the cached data in memory
        public Task<bool?> ClearCache(string key);

        //This signs in user and returns his data
        public Task<(User?, string)> VerifyUser(VerificationRequest verificationObject);

        //this lets user borrow a book
        public Task<ResponseType?> BorrowBook(BorrowBookRequest request);

        //this lets user assign a subscription
        public Task<ResponseType?> AddSubscription(AddSubscriptionRequest request);

        public new Task<(User?, string)> Create(User newObject);

    }
}
