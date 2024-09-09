using Library.Data.Domain;

namespace Library.Data.Repositories;

public class BookRepository : IBookRepository
{
    private readonly List<Book> books = [];

    public Book Add(Book book)
    {
        books.Add(book);
        return book;
    }

    public void DeleteById(Guid id)
    {
        books.RemoveAll(b => b.id == id);
    }

    public IEnumerable<Book> GetAll()
    {
        return books;
    }

    public IEnumerable<Book> Search(string query)
    {
        return books.Where(b => b.title.Contains(query, StringComparison.OrdinalIgnoreCase));
    }
}
