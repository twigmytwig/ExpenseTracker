namespace ExpenseTracker.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public float Amount { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }
        public DateOnly Date { get; set; }
        public bool isReoccuring { get; set; }
        public string? Notes { get; set; }
        public int? AccountUsersId { get; set; }
        public AccountUsers AccountUsers { get; set; }
    }
}
