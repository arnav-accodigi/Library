using Library.Data.Domain;
using Library.Data.Repositories;
using Library.Services.Services;
using Moq;

public class BookServiceTest
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

    public BookServiceTest()
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
        bookRepositoryMock.Setup(r => r.GetById(It.IsAny<Guid>())).Returns(initialBooks[3]);

        var book = bookService.GetBookById(It.IsAny<Guid>());

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
            Title = "Carrie",
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

        bookRepositoryMock.Verify(r => r.GetAll(), Times.Once);
        Assert.NotEqual(0, fee);
    }

    [Fact]
    public void CalculateLateFeeForId_BookNotOverdue()
    {
        var fee = bookService.GetLateFee(initialBooks[2].Id);

        bookRepositoryMock.Verify(r => r.GetAll(), Times.Once);
        Assert.Equal(0, fee);
    }

    [Fact]
    public void GetOverdueBooks_ShouldReturnOverdueBooks()
    {
        var overdueBooks = bookService.GetOverdueBooks();

        bookRepositoryMock.Verify(r => r.GetAll(), Times.Once);
        Assert.Single(overdueBooks);
        Assert.Equal("Stephen King", overdueBooks.ToList()[0].Author);
    }

    [Fact]
    public void GetCheckedoutBooks_ShouldReturnCheckedoutBooks()
    {
        var checkedOutBooks = bookService.GetCheckedOutBooks();

        bookRepositoryMock.Verify(r => r.GetAll(), Times.Once);
        Assert.Equal(2, checkedOutBooks.Count());
        Assert.Equal("IT", checkedOutBooks.ToList()[0].Title);
    }

    [Fact]
    public void GetAvailableBooks_ShouldReturnAvailableBooks()
    {
        var availableBooks = bookService.GetAvailableBooks();

        bookRepositoryMock.Verify(r => r.GetAll(), Times.Once);
        Assert.Equal(2, availableBooks.Count());
        Assert.Equal("Run", availableBooks.ToList()[0].Title);
    }

    [Fact]
    public void SearchBook_ShouldReturnSearchResults()
    {
        bookRepositoryMock.Setup(r => r.Search(It.IsAny<string>())).Returns([initialBooks[3]]);

        var searchResults = bookService.SearchBook("yel");

        bookRepositoryMock.Verify(r => r.Search(It.IsAny<string>()), Times.Once);
        Assert.Single(searchResults);
    }
}
