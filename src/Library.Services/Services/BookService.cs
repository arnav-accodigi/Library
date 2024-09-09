using System;
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
}
