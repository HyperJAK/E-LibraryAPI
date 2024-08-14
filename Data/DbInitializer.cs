using ELib_IDSFintech_Internship.Models.Books.Enums;
using ELib_IDSFintech_Internship.Models.Books;
using ELib_IDSFintech_Internship.Models.Users;
using ELib_IDSFintech_Internship.Models.Users.Enums;
using ELib_IDSFintech_Internship.Models.Enums;
using ELib_IDSFintech_Internship.Models.Common;

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
                && context.Subscriptions.Any()
                && context.BookFormats.Any())
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
            var genres = new List<BookGenre>
            {
                new BookGenre { Type = "Adventure" },
                new BookGenre { Type = "ScienceFiction" },
                new BookGenre { Type = "Fantasy" },
                new BookGenre { Type = "HistoricalFiction" },
                new BookGenre { Type = "Mystery" },
                new BookGenre { Type = "Thriller" },
                new BookGenre { Type = "Romance" },
                new BookGenre { Type = "Horror" },
                new BookGenre { Type = "LiteraryFiction" },
                new BookGenre { Type = "YoungAdult" },
                new BookGenre { Type = "ChildrensFiction" },
                new BookGenre { Type = "Dystopian" },
                new BookGenre { Type = "MagicalRealism" },
                new BookGenre { Type = "Paranormal" },
                new BookGenre { Type = "ContemporaryFiction" },
                new BookGenre { Type = "GraphicNovels" },
                new BookGenre { Type = "RomanticSuspense" },
                new BookGenre { Type = "HistoricalRomance" },
                new BookGenre { Type = "EpicFantasy" },
                new BookGenre { Type = "UrbanFantasy" },
                new BookGenre { Type = "Biography" },
                new BookGenre { Type = "Autobiography" },
                new BookGenre { Type = "Memoir" },
                new BookGenre { Type = "SelfHelp" },
                new BookGenre { Type = "TrueCrime" },
                new BookGenre { Type = "Travel" },
                new BookGenre { Type = "Cookbooks" },
                new BookGenre { Type = "HealthAndWellness" },
                new BookGenre { Type = "Psychology" },
                new BookGenre { Type = "ReligionAndSpirituality" },
                new BookGenre { Type = "Philosophy" },
                new BookGenre { Type = "Politics" },
                new BookGenre { Type = "Science" },
                new BookGenre { Type = "History" },
                new BookGenre { Type = "BusinessAndEconomics" },
                new BookGenre { Type = "Education" },
                new BookGenre { Type = "Parenting" },
                new BookGenre { Type = "ScienceAndNature" },
                new BookGenre { Type = "Technology" },
                new BookGenre { Type = "Reference" },
                new BookGenre { Type = "AlternateHistory" },
                new BookGenre { Type = "CozyMystery" },
                new BookGenre { Type = "Cyberpunk" },
                new BookGenre { Type = "Steampunk" },
                new BookGenre { Type = "LiteraryCriticism" },
                new BookGenre { Type = "Sports" },
                new BookGenre { Type = "MilitaryFiction" },
                new BookGenre { Type = "Western" },
                new BookGenre { Type = "Supernatural" },
                new BookGenre { Type = "MedicalFiction" },
                new BookGenre { Type = "ClassicLiterature" },
                new BookGenre { Type = "Erotica" },
                new BookGenre { Type = "GothicFiction" },
                new BookGenre { Type = "LegalThriller" },
                new BookGenre { Type = "HistoricalMystery" },
                new BookGenre { Type = "ScienceFantasy" },
                new BookGenre { Type = "HighFantasy" },
                new BookGenre { Type = "SwordAndSorcery" },
                new BookGenre { Type = "Mythology" },
                new BookGenre { Type = "Essays" }
            };

            var subscriptions = new List<Subscription>
            {
                new Subscription
                {
                    Type = SubscriptionType.None.ToString(),
                    Price = 0.00,
                    DurationInDays = new DateOnly(2023, 12, 31)
                },
                new Subscription
                {
                    Type = SubscriptionType.Basic.ToString(),
                    Price = 9.99,
                    DurationInDays = new DateOnly(2024, 12, 31)
                },
                new Subscription
                {
                    Type = SubscriptionType.Advanced.ToString(),
                    Price = 14.99,
                    DurationInDays = new DateOnly(2024, 12, 31)
                },
                new Subscription
                {
                    Type = SubscriptionType.Premium.ToString(),
                    Price = 19.99,
                    DurationInDays = new DateOnly(2024, 12, 31)
                }
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
            var tags = new List<BookTag>
            {
                new BookTag { Type = "YoungAdult" },
                new BookTag { Type = "Childrens" },
                new BookTag { Type = "Adult" },

                new BookTag { Type = "Mystery" },
                new BookTag { Type = "Thriller" },
                new BookTag { Type = "Romance" },
                new BookTag { Type = "ScienceFiction" },
                new BookTag { Type = "Fantasy" },
                new BookTag { Type = "Historical" },
                new BookTag { Type = "NonFiction" },
                new BookTag { Type = "Biography" },
                new BookTag { Type = "Memoir" },
                new BookTag { Type = "SelfHelp" },
                new BookTag { Type = "TrueCrime" },
                new BookTag { Type = "Adventure" },
                new BookTag { Type = "Horror" },
                new BookTag { Type = "Dystopian" },
                new BookTag { Type = "Paranormal" },
                new BookTag { Type = "Contemporary" },
                new BookTag { Type = "Classic" },
                new BookTag { Type = "GraphicNovel" },
                new BookTag { Type = "ShortStories" },
                new BookTag { Type = "Poetry" },

                new BookTag { Type = "ComingOfAge" },
                new BookTag { Type = "Family" },
                new BookTag { Type = "Friendship" },
                new BookTag { Type = "War" },
                new BookTag { Type = "Survival" },
                new BookTag { Type = "Magic" },
                new BookTag { Type = "Crime" },
                new BookTag { Type = "Politics" },
                new BookTag { Type = "Technology" },
                new BookTag { Type = "Religion" },
                new BookTag { Type = "Spirituality" },
                new BookTag { Type = "Philosophy" },
                new BookTag { Type = "Nature" },
                new BookTag { Type = "Science" },
                new BookTag { Type = "Travel" },
                new BookTag { Type = "Humor" },
                new BookTag { Type = "Satire" },

                new BookTag { Type = "HistoricalFiction" },
                new BookTag { Type = "UrbanFantasy" },
                new BookTag { Type = "EpicFantasy" },
                new BookTag { Type = "SpaceOpera" },
                new BookTag { Type = "Cyberpunk" },
                new BookTag { Type = "Steampunk" },
                new BookTag { Type = "CozyMystery" },
                new BookTag { Type = "LegalThriller" },
                new BookTag { Type = "PsychologicalThriller" },
                new BookTag { Type = "RomanticComedy" },
                new BookTag { Type = "HistoricalRomance" },
                new BookTag { Type = "MedicalFiction" },
                new BookTag { Type = "Supernatural" },
                new BookTag { Type = "Gothic" },
                new BookTag { Type = "AlternateHistory" },

                new BookTag { Type = "Feminism" },
                new BookTag { Type = "SocialJustice" },
                new BookTag { Type = "MentalHealth" },
                new BookTag { Type = "Environmental" },
                new BookTag { Type = "Inspirational" },
                new BookTag { Type = "Sports" },
                new BookTag { Type = "Education" },
                new BookTag { Type = "Business" },
                new BookTag { Type = "Economics" },
                new BookTag { Type = "Parenting" },
                new BookTag { Type = "HealthAndWellness" },
                new BookTag { Type = "Cooking" },
                new BookTag { Type = "Art" },
                new BookTag { Type = "Photography" }
            };

            var bookFormats = new List<BookFormat>
            {
                new BookFormat { Type = "ePub" },
                new BookFormat { Type = "MOBI" },
                new BookFormat { Type = "PDF" },
                new BookFormat { Type = "AZW" },
                new BookFormat { Type = "KFX" }
            };

            var languages = new List<Language>
            {
                new Language { Type = "English" },
                new Language { Type = "Spanish" },
                new Language { Type = "French" },
                new Language { Type = "German" },
                new Language { Type = "Italian" },
                new Language { Type = "Portuguese" },
                new Language { Type = "Dutch" },
                new Language { Type = "Russian" },
                new Language { Type = "Chinese" },
                new Language { Type = "Japanese" },
                new Language { Type = "Korean" },
                new Language { Type = "Arabic" },
                new Language { Type = "Turkish" },
                new Language { Type = "Hindi" },
                new Language { Type = "Bengali" },
                new Language { Type = "Urdu" },
                new Language { Type = "Vietnamese" },
                new Language { Type = "Thai" },
                new Language { Type = "Greek" },
                new Language { Type = "Hebrew" },
                new Language { Type = "Swedish" },
                new Language { Type = "Danish" },
                new Language { Type = "Norwegian" },
                new Language { Type = "Finnish" },
                new Language { Type = "Polish" }, 
                new Language { Type = "Czech" },
                new Language { Type = "Hungarian" },
                new Language { Type = "Lebanese" }
            };



            // Create some books
            var book1 = new Book
            {
                Title = "1984",
                PublishingDate = new DateOnly(1949, 6, 8),
                ISBN = "978-0451524935",
                Description = "A dystopian novel set in a totalitarian society ruled by Big Brother.",
                Type = "Physical",
                PhysicalBookAvailability = true,
                PhysicalBookCount = 3,
                PhysicalBookLocation = location1,
                Publisher = "Secker & Warburg",
                PageCount = 328,
                Languages = new List<Language> { languages[0] },
                Author = author1,
                Genres = new List<BookGenre> { genres[0], genres[5] },
                Tags = new List<BookTag> { tags[0], tags[2] },
                TimeStamp = DateTime.Now,
            };

            var book2 = new Book
            {
                Title = "Harry Potter and the Philosopher's Stone",
                PublishingDate = new DateOnly(1997, 6, 26),
                ISBN = "978-0747532699",
                Description = "The first book in the Harry Potter series, introducing the world of magic and Hogwarts.",
                Type = "Digital",
                PhysicalBookAvailability = false,
                DigitalBookURL = "http://example.com/harrypotter1",
                Format = new List<BookFormat> { bookFormats[0], bookFormats[1] },
                Publisher = "Bloomsbury",
                PageCount = 223,
                FileSizeInMB = 5,
                Languages = new List<Language> { languages[0] },
                Author = author2,
                Genres = new List<BookGenre> { genres[3] },
                Tags = new List<BookTag> { tags[6] },
                TimeStamp = DateTime.Now,
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
                Subscription = subscriptions[0],
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
                Subscription = subscriptions[1],
                SubscriptionStartDate = new DateOnly(2023, 8, 9),
                SubscriptionEndDate = new DateOnly(2023, 11, 7),
                Books = new List<Book> { book2 },
                TimeStamp = DateTime.Now,
            };

            // Add entities to context
            context.Authors.AddRange(author1, author2);
            context.Genres.AddRange(genres);
            context.BookLocations.AddRange(location1, location2);
            context.Tags.AddRange(tags);
            context.BookFormats.AddRange(bookFormats);
            context.Books.AddRange(book1, book2);
            context.CreditCards.AddRange(creditCard1, creditCard2);
            context.Subscriptions.AddRange(subscriptions);
            context.Users.AddRange(user1, user2);
            context.Languages.AddRange(languages);

            // Save changes to database
            context.SaveChanges();
        }
    }
}
