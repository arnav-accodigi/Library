namespace Library.Data.Domain;

public class Book : BaseRecord
{
    public string Title { get; set; }
    public string Author { get; set; }
    public int TotalQuantity { get; set; }
    public int AvailableQuantity { get; set; }
}
