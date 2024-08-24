﻿using ELib_IDSFintech_Internship.Models.Books;
using ELib_IDSFintech_Internship.Models.Users;

namespace ELib_IDSFintech_Internship.Repositories.Users
{
    public interface IUserRepository : IDefaultRepository<User>
    {
        //This clears the cached data in memory
        public Task<bool?> ClearCache(string key);

        //This signs in user and returns his data
        public Task<User?> VerifyUser(VerificationRequest verificationObject);

        //this lets user borrow a book
        public Task<int?> BorrowBook(BorrowBookRequest request);
    }
}
