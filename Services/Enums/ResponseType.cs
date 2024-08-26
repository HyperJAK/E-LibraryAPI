namespace ELib_IDSFintech_Internship.Services.Enums
{
    public enum ResponseType
    {
        NoObjectFound = (int)432,
        UserNotLoggedIn = (int)431,
        SubscriptionNeeded = (int)433,
        UserAlreadyBorrow = (int)434,
        ResponseSuccess = (int)200,
        UserAlreadySubscribed = (int)435,
        OutOfBook = (int)436,
        FailedRequiredDataValidation = (int)437,
        FailedToUpdate = (int)438,
        FailedToDelete = (int)439,
        FailedToCreate = (int)440,
        FailedToSignIn = (int)441,
        FailedToLogOut = (int)442

    }
}
