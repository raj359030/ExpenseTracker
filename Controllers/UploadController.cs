using System.Text;
using ExpenseTracker.Services;

// Replace this using
// using iTextSharp.text.pdf;
// With:
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using Microsoft.AspNetCore.Mvc;

public class UploadController : Controller
{
    private readonly BankParserService _parser;
    private readonly CategoryService _categorizer;

    public UploadController(BankParserService parser, CategoryService categorizer)
    {
        _parser = parser;
        _categorizer = categorizer;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult UploadFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return Content("File not selected");

        if (System.IO.Path.GetExtension(file.FileName).ToLower() != ".pdf")
            return Content("Invalid file type");

       using (var pdfReader = new PdfReader(file.OpenReadStream()))
{
    var pdfDoc = new PdfDocument(pdfReader);
    var text = new StringBuilder();
    
    for (int page = 1; page <= pdfDoc.GetNumberOfPages(); page++)
    {
        var strategy = new SimpleTextExtractionStrategy();
        text.Append(PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(page), strategy));
    }
            var transactions = _parser.ParseHdfcPdf(text.ToString());
            transactions.ForEach(t => t.Category = _categorizer.DetectCategory(t.Description));
            
            return RedirectToAction("Index", "Dashboard");
        }
    }
}