using Library.Data.Domain;

namespace Library.Services.Services;

public interface IBookService
{
    void AddBook(Book book);
    void DeleteBookById(Guid id);
    IEnumerable<Book> SearchBook(string query);
    IEnumerable<Book> GetAllBooks();
    // IEnumerable<Book> GetCheckedOutBooks();
    // void CheckoutBook(int id, DateTime dueDate);
    // void ReturnBook(int id);
    // decimal CalculateLateFees(int id);
}
