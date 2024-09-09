using System;
using Library.Data.Domain;

namespace Library.Data.Repositories;

public interface IBookRepository
{
    IEnumerable<Book> GetAll();
    IEnumerable<Book> Search(string query);
    Book Add(Book item);
    void DeleteById(Guid id);
}
