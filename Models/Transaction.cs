using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyStatementApp.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        public string? Description { get; set; }

        public decimal Debit { get; set; }

        public decimal Credit { get; set; }

        public decimal Balance { get; set; }

        public int AccountId { get; set; }

        [ForeignKey(nameof(AccountId))]
        public virtual Account Account { get; set; } = new Account();
    }
}
