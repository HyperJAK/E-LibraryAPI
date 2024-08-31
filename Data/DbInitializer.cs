using ELib_IDSFintech_Internship.Models.Books;
using ELib_IDSFintech_Internship.Models.Books.Authors;
using ELib_IDSFintech_Internship.Models.Books.Formats;
using ELib_IDSFintech_Internship.Models.Common;
using ELib_IDSFintech_Internship.Models.Users;
using ELib_IDSFintech_Internship.Models.Users.RequestPayloads;

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
                new BookGenre { Type = "Science Fiction" },
                new BookGenre { Type = "Fantasy" },
                new BookGenre { Type = "Historical Fiction" },
                new BookGenre { Type = "Mystery" },
                new BookGenre { Type = "Thriller" },
                new BookGenre { Type = "Romance" },
                new BookGenre { Type = "Horror" },
                new BookGenre { Type = "Literary Fiction" },
                new BookGenre { Type = "Young Adult" },
                new BookGenre { Type = "Childrens Fiction" },
                new BookGenre { Type = "Dystopian" },
                new BookGenre { Type = "Magical Realism" },
                new BookGenre { Type = "Paranormal" },
                new BookGenre { Type = "Contemporary Fiction" },
                new BookGenre { Type = "Graphic Novels" },
                new BookGenre { Type = "Romantic Suspense" },
                new BookGenre { Type = "Historical Romance" },
                new BookGenre { Type = "Epic Fantasy" },
                new BookGenre { Type = "Urban Fantasy" },
                new BookGenre { Type = "Biography" },
                new BookGenre { Type = "Autobiography" },
                new BookGenre { Type = "Memoir" },
                new BookGenre { Type = "Self Help" },
                new BookGenre { Type = "True Crime" },
                new BookGenre { Type = "Travel" },
                new BookGenre { Type = "Cook books" },
                new BookGenre { Type = "Health And Wellness" },
                new BookGenre { Type = "Psychology" },
                new BookGenre { Type = "Religion And Spirituality" },
                new BookGenre { Type = "Philosophy" },
                new BookGenre { Type = "Politics" },
                new BookGenre { Type = "Science" },
                new BookGenre { Type = "History" },
                new BookGenre { Type = "Business And Economics" },
                new BookGenre { Type = "Education" },
                new BookGenre { Type = "Parenting" },
                new BookGenre { Type = "Science And Nature" },
                new BookGenre { Type = "Technology" },
                new BookGenre { Type = "Reference" },
                new BookGenre { Type = "Alternate History" },
                new BookGenre { Type = "Cozy Mystery" },
                new BookGenre { Type = "Cyberpunk" },
                new BookGenre { Type = "Steampunk" },
                new BookGenre { Type = "Literary Criticism" },
                new BookGenre { Type = "Sports" },
                new BookGenre { Type = "Military Fiction" },
                new BookGenre { Type = "Western" },
                new BookGenre { Type = "Supernatural" },
                new BookGenre { Type = "Medical Fiction" },
                new BookGenre { Type = "Classic Literature" },
                new BookGenre { Type = "Erotica" },
                new BookGenre { Type = "Gothic Fiction" },
                new BookGenre { Type = "Legal Thriller" },
                new BookGenre { Type = "Historical Mystery" },
                new BookGenre { Type = "Science Fantasy" },
                new BookGenre { Type = "High Fantasy" },
                new BookGenre { Type = "Sword And Sorcery" },
                new BookGenre { Type = "Mythology" },
                new BookGenre { Type = "Essays" }
            };

            var subscriptions = new List<Subscription>
            {
                new Subscription
                {
                    Type = "None",
                    Price = 0.00,
                    DurationInDays = new DateOnly(2023, 12, 31)
                },
                new Subscription
                {
                    Type = "Basic",
                    Price = 9.99,
                    DurationInDays = new DateOnly(2024, 12, 31)
                },
                new Subscription
                {
                    Type = "Advanced",
                    Price = 14.99,
                    DurationInDays = new DateOnly(2024, 12, 31)
                },
                new Subscription
                {
                    Type = "Premium",
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
                new BookTag { Type = "Young Adult" },
                new BookTag { Type = "Childrens" },
                new BookTag { Type = "Adult" },

                new BookTag { Type = "Mystery" },
                new BookTag { Type = "Thriller" },
                new BookTag { Type = "Romance" },
                new BookTag { Type = "Science Fiction" },
                new BookTag { Type = "Fantasy" },
                new BookTag { Type = "Historical" },
                new BookTag { Type = "Non Fiction" },
                new BookTag { Type = "Biography" },
                new BookTag { Type = "Memoir" },
                new BookTag { Type = "Self Help" },
                new BookTag { Type = "TrueCrime" },
                new BookTag { Type = "Adventure" },
                new BookTag { Type = "Horror" },
                new BookTag { Type = "Dystopian" },
                new BookTag { Type = "Paranormal" },
                new BookTag { Type = "Contemporary" },
                new BookTag { Type = "Classic" },
                new BookTag { Type = "Graphic Novel" },
                new BookTag { Type = "Short Stories" },
                new BookTag { Type = "Poetry" },

                new BookTag { Type = "Coming Of Age" },
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

                new BookTag { Type = "Historical Fiction" },
                new BookTag { Type = "Urban Fantasy" },
                new BookTag { Type = "Epic Fantasy" },
                new BookTag { Type = "Space Opera" },
                new BookTag { Type = "Cyberpunk" },
                new BookTag { Type = "Steampunk" },
                new BookTag { Type = "Cozy Mystery" },
                new BookTag { Type = "Legal Thriller" },
                new BookTag { Type = "Psychological Thriller" },
                new BookTag { Type = "Romantic Comedy" },
                new BookTag { Type = "Historical Romance" },
                new BookTag { Type = "Medical Fiction" },
                new BookTag { Type = "Supernatural" },
                new BookTag { Type = "Gothic" },
                new BookTag { Type = "Alternate History" },

                new BookTag { Type = "Feminism" },
                new BookTag { Type = "Social Justice" },
                new BookTag { Type = "Mental Health" },
                new BookTag { Type = "Environmental" },
                new BookTag { Type = "Inspirational" },
                new BookTag { Type = "Sports" },
                new BookTag { Type = "Education" },
                new BookTag { Type = "Business" },
                new BookTag { Type = "Economics" },
                new BookTag { Type = "Parenting" },
                new BookTag { Type = "Health And Wellness" },
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




            var books = new List<Book>
{
    new Book
            {
                Title = "1984",
                PublishingDate = new DateOnly(1949, 6, 8),
                ISBN = "978-0451524935",
                CoverImageURL = "https://s3.amazonaws.com/AKIAJC5RLADLUMVRPFDQ.book-thumb-images/ableson.jpg",
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
            },
    new Book
    {
        Title = "Unlocking Android",
        ISBN = "1933988673",
        PageCount = 416,
        PublishingDate = new DateOnly(2009, 4, 1),
        CoverImageURL = "https://s3.amazonaws.com/AKIAJC5RLADLUMVRPFDQ.book-thumb-images/ableson.jpg",
        Description = "Android is an open source mobile phone platform based on the Linux operating system and developed by the Open Handset Alliance...",
        Type = "Digital",
        Formats = new List<BookFormat> { bookFormats[0], bookFormats[1] },
        Publisher = "Bloomsbury",
        PhysicalBookAvailability = true,
        FileSizeInMB = 5,
        Author = new BookAuthor
        {
            FirstName = "Michel",
            LastName = "Sandres",
            BirthDate = new DateOnly(1951, 1, 1),
            TimeStamp = DateTime.Parse("2023-08-31T00:00:00Z")
        },
        Languages = new List<Language> { languages[0] },
        Tags = new List<BookTag> { tags[6] },
        Genres = new List<BookGenre> { genres[4], genres[5] },
        TimeStamp = DateTime.Now
    },
    new Book
    {
        Title = "Android in Action, Second Edition",
        ISBN = "1935182722",
        PageCount = 592,
        PublishingDate = new DateOnly(2011, 1, 14),
        CoverImageURL = "https://s3.amazonaws.com/AKIAJC5RLADLUMVRPFDQ.book-thumb-images/ableson2.jpg",
        Description = "When it comes to mobile apps, Android can do almost anything... Android in Action, Second Edition is a comprehensive tutorial...",
        Type = "Digital",
                Formats = new List<BookFormat> { bookFormats[0], bookFormats[1] },
        Publisher = "Bloomsbury",
        PhysicalBookAvailability = true,
        FileSizeInMB = 5,
        Author = new BookAuthor
        {
            FirstName = "Lorel",
            LastName = "Ipsum",
            BirthDate = new DateOnly(1951, 1, 1),
            TimeStamp = DateTime.Parse("2023-08-31T00:00:00Z")
        },
        Languages = new List<Language> { languages[0] },
        Tags = new List<BookTag> { tags[6] },
        Genres = new List<BookGenre> { genres[8], genres[6] },
        TimeStamp = DateTime.Now
    },
    new Book
    {
        Title = "Specification by Example",
        ISBN = "1617290084",
        PageCount = 0,
        PublishingDate = new DateOnly(2011, 6, 3),
        CoverImageURL = "https://s3.amazonaws.com/AKIAJC5RLADLUMVRPFDQ.book-thumb-images/adzic.jpg",
        Description = "Specification by Example is a hands-on guide to building software specifications through examples...",
        Type = "Digital",
                Formats = new List<BookFormat> { bookFormats[0], bookFormats[1] },
        Publisher = "Bloomsbury",
        FileSizeInMB = 5,
        PhysicalBookAvailability = true,
        Author = new BookAuthor
        {
            FirstName = "Random",
            LastName = "Guy",
            BirthDate = new DateOnly(1951, 1, 1),
            TimeStamp = DateTime.Parse("2023-08-31T00:00:00Z")
        },
        Languages = new List<Language> { languages[0] },
        Tags = new List<BookTag> { tags[6] },
        Genres = new List<BookGenre> { genres[3] },
        TimeStamp = DateTime.Now
    },
    new Book
    {
        Title = "Flex 3 in Action",
        ISBN = "1933988746",
        PageCount = 576,
        PublishingDate = new DateOnly(2009, 2, 2),
        CoverImageURL = "https://s3.amazonaws.com/AKIAJC5RLADLUMVRPFDQ.book-thumb-images/ahmed.jpg",
        Description = "New web applications require engaging user-friendly interfaces... Flex 3 in Action is an easy-to-follow, hands-on Flex tutorial...",
        Type = "Digital",
                Formats = new List<BookFormat> { bookFormats[0], bookFormats[1] },
        Publisher = "Bloomsbury",
        PhysicalBookAvailability = true,
        FileSizeInMB = 5,
        Author = new BookAuthor
        {
            FirstName = "Natasha",
            LastName = "Loretta",
            BirthDate = new DateOnly(1951, 1, 1),
            TimeStamp = DateTime.Parse("2023-08-31T00:00:00Z")
        },
        Languages = new List<Language> { languages[0] },
        Tags = new List<BookTag> { tags[6] },
        Genres = new List<BookGenre> { genres[7], genres[8], genres[9] },
        TimeStamp = DateTime.Now
    },
    new Book
    {
        Title = "Flex 4 in Action",
        ISBN = "1935182420",
        PageCount = 600,
        PublishingDate = new DateOnly(2010, 11, 15),
        CoverImageURL = "https://s3.amazonaws.com/AKIAJC5RLADLUMVRPFDQ.book-thumb-images/ahmed2.jpg",
        Description = "Using Flex, you can create high-quality, effective, and interactive Rich Internet Applications (RIAs) quickly and easily...",
        Type = "Digital",
                Formats = new List<BookFormat> { bookFormats[0], bookFormats[1] },
        Publisher = "Bloomsbury",
        FileSizeInMB = 5,
        PhysicalBookAvailability = true,
        Author = new BookAuthor
        {
            FirstName = "Sumo",
            LastName = "Lumo",
            BirthDate = new DateOnly(1951, 1, 1),
            TimeStamp = DateTime.Parse("2023-08-31T00:00:00Z")
        },
        Languages = new List<Language> { languages[0] },
        Tags = new List<BookTag> { tags[6] },
        Genres = new List<BookGenre> { genres[1], genres[0] },
        TimeStamp = DateTime.Now
    },
    new Book
    {
        Title = "Java 7 Concurrency Cookbook",
        ISBN = "1782161453",
        PageCount = 296,
        PublishingDate = new DateOnly(2013, 1, 23),
        CoverImageURL = "https://s3.amazonaws.com/AKIAJC5RLADLUMVRPFDQ.book-thumb-images/allen.jpg",
        Description = "Java 7 Concurrency Cookbook covers all the new features of Java 7 related to concurrency, threading, and parallelism...",
        Type = "Digital",
        PhysicalBookAvailability = true,
                Formats = new List<BookFormat> { bookFormats[0], bookFormats[1] },
        Publisher = "Bloomsbury",
        FileSizeInMB = 5,
        Author = new BookAuthor
        {
            FirstName = "Batista",
            LastName = "Lista",
            BirthDate = new DateOnly(1951, 1, 1),
            TimeStamp = DateTime.Parse("2023-08-31T00:00:00Z")
        },
        Languages = new List<Language> { languages[0] },
        Tags = new List<BookTag> { tags[6] },
        Genres = new List<BookGenre> { genres[1], genres[2], genres[3], genres[4] },
        TimeStamp = DateTime.Now
    }
};


            // Create some credit cards
            var creditCard1 = new CreditCard
            {
                FullName = "John Doe",
                CardNumber = "1234-5678-9012-3456",
                BillingAddress = "1234 Elm Street, Springfield, USA",
                ExpirationDate = new DateTime(2025, 12, 31),
                TimeStamp = DateTime.Now,
            };

            var creditCard2 = new CreditCard
            {
                FullName = "Jane Smith",
                CardNumber = "9876-5432-1098-7654",
                BillingAddress = "5678 Oak Avenue, Metropolis, USA",
                ExpirationDate = new DateTime(2024, 6, 30),
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
                UserBooks = new List<UserHasBooks>
                {
                    new UserHasBooks
                    {
                        Book = books[0],
                        BorrowedDate = DateTime.Now,
                        DueDate = DateTime.Now.AddDays(14)
                    },
                    new UserHasBooks
                    {
                        Book = books[1],
                        BorrowedDate = DateTime.Now,
                        DueDate = DateTime.Now.AddDays(14)
                    }
                },
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
                UserBooks = new List<UserHasBooks>
                {
                    new UserHasBooks
                    {
                        Book = books[3],
                        BorrowedDate = DateTime.Now,
                        DueDate = DateTime.Now.AddDays(14)
                    }
                },
                TimeStamp = DateTime.Now,
            };

            // Add entities to context
            context.Authors.AddRange(author1, author2);
            context.Genres.AddRange(genres);
            context.BookLocations.AddRange(location1, location2);
            context.Tags.AddRange(tags);
            context.BookFormats.AddRange(bookFormats);
            context.Books.AddRange(books);
            context.CreditCards.AddRange(creditCard1, creditCard2);
            context.Subscriptions.AddRange(subscriptions);
            context.Users.AddRange(user1, user2);
            context.Languages.AddRange(languages);

            // Save changes to database
            context.SaveChanges();
        }
    }
}
