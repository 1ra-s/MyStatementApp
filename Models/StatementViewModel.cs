using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyStatementApp.Models
{
    public class StatementViewModel
    {
        [Required]
        public int AccountId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        
        // Make Account nullable
        public Account? Account { get; set; }

        public List<Transaction>? Transactions { get; set; } = new List<Transaction>();
    }
}
