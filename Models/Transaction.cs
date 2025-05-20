namespace ExpenseTracker.Models;
public class Transaction
{
    public DateTime Date { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public string Type { get; set; } // Credit/Debit
    public string Bank { get; set; }
    public string Category { get; set; }
}