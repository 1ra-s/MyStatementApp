using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MyStatementApp.Models;

namespace MyStatementApp.Models
{
    public class Account
    {
        public Account()
        {
            Transactions = new HashSet<Transaction>();
            AccountName = string.Empty;
            AccountNumber = string.Empty;
            UserId = string.Empty;
            User = new Customer(); 
        }

        [Key]
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string UserId { get; set; }

        public virtual Customer User { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
