using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyStatementApp.Models;


namespace MyStatementApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Customer-Account relationship
            modelBuilder.Entity<Account>()
                .HasOne(a => a.User) // Account has one Customer
                .WithMany(c => c.Accounts) // Customer has many Accounts
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Account-Transaction relationship
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Account) // Transaction has one Account
                .WithMany(a => a.Transactions) // Account has many Transactions
                .HasForeignKey(t => t.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            // Additional configurations

            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.Username)
                .IsUnique();

            modelBuilder.Entity<Transaction>()
                .Property(t => t.Date)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}

