namespace Library.Data.Domain;

public class Book : BaseRecord
{
    public string title { get; set; }
    public string author { get; set; }
    public int total_quantity { get; set; }
    public int available_quantity { get; set; }
}
