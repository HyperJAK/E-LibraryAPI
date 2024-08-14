using ELib_IDSFintech_Internship.Models.Common;

namespace ELib_IDSFintech_Internship.Repositories.Common
{
    public interface ILanguageRepository : IDefaultRepository<Language>
    {
        //This clears the cached data in memory
        public Task<bool?> ClearCache();
    }
}
