using ELib_IDSFintech_Internship.Models.Books;
using ELib_IDSFintech_Internship.Models.Common;
using ELib_IDSFintech_Internship.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace ELib_IDSFintech_Internship.Data
{
    public class ELibContext : DbContext
    {

        // example of how to make a cache, where to put it, and how to handle checking it before calling DB:

        /*
         public async Task Delete(string key)
            {
                using var context = new SidekickContext(options);
                var cache = await context.Caches.FindAsync(key);
                if (cache != null)
                {
                    context.Caches.Remove(cache);
                    await context.SaveChangesAsync();
                }
            }
         */

        public ELibContext(DbContextOptions<ELibContext> options)
        : base(options)
        {
        }


        //not needed until now in testing
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Book> Books => Set<Book>();

        public DbSet<CreditCard> CreditCards => Set<CreditCard>();
        public DbSet<Subscription> Subscriptions => Set<Subscription>();

        public DbSet<BookGenre> Genres => Set<BookGenre>();
        public DbSet<BookTag> Tags => Set<BookTag>();
        public DbSet<BookFormat> BookFormats => Set<BookFormat>();

        public DbSet<BookAuthor> Authors => Set<BookAuthor>();
        public DbSet<BookLocation> BookLocations => Set<BookLocation>();

        public DbSet<Language> Languages => Set<Language>();

    }
}
