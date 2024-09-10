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
}
