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

    [HttpPost]
    public IActionResult AddBook([FromBody] BookDto bookDto)
    {
        Book book =
            new()
            {
                id = Guid.NewGuid(),
                title = bookDto.Title,
                author = bookDto.Author,
                created_on = DateTime.Now,
                available_quantity = bookDto.AvailableQuantity,
                total_quantity = bookDto.TotalQuantity
            };
        bookService.AddBook(book);
        return Created();
    }
}
