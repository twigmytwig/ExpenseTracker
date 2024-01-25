using ExpenseTracker.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.DAL
{
    public class ExpenseTrackerContext : IdentityDbContext
    {
        public ExpenseTrackerContext(DbContextOptions<ExpenseTrackerContext> options) :base(options)
        {

        }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountUsers> AccountUsers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}
