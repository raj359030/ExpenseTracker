using System.Text.RegularExpressions;
using ExpenseTracker.Models;
using Microsoft.Extensions.Caching.Memory;
namespace ExpenseTracker.Services;
public class BankParserService
{
    private readonly IMemoryCache _cache;
    
    public BankParserService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public List<Transaction> ParseHdfcPdf(string pdfText)
    {
        var transactions = new List<Transaction>();
        var pattern = @"(\d{2}/\d{2}/\d{4})\s+(.+?)\s+(\d+)\s+\d{2}/\d{2}/\d{4}\s+([\d,]+\.\d{2})\s+([\d,]+\.\d{2})";
        var matches = Regex.Matches(pdfText, pattern, RegexOptions.Singleline);

        foreach (Match m in matches)
        {
            transactions.Add(new Transaction
            {
                Date = DateTime.ParseExact(m.Groups[1].Value, "dd/MM/yyyy", null),
                Description = m.Groups[2].Value.Trim(),
                Amount = m.Groups[4].Value.Contains(",") ? 
                    decimal.Parse(m.Groups[4].Value.Replace(",", "")) :
                    decimal.Parse(m.Groups[4].Value),
                Type = decimal.Parse(m.Groups[4].Value) > 0 ? "Debit" : "Credit",
                Bank = "HDFC"
            });
        }
        
        _cache.Set("Transactions", transactions, TimeSpan.FromHours(1));
        return transactions;
    }
}