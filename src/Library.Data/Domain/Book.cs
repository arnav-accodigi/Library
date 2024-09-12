namespace Library.Data.Domain;

public class Book : BaseRecord
{
    
    //Greg:  I like that the domain object contains the logic to control its internal state.
    
    //
    //  There should be unit tests for this class since it is more than just a few gets/sets/
    //
    
    public string Title { get; set; }
    public string Author { get; set; }
    public bool IsCheckedOut { get; set; }
    public DateTime? IssueDate { get; set; }
    public DateTime? ReturnDate { get; set; }

    public void Checkout()
    {
        //Greg:  no need for this comment line.  the code speaks for itself
        IssueDate = DateTime.Now;
        // Fixed lending period of 15 days
        ReturnDate = DateTime.Now.AddDays(15);
        IsCheckedOut = true;
    }

    public void Return()
    {
        // if (!IsOverdue)
        // {
        //     throw new Exception("There is an overdue fee on the book, please clear the dues");
        // }
        
        //Greg: should not ever have code that is commented out.  

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
