using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

public class DashboardController : Controller
{
    private readonly IMemoryCache _cache;

    public DashboardController(IMemoryCache cache)
    {
        _cache = cache;
    }

    public IActionResult Index()
    {
        var transactions = _cache.Get<List<Transaction>>("Transactions") ?? new List<Transaction>();
        
        var model = new DashboardViewModel
        {
            Transactions = transactions,
            TotalIncome = transactions.Where(t => t.Type == "Credit").Sum(t => t.Amount),
            TotalExpense = transactions.Where(t => t.Type == "Debit").Sum(t => t.Amount),
            NetBalance = transactions.Sum(t => t.Type == "Credit" ? t.Amount : -t.Amount)
        };

        return View(model);
    }
}

public class DashboardViewModel
{
    public List<Transaction> Transactions { get; set; }
    public decimal TotalIncome { get; set; }
    public decimal TotalExpense { get; set; }
    public decimal NetBalance { get; set; }
}