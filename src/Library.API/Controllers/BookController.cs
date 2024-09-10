using Library.Data.Domain;
using Library.Data.DTO;
using Library.Services.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers;

[ApiController]
[Route("api/books")]
public class BookController : ControllerBase
{
    private IBookService bookService;

    public BookController(IBookService bookService)
    {
        this.bookService = bookService;
    }

    [HttpGet]
    public IActionResult GetAllBooks()
    {
        return Ok(new ResponseDto { Data = bookService.GetAllBooks() });
    }

    [HttpGet("{id}")]
    public IActionResult GetBookById(Guid id)
    {
        var book = bookService.GetBookById(id);

        if (book == null)
        {
            return NotFound(new ResponseDto { Message = $"Book with id {id} not found" });
        }

        return Ok(new ResponseDto { Data = book });
    }

    [HttpGet("checkedOut")]
    public IActionResult GetCheckedOutBooks()
    {
        return Ok(new ResponseDto { Data = bookService.GetCheckedOutBooks() });
    }

    [HttpGet("available")]
    public IActionResult GetAvailableBooks()
    {
        return Ok(new ResponseDto { Data = bookService.GetAvailableBooks() });
    }

    [HttpPost]
    public IActionResult AddBook([FromBody] BookDto bookDto)
    {
        Book book =
            new()
            {
                Id = Guid.NewGuid(),
                Title = bookDto.Title,
                Author = bookDto.Author,
                CreatedOn = DateTime.Now,
                IsCheckedOut = false
            };
        bookService.AddBook(book);
        return CreatedAtAction(
            nameof(GetBookById),
            new { id = book.Id },
            new ResponseDto { Data = book }
        );
    }

    [HttpGet("search/{query}")]
    public IActionResult Search(string query)
    {
        return Ok(new ResponseDto { Data = bookService.SearchBook(query) });
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteBook(Guid id)
    {
        bookService.DeleteBookById(id);
        return NoContent();
    }

    [HttpPost("checkout/{id}")]
    public IActionResult CheckoutBook(Guid id)
    {
        try
        {
            bookService.CheckoutBook(id);
            return Ok(new ResponseDto { Message = $"Successfully checked out book with id ${id}" });
        }
        catch (Exception e)
        {
            return NotFound(new ResponseDto { Message = e.Message });
        }
    }

    [HttpPost("return/{id}")]
    public IActionResult ReturnBook(Guid id)
    {
        try
        {
            bookService.ReturnBook(id);
            return Ok(new ResponseDto { Message = $"Successfully returned book with id ${id}" });
        }
        catch (Exception e)
        {
            return NotFound(new ResponseDto { Message = e.Message });
        }
    }

    [HttpGet("totalLateFee")]
    public IActionResult GetTotalLateFee()
    {
        var lateFee = bookService.GetTotalLateFee();
        return Ok(new ResponseDto { Data = lateFee });
    }

    [HttpGet("lateFee/{id}")]
    public IActionResult GetLateFee(Guid id)
    {
        try
        {
            var lateFee = bookService.GetLateFee(id);
            return Ok(new ResponseDto { Data = lateFee });
        }
        catch (Exception e)
        {
            return NotFound(new ResponseDto { Message = e.Message });
        }
    }
}
