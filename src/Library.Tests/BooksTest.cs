using Library.Data.Domain;
using Library.Data.Repositories;
using Library.Services.Services;
using Moq;

namespace Library.Tests;

public class BooksTest
{
    private IEnumerable<Book> GetListOfBooks()
    {
        return
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
        ];
    }

    [Fact]
    public void GetAllBooks_ReturnsListOfBooks()
    {
        var booksRepository = new Mock<IBookRepository>();
        booksRepository.Setup(r => r.GetAll()).Returns(GetListOfBooks());
        var booksService = new BookService(booksRepository.Object);

        var books = booksService.GetAllBooks();

        Assert.NotEmpty(books);
        Assert.Equal(2, books.Count());
    }
}
