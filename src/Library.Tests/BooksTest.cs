using Library.Data.Domain;
using Library.Data.Repositories;
using Library.Services.Services;
using Moq;

public class BooksTest
{
    private readonly Mock<IBookRepository> bookRepositoryMock;
    private readonly IBookService bookService;
    private readonly List<Book> initialBooks =
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
            ReturnDate = DateTime.Now.AddDays(15),
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
        },
    ];

    public BooksTest()
    {
        bookRepositoryMock = new Mock<IBookRepository>();
        bookRepositoryMock.Setup(r => r.GetAll()).Returns(initialBooks);
        bookService = new BookService(bookRepositoryMock.Object);
    }

    [Fact]
    public void GetAllBooks_ShouldReturnAllBooks()
    {
        var books = bookService.GetAllBooks();

        Assert.NotEmpty(books);
        Assert.Equal(4, books.Count());
        Assert.Equal("Dark Matter", books.ToList()[2].Title);
    }

    [Fact]
    public void GetBookById_ShouldReturnTheBook()
    {
        var guid = new Guid("c2b89de4-c44a-4cf5-b547-542ca2394451");
        var bookWithId = new Book()
        {
            Title = "Yellowface",
            Author = "RF Kuang",
            IsCheckedOut = false,
            IssueDate = null,
            ReturnDate = null,
            Id = guid,
            CreatedOn = DateTime.Now
        };
        bookRepositoryMock.Setup(r => r.GetById(It.IsAny<Guid>())).Returns(bookWithId);

        var book = bookService.GetBookById(guid);

        Assert.NotNull(book);
        Assert.Equal("RF Kuang", book.Author);
        bookRepositoryMock.Verify(r => r.GetById(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public void AddBook_ShouldAddBook()
    {
        var book = new Book
        {
            Id = Guid.NewGuid(),
            Title = "IT",
            Author = "Stephen King",
            CreatedOn = DateTime.Now,
            IsCheckedOut = false
        };

        bookService.AddBook(book);

        bookRepositoryMock.Verify(repo => repo.Add(book), Times.Once);
    }

    [Fact]
    public void DeleteBook_ShouldDeleteBook()
    {
        var bookId = new Guid();

        bookService.DeleteBookById(bookId);

        bookRepositoryMock.Verify(repo => repo.DeleteById(bookId), Times.Once);
    }

    [Fact]
    public void CheckoutBook_BookIsAvailable_ShouldCheckout()
    {
        Book book = initialBooks[3];

        bookService.CheckoutBook(book.Id);

        bookRepositoryMock.Verify(r => r.GetAll(), Times.Once());
        Assert.True(book.IsCheckedOut);
    }

    [Fact]
    public void CheckoutBook_BookIsNotAvailable_ShouldNotCheckout()
    {
        Book book = initialBooks[0];
        Action action = () => bookService.CheckoutBook(book.Id);

        Exception exception = Assert.Throws<Exception>(action);

        Assert.Equal("The requested book is not available", exception.Message);
        bookRepositoryMock.Verify(r => r.GetAll(), Times.Once());
        Assert.True(book.IsCheckedOut);
    }

    [Fact]
    public void ReturnBook_BookIsAvailable_ShouldReturn()
    {
        Book book = initialBooks[0];

        bookService.ReturnBook(book.Id);

        bookRepositoryMock.Verify(r => r.GetAll(), Times.Once());
        Assert.False(book.IsCheckedOut);
    }

    [Fact]
    public void CalculateLateFee_ShouldReturnNonZero()
    {
        var fee = bookService.GetTotalLateFee();

        Assert.NotEqual(0, fee);
    }

    [Fact]
    public void CalculateLateFeeForId_BookNotOverdue()
    {
        var fee = bookService.GetLateFee(initialBooks[2].Id);

        Assert.Equal(0, fee);
    }
}
