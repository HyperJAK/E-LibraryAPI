using ELib_IDSFintech_Internship.Models.Books.Enums;
using ELib_IDSFintech_Internship.Models.Books;
using ELib_IDSFintech_Internship.Models.Users;
using ELib_IDSFintech_Internship.Models.Users.Enums;
using ELib_IDSFintech_Internship.Models.Enums;

namespace ELib_IDSFintech_Internship.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ELibContext context)
        {
            // Check if the database has been seeded
            if (context.Books.Any()
                && context.Authors.Any()
                && context.Genres.Any()
                && context.BookLocations.Any()
                && context.Tags.Any()
                && context.Users.Any()
                && context.CreditCards.Any()
                && context.Subscriptions.Any())
            {
                return;   // DB has been seeded
            }

            // Create some authors
            var author1 = new BookAuthor
            {
                FirstName = "George",
                LastName = "Orwell",
                BirthDate = new DateOnly(1903, 6, 25),
                TimeStamp = DateTime.Now,
            };

            var author2 = new BookAuthor
            {
                FirstName = "J.K.",
                LastName = "Rowling",
                BirthDate = new DateOnly(1965, 7, 31),
                TimeStamp = DateTime.Now,
            };

            // Create some genres
            var genre1 = new BookGenre
            {
                Type = BookGenreType.Fantasy,
            };

            var genre2 = new BookGenre
            {
                Type = BookGenreType.Dystopian,
            };

            // Create some book locations
            var location1 = new BookLocation
            {
                Floor = 2,
                Shelf = 4,
                Section = 'B',
            };

            var location2 = new BookLocation
            {
                Floor = 1,
                Shelf = 1,
                Section = 'A',
            };

            // Create some book tags
            var tag1 = new BookTag
            {
                Type = BookTagType.YoungAdult,
            };

            var tag2 = new BookTag
            {
                Type = BookTagType.Adventure,
            };

            // Create some books
            var book1 = new Book
            {
                Title = "1984",
                PublishingDate = new DateOnly(1949, 6, 8),
                ISBN = "978-0451524935",
                Description = "A dystopian novel set in a totalitarian society ruled by Big Brother.",
                Type = BookType.Physical,
                PhysicalBookAvailability = true,
                PhysicalBookCount = 3,
                PhysicalBookLocation = location1,
                Publisher = "Secker & Warburg",
                PageCount = 328,
                Language = LanguageType.English,
                Author = author1,
                Genres = new List<BookGenre> { genre2 },
                Tags = new List<BookTag> { tag1, tag2 },
                TimeStamp = DateTime.Now,
            };

            var book2 = new Book
            {
                Title = "Harry Potter and the Philosopher's Stone",
                PublishingDate = new DateOnly(1997, 6, 26),
                ISBN = "978-0747532699",
                Description = "The first book in the Harry Potter series, introducing the world of magic and Hogwarts.",
                Type = BookType.Digital,
                PhysicalBookAvailability = false,
                DigitalBookURL = "http://example.com/harrypotter1",
                Format = BookFormatType.ePub,
                Publisher = "Bloomsbury",
                PageCount = 223,
                FileSizeInMB = 5,
                Language = LanguageType.English,
                Author = author2,
                Genres = new List<BookGenre> { genre1 },
                Tags = new List<BookTag> { tag1 },
                TimeStamp = DateTime.Now,
            };

            // Create some subscriptions
            var subscription1 = new Subscription
            {
                Type = SubscriptionType.Basic,
                Price = 9.99,
                DurationInDays = new DateOnly(2023, 8, 9).AddDays(30)
            };

            var subscription2 = new Subscription
            {
                Type = SubscriptionType.Premium,
                Price = 19.99,
                DurationInDays = new DateOnly(2023, 8, 9).AddDays(90)
            };

            // Create some credit cards
            var creditCard1 = new CreditCard
            {
                FullName = "John Doe",
                CardNumber = "1234-5678-9012-3456",
                BillingAddress = "1234 Elm Street, Springfield, USA",
                ExpirationDate = new DateOnly(2025, 12, 31),
                TimeStamp = DateTime.Now,
            };

            var creditCard2 = new CreditCard
            {
                FullName = "Jane Smith",
                CardNumber = "9876-5432-1098-7654",
                BillingAddress = "5678 Oak Avenue, Metropolis, USA",
                ExpirationDate = new DateOnly(2024, 6, 30),
                TimeStamp = DateTime.Now,
            };

            // Create some users and assign books
            var user1 = new User
            {
                Username = "johndoe",
                Email = "johndoe@example.com",
                Password = "hashedpassword123", // Replace with actual hashed password
                PhoneNumber = "555-1234",
                CreditCard = creditCard1,
                Subscription = subscription1,
                SubscriptionStartDate = new DateOnly(2023, 8, 9),
                SubscriptionEndDate = new DateOnly(2023, 9, 8),
                Books = new List<Book> { book1 },
                TimeStamp = DateTime.Now,
            };

            var user2 = new User
            {
                Username = "janesmith",
                Email = "janesmith@example.com",
                Password = "hashedpassword456", // Replace with actual hashed password
                PhoneNumber = "555-5678",
                CreditCard = creditCard2,
                Subscription = subscription2,
                SubscriptionStartDate = new DateOnly(2023, 8, 9),
                SubscriptionEndDate = new DateOnly(2023, 11, 7),
                Books = new List<Book> { book2 },
                TimeStamp = DateTime.Now,
            };

            // Add entities to context
            context.Authors.AddRange(author1, author2);
            context.Genres.AddRange(genre1, genre2);
            context.BookLocations.AddRange(location1, location2);
            context.Tags.AddRange(tag1, tag2);
            context.Books.AddRange(book1, book2);
            context.CreditCards.AddRange(creditCard1, creditCard2);
            context.Subscriptions.AddRange(subscription1, subscription2);
            context.Users.AddRange(user1, user2);

            // Save changes to database
            context.SaveChanges();
        }
    }
}
