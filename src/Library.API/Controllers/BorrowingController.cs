using Library.Data.DTO;
using Library.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers;

[Route("api/borrowing")]
[ApiController]
public class BorrowingController : ControllerBase
{
    private IBookService bookService;

    public BorrowingController(IBookService bookService)
    {
        this.bookService = bookService;
    }

    [HttpGet("checked-out-books")]
    public IActionResult GetCheckedOutBooks()
    {
        return Ok(new ResponseDto { Data = bookService.GetCheckedOutBooks() });
    }

    [HttpGet("available-books")]
    public IActionResult GetAvailableBooks()
    {
        return Ok(new ResponseDto { Data = bookService.GetAvailableBooks() });
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
}
