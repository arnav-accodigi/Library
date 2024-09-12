using Library.Data.Domain;
using Library.Data.DTO;
using Library.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers;

[ApiController]
[Route("api/books")]
public class BookController : ControllerBase
{
    // since this field is set by the constructor it should be readonly
    private IBookService bookService;

    public BookController(IBookService bookService)
    {
        this.bookService = bookService;
    }

    [HttpGet]
    public IActionResult GetAllBooks()
    {
        // Separation of concerns,  the domain objects should not be exposed to this layer at all.
        // the DTO objects are intended to be the transfer mechanism between the api project and
        // the services projects
        return Ok(new ResponseDto { Data = bookService.GetAllBooks() });
    }

    [HttpGet("{id}")]
    public IActionResult GetBookById(Guid id)
    {
        var book = bookService.GetBookById(id);

        //
        // It is good you are throwing the not found exception. the issue is that the repository class should be 
        // throwing an exception.   Anytime you use this repository method, each consumer now has to check if it is 
        // null and throw an exception.   so now we have multiple 'if' conditions every time the repository GetById
        // method is called. 
        //
        if (book == null)
        {
            return NotFound(new ResponseDto { Message = $"Book with id {id} not found" });
        }

        return Ok(new ResponseDto { Data = book });
    }
    
    [HttpPost]
    public IActionResult AddBook([FromBody] BookDto bookDto)
    {
        // Violates Single class responsibility.  this is an API controller, it should delegate to a class 
        // for the conversion.
        // Is this where the conversion should occur?  (Separation of Concerns).
        // the domain object should NOT be exposed in the API layer,  this should be done in the service layer.
        // if I decide to reuse this for a Windows forms-based app or host as a Lambda or Fast EndPoints api
        // I have to rewrite this code there also 
        
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

    //
    [HttpGet("search/{query}")]
    public IActionResult Search(string query)
    {
        return Ok(new ResponseDto { Data = bookService.SearchBook(query) });
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteBook(Guid id)
    {
        //
        // again, in order to delete the book, you need to confirm it exists, which should be in the Repository
        //  if not found, it should throw an error
        //  REST standards
        //      - for a DELETE method that is successful a 200 (OK) response is the standard
        //      - Nocontent would be a graceful way to say "no data was found"            
        //
        bookService.DeleteBookById(id);
        return NoContent();
    }
}
