using Library.Data.Domain;
using Library.Data.Repositories;

namespace Library.Services.Services;

public class BookService : IBookService
{
    private readonly IBookRepository bookRepository;

    public BookService(IBookRepository bookRepository)
    {
        this.bookRepository = bookRepository;
    }

    public void AddBook(Book book)
    {
        bookRepository.Add(book);
    }

    public void DeleteBookById(Guid id)
    {
        bookRepository.DeleteById(id);
    }

    public IEnumerable<Book> GetAllBooks()
    {
        return bookRepository.GetAll();
    }

    public IEnumerable<Book> SearchBook(string query)
    {
        return bookRepository.Search(query);
    }

    public Book? GetBookById(Guid id)
    {
        return bookRepository.GetById(id);
    }

    public IEnumerable<Book> GetCheckedOutBooks()
    {
        return bookRepository.GetAll().Where(b => b.IsCheckedOut);
    }

    public IEnumerable<Book> GetAvailableBooks()
    {
        return bookRepository.GetAll().Where(b => !b.IsCheckedOut);
    }

    public IEnumerable<Book> GetOverdueBooks()
    {
        return bookRepository.GetAll().Where(b => b.IsOverdue());
    }

    public void CheckoutBook(Guid id)
    {
        var book =
            GetAvailableBooks().FirstOrDefault(b => b.Id == id)
            ?? throw new Exception("The requested book is not available");
        book.Checkout();
    }

    public void ReturnBook(Guid id)
    {
        var book =
            GetCheckedOutBooks().FirstOrDefault(b => b.Id == id)
            ?? throw new Exception("The requested book has not been checked out");
        book.Return();
    }

    public int GetTotalLateFee()
    {
        return GetCheckedOutBooks().Aggregate(0, (acc, x) => acc + x.CalculateLateFee());
    }

    public int GetLateFee(Guid id)
    {
        var book =
            GetCheckedOutBooks().FirstOrDefault(b => b.Id == id)
            ?? throw new Exception("The requested book has not been checked out");
        return book.CalculateLateFee();
    }
}
