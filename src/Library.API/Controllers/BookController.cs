using Library.Data.Domain;
using Library.Data.DTO;
using Library.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers;

[ApiController]
[Route("api/[controller]")]
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
        return Ok(bookService.GetAllBooks());
    }

    [HttpGet("{id}")]
    public IActionResult GetBookById(Guid id)
    {
        return Ok(bookService.GetBookById(id));
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
                AvailableQuantity = bookDto.AvailableQuantity,
                TotalQuantity = bookDto.TotalQuantity
            };
        bookService.AddBook(book);
        return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, book);
    }

    [HttpGet("Search/{query}")]
    public IActionResult Search(string query)
    {
        return Ok(bookService.SearchBook(query));
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteBook(Guid id)
    {
        bookService.DeleteBookById(id);
        return NoContent();
    }
}
