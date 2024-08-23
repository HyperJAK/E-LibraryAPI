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
            //Forein key creation for Author, key is in books
            modelBuilder.Entity<BookAuthor>()
            .HasMany(a => a.Books)
            .WithOne(b => b.Author)
            .HasForeignKey(b => b.AuthorId);

            //Forein key creation for Subscription, key is in users
            modelBuilder.Entity<Subscription>()
            .HasMany(a => a.Users)
            .WithOne(b => b.Subscription)
            .HasForeignKey(b => b.SubscriptionId);

            //linking credit card with user
            modelBuilder.Entity<User>()
            .HasOne(b => b.CreditCard)
            .WithOne(d => d.User)
            .HasForeignKey<User>(b => b.CreditCardId)
            .OnDelete(DeleteBehavior.Cascade);

            //join table creation for book and format
            modelBuilder.Entity<Book>()
            .HasMany(b => b.Formats)
            .WithMany(f => f.Books)
            .UsingEntity<Dictionary<string, object>>(
                "book_has_formats",
                j => j.HasOne<BookFormat>().WithMany().HasForeignKey("BookFormatId")
                .OnDelete(DeleteBehavior.Cascade),
                j => j.HasOne<Book>().WithMany().HasForeignKey("BookId")
            );

            //join table creation for book and genre
            modelBuilder.Entity<Book>()
            .HasMany(b => b.Genres)
            .WithMany(f => f.Books)
            .UsingEntity<Dictionary<string, object>>(
                "book_has_genres",
                j => j.HasOne<BookGenre>().WithMany().HasForeignKey("BookGenreId")
                .OnDelete(DeleteBehavior.Cascade),
                j => j.HasOne<Book>().WithMany().HasForeignKey("BookId")
            );

            //join table creation for book and tag
            modelBuilder.Entity<Book>()
            .HasMany(b => b.Tags)
            .WithMany(f => f.Books)
            .UsingEntity<Dictionary<string, object>>(
                "book_has_tags",
                j => j.HasOne<BookTag>().WithMany().HasForeignKey("BookTagId")
                .OnDelete(DeleteBehavior.Cascade),
                j => j.HasOne<Book>().WithMany().HasForeignKey("BookId")
            );

            //join table creation for book and language
            modelBuilder.Entity<Book>()
            .HasMany(b => b.Languages)
            .WithMany(f => f.Books)
            .UsingEntity<Dictionary<string, object>>(
                "book_has_languages",
                j => j.HasOne<Language>().WithMany().HasForeignKey("LanguageId")
                .OnDelete(DeleteBehavior.Cascade),
                j => j.HasOne<Book>().WithMany().HasForeignKey("BookId")
            );

            //Forein key creation for Location, key is in books
            modelBuilder.Entity<BookLocation>()
            .HasMany(a => a.Books)
            .WithOne(b => b.PhysicalBookLocation)
            .HasForeignKey(b => b.LocationId);
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
