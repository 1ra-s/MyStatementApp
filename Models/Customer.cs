using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MyStatementApp.Models;


namespace MyStatementApp.Models
{
    public class Customer
    {
        public Customer()
        {
            Accounts = new HashSet<Account>();
        }

        [Key]
        public int CustomerId { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty; 

        public virtual ICollection<Account> Accounts { get; set; }
    }
}
