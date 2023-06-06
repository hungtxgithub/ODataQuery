namespace ODataBookStore.EDM
{
    public class DataSource
    {
        private static IList<Book> listBooks { get; set; }
        public static IList<Book> GetBooks()
        {
            if (listBooks != null)
            {
                return listBooks;
            }
            listBooks = new List<Book>();
            Book book = new Book
            {
                Id = 1,
                ISBN = "978-0-321-87758-1",
                Title = "Essential C#5.0",
                Author = "Mark Michaelis",
                Price = 59.99m,
                Location = new Address
                {
                    City = "HCM City",
                    Street = "D1, Thu Duc District"
                },
                Press = new Press
                {
                    Id = 1,
                    Name = "Addison-Wesley",
                    Category = Category.Book
                }
            };
            listBooks.Add(book);
            book = new Book
            {
                Id = 2,
                ISBN = "978-0-321-87758-1",
                Title = "Essential C#6.0",
                Author = "Mark Michaelis",
                Price = 50.99m,
                Location = new Address
                {
                    City = "Ha Noi",
                    Street = "D1, Ha Dong District"
                },
                Press = new Press
                {
                    Id = 2,
                    Name = "Addison-BBB",
                    Category = Category.EBook
                }
            };
            listBooks.Add(book);
            book = new Book
            {
                Id = 3,
                ISBN = "978-0-321-87758-1",
                Title = "Java",
                Author = "DTS",
                Price = 50.99m,
                Location = new Address
                {
                    City = "Son La",
                    Street = "D1, Mai Son District"
                },
                Press = new Press
                {
                    Id = 3,
                    Name = "Addison-CCC",
                    Category = Category.Magazine
                }
            };
            listBooks.Add(book);
            return listBooks;
        }
    }
}
