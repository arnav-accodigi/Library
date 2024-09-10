using Library.Data.Domain;

namespace Library.Data.Repositories;

public class BookRepository : IBookRepository
{
    private readonly List<Book> books =
    [
        new()
        {
            Title = "IT",
            Author = "Stephen King",
            IsCheckedOut = true,
            IssueDate = null,
            ReturnDate = DateTime.Now.AddDays(-30),
            Id = new Guid("71683dc2-7104-42ba-9af5-1485fa3e3f6a"),
            CreatedOn = DateTime.Now
        },
        new()
        {
            Title = "Run",
            Author = "Blake Crouch",
            IsCheckedOut = false,
            IssueDate = null,
            ReturnDate = null,
            Id = new Guid("b69613b5-9f07-42a8-a437-0b4e186fffbe"),
            CreatedOn = DateTime.Now
        },
        new()
        {
            Title = "Dark Matter",
            Author = "Blake Crouch",
            IsCheckedOut = true,
            IssueDate = null,
            ReturnDate = DateTime.Now.AddDays(-30),
            Id = new Guid("1f3bf517-8678-433d-ad8d-c5c10d7ea1fc"),
            CreatedOn = DateTime.Now
        },
        new()
        {
            Title = "Yellowface",
            Author = "RF Kuang",
            IsCheckedOut = false,
            IssueDate = null,
            ReturnDate = null,
            Id = new Guid("c2b89de4-c44a-4cf5-b547-542ca2394451"),
            CreatedOn = DateTime.Now
        }
    ];

    public Book Add(Book book)
    {
        books.Add(book);
        return book;
    }

    public void DeleteById(Guid id)
    {
        books.RemoveAll(b => b.Id == id);
    }

    public IEnumerable<Book> GetAll()
    {
        return books;
    }

    public IEnumerable<Book> Search(string query)
    {
        return books.Where(b => b.Title.Contains(query, StringComparison.OrdinalIgnoreCase));
    }

    public Book? GetById(Guid id)
    {
        return books.Find(item => item.Id == id);
    }

    public IEnumerable<Book> GetAllCheckedOut()
    {
        return books.Where(item => item.IsCheckedOut);
    }

    public IEnumerable<Book> GetAllAvailable()
    {
        return books.Where(item => item.IsCheckedOut);
    }
}
