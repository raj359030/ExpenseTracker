namespace ExpenseTracker.Services;
public class CategoryService
{
    public string DetectCategory(string description)
    {
        var categories = new Dictionary<string, List<string>> {
            {"Food", new List<string>{"SWIGGY", "ZOMATO", "RESTAURANT"}},
            {"Travel", new List<string>{"OLA", "UBER", "FUEL"}},
            {"Shopping", new List<string>{"AMAZON", "FLIPKART", "MYNTRA"}}
        };

        foreach (var category in categories)
        {
            if (category.Value.Any(keyword => 
                description.Contains(keyword, StringComparison.OrdinalIgnoreCase)))
                return category.Key;
        }
        return "Other";
    }
}