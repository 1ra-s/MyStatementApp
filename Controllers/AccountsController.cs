using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyStatementApp.Data;
using MyStatementApp.Models;
using MyStatementApp.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;


namespace MyStatementApp.Controllers
{
    public class AccountsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly PdfService _pdfService;
    private readonly IViewRenderService _viewRenderService;

    public AccountsController(ApplicationDbContext context, PdfService pdfService, IViewRenderService viewRenderService)
    {
        _context = context;
        _pdfService = pdfService;
        _viewRenderService = viewRenderService;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var accounts = await _context.Accounts
            .Where(a => a.UserId == userId)
            .ToListAsync();
        return View(accounts);
    }

    public async Task<IActionResult> Details(int id, DateTime startDate, DateTime endDate)
    {
        var account = await _context.Accounts
            .Include(a => a.Transactions)
            .FirstOrDefaultAsync(a => a.AccountId == id);

        if (account == null)
        {
            return NotFound();
        }

        var transactions = account.Transactions
            .Where(t => t.Date >= startDate && t.Date <= endDate)
            .ToList();

        var statement = new StatementViewModel
        {
            Account = account,
            Transactions = transactions
        };

        return View(statement);
    }

    public async Task<IActionResult> Download(int id, DateTime startDate, DateTime endDate, string format)
    {
        var account = await _context.Accounts
            .Include(a => a.Transactions)
            .FirstOrDefaultAsync(a => a.AccountId == id);

        if (account == null)
        {
            return NotFound();
        }

        var transactions = account.Transactions
            .Where(t => t.Date >= startDate && t.Date <= endDate)
            .ToList();

        var statement = new StatementViewModel
        {
            Account = account,
            Transactions = transactions
        };

        if (format == "pdf")
        {
            var html = await _viewRenderService.RenderToStringAsync("Details", statement);
            var pdf = _pdfService.GeneratePdf(html);
            return File(pdf, "application/pdf", $"Statement_{account.AccountNumber}_{DateTime.Now:yyyyMMdd}.pdf");
        }
        else if (format == "csv")
        {
            var csv = GenerateCsv(account, transactions);
            return File(new System.Text.UTF8Encoding().GetBytes(csv), "text/csv", $"Statement_{account.AccountNumber}_{DateTime.Now:yyyyMMdd}.csv");
        }

        return BadRequest("Invalid format");
    }

    private string GenerateCsv(Account account, List<Transaction> transactions)
    {
        var csv = "Date,Description,Debit,Credit,Balance\n";
        foreach (var transaction in transactions)
        {
            csv += $"{transaction.Date:yyyy-MM-dd},{transaction.Description},{transaction.Debit},{transaction.Credit},{transaction.Balance}\n";
        }
        return csv;
    }
}

}
