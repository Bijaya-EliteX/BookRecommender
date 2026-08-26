using BookRecommender.Models;

namespace BookRecommender.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (!context.Genres.Any())
            {
                var genres = new List<Genre>
                {
                    new Genre { Name = "Fiction" },
                    new Genre { Name = "Fantasy" },
                    new Genre { Name = "Mystery" },
                    new Genre { Name = "Dystopian" },
                    new Genre { Name = "Romance" },
                    // Nepali-specific genres (replaced the single "Nepali Literature" bucket)
                    new Genre { Name = "निबन्ध" }, // essays / non-fiction
                    new Genre { Name = "कथा" }, // stories
                    new Genre { Name = "आख्यान" }, // novels / long narratives
                    new Genre { Name = "आत्मकथा" }, // autobiographies
                };
                context.Genres.AddRange(genres);
                await context.SaveChangesAsync();
            }

            if (!context.Authors.Any())
            {
                var authors = new List<Author>
                {
                    // modern international bestsellers
                    new Author { Name = "Rebecca Yarros" },
                    new Author { Name = "Colleen Hoover" },
                    new Author { Name = "Taylor Jenkins Reid" },
                    new Author { Name = "Alex Michaelides" },
                    new Author { Name = "Richard Osman" },
                    new Author { Name = "Freida McFadden" },
                    new Author { Name = "Andy Weir" },
                    new Author { Name = "Gabrielle Zevin" },
                    new Author { Name = "Bonnie Garmus" },
                    // modern Nepali authors
                    new Author { Name = "Narayan Wagle" }, // पल्पसा क्याफे
                    new Author { Name = "Buddhisagar Chapain" }, // कुसुम
                    new Author { Name = "Neelam Karki Niharika" }, // योगमाया
                    new Author { Name = "Bhupen Khaniya" }, // फिरफिरे, आँखी
                    new Author { Name = "Nayan Raj Pandey" }, // लू
                    new Author { Name = "Kumar Nagarkoti" }, // चापमा आफ्नो मान्छे
                    new Author { Name = "Karn Shakya" }, // सोच, अरू कस्ता छन्?
                    new Author { Name = "Bhupi Sherchan" }, // जन्मान्तर (आत्मकथा)
                };
                context.Authors.AddRange(authors);
                await context.SaveChangesAsync();
            }

            if (!context.Books.Any()) //is books table empty?
            {
                // ---- modern international authors ----
                var yarros = context.Authors.First(a => a.Name == "Rebecca Yarros");
                var hoover = context.Authors.First(a => a.Name == "Colleen Hoover");
                var reid = context.Authors.First(a => a.Name == "Taylor Jenkins Reid");
                var michaelides = context.Authors.First(a => a.Name == "Alex Michaelides");
                var osman = context.Authors.First(a => a.Name == "Richard Osman");
                var mcfadden = context.Authors.First(a => a.Name == "Freida McFadden");
                var weir = context.Authors.First(a => a.Name == "Andy Weir");
                var zevin = context.Authors.First(a => a.Name == "Gabrielle Zevin");
                var garmus = context.Authors.First(a => a.Name == "Bonnie Garmus");

                // ---- modern Nepali authors ----
                var wagle = context.Authors.First(a => a.Name == "Narayan Wagle"); // पल्पसा क्याफे
                var buddhisagar = context.Authors.First(a => a.Name == "Buddhisagar Chapain"); // कुसुम
                var neelam = context.Authors.First(a => a.Name == "Neelam Karki Niharika"); // योगमाया
                var khaniya = context.Authors.First(a => a.Name == "Bhupen Khaniya"); // फिरफिरे, आँखी
                var nayanraj = context.Authors.First(a => a.Name == "Nayan Raj Pandey"); // लू
                var nagarkoti = context.Authors.First(a => a.Name == "Kumar Nagarkoti"); // चापमा आफ्नो मान्छे
                var shakya = context.Authors.First(a => a.Name == "Karn Shakya"); // सोच, अरू कस्ता छन्?
                var bhupi = context.Authors.First(a => a.Name == "Bhupi Sherchan"); // जन्मान्तर

                var fiction = context.Genres.First(g => g.Name == "Fiction");
                var fantasy = context.Genres.First(g => g.Name == "Fantasy");
                var mystery = context.Genres.First(g => g.Name == "Mystery");
                var romance = context.Genres.First(g => g.Name == "Romance");
                // Nepali genres
                var nibandha = context.Genres.First(g => g.Name == "निबन्ध"); // essays
                var katha = context.Genres.First(g => g.Name == "कथा"); // stories
                var akhyana = context.Genres.First(g => g.Name == "आख्यान"); // novels/narratives
                var atmakatha = context.Genres.First(g => g.Name == "आत्मकथा"); // autobiographies

                var books = new List<Book>
                {
                    // ---- 10 modern international hits ----
                    new Book
                    {
                        Title = "Fourth Wing",
                        Description = "A dragon-rider fantasy that became a global phenomenon.",
                        AuthorId = yarros.Id,
                        BookGenres = new List<BookGenre> { new BookGenre { GenreId = fantasy.Id } },
                    },
                    new Book
                    {
                        Title = "Iron Flame",
                        Description = "The sequel to Fourth Wing — more dragons, more rebellion.",
                        AuthorId = yarros.Id,
                        BookGenres = new List<BookGenre> { new BookGenre { GenreId = fantasy.Id } },
                    },
                    new Book
                    {
                        Title = "It Ends With Us",
                        Description = "A emotional romance about breaking cycles of abuse.",
                        AuthorId = hoover.Id,
                        BookGenres = new List<BookGenre> { new BookGenre { GenreId = romance.Id } },
                    },
                    new Book
                    {
                        Title = "The Seven Husbands of Evelyn Hugo",
                        Description = "A reclusive Hollywood icon finally tells her true story.",
                        AuthorId = reid.Id,
                        BookGenres = new List<BookGenre>
                        {
                            new BookGenre { GenreId = romance.Id },
                            new BookGenre { GenreId = fiction.Id },
                        },
                    },
                    new Book
                    {
                        Title = "The Silent Patient",
                        Description = "A woman shoots her husband — and never speaks again.",
                        AuthorId = michaelides.Id,
                        BookGenres = new List<BookGenre> { new BookGenre { GenreId = mystery.Id } },
                    },
                    new Book
                    {
                        Title = "The Thursday Murder Club",
                        Description = "Four retirees in a village club solve real murders.",
                        AuthorId = osman.Id,
                        BookGenres = new List<BookGenre> { new BookGenre { GenreId = mystery.Id } },
                    },
                    new Book
                    {
                        Title = "The Housemaid",
                        Description = "A live-in housemaid discovers her employers' dark secrets.",
                        AuthorId = mcfadden.Id,
                        BookGenres = new List<BookGenre>
                        {
                            new BookGenre { GenreId = mystery.Id },
                            new BookGenre { GenreId = fiction.Id },
                        },
                    },
                    new Book
                    {
                        Title = "Project Hail Mary",
                        Description = "An astronaut wakes up alone on a spaceship and must save Earth.",
                        AuthorId = weir.Id,
                        BookGenres = new List<BookGenre> { new BookGenre { GenreId = fiction.Id } },
                    },
                    new Book
                    {
                        Title = "Tomorrow, and Tomorrow, and Tomorrow",
                        Description = "Two friends design video games — and navigate life together.",
                        AuthorId = zevin.Id,
                        BookGenres = new List<BookGenre> { new BookGenre { GenreId = fiction.Id } },
                    },
                    new Book
                    {
                        Title = "Lessons in Chemistry",
                        Description = "A chemist-turned-TV-cook challenges 1960s norms.",
                        AuthorId = garmus.Id,
                        BookGenres = new List<BookGenre> { new BookGenre { GenreId = fiction.Id } },
                    },
                    // ---- 10 modern Nepali hits (Title + Description in Nepali) ----
                    new Book
                    {
                        Title = "पल्पसा क्याफे",
                        Description = "नारायण वालीको द्वन्द्वकाललाई आधार बनाएको चर्चित उपन्यास।",
                        AuthorId = wagle.Id,
                        BookGenres = new List<BookGenre>
                        {
                            new BookGenre { GenreId = akhyana.Id },
                            new BookGenre { GenreId = fiction.Id },
                        },
                    },
                    new Book
                    {
                        Title = "कुसुम",
                        Description = "बुद्धिसागर चापाईंको मदन पुरस्कार विजेता उपन्यास।",
                        AuthorId = buddhisagar.Id,
                        BookGenres = new List<BookGenre>
                        {
                            new BookGenre { GenreId = akhyana.Id },
                            new BookGenre { GenreId = fiction.Id },
                        },
                    },
                    new Book
                    {
                        Title = "योगमाया",
                        Description = "नीलम कार्की निहारिकाको मदन पुरस्कार विजेता ऐतिहासिक उपन्यास।",
                        AuthorId = neelam.Id,
                        BookGenres = new List<BookGenre>
                        {
                            new BookGenre { GenreId = akhyana.Id },
                            new BookGenre { GenreId = fiction.Id },
                        },
                    },
                    new Book
                    {
                        Title = "फिरफिरे",
                        Description = "भूपेन खनिकरको चर्चित बेस्टसेलर उपन्यास।",
                        AuthorId = khaniya.Id,
                        BookGenres = new List<BookGenre>
                        {
                            new BookGenre { GenreId = katha.Id },
                            new BookGenre { GenreId = fiction.Id },
                        },
                    },
                    new Book
                    {
                        Title = "आँखी",
                        Description = "भूपेन खनिकरको पद्मश्री साहित्य पुरस्कार विजेता उपन्यास।",
                        AuthorId = khaniya.Id,
                        BookGenres = new List<BookGenre> { new BookGenre { GenreId = katha.Id } },
                    },
                    new Book
                    {
                        Title = "लू",
                        Description = "नयनराज पाण्डेको प्रसिद्ध उपन्यास।",
                        AuthorId = nayanraj.Id,
                        BookGenres = new List<BookGenre>
                        {
                            new BookGenre { GenreId = akhyana.Id },
                            new BookGenre { GenreId = fiction.Id },
                        },
                    },
                    new Book
                    {
                        Title = "चापमा आफ्नो मान्छे",
                        Description = "कुमार नागरकोटीको मदन पुरस्कार विजेता कृति।",
                        AuthorId = nagarkoti.Id,
                        BookGenres = new List<BookGenre> { new BookGenre { GenreId = akhyana.Id } },
                    },
                    new Book
                    {
                        Title = "सोच",
                        Description = "कर्ण शाक्यको चर्चित प्रेरणादायी निबन्ध सङ्ग्रह।",
                        AuthorId = shakya.Id,
                        BookGenres = new List<BookGenre> { new BookGenre { GenreId = nibandha.Id } },
                    },
                    new Book
                    {
                        Title = "अरू कस्ता छन्?",
                        Description = "कर्ण शाक्यको यात्रा-अनुभवमा आधारित निबन्ध कृति।",
                        AuthorId = shakya.Id,
                        BookGenres = new List<BookGenre> { new BookGenre { GenreId = nibandha.Id } },
                    },
                    new Book
                    {
                        Title = "जन्मान्तर",
                        Description = "कवि भूपि शेरचनको आत्मकथा।",
                        AuthorId = bhupi.Id,
                        BookGenres = new List<BookGenre> { new BookGenre { GenreId = atmakatha.Id } },
                    },
                };

                context.Books.AddRange(books);
                await context.SaveChangesAsync();
            }
        }
    }
}
