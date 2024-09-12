using Library.Data.DTO;
using Library.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers;

[Route("api/overdue")]
[ApiController]
public class OverdueController : ControllerBase
{
    private IBookService bookService;
    
    // See BookController for my feedback
    
    public OverdueController(IBookService bookService)
    {
        this.bookService = bookService;
    }

    [HttpGet("books")]
    public IActionResult GetOverdueBooks()
    {
        var overdueBooks = bookService.GetOverdueBooks();
        return Ok(new ResponseDto { Data = overdueBooks });
    }

    [HttpGet("late-fee")]
    public IActionResult GetTotalLateFee()
    {
        var lateFee = bookService.GetTotalLateFee();
        return Ok(new ResponseDto { Data = lateFee });
    }

    [HttpGet("late-fee/{id}")]
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
