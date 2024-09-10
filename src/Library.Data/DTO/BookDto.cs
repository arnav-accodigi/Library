using System.ComponentModel.DataAnnotations;

namespace Library.Data.DTO;

public class BookDto
{
    [Required]
    public string Title { get; set; }

    [Required]
    public string Author { get; set; }
}
