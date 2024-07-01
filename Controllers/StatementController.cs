using Microsoft.AspNetCore.Mvc;
using MyStatementApp.Services;
using System;
using System.Threading.Tasks;
using MyStatementApp.Models;


namespace MyStatementApp.Controllers
{
public class StatementController : Controller
{
    private readonly StatementService _statementService;
    private readonly PdfService _pdfService;
    private readonly IViewRenderService _viewRenderService;

    public StatementController(StatementService statementService, PdfService pdfService, IViewRenderService viewRenderService)
    {
        _statementService = statementService;
        _pdfService = pdfService;
        _viewRenderService = viewRenderService;
    }

    public async Task<IActionResult> GenerateStatement(int accountId, DateTime startDate, DateTime endDate)
    {
        var transactions = await _statementService.GetTransactionsByAccountAndDateRangeAsync(accountId, startDate, endDate);
        return View(transactions);
    }

    public async Task<IActionResult> DownloadPdf(int accountId, DateTime startDate, DateTime endDate)
    {
        var transactions = await _statementService.GetTransactionsByAccountAndDateRangeAsync(accountId, startDate, endDate);
        var html = await _viewRenderService.RenderToStringAsync("GenerateStatement", transactions);
        var pdf = _pdfService.GeneratePdf(html);
        return File(pdf, "application/pdf", $"Statement_{accountId}_{DateTime.Now:yyyyMMdd}.pdf");
    }

    public async Task<IActionResult> DownloadCsv(int accountId, DateTime startDate, DateTime endDate)
    {
        var transactions = await _statementService.GetTransactionsByAccountAndDateRangeAsync(accountId, startDate, endDate);
        var csv = GenerateCsv(transactions);
        return File(new System.Text.UTF8Encoding().GetBytes(csv), "text/csv", $"Statement_{accountId}_{DateTime.Now:yyyyMMdd}.csv");
    }

    private string GenerateCsv(IEnumerable<Transaction> transactions)
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


