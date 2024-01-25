namespace ExpenseTracker.Models
{
    public class AccountUsers
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }
    }
}
