
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyStatementApp.Data;
using MyStatementApp.Models;

namespace MyStatementApp.Services
{
    public class StatementService
    {
        private readonly ApplicationDbContext _context;

        public StatementService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Transaction>> GetTransactionsByAccountAndDateRangeAsync(int accountId, DateTime startDate, DateTime endDate)
        {
            return await _context.Transactions
                .Where(t => t.AccountId == accountId && t.Date >= startDate && t.Date <= endDate)
                .OrderBy(t => t.Date)
                .ToListAsync();
        }

        // Calculate the balance for an account over a date range
        public async Task<decimal> CalculateBalanceAsync(int accountId, DateTime startDate, DateTime endDate)
        {
            var transactions = await GetTransactionsByAccountAndDateRangeAsync(accountId, startDate, endDate);
            decimal balance = 0;

            foreach (var transaction in transactions)
            {
                balance += transaction.Credit - transaction.Debit;
            }

            return balance;
        }

        // Format the statement as a string
        public async Task<string> FormatStatementAsync(int accountId, DateTime startDate, DateTime endDate)
        {
            var transactions = await GetTransactionsByAccountAndDateRangeAsync(accountId, startDate, endDate);
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountId == accountId);
            if (account == null)
            {
                throw new Exception("Account not found");
            }

            var balance = await CalculateBalanceAsync(accountId, startDate, endDate);
            var statement = $"Statement for Account: {account.AccountNumber}\n";
            statement += $"Date Range: {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}\n\n";
            statement += "Date\tDescription\tDebit\tCredit\tBalance\n";

            foreach (var transaction in transactions)
            {
                statement += $"{transaction.Date:yyyy-MM-dd}\t{transaction.Description}\t{transaction.Debit}\t{transaction.Credit}\t{transaction.Balance}\n";
            }

            statement += $"\nTotal Balance: {balance}\n";
            return statement;
        }
    }
}
