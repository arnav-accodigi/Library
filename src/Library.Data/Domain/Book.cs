namespace Library.Data.Domain;

public class Book : BaseRecord
{
    public string Title { get; set; }
    public string Author { get; set; }
    public bool IsCheckedOut { get; set; }
    public DateTime? IssueDate { get; set; }
    public DateTime? ReturnDate { get; set; }

    public void Checkout()
    {
        IssueDate = DateTime.Now;
        // Fixed lending period of 15 days
        ReturnDate = DateTime.Now.AddDays(15);
        IsCheckedOut = true;
    }

    public void Return()
    {
        IssueDate = null;
        ReturnDate = null;
        IsCheckedOut = false;
    }

    public bool IsOverdue()
    {
        if (IsCheckedOut && ReturnDate.HasValue)
        {
            var daysPassedSinceDueDate = (DateTime.Now - ReturnDate.Value).Days;
            return daysPassedSinceDueDate > 0;
        }
        return false;
    }

    public int CalculateLateFee()
    {
        if (!IsOverdue())
        {
            return 0;
        }

        var daysPassedSinceDueDate = (DateTime.Now - ReturnDate.Value).Days;

        // Fixed fine of 20 currency units per day
        return daysPassedSinceDueDate > 0 ? daysPassedSinceDueDate * 20 : 0;
    }
}
