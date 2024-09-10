using Library.Data.Domain;

namespace Library.Services.Services;

public interface IBookService
{
    void AddBook(Book book);
    void DeleteBookById(Guid id);
    IEnumerable<Book> SearchBook(string query);
    IEnumerable<Book> GetAllBooks();
    IEnumerable<Book> GetCheckedOutBooks();
    public IEnumerable<Book> GetAvailableBooks();
    Book? GetBookById(Guid id);
    void CheckoutBook(Guid id);
    void ReturnBook(Guid id);
    int GetTotalLateFee();
    int GetLateFee(Guid id);
}
