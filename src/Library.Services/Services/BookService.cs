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
    
    // The Service classes are intended to act as the 'facade' into the services layer.  
    // it should be receiving the DTO, transforming it into domain objects as needed, and returning response Dtos 
    // as needed to the 'client', which in this case is your WebApi project

    public void AddBook(Book book)
    {
        //No where do I see any validation that the book state is valid.
        bookRepository.Add(book);
    }

    // if this were a real system, these would likely be calling a database and should support async/awaite
    // to be most efficient
    
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
